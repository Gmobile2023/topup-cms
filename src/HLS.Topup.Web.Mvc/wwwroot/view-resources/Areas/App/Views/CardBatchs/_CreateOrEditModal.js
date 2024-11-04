(function ($) {
    app.modals.CreateOrEditCardBatchModal = function () {

        var _cardBatchsService = abp.services.app.cardBatchs;

        var _modalManager;
        var _$cardBatchInformationForm = null;

        var _CardBatchproviderLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/CardBatchs/ProviderLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/CardBatchs/_CardBatchProviderLookupTableModal.js',
            modalClass: 'ProviderLookupTableModal'
        });
        var _CardBatchcategoryLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/CardBatchs/CategoryLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/CardBatchs/_CardBatchCategoryLookupTableModal.js',
            modalClass: 'CategoryLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

            var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$cardBatchInformationForm = _modalManager.getModal().find('form[name=CardBatchInformationsForm]');
            _$cardBatchInformationForm.validate();
            modal.find(".select2").select2();
        };

        $('#OpenProviderLookupTableButton').click(function () {

            var cardBatch = _$cardBatchInformationForm.serializeFormToObject();

            _CardBatchproviderLookupTableModal.open({
                id: cardBatch.providerId,
                displayName: cardBatch.providerName
            }, function (data) {
                _$cardBatchInformationForm.find('input[name=providerName]').val(data.displayName);
                _$cardBatchInformationForm.find('input[name=providerId]').val(data.id);
            });
        });

        $('#ClearProviderNameButton').click(function () {
            _$cardBatchInformationForm.find('input[name=providerName]').val('');
            _$cardBatchInformationForm.find('input[name=providerId]').val('');
        });

        $('#OpenCategoryLookupTableButton').click(function () {

            var cardBatch = _$cardBatchInformationForm.serializeFormToObject();

            _CardBatchcategoryLookupTableModal.open({
                id: cardBatch.categoryId,
                displayName: cardBatch.categoryCategoryName
            }, function (data) {
                _$cardBatchInformationForm.find('input[name=categoryCategoryName]').val(data.displayName);
                _$cardBatchInformationForm.find('input[name=categoryId]').val(data.id);
            });
        });

        $('#ClearCategoryCategoryNameButton').click(function () {
            _$cardBatchInformationForm.find('input[name=categoryCategoryName]').val('');
            _$cardBatchInformationForm.find('input[name=categoryId]').val('');
        });


        this.save = function () {
            if (!_$cardBatchInformationForm.valid()) {
                return;
            }
            if ($('#CardBatch_ProviderId').prop('required') && $('#CardBatch_ProviderId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Provider')));
                return;
            }
            if ($('#CardBatch_CategoryId').prop('required') && $('#CardBatch_CategoryId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Category')));
                return;
            }

            var cardBatch = _$cardBatchInformationForm.serializeFormToObject();          
            _modalManager.setBusy(true);
            _cardBatchsService.createOrEdit(
                cardBatch
            ).done(function () {
                abp.message.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditCardBatchModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);
