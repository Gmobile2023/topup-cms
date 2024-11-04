(function () {
    $(function () {

        var _$providersTable = $('#ProvidersTable');

        $('.datetime-index').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L LTS'
        });

        const _permissions = {
            create: abp.auth.hasPermission('Pages.TransactionManagements.Create'),
            edit: abp.auth.hasPermission('Pages.TransactionManagements.Edit'),
            delete: abp.auth.hasPermission('Pages.TransactionManagements.Delete'),
            cancel: abp.auth.hasPermission('Pages.TransactionManagements.Cancel'),
            refund: abp.auth.hasPermission('Pages.TransactionManagements.Refund'),
            pin_code: abp.auth.hasPermission('Pages.TransactionManagements.PinCode'),
        };

        var _createImportModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/TransactionManagement/CreateImportModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/TransactionManagement/_CreateImportModal.js',
            modalClass: 'CreateImportModal',
            modalSize: 'modal-xl'
        });

        var _viewProviderModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Providers/ViewproviderModal',
            modalClass: 'ViewProviderModal',
            modalSize: 'modal-xl'
        });

        var _viewPinCodeModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/TransactionManagement/PinCodeDetailModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/TransactionManagement/_PinCodeDetailModal.js',
            modalClass: 'ViewPinCodeModal',
            modalSize: 'modal-lg'
        });

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY/MM/DD HH:mm:ss");
        }
        // fromDate: $('#FromDateFilterId').val(),//getDateFilter($('#FromDateFilterId')),
        //  toDate: $('#ToDateFilterId').val(),//getDateFilter($('#ToDateFilterId')),
        const _transaction = abp.services.app.transactions;
        const dataTable = _$providersTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            deferLoading: 0,
            listAction: {
                ajaxFunction: _transaction.getTransactionHistories,
                inputFilter: function () {
                    return {
                        fromDate: getDateFilter($('#FromDateFilterId')),
                        toDate: getDateFilter($('#ToDateFilterId')),                      
                        partnerCodeFilter: $('#AgentFilter').val(),
                        staffAccount: $('#StaffAccountFilter').val(),
                        serviceCodes: $('#ServiceSelect').val(),
                        categoryCodes: $('#ProductType').val(),
                        productCodes: $('#ProductList').val(),
                        providerCode: $('#ProviderSelect').val(),
                        parentProvider: $('#ParentProviderSelect').val(),
                        mobileNumberFilter: $('#MobileNumberFilterId').val(),
                        transCodeFilter: $('#txtTransCode').val(),
                        transRefFilter: $('#txtTranRef').val(),
                        providerTransCode: $('#txtProviderTransCode').val(),
                        statusFilter: $('#StatusFilterId').val(),
                        agentTypeFilter: $('#AgentTypeFilter').val(),
                        saleTypeFilter: $('#SaleTypeFilter').val(),
                        receiverType: $('#selectReceiverType').val(),
                        providerResponseCode: $('#txtProviderResponseCode').val(),
                        receiverTypeResponse: $('#selectReceiverTypeResponse').val(),
                        isAdmin: true
                    };
                }
            },
            columnDefs: [
                {
                    targets: 0,
                    className: 'control responsive text-center',
                    orderable: false,
                    render: function () {
                        return '';
                    }
                },
                {
                    targets: 1,
                    className: "all clearfix",
                    data: null,
                    orderable: false,
                    autoWidth: false,
                    defaultContent: '',
                    rowAction: {
                        text: '<i class="fa fa-cog"></i> ' + app.localize('Actions') + ' <span class="caret"></span>',
                        items: [
                            {
                                text: 'Xem chi tiết',
                                visible: function (data) {
                                    return _permissions.pin_code && data.record.status === 1 && data.record.serviceCode === "PIN_CODE"
                                        || _permissions.pin_code && data.record.status === 1 && data.record.serviceCode === "PIN_DATA"
                                        || _permissions.pin_code && data.record.status === 1 && data.record.serviceCode === "PIN_GAME";
                                },
                                action: function (data) {
                                    if (data.record.serviceCode === "PIN_CODE" || data.record.serviceCode === "PIN_DATA" || data.record.serviceCode === "PIN_GAME")
                                        _viewPinCodeModal.open({ transCode: data.record.transRef });
                                }
                            },
                            {
                                text: 'Hủy giao dịch - Hoàn tiền',
                                visible: function (data) {
                                    return (data.record.status === 9 || data.record.status === 99 || data.record.status === 9 || data.record.status === 8 || data.record.status === 6 || data.record.status === 7) && _permissions.refund;
                                },
                                action: function (data) {
                                    abp.message.confirm("Bạn có chắc chắn muốn hủy giao dịch?").then(function (confirmed) {
                                        if (confirmed) {
                                            const obj = {
                                                transCode: data.record.transCode,
                                                transRef: data.record.transRef,
                                                partnerCode: data.record.partnerCode,
                                                paymentAmount: data.record.paymentAmount
                                            };
                                            abp.ui.setBusy();
                                            _transaction.refundTransactionRequest(obj)
                                                .done((rs) => {
                                                    abp.message.success("Thành công");
                                                    getProviders();
                                                }
                                                ).always((data) => {
                                                    abp.ui.clearBusy();
                                                })
                                        }
                                    });
                                }
                            },
                            {
                                text: 'Cập nhật trạng thái',
                                visible: function () {
                                    return _permissions.edit;
                                },
                                action: function (data) {
                                    showStatus(data);
                                }
                            },
                            {
                                text: 'Kiểm tra trạng thái giao dịch bên NCC',
                                visible: function (data) {
                                    return _permissions.refund && data.record.provider != null;
                                },
                                action: function (data) {
                                    checkTrans(data);
                                }
                            },
                            {
                                text: 'Nạp bù giao dịch',
                                visible: function (data) {
                                    return data.record.status === 8 && data.record.saleType === 1;
                                },
                                action: function (data) {
                                    offsetTopup(data);
                                }
                            },
                            {
                                text: 'Đồng bộ mã thẻ',
                                visible: function (data) {
                                    return _permissions.pin_code && data.record.serviceCode === "PIN_CODE"
                                        || _permissions.pin_code && data.record.serviceCode === "PIN_DATA"
                                        || _permissions.pin_code && data.record.serviceCode === "PIN_GAME";
                                },
                                action: function (data) {
                                    updateSyncCardCode(data);
                                }
                            }
                        ]
                    }
                },
                {
                    targets: 2,
                    data: "agentType",
                    render: function (agentType) {
                        return app.localize('Enum_AgentType_' + agentType);
                    }
                },
                {
                    targets: 3,
                    data: "partnerCode"
                },
                {
                    targets: 4,
                    data: "provider"
                },
                {
                    targets: 5,
                    data: "status",
                    name: "status",
                    // render: function (status) {
                    //     return app.localize('Enum_TopupStatus_' + status);
                    // }
                    render: function (status) {
                        const $span = $("<span/>").addClass("label");
                        if (status === 1) {
                            $span.addClass("label label-success label-inline").text(app.localize('Enum_TopupStatus_' + status));
                        } else if (status === 4 || status === 7 || status === 8 || status === 10) {
                            $span.addClass("label label-warning label-inline").text(app.localize('Enum_TopupStatus_' + status));
                        } else if (status === 9) {
                            $span.addClass("label label-primary label-inline").text(app.localize('Enum_TopupStatus_' + status));
                        } else if (status === 2 || status === 3) {
                            $span.addClass("label label-danger label-inline").text(app.localize('Enum_TopupStatus_' + status));
                        } else if (status === 6) {
                            $span.addClass("label label-info label-inline").text(app.localize('Enum_TopupStatus_' + status));
                        }
                        else if (status === 10) {
                            $span.addClass("label label-danger label-inline").text(app.localize('Enum_TopupStatus_' + status));
                        } else {
                            $span.addClass("label label-default label-inline").text(app.localize('Enum_TopupStatus_' + status));
                        }
                        return $span[0].outerHTML;
                    }
                },
                {
                    targets: 6,
                    data: "receiverInfo",
                    name: "receiverInfo"
                },
                {
                    targets: 7,
                    data: "serviceName",
                    render: function (data, row) {
                        return data;
                    }
                },
                {
                    targets: 8,
                    data: "categoryCode"
                },
                {
                    targets: 9,
                    data: "productCode"
                },
                {
                    targets: 10,
                    data: "price",
                    render: function (data) {
                        return data ? Sv.format_number(data) : "0"
                    }
                },
                {
                    targets: 11,
                    data: "quantity",
                    name: "quantity",
                    render: function (data) {
                        return data ? Sv.format_number(data) : "0"
                    }
                },
                {
                    targets: 12,
                    data: "discountAmount",
                    render: function (data) {
                        return data ? Sv.format_number(data) : "0"
                    }
                },
                {
                    targets: 13,
                    data: "fee",
                    render: function (data) {
                        return data ? Sv.format_number(data) : "0"
                    }
                },
                {
                    targets: 14,
                    data: "paymentAmount",
                    render: function (data) {
                        return data ? Sv.format_number(data) : "0"
                    }
                },
                {
                    targets: 15,
                    data: "createdTime",
                    name: "createdTime",
                    render: function (createdTime) {
                        if (createdTime) {
                            return moment(new Date(createdTime)).format('DD/MM/YYYY HH:mm:ss');
                        }
                        return "";
                    }
                },
                {
                    targets: 16,
                    data: "staffAccount"
                },
                {
                    targets: 17,
                    data: "transCode"
                },
                {
                    targets: 18,
                    data: "transRef"
                },
                {
                    targets: 19,
                    data: "providerTransCode"
                },
                {
                    targets: 20,
                    data: "requestDate",
                    name: "requestDate",
                    render: function (requestDate) {
                        if (requestDate) {
                            return moment(new Date(requestDate)).format('DD/MM/YYYY HH:mm:ss');
                        }
                        return "";
                    }
                },
                {
                    targets: 21,
                    data: "responseDate",
                    name: "responseDate",
                    render: function (responseDate) {
                        if (responseDate) {
                            return moment(new Date(responseDate)).format('DD/MM/YYYY HH:mm:ss');
                        }
                        return "";
                    }
                },
                {
                    targets: 22,
                    data: "totalTime"
                },
                {
                    targets: 23,
                    data: "saleType",
                    render: function (saleType) {
                        const $span = $("<span/>").addClass("label");
                        if (saleType === 0) {
                            $span.addClass("label label-success label-inline").text(app.localize('Enum_SaleType_' + saleType));
                        } else if (saleType === 1) {
                            $span.addClass("label label-warning label-inline").text(app.localize('Enum_SaleType_' + saleType));
                        } else {
                            $span.addClass("label label-default label-inline").text(app.localize('Enum_SaleType_' + saleType));
                        }
                        return $span[0].outerHTML;
                    }
                },
                {
                    targets: 24,
                    data: "receiverType"
                },
                {
                    targets: 25,
                    data: "providerResponseCode"
                },
                {
                    targets: 26,
                    data: "receiverTypeResponse"
                },
                {
                    targets: 27,
                    data: "parentProvider"
                }
            ]
        });

        function getProviders() {
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

        $('#ExportToExcelButton').click(function () {
            _transaction.getTransactionHistoryToExcel({
                fromDate: getDateFilter($('#FromDateFilterId')),
                toDate: getDateFilter($('#ToDateFilterId')),
                partnerCodeFilter: $('#AgentFilter').val(),
                staffAccount: $('#StaffAccountFilter').val(),
                serviceCodes: $('#ServiceSelect').val(),
                categoryCodes: $('#ProductType').val(),
                productCodes: $('#ProductList').val(),
                providerCode: $('#ProviderSelect').val(),
                parentProvider: $('#ParentProviderSelect').val(),
                mobileNumberFilter: $('#MobileNumberFilterId').val(),
                transCodeFilter: $('#txtTransCode').val(),
                transRefFilter: $('#txtTranRef').val(),
                providerTransCode: $('#txtProviderTransCode').val(),
                statusFilter: $('#StatusFilterId').val(),
                agentTypeFilter: $('#AgentTypeFilter').val(),
                saleTypeFilter: $('#SaleTypeFilter').val(),
                receiverType: $('#selectReceiverType').val(),
                providerResponseCode: $('#txtProviderResponseCode').val(),
                receiverTypeResponse: $('#selectReceiverTypeResponse').val(),
                isAdmin: true
            })
                .done(function (result) {
                    //app.downloadTempFile(result);
                    abp.message.info('Hệ thống đang bắt đầu lấy dữ liệu. Quá hoàn tất sẽ gửi thông báo cho bạn. Trân trọng!');
                });
        });

        $('.btn-search-transaction').click(function (e) {
            e.preventDefault();
            getProviders();
        });

        $('#btnOpenImportButton').click(function (e) {
            _createImportModal.open();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getProviders();
            }
        });

        $("#StaffAccountFilter").select2({
            placeholder: 'Select',
            ajax: {
                url: abp.appPath + "api/services/app/CommonLookup/GetListUserSearch",
                dataType: 'json',
                delay: 250,
                data: function (params) {
                    return {
                        search: params.term,
                        agentType: 99,
                        page: params.page,
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
            minimumInputLength: 1,
            language: abp.localization.currentCulture.name
        });

        $('#ServiceSelect').change(function () {
            let serviceCode = $('#ServiceSelect').val();

            if (serviceCode != null || serviceCode !== '' || serviceCode.length > 0) {
                $('#ProductType').removeAttr('disabled').empty();
                $('#ProductList').prop('disabled', true).empty().append('<option value="">Chọn</option>');
                getProductType(serviceCode);
            } else {
                $('#ProductType').prop('disabled', true);
            }
        });

        $('#ProductType').change(function () {
            /*  let productType = $('#ProductType option:selected').attr('data-id');*/
            let productTypeId = $('#ProductType').val();
            if (productTypeId != null || productTypeId !== '' || productTypeId.length > 0) {
                $('#ProductList').removeAttr('disabled').empty();
                getProductListMuti(productTypeId);
            } else {
                $('#ProductList').prop('disabled', true);
            }
        });

        $('#LimitProduct_AgentType').change(function () {
            if ($(this).val() != null || $(this).val() !== '') {
                $("#AgentFilter").removeAttr('disabled');
            } else {
                $("#AgentFilter").prop('disabled', true);
            }
        });

        $('#AgentTypeFilter').change(function () {
            if ($(this).val() != null || $(this).val() !== '') {
                $("#AgentFilter").removeAttr('disabled');
            } else {
                $("#AgentFilter").prop('disabled', true);
            }
        });

        $("#AgentFilter").select2({
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
                        agentType: $('#AgentTypeFilter').val()
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

        function getProductType(serviceCode) {
            let options = '';
            _transaction.getCategoryByServiceCodeMuti(serviceCode).done(function (obj) {
                $('#ProductType').append('<option value="">Tất cả</option>');
                $.each(obj, function (key, value) {
                    $('#ProductType')
                        .append($("<option></option>")
                            .attr("value", value.categoryCode)
                            .attr("data-id", value.id)
                            .attr("data-category", value.categoryName)
                            .text(value.categoryName));
                });
            }).always(function () {

            });
        }

        function getProductList(categoryId) {
            let options = '';
            _transaction.getProducts(categoryId).done(function (obj) {
                $('#ProductList').append('<option value="">Tất cả</option>');
                $.each(obj, function (key, value) {
                    $('#ProductList')
                        .append($("<option></option>")
                            .attr("value", value.productCode)
                            .attr("data-code", value.productCode)
                            .attr("data-category", value.categoryName)
                            .text(value.productName));
                });
            }).always(function () {

            });
        }

        function getProductListMuti(categoryCode) {
            let options = '';
            _transaction.getProductsMuti(categoryCode).done(function (obj) {
                $('#ProductList').append('<option value="">Tất cả</option>');
                $.each(obj, function (key, value) {
                    $('#ProductList')
                        .append($("<option></option>")
                            .attr("value", value.productCode)
                            .attr("data-code", value.productCode)
                            .attr("data-category", value.categoryName)
                            .text(value.productName));
                });
            }).always(function () {

            });
        }

        function showStatus(data) {
            console.log(data);
            bootbox.prompt({
                title: "Chọn trạng thái!",
                inputType: 'select',
                inputOptions: [
                    // {
                    //     text: app.localize('Enum_TopupStatus_0'),
                    //     value: '0'
                    // },
                    {
                        text: app.localize('Enum_TopupStatus_1'),
                        value: '1',
                    },
                    {
                        text: app.localize('Enum_TopupStatus_2'),
                        value: '2',
                    },
                    {
                        text: app.localize('Enum_TopupStatus_3'),
                        value: '3',
                    },
                    {
                        text: app.localize('Enum_TopupStatus_4'),
                        value: '4',
                    },
                    // {
                    //     text: app.localize('Enum_TopupStatus_5'),
                    //     value: '5',
                    // },
                    {
                        text: app.localize('Enum_TopupStatus_6'),
                        value: '6',
                    },
                    {
                        text: app.localize('Enum_TopupStatus_7'),
                        value: '7',
                    },
                    {
                        text: app.localize('Enum_TopupStatus_8'),
                        value: '8',
                    },
                    {
                        text: app.localize('Enum_TopupStatus_9'),
                        value: '9',
                    }
                ],
                callback: function (result) {
                    if (result !== null && result !== undefined) {
                        //abp.setBusy(true);
                        console.log(result);
                        _transaction.updateStatus(
                            { transCode: data.record.transCode, status: result }
                        ).done(function (rs) {
                            if (rs.responseCode === "01") {
                                abp.message.info(app.localize('SavedSuccessfully'));
                                getProviders();
                            } else {
                                abp.message.error(rs.responseMessage);
                            }

                        }).always(function () {
                            //abp.setBusy(false);
                        });
                    }

                }
            });
        }

        function checkTrans(data) {
            _transaction.checkTransProvider(
                {
                    transCodeToCheck: data.record.providerTransCode,
                    serviceCode: data.record.serviceCode,
                    providerCode: data.record.provider
                }
            ).done(function (rs) {
                if (rs.responseCode === "01") {
                    abp.message.info("Giao dịch thành công");
                } else if (rs.responseCode === "4007") {
                    abp.message.warn("Giao dịch chưa có kết quả");
                } else {
                    abp.message.error("GD lỗi: " + rs.responseMessage + "\nMã lỗi: " + rs.responseCode + "\nThông tin lỗi NCC: " + rs.extraInfo);
                }

            }).always(function () {
                //abp.setBusy(false);
            });
        }

        function offsetTopup(data) {
            abp.message.confirm(
                '', 'Bạn chắc chắn muốn nạp bù cho giao dịch này không?',
                function (isConfirmed) {
                    if (isConfirmed) {
                        _transaction.offsetBuRequest(
                            {
                                transCode: data.record.transCode
                            }
                        ).done(function () {
                            abp.message.info("Nạp bù thành công.");
                            getProviders();
                        }).always(function () {
                        });
                    }
                }
            );
        }

        function updateSyncCardCode(data) {
            _transaction.updateCardCode(
                {
                    transCode: data.record.transCode,
                }
            ).done(function (rs) {
                if (rs.responseStatus.errorCode == "01") {
                    abp.message.info(rs.responseStatus.message);
                } else {
                    abp.message.error(rs.responseStatus.message);
                }
            }).always(function () {
                //abp.setBusy(false);
            });
        }

        $("#PartnerCodeFilterId").on('select2:open', function () {
            let $p = $("#PartnerCodeFilterId").parent();
            let val = $("#PartnerCodeFilterId").val();
            if (val != null && val.length > 0) {
                if ($p.find(".s2icon").length == 0) {
                    $p.find(".select2-selection.select2-selection--single").append('<i class="fa fa-times s2icon"></i>')
                }
            } else {
                $p.find(".select2-selection.select2-selection--single .s2icon").remove();
            }
        });

        $("#AdvacedAuditFiltersArea").on('click', '.s2icon', function () {
            $("#PartnerCodeFilterId").val("").trigger("change");
            $("#PartnerCodeFilterId").parent().find(".select2-selection.select2-selection--single .s2icon").remove();
        });
    });
})();
