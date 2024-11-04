(function () {
    $(function () {
        var _$table = $("#Table");
        var _$topupRequests = abp.services.app.topupRequests;
        //lấy thời gian
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });
        var getDateFilter = function (element) {
            return element.data("DateTimePicker").date() == null ? null : element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z");
        }
        var dataTable = _$table.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _$topupRequests.getCheckChargesHistories,
                inputFilter: function () {
                    return {
                        codeRequestFilter: $("#CodeRequestFilter").val(),
                        fromDateFilter: getDateFilter($("#FromDateFilterId")),
                        toDateFilter: getDateFilter($("#ToDateFilterId"))
                    };
                }
            },
            columnDefs: [
                {
                    targets: 0,
                    orderable: false,
                    className: "text-center",
                    render: (data, type, row, meta) => {
                        return meta.row;
                    }
                },
                {
                    targets: 1,
                    className: "text-center",
                    data: null,
                    orderable: false,
                    autoWidth: false,
                    defaultContent: '',
                    rowAction: {
                        text: '<i class="fa fa-cog"></i> ' + app.localize('Actions') + ' <span class="caret"></span>',
                        items: [
                            {
                                text: app.localize('Delete'),
                                visible: function () {
                                    return false;
                                },
                                addAtributes: "href",
                                action: function (data) {
                                }
                            },
                            {
                                text: "Xuất excel",
                                visible: function (data) {
                                    return true;
                                },
                                action: function (data) {
                                    console.log(data);
                                    _$topupRequests.getCheckChargesToExcel({
                                        codeRequestFilter: data.record.codeRequest
                                    })
                                        .done((result) => {
                                            app.downloadTempFile(result);
                                        })
                                    /* download file excel
                                     abp.appPath: thhuw mục gốc của file hiện tại
                                     */
                                    //return window.open(abp.appPath + "assets/SampleFiles/CheckChargesListSampleFile.xlsx");
                                }
                            }
                        ]
                    }
                },
                {
                    targets: 2,
                    className: "text-center",
                    data: "codeRequest",
                    render: (data) => {
                        return "<a href='/CheckCharges/Detail?codeRequest=" + data + "'>" + data + "</a>";
                    }
                },
                {
                    targets: 3,
                    className: "text-center",
                    data: "amount"
                },
                {
                    targets: 4,
                    className: "text-center",
                    data: "createTime",
                    render: (data) => {
                        return moment(data).format("DD/MM/YYYY HH:mm:SS")
                    }
                },
                {
                    targets: 5,
                    className: "text-center",
                    data: "feeRequest"
                },
            ]
        });
        $("#btnSearch").click((e) => {
            e.preventDefault();
            dataTable.ajax.reload();
        })
    })
})()

