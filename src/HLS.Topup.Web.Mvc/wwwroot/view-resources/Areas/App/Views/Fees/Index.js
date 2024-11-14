(function () {
    $(function () {

        var _$feesTable = $('#FeesTable');
        var _feesService = abp.services.app.fees;

        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Fees.Create'),
            edit: abp.auth.hasPermission('Pages.Fees.Edit'),
            approval: abp.auth.hasPermission('Pages.Fees.Approval'),
            cancel: abp.auth.hasPermission('Pages.Fees.Cancel'),
            stop: abp.auth.hasPermission('Pages.Fees.Stop'),
            delete: abp.auth.hasPermission('Pages.Fees.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Fees/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Fees/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditFeeModal',
            modalSize: 'modal-xl'
        });

        var _viewFeeModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Fees/ViewFeeModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Fees/_ViewFeeModal.js',
            modalClass: 'ViewFeeModal',
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

        var dataTable = _$feesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _feesService.getAll,
                inputFilter: function () {
                    return {
                        nameFilter: $('#NameFilter').val(),
                        codeFilter: $('#CodeFilterId').val(),
                        fromCreationTimeFilter: getDateFilter($('#FromCreatedTimeFilter')),
                        toCreationTimeFilter: getToDateFilter($('#ToCreatedTimeFilter')),
                        fromApprovedTimeFilter: getDateFilter($('#FromApprovedTimeFilter')),
                        toApprovedTimeFilter: getToDateFilter($('#ToApprovedTimeFilter')),
                        fromAppliedTimeFilter: getDateFilter($('#FromAppliedTimeFilter')),
                        toAppliedTimeFilter: getToDateFilter($('#ToAppliedTimeFilter')),
                        statusFilter: $('#StatusFilterId').val(),
                        productTypeFilter: $('#ProductTypeFilter').val(),
                        productFilter: $('#productFilter').val()
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
                                    _viewFeeModal.open({id: data.record.fee.id, isView: true});
                                }
                            },
                            {
                                text: app.localize('Edit'),
                                visible: function (data) {
                                    return _permissions.edit && data.record.fee.status === 0;
                                },
                                action: function (data) {
                                    _viewFeeModal.open({id: data.record.fee.id, isView: false});
                                }
                            },
                            {
                                text: app.localize('Approval'),
                                visible: function (data) {
                                    return _permissions.approval && data.record.fee.status === 0;
                                },
                                action: function (data) {
                                    approval(data.record.fee);
                                }
                            },
                            {
                                text: app.localize('Cancel'),
                                visible: function (data) {
                                    return _permissions.cancel && data.record.fee.status === 0;
                                },
                                action: function (data) {
                                    cancel(data.record.fee);
                                }
                            },
                            {
                                text: app.localize('Fees_Stop'),
                                visible: function (data) {
                                    return _permissions.stop && data.record.fee.status === 2;
                                },
                                action: function (data) {
                                    stop(data.record.fee);
                                }
                            },
                            {
                                text: app.localize('Delete'),
                                visible: function () {
                                    return _permissions.delete;
                                },
                                action: function (data) {
                                    deleteFee(data.record.fee);
                                }
                            }]
                    }
                },
                {
                    targets: 2,
                    data: "fee.name",
                    name: "name"
                },
                {
                    targets: 3,
                    data: "fee.code",
                    name: "code"
                },
                {
                    targets: 4,
                    data: "fee.agentType",
                    name: "agentType",
                    render: function (agentType) {
                        return app.localize('Enum_AgentType_' + agentType);
                    }
                },
                {
                    targets: 5,
                    data: "fee.agentName",
                    name: "agentName"
                },
                {
                    targets: 6,
                    data: "fee.statusName",
                    name: "fee.statusName",
                },
                {
                    targets: 7,
                    data: "fee.dateApproved",
                    name: "dateApproved",
                    render: function (dateApproved) {
                        if (dateApproved) {
                            return moment(dateApproved).format('L LTS');
                        }
                        return "";
                    }
                },
                {
                    targets: 8,
                    data: "fee.creationTime",
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
                    data: "fee.fromDate",
                    name: "fromDate",
                    render: function (fromDate) {
                        if (fromDate) {
                            return moment(fromDate).format('L LTS');
                        }
                        return "";
                    }

                },
                {
                    targets: 10,
                    data: "fee.toDate",
                    name: "toDate",
                    render: function (toDate) {
                        if (toDate) {
                            return moment(toDate).format('L LTS');
                        }
                        return "";
                    }
                },
                {
                    targets: 11,
                    data: "fee.userApproved",
                    name: "userApproved",
                }
            ]
        });

        function getFees() {
            dataTable.ajax.reload();
        }

        function deleteFee(fee) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _feesService.delete({
                            id: fee.id
                        }).done(function () {
                            getFees(true);
                            abp.notify.success(app.localize('SuccessfullyDeleted'));
                        });
                    }
                }
            );
        }

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

                    $('#' + position).append(options).select2();
                }
            }).done(function () {

            });
        }

        function approval(fee) {
            abp.message.confirm(
                'Bạn có chắc chắn muốn duyệt chính sách này không?',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _feesService.approval({id: fee.id}).done(function () {
                            getFees();
                            abp.message.success('Duyệt thành công!');
                        });
                    }
                }
            );
        }

        function cancel(fee) {
            abp.message.confirm(
                'Bạn chắc chắn muốn hủy chính sách này không?',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _feesService.cancel({id: fee.id}).done(function () {
                            getFees();
                            abp.message.success('Hủy thành công');
                        });
                    }
                }
            );
        }

        function stop(fee) {
            abp.message.confirm(
                'Bạn chắc chắn muốn dừng chính sách này không?',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _feesService.stop({id: fee.id}).done(function () {
                            getFees();
                            abp.message.success('Dừng chính sách thành công');
                        });
                    }
                }
            );
        }

        $('#ProductTypeFilter').change(function () {
            let productTypeId = $('#ProductTypeFilter').val();
            if (productTypeId != '') {
                _feesService.getProducts(parseInt(productTypeId)).done(function (obj) {
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

        $('#CreateNewFeeButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportToExcelButton').click(function () {
            _feesService
                .getFeesToExcel({
                    nameFilter: $('#NameFilter').val(),
                    codeFilter: $('#CodeFilterId').val(),
                    fromCreationTimeFilter: getDateFilter($('#FromCreatedTimeFilter')),
                    toCreationTimeFilter: getToDateFilter($('#ToCreatedTimeFilter')),
                    fromApprovedTimeFilter: getDateFilter($('#FromAppliedTimeFilter')),
                    toApprovedTimeFilter: getToDateFilter($('#ToAppliedTimeFilter')),
                    statusFilter: $('#StatusFilterId').val(),
                    productTypeFilter: $('#ProductTypeFilter').val(),
                    productFilter: $('#productFilter').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditFeeModalSaved', function () {
            getFees();
        });

        $('#GetFeesButton').click(function (e) {
            e.preventDefault();
            getFees();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getFees();
            }
        });
    });
})();
