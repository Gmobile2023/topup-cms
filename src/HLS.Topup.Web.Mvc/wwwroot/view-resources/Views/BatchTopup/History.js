(function () {
    $(function () {
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var getDateFilter = function (element) {
            return element.data("DateTimePicker").date() === null ? null : element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z");
        };

        var _permissions = {
            stop: abp.auth.hasPermission('Pages.BatchLotStop'),
        };

        var _$table = $('#tableHistory');
        var _transService = abp.services.app.transactions;
        var dataTable = _$table.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _transService.getBatchLotList,
                inputFilter: function () {
                    return {
                        fromDate: getDateFilter($('#FromDateFilterId')),
                        toDate: getDateFilter($('#ToDateFilterId')),
                        status: $("#StatusFilterId").val(),
                        batchCode: $("#txtRequestTrans").val(),
                        batchType: $("#selectBachType").val()
                    };
                }
            },
            columnDefs: [
                {
                    targets: 0,
                    data: "status",
                    className: 'all',
                    rowAction: {
                        cssClass: 'btn btn-brand dropdown-toggle',
                        text: '<i class="fa fa-cog"></i>Hành động<span class="caret"></span>',
                        items: [
                            {
                                text: app.localize('Dừng'),
                                visible: function (data) {
                                    return _permissions.stop && (data.record.status == 0 || data.record.status == 2);
                                },
                                action: function (data) {
                                    var stopDto = {};
                                    stopDto.BatchCode = data.record.batchCode;
                                    approvalStop(stopDto);
                                }
                            },
                            {
                                text: app.localize('Xem chi tiết'),
                                visible: function (data) {
                                    return true;
                                },
                                action: function (data) {
                                    OpendLink(data.record.link);
                                }
                            }
                        ]
                    }
                },
                {
                    targets: 1,
                    data: "statusName",
                    className: 'all',
                },
                {
                    targets: 2,
                    data: "batchCode",
                    render: function (data, type, row) {
                        return "<a href='" + row.link + "'>" + row.batchCode + '</a>';
                    }
                },
                {
                    targets: 3,
                    data: "batchName",
                    className: 'all',
                },
                {
                    targets: 4,
                    data: "staffAccount",
                    className: 'all',
                },
                {
                    targets: 5,
                    data: "createdTime",
                    className: 'all',
                    render: function (createdTime, dispaly, row) {
                        return moment(new Date(createdTime)).format('DD/MM/YYYY HH:mm:ss');
                    }
                },
                {
                    targets: 6,
                    data: "endProcessTime",
                    className: 'all',
                    render: function (endProcessTime, dispaly, row) {
                        return moment(new Date(endProcessTime)).format('DD/MM/YYYY HH:mm:ss');
                    }
                },
            ]

        });

        function approvalStop(dto) {
            abp.message.confirm(
                'Bạn có chắc chắn dừng nạp lô này không?',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _transService.batchLotStopRequest(
                            dto
                        ).done(function () {
                            abp.message.info('Yêu cầu hủy lô đã được yêu cầu thực hiện.');
                            getTables();
                        }).always(function () {
                        });
                    }
                }
            );
        }

        function OpendLink(link) {
            window.location.href = link;
        }

        function getTables() {
            dataTable.ajax.reload();
        }


        $('#ExportToExcelButton').click(function () {
            _transService.getBatchLotExportToFile({
                fromDate: getDateFilter($('#FromDateFilterId')),
                toDate: getDateFilter($('#ToDateFilterId')),
                status: $("#StatusFilterId").val(),
                batchCode: $("#txtRequestTrans").val(),
                batchType: $("#selectBachType").val()
            })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        

        $('#btnSearch').click(function (e) {
            e.preventDefault();
            getTables();
        });

        $('#TableFilter').on('keydown', function (e) {
            if (e.keyCode !== 13) {
                return;
            }

            e.preventDefault();
            getTables();
        });

        abp.event.on('app.createOrEditAgentModalSaved', function () {
            getTables();
        });

        $("#TableFilter").focus();

    });
})();
