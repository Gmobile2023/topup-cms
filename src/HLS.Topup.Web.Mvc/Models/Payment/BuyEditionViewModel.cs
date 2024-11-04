using System.Collections.Generic;
using HLS.Topup.Editions;
using HLS.Topup.Editions.Dto;
using HLS.Topup.MultiTenancy.Payments;
using HLS.Topup.MultiTenancy.Payments.Dto;

namespace HLS.Topup.Web.Models.Payment
{
    public class BuyEditionViewModel
    {
        public SubscriptionStartType? SubscriptionStartType { get; set; }

        public EditionSelectDto Edition { get; set; }

        public decimal? AdditionalPrice { get; set; }

        public EditionPaymentType EditionPaymentType { get; set; }

        public List<PaymentGatewayModel> PaymentGateways { get; set; }
    }
}
