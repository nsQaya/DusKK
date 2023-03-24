using Abp.Application.Services.Dto;
using Abp.Webhooks;
using TDV.WebHooks.Dto;

namespace TDV.Web.Areas.Portal.Models.Webhooks
{
    public class CreateOrEditWebhookSubscriptionViewModel
    {
        public WebhookSubscription WebhookSubscription { get; set; }

        public ListResultDto<GetAllAvailableWebhooksOutput> AvailableWebhookEvents { get; set; }
    }
}
