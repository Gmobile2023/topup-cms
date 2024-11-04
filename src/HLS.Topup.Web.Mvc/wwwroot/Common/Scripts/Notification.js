(function ($) {
    $(function () {


        // Notifications
        var _appUserNotificationHelper = new app.UserNotificationHelper();
        //var _notificationService = abp.services.app.notification;
        var _notificationManagenmentService = abp.services.app.notificationManagement;

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

            $('li.user-notification-item-clickable').click(function () {
                var url = $(this).attr('data-url');
                document.location.href = url;
            });
        }
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

        function loadNotifications() {
            // _notificationService.getUserNotifications({
            //     maxResultCount: 3
            // }).done(function (result) {
            //     result.notifications = [];
            //     result.unreadMessageExists = result.unreadCount > 0;
            //     $.each(result.items, function (index, item) {
            //         var formattedItem = _appUserNotificationHelper.format(item);
            //         result.notifications.push(formattedItem);
            //     });
            //
            //     var $li = $('.header_notification_bar');
            //
            //     var template = $('#headerNotificationBarTemplate').html();
            //     //console.log(template);
            //     Mustache.parse(template);
            //     var rendered = Mustache.render(template, result);
            //
            //     $li.html(rendered);
            //
            //     bindNotificationEvents();
            //    
            //    
            // });

            _notificationManagenmentService.getLastNotifications({
                maxResultCount: 3
            }).done(function (result) {
                console.log('test' + result);
                result.notifications = [];
                result.unreadMessageExists = result.totalUnRead > 0;
                result.unreadCount = result.totalUnRead;
                $.each(result.lastNotification, function (index, item) {
                    var itemNotifi = _appUserNotificationHelper.format(item, false);
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
        }

        abp.event.on('abp.notifications.received', function (userNotification) {
            _appUserNotificationHelper.show(userNotification);
            loadNotifications();
        });

        abp.event.on('app.notifications.refresh', function () {
            loadNotifications();
        });

        abp.event.on('app.notifications.read', function (userNotificationId) {
            loadNotifications();
        });

        // Init

        function init() {
            loadNotifications();
        }

        init();
    });
})(jQuery);
