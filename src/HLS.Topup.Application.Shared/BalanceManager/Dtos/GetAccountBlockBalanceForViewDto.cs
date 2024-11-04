using System;
using HLS.Topup.Common;

namespace HLS.Topup.BalanceManager.Dtos
{
    public class GetAccountBlockBalanceForViewDto
    {
        public AccountBlockBalanceDto AccountBlockBalance { get; set; }
        public string UserName { get; set; }
        public CommonConst.AgentType AgentType { get; set; }
        public string FullAgentName { get; set; }
    }
    public class AccountBlockBalanceDetailDto
    {
        public virtual decimal Amount { get; set; }
        public virtual string Description { get; set; }
        public virtual string Attachments { get; set; }
        public virtual int AccountBlockBalanceId { get; set; }
        public virtual CommonConst.BlockBalanceType Type { get; set; }
        public virtual bool Success { get; set; }
        public virtual string TransRef { get; set; }
        public virtual string TransNote { get; set; }
        public virtual string UserName { get; set; }
        public virtual DateTime CreationTime { get; set; }
    }

}
