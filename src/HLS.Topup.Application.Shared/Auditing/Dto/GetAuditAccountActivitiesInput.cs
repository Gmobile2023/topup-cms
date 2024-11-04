using System;
using HLS.Topup.Common;
using HLS.Topup.Dto;

namespace HLS.Topup.Auditing.Dto
{
    public class GetAuditAccountActivitiesInput:PagedAndSortedInputDto
    {
        public string AccountCode { get; set; }
        public int AccountType { get; set; }
        public int AgentType { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string Note { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public CommonConst.AccountActivityType? AccountActivityType { get; set; }
    }
}
