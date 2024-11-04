using System.Threading.Tasks;
using HLS.Topup.Authorization.Users;

namespace HLS.Topup.WebHooks
{
    public interface IAppWebhookPublisher
    {
        Task PublishTestWebhook();
    }
}
