using System;

namespace HLS.Topup.TopupGateResponseMessage
{
    public class TopupGateResponseMessageDto : DocumentDto
    {
        public string Provider { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseName { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
    public abstract class DocumentDto
    {
        public Guid Id { get; set; }
        public DateTime AddedAtUtc { get; set; }
    }
}