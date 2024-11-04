using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Linq.Extensions;
using Abp.Runtime.Caching;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Common;
using HLS.Topup.Dtos.Discounts;
using HLS.Topup.Products;
using Microsoft.Extensions.Logging;
using ServiceStack;

namespace HLS.Topup.DiscountManager
{
    /// <summary>
    /// A/E lưu ý phần này là các hàm api viết lấy chính sách cho đại lý dùng app, web. A/e lưu ý khi tác động mấy phần này. Dễ bị sai chính sách bán hàng
    /// </summary>
    public partial class DiscountManger : TopupDomainServiceBase, IDiscountManger
    {
        private readonly IRepository<Discount> _discountRepository;
        private readonly IRepository<DiscountDetail> _discountDetailRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly ILogger<DiscountManger> _logger;
        private readonly UserManager _userManager;
        private readonly ICacheManager _cacheManager;
        private readonly IRedisCache _redisCache;

        public DiscountManger(IRepository<Discount> discountRepository,
            IRepository<DiscountDetail> discountDetailRepository,
            IRepository<Product> productRepository, UserManager userManager,
            ILogger<DiscountManger> logger, ICacheManager cacheManager, IRedisCache redisCache = null)
        {
            _discountRepository = discountRepository;
            _discountDetailRepository = discountDetailRepository;
            _productRepository = productRepository;
            _userManager = userManager;
            _logger = logger;
            _cacheManager = cacheManager;
            _redisCache = redisCache;
        }

        public async Task<bool> CreateOrUpdateDiscount(List<DiscountDetailDto> detail, User user)
        {
            try
            {
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task UpdateDiscountsStop()
        {
            try
            {
                var discounts = _discountRepository.GetAll()
                    .Where(x => x.Status == CommonConst.DiscountStatus.Approved).OrderByDescending(x => x.Id);
                if (discounts.Any())
                {
                    var maxId = await discounts.MaxAsync(x => x.Id);
                    var listUpdate = discounts.Where(x => x.Id != maxId);
                    foreach (var item in listUpdate)
                    {
                        item.Status = CommonConst.DiscountStatus.StopApply;
                        await _discountRepository.UpdateAsync(item);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// Hàm lấy danh sách chiết khấu khi thiết lập. Thêm mới hoặc tạo mới chiết khấu
        /// </summary>
        /// <param name="discountId"></param>
        /// <param name="cateIds"></param>
        /// <param name="isGetAll"></param>
        /// <returns></returns>
        public async Task<List<DiscountDetailDto>> GetDiscountDetails(int? discountId = null,
            List<int?> cateIds = null, bool? isGetAll = false)
        {
            var lst = new List<DiscountDetailDto>();
            if (discountId == null)
            {
                var lstProduct = _productRepository.GetAllIncluding(x => x.CategoryFk).Include(x => x.CategoryFk)
                    // .Where(x => x.CategoryFk.ServiceFk.ServiceCode == CommonConst.ServiceCodes.TOPUP ||
                    //             x.CategoryFk.ServiceFk.ServiceCode == CommonConst.ServiceCodes.PIN_CODE ||
                    //             x.CategoryFk.ServiceFk.ServiceCode == CommonConst.ServiceCodes.PIN_DATA)
                    .WhereIf(cateIds != null && cateIds.Count > 0 && cateIds.Count(x => x.HasValue) > 0,
                        e => cateIds.Contains(e.CategoryId));
                foreach (var item in lstProduct)
                {
                    var d = item.ConvertTo<DiscountDetailDto>();
                    d.CategoryId = item.CategoryFk != null ? item.CategoryFk.Id : 0;
                    d.ParentCategoryId = item.CategoryFk != null ? item.CategoryFk.ParentCategoryId : 0;
                    d.CategoryCode = item.CategoryFk != null ? item.CategoryFk.CategoryCode : string.Empty;
                    d.CategoryName = item.CategoryFk != null ? item.CategoryFk.CategoryName : string.Empty;
                    d.ProductId = item.Id;
                    d.ProductName = item.ProductName;
                    if (item.ProductValue != null) d.ProductValue = item.ProductValue;
                    d.ProductCode = item.ProductCode;
                    d.Order = item.CategoryFk != null ? item.CategoryFk.Order : 0;
                    d.Status = item.CategoryFk != null ? item.CategoryFk.Status : 0;
                    lst.Add(d);
                }

                return await Task.FromResult(lst.OrderBy(x => x.CategoryCode).ThenBy(x => x.ProductValue).ToList());
            }
            else
            {
                if (isGetAll != null && isGetAll == true)
                {
                    var query = from p in _productRepository.GetAllIncluding(x => x.CategoryFk)
                        join d in _discountDetailRepository.GetAllIncluding(x => x.DiscountFk)
                                .Where(x => x.DiscountId == discountId) on p.Id equals
                            d.ProductId into temDiscount
                        from discount in temDiscount.DefaultIfEmpty()
                        select new DiscountDetailDto
                        {
                            CategoryId = p.CategoryFk != null ? p.CategoryFk.Id : 0,
                            CategoryCode = p.CategoryFk != null ? p.CategoryFk.CategoryCode : string.Empty,
                            CategoryName = p.CategoryFk != null ? p.CategoryFk.CategoryName : string.Empty,
                            ParentCategoryId = p.CategoryFk != null ? p.CategoryFk.ParentCategoryId : 0,
                            DiscountValue = discount != null ? Math.Abs(discount.DiscountValue ?? 0) : null,
                            FixAmount = discount != null ? discount.FixAmount : null,
                            DiscountId = discount != null ? discount.DiscountId : (int?)null,
                            UserId = discount != null && discount.DiscountFk != null
                                ? discount.DiscountFk.UserId.Value
                                : (long?)null,
                            ProductId = p.Id,
                            ProductValue = p.ProductValue,
                            ProductName = p.ProductName,
                            ProductCode = p.ProductCode,
                            Order = p.CategoryFk != null ? p.CategoryFk.Order : 0,
                        };
                    return await Task.FromResult(query
                        .WhereIf(cateIds != null && cateIds.Count > 0 && cateIds.Count(x => x.HasValue) > 0,
                            e => cateIds.Contains(e.CategoryId)).OrderBy(x => x.CategoryCode)
                        .ThenBy(x => x.ProductValue)
                        .ToList());
                }
                else
                {
                    var query = _discountDetailRepository.GetAllIncluding(x => x.DiscountFk)
                        .Include(x => x.ProductFk)
                        .Where(x => x.DiscountId == discountId)
                        .WhereIf(cateIds != null && cateIds.Count > 0 && cateIds.Count(x => x.HasValue) > 0,
                            e => cateIds.Contains(e.CategoryFk.Id));
                    var list = query.Select(p => new DiscountDetailDto
                    {
                        CategoryId = p.ProductFk != null ? p.ProductFk.CategoryId : 0,
                        CategoryCode = p.ProductFk != null && p.ProductFk.CategoryFk != null
                            ? p.ProductFk.CategoryFk.CategoryCode
                            : string.Empty,
                        CategoryName = p.ProductFk != null && p.ProductFk.CategoryFk != null
                            ? p.ProductFk.CategoryFk.CategoryName
                            : string.Empty,
                        ParentCategoryId = p.ProductFk != null && p.ProductFk.CategoryFk != null
                            ? p.ProductFk.CategoryFk.ParentCategoryId
                            : 0,
                        DiscountValue = Math.Abs(p.DiscountValue ?? 0),
                        FixAmount = p.FixAmount,
                        //Increase = p.IsIncrease ? 1 : 0,
                        DiscountId = p.DiscountId,
                        UserId = p.DiscountFk != null ? p.DiscountFk.UserId : 0,
                        ProductId = p.ProductId,
                        ProductValue = p.ProductFk != null ? p.ProductFk.ProductValue : 0,
                        ProductName = p.ProductFk != null ? p.ProductFk.ProductName : string.Empty,
                        ProductCode = p.ProductFk != null ? p.ProductFk.ProductCode : string.Empty,
                        Order = p.CategoryFk != null ? p.CategoryFk.Order : 0,
                        ServiceName = p.ServiceFk != null ? p.ServiceFk.ServiceCode : string.Empty
                    });
                    var p = list.ToList();
                    return await Task.FromResult(p);
                }
            }
        }

        //todo hàm này xem lại. lấy list sp với discount cho hợp lý
        public async Task<List<ProductDiscountDto>> GetProductDiscounts(string categoryCode, string accountCode,
            string productCode = null)
        {
            var products = await _productRepository.GetAllIncluding(x => x.CategoryFk)
                .Where(x => x.Status == CommonConst.ProductStatus.Active)
                .Where(x => x.CategoryFk.Status == CommonConst.CategoryStatus.Active)
                .Where(x => x.CategoryFk.CategoryCode == categoryCode).ToListAsync();
            if (products == null)
                return null;
            var lst = new List<ProductDiscountDto>();
            foreach (var item in products.OrderBy(x => x.ProductValue).ThenBy(x => x.Order))
            {
                lst.Add(await GeProductDiscountAccount(item.ProductCode, accountCode));
            }

            return lst;
        }

        /// <summary>
        /// Lấy danh sách chiết khấu theo Service
        /// </summary>
        /// <param name="serviceCode"></param>
        /// <param name="accountCode"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public async Task<List<ProductDiscountDto>> GetProductDiscountsByService(string serviceCode, string accountCode,
            string search = null)
        {
            var products = _productRepository.GetAllIncluding(x => x.CategoryFk)
                .Include(x => x.CategoryFk.ServiceFk)
                .Where(x => x.Status == CommonConst.ProductStatus.Active)
                .Where(x => x.CategoryFk.Status == CommonConst.CategoryStatus.Active)
                .Where(x => x.CategoryFk.ServiceFk.ServiceCode == serviceCode)
                .WhereIf(!string.IsNullOrEmpty(search),
                    x => x.ProductName.ToLower().Contains(search.ToLower()) || x.ProductCode.Contains(search) ||
                         x.CategoryFk.CategoryCode == search ||
                         x.CategoryFk.CategoryName.ToLower().Contains(search.ToLower()));

            var lst = new List<ProductDiscountDto>();
            foreach (var item in products.ToList())
            {
                var pro = await GeProductDiscountAccount(item.ProductCode, accountCode);
                pro.CategoryCode = item.CategoryFk.CategoryCode;
                pro.CategoryName = item.CategoryFk.CategoryName;
                lst.Add(pro);
            }

            var lstView = new List<ProductDiscountDto>();
            var cates = lst.Select(c => c.CategoryName).Distinct().OrderBy(c => c).ToList();
            foreach (var c in cates)
            {
                var item = lst.Where(x => x.CategoryName == c).OrderBy(x => x.ProductValue).ToList();
                lstView.AddRange(item);
            }

            return lstView;
        }

        // public async Task<ProductDiscountDto> GetDiscountAccount(string product, string accountCode,
        //     decimal amount = 0, int total = 1)
        // {
        //     try
        //     {
        //         _logger.LogInformation($"GetDiscountAccount request:{accountCode}-{product}-{amount}-{total}");
        //         // var productItem = await _productRepository.FirstOrDefaultAsync(x =>
        //         //     x.ProductCode == product && x.Status == CommonConst.ProductStatus.Active);
        //         // if (productItem == null)
        //         //     return null;
        //         var detail = await GeProductDiscount(product, accountCode, amount, total);
        //         if (detail == null)
        //             return null;
        //         _logger.LogInformation($"GetDiscountAmount return:{detail.ToJson()}");
        //         return detail;
        //     }
        //     catch (Exception e)
        //     {
        //         _logger.LogError($"GetDiscountAccount error:{e}");
        //         return null;
        //     }
        // }

        /// <summary>
        /// Hàm này vừa lấy ck vừa check thông tin sản phẩm. Nếu k có ck thì cũng trả về giá của sản phẩm
        /// </summary>
        /// <param name="product"></param>
        /// <param name="accountCode"></param>
        /// <param name="amount"></param>
        /// <param name="total"></param>
        /// <param name="isApi"></param>
        /// <returns></returns>
        public async Task<ProductDiscountDto> GeProductDiscountAccount_BAK(string product, string accountCode,
            decimal amount = 0, int total = 1)
        {
            try
            {
                _logger.LogInformation($"GetDiscountAccount request:{accountCode}-{product}-{amount}-{total}");
                var detail = await GeProductDiscountAccount(product, accountCode, amount, total);
                if (detail == null)
                {
                    var pro = await _productRepository.GetAllIncluding(x => x.CategoryFk)
                        .FirstOrDefaultAsync(x =>
                            x.ProductCode == product && x.Status == CommonConst.ProductStatus.Active &&
                            x.CategoryFk.Status == CommonConst.CategoryStatus.Active);
                    if (pro == null)
                        return null;
                    return new ProductDiscountDto
                    {
                        ProductCode = pro.ProductCode,
                        ProductValue = pro.ProductValue ?? 0,
                        ProductName = pro.ProductName,
                        PaymentAmount = pro.ProductValue ?? 0,
                        Description = pro.Description,
                    };
                }

                _logger.LogInformation($"GetDiscountAmount return:{detail.ToJson()}");
                return detail;
            }
            catch (Exception e)
            {
                _logger.LogError($"GetDiscountAccount error:{e}");
                return null;
            }
        }

        private async Task<ProductDiscountDto> GeProductDiscount_bak_0(Product product, string accountCode,
            decimal amount = 0, int total = 1)
        {
            try
            {
                var user = await _userManager.GetUserByAccountCodeAsync(accountCode);
                var detail = await _discountDetailRepository.GetAllIncluding(x => x.ProductFk)
                    .Include(x => x.DiscountFk)
                    .Where(x => x.DiscountFk.Status == CommonConst.DiscountStatus.Approved &&
                                x.ProductFk.Status == CommonConst.ProductStatus.Active &&
                                x.DiscountFk.UserId == user.Id &&
                                x.DiscountFk.FromDate <= DateTime.Now &&
                                x.DiscountFk.ToDate >= DateTime.Now)
                    .OrderByDescending(x => x.CreationTime)
                    .FirstOrDefaultAsync(x => x.ProductId == product.Id);

                if (detail == null)
                {
                    detail = await _discountDetailRepository.GetAllIncluding(x => x.ProductFk)
                        .Include(x => x.DiscountFk)
                        .Where(x => x.DiscountFk.Status == CommonConst.DiscountStatus.Approved &&
                                    x.ProductFk.Status == CommonConst.ProductStatus.Active &&
                                    x.DiscountFk.AgentType == user.AgentType &&
                                    x.DiscountFk.UserId == null &&
                                    x.DiscountFk.AgentType != 0 &&
                                    x.DiscountFk.FromDate <= DateTime.Now &&
                                    x.DiscountFk.ToDate >= DateTime.Now)
                        .OrderByDescending(x => x.CreationTime)
                        .FirstOrDefaultAsync(x => x.ProductId == product.Id);
                }

                if (detail == null)
                    return new ProductDiscountDto
                    {
                        ProductCode = product.ProductCode,
                        ProductValue = product.ProductValue ?? 0,
                        ProductName = product.ProductName,
                        Description = product.Description,
                        PaymentAmount = amount > 0 ? amount : product.ProductValue ?? 0
                    };

                var rs = new ProductDiscountDto
                {
                    ProductCode = detail.ProductFk.ProductCode,
                    DiscountValue = detail.DiscountValue,
                    FixAmount = detail.FixAmount,
                    ProductValue = detail.ProductFk.ProductValue ?? 0,
                    ProductName = detail.ProductFk.ProductName,
                    DiscountId = detail.DiscountId,
                    DiscountDetailId = detail.Id,
                    Description = detail.ProductFk.Description
                };

                var result = GetDiscountAmount(rs, amount, total);
                return result;
            }
            catch (Exception e)
            {
                _logger.LogError($"GeProductDiscount_ERROR:{e}");
                return null;
            }
        }

        private async Task<ProductDiscountDto> GeProductDiscount_bak(string productCode, string accountCode,
            decimal amount = 0, int total = 1)
        {
            try
            {
                var user = await GetUserCache(accountCode);
                if (user == null)
                    return null;

                var discount = await GetAllDiscountCache();
                var detail = discount
                    .Where(x => x.Status == CommonConst.DiscountStatus.Approved &&
                                x.ProductStatus == CommonConst.ProductStatus.Active &&
                                x.UserId == user.Id &&
                                x.FromDate <= DateTime.Now &&
                                x.ToDate >= DateTime.Now)
                    .OrderByDescending(x => x.CreationTime)
                    .FirstOrDefault(x => x.ProductCode == productCode);

                if (detail == null)
                {
                    detail = discount
                        .Where(x => x.Status == CommonConst.DiscountStatus.Approved &&
                                    x.ProductStatus == CommonConst.ProductStatus.Active &&
                                    x.AgentType == user.AgentType &&
                                    x.UserId == null &&
                                    x.AgentType != 0 &&
                                    x.FromDate <= DateTime.Now &&
                                    x.ToDate >= DateTime.Now)
                        .OrderByDescending(x => x.CreationTime)
                        .FirstOrDefault(x => x.ProductCode == productCode);
                }

                if (detail == null)
                {
                    var product = await _productRepository.FirstOrDefaultAsync(x =>
                        x.ProductCode == productCode && x.Status == CommonConst.ProductStatus.Active);
                    if (product == null)
                        return null;

                    return new ProductDiscountDto
                    {
                        ProductCode = product.ProductCode,
                        ProductValue = product.ProductValue ?? 0,
                        ProductName = product.ProductName,
                        Description = product.Description,
                        PaymentAmount = amount > 0 ? amount : product.ProductValue ?? 0,
                    };
                }

                var rs = new ProductDiscountDto
                {
                    ProductCode = detail.ProductCode,
                    DiscountValue = detail.DiscountValue,
                    FixAmount = detail.FixAmount,
                    ProductValue = detail.ProductValue,
                    ProductName = detail.ProductName,
                    DiscountId = detail.DiscountId,
                    DiscountDetailId = detail.DiscountDetailId,
                    Description = detail.Description,
                    ToDate = detail.ToDate
                };
                //Lấy ngày hết hạn cho cache bên core
                var expireDate = detail.ToDate;
                var lastDiscount = await GetDiscountForExpireDate(user, productCode);
                if (lastDiscount != null && lastDiscount.FromDate < expireDate)
                {
                    expireDate = lastDiscount.FromDate;
                }

                rs.ToDate = expireDate;
                var result = GetDiscountAmount(rs, amount, total);
                return result;
            }
            catch (Exception e)
            {
                _logger.LogError($"GeProductDiscount_ERROR:{e}");
                return null;
            }
        }

        private ProductDiscountDto GetDiscountAmount(ProductDiscountDto rs, decimal amount = 0, int total = 1)
        {
            var price = amount > 0 ? amount : rs.ProductValue;
            if (rs.FixAmount != null && rs.DiscountValue != null)
            {
                var discountPercent = rs.DiscountValue * price / 100;
                if (discountPercent < rs.FixAmount)
                {
                    rs.DiscountAmount = discountPercent ?? 0;
                    rs.IsDiscount = true;
                }
                else
                {
                    rs.DiscountAmount = rs.FixAmount ?? 0;
                }
            }
            else if (rs.DiscountValue != null)
            {
                rs.DiscountAmount = (rs.DiscountValue * price ?? 0) / 100;
                rs.IsDiscount = true;
            }
            else if (rs.FixAmount != null)
            {
                rs.DiscountAmount = rs.FixAmount ?? 0;
            }

            rs.DiscountAmount = Math.Round(rs.DiscountAmount);
            rs.PaymentAmount = price - rs.DiscountAmount;
            return rs;
        }


        private Task<List<ProductDiscountCache>> GetAllDiscountCache()
        {
            try
            {
                return _cacheManager
                    .GetCache("Discounts")
                    .AsTyped<object, List<ProductDiscountCache>>()
                    .GetAsync("DiscountAccount", async f => await GetAllDiscount());
            }
            catch (Exception e)
            {
                _logger.LogError($"GetAllDiscountCache:{e}");
                return Task.FromResult<List<ProductDiscountCache>>(null);
            }
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

        private Task<Product> GetProductCache(string productCode)
        {
            try
            {
                return _cacheManager
                    .GetCache("Products")
                    .AsTyped<object, Product>()
                    .GetAsync($"Items:{productCode}",
                        async f => await _productRepository.FirstOrDefaultAsync(x => x.ProductCode == productCode));
            }
            catch (Exception e)
            {
                _logger.LogError($"GetProductCache:{e}");
                return null;
            }
        }

        private async Task<List<ProductDiscountCache>> GetAllDiscount()
        {
            try
            {
                var details = await _discountDetailRepository.GetAllIncluding(x => x.ProductFk)
                    .Include(x => x.DiscountFk)
                    .Where(x => (x.DiscountFk.Status == CommonConst.DiscountStatus.Approved ||
                                 x.DiscountFk.Status == CommonConst.DiscountStatus.Pending ||
                                 x.DiscountFk.Status == CommonConst.DiscountStatus.NotApply) &&
                                x.ProductFk.Status == CommonConst.ProductStatus.Active &&
                                ((x.DiscountFk.FromDate <= DateTime.Now && x.DiscountFk.ToDate >= DateTime.Now) ||
                                 x.DiscountFk.FromDate > DateTime.Now))
                    .OrderByDescending(x => x.CreationTime).ToListAsync();


                var items = details?.Select(x => new ProductDiscountCache
                {
                    ProductCode = x.ProductFk.ProductCode,
                    DiscountValue = x.DiscountValue,
                    FixAmount = x.FixAmount,
                    ProductValue = x.ProductFk.ProductValue ?? 0,
                    ProductName = x.ProductFk.ProductName,
                    DiscountId = x.DiscountId,
                    DiscountDetailId = x.Id,
                    Description = x.ProductFk.Description,
                    AgentType = x.DiscountFk.AgentType,
                    UserId = x.DiscountFk.UserId,
                    Status = x.DiscountFk.Status,
                    FromDate = x.DiscountFk.FromDate,
                    ToDate = x.DiscountFk.ToDate,
                    CreationTime = x.DiscountFk.CreationTime,
                    ProductId = x.ProductId ?? 0,
                    ProductStatus = x.ProductFk.Status
                });
                return items?.ToList();
            }
            catch (Exception e)
            {
                _logger.LogError($"GeProductDiscount_ERROR:{e}");
                return null;
            }
        }

        private Task<Discount> GetDiscountForExpireDate(User user, string productCode)
        {
            var detail = _discountDetailRepository.GetAllIncluding(x => x.DiscountFk).Include(x => x.ProductFk)
                .Include(x => x.DiscountFk.UserFk)
                .Where(x => x.DiscountFk.Status == CommonConst.DiscountStatus.Approved &&
                            x.ProductFk.Status == CommonConst.ProductStatus.Active &&
                            x.DiscountFk.UserFk.AccountCode == user.AccountCode &&
                            x.DiscountFk.FromDate > DateTime.Now)
                .OrderBy(x => x.DiscountFk.FromDate).ThenByDescending(x => x.CreationTime)
                .FirstOrDefault(x => x.ProductFk.ProductCode == productCode) ?? _discountDetailRepository
                .GetAllIncluding(x => x.DiscountFk).Include(x => x.ProductFk)
                .Where(x => x.DiscountFk.Status == CommonConst.DiscountStatus.Approved &&
                            x.ProductFk.Status == CommonConst.ProductStatus.Active &&
                            x.DiscountFk.AgentType == user.AgentType &&
                            x.DiscountFk.UserId == null &&
                            x.DiscountFk.AgentType != 0 &&
                            x.DiscountFk.FromDate > DateTime.Now)
                .OrderBy(x => x.DiscountFk.FromDate).ThenByDescending(x => x.CreationTime)
                .FirstOrDefault(x => x.ProductFk.ProductCode == productCode);
            return Task.FromResult(detail?.DiscountFk);
        }
    }
}