using Abp.Application.Services.Dto;
using System;

namespace HLS.Topup.Vendors.Dtos
{
    public class GetAllVendorsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string CodeFilter { get; set; }

		public string NameFilter { get; set; }

		public string DescriptionFilter { get; set; }

		public string Print_HelpFilter { get; set; }

		public string Print_SuportFilter { get; set; }

		public byte? MaxStatusFilter { get; set; }
		public byte? MinStatusFilter { get; set; }

		public string AddressFilter { get; set; }

		public string HotLineFilter { get; set; }



    }
}