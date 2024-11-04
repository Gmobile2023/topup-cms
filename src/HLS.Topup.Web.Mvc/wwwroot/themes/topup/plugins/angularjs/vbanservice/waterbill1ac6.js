//=============================================================================================
//                              WATER BILL CONTROLLER
//=============================================================================================
topupApp.controller('WaterBillController', ["$scope", "$locale", function ($scope, $locale) {
    //Global Init value
    $locale.NUMBER_FORMATS.GROUP_SEP = '.';
    $scope.amount = 0;
    $scope.panelStatus = 1;
    $scope.isReadTerm = true;
    $scope.payMethod = "";
    $scope.CityArr = [];
    $scope.DistrictArr = [];
    $scope.giftcode = '';
    $scope.isGiftCodeValid = '0';
    $scope.isGiftCodeNotValid = '0';
    $scope.isGiftCodeNotApproved = '0';
    $scope.gifterrormsg = '';
    $scope.giftcodevalue = 0;
    $scope.amountpay = -1;
    //Custom init value
    $scope.provider = "";
    $scope.feetnx = 0;

    //Captcha
    $scope.CaptchaValue = "";

    $scope.GenCap = function () {
        var text = "";
        var possible = "0123456789";

        for (var i = 0; i < 6; i++)
            text += possible.charAt(Math.floor(Math.random() * possible.length));

        $scope.CaptchaValue = text;

        $.ajax({
            url: "../../service/visadirect/gen-captcha-hash",
            data: {
                input: $scope.CaptchaValue
            },
            type: "POST",
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (data) {
                $scope.CapHash = data.Data;
                if (!$scope.$$phase)
                    $scope.$apply();
            }
        });

        if (!$scope.$$phase)
            $scope.$apply();
    }

    $scope.ChangeMobileBanking = function () {
        $scope.payMethod = "VNPAYQR";
        if (!$scope.$$phase)
            $scope.$apply();
    }

    $.validator.addMethod("regx", function (value, element, regexpr) {
        return regexpr.test(value);
    }, "");

    $scope.getCity = function () {
        $.ajax({
            url: "../../service/getregional",
            data: {
                parentId: 1
            },
            type: "POST",
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (data) {
                $scope.CityArr = data.Data;
                $scope.visaCusCity = 630;
                //$(".select-city").select2({
                //    width: '100%',
                //    placeholder: "Chọn Tỉnh/Thành phố",
                //});
                if (!$scope.$$phase)
                    $scope.$apply();
            },
            error: ""
        });
    };

    $scope.getDistrict = function (city) {
        $.ajax({
            url: "../../service/getregional",
            data: {
                parentId: city
            },
            type: "POST",
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (data) {
                $scope.DistrictArr = data.Data;
                $scope.visaCusDistrict = $scope.DistrictArr[0].RegionId;
                if (!$scope.$$phase)
                    $scope.$apply();
            },
            error: ""
        });
    };

    $scope.validGiftCode = function () {
        if ($scope.giftcode === '' || typeof $scope.giftcode === 'undefined') {
            $scope.gifterrormsg = "Quý khách vui lòng nhập mã giảm giá";
            //vinhnt, bo sung, reset lai trang thai ban dau
            $scope.deleteGiftCode();

            if (!$scope.$$phase)
                $scope.$apply();
            //$('#error-modal').modal();
        } else if (!isCharacterValid($scope.giftcode)) {
            $scope.gifterrormsg = "Mã giảm giá không được chứa kí tự đặc biệt";
            if (!$scope.$$phase)
                $scope.$apply();
            //$('#error-modal').modal();
        } else {
            $scope.isGiftCodeNotApproved = '0';
            //if ($scope.giftcode === '') {
            //    $scope.errormsg = "Quý khách vui lòng nhập mã giảm giá";
            //    if (!$scope.$$phase)
            //        $scope.$apply();
            //    $('#error-modal').modal();
            //} else if (!isCharacterValid($scope.giftcode)) {
            //    $scope.errormsg = "Mã giảm giá không được chứa kí tự đặc biệt";
            //    if (!$scope.$$phase)
            //        $scope.$apply();
            //    $('#error-modal').modal();
            //} else {
                $.ajax({
                    url: "../../service/valid-gift-code",
                    data: {
                        addData: "WATERBILLING-" + $scope.cusmobile,
                        giftcode: $scope.giftcode,
                        price: $scope.amount
                    },
                    type: "POST",
                    beforeSend: function () {
                    },
                    complete: function () {
                    },
                    success: function (data) {
                        if (data.Code === '00') {
                            $scope.giftcodevalue = data.GiftCodeValue;
                            $scope.amountpay = data.AmountPay;
                            $scope.isGiftCodeValid = '1';
                            $scope.isGiftCodeNotValid = '0';
                            $scope.gifterrormsg = '';
                        } else {
                            $scope.isGiftCodeValid = '0';
                            $scope.isGiftCodeNotValid = '1';
                            $scope.gifterrormsg = 'Mã Gift Code bị khóa hoặc hết hạn. Vui lòng liên hệ ĐT hỗ trợ 1900 5555 77';
                        }
                        if (!$scope.$$phase)
                            $scope.$apply();
                    },
                    error: ""
                });
            //}
        }
        return false;
    };

    $scope.deleteGiftCode = function () {
        $scope.giftcode = '';
        $scope.isGiftCodeValid = '0';
        $scope.isGiftCodeNotValid = '0';
        $scope.amountpay = -1;
        $scope.giftcodevalue = 0;
    };

    $scope.nextToPay = function () {
        $('#steponeForm').submit(function (e) {
            e.preventDefault();
        }).validate({
            rules: {
                cusmobile: {
                    required: true,
                    regx: /^(096|097|098|0162|0163|0164|0165|0166|0167|0168|0169|086|032|033|034|035|036|037|038|039|0129|0127|0125|0124|0123|094|091|088|081|082|083|084|085|090|093|0120|0121|0122|0126|0128|089|070|076|077|078|079|092|0188|0186|056|058|099|0199|059)\d{7}$/
                },
                billcode: {
                    required: true
                },
                provider: {
                    required: true
                },
                capchakey: {
                    required: true
                }
            },
            messages: {
                cusmobile: {
                    required: "Vui lòng nhập số điện thoại",
                    regx: "Số thuê bao không hợp lệ"
                },
                billcode: {
                    required: "Vui 	lòng nhập mã danh bạ."
                },
                provider: {
                    required: "Vui lòng chọn nhà cung cấp"
                },
                capchakey: {
                    required: "Vui lòng nhập mã xác thực!"
                }
            },
            errorPlacement: function (error, element) {
                if (element.attr("name") === "provider")
                    error.insertAfter("#provider-error");
                else
                    error.insertAfter(element);
            },
            submitHandler: function (form) {
                //clear amountpay, paymethod
                $scope.payMethod = "";
                $scope.amountpay = -1;
                $scope.feetnx = 0;
                $scope.amountpaytemp = 0;
                //clear visa input
                $scope.visaCusName = "";
                $scope.visaCusEmail = "";
                $scope.visaCusMobile = "";
                $scope.visaCusAddress = "";

                // do other things for a valid form
                if (!$scope.isReadTerm) {
                    $('#term-modal').modal();
                } else {
                    $.ajax({
                        url: "../../service/water-bill/getbill",
                        data: {
                            billcode: encodeURIComponent($scope.billcode),
                            providerId: $scope.provider,
                            captcha: $scope.capchakey,
                            hash: $scope.CapHash
                        },
                        type: "POST",
                        beforeSend: function () {
                            $("#service-loader-wrapper").css("display", "");
                        },
                        complete: function () {
                            $("#service-loader-wrapper").css("display", "none");
                        },
                        success: function (data) {
                            if (data.Code === '00') {
                                $scope.amount = data.Data;
                                if ($scope.giftcode !== '' && typeof $scope.giftcode !== 'undefined') { //Use GiftCode
                                    if ($scope.isGiftCodeValid !== '1') {
                                        if ($scope.isGiftCodeNotValid === '1') {
                                            $scope.gifterrormsg = 'Mã Gift Code bị khóa hoặc hết hạn. Vui lòng liên hệ ĐT hỗ trợ 1900 5555 77';
                                            return;
                                        }
                                        $scope.isGiftCodeNotApproved = '1';
                                        $scope.gifterrormsg = 'Quý khách chưa xác nhận mã giảm giá!';
                                        if (!$scope.$$phase)
                                            $scope.$apply();
                                        return;
                                    }
                                    $.ajax({
                                        url: "../../service/valid-gift-code",
                                        data: {
                                            addData: "WATERBILLING-" + $scope.cusmobile,
                                            giftcode: $scope.giftcode,
                                            price: $scope.amount
                                        },
                                        type: "POST",
                                        success: function (data) {
                                            if (data.Code === '00') {
                                                $scope.isGiftCodeNotApproved = '0';
                                                $scope.gifterrormsg = '';
                                                $scope.panelStatus = 2;
                                                $scope.amountpay = $scope.amount - $scope.giftcodevalue;
                                                if ($scope.amountpay < 0) {
                                                    $scope.amountpay = 0;
                                                }
                                                $scope.amountpaytemp = $scope.amountpay;
                                            } else if (data.Code === '12') {
                                                $scope.giftcode = '';
                                                $scope.isGiftCodeValid = '0';
                                                $scope.isGiftCodeNotValid = '0';
                                                $scope.gifterrormsg = '';
                                                $scope.errormsg = "Mã Gift Code bị khóa hoặc hết hạn. Vui lòng liên hệ ĐT hỗ trợ 1900 5555 77";

                                                if (!$scope.$$phase)
                                                    $scope.$apply();
                                                $('#error-modal').modal();

                                                return false;
                                            }
                                            else {
                                                $scope.giftcode = '';
                                                $scope.isGiftCodeValid = '0';
                                                $scope.isGiftCodeNotValid = '0';
                                                $scope.gifterrormsg = '';
                                                $scope.amountpay = $scope.amount;
                                                $scope.amountpaytemp = $scope.amountpay;
                                                $scope.giftcodevalue = 0;
                                                $scope.panelStatus = 2;
                                            }
                                            if (!$scope.$$phase)
                                                $scope.$apply();
                                            $('html, body').animate({
                                                scrollTop: 0
                                            }, 200);

                                        },
                                        error: ""
                                    });

                                } else { // Don't Use GiftCode
                                    $scope.isGiftCodeNotValid = "0";
                                    $scope.isGiftCodeNotApproved = '0';
                                    $scope.gifterrormsg = '';

                                    //=================BEGIN QRCODE======================
                                    //$.ajax({
                                    //    url: "../../service/water-bill/gen-qrcode",
                                    //    data: {
                                    //        billcode: encodeURIComponent($scope.billcode),
                                    //        providerid: $scope.provider,
                                    //        cusmobile: $scope.cusmobile,
                                    //        giftcode: $scope.giftcode
                                    //    },
                                    //    type: "POST",
                                    //    beforeSend: function () {
                                    //        //$("#service-loader-wrapper").css("display", "");
                                    //    },
                                    //    complete: function () {
                                    //        //$("#service-loader-wrapper").css("display", "none");
                                    //    },
                                    //    success: function (data) {
                                    //        console.log(data);
                                    //        if (data.Code === "00") {
                                    //            $scope.qrimg = data.Data;

                                    //            //create connection notify
                                    //            console.log(data.txnId);
                                    //            var notify = $.connection('/notify');
                                    //            notify.qs = { orderid: data.txnId };
                                    //            notify.received(function (data) {
                                    //                try {
                                    //                    var msg = JSON.parse(data);
                                    //                    console.log(msg);
                                    //                    switch (msg.message) {
                                    //                        case "confirm":
                                    //                            notify.stop();
                                    //                            top.location.href = msg.data;
                                    //                            break;
                                    //                    }
                                    //                } catch (e) {
                                    //                    if (window.console) {
                                    //                        window.console.log(e);
                                    //                    }
                                    //                }
                                    //            });
                                    //            notify.start();
                                    //        }
                                    //        if (!$scope.$$phase)
                                    //            $scope.$apply();
                                    //    },
                                    //    error: function () {
                                    //    }
                                    //});
                                    //=================END QRCODE======================

                                    $scope.panelStatus = 2;
                                    $scope.amountpay = $scope.amount;
                                    $scope.amountpaytemp = $scope.amountpay;
                                    if (typeof $scope.giftcode === 'undefined') {
                                        $scope.giftcode = '';
                                    }
                                    if (!$scope.$$phase)
                                        $scope.$apply();
                                    $('html, body').animate({
                                        scrollTop: 0
                                    }, 200);

                                }

                                //Dong cac tab lai, chi mo cac tab the noi dia
                                $("#tab1").collapse('show');
                                $("#tab2").collapse('hide');
                                $("#tab3").collapse('hide');
                                //$('.radio-big-info').css('display', 'none');

                            } else {
                                $scope.errormsg = data.Data;
                                if (!$scope.$$phase)
                                    $scope.$apply();
                                $('#error-modal').modal();
                            }
                        },
                        error: ""
                    });
                }

            }
        });
    };

    $scope.backButton = function () {
        $scope.panelStatus = 1;
        $scope.payMethod = "";
        $scope.feetnx = 0;

        $scope.capchakey = "";
        $scope.GenCap();

        //clear visa input
        $scope.visaCusName = "";
        $scope.visaCusEmail = "";
        $scope.visaCusMobile = "";
        $scope.visaCusAddress = "";
        $("input").siblings('label.required').show();
        if (!$scope.$$phase)
            $scope.$apply();
        $('html, body').animate({
            scrollTop: 0
        }, 200);
        $(".affix-wrap").removeClass('affix');
        $(".affix-wrap").removeClass('affix-bottom');
    }

    $scope.payNow = function () {
        //Check If Payment method is empty and don't use giftcode
        if ((typeof $scope.payMethod === 'undefined' || $scope.payMethod === "") && $scope.amountpay !== 0) {
            $('#payment-modal').modal();
            return false;
        } else {
            //Check If Visa and Master
            if ($scope.payMethod === "VISAMASTER") {
                var steptwovalidator = $('#steptwoform').validate({
                    ignore: ":hidden",
                    rules: {
                        visaCusName: {
                            required: true,
                            minlength: 3,
                            maxlength: 70
                        },
                        visaCusEmail: {
                            required: true,
                            regx: /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
                        },
                        visaCusMobile: {
                            required: true,
                            regx: /^(096|097|098|0162|0163|0164|0165|0166|0167|0168|0169|086|032|033|034|035|036|037|038|039|0129|0127|0125|0124|0123|094|091|088|081|082|083|084|085|090|093|0120|0121|0122|0126|0128|089|070|076|077|078|079|092|0188|0186|056|058|099|0199|059)\d{7}$/
                        },
                        visaCusAddress: {
                            required: true,
                            minlength: 5,
                            maxlength: 50
                        }
                    },
                    messages: {
                        visaCusName: {
                            required: "Vui lòng nhập đầy đủ tên người thanh toán.",
                            minlength: "Họ tên tối thiểu 3 ký tự",
                            maxlength: "Họ tên tối đa 70 ký tự"
                        },
                        visaCusEmail: {
                            required: "Vui lòng nhập email.",
                            regx: "Email không đúng định dạng"
                        },
                        visaCusMobile: {
                            required: "Vui lòng nhập số điện thoại.",
                            regx: "Số thuê bao không hợp lệ"
                        },
                        visaCusAddress: {
                            required: "Vui lòng nhập địa chỉ khách hàng.",
                            minlength: "Địa chỉ tối thiểu 5 ký tự",
                            maxlength: "Địa chỉ không vượt quá 50 ký tự"
                        }
                    },
                    submitHandler: function (form) {
                        if ($scope.payMethod === "VISAMASTER") {

                            //Check CustomerInformation Again (when tab is closed)
                            if ($scope.visaCusName === '' || typeof $scope.visaCusName === 'undefined'
                                || $scope.visaCusEmail === '' || typeof $scope.visaCusEmail === 'undefined'
                                || $scope.visaCusMobile === '' || typeof $scope.visaCusMobile === 'undefined'
                                || $scope.visaCusAddress === '' || typeof $scope.visaCusAddress === 'undefined') {
                                $scope.errormsg = "Phương thức thanh toán quốc tế yêu cầu nhập đủ thông tin liên hệ, xin cảm ơn!";
                                if (!$scope.$$phase)
                                    $scope.$apply();
                                $('#error-modal').modal();
                                $("#tab2").collapse('show');
                                return;
                            }

                            if (typeof $scope.visaCusDistrict === 'undefined') {
                                $scope.visaCusDistrict = 0;
                            }

                            $.ajax({
                                url: "../../service/water-bill/execute",
                                data: {
                                    billcode: encodeURIComponent($scope.billcode),
                                    providerid: $scope.provider,
                                    cusmobile: $scope.cusmobile,
                                    bankcode: $scope.payMethod,
                                    giftcode: $scope.giftcode,
                                    visaCusName: $scope.visaCusName,
                                    visaCusEmail: encodeURIComponent($scope.visaCusEmail),
                                    visaCusMobile: $scope.visaCusMobile,
                                    visaCusAddress: encodeURIComponent($scope.visaCusAddress),
                                    visaCusCity: $scope.visaCusCity,
                                    visaCusDistrict: $scope.visaCusDistrict,
                                    captcha: $scope.capchakey,
                                    hash: $scope.CapHash
                                },
                                type: "POST",
                                beforeSend: function () {
                                    $("#service-loader-wrapper").css("display", "");
                                },
                                complete: function () {
                                    $("#service-loader-wrapper").css("display", "none");
                                },
                                success: function (data) {
                                    if (data.Code === "00") {
                                        if (data.GiftCode === "1") {
                                            window.location.replace(data.Data);
                                        } else {
                                            vnpay.open({ width: 768, height: 600, url: data.Data });
                                        }

                                    } else {
                                        if (data.GiftCode === "1") {
                                            $scope.errormsg =
                                                "Có lỗi xảy ra trong quá trình xử lý giao dịch, Quý khách vui lòng gọi 1900 5555 77 để được hỗ trợ";
                                        } else {
                                            $scope.errormsg = data.Data;
                                        }
                                        if (!$scope.$$phase)
                                            $scope.$apply();
                                        $('#error-modal').modal();
                                    }
                                    if (!$scope.$$phase)
                                        $scope.$apply();
                                },
                                error: function () {
                                    $scope.errormsg = "Có lỗi xảy ra trong quá trình xử lý giao dịch, Quý khách vui lòng gọi 1900 5555 77 để được hỗ trợ";
                                    if (!$scope.$$phase)
                                        $scope.$apply();
                                    $('#error-modal').modal();
                                }
                            });
                        }
                    }
                });
                return false;
            } else {
                if (typeof $scope.visaCusDistrict === 'undefined') {
                    $scope.visaCusDistrict = 0;
                }
                $.ajax({
                    url: "../../service/water-bill/execute",
                    data: {
                        billcode: encodeURIComponent($scope.billcode),
                        providerid: $scope.provider,
                        cusmobile: $scope.cusmobile,
                        bankcode: $scope.payMethod,
                        giftcode: $scope.giftcode,
                        visaCusName: "",
                        visaCusEmail: "",
                        visaCusMobile: "",
                        visaCusAddress: "",
                        visaCusCity: 0,
                        visaCusDistrict: 0,
                        captcha: $scope.capchakey,
                        hash: $scope.CapHash
                    },
                    type: "POST",
                    beforeSend: function () {
                        $("#service-loader-wrapper").css("display", "");
                    },
                    complete: function () {
                        $("#service-loader-wrapper").css("display", "none");
                    },
                    success: function (data) {
                        if (data.Code === "00") {
                            if (data.GiftCode === "1") {
                                window.location.replace(data.Data);
                            } else {

                                vnpay.open({ width: 768, height: 600, url: data.Data });
                            }

                        } else {
                            if (data.GiftCode === "1") {
                                $scope.errormsg =
                                    "Có lỗi xảy ra trong quá trình xử lý giao dịch, Quý khách vui lòng gọi 1900 5555 77 để được hỗ trợ";
                            } else {
                                $scope.errormsg = data.Data;
                            }
                            if (!$scope.$$phase)
                                $scope.$apply();
                            $('#error-modal').modal();
                        }
                        if (!$scope.$$phase)
                            $scope.$apply();
                    },
                    error: function () {
                        $scope.errormsg = "Có lỗi xảy ra trong quá trình xử lý giao dịch, Quý khách vui lòng gọi 1900 5555 77 để được hỗ trợ";
                        if (!$scope.$$phase)
                            $scope.$apply();
                        $('#error-modal').modal();
                    }
                });
            }
        }
    };

    //Custom
    $scope.ChoosenVisa = function () {
        if ($scope.amountpay > 0) {
            $.ajax({
                url: "../../service/mobile-bill/get-fee-tranx",
                data: {
                    input: $scope.amountpay
                },
                type: "POST",
                beforeSend: function () {
                },
                complete: function () {
                },
                success: function (data) {
                    if (data.Code === "00") {
                        $scope.feetnx = data.Data;
                        $scope.amountpaytemp = $scope.amountpay;
                        $scope.amountpay = $scope.amountpay + $scope.feetnx;
                    }
                    if (!$scope.$$phase)
                        $scope.$apply();
                },
                error: ""
            });
        }
    }

    $scope.ChoosenPayNotVisa = function () {
        $scope.amountpay = $scope.amountpaytemp;
        $scope.feetnx = 0;
        $scope.payMethod = "";
        if (!$scope.$$phase)
            $scope.$apply();
    }

    $scope.ClosePaymethod = function () {
        //$scope.payMethod = "";
        //$scope.amountpay = $scope.amountpaytemp;
        //$scope.feetnx = 0;
        if (!$scope.$$phase)
            $scope.$apply();
    };

    //Custom new payment method
    $scope.ChoosePayBack = function () {
        $scope.payMethod = "";
        if (!$scope.$$phase)
            $scope.$apply();
    }
    $scope.ChoosenVisaTab = function () {
        $scope.payMethod = "VISAMASTER";
        if ($scope.amountpay > 0) {
            $.ajax({
                url: "../../service/mobile-bill/get-fee-tranx",
                data: {
                    input: $scope.amountpay
                },
                type: "POST",
                beforeSend: function () {
                },
                complete: function () {
                },
                success: function (data) {
                    if (data.Code === "00") {
                        $scope.feetnx = data.Data;
                        $scope.amountpaytemp = $scope.amountpay;
                        $scope.amountpay = $scope.amountpay + $scope.feetnx;
                    }
                    if (!$scope.$$phase)
                        $scope.$apply();
                },
                error: ""
            });
        }
        $('.navcard-btn').css('display', 'none');
        $('.banks2').css('display', 'block');
        if (!$scope.$$phase)
            $scope.$apply();
    };
    $scope.ChoosenAtmTab = function () {
        $scope.payMethod = "";
        $scope.amountpay = $scope.amountpaytemp;
        $scope.feetnx = 0;
        $('.navcard-btn').css('display', 'none');
        $('.banks1').css('display', 'block');
        if (!$scope.$$phase)
            $scope.$apply();
    };
    $scope.ChoosenVnpayQrTab = function () {
        $scope.payMethod = "VNPAYQR";
        $scope.payNow();
        if (!$scope.$$phase)
            $scope.$apply();
    };
    $scope.ChoosenVnmartTab = function () {
        $scope.payMethod = "VNMART";
        $scope.payNow();
        if (!$scope.$$phase)
            $scope.$apply();
    };
}]);