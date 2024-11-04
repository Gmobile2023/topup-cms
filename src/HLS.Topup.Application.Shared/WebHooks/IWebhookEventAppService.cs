using System.Threading.Tasks;
using Abp.Webhooks;

namespace HLS.Topup.WebHooks
{
    public interface IWebhookEventAppService
    {
        Task<WebhookEvent> Get(string id);
    }
}
