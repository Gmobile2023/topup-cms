using System;

namespace HLS.Topup.TopupGateResponseMessage.Dto
{
    public class CreateOrEditTopupGateResponse
    {
        public Guid Id { get; set; }
        public string Provider { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseName { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}