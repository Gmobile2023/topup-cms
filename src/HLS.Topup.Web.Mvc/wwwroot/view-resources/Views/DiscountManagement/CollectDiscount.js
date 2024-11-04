(function () {
    $(function () {

        var _$table = $('#Table');
        var _discountsService = abp.services.app.transactions;

        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });


        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z");
        }

        var dataTable = _$table.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _discountsService.getCollectLevelDiscounts,
                inputFilter: function () {
                    return {
                        filter: $('#TableFilter').val(),
                        fromDate: getDateFilter($('#FromDateFilterId')),
                        toDate: getDateFilter($('#ToDateFilterId')),
                        search: $("#TableFilter").val()
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
                    data: "status",
                    name: "status",
                    render: function (status) {
                        return app.localize('Enum_LevelDiscount_' + status);
                    }
                },
                {
                    targets: 3,
                    data: "transCode",
                    name: "transCode"
                },
                {
                    targets: 4,
                    data: "fullTransAcount",
                    name: "fullTransAcount"
                },
                {
                    targets: 5,
                    data: "partnerTransCode",
                    name: "partnerTransCode"
                },
                {
                    targets: 6,
                    data: "transAmount",
                    name: "transAmount",
                    className: "text-right",
                    render: function (transAmount) {
                        if (transAmount) {
                            return format_number(transAmount)
                        }
                        return "0";
                    }
                },
                {
                    targets: 7,
                    data: "levelDiscountRate",
                    name: "levelDiscountRate",
                    className: "text-right",
                    render: function (levelDiscountRate) {
                        if (levelDiscountRate) {
                            return format_number(levelDiscountRate)
                        }
                        return "0";
                    }
                },
                {
                    targets: 8,
                    data: "levelDiscountAmount",
                    name: "levelDiscountAmount",
                    className: "text-right",
                    render: function (levelDiscountAmount) {
                        if (levelDiscountAmount) {
                            return format_number(levelDiscountAmount)
                        }
                        return "0";
                    }
                }
            ]
        });


        function getDiscounts() {
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

        $('#btnCollectDiscount').click(function () {
            var fromDate = $("#FromDateFilterId").val();
            var todate = $("#ToDateFilterId").val();
            var discount = format_number($("#txtTotalDiscount").val());
            abp.message.confirm("Bạn có muốn thu hoạch lãi từ ngày: " + fromDate + " tới ngày: " + todate +". Số tiền thu hoạch là: "+discount).then(function (confirmed) {
                if (confirmed) {
                    abp.ui.setBusy();
                    _discountsService.collectDiscount({
                        filter: $('#TableFilter').val(),
                        fromDate: getDateFilter($('#FromDateFilterId')),
                        toDate: getDateFilter($('#ToDateFilterId')),
                        search: $("#TableFilter").val()
                    }).done(function (rs) {
                        if (rs.responseCode === "01") {
                            console.log(rs.extraInfo.replace('.',','));
                            console.log(rs.payload.replace('.',','));
                            abp.message.success("Thu hoạch lãi thành công. Số tiền lãi thu hoạch được là: " + format_number(rs.payload.replace('.',',')) +". Số dư sau giao dịch là: "+ format_number(rs.extraInfo.replace('.',',')));
                            //location.reload(true);
                        } else {
                            abp.notify.error(rs.responseMessage);
                        }
                    }).always(function () {
                        abp.ui.clearBusy();
                    });
                }
            });
        });


        abp.event.on('app.collectDiscountSaved', function () {
            getDiscounts();
        });

        $('#GetDiscountsButton').click(function (e) {
            e.preventDefault();
            getDiscounts();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getDiscounts();
            }
        });

    });
})();