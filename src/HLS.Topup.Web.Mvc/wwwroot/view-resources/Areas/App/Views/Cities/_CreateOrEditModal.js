(function ($) {
    app.modals.CreateOrEditCityModal = function () {

        var _citiesService = abp.services.app.cities;

        var _modalManager;
        var _$cityInformationForm = null;

        var _CitycountryLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Cities/CountryLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Cities/_CityCountryLookupTableModal.js',
            modalClass: 'CountryLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

            var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$cityInformationForm = _modalManager.getModal().find('form[name=CityInformationsForm]');
            _$cityInformationForm.validate();
        };

        $('#OpenCountryLookupTableButton').click(function () {

            var city = _$cityInformationForm.serializeFormToObject();

            _CitycountryLookupTableModal.open({
                id: city.countryId,
                displayName: city.countryCountryName
            }, function (data) {
                _$cityInformationForm.find('input[name=countryCountryName]').val(data.displayName);
                _$cityInformationForm.find('input[name=countryId]').val(data.id);
            });
        });

        $('#ClearCountryCountryNameButton').click(function () {
            _$cityInformationForm.find('input[name=countryCountryName]').val('');
            _$cityInformationForm.find('input[name=countryId]').val('');
        });


        this.save = function () {
            if (!_$cityInformationForm.valid()) {
                return;
            }
            if ($('#City_CountryId').prop('required') && $('#City_CountryId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Country')));
                return;
            }

            var city = _$cityInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _citiesService.createOrEdit(
                city
            ).done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditCityModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);
