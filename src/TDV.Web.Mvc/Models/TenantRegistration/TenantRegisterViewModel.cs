using TDV.Editions;
using TDV.Editions.Dto;
using TDV.MultiTenancy.Payments;
using TDV.Security;
using TDV.MultiTenancy.Payments.Dto;

namespace TDV.Web.Models.TenantRegistration
{
    public class TenantRegisterViewModel
    {
        public PasswordComplexitySetting PasswordComplexitySetting { get; set; }

        public int? EditionId { get; set; }

        public SubscriptionStartType? SubscriptionStartType { get; set; }

        public EditionSelectDto Edition { get; set; }

        public EditionPaymentType EditionPaymentType { get; set; }
    }
}
