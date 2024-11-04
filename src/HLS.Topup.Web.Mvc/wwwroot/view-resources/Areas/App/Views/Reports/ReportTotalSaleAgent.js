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
        const _commonLookupService = abp.services.app.commonLookup;
        const dataTable = _$totalTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _rptService.getReportTotalSaleAgentList,
                inputFilter: function () {
                    return {
                        fromDate: getDateFilter($('#fromDate')),
                        toDate: getDateFilter($('#toDate')),
                        agentType: $("#agentType").val(),
                        agentCode: $("#selectAgent").val(),
                        userSaleLeaderCode: $("#selectUserSaleLeader").val(),
                        userSaleCode: $("#selectUserSale").val(),
                        serviceCode: $('#selectService').val(),
                        categoryCode: $('#selectCategory').val(),
                        productCode: $('#selectProduct').val(),
                        cityId: ($('#selectCity').val() == null || $('#selectCity').val() == "") ? "0" : $('#selectCity').val(),
                        districtId: ($('#selectDistrict').val() === "" || $('#selectDistrict').val() === null) ? "0" : $('#selectDistrict').val(),
                        wardId: ($('#selectWard').val() === "" || $('#selectWard').val() === null) ? "0" : $('#selectWard').val(),
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
                    data: "agentInfo",
                    render: function (data, type, row) {
                        return "<a href='" + row.link + "'>" + row.agentInfo + '</a>';
                    }
                },
                {
                    targets: 3,
                    data: "agentName"
                },
                {
                    targets: 4,
                    data: "saleInfo"
                },
                {
                    targets: 5,
                    data: "saleLeaderInfo"
                },
                {
                    targets: 6,
                    data: "cityInfo"
                },
                {
                    targets: 7,
                    data: "districtInfo"
                },
                {
                    targets: 8,
                    data: "wardInfo"
                },
                {
                    targets: 9,
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
                    targets: 10,
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
                    targets: 11,
                    data: "price",
                    className: "all text-right",
                    render: function (data, type, row) {
                        if (row.price) {
                            return "<a href='" + row.link + "'>" + Sv.format_number(row.price) + '</a>';
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
                        console.log("bui van tien: " + rawServerResponse.totalData);
                        $(thead).find('th').eq(1).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.quantity));
                        $(thead).find('th').eq(2).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.discount));
                        $(thead).find('th').eq(3).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.price));
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
            _rptService.getReportTotalSaleAgentListToExcel({
                fromDate: getDateFilter($('#fromDate')),
                toDate: getDateFilter($('#toDate')),
                agentType: $("#agentType").val(),
                agentCode: $("#selectAgent").val(),
                userSaleLeaderCode: $("#selectUserSaleLeader").val(),
                userSaleCode: $("#selectUserSale").val(),
                serviceCode: $('#selectService').val(),
                categoryCode: $('#selectCategory').val(),
                productCode: $('#selectProduct').val(),
                cityId: ($('#selectCity').val() == null || $('#selectCity').val() == "") ? "0" : $('#selectCity').val(),
                districtId: ($('#selectDistrict').val() === "" || $('#selectDistrict').val() === null) ? "0" : $('#selectDistrict').val(),
                wardId: ($('#selectWard').val() === "" || $('#selectWard').val() === null) ? "0" : $('#selectWard').val(),
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


        $("#selectService").select2();
        $("#selectCategory").select2();
        $("#selectProduct").select2();

        $("#agentType").select2();

        $("#selectCity").select2();

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
                        page: params.page
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
