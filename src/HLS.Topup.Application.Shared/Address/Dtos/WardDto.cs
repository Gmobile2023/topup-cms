using HLS.Topup.Common;

using System;
using Abp.Application.Services.Dto;

namespace HLS.Topup.Address.Dtos
{
    public class WardDto : EntityDto
    {
		public string WardCode { get; set; }

		public string WardName { get; set; }

		public CommonConst.WardStatus Status { get; set; }


		 public int DistrictId { get; set; }


    }
}
