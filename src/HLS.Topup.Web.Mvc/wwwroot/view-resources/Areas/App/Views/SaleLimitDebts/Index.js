(function () {
    $(function () {

        var _$saleLimitDebtsTable = $('#SaleLimitDebtsTable');
        var _saleLimitDebtsService = abp.services.app.saleLimitDebts;

        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.SaleLimitDebts.Create'),
            edit: abp.auth.hasPermission('Pages.SaleLimitDebts.Edit'),
            'delete': abp.auth.hasPermission('Pages.SaleLimitDebts.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/SaleLimitDebts/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/SaleLimitDebts/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditSaleLimitDebtModal'
        });

        var _viewSaleLimitDebtModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/SaleLimitDebts/ViewsaleLimitDebtModal',
            modalClass: 'ViewSaleLimitDebtModal'
        });

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z");
        }

        $("#UserLeaderId").select2({
            placeholder: 'Select',
            allowClear: true,
            ajax: {
                url: abp.appPath + "api/services/app/CommonLookup/GetUserSaleLeader",
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
                                text: item.userName + "-" + item.phoneNumber + "-" + item.fullName,
                                id: item.id
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

        $("#UserId").select2({
            placeholder: 'Select',
            allowClear: true,
            ajax: {
                url: abp.appPath + "api/services/app/CommonLookup/GetUserSaleBySaleLeader",
                dataType: 'json',
                delay: 250,
                data: function (params) {
                    return {
                        search: params.term,
                        saleLeaderId: $("#UserLeaderId").val() === null ? 0 : $("#UserLeaderId").val(),
                        page: params.page,
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

        var dataTable = _$saleLimitDebtsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _saleLimitDebtsService.getAll,
                inputFilter: function () {
                    return {
                        filter: $('#SaleLimitDebtsTableFilter').val(),
                        statusFilter: $('#StatusFilterId').val(),
                        userNameFilter: $('#UserId').val(),
                        saleLeaderId: $('#UserLeaderId').val(),
                        fromDateFilter: getDateFilter($('#FromDateFilter')),
                        toDateFilter: getDateFilter($('#ToDateFilter'))
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
                    width: 120,
                    targets: 1,
                    data: null,
                    orderable: false,
                    autoWidth: false,
                    defaultContent: '',
                    rowAction: {
                        cssClass: 'btn btn-brand dropdown-toggle',
                        text: '<i class="fa fa-cog"></i> ' + app.localize('Actions') + ' <span class="caret"></span>',
                        items: [
                            {
                                text: app.localize('View'),
                                action: function (data) {
                                    _viewSaleLimitDebtModal.open({id: data.record.saleLimitDebt.id});
                                }
                            },
                            {
                                text: app.localize('Edit'),
                                visible: function () {
                                    return _permissions.edit;
                                },
                                action: function (data) {
                                    _createOrEditModal.open({id: data.record.saleLimitDebt.id});
                                }
                            },
                            {
                                text: app.localize('Delete'),
                                visible: function () {
                                    return _permissions.delete;
                                },
                                action: function (data) {
                                    deleteSaleLimitDebt(data.record.saleLimitDebt);
                                }
                            }]
                    }
                },
                {
                    targets: 2,
                    data: "saleLimitDebt.saleInfo",
                    name: "sale"
                },
                {
                    targets: 3,
                    data: "saleLimitDebt.saleLeaderInfo",
                    name: "saleLeaderInfo"
                },
                {
                    targets: 4,
                    data: "saleLimitDebt.limitAmount",
                    name: "limitAmount",
                    render: function (data) {
                        return (data ? Sv.format_number(data) : "0") + " đ"
                    }
                },
                {
                    targets: 5,
                    data: "saleLimitDebt.debtAge",
                    name: "debtAge",
                    render: function (data) {
                        return (data ? Sv.format_number(data) : "0") + " ngày"
                    }
                },
                {
                    targets: 6,
                    data: "saleLimitDebt.status",
                    name: "status",
                    render: function (status) {
                        return app.localize('Enum_DebtLimitAmountStatus_' + status);
                    }

                },
                {
                    targets: 7,
                    data: "saleLimitDebt.createdDate",
                    name: "createdDate",
                    render: function (createdTime) {
                        if (createdTime) {
                            return moment(new Date(createdTime)).format('DD/MM/YYYY HH:mm:ss');
                        }
                        return "";
                    }

                },
                {
                    targets: 8,
                    data: "saleLimitDebt.userCreated",
                    name: "description"
                }
            ]
        });

        function getSaleLimitDebts() {
            dataTable.ajax.reload();
        }

        function deleteSaleLimitDebt(saleLimitDebt) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _saleLimitDebtsService.delete({
                            id: saleLimitDebt.id
                        }).done(function () {
                            getSaleLimitDebts(true);
                            abp.notify.success(app.localize('SuccessfullyDeleted'));
                        });
                    }
                }
            );
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

        $('#CreateNewSaleLimitDebtButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportToExcelButton').click(function () {
            _saleLimitDebtsService
                .getSaleLimitDebtsToExcel({
                    filter: $('#SaleLimitDebtsTableFilter').val(),
                    statusFilter: $('#StatusFilterId').val(),
                    userNameFilter: $('#UserNameFilterId').val(),
                    saleLeaderId: $('#UserLeaderId').val(),
                    fromDateFilter: getDateFilter($('#FromDateFilter')),
                    toDateFilter: getDateFilter($('#ToDateFilter'))
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditSaleLimitDebtModalSaved', function () {
            getSaleLimitDebts();
        });

        $('#GetSaleLimitDebtsButton').click(function (e) {
            e.preventDefault();
            getSaleLimitDebts();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getSaleLimitDebts();
            }
        });
    });
})();