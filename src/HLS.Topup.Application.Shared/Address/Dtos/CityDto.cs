using HLS.Topup.Common;

using System;
using Abp.Application.Services.Dto;

namespace HLS.Topup.Address.Dtos
{
    public class CityDto : EntityDto
    {
		public string CityCode { get; set; }

		public string CityName { get; set; }

		public CommonConst.CityStatus Status { get; set; }


		 public int? CountryId { get; set; }


    }
}
