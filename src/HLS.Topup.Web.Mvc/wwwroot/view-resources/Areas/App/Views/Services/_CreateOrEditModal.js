(function ($) {
    app.modals.CreateOrEditServiceModal = function () {

        var _servicesService = abp.services.app.services;

        var _modalManager;
        var _$serviceInformationForm = null;


        this.init = function (modalManager) {
            _modalManager = modalManager;

            var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });
            _$serviceInformationForm = _modalManager.getModal().find('form[name=ServiceInformationsForm]');
            _$serviceInformationForm.validate();
        };


        this.save = function () {
            if (!_$serviceInformationForm.valid()) {
                return;
            }

            var service = _$serviceInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _servicesService.createOrEdit(
                service
            ).done(function () {
                abp.message.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditServiceModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);
