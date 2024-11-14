(function () {
    $(function () {

        var _$banksTable = $('#PayBacksTable');
        var _payBacksService = abp.services.app.payBacks;
        var _$Container = $('#PayBacksFormTab');

        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.PayBacks.Create'),
            edit: abp.auth.hasPermission('Pages.PayBacks.Edit'),
            approval: abp.auth.hasPermission('Pages.PayBacks.Approval'),
            cancel: abp.auth.hasPermission('Pages.PayBacks.Cancel'),
            delete: abp.auth.hasPermission('Pages.PayBacks.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/PayBacks/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/PayBacks/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditPayBacksModal',
            modalSize: 'modal-xl'
        });

        var _editPayBacksModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/PayBacks/ViewPayBacksModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/PayBacks/_ViewPayBacksModel.js',
            modalClass: 'ViewPayBacksModal',
            modalSize: 'modal-xl'
        });

        var _viewPayBacksModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/PayBacks/ViewPayBacksModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/PayBacks/_ViewPayBacksModel.js',
            modalClass: 'ViewPayBacksModal',
            modalSize: 'modal-xl'
        });

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z");
        }

        var dataTable = _$banksTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _payBacksService.getAll,
                inputFilter: function () {
                    return {
                        nameFilter: $('#PayBacksNameFilter').val(),
                        codeFilter: $('#PayBacksCodeFilter').val(),
                        productTypeFilter: $('#PayBacksProductTypeFilter').val(),
                        providerFilter: $('#PayBacksProviderFilter').val(),
                        statusFilter: $('#StatusFilter').val()
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
                                    _viewPayBacksModal.open({id: data.record.payBack.id, isView: true});
                                }
                            },
                            // {
                            //     text: app.localize('Edit'),
                            //     visible: function (data) {
                            //         return _permissions.edit && data.record.payBack.status === 0;
                            //     },
                            //     action: function (data) {
                            //         _viewPayBacksModal.open({ id: data.record.payBack.id, isView : false });
                            //     }
                            // },
                            // {
                            //     text: app.localize('Approval'),
                            //     visible: function (data) {
                            //         return _permissions.approval && data.record.payBack.status === 0;
                            //     },
                            //     action: function (data) {
                            //         //approval(data.record.payBack);
                            //         _viewPayBacksModal.open({ id: data.record.payBack.id, isView : true });
                            //     }
                            // },
                            {
                                text: app.localize('Cancel'),
                                visible: function (data) {
                                    return _permissions.cancel && data.record.payBack.status === 0;
                                },
                                action: function (data) {
                                    cancel(data.record.payBack);
                                }
                            },
                            {
                                text: app.localize('Delete'),
                                visible: function (data) {
                                    return _permissions.delete && data.record.payBack.status === 0;
                                },
                                action: function (data) {
                                    deletePayBacks(data.record.payBack);
                                }
                            }]
                    }
                },
                {
                    targets: 2,
                    data: "payBack.code",
                    name: "code"
                },
                {
                    targets: 3,
                    data: "payBack.name",
                    name: "branchName",
                    render: function (branchName) {
                        return '<div style="width: 400px; word-break: break-all; white-space: pre-line;">' + branchName + '</div>';
                    }
                },
                {
                    targets: 4,
                    data: "payBack.fromDate",
                    name: "fromDate",
                    render: function (data, type, row) {
                        return moment(new Date(row.payBack.fromDate)).format('DD/MM/YYYY') + " - " + moment(new Date(row.payBack.toDate)).format('DD/MM/YYYY');
                    }
                },
                {
                    targets: 5,
                    data: "payBack.total",
                    name: "total",
                },
                {
                    targets: 6,
                    data: "payBack.totalAmount",
                    name: "totalAmount",
                    render: function (totalAmount) {
                        return Sv.NumberToString(totalAmount);
                    }
                },
                {
                    targets: 7,
                    data: "payBack.status",
                    name: "status",
                    render: function (status) {
                        return app.localize('Enum_PayBacksStatus_' + status);
                    }
                },
                {
                    targets: 8,
                    data: "payBack.creationTime",
                    name: "creationTime",
                    render: function (creationTime) {
                        if (creationTime)
                            return moment(new Date(creationTime)).format('DD/MM/YYYY HH:hh:ss');
                        else
                            return "";
                    }
                },
                {
                    targets: 9,
                    data: "userName",
                    name: "userName"
                },
                {
                    targets: 10,
                    data: "payBack.dateApproved",
                    name: "dateApproved",
                    render: function (dateApproved) {
                        if (dateApproved)
                            return moment(new Date(dateApproved)).format('DD/MM/YYYY HH:hh:ss');
                        else
                            return "";
                    }
                },
                {
                    targets: 11,
                    data: "userApproved",
                    name: "userApproved"
                }
            ]
        });

        function getPayBacks() {
            dataTable.ajax.reload();
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

        $('#CreateNewPayBacksButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportToExcelButton').click(function () {
            _payBacksService
                .getPayBacksToExcel({
                    nameFilter: $('#PayBacksNameFilter').val(),
                    codeFilter: $('#PayBacksCodeFilter').val(),
                    statusFilter: $('#StatusFilter').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditPayBacksModalSaved', function () {
            getPayBacks();
        });

        $('#GetPayBacksButton').click(function (e) {
            e.preventDefault();
            getPayBacks();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getPayBacks();
            }
        });

        function deletePayBacks(payBacks) {
            abp.message.confirm(
                'Bạn chắc chắn muốn xóa danh sách trả phí khuyến mại này không?',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _payBacksService.delete({
                            id: payBacks.id
                        }).done(function () {
                            getPayBacks();
                            abp.message.success(app.localize('SuccessfullyDeleted'));
                        });
                    }
                }
            );
        }

        function approval(payBacks) {
            abp.message.confirm(
                'Bạn có chắc chắn muốn duyệt danh sách trả phí khuyến mại này không?',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        abp.ui.setBusy(_$Container);
                        abp.message.info('Hệ thống đang thực hiện trả thưởng. Vui lòng chờ thông báo kết quả');
                        _payBacksService.approval(payBacks.id).done(function () {
                            getPayBacks();
                            abp.message.success('Trả thưởng thành công');
                            abp.ui.clearBusy(_$Container);
                        });
                    }
                }
            );
        }

        function cancel(payBacks) {
            abp.message.confirm(
                'Bạn chắc chắn muốn hủy danh sách trả phí khuyến mại này không?',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _payBacksService.cancel({id: payBacks.id}).done(function () {
                            getPayBacks();
                            abp.message.success('Hủy thành công');
                        }).always(function () {
                        });
                    }
                }
            );
        }
    });
})();