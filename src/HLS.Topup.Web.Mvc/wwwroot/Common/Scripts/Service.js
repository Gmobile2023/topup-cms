$.fn.modal.Constructor.prototype._enforceFocus = function () {
    var $modalElement = this.$element;
    if ($modalElement) {
        $(document).on('focusin.modal', function (e) {
            if ($modalElement[0] !== e.target
                && !$modalElement.has(e.target).length
                && $(e.target).parentsUntil('*[role="dialog"]').length === 0) {
                $modalElement.focus();
            }
        });
    }
};

//serializeFormToObject plugin for jQuery
$.fn.serializeFormToObject = function () {
    //serialize to array
    var data = $(this).serializeArray();

    //add also disabled items
    $(':disabled[name]', this).each(function () {
        data.push({ name: this.name, value: $(this).val() });
    });

    //map to object
    var obj = {};
    data.map(function (x) {
        obj[x.name] = x.value;
    });

    return obj;
};

function removeNaValue(obj) {
    let newObj = {};
    for (var k in obj) {
        let val = obj[k];
        if (val != undefined) {
            if (typeof (val) == "object")
                newObj[k] = removeNaValue(val);
            else if (val.toString().toLowerCase() !== "n/a") {
                newObj[k] = val;
            }
        }
    }
    return newObj;
}

function getLocalStorage(key) {
    return localStorage.getItem(key)
}

function setLocalStorage(key, val) {
    localStorage.setItem(key, val);
}

function clearLocalStorage(data) {
    $.each(data, function (index, val) {
        localStorage.setItem(data[index], '');
    });
}

var Service = function () {
    var base = this;

    /**
     * Create a web friendly URL slug from a string.
     *
     * Requires XRegExp (http://xregexp.com) with unicode add-ons for UTF-8 support.
     *
     * Although supported, transliteration is discouraged because
     *     1) most web browsers support UTF-8 characters in URLs
     *     2) transliteration causes a loss of information
     *
     * @author Sean Murphy <sean@iamseanmurphy.com>
     * @copyright Copyright 2012 Sean Murphy. All rights reserved.
     * @license http://creativecommons.org/publicdomain/zero/1.0/
     *
     * @param string s
     * @param object opt
     * @return string
     */
    this.Url_slug = function (s, opt) {
        s = String(s);
        opt = Object(opt);

        var defaults = {
            'delimiter': '-',
            'limit': undefined,
            'lowercase': true,
            'replacements': {},
            'transliterate': (typeof (XRegExp) === 'undefined') ? true : false
        };

        // Merge options
        for (var k in defaults) {
            if (!opt.hasOwnProperty(k)) {
                opt[k] = defaults[k];
            }
        }

        var char_map = {
            // Latin
            'À': 'A', 'Á': 'A', 'Ả': 'A', 'Ã': 'A', 'Ạ': 'A',
            'Â': 'A', 'Ầ': 'A', 'Ấ': 'A', 'Ẩ': 'A', 'Ẫ': 'A', 'Ậ': 'A',
            'Ă': 'A', 'Ằ': 'A', 'Ắ': 'A', 'Ẳ': 'A', 'Ẵ': 'A', 'Ặ': 'A',
            'Ì': 'I', 'Í': 'I', 'Ỉ': 'I', 'Ĩ': 'I', 'Ị': 'I', 'Î': 'I', 'Ï': 'I',
            'Ù': 'U', 'Ú': 'U', 'Ủ': 'U', 'Ũ': 'U', 'Ụ': 'U',
            'Ư': 'U', 'Ừ': 'U', 'Ứ': 'U', 'Ử': 'U', 'Ữ': 'U', 'Ự': 'U',
            'È': 'E', 'É': 'E', 'Ẻ': 'E', 'Ẽ': 'E', 'Ẹ': 'E',
            'Ê': 'E', 'Ề': 'E', 'Ế': 'E', 'Ể': 'E', 'Ễ': 'E', 'Ệ': 'E',
            'Ỳ': 'Y', 'Ý': 'Y', 'Ỷ': 'Y', 'Ỹ': 'Y', 'Ỵ': 'Y',
            'Ò': 'O', 'Ó': 'O', 'Ỏ': 'O', 'Õ': 'O', 'Ọ': 'O',
            'Ô': 'O', 'Ồ': 'O', 'Ố': 'O', 'Ổ': 'O', 'Ỗ': 'O', 'Ộ': 'O',
            'Ơ': 'O', 'Ờ': 'O', 'Ớ': 'O', 'Ở': 'O', 'Ỡ': 'O', 'Ợ': 'O',


            'à': 'a', 'á': 'a', 'ả': 'a', 'ã': 'a', 'ạ': 'a',
            'â': 'a', 'ầ': 'a', 'ấ': 'a', 'ẩ': 'a', 'ẫ': 'a', 'ậ': 'a',
            'ă': 'a', 'ằ': 'a', 'ắ': 'a', 'ẳ': 'a', 'ẵ': 'a', 'ặ': 'a',
            'ì': 'i', 'í': 'i', 'ỉ': 'i', 'ĩ': 'i', 'ị': 'i', 'î': 'i', 'ï': 'i',
            'ù': 'u', 'ú': 'u', 'ủ': 'u', 'ũ': 'u', 'ụ': 'u',
            'ư': 'u', 'ừ': 'u', 'ứ': 'u', 'ử': 'u', 'ữ': 'u', 'ự': 'u',
            'è': 'e', 'é': 'e', 'ẻ': 'e', 'ẽ': 'e', 'ẹ': 'e',
            'ê': 'e', 'ề': 'e', 'ế': 'e', 'ể': 'e', 'ễ': 'e', 'ệ': 'e',
            'ỳ': 'y', 'ý': 'y', 'ỷ': 'y', 'ỹ': 'y', 'ỵ': 'y',
            'ò': 'o', 'ó': 'o', 'ỏ': 'o', 'õ': 'o', 'ọ': 'o',
            'ô': 'o', 'ồ': 'o', 'ố': 'o', 'ổ': 'o', 'ỗ': 'o', 'ộ': 'o',
            'ơ': 'o', 'ờ': 'o', 'ớ': 'o', 'ở': 'o', 'ỡ': 'o', 'ợ': 'o',
            'đ': 'd',

            'Ä': 'A', 'Å': 'A', 'Æ': 'AE', 'Ç': 'C',

            'È': 'E', 'É': 'E', 'Ê': 'E', 'Ë': 'E',
            'Đ': 'D', 'Ñ': 'N', 'Ô': 'O', 'Õ': 'O', 'Ö': 'O', 'Ő': 'O',
            'Ø': 'O', 'Ù': 'U', 'Ú': 'U', 'Û': 'U', 'Ü': 'U', 'Ű': 'U', 'Ý': 'Y', 'Þ': 'TH',
            'ß': 'ss',
            'à': 'a', 'á': 'a', 'â': 'a', 'ã': 'a', 'ä': 'a', 'å': 'a', 'æ': 'ae', 'ç': 'c',
            'è': 'e', 'é': 'e', 'ê': 'e', 'ë': 'e', 'ì': 'i', 'í': 'i', 'î': 'i', 'ï': 'i',
            'ð': 'd', 'ñ': 'n', 'ò': 'o', 'ó': 'o', 'ô': 'o', 'õ': 'o', 'ö': 'o', 'ő': 'o',
            'ø': 'o', 'ù': 'u', 'ú': 'u', 'û': 'u', 'ü': 'u', 'ű': 'u', 'ý': 'y', 'þ': 'th',
            'ÿ': 'y',

            // Latin symbols
            '©': '(c)',

            // Greek
            'Α': 'A', 'Β': 'B', 'Γ': 'G', 'Δ': 'D', 'Ε': 'E', 'Ζ': 'Z', 'Η': 'H', 'Θ': '8',
            'Ι': 'I', 'Κ': 'K', 'Λ': 'L', 'Μ': 'M', 'Ν': 'N', 'Ξ': '3', 'Ο': 'O', 'Π': 'P',
            'Ρ': 'R', 'Σ': 'S', 'Τ': 'T', 'Υ': 'Y', 'Φ': 'F', 'Χ': 'X', 'Ψ': 'PS', 'Ω': 'W',
            'Ά': 'A', 'Έ': 'E', 'Ί': 'I', 'Ό': 'O', 'Ύ': 'Y', 'Ή': 'H', 'Ώ': 'W', 'Ϊ': 'I',
            'Ϋ': 'Y',
            'α': 'a', 'β': 'b', 'γ': 'g', 'δ': 'd', 'ε': 'e', 'ζ': 'z', 'η': 'h', 'θ': '8',
            'ι': 'i', 'κ': 'k', 'λ': 'l', 'μ': 'm', 'ν': 'n', 'ξ': '3', 'ο': 'o', 'π': 'p',
            'ρ': 'r', 'σ': 's', 'τ': 't', 'υ': 'y', 'φ': 'f', 'χ': 'x', 'ψ': 'ps', 'ω': 'w',
            'ά': 'a', 'έ': 'e', 'ί': 'i', 'ό': 'o', 'ύ': 'y', 'ή': 'h', 'ώ': 'w', 'ς': 's',
            'ϊ': 'i', 'ΰ': 'y', 'ϋ': 'y', 'ΐ': 'i',

            // Turkish
            'Ş': 'S', 'İ': 'I', 'Ç': 'C', 'Ü': 'U', 'Ö': 'O', 'Ğ': 'G',
            'ş': 's', 'ı': 'i', 'ç': 'c', 'ü': 'u', 'ö': 'o', 'ğ': 'g',

            // Russian
            'А': 'A', 'Б': 'B', 'В': 'V', 'Г': 'G', 'Д': 'D', 'Е': 'E', 'Ё': 'Yo', 'Ж': 'Zh',
            'З': 'Z', 'И': 'I', 'Й': 'J', 'К': 'K', 'Л': 'L', 'М': 'M', 'Н': 'N', 'О': 'O',
            'П': 'P', 'Р': 'R', 'С': 'S', 'Т': 'T', 'У': 'U', 'Ф': 'F', 'Х': 'H', 'Ц': 'C',
            'Ч': 'Ch', 'Ш': 'Sh', 'Щ': 'Sh', 'Ъ': '', 'Ы': 'Y', 'Ь': '', 'Э': 'E', 'Ю': 'Yu',
            'Я': 'Ya',
            'а': 'a', 'б': 'b', 'в': 'v', 'г': 'g', 'д': 'd', 'е': 'e', 'ё': 'yo', 'ж': 'zh',
            'з': 'z', 'и': 'i', 'й': 'j', 'к': 'k', 'л': 'l', 'м': 'm', 'н': 'n', 'о': 'o',
            'п': 'p', 'р': 'r', 'с': 's', 'т': 't', 'у': 'u', 'ф': 'f', 'х': 'h', 'ц': 'c',
            'ч': 'ch', 'ш': 'sh', 'щ': 'sh', 'ъ': '', 'ы': 'y', 'ь': '', 'э': 'e', 'ю': 'yu',
            'я': 'ya',

            // Ukrainian
            'Є': 'Ye', 'І': 'I', 'Ї': 'Yi', 'Ґ': 'G',
            'є': 'ye', 'і': 'i', 'ї': 'yi', 'ґ': 'g',

            // Czech
            'Č': 'C', 'Ď': 'D', 'Ě': 'E', 'Ň': 'N', 'Ř': 'R', 'Š': 'S', 'Ť': 'T', 'Ů': 'U',
            'Ž': 'Z',
            'č': 'c', 'ď': 'd', 'ě': 'e', 'ň': 'n', 'ř': 'r', 'š': 's', 'ť': 't', 'ů': 'u',
            'ž': 'z',

            // Polish
            'Ą': 'A', 'Ć': 'C', 'Ę': 'e', 'Ł': 'L', 'Ń': 'N', 'Ó': 'o', 'Ś': 'S', 'Ź': 'Z',
            'Ż': 'Z',
            'ą': 'a', 'ć': 'c', 'ę': 'e', 'ł': 'l', 'ń': 'n', 'ó': 'o', 'ś': 's', 'ź': 'z',
            'ż': 'z',

            // Latvian
            'Ā': 'A', 'Č': 'C', 'Ē': 'E', 'Ģ': 'G', 'Ī': 'i', 'Ķ': 'k', 'Ļ': 'L', 'Ņ': 'N',
            'Š': 'S', 'Ū': 'u', 'Ž': 'Z',
            'ā': 'a', 'č': 'c', 'ē': 'e', 'ģ': 'g', 'ī': 'i', 'ķ': 'k', 'ļ': 'l', 'ņ': 'n',
            'š': 's', 'ū': 'u', 'ž': 'z'
        };

        // Make custom replacements
        for (var k in opt.replacements) {
            s = s.replace(RegExp(k, 'g'), opt.replacements[k]);
        }

        // Transliterate characters to ASCII
        if (opt.transliterate) {
            for (var k in char_map) {
                s = s.replace(RegExp(k, 'g'), char_map[k]);
            }
        }
        // Replace non-alphanumeric characters with our delimiter
        var alnum = (typeof (XRegExp) === 'undefined') ? RegExp('[^a-z0-9]+', 'ig') : XRegExp('[^\\p{L}\\p{N}]+', 'ig');
        s = s.replace(alnum, opt.delimiter);
        // Remove duplicate delimiters
        s = s.replace(RegExp('[' + opt.delimiter + ']{2,}', 'g'), opt.delimiter);
        // Truncate slug to max. characters
        s = s.substring(0, opt.limit);
        // Remove delimiter from ends
        s = s.replace(RegExp('(^' + opt.delimiter + '|' + opt.delimiter + '$)', 'g'), '');
        // Append random number
        if (opt.AppendRandMumber)
            s = s + '-' + Math.floor(Math.random() * 999999) + 1
        return opt.lowercase ? s.toLowerCase() : s;
    }

    this.convertLatinSymbols = function (s) {
        return base.Url_slug(s, { delimiter: " ", lowercase: false });
    }

    this.isMobile = function () {
        var check = false;
        (function (a) {
            if (/(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino|android|ipad|playbook|silk/i.test(a) || /1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-/i.test(a.substr(0, 4))) check = true;
        })(navigator.userAgent || navigator.vendor || window.opera);
        return check;
    }


    this.Lang = function (key) {
        //get multi language from xml
        return abp.localization.localize(key, "SaleSystem");
    }
    this.OtpTimeOut = parseInt(abp.setting.get("App.UserManagement.OtpSetting.OtpTimeOut"));
    this.toCamel = function (o) {
        var newO, origKey, newKey, value
        if (o instanceof Array) {
            return o.map(function (value) {
                if (typeof value === "object") {
                    value = base.toCamel(value)
                }
                return value
            })
        } else {
            newO = {}
            for (origKey in o) {
                if (o.hasOwnProperty(origKey)) {
                    newKey = (origKey.charAt(0).toLowerCase() + origKey.slice(1) || origKey).toString()
                    value = o[origKey]
                    if (value instanceof Array || (value !== null && value.constructor === Object)) {
                        value = base.toCamel(value)
                    }
                    newO[newKey] = value
                }
            }
        }
        return newO
    }

    this.toPascal = function (o) {
        var newO, origKey, newKey, value
        if (o instanceof Array) {
            return o.map(function (value) {
                if (typeof value === "object") {
                    value = base.toPascal(value)
                }
                return value
            })
        } else {
            newO = {}
            for (origKey in o) {
                if (o.hasOwnProperty(origKey)) {
                    newKey = (origKey.charAt(0).toUpperCase() + origKey.slice(1) || origKey).toString()
                    value = o[origKey]
                    if (value instanceof Array || (value !== null && value.constructor === Object)) {
                        value = base.toPascal(value)
                    }
                    newO[newKey] = value
                }
            }
        }
        return newO
    }


    this.ClubMemberType = function (value) {
        if (value == 1) {
            return app.localize("Operator");
        }
        if (value == 2) {
            return app.localize("CoOperator");
        }
        if (value == 3) {
            return app.localize("Member");
        }

        return ""
        // return Sv.BootstrapTableSTT(base.$table, index);
    }

    this.MemberType = function (value) {
        if (value == 1) {
            return app.localize("AccountNotAuthenticated");
        }
        if (value == 2) {
            return app.localize("AccountAuthenticated");
        }
        if (value == 3) {
            return app.localize("HAB");
        }
        if (value == 4) {
            return app.localize("HABO");
        }
        //if (value == 5) {
        //    return app.localize("Tab");
        //}
        //if (value == 6) {
        //    return app.localize("PT");
        //}
        return ""
        // return Sv.BootstrapTableSTT(base.$table, index);
    }

    this.ChecPermission = function (permission, callback) {
        //  callback();
        //return true;
        base.AuthenAjaxPost({
            Url: window.location.href + "/CheckPermission?permission=" + permission.toUpperCase()
        }, function (rs) {
            if (rs.code == "00") {
                alert(rs.message);
                return false;
            } else {
                callback();
                return true;
            }
        });
        return false;
    }
    this.CheckAuthen = function (option, fnSuccess, fnError) {
        $.ajax({
            url: option.Url,
            type: 'Post',
            data: option.Data,
            beforeSend: function () {
                base.RequestStart();
            },
            async: (option.async == undefined ? true : option.async),
            complete: function () {
                base.RequestEnd();
            },
            success: function (rs) {
                if (!rs.result.isAuthen)
                    base.RequestEnd();
                if (typeof fnSuccess === "function")
                    fnSuccess(rs);
            },
            error: function (e) {
                if (!fnError)
                    Dialog.Alert("Có lỗi trong quá trình xử lý", Dialog.Error);
                if (typeof fnError === "function")
                    fnError(e);
                base.RequestEnd();
            }
        });
    }
    this.CheckAuthenAuto = function (option, fnSuccess, fnError) {
        $.ajax({
            url: option.Url,
            type: 'Post',
            data: option.Data,
            //beforeSend: function () {
            //    base.RequestStart();
            //},
            async: (option.async == undefined ? true : option.async),
            //complete: function () {
            //    base.RequestEnd();
            //},
            success: function (rs) {
                if (!rs.result.isAuthen)
                    //base.RequestEnd();
                    if (typeof fnSuccess === "function")
                        fnSuccess(rs);
            },
            error: function (e) {
                if (!fnError)
                    Dialog.Alert("Có lỗi trong quá trình xử lý", Dialog.Error);
                if (typeof fnError === "function")
                    fnError(e);
                //base.RequestEnd();
            }
        });
    }
    this.RequestStart = function () {
        // abp.ui.setBusy();
        $(".se-pre-con")
            .css('background-color', 'rgba(255,255,255,0.5)')
            .show();
    };
    this.EncodeHtml = function (string) {
        var entityMap = {
            "&": "&amp;",
            "<": "&lt;",
            ">": "&gt;",
            '"': '&quot;',
            "'": '&#39;',
            "/": '&#x2F;'
        };
        return String(string).replace(/[&<>"'\/]/g, function (s) {
            return entityMap[s];
        });
    };
    this.RequestEnd = function () {
        $(".se-pre-con").fadeOut("slow");
        //abp.ui.clearBusy();
        //setTimeout(function () {
        //    if ($("body").find("div.ajaxInProgress").length > 0)
        //        $("body").find("div.ajaxInProgress").hide();
        //    //$("body").find("div.ajaxInProgress").remove();
        //}, 200);
    }

    this.Api = function (option, fnSuccess, fnError) {
        var url = option.Url || option.url;
        var data = option.Data || option.data;
        var type = option.type || option.Type;
        if (type === undefined || type.length == 0) {
            type = "POST";
        }
        abp.ajax({
            beforeSend: function () {
                base.RequestStart();
            },
            type: type,
            async: (option.async == undefined ? true : option.async),
            complete: function () {
                base.RequestEnd();
            },
            url: url,
            data: JSON.stringify(data),
            success: fnSuccess,
            error: function (xhr, status, error) {
                if (typeof fnError === "function")
                    fnError(error);
            }
        });

    }

    this.Post = function (option, fnSuccess, fnError) {
        if (option.Data) {
            option.Data.__RequestVerificationToken = $("input[name=__RequestVerificationToken]").val();
        }
        //  console.log(option.Data);
        $.ajax({
            url: option.Url,
            type: 'Post',
            data: option.Data,
            //headers: {
            //    'X-Request-Verification-Token': verificationToken
            //},
            beforeSend: function () {
                base.RequestStart();
            },
            async: (option.async == undefined ? true : option.async),
            complete: function () {
                base.RequestEnd();
            },
            success: function (rs) {
                try {
                    if (rs.Code === "Redirect") {
                        window.location = rs.Url;
                    } else if (typeof fnSuccess === "function")
                        fnSuccess(rs);
                } catch (ex) {
                    if (typeof fnSuccess === "function")
                        fnSuccess(rs);
                }
            },
            error: function (e) {
                if (typeof fnError === "function")
                    fnError(e);
            }
        });
    }

    this.PostBg = function (option) {
        var url = option.Url || option.url;
        var data = option.Data || option.data;

        if (data) {
            data.__RequestVerificationToken = $("input[name=__RequestVerificationToken]").val();
        }
        //  console.log(option.Data);
        return $.ajax({
            url: url,
            type: 'Post',
            data: option.Data,
            async: (option.async == undefined ? true : option.async),
        });
    }
    this.Post2 = function (option) {
        var url = option.Url || option.url;
        var data = option.Data || option.data;
        if (data) {
            data.__RequestVerificationToken = $("input[name=__RequestVerificationToken]").val();
        }
        //  console.log(option.Data);
        return $.ajax({
            url: url,
            type: 'Post',
            data: data,
            async: (option.async == undefined ? true : option.async),
            beforeSend: function () {
                base.RequestStart();
            },
            complete: function () {
                base.RequestEnd();
            },
        });
    }

    this.AuthenAjaxPost = function (option, fnSuccess, fnError) {
        base.AjaxPost({
            Url: "/Home/CheckAuthen",
        }, function (rs) {
            if (!rs.result.isAuthen) {
                Dialog.Alert("Time out", Dialog.Error, "Session Timedout", function () {
                    window.location = "/Admin/login?ReturnUrl=" + window.location.href;
                });
            } else {
                base.AjaxPost(option, fnSuccess, fnError);
            }
        }, function (e) {
        });
    }
    this.encodeHtml = function (r) {
        return r.replace(/[\x26\x0A\<>'"]/g, function (r) {
            return "&#" + r.charCodeAt(0) + ";"
        });
    }
    //---- Ajax get file
    this.AjaxPostFile = function (option, fnSuccess, fnError) {
        if (option.Data) {
            option.Data.__RequestVerificationToken = $("input[name=__RequestVerificationToken]").val();
        }
        $.ajax({
            url: option.Url,
            type: 'Post',
            enctype: 'multipart/form-data',
            cache: false,
            contentType: false,
            processData: false,
            data: option.Data,
            dataType: 'json',

            beforeSend: function () {
                base.RequestStart();
            },
            async: (option.async == undefined ? true : option.async),
            complete: function () {
                base.RequestEnd();
            },
            success: function (rs) {
                if (typeof fnSuccess === "function")
                    fnSuccess(rs);
            },
            error: function (e) {
                if (!fnError)
                    Dialog.Alert("Có lỗi trong quá trình xử lý", Dialog.Error);
                if (typeof fnError === "function")
                    fnError(e);
            }
        });
    }
    this.AjaxPostFile2 = function (option) {
        let url = option.Url || option.url;
        let data = option.Data || option.data;
        let type = option.Type || option.type;
        if (type == null || type == undefined)
            type = "POST";
        if (data) {
            data.__RequestVerificationToken = $("input[name=__RequestVerificationToken]").val();
        }
        return $.ajax({
            url: url,
            type: type,
            enctype: 'multipart/form-data',
            cache: false,
            contentType: false,
            processData: false,
            data: data,
            dataType: 'json',
            beforeSend: function () {
                base.RequestStart();
            },
            async: (option.async == undefined ? true : option.async),
            complete: function () {
                base.RequestEnd();
            }
        });
    }
    //--Posst FormCollection
    this.AjaxPostFormCollection = function (option, fnSuccess, fnError) {
        if (option.Data) {
            option.Data.append("__RequestVerificationToken", $("input[name=__RequestVerificationToken]").val());
        } else {
            var data = new FormData();
            data.append("__RequestVerificationToken", $("input[name=__RequestVerificationToken]").val());
            option.Data = data;
        }
        if (option.CheckAuthen != false) {
            base.CheckAuthen({
                Url: "/Home/CheckAuthen",
            }, function (rs) {
                if (!rs.result.isAuthen) {
                    Dialog.Alert("Time out", Dialog.Error, "Session Timedout", function () {
                        window.location = "/Admin/login?ReturnUrl=" + window.location.href;
                    });
                } else {
                    $.ajax({
                        url: option.Url,
                        type: 'Post',
                        enctype: 'multipart/form-data',
                        cache: false,
                        contentType: false,
                        processData: false,
                        data: option.Data,
                        dataType: 'json',

                        beforeSend: function () {
                            base.RequestStart();
                        },
                        async: (option.async == undefined ? true : option.async),
                        complete: function () {
                            base.RequestEnd();
                        },
                        success: function (rs) {
                            if (typeof fnSuccess === "function")
                                fnSuccess(rs);
                        },
                        error: function (e) {
                            if (!fnError)
                                Dialog.Alert("Có lỗi trong quá trình xử lý", Dialog.Error);
                            if (typeof fnError === "function")
                                fnError(e);
                        }
                    });
                }
            }, function (e) {
            });
        } else {
            $.ajax({
                url: option.Url,
                type: 'Post',
                enctype: 'multipart/form-data',
                cache: false,
                contentType: false,
                processData: false,
                data: option.Data,
                dataType: 'json',

                beforeSend: function () {
                    base.RequestStart();
                },
                async: (option.async == undefined ? true : option.async),
                complete: function () {
                    base.RequestEnd();
                },
                success: function (rs) {
                    if (typeof fnSuccess === "function")
                        fnSuccess(rs);
                },
                error: function (e) {
                    if (!fnError)
                        Dialog.Alert("Có lỗi trong quá trình xử lý", Dialog.Error);
                    if (typeof fnError === "function")
                        fnError(e);
                }
            });
        }
    }

    this.AjaxPostNonAuthen = function (option, fnSuccess, fnError) {
        if (option.Data) {
            option.Data.__RequestVerificationToken = $("input[name=__RequestVerificationToken]").val();
        }
        //  console.log(option.Data);
        $.ajax({
            url: option.Url,
            type: 'Post',
            data: option.Data,
            //headers: {
            //    'X-Request-Verification-Token': verificationToken
            //},
            beforeSend: function () {
                base.RequestStart();
            },
            async: (option.async == undefined ? true : option.async),
            complete: function () {
                base.RequestEnd();
            },
            success: function (rs) {
                if (typeof fnSuccess === "function")
                    fnSuccess(rs);
            },
            error: function (e) {
                if (typeof fnError === "function")
                    fnError(e);
            }
        });
    }
    this.AjaxPost1 = function (option, fnSuccess, fnError) {
        if (option.CheckAuthen === false) {
            base.Post(option, fnSuccess, fnError);
        } else {
            base.Post({
                Url: "/Home/CheckAuthen"
            }, function (rs) {
                if (!rs.result.isAuthen) {
                    Dialog.Alert("Phiên đăng nhập hết hạn", Dialog.Error, "Session Timedout", function () {
                        window.location = "/Account/Login?ReturnUrl=" + window.location.href;
                    });
                } else {
                    base.Post(option, fnSuccess, fnError);
                }
            }, function (e) {
            });
        }
    }
    this.AjaxPost = function (option, fnSuccess, fnError) {
        if (option.CheckAuthen === false) {
            base.Post(option, fnSuccess, fnError);
        } else {
            base.Post({
                Url: "/Home/CheckAuthen"
            }, function (rs) {
                if (!rs.result.isAuthen) {
                    Dialog.Alert("Phiên đăng nhập hết hạn", Dialog.Error, "Session Timedout", function () {
                        window.location = "/Account/Login?ReturnUrl=" + window.location.href;
                    });
                } else {
                    base.Post(option, fnSuccess, fnError);
                }
            }, function (e) {
            });
        }
    }
    this.AjaxPostSearch = function (option, fnSuccess, fnError) {
        if (option.Data) {
            option.Data.__RequestVerificationToken = $("input[name=__RequestVerificationToken]").val();
        }
        //  console.log(option.Data);
        var me = $(this);
        if (me.data('requestRunning')) {
            return;
        }
        me.data('requestRunning', true);
        $.ajax({
            url: option.Url,
            type: 'Post',
            data: option.Data,
            //headers: {
            //    'X-Request-Verification-Token': verificationToken
            //},
            beforeSend: function () {
                base.RequestStart();
            },
            async: (option.async == undefined ? true : option.async),
            complete: function () {
                me.data('requestRunning', false);
                base.RequestEnd();
            },
            success: function (rs) {
                if (typeof fnSuccess === "function")
                    fnSuccess(rs);
            },
            error: function (e) {
                if (typeof fnError === "function")
                    fnError(e);
            }
        });
    }
    this.AjaxPostList = function (option, fnSuccess, fnError) {
        if (option.Data) {
            option.Data.__RequestVerificationToken = $("input[name=__RequestVerificationToken]").val();
        }
        $.ajax({
            url: option.Url,
            type: 'Post',
            data: option.Data,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            beforeSend: function () {
                base.RequestStart();
            },
            complete: function () {
                base.RequestEnd();
            },
            success: function (rs) {
                if (typeof fnSuccess === "function")
                    fnSuccess(rs);
            },
            error: function (e) {
                if (typeof fnError === "function")
                    fnError(e);
            }
        });
    }

    this.AjaxGet = function (option, fnSuccess, fnError) {

        if (option.Data) {
            option.Data.__RequestVerificationToken = $("input[name=__RequestVerificationToken]").val();
        }
        $.ajax({
            url: option.Url,
            type: 'Get',
            data: option.Data,
            beforeSend: function () {
                base.RequestStart();
            },
            complete: function () {
                base.RequestEnd();
            },
            success: function (rs) {
                if (typeof fnSuccess === "function")
                    fnSuccess(rs);
            },
            error: function (e) {
                if (typeof fnError === "function")
                    fnError(e);
            }
        });
    }

    this.JoinObject = function (oldObj, newObj) {
        if (typeof oldObj === "object" && oldObj != null
            && typeof newObj === "object" && newObj != null) {
            for (var key in newObj) {
                if (newObj.hasOwnProperty(key)) {
                    oldObj[key] = newObj[key];
                }
            }
        }
        return oldObj;
    }
    //
    // this.NumberToString = function (value) {
    //     return value != null ? value.toString().replace('.', ',').replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1.') : 0;
    // };

    this.NumberToString = function (value) {
        if (value == null || value.toString() == "0" || value.length == 0)
            return "0";
        let val = parseFloat(value + "");
        return Math.round(val).toString().replace('.', ',').replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1.');
    };


    this.DateToString = function (value, fomart) {
        return moment(new Date(parseInt(value.slice(6, -2)))).format(fomart);
    };

    //==================================================================
    //	Description:  Bootstrap Table					 Option, function
    this.Sprintf = function (str) {
        var args = arguments,
            flag = true,
            i = 1;

        str = str.replace(/%s/g, function () {
            var arg = args[i++];

            if (typeof arg === 'undefined') {
                flag = false;
                return '';
            }
            return arg;
        });
        return flag ? str : '';
    };

    this.BootstrapTableOption1 = function (option) {
        var obj = {
            locale: 'vi-VN',
            classes: 'table table-condensed', // table-hover
            cache: false,
            pagination: true,
            pageSize: 15,
            limit: 15,
            pageList: [15, 20, 30, 50, 100],
            formatLoadingMessage: function () {
                return app.localize("Loading");
                //return '<div class="ajaxInProgress"> <div class="loading-ct" >' +
                //    '<img src="/images/AjaxLoader.gif">' +
                //    '<div>' + "Loadding" + '</div>' +
                //    '</div> </div>';
            },

            method: 'post',
            sidePagination: 'server',
            queryParams: function (params) {
                return params;
            },
            responseHandler: function (res) {
                return {
                    total: res.total,
                    rows: res.data
                };
            },
        };

        return base.JoinObject(obj, option);
    }
    this.getUrlParameter = function (sParam) {
        var sPageURL = decodeURIComponent(window.location.search.substring(1)),
            sURLVariables = sPageURL.split('&'),
            sParameterName,
            i;

        for (i = 0; i < sURLVariables.length; i++) {
            sParameterName = sURLVariables[i].split('=');

            if (sParameterName[0] === sParam) {
                return sParameterName[1] === undefined ? true : sParameterName[1];
            }
        }
    };
    this.BootstrapTableOption = function (option) {
        var obj = {
            locale: 'vi',
            classes: 'table table-hover table-condensed',
            refresh: true,
            cache: false,
            striped: true,
            height: 'auto',
            pagination: true,
            pageSize: 10,
            pageList: [10, 15, 20, 30, 50, 100],
            //paginationFirstText: 'Trang đầu',
            //paginationPreText: 'Trước',
            //paginationNextText: 'Sau',
            //paginationLastText: 'Trang cuối',
            //showFooter: false,
            //formatShowingRows: function (pageFrom, pageTo, totalRows) {
            //    return base.Sprintf(base.Lang('Total') + ': %s ', totalRows);
            //},
            //formatRecordsPerPage: function (pageNumber) {
            //    return base.Sprintf(base.Lang('ShowRow'), pageNumber);
            //},
            formatLoadingMessage: function () {
                //abp.ui.setBusy();
                return app.localize("Loading");
                //return '<div class="ajaxInProgress"> <div class="loading-ct" >' +
                //    '<img src="/images/AjaxLoader.gif">' +
                //    '<div>' + "Loadding" + '</div>' +
                //    '</div> </div>';
            },
            formatNoMatches: function () {
                return base.Lang('NoData');
            },
            method: 'post',
            sidePagination: 'server',
            queryParams: function (params) {
                return params;
            },
            responseHandler: function (res) {
                //console.log('đâsdsadsad' + res.result.total);
                //console.log('đâsdsadsad' + res.result.items);
                //abp.ui.clearBusy();
                if (res.result.total == 0) {
                    return {
                        total: 0,
                        rows: []
                    };
                } else {
                    return {
                        total: res.result.total,
                        rows: res.result.data
                    };
                }
            }
        };

        return base.JoinObject(obj, option);
    }

    this.BootstrapTableOptionSync = function (option) {
        var obj = {
            locale: 'vi-VN',
            classes: 'table table-hover table-condensed',
            refresh: true,
            cache: false,
            striped: true,
            height: 'auto',
            pagination: true,
            pageSize: 10,
            pageList: [10, 15, 20, 30, 50, 100],
            //paginationFirstText: 'Trang đầu',
            //paginationPreText: 'Trước',
            //paginationNextText: 'Sau',
            //paginationLastText: 'Trang cuối',
            //showFooter: false,
            //formatShowingRows: function (pageFrom, pageTo, totalRows) {
            //    return base.Sprintf(base.Lang('Total') + ': %s ', totalRows);
            //},
            //formatRecordsPerPage: function (pageNumber) {
            //    return base.Sprintf(base.Lang('ShowRow'), pageNumber);
            //},
            formatLoadingMessage: function () {
                return app.localize("Loading");

                //return '<div class="ajaxInProgress"> <div class="loading-ct" >' +
                //    '<img src="/images/AjaxLoader.gif">' +
                //    '<div>' + "Loadding" + '</div>' +
                //    '</div> </div>';
            },
            formatNoMatches: function () {
                return base.Lang('NoData');
            },
            method: 'post',
            sidePagination: 'server',
            queryParams: function (params) {
                return params;
            },
            responseHandler: function (res) {
                if (res.result.totalCount == 0) {
                    return {
                        total: 0,
                        rows: []
                    };
                } else {
                    return {
                        total: res.result.totalCount,
                        rows: res.result.items
                    };
                }
            }
        };

        return base.JoinObject(obj, option);
    }
    this.BootstrapTableOptionClient = function (option) {
        var obj = {
            locale: 'vi',
            classes: 'table table-condensed',
            pagination: true,
            height: 'auto',
            pageSize: 10,
            pageList: [10, 15, 20, 30, 50, 100],
            //showHeader: true,
            //sidePagination: 'client',
            //formatShowingRows: function (pageFrom, pageTo, totalRows) {
            //    return base.Sprintf('Tổng: %s', totalRows);
            //},
            //formatRecordsPerPage: function (pageNumber) {
            //    return base.Sprintf('Hiển thị %s dòng trên trang', pageNumber);
            //},
            formatLoadingMessage: function () {
                return app.localize("Loading");

                //return '<div class="ajaxInProgress"> <div class="loading-ct" >' +
                //    '<img src="/images/AjaxLoader.gif"></img>' +
                //    '<div>' + "Loading" + '</div>' +
                //    '</div> </div>';
            },
            formatNoMatches: function () {
                return 'Không có dữ liệu';
            }
        };

        return base.JoinObject(obj, option);
    }

    this.BootstrapTableOptionServiceSync = function (option) {
        var obj = {
            locale: 'vi',
            classes: 'table table-hover table-condensed',
            refresh: true,
            cache: false,
            striped: true,
            height: 'auto',
            pagination: true,
            pageSize: 10,
            pageList: [10, 15, 20, 30, 50, 100],
            //paginationFirstText: 'Trang đầu',
            //paginationPreText: 'Trước',
            //paginationNextText: 'Sau',
            //paginationLastText: 'Trang cuối',
            //showFooter: false,
            //formatShowingRows: function (pageFrom, pageTo, totalRows) {
            //    return base.Sprintf(base.Lang('Total') + ': %s ', totalRows);
            //},
            //formatRecordsPerPage: function (pageNumber) {
            //    return base.Sprintf(base.Lang('ShowRow'), pageNumber);
            //},
            formatLoadingMessage: function () {
                return app.localize("Loading");

                //return '<div class="ajaxInProgress"> <div class="loading-ct" >' +
                //    '<img src="/images/AjaxLoader.gif">' +
                //    '<div>' + "Loadding" + '</div>' +
                //    '</div> </div>';
            },
            formatNoMatches: function () {
                return base.Lang('NoData');
            },
            method: 'post',
            sidePagination: 'server',
            queryParams: function (params) {
                return params;
            },
            responseHandler: function (res) {
                var total = res.result.totalCount;
                var row = res.result.items;
                if (total == 0) {
                    return {
                        total: 0,
                        rows: []
                    };
                } else {
                    return {
                        total: total,
                        rows: row
                    };
                }
            }
        };

        return base.JoinObject(obj, option);
    }

    this.LoadTableSearchSyncService = function (table) {
        table.bootstrapTable('refreshOptions', {
            //url: "/CustomerManager/GetData",
            responseHandler: function (res) {
                var total = res.result.totalCount;
                var row = res.result.items;
                if (total === 0) {
                    table.bootstrapTable('removeAll');
                }
                return {
                    total: total,
                    rows: row
                };
            }
        });
    }
    this.LoadTableSearchSync = function (table) {
        table.bootstrapTable('refreshOptions',
            {
                responseHandler: function (res) {
                    if (res.result.total == 0) {
                        return {
                            total: 0,
                            rows: []
                        };
                    } else {
                        return {
                            total: res.result.total,
                            rows: res.result.items
                        };
                    }
                }
            });
    }
    this.BootstrapTableColumn = function (type, option) {
        var align = "";
        var formatFn;
        var className = "";
        if (typeof type === "function")
            type = type();
        switch (type) {
            case "Number":
                align = "right";
                className = "row-number";
                formatFn = function (value) {
                    return value ? value.toString().replace('.', ',').replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1.') : 0;
                }
                break;
            case "Number2":
                align = "right";
                className = "row-number";
                formatFn = function (value) {
                    return value ? value.toString().replace('.', ',').replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1.') : "";
                }
                break;
            case "NumberNull":
                align = "right";
                className = "row-number";
                formatFn = function (value) {
                    if (value == null) {
                        return "";
                    } else
                        return value ? value.toString().replace('.', ',').replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1.') : 0;
                }
                break;
            /* LongLD - phase 2 báo cáo cần view date ko */
            case "Date0":
                align = "center";
                className = "row-date";
                formatFn = function (value) {
                    return value ? moment(new Date(value)).format('DD/MM/YYYY') : "";
                }
                break;
            //case "DateTime0":
            //    align = "center";
            //    className = "row-date";
            //    formatFn = function (value) {
            //        return value ? moment(new Date(parseInt(value.slice(6, -2)))).format('DD/MM/YYYY HH:mm') : "";
            //    }
            //    break;
            /* LongLD - Dự án này áp dụng luôn 1 kiêu date format */
            case "DateTimeFull":
            case "Date":
            case "DateTime":
                align = "center";
                className = "row-date";
                formatFn = function (value) {
                    return value ? moment(new Date(value)).format('DD/MM/YYYY HH:mm:ss') : "";
                }
                break;
            default:
                align = "left";
                className = "row-string";
                formatFn = function (value) {
                    return value ? value : "";
                }
                break;
        }
        var obj = {
            align: align,
            valign: "middle",
            width: "100px",
            'class': className,
            formatter: formatFn,
        };
        return base.JoinObject(obj, option);
    }

    this.BootstrapTableSTT = function (table, index) {
        var option = table.bootstrapTable('getOptions');
        var i = (option.pageNumber - 1) * option.pageSize;
        return i + index + 1;
    }

    this.ResponseHandlerFooter = function (res) {
        var obj = {
            total: res.total,
            rows: res.data != null ? res.data : [],
        };
        obj.rows.Footer = res.footer;
        return obj;
    }

    this.ResponseHandlerSearch = function (res, $modalSearch, $table) {
        $modalSearch.modal("hide");
        if (res.total == 0) {
            $("body").css('padding-right', 0);
            $table.bootstrapTable('removeAll');
            Dialog.Alert("Dữ liệu trống", Dialog.Error);
        }
        return {
            total: res.total,
            rows: res.data
        };
    },

        this.ResetViewTable = function ($table) {
            $table.bootstrapTable('resetView');
        };
    this.SetupAmountMask = function () {
        //Mask_groupSeparator: '.',
        //Mask_radixPoint: ',',
        //Mask_integerDigits: 11,
        //Mask_digits: 0,
        $('.amount-mask,.price-mask').on().inputmask({
            alias: 'decimal',
            placeholder: '',
            groupSeparator: ".",
            radixPoint: ",",
            autoGroup: true,
            rightAlign: false,
            digits: "0",
            allowPlus: false,
            allowMinus: false,
            autoUnmask: true,
            integerDigits: "11"
        });
        $('.amount-mask2').on().inputmask({
            alias: 'decimal',
            placeholder: '',
            groupSeparator: ".",
            radixPoint: ",",
            autoGroup: true,
            digits: "0",
            allowPlus: true,
            allowMinus: true,
            autoUnmask: true,
            integerDigits: "1"
        });
        $('.decimal-mask').on().inputmask({
            alias: 'decimal',
            //mask: "99[.99]",
            rightAlign: true,
            groupSeparator: ".",
            radixPoint: ",",
            autoGroup: true
        });
        $('.discount-mask').inputmask({
            alias: "percentage",
            placeholder: '',
            radixPoint: ",",
            // digits: "0",
            autoUnmask: true
        });
        $('.percentage-mask').inputmask("percentage", {
            placeholder: '',
            radixPoint: ".",
            autoUnmask: true,
            allowMinus: false, /* Không cho nhập dấu trừ */
            allowPlus: false, /* Không cho nhập dấu cộng */
            rightAlign: false,
        });
        //$('.discount-mask').inputmask("percentage", {
        //    placeholder: '',
        //    radixPoint: ",",
        //    autoUnmask: true
        //});

        $(".amount-mask-currency").inputmask('currency', {
            alias: 'decimal',
            rightAlign: true,
            prefix: "",
            radixPoint: ".",
            //digits: "0"
        });
        $(".discount-mask-currency").inputmask('percentage', {
            radixPoint: ".",
            //digits: "0",
            autoUnmask: true
        });
    }
    this.DefaultDate = function () {
        var dfFormDate = new Date();
        var dfToDate = new Date();
        var dfMax = new Date();
        dfFormDate.setHours(0);
        dfFormDate.setMinutes(0);
        dfFormDate.setSeconds(0);
        dfFormDate.setMilliseconds(0);
        dfToDate.setHours(23);
        dfToDate.setMinutes(59);
        dfToDate.setSeconds(59);
        dfToDate.setMilliseconds(0);
        dfMax.setHours(23);
        dfMax.setMinutes(59);
        dfMax.setSeconds(59);
        dfMax.setMilliseconds(999);
        return {
            FormDate: dfFormDate,
            ToDate: dfToDate,
            MaxDate: dfMax,
            MomentFromDate: moment(dfFormDate),
            MomentToDate: moment(dfToDate),
            MomentMaxDate: moment(dfMax)
        }
    }
    this.SetupDateTime = function (input1, input2) {
        input1.datetimepicker({
            format: "DD/MM/YYYY HH:mm:ss",
            showTodayButton: true,
            maxDate: base.DefaultDate().MomentMaxDate,
            defaultDate: base.DefaultDate().MomentFromDate,
            showClose: true,
        });
        input2.datetimepicker({
            format: "DD/MM/YYYY HH:mm:ss",
            showTodayButton: true,
            maxDate: base.DefaultDate().MomentMaxDate,
            defaultDate: base.DefaultDate().MomentToDate,
            showClose: true,
        });
        //$('.datemask').inputmask({
        //    alias: '99/99/9999 99:99:99',
        //    placeholder: ""
        //});
        input1.on("focusout", function (e) {
            if (!moment($(e.target).val()).isValid()) {
                try {
                    input1.data("DateTimePicker").defaultDate(base.DefaultDate().FormDate);
                } catch (e) {
                    input1.data("DateTimePicker").defaultDate(input2.data("DateTimePicker").date());
                }
            }
        });
        input2.on("focusout", function (e) {
            if (!moment($(e.target).val()).isValid()) {
                try {
                    input2.data("DateTimePicker").defaultDate(base.DefaultDate().ToDate);
                } catch (e) {
                    input2.data("DateTimePicker").defaultDate(input1.data("DateTimePicker").date());
                }
            }
        });
    }
    this.SetupDateTimeNon = function (input1, input2) {
        input1.datetimepicker({
            format: "DD/MM/YYYY HH:mm:ss",
            showTodayButton: true,
            //maxDate: base.DefaultDate().MomentMaxDate,
            defaultDate: base.DefaultDate().MomentFromDate,
            showClose: true,
        });
        input2.datetimepicker({
            format: "DD/MM/YYYY HH:mm:ss",
            showTodayButton: true,
            //maxDate: base.DefaultDate().MomentMaxDate,
            defaultDate: base.DefaultDate().MomentToDate,
            showClose: true,
        });
        //$('.datemask').inputmask({
        //    alias: '99/99/9999 99:99:99',
        //    placeholder: ""
        //});
        input1.on("focusout", function (e) {
            if (!moment($(e.target).val()).isValid()) {
                try {
                    input1.data("DateTimePicker").defaultDate(base.DefaultDate().FormDate);
                } catch (e) {
                    input1.data("DateTimePicker").defaultDate(input2.data("DateTimePicker").date());
                }
            }
        });
        input2.on("focusout", function (e) {
            if (!moment($(e.target).val()).isValid()) {
                try {
                    input2.data("DateTimePicker").defaultDate(base.DefaultDate().ToDate);
                } catch (e) {
                    input2.data("DateTimePicker").defaultDate(input1.data("DateTimePicker").date());
                }
            }
        });
    }
    this.SetupDateTimeUptoDay = function (input2, day) {
        input2.datetimepicker({
            format: "DD/MM/YYYY HH:mm:ss",
            showTodayButton: true,
            //maxDate: base.DefaultDate().MomentMaxDate,
            defaultDate: moment().add(day, 'days'),
            showClose: true
        });
        input2.on("focusout", function (e) {
            if (!moment($(e.target).val()).isValid()) {
                try {
                    input2.data("DateTimePicker").defaultDate(base.DefaultDate.ToDate);
                } catch (e) {
                    input2.data("DateTimePicker").defaultDate(input2.data("DateTimePicker").date());
                }
            }
        });
    }
    this.SetupDateNonUptoDay = function (input2, day) {
        input2.datetimepicker({
            format: "DD/MM/YYYY",
            showTodayButton: true,
            //maxDate: base.DefaultDate().MomentMaxDate,
            defaultDate: moment().add(day, 'days'),
            showClose: true
        });
        input2.on("focusout", function (e) {
            if (!moment($(e.target).val()).isValid()) {
                try {
                    input2.data("DateTimePicker").defaultDate(base.DefaultDate.ToDate);
                } catch (e) {
                    input2.data("DateTimePicker").defaultDate(input2.data("DateTimePicker").date());
                }
            }
        });
    }
    this.SetupDateTimeCurrentMoth = function (input1) {
        var currentTime = new Date();
        // First Date Of the month
        var startDateFrom = new Date(currentTime.getFullYear(), currentTime.getMonth(), 1);
        // Last Date Of the Month
        var startDateTo = new Date(currentTime.getFullYear(), currentTime.getMonth() + 1, 0);
        input1.datetimepicker({
            format: Lang.Base_DateTime_Format,
            //changeMonth: false,
            //changeYear: false,
            showTodayButton: true,
            defaultDate: base.DefaultDate().MomentFromDate,
            showClose: true,
            //minDate: startDateFrom,xem lại chỗ này
            maxDate: startDateTo
        });
        //$('.datemask').inputmask({
        //    alias: '99/99/9999 99:99:99',
        //    placeholder: ""
        //});
    }
    this.SetupOnlyDate = function (input1) {
        input1.datetimepicker({
            format: "DD/MM/YYYY",
            showTodayButton: true,
            defaultDate: Sv.DefaultDate().MomentFromDate,
            showClose: true,
        });
    }
    this.SetupDateNon = function (input1) {
        input1.datetimepicker({
            format: "DD/MM/YYYY",
            showTodayButton: true,
            //maxDate: base.DefaultDate().MomentMaxDate,
            defaultDate: base.DefaultDate().MomentFromDate,
            showClose: true,
        });
        input1.on("focusout", function (e) {
            if (!moment($(e.target).val()).isValid()) {
                try {
                    input1.data("DateTimePicker").defaultDate(base.DefaultDate().FormDate);
                } catch (e) {
                    //input1.data("DateTimePicker").defaultDate(input2.data("DateTimePicker").date());
                }
            }
        });
    }
    this.SetupDate = function () {
        for (i = 0; i < arguments.length; i++) {
            var input = arguments[i];
            input.datetimepicker({
                format: 'DD/MM/YYYY',
                showTodayButton: true,
                maxDate: base.DefaultDate().MomentMaxDate,
                defaultDate: base.DefaultDate().MomentFromDate,
                showClose: true,
            });
        }

        //$('.datemask').inputmask({
        //    alias: '99/99/9999 99:99:99',
        //    placeholder: ""
        //});
        //$('.date1mask').inputmask({
        //    alias: '99/99/9999',
        //    placeholder: ""
        //});
    }
    this.SetupDateNotDefault = function () {
        for (i = 0; i < arguments.length; i++) {
            var input = arguments[i];
            input.datetimepicker({
                format: 'DD/MM/YYYY',
                showTodayButton: true,
                maxDate: base.DefaultDate().MomentMaxDate,
                //defaultDate: base.DefaultDate().MomentFromDate,
                showClose: true,
            });
        }
    }
    //number month limit
    this.SetupDate_Not_Time_Limit = function (input1, input2, number) {
        input1.datetimepicker({
            format: "DD/MM/YYYY",
            showTodayButton: true,
            defaultDate: base.DefaultDate().MomentFromDate,
            showClose: true
        });
        input2.datetimepicker({
            format: "DD/MM/YYYY",
            showTodayButton: true,
            defaultDate: base.DefaultDate().MomentToDate,
            minDate: base.DefaultDate().MomentFromDate,
            showClose: true
        });
        input2.data("DateTimePicker").minDate(base.DefaultDate().MomentFromDate);

        input2.data("DateTimePicker").maxDate(base.DefaultDate().MomentToDate.add(number, 'd'));
        input1.on("dp.change", function (e) {
            //var formDateTime = moment($(params).parent().data("DateTimePicker").date(), 'MM/DD/YYYY HH:mm');
            //var toDateTime = moment($(element).parent().data("DateTimePicker").date(), 'MM/DD/YYYY HH:mm');
            //var diff = parseInt(toDateTime) - parseInt(formDateTime);

            //return toDateTime.diff(formDateTime, 'minutes') <= 0;
            //var toDate = input2.data("DateTimePicker").maxDate();
            //if (toDate.diff(e.date, "months") < 0) {
            //    input2.data("DateTimePicker").maxDate(e.date.add(number, 'd'));
            //    input2.data("DateTimePicker").minDate(e.date);
            //} else {
            //    input2.data("DateTimePicker").minDate(e.date);
            //    input2.data("DateTimePicker").maxDate(e.date.add(number, 'd'));
            //}
            var toDate = moment(input2.data("DateTimePicker").date(), 'MM/DD/YYYY HH:mm:ss');
            var fromDate = moment(input1.data("DateTimePicker").date(), 'MM/DD/YYYY HH:mm:ss');
            var diff = parseInt(toDate) - parseInt(fromDate);
            if (toDate.diff(fromDate, 'seconds') < 0) {
                Dialog.Alert("Từ ngày không được lớn hơn đến ngày");
                input1.data("DateTimePicker").date(base.DefaultDate().MomentFromDate);
            }
            if (toDate.diff(fromDate, 'days') > number) {
                Dialog.Alert("Từ ngày, đến ngày không quá 30 ngày");
                input1.data("DateTimePicker").date(base.DefaultDate().MomentFromDate);
            }
        });
        input1.on("focusout", function (e) {
            if (!moment($(e.target).val()).isValid()) {
                try {
                    input1.data("DateTimePicker").defaultDate(base.DefaultDate().FormDate);
                } catch (e) {
                    input1.data("DateTimePicker").defaultDate(input2.data("DateTimePicker").date());
                }
            }
        });
        input2.on("focusout", function (e) {
            if (!moment($(e.target).val()).isValid()) {
                try {
                    input2.data("DateTimePicker").defaultDate(base.DefaultDate().ToDate);
                } catch (e) {
                    input2.data("DateTimePicker").defaultDate(input1.data("DateTimePicker").date());
                }
            }
        });
    }

    this.BindPopup = function (url, model, callback) {
        base.AjaxPost({
            Url: url,
            Data: model
        }, function (rs) {
            if (rs.Status === "00") {
                Dialog.Alert(rs.Message, Dialog.Error);
            } else {
                if (typeof callback == "function")
                    callback(rs);
            }
        }, function () {
            Dialog.Alert(base.Lang('System_Error'), Dialog.Error);
        });
    }

    this.ConfigAutocomplete = function (idControl, url, displayField, valueField, triggerLength, fnSelect, fnQuery, fnProcess, option) {
        var optionDefault = {
            onSelect: function (item) {
                $(idControl).data("seletectedValue", item.value);
                $(idControl).data("seletectedText", item.text);
                //$(idControl).valid();
                if (typeof (fnSelect) == "function")
                    fnSelect(item);
            },
            cache: false,
            ajax: {
                url: url,
                timeout: 500,
                displayField: displayField,
                valueField: valueField,
                cache: false,
                triggerLength: triggerLength,
                loadingClass: "ax",
                preDispatch: (fnQuery == undefined ? function (query) {
                    return {
                        search: query
                    }
                } : fnQuery),
                preProcess: (fnProcess == undefined ? function (data) {
                    if (data.success === false) {
                        return false;
                    }
                    return data;
                } : fnProcess)
            }
        }
        $.extend(optionDefault, option);
        $(idControl).typeahead(optionDefault);
    }
    //HoangLT-Format decemer
    this.AddCommas = function (nStr) {
        nStr += '';
        var x = nStr.split('.');
        var x1 = x[0];
        var x2 = x.length > 1 ? '.' + x[1] : '';
        var rgx = /(\d+)(\d{3})/;
        while (rgx.test(x1)) {
            x1 = x1.replace(rgx, '$1' + ',' + '$2');
        }
        return x1 + x2;
    }

    //==================================================================
    //	Description:  config 	Typeahead
    //	Author: LongLD dirUpload DirViewFile DefaultUrl
    //==================================================================
    this.ConfigTypeahead = function ($e, option) {
        var optionDefault = {
            onSelect: function (item) {
                $e.data("object", JSON.parse(item.object));
                $e.data("seletectedValue", item.value);
                $e.data("seletectedText", item.text);
                if (typeof (option.onSelect) == "function")
                    option.onSelect(item);
            },
            cache: false,
            fillValueOld: false,
            ajax: {
                url: option.url,
                timeout: option.timeout ? option.timeout : 500,
                displayField: option.displayField,
                valueField: option.valueField,
                cache: false,
                triggerLength: option.triggerLength ? option.triggerLength : 1,
                loadingClass: option.loadingClass ? option.loadingClass : "",
                preDispatch: function (query) {
                    if (option.preDispatch == undefined)
                        return {
                            search: query
                        }
                    else
                        return option.preDispatch(query);
                },
                preProcess: function (data) {
                    if (option.preProcess == undefined) {
                        if (data.success === false) {
                            return false;
                        }
                        return data;
                        //console.log('-' + data);
                    } else {
                        return option.preProcess(data);
                    }
                },
                loading: function (check) {
                    check ? base.RequestStart() : base.RequestEnd();
                },
            }
        }
        $.extend(optionDefault, option);
        $e.typeahead(optionDefault);
    }

    this.FormReset = function () {
        var optiondefault = {
            Type: "",
            Value: "",
            Element: $(".FormSearchInput"),
            Custom: undefined,
        }

        for (var i = 0; i < arguments.length; i++) {
            // lấy ra các option
            var option = arguments[i];
            var options = {};
            // check nếu option == input => gán vào element
            if (!option.Element)
                options.Element = option;
            else
                options = option;

            // Extend với thằng default (bổ sung những giá trị mà người dùng ko điền (những giá trị default))
            options = $.extend({}, optiondefault, options);
            if (options.Custom && typeof options.Custom === "function") {
                options.Custom(options.Element);
                continue;
            }
            // check tồn tại element
            if (options.Element.length == 0) continue;
            // check type(date, datetime, datetimefull,number,text, hoặc để tự nó làm (custom))
            switch (options.Type) {
                case "Date":
                    if (options.Value)
                        options.Element.data("DateTimePicker").date(options.Value);
                    else
                        options.Element.data("DateTimePicker").date(new Date(moment().format('YYYY-MM-DD HH:MM:SS')));
                    break;
                case "Typeahead":
                    options.Element.data("seletectedValue", "");
                    options.Element.data("seletectedText", "");
                    options.Element.data("seletectedObject", {});
                    options.Element.val("");
                    break;
                case "Number":
                    options.Element.val(0);
                    break;
                default:
                    if (options.Value)
                        options.Element.val(options.Value);
                    else
                        options.Element.val("");
                    break;
            }
        }
    };
    this.BindMoneyToString = function ($e, val) {
        var str = "" + val;
        str = str.replace(/\./g, '');
        str = str.replace(/\,/g, '.');
        var money = parseFloat(str);
        if (money > 0) {
            $e.html(base.MoneyToString(money));
        } else {
            $e.html("");
        }
    }
    this.MoneyToString = function (money, perfix) {
        var lang = abp.localization.currentLanguage.name;
        //if (lang == "vi") {
        var ChuSo = new Array(" không ", " một ", " hai ", " ba ", " bốn ", " năm ", " sáu ", " bảy ", " tám ", " chín ");
        var Tien = new Array("", " nghìn", " triệu", " tỷ", " nghìn tỷ", " triệu tỷ");
        var base = this;
        //1. Hàm đọc số có ba chữ số;
        this.DocSo3ChuSo = function (baso) {
            var tram;
            var chuc;
            var donvi;
            var KetQua = "";
            tram = parseInt(baso / 100);
            chuc = parseInt((baso % 100) / 10);
            donvi = baso % 10;
            if (tram == 0 && chuc == 0 && donvi == 0) return "";
            if (tram != 0) {
                KetQua += ChuSo[tram] + " trăm ";
                if ((chuc == 0) && (donvi != 0)) KetQua += " linh ";
            }
            if ((chuc != 0) && (chuc != 1)) {
                KetQua += ChuSo[chuc] + " mươi";
                if ((chuc == 0) && (donvi != 0)) KetQua = KetQua + " linh ";
            }
            if (chuc == 1) KetQua += " mười ";
            switch (donvi) {
                case 1:
                    if ((chuc != 0) && (chuc != 1)) {
                        KetQua += " mốt ";
                    } else {
                        KetQua += ChuSo[donvi];
                    }
                    break;
                case 5:
                    if (chuc == 0) {
                        KetQua += ChuSo[donvi];
                    } else {
                        KetQua += " lăm ";
                    }
                    break;
                default:
                    if (donvi != 0) {
                        KetQua += ChuSo[donvi];
                    }
                    break;
            }
            return KetQua;
        }

        //2. Hàm đọc số thành chữ (Sử dụng hàm đọc số có ba chữ số)
        this.DocSo = function (SoTien, perfix) {
            var lan = 0;
            var i = 0;
            var so = 0;
            var KetQua = "";
            var tmp = "";
            var ViTri = new Array();
            if (SoTien < 0) return "";
            if (SoTien == 0) return "Không " + perfix;
            if (SoTien > 0) {
                so = SoTien;
            } else {
                so = -SoTien;
            }
            if (SoTien > 8999999999999999) {
                //SoTien = 0;
                return "Số quá lớn!";
            }
            ViTri[5] = Math.floor(so / 1000000000000000);
            if (isNaN(ViTri[5]))
                ViTri[5] = "0";
            so = so - parseFloat(ViTri[5].toString()) * 1000000000000000;
            ViTri[4] = Math.floor(so / 1000000000000);
            if (isNaN(ViTri[4]))
                ViTri[4] = "0";
            so = so - parseFloat(ViTri[4].toString()) * 1000000000000;
            ViTri[3] = Math.floor(so / 1000000000);
            if (isNaN(ViTri[3]))
                ViTri[3] = "0";
            so = so - parseFloat(ViTri[3].toString()) * 1000000000;
            ViTri[2] = parseInt(so / 1000000);
            if (isNaN(ViTri[2]))
                ViTri[2] = "0";
            ViTri[1] = parseInt((so % 1000000) / 1000);
            if (isNaN(ViTri[1]))
                ViTri[1] = "0";
            ViTri[0] = parseInt(so % 1000);
            if (isNaN(ViTri[0]))
                ViTri[0] = "0";
            if (ViTri[5] > 0) {
                lan = 5;
            } else if (ViTri[4] > 0) {
                lan = 4;
            } else if (ViTri[3] > 0) {
                lan = 3;
            } else if (ViTri[2] > 0) {
                lan = 2;
            } else if (ViTri[1] > 0) {
                lan = 1;
            } else {
                lan = 0;
            }
            for (i = lan; i >= 0; i--) {
                tmp = base.DocSo3ChuSo(ViTri[i]);
                KetQua += tmp;
                if (ViTri[i] > 0) KetQua += Tien[i];
                if ((i > 0) && (tmp.length > 0)) KetQua += ','; //&& (!string.IsNullOrEmpty(tmp))
            }
            if (KetQua.substring(KetQua.length - 1) == ',') {
                KetQua = KetQua.substring(0, KetQua.length - 1);
            }
            KetQua = KetQua.substring(1, 2).toUpperCase() + KetQua.substring(2);


            return KetQua + " " + perfix; //.substring(0, 1);//.toUpperCase();// + KetQua.substring(1);
        }

        this.DocThapPhan = function (x) {

            var thapphan = base.ThapPhan(x);
            if (thapphan > 0) {
                var so = thapphan * 100;

                var str = " phẩy ";
                str += base.DocSo(so, '').toLowerCase();
                return str;
            } else {
                return "";
            }
        }

        this.ThapPhan = function (x) {
            return base.Financial(x - parseInt(x));
        }


        this.Financial = function (x) {
            return Number.parseFloat(x).toFixed(2);
        }
        return base.DocSo(money, perfix != undefined ? perfix : "") + base.DocThapPhan(money) + ' đồng';
        //}

    }
    this.LoadTableSearch = function ($table, url, showDialog) {
        $table.bootstrapTable('refreshOptions', {
            url: url,
            responseHandler: function (res) {
                if (res.Status) {
                    if (res.Status == "URL") {
                        window.location.assign(res.Message);
                        return false;
                    }
                    if (res.Status == "Login") {
                        window.location = "/Admin/login?ReturnUrl=" + window.location.href;
                        return false;
                    }
                    return base.ResponseHandler($table, showDialog, res.Data);
                } else {
                    return base.ResponseHandler($table, showDialog, res);
                }
            },
            sidePagination: 'server',
        });
    }

    this.GetUrlFileUpload = function () {
        var appSetting = '@(System.Configuration.ConfigurationManager.AppSettings["DirViewFile"].ToString())';
        return appSetting;
    }
    //--ResetForm
    this.ResetForm = function ($form, $fdate, $todate) {
        var validator = $form.validate();
        validator.resetForm();
        $form.each(function () {
            this.reset();
        });
        $form.find($fdate).data("DateTimePicker").date(Sv.DefaultDate().MomentFromDate);
        $form.find($todate).data("DateTimePicker").date(Sv.DefaultDate().MomentToDate);
    }
    this.ResetFormOnly = function ($form) {
        var validator = $form.validate();
        validator.resetForm();
        $form.each(function () {
            this.reset();
        });
    }
    //KhangPV
    this.SetUpdateInputMask = function (maxNumber) {
        $('.discounted-mask').inputmask("percentage", {
            placeholder: '',
            radixPoint: ".",
            autoUnmask: true,
            allowMinus: false, /* Không cho nhập dấu trừ */
            allowPlus: false /* Không cho nhập dấu cộng */
        });
        $('.price-mask').inputmask({
            alias: 'decimal',
            groupSeparator: ',', /* Ký tự phân cách giữa phần trăm, nghìn... */
            radixPoint: ".", /* Ký tự phân cách với phần thập phân */
            autoGroup: true,
            digits: 0, /* Lấy bao nhiêu số phần thập phân, mặc định của inputmask là 2 */
            autoUnmask: true, /* Tự động bỏ mask khi lấy giá trị */
            allowMinus: false, /* Không cho nhập dấu trừ */
            allowPlus: false, /* Không cho nhập dấu cộng */
            integerDigits: maxNumber,
            showMaskOnHover: false,
            showMaskOnFocus: false
        });
    };
    //HoangLT customize pagging
    this.PaggingService = function (option, fnSuccess, fnError) {
        $(".pagination li a").click(function (e) {
            var link = $(this).attr('data-value');
            var page = parseInt($(this).html());
            var className = $(this).parent().attr("class").trim();
            if (className == 'page-pre' || className == 'page-next' || className == 'page-first' || className == 'page-last') {
                page = parseInt($(this).attr('data-id'));
            }
            if (option.Data) {
                option.Data.__RequestVerificationToken = $("input[name=__RequestVerificationToken]").val();
            }
            var me = $(this);
            if (me.data('requestRunning')) {
                return;
            }
            me.data('requestRunning', true);

            $.ajax({
                url: link,
                type: 'Post',
                data: { search: option.Data, page: page },
                beforeSend: function () {
                    base.RequestStart();
                },
                async: (option.async == undefined ? true : option.async),
                complete: function () {
                    me.data('requestRunning', false);
                    base.RequestEnd();
                },
                success: function (rs) {
                    if (typeof fnSuccess === "function")
                        fnSuccess(rs);
                },
                error: function (e) {
                    if (typeof fnError === "function")
                        fnError(e);
                }
            });
        });
    }
    this.ChangePageNumber = function (option, fnSuccess, fnError) {
        $('#dropPagging li a').on('click', function () {
            var numpage = parseInt($(this).html());
            if (option.Data) {
                option.Data.__RequestVerificationToken = $("input[name=__RequestVerificationToken]").val();
                option.Data.Numperpage = numpage;
            }
            $(option.HidentNumperPage).val(numpage);
            $("#dropPagging li").find(".active").removeClass("active");
            $(this).parent().addClass("active");
            var me = $(this);
            if (me.data('requestRunning')) {
                return;
            }
            me.data('requestRunning', true);

            $.ajax({
                url: option.Url,
                type: 'Post',
                data: { search: option.Data, page: option.Page },
                beforeSend: function () {
                    base.RequestStart();
                },
                async: (option.async == undefined ? true : option.async),
                complete: function () {
                    me.data('requestRunning', false);
                    base.RequestEnd();
                },
                success: function (rs) {
                    if (typeof fnSuccess === "function")
                        fnSuccess(rs);
                },
                error: function (e) {
                    if (typeof fnError === "function")
                        fnError(e);
                }
            });
        });
    }

    this.totalTextFormatter = function (data) {
        return 'Tổng';
    }
    this.sumFormatter = function (data) {
        var field = this.field;
        var totalSum = data.reduce(function (sum, row) {
            return (sum) + (row[field] || 0);
        }, 0);
        return totalSum ? totalSum.toString().replace('.', ',').replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1.') : 0;
    }
    this.Serialize = function (obj) {
        var str = [];
        for (var p in obj)
            if (obj.hasOwnProperty(p)) {
                str.push(encodeURIComponent(p) + "=" + encodeURIComponent(obj[p]));
            }
        return str.join("&");
    }
    this.OpenInNewTab = function (url) {
        var win = window.open(url, '_blank');
        win.focus();
    }
    //------------------HoangLT. Những service dùng chung
    //---Hàm get tỉnh, huyện, xã
    var _commonLockupServer = abp.services.app.commonLookup;
    var _categoryService = abp.services.app.categoryses;
    var _accountService = abp.services.app.account;
    this.GetDistrictByCity = function (cityId, controlSelect, optionAll, textAll) {
        var textSelectAll = Sv.Lang("Profile_District_Optiondefault");
        if (textAll != undefined) {
            textSelectAll = textAll;
        }
        abp.ui.setBusy();
        _commonLockupServer.getDistrictsForCombobox(cityId).done(function (response) {
            //console.log(response);
            $(controlSelect).empty();
            if (optionAll)
                $(controlSelect).append('<option value="-1">' + textSelectAll + '</option>');
            if (!$.isEmptyObject(response.items))
                $.each(response.items, function (key, value) {
                    $(controlSelect).append('<option value="' + value.value + '">' + value.displayText + '</option>');
                });
        }).always(function () {
            abp.ui.clearBusy();
        });
    }

    this.GetWardByDistrict = function (districtId, controlSelect, optionAll, textAll) {
        var textSelectAll = Sv.Lang("WardName");
        if (textAll != undefined) {
            textSelectAll = textAll;
        }
        abp.ui.setBusy();
        _commonLockupServer.getWardsForCombobox(districtId).done(function (response) {
            $(controlSelect).empty();
            if (optionAll)
                $(controlSelect).append('<option value="-1">' + textSelectAll + '</option>');
            if (!$.isEmptyObject(response.items))
                $.each(response.items, function (key, value) {
                    $(controlSelect).append('<option value="' + value.value + '">' + value.displayText + '</option>');
                });
        }).always(function () {
            abp.ui.clearBusy();
        });
    }
    this.GetProductByCate = function (catecode, controlSelect, isActive) {
        abp.ui.setBusy();
        abp.services.app.commonLookup.getProductByCategory(catecode, isActive)
            .done(function (result) {
                let html = "<option value=\"\">Chọn sản phẩm</option>";
                if (result != null && result.length > 0) {
                    for (let i = 0; i < result.length; i++) {
                        let item = result[i];
                        html += ("<option value=\"" + item.productCode + "\">" + item.productName + "</option>");
                    }
                }
                controlSelect.html(html);
            })
            .always(function () {
                abp.ui.clearBusy();
            });
    }

    this.GetProductByCateMuti = function (catecode, controlSelect, isActive) {       
        abp.ui.setBusy();
        abp.services.app.commonLookup.getProductByCategoryMuti(catecode, isActive)
            .done(function (result) {
                let html = "<option value=\"\">Chọn sản phẩm</option>";
                if (result != null && result.length > 0) {
                    for (let i = 0; i < result.length; i++) {
                        let item = result[i];
                        html += ("<option value=\"" + item.productCode + "\">" + item.productName + "</option>");
                    }
                }
                controlSelect.html(html);
            })
            .always(function () {
                abp.ui.clearBusy();
            });
    }
    this.GetProductTwoByCate = function (cateId, controlSelect, isActive) {
        abp.ui.setBusy();
        abp.services.app.commonLookup.getProductTwoByCategory(cateId, isActive)
            .done(function (result) {
                let html = "<option value=\"\">Chọn sản phẩm</option>";
                if (result != null && result.length > 0) {
                    for (let i = 0; i < result.length; i++) {
                        let item = result[i];
                        html += ("<option value=\"" + item.id + "\">" + item.productName + "</option>");
                    }
                }
                controlSelect.html(html);
            })
            .always(function () {
                abp.ui.clearBusy();
            });
    }
    this.GetCateByService = function (serviceCode, controlSelect, isActive) {
        abp.ui.setBusy();
        abp.services.app.commonLookup.getCategories({ serviceCode: serviceCode, isActive: isActive })
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
                abp.ui.clearBusy();
            });
    }

    this.GetCateByServiceMuti = function (serviceCode, controlSelect, isActive) {
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
                abp.ui.clearBusy();
            });
    }

    this.GetCateTwoByService = function (serviceId, controlSelect, isActive) {
        abp.ui.setBusy();
        abp.services.app.commonLookup.getCategoriesTwoBy({ serviceIds: serviceId, isActive: isActive })
            .done(function (result) {
                let html = "<option value=\"\">Chọn loại sản phẩm</option>";
                if (result != null && result.length > 0) {
                    for (let i = 0; i < result.length; i++) {
                        let item = result[i];
                        html += ("<option value=\"" + item.id + "\">" + item.categoryName + "</option>");
                    }
                }
                controlSelect.html(html);
            })
            .always(function () {
                abp.ui.clearBusy();
            });
    }

    this.ShowConfirmOtp = function (requestType) {
        //var _createOrEditModal = new app.ModalManager({
        //    viewUrl: abp.appPath + 'Account/ShowOtpModal',
        //    scriptUrl: abp.appPath + 'view-resources/Views/Account/_ConfirmOtp.js',
        //    modalClass: 'ConfirmOtpModal'
        //});
        //_createOrEditModal.open({ requestType: requestType });
        abp.ui.setBusy();
        $.ajax({
            url: abp.appPath + 'Account/ShowOtpModal',
            type: 'GET',
            contentType: 'application/html',
            data: { requestType: requestType },
            success: function (content) {
                $('#ConfirmOtpModal div.modal-content').html(content);
                $("#ConfirmOtpModal").modal('show');
                abp.ui.clearBusy();
                base.StartCountDownTime($("#timeOtp"), 60);
            },
            error: function (e) {
                abp.ui.clearBusy();
            }
        });
    };
    this.format_number = function (n) {
        return (n + "").replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,")
    };
    this.ShowConfirmOtpNotAuthen = function (requestType) {
        abp.ui.setBusy();
        $.ajax({
            url: abp.appPath + 'Account/ShowOtpModalNotAuthen',
            type: 'GET',
            contentType: 'application/html',
            data: { requestType: requestType },
            success: function (content) {
                $('#ConfirmOtpModal div.modal-content').html(content);
                $("#ConfirmOtpModal").modal('show');
                abp.ui.clearBusy();
                base.StartCountDownTime($("#timeOtp"), base.OtpTimeOut);
            },
            error: function (e) {
                abp.ui.clearBusy();
            }
        });
    }
    var s = null; // Giây
    var timeout = null; // Timeout

    this.StartCountDownTime = function ($time, $input) {
        /*BƯỚC 1: LẤY GIÁ TRỊ BAN ĐẦU*/
        if ($input === base.OtpTimeOut) {
            clearTimeout(timeout);
            s = base.OtpTimeOut;
        }
        // Nếu số giờ = -1 tức là đã hết giờ, lúc này:
        //  - Dừng chương trình
        if (s === 0) {
            clearTimeout(timeout);
            $time.text("0");
            location.reload();
            return false;
        }
        /*BƯỚC 1: GIẢM PHÚT XUỐNG 1 GIÂY VÀ GỌI LẠI SAU 1 GIÂY */
        timeout = setTimeout(function () {
            s--;
            base.StartCountDownTime($time);
            $time.text("" + s + "");
        }, 1000);
        //if (typeof callback == "function")
        //    callback($time);
        return true;
    }
    this.ShowOtpResend = function ($time, $input) {
        /*BƯỚC 1: LẤY GIÁ TRỊ BAN ĐẦU*/
        if ($input === base.OtpTimeOut) {
            clearTimeout(timeout);
            s = base.OtpTimeOut;
        }
        // Nếu số giờ = -1 tức là đã hết giờ, lúc này:
        //  - Dừng chương trình
        if (s === 0) {
            clearTimeout(timeout);
            $time.text("0");
            $("#show-resend-otp").removeClass("hidden").addClass("show");
            $("#show-text-otp").removeClass("show").addClass("hidden");
            //location.reload();
            return false;
        }
        /*BƯỚC 1: GIẢM PHÚT XUỐNG 1 GIÂY VÀ GỌI LẠI SAU 1 GIÂY */
        timeout = setTimeout(function () {
            s--;
            base.ShowOtpResend($time);
            $time.text("" + s + "");
        }, 1000);
        //if (typeof callback == "function")
        //    callback($time);
        return true;
    }
    this.OtpHide = function (errorCode, isReLoad) {
        if (errorCode != "3000" && errorCode != "3001") {
            $("#ConfirmOtpModal").modal("hide");
            if (isReLoad != undefined && isReLoad == true) {
                setTimeout(function () {
                    location.reload();
                }, 3000);
            }
        }
    }
    this.OtpGetPara = function () {
        var obj = {
            OtpValues: $("#ConfirmOtpModal").on().find("#otp").val(),
            RequestType: $("#ConfirmOtpModal").on().find("#txtOtpRequestType").val()
        };
        return obj;
    }

    /**
     * @return {string}
     */
    this.GetURLParameter = function (sParam) {
        var sPageURL = window.location.search.substring(1);
        var sURLVariables = sPageURL.split('&');
        for (let i = 0; i < sURLVariables.length; i++) {
            const sParameterName = sURLVariables[i].split('=');
            if (sParameterName[0] === sParam) {
                return sParameterName[1];
            }
        }
    };
    //Get topup price
    this.GetCategoryByParent = function (cateCode, type, controlSelect, optionAll, textAll) {
        var textSelectAll = 'Vui lòng chọn';
        if (textAll != undefined) {
            textSelectAll = textAll;
        }
        abp.ui.setBusy();
        _categoryService.getCategorys(null, cateCode, type).done(function (response) {
            $(controlSelect).empty();
            if (optionAll)
                $(controlSelect).append('<option value="">' + textSelectAll + '</option>');
            if (!$.isEmptyObject(response))
                $.each(response, function (key, value) {
                    $(controlSelect).append('<option value="' + value.categoryCode + '">' + value.categoryName + '</option>');
                });
        }).always(function () {
            abp.ui.clearBusy();
        });
    }
    //Lấy thông tin tài khoản
    this.GetAccountByUserName = function (username, $showFullname, $setValue) {
        abp.ui.setBusy();
        _commonLockupServer.getUserInfoQuery({ userName: username }).done(function (rs) {
            if (rs !== null && rs !== undefined) {
                $showFullname.html(rs.fullName);
                $setValue.val(rs.accountCode);
            } else {
                abp.message.error('Không tìm thấy thông tin tài khoản nhận tiền');
            }
        }).always(function () {
            abp.ui.clearBusy();
        });
    }

    //Lấy thông tin tài khoản any field
    this.GetAccountAnyField = function (search, $showFullname, $setValue) {
        abp.ui.setBusy();
        _commonLockupServer.getUserInfoQuery({ search: search }).done(function (rs) {
            if (rs !== null && rs !== undefined) {
                $showFullname.html('Họ tên: <span style="color: #2188C9; font-weight: 500">' + rs.fullName + '</span> - Số ĐT: <span style="color: #2188C9; font-weight: 500">' + rs.phoneNumber + '</span> - Mã tài khoản: <span style="color: #2188C9; font-weight: 500">' + rs.accountCode + '</span>');
                $setValue.val(rs.accountCode);
            } else {
                $showFullname.html('');
                abp.notify.error('Không tìm thấy thông tin tài khoản nhận tiền');
            }
        }).always(function () {
            abp.ui.clearBusy();
        });
    }

    // Go back history
    this.goBackHistory = function () {
        //window.history.back();
        location.replace(document.referrer);//Load lại trang luôn để check 1 số điều kiện
    }

    // Format phone number
    this.removeSpaces = function () {
        let str = '';
        let new_str = '';

        $(document).on('blur', '.remove-spaces-phone-number', function () {
            str = $(this).val();
            new_str = str.split(' ').join('');
            $(this).val(new_str);
        });
    }

    this.checkBalance = function (obj) {
        // if(!obj.isActive)
        // {
        //     if (obj.accountType === "Staff") {
        //         abp.message.info("Tài khoản của bạn đã bị khóa. \nVui lòng liên hệ đại lý để biết thêm thông tin!");
        //     } else {
        //         abp.message.info("Tài khoản của bạn đã bị khóa. \nVui lòng liên hệ quản trị viên để biết thêm thông tin!");
        //     }
        //     window.setTimeout( function(){
        //         window.location.href = "/account/logout";
        //     }, 500 );
        //     return false;
        // }
        if (!base.clientIsAgencyVerify()) {
            return false;
        }
        console.log(obj);
        if (obj.balance == null || obj.amount == null || obj.balance < obj.amount) {
            if (obj.accountType === "Staff") {
                Dialog.deposit("Số dư không đủ để thực hiện giao dịch. \nVui lòng liên hệ đại lý để biết thêm thông tin! ", false);
            } else {
                Dialog.deposit("Số dư không đủ để thực hiện giao dịch.\nVui lòng nạp tiền để thực hiện giao dịch! ", true);
            }
            return false;
        }
        if (obj.accountType === "Staff" && (obj.limitBalance == null || obj.amount == null || obj.limitBalance < obj.amount)) {
            Dialog.deposit("Hạn mức không đủ để thực hiện giao dịch.\n Vui lòng liên hệ đại lý để biết thêm thông tin! ", false);
            return false;
        }
        return true;
    }

    this.checkTransBalance = function (amount, callback) {
        if (!base.clientIsAgencyVerify()) {
            return false;
        }
        Sv.Post({
            Url: abp.appPath + 'Common/GetTransInfo',
            Data: { amount: amount }
        }, function (rs) {
            var ckBalance = Sv.checkBalance(rs.result);
            if (ckBalance) {
                callback();
            }
        });
    }

    this.pageCheckAgencyVerify = function () {
        var flagLogin = getLocalStorage('flagUserLogin');
        if (app.session.user != null && flagLogin != null && flagLogin.length > 0) {
            // clear
            clearLocalStorage(['flagUserLogin']);
            // check
            base.serverIsAgencyVerify();
        }
    }

    this.serverIsAgencyVerify = function (show = true) {
        return abp.ajax({
            url: abp.appPath + 'Common/GetAgentVerify',
        }).done(function (isVerify) {
            app.session.user.isVerifyAccount = isVerify + "";
            if (show) {
                return base.clientIsAgencyVerify();
            } else {
                return isVerify
            }
        });
    }

    this.clientIsAgencyVerify = function (show = true) {
        const accountType = app.session.user.accountType;
        if (accountType === "Agent" || accountType === "MasterAgent") {
            let isVerify = app.session.user.isVerifyAccount.toLowerCase() === "true";
            let isOk = (isVerify === true);
            if (show && !isOk) {
                if (app.session.user.accountType === "Staff") {
                    Dialog.verify("Tài khoản đại lý chưa xác thực. \nVui lòng liên hệ đại lý để biết thêm thông tin! ", false);
                } else {
                    Dialog.verify("Tài khoản của bạn chưa xác thực. \nVui lòng xác thực tài khoản để sử dụng dịch vụ! ", true);
                }
            }
            return isOk;
        }
        return true;
    }
    this.keyEnter = function ($f, callback) {
        $f.find("input, select, textarea").on('keydown', function (e) {
            if (e.keyCode === 13) {
                callback();
                return false;
            }
        });
    }
    this.checkUserTransValid = function (serviceCode, categoryCode, productCode, paymentAmount, amount, quantity) {
        console.log(serviceCode, categoryCode, productCode, paymentAmount, amount, quantity);
        let type_check = 'CheckActiveAccount|CheckVerifyAccount|CheckTimeStaff|CheckPaymentMethod';
        let data = {};
        if (serviceCode != null && serviceCode.length > 0) {
            type_check += "|CheckServiceEnable";
            data.serviceCode = serviceCode;
        }
        if (categoryCode != null && categoryCode.length > 0) {
            type_check += "|CheckCategory";
            data.categoryCode = categoryCode;
        }
        if (productCode != null && productCode.length > 0) {
            type_check += "|CheckLimitProduct";
            data.productCode = productCode;
            data.amount = amount;
            data.quantity = quantity;
        }
        if (paymentAmount != null && paymentAmount > 0) {
            type_check += "|CheckBalance";
            data.paymentAmount = paymentAmount;
        } else if (paymentAmount != null && paymentAmount.toString().length > 0) {
            type_check += "|CheckBalance";
            data.paymentAmount = parseFloat(paymentAmount.toString());
        }
        data.checkTypes = type_check;
        base.RequestStart();
        return abp.services.app.common.checkAccountActivities(data).always(function (e) {
            //console.log(e);
            if (e.code === 113) {
                setTimeout(function () { window.location.href = '/Profile/SecurityMethod'; }, 3000);
            }
            base.RequestEnd();
        });

    }

    this.onlyNumberInput = function () {
        $('.only-number-input').keyup(function () {
            let reg = /^\d+$/;

            if (!reg.test($(this).val())) {
                $(this).val($(this).val().slice(0, -1));
            }
        });
    }

};
var Sv = new Service();

$(document).ready(function () {

    function SetupDatetime() {
        try {
            $('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            $('.datetime-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L LT'
            });
        } catch (e) {

        }
    }

    SetupDatetime();
    Sv.SetupAmountMask();

    $('.select2').select2({ "width": "100%" });

    $("#formDetail select,#formDetail input").keypress(function (e) {
        if (e.which == 13) {
            $("#formDetail button.enter:not(.hidden)").trigger("click");
            return false;
        }
    });

    $("#formOtp select,#formOtp input").keypress(function (e) {
        if (e.which == 13) {
            $("#formOtp button.confirm:not(.hidden)").trigger("click");
            return false;
        }
    });

    Sv.pageCheckAgencyVerify();
    //
    // $("body").on('hidden.bs.modal', function () {
    //     if($("#kt_wrapper, .page .content .card-body").length > 0){
    //         Sv.SetupAmountMask();
    //         $('.select2').select2();
    //     }
    // });

});
//String.Format
String.prototype.format = function () {
    var a = this;
    for (k in arguments) {
        a = a.replace("{" + k + "}", arguments[k]);
    }
    return a;
}


var Clipboard = (function (window, document, navigator) {
    var textArea, copy;

    function isOS() {
        return navigator.userAgent.match(/ipad|iphone/i);
    }

    function createTextArea(text) {
        textArea = document.createElement('textArea');
        textArea.value = text;
        document.body.appendChild(textArea);
    }

    function selectText() {
        var range,
            selection;

        if (isOS()) {
            range = document.createRange();
            range.selectNodeContents(textArea);
            selection = window.getSelection();
            selection.removeAllRanges();
            selection.addRange(range);
            textArea.setSelectionRange(0, 999999);
        } else {
            textArea.select();
        }
    }

    function copyToClipboard() {
        document.execCommand('copy');
        document.body.removeChild(textArea);
    }

    copy = function (text) {
        createTextArea(text);
        selectText();
        copyToClipboard();
    };

    return {
        copy: copy
    };
})(window, document, navigator);
// How to use
//Clipboard.copy('text to be copied');

var Dialog = {
    Success: 'Thành công',
    Warning: 'Cảnh báo',
    Error: 'Lỗi',
    CONFIRM: 'Xác nhận',
    Alert: function (message, status, dialogtitle, callbackFuntion, hideModalFuntion) {
        var typeDialog = this._getTypeDialog(status);
        window.bootbox.dialog({
            message: message,
            title: dialogtitle != 'undefine' ? dialogtitle : typeDialog.title,
            closeButton: false,
            className: typeDialog.className,
            buttons: {
                success: {
                    label: "<i class='fa fa-check'></i>" + abp.localization.localize("Label_Close", "CRM"),
                    className: typeDialog.buttonClass,
                    callback: callbackFuntion
                }
            }
        }).on('shown.bs.modal', function () {
            $('.bootbox').find('button:first').focus();
        }).on('hidden.bs.modal', function () {
            var p = $("body").css('padding-right');
            var p1 = parseInt(p) - 17;
            if (p1 >= 0)
                $("body").css('padding-right', p1);
            hideModalFuntion == undefined ? function () {
            } : hideModalFuntion();
        });
    },
    ConfirmCustom: function (title, message, callbackFuntion, showModalFuntion) {
        var typeDialog = this._getTypeDialog(this.CONFIRM);
        window.bootbox.dialog({
            message: message,
            title: title ? title : typeDialog.title,// title ? typeDialog.title : title,
            closeButton: false,
            className: typeDialog.className,
            buttons: {
                success: {
                    label: "<i class='fa fa-check'></i>" + abp.localization.localize("Dialog_Confirm", "CRM"),
                    className: typeDialog.buttonClass,
                    callback: callbackFuntion
                },
                cancel: {
                    label: "<i class='fa fa-reply'></i>" + abp.localization.localize("Label_Close", "CRM"),
                    className: "btn btn-df"
                }
            }
        }).on('shown.bs.modal', showModalFuntion == undefined ? function () {
            //$('.bootbox').find('button:first').focus();
        } : showModalFuntion);
    },
    AlertHide: function (message) {
        var dialog = window.bootbox.dialog({
            message: message,
            closeButton: false
        });
        dialog.find('.modal-content').css({ 'text-align': 'center' });
        setTimeout(
            function () {
                dialog.modal('hide');
            }, 1000);
    },
    _getTypeDialog: function (status) {
        var type = {};
        switch (status) {
            case 'success':
                type = {
                    title: base.Lang('Dialog_Success'),
                    className: 'my-modal-success',
                    buttonClass: 'btn-sm btn-lue'
                };
                break;
            case 'warning':
                type = {
                    title: base.Lang('Dialog_Warning'),
                    className: 'my-modal-warning',
                    buttonClass: 'btn-sm btn-blue'
                };
                break;
            case 'error':
                type = {
                    title: base.Lang('Dialog_Error'),
                    className: 'my-modal-error',
                    buttonClass: 'btn-sm btn-blue'
                };
                break;
            case 'primary':
                type = {
                    title: base.Lang('Dialog_Confirm'),
                    className: 'my-modal-primary',
                    buttonClass: 'btn-sm btn-blue'
                };

                break;
        }
        return type;
    },

    flag: false,

    _modalCustome: function (type, callback) {
        if (Dialog.flag === true) {
            return;
        }
        if (type === 'Pw2_Create') {
            Dialog._modalCreatePw2();
            return;
        }
        var title = "";
        var message = "";
        var className = "dialog-ngt ";
        if (type === 'Pw2') {
            title = "Mật khẩu cấp 2";
            message = '<form class="bootbox-form" autocomplete="off">' +
                '<div class="bootbox-prompt-message">' + app.localize("Message_Level2Pass_Description") + '</div>' +
                '<input class="bootbox-input bootbox-input-password dialog-fn-input form-control" autocomplete="off" type="password">' +
                '<div class="b-action"><a class="saction" href="/Profile/Password2Level">Quên mật khẩu cấp 2?</a></div>' +
                '</form>';
            className += "dialog-pw2";
        } else if (type === 'OTP') {
            let isOdp = false;
            const obj = JSON.parse(getLocalStorage("process_modal"));
            if (obj.type === Dialog.otpType.Payment || obj.type === Dialog.otpType.Transfer) {
                //check cho giao dịch thanh toán
                const verifyType = abp.setting.getInt("Abp.PaymentMethod.Web");
                if (verifyType === Dialog.verifyTransType.ODP) {
                    isOdp = true;
                }
            } else if (obj.type === Dialog.otpType.Register) {
                isOdp = abp.setting.getBoolean("App.UserManagement.OtpSetting.IsUseOdpRegister");
            } else if (obj.type === Dialog.otpType.ResetPass) {
                isOdp = abp.setting.getBoolean("App.UserManagement.OtpSetting.IsUseOdpResetPass");
            } else if (obj.type === Dialog.otpType.Login) {
                isOdp = abp.setting.getBoolean("App.UserManagement.OtpSetting.IsUseOdpLogin");
            } else {
                //Check cho các chức năng còn lại
                isOdp = abp.setting.getBoolean("App.UserManagement.OtpSetting.IsOdpVerificationEnabled");
            }
            title = "Nhập " + (isOdp === true ? "ODP" : "OTP");
            if (isOdp === true) {
                const time = abp.setting.getInt("App.UserManagement.OtpSetting.OdpAvailable") / 60;
                const description = app.localize("Message_ODP_Description", time);
                message = '<form class="bootbox-form text-center" autocomplete="off">' +
                    '<div class="bootbox-prompt-message">' + description + '</div>' +
                    '<input class="bootbox-input form-control dialog-fn-input" autocomplete="off" required="required" type="password" style="text-center">' +
                    '<div class="bootbox-prompt-message" style="text-align:center;padding-top: 10px;color:#3699FF;" id="show-text-otp"><i style="color:#3699FF;" class="fas fa-share-square"></i>&nbspLấy lại ODP sau <span style="color: red" id="timeOtp"></span>s</div>' +
                    '<div id="show-resend-otp" class="hidden" style="text-align:center;padding-top: 10px;color:#3699FF;"><a class="saction" href="#" onclick="Dialog._otp.resend()"><i style="color:#3699FF;" class="fas fa-share"></i>&nbspGửi lại ODP?</a></div>' +
                    '</form>';
            } else {
                //const time = abp.setting.getInt("App.UserManagement.OtpSetting.OtpTimeOut");
                const description = app.localize("Message_OTP_Description", Sv.OtpTimeOut);
                message = '<form class="bootbox-form" autocomplete="off">' +
                    '<div class="bootbox-prompt-message">' + description + '</div>' +
                    // '<div class="bootbox-prompt-message">Nếu không nhận được OTP vui lòng thử lại sau: <span style="color: red" id="timeOtp"></span> giây</div>' +
                    '<input class="bootbox-input form-control dialog-fn-input" autocomplete="off" required="required" type="password" style="text-center">' +
                    '<div class="bootbox-prompt-message" style="text-align:center;padding-top: 10px;color:#3699FF;" id="show-text-otp"><i style="color:#3699FF;" class="fas fa-share-square"></i>&nbspLấy lại OTP sau <span style="color: red" id="timeOtp"></span>s</div>' +
                    '<div id="show-resend-otp" class="hidden" style="text-align: center; padding-top: 10px;color:#3699FF;"><a class="saction" href="#" onclick="Dialog._otp.resend()"><i style="color:#3699FF;" class="fas fa-share"></i>&nbspGửi lại OTP?</a></div>' +
                    '</form>';
            }
            className += "dialog-otp";

        } else {
            return;
        }
        window.bootbox.dialog({
            title: title,
            message: message,
            className: className,
            closeButton: false,
            buttons: {
                cancel: {
                    label: "Hủy",
                    className: 'btn-default',
                    callback: function () {
                        console.log('Dialog cancel');
                    }
                },
                ok: {
                    label: "Xác nhận",
                    className: 'btn-success',
                    callback: function () {
                        let _m = $(this);
                        _m.find("button").prop('disabled', true);
                        setTimeout(function () {
                            _m.find("button").prop('disabled', false);
                        }, 500);
                        let input = _m.find('.dialog-fn-input').val();
                        callback(input);
                        return false;
                    }
                }
            },
        }).on('shown.bs.modal', function (e) {
            let $modal = $(".bootbox.dialog-ngt");
            if (type === 'OTP') {
                Sv.ShowOtpResend($("#timeOtp"), Sv.OtpTimeOut);
            }
            Dialog.flag = true;
            $modal.find("input").on('keydown', function (e) {
                if (e.keyCode === 13) {
                    $modal.find('.bootbox-accept').trigger("click");
                    return false;
                }
            });
        }).on('hidden.bs.modal', function (e) {
            Dialog.flag = false;
            clearLocalStorage(['process_modal']);
            $(".bootbox.dialog-ngt").find("input").off('keydown')
        });
    },
    _modalCreatePw2: function () {
        if (Dialog.flag == true) {
            return;
        }
        var title = "Tạo mật khẩu cấp 2";
        var message = '<form class="bootbox-form">' +
            '<div class="bootbox-prompt-message">Vui lòng tạo mật khẩu cấp 2 để thực hiện giao dịch</div>' +
            '</form>';

        window.bootbox.dialog({
            title: title,
            message: message,
            className: 'dialog-ngt dialog-noe',
            closeButton: false,
            buttons: {
                cancel: {
                    label: "Hủy",
                    className: 'btn-default',
                    callback: function () {
                        console.log('Dialog cancel');
                    }
                },
                ok: {
                    label: "Tạo mật khẩu",
                    className: 'btn-success',
                    callback: function () {
                        window.location.href = "/Profile/Password2Level";
                    }
                }
            },
        }).on('shown.bs.modal', function (e) {
            Dialog.flag = true;
        }).on('hidden.bs.modal', function (e) {
            Dialog.flag = false;
        });
    },
    _password2Level: {
        check: function () {
            return abp.services.app.profile.checkLevel2Password();
        },
        verify: function (type, inputVal) {
            return new Promise((resolve, reject) => {
                if (!inputVal || inputVal.length == 0) {
                    abp.message.error('Vui lòng nhập mật khẩu cấp 2');
                    reject('value null');
                } else {
                    abp.services.app.profile.verifyLevel2Password({ password: inputVal })
                        .then(function (e) {
                            resolve('ok');
                        })
                        .catch(function (e) {
                            reject(e);
                        });
                }
            });
        },
    },
    // verify code
    verifyTransCode: function (t, callback) {
        const type = abp.setting.getInt("Abp.PaymentMethod.Web");
        console.log(type);
        if (type === Dialog.verifyTransType.None) {
            console.log("None check verify");
            callback('done', { type: "GD", value: 0 });
        } else if (type === Dialog.verifyTransType.LelvelPass) {
            //Nếu dùng mật khẩu cấp 2
            // check
            Dialog._password2Level.check().then(function (isPw2) {
                if (!isPw2) {
                    Dialog._modalCustome("Pw2_Create");
                    return;
                }
                // open modal
                Dialog._modalCustome("Pw2", function (inputVal) {
                    // verify pass
                    Dialog._password2Level.verify("GD", inputVal)
                        // done
                        .then(function () {
                            bootbox.hideAll();
                            if (typeof callback === "function") {
                                callback('done', { type: "GD", value: inputVal });
                            }
                        })
                        .catch(function (e) {
                            console.log("password2Level: " + JSON.stringify(e))
                        });
                });
            });
        } else {
            const obj = { "authen": true, "type": t };
            // open modal
            Dialog._otp.send(obj)
                .then(function (e) {
                    Dialog._otp.process(obj, callback)
                })
                .catch(function (e) {
                    console.log("OTP: " + JSON.stringify(e))
                });
        }
    },
    // verify Init
    password2Level: function (callback) {
        // check
        Dialog._password2Level.check().then(function (isPw2) {
            if (!isPw2) {
                Dialog._modalCustome("Pw2_Create");
                return;
            }
            // open modal
            Dialog._modalCustome("Pw2", function (inputVal) {
                // verify pass
                Dialog._password2Level.verify("GD", inputVal)
                    // done
                    .then(function () {
                        bootbox.hideAll();
                        if (typeof callback === "function") {
                            callback('done', { type: "GD", value: inputVal });
                        }
                    })
                    .catch(function (e) {
                        console.log("password2Level: " + JSON.stringify(e))
                    });
            });
        });

    },
    testPw2: function () {
        Dialog.password2Level(function (msg, data) {
            console.log("password2Level: " + msg, data);
            alert("longld1")
        });
    },
    //////////////////           ====================================================================
    _otp: {
        resend: function () {
            const obj = JSON.parse(getLocalStorage("process_modal"));
            obj.isResend = true;
            if (obj.authen) {
                abp.services.app.otp.sendOtpAuth(obj).done(function (rs) {
                    abp.message.success("Gửi lại mã xác thực thành công");
                    setLocalStorage("Otp_Resend_Count",)
                    Sv.ShowOtpResend($("#timeOtp"), Sv.OtpTimeOut);
                    $("#show-resend-otp").addClass("hidden").removeClass("show");
                    $("#show-text-otp").addClass("show").removeClass("hidden");
                }).always(function () {
                });
            } else {
                abp.services.app.otp.sendOtp(obj).done(function (rs) {
                    abp.message.success("Gửi lại mã xác thực thành công");
                    Sv.ShowOtpResend($("#timeOtp"), Sv.OtpTimeOut);
                    $("#show-resend-otp").addClass("hidden").removeClass("show");
                    $("#show-text-otp").addClass("show").removeClass("hidden");
                }).always(function () {
                });
            }


            // const obj =  JSON.parse(getLocalStorage("process_modal"));
            // Dialog._otp.send(obj);
        },
        send: function (obj) {
            if (obj.authen)
                return abp.services.app.otp.sendOtpAuth(obj);
            else
                return abp.services.app.otp.sendOtp(obj);
        },
        verify: function (obj) {
            return new Promise((resolve, reject) => {
                if (!obj.otp || obj.otp.length == 0) {
                    abp.message.error('Vui lòng nhập mã xác nhận');
                    reject('value null');
                } else {
                    var _ajax;
                    if (obj.authen) {
                        _ajax = abp.services.app.otp.verifyOtpAuth(obj);
                    } else {
                        _ajax = abp.services.app.otp.verifyOtp(obj);
                    }
                    Sv.RequestStart();
                    _ajax
                        .then(function (e) {
                            resolve('ok');
                        })
                        .catch(function (e) {
                            Sv.RequestEnd();
                            reject(e);
                        });
                }
            });
        },
        process: function (obj, callback) {
            setLocalStorage("process_modal", JSON.stringify(obj));
            Dialog._modalCustome("OTP", function (inputVal) {
                //Sv.RequestStart();
                // verify Otp
                obj.otp = inputVal;
                Dialog._otp.verify(obj)
                    // done
                    .then(function () {
                        //Sv.RequestEnd();
                        bootbox.hideAll();
                        if (typeof callback === "function") {
                            callback('done', obj);
                        }
                    })
                    .catch(function (e) {
                        //Sv.RequestEnd();
                        console.log("OTP: " + JSON.stringify(obj));
                        console.log("OTP: " + JSON.stringify(e));
                    });
            });
        },
    },
    otpType: {
        Transfer: 1,
        PayBill: 2,
        Payment: 3,
        ResetPass: 4,
        ChangePassLevel2: 5,
        Register: 6,
        Login: 7,
        ChangePaymentMethod: 8
    },
    verifyTransType: {
        LelvelPass: 1,
        ODP: 2,
        OTP: 3,
        None: 0
    },
    otpNone: function (t, phone, callback) {
        var obj = { "authen": false, "type": t, "phoneNumber": phone };
        // open modal
        Dialog._otp.send(obj)
            .then(function (e) {
                Dialog._otp.process(obj, callback)
            })
            .catch(function (e) {
                console.log("OTP: " + JSON.stringify(e))
            });
    },
    otp: function (t, callback) {
        var obj = { "authen": true, "type": t };
        // open modal
        Dialog._otp.send(obj)
            .then(function (e) {
                Dialog._otp.process(obj, callback)
            })
            .catch(function (e) {
                console.log("OTP: " + JSON.stringify(e))
            });
    },
    testOtp: function () {
        Dialog.otp(Dialog.otpType.ChangePassLevel2, function (msg, data) {
            console.log("Dialog.otpType.ChangePassLevel2: " + msg, data)
            alert("longld1")
        });
    },
    testOtp2: function () {
        Dialog.otpNone(Dialog.otpType.Register, function (msg, data) {
            console.log("Dialog.otpType.Register: " + msg, data)
            alert("longld1")
        });
    },
    deposit: function (msg, isBtn) {
        if (!isBtn) {
            abp.message.info(msg);
        } else {
            abp.message.confirm(msg, '&nbsp;',
                function (isConfirmed) {
                    if (isConfirmed) {
                        window.open('/Transactions/Deposit', '_blank');
                    }
                }, {
                title: "",
                buttons: ["Đóng", "Nạp tiền"],
            });
        }
    },
    verify: function (msg, isBtn) {
        if (!isBtn) {
            abp.message.info(msg);
        } else {
            abp.message.confirm(msg, '&nbsp;',
                function (isConfirmed) {
                    if (isConfirmed) {
                        setLocalStorage('modalVerify', true);
                        window.open('/Profile/Edit', '_blank');
                    }
                }, {
                title: "",
                buttons: ["Đóng", "Xác thực ngay"],
            });
        }
    },
    sendCode: function (obj) {
        obj.isResend = true;
        abp.ui.setBusy();
        if (obj.authen)
            //return abp.services.app.otp.sendOtpAuth(obj);
            abp.services.app.otp.sendOtpAuth(obj).done(function (rs) {
                abp.message.success("Gửi lại mã xác thực thành công");
            }).always(function () {
                abp.ui.clearBusy();
            });

        else
            //return abp.services.app.otp.sendOtp(obj);
            abp.services.app.otp.sendOtp(obj).done(function (rs) {
                abp.message.success("Gửi lại mã xác thực thành công");
            }).always(function () {
                abp.ui.clearBusy();
            });
    },
}

const VietNamMobile = {
    prefixConfig: {
        VTE: ["086", "096", "097", "098", "032", "033", "034", "035", "036", "037", "038", "039"],
        VNA: ["091", "094", "083", "084", "085", "081", "082", "087", "088"],
        VMS: ["090", "093", "070", "076", "077", "078", "079", "089"],
        GMOBILE: ["099", "059"],
        VNM: ["092", "052", "056", "058"]
    },
    removeSpaces: function (val) {
        return val.split(' ').join('');
    },
    prefixAll: function () {
        var allPrefix = [];
        Object.keys(VietNamMobile.prefixConfig)
            .forEach(function (key) {
                allPrefix = allPrefix.concat(VietNamMobile.prefixConfig[key]);
            });
        return allPrefix;
    },
    isActive: function (str) {
        var prefix = str.substring(0, 3);
        return VietNamMobile.prefixAll().indexOf(prefix) > -1;
    },
    valid: function (str) {

        if (!(/^0[0-9]{9}$/).test(str)) {
            return ("Số điện thoại không đúng định dạng");
        }
        if (!VietNamMobile.isActive(str)) {
            return ("Số điện thoại không được hỗ trợ");
        }
        return "";
    },
    getTelco: function (str) {
        let telco = '';
        if (VietNamMobile.isActive(str)) {
            let prefix = str.substring(0, 3);
            Object.keys(VietNamMobile.prefixConfig)
                .forEach(function (key) {
                    if (VietNamMobile.prefixConfig[key].indexOf(prefix) > -1)
                        telco = key;
                });
        }
        return telco;
    },
}

var bindVerify = {
    init: function (id) {
        if (id === undefined)
            id = "#blockVerify";
        let $block = $(id);
        $block.hide();
        let flagOpenModal = getLocalStorage('modalVerify');
        if (flagOpenModal.length > 0) {
            $block.show();
            clearLocalStorage(['modalVerify']);
        }
        $block.find('select[select-auto="CityId"]').select2({ width: '100%' });
        $block.find('select[select-auto="DistrictId"]').select2({ width: '100%' });
        $block.find('select[select-auto="WardId"]').select2({ width: '100%' });

        bindVerify.loadProvince();
        $block.find('select[select-auto="CityId"]').on('change', bindVerify.changeProvince);
        $block.find('select[select-auto="DistrictId"]').on('change', bindVerify.changeDistrict);
    },
    loadProvince: function () {
        let $p = $('select[select-auto="CityId"]');
        let $d = $('select[select-auto="DistrictId"]');
        let $w = $('select[select-auto="WardId"]');
        if ($p.length > 0) {
            abp.services.app.commonLookup.getProvinces()
                .done(function (result) {
                    if (result.length > 0) {
                        let html = "<option value=''>Chọn Tỉnh/ TP</option>";
                        let id = $p.data('value');
                        for (let i = 0; i < result.length; i++) {
                            let item = result[i];
                            html += ("<option value='" + (item.id) + "' " + (item.id == id ? "selected" : "") + " >" + (item.cityName) + "</option>");
                        }
                        $p.removeAttr('data-value');
                        $p.html(html).select2({ width: '100%' }).trigger('change');
                    }
                });
        }
    },
    changeProvince: function () {
        let $d = $('select[select-auto="DistrictId"]');
        let $w = $('select[select-auto="WardId"]');
        if ($d.length > 0) {
            let $p = $('select[select-auto="CityId"]');
            let pId = $p.length > 0 ? $p.val() : "";
            abp.services.app.commonLookup.getDistricts(pId, false)
                .done(function (result) {
                    if (result.length > 0) {
                        let html = "<option value=''>Chọn Quận/ Huyện</option>";
                        let id = $d.data('value');
                        for (let i = 0; i < result.length; i++) {
                            let item = result[i];
                            html += ("<option value='" + (item.id) + "' " + (item.id == id ? "selected" : "") + " >" + (item.districtName) + "</option>");
                        }
                        $d.removeAttr('data-value');
                        $d.html(html).select2({ width: '100%' }).trigger('change');
                        if ($w.length > 0) {
                            $w.html("<option value=''>Chọn Phường/ Xã</option>").trigger('change');
                        }
                    }
                });
        }
    },
    changeDistrict: function () {
        let $w = $('select[select-auto="WardId"]');
        if ($w.length > 0) {
            let $d = $('select[select-auto="DistrictId"]');
            let dId = $d.length > 0 ? $d.val() : "";
            abp.services.app.commonLookup.getWards(dId, false)
                .done(function (result) {
                    if (result.length > 0) {
                        let html = "<option value=''>Chọn Phường/ Xã</option>";
                        let id = $w.data('value');
                        for (let i = 0; i < result.length; i++) {
                            let item = result[i];
                            html += ("<option value='" + (item.id) + "' " + (item.id == id ? "selected" : "") + " >" + (item.wardName) + "</option>");
                        }
                        $w.removeAttr('data-value');
                        $w.html(html).select2({ width: '100%' }).trigger('change');
                    }
                });
        }
    },
    loadImageInfo: function (imgStr) {
        return Sv.Post2({
            Url: abp.appPath + 'Common/DetectImage',
            Data: { "image": imgStr },
        }).then(response => response.result)
            .catch(error => console.log('error', error));
    },
    detectImage: function (file) {
        return new Promise((resolve, reject) => {
            var formData = new FormData();
            formData.append("file", file);
            Sv.AjaxPostFile({
                Url: abp.appPath + "Common/DetectImageFile",
                Data: formData
            }, function (response) {
                resolve(response.result);
            }, function () {
                abp.message.error("Upload file lỗi!");
            });
        });
    },
    suggestUserInfo: function (response, key) {
        console.log(response);
        let info_str = (getLocalStorage('identity_info'));
        let info = null;
        if (info_str != null && info_str.length > 0 && info_str != "undefined" && info_str != "null")
            info = JSON.parse(info_str);
        else
            info = {};
        if (response != null) {
            info[key] = JSON.parse(response);
        }
        setLocalStorage('identity_info', JSON.stringify(info));
        return info;
    },
    loadImageTest: function (imgStr) {
        var header = new Headers();
        header.append("api-key", "5dabd636-636f-11ea-b479-b42e99011dd9");
        header.append("Content-Type", "application/json");
        var requestOptions = {
            method: 'POST',
            headers: header,
            body: JSON.stringify({ "image": imgStr }),
            redirect: 'follow'
        };
        return fetch("https://aisol.vn/ekyc/recognition", requestOptions)
            .then(response => response.text())
            .catch(error => console.log('error', error));
    },
}


// $(document).ajaxError(function (xhr, props) {
//     if (props.status === 401) {
//         location.reload();
//     }
// });

$(document).on('click', '.euiButton-reset', function () {
    $('.filter-block-header').find('input, select')
        .each(function () {
            if ($(this).is('select')) {
                if ($(this).hasClass('ignore')) {
                    $(this).val('').trigger('change');
                } else {
                    $(this).find('option:first').prop('selected', true);
                    if ($(this).is('[class*="select2"]')) {
                        $(this).attr('placeholder', 'Tất cả').select2();
                    }
                }
            } else {
                $(this).val('');
            }
        });
});