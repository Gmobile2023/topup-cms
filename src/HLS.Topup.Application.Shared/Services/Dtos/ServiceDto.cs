using HLS.Topup.Services;

using System;
using Abp.Application.Services.Dto;

namespace HLS.Topup.Services.Dtos
{
    public class ServiceDto : EntityDto
    {
		public string ServiceCode { get; set; }

		public string ServicesName { get; set; }

		public ServiceStatus Status { get; set; }

		public int Order { get; set; }



    }
}