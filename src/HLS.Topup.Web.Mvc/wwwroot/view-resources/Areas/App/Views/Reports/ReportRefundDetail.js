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
                ajaxFunction: _rptService.getReportRefundDetailList,
                inputFilter: function () {
                    return {
                        fromDate: getDateFilter($('#fromDate')),
                        toDate: getDateFilter($('#toDate')),
                        agentCode: $("#selectAgent").val(),
                        transCode: $("#transCode").val(),
                        transCodeSouce: $("#transCodeSouce").val(),
                        serviceCode: $('#selectService').val(),
                        categoryCode: $('#selectCategory').val(),
                        productCode: $('#selectProduct').val()
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
                    data: "agentInfo"
                },
                {
                    targets: 2,
                    data: "agentName"
                },
                {
                    targets: 3,
                    data: "transCode"
                },
                {
                    targets: 4,
                    data: "serviceName"
                },
                {
                    targets: 5,
                    data: "categoryName"
                },
                {
                    targets: 6,
                    data: "productName"
                },
                {
                    targets: 7,
                    data: "price",
                    className: "all text-right",
                    render: function (price) {
                        if (price) {
                            return Sv.format_number(price)
                        }
                        return "0";
                    }
                },
                {
                    targets: 8,
                    data: "transCodeSouce"
                },
                {
                    targets: 9,
                    data: "createdTime",
                    render: function (createdTime) {
                        if (createdTime) {
                            return moment(new Date(createdTime)).format('DD/MM/YYYY HH:mm:ss');
                        }
                        return "";
                    }
                },
            ],
            "headerCallback": function (thead, data, start, end, display) {
                try {
                    var rawServerResponse = this.api().settings()[0].rawServerResponse;
                    if (rawServerResponse.warning.length > 0)
                        abp.message.info(rawServerResponse.warning);

                    if (rawServerResponse !== undefined && rawServerResponse.totalData !== undefined) {                      
                        $(thead).find('th').eq(1).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.price));
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
            _rptService.getReportRefundDetailListToExcel({
                fromDate: getDateFilter($('#fromDate')),
                toDate: getDateFilter($('#toDate')),
                agentCode: $("#selectAgent").val(),
                transCode: $("#transCode").val(),
                transCodeSouce: $("#transCodeSouce").val(),
                serviceCode: $('#selectService').val(),
                categoryCode: $('#selectCategory').val(),
                productCode: $('#selectProduct').val()
            })
                .done(function (result) {
                    if (result.fileName === "WAIT")
                        abp.message.info('Hệ thống đang bắt đầu lấy dữ liệu. Quá trình hoàn tất sẽ gửi thông báo cho bạn. Trân trọng!');
                    else if (result.fileName === "Warning")
                        abp.message.info(result.filePath);
                    else if (result.fileName === "Downloadlink")
                        window.open(result.filePath);
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

        $("#selectService").select2();
        $("#selectCategory").select2();
        $("#selectProduct").select2();

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
