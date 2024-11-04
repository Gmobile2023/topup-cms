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
                ajaxFunction: _reportService.getReportCommissionAgentTotalList,
                inputFilter: function () {
                    return {
                        fromDate: getDateFilter($('#txtFromDate')),
                        toDate: getDateFilter($('#txtToDate')),
                        agentCode: $("#selectAgent").val()
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
                    data: "agentCode"
                },
                {
                    targets: 2,
                    data: "agentName"
                },
                {
                    targets: 3,
                    className: "all text-right",
                    data: "before",
                    name: "before",
                    render: function (data) {
                        return data ? Sv.format_number(data) : "0"
                    }
                },
                {
                    targets: 4,
                    className: "all text-right",
                    data: "amountUp",
                    name: "amountUp",
                    render: function (data) {
                        return data ? Sv.format_number(data) : "0"
                    }
                },
                {
                    targets: 5,
                    className: "all text-right",
                    data: "amountDown",
                    name: "amountDown",
                    render: function (data) {
                        return data ? Sv.format_number(data) : "0"
                    }
                },
                {
                    targets: 6,
                    className: "all text-right",
                    data: "after",
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


        $("#selectAgent").select2({
            placeholder: 'Tìm kiếm',
            allowClear: true,
            ajax: {
                url: abp.appPath + "api/services/app/CommonLookup/GetUserAgentLevel",
                dataType: 'json',
                delay: 250,
                data: function (params) {
                    return {
                        search: params.term,
                        page: params.page,
                        saleLeaderCode: $('#hdnAccountCode').val()
                    };
                },
                processResults: function (data, params) {
                    params.page = params.page || 1;

                    return {
                        results: $.map(data.result, function (item) {
                            return {
                                text: item.accountCode + "-" + item.phoneNumber + "-" + item.fullName,
                                id: item.accountCode
                            }
                        }),
                        pagination: {
                            more: (params.page * 30) < data.result.length
                        }
                    };
                },
                cache: true
            },
            minimumInputLength: 3,
            language: abp.localization.currentCulture.name
        });

        abp.event.on('app.createOrEditAgentModalSaved', function () {
            getTables();
        });

        $("#TableFilter").focus();

        $('#ExportToExcelButton').click(function () {
            _reportService.getReportCommissionAgentTotalListToExcel({
                fromDate: getDateFilter($('#txtFromDate')),
                toDate: getDateFilter($('#txtToDate')),
                agentCode: $("#selectAgent").val()
            })
                .done(function (result) {
                    if (result.fileName === "Warning")
                        abp.message.info(result.filePath);
                    else
                        app.downloadTempFile(result);
                });
        });
    });
})();
