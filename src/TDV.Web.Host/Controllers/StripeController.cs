using TDV.MultiTenancy.Payments.Stripe;

namespace TDV.Web.Controllers
{
    public class StripeController : StripeControllerBase
    {
        public StripeController(
            StripeGatewayManager stripeGatewayManager,
            StripePaymentGatewayConfiguration stripeConfiguration,
            IStripePaymentAppService stripePaymentAppService) 
            : base(stripeGatewayManager, stripeConfiguration, stripePaymentAppService)
        {
        }
    }
}
