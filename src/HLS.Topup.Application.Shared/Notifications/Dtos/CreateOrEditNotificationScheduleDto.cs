using HLS.Topup.Common;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace HLS.Topup.Notifications.Dtos
{
    public class CreateOrEditNotificationScheduleDto : EntityDto<int?>
    {

		[Required]
		[StringLength(NotificationScheduleConsts.MaxNameLength, MinimumLength = NotificationScheduleConsts.MinNameLength)]
		public string Name { get; set; }


		[Required]
		[StringLength(NotificationScheduleConsts.MaxTitleLength, MinimumLength = NotificationScheduleConsts.MinTitleLength)]
		public string Title { get; set; }


		[Required]
		[StringLength(NotificationScheduleConsts.MaxBodyLength, MinimumLength = NotificationScheduleConsts.MinBodyLength)]
		public string Body { get; set; }


		public DateTime DateSchedule { get; set; }


		public CommonConst.SystemAccountType AccountType { get; set; }


		public CommonConst.AgentType AgentType { get; set; }


		[StringLength(NotificationScheduleConsts.MaxDescriptionLength, MinimumLength = NotificationScheduleConsts.MinDescriptionLength)]
		public string Description { get; set; }


		 public long? UserId { get; set; }


    }
}
