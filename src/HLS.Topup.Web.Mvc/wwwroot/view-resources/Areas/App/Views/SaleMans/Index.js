(function () {
    $(function () {

        var _$saleMansTable = $('#SaleMansTable');
        var _saleMansService = abp.services.app.saleMans;

        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.SaleMans.Create'),
            edit: abp.auth.hasPermission('Pages.SaleMans.Edit'),
            'delete': abp.auth.hasPermission('Pages.SaleMans.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/SaleMans/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/SaleMans/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditSaleManModal',
            modalSize: 'modal-xl'
        });

        var _viewSaleManModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/SaleMans/ViewSaleManModal',
            modalClass: 'ViewSaleManModal',
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

        var dataTable = _$saleMansTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _saleMansService.getAll,
                inputFilter: function () {
                    return {
                        status: $('#StatusFilter').val() != "" ? $('#StatusFilter').val() == 1 : "",
                        userNameFilter: $('#UserNameFilter').val(),
                        phoneNumberFilter: $('#PhoneNumberFilter').val(),
                        saleLeaderFilter: $('#SaleLeaderFilter').val(),
                        fromDateFilter: getDateFilter($('#FromDateFilter')),
                        toDateFilter: getToDateFilter($('#ToDateFilter')),
                        fullNameFilter: $('#FullNameFilter').val(),
                        saleTypeFilter: $('#SaleTypeFilter').val()
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
                                    _viewSaleManModal.open({id: data.record.saleMan.id});
                                }
                            },
                            {
                                text: app.localize('Edit'),
                                visible: function () {
                                    return _permissions.edit;
                                },
                                action: function (data) {
                                    _createOrEditModal.open({id: data.record.saleMan.id});
                                }
                            },
                            {
                                text: app.localize('Delete'),
                                visible: function () {
                                    return _permissions.delete;
                                },
                                action: function (data) {
                                    deleteSaleMan(data.record.saleMan);
                                }
                            }]
                    }
                },
                {
                    targets: 2,
                    data: "saleMan.userName",
                    name: "userName"
                },
                {
                    targets: 3,
                    data: null,
                    name: "saleName",
                    render: function (data, type, row, meta) {
                        return row.saleMan.fullName;
                    }
                },

                {
                    targets: 4,
                    data: "saleMan.phoneNumber",
                    name: "phoneNumber",
                },
                {
                    targets: 5,
                    data: null,
                    name: "saleType",
                    render: function (data, type, row, meta) {
                        return app.localize('Enum_SaleManType_' + row.saleMan.accountType);
                    }
                },
                {
                    targets: 6,
                    data: "saleMan.saleLeadName",
                    name: "saleLeadName"
                },
                {
                    targets: 7,
                    data: null,
                    name: "saleCreatedAt",
                    render: function (data, type, row, meta) {
                        return moment(row.saleMan.creationTime).format('DD/MM/YYYY HH:mm:ss');
                    }
                },
                {
                    targets: 8,
                    data: null,
                    name: "isActive",
                    render: function (data, type, row, meta) {
                        return app.localize('Enum_SaleManStatus_' + Number(row.saleMan.isActive));
                    }
                }
            ]
        });

        function getSaleMans() {
            dataTable.ajax.reload();
        }

        function deleteSaleMan(saleMan) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _saleMansService.delete({
                            id: saleMan.id
                        }).done(function () {
                            getSaleMans(true);
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

        $('#CreateNewSaleManButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportToExcelButton').click(function () {
            _saleMansService
                .getSaleMansToExcel({
                    userNameFilter: $('#UserNameFilter').val(),
                    phoneNumberFilter: $('#PhoneNumberFilter').val(),
                    saleLeaderFilter: $('#SaleLeaderFilter').val(),
                    fromDateFilter: getDateFilter($('#FromDateFilter')),
                    toDateFilter: getToDateFilter($('#ToDateFilter')),
                    fullNameFilter: $('#FullNameFilter').val(),
                    saleTypeFilter: $('#SaleTypeFilter').val(),
                    statusFilter: $('#StatusFilter').val() === 1
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditSaleManModalSaved', function () {
            getSaleMans();
        });

        $('#GetSaleMansButton').click(function (e) {
            e.preventDefault();
            getSaleMans();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getSaleMans();
            }
        });

        $("#SaleLeaderFilter").select2({
            placeholder: 'Tất cả',
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
    });
})();