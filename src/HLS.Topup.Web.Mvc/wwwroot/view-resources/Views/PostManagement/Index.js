(function () {
    $(function () {

        var _$agentsTable = $('#AgentsTable');
        var _agentService = abp.services.app.postManagement;

        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            edit: abp.auth.hasPermission('Pages.PostManagement.Edit')
        };

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z");
        }

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'PostManagement/CreateOrUpdate',
            scriptUrl: abp.appPath + 'view-resources/Views/PostManagement/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditPostModal',
            modalSize: 'modal-xl'
        });

        var _lockOrUnlockModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/AgentsManage/LockOrUnlockAgentModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/AgentsManage/_LockOrUnlockAgent.js',
            modalClass: 'LockOrUnlockModal',
            //modalSize: 'modal-xl'
        });

        var dataTable = _$agentsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _agentService.getAll,
                inputFilter: function () {
                    return {
                        filter: $('#inputFilter').val()
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
                                    window.location.href = '/PostManagement/Detail?userId=' + data.record.id + '&isView=true'
                                }
                            },
                            {
                                text: app.localize('Edit'),
                                visible: function () {
                                    return _permissions.edit;
                                },
                                action: function (data) {
                                    window.location.href = '/PostManagement/Detail?userId=' + data.record.id;
                                }
                            },
                            {
                                text: function (data) {
                                    return data.status === 1 ? app.localize('Lock') : app.localize('Unlock');
                                },
                                visible: function (data) {
                                    return (_permissions.lock || _permissions.unlock) && (data.record.status === 1 || data.record.status === 2) //? _permissions.lock : _permissions.unlock;
                                },
                                action: function (data) {
                                    if (data.record.status === 1) {
                                        _lockOrUnlockModal.open({id: data.record.id, type: 'lock'});
                                    } else if (data.record.status === 2) {
                                        _lockOrUnlockModal.open({id: data.record.id, type: 'unlock'});
                                    }
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
                    data: "phoneNumber",
                    name: "phoneNumber"
                },
                {
                    targets: 4,
                    data: "agentName",
                    name: "agentName"
                },
                {
                    targets: 5,
                    data: "creationTime",
                    name: "creationTime",
                    render: function (createdTime) {
                        if (createdTime) {
                            return moment(new Date(createdTime)).format('DD/MM/YYYY HH:mm:ss');
                        }
                        return "";
                    }
                },
                {
                    targets: 6,
                    data: "isActive",
                    name: "isActive",
                    render: function (isActive) {
                        return isActive === true ? 'Hoạt động' : 'Khoá';
                    }

                }
            ]
        });

        function getAgents() {
            dataTable.ajax.reload();
        }

        $('#btnCreate').click(function () {
            _createOrEditModal.open();
        });
        $('#btnSearch').click(function (e) {
            e.preventDefault();
            getAgents();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getAgents();
            }
        });

    });
})();
