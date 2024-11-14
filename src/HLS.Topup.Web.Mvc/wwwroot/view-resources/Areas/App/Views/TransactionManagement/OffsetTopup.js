(function () {
    $(function () {

        var _$providersTable = $('#ProvidersTable');

        $('.datetime-index').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L LTS'
        });

        const _permissions = {
            create: abp.auth.hasPermission('Pages.TransactionManagements.Create'),
            edit: abp.auth.hasPermission('Pages.TransactionManagements.Edit'),
            delete: abp.auth.hasPermission('Pages.TransactionManagements.Delete'),
            cancel: abp.auth.hasPermission('Pages.TransactionManagements.Cancel'),
            refund: abp.auth.hasPermission('Pages.TransactionManagements.Refund'),
            pin_code: abp.auth.hasPermission('Pages.TransactionManagements.PinCode'),
        };


        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY/MM/DD HH:mm:ss");
        }

        var getToFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY/MM/DD HH:mm:ss");
        }

        const _transaction = abp.services.app.transactions;
        const dataTable = _$providersTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _transaction.getOffsetTopupHistoryRequest,
                inputFilter: function () {
                    return {
                        fromDate: getDateFilter($('#FromDateFilterId')),
                        toDate: getDateFilter($('#ToDateFilterId')),
                        originPartnerCode: $('#originAgentFilter').val(),
                        partnerCode: $('#agentFilter').val(),
                        status: $('#StatusFilterId').val(),
                        receiverInfo: $('#MobileNumberFilterId').val(),
                        originTransCode: $('#txtOriginTransCode').val(),
                        transCode: $('#txtTransCode').val()
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
                    data: "status",
                    name: "status",
                    render: function (status) {
                        const $span = $("<span/>").addClass("label");
                        if (status === 1) {
                            $span.addClass("label label-success label-inline").text(app.localize('Enum_TopupStatus_' + status));
                        } else if (status === 4 || status === 7 || status === 8 || status === 10) {
                            $span.addClass("label label-warning label-inline").text(app.localize('Enum_TopupStatus_' + status));
                        } else if (status === 9) {
                            $span.addClass("label label-primary label-inline").text(app.localize('Enum_TopupStatus_' + status));
                        } else if (status === 2 || status === 3) {
                            $span.addClass("label label-danger label-inline").text(app.localize('Enum_TopupStatus_' + status));
                        } else if (status === 6) {
                            $span.addClass("label label-info label-inline").text(app.localize('Enum_TopupStatus_' + status));
                        } else if (status === 10) {
                            $span.addClass("label label-danger label-inline").text(app.localize('Enum_TopupStatus_' + status));
                        } else {
                            $span.addClass("label label-default label-inline").text(app.localize('Enum_TopupStatus_' + status));
                        }
                        return $span[0].outerHTML;
                    }
                },
                {
                    targets: 2,
                    data: "amount",
                    render: function (data) {
                        return data ? Sv.format_number(data) : "0"
                    }
                },
                {
                    targets: 3,
                    data: "receiverInfo"
                },
                {
                    targets: 4,
                    data: "createdTime",
                    name: "createdTime",
                    render: function (createdTime) {
                        if (createdTime) {
                            return moment(new Date(createdTime)).format('DD/MM/YYYY HH:mm:ss');
                        }
                        return "";
                    }
                },
                {
                    targets: 5,
                    data: "originPartnerCode",
                    render: function (data, row) {
                        return data;
                    }
                },
                {
                    targets: 6,
                    data: "partnerCode"
                },
                {
                    targets: 7,
                    data: "originTransCode"
                },
                {
                    targets: 8,
                    data: "originProviderCode"
                },
                {
                    targets: 9,
                    data: "transCode"
                }
            ]
        });

        function getProviders() {
            dataTable.ajax.reload();
        }

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

        $('#ExportToExcelButton').click(function () {
            _transaction.getOffsetTopupHistoryToExcel({
                fromDate: getDateFilter($('#FromDateFilterId')),
                toDate: getDateFilter($('#ToDateFilterId')),
                originPartnerCode: $('#originAgentFilter').val(),
                partnerCode: $('#agentFilter').val(),
                status: $('#StatusFilterId').val(),
                receiverInfo: $('#MobileNumberFilterId').val(),
                originTransCode: $('#txtOriginTransCode').val(),
                transCode: $('#txtTransCode').val()
            })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('.btn-search-transaction').click(function (e) {
            e.preventDefault();
            getProviders();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getProviders();
            }
        });

        $("#StaffAccountFilter").select2({
            placeholder: 'Select',
            ajax: {
                url: abp.appPath + "api/services/app/CommonLookup/GetListUserSearch",
                dataType: 'json',
                delay: 250,
                data: function (params) {
                    return {
                        search: params.term,
                        agentType: 99,
                        page: params.page,
                        accountType: 99
                    };
                },
                processResults: function (data, params) {
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


        $("#originAgentFilter").select2({
            placeholder: 'Chọn đại lý',
            allowClear: true,
            ajax: {
                url: abp.appPath + "api/services/app/CommonLookup/GetListUserSearch",
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
            minimumInputLength: 3,
            language: abp.localization.currentCulture.name
        });


        $("#agentFilter").select2({
            placeholder: 'Chọn đại lý',
            allowClear: true,
            ajax: {
                url: abp.appPath + "api/services/app/CommonLookup/GetListUserSearch",
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
            minimumInputLength: 3,
            language: abp.localization.currentCulture.name
        });

        $("#AdvacedAuditFiltersArea").on('click', '.s2icon', function () {
            $("#PartnerCodeFilterId").val("").trigger("change");
            $("#PartnerCodeFilterId").parent().find(".select2-selection.select2-selection--single .s2icon").remove();
        });
    });
})();
