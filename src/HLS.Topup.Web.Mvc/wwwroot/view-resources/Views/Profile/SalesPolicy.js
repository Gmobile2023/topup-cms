let productCategoryDiscountList = null;
const _$discountsTable = $('#productDiscoutTable');
const _discountsService = abp.services.app.commonLookup;
let serviceCode = "TOPUP";
let titleCate = "Nạp điện thoại";
const ctrl = {
    getProductDiscounts: function (code, title) {
        serviceCode = code;
        $("#showTitle").html(title);
        productCategoryDiscountList.ajax.reload();
    },
    showTable: function () {
        productCategoryDiscountList = _$discountsTable.DataTable({
            paging: false,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _discountsService.getPolicyAccount,
                inputFilter: function () {
                    return {
                        serviceCode: serviceCode
                    };
                }
            },
            rowGroup: {
                dataSrc: 1
            },
            columnDefs: [
                {
                    className: 'control responsive',
                    orderable: false,
                    render: function () {
                        return '';
                    },
                    targets: 0
                },
                {
                    targets: 1,
                    data: "categoryName",
                },
                {
                    targets: 2,
                    data: "productName",
                    render: function (data, type, row) {
                        return data;
                    }
                },
                {
                    targets: 3,
                    data: "discountValue",
                    render: function (data, type, row) {
                        if (serviceCode === "PAY_BILL") {
                            if (row.discountValue != null && row.fixAmount != null) {
                                return row.discountValue + "% tối đa " + Sv.format_number(row.fixAmount) + " đ";
                            } else if (row.discountValue !== null) {
                                return Sv.format_number(row.discountValue) + " %";
                            } else if (row.fixAmount !== null) {
                                return Sv.format_number(row.fixAmount) + " đ";
                            } else return "0%";
                        } else {
                            if (row.discountValue !== null) {
                                return Sv.format_number(row.discountValue) + " %";
                            } else if (row.fixAmount !== null) {
                                return Sv.format_number(row.fixAmount) + " đ";
                            } else return "0%";
                        }
                    }
                },
                {
                    targets: 4,
                    data: "paymentAmount",
                    render: function (data, type, row) {
                        if (serviceCode === "PAY_BILL") {
                            return "";
                        } else {
                            return Sv.format_number(row.paymentAmount)
                        }
                    }
                },
                {
                    targets: 5,
                    data: "paymentAmount",
                    render: function (data, type, row) {
                        var text = "";
                        if (serviceCode === "PAY_BILL") {
                            if (row.amountMinFee != null && row.subFee != null && row.amountIncrease != null && row.minFee != null) {
                                text = app.localize('ShowFeeText', Sv.format_number(row.minFee)+"đ", "<= " + Sv.format_number(row.amountMinFee)+"đ", ">= " + Sv.format_number(row.amountMinFee)+"đ", Sv.format_number(row.subFee)+"đ", Sv.format_number(row.amountIncrease)+"đ");
                            } else if (row.minFee !== null) {
                                text = Sv.format_number(row.minFee) + " đ";
                            } else return "";
                        } else {
                            return "";
                        }
                        let $container;
                        $container = $('<span class="preformatted">' + text + '</span>');
                        return $container[0].outerHTML;
                    }
                }
            ]
        });
    }
};

$(document).ready(function () {
    $("#showTitle").html(titleCate);
    ctrl.showTable();
    $('.group-help li:first-child a').addClass('help-active');

    $('.group-help li a').click(function () {
        $('.group-help li a').removeClass('help-active');
        $(this).addClass('help-active');
    });
});

