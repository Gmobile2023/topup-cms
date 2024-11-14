(function () {
    $(function () {

        const _$providersTable = $('#ProvidersTable');
        const _providersService = abp.services.app.providers;

        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });
        const getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z");
        };
        const _rptService = abp.services.app.reportSystem;
        const dataTable = _$providersTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _rptService.getReportCardStockImExProvider,
                inputFilter: function () {
                    return {
                        storeCode: $('#StockCode').val(),
                        fromDate: getDateFilter($('#fromDate')),
                        toDate: getDateFilter($('#toDate')),
                        categoryCode: $('#selectCategory').val(),
                        productCode: $('#selectProduct').val(),
                        serviceCode: $('#selectService').val(),
                        providerCode: $('#selectProvider').val()
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
                    data: "serviceName",
                    name: "serviceName"
                },
                {
                    targets: 2,
                    data: "categoryName",
                    name: "categoryName"
                },
                {
                    targets: 3,
                    data: "cardValue",
                    name: "cardValue",
                    className: "all text-right",
                    render: function (data) {
                        return Sv.format_number(data);
                    }
                },
                {
                    targets: 4,
                    data: "before",
                    name: "before",
                    className: "all text-right",
                    render: function (before) {
                        return Sv.format_number(before);
                    }
                },
                {
                    targets: 5,
                    data: "increaseSupplier",
                    name: "increaseSupplier",
                    className: "all text-right",
                    render: function (increaseSupplier) {
                        return Sv.format_number(increaseSupplier);
                    }
                },
                {
                    targets: 6,
                    data: "increaseOther",
                    name: "increaseOther",
                    className: "all text-right",
                    render: function (increaseOther) {
                        return Sv.format_number(increaseOther);
                    }
                },
                {
                    targets: 7,
                    data: "sale",
                    name: "sale",
                    className: "all text-right",
                    render: function (sale) {
                        return Sv.format_number(sale);
                    }
                },
                {
                    targets: 8,
                    data: "exportOther",
                    name: "exportOther",
                    className: "all text-right",
                    render: function (exportOther) {
                        return Sv.format_number(exportOther);
                    }
                },
                {
                    targets: 9,
                    data: "after",
                    name: "after",
                    className: "all text-right",
                    render: function (after) {
                        return Sv.format_number(after);
                    }
                },
                {
                    targets: 10,
                    data: "current",
                    name: "current",
                    className: "all text-right",
                    render: function (current) {
                        return Sv.format_number(current);
                    }
                }
            ],
            "headerCallback": function (thead, data, start, end, display) {
                try {
                    var rawServerResponse = this.api().settings()[0].rawServerResponse;
                    if (rawServerResponse.warning.length > 0)
                        abp.message.info(rawServerResponse.warning);

                    if (rawServerResponse !== undefined && rawServerResponse.totalData !== undefined) {
                        $(thead).find('th').eq(1).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.before));
                        $(thead).find('th').eq(2).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.increaseSupplier));
                        $(thead).find('th').eq(3).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.increaseOther));
                        $(thead).find('th').eq(4).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.sale));
                        $(thead).find('th').eq(5).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.exportOther));
                        $(thead).find('th').eq(6).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.after));
                        $(thead).find('th').eq(7).addClass("text-right").html(Sv.format_number(rawServerResponse.totalData.current));
                    }
                } catch (e) {
                    console.log("không có total")
                }
            }
        });

        function getProviders() {
            dataTable.ajax.reload();
        }

        $('#ExportToExcelButton').click(function () {
            _rptService.getReportCardStockImExProviderToExcel({
                storeCode: $('#StockCode').val(),
                fromDate: getDateFilter($('#fromDate')),
                toDate: getDateFilter($('#toDate')),
                categoryCode: $('#selectCategory').val(),
                productCode: $('#selectProduct').val(),
                serviceCode: $('#selectService').val(),
                providerCode: $('#selectProvider').val()
            })
                .done(function (result) {
                    if (result.fileName === "Warning")
                        abp.message.info(result.filePath);
                    else
                        app.downloadTempFile(result);
                });
        });


        $('#GetProvidersButton').click(function (e) {
            e.preventDefault();
            getProviders();
        });

        $("#selectProduct").select2();

        $("#selectService").change(function (e) {
            const serviceCode = $(e.target).val();
            Sv.GetCateByService(serviceCode, $("#selectCategory"), false);
        });

        $("#selectCategory").change(function (e) {
            const cateCode = $(e.target).val();
            abp.ui.setBusy();
            abp.services.app.commonLookup.getProductByCategory(cateCode)
                .done(function (result) {
                    let html = "<option value=\"\">Chọn sản phẩm</option>";
                    let _s = $("#AdvacedAuditFiltersArea");
                    if (result != null && result.length > 0) {
                        for (let i = 0; i < result.length; i++) {
                            let item = result[i];
                            html += ("<option value=\"" + item.productCode + "\">" + item.productName + "</option>");
                        }
                    }
                    _s.find("#selectProduct").html(html);
                })
                .always(function () {
                    abp.ui.clearBusy();
                });
        });


        $(document).keypress(function (e) {
            if (e.which === 13) {
                getProviders();
            }
        });
    });
})();
