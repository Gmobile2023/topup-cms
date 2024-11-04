(function ($) {
    app.modals.CreateOrEditNotificationScheduleModal = function () {

        var _notificationSchedulesService = abp.services.app.notificationSchedules;

        var _modalManager;
        var _$notificationScheduleInformationForm = null;

        var _NotificationScheduleuserLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/NotificationSchedules/UserLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/NotificationSchedules/_NotificationScheduleUserLookupTableModal.js',
            modalClass: 'UserLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

            var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L LTS'
            });
            modal.find(".select2").select2();
            _$notificationScheduleInformationForm = _modalManager.getModal().find('form[name=NotificationScheduleInformationsForm]');
            _$notificationScheduleInformationForm.validate();

            // $("#userId").select2({
            //     placeholder: 'Chọn đại lý',
            //     allowClear: true,
            //     ajax: {
            //         url: abp.appPath + "api/services/app/CommonLookup/GetListUserSearch",
            //         dataType: 'json',
            //         delay: 250,
            //         data: function (params) {
            //             return {
            //                 search: params.term,
            //                 page: params.page,
            //                 agentType: $('#AgentType').val()
            //             };
            //         },
            //         processResults: function (data, params) {
            //             params.page = params.page || 1;
            //
            //             return {
            //                 results: $.map(data.result, function (item) {
            //                     return {
            //                         text: item.accountCode + " - " + item.phoneNumber + " - " + item.fullName,
            //                         id: item.accountCode
            //                     }
            //                 }),
            //                 pagination: {
            //                     more: (params.page * 30) < data.result.length
            //                 }
            //             };
            //         },
            //         cache: true
            //     },
            //     minimumInputLength: 3,
            //     language: abp.localization.currentCulture.name
            // });
        };
        var emojis = [
            "smile", "iphone", "girl", "smiley", "heart", "kiss", "copyright", "coffee",
            "a", "ab", "airplane", "alien", "ambulance", "angel", "anger", "angry",
            "arrow_forward", "arrow_left", "arrow_lower_left", "arrow_lower_right",
            "arrow_right", "arrow_up", "arrow_upper_left", "arrow_upper_right",
            "art", "astonished", "atm", "b", "baby", "baby_chick", "baby_symbol",
            "balloon", "bamboo", "bank", "barber", "baseball", "basketball", "bath",
            "bear", "beer", "beers", "beginner", "bell", "bento", "bike", "bikini",
            "bird", "birthday", "black_square", "blue_car", "blue_heart", "blush",
            "boar", "boat", "bomb", "book", "boot", "bouquet", "bow", "bowtie",
            "boy", "bread", "briefcase", "broken_heart", "bug", "bulb",
            "person_with_blond_hair", "phone", "pig", "pill", "pisces", "plus1",
            "point_down", "point_left", "point_right", "point_up", "point_up_2",
            "police_car", "poop", "post_office", "postbox", "pray", "princess",
            "punch", "purple_heart", "question", "rabbit", "racehorse", "radio",
            "up", "us", "v", "vhs", "vibration_mode", "virgo", "vs", "walking",
            "warning", "watermelon", "wave", "wc", "wedding", "whale", "wheelchair",
            "white_square", "wind_chime", "wink", "wink2", "wolf", "woman",
            "womans_hat", "womens", "x", "yellow_heart", "zap", "zzz", "+1",
            "-1"
        ];
        var jeremy = decodeURI("J%C3%A9r%C3%A9my"); // Jérémy
        var names = ["{firt_name}", "{last_name}", "{full_name}", "{email}", "{phone}"];

        var namesMap = $.map(names, function (value, i) {
            return { 'id': i, 'name': value, 'description': value + " (thuộc tính)" };
        });
        var emojisMap = $.map(emojis, function (value, i) { return { key: value, name: value } });

        var at_config = {
            at: "@",
            data: namesMap,
            headerTpl: '<div class="atwho-header">Chọn tham số:<small>↑&nbsp;↓&nbsp;</small></div>',
            insertTpl: '${name}',
            displayTpl: "<li>${name} <small>${description}</small></li>",
            limit: 200
        };
        var emoji_config = {
            at: ":",
            data: emojisMap,
            displayTpl: "<li>${name} <img src='https://assets-cdn.github.com/images/icons/emoji/${key}.png'  height='20' width='20' /></li>",
            insertTpl: ':${key}:',
            delay: 400
        };
        //$('#NotificationsMessage_Message').atwho({
        //    at: "@",
        //    data: ['Peter', 'Tom', 'Anne']
        //});
        $inputor = $('#NotificationSchedule_Body').atwho(at_config);

        $('#OpenUserLookupTableButton').click(function () {

            var notificationSchedule = _$notificationScheduleInformationForm.serializeFormToObject();

            _NotificationScheduleuserLookupTableModal.open({
                id: notificationSchedule.userId,
                displayName: notificationSchedule.userName
            }, function (data) {
                _$notificationScheduleInformationForm.find('input[name=userName]').val(data.displayName);
                _$notificationScheduleInformationForm.find('input[name=userId]').val(data.id);
            });
        });

        $('#ClearUserNameButton').click(function () {
            _$notificationScheduleInformationForm.find('input[name=userName]').val('');
            _$notificationScheduleInformationForm.find('input[name=userId]').val('');
        });


        this.save = function () {
            if (!_$notificationScheduleInformationForm.valid()) {
                return;
            }
            if ($('#NotificationSchedule_UserId').prop('required') && $('#NotificationSchedule_UserId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('User')));
                return;
            }

            var notificationSchedule = _$notificationScheduleInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _notificationSchedulesService.createOrEdit(
                notificationSchedule
            ).done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditNotificationScheduleModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);
