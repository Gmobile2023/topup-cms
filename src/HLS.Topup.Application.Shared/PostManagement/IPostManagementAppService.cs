using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using HLS.Topup.PostManagement.Dtos;

namespace HLS.Topup.PostManagement
{
    public interface IPostManagementAppService
    {
        Task<PagedResultDto<PostManagementDto>> GetAll(GetPostsInput input);
        Task CreateOrEdit(PostManagementDto input);
        Task<PostManagementDto> GetAgentDetail(long userId);
    }
}
