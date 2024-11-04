using HLS.Topup.Authorization.Users;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace HLS.Topup.BalanceManager
{
    [Table("AccountBlockBalances")]
    public class AccountBlockBalance : AuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }


        [Required]
        [StringLength(AccountBlockBalanceConsts.MaxTransCodeLength,
            MinimumLength = AccountBlockBalanceConsts.MinTransCodeLength)]
        public virtual string TransCode { get; set; }

        public virtual decimal BlockedMoney { get; set; }

        [StringLength(AccountBlockBalanceConsts.MaxDescriptionLength,
            MinimumLength = AccountBlockBalanceConsts.MinDescriptionLength)]
        public virtual string Description { get; set; }


        public virtual long UserId { get; set; }

        [ForeignKey("UserId")] public User UserFk { get; set; }
    }
}
