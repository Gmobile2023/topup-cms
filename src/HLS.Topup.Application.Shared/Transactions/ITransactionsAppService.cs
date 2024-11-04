using System.Collections.Generic;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Services.Dto;
using HLS.Topup.Categories.Dtos;
using HLS.Topup.Dto;
using HLS.Topup.Dtos.Bill;
using HLS.Topup.Dtos.Reports;
using HLS.Topup.Dtos.Transactions;
using HLS.Topup.Products.Dtos;
using HLS.Topup.RequestDtos;
using HLS.Topup.Topup.Dtos;
using HLS.Topup.Transactions.Dtos;
using HLS.Topup.Vendors.Dtos;

namespace HLS.Topup.Transactions
{
    public interface ITransactionsAppService
    {
        Task<TransactionResponse> TransferMoney(TransferRequest input);
        Task<decimal> GetBalance(GetBalanceRequest input);
        Task<PagedResultDto<BalanceHistoryDto>> GetBalanceHistories(GetBalanceHistoryInput input);
        Task<decimal> GetLimitAmountBalance(GetAvailableLimitAccount input);
        Task<PagedResultDto<TopupRequestResponseDto>> GetTransactionHistories(GetAllTopupRequestsInput input);
        Task<TopupRequestResponseDto> GetTransactionByCode(string transactionCode);

        Task<TransactionResponseDto> GetTransaction(string type, string code);
        Task<PagedResultDto<TopupDetailResponseDTO>> GetTopupDetailRequest(GetAllTopupDetailRequestsInput input);
        Task<decimal> GetDiscountAvailable(GetDiscountAvailableRequest request);
        Task<FileDto> GetTransactionHistoryToExcel(GetAllTopupRequestsForExcelInput input);
        Task<FileDto> GetTransactionDetailHistoryToExcel(GetAllTopupDetailRequestsInput input);
        Task<FileDto> GetTransactionHistoryUserToExcel(GetAllTopupRequestsForExcelInput input);


        Task<ResponseMessages> CreateTopupRequest(CreateOrEditTopupRequestDto input);
        Task<ResponseMessages> CreateTopupListRequest(TopupListRequestDto input);

        Task<ResponseMessages> CreatePinCodeRequest(CreateOrEditPinCodeRequestDto input);
        Task<BillPaymentInfoDto> BillQueryRequest(BillQueryRequest input);
        Task<ResponseMessages> PayBillRequest(PayBillRequest input);
        Task<List<TopupDetailResponseDTO>> GetTransactionDetail(string transCode);
        Task<List<VendorDto>> GetVendors();
        Task<ResponseMessages> UpdateStatus(TopupsUpdateStatusRequest request);
        Task RefundTransactionRequest(RefundTransDto input);

        Task<List<TransactionsProviderLookupTableDto>> GetAllProviderForTableDropdown();
        Task<List<TransactionsServiceLookupTableDto>> GetAllServiceForTableDropdown();
        Task<List<CategoryDto>> GetCategoryByServiceCode(string serviceCode);
        Task<List<ProductInfoDto>> GetProducts(List<int?> cateIds = null);

        Task<List<ProductInfoDto>> GetProductsMuti(List<string> cateCodes);

        Task RemoveSavePayBill(RemoveSavePayBillInput request);
        Task<List<PayBillAccountDto>> GetSavePayBills(GetBillSaveInputDto request);

        Task<PagedResultDto<TopupDetailResponseDTO>> GetListTopupDetailRequest(
            GetAllTopupDetailRequestsInput input);

        Task<FileDto> GetListTopupDetailRequestToExcel(GetListTopupDetailRequestForExcelInput input);
        Task<int> GetTotalWaitingBill(GetTotalWaitingBill request);

        Task<NewMessageReponseBase<string>> CheckTransRequest(CheckTransStatusRequest input);
        Task<ResponseMessages> CheckTransProvider(ProviderCheckTransStatusRequest input);
        Task<NewMessageReponseBase<string>> Topup(CreateOrEditTopupRequestDto input);
        Task<NewMessageReponseBase<string>> PinCode(CreateOrEditPinCodeRequestDto input);
        Task<BillPaymentInfoDto> BillQuery(BillQueryRequest input);
        Task<NewMessageReponseBase<string>> PayBill(PayBillRequest input);
        Task<TransactionResponse> Transfer(TransferRequest input);

        Task<PagedResultDtoReport<BatchItemDto>> GetBatchLotList(
            BatchListGetInput input);

        Task<BatchItemDto> GetBatchLotSingle(BatchSingleInput input);
        Task<PagedResultDtoReport<BatchDetailDto>> GetBatchLotDetailList(
            BatchDetailGetInput input);

        Task<ResponseMessages> BatchLotStopRequest(
           BatchLotStopInput input);
        
        Task<FileDto> ZipCards(string transCode);

        Task<FileDto> GetBatchLotExportToFile(BatchListGetInput input);

        Task<FileDto> GetBatchLotTopupDetailExportToFile(BatchDetailGetInput input);

        Task<FileDto> GetBatchLotBillDetailExportToFile(BatchDetailGetInput input);

        Task<FileDto> GetBatchLotPinCodeDetailExportToFile(BatchDetailGetInput input);

        Task<PagedResultDto<SaleOffsetReponseDto>> GetOffsetTopupHistoryRequest(GetAllOffsetTopupRequestsInput input);

        Task<FileDto> GetOffsetTopupHistoryToExcel(GetAllOffsetTopupRequestsInput input);

        Task OffsetBuRequest(OffsetBuRequest input);

        Task ProcessSyncStatusTransactionJob(UserIdentifier user, string data);
    }
}
