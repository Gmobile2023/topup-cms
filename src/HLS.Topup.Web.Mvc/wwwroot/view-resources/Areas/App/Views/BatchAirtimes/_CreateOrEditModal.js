(function ($) {
    app.modals.CreateOrEditBatchAirtimeModal = function () {

        var _batchAirtimesService = abp.services.app.batchAirtimes;

        var _modalManager;
        var _$batchAirtimeInformationForm = null;

		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });
            modal.find(".select2").select2();
            Sv.SetupAmountMask();

            _$batchAirtimeInformationForm = _modalManager.getModal().find('form[name=BatchAirtimeInformationsForm]');
            _$batchAirtimeInformationForm.validate();

            modal.find("[name='amount'],[name='discount']").on('change', calculatorAirtime);
            modal.find("[name='amount']").trigger('change');
            Sv.SetupAmountMask();
        };

		  

        this.save = function () {
            if (!_$batchAirtimeInformationForm.valid()) {
                return;
            }

            var batchAirtime = _$batchAirtimeInformationForm.serializeFormToObject();
			if(batchAirtime.providerCode == null ||batchAirtime.providerCode.length == 0){
                abp.message.error(app.localize('BatchAirtime_providerCode_valid'));
			    return false;
            }
			 _modalManager.setBusy(true);
			 _batchAirtimesService.createOrEdit(
				batchAirtime
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditBatchAirtimeModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
        
        function calculatorAirtime(){
            let modal = _modalManager.getModal();
            let amount =modal.find("[name='amount']").val();
            let discount =modal.find("[name='discount']").val(); 
            let airtime = parseFloat(amount) + (parseFloat(amount) * (parseFloat(discount) / 100)); 
            modal.find("[name='airtime']").val(Math.round(airtime));
        }
    };
})(jQuery);