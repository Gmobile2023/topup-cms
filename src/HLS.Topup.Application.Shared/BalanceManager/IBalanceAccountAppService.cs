using System.Threading.Tasks;
using Abp.Application.Services;
using HLS.Topup.Dtos.Balance;
using HLS.Topup.RequestDtos;

namespace HLS.Topup.BalanceManager
{
    public interface IBalanceAccountAppService : IApplicationService
    {
        Task<AccountBalanceInfo> GetBalanceAccountInfo(AccountBalanceInfoCheckRequest request);
        Task<AccountBalanceInfo> GetBalanceAccountInfoById(long userId);
    }
}
