using HLS.Topup.Common;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace HLS.Topup.Address.Dtos
{
    public class CreateOrEditCountryDto : EntityDto<int?>
    {

		[Required]
		[StringLength(CountryConsts.MaxCountryCodeLength, MinimumLength = CountryConsts.MinCountryCodeLength)]
		public string CountryCode { get; set; }


		[Required]
		[StringLength(CountryConsts.MaxCountryNameLength, MinimumLength = CountryConsts.MinCountryNameLength)]
		public string CountryName { get; set; }


		public CommonConst.CountryStatus Status { get; set; }



    }
}
