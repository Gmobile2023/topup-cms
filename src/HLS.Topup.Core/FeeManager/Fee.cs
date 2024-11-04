using HLS.Topup.Common;
using HLS.Topup.Authorization.Users;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace HLS.Topup.FeeManager
{
    [Table("Fees")]
    public class Fee : AuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }


        [StringLength(FeeConsts.MaxCodeLength, MinimumLength = FeeConsts.MinCodeLength)]
        public virtual string Code { get; set; }

        [Required]
        [StringLength(FeeConsts.MaxNameLength, MinimumLength = FeeConsts.MinNameLength)]
        public virtual string Name { get; set; }

        public virtual DateTime FromDate { get; set; }

        public virtual DateTime ToDate { get; set; }

        public virtual DateTime? DateApproved { get; set; }

        public virtual long? ApproverId { get; set; }

        public virtual CommonConst.FeeStatus Status { get; set; }

        public virtual CommonConst.AgentType AgentType { get; set; }

        [StringLength(FeeConsts.MaxDescriptionLength, MinimumLength = FeeConsts.MinDescriptionLength)]
        public virtual string Description { get; set; }

        [StringLength(100)] public string ProductType { get; set; }
        
        [StringLength(255)] public string ProductList { get; set; }

        public virtual long? UserId { get; set; }

        [ForeignKey("UserId")] public User UserFk { get; set; }
    }
}