(function ($) {
    app.modals.CreateOrEditDistrictModal = function () {

        var _districtsService = abp.services.app.districts;

        var _modalManager;
        var _$districtInformationForm = null;

		        var _DistrictcityLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Districts/CityLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Districts/_DistrictCityLookupTableModal.js',
            modalClass: 'CityLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$districtInformationForm = _modalManager.getModal().find('form[name=DistrictInformationsForm]');
            _$districtInformationForm.validate();
        };

		          $('#OpenCityLookupTableButton').click(function () {

            var district = _$districtInformationForm.serializeFormToObject();

            _DistrictcityLookupTableModal.open({ id: district.cityId, displayName: district.cityCityName }, function (data) {
                _$districtInformationForm.find('input[name=cityCityName]').val(data.displayName); 
                _$districtInformationForm.find('input[name=cityId]').val(data.id); 
            });
        });
		
		$('#ClearCityCityNameButton').click(function () {
                _$districtInformationForm.find('input[name=cityCityName]').val(''); 
                _$districtInformationForm.find('input[name=cityId]').val(''); 
        });
		


        this.save = function () {
            if (!_$districtInformationForm.valid()) {
                return;
            }
            if ($('#District_CityId').prop('required') && $('#District_CityId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('City')));
                return;
            }

            var district = _$districtInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _districtsService.createOrEdit(
				district
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditDistrictModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);