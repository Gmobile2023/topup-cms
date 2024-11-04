(function () {
    $(function () {
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var getDateFilter = function (element) {
            return element.data("DateTimePicker").date() === null ? null : element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z");
        };

        var _$table = $('#Table');
        var _reportService = abp.services.app.reportSystem;
        var dataTable = _$table.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _reportService.getReportTotalDayList,
                inputFilter: function () {
                    return {
                        fromDate: getDateFilter($('#FromDateFilterId')),
                        toDate: getDateFilter($('#ToDateFilterId')),
                    };
                }
            },
            columnDefs: [
                {
                    targets: 0,
                    className: 'control responsive text-center',
                    orderable: false,
                    render: function () {
                        return '';
                    }
                },
                {
                    targets: 1,
                    data: "createdDay",
                    render: function (createdDay) {
                        if (createdDay) {
                            return moment(new Date(createdDay)).format('DD/MM/YYYY');
                        }
                        return "";
                    }
                },
                {
                    targets: 2,
                    className: "all text-right",
                    data: "balanceBefore",
                    name: "balanceBefore",
                    render: function (data) {
                        return data ? Sv.format_number(data) : "0"
                    }
                },
                {
                    targets: 3,
                    className: "all text-right",
                    data: "incDeposit",
                    name: "incDeposit",
                    render: function (data) {
                        return data ? Sv.format_number(data) : "0"
                    }
                },
                {
                    targets: 4,
                    className: "all text-right",
                    data: "incOther",
                    name: "incOther",
                    render: function (data) {
                        return data ? Sv.format_number(data) : "0"
                    }
                },
                {
                    targets: 5,
                    className: "all text-right",
                    data: "decPayment",
                    render: function (data) {
                        return data ? Sv.format_number(data) : "0"
                    }

                },
                {
                    targets: 6,
                    className: "all text-right",
                    data: "decOther",
                    render: function (data) {
                        return data ? Sv.format_number(data) : "0"
                    }
                },
                {
                    targets: 7,
                    className: "all text-right",
                    data: "balanceAfter",
                    render: function (data) {
                        return data ? Sv.format_number(data) : "0"
                    }
                }
            ],
            "headerCallback": function (thead, data, start, end, display) {
                try {
                    var rawServerResponse = this.api().settings()[0].rawServerResponse;
                    if (rawServerResponse.warning.length > 0)
                        abp.message.info(rawServerResponse.warning);
                } catch (e) {
                    console.log("không có total")
                }
            }
        });


        function getTables() {
            dataTable.ajax.reload();
        }

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

        $('#ExportToExcelButton').click(function () {
            _reportService.getReportTotalDayListToExcel({
                fromDate: getDateFilter($('#FromDateFilterId')),
                toDate: getDateFilter($('#ToDateFilterId')),
            })
                .done(function (result) {
                    if (result.fileName === "Warning")
                        abp.message.info(result.filePath);
                    else
                        app.downloadTempFile(result);
                });
        });

        $(document).on('click', '.print-invoice-item', function () {
            let _type = $(this).attr('data-type');
            let _transcode = $(this).attr('data-trans-code');
            ngtAction.open(_transcode, 'print');
        });
    });
})();
