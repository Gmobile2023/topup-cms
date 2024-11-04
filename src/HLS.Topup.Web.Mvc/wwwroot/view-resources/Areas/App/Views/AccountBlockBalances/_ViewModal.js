(function ($) {
    app.modals.ViewAccountBlockBalanceModal = function () {
        var _accountBlockBalancesService = abp.services.app.accountBlockBalances;
        var _modalManager;
        var _$accountBlockBalancesTable = $('#AccountBlockBalancesDetailTable');
        this.init = function (modalManager) {
            _modalManager = modalManager;
            var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });
            Sv.SetupAmountMask();
        };

        var dataTable = _$accountBlockBalancesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _accountBlockBalancesService.getListDetail,
                inputFilter: function () {
                    return {
                        id: $('#txtId').val(),
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
                    targets: 1,
                    data: "type",
                    name: "type",
                    render: function (type) {
                        return app.localize('Enum_BlockBalanceType_' + type);
                    }
                },
                {
                    targets: 2,
                    data: "amount",
                    name: "amount",
                    render: function (amount) {
                        return Sv.NumberToString(amount);
                    }
                },
                {
                    targets: 3,
                    data: "creationTime",
                    name: "creationTime",
                    render: function (creationTime) {
                        if (creationTime) {
                            return moment(new Date(creationTime)).format('DD/MM/YYYY HH:mm:ss');
                        }
                        return "";
                    }
                },
                {
                    targets: 4,
                    data: "userName",
                    name: "userName",
                    orderable: false
                },
                {
                    targets: 5,
                    data: "attachments",
                    name: "attachments",
                    orderable: false,
                    render: function (attachments) {
                        if (attachments) {
                            return '<a href="'+attachments+'" target="_blank"><i style="font-size: 14px !important;" class="fa fa-download" aria-hidden="true"></i> Xem văn bản</a>';
                        }
                        return "";
                    }
                },
                {
                    targets: 6,
                    data: "description",
                    name: "description"
                }
            ]
        });

    };
})(jQuery);
