using System.Threading.Tasks;
using Abp.Application.Services;
using HLS.Topup.Sessions.Dto;

namespace HLS.Topup.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();

        Task<UpdateUserSignInTokenOutput> UpdateUserSignInToken();
    }
}
