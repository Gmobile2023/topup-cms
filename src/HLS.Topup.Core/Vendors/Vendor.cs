using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace HLS.Topup.Vendors
{
	[Table("Vendors")]
    public class Vendor : AuditedEntity , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		[StringLength(VendorConsts.MaxCodeLength, MinimumLength = VendorConsts.MinCodeLength)]
		public virtual string Code { get; set; }
		
		public virtual string Name { get; set; }
		
		public virtual string Description { get; set; }
		
		public virtual string Print_Help { get; set; }
		
		public virtual string Print_Suport { get; set; }
		
		public virtual byte Status { get; set; }
		
		public virtual string Address { get; set; }
		
		public virtual string HotLine { get; set; }
		

    }
}