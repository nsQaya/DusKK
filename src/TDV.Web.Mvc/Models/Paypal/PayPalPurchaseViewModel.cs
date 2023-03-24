using System.Linq;
using TDV.MultiTenancy.Payments.Paypal;

namespace TDV.Web.Models.Paypal
{
    public class PayPalPurchaseViewModel
    {
        public long PaymentId { get; set; }

        public string Description { get; set; }

        public decimal Amount { get; set; }

        public PayPalPaymentGatewayConfiguration Configuration { get; set; }

        public string GetDisabledFundingsQueryString()
        {
            if (Configuration.DisabledFundings == null || !Configuration.DisabledFundings.Any())
            {
                return "";
            }

            return "&disable-funding=" + string.Join(',', Configuration.DisabledFundings.ToList());
        }
    }
}
