using HLS.Topup.Common;
using HLS.Topup.Address;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace HLS.Topup.Address
{
	[Table("Wards")]
    public class Ward : AuditedEntity , IMayHaveTenant
    {
			public int? TenantId { get; set; }


		[Required]
		[StringLength(WardConsts.MaxWardCodeLength, MinimumLength = WardConsts.MinWardCodeLength)]
		public virtual string WardCode { get; set; }

		[Required]
		[StringLength(WardConsts.MaxWardNameLength, MinimumLength = WardConsts.MinWardNameLength)]
		public virtual string WardName { get; set; }

		public virtual CommonConst.WardStatus Status { get; set; }


		public virtual int DistrictId { get; set; }

        [ForeignKey("DistrictId")]
		public District DistrictFk { get; set; }

    }
}
