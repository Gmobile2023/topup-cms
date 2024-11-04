﻿(function () {
    $(function () {
        let old_method = $('input[name="old_method"]').val();
        if (old_method !== '' && old_method > 0) {
            $('input[name="security_method"][value='+ old_method +']').prop('checked', true);
        }
        
        $('#confirm_security_method_btn').click(function () {
            let secure_method = $('input[name="security_method"]:checked').val();

            if (secure_method === '' || secure_method === null) {
                abp.message.info("Vui lòng chọn hình thức bảo mật!");
                return false;
            }
            
            if (old_method == secure_method) {
                abp.message.info("Bạn đang dùng hình thức bảo mật này. \n Nếu muốn thay đổi xin hãy chọn sang hình thức khác!");
                return false;
            }
            
            if (secure_method !== '' && old_method !== secure_method ) {
                Dialog.otp(Dialog.otpType.ChangePaymentMethod, function () {
                    Sv.Api({
                        url: abp.appPath + 'api/services/app/Profile/ChangePaymentVerifyMethod',
                        data: {
                            Channel: 'WEB',
                            Type: secure_method
                        }
                    }, function (rs) {
                        abp.message.success("Thay đổi hình thức bảo mật thành công!");
                        setTimeout(
                            function() { 
                                window.location.reload();
                            }, 1000);
                    }, function (e) {
                        return false;
                    });
                });
            }
        });
    });
})();