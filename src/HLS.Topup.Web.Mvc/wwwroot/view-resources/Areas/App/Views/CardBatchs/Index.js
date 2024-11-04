(function () {
    $(function () {

        var _$cardBatchsTable = $('#CardBatchsTable');
        var _cardBatchsService = abp.services.app.cardBatchs;

        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.CardBatchs.Create'),
            edit: abp.auth.hasPermission('Pages.CardBatchs.Edit'),
            'delete': abp.auth.hasPermission('Pages.CardBatchs.Delete')
        };
 
        var _importCardsModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Cards/ImportCardsModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Cards/_ImportCardsModal.js',
            modalClass: 'ImportCardsModal',
            modalSize: 'modal-xl'
        });

        var _importCardsApiModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Cards/CreateCardsApiModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Cards/_CreateCardsApiModal.js',
            modalClass: 'CreateCardsApiModal',
            modalSize: 'modal-xl'
        });


        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/CardBatchs/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/CardBatchs/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditCardBatchModal',
            modalSize: 'modal-xl'
        });

        var _viewCardBatchModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/CardBatchs/ViewcardBatchModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/CardBatchs/_viewModal.js',
            modalClass: 'ViewCardBatchModal',
            modalSize: 'modal-xl'
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


        var dataTable = _$cardBatchsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _cardBatchsService.getAll,
                inputFilter: function () {
                    return {
                        filter: $('#CardBatchsTableFilter').val(),
                        batchCodeFilter: $('#BatchCodeFilterId').val(),

                        minCreatedDateFilter: getDateFilter($('#MinCreatedDateFilterId'), false),
                        maxCreatedDateFilter: getDateFilter($('#MaxCreatedDateFilterId'), true),
                        statusFilter: $('#StatusFilterId').val(),
                        importTypeFilter: $('#ImportTypeFilterId').val(),
                        //vendorFilter: $('#vendorCodeFilter').val(),
                        providerFilter: $('#providerCodeFilter').val(),
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
                                    _viewCardBatchModal.open({id: data.record.id});
                                }
                            },
                            // {
                            //     text: app.localize('Edit'),
                            //     visible: function () {
                            //         return _permissions.edit;
                            //     },
                            //     action: function (data) {
                            //         _createOrEditModal.open({id: data.record.id});
                            //     }
                            // },
                            // {
                            //     text: app.localize('ImportFromExcel'),
                            //     visible: function () {
                            //         return _permissions.create;
                            //     },
                            //     action: function (data) {
                            //         _importCardsModal.open({ batchCode: data.record.batchCode });
                            //     }
                            // },
                            {
                                text: app.localize('Delete'),
                                visible: function (data) {
                                    return  (data.record.status != 1) && _permissions.delete;
                                },
                                action: function (data) {
                                    deleteCardBatch(data.record);
                                }
                            }
                        ]
                    }
                },
                {
                    targets: 1,
                    data: "createdDate",
                    name: "createdDate",
                    render: function (createdDate) {
                        if (createdDate) {
                            return moment(createdDate).format('L LTS');
                            //return moment(createdDate).format('DD/MM/YYYY HH:mm:ss');
                        }
                        return "";
                    }
                },
                {
                    targets: 2,
                    data: "batchCode",
                    name: "batchCode"
                },
                {
                    targets: 3,
                    data: "providerName",
                    name: "providerName"
                },  
                {
                    targets: 5,
                    class: "text-right",
                    data: "totalAmount",
                    name: "totalAmount",
                    render: function (data, e, row) { 
                        if(data && data > 0)
                        return Sv.NumberToString(data) + "đ";
                        return 0;
                    }
                },
                {
                    targets: 4,
                    class: "text-right",
                    data: "totalQuantity",
                    name: "totalQuantity",
                    render: function (data, e, row) {
                        if(data && data > 0)
                            return Sv.NumberToString(data) ;
                        return 0; 
                    }
                },
                {
                    targets: 6,
                    class: "text-right",
                    data: "importType",
                    name: "importType", 
                },
                {
                    targets: 7,
                    data: "status",
                    name: "status",
                    render: function (status) {
                        return app.localize('Enum_CardPackageStatus_' + status);
                    }
                }
            ]
        });

        function getCardBatchs() {
            dataTable.ajax.reload();
        }

        function deleteCardBatch(cardBatch) {
            if(cardBatch.status != 0){
                abp.notify.error(app.localize('StatusNotValid'));
                return false;
            }
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _cardBatchsService.delete(cardBatch.id.toString()).done(function () {
                            getCardBatchs(true);
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

        $('#CreateNewCardBatchButton').click(function () {
            _createOrEditModal.open();
        });
 

        $('#ImportCardsButton').click(function () {
            _importCardsModal.open();
        });
        $('#ImportCardsApiButton').click(function () {
            _importCardsApiModal.open();
        });

        $('#ExportToExcelButton').click(function () {
            _cardBatchsService
                .getCardBatchsToExcel({
                    filter: $('#CardBatchsTableFilter').val(),
                    batchCodeFilter: $('#BatchCodeFilterId').val(),

                    minCreatedDateFilter: getDateFilter($('#MinCreatedDateFilterId'), false),
                    maxCreatedDateFilter: getDateFilter($('#MaxCreatedDateFilterId'), true),
                    statusFilter: $('#StatusFilterId').val(),
                    importTypeFilter: $('#ImportTypeFilterId').val(),
                    //vendorFilter: $('#vendorCodeFilter').val(),
                    providerFilter: $('#providerCodeFilter').val(),
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        // add card trigger
        abp.event.on('app.createOrEditCardBatchModalSaved', function () {
            getCardBatchs();
        });
        // import file trigger
        abp.event.on('app.createCardsModalSaved', function () {
            getCardBatchs();
        });
        // import api trigger
        abp.event.on('app.importCardsApiSaved', function () {
            getCardBatchs();
        });


        $('#GetCardBatchsButton').click(function (e) {
            e.preventDefault();
            getCardBatchs();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getCardBatchs();
            }
        });
       
    });
})();
