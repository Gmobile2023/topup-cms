(function ($) {
    app.modals.CreateOrEditProductModal = function () {

        var _productsService = abp.services.app.products;

        var _modalManager;
        var _$productInformationForm = null;

        var _ProductcategoryLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Products/CategoryLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Products/_ProductCategoryLookupTableModal.js',
            modalClass: 'CategoryLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

            var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });
            modal.find(".select2").select2();
            _$productInformationForm = _modalManager.getModal().find('form[name=ProductInformationsForm]');
            _$productInformationForm.validate();
            $("#imageId").on('change', function () {
                app.uploadImage($("#imageId"), $('#thumbImageId'));
            });
            Sv.SetupAmountMask();
        };

        $('#OpenCategoryLookupTableButton').click(function () {

            var product = _$productInformationForm.serializeFormToObject();

            _ProductcategoryLookupTableModal.open({
                id: product.categoryId,
                displayName: product.categoryCategoryName
            }, function (data) {
                _$productInformationForm.find('input[name=categoryCategoryName]').val(data.displayName);
                _$productInformationForm.find('input[name=categoryId]').val(data.id);
            });
        });

        $('#ClearCategoryCategoryNameButton').click(function () {
            _$productInformationForm.find('input[name=categoryCategoryName]').val('');
            _$productInformationForm.find('input[name=categoryId]').val('');
        });

        //Sv.SetupAmountMask();
        $("#Product_ProductValue").on('keyup input', function (e) {
            var $element = $(this);
            var val = $element.val();
            var $str = $(".amount-to-text");
            Sv.BindMoneyToString($str, val);
        });

        this.save = function () {
            if (!_$productInformationForm.valid()) {
                return;
            }
            if ($('#Product_CategoryId').prop('required') && $('#Product_CategoryId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Category')));
                return;
            }

            var product = _$productInformationForm.serializeFormToObject();
            var image="";
            if ($("#thumbImageId").attr("src") !== "") {
                image=$("#thumbImageId").attr("src");
            }
            product.image=image;
            _modalManager.setBusy(true);
            _productsService.createOrEdit(
                product
            ).done(function () {
                abp.message.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditProductModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);
