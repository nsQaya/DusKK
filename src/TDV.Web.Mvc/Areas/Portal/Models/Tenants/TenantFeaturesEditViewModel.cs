using Abp.AutoMapper;
using TDV.MultiTenancy;
using TDV.MultiTenancy.Dto;
using TDV.Web.Areas.Portal.Models.Common;

namespace TDV.Web.Areas.Portal.Models.Tenants
{
    [AutoMapFrom(typeof (GetTenantFeaturesEditOutput))]
    public class TenantFeaturesEditViewModel : GetTenantFeaturesEditOutput, IFeatureEditViewModel
    {
        public Tenant Tenant { get; set; }
    }
}