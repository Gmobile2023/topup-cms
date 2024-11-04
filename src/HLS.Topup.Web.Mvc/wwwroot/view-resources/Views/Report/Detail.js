(function () {
    $(function () {
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var getDateFilter = function (element) {
            return element.data("DateTimePicker").date() === null ? null : element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z");
        };

        $("#selectProvider").select2();

        $("#selectUserProcess").select2({
            placeholder: 'Chọn người thực hiện',
            allowClear: true,
            ajax: {
                url: abp.appPath + "api/services/app/CommonLookup/GetStaffUserQuery",
                dataType: 'json',
                delay: 250,
                data: function (params) {
                    return {
                        search: params.term,
                        page: params.page,
                    };
                },
                processResults: function (data, params) {
                    params.page = params.page || 1;

                    return {
                        results: $.map(data.result, function (item) {
                            return {
                                text: item.accountCode + "-" + item.phoneNumber + "-" + item.fullName,
                                id: item.accountCode
                            }
                        }),
                        pagination: {
                            more: (params.page * 30) < data.result.length
                        }
                    };
                },
                cache: true
            },
            minimumInputLength: 3,
            language: abp.localization.currentCulture.name
        });

        var _$table = $('#Table');
        var _reportService = abp.services.app.reportSystem;
        var dataTable = _$table.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _reportService.getReportTransDetailList,
                inputFilter: function () {
                    return {
                        fromDate: getDateFilter($('#FromDateFilterId')),
                        toDate: getDateFilter($('#ToDateFilterId')),
                        serviceCode: $('#ServiceCodeFilter').val(),
                        status: $("#StatusFilterId").val(),
                        userProcess: $("#selectUserProcess").val(),
                        providerCode: $("#selectProvider").val(),
                        receivedAccount: $("#txtReceivedAccount").val(),
                        requestTransCode: $("#txtRequestTrans").val(),
                    };
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
                    data: "statusName",
                    className: 'all ctrl-ss',
                    render: (data, type, row) => {
                        console.log(row);
                        let opt_print = '';
                        if(agentType == 'WholesaleAgent' &&  (row.transType === "PIN_CODE" || row.transType === "PIN_DATA" || row.transType === "PIN_GAME") && row.status == 1 ){
                            opt_print = '<i class="fa fa-download download_file" data-type="' + row.statusName + '" data-trans-code="' + row.transCode + '" style="color: #2188C9; cursor: pointer; margin-right: 15px;" title="Tải file" aria-hidden="true"></i>';
                            return opt_print + row.statusName;
                        }else{  
                            if (row.print === 1) {
                                opt_print = '<i class="fa fa-print print-invoice-item" data-type="' + row.statusName + '" data-trans-code="' + row.transCode + '" style="color: #2188C9; cursor: pointer; margin-right: 15px;" title="In hoá đơn" aria-hidden="true"></i>';
                            } else {
                                opt_print = '<i class="fa fa-print" style="color: #2188C9; cursor: pointer; margin-right: 15px; opacity: .4;" title="In hoá đơn" aria-hidden="true"></i>';
                            }
    
                            if (row.transType === "PIN_CODE" || row.transType === "PIN_DATA" || row.transType === "PIN_GAME")
                                return opt_print + '<a href = "/Transactions/TransactionDetail?transcode=' + row.transCode + '&serviceCode=' + row.transType + '">' + row.statusName + '</a>';
                            return opt_print + row.statusName;
                        }
                       
                    }
                },
                {
                    targets: 2,
                    data: "transTypeName",
                    render: function (data, dispaly, row) {
                        if (row.status === 3)
                            return "<g style='color: red'>" + data + "</g>";
                        if (row.status === 2)
                            return "<g style='color: brown'>" + data + "</g>";
                        else
                            return data;
                    }
                },
                {
                    targets: 3,
                    data: "vender",
                    render: function (data, dispaly, row) {
                        if (row.status === 3)
                            return "<g style='color: red'>" + data + "</g>";
                        if (row.status === 2)
                            return "<g style='color: brown'>" + data + "</g>";
                        else
                            return data;
                    }
                },
                {
                    targets: 4,
                    className: "all text-right",
                    data: "amount",
                    name: "amount",
                    render: function (data, dispaly, row) {
                        if (row.status === 3)
                            return "<g style='color: red'>" + (data ? Sv.format_number(data) : "0") + "</g>";
                        if (row.status === 2)
                            return "<g style='color: brown'>" + (data ? Sv.format_number(data) : "0") + "</g>";
                        else
                            return (data ? Sv.format_number(data) : "0") ;
                    }
                },
                {
                    targets: 5,
                    className: "all text-right",
                    data: "quantity",
                    render: function (data, dispaly, row) {
                        if (row.status === 3)
                            return "<g style='color: red'>" + (data ? Sv.format_number(data) : "0") + "</g>";
                        if (row.status === 2)
                            return "<g style='color: brown'>" + (data ? Sv.format_number(data) : "0") + "</g>";
                        else
                            return (data ? Sv.format_number(data) : "0") ;
                    }

                },
                {
                    targets: 6,
                    className: "all text-right",
                    data: "discount",
                    render: function (data, dispaly, row) {
                        if (row.status === 3)
                            return "<g style='color: red'>" + (data ? Sv.format_number(data) : "0") + "</g>";
                        if (row.status === 2)
                            return "<g style='color: brown'>" + (data ? Sv.format_number(data) : "0") + "</g>";
                        else
                            return (data ? Sv.format_number(data) : "0");
                    }

                },
                {
                    targets: 7,
                    className: "all text-right",
                    data: "fee",
                    render: function (data, dispaly, row) {
                        if (row.status === 3)
                            return "<g style='color: red'>" + (data ? Sv.format_number(data) : "0") + "</g>";
                        if (row.status === 2)
                            return "<g style='color: brown'>" + (data ? Sv.format_number(data) : "0") + "</g>";
                        else
                            return (data ? Sv.format_number(data) : "0");
                    }

                },
                {
                    targets: 8,
                    className: "all text-right",
                    data: "priceIn",
                    render: function (data, dispaly, row) {
                        //if (row.status === 3)
                        //    return "<g style='color: red'>" + (data ? Sv.format_number(data) : "0") + "</g>";
                        //else if (row.status === 2)
                        //    return "<g style='color: brown'>" + (data ? Sv.format_number(data) : "0") + "</g>";
                        //else {
                        //    if (data < 0)
                        //        return "<g style='color: red'>" + (data ? Sv.format_number(data) : "0") + "</g>";
                        //    else return "<g style='color: blue'>" + (data ? Sv.format_number(data) : "0") + "</g>";
                        //}
                        return "<g style='color: blue'>" + (data ? Sv.format_number(data) : "0") + "</g>";
                    }
                },
                {
                    targets: 9,
                    className: "all text-right",
                    data: "priceOut",
                    render: function (data, dispaly, row) {
                        //if (row.status === 3)
                        //    return "<g style='color: red'>" + (data ? Sv.format_number(data) : "0") + "</g>";
                        //else if (row.status === 2)
                        //    return "<g style='color: brown'>" + (data ? Sv.format_number(data) : "0") + "</g>";
                        //else {
                        //    if (data < 0)
                        //        return "<g style='color: red'>" + (data ? Sv.format_number(data) : "0") + "</g>";
                        //    else return "<g style='color: blue'>" + (data ? Sv.format_number(data) : "0") + "</g>";
                        //}

                        return "<g style='color: red'>" + (data ? Sv.format_number(data) : "0") + "</g>";
                    }
                },
                {
                    targets: 10,
                    data: "balance",
                    className: "all text-right",
                    render: function (data, dispaly, row) {
                        if (row.status === 3)
                            return "<g style='color: red'>" + (data ? Sv.format_number(data) : "0") + "</g>";
                        if (row.status === 2)
                            return "<g style='color: brown'>" + (data ? Sv.format_number(data) : "0") + "</g>";
                        else
                            return (data ? Sv.format_number(data) : "0") ;
                    }
                },
                {
                    targets: 11,
                    data: "accountRef",
                    render: function (data, dispaly, row) {
                        if (row.status === 3)
                            return "<g style='color: red'>" + data + "</g>";
                        if (row.status === 2)
                            return "<g style='color: brown'>" + data + "</g>";
                        else
                            return data;
                    }
                },
                {
                    targets: 12,
                    data: "transCode",
                    render: function (data, dispaly, row) {
                        if (row.status === 3)
                            return "<g style='color: red'>" + data + "</g>";
                        if (row.status === 2)
                            return "<g style='color: brown'>" + data + "</g>";
                        else
                            return data;
                    }
                },
                {
                    targets: 13,
                    data: "userProcess",
                    render: function (data, dispaly, row) {
                        if (row.status === 3)
                            return "<g style='color: red'>" + data + "</g>";
                        if (row.status === 2)
                            return "<g style='color: brown'>" + data + "</g>";
                        else
                            return data;
                    }
                },
                {
                    targets: 14,
                    data: "createdDate",
                    name: "createdDate",
                    render: function (createdTime, dispaly, row) {
                        if (row.status === 3)
                            return "<g style='color: red'>" + (moment(new Date(createdTime)).format('DD/MM/YYYY HH:mm:ss')) + "</g>";
                        if (row.status === 2)
                            return "<g style='color: brown'>" + (moment(new Date(createdTime)).format('DD/MM/YYYY HH:mm:ss')) + "</g>";
                        else
                            return moment(new Date(createdTime)).format('DD/MM/YYYY HH:mm:ss');
                    }
                },
                {
                    targets: 15,
                    data: "requestTransSouce",
                    render: function (data, dispaly, row) {
                        if (row.status === 3)
                            return "<g style='color: red'>" + data + "</g>";
                        if (row.status === 2)
                            return "<g style='color: brown'>" + data + "</g>";
                        else
                            return data;
                    }
                }
            ],
            "headerCallback": function (thead, data, start, end, display) {
                try {
                    var rawServerResponse = this.api().settings()[0].rawServerResponse;
                    if (rawServerResponse.warning.length > 0)
                        abp.message.info(rawServerResponse.warning);


                    if (rawServerResponse !== undefined && rawServerResponse.totalData !== undefined) {                        
                        $(thead).find('th').eq(1).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.quantity));
                        $(thead).find('th').eq(2).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.discount));
                        $(thead).find('th').eq(3).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.fee));
                        $(thead).find('th').eq(4).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.priceIn));
                        $(thead).find('th').eq(5).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.priceOut));
                        $(thead).find('th').eq(6).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.balance));
                    }
                } catch (e) {
                    console.log("không có total")
                }
            }
        });


        function getTables() {
            dataTable.ajax.reload();
        }

        $('#btnSearch').click(function (e) {
            e.preventDefault();
            getTables();
        });

        $('#TableFilter').on('keydown', function (e) {
            if (e.keyCode !== 13) {
                return;
            }

            e.preventDefault();
            getTables();
        });

        abp.event.on('app.createOrEditAgentModalSaved', function () {
            getTables();
        });

        $("#TableFilter").focus();


        $('#ExportToExcelButton').click(function () {

            abp.ui.setBusy();
            _reportService.getReportTransDetailListToExcel({
                fromDate: getDateFilter($('#FromDateFilterId')),
                toDate: getDateFilter($('#ToDateFilterId')),
                serviceCode: $('#ServiceCodeFilter').val(),
                status: $("#StatusFilterId").val(),
                userProcess: $("#selectUserProcess").val(),
                providerCode: $("#selectProvider").val(),
                receivedAccount: $("#txtReceivedAccount").val(),
                requestTransCode: $("#txtRequestTrans").val(),
            })
                .done(function (result) {
                    if (result.fileName === "WAIT")
                        abp.message.info('Hệ thống đang bắt đầu lấy dữ liệu. Quá trình hoàn tất sẽ gửi thông báo cho bạn. Trân trọng!');
                   else  if (result.fileName === "Warning")
                        abp.message.info(result.filePath);
                    else if (result.fileName === "Downloadlink")
                        window.open(result.filePath);
                    else
                        app.downloadTempFile(result);
                }).always(function () {
                    abp.ui.clearBusy();
                });
        });

        $(document).on('click', '.print-invoice-item', function () {
            let _type = $(this).attr('data-type');
            let _transcode = $(this).attr('data-trans-code');
            ngtAction.open(_transcode, 'print');
        });
        $(document).on('click', '.download_file', function () { 
            let _transcode = $(this).attr('data-trans-code');
            let v1 = sendMethod == "Email" ? "Email" : "Số điện thoại" ;
            let v2 = sendMethod == "Email" ? "email" : "tin nhắn" ;
            var msg= "Tiến trình tải xuống đang diễn ra. Vui lòng đợi trong giây lát. File tải xuống đã được đặt mã bảo mật, mật khẩu đã được gửi về "+v1+" của bạn. Vui lòng kiểm tra  "+v2+"  để nhận mã.";
            if(sendMethod == "Email" || sendMethod == "Sms" ){
                abp.message.info(msg);
            }
            abp.ui.setBusy();
            abp.services.app.transactions.zipCards(_transcode)
                .done(function (result) {
                    app.downloadTempFile(result);
                    // let v1 = sendMethod == "Email" ? "Email" : "Số điện thoại" ;
                    // let v2 = sendMethod == "Email" ? "email" : "tin nhắn" ;
                    // var msg= "Tiến trình tải xuống đang diễn ra. Vui lòng đợi trong giây lát. File tải xuống đã được đặt mã bảo mật, mật khẩu đã được gửi về "+v1+" của bạn. Vui lòng kiểm tra  "+v2+"  để nhận mã.";
                    // if(sendMethod == "Email" || sendMethod == "Sms" ){
                    //     abp.message.info(msg);
                    // }
                }).always(function () {
                abp.ui.clearBusy();
            });
        });
    });
})();
