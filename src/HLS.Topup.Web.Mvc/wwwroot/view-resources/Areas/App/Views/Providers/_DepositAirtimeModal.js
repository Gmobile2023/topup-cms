(function ($) {
    app.modals.DepositAirtimeModal = function () {

        var _stocksAirtimesService = abp.services.app.stocksAirtimes;
        var _modalManager;
        this.init = function (modalManager) {
            _modalManager = modalManager;
            Sv.SetupAmountMask();
        };

        $("#StocksAirtime_Amount").on('keyup input', function (e) {
            const $element = $(this);
            const val = $element.val();
            const $str = $(".amount-to-text");
            Sv.BindMoneyToString($str, val);
        });

        this.save = function () {
            var amount = $("#StocksAirtime_Amount").val();
            var providerCode = $("#hdnDepositProviderCode").val();
            if (amount == null || amount == "") {

                abp.notify.info("Quý khách chưa nhập số tiền.");
                return;
            }

            if (providerCode == null || providerCode == "") {

                abp.notify.info("Quý khách kiểm tra lại kênh nạp tiền.");
                return;
            }

            _modalManager.setBusy(true);
            _stocksAirtimesService.depositAirtimeViettel(
                amount,
                providerCode
            ).done(function (data) {
                if (data.responseCode === "01") {
                    abp.message.success('Nạp tiền thành công');
                } else {
                    abp.message.error(data.responseMessage);
                }
                _modalManager.close();
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);