﻿(function () {
    $(function () {


        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z");
        }

        var _$table = $('#Table');
        var _transactionService = abp.services.app.transactions;
        var _rptService = abp.services.app.reportSystem;

        var dataTable = _$table.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _rptService.getReportDetailList,
                inputFilter: function () {
                    return {
                        filter: $('#TableFilter').val(),
                        transType: $("#ServiceCodeFilter").val(),
                        transCodeFilter: $("#TransCodeFilterId").val(),
                        serviceCodeFilter: $('#ServiceCodeFilter').val(),
                        fromDateFilter: getDateFilter($('#FromDateFilterId')),
                        toDateFilter: getDateFilter($('#ToDateFilterId'))
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
                    data: "transCode"
                },
                {
                    targets: 2,
                    data: "createdDate",
                    render: function (createdTime) {
                        if (createdTime) {
                            return moment(new Date(createdTime)).format('DD/MM/YYYY HH:mm:ss');
                        }
                        return "";
                    }
                },
                {
                    targets: 3,
                    data: "serviceName"
                },
                {
                    targets: 4,
                    data: "increment",
                    //className: "text-right",
                    render: function (increment) {
                        if (increment) {
                            return Sv.format_number(increment)
                        }
                        return "0";
                    }
                },
                {
                    targets: 5,
                    data: "decrement",
                    //className: "text-right",
                    render: function (decrement) {
                        if (decrement) {
                            return Sv.format_number(decrement)
                        }
                        return "0";
                    }
                },
                {
                    targets: 6,
                    data: "balanceAfter",
                    //className: "text-right",
                    render: function (balanceAfterTrans) {
                        if (balanceAfterTrans) {
                            return Sv.format_number(balanceAfterTrans)
                        }
                        return "0";
                    }
                },
                {
                    targets: 7,
                    data: "transNote"
                }
            ]
        });


        function getUsers() {
            dataTable.ajax.reload();
        }

        $('#btnSearch').click(function (e) {
            e.preventDefault();
            getUsers();
        });

        $('#TableFilter').on('keydown', function (e) {
            if (e.keyCode !== 13) {
                return;
            }

            e.preventDefault();
            getUsers();
        });

        abp.event.on('app.createOrEditAgentModalSaved', function () {
            getUsers();
        });

        $('#TableFilter').focus();
    });
})();
