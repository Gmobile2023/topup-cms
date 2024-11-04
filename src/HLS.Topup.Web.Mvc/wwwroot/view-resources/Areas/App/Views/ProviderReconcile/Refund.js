(function () {
    $(function () {

        var _$providersTable = $('#tableRefund');
        const _svcCompareService = abp.services.app.compare;
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            refund: abp.auth.hasPermission('Pages.RefundsReconcile.Approval'),
        };

        abp.event.on('app.refundCompareAirtime', function () {
            getProviders();
        });

        var _viewDetailModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ProviderReconcile/RefundDetailModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/ProviderReconcile/_ViewRefund.js',
            modalClass: 'RefundDetailModal',
            modalSize: 'modal-xl'
        });

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z");
        }

        const dataTable = _$providersTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _svcCompareService.getCompareRefundList,
                inputFilter: function () {
                    return {
                        fromDateTrans: getDateFilter($('#FromDateTran')),
                        toDateTrans: getDateFilter($('#TromDateTran')),
                        providerCode: $("#dropProvider").val()
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
                                    _viewDetailModal.open({ providerCode: data.record.provider, date: data.record.transDate, keyCode: data.record.keyCode });
                                }
                            },
                            {
                                text: app.localize('Hoàn tiền'),
                                visible: function (data) {
                                    return _permissions.refund && data.record.refundWaitQuantity !== 0;
                                },
                                action: function (data) {
                                    var refund = {};
                                    refund.ProviderCode = data.record.provider;
                                    refund.TransDate = data.record.transDateSoft;
                                    refund.KeyCode = data.record.keyCode;
                                    approvalRefund(refund);
                                    getProviders();
                                }
                            }
                        ]
                        //"headerCallback": function (thead, data, start, end, display) {
                        //    try {
                        //        var rawServerResponse = this.api().settings()[0].rawServerResponse;
                        //        if (rawServerResponse !== undefined && rawServerResponse.totalData !== undefined) {
                        //            $(thead).find('th').eq(1).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.amount));
                        //        }
                        //    } catch (e) {
                        //        console.log("không có total")
                        //    }
                        //}
                    }
                },
                {
                    targets: 2,
                    data: "transDate",
                    render: function (transDate) {
                        if (transDate) {
                            return moment(transDate).format('DD/MM/YYYY');
                        }
                        return "";
                    }
                },
                {
                    targets: 3,
                    data: "provider"
                },
                {
                    targets: 4,
                    data: "quantity",
                    render: function (data) {
                        return Sv.format_number(data);
                    }
                },
                {
                    targets: 5,
                    data: "amount",
                    render: function (data) {
                        return Sv.format_number(data);
                    }
                },
                {
                    targets: 6,
                    data: "refundQuantity",
                    render: function (data) {
                        return Sv.format_number(data);
                    }
                },
                {
                    targets: 7,
                    data: "refundAmount",
                    render: function (data) {
                        return Sv.format_number(data);
                    }
                },
                {
                    targets: 8,
                    data: "refundWaitQuantity",
                    render: function (data) {
                        return Sv.format_number(data);
                    }
                },
                {
                    targets: 9,
                    data: "refundWaitAmount",
                    render: function (data) {
                        return Sv.format_number(data);
                    }
                }
            ],
            "headerCallback": function (thead, data, start, end, display) {
                try {
                    var rawServerResponse = this.api().settings()[0].rawServerResponse;
                    if (rawServerResponse !== undefined && rawServerResponse.totalData !== undefined) {
                        //$(thead).find('th').eq(1).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.quantityAgent));
                        //$(thead).find('th').eq(2).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.quantity));
                        //$(thead).find('th').eq(3).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.discount));
                        //$(thead).find('th').eq(4).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.fee));
                        //$(thead).find('th').eq(5).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.price));
                    }
                } catch (e) {
                    console.log("không có total")
                }
            }
        });

        function getProviders() {
            dataTable.ajax.reload();
        }

        function approvalRefund(refund) {
            abp.message.confirm(
                'Bạn có chắc chắn muốn hoàn tiền này không?',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _svcCompareService.refundAmountCompare(
                            refund
                        ).done(function () {
                            abp.message.info('Xác nhận hoàn tiền. Tiến trình đang được xử lý, vui lòng kiểm tra email để xem kết quả');
                        }).always(function () {
                        });
                    }
                }
            );
        }

        $('#ExportToExcelButton').click(function () {
            _svcCompareService.getCompareRefundListToExcel({
                fromDateTrans: getDateFilter($('#FromDateTran')),
                toDateTrans: getDateFilter($('#TromDateTran')),
                providerCode: $("#dropProvider").val()
            })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetProvidersButton').click(function (e) {
            e.preventDefault();
            getProviders();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getProviders();
            }
        });
    });
})();
