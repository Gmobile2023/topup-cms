(function () {
    $(function () {

        var _$citiesTable = $('#CitiesTable');
        var _citiesService = abp.services.app.cities;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Cities.Create'),
            edit: abp.auth.hasPermission('Pages.Cities.Edit'),
            'delete': abp.auth.hasPermission('Pages.Cities.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Cities/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Cities/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditCityModal'
        });       

		 var _viewCityModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Cities/ViewcityModal',
            modalClass: 'ViewCityModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$citiesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _citiesService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#CitiesTableFilter').val(),
					cityNameFilter: $('#CityNameFilterId').val(),
					statusFilter: $('#StatusFilterId').val(),
					countryCountryNameFilter: $('#CountryCountryNameFilterId').val()
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
                                    _viewCityModal.open({ id: data.record.city.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.city.id });                                
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteCity(data.record.city);
                            }
                        }]
                    }
                },
					{
						targets: 2,
						 data: "city.cityCode",
						 name: "cityCode"   
					},
					{
						targets: 3,
						 data: "city.cityName",
						 name: "cityName"   
					},
					{
						targets: 4,
						 data: "city.status",
						 name: "status"   ,
						render: function (status) {
							return app.localize('Enum_CityStatus_' + status);
						}
			
					},
					{
						targets: 5,
						 data: "countryCountryName" ,
						 name: "countryFk.countryName" 
					}
            ]
        });

        function getCities() {
            dataTable.ajax.reload();
        }

        function deleteCity(city) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _citiesService.delete({
                            id: city.id
                        }).done(function () {
                            getCities(true);
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

        $('#CreateNewCityButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _citiesService
                .getCitiesToExcel({
				filter : $('#CitiesTableFilter').val(),
					cityNameFilter: $('#CityNameFilterId').val(),
					statusFilter: $('#StatusFilterId').val(),
					countryCountryNameFilter: $('#CountryCountryNameFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditCityModalSaved', function () {
            getCities();
        });

		$('#GetCitiesButton').click(function (e) {
            e.preventDefault();
            getCities();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getCities();
		  }
		});
    });
})();