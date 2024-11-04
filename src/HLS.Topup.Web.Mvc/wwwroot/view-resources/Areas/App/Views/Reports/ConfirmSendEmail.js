(function ($) {
    app.modals.ConfirmEmailModal = function () {
        const _rptService = abp.services.app.reportSystem;

        let _modalManager;

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };

        this.save = function () {

            var email = $("#txtEmail").val();
            if (email == null || email == "")
            {
                abp.message.info("Quý khách chưa nhập email để nhận báo cáo.");
                return;
            }

            var objSend =
            {
                FromDate: $('#hdnFromDate').val(),
                ToDate: $('#hdnToDate').val(),
                agentCode: $("#hdnAgentCode").val(),
                Email: $("#txtEmail").val(),
            };

            _modalManager.setBusy(true);
            _rptService.sendMailReportComparePartner(
                objSend
            ).done(function () {
                abp.message.info('Quý khách xem kết quả qua mail.');
                _modalManager.close();
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);
