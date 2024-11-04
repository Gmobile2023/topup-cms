(function ($) {
    app.modals.MappingSaleModal = function () {      

        var _modalManager;        
        var _agentService = abp.services.app.agentManagerment;
            
        this.init = function (modalManager) {
            _modalManager = modalManager;                      
            $("#selectSale").select2({
                placeholder: 'Select',
                allowClear: true,
                ajax: {
                    url: abp.appPath + "api/services/app/CommonLookup/GetUserSaleBySaleLeader",
                    dataType: 'json',
                    delay: 250,
                    data: function (params) {
                        return {
                            search: params.term,
                            page: params.page,
                            saleLeaderCode: '',
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
        };

        function getAgentsTable() {
            $('#AgentsTable').DataTable().ajax.reload();
        }
            
        $("#selectSale").change(function (e) {
            const userId = $(e.target).val();
            abp.ui.setBusy();
            abp.services.app.commonLookup.getUserSaleLeaderBySale(userId)
                .done(function (result) {                    
                    $("#txtSaleLeader").val(result.fullName);                    
                })
                .always(function () {
                    abp.ui.clearBusy();
                });           
        });

       
        this.save = function () {           
            var obj =
            {
                saleUserId: $("#selectSale").val(),
                userAgentId: $("#hdnAgentId").val(),
                id: $("#hdnId").val()
            };
            _modalManager.setBusy(true);
            _agentService.createOrEdit(
                obj
            ).done(function () {
                getAgentsTable();
                abp.message.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.mappingSaleModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);
