(function () {
    $(function () {

        var _$providersTable = $('#ProvidersTable');
        var _providersService = abp.services.app.providers;

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Providers.Create'),
            edit: abp.auth.hasPermission('Pages.Providers.Edit'),
            lockUnLock: abp.auth.hasPermission('Pages.Providers.LockUnLock'),
            deposit: abp.auth.hasPermission('Pages.StocksAirtimes.Deposit'),
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

        var _depositAirtimeModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Providers/DepositAirtimeModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Providers/_DepositAirtimeModal.js',
            modalClass: 'DepositAirtimeModal'
        });


        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z");
        }

        var dataTable = _$providersTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _providersService.getAll,
                inputFilter: function () {
                    return {
                        filter: $('#ProvidersTableFilter').val(),
                        codeFilter: $('#CodeFilterId').val(),
                        nameFilter: $('#NameFilterId').val(),
                        providerTypeFilter: $('#ProviderTypeFilterId').val(),
                        providerStatusFilter: $('#ProviderStatusFilterId').val(),
                        parentProviderFilter: $('#ParentProviderFilter').val()
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
                                    _viewProviderModal.open({id: data.record.provider.id});
                                }
                            },
                            {
                                text: app.localize('Edit'),
                                visible: function () {
                                    return _permissions.edit;
                                },
                                action: function (data) {
                                    _createOrEditModal.open({id: data.record.provider.id});
                                }
                            },
                            {
                                text: app.localize('Lock'),
                                visible: function (data) {
                                    return _permissions.lockUnLock && data.record.provider.providerStatus === 1;
                                },
                                action: function (data) {
                                    lock(data.record.provider);
                                }
                            },
                            {
                                text: app.localize('UnLock'),
                                visible: function (data) {
                                    return _permissions.lockUnLock && data.record.provider.providerStatus === 2;
                                },
                                action: function (data) {
                                    unlock(data.record.provider);
                                }
                            },
                            {
                                text: app.localize('Query'),
                                action: function (data) {
                                    abp.ui.setBusy();
                                    abp.services.app.stocksAirtimes.query(data.record.provider.code)
                                        .done(function (data) {
                                            abp.ui.clearBusy();
                                            if (data.responseCode === "1") {
                                                abp.message.success('Số dư thực tế: ' + Sv.NumberToString(data.payload));
                                            } else {
                                                abp.message.error(data.responseMessage);
                                            }
                                        }).catch(function () {
                                        abp.ui.clearBusy();
                                    });
                                }
                            },
                            {
                                text: app.localize('Nhập số dư'),
                                visible: function (data) {
                                    return _permissions.deposit;
                                },
                                action: function (data) {
                                    _depositAirtimeModal.open({keyCode: data.record.provider.code, providerCode: data.record.provider.code});
                                }
                            },
                            {
                                text: app.localize('Delete'),
                                visible: function () {
                                    return _permissions.delete;
                                },
                                action: function (data) {
                                    deleteProvider(data.record.provider);
                                }
                            }]
                    }
                },
                {
                    targets: 2,
                    data: "provider.code",
                    name: "code"
                },
                {
                    targets: 3,
                    data: "provider.parentProvider",
                    name: "code"
                },
                {
                    targets: 4,
                    data: "provider.name",
                    name: "name"
                },
                {
                    targets: 5,
                    data: "provider.phoneNumber",
                    name: "phoneNumber"
                },
                // {
                // 	targets: 5,
                // 	 data: "provider.providerType",
                // 	 name: "providerType"   ,
                // 	render: function (providerType) {
                // 		return app.localize('Enum_ProviderType_' + providerType);
                // 	}
                //
                // },
                {
                    targets: 6,
                    data: "provider.providerStatus",
                    name: "providerStatus",
                    render: function (providerStatus) {
                        return app.localize('Enum_ProviderStatus_' + providerStatus);
                    }

                }
            ]
        });

        function getProviders() {
            dataTable.ajax.reload();
        }

        function deleteProvider(provider) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _providersService.delete({
                            id: provider.id
                        }).done(function () {
                            getProviders(true);
                            abp.notify.success(app.localize('SuccessfullyDeleted'));
                        });
                    }
                }
            );
        }

        function lock(provider) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _providersService.lock({
                            id: provider.code
                        }).done(function () {
                            getProviders(true);
                            abp.notify.success('Thành công');
                        });
                    }
                }
            );
        }

        function unlock(provider) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _providersService.unLock({
                            id: provider.code
                        }).done(function () {
                            getProviders(true);
                            abp.notify.success('Thành công');
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

        $('#CreateNewProviderButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportToExcelButton').click(function () {
            _providersService
                .getProvidersToExcel({
                    filter: $('#ProvidersTableFilter').val(),
                    codeFilter: $('#CodeFilterId').val(),
                    nameFilter: $('#NameFilterId').val(),
                    providerTypeFilter: $('#ProviderTypeFilterId').val(),
                    providerStatusFilter: $('#ProviderStatusFilterId').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditProviderModalSaved', function () {
            getProviders();
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
    });
})();
