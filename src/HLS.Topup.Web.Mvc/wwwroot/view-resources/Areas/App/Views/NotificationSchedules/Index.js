(function () {
    $(function () {

        var _$notificationSchedulesTable = $('#NotificationSchedulesTable');
        var _notificationSchedulesService = abp.services.app.notificationSchedules;

        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.NotificationSchedules.Create'),
            edit: abp.auth.hasPermission('Pages.NotificationSchedules.Edit'),
            'delete': abp.auth.hasPermission('Pages.NotificationSchedules.Delete'),
            approval: abp.auth.hasPermission('Pages.NotificationSchedules.Approval'),
            cancel: abp.auth.hasPermission('Pages.NotificationSchedules.Cancel'),
            send: abp.auth.hasPermission('Pages.NotificationSchedules.Send'),
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/NotificationSchedules/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/NotificationSchedules/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditNotificationScheduleModal',
            modalSize: 'modal-xl'
        });

        var _viewNotificationScheduleModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/NotificationSchedules/ViewnotificationScheduleModal',
            modalClass: 'ViewNotificationScheduleModal'
        });


        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z");
        }

        var dataTable = _$notificationSchedulesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _notificationSchedulesService.getAll,
                inputFilter: function () {
                    return {
                        filter: $('#NotificationSchedulesTableFilter').val(),
                        codeFilter: $('#CodeFilterId').val(),
                        nameFilter: $('#NameFilterId').val(),
                        statusFilter: $('#StatusFilterId').val(),
                        accountTypeFilter: $('#AccountTypeFilterId').val(),
                        agentTypeFilter: $('#AgentTypeFilterId').val(),
                        userNameFilter: $('#UserNameFilterId').val()
                    };
                }
            },
            columnDefs: [
                {
                    className: 'control responsive',
                    orderable: false,
                    render: function () {
                        return '';
                    },
                    targets: 0
                },
                {
                    width: 120,
                    targets: 1,
                    data: null,
                    orderable: false,
                    autoWidth: false,
                    defaultContent: '',
                    rowAction: {
                        cssClass: 'btn btn-brand dropdown-toggle',
                        text: '<i class="fa fa-cog"></i> ' + app.localize('Actions') + ' <span class="caret"></span>',
                        items: [
                            {
                                text: app.localize('View'),
                                action: function (data) {
                                    _viewNotificationScheduleModal.open({id: data.record.notificationSchedule.id});
                                }
                            },
                            {
                                text: app.localize('Edit'),
                                visible: function (data) {
                                    return _permissions.edit && data.record.notificationSchedule.status === 0;
                                },
                                action: function (data) {
                                    _createOrEditModal.open({id: data.record.notificationSchedule.id});
                                }
                            },
                            {
                                text: app.localize('Delete'),
                                visible: function (data) {
                                    return _permissions.delete && data.record.notificationSchedule.status === 0;
                                },
                                action: function (data) {
                                    deleteNotificationSchedule(data.record.notificationSchedule);
                                }
                            },
                            {
                                text: app.localize('Approval'),
                                visible: function (data) {
                                    return _permissions.approval && data.record.notificationSchedule.status === 0;
                                },
                                action: function (data) {
                                    approvalNotification(data.record.notificationSchedule);
                                }
                            },
                            {
                                text: app.localize('Cancel'),
                                visible: function (data) {
                                    return _permissions.cancel && data.record.notificationSchedule.status === 0;
                                },
                                action: function (data) {
                                    cancelNotification(data.record.notificationSchedule);
                                }
                            },
                            {
                                text: 'Gửi thông báo ngay',
                                visible: function (data) {
                                    return _permissions.send && data.record.notificationSchedule.status === 1;
                                },
                                action: function (data) {
                                    sendNotificationNow(data.record.notificationSchedule);
                                }
                            }
                        ]
                    }
                },
                {
                    targets: 2,
                    data: "notificationSchedule.code",
                    name: "code"
                },
                {
                    targets: 3,
                    data: "notificationSchedule.name",
                    name: "name"
                },
                {
                    targets: 4,
                    data: "notificationSchedule.title",
                    name: "title"
                },
                {
                    targets: 5,
                    data: "notificationSchedule.dateSchedule",
                    name: "dateSchedule",
                    render: function (dateSchedule) {
                        if (dateSchedule) {
                            return moment(dateSchedule).format('L LTS');
                        }
                        return "";
                    }

                },
                {
                    targets: 6,
                    data: "notificationSchedule.dateSend",
                    name: "dateSend",
                    render: function (dateSend) {
                        if (dateSend) {
                            return moment(dateSend).format('L LTS');
                        }
                        return "";
                    }

                },
                {
                    targets: 7,
                    data: "notificationSchedule.status",
                    name: "status",
                    render: function (status) {
                        return app.localize('Enum_SendNotificationStatus_' + status);
                    }

                },
                // {
                //     targets: 8,
                //     data: "notificationSchedule.accountType",
                //     name: "accountType",
                //     render: function (accountType) {
                //         return app.localize('Enum_SystemAccountType_' + accountType);
                //     }
                //
                // },
                {
                    targets: 8,
                    data: "notificationSchedule.agentType",
                    name: "agentType",
                    render: function (agentType) {
                        return app.localize('Enum_AgentType_' + agentType);
                    }

                },
                {
                    targets: 9,
                    data: "notificationSchedule.dateApproved",
                    name: "dateApproved",
                    render: function (dateApproved) {
                        if (dateApproved) {
                            return moment(dateApproved).format('L LTS');
                        }
                        return "";
                    }

                },
                {
                    targets: 10,
                    data: "userName",
                    name: "userFk.name"
                }
            ]
        });

        function getNotificationSchedules() {
            dataTable.ajax.reload();
        }

        function deleteNotificationSchedule(notificationSchedule) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _notificationSchedulesService.delete({
                            id: notificationSchedule.id
                        }).done(function () {
                            getNotificationSchedules(true);
                            abp.notify.success(app.localize('SuccessfullyDeleted'));
                        });
                    }
                }
            );
        }

        function sendNotificationNow(notificationsMessage) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _notificationSchedulesService.send({
                            id: notificationsMessage.id
                        }).done(function (rs) {
                            getNotificationSchedules(true);
                            abp.message.success('Gửi thông báo thành công. Hệ thống đang bắt đầu gửi thông báo');
                        });
                    }
                }
            );
        }

        function approvalNotification(notificationsMessage) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _notificationSchedulesService.approval({id: notificationsMessage.id}).done(function (rs) {
                            getNotificationSchedules(true);
                            abp.message.success('Duyệt thông báo thành công');
                        });
                    }
                }
            );
        }

        function cancelNotification(notificationsMessage) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _notificationSchedulesService.cancel({id: notificationsMessage.id}).done(function (rs) {
                            getNotificationSchedules(true);
                            abp.message.success('Hủy thông báo thành công');
                        });
                    }
                }
            );
        }

        $('#ShowAdvancedFiltersSpan').click(function () {
            $('#ShowAdvancedFiltersSpan').hide();
            $('#HideAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideDown();
        });

        $('#HideAdvancedFiltersSpan').click(function () {
            $('#HideAdvancedFiltersSpan').hide();
            $('#ShowAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideUp();
        });

        $('#CreateNewNotificationScheduleButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportToExcelButton').click(function () {
            _notificationSchedulesService
                .getNotificationSchedulesToExcel({
                    filter: $('#NotificationSchedulesTableFilter').val(),
                    codeFilter: $('#CodeFilterId').val(),
                    nameFilter: $('#NameFilterId').val(),
                    statusFilter: $('#StatusFilterId').val(),
                    accountTypeFilter: $('#AccountTypeFilterId').val(),
                    agentTypeFilter: $('#AgentTypeFilterId').val(),
                    userNameFilter: $('#UserNameFilterId').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditNotificationScheduleModalSaved', function () {
            getNotificationSchedules();
        });

        $('#GetNotificationSchedulesButton').click(function (e) {
            e.preventDefault();
            getNotificationSchedules();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getNotificationSchedules();
            }
        });
    });
})();
