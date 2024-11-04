(function () {
    $(function () {

        var _$wardsTable = $('#WardsTable');
        var _wardsService = abp.services.app.wards;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Wards.Create'),
            edit: abp.auth.hasPermission('Pages.Wards.Edit'),
            'delete': abp.auth.hasPermission('Pages.Wards.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Wards/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Wards/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditWardModal'
        });       

		 var _viewWardModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Wards/ViewwardModal',
            modalClass: 'ViewWardModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$wardsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _wardsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#WardsTableFilter').val(),
					wardCodeFilter: $('#WardCodeFilterId').val(),
					wardNameFilter: $('#WardNameFilterId').val(),
					statusFilter: $('#StatusFilterId').val(),
					districtDistrictNameFilter: $('#DistrictDistrictNameFilterId').val()
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
                                    _viewWardModal.open({ id: data.record.ward.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.ward.id });                                
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteWard(data.record.ward);
                            }
                        }]
                    }
                },
					{
						targets: 2,
						 data: "ward.wardCode",
						 name: "wardCode"   
					},
					{
						targets: 3,
						 data: "ward.wardName",
						 name: "wardName"   
					},
					{
						targets: 4,
						 data: "ward.status",
						 name: "status"   ,
						render: function (status) {
							return app.localize('Enum_WardStatus_' + status);
						}
			
					},
					{
						targets: 5,
						 data: "districtDistrictName" ,
						 name: "districtFk.districtName" 
					}
            ]
        });

        function getWards() {
            dataTable.ajax.reload();
        }

        function deleteWard(ward) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _wardsService.delete({
                            id: ward.id
                        }).done(function () {
                            getWards(true);
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

        $('#CreateNewWardButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _wardsService
                .getWardsToExcel({
				filter : $('#WardsTableFilter').val(),
					wardCodeFilter: $('#WardCodeFilterId').val(),
					wardNameFilter: $('#WardNameFilterId').val(),
					statusFilter: $('#StatusFilterId').val(),
					districtDistrictNameFilter: $('#DistrictDistrictNameFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditWardModalSaved', function () {
            getWards();
        });

		$('#GetWardsButton').click(function (e) {
            e.preventDefault();
            getWards();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getWards();
		  }
		});
    });
})();