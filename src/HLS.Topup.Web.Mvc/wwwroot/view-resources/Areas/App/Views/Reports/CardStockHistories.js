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
                ajaxFunction: _rptService.getReportCardStockHistories,
                inputFilter: function () {
                    return {
                        filter: $('#ProvidersTableFilter').val(),
                        stockCodeFilter: $('#StockCodeFilterId').val(),
                        telcoFilter: $('#TelcoFilterId').val(),
                        fromDateFilter: getDateFilter($('#FromDateFilterId')),
                        toDateFilter: getDateFilter($('#ToDateFilterId')),
                        statusFilter: $('#StatusFilterId').val(),
                        cardValueFilter: $('#CardValueFilterId').val(),
                        productCodeFilter: $('#productCodeFilter').val()
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
                    data: "createdDate",
                    name: "createdDate",
                    render: function (createdDate) {
                        if (createdDate) {
                            return moment(new Date(createdDate)).format('DD/MM/YYYY HH:mm:ss');
                        }
                        return "";
                    }
                },
                {
                    targets: 2,
                    data: "stockCode",
                    name: "stockCode"
                },
                {
                    targets: 3,
                    data: "stockType",
                    name: "stockType"
                },
                {
                    targets: 4,
                    data: "productCode",
                    name: "productCode"
                },
                {
                    targets: 5,
                    data: "inventoryBefore",
                    name: "inventoryBefore",
                    className: "text-right"
                },
                {
                    targets: 6,
                    data: "increase",
                    name: "increase",
                    className: "text-right"
                },
                {
                    targets: 7,
                    data: "decrease",
                    name: "decrease",
                    className: "text-right"
                },
                {
                    targets: 8,
                    data: "inventoryAfter",
                    name: "inventoryAfter",
                    className: "text-right"
                },
                {
                    targets: 9,
                    data: "inventoryType",
                    name: "inventoryType"
                },
                {
                    targets: 10,
                    data: "serial",
                    name: "serial"
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

        function getProviders() {          
            dataTable.ajax.reload();
        }

        $('#ShowAdvancedFiltersSpan').click(function () {
            $('#ShowAdvancedFiltersSpan').hide();
            $('#HideAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideDown();
        });

        $('#HideAdvancedFiltersSpan').click(function () {
            $('#HideAdvancedFiltersSpan').hide();
            $('#ShowAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideUp();
        });

        $('#ExportToExcelButton').click(function () {
            _rptService.getReportCardStockHistoriesToExcel({
                filter: $('#ProvidersTableFilter').val(),
                stockCodeFilter: $('#StockCodeFilterId').val(),
                telcoFilter: $('#TelcoFilterId').val(),
                fromDateFilter: getDateFilter($('#FromDateFilterId')),
                toDateFilter: getDateFilter($('#ToDateFilterId')),
                statusFilter: $('#StatusFilterId').val(),
                cardValueFilter: $('#CardValueFilterId').val(),
                productCodeFilter: $('#productCodeFilter').val()
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
        $("#categoryCodeFilter").change(function (e) {
            const cateCode = $(e.target).val();
            Sv.GetProductByCate(cateCode, $("#productCodeFilter"), false);
        });
        $(document).keypress(function (e) {
            if (e.which === 13) {
                e.preventDefault();
                getProviders(); 
            }
        });
    });
})();
