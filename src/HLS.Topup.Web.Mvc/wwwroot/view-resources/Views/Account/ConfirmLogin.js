var CurrentPage = function () {
    const handleConfirmLogin = function () {

        $('#form-confirm-login').validate({
            submitHandler: function (form) {
                setLocalStorage('flagUserLogin', true);
                form.submit();
            }
        });

        $('.pass-reset-form input').keypress(function (e) {
            if (e.which === 13) {
                if ($('#form-confirm-login').valid()) {
                    setLocalStorage('flagUserLogin', true);
                    $('#form-confirm-login').submit();
                }
                return false;
            }
        });
        $(".btn-resend-code").click(function () {
            Dialog.sendCode({
                authen: false,
                phoneNumber: $("#txtPhoneNumber").val(),
                type: Dialog.otpType.Login
            });
            Sv.ShowOtpResend($("#timeOtp"), Sv.OtpTimeOut);
            $("#show-resend-otp").addClass("hidden").removeClass("show");
            $("#show-text-otp").addClass("show").removeClass("hidden");
        });
        Sv.ShowOtpResend($("#timeOtp"), Sv.OtpTimeOut);
    };

    return {
        init: function () {
            handleConfirmLogin();
        }
    };

}();
