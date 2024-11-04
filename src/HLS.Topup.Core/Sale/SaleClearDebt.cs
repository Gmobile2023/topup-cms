using HLS.Topup.Common;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Banks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace HLS.Topup.Sale
{
    [Table("SaleClearDebts")]
    public class SaleClearDebt : AuditedEntity, IMayHaveTenant
    {
        public virtual string TransCode { get; set; }
        public virtual CommonConst.ClearDebtStatus Status { get; set; }
        public int? TenantId { get; set; }


        public virtual decimal Amount { get; set; }

        public virtual CommonConst.ClearDebtType Type { get; set; }

        [StringLength(SaleClearDebtConsts.MaxDescriptionsLength,
            MinimumLength = SaleClearDebtConsts.MinDescriptionsLength)]
        public virtual string Descriptions { get; set; }

        [StringLength(SaleClearDebtConsts.MaxDescriptionsLength,
    MinimumLength = SaleClearDebtConsts.MinDescriptionsLength)]
        public virtual string ApprovalNote { get; set; }


        public virtual long UserId { get; set; }

        [ForeignKey("UserId")] public User UserFk { get; set; }

        public virtual int? BankId { get; set; }

        [ForeignKey("BankId")] public Bank BankFk { get; set; }

        public virtual string TransCodeBank { get; set; }
    }
}
