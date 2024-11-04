(function ($) {
    app.modals.CreateOrEditLowBalanceAlertModal = function () {
        var _lowBalanceAlertsService = abp.services.app.lowBalanceAlerts;

        var _modalManager;
        var _lowBalanceAlertInformationForm = null;
        
        this.init = function (modalManager) {
            _modalManager = modalManager;

            var modal = _modalManager.getModal();
            
            Sv.SetupAmountMask();

            $('.no-special-characters').keypress(function(e){
                let txt = String.fromCharCode(e.which)
                let pattern = /^[0-9\-]+$/;
                
                if (!(pattern.test(txt) || e.keyCode == 8)){
                    $(this).val($(this).val().slice(0, -1));
                }
            });
            
            _lowBalanceAlertInformationForm = _modalManager.getModal().find('form[name=LowBalanceAlertInformationsForm]');
            _lowBalanceAlertInformationForm.validate();
        };
        
        this.save = function () {
            if (!_lowBalanceAlertInformationForm.valid()) {
                return;
            }

            var lowBalanceAlert = _lowBalanceAlertInformationForm.serializeFormToObject();
            lowBalanceAlert.isRun = lowBalanceAlert.isRun * 1;

            _modalManager.setBusy(true);
            _lowBalanceAlertsService.createOrEdit(
                lowBalanceAlert
            ).done(function () {
                abp.message.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditLowBalanceAlertModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);
