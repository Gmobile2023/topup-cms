(function () {
    $(function () {

        var _$cardStocksTable = $('#CardStocksTable');
        var _cardStocksService = abp.services.app.cardStocks;

        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.CardStocks.Create'),
            edit: abp.auth.hasPermission('Pages.CardStocks.Edit'),
            editQuantity: abp.auth.hasPermission('Pages.CardStocks.EditQuantity'),
            'delete': abp.auth.hasPermission('Pages.CardStocks.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/CardStocks/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/CardStocks/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditCardStockModal',
            modalSize:'modal-xl'
        });

        var _editQuantityModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/CardStocks/EditQuantityModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/CardStocks/_EditQuantityModal.js',
            modalClass: 'EditQuantityModal',
            modalSize: 'modal-xl'
        });

        var _viewCardStockModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/CardStocks/ViewcardStockModal',
            modalClass: 'ViewCardStockModal',
            modalSize:'modal-xl'
        });

        var _transferStockModal_old = new app.ModalManager({
            viewUrl: abp.appPath + 'App/CardStocks/TransferModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/CardStocks/_TransferModal.js',
            modalClass: 'TransferStockModal',
            modalSize:'modal-xl'
        });

        var _transferStockModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/CardStocks/TransferCardModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/CardStocks/_TransferCardModal.js',
            modalClass: 'TransferCardStockModal',
            modalSize:'modal-xl'
        });

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z");
        }

        var dataTable = _$cardStocksTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _cardStocksService.getAll,
                inputFilter: function () {
                    return {
                        filter: $('#CardStocksTableFilter').val(),
                        stockCodeFilter: $('#stockCodeFilter').val(),
                        statusFilter: $('#StatusFilterId').val(),
                        
                        serviceCodeFilter: $('#serviceCodeFilter').val(),
                        categoryCodeFilter: $('#categoryCodeFilter').val(),
 
                        minCardValueFilter: $('#cardValueFilter').val(),
                        maxCardValueFilter: $('#cardValueFilter').val(),
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
                    width: 120,
                    targets: 1,
                    data: null,
                    orderable: false,
                    autoWidth: false,
                    defaultContent: '',
                    rowAction: {
                        cssClass: 'btn btn-brand dropdown-toggle',
                        text: '<i class="fa fa-cog"></i> ' + app.localize('Actions') + ' <span class="caret"></span>',
                        items: [
                            {
                                text: app.localize('View'),
                                action: function (data) {
                                    _viewCardStockModal.open({ code: data.record.stockCode, productCode: data.record.productCode, cardValue: data.record.cardValue });
                                }
                            },
                            {
                                text: app.localize('Edit'),
                                visible: function () {
                                    return _permissions.edit;
                                },
                                action: function (data) {
                                    _createOrEditModal.open({ code: data.record.stockCode, productCode: data.record.productCode, cardValue: data.record.cardValue });
                                }
                                // },
                                // {
                                //     text: app.localize('Delete'),
                                //     visible: function () {
                                //         return _permissions.delete;
                                //     },
                                //     action: function (data) {
                                //         deleteCardStock({ code: data.record.stockCode, productCode: data.record.productCode, cardValue: data.record.cardValue });
                                //     }
                            },
                            {
                                text: app.localize('EditQuantityStock'),
                                visible: function () {
                                    return _permissions.editQuantity;
                                },
                                action: function (data) {
                                    _editQuantityModal.open({ code: data.record.stockCode, productCode: data.record.productCode, cardValue: data.record.cardValue });
                                }                               
                            }
                        ]
                    }
                },
                {
                    targets: 2,
                    data: "stockCode",
                    name: "stockCode"
                },
                {
                    targets: 3,
                    data: "serviceName",
                    name: "serviceName"
                },
                {
                    targets: 4,
                    data: "categoryName",
                    name: "categoryName"
                },
                // {
                //     targets: 5,
                //     data: "productName",
                //     name: "productName"
                // },
                {
                    targets: 5,
                    class: "all text-right",
                    data: "cardValue",
                    name: "cardValue",
                    render: function (data, e, row) {
                        if(data == 0){
                            var arr = row.productCode.split("_");
                            var val = arr[arr.length-1];
                            return Sv.NumberToString(parseFloat(val)*1000)+"đ";
                        }
                        return Sv.NumberToString(data)+"đ";
                    }
                },
                {
                    targets: 6, 
                    data: "inventory",
                    name: "inventory",
                    class: "all text-right",
                    render: function (data, e, row) {
                        return Sv.NumberToString(data);
                    }
                },
                {
                    targets: 7,
                    data: "status",
                    name: "status"   ,
                    render: function (status) {
                        return app.localize('Enum_CardStockStatus_' + status);
                    }
                },
                {
                    targets: 8,
                    data: "inventoryLimit",
                    name: "inventoryLimit",
                    render: function (data, e, row) {
                        return Sv.NumberToString(data);
                    }
                },
                {
                    targets: 9,
                    data: "minimumInventoryLimit",
                    name: "minimumInventoryLimit",
                    render: function (data, e, row) {
                        return Sv.NumberToString(data);
                    }
                },

            ],
            "headerCallback": function (thead, data, start, end, display) {
                try {
                    var rawServerResponse = this.api().settings()[0].rawServerResponse;
                    if (rawServerResponse !== undefined && rawServerResponse.totalData !== undefined) {
                        $(thead).find('th').eq(1).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.inventory));                      
                    }
                } catch (e) {
                    console.log("không có total")
                }
            }
        });

        function getCardStocks() {
            dataTable.ajax.reload();
        }

        function deleteCardStock(cardStock) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _cardStocksService.delete({
                            id: id
                        }).done(function () {
                            getCardStocks(true);
                            abp.message.success(app.localize('SuccessfullyDeleted'));
                        });
                    }
                }
            );
        }

        $('#ShowAdvancedFiltersSpan').click(function () {
            $('#ShowAdvancedFiltersSpan').hide();
            $('#HideAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideDown();
        });

        $('#HideAdvancedFiltersSpan').click(function () {
            $('#HideAdvancedFiltersSpan').hide();
            $('#ShowAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideUp();
        });

        $('#CreateNewCardStockButton').click(function () {
            _createOrEditModal.open();
        });

        $('#TransferCardStockButton').click(function () {
            _transferStockModal.open();
        });

        $('#ExportToExcelButton').click(function () {
            _cardStocksService
                .getCardStocksToExcel({
                    filter: $('#CardStocksTableFilter').val(),
                    stockCodeFilter: $('#stockCodeFilter').val(),
                    statusFilter: $('#StatusFilterId').val(),

                    serviceCodeFilter: $('#serviceCodeFilter').val(),
                    categoryCodeFilter: $('#categoryCodeFilter').val(),

                    minCardValueFilter: $('#cardValueFilter').val(),
                    maxCardValueFilter: $('#cardValueFilter').val(),
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditCardStockModalSaved', function () {
            getCardStocks();
        });

        $('#GetCardStocksButton').click(function (e) {
            e.preventDefault();
            getCardStocks();
        });

        $(document).keypress(function(e) {
            if(e.which === 13) {
                getCardStocks();
            }
        });

        $('#AdvacedAuditFiltersArea #serviceCodeFilter').on('change', serviceChange);
        $('#AdvacedAuditFiltersArea #categoryCodeFilter').on('change', categoryChange);

        function serviceChange(e) {
            var serviceCode = $(e.target).val();
            abp.services.app.cards.categoryCardList(serviceCode, true)
                .done(function (result) {
                    let html = "<option value=\"\">Chọn loại sản phẩm</option>";
                    let _s = $("#AdvacedAuditFiltersArea");
                    if (result.length > 0) {
                        for (let i = 0; i < result.length; i++) {
                            let item = result[i];
                            html += ("<option value=\"" + item.id + "\">" +item.displayName + " </option>");
                        }
                    }
                    _s.find("#categoryCodeFilter").html(html);
                    _s.find("#cardValueFilter").html('<option value=\"\">Chọn mệnh giá</option>');
                })
                .always(function () {
                    abp.ui.clearBusy();
                });
        };
        function categoryChange(e) {
            var cateCode = $(e.target).val();
            abp.services.app.cards.getProductByCategory(cateCode)
                .done(function (result) {
                    let html = "<option value=\"\">Chọn mệnh giá</option>";
                    let _s = $("#AdvacedAuditFiltersArea");
                    if (result.length > 0) {
                        for (let i = 0; i < result.length; i++) {
                            let item = result[i];
                            html += ("<option value=\""+item.productValue+"\">"+Sv.NumberToString(item.productValue)+"đ </option>");
                        }
                    }
                    _s.find("#cardValueFilter").html(html);
                })
                .always(function () {
                    abp.ui.clearBusy();
                });
        };

    });
})();
