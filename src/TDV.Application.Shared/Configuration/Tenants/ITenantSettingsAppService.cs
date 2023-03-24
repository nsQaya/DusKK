using System.Threading.Tasks;
using Abp.Application.Services;
using TDV.Configuration.Tenants.Dto;

namespace TDV.Configuration.Tenants
{
    public interface ITenantSettingsAppService : IApplicationService
    {
        Task<TenantSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(TenantSettingsEditDto input);

        Task ClearLogo();

        Task ClearCustomCss();
    }
}
