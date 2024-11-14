(function () {
    $(function () {
        var _$categoriesTable = $('#CategoriesTable');
        var _categoriesService = abp.services.app.categories;
        var _servicesService = abp.services.app.services;

        getAllService();
        getAllCategory();

        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Categories.Create'),
            edit: abp.auth.hasPermission('Pages.Categories.Edit'),
            'delete': abp.auth.hasPermission('Pages.Categories.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Categories/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Categories/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditCategoryModal',
            modalSize: 'modal-xl'
        });

        var _viewCategoryModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Categories/ViewcategoryModal',
            modalClass: 'ViewCategoryModal',
            modalSize: 'modal-lg'
        });

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z");
        }

        var dataTable = _$categoriesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _categoriesService.getAll,
                inputFilter: function () {
                    return {
                        filter: $('#CategoriesTableFilter').val(),
                        categoryCodeFilter: $('#CategoryCodeFilterId').val(),
                        categoryNameFilter: $('#CategoryNameFilterId').val(),
                        statusFilter: $('#StatusFilterId').val(),
                        typeFilter: $('#TypeFilterId').val(),
                        categoryCategoryNameFilter: $('#CategoryCategoryNameFilterId').val(),
                        serviceServicesNameFilter: $('#ServiceServicesNameFilterId').val()
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
                                    _viewCategoryModal.open({id: data.record.category.id});
                                }
                            },
                            {
                                text: app.localize('Edit'),
                                visible: function () {
                                    return _permissions.edit;
                                },
                                action: function (data) {
                                    _createOrEditModal.open({id: data.record.category.id});
                                }
                            },
                            {
                                text: app.localize('Delete'),
                                visible: function () {
                                    return _permissions.delete;
                                },
                                action: function (data) {
                                    deleteCategory(data.record.category);
                                }
                            }]
                    }
                },
                {
                    targets: 2,
                    data: "category.categoryCode",
                    name: "categoryCode"
                },
                {
                    targets: 3,
                    data: "category.categoryName",
                    name: "categoryName"
                },
                {
                    targets: 4,
                    data: "category.order",
                    name: "order"
                },
                {
                    targets: 5,
                    data: "category.status",
                    name: "status",
                    render: function (status) {
                        return app.localize('Enum_CategoryStatus_' + status);
                    }

                },
                // {
                //     targets: 6,
                //     data: "category.type",
                //     name: "type",
                //     render: function (type) {
                //         return app.localize('Enum_CategoryType_' + type);
                //     }
                //
                // },
                {
                    targets: 6,
                    data: "categoryCategoryName",
                    name: "parentCategoryFk.categoryName"
                },
                {
                    targets: 7,
                    data: "serviceServicesName",
                    name: "serviceFk.servicesName"
                },
            ]
        });

        function getCategories() {
            dataTable.ajax.reload();
        }

        function deleteCategory(category) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _categoriesService.delete({
                            id: category.id
                        }).done(function () {
                            getCategories(true);
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

        $('#CreateNewCategoryButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportToExcelButton').click(function () {
            _categoriesService
                .getCategoriesToExcel({
                    filter: $('#CategoriesTableFilter').val(),
                    categoryCodeFilter: $('#CategoryCodeFilterId').val(),
                    categoryNameFilter: $('#CategoryNameFilterId').val(),
                    statusFilter: $('#StatusFilterId').val(),
                    typeFilter: $('#TypeFilterId').val(),
                    categoryCategoryNameFilter: $('#CategoryCategoryNameFilterId').val(),
                    serviceServicesNameFilter: $('#ServiceServicesNameFilterId').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditCategoryModalSaved', function () {
            getCategories();
        });

        $('#GetCategoriesButton').click(function (e) {
            e.preventDefault();
            getCategories();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getCategories();
            }
        });

        function getAllService() {
            _servicesService.getAll({'serviceServicesNameFilter': ''}).done(function (obj) {
                $.each(obj.items, function (key, value) {
                    $('#ServiceServicesNameFilterId')
                        .append($("<option></option>")
                            .attr("value", value.service.servicesName)
                            .text(value.service.servicesName));
                });
            }).always(function () {

            });
        }

        function getAllCategory() {
            _categoriesService.getAll({'filter': ''}).done(function (obj) {
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
