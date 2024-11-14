(function () {
    $(function () {

        var _$payBatchBillsTable = $('#PayBatchBillsTable');
        var _payBatchBillsService = abp.services.app.payBatchBills;

        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.PayBatchBills.Create'),
            cancel: abp.auth.hasPermission('Pages.PayBatchBills.Cancel'),
            approval: abp.auth.hasPermission('Pages.PayBatchBills.Approval'),
            delete: abp.auth.hasPermission('Pages.PayBatchBills.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/PayBatchBills/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/PayBatchBills/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditPayBatchBillModal',
            modalSize: 'modal-xl'
        });

        var _viewPayBatchBillModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/PayBatchBills/ViewpayBatchBillModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/PayBatchBills/_ViewDetailModal.js',
            modalClass: 'ViewPayBatchBillModal',
            modalSize: 'modal-xl'
        });


        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z");
        }

        $("#selectProduct").select2();
        $("#selectCategory").select2();

        var dataTable = _$payBatchBillsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _payBatchBillsService.getAll,
                inputFilter: function () {
                    return {
                        filter: $('#PayBatchBillsTableFilter').val(),
                        codeFilter: $('#CodeFilterId').val(),
                        nameFilter: $('#NameFilterId').val(),
                        statusFilter: $('#StatusFilterId').val(),
                        minDateApprovedFilter: getDateFilter($('#MinDateApprovedFilterId')),
                        maxDateApprovedFilter: getDateFilter($('#MaxDateApprovedFilterId')),
                        categoryCodeFilter: $('#selectCategory').val(),
                        productCodeFilter: $('#selectProduct').val()
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
                                    _viewPayBatchBillModal.open({id: data.record.payBatchBill.id});
                                }
                            },
                            //{
                            //    text: app.localize('Approval'),
                            //    visible: function (data) {
                            //        return _permissions.approval && data.record.payBatchBill.status === 0;
                            //    },
                            //    action: function (data) {
                            //        approvalBatchBill(data.record.payBatchBill);
                            //    }
                            //},
                            {
                                text: app.localize('Cancel'),
                                visible: function (data) {
                                    return _permissions.cancel && data.record.payBatchBill.status === 0;
                                },
                                action: function (data) {
                                    cancelBatchBill(data.record.payBatchBill);
                                }
                            },
                            {
                                text: app.localize('Delete'),
                                visible: function (data) {
                                    return _permissions.delete && data.record.payBatchBill.status === 0;
                                },
                                action: function (data) {
                                    deletePayBatchBill(data.record.payBatchBill);
                                }
                            }]
                    }
                },
                {
                    targets: 2,
                    data: "payBatchBill.code",
                    name: "code"
                },
                {
                    targets: 3,
                    data: "payBatchBill.name",
                    name: "name"
                },
                {
                    targets: 4,
                    data: "payBatchBill.status",
                    name: "status",
                    render: function (status) {
                        return app.localize('Enum_PayBatchBillStatus_' + status);
                    }

                },
                {
                    targets: 5,
                    data: "payBatchBill.totalAgent",
                    name: "totalAgent",
                    render: function (totalAgent) {
                        if (totalAgent) {
                            return Sv.format_number(totalAgent);
                        }
                        return "0";
                    }
                },
                {
                    targets: 6,
                    data: "payBatchBill.totalTrans",
                    name: "totalTrans",
                    render: function (totalTrans) {
                        if (totalTrans) {
                            return Sv.format_number(totalTrans);
                        }
                        return "0";
                    }
                },
                {
                    targets: 7,
                    data: "payBatchBill.totalAmount",
                    name: "totalAmount",
                    render: function (totalAmount) {
                        if (totalAmount) {
                            return Sv.format_number(totalAmount);
                        }
                        return "0";
                    }
                },
                {
                    targets: 8,
                    data: "payBatchBill.period",
                    name: "period",
                },
                {
                    targets: 9,
                    data: "payBatchBill.totalBlockBill",
                    name: "totalBlockBill",
                    render: function (totalBlockBill) {
                        if (totalBlockBill) {
                            return Sv.format_number(totalBlockBill);
                        }
                        return "0";
                    }
                },
                {
                    targets: 10,
                    data: "payBatchBill.amountPayBlock",
                    name: "amountPayBlock",
                    render: function (amountPayBlock) {
                        if (amountPayBlock) {
                            return Sv.format_number(amountPayBlock);
                        }
                        return "0";
                    }
                },
                {
                    targets: 11,
                    data: "categoryName",
                    name: "categoryName"
                },
                {
                    targets: 12,
                    data: "productName",
                    name: "productName"
                },
                {
                    targets: 13,
                    data: "payBatchBill.creationTime",
                    name: "creationTime",
                    render: function (creationTime) {
                        if (creationTime) {
                            return moment(creationTime).format('L');
                        }
                        return "";
                    }

                },
                {
                    targets: 14,
                    data: "userCreated",
                    name: "userCreated"
                },
                {
                    targets: 15,
                    data: "payBatchBill.dateApproved",
                    name: "dateApproved",
                    render: function (dateApproved) {
                        if (dateApproved) {
                            return moment(dateApproved).format('L');
                        }
                        return "";
                    }

                },
                {
                    targets: 16,
                    data: "userApproval",
                    name: "userApproval"
                }
            ]
        });


        function getPayBatchBills() {
            dataTable.ajax.reload();
        }

        function deletePayBatchBill(payBatchBill) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _payBatchBillsService.delete({
                            id: payBatchBill.id
                        }).done(function () {
                            getPayBatchBills(true);
                            abp.notify.success(app.localize('SuccessfullyDeleted'));
                        });
                    }
                }
            );
        }

        function cancelBatchBill(payBatchBill) {
            abp.message.confirm(
                'Bạn chắc chắn muốn hủy danh sách trả phí hóa đơn này không?',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _payBatchBillsService.confirmCancel(payBatchBill.id).done(function () {
                            getPayBatchBills(true);
                            abp.message.success('Hủy thành công');
                        });
                    }
                }
            );
        }

        function approvalBatchBill(payBatchBill) {
            abp.message.confirm(
                'Bạn có chắc chắn muốn duyệt danh sách trả phí thanh toán hóa đơn này không?',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _payBatchBillsService.confirmApproval(payBatchBill.id).done(function () {
                            getPayBatchBills();
                            abp.message.success('Thành công');
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

        $('#CreateNewPayBatchBillButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportToExcelButton').click(function () {
            _payBatchBillsService
                .getPayBatchBillsToExcel({
                    filter: $('#PayBatchBillsTableFilter').val(),
                    codeFilter: $('#CodeFilterId').val(),
                    nameFilter: $('#NameFilterId').val(),
                    statusFilter: $('#StatusFilterId').val(),
                    minDateApprovedFilter: getDateFilter($('#MinDateApprovedFilterId')),
                    maxDateApprovedFilter: getDateFilter($('#MaxDateApprovedFilterId')),
                    categoryCodeFilter: $('#selectCategory').val(),
                    productCodeFilter: $('#selectProduct').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditPayBatchBillModalSaved', function () {
            getPayBatchBills();
        });

        $('#GetPayBatchBillsButton').click(function (e) {
            e.preventDefault();
            getPayBatchBills();
        });

        $("#selectCategory").change(function (e) {
            const cateCode = $(e.target).val();
            let _s = $("#AdvacedAuditFiltersArea");
            Sv.GetProductByCate(cateCode, _s.find("#selectProduct"), false);
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getPayBatchBills();
            }
        });
    });
})();
