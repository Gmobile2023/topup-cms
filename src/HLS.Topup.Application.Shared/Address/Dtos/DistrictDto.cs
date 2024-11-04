using HLS.Topup.Common;
using System;
using Abp.Application.Services.Dto;

namespace HLS.Topup.Address.Dtos
{
    public class DistrictDto : EntityDto
    {
        public string DistrictCode { get; set; }

        public string DistrictName { get; set; }

        public CommonConst.DistrictStatus Status { get; set; }


        public int CityId { get; set; }
    }
}
