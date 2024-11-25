(function () {
    $(function () {

        var _$transCardsTable = $('#transCardsTable');
        var _cardsService = abp.services.app.cards;

        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var getDateFilter = function (element, isEnd) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            if (isEnd) {
                return element.data("DateTimePicker").date().format("YYYY-MM-DDT23:59:59Z");
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z");
        }

        var dataTable = _$transCardsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _cardsService.getCardStockTransList,
                inputFilter: function () {
                    return {
                        transCodeProvider: $('#TransCodeProviderFilterId').val(),
                        batchCode: $('#BatchCodeFilterId').val(),
                        fromDate: getDateFilter($('#fromDateFilterId'), false),
                        toDate: getDateFilter($('#toDateFilterId'), true),
                        provider: $('#providerCodeFilter').val(),
                        status: $('#StatusFilterId').val(),
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
                    width: 120,
                    targets: 1,
                    data: null,
                    orderable: false,
                    autoWidth: false,
                    defaultContent: '',
                    rowAction: {
                        cssClass: 'btn btn-brand dropdown-toggle',
                        text: '<i class="fa fa-cog"></i> ' + app.localize('Actions') + ' <span class="caret"></span>',
                        items: [
                            {
                                text: app.localize('View'),
                                visible: function () {
                                    return false;
                                },
                                action: function (data) {                                 
                                }
                            },                           
                            {
                                text: 'Kiểm tra mã thẻ',                                
                                action: function (data) {
                                    checkTransProvider(data);
                                }
                            }
                        ]
                    }
                },
                {
                    targets: 2,
                    data: "transCodeProvider",
                    name: "transCodeProvider"
                },
                {
                    targets: 3,
                    data: "batchCode",
                    name: "batchCode"
                },
                {
                    targets: 4,
                    data: "status",
                    name: "status",
                    render: function (status) {
                        const $span = $("<span/>").addClass("label");
                        if (status === 1) {
                            $span.addClass("label label-success label-inline").text(app.localize('Enum_CardTransStatus_' + status));
                        } else if (status === 3) {
                            $span.addClass("label label-danger label-inline").text(app.localize('Enum_CardTransStatus_' + status));
                        } else if (status === 2) {
                            $span.addClass("label label-warning label-inline").text(app.localize('Enum_CardTransStatus_' + status));
                        }
                        else {
                            $span.addClass("label label-default label-inline").text(app.localize('Enum_CardTransStatus_' + status));
                        }
                        return $span[0].outerHTML;
                    }
                },
                {
                    targets: 5,
                    data: "itemValue",
                    name: "itemValue",
                    className: "all text-right",
                    render: function (d) {
                        return Sv.NumberToString(d);
                    }
                },
                {
                    targets: 6,
                    data: "quantity",
                    name: "quantity",
                    className: "all text-right",
                    render: function (d) {
                        return Sv.NumberToString(d);
                    }
                },
                {
                    targets: 7,
                    data: "totalPrice",
                    name: "totalPrice",
                    className: "all text-right",
                    render: function (d) {
                        return Sv.NumberToString(d);
                    }
                },
                {
                    targets: 8,
                    data: "serviceCode",
                    name: "serviceCode",
                },
                {
                    targets: 9,
                    data: "createdDate",
                    name: "createdDate",
                    render: function (expiredDate) {
                        if (expiredDate && expiredDate.indexOf("0001-01-01")) {
                            return moment(expiredDate).format('DD/MM/YYYY HH:mm:ss');
                        }
                        return "";
                    }
                },
                {
                    targets: 10,
                    data: "provider",
                    name: "provider", 
                },
                {
                    targets: 11,
                    data: "isSyncCard",
                    name: "isSyncCard",
                    render: function (isSyncCard) {
                        const $span = $("<span/>").addClass("label");
                        if (isSyncCard === true) {
                            $span.addClass("label label-success label-inline").text('Đã đồng bộ');
                       }
                        else {
                            $span.addClass("label label-danger label-inline").text('Chưa đồng bộ');
                        }
                        return $span[0].outerHTML;
                    }
                },
                {
                    targets: 12,
                    data: "expiredDate",
                    name: "expiredDate",
                    render: function (expiredDate) {
                        if (expiredDate && expiredDate.indexOf("0001-01-01")) {
                            return moment(expiredDate).format('DD/MM/YYYY');
                        }
                        return "";
                    }
                }
            ]
        });
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
           
            _cardsService.getCardStockTransToExcel({
                transCodeProvider: $('#TransCodeProviderFilterId').val(),
                batchCode: $('#BatchCodeFilterId').val(),
                fromDate: getDateFilter($('#fromDateFilterId'), false),
                toDate: getDateFilter($('#toDateFilterId'), true),
                provider: $('#providerCodeFilter').val(),
                status: $('#StatusFilterId').val(),
            }).done(function (result) {                   
                    app.downloadTempFile(result);
                });
        });

        function getListTransCards() {
            dataTable.ajax.reload();
        }
        $('#GetListCardsButtonSeach').click(function (e) {
            e.preventDefault();
            getListTransCards();
        });

        $('#GetListCardsButton').click(function (e) {
            e.preventDefault();
            getListTransCards();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getListTransCards();
            }
        });

        function checkTransProvider(data) {
            _cardsService.checkTransStockProvider(
                {                    
                    transCodeProvider: data.record.transCodeProvider,
                    provider: data.record.provider
                }
            ).done(function (rs) {
                if (rs.responseCode === "1") {
                    abp.message.info("Giao dịch thành công");
                    getListTransCards();
                } else if (rs.responseCode === "4007") {
                    abp.message.warn("Giao dịch chưa có kết quả");
                } else {
                    abp.message.error("GD lỗi: " + rs.responseMessage + "\nMã lỗi: " + rs.responseCode);
                }

            }).always(function () {
                //abp.setBusy(false);
            });
        }
    });
})();
