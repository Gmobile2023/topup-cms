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
                ajaxFunction: _rptService.getReportTotalDebtList,
                inputFilter: function () {
                    return {
                        accountCode: $("#selectUserSale").val(),
                        fromDate: getDateFilter($('#fromDate')),
                        toDate: getDateFilter($('#toDate'))
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
                    data: "saleInfo"
                },
                {
                    targets: 2,
                    data: "balanceBefore",
                    className: "all text-right",
                    render: function (balanceBefore) {
                        if (balanceBefore) {
                            return Sv.format_number(balanceBefore) + " đ"
                        }
                        return "0 đ";
                    }
                },

                {
                    targets: 3,
                    data: "decPayment",
                    className: "all text-right",
                    render: function (data, type, row) {
                        if (row.decPayment) {
                            //return Sv.format_number(decPayment) + " đ";
                            return "<a href='" + row.linkDebt + "'>" + Sv.format_number(row.decPayment) + '</a>';
                        }
                        return "0 đ";
                    }
                },
                {
                    targets: 4,
                    data: "incDeposit",
                    className: "all text-right",
                    render: function (data, type, row) {
                        if (row.incDeposit) {
                            // return Sv.format_number(incDeposit) + " đ";                           
                            return "<a href='" + row.linkPayDebt + "'>" + Sv.format_number(row.incDeposit) + '</a>';
                        }
                        return "0 đ";
                    }
                },
                {
                    targets: 5,
                    data: "balanceAfter",
                    className: "all text-right",
                    render: function (balanceAfter) {
                        if (balanceAfter) {
                            return Sv.format_number(balanceAfter) + " đ";
                        }
                        return "0 đ";
                    }
                }
            ],
            "headerCallback": function (thead, data, start, end, display) {
                try {
                    var rawServerResponse = this.api().settings()[0].rawServerResponse;
                    if (rawServerResponse.warning.length > 0)
                        abp.message.info(rawServerResponse.warning);

                    if (rawServerResponse !== undefined && rawServerResponse.totalData !== undefined) {
                        $(thead).find('th').eq(1).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.balanceBefore));
                        $(thead).find('th').eq(2).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.decPayment));
                        $(thead).find('th').eq(3).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.incDeposit));
                        $(thead).find('th').eq(4).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.balanceAfter));
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
            _rptService.getReportTotalDebtListToExcel({
                accountCode: $("#selectUserSale").val(),
                fromDate: getDateFilter($('#fromDate')),
                toDate: getDateFilter($('#toDate'))
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

        $("#selectUserSale").select2({
            placeholder: 'Tìm kiếm',
            allowClear: true,
            ajax: {
                url: abp.appPath + "api/services/app/CommonLookup/GetListUserSaleSearch",
                dataType: 'json',
                delay: 250,
                data: function (params) {
                    return {
                        search: params.term,
                        page: params.page,
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
