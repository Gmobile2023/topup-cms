(function ($) {
    app.modals.UserLookupTableModal = function () {

        var _modalManager;

        var _depositsService = abp.services.app.deposits;
        var _$userTable = $('#UserTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };


        var dataTable = _$userTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _depositsService.getAllUserForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#UserTableFilter').val()
                    };
                }
            },
            columnDefs: [
                {
                    targets: 0,
                    data: null,
                    orderable: false,
                    autoWidth: false,
                    defaultContent: "<div class=\"text-center\"><input id='selectbtn' class='btn btn-success' type='button' width='25px' value='" + app.localize('Select') + "' /></div>"
                },
                {
                    autoWidth: false,
                    orderable: false,
                    targets: 1,
                    data: "displayName"
                },
                {
                    autoWidth: false,
                    orderable: false,
                    targets: 2,
                    data: "userName"
                },
                {
                    autoWidth: false,
                    orderable: false,
                    targets: 3,
                    data: "phoneNumber"
                }
            ]
        });

        $('#UserTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getUser() {
            dataTable.ajax.reload();
        }

        $('#GetUserButton').click(function (e) {
            e.preventDefault();
            getUser();
        });

        $('#SelectButton').click(function (e) {
            e.preventDefault();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getUser();
            }
        });

    };
})(jQuery);

