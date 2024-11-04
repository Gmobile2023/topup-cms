var ctrlList = {
    page: $("#Topup_Page"),
    formTopup: $("#input-topUp-box"),
    formTopupConfirm: $("#input_topUp_box_confirm"),
    formPayBill: $("#input-payBill-box"),
    formPayBillConfirm: $("#input_payBill_box_confirm"),
    formPinCode: $("#input-pinCode-box"),
    formPinCodeConfirm: $("#input_pinCode_box_confirm"),

    topLabel: [
        {
            "title": "Nạp lô",
            "icon": "/themes/topup/images/ic_banner_top/ic-01-xxx-white.svg"
        },
        {
            "title": "Mua mã thẻ",
            "icon": "/themes/topup/images/ic_banner_top/05.svg"
        },
        {
            "title": "Thanh toán hóa đơn",
            "icon": "/themes/topup/images/ic_banner_top/03.svg"
        }
    ],

    changeLabelHeader: function (index) {
        let label = ctrlList.topLabel[index];
        let element = $('.force-change-label');
        element.find('img').attr('src', label.icon);
        element.find('span').text(label.title);
    },

    tabBatchType: function (index) {
        $('#hdnBatchType').val(index);
        ctrlList.viewStepNext();
    },

    undoMenu: function () {
        var type = $('#hdnBatchType').val();
        $('#topup_PageList').show();
        $('#topup_PageConfirm').hide();
        $('#input_topUp_box_confirm').hide();
        $('#input_pinCode_box_confirm').hide();
        $('#input_payBill_box_confirm').hide();
        if (type == "TOPUP")
            $('#input_topUp_box').show();
        else if (type == "PINCODE")
            $('#input_pinCode_box').show();
        else if (type == "PAYBILL")
            $('#input_payBill_box').show();
        ctrlList.viewStepNext();
    },

    getFormTopupValue: function () {
        var obj = [];
        ctrlList.topupTable.rows().data().map(function (x, i) {
            {
                obj.push({
                    id: x.id,
                    receiverInfo: x.receiverInfo,
                    value: x.value,
                    productCode: x.productCode,
                    productName: x.productName,
                    categoryCode: x.categoryCode,
                    categoryName: x.categoryName,
                    serviceName: x.serviceName,
                    serviceCode: x.serviceCode,
                });
            }
        });
        return obj;
    },

    getFormTopupConfirmValue: function () {
        var obj = [];
        ctrlList.topupTableConfirm.rows().data().map(function (x, i) {
            {
                obj.push({
                    id: x.id,
                    receiverInfo: x.receiverInfo,
                    value: x.value,
                    discount: x.discount,
                    price: x.price,
                    quantity: 1,
                    productCode: x.productCode,
                    productName: x.productName,
                    categoryCode: x.categoryCode,
                    categoryName: x.categoryName,
                    serviceName: x.serviceName,
                    serviceCode: x.serviceCode,
                });
            }
        });

        return obj;
    },

    getFormPayBillValue: function () {
        var obj = [];
        ctrlList.payBillTable.rows().data().map(function (x, i) {
            {
                obj.push({
                    id: x.id,
                    productCode: x.productCode,
                    productName: x.productName,
                    categoryCode: x.categoryCode,
                    categoryName: x.categoryName,
                    serviceName: x.serviceName,
                    serviceCode: x.serviceCode,
                    receiverInfo: x.receiverInfo,
                    value: x.value,
                    isPayBill: "1"
                });
            }
        });
        return obj;
    },

    getFormPayBillConfirmValue: function () {
        var obj = [];
        ctrlList.payBillTableConfirm.rows().data().map(function (x, i) {

            if (x.value !== 0) {
                obj.push({
                    id: x.id,
                    value: x.value,
                    discount: x.discount,
                    price: x.price,
                    fee: x.fee,
                    quantity: 1,
                    receiverInfo: x.receiverInfo,
                    serviceName: x.serviceName,
                    serviceCode: x.serviceCode,
                    categoryCode: x.categoryCode,
                    categoryName: x.categoryName,
                    productCode: x.productCode,
                    productName: x.productName,
                    isPayBill: "1",
                });
            }

        });

        return obj;
    },

    getFormPinCodeValue: function () {
        var obj = [];
        ctrlList.pinCodeTable.rows().data().map(function (x, i) {
            {
                obj.push({
                    id: x.id,
                    value: x.value,
                    quantity: x.quantity,
                    productCode: x.productCode,
                    productName: x.productName,
                    categoryCode: x.categoryCode,
                    categoryName: x.categoryName,
                    serviceName: x.serviceName,
                    serviceCode: x.serviceCode,
                });
            }
        });
        return obj;
    },

    getFormPinCodeConfirmValue: function () {
        var obj = [];
        ctrlList.pinCodeTableConfirm.rows().data().map(function (x, i) {
            {
                obj.push({
                    id: x.id,
                    value: x.value,
                    discount: x.discount,
                    price: x.price,
                    quantity: x.quantity,
                    productCode: x.productCode,
                    productName: x.productName,
                    categoryCode: x.categoryCode,
                    categoryName: x.categoryName,
                    serviceCode: x.serviceCode,
                    serviceName: x.serviceName,
                });
            }
        });

        return obj;
    },

    validTopup: function () {
        const obj = ctrlList.getFormTopupValue();
        return true;
    },

    validPayBill: function () {
        const obj = ctrlList.getFormPayBillValue();
        //if (obj.listNumbers.length === 0) {
        //    abp.message.info("Vui lòng nhập danh sách số điện thoại cần nạp");
        //    return false;
        //}
        //if (!obj.isReadTerm) {
        //    abp.message.info("Vui lòng đồng ý chính sách của chúng tôi");
        //    return false;
        //}
        return true;
    },

    validPinCode: function () {
        const obj = ctrlList.getFormPinCodeValue();
        //if (obj.listNumbers.length === 0) {
        //    abp.message.info("Vui lòng nhập danh sách số điện thoại cần nạp");
        //    return false;
        //}
        //if (!obj.isReadTerm) {
        //    abp.message.info("Vui lòng đồng ý chính sách của chúng tôi");
        //    return false;
        //}
        return true;
    },

    viewStepNext: function () {
        var type = $('#hdnBatchType').val();
        var length = 0;
        if (type == "TOPUP") {
            var obj = ctrlList.getFormTopupValue();
            length = obj.length;
            if (length <= 0) {
                $('#btnNextTopup').hide();
            } else {
                $('#btnNextTopup').show();
            }
        }
        else if (type == "PINCODE") {
            var obj = ctrlList.getFormPinCodeValue();
            length = obj.length;
            if (length <= 0) {
                $('#btnNextPinCode').hide();
            } else {
                $('#btnNextPinCode').show();
            }
        }
        else if (type == "PAYBILL") {
            var obj = ctrlList.getFormPayBillValue();
            length = obj.length;
            if (length <= 0) {
                $('#btnNextPayBill').hide();
            } else {
                $('#btnNextPayBill').show();
            }
        }
    },

    // netx step 1
    nextToStep: function () {

        var type = $('#hdnBatchType').val();
        $('#topup_PageList').hide();
        $('#topup_PageConfirm').show();
        if (type == "TOPUP") {
            $('#input_topUp_box_confirm').show();
            var dto = ctrlList.getFormTopupValue();
            Sv.Post({
                Url: abp.appPath + "BatchTopup/QueryPriceTopupList",
                Data: { query: dto }
            }, function (response) {
                ctrlList.addTableDataTopupConfirm(response.result.payload);
            }, function () {
                abp.message.error("Chính giá bị lỗi!");
            });
        }
        else if (type == "PINCODE") {
            $('#input_pinCode_box_confirm').show();
            var dto1 = ctrlList.getFormPinCodeValue();
            Sv.Post({
                Url: abp.appPath + "BatchTopup/QueryPriceTopupList",
                Data: { query: dto1 }
            }, function (response) {
                ctrlList.addTableDataPinCodeConfirm(response.result.payload);
            }, function () {
                abp.message.error("Chính sách giá lỗi!");
            });
        }
        else if (type == "PAYBILL") {
            $('#input_payBill_box_confirm').show();
            var dto2 = ctrlList.getFormPayBillValue();
            
            Sv.Post({
                Url: abp.appPath + "BatchTopup/QueryPriceTopupList",
                Data: { query: dto2 }
            }, function (response) {
                ctrlList.addTableDataPayBillConfirm(response.result.payload);
            }, function () {
                abp.message.error("Chính xác giá lỗi!");
            });
        }
    },

    nextToStepConfirm: function () {
        //if (!ctrlList.valid()) {
        //    return false;
        //}

        var batchType = $('#hdnBatchType').val();
        var obj = {};
        if (batchType == "TOPUP")
            obj = ctrlList.getFormTopupConfirmValue();
        else if (batchType == "PINCODE")
            obj = ctrlList.getFormPinCodeConfirmValue();
        else if (batchType == "PAYBILL")
            obj = ctrlList.getFormPayBillConfirmValue();

        var value = 0;
        var price = 0;
        obj.forEach(function (item, i) {
            value = value + parseFloat(item.value);
            price = price + parseFloat(item.price);
        });

        var _data = {};
        _data.BatchType = batchType;
        _data.ListNumbers = obj;
        Sv.checkUserTransValid("", "", "", price, value, 1)
            .then(function (rs) {
                Dialog.verifyTransCode(Dialog.otpType.Payment, function () {
                    Sv.Api({
                        url: abp.appPath + 'api/services/app/Transactions/CreateTopupListRequest',
                        data: (_data)
                    }, function (rs) {
                        window.location.href = rs.extraInfo;
                    }, function (e) {
                        abp.message.info(e);
                        return false;
                    });
                });
            });
    },
    
    topupTable: null,
    topupTableConfirm: null,
    initTopupTable: function () {
        ctrlList.topupTable = ctrlList.formTopup.find('table#topupTable').DataTable({
            paging: true,
            serverSide: false,
            responsive: true,
            scrollX: true,
            fixedHeader: true,
            columnDefs: [
                {
                    targets: 0,
                    className: 'all text-center',
                    render: function (data, type, row, settings) {
                        return settings.row + 1;
                    },
                },
                {
                    targets: 1,
                    data: "receiverInfo",
                    className: 'all text-center',
                },
                {
                    targets: 2,
                    data: "categoryName",
                    className: 'all text-center',
                },
                {
                    targets: 3,
                    data: "value",
                    className: 'all text-center',
                    render: function (data, type, row, settings) {
                        return Sv.NumberToString(data);
                    },
                },
                {
                    targets: 4,
                    className: 'all text-center',
                    render: function (data, type, row) {
                        return '<button onclick="ctrlList.removeItemTopup(\'' + row.id + '\')" data-id="' + row.id + '">Xóa</button>';
                    },
                }
            ],
            fnRowCallback: function (nRow, aData, iDisplayIndex) {
                if (aData.categoryCode === "" || aData.categoryName === "Nhà mạng không hợp lệ") {
                    $(nRow).addClass("red")
                }
                return nRow;
            }
        });
    },
    initTopupTableConfirm: function () {
        ctrlList.topupTableConfirm = ctrlList.formTopupConfirm.find('table#topupTableConfirm').DataTable({
            paging: true,
            serverSide: false,
            responsive: true,
            scrollX: true,
            columnDefs: [
                {
                    targets: 0,
                    className: 'all text-center',
                    render: function (data, type, row, settings) {
                        return settings.row + 1;
                    },
                },
                {
                    targets: 1,
                    data: "receiverInfo",
                    className: 'all text-center',
                },
                {
                    targets: 2,
                    data: "categoryName",
                    className: 'all text-center',
                },
                {
                    targets: 3,
                    data: "value",
                    className: 'all text-center',
                    render: function (data, type, row, settings) {
                        return Sv.NumberToString(data);
                    },
                },
                {
                    targets: 4,
                    data: "discount",
                    className: 'all text-center',
                    render: function (data, type, row, settings) {
                        return Sv.NumberToString(data);
                    },
                },
                {
                    targets: 5,
                    data: "price",
                    className: 'all text-center',
                    render: function (data, type, row, settings) {
                        return Sv.NumberToString(data);
                    },
                }
            ]
        });
    },
    addTableDataTopup: function (dataList) {

        ctrlList.topupTable.rows().remove();
        if (dataList && dataList.length > 0) {
            var t = 0;
            var date = (new Date()).getTime();
            dataList.forEach(function (item, i) {
                if (item.id === undefined || item.id === null) {
                    item.id = date + "_" + i;
                }
                ctrlList.topupTable.row
                    .add(item)
                    .draw();
                t = t + item.value;
            });

            $("#sumTopupTable").text(Sv.NumberToString(t));
        }
        ctrlList.viewStepNext();
    },
    addTableDataTopupConfirm: function (dataList) {

        ctrlList.topupTableConfirm.rows().remove();
        if (dataList && dataList.length > 0) {
            var v = 0;
            var d = 0;
            var s = 0;
            var q = 0;
            var date = (new Date()).getTime();
            dataList.forEach(function (item, i) {
                if (item.id === undefined || item.id === null) {
                    item.id = date + "_" + i;
                }
                ctrlList.topupTableConfirm.row
                    .add(item)
                    .draw();
                s = s + item.price;
                v = v + item.value;
                d = d + item.discount;
                q = q + 1;
            });

            $("#sumTopupQty").text(Sv.NumberToString(q));
            $("#sumTopupPrice").text(Sv.NumberToString(s));
            $("#sumTopupTableValue").text(Sv.NumberToString(v));
            $("#sumTopupTableDiscount").text(Sv.NumberToString(d));
            $("#sumTopupTablePrice").text(Sv.NumberToString(s));
        }
    },
    removeItemTopup: function (id) {
        var tr = ctrlList.formTopup.find('[data-id="' + id + '"]').parents('tr');
        ctrlList.topupTable.rows(tr)
            .remove()
            .draw();

        var index = 0;
        ctrlList.topupTable.rows().data().map(function (x, i) {
            index = index + parseFloat(x.value);
        });

        $("#sumTopupTable").text(Sv.NumberToString(index));

        ctrlList.viewStepNext();
    },


    pinCodeTable: null,
    pinCodeTableConfirm: null,
    initPinCodeTable: function () {
        ctrlList.pinCodeTable = ctrlList.formPinCode.find('table#pinCodeTable').DataTable({
            paging: true,
            serverSide: false,
            responsive: true,
            scrollX: true,
            columnDefs: [
                {
                    targets: 0,
                    className: 'all text-center',
                    render: function (data, type, row, settings) {
                        return settings.row + 1;
                    },
                },
                {
                    targets: 1,
                    data: "serviceName",
                    className: 'all text-center',
                },
                {
                    targets: 2,
                    data: "categoryName",
                    className: 'all text-center',
                },
                {
                    targets: 3,
                    data: "value",
                    className: 'all text-center',
                    render: function (data, type, row, settings) {
                        return Sv.NumberToString(data);
                    },
                },
                {
                    targets: 4,
                    data: "quantity",
                    className: 'all text-center',
                    render: function (data, type, row, settings) {
                        return Sv.NumberToString(data);
                    },
                },
                {
                    targets: 5,
                    className: 'all text-center',
                    render: function (data, type, row) {
                        return '<button onclick="ctrlList.removeItemPinCode(\'' + row.id + '\')" data-id="' + row.id + '">Xóa</button>';
                    },
                }
            ],
            fnRowCallback: function (nRow, aData, iDisplayIndex) {
                if (aData.categoryCode === "" || aData.categoryName === "Nhà mạng không hợp lệ") {
                    $(nRow).addClass("red")
                }
                return nRow;
            }
        });
    },
    initPinCodeTableConfirm: function () {
        ctrlList.pinCodeTableConfirm = ctrlList.formPinCodeConfirm.find('table#pinCodeTableConfirm').DataTable({
            paging: true,
            serverSide: false,
            responsive: true,
            scrollX: true,
            columnDefs: [
                {
                    targets: 0,
                    className: 'all text-center',
                    render: function (data, type, row, settings) {
                        return settings.row + 1;
                    },
                },
                {
                    targets: 1,
                    data: "serviceName",
                    className: 'all text-center',
                },
                {
                    targets: 2,
                    data: "categoryName",
                    className: 'all text-center',
                },
                {
                    targets: 3,
                    data: "value",
                    className: 'all text-center',
                    render: function (data, type, row, settings) {
                        return Sv.NumberToString(data);
                    },
                },
                {
                    targets: 4,
                    data: "quantity",
                    className: 'all text-center',
                    render: function (data, type, row, settings) {
                        return Sv.NumberToString(data);
                    },
                },
                {
                    targets: 5,
                    data: "discount",
                    className: 'all text-center',
                    render: function (data, type, row, settings) {
                        return Sv.NumberToString(data);
                    },
                },
                {
                    targets: 6,
                    data: "price",
                    className: 'all text-center',
                    render: function (data, type, row, settings) {
                        return Sv.NumberToString(data);
                    },
                }
            ]
        });
    },
    addTableDataPinCode: function (dataList) {
        ctrlList.pinCodeTable.rows().remove();
        if (dataList && dataList.length > 0) {
            var date = (new Date()).getTime();
            var q = 0;
            dataList.forEach(function (item, i) {
                if (item.id === undefined || item.id === null) {
                    item.id = date + "_" + i;
                }
                ctrlList.pinCodeTable.row
                    .add(item)
                    .draw();
                q = q + item.quantity;
            });

            $("#sumPinCodeTable").text(Sv.NumberToString(q));
        }
        ctrlList.viewStepNext();
    },
    addTableDataPinCodeByPopup: function (item) {
        var date = (new Date()).getTime();
        item.id = date + "_";
        ctrlList.pinCodeTable.row
            .add(item)
            .draw();

        var q = 0;
        ctrlList.pinCodeTable.rows().data().map(function (x, i) {
            q = q + parseInt(x.quantity);
        });
        $("#sumPinCodeTable").text(Sv.NumberToString(q));
        ctrlList.viewStepNext();
    },
    addTableDataPinCodeConfirm: function (dataList) {

        ctrlList.pinCodeTableConfirm.rows().remove();
        if (dataList && dataList.length > 0) {

            var d = 0;
            var s = 0;
            var q = 0;

            var date = (new Date()).getTime();
            dataList.forEach(function (item, i) {
                if (item.id === undefined || item.id === null) {
                    item.id = date + "_" + i;
                }
                ctrlList.pinCodeTableConfirm.row
                    .add(item)
                    .draw();

                s = s + item.price;
                d = d + item.discount;
                q = q + item.quantity;
            });

            $("#sumPinCodeQty").text(Sv.NumberToString(q));
            $("#sumPinCodePrice").text(Sv.NumberToString(s));
            $("#sumPinCodeTableQty").text(Sv.NumberToString(q));
            $("#sumPinCodeTableDiscount").text(Sv.NumberToString(d));
            $("#sumPinCodeTablePrice").text(Sv.NumberToString(s));
        }
    },
    removeItemPinCode: function (id) {
        var tr = ctrlList.formPinCode.find('[data-id="' + id + '"]').parents('tr');
        ctrlList.pinCodeTable.rows(tr)
            .remove()
            .draw();

        var index = 0;
        ctrlList.pinCodeTable.rows().data().map(function (x, i) {
            index = index + parseInt(x.quantity);
        });

        $("#sumPinCodeTable").text(Sv.NumberToString(index));

        ctrlList.viewStepNext();
    },

    payBillTable: null,
    payBillTableConfirm: null,
    initPayBillTable: function () {
        ctrlList.payBillTable = ctrlList.formPayBill.find('table#payBillTable').DataTable({
            paging: true,
            serverSide: false,
            responsive: true,
            scrollX: true,
            fixedHeader: true,
            columnDefs: [
                {
                    targets: 0,
                    className: 'all text-center',
                    render: function (data, type, row, settings) {
                        return settings.row + 1;
                    },
                },
                {
                    targets: 1,
                    data: "serviceName",
                    className: 'all text-center',
                },
                {
                    targets: 2,
                    data: "productName",
                    className: 'all text-center',
                    render: function (data, type, row, settings) {
                        return data;
                    }
                },
                {
                    targets: 3,
                    data: "receiverInfo",
                    className: 'all text-center',
                },
                {
                    targets: 4,
                    data: "value",
                    className: 'all text-center',
                    render: function (data, type, row, settings) {
                        return Sv.NumberToString(data);
                    }
                },
                {
                    targets: 5,
                    data: "decription",
                    className: 'all text-center',
                },
                {
                    targets: 6,
                    className: 'all text-center',
                    render: function (data, type, row) {
                        return '<button onclick="ctrlList.removeItemPayBill(\'' + row.id + '\')" data-id="' + row.id + '">Xóa</button>';
                    },
                }
            ],
            fnRowCallback: function (nRow, aData, iDisplayIndex) {
                if (aData.categoryCode === "" || aData.categoryName === "Nhà mạng không hợp lệ") {
                    $(nRow).addClass("red")
                }
                return nRow;
            }
        });
    },
    initPayBillTableConfirm: function () {
        ctrlList.payBillTableConfirm = ctrlList.formPayBillConfirm.find('table#payBillTableConfirm').DataTable({
            paging: true,
            serverSide: false,
            responsive: true,
            scrollX: true,
            columnDefs: [
                {
                    targets: 0,
                    className: 'all text-center',
                    render: function (data, type, row, settings) {
                        return settings.row + 1;
                    },
                },
                {
                    targets: 1,
                    data: "serviceName",
                    className: 'all text-center',
                },
                {
                    targets: 2,
                    data: "productName",
                    className: 'all text-center',
                },
                {
                    targets: 3,
                    data: "receiverInfo",
                    className: 'all text-center',
                },
                {
                    targets: 4,
                    data: "value",
                    className: 'all text-center',
                    render: function (data, type, row, settings) {
                        return Sv.NumberToString(data);
                    },
                },
                {
                    targets: 5,
                    data: "discount",
                    className: 'all text-center',
                    render: function (data, type, row, settings) {
                        return Sv.NumberToString(data);
                    },
                },
                {
                    targets: 6,
                    data: "fee",
                    className: 'all text-center',
                    render: function (data, type, row, settings) {
                        return Sv.NumberToString(data);
                    },
                },
                {
                    targets: 7,
                    data: "price",
                    className: 'all text-center',
                    render: function (data, type, row, settings) {
                        return Sv.NumberToString(data);
                    },
                }
            ]
        });
    },
    addTableDataPayBill: function (dataList) {
        ctrlList.payBillTable.rows().remove();
        if (dataList && dataList.length > 0) {
            var date = (new Date()).getTime();
            var s = 0;
            dataList.forEach(function (item, i) {
                if (item.id === undefined || item.id === null) {
                    item.id = date + "_" + i;
                }
                ctrlList.payBillTable.row
                    .add(item)
                    .draw();

                s = s + item.value;
            });

            $("#sumPayBillTable").text(Sv.NumberToString(s));
        }
        ctrlList.viewStepNext();
    },
    addTableDataPayBillConfirm: function (dataList) {

        ctrlList.payBillTableConfirm.rows().remove();
        if (dataList && dataList.length > 0) {
            var date = (new Date()).getTime();
            var v = 0;
            var d = 0;
            var f = 0;
            var s = 0;
            var q = 0;
            dataList.forEach(function (item, i) {
                if (item.id === undefined || item.id === null) {
                    item.id = date + "_" + i;
                }

                if (item.value > 0) {
                    ctrlList.payBillTableConfirm.row
                        .add(item)
                        .draw();

                    v = v + item.value;
                    d = d + item.discount;
                    f = f + item.fee;
                    s = s + item.price;
                    q = q + 1;
                }
            });

            $("#sumPayBillTableValue").text(Sv.NumberToString(v));
            $("#sumPayBillTableDiscount").text(Sv.NumberToString(d));
            $("#sumPayBillTableFee").text(Sv.NumberToString(f));
            $("#sumPayBillTablePrice").text(Sv.NumberToString(s));
            $("#sumPayBillQty").text(Sv.NumberToString(q));
            $("#sumPayBillPrice").text(Sv.NumberToString(s));
        }
    },
    removeItemPayBill: function (id) {
        var tr = ctrlList.formPayBill.find('[data-id="' + id + '"]').parents('tr');
        ctrlList.payBillTable.rows(tr)
            .remove()
            .draw();

        var s = 0;
        ctrlList.payBillTable.rows().data().map(function (x, i) {
            s = s + parseFloat(x.value);
        });
        $("#sumPayBillTable").text(Sv.NumberToString(s));
        ctrlList.viewStepNext();
    },


    //Nhap file topup
    eventFileTopup: function () {
        ctrlList.formTopup.find('input[type=file][name=fileTopupInput]').on('change', function () {
            var fileInput = ctrlList.formTopup.find('input[type=file][name=fileTopupInput]');
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
            var batchType = $('#hdnBatchType').val();
            Sv.AjaxPostFile({
                Url: abp.appPath + "BatchTopup/ImportTopupList?BatchType=" + batchType,
                Data: formData
            }, function (response) {
                ctrlList.addTableDataTopup(response.result.payload);
                document.getElementById('fileTopupInput').value = "";
                if (response.result.responseCode == "00") {
                    abp.message.error(response.result.responseMessage);
                }
            }, function () {
                abp.message.error("Upload file lỗi!");
            });
        });
    },

    //Nhap file ma the
    eventFilePinCode: function () {
        ctrlList.formPinCode.find('input[type=file][name=filePinCodeInput]').on('change', function () {
            var fileInput = ctrlList.formPinCode.find('input[type=file][name=filePinCodeInput]');
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
            var batchType = $('#hdnBatchType').val();
            Sv.AjaxPostFile({
                Url: abp.appPath + "BatchTopup/ImportTopupList?BatchType=" + batchType,
                Data: formData
            }, function (response) {
                ctrlList.addTableDataPinCode(response.result.payload);
                document.getElementById('filePinCodeInput').value = "";
                if (response.result.responseCode == "00") {
                    abp.message.error(response.result.responseMessage);
                }
            }, function () {
                abp.message.error("Upload file lỗi!");
            });
        });
        ctrlList.viewStepNext();
    },

    //Nhap file hoa don
    eventFilePayBill: function () {
        ctrlList.formPayBill.find('input[type=file][name=filePayBillInput]').on('change', function () {
            var fileInput = ctrlList.formPayBill.find('input[type=file][name=filePayBillInput]');
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
            var batchType = $('#hdnBatchType').val();
            Sv.AjaxPostFile({
                Url: abp.appPath + "BatchTopup/ImportTopupList?BatchType=" + batchType,
                Data: formData
            }, function (response) {
                ctrlList.addTableDataPayBill(response.result.payload);
                document.getElementById('filePayBillInput').value = "";
                if (response.result.responseCode == "00") {
                    abp.message.error(response.result.responseMessage);
                }
            }, function () {
                abp.message.error("Upload file lỗi!");
            });
        });
    },

    changeActiveClass: function () {
        $('#topup_PageList').find('.tab .nav-tabs li a').click(function () {
            let indexEle = $(this).attr('data-label');
            ctrlList.changeLabelHeader(indexEle);
        });
    },
};

$('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
    $($.fn.dataTable.tables(true)).DataTable()
        .columns.adjust();
});

$(document).ready(function () {
    //ctrlList.enterHandler();
    ctrlList.changeActiveClass();
    //topup

    ctrlList.initTopupTable();
    ctrlList.eventFileTopup();
    ctrlList.initTopupTableConfirm();

    //PayBill
    ctrlList.initPayBillTable();
    ctrlList.eventFilePayBill();
    ctrlList.initPayBillTableConfirm();

    //PinCode
    ctrlList.initPinCodeTable();
    ctrlList.eventFilePinCode();
    ctrlList.initPinCodeTableConfirm();

    ctrlList.viewStepNext();

    var _addCardModal = new app.ModalManager({
        viewUrl: abp.appPath + 'BatchTopup/AddCardModal',
        scriptUrl: abp.appPath + 'view-resources/Views/BatchTopup/_AddCardModal.js',
        modalClass: 'AddCardModal',
        modalSize: 'modal-xl'
    });

    $('#CreateAddCardButton').click(function () {
        _addCardModal.open();
    });
});
