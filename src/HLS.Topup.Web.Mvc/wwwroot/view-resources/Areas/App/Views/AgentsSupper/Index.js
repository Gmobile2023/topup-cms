(function () {
    $(function () {

        var _$agentsTable = $('#AgentsSupperTable');
        var _agentService = abp.services.app.agentManagerment;

        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            edit: abp.auth.hasPermission('Pages.AgentsSupper.Edit'),
            new: abp.auth.hasPermission('Pages.AgentsSupper.New'),
            sendMail: abp.auth.hasPermission('Pages.AgentsSupper.SendMailTech'),
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/AgentsSupper/EditAgentSupperModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/AgentsSupper/_EditModal.js',
            modalClass: 'EditAgentSupperModal',
            modalSize: 'modal-xl'
        });


        var _viewAgentModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/AgentsSupper/ViewAgentSupperModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/AgentsSupper/_ViewAgentModal.js',
            modalClass: 'ViewAgentSupperModal',
            modalSize: 'modal-xl'
        });

        $('#CreateNewButton').click(function () {
            _createOrEditModal.open();
        });


        var dataTable = _$agentsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _agentService.getSupperAll,
                inputFilter: function () {
                    return {
                        accountAgent: $('#txtFullName').val(),
                        crossCheckPeriod: $('#SelectCrossCheckPeriod').val(),
                        status: $('#Status').val()
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
                                    _viewAgentModal.open({id: data.record.userId});
                                }
                            },
                            {
                                text: app.localize('Edit'),
                                visible: function () {
                                    return _permissions.edit;
                                },
                                action: function (data) {
                                    _createOrEditModal.open({id: data.record.userId});
                                }
                            },
                            {
                                text: app.localize('ReSendMail'),
                                visible: function () {
                                    return _permissions.sendMail;
                                },
                                action: function (data) {
                                    abp.message.confirm(
                                        'Bạn chắc chắn muốn gửi lại email thông tin kết nối?',
                                        app.localize('AreYouSure'),
                                        function (isConfirmed) {
                                            if (isConfirmed) {
                                                _agentService.resendEmailTech({id: data.record.userId}).done(function () {
                                                    abp.message.success('Gửi lại mail thành công');
                                                });
                                            }
                                        }
                                    );
                                }
                            }
                        ]
                    }
                },
                {
                    targets: 2,
                    data: "accountCode",
                    name: "accountCode"
                },
                {
                    targets: 3,
                    data: "fullName",
                    name: "fullName"
                },
                {
                    targets: 4,
                    data: "crossCheckPeriod",
                    name: "crossCheckPeriod"
                },
                {
                    targets: 5,
                    data: "statusName",
                    name: "statusName"
                },
                {
                    targets: 6,
                    data: "createdDate",
                    name: "createdDate",
                    render: function (createdTime) {
                        if (createdTime) {
                            return moment(new Date(createdTime)).format('DD/MM/YYYY HH:mm:ss');
                        }
                        return "";
                    }
                }
            ]
        });

        function getAgents() {
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

        $('#GetAgentsButton').click(function (e) {
            e.preventDefault();
            getAgents();
        });


        abp.event.on('app.createOrEditAgentSupperModalSaved', function () {
            getAgents();
        });

        $('#ExportToExcelButton').click(function () {
            _agentService.getAllAgentSupperListToExcel({
                accountAgent: $('#txtFullName').val(),
                crossCheckPeriod: $('#SelectCrossCheckPeriod').val(),
                status: $('#Status').val()
            })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });


        $(document).keypress(function (e) {
            if (e.which === 13) {
                getAgents();
            }
        });
    });
})();
