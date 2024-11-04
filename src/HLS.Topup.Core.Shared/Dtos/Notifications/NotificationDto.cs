using System;

namespace HLS.Topup.Dtos.Notifications
{
    public class NotificationDataDto
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string ExtraInfo { get; set; }
        public Guid NotificationId { get; set; }
        public int NotificationTempId { get; set; }
        public string NotifiTypeCode { get; set; }
        public string Icon { get; set; } = "logo.png";
        public string IconNotifi { get; set; }
        public string Image { get; set; }
        public string Tag { get; set; }
        public string Slug { get; set; }
        public ScreenActionDto ScreenAction { get; set; }
        public int TotalUnRead { get; set; }
        public int? TenanId { get; set; }
        public string Type { get; set; } = NotificationType.Notification;
        public string AppNotificationName { get; set; }
        public string Link { get; set; }
    }
    public static class NotificationType
    {
        public const string Notification = "Notification";
    }

    public class ScreenActionDto
    {
        public string Screen { get; set; }
        public ParamsScreenDto Params { get; set; }
    }

    public class ParamsScreenDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
    }
}
