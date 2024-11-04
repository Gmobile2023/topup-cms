using HLS.Topup.Common;
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace HLS.Topup.Banks.Dtos
{
    public class CreateOrEditBankDto : EntityDto<int?>
    {
        [Required]
        [StringLength(BankConsts.MaxBankNameLength, MinimumLength = BankConsts.MinBankNameLength)]
        public string BankName { get; set; }

        [Required] [StringLength(30)] public string ShortName { get; set; }

        [Required]
        [StringLength(BankConsts.MaxBranchNameLength, MinimumLength = BankConsts.MinBranchNameLength)]
        public string BranchName { get; set; }


        [Required]
        [StringLength(BankConsts.MaxBankAccountNameLength, MinimumLength = BankConsts.MinBankAccountNameLength)]
        public string BankAccountName { get; set; }


        [Required]
        [StringLength(BankConsts.MaxBankAccountCodeLength, MinimumLength = BankConsts.MinBankAccountCodeLength)]
        public string BankAccountCode { get; set; }


        public CommonConst.BankStatus Status { get; set; }


        [StringLength(BankConsts.MaxImagesLength, MinimumLength = BankConsts.MinImagesLength)]
        public string Images { get; set; }


        [StringLength(BankConsts.MaxDescriptionLength, MinimumLength = BankConsts.MinDescriptionLength)]
        public string Description { get; set; }
        
        public string SmsPhoneNumber { get; set; }
        
        public string SmsGatewayNumber { get; set; }
        
        // public string SmsSyntax { get; set; }
        //
        // public string NoteSyntax { get; set; }
    }
}