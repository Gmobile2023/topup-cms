using HLS.Topup.Common;
using ServiceStack;

namespace HLS.Topup.RequestDtos.TopupGateResponseMessage
{
    [Route("/api/v1/topupgate/provider_response/list", "GET")]
    public class GetListTopupGateResponseRMRequest : PaggingBaseDto
    {
        public string Provider { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseName { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}