using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using HLS.Topup.Authorization.Users;

namespace HLS.Topup.Paybacks
{
    [Table("PayBackDetails")]
    public class PayBackDetail : AuditedEntity, IMayHaveTenant
    {
        public virtual int? TenantId { get; set; }

        public virtual int PayBackId { get; set; }

        [ForeignKey("PayBackId")] public virtual PayBack PayBackFk { get; set; }

        public virtual long UserId { get; set; }

        [ForeignKey("UserId")] public virtual User UserFk { get; set; }
        public virtual decimal Amount { get; set; }

        public virtual bool? Status { get; set; }

        [StringLength(50)]
        public virtual string TransCode { get; set; }
        [StringLength(255)]
        public virtual string TransNote { get; set; }
    }
}
