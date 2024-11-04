using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace HLS.Topup.FeeManager.Dtos
{
    public class GetFeeForEditOutput
    {
		public CreateOrEditFeeDto Fee { get; set; }

		public string UserName { get; set;}
		
    }
}