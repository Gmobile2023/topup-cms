using HLS.Topup.Common;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace HLS.Topup.Address.Dtos
{
    public class CreateOrEditDistrictDto : EntityDto<int?>
    {

		[Required]
		[StringLength(DistrictConsts.MaxDistrictCodeLength, MinimumLength = DistrictConsts.MinDistrictCodeLength)]
		public string DistrictCode { get; set; }


		[Required]
		[StringLength(DistrictConsts.MaxDistrictNameLength, MinimumLength = DistrictConsts.MinDistrictNameLength)]
		public string DistrictName { get; set; }


		public CommonConst.DistrictStatus Status { get; set; }


		 public int CityId { get; set; }


    }
}
