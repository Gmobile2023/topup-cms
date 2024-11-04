using System;

namespace HLS.Topup.Dtos.Common
{
    public class AuditCommonDto
    {
        public DateTime DateCreated { get; set; }
        public string UserCreated { get; set; }
        public DateTime? DateApproved { get; set; }
        public string UserApproved { get; set; }
    }
}
