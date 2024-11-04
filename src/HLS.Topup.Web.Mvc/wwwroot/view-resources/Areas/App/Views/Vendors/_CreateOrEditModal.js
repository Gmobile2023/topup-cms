(function ($) {
    app.modals.CreateOrEditVendorModal = function () {

        var _vendorsService = abp.services.app.vendors;

        var _modalManager;
        var _$vendorInformationForm = null;

		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$vendorInformationForm = _modalManager.getModal().find('form[name=VendorInformationsForm]');
            _$vendorInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$vendorInformationForm.valid()) {
                return;
            }

            var vendor = _$vendorInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _vendorsService.createOrEdit(
				vendor
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditVendorModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);