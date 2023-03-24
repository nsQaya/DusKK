using System.Collections.Generic;
using TDV.Editions;
using TDV.Editions.Dto;
using TDV.MultiTenancy.Payments;
using TDV.MultiTenancy.Payments.Dto;

namespace TDV.Web.Models.Payment
{
    public class BuyEditionViewModel
    {
        public SubscriptionStartType? SubscriptionStartType { get; set; }

        public EditionSelectDto Edition { get; set; }

        public decimal? AdditionalPrice { get; set; }

        public EditionPaymentType EditionPaymentType { get; set; }

        public List<PaymentGatewayModel> PaymentGateways { get; set; }
    }
}
