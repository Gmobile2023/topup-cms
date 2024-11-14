//=============================================================================================
//                              VISADIRECT CONTROLLER
//=============================================================================================
topupApp.controller('VisaDirectController', ["$scope", "$locale", function ($scope, $locale) {
    //==================================GLOBAL METHOD===============================================
    //Global Init value
    $locale.NUMBER_FORMATS.GROUP_SEP = '.';
    $scope.panelStatus = 1;
    $scope.isReadTerm = true;
    $scope.payMethod = "";
    $scope.CityArr = [];
    $scope.DistrictArr = [];
    $scope.amountpay = -1;

    //Custom Init
    $scope.cardmask = "";
    $scope.msg = "";
    $scope.bankreleased = "";
    $scope.cardtype = "";
    $scope.visafee = 0;
    $scope.invalidcaptcha = "0";
    $scope.feetnx = 0;
    $scope.isvisa = 0;
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

    $scope.getIconBank = function () {
        var initCheck = $scope.cuscard;
        // console.log(initCheck);
        //var vcb = /^(412946|412947|412975|412976|412977|428310|436762|461136|469173|469174)\d{10}$/;
        //var acb = /^(404169|421494|422109|426926|436599|479138|479139|479140)\d{10}$/;
        //var tpbank = /^(466582|466583|466584|466585|468951|470970)\d{10}$/;
        //var scb = /^(426671|405573)\d{10}$/;
        //var eximbank = /^(404152|418159|422094|423960|436308|436309|437422|437423|452999|457485|469655)\d{10}$/;
        //var icb = /^(404184|404185|433286|439198|457271|457327|459804|459817|469374| 470570|474813|474814|474815| 474825| 486862|486863|498794|498795)\d{10}$/;
        //var bidv = /^(406220|406771|411153|427126|428695|476632|476633)\d{10}$/;

        // var visa = /^(412946|412947|412975|412976|412977|428310|436762|461136|469173|469174|404169|421494|422109|426926|436599|479138|479139|479140|426671|405573|404152|418159|422094|423960|436308|436309|437422|437423|452999|457485|469655|404184|404185|433286|439198|457271|457327|459804|459817|469374|470570|474813|474814|474815|474825|486862|486863|498794|498795|406220|406771|411153|427126|428695|476632|476633|437767|437768|486279|486280|486282|486283|40152|421093|422151|426088|429418|436361|436438|436445|437508|457573|461137|461138|461140|464932|466243|467964|468789|468945|468946|468947|468948|469654|472074|472075|478082|486265|422075|422076|422149|422150|429683|450798|457353|478097|414152|419661|419662|431742|452057|471249|490240|490241|428472|408906|469682|469683|469684|469685|486383|408904|408905|472674|484803|484804|484805|484806|442415|442416|467078|467079|470548|470549|457328|458584|461932|483542|436467|436468|436472|457526|457560|457561|405082|436545|436546|436547|436548|437420|437421|450180|450181|476636|437422|437423|403013|469672|469673|469674|469676|469678|437893|437894|437895|466236|466238|466582|466583|466584|466585|468951|47097|416259|450482|498766|498767|498768|498769|406598|413534|413535|418248|459220|459221|404648|413515|413516|413517|480237|421194|421193|429389|437841|445093|445094|458440|458761)\d{10}$/;
        //var isvcb = vcb.test(initCheck);
        //if (isvcb) {
        //    $("#ico_vcb").css("display", "");
        //} else {
        //    $("#ico_vcb").css("display", "none");
        //};
        //var isacb = acb.test(initCheck);
        //if (isacb) {
        //    $("#ico_acb").css("display", "");
        //} else {
        //    $("#ico_acb").css("display", "none");
        //};
        //var istpbank = tpbank.test(initCheck);
        //if (istpbank) {
        //    $("#ico_tpbank").css("display", "");
        //} else {
        //    $("#ico_tpbank").css("display", "none");
        //};
        //var isTestscb = scb.test(initCheck);
        //if (isTestscb) {
        //    $("#ico-scb").css("display", "");
        //} else {
        //    $("#ico_scb").css("display", "none");
        //};

        //var isTestexim = eximbank.test(initCheck);
        //if (isTestexim) {
        //    $("#ico-exim").css("display", "");
        //} else {
        //    $("#ico_exim").css("display", "none");
        //};
        //var isTesticb = icb.test(initCheck);
        //if (isTesticb) {
        //    $("#ico-icb").css("display", "");
        //} else {
        //    $("#ico_icb").css("display", "none");
        //};
        //var isTesbidv = bidv.test(initCheck);
        //if (isTesbidv) {
        //    $("#ico-bidv").css("display", "");
        //} else {
        //    $("#ico_bidv").css("display", "none");
        //};
        var visa = /^[0-9]{16}$/;
        var master1 = /^5\d{15}$/;
        var master2 = /^222100\d{10}$/;
        var isvisa = visa.test(initCheck);
        var ismaster1 = master1.test(initCheck);
        var ismaster2 = master2.test(initCheck);
        console.log(ismaster1 + " " + ismaster2);
        if (isvisa) {
            if (ismaster1 === true || ismaster2 === true) {
                $("#ico_master").css("display", "");
                $("#ico_visa").css("display", "none");
            } else {
                $("#ico_visa").css("display", "");
                $("#ico_master").css("display", "none");
            }
        } else {
            $("#ico_master").css("display", "none");
            $("#ico_visa").css("display", "none");
        }
    }

    $scope.showbank = function () {
        $('#bank-modal').modal();
    }

    $scope.nextToPay = function () {
        $('#steponeForm').submit(function (e) {
            e.preventDefault();
        }).validate({
            rules: {
                cuscard: {
                    required: true,
                    // regx: /^(412946|412947|412975|412976|412977|428310|436762|461136|469173|469174|404169|421494|422109|426926|436599|479138|479139|479140|426671|405573|404152|418159|422094|423960|436308|436309|437422|437423|452999|457485|469655|404184|404185|433286|439198|457271|457327|459804|459817|469374|470570|474813|474814|474815|474825|486862|486863|498794|498795|406220|406771|411153|427126|428695|476632|476633|437767|437768|486279|486280|486282|486283|40152|421093|422151|426088|429418|436361|436438|436445|437508|457573|461137|461138|461140|464932|466243|467964|468789|468945|468946|468947|468948|469654|472074|472075|478082|486265|422075|422076|422149|422150|429683|450798|457353|478097|414152|419661|419662|431742|452057|471249|490240|490241|428472|408906|469682|469683|469684|469685|486383|408904|408905|472674|484803|484804|484805|484806|442415|442416|467078|467079|470548|470549|457328|458584|461932|483542|436467|436468|436472|457526|457560|457561|405082|436545|436546|436547|436548|437420|437421|450180|450181|476636|437422|437423|403013|469672|469673|469674|469676|469678|437893|437894|437895|466236|466238|466582|466583|466584|466585|468951|47097|416259|450482|498766|498767|498768|498769|406598|413534|413535|418248|459220|459221|404648|413515|413516|413517|480237|421194|421193|429389|437841|445093|445094|458440|458761)\d{10}$/
                    regx: /^[0-9]{16}$/
                },
                amount: {
                    required: true
                },
                cusphone: {
                    required: true,
                    regx: /^(096|097|098|0162|0163|0164|0165|0166|0167|0168|0169|086|032|033|034|035|036|037|038|039|0129|0127|0125|0124|0123|094|091|087|088|081|082|083|084|085|090|093|0120|0121|0122|0126|0128|089|070|076|077|078|079|092|0188|0186|056|058|099|0199|059)\d{7}$/
                },
                capchakey: {
                    required: true
                }
            },
            messages: {
                cuscard: {
                    required: "Vui lòng nhập số thẻ!",
                    regx: "Số thẻ không hợp lệ"
                },
                amount: {
                    required: "Vui lòng nhập số tiền!"
                },
                cusphone: {
                    required: "Vui lòng nhập số điện thoại.",
                    regx: "Số điện thoại không hợp lệ"
                },
                capchakey: {
                    required: "Vui lòng nhập mã xác thực!"
                }
            },
            submitHandler: function (form) {
                // SAU KHI PASS VALIDATE
                if (!$scope.isReadTerm) {
                    $('#term-modal').modal();
                } else {
                    $.ajax({
                        url: "../../service/visadirect/check-captcha",
                        data: {
                            input: $scope.capchakey,
                            hash: $scope.CapHash
                        },
                        type: "POST",
                        beforeSend: function () {
                        },
                        complete: function () {
                        },
                        success: function (data) {
                            if (data.Code === "00") {
                                if ($scope.amount < 10000) {
                                    $scope.msg = "Vui lòng nhập số tiền lớn hơn hoặc bằng 10.000";
                                    if (!$scope.$$phase)
                                        $scope.$apply();
                                    $('#error-modal').modal();
                                    return;
                                }
                                $scope.invalidcaptcha = "0";
                                $.ajax({
                                    url: "../../service/visadirect/checkacount",
                                    data: {
                                        customerCode: $scope.cuscard,
                                        amount: $scope.amount
                                    },
                                    type: "POST",
                                    beforeSend: function () {
                                        $("#service-loader-wrapper").css("display", "");
                                    },
                                    complete: function () {
                                        $("#service-loader-wrapper").css("display", "none");
                                    },
                                    success: function (data) {
                                        if (data.sRespCode === "00") {
                                            $scope.cardmask = data.maskCard;
                                            $scope.cardtype = data.cardType;
                                            $scope.bankreleased = data.issuerName;
                                            if (data.cardType === "P") {
                                                $scope.cardtype = 'Thẻ Prepaid';
                                            } else if (data.cardType === "C") {
                                                $scope.cardtype = 'Thẻ Credit';
                                            } else if (data.cardType === "D") {
                                                $scope.cardtype = 'Thẻ Debit';
                                            } else {
                                                $scope.cardtype = 'Thẻ VISA';
                                            }

                                            //If check visa is Ok
                                            $scope.panelStatus = 2;
                                            $scope.amountpay = parseInt($scope.amount) + parseInt($scope.visafee);
                                            if (!$scope.$$phase)
                                                $scope.$apply();
                                            $('html, body').animate({
                                                scrollTop: 0
                                            }, 200);

                                        } else {
                                            $scope.cardmask = data.maskCard;
                                            $scope.cardtype = data.cardType;
                                            $scope.bankreleased = data.issuerName;
                                            if (data.cardType === "P") {
                                                $scope.cardtype = 'Thẻ Prepaid';
                                            } else if (data.cardType === "C") {
                                                $scope.cardtype = 'Thẻ Credit';
                                            } else if (data.cardType === "D") {
                                                $scope.cardtype = 'Thẻ Debit';
                                            } else {
                                                $scope.cardtype = 'Thẻ VISA';
                                            }
                                            $scope.msg = data.msg;
                                            if (!$scope.$$phase)
                                                $scope.$apply();
                                            $('#error-modal').modal();
                                        }
                                    },
                                    error: ""
                                });

                            } else {
                                $scope.invalidcaptcha = "1";
                                if (!$scope.$$phase)
                                    $scope.$apply();
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

        $scope.capchakey = "";
        $scope.GenCap();

        if (!$scope.$$phase)
            $scope.$apply();
        $('html, body').animate({
            scrollTop: 0
        }, 200);
    }

    $scope.payNow = function () {
        if ((typeof $scope.payMethod === 'undefined' || $scope.payMethod === "") && $scope.amountpay !== 0) {
            $('#payment-modal').modal();
            return false;
        }
        //Check If Visa and Master
        if ($scope.payMethod === "VISAMASTER") {
            $scope.msg = "Chỉ chấp nhận với phương thức thanh toán nội địa";
            $('#error-modal').modal();
            return false;
        } else {
            if (typeof $scope.visaCusDistrict === 'undefined') {
                $scope.visaCusDistrict = 0;
            }
            $.ajax({
                url: "../../service/visadirect/excute",
                data: {
                    captcha: $scope.capchakey,
                    hash: $scope.CapHash,
                    cusphone: $scope.cusphone,
                    amount: $scope.amount,
                    bankcode: $scope.payMethod,
                    numbercard: $scope.cuscard,
                    cardType: $scope.cardtype,
                    visaCusName: $scope.visaCusName,
                    visaCusEmail: $scope.visaCusEmail,
                    visaCusMobile: $scope.visaCusMobile,
                    visaCusAddress: $scope.visaCusAddress,
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
                        vnpay.open({width: 768, height: 600, url: data.Data});
                    } else {
                        $scope.msg = data.Data;
                        $('#error-modal').modal();
                    }
                    if (!$scope.$$phase)
                        $scope.$apply();
                },
                error: function () {
                    $scope.msg = "Có lỗi xảy ra trong quá trình xử lý giao dịch, Quý khách vui lòng gọi 1900 5555 77 để được hỗ trợ";
                    if (!$scope.$$phase)
                        $scope.$apply();
                    $('#error-modal').modal();
                }
            });
        }
    };

    $scope.processResult = function () {
        $("#service-loader-wrapper").css("display", "");
        $scope.issuccess = "0";
        $scope.isprocess = "1";
        if (!$scope.$$phase)
            $scope.$apply();
        if ($scope.payrspcode !== "00") {
            $scope.iserror = "1";
            $scope.isprocess = "0";
            if ($scope.payrspcode === "24") {
                $scope.orderError = "Giao dịch không thành công do Quý khách hủy thực hiện thanh toán";
            } else {
                $scope.orderError = "Có lỗi xảy ra trong quá trình xử lý giao dịch, Quý khách vui lòng gọi 1900 5555 77 để được hỗ trợ";
            }
            $("#service-loader-wrapper").css("display", "none");
            if (!$scope.$$phase)
                $scope.$apply();
        }
        setTimeout(function () {
            if ($scope.payrspcode === "00") {
                $.ajax({
                    url: "../../service/visadirect/check-order",
                    data: {
                        visadata: $scope.visadata,
                        visasign: $scope.visasign
                    },
                    type: "POST",
                    beforeSend: function () {
                        $("#service-loader-wrapper").css("display", "");
                    },
                    complete: function () {
                        $("#service-loader-wrapper").css("display", "none");
                        $scope.isprocess = "0";
                        if (!$scope.$$phase)
                            $scope.$apply();
                    },
                    success: function (data) {
                        if (data.Code === "00" || data.Code === "082") {
                            $scope.issuccess = "1";
                        } else {
                            $scope.iserror = "1";
                            $scope.orderError = data.Data;
                        }
                        if (!$scope.$$phase)
                            $scope.$apply();
                    },
                    error: ""
                });
            } else {
                $scope.iserror = "1";
                $scope.isprocess = "0";
            }

            $("#service-loader-wrapper").css("display", "none");

            if (!$scope.$$phase)
                $scope.$apply();
        }, 60000);
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