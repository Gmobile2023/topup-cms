using System.Collections.Generic;
using System.Threading.Tasks;
using HLS.Topup.Dtos.Balance;
using HLS.Topup.Dtos.Stock;

namespace HLS.Topup.BalanceManager
{
    public interface IBalanceAlertManager
    {
        Task<ApiResponseDto<List<LowBalanceAlertResponseDto>>> BalanceAlertGetAllRequest(BalanceAlertGetAllRequest request);
        Task<LowBalanceAlertResponseDto> BalanceAlertGetRequest(BalanceAlertGetRequest request);
        Task<NewMessageReponseBase<object>> BalanceAlertAddRequest(BalanceAlertAddRequest request);
        Task<NewMessageReponseBase<object>> BalanceAlertUpdateRequest(BalanceAlertUpdateRequest request);
    }
}