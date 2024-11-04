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
                ajaxFunction: _rptService.getReportCommissionDetailList,
                inputFilter: function () {
                    return {
                        fromDate: getDateFilter($('#fromDate')),
                        toDate: getDateFilter($('#toDate')),
                        filter: $("#txtFilter").val(),
                        transCode: $("#txttransCode").val(),
                        agentCodeSum: $("#selectAgentSum").val(),
                        agentCode: $("#selectAgent").val(),
                        serviceCode: $('#selectService').val(),
                        categoryCode: $('#selectCategory').val(),
                        productCode: $('#selectProduct').val(),
                        status: -1, /*$('#selectStatus').val()*/
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
                    data: "agentSumCode"
                },
                {
                    targets: 2,
                    data: "commissionCode"
                },
                {
                    targets: 3,
                    data: "commissionAmount",
                    render: function (commissionAmount) {
                        if (commissionAmount) {
                            return Sv.format_number(commissionAmount);
                        }
                        return "0";
                    }
                },
                {
                    targets: 4,
                    data: "statusName"
                },
                {
                    targets: 5,
                    data: "payDate",
                    render: function (payDate) {
                        if (payDate) {
                            return moment(new Date(payDate)).format('DD/MM/YYYY HH:mm:ss');
                        }
                        return "";
                    }
                },
                {
                    targets: 6,
                    data: "agentCode",
                },
                {
                    targets: 7,
                    data: "transCode"
                },
                {
                    targets: 8,
                    data: "requestRef"
                },
                {
                    targets: 9,
                    data: "serviceName"
                },
                {
                    targets: 10,
                    data: "categoryName"
                },
                {
                    targets: 11,
                    data: "productName"
                },
                {
                    targets: 12,
                    data: "createDate",
                    render: function (createDate) {
                        if (createDate) {
                            return moment(new Date(createDate)).format('DD/MM/YYYY HH:mm:ss');
                        }
                        return "";
                    }
                }
            ],
            "headerCallback": function (thead, data, start, end, display) {
                try {
                    var rawServerResponse = this.api().settings()[0].rawServerResponse;

                    if (rawServerResponse.warning.length > 0)
                        abp.message.info(rawServerResponse.warning);

                    if (rawServerResponse !== undefined && rawServerResponse.totalData !== undefined) {
                        $(thead).find('th').eq(1).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.commissionAmount));
                    }
                } catch (e) {
                    console.log("không có total")
                }
            }
        });

        function getDataSearch() {
            dataTable.ajax.reload();
        }

        $('#ExportToExcelButton').click(function () {
            _rptService.getReportCommissionDetailListToExcel({
                filter: $("#txtFilter").val(),
                fromDate: getDateFilter($('#fromDate')),
                toDate: getDateFilter($('#toDate')),
                transCode: $("#txttransCode").val(),
                agentCodeSum: $("#selectAgentSum").val(),
                agentCode: $("#selectAgent").val(),
                serviceCode: $('#selectService').val(),
                categoryCode: $('#selectCategory').val(),
                productCode: $('#selectProduct').val(),
                status: -1 /*$('#selectStatus').val()*/
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
            getDataSearch();
        });

        $('#GetSearchButton').click(function (e) {
            e.preventDefault();
            getDataSearch();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getDataSearch();
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
                        saleLeaderCode: $('#selectAgentSum option:selected').val()
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

        $("#selectService").select2();
        $("#selectCategory").select2();
        $("#selectProduct").select2();

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
    });
})();
