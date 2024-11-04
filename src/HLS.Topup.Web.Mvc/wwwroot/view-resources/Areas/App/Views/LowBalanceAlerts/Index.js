(function () {
    $(function () {
        var _lowBalanceAlertsTable = $('#LowBalanceAlertsTable');
        var _lowBalanceAlertsService = abp.services.app.lowBalanceAlerts;

        var _permissions = {
            create: abp.auth.hasPermission('Pages.LowBalanceAlerts.Create'),
            edit: abp.auth.hasPermission('Pages.LowBalanceAlerts.Edit'),
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/LowBalanceAlerts/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/LowBalanceAlerts/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditLowBalanceAlertModal',
            modalSize: 'modal-md'
        });

        var _viewModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/LowBalanceAlerts/ViewBalanceAlertModal',
            modalClass: 'ViewLowBalanceAlertModal',
            modalSize: 'modal-md'
        });

        var dataTable = _lowBalanceAlertsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _lowBalanceAlertsService.getAll,
                inputFilter: function () {
                    return {
                        accountCode: $('#AccountCodeFilterId').val(),
                        isRun: $('#StatusFilterId').val() == 1 ? true : false
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
                                    _viewModal.open({ accountCode: data.record.accountCode });
                                }
                            },
                            {
                                text: app.localize('Edit'),
                                visible: function () {
                                    return _permissions.edit;
                                },
                                action: function (data) {
                                    _createOrEditModal.open({ accountCode: data.record.accountCode });
                                }
                            }]
                    }
                },
                {
                    targets: 2,
                    data: "accountCode",
                    name: "accountCode"
                },
                {
                    targets: 3,
                    data: "accountName",
                    name: "accountName"
                },
                {
                    targets: 4,
                    data: "minBalance",
                    name: "minBalance",
                    render: function (data) {
                        return Sv.format_number(data)
                    }
                },
                {
                    targets: 5,
                    data: "teleChatId",
                    name: "teleChatId"
                },
                {
                    targets: 6,
                    data: "channel",
                    name: "channel",
                    // render: function (data) {
                    //     return 'Telegram';
                    // }
                },
                {
                    targets: 7,
                    data: "isRun",
                    name: "isRun",
                    render: function (isRun) {
                        let _status = isRun ? 1 : 0;
                        return app.localize('Enum_LowBalanceAlert_' + _status);
                    }
                },
                {
                    targets: 8,
                    data: "createdDate",
                    name: "createdDate",
                    render: function (creationTime) {
                        if (creationTime) {
                            return moment(creationTime).format('L LTS');
                        }
                        return "";
                    }
                },
            ]
        });

        function getLowBalanceAlerts() {
            dataTable.ajax.reload();
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

        $('#CreateNewLowBalanceAlertButton').click(function () {
            _createOrEditModal.open();
        });

        abp.event.on('app.createOrEditLowBalanceAlertModalSaved', function () {
            getLowBalanceAlerts();
        });

        $('#GetLowBalanceAlertsButton').click(function (e) {
            e.preventDefault();
            getLowBalanceAlerts();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getLowBalanceAlerts();
            }
        });
    });
})();