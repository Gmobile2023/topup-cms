(function ($) {
    app.modals.ViewPayBatchBillModal = function () {

        var _payBatchBillsService = abp.services.app.payBatchBills;
        var _$dataTable = $('#dataTablePayBatchDetail');
        var _modalManager;
        this.init = function (modalManager) {
            _modalManager = modalManager;           
        };


        const dataTable = _$dataTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _payBatchBillsService.getPayBatchBillDetail,
                inputFilter: function () {
                    return {
                        id: $('#idPayBatchBill').val()
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
                    targets: 1,
                    data: "agentCode"
                },
                {
                    targets: 2,
                    data: "mobile"
                },
                {
                    targets: 3,
                    data: "fullName"
                },
                {
                    targets: 4,
                    data: "quantity",
                    className: "all text-right",
                    render: function (data) {
                        return Sv.format_number(data);
                    }
                },
                {
                    targets: 5,
                    data: "payBatchMoney",
                    className: "all text-right",
                    render: function (data) {
                        return Sv.format_number(data);
                    }
                },
                {
                    targets: 6,
                    data: "statusName",
                    className: "all text-right"
                },
                {
                    targets: 7,
                    data: "transRef"
                },
            ],
            drawCallback: function (settings) {
                var rawServerResponse = this.api().settings()[0].rawServerResponse;
                if (rawServerResponse !== undefined && rawServerResponse.totalData !== undefined) {                
                    _$dataTable.find("tbody.f tr td:nth-child(2)").html(Sv.NumberToString(rawServerResponse.totalData.payBatchMoney));
                }                             
            }
        });

        $('#ViewExportDetail').click(function () {
            _payBatchBillsService
                .getPayBatchDetailToExcel({
                    id: $('#idPayBatchBill').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });
      
        $('#ApprovalPayBatchBillButton').click(function () {
            var id = $('#idPayBatchBill').val();
            approvalBatchBill(id);
        });

        function approvalBatchBill(id) {
            abp.message.confirm(
                'Bạn có chắc chắn muốn duyệt danh sách trả phí thanh toán hóa đơn này không?',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _payBatchBillsService.confirmApproval(id).done(function () {
                            abp.message.success('Thành công');
                           // dataTable.ajax.reload();
                            $('#PayBatchBillsTable').DataTable().ajax.reload();
                            abp.event.trigger('app.createOrEditPayBatchBillModalSaved');
                            _modalManager.close();
                        });
                    }
                }
            );
        }
    };
})(jQuery);