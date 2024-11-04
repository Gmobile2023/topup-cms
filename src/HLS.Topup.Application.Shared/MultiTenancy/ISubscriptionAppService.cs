using System.Threading.Tasks;
using Abp.Application.Services;

namespace HLS.Topup.MultiTenancy
{
    public interface ISubscriptionAppService : IApplicationService
    {
        Task DisableRecurringPayments();

        Task EnableRecurringPayments();
    }
}
