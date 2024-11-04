//const { stat } = require("fs");

(function ($) {
    app.modals.CreateOrEditSaleManModal = function () {
        const _profileService = abp.services.app.profile;
        const _passwordComplexityHelper = new app.PasswordComplexityHelper();
        let _saleMansService = abp.services.app.saleMans;

        let _modalManager;
        let _$saleManInformationForm = null;

        let _SaleManuserLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/SaleMans/UserLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/SaleMans/_SaleManUserLookupTableModal.js',
            modalClass: 'UserLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

            let modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$saleManInformationForm = _modalManager.getModal().find('form[name=SaleManInformationsForm]');
            _profileService.getPasswordComplexitySetting().done(function (result) {
                _$saleManInformationForm.validate();
                _passwordComplexityHelper.setPasswordComplexityRules(_$saleManInformationForm.find("input[name=password]"), result.setting);
            });
            _$saleManInformationForm.validate();
            modal.find('.select2').select2();

            showHideUserLead();

            $("#saleLeadUserId").select2({
                placeholder: 'Select',
                allowClear: true,
                ajax: {
                    url: abp.appPath + "api/services/app/CommonLookup/GetUserSaleLeader",
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
        };

        $('#OpenUserLookupTableButton').click(function () {

            let saleMan = _$saleManInformationForm.serializeFormToObject();

            _SaleManuserLookupTableModal.open({ id: saleMan.userId, displayName: saleMan.userName }, function (data) {
                _$saleManInformationForm.find('input[name=userName]').val(data.displayName);
                _$saleManInformationForm.find('input[name=userId]').val(data.id);
            });
        });

        $('#ClearUserNameButton').click(function () {
            _$saleManInformationForm.find('input[name=userName]').val('');
            _$saleManInformationForm.find('input[name=userId]').val('');
        });

        this.save = function () {

            if (!_$saleManInformationForm.valid()) {
                return;
            }

            if ($('#SaleMan_UserId').prop('required') && $('#SaleMan_UserId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('User')));
                return;
            }

            let saleMan = _$saleManInformationForm.serializeFormToObject();
            saleMan.isActive = $("#SaleMan_Status").val();
            _modalManager.setBusy(true);
            _saleMansService.createOrEdit(
                saleMan
            ).done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditSaleManModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };

        $('#SaleMan_SaleType').change(function () {
            showHideUserLead();
        });

        $('#SaleMan_Phone').change(function () {
            let v = VietNamMobile.valid('' + $(this).val() + '');
            if (v.length > 0) {
                abp.message.error('Sai định dạng Số điện thoại');
                $(this).val('');
                $(this).focus();
            }
        });

        function showHideUserLead() {
            let type = $('#SaleMan_SaleType').val();
            let record_id = _$saleManInformationForm.find('input[name="id"]').val();
            let is_edit_mode = false;

            is_edit_mode = record_id !== '' && record_id > 0;

            if (is_edit_mode) {
                $('#SaleMan_Password').removeAttr('required');
                $('#saleLeadUserId').attr('required', 'required');
            } else {
                $('#SaleMan_Password').attr('required', 'required');
            }

            if (is_edit_mode && parseInt(type) === 5 || parseInt(type) === 5) {
                $('.sale-man-manager').css({ 'display': 'none' });
            } else {
                $('.sale-man-manager').css({ 'display': 'block' });
            }
        }
    };
})(jQuery);
