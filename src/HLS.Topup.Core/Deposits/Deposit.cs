using HLS.Topup.Common;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Banks;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace HLS.Topup.Deposits
{
    [Table("Deposits")]
    public class Deposit : AuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }


        public virtual CommonConst.DepositStatus Status { get; set; }
        public virtual CommonConst.DepositType Type { get; set; }

        public virtual decimal Amount { get; set; }

        public virtual DateTime? ApprovedDate { get; set; }

        [Required]
        [StringLength(DepositConsts.MaxTransCodeLength, MinimumLength = DepositConsts.MinTransCodeLength)]
        public virtual string TransCode { get; set; }

        [StringLength(DepositConsts.MaxDescriptionLength, MinimumLength = DepositConsts.MinDescriptionLength)]
        public virtual string Description { get; set; }

        [StringLength(DepositConsts.MaxExtraInfoLength, MinimumLength = DepositConsts.MinExtraInfoLength)]
        public virtual string ExtraInfo { get; set; }

        public virtual long UserId { get; set; }

        [ForeignKey("UserId")] public User UserFk { get; set; }

        public virtual int? BankId { get; set; }
        
        [StringLength(DepositConsts.MinRecipientInfoLength, MinimumLength = DepositConsts.MaxRecipientInfoLength)]
        public virtual string RecipientInfo { get; set; }

        [ForeignKey("BankId")] public Bank BankFk { get; set; }

        public virtual long? ApproverId { get; set; }

        [ForeignKey("ApproverId")] public User ApproverFk { get; set; }

        public virtual long? UserSaleId { get; set; }

        [ForeignKey("UserSaleId")] public User UserSaleFk { get; set; }
        
        [StringLength(255)]
        public virtual string Attachment { get; set; }
        
        [StringLength(50)]
        public virtual string TransCodeBank { get; set; }
        
        [StringLength(10)]
        public virtual string RequestCode { get; set; }
    }
}
