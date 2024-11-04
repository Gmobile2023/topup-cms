(function ($) {
    app.modals.CreateDebtModal = function () {

        var _depositsService = abp.services.app.deposits;

        var _modalManager;
        var _$depositInformationForm = null;

     

        this.init = function (modalManager) {
            _modalManager = modalManager;

            var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$depositInformationForm = _modalManager.getModal().find('form[name=DepositInformationsForm]');
            _$depositInformationForm.validate();
            Sv.SetupAmountMask();
        };


        $("#selectSale_UserId").select2({
            placeholder: 'Select',
            allowClear: true,
            ajax: {
                url: abp.appPath + "api/services/app/CommonLookup/GetListUserSaleSearch",
                dataType: 'json',
                delay: 250,
                data: function (params) {
                    return {
                        search: params.term,
                        page: params.page,
                    };
                },
                processResults: function (data, params) {
                    params.page = params.page || 1;

                    return {
                        results: $.map(data.result, function (item) {
                            return {
                                text: item.userName + "-" + item.phoneNumber + "-" + item.fullName,
                                id: item.id
                            }
                        }),
                        pagination: {
                            more: (params.page * 30) < data.result.length
                        }
                    };
                },
                cache: true
            },
            minimumInputLength: 3,
            language: abp.localization.currentCulture.name
        });

        $("#selectSale_UserId").change(function (e) {
            const userId = $(e.target).val();
            if (userId === null || userId === "") userId = 0;
            _depositsService.getLimitAvailability(userId).done(function (rs) {
                $("#UserNameSaleLimit").val(Sv.NumberToString(rs));
            });
        });

        $("#UserAgent").select2({
            placeholder: 'Select',
            allowClear: true,
            ajax: {
                url: abp.appPath + "api/services/app/CommonLookup/GetListUserSearch",
                dataType: 'json',
                delay: 250,
                data: function (params) {
                    return {
                        saleId: $("#selectSale_UserId").val(),
                        search: params.term,
                        page: params.page,
                    };
                },
                processResults: function (data, params) {
                    params.page = params.page || 1;

                    return {
                        results: $.map(data.result, function (item) {
                            return {
                                text: item.accountCode + "-" + item.phoneNumber + "-" + item.fullName,
                                id: item.id
                            }
                        }),
                        pagination: {
                            more: (params.page * 30) < data.result.length
                        }
                    };
                },
                cache: true
            },
            minimumInputLength: 3,
            language: abp.localization.currentCulture.name
        });

     
        $("#Deposit_Amount").on('keyup input', function (e) {
            const $element = $(this);
            const val = $element.val();
            const $str = $(".amount-to-text");
            Sv.BindMoneyToString($str, val);
        });

        this.save = function () {
            if (!_$depositInformationForm.valid()) {
                return;
            }
            if ($('#selectSale_UserId').prop('required') && $('#selectSale_UserId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('User')));
                return;
            }


       

            var deposit = _$depositInformationForm.serializeFormToObject();
            deposit.userId = $("#UserAgent").val();
            deposit.userSaleId = $("#selectSale_UserId").val();
            deposit.type = $("#type").val();
            _modalManager.setBusy(true);
            _depositsService.createOrEditDebtSale(
                deposit
            ).done(function () {
                abp.message.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditDepositModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);
