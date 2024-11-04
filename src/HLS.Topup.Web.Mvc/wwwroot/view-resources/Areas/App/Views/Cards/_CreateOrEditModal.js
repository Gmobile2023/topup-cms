(function ($) {
    app.modals.CreateOrEditCardModal = function () {

        var _cardsService = abp.services.app.cards;

        var _modalManager;
        var _$cardInformationForm = null;

        var _CardcardBatchLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Cards/CardBatchLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Cards/_CardCardBatchLookupTableModal.js',
            modalClass: 'CardBatchLookupTableModal'
        }); var _CardcategoryLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Cards/CategoryLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Cards/_CardCategoryLookupTableModal.js',
            modalClass: 'CategoryLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

            var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$cardInformationForm = _modalManager.getModal().find('form[name=CardInformationsForm]');
            _$cardInformationForm.validate();
            modal.find(".select2").select2();
        };

        $('#OpenCardBatchLookupTableButton').click(function () {

            var card = _$cardInformationForm.serializeFormToObject();

            _CardcardBatchLookupTableModal.open({ id: card.cardBatchId, displayName: card.cardBatchBatchName }, function (data) {
                _$cardInformationForm.find('input[name=cardBatchBatchName]').val(data.displayName);
                _$cardInformationForm.find('input[name=cardBatchId]').val(data.id);
            });
        });

        $('#ClearCardBatchBatchNameButton').click(function () {
            _$cardInformationForm.find('input[name=cardBatchBatchName]').val('');
            _$cardInformationForm.find('input[name=cardBatchId]').val('');
        });

        $('#OpenCategoryLookupTableButton').click(function () {

            var card = _$cardInformationForm.serializeFormToObject();

            _CardcategoryLookupTableModal.open({ id: card.categoryId, displayName: card.categoryCategoryName }, function (data) {
                _$cardInformationForm.find('input[name=categoryCategoryName]').val(data.displayName);
                _$cardInformationForm.find('input[name=categoryId]').val(data.id);
            });
        });

        $('#ClearCategoryCategoryNameButton').click(function () {
            _$cardInformationForm.find('input[name=categoryCategoryName]').val('');
            _$cardInformationForm.find('input[name=categoryId]').val('');
        });

        var uploadButton = $("#btnUpload");
        $("#ImportCardsFromExcel").fileupload({
            url: abp.appPath + 'App/Cards/ImportCardsFromJobExcel',
            dataType: 'json',
            showUploadedThumbs: true,
            add: function (e, data) {
                uploadButton.unbind('click');
                uploadButton.click(function () {
                    bootbox.confirm({
                        message: "Bạn có chắc chắn muốn upload danh sách thẻ này không?",
                        buttons: {
                            confirm: {
                                label: 'Đồng ý',
                                className: 'btn-success'
                            },
                            cancel: {
                                label: 'Hủy',
                                className: 'btn-danger'
                            }
                        },
                        callback: function (result) {
                            if (result === true)
                                data.submit();
                            else
                                data.abort();
                        }
                    });
                })
            },
            maxFileSize: 1048576 * 100,
            uploadExtraData: function () {
                return {
                    telco: $("#telco").val(),
                    cardbatchCode: $("#cardPackageManagementPackageCode").val(),
                    cardValue: $("#Card_Values").val()
                };
            },
            done: function (e, response) {
                console.log(response);
                console.log(e);
                var jsonResult = response.result;
                if (jsonResult.result.responseCode === "01") {
                    abp.message.info('Import file thành công!. Tiến trình đang xử lý. Vui lòng chờ thông báo kết quả');
                    _modalManager.close();
                    abp.event.trigger('app.createOrEditCardModalSaved');
                } else {
                    abp.message.warn(jsonResult.result.responseMessage);
                }
            }
        }).prop('disabled', !$.support.fileInput)
            .parent().addClass($.support.fileInput ? undefined : 'disabled');



        this.save = function () {
            if (!_$cardInformationForm.valid()) {
                return;
            }
            if ($('#Card_CardBatchId').prop('required') && $('#Card_CardBatchId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('CardBatch')));
                return;
            }
            if ($('#Card_CategoryId').prop('required') && $('#Card_CategoryId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Category')));
                return;
            }

            var card = _$cardInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _cardsService.createOrEdit(
                card
            ).done(function () {
                abp.message.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditCardModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);
