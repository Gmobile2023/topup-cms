﻿(function ($) {
    app.modals.CreateOrEditModal = function () {

        var _userService = abp.services.app.user;

        var _modalManager;
        var _$userInformationForm = null;
        var _accountService = abp.services.app.account;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$userInformationForm = _modalManager.getModal().find('form[name=UserInformationsForm]');
            _$userInformationForm.validate();
            Sv.SetupAmountMask();
        };

        this.save = function () {
            if (!_$userInformationForm.valid()) {
                return;
            }

            var user = _$userInformationForm.serializeFormToObject();
            var objs = [];
            $('#tbl-discount-detail tr').each(function () {
                var input = $(this).find('input.discount-value');

                if (input.val() != undefined) {
                    var discount = "" + input.val() + "";
                    var obj = {
                        DiscountValue: discount.replace(/,/g, ''),//.toFixed(2).replace(/,/g, '.'),
                        ServiceId: input.attr("data-id"),
                        DiscountId: discount.id
                    };
                    objs.push(obj);
                }
            });
            user.DiscountDetail = objs;
            _modalManager.setBusy(true);
            _accountService.createOrEditAccount(user).done(function (rs) {
                if (rs.responseCode === "1") {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    _modalManager.close();
                    abp.event.trigger('app.createOrEditAgentModalSaved');
                }
                else {
                    abp.notify.error(rs.responseMessage);
                }
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);