(function ($) {
    app.modals.CreateOrEditAccountBlockBalanceModal = function () {

        var _accountBlockBalancesService = abp.services.app.accountBlockBalances;
        var _balanceService = abp.services.app.balanceAccount;

        var _modalManager;
        var _$accountBlockBalanceInformationForm = null;

        var _AccountBlockBalanceuserLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/AccountBlockBalances/UserLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/AccountBlockBalances/_AccountBlockBalanceUserLookupTableModal.js',
            modalClass: 'UserLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

            var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$accountBlockBalanceInformationForm = _modalManager.getModal().find('form[name=AccountBlockBalanceInformationsForm]');
            _$accountBlockBalanceInformationForm.validate();
            Sv.SetupAmountMask();
            $("#FileAttachment").on('change', function () {
                app.uploadFile($("#FileAttachment"), $('#FileAttachmentSrc'));
            });
        };

        $('#OpenUserLookupTableButton').click(function () {

            var accountBlockBalance = _$accountBlockBalanceInformationForm.serializeFormToObject();

            _AccountBlockBalanceuserLookupTableModal.open({
                id: accountBlockBalance.userId,
                displayName: accountBlockBalance.userName
            }, function (data) {
                _$accountBlockBalanceInformationForm.find('input[name=userName]').val(data.displayName);
                _$accountBlockBalanceInformationForm.find('input[name=userId]').val(data.id);
            });
        });

        $('#ClearUserNameButton').click(function () {
            _$accountBlockBalanceInformationForm.find('input[name=userName]').val('');
            _$accountBlockBalanceInformationForm.find('input[name=userId]').val('');
        });
        $("#AccountBlockBalance_BlockMoney").on('keyup input', function (e) {
            const $element = $(this);
            const val = $element.val();
            const $str = $(".amount-to-text");
            Sv.BindMoneyToString($str, val);
        });
        $("#select_UserId").select2({
            placeholder: 'Chọn đại lý',
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
        $("#select_UserId").change(function () {
            _balanceService.getBalanceAccountInfoById($(this).val()).done(function (rs) {
                $("#txtBalance").val(Sv.NumberToString(rs.balance));
                $("#txtAvailableBalance").val(Sv.NumberToString(rs.availableBalance));
                $("#txtBlockedMoney").val(Sv.NumberToString(rs.blockedMoney));
            });
        });
        $("#AccountBlockBalanc_Type").change(function () {
            var type = $(this).val();
            if (type === "2") {
                $("#lbl-amount").html("Nhập số tiền giải phong tỏa")
            } else {
                $("#lbl-amount").html("Nhập số tiền phong tỏa")
            }
        });

        this.save = function () {
            if (!_$accountBlockBalanceInformationForm.valid()) {
                return;
            }
            if ($('#AccountBlockBalance_UserId').prop('required') && $('#AccountBlockBalance_UserId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('User')));
                return;
            }

            var accountBlockBalance = _$accountBlockBalanceInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _accountBlockBalancesService.createOrEdit(
                accountBlockBalance
            ).done(function () {
                abp.message.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditAccountBlockBalanceModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);
