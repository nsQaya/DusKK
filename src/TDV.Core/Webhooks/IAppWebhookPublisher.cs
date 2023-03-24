using System.Threading.Tasks;
using TDV.Authorization.Users;

namespace TDV.WebHooks
{
    public interface IAppWebhookPublisher
    {
        Task PublishTestWebhook();
    }
}
