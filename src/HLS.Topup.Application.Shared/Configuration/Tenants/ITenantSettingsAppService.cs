using System.Threading.Tasks;
using Abp.Application.Services;
using HLS.Topup.Configuration.Tenants.Dto;

namespace HLS.Topup.Configuration.Tenants
{
    public interface ITenantSettingsAppService : IApplicationService
    {
        Task<TenantSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(TenantSettingsEditDto input);

        Task ClearLogo();

        Task ClearCustomCss();
    }
}
