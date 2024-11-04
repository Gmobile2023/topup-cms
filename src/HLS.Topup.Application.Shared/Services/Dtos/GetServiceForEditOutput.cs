using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace HLS.Topup.Services.Dtos
{
    public class GetServiceForEditOutput
    {
		public CreateOrEditServiceDto Service { get; set; }


    }
}