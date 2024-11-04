using HLS.Topup.Common;
using System;
using Abp.Application.Services.Dto;

namespace HLS.Topup.Deposits.Dtos
{
    public class DepositDto : EntityDto
    {
        public CommonConst.DepositStatus Status { get; set; }
        
        public CommonConst.DepositType Type { get; set; }

        public decimal Amount { get; set; }

        public DateTime? ApprovedDate { get; set; }

        public string TransCode { get; set; }
        
        public string TransCodeBank { get; set; }
        
        public string Description { get; set; }

        public long UserId { get; set; }

        public long? ApproverId { get; set; }
        
        public string RecipientInfo { get; set; }
        
        public DateTime? CreationTime { get; set; }
        
        public string Attachment { get; set; }

        public long CreatorUserId { get; set; }
        
        public int? BankId { get; set; }
        
        public string RequestCode { get; set; }
    }
}
