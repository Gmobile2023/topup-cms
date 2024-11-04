using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace HLS.Topup.Notifications.Dtos
{
    public class GetNotificationScheduleForEditOutput
    {
		public CreateOrEditNotificationScheduleDto NotificationSchedule { get; set; }

		public string UserName { get; set;}


    }
}