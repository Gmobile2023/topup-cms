using System.Collections.Generic;
using System.Threading.Tasks;
using HLS.Topup.Dtos.Accounts;
using HLS.Topup.Dtos.Balance;
using HLS.Topup.Dtos.Stock;
using HLS.Topup.Dtos.Transactions;
using HLS.Topup.RequestDtos;
using HLS.Topup.Topup;
using HLS.Topup.Topup.ResponseDto;
using HLS.Topup.Dtos.Bill;
using HLS.Topup.Dtos.Limitations;
using HLS.Topup.Dtos.PayBacks;
using HLS.Topup.Dtos.Provider;
using HLS.Topup.Dtos.Reports;
using HLS.Topup.Transactions.Dtos;

namespace HLS.Topup.Transactions
{
    public interface ITransactionManager
    {
        Task<TransactionResponse> DepositRequest(DepositRequest request);
        Task<TransactionResponse> TransferRequest(TransferRequest request);
        Task<ResponseMessageApi<decimal>> GetBalanceRequest(GetBalanceRequest request);
        Task<ApiResponseDto<List<TransactionReportDto>>> TransactionReportsGetRequest(TransactionReportsRequest request);
        Task<ApiResponseDto<List<BalanceHistoryResponseDto>>> BalanceHistoriesGetRequest(BalanceHistoriesRequest request);
        Task<ApiResponseDto<List<TransactionsHistoryResponseDTO>>> TransactionHistoriesGetRequest(TransactionsHistoryRequest request);
        Task<ApiResponseDto<List<TopupDetailResponseDTO>>> GetTopupDetailsRequest(GetTopupDetailRequest request);
        Task<TransactionResponse> GetDiscountAvailableRequest(GetDiscountAvailableRequest request);
        Task<ApiResponseDto<List<TopupDetailResponseDTO>>> GetTopupItemsRequest(TopupsListItemsRequest request);
        Task<ApiResponseDto<List<TopupRequestResponseDto>>> TopupListRequestAsync(TopupsListRequest input);
        Task<TopupRequestResponseDto> GetDetailsRequestAsync(GetSaleRequest input);
        Task<ResponseMessages> UpdateStatusRequestAsync(TopupsUpdateStatusRequest input);
        Task<NewMessageReponseBase<object>> TopupRequestAsync(TopupRequest input);
        Task<NewMessageReponseBase<string>> PayBatchRequestAsync(PayBatchRequest input);
        Task<ApiResponseDto<List<CheckChargesHistoryResponseDto>>> CheckChargesHistoryGetRequest(CheckChargesRequest request);
        Task<ApiResponseDto<List<CheckChargesDetaiResponselDto>>> CheckChargesDetailGetRequest(CheckChargesDetailRequest request);

        Task<ResponseMessages> TopupCancelRequestAsync(TopupCancelRequest input);
        Task<NewMessageReponseBase<BalanceResponseDto>> TransactionRefundRequestAsync(TransactionRefundRequest input);
        Task<NewMessageReponseBase<List<CardResponseDto>>> PinCodeAsync(CardSaleRequest input);
        Task<NewMessageReponseBase<InvoiceResultDto>> BillQueryRequestAsync(BillQueryRequest input);
        Task<NewMessageReponseBase<object>> PayBillRequestAsync(PayBillRequest input);
        Task<ResponseMessageApi<decimal>> GetLimitAmountBalance(GetAvailableLimitAccount input);

        Task<TransactionResponse> AdjustmentRequest(AdjustmentRequest request);

        Task<TransactionResponse> ClearDebtRequest(ClearDebtRequest request);
        Task<TransactionResponse> SaleDepositRequest(SaleDepositRequest request);
        Task<ResponseMessageApi<List<PaybatchAccount>>> PayBacksRequest(PaybatchRequest request);

        Task<ResponseMessageApi<AccountBalanceInfo>> GetBalanceAccountInfoRequest(AccountBalanceInfoCheckRequest request);
        Task<ApiResponseDto<AccountBalanceInfo>> BlockBalanceAsync(BlockBalanceRequest request);
        Task<ApiResponseDto<AccountBalanceInfo>> UnBlockBalanceAsync(UnBlockBalanceRequest request);

        Task<ResponseMessageApi<AccountProductLimitDto>> GetTotalPerDayProduct(GetTotalPerDayProductRequest input);
        Task<TransactionResponse> TransferSystemRequest(TransferSystemRequest request);
        Task<List<PayBillAccountDto>> GetSavePayBillRequest(GetSavePayBillRequest input);
        Task<int> GetTotalWaitingBillRequest(GetTotalWaitingBillRequest input);
        Task<ResponseMessageApi<object>> RemoveSavePayBillRequest(RemoveSavePayBillRequest input);
        Task<NewMessageReponseBase<ResponseProvider>> ProviderCheckTransRequest(ProviderCheckTransStatusRequest input);
        Task<NewMessageReponseBase<string>> CheckTransRequest(CheckTransStatusRequest input);
        Task<NewMessageReponseBase<string>> ProcessTopupRequest(TopupRequest request);
        Task<NewMessageReponseBase<string>> ProcessPinCodeRequest(CardSaleRequest request);
        Task<BillPaymentInfoDto> ProcessBillQueryRequest(BillQueryRequest input);
        Task<NewMessageReponseBase<string>> ProcessPayBillRequest(PayBillRequest input);

        Task<ApiResponseDto<List<BatchItemDto>>> GetBatchLotListRequest(
           BatchListGetRequest request);

        Task<ApiResponseDto<List<BatchDetailDto>>> GetBatchLotDetaiListLRequest(
         BatchDetailGetRequest request);

        Task<BatchItemDto> GetBatchSingleRequest(BatchSingleGetRequest request);
        Task<ResponseMessages> StopBatchLotAsync(BatchLotStopRequest input);

        Task<ApiResponseDto<List<SaleOffsetReponseDto>>> GetOffsetTopupListRequestAsync(OffsetTopupRequest input);

        Task<NewMessageReponseBase<string>> OffsetTopupRequestAsync(OffsetBuRequest input);

        Task<NewMessageReponseBase<string>> UpdateCardCodeRequest(UpdateCardCodeRequest input);
    }
}
