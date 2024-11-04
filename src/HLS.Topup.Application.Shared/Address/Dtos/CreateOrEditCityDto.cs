using HLS.Topup.Common;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace HLS.Topup.Address.Dtos
{
    public class CreateOrEditCityDto : EntityDto<int?>
    {

		[Required]
		[StringLength(CityConsts.MaxCityCodeLength, MinimumLength = CityConsts.MinCityCodeLength)]
		public string CityCode { get; set; }


		[Required]
		[StringLength(CityConsts.MaxCityNameLength, MinimumLength = CityConsts.MinCityNameLength)]
		public string CityName { get; set; }


		public CommonConst.CityStatus Status { get; set; }


		 public int? CountryId { get; set; }


    }
}
