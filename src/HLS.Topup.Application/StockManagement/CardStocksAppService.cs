using HLS.Topup.Categories;
using System.Collections.Generic;
using HLS.Topup.Common;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using HLS.Topup.StockManagement.Exporting;
using HLS.Topup.StockManagement.Dtos;
using HLS.Topup.Dto;
using Abp.Application.Services.Dto;
using HLS.Topup.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Abp.UI;
using HLS.Topup.Categories.Dtos;
using HLS.Topup.Common.Dto;
using HLS.Topup.Products.Dtos;
using HLS.Topup.RequestDtos;
using HLS.Topup.Services.Dtos;
using Microsoft.EntityFrameworkCore;
using ServiceStack;

namespace HLS.Topup.StockManagement
{
    [AbpAuthorize(AppPermissions.Pages_CardStocks)]
    public class CardStocksAppService : TopupAppServiceBase, ICardStocksAppService
    {
        private readonly ICardStocksExcelExporter _cardStocksExcelExporter;
        private readonly ICardManager _cardManager;
        private readonly ICommonLookupAppService _commonSv;


        public CardStocksAppService(
            ICardStocksExcelExporter cardStocksExcelExporter,
            ICommonLookupAppService commonSv,
            ICardManager cardManager)
        {
            _cardStocksExcelExporter = cardStocksExcelExporter;
            _cardManager = cardManager;
            _commonSv = commonSv;
        }

        public async Task<PagedResultDtoReport<CardStockDto>> GetAll(GetAllCardStocksInput input)
        {
            var request = new CardStockGetListRequest
            {
                Limit = input.MaxResultCount,
                Offset = input.SkipCount,
                Status = input.StatusFilter,
                SearchType = SearchType.Search,
                Order = input.Sorting,
                Filter = input.Filter,
                StockCode = input.StockCodeFilter,
                ServiceCode = input.ServiceCodeFilter,
                CategoryCode = input.CategoryCodeFilter,
                ProductCode = input.ProductCodeFilter,
                MinCardValue = input.MinCardValueFilter ?? 0,
                MaxCardValue = input.MaxCardValueFilter ?? 0,
            };

            var rs = await _cardManager.CardStockGetListRequest(request);

            var totalCount = rs.Total;
            var sumList = rs.SumData.ConvertTo<List<CardStockDto>>();
            var sumData = sumList != null && sumList.Count >= 1 ? sumList[0] : new CardStockDto();
            if (rs.ResponseCode != "01")
                return new PagedResultDtoReport<CardStockDto>(
                    0,
                    new CardStockDto(),
                    new List<CardStockDto>()
                );
            var data = rs.Payload.ConvertTo<List<CardStockDto>>();
            if (data.Any())
            {
                var serviceCardList = await _commonSv.ServiceCardList();
                List<CategoryDto> categoryList = await _commonSv.GetCategoryUseCard(false);
                List<ProductInfoDto> productList = await _commonSv.GetProducts(new ProductSearchInput() { });

                foreach (var item in data)
                {
                    var c = categoryList.FirstOrDefault(x => x.CategoryCode == item.CategoryCode);
                    item.CategoryName = c != null ? c.CategoryName : item.CategoryCode;
                    var s = serviceCardList.FirstOrDefault(x => x.Id == item.ServiceCode);
                    item.ServiceName = s != null ? s.DisplayName : item.ServiceCode;
                    var p = productList.FirstOrDefault(x => x.ProductCode == item.ProductCode);
                    item.ProductName = p != null ? p.ProductName : item.ProductCode;
                }
            }
            return new PagedResultDtoReport<CardStockDto>(
                totalCount, totalData: sumData, data
            );
        }

        /// <summary>
        /// </summary>
        /// <param name="code"></param>
        /// <param name="productCode">productCode kho the</param>
        /// <param name="cardvalue"></param>
        /// <returns></returns>
        public async Task<GetCardStockForViewDto> GetCardStockForView(string code, string productCode,
            decimal cardValue)
        {
            var rs = await _cardManager.CardStockGetRequest(new CardStockGetRequest
            { StockCode = code, ProductCode = productCode, CardValue = cardValue });

            if (rs.ResponseCode != "01")
                return new GetCardStockForViewDto
                {
                    CardStock = new CardStockDto
                    {
                    }
                };

            var cardStock = rs.Payload.ConvertTo<CardStockDto>();

            var serviceCardList = await _commonSv.ServiceCardList();
            List<CategoryDto> categoryList = await _commonSv.GetCategoryUseCard(false);
            List<ProductInfoDto> productList = await _commonSv.GetProducts(new ProductSearchInput() { CategoryCode = cardStock.CategoryCode });

            var c = categoryList.FirstOrDefault(x => x.CategoryCode == cardStock.CategoryCode);
            cardStock.CategoryName = c != null ? c.CategoryName : cardStock.CategoryCode;
            var s = serviceCardList.FirstOrDefault(x => x.Id == cardStock.ServiceCode);
            cardStock.ServiceName = s != null ? s.DisplayName : cardStock.ServiceCode;
            var p = productList.FirstOrDefault(x => x.ProductCode == cardStock.ProductCode);
            cardStock.ProductName = p != null ? p.ProductName : cardStock.ProductCode;

            return new GetCardStockForViewDto
            {
                CardStock = cardStock
            };
        }

        [AbpAuthorize(AppPermissions.Pages_CardStocks_Edit)]
        public async Task<GetCardStockForEditOutput> GetCardStockForEdit(string code, string productCode,
            decimal cardValue) //Ham này sửa lại sẽ thay productCode = category
        {
            var rs = await _cardManager.CardStockGetRequest(new CardStockGetRequest
            { StockCode = code, ProductCode = productCode, CardValue = cardValue });

            if (rs.ResponseCode != "01")
                return new GetCardStockForEditOutput
                {
                    CardStock = new CreateOrEditCardStockDto
                    {
                    }
                };
            var cardStock = rs.Payload.ConvertTo<CreateOrEditCardStockDto>();

            var serviceCardList = await _commonSv.ServiceCardList();
            List<CategoryDto> categoryList = await _commonSv.GetCategoryUseCard(false);
            List<ProductInfoDto> productList = await _commonSv.GetProducts(new ProductSearchInput() { CategoryCode = cardStock.CategoryCode });

            var c = categoryList.FirstOrDefault(x => x.CategoryCode == cardStock.CategoryCode);
            cardStock.CategoryName = c != null ? c.CategoryName : cardStock.CategoryCode;
            var s = serviceCardList.FirstOrDefault(x => x.Id == cardStock.ServiceCode);
            cardStock.ServiceName = s != null ? s.DisplayName : cardStock.ServiceCode;
            var p = productList.FirstOrDefault(x => x.ProductCode == cardStock.ProductCode);
            cardStock.ProductName = p != null ? p.ProductName : cardStock.ProductCode;

            return new GetCardStockForEditOutput
            {
                CardStock = cardStock
            };
        }

        public async Task CreateOrEdit(CreateOrEditCardStockDto input)
        {
            if (input.ActionType == "Add")
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_CardStocks_Create)]
        private async Task<ResponseMessages> Create(CreateOrEditCardStockDto input)
        {
            return await _cardManager.CardStockCreateRequest(new CardStockCreateRequest
            {
                CategoryCode = input.CategoryCode,
                ServiceCode = input.ServiceCode,
                CardValue = input.CardValue,
                StockCode = input.StockCode,
                InventoryLimit = input.InventoryLimit,
                MinimumInventoryLimit = input.MinimumInventoryLimit,
                Description = input.Description,
            });
        }

        [AbpAuthorize(AppPermissions.Pages_CardStocks_Edit)]
        private async Task<ResponseMessages> Update(CreateOrEditCardStockDto input)
        {
            return await _cardManager.CardStockUpdateRequest(new CardStockUpdateRequest
            {
                InventoryLimit = input.InventoryLimit,
                MinimumInventoryLimit = input.MinimumInventoryLimit,
                StockCode = input.StockCode,
                ProductCode = input.ProductCode,
                CardValue = input.CardValue,
                Description = input.Description,
                CategoryCode = input.CategoryCode,
                ServiceCode = input.ServiceCode
            });
        }

        [AbpAuthorize(AppPermissions.Pages_CardStocks_EditQuantity)]
        public async Task<ResponseMessages> UpdateEditQuantity(EditQuantityStockDto input)
        {
            return await _cardManager.CardStockUpdateQuantityRequest(new CardStockUpdateQuantityRequest
            {
                StockCode = input.StockCode,
                KeyCode = input.KeyCode,
                Inventory = input.Inventory,
            });
        }


        [AbpAuthorize(AppPermissions.Pages_CardStocks_Delete)]
        public async Task Delete(EntityDto input)
        {
        }

        public async Task<FileDto> GetCardStocksToExcel(GetAllCardStocksForExcelInput input)
        {
            var request = new CardStockGetListRequest
            {
                Limit = 0,
                Offset = 0,
                SearchType = SearchType.Export,
                Status = input.StatusFilter,
                Filter = input.Filter,
                StockCode = input.StockCodeFilter,
                ServiceCode = input.ServiceCodeFilter,
                CategoryCode = input.CategoryCodeFilter,
                ProductCode = input.ProductCodeFilter,
                MinCardValue = input.MinCardValueFilter ?? 0,
                MaxCardValue = input.MaxCardValueFilter ?? 0,
            };

            var rs = await _cardManager.CardStockGetListRequest(request);
            if (rs.ResponseCode != "01")
                _cardStocksExcelExporter.ExportToFile(new List<CardStockDto>());

            var data = rs.Payload.ConvertTo<List<CardStockDto>>();
            if (data.Any())
            {
                var serviceCardList = await _commonSv.ServiceCardList();
                List<CategoryDto> categoryList = await _commonSv.GetCategoryUseCard(false);
                List<ProductInfoDto> productList = await _commonSv.GetProducts(new ProductSearchInput() { });

                foreach (var item in data)
                {
                    var c = categoryList.FirstOrDefault(x => x.CategoryCode == item.CategoryCode);
                    item.CategoryName = c != null ? c.CategoryName : item.CategoryCode;
                    var s = serviceCardList.FirstOrDefault(x => x.Id == item.ServiceCode);
                    item.ServiceName = s != null ? s.DisplayName : item.ServiceCode;
                    var p = productList.FirstOrDefault(x => x.ProductCode == item.ProductCode);
                    item.ProductName = p != null ? p.ProductName : item.ProductCode;
                }
            }

            return _cardStocksExcelExporter.ExportToFile(data);
        }

        [AbpAuthorize(AppPermissions.Pages_CardStocks_Edit)]
        public async Task<TransferCardStockDto> GetTransferStock(Guid id)
        {
            var rs = await _cardManager.CardsStockTransferRequest(id);
            if (rs.ResponseCode != "01")
                return null;
            return rs.Payload.ConvertTo<TransferCardStockDto>();
        }

        [AbpAuthorize(AppPermissions.Pages_CardStocks_Transfer)]
        public async Task<ResponseMessages> TransferStock(TransferCardStockDto input)
        {
            var rs = await _cardManager.CardStockTransferRequest(input.ConvertTo<CardStockTransferRequest>());
            return rs.ConvertTo<ResponseMessages>();
        }

        public async Task<List<CommonLookupTableDto>> StockCodes()
        {
            var data = new List<CommonLookupTableDto>();
            data.Add(new CommonLookupTableDto() { Id = "STOCK_TEMP", DisplayName = "STOCK_TEMP" });
            //data.Add(new CommonLookupTableDto() {Id = "STOCK_ACTIVE", DisplayName = "STOCK_ACTIVE"});
            data.Add(new CommonLookupTableDto() { Id = "STOCK_SALE", DisplayName = "STOCK_SALE" });
            //data.Add(new CommonLookupTableDto() {Id = "STOCK_MAPPING_INVALID", DisplayName = "STOCK_MAPPING_INVALID"});
            return data;
        }

        public async Task<List<StockTransferItemInfo>> GetCardInfoTransfer(GetCardInfoTransferInput request)
        {
            var data = await _cardManager.GetCardInfoTransferRequest(request.ConvertTo<GetCardInfoTransferRequest>());
            if (data.ResponseCode != "01")
                return new List<StockTransferItemInfo>();
            var prodList = data.Payload.ConvertTo<List<StockTransferItemInfo>>();

            var serviceCardList = await _commonSv.ServiceCardList();
            List<CategoryDto> categoryList = await _commonSv.GetCategoryUseCard(false);
            List<ProductInfoDto> productList = await _commonSv.GetProducts(new ProductSearchInput() { CategoryCode = request.CategoryCode });

            foreach (var item in prodList)
            {
                var c = categoryList.FirstOrDefault(x => x.CategoryCode == item.CategoryCode);
                item.CategoryName = c != null ? c.CategoryName : item.CategoryCode;
                var s = serviceCardList.FirstOrDefault(x => x.Id == item.ServiceCode);
                item.ServiceName = s != null ? s.DisplayName : item.ServiceCode;
                var p = productList.FirstOrDefault(x => x.ProductCode == item.ProductCode);
                item.ProductName = p != null ? p.ProductName : item.ProductCode;
            }
            return prodList;
        }

        [AbpAuthorize(AppPermissions.Pages_CardStocks_Transfer)]
        public async Task<ResponseMessages> StockTransferRequest(StockTransferInput input)
        {
            var rs = await _cardManager.StockTransferRequest(input.ConvertTo<StockTransferCardRequest>());
            var response = rs.ConvertTo<ResponseMessages>();
            if (response.ResponseMessage == "Inventory is not enough")
            {
                response.ResponseMessage = L(response.ResponseMessage);
            }
            return response;
        }
    }
}
