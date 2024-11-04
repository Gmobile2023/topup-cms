using HLS.Topup.Common;
using HLS.Topup.Authorization.Users;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace HLS.Topup.LimitationManager
{
    [Table("LimitProducts")]
    public class LimitProduct : AuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        
        public virtual string Code { get; set; }

        [Required]
        [StringLength(LimitProductConsts.MaxNameLength, MinimumLength = LimitProductConsts.MinNameLength)]
        public virtual string Name { get; set; }

        public virtual DateTime FromDate { get; set; }

        public virtual DateTime ToDate { get; set; }

        public virtual DateTime DateApproved { get; set; }

        public virtual long? ApproverId { get; set; }

        public virtual CommonConst.AgentType AgentType { get; set; }

        [StringLength(LimitProductConsts.MaxDescriptionLength, MinimumLength = LimitProductConsts.MinDescriptionLength)]
        public virtual string Description { get; set; }

        [StringLength(LimitProductConsts.MaxProductTypeLength, MinimumLength = LimitProductConsts.MinProductTypeLength)]
        public virtual string ProductType { get; set; }

        [StringLength(LimitProductConsts.MaxProductListLength, MinimumLength = LimitProductConsts.MinProductListLength)]
        public virtual string ProductList { get; set; }

        public virtual CommonConst.LimitProductConfigStatus Status { get; set; }


        public virtual long? UserId { get; set; }

        [ForeignKey("UserId")] public User UserFk { get; set; }
    }
}