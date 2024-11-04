using HLS.Topup.Categories;
using System.Collections.Generic;
using HLS.Topup.Common;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using HLS.Topup.Products.Exporting;
using HLS.Topup.Products.Dtos;
using HLS.Topup.Dto;
using Abp.Application.Services.Dto;
using HLS.Topup.Authorization;
using Abp.Authorization;
using Abp.Runtime.Caching;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceStack;

namespace HLS.Topup.Products
{
    [AbpAuthorize(AppPermissions.Pages_Products)]
    public class ProductsAppService : TopupAppServiceBase, IProductsAppService
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IProductsExcelExporter _productsExcelExporter;
        private readonly IRepository<Category, int> _lookup_categoryRepository;
        private readonly UrlExtentions _extentions;
        private readonly ICacheManager _cacheManager;
        private readonly ILogger<ProductsAppService> _logger;
      

        public ProductsAppService(IRepository<Product> productRepository, IProductsExcelExporter productsExcelExporter,
            IRepository<Category, int> lookup_categoryRepository, UrlExtentions extentions, ICacheManager cacheManager, ILogger<ProductsAppService> logger)
        {
            _productRepository = productRepository;
            _productsExcelExporter = productsExcelExporter;
            _lookup_categoryRepository = lookup_categoryRepository;
            _extentions = extentions;
            _cacheManager = cacheManager;
            _logger = logger;  
        }

        public async Task<PagedResultDto<GetProductForViewDto>> GetAll(GetAllProductsInput input)
        {
            var productTypeFilter = input.ProductTypeFilter.HasValue
                ? (CommonConst.ProductType)input.ProductTypeFilter
                : default;
            var statusFilter = input.StatusFilter.HasValue
                ? (CommonConst.ProductStatus)input.StatusFilter
                : default;

            var filteredProducts = _productRepository.GetAll()
                .Include(e => e.CategoryFk)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    e => false || e.ProductCode.Contains(input.Filter) || e.ProductName.Contains(input.Filter) ||
                         e.Unit.Contains(input.Filter) || e.Image.Contains(input.Filter) ||
                         e.Description.Contains(input.Filter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProductCodeFilter),
                    e => e.ProductCode.Contains(input.ProductCodeFilter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter),
                    e => e.ProductName.Contains(input.ProductNameFilter))
                .WhereIf(input.ProductTypeFilter.HasValue && input.ProductTypeFilter > -1,
                    e => e.ProductType == productTypeFilter)
                .WhereIf(input.StatusFilter.HasValue && input.StatusFilter > -1, e => e.Status == statusFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.UnitFilter), e => e.Unit == input.UnitFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.CustomerSupportNoteFilter),
                    e => e.CustomerSupportNote.Contains(input.CustomerSupportNoteFilter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.UserManualNoteFilter),
                    e => e.UserManualNote.Contains(input.UserManualNoteFilter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.CategoryCategoryNameFilter),
                    e => e.CategoryFk != null && e.CategoryFk.CategoryName.Contains(input.CategoryCategoryNameFilter));

            var pagedAndFilteredProducts = filteredProducts
                .OrderByDescending(x=>x.Id).ThenBy(x=>x.ProductName)
                .PageBy(input);

            var products = from o in pagedAndFilteredProducts
                           join o1 in _lookup_categoryRepository.GetAll() on o.CategoryId equals o1.Id into j1
                           from s1 in j1.DefaultIfEmpty()
                           select new GetProductForViewDto()
                           {
                               Product = new ProductDto
                               {
                                   ProductCode = o.ProductCode,
                                   ProductName = o.ProductName,
                                   Order = o.Order,
                                   ProductValue = o.ProductValue,
                                   ProductType = o.ProductType,
                                   Status = o.Status,
                                   Unit = o.Unit,
                                   Id = o.Id,
                                   CustomerSupportNote = o.CustomerSupportNote,
                                   UserManualNote = o.UserManualNote
                               },
                               CategoryCategoryName = s1 == null || s1.CategoryName == null ? "" : s1.CategoryName.ToString()
                           };

            var totalCount = await filteredProducts.CountAsync();

            return new PagedResultDto<GetProductForViewDto>(
                totalCount,
                await products.ToListAsync()
            );
        }

        public async Task<GetProductForViewDto> GetProductForView(int id)
        {
            var product = await _productRepository.GetAsync(id);

            var output = new GetProductForViewDto { Product = ObjectMapper.Map<ProductDto>(product) };
            output.Product.Image = _extentions.GetFullPath(output.Product.Image);
            if (output.Product.CategoryId != null)
            {
                var _lookupCategory =
                    await _lookup_categoryRepository.FirstOrDefaultAsync((int)output.Product.CategoryId);
                output.CategoryCategoryName = _lookupCategory?.CategoryName?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Products_Edit)]
        public async Task<GetProductForEditOutput> GetProductForEdit(EntityDto input)
        {
            var product = await _productRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetProductForEditOutput { Product = ObjectMapper.Map<CreateOrEditProductDto>(product) };
            output.Product.Image = _extentions.GetFullPath(output.Product.Image);
            if (output.Product.CategoryId != null)
            {
                var _lookupCategory =
                    await _lookup_categoryRepository.FirstOrDefaultAsync((int)output.Product.CategoryId);
                output.CategoryCategoryName = _lookupCategory?.CategoryName?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditProductDto input)
        {
        

            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
           
        }

        [AbpAuthorize(AppPermissions.Pages_Products_Create)]
        protected virtual async Task Create(CreateOrEditProductDto input)
        {
            var product = ObjectMapper.Map<Product>(input);

            if (AbpSession.TenantId != null)
            {
                product.TenantId = (int?)AbpSession.TenantId;
            }

            await _productRepository.InsertAsync(product);
           
        }

        [AbpAuthorize(AppPermissions.Pages_Products_Edit)]
        protected virtual async Task Update(CreateOrEditProductDto input)
        {
            var product = await _productRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, product);
          
        }

        [AbpAuthorize(AppPermissions.Pages_Products_Delete)]
        public async Task Delete(EntityDto input)
        {
          
            await _productRepository.DeleteAsync(input.Id);
            
        }
        private async Task<bool> ClearCache(EntityDto<string> input)
        {
            try
            {
                var cache = _cacheManager.GetCache(input.Id);
                await cache.ClearAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError($"ClearCache:{e}");
                return false;
            }
        }

        public async Task<FileDto> GetProductsToExcel(GetAllProductsForExcelInput input)
        {
            var productTypeFilter = input.ProductTypeFilter.HasValue
                ? (CommonConst.ProductType)input.ProductTypeFilter
                : default;
            var statusFilter = input.StatusFilter.HasValue
                ? (CommonConst.ProductStatus)input.StatusFilter
                : default;

            var filteredProducts = _productRepository.GetAll()
                .Include(e => e.CategoryFk)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    e => false || e.ProductCode.Contains(input.Filter) || e.ProductName.Contains(input.Filter) ||
                         e.Unit.Contains(input.Filter) || e.Image.Contains(input.Filter) ||
                         e.Description.Contains(input.Filter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProductCodeFilter),
                    e => e.ProductCode == input.ProductCodeFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter),
                    e => e.ProductName == input.ProductNameFilter)
                .WhereIf(input.ProductTypeFilter.HasValue && input.ProductTypeFilter > -1,
                    e => e.ProductType == productTypeFilter)
                .WhereIf(input.StatusFilter.HasValue && input.StatusFilter > -1, e => e.Status == statusFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.UnitFilter), e => e.Unit == input.UnitFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.CustomerSupportNoteFilter),
                    e => e.CustomerSupportNote.Contains(input.CustomerSupportNoteFilter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.UserManualNoteFilter),
                    e => e.UserManualNote.Contains(input.UserManualNoteFilter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.CategoryCategoryNameFilter),
                    e => e.CategoryFk != null && e.CategoryFk.CategoryName == input.CategoryCategoryNameFilter);

            var query = (from o in filteredProducts
                         join o1 in _lookup_categoryRepository.GetAll() on o.CategoryId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         select new GetProductForViewDto()
                         {
                             Product = new ProductDto
                             {
                                 ProductCode = o.ProductCode,
                                 ProductName = o.ProductName,
                                 Order = o.Order,
                                 ProductValue = o.ProductValue,
                                 ProductType = o.ProductType,
                                 Status = o.Status,
                                 Unit = o.Unit,
                                 Id = o.Id,
                                 CustomerSupportNote = o.CustomerSupportNote,
                                 UserManualNote = o.UserManualNote
                             },
                             CategoryCategoryName = s1 == null || s1.CategoryName == null ? "" : s1.CategoryName.ToString()
                         });

            var productListDtos = await query.ToListAsync();

            return _productsExcelExporter.ExportToFile(productListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_Products)]
        public async Task<List<ProductCategoryLookupTableDto>> GetAllCategoryForTableDropdown()
        {
            return await _lookup_categoryRepository.GetAllIncluding(x => x.ServiceFk)
                .Where(x => x.Status == CommonConst.CategoryStatus.Active)
                .Select(category => new ProductCategoryLookupTableDto
                {
                    Id = category.Id,
                    DisplayName = category == null || category.CategoryName == null
                        ? ""
                        : category.CategoryCode + "-" + category.CategoryName.ToString()
                }).ToListAsync();
        }

        [AbpAllowAnonymous]

        public async Task<ProductInformationDto> GetProductInfo(GetProductInfoInput input)
        {
            _logger.LogInformation($"Get Product info {input.CategoryCode}_{input.ProductCode}_{input.Amount}");

            var ressult = await _cacheManager.GetCache(CacheConst.ProductInfo).AsTyped<string, ProductInformationDto>().GetAsync(
                $"ProductInfo_{input.CategoryCode}_{input.ProductCode}_{input.Amount}",
                async () =>
                {
                    _logger.LogInformation($"Get Product info {input.CategoryCode}_{input.ProductCode}_{input.Amount} Cache not exist");
                    try
                    {
                        var query = _productRepository.GetAll()
                            .Include(x => x.CategoryFk)
                            .Where(x => x.CategoryFk.CategoryCode == input.CategoryCode
                                        && x.ProductCode == input.ProductCode
                                        && x.ProductValue == input.Amount);

                        var productInfo = query.Select(x => new ProductInformationDto
                        {
                            ProductCode = x.ProductCode,
                            ProductValue = x.ProductValue,
                            Status = (int)x.Status,
                            MinAmount = x.MinAmount,
                            MaxAmount = x.MaxAmount,
                        }).FirstOrDefault();

                        if (productInfo != null)
                        {
                            return productInfo;
                        }
                        else
                        {
                            //lấy thông tin Của Sản phẩm mệnh giá 1đ
                            var query1d = _productRepository.GetAll()
                                .Include(x => x.CategoryFk)
                                .Where(x => x.CategoryFk.CategoryCode == input.CategoryCode
                                            && x.ProductValue == 1);

                            var product1dInfo = query1d.Select(x => new ProductInformationDto
                            {
                                ProductCode = x.ProductCode,
                                ProductValue = x.ProductValue,
                                Status = (int)x.Status,
                                MinAmount = x.MinAmount,
                                MaxAmount = x.MaxAmount,
                            }).FirstOrDefault();
                            return product1dInfo;
                        }
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, $"Get Product info {input.CategoryCode}_{input.ProductCode}_{input.Amount}  err: {e.Message}");
                        return null;
                    }

                });
            _logger.LogInformation($"Get Product info {input.CategoryCode}_{input.ProductCode}_{input.Amount}  => {(ressult == null ? "Null" : ressult.ToJson())}");
            return ressult;
        }

    }
}
