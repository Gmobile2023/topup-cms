var ctrl = {
    page: $("#Topup_Page"),
    TabActive: function (value) {
        ctrl.page.find('.navbar-card.row  li')
            .removeClass('active')
            .removeClass('show');
        ctrl.page.find('.navbar-card.row li.' + value)
            .addClass('active');
        ctrl.page.find('.tab-content.tab-content--card > div.tab-pane')
            .removeClass('active')
            .removeClass('show')
            .css('opacity', 0)
            .css('display', 'none');
        ctrl.page.find('#' + value)
            .addClass('active')
            .css('opacity', 1)
            .fadeOut('slow', function () {
                ctrl.page.find('#' + value).show();
            });
    },
    form: $("#card__slider_menu1"),
    getTopupPrice: function (code) {
        Sv.Post({
            Url: abp.appPath + 'Topup/GetTopupPrice',
            Data: { catecode: code, serviceCode:$("#ServiceCode").val() }
        }, function (rs) {
            var obj = ctrl.getFormValue();
            ctrl.form.find("#card-value-items").html(rs.result.content);
            ctrl.form.find('.radio-btn-upperlogo').css({ 'background-image': 'url(' + localStorage.getItem('telco-logo') + ')' });
            ctrl.form.find("[name='cardPrice'][data-amount='" + obj.cardPrice + "']").prop("checked", 'checked').trigger('change');
        }, function () {
            ctrl.form.find("#card-value-items").html("");
        });
    },
    activeStep: function (index) {
        ctrl.form.find('.step').hide();
        ctrl.form.find('.step' + index).show();
    },
    getFormValue: function () {
        var prod = ctrl.form.find("[name='cardPrice']:checked");
        var obj = {
            cardTelco: ctrl.form.find("[name='cardTelco'] option:selected").val(),
            phoneNumber: ctrl.form.find("[name='phoneNumber']").val(),
            cardPrice: prod.val(),
            productCode: prod.data("productcode"),
            categoryCode: ctrl.form.find("[name='cardTelco'] option:selected").val(),
            amount: prod.val(),
            isReadTerm: ctrl.form.find("[name='isReadTerm']").is(":checked"),
            paymentAmount: $(".step2 [name='_TotalAmount']").val(),
        };
        return obj;
    },
    changePhoneNumber: function () {
        setTimeout(function () {
            ctrl.form.find('input[name=phoneNumber]').focus();
        }, 100);
        ctrl.form.find('input[name=phoneNumber]').on("keyup", function (event) {
            let prod = $(document).find('input[name="cardPrice"]:checked');
            let val = ctrl.form.find("[name='phoneNumber']").val();
            ctrl.form.find("[name='phoneNumber']").val(VietNamMobile.removeSpaces(val));
            let valid = VietNamMobile.valid(val);
            if (valid.length > 0 && !prod) {
                ctrl.form.find('.button.btn-custom.btn-blue').prop('disabled', true);
            } else {
                ctrl.form.find('.button.btn-custom.btn-blue').prop('disabled', false);
            }
        }).trigger('keyup');
        ctrl.form.find('input[name=phoneNumber]').on("change", function (event) {
            const obj = ctrl.getFormValue();
            if (obj.phoneNumber.length === 0) {
                return false;
            }
            let v = VietNamMobile.valid(obj.phoneNumber);
            if (v.length > 0) {
                abp.message.info(v);
                return false;
            }
            let telco = VietNamMobile.getTelco(obj.phoneNumber);
            ctrl.form.find(".select2-card-telco option").each(function (i, e) {
                var service = $(e).val();
                if (service.startsWith(telco)) {
                    $(e).prop('selected', true).trigger('change');
                }
            });
        });
    },
    valid: function () {
        var obj = ctrl.getFormValue();
        if (obj.phoneNumber.length === 0) {
            abp.message.info("Vui lòng nhập số điện thoại");
            return false;
        }
        var v = VietNamMobile.valid(obj.phoneNumber);
        if (v.length > 0) {
            abp.message.info(v);
            return false;
        }
        if (obj.cardTelco == null || obj.cardTelco.length === 0) {
            abp.message.info("Vui lòng chọn nhà mạng");
            return false;
        }
        if (obj.cardPrice == null || obj.cardPrice.length === 0 || parseFloat(obj.cardPrice + "") <= 0) {
            abp.message.info("Vui lòng chọn mệnh giá");
            return false;
        }
        // if (!obj.isReadTerm) {
        //     abp.message.info("Vui lòng đồng ý chính sách của chúng tôi");
        //     return false;
        // }
        return true;
    },
    // netx step 1
    nextToStep: function () {
        if (!ctrl.valid()) {
            return false;
        }
        var obj = ctrl.getFormValue();
        var service = $("#ServiceCode").val();
        Sv.Post({
            Url: abp.appPath + 'Common/GetPayInfo',
            Data: { type: service, model: obj }
        }, function (rs) {
            // var ckBalance = Sv.checkBalance(rs.result);
            // if (ckBalance) {            
            ctrl.form.find("#topupInfoForm .section-body-p1").html(rs.result.content);
            ctrl.activeStep(2);
            //}
        }, function () {
            ctrl.form.find("#topupInfoForm .section-body-p1").html("");
        });
    },
    nextToStep2: function () {
        if (!ctrl.valid()) {
            return false;
        }
        var obj = ctrl.getFormValue();
        obj.serviceCode = $("#ServiceCode").val();
        Sv.checkUserTransValid(obj.serviceCode, obj.categoryCode, obj.productCode, obj.paymentAmount, obj.cardPrice, 1)
            .then(function (rs) {
                Dialog.verifyTransCode(Dialog.otpType.Payment, function () {
                    Sv.Api({
                        url: abp.appPath + 'api/services/app/Transactions/CreateTopupRequest',
                        data: (obj)
                    }, function (rs) {
                        window.location.href = rs.extraInfo;
                    }, function (e) {
                        abp.message.info(e);
                        return false;
                    });
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
    checkTopupValue: function () {
        ctrl.form.find('.button.btn-custom.btn-blue').prop('disabled', true);
        $(document).on('click', '#card-value-items .form-group-radio li', function (event) {
            if ($('input[name="phoneNumber"]').val().length > 0) {
                ctrl.form.find('.button.btn-custom.btn-blue').prop('disabled', false);
            }
        });
    },
    handlerListenChangeSelect: function () {
        $('.select2-card-telco').on("change", function (event) {
            let $this = $(this);
            let image = $this.find(':selected').attr('data-image');
            let location = $(this).attr('data-location');
            let form = $(this).attr('data-form');
            let telCo = $(this).val();

            $('#' + form + ' .form-logo').attr('src', image);
            localStorage.setItem('telco-logo', image);            
            ctrl.getTopupPrice(telCo, '#' + location, '#' + form);
        });
    },
};

$(document).ready(function () {
    ctrl.TabActive('card__slider_menu1');
    ctrl.changePhoneNumber();
    //ctrl.enterHandler();
    ctrl.checkTopupValue();
    if (!Sv.isMobile()) {
        Sv.keyEnter($(".step1"), ctrl.nextToStep);
    } else {
        $(window).keydown(function (event) {
            if (event.keyCode === 13) {
                event.preventDefault();
                return false;
            }
        });
    }
    ctrl.handlerListenChangeSelect();
    // default select telco
    $(".select2-card-telco option:eq(0)").prop('selected', true).trigger('change');

    $("#Topup_Page", document).find('ul.form-group-radio li').on("click", function (event) {
        $("#Topup_Page", document).find('ul.form-group-radio li label.label-height-auto').removeClass('checked');
        $(this).find('label.label-height-auto').addClass('checked');
    });
});
