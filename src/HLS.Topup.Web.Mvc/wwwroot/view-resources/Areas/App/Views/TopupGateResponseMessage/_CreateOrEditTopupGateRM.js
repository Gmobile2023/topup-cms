(function ($){
    app.modals.CrateOrEditTopupGateRM = function(){
        var _TopupGateResponseMessageAppService = abp.services.app.topupGateResponseMessage;
        var _modalManager;
        var _informationForm = null
        this.init = function (modalManager){
            _modalManager = modalManager;
            var modal = _modalManager.getModal();
            modal.find('.select2').select2();
            _$informationForm = _modalManager.getModal().find('form[name = TopupGateMessageResponseInformationsForm]');
            _$informationForm.validate();
        };
        this.save = function(){
            if(!_$informationForm.valid()){
                return;
            }
            var data = _$informationForm.serializeFormToObject();
            console.log(data);
            _modalManager.setBusy(true);
            _TopupGateResponseMessageAppService.createOrEditTopupGateResponseMessage(data).done(function(){
                abp.message.info((app.localize('SavedSuccessfully')));
                _modalManager.close();
                abp.event.trigger('app.CreateOrEditTopupGateResponseMessage');
            }).always(function (){
                _modalManager.setBusy(false);
            })
        }
    }
})(jQuery);