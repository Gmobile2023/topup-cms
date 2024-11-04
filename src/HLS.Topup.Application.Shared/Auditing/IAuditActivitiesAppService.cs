using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using HLS.Topup.Auditing.Dto;
using HLS.Topup.Dtos.Audit;

namespace HLS.Topup.Auditing
{
    public interface IAuditActivitiesAppService:IApplicationService
    {
        Task<PagedResultDto<AccountActivityHistoryDto>> GetAccountActivityHistories(GetAuditAccountActivitiesInput input);

        Task<PagedResultDto<AccountActivityHistoryDto>> GetAll(GetAuditAccountActivitiesInput input);
    }
}
