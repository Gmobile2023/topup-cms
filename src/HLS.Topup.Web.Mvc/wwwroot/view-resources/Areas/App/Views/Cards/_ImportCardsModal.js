(function ($) {
    app.modals.ImportCardsModal = function () {

        var _cardsService = abp.services.app.cards;
        var _modalManager;
        var _dataTable;
        
        this.init = function (modalManager) {
            _modalManager = modalManager;
            var modal = _modalManager.getModal();
            modal.find('input#importCardsFromExcel').on('change', uploadFileImportCards);

            _dataTable = initTable();
            discountChange();
            modal.find(".select2").select2();
            // modal.find('.date-picker').datetimepicker({
            //     locale: abp.localization.currentLanguage.name,
            //     format: 'L'
            // });
            //_$cardInformationForm = _modalManager.getModal().find('form[name=ImportCardInformationsForm]');
            // _$cardInformationForm.validate();
            // modal.find(".select2").select2();
            // $("#importCardsFromExcel").fileupload(fileImport)
            //     .prop('disabled', !$.support.fileInput)
            //     .parent().addClass($.support.fileInput ? undefined : 'disabled');
            //$('#importCardsFromExcel').bind('fileuploadsubmit', fileuploadsubmit);
        };
        
        function initTable(){
            let $table = _modalManager.getModal().find('#ProductFileTable');
            return $table.DataTable({
                paging: true,
                serverSide: false,
                ordering: false,
                data: [],
                columnDefs: [ 
                    {
                        targets: 0,
                        data: "serviceCode",
                    },
                    {
                        targets: 1,
                        data: "categoryName",
                    },
                    {
                        targets: 2,
                        data: "productName",
                    },
                    {
                        targets: 3,
                        data: "cardValue",
                        class : "text-right",
                        render: function (data, e, row) {
                            return Sv.NumberToString(data) ;
                        }
                    },
                    {
                        targets: 4,
                        class : "text-right",
                        data: "quantity",
                        render: function (data, e, row) {
                            return Sv.NumberToString(data) ;
                        }
                    }, 
                    {
                        targets: 5,
                        class : "text-right", 
                        data: "discount",
                        render: function (data, e, row) { 
                            return "<input style='width:100px' type='text' name='discount' value='"+data+"' class='form-control percentage-mask' />"
                                
                        }
                    },
                    {
                        targets: 6,
                        class : "text-right",
                        data: "amount",
                        render: function (data, e, row) {
                            if(data && data > 0)
                                return Sv.NumberToString(data) + "đ" ;
                            return 0;
                        }
                    }
                ],
                drawCallback: function( settings) {
                    var data = _dataTable.data();
                    console.log(data)
                    var q = 0;
                    var t = 0;
                    if(data.length > 0){
                        q = data.reduce((a, b) => parseFloat(a) + parseFloat(b["quantity"] || 0), 0);
                        t = data.reduce((a, b) => parseFloat(a) + parseFloat(b["amount"] || 0), 0);
                    }
                    $table.find("tbody.f tr td:nth-child(2)").html(Sv.NumberToString(q));
                    $table.find("tbody.f tr td:nth-child(4)").html(Sv.NumberToString(t) + "đ");
                }
            });
        } 
        function uploadFileImportCards() {
            let $f = _modalManager.getModal().find('input#importCardsFromExcel');
            let files = $f[0].files;
            if (files && files.length > 0) {
                let file = files[0];
                //File type check
                let type = file.type.slice(file.type.lastIndexOf('/') + 1);
                let typeAlow = ['xlsx', 'xls', 'csv'];
                if (typeAlow.indexOf(type) > -1) {
                    abp.message.warn("Định dạng file không hợp lệ");
                    resetValueFile();
                    return false;
                }
                //File size check
                if (file.size > 1048576 * 100) //100 MB
                {
                    abp.message.warn("Dung lượng file vượt quá giới hạn");
                    resetValueFile();
                    return false;
                } 
                $f.closest('label').find('span').html(file.name);

                var formData = new FormData();
                formData.append("file", file);
                _modalManager.setBusy(true);
                Sv.AjaxPostFile2({
                    url: abp.appPath + "App/Cards/ReadFileImport",
                    data: formData
                }).then(function(response){
                    _dataTable.clear();
                    let data = [];
                    let path = "";
                    if(response.result.responseCode == "01"){
                        data = response.result.payload;
                        data.forEach(function(v){ delete v.discount; });
                    }             
                    _dataTable.rows.add(data).draw();
                    Sv.SetupAmountMask();
                    if(response.result.responseCode != "01"){
                        resetValueFile();
                        abp.message.warn(response.result.responseMessage);
                    }
                }).always(function () {
                    _modalManager.setBusy(false);
                });
                
            } else {
                _dataTable.rows.add([]).draw();
                $f.closest('label').find('span').html('Chọn file');
            }
        }
        function resetValueFile(){
            document.getElementById('importCardsFromExcel').value = "";
        }
        function discountChange(){
            let $f = _modalManager.getModal().find('#ProductFileTable');
            $f.on('change', '[name=discount]',function(e){
                let $e = $(e.target);
                let discount = $e.val();
                let tr = _dataTable.row($e.closest('tr'));
                let index = tr.index();
                let data = _dataTable.data();  
                for (var i in data) { 
                    if (i == index) {
                        data[i].discount = (discount+"");
                        let total = parseFloat(data[i].cardValue+"") * parseFloat(data[i].quantity+"");
                        data[i].amount = total - total * parseFloat(discount+"")/100; 
                        break;
                    }
                }
                _dataTable.clear();
                _dataTable.rows.add(data).draw();
                Sv.SetupAmountMask();
            })
        }
        this.save = function () {
            let data = [];
            let checkDiscount = true;
            _dataTable.data().map(function(x, i){
                if(x.discount == null || x.discount < 0){
                    checkDiscount = false;
                }
                data.push(x);
            });
            if(checkDiscount == false){
                abp.message.warn("Vui lòng nhập dữ liệu chiết khấu!"); 
                return false;
            }
            
            let formData = new FormData();
            let $f = _modalManager.getModal().find('input#importCardsFromExcel'); 
            if($f[0].files.length == 0){
                abp.message.warn("Vui lòng chọn file nhập thẻ!");
                resetValueFile();
                return false;
            }
            if(data.length == 0){
                abp.message.warn("Dữ liệu thẻ nhận không chính xác, vui lòng thử lại!");
                resetValueFile();
                return false;
            }
            
            let _$importCardInformationsForm = _modalManager.getModal().find('form[name=ImportCardInformationsForm]');
            var obj = _$importCardInformationsForm.serializeFormToObject();
            if(obj.providerCode == null || obj.providerCode.length == 0){
                abp.message.warn("Vui lòng chọn nhà cung cấp thẻ!"); 
                return false;
            }
            formData.append("file", $f[0].files[0]); 
            formData.append("data", JSON.stringify(data));
            formData.append("description", obj.description);
            formData.append("providerCode", obj.providerCode);
            
            _modalManager.setBusy(true);
            Sv.AjaxPostFile2({
                url: abp.appPath + "App/Cards/ImportCardsFromJobExcel",
                data: formData
            }).then(function(response){
                _modalManager.setBusy(false);
                if (response.result.responseCode === "01") {
                    abp.message.info('Xác nhận nhập thẻ từ file!. Tiến trình đang xử lý. Vui lòng chờ thông báo kết quả');
                    _modalManager.close();
                    abp.event.trigger('app.importCardsFileSaved');
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
