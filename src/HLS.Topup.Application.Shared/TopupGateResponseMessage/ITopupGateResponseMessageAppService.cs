using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using HLS.Topup.TopupGateResponseMessage.Dto;

namespace HLS.Topup.TopupGateResponseMessage
{
    public interface ITopupGateResponseMessageAppService: IApplicationService
    {
        Task CreateOrEditTopupGateResponseMessage(CreateOrEditTopupGateResponse input);
        Task<GetTopupGateRMForEditOutput> GetTopupGateRMForEdit(string provider, string code);

        Task<PagedResultDto<TopupGateResponseMessageDto>> GetListTopupGateResponseMessage(
            GetAllTopupGateRMInput input);

        Task<GetTopupGateRMForView> GetTopupGateRMForView(string provider, string code);

        Task<object> DeleteTopupGateRM(string provider, string code);

    }
}