(function ($) {
    app.modals.CreateCardsApiModal = function () {

        var _cardsService = abp.services.app.cards;
        var _modalManager;
        var _dataTable;
        var _$cardInformationForm = null;
        var _base = this;      
        this.init = function (modalManager) {
            _modalManager = modalManager;

            var modal = _modalManager.getModal();
            _dataTable = this.initTable();

            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _base.initPanel();
            _base.removeRow();

            _$cardInformationForm = _modalManager.getModal().find('form[name=ImportCardsApiInformationsForm]');
            _$cardInformationForm.validate();

            modal.find(".select2").select2();
            modal.find(".button-add").on('click', _base.addRow);           
        };

        this.save = function () {
            var modal = _modalManager.getModal();
            if (!_$cardInformationForm.valid()) {
                return;
            }
            if (modal.find('#providerCode').prop('required') && modal.find('#providerCode').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('providerCode')));
                return;
            }
            var formData = _$cardInformationForm.serializeFormToObject();
            var cardsData = [];
            _dataTable.data().map(function (x, i) {
                if (x.serviceCode)
                    cardsData.push(x);
            });
            if (cardsData.length == 0) {
                abp.message.error("Không có dữ liệu thẻ cần nhập");
                return;
            }
            var postData = {
                providerCode: formData.providerCode,
                description: formData.description,
                expiredDate: formData.expiredDate,
                cardItems: cardsData,
            };
            _modalManager.setBusy(true);
            _cardsService.importCardsApi(postData).done(function (result) {                
                _modalManager.setBusy(false);
                if (result.responseStatus.errorCode === "01") {
                    abp.message.info('Xác nhận nhập thẻ từ API đối tác!. Tiến trình đang xử lý. Vui lòng chờ thông báo kết quả');
                    _modalManager.close();
                    abp.event.trigger('app.importCardsApiSaved');
                } else {
                    abp.message.warn(result.responseStatus.message);
                }
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };

        this.initTable = function () {
            let $table = _modalManager.getModal().find('#ProductApiTable');
            return $table.DataTable({
                paging: true,
                serverSide: false,
                data: [],
                ordering: false,
                columnDefs: [
                    {
                        targets: 0,
                        width: 15,
                        data: "serviceCode",
                    },
                    {
                        targets: 1,
                        width: 15,
                        data: "categoryName",
                        // render: function (data, e, row) {
                        //     return ' <select class="form-control" name="categoryCode_api"><option value="">Chọn loại sản phẩm</option> </select>'
                        // }
                    },
                    {
                        targets: 2,
                        width: 15,
                        data: "productName"
                    },
                    {
                        targets: 3,
                        width: 10,
                        data: "cardValue",
                        class: "text-right",
                        render: function (data, e, row) {
                            return '<span class="">' + (data > 0 ? (Sv.NumberToString(data) + "đ") : "0") + '</span>';
                        }
                    },
                    {
                        targets: 4,
                        width: 10,
                        class: "text-right",
                        data: "quantity",
                        render: function (data, e, row) {
                            return '<span class="">' + Sv.NumberToString(data) + '</span>';
                        }
                    },
                    {
                        targets: 5,
                        width: 10,
                        class: "text-right",
                        data: "discount",
                        render: function (data, e, row) {
                            console.log(data);
                            return '<span class="">' + (data) + '%' + '</span>';
                        }
                    },
                    {
                        targets: 6,
                        width: 15,
                        class: "text-right",
                        data: "amount",
                        render: function (data, e, row) {
                            return '<span class="">' + (data > 0 ? (Sv.NumberToString(data) + "đ") : "0") + '</span>';
                        }
                    },
                    {
                        targets: 7,
                        width: 10,
                        data: null,
                        defaultContent: '',
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
                        // render: function (data, e, row) {
                        //     return '<button type="button" class="btn btn-danger btn-sm btn-icon btn-remove"><i class="la la-times"></i></button>';
                        // }
                    }
                ],
                drawCallback: function( settings) {
                    var data = _dataTable.data();
                    console.log(data);
                    var q = 0;
                    var t = 0;
                    if(data.length > 0){
                        q = data.reduce((a, b) => parseFloat(a) + parseFloat(b["quantity"] || 0), 0);
                        t = data.reduce((a, b) => parseFloat(a) + parseFloat(b["amount"] || 0), 0);
                    }
                    $table.find("tbody.f tr td:nth-child(2)").html(Sv.NumberToString(q));
                    $table.find("tbody.f tr td:nth-child(4)").html(Sv.NumberToString(t) + "đ");
                }
            });
        };

        this.initPanel = function () {
            var $modal = _modalManager.getModal();
            let $serviceCode = $modal.find('[name="serviceCode_api"]');
            let $categoryCode = $modal.find('[name="categoryCode_api"]');
            let $productCode = $modal.find('[name="productCode_api"]');
            let $discount = $modal.find('[name="discount"]');
            let $quantity = $modal.find('[name="quantity"]');

            _base.setupS2($serviceCode, {url: abp.appPath + "api/services/app/CommonLookup/ServiceCardList"}, "Chọn dịch vụ",);
            $serviceCode.trigger('change');
            $serviceCode.on('change', function () {
                $categoryCode.val("").html("<option value=''>Chọn loại sản phẩm</option>");
                let url_get_child = (abp.appPath + "api/services/app/Cards/CategoryCardList" +
                    abp.utils.buildQueryString([
                        {name: 'serviceCode', value: $serviceCode.val()},
                        {name: 'isAll', value: false}]));
                _base.setupS2($categoryCode, {
                    url: url_get_child,
                }, "Chọn loại sản phẩm");

                $categoryCode.trigger('change');
            });

            $categoryCode.on('change', function () { 
                $productCode.val("").html("<option value=''>Chọn sản phẩm</option>");
                let url_get_child = (abp.appPath + "api/services/app/Cards/ProductCardList" +
                    abp.utils.buildQueryString([
                        {name: 'categoryCode', value: $categoryCode.val()},
                        {name: 'isAll', value: false}]));
                _base.setupS2($productCode, {
                    url: url_get_child,
                }, "Chọn sản phẩm");
                 
                $productCode.trigger('change');
            });

            $productCode.on('change', function () {
                _base.getPanel(true);
            });
            $discount.on('change', function () {
                _base.getPanel(true);
            });
            $quantity.on('change', function () {
                _base.getPanel(true);
            });
            Sv.SetupAmountMask();
        };
        this.clearPanel = function () {
            var $modal = _modalManager.getModal();
            $modal.find('[name="categoryCode_api"]').html("<option value=''>Chọn loại sản phẩm</option>");
            $modal.find('[name="productCode_api"]').html("<option value=''>Chọn sản phẩm</option>");
            $modal.find('[name="serviceCode_api"]').val("").trigger('change');
            $modal.find('[name="categoryCode_api"]').val("").trigger('change');
            $modal.find('[name="productCode_api"]').val("").trigger('change');
            $modal.find('[name="quantity"]').val("1");
            $modal.find('[name="discount"]').val("");
        }
        this.getPanel = function (updateHtml) {
            var $modal = _modalManager.getModal();
            let dataItem = {
                serviceCode: $modal.find('[name="serviceCode_api"]').val(),
                serviceName: $modal.find('[name="serviceCode_api"] option:selected').text(),
                categoryCode: $modal.find('[name="categoryCode_api"]').val(),
                categoryName: $modal.find('[name="categoryCode_api"] option:selected').text(),
                productCode: $modal.find('[name="productCode_api"]').val(),
                productName: $modal.find('[name="productCode_api"] option:selected').text(),
                cardValue: 0,
                quantity: $modal.find('[name="quantity"]').val(),
                discount: $modal.find('[name="discount"]').val(),
                amount: 0,
            };
            dataItem.discount = dataItem.discount === "" ? 0 : dataItem.discount;
            let prod = $modal.find('[name="productCode_api"]').find("option:selected").data("payload");
            if (prod != null) {
                dataItem.cardValue = prod.productValue;
                dataItem.amount = parseFloat(dataItem.cardValue) * parseFloat(dataItem.quantity) -
                    (parseFloat(dataItem.cardValue) * parseFloat(dataItem.quantity)) * (parseFloat(dataItem.discount) / 100);
                dataItem.amount = Math.round(dataItem.amount);
            }
            if (updateHtml) {
                $modal.find(".cardValue").html(dataItem.cardValue == 0 ? "0" : (Sv.NumberToString(dataItem.cardValue) + "đ"));
                $modal.find(".total_amount").html(dataItem.amount == 0 ? "0" : (Sv.NumberToString(dataItem.amount + "") + "đ"));
            }
            return dataItem;
        };
        this.addRow = function () {
            var data = _base.getPanel(false);
            if (data.serviceCode == "") {
                abp.message.info("Vui lòng chọn dịch vụ cần nhập thẻ");
                return false;
            }
            if (data.categoryCode == "") {
                abp.message.info("Vui lòng chọn loại sản phẩm cần nhập thẻ");
                return false;
            }
            if (data.productCode == "") {
                abp.message.info("Vui lòng chọn sản phẩm cần nhập thẻ");
                return false;
            }
            if (data.discount == "") {
                abp.message.info("Vui lòng nhập chiết khấu nhập thẻ");
                return false;
            }
            _dataTable.row.add(data).draw();
            _base.clearPanel();
        };
        this.removeRow = function (e) {
            if (e) {
                let $tr = $(e.target).closest('tr');
                _dataTable.rows($tr)
                    .remove()
                    .draw();
            }
        };
        this.setupS2 = function ($e, ajaxOption, title) {
            abp.ajax(ajaxOption).done(function (data) {
                let html = (title && title.length > 0) ? "<option value=''>" + title + "</option>" : "";
                if (data && data.length > 0) {
                    for (let i = 0; i < data.length; i++) {
                        let item = data[i];
                        html += "<option value='" + item.id + "' data-payload='" + JSON.stringify(item.payload) + "'>" + item.displayName + "</option>";
                    }
                }
                $e.html(html).select2({width: '100%'});
            });
        }
    };

})(jQuery);
