using Abp.Application.Services.Dto;
using System;

namespace HLS.Topup.Address.Dtos
{
    public class GetAllCitiesForExcelInput
    {
		public string Filter { get; set; }

		public string CityNameFilter { get; set; }

		public int? StatusFilter { get; set; }


		 public string CountryCountryNameFilter { get; set; }

		 
    }
}