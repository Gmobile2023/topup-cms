﻿(function () {
    $(function () {
        const _viettelService = abp.services.app.viettel;
        const _$form = $("#Form");
        _$form.validate();

        function resetCapChar() {

            abp.ui.setBusy();
            _viettelService.getCaptchaRequest().done(function (rs) {
                if (rs.responseCode === "01") {
                    $("#txtSid").val(rs.payload.sid);
                    $("#txtSid1").val(rs.payload.sid);
                    $("#captcha").attr("src", rs.payload.url);
                } else {
                    abp.notify.error(rs.responseMessage);
                }
            }).always(function () {
                abp.ui.clearBusy();
            });
        }


        $("#btnSentOtp").click(function () {
            const phone = $("#txtPhoneNumber").val();
            if (phone === null || phone === undefined) {
                abp.notify.warn('Chưa nhập số điện thoại cần đăng ký');
                return;
            }
            //var user = _$userInformationForm.serializeFormToObject();
            abp.ui.setBusy();
            _viettelService.getOtpRequest(phone, false).done(function (rs) {
                if (rs.responseCode === "01") {
                    resetCapChar();
                    $("#box-info").removeClass("hidden").addClass("show");
                } else {
                    abp.notify.error(rs.responseMessage);
                }
            }).always(function () {
                abp.ui.clearBusy();
            });

        });


        $("#btnRegister").click(function () {
            if (!_$form.valid()) {
                return;
            }
            var info = _$form.serializeFormToObject();
            info.CaptchaCode = $("#txtCaptcha").val();
            abp.ui.setBusy();
            _viettelService.registerMyViettelRequest(info).done(function (rs) {
                if (rs.responseCode === "01") {
                    abp.message.success("Đăng ký MyViettel thành công. Mật khẩu đăng nhập MyViettel là: " + rs.extraInfo);
                    //window.location.href = '/Topup/Index';
                } else {
                    abp.notify.error(rs.responseMessage);
                }
            }).always(function () {
                abp.ui.clearBusy();
            });

        });

        $("#btnResetCaptcha").click(function () {
            resetCapChar();
        });
    });
})();