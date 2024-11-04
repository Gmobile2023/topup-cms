using HLS.Topup.Address.Dtos;
using System.Collections.Generic;

using Abp.Extensions;

namespace HLS.Topup.Web.Areas.App.Models.Cities
{
    public class CreateOrEditCityModalViewModel
    {
       public CreateOrEditCityDto City { get; set; }

	   		public string CountryCountryName { get; set;}


       public List<CityCountryLookupTableDto> CityCountryList { get; set;}


	   public bool IsEditMode => City.Id.HasValue;
    }
}