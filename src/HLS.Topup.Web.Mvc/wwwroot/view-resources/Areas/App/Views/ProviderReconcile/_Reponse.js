(function ($) {
    app.modals.ReponseModal = function () {
        var _$providersTable = $('#tblDetail');
        const _svcCompareService = abp.services.app.compare;
        var _modalManager;

        function getProviders() {
            dataTable.ajax.reload();
        }

        this.init = function (modalManager) {
            _modalManager = modalManager;
            var modal = _modalManager.getModal();
            //_dataTable = initTable();            
            modal.find(".select2").select2();
            modal.find('select#droTypeCompare').on('change', getProviders);

        };


        $('#ExportDetailToExcelButton').click(function (e) {
            e.preventDefault();
            _svcCompareService.getCompareReponseDetailListToExcel({
                transDate: $('#hdnTrandate').val(),
                providerCode: $('#hdnProvider').val(),
                compareType: $('#droTypeCompare').val(),
                keyCode: $('#hdnKeyCode').val()
            }).done(function (result) {
                app.downloadTempFile(result);
            });
        });

        const dataTable = _$providersTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _svcCompareService.getCompareReponseDetailList,
                inputFilter: function () {
                    return {
                        transDate: $('#hdnTrandate').val(),
                        providerCode: $('#hdnProvider').val(),
                        compareType: $('#droTypeCompare').val(),
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
                    render: function (transDate) {
                        if (transDate) {
                            return moment(transDate).format('L LTS');
                        }
                        return "";
                    }
                },
                {
                    targets: 2,
                    data: "agentCode"
                },
                {
                    targets: 3,
                    data: "transCode"
                },
                {
                    targets: 4,
                    data: "transPay"
                },
                {
                    targets: 5,
                    data: "productValue",
                    render: function (data) {
                        return Sv.format_number(data);
                    }
                },
                {
                    targets: 6,
                    data: "receivedAccount"
                },
                {
                    targets: 7,
                    data: "productCode"
                }
            ]
        });
    };
})(jQuery);

