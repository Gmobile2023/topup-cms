using HLS.Topup.Common;

namespace HLS.Topup.Dtos.Settings
{
    public class CheckAccountActivityInput
    {
        public string CheckParam { get; set; }
        public string ProductCode { get; set; }
        public string CategoryCode { get; set; }
        public string ServiceCode { get; set; }
        public decimal Amount { get; set; }
        public decimal PaymentAmount { get; set; }
        public int Quantity { get; set; }
        public string CheckTypes { get; set; }
        public CommonConst.Channel Channel { get; set; }
    }
}
