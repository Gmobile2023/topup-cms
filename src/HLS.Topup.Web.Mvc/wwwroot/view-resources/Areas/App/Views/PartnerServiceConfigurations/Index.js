(function () {
    $(function () {

        var _$serviceConfigurationsTable = $('#ServiceConfigurationsTable');
        var _serviceConfigurationsService = abp.services.app.partnerServiceConfigurations;
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.PartnerServiceConfigurations.Create'),
            edit: abp.auth.hasPermission('Pages.PartnerServiceConfigurations.Edit'),
            'delete': abp.auth.hasPermission('Pages.PartnerServiceConfigurations.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/PartnerServiceConfigurations/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/PartnerServiceConfigurations/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditPartnerServiceConfigurationModal',
            modalSize: 'modal-xl'
        });

        var _viewServiceConfigurationModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/PartnerServiceConfigurations/ViewServiceConfigurationModal',
            modalClass: 'ViewServiceConfigurationModal',
            modalSize: 'modal-xl'
        });


        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z");
        }

        var dataTable = _$serviceConfigurationsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _serviceConfigurationsService.getAll,
                inputFilter: function () {
                    return {
                        filter: $('#ServiceConfigurationsTableFilter').val(),
                        nameFilter: $('#NameFilterId').val(),
                        statusFilter: $('#StatusFilter').val(),
                        serviceIds: $('#ServiceId').val() == "" ? "-1" : $('#ServiceId').val(),
                        categoryIds: $('#CategoryId').val() == "" ? "-1" : $('#CategoryId').val(),
                        providerId: $('#ProviderId').val(),
                        userId: $('#UserId').val()
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
                                    _viewServiceConfigurationModal.open({id: data.record.serviceConfiguration.id});
                                }
                            },
                            {
                                text: app.localize('Edit'),
                                visible: function () {
                                    return _permissions.edit;
                                },
                                action: function (data) {
                                    _createOrEditModal.open({id: data.record.serviceConfiguration.id});
                                }
                            },
                            {
                                text: app.localize('Delete'),
                                visible: function () {
                                    return _permissions.delete;
                                },
                                action: function (data) {
                                    deleteServiceConfiguration(data.record.serviceConfiguration);
                                }
                            }]
                    }
                },
                {
                    targets: 2,
                    data: "serviceConfiguration.name",
                    name: "name"
                },
                {
                    targets: 3,
                    data: "agentType",
                    name: "userFk.agentType"
                }, {
                    targets: 4,
                    data: "userName",
                    name: "userFk.name"
                },
                {
                    targets: 5,
                    data: "serviceServicesName",
                    name: "serviceFk.servicesName"
                },
                {
                    targets: 6,
                    data: "categoryCategoryName",
                    name: "categoryFk.categoryName"
                },
                {
                    targets: 7,
                    data: "providerName",
                    name: "providerFk.name"
                },
                {
                    targets: 8,
                    data: "serviceConfiguration.status",
                    name: "status",
                    render: function (status) {
                        return app.localize('Enum_PartnerServiceConfigurationStatus_' + status);
                    }
                },
                {
                    targets: 9,
                    data: "serviceConfiguration.description",
                    name: "description"
                },
            ]
        });

        function getServiceConfigurations() {
            dataTable.ajax.reload();
        }

        function deleteServiceConfiguration(serviceConfiguration) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _serviceConfigurationsService.delete({
                            id: serviceConfiguration.id
                        }).done(function () {
                            getServiceConfigurations(true);
                            abp.message.success(app.localize('SuccessfullyDeleted'));
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

        $('#CreateNewServiceConfigurationButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportToExcelButton').click(function () {
            _serviceConfigurationsService
                .getPartnerServiceConfigurationsToExcel({
                    filter: $('#ServiceConfigurationsTableFilter').val(),
                    nameFilter: $('#NameFilterId').val(),
                    statusFilter: $('#StatusFilter').val(),
                    providerId: $('#ProviderId').val(),
                    serviceIds: $('#ServiceId').val() == "" ? "-1" : $('#ServiceId').val(),
                    categoryIds: $('#CategoryId').val() == "" ? "-1" : $('#CategoryId').val(),
                    userId: $('#UserId').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        var userSelect2 = $("#UserId").select2({
            placeholder: 'Select',
            allowClear: true,
            ajax: {
                url: abp.appPath + "api/services/app/CommonLookup/GetListUserSearch",
                dataType: 'json',
                delay: 250,
                data: function (params) {
                    return {
                        search: params.term, // search term
                        page: params.page,
                        agentType: $('#Discount_AgentType option:selected').val(),
                        accountType: 99
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

        $("#ServiceId").change(function (e) {
            let serviceId = $(e.target).val();
            if (serviceId == "8")
                serviceId = "4";
            if (serviceId == null || serviceId == "") serviceId = "0";
            Sv.GetCateTwoByService(serviceId, $("#CategoryId"), false);
        });

        $("#CategoryId").change(function (e) {
            let cateId = $(e.target).val();
            if (cateId == null || cateId == "") cateId = "0";
            Sv.GetProductTwoByCate(cateId, $("#ProductId"), false);
        });

        abp.event.on('app.CreateOrEditPartnerServiceConfigurationModalSaved', function () {
            getServiceConfigurations();
        });

        $('#GetServiceConfigurationsButton').click(function (e) {
            e.preventDefault();
            getServiceConfigurations();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getServiceConfigurations();
            }
        });
    });
})();
