using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using HLS.Topup.Authorization.Users;

namespace HLS.Topup.AgentsManager
{
    [Table("ChangeUserNameHistories")]
    public class ChangeUserNameHistories : AuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        [StringLength(255)]
        public virtual string Description { get; set; }
        [StringLength(500)]
        public virtual string Attachment { get; set; }
        public virtual byte Status { get; set; }

        [StringLength(20)]
        public virtual string NewUserName { get; set; }
        [StringLength(20)]
        public virtual string OldUserName { get; set; }
        public virtual long? ApproverId { get; set; }
        public virtual DateTime? DateApproved { get; set; }

        [ForeignKey("UserId")]
        public virtual long UserId { get; set; }
        public User UserFk { get; set; }
    }
}
