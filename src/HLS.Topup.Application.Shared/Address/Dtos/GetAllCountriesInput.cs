using Abp.Application.Services.Dto;
using System;

namespace HLS.Topup.Address.Dtos
{
    public class GetAllCountriesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string CountryCodeFilter { get; set; }

		public string CountryNameFilter { get; set; }

		public int? StatusFilter { get; set; }



    }
}