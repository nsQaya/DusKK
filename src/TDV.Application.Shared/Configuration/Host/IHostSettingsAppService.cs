using System.Threading.Tasks;
using Abp.Application.Services;
using TDV.Configuration.Host.Dto;

namespace TDV.Configuration.Host
{
    public interface IHostSettingsAppService : IApplicationService
    {
        Task<HostSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(HostSettingsEditDto input);

        Task SendTestEmail(SendTestEmailInput input);
    }
}
