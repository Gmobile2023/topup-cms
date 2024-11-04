(function ($) {
    app.modals.EditAgentSupperModal = function () {

        var _agentService = abp.services.app.agentManagerment;
        var _commonLookupService = abp.services.app.commonLookup;

        var _modalManager;

        var _$agentApiForm;
        var serviceConfigs;
        var allowedScopes;
        var allowedGrantTypes;
        const categories = $("#txtCategoryConfigs").val();
        const notAllowProducts = $("#txtProductConfigsNotAllow").val();
        this.init = function (modalManager) {
            _modalManager = modalManager;

            var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });
            modal.find(".select2").select2();
            serviceConfigs = $("#txtServiceConfigs").val();
            allowedScopes = $("#txtAllowedScopes").val();
            allowedGrantTypes = $("#txtAllowedGrantTypes").val();
            _$agentApiForm = _modalManager.getModal().find('form[name=AgentInformationsForm]');
            _$agentApiForm.validate();

            $("#txtManagerEmail").on('change', function () {
                var email = $("#txtManagerEmail").val();
                if (email == null || email == "") {
                    $("#spanManagerEmail").text('');
                } else {
                    const check = validateEmail(email);
                    if (!check)
                        $("#spanManagerEmail").text('Nhập email chưa đúng');
                    else
                        $("#spanManagerEmail").text('');
                }
            });

            $("#txtTechnicalEmail").on('change', function () {
                var email = $("#txtTechnicalEmail").val();
                if (email == null || email == "") {
                    $("#spanTechnicalEmail").text('');
                } else {
                    var check = validateEmail(email);
                    if (!check)
                        $("#spanTechnicalEmail").text('Nhập email chưa đúng');
                    else
                        $("#spanTechnicalEmail").text('');
                }
            });

            $("#txtCompareEmail").on('change', function () {
                var email = $("#txtCompareEmail").val();
                if (email == null || email == "") {
                    $("#spanCompareEmail").text('');
                } else {
                    var check = validateEmail(email);
                    if (!check)
                        $("#spanCompareEmail").text('Nhập email chưa đúng');
                    else
                        $("#spanCompareEmail").text('');
                }
            });

            $("#txtAccountancyEmail").on('change', function () {
                var email = $("#txtAccountancyEmail").val();
                if (email == null || email == "") {
                    $("#spanAccountancyEmail").text('');
                } else {
                    var check = validateEmail(email);
                    if (!check)
                        $("#spanAccountancyEmail").text('Nhập email chưa đúng');
                    else
                        $("#spanAccountancyEmail").text('');
                }
            });
            showSelect2();

            $("#cob_Province").select2();
            $("#cob_District").select2();
            $("#cob_Ward").select2();

            $(document).ready(function () {
                let provinceId = $('#cob_Province').attr('data-id');
                let districtId = $('#cob_District').attr('data-id');
                let wardId = $('#cob_Ward').attr('data-id');

                if (provinceId.length > 0) {
                    _getProvince(provinceId);
                } else {
                    _getProvince();
                }
                if (provinceId.length > 0 && districtId.length > 0) _getDistrict(provinceId, districtId);
                if (districtId.length > 0 && wardId.length > 0) _getWard(districtId, wardId);

                $("#cob_Province").on('change', function () {
                    let id = $(this).val();
                    if (id !== undefined && id !== '') {
                        _getDistrict(id);
                    }
                });

                $("#cob_District").on('change', function () {
                    let id = $(this).val();
                    if (id !== undefined && id !== '') {
                        _getWard(id);
                    }
                });


            });
        };
        $("#ClientId").on('keyup input', function () {
            const text = $(this).val();
            const key = text.toUpperCase() + "_PublicKey.pem";
            $("#PublicKeyFile").val(key);
        });

        function showSelect2() {
            if (serviceConfigs !== null && serviceConfigs !== undefined && serviceConfigs !== "") {
                const selected = JSON.parse(serviceConfigs);
                //console.log(selected);
                //$("#ServiceConfig").select2('val', selected);
                $('#ServiceConfig').val(selected).trigger('change');
            }
            if (allowedScopes !== null && allowedScopes !== undefined && allowedScopes !== "") {
                const selected = JSON.parse(allowedScopes);
                //console.log(selected);
                //$("#ServiceConfig").select2('val', selected);
                $('#AllowedScopes').val(selected).trigger('change');
            }
            if (allowedGrantTypes !== null && allowedGrantTypes !== undefined && allowedGrantTypes !== "") {
                const selected = JSON.parse(allowedGrantTypes);
                //console.log(selected);
                //$("#ServiceConfig").select2('val', selected);
                $('#AllowedGrantTypes').val(selected).trigger('change');
            }
        }


        function validateEmail(email) {
            var re = /\S+@\S+\.\S+/;
            return re.test(email);
        }


        function GetContactInfos() {
            var obj = [];

            obj.push({
                FullName: $("#txtManagerFullName").val(),
                PhoneNumber: $("#txtManagerPhone").val(),
                Email: $("#txtManagerEmail").val(),
                ContactType: "Director"
            });

            obj.push({
                FullName: $("#txtTechnicalFullName").val(),
                PhoneNumber: $("#txtTechnicalPhone").val(),
                Email: $("#txtTechnicalEmail").val(),
                ContactType: "Technical"
            });


            obj.push({
                FullName: $("#txtCompareFullName").val(),
                PhoneNumber: $("#txtComparePhone").val(),
                Email: $("#txtCompareEmail").val(),
                ContactType: "Comparator"
            });


            obj.push({
                FullName: $("#txtAccountancyFullName").val(),
                PhoneNumber: $("#txtAccountancyPhone").val(),
                Email: $("#txtAccountancyEmail").val(),
                ContactType: "Accountant",
            });


            return obj;
        };

        function getCategories(serviceCode, controlSelect, isActive) {
            abp.ui.setBusy();
            abp.services.app.commonLookup.getCategoriesMuti({ serviceCodes: serviceCode, isActive: isActive })
                .done(function (result) {
                    let html = "<option value=\"\">Chọn loại sản phẩm</option>";
                    if (result != null && result.length > 0) {
                        for (let i = 0; i < result.length; i++) {
                            let item = result[i];
                            html += ("<option value=\"" + item.categoryCode + "\">" + item.categoryName + "</option>");
                        }
                    }
                    controlSelect.html(html);
                })
                .always(function () {
                    if (categories !== null && categories !== undefined && categories !== "") {
                        const selected = JSON.parse(categories);
                        $('#CategoryConfigs').val(selected).trigger('change');
                    }
                    abp.ui.clearBusy();
                });
        }

        function getNotAllowProducts(categoryCode, controlSelect, isActive) {
            abp.ui.setBusy();
            abp.services.app.commonLookup.getProductByCategoryMuti(categoryCode, isActive)
                .done(function (result) {
                    let html = "<option value=\"\">Chọn sản phẩm</option>";
                    if (result != null && result.length > 0) {
                        for (let i = 0; i < result.length; i++) {
                            let item = result[i];
                            html += ("<option value=\"" + item.productCode + "\">" + item.productCode + ' - ' + item.productName + "</option>");
                        }
                    }
                    controlSelect.html(html);
                })
                .always(function () {
                    if (notAllowProducts !== null && notAllowProducts !== undefined && notAllowProducts !== "") {
                        const selected = JSON.parse(notAllowProducts);
                        $('#ProductConfigsNotAllow').val(selected).trigger('change');
                    }
                    abp.ui.clearBusy();
                });
        }


        $('#ServiceConfig').change(function () {
            var serviceCode = $(this).val();
            if (serviceCode != null || serviceCode !== '' || serviceCode.length > 0) {
                getCategories(serviceCode, $("#CategoryConfigs"), false);
            }
        }).trigger("change");

        $('#CategoryConfigs').change(function () {
            var categoryCode = $(this).val();
            if (categoryCode != null || categoryCode !== '' || categoryCode.length > 0) {
                getNotAllowProducts(categoryCode, $("#ProductConfigsNotAllow"), false);
            }
        }).trigger("change");

        if (categories !== null && categories !== undefined && categories !== "") {
            const selected = JSON.parse(categories);
            console.log(selected);
            //$("#ServiceConfig").select2('val', selected);
            $('#CategoryConfigs').val(selected).trigger('change');
        }

        if (notAllowProducts !== null && notAllowProducts !== undefined && notAllowProducts !== "") {
            const selected = JSON.parse(notAllowProducts);
            console.log(selected);
            //$("#ServiceConfig").select2('val', selected);
            $('#ProductConfigsNotAllow').val(selected).trigger('change');
        }

        this.save = function () {
            if (!_$agentApiForm.valid()) {
                return;
            } else {
                var managerEmail = $("#spanManagerEmail").text();
                var technicalEmail = $("#spanTechnicalEmail").text();
                var compareEmail = $("#spanCompareEmail").text();
                var accountancyEmail = $("#spanAccountancyEmail").text();

                if (managerEmail !== ""
                    || technicalEmail !== ""
                    || compareEmail !== ""
                    || accountancyEmail !== "") {
                    //abp.message.info('Quý khách kiểm tra lại email không hợp lệ.');
                    return;
                }
            }
            //Gunner chỗ này sao k dùng map object luôn??
            var data = _$agentApiForm.serializeFormToObject();
            var agent =
            {
                Id: $("#hdnUserId").val(),
                Name: $("#Name").val(),
                Surname: $("#Surname").val(),
                PhoneNumber: $("#PhoneNumber").val(),
                EmailAddress: $("#txtEmailCompare").val(),
                FolderFtp: $("#txtFolderFtp").val(),
                UserName: $("#PhoneNumber").val(),
                Address: $("#Address").val(),
                Description: "",
                Password: $("#txtPassword").val(),
                CityId: $("#cob_Province").val(),
                DistrictId: $("#cob_District").val(),
                WardId: $("#cob_Ward").val(),
                PeriodCheck: $("#CrossCheckPeriod").val(),
                IsActive: $("#selectStatus").val() == "1" ? true : false,
                ContractNumber: $("#txtContract").val(),
                SigDate: $("#ContractRegister").val(),
                TaxCode: "",
                EmailReceives: $("#txtEmailCompare").val(),
                EmailTech: $("#EmailTech").val(),
                ContactInfos: GetContactInfos(),
                ChatId: $("#ChatId").val(),
                LimitChannel: $("#LimitChannel").val(),
                IsApplySlowTrans: data.IsApplySlowTrans

            };
            if (agent.Id == 0) {
                if (agent.Password != $("#txtConfirmPassword").val()) {

                    abp.message.info('Mật khẩu đang không đồng nhất khi nhắc lại.');
                    return;
                }
            }
            agent.PartnerConfig = {
                ClientId: data.ClientId,
                PublicKeyFile: data.PublicKeyFile,
                PrivateKeyFile: data.PrivateKeyFile,
                ServiceConfigList: $("#ServiceConfig").val(),
                CategoryConfigList: $("#CategoryConfigs").val(),
                ProductConfigsNotAllowList: $("#ProductConfigsNotAllow").val(),
                LastTransTimeConfig: $("#LastTransTimeConfig").val(),
                MaxTotalTrans: $("#MaxTotalTrans").val(),
                IsCheckReceiverType: data.IsCheckReceiverType,
                IsCheckPhone: data.IsCheckPhone,
                IsCheckAllowTopupReceiverType: data.IsCheckAllowTopupReceiverType,
                DefaultReceiverType:data.DefaultReceiverType,
                IsNoneDiscount:data.IsNoneDiscount,
                Ips: data.Ips,
                EnableSig: data.EnableSig,
                SecretKey: data.SecretKey
            };
            agent.IdentityServerStorage = {
                RedirectUris: data.RedirectUris,
                PostLogoutRedirectUris: data.PostLogoutRedirectUris,
                PrivateKeyFile: data.PrivateKeyFile,
                AllowedGrantTypes: $("#AllowedGrantTypes").val(),
                AllowedScopes: $("#AllowedScopes").val(),
                AllowOfflineAccess: data.AllowOfflineAccess,
                RequireConsent: data.RequireConsent
            };
            _modalManager.setBusy(true);
            _agentService.createOrEditAgentPartner(agent).done(function () {
                abp.message.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                getAgentsTable();
                abp.event.trigger('app.createOrEditAgentSupperModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };

        function getAgentsTable() {
            $('#AgentsSupperTable').DataTable().ajax.reload();
        }

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

                    $("#cob_Province").html(html);
                }
            });
        }

        function _getDistrict(id, idx) {
            let dataId = idx;
            abp.ui.setBusy();
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
                    $("#cob_District").html(html);
                    abp.ui.clearBusy();
                }
            });
        }

        function _getWard(id, idx) {
            let dataId = idx;
            abp.ui.setBusy();
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

                    $("#cob_Ward").html(html);
                    abp.ui.clearBusy();
                }
            });
        }
    };
})(jQuery);
