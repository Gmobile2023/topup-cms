using HLS.Topup.Address.Dtos;
using System.Collections.Generic;

using Abp.Extensions;

namespace HLS.Topup.Web.Areas.App.Models.Districts
{
    public class CreateOrEditDistrictModalViewModel
    {
       public CreateOrEditDistrictDto District { get; set; }

	   		public string CityCityName { get; set;}


       public List<DistrictCityLookupTableDto> DistrictCityList { get; set;}


	   public bool IsEditMode => District.Id.HasValue;
    }
}