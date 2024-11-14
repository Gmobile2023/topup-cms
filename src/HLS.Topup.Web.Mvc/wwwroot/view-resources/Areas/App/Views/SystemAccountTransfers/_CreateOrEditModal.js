(function ($) {
    app.modals.CreateOrEditSystemAccountTransferModal = function () {

        var _systemAccountTransfersService = abp.services.app.systemAccountTransfers;
        var _transactionService = abp.services.app.transactions;

        var _modalManager;
        var _$systemAccountTransferInformationForm = null;


        this.init = function (modalManager) {
            _modalManager = modalManager;

            var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$systemAccountTransferInformationForm = _modalManager.getModal().find('form[name=SystemAccountTransferInformationsForm]');
            _$systemAccountTransferInformationForm.validate();
            Sv.SetupAmountMask();
            $("#FileAttachment").on('change', function () {
                app.uploadFile($("#FileAttachment"), $('#FileAttachmentSrc'));
            });
        };


        $("#SystemAccountTransfer_Amount").on('keyup input', function (e) {
            const $element = $(this);
            const val = $element.val();
            const $str = $(".amount-to-text");
            Sv.BindMoneyToString($str, val);
        });
        $("#SystemAccountTransfer_SrcAccount").change(function () {
            if ($(this).val()) {
                _transactionService.getBalance({accountCode: $(this).val()}).done(function (rs) {
                    $("#txtSrcBalance").val(Sv.NumberToString(rs));
                });
            } else {
                $("#txtSrcBalance").val('');
            }
        }).trigger("change");
        $("#SystemAccountTransfer_DesAccount").change(function () {
            if ($(this).val()) {
                _transactionService.getBalance({accountCode: $(this).val()}).done(function (rs) {
                    $("#txtDesBalance").val(Sv.NumberToString(rs));
                });
            } else {
                $("#txtDesBalance").val('');
            }
        }).trigger("change");
        this.save = function () {
            if (!_$systemAccountTransferInformationForm.valid()) {
                return;
            }
            var check = $("#FileAttachmentSrc").val();
            if (check == null || check == "") {
                abp.message.info('Vui lòng chọn văn bản đính kèm');
                return;
            }
            var systemAccountTransfer = _$systemAccountTransferInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _systemAccountTransfersService.createOrEdit(
                systemAccountTransfer
            ).done(function () {
                abp.message.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditSystemAccountTransferModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);
