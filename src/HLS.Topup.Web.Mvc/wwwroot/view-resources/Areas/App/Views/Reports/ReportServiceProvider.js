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
                ajaxFunction: _rptService.getReportServiceProviderList,
                inputFilter: function () {
                    return {
                        fromDate: getDateFilter($('#fromDate')),
                        toDate: getDateFilter($('#toDate')),
                        serviceCode: $('#selectService').val(),
                        categoryCode: $('#selectCategory').val(),
                        productCode: $('#selectProduct').val(),
                        agentType: $("#agentType").val(),
                        agentCode: $("#selectAgent").val(),
                        agentCodeParent: $("#selectAgentParent").val(),
                        providerCode: $('#selectProvider').val(),
                        receiverType: $('#selectReceiverType').val(),
                        providerReceiverType: $('#selectReceiverTypeResponse').val()
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
                    data: "serviceName"
                },
                {
                    targets: 2,
                    data: "categoryName"
                },
                {
                    targets: 3,
                    data: "productName",
                    render: function (data, type, row) {
                        return "<a href='" + row.link + "'>" + row.productName + '</a>';

                    }
                },
                {
                    targets: 4,
                    data: "quantity",
                    className: "all text-right",
                    render: function (data, type, row) {
                        if (row.quantity) {
                            return "<a href='" + row.link + "'>" + Sv.format_number(row.quantity) + '</a>';
                        }
                        return "0";
                    }
                },
                {
                    targets: 5,
                    data: "value",
                    className: "all text-right",
                    render: function (data, type, row) {
                        if (row.value) {
                            return "<a href='" + row.link + "'>" + Sv.format_number(row.value) + '</a>';
                        }
                        return "0";
                    }
                },
                {
                    targets: 6,
                    data: "discount",
                    className: "all text-right",
                    render: function (data, type, row) {
                        if (row.discount) {
                            return "<a href='" + row.link + "'>" + Sv.format_number(row.discount) + '</a>';
                        }
                        return "0";
                    }
                },
                {
                    targets: 7,
                    data: "fee",
                    className: "all text-right",
                    render: function (data, type, row) {
                        if (row.fee) {
                            return "<a href='" + row.link + "'>" + Sv.format_number(row.fee) + '</a>';
                        }
                        return "0";
                    }
                },
                {
                    targets: 8,
                    data: "price",
                    className: "all text-right",
                    render: function (data, type, row) {
                        if (row.price) {
                            return "<a href='" + row.link + "'>" + Sv.format_number(row.price) + '</a>';
                        }
                        return "0";
                    }
                },
                {
                    targets: 9,
                    data: "providerName"
                }
            ],
            "headerCallback": function (thead, data, start, end, display) {
                try {
                    var rawServerResponse = this.api().settings()[0].rawServerResponse;
                    if (rawServerResponse.warning.length > 0)
                        abp.message.info(rawServerResponse.warning);

                    if (rawServerResponse !== undefined && rawServerResponse.totalData !== undefined) {
                        $(thead).find('th').eq(1).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.quantity));
                        $(thead).find('th').eq(2).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.value));
                        $(thead).find('th').eq(3).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.discount));
                        $(thead).find('th').eq(4).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.fee));
                        $(thead).find('th').eq(5).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.price));
                    }
                } catch (e) {
                    console.log("không có total")
                }
            }
        });

        function getProviders() {
            dataTable.ajax.reload();

            //abp.ui.setBusy();
            //_rptService.checkValidateSearch({
            //    fromDate: getDateFilter($('#fromDate')),
            //    toDate: getDateFilter($('#toDate')),
            //    reportType: "Total",
            //    type: "Search",
            //})
            //    .done(function (result) {
            //        if (result.code !== "1")
            //            abp.message.info(result.message);
            //        else {
            //            dataTable.ajax.reload();
            //        }
            //    }).always(function () {
            //        abp.ui.clearBusy();
            //    });

           
        }

        $('#ExportToExcelButton').click(function () {
            _rptService.getReportServiceProviderListToExcel({
                fromDate: getDateFilter($('#fromDate')),
                toDate: getDateFilter($('#toDate')),
                serviceCode: $('#selectService').val(),
                categoryCode: $('#selectCategory').val(),
                productCode: $('#selectProduct').val(),
                agentType: $("#agentType").val(),
                agentCode: $("#selectAgent").val(),
                agentCodeParent: $("#selectAgentParent").val(),
                providerCode: $('#selectProvider').val(),
                receiverType: $('#selectReceiverType').val(),
                providerReceiverType: $('#selectReceiverTypeResponse').val(),
            }).done(function (result) {
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

        $("#selectProvider").select2();

        $("#agentType").select2();

        $("#selectProduct").select2();
        $("#selectService").select2();
        $("#selectCategory").select2();

        $("#selectService").change(function (e) {
            const serviceCode = $(e.target).val();
            $("#selectCategory").text('');
            $("#selectProduct").text('');
            Sv.GetCateByServiceMuti(serviceCode, $("#selectCategory"), false);
            Sv.GetProductByCateMuti('', $("#selectProduct"), false);
        });

        $("#selectCategory").change(function (e) {
            const cateCode = $(e.target).val();
            $("#selectProduct").text('');
            Sv.GetProductByCateMuti(cateCode, $("#selectProduct"), false);
        });

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

        $("#selectAgentParent").select2({
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
