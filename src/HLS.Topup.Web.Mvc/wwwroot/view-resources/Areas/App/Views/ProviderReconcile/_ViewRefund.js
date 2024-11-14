(function ($) {
    app.modals.RefundDetailModal = function () {
        var _$providersTable = $('#tableRefundDetail');
        const _svcCompareService = abp.services.app.compare;
        var _modalManager;

        this.init = function (modalManager) {
            _modalManager = modalManager;
            var modal = _modalManager.getModal();
            ///getProviders();
        };

        function getComapreDetial() {
            dataTable.ajax.reload();
        }

        const dataTable = _$providersTable.DataTable({
            paging: true,
            serverSide: false,
            processing: true,

            listAction: {
                ajaxFunction: _svcCompareService.getCompareRefundDetailList,
                inputFilter: function () {
                    return {
                        transDate: $('#hdnTrandate').val(),
                        providerCode: $('#hdnProvider').val(),
                        keyCode: $('#hdnKeyCode').val()
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
                    data: "transDate",
                    render: function (data, type, row) {
                        if (row.status === 0) {
                            return '<input type="checkbox" id="check_' + row.transCode + '" > <input type="text" style="display:none" id="hdn_' + row.transCode + '" value="' + row.agentCode + '|' + row.amount + '|' + row.status + '"/> ';
                        } else {
                            return '<input type="checkbox" id="checkRefund_' + row.transCode + '" disabled="disabled"> <input type="text" style="display:none" id="hdn_' + row.transCode + '" value="' + row.agentCode + '|' + row.amount + '|' + row.status + '"/> ';
                        }
                    },

                },
                {
                    targets: 2,
                    title: 'Thời gian GD',
                    data: "transDate",
                    render: function (transDate) {
                        if (transDate) {
                            return moment(transDate).format('L LTS');
                        }
                        return "";
                    }
                },
                {
                    targets: 3,
                    data: "agentCode",
                    title: 'Đại lý',
                },
                {
                    targets: 4,
                    data: "transCode",
                    title: 'Mã giao dịch',
                },
                {
                    targets: 5,
                    data: "transPay",
                    title: 'Mã NCC',
                },
                {
                    targets: 6,
                    data: "productValue",
                    title: 'Mệnh giá',
                    render: function (data) {
                        return Sv.format_number(data);
                    }
                },
                {
                    targets: 7,
                    data: "amount",
                    title: 'Thành tiền',
                    render: function (data) {
                        return Sv.format_number(data);
                    }
                },
                {
                    targets: 8,
                    title: 'Số thụ hưởng',
                    data: "receivedAccount"
                },
                {
                    targets: 9,
                    title: 'Loại sản phẩm',
                    data: "productCode"
                },
                {
                    targets: 10,
                    title: 'Tình trạng',
                    data: "statusName"
                },
                {
                    targets: 11,
                    title: 'Mã GD hoàn tiền',
                    data: "transCodeRefund"
                }
            ]
        });


        $('#btnRefund').click(function (e) {
            var refunds = [];
            $('input[type=checkbox]').each(function () {
                var id = $(this).attr("id");
                if (id.startsWith("check_") && id != "check_All") {
                    var res = id.split('_');
                    var check = $("#check_" + res[1]).prop('checked');
                    if (check) {
                        refunds.push(res[1]);
                    }
                }
            });

            var input = {};
            input.TransCodes = refunds;
            input.KeyCode = $('#hdnKeyCode').val();
            _modalManager.setBusy(true);
            _svcCompareService.refundAmoutSelectCompare(
                input
            ).done(function () {
                abp.message.info('Xác nhận hoàn tiền. Tiến trình đang được xử lý, vui lòng kiểm tra email để xem kết quả.');
                _modalManager.close();
                abp.event.trigger('app.refundCompareAirtime');

            }).always(function () {
                _modalManager.setBusy(false);
            });


        });


        $('#btnExportRefundDetial').click(function () {
            _svcCompareService.getCompareRefundDetailListToExcel({
                transDate: $('#hdnTrandate').val(),
                providerCode: $('#hdnProvider').val(),
                keyCode: $('#hdnKeyCode').val()
            })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#checkAll').click(function (e) {

            var isCheck = $("#checkAll").prop('checked');

            $('input[type=checkbox]').each(function () {
                var id = $(this).attr("id");
                if (id.startsWith("check_") && id != "checkAll") {
                    if (isCheck) {

                        if (!$(this).prop('checked'))
                            $(this).prop('checked', true);
                    } else {

                        if ($(this).prop('checked')) {
                            $(this).prop('checked', false);
                        }
                    }
                }
            });
        });
    };
})(jQuery);

