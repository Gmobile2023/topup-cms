using HLS.Topup.Notifications.Dtos;
using System.Collections.Generic;

using Abp.Extensions;

namespace HLS.Topup.Web.Areas.App.Models.NotificationSchedules
{
    public class CreateOrEditNotificationScheduleModalViewModel
    {
       public CreateOrEditNotificationScheduleDto NotificationSchedule { get; set; }

	   		public string UserName { get; set;}


       public List<NotificationScheduleUserLookupTableDto> NotificationScheduleUserList { get; set;}


	   public bool IsEditMode => NotificationSchedule.Id.HasValue;
    }
}