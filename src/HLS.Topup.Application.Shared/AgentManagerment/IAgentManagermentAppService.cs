using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using HLS.Topup.AgentsManage.Dtos;
using HLS.Topup.Dto;
using HLS.Topup.Dtos.Accounts;
using HLS.Topup.Dtos.Audit;

namespace HLS.Topup.AgentManagerment
{
    public interface IAgentManagermentAppService
    {
        Task<PagedResultDto<AgentsDto>> GetAll(
            GetAllAgentsInput input);
        Task CreateOrEdit(CreateOrEditSaleAssignAgentDto input);
        Task UnlockUser(BlockUnlockUserDto input);
        Task<MappingSaleView> GetSaleAssignAgent(int userAgentId);

        Task<AgentDetailView> GetAgentDetail(int userAgentId);
        Task<FileDto> GetAllListToExcel(GetAllAgentsInput input);
        Task BlockUser(BlockUnlockUserDto input);
        Task ResetOdp(ResetOdpInput input);

        Task ResendEmailTech(EntityDto<long> input);
        Task<PagedResultDto<AgentsSupperDto>> GetSupperAll(GetAllAgentSupperInput input);

        Task<AgentSupperDetailView> GetAgentSupperDetail(int userAgentId);
        Task UpdateUserName(UpdateUserNameInputDto input);
        Task CreateOrEditAgentPartner(CreateOrUpdateAgentPartnerInput input);

    }
}
