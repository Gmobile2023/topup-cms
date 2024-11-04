(function () {
    $(function () {

        var _$totalTable = $('#totalTable');

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
        const _rptService = abp.services.app.reportSystem;
        const dataTable = _$totalTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _rptService.getReportCommissionTotalList,
                inputFilter: function () {
                    return {
                        fromDate: getDateFilter($('#fromDate')),
                        toDate: getDateFilter($('#toDate')),
                        filter: $('#txtFilter').val(),
                        agentCode: $('#selectAgentSum').val()
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
                    data: "agentCode"
                },
                {
                    targets: 2,
                    data: "agentName"
                },
                {
                    targets: 3,
                    data: "quantity",
                    className: "all text-right",
                    render: function (data, type, row) {
                        return Sv.format_number(row.quantity);
                    }
                },
                {
                    targets: 4,
                    data: "commissionAmount",
                    className: "all text-right",
                    render: function (data, type, row) {
                        return Sv.format_number(row.commissionAmount);
                    }
                },
                {
                    targets: 5,
                    data: "payment",
                    className: "all text-right",
                    render: function (data, type, row) {
                        return Sv.format_number(row.payment);
                    }
                },
                {
                    targets: 6,
                    data: "unPayment",
                    className: "all text-right",
                    render: function (data, type, row) {
                        return Sv.format_number(row.unPayment);
                    }
                }
            ],
            "headerCallback": function (thead, data, start, end, display) {
                try {
                    var rawServerResponse = this.api().settings()[0].rawServerResponse;
                    if (rawServerResponse.warning.length > 0)
                        abp.message.info(rawServerResponse.warning);

                    if (rawServerResponse !== undefined && rawServerResponse.totalData !== undefined) {
                        $(thead).find('th').eq(1).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.quantity));
                        $(thead).find('th').eq(2).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.commissionAmount));
                        $(thead).find('th').eq(3).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.payment));
                        $(thead).find('th').eq(4).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.unPayment));
                    }
                } catch (e) {
                    console.log("không có total")
                }
            }
        });

        function getDataRose() {
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

        $('#ExportToExcelButton').click(function () {
            _rptService.getReportCommissionTotalListToExcel({
                fromDate: getDateFilter($('#fromDate')),
                toDate: getDateFilter($('#toDate')),
                filter: $('#txtFilter').val(),
                agentCode: $('#selectAgentSum').val()
            })
                .done(function (result) {
                    if (result.fileName === "Warning")
                        abp.message.info(result.filePath);
                    else
                        app.downloadTempFile(result);
                });
        });

        $('#GetSearchAllButton').click(function (e) {
            e.preventDefault();
            getDataRose();
        });

        $('#GetSearchButton').click(function (e) {
            e.preventDefault();
            getDataRose();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getDataRose();
            }
        });

        $("#selectAgentSum").select2({
            placeholder: 'Tìm kiếm',
            allowClear: true,
            ajax: {
                url: abp.appPath + "api/services/app/CommonLookup/GetListUserSearch",
                dataType: 'json',
                delay: 250,
                data: function (params) {
                    return {
                        search: params.term,
                        page: params.page,
                        agentType: 4,
                        accountType: 99
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
    });
})();
