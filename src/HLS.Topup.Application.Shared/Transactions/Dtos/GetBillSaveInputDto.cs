using HLS.Topup.Common;

namespace HLS.Topup.Transactions.Dtos
{
    public class RemoveSavePayBillInput
    {
        public string ProductCode { get; set; }
        public string InvoiceCode { get; set; }
    }
    public class GetBillSaveInputDto
    {
        public string Search { get; set; }
        public CommonConst.PayBillCustomerStatus Status { get; set; }
        public string ProductCode { get; set; }
    }
    public class GetTotalWaitingBill
    {
    }
}
