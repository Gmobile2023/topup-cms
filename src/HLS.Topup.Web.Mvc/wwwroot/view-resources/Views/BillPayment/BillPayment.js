(function () {
    let ctrl = {
        form: $('#steponeForm'),
        changePhoneNumber: function () {
            setTimeout(function () {
                ctrl.form.find('input[name=ReceiverInfo]').focus();
            }, 100);
            ctrl.form.find('input[name=ReceiverInfo]').on("keyup", function (event) {
                let val = ctrl.form.find("[name='ReceiverInfo']").val();
                ctrl.form.find("[name='ReceiverInfo']").val(VietNamMobile.removeSpaces(val));
            }).trigger('keyup');
        },
        valid: function (input) {
            return VietNamMobile.valid(input);
        }
    }

    let _commonService = abp.services.app.commonLookup;
    let _transactionService = abp.services.app.transactions;

    Sv.SetupAmountMask();
    ctrl.changePhoneNumber();

    $(function () {
        let categoryCode = '';
        let productCode = '';
        let _categoryBillMobile = "MOBILE_BILL";
        let _categoryBillEvn = "EVN_BILL";
        let data_storage = ['serviceCode', 'productCode', 'categoryCode', 'categoryName', 'amount', 'dataIcon'];

        $(document).ready(function () {
            let hash = location.hash;
            if (hash.length > 0) {
                handleClickPayBillCategory(hash.substring(1), getLocalStorage('dataIcon'));
            }
            $("#select2-product-code").on("change", select2ProductCodeChange);
        });

        let receiverInfoX = $('input[name="ReceiverInfo"]');
        receiverInfoX.on('blur', receiverInfoX, function () {
            if (receiverInfoX.val().length > 0 && getLocalStorage('categoryCode') === 'MOBILE_BILL' && ctrl.valid(receiverInfoX.val()).length > 0) {
                abp.message.warn('Số điện thoại không đúng định dạng');
                return;
            }

            return true;
        });

        $('.pay-bill-category-code').click(function () {
            clearLocalStorage(data_storage);
            let categoryCode = $(this).attr('data-category-code');
            let dataIcon = $(this).attr('data-src');
            let categoryName = $(this).find('.group-pay-bill-content').text();
            setLocalStorage('categoryCode', categoryCode);
            setLocalStorage('categoryName', categoryName);
            setLocalStorage('dataIcon', dataIcon);
            handleClickPayBillCategory(categoryCode, dataIcon, categoryName);
            select2ProductCodeChange();
        });

        $('#btnCreateBillPayment, .btn-create-pay').click(function () {
            payBill();
        });

        $(".txtPaymentAmount").on('keyup input', function (e) {
            const $element = $(this);
            const val = $element.val();
            const $str = $("#amount-to-text");
            setLocalStorage('amount', $(this).val());
            Sv.BindMoneyToString($str, val);
        });

        //Case này là truy vấn cho thanh toán trả sau
        $("#btn-query-payment").click(function () {
            let receiverInfo_data = $('input[name="ReceiverInfo"]').val();

            setLocalStorage('productCode', $('#select2-product-code option:selected').val());
            productCode = getLocalStorage('productCode');
            categoryCode = getLocalStorage('categoryCode');

            if (categoryCode === 'MOBILE_BILL' && ctrl.valid(receiverInfo_data).length > 0) {
                abp.message.warn('Số điện thoại không đúng định dạng');
                return;
            }

            if ((productCode === "" || productCode === null || productCode === undefined) && categoryCode !== _categoryBillEvn) {
                abp.message.warn('Vui lòng chọn nhà cung cấp dịch vụ');
                return;
            }

            if (receiverInfo_data === "" || receiverInfo_data === null || receiverInfo_data === undefined) {
                abp.message.warn('Vui lòng nhập thông tin truy vấn');
                return;
            }

            // Limit
            // if (productCode === 'VTE_BILL' && $('#txtPaymentAmount_Fill').val() < 5000) {
            //     abp.message.warn('Giá trị thanh toán của hóa đơn hiện tại nhỏ hơn giá trị tối thiểu theo quy định của nhà mạng. Quý khách vui lòng nhập số tiền thanh toán >= 5.000đ hoặc sử dụng tính năng Nạp tiền điện thoại để thanh toán hóa đơn. Trân trọng!');
            //     return;
            // }

            let receiverInfo = {
                ReceiverInfo: receiverInfo_data,
                ProductCode: productCode,
                CategoryCode: categoryCode,
                ServiceCode: '',
                IsInvoice: $('input[name=isInvoice]:checked').val()
            };
            abp.ui.setBusy();
            _transactionService.billQueryRequest(receiverInfo).done(function (obj) {
                $("#txtPaymentAmount_Fill").val(obj.amount);
                $("[name=_TotalAmount]").val(obj.paymentAmount);
            }).always(function () {
                abp.ui.clearBusy();
            });
        });

        $('.btn-query-bill').click(function () {
            let receiverInfo_data = $('input[name="ReceiverInfo"]').val();

            setLocalStorage('productCode', $('#select2-product-code option:selected').val());
            productCode = getLocalStorage('productCode');
            categoryCode = getLocalStorage('categoryCode');

            if (categoryCode === 'MOBILE_BILL' && ctrl.valid(receiverInfo_data).length > 0) {
                abp.message.warn('Số điện thoại không đúng định dạng');
                return;
            }

            if ((productCode === "" || productCode === null || productCode === undefined) && categoryCode !== _categoryBillEvn) {
                abp.message.warn('Vui lòng chọn nhà cung cấp dịch vụ');
                return;
            }

            if (receiverInfo_data === "" || receiverInfo_data === null || receiverInfo_data === undefined) {
                abp.message.warn('Vui lòng nhập thông tin truy vấn');
                return;
            }

            // Limit
            // if (productCode === 'VTE_BILL' && $('#txtPaymentAmount_Fill').val() < 5000) {
            //     abp.message.warn('Giá trị thanh toán của hóa đơn hiện tại nhỏ hơn giá trị tối thiểu theo quy định của nhà mạng. Quý khách vui lòng nhập số tiền thanh toán >= 5.000đ hoặc sử dụng tính năng Nạp tiền điện thoại để thanh toán hóa đơn. Trân trọng!');
            //     return;
            // }

            let isReadTerm = $("[name='isReadTerm']").is(":checked");
            if (!isReadTerm) {
                abp.message.warn('Vui lòng đồng ý chính sách của chúng tôi');
                return;
            }

            let receiverInfo = {
                ReceiverInfo: receiverInfo_data,
                ProductCode: productCode,
                CategoryCode: categoryCode,
                ServiceCode: '',
                IsInvoice: $('input[name=isInvoice]:checked').val(),
            };

            var amountFix = $("#txtPaymentAmount_Fill").val();
            receiverInfo.amount = amountFix === "" ? 0 : amountFix;

            if (categoryCode !== 'MOBILE_BILL') {
                receiverInfo.amount = 0;//chặn luôn từ js. chỉ thanh toán điện thoại trả sau mới được thanh toán theo số tiền
            }

            if (productCode === 'VTE_BILL' && receiverInfo.amount < 5000) {
                abp.message.warn('Giá trị thanh toán của hóa đơn hiện tại nhỏ hơn giá trị tối thiểu theo quy định của nhà mạng. Quý khách vui lòng nhập số tiền thanh toán >= 5.000đ hoặc sử dụng tính năng Nạp tiền điện thoại để thanh toán hóa đơn. Trân trọng!');
                return;
            }

            abp.ui.setBusy();
            receiverInfo.isCheckAmount = true;
            _transactionService.billQueryRequest(receiverInfo).done(function (obj) {
                console.log(obj);
                setLocalStorage('amount', obj.amount);
                clickHandleShowHide(3);
                let customerInfo = {
                    'fullName': obj.fullName,
                    'customerReference': obj.customerReference,
                    'address': obj.address,
                    'amount': Sv.NumberToString(obj.amount) + ' VND',
                    'disountAmount': Sv.NumberToString(obj.disountAmount) + ' VND',
                    'paymentAmount': Sv.NumberToString(obj.paymentAmount) + ' VND',
                    /*'period': obj.period,*/
                    'fee': Sv.NumberToString(obj.fee) + ' VND',
                    'productName': obj.productName
                };

                $.each(customerInfo, function (key, val) {
                    $('.' + key).text(val);
                });

                if (obj.periodDetails != null && obj.periodDetails.length > 0) {
                    var str = '';
                    $(".periodNow").remove();
                    $.each(obj.periodDetails, function (key, val) {
                        str = str + '<div class="form-group form-group-dashed row periodNow">';
                        str = str + '<div class="col-xs-6 control-label">Kỳ thanh toán ' + val.period + '</div>';
                        str = str + '<div class="col-xs-6 control-label control-label-result">' + Sv.NumberToString(val.amount) + ' VND</div>';
                        str = str + '</div>';
                    });

                    $(".periodList").after(str);
                    //$('.period').hide();

                }

                $("[name=_TotalAmount]").val(obj.paymentAmount);
                $("#txtFullName").val(obj.fullName);
                $("#txtCustomerReference").val(obj.customerReference);
                $("#txtPeriod").val(obj.period);
                $("#txtAddress").val(obj.address);
            }).always(function () {
                abp.ui.clearBusy();
            });
        });

        function payBill() {
            let amount = parseInt(getLocalStorage('amount'));
            $(".txtPaymentAmount").val(amount);

            const cate = getLocalStorage("categoryCode");

            if (!$("#billpayment-form").valid()) {
                return;
            }
            const productCode = $('#select2-product-code option:selected').val();
            if ((productCode === "" || productCode === null || productCode === undefined) && categoryCode !== _categoryBillEvn) {
                abp.message.warn('Vui lòng chọn nhà cung cấp dịch vụ');
                return;
            }

            if (cate === _categoryBillMobile || amount === 0 || amount === undefined) {
                amount = $(".txtPaymentAmount").val();
            }

            if (amount === 0 || amount === "" || amount === undefined) {
                abp.message.warn("Số tiền thanh toán không hợp lệ!");
                return;
            }

            // Limit
            if (productCode === 'VTE_BILL' && amount < 5000) {
                abp.message.warn('Giá trị thanh toán của hóa đơn hiện tại nhỏ hơn giá trị tối thiểu theo quy định của nhà mạng. Quý khách vui lòng nhập số tiền thanh toán >= 5.000đ hoặc sử dụng tính năng Nạp tiền điện thoại để thanh toán hóa đơn. Trân trọng!');
                return;
            }

            let data = {
                'ReceiverInfo': $('input[name="ReceiverInfo"]').val(),
                'Amount': amount,
                'CategoryCode': getLocalStorage('categoryCode'),
                'ProductCode': productCode,
                invoiceInfo: {
                    fullName: $("#txtFullName").val(),
                    customerReference: $("#txtCustomerReference").val(),
                    period: $("#txtPeriod").val(),
                    address: $("#txtAddress").val()
                }
            }

            Sv.checkUserTransValid("PAY_BILL", data.CategoryCode, data.ProductCode, $('input[name="_TotalAmount"]').val(), data.Amount, 1)
                .then(function (rs) {
                    //Sv.checkTransBalance(data.Amount, function () {
                    Dialog.verifyTransCode(Dialog.otpType.Payment, function () {
                        abp.ui.setBusy();
                        _transactionService.payBillRequest(data).done(function (rs) {
                            window.location.href = rs.extraInfo;
                        }).always(function () {
                            clearLocalStorage(data_storage);
                            abp.ui.clearBusy();
                        });
                        //});
                    });
                });
        }

        function getAllProducts(categoryCode) {
            $('input[type="text"]').val('');

            Sv.RequestStart();
            _commonService.getProducts({ 'categoryCode': categoryCode, isActive: true }).done(function (obj) {
                $('#select2-product-code').append('<option value="">Chọn nhà cung cấp</option>');
                $.each(obj, function (key, value) {
                    $('#select2-product-code')
                        .append($("<option></option>")
                            .attr("value", value.productCode)
                            .attr('data-image', value.image)
                            .text(value.productName));
                });
                Sv.RequestEnd();
            }).always(function () {

            });
        }

        function handleClickPayBillCategory(categoryCode, dataIcon, categoryName) {
            clickHandleShowHide(2);
            $('#select2-product-code').empty();
            $('.bill-type-name').text(getLocalStorage('categoryName'));
            $('.section-body-icon-pay-bill').css(
                {
                    'background': 'url(' + dataIcon + ')',
                    'background-size': '5.5em 11em',
                    'background-position': 'center top',
                    'background-repeat': 'no-repeat',
                    'margin-top': '-20px'
                }
            );
            if (categoryCode === _categoryBillMobile) {
                //$('.btn-query-bill').hide();
                $('.request-by-type').hide();
                $('.payment-amount-tele').show();
                //$('.btn-create-pay').show();
                $('.receiver-info-code').text('Số điện thoại');
                $('input[name="ReceiverInfo"]').attr('placeholder', 'Nhập số điện thoại');
                $("#show-product").removeClass("hidden").addClass("show");
            } else if (categoryCode === _categoryBillEvn) {
                $("#show-product").removeClass("show").addClass("hidden");
                //productCode="EVN_BILL";
            } else {
                $('.payment-amount-tele').hide();
                $('.request-by-type').show();
                //$('.btn-query-bill').show();
                //$('.btn-create-pay').hide();
                $('.receiver-info-code').text('Mã khách hàng');
                $('input[name="ReceiverInfo"]').attr('placeholder', 'Nhập mã khách hàng');
                $("#show-product").removeClass("hidden").addClass("show");
            }

            getAllProducts(categoryCode);
        }

        // select2ProductCodeChange select2-product-code
        function select2ProductCodeChange(e) {
            let image_default = '/themes/topup/images/1ceilings-service.png';
            if (e == undefined)
                return;
            let $this = $(e.target);
            let val = $(e.target).val();
            let image = $this.find('option:selected').attr('data-image');
            let logo_element = $('.section-body-middle-form-p1 .form-logo');

            if (typeof image == "undefined") {
                logo_element.css({ 'display': 'none' });
            } else if (image != '' && val != '' || image != null && val != '') {
                logo_element.attr('src', image);
                logo_element.css({ 'display': 'block' });

                logo_element.on("error", function () {
                    $(this).unbind("error").css({ 'display': 'none' });
                });
            } else {
                $('.section-body-middle-form-p1 .form-logo').css({ 'display': 'none' });
            }

            let cate = getLocalStorage("categoryCode");
            let $p = $("#txtPaymentAmount_Fill").parents('.form-group-wrap');
            if (cate === "MOBILE_BILL" && val === "VNA_BILL") {
                $p.append("<small class='red'>" + app.localize('VinaBill_MinAmount_Warning') + "</small>");
                $('#btn-query-payment').hide();
            } else {
                $('#btn-query-payment').show();
                $p.find('small').remove();
            }
        }


    });
})();
