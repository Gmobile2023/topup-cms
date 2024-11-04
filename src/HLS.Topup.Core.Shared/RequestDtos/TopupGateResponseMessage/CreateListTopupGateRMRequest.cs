using System.Collections.Generic;
using HLS.Topup.TopupGateResponseMessage;
using ServiceStack;

namespace HLS.Topup.RequestDtos.TopupGateResponseMessage
{
    [Route("api/v1/topupgate/provider_response/import", "POST")]
    public class CreateListTopupGateRMRequest
    {
        public List<TopupGateResponseMessageDto> ListProviderResponse { get; set; }
    }
}