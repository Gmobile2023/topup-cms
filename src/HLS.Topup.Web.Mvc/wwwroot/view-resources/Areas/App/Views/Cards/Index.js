(function () {
    $(function () {

        var _$cardsTable = $('#CardsTable');
        var _cardsService = abp.services.app.cards;

        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Cards.Create'),
            edit: abp.auth.hasPermission('Pages.Cards.Edit'),
            'delete': abp.auth.hasPermission('Pages.Cards.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Cards/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Cards/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditCardModal',
            modalSize: 'modal-xl'
        });

        var _viewCardModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Cards/ViewcardModal',
            modalClass: 'ViewCardModal',
            modalSize: 'modal-xl'
        });

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

        var getDateFilter = function (element, isEnd) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            if (isEnd) {
                return element.data("DateTimePicker").date().format("YYYY-MM-DDT23:59:59Z");
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z");
        }

        var dataTable = _$cardsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _cardsService.getAll,
                inputFilter: function () {
                    return {
                        filter: "",
                        batchCodeFilter: $('#BatchCodeFilterId').val(),
                        serialFilter: $('#SerialFilterId').val(),
                        minExpiredDateFilter: getDateFilter($('#MinExpiredDateFilterId'), false),
                        maxExpiredDateFilter: getDateFilter($('#MaxExpiredDateFilterId'), true),

                        providerCodeFilter: $('#providerCodeFilter').val(),
                        stockCodeFilter: $('#stockCodeFilter').val(),

                        serviceCodeFilter: $('#serviceCodeFilter').val(),
                        categoryCodeFilter: $('#categoryCodeFilter').val(),

                        minCardValueFilter: $('#cardValueFilter').val(),
                        maxCardValueFilter: $('#cardValueFilter').val(),

                        minImportedDateFilter: getDateFilter($('#MinImportedDateFilterId'), false),
                        maxImportedDateFilter: getDateFilter($('#MaxImportedDateFilterId'), true),

                        minExportedDateFilter: getDateFilter($('#MinExportedDateFilterId'), false),
                        maxExportedDateFilter: getDateFilter($('#MaxExportedDateFilterId'), true),

                        statusFilter: $('#StatusFilterId').val(),
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
                                    _viewCardModal.open({ id: data.record.id });
                                }
                            },
                            {
                                text: app.localize('Edit'),
                                visible: function (data) {
                                    return _permissions.edit && data.record.status !== 2 && data.record.status !== 3 && data.record.status !== 4;
                                },
                                action: function (data) {
                                    _createOrEditModal.open({ id: data.record.id });
                                }
                            },
                            {
                                text: app.localize('Delete'),
                                visible: function (data) {
                                    return _permissions.delete && data.record.status !== 2 && data.record.status !== 3 && data.record.status !== 4;
                                },
                                action: function (data) {
                                    deleteCard({ id: data.record.id });
                                }
                            }]
                    }
                },
                {
                    targets: 2,
                    data: "serial",
                    name: "serial"
                },
                {
                    targets: 3,
                    data: "cardValue",
                    name: "cardValue",
                    className: "all text-right",
                    render: function (d) {
                        return Sv.NumberToString(d) + "đ";
                    }
                },
                {
                    targets: 4,
                    data: "serviceName",
                    name: "serviceName",
                },
                {
                    targets: 5,
                    data: "categoryName",
                    name: "categoryName",
                },
                {
                    targets: 6,
                    data: "providerName",
                    name: "providerName"
                },
                {
                    targets: 7,
                    data: "batchCode",
                    name: "batchCode"
                },
                {
                    targets: 8,
                    data: "expiredDate",
                    name: "expiredDate",
                    render: function (expiredDate) {
                        if (expiredDate && expiredDate.indexOf("0001-01-01")) {
                            return moment(expiredDate).format('DD/MM/YYYY');
                        }
                        return "";
                    }
                },
                {
                    targets: 9,
                    data: "status",
                    name: "status",
                    render: function (status) {
                        return app.localize('Enum_CardStatus_' + status);
                    }
                },
                {
                    targets: 10,
                    data: "stockCode",
                    name: "stockCode",
                },
                {
                    targets: 11,
                    data: "importedDate",
                    name: "importedDate",
                    render: function (importedDate) {
                        if (importedDate && importedDate.indexOf("0001-01-01")) {
                            return moment(importedDate).format('DD/MM/YYYY HH:mm:ss');
                        }
                        return "";
                    }
                },
                {
                    targets: 12,
                    data: "exportedDate",
                    name: "exportedDate",
                    render: function (exportedDate) {
                        if (exportedDate && exportedDate.indexOf("0001-01-01")) {
                            return moment(exportedDate).format('DD/MM/YYYY HH:mm:ss');
                        }
                        return "";
                    }
                },
            ]
        });

        function getCards() {
            dataTable.ajax.reload();
        }

        function deleteCard(card) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _cardsService.delete({
                            id: id
                        }).done(function () {
                            getCards(true);
                            abp.message.success(app.localize('SuccessfullyDeleted'));
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

        $('#CreateNewCardButton').click(function () {
            _createOrEditModal.open();
        });
        $('#ImportCardsButton').click(function () {
            _importCardsModal.open();
        });
        $('#ImportCardsApiButton').click(function () {
            _importCardsApiModal.open();
        });

        $('#ExportToExcelButton').click(function () {
            //abp.ui.setBusy(true);
            _cardsService
                .getCardsToExcel({
                    filter: "",
                    batchCodeFilter: $('#BatchCodeFilterId').val(),
                    serialFilter: $('#SerialFilterId').val(),
                    minExpiredDateFilter: getDateFilter($('#MinExpiredDateFilterId'), false),
                    maxExpiredDateFilter: getDateFilter($('#MaxExpiredDateFilterId'), true),

                    providerCodeFilter: $('#providerCodeFilter').val(),
                    stockCodeFilter: $('#stockCodeFilter').val(),

                    serviceCodeFilter: $('#serviceCodeFilter').val(),
                    categoryCodeFilter: $('#categoryCodeFilter').val(),

                    minCardValueFilter: $('#cardValueFilter').val(),
                    maxCardValueFilter: $('#cardValueFilter').val(),

                    minImportedDateFilter: getDateFilter($('#MinImportedDateFilterId'), false),
                    maxImportedDateFilter: getDateFilter($('#MaxImportedDateFilterId'), true),

                    minExportedDateFilter: getDateFilter($('#MinExportedDateFilterId'), false),
                    maxExportedDateFilter: getDateFilter($('#MaxExportedDateFilterId'), true),

                    statusFilter: $('#StatusFilterId').val(),
                })
                .done(function (result) {
                    //abp.ui.clearBusy();
                    app.downloadTempFile(result);
                });
        });

        // add card trigger
        abp.event.on('app.createOrEditCardModalSaved', function () {
            getCards();
        });
        // import file trigger
        abp.event.on('app.importCardsFileSaved', function () {
            getCards();
        });
        // import api trigger
        abp.event.on('app.importCardsApiSaved', function () {
            getCards();
        });

        $('#GetCardsButton').click(function (e) {
            e.preventDefault();
            getCards();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getCards();
            }
        });

        $('#AdvacedAuditFiltersArea #serviceCodeFilter').on('change', serviceChange);
        $('#AdvacedAuditFiltersArea #categoryCodeFilter').on('change', categoryChange);

        function serviceChange(e) {
            var serviceCode = $(e.target).val();
            abp.services.app.cards.categoryCardList(serviceCode, true)
                .done(function (result) {
                    let html = "<option value=\"\">Chọn loại sản phẩm</option>";
                    let _s = $("#AdvacedAuditFiltersArea");
                    if (result.length > 0) {
                        for (let i = 0; i < result.length; i++) {
                            let item = result[i];
                            html += ("<option value=\"" + item.id + "\">" + item.displayName + " </option>");
                        }
                    }
                    _s.find("#categoryCodeFilter").html(html);
                    _s.find("#cardValueFilter").html('<option value=\"\">Chọn mệnh giá</option>');
                })
                .always(function () {
                    abp.ui.clearBusy();
                });
        };
        function categoryChange(e) {
            var cateCode = $(e.target).val();
            abp.services.app.cards.getProductByCategory(cateCode)
                .done(function (result) {
                    let html = "<option value=\"\">Chọn mệnh giá</option>";
                    let _s = $("#AdvacedAuditFiltersArea");
                    if (result.length > 0) {
                        for (let i = 0; i < result.length; i++) {
                            let item = result[i];
                            html += ("<option value=\"" + item.productValue + "\">" + Sv.NumberToString(item.productValue) + "đ </option>");
                        }
                    }
                    _s.find("#cardValueFilter").html(html);
                })
                .always(function () {
                    abp.ui.clearBusy();
                });
        };

    });
})();
