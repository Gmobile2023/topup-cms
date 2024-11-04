(function () {
    $(function () {

        var _$providersTable = $('#ProvidersTable');
        var _providersService = abp.services.app.providers;

        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Providers.Create'),
            edit: abp.auth.hasPermission('Pages.Providers.Edit'),
            'delete': abp.auth.hasPermission('Pages.Providers.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Providers/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Providers/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditProviderModal',
            modalSize: 'modal-xl size-80'
        });

        var _viewProviderModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Providers/ViewproviderModal',
            modalClass: 'ViewProviderModal',
            modalSize: 'modal-xl'
        });


        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z");
        }
        const _rptService = abp.services.app.reportSystem;
        const dataTable = _$providersTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _rptService.getReportTotalList,
                inputFilter: function () {
                    return {
                        accountCodeFilter: $("#AccountCode").val(),
                        fromDateFilter: getDateFilter($('#FromDateFilterId')),
                        toDateFilter: getDateFilter($('#ToDateFilterId')),
                        agentType: $('#agentType').val()
                    };
                },
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
                    data: "agentType",
                    className: "all",
                    render: function (agentType) {
                        if (agentType == 1)
                            return "Đại lý";
                        else if (agentType == 2)
                            return "Đại lý API";
                        else if (agentType == 3)
                            return "Đại lý công ty";
                        else if (agentType == 4)
                            return "Đại lý Tổng";
                        else if (agentType == 5)
                            return "Đại lý cấp 1";
                        else if (agentType == 6)
                            return "Đại lý sỉ";
                        else
                            return "Đại lý";
                    }
                },
                {
                    targets: 2,
                    data: "accountInfo"
                },
                {
                    targets: 3,
                    data: "balanceBefore",
                    className: "all text-right",
                    render: function (balanceBefore) {
                        if (balanceBefore) {
                            return Sv.format_number(balanceBefore)
                        }
                        return "0";
                    }
                },
                {
                    targets: 4,
                    data: "credited",
                    className: "all text-right",
                    render: function (credited) {
                        if (credited) {
                            return Sv.format_number(credited)
                        }
                        return "0";
                    }
                },
                {
                    targets: 5,
                    data: "debit",
                    className: "all text-right",
                    render: function (debit) {
                        if (debit) {
                            return Sv.format_number(debit)
                        }
                        return "0";
                    }
                },
                {
                    targets: 6,
                    data: "balanceAfter",
                    className: "all text-right",
                    render: function (balanceAfter) {
                        if (balanceAfter) {
                            return Sv.format_number(balanceAfter)
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
                        $(thead).find('th').eq(1).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.balanceBefore));
                        $(thead).find('th').eq(2).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.credited));
                        $(thead).find('th').eq(3).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.debit));
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
            _rptService.getReportTotalListToExcel({
                filter: $('#ProvidersTableFilter').val(),
                accountCodeFilter: $('#AccountCode').val(),
                fromDateFilter: getDateFilter($('#FromDateFilterId')),
                toDateFilter: getDateFilter($('#ToDateFilterId')),
                agentType: $('#agentType').val()
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
            getProviders();
        });

        $("#AccountCode").select2({
            placeholder: 'Tìm kiếm',
            allowClear: true,
            ajax: {
                url: abp.appPath + "api/services/app/CommonLookup/GetListUserSearch",
                dataType: 'json',
                delay: 250,
                data: function (params) {
                    return {
                        search: params.term,
                        accountType: 99
                        //page: params.page
                    };
                },
                processResults: function (data, params) {
                    data.result.push({
                        "userId": 0,
                        "accountCode": "PAYMENT",
                        "fullName": "PAYMENT",
                        "email": "payment@gmail.com",
                        "phoneNumber": "",
                        "userName": "PAYMENT"
                    });
                    data.result.push({
                        "userId": 0,
                        "accountCode": "MASTER",
                        "fullName": "MASTER",
                        "email": "master@gmail.com",
                        "phoneNumber": "",
                        "userName": "MASTER"
                    });
                    data.result.push({
                        "userId": 0,
                        "accountCode": "CONTROL",
                        "fullName": "CONTROL",
                        "email": "control@gmail.com",
                        "phoneNumber": "",
                        "userName": "CONTROL"
                    });
                    data.result.push({
                        "userId": 0,
                        "accountCode": "COMMISSION",
                        "fullName": "COMMISSION",
                        "email": "commission@gmail.com",
                        "phoneNumber": "",
                        "userName": "COMMISSION"
                    });
                    params.page = params.page || 1;

                    return {
                        results: $.map(data.result, function (item) {
                            return {
                                text: item.accountCode + " - " + item.phoneNumber + " - " + item.fullName,
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
            minimumInputLength: 1,
            language: abp.localization.currentCulture.name
        });
    });
})();
