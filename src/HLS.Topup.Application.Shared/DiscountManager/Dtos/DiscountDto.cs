using HLS.Topup.Common;

using System;
using Abp.Application.Services.Dto;

namespace HLS.Topup.DiscountManager.Dtos
{
    public class DiscountDto : EntityDto
    {
		public string Code { get; set; }

		public string Name { get; set; }
		public string StatusName { get; set; }

		public DateTime FromDate { get; set; }

		public DateTime ToDate { get; set; }

		public DateTime? DateApproved { get; set; }

		public CommonConst.DiscountStatus Status { get; set; }

		public CommonConst.AgentType AgentType { get; set; }


		 public long? UserId { get; set; }


    }
}