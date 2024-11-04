using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using HLS.Topup.Dtos.Notifications;
using HLS.Topup.Notifications.Dto;
using HLS.Topup.RequestDtos;

namespace HLS.Topup.Notifications
{
    public interface INotificationManagementAppService : IApplicationService
    {
        Task<PagedResultDto<NotificationAppOutDto>> GetUserNotifications(GetUserNotificationInput request);
        Task<ShowNotificationDto> GetLastNotifications(GetLastNotificationInput request);
        Task<NotificationAppOutDto> GetNotification(GetNotificationRequest request);
        Task SetAllNotificationsAsRead(SetAllNotificationsAsReadRequest request);
        Task SetNotificationAsRead(SetNotificationAsReadRequest request);
        Task DeleteNotificationAccount(EntityDto<Guid> input);
        Task NotificationDelete(EntityDto<Guid> input);
        Task Subscribe(SubscribeNotificationRequest request);
        Task UnSubscribe(UnSubscribeNotificationRequest request);
        Task TestRegisterNotification(string mobile);
    }
}
