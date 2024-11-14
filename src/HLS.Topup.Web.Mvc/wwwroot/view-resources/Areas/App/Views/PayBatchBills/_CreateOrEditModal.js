(function ($) {
    app.modals.CreateOrEditPayBatchBillModal = function () {

        var _payBatchBillsService = abp.services.app.payBatchBills;

        var _$dataTable = $('#dataTablePayBatch');

        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z");
        }

        var _modalManager;
        var _$payBatchBillInformationForm = null;

        var _PayBatchBillproductLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/PayBatchBills/ProductLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/PayBatchBills/_PayBatchBillProductLookupTableModal.js',
            modalClass: 'ProductLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

            var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$payBatchBillInformationForm = _modalManager.getModal().find('form[name=PayBatchBillInformationsForm]');
            _$payBatchBillInformationForm.validate();
        };

        $("#SearchPayBatchBillsButton").click(function () {
            $('#idHiddenSearch').val('1');
            var name = $("#PayBatchBill_Name").val();
            if (name == null || name == "") {
                abp.message.error("Quý khách chưa nhập tên chương trình !");
                return;
            }

            var payBatchBill = _$payBatchBillInformationForm.serializeFormToObject();
            payBatchBill.name = name;
            payBatchBill.fromDate = getDateFilter($('#PayBatchBill_FromDate'));
            if (payBatchBill.fromDate == null && payBatchBill.fromDate == "") {
                abp.message.error("Quý khách chưa chọn thời gian từ !");
                return;
            }

            payBatchBill.toDate = getDateFilter($('#PayBatchBill_ToDate'));
            if (payBatchBill.toDate == null && payBatchBill.toDate == "") {
                abp.message.error("Quý khách chưa chọn thời gian tới !");
                return;
            }

            if ($('#PayBatchBill_TotalBlockBill').val() <= 0) {
                abp.message.error("Quý khách phải nhập số Block hóa đơn lớn hơn 0.");
                return;
            }

            if ($('#PayBatchBill_AmountPayBlock').val() <= 0) {
                abp.message.error("Quý khách phải nhập số tiền trên Block phải hơn 0.");
                return;
            }

            var maxAmountPay = $('#PayBatchBill_MaxAmountPay').val();
            if (maxAmountPay == null || maxAmountPay == "") {
                maxAmountPay = -1;
            }

            var minBillAmount = $('#PayBatchBill_MinBillAmount').val();
            if (minBillAmount == null || minBillAmount == "") {
                minBillAmount = 0;
            }

            payBatchBill.categoryCode = $('#selectCategoryItem').val();
            payBatchBill.productCode = $('#selectProductItem').val();

            if (payBatchBill.categoryCode == null || payBatchBill.categoryCode == "") {
                abp.message.error("Quý khách chưa chọn loại sản phẩm !");
                return;
            }



            getProviders();
        });

        $("#CancelPayBatchBillsButton").click(function () {
            $('#idHiddenSearch').val('0');
            getProviders();
        });

        $("#selectCategoryItem").change(function (e) {
            const cateCode = $(e.target).val();
            abp.ui.setBusy();
            abp.services.app.commonLookup.getProductByCategory(cateCode)
                .done(function (result) {
                    let html = "<option value=\"\">Chọn sản phẩm</option>";
                    if (result != null && result.length > 0) {
                        for (let i = 0; i < result.length; i++) {
                            let item = result[i];
                            html += ("<option value=\"" + item.productCode + "\">" + item.productName + "</option>");
                        }
                    }
                    $("#selectProductItem").html(html);
                })
                .always(function () {
                    abp.ui.clearBusy();
                });
        });

        const dataTable = _$dataTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _payBatchBillsService.payBatchBillGetRequest,
                inputFilter: function () {
                    return {
                        fromDate: getDateFilter($('#PayBatchBill_FromDate')),
                        toDate: getDateFilter($('#PayBatchBill_ToDate')),
                        categoryCode: $('#selectCategoryItem').val(),
                        productCode: $('#selectProductItem').val(),
                        blockMin: $('#PayBatchBill_TotalBlockBill').val() == null || $('#PayBatchBill_TotalBlockBill').val() == "" ? "0" : $('#PayBatchBill_TotalBlockBill').val(),
                        moneyBlock: $('#PayBatchBill_AmountPayBlock').val() == null || $('#PayBatchBill_AmountPayBlock').val() == "" ? "0" : $('#PayBatchBill_AmountPayBlock').val(),
                        billAmountMin: $('#PayBatchBill_AmountPayBlock').val() == null || $('#PayBatchBill_AmountPayBlock').val() == "" ? "0" : $('#PayBatchBill_AmountPayBlock').val(),
                        bonusMoneyMax: $('#PayBatchBill_MaxAmountPay').val() == null || $('#PayBatchBill_MaxAmountPay').val() == "-1" ? "0" : $('#PayBatchBill_MaxAmountPay').val(),
                        isSearch: $('#idHiddenSearch').val()
                    };
                }
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
                    data: "agentCode"
                },
                {
                    targets: 2,
                    data: "mobile"
                },
                {
                    targets: 3,
                    data: "fullName"
                },
                {
                    targets: 4,
                    data: "quantity",
                    className: "all text-right",
                    render: function (data) {
                        return Sv.format_number(data);
                    }
                },
                {
                    targets: 5,
                    data: "payBatchMoney",
                    className: "all text-right",
                    render: function (data) {
                        return Sv.format_number(data);
                    }
                }
            ],
            drawCallback: function (settings) {
                var rawServerResponse = this.api().settings()[0].rawServerResponse;
                if (rawServerResponse !== undefined && rawServerResponse.totalData !== undefined) {
                    _$dataTable.find("tbody.f tr td:nth-child(2)").html(Sv.NumberToString(rawServerResponse.totalData.payBatchMoney));
                }
            }     
        });

    function getProviders() {
        var blockMin = $('#PayBatchBill_TotalBlockBill').val();
        if (blockMin === null || blockMin === "")
            return;

        var billAmountMin = $('#PayBatchBill_AmountPayBlock').val();
        if (billAmountMin === null || billAmountMin === "")
            return;

        //var bonusMoneyMax = $('#PayBatchBill_MaxAmountPay').val();
        //if (bonusMoneyMax === null || bonusMoneyMax === "")
        //    return;

        //var moneyBlock = $('#PayBatchBill_AmountPayBlock').val();
        //if (moneyBlock === null || moneyBlock === "")
        //    return;

        dataTable.ajax.reload();
    }

    $("#selectCategoryItem").select2();
    $("#selectProductItem").select2();

    this.save = function () {
        //if (!_$payBatchBillInformationForm.valid()) {
        //    return;
        //}

        var name = $("#PayBatchBill_Name").val();
        if (name == null || name == "") {
            abp.message.error("Quý khách chưa nhập tên chương trình !");
            return;
        }

        var payBatchBill = _$payBatchBillInformationForm.serializeFormToObject();
        payBatchBill.name = name;
        payBatchBill.fromDate = getDateFilter($('#PayBatchBill_FromDate'));
        if (payBatchBill.fromDate == null && payBatchBill.fromDate == "") {
            abp.message.error("Quý khách chưa chọn thời gian từ !");
            return;
        }
        payBatchBill.toDate = getDateFilter($('#PayBatchBill_ToDate'));
        if (payBatchBill.toDate == null && payBatchBill.toDate == "") {
            abp.message.error("Quý khách chưa chọn thời gian tới !");
            return;
        }

        if ($('#PayBatchBill_TotalBlockBill').val() == null || $('#PayBatchBill_TotalBlockBill').val() == "" || $('#PayBatchBill_TotalBlockBill').val() <= 0) {
            abp.message.error("Quý khách phải nhập số Block hóa đơn lớn hơn 0.");
            return;
        }

        if ($('#PayBatchBill_AmountPayBlock').val() == null || $('#PayBatchBill_AmountPayBlock').val() == "" || $('#PayBatchBill_AmountPayBlock').val() <= 0) {
            abp.message.error("Quý khách phải nhập số tiền trên Block phải hơn 0.");
            return;
        }

        payBatchBill.categoryCode = $('#selectCategoryItem').val();

        if (payBatchBill.categoryCode == null || payBatchBill.categoryCode == "") {
            abp.message.error("Quý khách chưa chọn loại sản phẩm !");
            return;
        }
        payBatchBill.productCode = $('#selectProductItem').val();
        payBatchBill.totalBlockBill = ($('#PayBatchBill_TotalBlockBill').val() == null || $('#PayBatchBill_TotalBlockBill').val() == "") ? "0" : $('#PayBatchBill_TotalBlockBill').val();
        payBatchBill.amountPayBlock = ($('#PayBatchBill_AmountPayBlock').val() == null || $('#PayBatchBill_AmountPayBlock').val() == "") ? "0" : $('#PayBatchBill_AmountPayBlock').val();
        payBatchBill.maxAmountPay = ($('#PayBatchBill_MaxAmountPay').val() == null || $('#PayBatchBill_MaxAmountPay').val() == "") ? "-1" : $('#PayBatchBill_MaxAmountPay').val();
        payBatchBill.minBillAmount = ($('#PayBatchBill_MinBillAmount').val() == null || $('#PayBatchBill_MinBillAmount').val() == "") ? "0" : $('#PayBatchBill_MinBillAmount').val();

        var objCheck =
        {
            FromDate: payBatchBill.fromDate,
            ToDate: payBatchBill.toDate,
            CategoryCode: payBatchBill.categoryCode,
            ProductCode: payBatchBill.productCode
        };

        _modalManager.setBusy(true);
        _payBatchBillsService.checkPayBatchBill(
            objCheck
        ).done(function (data) {
            if (data > 0) {
                abp.message.confirm(
                    'Đã có kỳ thanh toán với Loại sản phẩm/Nhà cung cấp đã tạo trước đó bạn có muốn tạo nữa không ?',
                    app.localize('AreYouSure'),
                    function (isConfirmed) {
                        if (isConfirmed) {
                            _payBatchBillsService.createOrEdit(
                                payBatchBill
                            ).done(function () {
                                abp.notify.info(app.localize('SavedSuccessfully'));
                                _modalManager.close();
                                abp.event.trigger('app.createOrEditPayBatchBillModalSaved');
                            }).always(function () {
                                // _modalManager.setBusy(false);
                            });
                        }
                    });
            }
            else {
                _payBatchBillsService.createOrEdit(
                    payBatchBill
                ).done(function () {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    _modalManager.close();
                    abp.event.trigger('app.createOrEditPayBatchBillModalSaved');
                }).always(function () {
                    // _modalManager.setBusy(false);
                });
            }
        }).always(function () {
            _modalManager.setBusy(false);
        });
    };

    function HeadderControl(value) {
        $('#PayBatchBill_TotalBlockBill').prop('disabled', value);
        $('#PayBatchBill_AmountPayBlock').prop('disabled', value);
        $('#PayBatchBill_MinBillAmount').prop('disabled', value);
        $('#PayBatchBill_MaxAmountPay').prop('disabled', value);
        $('#PayBatchBill_FromDate').prop('disabled', value);
        $('#PayBatchBill_ToDate').prop('disabled', value);
        $('#PayBatchBill_ToDate').prop('disabled', value);
        $('#selectCategoryItem').prop('disabled', value);
        $('#selectProductItem').prop('disabled', value);
    }
};
}) (jQuery);