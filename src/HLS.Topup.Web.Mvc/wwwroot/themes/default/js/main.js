function showAlert(n, t, i) {
    showMessage("Thông báo", t, i)
}

function scrollTo(n) {
    $("html, body").animate({
        scrollTop: $(n).offset().top
    }, "slow")
}

function htmlEscape(n) {
    return String(n).replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;").replace(/"/g, "&quot;")
}

function random(n, t) {
    return Math.floor(Math.random() * (t - n + 1)) + n
}

function copyToClipboard(n) {
    var t;
    document.selection ? (t = document.body.createTextRange(), t.moveToElementText(document.getElementById(n)), t.select().createTextRange(), document.execCommand("Copy")) : window.getSelection && (t = document.createRange(), t.selectNode(document.getElementById(n)), window.getSelection().addRange(t), document.execCommand("Copy"))
}

function showConfirm(n, t, i) {
    $.confirm({
        title: n,
        content: t,
        theme: "material",
        type: "red",
        buttons: {
            yes: {
                btnClass: "btn-red",
                action: function() {
                    i()
                }
            },
            cancel: function() {}
        }
    })
}

function showMessage(n, t, i, r) {
    var u;
    u = i == "warning" ? "yellow" : i == "danger" ? "red" : i == "success" ? "green" : "blue";
    $.confirm({
        title: n,
        content: t,
        theme: "material",
        type: u,
        animation: "zoom",
        scrollToPreviousElement: !1,
        columnClass: "medium",
        buttons: {
            close: function() {
                r && getType.toString.call(r) === "[object Function]" && r()
            }
        }
    })
}

function isValidEmail(n) {
    return /^([a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+(\.[a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+)*|"((([ \t]*\r\n)?[ \t]+)?([\x01-\x08\x0b\x0c\x0e-\x1f\x7f\x21\x23-\x5b\x5d-\x7e\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|\\[\x01-\x09\x0b\x0c\x0d-\x7f\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))*(([ \t]*\r\n)?[ \t]+)?")@(([a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.)+([a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.?$/i.test(n)
}

function htmlAjaxLoad() {
    $(".html_ajax_load").each(function() {
        var n = $(this),
            t = n.attr("data-url");
        n.load(t, function() {
            n.attr("id") === "mobile-menu" && $("#mobile-menu").mmenu({
                offCanvas: {
                    position: "right"
                }
            })
        })
    })
}

function addRequestVerificationToken(n, t) {
    return n.__RequestVerificationToken = $(t).find("input[name=__RequestVerificationToken]").val(), n
}

function alerthtml(n, t) {
    var i = '<div class="alert alert-' + t + ' alert-dismissible" role="alert">';
    return i += '<button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;<\/span><\/button>', i += "<strong>" + n + "<\/strong>", i + "<\/div>"
}

function add_active_class() {
    $(".price-tag-item").on("click", function(n) {
        n.preventDefault();
        $(".price-tag-item").removeClass("active");
        $(this).addClass("active");
        var t = $(this).attr("data-amount-id"),
            i = $("#topup-form #quantity").val(),
            r = $("#topup-form #bank_id").val();
        get_server_price(t, r, i)
    });
    $("#topup-form #bank-topup .bank-icon").on("click", function(n) {
        var i, t, r, u;
        n.preventDefault();
        i = $(this);
        t = i.attr("data-id");
        $("#topup-form #bank-topup .bank-icon").removeClass("active");
        $("#topup-form #bank_id").val(t);
        $(this).addClass("active");
        r = $("#topup-form #amount_id").val();
        u = $("#topup-form #quantity").val();
        get_server_price(r, t, u)
    });
    $("#topup-form #quantity").on("change", function() {
        var n = $("#topup-form #amount_id").val(),
            t = $("#topup-form #bank_id").val(),
            i = $(this).val();
        get_server_price(n, t, i)
    });
    $(".btn-topup-game").on("click", function(n) {
        n.preventDefault();
        $(".btn-topup-game").removeClass("active");
        $(this).addClass("active");
        var t = $(this);
        $(".tu_form").find("input").removeAttr("required");
        $(".tu_form").find("input[type=number]").val(1);
        $("#topup-form #quantity").trigger("change");
        $("#topup-form #topup_type").val($(this).attr("data-topup-type"));
        $(".tu_form").fadeOut(0, function() {
            $(t.attr("data-target")).fadeIn(0, function() {
                $(this).find("input").attr("required", "required")
            })
        })
    })
}

function set_amount_display(n, t, i) {
    $("#topup-form #amount_id").val(n);
    $("#topup-form #topup_amount").html(t);
    $("#topup-form #topup_total_amount").html(format_number(i) + " đ")
}

function format_number(n) {
    return (n + "").replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,")
}

function get_server_price(n, t, i) {
    var u = $("#topup-form #link-ultils").attr("data-get-price"),
        r;
    $("#topup-form").find(".price-tag-item.active").length > 0 && (r = $("#topup-form #TransactionModel_UserId").val(), $.ajax({
        url: u,
        method: "GET",
        data: {
            amountId: n,
            paymentId: t,
            quantity: i,
            userId: r
        },
        success: function(t) {
            var i = $("#topup-form").find(".price-tag-item.active");
            t.status ? set_amount_display(n, i.attr("data-amount-display"), t.totalPrice) : console.log(t)
        }
    }))
}

function scrollSidebar() {
    var n = $("#sidebar");
    $(window).scroll(function() {
        if ($(window).width() > 1024) {
            var t = $(this).scrollTop() + $("header.header").height() + 40,
                u = n.offset().top,
                i = $("footer#footer").offset().top,
                r = n.find(".sidebar-inner");
            if (t >= u && t + r.height() < i) n.find(".sidebar-inner").css({
                position: "fixed",
                width: n.width() + "px",
                top: $("header.header").height() + 40 + "px",
                bottom: "auto"
            });
            else if (t + r.height() >= i) {
                var f = $(window).height(),
                    e = $(window).scrollTop(),
                    o = $("footer#footer").offset().top,
                    s = Math.abs(o - e),
                    h = f - s;
                n.find(".sidebar-inner").css({
                    position: "fixed",
                    width: n.width() + "px",
                    bottom: h + "px",
                    top: "auto"
                })
            } else n.find(".sidebar-inner").css({
                position: "static",
                width: "100%"
            })
        } else n.find(".sidebar-inner").css({
            position: "static",
            width: "100%"
        })
    })
}

function chat_typing() {
    $("#chat_text").keydown(function(n) {
        if (n.keyCode == 13) return $(this).val() === "" ? ($(this).focus(), !1) : ($("#chat_submit_btn").trigger("click"), !1)
    })
}

function show_hide_chatbox() {
    if ($("#chatbox").length) var n = $("#chatbox").simplechat({
        debug: !0,
        firebase: window.firebase,
        onLoad: function() {}
    });
    chat_typing();
    $("#chat_pupple").on("click", function() {
        toggle_chatbox()
    });
    $("#chatbox #close-chat").on("click", function() {
        toggle_chatbox()
    })
}

function toggle_chatbox() {
    var n = $("#chatbox");
    n.css("display") === "none" ? $("#chat_pupple").fadeOut(100, function() {
        n.show(300);
        animate_scroll()
    }) : n.hide(300, function() {
        $("#chat_pupple").fadeIn(50)
    })
}

function animate_scroll() {
    $("#chatbox .chat-content").animate({
        scrollTop: $("#chatbox .chat-content").prop("scrollHeight")
    }, 0, function() {})
}

function getFormData(n) {
    var i = n.serializeArray(),
        t = {};
    return $.map(i, function(n) {
        t[n.name.replace(".", "_")] = n.value
    }), t
}

function initTransaction() {
    $("#quick-topup-form").on("submit", function(n) {
        var f, r, u;
        if (n.preventDefault(), isSubmiting) return !1;
        if (f = getPaymentGroup(), isSubmiting = !0, r = "", f == 2)
            if (u = getCustomerInfoToSave(), u != null) r = JSON.stringify(u);
            else return isSubmiting = !1, !1;
        var i = getFormData($(this)),
            s = $(this),
            e = {
                account: i.Transaction_Account,
                amountId: i.Transaction_AmountId,
                commandCode: i.Transaction_CommandCode,
                paymentMethodId: i.Transaction_PaymentMethodId,
                quantity: i.Transaction_Quantity,
                serviceCode: i.Transaction_ServiceCode,
                userId: i.Transaction_UserId,
                provider: i.Transaction_Provider,
                customerInfo: r,
                flatform: "WEB"
            },
            o = apiPrefix + "transaction/initrans",
            t = $.confirm({
                icon: "fa fa-spinner fa-spin",
                title: "Khởi tạo giao dịch",
                content: "Giao dịch đang thực hiện, vui lòng không tắt trình duyệt ...",
                theme: "material",
                closeIcon: !1,
                columnClass: "medium",
                buttons: {
                    close: {
                        btnClass: "btn-red hidden",
                        action: function() {}
                    }
                }
            });
        $.ajax({
            url: o,
            type: "POST",
            headers: {
                "Content-Type": "application/x-www-form-urlencoded",
                Token: "FD4C26708337FEAC9E113D44EBD227F6",
                DataToken: "123",
                Flatform: "WEB"
            },
            xhrFields: {
                withCredentials: !0
            },
            data: e,
            success: function(n) {
                n.status ? (t.setType("green"), t.setIcon("fa fa-check hidden-sm hidden-xs"), t.setTitle("Khởi tạo giao dịch thành công"), t.setContent("Khởi tạo giao dịch thành công, đang chuyển hướng ...."), window.location.href = n.data.url) : (t.setType("red"), t.setIcon("fa fa-exclamation-triangle hidden-sm hidden-xs"), t.setTitle("Khởi tạo giao dịch không thành công"), t.buttons.close.removeClass("hidden"), t.setContent(n.message), n.meta.isClear && ($.removeCookie(pmiKey), hideSelectedBank(), showListBank()))
            }
        }).done(function() {
            isGetPrice = !1;
            isSubmiting = !1
        })
    })
}

function getTopupPrice(n) {
    if (isGetPrice) return !1;
    isGetPrice = !0;
    var t = $("#topup_price .price-tag"),
        i = apiPrefix + "pricetopup/listprice";
    t.html('<div class="form-group text-center"><i class="fa fa-circle-o-notch fa-spin" aria-hidden="true"><\/i><\/di>');
    $.ajax({
        url: i,
        type: "GET",
        headers: {
            "Content-Type": "application/x-www-form-urlencoded",
            Token: "FD4C26708337FEAC9E113D44EBD227F6",
            DataToken: "123",
            Flatform: "WEB"
        },
        xhrFields: {
            withCredentials: !0
        },
        data: {
            serviceCode: n
        },
        success: function(n) {
            var r = "",
                u, i;
            if (n.status) {
                for (u = 0; u < n.data.listPrice.length; u++) i = n.data.listPrice[u], r += '<div class="price-tag-item an ' + (i.isDefault ? "active " : " ") + (i.isWideItem ? "price-tag-item-wide" : " ") + '" data-koin="' + i.coin + '" data-koin-multi="' + i.coinMultiTimes + '" data-koin-discount="' + i.coinDiscount + '" data-amount-id="' + i.amountId + '" data-amount="' + i.priceValue + '" data-amount-display="' + i.priceDisplay + '" data-amount-discount="' + i.discount + '">', r += '<span class="price">' + (i.isWideItem ? i.priceDisplay.replace("|", "<br/><small>") + "<\/small>" : i.priceDisplay) + "<\/span>", r += '<span class="coin">+ ' + i.coin + (i.coinMultiTimes > 1 ? "x" + i.coinMultiTimes : "") + (i.coinDiscount > 0 ? "+" + i.coinDiscount : "") + " koin<\/span>", r += "<\/div>", i.isDefault && $("#quick-topup-form").find("#amountId").val(i.amountId).trigger("change");
                r += '<div class="clearfix"><\/div>';
                t.fadeOut(100, function() {
                    t.html($(r));
                    t.fadeIn(100, function() {
                        loadTopupBank(1);
                        changeTopupPrice();
                        isGetPrice = !1
                    })
                })
            }
        }
    }).done(function() {})
}

function changeTopupPrice() {
    var n = $("#quick-topup-form"),
        t = n.find(".price-tag-item");
    t.on("click", function(i) {
        i.preventDefault();
        t.removeClass("active");
        $(this).addClass("active");
        n.find("#amountId").val($(this).attr("data-amount-id")).trigger("change")
    })
}

function loadTopupPrice() {
    var n = $("#topup_form_section"),
        t;
    if ($(n).find(".list-provider").length) {
        t = $(n).find(".list-provider").find(".provider-item");
        t.on("click", function(i) {
            i.preventDefault();
            t.removeClass("active");
            var r = $(this).attr("data-servicecode");
            n.find("#amountId").val(0);
            n.find("#topupProvider").val($(this).attr("data-provider"));
            n.find("#topupServiceCode").val(r);
            $(this).addClass("active");
            getTopupPrice(r)
        })
    } else getTopupPrice("")
}

function loadTopupBank(n) {
    var t = $("#quick-topup-form .bank_container"),
        i = apiPrefix + "paygates/listbank";
    $.ajax({
        url: i,
        type: "GET",
        headers: {
            "Content-Type": "application/x-www-form-urlencoded",
            Token: "FD4C26708337FEAC9E113D44EBD227F6",
            DataToken: "123",
            Flatform: "WEB"
        },
        xhrFields: {
            withCredentials: !0
        },
        data: {
            group: n
        },
        success: function(n) {
            var r = "",
                u, i;
            if (n.status) {
                for (u = 0; u < n.data.listBank.length; u++) i = n.data.listBank[u], r += '<a class="bank-icon an" href="" data-group="' + i.bankGroup + '" data-id="' + i.paymentId + '" data-icon="' + i.bankImage + '" data-name="' + i.bankName + '" data-full-name="' + i.bankFullName + '" data-code="' + i.bankCode + '" data-toggle="tooltip" data-placement="top" title="' + i.bankFullName + '">', r += '<img src="' + i.bankImage + '" alt="' + i.bankName + '">', r += "<\/a>";
                r += '<div class="clearfix"><\/div>';
                t.html($(r))
            }
        }
    }).done(function() {
        setPaymentGroup(n);
        getPaymentCookie();
        $('[data-toggle="tooltip"]').tooltip()
    })
}

function loadTopupForm(n) {
    var t = $("#topup_form_section");
    if (isLoadingForm == !0) return !1;
    isLoadingForm = !0;
    $.ajax({
        url: n,
        method: "GET",
        data: {},
        success: function(n) {
            t.fadeOut(100, function() {
                t.html(n);
                t.fadeIn(100, function() {
                    t.find(".form-container").fadeIn();
                    formLoaded();
                    isLoadingForm = !1
                })
            })
        }
    }).done(function() {})
}

function formLoaded() {
    var n = $("#topup_form_section"),
        t, i;
    $(n).find(".list-provider .provider-item.active").length ? (t = $(n).find(".list-provider").find(".provider-item.active").attr("data-servicecode"), i = $(n).find(".list-provider").find(".provider-item.active").attr("data-provider"), n.find("#topupServiceCode").val(t), n.find("#topupProvider").val(i), getTopupPrice(t)) : getTopupPrice("");
    loadTopupPrice();
    initTransaction()
}

function loadInitTopupPrice() {
    var t = 0,
        i = 0,
        r = 1,
        u = 0,
        n = $("#quick-topup-form");
    n.find("#amountId").length && (t = n.find("#amountId").val() != "0" ? n.find("#amountId").val() : 0);
    n.find("#paymentId").length && (i = n.find("#paymentId").val() != "0" ? n.find("#paymentId").val() : 0);
    n.find("#topupQuantity").length && (r = n.find("#topupQuantity").val() != "" ? n.find("#topupQuantity").val() : "1");
    n.find("#topupUserId").length && (u = n.find("#topupUserId").val() != "" ? n.find("#topupUserId").val() : "1");
    initTopupPrice(t, i, r, u)
}

function loadTopupPriceChange() {
    var n = $("#quick-topup-form");
    n.find("#amountId").on("change", function() {
        loadInitTopupPrice()
    });
    n.find("#paymentId").on("change", function() {
        loadInitTopupPrice()
    });
    n.find("#topupQuantity").on("change", function() {
        loadInitTopupPrice()
    });
    n.find("#Transaction_Account").on("change", function() {
        changePaymentInfo()
    })
}

function initTopupPrice(n, t, i, r) {
    var u = apiPrefix + "pricetopup/getprice";
    if (isGetPrice) return !1;
    isGetPrice = !0;
    $.ajax({
        url: u,
        type: "GET",
        headers: {
            "Content-Type": "application/x-www-form-urlencoded",
            Token: "FD4C26708337FEAC9E113D44EBD227F6",
            DataToken: "123",
            Flatform: "WEB"
        },
        xhrFields: {
            withCredentials: !0
        },
        data: {
            amountId: n == "" ? 0 : n,
            paymentId: t == "" ? 0 : t,
            quantity: i == "" ? 1 : i,
            userId: r == "" ? 0 : r
        },
        success: function(n) {
            n.status ? setAmountDisplay(n.meta.totalPrice) : setAmountDisplay(0)
        }
    }).done(function() {
        isGetPrice = !1;
        showHideTopupFee()
    })
}

function setAmountDisplay(n) {
    var t = $("#quick-topup-form");
    if (t.find("#topup_total_amount").text(format_number(n) + "đ"), t.find(".price-tag-item.active").length) {
        var u = t.find(".price-tag-item.active").attr("data-amount"),
            f = t.find(".price-tag-item.active").attr("data-koin"),
            i = t.find(".price-tag-item.active").attr("data-koin-multi"),
            e = t.find(".price-tag-item.active").attr("data-koin-discount"),
            r = t.find("#topupQuantity").val(),
            o = t.find("#topupUserId").val();
        i = parseInt(i) == 0 ? 1 : parseInt(i);
        t.find("#topup_amount").text(format_number(u) + "đ");
        o <= 0 ? t.find(".topup-koin .col-md-8").text("Đăng nhập để nhận được") : t.find(".topup-koin .col-md-8").text("Koin nhận được");
        t.find("#topup_koin").text(format_number(format_number(r >= 1 && r <= 50 ? parseInt(f) * parseInt(r) * parseInt(i) + parseInt(e) : 0)));
        changePaymentInfo()
    }
}

function changePaymentInfo() {
    var n = $("#quick-topup-form"),
        t;
    n.find("#topup_account").length && n.find("#topup_account").text(n.find("#Transaction_Account").val());
    n.find("#topup_quantity").length && (t = n.find("#topupQuantity").val(), n.find("#topup_quantity").text(t >= 1 && t <= 50 ? t : "Không hợp lệ"))
}

function changeBankButton() {
    $(".btn-change-payment").on("click", function() {
        removeCookiePayment();
        $("#payment-selected").fadeOut(0, function() {
            $("#visa").is(":checked") ? loadTopupBank(2) : loadTopupBank(1)
        })
    })
}

function changeBank(n, t, i, r) {
    var u = $("#payment-selected");
    u.find(".payment-selected-image").css({
        "background-image": 'url("' + n + '")'
    });
    u.find(".payment-selected-name").html(i);
    u.find(".payment-selected-fullname").html(r);
    setPaymentId(t)
}

function setPaymentId(n) {
    $("#paymentId").val(n).trigger("change")
}

function triggerPaymentMethod() {
    $("#visa").unbind().on("change", function() {
        $(this).is(":checked") && (loadTopupBank(2), hideSelectedBank(), removeCookiePayment())
    });
    $("#ebank").unbind().on("change", function() {
        $(this).is(":checked") && (loadTopupBank(1), removeCookiePayment(), hideSelectedBank())
    })
}

function setPaymentGroup(n) {
    $.cookie("_pmg", n, {
        expires: 60
    })
}

function getPaymentGroup() {
    return $.cookie("_pmg")
}

function selectBank() {
    $("#list-bank .bank-icon").on("click", function(n) {
        var t, i, r;
        n.preventDefault();
        t = $(this);
        changeBank(t.attr("data-icon"), t.attr("data-id"), t.attr("data-name"), t.attr("data-full-name"));
        i = t.attr("data-group");
        r = i === "2" ? "visa" : "ebank";
        setPaymentCookie(t.attr("data-id"), r, t.attr("data-icon"), t.attr("data-name"), t.attr("data-full-name"));
        $("#list-bank").fadeOut(200, function() {
            showSelectedBank()
        })
    })
}

function showSelectedBank() {
    $("#payment-selected").fadeIn(200, function() {
        changeBankButton()
    })
}

function showListBank() {
    $("#list-bank").fadeIn(300, function() {
        selectBank()
    })
}

function hideSelectedBank() {
    $("#payment-selected").fadeOut()
}

function hideListBank() {
    $("#list-bank").fadeOut()
}

function setPaymentCookie(n, t, i, r, u) {
    var f = {
            id: n,
            method: t,
            icon: i,
            name: r,
            fullname: u
        },
        e = JSON.stringify(f);
    $.cookie.json = !0;
    $.cookie(pmiKey, e, {
        expires: 60
    })
}

function getPaymentCookie() {
    var t, n, i;
    $.cookie.json = !0;
    t = $.cookie(pmiKey);
    n = {};
    t && t !== "undefined" ? (n = JSON.parse(t), n.method === "visa" ? (setPaymentId(n.id), changeBank(n.icon, n.id, n.name, n.fullname), setPaymentGroup(2), showSelectedBank(), $("#visa").prop("checked", !0)) : (setPaymentId(n.id), changeBank(n.icon, n.id, n.name, n.fullname), setPaymentGroup(1), showSelectedBank())) : showListBank();
    triggerPaymentMethod();
    loadTopupPriceChange();
    loadInitTopupPrice();
    i = getPaymentGroup();
    i == 2 ? showCustomerInfo() : hideCustomerInfo()
}

function removeCookiePayment() {
    $.removeCookie(pmiKey);
    setPaymentId("")
}

function showHideTopupFee() {
    var r = $("#quick-topup-form"),
        t = r.find(".fee-payment"),
        n, i;
    $.cookie.json = !0;
    n = $.cookie(pmiKey);
    i = {};
    n && n !== "undefined" ? (i = JSON.parse(n), i.method === "visa" ? t.hide(300) : t.show(300)) : t.show(300)
}

function showModal(n) {
    if (checkModalIsShow(n)) return !1;
    n.appendTo("body").modal({
        backdrop: "static",
        keyboard: !1
    });
    n.modal("show")
}

function setCustomerInfoCookie(n) {
    var t = JSON.stringify(n);
    $.cookie.json = !0;
    $.cookie(ctmKey, t, {
        expires: 60
    })
}

function getCustomerInfoCookie() {
    $.cookie.json = !0;
    var n = $.cookie(ctmKey);
    return n && n !== "undefined" ? JSON.parse(n) : null
}

function showCustomerInfo() {
    var t = $("#customerInfo"),
        n = getCustomerInfoCookie();
    n == null || n.name == "" || n.email == "" || n.phone == "" ? showCustomerInfoForm() : showCustomerInfoDisplay();
    triggerCustomerInfo();
    t.css("display") == "none" && t.fadeIn(0)
}

function hideCustomerInfo() {
    $("#customerInfo").css("display") != "none" && $("#customerInfo").fadeOut(0)
}

function showCustomerInfoForm() {
    var n = $("#customerInfo"),
        t;
    n.find(".customerInfoDisplay").css("display") != "none" && n.find(".customerInfoDisplay").fadeOut(0);
    t = getCustomerInfoCookie();
    t != null && ($(n).find("#customerInfoName").val(t.name), $(n).find("#customerInfoEmail").val(t.email), $(n).find("#customerInfoPhone").val(t.phone));
    n.find(".customerInfoForm").fadeIn(0)
}

function showCustomerInfoDisplay() {
    var n = $("#customerInfo"),
        t = getCustomerInfoCookie();
    t != null && (n.find("#customerInfoName").val(t.name), n.find("#customerInfoEmail").val(t.email), n.find("#customerInfoPhone").val(t.phone), n.find("#customerInfoDisplayName").text(t.name), n.find("#customerInfoDisplayEmail").text(t.email), n.find("#customerInfoDisplayPhone").text(t.phone));
    n.find(".customerInfoForm").css("display") != "none" && n.find(".customerInfoForm").fadeOut(0);
    n.find(".customerInfoDisplay").fadeIn(0)
}

function saveCustomerInfo() {
    $("#saveCustomerInfo").unbind("click").on("click", function(n) {
        n.preventDefault();
        var t = getCustomerInfoToSave();
        return t != null && showCustomerInfoDisplay(), !1
    })
}

function getCustomerInfoToSave() {
    var r = $("#customerInfoName").val(),
        i = $("#customerInfoEmail").val(),
        u = $("#customerInfoPhone").val(),
        n = "",
        t;
    return (r == "" && (n += "- <b>Họ và tên<\/b> không được để trống<br/>"), i == "" && (n += "- <b>Địa chỉ Email<\/b> không được để trống.<br/>"), i == "" || isValidEmail(i) || (n += "- <b>Địa chỉ Email<\/b> không hợp lệ.<br/>"), u == "" && (n += "- <b>Số điện thoại<\/b> không được để trống.<br/>"), n != "") ? (showMessage("Thông tin thanh toán", n, "danger"), null) : (t = {
        name: r,
        email: i,
        phone: u
    }, setCustomerInfoCookie(t), {
        fullName: t.name,
        email: t.email,
        phone: t.phone,
        ip: "",
        address: ""
    })
}

function changeCustomerInfo() {
    $("#changeCustomerInfo").unbind("click").on("click", function(n) {
        n.preventDefault();
        showCustomerInfoForm()
    })
}

function clearCustomerInfo() {
    $("#clearCustomerInfo").unbind("click").on("click", function(n) {
        n.preventDefault();
        showConfirm("Xác nhận", "Bạn có chắc muốn xóa thông tin xác thực thẻ thanh toán quốc tế ?", function() {
            $("#customerInfoName").val("");
            $("#customerInfoEmail").val("");
            $("#customerInfoPhone").val("");
            $.removeCookie("_ctm")
        })
    })
}

function triggerCustomerInfo() {
    saveCustomerInfo();
    changeCustomerInfo();
    clearCustomerInfo()
}
// var jconfirm, Jconfirm, isSubmiting;
// if (! function(n, t) {
//     "use strict";
//     "object" == typeof module && "object" == typeof module.exports ? module.exports = n.document ? t(n, !0) : function(n) {
//         if (!n.document) throw new Error("jQuery requires a window with a document");
//         return t(n)
//     } : t(n)
// }


// ("undefined" != typeof window ? window : this, function(n, t) {
//     "use strict";

//     function gi(n, t) {
//         t = t || u;
//         var i = t.createElement("script");
//         i.text = n;
//         t.head.appendChild(i).parentNode.removeChild(i)
//     }

//     function ui(n) {
//         var t = !!n && "length" in n && n.length,
//             r = i.type(n);
//         return "function" !== r && !i.isWindow(n) && ("array" === r || 0 === t || "number" == typeof t && t > 0 && t - 1 in n)
//     }

//     function fi(n, t, r) {
//         if (i.isFunction(t)) return i.grep(n, function(n, i) {
//             return !!t.call(n, i, n) !== r
//         });
//         if (t.nodeType) return i.grep(n, function(n) {
//             return n === t !== r
//         });
//         if ("string" == typeof t) {
//             if (gf.test(t)) return i.filter(t, n, r);
//             t = i.filter(t, n)
//         }
//         return i.grep(n, function(n) {
//             return lt.call(t, n) > -1 !== r && 1 === n.nodeType
//         })
//     }

//     function hr(n, t) {
//         while ((n = n[t]) && 1 !== n.nodeType);
//         return n
//     }

//     function ne(n) {
//         var t = {};
//         return i.each(n.match(h) || [], function(n, i) {
//             t[i] = !0
//         }), t
//     }

//     function d(n) {
//         return n
//     }

//     function yt(n) {
//         throw n;
//     }

//     function cr(n, t, r) {
//         var u;
//         try {
//             n && i.isFunction(u = n.promise) ? u.call(n).done(t).fail(r) : n && i.isFunction(u = n.then) ? u.call(n, t, r) : t.call(void 0, n)
//         } catch (n) {
//             r.call(void 0, n)
//         }
//     }

//     function wt() {
//         u.removeEventListener("DOMContentLoaded", wt);
//         n.removeEventListener("load", wt);
//         i.ready()
//     }

//     function ot() {
//         this.expando = i.expando + ot.uid++
//     }

//     function ar(n, t, i) {
//         var r;
//         if (void 0 === i && 1 === n.nodeType)
//             if (r = "data-" + t.replace(ie, "-$&").toLowerCase(), i = n.getAttribute(r), "string" == typeof i) {
//                 try {
//                     i = "true" === i || "false" !== i && ("null" === i ? null : +i + "" === i ? +i : te.test(i) ? JSON.parse(i) : i)
//                 } catch (u) {}
//                 e.set(n, t, i)
//             } else i = void 0;
//         return i
//     }

//     function pr(n, t, r, u) {
//         var h, e = 1,
//             l = 20,
//             c = u ? function() {
//                 return u.cur()
//             } : function() {
//                 return i.css(n, t, "")
//             },
//             s = c(),
//             o = r && r[3] || (i.cssNumber[t] ? "" : "px"),
//             f = (i.cssNumber[t] || "px" !== o && +s) && st.exec(i.css(n, t));
//         if (f && f[3] !== o) {
//             o = o || f[3];
//             r = r || [];
//             f = +s || 1;
//             do e = e || ".5", f /= e, i.style(n, t, f + o); while (e !== (e = c() / s) && 1 !== e && --l)
//         }
//         return r && (f = +f || +s || 0, h = r[1] ? f + (r[1] + 1) * r[2] : +r[2], u && (u.unit = o, u.start = f, u.end = h)), h
//     }

//     function re(n) {
//         var r, f = n.ownerDocument,
//             u = n.nodeName,
//             t = ei[u];
//         return t ? t : (r = f.body.appendChild(f.createElement(u)), t = i.css(r, "display"), r.parentNode.removeChild(r), "none" === t && (t = "block"), ei[u] = t, t)
//     }

//     function g(n, t) {
//         for (var e, u, f = [], i = 0, o = n.length; i < o; i++) u = n[i], u.style && (e = u.style.display, t ? ("none" === e && (f[i] = r.get(u, "display") || null, f[i] || (u.style.display = "")), "" === u.style.display && bt(u) && (f[i] = re(u))) : "none" !== e && (f[i] = "none", r.set(u, "display", e)));
//         for (i = 0; i < o; i++) null != f[i] && (n[i].style.display = f[i]);
//         return n
//     }

//     function o(n, t) {
//         var r = "undefined" != typeof n.getElementsByTagName ? n.getElementsByTagName(t || "*") : "undefined" != typeof n.querySelectorAll ? n.querySelectorAll(t || "*") : [];
//         return void 0 === t || t && i.nodeName(n, t) ? i.merge([n], r) : r
//     }

//     function oi(n, t) {
//         for (var i = 0, u = n.length; i < u; i++) r.set(n[i], "globalEval", !t || r.get(t[i], "globalEval"))
//     }

//     function gr(n, t, r, u, f) {
//         for (var e, s, p, a, w, v, h = t.createDocumentFragment(), y = [], l = 0, b = n.length; l < b; l++)
//             if (e = n[l], e || 0 === e)
//                 if ("object" === i.type(e)) i.merge(y, e.nodeType ? [e] : e);
//                 else if (dr.test(e)) {
//                     for (s = s || h.appendChild(t.createElement("div")), p = (br.exec(e) || ["", ""])[1].toLowerCase(), a = c[p] || c._default, s.innerHTML = a[1] + i.htmlPrefilter(e) + a[2], v = a[0]; v--;) s = s.lastChild;
//                     i.merge(y, s.childNodes);
//                     s = h.firstChild;
//                     s.textContent = ""
//                 } else y.push(t.createTextNode(e));
//         for (h.textContent = "", l = 0; e = y[l++];)
//             if (u && i.inArray(e, u) > -1) f && f.push(e);
//             else if (w = i.contains(e.ownerDocument, e), s = o(h.appendChild(e), "script"), w && oi(s), r)
//                 for (v = 0; e = s[v++];) kr.test(e.type || "") && r.push(e);
//         return h
//     }

//     function dt() {
//         return !0
//     }

//     function nt() {
//         return !1
//     }

//     function tu() {
//         try {
//             return u.activeElement
//         } catch (n) {}
//     }

//     function si(n, t, r, u, f, e) {
//         var o, s;
//         if ("object" == typeof t) {
//             "string" != typeof r && (u = u || r, r = void 0);
//             for (s in t) si(n, s, r, u, t[s], e);
//             return n
//         }
//         if (null == u && null == f ? (f = r, u = r = void 0) : null == f && ("string" == typeof r ? (f = u, u = void 0) : (f = u, u = r, r = void 0)), f === !1) f = nt;
//         else if (!f) return n;
//         return 1 === e && (o = f, f = function(n) {
//             return i().off(n), o.apply(this, arguments)
//         }, f.guid = o.guid || (o.guid = i.guid++)), n.each(function() {
//             i.event.add(this, t, f, u, r)
//         })
//     }

//     function iu(n, t) {
//         return i.nodeName(n, "table") && i.nodeName(11 !== t.nodeType ? t : t.firstChild, "tr") ? n.getElementsByTagName("tbody")[0] || n : n
//     }

//     function le(n) {
//         return n.type = (null !== n.getAttribute("type")) + "/" + n.type, n
//     }

//     function ae(n) {
//         var t = he.exec(n.type);
//         return t ? n.type = t[1] : n.removeAttribute("type"), n
//     }

//     function ru(n, t) {
//         var u, c, f, s, h, l, a, o;
//         if (1 === t.nodeType) {
//             if (r.hasData(n) && (s = r.access(n), h = r.set(t, s), o = s.events)) {
//                 delete h.handle;
//                 h.events = {};
//                 for (f in o)
//                     for (u = 0, c = o[f].length; u < c; u++) i.event.add(t, f, o[f][u])
//             }
//             e.hasData(n) && (l = e.access(n), a = i.extend({}, l), e.set(t, a))
//         }
//     }

//     function ve(n, t) {
//         var i = t.nodeName.toLowerCase();
//         "input" === i && wr.test(n.type) ? t.checked = n.checked : "input" !== i && "textarea" !== i || (t.defaultValue = n.defaultValue)
//     }

//     function tt(n, t, u, e) {
//         t = bi.apply([], t);
//         var l, p, c, a, s, w, h = 0,
//             v = n.length,
//             k = v - 1,
//             y = t[0],
//             b = i.isFunction(y);
//         if (b || v > 1 && "string" == typeof y && !f.checkClone && se.test(y)) return n.each(function(i) {
//             var r = n.eq(i);
//             b && (t[0] = y.call(this, i, r.html()));
//             tt(r, t, u, e)
//         });
//         if (v && (l = gr(t, n[0].ownerDocument, !1, n, e), p = l.firstChild, 1 === l.childNodes.length && (l = p), p || e)) {
//             for (c = i.map(o(l, "script"), le), a = c.length; h < v; h++) s = l, h !== k && (s = i.clone(s, !0, !0), a && i.merge(c, o(s, "script"))), u.call(n[h], s, h);
//             if (a)
//                 for (w = c[c.length - 1].ownerDocument, i.map(c, ae), h = 0; h < a; h++) s = c[h], kr.test(s.type || "") && !r.access(s, "globalEval") && i.contains(w, s) && (s.src ? i._evalUrl && i._evalUrl(s.src) : gi(s.textContent.replace(ce, ""), w))
//         }
//         return n
//     }

//     function uu(n, t, r) {
//         for (var u, e = t ? i.filter(t, n) : n, f = 0; null != (u = e[f]); f++) r || 1 !== u.nodeType || i.cleanData(o(u)), u.parentNode && (r && i.contains(u.ownerDocument, u) && oi(o(u, "script")), u.parentNode.removeChild(u));
//         return n
//     }

//     function ht(n, t, r) {
//         var o, s, h, u, e = n.style;
//         return r = r || gt(n), r && (u = r.getPropertyValue(t) || r[t], "" !== u || i.contains(n.ownerDocument, n) || (u = i.style(n, t)), !f.pixelMarginRight() && hi.test(u) && fu.test(t) && (o = e.width, s = e.minWidth, h = e.maxWidth, e.minWidth = e.maxWidth = e.width = u, u = r.width, e.width = o, e.minWidth = s, e.maxWidth = h)), void 0 !== u ? u + "" : u
//     }

//     function eu(n, t) {
//         return {
//             get: function() {
//                 return n() ? void delete this.get : (this.get = t).apply(this, arguments)
//             }
//         }
//     }

//     function cu(n) {
//         if (n in hu) return n;
//         for (var i = n[0].toUpperCase() + n.slice(1), t = su.length; t--;)
//             if (n = su[t] + i, n in hu) return n
//     }

//     function lu(n, t, i) {
//         var r = st.exec(t);
//         return r ? Math.max(0, r[2] - (i || 0)) + (r[3] || "px") : t
//     }

//     function au(n, t, r, u, f) {
//         for (var e = r === (u ? "border" : "content") ? 4 : "width" === t ? 1 : 0, o = 0; e < 4; e += 2) "margin" === r && (o += i.css(n, r + w[e], !0, f)), u ? ("content" === r && (o -= i.css(n, "padding" + w[e], !0, f)), "margin" !== r && (o -= i.css(n, "border" + w[e] + "Width", !0, f))) : (o += i.css(n, "padding" + w[e], !0, f), "padding" !== r && (o += i.css(n, "border" + w[e] + "Width", !0, f)));
//         return o
//     }

//     function vu(n, t, r) {
//         var u, o = !0,
//             e = gt(n),
//             s = "border-box" === i.css(n, "boxSizing", !1, e);
//         if (n.getClientRects().length && (u = n.getBoundingClientRect()[t]), u <= 0 || null == u) {
//             if (u = ht(n, t, e), (u < 0 || null == u) && (u = n.style[t]), hi.test(u)) return u;
//             o = s && (f.boxSizingReliable() || u === n.style[t]);
//             u = parseFloat(u) || 0
//         }
//         return u + au(n, t, r || (s ? "border" : "content"), o, e) + "px"
//     }

//     function s(n, t, i, r, u) {
//         return new s.prototype.init(n, t, i, r, u)
//     }

//     function wu() {
//         rt && (n.requestAnimationFrame(wu), i.fx.tick())
//     }

//     function bu() {
//         return n.setTimeout(function() {
//             it = void 0
//         }), it = i.now()
//     }

//     function ni(n, t) {
//         var r, u = 0,
//             i = {
//                 height: n
//             };
//         for (t = t ? 1 : 0; u < 4; u += 2 - t) r = w[u], i["margin" + r] = i["padding" + r] = n;
//         return t && (i.opacity = i.width = n), i
//     }

//     function ku(n, t, i) {
//         for (var u, f = (l.tweeners[t] || []).concat(l.tweeners["*"]), r = 0, e = f.length; r < e; r++)
//             if (u = f[r].call(i, t, n)) return u
//     }

//     function we(n, t, u) {
//         var f, y, w, c, b, s, o, l, k = "width" in t || "height" in t,
//             v = this,
//             p = {},
//             h = n.style,
//             a = n.nodeType && bt(n),
//             e = r.get(n, "fxshow");
//         u.queue || (c = i._queueHooks(n, "fx"), null == c.unqueued && (c.unqueued = 0, b = c.empty.fire, c.empty.fire = function() {
//             c.unqueued || b()
//         }), c.unqueued++, v.always(function() {
//             v.always(function() {
//                 c.unqueued--;
//                 i.queue(n, "fx").length || c.empty.fire()
//             })
//         }));
//         for (f in t)
//             if (y = t[f], yu.test(y)) {
//                 if (delete t[f], w = w || "toggle" === y, y === (a ? "hide" : "show")) {
//                     if ("show" !== y || !e || void 0 === e[f]) continue;
//                     a = !0
//                 }
//                 p[f] = e && e[f] || i.style(n, f)
//             } if (s = !i.isEmptyObject(t), s || !i.isEmptyObject(p)) {
//             k && 1 === n.nodeType && (u.overflow = [h.overflow, h.overflowX, h.overflowY], o = e && e.display, null == o && (o = r.get(n, "display")), l = i.css(n, "display"), "none" === l && (o ? l = o : (g([n], !0), o = n.style.display || o, l = i.css(n, "display"), g([n]))), ("inline" === l || "inline-block" === l && null != o) && "none" === i.css(n, "float") && (s || (v.done(function() {
//                 h.display = o
//             }), null == o && (l = h.display, o = "none" === l ? "" : l)), h.display = "inline-block"));
//             u.overflow && (h.overflow = "hidden", v.always(function() {
//                 h.overflow = u.overflow[0];
//                 h.overflowX = u.overflow[1];
//                 h.overflowY = u.overflow[2]
//             }));
//             s = !1;
//             for (f in p) s || (e ? "hidden" in e && (a = e.hidden) : e = r.access(n, "fxshow", {
//                 display: o
//             }), w && (e.hidden = !a), a && g([n], !0), v.done(function() {
//                 a || g([n]);
//                 r.remove(n, "fxshow");
//                 for (f in p) i.style(n, f, p[f])
//             })), s = ku(a ? e[f] : 0, f, v), f in e || (e[f] = s.start, a && (s.end = s.start, s.start = 0))
//         }
//     }

//     function be(n, t) {
//         var r, f, e, u, o;
//         for (r in n)
//             if (f = i.camelCase(r), e = t[f], u = n[r], i.isArray(u) && (e = u[1], u = n[r] = u[0]), r !== f && (n[f] = u, delete n[r]), o = i.cssHooks[f], o && "expand" in o) {
//                 u = o.expand(u);
//                 delete n[f];
//                 for (r in u) r in n || (n[r] = u[r], t[r] = e)
//             } else t[f] = e
//     }

//     function l(n, t, r) {
//         var e, o, s = 0,
//             a = l.prefilters.length,
//             f = i.Deferred().always(function() {
//                 delete c.elem
//             }),
//             c = function() {
//                 if (o) return !1;
//                 for (var s = it || bu(), t = Math.max(0, u.startTime + u.duration - s), h = t / u.duration || 0, i = 1 - h, r = 0, e = u.tweens.length; r < e; r++) u.tweens[r].run(i);
//                 return f.notifyWith(n, [u, i, t]), i < 1 && e ? t : (f.resolveWith(n, [u]), !1)
//             },
//             u = f.promise({
//                 elem: n,
//                 props: i.extend({}, t),
//                 opts: i.extend(!0, {
//                     specialEasing: {},
//                     easing: i.easing._default
//                 }, r),
//                 originalProperties: t,
//                 originalOptions: r,
//                 startTime: it || bu(),
//                 duration: r.duration,
//                 tweens: [],
//                 createTween: function(t, r) {
//                     var f = i.Tween(n, u.opts, t, r, u.opts.specialEasing[t] || u.opts.easing);
//                     return u.tweens.push(f), f
//                 },
//                 stop: function(t) {
//                     var i = 0,
//                         r = t ? u.tweens.length : 0;
//                     if (o) return this;
//                     for (o = !0; i < r; i++) u.tweens[i].run(1);
//                     return t ? (f.notifyWith(n, [u, 1, 0]), f.resolveWith(n, [u, t])) : f.rejectWith(n, [u, t]), this
//                 }
//             }),
//             h = u.props;
//         for (be(h, u.opts.specialEasing); s < a; s++)
//             if (e = l.prefilters[s].call(u, n, h, u.opts)) return i.isFunction(e.stop) && (i._queueHooks(u.elem, u.opts.queue).stop = i.proxy(e.stop, e)), e;
//         return i.map(h, ku, u), i.isFunction(u.opts.start) && u.opts.start.call(n, u), i.fx.timer(i.extend(c, {
//             elem: n,
//             anim: u,
//             queue: u.opts.queue
//         })), u.progress(u.opts.progress).done(u.opts.done, u.opts.complete).fail(u.opts.fail).always(u.opts.always)
//     }

//     function b(n) {
//         return n.getAttribute && n.getAttribute("class") || ""
//     }

//     function ai(n, t, r, u) {
//         var f;
//         if (i.isArray(t)) i.each(t, function(t, i) {
//             r || ke.test(n) ? u(n, i) : ai(n + "[" + ("object" == typeof i && null != i ? t : "") + "]", i, r, u)
//         });
//         else if (r || "object" !== i.type(t)) u(n, t);
//         else
//             for (f in t) ai(n + "[" + f + "]", t[f], r, u)
//     }

//     function sf(n) {
//         return function(t, r) {
//             "string" != typeof t && (r = t, t = "*");
//             var u, f = 0,
//                 e = t.toLowerCase().match(h) || [];
//             if (i.isFunction(r))
//                 while (u = e[f++]) "+" === u[0] ? (u = u.slice(1) || "*", (n[u] = n[u] || []).unshift(r)) : (n[u] = n[u] || []).push(r)
//         }
//     }

//     function hf(n, t, r, u) {
//         function e(s) {
//             var h;
//             return f[s] = !0, i.each(n[s] || [], function(n, i) {
//                 var s = i(t, r, u);
//                 return "string" != typeof s || o || f[s] ? o ? !(h = s) : void 0 : (t.dataTypes.unshift(s), e(s), !1)
//             }), h
//         }
//         var f = {},
//             o = n === vi;
//         return e(t.dataTypes[0]) || !f["*"] && e("*")
//     }

//     function pi(n, t) {
//         var r, u, f = i.ajaxSettings.flatOptions || {};
//         for (r in t) void 0 !== t[r] && ((f[r] ? n : u || (u = {}))[r] = t[r]);
//         return u && i.extend(!0, n, u), n
//     }

//     function eo(n, t, i) {
//         for (var e, u, f, o, s = n.contents, r = n.dataTypes;
//              "*" === r[0];) r.shift(), void 0 === e && (e = n.mimeType || t.getResponseHeader("Content-Type"));
//         if (e)
//             for (u in s)
//                 if (s[u] && s[u].test(e)) {
//                     r.unshift(u);
//                     break
//                 } if (r[0] in i) f = r[0];
//         else {
//             for (u in i) {
//                 if (!r[0] || n.converters[u + " " + r[0]]) {
//                     f = u;
//                     break
//                 }
//                 o || (o = u)
//             }
//             f = f || o
//         }
//         if (f) return f !== r[0] && r.unshift(f), i[f]
//     }

//     function oo(n, t, i, r) {
//         var h, u, f, s, e, o = {},
//             c = n.dataTypes.slice();
//         if (c[1])
//             for (f in n.converters) o[f.toLowerCase()] = n.converters[f];
//         for (u = c.shift(); u;)
//             if (n.responseFields[u] && (i[n.responseFields[u]] = t), !e && r && n.dataFilter && (t = n.dataFilter(t, n.dataType)), e = u, u = c.shift())
//                 if ("*" === u) u = e;
//                 else if ("*" !== e && e !== u) {
//                     if (f = o[e + " " + u] || o["* " + u], !f)
//                         for (h in o)
//                             if (s = h.split(" "), s[1] === u && (f = o[e + " " + s[0]] || o["* " + s[0]])) {
//                                 f === !0 ? f = o[h] : o[h] !== !0 && (u = s[0], c.unshift(s[1]));
//                                 break
//                             } if (f !== !0)
//                         if (f && n.throws) t = f(t);
//                         else try {
//                             t = f(t)
//                         } catch (l) {
//                             return {
//                                 state: "parsererror",
//                                 error: f ? l : "No conversion from " + e + " to " + u
//                             }
//                         }
//                 }
//         return {
//             state: "success",
//             data: t
//         }
//     }

//     function lf(n) {
//         return i.isWindow(n) ? n : 9 === n.nodeType && n.defaultView
//     }
//     var y = [],
//         u = n.document,
//         yf = Object.getPrototypeOf,
//         p = y.slice,
//         bi = y.concat,
//         ri = y.push,
//         lt = y.indexOf,
//         at = {},
//         ki = at.toString,
//         vt = at.hasOwnProperty,
//         di = vt.toString,
//         pf = di.call(Object),
//         f = {},
//         nr = "3.1.0",
//         i = function(n, t) {
//             return new i.fn.init(n, t)
//         },
//         wf = /^[\s\uFEFF\xA0]+|[\s\uFEFF\xA0]+$/g,
//         bf = /^-ms-/,
//         kf = /-([a-z])/g,
//         df = function(n, t) {
//             return t.toUpperCase()
//         },
//         v, ur, fr, er, or, sr, h, lr, pt, a, et, ei, dr, it, rt, yu, pu, du, ut, gu, nf, ti, tf, rf, ci, cf, ft, wi, ii, af, vf;
//     i.fn = i.prototype = {
//         jquery: nr,
//         constructor: i,
//         length: 0,
//         toArray: function() {
//             return p.call(this)
//         },
//         get: function(n) {
//             return null != n ? n < 0 ? this[n + this.length] : this[n] : p.call(this)
//         },
//         pushStack: function(n) {
//             var t = i.merge(this.constructor(), n);
//             return t.prevObject = this, t
//         },
//         each: function(n) {
//             return i.each(this, n)
//         },
//         map: function(n) {
//             return this.pushStack(i.map(this, function(t, i) {
//                 return n.call(t, i, t)
//             }))
//         },
//         slice: function() {
//             return this.pushStack(p.apply(this, arguments))
//         },
//         first: function() {
//             return this.eq(0)
//         },
//         last: function() {
//             return this.eq(-1)
//         },
//         eq: function(n) {
//             var i = this.length,
//                 t = +n + (n < 0 ? i : 0);
//             return this.pushStack(t >= 0 && t < i ? [this[t]] : [])
//         },
//         end: function() {
//             return this.prevObject || this.constructor()
//         },
//         push: ri,
//         sort: y.sort,
//         splice: y.splice
//     };
//     i.extend = i.fn.extend = function() {
//         var e, f, r, t, o, s, n = arguments[0] || {},
//             u = 1,
//             c = arguments.length,
//             h = !1;
//         for ("boolean" == typeof n && (h = n, n = arguments[u] || {}, u++), "object" == typeof n || i.isFunction(n) || (n = {}), u === c && (n = this, u--); u < c; u++)
//             if (null != (e = arguments[u]))
//                 for (f in e) r = n[f], t = e[f], n !== t && (h && t && (i.isPlainObject(t) || (o = i.isArray(t))) ? (o ? (o = !1, s = r && i.isArray(r) ? r : []) : s = r && i.isPlainObject(r) ? r : {}, n[f] = i.extend(h, s, t)) : void 0 !== t && (n[f] = t));
//         return n
//     };
//     i.extend({
//         expando: "jQuery" + (nr + Math.random()).replace(/\D/g, ""),
//         isReady: !0,
//         error: function(n) {
//             throw new Error(n);
//         },
//         noop: function() {},
//         isFunction: function(n) {
//             return "function" === i.type(n)
//         },
//         isArray: Array.isArray,
//         isWindow: function(n) {
//             return null != n && n === n.window
//         },
//         isNumeric: function(n) {
//             var t = i.type(n);
//             return ("number" === t || "string" === t) && !isNaN(n - parseFloat(n))
//         },
//         isPlainObject: function(n) {
//             var t, i;
//             return !(!n || "[object Object]" !== ki.call(n)) && (!(t = yf(n)) || (i = vt.call(t, "constructor") && t.constructor, "function" == typeof i && di.call(i) === pf))
//         },
//         isEmptyObject: function(n) {
//             var t;
//             for (t in n) return !1;
//             return !0
//         },
//         type: function(n) {
//             return null == n ? n + "" : "object" == typeof n || "function" == typeof n ? at[ki.call(n)] || "object" : typeof n
//         },
//         globalEval: function(n) {
//             gi(n)
//         },
//         camelCase: function(n) {
//             return n.replace(bf, "ms-").replace(kf, df)
//         },
//         nodeName: function(n, t) {
//             return n.nodeName && n.nodeName.toLowerCase() === t.toLowerCase()
//         },
//         each: function(n, t) {
//             var r, i = 0;
//             if (ui(n)) {
//                 for (r = n.length; i < r; i++)
//                     if (t.call(n[i], i, n[i]) === !1) break
//             } else
//                 for (i in n)
//                     if (t.call(n[i], i, n[i]) === !1) break;
//             return n
//         },
//         trim: function(n) {
//             return null == n ? "" : (n + "").replace(wf, "")
//         },
//         makeArray: function(n, t) {
//             var r = t || [];
//             return null != n && (ui(Object(n)) ? i.merge(r, "string" == typeof n ? [n] : n) : ri.call(r, n)), r
//         },
//         inArray: function(n, t, i) {
//             return null == t ? -1 : lt.call(t, n, i)
//         },
//         merge: function(n, t) {
//             for (var u = +t.length, i = 0, r = n.length; i < u; i++) n[r++] = t[i];
//             return n.length = r, n
//         },
//         grep: function(n, t, i) {
//             for (var u, f = [], r = 0, e = n.length, o = !i; r < e; r++) u = !t(n[r], r), u !== o && f.push(n[r]);
//             return f
//         },
//         map: function(n, t, i) {
//             var e, u, r = 0,
//                 f = [];
//             if (ui(n))
//                 for (e = n.length; r < e; r++) u = t(n[r], r, i), null != u && f.push(u);
//             else
//                 for (r in n) u = t(n[r], r, i), null != u && f.push(u);
//             return bi.apply([], f)
//         },
//         guid: 1,
//         proxy: function(n, t) {
//             var u, f, r;
//             if ("string" == typeof t && (u = n[t], t = n, n = u), i.isFunction(n)) return f = p.call(arguments, 2), r = function() {
//                 return n.apply(t || this, f.concat(p.call(arguments)))
//             }, r.guid = n.guid = n.guid || i.guid++, r
//         },
//         now: Date.now,
//         support: f
//     });
//     "function" == typeof Symbol && (i.fn[Symbol.iterator] = y[Symbol.iterator]);
//     i.each("Boolean Number String Function Array Date RegExp Object Error Symbol".split(" "), function(n, t) {
//         at["[object " + t + "]"] = t.toLowerCase()
//     });
//     v = function(n) {
//         function u(n, t, r, u) {
//             var s, w, l, a, d, y, g, p = t && t.ownerDocument,
//                 v = t ? t.nodeType : 9;
//             if (r = r || [], "string" != typeof n || !n || 1 !== v && 9 !== v && 11 !== v) return r;
//             if (!u && ((t ? t.ownerDocument || t : h) !== i && b(t), t = t || i, c)) {
//                 if (11 !== v && (d = cr.exec(n)))
//                     if (s = d[1]) {
//                         if (9 === v) {
//                             if (!(l = t.getElementById(s))) return r;
//                             if (l.id === s) return r.push(l), r
//                         } else if (p && (l = p.getElementById(s)) && et(t, l) && l.id === s) return r.push(l), r
//                     } else {
//                         if (d[2]) return k.apply(r, t.getElementsByTagName(n)), r;
//                         if ((s = d[3]) && f.getElementsByClassName && t.getElementsByClassName) return k.apply(r, t.getElementsByClassName(s)), r
//                     } if (f.qsa && !lt[n + " "] && (!o || !o.test(n))) {
//                     if (1 !== v) p = t, g = n;
//                     else if ("object" !== t.nodeName.toLowerCase()) {
//                         for ((a = t.getAttribute("id")) ? a = a.replace(vi, yi) : t.setAttribute("id", a = e), y = ft(n), w = y.length; w--;) y[w] = "#" + a + " " + yt(y[w]);
//                         g = y.join(",");
//                         p = ni.test(n) && ri(t.parentNode) || t
//                     }
//                     if (g) try {
//                         return k.apply(r, p.querySelectorAll(g)), r
//                     } catch (nt) {} finally {
//                         a === e && t.removeAttribute("id")
//                     }
//                 }
//             }
//             return si(n.replace(at, "$1"), t, r, u)
//         }

//         function ti() {
//             function n(r, u) {
//                 return i.push(r + " ") > t.cacheLength && delete n[i.shift()], n[r + " "] = u
//             }
//             var i = [];
//             return n
//         }

//         function l(n) {
//             return n[e] = !0, n
//         }

//         function a(n) {
//             var t = i.createElement("fieldset");
//             try {
//                 return !!n(t)
//             } catch (r) {
//                 return !1
//             } finally {
//                 t.parentNode && t.parentNode.removeChild(t);
//                 t = null
//             }
//         }

//         function ii(n, i) {
//             for (var r = n.split("|"), u = r.length; u--;) t.attrHandle[r[u]] = i
//         }

//         function wi(n, t) {
//             var i = t && n,
//                 r = i && 1 === n.nodeType && 1 === t.nodeType && n.sourceIndex - t.sourceIndex;
//             if (r) return r;
//             if (i)
//                 while (i = i.nextSibling)
//                     if (i === t) return -1;
//             return n ? 1 : -1
//         }

//         function ar(n) {
//             return function(t) {
//                 var i = t.nodeName.toLowerCase();
//                 return "input" === i && t.type === n
//             }
//         }

//         function vr(n) {
//             return function(t) {
//                 var i = t.nodeName.toLowerCase();
//                 return ("input" === i || "button" === i) && t.type === n
//             }
//         }

//         function bi(n) {
//             return function(t) {
//                 return "label" in t && t.disabled === n || "form" in t && t.disabled === n || "form" in t && t.disabled === !1 && (t.isDisabled === n || t.isDisabled !== !n && ("label" in t || !lr(t)) !== n)
//             }
//         }

//         function it(n) {
//             return l(function(t) {
//                 return t = +t, l(function(i, r) {
//                     for (var u, f = n([], i.length, t), e = f.length; e--;) i[u = f[e]] && (i[u] = !(r[u] = i[u]))
//                 })
//             })
//         }

//         function ri(n) {
//             return n && "undefined" != typeof n.getElementsByTagName && n
//         }

//         function ki() {}

//         function yt(n) {
//             for (var t = 0, r = n.length, i = ""; t < r; t++) i += n[t].value;
//             return i
//         }

//         function pt(n, t, i) {
//             var r = t.dir,
//                 u = t.next,
//                 f = u || r,
//                 o = i && "parentNode" === f,
//                 s = di++;
//             return t.first ? function(t, i, u) {
//                 while (t = t[r])
//                     if (1 === t.nodeType || o) return n(t, i, u)
//             } : function(t, i, h) {
//                 var c, l, a, y = [v, s];
//                 if (h) {
//                     while (t = t[r])
//                         if ((1 === t.nodeType || o) && n(t, i, h)) return !0
//                 } else
//                     while (t = t[r])
//                         if (1 === t.nodeType || o)
//                             if (a = t[e] || (t[e] = {}), l = a[t.uniqueID] || (a[t.uniqueID] = {}), u && u === t.nodeName.toLowerCase()) t = t[r] || t;
//                             else {
//                                 if ((c = l[f]) && c[0] === v && c[1] === s) return y[2] = c[2];
//                                 if (l[f] = y, y[2] = n(t, i, h)) return !0
//                             }
//             }
//         }

//         function ui(n) {
//             return n.length > 1 ? function(t, i, r) {
//                 for (var u = n.length; u--;)
//                     if (!n[u](t, i, r)) return !1;
//                 return !0
//             } : n[0]
//         }

//         function yr(n, t, i) {
//             for (var r = 0, f = t.length; r < f; r++) u(n, t[r], i);
//             return i
//         }

//         function wt(n, t, i, r, u) {
//             for (var e, o = [], f = 0, s = n.length, h = null != t; f < s; f++)(e = n[f]) && (i && !i(e, r, u) || (o.push(e), h && t.push(f)));
//             return o
//         }

//         function fi(n, t, i, r, u, f) {
//             return r && !r[e] && (r = fi(r)), u && !u[e] && (u = fi(u, f)), l(function(f, e, o, s) {
//                 var l, c, a, p = [],
//                     y = [],
//                     w = e.length,
//                     b = f || yr(t || "*", o.nodeType ? [o] : o, []),
//                     v = !n || !f && t ? b : wt(b, p, n, o, s),
//                     h = i ? u || (f ? n : w || r) ? [] : e : v;
//                 if (i && i(v, h, o, s), r)
//                     for (l = wt(h, y), r(l, [], o, s), c = l.length; c--;)(a = l[c]) && (h[y[c]] = !(v[y[c]] = a));
//                 if (f) {
//                     if (u || n) {
//                         if (u) {
//                             for (l = [], c = h.length; c--;)(a = h[c]) && l.push(v[c] = a);
//                             u(null, h = [], l, s)
//                         }
//                         for (c = h.length; c--;)(a = h[c]) && (l = u ? nt(f, a) : p[c]) > -1 && (f[l] = !(e[l] = a))
//                     }
//                 } else h = wt(h === e ? h.splice(w, h.length) : h), u ? u(null, e, h, s) : k.apply(e, h)
//             })
//         }

//         function ei(n) {
//             for (var o, u, r, s = n.length, h = t.relative[n[0].type], c = h || t.relative[" "], i = h ? 1 : 0, l = pt(function(n) {
//                 return n === o
//             }, c, !0), a = pt(function(n) {
//                 return nt(o, n) > -1
//             }, c, !0), f = [function(n, t, i) {
//                 var r = !h && (i || t !== ht) || ((o = t).nodeType ? l(n, t, i) : a(n, t, i));
//                 return o = null, r
//             }]; i < s; i++)
//                 if (u = t.relative[n[i].type]) f = [pt(ui(f), u)];
//                 else {
//                     if (u = t.filter[n[i].type].apply(null, n[i].matches), u[e]) {
//                         for (r = ++i; r < s; r++)
//                             if (t.relative[n[r].type]) break;
//                         return fi(i > 1 && ui(f), i > 1 && yt(n.slice(0, i - 1).concat({
//                             value: " " === n[i - 2].type ? "*" : ""
//                         })).replace(at, "$1"), u, i < r && ei(n.slice(i, r)), r < s && ei(n = n.slice(r)), r < s && yt(n))
//                     }
//                     f.push(u)
//                 } return ui(f)
//         }

//         function pr(n, r) {
//             var f = r.length > 0,
//                 e = n.length > 0,
//                 o = function(o, s, h, l, a) {
//                     var y, nt, d, g = 0,
//                         p = "0",
//                         tt = o && [],
//                         w = [],
//                         it = ht,
//                         rt = o || e && t.find.TAG("*", a),
//                         ut = v += null == it ? 1 : Math.random() || .1,
//                         ft = rt.length;
//                     for (a && (ht = s === i || s || a); p !== ft && null != (y = rt[p]); p++) {
//                         if (e && y) {
//                             for (nt = 0, s || y.ownerDocument === i || (b(y), h = !c); d = n[nt++];)
//                                 if (d(y, s || i, h)) {
//                                     l.push(y);
//                                     break
//                                 } a && (v = ut)
//                         }
//                         f && ((y = !d && y) && g--, o && tt.push(y))
//                     }
//                     if (g += p, f && p !== g) {
//                         for (nt = 0; d = r[nt++];) d(tt, w, s, h);
//                         if (o) {
//                             if (g > 0)
//                                 while (p--) tt[p] || w[p] || (w[p] = nr.call(l));
//                             w = wt(w)
//                         }
//                         k.apply(l, w);
//                         a && !o && w.length > 0 && g + r.length > 1 && u.uniqueSort(l)
//                     }
//                     return a && (v = ut, ht = it), tt
//                 };
//             return f ? l(o) : o
//         }
//         var rt, f, t, st, oi, ft, bt, si, ht, w, ut, b, i, s, c, o, d, ct, et, e = "sizzle" + 1 * new Date,
//             h = n.document,
//             v = 0,
//             di = 0,
//             hi = ti(),
//             ci = ti(),
//             lt = ti(),
//             kt = function(n, t) {
//                 return n === t && (ut = !0), 0
//             },
//             gi = {}.hasOwnProperty,
//             g = [],
//             nr = g.pop,
//             tr = g.push,
//             k = g.push,
//             li = g.slice,
//             nt = function(n, t) {
//                 for (var i = 0, r = n.length; i < r; i++)
//                     if (n[i] === t) return i;
//                 return -1
//             },
//             dt = "checked|selected|async|autofocus|autoplay|controls|defer|disabled|hidden|ismap|loop|multiple|open|readonly|required|scoped",
//             r = "[\\x20\\t\\r\\n\\f]",
//             tt = "(?:\\\\.|[\\w-]|[^\0-\\xa0])+",
//             ai = "\\[" + r + "*(" + tt + ")(?:" + r + "*([*^$|!~]?=)" + r + "*(?:'((?:\\\\.|[^\\\\'])*)'|\"((?:\\\\.|[^\\\\\"])*)\"|(" + tt + "))|)" + r + "*\\]",
//             gt = ":(" + tt + ")(?:\\((('((?:\\\\.|[^\\\\'])*)'|\"((?:\\\\.|[^\\\\\"])*)\")|((?:\\\\.|[^\\\\()[\\]]|" + ai + ")*)|.*)\\)|)",
//             ir = new RegExp(r + "+", "g"),
//             at = new RegExp("^" + r + "+|((?:^|[^\\\\])(?:\\\\.)*)" + r + "+$", "g"),
//             rr = new RegExp("^" + r + "*," + r + "*"),
//             ur = new RegExp("^" + r + "*([>+~]|" + r + ")" + r + "*"),
//             fr = new RegExp("=" + r + "*([^\\]'\"]*?)" + r + "*\\]", "g"),
//             er = new RegExp(gt),
//             or = new RegExp("^" + tt + "$"),
//             vt = {
//                 ID: new RegExp("^#(" + tt + ")"),
//                 CLASS: new RegExp("^\\.(" + tt + ")"),
//                 TAG: new RegExp("^(" + tt + "|[*])"),
//                 ATTR: new RegExp("^" + ai),
//                 PSEUDO: new RegExp("^" + gt),
//                 CHILD: new RegExp("^:(only|first|last|nth|nth-last)-(child|of-type)(?:\\(" + r + "*(even|odd|(([+-]|)(\\d*)n|)" + r + "*(?:([+-]|)" + r + "*(\\d+)|))" + r + "*\\)|)", "i"),
//                 bool: new RegExp("^(?:" + dt + ")$", "i"),
//                 needsContext: new RegExp("^" + r + "*[>+~]|:(even|odd|eq|gt|lt|nth|first|last)(?:\\(" + r + "*((?:-\\d)?\\d*)" + r + "*\\)|)(?=[^-]|$)", "i")
//             },
//             sr = /^(?:input|select|textarea|button)$/i,
//             hr = /^h\d$/i,
//             ot = /^[^{]+\{\s*\[native \w/,
//             cr = /^(?:#([\w-]+)|(\w+)|\.([\w-]+))$/,
//             ni = /[+~]/,
//             y = new RegExp("\\\\([\\da-f]{1,6}" + r + "?|(" + r + ")|.)", "ig"),
//             p = function(n, t, i) {
//                 var r = "0x" + t - 65536;
//                 return r !== r || i ? t : r < 0 ? String.fromCharCode(r + 65536) : String.fromCharCode(r >> 10 | 55296, 1023 & r | 56320)
//             },
//             vi = /([\0-\x1f\x7f]|^-?\d)|^-$|[^\x80-\uFFFF\w-]/g,
//             yi = function(n, t) {
//                 return t ? "\0" === n ? "�" : n.slice(0, -1) + "\\" + n.charCodeAt(n.length - 1).toString(16) + " " : "\\" + n
//             },
//             pi = function() {
//                 b()
//             },
//             lr = pt(function(n) {
//                 return n.disabled === !0
//             }, {
//                 dir: "parentNode",
//                 next: "legend"
//             });
//         try {
//             k.apply(g = li.call(h.childNodes), h.childNodes);
//             g[h.childNodes.length].nodeType
//         } catch (wr) {
//             k = {
//                 apply: g.length ? function(n, t) {
//                     tr.apply(n, li.call(t))
//                 } : function(n, t) {
//                     for (var i = n.length, r = 0; n[i++] = t[r++];);
//                     n.length = i - 1
//                 }
//             }
//         }
//         f = u.support = {};
//         oi = u.isXML = function(n) {
//             var t = n && (n.ownerDocument || n).documentElement;
//             return !!t && "HTML" !== t.nodeName
//         };
//         b = u.setDocument = function(n) {
//             var v, u, l = n ? n.ownerDocument || n : h;
//             return l !== i && 9 === l.nodeType && l.documentElement ? (i = l, s = i.documentElement, c = !oi(i), h !== i && (u = i.defaultView) && u.top !== u && (u.addEventListener ? u.addEventListener("unload", pi, !1) : u.attachEvent && u.attachEvent("onunload", pi)), f.attributes = a(function(n) {
//                 return n.className = "i", !n.getAttribute("className")
//             }), f.getElementsByTagName = a(function(n) {
//                 return n.appendChild(i.createComment("")), !n.getElementsByTagName("*").length
//             }), f.getElementsByClassName = ot.test(i.getElementsByClassName), f.getById = a(function(n) {
//                 return s.appendChild(n).id = e, !i.getElementsByName || !i.getElementsByName(e).length
//             }), f.getById ? (t.find.ID = function(n, t) {
//                 if ("undefined" != typeof t.getElementById && c) {
//                     var i = t.getElementById(n);
//                     return i ? [i] : []
//                 }
//             }, t.filter.ID = function(n) {
//                 var t = n.replace(y, p);
//                 return function(n) {
//                     return n.getAttribute("id") === t
//                 }
//             }) : (delete t.find.ID, t.filter.ID = function(n) {
//                 var t = n.replace(y, p);
//                 return function(n) {
//                     var i = "undefined" != typeof n.getAttributeNode && n.getAttributeNode("id");
//                     return i && i.value === t
//                 }
//             }), t.find.TAG = f.getElementsByTagName ? function(n, t) {
//                 return "undefined" != typeof t.getElementsByTagName ? t.getElementsByTagName(n) : f.qsa ? t.querySelectorAll(n) : void 0
//             } : function(n, t) {
//                 var i, r = [],
//                     f = 0,
//                     u = t.getElementsByTagName(n);
//                 if ("*" === n) {
//                     while (i = u[f++]) 1 === i.nodeType && r.push(i);
//                     return r
//                 }
//                 return u
//             }, t.find.CLASS = f.getElementsByClassName && function(n, t) {
//                 if ("undefined" != typeof t.getElementsByClassName && c) return t.getElementsByClassName(n)
//             }, d = [], o = [], (f.qsa = ot.test(i.querySelectorAll)) && (a(function(n) {
//                 s.appendChild(n).innerHTML = "<a id='" + e + "'><\/a><select id='" + e + "-\r\\' msallowcapture=''><option selected=''><\/option><\/select>";
//                 n.querySelectorAll("[msallowcapture^='']").length && o.push("[*^$]=" + r + "*(?:''|\"\")");
//                 n.querySelectorAll("[selected]").length || o.push("\\[" + r + "*(?:value|" + dt + ")");
//                 n.querySelectorAll("[id~=" + e + "-]").length || o.push("~=");
//                 n.querySelectorAll(":checked").length || o.push(":checked");
//                 n.querySelectorAll("a#" + e + "+*").length || o.push(".#.+[+~]")
//             }), a(function(n) {
//                 n.innerHTML = "<a href='' disabled='disabled'><\/a><select disabled='disabled'><option/><\/select>";
//                 var t = i.createElement("input");
//                 t.setAttribute("type", "hidden");
//                 n.appendChild(t).setAttribute("name", "D");
//                 n.querySelectorAll("[name=d]").length && o.push("name" + r + "*[*^$|!~]?=");
//                 2 !== n.querySelectorAll(":enabled").length && o.push(":enabled", ":disabled");
//                 s.appendChild(n).disabled = !0;
//                 2 !== n.querySelectorAll(":disabled").length && o.push(":enabled", ":disabled");
//                 n.querySelectorAll("*,:x");
//                 o.push(",.*:")
//             })), (f.matchesSelector = ot.test(ct = s.matches || s.webkitMatchesSelector || s.mozMatchesSelector || s.oMatchesSelector || s.msMatchesSelector)) && a(function(n) {
//                 f.disconnectedMatch = ct.call(n, "*");
//                 ct.call(n, "[s!='']:x");
//                 d.push("!=", gt)
//             }), o = o.length && new RegExp(o.join("|")), d = d.length && new RegExp(d.join("|")), v = ot.test(s.compareDocumentPosition), et = v || ot.test(s.contains) ? function(n, t) {
//                 var r = 9 === n.nodeType ? n.documentElement : n,
//                     i = t && t.parentNode;
//                 return n === i || !(!i || 1 !== i.nodeType || !(r.contains ? r.contains(i) : n.compareDocumentPosition && 16 & n.compareDocumentPosition(i)))
//             } : function(n, t) {
//                 if (t)
//                     while (t = t.parentNode)
//                         if (t === n) return !0;
//                 return !1
//             }, kt = v ? function(n, t) {
//                 if (n === t) return ut = !0, 0;
//                 var r = !n.compareDocumentPosition - !t.compareDocumentPosition;
//                 return r ? r : (r = (n.ownerDocument || n) === (t.ownerDocument || t) ? n.compareDocumentPosition(t) : 1, 1 & r || !f.sortDetached && t.compareDocumentPosition(n) === r ? n === i || n.ownerDocument === h && et(h, n) ? -1 : t === i || t.ownerDocument === h && et(h, t) ? 1 : w ? nt(w, n) - nt(w, t) : 0 : 4 & r ? -1 : 1)
//             } : function(n, t) {
//                 if (n === t) return ut = !0, 0;
//                 var r, u = 0,
//                     o = n.parentNode,
//                     s = t.parentNode,
//                     f = [n],
//                     e = [t];
//                 if (!o || !s) return n === i ? -1 : t === i ? 1 : o ? -1 : s ? 1 : w ? nt(w, n) - nt(w, t) : 0;
//                 if (o === s) return wi(n, t);
//                 for (r = n; r = r.parentNode;) f.unshift(r);
//                 for (r = t; r = r.parentNode;) e.unshift(r);
//                 while (f[u] === e[u]) u++;
//                 return u ? wi(f[u], e[u]) : f[u] === h ? -1 : e[u] === h ? 1 : 0
//             }, i) : i
//         };
//         u.matches = function(n, t) {
//             return u(n, null, null, t)
//         };
//         u.matchesSelector = function(n, t) {
//             if ((n.ownerDocument || n) !== i && b(n), t = t.replace(fr, "='$1']"), f.matchesSelector && c && !lt[t + " "] && (!d || !d.test(t)) && (!o || !o.test(t))) try {
//                 var r = ct.call(n, t);
//                 if (r || f.disconnectedMatch || n.document && 11 !== n.document.nodeType) return r
//             } catch (e) {}
//             return u(t, i, null, [n]).length > 0
//         };
//         u.contains = function(n, t) {
//             return (n.ownerDocument || n) !== i && b(n), et(n, t)
//         };
//         u.attr = function(n, r) {
//             (n.ownerDocument || n) !== i && b(n);
//             var e = t.attrHandle[r.toLowerCase()],
//                 u = e && gi.call(t.attrHandle, r.toLowerCase()) ? e(n, r, !c) : void 0;
//             return void 0 !== u ? u : f.attributes || !c ? n.getAttribute(r) : (u = n.getAttributeNode(r)) && u.specified ? u.value : null
//         };
//         u.escape = function(n) {
//             return (n + "").replace(vi, yi)
//         };
//         u.error = function(n) {
//             throw new Error("Syntax error, unrecognized expression: " + n);
//         };
//         u.uniqueSort = function(n) {
//             var r, u = [],
//                 t = 0,
//                 i = 0;
//             if (ut = !f.detectDuplicates, w = !f.sortStable && n.slice(0), n.sort(kt), ut) {
//                 while (r = n[i++]) r === n[i] && (t = u.push(i));
//                 while (t--) n.splice(u[t], 1)
//             }
//             return w = null, n
//         };
//         st = u.getText = function(n) {
//             var r, i = "",
//                 u = 0,
//                 t = n.nodeType;
//             if (t) {
//                 if (1 === t || 9 === t || 11 === t) {
//                     if ("string" == typeof n.textContent) return n.textContent;
//                     for (n = n.firstChild; n; n = n.nextSibling) i += st(n)
//                 } else if (3 === t || 4 === t) return n.nodeValue
//             } else
//                 while (r = n[u++]) i += st(r);
//             return i
//         };
//         t = u.selectors = {
//             cacheLength: 50,
//             createPseudo: l,
//             match: vt,
//             attrHandle: {},
//             find: {},
//             relative: {
//                 ">": {
//                     dir: "parentNode",
//                     first: !0
//                 },
//                 " ": {
//                     dir: "parentNode"
//                 },
//                 "+": {
//                     dir: "previousSibling",
//                     first: !0
//                 },
//                 "~": {
//                     dir: "previousSibling"
//                 }
//             },
//             preFilter: {
//                 ATTR: function(n) {
//                     return n[1] = n[1].replace(y, p), n[3] = (n[3] || n[4] || n[5] || "").replace(y, p), "~=" === n[2] && (n[3] = " " + n[3] + " "), n.slice(0, 4)
//                 },
//                 CHILD: function(n) {
//                     return n[1] = n[1].toLowerCase(), "nth" === n[1].slice(0, 3) ? (n[3] || u.error(n[0]), n[4] = +(n[4] ? n[5] + (n[6] || 1) : 2 * ("even" === n[3] || "odd" === n[3])), n[5] = +(n[7] + n[8] || "odd" === n[3])) : n[3] && u.error(n[0]), n
//                 },
//                 PSEUDO: function(n) {
//                     var i, t = !n[6] && n[2];
//                     return vt.CHILD.test(n[0]) ? null : (n[3] ? n[2] = n[4] || n[5] || "" : t && er.test(t) && (i = ft(t, !0)) && (i = t.indexOf(")", t.length - i) - t.length) && (n[0] = n[0].slice(0, i), n[2] = t.slice(0, i)), n.slice(0, 3))
//                 }
//             },
//             filter: {
//                 TAG: function(n) {
//                     var t = n.replace(y, p).toLowerCase();
//                     return "*" === n ? function() {
//                         return !0
//                     } : function(n) {
//                         return n.nodeName && n.nodeName.toLowerCase() === t
//                     }
//                 },
//                 CLASS: function(n) {
//                     var t = hi[n + " "];
//                     return t || (t = new RegExp("(^|" + r + ")" + n + "(" + r + "|$)")) && hi(n, function(n) {
//                         return t.test("string" == typeof n.className && n.className || "undefined" != typeof n.getAttribute && n.getAttribute("class") || "")
//                     })
//                 },
//                 ATTR: function(n, t, i) {
//                     return function(r) {
//                         var f = u.attr(r, n);
//                         return null == f ? "!=" === t : !t || (f += "", "=" === t ? f === i : "!=" === t ? f !== i : "^=" === t ? i && 0 === f.indexOf(i) : "*=" === t ? i && f.indexOf(i) > -1 : "$=" === t ? i && f.slice(-i.length) === i : "~=" === t ? (" " + f.replace(ir, " ") + " ").indexOf(i) > -1 : "|=" === t && (f === i || f.slice(0, i.length + 1) === i + "-"))
//                     }
//                 },
//                 CHILD: function(n, t, i, r, u) {
//                     var s = "nth" !== n.slice(0, 3),
//                         o = "last" !== n.slice(-4),
//                         f = "of-type" === t;
//                     return 1 === r && 0 === u ? function(n) {
//                         return !!n.parentNode
//                     } : function(t, i, h) {
//                         var p, w, y, c, a, b, k = s !== o ? "nextSibling" : "previousSibling",
//                             d = t.parentNode,
//                             nt = f && t.nodeName.toLowerCase(),
//                             g = !h && !f,
//                             l = !1;
//                         if (d) {
//                             if (s) {
//                                 while (k) {
//                                     for (c = t; c = c[k];)
//                                         if (f ? c.nodeName.toLowerCase() === nt : 1 === c.nodeType) return !1;
//                                     b = k = "only" === n && !b && "nextSibling"
//                                 }
//                                 return !0
//                             }
//                             if (b = [o ? d.firstChild : d.lastChild], o && g) {
//                                 for (c = d, y = c[e] || (c[e] = {}), w = y[c.uniqueID] || (y[c.uniqueID] = {}), p = w[n] || [], a = p[0] === v && p[1], l = a && p[2], c = a && d.childNodes[a]; c = ++a && c && c[k] || (l = a = 0) || b.pop();)
//                                     if (1 === c.nodeType && ++l && c === t) {
//                                         w[n] = [v, a, l];
//                                         break
//                                     }
//                             } else if (g && (c = t, y = c[e] || (c[e] = {}), w = y[c.uniqueID] || (y[c.uniqueID] = {}), p = w[n] || [], a = p[0] === v && p[1], l = a), l === !1)
//                                 while (c = ++a && c && c[k] || (l = a = 0) || b.pop())
//                                     if ((f ? c.nodeName.toLowerCase() === nt : 1 === c.nodeType) && ++l && (g && (y = c[e] || (c[e] = {}), w = y[c.uniqueID] || (y[c.uniqueID] = {}), w[n] = [v, l]), c === t)) break;
//                             return l -= u, l === r || l % r == 0 && l / r >= 0
//                         }
//                     }
//                 },
//                 PSEUDO: function(n, i) {
//                     var f, r = t.pseudos[n] || t.setFilters[n.toLowerCase()] || u.error("unsupported pseudo: " + n);
//                     return r[e] ? r(i) : r.length > 1 ? (f = [n, n, "", i], t.setFilters.hasOwnProperty(n.toLowerCase()) ? l(function(n, t) {
//                         for (var u, f = r(n, i), e = f.length; e--;) u = nt(n, f[e]), n[u] = !(t[u] = f[e])
//                     }) : function(n) {
//                         return r(n, 0, f)
//                     }) : r
//                 }
//             },
//             pseudos: {
//                 not: l(function(n) {
//                     var t = [],
//                         r = [],
//                         i = bt(n.replace(at, "$1"));
//                     return i[e] ? l(function(n, t, r, u) {
//                         for (var e, o = i(n, null, u, []), f = n.length; f--;)(e = o[f]) && (n[f] = !(t[f] = e))
//                     }) : function(n, u, f) {
//                         return t[0] = n, i(t, null, f, r), t[0] = null, !r.pop()
//                     }
//                 }),
//                 has: l(function(n) {
//                     return function(t) {
//                         return u(n, t).length > 0
//                     }
//                 }),
//                 contains: l(function(n) {
//                     return n = n.replace(y, p),
//                         function(t) {
//                             return (t.textContent || t.innerText || st(t)).indexOf(n) > -1
//                         }
//                 }),
//                 lang: l(function(n) {
//                     return or.test(n || "") || u.error("unsupported lang: " + n), n = n.replace(y, p).toLowerCase(),
//                         function(t) {
//                             var i;
//                             do
//                                 if (i = c ? t.lang : t.getAttribute("xml:lang") || t.getAttribute("lang")) return i = i.toLowerCase(), i === n || 0 === i.indexOf(n + "-"); while ((t = t.parentNode) && 1 === t.nodeType);
//                             return !1
//                         }
//                 }),
//                 target: function(t) {
//                     var i = n.location && n.location.hash;
//                     return i && i.slice(1) === t.id
//                 },
//                 root: function(n) {
//                     return n === s
//                 },
//                 focus: function(n) {
//                     return n === i.activeElement && (!i.hasFocus || i.hasFocus()) && !!(n.type || n.href || ~n.tabIndex)
//                 },
//                 enabled: bi(!1),
//                 disabled: bi(!0),
//                 checked: function(n) {
//                     var t = n.nodeName.toLowerCase();
//                     return "input" === t && !!n.checked || "option" === t && !!n.selected
//                 },
//                 selected: function(n) {
//                     return n.parentNode && n.parentNode.selectedIndex, n.selected === !0
//                 },
//                 empty: function(n) {
//                     for (n = n.firstChild; n; n = n.nextSibling)
//                         if (n.nodeType < 6) return !1;
//                     return !0
//                 },
//                 parent: function(n) {
//                     return !t.pseudos.empty(n)
//                 },
//                 header: function(n) {
//                     return hr.test(n.nodeName)
//                 },
//                 input: function(n) {
//                     return sr.test(n.nodeName)
//                 },
//                 button: function(n) {
//                     var t = n.nodeName.toLowerCase();
//                     return "input" === t && "button" === n.type || "button" === t
//                 },
//                 text: function(n) {
//                     var t;
//                     return "input" === n.nodeName.toLowerCase() && "text" === n.type && (null == (t = n.getAttribute("type")) || "text" === t.toLowerCase())
//                 },
//                 first: it(function() {
//                     return [0]
//                 }),
//                 last: it(function(n, t) {
//                     return [t - 1]
//                 }),
//                 eq: it(function(n, t, i) {
//                     return [i < 0 ? i + t : i]
//                 }),
//                 even: it(function(n, t) {
//                     for (var i = 0; i < t; i += 2) n.push(i);
//                     return n
//                 }),
//                 odd: it(function(n, t) {
//                     for (var i = 1; i < t; i += 2) n.push(i);
//                     return n
//                 }),
//                 lt: it(function(n, t, i) {
//                     for (var r = i < 0 ? i + t : i; --r >= 0;) n.push(r);
//                     return n
//                 }),
//                 gt: it(function(n, t, i) {
//                     for (var r = i < 0 ? i + t : i; ++r < t;) n.push(r);
//                     return n
//                 })
//             }
//         };
//         t.pseudos.nth = t.pseudos.eq;
//         for (rt in {
//             radio: !0,
//             checkbox: !0,
//             file: !0,
//             password: !0,
//             image: !0
//         }) t.pseudos[rt] = ar(rt);
//         for (rt in {
//             submit: !0,
//             reset: !0
//         }) t.pseudos[rt] = vr(rt);
//         return ki.prototype = t.filters = t.pseudos, t.setFilters = new ki, ft = u.tokenize = function(n, i) {
//             var e, f, s, o, r, h, c, l = ci[n + " "];
//             if (l) return i ? 0 : l.slice(0);
//             for (r = n, h = [], c = t.preFilter; r;) {
//                 (!e || (f = rr.exec(r))) && (f && (r = r.slice(f[0].length) || r), h.push(s = []));
//                 e = !1;
//                 (f = ur.exec(r)) && (e = f.shift(), s.push({
//                     value: e,
//                     type: f[0].replace(at, " ")
//                 }), r = r.slice(e.length));
//                 for (o in t.filter)(f = vt[o].exec(r)) && (!c[o] || (f = c[o](f))) && (e = f.shift(), s.push({
//                     value: e,
//                     type: o,
//                     matches: f
//                 }), r = r.slice(e.length));
//                 if (!e) break
//             }
//             return i ? r.length : r ? u.error(n) : ci(n, h).slice(0)
//         }, bt = u.compile = function(n, t) {
//             var r, u = [],
//                 f = [],
//                 i = lt[n + " "];
//             if (!i) {
//                 for (t || (t = ft(n)), r = t.length; r--;) i = ei(t[r]), i[e] ? u.push(i) : f.push(i);
//                 i = lt(n, pr(f, u));
//                 i.selector = n
//             }
//             return i
//         }, si = u.select = function(n, i, r, u) {
//             var s, e, o, a, v, l = "function" == typeof n && n,
//                 h = !u && ft(n = l.selector || n);
//             if (r = r || [], 1 === h.length) {
//                 if (e = h[0] = h[0].slice(0), e.length > 2 && "ID" === (o = e[0]).type && f.getById && 9 === i.nodeType && c && t.relative[e[1].type]) {
//                     if (i = (t.find.ID(o.matches[0].replace(y, p), i) || [])[0], !i) return r;
//                     l && (i = i.parentNode);
//                     n = n.slice(e.shift().value.length)
//                 }
//                 for (s = vt.needsContext.test(n) ? 0 : e.length; s--;) {
//                     if (o = e[s], t.relative[a = o.type]) break;
//                     if ((v = t.find[a]) && (u = v(o.matches[0].replace(y, p), ni.test(e[0].type) && ri(i.parentNode) || i))) {
//                         if (e.splice(s, 1), n = u.length && yt(e), !n) return k.apply(r, u), r;
//                         break
//                     }
//                 }
//             }
//             return (l || bt(n, h))(u, i, !c, r, !i || ni.test(n) && ri(i.parentNode) || i), r
//         }, f.sortStable = e.split("").sort(kt).join("") === e, f.detectDuplicates = !!ut, b(), f.sortDetached = a(function(n) {
//             return 1 & n.compareDocumentPosition(i.createElement("fieldset"))
//         }), a(function(n) {
//             return n.innerHTML = "<a href='#'><\/a>", "#" === n.firstChild.getAttribute("href")
//         }) || ii("type|href|height|width", function(n, t, i) {
//             if (!i) return n.getAttribute(t, "type" === t.toLowerCase() ? 1 : 2)
//         }), f.attributes && a(function(n) {
//             return n.innerHTML = "<input/>", n.firstChild.setAttribute("value", ""), "" === n.firstChild.getAttribute("value")
//         }) || ii("value", function(n, t, i) {
//             if (!i && "input" === n.nodeName.toLowerCase()) return n.defaultValue
//         }), a(function(n) {
//             return null == n.getAttribute("disabled")
//         }) || ii(dt, function(n, t, i) {
//             var r;
//             if (!i) return n[t] === !0 ? t.toLowerCase() : (r = n.getAttributeNode(t)) && r.specified ? r.value : null
//         }), u
//     }(n);
//     i.find = v;
//     i.expr = v.selectors;
//     i.expr[":"] = i.expr.pseudos;
//     i.uniqueSort = i.unique = v.uniqueSort;
//     i.text = v.getText;
//     i.isXMLDoc = v.isXML;
//     i.contains = v.contains;
//     i.escapeSelector = v.escape;
//     var k = function(n, t, r) {
//             for (var u = [], f = void 0 !== r;
//                  (n = n[t]) && 9 !== n.nodeType;)
//                 if (1 === n.nodeType) {
//                     if (f && i(n).is(r)) break;
//                     u.push(n)
//                 } return u
//         },
//         tr = function(n, t) {
//             for (var i = []; n; n = n.nextSibling) 1 === n.nodeType && n !== t && i.push(n);
//             return i
//         },
//         ir = i.expr.match.needsContext,
//         rr = /^<([a-z][^\/\0>:\x20\t\r\n\f]*)[\x20\t\r\n\f]*\/?>(?:<\/\1>|)$/i,
//         gf = /^.[^:#\[\.,]*$/;
//     i.filter = function(n, t, r) {
//         var u = t[0];
//         return r && (n = ":not(" + n + ")"), 1 === t.length && 1 === u.nodeType ? i.find.matchesSelector(u, n) ? [u] : [] : i.find.matches(n, i.grep(t, function(n) {
//             return 1 === n.nodeType
//         }))
//     };
//     i.fn.extend({
//         find: function(n) {
//             var t, r, u = this.length,
//                 f = this;
//             if ("string" != typeof n) return this.pushStack(i(n).filter(function() {
//                 for (t = 0; t < u; t++)
//                     if (i.contains(f[t], this)) return !0
//             }));
//             for (r = this.pushStack([]), t = 0; t < u; t++) i.find(n, f[t], r);
//             return u > 1 ? i.uniqueSort(r) : r
//         },
//         filter: function(n) {
//             return this.pushStack(fi(this, n || [], !1))
//         },
//         not: function(n) {
//             return this.pushStack(fi(this, n || [], !0))
//         },
//         is: function(n) {
//             return !!fi(this, "string" == typeof n && ir.test(n) ? i(n) : n || [], !1).length
//         }
//     });
//     fr = /^(?:\s*(<[\w\W]+>)[^>]*|#([\w-]+))$/;
//     er = i.fn.init = function(n, t, r) {
//         var f, e;
//         if (!n) return this;
//         if (r = r || ur, "string" == typeof n) {
//             if (f = "<" === n[0] && ">" === n[n.length - 1] && n.length >= 3 ? [null, n, null] : fr.exec(n), !f || !f[1] && t) return !t || t.jquery ? (t || r).find(n) : this.constructor(t).find(n);
//             if (f[1]) {
//                 if (t = t instanceof i ? t[0] : t, i.merge(this, i.parseHTML(f[1], t && t.nodeType ? t.ownerDocument || t : u, !0)), rr.test(f[1]) && i.isPlainObject(t))
//                     for (f in t) i.isFunction(this[f]) ? this[f](t[f]) : this.attr(f, t[f]);
//                 return this
//             }
//             return e = u.getElementById(f[2]), e && (this[0] = e, this.length = 1), this
//         }
//         return n.nodeType ? (this[0] = n, this.length = 1, this) : i.isFunction(n) ? void 0 !== r.ready ? r.ready(n) : n(i) : i.makeArray(n, this)
//     };
//     er.prototype = i.fn;
//     ur = i(u);
//     or = /^(?:parents|prev(?:Until|All))/;
//     sr = {
//         children: !0,
//         contents: !0,
//         next: !0,
//         prev: !0
//     };
//     i.fn.extend({
//         has: function(n) {
//             var t = i(n, this),
//                 r = t.length;
//             return this.filter(function() {
//                 for (var n = 0; n < r; n++)
//                     if (i.contains(this, t[n])) return !0
//             })
//         },
//         closest: function(n, t) {
//             var r, f = 0,
//                 o = this.length,
//                 u = [],
//                 e = "string" != typeof n && i(n);
//             if (!ir.test(n))
//                 for (; f < o; f++)
//                     for (r = this[f]; r && r !== t; r = r.parentNode)
//                         if (r.nodeType < 11 && (e ? e.index(r) > -1 : 1 === r.nodeType && i.find.matchesSelector(r, n))) {
//                             u.push(r);
//                             break
//                         } return this.pushStack(u.length > 1 ? i.uniqueSort(u) : u)
//         },
//         index: function(n) {
//             return n ? "string" == typeof n ? lt.call(i(n), this[0]) : lt.call(this, n.jquery ? n[0] : n) : this[0] && this[0].parentNode ? this.first().prevAll().length : -1
//         },
//         add: function(n, t) {
//             return this.pushStack(i.uniqueSort(i.merge(this.get(), i(n, t))))
//         },
//         addBack: function(n) {
//             return this.add(null == n ? this.prevObject : this.prevObject.filter(n))
//         }
//     });
//     i.each({
//         parent: function(n) {
//             var t = n.parentNode;
//             return t && 11 !== t.nodeType ? t : null
//         },
//         parents: function(n) {
//             return k(n, "parentNode")
//         },
//         parentsUntil: function(n, t, i) {
//             return k(n, "parentNode", i)
//         },
//         next: function(n) {
//             return hr(n, "nextSibling")
//         },
//         prev: function(n) {
//             return hr(n, "previousSibling")
//         },
//         nextAll: function(n) {
//             return k(n, "nextSibling")
//         },
//         prevAll: function(n) {
//             return k(n, "previousSibling")
//         },
//         nextUntil: function(n, t, i) {
//             return k(n, "nextSibling", i)
//         },
//         prevUntil: function(n, t, i) {
//             return k(n, "previousSibling", i)
//         },
//         siblings: function(n) {
//             return tr((n.parentNode || {}).firstChild, n)
//         },
//         children: function(n) {
//             return tr(n.firstChild)
//         },
//         contents: function(n) {
//             return n.contentDocument || i.merge([], n.childNodes)
//         }
//     }, function(n, t) {
//         i.fn[n] = function(r, u) {
//             var f = i.map(this, t, r);
//             return "Until" !== n.slice(-5) && (u = r), u && "string" == typeof u && (f = i.filter(u, f)), this.length > 1 && (sr[n] || i.uniqueSort(f), or.test(n) && f.reverse()), this.pushStack(f)
//         }
//     });
//     h = /\S+/g;
//     i.Callbacks = function(n) {
//         n = "string" == typeof n ? ne(n) : i.extend({}, n);
//         var f, r, h, e, t = [],
//             o = [],
//             u = -1,
//             c = function() {
//                 for (e = n.once, h = f = !0; o.length; u = -1)
//                     for (r = o.shift(); ++u < t.length;) t[u].apply(r[0], r[1]) === !1 && n.stopOnFalse && (u = t.length, r = !1);
//                 n.memory || (r = !1);
//                 f = !1;
//                 e && (t = r ? [] : "")
//             },
//             s = {
//                 add: function() {
//                     return t && (r && !f && (u = t.length - 1, o.push(r)), function e(r) {
//                         i.each(r, function(r, u) {
//                             i.isFunction(u) ? n.unique && s.has(u) || t.push(u) : u && u.length && "string" !== i.type(u) && e(u)
//                         })
//                     }(arguments), r && !f && c()), this
//                 },
//                 remove: function() {
//                     return i.each(arguments, function(n, r) {
//                         for (var f;
//                              (f = i.inArray(r, t, f)) > -1;) t.splice(f, 1), f <= u && u--
//                     }), this
//                 },
//                 has: function(n) {
//                     return n ? i.inArray(n, t) > -1 : t.length > 0
//                 },
//                 empty: function() {
//                     return t && (t = []), this
//                 },
//                 disable: function() {
//                     return e = o = [], t = r = "", this
//                 },
//                 disabled: function() {
//                     return !t
//                 },
//                 lock: function() {
//                     return e = o = [], r || f || (t = r = ""), this
//                 },
//                 locked: function() {
//                     return !!e
//                 },
//                 fireWith: function(n, t) {
//                     return e || (t = t || [], t = [n, t.slice ? t.slice() : t], o.push(t), f || c()), this
//                 },
//                 fire: function() {
//                     return s.fireWith(this, arguments), this
//                 },
//                 fired: function() {
//                     return !!h
//                 }
//             };
//         return s
//     };
//     i.extend({
//         Deferred: function(t) {
//             var u = [
//                     ["notify", "progress", i.Callbacks("memory"), i.Callbacks("memory"), 2],
//                     ["resolve", "done", i.Callbacks("once memory"), i.Callbacks("once memory"), 0, "resolved"],
//                     ["reject", "fail", i.Callbacks("once memory"), i.Callbacks("once memory"), 1, "rejected"]
//                 ],
//                 e = "pending",
//                 f = {
//                     state: function() {
//                         return e
//                     },
//                     always: function() {
//                         return r.done(arguments).fail(arguments), this
//                     },
//                     "catch": function(n) {
//                         return f.then(null, n)
//                     },
//                     pipe: function() {
//                         var n = arguments;
//                         return i.Deferred(function(t) {
//                             i.each(u, function(u, f) {
//                                 var e = i.isFunction(n[f[4]]) && n[f[4]];
//                                 r[f[1]](function() {
//                                     var n = e && e.apply(this, arguments);
//                                     n && i.isFunction(n.promise) ? n.promise().progress(t.notify).done(t.resolve).fail(t.reject) : t[f[0] + "With"](this, e ? [n] : arguments)
//                                 })
//                             });
//                             n = null
//                         }).promise()
//                     },
//                     then: function(t, r, f) {
//                         function o(t, r, u, f) {
//                             return function() {
//                                 var s = this,
//                                     h = arguments,
//                                     l = function() {
//                                         var n, c;
//                                         if (!(t < e)) {
//                                             if (n = u.apply(s, h), n === r.promise()) throw new TypeError("Thenable self-resolution");
//                                             c = n && ("object" == typeof n || "function" == typeof n) && n.then;
//                                             i.isFunction(c) ? f ? c.call(n, o(e, r, d, f), o(e, r, yt, f)) : (e++, c.call(n, o(e, r, d, f), o(e, r, yt, f), o(e, r, d, r.notifyWith))) : (u !== d && (s = void 0, h = [n]), (f || r.resolveWith)(s, h))
//                                         }
//                                     },
//                                     c = f ? l : function() {
//                                         try {
//                                             l()
//                                         } catch (n) {
//                                             i.Deferred.exceptionHook && i.Deferred.exceptionHook(n, c.stackTrace);
//                                             t + 1 >= e && (u !== yt && (s = void 0, h = [n]), r.rejectWith(s, h))
//                                         }
//                                     };
//                                 t ? c() : (i.Deferred.getStackHook && (c.stackTrace = i.Deferred.getStackHook()), n.setTimeout(c))
//                             }
//                         }
//                         var e = 0;
//                         return i.Deferred(function(n) {
//                             u[0][3].add(o(0, n, i.isFunction(f) ? f : d, n.notifyWith));
//                             u[1][3].add(o(0, n, i.isFunction(t) ? t : d));
//                             u[2][3].add(o(0, n, i.isFunction(r) ? r : yt))
//                         }).promise()
//                     },
//                     promise: function(n) {
//                         return null != n ? i.extend(n, f) : f
//                     }
//                 },
//                 r = {};
//             return i.each(u, function(n, t) {
//                 var i = t[2],
//                     o = t[5];
//                 f[t[1]] = i.add;
//                 o && i.add(function() {
//                     e = o
//                 }, u[3 - n][2].disable, u[0][2].lock);
//                 i.add(t[3].fire);
//                 r[t[0]] = function() {
//                     return r[t[0] + "With"](this === r ? void 0 : this, arguments), this
//                 };
//                 r[t[0] + "With"] = i.fireWith
//             }), f.promise(r), t && t.call(r, r), r
//         },
//         when: function(n) {
//             var f = arguments.length,
//                 t = f,
//                 e = Array(t),
//                 u = p.call(arguments),
//                 r = i.Deferred(),
//                 o = function(n) {
//                     return function(t) {
//                         e[n] = this;
//                         u[n] = arguments.length > 1 ? p.call(arguments) : t;
//                         --f || r.resolveWith(e, u)
//                     }
//                 };
//             if (f <= 1 && (cr(n, r.done(o(t)).resolve, r.reject), "pending" === r.state() || i.isFunction(u[t] && u[t].then))) return r.then();
//             while (t--) cr(u[t], o(t), r.reject);
//             return r.promise()
//         }
//     });
//     lr = /^(Eval|Internal|Range|Reference|Syntax|Type|URI)Error$/;
//     i.Deferred.exceptionHook = function(t, i) {
//         n.console && n.console.warn && t && lr.test(t.name) && n.console.warn("jQuery.Deferred exception: " + t.message, t.stack, i)
//     };
//     i.readyException = function(t) {
//         n.setTimeout(function() {
//             throw t;
//         })
//     };
//     pt = i.Deferred();
//     i.fn.ready = function(n) {
//         return pt.then(n)["catch"](function(n) {
//             i.readyException(n)
//         }), this
//     };
//     i.extend({
//         isReady: !1,
//         readyWait: 1,
//         holdReady: function(n) {
//             n ? i.readyWait++ : i.ready(!0)
//         },
//         ready: function(n) {
//             (n === !0 ? --i.readyWait : i.isReady) || (i.isReady = !0, n !== !0 && --i.readyWait > 0 || pt.resolveWith(u, [i]))
//         }
//     });
//     i.ready.then = pt.then;
//     "complete" === u.readyState || "loading" !== u.readyState && !u.documentElement.doScroll ? n.setTimeout(i.ready) : (u.addEventListener("DOMContentLoaded", wt), n.addEventListener("load", wt));
//     a = function(n, t, r, u, f, e, o) {
//         var s = 0,
//             c = n.length,
//             h = null == r;
//         if ("object" === i.type(r)) {
//             f = !0;
//             for (s in r) a(n, t, s, r[s], !0, e, o)
//         } else if (void 0 !== u && (f = !0, i.isFunction(u) || (o = !0), h && (o ? (t.call(n, u), t = null) : (h = t, t = function(n, t, r) {
//             return h.call(i(n), r)
//         })), t))
//             for (; s < c; s++) t(n[s], r, o ? u : u.call(n[s], s, t(n[s], r)));
//         return f ? n : h ? t.call(n) : c ? t(n[0], r) : e
//     };
//     et = function(n) {
//         return 1 === n.nodeType || 9 === n.nodeType || !+n.nodeType
//     };
//     ot.uid = 1;
//     ot.prototype = {
//         cache: function(n) {
//             var t = n[this.expando];
//             return t || (t = {}, et(n) && (n.nodeType ? n[this.expando] = t : Object.defineProperty(n, this.expando, {
//                 value: t,
//                 configurable: !0
//             }))), t
//         },
//         set: function(n, t, r) {
//             var u, f = this.cache(n);
//             if ("string" == typeof t) f[i.camelCase(t)] = r;
//             else
//                 for (u in t) f[i.camelCase(u)] = t[u];
//             return f
//         },
//         get: function(n, t) {
//             return void 0 === t ? this.cache(n) : n[this.expando] && n[this.expando][i.camelCase(t)]
//         },
//         access: function(n, t, i) {
//             return void 0 === t || t && "string" == typeof t && void 0 === i ? this.get(n, t) : (this.set(n, t, i), void 0 !== i ? i : t)
//         },
//         remove: function(n, t) {
//             var u, r = n[this.expando];
//             if (void 0 !== r) {
//                 if (void 0 !== t)
//                     for (i.isArray(t) ? t = t.map(i.camelCase) : (t = i.camelCase(t), t = (t in r) ? [t] : t.match(h) || []), u = t.length; u--;) delete r[t[u]];
//                 (void 0 === t || i.isEmptyObject(r)) && (n.nodeType ? n[this.expando] = void 0 : delete n[this.expando])
//             }
//         },
//         hasData: function(n) {
//             var t = n[this.expando];
//             return void 0 !== t && !i.isEmptyObject(t)
//         }
//     };
//     var r = new ot,
//         e = new ot,
//         te = /^(?:\{[\w\W]*\}|\[[\w\W]*\])$/,
//         ie = /[A-Z]/g;
//     i.extend({
//         hasData: function(n) {
//             return e.hasData(n) || r.hasData(n)
//         },
//         data: function(n, t, i) {
//             return e.access(n, t, i)
//         },
//         removeData: function(n, t) {
//             e.remove(n, t)
//         },
//         _data: function(n, t, i) {
//             return r.access(n, t, i)
//         },
//         _removeData: function(n, t) {
//             r.remove(n, t)
//         }
//     });
//     i.fn.extend({
//         data: function(n, t) {
//             var o, f, s, u = this[0],
//                 h = u && u.attributes;
//             if (void 0 === n) {
//                 if (this.length && (s = e.get(u), 1 === u.nodeType && !r.get(u, "hasDataAttrs"))) {
//                     for (o = h.length; o--;) h[o] && (f = h[o].name, 0 === f.indexOf("data-") && (f = i.camelCase(f.slice(5)), ar(u, f, s[f])));
//                     r.set(u, "hasDataAttrs", !0)
//                 }
//                 return s
//             }
//             return "object" == typeof n ? this.each(function() {
//                 e.set(this, n)
//             }) : a(this, function(t) {
//                 var i;
//                 if (u && void 0 === t) {
//                     if ((i = e.get(u, n), void 0 !== i) || (i = ar(u, n), void 0 !== i)) return i
//                 } else this.each(function() {
//                     e.set(this, n, t)
//                 })
//             }, null, t, arguments.length > 1, null, !0)
//         },
//         removeData: function(n) {
//             return this.each(function() {
//                 e.remove(this, n)
//             })
//         }
//     });
//     i.extend({
//         queue: function(n, t, u) {
//             var f;
//             if (n) return t = (t || "fx") + "queue", f = r.get(n, t), u && (!f || i.isArray(u) ? f = r.access(n, t, i.makeArray(u)) : f.push(u)), f || []
//         },
//         dequeue: function(n, t) {
//             t = t || "fx";
//             var r = i.queue(n, t),
//                 e = r.length,
//                 u = r.shift(),
//                 f = i._queueHooks(n, t),
//                 o = function() {
//                     i.dequeue(n, t)
//                 };
//             "inprogress" === u && (u = r.shift(), e--);
//             u && ("fx" === t && r.unshift("inprogress"), delete f.stop, u.call(n, o, f));
//             !e && f && f.empty.fire()
//         },
//         _queueHooks: function(n, t) {
//             var u = t + "queueHooks";
//             return r.get(n, u) || r.access(n, u, {
//                 empty: i.Callbacks("once memory").add(function() {
//                     r.remove(n, [t + "queue", u])
//                 })
//             })
//         }
//     });
//     i.fn.extend({
//         queue: function(n, t) {
//             var r = 2;
//             return "string" != typeof n && (t = n, n = "fx", r--), arguments.length < r ? i.queue(this[0], n) : void 0 === t ? this : this.each(function() {
//                 var r = i.queue(this, n, t);
//                 i._queueHooks(this, n);
//                 "fx" === n && "inprogress" !== r[0] && i.dequeue(this, n)
//             })
//         },
//         dequeue: function(n) {
//             return this.each(function() {
//                 i.dequeue(this, n)
//             })
//         },
//         clearQueue: function(n) {
//             return this.queue(n || "fx", [])
//         },
//         promise: function(n, t) {
//             var u, e = 1,
//                 o = i.Deferred(),
//                 f = this,
//                 s = this.length,
//                 h = function() {
//                     --e || o.resolveWith(f, [f])
//                 };
//             for ("string" != typeof n && (t = n, n = void 0), n = n || "fx"; s--;) u = r.get(f[s], n + "queueHooks"), u && u.empty && (e++, u.empty.add(h));
//             return h(), o.promise(t)
//         }
//     });
//     var vr = /[+-]?(?:\d*\.|)\d+(?:[eE][+-]?\d+|)/.source,
//         st = new RegExp("^(?:([+-])=|)(" + vr + ")([a-z%]*)$", "i"),
//         w = ["Top", "Right", "Bottom", "Left"],
//         bt = function(n, t) {
//             return n = t || n, "none" === n.style.display || "" === n.style.display && i.contains(n.ownerDocument, n) && "none" === i.css(n, "display")
//         },
//         yr = function(n, t, i, r) {
//             var f, u, e = {};
//             for (u in t) e[u] = n.style[u], n.style[u] = t[u];
//             f = i.apply(n, r || []);
//             for (u in t) n.style[u] = e[u];
//             return f
//         };
//     ei = {};
//     i.fn.extend({
//         show: function() {
//             return g(this, !0)
//         },
//         hide: function() {
//             return g(this)
//         },
//         toggle: function(n) {
//             return "boolean" == typeof n ? n ? this.show() : this.hide() : this.each(function() {
//                 bt(this) ? i(this).show() : i(this).hide()
//             })
//         }
//     });
//     var wr = /^(?:checkbox|radio)$/i,
//         br = /<([a-z][^\/\0>\x20\t\r\n\f]+)/i,
//         kr = /^$|\/(?:java|ecma)script/i,
//         c = {
//             option: [1, "<select multiple='multiple'>", "<\/select>"],
//             thead: [1, "<table>", "<\/table>"],
//             col: [2, "<table><colgroup>", "<\/colgroup><\/table>"],
//             tr: [2, "<table><tbody>", "<\/tbody><\/table>"],
//             td: [3, "<table><tbody><tr>", "<\/tr><\/tbody><\/table>"],
//             _default: [0, "", ""]
//         };
//     c.optgroup = c.option;
//     c.tbody = c.tfoot = c.colgroup = c.caption = c.thead;
//     c.th = c.td;
//     dr = /<|&#?\w+;/;
//     ! function() {
//         var i = u.createDocumentFragment(),
//             n = i.appendChild(u.createElement("div")),
//             t = u.createElement("input");
//         t.setAttribute("type", "radio");
//         t.setAttribute("checked", "checked");
//         t.setAttribute("name", "t");
//         n.appendChild(t);
//         f.checkClone = n.cloneNode(!0).cloneNode(!0).lastChild.checked;
//         n.innerHTML = "<textarea>x<\/textarea>";
//         f.noCloneChecked = !!n.cloneNode(!0).lastChild.defaultValue
//     }();
//     var kt = u.documentElement,
//         ue = /^key/,
//         fe = /^(?:mouse|pointer|contextmenu|drag|drop)|click/,
//         nu = /^([^.]*)(?:\.(.+)|)/;
//     i.event = {
//         global: {},
//         add: function(n, t, u, f, e) {
//             var v, y, w, p, b, c, s, l, o, k, d, a = r.get(n);
//             if (a)
//                 for (u.handler && (v = u, u = v.handler, e = v.selector), e && i.find.matchesSelector(kt, e), u.guid || (u.guid = i.guid++), (p = a.events) || (p = a.events = {}), (y = a.handle) || (y = a.handle = function(t) {
//                     if ("undefined" != typeof i && i.event.triggered !== t.type) return i.event.dispatch.apply(n, arguments)
//                 }), t = (t || "").match(h) || [""], b = t.length; b--;) w = nu.exec(t[b]) || [], o = d = w[1], k = (w[2] || "").split(".").sort(), o && (s = i.event.special[o] || {}, o = (e ? s.delegateType : s.bindType) || o, s = i.event.special[o] || {}, c = i.extend({
//                     type: o,
//                     origType: d,
//                     data: f,
//                     handler: u,
//                     guid: u.guid,
//                     selector: e,
//                     needsContext: e && i.expr.match.needsContext.test(e),
//                     namespace: k.join(".")
//                 }, v), (l = p[o]) || (l = p[o] = [], l.delegateCount = 0, s.setup && s.setup.call(n, f, k, y) !== !1 || n.addEventListener && n.addEventListener(o, y)), s.add && (s.add.call(n, c), c.handler.guid || (c.handler.guid = u.guid)), e ? l.splice(l.delegateCount++, 0, c) : l.push(c), i.event.global[o] = !0)
//         },
//         remove: function(n, t, u, f, e) {
//             var y, k, c, v, p, s, l, a, o, b, d, w = r.hasData(n) && r.get(n);
//             if (w && (v = w.events)) {
//                 for (t = (t || "").match(h) || [""], p = t.length; p--;)
//                     if (c = nu.exec(t[p]) || [], o = d = c[1], b = (c[2] || "").split(".").sort(), o) {
//                         for (l = i.event.special[o] || {}, o = (f ? l.delegateType : l.bindType) || o, a = v[o] || [], c = c[2] && new RegExp("(^|\\.)" + b.join("\\.(?:.*\\.|)") + "(\\.|$)"), k = y = a.length; y--;) s = a[y], !e && d !== s.origType || u && u.guid !== s.guid || c && !c.test(s.namespace) || f && f !== s.selector && ("**" !== f || !s.selector) || (a.splice(y, 1), s.selector && a.delegateCount--, l.remove && l.remove.call(n, s));
//                         k && !a.length && (l.teardown && l.teardown.call(n, b, w.handle) !== !1 || i.removeEvent(n, o, w.handle), delete v[o])
//                     } else
//                         for (o in v) i.event.remove(n, o + t[p], u, f, !0);
//                 i.isEmptyObject(v) && r.remove(n, "handle events")
//             }
//         },
//         dispatch: function(n) {
//             var t = i.event.fix(n),
//                 u, c, s, e, f, l, h = new Array(arguments.length),
//                 a = (r.get(this, "events") || {})[t.type] || [],
//                 o = i.event.special[t.type] || {};
//             for (h[0] = t, u = 1; u < arguments.length; u++) h[u] = arguments[u];
//             if (t.delegateTarget = this, !o.preDispatch || o.preDispatch.call(this, t) !== !1) {
//                 for (l = i.event.handlers.call(this, t, a), u = 0;
//                      (e = l[u++]) && !t.isPropagationStopped();)
//                     for (t.currentTarget = e.elem, c = 0;
//                          (f = e.handlers[c++]) && !t.isImmediatePropagationStopped();) t.rnamespace && !t.rnamespace.test(f.namespace) || (t.handleObj = f, t.data = f.data, s = ((i.event.special[f.origType] || {}).handle || f.handler).apply(e.elem, h), void 0 !== s && (t.result = s) === !1 && (t.preventDefault(), t.stopPropagation()));
//                 return o.postDispatch && o.postDispatch.call(this, t), t.result
//             }
//         },
//         handlers: function(n, t) {
//             var e, u, f, o, h = [],
//                 s = t.delegateCount,
//                 r = n.target;
//             if (s && r.nodeType && ("click" !== n.type || isNaN(n.button) || n.button < 1))
//                 for (; r !== this; r = r.parentNode || this)
//                     if (1 === r.nodeType && (r.disabled !== !0 || "click" !== n.type)) {
//                         for (u = [], e = 0; e < s; e++) o = t[e], f = o.selector + " ", void 0 === u[f] && (u[f] = o.needsContext ? i(f, this).index(r) > -1 : i.find(f, this, null, [r]).length), u[f] && u.push(o);
//                         u.length && h.push({
//                             elem: r,
//                             handlers: u
//                         })
//                     } return s < t.length && h.push({
//                 elem: this,
//                 handlers: t.slice(s)
//             }), h
//         },
//         addProp: function(n, t) {
//             Object.defineProperty(i.Event.prototype, n, {
//                 enumerable: !0,
//                 configurable: !0,
//                 get: i.isFunction(t) ? function() {
//                     if (this.originalEvent) return t(this.originalEvent)
//                 } : function() {
//                     if (this.originalEvent) return this.originalEvent[n]
//                 },
//                 set: function(t) {
//                     Object.defineProperty(this, n, {
//                         enumerable: !0,
//                         configurable: !0,
//                         writable: !0,
//                         value: t
//                     })
//                 }
//             })
//         },
//         fix: function(n) {
//             return n[i.expando] ? n : new i.Event(n)
//         },
//         special: {
//             load: {
//                 noBubble: !0
//             },
//             focus: {
//                 trigger: function() {
//                     if (this !== tu() && this.focus) return this.focus(), !1
//                 },
//                 delegateType: "focusin"
//             },
//             blur: {
//                 trigger: function() {
//                     if (this === tu() && this.blur) return this.blur(), !1
//                 },
//                 delegateType: "focusout"
//             },
//             click: {
//                 trigger: function() {
//                     if ("checkbox" === this.type && this.click && i.nodeName(this, "input")) return this.click(), !1
//                 },
//                 _default: function(n) {
//                     return i.nodeName(n.target, "a")
//                 }
//             },
//             beforeunload: {
//                 postDispatch: function(n) {
//                     void 0 !== n.result && n.originalEvent && (n.originalEvent.returnValue = n.result)
//                 }
//             }
//         }
//     };
//     i.removeEvent = function(n, t, i) {
//         n.removeEventListener && n.removeEventListener(t, i)
//     };
//     i.Event = function(n, t) {
//         return this instanceof i.Event ? (n && n.type ? (this.originalEvent = n, this.type = n.type, this.isDefaultPrevented = n.defaultPrevented || void 0 === n.defaultPrevented && n.returnValue === !1 ? dt : nt, this.target = n.target && 3 === n.target.nodeType ? n.target.parentNode : n.target, this.currentTarget = n.currentTarget, this.relatedTarget = n.relatedTarget) : this.type = n, t && i.extend(this, t), this.timeStamp = n && n.timeStamp || i.now(), void(this[i.expando] = !0)) : new i.Event(n, t)
//     };
//     i.Event.prototype = {
//         constructor: i.Event,
//         isDefaultPrevented: nt,
//         isPropagationStopped: nt,
//         isImmediatePropagationStopped: nt,
//         isSimulated: !1,
//         preventDefault: function() {
//             var n = this.originalEvent;
//             this.isDefaultPrevented = dt;
//             n && !this.isSimulated && n.preventDefault()
//         },
//         stopPropagation: function() {
//             var n = this.originalEvent;
//             this.isPropagationStopped = dt;
//             n && !this.isSimulated && n.stopPropagation()
//         },
//         stopImmediatePropagation: function() {
//             var n = this.originalEvent;
//             this.isImmediatePropagationStopped = dt;
//             n && !this.isSimulated && n.stopImmediatePropagation();
//             this.stopPropagation()
//         }
//     };
//     i.each({
//         altKey: !0,
//         bubbles: !0,
//         cancelable: !0,
//         changedTouches: !0,
//         ctrlKey: !0,
//         detail: !0,
//         eventPhase: !0,
//         metaKey: !0,
//         pageX: !0,
//         pageY: !0,
//         shiftKey: !0,
//         view: !0,
//         char: !0,
//         charCode: !0,
//         key: !0,
//         keyCode: !0,
//         button: !0,
//         buttons: !0,
//         clientX: !0,
//         clientY: !0,
//         offsetX: !0,
//         offsetY: !0,
//         pointerId: !0,
//         pointerType: !0,
//         screenX: !0,
//         screenY: !0,
//         targetTouches: !0,
//         toElement: !0,
//         touches: !0,
//         which: function(n) {
//             var t = n.button;
//             return null == n.which && ue.test(n.type) ? null != n.charCode ? n.charCode : n.keyCode : !n.which && void 0 !== t && fe.test(n.type) ? 1 & t ? 1 : 2 & t ? 3 : 4 & t ? 2 : 0 : n.which
//         }
//     }, i.event.addProp);
//     i.each({
//         mouseenter: "mouseover",
//         mouseleave: "mouseout",
//         pointerenter: "pointerover",
//         pointerleave: "pointerout"
//     }, function(n, t) {
//         i.event.special[n] = {
//             delegateType: t,
//             bindType: t,
//             handle: function(n) {
//                 var u, f = this,
//                     r = n.relatedTarget,
//                     e = n.handleObj;
//                 return r && (r === f || i.contains(f, r)) || (n.type = e.origType, u = e.handler.apply(this, arguments), n.type = t), u
//             }
//         }
//     });
//     i.fn.extend({
//         on: function(n, t, i, r) {
//             return si(this, n, t, i, r)
//         },
//         one: function(n, t, i, r) {
//             return si(this, n, t, i, r, 1)
//         },
//         off: function(n, t, r) {
//             var u, f;
//             if (n && n.preventDefault && n.handleObj) return u = n.handleObj, i(n.delegateTarget).off(u.namespace ? u.origType + "." + u.namespace : u.origType, u.selector, u.handler), this;
//             if ("object" == typeof n) {
//                 for (f in n) this.off(f, t, n[f]);
//                 return this
//             }
//             return t !== !1 && "function" != typeof t || (r = t, t = void 0), r === !1 && (r = nt), this.each(function() {
//                 i.event.remove(this, n, r, t)
//             })
//         }
//     });
//     var ee = /<(?!area|br|col|embed|hr|img|input|link|meta|param)(([a-z][^\/\0>\x20\t\r\n\f]*)[^>]*)\/>/gi,
//         oe = /<script|<style|<link/i,
//         se = /checked\s*(?:[^=]|=\s*.checked.)/i,
//         he = /^true\/(.*)/,
//         ce = /^\s*<!(?:\[CDATA\[|--)|(?:\]\]|--)>\s*$/g;
//     i.extend({
//         htmlPrefilter: function(n) {
//             return n.replace(ee, "<$1><\/$2>")
//         },
//         clone: function(n, t, r) {
//             var u, c, s, e, h = n.cloneNode(!0),
//                 l = i.contains(n.ownerDocument, n);
//             if (!(f.noCloneChecked || 1 !== n.nodeType && 11 !== n.nodeType || i.isXMLDoc(n)))
//                 for (e = o(h), s = o(n), u = 0, c = s.length; u < c; u++) ve(s[u], e[u]);
//             if (t)
//                 if (r)
//                     for (s = s || o(n), e = e || o(h), u = 0, c = s.length; u < c; u++) ru(s[u], e[u]);
//                 else ru(n, h);
//             return e = o(h, "script"), e.length > 0 && oi(e, !l && o(n, "script")), h
//         },
//         cleanData: function(n) {
//             for (var u, t, f, s = i.event.special, o = 0; void 0 !== (t = n[o]); o++)
//                 if (et(t)) {
//                     if (u = t[r.expando]) {
//                         if (u.events)
//                             for (f in u.events) s[f] ? i.event.remove(t, f) : i.removeEvent(t, f, u.handle);
//                         t[r.expando] = void 0
//                     }
//                     t[e.expando] && (t[e.expando] = void 0)
//                 }
//         }
//     });
//     i.fn.extend({
//         detach: function(n) {
//             return uu(this, n, !0)
//         },
//         remove: function(n) {
//             return uu(this, n)
//         },
//         text: function(n) {
//             return a(this, function(n) {
//                 return void 0 === n ? i.text(this) : this.empty().each(function() {
//                     1 !== this.nodeType && 11 !== this.nodeType && 9 !== this.nodeType || (this.textContent = n)
//                 })
//             }, null, n, arguments.length)
//         },
//         append: function() {
//             return tt(this, arguments, function(n) {
//                 if (1 === this.nodeType || 11 === this.nodeType || 9 === this.nodeType) {
//                     var t = iu(this, n);
//                     t.appendChild(n)
//                 }
//             })
//         },
//         prepend: function() {
//             return tt(this, arguments, function(n) {
//                 if (1 === this.nodeType || 11 === this.nodeType || 9 === this.nodeType) {
//                     var t = iu(this, n);
//                     t.insertBefore(n, t.firstChild)
//                 }
//             })
//         },
//         before: function() {
//             return tt(this, arguments, function(n) {
//                 this.parentNode && this.parentNode.insertBefore(n, this)
//             })
//         },
//         after: function() {
//             return tt(this, arguments, function(n) {
//                 this.parentNode && this.parentNode.insertBefore(n, this.nextSibling)
//             })
//         },
//         empty: function() {
//             for (var n, t = 0; null != (n = this[t]); t++) 1 === n.nodeType && (i.cleanData(o(n, !1)), n.textContent = "");
//             return this
//         },
//         clone: function(n, t) {
//             return n = null != n && n, t = null == t ? n : t, this.map(function() {
//                 return i.clone(this, n, t)
//             })
//         },
//         html: function(n) {
//             return a(this, function(n) {
//                 var t = this[0] || {},
//                     r = 0,
//                     u = this.length;
//                 if (void 0 === n && 1 === t.nodeType) return t.innerHTML;
//                 if ("string" == typeof n && !oe.test(n) && !c[(br.exec(n) || ["", ""])[1].toLowerCase()]) {
//                     n = i.htmlPrefilter(n);
//                     try {
//                         for (; r < u; r++) t = this[r] || {}, 1 === t.nodeType && (i.cleanData(o(t, !1)), t.innerHTML = n);
//                         t = 0
//                     } catch (f) {}
//                 }
//                 t && this.empty().append(n)
//             }, null, n, arguments.length)
//         },
//         replaceWith: function() {
//             var n = [];
//             return tt(this, arguments, function(t) {
//                 var r = this.parentNode;
//                 i.inArray(this, n) < 0 && (i.cleanData(o(this)), r && r.replaceChild(t, this))
//             }, n)
//         }
//     });
//     i.each({
//         appendTo: "append",
//         prependTo: "prepend",
//         insertBefore: "before",
//         insertAfter: "after",
//         replaceAll: "replaceWith"
//     }, function(n, t) {
//         i.fn[n] = function(n) {
//             for (var u, f = [], e = i(n), o = e.length - 1, r = 0; r <= o; r++) u = r === o ? this : this.clone(!0), i(e[r])[t](u), ri.apply(f, u.get());
//             return this.pushStack(f)
//         }
//     });
//     var fu = /^margin/,
//         hi = new RegExp("^(" + vr + ")(?!px)[a-z%]+$", "i"),
//         gt = function(t) {
//             var i = t.ownerDocument.defaultView;
//             return i && i.opener || (i = n), i.getComputedStyle(t)
//         };
//     ! function() {
//         function r() {
//             if (t) {
//                 t.style.cssText = "box-sizing:border-box;position:relative;display:block;margin:auto;border:1px;padding:1px;top:1%;width:50%";
//                 t.innerHTML = "";
//                 kt.appendChild(e);
//                 var i = n.getComputedStyle(t);
//                 o = "1%" !== i.top;
//                 c = "2px" === i.marginLeft;
//                 s = "4px" === i.width;
//                 t.style.marginRight = "50%";
//                 h = "4px" === i.marginRight;
//                 kt.removeChild(e);
//                 t = null
//             }
//         }
//         var o, s, h, c, e = u.createElement("div"),
//             t = u.createElement("div");
//         t.style && (t.style.backgroundClip = "content-box", t.cloneNode(!0).style.backgroundClip = "", f.clearCloneStyle = "content-box" === t.style.backgroundClip, e.style.cssText = "border:0;width:8px;height:0;top:0;left:-9999px;padding:0;margin-top:1px;position:absolute", e.appendChild(t), i.extend(f, {
//             pixelPosition: function() {
//                 return r(), o
//             },
//             boxSizingReliable: function() {
//                 return r(), s
//             },
//             pixelMarginRight: function() {
//                 return r(), h
//             },
//             reliableMarginLeft: function() {
//                 return r(), c
//             }
//         }))
//     }();
//     var ye = /^(none|table(?!-c[ea]).+)/,
//         pe = {
//             position: "absolute",
//             visibility: "hidden",
//             display: "block"
//         },
//         ou = {
//             letterSpacing: "0",
//             fontWeight: "400"
//         },
//         su = ["Webkit", "Moz", "ms"],
//         hu = u.createElement("div").style;
//     i.extend({
//         cssHooks: {
//             opacity: {
//                 get: function(n, t) {
//                     if (t) {
//                         var i = ht(n, "opacity");
//                         return "" === i ? "1" : i
//                     }
//                 }
//             }
//         },
//         cssNumber: {
//             animationIterationCount: !0,
//             columnCount: !0,
//             fillOpacity: !0,
//             flexGrow: !0,
//             flexShrink: !0,
//             fontWeight: !0,
//             lineHeight: !0,
//             opacity: !0,
//             order: !0,
//             orphans: !0,
//             widows: !0,
//             zIndex: !0,
//             zoom: !0
//         },
//         cssProps: {
//             float: "cssFloat"
//         },
//         style: function(n, t, r, u) {
//             if (n && 3 !== n.nodeType && 8 !== n.nodeType && n.style) {
//                 var e, h, o, s = i.camelCase(t),
//                     c = n.style;
//                 return t = i.cssProps[s] || (i.cssProps[s] = cu(s) || s), o = i.cssHooks[t] || i.cssHooks[s], void 0 === r ? o && "get" in o && void 0 !== (e = o.get(n, !1, u)) ? e : c[t] : (h = typeof r, "string" === h && (e = st.exec(r)) && e[1] && (r = pr(n, t, e), h = "number"), null != r && r === r && ("number" === h && (r += e && e[3] || (i.cssNumber[s] ? "" : "px")), f.clearCloneStyle || "" !== r || 0 !== t.indexOf("background") || (c[t] = "inherit"), o && "set" in o && void 0 === (r = o.set(n, r, u)) || (c[t] = r)), void 0)
//             }
//         },
//         css: function(n, t, r, u) {
//             var f, s, o, e = i.camelCase(t);
//             return t = i.cssProps[e] || (i.cssProps[e] = cu(e) || e), o = i.cssHooks[t] || i.cssHooks[e], o && "get" in o && (f = o.get(n, !0, r)), void 0 === f && (f = ht(n, t, u)), "normal" === f && t in ou && (f = ou[t]), "" === r || r ? (s = parseFloat(f), r === !0 || isFinite(s) ? s || 0 : f) : f
//         }
//     });
//     i.each(["height", "width"], function(n, t) {
//         i.cssHooks[t] = {
//             get: function(n, r, u) {
//                 if (r) return !ye.test(i.css(n, "display")) || n.getClientRects().length && n.getBoundingClientRect().width ? vu(n, t, u) : yr(n, pe, function() {
//                     return vu(n, t, u)
//                 })
//             },
//             set: function(n, r, u) {
//                 var f, e = u && gt(n),
//                     o = u && au(n, t, u, "border-box" === i.css(n, "boxSizing", !1, e), e);
//                 return o && (f = st.exec(r)) && "px" !== (f[3] || "px") && (n.style[t] = r, r = i.css(n, t)), lu(n, r, o)
//             }
//         }
//     });
//     i.cssHooks.marginLeft = eu(f.reliableMarginLeft, function(n, t) {
//         if (t) return (parseFloat(ht(n, "marginLeft")) || n.getBoundingClientRect().left - yr(n, {
//             marginLeft: 0
//         }, function() {
//             return n.getBoundingClientRect().left
//         })) + "px"
//     });
//     i.each({
//         margin: "",
//         padding: "",
//         border: "Width"
//     }, function(n, t) {
//         i.cssHooks[n + t] = {
//             expand: function(i) {
//                 for (var r = 0, f = {}, u = "string" == typeof i ? i.split(" ") : [i]; r < 4; r++) f[n + w[r] + t] = u[r] || u[r - 2] || u[0];
//                 return f
//             }
//         };
//         fu.test(n) || (i.cssHooks[n + t].set = lu)
//     });
//     i.fn.extend({
//         css: function(n, t) {
//             return a(this, function(n, t, r) {
//                 var f, e, o = {},
//                     u = 0;
//                 if (i.isArray(t)) {
//                     for (f = gt(n), e = t.length; u < e; u++) o[t[u]] = i.css(n, t[u], !1, f);
//                     return o
//                 }
//                 return void 0 !== r ? i.style(n, t, r) : i.css(n, t)
//             }, n, t, arguments.length > 1)
//         }
//     });
//     i.Tween = s;
//     s.prototype = {
//         constructor: s,
//         init: function(n, t, r, u, f, e) {
//             this.elem = n;
//             this.prop = r;
//             this.easing = f || i.easing._default;
//             this.options = t;
//             this.start = this.now = this.cur();
//             this.end = u;
//             this.unit = e || (i.cssNumber[r] ? "" : "px")
//         },
//         cur: function() {
//             var n = s.propHooks[this.prop];
//             return n && n.get ? n.get(this) : s.propHooks._default.get(this)
//         },
//         run: function(n) {
//             var t, r = s.propHooks[this.prop];
//             return this.pos = this.options.duration ? t = i.easing[this.easing](n, this.options.duration * n, 0, 1, this.options.duration) : t = n, this.now = (this.end - this.start) * t + this.start, this.options.step && this.options.step.call(this.elem, this.now, this), r && r.set ? r.set(this) : s.propHooks._default.set(this), this
//         }
//     };
//     s.prototype.init.prototype = s.prototype;
//     s.propHooks = {
//         _default: {
//             get: function(n) {
//                 var t;
//                 return 1 !== n.elem.nodeType || null != n.elem[n.prop] && null == n.elem.style[n.prop] ? n.elem[n.prop] : (t = i.css(n.elem, n.prop, ""), t && "auto" !== t ? t : 0)
//             },
//             set: function(n) {
//                 i.fx.step[n.prop] ? i.fx.step[n.prop](n) : 1 !== n.elem.nodeType || null == n.elem.style[i.cssProps[n.prop]] && !i.cssHooks[n.prop] ? n.elem[n.prop] = n.now : i.style(n.elem, n.prop, n.now + n.unit)
//             }
//         }
//     };
//     s.propHooks.scrollTop = s.propHooks.scrollLeft = {
//         set: function(n) {
//             n.elem.nodeType && n.elem.parentNode && (n.elem[n.prop] = n.now)
//         }
//     };
//     i.easing = {
//         linear: function(n) {
//             return n
//         },
//         swing: function(n) {
//             return .5 - Math.cos(n * Math.PI) / 2
//         },
//         _default: "swing"
//     };
//     i.fx = s.prototype.init;
//     i.fx.step = {};
//     yu = /^(?:toggle|show|hide)$/;
//     pu = /queueHooks$/;
//     i.Animation = i.extend(l, {
//         tweeners: {
//             "*": [function(n, t) {
//                 var i = this.createTween(n, t);
//                 return pr(i.elem, n, st.exec(t), i), i
//             }]
//         },
//         tweener: function(n, t) {
//             i.isFunction(n) ? (t = n, n = ["*"]) : n = n.match(h);
//             for (var r, u = 0, f = n.length; u < f; u++) r = n[u], l.tweeners[r] = l.tweeners[r] || [], l.tweeners[r].unshift(t)
//         },
//         prefilters: [we],
//         prefilter: function(n, t) {
//             t ? l.prefilters.unshift(n) : l.prefilters.push(n)
//         }
//     });
//     i.speed = function(n, t, r) {
//         var f = n && "object" == typeof n ? i.extend({}, n) : {
//             complete: r || !r && t || i.isFunction(n) && n,
//             duration: n,
//             easing: r && t || t && !i.isFunction(t) && t
//         };
//         return f.duration = i.fx.off || u.hidden ? 0 : "number" == typeof f.duration ? f.duration : f.duration in i.fx.speeds ? i.fx.speeds[f.duration] : i.fx.speeds._default, null != f.queue && f.queue !== !0 || (f.queue = "fx"), f.old = f.complete, f.complete = function() {
//             i.isFunction(f.old) && f.old.call(this);
//             f.queue && i.dequeue(this, f.queue)
//         }, f
//     };
//     i.fn.extend({
//         fadeTo: function(n, t, i, r) {
//             return this.filter(bt).css("opacity", 0).show().end().animate({
//                 opacity: t
//             }, n, i, r)
//         },
//         animate: function(n, t, u, f) {
//             var s = i.isEmptyObject(n),
//                 o = i.speed(t, u, f),
//                 e = function() {
//                     var t = l(this, i.extend({}, n), o);
//                     (s || r.get(this, "finish")) && t.stop(!0)
//                 };
//             return e.finish = e, s || o.queue === !1 ? this.each(e) : this.queue(o.queue, e)
//         },
//         stop: function(n, t, u) {
//             var f = function(n) {
//                 var t = n.stop;
//                 delete n.stop;
//                 t(u)
//             };
//             return "string" != typeof n && (u = t, t = n, n = void 0), t && n !== !1 && this.queue(n || "fx", []), this.each(function() {
//                 var s = !0,
//                     t = null != n && n + "queueHooks",
//                     o = i.timers,
//                     e = r.get(this);
//                 if (t) e[t] && e[t].stop && f(e[t]);
//                 else
//                     for (t in e) e[t] && e[t].stop && pu.test(t) && f(e[t]);
//                 for (t = o.length; t--;) o[t].elem !== this || null != n && o[t].queue !== n || (o[t].anim.stop(u), s = !1, o.splice(t, 1));
//                 !s && u || i.dequeue(this, n)
//             })
//         },
//         finish: function(n) {
//             return n !== !1 && (n = n || "fx"), this.each(function() {
//                 var t, e = r.get(this),
//                     u = e[n + "queue"],
//                     o = e[n + "queueHooks"],
//                     f = i.timers,
//                     s = u ? u.length : 0;
//                 for (e.finish = !0, i.queue(this, n, []), o && o.stop && o.stop.call(this, !0), t = f.length; t--;) f[t].elem === this && f[t].queue === n && (f[t].anim.stop(!0), f.splice(t, 1));
//                 for (t = 0; t < s; t++) u[t] && u[t].finish && u[t].finish.call(this);
//                 delete e.finish
//             })
//         }
//     });
//     i.each(["toggle", "show", "hide"], function(n, t) {
//         var r = i.fn[t];
//         i.fn[t] = function(n, i, u) {
//             return null == n || "boolean" == typeof n ? r.apply(this, arguments) : this.animate(ni(t, !0), n, i, u)
//         }
//     });
//     i.each({
//         slideDown: ni("show"),
//         slideUp: ni("hide"),
//         slideToggle: ni("toggle"),
//         fadeIn: {
//             opacity: "show"
//         },
//         fadeOut: {
//             opacity: "hide"
//         },
//         fadeToggle: {
//             opacity: "toggle"
//         }
//     }, function(n, t) {
//         i.fn[n] = function(n, i, r) {
//             return this.animate(t, n, i, r)
//         }
//     });
//     i.timers = [];
//     i.fx.tick = function() {
//         var r, n = 0,
//             t = i.timers;
//         for (it = i.now(); n < t.length; n++) r = t[n], r() || t[n] !== r || t.splice(n--, 1);
//         t.length || i.fx.stop();
//         it = void 0
//     };
//     i.fx.timer = function(n) {
//         i.timers.push(n);
//         n() ? i.fx.start() : i.timers.pop()
//     };
//     i.fx.interval = 13;
//     i.fx.start = function() {
//         rt || (rt = n.requestAnimationFrame ? n.requestAnimationFrame(wu) : n.setInterval(i.fx.tick, i.fx.interval))
//     };
//     i.fx.stop = function() {
//         n.cancelAnimationFrame ? n.cancelAnimationFrame(rt) : n.clearInterval(rt);
//         rt = null
//     };
//     i.fx.speeds = {
//         slow: 600,
//         fast: 200,
//         _default: 400
//     };
//     i.fn.delay = function(t, r) {
//         return t = i.fx ? i.fx.speeds[t] || t : t, r = r || "fx", this.queue(r, function(i, r) {
//             var u = n.setTimeout(i, t);
//             r.stop = function() {
//                 n.clearTimeout(u)
//             }
//         })
//     },
//         function() {
//             var n = u.createElement("input"),
//                 t = u.createElement("select"),
//                 i = t.appendChild(u.createElement("option"));
//             n.type = "checkbox";
//             f.checkOn = "" !== n.value;
//             f.optSelected = i.selected;
//             n = u.createElement("input");
//             n.value = "t";
//             n.type = "radio";
//             f.radioValue = "t" === n.value
//         }();
//     ut = i.expr.attrHandle;
//     i.fn.extend({
//         attr: function(n, t) {
//             return a(this, i.attr, n, t, arguments.length > 1)
//         },
//         removeAttr: function(n) {
//             return this.each(function() {
//                 i.removeAttr(this, n)
//             })
//         }
//     });
//     i.extend({
//         attr: function(n, t, r) {
//             var u, f, e = n.nodeType;
//             if (3 !== e && 8 !== e && 2 !== e) return "undefined" == typeof n.getAttribute ? i.prop(n, t, r) : (1 === e && i.isXMLDoc(n) || (f = i.attrHooks[t.toLowerCase()] || (i.expr.match.bool.test(t) ? du : void 0)), void 0 !== r ? null === r ? void i.removeAttr(n, t) : f && "set" in f && void 0 !== (u = f.set(n, r, t)) ? u : (n.setAttribute(t, r + ""), r) : f && "get" in f && null !== (u = f.get(n, t)) ? u : (u = i.find.attr(n, t), null == u ? void 0 : u))
//         },
//         attrHooks: {
//             type: {
//                 set: function(n, t) {
//                     if (!f.radioValue && "radio" === t && i.nodeName(n, "input")) {
//                         var r = n.value;
//                         return n.setAttribute("type", t), r && (n.value = r), t
//                     }
//                 }
//             }
//         },
//         removeAttr: function(n, t) {
//             var i, u = 0,
//                 r = t && t.match(h);
//             if (r && 1 === n.nodeType)
//                 while (i = r[u++]) n.removeAttribute(i)
//         }
//     });
//     du = {
//         set: function(n, t, r) {
//             return t === !1 ? i.removeAttr(n, r) : n.setAttribute(r, r), r
//         }
//     };
//     i.each(i.expr.match.bool.source.match(/\w+/g), function(n, t) {
//         var r = ut[t] || i.find.attr;
//         ut[t] = function(n, t, i) {
//             var f, e, u = t.toLowerCase();
//             return i || (e = ut[u], ut[u] = f, f = null != r(n, t, i) ? u : null, ut[u] = e), f
//         }
//     });
//     gu = /^(?:input|select|textarea|button)$/i;
//     nf = /^(?:a|area)$/i;
//     i.fn.extend({
//         prop: function(n, t) {
//             return a(this, i.prop, n, t, arguments.length > 1)
//         },
//         removeProp: function(n) {
//             return this.each(function() {
//                 delete this[i.propFix[n] || n]
//             })
//         }
//     });
//     i.extend({
//         prop: function(n, t, r) {
//             var f, u, e = n.nodeType;
//             if (3 !== e && 8 !== e && 2 !== e) return 1 === e && i.isXMLDoc(n) || (t = i.propFix[t] || t, u = i.propHooks[t]), void 0 !== r ? u && "set" in u && void 0 !== (f = u.set(n, r, t)) ? f : n[t] = r : u && "get" in u && null !== (f = u.get(n, t)) ? f : n[t]
//         },
//         propHooks: {
//             tabIndex: {
//                 get: function(n) {
//                     var t = i.find.attr(n, "tabindex");
//                     return t ? parseInt(t, 10) : gu.test(n.nodeName) || nf.test(n.nodeName) && n.href ? 0 : -1
//                 }
//             }
//         },
//         propFix: {
//             "for": "htmlFor",
//             "class": "className"
//         }
//     });
//     f.optSelected || (i.propHooks.selected = {
//         get: function(n) {
//             var t = n.parentNode;
//             return t && t.parentNode && t.parentNode.selectedIndex, null
//         },
//         set: function(n) {
//             var t = n.parentNode;
//             t && (t.selectedIndex, t.parentNode && t.parentNode.selectedIndex)
//         }
//     });
//     i.each(["tabIndex", "readOnly", "maxLength", "cellSpacing", "cellPadding", "rowSpan", "colSpan", "useMap", "frameBorder", "contentEditable"], function() {
//         i.propFix[this.toLowerCase()] = this
//     });
//     ti = /[\t\r\n\f]/g;
//     i.fn.extend({
//         addClass: function(n) {
//             var o, t, r, u, f, s, e, c = 0;
//             if (i.isFunction(n)) return this.each(function(t) {
//                 i(this).addClass(n.call(this, t, b(this)))
//             });
//             if ("string" == typeof n && n)
//                 for (o = n.match(h) || []; t = this[c++];)
//                     if (u = b(t), r = 1 === t.nodeType && (" " + u + " ").replace(ti, " ")) {
//                         for (s = 0; f = o[s++];) r.indexOf(" " + f + " ") < 0 && (r += f + " ");
//                         e = i.trim(r);
//                         u !== e && t.setAttribute("class", e)
//                     } return this
//         },
//         removeClass: function(n) {
//             var o, r, t, u, f, s, e, c = 0;
//             if (i.isFunction(n)) return this.each(function(t) {
//                 i(this).removeClass(n.call(this, t, b(this)))
//             });
//             if (!arguments.length) return this.attr("class", "");
//             if ("string" == typeof n && n)
//                 for (o = n.match(h) || []; r = this[c++];)
//                     if (u = b(r), t = 1 === r.nodeType && (" " + u + " ").replace(ti, " ")) {
//                         for (s = 0; f = o[s++];)
//                             while (t.indexOf(" " + f + " ") > -1) t = t.replace(" " + f + " ", " ");
//                         e = i.trim(t);
//                         u !== e && r.setAttribute("class", e)
//                     } return this
//         },
//         toggleClass: function(n, t) {
//             var u = typeof n;
//             return "boolean" == typeof t && "string" === u ? t ? this.addClass(n) : this.removeClass(n) : i.isFunction(n) ? this.each(function(r) {
//                 i(this).toggleClass(n.call(this, r, b(this), t), t)
//             }) : this.each(function() {
//                 var t, e, f, o;
//                 if ("string" === u)
//                     for (e = 0, f = i(this), o = n.match(h) || []; t = o[e++];) f.hasClass(t) ? f.removeClass(t) : f.addClass(t);
//                 else void 0 !== n && "boolean" !== u || (t = b(this), t && r.set(this, "__className__", t), this.setAttribute && this.setAttribute("class", t || n === !1 ? "" : r.get(this, "__className__") || ""))
//             })
//         },
//         hasClass: function(n) {
//             for (var t, r = 0, i = " " + n + " "; t = this[r++];)
//                 if (1 === t.nodeType && (" " + b(t) + " ").replace(ti, " ").indexOf(i) > -1) return !0;
//             return !1
//         }
//     });
//     tf = /\r/g;
//     rf = /[\x20\t\r\n\f]+/g;
//     i.fn.extend({
//         val: function(n) {
//             var t, r, f, u = this[0];
//             return arguments.length ? (f = i.isFunction(n), this.each(function(r) {
//                 var u;
//                 1 === this.nodeType && (u = f ? n.call(this, r, i(this).val()) : n, null == u ? u = "" : "number" == typeof u ? u += "" : i.isArray(u) && (u = i.map(u, function(n) {
//                     return null == n ? "" : n + ""
//                 })), t = i.valHooks[this.type] || i.valHooks[this.nodeName.toLowerCase()], t && "set" in t && void 0 !== t.set(this, u, "value") || (this.value = u))
//             })) : u ? (t = i.valHooks[u.type] || i.valHooks[u.nodeName.toLowerCase()], t && "get" in t && void 0 !== (r = t.get(u, "value")) ? r : (r = u.value, "string" == typeof r ? r.replace(tf, "") : null == r ? "" : r)) : void 0
//         }
//     });
//     i.extend({
//         valHooks: {
//             option: {
//                 get: function(n) {
//                     var t = i.find.attr(n, "value");
//                     return null != t ? t : i.trim(i.text(n)).replace(rf, " ")
//                 }
//             },
//             select: {
//                 get: function(n) {
//                     for (var e, t, o = n.options, r = n.selectedIndex, u = "select-one" === n.type, s = u ? null : [], h = u ? r + 1 : o.length, f = r < 0 ? h : u ? r : 0; f < h; f++)
//                         if (t = o[f], (t.selected || f === r) && !t.disabled && (!t.parentNode.disabled || !i.nodeName(t.parentNode, "optgroup"))) {
//                             if (e = i(t).val(), u) return e;
//                             s.push(e)
//                         } return s
//                 },
//                 set: function(n, t) {
//                     for (var u, r, f = n.options, e = i.makeArray(t), o = f.length; o--;) r = f[o], (r.selected = i.inArray(i.valHooks.option.get(r), e) > -1) && (u = !0);
//                     return u || (n.selectedIndex = -1), e
//                 }
//             }
//         }
//     });
//     i.each(["radio", "checkbox"], function() {
//         i.valHooks[this] = {
//             set: function(n, t) {
//                 if (i.isArray(t)) return n.checked = i.inArray(i(n).val(), t) > -1
//             }
//         };
//         f.checkOn || (i.valHooks[this].get = function(n) {
//             return null === n.getAttribute("value") ? "on" : n.value
//         })
//     });
//     ci = /^(?:focusinfocus|focusoutblur)$/;
//     i.extend(i.event, {
//         trigger: function(t, f, e, o) {
//             var w, s, c, b, a, v, l, p = [e || u],
//                 h = vt.call(t, "type") ? t.type : t,
//                 y = vt.call(t, "namespace") ? t.namespace.split(".") : [];
//             if (s = c = e = e || u, 3 !== e.nodeType && 8 !== e.nodeType && !ci.test(h + i.event.triggered) && (h.indexOf(".") > -1 && (y = h.split("."), h = y.shift(), y.sort()), a = h.indexOf(":") < 0 && "on" + h, t = t[i.expando] ? t : new i.Event(h, "object" == typeof t && t), t.isTrigger = o ? 2 : 3, t.namespace = y.join("."), t.rnamespace = t.namespace ? new RegExp("(^|\\.)" + y.join("\\.(?:.*\\.|)") + "(\\.|$)") : null, t.result = void 0, t.target || (t.target = e), f = null == f ? [t] : i.makeArray(f, [t]), l = i.event.special[h] || {}, o || !l.trigger || l.trigger.apply(e, f) !== !1)) {
//                 if (!o && !l.noBubble && !i.isWindow(e)) {
//                     for (b = l.delegateType || h, ci.test(b + h) || (s = s.parentNode); s; s = s.parentNode) p.push(s), c = s;
//                     c === (e.ownerDocument || u) && p.push(c.defaultView || c.parentWindow || n)
//                 }
//                 for (w = 0;
//                      (s = p[w++]) && !t.isPropagationStopped();) t.type = w > 1 ? b : l.bindType || h, v = (r.get(s, "events") || {})[t.type] && r.get(s, "handle"), v && v.apply(s, f), v = a && s[a], v && v.apply && et(s) && (t.result = v.apply(s, f), t.result === !1 && t.preventDefault());
//                 return t.type = h, o || t.isDefaultPrevented() || l._default && l._default.apply(p.pop(), f) !== !1 || !et(e) || a && i.isFunction(e[h]) && !i.isWindow(e) && (c = e[a], c && (e[a] = null), i.event.triggered = h, e[h](), i.event.triggered = void 0, c && (e[a] = c)), t.result
//             }
//         },
//         simulate: function(n, t, r) {
//             var u = i.extend(new i.Event, r, {
//                 type: n,
//                 isSimulated: !0
//             });
//             i.event.trigger(u, null, t)
//         }
//     });
//     i.fn.extend({
//         trigger: function(n, t) {
//             return this.each(function() {
//                 i.event.trigger(n, t, this)
//             })
//         },
//         triggerHandler: function(n, t) {
//             var r = this[0];
//             if (r) return i.event.trigger(n, t, r, !0)
//         }
//     });
//     i.each("blur focus focusin focusout resize scroll click dblclick mousedown mouseup mousemove mouseover mouseout mouseenter mouseleave change select submit keydown keypress keyup contextmenu".split(" "), function(n, t) {
//         i.fn[t] = function(n, i) {
//             return arguments.length > 0 ? this.on(t, null, n, i) : this.trigger(t)
//         }
//     });
//     i.fn.extend({
//         hover: function(n, t) {
//             return this.mouseenter(n).mouseleave(t || n)
//         }
//     });
//     f.focusin = "onfocusin" in n;
//     f.focusin || i.each({
//         focus: "focusin",
//         blur: "focusout"
//     }, function(n, t) {
//         var u = function(n) {
//             i.event.simulate(t, n.target, i.event.fix(n))
//         };
//         i.event.special[t] = {
//             setup: function() {
//                 var i = this.ownerDocument || this,
//                     f = r.access(i, t);
//                 f || i.addEventListener(n, u, !0);
//                 r.access(i, t, (f || 0) + 1)
//             },
//             teardown: function() {
//                 var i = this.ownerDocument || this,
//                     f = r.access(i, t) - 1;
//                 f ? r.access(i, t, f) : (i.removeEventListener(n, u, !0), r.remove(i, t))
//             }
//         }
//     });
//     var ct = n.location,
//         uf = i.now(),
//         li = /\?/;
//     i.parseXML = function(t) {
//         var r;
//         if (!t || "string" != typeof t) return null;
//         try {
//             r = (new n.DOMParser).parseFromString(t, "text/xml")
//         } catch (u) {
//             r = void 0
//         }
//         return r && !r.getElementsByTagName("parsererror").length || i.error("Invalid XML: " + t), r
//     };
//     var ke = /\[\]$/,
//         ff = /\r?\n/g,
//         de = /^(?:submit|button|image|reset|file)$/i,
//         ge = /^(?:input|select|textarea|keygen)/i;
//     i.param = function(n, t) {
//         var r, u = [],
//             f = function(n, t) {
//                 var r = i.isFunction(t) ? t() : t;
//                 u[u.length] = encodeURIComponent(n) + "=" + encodeURIComponent(null == r ? "" : r)
//             };
//         if (i.isArray(n) || n.jquery && !i.isPlainObject(n)) i.each(n, function() {
//             f(this.name, this.value)
//         });
//         else
//             for (r in n) ai(r, n[r], t, f);
//         return u.join("&")
//     };
//     i.fn.extend({
//         serialize: function() {
//             return i.param(this.serializeArray())
//         },
//         serializeArray: function() {
//             return this.map(function() {
//                 var n = i.prop(this, "elements");
//                 return n ? i.makeArray(n) : this
//             }).filter(function() {
//                 var n = this.type;
//                 return this.name && !i(this).is(":disabled") && ge.test(this.nodeName) && !de.test(n) && (this.checked || !wr.test(n))
//             }).map(function(n, t) {
//                 var r = i(this).val();
//                 return null == r ? null : i.isArray(r) ? i.map(r, function(n) {
//                     return {
//                         name: t.name,
//                         value: n.replace(ff, "\r\n")
//                     }
//                 }) : {
//                     name: t.name,
//                     value: r.replace(ff, "\r\n")
//                 }
//             }).get()
//         }
//     });
//     var no = /%20/g,
//         to = /#.*$/,
//         io = /([?&])_=[^&]*/,
//         ro = /^(.*?):[ \t]*([^\r\n]*)$/gm,
//         uo = /^(?:GET|HEAD)$/,
//         fo = /^\/\//,
//         ef = {},
//         vi = {},
//         of = "*/".concat("*"),
//         yi = u.createElement("a");
//     return yi.href = ct.href, i.extend({
//         active: 0,
//         lastModified: {},
//         etag: {},
//         ajaxSettings: {
//             url: ct.href,
//             type: "GET",
//             isLocal: /^(?:about|app|app-storage|.+-extension|file|res|widget):$/.test(ct.protocol),
//             global: !0,
//             processData: !0,
//             async: !0,
//             contentType: "application/x-www-form-urlencoded; charset=UTF-8",
//             accepts: {
//                 "*": of ,
//                 text: "text/plain",
//                 html: "text/html",
//                 xml: "application/xml, text/xml",
//                 json: "application/json, text/javascript"
//             },
//             contents: {
//                 xml: /\bxml\b/,
//                 html: /\bhtml/,
//                 json: /\bjson\b/
//             },
//             responseFields: {
//                 xml: "responseXML",
//                 text: "responseText",
//                 json: "responseJSON"
//             },
//             converters: {
//                 "* text": String,
//                 "text html": !0,
//                 "text json": JSON.parse,
//                 "text xml": i.parseXML
//             },
//             flatOptions: {
//                 url: !0,
//                 context: !0
//             }
//         },
//         ajaxSetup: function(n, t) {
//             return t ? pi(pi(n, i.ajaxSettings), t) : pi(i.ajaxSettings, n)
//         },
//         ajaxPrefilter: sf(ef),
//         ajaxTransport: sf(vi),
//         ajax: function(t, r) {
//             function b(t, r, u, h) {
//                 var y, rt, g, p, b, l = r;
//                 s || (s = !0, d && n.clearTimeout(d), a = void 0, k = h || "", e.readyState = t > 0 ? 4 : 0, y = t >= 200 && t < 300 || 304 === t, u && (p = eo(f, e, u)), p = oo(f, p, e, y), y ? (f.ifModified && (b = e.getResponseHeader("Last-Modified"), b && (i.lastModified[o] = b), b = e.getResponseHeader("etag"), b && (i.etag[o] = b)), 204 === t || "HEAD" === f.type ? l = "nocontent" : 304 === t ? l = "notmodified" : (l = p.state, rt = p.data, g = p.error, y = !g)) : (g = l, !t && l || (l = "error", t < 0 && (t = 0))), e.status = t, e.statusText = (r || l) + "", y ? tt.resolveWith(c, [rt, l, e]) : tt.rejectWith(c, [e, l, g]), e.statusCode(w), w = void 0, v && nt.trigger(y ? "ajaxSuccess" : "ajaxError", [e, f, y ? rt : g]), it.fireWith(c, [e, l]), v && (nt.trigger("ajaxComplete", [e, f]), --i.active || i.event.trigger("ajaxStop")))
//             }
//             "object" == typeof t && (r = t, t = void 0);
//             r = r || {};
//             var a, o, k, y, d, l, s, v, g, p, f = i.ajaxSetup({}, r),
//                 c = f.context || f,
//                 nt = f.context && (c.nodeType || c.jquery) ? i(c) : i.event,
//                 tt = i.Deferred(),
//                 it = i.Callbacks("once memory"),
//                 w = f.statusCode || {},
//                 rt = {},
//                 ut = {},
//                 ft = "canceled",
//                 e = {
//                     readyState: 0,
//                     getResponseHeader: function(n) {
//                         var t;
//                         if (s) {
//                             if (!y)
//                                 for (y = {}; t = ro.exec(k);) y[t[1].toLowerCase()] = t[2];
//                             t = y[n.toLowerCase()]
//                         }
//                         return null == t ? null : t
//                     },
//                     getAllResponseHeaders: function() {
//                         return s ? k : null
//                     },
//                     setRequestHeader: function(n, t) {
//                         return null == s && (n = ut[n.toLowerCase()] = ut[n.toLowerCase()] || n, rt[n] = t), this
//                     },
//                     overrideMimeType: function(n) {
//                         return null == s && (f.mimeType = n), this
//                     },
//                     statusCode: function(n) {
//                         var t;
//                         if (n)
//                             if (s) e.always(n[e.status]);
//                             else
//                                 for (t in n) w[t] = [w[t], n[t]];
//                         return this
//                     },
//                     abort: function(n) {
//                         var t = n || ft;
//                         return a && a.abort(t), b(0, t), this
//                     }
//                 };
//             if (tt.promise(e), f.url = ((t || f.url || ct.href) + "").replace(fo, ct.protocol + "//"), f.type = r.method || r.type || f.method || f.type, f.dataTypes = (f.dataType || "*").toLowerCase().match(h) || [""], null == f.crossDomain) {
//                 l = u.createElement("a");
//                 try {
//                     l.href = f.url;
//                     l.href = l.href;
//                     f.crossDomain = yi.protocol + "//" + yi.host != l.protocol + "//" + l.host
//                 } catch (et) {
//                     f.crossDomain = !0
//                 }
//             }
//             if (f.data && f.processData && "string" != typeof f.data && (f.data = i.param(f.data, f.traditional)), hf(ef, f, r, e), s) return e;
//             v = i.event && f.global;
//             v && 0 == i.active++ && i.event.trigger("ajaxStart");
//             f.type = f.type.toUpperCase();
//             f.hasContent = !uo.test(f.type);
//             o = f.url.replace(to, "");
//             f.hasContent ? f.data && f.processData && 0 === (f.contentType || "").indexOf("application/x-www-form-urlencoded") && (f.data = f.data.replace(no, "+")) : (p = f.url.slice(o.length), f.data && (o += (li.test(o) ? "&" : "?") + f.data, delete f.data), f.cache === !1 && (o = o.replace(io, ""), p = (li.test(o) ? "&" : "?") + "_=" + uf++ + p), f.url = o + p);
//             f.ifModified && (i.lastModified[o] && e.setRequestHeader("If-Modified-Since", i.lastModified[o]), i.etag[o] && e.setRequestHeader("If-None-Match", i.etag[o]));
//             (f.data && f.hasContent && f.contentType !== !1 || r.contentType) && e.setRequestHeader("Content-Type", f.contentType);
//             e.setRequestHeader("Accept", f.dataTypes[0] && f.accepts[f.dataTypes[0]] ? f.accepts[f.dataTypes[0]] + ("*" !== f.dataTypes[0] ? ", " + of +"; q=0.01" : "") : f.accepts["*"]);
//             for (g in f.headers) e.setRequestHeader(g, f.headers[g]);
//             if (f.beforeSend && (f.beforeSend.call(c, e, f) === !1 || s)) return e.abort();
//             if (ft = "abort", it.add(f.complete), e.done(f.success), e.fail(f.error), a = hf(vi, f, r, e)) {
//                 if (e.readyState = 1, v && nt.trigger("ajaxSend", [e, f]), s) return e;
//                 f.async && f.timeout > 0 && (d = n.setTimeout(function() {
//                     e.abort("timeout")
//                 }, f.timeout));
//                 try {
//                     s = !1;
//                     a.send(rt, b)
//                 } catch (et) {
//                     if (s) throw et;
//                     b(-1, et)
//                 }
//             } else b(-1, "No Transport");
//             return e
//         },
//         getJSON: function(n, t, r) {
//             return i.get(n, t, r, "json")
//         },
//         getScript: function(n, t) {
//             return i.get(n, void 0, t, "script")
//         }
//     }), i.each(["get", "post"], function(n, t) {
//         i[t] = function(n, r, u, f) {
//             return i.isFunction(r) && (f = f || u, u = r, r = void 0), i.ajax(i.extend({
//                 url: n,
//                 type: t,
//                 dataType: f,
//                 data: r,
//                 success: u
//             }, i.isPlainObject(n) && n))
//         }
//     }), i._evalUrl = function(n) {
//         return i.ajax({
//             url: n,
//             type: "GET",
//             dataType: "script",
//             cache: !0,
//             async: !1,
//             global: !1,
//             throws: !0
//         })
//     }, i.fn.extend({
//         wrapAll: function(n) {
//             var t;
//             return this[0] && (i.isFunction(n) && (n = n.call(this[0])), t = i(n, this[0].ownerDocument).eq(0).clone(!0), this[0].parentNode && t.insertBefore(this[0]), t.map(function() {
//                 for (var n = this; n.firstElementChild;) n = n.firstElementChild;
//                 return n
//             }).append(this)), this
//         },
//         wrapInner: function(n) {
//             return i.isFunction(n) ? this.each(function(t) {
//                 i(this).wrapInner(n.call(this, t))
//             }) : this.each(function() {
//                 var t = i(this),
//                     r = t.contents();
//                 r.length ? r.wrapAll(n) : t.append(n)
//             })
//         },
//         wrap: function(n) {
//             var t = i.isFunction(n);
//             return this.each(function(r) {
//                 i(this).wrapAll(t ? n.call(this, r) : n)
//             })
//         },
//         unwrap: function(n) {
//             return this.parent(n).not("body").each(function() {
//                 i(this).replaceWith(this.childNodes)
//             }), this
//         }
//     }), i.expr.pseudos.hidden = function(n) {
//         return !i.expr.pseudos.visible(n)
//     }, i.expr.pseudos.visible = function(n) {
//         return !!(n.offsetWidth || n.offsetHeight || n.getClientRects().length)
//     }, i.ajaxSettings.xhr = function() {
//         try {
//             return new n.XMLHttpRequest
//         } catch (t) {}
//     }, cf = {
//         0: 200,
//         1223: 204
//     }, ft = i.ajaxSettings.xhr(), f.cors = !!ft && "withCredentials" in ft, f.ajax = ft = !!ft, i.ajaxTransport(function(t) {
//         var i, r;
//         if (f.cors || ft && !t.crossDomain) return {
//             send: function(u, f) {
//                 var o, e = t.xhr();
//                 if (e.open(t.type, t.url, t.async, t.username, t.password), t.xhrFields)
//                     for (o in t.xhrFields) e[o] = t.xhrFields[o];
//                 t.mimeType && e.overrideMimeType && e.overrideMimeType(t.mimeType);
//                 t.crossDomain || u["X-Requested-With"] || (u["X-Requested-With"] = "XMLHttpRequest");
//                 for (o in u) e.setRequestHeader(o, u[o]);
//                 i = function(n) {
//                     return function() {
//                         i && (i = r = e.onload = e.onerror = e.onabort = e.onreadystatechange = null, "abort" === n ? e.abort() : "error" === n ? "number" != typeof e.status ? f(0, "error") : f(e.status, e.statusText) : f(cf[e.status] || e.status, e.statusText, "text" !== (e.responseType || "text") || "string" != typeof e.responseText ? {
//                             binary: e.response
//                         } : {
//                             text: e.responseText
//                         }, e.getAllResponseHeaders()))
//                     }
//                 };
//                 e.onload = i();
//                 r = e.onerror = i("error");
//                 void 0 !== e.onabort ? e.onabort = r : e.onreadystatechange = function() {
//                     4 === e.readyState && n.setTimeout(function() {
//                         i && r()
//                     })
//                 };
//                 i = i("abort");
//                 try {
//                     e.send(t.hasContent && t.data || null)
//                 } catch (s) {
//                     if (i) throw s;
//                 }
//             },
//             abort: function() {
//                 i && i()
//             }
//         }
//     }), i.ajaxPrefilter(function(n) {
//         n.crossDomain && (n.contents.script = !1)
//     }), i.ajaxSetup({
//         accepts: {
//             script: "text/javascript, application/javascript, application/ecmascript, application/x-ecmascript"
//         },
//         contents: {
//             script: /\b(?:java|ecma)script\b/
//         },
//         converters: {
//             "text script": function(n) {
//                 return i.globalEval(n), n
//             }
//         }
//     }), i.ajaxPrefilter("script", function(n) {
//         void 0 === n.cache && (n.cache = !1);
//         n.crossDomain && (n.type = "GET")
//     }), i.ajaxTransport("script", function(n) {
//         if (n.crossDomain) {
//             var r, t;
//             return {
//                 send: function(f, e) {
//                     r = i("<script>").prop({
//                         charset: n.scriptCharset,
//                         src: n.url
//                     }).on("load error", t = function(n) {
//                         r.remove();
//                         t = null;
//                         n && e("error" === n.type ? 404 : 200, n.type)
//                     });
//                     u.head.appendChild(r[0])
//                 },
//                 abort: function() {
//                     t && t()
//                 }
//             }
//         }
//     }), wi = [], ii = /(=)\?(?=&|$)|\?\?/, i.ajaxSetup({
//         jsonp: "callback",
//         jsonpCallback: function() {
//             var n = wi.pop() || i.expando + "_" + uf++;
//             return this[n] = !0, n
//         }
//     }), i.ajaxPrefilter("json jsonp", function(t, r, u) {
//         var f, e, o, s = t.jsonp !== !1 && (ii.test(t.url) ? "url" : "string" == typeof t.data && 0 === (t.contentType || "").indexOf("application/x-www-form-urlencoded") && ii.test(t.data) && "data");
//         if (s || "jsonp" === t.dataTypes[0]) return f = t.jsonpCallback = i.isFunction(t.jsonpCallback) ? t.jsonpCallback() : t.jsonpCallback, s ? t[s] = t[s].replace(ii, "$1" + f) : t.jsonp !== !1 && (t.url += (li.test(t.url) ? "&" : "?") + t.jsonp + "=" + f), t.converters["script json"] = function() {
//             return o || i.error(f + " was not called"), o[0]
//         }, t.dataTypes[0] = "json", e = n[f], n[f] = function() {
//             o = arguments
//         }, u.always(function() {
//             void 0 === e ? i(n).removeProp(f) : n[f] = e;
//             t[f] && (t.jsonpCallback = r.jsonpCallback, wi.push(f));
//             o && i.isFunction(e) && e(o[0]);
//             o = e = void 0
//         }), "script"
//     }), f.createHTMLDocument = function() {
//         var n = u.implementation.createHTMLDocument("").body;
//         return n.innerHTML = "<form><\/form><form><\/form>", 2 === n.childNodes.length
//     }(), i.parseHTML = function(n, t, r) {
//         if ("string" != typeof n) return [];
//         "boolean" == typeof t && (r = t, t = !1);
//         var s, e, o;
//         return t || (f.createHTMLDocument ? (t = u.implementation.createHTMLDocument(""), s = t.createElement("base"), s.href = u.location.href, t.head.appendChild(s)) : t = u), e = rr.exec(n), o = !r && [], e ? [t.createElement(e[1])] : (e = gr([n], t, o), o && o.length && i(o).remove(), i.merge([], e.childNodes))
//     }, i.fn.load = function(n, t, r) {
//         var u, o, s, f = this,
//             e = n.indexOf(" ");
//         return e > -1 && (u = i.trim(n.slice(e)), n = n.slice(0, e)), i.isFunction(t) ? (r = t, t = void 0) : t && "object" == typeof t && (o = "POST"), f.length > 0 && i.ajax({
//             url: n,
//             type: o || "GET",
//             dataType: "html",
//             data: t
//         }).done(function(n) {
//             s = arguments;
//             f.html(u ? i("<div>").append(i.parseHTML(n)).find(u) : n)
//         }).always(r && function(n, t) {
//             f.each(function() {
//                 r.apply(this, s || [n.responseText, t, n])
//             })
//         }), this
//     }, i.each(["ajaxStart", "ajaxStop", "ajaxComplete", "ajaxError", "ajaxSuccess", "ajaxSend"], function(n, t) {
//         i.fn[t] = function(n) {
//             return this.on(t, n)
//         }
//     }), i.expr.pseudos.animated = function(n) {
//         return i.grep(i.timers, function(t) {
//             return n === t.elem
//         }).length
//     }, i.offset = {
//         setOffset: function(n, t, r) {
//             var e, o, s, h, u, c, v, l = i.css(n, "position"),
//                 a = i(n),
//                 f = {};
//             "static" === l && (n.style.position = "relative");
//             u = a.offset();
//             s = i.css(n, "top");
//             c = i.css(n, "left");
//             v = ("absolute" === l || "fixed" === l) && (s + c).indexOf("auto") > -1;
//             v ? (e = a.position(), h = e.top, o = e.left) : (h = parseFloat(s) || 0, o = parseFloat(c) || 0);
//             i.isFunction(t) && (t = t.call(n, r, i.extend({}, u)));
//             null != t.top && (f.top = t.top - u.top + h);
//             null != t.left && (f.left = t.left - u.left + o);
//             "using" in t ? t.using.call(n, f) : a.css(f)
//         }
//     }, i.fn.extend({
//         offset: function(n) {
//             if (arguments.length) return void 0 === n ? this : this.each(function(t) {
//                 i.offset.setOffset(this, n, t)
//             });
//             var u, f, t, e, r = this[0];
//             if (r) return r.getClientRects().length ? (t = r.getBoundingClientRect(), t.width || t.height ? (e = r.ownerDocument, f = lf(e), u = e.documentElement, {
//                 top: t.top + f.pageYOffset - u.clientTop,
//                 left: t.left + f.pageXOffset - u.clientLeft
//             }) : t) : {
//                 top: 0,
//                 left: 0
//             }
//         },
//         position: function() {
//             if (this[0]) {
//                 var t, r, u = this[0],
//                     n = {
//                         top: 0,
//                         left: 0
//                     };
//                 return "fixed" === i.css(u, "position") ? r = u.getBoundingClientRect() : (t = this.offsetParent(), r = this.offset(), i.nodeName(t[0], "html") || (n = t.offset()), n = {
//                     top: n.top + i.css(t[0], "borderTopWidth", !0),
//                     left: n.left + i.css(t[0], "borderLeftWidth", !0)
//                 }), {
//                     top: r.top - n.top - i.css(u, "marginTop", !0),
//                     left: r.left - n.left - i.css(u, "marginLeft", !0)
//                 }
//             }
//         },
//         offsetParent: function() {
//             return this.map(function() {
//                 for (var n = this.offsetParent; n && "static" === i.css(n, "position");) n = n.offsetParent;
//                 return n || kt
//             })
//         }
//     }), i.each({
//         scrollLeft: "pageXOffset",
//         scrollTop: "pageYOffset"
//     }, function(n, t) {
//         var r = "pageYOffset" === t;
//         i.fn[n] = function(i) {
//             return a(this, function(n, i, u) {
//                 var f = lf(n);
//                 return void 0 === u ? f ? f[t] : n[i] : void(f ? f.scrollTo(r ? f.pageXOffset : u, r ? u : f.pageYOffset) : n[i] = u)
//             }, n, i, arguments.length)
//         }
//     }), i.each(["top", "left"], function(n, t) {
//         i.cssHooks[t] = eu(f.pixelPosition, function(n, r) {
//             if (r) return r = ht(n, t), hi.test(r) ? i(n).position()[t] + "px" : r
//         })
//     }), i.each({
//         Height: "height",
//         Width: "width"
//     }, function(n, t) {
//         i.each({
//             padding: "inner" + n,
//             content: t,
//             "": "outer" + n
//         }, function(r, u) {
//             i.fn[u] = function(f, e) {
//                 var o = arguments.length && (r || "boolean" != typeof f),
//                     s = r || (f === !0 || e === !0 ? "margin" : "border");
//                 return a(this, function(t, r, f) {
//                     var e;
//                     return i.isWindow(t) ? 0 === u.indexOf("outer") ? t["inner" + n] : t.document.documentElement["client" + n] : 9 === t.nodeType ? (e = t.documentElement, Math.max(t.body["scroll" + n], e["scroll" + n], t.body["offset" + n], e["offset" + n], e["client" + n])) : void 0 === f ? i.css(t, r, s) : i.style(t, r, f, s)
//                 }, t, o ? f : void 0, o)
//             }
//         })
//     }), i.fn.extend({
//         bind: function(n, t, i) {
//             return this.on(n, null, t, i)
//         },
//         unbind: function(n, t) {
//             return this.off(n, null, t)
//         },
//         delegate: function(n, t, i, r) {
//             return this.on(t, n, i, r)
//         },
//         undelegate: function(n, t, i) {
//             return 1 === arguments.length ? this.off(n, "**") : this.off(t, n || "**", i)
//         }
//     }), i.parseJSON = JSON.parse, "function" == typeof define && define.amd && define("jquery", [], function() {
//         return i
//     }), af = n.jQuery, vf = n.$, i.noConflict = function(t) {
//         return n.$ === i && (n.$ = vf), t && n.jQuery === i && (n.jQuery = af), i
//     }, t || (n.jQuery = n.$ = i), i
// }), typeof jQuery == "undefined") throw new Error("jquery-confirm requires jQuery");

if (function(n, t) {
    n.fn.confirm = function(t, i) {
        return typeof t == "undefined" && (t = {}), typeof t == "string" && (t = {
            content: t,
            title: i ? i : !1
        }), n(this).each(function() {
            var i = n(this);
            i.on("click", function(r) {
                var u, f, e;
                r.preventDefault();
                u = n.extend({}, t);
                i.attr("data-title") && (u.title = i.attr("data-title"));
                i.attr("data-content") && (u.content = i.attr("data-content"));
                typeof u.buttons == "undefined" && (u.buttons = {});
                u.$target = i;
                i.attr("href") && Object.keys(u.buttons).length == 0 && (f = n.extend(!0, {}, jconfirm.pluginDefaults.defaultButtons, (jconfirm.defaults || {}).defaultButtons || {}), e = Object.keys(f)[0], u.buttons = f, u.buttons[e].action = function() {
                    location.href = i.attr("href")
                });
                u.closeIcon = !1;
                n.confirm(u)
            })
        }), n(this)
    };
    n.confirm = function(t, i) {
        if (typeof t == "undefined" && (t = {}), typeof t == "string" && (t = {
            content: t,
            title: i ? i : !1
        }), typeof t.buttons != "object" && (t.buttons = {}), Object.keys(t.buttons).length == 0) {
            var r = n.extend(!0, {}, jconfirm.pluginDefaults.defaultButtons, (jconfirm.defaults || {}).defaultButtons || {});
            t.buttons = r
        }
        return jconfirm(t)
    };
    n.alert = function(t, i) {
        if (typeof t == "undefined" && (t = {}), typeof t == "string" && (t = {
            content: t,
            title: i ? i : !1
        }), typeof t.buttons != "object" && (t.buttons = {}), Object.keys(t.buttons).length == 0) {
            var r = n.extend(!0, {}, jconfirm.pluginDefaults.defaultButtons, (jconfirm.defaults || {}).defaultButtons || {}),
                u = Object.keys(r)[0];
            t.buttons[u] = r[u]
        }
        return jconfirm(t)
    };
    n.dialog = function(n, t) {
        return typeof n == "undefined" && (n = {}), typeof n == "string" && (n = {
            content: n,
            title: t ? t : !1,
            closeIcon: function() {}
        }), n.buttons = {}, typeof n.closeIcon == "undefined" && (n.closeIcon = function() {}), n.confirmKeys = [13], jconfirm(n)
    };
    jconfirm = function(t) {
        var i, r;
        return typeof t == "undefined" && (t = {}), i = n.extend(!0, {}, jconfirm.pluginDefaults), jconfirm.defaults && (i = n.extend(!0, i, jconfirm.defaults)), i = n.extend(!0, {}, i, t), r = new Jconfirm(i), jconfirm.instances.push(r), r
    };
    Jconfirm = function(t) {
        n.extend(this, t);
        this._init()
    };
    Jconfirm.prototype = {
        _init: function() {
            var t = this;
            jconfirm.instances.length || (jconfirm.lastFocused = n("body").find(":focus"));
            this._id = Math.round(Math.random() * 99999);
            this.lazyOpen || setTimeout(function() {
                t.open()
            }, 0)
        },
        _buildHTML: function() {
            var t = this,
                i, r;
            this._parseAnimation(this.animation, "o");
            this._parseAnimation(this.closeAnimation, "c");
            this._parseBgDismissAnimation(this.backgroundDismissAnimation);
            this._parseColumnClass(this.columnClass);
            this._parseTheme(this.theme);
            this._parseType(this.type);
            i = n(this.template);
            i.find(".jconfirm-box").addClass(this.animationParsed).addClass(this.backgroundDismissAnimationParsed).addClass(this.typeParsed);
            this.typeAnimated && i.find(".jconfirm-box").addClass("jconfirm-type-animated");
            this.useBootstrap ? (i.find(".jc-bs3-row").addClass(this.bootstrapClasses.row), i.find(".jc-bs3-row").addClass("justify-content-md-center justify-content-sm-center justify-content-xs-center justify-content-lg-center"), i.find(".jconfirm-box-container").addClass(this.columnClassParsed), this.containerFluid ? i.find(".jc-bs3-container").addClass(this.bootstrapClasses.containerFluid) : i.find(".jc-bs3-container").addClass(this.bootstrapClasses.container)) : i.find(".jconfirm-box").css("width", this.boxWidth);
            this.titleClass && i.find(".jconfirm-title-c").addClass(this.titleClass);
            i.addClass(this.themeParsed);
            r = "jconfirm-box" + this._id;
            i.find(".jconfirm-box").attr("aria-labelledby", r).attr("tabindex", -1);
            i.find(".jconfirm-content").attr("id", r);
            this.bgOpacity != null && i.find(".jconfirm-bg").css("opacity", this.bgOpacity);
            this.rtl && i.addClass("jconfirm-rtl");
            this.$el = i.appendTo(this.container);
            this.$jconfirmBoxContainer = this.$el.find(".jconfirm-box-container");
            this.$jconfirmBox = this.$body = this.$el.find(".jconfirm-box");
            this.$jconfirmBg = this.$el.find(".jconfirm-bg");
            this.$title = this.$el.find(".jconfirm-title");
            this.$titleContainer = this.$el.find(".jconfirm-title-c");
            this.$content = this.$el.find("div.jconfirm-content");
            this.$contentPane = this.$el.find(".jconfirm-content-pane");
            this.$icon = this.$el.find(".jconfirm-icon-c");
            this.$closeIcon = this.$el.find(".jconfirm-closeIcon");
            this.$btnc = this.$el.find(".jconfirm-buttons");
            this.$scrollPane = this.$el.find(".jconfirm-scrollpane");
            this._contentReady = n.Deferred();
            this._modalReady = n.Deferred();
            this.setTitle();
            this.setIcon();
            this._setButtons();
            this._parseContent();
            this.initDraggable();
            this.isAjax && this.showLoading(!1);
            n.when(this._contentReady, this._modalReady).then(function() {
                t.isAjaxLoading ? setTimeout(function() {
                    t.isAjaxLoading = !1;
                    t.setContent();
                    t.setTitle();
                    t.setIcon();
                    setTimeout(function() {
                        t.hideLoading(!1)
                    }, 100);
                    typeof t.onContentReady == "function" && t.onContentReady()
                }, 50) : (t.setContent(), t.setTitle(), t.setIcon(), typeof t.onContentReady == "function" && t.onContentReady());
                t.autoClose && t._startCountDown()
            });
            t._contentHash = this._hash(t.$content.html());
            t._contentHeight = this.$content.height();
            this._watchContent();
            this.setDialogCenter();
            this.animation == "none" && (this.animationSpeed = 1, this.animationBounce = 1);
            this.$body.css(this._getCSS(this.animationSpeed, this.animationBounce));
            this.$contentPane.css(this._getCSS(this.animationSpeed, 1));
            this.$jconfirmBg.css(this._getCSS(this.animationSpeed, 1))
        },
        _typePrefix: "jconfirm-type-",
        typeParsed: "",
        _parseType: function(n) {
            this.typeParsed = this._typePrefix + n
        },
        setType: function(n) {
            var t = this.typeParsed;
            this._parseType(n);
            this.$jconfirmBox.removeClass(t).addClass(this.typeParsed)
        },
        themeParsed: "",
        _themePrefix: "jconfirm-",
        setTheme: function(n) {
            var t = this.theme;
            this.theme = n || this.theme;
            this._parseTheme(this.theme);
            t && this.$el.removeClass(t);
            this.$el.addClass(this.themeParsed);
            this.theme = n
        },
        _parseTheme: function(t) {
            var i = this;
            t = t.split(",");
            n.each(t, function(r, u) {
                u.indexOf(i._themePrefix) == -1 && (t[r] = i._themePrefix + n.trim(u))
            });
            this.themeParsed = t.join(" ").toLowerCase()
        },
        backgroundDismissAnimationParsed: "",
        _bgDismissPrefix: "jconfirm-hilight-",
        _parseBgDismissAnimation: function(t) {
            var i = t.split(","),
                r = this;
            n.each(i, function(t, u) {
                u.indexOf(r._bgDismissPrefix) == -1 && (i[t] = r._bgDismissPrefix + n.trim(u))
            });
            this.backgroundDismissAnimationParsed = i.join(" ").toLowerCase()
        },
        animationParsed: "",
        closeAnimationParsed: "",
        _animationPrefix: "jconfirm-animation-",
        setAnimation: function(n) {
            this.animation = n || this.animation;
            this._parseAnimation(this.animation, "o")
        },
        _parseAnimation: function(t, i) {
            var r, f, u;
            return i = i || "o", r = t.split(","), f = this, n.each(r, function(t, i) {
                i.indexOf(f._animationPrefix) == -1 && (r[t] = f._animationPrefix + n.trim(i))
            }), u = r.join(" ").toLowerCase(), i == "o" ? this.animationParsed = u : this.closeAnimationParsed = u, u
        },
        setCloseAnimation: function(n) {
            this.closeAnimation = n || this.closeAnimation;
            this._parseAnimation(this.closeAnimation, "c")
        },
        setAnimationSpeed: function(n) {
            this.animationSpeed = n || this.animationSpeed
        },
        columnClassParsed: "",
        setColumnClass: function(n) {
            if (!this.useBootstrap) {
                console.warn("cannot set columnClass, useBootstrap is set to false");
                return
            }
            this.columnClass = n || this.columnClass;
            this._parseColumnClass(this.columnClass);
            this.$jconfirmBoxContainer.addClass(this.columnClassParsed)
        },
        setBoxWidth: function() {
            if (this.useBootstrap) {
                console.warn("cannot set boxWidth, useBootstrap is set to true");
                return
            }
            this.$jconfirmBox.css("width", this.boxWidth)
        },
        _parseColumnClass: function(n) {
            n = n.toLowerCase();
            var t;
            switch (n) {
                case "xl":
                case "xlarge":
                    t = "col-md-12";
                    break;
                case "l":
                case "large":
                    t = "col-md-8 col-md-offset-2";
                    break;
                case "m":
                case "medium":
                    t = "col-md-6 col-md-offset-3";
                    break;
                case "s":
                case "small":
                    t = "col-md-4 col-md-offset-4";
                    break;
                case "xs":
                case "xsmall":
                    t = "col-md-2 col-md-offset-5";
                    break;
                default:
                    t = n
            }
            this.columnClassParsed = t
        },
        initDraggable: function() {
            var i = this,
                r = this.$titleContainer;
            if (this.resetDrag(), this.draggable) {
                r.addClass("jconfirm-hand");
                r.on("mousedown", function(n) {
                    i.mouseX = n.clientX;
                    i.mouseY = n.clientY;
                    i.isDrag = !0
                });
                n(t).on("mousemove." + this._id, function(n) {
                    i.isDrag && (i.movingX = n.clientX - i.mouseX + i.initialX, i.movingY = n.clientY - i.mouseY + i.initialY, i.setDrag())
                });
                n(t).on("mouseup." + this._id, function() {
                    i.isDrag && (i.isDrag = !1, i.initialX = i.movingX, i.initialY = i.movingY)
                })
            }
        },
        resetDrag: function() {
            this.isDrag = !1;
            this.initialX = 0;
            this.initialY = 0;
            this.movingX = 0;
            this.movingY = 0;
            this.movingXCurrent = 0;
            this.movingYCurrent = 0;
            this.mouseX = 0;
            this.mouseY = 0;
            this.$jconfirmBoxContainer.css("transform", "translate(0px, 0px)")
        },
        setDrag: function() {
            var f, i, e, r, u;
            this.draggable && (this.alignMiddle = !1, this._boxWidth = this.$jconfirmBox.outerWidth(), f = n(t).width(), i = this, (i.movingX % 2 == 0 || i.movingY % 2 == 0) && (e = i._boxTopMargin - i.dragWindowGap, e + i.movingY < 0 ? i.movingY = -e : i.movingYCurrent = i.movingY, r = f / 2 - i._boxWidth / 2, u = f / 2 + i._boxWidth / 2 - i._boxWidth, u -= i.dragWindowGap, r -= i.dragWindowGap, r + i.movingX < 0 ? i.movingX = -r : u - i.movingX < 0 ? i.movingX = u : i.movingXCurrent = i.movingX, i.$jconfirmBoxContainer.css("transform", "translate(" + i.movingX + "px, " + i.movingY + "px)")))
        },
        _hash: function(n) {
            var r = n.toString(),
                t = 0,
                i, u;
            if (r.length == 0) return t;
            for (i = 0; i < r.length; i++) u = r.toString().charCodeAt(i), t = (t << 5) - t + u, t = t & t;
            return t
        },
        _watchContent: function() {
            var n = this;
            this._timer && clearInterval(this._timer);
            this._timer = setInterval(function() {
                var t = n._hash(n.$content.html()),
                    i = n.$content.height();
                (n._contentHash != t || n._contentHeight != i) && (n._contentHash = t, n._contentHeight = i, n.setDialogCenter(), n._imagesLoaded())
            }, this.watchInterval)
        },
        _hilightAnimating: !1,
        _hiLightModal: function() {
            var n = this,
                t;
            this._hilightAnimating || (n.$body.addClass("hilight"), t = 2, this._hilightAnimating = !0, setTimeout(function() {
                n._hilightAnimating = !1;
                n.$body.removeClass("hilight")
            }, t * 1e3))
        },
        _bindEvents: function() {
            var i = this,
                r;
            this.boxClicked = !1;
            this.$scrollPane.click(function() {
                var r, t, n, u;
                i.boxClicked || (r = !1, t = !1, n = typeof i.backgroundDismiss == "function" ? i.backgroundDismiss() : i.backgroundDismiss, typeof n == "string" && typeof i.buttons[n] != "undefined" ? (r = n, t = !1) : t = typeof n == "undefined" || !!n == !0 ? !0 : !1, r && (u = i.buttons[r].action.apply(i), t = typeof u == "undefined" || !!u), t ? i.close() : i._hiLightModal());
                i.boxClicked = !1
            });
            this.$jconfirmBox.click(function() {
                i.boxClicked = !0
            });
            r = !1;
            n(t).on("jcKeyDown." + i._id, function() {
                r || (r = !0)
            });
            n(t).on("keyup." + i._id, function(n) {
                r && (i.reactOnKey(n), r = !1)
            });
            n(t).on("resize." + this._id, function() {
                i.setDialogCenter(!0);
                setTimeout(function() {
                    i.resetDrag()
                }, 100)
            })
        },
        _cubic_bezier: "0.36, 0.55, 0.19",
        _getCSS: function(n, t) {
            return {
                "-webkit-transition-duration": n / 1e3 + "s",
                "transition-duration": n / 1e3 + "s",
                "-webkit-transition-timing-function": "cubic-bezier(" + this._cubic_bezier + ", " + t + ")",
                "transition-timing-function": "cubic-bezier(" + this._cubic_bezier + ", " + t + ")"
            }
        },
        _imagesLoaded: function() {
            var t = this;
            t.imageLoadInterval && clearInterval(t.imageLoadInterval);
            n.each(this.$content.find("img:not(.loaded)"), function(i, r) {
                t.imageLoadInterval = setInterval(function() {
                    var i = n(r).css("height");
                    i !== "0px" && (n(r).addClass("loaded"), clearInterval(t.imageLoadInterval), t.setDialogCenter())
                }, 40)
            })
        },
        _setButtons: function() {
            var t = this,
                i = 0,
                r;
            typeof this.buttons != "object" && (this.buttons = {});
            n.each(this.buttons, function(r, u) {
                i += 1;
                typeof u == "function" && (t.buttons[r] = u = {
                    action: u
                });
                t.buttons[r].text = u.text || r;
                t.buttons[r].btnClass = u.btnClass || "btn-default";
                t.buttons[r].action = u.action || function() {};
                t.buttons[r].keys = u.keys || [];
                t.buttons[r].isHidden = u.isHidden || !1;
                t.buttons[r].isDisabled = u.isDisabled || !1;
                n.each(t.buttons[r].keys, function(n, i) {
                    t.buttons[r].keys[n] = i.toLowerCase()
                });
                var f = n('<button type="button" class="btn"><\/button>').html(t.buttons[r].text).addClass(t.buttons[r].btnClass).prop("disabled", t.buttons[r].isDisabled).css("display", t.buttons[r].isHidden ? "none" : "").click(function(n) {
                    n.preventDefault();
                    var i = t.buttons[r].action.apply(t);
                    t.onAction(r);
                    t._stopCountDown();
                    (typeof i == "undefined" || i) && t.close()
                });
                t.buttons[r].el = f;
                t.buttons[r].setText = function(n) {
                    f.html(n)
                };
                t.buttons[r].addClass = function(n) {
                    f.addClass(n)
                };
                t.buttons[r].removeClass = function(n) {
                    f.removeClass(n)
                };
                t.buttons[r].disable = function() {
                    t.buttons[r].isDisabled = !0;
                    f.prop("disabled", !0)
                };
                t.buttons[r].enable = function() {
                    t.buttons[r].isDisabled = !1;
                    f.prop("disabled", !1)
                };
                t.buttons[r].show = function() {
                    t.buttons[r].isHidden = !1;
                    f.css("display", "");
                    t.setDialogCenter()
                };
                t.buttons[r].hide = function() {
                    t.buttons[r].isHidden = !0;
                    f.css("display", "none");
                    t.setDialogCenter()
                };
                t["$_" + r] = t["$$" + r] = f;
                t.$btnc.append(f)
            });
            i === 0 && this.$btnc.hide();
            this.closeIcon === null && i === 0 && (this.closeIcon = !0);
            this.closeIcon ? (this.closeIconClass && (r = '<i class="' + this.closeIconClass + '"><\/i>', this.$closeIcon.html(r)), this.$closeIcon.click(function(n) {
                var u, r, i, f;
                n.preventDefault();
                u = !1;
                r = !1;
                i = typeof t.closeIcon == "function" ? t.closeIcon() : t.closeIcon;
                typeof i == "string" && typeof t.buttons[i] != "undefined" ? (u = i, r = !1) : r = typeof i == "undefined" || !!i == !0 ? !0 : !1;
                u && (f = t.buttons[u].action.apply(t), r = typeof f == "undefined" || !!f);
                r && t.close()
            }), this.$closeIcon.show()) : this.$closeIcon.hide()
        },
        setTitle: function(n, t) {
            if (t = t || !1, typeof n != "undefined")
                if (typeof n == "string") this.title = n;
                else if (typeof n == "function") {
                    typeof n.promise == "function" && console.error("Promise was returned from title function, this is not supported.");
                    var i = n();
                    this.title = typeof i == "string" ? i : !1
                } else this.title = !1;
            (!this.isAjaxLoading || t) && this.$title.html(this.title || "")
        },
        setIcon: function(n, t) {
            if (t = t || !1, typeof n != "undefined")
                if (typeof n == "string") this.icon = n;
                else if (typeof n == "function") {
                    var i = n();
                    this.icon = typeof i == "string" ? i : !1
                } else this.icon = !1;
            (!this.isAjaxLoading || t) && this.$icon.html(this.icon ? '<i class="' + this.icon + '"><\/i>' : "")
        },
        setContentPrepend: function(n, t) {
            (this.contentParsed = n + this.contentParsed, !this.isAjaxLoading || t) && this.$content.prepend(n)
        },
        setContentAppend: function(n, t) {
            (this.contentParsed = this.contentParsed + n, !this.isAjaxLoading || t) && this.$content.append(n)
        },
        setContent: function(n, t) {
            t = t || !1;
            var i = this;
            (this.contentParsed = typeof n == "undefined" ? this.contentParsed : n, !this.isAjaxLoading || t) && (this.$content.html(this.contentParsed), this.setDialogCenter(), setTimeout(function() {
                i.$body.find("input[autofocus]:visible:first").focus()
            }, 100))
        },
        loadingSpinner: !1,
        showLoading: function(n) {
            this.loadingSpinner = !0;
            this.$jconfirmBox.addClass("loading");
            n && this.$btnc.find("button").prop("disabled", !0);
            this.setDialogCenter()
        },
        hideLoading: function(n) {
            this.loadingSpinner = !1;
            this.$jconfirmBox.removeClass("loading");
            n && this.$btnc.find("button").prop("disabled", !1);
            this.setDialogCenter()
        },
        ajaxResponse: !1,
        contentParsed: "",
        isAjax: !1,
        isAjaxLoading: !1,
        _parseContent: function() {
            var t = this,
                r = "&nbsp;",
                i, u;
            typeof this.content == "function" && (i = this.content.apply(this), typeof i == "string" ? this.content = i : typeof i == "object" && typeof i.always == "function" ? (this.isAjax = !0, this.isAjaxLoading = !0, i.always(function(n, i, r) {
                t.ajaxResponse = {
                    data: n,
                    status: i,
                    xhr: r
                };
                t._contentReady.resolve(n, i, r);
                typeof t.contentLoaded == "function" && t.contentLoaded(n, i, r)
            }), this.content = r) : this.content = r);
            typeof this.content == "string" && this.content.substr(0, 4).toLowerCase() === "url:" && (this.isAjax = !0, this.isAjaxLoading = !0, u = this.content.substring(4, this.content.length), n.get(u).done(function(n) {
                t.contentParsed = n
            }).always(function(n, i, r) {
                t.ajaxResponse = {
                    data: n,
                    status: i,
                    xhr: r
                };
                t._contentReady.resolve(n, i, r);
                typeof t.contentLoaded == "function" && t.contentLoaded(n, i, r)
            }));
            this.content || (this.content = r);
            this.isAjax || (this.contentParsed = this.content, this.setContent(this.contentParsed), t._contentReady.resolve())
        },
        _stopCountDown: function() {
            clearInterval(this.autoCloseInterval);
            this.$cd && this.$cd.remove()
        },
        _startCountDown: function() {
            var r = this,
                u = this.autoClose.split("|"),
                t, f, i;
            if (u.length !== 2) return console.error("Invalid option for autoClose. example 'close|10000'"), !1;
            if (t = u[0], f = parseInt(u[1]), typeof this.buttons[t] == "undefined") return console.error("Invalid button key '" + t + "' for autoClose"), !1;
            i = Math.ceil(f / 1e3);
            this.$cd = n('<span class="countdown"> (' + i + ")<\/span>").appendTo(this["$_" + t]);
            this.autoCloseInterval = setInterval(function() {
                r.$cd.html(" (" + (i -= 1) + ") ");
                i <= 0 && (r["$$" + t].trigger("click"), r._stopCountDown())
            }, 1e3)
        },
        _getKey: function(n) {
            switch (n) {
                case 192:
                    return "tilde";
                case 13:
                    return "enter";
                case 16:
                    return "shift";
                case 9:
                    return "tab";
                case 20:
                    return "capslock";
                case 17:
                    return "ctrl";
                case 91:
                    return "win";
                case 18:
                    return "alt";
                case 27:
                    return "esc";
                case 32:
                    return "space"
            }
            var t = String.fromCharCode(n);
            return /^[A-z0-9]+$/.test(t) ? t.toLowerCase() : !1
        },
        reactOnKey: function(t) {
            var e = this,
                f = n(".jconfirm"),
                r, u, i;
            if (f.eq(f.length - 1)[0] !== this.$el[0] || (r = t.which, this.$content.find(":input").is(":focus") && /13|32/.test(r))) return !1;
            u = this._getKey(r);
            u === "esc" && this.escapeKey && (this.escapeKey === !0 ? this.$scrollPane.trigger("click") : (typeof this.escapeKey == "string" || typeof this.escapeKey == "function") && (i = typeof this.escapeKey == "function" ? this.escapeKey() : this.escapeKey, i && (typeof this.buttons[i] == "undefined" ? console.warn("Invalid escapeKey, no buttons found with key " + i) : this["$_" + i].trigger("click"))));
            n.each(this.buttons, function(n, t) {
                t.keys.indexOf(u) != -1 && e["$_" + n].trigger("click")
            })
        },
        _boxTopMargin: 0,
        _boxBottomMargin: 0,
        _boxWidth: 0,
        setDialogCenter: function() {
            var i, r, u, f, e, o, s, h;
            i = 0;
            r = 0;
            this.$contentPane.css("display") != "none" && (i = this.$content.outerHeight() || 0, r = this.$contentPane.height() || 0);
            f = this.$content.children();
            f.length != 0 && (e = parseInt(f.eq(0).css("margin-top")), e && (i += e));
            r == 0 && (r = i);
            o = n(t).height();
            s = this.$body.outerHeight() - r + i;
            h = (o - s) / 2;
            s > o - (this.offsetTop + this.offsetBottom) || !this.alignMiddle ? (u = {
                "margin-top": this.offsetTop,
                "margin-bottom": this.offsetBottom
            }, this._boxTopMargin = this.offsetTop, this._boxBottomMargin = this.offsetBottom, n("body").addClass("jconfirm-no-scroll-" + this._id)) : (u = {
                "margin-top": h,
                "margin-bottom": this.offsetBottom
            }, this._boxTopMargin = h, this._boxBottomMargin = this.offsetBottom, n("body").removeClass("jconfirm-no-scroll-" + this._id));
            this.$contentPane.css({
                height: i
            }).scrollTop(0);
            this.$body.css(u);
            this.setDrag()
        },
        _unwatchContent: function() {
            clearInterval(this._timer)
        },
        close: function() {
            var i = this,
                r;
            return typeof this.onClose == "function" && this.onClose(), this._unwatchContent(), clearInterval(this.imageLoadInterval), n(t).unbind("resize." + this._id), n(t).unbind("keyup." + this._id), n(t).unbind("jcKeyDown." + this._id), this.draggable && (n(t).unbind("mousemove." + this._id), n(t).unbind("mouseup." + this._id), this.$titleContainer.unbind("mousedown")), n("body").removeClass("jconfirm-no-scroll-" + this._id), this.$body.addClass(this.closeAnimationParsed), this.$jconfirmBg.addClass("jconfirm-bg-h"), r = this.closeAnimation == "none" ? 1 : this.animationSpeed, i.$el.removeClass(i.loadedClass), setTimeout(function() {
                var h, r, u, s;
                for (i.$el.remove(), h = jconfirm.instances, r = jconfirm.instances.length - 1, r; r >= 0; r--) jconfirm.instances[r]._id == i._id && jconfirm.instances.splice(r, 1);
                if (!jconfirm.instances.length && i.scrollToPreviousElement && jconfirm.lastFocused && jconfirm.lastFocused.length && n.contains(document, jconfirm.lastFocused[0])) {
                    if (u = jconfirm.lastFocused, i.scrollToPreviousElementAnimate) {
                        var e = n(t).scrollTop(),
                            f = jconfirm.lastFocused.offset().top,
                            o = n(t).height();
                        f > e && f < e + o ? u.focus() : (s = f - Math.round(o / 3), n("html, body").animate({
                            scrollTop: s
                        }, i.animationSpeed, "swing", function() {
                            u.focus()
                        }))
                    } else u.focus();
                    jconfirm.lastFocused = !1
                }
                typeof i.onDestroy == "function" && i.onDestroy()
            }, r * .4), !0
        },
        open: function() {
            return this.isOpen() ? !1 : (this._buildHTML(), this._bindEvents(), this._open(), !0)
        },
        _open: function() {
            var n = this;
            typeof n.onOpenBefore == "function" && n.onOpenBefore();
            this.$body.removeClass(this.animationParsed);
            this.$jconfirmBg.removeClass("jconfirm-bg-h");
            this.$body.focus();
            setTimeout(function() {
                n.$body.css(n._getCSS(n.animationSpeed, 1));
                n.$body.css({
                    "transition-property": n.$body.css("transition-property") + ", margin"
                });
                n._modalReady.resolve();
                typeof n.onOpen == "function" && n.onOpen();
                n.$el.addClass(n.loadedClass)
            }, this.animationSpeed)
        },
        loadedClass: "jconfirm-open",
        isClosed: function() {
            return !this.$el || this.$el.css("display") === ""
        },
        isOpen: function() {
            return !this.isClosed()
        },
        toggle: function() {
            this.isOpen() ? this.close() : this.open()
        }
    };
    jconfirm.instances = [];
    jconfirm.lastFocused = !1;
    jconfirm.pluginDefaults = {
        template: '<div class="jconfirm"><div class="jconfirm-bg jconfirm-bg-h"><\/div><div class="jconfirm-scrollpane"><div class="jc-bs3-container"><div class="jc-bs3-row"><div class="jconfirm-box-container"><div class="jconfirm-box" role="dialog" aria-labelledby="labelled" tabindex="-1"><div class="jconfirm-closeIcon">&times;<\/div><div class="jconfirm-title-c"><span class="jconfirm-icon-c"><\/span><span class="jconfirm-title"><\/span><\/div><div class="jconfirm-content-pane"><div class="jconfirm-content"><\/div><\/div><div class="jconfirm-buttons"><\/div><div class="jconfirm-clear"><\/div><\/div><\/div><\/div><\/div><\/div><\/div>',
        title: "Hello",
        titleClass: "",
        type: "default",
        typeAnimated: !0,
        draggable: !1,
        alignMiddle: !0,
        content: "Are you sure to continue?",
        buttons: {},
        defaultButtons: {
            ok: {
                action: function() {}
            },
            close: {
                action: function() {}
            }
        },
        contentLoaded: function() {},
        icon: "",
        lazyOpen: !1,
        bgOpacity: null,
        theme: "light",
        animation: "zoom",
        closeAnimation: "scale",
        animationSpeed: 400,
        animationBounce: 1.2,
        escapeKey: !0,
        rtl: !1,
        container: "body",
        containerFluid: !1,
        backgroundDismiss: !1,
        backgroundDismissAnimation: "shake",
        autoClose: !1,
        closeIcon: null,
        closeIconClass: !1,
        watchInterval: 100,
        columnClass: "col-md-4 col-md-offset-4 col-sm-6 col-sm-offset-3 col-xs-10 col-xs-offset-1",
        boxWidth: "50%",
        scrollToPreviousElement: !0,
        scrollToPreviousElementAnimate: !0,
        useBootstrap: !0,
        offsetTop: 50,
        offsetBottom: 50,
        dragWindowGap: 15,
        bootstrapClasses: {
            container: "container",
            containerFluid: "container-fluid",
            row: "row"
        },
        onContentReady: function() {},
        onOpenBefore: function() {},
        onOpen: function() {},
        onClose: function() {},
        onDestroy: function() {},
        onAction: function() {}
    };
    var i = !1;
    n(t).on("keydown", function(r) {
        if (!i) {
            var f = n(r.target),
                u = !1;
            f.closest(".jconfirm-box").length && (u = !0);
            u && n(t).trigger("jcKeyDown");
            i = !0
        }
    });
    n(t).on("keyup", function() {
        i = !1
    })
}(jQuery, window), ! function(n) {
    "function" == typeof define && define.amd ? define(["jquery"], n) : "object" == typeof exports ? n(require("jquery")) : n(jQuery)
}(function(n) {
    function i(n) {
        return t.raw ? n : encodeURIComponent(n)
    }

    function u(n) {
        return t.raw ? n : decodeURIComponent(n)
    }

    function f(n) {
        return i(t.json ? JSON.stringify(n) : String(n))
    }

    function e(n) {
        0 === n.indexOf('"') && (n = n.slice(1, -1).replace(/\\"/g, '"').replace(/\\\\/g, "\\"));
        try {
            return n = decodeURIComponent(n.replace(o, " ")), t.json ? JSON.parse(n) : n
        } catch (i) {}
    }

    function r(i, r) {
        var u = t.raw ? i : e(i);
        return n.isFunction(r) ? r(u) : u
    }
    var o = /\+/g,
        t = n.cookie = function(e, o, s) {
            var v, c;
            if (void 0 !== o && !n.isFunction(o)) return (s = n.extend({}, t.defaults, s), "number" == typeof s.expires) && (v = s.expires, c = s.expires = new Date, c.setTime(+c + 864e5 * v)), document.cookie = [i(e), "=", f(o), s.expires ? "; expires=" + s.expires.toUTCString() : "", s.path ? "; path=" + s.path : "", s.domain ? "; domain=" + s.domain : "", s.secure ? "; secure" : ""].join("");
            for (var l = e ? void 0 : {}, y = document.cookie ? document.cookie.split("; ") : [], a = 0, b = y.length; b > a; a++) {
                var p = y[a].split("="),
                    w = u(p.shift()),
                    h = p.join("=");
                if (e && e === w) {
                    l = r(h, o);
                    break
                }
                e || void 0 === (h = r(h)) || (l[w] = h)
            }
            return l
        };
    t.defaults = {};
    n.removeCookie = function(t, i) {
        return void 0 === n.cookie(t) ? !1 : (n.cookie(t, "", n.extend({}, i, {
            expires: -1
        })), !n.cookie(t))
    }
}), ! function(n) {
    "use strict";

    function f(n, t) {
        var i = (65535 & n) + (65535 & t),
            r = (n >> 16) + (t >> 16) + (i >> 16);
        return r << 16 | 65535 & i
    }

    function p(n, t) {
        return n << t | n >>> 32 - t
    }

    function e(n, t, i, r, u, e) {
        return f(p(f(f(t, n), f(r, e)), u), i)
    }

    function t(n, t, i, r, u, f, o) {
        return e(t & i | ~t & r, n, t, u, f, o)
    }

    function i(n, t, i, r, u, f, o) {
        return e(t & r | i & ~r, n, t, u, f, o)
    }

    function r(n, t, i, r, u, f, o) {
        return e(t ^ i ^ r, n, t, u, f, o)
    }

    function u(n, t, i, r, u, f, o) {
        return e(i ^ (t | ~r), n, t, u, f, o)
    }

    function o(n, e) {
        n[e >> 5] |= 128 << e % 32;
        n[(e + 64 >>> 9 << 4) + 14] = e;
        for (var a, v, y, p, o = 1732584193, s = -271733879, h = -1732584194, c = 271733878, l = 0; l < n.length; l += 16) a = o, v = s, y = h, p = c, o = t(o, s, h, c, n[l], 7, -680876936), c = t(c, o, s, h, n[l + 1], 12, -389564586), h = t(h, c, o, s, n[l + 2], 17, 606105819), s = t(s, h, c, o, n[l + 3], 22, -1044525330), o = t(o, s, h, c, n[l + 4], 7, -176418897), c = t(c, o, s, h, n[l + 5], 12, 1200080426), h = t(h, c, o, s, n[l + 6], 17, -1473231341), s = t(s, h, c, o, n[l + 7], 22, -45705983), o = t(o, s, h, c, n[l + 8], 7, 1770035416), c = t(c, o, s, h, n[l + 9], 12, -1958414417), h = t(h, c, o, s, n[l + 10], 17, -42063), s = t(s, h, c, o, n[l + 11], 22, -1990404162), o = t(o, s, h, c, n[l + 12], 7, 1804603682), c = t(c, o, s, h, n[l + 13], 12, -40341101), h = t(h, c, o, s, n[l + 14], 17, -1502002290), s = t(s, h, c, o, n[l + 15], 22, 1236535329), o = i(o, s, h, c, n[l + 1], 5, -165796510), c = i(c, o, s, h, n[l + 6], 9, -1069501632), h = i(h, c, o, s, n[l + 11], 14, 643717713), s = i(s, h, c, o, n[l], 20, -373897302), o = i(o, s, h, c, n[l + 5], 5, -701558691), c = i(c, o, s, h, n[l + 10], 9, 38016083), h = i(h, c, o, s, n[l + 15], 14, -660478335), s = i(s, h, c, o, n[l + 4], 20, -405537848), o = i(o, s, h, c, n[l + 9], 5, 568446438), c = i(c, o, s, h, n[l + 14], 9, -1019803690), h = i(h, c, o, s, n[l + 3], 14, -187363961), s = i(s, h, c, o, n[l + 8], 20, 1163531501), o = i(o, s, h, c, n[l + 13], 5, -1444681467), c = i(c, o, s, h, n[l + 2], 9, -51403784), h = i(h, c, o, s, n[l + 7], 14, 1735328473), s = i(s, h, c, o, n[l + 12], 20, -1926607734), o = r(o, s, h, c, n[l + 5], 4, -378558), c = r(c, o, s, h, n[l + 8], 11, -2022574463), h = r(h, c, o, s, n[l + 11], 16, 1839030562), s = r(s, h, c, o, n[l + 14], 23, -35309556), o = r(o, s, h, c, n[l + 1], 4, -1530992060), c = r(c, o, s, h, n[l + 4], 11, 1272893353), h = r(h, c, o, s, n[l + 7], 16, -155497632), s = r(s, h, c, o, n[l + 10], 23, -1094730640), o = r(o, s, h, c, n[l + 13], 4, 681279174), c = r(c, o, s, h, n[l], 11, -358537222), h = r(h, c, o, s, n[l + 3], 16, -722521979), s = r(s, h, c, o, n[l + 6], 23, 76029189), o = r(o, s, h, c, n[l + 9], 4, -640364487), c = r(c, o, s, h, n[l + 12], 11, -421815835), h = r(h, c, o, s, n[l + 15], 16, 530742520), s = r(s, h, c, o, n[l + 2], 23, -995338651), o = u(o, s, h, c, n[l], 6, -198630844), c = u(c, o, s, h, n[l + 7], 10, 1126891415), h = u(h, c, o, s, n[l + 14], 15, -1416354905), s = u(s, h, c, o, n[l + 5], 21, -57434055), o = u(o, s, h, c, n[l + 12], 6, 1700485571), c = u(c, o, s, h, n[l + 3], 10, -1894986606), h = u(h, c, o, s, n[l + 10], 15, -1051523), s = u(s, h, c, o, n[l + 1], 21, -2054922799), o = u(o, s, h, c, n[l + 8], 6, 1873313359), c = u(c, o, s, h, n[l + 15], 10, -30611744), h = u(h, c, o, s, n[l + 6], 15, -1560198380), s = u(s, h, c, o, n[l + 13], 21, 1309151649), o = u(o, s, h, c, n[l + 4], 6, -145523070), c = u(c, o, s, h, n[l + 11], 10, -1120210379), h = u(h, c, o, s, n[l + 2], 15, 718787259), s = u(s, h, c, o, n[l + 9], 21, -343485551), o = f(o, a), s = f(s, v), h = f(h, y), c = f(c, p);
        return [o, s, h, c]
    }

    function l(n) {
        for (var i = "", r = 32 * n.length, t = 0; t < r; t += 8) i += String.fromCharCode(n[t >> 5] >>> t % 32 & 255);
        return i
    }

    function s(n) {
        var t, i = [],
            r;
        for (i[(n.length >> 2) - 1] = void 0, t = 0; t < i.length; t += 1) i[t] = 0;
        for (r = 8 * n.length, t = 0; t < r; t += 8) i[t >> 5] |= (255 & n.charCodeAt(t / 8)) << t % 32;
        return i
    }

    function w(n) {
        return l(o(s(n), 8 * n.length))
    }

    function b(n, t) {
        var i, e, r = s(n),
            u = [],
            f = [];
        for (u[15] = f[15] = void 0, r.length > 16 && (r = o(r, 8 * n.length)), i = 0; i < 16; i += 1) u[i] = 909522486 ^ r[i], f[i] = 1549556828 ^ r[i];
        return e = o(u.concat(s(t)), 512 + 8 * t.length), l(o(f.concat(e), 640))
    }

    function a(n) {
        for (var i, r = "0123456789abcdef", u = "", t = 0; t < n.length; t += 1) i = n.charCodeAt(t), u += r.charAt(i >>> 4 & 15) + r.charAt(15 & i);
        return u
    }

    function h(n) {
        return unescape(encodeURIComponent(n))
    }

    function v(n) {
        return w(h(n))
    }

    function k(n) {
        return a(v(n))
    }

    function y(n, t) {
        return b(h(n), h(t))
    }

    function d(n, t) {
        return a(y(n, t))
    }

    function c(n, t, i) {
        return t ? i ? y(t, n) : d(t, n) : i ? v(n) : k(n)
    }
    "function" == typeof define && define.amd ? define(function() {
        return c
    }) : "object" == typeof module && module.exports ? module.exports = c : n.md5 = c
}(this), "undefined" == typeof jQuery) throw new Error("Bootstrap's JavaScript requires jQuery"); + function(n) {
    "use strict";
    var t = n.fn.jquery.split(" ")[0].split(".");
    if (t[0] < 2 && t[1] < 9 || 1 == t[0] && 9 == t[1] && t[2] < 1 || t[0] > 3) throw new Error("Bootstrap's JavaScript requires jQuery version 1.9.1 or higher, but lower than version 4");
}(jQuery); + function(n) {
    "use strict";

    function t() {
        var i = document.createElement("bootstrap"),
            t = {
                WebkitTransition: "webkitTransitionEnd",
                MozTransition: "transitionend",
                OTransition: "oTransitionEnd otransitionend",
                transition: "transitionend"
            },
            n;
        for (n in t)
            if (void 0 !== i.style[n]) return {
                end: t[n]
            };
        return !1
    }
    n.fn.emulateTransitionEnd = function(t) {
        var i = !1,
            u = this,
            r;
        n(this).one("bsTransitionEnd", function() {
            i = !0
        });
        return r = function() {
            i || n(u).trigger(n.support.transition.end)
        }, setTimeout(r, t), this
    };
    n(function() {
        n.support.transition = t();
        n.support.transition && (n.event.special.bsTransitionEnd = {
            bindType: n.support.transition.end,
            delegateType: n.support.transition.end,
            handle: function(t) {
                if (n(t.target).is(this)) return t.handleObj.handler.apply(this, arguments)
            }
        })
    })
}(jQuery); + function(n) {
    "use strict";

    function u(i) {
        return this.each(function() {
            var r = n(this),
                u = r.data("bs.alert");
            u || r.data("bs.alert", u = new t(this));
            "string" == typeof i && u[i].call(r)
        })
    }
    var i = '[data-dismiss="alert"]',
        t = function(t) {
            n(t).on("click", i, this.close)
        },
        r;
    t.VERSION = "3.3.7";
    t.TRANSITION_DURATION = 150;
    t.prototype.close = function(i) {
        function e() {
            r.detach().trigger("closed.bs.alert").remove()
        }
        var f = n(this),
            u = f.attr("data-target"),
            r;
        u || (u = f.attr("href"), u = u && u.replace(/.*(?=#[^\s]*$)/, ""));
        r = n("#" === u ? [] : u);
        i && i.preventDefault();
        r.length || (r = f.closest(".alert"));
        r.trigger(i = n.Event("close.bs.alert"));
        i.isDefaultPrevented() || (r.removeClass("in"), n.support.transition && r.hasClass("fade") ? r.one("bsTransitionEnd", e).emulateTransitionEnd(t.TRANSITION_DURATION) : e())
    };
    r = n.fn.alert;
    n.fn.alert = u;
    n.fn.alert.Constructor = t;
    n.fn.alert.noConflict = function() {
        return n.fn.alert = r, this
    };
    n(document).on("click.bs.alert.data-api", i, t.prototype.close)
}(jQuery); + function(n) {
    "use strict";

    function i(i) {
        return this.each(function() {
            var u = n(this),
                r = u.data("bs.button"),
                f = "object" == typeof i && i;
            r || u.data("bs.button", r = new t(this, f));
            "toggle" == i ? r.toggle() : i && r.setState(i)
        })
    }
    var t = function(i, r) {
            this.$element = n(i);
            this.options = n.extend({}, t.DEFAULTS, r);
            this.isLoading = !1
        },
        r;
    t.VERSION = "3.3.7";
    t.DEFAULTS = {
        loadingText: "loading..."
    };
    t.prototype.setState = function(t) {
        var i = "disabled",
            r = this.$element,
            f = r.is("input") ? "val" : "html",
            u = r.data();
        t += "Text";
        null == u.resetText && r.data("resetText", r[f]());
        setTimeout(n.proxy(function() {
            r[f](null == u[t] ? this.options[t] : u[t]);
            "loadingText" == t ? (this.isLoading = !0, r.addClass(i).attr(i, i).prop(i, !0)) : this.isLoading && (this.isLoading = !1, r.removeClass(i).removeAttr(i).prop(i, !1))
        }, this), 0)
    };
    t.prototype.toggle = function() {
        var t = !0,
            i = this.$element.closest('[data-toggle="buttons"]'),
            n;
        i.length ? (n = this.$element.find("input"), "radio" == n.prop("type") ? (n.prop("checked") && (t = !1), i.find(".active").removeClass("active"), this.$element.addClass("active")) : "checkbox" == n.prop("type") && (n.prop("checked") !== this.$element.hasClass("active") && (t = !1), this.$element.toggleClass("active")), n.prop("checked", this.$element.hasClass("active")), t && n.trigger("change")) : (this.$element.attr("aria-pressed", !this.$element.hasClass("active")), this.$element.toggleClass("active"))
    };
    r = n.fn.button;
    n.fn.button = i;
    n.fn.button.Constructor = t;
    n.fn.button.noConflict = function() {
        return n.fn.button = r, this
    };
    n(document).on("click.bs.button.data-api", '[data-toggle^="button"]', function(t) {
        var r = n(t.target).closest(".btn");
        i.call(r, "toggle");
        n(t.target).is('input[type="radio"], input[type="checkbox"]') || (t.preventDefault(), r.is("input,button") ? r.trigger("focus") : r.find("input:visible,button:visible").first().trigger("focus"))
    }).on("focus.bs.button.data-api blur.bs.button.data-api", '[data-toggle^="button"]', function(t) {
        n(t.target).closest(".btn").toggleClass("focus", /^focus(in)?$/.test(t.type))
    })
}(jQuery); + function(n) {
    "use strict";

    function i(i) {
        return this.each(function() {
            var u = n(this),
                r = u.data("bs.carousel"),
                f = n.extend({}, t.DEFAULTS, u.data(), "object" == typeof i && i),
                e = "string" == typeof i ? i : f.slide;
            r || u.data("bs.carousel", r = new t(this, f));
            "number" == typeof i ? r.to(i) : e ? r[e]() : f.interval && r.pause().cycle()
        })
    }
    var t = function(t, i) {
            this.$element = n(t);
            this.$indicators = this.$element.find(".carousel-indicators");
            this.options = i;
            this.paused = null;
            this.sliding = null;
            this.interval = null;
            this.$active = null;
            this.$items = null;
            this.options.keyboard && this.$element.on("keydown.bs.carousel", n.proxy(this.keydown, this));
            "hover" == this.options.pause && !("ontouchstart" in document.documentElement) && this.$element.on("mouseenter.bs.carousel", n.proxy(this.pause, this)).on("mouseleave.bs.carousel", n.proxy(this.cycle, this))
        },
        u, r;
    t.VERSION = "3.3.7";
    t.TRANSITION_DURATION = 600;
    t.DEFAULTS = {
        interval: 5e3,
        pause: "hover",
        wrap: !0,
        keyboard: !0
    };
    t.prototype.keydown = function(n) {
        if (!/input|textarea/i.test(n.target.tagName)) {
            switch (n.which) {
                case 37:
                    this.prev();
                    break;
                case 39:
                    this.next();
                    break;
                default:
                    return
            }
            n.preventDefault()
        }
    };
    t.prototype.cycle = function(t) {
        return t || (this.paused = !1), this.interval && clearInterval(this.interval), this.options.interval && !this.paused && (this.interval = setInterval(n.proxy(this.next, this), this.options.interval)), this
    };
    t.prototype.getItemIndex = function(n) {
        return this.$items = n.parent().children(".item"), this.$items.index(n || this.$active)
    };
    t.prototype.getItemForDirection = function(n, t) {
        var i = this.getItemIndex(t),
            f = "prev" == n && 0 === i || "next" == n && i == this.$items.length - 1,
            r, u;
        return f && !this.options.wrap ? t : (r = "prev" == n ? -1 : 1, u = (i + r) % this.$items.length, this.$items.eq(u))
    };
    t.prototype.to = function(n) {
        var i = this,
            t = this.getItemIndex(this.$active = this.$element.find(".item.active"));
        if (!(n > this.$items.length - 1 || n < 0)) return this.sliding ? this.$element.one("slid.bs.carousel", function() {
            i.to(n)
        }) : t == n ? this.pause().cycle() : this.slide(n > t ? "next" : "prev", this.$items.eq(n))
    };
    t.prototype.pause = function(t) {
        return t || (this.paused = !0), this.$element.find(".next, .prev").length && n.support.transition && (this.$element.trigger(n.support.transition.end), this.cycle(!0)), this.interval = clearInterval(this.interval), this
    };
    t.prototype.next = function() {
        if (!this.sliding) return this.slide("next")
    };
    t.prototype.prev = function() {
        if (!this.sliding) return this.slide("prev")
    };
    t.prototype.slide = function(i, r) {
        var e = this.$element.find(".item.active"),
            u = r || this.getItemForDirection(i, e),
            l = this.interval,
            f = "next" == i ? "left" : "right",
            a = this,
            o, s, h, c;
        return u.hasClass("active") ? this.sliding = !1 : (o = u[0], s = n.Event("slide.bs.carousel", {
            relatedTarget: o,
            direction: f
        }), (this.$element.trigger(s), !s.isDefaultPrevented()) ? ((this.sliding = !0, l && this.pause(), this.$indicators.length) && (this.$indicators.find(".active").removeClass("active"), h = n(this.$indicators.children()[this.getItemIndex(u)]), h && h.addClass("active")), c = n.Event("slid.bs.carousel", {
            relatedTarget: o,
            direction: f
        }), n.support.transition && this.$element.hasClass("slide") ? (u.addClass(i), u[0].offsetWidth, e.addClass(f), u.addClass(f), e.one("bsTransitionEnd", function() {
            u.removeClass([i, f].join(" ")).addClass("active");
            e.removeClass(["active", f].join(" "));
            a.sliding = !1;
            setTimeout(function() {
                a.$element.trigger(c)
            }, 0)
        }).emulateTransitionEnd(t.TRANSITION_DURATION)) : (e.removeClass("active"), u.addClass("active"), this.sliding = !1, this.$element.trigger(c)), l && this.cycle(), this) : void 0)
    };
    u = n.fn.carousel;
    n.fn.carousel = i;
    n.fn.carousel.Constructor = t;
    n.fn.carousel.noConflict = function() {
        return n.fn.carousel = u, this
    };
    r = function(t) {
        var o, r = n(this),
            u = n(r.attr("data-target") || (o = r.attr("href")) && o.replace(/.*(?=#[^\s]+$)/, "")),
            e, f;
        u.hasClass("carousel") && (e = n.extend({}, u.data(), r.data()), f = r.attr("data-slide-to"), f && (e.interval = !1), i.call(u, e), f && u.data("bs.carousel").to(f), t.preventDefault())
    };
    n(document).on("click.bs.carousel.data-api", "[data-slide]", r).on("click.bs.carousel.data-api", "[data-slide-to]", r);
    n(window).on("load", function() {
        n('[data-ride="carousel"]').each(function() {
            var t = n(this);
            i.call(t, t.data())
        })
    })
}(jQuery); + function(n) {
    "use strict";

    function r(t) {
        var i, r = t.attr("data-target") || (i = t.attr("href")) && i.replace(/.*(?=#[^\s]+$)/, "");
        return n(r)
    }

    function i(i) {
        return this.each(function() {
            var u = n(this),
                r = u.data("bs.collapse"),
                f = n.extend({}, t.DEFAULTS, u.data(), "object" == typeof i && i);
            !r && f.toggle && /show|hide/.test(i) && (f.toggle = !1);
            r || u.data("bs.collapse", r = new t(this, f));
            "string" == typeof i && r[i]()
        })
    }
    var t = function(i, r) {
            this.$element = n(i);
            this.options = n.extend({}, t.DEFAULTS, r);
            this.$trigger = n('[data-toggle="collapse"][href="#' + i.id + '"],[data-toggle="collapse"][data-target="#' + i.id + '"]');
            this.transitioning = null;
            this.options.parent ? this.$parent = this.getParent() : this.addAriaAndCollapsedClass(this.$element, this.$trigger);
            this.options.toggle && this.toggle()
        },
        u;
    t.VERSION = "3.3.7";
    t.TRANSITION_DURATION = 350;
    t.DEFAULTS = {
        toggle: !0
    };
    t.prototype.dimension = function() {
        var n = this.$element.hasClass("width");
        return n ? "width" : "height"
    };
    t.prototype.show = function() {
        var f, r, e, u, o, s;
        if (!this.transitioning && !this.$element.hasClass("in") && (r = this.$parent && this.$parent.children(".panel").children(".in, .collapsing"), !(r && r.length && (f = r.data("bs.collapse"), f && f.transitioning)) && (e = n.Event("show.bs.collapse"), this.$element.trigger(e), !e.isDefaultPrevented()))) {
            if (r && r.length && (i.call(r, "hide"), f || r.data("bs.collapse", null)), u = this.dimension(), this.$element.removeClass("collapse").addClass("collapsing")[u](0).attr("aria-expanded", !0), this.$trigger.removeClass("collapsed").attr("aria-expanded", !0), this.transitioning = 1, o = function() {
                this.$element.removeClass("collapsing").addClass("collapse in")[u]("");
                this.transitioning = 0;
                this.$element.trigger("shown.bs.collapse")
            }, !n.support.transition) return o.call(this);
            s = n.camelCase(["scroll", u].join("-"));
            this.$element.one("bsTransitionEnd", n.proxy(o, this)).emulateTransitionEnd(t.TRANSITION_DURATION)[u](this.$element[0][s])
        }
    };
    t.prototype.hide = function() {
        var r, i, u;
        if (!this.transitioning && this.$element.hasClass("in") && (r = n.Event("hide.bs.collapse"), this.$element.trigger(r), !r.isDefaultPrevented())) return i = this.dimension(), this.$element[i](this.$element[i]())[0].offsetHeight, this.$element.addClass("collapsing").removeClass("collapse in").attr("aria-expanded", !1), this.$trigger.addClass("collapsed").attr("aria-expanded", !1), this.transitioning = 1, u = function() {
            this.transitioning = 0;
            this.$element.removeClass("collapsing").addClass("collapse").trigger("hidden.bs.collapse")
        }, n.support.transition ? void this.$element[i](0).one("bsTransitionEnd", n.proxy(u, this)).emulateTransitionEnd(t.TRANSITION_DURATION) : u.call(this)
    };
    t.prototype.toggle = function() {
        this[this.$element.hasClass("in") ? "hide" : "show"]()
    };
    t.prototype.getParent = function() {
        return n(this.options.parent).find('[data-toggle="collapse"][data-parent="' + this.options.parent + '"]').each(n.proxy(function(t, i) {
            var u = n(i);
            this.addAriaAndCollapsedClass(r(u), u)
        }, this)).end()
    };
    t.prototype.addAriaAndCollapsedClass = function(n, t) {
        var i = n.hasClass("in");
        n.attr("aria-expanded", i);
        t.toggleClass("collapsed", !i).attr("aria-expanded", i)
    };
    u = n.fn.collapse;
    n.fn.collapse = i;
    n.fn.collapse.Constructor = t;
    n.fn.collapse.noConflict = function() {
        return n.fn.collapse = u, this
    };
    n(document).on("click.bs.collapse.data-api", '[data-toggle="collapse"]', function(t) {
        var u = n(this);
        u.attr("data-target") || t.preventDefault();
        var f = r(u),
            e = f.data("bs.collapse"),
            o = e ? "toggle" : u.data();
        i.call(f, o)
    })
}(jQuery); + function(n) {
    "use strict";

    function r(t) {
        var i = t.attr("data-target"),
            r;
        return i || (i = t.attr("href"), i = i && /#[A-Za-z]/.test(i) && i.replace(/.*(?=#[^\s]*$)/, "")), r = i && n(i), r && r.length ? r : t.parent()
    }

    function u(t) {
        t && 3 === t.which || (n(o).remove(), n(i).each(function() {
            var u = n(this),
                i = r(u),
                f = {
                    relatedTarget: this
                };
            i.hasClass("open") && (t && "click" == t.type && /input|textarea/i.test(t.target.tagName) && n.contains(i[0], t.target) || (i.trigger(t = n.Event("hide.bs.dropdown", f)), t.isDefaultPrevented() || (u.attr("aria-expanded", "false"), i.removeClass("open").trigger(n.Event("hidden.bs.dropdown", f)))))
        }))
    }

    function e(i) {
        return this.each(function() {
            var r = n(this),
                u = r.data("bs.dropdown");
            u || r.data("bs.dropdown", u = new t(this));
            "string" == typeof i && u[i].call(r)
        })
    }
    var o = ".dropdown-backdrop",
        i = '[data-toggle="dropdown"]',
        t = function(t) {
            n(t).on("click.bs.dropdown", this.toggle)
        },
        f;
    t.VERSION = "3.3.7";
    t.prototype.toggle = function(t) {
        var f = n(this),
            i, o, e;
        if (!f.is(".disabled, :disabled")) {
            if (i = r(f), o = i.hasClass("open"), u(), !o) {
                if ("ontouchstart" in document.documentElement && !i.closest(".navbar-nav").length && n(document.createElement("div")).addClass("dropdown-backdrop").insertAfter(n(this)).on("click", u), e = {
                    relatedTarget: this
                }, i.trigger(t = n.Event("show.bs.dropdown", e)), t.isDefaultPrevented()) return;
                f.trigger("focus").attr("aria-expanded", "true");
                i.toggleClass("open").trigger(n.Event("shown.bs.dropdown", e))
            }
            return !1
        }
    };
    t.prototype.keydown = function(t) {
        var e, o, s, h, f, u;
        if (/(38|40|27|32)/.test(t.which) && !/input|textarea/i.test(t.target.tagName) && (e = n(this), t.preventDefault(), t.stopPropagation(), !e.is(".disabled, :disabled"))) {
            if (o = r(e), s = o.hasClass("open"), !s && 27 != t.which || s && 27 == t.which) return 27 == t.which && o.find(i).trigger("focus"), e.trigger("click");
            h = " li:not(.disabled):visible a";
            f = o.find(".dropdown-menu" + h);
            f.length && (u = f.index(t.target), 38 == t.which && u > 0 && u--, 40 == t.which && u < f.length - 1 && u++, ~u || (u = 0), f.eq(u).trigger("focus"))
        }
    };
    f = n.fn.dropdown;
    n.fn.dropdown = e;
    n.fn.dropdown.Constructor = t;
    n.fn.dropdown.noConflict = function() {
        return n.fn.dropdown = f, this
    };
    n(document).on("click.bs.dropdown.data-api", u).on("click.bs.dropdown.data-api", ".dropdown form", function(n) {
        n.stopPropagation()
    }).on("click.bs.dropdown.data-api", i, t.prototype.toggle).on("keydown.bs.dropdown.data-api", i, t.prototype.keydown).on("keydown.bs.dropdown.data-api", ".dropdown-menu", t.prototype.keydown)
}(jQuery); + function(n) {
    "use strict";


}(jQuery); + function(n) {
    "use strict";

    function r(i) {
        return this.each(function() {
            var u = n(this),
                r = u.data("bs.tooltip"),
                f = "object" == typeof i && i;
            !r && /destroy|hide/.test(i) || (r || u.data("bs.tooltip", r = new t(this, f)), "string" == typeof i && r[i]())
        })
    }
    var t = function(n, t) {
            this.type = null;
            this.options = null;
            this.enabled = null;
            this.timeout = null;
            this.hoverState = null;
            this.$element = null;
            this.inState = null;
            this.init("tooltip", n, t)
        },
        i;
    t.VERSION = "3.3.7";
    t.TRANSITION_DURATION = 150;
    t.DEFAULTS = {
        animation: !0,
        placement: "top",
        selector: !1,
        template: '<div class="tooltip" role="tooltip"><div class="tooltip-arrow"><\/div><div class="tooltip-inner"><\/div><\/div>',
        trigger: "hover focus",
        title: "",
        delay: 0,
        html: !1,
        container: !1,
        viewport: {
            selector: "body",
            padding: 0
        }
    };
    t.prototype.init = function(t, i, r) {
        var f, e, u, o, s;
        if (this.enabled = !0, this.type = t, this.$element = n(i), this.options = this.getOptions(r), this.$viewport = this.options.viewport && n(n.isFunction(this.options.viewport) ? this.options.viewport.call(this, this.$element) : this.options.viewport.selector || this.options.viewport), this.inState = {
            click: !1,
            hover: !1,
            focus: !1
        }, this.$element[0] instanceof document.constructor && !this.options.selector) throw new Error("`selector` option must be specified when initializing " + this.type + " on the window.document object!");
        for (f = this.options.trigger.split(" "), e = f.length; e--;)
            if (u = f[e], "click" == u) this.$element.on("click." + this.type, this.options.selector, n.proxy(this.toggle, this));
            else "manual" != u && (o = "hover" == u ? "mouseenter" : "focusin", s = "hover" == u ? "mouseleave" : "focusout", this.$element.on(o + "." + this.type, this.options.selector, n.proxy(this.enter, this)), this.$element.on(s + "." + this.type, this.options.selector, n.proxy(this.leave, this)));
        this.options.selector ? this._options = n.extend({}, this.options, {
            trigger: "manual",
            selector: ""
        }) : this.fixTitle()
    };
    t.prototype.getDefaults = function() {
        return t.DEFAULTS
    };
    t.prototype.getOptions = function(t) {
        return t = n.extend({}, this.getDefaults(), this.$element.data(), t), t.delay && "number" == typeof t.delay && (t.delay = {
            show: t.delay,
            hide: t.delay
        }), t
    };
    t.prototype.getDelegateOptions = function() {
        var t = {},
            i = this.getDefaults();
        return this._options && n.each(this._options, function(n, r) {
            i[n] != r && (t[n] = r)
        }), t
    };
    t.prototype.enter = function(t) {
        var i = t instanceof this.constructor ? t : n(t.currentTarget).data("bs." + this.type);
        return i || (i = new this.constructor(t.currentTarget, this.getDelegateOptions()), n(t.currentTarget).data("bs." + this.type, i)), t instanceof n.Event && (i.inState["focusin" == t.type ? "focus" : "hover"] = !0), i.tip().hasClass("in") || "in" == i.hoverState ? void(i.hoverState = "in") : (clearTimeout(i.timeout), i.hoverState = "in", i.options.delay && i.options.delay.show ? void(i.timeout = setTimeout(function() {
            "in" == i.hoverState && i.show()
        }, i.options.delay.show)) : i.show())
    };
    t.prototype.isInStateTrue = function() {
        for (var n in this.inState)
            if (this.inState[n]) return !0;
        return !1
    };
    t.prototype.leave = function(t) {
        var i = t instanceof this.constructor ? t : n(t.currentTarget).data("bs." + this.type);
        if (i || (i = new this.constructor(t.currentTarget, this.getDelegateOptions()), n(t.currentTarget).data("bs." + this.type, i)), t instanceof n.Event && (i.inState["focusout" == t.type ? "focus" : "hover"] = !1), !i.isInStateTrue()) return clearTimeout(i.timeout), i.hoverState = "out", i.options.delay && i.options.delay.hide ? void(i.timeout = setTimeout(function() {
            "out" == i.hoverState && i.hide()
        }, i.options.delay.hide)) : i.hide()
    };
    t.prototype.show = function() {
        var c = n.Event("show.bs." + this.type),
            l, p, e, w, h;
        if (this.hasContent() && this.enabled) {
            if (this.$element.trigger(c), l = n.contains(this.$element[0].ownerDocument.documentElement, this.$element[0]), c.isDefaultPrevented() || !l) return;
            var u = this,
                r = this.tip(),
                a = this.getUID(this.type);
            this.setContent();
            r.attr("id", a);
            this.$element.attr("aria-describedby", a);
            this.options.animation && r.addClass("fade");
            var i = "function" == typeof this.options.placement ? this.options.placement.call(this, r[0], this.$element[0]) : this.options.placement,
                v = /\s?auto?\s?/i,
                y = v.test(i);
            y && (i = i.replace(v, "") || "top");
            r.detach().css({
                top: 0,
                left: 0,
                display: "block"
            }).addClass(i).data("bs." + this.type, this);
            this.options.container ? r.appendTo(this.options.container) : r.insertAfter(this.$element);
            this.$element.trigger("inserted.bs." + this.type);
            var f = this.getPosition(),
                o = r[0].offsetWidth,
                s = r[0].offsetHeight;
            y && (p = i, e = this.getPosition(this.$viewport), i = "bottom" == i && f.bottom + s > e.bottom ? "top" : "top" == i && f.top - s < e.top ? "bottom" : "right" == i && f.right + o > e.width ? "left" : "left" == i && f.left - o < e.left ? "right" : i, r.removeClass(p).addClass(i));
            w = this.getCalculatedOffset(i, f, o, s);
            this.applyPlacement(w, i);
            h = function() {
                var n = u.hoverState;
                u.$element.trigger("shown.bs." + u.type);
                u.hoverState = null;
                "out" == n && u.leave(u)
            };
            n.support.transition && this.$tip.hasClass("fade") ? r.one("bsTransitionEnd", h).emulateTransitionEnd(t.TRANSITION_DURATION) : h()
        }
    };
    t.prototype.applyPlacement = function(t, i) {
        var r = this.tip(),
            l = r[0].offsetWidth,
            e = r[0].offsetHeight,
            o = parseInt(r.css("margin-top"), 10),
            s = parseInt(r.css("margin-left"), 10),
            h, f, u;
        isNaN(o) && (o = 0);
        isNaN(s) && (s = 0);
        t.top += o;
        t.left += s;
        n.offset.setOffset(r[0], n.extend({
            using: function(n) {
                r.css({
                    top: Math.round(n.top),
                    left: Math.round(n.left)
                })
            }
        }, t), 0);
        r.addClass("in");
        h = r[0].offsetWidth;
        f = r[0].offsetHeight;
        "top" == i && f != e && (t.top = t.top + e - f);
        u = this.getViewportAdjustedDelta(i, t, h, f);
        u.left ? t.left += u.left : t.top += u.top;
        var c = /top|bottom/.test(i),
            a = c ? 2 * u.left - l + h : 2 * u.top - e + f,
            v = c ? "offsetWidth" : "offsetHeight";
        r.offset(t);
        this.replaceArrow(a, r[0][v], c)
    };
    t.prototype.replaceArrow = function(n, t, i) {
        this.arrow().css(i ? "left" : "top", 50 * (1 - n / t) + "%").css(i ? "top" : "left", "")
    };
    t.prototype.setContent = function() {
        var n = this.tip(),
            t = this.getTitle();
        n.find(".tooltip-inner")[this.options.html ? "html" : "text"](t);
        n.removeClass("fade in top bottom left right")
    };
    t.prototype.hide = function(i) {
        function f() {
            "in" != r.hoverState && u.detach();
            r.$element && r.$element.removeAttr("aria-describedby").trigger("hidden.bs." + r.type);
            i && i()
        }
        var r = this,
            u = n(this.$tip),
            e = n.Event("hide.bs." + this.type);
        if (this.$element.trigger(e), !e.isDefaultPrevented()) return u.removeClass("in"), n.support.transition && u.hasClass("fade") ? u.one("bsTransitionEnd", f).emulateTransitionEnd(t.TRANSITION_DURATION) : f(), this.hoverState = null, this
    };
    t.prototype.fixTitle = function() {
        var n = this.$element;
        (n.attr("title") || "string" != typeof n.attr("data-original-title")) && n.attr("data-original-title", n.attr("title") || "").attr("title", "")
    };
    t.prototype.hasContent = function() {
        return this.getTitle()
    };
    t.prototype.getPosition = function(t) {
        t = t || this.$element;
        var r = t[0],
            u = "BODY" == r.tagName,
            i = r.getBoundingClientRect();
        null == i.width && (i = n.extend({}, i, {
            width: i.right - i.left,
            height: i.bottom - i.top
        }));
        var f = window.SVGElement && r instanceof window.SVGElement,
            e = u ? {
                top: 0,
                left: 0
            } : f ? null : t.offset(),
            o = {
                scroll: u ? document.documentElement.scrollTop || document.body.scrollTop : t.scrollTop()
            },
            s = u ? {
                width: n(window).width(),
                height: n(window).height()
            } : null;
        return n.extend({}, i, o, s, e)
    };
    t.prototype.getCalculatedOffset = function(n, t, i, r) {
        return "bottom" == n ? {
            top: t.top + t.height,
            left: t.left + t.width / 2 - i / 2
        } : "top" == n ? {
            top: t.top - r,
            left: t.left + t.width / 2 - i / 2
        } : "left" == n ? {
            top: t.top + t.height / 2 - r / 2,
            left: t.left - i
        } : {
            top: t.top + t.height / 2 - r / 2,
            left: t.left + t.width
        }
    };
    t.prototype.getViewportAdjustedDelta = function(n, t, i, r) {
        var f = {
                top: 0,
                left: 0
            },
            e, u, o, s, h, c;
        return this.$viewport ? (e = this.options.viewport && this.options.viewport.padding || 0, u = this.getPosition(this.$viewport), /right|left/.test(n) ? (o = t.top - e - u.scroll, s = t.top + e - u.scroll + r, o < u.top ? f.top = u.top - o : s > u.top + u.height && (f.top = u.top + u.height - s)) : (h = t.left - e, c = t.left + e + i, h < u.left ? f.left = u.left - h : c > u.right && (f.left = u.left + u.width - c)), f) : f
    };
    t.prototype.getTitle = function() {
        var t = this.$element,
            n = this.options;
        return t.attr("data-original-title") || ("function" == typeof n.title ? n.title.call(t[0]) : n.title)
    };
    t.prototype.getUID = function(n) {
        do n += ~~(1e6 * Math.random()); while (document.getElementById(n));
        return n
    };
    t.prototype.tip = function() {
        if (!this.$tip && (this.$tip = n(this.options.template), 1 != this.$tip.length)) throw new Error(this.type + " `template` option must consist of exactly 1 top-level element!");
        return this.$tip
    };
    t.prototype.arrow = function() {
        return this.$arrow = this.$arrow || this.tip().find(".tooltip-arrow")
    };
    t.prototype.enable = function() {
        this.enabled = !0
    };
    t.prototype.disable = function() {
        this.enabled = !1
    };
    t.prototype.toggleEnabled = function() {
        this.enabled = !this.enabled
    };
    t.prototype.toggle = function(t) {
        var i = this;
        t && (i = n(t.currentTarget).data("bs." + this.type), i || (i = new this.constructor(t.currentTarget, this.getDelegateOptions()), n(t.currentTarget).data("bs." + this.type, i)));
        t ? (i.inState.click = !i.inState.click, i.isInStateTrue() ? i.enter(i) : i.leave(i)) : i.tip().hasClass("in") ? i.leave(i) : i.enter(i)
    };
    t.prototype.destroy = function() {
        var n = this;
        clearTimeout(this.timeout);
        this.hide(function() {
            n.$element.off("." + n.type).removeData("bs." + n.type);
            n.$tip && n.$tip.detach();
            n.$tip = null;
            n.$arrow = null;
            n.$viewport = null;
            n.$element = null
        })
    };
    i = n.fn.tooltip;
    n.fn.tooltip = r;
    n.fn.tooltip.Constructor = t;
    n.fn.tooltip.noConflict = function() {
        return n.fn.tooltip = i, this
    }
}(jQuery); + function(n) {
    "use strict";

    function r(i) {
        return this.each(function() {
            var u = n(this),
                r = u.data("bs.popover"),
                f = "object" == typeof i && i;
            !r && /destroy|hide/.test(i) || (r || u.data("bs.popover", r = new t(this, f)), "string" == typeof i && r[i]())
        })
    }
    var t = function(n, t) {
            this.init("popover", n, t)
        },
        i;
    if (!n.fn.tooltip) throw new Error("Popover requires tooltip.js");
    t.VERSION = "3.3.7";
    t.DEFAULTS = n.extend({}, n.fn.tooltip.Constructor.DEFAULTS, {
        placement: "right",
        trigger: "click",
        content: "",
        template: '<div class="popover" role="tooltip"><div class="arrow"><\/div><h3 class="popover-title"><\/h3><div class="popover-content"><\/div><\/div>'
    });
    t.prototype = n.extend({}, n.fn.tooltip.Constructor.prototype);
    t.prototype.constructor = t;
    t.prototype.getDefaults = function() {
        return t.DEFAULTS
    };
    t.prototype.setContent = function() {
        var n = this.tip(),
            i = this.getTitle(),
            t = this.getContent();
        n.find(".popover-title")[this.options.html ? "html" : "text"](i);
        n.find(".popover-content").children().detach().end()[this.options.html ? "string" == typeof t ? "html" : "append" : "text"](t);
        n.removeClass("fade top bottom left right in");
        n.find(".popover-title").html() || n.find(".popover-title").hide()
    };
    t.prototype.hasContent = function() {
        return this.getTitle() || this.getContent()
    };
    t.prototype.getContent = function() {
        var t = this.$element,
            n = this.options;
        return t.attr("data-content") || ("function" == typeof n.content ? n.content.call(t[0]) : n.content)
    };
    t.prototype.arrow = function() {
        return this.$arrow = this.$arrow || this.tip().find(".arrow")
    };
    i = n.fn.popover;
    n.fn.popover = r;
    n.fn.popover.Constructor = t;
    n.fn.popover.noConflict = function() {
        return n.fn.popover = i, this
    }
}(jQuery); + function(n) {
    "use strict";

    function t(i, r) {
        this.$body = n(document.body);
        this.$scrollElement = n(n(i).is(document.body) ? window : i);
        this.options = n.extend({}, t.DEFAULTS, r);
        this.selector = (this.options.target || "") + " .nav li > a";
        this.offsets = [];
        this.targets = [];
        this.activeTarget = null;
        this.scrollHeight = 0;
        this.$scrollElement.on("scroll.bs.scrollspy", n.proxy(this.process, this));
        this.refresh();
        this.process()
    }

    function i(i) {
        return this.each(function() {
            var u = n(this),
                r = u.data("bs.scrollspy"),
                f = "object" == typeof i && i;
            r || u.data("bs.scrollspy", r = new t(this, f));
            "string" == typeof i && r[i]()
        })
    }
    t.VERSION = "3.3.7";
    t.DEFAULTS = {
        offset: 10
    };
    t.prototype.getScrollHeight = function() {
        return this.$scrollElement[0].scrollHeight || Math.max(this.$body[0].scrollHeight, document.documentElement.scrollHeight)
    };
    t.prototype.refresh = function() {
        var t = this,
            i = "offset",
            r = 0;
        this.offsets = [];
        this.targets = [];
        this.scrollHeight = this.getScrollHeight();
        n.isWindow(this.$scrollElement[0]) || (i = "position", r = this.$scrollElement.scrollTop());
        this.$body.find(this.selector).map(function() {
            var f = n(this),
                u = f.data("target") || f.attr("href"),
                t = /^#./.test(u) && n(u);
            return t && t.length && t.is(":visible") && [
                [t[i]().top + r, u]
            ] || null
        }).sort(function(n, t) {
            return n[0] - t[0]
        }).each(function() {
            t.offsets.push(this[0]);
            t.targets.push(this[1])
        })
    };
    t.prototype.process = function() {
        var n, i = this.$scrollElement.scrollTop() + this.options.offset,
            f = this.getScrollHeight(),
            e = this.options.offset + f - this.$scrollElement.height(),
            t = this.offsets,
            r = this.targets,
            u = this.activeTarget;
        if (this.scrollHeight != f && this.refresh(), i >= e) return u != (n = r[r.length - 1]) && this.activate(n);
        if (u && i < t[0]) return this.activeTarget = null, this.clear();
        for (n = t.length; n--;) u != r[n] && i >= t[n] && (void 0 === t[n + 1] || i < t[n + 1]) && this.activate(r[n])
    };
    t.prototype.activate = function(t) {
        this.activeTarget = t;
        this.clear();
        var r = this.selector + '[data-target="' + t + '"],' + this.selector + '[href="' + t + '"]',
            i = n(r).parents("li").addClass("active");
        i.parent(".dropdown-menu").length && (i = i.closest("li.dropdown").addClass("active"));
        i.trigger("activate.bs.scrollspy")
    };
    t.prototype.clear = function() {
        n(this.selector).parentsUntil(this.options.target, ".active").removeClass("active")
    };
    var r = n.fn.scrollspy;
    n.fn.scrollspy = i;
    n.fn.scrollspy.Constructor = t;
    n.fn.scrollspy.noConflict = function() {
        return n.fn.scrollspy = r, this
    };
    n(window).on("load.bs.scrollspy.data-api", function() {
        n('[data-spy="scroll"]').each(function() {
            var t = n(this);
            i.call(t, t.data())
        })
    })
}(jQuery); + function(n) {
    "use strict";

    function r(i) {
        return this.each(function() {
            var u = n(this),
                r = u.data("bs.tab");
            r || u.data("bs.tab", r = new t(this));
            "string" == typeof i && r[i]()
        })
    }
    var t = function(t) {
            this.element = n(t)
        },
        u, i;
    t.VERSION = "3.3.7";
    t.TRANSITION_DURATION = 150;
    t.prototype.show = function() {
        var t = this.element,
            f = t.closest("ul:not(.dropdown-menu)"),
            i = t.data("target"),
            u;
        if (i || (i = t.attr("href"), i = i && i.replace(/.*(?=#[^\s]*$)/, "")), !t.parent("li").hasClass("active")) {
            var r = f.find(".active:last a"),
                e = n.Event("hide.bs.tab", {
                    relatedTarget: t[0]
                }),
                o = n.Event("show.bs.tab", {
                    relatedTarget: r[0]
                });
            (r.trigger(e), t.trigger(o), o.isDefaultPrevented() || e.isDefaultPrevented()) || (u = n(i), this.activate(t.closest("li"), f), this.activate(u, u.parent(), function() {
                r.trigger({
                    type: "hidden.bs.tab",
                    relatedTarget: t[0]
                });
                t.trigger({
                    type: "shown.bs.tab",
                    relatedTarget: r[0]
                })
            }))
        }
    };
    t.prototype.activate = function(i, r, u) {
        function e() {
            f.removeClass("active").find("> .dropdown-menu > .active").removeClass("active").end().find('[data-toggle="tab"]').attr("aria-expanded", !1);
            i.addClass("active").find('[data-toggle="tab"]').attr("aria-expanded", !0);
            o ? (i[0].offsetWidth, i.addClass("in")) : i.removeClass("fade");
            i.parent(".dropdown-menu").length && i.closest("li.dropdown").addClass("active").end().find('[data-toggle="tab"]').attr("aria-expanded", !0);
            u && u()
        }
        var f = r.find("> .active"),
            o = u && n.support.transition && (f.length && f.hasClass("fade") || !!r.find("> .fade").length);
        f.length && o ? f.one("bsTransitionEnd", e).emulateTransitionEnd(t.TRANSITION_DURATION) : e();
        f.removeClass("in")
    };
    u = n.fn.tab;
    n.fn.tab = r;
    n.fn.tab.Constructor = t;
    n.fn.tab.noConflict = function() {
        return n.fn.tab = u, this
    };
    i = function(t) {
        t.preventDefault();
        r.call(n(this), "show")
    };
    n(document).on("click.bs.tab.data-api", '[data-toggle="tab"]', i).on("click.bs.tab.data-api", '[data-toggle="pill"]', i)
}(jQuery); + function(n) {
    "use strict";

    function i(i) {
        return this.each(function() {
            var u = n(this),
                r = u.data("bs.affix"),
                f = "object" == typeof i && i;
            r || u.data("bs.affix", r = new t(this, f));
            "string" == typeof i && r[i]()
        })
    }
    var t = function(i, r) {
            this.options = n.extend({}, t.DEFAULTS, r);
            this.$target = n(this.options.target).on("scroll.bs.affix.data-api", n.proxy(this.checkPosition, this)).on("click.bs.affix.data-api", n.proxy(this.checkPositionWithEventLoop, this));
            this.$element = n(i);
            this.affixed = null;
            this.unpin = null;
            this.pinnedOffset = null;
            this.checkPosition()
        },
        r;
    t.VERSION = "3.3.7";
    t.RESET = "affix affix-top affix-bottom";
    t.DEFAULTS = {
        offset: 0,
        target: window
    };
    t.prototype.getState = function(n, t, i, r) {
        var u = this.$target.scrollTop(),
            f = this.$element.offset(),
            e = this.$target.height();
        if (null != i && "top" == this.affixed) return u < i && "top";
        if ("bottom" == this.affixed) return null != i ? !(u + this.unpin <= f.top) && "bottom" : !(u + e <= n - r) && "bottom";
        var o = null == this.affixed,
            s = o ? u : f.top,
            h = o ? e : t;
        return null != i && u <= i ? "top" : null != r && s + h >= n - r && "bottom"
    };
    t.prototype.getPinnedOffset = function() {
        if (this.pinnedOffset) return this.pinnedOffset;
        this.$element.removeClass(t.RESET).addClass("affix");
        var n = this.$target.scrollTop(),
            i = this.$element.offset();
        return this.pinnedOffset = i.top - n
    };
    t.prototype.checkPositionWithEventLoop = function() {
        setTimeout(n.proxy(this.checkPosition, this), 1)
    };
    t.prototype.checkPosition = function() {
        var i, e, o;
        if (this.$element.is(":visible")) {
            var s = this.$element.height(),
                r = this.options.offset,
                f = r.top,
                u = r.bottom,
                h = Math.max(n(document).height(), n(document.body).height());
            if ("object" != typeof r && (u = f = r), "function" == typeof f && (f = r.top(this.$element)), "function" == typeof u && (u = r.bottom(this.$element)), i = this.getState(h, s, f, u), this.affixed != i) {
                if (null != this.unpin && this.$element.css("top", ""), e = "affix" + (i ? "-" + i : ""), o = n.Event(e + ".bs.affix"), this.$element.trigger(o), o.isDefaultPrevented()) return;
                this.affixed = i;
                this.unpin = "bottom" == i ? this.getPinnedOffset() : null;
                this.$element.removeClass(t.RESET).addClass(e).trigger(e.replace("affix", "affixed") + ".bs.affix")
            }
            "bottom" == i && this.$element.offset({
                top: h - s - u
            })
        }
    };
    r = n.fn.affix;
    n.fn.affix = i;
    n.fn.affix.Constructor = t;
    n.fn.affix.noConflict = function() {
        return n.fn.affix = r, this
    };
    n(window).on("load", function() {
        n('[data-spy="affix"]').each(function() {
            var r = n(this),
                t = r.data();
            t.offset = t.offset || {};
            null != t.offsetBottom && (t.offset.bottom = t.offsetBottom);
            null != t.offsetTop && (t.offset.top = t.offsetTop);
            i.call(r, t)
        })
    })
}(jQuery);
! function(n) {
    function o() {
        n[i].glbl || (f = {
            $wndw: n(window),
            $docu: n(document),
            $html: n("html"),
            $body: n("body")
        }, t = {}, u = {}, r = {}, n.each([t, u, r], function(n, t) {
            t.add = function(n) {
                n = n.split(" ");
                for (var i = 0, r = n.length; i < r; i++) t[n[i]] = t.mm(n[i])
            }
        }), t.mm = function(n) {
            return "mm-" + n
        }, t.add("wrapper menu panels panel nopanel current highest opened subopened navbar hasnavbar title btn prev next listview nolistview inset vertical selected divider spacer hidden fullsubopen"), t.umm = function(n) {
            return "mm-" == n.slice(0, 3) && (n = n.slice(3)), n
        }, u.mm = function(n) {
            return "mm-" + n
        }, u.add("parent child"), r.mm = function(n) {
            return n + ".mm"
        }, r.add("transitionend webkitTransitionEnd click scroll keydown mousedown mouseup touchstart touchmove touchend orientationchange"), n[i]._c = t, n[i]._d = u, n[i]._e = r, n[i].glbl = f)
    }
    var i = "mmenu",
        e = "5.7.8",
        t, u, r, f;
    n[i] && n[i].version > e || (n[i] = function(n, t, i) {
        this.$menu = n;
        this._api = ["bind", "getInstance", "update", "initPanels", "openPanel", "closePanel", "closeAllPanels", "setSelected"];
        this.opts = t;
        this.conf = i;
        this.vars = {};
        this.cbck = {};
        "function" == typeof this.___deprecated && this.___deprecated();
        this._initMenu();
        this._initAnchors();
        var r = this.$pnls.children();
        return this._initAddons(), this.initPanels(r), "function" == typeof this.___debug && this.___debug(), this
    }, n[i].version = e, n[i].addons = {}, n[i].uniqueId = 0, n[i].defaults = {
        extensions: [],
        initMenu: function() {},
        initPanels: function() {},
        navbar: {
            add: !0,
            title: "Menu",
            titleLink: "panel"
        },
        onClick: {
            setSelected: !0
        },
        slidingSubmenus: !0
    }, n[i].configuration = {
        classNames: {
            divider: "Divider",
            inset: "Inset",
            panel: "Panel",
            selected: "Selected",
            spacer: "Spacer",
            vertical: "Vertical"
        },
        clone: !1,
        openingInterval: 25,
        panelNodetype: "ul, ol, div",
        transitionDuration: 400
    }, n[i].prototype = {
        init: function(n) {
            this.initPanels(n)
        },
        getInstance: function() {
            return this
        },
        update: function() {
            this.trigger("update")
        },
        initPanels: function(n) {
            n = n.not("." + t.nopanel);
            n = this._initPanels(n);
            this.opts.initPanels.call(this, n);
            this.trigger("initPanels", n);
            this.trigger("update")
        },
        openPanel: function(r) {
            var e = r.parent(),
                u = this,
                o, s, f;
            if (e.hasClass(t.vertical)) {
                if (o = e.parents("." + t.subopened), o.length) return void this.openPanel(o.first());
                e.addClass(t.opened);
                this.trigger("openPanel", r);
                this.trigger("openingPanel", r);
                this.trigger("openedPanel", r)
            } else {
                if (r.hasClass(t.current)) return;
                s = this.$pnls.children("." + t.panel);
                f = s.filter("." + t.current);
                s.removeClass(t.highest).removeClass(t.current).not(r).not(f).not("." + t.vertical).addClass(t.hidden);
                n[i].support.csstransitions || f.addClass(t.hidden);
                r.hasClass(t.opened) ? r.nextAll("." + t.opened).addClass(t.highest).removeClass(t.opened).removeClass(t.subopened) : (r.addClass(t.highest), f.addClass(t.subopened));
                r.removeClass(t.hidden).addClass(t.current);
                u.trigger("openPanel", r);
                setTimeout(function() {
                    r.removeClass(t.subopened).addClass(t.opened);
                    u.trigger("openingPanel", r);
                    u.__transitionend(r, function() {
                        u.trigger("openedPanel", r)
                    }, u.conf.transitionDuration)
                }, this.conf.openingInterval)
            }
        },
        closePanel: function(n) {
            var i = n.parent();
            i.hasClass(t.vertical) && (i.removeClass(t.opened), this.trigger("closePanel", n), this.trigger("closingPanel", n), this.trigger("closedPanel", n))
        },
        closeAllPanels: function() {
            this.$menu.find("." + t.listview).children().removeClass(t.selected).filter("." + t.vertical).removeClass(t.opened);
            var i = this.$pnls.children("." + t.panel),
                n = i.first();
            this.$pnls.children("." + t.panel).not(n).removeClass(t.subopened).removeClass(t.opened).removeClass(t.current).removeClass(t.highest).addClass(t.hidden);
            this.openPanel(n)
        },
        togglePanel: function(n) {
            var i = n.parent();
            i.hasClass(t.vertical) && this[i.hasClass(t.opened) ? "closePanel" : "openPanel"](n)
        },
        setSelected: function(n) {
            this.$menu.find("." + t.listview).children("." + t.selected).removeClass(t.selected);
            n.addClass(t.selected);
            this.trigger("setSelected", n)
        },
        bind: function(n, t) {
            n = "init" == n ? "initPanels" : n;
            this.cbck[n] = this.cbck[n] || [];
            this.cbck[n].push(t)
        },
        trigger: function() {
            var u = this,
                i = Array.prototype.slice.call(arguments),
                n = i.shift(),
                t, r;
            if (n = "init" == n ? "initPanels" : n, this.cbck[n])
                for (t = 0, r = this.cbck[n].length; t < r; t++) this.cbck[n][t].apply(u, i)
        },
        _initMenu: function() {
            this.conf.clone && (this.$orig = this.$menu, this.$menu = this.$orig.clone(!0), this.$menu.add(this.$menu.find("[id]")).filter("[id]").each(function() {
                n(this).attr("id", t.mm(n(this).attr("id")))
            }));
            this.opts.initMenu.call(this, this.$menu, this.$orig);
            this.$menu.attr("id", this.$menu.attr("id") || this.__getUniqueId());
            this.$pnls = n('<div class="' + t.panels + '" />').append(this.$menu.children(this.conf.panelNodetype)).prependTo(this.$menu);
            this.$menu.parent().addClass(t.wrapper);
            var i = [t.menu];
            this.opts.slidingSubmenus || i.push(t.vertical);
            this.opts.extensions = this.opts.extensions.length ? "mm-" + this.opts.extensions.join(" mm-") : "";
            this.opts.extensions && i.push(this.opts.extensions);
            this.$menu.addClass(i.join(" "));
            this.trigger("_initMenu")
        },
        _initPanels: function(r) {
            var e = this,
                l = this.__findAddBack(r, "ul, ol"),
                s, f, h, a, c, o;
            return this.__refactorClass(l, this.conf.classNames.inset, "inset").addClass(t.nolistview + " " + t.nopanel), l.not("." + t.nolistview).addClass(t.listview), s = this.__findAddBack(r, "." + t.listview).children(), this.__refactorClass(s, this.conf.classNames.selected, "selected"), this.__refactorClass(s, this.conf.classNames.divider, "divider"), this.__refactorClass(s, this.conf.classNames.spacer, "spacer"), this.__refactorClass(this.__findAddBack(r, "." + this.conf.classNames.panel), this.conf.classNames.panel, "panel"), f = n(), h = r.add(r.find("." + t.panel)).add(this.__findAddBack(r, "." + t.listview).children().children(this.conf.panelNodetype)).not("." + t.nopanel), this.__refactorClass(h, this.conf.classNames.vertical, "vertical"), this.opts.slidingSubmenus || h.addClass(t.vertical), h.each(function() {
                var i = n(this),
                    r = i,
                    u;
                i.is("ul, ol") ? (i.wrap('<div class="' + t.panel + '" />'), r = i.parent()) : r.addClass(t.panel);
                u = i.attr("id");
                i.removeAttr("id");
                r.attr("id", u || e.__getUniqueId());
                i.hasClass(t.vertical) && (i.removeClass(e.conf.classNames.vertical), r.add(r.parent()).addClass(t.vertical));
                f = f.add(r)
            }), a = n("." + t.panel, this.$menu), f.each(function() {
                var o, l, f = n(this),
                    r = f.parent(),
                    s = r.children("a, span").first(),
                    h, c;
                if (r.is("." + t.panels) || (r.data(u.child, f), f.data(u.parent, r)), r.children("." + t.next).length || r.parent().is("." + t.listview) && (o = f.attr("id"), l = n('<a class="' + t.next + '" href="#' + o + '" data-target="#' + o + '" />').insertBefore(s), s.is("span") && l.addClass(t.fullsubopen)), !f.children("." + t.navbar).length && !r.hasClass(t.vertical))
                    if (r.parent().is("." + t.listview) ? r = r.closest("." + t.panel) : (s = r.closest("." + t.panel).find('a[href="#' + f.attr("id") + '"]').first(), r = s.closest("." + t.panel)), h = !1, c = n('<div class="' + t.navbar + '" />'), e.opts.navbar.add && f.addClass(t.hasnavbar), r.length) {
                        switch (o = r.attr("id"), e.opts.navbar.titleLink) {
                            case "anchor":
                                h = s.attr("href");
                                break;
                            case "panel":
                            case "parent":
                                h = "#" + o;
                                break;
                            default:
                                h = !1
                        }
                        c.append('<a class="' + t.btn + " " + t.prev + '" href="#' + o + '" data-target="#' + o + '" />').append(n('<a class="' + t.title + '"' + (h ? ' href="' + h + '"' : "") + " />").text(s.text())).prependTo(f)
                    } else e.opts.navbar.title && c.append('<a class="' + t.title + '">' + n[i].i18n(e.opts.navbar.title) + "<\/a>").prependTo(f)
            }), c = this.__findAddBack(r, "." + t.listview).children("." + t.selected).removeClass(t.selected).last().addClass(t.selected), c.add(c.parentsUntil("." + t.menu, "li")).filter("." + t.vertical).addClass(t.opened).end().each(function() {
                n(this).parentsUntil("." + t.menu, "." + t.panel).not("." + t.vertical).first().addClass(t.opened).parentsUntil("." + t.menu, "." + t.panel).not("." + t.vertical).first().addClass(t.opened).addClass(t.subopened)
            }), c.children("." + t.panel).not("." + t.vertical).addClass(t.opened).parentsUntil("." + t.menu, "." + t.panel).not("." + t.vertical).first().addClass(t.opened).addClass(t.subopened), o = a.filter("." + t.opened), o.length || (o = f.first()), o.addClass(t.opened).last().addClass(t.current), f.not("." + t.vertical).not(o.last()).addClass(t.hidden).end().filter(function() {
                return !n(this).parent().hasClass(t.panels)
            }).appendTo(this.$pnls), this.trigger("_initPanels", f), f
        },
        _initAnchors: function() {
            var u = this;
            f.$body.on(r.click + "-oncanvas", "a[href]", function(r) {
                var f = n(this),
                    e = !1,
                    s = u.$menu.find(f).length,
                    l, o, h, c;
                for (l in n[i].addons)
                    if (n[i].addons[l].clickAnchor.call(u, f, s)) {
                        e = !0;
                        break
                    } if (o = f.attr("href"), !e && s && o.length > 1 && "#" == o.slice(0, 1)) try {
                    h = n(o, u.$menu);
                    h.is("." + t.panel) && (e = !0, u[f.parent().hasClass(t.vertical) ? "togglePanel" : "openPanel"](h))
                } catch (a) {}(e && r.preventDefault(), e || !s || !f.is("." + t.listview + " > li > a") || f.is('[rel="external"]') || f.is('[target="_blank"]')) || (u.__valueOrFn(u.opts.onClick.setSelected, f) && u.setSelected(n(r.target).parent()), c = u.__valueOrFn(u.opts.onClick.preventDefault, f, "#" == o.slice(0, 1)), c && r.preventDefault(), u.__valueOrFn(u.opts.onClick.close, f, c) && u.close())
            });
            this.trigger("_initAnchors")
        },
        _initAddons: function() {
            var t;
            for (t in n[i].addons) n[i].addons[t].add.call(this), n[i].addons[t].add = function() {};
            for (t in n[i].addons) n[i].addons[t].setup.call(this);
            this.trigger("_initAddons")
        },
        _getOriginalMenuId: function() {
            var n = this.$menu.attr("id");
            return n && n.length && this.conf.clone && (n = t.umm(n)), n
        },
        __api: function() {
            var i = this,
                t = {};
            return n.each(this._api, function() {
                var n = this;
                t[n] = function() {
                    var r = i[n].apply(i, arguments);
                    return "undefined" == typeof r ? t : r
                }
            }), t
        },
        __valueOrFn: function(n, t, i) {
            return "function" == typeof n ? n.call(t[0]) : "undefined" == typeof n && "undefined" != typeof i ? i : n
        },
        __refactorClass: function(n, i, r) {
            return n.filter("." + i).removeClass(i).addClass(t[r])
        },
        __findAddBack: function(n, t) {
            return n.find(t).add(n.filter(t))
        },
        __filterListItems: function(n) {
            return n.not("." + t.divider).not("." + t.hidden)
        },
        __transitionend: function(t, i, u) {
            var e = !1,
                f = function(u) {
                    if ("undefined" != typeof u) {
                        if (!n(u.target).is(t)) return !1;
                        t.unbind(r.transitionend);
                        t.unbind(r.webkitTransitionEnd)
                    }
                    e || i.call(t[0]);
                    e = !0
                };
            t.on(r.transitionend, f);
            t.on(r.webkitTransitionEnd, f);
            setTimeout(f, 1.1 * u)
        },
        __getUniqueId: function() {
            return t.mm(n[i].uniqueId++)
        }
    }, n.fn[i] = function(t, r) {
        o();
        t = n.extend(!0, {}, n[i].defaults, t);
        r = n.extend(!0, {}, n[i].configuration, r);
        var u = n();
        return this.each(function() {
            var e = n(this),
                f;
            e.data(i) || (f = new n[i](e, t, r), f.$menu.data(i, f.__api()), u = u.add(f.$menu))
        }), u
    }, n[i].i18n = function() {
        var t = {};
        return function(i) {
            switch (typeof i) {
                case "object":
                    return n.extend(t, i), t;
                case "string":
                    return t[i] || i;
                case "undefined":
                default:
                    return t
            }
        }
    }(), n[i].support = {
        touch: "ontouchstart" in window || navigator.msMaxTouchPoints || !1,
        csstransitions: function() {
            var i, t;
            if ("undefined" != typeof Modernizr && "undefined" != typeof Modernizr.csstransitions) return Modernizr.csstransitions;
            var u = document.body || document.documentElement,
                r = u.style,
                n = "transition";
            if ("string" == typeof r[n]) return !0;
            for (i = ["Moz", "webkit", "Webkit", "Khtml", "O", "ms"], n = n.charAt(0).toUpperCase() + n.substr(1), t = 0; t < i.length; t++)
                if ("string" == typeof r[i[t] + n]) return !0;
            return !1
        }(),
        csstransforms: function() {
            return "undefined" == typeof Modernizr || "undefined" == typeof Modernizr.csstransforms || Modernizr.csstransforms
        }(),
        csstransforms3d: function() {
            return "undefined" == typeof Modernizr || "undefined" == typeof Modernizr.csstransforms3d || Modernizr.csstransforms3d
        }()
    })
}(jQuery),
    function(n) {
        var u = "mmenu",
            r = "offCanvas",
            t, e, f, i;
        n[u].addons[r] = {
            setup: function() {
                var f, e, o, s, h;
                this.opts[r] && (f = this.opts[r], e = this.conf[r], i = n[u].glbl, this._api = n.merge(this._api, ["open", "close", "setPage"]), "top" != f.position && "bottom" != f.position || (f.zposition = "front"), "string" != typeof e.pageSelector && (e.pageSelector = "> " + e.pageNodetype), i.$allMenus = (i.$allMenus || n()).add(this.$menu), this.vars.opened = !1, o = [t.offcanvas], "left" != f.position && o.push(t.mm(f.position)), "back" != f.zposition && o.push(t.mm(f.zposition)), this.$menu.addClass(o.join(" ")).parent().removeClass(t.wrapper), n[u].support.csstransforms || this.$menu.addClass(t["no-csstransforms"]), n[u].support.csstransforms3d || this.$menu.addClass(t["no-csstransforms3d"]), this.setPage(i.$page), this._initBlocker(), this["_initWindow_" + r](), this.$menu[e.menuInjectMethod + "To"](e.menuWrapperSelector), s = window.location.hash, s && (h = this._getOriginalMenuId(), h && h == s.slice(1) && this.open()))
            },
            add: function() {
                t = n[u]._c;
                e = n[u]._d;
                f = n[u]._e;
                t.add("offcanvas slideout blocking modal background opening blocker page no-csstransforms3d");
                e.add("style");
                f.add("resize")
            },
            clickAnchor: function(n, u) {
                var s = this,
                    f, e, o;
                if (this.opts[r]) {
                    if (f = this._getOriginalMenuId(), f && n.is('[href="#' + f + '"]')) return u ? !0 : (e = n.closest("." + t.menu), e.length && (o = e.data("mmenu"), o && o.close)) ? (o.close(), s.__transitionend(e, function() {
                        s.open()
                    }, s.conf.transitionDuration), !0) : (this.open(), !0);
                    if (i.$page) return f = i.$page.first().attr("id"), f && n.is('[href="#' + f + '"]') ? (this.close(), !0) : void 0
                }
            }
        };
        n[u].defaults[r] = {
            position: "left",
            zposition: "back",
            blockUI: !0,
            moveBackground: !0
        };
        n[u].configuration[r] = {
            pageNodetype: "div",
            pageSelector: null,
            noPageSelector: [],
            wrapPageIfNeeded: !0,
            menuWrapperSelector: "body",
            menuInjectMethod: "prepend"
        };
        n[u].prototype.open = function() {
            if (!this.vars.opened) {
                var n = this;
                this._openSetup();
                setTimeout(function() {
                    n._openFinish()
                }, this.conf.openingInterval);
                this.trigger("open")
            }
        };
        n[u].prototype._openSetup = function() {
            var s = this,
                o = this.opts[r],
                u;
            this.closeAllOthers();
            i.$page.each(function() {
                n(this).data(e.style, n(this).attr("style") || "")
            });
            i.$wndw.trigger(f.resize + "-" + r, [!0]);
            u = [t.opened];
            o.blockUI && u.push(t.blocking);
            "modal" == o.blockUI && u.push(t.modal);
            o.moveBackground && u.push(t.background);
            "left" != o.position && u.push(t.mm(this.opts[r].position));
            "back" != o.zposition && u.push(t.mm(this.opts[r].zposition));
            this.opts.extensions && u.push(this.opts.extensions);
            i.$html.addClass(u.join(" "));
            setTimeout(function() {
                s.vars.opened = !0
            }, this.conf.openingInterval);
            this.$menu.addClass(t.current + " " + t.opened)
        };
        n[u].prototype._openFinish = function() {
            var n = this;
            this.__transitionend(i.$page.first(), function() {
                n.trigger("opened")
            }, this.conf.transitionDuration);
            i.$html.addClass(t.opening);
            this.trigger("opening")
        };
        n[u].prototype.close = function() {
            if (this.vars.opened) {
                var u = this;
                this.__transitionend(i.$page.first(), function() {
                    u.$menu.removeClass(t.current + " " + t.opened);
                    var f = [t.opened, t.blocking, t.modal, t.background, t.mm(u.opts[r].position), t.mm(u.opts[r].zposition)];
                    u.opts.extensions && f.push(u.opts.extensions);
                    i.$html.removeClass(f.join(" "));
                    i.$page.each(function() {
                        n(this).attr("style", n(this).data(e.style))
                    });
                    u.vars.opened = !1;
                    u.trigger("closed")
                }, this.conf.transitionDuration);
                i.$html.removeClass(t.opening);
                this.trigger("close");
                this.trigger("closing")
            }
        };
        n[u].prototype.closeAllOthers = function() {
            i.$allMenus.not(this.$menu).each(function() {
                var t = n(this).data(u);
                t && t.close && t.close()
            })
        };
        n[u].prototype.setPage = function(u) {
            var e = this,
                f = this.conf[r];
            u && u.length || (u = i.$body.find(f.pageSelector), f.noPageSelector.length && (u = u.not(f.noPageSelector.join(", "))), u.length > 1 && f.wrapPageIfNeeded && (u = u.wrapAll("<" + this.conf[r].pageNodetype + " />").parent()));
            u.each(function() {
                n(this).attr("id", n(this).attr("id") || e.__getUniqueId())
            });
            u.addClass(t.page + " " + t.slideout);
            i.$page = u;
            this.trigger("setPage", u)
        };
        n[u].prototype["_initWindow_" + r] = function() {
            i.$wndw.off(f.keydown + "-" + r).on(f.keydown + "-" + r, function(n) {
                if (i.$html.hasClass(t.opened) && 9 == n.keyCode) return n.preventDefault(), !1
            });
            var n = 0;
            i.$wndw.off(f.resize + "-" + r).on(f.resize + "-" + r, function(r, u) {
                if (1 == i.$page.length && (u || i.$html.hasClass(t.opened))) {
                    var f = i.$wndw.height();
                    (u || f != n) && (n = f, i.$page.css("minHeight", f))
                }
            })
        };
        n[u].prototype._initBlocker = function() {
            var u = this;
            this.opts[r].blockUI && (i.$blck || (i.$blck = n('<div id="' + t.blocker + '" class="' + t.slideout + '" />')), i.$blck.appendTo(i.$body).off(f.touchstart + "-" + r + " " + f.touchmove + "-" + r).on(f.touchstart + "-" + r + " " + f.touchmove + "-" + r, function(n) {
                n.preventDefault();
                n.stopPropagation();
                i.$blck.trigger(f.mousedown + "-" + r)
            }).off(f.mousedown + "-" + r).on(f.mousedown + "-" + r, function(n) {
                n.preventDefault();
                i.$html.hasClass(t.modal) || (u.closeAllOthers(), u.close())
            }))
        }
    }(jQuery),
    function(n) {
        var t = "mmenu",
            r = "scrollBugFix",
            i, e, u, f;
        n[t].addons[r] = {
            setup: function() {
                var o = this,
                    e = this.opts[r],
                    h, s;
                this.conf[r];
                (f = n[t].glbl, n[t].support.touch && this.opts.offCanvas && this.opts.offCanvas.blockUI && ("boolean" == typeof e && (e = {
                    fix: e
                }), "object" != typeof e && (e = {}), e = this.opts[r] = n.extend(!0, {}, n[t].defaults[r], e), e.fix)) && (h = this.$menu.attr("id"), s = !1, this.bind("opening", function() {
                    this.$pnls.children("." + i.current).scrollTop(0)
                }), f.$docu.on(u.touchmove, function(n) {
                    o.vars.opened && n.preventDefault()
                }), f.$body.on(u.touchstart, "#" + h + "> ." + i.panels + "> ." + i.current, function(n) {
                    o.vars.opened && (s || (s = !0, 0 === n.currentTarget.scrollTop ? n.currentTarget.scrollTop = 1 : n.currentTarget.scrollHeight === n.currentTarget.scrollTop + n.currentTarget.offsetHeight && (n.currentTarget.scrollTop -= 1), s = !1))
                }).on(u.touchmove, "#" + h + "> ." + i.panels + "> ." + i.current, function(t) {
                    o.vars.opened && n(this)[0].scrollHeight > n(this).innerHeight() && t.stopPropagation()
                }), f.$wndw.on(u.orientationchange, function() {
                    o.$pnls.children("." + i.current).scrollTop(0).css({
                        "-webkit-overflow-scrolling": "auto"
                    }).css({
                        "-webkit-overflow-scrolling": "touch"
                    })
                }))
            },
            add: function() {
                i = n[t]._c;
                e = n[t]._d;
                u = n[t]._e
            },
            clickAnchor: function() {}
        };
        n[t].defaults[r] = {
            fix: !0
        }
    }(jQuery),
    function(n) {
        var i = "mmenu",
            r = "autoHeight",
            t, f, u, e;
        n[i].addons[r] = {
            setup: function() {
                var u, f;
                this.opts.offCanvas && (u = this.opts[r], this.conf[r], (e = n[i].glbl, "boolean" == typeof u && u && (u = {
                    height: "auto"
                }), "string" == typeof u && (u = {
                    height: u
                }), "object" != typeof u && (u = {}), u = this.opts[r] = n.extend(!0, {}, n[i].defaults[r], u), "auto" == u.height || "highest" == u.height) && (this.$menu.addClass(t.autoheight), f = function(i) {
                    if (this.vars.opened) {
                        var f = parseInt(this.$pnls.css("top"), 10) || 0,
                            e = parseInt(this.$pnls.css("bottom"), 10) || 0,
                            r = 0;
                        this.$menu.addClass(t.measureheight);
                        "auto" == u.height ? (i = i || this.$pnls.children("." + t.current), i.is("." + t.vertical) && (i = i.parents("." + t.panel).not("." + t.vertical).first()), r = i.outerHeight()) : "highest" == u.height && this.$pnls.children().each(function() {
                            var i = n(this);
                            i.is("." + t.vertical) && (i = i.parents("." + t.panel).not("." + t.vertical).first());
                            r = Math.max(r, i.outerHeight())
                        });
                        this.$menu.height(r + f + e).removeClass(t.measureheight)
                    }
                }, this.bind("opening", f), "highest" == u.height && this.bind("initPanels", f), "auto" == u.height && (this.bind("update", f), this.bind("openPanel", f), this.bind("closePanel", f))))
            },
            add: function() {
                t = n[i]._c;
                f = n[i]._d;
                u = n[i]._e;
                t.add("autoheight measureheight");
                u.add("resize")
            },
            clickAnchor: function() {}
        };
        n[i].defaults[r] = {
            height: "default"
        }
    }(jQuery),
    function(n) {
        var t = "mmenu",
            i = "backButton",
            r, f, e, u;
        n[t].addons[i] = {
            setup: function() {
                var e, f, o;
                this.opts.offCanvas && (e = this, f = this.opts[i], this.conf[i], (u = n[t].glbl, "boolean" == typeof f && (f = {
                    close: f
                }), "object" != typeof f && (f = {}), f = n.extend(!0, {}, n[t].defaults[i], f), f.close) && (o = "#" + e.$menu.attr("id"), this.bind("opened", function() {
                    location.hash != o && history.pushState(null, document.title, o)
                }), n(window).on("popstate", function(n) {
                    u.$html.hasClass(r.opened) ? (n.stopPropagation(), e.close()) : location.hash == o && (n.stopPropagation(), e.open())
                })))
            },
            add: function() {
                return window.history && window.history.pushState ? (r = n[t]._c, f = n[t]._d, void(e = n[t]._e)) : void(n[t].addons[i].setup = function() {})
            },
            clickAnchor: function() {}
        };
        n[t].defaults[i] = {
            close: !1
        }
    }(jQuery),
    function(n) {
        var i = "mmenu",
            r = "columns",
            t, f, e, u;
        n[i].addons[r] = {
            setup: function() {
                var f = this.opts[r];
                if (this.conf[r], u = n[i].glbl, "boolean" == typeof f && (f = {
                    add: f
                }), "number" == typeof f && (f = {
                    add: !0,
                    visible: f
                }), "object" != typeof f && (f = {}), "number" == typeof f.visible && (f.visible = {
                    min: f.visible,
                    max: f.visible
                }), f = this.opts[r] = n.extend(!0, {}, n[i].defaults[r], f), f.add) {
                    f.visible.min = Math.max(1, Math.min(6, f.visible.min));
                    f.visible.max = Math.max(f.visible.min, Math.min(6, f.visible.max));
                    this.$menu.addClass(t.columns);
                    for (var c = this.opts.offCanvas ? this.$menu.add(u.$html) : this.$menu, e = [], s = 0; s <= f.visible.max; s++) e.push(t.columns + "-" + s);
                    e = e.join(" ");
                    var l = function() {
                            h.call(this, this.$pnls.children("." + t.current))
                        },
                        o = function() {
                            var n = this.$pnls.children("." + t.panel).filter("." + t.opened).length;
                            n = Math.min(f.visible.max, Math.max(f.visible.min, n));
                            c.removeClass(e).addClass(t.columns + "-" + n)
                        },
                        a = function() {
                            this.opts.offCanvas && u.$html.removeClass(e)
                        },
                        h = function(i) {
                            this.$pnls.children("." + t.panel).removeClass(e).filter("." + t.subopened).removeClass(t.hidden).add(i).slice(-f.visible.max).each(function(i) {
                                n(this).addClass(t.columns + "-" + i)
                            })
                        };
                    this.bind("open", o);
                    this.bind("close", a);
                    this.bind("initPanels", l);
                    this.bind("openPanel", h);
                    this.bind("openingPanel", o);
                    this.bind("openedPanel", o);
                    this.opts.offCanvas || o.call(this)
                }
            },
            add: function() {
                t = n[i]._c;
                f = n[i]._d;
                e = n[i]._e;
                t.add("columns")
            },
            clickAnchor: function(i, u) {
                var e, s, f, o;
                if (!this.opts[r].add) return !1;
                if (u && (e = i.attr("href"), e.length > 1 && "#" == e.slice(0, 1))) try {
                    if (s = n(e, this.$menu), s.is("." + t.panel))
                        for (f = parseInt(i.closest("." + t.panel).attr("class").split(t.columns + "-")[1].split(" ")[0], 10) + 1; f !== !1;) {
                            if (o = this.$pnls.children("." + t.columns + "-" + f), !o.length) {
                                f = !1;
                                break
                            }
                            f++;
                            o.removeClass(t.subopened).removeClass(t.opened).removeClass(t.current).removeClass(t.highest).addClass(t.hidden)
                        }
                } catch (h) {}
            }
        };
        n[i].defaults[r] = {
            add: !1,
            visible: {
                min: 1,
                max: 3
            }
        }
    }(jQuery),
    function(n) {
        var t = "mmenu",
            i = "counters",
            r, u, f, e;
        n[t].addons[i] = {
            setup: function() {
                var o = this,
                    f = this.opts[i];
                this.conf[i];
                e = n[t].glbl;
                "boolean" == typeof f && (f = {
                    add: f,
                    update: f
                });
                "object" != typeof f && (f = {});
                f = this.opts[i] = n.extend(!0, {}, n[t].defaults[i], f);
                this.bind("initPanels", function(t) {
                    this.__refactorClass(n("em", t), this.conf.classNames[i].counter, "counter")
                });
                f.add && this.bind("initPanels", function(t) {
                    var i;
                    switch (f.addTo) {
                        case "panels":
                            i = t;
                            break;
                        default:
                            i = t.filter(f.addTo)
                    }
                    i.each(function() {
                        var t = n(this).data(u.parent);
                        t && (t.children("em." + r.counter).length || t.prepend(n('<em class="' + r.counter + '" />')))
                    })
                });
                f.update && this.bind("update", function() {
                    this.$pnls.find("." + r.panel).each(function() {
                        var t = n(this),
                            f = t.data(u.parent),
                            i;
                        f && (i = f.children("em." + r.counter), i.length && (t = t.children("." + r.listview), t.length && i.html(o.__filterListItems(t.children()).length)))
                    })
                })
            },
            add: function() {
                r = n[t]._c;
                u = n[t]._d;
                f = n[t]._e;
                r.add("counter search noresultsmsg")
            },
            clickAnchor: function() {}
        };
        n[t].defaults[i] = {
            add: !1,
            addTo: "panels",
            update: !1
        };
        n[t].configuration.classNames[i] = {
            counter: "Counter"
        }
    }(jQuery),
    function(n) {
        var r = "mmenu",
            i = "dividers",
            t, f, u, e;
        n[r].addons[i] = {
            setup: function() {
                var s = this,
                    f = this.opts[i],
                    o;
                this.conf[i];
                (e = n[r].glbl, "boolean" == typeof f && (f = {
                    add: f,
                    fixed: f
                }), "object" != typeof f && (f = {}), f = this.opts[i] = n.extend(!0, {}, n[r].defaults[i], f), this.bind("initPanels", function() {
                    this.__refactorClass(n("li", this.$menu), this.conf.classNames[i].collapsed, "collapsed")
                }), f.add && this.bind("initPanels", function(i) {
                    var r;
                    switch (f.addTo) {
                        case "panels":
                            r = i;
                            break;
                        default:
                            r = i.filter(f.addTo)
                    }
                    n("." + t.divider, r).remove();
                    r.find("." + t.listview).not("." + t.vertical).each(function() {
                        var i = "";
                        s.__filterListItems(n(this).children()).each(function() {
                            var r = n.trim(n(this).children("a, span").text()).slice(0, 1).toLowerCase();
                            r != i && r.length && (i = r, n('<li class="' + t.divider + '">' + r + "<\/li>").insertBefore(this))
                        })
                    })
                }), f.collapse && this.bind("initPanels", function(i) {
                    n("." + t.divider, i).each(function() {
                        var i = n(this),
                            r = i.nextUntil("." + t.divider, "." + t.collapsed);
                        r.length && (i.children("." + t.subopen).length || (i.wrapInner("<span />"), i.prepend('<a href="#" class="' + t.subopen + " " + t.fullsubopen + '" />')))
                    })
                }), f.fixed) && (o = function(i) {
                    var f, r, u;
                    i = i || this.$pnls.children("." + t.current);
                    f = i.find("." + t.divider).not("." + t.hidden);
                    f.length ? (this.$menu.addClass(t.hasdividers), r = i.scrollTop() || 0, u = "", i.is(":visible") && i.find("." + t.divider).not("." + t.hidden).each(function() {
                        n(this).position().top + r < r + 1 && (u = n(this).text())
                    }), this.$fixeddivider.text(u)) : this.$menu.removeClass(t.hasdividers)
                }, this.$fixeddivider = n('<ul class="' + t.listview + " " + t.fixeddivider + '"><li class="' + t.divider + '"><\/li><\/ul>').prependTo(this.$pnls).children(), this.bind("openPanel", o), this.bind("update", o), this.bind("initPanels", function(t) {
                    t.off(u.scroll + "-dividers " + u.touchmove + "-dividers").on(u.scroll + "-dividers " + u.touchmove + "-dividers", function() {
                        o.call(s, n(this))
                    })
                }))
            },
            add: function() {
                t = n[r]._c;
                f = n[r]._d;
                u = n[r]._e;
                t.add("collapsed uncollapsed fixeddivider hasdividers");
                u.add("scroll")
            },
            clickAnchor: function(n, r) {
                var u, f;
                return this.opts[i].collapse && r && (u = n.parent(), u.is("." + t.divider)) ? (f = u.nextUntil("." + t.divider, "." + t.collapsed), u.toggleClass(t.opened), f[u.hasClass(t.opened) ? "addClass" : "removeClass"](t.uncollapsed), !0) : !1
            }
        };
        n[r].defaults[i] = {
            add: !1,
            addTo: "panels",
            fixed: !1,
            collapse: !1
        };
        n[r].configuration.classNames[i] = {
            collapsed: "Collapsed"
        }
    }(jQuery),
    function(n) {
        function f(n, t, i) {
            return n < t && (n = t), n > i && (n = i), n
        }

        function o(i, u, e) {
            var c, v, p, w, b, y = this,
                o = {},
                s = 0,
                k = !1,
                h = !1,
                l = 0,
                d = 0,
                a, g;
            switch (this.opts.offCanvas.position) {
                case "left":
                case "right":
                    o.events = "panleft panright";
                    o.typeLower = "x";
                    o.typeUpper = "X";
                    h = "width";
                    break;
                case "top":
                case "bottom":
                    o.events = "panup pandown";
                    o.typeLower = "y";
                    o.typeUpper = "Y";
                    h = "height"
            }
            switch (this.opts.offCanvas.position) {
                case "right":
                case "bottom":
                    o.negative = !0;
                    w = function(n) {
                        n >= e.$wndw[h]() - i.maxStartPos && (s = 1)
                    };
                    break;
                default:
                    o.negative = !1;
                    w = function(n) {
                        n <= i.maxStartPos && (s = 1)
                    }
            }
            switch (this.opts.offCanvas.position) {
                case "left":
                    o.open_dir = "right";
                    o.close_dir = "left";
                    break;
                case "right":
                    o.open_dir = "left";
                    o.close_dir = "right";
                    break;
                case "top":
                    o.open_dir = "down";
                    o.close_dir = "up";
                    break;
                case "bottom":
                    o.open_dir = "up";
                    o.close_dir = "down"
            }
            switch (this.opts.offCanvas.zposition) {
                case "front":
                    b = function() {
                        return this.$menu
                    };
                    break;
                default:
                    b = function() {
                        return n("." + r.slideout)
                    }
            }
            a = this.__valueOrFn(i.node, this.$menu, e.$page);
            "string" == typeof a && (a = n(a));
            g = new Hammer(a[0], this.opts[t].vendors.hammer);
            g.on("panstart", function(n) {
                w(n.center[o.typeLower]);
                e.$slideOutNodes = b();
                k = o.open_dir
            }).on(o.events + " panend", function(n) {
                s > 0 && n.preventDefault()
            }).on(o.events, function(n) {
                if (c = n["delta" + o.typeUpper], o.negative && (c = -c), c != l && (k = c >= l ? o.open_dir : o.close_dir), l = c, l > i.threshold && 1 == s) {
                    if (e.$html.hasClass(r.opened)) return;
                    s = 2;
                    y._openSetup();
                    y.trigger("opening");
                    e.$html.addClass(r.dragging);
                    d = f(e.$wndw[h]() * u[h].perc, u[h].min, u[h].max)
                }
                2 == s && (v = f(l, 10, d) - ("front" == y.opts.offCanvas.zposition ? d : 0), o.negative && (v = -v), p = "translate" + o.typeUpper + "(" + v + "px )", e.$slideOutNodes.css({
                    "-webkit-transform": "-webkit-" + p,
                    transform: p
                }))
            }).on("panend", function() {
                2 == s && (e.$html.removeClass(r.dragging), e.$slideOutNodes.css("transform", ""), y[k == o.open_dir ? "_openFinish" : "close"]());
                s = 0
            })
        }

        function s(i) {
            var u = this;
            i.each(function() {
                var f = n(this),
                    i = f.data(e.parent),
                    o;
                if (i && (i = i.closest("." + r.panel), i.length)) {
                    o = new Hammer(f[0], u.opts[t].vendors.hammer);
                    o.on("panright", function() {
                        u.openPanel(i)
                    })
                }
            })
        }
        var i = "mmenu",
            t = "drag",
            r, e, h, u;
        n[i].addons[t] = {
            setup: function() {
                if (this.opts.offCanvas) {
                    var r = this.opts[t],
                        f = this.conf[t];
                    u = n[i].glbl;
                    "boolean" == typeof r && (r = {
                        menu: r,
                        panels: r
                    });
                    "object" != typeof r && (r = {});
                    "boolean" == typeof r.menu && (r.menu = {
                        open: r.menu
                    });
                    "object" != typeof r.menu && (r.menu = {});
                    "boolean" == typeof r.panels && (r.panels = {
                        close: r.panels
                    });
                    "object" != typeof r.panels && (r.panels = {});
                    r = this.opts[t] = n.extend(!0, {}, n[i].defaults[t], r);
                    r.menu.open && o.call(this, r.menu, f.menu, u);
                    r.panels.close && this.bind("initPanels", function(n) {
                        s.call(this, n, r.panels, f.panels, u)
                    })
                }
            },
            add: function() {
                return "function" != typeof Hammer || Hammer.VERSION < 2 ? void(n[i].addons[t].setup = function() {}) : (r = n[i]._c, e = n[i]._d, h = n[i]._e, void r.add("dragging"))
            },
            clickAnchor: function() {}
        };
        n[i].defaults[t] = {
            menu: {
                open: !1,
                maxStartPos: 100,
                threshold: 50
            },
            panels: {
                close: !1
            },
            vendors: {
                hammer: {}
            }
        };
        n[i].configuration[t] = {
            menu: {
                width: {
                    perc: .8,
                    min: 140,
                    max: 440
                },
                height: {
                    perc: .8,
                    min: 140,
                    max: 880
                }
            },
            panels: {}
        }
    }(jQuery),
    function(n) {
        var t = "mmenu",
            i = "fixedElements",
            u, f, e, r;
        n[t].addons[i] = {
            setup: function() {
                var u, f;
                this.opts.offCanvas && (u = this.opts[i], this.conf[i], r = n[t].glbl, u = this.opts[i] = n.extend(!0, {}, n[t].defaults[i], u), f = function(n) {
                    var t = this.conf.classNames[i].fixed;
                    this.__refactorClass(n.find("." + t), t, "slideout").appendTo(r.$body)
                }, f.call(this, r.$page), this.bind("setPage", f))
            },
            add: function() {
                u = n[t]._c;
                f = n[t]._d;
                e = n[t]._e;
                u.add("fixed")
            },
            clickAnchor: function() {}
        };
        n[t].configuration.classNames[i] = {
            fixed: "Fixed"
        }
    }(jQuery),
    function(n) {
        var i = "mmenu",
            r = "dropdown",
            t, e, f, u;
        n[i].addons[r] = {
            setup: function() {
                var s, h, v, a;
                if (this.opts.offCanvas) {
                    var c = this,
                        o = this.opts[r],
                        l = this.conf[r];
                    (u = n[i].glbl, "boolean" == typeof o && o && (o = {
                        drop: o
                    }), "object" != typeof o && (o = {}), "string" == typeof o.position && (o.position = {
                        of: o.position
                    }), o = this.opts[r] = n.extend(!0, {}, n[i].defaults[r], o), o.drop) && ("string" != typeof o.position.of && (s = this.$menu.attr("id"), s && s.length && (this.conf.clone && (s = t.umm(s)), o.position.of = '[href="#' + s + '"]')), "string" == typeof o.position.of && (h = n(o.position.of), h.length && (this.$menu.addClass(t.dropdown), o.tip && this.$menu.addClass(t.tip), o.event = o.event.split(" "), 1 == o.event.length && (o.event[1] = o.event[0]), "hover" == o.event[0] && h.on(f.mouseenter + "-dropdown", function() {
                        c.open()
                    }), "hover" == o.event[1] && this.$menu.on(f.mouseleave + "-dropdown", function() {
                        c.close()
                    }), this.bind("opening", function() {
                        this.$menu.data(e.style, this.$menu.attr("style") || "");
                        u.$html.addClass(t.dropdown)
                    }), this.bind("closed", function() {
                        this.$menu.attr("style", this.$menu.data(e.style));
                        u.$html.removeClass(t.dropdown)
                    }), v = function(f, e) {
                        var s = e[0],
                            y = e[1],
                            tt = "x" == f ? "scrollLeft" : "scrollTop",
                            it = "x" == f ? "outerWidth" : "outerHeight",
                            p = "x" == f ? "left" : "top",
                            k = "x" == f ? "right" : "bottom",
                            d = "x" == f ? "width" : "height",
                            rt = "x" == f ? "maxWidth" : "maxHeight",
                            a = null,
                            ut = u.$wndw[tt](),
                            v = h.offset()[p] -= ut,
                            w = v + h[it](),
                            g = u.$wndw[d](),
                            nt = l.offset.button[f] + l.offset.viewport[f],
                            c, b;
                        if (o.position[f]) switch (o.position[f]) {
                            case "left":
                            case "bottom":
                                a = "after";
                                break;
                            case "right":
                            case "top":
                                a = "before"
                        }
                        return null === a && (a = v + (w - v) / 2 < g / 2 ? "after" : "before"), "after" == a ? (c = "x" == f ? v : w, b = g - (c + nt), s[p] = c + l.offset.button[f], s[k] = "auto", y.push(t["x" == f ? "tipleft" : "tiptop"])) : (c = "x" == f ? w : v, b = c - nt, s[k] = "calc( 100% - " + (c - l.offset.button[f]) + "px )", s[p] = "auto", y.push(t["x" == f ? "tipright" : "tipbottom"])), s[rt] = Math.min(n[i].configuration[r][d].max, b), [s, y]
                    }, a = function() {
                        if (this.vars.opened) {
                            this.$menu.attr("style", this.$menu.data(e.style));
                            var n = [{},
                                []
                            ];
                            n = v.call(this, "y", n);
                            n = v.call(this, "x", n);
                            this.$menu.css(n[0]);
                            o.tip && this.$menu.removeClass(t.tipleft + " " + t.tipright + " " + t.tiptop + " " + t.tipbottom).addClass(n[1].join(" "))
                        }
                    }, this.bind("opening", a), u.$wndw.on(f.resize + "-dropdown", function() {
                        a.call(c)
                    }), this.opts.offCanvas.blockUI || u.$wndw.on(f.scroll + "-dropdown", function() {
                        a.call(c)
                    }))))
                }
            },
            add: function() {
                t = n[i]._c;
                e = n[i]._d;
                f = n[i]._e;
                t.add("dropdown tip tipleft tipright tiptop tipbottom");
                f.add("mouseenter mouseleave resize scroll")
            },
            clickAnchor: function() {}
        };
        n[i].defaults[r] = {
            drop: !1,
            event: "click",
            position: {},
            tip: !0
        };
        n[i].configuration[r] = {
            offset: {
                button: {
                    x: -10,
                    y: 10
                },
                viewport: {
                    x: 20,
                    y: 20
                }
            },
            height: {
                max: 880
            },
            width: {
                max: 440
            }
        }
    }(jQuery),
    function(n) {
        var i = "mmenu",
            r = "iconPanels",
            t, u, f, e;
        n[i].addons[r] = {
            setup: function() {
                var s = this,
                    u = this.opts[r],
                    f, o, h;
                if (this.conf[r], e = n[i].glbl, "boolean" == typeof u && (u = {
                    add: u
                }), "number" == typeof u && (u = {
                    add: !0,
                    visible: u
                }), "object" != typeof u && (u = {}), u = this.opts[r] = n.extend(!0, {}, n[i].defaults[r], u), u.visible++, u.add) {
                    for (this.$menu.addClass(t.iconpanel), f = [], o = 0; o <= u.visible; o++) f.push(t.iconpanel + "-" + o);
                    f = f.join(" ");
                    h = function(i) {
                        i.hasClass(t.vertical) || s.$pnls.children("." + t.panel).removeClass(f).filter("." + t.subopened).removeClass(t.hidden).add(i).not("." + t.vertical).slice(-u.visible).each(function(i) {
                            n(this).addClass(t.iconpanel + "-" + i)
                        })
                    };
                    this.bind("openPanel", h);
                    this.bind("initPanels", function(i) {
                        h.call(s, s.$pnls.children("." + t.current));
                        i.not("." + t.vertical).each(function() {
                            n(this).children("." + t.subblocker).length || n(this).prepend('<a href="#' + n(this).closest("." + t.panel).attr("id") + '" class="' + t.subblocker + '" />')
                        })
                    })
                }
            },
            add: function() {
                t = n[i]._c;
                u = n[i]._d;
                f = n[i]._e;
                t.add("iconpanel subblocker")
            },
            clickAnchor: function() {}
        };
        n[i].defaults[r] = {
            add: !1,
            visible: 3
        }
    }(jQuery),
    function(n) {
        function o(i, r) {
            i || (i = this.$pnls.children("." + t.current));
            var u = n();
            "default" == r && (u = i.children("." + t.listview).find("a[href]").not(":hidden"), u.length || (u = i.find(e).not(":hidden")), u.length || (u = this.$menu.children("." + t.navbar).find(e).not(":hidden")));
            u.length || (u = this.$menu.children("." + t.tabstart));
            u.first().focus()
        }

        function s(n) {
            n || (n = this.$pnls.children("." + t.current));
            var i = this.$pnls.children("." + t.panel),
                r = i.not(n);
            r.find(e).attr("tabindex", -1);
            n.find(e).attr("tabindex", 0);
            n.find("input.mm-toggle, input.mm-check").attr("tabindex", -1)
        }
        var r = "mmenu",
            i = "keyboardNavigation",
            t, h, u, f, e;
        n[r].addons[i] = {
            setup: function() {
                var h = this,
                    u = this.opts[i],
                    c, l;
                this.conf[i];
                (f = n[r].glbl, "boolean" != typeof u && "string" != typeof u || (u = {
                    enable: u
                }), "object" != typeof u && (u = {}), u = this.opts[i] = n.extend(!0, {}, n[r].defaults[i], u), u.enable) && (u.enhance && this.$menu.addClass(t.keyboardfocus), c = n('<input class="' + t.tabstart + '" tabindex="0" type="text" />'), l = n('<input class="' + t.tabend + '" tabindex="0" type="text" />'), this.bind("initPanels", function() {
                    this.$menu.prepend(c).append(l).children("." + t.navbar).find(e).attr("tabindex", 0)
                }), this.bind("open", function() {
                    s.call(this);
                    this.__transitionend(this.$menu, function() {
                        o.call(h, null, u.enable)
                    }, this.conf.transitionDuration)
                }), this.bind("openPanel", function(n) {
                    s.call(this, n);
                    this.__transitionend(n, function() {
                        o.call(h, n, u.enable)
                    }, this.conf.transitionDuration)
                }), this["_initWindow_" + i](u.enhance))
            },
            add: function() {
                t = n[r]._c;
                h = n[r]._d;
                u = n[r]._e;
                t.add("tabstart tabend keyboardfocus");
                u.add("focusin keydown")
            },
            clickAnchor: function() {}
        };
        n[r].defaults[i] = {
            enable: !1,
            enhance: !1
        };
        n[r].configuration[i] = {};
        n[r].prototype["_initWindow_" + i] = function(r) {
            f.$wndw.off(u.keydown + "-offCanvas");
            f.$wndw.off(u.focusin + "-" + i).on(u.focusin + "-" + i, function(i) {
                if (f.$html.hasClass(t.opened)) {
                    var r = n(i.target);
                    r.is("." + t.tabend) && r.parent().find("." + t.tabstart).focus()
                }
            });
            f.$wndw.off(u.keydown + "-" + i).on(u.keydown + "-" + i, function(i) {
                var r = n(i.target),
                    f = r.closest("." + t.menu);
                if (f.length && (f.data("mmenu"), !r.is("input, textarea"))) switch (i.keyCode) {
                    case 13:
                        (r.is(".mm-toggle") || r.is(".mm-check")) && r.trigger(u.click);
                        break;
                    case 32:
                    case 37:
                    case 38:
                    case 39:
                    case 40:
                        i.preventDefault()
                }
            });
            r && f.$wndw.on(u.keydown + "-" + i, function(i) {
                var r = n(i.target),
                    f = r.closest("." + t.menu),
                    e, u;
                if (f.length)
                    if (e = f.data("mmenu"), r.is("input, textarea")) switch (i.keyCode) {
                        case 27:
                            r.val("")
                    } else switch (i.keyCode) {
                        case 8:
                            u = r.closest("." + t.panel).data(h.parent);
                            u && u.length && e.openPanel(u.closest("." + t.panel));
                            break;
                        case 27:
                            f.hasClass(t.offcanvas) && e.close()
                    }
            })
        };
        e = "input, select, textarea, button, label, a[href]"
    }(jQuery),
    function(n) {
        var t = "mmenu",
            i = "lazySubmenus",
            r, u, f, e;
        n[t].addons[i] = {
            setup: function() {
                var f = this.opts[i];
                this.conf[i];
                e = n[t].glbl;
                "boolean" == typeof f && (f = {
                    load: f
                });
                "object" != typeof f && (f = {});
                f = this.opts[i] = n.extend(!0, {}, n[t].defaults[i], f);
                f.load && (this.$menu.find("li").find("li").children(this.conf.panelNodetype).each(function() {
                    n(this).parent().addClass(r.lazysubmenu).data(u.lazysubmenu, this).end().remove()
                }), this.bind("openingPanel", function(t) {
                    var i = t.find("." + r.lazysubmenu);
                    i.length && (i.each(function() {
                        n(this).append(n(this).data(u.lazysubmenu)).removeData(u.lazysubmenu).removeClass(r.lazysubmenu)
                    }), this.initPanels(t))
                }))
            },
            add: function() {
                r = n[t]._c;
                u = n[t]._d;
                f = n[t]._e;
                r.add("lazysubmenu");
                u.add("lazysubmenu")
            },
            clickAnchor: function() {}
        };
        n[t].defaults[i] = {
            load: !1
        };
        n[t].configuration[i] = {}
    }(jQuery),
    function(n) {
        var i = "mmenu",
            r = "navbars",
            t, u, f, e;
        n[i].addons[r] = {
            setup: function() {
                var o = this,
                    f = this.opts[r],
                    h = this.conf[r],
                    u, s;
                if ((e = n[i].glbl, "undefined" != typeof f) && (f instanceof Array || (f = [f]), u = {}, f.length)) {
                    n.each(f, function(e) {
                        var s = f[e],
                            c, l, a, v;
                        "boolean" == typeof s && s && (s = {});
                        "object" != typeof s && (s = {});
                        "undefined" == typeof s.content && (s.content = ["prev", "title"]);
                        s.content instanceof Array || (s.content = [s.content]);
                        s = n.extend(!0, {}, o.opts.navbar, s);
                        c = s.position;
                        l = s.height;
                        "number" != typeof l && (l = 1);
                        l = Math.min(4, Math.max(1, l));
                        "bottom" != c && (c = "top");
                        u[c] || (u[c] = 0);
                        u[c]++;
                        a = n("<div />").addClass(t.navbar + " " + t.navbar + "-" + c + " " + t.navbar + "-" + c + "-" + u[c] + " " + t.navbar + "-size-" + l);
                        u[c] += l - 1;
                        for (var p = 0, y = 0, w = s.content.length; y < w; y++) v = n[i].addons[r][s.content[y]] || !1, v ? p += v.call(o, a, s, h) : (v = s.content[y], v instanceof n || (v = n(s.content[y])), a.append(v));
                        p += Math.ceil(a.children().not("." + t.btn).length / l);
                        p > 1 && a.addClass(t.navbar + "-content-" + p);
                        a.children("." + t.btn).length && a.addClass(t.hasbtns);
                        a.prependTo(o.$menu)
                    });
                    for (s in u) o.$menu.addClass(t.hasnavbar + "-" + s + "-" + u[s])
                }
            },
            add: function() {
                t = n[i]._c;
                u = n[i]._d;
                f = n[i]._e;
                t.add("close hasbtns")
            },
            clickAnchor: function() {}
        };
        n[i].configuration[r] = {
            breadcrumbSeparator: "/"
        };
        n[i].configuration.classNames[r] = {}
    }(jQuery),
    function(n) {
        var t = "mmenu";
        n[t].addons["navbars"]["breadcrumbs"] = function(i, r, u) {
            var f = n[t]._c,
                s = n[t]._d,
                o, e;
            return f.add("breadcrumbs separator"), o = n('<span class="' + f.breadcrumbs + '" />').appendTo(i), this.bind("initPanels", function(t) {
                t.removeClass(f.hasnavbar).each(function() {
                    for (var i, r = [], o = n(this), h = n('<span class="' + f.breadcrumbs + '"><\/span>'), t = n(this).children().first(), e = !0; t && t.length;) t.is("." + f.panel) || (t = t.closest("." + f.panel)), i = t.children("." + f.navbar).children("." + f.title).text(), r.unshift(e ? "<span>" + i + "<\/span>" : '<a href="#' + t.attr("id") + '">' + i + "<\/a>"), e = !1, t = t.data(s.parent);
                    h.append(r.join('<span class="' + f.separator + '">' + u.breadcrumbSeparator + "<\/span>")).appendTo(o.children("." + f.navbar))
                })
            }), e = function() {
                o.html(this.$pnls.children("." + f.current).children("." + f.navbar).children("." + f.breadcrumbs).html())
            }, this.bind("openPanel", e), this.bind("initPanels", e), 0
        }
    }(jQuery),
    function(n) {
        var t = "mmenu";
        n[t].addons["navbars"]["close"] = function(i) {
            var r = n[t]._c,
                f = n[t].glbl,
                e = n('<a class="' + r.close + " " + r.btn + '" href="#" />').appendTo(i),
                u = function(n) {
                    e.attr("href", "#" + n.attr("id"))
                };
            return u.call(this, f.$page), this.bind("setPage", u), -1
        }
    }(jQuery),
    function(n) {
        var t = "mmenu",
            i = "navbars";
        n[t].addons[i]["next"] = function(r) {
            var u, o, s, f = n[t]._c,
                e = n('<a class="' + f.next + " " + f.btn + '" href="#" />').appendTo(r),
                h = function(n) {
                    n = n || this.$pnls.children("." + f.current);
                    var t = n.find("." + this.conf.classNames[i].panelNext);
                    u = t.attr("href");
                    s = t.attr("aria-owns");
                    o = t.html();
                    e[u ? "attr" : "removeAttr"]("href", u);
                    e[s ? "attr" : "removeAttr"]("aria-owns", s);
                    e[u || o ? "removeClass" : "addClass"](f.hidden);
                    e.html(o)
                };
            return this.bind("openPanel", h), this.bind("initPanels", function() {
                h.call(this)
            }), -1
        };
        n[t].configuration.classNames[i].panelNext = "Next"
    }(jQuery),
    function(n) {
        var t = "mmenu",
            i = "navbars";
        n[t].addons[i]["prev"] = function(r) {
            var u = n[t]._c,
                f = n('<a class="' + u.prev + " " + u.btn + '" href="#" />').appendTo(r),
                e, o, s, h;
            return this.bind("initPanels", function(n) {
                n.removeClass(u.hasnavbar).children("." + u.navbar).addClass(u.hidden)
            }), h = function(n) {
                if (n = n || this.$pnls.children("." + u.current), !n.hasClass(u.vertical)) {
                    var t = n.find("." + this.conf.classNames[i].panelPrev);
                    t.length || (t = n.children("." + u.navbar).children("." + u.prev));
                    e = t.attr("href");
                    s = t.attr("aria-owns");
                    o = t.html();
                    f[e ? "attr" : "removeAttr"]("href", e);
                    f[s ? "attr" : "removeAttr"]("aria-owns", s);
                    f[e || o ? "removeClass" : "addClass"](u.hidden);
                    f.html(o)
                }
            }, this.bind("openPanel", h), this.bind("initPanels", function() {
                h.call(this)
            }), -1
        };
        n[t].configuration.classNames[i].panelPrev = "Prev"
    }(jQuery),
    function(n) {
        var t = "mmenu";
        n[t].addons["navbars"]["searchfield"] = function(i) {
            var r = n[t]._c,
                u = n('<div class="' + r.search + '" />').appendTo(i);
            return "object" != typeof this.opts.searchfield && (this.opts.searchfield = {}), this.opts.searchfield.add = !0, this.opts.searchfield.addTo = u, 0
        }
    }(jQuery),
    function(n) {
        var t = "mmenu",
            i = "navbars";
        n[t].addons[i]["title"] = function(r, u) {
            var e, o, f = n[t]._c,
                s = n('<a class="' + f.title + '" />').appendTo(r),
                h = function(n) {
                    if (n = n || this.$pnls.children("." + f.current), !n.hasClass(f.vertical)) {
                        var t = n.find("." + this.conf.classNames[i].panelTitle);
                        t.length || (t = n.children("." + f.navbar).children("." + f.title));
                        e = t.attr("href");
                        o = t.html() || u.title;
                        s[e ? "attr" : "removeAttr"]("href", e);
                        s[e || o ? "removeClass" : "addClass"](f.hidden);
                        s.html(o)
                    }
                };
            return this.bind("openPanel", h), this.bind("initPanels", function() {
                h.call(this)
            }), 0
        };
        n[t].configuration.classNames[i].panelTitle = "Title"
    }(jQuery),
    function(n) {
        var t = "mmenu",
            i = "rtl",
            r, f, e, u;
        n[t].addons[i] = {
            setup: function() {
                var f = this.opts[i];
                this.conf[i];
                u = n[t].glbl;
                "object" != typeof f && (f = {
                    use: f
                });
                f = this.opts[i] = n.extend(!0, {}, n[t].defaults[i], f);
                "boolean" != typeof f.use && (f.use = "rtl" == (u.$html.attr("dir") || "").toLowerCase());
                f.use && this.$menu.addClass(r.rtl)
            },
            add: function() {
                r = n[t]._c;
                f = n[t]._d;
                e = n[t]._e;
                r.add("rtl")
            },
            clickAnchor: function() {}
        };
        n[t].defaults[i] = {
            use: "detect"
        }
    }(jQuery),
    function(n) {
        function i(n, t, i) {
            n.prop("aria-" + t, i)[i ? "attr" : "removeAttr"]("aria-" + t, i)
        }

        function f(n) {
            return '<span class="' + t.sronly + '">' + n + "<\/span>"
        }
        var r = "mmenu",
            u = "screenReader",
            t, e, o, s;
        n[r].addons[u] = {
            setup: function() {
                var e = this.opts[u],
                    o = this.conf[u],
                    a, v, h, y, c, l;
                (s = n[r].glbl, "boolean" == typeof e && (e = {
                    aria: e,
                    text: e
                }), "object" != typeof e && (e = {}), e = this.opts[u] = n.extend(!0, {}, n[r].defaults[u], e), e.aria) && (this.opts.offCanvas && (a = function() {
                    i(this.$menu, "hidden", !1)
                }, v = function() {
                    i(this.$menu, "hidden", !0)
                }, this.bind("open", a), this.bind("close", v), i(this.$menu, "hidden", !0)), h = function() {}, y = function(n) {
                    var r = this.$menu.children("." + t.navbar),
                        u = r.children("." + t.prev),
                        f = r.children("." + t.next),
                        o = r.children("." + t.title);
                    i(u, "hidden", u.is("." + t.hidden));
                    i(f, "hidden", f.is("." + t.hidden));
                    e.text && i(o, "hidden", !u.is("." + t.hidden));
                    i(this.$pnls.children("." + t.panel).not(n), "hidden", !0);
                    i(n, "hidden", !1)
                }, this.bind("update", h), this.bind("openPanel", h), this.bind("openPanel", y), c = function(r) {
                    var u;
                    r = r || this.$menu;
                    var f = r.children("." + t.navbar),
                        o = f.children("." + t.prev),
                        s = f.children("." + t.next);
                    f.children("." + t.title);
                    i(o, "haspopup", !0);
                    i(s, "haspopup", !0);
                    u = r.is("." + t.panel) ? r.find("." + t.prev + ", ." + t.next) : o.add(s);
                    u.each(function() {
                        i(n(this), "owns", n(this).attr("href").replace("#", ""))
                    });
                    e.text && r.is("." + t.panel) && (u = r.find("." + t.listview).find("." + t.fullsubopen).parent().children("span"), i(u, "hidden", !0))
                }, this.bind("initPanels", c), this.bind("_initAddons", c));
                e.text && (l = function(i) {
                    var u, e;
                    i = i || this.$menu;
                    e = i.children("." + t.navbar);
                    e.each(function() {
                        var i = n(this),
                            e = n[r].i18n(o.text.closeSubmenu);
                        u = i.children("." + t.title);
                        u.length && (e += " (" + u.text() + ")");
                        i.children("." + t.prev).html(f(e))
                    });
                    e.children("." + t.close).html(f(n[r].i18n(o.text.closeMenu)));
                    i.is("." + t.panel) && i.find("." + t.listview).children("li").children("." + t.next).each(function() {
                        var i = n(this),
                            e = n[r].i18n(o.text[i.parent().is("." + t.vertical) ? "toggleSubmenu" : "openSubmenu"]);
                        u = i.nextAll("span, a").first();
                        u.length && (e += " (" + u.text() + ")");
                        i.html(f(e))
                    })
                }, this.bind("initPanels", l), this.bind("_initAddons", l))
            },
            add: function() {
                t = n[r]._c;
                e = n[r]._d;
                o = n[r]._e;
                t.add("sronly")
            },
            clickAnchor: function() {}
        };
        n[r].defaults[u] = {
            aria: !1,
            text: !1
        };
        n[r].configuration[u] = {
            text: {
                closeMenu: "Close menu",
                closeSubmenu: "Close submenu",
                openSubmenu: "Open submenu",
                toggleSubmenu: "Toggle submenu"
            }
        }
    }(jQuery),
    function(n) {
        function e(n) {
            switch (n) {
                case 9:
                case 16:
                case 17:
                case 18:
                case 37:
                case 38:
                case 39:
                case 40:
                    return !0
            }
            return !1
        }
        var r = "mmenu",
            i = "searchfield",
            t, f, u, o;
        n[r].addons[i] = {
            setup: function() {
                var h = this,
                    s = this.opts[i],
                    c = this.conf[i];
                o = n[r].glbl;
                "boolean" == typeof s && (s = {
                    add: s
                });
                "object" != typeof s && (s = {});
                "boolean" == typeof s.resultsPanel && (s.resultsPanel = {
                    add: s.resultsPanel
                });
                s = this.opts[i] = n.extend(!0, {}, n[r].defaults[i], s);
                c = this.conf[i] = n.extend(!0, {}, n[r].configuration[i], c);
                this.bind("close", function() {
                    this.$menu.find("." + t.search).find("input").blur()
                });
                this.bind("initPanels", function(o) {
                    var a, l;
                    if (s.add) {
                        switch (s.addTo) {
                            case "panels":
                                a = o;
                                break;
                            default:
                                a = this.$menu.find(s.addTo)
                        }(a.each(function() {
                            var i = n(this),
                                e, p, l;
                            if (!i.is("." + t.panel) || !i.is("." + t.vertical)) {
                                if (!i.children("." + t.search).length) {
                                    var y = h.__valueOrFn(c.clear, i),
                                        o = h.__valueOrFn(c.form, i),
                                        a = h.__valueOrFn(c.input, i),
                                        w = h.__valueOrFn(c.submit, i),
                                        f = n("<" + (o ? "form" : "div") + ' class="' + t.search + '" />'),
                                        v = n('<input placeholder="' + n[r].i18n(s.placeholder) + '" type="text" autocomplete="off" />');
                                    if (f.append(v), a)
                                        for (e in a) v.attr(e, a[e]);
                                    if (y && n('<a class="' + t.btn + " " + t.clear + '" href="#" />').appendTo(f).on(u.click + "-searchfield", function(n) {
                                        n.preventDefault();
                                        v.val("").trigger(u.keyup + "-searchfield")
                                    }), o) {
                                        for (e in o) f.attr(e, o[e]);
                                        w && !y && n('<a class="' + t.btn + " " + t.next + '" href="#" />').appendTo(f).on(u.click + "-searchfield", function(n) {
                                            n.preventDefault();
                                            f.submit()
                                        })
                                    }
                                    i.hasClass(t.search) ? i.replaceWith(f) : i.prepend(f).addClass(t.hassearch)
                                }
                                s.noResults && (p = i.closest("." + t.panel).length, (p || (i = h.$pnls.children("." + t.panel).first()), i.children("." + t.noresultsmsg).length) || (l = i.children("." + t.listview).first(), n('<div class="' + t.noresultsmsg + " " + t.hidden + '" />').append(n[r].i18n(s.noResults))[l.length ? "insertAfter" : "prependTo"](l.length ? l : i)))
                            }
                        }), s.search) && (s.resultsPanel.add && (s.showSubPanels = !1, l = this.$pnls.children("." + t.resultspanel), l.length || (l = n('<div class="' + t.panel + " " + t.resultspanel + " " + t.hidden + '" />').appendTo(this.$pnls).append('<div class="' + t.navbar + " " + t.hidden + '"><a class="' + t.title + '">' + n[r].i18n(s.resultsPanel.title) + "<\/a><\/div>").append('<ul class="' + t.listview + '" />').append(this.$pnls.find("." + t.noresultsmsg).first().clone()), this.initPanels(l))), this.$menu.find("." + t.search).each(function() {
                            var r, v, c = n(this),
                                w = c.closest("." + t.panel).length,
                                p;
                            w ? (r = c.closest("." + t.panel), v = r) : (r = n("." + t.panel, h.$menu), v = h.$menu);
                            s.resultsPanel.add && (r = r.not(l));
                            var o = c.children("input"),
                                b = h.__findAddBack(r, "." + t.listview).children("li"),
                                g = b.filter("." + t.divider),
                                y = h.__filterListItems(b),
                                k = "a",
                                nt = k + ", span",
                                a = "",
                                d = function() {
                                    var u = o.val().toLowerCase(),
                                        i;
                                    u != a && ((a = u, s.resultsPanel.add && l.children("." + t.listview).empty(), r.scrollTop(0), y.add(g).addClass(t.hidden).find("." + t.fullsubopensearch).removeClass(t.fullsubopen + " " + t.fullsubopensearch), y.each(function() {
                                        var i = n(this),
                                            r = k,
                                            u;
                                        (s.showTextItems || s.showSubPanels && i.find("." + t.next)) && (r = nt);
                                        u = i.data(f.searchtext) || i.children(r).text();
                                        u.toLowerCase().indexOf(a) > -1 && i.add(i.prevAll("." + t.divider).first()).removeClass(t.hidden)
                                    }), s.showSubPanels && r.each(function() {
                                        var i = n(this);
                                        h.__filterListItems(i.find("." + t.listview).children()).each(function() {
                                            var i = n(this),
                                                r = i.data(f.child);
                                            i.removeClass(t.nosubresults);
                                            r && r.find("." + t.listview).children().removeClass(t.hidden)
                                        })
                                    }), s.resultsPanel.add) ? "" === a ? (this.closeAllPanels(), this.openPanel(this.$pnls.children("." + t.subopened).last())) : (i = n(), r.each(function() {
                                        var r = h.__filterListItems(n(this).find("." + t.listview).children()).not("." + t.hidden).clone(!0);
                                        r.length && (s.resultsPanel.dividers && (i = i.add('<li class="' + t.divider + '">' + n(this).children("." + t.navbar).text() + "<\/li>")), i = i.add(r))
                                    }), i.find("." + t.next).remove(), l.children("." + t.listview).append(i), this.openPanel(l)) : n(r.get().reverse()).each(function(i) {
                                        var u = n(this),
                                            r = u.data(f.parent);
                                        r && (h.__filterListItems(u.find("." + t.listview).children()).length ? (r.hasClass(t.hidden) && r.children("." + t.next).not("." + t.fullsubopen).addClass(t.fullsubopen).addClass(t.fullsubopensearch), r.removeClass(t.hidden).removeClass(t.nosubresults).prevAll("." + t.divider).first().removeClass(t.hidden)) : w || (u.hasClass(t.opened) && setTimeout(function() {
                                            h.openPanel(r.closest("." + t.panel))
                                        }, (i + 1) * 1.5 * h.conf.openingInterval), r.addClass(t.nosubresults)))
                                    }), v.find("." + t.noresultsmsg)[y.not("." + t.hidden).length ? "addClass" : "removeClass"](t.hidden), this.update())
                                };
                            o.off(u.keyup + "-" + i + " " + u.change + "-" + i).on(u.keyup + "-" + i, function(n) {
                                e(n.keyCode) || d.call(h)
                            }).on(u.change + "-" + i, function() {
                                d.call(h)
                            });
                            p = c.children("." + t.btn);
                            p.length && o.on(u.keyup + "-" + i, function() {
                                p[o.val().length ? "removeClass" : "addClass"](t.hidden)
                            });
                            o.trigger(u.keyup + "-" + i)
                        }))
                    }
                })
            },
            add: function() {
                t = n[r]._c;
                f = n[r]._d;
                u = n[r]._e;
                t.add("clear search hassearch resultspanel noresultsmsg noresults nosubresults fullsubopensearch");
                f.add("searchtext");
                u.add("change keyup")
            },
            clickAnchor: function() {}
        };
        n[r].defaults[i] = {
            add: !1,
            addTo: "panels",
            placeholder: "Search",
            noResults: "No results found.",
            resultsPanel: {
                add: !1,
                dividers: !0,
                title: "Search results"
            },
            search: !0,
            showTextItems: !1,
            showSubPanels: !0
        };
        n[r].configuration[i] = {
            clear: !1,
            form: !1,
            input: !1,
            submit: !1
        }
    }(jQuery),
    function(n) {
        var i = "mmenu",
            r = "sectionIndexer",
            t, f, u, e;
        n[i].addons[r] = {
            setup: function() {
                var o = this,
                    f = this.opts[r];
                this.conf[r];
                e = n[i].glbl;
                "boolean" == typeof f && (f = {
                    add: f
                });
                "object" != typeof f && (f = {});
                f = this.opts[r] = n.extend(!0, {}, n[i].defaults[r], f);
                this.bind("initPanels", function(i) {
                    var r, e;
                    if (f.add) {
                        switch (f.addTo) {
                            case "panels":
                                r = i;
                                break;
                            default:
                                r = n(f.addTo, this.$menu).filter("." + t.panel)
                        }
                        r.find("." + t.divider).closest("." + t.panel).addClass(t.hasindexer)
                    }!this.$indexer && this.$pnls.children("." + t.hasindexer).length && (this.$indexer = n('<div class="' + t.indexer + '" />').prependTo(this.$pnls).append('<a href="#a">a<\/a><a href="#b">b<\/a><a href="#c">c<\/a><a href="#d">d<\/a><a href="#e">e<\/a><a href="#f">f<\/a><a href="#g">g<\/a><a href="#h">h<\/a><a href="#i">i<\/a><a href="#j">j<\/a><a href="#k">k<\/a><a href="#l">l<\/a><a href="#m">m<\/a><a href="#n">n<\/a><a href="#o">o<\/a><a href="#p">p<\/a><a href="#q">q<\/a><a href="#r">r<\/a><a href="#s">s<\/a><a href="#t">t<\/a><a href="#u">u<\/a><a href="#v">v<\/a><a href="#w">w<\/a><a href="#x">x<\/a><a href="#y">y<\/a><a href="#z">z<\/a>'), this.$indexer.children().on(u.mouseover + "-sectionindexer " + t.touchstart + "-sectionindexer", function() {
                        var u = n(this).attr("href").slice(1),
                            i = o.$pnls.children("." + t.current),
                            f = i.find("." + t.listview),
                            r = !1,
                            e = i.scrollTop();
                        i.scrollTop(0);
                        f.children("." + t.divider).not("." + t.hidden).each(function() {
                            r === !1 && u == n(this).text().slice(0, 1).toLowerCase() && (r = n(this).position().top)
                        });
                        i.scrollTop(r !== !1 ? r : e)
                    }), e = function(n) {
                        o.$menu[(n.hasClass(t.hasindexer) ? "add" : "remove") + "Class"](t.hasindexer)
                    }, this.bind("openPanel", e), e.call(this, this.$pnls.children("." + t.current)))
                })
            },
            add: function() {
                t = n[i]._c;
                f = n[i]._d;
                u = n[i]._e;
                t.add("indexer hasindexer");
                u.add("mouseover touchstart")
            },
            clickAnchor: function(n) {
                if (n.parent().is("." + t.indexer)) return !0
            }
        };
        n[i].defaults[r] = {
            add: !1,
            addTo: "panels"
        }
    }(jQuery),
    function(n) {
        var i = "mmenu",
            r = "setSelected",
            t, u, f, e;
        n[i].addons[r] = {
            setup: function() {
                var h = this,
                    f = this.opts[r],
                    o, s;
                this.conf[r];
                (e = n[i].glbl, "boolean" == typeof f && (f = {
                    hover: f,
                    parent: f
                }), "object" != typeof f && (f = {}), f = this.opts[r] = n.extend(!0, {}, n[i].defaults[r], f), "detect" == f.current) ? (o = function(n) {
                    n = n.split("?")[0].split("#")[0];
                    var t = h.$menu.find('a[href="' + n + '"], a[href="' + n + '/"]');
                    t.length ? h.setSelected(t.parent(), !0) : (n = n.split("/").slice(0, -1), n.length && o(n.join("/")))
                }, o(window.location.href)) : f.current || this.bind("initPanels", function(n) {
                    n.find("." + t.listview).children("." + t.selected).removeClass(t.selected)
                });
                (f.hover && this.$menu.addClass(t.hoverselected), f.parent) && (this.$menu.addClass(t.parentselected), s = function(n) {
                    this.$pnls.find("." + t.listview).find("." + t.next).removeClass(t.selected);
                    for (var i = n.data(u.parent); i && i.length;) i = i.not("." + t.vertical).children("." + t.next).addClass(t.selected).end().closest("." + t.panel).data(u.parent)
                }, this.bind("openedPanel", s), this.bind("initPanels", function() {
                    s.call(this, this.$pnls.children("." + t.current))
                }))
            },
            add: function() {
                t = n[i]._c;
                u = n[i]._d;
                f = n[i]._e;
                t.add("hoverselected parentselected")
            },
            clickAnchor: function() {}
        };
        n[i].defaults[r] = {
            current: !0,
            hover: !1,
            parent: !1
        }
    }(jQuery),
    function(n) {
        var t = "mmenu",
            i = "toggles",
            r, u, f, e;
        n[t].addons[i] = {
            setup: function() {
                var u = this;
                this.opts[i];
                this.conf[i];
                e = n[t].glbl;
                this.bind("initPanels", function(t) {
                    this.__refactorClass(n("input", t), this.conf.classNames[i].toggle, "toggle");
                    this.__refactorClass(n("input", t), this.conf.classNames[i].check, "check");
                    n("input." + r.toggle + ", input." + r.check, t).each(function() {
                        var t = n(this),
                            i = t.closest("li"),
                            e = t.hasClass(r.toggle) ? "toggle" : "check",
                            f = t.attr("id") || u.__getUniqueId();
                        i.children('label[for="' + f + '"]').length || (t.attr("id", f), i.prepend(t), n('<label for="' + f + '" class="' + r[e] + '"><\/label>').insertBefore(i.children("a, span").last()))
                    })
                })
            },
            add: function() {
                r = n[t]._c;
                u = n[t]._d;
                f = n[t]._e;
                r.add("toggle check")
            },
            clickAnchor: function() {}
        };
        n[t].configuration.classNames[i] = {
            toggle: "Toggle",
            check: "Check"
        }
    }(jQuery);
! function(n) {
    if ("object" == typeof exports && "undefined" != typeof module) module.exports = n();
    else if ("function" == typeof define && define.amd) define([], n);
    else {
        var t;
        t = "undefined" != typeof window ? window : "undefined" != typeof global ? global : "undefined" != typeof self ? self : this;
        t.Clipboard = n()
    }
}(function() {
    var n;
    return function n(t, i, r) {
        function u(f, o) {
            var h, c, s;
            if (!i[f]) {
                if (!t[f]) {
                    if (h = "function" == typeof require && require, !o && h) return h(f, !0);
                    if (e) return e(f, !0);
                    c = new Error("Cannot find module '" + f + "'");
                    throw c.code = "MODULE_NOT_FOUND", c;
                }
                s = i[f] = {
                    exports: {}
                };
                t[f][0].call(s.exports, function(n) {
                    var i = t[f][1][n];
                    return u(i || n)
                }, s, s.exports, n, t, i, r)
            }
            return i[f].exports
        }
        for (var e = "function" == typeof require && require, f = 0; f < r.length; f++) u(r[f]);
        return u
    }({
        1: [function(n, t) {
            function r(n, t) {
                for (; n && n.nodeType !== u;) {
                    if ("function" == typeof n.matches && n.matches(t)) return n;
                    n = n.parentNode
                }
            }
            var u = 9,
                i;
            "undefined" == typeof Element || Element.prototype.matches || (i = Element.prototype, i.matches = i.matchesSelector || i.mozMatchesSelector || i.msMatchesSelector || i.oMatchesSelector || i.webkitMatchesSelector);
            t.exports = r
        }, {}],
        2: [function(n, t) {
            function i(n, t, i, u, f) {
                var e = r.apply(this, arguments);
                return n.addEventListener(i, e, f), {
                    destroy: function() {
                        n.removeEventListener(i, e, f)
                    }
                }
            }

            function r(n, t, i, r) {
                return function(i) {
                    i.delegateTarget = u(i.target, t);
                    i.delegateTarget && r.call(n, i)
                }
            }
            var u = n("./closest");
            t.exports = i
        }, {
            "./closest": 1
        }],
        3: [function(n, t, i) {
            i.node = function(n) {
                return void 0 !== n && n instanceof HTMLElement && 1 === n.nodeType
            };
            i.nodeList = function(n) {
                var t = Object.prototype.toString.call(n);
                return void 0 !== n && ("[object NodeList]" === t || "[object HTMLCollection]" === t) && "length" in n && (0 === n.length || i.node(n[0]))
            };
            i.string = function(n) {
                return "string" == typeof n || n instanceof String
            };
            i.fn = function(n) {
                return "[object Function]" === Object.prototype.toString.call(n)
            }
        }, {}],
        4: [function(n, t) {
            function r(n, t, r) {
                if (!n && !t && !r) throw new Error("Missing required arguments");
                if (!i.string(t)) throw new TypeError("Second argument must be a String");
                if (!i.fn(r)) throw new TypeError("Third argument must be a Function");
                if (i.node(n)) return u(n, t, r);
                if (i.nodeList(n)) return f(n, t, r);
                if (i.string(n)) return e(n, t, r);
                throw new TypeError("First argument must be a String, HTMLElement, HTMLCollection, or NodeList");
            }

            function u(n, t, i) {
                return n.addEventListener(t, i), {
                    destroy: function() {
                        n.removeEventListener(t, i)
                    }
                }
            }

            function f(n, t, i) {
                return Array.prototype.forEach.call(n, function(n) {
                    n.addEventListener(t, i)
                }), {
                    destroy: function() {
                        Array.prototype.forEach.call(n, function(n) {
                            n.removeEventListener(t, i)
                        })
                    }
                }
            }

            function e(n, t, i) {
                return o(document.body, n, t, i)
            }
            var i = n("./is"),
                o = n("delegate");
            t.exports = r
        }, {
            "./is": 3,
            delegate: 2
        }],
        5: [function(n, t) {
            function i(n) {
                var t, r, i, u;
                return "SELECT" === n.nodeName ? (n.focus(), t = n.value) : "INPUT" === n.nodeName || "TEXTAREA" === n.nodeName ? (r = n.hasAttribute("readonly"), r || n.setAttribute("readonly", ""), n.select(), n.setSelectionRange(0, n.value.length), r || n.removeAttribute("readonly"), t = n.value) : (n.hasAttribute("contenteditable") && n.focus(), i = window.getSelection(), u = document.createRange(), u.selectNodeContents(n), i.removeAllRanges(), i.addRange(u), t = i.toString()), t
            }
            t.exports = i
        }, {}],
        6: [function(n, t) {
            function i() {}
            i.prototype = {
                on: function(n, t, i) {
                    var r = this.e || (this.e = {});
                    return (r[n] || (r[n] = [])).push({
                        fn: t,
                        ctx: i
                    }), this
                },
                once: function(n, t, i) {
                    function r() {
                        u.off(n, r);
                        t.apply(i, arguments)
                    }
                    var u = this;
                    return r._ = t, this.on(n, r, i)
                },
                emit: function(n) {
                    var r = [].slice.call(arguments, 1),
                        i = ((this.e || (this.e = {}))[n] || []).slice(),
                        t = 0,
                        u = i.length;
                    for (t; t < u; t++) i[t].fn.apply(i[t].ctx, r);
                    return this
                },
                off: function(n, t) {
                    var u = this.e || (this.e = {}),
                        r = u[n],
                        f = [],
                        i, e;
                    if (r && t)
                        for (i = 0, e = r.length; i < e; i++) r[i].fn !== t && r[i].fn._ !== t && f.push(r[i]);
                    return f.length ? u[n] = f : delete u[n], this
                }
            };
            t.exports = i
        }, {}],
        7: [function(t, i, r) {
            ! function(u, f) {
                if ("function" == typeof n && n.amd) n(["module", "select"], f);
                else if (void 0 !== r) f(i, t("select"));
                else {
                    var e = {
                        exports: {}
                    };
                    f(e, u.select);
                    u.clipboardAction = e.exports
                }
            }(this, function(n, t) {
                "use strict";

                function r(n) {
                    return n && n.__esModule ? n : {
                        "default": n
                    }
                }

                function u(n, t) {
                    if (!(n instanceof t)) throw new TypeError("Cannot call a class as a function");
                }
                var i = r(t),
                    f = "function" == typeof Symbol && "symbol" == typeof Symbol.iterator ? function(n) {
                        return typeof n
                    } : function(n) {
                        return n && "function" == typeof Symbol && n.constructor === Symbol && n !== Symbol.prototype ? "symbol" : typeof n
                    },
                    e = function() {
                        function n(n, t) {
                            for (var i, r = 0; r < t.length; r++) i = t[r], i.enumerable = i.enumerable || !1, i.configurable = !0, "value" in i && (i.writable = !0), Object.defineProperty(n, i.key, i)
                        }
                        return function(t, i, r) {
                            return i && n(t.prototype, i), r && n(t, r), t
                        }
                    }(),
                    o = function() {
                        function n(t) {
                            u(this, n);
                            this.resolveOptions(t);
                            this.initSelection()
                        }
                        return e(n, [{
                            key: "resolveOptions",
                            value: function() {
                                var n = arguments.length > 0 && void 0 !== arguments[0] ? arguments[0] : {};
                                this.action = n.action;
                                this.container = n.container;
                                this.emitter = n.emitter;
                                this.target = n.target;
                                this.text = n.text;
                                this.trigger = n.trigger;
                                this.selectedText = ""
                            }
                        }, {
                            key: "initSelection",
                            value: function() {
                                this.text ? this.selectFake() : this.target && this.selectTarget()
                            }
                        }, {
                            key: "selectFake",
                            value: function() {
                                var t = this,
                                    r = "rtl" == document.documentElement.getAttribute("dir"),
                                    n;
                                this.removeFake();
                                this.fakeHandlerCallback = function() {
                                    return t.removeFake()
                                };
                                this.fakeHandler = this.container.addEventListener("click", this.fakeHandlerCallback) || !0;
                                this.fakeElem = document.createElement("textarea");
                                this.fakeElem.style.fontSize = "12pt";
                                this.fakeElem.style.border = "0";
                                this.fakeElem.style.padding = "0";
                                this.fakeElem.style.margin = "0";
                                this.fakeElem.style.position = "absolute";
                                this.fakeElem.style[r ? "right" : "left"] = "-9999px";
                                n = window.pageYOffset || document.documentElement.scrollTop;
                                this.fakeElem.style.top = n + "px";
                                this.fakeElem.setAttribute("readonly", "");
                                this.fakeElem.value = this.text;
                                this.container.appendChild(this.fakeElem);
                                this.selectedText = i.default(this.fakeElem);
                                this.copyText()
                            }
                        }, {
                            key: "removeFake",
                            value: function() {
                                this.fakeHandler && (this.container.removeEventListener("click", this.fakeHandlerCallback), this.fakeHandler = null, this.fakeHandlerCallback = null);
                                this.fakeElem && (this.container.removeChild(this.fakeElem), this.fakeElem = null)
                            }
                        }, {
                            key: "selectTarget",
                            value: function() {
                                this.selectedText = i.default(this.target);
                                this.copyText()
                            }
                        }, {
                            key: "copyText",
                            value: function() {
                                var n = void 0;
                                try {
                                    n = document.execCommand(this.action)
                                } catch (t) {
                                    n = !1
                                }
                                this.handleResult(n)
                            }
                        }, {
                            key: "handleResult",
                            value: function(n) {
                                this.emitter.emit(n ? "success" : "error", {
                                    action: this.action,
                                    text: this.selectedText,
                                    trigger: this.trigger,
                                    clearSelection: this.clearSelection.bind(this)
                                })
                            }
                        }, {
                            key: "clearSelection",
                            value: function() {
                                this.trigger && this.trigger.focus();
                                window.getSelection().removeAllRanges()
                            }
                        }, {
                            key: "destroy",
                            value: function() {
                                this.removeFake()
                            }
                        }, {
                            key: "action",
                            set: function() {
                                var n = arguments.length > 0 && void 0 !== arguments[0] ? arguments[0] : "copy";
                                if (this._action = n, "copy" !== this._action && "cut" !== this._action) throw new Error('Invalid "action" value, use either "copy" or "cut"');
                            },
                            get: function() {
                                return this._action
                            }
                        }, {
                            key: "target",
                            set: function(n) {
                                if (void 0 !== n) {
                                    if (!n || "object" !== (void 0 === n ? "undefined" : f(n)) || 1 !== n.nodeType) throw new Error('Invalid "target" value, use a valid Element');
                                    if ("copy" === this.action && n.hasAttribute("disabled")) throw new Error('Invalid "target" attribute. Please use "readonly" instead of "disabled" attribute');
                                    if ("cut" === this.action && (n.hasAttribute("readonly") || n.hasAttribute("disabled"))) throw new Error('Invalid "target" attribute. You can\'t cut text from elements with "readonly" or "disabled" attributes');
                                    this._target = n
                                }
                            },
                            get: function() {
                                return this._target
                            }
                        }]), n
                    }();
                n.exports = o
            })
        }, {
            select: 5
        }],
        8: [function(t, i, r) {
            ! function(u, f) {
                if ("function" == typeof n && n.amd) n(["module", "./clipboard-action", "tiny-emitter", "good-listener"], f);
                else if (void 0 !== r) f(i, t("./clipboard-action"), t("tiny-emitter"), t("good-listener"));
                else {
                    var e = {
                        exports: {}
                    };
                    f(e, u.clipboardAction, u.tinyEmitter, u.goodListener);
                    u.clipboard = e.exports
                }
            }(this, function(n, t, i, r) {
                "use strict";

                function u(n) {
                    return n && n.__esModule ? n : {
                        "default": n
                    }
                }

                function e(n, t) {
                    if (!(n instanceof t)) throw new TypeError("Cannot call a class as a function");
                }

                function o(n, t) {
                    if (!n) throw new ReferenceError("this hasn't been initialised - super() hasn't been called");
                    return !t || "object" != typeof t && "function" != typeof t ? n : t
                }

                function s(n, t) {
                    if ("function" != typeof t && null !== t) throw new TypeError("Super expression must either be null or a function, not " + typeof t);
                    n.prototype = Object.create(t && t.prototype, {
                        constructor: {
                            value: n,
                            enumerable: !1,
                            writable: !0,
                            configurable: !0
                        }
                    });
                    t && (Object.setPrototypeOf ? Object.setPrototypeOf(n, t) : n.__proto__ = t)
                }

                function f(n, t) {
                    var i = "data-clipboard-" + n;
                    if (t.hasAttribute(i)) return t.getAttribute(i)
                }
                var h = u(t),
                    c = u(i),
                    l = u(r),
                    a = "function" == typeof Symbol && "symbol" == typeof Symbol.iterator ? function(n) {
                        return typeof n
                    } : function(n) {
                        return n && "function" == typeof Symbol && n.constructor === Symbol && n !== Symbol.prototype ? "symbol" : typeof n
                    },
                    v = function() {
                        function n(n, t) {
                            for (var i, r = 0; r < t.length; r++) i = t[r], i.enumerable = i.enumerable || !1, i.configurable = !0, "value" in i && (i.writable = !0), Object.defineProperty(n, i.key, i)
                        }
                        return function(t, i, r) {
                            return i && n(t.prototype, i), r && n(t, r), t
                        }
                    }(),
                    y = function(n) {
                        function t(n, i) {
                            e(this, t);
                            var r = o(this, (t.__proto__ || Object.getPrototypeOf(t)).call(this));
                            return r.resolveOptions(i), r.listenClick(n), r
                        }
                        return s(t, n), v(t, [{
                            key: "resolveOptions",
                            value: function() {
                                var n = arguments.length > 0 && void 0 !== arguments[0] ? arguments[0] : {};
                                this.action = "function" == typeof n.action ? n.action : this.defaultAction;
                                this.target = "function" == typeof n.target ? n.target : this.defaultTarget;
                                this.text = "function" == typeof n.text ? n.text : this.defaultText;
                                this.container = "object" === a(n.container) ? n.container : document.body
                            }
                        }, {
                            key: "listenClick",
                            value: function(n) {
                                var t = this;
                                this.listener = l.default(n, "click", function(n) {
                                    return t.onClick(n)
                                })
                            }
                        }, {
                            key: "onClick",
                            value: function(n) {
                                var t = n.delegateTarget || n.currentTarget;
                                this.clipboardAction && (this.clipboardAction = null);
                                this.clipboardAction = new h.default({
                                    action: this.action(t),
                                    target: this.target(t),
                                    text: this.text(t),
                                    container: this.container,
                                    trigger: t,
                                    emitter: this
                                })
                            }
                        }, {
                            key: "defaultAction",
                            value: function(n) {
                                return f("action", n)
                            }
                        }, {
                            key: "defaultTarget",
                            value: function(n) {
                                var t = f("target", n);
                                if (t) return document.querySelector(t)
                            }
                        }, {
                            key: "defaultText",
                            value: function(n) {
                                return f("text", n)
                            }
                        }, {
                            key: "destroy",
                            value: function() {
                                this.listener.destroy();
                                this.clipboardAction && (this.clipboardAction.destroy(), this.clipboardAction = null)
                            }
                        }], [{
                            key: "isSupported",
                            value: function() {
                                var n = arguments.length > 0 && void 0 !== arguments[0] ? arguments[0] : ["copy", "cut"],
                                    i = "string" == typeof n ? [n] : n,
                                    t = !!document.queryCommandSupported;
                                return i.forEach(function(n) {
                                    t = t && !!document.queryCommandSupported(n)
                                }), t
                            }
                        }]), t
                    }(c.default);
                n.exports = y
            })
        }, {
            "./clipboard-action": 7,
            "good-listener": 4,
            "tiny-emitter": 6
        }]
    }, {}, [8])(8)
}),
    function(n) {
        function t(t, r) {
            this.element = t;
            this.options = n.extend(i, r);
            this.chatbox = n(this.element).find(".chatbox");
            this.chatresult = n(this.chatbox).find("#chatbox-result");
            this.chatboxbody = n(this.chatbox).find(".chatbox-body");
            this.chatform = n(this.chatbox).find("#chatbox-form");
            this.chatid = n(this.chatform).find("input[name=clientid]");
            this.chatkey = n(this.chatform).find("input[name=chatkey]");
            this.chatname = n(this.chatform).find("input[name=clientname]");
            this.chattype = n(this.chatform).find("input[name=clienttype]");
            this.chatavatar = n(this.chatform).find("input[name=clientavatar]");
            this.chatip = n(this.chatform).find("input[name=clientip]");
            this.chatmessage = n(this.chatform).find("input[name=message]");
            this.chatgroup = this.chatkey.val();
            this.deviceToken = "";
            this.userVisible = !0;
            this.notifySound = "https://naptien24h.vn/admincp/Plugins/Chat/img/notify.mp3";
            this.isclient = n(this.chatform).find("input[name=isClient]");
            this.options.isServer && (this.chatlist = n(this.element).find(".chatlist-active"));
            this._default = i;
            this.firebase = this.options.firebase;
            this.database = null;
            this.debug = this.options.debug;
            this.isLoadingMessage = !0;
            this.isSubmitting = !1;
            this.ischatting = !1;
            this.chatitemtemplate = '<div class="chat-item"><div class="name"><span><\/span> <i class="fa fa-clock-o"><\/i> <small><\/small><\/div><div class="chat-content"><\/div><\/div>';
            this.chatlistitemtemplate = '<div class="chatlist-item"><div class="avatar"><img><\/div><div class="info"><h5><\/h5><div class="time"><\/div><div class="msg"><\/div><div class="url"><\/div><div><span class="agent-name"><\/span> <span class="status"><\/span><\/div><\/div><\/div>';
            this.init()
        }
        var i = {
                apiKey: "AIzaSyBDH5uhAV2z4-CehtXj_qX6_mvT1glBqLs",
                authDomain: "vn-ctnet-thanhtoan.firebaseapp.com",
                databaseURL: "https://vn-ctnet-thanhtoan.firebaseio.com",
                fcmSenderId: "14345431474",
                fcmAuthentication: "key=AAAAA1cN6bI:APA91bGS2NxQUUYYipoSqINnX8b8TwZRV9l7i4eF3kingWSyVLYDnfz244iL-L4BnUxO_pG8SATu5NGC37mFYCY62SUPrN8vCv9GT8gbdxdH8puqcuGAyGFbPPGp2lgwlFdRSDKz3Gmt",
                debug: !1,
                isServer: !1
            },
            r;
        t.prototype.init = function() {
            var t, i;
            if (!this.checkFirebase()) return !1;
            t = this.randomUser();
            this.chatid.val() == "" && this.chatid.val(t);
            this.chatkey.val() == "" && this.chatkey.val(t);
            this.chatname.val() == "" && this.chatname.val(t);
            i = {
                apiKey: this.options.apiKey,
                authDomain: this.options.authDomain,
                databaseURL: this.options.databaseURL,
                messagingSenderId: this.options.fcmSenderId
            };
            this.firebase.initializeApp(i);
            this.database = this.firebase.database().ref("chats");
            this.messaging = this.firebase.messaging();
            this.database.off();
            this.chatgroup = this.chatkey.val();
            window.userVisible(function() {
                this.userVisible = window.userVisible()
            }.bind(this));
            this.userVisible = window.userVisible();
            this.chatform[0].addEventListener("submit", this.submitMessage.bind(this));
            this.options.isServer || (this.database.child("typing").child(this.chatgroup).onDisconnect().remove(), this.chatmessage.bind("propertychange change click keyup input paste", function() {
                this.setClientTyping(this.chatmessage.val())
            }.bind(this)));
            this.initMessaging();
            this.options.isServer ? (this.loadChatlist(), this.disableChatForm(), this.adminIsOnline(), this.log("admin")) : (this.log("client"), this.loadMessage(), this.detectStatus(), this.initChatboxClient());
            n.isFunction(this.options.onLoad) && this.options.onLoad.call(this)
        };
        t.prototype.initMessaging = function() {
            this.messaging.requestPermission().then(function() {
                this.log("Notification permission granted.");
                this.saveMessagingDeviceToken()
            }.bind(this)).catch(function(n) {
                this.log("Unable to get permission to notify." + n)
            }.bind(this));
            this.messaging.onTokenRefresh(function() {
                this.messaging.getToken().then(function(n) {
                    this.log("Token refreshed.");
                    this.setTokenSentToServer(!1);
                    this.sendTokenToServer(n)
                }).catch(function(n) {
                    this.log("Unable to retrieve refreshed token " + n)
                }.bind(this))
            });
            this.messaging.onMessage(function(n) {
                this.log("receiving message");
                this.playNotifySound();
                var t = n.notification,
                    i = t.title,
                    r = {
                        body: t.body,
                        icon: t.icon,
                        silent: !1,
                        click_action: "https://naptien24h.vn",
                        timestamp: new Date
                    },
                    u = new Notification(i, r);
                u.addEventListener("click", function(n) {
                    this.log(n)
                }.bind(this))
            }.bind(this))
        };
        t.prototype.saveMessagingDeviceToken = function() {
            this.messaging.getToken().then(function(n) {
                n ? (this.setTokenSentToServer(!1), this.sendTokenToServer(n)) : this.requestNotificationsPermissions()
            }.bind(this)).catch(function(n) {
                this.log("Unable to get messaging token." + n)
            }.bind(this))
        };
        t.prototype.requestNotificationsPermissions = function() {
            this.log("Requesting notifications permission...");
            this.messaging.requestPermission().then(function() {
                this.saveMessagingDeviceToken()
            }.bind(this)).catch(function(n) {
                this.log("Unable to get permission to notify." + n)
            }.bind(this))
        };
        t.prototype.sendTokenToServer = function(n) {
            this.isTokenSentToServer() ? this.log("Token already sent to server so won't send it again unless it changes") : (this.deviceToken = n, this.setTokenSentToServer(!0))
        };
        t.prototype.isTokenSentToServer = function() {
            return window.localStorage.getItem("sentToServer") == 1
        };
        t.prototype.setTokenSentToServer = function(n) {
            window.localStorage.setItem("sentToServer", n ? 1 : 0)
        };
        t.prototype.sendNotify = function(t, i) {
            n.ajax({
                url: "https://fcm.googleapis.com/fcm/send",
                type: "POST",
                dataType: "json",
                headers: {
                    "Content-Type": "application/json",
                    Authorization: this.options.fcmAuthentication
                },
                data: JSON.stringify({
                    to: t,
                    notification: i
                }),
                success: function() {}.bind(this)
            })
        };
        t.prototype.playNotifySound = function() {
            n("body").find(".sound-player").remove();
            var t = '<audio class="sound-player" autoplay="autoplay" style="display:none;"><source src="' + this.notifySound + '" /><embed src="' + this.notifySound + '" hidden="true" autostart="true" loop="false"/><\/audio>';
            n(t).appendTo("body")
        };
        t.prototype.callCreateGroup = function(t) {
            this.api = "https://naptien24h.vn/api/chat/createGroup";
            n.ajax({
                url: this.api,
                type: "POST",
                headers: {
                    "Content-Type": "application/x-www-form-urlencoded"
                },
                xhrFields: {
                    withCredentials: !0
                },
                data: {
                    groupKey: t
                },
                success: function() {}.bind(this)
            })
        };
        t.prototype.callCreateConversation = function(t) {
            this.api = "https://naptien24h.vn/api/chat/createConversation";
            n.ajax({
                url: this.api,
                type: "POST",
                headers: {
                    "Content-Type": "application/x-www-form-urlencoded"
                },
                xhrFields: {
                    withCredentials: !0
                },
                data: t,
                success: function(n) {
                    n.status && (this.currentConversationId = n.data.conversation.conversationId)
                }.bind(this)
            })
        };
        t.prototype.callJoinConversation = function(t, i) {
            this.api = "https://naptien24h.vn/api/chat/joinconversation";
            n.ajax({
                url: this.api,
                type: "POST",
                headers: {
                    "Content-Type": "application/x-www-form-urlencoded"
                },
                xhrFields: {
                    withCredentials: !0
                },
                data: {
                    groupKey: t,
                    agentId: i
                },
                success: function() {}.bind(this)
            })
        };
        t.prototype.callEndConversation = function(t) {
            this.api = "https://naptien24h.vn/api/chat/endconversation?groupKey=" + t;
            this.log(this.api);
            n.ajax({
                url: this.api,
                type: "GET",
                success: function() {}.bind(this)
            })
        };
        t.prototype.callSendMessage = function(t) {
            this.api = "https://naptien24h.vn/api/chat/sendmessage";
            n.ajax({
                url: this.api,
                type: "POST",
                headers: {
                    "Content-Type": "application/x-www-form-urlencoded"
                },
                xhrFields: {
                    withCredentials: !0
                },
                data: t,
                success: function() {}.bind(this)
            })
        };
        t.prototype.getCurrentUser = function() {
            return {
                name: this.chatname.val(),
                avatar: this.chatavatar.val() || "https://naptien24h.vn/Uploads/Images/avatar.png",
                ip: this.chatip.val(),
                type: this.chattype.val(),
                id: this.chatid.val(),
                isClient: this.isclient.val() == "true",
                url: window.location.href
            }
        };
        t.prototype.loadMessage = function() {
            this.messageRef = this.firebase.database().ref("chats/groups/" + this.chatgroup).child("messages");
            this.messageRef.off();
            this.chatresult.html("");
            this.messageRef.limitToLast(100).off("child_added");
            this.messageRef.limitToLast(100).off("child_changed");
            var n = function(n) {
                var t = n.val();
                this.displayMessage(n.key, t.name, t.content, t.avatar, t.type, t.time)
            }.bind(this);
            this.messageRef.limitToLast(100).on("child_added", n);
            this.messageRef.limitToLast(100).on("child_changed", n)
        };
        t.prototype.submitMessage = function(n) {
            var t, i, r;
            if (n.preventDefault(), this.isLoadingMessage = !1, this.isSubmitting) return !1;
            if (this.isSubmitting = !0, this.chatmessage.val()) {
                this.messageRoot = this.firebase.database().ref("chats/groups");
                this.messageRoot.off();
                this.chatlistRef || (this.chatlistRef = this.firebase.database().ref("chats").child("actives"), this.chatlistRef.off());
                t = {
                    name: this.chatname.val(),
                    content: this.chatmessage.val(),
                    avatar: this.chatavatar.val() || "https://naptien24h.vn/Uploads/Images/avatar.png",
                    ip: this.chatip.val(),
                    type: this.chattype.val(),
                    time: this.firebase.database.ServerValue.TIMESTAMP,
                    id: this.chatid.val(),
                    isClient: this.isclient.val() == "true",
                    url: window.location.href
                };
                this.chatform.trigger("reset");
                this.chatmessage.focus();
                i = function(n) {
                    n.child("agent").exists() ? this.saveMessage(t) : (this.callCreateGroup(this.chatgroup), this.messageRoot.child(this.chatgroup).update({
                        agent: {},
                        joinTime: 0
                    }).then(function() {
                        this.saveMessage(t)
                    }.bind(this)))
                }.bind(this);
                this.messageRoot.child(this.chatgroup).once("value", i);
                if (!this.options.isServer) {
                    r = function() {
                        this.chatlistRef.child(this.chatgroup).update(t).then(function() {
                            this.chatlistRef.child(this.chatgroup).onDisconnect().remove();
                            var n = {
                                name: t.name,
                                content: t.content,
                                avatar: t.avatar,
                                ip: t.ip,
                                type: t.type,
                                id: t.id,
                                isClient: t.isClient,
                                url: t.url,
                                groupKey: this.chatgroup
                            };
                            this.callCreateConversation(n)
                        }.bind(this))
                    }.bind(this);
                    this.chatlistRef.child(this.chatgroup).once("value", r)
                }
                this.options.isServer ? this.ischatting = !1 : (this.ischatting = !0, this.checkClientLeaving())
            }
            setTimeout(function() {
                this.isSubmitting = !1
            }.bind(this), 2e3)
        };
        t.prototype.saveMessage = function(n) {
            this.messageRef.push(n).then(function() {
                var t = {};
                t.avatar = n.avatar;
                t.content = n.content;
                t.id = n.id;
                t.ip = n.ip;
                t.isClient = n.isClient;
                t.name = n.name;
                t.type = n.type;
                t.url = n.url;
                t.groupKey = this.chatgroup;
                this.callSendMessage(t);
                this.isSubmitting = !1;
                this.setClientTyping("")
            }.bind(this)).catch(function(n) {
                this.log(n)
            }.bind(this))
        };
        t.prototype.displayMessage = function(t, i, r, u, f, e) {
            var o = document.getElementById(t),
                s, h;
            o || (s = document.createElement("div"), s.innerHTML = this.chatitemtemplate, o = s.firstChild, o.setAttribute("id", t));
            h = new Date(e);
            n(o).find(".name span").text(i);
            f == "user" ? n(o).addClass("client") : n(o).addClass("server");
            n(o).find(".name small").text(h.customFormat("#DD#/#MM#/#YYYY# #hh#:#mm#:#ss#"));
            n(o).find(".chat-content").html(this.htmlEntities(r).replace(/\n/g, "<br>"));
            this.chatresult.append(n(o));
            this.isLoadingMessage ? this.scrollChatbox(0) : this.scrollChatbox(0);
            this.isLoadingMessage || i == this.chatname.val() || this.options.isServer || (this.userVisible ? this.playNotifySound() : (this.log("send notify.."), this.sendNotify(this.deviceToken, {
                title: i,
                body: r,
                icon: u
            })))
        };
        t.prototype.loadChatlist = function() {
            var n, t, i;
            this.chatlistRef || (this.chatlistRef = this.firebase.database().ref("chats").child("actives"), this.chatlistRef.off());
            this.chatlist.html("");
            n = function(n) {
                var t = n.val();
                this.displayChatlist(n.key, t);
                this.log("chat list added")
            }.bind(this);
            t = function(n) {
                var t = n.val();
                this.displayChatlist(n.key, t);
                this.playNotifySound();
                this.log("chat list changed");
                this.userVisible || (this.log("send notify.."), this.sendNotify(this.deviceToken, {
                    title: t.name,
                    body: t.content,
                    icon: t.avatar
                }))
            }.bind(this);
            this.chatlistRef.on("child_added", n);
            this.chatlistRef.on("child_changed", t);
            i = function(n) {
                var t = n.val();
                this.chatlist.find("#chatlist_" + n.key).remove();
                n.key == this.groupactive && this.resetChatForm();
                this.callEndConversation(t.id)
            }.bind(this);
            this.chatlistRef.on("child_removed", i)
        };
        t.prototype.displayChatlist = function(t, i) {
            var r = document.getElementById("chatlist_" + t),
                u, f;
            r || (u = document.createElement("div"), u.innerHTML = this.chatlistitemtemplate, r = u.firstChild, r.setAttribute("id", "chatlist_" + t), r.setAttribute("data-chatkey", t));
            f = new Date(i.time);
            n(r).addClass("new").delay(5e3).queue(function() {
                n(r).removeClass("new").dequeue()
            });
            n(r).find("img").attr("src", i.avatar);
            n(r).find(".time").html('<i class="fa fa-clock-o"><\/i> ' + f.customFormat("#DD#/#MM#/#YYYY# #hh#:#mm#:#ss#"));
            n(r).find(".msg").html('<i class="fa fa-comment"><\/i> ' + i.content);
            n(r).find("h5").text(i.name);
            i.agent && (n(r).find(".status").text("Đang hỗ trợ"), n(r).find(".agent-name").html('<i class="fa fa-user"><\/i> ' + i.agent.name));
            n(r).off("click");
            n(r).on("click", function() {
                if (n(r).hasClass("active")) return this.scrollChatbox(50), !1;
                this.chatlist.find(".chatlist-item").removeClass("active");
                n(r).addClass("active");
                this.takeConversation(t)
            }.bind(this));
            this.chatlist.prepend(n(r))
        };
        t.prototype.takeConversation = function(n) {
            var t = this.getCurrentUser(),
                i;
            this.log(t);
            this.chatlistRef || (this.chatlistRef = this.firebase.database().ref("chats").child("actives"), this.chatlistRef.off());
            this.chatkey.val(n);
            this.chatgroup = this.chatkey.val();
            this.bindClientTyping(this.chatgroup);
            this.groupactive = this.chatgroup;
            this.log("group active" + this.groupactive);
            i = function(n) {
                var i = n.val();
                this.displayClientInfo(i);
                this.chatlistRef.child(this.chatgroup + "/agent").update(t).then(function() {
                    this.callJoinConversation(this.groupactive, t.id);
                    this.loadMessage();
                    this.enableChatForm()
                }.bind(this))
            }.bind(this);
            this.chatlistRef.child(this.chatgroup).once("value", i);
            this.isLoadingMessage || this.scrollChatbox(50)
        };
        t.prototype.adminIsOnline = function() {
            this.chatAdminOnlineRef || (this.chatAdminOnlineRef = this.firebase.database().ref("chats").child("admins"), this.chatAdminOnlineRef.off());
            var n = this.getCurrentUser();
            this.log(n);
            this.chatAdminOnlineRef.child(n.id).update(n, function() {
                this.chatAdminOnlineRef.child(n.id).onDisconnect().remove()
            }.bind(this))
        };
        t.prototype.disableChatForm = function() {
            this.chatform.find("input").each(function(t, i) {
                n(i).attr("readonly", "readonly")
            }.bind(this))
        };
        t.prototype.enableChatForm = function() {
            this.chatform.find("input").each(function(t, i) {
                n(i).removeAttr("readonly")
            }.bind(this))
        };
        t.prototype.resetChatForm = function() {
            this.chatresult.html("");
            this.chatkey.val("");
            this.chatgroup = this.chatkey.val();
            this.messageRef && (this.messageRef.off("child_added"), this.messageRef.off("child_changed"));
            this.clientInfo = n(this.chatbox).find(".chatbox-admin");
            this.clientInfo.find(".name").text("");
            this.clientInfo.find(".ip").text("");
            this.clientInfo.find(".item-url a").attr("href", "");
            this.clientInfo.find(".item-url a").text("");
            this.clientInfo.hasClass("hidden") || this.clientInfo.addClass("hidden");
            this.disableChatForm()
        };
        t.prototype.detectStatus = function() {
            var t = function(t) {
                var i = t.val();
                t.hasChild("admins") ? n(this.element).find(".chatbox-client .chatbox-title span").text("Chat hỗ trợ hoặc gọi 0935 131 858") : n(this.element).find(".chatbox-client .chatbox-title span").text("Offline hãy gọi 0935 131 858")
            }.bind(this);
            this.database.on("value", t)
        };
        t.prototype.initChatboxClient = function() {
            this.chatbox.addClass("chatbox-client");
            var t = n(this.element).find(".chatbox-client");
            t.find(".chatbox-header").on("click", function() {
                var n = t.find(".chatbox-body"),
                    i = t.find(".chatbox-footer");
                n.is(":visible") ? (n.hide(300), i.hide(0)) : (n.show(300), i.show(0))
            }.bind(this))
        };
        t.prototype.setClientTyping = function(n) {
            this.database.child("typing").child(this.chatgroup).update({
                content: n
            }).then(function() {}.bind(this))
        };
        t.prototype.bindClientTyping = function(t) {
            this.log(t);
            this.clientInfo = n(this.chatbox).find(".chatbox-admin");
            var i = function(n) {
                var t = n.val();
                this.clientInfo.find(".item-pre-typing .content").text(t == "" ? "..." : t)
            }.bind(this);
            this.database.child("typing").child(t).off("child_changed");
            this.database.child("typing").child(t).on("child_changed", i);
            this.log("end bind Client typing")
        };
        t.prototype.displayClientInfo = function(t) {
            this.clientInfo = n(this.chatbox).find(".chatbox-admin");
            this.clientInfo.find(".name").text(t.name);
            this.clientInfo.find(".ip").text(t.ip);
            t.url && t.url !== "" && (this.clientInfo.find(".item-url a").attr("href", t.url), this.clientInfo.find(".item-url a").text(t.url));
            this.clientInfo.hasClass("hidden") && this.clientInfo.removeClass("hidden")
        };
        t.prototype.scrollChatbox = function(n) {
            this.chatboxbody.animate({
                scrollTop: this.chatboxbody.prop("scrollHeight")
            }, n)
        };
        t.prototype.checkClientLeaving = function() {
            n(window).bind("beforeunload", function(n) {
                if (this.ischatting) return "Bạn muốn kết thúc cuộc trò chuyện này?";
                n = null
            }.bind(this))
        };
        t.prototype.checkFirebase = function() {
            return !this.firebase || !(this.firebase.app instanceof Function) ? (window.alert("You have not configured and imported the Firebase SDK. Make sure you go through the codelab setup instructions and make sure you are running the codelab using `firebase serve`"), !1) : !0
        };
        t.prototype.log = function(n) {
            this.debug && console.log(n)
        };
        t.prototype.htmlEntities = function(n) {
            return String(n).replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;").replace(/"/g, "&quot;")
        };
        t.prototype.randomUser = function() {
            var n = "",
                i = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789",
                t;
            for (n += Math.round(+new Date / 1e3), t = 0; t < 10; t++) n += i.charAt(Math.floor(Math.random() * i.length));
            return n
        };
        r = null;
        n.fn.extend({
            simplechat: function(n) {
                return this.each(function() {
                    r = new t(this, n)
                })
            }
        })
    }(jQuery);
Date.prototype.customFormat = function(n) {
    var s, h, c, l, a, u, v, y, p, i, w, r, b, t, k, f, d, e, g, nt, o, tt;
    return h = ((s = this.getFullYear()) + "").slice(-2), a = (u = this.getMonth() + 1) < 10 ? "0" + u : u, l = (c = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"][u - 1]).substring(0, 3), p = (i = this.getDate()) < 10 ? "0" + i : i, y = (v = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"][this.getDay()]).substring(0, 3), tt = i >= 10 && i <= 20 ? "th" : (o = i % 10) == 1 ? "st" : o == 2 ? "nd" : o == 3 ? "rd" : "th", n = n.replace("#YYYY#", s).replace("#YY#", h).replace("#MMMM#", c).replace("#MMM#", l).replace("#MM#", a).replace("#M#", u).replace("#DDDD#", v).replace("#DDD#", y).replace("#DD#", p).replace("#D#", i).replace("#th#", tt), t = r = this.getHours(), t == 0 && (t = 24), t > 12 && (t -= 12), b = t < 10 ? "0" + t : t, w = r < 10 ? "0" + r : r, nt = (g = r < 12 ? "am" : "pm").toUpperCase(), k = (f = this.getMinutes()) < 10 ? "0" + f : f, d = (e = this.getSeconds()) < 10 ? "0" + e : e, n.replace("#hhhh#", w).replace("#hhh#", r).replace("#hh#", b).replace("#h#", t).replace("#mm#", k).replace("#m#", f).replace("#ss#", d).replace("#s#", e).replace("#ampm#", g).replace("#AMPM#", nt)
};
window.userVisible = function() {
    var n, t, i = {
        hidden: "visibilitychange",
        webkitHidden: "webkitvisibilitychange",
        mozHidden: "mozvisibilitychange",
        msHidden: "msvisibilitychange"
    };
    for (n in i)
        if (n in document) {
            t = i[n];
            break
        } return function(i) {
        return i && document.addEventListener(t, i), !document[n]
    }
}();
Date.prototype.customFormat = function(n) {
    var s, h, c, l, a, u, v, y, p, i, w, r, b, t, k, f, d, e, g, nt, o, tt;
    return h = ((s = this.getFullYear()) + "").slice(-2), a = (u = this.getMonth() + 1) < 10 ? "0" + u : u, l = (c = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"][u - 1]).substring(0, 3), p = (i = this.getDate()) < 10 ? "0" + i : i, y = (v = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"][this.getDay()]).substring(0, 3), tt = i >= 10 && i <= 20 ? "th" : (o = i % 10) == 1 ? "st" : o == 2 ? "nd" : o == 3 ? "rd" : "th", n = n.replace("#YYYY#", s).replace("#YY#", h).replace("#MMMM#", c).replace("#MMM#", l).replace("#MM#", a).replace("#M#", u).replace("#DDDD#", v).replace("#DDD#", y).replace("#DD#", p).replace("#D#", i).replace("#th#", tt), t = r = this.getHours(), t == 0 && (t = 24), t > 12 && (t -= 12), b = t < 10 ? "0" + t : t, w = r < 10 ? "0" + r : r, nt = (g = r < 12 ? "am" : "pm").toUpperCase(), k = (f = this.getMinutes()) < 10 ? "0" + f : f, d = (e = this.getSeconds()) < 10 ? "0" + e : e, n.replace("#hhhh#", w).replace("#hhh#", r).replace("#hh#", b).replace("#h#", t).replace("#mm#", k).replace("#m#", f).replace("#ss#", d).replace("#s#", e).replace("#ampm#", g).replace("#AMPM#", nt)
};
isSubmiting = !1;
$(function() {
    var t = new Clipboard(".copy"),
        n;
    t.on("success", function(n) {
        $(n.trigger).tooltip({
            trigger: "manual"
        });
        $(n.trigger).tooltip("show");
        $(n.trigger).on("mouseout", function() {
            $(this).tooltip("hide")
        })
    });
    $(".modal-dialog").parent().on("show.bs.modal", function(n) {
        n.relatedTarget && $(n.relatedTarget.attributes["data-target"].value).appendTo("body")
    });
    if ($("#topup-form").length > 0) {
        add_active_class();
        get_server_price($("#topup-form #amount_id").val(), $("#topup-form #bank_id").val(), $("#topup-form #quantity").val());
        $("#topup-form").on("submit", function(n) {
            if (n.preventDefault(), isSubmiting) return !1;
            isSubmiting = !0;
            var i = $(this).serialize(),
                t = $(this);
            $.ajax({
                url: $(this).attr("action"),
                method: $(this).attr("method"),
                data: i,
                success: function(n) {
                    var i, r;
                    if (console.log(n), n.status) showAlert(t, "<strong><i class='fa fa-refresh fa-spin'><\/i> Khởi tạo giao dịch thành công, đang chuyển hướng ...<\/strong>", "success"), scrollTo(t), $("#topup-form input").attr("disabled", "disabled"), $("#topup-form button").attr("disabled", "disabled"), $("#topup-form").attr("disabled", "disabled"), window.location.href = n.data.Url;
                    else {
                        for (i = "<p>", i += "<h4>Đã có lỗi xảy ra khi khởi tạo giao dịch<\/h4>", r = 0; r < n.messages.length; r++) i += "- " + n.messages[r] + "<br/>";
                        i += "<\/p>";
                        showAlert(t, i, "danger");
                        scrollTo(t)
                    }
                }
            }).done(function() {
                isSubmiting = !1
            })
        })
    }
    $("#login_form").on("submit", function(n) {
        n.preventDefault();
        var t = $(this).serialize();
        $.ajax({
            url: $(this).attr("action"),
            method: $(this).attr("method"),
            data: t,
            success: function(n) {
                n.status && n.isReload ? window.location.reload() : showMessage("", n.message, n.type);
                n.isReset && $("#login_form").trigger("reset")
            }
        })
    });
    htmlAjaxLoad();
    $(".topupItem").on("click", function(n) {
        if (n.preventDefault(), $(window).width() > 991) return !1;
        var t = $("#DetailTopupGroupModal"),
            i = $(this).attr("data-detail");
        t.find(".modal-body").html("");
        t.find(".modal-body").load(i, function() {
            t.appendTo("body").modal("show")
        })
    });
    $("#postItem").length > 0 && (n = $("#postItem"), $.ajax({
        url: n.attr("data-url"),
        method: "GET",
        data: {},
        success: function() {}
    }));
    $("#btn-chose-avatar").on("click", function(n) {
        n.preventDefault();
        $("#profile-avatar").trigger("click")
    });
    $(".history-payment-content").find("table tr").on("click", function(n) {
        n.preventDefault();
        $(".history-payment-content").fadeOut(100, function() {
            $(".history-payment-detail").fadeIn(100, function() {
                $(this).find("#backtohistory").on("click", function() {
                    $(".history-payment-detail").fadeOut(100, function() {
                        $(".history-payment-content").fadeIn(100)
                    })
                })
            })
        })
    });
    $(".readmore-content-item").each(function() {
        $(this).load($(this).attr("data-url"))
    });
    $("#preloader").delay(400).fadeOut("fast", function() {
        $(this).remove()
    });
    $("#sidebar").length && scrollSidebar();
    $(window).width() < 1024 && $(".guilde-content").find("img").removeAttr("style");
    show_hide_chatbox()
});
var isLoadingForm = !1,
    isGetPrice = !1,
    pmiKey = "_pmi",
    ctmKey = "_ctm",
    apiPrefix = $('meta[name="host"]').attr("content") + "/api/";
$(function () {
    $(".topup-nav-item:not(.silent)").on("click", function (n) {
        if ($(this).hasClass("nav-item-direct")) return !0;
        if (isLoadingForm) return !1;
        var t = $(this).attr("data-url");
        if ($(this).parents("li:not(.nav-parent-item)").hasClass("active")) return !1;
        $(".main-nav  li.active").removeClass("active");
        $(this).parents("li").addClass("active");
        $(this).parents("li").siblings().find(".nav-sub").hide("fast");
        $(this).parents("li").hasClass("nav-sub-item") && $(this).parents(".nav-sub").hide("fast");
        loadTopupForm(t);
        n.preventDefault()
    });
    $(".topup-nav-item.silent").on("click", function (n) {
        var t, i;
        n.preventDefault();
        $(".main-nav li.active").removeClass("active");
        t = $(this).parents("li");
        t.addClass("active");
        i = t.find(".nav-sub");
        i.length && i.show("fast")
    });
    $(".main-nav  li.active .topup-nav-item").each(function () {
        if (!$(this).hasClass("nav-item-direct")) {
            var n = $(this).attr("data-url");
            loadTopupForm(n)
        }
    });
    $("#quick-topup-form").length && formLoaded()
});

//$(function () {
     
//    if ($('tr .sta-service').hasClass('stop')) {
//        $('.pro-discount tr td').css('opacity', '.7')
//    }
   
//});

