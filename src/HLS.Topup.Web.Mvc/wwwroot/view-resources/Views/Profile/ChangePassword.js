(function ($) {
    const _profileService = abp.services.app.profile;
    const _passwordComplexityHelper = new app.PasswordComplexityHelper();

    let _modalManager;
    let _$form = $("#changePassword");

    _profileService.getPasswordComplexitySetting().done(function (result) {
        _$form.validate();
        _passwordComplexityHelper.setPasswordComplexityRules(_$form.find("input[name=NewPassword],input[name=NewPasswordRepeat]"), result.setting);
    });

    $("#btn-Confirm").click(function () {

        if (!_$form.valid()) {
            return;
        }

        Sv.RequestStart();
        _profileService.changePassword(_$form.serializeFormToObject())
            .done(function () {
                abp.message.info(app.localize('YourPasswordHasChangedSuccessfully'));
                setTimeout(function () {
                    window.location.reload();
                }, 1000);
            }).always(function () {
            Sv.RequestEnd();
        });
    });
})(jQuery);
