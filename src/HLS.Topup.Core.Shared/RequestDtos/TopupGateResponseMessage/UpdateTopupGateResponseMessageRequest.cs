using HLS.Topup.TopupGateResponseMessage;
using ServiceStack;

namespace HLS.Topup.RequestDtos.TopupGateResponseMessage
{
    [Route("api/v1/topupgate/provider_response", "PUT")]
    public class UpdateTopupGateResponseMessageRequest
    {
        public string Provider { get; set; }
        public string ReponseCode { get; set; }
        public string ReponseName { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}