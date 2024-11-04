using HLS.Topup.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace HLS.Topup.Address
{
	[Table("Countries")]
    public class Country : AuditedEntity , IMayHaveTenant
    {
			public int? TenantId { get; set; }


		[Required]
		[StringLength(CountryConsts.MaxCountryCodeLength, MinimumLength = CountryConsts.MinCountryCodeLength)]
		public virtual string CountryCode { get; set; }

		[Required]
		[StringLength(CountryConsts.MaxCountryNameLength, MinimumLength = CountryConsts.MinCountryNameLength)]
		public virtual string CountryName { get; set; }

		public virtual CommonConst.CountryStatus Status { get; set; }


    }
}
