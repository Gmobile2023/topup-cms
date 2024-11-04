(function ($) {
    app.modals.AgentGeneralModal = function () {
        let _accountService = abp.services.app.accountManagement;
        let _commonLookupService = abp.services.app.commonLookup;

        let _modalManager;
        let _$accountInformationForm = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            let modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            modal.find('.select2').select2();

            $('#District').prop('disabled', true);
            $('#Ward').prop('disabled', true);

            $("#ParentAccount").select2({
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
                            agentType: 4,
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
                minimumInputLength: 3,
                language: abp.localization.currentCulture.name
            });

            _$accountInformationForm = _modalManager.getModal().find('form[name=AgentGeneralInformationsForm]');
            _$accountInformationForm.validate();
        };

        $(document).ready(function () {
            $("#Province").on('change', function () {
                let id = $(this).val();
                if (id !== undefined && id !== '') {
                    $('#District').removeAttr('disabled');
                    _getDistrict(id);
                } else {
                    $('#District').prop('disabled', true);
                }
            });

            $("#District").on('change', function () {
                let id = $(this).val();
                if (id !== undefined && id !== '') {
                    $('#Ward').removeAttr('disabled');
                    _getWard(id);
                } else {
                    $('#Ward').prop('disabled', true);
                }
            });

            $("#FrontPhotoUpload").on('change', function () {
                let image = $('#FrontPhoto');
                app.uploadFile($("#FrontPhotoUpload"), image);
                if (image.val()) {
                    $('.front-image-preview').attr('src', image.val());
                }
            });

            $("#BackSitePhotoUpload").on('change', function () {
                let image = $('#BackSitePhoto');
                app.uploadFile($("#BackSitePhotoUpload"), image);
                if (image.val()) {
                    $('.back-image-preview').attr('src', image.val());
                }
            });
        });

        function validateEmail(email) {
            let re = /\S+@\S+\.\S+/;
            return re.test(email);
        }
        
        function _getDistrict(id) {
            _modalManager.setBusy(true);
            _commonLookupService.getDistricts(id, true).done(function (data) {
                if (data !== null && data !== undefined) {
                    let html = '';
                    html += '<option value="">Tất cả</option>';
                    $.each(data, function (key, item) {
                        html += '<option value=' + item.id + '>' + item.districtName + '</option>';
                    });
                    
                    $("#District").html(html);
                    _modalManager.setBusy(false);
                }
            });
        }

        function _getWard(id) {
            _modalManager.setBusy(true);
            _commonLookupService.getWards(id, true).done(function (data) {
                if (data !== null && data !== undefined) {
                    let html = '';
                    html += '<option value="">Tất cả</option>';
                    $.each(data, function (key, item) {
                        html += '<option value=' + item.id + '>' + item.wardName + '</option>';
                    });

                    $("#Ward").html(html);
                }
                _modalManager.setBusy(false);
            });
        }
        
        this.save = function () {
            if (!_$accountInformationForm.valid()) {
                return;
            }

            let agent = _$accountInformationForm.serializeFormToObject();

            if (agent.idIdentity && agent.identityIdExpireDate && !agent.idType) {
                abp.message.error('Vui lòng chọn loại giấy tờ!');
                return false;
            }

            if (agent.cityId && !agent.districtId || agent.cityId && !agent.wardId || agent.cityId && !agent.address) {
                abp.message.error('Vui lòng chọn quận huyện, phường xã, địa chỉ chi tiết');
                return false;
            }
            
            if (agent.agentType == 5) {
                if (!agent.parentAccount) {
                    abp.message.error('Vui lòng chọn đại lý tổng');
                    return false;
                }
                
                _modalManager.setBusy(true);
                _accountService.createOrEditSubAgent(
                    agent
                ).done(function () {
                    abp.message.info(app.localize('SavedSuccessfully'));
                    _modalManager.close();
                    abp.event.trigger('app.createOrEditAgentsTableModalSaved');
                }).always(function () {
                    _modalManager.setBusy(false);
                });
                
                return false;
            }
            
            _modalManager.setBusy(true);
            _accountService.createOrEditAgent(
                agent
            ).done(function () {
                abp.message.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditAgentsTableModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);