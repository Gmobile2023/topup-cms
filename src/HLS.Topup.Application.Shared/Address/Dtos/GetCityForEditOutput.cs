using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace HLS.Topup.Address.Dtos
{
    public class GetCityForEditOutput
    {
		public CreateOrEditCityDto City { get; set; }

		public string CountryCountryName { get; set;}


    }
}