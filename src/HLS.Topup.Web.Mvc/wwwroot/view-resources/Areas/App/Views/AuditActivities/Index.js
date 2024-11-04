(function () {
    $(function () {

        let _$activitiesTable = $('#AuditActivitiesTable');
        let _auditActivitiesAppService = abp.services.app.auditActivities;

        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        let getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z");
        }

        let getToDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT23:59:59Z");
        }

        let dataTable = _$activitiesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _auditActivitiesAppService.getAll,
                inputFilter: function () {
                    return {
                        fromDate: getDateFilter($('#FromCreatedTimeFilter')),
                        toDate: getToDateFilter($('#ToCreatedTimeFilter')),
                        accountActivityType: $('#ActivitiesFilterId').val(),
                        accountCode: $('#AgentFilterId').val(),
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
                    data: "agentName",
                    name: "agentName",
                    render: function (data, type, row) {
                        return data;
                    }
                },
                {
                    targets: 2,
                    data: "createdDate",
                    name: "createdDate",
                    render: function (createdDate) {
                        if (createdDate) {
                            return moment(createdDate).format('L LTS');
                        }
                        return "";
                    }
                },
                {
                    targets: 3,
                    data: "userCreated",
                    name: "userCreated"
                },
                {
                    targets: 4,
                    data: "accountActivityType",
                    name: "accountActivityType",
                    render: function (accountActivityType) {
                        return app.localize('Enum_AccountActivityType_' + accountActivityType);
                    }
                },
                {
                    targets: 5,
                    data: "note",
                    name: "note"
                },
                {
                    targets: 6,
                    data: "attachment",
                    name: "attachment",
                    render: function (attachment) {
                        if (attachment != "" && attachment != null)
                            return "<a href='" + attachment + "'>Tài về</a>";
                        else return "";
                    }
                }
            ]
        });

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

        $("#AgentFilterId").select2({
            placeholder: 'Chọn đại lý',
            ajax: {
                url: abp.appPath + "api/services/app/CommonLookup/GetListUserSearch",
                dataType: 'json',
                delay: 250,
                data: function (params) {
                    return {
                        search: params.term,
                        agentType: 99,
                        page: params.page,
                        accountType: 99
                    };
                },
                processResults: function (data, params) {
                    params.page = params.page || 1;

                    return {
                        results: $.map(data.result, function (item) {
                            return {
                                text: item.accountCode + " - " + item.phoneNumber + " - " + item.fullName,
                                id: item.accountCode
                            }
                        }),
                        pagination: {
                            more: (params.page * 30) < data.result.length
                        }
                    };
                },
                cache: true
            },
            minimumInputLength: 1,
            language: abp.localization.currentCulture.name
        });

        $('#GetAuditActivitiesButton').click(function (e) {
            e.preventDefault();
            getTable();
        });

        function getTable() {
            dataTable.ajax.reload();
        }
    });
})();