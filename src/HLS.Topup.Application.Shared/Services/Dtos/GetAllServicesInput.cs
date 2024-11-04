using Abp.Application.Services.Dto;
using System;

namespace HLS.Topup.Services.Dtos
{
    public class GetAllServicesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string ServiceCodeFilter { get; set; }

		public string ServicesNameFilter { get; set; }

		public int? StatusFilter { get; set; }



    }
}