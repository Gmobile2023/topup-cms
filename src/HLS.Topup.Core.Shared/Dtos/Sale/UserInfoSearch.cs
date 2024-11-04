using HLS.Topup.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace HLS.Topup.Dtos.Sale
{
    public class UserInfoSearch
    {
        public string Search { get; set; }
        public int SaleId { get; set; }
        public int SaleLeadId { get; set; }
        public string SaleLeadCode { get; set; }
        public string LoginCode { get; set; }
        
        public CommonConst.AgentType? AgentType { get; set; }

        public CommonConst.SystemAccountType AccountType { get; set; }
    }
}
