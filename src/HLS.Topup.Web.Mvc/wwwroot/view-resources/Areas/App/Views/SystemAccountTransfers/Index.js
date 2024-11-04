(function () {
    $(function () {

        var _$systemAccountTransfersTable = $('#SystemAccountTransfersTable');
        var _systemAccountTransfersService = abp.services.app.systemAccountTransfers;

        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.SystemAccountTransfers.Create'),
            edit: abp.auth.hasPermission('Pages.SystemAccountTransfers.Edit'),
            'delete': abp.auth.hasPermission('Pages.SystemAccountTransfers.Delete'),
            approval: abp.auth.hasPermission('Pages.SystemAccountTransfers.Approval'),
            cancel: abp.auth.hasPermission('Pages.SystemAccountTransfers.Cancel'),
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/SystemAccountTransfers/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/SystemAccountTransfers/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditSystemAccountTransferModal',
            modalSize: 'modal-xl'
        });

        var _viewSystemAccountTransferModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/SystemAccountTransfers/ViewsystemAccountTransferModal',
            modalClass: 'ViewSystemAccountTransferModal'
        });


        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z");
        }
        var getToDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT23:59:59Z");
        }

        var dataTable = _$systemAccountTransfersTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _systemAccountTransfersService.getAll,
                inputFilter: function () {
                    return {
                        filter: $('#SystemAccountTransfersTableFilter').val(),
                        srcAccountFilter: $('#SrcAccountFilterId').val(),
                        desAccountFilter: $('#DesAccountFilterId').val(),
                        statusFilter: $('#StatusFilterId').val(),
                        fromCreatedTimeFilter: getDateFilter($('#FromCreatedTimeFilter')),
                        toCreatedTimeFilter: getToDateFilter($('#ToCreatedTimeFilter')),
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
                                    _viewSystemAccountTransferModal.open({id: data.record.systemAccountTransfer.id});
                                }
                            },
                            {
                                text: app.localize('Edit'),
                                visible: function (data) {
                                    return _permissions.edit && data.record.systemAccountTransfer.status === 0;
                                },
                                action: function (data) {
                                    _createOrEditModal.open({id: data.record.systemAccountTransfer.id});
                                }
                            },
                            {
                                text: app.localize('Approval'),
                                visible: function (data) {
                                    return _permissions.approval && data.record.systemAccountTransfer.status === 0;
                                },
                                action: function (data) {
                                    approval(data.record.systemAccountTransfer);
                                }
                            },
                            {
                                text: app.localize('Cancel'),
                                visible: function (data) {
                                    return _permissions.cancel && data.record.systemAccountTransfer.status === 0;
                                },
                                action: function (data) {
                                    cancel(data.record.systemAccountTransfer);
                                }
                            },
                            {
                                text: app.localize('Delete'),
                                visible: function (data) {
                                    return _permissions.delete && data.record.systemAccountTransfer.status === 0;
                                },
                                action: function (data) {
                                    deleteSystemAccountTransfer(data.record.systemAccountTransfer);
                                }
                            }]
                    }
                },
                {
                    targets: 2,
                    data: "systemAccountTransfer.transCode",
                    name: "transCode"
                },
                {
                    targets: 3,
                    data: "systemAccountTransfer.srcAccount",
                    name: "srcAccount"
                },
                {
                    targets: 4,
                    data: "systemAccountTransfer.desAccount",
                    name: "desAccount"
                },
                {
                    targets: 5,
                    data: "systemAccountTransfer.status",
                    name: "status",
                    render: function (status) {
                        return app.localize('Enum_SystemTransferStatus_' + status);
                    }

                },
                {
                    targets: 6,
                    data: "systemAccountTransfer.amount",
                    name: "amount",
                    render: function (amount) {
                        return Sv.NumberToString(amount);
                    }
                },
                {
                    targets: 7,
                    data: "userCreated",
                    name: "UserCreated"
                },
                {
                    targets: 8,
                    data: "dateCreated",
                    name: "DateCreated",
                    render: function (dateCreated) {
                        if (dateCreated) {
                            return moment(new Date(dateCreated)).format('DD/MM/YYYY HH:mm:ss');
                        }
                        return "";
                    }
                },
                {
                    targets: 9,
                    data: "userApproved",
                    name: "UserApproved"
                },
                {
                    targets: 10,
                    data: "dateApproved",
                    name: "DateApproved",
                    render: function (dateApproved) {
                        if (dateApproved) {
                            return moment(new Date(dateApproved)).format('DD/MM/YYYY HH:mm:ss');
                        }
                        return "";
                    }
                }
            ]
        });

        function getSystemAccountTransfers() {
            dataTable.ajax.reload();
        }

        function deleteSystemAccountTransfer(systemAccountTransfer) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _systemAccountTransfersService.delete({
                            id: systemAccountTransfer.id
                        }).done(function () {
                            getSystemAccountTransfers(true);
                            abp.notify.success(app.localize('SuccessfullyDeleted'));
                        });
                    }
                }
            );
        }

        function approval(systemAccountTransfer) {
            abp.message.confirm(
                'Bạn có chắc chắn muốn duyệt giao dịch này không?',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _systemAccountTransfersService.approval({id: systemAccountTransfer.id}).done(function () {
                            getSystemAccountTransfers(true);
                            abp.message.success('Duyệt thành công');
                        });
                    }
                }
            );
        }

        function cancel(systemAccountTransfer) {
            abp.message.confirm(
                'Bạn chắc chắn muốn hủy giao dịch này không?',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _systemAccountTransfersService.cancel({id: systemAccountTransfer.id}).done(function () {
                            getSystemAccountTransfers(true);
                            abp.message.success('Hủy thành công');
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

        $('#CreateNewSystemAccountTransferButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportToExcelButton').click(function () {
            _systemAccountTransfersService
                .getSystemAccountTransfersToExcel({
                    filter: $('#SystemAccountTransfersTableFilter').val(),
                    srcAccountFilter: $('#SrcAccountFilterId').val(),
                    desAccountFilter: $('#DesAccountFilterId').val(),
                    statusFilter: $('#StatusFilterId').val(),
                    fromCreatedTimeFilter: getDateFilter($('#FromCreatedTimeFilter')),
                    toCreatedTimeFilter: getToDateFilter($('#ToCreatedTimeFilter')),
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditSystemAccountTransferModalSaved', function () {
            getSystemAccountTransfers();
        });

        $('#GetSystemAccountTransfersButton').click(function (e) {
            e.preventDefault();
            getSystemAccountTransfers();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getSystemAccountTransfers();
            }
        });
    });
})();
