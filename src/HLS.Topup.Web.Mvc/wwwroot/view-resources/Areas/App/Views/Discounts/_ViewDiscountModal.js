(function ($) {
    app.modals.ViewOrEditDiscountModal = function () {
        let _discountsService = abp.services.app.discounts;
        let _commonLookup = abp.services.app.commonLookup;
        let _$discountsTable = $('#productDiscoutTable');
        let _modalManager;
        let _$discountInformationForm = null;
        let counter = 1;
        let _$data = [];

        let servicesCode = [];
        let productsType = [];
        let productStore = [];
        let limitProductList = null;

        let viewMode = $('input[name="mode"]').val();

        let productCategoryDiscountList = null;
        this.init = function (modalManager) {
            _modalManager = modalManager;

            let modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L LT'
            });

            modal.find(".select2").select2();

            _$discountInformationForm = _modalManager.getModal().find('form[name=DiscountInformationsForm]');
            _$discountInformationForm.validate();

            Sv.SetupAmountMask();

            productCategoryDiscountList = _$discountsTable.DataTable({
                data: _$data,
                destroy: true,
                paging: false,
                pageLength: 5,
                listAction: {
                    ajaxFunction: _discountsService.getDiscountDetailsTable,
                    inputFilter: function () {
                        return {
                            discountId: $("#txtId").val(),
                        };
                    }
                },
                createdRow: function (row, data, dataIndex) {
                    $(row).attr('data-id', data.id);
                    $(row).addClass('row_data_' + data.id);
                },
                columnDefs: [
                    {
                        targets: 0,
                        data: "serviceName",
                        class: "text-center"
                    },
                    {
                        targets: 1,
                        data: "categoryName",
                        class: "categoryName",
                        render: function (data, type, row) {
                            return data;
                        }
                    },
                    {
                        targets: 2,
                        data: "productName",
                        class: "productName",
                        render: function (data, type, row) {
                            return data;
                        }
                    },
                    {
                        targets: 3,
                        data: "productValue",
                        class: "text-right",
                        render: function (data) {
                            if (data) {
                                return Sv.format_number(data)
                            }
                            return "0";
                        }
                    },
                    {
                        targets: 4,
                        data: "discountValue",
                        class: "text-right",
                        render: function (data, type, row) {
                            const fixAmount = row.fixAmount === null ? "" : row.fixAmount;
                            if (viewMode === 'view') {
                                return '<input class="discount form-control txtDiscount text-right" id="txtDiscount_' + row.productId + '" data-id=' + row.productId + ' type="text" disabled data-amount=' + fixAmount + '"" value = ' + (data === null ? "" : data) + '>';
                            } else {
                                return '<input class="discount form-control txtDiscount text-right" id="txtDiscount_' + row.productId + '" data-id=' + row.productId + ' type="text" data-amount=' + fixAmount + '"" value = ' + (data === null ? "" : data) + '   >';
                            }

                        }
                    },
                    {
                        targets: 5,
                        data: "fixAmount",
                        class: "text-right",
                        render: function (data, type, row) {
                            if (viewMode === 'view') {
                                return '<input class="fixAmount form-control amount-mask txtFixAmount text-right" id="txtFixAmount_' + row.productId + '" disabled data-id=' + row.productId + ' type="text"  value = ' + (data === null ? "" : data) + '>';
                            } else {
                                return '<input class="fixAmount form-control amount-mask txtFixAmount text-right" id="txtFixAmount_' + row.productId + '"  data-id=' + row.productId + ' type="text"  value = ' + (data === null ? "" : data) + '>';
                            }
                        }
                    },
                    {
                        targets: 6,
                        data: null,
                        class: "text-right",
                        render: function (data, type, row) {
                            if (viewMode === 'view') {
                                return '';
                            } else {
                                return '<button class="btn btn-danger btn-remove" type="button" data-id="' + row.productId + '" onclick="$(\'#productDiscoutTable\').DataTable().row($(this).parents(\'tr\')).remove().draw(false)"> Xoá </button>';
                            }
                        }
                    }
                ],
                "drawCallback": function (settings) {
                    Sv.SetupAmountMask();
                },
                "rowCallback": function (row, data) {
                    Sv.SetupAmountMask();
                },
            });
            productCategoryDiscountList.on('order.dt search.dt', function () {
                productCategoryDiscountList.column(0, {
                    search: 'applied',
                    order: 'applied'
                }).nodes().each(function (cell, i) {
                    cell.innerHTML = i + 1;
                });
            }).draw();

            Sv.SetupAmountMask();

            modal.find('#AddProductsButton').off().on('click', clickAdd);

            $("#UserId").select2({
                placeholder: 'Chọn đại lý',
                allowClear: true,
                ajax: {
                    url: abp.appPath + "api/services/app/CommonLookup/GetListUserSearch",
                    dataType: 'json',
                    delay: 250,
                    data: function (params) {
                        return {
                            search: params.term, // search term
                            page: params.page,
                            agentType: $('#Discount_AgentType option:selected').val(),
                            accountType: 99
                        };
                    },
                    processResults: function (data, params) {
                        params.page = params.page || 1;

                        return {
                            results: $.map(data.result, function (item) {
                                return {
                                    text: item.accountCode + " - " + item.phoneNumber + " - " + item.fullName,
                                    id: item.id
                                }
                            }),
                            pagination: {
                                more: (params.page * 30) < data.result.length
                            }
                        };
                    },
                    cache: true
                },
                minimumInputLength: 3,
                language: abp.localization.currentCulture.name
            });
        };

        $("#Discount_AgentType").change(function () {
            $('#UserId').val(null).trigger('change');
        });

        $("#btnSearch").click(function () {
            getDiscounts();
        });

        $('#ServiceSelect').change(function () {
            servicesCode = [];
            let serviceCode = $('#ServiceSelect').val();

            if (serviceCode != null || serviceCode !== '') {
                $('#ProductType').removeAttr('disabled').empty();
                if (checkItemInArray(servicesCode, serviceCode) === -1) {
                    servicesCode.push(serviceCode);
                }
                let serviceName = $('#ServiceSelect option[value="' + serviceCode + '"]').text();
                getProductType(serviceCode, serviceName);
            } else {
                $('#ProductType').prop('disabled', true);
            }
        });

        $('#ProductType').change(function () {
            productsType = [];
            let productType = $('#ProductType').val();
            if (productType != null) {
                $('#ProductList').removeAttr('disabled').empty();
                $.each(productType, function (i, e) {
                    if (checkItemInArray(productsType, e) === -1) {
                        productsType.push(e);
                    }
                    let serviceName = $('#ProductType option[value="' + e + '"]').attr('data-service');
                    getProductList(e, serviceName);
                })
            } else {
                $('#ProductList').prop('disabled', true);
            }
        });

        $('#LimitProduct_AgentType').change(function () {
            if ($(this).val() != null || $(this).val() !== '') {
                $("#AgentFilter").removeAttr('disabled');
            } else {
                $("#AgentFilter").prop('disabled', true);
            }
        });

        $(document).on('click', '.btn-remove', function () {
            let id = $(this).attr('data-id');

            removeItemOnce(productStore, parseInt(id));
        });

        $('#ExportDiscountProductList').click(function () {
            _discountsService
                .getDetailDiscountToExcel({
                    discountId: $('input[name="id"]').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#productMode').click(function () {
            $('#productModeBlock').css({'display': 'flex'});
            $('#importModeBlock').css({'display': 'none'});
        });

        $('#importMode').click(function () {
            $('#importModeBlock').css({'display': 'block'});
            $('#productModeBlock').css({'display': 'none'});
        });

        function getProductType(serviceCode, serviceName) {
            let options = '';
            _discountsService.getCategoryByServiceCode(serviceCode).done(function (obj) {
                $.each(obj, function (key, value) {
                    $('#ProductType')
                        .append($("<option></option>")
                            .attr("value", value.id)
                            .attr("data-service", serviceName)
                            .attr("data-category", value.categoryName)
                            .text(value.categoryName));
                });
            }).always(function () {

            });
        }

        function getProductList(categoryId, serviceName) {
            let options = '';
            _discountsService.getProducts(categoryId).done(function (obj) {
                $.each(obj, function (key, value) {
                    $('#ProductList')
                        .append($("<option></option>")
                            .attr("value", value.id)
                            .attr("data-value", value.productValue)
                            .attr("data-service", serviceName)
                            .attr("data-code", value.productCode)
                            .attr("data-category", value.categoryName)
                            .text(value.productName));
                });
            }).always(function () {

            });
        }

        function clickAdd() {
            let serviceCode = $('#ServiceSelect');
            let productCode = $('#ProductType');
            let productItem = $('#ProductList');

            if (serviceCode == null || serviceCode == '') {
                abp.message.error('Vui lòng chọn dịch vụ!');
                return false;
            }

            if (productCode.val() == null || productCode.val() == '') {
                abp.message.error('Vui lòng chọn loại sản phẩm!');
                return false;
            }

            if (productItem.val() !== '' && productItem.val().length === 1) {
                if (checkItemInArray(productStore, parseInt(productItem.val())) === -1) {
                    productStore.push(parseInt(productItem.val()));
                    let productSelected = $('#ProductList option:selected');

                    let data = {
                        id: productSelected.val(),
                        service: productSelected.attr('data-service'),
                        category: productSelected.attr('data-category'),
                        value: productSelected.attr('data-value'),
                        name: productSelected.text(),
                    }

                    productCategoryDiscountList.row.add({
                        productId: data.id,
                        serviceName: data.service,
                        categoryName: data.category,
                        productName: data.name,
                        productValue: data.value,
                        discountValue: '',
                        fixAmount: '',
                    }).draw();
                }

                $('#ProductList').empty().trigger('change');
                $('#ProductType').empty().trigger('change');
                $('#ServiceSelect').val('').trigger('change');
            } else if (productItem.val() !== '' && productItem.val().length > 1) {
                let productSelected = productItem.val();

                $.each(productSelected, function (index, element) {
                    $("#ProductList option").each(function () {
                        let productItem = $(this);

                        if (parseInt(productItem.val()) === parseInt(element) && checkItemInArray(productStore, parseInt(element)) === -1) {
                            productStore.push(parseInt(element));

                            let data = {
                                id: productItem.val(),
                                service: productItem.attr('data-service'),
                                category: productItem.attr('data-category'),
                                value: productItem.attr('data-value'),
                                name: productItem.text()
                            }

                            productCategoryDiscountList.row.add({
                                productId: data.id,
                                serviceName: data.service,
                                categoryName: data.category,
                                productName: data.name,
                                productValue: data.value,
                                discountValue: '',
                                fixAmount: '',
                            }).draw();
                        }
                    });
                });

                $('#ProductList').empty().trigger('change');
                $('#ProductType').empty().trigger('change');
                $('#ServiceSelect').val('').trigger('change');
            } else {
                $("#ProductList option").each(function () {
                    let productItem = $(this);

                    if (checkItemInArray(productStore, parseInt(productItem.val())) === -1) {
                        productStore.push(parseInt(productItem.val()));

                        let data = {
                            id: productItem.val(),
                            service: productItem.attr('data-service'),
                            category: productItem.attr('data-category'),
                            value: productItem.attr('data-value'),
                            name: productItem.text()
                        }

                        console.log(data)

                        productCategoryDiscountList.row.add({
                            productId: data.id,
                            serviceName: data.service,
                            categoryName: data.category,
                            productName: data.name,
                            productValue: data.value,
                            discountValue: '',
                            fixAmount: '',
                        }).draw();
                    }
                });

                $('#ProductList').empty().trigger('change');
                $('#ProductType').empty().trigger('change');
                $('#ServiceSelect').val('').trigger('change');
            }
        }

        function checkItemInArray(arr, value) {
            return arr.indexOf(value);
        }

        function removeItemOnce(arr, value) {
            let index = arr.indexOf(value);
            if (index > -1) {
                arr.splice(index, 1);
            }

            return arr;
        }

        function getCateIds() {
            const cateIds = [];
            $.each($('#categorySelect').select2('data'), function (index, item) {
                cateIds.push(item.id);
            });
            return cateIds;
        }

        function getDiscounts() {
            productCategoryDiscountList.ajax.reload();
        }

        function filterDiscount() {
            const cateIds = getCateIds();
            const discountId = $("#txtId").val();
        }

        function find_duplicate_in_array(arr) {
            let uniqueValues = new Set(arr.map(v => v.productCode));
            return uniqueValues.size < arr.length;
        }

        function resetValueFile() {
            document.getElementById('ImportFromExcel').value = "";
        }

        function uploadFileImport() {
            let $f = _modalManager.getModal().find('input#ImportFromExcel');
            let files = $f[0].files;
            if (files && files.length > 0) {
                let file = files[0];

                //File type check
                let type = file.type.slice(file.type.lastIndexOf('/') + 1);
                let typeAlow = ['xlsx', 'xls', 'csv'];
                if (typeAlow.indexOf(type) > -1) {
                    abp.message.warn("Định dạng file không hợp lệ!");
                    resetValueFile();
                    return false;
                }

                //File size check
                if (file.size > 1048576 * 100) //100 MB
                {
                    abp.message.warn("Dung lượng file vượt quá giới hạn!");
                    resetValueFile();
                    return false;
                }
                $f.closest('label').find('span').html(file.name);

                var formData = new FormData();
                formData.append("file", file);
                _modalManager.setBusy(true);

                Sv.AjaxPostFile2({
                    url: abp.appPath + "App/Discounts/ReadFileImport",
                    data: formData
                }).then(function (rs) {
                    let response = rs.result;

                    let _dataTable = _$discountsTable.DataTable();
                    _dataTable.clear();

                    let data = [];
                    let path = "";
                    if (response.responseCode == "01") {
                        data = response.payload;
                    }

                    let format_error = false;

                    $.each(data, function (i, e) {
                        if (parseFloat(e.discountValue) < 0 || parseFloat(e.discountValue) >= 100) {
                            format_error = true;
                            abp.message.warn("File Import có số tiền không đúng định dạng!");
                            return false;
                        }
                    });

                    if (find_duplicate_in_array(data)) {
                        abp.message.warn("File Import có sản phẩm trùng lặp");
                        return false;
                    }

                    if (format_error) {
                        resetValueFile();
                        _dataTable.clear();
                        return false;
                    }

                    _dataTable.rows.add(data).draw();
                    Sv.SetupAmountMask();

                    if (response.responseCode != "01") {
                        resetValueFile();
                        abp.message.warn(response.responseMessage);
                    }
                }).always(function () {
                    _modalManager.setBusy(false);
                });

            } else {
                _dataTable.rows.add([]).draw();
                $f.closest('label').find('span').html('Chọn File');
            }
        }

        this.save = function () {
            if (!_$discountInformationForm.valid()) {
                return;
            }
            if ($('#Discount_UserId').prop('required') && $('#Discount_UserId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('User')));
                return;
            }

            const discount = _$discountInformationForm.serializeFormToObject();
            const listDiscount = [];
            productCategoryDiscountList.column(4).nodes().to$().each(function (index) {
                const productId = $(this).find('.txtDiscount').attr('data-id');
                const discountValue = $(this).find('.txtDiscount').val();
                const fixAmount = $("" + "#txtFixAmount_" + productId + "").val()
                //console.log(fixAmount);
                const item = {
                    productId: productId,
                    discountValue: discountValue,
                    fixAmount: fixAmount
                };
                listDiscount.push(item);
            });

            discount.discountDetail = listDiscount;

            _modalManager.setBusy(true);
            _discountsService.createOrEdit(
                discount
            ).done(function () {
                abp.message.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditDiscountModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);
