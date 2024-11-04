(function ($) {
    app.modals.ViewPinCodeModal = function () {
        const _transaction = abp.services.app.transactions;
        
        let _$providersTable = $('#DetailTable');
        let _modalManager;
        
        this.init = function (modalManager) {
            _modalManager = modalManager;

            let modal = _modalManager.getModal();
            const dataTable = _$providersTable.DataTable({
                paging: true,
                serverSide: true,
                processing: true,
                listAction: {
                    ajaxFunction: _transaction.getListTopupDetailRequest,
                    inputFilter: function () {
                        return {
                            transCode: $('input[name="TransCode"]').val(),
                        };
                    }
                },
                columnDefs: [
                    {
                        targets: 0,
                        data: "",
                        render: function (data, type, row, meta) {
                            return meta.row + 1;
                        }
                    },
                    {
                        targets: 1,
                        data: "cardValue",
                        render: function (data) {
                            return data ? Sv.format_number(data) : "0"
                        }
                    },
                    {
                        targets: 2,
                        data: "serial"
                    },
                    {
                        targets: 3,
                        data: "expiredDate",
                        render: function (data) {
                            return moment(data).format('L');
                        }
                    }
                ]
            });
        };
        
        $('#ExportToExcel').click(function() {
            _transaction.getListTopupDetailRequestToExcel({
                transCode: $('input[name="TransCode"]').val(),
            }).done(function (result) {
                app.downloadTempFile(result);
            });
        })
    };
})(jQuery);
