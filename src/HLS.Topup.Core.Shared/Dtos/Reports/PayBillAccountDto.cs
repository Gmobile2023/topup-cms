using System;
using HLS.Topup.Common;

namespace HLS.Topup.Dtos.Reports
{
    public class PayBillAccountDto
    {
        public int? TenantId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? LastTransDate { get; set; }
        public string Description { get; set; }
        public string AccountCode { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string CategoryCode { get; set; }
        public string ServiceCode { get; set; }
        public string LastProviderCode { get; set; }
        public string LastTransCode { get; set; }
        public string InvoiceInfo { get; set; }
        public string InvoiceCode { get; set; }
        public string ExtraInfo { get; set; }
        public CommonConst.PayBillCustomerStatus Status { get; set; }
        public bool IsQueryBill { get; set; }
        public bool IsLastSuccess { get; set; }
        public decimal PaymentAmount { get; set; }
    }
}
