using HLS.Topup.Common;

using System;
using Abp.Application.Services.Dto;

namespace HLS.Topup.Address.Dtos
{
    public class CountryDto : EntityDto
    {
		public string CountryCode { get; set; }

		public string CountryName { get; set; }

		public CommonConst.CountryStatus Status { get; set; }



    }
}
