using System;
using Abp.Application.Services.Dto;
using Abp.Notifications;
using HLS.Topup.Dto;

namespace HLS.Topup.Notifications.Dto
{
    public class GetUserNotificationsInput : PagedInputDto
    {
        public UserNotificationState? State { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }


    public class GetUserNotificationInput : PagedAndSortedResultRequestDto
    {
        public string NotificationType { get; set; }
        public int? State { get; set; }
        public string AccountCode { get; set; }
        public bool? IsTotalOnly { get; set; }
    }

    public class GetLastNotificationInput : PagedAndSortedResultRequestDto
    {
        public string NotificationType { get; set; }
        public int? State { get; set; }
        public string AccountCode { get; set; }
        public bool? IsTotalOnly { get; set; }
    }
}
