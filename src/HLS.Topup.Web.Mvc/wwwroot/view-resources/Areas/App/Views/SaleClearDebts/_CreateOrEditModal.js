(function ($) {
    app.modals.CreateOrEditSaleClearDebtModal = function () {

        var _saleClearDebtsService = abp.services.app.saleClearDebts;

        var _modalManager;
        var _$saleClearDebtInformationForm = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$saleClearDebtInformationForm = _modalManager.getModal().find('form[name=SaleClearDebtInformationsForm]');
            _$saleClearDebtInformationForm.validate();
            Sv.SetupAmountMask();
            var type = $("#SaleClearDebt_Type").val();
            if (type === "1") {
                $("#bankId").hide();
                $("#SaleClearDebt_Descriptions").show();
                $("#group_TransCodeBank").hide();
            }
            else {
                $("#bankId").show();
                $("#SaleClearDebt_Descriptions").hide();
                $("#group_TransCodeBank").show();
            }
        };

        $("#SaleClearDebt_Amount").on('keyup input', function (e) {
            const $element = $(this);
            const val = $element.val();
            const $str = $(".amount-to-text");
            Sv.BindMoneyToString($str, val);
        });

        $("#saleClearDebt_userId").select2({
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

        $("#SaleClearDebt_Type").change(function (e) {
            const type = $(e.target).val();
            if (type === "1") {
                $("#bankId").hide();
                $("#SaleClearDebt_Descriptions").show();
                $("#group_TransCodeBank").hide();
            }
            else {
                $("#bankId").show();
                $("#SaleClearDebt_Descriptions").hide();
                $("#group_TransCodeBank").show();
            }
        });

        this.save = function () {
            if (!_$saleClearDebtInformationForm.valid()) {
                return;
            }
            if ($('#saleClearDebt_userId').prop('required') && $('#saleClearDebt_userId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('User')));
                return;
            }

            var type = $("#SaleClearDebt_Type").val();
            var transCodeBank = $("#SaleClearDebt_TransCodeBank").val();
            if (type === "2") {
                if ($('#SaleClearDebt_BankId').prop('required') && $('#SaleClearDebt_BankId').val() == '') {
                    abp.message.error(app.localize('{0}IsRequired', app.localize('Bank')));
                    return;
                }

                if (transCodeBank == null || transCodeBank == "") {
                    abp.message.error("Quý khách chưa nhập mã ngân hàng.");
                    return;
                }
            }

            var amount = $('#SaleClearDebt_Amount').val();
            if (amount == null || amount == "" || amount == "0") {
                abp.message.error("Số tiền thanh toán không hợp lệ.");
                return;
            }

            var saleClearDebt =
            {
                Id: $('#id').val(),
                UserId: $('#saleClearDebt_userId').val(),
                Amount: $('#SaleClearDebt_Amount').val(),
                Type: $('#SaleClearDebt_Type').val(),
                TransCode: $('#SaleClearDebt_TransCode').val(),
                TransCodeBank: $("#SaleClearDebt_TransCodeBank").val()
            };

            if (type === "2") {
                saleClearDebt.BankId = $('#bankId').val();
                saleClearDebt.Descriptions = "";
            }
            else {
                saleClearDebt.BankId = null;
                saleClearDebt.Descriptions = $("#SaleClearDebt_Descriptions").val();
                saleClearDebt.TransCodeBank = "";
            }

            _modalManager.setBusy(true);
            _saleClearDebtsService.createOrEdit(
                saleClearDebt
            ).done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditSaleClearDebtModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);