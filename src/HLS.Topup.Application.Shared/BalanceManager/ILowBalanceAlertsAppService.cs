using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using HLS.Topup.BalanceManager.Dtos;

namespace HLS.Topup.BalanceManager
{
    public interface ILowBalanceAlertsAppService : IApplicationService
    {
        Task<PagedResultDto<LowBalanceAlertDto>> GetAll(GetAllLowBalanceAlertsInput input);
        Task<GetLowBalanceAlertForEditOutput> GetLowBalanceAlertForEdit(string accountCode);
        Task<GetLowBalanceAlertForViewDto> GetLowBalanceAlertForView(string accountCode);
    }
}