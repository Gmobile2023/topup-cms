(function ($) {
    app.modals.EditQuantityModal = function () {

        var _cardStocksService = abp.services.app.cardStocks;

        var _modalManager;
        var _$cardStockInformationForm = null;

        var _CardStockcategoryLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/CardStocks/CategoryLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/CardStocks/_CardStockCategoryLookupTableModal.js',
            modalClass: 'CategoryLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

            var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$cardStockInformationForm = _modalManager.getModal().find('form[name=CardStockInformationsForm]');
            _$cardStockInformationForm.validate();
            modal.find('.select2').select2({ "width": "100%" });
        };

        $('#OpenCategoryLookupTableButton').click(function () {

            var cardStock = _$cardStockInformationForm.serializeFormToObject();
            _CardStockcategoryLookupTableModal.open({ id: cardStock.categoryId, displayName: cardStock.categoryCategoryName }, function (data) {
                _$cardStockInformationForm.find('input[name=categoryCategoryName]').val(data.displayName);
                _$cardStockInformationForm.find('input[name=categoryId]').val(data.id);
            });
        });

        $('#ClearCategoryCategoryNameButton').click(function () {
            _$cardStockInformationForm.find('input[name=categoryCategoryName]').val('');
            _$cardStockInformationForm.find('input[name=categoryId]').val('');
        });

        this.save = function () {
            if (!_$cardStockInformationForm.valid()) {
                return;
            }
            if ($('#CardStock_CategoryId').prop('required') && $('#CardStock_CategoryId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Category')));
                return;
            }

            var cardStock = _$cardStockInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _cardStocksService.updateEditQuantity(
                cardStock
            ).done(function () {
                abp.message.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditCardStockModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);
