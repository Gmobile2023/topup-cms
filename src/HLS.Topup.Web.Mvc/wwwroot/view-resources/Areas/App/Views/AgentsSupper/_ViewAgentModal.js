(function ($) {
    app.modals.ViewAgentModal = function () {

        var _modalManager;
        var _agentService = abp.services.app.agentManagerment;

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };
    };
})(jQuery);
