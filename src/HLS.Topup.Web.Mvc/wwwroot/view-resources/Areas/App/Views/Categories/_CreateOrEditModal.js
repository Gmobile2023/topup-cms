(function ($) {
    app.modals.CreateOrEditCategoryModal = function () {

        var _categoriesService = abp.services.app.categories;

        var _modalManager;
        var _$categoryInformationForm = null;

		        var _CategorycategoryLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Categories/CategoryLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Categories/_CategoryCategoryLookupTableModal.js',
            modalClass: 'CategoryLookupTableModal'
        });        var _CategoryserviceLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Categories/ServiceLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Categories/_CategoryServiceLookupTableModal.js',
            modalClass: 'ServiceLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$categoryInformationForm = _modalManager.getModal().find('form[name=CategoryInformationsForm]');
            _$categoryInformationForm.validate();
            modal.find(".select2").select2();
            $("#imageId").on('change', function () {
                app.uploadImage($("#imageId"), $('#thumbImageId'));
            });
        };

		          $('#OpenCategoryLookupTableButton').click(function () {

            var category = _$categoryInformationForm.serializeFormToObject();

            _CategorycategoryLookupTableModal.open({ id: category.parentCategoryId, displayName: category.categoryCategoryName }, function (data) {
                _$categoryInformationForm.find('input[name=categoryCategoryName]').val(data.displayName);
                _$categoryInformationForm.find('input[name=parentCategoryId]').val(data.id);
            });
        });

		$('#ClearCategoryCategoryNameButton').click(function () {
                _$categoryInformationForm.find('input[name=categoryCategoryName]').val('');
                _$categoryInformationForm.find('input[name=parentCategoryId]').val('');
        });

        $('#OpenServiceLookupTableButton').click(function () {

            var category = _$categoryInformationForm.serializeFormToObject();

            _CategoryserviceLookupTableModal.open({ id: category.serviceId, displayName: category.serviceServicesName }, function (data) {
                _$categoryInformationForm.find('input[name=serviceServicesName]').val(data.displayName);
                _$categoryInformationForm.find('input[name=serviceId]').val(data.id);
            });
        });

		$('#ClearServiceServicesNameButton').click(function () {
                _$categoryInformationForm.find('input[name=serviceServicesName]').val('');
                _$categoryInformationForm.find('input[name=serviceId]').val('');
        });

        this.save = function () {
            if (!_$categoryInformationForm.valid()) {
                return;
            }
            if ($('#Category_ParentCategoryId').prop('required') && $('#Category_ParentCategoryId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Category')));
                return;
            }
            if ($('#Category_ServiceId').prop('required') && $('#Category_ServiceId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Service')));
                return;
            }

            var category = _$categoryInformationForm.serializeFormToObject();
            var image="";
            if ($("#thumbImageId").attr("src") !== "") {
                image=$("#thumbImageId").attr("src");
            }
            category.image=image;
			 _modalManager.setBusy(true);
			 _categoriesService.createOrEdit(
				category
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditCategoryModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);
