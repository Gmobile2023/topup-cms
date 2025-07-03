(function () {
    $(function () {
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var getDateFilter = function (element) {
            return element.data("DateTimePicker").date() === null ? null : element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z");
        };

        var _$table = $('#Table');
        var _transactionService = abp.services.app.transactions;
        var dataTable = _$table.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _transactionService.getTransactionHistories,
                inputFilter: function () {
                    return {
                        filter: $("#Filter").val(),
                        serviceCode: $('#ServiceCodeFilter').val(),
                        statusFilter: $("#StatusFilterId").val(),
                        fromDate: getDateFilter($('#FromDateFilterId')),
                        toDate: getDateFilter($('#ToDateFilterId')),
                        agentTypeFilter:99
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
                    //data:"transRef",
                    //name:"transRef"
                    render: (data, type, row) => {
                        let opt_print = '';
                        if (row.status === 1 || row.status === 9) {
                            opt_print = '<i class="fa fa-print print-invoice-item" data-type="'+ row.serviceCode +'" data-trans-code="'+ row.transRef +'" style="color: #2188C9; cursor: pointer; margin-right: 15px;" title="In hoá đơn" aria-hidden="true"></i>';
                        } else {
                            opt_print = '<i class="fa fa-print" style="color: #2188C9; cursor: pointer; margin-right: 15px; opacity: .4;" title="In hoá đơn" aria-hidden="true"></i>';
                        }

                        if (row.serviceCode === "PIN_CODE" || row.serviceCode === "PIN_DATA" || row.serviceCode === "PIN_GAME")
                            return opt_print + '<a href = "TransactionDetail?transcode=' + row.transRef + '&serviceCode=' + row.serviceCode + '">' + row.transRef + '</a>';
                        return opt_print + row.transRef;
                    }
                },
                {
                    targets: 2,
                    data: "serviceName",
                    render: function (data, row) {
                        return data;
                    }
                },
                {
                    targets: 3,
                    data: "status",
                    name: "status",
                    render: function (status) {
                        return app.localize('Enum_TopupStatus_' + status);
                    }
                },
                {
                    targets: 4,
                    data: "receiverInfo",
                    name: "receiverInfo"
                },
                {
                    targets: 5,
                    data: "quantity",
                    name: "quantity",
                    //className: "text-right",
                    render: function (data) {
                        return data ? Sv.format_number(data) : "0"
                    }
                },
                {
                    targets: 6,
                    data: "price",
                    //className: "text-right",
                    render: function (data) {
                        return data ? Sv.format_number(data) : "0"
                    }

                },
                {
                    targets: 7,
                    data: "amount",
                    // className: "text-right",
                    render: function (data) {
                        return data ? Sv.format_number(data) : "0"
                    }
                },
                {
                    targets: 8,
                    data: "discountAmount",
                    render: function (data) {
                        return data ? Sv.format_number(data) : "0"
                    }
                },
                {
                    targets: 9,
                    data: "paymentAmount",
                    //className: "text-right",
                    render: function (data) {
                        return data ? Sv.format_number(data) : "0"
                    }
                },
                {
                    targets: 10,
                    data: "createdTime",
                    name: "createdTime",
                    render: function (createdTime) {
                        if (createdTime) {
                            return moment(new Date(createdTime)).format('DD/MM/YYYY HH:mm:ss');
                        }
                        return "";
                    }

                },
                // {
                //     targets: 10,
                //     data: "provider"
                // },
                // {
                //     targets: 10,
                //     data: "categoryCode"
                // },
                // {
                //     targets: 10,
                //     data: "productCode"
                // },
                {
                    targets: 11,
                    data: "partnerCode"
                },
                {
                    targets: 12,
                    data: "staffAccount"
                }
            ]
        });


        function getTables() {
            dataTable.ajax.reload();
        }

        $('#btnSearch').click(function (e) {
            e.preventDefault();
            getTables();
        });

        $('#TableFilter').on('keydown', function (e) {
            if (e.keyCode !== 13) {
                return;
            }

            e.preventDefault();
            getTables();
        });

        abp.event.on('app.createOrEditAgentModalSaved', function () {
            getTables();
        });

        $("#TableFilter").focus();


        $('#ExportToExcelButton').click(function () {
            abp.ui.setBusy();
            _transactionService.getTransactionHistoryUserToExcel({
                filter: $("#Filter").val(),
                serviceCode: $('#ServiceCodeFilter').val(),
                statusFilter: $("#StatusFilterId").val(),
                fromDate: getDateFilter($('#FromDateFilterId')),
                toDate: getDateFilter($('#ToDateFilterId'))
            })
                .done(function (result) {
                    abp.ui.clearBusy();
                    app.downloadTempFile(result);
                });
        });

        $(document).on('click', '.print-invoice-item', function() {
            let _type = $(this).attr('data-type');
            let _transcode = $(this).attr('data-trans-code');
            ngtAction.open(_transcode, 'print');
        });
    });
})();
