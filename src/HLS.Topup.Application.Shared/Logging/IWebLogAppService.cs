using Abp.Application.Services;
using HLS.Topup.Dto;
using HLS.Topup.Logging.Dto;

namespace HLS.Topup.Logging
{
    public interface IWebLogAppService : IApplicationService
    {
        GetLatestWebLogsOutput GetLatestWebLogs();

        FileDto DownloadWebLogs();
    }
}
