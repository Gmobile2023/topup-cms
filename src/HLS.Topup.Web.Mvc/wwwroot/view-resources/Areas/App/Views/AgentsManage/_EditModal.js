(function ($) {
    app.modals.EditAgentModal = function () {

        var _agentService = abp.services.app.agentManagerment;
        var _profileService = abp.services.app.profile;
        var _citiesService = abp.services.app.cities;
        var _districtsService = abp.services.app.districts;
        var _wardsService = abp.services.app.wards;
        var _commonLookupService = abp.services.app.commonLookup;

        var _modalManager;
        var _$bankInformationForm = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$agentInformationForm = _modalManager.getModal().find('form[name=AgentInformationsForm]');
            _$agentInformationForm.validate();
        };

        this.save = function () {
            if (!_$agentInformationForm.valid()) {
                return;
            }

            var agent = _$agentInformationForm.serializeFormToObject();
            //agent.identityIdExpireDate = $('#IdentityIdExpireDate').data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z");

            _modalManager.setBusy(true);
            _profileService.updateUserById(agent, _$agentInformationForm.find('input[name="UserId"]').val()).done(function () {
                abp.message.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                getAgentsTable();
                //abp.event.trigger('app.createOrEditBankModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };

        function getAgentsTable() {
            $('#AgentsTable').DataTable().ajax.reload();
        }

        $(document).ready(function () {
            let provinceId = $('#Province').attr('data-id');
            let districtId = $('#District').attr('data-id');
            let wardId = $('#Ward').attr('data-id');

            if (provinceId.length > 0) {
                _getProvince(provinceId);
            } else {
                _getProvince();
            }
            if (provinceId.length > 0 && districtId.length > 0) _getDistrict(provinceId, districtId);
            if (districtId.length > 0 && wardId.length > 0) _getWard(districtId, wardId);

            $("#Province").on('change', function () {
                let id = $(this).val();
                if (id !== undefined && id !== '') {
                    _getDistrict(id);
                }
            });

            $("#District").on('change', function () {
                let id = $(this).val();
                if (id !== undefined && id !== '') {
                    _getWard(id);
                }
            });
        });

        function _getProvince(id) {
            let dataId = id;
            _commonLookupService.getProvinces().done(function (data) {
                if (data !== null && data !== undefined) {
                    let html = '';
                    html += '<option value="">Tất cả</option>';
                    $.each(data, function (key, item) {
                        if (item.id == dataId) {
                            html += '<option value=' + item.id + ' selected>' + item.cityName + '</option>';
                        } else {
                            html += '<option value=' + item.id + '>' + item.cityName + '</option>';
                        }
                    });

                    $("#Province").html(html);
                }
            });
        }

        function _getDistrict(id, idx) {
            let dataId = idx;

            _commonLookupService.getDistricts(id, true).done(function (data) {
                if (data !== null && data !== undefined) {
                    let html = '';
                    html += '<option value="">Tất cả</option>';
                    $.each(data, function (key, item) {
                        if (item.id == dataId) {
                            html += '<option value=' + item.id + ' selected>' + item.districtName + '</option>';
                        } else {
                            html += '<option value=' + item.id + '>' + item.districtName + '</option>';
                        }

                    });
                    $("#District").html(html);
                }
            });
        }

        function _getWard(id, idx) {
            let dataId = idx;

            _commonLookupService.getWards(id, true).done(function (data) {
                if (data !== null && data !== undefined) {
                    let html = '';
                    html += '<option value="">Tất cả</option>';
                    $.each(data, function (key, item) {
                        if (item.id == dataId) {
                            html += '<option value=' + item.id + ' selected>' + item.wardName + '</option>';
                        } else {
                            html += '<option value=' + item.id + '>' + item.wardName + '</option>';
                        }
                    });

                    $("#Ward").html(html);
                }
            });
        }
    };
})(jQuery);
