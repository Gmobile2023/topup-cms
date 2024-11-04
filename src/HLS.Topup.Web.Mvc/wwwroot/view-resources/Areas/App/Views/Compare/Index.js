(function () {
    $(function () {

        var _$providersTable = $('#ProvidersTable');

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
                ajaxFunction: _rptService.getReportDetailList,
                inputFilter: function () {
                    return {
                        filter: $('#ProvidersTableFilter').val(),
                        transCodeFilter: $("#TransCodeFilterId").val(),
                        serviceCode: $('#ServiceCodeFilter').val(),
                        accountCodeFilter: $("#AccountCode").val(),
                        fromDateFilter: getDateFilter($('#FromDateFilterId')),
                        toDateFilter: getDateFilter($('#ToDateFilterId')),
                        transType: $("#ServiceCodeFilter").val()
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
                    data: "transCode"
                },
                {
                    targets: 2,
                    data: "createdDate",
                    render: function (createdTime) {
                        if (createdTime) {
                            return moment(new Date(createdTime)).format('DD/MM/YYYY HH:mm:ss');
                        }
                        return "";
                    }
                },
                {
                    targets: 3,
                    data: "serviceName"
                },
                {
                    targets: 4,
                    data: "balanceBefore",
                    //className: "text-right",
                    render: function (balanceBefore) {
                        if (balanceBefore) {
                            return Sv.format_number(balanceBefore)
                        }
                        return "0";
                    }
                },
                {
                    targets: 5,
                    data: "increment",
                    //className: "text-right",
                    render: function (increment) {
                        if (increment) {
                            return Sv.format_number(increment)
                        }
                        return "0";
                    }
                },
                {
                    targets: 6,
                    data: "decrement",
                    //className: "text-right",
                    render: function (decrement) {
                        if (decrement) {
                            return Sv.format_number(decrement)
                        }
                        return "0";
                    }
                },
                {
                    targets: 7,
                    data: "balanceAfter",
                    //className: "text-right",
                    render: function (balanceAfter) {
                        if (balanceAfter) {
                            return Sv.format_number(balanceAfter)
                        }
                        return "0";
                    }
                },
                {
                    targets: 8,
                    data: "transNote"
                }
            ],
            "headerCallback": function (thead, data, start, end, display) {
                try {
                    var rawServerResponse = this.api().settings()[0].rawServerResponse;
                    if (rawServerResponse !== undefined && rawServerResponse.totalData !== undefined) {
                        $(thead).find('th').eq(1).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.balanceBefore));
                        $(thead).find('th').eq(2).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.increment));
                        $(thead).find('th').eq(3).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.decrement));
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


        //$('#ShowAdvancedFiltersSpan').click(function () {
        //    $('#ShowAdvancedFiltersSpan').hide();
        //    $('#HideAdvancedFiltersSpan').show();
        //    $('#AdvacedAuditFiltersArea').slideDown();
        //});

        //$('#HideAdvancedFiltersSpan').click(function () {
        //    $('#HideAdvancedFiltersSpan').hide();
        //    $('#ShowAdvancedFiltersSpan').show();
        //    $('#AdvacedAuditFiltersArea').slideUp();
        //});

        $('#ExportToExcelButton').click(function () {
            _rptService.getReportDetailListToExcel({
                filter: $('#ProvidersTableFilter').val(),
                serviceCode: $('#ServiceCodeFilter').val(),
                accountCodeFilter: $('#AccountCode').val(),
                transCodeFilter: $('#TransCodeFilterId').val(),
                fromDateFilter: getDateFilter($('#FromDateFilterId')),
                toDateFilter: getDateFilter($('#ToDateFilterId'))
            })
                .done(function (result) {
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

        $("#AccountCode").select2({
            placeholder: 'Select',
            ajax: {
                url: abp.appPath + "api/services/app/CommonLookup/GetListUserSearch",
                dataType: 'json',
                delay: 250,
                data: function (params) {
                    return {
                        search: params.term,
                        accountType:99
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
                        "phoneNumber": "master@gmail.com",
                        "mobile": "",
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
                    params.page = params.page || 1;

                    return {
                        results: $.map(data.result, function (item) {
                            return {
                                text: item.accountCode + " - " + item.phoneNumber + " - " + item.fullName,
                                id: item.accountCode
                            }
                        }),
                        // pagination: {
                        //     more: (params.page * 30) < data.result.length
                        // }
                    };
                },
                cache: true
            },
            minimumInputLength: 1,
            language: abp.localization.currentCulture.name
        });
    });
})();
