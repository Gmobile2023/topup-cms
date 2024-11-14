(function () {
    $(function () {

        var _$table = $('#Table');
        var _reportService = abp.services.app.reportSystem;
        var getDateFilter = function (element) {
            return element.data("DateTimePicker").date() === null ? null : element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z");
        };

        var getDateSub = function (element) {
            return element.data("DateTimePicker").date() === null ? null : element.data("DateTimePicker").date().format("DD-MM-YYYY");
        };


        $("#selectService").select2();
        $("#selectCategory").select2();
        $("#selectProduct").select2();

        getDashboard();

        function getDashboard() {
            var formData = {};
            formData.fromDate = getDateFilter($('#FromDate'));
            formData.toDate = getDateFilter($('#ToDate'));
            formData.serviceCode = $('#selectService').val();
            formData.categoryCode = $('#selectCategory').val();
            formData.productCode = $('#selectProduct').val();

            _reportService.getDashboardListRevenue(
                formData
            ).done(function (reponse) {
                if (reponse.length > 0)
                    $("#chartContainer").show();
                else $("#chartContainer").hide();

                var r = 0;
                var d = 0;
                reponse.forEach(function (item, i) {
                    r = r + item.y;
                    d = d + item.d;
                });

                $("#spanRevenue").text(Sv.NumberToString(r) + 'đ');
                $("#spanDiscount").text(Sv.NumberToString(d) + 'đ');

                var chart = new CanvasJS.Chart("chartContainer", {
                    animationEnabled: true,
                    theme: "light2", // "light1", "light2", "dark1", "dark2"
                    title: {
                        text: ""
                    },
                    axisY: {
                        title: ""
                    },
                    data: [{
                        type: "column",
                        name: "Doanh số",
                        showInLegend: true,
                        color: "#17A78F",
                        indexLabel: "{y}",
                        ValueFormatString: "#,##0k",
                        dataPoints: reponse
                    }]
                });
                chart.render();
            }).always(function () {
            });
        }

        var dataTable = _$table.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _reportService.getReportRevenueDashboardList,
                inputFilter: function () {
                    return {
                        fromDate: getDateFilter($('#FromDate')),
                        toDate: getDateFilter($('#ToDate')),
                        serviceCode: $('#selectService').val(),
                        categoryCode: $('#selectCategory').val(),
                        productCode: $('#selectProduct').val(),
                    };
                }
            },
            columnDefs: [
                {
                    targets: 0,
                    data: "createdDay",
                    render: function (createdDay) {
                        if (createdDay) {
                            return moment(new Date(createdDay)).format('DD/MM/YYYY');
                        }
                        return "";
                    }
                },
                {
                    targets: 1,
                    className: "all text-right",
                    data: "revenue",
                    name: "revenue",
                    render: function (data) {
                        return data ? Sv.format_number(data) : "0"
                    }
                },
                {
                    targets: 2,
                    className: "all text-right",
                    data: "discount",
                    name: "discount",
                    render: function (data) {
                        return data ? Sv.format_number(data) : "0"
                    }
                }
            ],
            "headerCallback": function (thead, data, start, end, display) {
                try {
                    var rawServerResponse = this.api().settings()[0].rawServerResponse;
                    if (rawServerResponse.warning.length > 0)
                        abp.message.info(rawServerResponse.warning);
                } catch (e) {
                    console.log("không có total")
                }
            }
        });

        function getTables() {
            dataTable.ajax.reload();
        }


        $('#btnSearch').click(function (e) {

            var ftxtDate = getDateSub($('#FromDate'));
            var ttxtDate = getDateSub($('#ToDate'));
            var fDate = ftxtDate.split("-");
            var fromDate = new Date(fDate[2], fDate[1], fDate[0]);
            var tDate = ttxtDate.split("-");
            var toDate = new Date(tDate[2], tDate[1], tDate[0]);
            var diff = toDate.getTime() - fromDate.getTime();


            var days = Math.floor(diff / (1000 * 60 * 60 * 24));
            if (days < 0) {
                abp.message.info('Thời gian tìm kiếm không hợp lệ. Quý khách chọn thời gian từ lớn hơn tới');
            } else if (days > 31) {

                abp.message.info('Thời gian tìm kiếm không hợp lệ. Quý khách tìm kiếm trong khoảng 31 ngày');

            } else {
                getDashboard();
                getTables();
            }
        });

        $('#ExportToExcelButton').click(function () {
            _reportService.getReportRevenueDashboardListToExcel({
                fromDate: getDateFilter($('#FromDate')),
                toDate: getDateFilter($('#ToDate')),
                serviceCode: $('#selectService').val(),
                categoryCode: $('#selectCategory').val(),
                productCode: $('#selectProduct').val(),
            })
                .done(function (result) {
                    if (result.fileName === "Warning")
                        abp.message.info(result.filePath);
                    else
                        app.downloadTempFile(result);
                });
        });

        $("#selectService").change(function (e) {
            const serviceCode = $(e.target).val();
            Sv.GetCateByService(serviceCode, $("#selectCategory"), false);
        });

        $("#selectCategory").change(function (e) {
            const cateCode = $(e.target).val();
            Sv.GetProductByCate(cateCode, $("#selectProduct"), false);
        });


    });
})();