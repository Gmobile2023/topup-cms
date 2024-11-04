(function () {
    $(function () {

        var _$detailTable = $('#detailTable');

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
        const dataTable = _$detailTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _rptService.getReportDebtDetailList,
                inputFilter: function () {
                    return {
                        transCode: $("#transCode").val(),
                        serviceCode: $('#typeSelect').val(),
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
                    data: "createdTime",
                    render: function (createdTime) {
                        if (createdTime) {
                            return moment(new Date(createdTime)).format('DD/MM/YYYY HH:mm:ss');
                        }
                        return "";
                    }
                },
                {
                    targets: 2,
                    data: "transCode"
                },
                {
                    targets: 3,
                    data: "serviceName"
                },
                {
                    targets: 4,
                    data: "description"
                },
                {
                    targets: 5,
                    data: "debitAmount",
                    className: "all text-right",
                    render: function (debitAmount) {
                        if (debitAmount) {
                            return Sv.format_number(debitAmount) + " đ"
                        }
                        return "0 đ";
                    }
                },
                {
                    targets: 6,
                    data: "creditAmount",
                    className: "all text-right",
                    render: function (creditAmount) {
                        if (creditAmount) {
                            return Sv.format_number(creditAmount) + " đ"
                        }
                        return "0 đ";
                    }
                },
                {
                    targets: 7,
                    data: "balance",
                    className: "all text-right",
                    render: function (balance) {
                        if (balance) {
                            return Sv.format_number(balance) + " đ"
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
                        $(thead).find('th').eq(1).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.debitAmount));
                        $(thead).find('th').eq(2).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.creditAmount));
                        $(thead).find('th').eq(3).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.balance));
                    }
                } catch (e) {
                    console.log("không có total")
                }
            }
        });

        function getProviders() {
            var accountCode = $("#selectUserSale").val();
            if (accountCode === null || accountCode === "") {
                abp.message.error("Quý khách chưa chọn nhân viên kinh doanh");
                return;
            }
            else {
                dataTable.ajax.reload();
            }
        }

        $('#ExportToExcelButton').click(function () {
            _rptService.getReportDebtDetailListToExcel({
                transCode: $("#transCode").val(),
                serviceCode: $('#typeSelect').val(),
                accountCode: $("#selectUserSale").val(),
                fromDate: getDateFilter($('#fromDate')),
                toDate: getDateFilter($('#toDate'))
            })
                .done(function (result) {
                    if (result.fileName === "Warning")
                        abp.message.info(result.filePath);
                    else app.downloadTempFile(result);
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
