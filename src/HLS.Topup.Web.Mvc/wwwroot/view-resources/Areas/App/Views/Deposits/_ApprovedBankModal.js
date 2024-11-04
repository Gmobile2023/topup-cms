(function ($) {
    app.modals.ApprovedBankModal = function () {
        let _depositsService = abp.services.app.deposits;

        let _modalManager;
        let _$depositInformationForm = null;
        
        this.init = function (modalManager) {
            _modalManager = modalManager;

            let modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$depositInformationForm = _modalManager.getModal().find('form[name=ApprovedBankInformationsForm]');
            _$depositInformationForm.validate();
        };

        $('.approved-button').click(function() {
            if (!_$depositInformationForm.valid()) {
                return;
            }

            if ($('#txtTransCodeBank').prop('required') && $('#txtTransCodeBank').val() == '') {
                abp.message.error('Vui lòng nhập mã giao dịch ngân hàng!');
                return;
            }

            let deposit = _$depositInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            
            _depositsService.approval(
                deposit
            ).done(function (rs) {
                
                abp.message.info(app.localize('Duyệt thành công!'));
                $('#DepositsTable').DataTable().ajax.reload();
                _modalManager.close();
                
            }).always(function () {
                _modalManager.setBusy(false);
            });
        });
    };
})(jQuery);
