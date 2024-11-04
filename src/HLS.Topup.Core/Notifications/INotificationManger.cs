using System.Collections.Generic;
using System.Threading.Tasks;
using HLS.Topup.Dtos.Notifications;
using HLS.Topup.Dtos.Stock;
using HLS.Topup.RequestDtos;

namespace HLS.Topup.Notifications
{
    public interface INotificationManger
    {
        Task<ResponseMessageApi<bool>> SendNotification (SendNotificationRequest request);
        Task<ApiResponseDto<List<NotificationAppOutDto>>> GetUserNotifications(GetUserNotificationRequest request);
        Task<ResponseMessageApi<NotificationAppOutDto>> GetNotification(GetNotificationRequest request);
        Task<ResponseMessageApi<ShowNotificationDto>> GetLastNotificationRequest(GetLastNotificationRequest request);
        Task<ResponseMessageApi<bool>> SetAllNotificationsAsRead(SetAllNotificationsAsReadRequest request);
        Task<ResponseMessageApi<bool>> SetNotificationAsRead(SetNotificationAsReadRequest request);
        Task<ResponseMessageApi<bool>> DeleteNotification(DeleteNotificationRequest request);
        Task<ResponseMessageApi<bool>> SubscribeNotification(SubscribeNotificationRequest request);
        Task<ResponseMessageApi<bool>> UnSubscribeNotification(UnSubscribeNotificationRequest request);
    }
}
