var ctrl = {
    page: $("#TransferMoney_Page", document),
    form: $("#mainForm1"),
    enabled_status: false,
    changeAccount: function () {
        Sv.GetAccountAnyField(ctrl.form.find("[name=Account]").val(), ctrl.form.find("#account-receive"), ctrl.form.find("#txtAccountCode"));
    },
    docSo: function (total) {
        Sv.BindMoneyToString($("#amount-to-text"), ctrl.form.find("[name=amount]").val());
    },
    getFormValue: function () {
        var obj = {
            account: ctrl.form.find("[name='Account']").val(),
            desAccount: ctrl.form.find("[name='DesAccount']").val(),
            amount: (ctrl.form.find("[name='amount']").val()),
            description: ctrl.form.find("[name='description']").val(),
        };
        return obj;
    },
    
    valid: function () {
        var obj = ctrl.getFormValue();
        if (obj.account.length === 0) {
            abp.message.info("Vui lòng nhập tài khoản nhận tiền");
            return false;
        }
        if (obj.amount.length === 0) {
            abp.message.info("Vui lòng nhập số tiền");
            return false;
        }
        if (obj.amount < 50000) {
            abp.message.info("Số tiền tối thiểu là 50.000đ ");
            return false;
        }
        if (obj.description.length === 0) {
            abp.message.info("Vui lòng nhập đầy đủ thông tin!");
            return false;
        }
        
        return true;
    },

    enabled_step: function () {
        if (ctrl.enabled_status) {
            $('.button.btn-custom.btn-blue').removeClass('btn-disabled').prop('disabled', false);
        } else {
            $('.button.btn-custom.btn-blue').addClass('btn-disabled').prop('disabled', true);
        }
    },

    activeStep: function (index) {
        ctrl.page.find('.step').hide();
        ctrl.page.find('.step' + index).show();
    },

    nextToStep: function () {
        // if (!ctrl.valid()) {
        //     return false;
        // }
        ctrl.enabled_step();
        var obj = ctrl.getFormValue();
        Sv.Post({
            Url: abp.appPath + 'Common/GetPayInfoDeposit',
            Data: {type: "TRANSFER", model: obj}
        }, function (rs) {
            ctrl.page.find("#mainForm2 .section-body-middle-form-p1").html(rs.result.content);
            ctrl.activeStep(2);
        }, function () {
            ctrl.page.find("#mainForm2 .section-body-middle-form-p1").html("");
        });
    },

    nextToStep2: function () {
        if (!ctrl.valid()) {
            return false;
        }
        ctrl.enabled_step();
        var obj = ctrl.getFormValue();
        Sv.checkUserTransValid("TRANSFER", "", "", obj.amount, 0, 0)
            .then(function(rs){
       // Sv.checkTransBalance(obj.amount, function () {
            Dialog.verifyTransCode(Dialog.otpType.Transfer, function () {
                //abp.ui.setBusy();
                abp.services.app.transactions.transferMoney(obj).done(function (rs) {
                    window.location.href = rs.extraInfo;
                }).always(function () {
                    abp.ui.clearBusy();
                });
            });
        })
    },

    enterHandler: function () {
        ctrl.form.find('input, select').on("keyup", function (event) {
            ctrl.enabled_step();
            if (event.keyCode === 13) {
                event.preventDefault();
                ctrl.nextToStep2();
                return false;
            }
        });
    },

    inputHandler: function () {
        ctrl.form.find('input, select').on("change, blur", function (event) {
            ctrl.enabled_step();
        });
    },
    
    amountToString: function () {
        $("#txtAmount").on('keyup input', function (e) {
            const $element = $(this);
            const val = $element.val();
            const $str = $("#amount-to-text");
            Sv.BindMoneyToString($str, val);
        });
    }
}

$(document).ajaxComplete(function(event, request, settings) {
    if (request.responseJSON.result != null) {
        let accountCode = request.responseJSON.result.accountCode;
        if (typeof accountCode !== 'undefined' && accountCode.length > 0) {
            ctrl.enabled_status = true;
            ctrl.enabled_step();
        }
    } else {
        ctrl.enabled_status = false;
        ctrl.enabled_step();
    }
});

$(document).ready(function () {
    Sv.SetupAmountMask();
    ctrl.inputHandler();
    ctrl.enabled_step();
    ctrl.form.find('[name=Account]').change(ctrl.changeAccount);
    //ctrl.form.find('[name=amount]').keyup(ctrl.docSo);
    ctrl.amountToString();
    Sv.keyEnter($(".step1"),  ctrl.nextToStep2);
});
