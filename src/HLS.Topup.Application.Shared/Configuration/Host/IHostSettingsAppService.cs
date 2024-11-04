using System.Threading.Tasks;
using Abp.Application.Services;
using HLS.Topup.Configuration.Host.Dto;

namespace HLS.Topup.Configuration.Host
{
    public interface IHostSettingsAppService : IApplicationService
    {
        Task<HostSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(HostSettingsEditDto input);

        Task SendTestEmail(SendTestEmailInput input);
    }
}
