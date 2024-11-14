(function () {
    $(function () {

        var _$limitProductsTable = $('#LimitProductsTable');
        var _limitProductsService = abp.services.app.limitProducts;
        let _commonService = abp.services.app.commonLookup;

        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L LTS'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.LimitProducts.Create'),
            edit: abp.auth.hasPermission('Pages.LimitProducts.Edit'),
            delete: abp.auth.hasPermission('Pages.LimitProducts.Delete'),
            approval: abp.auth.hasPermission('Pages.LimitProducts.Approval'),
            cancel: abp.auth.hasPermission('Pages.LimitProducts.Cancel')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/LimitProducts/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/LimitProducts/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditLimitProductModal',
            modalSize: 'modal-xl'
        });

        var _viewLimitProductModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/LimitProducts/ViewLimitProductModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/LimitProducts/_ViewLimitProductModal.js',
            modalClass: 'ViewLimitProductModal',
            modalSize: 'modal-xl'
        });

        var _editLimitProductModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/LimitProducts/ViewLimitProductModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/LimitProducts/_ViewLimitProductModal.js',
            modalClass: 'ViewLimitProductModal',
            modalSize: 'modal-xl'
        });

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z");
        }

        var getToDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT23:59:59Z");
        }

        var dataTable = _$limitProductsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _limitProductsService.getAll,
                inputFilter: function () {
                    return {
                        codeFilter: $('#CodeFilterId').val(),
                        nameFilter: $('#NameFilterId').val(),
                        fromDateFilter: getDateFilter($('#FromCreatedTimeFilter')),
                        toDateFilter: getToDateFilter($('#ToCreatedTimeFilter')),
                        agentTypeFilter: $('#AgentTypeFilterId').val(),
                        agentFilter: $('#AgentFilterId').val(),
                        statusFilter: $('#StatusFilterId').val()
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
                                    _viewLimitProductModal.open({id: data.record.limitProduct.id, isView: true});
                                }
                            },
                            {
                                text: app.localize('Edit'),
                                visible: function (data) {
                                    return _permissions.edit && data.record.limitProduct.status === 0;
                                },
                                action: function (data) {
                                    _editLimitProductModal.open({id: data.record.limitProduct.id, isView: false});
                                }
                            },
                            {
                                text: app.localize('Approval'),
                                visible: function (data) {
                                    return _permissions.approval && data.record.limitProduct.status === 0;
                                },
                                action: function (data) {
                                    approval(data.record.limitProduct);
                                }
                            },
                            {
                                text: app.localize('Cancel'),
                                visible: function (data) {
                                    return _permissions.cancel && data.record.limitProduct.status === 0;
                                },
                                action: function (data) {
                                    cancel(data.record.limitProduct);
                                }
                            },
                            {
                                text: app.localize('Delete'),
                                visible: function () {
                                    return _permissions.delete;
                                },
                                action: function (data) {
                                    deleteLimitProduct(data.record.limitProduct);
                                }
                            }]
                    }
                },
                {
                    targets: 2,
                    data: "limitProduct.code",
                    name: "code"
                },
                {
                    targets: 3,
                    data: "limitProduct.name",
                    name: "name",
                },
                {
                    targets: 4,
                    data: "limitProduct.agentType",
                    name: "agentType",
                    render: function (agentType) {
                        return app.localize('Enum_AgentType_' + agentType);
                    }

                },
                {
                    targets: 5,
                    data: "agentName",
                    name: "agentName",
                },
                {
                    targets: 6,
                    data: "limitProduct.statusName",
                    name: "statusName",
                    // render: function (status) {
                    //     if (status) {
                    //         return app.localize('Enum_LimitProductConfigStatus_' + status);
                    //     }
                    //     return "";
                    // }
                },
                {
                    targets: 7,
                    data: "userCreated",
                    name: "userCreated"
                },
                {
                    targets: 8,
                    data: "creationTime",
                    name: "creationTime",
                    render: function (creationTime) {
                        if (creationTime) {
                            return moment(creationTime).format('L LTS');
                        }
                        return "";
                    }
                },
                {
                    targets: 9,
                    data: "userApproved",
                    name: "userApproved"
                },
                {
                    targets: 10,
                    data: "limitProduct.dateApproved",
                    name: "dateApproved",
                    render: function (data, type, row) {
                        if (row.limitProduct.status === 2) {
                            return moment(row.limitProduct.dateApproved).format('L LTS');
                        }
                        return "";
                    }
                },

                {
                    targets: 11,
                    data: "limitProduct.fromDate",
                    name: "fromDate",
                    render: function (fromDate) {
                        if (fromDate) {
                            return moment(fromDate).format('L LTS');
                        }
                        return "";
                    }
                },
                {
                    targets: 12,
                    data: "limitProduct.toDate",
                    name: "toDate",
                    render: function (toDate) {
                        if (toDate) {
                            return moment(toDate).format('L LTS');
                        }
                        return "";
                    }
                },
            ]
        });

        function getLimitProducts() {
            dataTable.ajax.reload();
        }

        function deleteLimitProduct(limitProduct) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _limitProductsService.delete({
                            id: limitProduct.id
                        }).done(function () {
                            getLimitProducts(true);
                            abp.notify.success(app.localize('SuccessfullyDeleted'));
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

        $('#CreateNewLimitProductButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportToExcelButton').click(function () {
            _limitProductsService
                .getLimitProductsToExcel({
                    codeFilter: $('#CodeFilterId').val(),
                    nameFilter: $('#NameFilterId').val(),
                    fromDateFilter: getDateFilter($('#FromCreatedTimeFilter')),
                    toDateFilter: getToDateFilter($('#ToCreatedTimeFilter')),
                    agentTypeFilter: $('#AgentTypeFilterId').val(),
                    agentFilter: $('#AgentFilterId').val(),
                    statusFilter: $('#StatusFilterId').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#AgentTypeFilterId').change(function () {
            if ($(this).val() != null || $(this).val() !== '') {
                $("#AgentFilterId").removeAttr('disabled');
            } else {
                $("#AgentFilterId").prop('disabled', true);
            }
        });

        $("#AgentFilterId").select2({
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
                        agentType: $('#AgentTypeFilterId').val()
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

        abp.event.on('app.createOrEditLimitProductModalSaved', function () {
            getLimitProducts();
        });

        $('#GetLimitProductsButton').click(function (e) {
            e.preventDefault();
            getLimitProducts();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getLimitProducts();
            }
        });

        $('#xServiceSelect').change(function () {
            let serviceCode = $('#xServiceSelect').val();

            if (serviceCode != null || serviceCode !== '' || serviceCode.length > 0) {
                $('#xProductType').removeAttr('disabled').empty();
                getProductType(serviceCode);
            } else {
                $('#xProductType').prop('disabled', true);
            }
        });

        $('#xProductType').change(function () {
            let productType = $('#xProductType').val();
            if (productType != null || productType !== '' || productType.length > 0) {
                $('#xProductList').removeAttr('disabled').empty();
                getProductList(productType);
            } else {
                $('#xProductList').prop('disabled', true);
            }
        });

        $('#LimitProduct_AgentType').change(function () {
            if ($(this).val() != null || $(this).val() !== '') {
                $("#AgentFilter").removeAttr('disabled');
            } else {
                $("#AgentFilter").prop('disabled', true);
            }
        });

        function approval(object) {
            abp.message.confirm(
                'Bạn có chắc chắn muốn duyệt danh sách hạn mức này không?',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _limitProductsService.approval({id: object.id}).done(function () {
                            getTable();
                            abp.message.success('Duyệt thành công!');
                        });
                    }
                }
            );
        }

        function cancel(object) {
            abp.message.confirm(
                'Bạn chắc chắn muốn hủy danh sách hạn mức này không?',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _limitProductsService.cancel({id: object.id}).done(function () {
                            getTable();
                            abp.message.success('Hủy thành công');
                        });
                    }
                }
            );
        }

        function getTable() {
            dataTable.ajax.reload();
        }

        function getProductType(serviceCode) {
            let options = '';
            _limitProductsService.getCategoryByServiceCode(serviceCode).done(function (obj) {
                $.each(obj, function (key, value) {
                    $('#xProductType')
                        .append($("<option></option>")
                            .attr("value", value.id)
                            .attr("data-category", value.categoryName)
                            .text(value.categoryName));
                });
            }).always(function () {

            });
        }

        function getProductList(categoryId) {
            let options = '';
            _limitProductsService.getProducts(categoryId).done(function (obj) {
                $.each(obj, function (key, value) {
                    $('#xProductList')
                        .append($("<option></option>")
                            .attr("value", value.id)
                            .attr("data-code", value.productCode)
                            .attr("data-category", value.categoryName)
                            .text(value.productName));
                });
            }).always(function () {

            });
        }
    });
})();