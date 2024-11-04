using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using HLS.Topup.Authorization.Users;
using HLS.Topup.AccountManagement.Dtos;
using HLS.Topup.Dto;

namespace HLS.Topup.AccountManagement
{
    public interface IAccountManagementAppService : IApplicationService
    {
        Task<PagedResultDto<UserProfileDto>> GetAllSubAgents(GetSubAgenstInput input);
        Task CreateOrEditSubAgent(CreateAccountDto input);

        Task CreateOrEditAgent(CreateAccountDto input);
        Task<UserProfileDto> GetAccount(EntityDto<long> input);
        Task<FileDto> GetAllSubAgentsListToExcel(GetSubAgenstInput input);
    }
}
