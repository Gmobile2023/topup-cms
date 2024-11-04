using System;
using HLS.Topup.Common;

namespace HLS.Topup.Dtos.Audit
{
    public class AccountActivityHistoryDto
    {
        public string AccountCode { get; set; }
        public string FullName { get; set; }
        public int AccountType { get; set; }
        public int AgentType { get; set; }
        public string PhoneNumber { get; set; }
        public string UserCreated { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Note { get; set; }
        public string Payload { get; set; }
        public string SrcValue { get; set; }
        public string DesValue { get; set; }
        public string AgentName { get; set; }
        public CommonConst.AccountActivityType AccountActivityType { get; set; }

        public string Attachment { get; set; }
    }

    public class BlockUnlockUserDto
    {
        public long UserId { get; set; }
        public string Note { get; set; }
    }
}
