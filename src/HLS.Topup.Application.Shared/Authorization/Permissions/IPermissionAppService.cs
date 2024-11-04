using Abp.Application.Services;
using Abp.Application.Services.Dto;
using HLS.Topup.Authorization.Permissions.Dto;

namespace HLS.Topup.Authorization.Permissions
{
    public interface IPermissionAppService : IApplicationService
    {
        ListResultDto<FlatPermissionWithLevelDto> GetAllPermissions();
    }
}
