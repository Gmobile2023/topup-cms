var CurrentPage = function () {

    var _passwordComplexityHelper = new app.PasswordComplexityHelper();

    var handleResetPassword = function() {

        $('.pass-reset-form').validate({
            rules: {
                PasswordRepeat: {
                    equalTo: "#Password"
                }
            },

            submitHandler: function(form) {
                form.submit();
            }
        });

        $('.pass-reset-form input').keypress(function(e) {
            if (e.which === 13) {
                if ($('.pass-reset-form').valid()) {
                    $('.pass-reset-form').submit();
                }

                return false;
            }
        });

        _passwordComplexityHelper.setPasswordComplexityRules($('input[name=Password],input[name=PasswordRepeat]'),
            window.passwordComplexitySetting);

        $(".btn-resend-code").click(function () {
            Dialog.sendCode({
                authen: false,
                phoneNumber: $("#txtPhoneNumber").val(),
                type: Dialog.otpType.ResetPass
            });
            Sv.ShowOtpResend($("#timeOtp"), Sv.OtpTimeOut);
            $("#show-resend-otp").addClass("hidden").removeClass("show");
            $("#show-text-otp").addClass("show").removeClass("hidden");
        });
        Sv.ShowOtpResend($("#timeOtp"), Sv.OtpTimeOut);
    };

    return {
        init: function () {
            handleResetPassword();
        }
    };

}();
