using ServiceStack;
using System;
using System.Collections.Generic;
using HLS.Topup.Common;
using HLS.Topup.Dtos.Bill;
using HLS.Topup.Dtos.PayBacks;
using ServiceStack.DataAnnotations;

namespace HLS.Topup.RequestDtos
{
    [Route("/api/v1/balance/deposit")]
    public class DepositRequest : IUserInfoRequest
    {
        public string CurrencyCode { get; set; }
        public string AccountCode { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public string TransRef { get; set; }
        public string TransNote { get; set; }
        public string PartnerCode { get; set; }
        public string ParentCode { get; set; }
        public string StaffAccount { get; set; }
        public string StaffUser { get; set; }
        public CommonConst.SystemAccountType AccountType { get; set; }
        public CommonConst.AgentType AgentType { get; set; }

        public string ExtraInfo { get; set; }
    }

    [Route("/api/v1/balance/adjustment", "POST")]
    public class AdjustmentRequest
    {
        public string AccountCode { get; set; }
        public string CurrencyCode { get; set; }
        public decimal Amount { get; set; }
        public string TransRef { get; set; }
        public string TransNote { get; set; }
        public CommonConst.AdjustmentType AdjustmentType { get; set; }
    }

    [Route("/api/v1/balance/transfer")]
    public class TransferRequest : IUserInfoRequest
    {
        public decimal Amount { get; set; }
        public string SrcAccount { get; set; }
        public string CurrencyCode { get; set; }
        public string DesAccount { get; set; }
        public string TransRef { get; set; }
        public string Description { get; set; }
        public string TransNote { get; set; }
        public string PartnerCode { get; set; }
        public string ParentCode { get; set; }
        public string StaffAccount { get; set; }
        public string StaffUser { get; set; }
        public CommonConst.SystemAccountType AccountType { get; set; }
        public CommonConst.AgentType AgentType { get; set; }
    }

    [Route("/api/v1/balance/check")]
    public class GetBalanceRequest : IUserInfoRequest
    {
        public string AccountCode { get; set; }
        public string CurrencyCode { get; set; }
        public string PartnerCode { get; set; }
        public string ParentCode { get; set; }
        public string StaffAccount { get; set; }
        public string StaffUser { get; set; }
        public CommonConst.SystemAccountType AccountType { get; set; }
        public CommonConst.AgentType AgentType { get; set; }
    }

    [Route("/api/v1/balance/report/transactions", "GET")]
    public class TransactionReportsRequest : PaggingBaseDto, IUserInfoRequest
    {
        public CommonConst.TransactionType? TransType { get; set; }
        public string TransCode { get; set; }
        public string TransRef { get; set; }
        public string SrcAccount { get; set; }
        public string DesAccount { get; set; }
        public string AccountTrans { get; set; }
        public string TransNote { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string PartnerCode { get; set; }
        public string ParentCode { get; set; }
        public string StaffAccount { get; set; }
        public string StaffUser { get; set; }
        public CommonConst.SystemAccountType AccountType { get; set; }
        public CommonConst.AgentType AgentType { get; set; }
    }

    [Route("/api/v1/balance/report/transaction/{TransCode}", "GET")]
    public class TransactionReportRequest
    {
        public string TransCode { get; set; }
    }


    [Route("/api/v1/balance/report/balanceHistories", "GET")]
    public class BalanceHistoriesRequest : PaggingBaseDto
    {
        public CommonConst.TransactionType? TransType { get; set; }
        public string TransCode { get; set; }
        public string TransRef { get; set; }
        public string SrcAccount { get; set; }
        public string DesAccount { get; set; }
        public string TransNote { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string AccountTrans { get; set; }
    }

    [Route("/api/v1/balance/report/balanceHistory/{TransCode}", "GET")]
    public class BalanceHistoryRequest
    {
        public string TransCode { get; set; }
    }

    [Route("/api/v1/balance/report/transactionHistories", "GET")]
    public class TransactionsHistoryRequest : PaggingBaseDto
    {
        public CommonConst.TransactionType? TransType { get; set; }
        public CommonConst.TransStatus? TransStatus { get; set; }
        public string TransCode { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string AccountTrans { get; set; }
        public string SrcAccount { get; set; }
    }

    [Route("/api/v1/backend/topup/history", "GET")]
    public class GetTopupDetailRequest : PaggingBaseDto
    {
        public string TransCode { get; set; }
    }

    [Route("/api/v1/backend/topup/getTopupItems")]
    public class TopupsListItemsRequest : PaggingBaseDto
    {
        public virtual string MobileNumber { get; set; }
        public virtual string TransRef { get; set; }
        public virtual string TransCode { get; set; }
        public virtual byte Status { get; set; }
        public virtual DateTime? FromDate { get; set; }
        public virtual DateTime? ToDate { get; set; }
        public string ServiceCode { get; set; }
        public string CategoryCode { get; set; }
        public string ProductCode { get; set; }
        public string PartnerCode { get; set; }
        public string Telco { get; set; }
        public string TopupTransactionType { get; set; }
        public string WorkerApp { get; set; }
        public string Serial { get; set; }
    }

    [Route("/api/v1/backend/discount/levelDiscounts", "GET")]
    public class GetLevelDiscountsRequest : PaggingBaseDto
    {
        public string TransRef { get; set; }
        public string TransCode { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public CommonConst.LevelDiscountStatus Status { get; set; }
        public string TransAccount { get; set; }
        public string RefAccount { get; set; }
        public string Search { get; set; }
        public string AccountCode { get; set; }
    }

    [Route("/api/v1/backend/discount/discountAvailable", "GET")]
    public class GetDiscountAvailableRequest
    {
        public string AccountCode { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }

    [Route("/api/v1/backend/discount/collectDiscount", "POST")]
    public class CollectDiscountRequest
    {
        public string TransRef { get; set; }
        public string TransCode { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string TransAccount { get; set; }
        public string RefAccount { get; set; }
        public string Search { get; set; }
        public string AccountCode { get; set; }
    }

    [Route("/api/v1/gateway/bill_query", "GET")]
    public class BillQueryRequest : IUserInfoRequest
    {
        public string ReceiverInfo { get; set; }
        public string ProductCode { get; set; }
        public string ServiceCode { get; set; }
        public string CategoryCode { get; set; }
        public string PartnerCode { get; set; }
        public string ParentCode { get; set; }
        public string StaffAccount { get; set; }
        public string StaffUser { get; set; }
        public CommonConst.SystemAccountType AccountType { get; set; }
        public CommonConst.AgentType AgentType { get; set; }
        public bool IsInvoice { get; set; }
        public decimal Amount { get; set; }
        public bool IsCheckAmount { get; set; }
    }

    [Route("/api/v1/gateway/pay_bill", "POST")]
    public class PayBillRequest : IUserInfoRequest
    {
        public string ReceiverInfo { get; set; }
        public decimal Amount { get; set; }
        public string TransCode { get; set; }
        public string PartnerCode { get; set; }
        public string ParentCode { get; set; }
        public string StaffAccount { get; set; }
        public string StaffUser { get; set; }
        public string CategoryCode { get; set; }
        public string ServiceCode { get; set; }
        public string ProductCode { get; set; }
        public InvoiceBillDto InvoiceInfo { get; set; }
        public string ExtraInfo { get; set; }
        public bool IsSaveBill { get; set; }
        public CommonConst.SystemAccountType AccountType { get; set; }
        public CommonConst.AgentType AgentType { get; set; }
        public CommonConst.Channel Channel { get; set; }
    }

    [Route("/api/v1/common/transaction/save-bill/remove", "DELETE")]
    public class RemoveSavePayBillRequest
    {
        public string AccountCode { get; set; }
        public string ProductCode { get; set; }
        public string InvoiceCode { get; set; }
    }

    [Route("/api/v1/common/transaction/save-bill/get-bill", "GET")]
    public class GetSavePayBillRequest
    {
        public string AccountCode { get; set; }
        public string Search { get; set; }
        public int Status { get; set; }
        public string ProductCode { get; set; }
    }
    [Route("/api/v1/common/transaction/save-bill/total-waiting-bill", "GET")]
    public class GetTotalWaitingBillRequest
    {
        public string AccountCode { get; set; }
    }

    [Route("/api/v1/balance/clear-debt", "POST")]
    public class ClearDebtRequest : IPost, IReturn<TransactionResponse>
    {
        public string AccountCode { get; set; }
        public decimal Amount { get; set; }
        public string TransRef { get; set; }
        public string TransNote { get; set; }
    }

    [Route("/api/v1/balance/sale-deposit", "POST")]
    public class SaleDepositRequest : IPost, IReturn<TransactionResponse>
    {
        public string AccountCode { get; set; }

        public string SaleCode { get; set; }

        public decimal Amount { get; set; }
        public string TransRef { get; set; }
        public string TransNote { get; set; }
    }

    [Route("/api/v1/balance/paybatch", "POST")]
    public class PaybatchRequest
    {
        public List<PaybatchAccount> Accounts { get; set; }
        public string CurrencyCode { get; set; }
        public string TransRef { get; set; }
        public string TransNote { get; set; }
    }

    [Route("/api/v1/balance/checkBalanceInfo", "GET")]
    public class AccountBalanceInfoCheckRequest
    {
        public string AccountCode { get; set; }
        public string CurrencyCode { get; set; }
    }

    [Route("/api/v1/balance/block", "POST")]
    public class BlockBalanceRequest
    {
        [Required] public string AccountCode { get; set; }
        [Required] public string CurrencyCode { get; set; }
        [Required] public decimal BlockAmount { get; set; }
        [Required] public string TransRef { get; set; }
        [Required] public string TransNote { get; set; }
    }

    [Route("/api/v1/balance/unblock", "POST")]
    public class UnBlockBalanceRequest
    {
        [Required] public string AccountCode { get; set; }
        [Required] public string CurrencyCode { get; set; }
        [Required] public decimal UnBlockAmount { get; set; }
        [Required] public string TransRef { get; set; }
        [Required] public string TransNote { get; set; }
    }

    [Route("/api/v1/balance/transfer-system")]
    public class TransferSystemRequest
    {
        public decimal Amount { get; set; }
        public string SrcAccount { get; set; }
        public string CurrencyCode { get; set; }
        public string DesAccount { get; set; }
        public string TransRef { get; set; }
        public string TransNote { get; set; }
    }

    [Route("api/v1/topupgate/check_trans", "GET")]
    public class ProviderCheckTransStatusRequest
    {
        public string TransCodeToCheck { get; set; }
        public string ServiceCode { get; set; }
        public string ProviderCode { get; set; }
    }
    [Route("/api/v1/gateway/transactions/checktrans", "GET")]
    public class CheckTransStatusRequest
    {
        public string TransCodeToCheck { get; set; }
        public string TransCode { get; set; }
        public string PartnerCode { get; set; }
    }

    [Route("/api/v1/backend/update_card_code", "POST")]
    public class UpdateCardCodeRequest
    {
        public string TransCode { get; set; }
    }

    public class BalanceResponse
    {
        public decimal SrcBalance { get; set; }
        public decimal DesBalance { get; set; }
        public string TransactionCode { get; set; }
    }
}
