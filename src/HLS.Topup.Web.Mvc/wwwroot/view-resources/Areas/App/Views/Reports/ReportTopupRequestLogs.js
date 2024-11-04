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
        const _commonLookupService = abp.services.app.commonLookup;
        const dataTable = _$detailTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _rptService.getReportTopupRequestLogList,
                inputFilter: function () {
                    return {
                        fromDate: getDateFilter($('#fromDate')),
                        toDate: getDateFilter($('#toDate')),
                        agentType: $("#agentType").val(),
                        agentCode: $("#selectAgent").val(),
                        receivedAccount: $("#receivedAccount").val(),
                        transCode: $("#transCode").val(),
                        requestRef: $("#requestRef").val(),
                        payTransRef: $("#payTransRef").val(),
                        venderCode: $('#selectVender').val(),
                        serviceCode: $('#selectService').val(),
                        categoryCode: $('#hdnCategoryCode').val() == "" ? $('#selectCategory').val() : $('#hdnCategoryCode').val(),
                        productCode: $('#hdnProductCode').val() == "" ? $('#selectProduct').val() : $('#hdnProductCode').val(),
                        status: $('#selectStatus').val()
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
                    data: "transRef"
                },
                {
                    targets: 2,
                    data: "transCode"
                },
                {
                    targets: 3,
                    data: "serviceCode"
                },
                {
                    targets: 4,
                    data: "categoryCode",
                },
                {
                    targets: 5,
                    data: "productCode"
                },
                {
                    targets: 6,
                    data: "providerCode"
                },
                {
                    targets: 7,
                    data: "partnerCode"
                },
                {
                    targets: 8,
                    data: "transAmount",
                    className: "all text-right",
                    render: function (value) {
                        if (value) {
                            return Sv.format_number(value);
                        }
                        return "0";
                    }
                },
                {
                    targets: 9,
                    data: "receiverInfo"
                },
                {
                    targets: 10,
                    data: "transIndex"
                },
                {
                    targets: 11,
                    data: "requestDate",
                    render: function (requestDate) {
                        if (requestDate) {
                            return moment(new Date(requestDate)).format('DD/MM/YYYY HH:mm:ss');
                        }
                        return "";
                    }
                },
                {
                    targets: 12,
                    data: "modifiedDate",
                    render: function (modifiedDate) {
                        if (modifiedDate) {
                            return moment(new Date(modifiedDate)).format('DD/MM/YYYY HH:mm:ss');
                        }
                        return "";
                    }
                },
                {
                    targets: 13,
                    data: "statusName"
                },
                {
                    targets: 14,
                    data: "responseInfo"
                }
            ],
            "headerCallback": function (thead, data, start, end, display) {
                try {
                    var rawServerResponse = this.api().settings()[0].rawServerResponse;                 
                    if (rawServerResponse.warning.length > 0)
                        abp.message.info(rawServerResponse.warning);
                    
                    if (rawServerResponse !== undefined && rawServerResponse.totalData !== undefined) {
                        //$(thead).find('th').eq(1).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.value));
                        //$(thead).find('th').eq(2).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.quantity));
                        //$(thead).find('th').eq(3).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.discount));
                        //$(thead).find('th').eq(4).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.fee));
                        //$(thead).find('th').eq(5).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.price));
                        //$(thead).find('th').eq(6).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.commistionAmount));
                    }
                } catch (e) {
                    console.log("Không có total")
                }
            }
        });

        function getProviders() {
            dataTable.ajax.reload();
        }

        $('#ExportToExcelButton').click(function () {
            abp.ui.setBusy();
            _rptService.getReportTopupRequestLogListToExcel({
                fromDate: getDateFilter($('#fromDate')),
                toDate: getDateFilter($('#toDate')),
                agentType: $("#agentType").val(),
                agentCode: $("#selectAgent").val(),
                receivedAccount: $("#receivedAccount").val(),
                transCode: $("#transCode").val(),
                requestRef: $("#requestRef").val(),
                payTransRef: $("#payTransRef").val(),
                venderCode: $('#selectVender').val(),
                serviceCode: $('#selectService').val(),
                categoryCode: $('#hdnCategoryCode').val() == "" ? $('#selectCategory').val() : $('#hdnCategoryCode').val(),
                productCode: $('#hdnProductCode').val() == "" ? $('#selectProduct').val() : $('#hdnProductCode').val(),
                status: $('#selectStatus').val(),
            })
                .done(function (result) {
                    if (result.fileName === "WAIT")
                        abp.message.info('Hệ thống đang bắt đầu lấy dữ liệu. Quá trình hoàn tất sẽ gửi thông báo cho bạn. Trân trọng!');
                    if (result.fileName === "Warning")
                        abp.message.info(result.filePath);
                    else if (result.fileName === "Downloadlink")
                        window.open(result.filePath);
                    else
                        app.downloadTempFile(result);
                }).always(function () {
                    abp.ui.clearBusy();
                });
        });

        $('#GetProvidersButton').click(function (e) {
            $('#hdnCategoryCode').val('');
            $('#hdnProductCode').val('');
            e.preventDefault();
            getProviders();
        });

        $('#GetProvidersButtonFillter').click(function (e) {
            $('#hdnCategoryCode').val('');
            $('#hdnProductCode').val('');
            e.preventDefault();
            getProviders();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getProviders();
            }
        });

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

        $("#selectService").select2();
        $("#selectCategory").select2();
        $("#selectProduct").select2();

        $("#selectVender").select2();

        $("#agentType").select2();

        $("#selectCity").select2();
        $("#selectDistrict").select2();
        $("#selectWard").select2();

        $("#selectUserAgentStaff").select2({
            placeholder: 'Tìm kiếm',
            allowClear: true,
            ajax: {
                url: abp.appPath + "api/services/app/CommonLookup/GetUserAgentStaff",
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
                        //agentType: $('#agentType option:selected').val(),
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

        $(document).ready(function () {
            _getProvince();
            $('#selectDistrict').prop('disabled', true);
            $('#selectWard').prop('disabled', true);

            $("#selectCity").on('change', function () {
                let id = $(this).val();
                if (id !== undefined && id !== '') {
                    $('#selectDistrict').removeAttr('disabled');
                    _getDistrict(id);
                } else {
                    $('#selectDistrict').prop('disabled', true);
                }
            });

            $("#selectDistrict").on('change', function () {
                let id = $(this).val();
                if (id !== undefined && id !== '') {
                    $('#selectWard').removeAttr('disabled');
                    _getWard(id);
                } else {
                    $('#selectWard').prop('disabled', true);
                }
            });
        });

        function _getProvince() {
            _commonLookupService.getProvinces().done(function (data) {
                if (data !== null && data !== undefined) {
                    var html = '';
                    html += '<option value="0">Tất cả</option>';
                    $.each(data, function (key, item) {
                        html += '<option value=' + item.id + '>' + item.cityName + '</option>';
                    });
                    $("#selectCity").html(html);
                }
            });
        }

        function _getDistrict(id) {
            _commonLookupService.getDistricts(id, true).done(function (data) {
                if (data !== null && data !== undefined) {
                    var html = '';
                    html += '<option value="0">Tất cả</option>';
                    $.each(data, function (key, item) {
                        html += '<option value=' + item.id + '>' + item.districtName + '</option>';
                    });
                    $("#selectDistrict").html(html);
                }
            });
        }

        function _getWard(id) {
            _commonLookupService.getWards(id, true).done(function (data) {
                if (data !== null && data !== undefined) {
                    var html = '';
                    html += '<option value="0">Tất cả</option>';
                    $.each(data, function (key, item) {
                        html += '<option value=' + item.id + '>' + item.wardName + '</option>';
                    });
                    $("#selectWard").html(html);
                }
            });
        }
    });
})();
