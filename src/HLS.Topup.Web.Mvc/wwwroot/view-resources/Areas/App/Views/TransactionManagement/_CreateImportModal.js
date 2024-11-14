(function ($) {
    app.modals.CreateImportModal = function () {
        var _svcCompareService = abp.services.app.compare;
        var _modalManager;
        var _dataTable;

        this.init = function (modalManager) {
            _modalManager = modalManager;
            var modal = _modalManager.getModal();
            _dataTable = initTable();
            $('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });
        };

        $('#btnLoadData').click(function () {
            uploadImportFile();
        });

        $('#btnProcessData').click(function () {
            CompareImportFile();
        });

        function initTable() {
            let $table = _modalManager.getModal().find('#tableViewData');
            return $table.DataTable({
                paging: true,
                serverSide: false,
                ordering: false,
                data: [],
                columnDefs: [
                    {
                        targets: 0,
                        data: "transCode"
                    },
                    {
                        targets: 1,
                        data: "statusName"
                    }
                ]
            });
        }

        function uploadImportFile() {
            var formData = new FormData();
            let $fNhattran = _modalManager.getModal().find('input#importFileTransStatus');
            let fileNT = $fNhattran[0].files[0];
            formData.append("file", fileNT);
            _modalManager.setBusy(true);
            Sv.AjaxPostFile2({
                url: abp.appPath + "App/TransactionManagement/ReadFileData",
                data: formData
            }).then(function (response) {

                let data = [];
                _dataTable.rows().remove();
                if (response.result.responseCode == "01") {
                    data = response.result.payload;
                    data.forEach(function (v) {
                        delete v.discount;
                    });
                    _dataTable.rows.add(data).draw();
                } else {
                    abp.message.warn(response.result.responseMessage);
                }
            }).always(function () {
                _modalManager.setBusy(false);
            });
        }

        function CompareImportFile() {
            var formData = new FormData();
            let $fNhattran = _modalManager.getModal().find('input#importFileTransStatus');
            let fileNT = $fNhattran[0].files[0];
            formData.append("file", fileNT);
            _modalManager.setBusy(true);
            Sv.AjaxPostFile2({
                url: abp.appPath + "App/TransactionManagement/ProcessFromJobExcel",
                data: formData
            }).then(function (response) {
                _modalManager.setBusy(false);
                if (response.result.responseCode === "01") {
                    abp.message.info("Chuyển đổi trạng thái giao dịch bằng file đang được xử lý....");
                    _modalManager.close();
                } else {
                    abp.message.warn(response.result.responseMessage);
                }
                console.log(response);
            }).always(function () {
                _modalManager.setBusy(false);
            });
        }
    };
})(jQuery);

