(function ($) {
    app.modals.CreateOrEditStocksAirtimeModal = function () {

        var _stocksAirtimesService = abp.services.app.stocksAirtimes;

        var _modalManager;
        var _$stocksAirtimeInformationForm = null;

		

        this.init = function (modalManager) {
            _modalManager = modalManager;
			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });
            modal.find(".select2").select2(); 
            Sv.SetupAmountMask();
            
            _$stocksAirtimeInformationForm = _modalManager.getModal().find('form[name=StocksAirtimeInformationsForm]');
            _$stocksAirtimeInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$stocksAirtimeInformationForm.valid()) {
                return;
            }

            var stocksAirtime = _$stocksAirtimeInformationForm.serializeFormToObject();
            if(stocksAirtime.providerCode == null ||stocksAirtime.providerCode.length == 0){
                abp.message.error(app.localize('StocksAirtimes_providerCode_valid'));
                return false;
            }
            if(stocksAirtime.minLimitAirtime == null || stocksAirtime.minLimitAirtime.length == 0)
                stocksAirtime.minLimitAirtime = 0;
            if(stocksAirtime.maxLimitAirtime== null || stocksAirtime.maxLimitAirtime.length == 0)
                stocksAirtime.maxLimitAirtime = 0;
			 _modalManager.setBusy(true);
			 _stocksAirtimesService.createOrEdit(
				stocksAirtime
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditStocksAirtimeModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);