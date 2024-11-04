using HLS.Topup.Common;

using System;
using Abp.Application.Services.Dto;

namespace HLS.Topup.Notifications.Dtos
{
    public class NotificationScheduleDto : EntityDto
    {
		public string Code { get; set; }

		public string Name { get; set; }
		public string Body { get; set; }

		public string Title { get; set; }

		public DateTime DateSchedule { get; set; }

		public DateTime? DateSend { get; set; }

		public CommonConst.SendNotificationStatus Status { get; set; }

		public CommonConst.SystemAccountType AccountType { get; set; }

		public CommonConst.AgentType AgentType { get; set; }

		public DateTime? DateApproved { get; set; }


		 public long? UserId { get; set; }


    }
}
