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
                ajaxFunction: _reportService.getReportCommissionAgentDetailList,
                inputFilter: function () {
                    return {
                        fromDate: getDateFilter($('#txtFromDate')),
                        toDate: getDateFilter($('#txtToDate')),
                        transCode: $("#txttransCode").val(),
                        serviceCode: $('#selectService').val(),
                        categoryCode: $('#selectCategory').val(),
                        productCode: $("#selectProduct").val(),
                        status: $("#selectStatus").val(),
                        statusPayment: $("#selectPaymentStatus").val(),
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
                    data: "agentCode",
                    className: 'all ctrl-ss'
                },
                {
                    targets: 2,
                    data: "commissionCode"
                },
                {
                    targets: 3,
                    data: "commissionAmount",
                    render: function (commissionAmount) {
                        return Sv.format_number(commissionAmount)
                    }
                },
                {
                    targets: 4,
                    className: "all text-right",
                    data: "statusPaymentName"
                },
                {
                    targets: 5,
                    data: "payDate",
                    name: "payDate",
                    render: function (payDate) {
                        if (payDate) {
                            return moment(new Date(payDate)).format('DD/MM/YYYY HH:mm:ss');
                        }
                        return "";
                    }
                },
                {
                    targets: 6,
                    data: "requestRef"
                },
                {
                    targets: 7,
                    className: "all text-right",
                    data: "serviceName"
                },
                {
                    targets: 8,
                    className: "all text-right",
                    data: "categoryName"
                },
                {
                    targets: 9,
                    className: "all text-right",
                    data: "productName"
                },
                {
                    targets: 10,
                    className: "all text-right",
                    data: "price",
                    render: function (data) {
                        return data ? Sv.format_number(data) : "0"
                    }
                },
                {
                    targets: 11,
                    className: "all text-right",
                    data: "quantity"
                },
                {
                    targets: 12,
                    className: "all text-right",
                    data: "discount"
                },
                {
                    targets: 13,
                    className: "all text-right",
                    data: "fee",
                    render: function (data) {
                        return data ? Sv.format_number(data) : "0"
                    }
                },
                {
                    targets: 14,
                    data: "totalPrice",
                    className: "all text-right",
                    render: function (data) {
                        return data ? Sv.format_number(data) : "0"
                    }
                },
                {
                    targets: 15,
                    data: "statusName"
                },
                {
                    targets: 16,
                    data: "createDate",
                    name: "createDate",
                    render: function (createDate, dispaly, row) {
                        return moment(new Date(createDate)).format('DD/MM/YYYY HH:mm:ss');
                    }
                }
            ],
            "headerCallback": function (thead, data, start, end, display) {
                try {
                    var rawServerResponse = this.api().settings()[0].rawServerResponse;
                    if (rawServerResponse.warning.length > 0)
                        abp.message.info(rawServerResponse.warning);

                    if (rawServerResponse !== undefined && rawServerResponse.totalData !== undefined) {
                        $(thead).find('th').eq(0).addClass("text-right").html(Sv.format_number("Số lượng: " + rawServerResponse.totalData.quantity));
                        $(thead).find('th').eq(1).addClass("text-right").html(Sv.format_number("Doanh số: " + rawServerResponse.totalData.amount));
                        $(thead).find('th').eq(2).addClass("text-right").html(Sv.format_number("Hoa hồng: " + rawServerResponse.totalData.commissionAmount));
                    }
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
            _reportService.getReportCommissionAgentDetailListToExcel({
                fromDate: getDateFilter($('#txtFromDate')),
                toDate: getDateFilter($('#txtToDate')),
                transCode: $("#txttransCode").val(),
                serviceCode: $('#selectService').val(),
                categoryCode: $('#selectCategory').val(),
                productCode: $("#selectProduct").val(),
                status: $("#selectStatus").val(),
                statusPayment: $("#selectPaymentStatus").val(),
                agentCode: $("#selectAgent").val()
            })
                .done(function (result) {
                    if (result.fileName === "Warning")
                        abp.message.info(result.filePath);
                    else if (result.fileName === "WAIT")
                        abp.message.info('Hệ thống đang bắt đầu lấy dữ liệu. Quá trình hoàn tất sẽ gửi thông báo cho bạn. Trân trọng!');
                    else
                        app.downloadTempFile(result);
                });
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

        $("#selectService").change(function (e) {
            const serviceCode = $(e.target).val();
            Sv.GetCateByService(serviceCode, $("#selectCategory"), false);
        });

        $("#selectCategory").change(function (e) {
            const cateCode = $(e.target).val();
            Sv.GetProductByCate(cateCode, $("#selectProduct"), false);
        });

        $("#selectProduct").select2();
    });
})();
