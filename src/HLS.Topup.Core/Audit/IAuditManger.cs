using System.Collections.Generic;
using System.Threading.Tasks;
using HLS.Topup.Dtos.Audit;
using HLS.Topup.Dtos.Stock;
using HLS.Topup.RequestDtos;

namespace HLS.Topup.Audit
{
    public interface IAuditManger
    {
        Task<ApiResponseDto<List<AccountActivityHistoryDto>>> GetAccountActivityHistoryRequest(GetAccountActivityHistoryRequest request);
        Task AddAccountActivities(AccountActivityHistoryRequest request);
    }
}
