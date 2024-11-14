(function ($) {
    app.modals.CreateOrEditDepositCashModal = function () {

        var _depositsService = abp.services.app.deposits;
        var _commonLockup = abp.services.app.commonLookup;

        var _modalManager;
        var _$depositInformationForm = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$depositInformationForm = _modalManager.getModal().find('form[name=DepositCashInformationsForm]');
            _$depositInformationForm.validate();
            Sv.SetupAmountMask();
        };

        $("#select_UserId").select2({
            placeholder: 'Chọn người dùng',
            allowClear: true,
            ajax: {
                url: abp.appPath + "api/services/app/CommonLookup/GetListUserSearch",
                dataType: 'json',
                delay: 250,
                data: function (params) {
                    return {
                        search: params.term,
                        page: params.page,
                        agentType: 99
                    };
                },
                processResults: function (data, params) {
                    params.page = params.page || 1;

                    return {
                        results: $.map(data.result, function (item) {
                            return {
                                text: item.accountCode + " - " + item.phoneNumber + " - " + item.fullName,
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

        $(document).ready(function () {
            let userId = $('#select_UserId').val();
            bindSaleAssign(userId);
        });

        $("#select_UserId").change(function (e) {
            let userId = $(e.target).val();
            bindSaleAssign(userId);
        });

        $("#Deposit_Amount").on('keyup input', function (e) {
            const $element = $(this);
            const val = $element.val();
            const $str = $(".amount-to-text");
            Sv.BindMoneyToString($str, val);
        });

        function bindSaleAssign(userId) {
            if (userId === null || userId === "") return false;

            _commonLockup.getUserSaleAssign(userId).done(function (rs) {
                if (rs) {
                    $("#SaleMan").val(rs.userName + ' - ' + rs.phoneNumber + ' - ' + rs.fullName);
                }
            });
        }

        this.save = function () {
            if (!_$depositInformationForm.valid()) {
                return;
            }
            if ($('#select_UserId').prop('required') && $('#select_UserId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('User')));
                return;
            }

            var deposit = _$depositInformationForm.serializeFormToObject();
            deposit.userId = $("#select_UserId").val();
            deposit.type = $("#type").val();
            _modalManager.setBusy(true);
            _depositsService.createOrEditDepositCash(
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
