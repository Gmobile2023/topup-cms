//=============================================================================================
//                              CARDSHOPPING CONTROLLER
//=============================================================================================
topupApp.controller('CardShoppingController', ["$scope", "$locale", function ($scope, $locale) {
    //==================================GLOBAL METHOD===============================================
    //Global Init value
    $locale.NUMBER_FORMATS.GROUP_SEP = '.';
    $scope.panelStatus = 1;
    $scope.isReadTerm = true;
    $scope.payMethod = "";
    $scope.CityArr = [];
    $scope.DistrictArr = [];
    $scope.giftcode = '';
    $scope.isGiftCodeValid = '0';
    $scope.isGiftCodeNotValid = '0';
    $scope.isGiftCodeNotApproved = '0';
    $scope.giftcodevalue = 0;
    $scope.amountpay = -1;
    $scope.gifterrormsg = '';

    //Custom Init
    //$scope.cardPrice = 100000;
    //$scope.cardSellPrice = $scope.cardPrice - ($scope.cardPrice * 3 / 100);
    //$scope.vendorid = "MB";
    //$scope.vendorname = "Mobifone";
    //$scope.productid = "MBC100";
    $scope.cardquantity = 1;
    $scope.CardDataAccess = [];
    $scope.tabactive = "mobile";

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
            //if ($scope.giftcode !== '') {
            $.ajax({
                url: "../../service/valid-gift-code",
                data: {
                    addData: encodeURIComponent("CARDSHOPPING-" + $scope.receiveremail),
                    giftcode: $scope.giftcode,
                    price: $scope.cardSellPrice * $scope.cardquantity
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
            //    } else {
            //        $scope.isGiftCodeNotValid = '1';
            //        if (!$scope.$$phase)
            //            $scope.$apply();
            //    }
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

    function isInt(value) {
        return !isNaN(value) &&
               parseInt(Number(value)) === value &&
               !isNaN(parseInt(value, 10));
    }

    $scope.nextToPay = function () {
        $('#steponeForm').submit(function (e) {
            e.preventDefault();
        }).validate({
            rules: {
                cardquantity: {
                    required: true,
                    max: 10,
                    min: 1
                },
                receiveremail: {
                    required: true,
                    regx: /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
                }
            },
            messages: {
                cardquantity: {
                    required: "Vui lòng nhập số lượng!",
                    max: "Bạn chỉ có thể mua tối đa 10 thẻ trong một lần mua",
                    min: "Bạn phải nhập tối thiểu 1 thẻ"
                },
                receiveremail: {
                    required: "Vui lòng nhập địa chỉ Email!",
                    regx: "Email không đúng định dạng"
                }
            },
            submitHandler: function (form) {
                // do other things for a valid form
                if (!$scope.isReadTerm) {
                    $('#term-modal').modal();
                } else if (!isInt($scope.cardquantity)) {
                    $scope.errormsg = "Số lượng thẻ không hợp lệ";
                    if (!$scope.$$phase)
                        $scope.$apply();
                    $('#error-modal').modal();
                }
                else if ($scope.vendorid === '' || $scope.vendorid === 'undefined') {
                    $scope.errormsg = "Quý khách vui lòng chọn nhà cung cấp";
                    if (!$scope.$$phase)
                        $scope.$apply();
                    $('#error-modal').modal();
                }
                else if ($scope.productid === '' || $scope.productid === 'undefined') {
                    $scope.errormsg = "Quý khách vui lòng chọn mệnh giá thẻ";
                    if (!$scope.$$phase)
                        $scope.$apply();
                    $('#error-modal').modal();
                }
                else {
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
                                addData: encodeURIComponent("CARDSHOPPING-" + $scope.receiveremail),
                                giftcode: $scope.giftcode,
                                price: $scope.cardSellPrice * $scope.cardquantity
                            },
                            type: "POST",
                            beforeSend: function () {
                            },
                            complete: function () {
                            },
                            success: function (data) {
                                if (data.Code === '00') {
                                    $scope.isGiftCodeNotApproved = '0';
                                    $scope.gifterrormsg = '';
                                    $scope.panelStatus = 2;
                                    $scope.amountpay = ($scope.cardSellPrice * $scope.cardquantity) - $scope.giftcodevalue;
                                    if ($scope.amountpay < 0) {
                                        $scope.amountpay = 0;
                                    }
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
                                    $scope.amountpay = $scope.cardSellPrice * $scope.cardquantity;
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
                        $scope.panelStatus = 2;
                        $scope.amountpay = $scope.cardSellPrice * $scope.cardquantity;
                        if (typeof $scope.giftcode === 'undefined') {
                            $scope.giftcode = '';
                        }
                        if (!$scope.$$phase)
                            $scope.$apply();
                        $('html, body').animate({
                            scrollTop: 0
                        }, 200);
                    }
                }
            }
        });
    };

    $scope.backButton = function () {
        $scope.panelStatus = 1;
        //clear visa input
        $scope.visaCusName = "";
        $scope.visaCusEmail = "";
        $scope.visaCusMobile = "";
        $scope.visaCusAddress = "";
        $("input").siblings('label.required').show();
        $scope.payMethod = "";
        if (!$scope.$$phase)
            $scope.$apply();
        $('html, body').animate({
            scrollTop: 0
        }, 200);
    }

    $scope.payNow = function () {
        //Check If Payment method is empty
        if ((typeof $scope.payMethod === 'undefined' || $scope.payMethod === "") && $scope.amountpay !== 0) {
            $('#payment-modal').modal();
            return false;
        }

        //Check If Visa and Master
        if ($scope.payMethod === "VISAMASTER") {
            $('#steptwoform').submit(function (e) {
                e.preventDefault();
            }).validate({
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
                        regx: "Số điện thoại không đúng định dạng"
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
                            url: "../../service/shoppingcard/execute",
                            data: {
                                emailreceiver: encodeURIComponent($scope.receiveremail),
                                cardquantity: $scope.cardquantity,
                                productid: $scope.productid,
                                bankcode: $scope.payMethod,
                                giftcode: $scope.giftcode,
                                visaCusName: $scope.visaCusName,
                                visaCusEmail: encodeURIComponent($scope.visaCusEmail),
                                visaCusMobile: $scope.visaCusMobile,
                                visaCusAddress: encodeURIComponent($scope.visaCusAddress),
                                visaCusCity: $scope.visaCusCity,
                                visaCusDistrict: $scope.visaCusDistrict
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
                url: "../../service/shoppingcard/execute",
                data: {
                    emailreceiver: encodeURIComponent($scope.receiveremail),
                    cardquantity: $scope.cardquantity,
                    productid: $scope.productid,
                    bankcode: $scope.payMethod,
                    giftcode: $scope.giftcode,
                    visaCusName: "",
                    visaCusEmail: "",
                    visaCusMobile: "",
                    visaCusAddress: "",
                    visaCusCity: 0,
                    visaCusDistrict: 0
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
    };

    //===================================CUSTOM============================
    $scope.ChangeVendor = function (value, vendorid) {
        $scope.vendorname = value;
        $.ajax({
            url: "../../service/shoppingcard/get-card-product",
            data: {
                vendorid: vendorid
            },
            type: "POST",
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (data) {
                var lazy = Lazy(data.Data);
                var card100 = lazy.filter(function (x) {
                    return (x.Price === 100000);
                }).first();
                if (typeof card100 === 'undefined') {
                    card100 = lazy.filter(function (x) {
                        return (x.Price === 50000);
                    }).first();
                }
                if (typeof card100 === 'undefined') {
                    card100 = lazy.filter(function (x) {
                        return (x.Price === 500000);
                    }).first();
                }
                if (typeof card100 === 'undefined') {
                    card100 = lazy.filter(function (x) {
                        return (x.Price === 10000);
                    }).first();
                }
                if (typeof card100 === 'undefined') {
                    card100 = lazy.filter(function (x) {
                        return (x.Price === 20000);
                    }).first();
                }
                if (typeof card100 === 'undefined') {
                    card100 = lazy.filter(function (x) {
                        return (x.Price === 200000);
                    }).first();
                }
                if (typeof card100 === 'undefined') {
                    card100 = lazy.filter(function (x) {
                        return (x.Price === 300000);
                    }).first();
                }
                $scope.productid = card100.ProductId;
                $scope.cardPrice = card100.Price;
                $scope.cardSellPrice = card100.Price - (card100.Price * card100.DiscountValue / 100);
                if (!$scope.$$phase)
                    $scope.$apply();
                if ($('.form-group-radio input[type="radio"]').is(':not(:checked)')) {
                    $('.form-group-radio input[type="radio"]').parent('label').removeClass('checked');
                }
                if ($('.form-group-radio input[type="radio"]').is(':checked')) {
                    $('.form-group-radio input[type="radio"]:checked').closest('label').addClass('checked');
                }
            },
            error: ""
        });
        if (!$scope.$$phase)
            $scope.$apply();
    }

    $scope.ChangeProduct = function (price, discount) {
        $scope.cardPrice = price;
        $scope.cardSellPrice = price - (price * discount / 100);
        if (!$scope.$$phase)
            $scope.$apply();
    }

    $scope.ViewCard = function () {
        $('#frmAccess').submit(function (e) {
            e.preventDefault();
        }).validate({
            rules: {
                accesscode: {
                    required: true
                }
            },
            messages: {
                accesscode: {
                    required: "Yêu cầu nhập!"
                }
            },
            submitHandler: function (form) {
                $.ajax({
                    url: "../../service/shoppingcard/viewcard",
                    data: {
                        accesscode: $scope.accesscode
                    },
                    type: "POST",
                    beforeSend: function () {
                        $("#service-loader-wrapper").css("display", "");
                    },
                    complete: function () {
                        $("#service-loader-wrapper").css("display", "none");
                    },
                    success: function (data) {
                        $scope.CardDataAccess = data;
                        if (data.Code !== "00") {
                            $('#error-modal').modal();
                        }
                        if (!$scope.$$phase)
                            $scope.$apply();
                    },
                    error: ""
                });
            }
        });
    }

    var swiper3 = new Swiper('.card-swiper-container', {
        slidesPerView: 4,
        spaceBetween: 30,
        preventClicks: false,
        preventClicksPropagation: false,
        nextButton: '.swiper-button-next-card1',
        prevButton: '.swiper-button-prev-card1',
        breakpoints: {
            1200: {
                slidesPerView: 4,
            },
            1024: {
                slidesPerView: 3,
                spaceBetween: 0
            },
            768: {
                slidesPerView: 3,
                spaceBetween: 30
            },
            640: {
                slidesPerView: 3,
                spaceBetween: 20
            },
            320: {
                slidesPerView: 2,
                spaceBetween: 10
            }
        }
    }); //swiper tab1 05_Mua_ma_the_1.html
    var swiper4 = new Swiper('.card-swiper-container-2', {
        slidesPerView: 4,
        spaceBetween: 30,
        preventClicks: false,
        preventClicksPropagation: false,
        nextButton: '.swiper-button-next-card2',
        prevButton: '.swiper-button-prev-card2',
        breakpoints: {
            1200: {
                slidesPerView: 4,
            },
            1024: {
                slidesPerView: 3,
                spaceBetween: 0
            },
            768: {
                slidesPerView: 3,
                spaceBetween: 30
            },
            640: {
                slidesPerView: 3,
                spaceBetween: 20
            },
            320: {
                slidesPerView: 2,
                spaceBetween: 10
            }
        }
    }); //swiper tab2 05_Mua_ma_the_1.html
    var swiper42 = new Swiper('.card-swiper-container-3', {
        slidesPerView: 4,
        spaceBetween: 30,
        preventClicks: false,
        preventClicksPropagation: false,
        nextButton: '.swiper-button-next-card3',
        prevButton: '.swiper-button-prev-card3',
        breakpoints: {
            1200: {
                slidesPerView: 4,
            },
            1024: {
                slidesPerView: 3,
                spaceBetween: 0
            },
            768: {
                slidesPerView: 3,
                spaceBetween: 30
            },
            640: {
                slidesPerView: 3,
                spaceBetween: 20
            },
            320: {
                slidesPerView: 2,
                spaceBetween: 10
            }
        }
    }); //swiper tab3 05_Mua_ma_the_1.html

    $scope.CheckActivePanel = function () {
        setTimeout(function () {
            $('.navbar-card li.active:nth-of-type(1)').closest('ul').find('.navbar-card__slider').css('left', '0%');
            $('.card-data').css('display', 'none');
            swiper3.update(); //swiper tab1 05_Mua_ma_the_1.html
            swiper42.update();
            $('.ripple').remove();
            $('.navbar-card li.active:nth-of-type(2)').closest('ul').find('.navbar-card__slider').css({ 'left': '100%', 'border-radius': '0' });
            $('.navbar-card li.active:nth-of-type(1)').closest('ul').find('.navbar-card__slider').css({ 'left': '0%', 'border-radius': '4px 0 0 4px' });
            $('.navbar-card li.active:nth-of-type(3)').closest('ul').find('.navbar-card__slider').css({ 'left': '200%', 'border-radius': '0px 4px 4px 0' });
            var id = $('.tab-content .active').attr('id');
            if (id == "menu3") {
                $('.card-data').css('display', 'block');
            } else {
                $('.card-data').css('display', 'none');
            }
        }, 0);

        var tab = getParameterByName("tab");
        if (tab === "game") {
            $scope.tabactive = "game";
            $scope.vendorid = "GA";
            $scope.vendorname = "Garena";
            $('.card-data').css('display', 'none');
        } else if (tab === "datamg") {
            $scope.tabactive = "datamg";
            $scope.vendorid = "GV";
            $scope.vendorname = "Viettel Data 3G-4G";
            $('.card-data').css('display', '');
        } else {
            $scope.tabactive = "mobile";
            $scope.vendorid = "MB";
            $scope.vendorname = "Mobifone";
            $('.card-data').css('display', 'none');
        }

        $.ajax({
            url: "../../service/shoppingcard/get-card-product",
            data: {
                vendorid: $scope.vendorid
            },
            type: "POST",
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (data) {
                var lazy = Lazy(data.Data);
                var card100 = lazy.filter(function (x) {
                    return (x.Price === 100000);
                }).first();
                if (typeof card100 === 'undefined') {
                    card100 = lazy.filter(function (x) {
                        return (x.Price === 50000);
                    }).first();
                }
                if (typeof card100 === 'undefined') {
                    card100 = lazy.filter(function (x) {
                        return (x.Price === 500000);
                    }).first();
                }
                if (typeof card100 === 'undefined') {
                    card100 = lazy.filter(function (x) {
                        return (x.Price === 10000);
                    }).first();
                }
                if (typeof card100 === 'undefined') {
                    card100 = lazy.filter(function (x) {
                        return (x.Price === 20000);
                    }).first();
                }
                if (typeof card100 === 'undefined') {
                    card100 = lazy.filter(function (x) {
                        return (x.Price === 200000);
                    }).first();
                }
                if (typeof card100 === 'undefined') {
                    card100 = lazy.filter(function (x) {
                        return (x.Price === 300000);
                    }).first();
                }
                $scope.productid = card100.ProductId;
                $scope.cardPrice = card100.Price;
                $scope.cardSellPrice = card100.Price - (card100.Price * card100.DiscountValue / 100);
                if (!$scope.$$phase)
                    $scope.$apply();
                if ($('.form-group-radio input[type="radio"]').is(':not(:checked)')) {
                    $('.form-group-radio input[type="radio"]').parent('label').removeClass('checked');
                }
                if ($('.form-group-radio input[type="radio"]').is(':checked')) {
                    $('.form-group-radio input[type="radio"]:checked').closest('label').addClass('checked');
                }
            },
            error: ""
        });
    }

    $scope.ChangeActiveForm = function (value) {
        setTimeout(function () {
            $('.navbar-card li.active:nth-of-type(1)').closest('ul').find('.navbar-card__slider').css('left', '0%');
            $('.card-data').css('display', 'none');
            swiper3.update(); //swiper tab1 05_Mua_ma_the_1.html
            swiper42.update();
            $('.ripple').remove();
            $('.navbar-card li.active:nth-of-type(2)').closest('ul').find('.navbar-card__slider').css({ 'left': '100%', 'border-radius': '0px 4px 4px 0' });
            $('.navbar-card li.active:nth-of-type(1)').closest('ul').find('.navbar-card__slider').css({ 'left': '0', 'border-radius': '4px 0 0 4px' });
            $('.navbar-card li.active:nth-of-type(3)').closest('ul').find('.navbar-card__slider').css({ 'left': '200%', 'border-radius': '0px 4px 4px 0' });
            var id = $('.tab-content .active').attr('id');
            if (id == "menu3") {
                $('.card-data').css('display', 'block');
            } else {
                $('.card-data').css('display', 'none');
            }
        }, 0);

        $scope.tabactive = value;
        if (value === "game") {
            $scope.vendorid = "GA";
            $scope.vendorname = "Garena";
            $('.card-data').css('display', 'none');
        }
        if (value === "mobile") {
            $scope.vendorid = "MB";
            $scope.vendorname = "Mobifone";
            $('.card-data').css('display', 'none');
        }
        if (value === "datamg") {
            $scope.vendorid = "GV";
            $scope.vendorname = "Viettel Data 3G-4G";
            $('.card-data').css('display', '');
        }

        $.ajax({
            url: "../../service/shoppingcard/get-card-product",
            data: {
                vendorid: $scope.vendorid
            },
            type: "POST",
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (data) {
                var lazy = Lazy(data.Data);
                var card100 = lazy.filter(function (x) {
                    return (x.Price === 100000);
                }).first();
                if (typeof card100 === 'undefined') {
                    card100 = lazy.filter(function (x) {
                        return (x.Price === 50000);
                    }).first();
                }
                if (typeof card100 === 'undefined') {
                    card100 = lazy.filter(function (x) {
                        return (x.Price === 500000);
                    }).first();
                }
                if (typeof card100 === 'undefined') {
                    card100 = lazy.filter(function (x) {
                        return (x.Price === 10000);
                    }).first();
                }
                if (typeof card100 === 'undefined') {
                    card100 = lazy.filter(function (x) {
                        return (x.Price === 20000);
                    }).first();
                }
                if (typeof card100 === 'undefined') {
                    card100 = lazy.filter(function (x) {
                        return (x.Price === 200000);
                    }).first();
                }
                if (typeof card100 === 'undefined') {
                    card100 = lazy.filter(function (x) {
                        return (x.Price === 300000);
                    }).first();
                }
                $scope.productid = card100.ProductId;
                $scope.cardPrice = card100.Price;
                $scope.cardSellPrice = card100.Price - (card100.Price * card100.DiscountValue / 100);
                if (!$scope.$$phase)
                    $scope.$apply();
                if ($('.form-group-radio input[type="radio"]').is(':not(:checked)')) {
                    $('.form-group-radio input[type="radio"]').parent('label').removeClass('checked');
                }
                if ($('.form-group-radio input[type="radio"]').is(':checked')) {
                    $('.form-group-radio input[type="radio"]:checked').closest('label').addClass('checked');
                }
            },
            error: ""
        });
    }

    $scope.ClosePaymethod = function () {
        //$scope.payMethod = "";
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
        $('.navcard-btn').css('display', 'none');
        $('.banks2').css('display', 'block');
        if (!$scope.$$phase)
            $scope.$apply();
    };
    $scope.ChoosenAtmTab = function () {
        $scope.payMethod = "";
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