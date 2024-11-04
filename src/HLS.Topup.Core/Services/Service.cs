using HLS.Topup.Services;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace HLS.Topup.Services
{
	[Table("Services")]
    public class Service : AuditedEntity , IMayHaveTenant
    {
	    public int? TenantId { get; set; }
			

		[Required]
		[StringLength(ServiceConsts.MaxServiceCodeLength, MinimumLength = ServiceConsts.MinServiceCodeLength)]
		public virtual string ServiceCode { get; set; }
		
		[Required]
		[StringLength(ServiceConsts.MaxServicesNameLength, MinimumLength = ServiceConsts.MinServicesNameLength)]
		public virtual string ServicesName { get; set; }
		
		[StringLength(ServiceConsts.MaxServiceConfigLength, MinimumLength = ServiceConsts.MinServiceConfigLength)]
		public virtual string ServiceConfig { get; set; }
		
		public virtual ServiceStatus Status { get; set; }
		
		public virtual int Order { get; set; }
		
		[StringLength(ServiceConsts.MaxDescriptionLength, MinimumLength = ServiceConsts.MinDescriptionLength)]
		public virtual string Description { get; set; }
		

    }
}