using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Common;
using HLS.Topup.Dtos.Discounts;
using Microsoft.Extensions.Logging;
using Abp.UI;

namespace HLS.Topup.DiscountManager
{
    public partial class DiscountManger
    {

        public async Task<ProductDiscountDto> GeProductDiscountAccount(string productCode, string accountCode,
            decimal amount = 0, int total = 1)
        {
            try
            {
                if (string.IsNullOrEmpty(accountCode))
                    throw new UserFriendlyException("User not found");
                var user = await GetUserCache(accountCode);
                if (user == null)
                    throw new UserFriendlyException("User not found");

                var discount = await _redisCache.GetEntity<ProductDiscountDto>($"{ProductDiscountDto.CacheKey}:{accountCode}:{productCode}");
                if (discount != null)
                {
                    _logger.LogInformation($"GetDiscount:{accountCode}-{productCode} from Cache => {discount.DiscountValue}");
                }
                else
                {
                    discount = await GetDiscountFromDb(productCode, accountCode, amount, total);
                    if (discount is { IsSaveCache: true })
                    {
                        //Lấy ngày hết hạn cho cache bên core
                        var expireDate = discount.ToDate;
                        var getLastPending = await GetDiscountForExpireDate(user, productCode);
                        if (getLastPending != null && getLastPending.FromDate < expireDate)
                        {
                            _logger.LogInformation($"Cache to expire last item:{expireDate}");
                            expireDate = getLastPending.FromDate;
                            discount.NextDiscount = getLastPending.Code;
                            discount.ExpireTime = expireDate.ToString("dd/MM/yyyy hh:mm:ss");
                        }
                        else
                        {
                            discount.NextDiscount = "None";
                            discount.ExpireTime = expireDate.ToString("dd/MM/yyyy hh:mm:ss");
                        }

                        var expTime = (expireDate - DateTime.Now).TotalMinutes;
                        if (expTime > 0)
                        {
                            _logger.LogInformation($"Cache to expire total time:{expTime}");
                            await _redisCache.AddEntity($"{ProductDiscountDto.CacheKey}:{accountCode}:{productCode}", discount, TimeSpan.FromMinutes(expTime));
                        }
                    }
                }

                if (discount == null)
                    return null;
                var result = GetDiscountAmount(discount, amount, total);
                return result;
            }
            catch (Exception e)
            {
                _logger.LogError($"GetDiscount:{e}");
                return null;
            }
        }
        private Task<Discount> GetDiscountForExpireDateDB(User user, string productCode)
        {
            var detail = _discountDetailRepository.GetAllIncluding(x => x.DiscountFk).Include(x => x.ProductFk)
                .Include(x => x.DiscountFk.UserFk)
                .Where(x => x.DiscountFk.Status == CommonConst.DiscountStatus.Approved &&
                            x.ProductFk.Status == CommonConst.ProductStatus.Active &&
                            x.DiscountFk.UserFk.AccountCode == user.AccountCode &&
                            x.DiscountFk.FromDate > DateTime.Now)
                .OrderBy(x => x.DiscountFk.FromDate).ThenByDescending(x => x.DiscountFk.CreationTime)
                .FirstOrDefault(x => x.ProductFk.ProductCode == productCode) ?? _discountDetailRepository
                .GetAllIncluding(x => x.DiscountFk).Include(x => x.ProductFk)
                .Where(x => x.DiscountFk.Status == CommonConst.DiscountStatus.Approved &&
                            x.ProductFk.Status == CommonConst.ProductStatus.Active &&
                            x.DiscountFk.AgentType == user.AgentType &&
                            x.DiscountFk.UserId == null &&
                            x.DiscountFk.AgentType != 0 &&
                            x.DiscountFk.FromDate > DateTime.Now)
                .OrderBy(x => x.DiscountFk.FromDate).ThenByDescending(x => x.DiscountFk.CreationTime)
                .FirstOrDefault(x => x.ProductFk.ProductCode == productCode);
            return Task.FromResult(detail?.DiscountFk);
        }
        private async Task<ProductDiscountDto> GetDiscountFromDb(string productCode, string accountCode,
            decimal amount = 0, int total = 1)
        {
            try
            {
                var user = await GetUserCache(accountCode);
                if (user == null)
                    return null;
                var detail = _discountDetailRepository.GetAllIncluding(x => x.DiscountFk).Include(x => x.ProductFk)
                    .Include(x => x.DiscountFk.UserFk)
                    .Where(x => x.DiscountFk.Status == CommonConst.DiscountStatus.Approved &&
                                x.ProductFk.Status == CommonConst.ProductStatus.Active &&
                                x.DiscountFk.UserFk.AccountCode == accountCode &&
                                x.DiscountFk.FromDate <= DateTime.Now &&
                                x.DiscountFk.ToDate >= DateTime.Now)
                    .OrderByDescending(x => x.DiscountFk.CreationTime)
                    .FirstOrDefault(x => x.ProductFk.ProductCode == productCode) ?? _discountDetailRepository
                    .GetAllIncluding(x => x.DiscountFk).Include(x => x.ProductFk)
                    .Where(x => x.DiscountFk.Status == CommonConst.DiscountStatus.Approved &&
                                x.ProductFk.Status == CommonConst.ProductStatus.Active &&
                                x.DiscountFk.AgentType == user.AgentType &&
                                x.DiscountFk.UserId == null &&
                                x.DiscountFk.AgentType != 0 &&
                                x.DiscountFk.FromDate <= DateTime.Now &&
                                x.DiscountFk.ToDate >= DateTime.Now)
                    .OrderByDescending(x => x.DiscountFk.CreationTime)
                    .FirstOrDefault(x => x.ProductFk.ProductCode == productCode);

                if (detail == null)
                {
                    var product = await _productRepository.FirstOrDefaultAsync(x =>
                        x.ProductCode == productCode && x.Status == CommonConst.ProductStatus.Active);
                    if (product == null)
                        throw new UserFriendlyException("Product not found");

                    var item = new ProductDiscountDto
                    {
                        ProductCode = product.ProductCode,
                        ProductValue = product.ProductValue ?? 0,
                        ProductName = product.ProductName,
                        Description = product.Description,
                        PaymentAmount = amount > 0
                            ? amount * total
                            : (product.ProductValue ?? 0) * total
                    };
                    return item;
                }

                var itemDb = new ProductDiscountDto
                {
                    ProductCode = detail.ProductFk.ProductCode,
                    DiscountValue = detail.DiscountValue,
                    FixAmount = detail.FixAmount,
                    ProductValue = detail.ProductFk.ProductValue ?? 0,
                    ProductName = detail.ProductFk.ProductName,
                    DiscountId = detail.DiscountId,
                    DiscountDetailId = detail.Id,
                    Description = detail.Description,
                    ToDate = detail.DiscountFk.ToDate,
                    FromDate = detail.DiscountFk.FromDate,
                    DiscountCode = detail.DiscountFk.Code,
                    CreatedDate = detail.DiscountFk.CreationTime.ToString("dd/MM/yyyy hh:mm:ss"),
                    ApprovedDate = detail.DiscountFk.DateApproved?.ToString("dd/MM/yyyy hh:mm:ss"),
                    IsSaveCache = true
                };
                return itemDb;
            }
            catch (Exception e)
            {
                _logger.LogError($"GeProductDiscount_ERROR:{e}");
                return null;
            }
        }
    }
}