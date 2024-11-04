(function () {
    $(function () {

        var _$servicesTable = $('#ServicesTable');
        var _servicesService = abp.services.app.services;

        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Services.Create'),
            edit: abp.auth.hasPermission('Pages.Services.Edit'),
            'delete': abp.auth.hasPermission('Pages.Services.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Services/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Services/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditServiceModal',
             modalSize:'modal-xl'
        });

		 var _viewServiceModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Services/ViewserviceModal',
            modalClass: 'ViewServiceModal',
             modalSize:'modal-xl size-80'
        });




        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z");
        }

        var dataTable = _$servicesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _servicesService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#ServicesTableFilter').val(),
					serviceCodeFilter: $('#ServiceCodeFilterId').val(),
					servicesNameFilter: $('#ServicesNameFilterId').val(),
					statusFilter: $('#StatusFilterId').val()
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
                                    _viewServiceModal.open({ id: data.record.service.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.service.id });
                            }
                        },
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteService(data.record.service);
                            }
                        }]
                    }
                },
					{
						targets: 2,
						 data: "service.serviceCode",
						 name: "serviceCode"
					},
					{
						targets: 3,
						 data: "service.servicesName",
						 name: "servicesName"
					},
					{
						targets: 4,
						 data: "service.status",
						 name: "status"   ,
						render: function (status) {
							return app.localize('Enum_ServiceStatus_' + status);
						}

					},
					{
						targets: 5,
						 data: "service.order",
						 name: "order"
					}
            ]
        });

        function getServices() {
            dataTable.ajax.reload();
        }

        function deleteService(service) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _servicesService.delete({
                            id: service.id
                        }).done(function () {
                            getServices(true);
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

        $('#CreateNewServiceButton').click(function () {
            _createOrEditModal.open();
        });

		$('#ExportToExcelButton').click(function () {
            _servicesService
                .getServicesToExcel({
				filter : $('#ServicesTableFilter').val(),
					serviceCodeFilter: $('#ServiceCodeFilterId').val(),
					servicesNameFilter: $('#ServicesNameFilterId').val(),
					statusFilter: $('#StatusFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditServiceModalSaved', function () {
            getServices();
        });

		$('#GetServicesButton').click(function (e) {
            e.preventDefault();
            getServices();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getServices();
		  }
		});
    });
})();
