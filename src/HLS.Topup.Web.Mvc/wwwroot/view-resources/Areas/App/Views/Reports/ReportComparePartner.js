(function () {
    $(function () {

        var _$totalTablePinCode = $('#totalTablePinCode');
        var _$totalTablePinGame = $('#totalTablePinGame');
        var _$totalTablePrepaid = $('#totalTablePrepaid');
        var _$totalTablePostpaid = $('#totalTablePostpaid');
        var _$totalTableData = $('#totalTableData');
        var _$totalTableTopup = $('#totalTableTopup');        
        var _$totalTablePayBill = $('#totalTablePayBill');
        var _$totalTableBalance = $('#totalTableBalance');

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

        const dataTablePinCode = _$totalTablePinCode.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _rptService.getReportComparePartnerList,
                inputFilter: function () {
                    return {
                        fromDate: getDateFilter($('#fromDate')),
                        toDate: getDateFilter($('#toDate')),
                        agentType: $("#agentType").val(),
                        agentCode: $("#selectAgent").val(),
                        type: "PINCODE",
                        serviceCode: "PIN_CODE"
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
                    data: "productValue",
                    render: function (data) {
                        if (data) {
                            return Sv.format_number(data);
                        }
                        return "0";
                    }
                },
                {
                    targets: 4,
                    data: "quantity",
                    className: "all text-right",
                    render: function (data, type, row) {
                        if (row.quantity) {
                            return Sv.format_number(row.quantity);
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
                            return Sv.format_number(row.value);
                        }
                        return "0";
                    }
                },
                {
                    targets: 6,
                    data: "discountRate",
                    className: "all text-right",
                    render: function (data, type, row) {
                        if (row.discountRate) {
                            return Sv.format_number(row.discountRate) + "%";
                        }
                        return "0%";
                    }
                },
                {
                    targets: 7,
                    data: "price",
                    className: "all text-right",
                    render: function (data, type, row) {
                        if (row.price) {
                            return Sv.format_number(row.price);
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
                        $(thead).find('th').eq(1).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.quantity));
                        $(thead).find('th').eq(2).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.value));
                        // $(thead).find('th').eq(3).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.discount));
                        $(thead).find('th').eq(4).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.price));                      
                    }
                } catch (e) {
                    console.log("không có total")
                }
            }
        });

        const dataTablePinGame = _$totalTablePinGame.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _rptService.getReportComparePartnerList,
                inputFilter: function () {
                    return {
                        fromDate: getDateFilter($('#fromDate')),
                        toDate: getDateFilter($('#toDate')),
                        agentType: $("#agentType").val(),
                        agentCode: $("#selectAgent").val(),
                        type: "PINCODE",
                        serviceCode: "PIN_GAME"
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
                    data: "productValue",
                    render: function (data) {
                        if (data) {
                            return Sv.format_number(data);
                        }
                        return "0";
                    }
                },
                {
                    targets: 4,
                    data: "quantity",
                    className: "all text-right",
                    render: function (data, type, row) {
                        if (row.quantity) {
                            return Sv.format_number(row.quantity);
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
                            return Sv.format_number(row.value);
                        }
                        return "0";
                    }
                },
                {
                    targets: 6,
                    data: "discountRate",
                    className: "all text-right",
                    render: function (data, type, row) {
                        if (row.discountRate) {
                            return Sv.format_number(row.discountRate) + "%";
                        }
                        return "0%";
                    }
                },
                {
                    targets: 7,
                    data: "price",
                    className: "all text-right",
                    render: function (data, type, row) {
                        if (row.price) {
                            return Sv.format_number(row.price);
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
                        $(thead).find('th').eq(1).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.quantity));
                        $(thead).find('th').eq(2).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.value));
                        // $(thead).find('th').eq(3).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.discount));
                        $(thead).find('th').eq(4).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.price));                      
                    }
                } catch (e) {
                    console.log("không có total")
                }
            }
        });

        const dataTableTopup = _$totalTableTopup.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _rptService.getReportComparePartnerList,
                inputFilter: function () {
                    return {
                        fromDate: getDateFilter($('#fromDate')),
                        toDate: getDateFilter($('#toDate')),
                        agentType: $("#agentType").val(),
                        agentCode: $("#selectAgent").val(),
                        type: "TOPUP",
                        changerType: ""
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
                    data: "productValue",
                    render: function (data) {
                        if (data) {
                            return Sv.format_number(data);
                        }
                        return "0";
                    }
                },
                {
                    targets: 4,
                    data: "quantity",
                    className: "all text-right",
                    render: function (data, type, row) {
                        if (row.quantity) {
                            return Sv.format_number(row.quantity);
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
                            return Sv.format_number(row.value);
                        }
                        return "0";
                    }
                },
                {
                    targets: 6,
                    data: "discountRate",
                    className: "all text-right",
                    render: function (data, type, row) {
                        if (row.discountRate) {
                            return Sv.format_number(row.discountRate) + "%";
                        }
                        return "0%";
                    }
                },
                {
                    targets: 7,
                    data: "price",
                    className: "all text-right",
                    render: function (data, type, row) {
                        if (row.price) {
                            return Sv.format_number(row.price);
                        }
                        return "0";
                    }
                }               
            ],
            "headerCallback": function (thead, data, start, end, display) {
                try {
                    var rawServerResponse = this.api().settings()[0].rawServerResponse;
                    if (rawServerResponse !== undefined && rawServerResponse.totalData !== undefined) {
                        $(thead).find('th').eq(1).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.quantity));
                        $(thead).find('th').eq(2).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.value));
                        $(thead).find('th').eq(4).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.price));                      
                    }
                } catch (e) {
                    console.log("không có total")
                }
            }
        });

        const dataTablePrepaid = _$totalTablePrepaid.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _rptService.getReportComparePartnerList,
                inputFilter: function () {
                    return {
                        fromDate: getDateFilter($('#fromDate')),
                        toDate: getDateFilter($('#toDate')),
                        agentType: $("#agentType").val(),
                        agentCode: $("#selectAgent").val(),
                        type: "TOPUP",
                        serviceCode:"TOPUP",
                        changerType: "PREPAID"
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
                    data: "productValue",
                    render: function (data) {
                        if (data) {
                            return Sv.format_number(data);
                        }
                        return "0";
                    }
                },
                {
                    targets: 4,
                    data: "quantity",
                    className: "all text-right",
                    render: function (data, type, row) {
                        if (row.quantity) {
                            return Sv.format_number(row.quantity);
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
                            return Sv.format_number(row.value);
                        }
                        return "0";
                    }
                },
                {
                    targets: 6,
                    data: "discountRate",
                    className: "all text-right",
                    render: function (data, type, row) {
                        if (row.discountRate) {
                            return Sv.format_number(row.discountRate) + "%";
                        }
                        return "0%";
                    }
                },
                {
                    targets: 7,
                    data: "price",
                    className: "all text-right",
                    render: function (data, type, row) {
                        if (row.price) {
                            return Sv.format_number(row.price);
                        }
                        return "0";
                    }
                }
            ],
            "headerCallback": function (thead, data, start, end, display) {
                try {
                    var rawServerResponse = this.api().settings()[0].rawServerResponse;
                    if (rawServerResponse !== undefined && rawServerResponse.totalData !== undefined) {
                        $(thead).find('th').eq(1).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.quantity));
                        $(thead).find('th').eq(2).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.value));
                        $(thead).find('th').eq(4).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.price));                        
                    }
                } catch (e) {
                    console.log("không có total")
                }
            }
        });

        const dataTablePostpaid = _$totalTablePostpaid.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _rptService.getReportComparePartnerList,
                inputFilter: function () {
                    return {
                        fromDate: getDateFilter($('#fromDate')),
                        toDate: getDateFilter($('#toDate')),
                        agentType: $("#agentType").val(),
                        agentCode: $("#selectAgent").val(),
                        type: "TOPUP",
                        serviceCode: "TOPUP",
                        changerType: "POSTPAID"
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
                    data: "productValue",
                    render: function (data) {
                        if (data) {
                            return Sv.format_number(data);
                        }
                        return "0";
                    }
                },
                {
                    targets: 4,
                    data: "quantity",
                    className: "all text-right",
                    render: function (data, type, row) {
                        if (row.quantity) {
                            return Sv.format_number(row.quantity);
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
                            return Sv.format_number(row.value);
                        }
                        return "0";
                    }
                },
                {
                    targets: 6,
                    data: "discountRate",
                    className: "all text-right",
                    render: function (data, type, row) {
                        if (row.discountRate) {
                            return Sv.format_number(row.discountRate) + "%";
                        }
                        return "0%";
                    }
                },
                {
                    targets: 7,
                    data: "price",
                    className: "all text-right",
                    render: function (data, type, row) {
                        if (row.price) {
                            return Sv.format_number(row.price);
                        }
                        return "0";
                    }
                }
            ],
            "headerCallback": function (thead, data, start, end, display) {
                try {
                    var rawServerResponse = this.api().settings()[0].rawServerResponse;
                    if (rawServerResponse !== undefined && rawServerResponse.totalData !== undefined) {
                        $(thead).find('th').eq(1).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.quantity));
                        $(thead).find('th').eq(2).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.value));
                        $(thead).find('th').eq(4).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.price));                       
                    }
                } catch (e) {
                    console.log("không có total")
                }
            }
        });

        const dataTableData = _$totalTableData.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _rptService.getReportComparePartnerList,
                inputFilter: function () {
                    return {
                        fromDate: getDateFilter($('#fromDate')),
                        toDate: getDateFilter($('#toDate')),
                        agentType: $("#agentType").val(),
                        agentCode: $("#selectAgent").val(),
                        type: "DATA",
                        serviceCode: "",
                        changerType: ""
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
                    data: "productValue",
                    render: function (data) {
                        if (data) {
                            return Sv.format_number(data);
                        }
                        return "0";
                    }
                },
                {
                    targets: 4,
                    data: "quantity",
                    className: "all text-right",
                    render: function (data, type, row) {
                        if (row.quantity) {
                            return Sv.format_number(row.quantity);
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
                            return Sv.format_number(row.value);
                        }
                        return "0";
                    }
                },
                {
                    targets: 6,
                    data: "discountRate",
                    className: "all text-right",
                    render: function (data, type, row) {
                        if (row.discountRate) {
                            return Sv.format_number(row.discountRate) + "%";
                        }
                        return "0%";
                    }
                },
                {
                    targets: 7,
                    data: "price",
                    className: "all text-right",
                    render: function (data, type, row) {
                        if (row.price) {
                            return Sv.format_number(row.price);
                        }
                        return "0";
                    }
                }
            ],
            "headerCallback": function (thead, data, start, end, display) {
                try {
                    var rawServerResponse = this.api().settings()[0].rawServerResponse;
                    if (rawServerResponse !== undefined && rawServerResponse.totalData !== undefined) {
                        $(thead).find('th').eq(1).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.quantity));
                        $(thead).find('th').eq(2).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.value));
                        $(thead).find('th').eq(4).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.price));                      
                    }
                } catch (e) {
                    console.log("không có total")
                }
            }
        });

        const dataTablePayBill = _$totalTablePayBill.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _rptService.getReportComparePartnerList,
                inputFilter: function () {
                    return {
                        fromDate: getDateFilter($('#fromDate')),
                        toDate: getDateFilter($('#toDate')),
                        agentCode: $("#selectAgent").val(),
                        type: "PAYBILL"
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
                    data: "categoryName"
                },
                {
                    targets: 2,
                    data: "productName"
                },
                {
                    targets: 3,
                    data: "quantity",
                    className: "all text-right",
                    render: function (data, type, row) {
                        if (row.quantity) {
                            return + Sv.format_number(row.quantity);
                        }
                        return "0";
                    }
                },
                {
                    targets: 4,
                    data: "value",
                    className: "all text-right",
                    render: function (data, type, row) {
                        if (row.value) {
                            return Sv.format_number(row.value);
                        }
                        return "0";
                    }
                },
                {
                    targets: 5,
                    data: "feeText",
                    className: "all text-right",
                    render: function (data, type, row) {
                        if (row.value) {
                            return '<div style="width: 400px; word-break: break-all; white-space: pre-line;">' + row.feeText + '</div>';
                        }
                        return "";
                    }

                },
                {
                    targets: 6,
                    data: "fee",
                    className: "all text-right",
                    render: function (data, type, row) {
                        if (row.fee) {
                            return Sv.format_number(row.fee);
                        }
                        return "0";
                    }
                },
                {
                    targets: 7,
                    data: "discount",
                    className: "all text-right",
                    render: function (data, type, row) {
                        if (row.discount) {
                            return Sv.format_number(row.discount);
                        }
                        return "0";
                    }
                }
            ],
            "headerCallback": function (thead, data, start, end, display) {
                try {
                    var rawServerResponse = this.api().settings()[0].rawServerResponse;
                    if (rawServerResponse !== undefined && rawServerResponse.totalData !== undefined) {
                        $(thead).find('th').eq(1).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.quantity));
                        $(thead).find('th').eq(2).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.value));
                        $(thead).find('th').eq(4).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.fee));
                        $(thead).find('th').eq(5).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.discount));                      
                    }
                } catch (e) {
                    console.log("không có total")
                }
            }
        });

        const dataTableBalance = _$totalTableBalance.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _rptService.getReportBalancePartner,
                inputFilter: function () {
                    return {
                        fromDate: getDateFilter($('#fromDate')),
                        toDate: getDateFilter($('#toDate')),
                        agentCode: $("#selectAgent").val(),
                        type: "BALANCE"
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
                    data: "index"
                },
                {
                    targets: 2,
                    data: "name"
                },
                {
                    targets: 3,
                    data: "price",
                    className: "all text-right",
                    render: function (data, type, row) {
                        if (row.price) {
                            return Sv.format_number(row.price);
                        }
                        return "0";
                    }
                }]
        });

        function getComparePartner() {
            dataTablePinCode.ajax.reload();
            dataTablePinGame.ajax.reload();
            dataTableTopup.ajax.reload();
            dataTablePostpaid.ajax.reload();
            dataTablePrepaid.ajax.reload();
            dataTablePostpaid.ajax.reload();
            dataTableData.ajax.reload();
            dataTablePayBill.ajax.reload();
            dataTableBalance.ajax.reload();
        }

        $('#ExportToExcelButton').click(function () {

            if ($("#selectAgent").val() == null || $("#selectAgent").val() == "") {
                abp.message.info('Vui lòng chọn đại lý.');
                return;
            }

            _rptService.getReportComparePartnerToExcel({
                fromDate: getDateFilter($('#fromDate')),
                toDate: getDateFilter($('#toDate')),
                agentCode: $("#selectAgent").val(),
                type: "EXPORT"
            })
                .done(function (result) {
                    if (result.fileName === "Warning")
                        abp.message.info(result.filePath);
                    else
                        app.downloadTempFile(result);
                });
        });

        var _openFromEmailModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Reports/ConfirmEmailModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Reports/ConfirmSendEmail.js',
            modalClass: 'ConfirmEmailModal',
            modalSize: 'modal-xl'
        });

        $('#GetSearchButton').click(function (e) {
            if ($("#selectAgent").val() == null || $("#selectAgent").val() == "") {
                abp.message.info('Vui lòng chọn đại lý.');
                return;
            }
            e.preventDefault();
            getComparePartner();
        });

        $('#btnSendMail').click(function (e) {
            if ($("#selectAgent").val() == null || $("#selectAgent").val() == "") {
                abp.message.info('Vui lòng chọn đại lý.');
                return;
            }
            _openFromEmailModal.open({
                agentCode: $("#selectAgent").val(),
                fromDate: getDateFilter($('#fromDate')),
                toDate: getDateFilter($('#toDate'))
            });
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

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getComparePartner();
            }
        });

    });
})();
