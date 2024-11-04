(function () {
    $(function () {
        var i = 1;
        var _$timeNow = new Date();
        var _$table = $("#Table");
        var _$transactionService = abp.services.app.transactions;
        var _$transcode = Sv.GetURLParameter("transcode");

        $("#date").text("Thời gian: " + moment(_$timeNow).format('DD/MM/YYYY HH:mm:ss'));
        var dataTable = _$table.DataTable({
            paging: true,
            serverSide: true,
            listAction: {
                ajaxFunction: _$transactionService.getTopupDetailRequest,
                inputFilter: function () {
                    return {
                        transCode: _$transcode
                    }
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
                    data: "cardValue",
                    name: "cardValue",
                    //className: "text-right",
                    render: function (data) {
                        return data ? Sv.format_number(data) : "0"
                    }
                },
                {
                    targets: 2,
                    data: "serial",
                    name: "serial"
                },
                {
                    targets: 3,
                    data: "cardCode",
                    name: "cardCode"
                },
                {
                    targets: 4,
                    data: "expiredDate",
                    name: "expiredDate",
                    render: function (expiredDate) {
                        if (expiredDate) {
                            return moment(new Date(expiredDate)).format('DD/MM/YYYY');
                        }
                        return "";
                    }
                },
                {
                    targets: 5, data: "discountAmount",
                    name: "discountAmount",
                    className: "text-right",
                    render: function (data, e, row) {
                        return data ? Sv.format_number(row.discountAmount / row.quantity) : "0"
                    }
                },
                {
                    targets: 6,
                    data: "itemAmount",
                    name: "itemAmount",
                    className: "all text-right",
                    render: function (data, e, row) {
                        //console.log(row);
                        return data ? Sv.format_number(row.paymentAmount / row.quantity) : "0"
                    }
                },
            ]
        });

    });

    function getTable() {
        dataTable.ajax.reload();
    }

    abp.event.on('app.createOrEditAgentModalSaved', function () {
        getTable();
    });

})();
