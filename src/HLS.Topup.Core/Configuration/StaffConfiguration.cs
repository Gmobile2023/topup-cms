using System.ComponentModel.DataAnnotations;
using HLS.Topup.Authorization.Users;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace HLS.Topup.Configuration
{
    [Table("StaffConfigurations")]
    public class StaffConfiguration : AuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual decimal LimitAmount { get; set; }
        public virtual decimal LimitPerTrans { get; set; }
        [StringLength(100)] public string Days { get; set; }
        [StringLength(100)] public string FromTime { get; set; }
        [StringLength(100)] public string ToTime { get; set; }
        [StringLength(255)] public string Description { get; set; }

        public virtual long? UserId { get; set; }

        [ForeignKey("UserId")] public User UserFk { get; set; }
    }
}
