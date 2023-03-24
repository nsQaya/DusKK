using System.Threading.Tasks;
using Abp.Application.Services;
using TDV.MultiTenancy.Payments.Dto;
using TDV.MultiTenancy.Payments.Stripe.Dto;

namespace TDV.MultiTenancy.Payments.Stripe
{
    public interface IStripePaymentAppService : IApplicationService
    {
        Task ConfirmPayment(StripeConfirmPaymentInput input);

        StripeConfigurationDto GetConfiguration();

        Task<SubscriptionPaymentDto> GetPaymentAsync(StripeGetPaymentInput input);

        Task<string> CreatePaymentSession(StripeCreatePaymentSessionInput input);
    }
}