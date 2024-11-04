$(function ($) {
    $.fn.weekdays = function (options) {
        options = consolideOptions(options);
        var $this = $(this);

        var $html = $("<ul class=" + options.listClass + ">");
        $this.data({
            days: options.days,
            selectedIndexes: options.selectedIndexes
        });

        $($this.data().days).each(function (index, item) {
            var selected = $this.data().selectedIndexes.includes(index);
            var $liElement = $("<li data-day=" + index + " class=" + options.itemClass + " selected=" + selected + ">" + item + "</li>");

            $liElement.on('click', function (item) {
                if (options.singleSelect)
                    singleSelectMode(options, $html);

                var $li = $(item.target);
                toggleSelection($li, options);
            });

            if (selected)
                $liElement.toggleClass(options.itemSelectedClass);

            $liElement.prop('selected', selected);

            $html.append($liElement);
        });

        $this.append($html);
    };

    $.fn.weekdays.days = ["T2", "T3", "T4", "T5", "T6", "T7", "CN"];

    $.fn.selectedIndexes = function () {
        return $(this).find('li')
            .filter(function (index, a) {
                return a.selected;
            })
            .map(function (index, item) {
                return $(item).attr("data-day");
            });
    };

    $.fn.selectedDays = function () {
        var $this = $(this);

        return $(this).find('li')
            .filter(function (index, a) {
                return a.selected;
            })
            .map(function (index, item) {
                return $this.data().days[$(item).attr("data-day")];
            });
    };

    function consolideOptions(options) {
        options = options ? options : {};
        options.days = options.days ? options.days : $.fn.weekdays.days;
        options.selectedIndexes = options.selectedIndexes ? options.selectedIndexes : [];
        options.listClass = options.listClass ? options.listClass : 'weekdays-list';
        options.itemClass = options.itemClass ? options.itemClass : 'weekdays-day';
        options.itemSelectedClass = options.itemSelectedClass ? options.itemSelectedClass : 'weekday-selected';
        options.singleSelect = options.singleSelect ? options.singleSelect : false;

        return options;
    }

    function singleSelectMode(options, $html) {
        $html.find('li')
            .each(function (index, item) {
                var $li = $(item);

                $li.prop('selected', false);
                $li.removeClass(options.itemSelectedClass);
            });
    }

    function toggleSelection($li, options) {
        var selected = !$li.prop('selected')

        $li.prop('selected', selected);
        $li.toggleClass(options.itemSelectedClass);
    }
});

let ctrl = {
    agentService: abp.services.app.agentService,
    userId: $('input[name="userId"]').val(),
    page: $('#create-staff-page'),
    form: $('#create-staff-form'),
    getFormValue: function () {
        let obj = ctrl.form.serializeFormToObject();
        return obj;
    },
    isVietnamesePhoneNumber: function (number) {
        //let validp = /(03|07|08|09|01[2|6|8|9])+([0-9]{8})\b/.test(number);
        let validp = VietNamMobile.valid(number);
        if (validp.length > 0) {
            abp.message.warn('Số điện thoại không đúng định dạng');
        }
    },
    getStaffInfo: function (userId) {
        Sv.RequestStart();
        ctrl.agentService.getUserStaffInfo({'userId': userId}).done(function (obj) {
            let data = obj;
            //let isActive = data['isActive'] * 1;
            let isActive = data['isActive'];
            let fromTime = data['fromTime'];
            let fromTimeHour = fromTime.fromTime;
            let fromTimeMin = fromTime.toTime;
            let toTime = data['toTime'];
            let toTimeHour = toTime.fromTime;
            let toTimeMin = toTime.toTime;
            let days = data['days'];
            let field = ['surname', 'name', 'phoneNumber', 'limitAmount', 'limitPerTrans', 'description'];

            $.each(field, function (index, value) {
                $('input[name="' + value + '"]').val(data[value]);
            });

            $('textarea[name="description"]').val(data['description']);
            $('input[name="isActive').filter('[value=' + isActive + ']').prop('checked', true);
            $('select[name="fromTimeHour"]').val(fromTimeHour);
            $('select[name="fromTimeMin"]').val(fromTimeMin);
            $('select[name="toTimeHour"]').val(toTimeHour);
            $('select[name="toTimeMin"]').val(toTimeMin);

            $.each(days, function (index, value) {
                $('ul.weekdays-list li').eq(value * 1 - 2).addClass('weekday-selected');
            });

        }).always(function () {
            Sv.RequestStart();
        });
    },
    nextToStep: function () {
        if (!$("#create-staff-form").valid()) {
            return;
        }
        Sv.RequestStart();
        let obj = ctrl.getFormValue();

        obj.fromTime = {'fromTime': obj.fromTimeHour, 'toTime': obj.fromTimeMin};
        obj.toTime = {'fromTime': obj.toTimeHour, 'toTime': obj.toTimeMin};

        let listDays = [];
        $('ul.weekdays-list li').each(function () {
            if ($(this).hasClass('weekday-selected')) {
                let d = $(this).attr('data-day');
                d = d * 1 + 2;

                listDays.push(d);
            }
        });

        obj.days = listDays;
        //obj.isActive = obj.isActive ? true : false;

        console.log(obj)

        ctrl.agentService.createUserStaff(obj).done(function (obj) {
            abp.message.success("Tạo nhân viên thành công!");
            setTimeout(function () {
                window.location.href = "/Profile/StaffManager";
            }, 2000);
        }).always(function () {
            Sv.RequestEnd();
        });
    },
    editStaffInfo: function () {
        if (!$("#create-staff-form").valid()) {
            return;
        }
        Sv.RequestStart();
        let obj = ctrl.getFormValue();

        obj.fromTime = {'fromTime': obj.fromTimeHour, 'toTime': obj.fromTimeMin};
        obj.toTime = {'fromTime': obj.toTimeHour, 'toTime': obj.toTimeMin};

        let listDays = [];
        $('ul.weekdays-list li').each(function () {
            if ($(this).hasClass('weekday-selected')) {
                let d = $(this).attr('data-day');
                d = d * 1 + 2;

                listDays.push(d);
            }
        });

        obj.days = listDays;
        //obj.isActive = obj.isActive ? true : false;

        ctrl.agentService.updateUserStaff(obj).done(function (obj) {
            abp.message.success("Sửa thông tin nhân viên thành công!");
            setTimeout(function () {
                window.location.href = "/Profile/StaffManager";
            }, 2000);
        }).always(function () {
            Sv.RequestEnd();
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
    back: function () {
        window.location.href = "/Profile/StaffManager";
    },
}

$(document).ready(function () {
    const _profileService = abp.services.app.profile;
    const _passwordComplexityHelper = new app.PasswordComplexityHelper();
    _profileService.getPasswordComplexitySetting().done(function (result) {
        $("#create-staff-form").validate();
        _passwordComplexityHelper.setPasswordComplexityRules($("#create-staff-form").find("input[name=password]"), result.setting);
    });

    Sv.SetupAmountMask();
    ctrl.enterHandler();
    if (typeof ctrl.userId !== 'undefined') {
        ctrl.getStaffInfo(ctrl.userId);
        $('button.btn-handle').attr("onclick", "ctrl.editStaffInfo()");
        $('input[name="phoneNumber"]').prop('readonly', 'readonly');
        $('input[name="password"]').prop('readonly', 'readonly');
    }
    
    $('#weekdays').weekdays();
    $(".hour-select").timepicker();

    if (typeof ctrl.userId === 'undefined') {
        $('ul.weekdays-list li').each(function () {
            $(this).addClass('weekday-selected');
        });
    }

    $('input[name="phoneNumber"]').on('blur', function (e) {
        const $element = $(this);
        const val = $element.val();
        ctrl.isVietnamesePhoneNumber(val);
    });

    $('input[name="limitAmount"]').on('keyup input', function (e) {
        const $element = $(this);
        const val = $element.val();
        const $str = $("#amount-to-text");
        Sv.BindMoneyToString($str, val);
    });
    $('input[name="limitPerTrans"]').on('keyup input', function (e) {
        const $element = $(this);
        const val = $element.val();
        const $str = $("#amount-to-text-per-trans");
        Sv.BindMoneyToString($str, val);
    });
});
