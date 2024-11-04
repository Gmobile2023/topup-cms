(function ($) {
    app.modals.LockOrUnlockModal = function () {

        var _modalManager;
        var _agentService = abp.services.app.agentManagerment;

        let id = $("#agentId").val();
        let type = $("#type").val();

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };

        this.save = function () {
            if ($('#LockAccountNote').val().length == 0) {
                abp.message.warn('Vui lòng nhập lý do!');
                return false;
            }

            if (id != null && type != null) {
                if (type == 'lock') {
                    _modalManager.setBusy(true);
                    _agentService.blockUser({userId:id,note:$('#LockAccountNote').val()}).done(function () {
                        abp.message.success('Khoá thành công!');
                        getAgentsTable();
                        _modalManager.close();
                    }).always(function () {
                        _modalManager.setBusy(false);
                    });
                } else if (type == 'unlock') {
                    _modalManager.setBusy(true);
                    _agentService.unlockUser({userId:id,note:$('#LockAccountNote').val()}).done(function () {
                        abp.message.success('Mở khoá thành công!');
                        getAgentsTable();
                        _modalManager.close();
                    }).always(function () {
                        _modalManager.setBusy(false);
                    });
                }
            }
        };

        function getAgentsTable() {
            $('#AgentsTable').DataTable().ajax.reload();
        }
    };
})(jQuery);
