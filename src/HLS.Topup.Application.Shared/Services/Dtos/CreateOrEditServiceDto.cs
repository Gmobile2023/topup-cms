using HLS.Topup.Services;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace HLS.Topup.Services.Dtos
{
    public class CreateOrEditServiceDto : EntityDto<int?>
    {

		[Required]
		[StringLength(ServiceConsts.MaxServiceCodeLength, MinimumLength = ServiceConsts.MinServiceCodeLength)]
		public string ServiceCode { get; set; }
		
		
		[Required]
		[StringLength(ServiceConsts.MaxServicesNameLength, MinimumLength = ServiceConsts.MinServicesNameLength)]
		public string ServicesName { get; set; }
		
		
		[StringLength(ServiceConsts.MaxServiceConfigLength, MinimumLength = ServiceConsts.MinServiceConfigLength)]
		public string ServiceConfig { get; set; }
		
		
		public ServiceStatus Status { get; set; }
		
		
		public int Order { get; set; }
		
		
		[StringLength(ServiceConsts.MaxDescriptionLength, MinimumLength = ServiceConsts.MinDescriptionLength)]
		public string Description { get; set; }
		
		

    }
}