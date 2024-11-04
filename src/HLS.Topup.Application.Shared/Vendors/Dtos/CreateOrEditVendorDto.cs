
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace HLS.Topup.Vendors.Dtos
{
    public class CreateOrEditVendorDto : EntityDto<int?>
    {

		[StringLength(VendorConsts.MaxCodeLength, MinimumLength = VendorConsts.MinCodeLength)]
		public string Code { get; set; }
		
		
		public string Name { get; set; }
		
		
		public string Description { get; set; }
		
		
		public string Print_Help { get; set; }
		
		
		public string Print_Suport { get; set; }
		
		
		public byte Status { get; set; }
		
		
		public string Address { get; set; }
		
		
		public string HotLine { get; set; }
		
		

    }
}