using HLS.Topup.Authorization.Users;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using HLS.Topup.Common;

namespace HLS.Topup.BalanceManager
{
    [Table("AccountBlockBalanceDetails")]
    public class AccountBlockBalanceDetail : AuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual decimal Amount { get; set; }

        [StringLength(AccountBlockBalanceConsts.MaxDescriptionLength, MinimumLength = AccountBlockBalanceConsts.MinDescriptionLength)]
        public virtual string Description { get; set; }
        [StringLength(255)]
        public virtual string Attachments { get; set; }
        public virtual CommonConst.BlockBalanceType Type { get; set; }
        public virtual bool Success { get; set; }
        [StringLength(50)]
        public virtual string TransRef { get; set; }
        [StringLength(2500)]
        public virtual string TransNote { get; set; }

        public virtual int AccountBlockBalanceId { get; set; }

        [ForeignKey("AccountBlockBalanceId")]
        public AccountBlockBalance AccountBlockBalanceFk { get; set; }

    }
}
