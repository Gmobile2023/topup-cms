(function ($) {
    app.modals.CreateOrEditDepositModal = function () {

        var _depositsService = abp.services.app.deposits;
        var _commonLockup = abp.services.app.commonLookup;

        var _modalManager;
        var _$depositInformationForm = null;

        var _DeposituserLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Deposits/UserLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Deposits/_DepositUserLookupTableModal.js',
            modalClass: 'UserLookupTableModal'
        });
        var _DepositbankLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Deposits/BankLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Deposits/_DepositBankLookupTableModal.js',
            modalClass: 'BankLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

            var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });
            //modal.find(".select2").select2();
            _$depositInformationForm = _modalManager.getModal().find('form[name=DepositInformationsForm]');
            _$depositInformationForm.validate();
            Sv.SetupAmountMask();
        };

        $('#bankId').select2();

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

        $('#OpenUserLookupTableButton').click(function () {
            var deposit = _$depositInformationForm.serializeFormToObject();
            _DeposituserLookupTableModal.open({id: deposit.userId, displayName: deposit.userName}, function (data) {
                _$depositInformationForm.find('input[name=userName]').val(data.displayName);
                _$depositInformationForm.find('input[name=userId]').val(data.id);
            });
        });

        $('#ClearUserNameButton').click(function () {
            _$depositInformationForm.find('input[name=userName]').val('');
            _$depositInformationForm.find('input[name=userId]').val('');
        });

        $('#OpenBankLookupTableButton').click(function () {

            var deposit = _$depositInformationForm.serializeFormToObject();

            _DepositbankLookupTableModal.open({id: deposit.bankId, displayName: deposit.bankBankName}, function (data) {
                _$depositInformationForm.find('input[name=bankBankName]').val(data.displayName);
                _$depositInformationForm.find('input[name=bankId]').val(data.id);
            });
        });

        $('#ClearBankBankNameButton').click(function () {
            _$depositInformationForm.find('input[name=bankBankName]').val('');
            _$depositInformationForm.find('input[name=bankId]').val('');
        });

        $('#OpenUser2LookupTableButton').click(function () {

            var deposit = _$depositInformationForm.serializeFormToObject();

            _DeposituserLookupTableModal.open({
                id: deposit.approverId,
                displayName: deposit.userName2
            }, function (data) {
                _$depositInformationForm.find('input[name=userName2]').val(data.displayName);
                _$depositInformationForm.find('input[name=approverId]').val(data.id);
            });
        });

        $('#ClearUserName2Button').click(function () {
            _$depositInformationForm.find('input[name=userName2]').val('');
            _$depositInformationForm.find('input[name=approverId]').val('');
        });

        //Sv.SetupAmountMask();
        $("#Deposit_Amount").on('keyup input', function (e) {
            const $element = $(this);
            const val = $element.val();
            const $str = $(".amount-to-text");
            Sv.BindMoneyToString($str, val);
        });

        $(document).ready(function() {
            let userId = $('#select_UserId').val();
            bindSaleAssign(userId);
        });

        $("#select_UserId").change(function (e) {
            let userId = $(e.target).val();
            bindSaleAssign(userId);
        });

        function bindSaleAssign(userId) {
            if (userId === null || userId === "") return false;

            _commonLockup.getUserSaleAssign(userId).done(function (rs) {
                if (rs) {{
                    $("#SaleMan").val(rs.userName + ' - ' + rs.phoneNumber + ' - ' + rs.fullName);
                }}
            });
        }

        this.save = function () {
            if (!_$depositInformationForm.valid()) {
                return;
            }
            if ($('#Deposit_UserId').prop('required') && $('#Deposit_UserId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('User')));
                return;
            }
            if ($('#Deposit_BankId').prop('required') && $('#Deposit_BankId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Bank')));
                return;
            }
            if ($('#Deposit_ApproverId').prop('required') && $('#Deposit_ApproverId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('User')));
                return;
            }

            var deposit = _$depositInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _depositsService.createOrEdit(
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
