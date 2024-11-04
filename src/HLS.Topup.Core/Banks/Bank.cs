using HLS.Topup.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace HLS.Topup.Banks
{
    [Table("Banks")]
    public class Bank : AuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        
        [Required]
        [StringLength(BankConsts.MaxBankNameLength, MinimumLength = BankConsts.MinBankNameLength)]
        public virtual string BankName { get; set; }

        [Required] [StringLength(30)] public virtual string ShortName { get; set; }

        [Required]
        [StringLength(BankConsts.MaxBranchNameLength, MinimumLength = BankConsts.MinBranchNameLength)]
        public virtual string BranchName { get; set; }

        [Required]
        [StringLength(BankConsts.MaxBankAccountNameLength, MinimumLength = BankConsts.MinBankAccountNameLength)]
        public virtual string BankAccountName { get; set; }

        [Required]
        [StringLength(BankConsts.MaxBankAccountCodeLength, MinimumLength = BankConsts.MinBankAccountCodeLength)]
        public virtual string BankAccountCode { get; set; }

        public virtual CommonConst.BankStatus Status { get; set; }

        [StringLength(BankConsts.MaxImagesLength, MinimumLength = BankConsts.MinImagesLength)]
        public virtual string Images { get; set; }

        [StringLength(BankConsts.MaxDescriptionLength, MinimumLength = BankConsts.MinDescriptionLength)]
        public virtual string Description { get; set; }
        
        [StringLength(30)]
        public virtual string SmsPhoneNumber { get; set; }
        
        [StringLength(30)]
        public virtual string SmsGatewayNumber { get; set; }
        
        [StringLength(600)]
        public virtual string SmsSyntax { get; set; }
        [StringLength(600)]
        public virtual string NoteSyntax { get; set; }
    }
}