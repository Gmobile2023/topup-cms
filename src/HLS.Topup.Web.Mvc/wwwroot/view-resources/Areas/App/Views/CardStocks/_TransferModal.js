(function ($) {
    app.modals.TransferStockModal = function () {

        var _cardStocksService = abp.services.app.cardStocks;

        var _modalManager;
        var _$cardStockInformationForm = null;


        this.init = function (modalManager) {
            _modalManager = modalManager;

            var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$cardStockInformationForm = _modalManager.getModal().find('form[name=CardStockInformationsForm]');
            _$cardStockInformationForm.validate();
            modal.find('.select2').select2({"width": "100%"});
            modal.find('#serviceCode').on('change', serviceChangeModal);
            modal.find('#vendorCode').on('change', categoryChangeModal);
        };


        this.save = function () {
            if (!_$cardStockInformationForm.valid()) {
                return;
            }

            var cardStock = _$cardStockInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _cardStocksService.transferStock(
                cardStock
            ).done(function (rs) {
                console.log(rs);
                if (rs.responseCode === "1") {
                    abp.message.info(app.localize('SavedSuccessfully'));
                    _modalManager.close();
                    abp.event.trigger('app.createOrEditCardStockModalSaved');
                } else {
                    abp.message.error(rs.responseMessage);
                }
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };

        function serviceChangeModal(e) {
            var $e = $(e.target);
            let _s = $e.closest('.modal-body');
            var cateCode = $e.val();
            abp.services.app.cards.getCategoryByServiceCode(cateCode)
                .done(function (result) {
                    let html = "<option value=\"\">Chọn loại sản phẩm</option>";
                    if (result.length > 0) {
                        for (let i = 0; i < result.length; i++) {
                            let item = result[i];
                            html += ("<option value=\"" + item.categoryCode + "\">" + item.categoryName + " </option>");
                        }
                    }
                    _s.find("[name='vendorCode']").html(html);
                    _s.find("[name='cardValue']").html("<option value=\"\">Chọn mệnh giá</option>");
                })
                .always(function () {
                    abp.ui.clearBusy();
                });
        }

        function categoryChangeModal(e) {
            var $e = $(e.target);
            let _s = $e.closest('.modal-body');
            var cateCode = $e.val();
            abp.services.app.cards.getProductByCategory(cateCode)
                .done(function (result) {
                    let html = "<option value=\"\">Chọn mệnh giá</option>";
                    if (result.length > 0) {
                        for (let i = 0; i < result.length; i++) {
                            let item = result[i];
                            html += ("<option value=\"" + item.productValue + "\">" + Sv.NumberToString(item.productValue) + "đ </option>");
                        }
                    }
                    _s.find("[name='cardValue']").html(html);
                })
                .always(function () {
                    abp.ui.clearBusy();
                });
        }
    };
})(jQuery);
