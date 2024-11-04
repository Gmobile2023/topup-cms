using HLS.Topup.Common;
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace HLS.Topup.Deposits.Dtos
{
    public class CreateOrEditDepositDto : EntityDto<int?>
    {
        public CommonConst.DepositStatus Status { get; set; }
        public CommonConst.DepositType Type { get; set; }

        public decimal Amount { get; set; }

        [StringLength(DepositConsts.MaxDescriptionLength, MinimumLength = DepositConsts.MinDescriptionLength)]
        public string Description { get; set; }
        
        public long UserId { get; set; }

        public long ? UserSaleId { get; set; }

        public int? BankId { get; set; }

        public long? ApproverId { get; set; }
        
        public string RecipientInfo { get; set; }
        
        public string Attachment { get; set; }
    }

    public class ApprovalDepositDto
    {
        public string TransCode { get; set; }
        public string TransNote { get; set; }
        public string TransCodeBank { get; set; }
    }
    public class CancelDepositDto
    {
        public string TransCode { get; set; }
        public string TransNote { get; set; }
    }
}
