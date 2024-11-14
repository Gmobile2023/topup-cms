var ctrl = {
    page: $("#Deposit_Page", document),
    form: $("#mainForm1"),
    TabActive: function (value) {
        $('.navbar-card.row  li')
            .removeClass('active')
            .removeClass('show');
        $('.navbar-card.row li.' + value)
            .addClass('active');
        $('.tab-content.tab-content--card > div.tab-pane')
            .removeClass('active')
            .removeClass('show')
            .css('opacity', 0)
            .css('display', 'none');
        $('#' + value)
            .addClass('active')
            .css('opacity', 1)
            .fadeOut('slow', function () {
                $('#' + value).show();
            });
    },
    getRecentDeposit: function (total) {
        abp.services.app.deposits.getDepositRequest({total: total}).done(function (obj) {
            let append = '<div >';
            const trans_status = {
                0: 'Chờ duyệt',
                1: 'Đã duyệt',
                3: 'Đã huỷ',
                5: 'Lỗi'
            };
            $.each(obj, function (i, $val) {
                const date = moment(new Date($val.createtionTime)).format('DD/MM/YYYY HH:mm:ss');
                const nf = Intl.NumberFormat();
                append += '<div class="form-group-dashed" >' +
                    '    <div class="form-group row">' +
                    '       <label class="col-md-4 control-label">Mã nạp: <strong>' + $val.requestCode + '</strong> </label>' +
                    '       <label class="col-md-4 control-label">Thời gian GD: ' + date + ' </label>' +
                    '       <label class="col-md-4 control-label">Số tiền: ' + nf.format($val.amount) + ' đ </label>' +
                    '       <label class="col-md-4 control-label">Ngân hàng: ' + $val.bankName + ' </label>' +
                    '       <label class="col-md-4 control-label">Trạng thái: ' + trans_status[$val.status] + ' </label>' +
                    '   </div>' +
                    '</div>' + 
                    '<hr />';
            });
            append += '</div>';
            $('.recent-deposits-list').html(append);

        }).always(function () {

        });
    },

    getBank: function () {
        abp.services.app.commonLookup.getBanks().done(function (obj) {
            $.each(obj, function (key, value) {
                $('#bankId')
                    .append($("<option></option>")
                        .attr("value", value.id)
                        .attr("data-bank-id", value.id)
                        .attr("data-bank-name", value.bankName)
                        .attr("data-bankaccountcode", value.bankAccountCode)
                        .attr("data-bankaccountname", value.bankAccountName)
                        .attr("data-branchname", value.branchName)
                        .attr("data-image", value.images)
                        .text(value.bankName));
            });
        }).always(function () {
        });
    },

    changeBank: function (_this) {
        ctrl.page.find('[name=bankAccountCode]').val(_this.attr('data-bankaccountcode'));
        ctrl.page.find('[name=bankAccountName]').val(_this.attr('data-bankaccountname'));
        ctrl.page.find('[name=branchName]').val(_this.attr('data-branchname'));
    },

    getFormValue: function () {
        var obj = {
            amount: (ctrl.form.find("[name='amount']").val()),
            bankId: ctrl.form.find("[name='bankId']").val(),
            bankName: ctrl.form.find("[name='bankName']").val(),
            bankAccountCode: ctrl.form.find("[name='bankAccountCode']").val(),
            bankAccountName: ctrl.form.find("[name='bankAccountName']").val(),
            branchName: ctrl.form.find("[name='branchName']").val(),
            description: ctrl.form.find("[name='description']").val(),
            requestCode: ctrl.form.find("[name='requestCode']").val(),
        };
        return obj;
    },

    valid: function () {
        var obj = ctrl.getFormValue();
        //console.log("dsadasdasd " + obj.bankId);
        if (obj.bankId <= 0) {
            abp.message.info("Vui lòng chọn ngân hàng");
            return false;
        }
        if (obj.bankName === null || obj.bankName === undefined || obj.bankName === "") {
            abp.message.info("Vui lòng chọn ngân hàng");
            return false;
        }
        // if (obj.amount == undefined || obj.amount.length == 0) {
        //     abp.message.info("Vui lòng nhập số tiền nạp");
        //     return false;
        // }
        obj.amount = parseFloat(obj.amount);
        if (obj.amount < 50000) {
            abp.message.info("Vui lòng nhập số tiền nạp lớn hơn hoặc bằng 50.000đ ");
            return false;
        }
        if (obj.description.length == 0) {
            abp.message.info("Vui lòng nhập đầy đủ thông tin!");
            return false;
        }
        return true;
    },

    activeStep: function (index) {
        ctrl.page.find('.step').hide();
        ctrl.page.find('.step' + index).show();
    },

    nextToStep: function () {
        if (!ctrl.valid()) {
            return false;
        }
        if (!$("#mainForm1").valid())//Validate luôn kiểu này. K phải alert tưng thông báo 1
        {
            return false;
        }
        var obj = ctrl.getFormValue();
        Sv.Post({
            Url: abp.appPath + 'Common/GetPayInfoDeposit',
            Data: {type: "DEPOSIT", model: obj}
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
        var obj = ctrl.getFormValue();
        //Dialog.password2Level(function () {
        // Sv.Api({
        //     url: abp.appPath + 'api/services/app/Deposits/DepositRequest',
        //     data: (obj)
        // }, function (transcode) {
        //     const mess = "Yêu cầu nạp nạp tiền của bạn đã được gửi thành công";
        //     window.location.href = "/Transactions/TransactionInfo?code=01&transCode=" + transcode + "&message=" + mess + "&transType=2";
        // }, function (e) {
        //     abp.message.info(e);
        //     return false;
        // });
        //})
        const _transaction = abp.services.app.deposits;
        abp.ui.setBusy();
        _transaction.depositRequest(obj).done(function (rs) {
            window.location.href = rs.extraInfo;
            //window.location.href = "/Transactions/TransactionInfo?code=01&transCode=" + transcode + "&message=" + mess + "&transType=2";
        }).always(function () {
            abp.ui.clearBusy();
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
    amountToString: function () {
        $("#txtAmount").on('keyup input', function (e) {
            const $element = $(this);
            const val = $element.val();
            const $str = $("#amount-to-text");
            Sv.BindMoneyToString($str, val);
        });
    },
    copy: function () {
        ctrl.page.find(".form-group-wrap .icon-copy").on('click', function (e) {
            var $p = $(e.target).parents('.form-group');
            var inputTemp = $p.find('input[type="text"]');
            if (inputTemp.val() !== undefined && inputTemp.val().length > 0) {
                Clipboard.copy(inputTemp.val() + "");
                abp.message.info("Đã copy " + $p.find('label.title').text().toLowerCase());
            }
        });
    },
    copy2: function (str) {
        Clipboard.copy(str + "");
        abp.message.info("Đã copy số tài khoản");
        //ctrl.page.find(".xcopy").on('click', function () {
        //     var dummy = document.createElement("textarea");
        //     document.body.appendChild(dummy);
        //     dummy.value = str+"";
        //     dummy.select();
        //     document.execCommand("copy");
        //     document.body.removeChild(dummy);
        //     abp.message.info("Đã copy số tài khoản");
        //});
    },
    handlerListenChangeBank: function () {
        $('#bankId').on("change", function (event) {
            let $this = $(this);
            $this = $this.find(':selected');
            let image = $this.attr('data-image');

            if ($(this).val() != '' || $(this).val() == null) {
                $('.bank-account-info i').hide();
                ctrl.page.find('p.bankAccountName').html('<span class="bank-info">Ngân hàng: </span> <span class="xinfo bankName">' + $this.attr('data-bankname') + '</span>');
                ctrl.page.find('p.bankAccountCode').html('<span class="bank-info">Số tài khoản: </span> <span class="xinfo bankAccountCode">' + $this.attr('data-bankaccountcode') + '</span><i class="far fa-copy icon-copy xcopy" onclick="ctrl.copy2(\'' + $this.attr('data-bankaccountcode') + '\')"></i>');
                ctrl.page.find('p.bankAccountName').html('<span class="bank-info">Chủ tài khoản: </span> <span class="xinfo bankAccountName">' + $this.attr('data-bankaccountname') + '</span>');
                ctrl.page.find('p.branchName').html('<span class="bank-info">Chi nhánh: </span> <span class="xinfo branchName">' + $this.attr('data-branchname') + '</span>');

                //Xem lại đoạn này k phải gán lại vào input cũ nữa.
                ctrl.page.find('[name=bankAccountCode]').val($this.attr('data-bankaccountcode'));
                ctrl.page.find('[name=bankAccountName]').val($this.attr('data-bankaccountname'));
                ctrl.page.find('[name=branchName]').val($this.attr('data-branchname'));
                ctrl.page.find('[name=bankName]').val($this.attr('data-bank-name'));
                ctrl.page.find('[name=bankId]').val($this.attr('data-bank-id'));

                $('.form-logo').attr('src', image).css({'display': 'inherit'});
            } else {
                $('.form-logo').attr('src', image).css({'display': 'none'});
            }
        });
    },
    // getRequestCode: function () {
    //     const _transaction = abp.services.app.deposits;
    //     abp.ui.setBusy();
    //     _transaction.getRandomRequestCode().done(function (rs) {
    //         if (rs.payload) {
    //             $('#requestCode').text(rs.payload + ' - ');
    //             $('input[name="requestCode"]').val(rs.payload);
    //             let descInput = $('input[name="description"]');
    //             descInput.val(rs.payload + ' - ' + descInput.val());
    //         }
    //     }).always(function () {
    //         abp.ui.clearBusy();
    //     });
    // }
}

$(document).ready(function () {
    Sv.SetupAmountMask();
    ctrl.TabActive('card__slider_menu1');
    ctrl.getRecentDeposit(3);
    ctrl.enterHandler();
    ctrl.getBank();
    ctrl.amountToString();
    ctrl.copy();
    ctrl.handlerListenChangeBank();
});
