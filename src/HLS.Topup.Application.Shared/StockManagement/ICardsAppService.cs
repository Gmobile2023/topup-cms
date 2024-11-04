using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using HLS.Topup.StockManagement.Dtos;
using HLS.Topup.Dto;
using System.Collections.Generic; 
using Abp;
using HLS.Topup.Products.Dtos;
using HLS.Topup.RequestDtos;
using HLS.Topup.Dtos.Stock;


namespace HLS.Topup.StockManagement
{
    public interface ICardsAppService : IApplicationService
    {
        Task<PagedResultDto<CardDto>> GetAll(GetAllCardsInput input);

        Task<GetCardForViewDto> GetCardForView(Guid id);

        Task<GetCardForViewDto> GetCardForViewFull(Guid id);

        Task<GetCardForEditOutput> GetCardForEdit(Guid id);

        Task CreateOrEdit(CreateOrEditCardDto input);

        Task Delete(Guid id);
        //Task ImportCards(CardImportListRequest input);
        Task ImportCardsJob(Guid fileObjectId, string provider, string dataStr, string description, UserIdentifier user);
        Task<ResponseMessages> GetCardImportList(List<CardImportItem> list);
        Task<NewMessageReponseBase<string>> ImportCardsApi(CardApiImportDto input);
        Task UpdateCardStatus(CardUpdateStatusRequest input);
        Task<FileDto> GetCardsToExcel(GetAllCardsForExcelInput input);
        
        List<decimal> CardValues();

        Task<List<CardProviderLookupTableDto>> GetAllProviderForTableDropdown();

        Task<List<CardVendorLookupTableDto>> GetAllVendorForTableDropdown();
        Task<List<CardBatchLookupTableDto>> GetAllCardBatchForTableDropdown();

        Task<List<ProductDto>>  GetProductByCategory(string categoryCode);

        Task<List<CommonLookupTableDto>> CategoryCardList(string serviceCode = null, bool isAll = true);
        Task<List<CommonLookupTableDto>> ProductCardList(string categoryCode = null, bool isAll = true);

        Task<PagedResultDto<StockTransRequestDto>> GetCardStockTransList(CardStockTransListInput input);
    }
}