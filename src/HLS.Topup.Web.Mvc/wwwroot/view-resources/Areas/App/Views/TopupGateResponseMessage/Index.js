(function () {
    $(function () {
        var _TopuGateResponseMessageTable = $('#TopuGateResponseMessageTable');
        var _TopupGateResponseMessageAppService = abp.services.app.topupGateResponseMessage;

        $('.select2').select2();
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L LTS'
        });
        $('#ExportToExcelButton').click(function () {

            var fileUrl = '/assets/SampleFiles/ImportMaLoiNhaCungCapSample.xlsx';
            var a = document.createElement('a');
            a.href = fileUrl
            a.download = 'ImportMaLoiNhaCungCapSample.xlsx';
            document.body.appendChild(a);
            a.click();
            document.body.removeChild(a);
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.TopupGateResponseMessage.Create'),
            edit: abp.auth.hasPermission('Pages.TopupGateResponseMessage.Edit'),
            delete: abp.auth.hasPermission('Pages.TopupGateResponseMessage.Delete'),
        };

        var _createOrEditTopupGateResponseMessageModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/TopupGateResponseMessage/CreateOrEditTopupGateResponseMessageModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/TopupGateResponseMessage/_CreateOrEditTopupGateRM.js',
            modalClass: 'CrateOrEditTopupGateRM',
            modalSize: 'modal-lg'
        });

        var _viewTopupGateResponseMessageModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/TopupGateResponseMessage/ViewTopupGateResponseMessage',
            modalClass: 'ViewBankMessageConfigModal',
            modalSize: 'modal-lg'
        });

        var getDateFilter = function (element, isEnd = false) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }

            if (element.data("DateTimePicker").date() != null && isEnd) {
                return element.data("DateTimePicker").date().format("YYYY-MM-DDT23:59:59Z");
            }

            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z");
        }
        $('#excel-file').on('change', function (e) {
            var file = e.target.files[0];
            if (!file) return;
            var reader = new FileReader();
            reader.onload = function (e) {
                var data = e.target.result;
                var workbook = XLSX.read(data, {type: 'binary'});

                // Đọc dữ liệu từ sheet đầu tiên (index 0)
                var sheetName = workbook.SheetNames[0];
                var worksheet = workbook.Sheets[sheetName];
                var jsonData = XLSX.utils.sheet_to_json(worksheet);

                // Chuyển đổi dữ liệu JSON thành danh sách ProviderResponseDto
                var listTopupGateResponseMessage = [];
                jsonData.forEach(function (row) {
                    var bankMessageConfig = {
                        Provider: row["Nhà cung cấp"],
                        Code: row["Mã lỗi nhà cung cấp"],
                        Name: row["Thông báo lỗi nhà cung cấp"],
                        ReponseCode: row['Mã Lỗi NT'],
                        ReponseName: row["Thông báo lỗi NT"],
                        AddedAtUtc: new Date(),
                    };
                    listTopupGateResponseMessage.push(bankMessageConfig);
                    var requestData = {
                        ListProviderResponse: listTopupGateResponseMessage
                    };
                    abp.message.confirm(
                        'Bạn chắc chắn muốn import ProviderResponse này không?',
                        app.localize('AreYouSure'),
                        function (isConfirmed) {
                            if (isConfirmed) {
                                if (isConfirmed) {
                                    _TopupGateResponseMessageAppService.createListTopupGateRM(requestData).done(function () {
                                        abp.notify.success(app.localize('Successfully'));
                                        window.location.reload();
                                    });
                                }
                            }
                        }
                    );
                });

            };
            reader.readAsArrayBuffer(file);
        });
        var dataTable = _TopuGateResponseMessageTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            pagingType: 'full_numbers',
            listAction: {
                ajaxFunction: _TopupGateResponseMessageAppService.getListTopupGateResponseMessage,
                inputFilter: function () {
                    return {
                        provider: $('#ProviderFilter').val(),
                        responseCode: $('#ResponseCodeFilter').val(),
                        responseName: $('#ResponseNameFilter').val(),
                        name: $('#NameFilter').val(),
                        code: $('#CodeFilter').val(),
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
                                    _viewTopupGateResponseMessageModal.open({
                                        provider: data.record.provider,
                                        code: data.record.code
                                    });
                                }
                            },
                            {
                                text: app.localize('Edit'),
                                visible: function () {
                                    return _permissions.edit;
                                },
                                action: function (data) {
                                    _createOrEditTopupGateResponseMessageModal.open({
                                        provider: data.record.provider,
                                        code: data.record.code
                                    });
                                }
                            }, {
                                text: app.localize('Delete'),
                                visible: function () {
                                    return _permissions.delete;
                                },
                                action: function (data) {
                                    deleteAppPage(data.record);
                                }
                            }
                        ]
                    }
                },
                {
                    targets: 1,
                    data: "provider",
                    name: "provider"
                },
                {
                    targets: 2,
                    data: "code",
                    name: "code",
                    className: "all text-center",
                },
                {
                    targets: 3,
                    data: "name",
                    name: "name"
                },
                {
                    targets: 4,
                    data: "reponseCode",
                    name: "reponseCode",
                    className: "all text-center",

                },
                {
                    targets: 5,
                    data: "reponseName",
                    name: "reponseName",
                },
                {
                    targets: 6,
                    data: "addedAtUtc",
                    name: "addedAtUtc",
                    render: function (data, type, row) {
                        if (row.addedAtUtc) {
                            return moment(row.addedAtUtc).format('L LTS');
                        }
                        return "";
                    }
                }
            ]
        });

        function getTopupGateResponseMessage() {
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

        $('#CreateTopupGateResponseMessageButton').click(function () {
            _createOrEditTopupGateResponseMessageModal.open();
        });
        abp.event.on('app.CreateOrEditTopupGateResponseMessage', function (data) {
            getTopupGateResponseMessage();
        });

        function deleteAppPage(providerResponse) {
            abp.message.confirm(
                'Bạn chắc chắn muốn xoá ProviderResponse này không?',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        if (isConfirmed) {
                            _TopupGateResponseMessageAppService.deleteTopupGateRM(providerResponse.provider, providerResponse.code).done(function () {

                                getTopupGateResponseMessage();
                                abp.notify.success(app.localize('SuccessfullyDeleted'));
                            })
                        }
                    }
                }
            );
        }

        $('#GetTopupGateResponseMessageButton').click(function (e) {
            e.preventDefault();
            getTopupGateResponseMessage();
        });
        $(document).keypress(function (e) {
            if (e.which === 13) {
                getTopupGateResponseMessage();
            }
        });
    });
})();