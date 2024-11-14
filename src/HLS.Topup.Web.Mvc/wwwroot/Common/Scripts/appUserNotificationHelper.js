var app = app || {};
(function ($) {

    app.UserNotificationHelper = (function () {

        return function () {

            /* Message Extracting based on Notification Data Type ********/

            //add your custom notification data types here...

            /* Example:
            abp.notifications.messageFormatters['HLS.Topup.MyNotificationDataType'] = function(userNotification) {
                return ...; //format and return message here
            };
            */

            //var _notificationService = abp.services.app.notification;
            var _notificationManagenmentService = abp.services.app.notificationManagement;

            /* Converter functions ***************************************/

            function getUrl(userNotification) {
                
                switch (userNotification.notification.notificationName) {
                    case 'App.NewUserRegistered':
                        return '/App/users?filterText=' + userNotification.notification.data.properties.emailAddress;
                    case 'App.NewTenantRegistered':
                        return '/App/tenants?filterText=' + userNotification.notification.data.properties.tenancyName;
                    case 'App.GdprDataPrepared':
                        return '/File/DownloadBinaryFile?id=' + userNotification.notification.data.properties.binaryObjectId + '&contentType=application/zip&fileName=collectedData.zip';
                    case 'App.DownloadInvalidImportUsers':
                        return '/File/DownloadTempFile?fileToken=' + userNotification.notification.data.properties.fileToken + '&fileType=' + userNotification.notification.data.properties.fileType + '&fileName=' + userNotification.notification.data.properties.fileName;
                    case 'App.GdUrllinkDownload':
                        return userNotification.notification.data.properties.linkDownload ;
                    //Add your custom notification names to navigate to a URL when user clicks to a notification.
                }

                //No url for this notification
                return '/Profile/Notifications';
            }

            function bindNotificationEvents() {
                $('#setAllNotificationsAsReadLink').click(function (e) {
                    e.preventDefault();
                    e.stopPropagation();

                    _appUserNotificationHelper.setAllAsRead(function () {
                        loadNotifications();
                    });
                });

                $('.set-notification-as-read').click(function (e) {
                    e.preventDefault();
                    e.stopPropagation();

                    var notificationId = $(this).attr("data-notification-id");
                    if (notificationId) {
                        _appUserNotificationHelper.setAsRead(notificationId, function () {
                            loadNotifications();
                        });
                    }
                });

                $('.openNotificationSettingsModalLink').click(function (e) {
                    e.preventDefault();
                    e.stopPropagation();

                    _appUserNotificationHelper.openSettingsModal();
                });

                $('div.user-notification-item-clickable').click(function () {
                    var url = $(this).attr('data-url');
                    document.location.href = url;
                });
            }


            var format = function (userNotification, truncateText) {
                var data = JSON.parse(userNotification.data);
                var formatted = {
                    userNotificationId: userNotification.id,
                    text: userNotification.body,//abp.notifications.getFormattedMessageFromUserNotification(userNotification),
                    time: moment(userNotification.createdDate).format("YYYY-MM-DD HH:mm:ss"),//moment(userNotification.notification.creationTime).format("YYYY-MM-DD HH:mm:ss"),
                    icon: 'fas fa-info-circle  text-info fa-2x',//app.notification.getUiIconBySeverity(userNotification.notification.severity),
                    state: userNotification.state,//abp.notifications.getUserNotificationStateAsString(userNotification.state),
                    data: data,//userNotification.notification.data,
                    url: userNotification.link,//getUrl(userNotification),
                    isUnread: userNotification.state === 0,//userNotification.state === abp.notifications.userNotificationState.UNREAD,
                    isread: userNotification.state === 1,
                    timeAgo: moment(userNotification.createdDate).fromNow()//moment(userNotification.notification.creationTime).fromNow(),
                    //iconFontClass: app.notification.getIconFontClassBySeverity(userNotification.notification.severity)
                };

                if (truncateText || truncateText === undefined) {
                    formatted.text = abp.utils.truncateStringWithPostfix(formatted.text, 50);
                }

                return formatted;
            };

            var show = function (userNotification) {
                //Application notification
                abp.notifications.showUiNotifyForUserNotification(userNotification, {
                    'onclick': function () {
                        //Take action when user clicks to live toastr notification
                        var url = getUrl(userNotification);
                        if (url) {
                            location.href = url;
                        }
                    }
                });

                //Desktop notification
                Push.create("Topup", {
                    body: format(userNotification).text,
                    icon: abp.appPath + 'Common/Images/app-logo-small.svg',
                    timeout: 6000,
                    onClick: function () {
                        window.focus();
                        this.close();
                    }
                });
            };

            var setAllAsRead = function (callback) {
                _notificationManagenmentService.setAllNotificationsAsRead().done(function () {
                    abp.event.trigger('app.notifications.refresh');
                    callback && callback();
                });
            };

            var setAsRead = function (userNotificationId, callback) {
                _notificationManagenmentService.setNotificationAsRead({
                    id: userNotificationId
                }).done(function () {
                    abp.event.trigger('app.notifications.read', userNotificationId);
                    callback && callback(userNotificationId);
                });
            };
            function showRingBell() {
                var el = document.querySelector('.ring-bell-notifi');
                if (el != undefined) {
                    var count = Number(el.getAttribute('data-count')) || 0;
                    if (count > 0) {
                        el.setAttribute('data-count', count);
                        el.classList.remove('notify');
                        el.offsetWidth = el.offsetWidth;
                        el.classList.add('notify');
                        el.classList.add('show-count');
                    }
                }
            }
            var openSettingsModal = function () {
                new app.ModalManager({
                    viewUrl: abp.appPath + 'App/Notifications/SettingsModal',
                    scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Notifications/_SettingsModal.js',
                    modalClass: 'NotificationSettingsModal',
                    modalSize: 'modal-md'
                }).open();
            };
            var loadNotifications = function () {
                _notificationManagenmentService.getLastNotifications({
                    maxResultCount: 3
                }).done(function (result) {
                    console.log('test' + result);
                    result.notifications = [];
                    result.unreadMessageExists = result.totalUnRead > 0;
                    result.unreadCount = result.totalUnRead;
                    $.each(result.lastNotification, function (index, item) {
                        var itemNotifi = format(item, false);
                        //item.isUnread = item.state === 0;
                        //item.text = abp.utils.truncateStringWithPostfix(item.body, 70);
                        //item.userNotificationId = item.id;
                        //item.timeAgo = moment(item.creationTime).fromNow();
                        result.notifications.push(itemNotifi);
                    });

                    var $li = $('.header_notification_bar');
                    var template = $('#headerNotificationBarTemplate').html();
                    Mustache.parse(template);
                    var rendered = Mustache.render(template, result);

                    $li.html(rendered);
                    showRingBell();
                    bindNotificationEvents();
                });
            };

            /* Expose public API *****************************************/

            return {
                format: format,
                show: show,
                setAllAsRead: setAllAsRead,
                setAsRead: setAsRead,
                openSettingsModal: openSettingsModal,
                loadNotifications: loadNotifications,
                getUrl: getUrl
            };

        };

    })();

})(jQuery);
