var ctrlList = {
    page: $("#Topup_Page"),
    form: $("#card__slider_menu2"),
    getTopupPrice: function (code) {
        var obj = ctrlList.getFormAddValue();
        Sv.Post({
            Url: abp.appPath + 'Topup/GetTopupPrice',
            Data: {catecode: code}
        }, function (rs) {
            // if (obj.cardPrice == undefined || obj.cardPrice == "")
            //     obj.cardPrice = "50000";
            ctrlList.form.find("#topupList-card-value-items").html(rs.result.content);
            ctrlList.form.find("[name='cardPrice'][data-amount='" + obj.cardPrice + "']").prop("checked", 'checked').trigger('change');
        }, function () {
            ctrlList.form.find("#topupList-card-value-items").html("");
        });
    },
    activeStep: function (index) {
        ctrlList.form.find('.step').hide();
        ctrlList.form.find('.step' + index).show();
    },
    getFormValue: function () {
        var dataList = [];
        ctrlList.tablePhoneNumber.rows().data().map(function(x, i){
            if(x.categoryCode !== undefined && x.categoryCode.length > 0){
                dataList.push({
                   id: x.id,
                   phoneNumber: x.phoneNumber,
                    cardPrice: x.cardPrice,
                    categoryCode: x.categoryCode,
                    categoryName: x.categoryName,
               });
            }
        });
        var obj = {
            isReadTerm: ctrlList.form.find("[name='isReadTerm']").is(":checked"),
            listNumbers: dataList,
        };
        return obj;
    },
    getFormAddValue: function () {
        return {
            phoneNumber: ctrlList.form.find("[name='phoneNumberList']").val(),
            cardPrice: ctrlList.form.find("[name='cardPrice']:checked").val(),
            productCode: ctrlList.form.find("[name='cardPrice']:checked").data("productcode"),
            categoryCode: ctrlList.form.find("[name='cardTelco']:checked").val(),
            categoryName: ctrlList.form.find("[name='cardTelco']:checked").data('servicename'),
        };
    },
    valid: function () {
        const obj = ctrlList.getFormValue();
        if (obj.listNumbers.length === 0) {
            abp.message.info("Vui lòng nhập danh sách số điện thoại cần nạp");
            return false;
        }
        if (!obj.isReadTerm) {
            abp.message.info("Vui lòng đồng ý chính sách của chúng tôi");
            return false;
        }
        return true;
    },
    // netx step 1
    nextToStep: function () {
        if (!ctrlList.valid()) {
            return false;
        }
        var obj = ctrlList.getFormValue();
        Sv.Post({
            Url: abp.appPath + 'Common/GetPayInfo',
            Data: {type: 'TOPUPLIST', model: obj}
        }, function (rs) {
            var ckBalance = Sv.checkBalance(rs.result );
            if (ckBalance) {
                ctrlList.form.find("#topupListInfoForm .section-body-p1").html(rs.result.content);
                ctrlList.activeStep(2);
            }
        }, function () {
            ctrlList.form.find("#topupListInfoForm .section-body-p1").html("");
        });
    },
    nextToStep2: function () {
        if (!ctrlList.valid()) {
            return false;
        }
        const obj = ctrlList.getFormValue();
        Dialog.verifyTransCode(Dialog.otpType.Payment, function () {
            Sv.Api({
                url: abp.appPath + 'api/services/app/Transactions/CreateTopupListRequest',
                data: (obj)
            }, function (rs) {
                window.location.href = rs.extraInfo;
            }, function (e) {
                abp.message.info(e);
                return false;
            });
        });
    },
    enterHandler: function () {
        ctrlList.form.find('input, select').on("keyup", function (event) {
            if (event.keyCode === 13) {
                event.preventDefault();
                ctrlList.nextToStep();
                return false;
            }
        });
    },

    // nhập sdt
    tablePhoneNumber: null,
    initTable: function () {
        ctrlList.tablePhoneNumber = ctrlList.form.find('table#phoneNumberTable').DataTable({
            paging: true,
            serverSide: false,
            columnDefs: [
                {
                    targets: 0,
                    render: function (data, type, row, settings) {
                        return settings.row + 1;
                    },
                },
                {
                    targets: 1,
                    data: "categoryName",
                },
                {
                    targets: 2,
                    data: "phoneNumber",
                },
                {
                    targets: 3,
                    data: "cardPrice",
                    render: function (data, type, row, settings) {
                        return Sv.NumberToString(data) +'đ';
                    },
                },
                {
                    targets: 4,
                    render: function (data, type, row) {
                        return '<button onclick="ctrlList.removePhoneNumber(\''+row.id+'\')" data-id="'+row.id+'">Xóa</button>';
                    },
                }
            ],
            fnRowCallback: function( nRow, aData, iDisplayIndex ) {
                if(aData.categoryCode ==="" || aData.categoryName==="Nhà mạng không hợp lệ" ){
                    $(nRow).addClass("red")
                }
                return nRow;
            }
        });
    },
    addTableData: function (dataList) {
        if(dataList && dataList.length > 0){
            var date = (new Date()).getTime();
            dataList.forEach(function(item, i){
                if(item.id === undefined || item.id === null){
                    item.id =  date + "_" + i;
                }
                ctrlList.tablePhoneNumber.row
                    .add(item)
                    .draw();
            });
        }
        ctrlList.btnActionHandler();
    },

    changePhoneNumberList: function () {
        ctrlList.form.find('input[name=phoneNumberList]').on("keyup", function (event) {
            let val = ctrlList.form.find("[name='phoneNumberList']").val();
            let valid = VietNamMobile.valid(val);
            if (valid.length > 0 ) {
                ctrlList.form.find('.button.btn-addPhone').prop('disabled', true);
            }else{
                ctrlList.form.find('.button.btn-addPhone').prop('disabled', false);
            }
        }).trigger('keyup');
    },

    addPhoneNumber: function () {
        const obj = ctrlList.getFormAddValue();
        if (obj.categoryCode === undefined || obj.categoryCode.length === 0) {
            abp.message.info("Vui lòng chọn nhà mạng");
            return false;
        }
        if (obj.cardPrice === undefined || obj.cardPrice.length === 0 ||  parseFloat(obj.cardPrice+"") <= 0) {
            abp.message.info("Vui lòng chọn mệnh giá");
            return false;
        }
        if (obj.phoneNumber.length === 0) {
            abp.message.info("Vui lòng nhập số điện thoại nạp tiền");
            return false;
        }
        let valid = VietNamMobile.valid(obj.phoneNumber);
        if(valid.length > 0){
            abp.message.info(valid);
            return false;
        }
        ctrlList.addTableData([obj]);
        ctrlList.form.find("[name='phoneNumberList']").val("").trigger('change').trigger('keyup');
    },

    removePhoneNumber: function (id) {
        var tr = ctrlList.form.find('[data-id="'+id+'"]').parents('tr');
        ctrlList.tablePhoneNumber.rows(tr)
            .remove()
            .draw();
        ctrlList.btnActionHandler();
    },
    btnActionHandler: function () {
        const obj = ctrlList.getFormValue();
        if(obj.listNumbers.length <= 0){
           ctrlList.form.find('.step1 .button.btn-custom.btn-blue').prop('disabled', true);
       }else{
           ctrlList.form.find('.step1 .button.btn-custom.btn-blue').prop('disabled', false);
       }
    },

    // nhap file
    eventInputFile:  function () {
        ctrlList.form.find('input[type=file][name=fileInput]').off('change').on('change',function(){
            var fileInput = ctrlList.form.find('input[type=file][name=fileInput]');
            var files = fileInput.get()[0].files;
            if (!files.length) {
                abp.message.info("Vui lòng chọn file nhập");
                return false;
            }
            var file = files[0];
             //File type check
            if (file.type.indexOf('spreadsheetml.sheet') < 0) {
                abp.message.info("File nhập không đúng định dạng");
                return false;
            }

            var formData = new FormData();
            formData.append("file", file);
            Sv.AjaxPostFile({
                Url: abp.appPath + "Topup/ImportTopupList",
                Data: formData
            }, function(response){
                ctrlList.addTableData(response.result.payload);
                //if()
            },function(){
                abp.message.error("Upload file lỗi!");
            });
        });
    },
};

$(document).ready(function () {
    ctrlList.enterHandler();
    // nhập sdt
    ctrlList.initTable();
    ctrlList.changePhoneNumberList();
    // nhap file
    ctrlList.eventInputFile();
    // default select telco
    ctrlList.form.find(".grid-telco li:first-child").find("input:first-child").prop("checked", 'checked').trigger('change');

    ctrlList.btnActionHandler();
});
