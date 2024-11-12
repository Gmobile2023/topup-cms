var ctrl = {
    page: $("#Profile_Password2_Page"),
    form: $("#profileForm"),
    getFormValue: function () {
        var obj = ctrl.form.serializeFormToObject();
        return obj;
    },
    valid: function () {
        var obj = ctrl.getFormValue();
        if (obj.password.length === 0) {
            abp.message.info("Vui lòng nhập mật khẩu cấp 2 mới");
            return false;
        }
        if (obj.password.length === 0) {
            abp.message.info("Vui lòng nhập lại mật khẩu cấp 2");
            return false;
        }
        if (obj.password !== obj.passwordConfirm) {
            abp.message.info("Vui lòng nhập đúng mật khẩu cấp 2 mới");
            return false;
        }
        if (obj.password.length < 6 || obj.passwordConfirm < 6) {
            abp.message.info("Vui lòng nhập mật khẩu chứa 6 kí tự");
            return false;
        }
        return true;
    },
    // Submit
    nextToStep: function () {
        if (!ctrl.valid()) {
            return false;
        }
        var obj = ctrl.getFormValue();
        Dialog.otp(Dialog.otpType.ChangePassLevel2, function (msg, data) {
            console.log(data);
            // var url = abp.appPath + 'api/services/app/Profile/ChangeLevel2Password' + abp.utils.buildQueryString([{
            //     name: 'password',
            //     value: obj.password
            // }, {name: 'otp', value: data.otp}]) + '';
            Sv.Api({
                url: abp.appPath + 'api/services/app/Profile/ChangeLevel2Password',
                data: {password:obj.password,otp:data.otp}
            }, function (rs) {
                abp.message.success("Thay đổi mật khẩu cấp 2 thành công");
                setTimeout(function () {
                    window.location.href = "/Profile";
                }, 1000);
            }, function (e) {
                if (e.responseJSON) {
                    if (e.responseJSON.success) {
                        abp.message.success(window.changePw2_msg);
                        setTimeout(function () {
                            window.location.href = "/Profile";
                        }, 1000);
                    } else {
                        abp.message.error(e.responseJSON.error.message);
                    }
                } else {
                    abp.message.error("Có lỗi trong quá trình xử lý");
                }
            });
        });
    },
    enterHandler: function () {
        ctrl.form.find('input, select').on("keyup", function (event) {
            if (event.keyCode === 13) {
                event.preventDefault();
                ctrl.nextToStep();
                return false;
            }
        });
    },
    back: function () {
        window.location.href = "/Profile";
    },
}

$(document).ready(function () {
    Sv.onlyNumberInput();
    ctrl.enterHandler();
});
