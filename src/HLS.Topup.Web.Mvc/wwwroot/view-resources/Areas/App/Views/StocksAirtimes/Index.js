(function () {
    $(function () {

        var _$stocksAirtimesTable = $('#StocksAirtimesTable');
        var _stocksAirtimesService = abp.services.app.stocksAirtimes;

        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.StocksAirtimes.Create'),
            edit: abp.auth.hasPermission('Pages.StocksAirtimes.Edit'),
            deposit: abp.auth.hasPermission('Pages.StocksAirtimes.Deposit'),
            'delete': abp.auth.hasPermission('Pages.StocksAirtimes.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/StocksAirtimes/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/StocksAirtimes/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditStocksAirtimeModal'
        });
        var _depositAirtimeModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/StocksAirtimes/DepositAirtimeModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/StocksAirtimes/_DepositAirtimeModal.js',
            modalClass: 'DepositAirtimeModal'
        });


        var _viewStocksAirtimeModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/StocksAirtimes/ViewstocksAirtimeModal',
            modalClass: 'ViewStocksAirtimeModal'
        });

        var _queryStocksAirtimeModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/StocksAirtimes/QueryStocksAirtimeModal',
            modalClass: 'QueryStocksAirtimeModal'
        });

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z");
        }

        var dataTable = _$stocksAirtimesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _stocksAirtimesService.getAll,
                inputFilter: function () {
                    return {
                        filter: $('#StocksAirtimesTableFilter').val(),
                        providerCodeFilter: $('#providerCode').val(),
                    };
                }
            },
            columnDefs: [
                {
                    width: 120,
                    targets: 0,
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
                                action: function (data) {
                                    _viewStocksAirtimeModal.open({code: data.record.keyCode});
                                }
                            },
                            {
                                text: app.localize('Query'),
                                action: function (data) {
                                    abp.ui.setBusy();
                                    abp.services.app.stocksAirtimes.query(data.record.keyCode.trim())
                                        .done(function (data) {
                                            abp.ui.clearBusy();
                                            let str = data && data.responseCode === "1" ? Sv.NumberToString(data.payload) : "0" ;
                                            abp.message.success('Số dư thực tế: ' + str);
                                        }).catch(function () {
                                        abp.ui.clearBusy();
                                    });
                                }
                            },
                            {
                                text: app.localize('Edit'),
                                visible: function () {
                                    return _permissions.edit;
                                },
                                action: function (data) {
                                    _createOrEditModal.open({code: data.record.keyCode});
                                }
                            },
                            {
                                text: app.localize('Nhập số dư'),
                                visible: function (data) {
                                  
                                    return _permissions.deposit && (data.record.keyCode == "VIETTEL" || data.record.keyCode == "VIETTEL2-VT" || data.record.keyCode == "VIETTEL2");
                                },
                                action: function (data) {
                                    _depositAirtimeModal.open({ keyCode: data.record.keyCode, providerCode: data.record.keyCode });
                                }
                            }
                        ]
                    }
                },
                {
                    targets: 1,
                    data: "providerCode",
                    name: "providerCode",
                    render: function (data, e, row) {
                        //console.log(row);
                        return row.providerCode + " - " + row.providerName
                    }
                },
                {
                    targets: 2,
                    data: "totalAirtime",
                    class: "text-right",
                    name: "totalAirtime",
                    render: function (data, e, row) {
                        return Sv.NumberToString(data)
                    }
                },
                {
                    targets: 3,
                    data: "minLimitAirtime",
                    class: "text-right",
                    name: "minLimitAirtime",
                    render: function (data, e, row) {
                        return Sv.NumberToString(data)
                    }
                },
                {
                    targets: 4,
                    data: "maxLimitAirtime",
                    class: "text-right",
                    name: "maxLimitAirtime",
                    render: function (data, e, row) {
                        return Sv.NumberToString(data)
                    }
                },
            ]
        });


        function getStocksAirtimes() {
            dataTable.ajax.reload();
        }

        function deleteStocksAirtime(stocksAirtime) {
            abp.message.confirm(
                '',
                function (isConfirmed) {
                    if (isConfirmed) {
                        _stocksAirtimesService.delete({
                            id: stocksAirtime.id
                        }).done(function () {
                            getStocksAirtimes(true);
                            abp.notify.success(app.localize('SuccessfullyDeleted'));
                        });
                    }
                }
            );
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

        $('#CreateNewStocksAirtimeButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportToExcelButton').click(function () {
            _stocksAirtimesService
                .getStocksAirtimesToExcel({
                    filter: $('#StocksAirtimesTableFilter').val(),
                    providerCodeFilter: $('#providerCode').val(),
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditStocksAirtimeModalSaved', function () {
            getStocksAirtimes();
        });

        $('#GetStocksAirtimesButton').click(function (e) {
            getStocksAirtimes();
            e.preventDefault();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getStocksAirtimes();
            }
        });


    });
})();

function queryBalance(e) {
    let $p = $(e.target).closest('.mb-3');
    let code = $p.find('button').data('code');
    abp.ui.setBusy();
    abp.services.app.stocksAirtimes.query(code.trim())
        .done(function (data) {
            abp.ui.clearBusy();
            let str = data && data.responseCode === "1" ? Sv.NumberToString(data.payload) : "0" ;
            $p.find(".q_airtime").html(str);
        }).catch(function () {
            $p.find(".q_airtime").html("Không xác định");
            abp.ui.clearBusy();
        });
}
