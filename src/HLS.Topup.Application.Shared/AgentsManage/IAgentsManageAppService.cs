using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using HLS.Topup.AgentsManage.Dtos;

namespace HLS.Topup.AgentsManage
{
    public interface IAgentsManageAppService : IApplicationService 
    {
        Task<PagedResultDto<AgentsDto>> GetAll(GetAllAgentsInput input);
    }
}