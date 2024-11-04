using System.Threading.Tasks;

namespace HLS.Topup.Notifications
{
    public interface INotificationScheduleManager
    {
        Task PublishNotifications(int messageId);
        Task ScheduleNotification(NotificationSchedule message);
        Task SendNowNotification(int messageId);
    }
}
