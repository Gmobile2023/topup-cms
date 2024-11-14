topupApp.filter('formatdate', [
    '$filter', function ($filter) {
        return function (input, format) {
            return $filter('date')(new Date(input), format);
        };
    }
]);

topupApp.filter("strLimit", ["$filter", function ($filter) {
    return function (input, limit) {
        if (input === null) {
            input = "";
        }
        if (input.length <= limit) {
            return input;
        }

        return $filter("limitTo")(input, limit) + '...';
    };
}]);

topupApp.directive("ngDatePicker", function () {
    return {
        link: function (scope, element, attr) {
            var target = $(element);
            //Set TodayDate
            var currentDateTime = new Date();
            var fromDateString = (currentDateTime.getDate() < 10 ? '0' : '') + currentDateTime.getDate() + "/" + ((currentDateTime.getMonth() + 1) < 10 ? '0' : '') + (currentDateTime.getMonth() + 1) + "/" + currentDateTime.getFullYear();
            scope.dDepartDate = fromDateString;
            scope.iDepartDate = fromDateString;
            //Get Lunar Date
            $.ajax({
                url: "../../service/booking/getlunar",
                data: {
                    input: fromDateString
                },
                type: "POST",
                beforeSend: function () {
                },
                complete: function () {
                },
                success: function (data) {
                    if (data.Code === '00') {
                        if (target.is('[ng-dosmestic-date]')) {
                            if (typeof scope.dTextdepart === "undefined" || scope.dTextdepart === "") {
                                scope.dTextdepart = data.Data;
                            }
                        }
                        if (target.is('[ng-international-date]')) {
                            if (typeof scope.iTextdepart === "undefined" || scope.iTextdepart === "") {
                                scope.iTextdepart = data.Data;
                            }
                        }
                    }
                    if (!scope.$$phase)
                        scope.$apply();
                },
                error: ""
            });

            if (target.is('[ng-depart-date]')) {
                var departTarget = target;
                var departDatepicker = {
                    showButtonPanel: true,
                    dateFormat: "dd/mm/yy",
                    showAnim: '',
                    onClose: function () {
                        if ($(window).width() < 769) {
                            $('#ui-datepicker-div').css({
                                'display': 'block'
                            });
                            setTimeout(function () {
                                $('#ui-datepicker-div').removeClass('transform-0');
                            }, 1);
                            setTimeout(function () {
                                $('#ui-datepicker-div').css('display', 'none');
                            }, 400);
                            departTarget.removeAttr('disabled');
                            //overlayClose();
                            BNS.off();
                        }
                        departTarget.blur();
                    },
                    beforeShow: function () {
                        if ($(window).width() < 769) {
                            //setTimeout(function () { overlayOpen(); }, 550);
                            BNS.on();
                            departTarget.blur();
                            departTarget.attr('disabled', 'disabled');
                        }
                    },
                    dayNamesMin: ["CN", "T2", "T3", "T4", "T5", "T6", "T7"],
                    monthNames: ["Tháng 1", "Tháng 2", "Tháng 3", "Tháng 4", "Tháng 5", "Tháng 6", "Tháng 7", "Tháng 8", "Tháng 9", "Tháng 10", "Tháng 11", "Tháng 12"],
                    nextText: '',
                    prevText: '',
                    firstDay: 1,
                    showMonthAfterYear: true
                };

                departTarget.removeAttr('disabled', 'disabled');

                var datepickerStart = {
                    closeText: "Chọn ngày khởi hành",
                    minDate: 0,
                    onSelect: function (dateText, inst) {
                        var date = $(this).val();
                        departTarget.datepicker("setDate", date);
                        //set Scope
                        if (target.is('[ng-dosmestic-date]')) {
                            scope.dDepartDate = date;
                        }
                        if (target.is('[ng-international-date]')) {
                            scope.iDepartDate = date;
                        }

                        if (!scope.$$phase)
                            scope.$apply();
                        departTarget.val(date);

                        //Get Lunar Date
                        $.ajax({
                            url: "../../service/booking/getlunar",
                            data: {
                                input: date
                            },
                            type: "POST",
                            beforeSend: function () {
                            },
                            complete: function () {
                            },
                            success: function (data) {
                                if (data.Code === '00') {
                                    if (target.is('[ng-dosmestic-date]')) {
                                        scope.dTextdepart = data.Data;
                                    }
                                    if (target.is('[ng-international-date]')) {
                                        scope.iTextdepart = data.Data;
                                    }
                                }
                                if (!scope.$$phase)
                                    scope.$apply();
                            },
                            error: ""
                        });

                        return date;
                    }
                };
                departTarget.datepicker($.extend(departDatepicker, datepickerStart)).datepicker("setDate", "0");

                departTarget.focus(function () {
                    setTimeout(function () {
                        $('#ui-datepicker-div').addClass('transform-0');
                    }, 0.0001);
                });
            }
            if (target.is('[ng-arrival-date]')) {
                var arrivalTarget = target;
                var arrivalDatepicker = {
                    showButtonPanel: true,
                    dateFormat: "dd/mm/yy",
                    showAnim: '',
                    onClose: function () {
                        if ($(window).width() < 769) {
                            $('#ui-datepicker-div').css({
                                'display': 'block'
                            });
                            setTimeout(function () {
                                $('#ui-datepicker-div').removeClass('transform-0');
                            }, 1);
                            setTimeout(function () {
                                $('#ui-datepicker-div').css('display', 'none');
                            }, 400);
                            arrivalTarget.removeAttr('disabled');
                            //overlayClose();
                            BNS.off();
                        }
                        arrivalTarget.blur();
                    },
                    beforeShow: function () {
                        if ($(window).width() < 769) {
                            //setTimeout(function () { overlayOpen(); }, 550);
                            BNS.on();
                            arrivalTarget.blur();
                            arrivalTarget.attr('disabled', 'disabled');
                        }
                    },
                    dayNamesMin: ["CN", "T2", "T3", "T4", "T5", "T6", "T7"],
                    monthNames: ["Tháng 1", "Tháng 2", "Tháng 3", "Tháng 4", "Tháng 5", "Tháng 6", "Tháng 7", "Tháng 8", "Tháng 9", "Tháng 10", "Tháng 11", "Tháng 12"],
                    nextText: '',
                    prevText: '',
                    firstDay: 1,
                    showMonthAfterYear: true
                };

                arrivalTarget.removeAttr('disabled', 'disabled');
                var datepickerArrival = {
                    closeText: "Chọn ngày về",
                    minDate: target.datepicker('getDate'),
                    onSelect: function (dateText, inst) {
                        var date = $(this).val();
                        arrivalTarget.datepicker("setDate", date);

                        //set scope
                        //set Scope
                        if (target.is('[ng-dosmestic-date]')) {
                            scope.dArrivalDate = date;
                        }
                        if (target.is('[ng-international-date]')) {
                            scope.iArrivalDate = date;
                        }

                        if (!scope.$$phase)
                            scope.$apply();
                        arrivalTarget.val(date);

                        //Get Lunar Date
                        $.ajax({
                            url: "../../service/booking/getlunar",
                            data: {
                                input: date
                            },
                            type: "POST",
                            beforeSend: function () {
                            },
                            complete: function () {
                            },
                            success: function (data) {
                                if (data.Code === '00') {
                                    if (target.is('[ng-dosmestic-date]')) {
                                        scope.dTextArrival = data.Data;
                                    }
                                    if (target.is('[ng-international-date]')) {
                                        scope.iTextArrival = data.Data;
                                    }
                                }
                                if (!scope.$$phase)
                                    scope.$apply();
                            },
                            error: ""
                        });

                        return date;
                    }
                };

                arrivalTarget.datepicker($.extend(arrivalDatepicker, datepickerArrival)).datepicker('option', 'minDate', 0);
                arrivalTarget.focus(function () {
                    setTimeout(function () {
                        $('#ui-datepicker-div').addClass('transform-0');
                    }, 0.0001);
                });
            }

        }
    };
});

topupApp.controller('FlightController', ["$scope", "$locale", function ($scope, $locale) {
    //==================================GLOBAL METHOD===============================================
    //Global Init value
    $locale.NUMBER_FORMATS.GROUP_SEP = '.';
    //$scope.panelFlight = "1";
    $scope.panelStatus = 1;
    $scope.isCheap = true;
    //$scope.tickettype = "domestic";// loại hành trình
    $scope.dJourney = 2;
    $scope.iJourney = 2;
    $scope.urlredirect = "";
    //International Airport, Domestic Aiport
    $scope.InterAirport = [];
    $scope.InterDomesticAirport = [];
    $scope.DomesticAirport = [];
    $scope.AllAirport = [];
    $scope.AirportType = 0;
    $scope.airline = "";
    $scope.AirlineNews = [];

    $.validator.addMethod("regx", function (value, element, regexpr) {
        return regexpr.test(value);
    }, "");

    
    $scope.searchDomesticFlight = function () {
        if ($scope.panelFlight === "1") {
            $('#steponeForm').submit(function (e) {
                e.preventDefault();
            }).validate({
                rules: {
                    dStartPoint: {
                        required: true
                    },
                    dEndPoint: {
                        required: true
                    },
                    dDepartDate: {
                        required: true
                    },
                    dArrivalDate: {
                        required: true
                    }
                },
                messages: {
                    dStartPoint: {
                        required: "Vui lòng chọn nơi đi"
                    },
                    dEndPoint: {
                        required: "Vui lòng chọn nơi đến"
                    },
                    dDepartDate: {
                        required: "Vui lòng chọn ngày đi"
                    },
                    dArrivalDate: {
                        required: "Vui lòng chọn ngày về"
                    }
                },
                errorPlacement: function (error, element) {
                    if (element.attr("name") === "dStartPoint") {
                        error.insertAfter("#d-startpoint-error");
                    } else if (element.attr("name") === "dEndPoint") {
                        error.insertAfter("#d-endpoint-error");
                    } else
                        error.insertAfter(element);
                },
                submitHandler: function (form) {
                    //Check duplicate point
                    if ($scope.dStartPoint === $scope.dEndPoint) {
                        $scope.errormsg = "Vui lòng không chọn nơi đi trùng nơi đến";
                        if (!$scope.$$phase)
                            $scope.$apply();
                        $('#error-modal').modal();
                        return false;
                    }

                    var departDateArr = $scope.dDepartDate.split('/');
                    var departDate = new Date(departDateArr[2] + "-" + departDateArr[1] + "-" + departDateArr[0]);
                    var arrivalDateArr = $scope.dArrivalDate.split('/');
                    var arrivalDate = new Date(arrivalDateArr[2] + "-" + arrivalDateArr[1] + "-" + arrivalDateArr[0]);
                    var date = new Date();
                    //if (departDate < date.getDate()-1) {
                    //    $scope.errormsg = "Ngày đi không được nhỏ hơn ngày hiện tại, Quý khách vui lòng chọn lại";
                    //    if (!$scope.$$phase)
                    //        $scope.$apply();
                    //    $('#error-modal').modal();
                    //    return false;
                    //}
                    if ($scope.dJourney === 2) {                       
                        if (arrivalDate < departDate) {
                            $scope.errormsg = "Ngày đi không được lớn hơn ngày về";
                            if (!$scope.$$phase)
                                $scope.$apply();
                            $('#error-modal').modal();
                            return false;
                        }
                    }

                    $.ajax({
                        url: "../../service/booking/searchdomestic",
                        data: {
                            adt: $scope.dAdult,
                            chd: $scope.dChild,
                            inf: $scope.dInfant,
                            round: $scope.dJourney,
                            departdate: $scope.dDepartDate,
                            arrivedDate: $scope.dArrivalDate,
                            startpoint: $scope.dStartPoint,
                            endpoint: $scope.dEndPoint
                        },
                        type: "POST",
                        beforeSend: function () {
                        },
                        complete: function () {
                        },
                        success: function (data) {
                            if (data.Code === '00') {
                                var url = "mua-ve-may-bay/dat-ve-noi-dia.html";
                                window.location.href = url;
                            } else {
                                $scope.errormsg = data.msg;
                                if (!$scope.$$phase)
                                    $scope.$apply();
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
    };

    $scope.searchInternationFlight = function () {
        if ($scope.panelFlight === "2") {
            $('#steptwoForm').submit(function (e) {
                e.preventDefault();
            }).validate({
                rules: {
                    iStartPoint: {
                        required: true
                    },
                    iEndPoint: {
                        required: true
                    },
                    iDepartDate: {
                        required: true
                    },
                    iArrivalDate: {
                        required: true
                    }
                },
                messages: {
                    iStartPoint: {
                        required: "Vui lòng chọn điểm đi"
                    },
                    iEndPoint: {
                        required: "Vui lòng chọn điểm đến"
                    },
                    iDepartDate: {
                        required: "Vui lòng chọn ngày khởi hành"
                    },
                    iArrivalDate: {
                        required: "Vui lòng chọn ngày về"
                    }
                },
                errorPlacement: function (error, element) {
                    if (element.attr("name") === "iStartPoint") {
                        error.insertAfter("#i-startpoint-error");
                    }
                    else if (element.attr("name") === "iEndPoint") {
                        error.insertAfter("#i-endpoint-error");
                    }
                    else
                        error.insertAfter(element);
                },
                submitHandler: function (form) {
                    //Check duplicate point
                    if ($scope.iStartPoint === $scope.iEndPoint) {
                        $scope.errormsg = "Vui lòng không chọn nơi đi trùng nơi đến";
                        if (!$scope.$$phase)
                            $scope.$apply();
                        $('#error-modal').modal();
                        return false;
                    }

                    var departDateArr = $scope.dDepartDate.split('/');
                    var departDate = new Date(departDateArr[2] + "-" + departDateArr[1] + "-" + departDateArr[0]);
                    var arrivalDateArr = $scope.dArrivalDate.split('/');
                    var arrivalDate = new Date(arrivalDateArr[2] + "-" + arrivalDateArr[1] + "-" + arrivalDateArr[0]);
                    //if (departDate < new Date()) {
                    //    $scope.errormsg = "Ngày đi không được nhỏ hơn ngày hiện tại, Quý khách vui lòng chọn lại";
                    //    if (!$scope.$$phase)
                    //        $scope.$apply();
                    //    $('#error-modal').modal();
                    //    return false;
                    //}
                    if ($scope.dJourney === 2) {
                        if (arrivalDate < departDate) {
                            $scope.errormsg = "Ngày đi không được lớn hơn ngày về";
                            if (!$scope.$$phase)
                                $scope.$apply();
                            $('#error-modal').modal();
                            return false;
                        }
                    }
                    //Check passenger
                    var adult = parseInt($scope.iAdult);
                    var child = parseInt($scope.iChild);
                    var infant = parseInt($scope.iInfant);
                    if ((adult + child) > 9) {
                        $scope.errormsg = "Số lượng hành khách vượt quá quy định. Vui lòng liên hệ tổng đài 1900 5555 20 để được hỗ trợ";
                        if (!$scope.$$phase)
                            $scope.$apply();
                        $('#error-modal').modal();
                        return false;
                    }

                    var isOk = validPax(adult, child, infant);
                    if (isOk !== '1') {
                        $scope.errormsg = isOk;
                        if (!$scope.$$phase)
                            $scope.$apply();
                        $('#error-modal').modal();
                        return false;
                    }

                    $.ajax({
                        url: "../../service/booking/international",
                        data: {
                            adt: $scope.iAdult,
                            chd: $scope.iChild,
                            inf: $scope.iInfant,
                            round: $scope.iJourney,
                            departdate: $scope.iDepartDate,
                            arrivedDate: $scope.iArrivalDate,
                            startpoint: $scope.iStartPoint,
                            endpoint: $scope.iEndPoint,
                            airline: $scope.airline
                        },
                        type: "POST",
                        beforeSend: function () {
                        },
                        complete: function () {
                        },
                        success: function (data) {
                            if (data.Code === '00') {
                                var url = "mua-ve-may-bay/dat-ve-quoc-te.html";
                                window.location.href = url;
                            } else {
                                $scope.errormsg = data.msg;
                                if (!$scope.$$phase)
                                    $scope.$apply();
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
    }

    $scope.changeticket = function () {
        if ($scope.tickettype === "domestic") {
            $scope.panelFlight = "1";
        } else {
            $scope.panelFlight = "2";
        }
        if (!$scope.$$phase)
            $scope.$apply();
    }

    $scope.changejourney = function (dtype, itype) {
        if (dtype !== 0) {
            $scope.dJourney = dtype;
        }
        if (itype !== 0) {
            $scope.iJourney = itype;
        }

        if (!$scope.$$phase)
            $scope.$apply();
    }

    //Utils
    function validPax(adt, chd, inf) {
        if (chd === 0 && inf === 0 && adt > 0) {
            return '1';
        }

        if (inf === 0) {
            if (adt * 2 < chd) {
                return 'Số lượng trẻ em không hợp lệ';
            }
            else {
                return '1';
            }
        }

        if (chd === 0) {
            if (adt < inf) {
                return 'Số lượng em bé không hợp lệ';
            }
            else {
                return '1';
            }
        }

        /* neu adt>0 and inf>0 */
        if (inf > adt) {
            return 'Số lượng em bé không hợp lệ';
        }
        if (inf > chd) {
            return '1';
        }
        var temp = chd - inf;  //ra so luong tre con lai
        var tempadt = adt - inf;
        tempadt = tempadt * 2; //ra so luong tre em nguoi lon co the di kem
        if (tempadt < temp) {
            return 'Theo quy định 1 người lớn được đi kèm 1 trẻ em và 1 em bé. Hoặc 1 người lớn được đi kèm 2 trẻ em. Quý khách vui lòng chọn lại';
        }

        return '1';
    }

    $scope.getInternationalAirport = function () {
        $.ajax({
            url: "../../service/booking/getallairport",
            data: {
            },
            type: "POST",
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (data) {
                if (data.Code === '00') {
                    $scope.AllAirport = data.Data;
                    var lazy = Lazy(data.Data);
                    var domesticAirport = lazy.filter(function (x) {
                        return (x.Region_Group === "VN");
                    }).toArray();
                    var interAirport = lazy.filter(function (x) {
                        return (x.Region_Group !== "VN");
                    }).toArray();
                    $scope.InterDomesticAirport = domesticAirport;
                    $scope.InterAirport = interAirport;

                    //Check Cookies
                    var airport = lazy.filter(function (x) {
                        return (x.Airport_Code === data.iStartPoint);
                    }).toArray();
                    if (airport.length > 0) {
                        if (airport[0].Region_Group === "VN") {
                            $scope.AirportType = 1;
                        } else {
                            $scope.AirportType = 2;
                        }
                    }

                    $scope.iStartPoint = data.iStartPoint;
                    $scope.iEndPoint = data.iEndPoint;

                    if (data.iDepartDate !== "") {
                        $scope.iDepartDate = data.iDepartDate;
                        $scope.iTextdepart = data.iTextDepartDate;
                    }
                    if (data.iArrivalDat !== "") {
                        $scope.iArrivalDate = data.iArrivalDate;
                        $scope.iTextArrival = data.iTextArrivalDate;
                    }
                    if (data.iJourney !== "") {
                        $scope.iJourney = data.iJourney;
                    }
                    $scope.iAdult = data.iAdult;
                    $scope.iChild = data.iChild;
                    $scope.iInfant = data.iInfant;
                }
                if (!$scope.$$phase)
                    $scope.$apply();
            },
            error: ""
        });
    }

    $scope.getDomesticAirport = function () {
        $.ajax({
            url: "../../service/booking/get-domestic-airport",
            data: {
            },
            type: "POST",
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (data) {
                if (data.Code === '00') {
                    $scope.DomesticAirport = data.Data;

                    //Check Cookies
                    $scope.dStartPoint = data.dStartPoint;
                    $scope.dEndPoint = data.dEndPoint;
                    if (data.dDepartDate !== "") {
                        $scope.dDepartDate = data.dDepartDate;
                        $scope.dTextdepart = data.dTextDepartDate;
                    }
                    if (data.dArrivalDat !== "") {
                        $scope.dArrivalDate = data.dArrivalDate;
                        $scope.dTextArrival = data.dTextArrivalDate;
                    }
                    if (data.dJourney !== "") {
                        $scope.dJourney = data.dJourney;
                    }
                    $scope.dAdult = data.dAdult;
                    $scope.dChild = data.dChild;
                    $scope.dInfant = data.dInfant;
                }
                if (!$scope.$$phase)
                    $scope.$apply();
            },
            error: ""
        });
    }

    $scope.ChangeAirport = function () {
        var lazy = Lazy($scope.AllAirport);
        var airport = lazy.filter(function (x) {
            return (x.Airport_Code === $scope.iStartPoint);
        }).toArray();
        if (airport[0].Region_Group === "VN") {
            $scope.AirportType = 1;
        } else {
            $scope.AirportType = 2;
        }
        $scope.iEndPoint = "";
        if (!$scope.$$phase)
            $scope.$apply();
        //$(".select--custom-1r").select2({
        //    width: '100%',
        //    placeholder: "Chọn nơi đến"
        //});
        $('.select--custom-1r').val($scope.iEndPoint).trigger('change');
    }

    $scope.CheckActivePanel = function () {
        var tab = getParameterByName("tab");
        if (tab === "quoc-te") {
            $scope.tickettype = "international";
            $scope.panelFlight = "2";
        } else {
            $scope.tickettype = "domestic";
            $scope.panelFlight = "1";
        }
    }

    $scope.searchflightfake = function () {
        $.ajax({
            url: "../../service/booking/international",
            data: {
                adt: 1,
                chd: 0,
                inf: 0,
                round: 1,
                departdate: '18/07/2017',
                arriveddate: '',
                startpoint: 'HAN',
                endpoint: 'SIN'
            },
            type: "post",
            beforesend: function () {
            },
            complete: function () {
            },
            success: function (data) {
                if (data.Code === '00') {
                    var url = "mua-ve-may-bay/dat-ve-quoc-te.html?adt=" + 1 + "&chd=" + 1 + "&inf=" + 0 + "&departdate=" + "18/07/2017" + "&arriveddate=" + "" + "&startpoint=" + "HAN" + "&endpoint=" + "SIN" + "&round=" + 1 + "&airline=VN";
                    window.location.href = url;
                }
                if (!$scope.$$phase)
                    $scope.$apply();
            },
            error: ""
        });
    }

    $scope.GetAirlineNews = function () {
        $.ajax({
            url: "/booking/get-airline-news-by-alias",
            data: {
                pageNo: 1,
                pageSize: 3
            },
            type: "POST",
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (data) {
                if (data.Code === "00") {
                    $scope.AirlineNews = data.Data;
                }

                if (!$scope.$$phase)
                    $scope.$apply();
            },
            error: ""
        });
    }
    $scope.getLunnerText = function () {
        //var date = new Date(value.replace(/(\d{1,2})-(\d{1,2})-(\d{4}) (\d{1,2}):(\d{1,2})/, "$2/$1/$3 $4:$5")) > new Date($(params).val().replace(/(\d{1,2})-(\d{1,2})-(\d{4}) (\d{1,2}):(\d{1,2})/, "$2/$1/$3 $4:$5"));
    

        var departDateArr = $scope.dDepartDate.split('/');
        var departDate = new Date(departDateArr[2] + "-" + departDateArr[1] + "-" + departDateArr[0]);
        var arrivalDateArr = $scope.dArrivalDate.split('/');
        var arrivalDate = new Date(arrivalDateArr[2] + "-" + arrivalDateArr[1] + "-" + arrivalDateArr[0]);
       
        var idepartDateArr = $scope.iDepartDate.split('/');
        var idepartDate = new Date(idepartDateArr[2] + "-" + idepartDateArr[1] + "-" + idepartDateArr[0]);
        var iarrivalDateArr = $scope.iArrivalDate.split('/');
        var iarrivalDate = new Date(iarrivalDateArr[2] + "-" + iarrivalDateArr[1] + "-" + iarrivalDateArr[0]);
        if (!isNaN(departDate) || !isNaN(arrivalDate) || !isNaN(idepartDate) || !isNaN(iarrivalDate)) {
           
            $.ajax({
                url: "/booking/get-text-lunar",
                data: {
                    dDepartDate: $scope.dDepartDate,
                    dArrivalDate: $scope.dArrivalDate,
                    iDepartDate: $scope.iDepartDate,
                    iArrivalDate: $scope.iArrivalDate
                },
                type: "POST",
                beforeSend: function () {
                },
                complete: function () {
                },
                success: function (data) {
                    if (data.Code === "00") {
                        $scope.dTextdepart = data.dTextdepart;
                        $scope.dTextArrival = data.dTextarrival;
                        $scope.iTextdepart = data.iTextdepart;
                        $scope.iTextArrival = data.iTextarrival;
                    }
                    if (!$scope.$$phase)
                        $scope.$apply();
                },
                error: ""
            });
        }
        
    }
}]);


//BookPayController

topupApp.controller('BookPayController', ["$scope", "$locale", function ($scope, $locale) {
    //==================================GLOBAL METHOD===============================================
    //Global Init value
    $locale.NUMBER_FORMATS.GROUP_SEP = '.';
    $scope.amount = 0;
    $scope.panelStatus = 1;
    $scope.detailStatus = 1;
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
    //Custom init value
    $scope.paymentCode = "";
    $scope.CityArr = [];
    $scope.DistrictArr = [];
    $scope.Passenger = [];
    $scope.Flight = [];
    $scope.Book = [];
    $scope.CreateDate = "";
    $scope.journey = 1;
    $scope.AirlineNews = [];
    $scope.feetnx = 0;

    $scope.ChangeMobileBanking = function () {
        $scope.payMethod = "VNPAYQR";
        if (!$scope.$$phase)
            $scope.$apply();
    }

    //Captcha
    $scope.CaptchaValue = "";

    $scope.GenCap = function () {
        var text = "";
        var possible = "0123456789";

        for (var i = 0; i < 6; i++)
            text += possible.charAt(Math.floor(Math.random() * possible.length));

        $scope.CaptchaValue = text;

        $.ajax({
            url: "../../service/book/gen-captcha-hash",
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
                        addData: encodeURIComponent("BOOK-" + $scope.paymentCode),
                        giftcode: $scope.giftcode,
                        price: 0
                    },
                    type: "POST",
                    beforeSend: function() {
                    },
                    complete: function() {
                    },
                    success: function(data) {
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

    $scope.validCheck = function () {
        if ($scope.paymentCode === '') {
            $scope.errormsg = "Vui lòng nhập mã thanh toán";
            $('#error-modal').modal();
            if (!$scope.$$phase)
                $scope.$apply();
        } else if ($scope.capchakey === '' || typeof $scope.capchakey === 'undefined') {
            $scope.invalidcaptcha = "2";
            if (!$scope.$$phase)
                $scope.$apply();
        }
        else {
            $.ajax({
                url: "../../service/book/check-captcha",
                data: {
                    input: $scope.capchakey,
                    hash: $scope.CapHash
                },
                type: "POST",
                beforeSend: function () {
                    //$("#service-loader-wrapper").css("display", "");
                },
                complete: function () {
                    //$("#service-loader-wrapper").css("display", "none");
                },
                success: function (data) {
                    if (data.Code === '00') {
                        $scope.invalidcaptcha = "0";
                       
                        //$scope.panelStatus = 1;
                        $.ajax({
                            url: "../../service/book/getbook",
                            data: {
                                input: encodeURIComponent($scope.paymentCode)
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
                                    $("#btnpay").removeClass("section-frame__btn--disable");
                                    $scope.detailStatus = 2;
                                    $scope.Passenger = JSON.parse(data.passenger);
                                    $scope.Flight = JSON.parse(data.flight);
                                    $scope.journey = data.tickettype;
                                    $scope.amount = data.amount;
                                    $scope.CreateDate = data.create;
                                } else {
                                    // không có mã thanh toán thì phải đổi capcha
                                    $scope.GenCap();
                                    $scope.detailStatus = 1;
                                    $("#btnpay").addClass("section-frame__btn--disable");
                                    $scope.errormsg = data.msg;
                                    $('#error-modal').modal();
                                   
                                }
                                if (!$scope.$$phase)
                                    $scope.$apply();
                                $('html, body').animate({
                                    scrollTop: 0
                                }, 200);

                            },
                        });

                    } else {
                        $scope.invalidcaptcha = "1";
                        $scope.detailStatus = 1;
                        $("#btnpay").addClass("section-frame__btn--disable");
                        if (!$scope.$$phase)
                            $scope.$apply();
                    }
                },
                error: ""
            });
        }
    };

    $scope.nextToPay = function () {
        $('#steponeForm').submit(function (e) {
            e.preventDefault();
        }).validate({
            rules: {
                paymentCode: {
                    required: true,
                },
                capchakey: {
                    required: true,
                },
            },
            messages: {
                paymentCode: {
                    required: "Vui lòng nhập mã thanh toán",
                },
                capchakey: {
                    required: "Vui lòng nhập mã xác thực",
                },
            },

            submitHandler: function (form) {
                // sau khi past validate form

                $.ajax({
                    url: "../../service/book/check-captcha",
                    data: {
                        input: $scope.capchakey,
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
                            $scope.invalidcaptcha = "0";
                            $.ajax({
                                // lấy thông tin booking
                                url: "../../service/book/getbook",
                                data: {
                                    input: encodeURIComponent($scope.paymentCode)
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
                                        $scope.amount = data.amount;
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
                                                    addData: encodeURIComponent("BOOK-" + $scope.paymentCode),
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
                                        $scope.errormsg = data.msg;
                                        $('#error-modal').modal();
                                        if (!$scope.$$phase)
                                            $scope.$apply();
                                        $('html, body').animate({
                                            scrollTop: 0
                                        }, 200);
                                    }
                                },
                                error: ""
                            });
                        } else {
                            $scope.invalidcaptcha = "1";
                            if (!$scope.$$phase)
                                $scope.$apply();
                            $('html, body').animate({
                                scrollTop: 0
                            }, 200);
                        }
                    },
                    error: ""
                });


            }

        });
    };

    $scope.payNow = function () {
        //kiểm tra lựa chọn bank thanh toán
        if ((typeof $scope.payMethod === 'undefined' || $scope.payMethod === "") && $scope.amountpay !== 0) {
            $('#payment-modal').modal();
            return false;
        }
        //validate thông tin nếu thanh toán bằng thẻ visa
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
                        regx:
                            /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
                    },
                    visaCusMobile: {
                        required: true,
                        regx:
                            /^(096|097|098|0162|0163|0164|0165|0166|0167|0168|0169|086|032|033|034|035|036|037|038|039|0129|0127|0125|0124|0123|094|091|088|081|082|083|084|085|090|093|0120|0121|0122|0126|0128|089|070|076|077|078|079|092|0188|0186|056|058|099|0199|059)\d{7}$/
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
                            url: "../../service/book/execute",
                            data: {
                                paycode: encodeURIComponent($scope.paymentCode),
                                //providerid: $scope.provider,
                                //cusmobile: $scope.cusmobile,
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
                url: "../../service/book/execute",
                data: {
                    paycode: encodeURIComponent($scope.paymentCode),
                    //providerid: $scope.provider,
                    //cusmobile: $scope.cusmobile,
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

    $scope.backButton = function () {
        $scope.panelStatus = 1;
        $scope.payMethod = "";
        $scope.feetnx = 0;
        //clear visa input
        $scope.visaCusName = '';
        $scope.visaCusEmail = '';
        $scope.visaCusMobile = '';
        $scope.visaCusAddress = '';
        $scope.capchakey = '';
        $scope.GenCap();
        // remove button thanh toán, đóng form chi tiết
        $("#btnpay").addClass("section-frame__btn--disable");
        $("input").siblings('label.required').show();
        $scope.detailStatus = 1;

        if (!$scope.$$phase)
            $scope.$apply();
        $('html, body').animate({
            scrollTop: 0
        }, 200);
        $(".affix-wrap").removeClass('affix');
        $(".affix-wrap").removeClass('affix-bottom');
    }

    $scope.ChoosenVisa = function () {
        if ($scope.amountpay > 0) {
            $.ajax({
                url: "../../service/adsl/get-fee-tranx",
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

    $scope.GetAirlineNews = function () {
        $.ajax({
            url: "/booking/get-airline-news-by-alias",
            data: {
                pageNo: 1,
                pageSize: 8
            },
            type: "POST",
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (data) {
                if (data.Code === "00") {
                    $scope.AirlineNews = data.Data;
                }

                if (!$scope.$$phase)
                    $scope.$apply();
            },
            error: ""
        });
    }

    $scope.inputPayCode=function() {
        $("#btnpay").addClass("section-frame__btn--disable");
        $scope.detailStatus = 1;
      
    }

    $scope.ClosePaymethod = function () {
        //$scope.payMethod = "";
        //$scope.amountpay = $scope.amountpaytemp;
        //$scope.feetnx = 0;
        if (!$scope.$$phase)
            $scope.$apply();
    };

    $scope.InitPayBookCode = function() {
        var payCode = getParameterByName("bookingcode");
        $scope.paymentCode = payCode;
        if (!$scope.$$phase)
            $scope.$apply();
    }

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
                url: "../../service/adsl/get-fee-tranx",
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

