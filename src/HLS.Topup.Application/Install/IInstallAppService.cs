using System.Threading.Tasks;
using Abp.Application.Services;
using HLS.Topup.Install.Dto;

namespace HLS.Topup.Install
{
    public interface IInstallAppService : IApplicationService
    {
        Task Setup(InstallDto input);

        AppSettingsJsonDto GetAppSettingsJson();

        CheckDatabaseOutput CheckDatabase();
    }
}