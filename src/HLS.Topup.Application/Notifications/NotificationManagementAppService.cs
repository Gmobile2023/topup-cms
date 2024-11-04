using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.UI;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Dtos.Notifications;
using HLS.Topup.Notifications.Dto;
using HLS.Topup.RequestDtos;
using Microsoft.Extensions.Logging;
using ServiceStack;

namespace HLS.Topup.Notifications
{
    public class NotificationManagementAppService : TopupAppServiceBase, INotificationManagementAppService
    {
        private readonly INotificationManger _notificationManger;
        private readonly TopupAppSession _topupAppSession;
        private readonly ILogger<NotificationManagementAppService> _logger;

        public NotificationManagementAppService(INotificationManger notificationManger, TopupAppSession topupAppSession,
            ILogger<NotificationManagementAppService> logger)
        {
            _notificationManger = notificationManger;
            _topupAppSession = topupAppSession;
            _logger = logger;
        }
        [AbpAuthorize]
        public async Task<PagedResultDto<NotificationAppOutDto>> GetUserNotifications(
            GetUserNotificationInput request)
        {
            var rs = await _notificationManger.GetUserNotifications(new GetUserNotificationRequest
            {
                Limit = request.MaxResultCount,
                Offset = request.SkipCount,
                AccountCode = _topupAppSession.AccountCode,
                State = request.State,
                NotificationType = request.NotificationType,
                IsTotalOnly = request.IsTotalOnly
            });
            return new PagedResultDto<NotificationAppOutDto>(
                rs.Total,
                rs.Payload.ConvertTo<List<NotificationAppOutDto>>());
        }
        [AbpAuthorize]
        public async Task<ShowNotificationDto> GetLastNotifications(GetLastNotificationInput request)
        {
            var rs = await _notificationManger.GetLastNotificationRequest(new GetLastNotificationRequest()
            {
                Limit = request.MaxResultCount,
                Offset = request.SkipCount,
                AccountCode = _topupAppSession.AccountCode,
                State = request.State,
                NotificationType = request.NotificationType,
                IsTotalOnly = request.IsTotalOnly
            });
            return rs.Result;
        }
        [AbpAuthorize]
        public async Task<NotificationAppOutDto> GetNotification(GetNotificationRequest request)
        {
            request.AccountCode = _topupAppSession.AccountCode;
            var rs = await _notificationManger.GetNotification(request);
            return rs.Result;
        }
        [AbpAuthorize]
        public async Task SetAllNotificationsAsRead(SetAllNotificationsAsReadRequest request)
        {
            request.AccountCode = _topupAppSession.AccountCode;
            var rs = await _notificationManger.SetAllNotificationsAsRead(request);
            if (!rs.Success)
                throw new UserFriendlyException(rs.Error.Message);
        }
        [AbpAuthorize]
        public async Task SetNotificationAsRead(SetNotificationAsReadRequest request)
        {
            request.AccountCode = _topupAppSession.AccountCode;
            var rs = await _notificationManger.SetNotificationAsRead(request);
            if (!rs.Success)
                throw new UserFriendlyException(rs.Error.Message);
        }
        [AbpAuthorize]
        public async Task DeleteNotificationAccount(EntityDto<Guid> input)
        {
            try
            {
                _logger.LogInformation($"DeleteNotificationAccount request:{input.ToJson()}");
                var rs = await _notificationManger.DeleteNotification(new DeleteNotificationRequest
                    {AccountCode = _topupAppSession.AccountCode, Id = input.Id});
                if (!rs.Success)
                    throw new UserFriendlyException("Xóa không thành công");
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
        [AbpAuthorize]
        public async Task NotificationDelete(EntityDto<Guid> input)
        {
            try
            {
                _logger.LogInformation($"NotificationDelete request:{input.ToJson()}");
                var rs = await _notificationManger.DeleteNotification(new DeleteNotificationRequest
                    {AccountCode = _topupAppSession.AccountCode, Id = input.Id});
                if (!rs.Success)
                    throw new UserFriendlyException("Xóa không thành công");
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
        [AbpAuthorize]
        public async Task Subscribe(SubscribeNotificationRequest request)
        {
            _logger.LogInformation($"SubscribeNotificationRequest request:{request.ToJson()}");
            request.AccountCode = _topupAppSession.AccountCode;
            var rs = await _notificationManger.SubscribeNotification(request);
            _logger.LogInformation($"SubscribeNotificationRequest return:{rs.ToJson()}");
        }
        public async Task UnSubscribe(UnSubscribeNotificationRequest request)
        {
            _logger.LogInformation($"UnSubscribe request:{request.ToJson()}");
            var rs = await _notificationManger.UnSubscribeNotification(request);
            _logger.LogInformation($"UnSubscribe return:{rs.ToJson()}");
        }
        [AbpAuthorize]
        public async Task TestRegisterNotification(string accountcode)
        {
            await _notificationManger.SendNotification(new SendNotificationRequest
            {
                Body = "Xin chào bạn. Chúc mừng bạn đã nhận được 50.000đ từ tài khoản NT432434",
                Title = "Thông báo chuyển tiền",
                AccountCode = !string.IsNullOrEmpty(accountcode) ? accountcode : _topupAppSession.AccountCode
            });
        }
    }
}
