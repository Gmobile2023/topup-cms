(function () {
    $(function () {

        var _$depositsTable = $('#DepositsTable');
        var _depositsService = abp.services.app.deposits;
        var _banksService = abp.services.app.banks;
        //let transaction_type = ['Điều chỉnh tăng', 'Điều chỉnh giảm'];

        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Deposits.Create'),
            edit: abp.auth.hasPermission('Pages.Deposits.Edit'),
            approval: abp.auth.hasPermission('Pages.Deposits.Approval'),
            cancel: abp.auth.hasPermission('Pages.Deposits.Cancel'),
            'delete': abp.auth.hasPermission('Pages.Deposits.Delete')
        };

        var _createDebtModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Deposits/CreateDebtModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Deposits/_CreateDebtModal.js',
            modalClass: 'CreateDebtModal',
            modalSize: 'modal-xl'
        });

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Deposits/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Deposits/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditDepositModal',
            modalSize: 'modal-xl'
        });

        var _viewDepositModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Deposits/ViewdepositModal',
            modalClass: 'ViewDepositModal',
            modalSize: 'modal-xl'
        });

        var _createOrEditAccountingEntryModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Deposits/CreateOrEditAccountingEntryModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Deposits/_CreateOrEditAccountingEntryModal.js',
            modalClass: 'CreateOrEditAccountingEntryModal',
            modalSize: 'modal-xl'
        });

        var _viewAccountingEntryModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Deposits/ViewAccountingEntryModal',
            modalClass: 'ViewAccountingEntryModal',
            //modalSize: 'modal-xl'
        });

        var _createOrEditDepositCashModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Deposits/CreateOrEditDepositCashModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Deposits/_CreateOrEditDepositCashModal.js',
            modalClass: 'CreateOrEditDepositCashModal',
            modalSize: 'modal-xl'
        });

        var _approvedBankModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Deposits/ApprovedBankModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Deposits/_ApprovedBankModal.js',
            modalClass: 'ApprovedBankModal',
            modalSize: 'modal-md modal-dialog-centered'
        });

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z");
        }

        var getToDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT23:59:59Z");
        }

        var dataTable = _$depositsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _depositsService.getAll,
                inputFilter: function () {
                    return {
                        filter: $('#DepositsTableFilter').val(),
                        statusFilter: $('#StatusFilterId').val(),
                        minApprovedDateFilter: getDateFilter($('#MinApprovedDateFilterId')),
                        maxApprovedDateFilter: getToDateFilter($('#MaxApprovedDateFilterId')),
                        transCodeFilter: $('#TransCodeFilter').val(),
                        transCodeBankFilter: $('#TransCodeBankFilter').val(),
                        userId: $('#UserId').val(),
                        bankId: $('#BankId').val(),
                        depositTypeFilter: $('#DepositTypeFilter').val(),
                        agentTypeFilter: $('#AgentTypeFilter').val(),
                        saleLeadFilter: $('#SaleLeaderFilter').val(),
                        saleManFilter: $('#SaleEmpFilter').val(),
                        requestCodeFilter: $('#RequestCodeDepositFilter').val(),
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
                                    if (data.record.deposit.type == 2 || data.record.deposit.type == 3) {
                                        _viewAccountingEntryModal.open({id: data.record.deposit.id});
                                    } else {
                                        _viewDepositModal.open({id: data.record.deposit.id});
                                    }
                                }
                            },
                            {
                                text: app.localize('Edit'),
                                visible: function (data) {
                                    return _permissions.edit && data.record.deposit.status === 0;
                                },
                                action: function (data) {
                                    if (data.record.deposit.type == 4) {
                                        _createDebtModal.open({id: data.record.deposit.id});
                                    } else if (data.record.deposit.type == 2 || data.record.deposit.type == 3) {
                                        _createOrEditAccountingEntryModal.open({id: data.record.deposit.id});
                                    } else if (data.record.deposit.type == 5) {
                                        _createOrEditDepositCashModal.open({id: data.record.deposit.id});
                                    } else {
                                        _createOrEditModal.open({id: data.record.deposit.id});
                                    }
                                }
                            },
                            {
                                text: app.localize('Approval'),
                                visible: function (data) {
                                    return _permissions.approval && data.record.deposit.status === 0;
                                },
                                action: function (data) {
                                    if (data.record.bankBankName != '') {
                                        //approvalBank(data.record.deposit);
                                        _approvedBankModal.open({transCode: data.record.deposit.transCode});
                                    } else {
                                        approval(data.record.deposit);
                                    }
                                }
                            },
                            {
                                text: app.localize('Cancel'),
                                visible: function (data) {
                                    return _permissions.cancel && data.record.deposit.status === 0;
                                },
                                action: function (data) {
                                    cancel(data.record.deposit);
                                }
                            }
                            // {
                            //     text: app.localize('Delete'),
                            //     visible: function () {
                            //         return _permissions.delete;
                            //     },
                            //     action: function (data) {
                            //         deleteDeposit(data.record.deposit);
                            //     }
                            // }
                            ],
                        "headerCallback": function (thead, data, start, end, display) {
                            try {
                                var rawServerResponse = this.api().settings()[0].rawServerResponse;
                                if (rawServerResponse !== undefined && rawServerResponse.totalData !== undefined) {
                                    $(thead).find('th').eq(1).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.amount));
                                }
                            } catch (e) {
                                console.log("không có total")
                            }
                        }
                    }
                },
                {
                    targets: 2,
                    data: "deposit.status",
                    name: "status",
                    render: function (status) {
                        return app.localize('Enum_DepositStatus_' + status);
                    }

                },
                {
                    targets: 3,
                    data: "agentName",
                    name: "agentName"
                },
                {
                    targets: 4,
                    data: "deposit.amount",
                    name: "amount",
                    render: function (data) {
                        return data ? Sv.format_number(data) : "0"
                    }
                },
                {
                    targets: 5,
                    data: "deposit.type",
                    name: "type",
                    render: function (status) {
                        return app.localize('Enum_DepositType_' + status);
                    }
                },
                {
                    targets: 6,
                    data: null,
                    render: function (data, type, row) {
                        if (data.deposit.recipientInfo != null) {
                            return data.deposit.recipientInfo;
                        } else if (data.bankBankName != null) {
                            return data.bankBankName;
                        }
                    }
                },
                {
                    targets: 7,
                    data: "deposit.requestCode",
                    name: "requestCode"
                },
                {
                    targets: 8,
                    data: "deposit.transCode",
                    name: "transCode"
                },
                {
                    targets: 9,
                    data: "deposit.transCodeBank"
                },
                {
                    targets: 10,
                    data: "agentType",
                    name: "agentType",
                    render: function (agentType) {
                        return app.localize('Enum_AgentType_' + agentType);
                    }
                },
                {
                    targets: 11,
                    data: "saleLeader",
                    name: "saleLeader"
                },

                {
                    targets: 12,
                    data: "saleMan",
                    name: "saleMan"
                },
                {
                    targets: 13,
                    data: "deposit.creationTime",
                    name: "creationTime",
                    render: function (creationTime) {
                        if (creationTime) {
                            return moment(creationTime).format('L LTS');
                        }
                        return "";
                    }
                },
                {
                    targets: 14,
                    data: "deposit.approvedDate",
                    name: "approvedDate",
                    render: function (approvedDate) {
                        if (approvedDate) {
                            return moment(approvedDate).format('L LTS');
                        }
                        return "";
                    }
                },
                {
                    targets: 15,
                    data: "userName2",
                    name: "userName2",
                    render: function (data, type, row) {
                        if (row.deposit.status === 1 && row.deposit.requestCode != null && data == '') {
                            return 'Duyệt tự động';
                        } else {
                            return data;
                        }
                    }
                }
            ],
            "headerCallback": function (thead, data, start, end, display) {
                try {
                    var rawServerResponse = this.api().settings()[0].rawServerResponse;
                    if (rawServerResponse !== undefined && rawServerResponse.totalData !== undefined) {
                        $(thead).find('th').eq(1).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.deposit.amount));
                    }
                } catch (e) {
                    console.log("không có total")
                }
            }
        });

        function getDeposits() {
            dataTable.ajax.reload();
        }

        function deleteDeposit(deposit) {
            abp.message.confirm(
                'Bạn có chắc chắn muốn xóa giao dịch này không?',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _depositsService.delete({
                            id: deposit.id
                        }).done(function () {
                            getDeposits(true);
                            abp.message.success(app.localize('SuccessfullyDeleted'));
                        });
                    }
                }
            );
        }

        function cancel(deposit) {
            abp.message.confirm(
                'Bạn có chắc chắn muốn hủy giao dịch này không',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _depositsService.cancel({transCode: deposit.transCode}).done(function (rs) {
                            getDeposits(true);
                            abp.message.success("Hủy giao dịch thành công");
                        });
                    }
                }
            );
        }

        function approval(deposit) {
            abp.message.confirm(
                'Bạn có chắc chắn muốn duyệt giao dịch này không?',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _depositsService.approval({transCode: deposit.transCode}).done(function (rs) {
                            getDeposits(true);
                            abp.message.success("Duyệt giao dịch thành công");
                        });
                    }
                }
            );
        }

        function approvalBank(deposit) {
            swal({
                title: "Bạn chắc chắn muốn duyệt giao dịch này?",
                content: {
                    element: "input",
                    attributes: {
                        placeholder: "Vui lòng nhập Mã giao dịch ngân hàng (*)",
                        type: "text",
                    },
                },
                buttons: {
                    cancel: "Huỷ bỏ",
                    confirm: "Đồng ý",
                },
            }).then((input) => {
                if (input) {
                    _depositsService.approval({transCode: deposit.transCode, transCodeBank: input}).done(function (rs) {
                        getDeposits(true);
                        abp.message.success("Duyệt giao dịch thành công");
                    });
                } else {
                    swal("Lỗi", input);
                }
            });
        }

        $("#BankId").select2();
        $("#UserId").select2({
            placeholder: 'Chọn đại lý',
            allowClear: true,
            ajax: {
                url: abp.appPath + "api/services/app/CommonLookup/GetListUserSearch",
                dataType: 'json',
                delay: 250,
                data: function (params) {
                    return {
                        search: params.term, // search term
                        page: params.page,
                        agentType: $('#AgentTypeFilter').val() != null ? $('#AgentTypeFilter').val() : '',
                        //saleId: 0
                    };
                },
                processResults: function (data, params) {
                    params.page = params.page || 1;

                    return {
                        results: $.map(data.result, function (item) {
                            return {
                                text: item.accountCode + "-" + item.phoneNumber + "-" + item.fullName,
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

        $("#SaleLeaderFilter").select2({
            placeholder: 'Chọn trưởng nhóm KD',
            allowClear: true,
            ajax: {
                url: abp.appPath + "api/services/app/CommonLookup/GetUserSaleLeader",
                dataType: 'json',
                delay: 250,
                data: function (params) {
                    return {
                        search: params.term
                    };
                },
                processResults: function (data, params) {
                    params.page = params.page || 1;

                    return {
                        results: $.map(data.result, function (item) {
                            return {
                                text: item.userName + " - " + item.phoneNumber + " - " + item.fullName,
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

        $("#SaleEmpFilter").select2({
            placeholder: 'Chọn nhân viên KD',
            allowClear: true,
            ajax: {
                url: abp.appPath + "api/services/app/CommonLookup/GetUserSaleBySaleLeader",
                dataType: 'json',
                delay: 250,
                data: function (params) {
                    return {
                        search: params.term,
                        accountType: 99,
                        saleLeaderId: $("#SaleLeaderFilter").val() != null ? $("#SaleLeaderFilter").val() : '0'
                    };
                },
                processResults: function (data, params) {
                    params.page = params.page || 1;

                    return {
                        results: $.map(data.result, function (item) {
                            return {
                                text: item.userName + " - " + item.phoneNumber + " - " + item.fullName,
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

        $('#CreateNewDepositButton').click(function () {
            _createOrEditModal.open();
        });

        $('#CreateNewDepositCashButton').click(function () {
            _createOrEditDepositCashModal.open();
        });

        $('#CreateNewDebtSaleButton').click(function () {
            _createDebtModal.open();
        });

        $('#CreateNewAccountingEntryButton').click(function () {
            _createOrEditAccountingEntryModal.open();
        });

        $('#ExportToExcelButton').click(function () {
            _depositsService
                .getDepositsToExcel({
                    filter: $('#DepositsTableFilter').val(),
                    statusFilter: $('#StatusFilterId').val(),
                    minApprovedDateFilter: getDateFilter($('#MinApprovedDateFilterId')),
                    maxApprovedDateFilter: getToDateFilter($('#MaxApprovedDateFilterId')),
                    transCodeFilter: $('#TransCodeFilter').val(),
                    transCodeBankFilter: $('#TransCodeBankFilter').val(),
                    userId: $('#UserId').val(),
                    bankId: $('#BankId').val(),
                    depositTypeFilter: $('#DepositTypeFilter').val(),
                    agentTypeFilter: $('#AgentTypeFilter').val(),
                    saleLeadFilter: $('#SaleLeaderFilter').val(),
                    saleManFilter: $('#SaleEmpFilter').val(),
                    requestCodeFilter: $('#RequestCodeDepositFilter').val(),
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditDepositModalSaved', function () {
            getDeposits();
        });

        abp.event.on('app.createOrEditAccountingEntryModalSaved', function () {
            getDeposits();
        });

        $('#GetDepositsButton').click(function (e) {
            e.preventDefault();
            getDeposits();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getDeposits();
            }
        });

        // function getAllBank() {
        //     _banksService.getAll().done(function (obj) {
        //         $.each(obj.items, function (key, value) {
        //             $('#BankBankNameFilterId')
        //                 .append($("<option></option>")
        //                     .attr("value", value.bank.servicesName)
        //                     .text(value.bank.servicesName));
        //         });
        //     }).always(function () {
        //
        //     });
        // }
    });
})();
