using HLS.Topup.Common;

namespace HLS.Topup.Dtos.Common
{
    public class GetUserQueryRequest
    {
        public string Search { get; set; }
        public  CommonConst.AgentType ? AgentType { get; set; }
        public bool IsAccountBackend { get; set; }
        public bool IsAccountSystem { get; set; }

        public bool IsAccountAgent { get; set; }

        public int SaleId { get; set; }

        public int SaleLeaderId { get; set; }

        public string SaleLeaderCode { get; set; }

        public CommonConst.SystemAccountType AccountType { get; set; }
    }
}
