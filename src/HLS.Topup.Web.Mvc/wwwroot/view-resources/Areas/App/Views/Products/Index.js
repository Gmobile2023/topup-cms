(function () {
    $(function () {

        var _$productsTable = $('#ProductsTable');
        var _productsService = abp.services.app.products;
        var _categoriesService = abp.services.app.categories;

        getAllCategory();

        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Products.Create'),
            edit: abp.auth.hasPermission('Pages.Products.Edit'),
            'delete': abp.auth.hasPermission('Pages.Products.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Products/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Products/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditProductModal',
            modalSize: 'modal-xl'
        });

        var _viewProductModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Products/ViewproductModal',
            modalClass: 'ViewProductModal',
            modalSize: 'modal-xl'
        });

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z");
        }

        var dataTable = _$productsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _productsService.getAll,
                inputFilter: function () {
                    return {
                        filter: $('#ProductsTableFilter').val(),
                        productCodeFilter: $('#ProductCodeFilterId').val(),
                        productNameFilter: $('#ProductNameFilterId').val(),
                        productTypeFilter: $('#ProductTypeFilterId').val(),
                        statusFilter: $('#StatusFilterId').val(),
                        unitFilter: $('#UnitFilterId').val(),
                        categoryCategoryNameFilter: $('#CategoryCategoryNameFilterId').val(),
                        customerSupportNoteFilter: $('#CustomerNoteFilterId').val(),
                        userManualNoteFilter: $('#UserManualNoteFilterId').val()
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
                                    _viewProductModal.open({id: data.record.product.id});
                                }
                            },
                            {
                                text: app.localize('Edit'),
                                visible: function () {
                                    return _permissions.edit;
                                },
                                action: function (data) {
                                    _createOrEditModal.open({id: data.record.product.id});
                                }
                            },
                            {
                                text: app.localize('Delete'),
                                visible: function () {
                                    return _permissions.delete;
                                },
                                action: function (data) {
                                    deleteProduct(data.record.product);
                                }
                            }]
                    }
                },
                {
                    targets: 2,
                    data: "product.productCode",
                    name: "productCode"
                },
                {
                    targets: 3,
                    data: "product.productName",
                    name: "productName"
                },
                {
                    targets: 4,
                    data: "product.order",
                    name: "order"
                },
                {
                    targets: 5,
                    data: "product.productValue",
                    name: "productValue",
                    render: function (data) {
                        return Sv.format_number(data)
                    }
                },
                // {
                // 	targets: 6,
                // 	 data: "product.productType",
                // 	 name: "productType"   ,
                // 	render: function (productType) {
                // 		return app.localize('Enum_ProductType_' + productType);
                // 	}
                //
                // },
                {
                    targets: 6,
                    data: "product.status",
                    name: "status",
                    render: function (status) {
                        return app.localize('Enum_ProductStatus_' + status);
                    }

                },
                {
                    targets: 7,
                    data: "product.unit",
                    name: "unit"
                },
                {
                    targets: 8,
                    data: "categoryCategoryName",
                    name: "categoryFk.categoryName"
                },
                {
                    targets: 9,
                    data: "product.customerSupportNote",
                    name: "customerSupportNote"
                },
                {
                    targets: 10,
                    data: "product.userManualNote",
                    name: "userManualNote"
                }
            ]
        });

        function getProducts() {
            dataTable.ajax.reload();
        }

        function deleteProduct(product) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _productsService.delete({
                            id: product.id
                        }).done(function () {
                            getProducts(true);
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

        $('#CreateNewProductButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportToExcelButton').click(function () {
            _productsService
                .getProductsToExcel({
                    filter: $('#ProductsTableFilter').val(),
                    productCodeFilter: $('#ProductCodeFilterId').val(),
                    productNameFilter: $('#ProductNameFilterId').val(),
                    productTypeFilter: $('#ProductTypeFilterId').val(),
                    statusFilter: $('#StatusFilterId').val(),
                    unitFilter: $('#UnitFilterId').val(),
                    categoryCategoryNameFilter: $('#CategoryCategoryNameFilterId').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditProductModalSaved', function () {
            getProducts();
        });

        $('#GetProductsButton').click(function (e) {
            e.preventDefault();
            getProducts();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getProducts();
            }
        });

        function getAllCategory() {
            _categoriesService.getAll({'filter': '', 'skipCount': 0, 'maxResultCount': '9999'}).done(function (obj) {
                $.each(obj.items, function (key, value) {
                    $('#CategoryCategoryNameFilterId')
                        .append($("<option></option>")
                            .attr("value", value.category.categoryName)
                            .text(value.category.categoryName));
                });
            }).always(function () {

            });
        }
    });
})();
