using HLS.Topup.Editions;
using HLS.Topup.Editions.Dto;
using HLS.Topup.MultiTenancy.Payments;
using HLS.Topup.Security;
using HLS.Topup.MultiTenancy.Payments.Dto;

namespace HLS.Topup.Web.Models.TenantRegistration
{
    public class TenantRegisterViewModel
    {
        public PasswordComplexitySetting PasswordComplexitySetting { get; set; }

        public int? EditionId { get; set; }

        public SubscriptionStartType? SubscriptionStartType { get; set; }

        public EditionSelectDto Edition { get; set; }

        public EditionPaymentType EditionPaymentType { get; set; }
    }
}
