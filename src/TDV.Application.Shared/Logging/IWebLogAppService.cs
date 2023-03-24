using Abp.Application.Services;
using TDV.Dto;
using TDV.Logging.Dto;

namespace TDV.Logging
{
    public interface IWebLogAppService : IApplicationService
    {
        GetLatestWebLogsOutput GetLatestWebLogs();

        FileDto DownloadWebLogs();
    }
}
