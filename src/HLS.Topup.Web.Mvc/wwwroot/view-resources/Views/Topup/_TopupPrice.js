(function () {
    $(function () {
        const _service = abp.services.app.commonLookup;

        function getPaymentAmount(productCode) {
            //abp.ui.setBusy();
            $("#" + productCode + "").html('<img src="/themes/topup/images/1488.gif" alt="">');
            _service.getProductDiscount({productCode: productCode})
                .done((rs) => {
                    if (rs) {
                        $("#" + productCode + "").html(Sv.format_number(rs.paymentAmount)+"đ");
                    }
                })
                .always(function () {
                    //abp.ui.clearBusy();
                });
        }

        $(document).ready(function () {
            let i = 0, timeOut = 0;

            $('.eye-view-card').on('mousedown touchstart', function (e) {
                const productCode = $(this).attr('data-product-code');
                const check = $("#" + productCode + "").html();
                if (!check)
                    getPaymentAmount(productCode);
                $(this).find('.text-view').hide();
                $(this).find('.card-note__name, .card-note__price').show();

                let card_value = $(this).attr('data-value');
                if ($(window).width() <= 768) {
                    $('.card-hide-' + card_value).hide();
                    $('.card-show-' + card_value).show();
                }

                timeOut = setInterval(function () {
                }, 100);
            }).bind('mouseup mouseleave touchend', function () {
                //getPaymentAmount($(this).attr('data-product-code'));
                $(this).find('.text-view').show();
                $(this).find('.card-note__name, .card-note__price').hide();


                let card_value = $(this).attr('data-value');
                if ($(window).width() <= 768) {
                    $('.card-hide-' + card_value).show();
                    $('.card-show-' + card_value).hide();
                }

                clearInterval(timeOut);
            });
        });

    });
})();
