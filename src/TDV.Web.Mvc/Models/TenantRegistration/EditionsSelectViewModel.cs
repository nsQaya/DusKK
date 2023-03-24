using Abp.AutoMapper;
using TDV.MultiTenancy.Dto;

namespace TDV.Web.Models.TenantRegistration
{
    [AutoMapFrom(typeof(EditionsSelectOutput))]
    public class EditionsSelectViewModel : EditionsSelectOutput
    {
    }
}
