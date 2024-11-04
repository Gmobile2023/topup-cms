(function ($) {
    app.modals.CreateOrEditWardModal = function () {

        var _wardsService = abp.services.app.wards;

        var _modalManager;
        var _$wardInformationForm = null;

		        var _WarddistrictLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Wards/DistrictLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Wards/_WardDistrictLookupTableModal.js',
            modalClass: 'DistrictLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$wardInformationForm = _modalManager.getModal().find('form[name=WardInformationsForm]');
            _$wardInformationForm.validate();
        };

		          $('#OpenDistrictLookupTableButton').click(function () {

            var ward = _$wardInformationForm.serializeFormToObject();

            _WarddistrictLookupTableModal.open({ id: ward.districtId, displayName: ward.districtDistrictName }, function (data) {
                _$wardInformationForm.find('input[name=districtDistrictName]').val(data.displayName); 
                _$wardInformationForm.find('input[name=districtId]').val(data.id); 
            });
        });
		
		$('#ClearDistrictDistrictNameButton').click(function () {
                _$wardInformationForm.find('input[name=districtDistrictName]').val(''); 
                _$wardInformationForm.find('input[name=districtId]').val(''); 
        });
		


        this.save = function () {
            if (!_$wardInformationForm.valid()) {
                return;
            }
            if ($('#Ward_DistrictId').prop('required') && $('#Ward_DistrictId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('District')));
                return;
            }

            var ward = _$wardInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _wardsService.createOrEdit(
				ward
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditWardModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);