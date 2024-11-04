(function () {
    $(function () {

        var _agentService = abp.services.app.postManagement;
        var _commonLookupService = abp.services.app.commonLookup;
        const _passwordComplexityHelper = new app.PasswordComplexityHelper();
        var _$form=$("#create-agent-form")
        $(document).ready(function () {
            let provinceId = $('#cityId').attr('data-id');
            let districtId = $('#districtId').attr('data-id');
            let wardId = $('#wardId').attr('data-id');

            if (provinceId.length > 0) {
                _getProvince(provinceId);
            } else {
                _getProvince();
            }
            if (provinceId.length > 0 && districtId.length > 0) _getDistrict(provinceId, districtId);
            if (districtId.length > 0 && wardId.length > 0) _getWard(districtId, wardId);

            $("#cityId").on('change', function () {
                let id = $(this).val();
                if (id !== undefined && id !== '') {
                    _getDistrict(id);
                }
            });

            $("#districtId").on('change', function () {
                let id = $(this).val();
                if (id !== undefined && id !== '') {
                    _getWard(id);
                }
            });
        });

        function _getProvince(id) {
            let dataId = id;
            _commonLookupService.getProvinces().done(function (data) {
                if (data !== null && data !== undefined) {
                    let html = '';
                    html += '<option value="">Tất cả</option>';
                    $.each(data, function (key, item) {
                        if (item.id == dataId) {
                            html += '<option value=' + item.id + ' selected>' + item.cityName + '</option>';
                        } else {
                            html += '<option value=' + item.id + '>' + item.cityName + '</option>';
                        }
                    });

                    $("#cityId").html(html);
                }
            });
        }

        function _getDistrict(id, idx) {
            let dataId = idx;

            _commonLookupService.getDistricts(id, true).done(function (data) {
                if (data !== null && data !== undefined) {
                    let html = '';
                    html += '<option value="">Tất cả</option>';
                    $.each(data, function (key, item) {
                        if (item.id == dataId) {
                            html += '<option value=' + item.id + ' selected>' + item.districtName + '</option>';
                        } else {
                            html += '<option value=' + item.id + '>' + item.districtName + '</option>';
                        }

                    });
                    $("#districtId").html(html);
                }
            });
        }

        function _getWard(id, idx) {
            let dataId = idx;

            _commonLookupService.getWards(id, true).done(function (data) {
                if (data !== null && data !== undefined) {
                    let html = '';
                    html += '<option value="">Tất cả</option>';
                    $.each(data, function (key, item) {
                        if (item.id == dataId) {
                            html += '<option value=' + item.id + ' selected>' + item.wardName + '</option>';
                        } else {
                            html += '<option value=' + item.id + '>' + item.wardName + '</option>';
                        }
                    });

                    $("#wardId").html(html);
                }
            });
        }

        $("#btnSave").click(function () {
            if (!_$form.valid()) {
                return;
            }
            const formToObject = _$form.serializeFormToObject();
            _agentService.createOrEdit(
                formToObject
            ).done(function () {
                //abp.message.info(app.localize('SavedSuccessfully'));
                window.location.href = '/PostManagement';
            }).always(function () {

            });
        })
    });
})();
