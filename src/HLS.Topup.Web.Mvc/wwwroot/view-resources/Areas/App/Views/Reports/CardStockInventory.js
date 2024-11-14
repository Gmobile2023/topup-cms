(function () {
    $(function () {

        const _$providersTable = $('#ProvidersTable');

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
                ajaxFunction: _rptService.getReportCardStockInventory,
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
                    data: "stockCode",
                    name: "stockCode"
                },
                {
                    targets: 2,
                    data: "stockType",
                    name: "stockType"
                },
                {
                    targets: 3,
                    data: "productCode",
                    name: "productCode"
                },
                {
                    targets: 4,
                    data: "inventoryBefore",
                    name: "inventoryBefore",
                    className: "all text-right"
                },
                {
                    targets: 5,
                    data: "increase",
                    name: "increase",
                    className: "all text-right"
                },
                {
                    targets: 6,
                    data: "decrease",
                    name: "decrease",
                    className: "all text-right"
                },
                {
                    targets: 7,
                    data: "inventoryAfter",
                    name: "inventoryAfter",
                    className: "all text-right"
                },
                {
                    targets: 8,
                    data: "createdDate",
                    name: "createdDate",
                    render: function (createdDate) {
                        if (createdDate) {
                            return moment(new Date(createdDate)).format('DD/MM/YYYY');
                        }
                        return "";
                    }
                },
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
            _rptService.getReportCardStockInventoryToExcel({
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
            abp.ui.setBusy();
            let _s = $("#AdvacedAuditFiltersArea");
            Sv.GetProductByCate(cateCode, _s.find("#productCodeFilter"), false);
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getProviders();
            }
        });
    });
})();
