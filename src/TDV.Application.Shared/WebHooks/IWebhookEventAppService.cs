using System.Threading.Tasks;
using Abp.Webhooks;

namespace TDV.WebHooks
{
    public interface IWebhookEventAppService
    {
        Task<WebhookEvent> Get(string id);
    }
}
