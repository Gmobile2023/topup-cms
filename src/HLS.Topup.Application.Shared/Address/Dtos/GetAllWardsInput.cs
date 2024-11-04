using Abp.Application.Services.Dto;
using System;

namespace HLS.Topup.Address.Dtos
{
    public class GetAllWardsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string WardCodeFilter { get; set; }

		public string WardNameFilter { get; set; }

		public int? StatusFilter { get; set; }


		 public string DistrictDistrictNameFilter { get; set; }

		 
    }
}