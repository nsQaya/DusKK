using System.Threading.Tasks;
using TDV.MultiTenancy.Payments.Paypal;
using TDV.MultiTenancy.Payments.PayPal;
using TDV.MultiTenancy.Payments.PayPal.Dto;

namespace TDV.MultiTenancy.Payments
{
    public class PayPalPaymentAppService : TDVAppServiceBase, IPayPalPaymentAppService
    {
        private readonly PayPalGatewayManager _payPalGatewayManager;
        private readonly ISubscriptionPaymentRepository _subscriptionPaymentRepository;
        private readonly PayPalPaymentGatewayConfiguration _payPalPaymentGatewayConfiguration;

        public PayPalPaymentAppService(
            PayPalGatewayManager payPalGatewayManager,
            ISubscriptionPaymentRepository subscriptionPaymentRepository, 
            PayPalPaymentGatewayConfiguration payPalPaymentGatewayConfiguration)
        {
            _payPalGatewayManager = payPalGatewayManager;
            _subscriptionPaymentRepository = subscriptionPaymentRepository;
            _payPalPaymentGatewayConfiguration = payPalPaymentGatewayConfiguration;
        }

        public async Task ConfirmPayment(long paymentId, string paypalOrderId)
        {
            var payment = await _subscriptionPaymentRepository.GetAsync(paymentId);

            await _payPalGatewayManager.CaptureOrderAsync(
                new PayPalCaptureOrderRequestInput(paypalOrderId)
            );

            payment.Gateway = SubscriptionPaymentGatewayType.Paypal;
            payment.ExternalPaymentId = paypalOrderId;
            payment.SetAsPaid();
        }

        public PayPalConfigurationDto GetConfiguration()
        {
            return new PayPalConfigurationDto
            {
                ClientId = _payPalPaymentGatewayConfiguration.ClientId,
                DemoUsername = _payPalPaymentGatewayConfiguration.DemoUsername,
                DemoPassword = _payPalPaymentGatewayConfiguration.DemoPassword,
                DisabledFundings = _payPalPaymentGatewayConfiguration.DisabledFundings
            };
        }
    }
}