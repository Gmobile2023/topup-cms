using System;
using System.Collections.Generic;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Abp.UI;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Categories;
using HLS.Topup.Common;
using HLS.Topup.LimitationManager.Dtos;
using HLS.Topup.Products;
using HLS.Topup.RequestDtos;
using HLS.Topup.Services;
using HLS.Topup.Transactions;
using Microsoft.Extensions.Logging;
using ServiceStack;

namespace HLS.Topup.LimitationManager
{
    public class LimitationManager : TopupDomainServiceBase, ILimitationManager
    {
        private readonly IRepository<LimitProductDetail> _limitProductDetailRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly ILogger<LimitationManager> _logger;
        private readonly UserManager _userManager;
        private readonly ITransactionManager _transactionManager;
        private readonly IRepository<Service, int> _lookup_serviceRepository;
        private readonly IRepository<Category, int> _lookup_categoryRepository;

        public LimitationManager(IRepository<Product> productRepository, UserManager userManager,
            IRepository<LimitProductDetail> limitProductDetailRepository, ILogger<LimitationManager> logger,
            ITransactionManager transactionManager,
            IRepository<Service, int> lookup_serviceRepository,
            IRepository<Category, int> lookup_categoryRepository)
        {
            _productRepository = productRepository;
            _userManager = userManager;
            _limitProductDetailRepository = limitProductDetailRepository;
            _logger = logger;
            _transactionManager = transactionManager;
            _lookup_serviceRepository = lookup_serviceRepository;
            _lookup_categoryRepository = lookup_categoryRepository;
        }


        public async Task<bool> CheckLimitConfigProduct(string productcode, string accountCode,int quantity,decimal amount)
        {
            try
            {
                var checkProductInfo = await _transactionManager.GetTotalPerDayProduct(new GetTotalPerDayProductRequest
                {
                    AccountCode = accountCode,
                    ProductCode = productcode
                });
                if (!checkProductInfo.Success || checkProductInfo.Result == null)
                {
                    _logger.LogInformation($"Get limit product error");
                    throw new UserFriendlyException(
                        "Không lấy được thông tin hạn mức sản phẩm. Vui lòng quay lại sau");
                }

                var totalQuantiry = quantity + checkProductInfo.Result.TotalQuantity;
                var totalAmount = checkProductInfo.Result.TotalAmount + (quantity * amount);

                return await CheckLimitConfigProduct(productcode, accountCode, totalAmount,
                    totalQuantiry);
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }

        public async Task<LimitProductDetailDto> GetLimitConfigProduct(string productcode, string accountCode)
        {
            try
            {
                _logger.LogInformation(
                    $"CheckLimitConfigProduct request:{accountCode}-{productcode}");
                var product = await _productRepository.FirstOrDefaultAsync(x => x.ProductCode == productcode);
                if (product == null)
                    throw new UserFriendlyException("Sản phẩm không tồn tại");
                var user = await _userManager.GetUserByAccountCodeAsync(accountCode);
                var detail = await _limitProductDetailRepository.GetAllIncluding(x => x.ProductFk)
                                 .Include(x => x.LimitProductFk)
                                 .Where(x => x.LimitProductFk.Status == CommonConst.LimitProductConfigStatus.Approved &&
                                             x.LimitProductFk.UserId == user.Id &&
                                             x.LimitProductFk.FromDate <= DateTime.Now &&
                                             x.LimitProductFk.ToDate >= DateTime.Now)
                                 .OrderByDescending(x => x.CreationTime)
                                 .FirstOrDefaultAsync(x => x.ProductId == product.Id) ??
                             await _limitProductDetailRepository
                                 .GetAllIncluding(x => x.ProductFk)
                                 .Include(x => x.LimitProductFk)
                                 .Where(x => x.LimitProductFk.Status == CommonConst.LimitProductConfigStatus.Approved &&
                                             x.LimitProductFk.AgentType == user.AgentType &&
                                             x.LimitProductFk.UserId == null &&
                                             x.LimitProductFk.AgentType != 0 &&
                                             x.LimitProductFk.FromDate <= DateTime.Now &&
                                             x.LimitProductFk.ToDate >= DateTime.Now)
                                 .OrderByDescending(x => x.CreationTime)
                                 .FirstOrDefaultAsync(x => x.ProductId == product.Id);

                return detail?.ConvertTo<LimitProductDetailDto>();
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }

        public async Task<List<LimitProductDetailDto>> GetLimitProductsDetails(int limitProductId)
        {
            try
            {
                var query = from o in _limitProductDetailRepository.GetAll()
                        .Where(x => x.LimitProductId == limitProductId)
                    join p1 in _productRepository.GetAll() on o.ProductId equals p1.Id into q1
                    from x1 in q1.DefaultIfEmpty()
                    join o1 in _lookup_categoryRepository.GetAll() on x1.CategoryId equals o1.Id into j1
                    from s1 in j1.DefaultIfEmpty()
                    join o2 in _lookup_serviceRepository.GetAll() on s1.ServiceId equals o2.Id into j2
                    from s2 in j2.DefaultIfEmpty()
                    select new LimitProductDetailDto
                    {
                        ProductId = o.ProductId,
                        LimitAmount = o.LimitAmount,
                        LimitQuantity = o.LimitQuantity,
                        ServiceName = s2.ServicesName,
                        ProductType = s1.CategoryName,
                        ProductName = x1.ProductName
                    };
                return query.ToList();
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }

        private async Task<bool> CheckLimitConfigProduct(string productcode, string accountCode,
            decimal totalAmount, int quantity)
        {
            try
            {
                _logger.LogInformation(
                    $"CheckLimitConfigProduct request:{accountCode}-{productcode}{totalAmount}{quantity}");
                var detail = await GetLimitConfigProduct(productcode, accountCode);

                if (detail == null)
                    return true;

                if (detail.LimitQuantity != null && quantity > detail.LimitQuantity)
                {
                    _logger.LogInformation($"Product over limit total perday");
                    throw new UserFriendlyException(
                        "Sản phẩm đã vượt quá số lượng thanh toán cho phép trong ngày. Vui lòng quay lại sau");
                }

                if (detail.LimitAmount != null && totalAmount > detail.LimitAmount)
                {
                    _logger.LogInformation($"Product over limit totalAmount perday");
                    throw new UserFriendlyException(
                        "Sản phẩm đã vượt quá số tiền thanh toán cho phép trong ngày. Vui lòng quay lại sau");
                }

                return true;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
    }
}
