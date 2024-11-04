using HLS.Topup.MultiTenancy.Payments;

namespace HLS.Topup.Web.Models.Payment
{
    public class CancelPaymentModel
    {
        public string PaymentId { get; set; }

        public SubscriptionPaymentGatewayType Gateway { get; set; }
    }
}