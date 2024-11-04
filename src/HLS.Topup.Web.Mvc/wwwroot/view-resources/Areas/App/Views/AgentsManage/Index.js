(function () {
    $(function () {

        var _$agentsTable = $('#AgentsTable');
        var _agentService = abp.services.app.agentManagerment;
        var _userService = abp.services.app.user;
        var _commonLookupService = abp.services.app.commonLookup;

        var _citiesService = abp.services.app.cities;
        var _districtsService = abp.services.app.districts;
        var _wardsService = abp.services.app.wards;

        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
              edit: abp.auth.hasPermission('Pages.AgentsManage.Edit'),
              lock: abp.auth.hasPermission('Pages.AgentsManage.Lock'),
            unlock: abp.auth.hasPermission('Pages.AgentsManage.Unlock'),
            assign: abp.auth.hasPermission('Pages.AgentsManage.Assign'),
           convert: abp.auth.hasPermission('Pages.AgentsManage.ConvertPhone')
        };

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z");
        }

        var _editModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/AgentsManage/EditAgentModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/AgentsManage/_EditModal.js',
            modalClass: 'EditAgentModal',
            modalSize: 'modal-xl'
        });

        var _viewAgentModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/AgentsManage/ViewAgentModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/AgentsManage/_ViewAgentModal.js',
            modalClass: 'ViewAgentModal',
            modalSize: 'modal-xl'
        });

        var _mappingSaleModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/AgentsManage/MappingSaleModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/AgentsManage/_MappingSale.js',
            modalClass: 'MappingSaleModal',
            modalSize: 'modal-xl'
        });

        var _editMapSaleModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/AgentsManage/EditMapSaleModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/AgentsManage/_MappingSale.js',
            modalClass: 'MappingSaleModal',
            modalSize: 'modal-xl'
        });

        var _lockOrUnlockModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/AgentsManage/LockOrUnlockAgentModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/AgentsManage/_LockOrUnlockAgent.js',
            modalClass: 'LockOrUnlockModal',
            //modalSize: 'modal-xl'
        });

        var _convertPhoneModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/AgentsManage/ConvertPhoneModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/AgentsManage/_ConvertPhoneModal.js',
            modalClass: 'ConvertPhoneModal',
            modalSize: 'modal-xl'
        });
        
        /* Create or Edit Agent */
        var _createOrEditAgentGeneralModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/AgentsManage/CreateOrEditAgentModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/AgentsManage/_CreateOrEditAgentModal.js',
            modalClass: 'AgentGeneralModal',
        });

        var dataTable = _$agentsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _agentService.getAll,
                inputFilter: function () {
                    return {
                        filter: $('#SaleMansTableFilter').val(),
                        fromDateFilter: getDateFilter($('#FromDateFilter')),
                        toDateFilter: getDateFilter($('#ToDateFilter')),
                        agentTypeFilter: $('#AgentTypeFilter').val(),
                        saleLeadFilter: $('#SaleLeaderFilter').val(),
                        managerFilter: $('#SaleStaffFilter').val(),
                        agentId: $('#AgentNameFilter').val(),
                        exhibitFilter: $('#ExhibitFilter').val(),
                        province: $('#ProvinceFilter').val(),
                        district: $('#DistrictFilter').val(),
                        village: $('#VillageFilter').val(),
                        status: $('#StatusFilter').val(),
                        isMapSale: $('#MappingFilter').val()
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
                                    _viewAgentModal.open({id: data.record.id});
                                }
                            },
                            {
                                text: app.localize('Edit'),
                                visible: function () {
                                    return _permissions.edit;
                                },
                                action: function (data) {
                                    let agentTypeTemplate = {
                                        4: 'AgentGeneral',
                                        5: 'WholesaleAgent',
                                        6: 'SubAgent'
                                    };
                                    console.log(data.record.agentType);
                                    if (data.record.agentType === 4 || data.record.agentType === 5 || data.record.agentType === 6)
                                        _createOrEditAgentGeneralModal.open({id: data.record.id, type: agentTypeTemplate[data.record.agentType]});
                                    else
                                        _editModal.open({id: data.record.id});
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
                            },
                            {
                                text: app.localize('MappingSale'),
                                visible: function (data) {
                                    return _permissions.assign && data.record.isMapSale === false;
                                },
                                action: function (data) {
                                    _mappingSaleModal.open({id: data.record.id});
                                }
                            },
                            {
                                text: app.localize('EditMappingSale'),
                                visible: function (data) {
                                    return _permissions.assign && data.record.isMapSale === true;
                                },
                                action: function (data) {
                                    _editMapSaleModal.open({id: data.record.id});
                                }
                            },
                            {
                                text: app.localize('Thay đổi số điện thoại'),
                                visible: function (data) {
                                    return _permissions.convert;
                                },
                                action: function (data) {
                                    _convertPhoneModal.open({ id: data.record.id });
                                }
                            },
                            {
                                text: 'Reset lần gửi ODP',
                                visible: function (data) {
                                    return _permissions.edit;
                                },
                                action: function (data) {
                                    resetOdp(data);
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
                    data: "fullName",
                    name: "fullName"
                },
                {
                    targets: 5,
                    data: "agentType",
                    name: "agentType",
                    render: function (type) {
                        
                        return app.localize('Enum_AgentType_' + type);
                    }
                },
                {
                    targets: 6,
                    data: "agentGeneral",
                    name: "agentGeneral",
                    render: function (data, type, row) {
                        if (row.agentType == 5) {
                            return data;
                        }
                        
                        return '';
                    }
                },
                {
                    targets: 7,
                    data: "managerName",
                    name: "managerName"
                },
                {
                    targets: 8,
                    data: "saleLeadName",
                    name: "saleLeadName"
                },
                {
                    targets: 9,
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
                    targets: 10,
                    data: "isMapSale",
                    name: "isMapSale",
                    render: function (status) {
                        return status === true ? "Đã gán" : "Chưa gán"
                    }
                },
                {
                    targets: 11,
                    data: "status",
                    name: "status",
                    render: function (status) {
                        return (status === 0) ? "Chưa xác thực" : (status === 1) ? 'Hoạt động' : 'Khoá';
                    }

                },
                {
                    targets: 12,
                    data: "address",
                    name: "address"
                },
                {
                    targets: 13,
                    data: "exhibit",
                    name: "exhibit"
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


        $('#ExportToExcelButton').click(function () {
            _agentService.getAllListToExcel({
                filter: $('#SaleMansTableFilter').val(),
                fromDateFilter: getDateFilter($('#FromDateFilter')),
                toDateFilter: getDateFilter($('#ToDateFilter')),
                agentTypeFilter: $('#AgentTypeFilter').val(),
                saleLeadFilter: $('#SaleLeaderFilter').val(),
                managerFilter: $('#SaleStaffFilter').val(),
                agentId: $('#AgentNameFilter').val(),
                exhibitFilter: $('#ExhibitFilter').val(),
                province: $('#ProvinceFilter').val(),
                district: $('#DistrictFilter').val(),
                village: $('#VillageFilter').val(),
                status: $('#StatusFilter').val()
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

        $("#AgentNameFilter").select2({
            placeholder: 'Tất cả',
            allowClear: true,
            ajax: {
                url: abp.appPath + "api/services/app/CommonLookup/GetListUserSearch",
                dataType: 'json',
                delay: 250,
                data: function (params) {
                    return {
                        search: params.term,
                        page: params.page,
                        agentType: $('#AgentTypeFilter option:selected').val(),
                        accountType: 99
                    };
                },
                processResults: function (data, params) {
                    params.page = params.page || 1;

                    return {
                        results: $.map(data.result, function (item) {
                            return {
                                text: item.accountCode + " - " + item.phoneNumber + " - " + item.fullName,
                                id: item.id
                            }
                        }),
                        pagination: {
                            more: (params.page * 30) < data.result.length
                        }
                    };
                },
                cache: true
            },
            minimumInputLength: 3,
            language: abp.localization.currentCulture.name
        });

        $("#SaleLeaderFilter").select2({
            placeholder: 'Tất cả',
            allowClear: true,
            ajax: {
                url: abp.appPath + "api/services/app/CommonLookup/GetUserSaleLeader",
                dataType: 'json',
                delay: 250,
                data: function (params) {
                    return {
                        search: params.term,
                        page: params.page
                    };
                },
                processResults: function (data, params) {
                    params.page = params.page || 1;

                    return {
                        results: $.map(data.result, function (item) {
                            return {
                                text: item.userName + " - " + item.phoneNumber + " - " + item.fullName,
                                id: item.id
                            }
                        }),
                        pagination: {
                            more: (params.page * 30) < data.result.length
                        }
                    };
                },
                cache: true
            },
            minimumInputLength: 3,
            language: abp.localization.currentCulture.name
        });

        $("#SaleStaffFilter").select2({
            placeholder: 'Tất cả',
            allowClear: true,
            ajax: {
                url: abp.appPath + "api/services/app/CommonLookup/GetUserSaleBySaleLeader",
                dataType: 'json',
                delay: 250,
                data: function (params) {
                    return {
                        search: params.term,
                        page: params.page,
                        saleLeaderId: $('#SaleLeaderFilter option:selected').val(),
                        accountType: 99
                    };
                },
                processResults: function (data, params) {
                    params.page = params.page || 1;

                    return {
                        results: $.map(data.result, function (item) {
                            return {
                                text: item.userName + "-" + item.phoneNumber + "-" + item.fullName,
                                id: item.id
                            }
                        }),
                        pagination: {
                            more: (params.page * 30) < data.result.length
                        }
                    };
                },
                cache: true
            },
            minimumInputLength: 3,
            language: abp.localization.currentCulture.name
        });

        $(document).ready(function () {
            _getProvince();
            $('#DistrictFilter').prop('disabled', true);
            $('#VillageFilter').prop('disabled', true);

            $("#ProvinceFilter").on('change', function () {
                let id = $(this).val();
                if (id !== undefined && id !== '') {
                    $('#DistrictFilter').removeAttr('disabled');
                    _getDistrict(id);
                } else {
                    $('#DistrictFilter').html('').prop('disabled', true);
                    $('#VillageFilter').html('').prop('disabled', true);
                }
            });

            $("#DistrictFilter").on('change', function () {
                let id = $(this).val();
                if (id !== undefined && id !== '') {
                    $('#VillageFilter').removeAttr('disabled');
                    _getWard(id);
                } else {
                    $('#VillageFilter').html('').prop('disabled', true);
                }
            });
        });

        function _getProvince() {
            _commonLookupService.getProvinces().done(function (data) {
                if (data !== null && data !== undefined) {
                    var html = '';
                    html += '<option value="">Tất cả</option>';
                    $.each(data, function (key, item) {
                        html += '<option value=' + item.id + '>' + item.cityName + '</option>';
                    });
                    $("#ProvinceFilter").html(html);
                }
            });
        }

        function _getDistrict(id) {
            _commonLookupService.getDistricts(id, true).done(function (data) {
                if (data !== null && data !== undefined) {
                    var html = '';
                    html += '<option value="">Tất cả</option>';
                    $.each(data, function (key, item) {
                        html += '<option value=' + item.id + '>' + item.districtName + '</option>';
                    });
                    $("#DistrictFilter").html(html);
                }
            });
        }

        function _getWard(id) {
            _commonLookupService.getWards(id, true).done(function (data) {
                if (data !== null && data !== undefined) {
                    var html = '';
                    html += '<option value="">Tất cả</option>';
                    $.each(data, function (key, item) {
                        html += '<option value=' + item.id + '>' + item.wardName + '</option>';
                    });
                    $("#VillageFilter").html(html);
                }
            });
        }

        function resetOdp(data) {
            console.log(data);
            bootbox.prompt({
                title: "Nhập số lần gửi ODP cần reset",
                callback: function (result) {
                    console.log(result);
                    //abp.setBusy(true);
                    if (result !== null && result !== undefined) {
                        _agentService.resetOdp(
                            {userId: data.record.id, resetCount: result}
                        ).done(function () {
                            abp.message.info(app.localize('SavedSuccessfully'));

                        }).always(function () {
                            //abp.setBusy(false);
                        });
                    }

                }
            });
        }

        $('#CreateNewAgentGeneralButton').click(function () {
            _createOrEditAgentGeneralModal.open({type: 'AgentGeneral'});
        });

        $('#CreateNewWholesaleAgentButton').click(function () {
            _createOrEditAgentGeneralModal.open({type: 'WholesaleAgent'});
        });

        $('#CreateNewSubAgentButton').click(function () {
            _createOrEditAgentGeneralModal.open({type: 'SubAgent'});
        });

        abp.event.on('app.createOrEditAgentsTableModalSaved', function () {
            getAgents();
        });

    });
})();
