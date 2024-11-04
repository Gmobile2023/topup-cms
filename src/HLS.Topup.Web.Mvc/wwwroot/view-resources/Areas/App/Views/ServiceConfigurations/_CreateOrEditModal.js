(function ($) {
    app.modals.CreateOrEditServiceConfigurationModal = function () {

        var _serviceConfigurationsService = abp.services.app.serviceConfigurations;

        var _modalManager;
        var _$serviceConfigurationInformationForm = null;

        var _ServiceConfigurationserviceLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ServiceConfigurations/ServiceLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/ServiceConfigurations/_ServiceConfigurationServiceLookupTableModal.js',
            modalClass: 'ServiceLookupTableModal'
        }); var _ServiceConfigurationproviderLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ServiceConfigurations/ProviderLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/ServiceConfigurations/_ServiceConfigurationProviderLookupTableModal.js',
            modalClass: 'ProviderLookupTableModal'
        }); var _ServiceConfigurationcategoryLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ServiceConfigurations/CategoryLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/ServiceConfigurations/_ServiceConfigurationCategoryLookupTableModal.js',
            modalClass: 'CategoryLookupTableModal'
        }); var _ServiceConfigurationproductLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ServiceConfigurations/ProductLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/ServiceConfigurations/_ServiceConfigurationProductLookupTableModal.js',
            modalClass: 'ProductLookupTableModal'
        }); var _ServiceConfigurationuserLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ServiceConfigurations/UserLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/ServiceConfigurations/_ServiceConfigurationUserLookupTableModal.js',
            modalClass: 'UserLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

            var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });
            modal.find(".select2").select2();
            _$serviceConfigurationInformationForm = _modalManager.getModal().find('form[name=ServiceConfigurationInformationsForm]');
            _$serviceConfigurationInformationForm.validate();
        };

        $('#OpenServiceLookupTableButton').click(function () {

            var serviceConfiguration = _$serviceConfigurationInformationForm.serializeFormToObject();

            _ServiceConfigurationserviceLookupTableModal.open({ id: serviceConfiguration.serviceId, displayName: serviceConfiguration.serviceServicesName }, function (data) {
                _$serviceConfigurationInformationForm.find('input[name=serviceServicesName]').val(data.displayName);
                _$serviceConfigurationInformationForm.find('input[name=serviceId]').val(data.id);
            });
        });

        $('#ClearServiceServicesNameButton').click(function () {
            _$serviceConfigurationInformationForm.find('input[name=serviceServicesName]').val('');
            _$serviceConfigurationInformationForm.find('input[name=serviceId]').val('');
        });

        $('#OpenProviderLookupTableButton').click(function () {

            var serviceConfiguration = _$serviceConfigurationInformationForm.serializeFormToObject();

            _ServiceConfigurationproviderLookupTableModal.open({ id: serviceConfiguration.providerId, displayName: serviceConfiguration.providerName }, function (data) {
                _$serviceConfigurationInformationForm.find('input[name=providerName]').val(data.displayName);
                _$serviceConfigurationInformationForm.find('input[name=providerId]').val(data.id);
            });
        });

        $('#ClearProviderNameButton').click(function () {
            _$serviceConfigurationInformationForm.find('input[name=providerName]').val('');
            _$serviceConfigurationInformationForm.find('input[name=providerId]').val('');
        });

        $('#OpenCategoryLookupTableButton').click(function () {

            var serviceConfiguration = _$serviceConfigurationInformationForm.serializeFormToObject();

            _ServiceConfigurationcategoryLookupTableModal.open({ id: serviceConfiguration.categoryId, displayName: serviceConfiguration.categoryCategoryName }, function (data) {
                _$serviceConfigurationInformationForm.find('input[name=categoryCategoryName]').val(data.displayName);
                _$serviceConfigurationInformationForm.find('input[name=categoryId]').val(data.id);
            });
        });

        $('#ClearCategoryCategoryNameButton').click(function () {
            _$serviceConfigurationInformationForm.find('input[name=categoryCategoryName]').val('');
            _$serviceConfigurationInformationForm.find('input[name=categoryId]').val('');
        });

        $('#OpenProductLookupTableButton').click(function () {

            var serviceConfiguration = _$serviceConfigurationInformationForm.serializeFormToObject();

            _ServiceConfigurationproductLookupTableModal.open({ id: serviceConfiguration.productId, displayName: serviceConfiguration.productProductName }, function (data) {
                _$serviceConfigurationInformationForm.find('input[name=productProductName]').val(data.displayName);
                _$serviceConfigurationInformationForm.find('input[name=productId]').val(data.id);
            });
        });

        $('#ClearProductProductNameButton').click(function () {
            _$serviceConfigurationInformationForm.find('input[name=productProductName]').val('');
            _$serviceConfigurationInformationForm.find('input[name=productId]').val('');
        });

        $('#OpenUserLookupTableButton').click(function () {

            var serviceConfiguration = _$serviceConfigurationInformationForm.serializeFormToObject();

            _ServiceConfigurationuserLookupTableModal.open({ id: serviceConfiguration.userId, displayName: serviceConfiguration.userName }, function (data) {
                _$serviceConfigurationInformationForm.find('input[name=userName]').val(data.displayName);
                _$serviceConfigurationInformationForm.find('input[name=userId]').val(data.id);
            });
        });

        $('#ClearUserNameButton').click(function () {
            _$serviceConfigurationInformationForm.find('input[name=userName]').val('');
            _$serviceConfigurationInformationForm.find('input[name=userId]').val('');
        });


        $("#serviceId").change(function (e) {

            var serviceId = $(e.target).val();
            if (serviceId == "") serviceId = "0";
            else if (serviceId == "8") serviceId = "4";
            Sv.GetCateTwoByService(serviceId == "" ? "0" : serviceId, $("#categoryId"), false);
        });

        $("#categoryId").change(function (e) {
            const cateId = $(e.target).val();
            Sv.GetProductTwoByCate(cateId, $("#ProductProductName"), false);
        });


        this.save = function () {
            if (!_$serviceConfigurationInformationForm.valid()) {
                return;
            }
            if ($('#ServiceConfiguration_ServiceId').prop('required') && $('#ServiceConfiguration_ServiceId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Service')));
                return;
            }
            if ($('#ServiceConfiguration_ProviderId').prop('required') && $('#ServiceConfiguration_ProviderId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Provider')));
                return;
            }
            if ($('#ServiceConfiguration_CategoryId').prop('required') && $('#ServiceConfiguration_CategoryId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Category')));
                return;
            }
            if ($('#ServiceConfiguration_ProductId').prop('required') && $('#ServiceConfiguration_ProductId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Product')));
                return;
            }
            if ($('#ServiceConfiguration_UserId').prop('required') && $('#ServiceConfiguration_UserId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('User')));
                return;
            }

            var serviceConfiguration = _$serviceConfigurationInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _serviceConfigurationsService.createOrEdit(
                serviceConfiguration
            ).done(function () {
                abp.message.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditServiceConfigurationModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);
