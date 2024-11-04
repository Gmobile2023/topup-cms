﻿(function () {
    $(function () {
        var _showDiscountModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Profile/ShowDiscountAccount',
            modalClass: 'ModalDiscountAccount'
        });

        $('#btnShowDiscountAccount').click(function () {
            _showDiscountModal.open();
        });
    });
})();