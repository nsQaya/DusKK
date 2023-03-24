using System.Threading.Tasks;
using Abp.Application.Services;
using TDV.Install.Dto;

namespace TDV.Install
{
    public interface IInstallAppService : IApplicationService
    {
        Task Setup(InstallDto input);

        AppSettingsJsonDto GetAppSettingsJson();

        CheckDatabaseOutput CheckDatabase();
    }
}