var print = {
    getData: function (transcode, callback) {
        Sv.Post({
                Url: abp.appPath + 'Common/GetTransactionPrint',
                Data: {transcode: transcode}
            },
            function (rs) {
                callback("1", rs);
            },
            function (e) {
                callback("00", e);
            });
    },
    card: function (transcode) {
        ngtAction.open(transcode, 'print');
    },
    // nếu mobile => in mobile
    // néu desktop => in nhiệt
    printOther: function (transcode) {
        print.getData(transcode, function (code, rs) {
            if (code === "1") {
                if (Sv.isMobile()) {
                    print.mobile(rs.result.data)
                } else {
                    print.open(rs.result.content, 480, 600, false)
                }
            } else {
                abp.notify.info(e);
            }
        });
    },
    mobile: function (trans) {
        if (trans == null) {
        } else {
            if (trans.transactionInfo !== undefined && trans.transactionInfo.serviceCode === "TOPUP") {
                return print.orderType.TOPUP(trans);
            } else if (trans.transactionInfo !== undefined && trans.transactionInfo.serviceCode === "TOPUP_DATA") {
                return print.orderType.TOPUP_DATA(trans);
            } else if (trans.transactionInfo !== undefined && trans.transactionInfo.serviceCode === "PAY_BILL") {
                return print.orderType.PAY_BILL(trans);
            } else if (trans.transactionInfo !== undefined && trans.transactionInfo.serviceCode === "PIN_DATA") {
                return print.orderType.PIN_DATA(trans);
            } else if (trans.transactionInfo !== undefined && trans.transactionInfo.serviceCode === "PIN_CODE") {
                return print.orderType.PIN_CODE(trans);
            } else if (trans.transactionInfo !== undefined && trans.transactionInfo.serviceCode === "PIN_GAME") {
                return print.orderType.PIN_GAME(trans);
            }
        }
    },

    sr: function (s) {
        s = String(s);
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

        for (var k in char_map) {
            s = s.replace(RegExp(k, 'g'), char_map[k]);
        }
        return s;
    },
    r: function (key, value, type = "string") {
        if (value === undefined) {
            value = "";
        }
        let t = "";
        if (key != null && key.length > 0) {
            t = app.localize(key);
            if (t.indexOf(':') < 0 && value.length > 0) {
                t += ": "
            }
            if (value.length > 0) {
                t += " ";
            }
        }
        var str = "";
        if (type === "amount")
            str = (t + Sv.NumberToString(value) + 'đ');
        else if (type === "date")
            str = (t + moment(value).format("DD/MM/YYYY HH:mm:ss"));
        else
            str = (t + value);
        return print.sr(str);
    },
    orderType: {
        open: function (content) {
            let str = "com.fidelier.printfromweb://$small$" + content + "$intro$$intro$$intro$$drawer$";
            window.open(str);
        },
        PAY_BILL: function (obj) {
            if (obj == null || obj.transcode == null || obj.transactionInfo == null) {
                abp.notify.error("Không có thông tin giao dịch, vui lòng kiểm tra lại");
            } else {
                let extraInfo = obj.transactionInfo.invoice;
                //window.objTest = obj;
                //console.log(obj); 
                let content = "";
                content += "$bighw$" + "   " + print.r("PrintCard_Header_Brand", "") + "$intro$";
                content += "$small$" + print.r("PrintBill_Receipt_Payment", "") + "$intro$";
                content += "$small$" + print.r("Print_Provider", obj.transactionInfo.productName) + "$intro$";
                content += "$small$" + print.r("PrintCard_TransCode", obj.transcode) + "$intro$";
                content += "$small$" + print.r("PrintCard_CreatedDate", obj.transactionInfo.createdTime, "date") + "$intro$";
                content += "$small$" + print.r("Print_Customer_Code", extraInfo != null && extraInfo.customerReference != null ? extraInfo.customerReference : "") + "$intro$";
                content += "$small$" + print.r("Print_Customer_Name", extraInfo != null && extraInfo.fullName != null ? extraInfo.fullName : "") + "$intro$";
                content += "$small$" + print.r("Print_Customer_Address", extraInfo != null && extraInfo.address != null ? extraInfo.address : "") + "$intro$";
                content += "$small$" + print.r("Print_Customer_Period", extraInfo != null && extraInfo.period != null ? extraInfo.period : "") + "$intro$";
                //content += "$small$" + print.r("Print_Amount", obj.transactionInfo.amount , "amount") + "$intro$";
                //content += "$small$" + print.r("Print_Collection_Fee", obj.transactionInfo.fee , "amount") + "$intro$";
                let total_payment = parseInt(obj.transactionInfo.amount);// + parseInt(obj.transactionInfo.fee);
                //content += "$small$" + print.r("Print_Total_Payment", total_payment , "amount") + "$intro$";
                content += "$small$" + print.r("Print_Amount", total_payment, "amount") + "$intro$";

                if (!isEmptyValue(obj.transactionInfo.customerSupportNote)) {
                    content += "$small$" + print.r("PrintCard_Customer_Help_Title", obj.transactionInfo.customerSupportNote) + "$intro$";
                }

                content += "$small$" + print.r("Agent_Label", obj.network.agentName) + "$intro$";
                content += "$small$" + print.r("Address", obj.address) + "$intro$";
                content += "$intro$";
                print.orderType.open(content);
            }
        },
        PIN_CODE: function (obj) {
            if (obj == null || obj.transcode == null || obj.transactionInfo == null || obj.items == null || obj.items.length == 0) {
                abp.notify.error("Không có thông tin giao dịch, vui lòng kiểm tra lại");
            } else {
                let content = "";
                for (let i = 0; i < obj.items.length; i++) {
                    let item = obj.items[i];
                    let vendor = getVendor(item.productCode, obj.vendors);
                    content += "$bighw$" + "   " + print.r("PrintCard_Header_Brand", "") + "$intro$";
                    content += "$small$" + print.r("PrintCard_CardCode", "") + " " + vendor.name + " " + print.r("", item.cardValue, "amount") + "$intro$";
                    content += "$small$" + print.r("PrintCard_TransCode", obj.transcode) + "$intro$";
                    content += "$small$" + print.r("PrintCard_CreatedDate", obj.transactionInfo.createdTime, "date") + "$intro$";
                    content += "$small$" + print.r("PrintCard_Serial", splitCode(item.serial)) + "$intro$";
                    content += "$bigh$" + "  " + print.r("", splitCode(item.cardCode)) + "$intro$";
                    content += "$small$" + print.r("PrintCard_HSD", moment(item.expiredDate).format("DD/MM/YYYY")) + "$intro$";

                    if (!isEmptyValue(obj.transactionInfo.userManualNote)) {
                        content += "$small$" + print.r("PrintCard_Help_Title", obj.transactionInfo.userManualNote) + "$intro$";
                    }

                    if (!isEmptyValue(obj.transactionInfo.customerSupportNote)) {
                        content += "$small$" + print.r("PrintCard_Customer_Help_Title", obj.transactionInfo.customerSupportNote) + "$intro$";
                    }

                    content += "$small$" + print.r("Agent_Label", obj.network.agentName) + "$intro$";
                    content += "$small$" + print.r("Address", obj.address) + "$intro$";
                    content += "$intro$";
                }
                print.orderType.open(content);
            }
        },
        PIN_DATA: function (obj) {
            if (obj == null || obj.transcode == null || obj.transactionInfo == null || obj.items == null || obj.items.length == 0) {
                abp.notify.error("Không có thông tin giao dịch, vui lòng kiểm tra lại");
            } else {
                let content = "";
                for (let i = 0; i < obj.items.length; i++) {
                    let item = obj.items[i];
                    let vendor = getVendor(item.productCode, obj.vendors);
                    content += "$bighw$" + "   " + print.r("PrintCard_Header_Brand", "") + "$intro$";
                    content += "$small$" + print.r("PrintCard_CardCode", "") + " " + print.r("PrintCard_PinCode_Type", "") + " " + vendor.name + " " + print.r("", item.cardValue, "amount") + "$intro$";
                    content += "$small$" + print.r("PrintCard_TransCode", obj.transcode) + "$intro$";
                    content += "$small$" + print.r("PrintCard_CreatedDate", obj.transactionInfo.createdTime, "date") + "$intro$";
                    content += "$small$" + print.r("PrintCard_Serial", splitCode(item.serial)) + "$intro$";
                    content += "$bigh$" + "  " + print.r("", splitCode(item.cardCode)) + "$intro$";
                    content += "$small$" + print.r("PrintCard_HSD", moment(item.expiredDate).format("DD/MM/YYYY")) + "$intro$";

                    if (!isEmptyValue(obj.transactionInfo.userManualNote)) {
                        content += "$small$" + print.r("PrintCard_Help_Title", obj.transactionInfo.userManualNote) + "$intro$";
                    }

                    if (!isEmptyValue(obj.transactionInfo.customerSupportNote)) {
                        content += "$small$" + print.r("PrintCard_Customer_Help_Title", obj.transactionInfo.customerSupportNote) + "$intro$";
                    }

                    content += "$small$" + print.r("Agent_Label", obj.network.agentName) + "$intro$";
                    content += "$small$" + print.r("Address", obj.address) + "$intro$";
                    content += "$intro$$intro$";
                }
                print.orderType.open(content);
            }
        },
        PIN_GAME: function (obj) {
            if (obj == null || obj.transcode == null || obj.transactionInfo == null || obj.items == null || obj.items.length == 0) {
                abp.notify.error("Không có thông tin giao dịch, vui lòng kiểm tra lại");
            } else {
                let content = "";
                for (let i = 0; i < obj.items.length; i++) {
                    let item = obj.items[i];
                    let vendor = getVendor(item.productCode, obj.vendors);
                    content += "$bighw$" + "   " + print.r("PrintCard_Header_Brand", "") + "$intro$";
                    content += "$small$" + print.r("PrintCard_CardCode", "") + " " + vendor.name + " " + print.r("", item.cardValue, "amount") + "$intro$";
                    content += "$small$" + print.r("PrintCard_TransCode", obj.transcode) + "$intro$";
                    content += "$small$" + print.r("PrintCard_CreatedDate", obj.transactionInfo.createdTime, "date") + "$intro$";
                    content += "$small$" + print.r("PrintCard_Serial", splitCode(item.serial)) + "$intro$";
                    content += "$bigh$" + "  " + print.r("", splitCode(item.cardCode)) + "$intro$";
                    content += "$small$" + print.r("PrintCard_HSD", moment(item.expiredDate).format("DD/MM/YYYY")) + "$intro$";

                    if (!isEmptyValue(obj.transactionInfo.userManualNote)) {
                        content += "$small$" + print.r("PrintCard_Help_Title", obj.transactionInfo.userManualNote) + "$intro$";
                    }

                    if (!isEmptyValue(obj.transactionInfo.customerSupportNote)) {
                        content += "$small$" + print.r("PrintCard_Customer_Help_Title", obj.transactionInfo.customerSupportNote) + "$intro$";
                    }

                    content += "$small$" + print.r("Agent_Label", obj.network.agentName) + "$intro$";
                    content += "$small$" + print.r("Address", obj.address) + "$intro$";
                    content += "$intro$$intro$";
                }
                print.orderType.open(content);
            }
        },
        TOPUP: function (obj) {
            if (obj == null || obj.transcode == null || obj.transactionInfo == null) {
                abp.notify.error("Không có thông tin giao dịch, vui lòng kiểm tra lại");
            } else {
                let content = "";
                let vendor = getVendor(obj.transactionInfo.productCode, obj.vendors);
                content += "$bighw$" + "   " + print.r("PrintCard_Header_Brand", "") + "$intro$";
                content += "$small$" + print.r("PrintTopup_ServiceName", vendor.name) + "$intro$";
                content += "$small$" + print.r("PrintCard_TransCode", obj.transcode) + "$intro$";
                content += "$small$" + print.r("PrintCard_CreatedDate", obj.transactionInfo.createdTime, "date") + "$intro$";
                content += "$small$" + print.r("PrintTopup_ReceiverPhone", obj.transactionInfo.receiverInfo) + "$intro$";
                content += "$small$" + print.r("PrintTopup_Value", obj.transactionInfo.amount, "amount") + "$intro$";

                if (!isEmptyValue(obj.transactionInfo.customerSupportNote)) {
                    content += "$small$" + print.r("PrintCard_Customer_Help_Title", obj.transactionInfo.customerSupportNote) + "$intro$";
                }

                content += "$small$" + print.r("Agent_Label", obj.network.agentName) + "$intro$";
                content += "$small$" + print.r("Address", obj.address) + "$intro$";
                content += "$intro$";
                print.orderType.open(content);
            }
        },
        TOPUP_DATA: function (obj) {
            if (obj == null || obj.transcode == null || obj.transactionInfo == null) {
                abp.notify.error("Không có thông tin giao dịch, vui lòng kiểm tra lại");
            } else {
                let content = "";
                let vendor = getVendor(obj.transactionInfo.productCode, obj.vendors);
                content += "$bighw$" + "   " + print.r("PrintCard_Header_Brand", "") + "$intro$";
                content += "$small$" + print.r(obj.transactionInfo.categoryName) + "$intro$";
                content += "$small$" + print.r("PrintCard_TransCode", obj.transcode) + "$intro$";
                content += "$small$" + print.r("PrintCard_CreatedDate", obj.transactionInfo.createdTime, "date") + "$intro$";
                content += "$small$" + print.r("PrintTopup_ReceiverPhone", obj.transactionInfo.receiverInfo) + "$intro$";
                content += "$small$" + print.r("Gói Data", obj.transactionInfo.description) + "$intro$";
                content += "$small$" + print.r("PrintTopup_Value", obj.transactionInfo.amount, "amount") + "$intro$";

                if (!isEmptyValue(obj.transactionInfo.customerSupportNote)) {
                    content += "$small$" + print.r("PrintCard_Customer_Help_Title", obj.transactionInfo.customerSupportNote) + "$intro$";
                }

                content += "$small$" + print.r("Agent_Label", obj.network.agentName) + "$intro$";
                content += "$small$" + print.r("Address", obj.address) + "$intro$";
                content += "$intro$";
                print.orderType.open(content);
            }
        },
    },

    open: function (htmlContent, width, height, isAuto) {
        var mywindow = window.open('', '', 'height=' + height + ',width=' + width + ',scrollbars=yes');
        mywindow.document.write(print.wardHtml(htmlContent));
        // ko auto
        if (isAuto) {
            // nhỏ hơn 400 thì tèo để 350 cho khủng
            setTimeout(function () {
                mywindow.print();
            }, 400);
            mywindow.onfocus = function () {
                setTimeout(function () {
                    mywindow.close();
                }, 410);
            }
        }
        mywindow.focus();
    },
    wardHtml: function (htmlData) {
        var html = ('<html>');
        html += ('<head>');
        // html += ('<link rel="stylesheet" href="/themes/topup/css/style1.css?v=1" type="text/css" />');
        // html += ('<link rel="stylesheet" href="/themes/topup/print/print.css?v=' + (new Date()).getTime() + '" type="text/css" />');
        html += ('<link rel="stylesheet" href="/themes/topup/print/print_20210118.css?v=' + (new Date()).getTime() + '" type="text/css" />');
        html += ('</head>');
        html += ('<body class="print-body">');
        html += ('<div class="box-action noprint">');
        html += ('<button class="btn print-closed-btn" onclick="winClose()">Đóng</button>');
        html += ('<button class="btn print-btn" onclick="winPrint()">In</button>');
        html += ("</div>");
        html += ('<div id="print-hls-content" class="print-hls-content">');
        html += htmlData;
        html += ("<div class='foo_print'></div>");
        html += ("</div>");
        html += ('</body>');
        html += ('<script src="/themes/topup/print/print.js?v=' + (new Date()).getTime() + '"></script>');
        html += ('</html>');
        return html;
    },
};


const isEmptyValue = (value) => {
    return value === '' || value === null || value === undefined;
}

const splitCode = (value) => {
    return value.match(/.{1,3}/g).join(" ");
}

function getVendor(categoryCode, lst) {
    var c = categoryCode.split("_");
    var vendor = lst.filter(x => x.code === c[0]);
    if (vendor.length > 0) {
        return vendor[0];
    }
    return {
        code: c[0],
        name: c[0],
    };
}

var ngtAction = {
    open: function (transcode, action) {
        if (action === "export") {
            ngtAction.export(transcode);
            return false;
        }
        ngtAction.getData(transcode)
            .then(function (rs) {
                if (!rs.success) {
                    abp.notify.info(rs);
                } else if (action === "copy") {
                    ngtAction.copy(rs);
                } else if (action === "print") {
                    ngtAction.print(rs);
                } else if (action === "printA4") {
                    ngtAction.printA4(rs);
                }
            });
    },
    getData: function (transcode) {
        return Sv.Post2({
            url: abp.appPath + 'Common/GetTransactionPrint',
            data: ({transcode: transcode}),
        });
    },
    // copy nhanh
    copy: function (response) {
        // Clipboard.copy("test copy");
        // abp.message.info("Đã copy test");

        if (response.result.data == null || response.result.data.items == null || response.result.data.items.length == 0) {
            abp.notify.info("Chưa có thông tin mã thẻ, vui lòng chờ trong giây lát!");
            return false;
        }
        var content = "";
        for (let i = 0; i < response.result.data.items.length; i++) {
            let item = response.result.data.items[i];
            let vendor = getVendor(item.categoryCode, response.result.data.vendors);
            content += Sv.Sprintf("Loại thẻ: %s - Số serial: %s - Mã nạp thẻ: %s \n", vendor.name + " " + Sv.NumberToString(item.cardValue) + "đ", item.serial, item.cardCode);
        }
        if (content.length == 0) {
            abp.notify.info("Chưa có thông tin mã thẻ, vui lòng chờ trong giây lát!");
            return false;
        }
        Clipboard.copy(content);
        abp.message.info("Đã copy");
    },
    // print
    print: function (response) {
        if (Sv.isMobile()) {
            print.mobile(response.result.data)
        } else {
            print.open(response.result.content, 480, 600, false)
        }
    },
    // in A4
    printA4: function (response) {
        var c = "<div class='print_a4'>" + response.result.content + "</div>";
        print.open(c, 860, 800, false)
    },
    // exp,ort excel
    export: function (transcode) {
        abp.services.app.transactions.getTransactionDetailHistoryToExcel({transCode: transcode})
            .done(function (result) {
                app.downloadTempFile(result);
            });
    },

}

jQuery(document).ready(function () {
    if (Sv.isMobile()) {
        jQuery(".btn-printMobile").show();
        jQuery(".btn-printDesktop").hide();
    } else {
        jQuery(".btn-printMobile").hide();
        jQuery(".btn-printDesktop").show();
    }
});
