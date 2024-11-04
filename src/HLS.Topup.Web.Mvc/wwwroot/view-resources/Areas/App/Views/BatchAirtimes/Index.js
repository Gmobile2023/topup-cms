(function () {
    $(function () {

            var _$batchAirtimesTable = $('#BatchAirtimesTable');
            var _batchAirtimesService = abp.services.app.batchAirtimes;

            $('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            var _permissions = {
                create: abp.auth.hasPermission('Pages.BatchAirtimes.Create'),
                edit: abp.auth.hasPermission('Pages.BatchAirtimes.Edit'),
                approval: abp.auth.hasPermission('Pages.BatchAirtimes.Approval'),
                'delete': abp.auth.hasPermission('Pages.BatchAirtimes.Delete')
            };

            var _createOrEditModal = new app.ModalManager({
                viewUrl: abp.appPath + 'App/BatchAirtimes/CreateOrEditModal',
                scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/BatchAirtimes/_CreateOrEditModal.js',
                modalClass: 'CreateOrEditBatchAirtimeModal'
            });

            var _viewBatchAirtimeModal = new app.ModalManager({
                viewUrl: abp.appPath + 'App/BatchAirtimes/ViewbatchAirtimeModal',
                modalClass: 'ViewBatchAirtimeModal'
            });


            var getDateFilter = function (element, isEnd) {
                if (element.data("DateTimePicker").date() == null) {
                    return null;
                }
                if (isEnd) {
                    return element.data("DateTimePicker").date().format("YYYY-MM-DDT23:59:59Z");
                }
                return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z");
            }

            var dataTable = _$batchAirtimesTable.DataTable({
                paging: true,
                serverSide: true,
                processing: true,
                listAction: {
                    ajaxFunction: _batchAirtimesService.getAll,
                    inputFilter: function () {
                        return {
                            filter: $('#BatchAirtimesTableFilter').val(),
                            batchCodeFilter: $('#BatchCodeFilterId').val(),
                            providerCodeFilter: $('#providerCode').val(),
                            minAirtimeFilter: getDateFilter($('#MinCreatedDateFilterId'), false),
                            maxAirtimeFilter: getDateFilter($('#MaxCreatedDateFilterId'), false),
                            statusFilter: $('#StatusFilterId').val(),
                        };
                    }
                },
                columnDefs: [
                    {
                        width: 120,
                        targets: 0,
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
                                        _viewBatchAirtimeModal.open({code: data.record.batchCode});
                                    }
                                },
                                {
                                    text: app.localize('Approval'),
                                    visible: function (data) { 
                                        return (data.record.status == 0) && _permissions.approval;
                                    },
                                    action: function (data) {
                                        approvalBatchAirtime(data.record);
                                    }
                                },  {
                                    text: app.localize('Reject'),
                                    visible: function (data) { 
                                        return (data.record.status == 0) && _permissions.approval;
                                    },
                                    action: function (data) {
                                        rejectBatchAirtime(data.record);
                                    }
                                }, {
                                    text: app.localize('Edit'),
                                    visible: function (data) {
                                        return (data.record.status == 0) && _permissions.edit;
                                    },
                                    action: function (data) {
                                        _createOrEditModal.open({code: data.record.batchCode});
                                    }
                                },
                                {
                                    text: app.localize('Delete'),
                                    visible: function (data) {
                                        return (data.record.status == 0 || data.record.status == 3) && _permissions.delete;
                                    },
                                    action: function (data) {
                                        deleteBatchAirtime(data.record);
                                    }
                                }]
                        }
                    },
                    {
                        targets: 1,
                        data: "batchCode",
                        name: "batchCode"
                    },
                    {
                        targets: 2,
                        data: "providerCode",
                        name: "providerCode",
                        render: function (data, e, row) {
                            return row.providerCode + " - " + row.providerName
                        }
                    },
                    {
                        targets: 3,
                        class: "text-right",
                        data: "amount",
                        name: "amount",
                        render: function (data, e, row) {
                            return Sv.NumberToString(data) + "đ";
                        }
                    },
                    {
                        targets: 4,
                        data: "discount",
                        class: "text-right",
                        name: "discount",
                        render: function (data, e, row) {
                            return data + "%"
                        }
                    },
                    {
                        targets: 5,
                        data: "airtime",
                        class: "text-right",
                        name: "airtime",
                        render: function (data, e, row) {
                            var a = parseFloat(row.amount) + parseFloat(row.amount) * (parseFloat(row.discount) / 100);
                            return Sv.NumberToString(Math.round(a));
                        }
                    },
                    {
                        targets: 6,
                        data: "status",
                        name: "status",
                        render: function (status) {
                            return app.localize('Enum_BatchAirtimeStatus_' + status);
                        }

                    },

                    {
                        targets: 7,
                        data: null,
                        render: function (data, e, row) {
                            if (row.createdDate) {
                                return moment(row.createdDate).format('DD/MM/YYYY HH:mm:ss');
                            }
                            return ""; 
                        }
                    },
                    {
                        targets: 8,
                        data: null,
                        render: function (data, e, row) {
                            return row.createdAccountName != null ? row.createdAccountName : "";
                        }
                    },
                    {
                        targets: 9,
                        data: null,
                        render: function (data, e, row) {
                            if (row.modifiedDate) {
                                let date =  moment(row.modifiedDate).format('DD/MM/YYYY HH:mm:ss');
                                return date != "01/01/0001 00:00" ? date : "";
                            }
                            return "";
                        }
                    },
                    {
                        targets: 10,
                        data: null,
                        render: function (data, e, row) {
                            return row.modifiedAccountName != null ? row.modifiedAccountName : "";
                        }
                    }
                ]
            });

            function getBatchAirtimes() {
                dataTable.ajax.reload();
            }

            function deleteBatchAirtime(batchAirtime) {
                abp.message.confirm("Bạn có chắc chắn muốn xóa ?")
                    .then(function (confirmed) {
                        if (confirmed) {
                            _batchAirtimesService.delete(batchAirtime.batchCode).done(function () {
                                getBatchAirtimes(true);
                                abp.notify.success(app.localize('SuccessfullyDeleted'));
                            });
                        }
                    });
            }

            function approvalBatchAirtime(batchAirtime) {
                abp.message.confirm("Bạn có chắc chắn muốn duyệt giao dịch nhập kho Airtime: "+batchAirtime.batchCode+"?")
                    .then(function (confirmed) {
                        if (confirmed) {
                            _batchAirtimesService.approval(batchAirtime.batchCode).done(function () {
                                getBatchAirtimes(true);
                                abp.notify.success(app.localize('SuccessfullyApproval'));
                            });
                        }
                    });
            }

            function rejectBatchAirtime(batchAirtime) {
                abp.message.confirm("Bạn có chắc chắn muốn hủy giao dịch nhập kho Airtime: "+batchAirtime.batchCode+"?")
                    .then(function (confirmed) {
                        if (confirmed) {
                            _batchAirtimesService.reject(batchAirtime.batchCode).done(function () {
                                getBatchAirtimes(true);
                                abp.notify.success(app.localize('SuccessfullyReject'));
                            });
                        }
                    });
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

              $('#CreateNewAgentSupperButton').click(function () {
                _createOrEditModal.open();
            });

            $('#ExportToExcelButton').click(function () {
                _batchAirtimesService
                    .getBatchAirtimesToExcel({
                        filter: $('#BatchAirtimesTableFilter').val(),
                        batchCodeFilter: $('#BatchCodeFilterId').val(),
                        providerCodeFilter: $('#providerCode').val(),
                        minAirtimeFilter: getDateFilter($('#MinCreatedDateFilterId'), false),
                        maxAirtimeFilter: getDateFilter($('#MaxCreatedDateFilterId'), true),
                        statusFilter: $('#StatusFilterId').val(),
                    })
                    .done(function (result) {
                        app.downloadTempFile(result);
                    });
            });

            abp.event.on('app.createOrEditBatchAirtimeModalSaved', function () {
                getBatchAirtimes();
            });

            $('#GetBatchAirtimesButton').click(function (e) {
                e.preventDefault();
                getBatchAirtimes();
            });

            $(document).keypress(function (e) {
                if (e.which === 13) {
                    getBatchAirtimes();
                }
            });

        }
    );
})();