var ctrl = {
    page: $("#Profile_Edit_Page"),
    form: $("#profileForm"),
    getFormValue: function () {
        var obj = ctrl.form.serializeFormToObject();
        obj.isOpenVerify = $("#blockVerify").is(":visible");
        obj.img_before = $('#img_before').length > 0 ? $('#img_before')[0].files : [];
        obj.img_after = $('#img_after').length > 0 ? $('#img_after')[0].files : [];
        obj.img_before_old = $('#pre_before').data("src");
        obj.img_after_old = $('#pre_after').data("src");
        if (obj.img_before.length > 0 || obj.img_after.length > 0) {
            obj.extraInfo = (getLocalStorage('identity_info')) + "";
        } else {
            obj.extraInfo = "";
        }

        return obj;
    },
    initValid: function () {
        ctrl.form.validate({
            ignore: ":hidden",
            highlight: function (element) {
                $(element).closest('.form-group-wrap').addClass('ferror');
            },
            unhighlight: function (element) {
                $(element).closest('.form-group-wrap').removeClass('ferror');
            },
            success: function (element) {
                element.closest('.form-group-wrap').removeClass('ferror');
            },
            errorPlacement: function (error, element) {

            },
        });
        $(".select2").on("select2:select", function (e) {
            $(e.target).valid();
        });
    },
    valid: function () {
        if (!$("#profileForm").valid()) {
            abp.message.warn("Vui lòng nhập đầy đủ thông tin")
                .done(function () {
                    $('.form-group-wrap.ferror  input,.form-group-wrap.ferror  select,.form-group-wrap.ferror  textarea')[0].focus();
                });
            return false;
        }
        var obj = ctrl.getFormValue();
        if (obj.isOpenVerify) {
            if (obj.img_before.length == 0 && obj.img_before_old.length == 0) {
                abp.message.warn("Vui lòng upload ảnh thông tin giấy tờ 1");
                return false
            }
            let tp =  ctrl.form.find("[name='idType']:checked").attr('doc');
            if (tp !== "PASSPORT" && (obj.img_after.length == 0 && obj.img_after_old.length == 0)) {
                abp.message.warn("Vui lòng upload ảnh thông tin giấy tờ 2");
                return false
            }
        }
        return true;
    },
    // Submit
    nextToStep: function () {
        if (!ctrl.valid()) {
            return false;
        }
        var obj = ctrl.getFormValue();
        //console.log(obj) 
        Sv.RequestStart();
        ctrl.uploadFile(function (url_before, url_after) {
            Sv.Post({
                Url: abp.appPath + 'Profile/UpdateCurrentUserProfile',
                Data: {
                    name: obj.name,
                    surname: obj.surname,
                    cityId: obj.cityId,
                    districtId: obj.districtId,
                    wardId: obj.wardId,
                    address: obj.address,
                    idType: obj.idType,
                    identityId: obj.identityId,
                    agentName: obj.agentName,
                    frontPhoto: url_before,
                    backSitePhoto: url_after,
                    url_before: obj.img_before_old,
                    url_after: obj.img_after_old,
                    IsUpdateVerify: obj.isOpenVerify,
                    extraInfo: obj.extraInfo
                },
            }, function (rs) {
                Sv.serverIsAgencyVerify(false);
                clearLocalStorage(['identity_info']);
                let msg = "";
                console.log(obj);
                if (obj.isOpenVerify && obj.isVerifyAccount == 0) {
                    msg = "Xác thực tài khoản thành công";
                } else {
                    msg = "Cập nhật thông tin tài khoản thành công";
                }
                abp.message.success(msg)
                    .done(function () {
                        window.location.href = "/Profile";
                    });
                Sv.RequestEnd();
            }, function (e) {
                Sv.RequestEnd();
                abp.message.info(e);
            });
        });
    },

    eventChangeAvatar: function () {
        $('.main').find('.section-top-middle a >img').on("click", function (event) {
            $('form#profileAvatar').find('input[type=file]').trigger('click');
            $('form#profileAvatar').find('input[type=file]').off('change').on('change', ctrl.uploadAvatar);
        });
    },
    uploadAvatar: function () {
        var $fileInput = $('form#profileAvatar').find('input[type=file]');
        var files = $fileInput.get()[0].files;
        if (!files.length) {
            abp.message.info("Vui lòng chọn file ảnh");
            return false;
        }
        var file = files[0];
        //File type check
        var type = '|' + file.type.slice(file.type.lastIndexOf('/') + 1) + '|';
        if ('|jpg|jpeg|png|gif|'.indexOf(type) === -1) {
            abp.message.info(app.localize('ProfilePicture_Warn_FileType'));
            return false;
        }
        //File size check
        if (file.size > 20971520) //20MB
        {
            abp.message.info(app.localize('ProfilePicture_Warn_SizeLimit', 20));
            return false;
        }
        var formData = new FormData();
        formData.append("file", file);
        formData.append("FileType", file.type);
        formData.append("FileName", file.name);
        formData.append("FileToken", app.guid());

        Sv.AjaxPostFile({
            Url: abp.appPath + "Profile/UploadProfilePicture",
            Data: formData
        }, function (response) {
            var avatarFilePath = abp.appPath + 'File/DownloadTempFile?fileToken=' + response.result.fileToken + '&fileName=' + response.result.fileName + '&fileType=' + response.result.fileType + '&v=' + new Date().valueOf();
            var inputUpdateAvatar = {
                fileToken: response.result.fileToken,
                x: 0,
                y: 0,
                width: response.result.width,
                height: response.result.height,
            };
            Sv.Api({
                Url: abp.appPath + 'api/services/app/Profile/UpdateProfilePicture',
                type: 'PUT',
                Data: inputUpdateAvatar
            }, function () {
                $('.main').find('.section-top-middle a >img').attr('src', avatarFilePath);
                abp.message.success("Cập nhật ảnh đại diện thành công");
            }, function () {
                abp.message.error("Có lỗi trong quá trình xử lý!");
            });
        }, function () {
            abp.message.error("Upload file lỗi!");
        });
    },
    enterHandler: function () {
        ctrl.form.find('input, select').on("keyup", function (event) {
            if (event.keyCode === 13) {
                event.preventDefault();
                ctrl.nextToStep();
                return false;
            }
        });
    },
    back: function () {
        window.location.href = "/Profile";
    },
    uploadFile: function (callback) {
        if ($('#img_before').length === 0 || $('#img_after').length === 0) {
            callback("", "");
            return;
        }
        let img_before = $('#img_before')[0].files;
        let img_after = $('#img_after')[0].files;
        if (img_before.length === 0 && img_after.length === 0) {
            callback("", "");
            return;
        }
        let indexBefore = -1;
        let indexAfter = -1;
        let formData = new FormData();
        if (img_before.length > 0) {
            formData.append("files", img_before[0]);
            indexBefore = 0;
        }
        if (img_after.length > 0) {
            formData.append("files", img_after[0]);
            if (img_before.length > 0)
                indexAfter = 1;
            else
                indexAfter = 0;
        }
        $.ajax({
            "url": "/api/services/app/File/UploadFiles",
            "method": "POST",
            "timeout": 0,
            "processData": false,
            "mimeType": "multipart/form-data",
            "contentType": false,
            "data": formData
        }).done(function (response) {
            var rs = JSON.parse(response);
            let before_url = "";
            let after_url = "";
            if (rs != null && rs.result != null) {
                before_url = indexBefore > -1 ? rs.result[indexBefore] : "";
                after_url = indexAfter > -1 ? rs.result[indexAfter] : "";
            }
            callback(before_url, after_url);
        });

    },
    showVerifyAccount: function () {
        $("#blockVerify").slideToggle("none", ctrl.textVerifyAccount);
    },
    textVerifyAccount: function () {
        let isOpenVerify = $("#blockVerify").is(":visible");
        let isVerifyAccount = $("#blockVerify [name=isVerifyAccount]").val();
        if (isVerifyAccount == "0") {
            $("a.action_verify").html(isOpenVerify ? "Ẩn xác thực" : "Xác thực ngay");
        } else {
            $("a.action_verify").html(isOpenVerify ? "Ẩn thông tin xác thực" : "Xem thông tin xác thực");
        }
    },
    readURL: function ($input, preview) {
        const $ = jQuery;
        const $preview = $('#' + preview);
        if ($input.files && $input.files[0]) {
            let reader = new FileReader();
            var file = $input.files[0];
            //File type check
            var type = '|' + file.type.slice(file.type.lastIndexOf('/') + 1) + '|';
            //console.log(file.type);
            if ('|jpg|jpeg|png|gif|'.indexOf(type) === -1) {
                $($input).val('');
                abp.message.info(app.localize('ProfilePicture_Warn_FileType'));
                return false;
            }
            //File size check
            if (file.size > 20971520) //20MB
            {
                abp.message.info(app.localize('ProfilePicture_Warn_SizeLimit', 20));
                return false;
            }
            reader.onload = function (e) {
                let $img = $preview.find('img');
                if ($img.length === 0) {
                    $preview.append('<img  alt=""/>');
                    $img = $preview.find('img');
                }
                $img.attr('src', e.target.result); 
                bindVerify.detectImage($input.files[0])
                    .then(function (re) { 
                        bindVerify.suggestUserInfo(re, preview);
                        let data = JSON.parse(re);
                        console.log(data);
                        if (preview == "pre_before") {
                            if (data.id_type == "0" && data.id != null) {
                                $("#blockVerify [name='identityId']").val(data.id);
                            }
                        }
                        if (data.document != null) {
                            $("#blockVerify [name='idType']").prop('checked', false);
                            var $elm = $("#blockVerify [name='idType'][doc='" + data.document + "']");
                            if ($elm.length > 0) {
                                $elm.prop('checked', true);
                            }
                            ctrl.form.find("[name='idType']").trigger('change'); 
                        }
                    });
            };
            reader.readAsDataURL($input.files[0]); // convert to base64 string
            $preview.find('i').remove();
        } else {
            if ($preview.find('img').length > 0) {
                const src = $preview.data('src');
                if (src.length > 0) {
                    $preview.find('img').attr('src', src);
                    $preview.find('i').remove();
                } else {
                    $preview.find('img').remove();
                    $preview.append('<i class="fas fa-camera"></i>');
                }
            }
        }
    },
    changeIdType: function () {
       ctrl.form.find("[name='idType']").on('change', function(e){
          let val = ctrl.form.find("[name='idType']:checked").attr('doc');
          if(val === "PASSPORT"){
              ctrl.form.find("#img_after").closest('.form-group ').hide(); 
          }else{ 
              ctrl.form.find("#img_after").closest('.form-group ').show(); 
          }
       });
    }
}

$(document).ready(function () {
    clearLocalStorage(['identity_info']);
    ctrl.initValid();
    ctrl.enterHandler();
    ctrl.eventChangeAvatar();
    bindVerify.init();
    ctrl.textVerifyAccount();
    ctrl.changeIdType(); 
});
