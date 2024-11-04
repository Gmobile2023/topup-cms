var app = app || {};
(function () {

    var appLocalizationSource = abp.localization.getSource('Topup');
    app.localize = function () {
        return appLocalizationSource.apply(this, arguments);
    };

    app.downloadTempFile = function (file) {
        location.href = abp.appPath + 'File/DownloadTempFile?fileType=' + file.fileType + '&fileToken=' + file.fileToken + '&fileName=' + file.fileName;
    };

    app.createDateRangePickerOptions = function (extraOptions) {
        extraOptions = extraOptions ||
            {
                allowFutureDate: false
            };

        var options = {
            locale: {
                format: 'L',
                applyLabel: app.localize('Apply'),
                cancelLabel: app.localize('Cancel'),
                customRangeLabel: app.localize('CustomRange')
            },
            min: moment('2015-05-01'),
            minDate: moment('2015-05-01'),
            opens: 'left',
            ranges: {}
        };

        if (!extraOptions.allowFutureDate) {
            options.max = moment();
            options.maxDate = moment();
        }

        options.ranges[app.localize('Today')] = [moment().startOf('day'), moment().endOf('day')];
        options.ranges[app.localize('Yesterday')] = [moment().subtract(1, 'days').startOf('day'), moment().subtract(1, 'days').endOf('day')];
        options.ranges[app.localize('Last7Days')] = [moment().subtract(6, 'days').startOf('day'), moment().endOf('day')];
        options.ranges[app.localize('Last30Days')] = [moment().subtract(29, 'days').startOf('day'), moment().endOf('day')];
        options.ranges[app.localize('ThisMonth')] = [moment().startOf('month'), moment().endOf('month')];
        options.ranges[app.localize('LastMonth')] = [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')];

        return options;
    };
    app.createDateTimeRangePickerOptions = function (extraOptions) {
        extraOptions = extraOptions ||
            {
                allowFutureDate: false
            };

        var options = {
            locale: {
                format: 'L LT',
                applyLabel: app.localize('Apply'),
                cancelLabel: app.localize('Cancel'),
                customRangeLabel: app.localize('CustomRange')
            },
            min: moment('2015-05-01'),
            minDate: moment('2015-05-01'),
            opens: 'left',
            ranges: {}
        };

        if (!extraOptions.allowFutureDate) {
            options.max = moment();
            options.maxDate = moment();
        }

        options.ranges[app.localize('Today')] = [moment().startOf('day'), moment().endOf('day')];
        options.ranges[app.localize('Yesterday')] = [moment().subtract(1, 'days').startOf('day'), moment().subtract(1, 'days').endOf('day')];
        options.ranges[app.localize('Last7Days')] = [moment().subtract(6, 'days').startOf('day'), moment().endOf('day')];
        options.ranges[app.localize('Last30Days')] = [moment().subtract(29, 'days').startOf('day'), moment().endOf('day')];
        options.ranges[app.localize('ThisMonth')] = [moment().startOf('month'), moment().endOf('month')];
        options.ranges[app.localize('LastMonth')] = [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')];

        return options;
    };

    app.getUserProfilePicturePath = function (profilePictureId) {
        return profilePictureId ?
            (abp.appPath + 'Profile/GetProfilePictureById?id=' + profilePictureId) :
            (abp.appPath + 'Common/Images/default-profile-picture.png');
    }

    app.getUserProfilePicturePath = function () {
        return abp.appPath + 'Profile/GetProfilePicture?v=' + new Date().valueOf();
    }

    app.getShownLinkedUserName = function (linkedUser) {
        if (!abp.multiTenancy.isEnabled) {
            return linkedUser.username;
        } else {
            if (linkedUser.tenancyName) {
                return linkedUser.tenancyName + '\\' + linkedUser.username;
            } else {
                return '.\\' + linkedUser.username;
            }
        }
    }

    app.notification = app.notification || {};

    app.notification.getUiIconBySeverity = function (severity) {
        switch (severity) {
            case abp.notifications.severity.SUCCESS:
                return 'fas fa-check-circle';
            case abp.notifications.severity.WARN:
                return 'fas fa-exclamation-triangle';
            case abp.notifications.severity.ERROR:
                return 'fas fa-exclamation-circle';
            case abp.notifications.severity.FATAL:
                return 'fas fa-bomb';
            case abp.notifications.severity.INFO:
            default:
                return 'fas fa-info-circle';
        }
    };

    app.notification.getIconFontClassBySeverity = function (severity) {
        switch (severity) {
        case abp.notifications.severity.SUCCESS:
            return ' text-success';
        case abp.notifications.severity.WARN:
            return ' text-warning';
        case abp.notifications.severity.ERROR:
            return ' text-danger';
        case abp.notifications.severity.FATAL:
            return ' text-danger';
        case abp.notifications.severity.INFO:
        default:
            return ' text-info';
        }
    }

    app.changeNotifyPosition = function (positionClass) {
        if (!toastr) {
            return;
        }

        toastr.clear();
        toastr.options.positionClass = positionClass;
    };

    app.waitUntilElementIsReady = function (selector, callback, checkPeriod) {
        if (!$) {
            return;
        }

        var elementCount = selector.split(',').length;

        if (!checkPeriod) {
            checkPeriod = 100;
        }

        var checkExist = setInterval(function () {
            if ($(selector).length >= elementCount) {
                clearInterval(checkExist);
                callback();
            }
        }, checkPeriod);
    };

    app.calculateTimeDifference = function (fromTime, toTime, period) {
        if (!moment) {
            return null;
        }

        var from = moment(fromTime);
        var to = moment(toTime);
        return to.diff(from, period);
    };

    app.htmlUtils = {
        htmlEncodeText: function (value) {
            return $("<div/>").text(value).html();
        },

        htmlDecodeText: function (value) {
            return $("<div/>").html(value).text();
        },

        htmlEncodeJson: function (jsonObject) {
            return JSON.parse(app.htmlUtils.htmlEncodeText(JSON.stringify(jsonObject)));
        },

        htmlDecodeJson: function (jsonObject) {
            return JSON.parse(app.htmlUtils.htmlDecodeText(JSON.stringify(jsonObject)));
        }
    };

    app.guid = function() {
        function s4() {
            return Math.floor((1 + Math.random()) * 0x10000)
                .toString(16)
                .substring(1);
        }

        return s4() + s4() + '-' + s4() + '-' + s4() + '-' + s4() + '-' + s4() + s4() + s4();
    };

    app.isRTL = function() {
        return document.documentElement.getAttribute('dir') === 'rtl' || document.documentElement.getAttribute('direction') === 'rtl';
    };

    app.uploadImage = function (imageElm, showImageElm) {
        const form = new FormData();
        form.append("file", imageElm[0].files[0]);
        const settings = {
            "url": "/api/services/app/File/UploadFile",
            "method": "POST",
            "timeout": 0,
            "processData": false,
            "mimeType": "multipart/form-data",
            "contentType": false,
            "data": form
        };
        abp.ui.setBusy();
        $.ajax(settings).done(function (response) {
            const rs = JSON.parse(response);
            if (rs.success === true) {
                showImageElm.attr('src', rs.result);
            } else {
                abp.notify.error("Upload file không thành công");
            }
        }).always(function () {
                abp.ui.clearBusy();
            }
        );
    }

    app.uploadImages = function ($imageElm, $showImageElm) {
        const form = new FormData();
        form.append("file", $imageElm[0].files[0]);
        const settings = {
            "url": "/api/services/app/File/UploadFiles",
            "method": "POST",
            "timeout": 0,
            "processData": false,
            "mimeType": "multipart/form-data",
            "contentType": false,
            "data": form
        };
        abp.ui.setBusy();
        $.ajax(settings).done(function (response) {
            const rs = JSON.parse(response);
            if (rs.success === true) {
                $showImageElm.append(' <li class="image-area" ><img src="' + rs.result + '" /> <a class="remove-image" href="#" style="display: inline;">&#215;</a></li>');

                $showImageElm.find(".remove-image").on('click', function () {
                    $(this).parents('li').remove();
                });

            } else {
                abp.notify.error("Upload file không thành công");
            }
        }).always(function () {
                abp.ui.clearBusy();
            }
        );
    };

    app.uploadFile = function (fileInput, inputVal) {
        const form = new FormData();
        form.append("file", fileInput[0].files[0]);
        const settings = {
            "url": "/api/services/app/File/UploadFile",
            "method": "POST",
            "timeout": 0,
            "processData": false,
            "mimeType": "multipart/form-data",
            "contentType": false,
            "data": form
        };
        abp.ui.setBusy();
        $.ajax(settings).done(function (response) {
            const rs = JSON.parse(response);
            if (rs.success === true) {
                inputVal.val(rs.result);
            } else {
                abp.notify.error("Upload file không thành công");
            }
        }).always(function () {
                abp.ui.clearBusy();
            }
        );
    }
})();
