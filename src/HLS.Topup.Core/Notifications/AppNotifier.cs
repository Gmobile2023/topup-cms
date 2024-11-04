using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp;
using Abp.Localization;
using Abp.Notifications;
using HLS.Topup.Authorization.Users;
using HLS.Topup.MultiTenancy;

namespace HLS.Topup.Notifications
{
    public class AppNotifier : TopupDomainServiceBase, IAppNotifier
    {
        private readonly INotificationPublisher _notificationPublisher;

        public AppNotifier(INotificationPublisher notificationPublisher)
        {
            _notificationPublisher = notificationPublisher;
        }

        public async Task WelcomeToTheApplicationAsync(User user)
        {
            await _notificationPublisher.PublishAsync(
                AppNotificationNames.WelcomeToTheApplication,
                new MessageNotificationData(L("WelcomeToTheApplicationNotificationMessage")),
                severity: NotificationSeverity.Success,
                userIds: new[] {user.ToUserIdentifier()}
            );
        }

        public async Task NewUserRegisteredAsync(User user)
        {
            var notificationData = new LocalizableMessageNotificationData(
                new LocalizableString(
                    "NewUserRegisteredNotificationMessage",
                    TopupConsts.LocalizationSourceName
                )
            );

            notificationData["userName"] = user.UserName;
            notificationData["emailAddress"] = user.EmailAddress;

            await _notificationPublisher.PublishAsync(AppNotificationNames.NewUserRegistered, notificationData,
                tenantIds: new[] {user.TenantId});
        }

        public async Task NewTenantRegisteredAsync(Tenant tenant)
        {
            var notificationData = new LocalizableMessageNotificationData(
                new LocalizableString(
                    "NewTenantRegisteredNotificationMessage",
                    TopupConsts.LocalizationSourceName
                )
            );

            notificationData["tenancyName"] = tenant.TenancyName;
            await _notificationPublisher.PublishAsync(AppNotificationNames.NewTenantRegistered, notificationData);
        }

        public async Task GdprDataPrepared(UserIdentifier user, Guid binaryObjectId)
        {
            var notificationData = new LocalizableMessageNotificationData(
                new LocalizableString(
                    "GdprDataPreparedNotificationMessage",
                    TopupConsts.LocalizationSourceName
                )
            );

            notificationData["binaryObjectId"] = binaryObjectId;

            await _notificationPublisher.PublishAsync(AppNotificationNames.GdprDataPrepared, notificationData,
                userIds: new[] {user});
        }

        //This is for test purposes
        public async Task SendMessageAsync(UserIdentifier user, string message,
            NotificationSeverity severity = NotificationSeverity.Info)
        {
            await _notificationPublisher.PublishAsync(
                AppNotificationNames.SimpleMessage,
                new MessageNotificationData(message),
                severity: severity,
                userIds: new[] {user}
            );
        }

        public Task SendMessageAsync(UserIdentifier user, LocalizableString localizableMessage,
            IDictionary<string, object> localizableMessageData = null,
            NotificationSeverity severity = NotificationSeverity.Info)
        {
            return SendNotificationAsync(AppNotificationNames.SimpleMessage, user, localizableMessage,
                localizableMessageData, severity);
        }

        protected async Task SendNotificationAsync(string notificationName, UserIdentifier user,
            LocalizableString localizableMessage, IDictionary<string, object> localizableMessageData = null,
            NotificationSeverity severity = NotificationSeverity.Info)
        {
            var notificationData = new LocalizableMessageNotificationData(localizableMessage);
            if (localizableMessageData != null)
            {
                foreach (var pair in localizableMessageData)
                {
                    notificationData[pair.Key] = pair.Value;
                }
            }

            await _notificationPublisher.PublishAsync(notificationName, notificationData, severity: severity,
                userIds: new[] {user});
        }

        public Task TenantsMovedToEdition(UserIdentifier user, string sourceEditionName, string targetEditionName)
        {
            return SendNotificationAsync(AppNotificationNames.TenantsMovedToEdition, user,
                new LocalizableString(
                    "TenantsMovedToEditionNotificationMessage",
                    TopupConsts.LocalizationSourceName
                ),
                new Dictionary<string, object>
                {
                    {"sourceEditionName", sourceEditionName},
                    {"targetEditionName", targetEditionName}
                });
        }

        public Task<TResult> TenantsMovedToEdition<TResult>(UserIdentifier argsUser, int sourceEditionId,
            int targetEditionId)
        {
            throw new NotImplementedException();
        }

        public Task SomeUsersCouldntBeImported(UserIdentifier user, string fileToken, string fileType, string fileName)
        {
            return SendNotificationAsync(AppNotificationNames.DownloadInvalidImportUsers, user, 
                new LocalizableString(
                    "ClickToSeeInvalidUsers",
                    TopupConsts.LocalizationSourceName
                ), 
                new Dictionary<string, object>
                {
                    { "fileToken", fileToken },
                    { "fileType", fileType },
                    { "fileName", fileName }
                });
        }

        public async Task GdUrllinkDownload(UserIdentifier user,string link)
        {
            var notificationData = new LocalizableMessageNotificationData(
                new LocalizableString(
                    "GdprDataPreparedNotificationMessage",
                    TopupConsts.LocalizationSourceName
                )
            );

            notificationData["LinkDownload"] = link;
            await _notificationPublisher.PublishAsync(AppNotificationNames.GdUrllinkDownload, notificationData,
                userIds: new[] { user });
        }
    }
}
