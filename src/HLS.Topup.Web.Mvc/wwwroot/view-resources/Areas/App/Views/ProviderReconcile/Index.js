(function () {
    $(function () {

        var _$providersTable = $('#tableCompareView');
        const _svcCompareService = abp.services.app.compare;
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            compare: abp.auth.hasPermission('Pages.ProviderReconcile.Compare'),            
        };


        abp.event.on('app.compareAirtimeFile', function () {
            getProviders();
        });

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ProviderReconcile/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/ProviderReconcile/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditModal',
            modalSize: 'modal-xl'  
        });

        var _viewDetailModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ProviderReconcile/ReponseModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/ProviderReconcile/_Reponse.js',
            modalClass: 'ReponseModal',
            modalSize: 'modal-xl'             
        });

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z");
        }

        const dataTable = _$providersTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _svcCompareService.getCompareServiceTotalList,
                inputFilter: function () {
                    return {
                        fromCompareDate: getDateFilter($('#FromDateCompare')),
                        toCompareDate: getDateFilter($('#TromDateCompare')),
                        fromTransDate: getDateFilter($('#FromDateTran')),
                        toTransDate: getDateFilter($('#TromDateTran')),
                        providerCode: $("#dropProvider").val()
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
                                action: function (data) {                                    
                                    _viewDetailModal.open({ providerCode: data.record.providerCode, date: data.record.transDate, keyCode: data.record.keyCode });
                                }
                            },
                            {
                                text: app.localize('Cancel'),
                                visible: function (data) {
                                    return false;
                                },
                                action: function (data) {
                                }
                            }
                        ]                       
                    }
                },
                {
                    targets: 2,
                    data: "compareDate",
                    render: function (compareDate) {
                        if (compareDate) {
                            return moment(compareDate).format('L LTS');
                        }
                        return "";
                    }
                },
                {
                    targets: 3,
                    data: "transDate",
                    render: function (createdTime) {
                        if (createdTime) {
                            return moment(new Date(createdTime)).format('DD/MM/YYYY');
                        }
                        return "";
                    }
                },
                {
                    targets: 4,
                    data: "providerCode"
                },
                {
                    targets: 5,
                    data: "sysFileName"
                },
                {
                    targets: 6,
                    data: "providerFileName"
                },
                {
                    targets: 7,
                    data: "sysQuantity",
                    render: function (data) {
                        return Sv.format_number(data);
                    }
                },
                {
                    targets: 8,
                    data: "sysAmount",
                    render: function (data) {
                        return Sv.format_number(data);
                    }
                },
                {
                    targets: 9,
                    data: "providerQuantity",
                    render: function (data) {
                        return Sv.format_number(data);
                    }
                },
                {
                    targets: 10,
                    data: "providerAmount",
                    render: function (data) {
                        return Sv.format_number(data);
                    }
                },
                {
                    targets: 11,
                    data: "sameQuantity",
                    render: function (data) {
                        return Sv.format_number(data);
                    }
                },
                {
                    targets: 12,
                    data: "notSameQuantity",
                    render: function (data) {
                        return Sv.format_number(data);
                    }
                },
                {
                    targets: 13,
                    data: "sysOnlyQuantity",
                    render: function (data) {
                        return Sv.format_number(data);
                    }
                },
                {
                    targets: 14,
                    data: "providerOnlyQuantity",
                    render: function (data) {
                        return Sv.format_number(data);
                    }
                },
                {
                    targets: 15,
                    data: "accountCompare"
                }
            ],
            "headerCallback": function (thead, data, start, end, display) {
                try {
                    var rawServerResponse = this.api().settings()[0].rawServerResponse;
                    if (rawServerResponse !== undefined && rawServerResponse.totalData !== undefined) {
                        $(thead).find('th').eq(1).addClass("text-left").html(Sv.format_number(rawServerResponse.totalData.sysQuantity));
                        $(thead).find('th').eq(2).addClass("text-left").html(Sv.format_number(rawServerResponse.totalData.sysAmount));
                        $(thead).find('th').eq(3).addClass("text-left").html(Sv.format_number(rawServerResponse.totalData.providerQuantity));
                        $(thead).find('th').eq(4).addClass("text-left").html(Sv.format_number(rawServerResponse.totalData.providerAmount));
                        $(thead).find('th').eq(5).addClass("text-left").html(Sv.format_number(rawServerResponse.totalData.sameQuantity));
                        $(thead).find('th').eq(6).addClass("text-left").html(Sv.format_number(rawServerResponse.totalData.notSameQuantity));
                        $(thead).find('th').eq(7).addClass("text-left").html(Sv.format_number(rawServerResponse.totalData.sysOnlyQuantity));
                        $(thead).find('th').eq(8).addClass("text-left").html(Sv.format_number(rawServerResponse.totalData.providerOnlyQuantity));
                    }
                } catch (e) {
                    console.log("không có total")
                }
            }
        });

        function getProviders() {
            dataTable.ajax.reload();
        }

        $('#CreateNewProviderReconcileButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportToExcelButton').click(function () {
            _svcCompareService.getCompareServiceTotalListToExcel({
                fromCompareDate: getDateFilter($('#FromDateCompare')),
                toCompareDate: getDateFilter($('#TromDateCompare')),
                fromTransDate: getDateFilter($('#FromDateTran')),
                toTransDate: getDateFilter($('#TromDateTran')),
                providerCode: $("#dropProvider").val()
            })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });


        

        $('#GetProvidersButton').click(function (e) {
            e.preventDefault();
            getProviders();
        });

        //$('#ResetButton').click(function (e) {
        //    alert('s');
        //    $("#dropProvider").val("");
        //});


        $(document).keypress(function (e) {
            if (e.which === 13) {
                getProviders();
            }
        });
    });
})();
