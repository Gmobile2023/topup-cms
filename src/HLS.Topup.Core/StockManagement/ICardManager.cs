using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HLS.Topup.Dtos.Stock;
using HLS.Topup.RequestDtos;

namespace HLS.Topup.StockManagement
{
    public interface ICardManager
    {
        Task<ResponseMessages> CardBatchCreateRequest(CardBatchCreateRequest input);
        Task<ResponseMessages> CardBatchUpdateRequest(CardBatchUpdateRequest input);

        Task<ApiResponseDto<List<CardBatchResponseDto>>> CardBatchGetListRequest(CardBatchGetListRequest input); 
        Task<ApiResponseDto<CardBatchResponseDto>> CardBatchGetRequest(CardBatchGetRequest input);

        Task<ResponseMessages> CardBatchDeleteRequest(CardBatchDeleteRequest input);

        Task<ApiResponseDto<CardResponseDto>> CardGetRequest(CardGetRequest input);

        Task<ApiResponseDto<CardResponseDto>> CardGetFullRequest(CardGetFullRequest input);
        Task<ApiResponseDto<List<CardResponseDto>>> CardGetListRequest(CardGetListRequest input);
        // Task<ApiResponseDto<List<CardRequestResponseDto>>> CardRequestGetListRequest(CardRequestGetListRequest input);
        Task<ResponseMessages> CardImportListRequest(CardImportListRequest input);
        Task<ResponseMessages> CardImportFileRequest(CardImportFileModel input);
        Task<NewMessageReponseBase<string>> CardImportApiRequest(CardImportApiRequest input);
        Task<ResponseMessages> CardImportRequest(CardImportRequest input);

        Task<ResponseMessages> CardStockCreateRequest(CardStockCreateRequest input);
        Task<ResponseMessages> CardStockUpdateRequest(CardStockUpdateRequest input);
        Task<ResponseMessages> CardStockTransferRequest(CardStockTransferRequest input);
        Task<ResponseMessages> CardsStockTransferRequest(Guid id);
        Task<ApiResponseDto<List<StockResponseDto>>> CardStockGetListRequest(CardStockGetListRequest input);
        Task<ApiResponseDto<StockResponseDto>> CardStockGetRequest(CardStockGetRequest input);


        Task<ResponseMessages> SimCreateRequest(SimCreateRequest input);
        Task<ResponseMessages> SimCreateManyRequest(SimCreateManyRequest input);
        Task<ResponseMessages> SimUpdateRequest(SimUpdateRequest input);
        Task<ApiResponseDto<List<SimResponseDto>>> SimGetListRequest(SimGetListRequest input);
        Task<ApiResponseDto<SimResponseDto>> SimGetRequest(SimGetRequest input);
        Task<ResponseMessages> CardUpdateRequest(CardUpdateRequest input);
        Task<ResponseMessages> CardUpdateStatusRequest(CardUpdateStatusRequest input);
        Task<ApiResponseDto<List<StockTransferItemInfoRespond>>> GetCardInfoTransferRequest(GetCardInfoTransferRequest input);
        Task<ResponseMessages> StockTransferRequest(StockTransferCardRequest input);

        Task<ApiResponseDto<List<StockTransRequestDto>>> CardStockTransListAsync(CardStockTransListRequest input);
        Task<ResponseMessages> CardStockUpdateQuantityRequest(CardStockUpdateQuantityRequest input);

        Task<NewMessageReponseBase<string>> StockCardApiCheckTransRequest(StockCardApiCheckTransRequest input);
    }
}