(function ($) {
    app.modals.ConvertPhoneModal = function () {

        const VietNamMobile = {
            prefixConfig: {
                VTE: ["086", "096", "097", "098", "032", "033", "034", "035", "036", "037", "038", "039"],
                VNA: ["091", "094", "083", "084", "085", "081", "082", "087", "088"],
                VMS: ["090", "093", "070", "076", "077", "078", "079", "089"],
                GMOBILE: ["099", "059"],
                VNM: ["092", "052", "056", "058"]
            },
            removeSpaces: function (val) {
                return val.split(' ').join('');
            },
            prefixAll: function () {
                var allPrefix = [];
                Object.keys(VietNamMobile.prefixConfig)
                    .forEach(function (key) {
                        allPrefix = allPrefix.concat(VietNamMobile.prefixConfig[key]);
                    });
                return allPrefix;
            },
            isActive: function (str) {
                var prefix = str.substring(0, 3);
                return VietNamMobile.prefixAll().indexOf(prefix) > -1;
            },
            valid: function (str) {

                if (!(/^0[0-9]{9}$/).test(str)) {
                    return ("Số điện thoại không đúng định dạng");
                }
                if (!VietNamMobile.isActive(str)) {
                    return ("Số điện thoại không được hỗ trợ");
                }
                return "";
            },
            getTelco: function (str) {
                let telco = '';
                if (VietNamMobile.isActive(str)) {
                    let prefix = str.substring(0, 3);
                    Object.keys(VietNamMobile.prefixConfig)
                        .forEach(function (key) {
                            if (VietNamMobile.prefixConfig[key].indexOf(prefix) > -1)
                                telco = key;
                        });
                }
                return telco;
            },
        }

        var _agentService = abp.services.app.agentManagerment;

        var _modalManager;


        var _$systemInformationForm = null;
        this.init = function (modalManager) {
            _modalManager = modalManager;

            //var modal = _modalManager.getModal();
            //modal.find('.date-picker').datetimepicker({
            //    locale: abp.localization.currentLanguage.name,
            //    format: 'L'
            //});

            _$systemInformationForm = _modalManager.getModal().find('form[name=ConvertPhoneInformationsForm]');
            _$systemInformationForm.validate();
            Sv.SetupAmountMask();
            $("#FileAttachment").on('change', function () {
                app.uploadFile($("#FileAttachment"), $('#FileAttachmentSrc'));
            });
        };

        this.save = function () {
            if (!_$systemInformationForm.valid()) {
                return;
            }
            var check = $("#FileAttachmentSrc").val();
            if (check == null || check == "") {
                abp.message.info('Vui lòng chọn văn bản đính kèm');
                return;
            }
            //var systemConvert = _$systemInformationForm.serializeFormToObject();

            var phone = $("#txtPhoneNew").val();
            if (phone.length === 0) {
                abp.message.info("Vui lòng nhập số điện thoại");
                return false;
            }

            var v = VietNamMobile.valid(phone);
            if (v.length > 0) {
                abp.message.info(v);
                return false;
            }


            var systemConvert =
                {
                    UserId: $("#hdnUserId").val(),
                    UserName: phone,
                    Attachment: $('#FileAttachmentSrc').val()
                };

            _modalManager.setBusy(true);
            _agentService.updateUserName(
                systemConvert
            ).done(function () {
                abp.message.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditAgentsTableModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);
