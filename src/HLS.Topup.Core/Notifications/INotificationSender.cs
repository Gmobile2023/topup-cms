using System.Threading.Tasks;
using Abp.Notifications;
using HLS.Topup.Dtos.Notifications;
using HLS.Topup.RequestDtos;

namespace HLS.Topup.Notifications
{
    public interface INotificationSender
    {
        Task SendNotificationAsync(string notificationName, string accountcode, string message, string title,
            NotificationSeverity severity = NotificationSeverity.Info);

        Task SendNotificationDataAsync(string accountcode,string notificationName,SendNotificationData data, string message, string title,
            NotificationSeverity severity = NotificationSeverity.Info);

        Task PublishNotification(string accountcode,string notificationName,SendNotificationData data, string message, string title,
            NotificationSeverity severity = NotificationSeverity.Info);

        Task PublishTeleMessage(SendTeleMessageRequest request);
        Task PublishTeleToGroupMessage(SendTeleMessageRequest request);
    }
}
