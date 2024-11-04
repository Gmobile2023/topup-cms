using ServiceStack;

namespace HLS.Topup.RequestDtos.TopupGateResponseMessage
{
    [Route("api/v1/topupgate/provider_response", "DELETE")]
    public class DeleteTopupGateResponseMessageRequest
    {
        public string Provider { get; set; }
        public string Code { get; set; }
    }
}