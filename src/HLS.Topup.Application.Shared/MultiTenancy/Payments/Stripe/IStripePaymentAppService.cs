using System.Threading.Tasks;
using Abp.Application.Services;
using HLS.Topup.MultiTenancy.Payments.Dto;
using HLS.Topup.MultiTenancy.Payments.Stripe.Dto;

namespace HLS.Topup.MultiTenancy.Payments.Stripe
{
    public interface IStripePaymentAppService : IApplicationService
    {
        Task ConfirmPayment(StripeConfirmPaymentInput input);

        StripeConfigurationDto GetConfiguration();

        Task<SubscriptionPaymentDto> GetPaymentAsync(StripeGetPaymentInput input);

        Task<string> CreatePaymentSession(StripeCreatePaymentSessionInput input);
    }
}