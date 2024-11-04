using HLS.Topup.Common;

namespace HLS.Topup.Dtos.Common
{
    public class PaymentMethodDto
    {
        public CommonConst.Channel Channel { get; set; }
        public CommonConst.VerifyTransType Type { get; set; }
    }

    public class GetPaymentMothodInput
    {
        public CommonConst.Channel Channel { get; set; }
    }
}
