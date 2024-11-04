(function () {
    $(function () {
        var _topupService = abp.services.app.topupRequests;
        var _$checkChargesForm = $("#checkcharges-list-form");
        var _$topupTable = $('#Table');
        var _$listTopup = [];
        var _$dataTableTopupList = null;
        var _$transactionService = abp.services.app.transactions;
        _$checkChargesForm.validate();

        //Show dataTable
        function getDataTableTopup() {
            _$dataTableTopupList = _$topupTable.DataTable({
                paging: true,
                serverSide: false,
                processing: false,
                destroy: true,
                data: _$listTopup,
                createdRow: function (row, data, dataIndex) {
                    $(row).attr('data-id', data.id);
                },
                columnDefs: [
                    {
                        orderable: false,
                        width: "30%",
                        className: "text-center",
                        render: function (data, type, row, meta) {
                            return meta.row+1;
                        },
                        targets: 0
                    },
                    {
                        autoWidth: false,
                        orderable: false,
                        className: "text-center",
                        targets: 1,
                        data: "phoneNumber"
                    }
                ]
            });
        };

        function deleteTopupItem(index) {
            _$dataTableTopupList.splice(index, 1);
            getDataTableTopup();
        }



        getDataTableTopup();

        function refreshTable() {
            _$topupTable.DataTable().clear();
            _$topupTable.DataTable().ajax.reload();
        }

        $("#btnUploadTopup").click(function () {

            var fd = new FormData();
            var files = $('#TopupFile')[0].files[0];
            console.log(files);
            fd.append('TopupFile', files);
            //fd.append('catecode', $("#TopupRequest_TopupType").val());
            fd.append('serviceCode', "CHECKCHARGES");
            $.ajax({
                url: abp.appPath + 'CheckCharges/UploadChargesViettelList',
                type: 'post',
                data: fd,
                contentType: false,
                processData: false,
                success: function (response) {
                    var jsonResult = response.result;
                    if (jsonResult.responseCode == "01") {
                        _$listTopup = jsonResult.payload;
                        if (jsonResult.payload && jsonResult.payload.length >= 1) {
                            $("#GetCheckCharges").show();
                        } else {
                            $("#GetCheckCharges").hide();
                        }
                        getDataTableTopup();
                    } else {
                        abp.notify.warn(jsonResult.responseMessage);
                    }
                },
            });

        });
        $("#GetCheckCharges").click(function () {
            var checkcharges = _$checkChargesForm.serializeFormToObject();
            checkcharges.checkchargesList = _$listTopup;
            abp.message.confirm("Bạn có chắc chắn muốn thực hiện giao dịch?").
                then((confirm) => {
                    if (confirm) {
                        abp.ui.setBusy();
                        _topupService.createCheckChargesListRequest(checkcharges)
                            .done((rs) => {
                                if (rs.responseCode === "01") {
                                    window.location.href = abp.appPath + "CheckCharges/PaymentSuccess";
                                    //abp.message.success("Giao dịch thực hiện thành công");
                                    //location.reload(true);
                                } else {
                                    abp.notify.error(rs.responseMessage);
                                }
                            })
                            .always(function () {
                                abp.ui.clearBusy();
                            });
                    }
                })

        });
    });
})();
