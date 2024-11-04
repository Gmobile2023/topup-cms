var CurrentPage = function () {

    jQuery.validator.addMethod("customUsername", function (value, element) {
        if (value === $('input[name="EmailAddress"]').val()) {
            return true;
        }

        return !$.validator.methods.email.apply(this, arguments);
    }, abp.localization.localize("RegisterFormUserNameInvalidMessage"));

    var _passwordComplexityHelper = new app.PasswordComplexityHelper();

    var handleRegister = function () {

        $('.register-form').validate({
            rules: {
                PasswordRepeat: {
                    equalTo: "#RegisterPassword"
                },
                UserName: {
                    required: true,
                    customUsername: true
                }
            },

            submitHandler: function (form) {
                form.submit();
            }
        });

        $('.register-form input').keypress(function (e) {
            if (e.which === 13) {
                if ($('.register-form').valid()) {
                    //$('.register-form').submit();
                    register();
                }
                return false;
            }
        });

        $("input[name=Password]").pwstrength({
            i18n: {
                t: function (key) {
                    return app.localize(key);
                }
            }
        });

        _passwordComplexityHelper.setPasswordComplexityRules($('input[name=Password], input[name=PasswordRepeat]'), window.passwordComplexitySetting);
    }

    return {
        init: function () {
            handleRegister();
        }
    };
}();
const register = function () {
    const isVerify = abp.setting.getBoolean("App.UserManagement.OtpSetting.IsUseVerifyRegister");
    if ($('.register-form').valid()) {
        abp.ui.setBusy();
        abp.services.app.account.validateAccount({
            PhoneNumber: $("#txtUserName").val(),
            IsSendCode: false
        }).done(function () {
            if ($('.register-form').valid()) {
                if (isVerify) {
                    Dialog.otpNone(Dialog.otpType.Register, $("#txtUserName").val(), function () {
                        //$("#txtOtp").val(otp);
                        $('.register-form').submit();
                    });
                } else {
                    $('.register-form').submit();
                }
            }
            // if (isVerify) {
            //     const isOdp = abp.setting.getBoolean("App.UserManagement.OtpSetting.IsUseOdpRegister");
            //     let title = "";
            //     if (isOdp === true) {
            //         const time = abp.setting.getInt("App.UserManagement.OtpSetting.OdpAvailable") / 60;
            //         title = app.localize("Message_ODP_Description", time);
            //     } else {
            //         const time = abp.setting.getInt("App.UserManagement.OtpSetting.OtpTimeOut");
            //         title = app.localize("Message_OTP_Description", time);
            //     }
            //     bootbox.prompt({
            //         title: 'Xác nhận ' + (isOdp ? "ODP" : "OTP" + ' để đăng ký tài khoản'),
            //         message: '<p>' + title + '</p>',//'<p>' + app.localize('Message_Confirm_Register', $("#txtUserName").val()) + '</p>'
            //         inputType: 'text',
            //         placeholder: 'Nhập ' + ((isOdp ? "ODP" : "OTP")),
            //         required: true,
            //         callback: function (otp) {
            //             if (otp !== null && otp !== undefined && otp !== 'null') {
            //                 $("#txtOtp").val(otp);
            //                 if ($('.register-form').valid()) {
            //                     $('.register-form').submit();
            //                 }
            //             }
            //         }
            //     });
            // } else {
            //     if ($('.register-form').valid()) {
            //         $('.register-form').submit();
            //     }
            // }
        }).always(function () {
            abp.ui.clearBusy();
        });
    }
};

$(document).ready(function () {
    $("#register-submit-btn").click(function () {
        register();
    });
});
