let ctrl = {
    page: $("#mix-pin-panel"),
    topLabel: [
        {
            "title": "Mua thẻ điện thoại",
            "icon": "/themes/topup/images/ic_banner_top/05.svg"
        },
        {
            "title": "Mua thẻ Data",
            "icon": "/themes/topup/images/ic_banner_top/05.svg"
        },
        {
            "title": "Mua thẻ Game",
            "icon": "/themes/topup/images/nhattran/video-game.svg"
        }
    ],
    form: $("#topupForm", document),
    getTopupPrice: function (code, location, form) {
        Sv.Post({
            Url: abp.appPath + 'Topup/GetTopupPrice',
            Data: {catecode: code, serviceCode: ctrl.getServiceCode()}
        }, function (rs) {
            $(form).find(location).html(rs.result.content);
            $(form).find('.radio-btn-upperlogo').css({'background-image': 'url(' + localStorage.getItem('telco-logo') + ')'});
            let obj = ctrl.getFormValue();
            $(form).find("[name='cardPrice'][data-amount='" + obj.cardPrice + "']").prop("checked", 'checked').trigger('change').trigger('click');
        }, function () {
            $(form).find(location).html("");
        });
    },
    activeStep: function (index) {
        let _form = $("#" + ctrl.getPinFormCurrent()).parent();
        _form.find('.step').hide();
        _form.find('.step' + index).show();
    },
    getFormValue: function () {
        let serviceCode = ctrl.getServiceCode();
        let pinForm = ctrl.getPinFormCurrent();
        let _pinForm = $('#' + pinForm);
        let prod = _pinForm.find("[name='cardPrice']:checked");
        let obj = {
            cardTelco: _pinForm.find("[name='cardTelco'] option:selected").val(),
            cardPrice: prod.val(),
            productCode: prod.data("productcode"),
            categoryCode: _pinForm.find("[name='cardTelco'] option:selected").val(),
            amount: prod.val(),
            quantity: _pinForm.find("[name='quantity']").val(),
            email: _pinForm.find("[name='email']").val(),
            isReadTerm: _pinForm.find("[name='isReadTerm']").is(":checked"),
            serviceCode: serviceCode,
            paymentAmount: $(".step2 [name='_TotalAmount']").val(),
        };

        return obj;
    },
    valid: function () {
        let obj = ctrl.getFormValue();

        console.log('Thẻ: ' + obj)

        if (obj.cardTelco.length === 0) {
            abp.message.info("Vui lòng chọn nhà mạng");
            return false;
        }
        if (obj.productCode === null || obj.productCode === undefined || obj.productCode.length === 0) {
            abp.message.info("Vui lòng chọn mệnh giá");
            return false;
        }

        if (obj.quantity.length === 0) {
            abp.message.info("Vui lòng nhập số lượng mua");
            return false;
        }

        if (obj.quantity <= 0 || obj.quantity === "") {
            abp.message.info("Vui lòng nhập số lượng mua lớn hơn 0");
            return false;
        }

        if (obj.email && obj.email.length > 0 && !((/^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/).test(obj.email))) {
            abp.message.info("Email không đúng định dạng");
            return false;
        }

        if (!obj.isReadTerm) {
            abp.message.info("Vui lòng đồng ý chính sách của chúng tôi");
            return false;
        }

        return true;
    },
    nextToStep: function () {
        let pinForm = ctrl.getPinFormCurrent();
        let $step2 = $("#" + pinForm).parents('.tab-pane').find('form.step2');
        let obj = ctrl.getFormValue();

        if (!ctrl.valid()) {
            return false;
        }

        Sv.Post({
            Url: abp.appPath + 'Common/GetPayInfo',
            Data: {type: 'PIN_CODE', model: obj}
        }, function (rs) {
            $step2.find(".section-body-middle-form-p1").html(rs.result.content);
            ctrl.activeStep(2);
        }, function () {
            ctrl.page.find("#" + pinForm + " .section-body-middle-form-p1").html("");
        });
    },
    nextToStep2: function () {
        if (!ctrl.valid()) {
            return false;
        }

        let obj = ctrl.getFormValue();
        console.log(obj);
        Sv.checkUserTransValid(obj.serviceCode, obj.categoryCode, obj.productCode, obj.paymentAmount, obj.cardPrice, obj.quantity)
            .then(function(rs){
                Dialog.verifyTransCode(Dialog.otpType.Payment, function () {
                    Sv.Api({
                        url: abp.appPath + 'api/services/app/Transactions/CreatePinCodeRequest',
                        data: (obj)
                    }, function (rs) {
                        window.location.href = rs.extraInfo;
                    }, function (e) {
                        return false;
                    });
                });
            });
    },
    enterHandler: function () {
        let pinForm = ctrl.getPinFormCurrent();

        $('#' + pinForm).find('input, select').on("keyup", function (event) {
            if (event.keyCode === 13) {
                event.preventDefault();
                ctrl.nextToStep();
                return false;
            }
        });
    },
    changeQuantity: function () {
        setTimeout(function () {
            ctrl.form.find('input[name=quantity]').focus();
        }, 100);

        ctrl.form.find('input[name=quantity]').on("keyup", function (event) {
            let val = ctrl.form.find("[name='quantity']").val();
            if (val === "" || parseInt(val) <= 0) {
                ctrl.form.find('.button.btn-custom.btn-blue').prop('disabled', true);
            } else {
                ctrl.form.find('.button.btn-custom.btn-blue').prop('disabled', false);
            }
        }).trigger('keyup');
    },
    checkCardValue: function () {
        ctrl.form.find('.button.btn-custom.btn-blue').prop('disabled', true);
        $(document).on('click', '.form-group-radio li', function (event) {
            ctrl.form.find('.button.btn-custom.btn-blue').prop('disabled', false);
        });
    },
    handlerClickTelco: function () {
        let pinForm = ctrl.getPinFormCurrent();
        $('#' + pinForm + ' .section-body-middle-form-p1 .select2-card-telco').on("change", function (event) {
            let $this = $(this);
            let category_src = $this.find(':selected').attr('data-image');
            localStorage.setItem('telco-logo', category_src);
        });
    },
    changeActiveClass: function () {
        ctrl.page.find('.tab .nav-tabs > li').click(function () {
            let pinForm = $(this).attr('data-form');
            let indexEle = $(this).attr('data-label');

            ctrl.page.find('.tab .nav-tabs > li.active').removeClass('active');
            $(this).addClass('active').trigger('change');
            ctrl.changeLabelHeader(indexEle);

            ctrl.handlerClickTelco();
            $("#" + pinForm + " .section-body-middle-form-p1 .select2-card-telco option:eq(0)").prop('selected', true).trigger('change');

            $('.tab-pane').removeClass('in show active');
            if (pinForm === 'pinGameForm') {
                $('#Section3').addClass('active show');
            } else if (pinForm === 'pinDataForm') {
                $('#Section2').addClass('active show');
            } else if (pinForm === 'pinCodeForm') {
                $('#Section1').addClass('active show');
            }
            
            ctrl.activeStep(1);
        });
    },
    handlerListenChangeSelect: function () {
        $('.section-body-middle-form-p1 .select2-card-telco').on("change", function (event) {
            let $this = $(this);
            let image = $this.find(':selected').attr('data-image');
            let location = $(this).attr('data-location');
            let form = $(this).attr('data-form');
            let telCo = $(this).val();

            $('#' + form + ' .section-body-middle-form-p1 .form-logo').attr('src', image);
            localStorage.setItem('telco-logo', image);
            
            ctrl.getTopupPrice(telCo, '#' + location, '#' + form);
        });
    },
    handlerShowLogoInForm: function () {
        $('.section-body-middle-form-p1 .select2-card-telco').on("change", function (event) {
            let image = $(this).attr('data-image');
            let form = $(this).attr('data-form');
            $('#' + form + ' .section-body-middle-form-p1 .form-logo').attr('src', image);
        });
    },
    getServiceCode: function () {
        return ctrl.page.find('.tab .nav-tabs > li.active').attr('data-service');
    },
    getPinFormCurrent: function () {
        return ctrl.page.find('.tab .nav-tabs > li.active').attr('data-form');
    },
    changeLabelHeader: function (index) {
        let label = ctrl.topLabel[index];
        let element = $('.force-change-label');
        element.find('img').attr('src', label.icon);
        element.find('span').text(label.title);
    },
    getUrlVars: function () {
        let vars = [], hash;
        const hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        for (let i = 0; i < hashes.length; i++) {
            hash = hashes[i].split('=');
            vars.push(hash[0]);
            vars[hash[0]] = hash[1];
        }
        return vars;
    }
}

$(document).ready(function () {
    let mix_panel = $('#mix-pin-panel');
    ctrl.changeActiveClass();
    ctrl.handlerClickTelco();
    ctrl.enterHandler();
    ctrl.changeQuantity();
    ctrl.checkCardValue();
    ctrl.handlerListenChangeSelect();
    ctrl.handlerShowLogoInForm();
    
    let hash = window.location.hash;
    console.log(hash)
    if (hash) {
        $("#mix-pin-panel").find('li.' + hash.substring(1).toLowerCase()).trigger('click');
        ctrl.changeActiveClass();
    } else {
        mix_panel.find("ul.nav-tabs").find(".pin_code").trigger('click');
        $("#pinCodeForm .section-body-middle-form-p1 .select2-card-telco option:eq(0)").prop('selected', true).trigger('change');
    }

    Sv.keyEnter($(".step1"),  ctrl.nextToStep);
});