(function ($) {
    app.modals.ViewFeeModal = function () {

        let _feesService = abp.services.app.fees;
        let _commonService = abp.services.app.commonLookup;

        let _modalManager;
        let _$feeInformationForm = null;
        let _$data = [];

        let _$productFeesTable = $('#ProductFeesTable');

        let _FeeuserLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Fees/UserLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Fees/_FeeUserLookupTableModal.js',
            modalClass: 'UserLookupTableModal'
        });

        let productFeesList = null;
        let productType = [];
        let productStore = [];

        let feeMode = $('input[name="mode"]');
        let disabledStatus = '';
        if (feeMode.val() != null && feeMode.val() == 'view') {
            disabledStatus = 'disabled';
        }

        bindCategories('Fees_ProductType');

        this.init = function (modalManager) {
            _modalManager = modalManager;
            let modal = _modalManager.getModal();

            modal.find('input#ImportFromExcel').on('change', uploadFileImport);

            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L LTS'
            });

            _$feeInformationForm = _modalManager.getModal().find('form[name=FeeInformationsForm]');
            _$feeInformationForm.validate();

            modal.find('.select2').select2(0);

            productFeesList = _$productFeesTable.DataTable({
                data: _$data,
                paging: false,
                listAction: {
                    ajaxFunction: _feesService.getFeeDetailsTable,
                    inputFilter: function () {
                        return {
                            feeId: $('input[name="id"]').val(),
                            productIds: getCateIds()
                        };
                    }
                },
                columnDefs: [
                    {
                        targets: 0,
                        data: "categoryName",
                        class: "categoryName",
                        render: function (data, type, row) {
                            return data;
                        }
                    },
                    {
                        targets: 1,
                        data: "productName",
                        class: "productName",
                        render: function (data, type, row) {
                            return data;
                        }
                    },
                    {
                        targets: 2,
                        data: "minFee",
                        class: "text-right",
                        render: function (data, type, row) {
                            let minFee = row.minFee === null ? "" : row.minFee;
                            return '<input class="fees form-control amount-mask txtMinFee text-right" id="txtMinFee_' + row.productId + '" data-id=' + row.productId + ' type="text" data-amount="' + minFee + '" ' + disabledStatus + ' value = ' + (data === null ? "" : data) + '>';
                        }
                    },
                    {
                        targets: 3,
                        data: "amountMinFee",
                        class: "text-right",
                        render: function (data, type, row) {
                            let amountMinFee = row.amountMinFee === null ? "" : row.amountMinFee;
                            return '<input class="fees form-control amount-mask txtAmountMinFee text-right" id="txtAmountMinFee_' + row.productId + '" data-id=' + row.productId + ' type="text" data-amount="' + amountMinFee + '" ' + disabledStatus + ' value = ' + (data === null ? "" : data) + '>';
                        }
                    },
                    {
                        targets: 4,
                        data: "amountIncrease",
                        class: "text-right",
                        render: function (data, type, row) {
                            let amountIncrease = row.amountIncrease === null ? "" : row.amountIncrease;
                            return '<input class="fees form-control amount-mask txtAmountIncrease text-right" id="txtAmountIncrease_' + row.productId + '" data-id=' + row.productId + ' type="text" data-amount="' + amountIncrease + '" ' + disabledStatus + ' value = ' + (data === null ? "" : data) + '>';
                        }
                    },
                    {
                        targets: 5,
                        data: "subFee",
                        class: "text-right",
                        render: function (data, type, row) {
                            let subFee = row.subFee === null ? "" : row.subFee;
                            return '<input class="fees form-control amount-mask txtSubFee text-right" id="txtSubFee_' + row.productId + '" data-id=' + row.productId + ' type="text" data-amount="' + subFee + '" ' + disabledStatus + ' value = ' + (data === null ? "" : data) + '>';
                        }
                    },
                    {
                        targets: 6,
                        data: null,
                        render: function (data, type, row) {
                            return '<button class="btn btn-danger btn-remove" data-id="' + row.productId + '" onclick="$(\'#ProductFeesTable\').DataTable().row($(this).parents(\'tr\')).remove().draw(false)" ' + disabledStatus + '> Xoá </button>';
                        }
                    }
                ],
            });

            modal.find('.add-product-button').off().on('click', clickAdd);
        };

        $('#Fee_AgentType').change(function () {
            let agentType = $('#Fee_AgentType').val();

            if (agentType != '') {
                $('#Fee_AgentId').removeAttr('disabled');
            } else {
                $('#Fee_AgentId').prop('disabled', true);
            }

            $("#Fee_AgentId").select2({
                placeholder: 'Tất cả',
                allowClear: true,
                ajax: {
                    url: abp.appPath + "api/services/app/CommonLookup/GetListUserSearch",
                    dataType: 'json',
                    delay: 250,
                    data: function (params) {
                        return {
                            search: params.term,
                            page: params.page,
                            agentType: agentType,
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
        });

        $('#Fees_ProductType').change(function () {
            productType = [];
            let productTypeId = $('#Fees_ProductType').val();
            if (productTypeId != null) {
                $('#Fees_Product').removeAttr('disabled').empty();
                $.each(productTypeId, function (i, e) {
                    if (parseInt(e) > 0 && checkItemInArray(productType, parseInt(e)) === -1) {
                        productType.push(parseInt(e));
                    }

                    getProduct(parseInt(e));
                })
            } else {
                $('#Fees_Product').prop('disabled', true);
            }
        });

        // $('#Fee_AgentId').change(function () {
        //     userList = [];
        //     let userId = $(this).val();
        //     if (userId != null) {
        //         $.each(userId, function (i, e) {
        //             if (parseInt(e) > 0 && checkItemInArray(userList, parseInt(e)) === -1) {
        //                 userList.push(parseInt(e));
        //             }
        //         })
        //     }
        //     console.log(userList)
        // });

        function clickAdd() {
            let productTypeId = $('#Fees_ProductType').val();
            let productItem = $('#Fees_Product');

            if (productTypeId == null || productTypeId == '') {
                abp.message.error('Vui lòng chọn loại sản phẩm!');
                return false;
            }

            if (productItem.val() !== '' && productItem.val().length === 1) {
                if (checkItemInArray(productStore, parseInt(productItem.val())) === -1) {
                    productStore.push(parseInt(productItem.val()));
                    let productSelected = $('#Fees_Product option:selected');

                    let data = {
                        id: productItem.val(),
                        category: productSelected.attr('data-category'),
                        name: productSelected.attr('data-name')
                    }

                    productFeesList.row.add({
                        categoryName: data.category,
                        id: data.id,
                        productName: data.name,
                        productId: data.id,
                        minFee: '',
                        amountMinFee: '',
                        amountIncrease: '',
                        subFee: ''
                    }).draw();
                }

                $('#Fees_ProductType').val('').trigger('change');
                $('#Fees_Product').val('').trigger('change');
            } else if (productTypeId !== '' && productItem.val() != null && productItem.val().length > 1) {
                let feeProductSelected = productItem.val();

                $.each(feeProductSelected, function (index, element) {
                    $("#Fees_Product option").each(function () {
                        let productItem = $(this);

                        if (parseInt(productItem.val()) === parseInt(element) && checkItemInArray(productStore, parseInt(element)) === -1) {
                            productStore.push(parseInt(productItem.val()));

                            let data = {
                                id: productItem.val(),
                                category: productItem.attr('data-category'),
                                name: productItem.attr('data-name')
                            }

                            productFeesList.row.add({
                                categoryName: data.category,
                                id: data.id,
                                productName: data.name,
                                productId: data.id,
                                minFee: '',
                                amountMinFee: '',
                                amountIncrease: '',
                                subFee: ''
                            }).draw();
                        }
                    });
                });

                $('#Fees_ProductType').val('').trigger('change');
                $('#Fees_Product').val('').trigger('change');
            } else {
                $("#Fees_Product option").each(function () {
                    let productItem = $(this);

                    if (checkItemInArray(productStore, parseInt(productItem.val())) === -1) {
                        productStore.push(parseInt(productItem.val()));

                        let data = {
                            id: productItem.val(),
                            category: productItem.attr('data-category'),
                            name: productItem.attr('data-name')
                        }

                        productFeesList.row.add({
                            categoryName: data.category,
                            id: data.id,
                            productName: data.name,
                            productId: data.id,
                            minFee: '',
                            amountMinFee: '',
                            amountIncrease: '',
                            subFee: ''
                        }).draw();
                    }
                });

                $('#Fees_ProductType').val('').trigger('change');
                $('#Fees_Product').val('').trigger('change');
            }
        }


        $(document).on('click', '.btn-remove', function () {
            let id = $(this).attr('data-id');

            removeItemOnce(productStore, parseInt(id));
        });

        function bindCategories(position) {
            let options = '';
            abp.ajax({
                url: abp.appPath + "api/services/app/Fees/GetCategories",
                method: 'GET',
                data: null,
                success: function (data) {
                    $.each(data, function (index, item) {
                        options += '<option value="' + item.id + '">' + item.displayName + '</option>';
                    });

                    $('#' + position).append(options);
                }
            }).done(function () {
            });
        }

        function getProduct(id) {
            let options = '';
            _feesService.getProducts(id).done(function (obj) {
                $.each(obj, function (key, value) {
                    $('#Fees_Product')
                        .append($("<option></option>")
                            .attr("value", value.id)
                            .attr("data-category", value.categoryName)
                            .attr("data-name", value.productName)
                            .text(value.productName));
                });
            }).always(function () {

            });
        }

        function getCateIds() {
            let product_list = $('input[name="product_list"]').val();
            $.each(product_list.split(','), function (i, e) {
                if (checkItemInArray(productStore, parseInt(e)) === -1) {
                    productStore.push(parseInt(e));
                }
            })

            $('#Fees_ProductType').trigger('change');

            return product_list.split(',');
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

        $(document).on('change', '.fees', function () {
            let id = $(this).attr('data-id');

            if ($('#txtMinFee_' + id).val() != '' && $('#txtAmountMinFee_' + id).val() != '' && $('#txtAmountIncrease_' + id).val() != '' && $('#txtSubFee_' + id).val() == '') {
                $('#txtAmountMinFee_' + id).focus();
                abp.message.error('Vui lòng nhập Phụ phí!');
            }

            if ($('#txtMinFee_' + id).val() != '' && $('#txtAmountMinFee_' + id).val() != '' && $('#txtAmountIncrease_' + id).val() == '' && $('#txtSubFee_' + id).val() != '') {
                $('#txtAmountMinFee_' + id).focus();
                abp.message.error('Vui lòng nhập Block tăng thêm!');
            }

            if ($('#txtMinFee_' + id).val() != '' && $('#txtAmountMinFee_' + id).val() == '' && $('#txtSubFee_' + id).val() != '' && $('#txtAmountIncrease_' + id).val() == '') {
                $('#txtAmountMinFee_' + id).focus();
                abp.message.error('Vui lòng nhập Giá trị áp dụng phí và Phụ phí!');
            }

            if ($('#txtMinFee_' + id).val() != '' && $('#txtAmountMinFee_' + id).val() == '' && $('#txtAmountIncrease_' + id).val() != '' && $('#txtSubFee_' + id).val() != '') {
                $('#txtAmountMinFee_' + id).focus();
                abp.message.error('Vui lòng nhập Giá trị áp dụng phí!');
            }

            if ($('#txtMinFee_' + id).val() != '' && $('#txtAmountMinFee_' + id).val() == '' && $('#txtAmountIncrease_' + id).val() != '' && $('#txtSubFee_' + id).val() == '') {
                $('#txtAmountMinFee_' + id).focus();
                abp.message.error('Vui lòng nhập Giá trị áp dụng phí và Phụ phí!');
            }

            if ($('#txtMinFee_' + id).val() != '' && $('#txtAmountMinFee_' + id).val() == '' && $('#txtAmountIncrease_' + id).val() != '' && $('#txtSubFee_' + id).val() == '') {
                $('#txtAmountMinFee_' + id).focus();
                abp.message.error('Vui lòng nhập Giá trị áp dụng phí và Phụ phí!');
            }

            if ($('#txtMinFee_' + id).val() == '' && $('#txtAmountMinFee_' + id).val() != '' && $('#txtAmountIncrease_' + id).val() != '' && $('#txtSubFee_' + id).val() == '') {
                $('#txtAmountMinFee_' + id).focus();
                abp.message.error('Vui lòng nhập Phụ phí!');
            }

            if ($('#txtMinFee_' + id).val() == '' && $('#txtAmountMinFee_' + id).val() != '' && $('#txtAmountIncrease_' + id).val() == '' && $('#txtSubFee_' + id).val() == '') {
                $('#txtAmountMinFee_' + id).focus();
                abp.message.error('Vui lòng nhập Phí tối thiểu!');
            }
        });

        $(document).on('keyup', '.fees', function () {
            let reg = /^\d+$/;

            if (!reg.test($(this).val())) {
                return $(this).val($(this).val().slice(0, -1));
            }
        });

        $(document).find('.amount-mask').inputmask({
            alias: 'decimal',
            placeholder: '',
            groupSeparator: ".",
            radixPoint: ",",
            autoGroup: true,
            rightAlign: false,
            digits: "0",
            allowPlus: false,
            allowMinus: false,
            autoUnmask: true,
            integerDigits: "11"
        });

        $('#ExportDetailFees').click(function () {
            _feesService.getDetailFeesToExcel({
                feeId: $('input[name="id"]').val(),
                productIds: getCateIds()
            }).done(function (result) {
                app.downloadTempFile(result);
            });
        });

        $('#productMode').click(function () {
            $('#productModeBlock').css({'display': 'flex'});
            $('#importModeBlock').css({'display': 'none'});
            _$productFeesTable.DataTable().clear().draw();
        });

        $('#importMode').click(function () {
            $('#importModeBlock').css({'display': 'block'});
            $('#productModeBlock').css({'display': 'none'});
            _$productFeesTable.DataTable().clear().draw();
        });

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
                    url: abp.appPath + "App/Fees/ReadFileImport",
                    data: formData
                }).then(function (rs) {
                    let response = rs.result;

                    let _dataTable = _$productFeesTable.DataTable();
                    _dataTable.clear();

                    let data = [];
                    let path = "";
                    if (response.responseCode == "1") {
                        data = response.payload;
                    }

                    let format_error = false;

                    $.each(data, function (i, e) {
                        if (parseFloat(e.minFee) < 0) {
                            format_error = true;
                            abp.message.warn("File Import có phí tối thiểu không đúng định dạng!");
                            return false;
                        }

                        if (parseFloat(e.amountMinFee) < 0) {
                            format_error = true;
                            abp.message.warn("File Import có giá trị áp dụng phí không đúng định dạng!");
                            return false;
                        }

                        if (parseFloat(e.amountIncrease) < 0) {
                            format_error = true;
                            abp.message.warn("File Import có block tăng thêm không đúng định dạng!");
                            return false;
                        }

                        if (parseFloat(e.subFee) < 0) {
                            format_error = true;
                            abp.message.warn("File Import có phụ không đúng định dạng!");
                            return false;
                        }
                    });

                    if (find_duplicate_in_array(data)) {
                        abp.message.warn("File Import có sản phẩm trùng lặp!");
                        return false;
                    }

                    if (format_error) {
                        resetValueFile();
                        _dataTable.clear();
                        return false;
                    }

                    _dataTable.rows.add(data).draw();
                    Sv.SetupAmountMask();

                    if (response.responseCode != "1") {
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
            if (!_$feeInformationForm.valid()) {
                return;
            }
            if ($('#Fee_UserId').prop('required') && $('#Fee_UserId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('User')));
                return;
            }

            let fee = _$feeInformationForm.serializeFormToObject();
            let listFees = [];
            let step = true;
            productFeesList.column(3).nodes().to$().each(function (index) {
                let productId = $(this).find('.fees').attr('data-id');
                let minFee = $("#txtMinFee_" + productId + "").val();
                let amountMinFee = $("#txtAmountMinFee_" + productId + "").val();
                let amountIncrease = $("#txtAmountIncrease_" + productId + "").val();
                let subFee = $("#txtSubFee_" + productId + "").val();

                if (minFee == '' && amountMinFee == '' && amountIncrease == '' && subFee == '') {
                    step = false;
                }

                let item = {
                    productId: productId,
                    minFee: minFee,
                    amountMinFee: amountMinFee,
                    amountIncrease: amountIncrease,
                    subFee: subFee
                };

                listFees.push(item);
            });

            if (!step) {
                abp.message.error('Vui lòng nhập thông tin sản phẩm thu phí!');
                return false;
            }

            fee.feeDetail = listFees;
            fee.productType = JSON.stringify(productType);
            fee.productList = JSON.stringify(productStore);

            console.log(fee)

            _modalManager.setBusy(true);
            _feesService.createOrEdit(
                fee
            ).done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditFeeModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);