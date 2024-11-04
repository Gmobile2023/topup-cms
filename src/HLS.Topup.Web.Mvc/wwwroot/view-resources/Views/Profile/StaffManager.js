﻿(function () {
    $(function () {
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        let getDateFilter = function (element) {
            return element.data("DateTimePicker").date() === null ? null : element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z");
        };

        let _permissions = {
            create: abp.auth.hasPermission('Pages.StaffManager.Create'),
            edit: abp.auth.hasPermission('Pages.StaffManager.Update'),
            delete: abp.auth.hasPermission('Pages.StaffManager.Delete')
        };

        let _$table = $('#Table');
        let _agentService = abp.services.app.agentService;
        let dataTable = _$table.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _agentService.getListUserStaff,
                inputFilter: function () {
                    return {
                        search: $("#inputFilter").val(),
                    };
                }
            },
            columnDefs: [
                {
                    targets: 0,
                    className: 'control responsive text-center',
                    orderable: false,
                    render: function () {
                        return '';
                    }
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
                        text: '<i class="fa fa-cog"></i>Hành động<span class="caret"></span>',
                        items: [
                            {
                                text: 'Sửa thông tin',
                                visible: function () {
                                    return _permissions.edit;
                                },
                                action: function (data) {
                                    editStaff(data.record.id);
                                }
                            },
                            {
                                text: 'Xoá',
                                visible: function () {
                                    return _permissions.delete;
                                },
                                action: function (data) {
                                    deleteStaff(data.record.bank);
                                }
                            }]
                    }
                },
                {
                    targets: 2,
                    data: "accountCode",
                },
                {
                    targets: 3,
                    data: "phoneNumber",
                },
                {
                    targets: 4,
                    data: "fullName",
                },
                {
                    targets: 5,
                    data: "limitAmount",
                    render: function (data) {
                        return data ?  Sv.format_number(data) : "0"
                    }
                },
                {
                    targets: 6,
                    data: null,
                    render: function (data) {
                        return moment(data).format("DD/MM/YYYY")
                    }
                },
                {
                    targets: 7,
                    data: "isActive",
                    render: function (data) {
                        return data ? "Hoạt động" : "Khoá"
                    }

                },
            ]
        });

        function editStaff(id) {
            window.location.href = '/Profile/EditStaff/' + id;
        }

        function deleteStaff(staff) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _agentService.delete({
                            id: staff.id
                        }).done(function () {
                            getTables();
                            abp.message.success(app.localize('SuccessfullyDeleted'));
                        });
                    }
                }
            );
        }

        function getTables() {
            dataTable.ajax.reload();
        }

        $('#btnSearch').click(function (e) {
            e.preventDefault();
            getTables();
        });

        $('#TableFilter').on('keydown', function (e) {
            if (e.keyCode !== 13) {
                return;
            }

            e.preventDefault();
            getTables();
        });

        abp.event.on('app.createOrEditAgentModalSaved', function () {
            getTables();
        });

        $("#TableFilter").focus();
    });
})();
