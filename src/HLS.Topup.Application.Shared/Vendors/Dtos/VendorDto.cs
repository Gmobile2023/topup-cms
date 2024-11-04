
using System;
using Abp.Application.Services.Dto;

namespace HLS.Topup.Vendors.Dtos
{
    public class VendorDto : EntityDto
    {
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