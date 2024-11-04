(function () {
    $(function () {
        var _$notificationsTable = $('#NotificationsTable');
        var _notificationService = abp.services.app.notificationManagement;

        var _$targetValueFilterSelectionCombobox = $('#TargetValueFilterSelectionCombobox');
        _$targetValueFilterSelectionCombobox.selectpicker();

        var _appUserNotificationHelper = new app.UserNotificationHelper();

        var _selectedDateRangeNotification = {
            startDate: moment().startOf('day').subtract(7, 'days'),
            endDate: moment().endOf('day')
        };

        $(document).find('input.date-range-picker').daterangepicker(
            $.extend(true, app.createDateRangePickerOptions(), _selectedDateRangeNotification),
            function (start, end) {
                _selectedDateRangeNotification.startDate = start.format('YYYY-MM-DDT00:00:00Z');
                _selectedDateRangeNotification.endDate = end.format('YYYY-MM-DDT23:59:59.999Z');
                getNotifications();
            });

        var dataTable = _$notificationsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _notificationService.getUserNotifications,
                inputFilter: function () {
                    return {
                        state: _$targetValueFilterSelectionCombobox.val(),
                        startDate: _selectedDateRangeNotification.startDate,
                        endDate: _selectedDateRangeNotification.endDate
                    };
                }
            },
            columnDefs: [
                // {
                //     className: 'control responsive',
                //     orderable: false,
                //     render: function () {
                //         return '';
                //     },
                //     targets: 0
                // },
                // {
                //     targets: 3,
                //     data: null,
                //     orderable: false,
                //     defaultContent: '',
                //     createdCell: function (td, cellData, rowData, rowIndex, colIndex) {
                //         createNotificationReadButton($(td), rowData);
                //     }
                // },
                // {
                //     targets: 2,
                //     data: "severity",
                //     render: function (severity, type, row, meta) {
                //         var icon = app.notification.getUiIconBySeverity(row.notification.severity);
                //         var iconFontClass = app.notification.getIconFontClassBySeverity(row.notification.severity);
                //         var $span = $('<span></span>');
                //         var $icon = $('<i class="' + icon + ' ' + iconFontClass + ' fa-2x"></i>');
                //         $span
                //             .append($icon)
                //             .append("<br>")
                //             .append(getNotificationTextBySeverity(row.notification.severity));
                //
                //         return $span[0].outerHTML;
                //     }
                // },
                {
                    targets: 0,
                    data: "notification",
                    render: function (notification, type, row, meta) {
                        let $container;
                        const formattedRecord = _appUserNotificationHelper.format(row, false);
                        let rowClass = getRowClass(formattedRecord);
                        rowClass = rowClass === 'notification-read' ? 'notification-read' : 'notification-unread';
                        let timeAgo = '<br><small style="color: #2188C9; font-weight: 500;"><i class="fa fa-clock"></i> ' + formattedRecord.timeAgo + '</small>';
                        if (formattedRecord.url && formattedRecord.url !== '#') {
                            $container = $('<span class="preformatted ' + rowClass + '"><a title="' + formattedRecord.text + '" href="' + formattedRecord.url + '">' + abp.utils.truncateStringWithPostfix(formattedRecord.text, 1000).trim() + '</a></span>');
                        } else {
                            $container = $('<span class="preformatted ' + rowClass + '" title="' + formattedRecord.text + '"><i class="fa fa-bell" style="color: #2188C9; padding-right: 10px;"></i>' + abp.utils.truncateStringWithPostfix(formattedRecord.text, 1000).trim() + timeAgo + '</span>');
                        }

                        $('span.notification-unread').parent().closest('tr').addClass('notify-unread').attr('data-id', row.id);

                        return $container[0].outerHTML;
                    }
                },
                // {
                //     targets: 2,
                //     data: "creationTime",
                //     render: function (creationTime, type, row, meta) {
                //         var formattedRecord = _appUserNotificationHelper.format(row);
                //         var rowClass = getRowClass(formattedRecord);
                //         var $container = $('<span title="' + moment(row.notification.creationTime).format("llll") + '" class="' + rowClass + '">' + formattedRecord.timeAgo + '</span> &nbsp;');
                //         return $container[0].outerHTML;
                //     }
                // },
                {
                    // targets: 1,
                    // data: null,
                    // render: function (notification, type, row, meta) {
                    //     let $container;
                    //     let asRead = '';
                    //     const formattedRecord = _appUserNotificationHelper.format(row, false);
                    //     let rowClass = getRowClass(formattedRecord);
                    //
                    //     if (rowClass.length === 0) {
                    //         asRead = '<span class="mark-read-notify" style="color: #004e7f; cursor: pointer;" data-notify-id-custom="' + row.notification.id + '"><i class="fa fa-check-circle"></i> ' + app.localize('SetAsReadSimple') + '</span><br>';
                    //     }
                    //
                    //     return asRead + '<span class="delete-notify" style="color: #DC4E41; cursor: pointer;" data-notify-id-custom="' + row.notification.id + '"><i class="fa fa-trash"></i> ' + app.localize('Delete') + '</span>';
                    // },

                    targets: 1,
                    data: null,
                    orderable: false,
                    defaultContent: '',
                    createdCell: function (td, cellData, rowData, rowIndex, colIndex) {
                        createNotificationReadButton($(td), rowData);
                    }
                }
            ]
        });

        var createNotificationReadButton = function ($td, record) {

            var $span = $('<span/>');
            var $button = $("<button/>")
                .addClass("mark-read-notify")
                .attr("title", app.localize('SetAsRead'))
                .click(function (e) {
                    e.preventDefault();
                    setNotificationAsRead(record, function () {
                        $button.find('i')
                            .removeClass('fa-check-square')
                            .addClass('la-check');
                        $button.attr('disabled', 'disabled');
                        $td.closest("tr").addClass("notification-read");
                    });
                }).appendTo($span);

            var $buttonDelete = $("<button/>")
                .addClass("delete-notify")
                .attr("title", app.localize('Delete'))
                .click(function () {
                    deleteNotification(record);
                }).appendTo($span);
            $('<i class="la la-remove" >').appendTo($buttonDelete);

            var $i = $('<i class="la" >').appendTo($button);
            var notificationState = _appUserNotificationHelper.format(record).state;

            if (notificationState === 'READ') {
                $button.attr('disabled', 'disabled');
                $i.addClass('la-check');
            }

            if (notificationState === 'UNREAD') {
                $i.addClass('fa-check-square');
            }

            $td.append($span);
        };

        function deleteNotification(record) {
            abp.message.confirm(
                app.localize('NotificationDeleteWarningMessage'),
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _notificationService.notificationDelete({
                            id: record.id
                        }).done(function () {
                            getNotifications();
                            abp.message.success(app.localize('SuccessfullyDeleted'));
                        });
                    }
                }
            );
        };

        function deleteNotifications() {
            abp.message.confirm(
                app.localize('DeleteListedNotificationsWarningMessage'),
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _notificationService.deleteAllUserNotifications(
                            {
                                state: _$targetValueFilterSelectionCombobox.val(),
                                startDate: _selectedDateRangeNotification.startDate,
                                endDate: _selectedDateRangeNotification.endDate
                            })
                            .done(function () {
                                getNotifications();
                                abp.message.success(app.localize('SuccessfullyDeleted'));
                            });
                    }
                }
            );
        };

        function getRowClass(formattedRecord) {
            return formattedRecord.state === 'READ' ? 'notification-read' : '';
        }

        function getNotifications() {
            dataTable.ajax.reload();
        }

        function setNotificationAsRead(record, callback) {
            _appUserNotificationHelper.setAsRead(record.id, function () {
                if (callback) {
                    callback();
                }
            });
        }

        function setAllNotificationsAsRead() {
            _appUserNotificationHelper.setAllAsRead(function () {
                getNotifications();
            });
        };

        function openNotificationSettingsModal() {
            _appUserNotificationHelper.openSettingsModal();
        };

        _$targetValueFilterSelectionCombobox.change(function () {
            getNotifications();
        });

        $('#RefreshNotificationTableButton').click(function (e) {
            e.preventDefault();
            getNotifications();
        });

        $('#btnOpenNotificationSettingsModal').click(function (e) {
            openNotificationSettingsModal();
        });

        $('#btnSetAllNotificationsAsRead').click(function (e) {
            e.preventDefault();
            setAllNotificationsAsRead();
        });

        $('#DeleteAllNotificationsButton').click(function (e) {
            e.preventDefault();
            deleteNotifications();
        });

        // $(document).on('click', '.delete-notify', function () {
        //     let id = $(this).attr('data-notify-id-custom');
        //     console.log(log)
        //     deleteNotification(id);
        // });
        //
        // $(document).on('click', '.mark-read-notify', function () {
        //     let id = $(this).attr('data-notify-id-custom');
        //     setNotificationAsRead(id, null);
        //     $(this).closest('tr').removeClass('notify-unread');
        //     $(this).hide();
        // });
    });
})();
