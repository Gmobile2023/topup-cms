using Abp.Application.Services.Dto;
using System;
using HLS.Topup.Common;

namespace HLS.Topup.Deposits.Dtos
{
    public class GetAllDepositsForExcelInput
    {
        public string Filter { get; set; }
        public int? StatusFilter { get; set; }
        public DateTime? MaxApprovedDateFilter { get; set; }
        public DateTime? MinApprovedDateFilter { get; set; }
        public string TransCodeFilter { get; set; }
        public string TransCodeBankFilter { get; set; }
        public string UserNameFilter { get; set; }
        public long? UserId { get; set; }
        public long? ApproverId { get; set; }
        public long? BankId { get; set; }
        public string BankBankNameFilter { get; set; }
        public string UserName2Filter { get; set; }
        public CommonConst.AgentType? AgentTypeFilter { get; set; }
        public CommonConst.DepositType? DepositTypeFilter { get; set; }
        public long? SaleLeadFilter { get; set; }
        public long? SaleManFilter { get; set; }
        
        public string RequestCodeFilter { get; set; }
    }
}