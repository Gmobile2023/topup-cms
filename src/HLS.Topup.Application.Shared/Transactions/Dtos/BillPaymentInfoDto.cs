using System.Collections.Generic;

namespace HLS.Topup.Transactions.Dtos
{
    public class BillPaymentInfoDto
    {
        public string FullName { get; set; }
        public string CustomerReference { get; set; }
        public string Address { get; set; }
        public decimal Amount { get; set; }
        public decimal Fee { get; set; }
        public decimal DisountAmount { get; set; }
        public decimal PaymentAmount { get; set; }
        public string Period { get; set; }
        public string PhoneNumber { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }

        public List<BillInfoPeriod> PeriodDetails { get; set; }
    }

    public class BillInfoPeriod
    {
        public string Period { get; set; }
        public decimal Amount { get; set; }
        public string BillNumber { get; set; }
        public string BillType { get; set; }
    }
}
