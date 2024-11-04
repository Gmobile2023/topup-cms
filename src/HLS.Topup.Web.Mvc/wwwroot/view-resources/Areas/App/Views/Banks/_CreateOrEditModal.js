(function ($) {
    app.modals.CreateOrEditBankModal = function () {

        var _banksService = abp.services.app.banks;

        var _modalManager;
        var _$bankInformationForm = null;



        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$bankInformationForm = _modalManager.getModal().find('form[name=BankInformationsForm]');
            _$bankInformationForm.validate();
            $("#imageId").on('change', function () {
                app.uploadImage($("#imageId"), $('#thumbImageId'));
            });
        };



        this.save = function () {
            if (!_$bankInformationForm.valid()) {
                return;
            }

            var bank = _$bankInformationForm.serializeFormToObject();
            var image="";
            if ($("#thumbImageId").attr("src") !== "") {
                image=$("#thumbImageId").attr("src");
            }
            bank.images=image;
			 _modalManager.setBusy(true);
			 _banksService.createOrEdit(
				bank
			 ).done(function () {
               abp.message.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditBankModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);
