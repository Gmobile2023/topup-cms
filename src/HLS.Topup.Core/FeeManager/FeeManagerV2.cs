using System;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Collections.Extensions;
using Abp.UI;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Common;
using HLS.Topup.Dtos;
using HLS.Topup.Dtos.BillFees;
using HLS.Topup.Dtos.Discounts;
using HLS.Topup.Dtos.Fees;
using HLS.Topup.Products;
using HLS.Topup.Services;
using Microsoft.Extensions.Logging;
using ServiceStack;
using Abp.Runtime.Caching;

namespace HLS.Topup.FeeManager
{
    /// <summary>
    /// A/E lưu ý phần này là các hàm api viết lấy chính sách cho đại lý dùng app, web. A/e lưu ý khi tác động mấy phần này. Dễ bị sai chính sách bán hàng
    /// </summary>
    public partial class FeeManager
    {
        public async Task<ProductFeeDto> GetProductFee(string productcode, string accountCode, decimal amount)
        {
            try
            {
                if (string.IsNullOrEmpty(accountCode))
                    throw new UserFriendlyException("User not found");
                var user = await GetUserCache(accountCode);
                if (user == null)
                    throw new UserFriendlyException("User not found");
                var fee = await _redisCache.GetEntity<ProductFeeDto>($"{ProductFeeDto.CacheKey}:{accountCode}:{productcode}");
                if (fee != null)
                {
                    _logger.LogInformation($"GetFee:{accountCode}-{productcode} from Cache => {fee.ToJson()}");
                }
                else
                {
                    fee = await GetProductFeeFromDb(productcode, accountCode, amount);
                    if (fee != null)
                    {
                        //Lấy ngày hết hạn cho cache bên core
                        var expireDate = fee.ToDate;
                        var getExpireDate = await GetForExpireDate(productcode, user);
                        if (getExpireDate != DateTime.MinValue && getExpireDate < expireDate)
                        {
                            expireDate = getExpireDate;
                        }

                        var expTime = (expireDate - DateTime.Now).TotalMinutes;
                        if (expTime > 0)
                        {
                            await _redisCache.AddEntity($"{ProductFeeDto.CacheKey}:{accountCode}:{productcode}",
                                fee, TimeSpan.FromMinutes(expTime));
                        }
                    }
                }

                if (fee == null)
                    fee = new ProductFeeDto
                    {
                        Amount = amount,
                        FeeValue=0,
                        TotalAmount = amount,
                        ProductCode=productcode
                    };
                var result = GetFreeAmount(fee);
                return result;
            }
            catch (Exception e)
            {
                _logger.LogError($"GetDiscount:{e}");
                throw new UserFriendlyException("Không thành công");
            }
        }
        private async Task<ProductFeeDto> GetProductFeeFromDb(string productcode, string accountCode, decimal amount)
        {
            try
            {
                if (string.IsNullOrEmpty(accountCode))
                    throw new UserFriendlyException("User not found");
                var user = await GetUserCache(accountCode);
                if (user == null)
                    throw new UserFriendlyException("User not found");
                var detail = await _feeDetailsRepository.GetAllIncluding(x => x.ProductFk)
                                 .Include(x => x.FeeFk).Include(x => x.FeeFk.UserFk)
                                 .Where(x => x.FeeFk.Status == CommonConst.FeeStatus.Approved &&
                                             x.ProductFk.Status == CommonConst.ProductStatus.Active &&
                                             x.FeeFk.UserFk.AccountCode == accountCode &&
                                             x.FeeFk.FromDate <= DateTime.Now &&
                                             x.FeeFk.ToDate >= DateTime.Now)
                                 .OrderByDescending(x => x.FeeFk.CreationTime)
                                 .FirstOrDefaultAsync(x => x.ProductFk.ProductCode == productcode) ??
                             await _feeDetailsRepository
                                 .GetAllIncluding(x => x.ProductFk)
                                 .Include(x => x.FeeFk)
                                 .Where(x => x.FeeFk.Status == CommonConst.FeeStatus.Approved &&
                                             x.ProductFk.Status == CommonConst.ProductStatus.Active &&
                                             x.FeeFk.AgentType == user.AgentType &&
                                             x.FeeFk.UserId == null &&
                                             x.FeeFk.AgentType != 0 &&
                                             x.FeeFk.FromDate <= DateTime.Now &&
                                             x.FeeFk.ToDate >= DateTime.Now)
                                 .OrderByDescending(x => x.FeeFk.CreationTime)
                                 .FirstOrDefaultAsync(x => x.ProductFk.ProductCode == productcode);

                if (detail == null)
                    return null;
                // return new ProductFeeDto
                // {
                //     ProductCode = product.ProductCode,
                //     ProductName = product.ProductName,
                //     Amount = amount,
                //     TotalAmount = amount
                // };
                return new ProductFeeDto
                {
                    ProductCode = detail.ProductFk.ProductCode,
                    MinFee = detail.MinFee,
                    SubFee = detail.SubFee,
                    ProductName = detail.ProductFk.ProductName,
                    FeeId = detail.FeeId,
                    FeeDetailId = detail.Id,
                    AmountIncrease = detail.AmountIncrease,
                    AmountMinFee = detail.AmountMinFee,
                    Amount = amount,
                    ToDate = detail.FeeFk.ToDate,
                };
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
        private Task<DateTime> GetForExpireDate(string productcode, User user)
        {
            var detail = _feeDetailsRepository.GetAllIncluding(x => x.FeeFk).Include(x => x.ProductFk)
                .Include(x => x.FeeFk.UserFk)
                .Where(x => x.FeeFk.Status == CommonConst.FeeStatus.Approved &&
                            x.ProductFk.Status == CommonConst.ProductStatus.Active &&
                            x.FeeFk.UserFk.AccountCode == user.AccountCode &&
                            x.FeeFk.FromDate > DateTime.Now)
                .OrderBy(x => x.FeeFk.FromDate).ThenByDescending(x => x.FeeFk.CreationTime)
                .FirstOrDefault(x => x.ProductFk.ProductCode == productcode) ?? _feeDetailsRepository
                .GetAllIncluding(x => x.FeeFk).Include(x => x.ProductFk)
                .Where(x => x.FeeFk.Status == CommonConst.FeeStatus.Approved &&
                            x.ProductFk.Status == CommonConst.ProductStatus.Active &&
                            x.FeeFk.AgentType == user.AgentType &&
                            x.FeeFk.UserId == null &&
                            x.FeeFk.AgentType != 0 &&
                            x.FeeFk.FromDate > DateTime.Now)
                .OrderBy(x => x.FeeFk.FromDate).ThenByDescending(x => x.FeeFk.CreationTime)
                .FirstOrDefault(x => x.ProductFk.ProductCode == productcode);
            return Task.FromResult(detail != null ? detail.FeeFk.FromDate : DateTime.MinValue);
        }
        private Task<User> GetUserCache(string accountCode)
        {
            try
            {
                return _cacheManager
                    .GetCache(CacheConst.Users)
                    .AsTyped<object, User>()
                    .GetAsync($"Items:{accountCode}",
                        async f => await _userManager.GetUserByAccountCodeAsync(accountCode));
            }
            catch (Exception e)
            {
                _logger.LogError($"GetUserCache:{e}");
                return null;
            }
        }
    }
}