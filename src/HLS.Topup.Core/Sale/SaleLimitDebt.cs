using HLS.Topup.Common;
using HLS.Topup.Authorization.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace HLS.Topup.Sale
{
    [Table("SaleLimitDebts")]
    public class SaleLimitDebt : AuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }


        public virtual decimal LimitAmount { get; set; }

        public virtual int DebtAge { get; set; }

        public virtual CommonConst.DebtLimitAmountStatus Status { get; set; }

        [StringLength(SaleLimitDebtConsts.MaxDescriptionLength, MinimumLength = SaleLimitDebtConsts.MinDescriptionLength)]
        public virtual string Description { get; set; }

        public virtual string UserName { get; set; }


        public virtual long UserId { get; set; }

        [ForeignKey("UserId")]
        public User UserFk { get; set; }

    }
}
