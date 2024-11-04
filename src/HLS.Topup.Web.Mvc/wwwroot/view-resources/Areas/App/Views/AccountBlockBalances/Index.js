(function () {
    $(function () {

        var _$accountBlockBalancesTable = $('#AccountBlockBalancesTable');
        var _accountBlockBalancesService = abp.services.app.accountBlockBalances;

        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.AccountBlockBalances.Create'),
            edit: abp.auth.hasPermission('Pages.AccountBlockBalances.Edit'),
            'delete': abp.auth.hasPermission('Pages.AccountBlockBalances.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/AccountBlockBalances/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/AccountBlockBalances/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditAccountBlockBalanceModal',
            modalSize: 'modal-xl'
        });

        var _viewAccountBlockBalanceModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/AccountBlockBalances/ViewaccountBlockBalanceModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/AccountBlockBalances/_ViewModal.js',
            modalClass: 'ViewAccountBlockBalanceModal',
            modalSize: 'modal-xl'
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

        var dataTable = _$accountBlockBalancesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _accountBlockBalancesService.getAll,
                inputFilter: function () {
                    return {
                        filter: $('#AccountBlockBalancesTableFilter').val(),
                        transCodeFilter: $('#TransCodeFilterId').val(),
                        userIdFilter: $('#UserNameFilterId').val(),
                        fromCreatedTimeFilter: getDateFilter($('#FromCreatedTimeFilter')),
                        toCreatedTimeFilter: getToDateFilter($('#ToCreatedTimeFilter')),
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
                                    _viewAccountBlockBalanceModal.open({id: data.record.accountBlockBalance.id});
                                }
                            },
                            // {
                            //     text: app.localize('Edit'),
                            //     visible: function () {
                            //         return _permissions.edit;
                            //     },
                            //     action: function (data) {
                            //         _createOrEditModal.open({id: data.record.accountBlockBalance.id});
                            //     }
                            // },
                            {
                                text: app.localize('Delete'),
                                visible: function () {
                                    return _permissions.delete;
                                },
                                action: function (data) {
                                    deleteAccountBlockBalance(data.record.accountBlockBalance);
                                }
                            }]
                    }
                },
                {
                    targets: 2,
                    data: "agentType",
                    name: "agentType",
                    render: function (type) {
                        return app.localize('Enum_AgentType_' + type);
                    }
                },
                {
                    targets: 3,
                    data: "fullAgentName",
                    name: "fullAgentName"
                },
                {
                    targets: 4,
                    data: "accountBlockBalance.blockedMoney",
                    name: "blockedMoney",
                    render: function (blockedMoney) {
                        return Sv.NumberToString(blockedMoney);
                    }
                },
                {
                    targets: 5,
                    data: "accountBlockBalance.lastModificationTime",
                    name: "lastModificationTime",
                    render: function (lastModificationTime) {
                        if (lastModificationTime) {
                            return moment(new Date(lastModificationTime)).format('DD/MM/YYYY HH:mm:ss');
                        }
                        return "";
                    }
                },
                {
                    targets: 6,
                    data: "userName",
                    name: "userName"
                }
            ]
        });

        function getAccountBlockBalances() {
            dataTable.ajax.reload();
        }

        function deleteAccountBlockBalance(accountBlockBalance) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _accountBlockBalancesService.delete({
                            id: accountBlockBalance.id
                        }).done(function () {
                            getAccountBlockBalances(true);
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

        $('#CreateNewAccountBlockBalanceButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportToExcelButton').click(function () {
            _accountBlockBalancesService
                .getAccountBlockBalancesToExcel({
                    filter: $('#AccountBlockBalancesTableFilter').val(),
                    transCodeFilter: $('#TransCodeFilterId').val(),
                    userIdFilter: $('#UserNameFilterId').val(),
                    fromCreatedTimeFilter: getDateFilter($('#FromCreatedTimeFilter')),
                    toCreatedTimeFilter: getToDateFilter($('#ToCreatedTimeFilter')),
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $("#UserNameFilterId").select2({
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
                        agentType: 99
                    };
                },
                processResults: function (data, params) {
                    params.page = params.page || 1;

                    return {
                        results: $.map(data.result, function (item) {
                            return {
                                text: item.accountCode + " - " + item.phoneNumber + " - " + item.fullName,
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

        abp.event.on('app.createOrEditAccountBlockBalanceModalSaved', function () {
            getAccountBlockBalances();
        });

        $('#GetAccountBlockBalancesButton').click(function (e) {
            e.preventDefault();
            getAccountBlockBalances();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getAccountBlockBalances();
            }
        });
    });
})();
