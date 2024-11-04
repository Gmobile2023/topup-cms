(function () {
    $(function () {

        var _$discountsTable = $('#DiscountsTable');
        var _discountsService = abp.services.app.discounts;

        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Discounts.Create'),
            edit: abp.auth.hasPermission('Pages.Discounts.Edit'),
            approval: abp.auth.hasPermission('Pages.Discounts.Approval'),
            cancel: abp.auth.hasPermission('Pages.Discounts.Cancel'),
            'delete': abp.auth.hasPermission('Pages.Discounts.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'DiscountManagement/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Views/DiscountManagement/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditDiscountModal'
        });

        var _viewDiscountModal = new app.ModalManager({
            viewUrl: abp.appPath + 'DiscountManagement/ViewdiscountModal',
            modalClass: 'ViewDiscountModal'
        });




        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z");
        }

        var dataTable = _$discountsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _discountsService.getAll,
                inputFilter: function () {
                    return {
                        filter: $('#DiscountsTableFilter').val(),
                        nameFilter: $('#NameFilterId').val(),
                        codeFilter: $('#CodeFilterId').val(),
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
                        text:  app.localize('Actions') + ' <span class="caret"></span>',
                        items: [
                            {
                                text: app.localize('View'),
                                action: function (data) {
                                    _createOrEditModal.open({ id: data.record.discount.id, isView: true });
                                }
                            },
                            {
                                text: app.localize('Edit'),
                                visible: function (data) {
                                    console.log(data.record.status);
                                    return _permissions.edit && (data.record.discount.status == 0);
                                },
                                action: function (data) {
                                    _createOrEditModal.open({ id: data.record.discount.id });
                                }
                            },
                            {
                                text: app.localize('Approval'),
                                visible: function (data) {
                                    return _permissions.approval && data.record.discount.status == 0;
                                },
                                action: function (data) {
                                    approvalDiscount(data.record.discount);
                                }
                            },
                            {
                                text: app.localize('Cancel'),
                                visible: function (data) {
                                    return _permissions.cancel && data.record.discount.status == 0;
                                },
                                action: function (data) {
                                    cancelDiscount(data.record.discount);
                                }
                            },
                            {
                                text: app.localize('Delete'),
                                visible: function (data) {
                                    return _permissions.delete && data.record.discount.status == 0;
                                },
                                action: function (data) {
                                    deleteDiscount(data.record.discount);
                                }
                            }]
                    }
                },
                {
                    targets: 2,
                    data: "discount.name",
                    name: "name"
                },
                {
                    targets: 4,
                    data: "discount.code",
                    name: "code"
                },
                {
                    targets: 5,
                    data: "discount.creationTime",
                    name: "creationTime",
                    render: function (creationTime) {
                        if (creationTime) {
                            return moment(creationTime).format('L');
                        }
                        return "";
                    }

                },
                {
                    targets: 6,
                    data: "discount.dateApproved",
                    name: "toDate",
                    render: function (dateApproved) {
                        if (dateApproved) {
                            return moment(dateApproved).format('L');
                        }
                        return "";
                    }

                },
                {
                    targets: 3,
                    data: "discount.status",
                    name: "status",
                    render: function (status) {
                        return app.localize('Enum_DiscountStatus_' + status);
                    }

                }
            ]
        });


        function getDiscounts() {
            dataTable.ajax.reload();
        }

        function deleteDiscount(discount) {
            abp.message.confirm(
                '',
                function (isConfirmed) {
                    if (isConfirmed) {
                        _discountsService.delete({
                            id: discount.id
                        }).done(function (rs) {
                            if (rs.responseCode === "01") {
                                getDiscounts(true);
                                abp.notify.success(app.localize('SuccessfullyDeleted'));
                            }
                            else {
                                abp.notify.error(rs.responseMessage);
                            }
                        });
                    }
                }
            );
        }
        function approvalDiscount(discount) {
            abp.message.confirm(
                '',
                function (isConfirmed) {
                    if (isConfirmed) {
                        _discountsService.approval({
                            id: discount.id
                        }).done(function (rs) {
                            if (rs.responseCode === "01") {
                                getDiscounts(true);
                                abp.notify.success(rs.responseMessage);
                            }
                            else {
                                abp.notify.error(rs.responseMessage);
                            }
                        });
                    }
                }
            );
        }
        function cancelDiscount(discount) {
            abp.message.confirm(
                '',
                function (isConfirmed) {
                    if (isConfirmed) {
                        _discountsService.cancel({
                            id: discount.id
                        }).done(function (rs) {
                            if (rs.responseCode === "01") {
                                getDiscounts(true);
                                abp.notify.success(rs.responseMessage);
                            }
                            else {
                                abp.notify.error(rs.responseMessage);
                            }
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

        $('#CreateNewDiscountButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportToExcelButton').click(function () {
            _discountsService
                .getDiscountsToExcel({
                    filter: $('#DiscountsTableFilter').val(),
                    nameFilter: $('#NameFilterId').val(),
                    codeFilter: $('#CodeFilterId').val(),
                    statusFilter: $('#StatusFilterId').val()
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

    });
})();