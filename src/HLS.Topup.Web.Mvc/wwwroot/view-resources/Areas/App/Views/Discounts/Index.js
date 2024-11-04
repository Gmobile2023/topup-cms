(function () {
    $(function () {

        var _$discountsTable = $('#DiscountsTable');
        var _discountsService = abp.services.app.discounts;

        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L LT'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Discounts.Create'),
            edit: abp.auth.hasPermission('Pages.Discounts.Edit'),
            approval: abp.auth.hasPermission('Pages.Discounts.Approval'),
            cancel: abp.auth.hasPermission('Pages.Discounts.Cancel'),
            stop: abp.auth.hasPermission('Pages.Discounts.Stop'),
            'delete': abp.auth.hasPermission('Pages.Discounts.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Discounts/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Discounts/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditDiscountModal',
            modalSize: 'modal-xl'
        });

        var _editModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Discounts/ViewDiscountModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Discounts/_ViewDiscountModal.js',
            modalClass: 'ViewOrEditDiscountModal',
            modalSize: 'modal-xl'
        });

        var _viewDiscountModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Discounts/ViewDiscountModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Discounts/_ViewDiscountModal.js',
            modalClass: 'ViewOrEditDiscountModal',
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

        bindCategories('ProductTypeFilter');

        let dataTable = _$discountsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _discountsService.getAll,
                inputFilter: function () {
                    return {
                        filter: $('#DiscountsTableFilter').val(),
                        codeFilter: $('#CodeFilterId').val(),
                        nameFilter: $('#NameFilterId').val(),
                        statusFilter: $('#StatusFilterId').val(),
                        agentTypeFilter: $('#AgentTypeFilterId').val(),
                        userNameFilter: $('#UserIdSearch').val(),
                        fromCreationTimeFilter: getDateFilter($('#FromCreatedTimeFilter')),
                        toCreationTimeFilter: getToDateFilter($('#ToCreatedTimeFilter')),
                        fromAppliedTimeFilter: getDateFilter($('#FromAppliedTimeFilter')),
                        toAppliedTimeFilter: getToDateFilter($('#ToAppliedTimeFilter')),
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
                                    _viewDiscountModal.open({id: data.record.discount.id, isView: true});
                                }
                            },
                            {
                                text: app.localize('Edit'),
                                visible: function (data) {
                                    return _permissions.edit && data.record.discount.status === 0;
                                },
                                action: function (data) {
                                    _editModal.open({id: data.record.discount.id, isView: false});
                                }
                            },
                            {
                                text: app.localize('Approval'),
                                visible: function (data) {
                                    return _permissions.approval && data.record.discount.status === 0;
                                },
                                action: function (data) {
                                    approval(data.record.discount);
                                }
                            },
                            {
                                text: app.localize('Cancel'),
                                visible: function (data) {
                                    return _permissions.cancel && data.record.discount.status === 0;
                                },
                                action: function (data) {
                                    cancel(data.record.discount);
                                }
                            },
                            {
                                text: app.localize('StopDiscount'),
                                visible: function (data) {
                                    return _permissions.stop && data.record.discount.status === 2;
                                },
                                action: function (data) {
                                    stop(data.record.discount);
                                }
                            },
                            {
                                text: app.localize('Delete'),
                                visible: function (data) {
                                    return _permissions.delete && data.record.discount.status === 0;
                                },
                                action: function (data) {
                                    deleteDiscount(data.record.discount);
                                }
                            }]
                    }
                },
                {
                    targets: 2,
                    data: "createdDate",
                    name: "createdDate",
                    render: function (creationTime) {
                        if (creationTime) {
                            return moment(new Date(creationTime)).format('DD/MM/YYYY HH:mm:ss');
                        }
                        return "";
                    }
                },
                {
                    targets: 3,
                    data: "discount.code",
                    name: "code"
                },
                {
                    targets: 4,
                    data: "discount.name",
                    name: "discount.name"
                },
                {
                    width: 150,
                    targets: 5,
                    data: "discount.agentType",
                    name: "agentType",
                    render: function (status) {
                        return app.localize('Enum_AgentType_' + status);
                    }
                },
                {
                    targets: 6,
                    data: "userName",
                    name: "userName",
                },
                {
                    targets: 7,
                    data: "discount.statusName",
                    name: "discount.statusName",
                },
                {
                    targets: 8,
                    data: "discount.fromDate",
                    name: "discount.fromDate",
                    render: function (fromDate) {
                        if (fromDate) {
                            return moment(new Date(fromDate)).format('DD/MM/YYYY HH:mm');
                        }
                        return "";
                    }
                },
                {
                    targets: 9,
                    data: "discount.toDate",
                    name: "discount.toDate",
                    render: function (toDate) {
                        if (toDate) {
                            return moment(new Date(toDate)).format('DD/MM/YYYY HH:mm');
                        }
                        return "";
                    }
                },
                {
                    targets: 10,
                    data: "discount.dateApproved",
                    name: "discount.dateApproved",
                    render: function (dateApproved) {
                        if (dateApproved) {
                            return moment(new Date(dateApproved)).format('DD/MM/YYYY HH:mm:ss');
                        }
                        return "";
                    }
                },
                {
                    targets: 11,
                    data: "createtor",
                    name: "createtor"
                },
                {
                    targets: 12,
                    data: "approver",
                    name: "approver"
                },
                {
                    targets: "_all",
                    orderable: false
                }
            ],
        });

        $("#UserIdSearch").select2({
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
                        agentType: null,
                        accountType: 99
                    };
                },
                processResults: function (data, params) {
                    params.page = params.page || 1;

                    return {
                        results: $.map(data.result, function (item) {
                            return {
                                text: item.accountCode + " - " + item.phoneNumber + " - " + item.fullName,
                                id: item.accountCode
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

        function getDiscounts() {
            dataTable.ajax.reload();
        }

        function bindCategories(position) {
            let options = '';
            abp.ajax({
                url: abp.appPath + "api/services/app/Discounts/GetCategories",
                method: 'GET',
                data: null,
                success: function (data) {
                    $.each(data, function (index, item) {
                        options += '<option value="'+ item.id +'">'+ item.displayName +'</option>';
                    });

                    $('#' + position).append(options).select2();
                }
            }).done(function() {

            });
        }

        function deleteDiscount(discount) {
            abp.message.confirm(
                'Bạn chắc chắn muốn xóa chính sách này không?',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _discountsService.delete({
                            id: discount.id
                        }).done(function () {
                            getDiscounts(true);
                            abp.message.success(app.localize('SuccessfullyDeleted'));
                        });
                    }
                }
            );
        }

        function approval(discount) {
            abp.message.confirm(
                'Bạn có chắc chắn muốn duyệt chính sách này không?',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _discountsService.approval({id: discount.id}).done(function () {
                            getDiscounts(true);
                            abp.message.success('Duyệt thành công');
                        });
                    }
                }
            );
        }

        function cancel(discount) {
            abp.message.confirm(
                'Bạn chắc chắn muốn hủy chính sách này không?',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _discountsService.cancel({id: discount.id}).done(function () {
                            getDiscounts(true);
                            abp.message.success('Hủy thành công');
                        });
                    }
                }
            );
        }
        function stop(discount) {
            abp.message.confirm(
                'Bạn chắc chắn muốn dừng chính sách này không?',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _discountsService.stop({id: discount.id}).done(function () {
                            getDiscounts(true);
                            abp.message.success('Dừng chính sách phí thành công');
                        });
                    }
                }
            );
        }

        $('#ProductTypeFilter').change(function () {
            let productTypeId = $('#ProductTypeFilter').val();
            if (productTypeId != '') {
                _discountsService.getProducts(parseInt(productTypeId)).done(function (obj) {
                    $('#productFilter').empty();
                    $('#productFilter').append($("<option></option>").attr("value", "").text("Tất cả"));
                    $.each(obj, function (key, value) {
                        $('#productFilter')
                            .append($("<option></option>")
                                .attr("value", value.id)
                                .attr("data-category", value.categoryName)
                                .attr("data-name", value.productName)
                                .text(value.productName));
                    });
                }).always(function () {

                });
            } else {
                $('#productFilter').prop('disabled', true);
            }
        });

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

        $('#CreateNewDiscountButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportToExcelButton').click(function () {
            _discountsService
                .getDiscountsToExcel({
                    filter: $('#DiscountsTableFilter').val(),
                    codeFilter: $('#CodeFilterId').val(),
                    nameFilter: $('#NameFilterId').val(),
                    statusFilter: $('#StatusFilterId').val(),
                    agentTypeFilter: $('#AgentTypeFilterId').val(),
                    userNameFilter: $('#UserIdSearch').val(),
                    fromCreationTimeFilter: getDateFilter($('#FromCreatedTimeFilter')),
                    toCreationTimeFilter: getToDateFilter($('#ToCreatedTimeFilter')),
                    fromAppliedTimeFilter: getDateFilter($('#FromAppliedTimeFilter')),
                    toAppliedTimeFilter: getToDateFilter($('#ToAppliedTimeFilter')),
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditDiscountModalSaved', function () {
            getDiscounts();
        });

        $('#GetDiscountsButton').click(function (e) {
            e.preventDefault();
            getDiscounts();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getDiscounts();
            }
        });

        $('.btn-head-search').click(function (e) {
            e.preventDefault();
            getDiscounts();
        });
    });
})();
