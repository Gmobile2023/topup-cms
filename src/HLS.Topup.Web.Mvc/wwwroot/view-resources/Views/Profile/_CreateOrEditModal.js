﻿(function ($) {
    app.modals.CreateOrEditUserModal = function () {
        let _modalManager;
        let _$userInformationForm = null;
        //var _accountService = abp.services.app.account;
        const _profileService = abp.services.app.profile;
        const _passwordComplexityHelper = new app.PasswordComplexityHelper();
        this.init = function (modalManager) {
            _modalManager = modalManager;

            const modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            //_$userInformationForm = _modalManager.getModal().find('form[name=UserInformationsForm]');

            _profileService.getPasswordComplexitySetting().done(function (result) {
                _$userInformationForm = _modalManager.getModal().find('form[name=UserInformationsForm]');
                _$userInformationForm.validate();

                _passwordComplexityHelper.setPasswordComplexityRules(_$userInformationForm.find("input[name=Password],input[name=PasswordRepeat]"), result.setting);
            });
            //_$userInformationForm.validate();
            Sv.SetupAmountMask();
        };

        $("#btnChangePass").click(function () {
            console.log('test');
            if ($("#showChangePassword").attr('class') === "hidden") {
                $("#showChangePassword").removeClass('hidden').addClass('show');
            } else {
                $("#showChangePassword").removeClass('show').addClass('hidden');
            }
        });
        this.save = function () {
            if (!_$userInformationForm.valid()) {
                return;
            }

            var user = _$userInformationForm.serializeFormToObject();
            _modalManager.setBusy(true);
            _profileService.updateCurrentUserProfile(user).done(function (rs) {
                abp.message.success(app.localize('SavedSuccessfully'));
                location.reload();
                _modalManager.close();
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);