(function ($) {
    app.modals.CreateOrEditModal = function () {
        var _svcCompareService = abp.services.app.compare;
        var _modalManager;
        var _dataTable;

        this.init = function (modalManager) {
            _modalManager = modalManager;
            var modal = _modalManager.getModal();
            _dataTable = initTable();
            // modal.find(".select2").select2();         

            $('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

        };


        $('#btnLoadData').click(function () {
            uploadImportFile();
        });

        $('#btnCompare').click(function () {
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
                        data: "compareType",
                    },
                    {
                        targets: 1,
                        data: "quantity",
                        render: function (data) {
                            return Sv.format_number(data);
                        }
                    },
                    {
                        targets: 2,
                        data: "amount",
                        render: function (data) {
                            return Sv.format_number(data);
                        }
                    },
                    {
                        targets: 3,
                        data: "quantityRefund",
                        class: "text-right",
                        render: function (data, e, row) {
                            return Sv.NumberToString(data);
                        }
                    },
                    {
                        targets: 4,
                        class: "text-right",
                        data: "amountRefund",
                        render: function (data, e, row) {
                            return Sv.NumberToString(data);
                        }
                    }
                ]
            });
        }

        //function resetValueFile() {
        //    document.getElementById('importCardsFromExcel').value = "";
        //}   

        function uploadImportFile() {
            // let $f = _modalManager.getModal().find('input#ProviderData');
            var fileInput = document.getElementById('ProviderData');
            var formData = new FormData();
            //let file_0 = null;
            //let file_1 = null;
            //let file_2 = null;
            //Iterating through each files selected in fileInput  
            for (i = 0; i < fileInput.files.length; i++) {
                var files = fileInput.files[i];
                formData.append("file", files);
                //var file = files[0];
                //if (i == 0)
                //    file_0 = files;
                //else if (i == 1)
                //    file_1 = files;
                //else if (i == 2)
                //    file_2 = files;

                //if (fileInput != null && fileInput.length > 0) {
                //    //file = files[0];
                //    //File type check
                //    let type = file.type.slice(file.type.lastIndexOf('/') + 1);
                //    let typeAlow = ['xlsx', 'xls', 'csv'];
                //    if (typeAlow.indexOf(type) > -1) {
                //        abp.message.warn("Định dạng file không hợp lệ");
                //        // resetValueFile();
                //        return false;
                //    }
                //    //File size check
                //    if (file.size > 1048576 * 100) //100 MB
                //    {
                //        abp.message.warn("Dung lượng file vượt quá giới hạn");
                //        // resetValueFile();
                //        return false;
                //    }


                //}                
            }

            //formData.append("file", file_0);
            //formData.append("file", file_1);
            //formData.append("file", file_2);
            let $fNhattran = _modalManager.getModal().find('input#NTData');
            let fileNT = $fNhattran[0].files[0];

            //  $f.closest('label').find('span').html(file.name);
            var providerCode = $("#providerSelect").val();
            var transDate = $("#TransDate").val();

            formData.append("file", fileNT);
            formData.append("providerCode", providerCode);
            formData.append("transDate", transDate);
            _modalManager.setBusy(true);
            Sv.AjaxPostFile2({
                url: abp.appPath + "App/ProviderReconcile/ReadFileCompare",
                data: formData
            }).then(function (response) {

                let data = [];
                _dataTable.rows().remove();
                //   .draw();
                if (response.result.responseCode == "1") {
                    data = response.result.payload;
                    data.forEach(function (v) { delete v.discount; });
                    _dataTable.rows.add(data).draw();
                }
                else {
                    // resetValueFile();
                    abp.message.warn(response.result.responseMessage);
                }
            }).always(function () {
                _modalManager.setBusy(false);
            });

            //} else {
            //    _dataTable.rows.add([]).draw();
            //    $f.closest('label').find('span').html('Chọn file');
            //}
        }



        function CompareImportFile() {
            var fileInput = document.getElementById('ProviderData');
            var formData = new FormData();            
            //Iterating through each files selected in fileInput  
            for (i = 0; i < fileInput.files.length; i++) {
                var files = fileInput.files[i];
                formData.append("file", files);                               
            }

            let $fNhattran = _modalManager.getModal().find('input#NTData');
            let fileNT = $fNhattran[0].files[0];

            //  $f.closest('label').find('span').html(file.name);
            var providerCode = $("#providerSelect").val();
            var transDate = $("#TransDate").val();

            formData.append("file", fileNT);
            formData.append("providerCode", providerCode);
            formData.append("transDate", transDate);
                _modalManager.setBusy(true);
                Sv.AjaxPostFile2({
                    url: abp.appPath + "App/ProviderReconcile/ImportFileCompareFromJobExcel",
                    data: formData
                }).then(function (response) {
                    _modalManager.setBusy(false);
                    if (response.result.responseCode === "1") {
                        abp.message.info('Xác nhận đối soát. Tiến trình đang được xử lý, vui lòng kiểm tra email để xem kết quả.');
                        _modalManager.close();
                        abp.event.trigger('app.compareAirtimeFile');
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

