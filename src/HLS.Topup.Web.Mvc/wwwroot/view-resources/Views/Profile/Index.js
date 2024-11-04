﻿(function () {
    $(function () {

        Sv.SetupAmountMask();
        var _agentService = abp.services.app.agent;


        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Profile/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Views/Profile/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditUserModal'
        });

        $('#btnUpdateProfile').click(function () {
            _createOrEditModal.open();
        });
    });
})();