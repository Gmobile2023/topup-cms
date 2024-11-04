using System.Collections.Generic;
using HLS.Topup.Editions.Dto;
using HLS.Topup.MultiTenancy.Payments;

namespace HLS.Topup.Web.Models.Payment
{
    public class ExtendEditionViewModel
    {
        public EditionSelectDto Edition { get; set; }

        public List<PaymentGatewayModel> PaymentGateways { get; set; }
    }
}