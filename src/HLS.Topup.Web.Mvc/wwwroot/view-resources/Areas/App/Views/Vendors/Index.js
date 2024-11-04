(function () {
    $(function () {

        var _$vendorsTable = $('#VendorsTable');
        var _vendorsService = abp.services.app.vendors;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Vendors.Create'),
            edit: abp.auth.hasPermission('Pages.Vendors.Edit'),
            'delete': abp.auth.hasPermission('Pages.Vendors.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Vendors/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Vendors/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditVendorModal'
        });       

		 var _viewVendorModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Vendors/ViewvendorModal',
            modalClass: 'ViewVendorModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$vendorsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _vendorsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#VendorsTableFilter').val(),
					codeFilter: $('#CodeFilterId').val(),
					nameFilter: $('#NameFilterId').val(),
					descriptionFilter: $('#DescriptionFilterId').val(),
					print_HelpFilter: $('#Print_HelpFilterId').val(),
					print_SuportFilter: $('#Print_SuportFilterId').val(),
					minStatusFilter: $('#MinStatusFilterId').val(),
					maxStatusFilter: $('#MaxStatusFilterId').val(),
					addressFilter: $('#AddressFilterId').val(),
					hotLineFilter: $('#HotLineFilterId').val()
                    };
                }
            },
            columnDefs: [
                {
                    width: 120,
                    targets: 0,
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
                                    _viewVendorModal.open({ id: data.record.vendor.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.vendor.id });                                
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteVendor(data.record.vendor);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "vendor.code",
						 name: "code"   
					},
					{
						targets: 2,
						 data: "vendor.name",
						 name: "name"   
					},
					{
						targets: 3,
						 data: "vendor.description",
						 name: "description"   
					},
					{
						targets: 4,
						 data: "vendor.print_Help",
						 name: "print_Help"   
					},
					{
						targets: 5,
						 data: "vendor.print_Suport",
						 name: "print_Suport"   
					},
					{
						targets: 6,
						 data: "vendor.status",
						 name: "status"   
					},
					{
						targets: 7,
						 data: "vendor.address",
						 name: "address"   
					},
					{
						targets: 8,
						 data: "vendor.hotLine",
						 name: "hotLine"   
					}
            ]
        });

        function getVendors() {
            dataTable.ajax.reload();
        }

        function deleteVendor(vendor) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _vendorsService.delete({
                            id: vendor.id
                        }).done(function () {
                            getVendors(true);
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

        $('#CreateNewVendorButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _vendorsService
                .getVendorsToExcel({
				filter : $('#VendorsTableFilter').val(),
					codeFilter: $('#CodeFilterId').val(),
					nameFilter: $('#NameFilterId').val(),
					descriptionFilter: $('#DescriptionFilterId').val(),
					print_HelpFilter: $('#Print_HelpFilterId').val(),
					print_SuportFilter: $('#Print_SuportFilterId').val(),
					minStatusFilter: $('#MinStatusFilterId').val(),
					maxStatusFilter: $('#MaxStatusFilterId').val(),
					addressFilter: $('#AddressFilterId').val(),
					hotLineFilter: $('#HotLineFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditVendorModalSaved', function () {
            getVendors();
        });

		$('#GetVendorsButton').click(function (e) {
            e.preventDefault();
            getVendors();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getVendors();
		  }
		});
    });
})();