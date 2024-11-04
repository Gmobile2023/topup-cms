(function ($) {
    app.modals.CreateOrEditAccountingEntryModal = function () {

        var _depositsService = abp.services.app.deposits;

        var _modalManager;
        var _$accountingEntryInformationForm = null;

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

            modal.find('input#FileAttachment').on('change', uploadFileAttachment);
            //modal.find(".select2").select2();
            _$accountingEntryInformationForm = _modalManager.getModal().find('form[name=AccountingEntryInformationsForm]');
            _$accountingEntryInformationForm.validate();
            Sv.SetupAmountMask();
        };

        $("#Select_UserId").select2({
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

        //Sv.SetupAmountMask();
        $("#Deposit_Amount").on('keyup input', function (e) {
            const $element = $(this);
            const val = $element.val();
            const $str = $(".amount-to-text");
            Sv.BindMoneyToString($str, val);
        });

        function uploadFileAttachment() {
            let $f = _modalManager.getModal().find('input#FileAttachment');
            let files = $f[0].files;
            if (files && files.length > 0) {
                let file = files[0];
                //File type check
                let type = file.type.slice(file.type.lastIndexOf('/') + 1);
                let typeAlow = ['png', 'jpg', 'jpeg', 'pdf'];
                if (typeAlow.indexOf(type) == -1) {
                    abp.message.warn("Định dạng File văn bản đính kèm không hợp lệ!");
                    resetValueFile();
                    return false;
                }
                //File size check
                if (file.size > 1048576 * 100) //100 MB
                {
                    abp.message.warn("Dung lượng File vượt quá giới hạn 100MB!");
                    resetValueFile();
                    return false;
                }
                $f.closest('label').find('span').html(file.name);

                let form = new FormData();
                form.append("file", files[0]);
                let settings = {
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
                    let rs = JSON.parse(response);
                    if (rs.success === true) {
                        $('#FileAttachmentSrc').val(rs.result);
                    } else {
                        abp.notify.error("Upload file không thành công!");
                    }
                }).always(function () {
                        abp.ui.clearBusy();
                    }
                );

            } else {
                _dataTable.rows.add([]).draw();
                $f.closest('label').find('span').html('Chọn File');
            }
        }

        function resetValueFile() {
            document.getElementById('FileAttachment').value = "";
        }

        this.save = function () {
            if (!_$accountingEntryInformationForm.valid()) {
                return;
            }
            if ($('#Deposit_UserId').prop('required') && $('#Deposit_UserId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('User')));
                return;
            }
            if ($('#Deposit_Type').prop('required') && $('#Deposit_Type').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('AccountingEntry_Type')));
                return;
            }
            if ($('#Deposit_ApproverId').prop('required') && $('#Deposit_ApproverId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('User')));
                return;
            }

            var deposit = _$accountingEntryInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _depositsService.createOrEdit(
                deposit
            ).done(function () {
                abp.message.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditAccountingEntryModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);
