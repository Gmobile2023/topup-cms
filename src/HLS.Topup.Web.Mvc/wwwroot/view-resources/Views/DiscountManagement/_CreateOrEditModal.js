(function ($) {
    app.modals.CreateOrEditDiscountModal = function () {

        var _discountsService = abp.services.app.discounts;

        var _modalManager;
        var _$discountInformationForm = null;



        this.init = function (modalManager) {
            _modalManager = modalManager;

            var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$discountInformationForm = _modalManager.getModal().find('form[name=DiscountInformationsForm]');
            _$discountInformationForm.validate();
            Sv.SetupAmountMask();
        };



        this.save = function () {
            if (!_$discountInformationForm.valid()) {
                return;
            }

            var discount = _$discountInformationForm.serializeFormToObject();
            var objs = [];
            $('#tbl-discount-detail tr').each(function () {
                var input = $(this).find('input.discount-value');
                if (input.val() != undefined) {
                    var discount = "" + input.val() + "";
                    var obj = {
                        DiscountValue: discount.replace(/,/g, '.'),//.toFixed(2).replace(/,/g, '.'),
                        CategoryId: input.attr("data-id"),
                        ProductId: input.attr("product-id"),
                        DiscountId: discount.id
                    };
                    objs.push(obj);
                }
            });
            discount.DiscountDetail = objs;

            _modalManager.setBusy(true);
            _discountsService.createOrEdit(
                discount
            ).done(function (rs) {
                if (rs.responseCode === "1") {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    _modalManager.close();
                    abp.event.trigger('app.createOrEditDiscountModalSaved');
                }
                else {
                    abp.notify.error(rs.responseMessage);
                }
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);