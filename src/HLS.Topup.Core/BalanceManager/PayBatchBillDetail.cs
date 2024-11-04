using HLS.Topup.Common;
using HLS.Topup.Products;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using HLS.Topup.Authorization.Users;

namespace HLS.Topup.BalanceManager
{
    [Table("PayBatchBillDetails")]
    public class PayBatchBillDetail : AuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual decimal Amount { get; set; }
        public virtual decimal? Money { get; set; }
        public virtual int ? Quantity { get; set; }

        [StringLength(PayBatchBillConsts.MaxDescriptionLength, MinimumLength = PayBatchBillConsts.MinDescriptionLength)]
        public virtual string Description { get; set; }

        public virtual int PayBatchBillId { get; set; }

        [ForeignKey("PayBatchBillId")]
        public PayBatchBill PayBatchBillFk { get; set; }

        public virtual long? UserId { get; set; }

        [ForeignKey("UserId")] public User UserFk { get; set; }

        [StringLength(50)]
        public virtual string TransRef { get; set; }
        [StringLength(255)]
        public virtual string TransNote { get; set; }
        [StringLength(255)]
        public virtual bool Success { get; set; }

    }
}
