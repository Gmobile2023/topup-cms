(function () {
    $(function () {                     
        var _$table = $('#tableDetail');
        var _transService = abp.services.app.transactions;
        var dataTable = _$table.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _transService.getBatchLotDetailList,
                inputFilter: function () {
                    return {
                        batchCode: $('#hdnBatchCode').val(),
                        batchStatus: $('#selectBatchStatus').val(),
                        status: $('#selectStatusTrans').val(),
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
                    className: 'all',
                    data: "batchName"                                      
                },
                {
                    targets: 2,
                    className: 'all',
                    data: "productName"                   
                },
                {
                    targets: 3,
                    className: 'all',
                    data: "provider",                   
                },
                {
                    targets: 4,
                    className: 'all',
                    data: "receiverInfo",
                },
                {
                    targets: 5,
                    className: "all text-right",
                    data: "amount",
                    name: "amount",
                    render: function (data, dispaly, row) {
                        return (data ? Sv.format_number(data) : "0");
                    }
                },               
                {
                    targets: 6,
                    className: "all text-right",
                    data: "discountAmount",
                    render: function (data, dispaly, row) {                       
                            return (data ? Sv.format_number(data) : "0");
                    }
                },
                {
                    targets: 7,
                    className: "all text-right",
                    data: "fee",
                    render: function (data, dispaly, row) {
                        return (data ? Sv.format_number(data) : "0");
                    }
                },
                {
                    targets: 8,
                    className: "all text-right",
                    data: "paymentAmount",
                    render: function (data, dispaly, row) {                        
                            return (data ? Sv.format_number(data) : "0");
                    }
                },
                {
                    targets: 9,
                    className: 'all',
                    data: "statusName"
                },               
                {
                    targets: 10,
                    data: "updateTime",
                    name: "updateTime",
                    render: function (createdTime, dispaly, row) {
                        if (row.status === 3)
                            return "<g style='color: red'>" + (moment(new Date(createdTime)).format('DD/MM/YYYY HH:mm:ss')) + "</g>";
                        if (row.status === 2)
                            return "<g style='color: brown'>" + (moment(new Date(createdTime)).format('DD/MM/YYYY HH:mm:ss')) + "</g>";
                        else
                            return moment(new Date(createdTime)).format('DD/MM/YYYY HH:mm:ss');
                    }
                }
            ],
            "headerCallback": function (thead, data, start, end, display) {
                try {
                    var rawServerResponse = this.api().settings()[0].rawServerResponse;
                    if (rawServerResponse !== undefined && rawServerResponse.totalData !== undefined) {
                        $(thead).find('th').eq(1).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.amount));
                        $(thead).find('th').eq(2).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.discountAmount));
                        $(thead).find('th').eq(3).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.fee));
                        $(thead).find('th').eq(4).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.paymentAmount));   ;
                    }
                } catch (e) {
                    console.log("không có total")
                }
            }
        });


        function getTables() {
            dataTable.ajax.reload();
        }



        $('#btnSeachDetail').click(function (e) {
            e.preventDefault();
            getTables();
        });

        $('#btnUndo').click(function (e) {
            window.location.href = "/BatchTopup/History";
        });

        $('#TableFilter').on('keydown', function (e) {
            if (e.keyCode !== 13) {
                return;
            }

            e.preventDefault();
            getTables();
        });


        $("#TableFilter").focus();


        $('#btnExportDetail').click(function () {
            _transService.getBatchLotBillDetailExportToFile({
                batchCode: $('#hdnBatchCode').val(),
                batchStatus: $('#selectBatchStatus').val(),
                status: $('#selectStatusTrans').val(),
            })
                .done(function (result) {                  
                    app.downloadTempFile(result);
                });
        });
     

        $(document).on('click', '.print-invoice-item', function () {
            let _type = $(this).attr('data-type');
            let _transcode = $(this).attr('data-trans-code');
            ngtAction.open(_transcode, 'print');
        });
    });
})();
