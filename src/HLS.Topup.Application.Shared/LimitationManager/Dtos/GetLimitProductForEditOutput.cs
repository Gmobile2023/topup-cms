using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace HLS.Topup.LimitationManager.Dtos
{
    public class GetLimitProductForEditOutput
    {
		public CreateOrEditLimitProductDto LimitProduct { get; set; }

		public string UserName { get; set;}
		
		public string AgentName { get; set;}

		public string UserApproved { get; set;}
		
		public DateTime CreationTime { get; set;}
    }
}