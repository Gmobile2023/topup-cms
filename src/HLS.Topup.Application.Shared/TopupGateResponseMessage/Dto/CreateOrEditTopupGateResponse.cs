using System;

namespace HLS.Topup.TopupGateResponseMessage.Dto
{
    public class CreateOrEditTopupGateResponse
    {
        public Guid Id { get; set; }
        public string Provider { get; set; }
        public string ReponseCode { get; set; }
        public string ReponseName { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}