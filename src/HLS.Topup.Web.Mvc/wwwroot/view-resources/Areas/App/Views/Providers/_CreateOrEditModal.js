(function ($) {
    app.modals.CreateOrEditProviderModal = function () {

        var _providersService = abp.services.app.providers;

        var _modalManager;
        var _$providerInformationForm = null;



        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });
            modal.find(".select2").select2();
            Sv.SetupAmountMask();
            _$providerInformationForm = _modalManager.getModal().find('form[name=ProviderInformationsForm]');
            _$providerInformationForm.validate();
            $("#imageId").on('change', function () {
                app.uploadImage($("#imageId"), $('#thumbImageId'));
            });
        };



        this.save = function () {
            if (!_$providerInformationForm.valid()) {
                return;
            }

            var provider = _$providerInformationForm.serializeFormToObject();
            var image="";
            if ($("#thumbImageId").attr("src") !== "") {
                image=$("#thumbImageId").attr("src");
            }
            provider.images=image;
            provider.providerUpdateInfo=_$providerInformationForm.serializeFormToObject();
			 _modalManager.setBusy(true);
			 _providersService.createOrEdit(
				provider
			 ).done(function () {
               abp.message.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditProviderModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);
