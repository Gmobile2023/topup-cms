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
using HLS.Topup.Dtos.BillFees;
using HLS.Topup.Dtos.Fees;
using HLS.Topup.Products;
using Microsoft.Extensions.Logging;
using ServiceStack;
using Abp.Runtime.Caching;

namespace HLS.Topup.FeeManager
{
    /// <summary>
    /// A/E lưu ý phần này là các hàm api viết lấy chính sách cho đại lý dùng app, web. A/e lưu ý khi tác động mấy phần này. Dễ bị sai chính sách bán hàng
    /// </summary>
    public partial class FeeManager : TopupDomainServiceBase, IFeeManager
    {
        private readonly IRepository<FeeDetail> _feeDetailsRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly ILogger<FeeManager> _logger;
        private readonly UserManager _userManager;
        private readonly ICacheManager _cacheManager;
        private readonly IRedisCache _redisCache;
        public FeeManager(IRepository<FeeDetail> feeDetailsRepository, IRepository<Product> productRepository,
            UserManager userManager,
            ILogger<FeeManager> logger, ICacheManager cacheManager, IRedisCache redisCache = null)
        {
            _feeDetailsRepository = feeDetailsRepository;
            _productRepository = productRepository;
            _userManager = userManager;
            _logger = logger;
            _cacheManager = cacheManager;
            _redisCache = redisCache;
        }

        public async Task<List<BillFeeDetailDto>> GetFeeDetails(int? feeId = null,
            List<int?> cateIds = null, List<int?> prdIds = null)
        {
            var query = from p in _productRepository.GetAllIncluding(x => x.CategoryFk).Where(x =>
                    x.Status == CommonConst.ProductStatus.Active &&
                    x.CategoryFk.Status == CommonConst.CategoryStatus.Active)
                join fee in _feeDetailsRepository.GetAllIncluding(x => x.FeeFk)
                        .Where(x => x.FeeId == feeId) on p.Id equals
                    fee.ProductId
                select new BillFeeDetailDto
                {
                    CategoryId = p.CategoryFk.Id,
                    CategoryCode = p.CategoryFk.CategoryCode,
                    CategoryName = p.CategoryFk.CategoryName,
                    ParentCategoryId = p.CategoryFk.ParentCategoryId,
                    AmountMinFee = fee.AmountMinFee,
                    MinFee = fee.MinFee,
                    AmountIncrease = fee.AmountIncrease,
                    UserId = fee.FeeFk.UserId,
                    ProductId = p.Id,
                    SubFee = fee.SubFee??0,
                    ProductName = p.ProductName,
                    ProductCode = p.ProductCode,
                    Order = p.CategoryFk.Order
                };
            return query.WhereIf(prdIds != null && prdIds.Count > 0 && prdIds.Count(x => x.HasValue) > 0,
                    e => prdIds.Contains(e.ProductId)).OrderBy(x => x.ProductCode)
                .ToList();
        }


        public async Task<ProductFeeDto> GetProductFee_bak(string productcode, string accountCode, decimal amount)
        {
            try
            {
                // if (amount <= 0)
                //     throw new UserFriendlyException("Số tiền thanh toán không hợp lệ");
                var product = await _productRepository.FirstOrDefaultAsync(x => x.ProductCode == productcode);
                if (product == null)
                    throw new UserFriendlyException("Sản phẩm không tồn tại");
                var user = await _userManager.GetUserByAccountCodeAsync(accountCode);
                var detail = await _feeDetailsRepository.GetAllIncluding(x => x.ProductFk)
                    .Include(x => x.FeeFk)
                    .Where(x => x.FeeFk.Status == CommonConst.FeeStatus.Approved &&
                                x.ProductFk.Status == CommonConst.ProductStatus.Active &&
                                x.FeeFk.UserId == user.Id &&
                                x.FeeFk.FromDate <= DateTime.Now &&
                                x.FeeFk.ToDate >= DateTime.Now)
                    .OrderByDescending(x => x.CreationTime)
                    .FirstOrDefaultAsync(x => x.ProductId == product.Id) ?? await _feeDetailsRepository
                    .GetAllIncluding(x => x.ProductFk)
                    .Include(x => x.FeeFk)
                    .Where(x => x.FeeFk.Status == CommonConst.FeeStatus.Approved &&
                                x.ProductFk.Status == CommonConst.ProductStatus.Active &&
                                x.FeeFk.AgentType == user.AgentType &&
                                x.FeeFk.UserId == null &&
                                x.FeeFk.AgentType != 0 &&
                                x.FeeFk.FromDate <= DateTime.Now &&
                                x.FeeFk.ToDate >= DateTime.Now)
                    .OrderByDescending(x => x.CreationTime)
                    .FirstOrDefaultAsync(x => x.ProductId == product.Id);

                if (detail == null)
                    return new ProductFeeDto
                    {
                        ProductCode = product.ProductCode,
                        ProductName = product.ProductName,
                        Amount = amount,
                        TotalAmount = amount
                    };
                var rs = new ProductFeeDto
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
                var result = GetFreeAmount(rs);
                return result;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }

        // public async Task<ProductFeeDto> GetFeeConfigAccount(string productcode, string accountCode)
        // {
        //     try
        //     {
        //         // if (amount <= 0)
        //         //     throw new UserFriendlyException("Số tiền thanh toán không hợp lệ");
        //         var product = await _productRepository.FirstOrDefaultAsync(x => x.ProductCode == productcode);
        //         if (product == null)
        //             throw new UserFriendlyException("Sản phẩm không tồn tại");
        //         var user = await _userManager.GetUserByAccountCodeAsync(accountCode);
        //         var detail = await _feeDetailsRepository.GetAllIncluding(x => x.ProductFk)
        //             .Include(x => x.FeeFk)
        //             .Where(x => x.FeeFk.Status == CommonConst.FeeStatus.Approved &&
        //                         x.FeeFk.UserId == user.Id &&
        //                         x.FeeFk.FromDate <= DateTime.Now &&
        //                         x.FeeFk.ToDate >= DateTime.Now)
        //             .OrderByDescending(x => x.CreationTime)
        //             .FirstOrDefaultAsync(x => x.ProductId == product.Id) ?? await _feeDetailsRepository
        //             .GetAllIncluding(x => x.ProductFk)
        //             .Include(x => x.FeeFk)
        //             .Where(x => x.FeeFk.Status == CommonConst.FeeStatus.Approved &&
        //                         x.FeeFk.AgentType == user.AgentType &&
        //                         x.FeeFk.UserId == null &&
        //                         x.FeeFk.AgentType != 0 &&
        //                         x.FeeFk.FromDate <= DateTime.Now &&
        //                         x.FeeFk.ToDate >= DateTime.Now)
        //             .OrderByDescending(x => x.CreationTime)
        //             .FirstOrDefaultAsync(x => x.ProductId == product.Id);

        //         if (detail == null)
        //             return null;
        //         return new ProductFeeDto
        //         {
        //             ProductCode = detail.ProductFk.ProductCode,
        //             MinFee = detail.MinFee,
        //             SubFee = detail.SubFee,
        //             ProductName = detail.ProductFk.ProductName,
        //             FeeId = detail.FeeId,
        //             FeeDetailId = detail.Id,
        //             AmountIncrease = detail.AmountIncrease,
        //             AmountMinFee = detail.AmountMinFee
        //         };
        //     }
        //     catch (Exception e)
        //     {
        //         throw new UserFriendlyException(e.Message);
        //     }
        // }

        private ProductFeeDto GetFreeAmount(ProductFeeDto rs)
        {
            var fee = GetFeePlus(rs.Amount, (rs.AmountMinFee ?? 0), (rs.AmountIncrease ?? 0), (rs.MinFee ?? 0),
                rs.SubFee ?? 0);
            rs.FeeValue = fee;
            rs.TotalAmount = rs.Amount + rs.FeeValue;
            return rs;
        }

        private decimal GetFeePlus(decimal paymentAmount, decimal minAmount, decimal amountPlus, decimal minfee,
            decimal subFee)
        {
            //Nếu số tiền thanh toán <= giá trị áp dụng phí. => Lấy phí tối thiểu
            if (paymentAmount <= minAmount)
                return minfee;
            var amountP = paymentAmount - minAmount;
            //Nếu số tiền tăng thêm <= giá trị áp dụng=> Lấy thụ phí
            decimal total;
            if (amountP <= amountPlus)
            {
                total = 1;
            }
            else
            {
                total = amountPlus > 0 ? amountP / amountPlus : amountP;
            }

            var totalPlus = Math.Ceiling(total);
            return (subFee * totalPlus) + minfee;
        }
    }
}
