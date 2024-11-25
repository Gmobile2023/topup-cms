(function ($) {
    app.modals.TransferCardStockModal = function () {
        var _cardStocksService = abp.services.app.cardStocks;
        var _modalManager;
        var _dataProduct;
        var _dataTable;
        var _$cardStockInformationForm = null;
        var _base = this;

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

            modal.find('[name="transferType"]').on('change', transferTypeChange);
            modal.find('[name="batchCode"]').on('change', batchCodeChangeModal);
            modal.find('#TransferType_Batch_add').on('click', transferTypeBatchAdd);

            modal.find('[name="serviceCode"]').on('change', serviceChangeModal);
            modal.find('[name="categoryCode"]').on('change', categoryChangeModal);
            modal.find('#TransferType_Prod_add').on('click', transferTypeProdAdd);
            modal.find('#TransferProdCardTable').on('change', '.input-qty', _base.inputQtyChange);

            _dataTable = this.initTable();
        };

        this.save = function () {
            if (!_$cardStockInformationForm.valid()) {
                return;
            } 
            let productList = [];
            _dataTable.data().map(function (x, i) { 
                if(x.quantity > 0){
                    productList.push(x);
                }
            });
            if (productList.length == 0) {
                abp.message.error("Không có dữ liệu thẻ cần điều chuyển");
                return;
            }            
            var data = _$cardStockInformationForm.serializeFormToObject();
            if (data.srcStockCode == data.desStockCode) {
                abp.message.error("Kho nhận phải khác kho chuyển");
                return;
            } 
            data.productList = productList;
            abp.ui.setBusy();
            _modalManager.setBusy(true);
            _cardStocksService.stockTransferRequest(data).done(function (rs) {
                console.log(rs);
                if (rs.responseCode === "1") {
                    abp.message.info(app.localize('SavedSuccessfully'));
                    _modalManager.close();
                    abp.event.trigger('app.createOrEditCardStockModalSaved');
                } else {
                    abp.message.error(rs.responseMessage);
                }
            }).always(function () {
                abp.ui.clearBusy();
                _modalManager.setBusy(false);
            });
        };

        // type
        function clearData() {
            var modal = _modalManager.getModal();
            modal.find('.select2').select2({"width": "100%"});
            _dataProduct = [];
            _dataTable.clear();
            _dataTable.rows.add(_dataProduct).draw(); 
            modal.find('[name="batchCode"]').val("").trigger("change");
            modal.find('[name="serviceCode"]').val("").trigger("change");
            modal.find('[name="categoryCode"]').val("").trigger("change");
            modal.find('[name="productCode"]').val("").trigger("change");
        }

        function transferTypeChange(e) {
            var $e = $(e.target);
            var _val = $(e.target).val();
            if (_val == "batch") {
                $("#block_TransferType_Batch").show();
                $("#block_TransferType_Prod").hide();
            } else if (_val == "prod") {
                $("#block_TransferType_Batch").hide();
                $("#block_TransferType_Prod").show();
            }
            clearData();
        }

        // batch
        function batchCodeChangeModal(e) {
            var $e = $(e.target);
            var _val = $(e.target).val();
            var _pCode = $e.find("option:selected").data('providercode');
            var _pName = $e.find("option:selected").data('providername');
            var modal = _modalManager.getModal();
            modal.find('[name="providerCode"]').val(_pCode != null ? _pCode : "");
            modal.find('[name="providerName"]').val(_pName != null ? _pName : "");
        }

        function transferTypeBatchAdd() {
            _base.getProductByType("batch");
        }


        // prod
        function serviceChangeModal(e) {
            var $e = $(e.target);
            var _val = $(e.target).val();
            var modal = _modalManager.getModal();
            abp.services.app.cards.getCategoryByServiceCode(_val)
                .done(function (result) {
                    let html = "<option value=\"\">Chọn loại sản phẩm</option>";
                    if (result.length > 0) {
                        for (let i = 0; i < result.length; i++) {
                            let item = result[i];
                            html += ("<option value=\"" + item.categoryCode + "\">" + item.categoryName + " </option>");
                        }
                    }
                    modal.find("[name='categoryCode']").html(html);
                    modal.find("[name='productCode']").html("<option value=\"\">Chọn sản phẩm</option>");
                })
                .always(function () {
                    abp.ui.clearBusy();
                });
        }

        function categoryChangeModal(e) {
            var $e = $(e.target);
            var _val = $(e.target).val();
            var modal = _modalManager.getModal();
            abp.services.app.cards.getProductByCategory(_val)
                .done(function (result) {
                    let html = "<option value=\"\">Chọn sản phẩm</option>";
                    if (result.length > 0) {
                        for (let i = 0; i < result.length; i++) {
                            let item = result[i];
                            html += ("<option value=\"" + item.productCode + "\">" + item.productName + " </option>");
                        }
                    }
                    modal.find("[name='productCode']").html(html);
                })
                .always(function () {
                    abp.ui.clearBusy();
                });
        }

        function transferTypeProdAdd() {
            _base.getProductByType("prod");
        }

        // table
        this.initTable = function () {
            let $table = _modalManager.getModal().find('#TransferProdCardTable');
            return $table.DataTable({
                paging: true,
                serverSide: false,
                data: _dataProduct,
                ordering: false,
                scrollX: true,
                columnDefs: [
                    {
                        targets: 0,
                        width: 20,
                        data: "serviceCode",
                        className: "all",
                    },
                    {
                        targets: 1,
                        width: 20,
                        data: "categoryCode",
                        className: "all",
                    },
                    {
                        targets: 2,
                        width: 30,
                        data: "productCode",
                        className: "all",
                    },
                    {
                        targets: 3,
                        width: 10,
                        data: "cardValue",
                        class: "all text-right",
                        render: function (data, e, row) {
                            return '<span class="">' + (data > 0 ? (Sv.NumberToString(data) + "đ") : "0") + '</span>';
                        }
                    },
                    {
                        targets: 4,
                        width: 10,
                        class: "all text-right",
                        data: "quantityAvailable",
                        render: function (data, e, row) {
                            return '<span class="">' + Sv.NumberToString(data) + '</span>';
                        }
                    },
                    {
                        targets: 5,
                        width: 10,
                        data: "quantity",
                        className: "all",
                        render: function (data, type, row) {
                            return '<div><input max="'+row.quantityAvailable+'" class="input-qty form-control amount-mask text-right" type="text" value="' + data + '" style="width:100px !important; "></div>';
                        }
                    },
                    {
                        targets: 6,
                        width: 10,
                        data: null,
                        className: "all",
                        rowAction: {
                            element: $("<div/>")
                                .addClass("text-center")
                                .append($("<button/>")
                                    .addClass("btn btn-danger btn-sm btn-icon btn-remove")
                                    .attr("title", app.localize("Remove"))
                                    .append($("<i/>").addClass("la la-times"))
                                ).click(function (e) {
                                    _base.removeRow(e);
                                })
                        }
                    },
                ],
            });
        };

        this.removeRow = function (e) {
            if (e) {
                let $tr = $(e.target).closest('tr');
                _dataTable.rows($tr)
                    .remove()
                    .draw(); 
                _dataProduct = _dataTable.data();
            }
        };
        
        this.inputQtyChange = function (e) {
            if (e) {
                let $tr = $(e.target).closest('tr');
                var _val = $(e.target).val();
                var _index = $tr.index(); 
                _dataProduct = _dataProduct.map((x, i) => {
                    if (i === _index) {
                        x['quantity'] = _val;
                    } 
                    return x;
                });
            }
        };

        this.getProductByType = function(type) {
            var modal = _modalManager.getModal();
            var _stock = modal.find('[name="srcStockCode"]').val(); 
            var _batch = modal.find('[name="batchCode"]').val();
            if (_stock == null || _stock.length == 0) {
                abp.message.info("Vui lòng chọn kho nguồn");
                return;
            }
            var input = {};
            if (type === 'batch') { 
                if (_batch == null || _batch.length == 0) {
                    abp.message.info("Vui lòng chọn lô thẻ cần chuyển");
                    return;
                }
                input = {
                    "srcStockCode": _stock,
                    "transferType": "batch",
                    "batchCode": _batch
                };

            } else if (type === 'prod') {
                var _serviceCode = modal.find('[name="serviceCode"]').val();
                var _categoryCode = modal.find('[name="categoryCode"]').val();
                var _productCode = modal.find('[name="productCode"]').val();
                if (_serviceCode == null || _serviceCode.length == 0) {
                    abp.message.info("Vui lòng chọn dịch vụ cần chuyển");
                    return;
                }
                if (_categoryCode == null || _categoryCode.length == 0) {
                    abp.message.info("Vui lòng chọn loại sản phẩm cần chuyển");
                    return;
                }
                input = {
                    "srcStockCode": _stock,
                    "transferType": "prod",
                    "batchCode": "",
                    "serviceCode": _serviceCode,
                    "categoryCode": _categoryCode,
                    "productCode": _productCode,
                };
            }
            abp.ui.setBusy();
            abp.services.app.cardStocks.getCardInfoTransfer(input).done(function (data) {
                if (type === 'batch') {
                    if (data == null || data.length == 0) {
                        abp.message.info("Thẻ trong lô: "+_batch+" đã được điều chuyển khỏi kho nguồn: "+_stock+"!");
                    }
                    _dataProduct = data;
                    _dataTable.clear();
                    _dataTable.rows.add(_dataProduct).draw(); 
                } else {
                    if (data == null || data.length == 0) {
                        abp.message.info("Thẻ không còn tồn kho trong kho nguồn: "+_stock+"!");
                        return;
                    }
                    var _data = data.filter(x => {
                        return _dataProduct.filter(e => e.productCode == x.productCode).length == 0; 
                    });
                    console.log(_data);
                    if (_data == null || _data.length === 0) {
                        abp.message.info("Dữ liệu đã tồn tại trong bảng!");
                        return;
                    }
                    for (let i =0; i < _data.length; i++){
                        _dataProduct.push(_data[i]);
                    }
                    _dataTable.clear();
                    _dataTable.rows.add(_dataProduct).draw();
                }
                Sv.SetupAmountMask();
            }).always(function () {
                abp.ui.clearBusy();
            });

        };
        
    };
})(jQuery);
