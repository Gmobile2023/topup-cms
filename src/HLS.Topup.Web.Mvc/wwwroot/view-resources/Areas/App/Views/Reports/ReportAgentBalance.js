(function () {
    $(function () {

        var _$toalTable = $('#totalTable');

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
        const dataTable = _$toalTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _rptService.getReportAgentBalanceList,
                inputFilter: function () {
                    return {
                        fromDate: getDateFilter($('#fromDate')),
                        toDate: getDateFilter($('#toDate')),
                        agentType: $("#typeAgent").val(),
                        agentCode: $("#selectAgent").val(),
                        userSaleLeaderCode: $("#selectUserSaleLeader").val(),
                        userSaleCode: $("#selectUserSale").val()
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
                    data: "agentTypeName"
                },
                {
                    targets: 2,
                    data: "agentInfo"
                },
                {
                    targets: 3,
                    data: "saleInfo"
                },
                {
                    targets: 4,
                    data: "saleLeaderInfo"
                },
                {
                    targets: 5,
                    data: "beforeAmount",
                    className: "all text-right",
                    render: function (value) {
                        if (value) {
                            return Sv.format_number(value);
                        }
                        return "0";
                    }
                },
                {
                    targets: 6,
                    data: "inputAmount",
                    className: "all text-right",
                    render: function (data, type, row) {
                        if (row.inputAmount) {
                            return "<a href='" + row.link + "'>" + Sv.format_number(row.inputAmount) + '</a>';
                        }
                        return "0";
                    }
                },
                {
                    targets: 7,
                    data: "amountUp",
                    className: "all text-right",
                    render: function (value) {
                        if (value) {
                            return Sv.format_number(value)
                        }
                        return "0";
                    }
                },
                {
                    targets: 8,
                    data: "saleAmount",
                    className: "all text-right",
                    render: function (data, type, row) {
                        if (row.saleAmount) {
                            return "<a href='" + row.link + "'>" + Sv.format_number(row.saleAmount) + '</a>';
                        }
                        return "0";
                    }
                },
                {
                    targets: 9,
                    data: "amountDown",
                    className: "all text-right",
                    render: function (value) {
                        if (value) {
                            return Sv.format_number(value)
                        }
                        return "0";
                    }
                },
                {
                    targets: 10,
                    data: "afterAmount",
                    className: "all text-right",
                    render: function (value) {
                        if (value) {
                            return Sv.format_number(value)
                        }
                        return "0";
                    }
                }
            ],
            "headerCallback": function (thead, data, start, end, display) {
                try {
                    var rawServerResponse = this.api().settings()[0].rawServerResponse;
                    if (rawServerResponse.warning.length > 0)
                        abp.message.info(rawServerResponse.warning);

                    if (rawServerResponse !== undefined && rawServerResponse.totalData !== undefined) {
                        $(thead).find('th').eq(1).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.beforeAmount));
                        $(thead).find('th').eq(2).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.inputAmount));
                        $(thead).find('th').eq(3).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.amountUp));
                        $(thead).find('th').eq(4).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.saleAmount));
                        $(thead).find('th').eq(5).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.amountDown));
                        $(thead).find('th').eq(6).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.afterAmount));
                    }
                } catch (e) {
                    console.log("không có total")
                }
            }
        });

        function getProviders() {
            dataTable.ajax.reload();
        }

        $('#ExportToExcelButton').click(function () {
            _rptService.getReportAgentBalanceListToExcel({
                fromDate: getDateFilter($('#fromDate')),
                toDate: getDateFilter($('#toDate')),
                agentType: $("#agentType").val(),
                agentCode: $("#selectAgent").val(),
                userSaleLeaderCode: $("#selectUserSaleLeader").val(),
                userSaleCode: $("#selectUserSale").val()
            })
                .done(function (result) {
                    if (result.fileName === "Warning")
                        abp.message.info(result.filePath);
                    else
                        app.downloadTempFile(result);
                });
        });


        $('#GetProvidersButton').click(function (e) {
            e.preventDefault();
            getProviders();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getProviders();
            }
        });


        $("#agentType").select2();

        $("#selectAgent").select2({
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
                        agentType: $('#typeAgent option:selected').val(),
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

        $("#selectUserSaleLeader").select2({
            placeholder: 'Tìm kiếm',
            allowClear: true,
            ajax: {
                url: abp.appPath + "api/services/app/CommonLookup/GetUserSaleLeader",
                dataType: 'json',
                delay: 250,
                data: function (params) {
                    return {
                        search: params.term,
                        page: params.page,
                        agentType: $('#agentType option:selected').val(),
                        accountType: 99
                    };
                },
                processResults: function (data, params) {
                    params.page = params.page || 1;

                    return {
                        results: $.map(data.result, function (item) {
                            return {
                                text: item.userName + "-" + item.phoneNumber + "-" + item.fullName,
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

        $("#selectUserSale").select2({
            placeholder: 'Tìm kiếm',
            allowClear: true,
            ajax: {
                url: abp.appPath + "api/services/app/CommonLookup/GetUserSaleBySaleLeader",
                dataType: 'json',
                delay: 250,
                data: function (params) {
                    return {
                        search: params.term,
                        page: params.page,
                        saleLeaderCode: $('#selectUserSaleLeader option:selected').val(),
                        accountType: 99
                    };
                },
                processResults: function (data, params) {
                    params.page = params.page || 1;

                    return {
                        results: $.map(data.result, function (item) {
                            return {
                                text: item.userName + "-" + item.phoneNumber + "-" + item.fullName,
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
