using System.Threading.Tasks;
using Abp.Application.Services;
using HLS.Topup.MultiTenancy.Payments.PayPal.Dto;

namespace HLS.Topup.MultiTenancy.Payments.PayPal
{
    public interface IPayPalPaymentAppService : IApplicationService
    {
        Task ConfirmPayment(long paymentId, string paypalOrderId);

        PayPalConfigurationDto GetConfiguration();
    }
}
