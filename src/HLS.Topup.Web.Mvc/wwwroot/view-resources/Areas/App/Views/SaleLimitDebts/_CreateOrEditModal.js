(function ($) {
    app.modals.CreateOrEditSaleLimitDebtModal = function () {

        var _saleLimitDebtsService = abp.services.app.saleLimitDebts;

        var _modalManager;
        var _$saleLimitDebtInformationForm = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            Sv.SetupAmountMask();
            _$saleLimitDebtInformationForm = _modalManager.getModal().find('form[name=SaleLimitDebtInformationsForm]');
            _$saleLimitDebtInformationForm.validate();
        };

        $("#userId").select2({
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

        this.save = function () {
            if (!_$saleLimitDebtInformationForm.valid()) {
                return;
            }

            if ($('#userId').prop('required') && $('#userId').val() == '') {
                abp.message.error('Vui lòng chọn nhân viên viên Sale');
                return;
            }

            var saleLimitDebt = _$saleLimitDebtInformationForm.serializeFormToObject();
            var amount = $("#SaleLimitDebt_LimitAmount").val();
            var debtAge = $("#SaleLimitDebt_DebtAge").val();
            if (amount === "" || amount === null) {
                abp.message.error('Vui lòng nhập công nợ');
                return false;
            }

            if (debtAge === "" || debtAge === null) {
                abp.message.error('Vui lòng nhập tuổi nợ');
                return false;
            }

            if (parseInt(amount) < amount) {
                abp.message.error('Vui lòng nhập công nợ là số nguyên dương');
                return false;
            }

            if (parseInt(debtAge) < debtAge) {
                abp.message.error('Vui lòng nhập tuổi nợ là số nguyên dương');
                return false;
            }

            _modalManager.setBusy(true);
            _saleLimitDebtsService.createOrEdit(
                saleLimitDebt
            ).done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditSaleLimitDebtModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };

        $("#SaleLimitDebt_LimitAmount").on('keyup input', function (e) {
            const $element = $(this);
            const val = $element.val();
            const $str = $(".amount-to-text");
            Sv.BindMoneyToString($str, val);
        });
    };
})(jQuery);