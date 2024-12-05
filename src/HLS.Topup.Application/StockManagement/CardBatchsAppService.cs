using System;
using System.Collections.Generic;
using HLS.Topup.Common;
using System.Linq;
using System.Threading.Tasks;
using HLS.Topup.StockManagement.Exporting;
using HLS.Topup.StockManagement.Dtos;
using HLS.Topup.Dto;
using Abp.Application.Services.Dto;
using HLS.Topup.Authorization;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using HLS.Topup.Categories;
using HLS.Topup.Categories.Dtos;
using HLS.Topup.Products;
using HLS.Topup.Providers;
using HLS.Topup.RequestDtos;
using Microsoft.EntityFrameworkCore;
using ServiceStack;

namespace HLS.Topup.StockManagement
{
    [AbpAuthorize(AppPermissions.Pages_CardBatchs)]
    public class CardBatchsAppService : TopupAppServiceBase, ICardBatchsAppService
    {
        private readonly ICardBatchsExcelExporter _cardBatchsExcelExporter;
        private readonly ICardManager _cardManager;
        private readonly ICommonLookupAppService _commonSv;
        private readonly IRepository<Provider, int> _lookupProviderRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Category, int> _lookupCategoryRepository;


        public CardBatchsAppService(ICardBatchsExcelExporter cardBatchsExcelExporter, ICardManager cardManager,
            ICommonLookupAppService commonSv,
            IRepository<Category, int> lookupCategoryRepository,
            IRepository<Product> productRepository,
            IRepository<Provider, int> lookupProviderRepository)
        {
            _cardBatchsExcelExporter = cardBatchsExcelExporter;
            _cardManager = cardManager;
            _lookupCategoryRepository = lookupCategoryRepository;
            _lookupProviderRepository = lookupProviderRepository;
            _commonSv = commonSv;
            _productRepository = productRepository;
        }

        public async Task<PagedResultDto<CardBatchDto>> GetAll(GetAllCardBatchsInput input)
        {
            var request = new CardBatchGetListRequest
            {
                Filter = input.Filter,
                BatchCode = input.BatchCodeFilter,
                // BatchName = input.BatchNameFilter,
                // ProductCode = input.ProductCodeFilter,
                Status = input.StatusFilter,
                ImportType = input.ImportTypeFilter,
                FromDate = input.MinCreatedDateFilter,
                ToDate = input.MaxCreatedDateFilter,
                Provider = input.ProviderFilter,
                Vendor = input.VendorFilter,

                Limit = input.MaxResultCount,
                Offset = input.SkipCount,
                SearchType = SearchType.Search,
            };
            var rs = await _cardManager.CardBatchGetListRequest(request);
            var totalCount = rs.Total;
            if (rs.ResponseCode != "1")
                return new PagedResultDto<CardBatchDto>(
                    0, new List<CardBatchDto>()
                );
            var data = rs.Payload.ConvertTo<List<CardBatchDto>>();

            if (data.Any())
            {
                var pCodeList = data.Select(x => x.ProviderCode).ToList();
                var providerList = await _lookupProviderRepository.GetAll()
                    .Where(x => pCodeList.Contains(x.Code)).ToListAsync();
                // List<CategoryDto> allCategory = await _commonSv.GetCategoryUseCard();
                foreach (var item in data)
                {
                    var p = providerList.FirstOrDefault(x => x.Code == item.ProviderCode);
                    item.ProviderName = p != null ? p.Name : item.ProviderCode;
                    // var c = allCategory.FirstOrDefault(x=>x.CategoryCode == item.VendorCode);
                    // item.VendorName = c != null ? c.CategoryName : item.VendorCode;
                }
            }

            return new PagedResultDto<CardBatchDto>(
                totalCount, data
            );
        }

        public async Task<GetCardBatchForViewDto> GetCardBatchForView(Guid id)
        {
            var rs = await _cardManager.CardBatchGetRequest(new CardBatchGetRequest {Id = id});

            if (rs == null || rs.ResponseCode != "1")
                return new GetCardBatchForViewDto
                {
                    CardBatch = new CardBatchDto()
                };
            var data = new GetCardBatchForViewDto
            {
                CardBatch = rs.Payload.ConvertTo<CardBatchDto>()
            };

            if (!string.IsNullOrEmpty(data.CardBatch.ProviderCode))
            {
                var p = await _lookupProviderRepository.FirstOrDefaultAsync(x => x.Code == data.CardBatch.ProviderCode);
                if (p != null)
                {
                    data.ProviderName = p.Name;
                    data.CardBatch.ProviderName = p.Name;
                }
            }

            if (data.CardBatch.StockBatchItems.Any())
            {
                var prodsCode = data.CardBatch.StockBatchItems.Select(x => x.ProductCode);
                var prods = await _productRepository
                    .GetAllIncluding(x => x.CategoryFk)
                    .Include(x => x.CategoryFk.ServiceFk)
                    .Where(x => prodsCode.Contains(x.ProductCode))
                    .ToListAsync();
                foreach (var p in data.CardBatch.StockBatchItems)
                {
                    var prod = prods.FirstOrDefault(x => x.ProductCode == p.ProductCode);
                    p.ProductName = prod != null ? prod.ProductName : p.ProductCode;
                    p.CategoryCode = prod != null ? prod.CategoryFk.CategoryCode : "";
                    p.CategoryName = prod != null ? prod.CategoryFk.CategoryName : "";
                    p.ServiceCode = prod != null ? prod.CategoryFk.ServiceFk.ServiceCode : "";
                    p.ServiceName = prod != null ? prod.CategoryFk.ServiceFk.ServicesName : "";
                }
            }

            return data;
        }

        [AbpAuthorize(AppPermissions.Pages_CardBatchs_Edit)]
        public async Task<GetCardBatchForEditOutput> GetCardBatchForEdit(Guid id)
        {
            var rs = await _cardManager.CardBatchGetRequest(new CardBatchGetRequest {Id = id});

            if (rs == null || rs.ResponseCode != "1")
                return new GetCardBatchForEditOutput
                {
                    CardBatch = new CreateOrEditCardBatchDto()
                    {
                    }
                };
            var data = new GetCardBatchForEditOutput
            {
                CardBatch = rs.Payload.ConvertTo<CreateOrEditCardBatchDto>()
            };
            // if (!string.IsNullOrEmpty(data.CardBatch.VendorCode))
            // {
            //     var p = await _lookupCategoryRepository.FirstOrDefaultAsync(x => x.CategoryCode == data.CardBatch.VendorCode);
            //     if (p != null)
            //         data.VendorName = p.CategoryName;
            // }
            if (!string.IsNullOrEmpty(data.CardBatch.ProviderCode))
            {
                var p = await _lookupProviderRepository.FirstOrDefaultAsync(x => x.Code == data.CardBatch.ProviderCode);
                if (p != null)
                    data.ProviderName = p.Name;
            }

            if (data.CardBatch.StockBatchItems.Any())
            {
                var prodsCode = data.CardBatch.StockBatchItems.Select(x => x.ProductCode);
                var prods = await _productRepository
                    .GetAllIncluding(x => x.CategoryFk)
                    .Include(x => x.CategoryFk.ServiceFk)
                    .Where(x => prodsCode.Contains(x.ProductCode))
                    .ToListAsync();
                foreach (var p in data.CardBatch.StockBatchItems)
                {
                    var prod = prods.FirstOrDefault(x => x.ProductCode == p.ProductCode);
                    p.ProductName = prod != null ? prod.ProductName : p.ProductCode;
                    p.CategoryCode = prod != null ? prod.CategoryFk.CategoryCode : "";
                    p.CategoryName = prod != null ? prod.CategoryFk.CategoryName : "";
                    p.ServiceCode = prod != null ? prod.CategoryFk.ServiceFk.ServiceCode : "";
                    p.ServiceName = prod != null ? prod.CategoryFk.ServiceFk.ServicesName : "";
                }
            }

            return data;
        }

        public async Task CreateOrEdit(CreateOrEditCardBatchDto input)
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

        [AbpAuthorize(AppPermissions.Pages_CardBatchs_Create)]
        protected virtual async Task Create(CreateOrEditCardBatchDto input)
        {
            var rs = await _cardManager.CardBatchCreateRequest(input.ConvertTo<CardBatchCreateRequest>());
            if (rs.ResponseCode != "1")
                throw new UserFriendlyException(rs.ResponseMessage);
        }

        [AbpAuthorize(AppPermissions.Pages_CardBatchs_Edit)]
        protected virtual async Task Update(CreateOrEditCardBatchDto input)
        {
            var rs = await _cardManager.CardBatchUpdateRequest(input.ConvertTo<CardBatchUpdateRequest>());
            if (rs.ResponseCode != "1")
                throw new UserFriendlyException(rs.ResponseMessage);
        }

        [AbpAuthorize(AppPermissions.Pages_CardBatchs_Delete)]
        public async Task Delete(Guid id)
        {
            var rs = await _cardManager.CardBatchDeleteRequest(new CardBatchDeleteRequest {Id = id});
            if (rs.ResponseCode != "1")
                throw new UserFriendlyException(rs.ResponseMessage);
        }

        public async Task<FileDto> GetCardBatchsToExcel(GetAllCardBatchsForExcelInput input)
        {
            var request = new CardBatchGetListRequest
            {
                Filter = input.Filter,
                BatchCode = input.BatchCodeFilter,
                // BatchName = input.BatchNameFilter,
                // ProductCode = input.ProductCodeFilter,
                ImportType = input.ImportTypeFilter,
                Provider = input.ProviderFilter,
                Vendor = input.VendorFilter,

                Status = input.StatusFilter,
                FromDate = input.MinCreatedDateFilter,
                ToDate = input.MaxCreatedDateFilter,

                SearchType = SearchType.Export,
            };
            var rs = await _cardManager.CardBatchGetListRequest(request);

            if (rs == null || rs.ResponseCode != "1")
                return null;
            var data = rs.Payload.ConvertTo<List<CardBatchDto>>();
            if (data.Any())
            {
                var providerList = await _lookupProviderRepository.GetAll().ToListAsync();
                foreach (var item in data)
                {
                    var p = providerList.FirstOrDefault(x => x.Code == item.ProviderCode);
                    item.ProviderName = p != null ? p.Name : item.ProviderCode;
                }
            }

            return _cardBatchsExcelExporter.ExportToFile(data);
        }

        [AbpAuthorize(AppPermissions.Pages_CardBatchs)]
        public async Task<List<CardBatchProviderLookupTableDto>> GetAllProviderForTableDropdown()
        {
            return await _lookupProviderRepository.GetAll()
                .Select(provider => new CardBatchProviderLookupTableDto
                {
                    Id = provider == null || provider.Code == null
                        ? ""
                        : provider.Code.ToString(),
                    DisplayName = provider == null || provider.Name == null ? "" : provider.Name.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_CardBatchs)]
        public async Task<List<CardBatchVendorLookupTableDto>> GetAllVendorForTableDropdown()
        {
            List<CategoryDto> allCategory = await _commonSv.GetCategoryUseCard(false);
            return allCategory.Select(provider => new CardBatchVendorLookupTableDto
            {
                Id = provider == null || provider.CategoryCode == null
                    ? ""
                    : provider.CategoryCode.ToString(),
                DisplayName = provider == null || provider.CategoryName == null ? "" : provider.CategoryName.ToString()
            }).ToList();
        }

        [AbpAuthorize(AppPermissions.Pages_CardBatchs)]
        public async Task<List<CardBatchCategoryLookupTableDto>> GetAllCategoryForTableDropdown()
        {
            return await _lookupCategoryRepository.GetAll()
                .Select(category => new CardBatchCategoryLookupTableDto
                {
                    Id = category == null || category.CategoryCode == null
                        ? ""
                        : category.CategoryCode.ToString(),
                    DisplayName = category == null || category.CategoryName == null
                        ? ""
                        : category.CategoryName.ToString()
                }).ToListAsync();
        }
    }
}
