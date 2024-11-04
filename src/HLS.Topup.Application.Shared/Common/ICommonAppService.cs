using System.Threading.Tasks;
using Abp.Application.Services;
using HLS.Topup.Dtos.Settings;
using Abp.Application.Services.Dto;

namespace HLS.Topup.Common
{
    public interface ICommonAppService : IApplicationService
    {
        Task<bool> CheckAccountActivities(CheckAccountActivityInput input);
        Task<object> AppSetting();
        Task<object> GetAppSetting();
        Task DeleteUser(EntityDto<long> input);
    }
}
