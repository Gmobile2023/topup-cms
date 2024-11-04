(function ($) {
    app.modals.AddCardModal = function () {

        var _modalManager;
        this.init = function (modalManager) {
            _modalManager = modalManager;
        };

        $("#selectService").change(function (e) {
            const serviceCode = $(e.target).val();
            Sv.GetCateByService(serviceCode, $("#selectCategory"), true);
            validateAdd();
        });

        $("#selectCategory").change(function (e) {
            const cateCode = $(e.target).val();
            Sv.GetProductByCate(cateCode, $("#selectProduct"), true);
            validateAdd();
        });

        $("#selectProduct").change(function (e) {
            const productCode = $(e.target).val();
            abp.ui.setBusy();
            abp.services.app.commonLookup.getProductByProductCode(productCode, true)
                .done(function (result) {
                    $("#hdnProductValue").val(result.productValue);
                    validateAdd();
                })
                .always(function () {
                    abp.ui.clearBusy();
                });
        });

        $('#txtQuantity').keyup(function (event) {           
            this.value = this.value.replace(/[^0-9\.]/g, '');
            validateAdd();
        });

        //$("#txtQuantity").keyup(function () {

           
        //    //alert(this.event);
        //    //console.log(this.event);
        //    //alert(this.keyup);

        //    //return valid_numbers(e);
        //});

        $('#btnAdd').click(function () {
            var data = {};
            data.serviceCode = $("#selectService").val();
            data.serviceName = $("#selectService option:selected").text();
            data.categoryCode = $("#selectCategory").val();
            data.categoryName = $("#selectCategory option:selected").text();
            data.productCode = $("#selectProduct").val();
            data.productName = $("#selectProduct option:selected").text();
            data.quantity = $("#txtQuantity").val();
            data.value = $("#hdnProductValue").val();

            if (!data.serviceCode) {
                abp.message.info("Quý khách chưa chọn dịch vụ mã thẻ.");
                return false;
            }

            if (!data.categoryCode) {
                abp.message.info("Quý khách chưa chọn nhà phát hành.");
                return false;
            }

            if (!data.productCode) {
                abp.message.info("Quý khách chưa chọn mệnh giá.");
                return false;
            }

            if (!data.quantity) {
                abp.message.info("Số lượng không hợp lệ.");
                return false;
            }

            ctrlList.addTableDataPinCodeByPopup(data);
            _modalManager.close();
        });

        validateAdd = function () {

            var serviceCode = $("#selectService").val();
            if (serviceCode == "" || serviceCode == null) {
                $('#btnAdd').hide();
                return;
            }

            var categoryCode = $("#selectCategory").val();
            if (categoryCode == "" || categoryCode == null) {
                $('#btnAdd').hide();
                return;
            }


            var productCode = $("#selectProduct").val();
            if (productCode == "" || productCode == null) {
                $('#btnAdd').hide();
                return;
            }
            var quantity = $("#txtQuantity").val();
            if (quantity == "" || quantity == null) {
                $('#btnAdd').hide();
                return;
            }

            $('#btnAdd').show();

        }
    };
})(jQuery);

