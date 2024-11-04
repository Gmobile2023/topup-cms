(function ($) {
    app.modals.CreateOrEditLimitProductModal = function () {
        let _limitProductsService = abp.services.app.limitProducts;
        let _commonService = abp.services.app.commonLookup;

        let _modalManager;
        let _$limitProductInformationForm = null;
        let _$data = [];

        let _$limitProductsTable = $('#LimitProductsDetailTable');

        const _LimitProductuserLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/LimitProducts/UserLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/LimitProducts/_LimitProductUserLookupTableModal.js',
            modalClass: 'UserLookupTableModal'
        });

        let servicesCode = [];
        let productsType = [];
        let productStore = [];
        let limitProductList = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;
            let modal = _modalManager.getModal();

            modal.find('input#ImportFromExcel').on('change', uploadFileImport);
            
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L LTS'
            });

            modal.find('.select2').select2();

            Sv.SetupAmountMask();

            _$limitProductInformationForm = _modalManager.getModal().find('form[name=LimitProductInformationsForm]');
            _$limitProductInformationForm.validate();

            limitProductList = _$limitProductsTable.DataTable({
                destroy: true,
                paging: false,
                serverSide: false,
                data: _$data,
                ordering: false,
                //pageLength: 250,
                columnDefs: [
                    {
                        targets: 0,
                        data: "order",
                        render: function ( data, type, row, meta ) {
                            return  meta.row + 1;
                        }
                    },
                    {
                        targets: 1,
                        data: "serviceName",
                        class: "serviceName",
                    },
                    {
                        targets: 2,
                        data: "productType",
                        class: "productType",
                    },
                    {
                        targets: 3,
                        data: "productName",
                        class: "productName",
                    },
                    {
                        targets: 4,
                        data: "limitQuantity",
                        class: "text-right",
                        render: function (data, type, row) {
                            let limitQuantity = row.limitQuantity === null ? "" : (row.limitQuantity === -1 ? "" : row.limitQuantity);
                            return '<input class="limit-product form-control amount-mask txtLimitQuantity text-right" id="txtLimitQuantity_' + row.productId + '" data-id=' + row.productId + ' type="text" value = ' + limitQuantity + '   >';
                        }
                    },
                    {
                        targets: 5,
                        data: "limitAmount",
                        class: "text-right",
                        render: function (data, type, row) {
                            let limitAmount = row.limitAmount === null ? "" : (row.limitAmount === -1 ? "" : row.limitAmount);
                            return '<input class="limit-product form-control amount-mask txtLimitAmount text-right" id="txtLimitAmount_' + row.productId + '" data-id=' + row.productId + ' type="text" value = ' + limitAmount + '   >';
                        }
                    },
                    {
                        targets: 6,
                        data: "action",
                        render: function (data, type, row) {
                            return '<button class="btn btn-danger btn-remove" type="button" data-id="' + row.productId + '" onclick="javascript:$(\'#LimitProductsDetailTable\').DataTable().row($(this).parents(\'tr\')).remove().draw(false)"> Xoá </button>';
                        }
                    }
                ],
                "drawCallback": function( settings ) {
                    Sv.SetupAmountMask();
                },
                "rowCallback": function( row, data ) {
                    Sv.SetupAmountMask();
                },
            });

            modal.find('#GetLimitProductsButton').off().on('click', clickAdd);

            $("#AgentFilter").select2({
                placeholder: 'Chọn đại lý',
                allowClear: true,
                ajax: {
                    url: abp.appPath + "api/services/app/CommonLookup/GetListUserSearch",
                    dataType: 'json',
                    delay: 250,
                    data: function (params) {
                        return {
                            search: params.term,
                            page: params.page,
                            agentType: $('#LimitProduct_AgentType').val()
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

        $('#ServiceSelect').change(function () {
            servicesCode = [];
            let serviceCode = $('#ServiceSelect').val();

            if (serviceCode != null || serviceCode !== '') {
                $('#ProductType').removeAttr('disabled').empty();
                if (checkItemInArray(servicesCode, serviceCode) === -1) {
                    servicesCode.push(serviceCode);
                }
                let serviceName = $('#ServiceSelect option[value="'+ serviceCode +'"]').text();
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
                    let serviceName = $('#ProductType option[value="'+ e +'"]').attr('data-service');
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

        $(document).on('keyup', '.limit-product', function () {
            let reg = /^\d+$/;

            if(!reg.test($(this).val())) {
                return $(this).val($(this).val().slice(0, -1));
            }
        });

        $('#productMode').click(function() {
            $('#productModeBlock').css({'display': 'flex'});
            $('#importModeBlock').css({'display': 'none'});
            _$limitProductsTable.DataTable().clear().draw();
        });

        $('#importMode').click(function() {
            $('#importModeBlock').css({'display': 'block'});
            $('#productModeBlock').css({'display': 'none'});
            _$limitProductsTable.DataTable().clear().draw();
        });

        function getProductType(serviceCode, serviceName) {
            let options = '';
            _limitProductsService.getCategoryByServiceCode(serviceCode).done(function (obj) {
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
            _limitProductsService.getProducts(categoryId).done(function (obj) {
                $.each(obj, function (key, value) {
                    $('#ProductList')
                        .append($("<option></option>")
                            .attr("value", value.id)
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
            
            console.log(productItem.val())

            if (productItem.val() !== '' && productItem.val().length === 1) {
                if (checkItemInArray(productStore, parseInt(productItem.val())) === -1) {
                    productStore.push(parseInt(productItem.val()));
                    let productSelected = $('#ProductList option:selected');

                    let data = {
                        id: productSelected.val(),
                        service: productSelected.attr('data-service'),
                        category: productSelected.attr('data-category'),
                        name: productSelected.text()
                    }

                    limitProductList.row.add({
                        productId: data.id,
                        serviceName: data.service,
                        productType: data.category,
                        productName: data.name,
                        limitQuantity: '',
                        limitAmount: '',
                        action: ''
                    }).draw();
                }

                $('#ProductList').empty().trigger('change');
                $('#ProductType').empty().trigger('change');
                $('#ServiceSelect').val('').trigger('change');
            }
            else if (productItem.val() !== '' && productItem.val().length > 1) {
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
                                name: productItem.text()
                            }

                            limitProductList.row.add({
                                productId: data.id,
                                serviceName: data.service,
                                productType: data.category,
                                productName: data.name,
                                limitQuantity: '',
                                limitAmount: '',
                                action: ''
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
                            name: productItem.text()
                        }

                        limitProductList.row.add({
                            productId: data.id,
                            serviceName: data.service,
                            productType: data.category,
                            productName: data.name,
                            limitQuantity: '',
                            limitAmount: '',
                            action: ''
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

        function find_duplicate_in_array(arr) {
            let uniqueValues = new Set(arr.map(v => v.productCode));
            return uniqueValues.size < arr.length;
        }

        function resetValueFile(){
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
                    url: abp.appPath + "App/LimitProducts/ReadFileImport",
                    data: formData
                }).then(function(rs){
                    let response = rs.result;

                    let _dataTable = _$limitProductsTable.DataTable();
                    _dataTable.clear();

                    let data = [];
                    let path = "";
                    if (response.responseCode == "01"){
                        data = response.payload;
                    }

                    let format_error = false;

                    $.each(data, function(i, e) {
                        if (parseInt(e.limitQuantity) < -2) {
                            format_error = true;
                            abp.message.warn("File Import có hạn mức số lượng không đúng định dạng!");
                            return false;
                        }
                        
                        if (parseFloat(e.limitAmount) < -2) {
                            format_error = true;
                            abp.message.warn("File Import có hạn mức số tiền không đúng định dạng!");
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

                    if(response.responseCode != "01"){
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
            if (!_$limitProductInformationForm.valid()) {
                return;
            }
            
            if ($('#LimitProduct_UserId').prop('required') && $('#LimitProduct_UserId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('User')));
                return false;
            }

            if ($('input[name="fromDate"]').val() == '' || $('input[name="toDate"]').val() == '') {
                abp.message.error('Vui lòng chọn thời gian áp dụng!');
                return false;
            }

            if ($('#LimitProduct_AgentType').prop('required') && $('#LimitProduct_AgentType').val() == null) {
                abp.message.error('Vui lòng chọn loại đại lý!');
                return false;
            }

            let limitProduct = _$limitProductInformationForm.serializeFormToObject();
            let limitProductArr = [];
            let step = true;
            limitProductList.column(5).nodes().to$().each(function (index) {
                let productId = $(this).find('.limit-product').attr('data-id');
                let limitQuantity = $("#txtLimitQuantity_" + productId + "").val();
                let limitAmount = $("#txtLimitAmount_" + productId + "").val();

                if (limitQuantity == '' && limitAmount == '') {
                    step = false;
                }

                let item = {
                    productId: productId,
                    limitQuantity: limitQuantity,
                    limitAmount: limitAmount,
                };

                limitProductArr.push(item);
            });

            if (!step) {
                abp.message.error('Vui lòng nhập Hạn mức bạn hàng!');
                return false;
            }

            limitProduct.limitProductsDetail = limitProductArr;

            delete limitProduct.LimitProductsDetailTable_length;
            delete limitProduct.productType;
            delete limitProduct.service;

            limitProduct.listUserId = $('#AgentFilter').val();

            _modalManager.setBusy(true);
            _limitProductsService.createOrEdit(
                limitProduct
            ).done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditLimitProductModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);