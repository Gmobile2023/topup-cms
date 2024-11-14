(function () {
    $(function () {

        var _$saleClearDebtsTable = $('#SaleClearDebtsTable');
        var _saleClearDebtsService = abp.services.app.saleClearDebts;

        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.SaleClearDebts.Create'),
            edit: abp.auth.hasPermission('Pages.SaleClearDebts.Edit'),
            delete: abp.auth.hasPermission('Pages.SaleClearDebts.Delete'),
            approval: abp.auth.hasPermission('Pages.SaleClearDebts.Approval'),
            cancel: abp.auth.hasPermission('Pages.SaleClearDebts.Cancel')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/SaleClearDebts/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/SaleClearDebts/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditSaleClearDebtModal'
        });

        var _viewSaleClearDebtModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/SaleClearDebts/ViewsaleClearDebtModal',
            modalClass: 'ViewSaleClearDebtModal'
        });

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() === null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z");
        }

        $("#UserId").select2({
            placeholder: 'Select',
            allowClear: true,
            ajax: {
                url: abp.appPath + "api/services/app/CommonLookup/GetListUserSaleSearch",
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

        var dataTable = _$saleClearDebtsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _saleClearDebtsService.getAll,
                inputFilter: function () {
                    return {
                        filter: $('#SaleClearDebtsTableFilter').val(),
                        typeFilter: $('#TypeFilterId').val(),
                        userId: $('#UserId').val(),
                        transCodeFilter: $('#TransCodeFilterId').val(),
                        transCodeBank: $('#TransCodeBank').val(),
                        statusFilter: $('#StatusFilterId').val(),
                        bankId: $('#BankId').val(),
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
                                    _viewSaleClearDebtModal.open({id: data.record.saleClearDebt.id});
                                }
                            },
                            {
                                text: app.localize('Edit'),
                                visible: function (data) {
                                    return _permissions.edit && data.record.saleClearDebt.status !== 1;
                                },
                                action: function (data) {
                                    _createOrEditModal.open({id: data.record.saleClearDebt.id});
                                }
                            },
                            {
                                text: app.localize('Delete'),
                                visible: function (data) {
                                    return _permissions.delete && data.record.saleClearDebt.status !== 1;
                                },
                                action: function (data) {
                                    deleteSaleClearDebt(data.record.saleClearDebt);
                                }
                            },
                            {
                                text: app.localize('Cancel'),
                                visible: function (data) {
                                    return _permissions.cancel && data.record.saleClearDebt.status === 0;
                                },
                                action: function (data) {
                                    lockSaleClearDebt(data.record.saleClearDebt);
                                }
                            },
                            {
                                text: app.localize('Approval'),
                                visible: function (data) {
                                    return _permissions.approval && data.record.saleClearDebt.status === 0;
                                },
                                action: function (data) {
                                    approvalSaleClearDebt(data.record.saleClearDebt);
                                }
                            }]
                    }
                },
                {
                    targets: 2,
                    data: "saleClearDebt.transCode",
                    name: "transCode"
                },
                {
                    targets: 3,
                    data: "saleClearDebt.saleInfo",
                    name: "saleInfo"
                },
                {
                    targets: 4,
                    data: "saleClearDebt.amount",
                    name: "amount",
                    render: function (data) {
                        return data ? Sv.format_number(data) : "0"
                    }
                },
                {
                    targets: 5,
                    data: "saleClearDebt.status",
                    name: "status",
                    render: function (status) {
                        return app.localize('Enum_ClearDebtStatus_' + status);
                    }
                },
                {
                    targets: 6,
                    data: "saleClearDebt.transCodeBank",
                    name: "transCodeBank"
                },
                {
                    targets: 7,
                    data: "bankBankName",
                    name: "bankBankName"
                },
                {
                    targets: 8,
                    data: "saleClearDebt.type",
                    name: "type",
                    render: function (type) {
                        return app.localize('Enum_ClearDebtType_' + type);
                    }
                },
                {
                    targets: 9,
                    data: "saleClearDebt.modifyDate",
                    name: "modifyDate",
                    render: function (modifyDate) {
                        if (modifyDate) {
                            return moment(new Date(modifyDate)).format('DD/MM/YYYY HH:mm:ss');
                        }
                        return "";
                    }
                },
                {
                    targets: 10,
                    data: "saleClearDebt.userModify",
                    name: "userModify"
                },
                {
                    targets: 11,
                    data: "saleClearDebt.creationTime",
                    name: "creationTime",
                    render: function (creationTime) {
                        if (creationTime) {
                            return moment(new Date(creationTime)).format('DD/MM/YYYY HH:mm:ss');
                        }
                        return "";
                    }
                },
                {
                    targets: 12,
                    data: "saleClearDebt.userCreated",
                    name: "userCreated"
                },
            ],
            "headerCallback": function (thead, data, start, end, display) {
                try {
                    var rawServerResponse = this.api().settings()[0].rawServerResponse;
                    if (rawServerResponse !== undefined && rawServerResponse.totalData !== undefined) {
                        $(thead).find('th').eq(1).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.saleClearDebt.amount));
                    }
                } catch (e) {
                    console.log("không có total")
                }
            }
        });

        function getSaleClearDebts() {
            dataTable.ajax.reload();
        }

        function deleteSaleClearDebt(saleClearDebt) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _saleClearDebtsService.delete({
                            id: saleClearDebt.id
                        }).done(function () {
                            getSaleClearDebts(true);
                            abp.notify.success(app.localize('SuccessfullyDeleted'));
                        });
                    }
                }
            );
        }

        function approvalSaleClearDebt(deposit) {
            abp.message.confirm(
                'Bạn có chắc chắn muốn duyệt giao dịch này không?',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _saleClearDebtsService.approval(deposit.transCode, "Duyệt giao dịch nạp tiền").done(function (rs) {
                            getSaleClearDebts(true);
                            abp.message.success("Duyệt giao dịch thành công");
                        });
                    }
                }
            );
        }

        function lockSaleClearDebt(deposit) {
            abp.message.confirm(
                'Bạn có chắc chắn muốn hủy giao dịch này không?',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _saleClearDebtsService.cancel(deposit.transCode).done(function (rs) {
                            getSaleClearDebts(true);
                            abp.message.success("Hủy giao dịch thành công");
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

        $('#CreateNewSaleClearDebtButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportToExcelButton').click(function () {
            _saleClearDebtsService
                .getSaleClearDebtsToExcel({
                    filter: $('#SaleClearDebtsTableFilter').val(),
                    typeFilter: $('#TypeFilterId').val(),
                    userId: $('#UserId').val(),
                    transCodeFilter: $('#TransCodeFilterId').val(),
                    transCodeBank: $('#TransCodeBank').val(),
                    statusFilter: $('#StatusFilterId').val(),
                    bankId: $('#BankId').val(),
                    fromDateFilter: getDateFilter($('#FromDateFilter')),
                    toDateFilter: getDateFilter($('#ToDateFilter'))
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditSaleClearDebtModalSaved', function () {
            getSaleClearDebts();
        });

        $('#GetSaleClearDebtsButton').click(function (e) {
            e.preventDefault();
            getSaleClearDebts();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getSaleClearDebts();
            }
        });
    });
})();