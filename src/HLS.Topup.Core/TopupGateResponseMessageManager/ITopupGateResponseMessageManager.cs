using System.Collections.Generic;
using System.Threading.Tasks;
using HLS.Topup.Dtos.Stock;
using HLS.Topup.RequestDtos.TopupGateResponseMessage;
using HLS.Topup.TopupGateResponseMessage;

namespace HLS.Topup.TopupGateResponseMessageManager
{
    public interface ITopupGateResponseMessageManager
    {
        Task<NewMessageReponseBase<object>> CreateTopupGateResponseMessageManager(
            CreateTopupGateResponseMessageRequest request);

        Task<NewMessageReponseBase<object>> UpdateTopupGateResponseMessageManager(
            UpdateTopupGateResponseMessageRequest request);

        Task<NewMessageReponseBase<TopupGateResponseMessageDto>> GetTopupGateResponseMessageManager(
            GetTopupGateResponseMRequest request);

        Task<ApiResponseDto<List<TopupGateResponseMessageDto>>> GetListTopupGateResponseMessageAsync(
            GetListTopupGateResponseRMRequest request);

        Task<NewMessageReponseBase<object>> CreateListTopupGateResponseMessageAsync(CreateListTopupGateRMRequest request);

        Task<NewMessageReponseBase<object>> DeleteTopupGateResponseMessageManager(
            DeleteTopupGateResponseMessageRequest request);
    }
}