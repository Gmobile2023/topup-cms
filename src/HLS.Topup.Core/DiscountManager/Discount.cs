using HLS.Topup.Common;
using HLS.Topup.Authorization.Users;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace HLS.Topup.DiscountManager
{
	[Table("Discounts")]
    public class Discount : AuditedEntity , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		[StringLength(DiscountConsts.MaxCodeLength, MinimumLength = DiscountConsts.MinCodeLength)]
		public virtual string Code { get; set; }
		
		[Required]
		[StringLength(DiscountConsts.MaxNameLength, MinimumLength = DiscountConsts.MinNameLength)]
		public virtual string Name { get; set; }
		
		public virtual DateTime FromDate { get; set; }
		
		public virtual DateTime ToDate { get; set; }
		
		public virtual DateTime? DateApproved { get; set; }
		
		public virtual CommonConst.DiscountStatus Status { get; set; }
		
		public virtual CommonConst.DiscountType DiscountType { get; set; }
		
		public virtual long? ApproverId { get; set; }
		
		public virtual CommonConst.AgentType AgentType { get; set; }
		
		[StringLength(DiscountConsts.MaxDesciptionsLength, MinimumLength = DiscountConsts.MinDesciptionsLength)]
		public virtual string Desciptions { get; set; }
		

		public virtual long? UserId { get; set; }
		
        [ForeignKey("UserId")]
		public User UserFk { get; set; }
		
    }
}