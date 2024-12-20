/* Minification failed. Returning unminified contents.
(763,39-40): run-time error JS1004: Expected ';': {
(763,41-42): run-time error JS1195: Expected expression: |
(763,42-43): run-time error JS1014: Invalid character: \
(763,43-44): run-time error JS1014: Invalid character: \
(763,44-66): run-time error JS1015: Unterminated string constant: ":<>\?]/g.test(str);
(937,1-2): run-time error JS1107: Expecting more source characters
(762,1-31): run-time error JS1301: End of file encountered before function is properly closed: function isCharacterValid(str)
(763,13-39): run-time error JS5017: Syntax error in regular expression: /[~`!#$%\^&*+=\-\[\]\\';,/
 */
/**
 * @license AngularJS v1.3.11
 * (c) 2010-2014 Google, Inc. http://angularjs.org
 * License: MIT
 */
(function (n, t, i) {
    "use strict";

    function v(n, t) {
        return t = t || Error, function () {
            var u = arguments[0], e = "[" + (n ? n + ":" : "") + u + "] ", o = arguments[1], f = arguments, i, r;
            for (i = e + o.replace(/\{\d+\}/g, function (n) {
                var t = +n.slice(1, -1);
                return t + 2 < f.length ? co(f[t + 2]) : n
            }), i = i + "\nhttp://errors.angularjs.org/1.3.11/" + (n ? n + "/" : "") + u, r = 2; r < arguments.length; r++) i = i + (r == 2 ? "?" : "&") + "p" + (r - 2) + "=" + encodeURIComponent(co(arguments[r]));
            return new t(i)
        }
    }

    function di(n) {
        if (n == null || gi(n)) return !1;
        var t = n.length;
        return n.nodeType === vt && t ? !0 : c(n) || o(n) || t === 0 || typeof t == "number" && t > 0 && t - 1 in n
    }

    function r(n, t, i) {
        var u, f, e;
        if (n) if (l(n)) for (u in n) u != "prototype" && u != "length" && u != "name" && (!n.hasOwnProperty || n.hasOwnProperty(u)) && t.call(i, n[u], u, n); else if (o(n) || di(n)) for (e = typeof n != "object", u = 0, f = n.length; u < f; u++) (e || u in n) && t.call(i, n[u], u, n); else if (n.forEach && n.forEach !== r) n.forEach(t, i, n); else for (u in n) n.hasOwnProperty(u) && t.call(i, n[u], u, n);
        return n
    }

    function pe(n) {
        return Object.keys(n).sort()
    }

    function nl(n, t, i) {
        for (var r = pe(n), u = 0; u < r.length; u++) t.call(i, n[r[u]], r[u]);
        return r
    }

    function we(n) {
        return function (t, i) {
            n(i, t)
        }
    }

    function br() {
        return ++gc
    }

    function be(n, t) {
        t ? n.$$hashKey = t : delete n.$$hashKey
    }

    function a(n) {
        for (var t, u, i, e, f, o = n.$$hashKey, r = 1, s = arguments.length; r < s; r++) if (t = arguments[r], t) for (u = Object.keys(t), i = 0, e = u.length; i < e; i++) f = u[i], n[f] = t[f];
        return be(n, o), n
    }

    function g(n) {
        return parseInt(n, 10)
    }

    function ke(n, t) {
        return a(Object.create(n), t)
    }

    function s() {
    }

    function ct(n) {
        return n
    }

    function nt(n) {
        return function () {
            return n
        }
    }

    function e(n) {
        return typeof n == "undefined"
    }

    function u(n) {
        return typeof n != "undefined"
    }

    function h(n) {
        return n !== null && typeof n == "object"
    }

    function c(n) {
        return typeof n == "string"
    }

    function k(n) {
        return typeof n == "number"
    }

    function lt(n) {
        return ni.call(n) === "[object Date]"
    }

    function l(n) {
        return typeof n == "function"
    }

    function kr(n) {
        return ni.call(n) === "[object RegExp]"
    }

    function gi(n) {
        return n && n.window === n
    }

    function nr(n) {
        return n && n.$evalAsync && n.$watch
    }

    function tl(n) {
        return ni.call(n) === "[object File]"
    }

    function il(n) {
        return ni.call(n) === "[object FormData]"
    }

    function rl(n) {
        return ni.call(n) === "[object Blob]"
    }

    function tr(n) {
        return typeof n == "boolean"
    }

    function dr(n) {
        return n && l(n.then)
    }

    function de(n) {
        return !!(n && (n.nodeName || n.prop && n.attr && n.find))
    }

    function ul(n) {
        for (var i = {}, r = n.split(","), t = 0; t < r.length; t++) i[r[t]] = !0;
        return i
    }

    function pt(n) {
        return y(n.nodeName || n[0] && n[0].nodeName)
    }

    function ir(n, t) {
        var i = n.indexOf(t);
        return i >= 0 && n.splice(i, 1), t
    }

    function ti(n, t, i, u) {
        var l, c, f, e, a, s;
        if (gi(n) || nr(n)) throw hi("cpws", "Can't copy! Making copies of Window or Scope instances is not supported.");
        if (t) {
            if (n === t) throw hi("cpi", "Can't copy! Source and destination are identical.");
            if (i = i || [], u = u || [], h(n)) {
                if (c = i.indexOf(n), c !== -1) return u[c];
                i.push(n);
                u.push(t)
            }
            if (o(n)) for (t.length = 0, e = 0; e < n.length; e++) f = ti(n[e], null, i, u), h(n[e]) && (i.push(n[e]), u.push(f)), t.push(f); else {
                a = t.$$hashKey;
                o(t) ? t.length = 0 : r(t, function (n, i) {
                    delete t[i]
                });
                for (s in n) n.hasOwnProperty(s) && (f = ti(n[s], null, i, u), h(n[s]) && (i.push(n[s]), u.push(f)), t[s] = f);
                be(t, a)
            }
        } else t = n, n && (o(n) ? t = ti(n, [], i, u) : lt(n) ? t = new Date(n.getTime()) : kr(n) ? (t = new RegExp(n.source, n.toString().match(/[^\/]*$/)[0]), t.lastIndex = n.lastIndex) : h(n) && (l = Object.create(Object.getPrototypeOf(n)), t = ti(n, l, i, u)));
        return t
    }

    function at(n, t) {
        var i, u, r;
        if (o(n)) for (t = t || [], i = 0, u = n.length; i < u; i++) t[i] = n[i]; else if (h(n)) {
            t = t || {};
            for (r in n) r.charAt(0) === "$" && r.charAt(1) === "$" || (t[r] = n[r])
        }
        return t || n
    }

    function et(n, t) {
        if (n === t) return !0;
        if (n === null || t === null) return !1;
        if (n !== n && t !== t) return !0;
        var f = typeof n, s = typeof t, e, r, u;
        if (f == s && f == "object") if (o(n)) {
            if (!o(t)) return !1;
            if ((e = n.length) == t.length) {
                for (r = 0; r < e; r++) if (!et(n[r], t[r])) return !1;
                return !0
            }
        } else {
            if (lt(n)) return lt(t) ? et(n.getTime(), t.getTime()) : !1;
            if (kr(n) && kr(t)) return n.toString() == t.toString();
            if (nr(n) || nr(t) || gi(n) || gi(t) || o(t)) return !1;
            u = {};
            for (r in n) if (r.charAt(0) !== "$" && !l(n[r])) {
                if (!et(n[r], t[r])) return !1;
                u[r] = !0
            }
            for (r in t) if (!u.hasOwnProperty(r) && r.charAt(0) !== "$" && t[r] !== i && !l(t[r])) return !1;
            return !0
        }
        return !1
    }

    function rr(n, t, i) {
        return n.concat(gu.call(t, i))
    }

    function tf(n, t) {
        return gu.call(n, t || 0)
    }

    function ge(n, t) {
        var i = arguments.length > 2 ? tf(arguments, 2) : [];
        return !l(t) || t instanceof RegExp ? t : i.length ? function () {
            return arguments.length ? t.apply(n, rr(i, arguments, 0)) : t.apply(n, i)
        } : function () {
            return arguments.length ? t.apply(n, arguments) : t.call(n)
        }
    }

    function no(n, r) {
        var u = r;
        return typeof n == "string" && n.charAt(0) === "$" && n.charAt(1) === "$" ? u = i : gi(r) ? u = "$WINDOW" : r && t === r ? u = "$DOCUMENT" : nr(r) && (u = "$SCOPE"), u
    }

    function ur(n, t) {
        return typeof n == "undefined" ? i : (k(t) || (t = t ? 2 : null), JSON.stringify(n, no, t))
    }

    function to(n) {
        return c(n) ? JSON.parse(n) : n
    }

    function wt(n) {
        n = f(n).clone();
        try {
            n.empty()
        } catch (i) {
        }
        var t = f("<div>").append(n).html();
        try {
            return n[0].nodeType === iu ? y(t) : t.match(/^(<[^>]+>)/)[1].replace(/^<([\w\-]+)/, function (n, t) {
                return "<" + y(t)
            })
        } catch (i) {
            return y(t)
        }
    }

    function io(n) {
        try {
            return decodeURIComponent(n)
        } catch (t) {
        }
    }

    function ro(n) {
        var i = {}, f, t;
        return r((n || "").split("&"), function (n) {
            if (n && (f = n.replace(/\+/g, "%20").split("="), t = io(f[0]), u(t))) {
                var r = u(f[1]) ? io(f[1]) : !0;
                ye.call(i, t) ? o(i[t]) ? i[t].push(r) : i[t] = [i[t], r] : i[t] = r
            }
        }), i
    }

    function rf(n) {
        var t = [];
        return r(n, function (n, i) {
            o(n) ? r(n, function (n) {
                t.push(ii(i, !0) + (n === !0 ? "" : "=" + ii(n, !0)))
            }) : t.push(ii(i, !0) + (n === !0 ? "" : "=" + ii(n, !0)))
        }), t.length ? t.join("&") : ""
    }

    function gr(n) {
        return ii(n, !0).replace(/%26/gi, "&").replace(/%3D/gi, "=").replace(/%2B/gi, "+")
    }

    function ii(n, t) {
        return encodeURIComponent(n).replace(/%40/gi, "@").replace(/%3A/gi, ":").replace(/%24/g, "$").replace(/%2C/gi, ",").replace(/%3B/gi, ";").replace(/%20/g, t ? "%20" : "+")
    }

    function fl(n, t) {
        var i, r, u = fr.length;
        for (n = f(n), r = 0; r < u; ++r) if (i = fr[r] + t, c(i = n.attr(i))) return i;
        return null
    }

    function el(n, t) {
        var i, u, f = {};
        r(fr, function (t) {
            var r = t + "app";
            !i && n.hasAttribute && n.hasAttribute(r) && (i = n, u = n.getAttribute(r))
        });
        r(fr, function (t) {
            var f = t + "app", r;
            !i && (r = n.querySelector("[" + f.replace(":", "\\:") + "]")) && (i = r, u = r.getAttribute(f))
        });
        i && (f.strictDi = fl(i, "strict-di") !== null, t(i, u ? [u] : [], f))
    }

    function uo(i, u, e) {
        var o;
        h(e) || (e = {});
        o = {strictDi: !1};
        e = a(o, e);
        var s = function () {
            var r, n;
            if (i = f(i), i.injector()) {
                r = i[0] === t ? "document" : wt(i);
                throw hi("btstrpd", "App Already Bootstrapped with this Element '{0}'", r.replace(/</, "&lt;").replace(/>/, "&gt;"));
            }
            return u = u || [], u.unshift(["$provide", function (n) {
                n.value("$rootElement", i)
            }]), e.debugInfoEnabled && u.push(["$compileProvider", function (n) {
                n.debugInfoEnabled(!0)
            }]), u.unshift("ng"), n = wf(u, e.strictDi), n.invoke(["$rootScope", "$rootElement", "$compile", "$injector", function (n, t, i, r) {
                n.$apply(function () {
                    t.data("$injector", r);
                    i(t)(n)
                })
            }]), n
        }, c = /^NG_ENABLE_DEBUG_INFO!/, l = /^NG_DEFER_BOOTSTRAP!/;
        if (n && c.test(n.name) && (e.debugInfoEnabled = !0, n.name = n.name.replace(c, "")), n && !l.test(n.name)) return s();
        n.name = n.name.replace(l, "");
        ft.resumeBootstrap = function (n) {
            r(n, function (n) {
                u.push(n)
            });
            s()
        }
    }

    function ol() {
        n.name = "NG_ENABLE_DEBUG_INFO!" + n.name;
        n.location.reload()
    }

    function sl(n) {
        var t = ft.element(n).injector();
        if (!t) throw hi("test", "no injector found for element argument to getTestability");
        return t.get("$$testability")
    }

    function eo(n, t) {
        return t = t || "_", n.replace(fo, function (n, i) {
            return (i ? t : "") + n.toLowerCase()
        })
    }

    function hl() {
        var t;
        uf || (ut = n.jQuery, ut && ut.fn.on ? (f = ut, a(ut.fn, {
            scope: ri.scope,
            isolateScope: ri.isolateScope,
            controller: ri.controller,
            injector: ri.injector,
            inheritedData: ri.inheritedData
        }), t = ut.cleanData, ut.cleanData = function (n) {
            var i, r, u;
            if (ff) ff = !1; else for (r = 0; (u = n[r]) != null; r++) i = ut._data(u, "events"), i && i.$destroy && ut(u).triggerHandler("$destroy");
            t(n)
        }) : f = w, ft.element = f, uf = !0)
    }

    function ef(n, t, i) {
        if (!n) throw hi("areq", "Argument '{0}' is {1}", t || "?", i || "required");
        return n
    }

    function nu(n, t, i) {
        return i && o(n) && (n = n[n.length - 1]), ef(l(n), t, "not a function, got " + (n && typeof n == "object" ? n.constructor.name || "Object" : typeof n)), n
    }

    function li(n, t) {
        if (n === "hasOwnProperty") throw hi("badname", "hasOwnProperty is not a valid {0} name", t);
    }

    function oo(n, t, i) {
        var r;
        if (!t) return n;
        var u = t.split("."), f, e = n, o = u.length;
        for (r = 0; r < o; r++) f = u[r], n && (n = (e = n)[f]);
        return !i && l(n) ? ge(e, n) : n
    }

    function tu(n) {
        var t = n[0], r = n[n.length - 1], i = [t];
        do {
            if (t = t.nextSibling, !t) break;
            i.push(t)
        } while (t !== r);
        return f(i)
    }

    function ot() {
        return Object.create(null)
    }

    function cl(n) {
        function t(n, t, i) {
            return n[t] || (n[t] = i())
        }

        var r = v("$injector"), u = v("ng"), i = t(n, "angular", Object);
        return i.$$minErr = i.$$minErr || v, t(i, "module", function () {
            var n = {};
            return function (i, f, e) {
                var o = function (n, t) {
                    if (n === "hasOwnProperty") throw u("badname", "hasOwnProperty is not a valid {0} name", t);
                };
                return o(i, "module"), f && n.hasOwnProperty(i) && (n[i] = null), t(n, i, function () {
                    function n(n, i, r, u) {
                        return u || (u = t), function () {
                            return u[r || "push"]([n, i, arguments]), h
                        }
                    }

                    if (!f) throw r("nomod", "Module '{0}' is not available! You either misspelled the module name or forgot to load it. If registering a module ensure that you specify the dependencies as the second argument.", i);
                    var t = [], u = [], o = [], s = n("$injector", "invoke", "push", u), h = {
                        _invokeQueue: t,
                        _configBlocks: u,
                        _runBlocks: o,
                        requires: f,
                        name: i,
                        provider: n("$provide", "provider"),
                        factory: n("$provide", "factory"),
                        service: n("$provide", "service"),
                        value: n("$provide", "value"),
                        constant: n("$provide", "constant", "unshift"),
                        animation: n("$animateProvider", "register"),
                        filter: n("$filterProvider", "register"),
                        controller: n("$controllerProvider", "register"),
                        directive: n("$compileProvider", "directive"),
                        config: s,
                        run: function (n) {
                            return o.push(n), this
                        }
                    };
                    return e && s(e), h
                })
            }
        })
    }

    function ll(n) {
        var t = [];
        return JSON.stringify(n, function (n, i) {
            if (i = no(n, i), h(i)) {
                if (t.indexOf(i) >= 0) return "<<already seen>>";
                t.push(i)
            }
            return i
        })
    }

    function co(n) {
        return typeof n == "function" ? n.toString().replace(/ \{[\s\S]*$/, "") : typeof n == "undefined" ? "undefined" : typeof n != "string" ? ll(n) : n
    }

    function al(t) {
        a(t, {
            bootstrap: uo,
            copy: ti,
            extend: a,
            equals: et,
            element: f,
            forEach: r,
            injector: wf,
            noop: s,
            bind: ge,
            toJson: ur,
            fromJson: to,
            identity: ct,
            isUndefined: e,
            isDefined: u,
            isString: c,
            isFunction: l,
            isObject: h,
            isNumber: k,
            isElement: de,
            isArray: o,
            version: lo,
            isDate: lt,
            lowercase: y,
            uppercase: bi,
            callbacks: {counter: 0},
            getTestability: sl,
            $$minErr: v,
            $$csp: ci,
            reloadWithDebugInfo: ol
        });
        ki = cl(n);
        try {
            ki("ngLocale")
        } catch (i) {
            ki("ngLocale", []).provider("$locale", sv)
        }
        ki("ng", ["ngLocale"], ["$provide", function (n) {
            n.provider({$$sanitizeUri: fy});
            n.provider("$compile", rs).directive({
                a: ah,
                input: tc,
                textarea: tc,
                form: up,
                script: eb,
                select: hb,
                style: lb,
                option: cb,
                ngBind: dp,
                ngBindHtml: nw,
                ngBindTemplate: gp,
                ngClass: iw,
                ngClassEven: uw,
                ngClassOdd: rw,
                ngCloak: fw,
                ngController: ew,
                ngForm: fp,
                ngHide: nb,
                ngIf: sw,
                ngInclude: hw,
                ngInit: lw,
                ngNonBindable: bw,
                ngPluralize: kw,
                ngRepeat: dw,
                ngShow: gw,
                ngStyle: tb,
                ngSwitch: ib,
                ngSwitchWhen: rb,
                ngSwitchDefault: ub,
                ngOptions: sb,
                ngTransclude: fb,
                ngModel: yw,
                ngList: aw,
                ngChange: tw,
                pattern: lc,
                ngPattern: lc,
                required: cc,
                ngRequired: cc,
                minlength: vc,
                ngMinlength: vc,
                maxlength: ac,
                ngMaxlength: ac,
                ngValue: kp,
                ngModelOptions: ww
            }).directive({ngInclude: cw}).directive(ar).directive(ic);
            n.provider({
                $anchorScroll: ca,
                $animate: is,
                $browser: va,
                $cacheFactory: ya,
                $controller: wa,
                $document: ba,
                $exceptionHandler: ka,
                $filter: ih,
                $interpolate: ev,
                $interval: ov,
                $http: iv,
                $httpBackend: uv,
                $location: av,
                $log: vv,
                $parse: ny,
                $rootScope: uy,
                $q: ty,
                $$q: iy,
                $sce: sy,
                $sceDelegate: oy,
                $sniffer: hy,
                $templateCache: pa,
                $templateRequest: cy,
                $$testability: ly,
                $timeout: ay,
                $window: vy,
                $$rAF: ry,
                $$asyncCallback: la,
                $$jqLite: ea
            })
        }])
    }

    function yl() {
        return ++vl
    }

    function or(n) {
        return n.replace(pl, function (n, t, i, r) {
            return r ? i.toUpperCase() : i
        }).replace(wl, "Moz$1")
    }

    function hf(n) {
        return !dl.test(n)
    }

    function ao(n) {
        var t = n.nodeType;
        return t === vt || !t || t === ho
    }

    function vo(n, t) {
        var i, o, f, u = t.createDocumentFragment(), e = [], s;
        if (hf(n)) e.push(t.createTextNode(n)); else {
            for (i = i || u.appendChild(t.createElement("div")), o = (gl.exec(n) || ["", ""])[1].toLowerCase(), f = st[o] || st._default, i.innerHTML = f[1] + n.replace(na, "<$1><\/$2>") + f[2], s = f[0]; s--;) i = i.lastChild;
            e = rr(e, i.childNodes);
            i = u.firstChild;
            i.textContent = ""
        }
        return u.textContent = "", u.innerHTML = "", r(e, function (n) {
            u.appendChild(n)
        }), u
    }

    function ta(n, i) {
        i = i || t;
        var r;
        return (r = kl.exec(n)) ? [i.createElement(r[1])] : (r = vo(n, i)) ? r.childNodes : []
    }

    function w(n) {
        if (n instanceof w) return n;
        var t;
        if (c(n) && (n = p(n), t = !0), !(this instanceof w)) {
            if (t && n.charAt(0) != "<") throw sf("nosel", "Looking up elements via selectors is not supported by jqLite! See: http://docs.angularjs.org/api/angular.element");
            return new w(n)
        }
        t ? af(this, ta(n)) : af(this, n)
    }

    function cf(n) {
        return n.cloneNode(!0)
    }

    function fu(n, t) {
        var r, i, u;
        if (t || eu(n), n.querySelectorAll) for (r = n.querySelectorAll("*"), i = 0, u = r.length; i < u; i++) eu(r[i])
    }

    function yo(n, t, i, f) {
        if (u(f)) throw sf("offargs", "jqLite#off() does not support the `selector` argument");
        var e = ou(n), o = e && e.events, s = e && e.handle;
        if (s) if (t) r(t.split(" "), function (t) {
            if (u(i)) {
                var r = o[t];
                if (ir(r || [], i), r && r.length > 0) return
            }
            er(n, t, s);
            delete o[t]
        }); else for (t in o) t !== "$destroy" && er(n, t, s), delete o[t]
    }

    function eu(n, t) {
        var u = n.ng339, r = u && ru[u];
        if (r) {
            if (t) {
                delete r.data[t];
                return
            }
            r.handle && (r.events.$destroy && r.handle({}, "$destroy"), yo(n));
            delete ru[u];
            n.ng339 = i
        }
    }

    function ou(n, t) {
        var r = n.ng339, u = r && ru[r];
        return t && !u && (n.ng339 = r = yl(), u = ru[r] = {events: {}, data: {}, handle: i}), u
    }

    function lf(n, t, i) {
        if (ao(n)) {
            var f = u(i), e = !f && t && !h(t), s = !t, o = ou(n, !e), r = o && o.data;
            if (f) r[t] = i; else {
                if (s) return r;
                if (e) return r && r[t];
                a(r, t)
            }
        }
    }

    function su(n, t) {
        return n.getAttribute ? (" " + (n.getAttribute("class") || "") + " ").replace(/[\n\t]/g, " ").indexOf(" " + t + " ") > -1 : !1
    }

    function hu(n, t) {
        t && n.setAttribute && r(t.split(" "), function (t) {
            n.setAttribute("class", p((" " + (n.getAttribute("class") || "") + " ").replace(/[\n\t]/g, " ").replace(" " + p(t) + " ", " ")))
        })
    }

    function cu(n, t) {
        if (t && n.setAttribute) {
            var i = (" " + (n.getAttribute("class") || "") + " ").replace(/[\n\t]/g, " ");
            r(t.split(" "), function (n) {
                n = p(n);
                i.indexOf(" " + n + " ") === -1 && (i += n + " ")
            });
            n.setAttribute("class", p(i))
        }
    }

    function af(n, t) {
        var i, r;
        if (t) if (t.nodeType) n[n.length++] = t; else if (i = t.length, typeof i == "number" && t.window !== t) {
            if (i) for (r = 0; r < i; r++) n[n.length++] = t[r]
        } else n[n.length++] = t
    }

    function po(n, t) {
        return lu(n, "$" + (t || "ngController") + "Controller")
    }

    function lu(n, t, r) {
        var e, u, s;
        for (n.nodeType == ho && (n = n.documentElement), e = o(t) ? t : [t]; n;) {
            for (u = 0, s = e.length; u < s; u++) if ((r = f.data(n, e[u])) !== i) return r;
            n = n.parentNode || n.nodeType === of && n.host
        }
    }

    function wo(n) {
        for (fu(n, !0); n.firstChild;) n.removeChild(n.firstChild)
    }

    function bo(n, t) {
        t || fu(n);
        var i = n.parentNode;
        i && i.removeChild(n)
    }

    function ia(t, i) {
        if (i = i || n, i.document.readyState === "complete") i.setTimeout(t); else f(i).on("load", t)
    }

    function ko(n, t) {
        var i = sr[t.toLowerCase()];
        return i && vf[pt(n)] && i
    }

    function ra(n, t) {
        var i = n.nodeName;
        return (i === "INPUT" || i === "TEXTAREA") && yf[t]
    }

    function ua(n, t) {
        var i = function (i, r) {
            var u, f, s, o;
            if (i.isDefaultPrevented = function () {
                return i.defaultPrevented
            }, u = t[r || i.type], f = u ? u.length : 0, f) for (e(i.immediatePropagationStopped) && (s = i.stopImmediatePropagation, i.stopImmediatePropagation = function () {
                i.immediatePropagationStopped = !0;
                i.stopPropagation && i.stopPropagation();
                s && s.call(i)
            }), i.isImmediatePropagationStopped = function () {
                return i.immediatePropagationStopped === !0
            }, f > 1 && (u = at(u)), o = 0; o < f; o++) i.isImmediatePropagationStopped() || u[o].call(n, i)
        };
        return i.elem = n, i
    }

    function ea() {
        this.$get = function () {
            return a(w, {
                hasClass: function (n, t) {
                    return n.attr && (n = n[0]), su(n, t)
                }, addClass: function (n, t) {
                    return n.attr && (n = n[0]), cu(n, t)
                }, removeClass: function (n, t) {
                    return n.attr && (n = n[0]), hu(n, t)
                }
            })
        }
    }

    function ai(n, t) {
        var r = n && n.$$hashKey, i;
        return r ? (typeof r == "function" && (r = n.$$hashKey()), r) : (i = typeof n, i == "function" || i == "object" && n !== null ? n.$$hashKey = i + ":" + (t || br)() : i + ":" + n)
    }

    function hr(n, t) {
        if (t) {
            var i = 0;
            this.nextUid = function () {
                return ++i
            }
        }
        r(n, this.put, this)
    }

    function ha(n) {
        var i = n.toString().replace(ns, ""), t = i.match(go);
        return t ? "function(" + (t[1] || "").replace(/[\s\r\n]+/, " ") + ")" : "fn"
    }

    function pf(n, t, i) {
        var u, e, s, f;
        if (typeof n == "function") {
            if (!(u = n.$inject)) {
                if (u = [], n.length) {
                    if (t) {
                        c(i) && i || (i = n.name || ha(n));
                        throw ui("strictdi", "{0} is not using explicit annotation and cannot be invoked in strict mode", i);
                    }
                    e = n.toString().replace(ns, "");
                    s = e.match(go);
                    r(s[1].split(oa), function (n) {
                        n.replace(sa, function (n, t, i) {
                            u.push(i)
                        })
                    })
                }
                n.$inject = u
            }
        } else o(n) ? (f = n.length - 1, nu(n[f], "fn"), u = n.slice(0, f)) : nu(n, "fn", !0);
        return u
    }

    function wf(n, t) {
        function y(n) {
            return function (t, i) {
                if (h(t)) r(t, we(n)); else return n(t, i)
            }
        }

        function g(n, t) {
            if (li(n, "service"), (l(t) || o(t)) && (t = u.instantiate(t)), !t.$get) throw ui("pget", "Provider '{0}' must define $get factory method.", n);
            return v[n + p] = t
        }

        function rt(n, t) {
            return function () {
                var i = f.invoke(t, this);
                if (e(i)) throw ui("undef", "Provider '{0}' must return a value from $get factory method.", n);
                return i
            }
        }

        function k(n, t, i) {
            return g(n, {$get: i !== !1 ? rt(n, t) : t})
        }

        function ut(n, t) {
            return k(n, ["$injector", function (n) {
                return n.instantiate(t)
            }])
        }

        function et(n, t) {
            return k(n, nt(t), !1)
        }

        function ot(n, t) {
            li(n, "constant");
            v[n] = t;
            b[n] = t
        }

        function st(n, t) {
            var i = u.get(n + p), r = i.$get;
            i.$get = function () {
                var n = f.invoke(r, i);
                return f.invoke(t, null, {$delegate: n})
            }
        }

        function tt(n) {
            var t = [], i;
            return r(n, function (n) {
                function f(n) {
                    for (var i, r, t = 0, f = n.length; t < f; t++) i = n[t], r = u.get(i[0]), r[i[1]].apply(r, i[2])
                }

                if (!d.get(n)) {
                    d.put(n, !0);
                    try {
                        c(n) ? (i = ki(n), t = t.concat(tt(i.requires)).concat(i._runBlocks), f(i._invokeQueue), f(i._configBlocks)) : l(n) ? t.push(u.invoke(n)) : o(n) ? t.push(u.invoke(n)) : nu(n, "module")
                    } catch (r) {
                        o(n) && (n = n[n.length - 1]);
                        r.message && r.stack && r.stack.indexOf(r.message) == -1 && (r = r.message + "\n" + r.stack);
                        throw ui("modulerr", "Failed to instantiate module {0} due to:\n{1}", n, r.stack || r.message || r);
                    }
                }
            }), t
        }

        function it(n, i) {
            function r(t, r) {
                if (n.hasOwnProperty(t)) {
                    if (n[t] === w) throw ui("cdep", "Circular dependency found: {0}", t + " <- " + a.join(" <- "));
                    return n[t]
                }
                try {
                    return a.unshift(t), n[t] = w, n[t] = i(t, r)
                } catch (u) {
                    n[t] === w && delete n[t];
                    throw u;
                } finally {
                    a.shift()
                }
            }

            function u(n, i, u, f) {
                typeof u == "string" && (f = u, u = null);
                for (var c = [], l = pf(n, t, f), e, s = 0, h = l.length; s < h; s++) {
                    if (e = l[s], typeof e != "string") throw ui("itkn", "Incorrect injection token! Expected service name as string, got {0}", e);
                    c.push(u && u.hasOwnProperty(e) ? u[e] : r(e, f))
                }
                return o(n) && (n = n[h]), n.apply(i, c)
            }

            function f(n, t, i) {
                var f = Object.create((o(n) ? n[n.length - 1] : n).prototype || null), r = u(n, f, t, i);
                return h(r) || l(r) ? r : f
            }

            return {
                invoke: u, instantiate: f, get: r, annotate: pf, has: function (t) {
                    return v.hasOwnProperty(t + p) || n.hasOwnProperty(t)
                }
            }
        }

        t = t === !0;
        var w = {}, p = "Provider", a = [], d = new hr([], !0), v = {
            $provide: {
                provider: y(g),
                factory: y(k),
                service: y(ut),
                value: y(et),
                constant: y(ot),
                decorator: st
            }
        }, u = v.$injector = it(v, function (n, t) {
            ft.isString(t) && a.push(t);
            throw ui("unpr", "Unknown provider: {0}", a.join(" <- "));
        }), b = {}, f = b.$injector = it(b, function (n, t) {
            var r = u.get(n + p, t);
            return f.invoke(r.$get, r, i, n)
        });
        return r(tt(n), function (n) {
            f.invoke(n || s)
        }), f
    }

    function ca() {
        var n = !0;
        this.disableAutoScrolling = function () {
            n = !1
        };
        this.$get = ["$window", "$location", "$rootScope", function (t, i, r) {
            function o(n) {
                var t = null;
                return Array.prototype.some.call(n, function (n) {
                    if (pt(n) === "a") return t = n, !0
                }), t
            }

            function s() {
                var n = f.yOffset, i, r;
                return l(n) ? n = n() : de(n) ? (i = n[0], r = t.getComputedStyle(i), n = r.position !== "fixed" ? 0 : i.getBoundingClientRect().bottom) : k(n) || (n = 0), n
            }

            function u(n) {
                var i, r;
                n ? (n.scrollIntoView(), i = s(), i && (r = n.getBoundingClientRect().top, t.scrollBy(0, r - i))) : t.scrollTo(0, 0)
            }

            function f() {
                var n = i.hash(), t;
                n ? (t = e.getElementById(n)) ? u(t) : (t = o(e.getElementsByName(n))) ? u(t) : n === "top" && u(null) : u(null)
            }

            var e = t.document;
            return n && r.$watch(function () {
                return i.hash()
            }, function (n, t) {
                (n !== t || n !== "") && ia(function () {
                    r.$evalAsync(f)
                })
            }), f
        }]
    }

    function la() {
        this.$get = ["$$rAF", "$timeout", function (n, t) {
            return n.supported ? function (t) {
                return n(t)
            } : function (n) {
                return t(n, 0, !1)
            }
        }]
    }

    function aa(n, t, u, o) {
        function it(n) {
            try {
                n.apply(null, tf(arguments, 1))
            } finally {
                if (v--, v === 0) while (k.length) try {
                    k.pop()()
                } catch (t) {
                    u.error(t)
                }
            }
        }

        function wt(n) {
            var t = n.indexOf("#");
            return t === -1 ? "" : n.substr(t + 1)
        }

        function bt(n, t) {
            (function i() {
                r(d, function (n) {
                    n()
                });
                ht = t(i, n)
            })()
        }

        function lt() {
            ft();
            at()
        }

        function ft() {
            l = n.history.state;
            l = e(l) ? null : l;
            et(l, g) && (l = g);
            g = l
        }

        function at() {
            (p !== h.url() || y !== l) && (p = h.url(), y = l, r(rt, function (n) {
                n(h.url(), l)
            }))
        }

        function yt(n) {
            try {
                return decodeURIComponent(n)
            } catch (t) {
                return n
            }
        }

        var h = this, w = t[0], a = n.location, tt = n.history, st = n.setTimeout, pt = n.clearTimeout, b = {}, v, k, d,
            ht, rt, ut, g;
        h.isMock = !1;
        v = 0;
        k = [];
        h.$$completeOutstandingRequest = it;
        h.$$incOutstandingRequestCount = function () {
            v++
        };
        h.notifyWhenNoOutstandingRequests = function (n) {
            r(d, function (n) {
                n()
            });
            v === 0 ? n() : k.push(n)
        };
        d = [];
        h.addPollFn = function (n) {
            return e(ht) && bt(100, st), d.push(n), n
        };
        var l, y, p = a.href, kt = t.find("base"), ct = null;
        ft();
        y = l;
        h.url = function (t, i, r) {
            var f, u;
            return e(r) && (r = null), a !== n.location && (a = n.location), tt !== n.history && (tt = n.history), t ? (f = y === r, p === t && (!o.history || f)) ? h : (u = p && fi(p) === fi(t), p = t, y = r, !o.history || u && f ? (u || (ct = t), i ? a.replace(t) : u ? a.hash = wt(t) : a.href = t) : (tt[i ? "replaceState" : "pushState"](r, "", t), ft(), y = l), h) : ct || a.href.replace(/%27/g, "'")
        };
        h.state = function () {
            return l
        };
        rt = [];
        ut = !1;
        g = null;
        h.onUrlChange = function (t) {
            if (!ut) {
                if (o.history) f(n).on("popstate", lt);
                f(n).on("hashchange", lt);
                ut = !0
            }
            return rt.push(t), t
        };
        h.$$checkUrlChange = at;
        h.baseHref = function () {
            var n = kt.attr("href");
            return n ? n.replace(/^(https?\:)?\/\/[^\/]*/, "") : ""
        };
        var nt = {}, ot = "", vt = h.baseHref();
        h.cookies = function (n, t) {
            var o, s, r, f, e;
            if (n) t === i ? w.cookie = encodeURIComponent(n) + "=;path=" + vt + ";expires=Thu, 01 Jan 1970 00:00:00 GMT" : c(t) && (o = (w.cookie = encodeURIComponent(n) + "=" + encodeURIComponent(t) + ";path=" + vt).length + 1, o > 4096 && u.warn("Cookie '" + n + "' possibly not set or overflowed because it was too large (" + o + " > 4096 bytes)!")); else {
                if (w.cookie !== ot) for (ot = w.cookie, s = ot.split("; "), nt = {}, f = 0; f < s.length; f++) r = s[f], e = r.indexOf("="), e > 0 && (n = yt(r.substring(0, e)), nt[n] === i && (nt[n] = yt(r.substring(e + 1))));
                return nt
            }
        };
        h.defer = function (n, t) {
            var i;
            return v++, i = st(function () {
                delete b[i];
                it(n)
            }, t || 0), b[i] = !0, i
        };
        h.defer.cancel = function (n) {
            return b[n] ? (delete b[n], pt(n), it(s), !0) : !1
        }
    }

    function va() {
        this.$get = ["$window", "$log", "$sniffer", "$document", function (n, t, i, r) {
            return new aa(n, r, t, i)
        }]
    }

    function ya() {
        this.$get = function () {
            function t(t, i) {
                function y(n) {
                    n != f && (r ? r == n && (r = n.n) : r = n, c(n.n, n.p), c(n, f), f = n, f.n = null)
                }

                function c(n, t) {
                    n != t && (n && (n.p = t), t && (t.n = n))
                }

                if (t in n) throw v("$cacheFactory")("iid", "CacheId '{0}' is already taken!", t);
                var s = 0, l = a({}, i, {id: t}), o = {}, h = i && i.capacity || Number.MAX_VALUE, u = {}, f = null,
                    r = null;
                return n[t] = {
                    put: function (n, t) {
                        if (h < Number.MAX_VALUE) {
                            var i = u[n] || (u[n] = {key: n});
                            y(i)
                        }
                        if (!e(t)) return n in o || s++, o[n] = t, s > h && this.remove(r.key), t
                    }, get: function (n) {
                        if (h < Number.MAX_VALUE) {
                            var t = u[n];
                            if (!t) return;
                            y(t)
                        }
                        return o[n]
                    }, remove: function (n) {
                        if (h < Number.MAX_VALUE) {
                            var t = u[n];
                            if (!t) return;
                            t == f && (f = t.p);
                            t == r && (r = t.n);
                            c(t.n, t.p);
                            delete u[n]
                        }
                        delete o[n];
                        s--
                    }, removeAll: function () {
                        o = {};
                        s = 0;
                        u = {};
                        f = r = null
                    }, destroy: function () {
                        o = null;
                        l = null;
                        u = null;
                        delete n[t]
                    }, info: function () {
                        return a({}, l, {size: s})
                    }
                }
            }

            var n = {};
            return t.info = function () {
                var t = {};
                return r(n, function (n, i) {
                    t[i] = n.info()
                }), t
            }, t.get = function (t) {
                return n[t]
            }, t
        }
    }

    function pa() {
        this.$get = ["$cacheFactory", function (n) {
            return n("templates")
        }]
    }

    function rs(n, e) {
        function ft(n, t) {
            var u = /^\s*([@&]|=(\*?))(\??)\s*(\w*)\s*$/, i = {};
            return r(n, function (n, r) {
                var f = n.match(u);
                if (!f) throw tt("iscp", "Invalid isolate scope definition for directive '{0}'. Definition: {... {1}: '{2}' ...}", t, r, n);
                i[r] = {mode: f[1][0], collection: f[2] === "*", optional: f[3] === "?", attrName: f[4] || r}
            }), i
        }

        var w = {}, b = "Directive", k = /^\s*directive\:\s*([\w\-]+)\s+(.*)$/, d = /(([\w\-]+)(?:\:([^;]+))?;?)/,
            g = ul("ngSrc,ngSrcset,src,srcset"), it = /^(?:(\^\^?)?(\?)?(\^\^?)?)?/, rt = /^(on[a-z]+|formaction)$/, v;
        this.directive = function st(t, i) {
            return li(t, "directive"), c(t) ? (ef(i, "directiveFactory"), w.hasOwnProperty(t) || (w[t] = [], n.factory(t + b, ["$injector", "$exceptionHandler", function (n, i) {
                var u = [];
                return r(w[t], function (r, f) {
                    try {
                        var e = n.invoke(r);
                        l(e) ? e = {compile: nt(e)} : !e.compile && e.link && (e.compile = nt(e.link));
                        e.priority = e.priority || 0;
                        e.index = f;
                        e.name = e.name || t;
                        e.require = e.require || e.controller && e.name;
                        e.restrict = e.restrict || "EA";
                        h(e.scope) && (e.$$isolateBindings = ft(e.scope, e.name));
                        u.push(e)
                    } catch (o) {
                        i(o)
                    }
                }), u
            }])), w[t].push(i)) : r(t, we(st)), this
        };
        this.aHrefSanitizationWhitelist = function (n) {
            return u(n) ? (e.aHrefSanitizationWhitelist(n), this) : e.aHrefSanitizationWhitelist()
        };
        this.imgSrcSanitizationWhitelist = function (n) {
            return u(n) ? (e.imgSrcSanitizationWhitelist(n), this) : e.imgSrcSanitizationWhitelist()
        };
        v = !0;
        this.debugInfoEnabled = function (n) {
            return u(n) ? (v = n, this) : v
        };
        this.$get = ["$injector", "$interpolate", "$exceptionHandler", "$templateRequest", "$parse", "$controller", "$rootScope", "$document", "$sce", "$animate", "$$sanitizeUri", function (n, u, e, nt, ft, st, ht, lt, at, yt, kt) {
            function ni(n, t) {
                try {
                    n.addClass(t)
                } catch (i) {
                }
            }

            function dt(n, t, i, u, e) {
                var s, o;
                return n instanceof f || (n = f(n)), r(n, function (t, i) {
                    t.nodeType == iu && t.nodeValue.match(/\S+/) && (n[i] = f(t).wrap("<span><\/span>").parent()[0])
                }), s = ei(n, t, n, i, u, e), dt.$$addScopeClass(n), o = null, function (t, i, r) {
                    var u, c;
                    ef(t, "scope");
                    r = r || {};
                    var e = r.parentBoundTranscludeFn, h = r.transcludeControllers, l = r.futureParentElement;
                    if (e && e.$$boundTransclude && (e = e.$$boundTransclude), o || (o = gi(l)), u = o !== "html" ? f(si(o, f("<div>").append(n).html())) : i ? ri.clone.call(n) : n, h) for (c in h) u.data("$" + c + "Controller", h[c].instance);
                    return dt.$$addScopeInfo(u, t), i && i(u, t), s && s(t, u, u, e), u
                }
            }

            function gi(n) {
                var t = n && n[0];
                return t ? pt(t) !== "foreignobject" && t.toString().match(/SVG/) ? "svg" : "html" : "html"
            }

            function ei(n, t, r, u, e, o) {
                function b(n, r, u, e) {
                    var s, c, l, a, o, w, y, b, v, k;
                    if (p) for (k = r.length, v = new Array(k), o = 0; o < h.length; o += 3) y = h[o], v[y] = r[y]; else v = r;
                    for (o = 0, w = h.length; o < w;) l = v[h[o++]], s = h[o++], c = h[o++], s ? (s.scope ? (a = n.$new(), dt.$$addScopeInfo(f(l), a)) : a = n, b = s.transcludeOnThisElement ? ti(n, s.transclude, e, s.elementTranscludeOnThisElement) : !s.templateOnThisElement && e ? e : !e && t ? ti(n, t) : null, s(c, a, l, u, b)) : c && c(n, l.childNodes, i, e)
                }

                for (var h = [], l, a, s, v, y, w, p, c = 0; c < n.length; c++) l = new fi, a = oi(n[c], [], l, c === 0 ? u : i, e), s = a.length ? yi(a, n[c], l, t, r, null, [], [], o) : null, s && s.scope && dt.$$addScopeClass(l.$$element), y = s && s.terminal || !(v = n[c].childNodes) || !v.length ? null : ei(v, s ? (s.transcludeOnThisElement || !s.templateOnThisElement) && s.transclude : t), (s || y) && (h.push(c, s, y), w = !0, p = p || s), o = null;
                return w ? b : null
            }

            function ti(n, t, i) {
                return function (r, u, f, e, o) {
                    return r || (r = n.$new(!1, o), r.$$transcluded = !0), t(r, u, {
                        parentBoundTranscludeFn: i,
                        transcludeControllers: f,
                        futureParentElement: e
                    })
                }
            }

            function oi(n, t, i, r, u) {
                var it = n.nodeType, rt = i.$attr, o, s, g, nt, tt;
                switch (it) {
                    case vt:
                        ii(t, bt(pt(n)), "E", r, u);
                        for (var a, e, f, l, v, y, w = n.attributes, b = 0, ut = w && w.length; b < ut; b++) g = !1, nt = !1, a = w[b], e = a.name, v = p(a.value), l = bt(e), (y = di.test(l)) && (e = e.replace(bf, "").substr(8).replace(/_(.)/g, function (n, t) {
                            return t.toUpperCase()
                        })), tt = l.replace(/(Start|End)$/, ""), tr(tt) && l === tt + "Start" && (g = e, nt = e.substr(0, e.length - 5) + "end", e = e.substr(0, e.length - 6)), f = bt(e.toLowerCase()), rt[f] = e, (y || !i.hasOwnProperty(f)) && (i[f] = v, ko(n, f) && (i[f] = !0)), or(n, t, v, f, y), ii(t, f, "A", r, u, g, nt);
                        if (s = n.className, h(s) && (s = s.animVal), c(s) && s !== "") while (o = d.exec(s)) f = bt(o[2]), ii(t, f, "C", r, u) && (i[f] = p(o[3])), s = s.substr(o.index + o[0].length);
                        break;
                    case iu:
                        fr(t, n.nodeValue);
                        break;
                    case so:
                        try {
                            o = k.exec(n.nodeValue);
                            o && (f = bt(o[1]), ii(t, f, "M", r, u) && (i[f] = p(o[2])))
                        } catch (ft) {
                        }
                }
                return t.sort(ur), t
            }

            function ai(n, t, i) {
                var r = [], u = 0;
                if (t && n.hasAttribute && n.hasAttribute(t)) {
                    do {
                        if (!n) throw tt("uterdir", "Unterminated attribute, found '{0}' but no matching '{1}' found.", t, i);
                        n.nodeType == vt && (n.hasAttribute(t) && u++, n.hasAttribute(i) && u--);
                        r.push(n);
                        n = n.nextSibling
                    } while (u > 0)
                } else r.push(n);
                return f(r)
            }

            function vi(n, t, i) {
                return function (r, u, f, e, o) {
                    return u = ai(u[0], t, i), n(r, u, f, e, o)
                }
            }

            function yi(n, s, a, v, y, w, b, k, d) {
                function fr(n, t, i, r) {
                    n && (i && (n = vi(n, i, r)), n.require = g.require, n.directiveName = ot, (nt === g || g.$$isolateScope) && (n = bi(n, {isolateScope: !0})), b.push(n));
                    t && (i && (t = vi(t, i, r)), t.require = g.require, t.directiveName = ot, (nt === g || g.$$isolateScope) && (t = bi(t, {isolateScope: !0})), k.push(t))
                }

                function tr(n, t, i, u) {
                    var f, s = "data", h = !1, l = i, e;
                    if (c(t)) {
                        if (e = t.match(it), t = t.substring(e[0].length), e[3] && (e[1] ? e[3] = null : e[1] = e[3]), e[1] === "^" ? s = "inheritedData" : e[1] === "^^" && (s = "inheritedData", l = i.parent()), e[2] === "?" && (h = !0), f = null, u && s === "data" && (f = u[t]) && (f = f.instance), f = f || l[s]("$" + t + "Controller"), !f && !h) throw tt("ctreq", "Controller '{0}', required by directive '{1}', can't be found!", t, n);
                        return f || null
                    }
                    return o(t) && (f = [], r(t, function (t) {
                        f.push(tr(n, t, i, u))
                    })), f
                }

                function at(n, t, e, o, h) {
                    function ht(n, t, r) {
                        var u;
                        return nr(n) || (r = t, t = n, n = i), bt && (u = d), r || (r = bt ? l.parent() : l), h(n, t, u, r, ut)
                    }

                    var w, ot, v, it, p, d, g, l, c, rt, y, ut;
                    for (s === e ? (c = a, l = a.$$element) : (l = f(e), c = new fi(l, a)), nt && (p = t.$new(!0)), h && (g = ht, g.$$boundTransclude = h), yt && (pt = {}, d = {}, r(yt, function (n) {
                        var r = {$scope: n === nt || n.$$isolateScope ? p : t, $element: l, $attrs: c, $transclude: g},
                            i;
                        it = n.controller;
                        it == "@" && (it = c[n.name]);
                        i = st(it, r, !0, n.controllerAs);
                        d[n.name] = i;
                        bt || l.data("$" + n.name + "Controller", i.instance);
                        pt[n.name] = i
                    })), nt && (dt.$$addScopeInfo(l, p, !0, !(ct && (ct === nt || ct === nt.$$originalDirective))), dt.$$addScopeClass(l, !0), rt = pt && pt[nt.name], y = p, rt && rt.identifier && nt.bindToController === !0 && (y = rt.instance), r(p.$$isolateBindings = nt.$$isolateBindings, function (n, i) {
                        var r = n.attrName, a = n.optional, v = n.mode, e, f, h, s, o, l;
                        switch (v) {
                            case"@":
                                c.$observe(r, function (n) {
                                    y[i] = n
                                });
                                c.$$observers[r].$$scope = t;
                                c[r] && (y[i] = u(c[r])(t));
                                break;
                            case"=":
                                if (a && !c[r]) return;
                                f = ft(c[r]);
                                s = f.literal ? et : function (n, t) {
                                    return n === t || n !== n && t !== t
                                };
                                h = f.assign || function () {
                                    e = y[i] = f(t);
                                    throw tt("nonassign", "Expression '{0}' used with directive '{1}' is non-assignable!", c[r], nt.name);
                                };
                                e = y[i] = f(t);
                                o = function (n) {
                                    return s(n, y[i]) || (s(n, e) ? h(t, n = y[i]) : y[i] = n), e = n
                                };
                                o.$stateful = !0;
                                l = n.collection ? t.$watchCollection(c[r], o) : t.$watch(ft(c[r], o), null, f.literal);
                                p.$on("$destroy", l);
                                break;
                            case"&":
                                f = ft(c[r]);
                                y[i] = function (n) {
                                    return f(t, n)
                                }
                        }
                    })), pt && (r(pt, function (n) {
                        n()
                    }), pt = null), w = 0, ot = b.length; w < ot; w++) v = b[w], ki(v, v.isolateScope ? p : t, l, c, v.require && tr(v.directiveName, v.require, l, d), g);
                    for (ut = t, nt && (nt.template || nt.templateUrl === null) && (ut = p), n && n(ut, e.childNodes, i, h), w = k.length - 1; w >= 0; w--) v = k[w], ki(v, v.isolateScope ? p : t, l, c, v.require && tr(v.directiveName, v.require, l, d), g)
                }

                var lt, ci, ri, yi;
                d = d || {};
                var ni = -Number.MAX_VALUE, ti, yt = d.controllerDirectives, pt, nt = d.newIsolateScopeDirective,
                    ct = d.templateDirective, ei = d.nonTlbTranscludeDirective, di = !1, gi = !1,
                    bt = d.hasElementTranscludeDirective, rt = a.$$element = f(s), g, ot, ht, hi = w, ii = v, kt, ut;
                for (lt = 0, ci = n.length; lt < ci; lt++) {
                    if (g = n[lt], ri = g.$$start, yi = g.$$end, ri && (rt = ai(s, ri, yi)), ht = i, ni > g.priority) break;
                    if ((ut = g.scope) && (g.templateUrl || (h(ut) ? (gt("new/isolated scope", nt || ti, g, rt), nt = g) : gt("new/isolated scope", nt, g, rt)), ti = ti || g), ot = g.name, !g.templateUrl && g.controller && (ut = g.controller, yt = yt || {}, gt("'" + ot + "' controller", yt[ot], g, rt), yt[ot] = g), (ut = g.transclude) && (di = !0, g.$$tlb || (gt("transclusion", ei, g, rt), ei = g), ut == "element" ? (bt = !0, ni = g.priority, ht = rt, rt = a.$$element = f(t.createComment(" " + ot + ": " + a[ot] + " ")), s = rt[0], ui(y, tf(ht), s), ii = dt(ht, v, ni, hi && hi.name, {nonTlbTranscludeDirective: ei})) : (ht = f(cf(s)).contents(), rt.empty(), ii = dt(ht, v))), g.template) if (gi = !0, gt("template", ct, g, rt), ct = g, ut = l(g.template) ? g.template(rt, a) : g.template, ut = li(ut), g.replace) {
                        if (hi = g, ht = hf(ut) ? [] : fs(si(g.templateNamespace, p(ut))), s = ht[0], ht.length != 1 || s.nodeType !== vt) throw tt("tplrt", "Template for directive '{0}' must have exactly one root element. {1}", ot, "");
                        ui(y, rt, s);
                        var ir = {$attr: {}}, ur = oi(s, [], ir), er = n.splice(lt + 1, n.length - (lt + 1));
                        nt && pi(ur);
                        n = n.concat(ur).concat(er);
                        wi(a, ir);
                        ci = n.length
                    } else rt.html(ut);
                    if (g.templateUrl) gi = !0, gt("template", ct, g, rt), ct = g, g.replace && (hi = g), at = rr(n.splice(lt, n.length - lt), rt, a, y, di && ii, b, k, {
                        controllerDirectives: yt,
                        newIsolateScopeDirective: nt,
                        templateDirective: ct,
                        nonTlbTranscludeDirective: ei
                    }), ci = n.length; else if (g.compile) try {
                        kt = g.compile(rt, a, ii);
                        l(kt) ? fr(null, kt, ri, yi) : kt && fr(kt.pre, kt.post, ri, yi)
                    } catch (or) {
                        e(or, wt(rt))
                    }
                    g.terminal && (at.terminal = !0, ni = Math.max(ni, g.priority))
                }
                return at.scope = ti && ti.scope === !0, at.transcludeOnThisElement = di, at.elementTranscludeOnThisElement = bt, at.templateOnThisElement = gi, at.transclude = ii, d.hasElementTranscludeDirective = bt, at
            }

            function pi(n) {
                for (var t = 0, i = n.length; t < i; t++) n[t] = ke(n[t], {$$isolateScope: !0})
            }

            function ii(t, r, u, f, o, s, h) {
                var l;
                if (r === o) return null;
                if (l = null, w.hasOwnProperty(r)) for (var c, v = n.get(r + b), a = 0, y = v.length; a < y; a++) try {
                    c = v[a];
                    (f === i || f > c.priority) && c.restrict.indexOf(u) != -1 && (s && (c = ke(c, {
                        $$start: s,
                        $$end: h
                    })), t.push(c), l = c)
                } catch (p) {
                    e(p)
                }
                return l
            }

            function tr(t) {
                if (w.hasOwnProperty(t)) for (var r, u = n.get(t + b), i = 0, f = u.length; i < f; i++) if (r = u[i], r.multiElement) return !0;
                return !1
            }

            function wi(n, t) {
                var u = t.$attr, f = n.$attr, i = n.$$element;
                r(n, function (i, r) {
                    r.charAt(0) != "$" && (t[r] && t[r] !== i && (i += (r === "style" ? ";" : " ") + t[r]), n.$set(r, i, !0, u[r]))
                });
                r(t, function (t, r) {
                    r == "class" ? (ni(i, t), n["class"] = (n["class"] ? n["class"] + " " : "") + t) : r == "style" ? (i.attr("style", i.attr("style") + ";" + t), n.style = (n.style ? n.style + ";" : "") + t) : r.charAt(0) == "$" || n.hasOwnProperty(r) || (n[r] = t, f[r] = u[r])
                })
            }

            function rr(n, t, i, u, e, o, s, c) {
                var y = [], w, b, k = t[0], v = n.shift(),
                    g = a({}, v, {templateUrl: null, transclude: null, replace: null, $$originalDirective: v}),
                    d = l(v.templateUrl) ? v.templateUrl(t, i) : v.templateUrl, it = v.templateNamespace;
                return t.empty(), nt(at.getTrustedResourceUrl(d)).then(function (l) {
                    var a, rt, ut, st, ft, ct;
                    if (l = li(l), v.replace) {
                        if (ut = hf(l) ? [] : fs(si(it, p(l))), a = ut[0], ut.length != 1 || a.nodeType !== vt) throw tt("tplrt", "Template for directive '{0}' must have exactly one root element. {1}", v.name, d);
                        rt = {$attr: {}};
                        ui(u, t, a);
                        ft = oi(a, [], rt);
                        h(v.scope) && pi(ft);
                        n = ft.concat(n);
                        wi(i, rt)
                    } else a = k, t.html(l);
                    for (n.unshift(g), w = yi(n, a, i, e, t, v, o, s, c), r(u, function (n, i) {
                        n == a && (u[i] = t[0])
                    }), b = ei(t[0].childNodes, e); y.length;) {
                        var et = y.shift(), ot = y.shift(), lt = y.shift(), ht = y.shift(), nt = t[0];
                        et.$$destroyed || (ot !== k && (ct = ot.className, c.hasElementTranscludeDirective && v.replace || (nt = cf(a)), ui(lt, f(ot), nt), ni(f(nt), ct)), st = w.transcludeOnThisElement ? ti(et, w.transclude, ht) : ht, w(b, et, nt, u, st))
                    }
                    y = null
                }), function (n, t, i, r, u) {
                    var f = u;
                    t.$$destroyed || (y ? y.push(t, i, r, f) : (w.transcludeOnThisElement && (f = ti(t, w.transclude, u)), w(b, t, i, r, f)))
                }
            }

            function ur(n, t) {
                var i = t.priority - n.priority;
                return i !== 0 ? i : n.name !== t.name ? n.name < t.name ? -1 : 1 : n.index - t.index
            }

            function gt(n, t, i, r) {
                if (t) throw tt("multidir", "Multiple directives [{0}, {1}] asking for {2} on: {3}", t.name, i.name, n, wt(r));
            }

            function fr(n, t) {
                var i = u(t, !0);
                i && n.push({
                    priority: 0, compile: function (n) {
                        var t = n.parent(), r = !!t.length;
                        return r && dt.$$addBindingClass(t), function (n, t) {
                            var u = t.parent();
                            r || dt.$$addBindingClass(u);
                            dt.$$addBindingInfo(u, i.expressions);
                            n.$watch(i, function (n) {
                                t[0].nodeValue = n
                            })
                        }
                    }
                })
            }

            function si(n, i) {
                n = y(n || "html");
                switch (n) {
                    case"svg":
                    case"math":
                        var r = t.createElement("div");
                        return r.innerHTML = "<" + n + ">" + i + "<\/" + n + ">", r.childNodes[0].childNodes;
                    default:
                        return i
                }
            }

            function er(n, t) {
                if (t == "srcdoc") return at.HTML;
                var i = pt(n);
                if (t == "xlinkHref" || i == "form" && t == "action" || i != "img" && (t == "src" || t == "ngSrc")) return at.RESOURCE_URL
            }

            function or(n, t, i, r, f) {
                var o = er(n, r), e;
                if (f = g[r] || f, e = u(i, !0, o, f), e) {
                    if (r === "multiple" && pt(n) === "select") throw tt("selmulti", "Binding to the 'multiple' attribute is not supported. Element: {0}", wt(n));
                    t.push({
                        priority: 100, compile: function () {
                            return {
                                pre: function (n, t, s) {
                                    var c = s.$$observers || (s.$$observers = {}), h;
                                    if (rt.test(r)) throw tt("nodomevents", "Interpolations for HTML DOM event attributes are disallowed.  Please use the ng- versions (such as ng-click instead of onclick) instead.");
                                    (h = s[r], h !== i && (e = h && u(h, !0, o, f), i = h), e) && (s[r] = e(n), (c[r] || (c[r] = [])).$$inter = !0, (s.$$observers && s.$$observers[r].$$scope || n).$watch(e, function (n, t) {
                                        r === "class" && n != t ? s.$updateClass(n, t) : s.$set(r, n)
                                    }))
                                }
                            }
                        }
                    })
                }
            }

            function ui(n, i, r) {
                var u = i[0], a = i.length, v = u.parentNode, e, y, c, s, w, l;
                if (n) for (e = 0, y = n.length; e < y; e++) if (n[e] == u) {
                    n[e++] = r;
                    for (var o = e, h = o + a - 1, p = n.length; o < p; o++, h++) h < p ? n[o] = n[h] : delete n[o];
                    n.length -= a - 1;
                    n.context === u && (n.context = r);
                    break
                }
                for (v && v.replaceChild(r, u), c = t.createDocumentFragment(), c.appendChild(u), f(r).data(f(u).data()), ut ? (ff = !0, ut.cleanData([u])) : delete f.cache[u[f.expando]], s = 1, w = i.length; s < w; s++) l = i[s], f(l).remove(), c.appendChild(l), delete i[s];
                i[0] = r;
                i.length = 1
            }

            function bi(n, t) {
                return a(function () {
                    return n.apply(null, arguments)
                }, n, t)
            }

            function ki(n, t, i, r, u, f) {
                try {
                    n(t, i, r, u, f)
                } catch (o) {
                    e(o, wt(i))
                }
            }

            var fi = function (n, t) {
                if (t) for (var u = Object.keys(t), r, i = 0, f = u.length; i < f; i++) r = u[i], this[r] = t[r]; else this.$attr = {};
                this.$$element = n
            };
            fi.prototype = {
                $normalize: bt, $addClass: function (n) {
                    n && n.length > 0 && yt.addClass(this.$$element, n)
                }, $removeClass: function (n) {
                    n && n.length > 0 && yt.removeClass(this.$$element, n)
                }, $updateClass: function (n, t) {
                    var r = us(n, t), i;
                    r && r.length && yt.addClass(this.$$element, r);
                    i = us(t, n);
                    i && i.length && yt.removeClass(this.$$element, i)
                }, $set: function (n, t, u, f) {
                    var w = this.$$element[0], b = ko(w, n), a = ra(w, n), k = n, h, s, v, l, y;
                    if (b ? (this.$$element.prop(n, t), f = b) : a && (this[a] = t, k = a), this[n] = t, f ? this.$attr[n] = f : (f = this.$attr[n], f || (this.$attr[n] = f = eo(n, "-"))), h = pt(this.$$element), h === "a" && n === "href" || h === "img" && n === "src") this[n] = t = kt(t, n === "src"); else if (h === "img" && n === "srcset") {
                        var o = "", d = p(t), g = /\s/.test(d) ? /(\s+\d+x\s*,|\s+\d+w\s*,|\s+,|,\s+)/ : /(,)/,
                            c = d.split(g), nt = Math.floor(c.length / 2);
                        for (s = 0; s < nt; s++) v = s * 2, o += kt(p(c[v]), !0), o += " " + p(c[v + 1]);
                        l = p(c[s * 2]).split(/\s/);
                        o += kt(p(l[0]), !0);
                        l.length === 2 && (o += " " + p(l[1]));
                        this[n] = t = o
                    }
                    u !== !1 && (t === null || t === i ? this.$$element.removeAttr(f) : this.$$element.attr(f, t));
                    y = this.$$observers;
                    y && r(y[k], function (n) {
                        try {
                            n(t)
                        } catch (i) {
                            e(i)
                        }
                    })
                }, $observe: function (n, t) {
                    var i = this, u = i.$$observers || (i.$$observers = ot()), r = u[n] || (u[n] = []);
                    return r.push(t), ht.$evalAsync(function () {
                        !r.$$inter && i.hasOwnProperty(n) && t(i[n])
                    }), function () {
                        ir(r, t)
                    }
                }
            };
            var hi = u.startSymbol(), ci = u.endSymbol(), li = hi == "{{" || ci == "}}" ? ct : function (n) {
                return n.replace(/\{\{/g, hi).replace(/}}/g, ci)
            }, di = /^ngAttr[A-Z]/;
            return dt.$$addBindingInfo = v ? function (n, t) {
                var i = n.data("$binding") || [];
                o(t) ? i = i.concat(t) : i.push(t);
                n.data("$binding", i)
            } : s, dt.$$addBindingClass = v ? function (n) {
                ni(n, "ng-binding")
            } : s, dt.$$addScopeInfo = v ? function (n, t, i, r) {
                var u = i ? r ? "$isolateScopeNoTemplate" : "$isolateScope" : "$scope";
                n.data(u, t)
            } : s, dt.$$addScopeClass = v ? function (n, t) {
                ni(n, t ? "ng-isolate-scope" : "ng-scope")
            } : s, dt
        }]
    }

    function bt(n) {
        return or(n.replace(bf, ""))
    }

    function us(n, t) {
        var u = "", e = n.split(/\s+/), o = t.split(/\s+/), i, f, r;
        n:for (i = 0; i < e.length; i++) {
            for (f = e[i], r = 0; r < o.length; r++) if (f == o[r]) continue n;
            u += (u.length > 0 ? " " : "") + f
        }
        return u
    }

    function fs(n) {
        var t, i;
        if (n = f(n), t = n.length, t <= 1) return n;
        while (t--) i = n[t], i.nodeType === so && kc.call(n, t, 1);
        return n
    }

    function wa() {
        var n = {}, t = !1, r = /^(\S+)(\s+as\s+(\w+))?$/;
        this.register = function (t, i) {
            li(t, "controller");
            h(t) ? a(n, t) : n[t] = i
        };
        this.allowGlobals = function () {
            t = !0
        };
        this.$get = ["$injector", "$window", function (u, f) {
            function e(n, t, i, r) {
                if (!(n && h(n.$scope))) throw v("$controller")("noscp", "Cannot export controller '{0}' as '{1}'! No $scope object provided via `locals`.", r, t);
                n.$scope[t] = i
            }

            return function (s, h, l, v) {
                var p, b, y, w, k;
                return (l = l === !0, v && c(v) && (w = v), c(s) && (b = s.match(r), y = b[1], w = w || b[3], s = n.hasOwnProperty(y) ? n[y] : oo(h.$scope, y, !0) || (t ? oo(f, y, !0) : i), nu(s, y, !0)), l) ? (k = (o(s) ? s[s.length - 1] : s).prototype, p = Object.create(k || null), w && e(h, w, p, y || s.name), a(function () {
                    return u.invoke(s, p, h, y), p
                }, {instance: p, identifier: w})) : (p = u.instantiate(s, h, y), w && e(h, w, p, y || s.name), p)
            }
        }]
    }

    function ba() {
        this.$get = ["$window", function (n) {
            return f(n.document)
        }]
    }

    function ka() {
        this.$get = ["$log", function (n) {
            return function () {
                n.error.apply(n, arguments)
            }
        }]
    }

    function df(n, t) {
        var i, r;
        return c(n) && (i = n.replace(nv, "").trim(), i && (r = t("Content-Type"), (r && r.indexOf(es) === 0 || tv(i)) && (n = to(i)))), n
    }

    function tv(n) {
        var t = n.match(da);
        return t && ga[t[0]].test(n)
    }

    function os(n) {
        var t = ot(), i, u, f;
        return n ? (r(n.split("\n"), function (n) {
            f = n.indexOf(":");
            i = y(p(n.substr(0, f)));
            u = p(n.substr(f + 1));
            i && (t[i] = t[i] ? t[i] + ", " + u : u)
        }), t) : t
    }

    function ss(n) {
        var t = h(n) ? n : i;
        return function (i) {
            if (t || (t = os(n)), i) {
                var r = t[y(i)];
                return r === void 0 && (r = null), r
            }
            return t
        }
    }

    function hs(n, t, i, u) {
        return l(u) ? u(n, t, i) : (r(u, function (r) {
            n = r(n, t, i)
        }), n)
    }

    function gf(n) {
        return 200 <= n && n < 300
    }

    function iv() {
        var n = this.defaults = {
            transformResponse: [df],
            transformRequest: [function (n) {
                return h(n) && !tl(n) && !rl(n) && !il(n) ? ur(n) : n
            }],
            headers: {common: {Accept: "application/json, text/plain, */*"}, post: at(kf), put: at(kf), patch: at(kf)},
            xsrfCookieName: "XSRF-TOKEN",
            xsrfHeaderName: "X-XSRF-TOKEN"
        }, t = !1, f;
        this.useApplyAsync = function (n) {
            return u(n) ? (t = !!n, this) : t
        };
        f = this.interceptors = [];
        this.$get = ["$httpBackend", "$browser", "$cacheFactory", "$rootScope", "$q", "$injector", function (s, p, w, b, k, d) {
            function g(t) {
                function c(n) {
                    var t = a({}, n);
                    return t.data = n.data ? hs(n.data, n.headers, n.status, f.transformResponse) : n.data, gf(n.status) ? t : k.reject(t)
                }

                function w(n) {
                    var t, i = {};
                    return r(n, function (n, r) {
                        l(n) ? (t = n(), t != null && (i[r] = t)) : i[r] = n
                    }), i
                }

                function b(t) {
                    var i = n.headers, u = a({}, t.headers), r, f, e;
                    i = a({}, i.common, i[y(t.method)]);
                    n:for (r in i) {
                        f = y(r);
                        for (e in u) if (y(e) === f) continue n;
                        u[r] = i[r]
                    }
                    return w(u)
                }

                var f, s, h;
                if (!ft.isObject(t)) throw v("$http")("badreq", "Http request configuration must be an object.  Received: {0}", t);
                f = a({method: "get", transformRequest: n.transformRequest, transformResponse: n.transformResponse}, t);
                f.headers = b(t);
                f.method = bi(f.method);
                var p = function (t) {
                    var u = t.headers, f = hs(t.data, ss(u), i, t.transformRequest);
                    return e(f) && r(u, function (n, t) {
                        y(t) === "content-type" && delete u[t]
                    }), e(t.withCredentials) && !e(n.withCredentials) && (t.withCredentials = n.withCredentials), ut(t, f).then(c, c)
                }, o = [p, i], u = k.when(f);
                for (r(nt, function (n) {
                    (n.request || n.requestError) && o.unshift(n.request, n.requestError);
                    (n.response || n.responseError) && o.push(n.response, n.responseError)
                }); o.length;) s = o.shift(), h = o.shift(), u = u.then(s, h);
                return u.success = function (n) {
                    return u.then(function (t) {
                        n(t.data, t.status, t.headers, f)
                    }), u
                }, u.error = function (n) {
                    return u.then(null, function (t) {
                        n(t.data, t.status, t.headers, f)
                    }), u
                }, u
            }

            function it() {
                r(arguments, function (n) {
                    g[n] = function (t, i) {
                        return g(a(i || {}, {method: n, url: t}))
                    }
                })
            }

            function rt() {
                r(arguments, function (n) {
                    g[n] = function (t, i, r) {
                        return g(a(r || {}, {method: n, url: t, data: i}))
                    }
                })
            }

            function ut(r, f) {
                function ut(n, i, r, u) {
                    function f() {
                        v(i, n, r, u)
                    }

                    l && (gf(n) ? l.put(a, [n, i, os(r), u]) : l.remove(a));
                    t ? b.$applyAsync(f) : (f(), b.$$phase || b.$apply())
                }

                function v(n, t, i, u) {
                    t = Math.max(t, 0);
                    (gf(t) ? y.resolve : y.reject)({data: n, status: t, headers: ss(i), config: r, statusText: u})
                }

                function it(n) {
                    v(n.data, n.status, at(n.headers()), n.statusText)
                }

                function rt() {
                    var n = g.pendingRequests.indexOf(r);
                    n !== -1 && g.pendingRequests.splice(n, 1)
                }

                var y = k.defer(), w = y.promise, l, c, nt = r.headers, a = et(r.url, r.params), d;
                return g.pendingRequests.push(r), w.then(rt, rt), (r.cache || n.cache) && r.cache !== !1 && (r.method === "GET" || r.method === "JSONP") && (l = h(r.cache) ? r.cache : h(n.cache) ? n.cache : tt), l && (c = l.get(a), u(c) ? dr(c) ? c.then(it, it) : o(c) ? v(c[1], c[0], at(c[2]), c[3]) : v(c, 200, {}, "OK") : l.put(a, w)), e(c) && (d = th(r.url) ? p.cookies()[r.xsrfCookieName || n.xsrfCookieName] : i, d && (nt[r.xsrfHeaderName || n.xsrfHeaderName] = d), s(r.method, a, f, ut, nt, r.timeout, r.withCredentials, r.responseType)), w
            }

            function et(n, t) {
                if (!t) return n;
                var i = [];
                return nl(t, function (n, t) {
                    n === null || e(n) || (o(n) || (n = [n]), r(n, function (n) {
                        h(n) && (n = lt(n) ? n.toISOString() : ur(n));
                        i.push(ii(t) + "=" + ii(n))
                    }))
                }), i.length > 0 && (n += (n.indexOf("?") == -1 ? "?" : "&") + i.join("&")), n
            }

            var tt = w("$http"), nt = [];
            return r(f, function (n) {
                nt.unshift(c(n) ? d.get(n) : d.invoke(n))
            }), g.pendingRequests = [], it("get", "delete", "head", "jsonp"), rt("post", "put", "patch"), g.defaults = n, g
        }]
    }

    function rv() {
        return new n.XMLHttpRequest
    }

    function uv() {
        this.$get = ["$browser", "$window", "$document", function (n, t, i) {
            return fv(n, rv, n.defer, t.angular.callbacks, i[0])
        }]
    }

    function fv(n, t, f, e, o) {
        function h(n, t, i) {
            var r = o.createElement("script"), u = null;
            return r.type = "text/javascript", r.src = n, r.async = !0, u = function (n) {
                er(r, "load", u);
                er(r, "error", u);
                o.body.removeChild(r);
                r = null;
                var f = -1, s = "unknown";
                n && (n.type !== "load" || e[t].called || (n = {type: "error"}), s = n.type, f = n.type === "error" ? 404 : 200);
                i && i(f, s)
            }, uu(r, "load", u), uu(r, "error", u), o.body.appendChild(r), u
        }

        return function (o, c, l, a, v, p, w, b) {
            function rt() {
                g && g();
                k && k.abort()
            }

            function it(t, r, u, e, o) {
                tt !== i && f.cancel(tt);
                g = k = null;
                t(r, u, e, o);
                n.$$completeOutstandingRequest(s)
            }

            var d, g, k, nt, tt;
            if (n.$$incOutstandingRequestCount(), c = c || n.url(), y(o) == "jsonp") d = "_" + (e.counter++).toString(36), e[d] = function (n) {
                e[d].data = n;
                e[d].called = !0
            }, g = h(c.replace("JSON_CALLBACK", "angular.callbacks." + d), d, function (n, t) {
                it(a, n, e[d].data, "", t);
                e[d] = s
            }); else {
                if (k = t(), k.open(o, c, !0), r(v, function (n, t) {
                    u(n) && k.setRequestHeader(t, n)
                }), k.onload = function () {
                    var i = k.statusText || "", t = "response" in k ? k.response : k.responseText,
                        n = k.status === 1223 ? 204 : k.status;
                    n === 0 && (n = t ? 200 : gt(c).protocol == "file" ? 404 : 0);
                    it(a, n, t, k.getAllResponseHeaders(), i)
                }, nt = function () {
                    it(a, -1, null, null, "")
                }, k.onerror = nt, k.onabort = nt, w && (k.withCredentials = !0), b) try {
                    k.responseType = b
                } catch (ut) {
                    if (b !== "json") throw ut;
                }
                k.send(l || null)
            }
            p > 0 ? tt = f(rt, p) : dr(p) && p.then(rt)
        }
    }

    function ev() {
        var n = "{{", t = "}}";
        this.startSymbol = function (t) {
            return t ? (n = t, this) : n
        };
        this.endSymbol = function (n) {
            return n ? (t = n, this) : t
        };
        this.$get = ["$parse", "$exceptionHandler", "$sce", function (i, r, f) {
            function h(n) {
                return "\\\\\\" + n
            }

            function o(o, h, p, w) {
                function et(i) {
                    return i.replace(v, n).replace(y, t)
                }

                function ht(n) {
                    try {
                        return n = ot(n), w && !u(n) ? n : st(n)
                    } catch (t) {
                        var i = au("interr", "Can't interpolate: {0}\n{1}", o, t.toString());
                        r(i)
                    }
                }

                w = !!w;
                for (var d, nt, b = 0, g = [], tt = [], rt = o.length, it, k = [], ut = []; b < rt;) if ((d = o.indexOf(n, b)) != -1 && (nt = o.indexOf(t, d + s)) != -1) b !== d && k.push(et(o.substring(b, d))), it = o.substring(d + s, nt), g.push(it), tt.push(i(it, ht)), b = nt + c, ut.push(k.length), k.push(""); else {
                    b !== rt && k.push(et(o.substring(b)));
                    break
                }
                if (p && k.length > 1) throw au("noconcat", "Error while interpolating: {0}\nStrict Contextual Escaping disallows interpolations that concatenate multiple expressions when a trusted value is required.  See http://docs.angularjs.org/api/ng.$sce", o);
                if (!h || g.length) {
                    var ft = function (n) {
                        for (var t = 0, i = g.length; t < i; t++) {
                            if (w && e(n[t])) return;
                            k[ut[t]] = n[t]
                        }
                        return k.join("")
                    }, ot = function (n) {
                        return p ? f.getTrusted(p, n) : f.valueOf(n)
                    }, st = function (n) {
                        if (n == null) return "";
                        switch (typeof n) {
                            case"string":
                                break;
                            case"number":
                                n = "" + n;
                                break;
                            default:
                                n = ur(n)
                        }
                        return n
                    };
                    return a(function (n) {
                        var t = 0, i = g.length, u = new Array(i), f;
                        try {
                            for (; t < i; t++) u[t] = tt[t](n);
                            return ft(u)
                        } catch (e) {
                            f = au("interr", "Can't interpolate: {0}\n{1}", o, e.toString());
                            r(f)
                        }
                    }, {
                        exp: o, expressions: g, $$watchDelegate: function (n, t, i) {
                            var r;
                            return n.$watchGroup(tt, function (i, u) {
                                var f = ft(i);
                                l(t) && t.call(this, f, i !== u ? r : f, n);
                                r = f
                            }, i)
                        }
                    })
                }
            }

            var s = n.length, c = t.length, v = new RegExp(n.replace(/./g, h), "g"),
                y = new RegExp(t.replace(/./g, h), "g");
            return o.startSymbol = function () {
                return n
            }, o.endSymbol = function () {
                return t
            }, o
        }]
    }

    function ov() {
        this.$get = ["$rootScope", "$window", "$q", "$$q", function (n, t, i, r) {
            function e(e, o, s, h) {
                var y = t.setInterval, p = t.clearInterval, a = 0, v = u(h) && !h, l = (v ? r : i).defer(),
                    c = l.promise;
                return s = u(s) ? s : 0, c.then(null, null, e), c.$$intervalId = y(function () {
                    l.notify(a++);
                    s > 0 && a >= s && (l.resolve(a), p(c.$$intervalId), delete f[c.$$intervalId]);
                    v || n.$apply()
                }, o), f[c.$$intervalId] = l, c
            }

            var f = {};
            return e.cancel = function (n) {
                return n && n.$$intervalId in f ? (f[n.$$intervalId].reject("canceled"), t.clearInterval(n.$$intervalId), delete f[n.$$intervalId], !0) : !1
            }, e
        }]
    }

    function sv() {
        this.$get = function () {
            return {
                id: "en-us",
                NUMBER_FORMATS: {
                    DECIMAL_SEP: ".",
                    GROUP_SEP: ",",
                    PATTERNS: [{
                        minInt: 1,
                        minFrac: 0,
                        maxFrac: 3,
                        posPre: "",
                        posSuf: "",
                        negPre: "-",
                        negSuf: "",
                        gSize: 3,
                        lgSize: 3
                    }, {
                        minInt: 1,
                        minFrac: 2,
                        maxFrac: 2,
                        posPre: "¤",
                        posSuf: "",
                        negPre: "(¤",
                        negSuf: ")",
                        gSize: 3,
                        lgSize: 3
                    }],
                    CURRENCY_SYM: "$"
                },
                DATETIME_FORMATS: {
                    MONTH: "January,February,March,April,May,June,July,August,September,October,November,December".split(","),
                    SHORTMONTH: "Jan,Feb,Mar,Apr,May,Jun,Jul,Aug,Sep,Oct,Nov,Dec".split(","),
                    DAY: "Sunday,Monday,Tuesday,Wednesday,Thursday,Friday,Saturday".split(","),
                    SHORTDAY: "Sun,Mon,Tue,Wed,Thu,Fri,Sat".split(","),
                    AMPMS: ["AM", "PM"],
                    medium: "MMM d, y h:mm:ss a",
                    short: "M/d/yy h:mm a",
                    fullDate: "EEEE, MMMM d, y",
                    longDate: "MMMM d, y",
                    mediumDate: "MMM d, y",
                    shortDate: "M/d/yy",
                    mediumTime: "h:mm:ss a",
                    shortTime: "h:mm a"
                },
                pluralCat: function (n) {
                    return n === 1 ? "one" : "other"
                }
            }
        }
    }

    function ne(n) {
        for (var t = n.split("/"), i = t.length; i--;) t[i] = gr(t[i]);
        return t.join("/")
    }

    function cs(n, t) {
        var i = gt(n);
        t.$$protocol = i.protocol;
        t.$$host = i.hostname;
        t.$$port = g(i.port) || cv[i.protocol] || null
    }

    function ls(n, t) {
        var r = n.charAt(0) !== "/", i;
        r && (n = "/" + n);
        i = gt(n);
        t.$$path = decodeURIComponent(r && i.pathname.charAt(0) === "/" ? i.pathname.substring(1) : i.pathname);
        t.$$search = ro(i.search);
        t.$$hash = decodeURIComponent(i.hash);
        t.$$path && t.$$path.charAt(0) != "/" && (t.$$path = "/" + t.$$path)
    }

    function kt(n, t) {
        if (t.indexOf(n) === 0) return t.substr(n.length)
    }

    function fi(n) {
        var t = n.indexOf("#");
        return t == -1 ? n : n.substr(0, t)
    }

    function as(n) {
        return n.replace(/(#.+)|#$/, "$1")
    }

    function te(n) {
        return n.substr(0, fi(n).lastIndexOf("/") + 1)
    }

    function lv(n) {
        return n.substring(0, n.indexOf("/", n.indexOf("//") + 2))
    }

    function ie(n, t) {
        this.$$html5 = !0;
        t = t || "";
        var r = te(n);
        cs(n, this);
        this.$$parse = function (n) {
            var t = kt(r, n);
            if (!c(t)) throw vu("ipthprfx", 'Invalid url "{0}", missing path prefix "{1}".', n, r);
            ls(t, this);
            this.$$path || (this.$$path = "/");
            this.$$compose()
        };
        this.$$compose = function () {
            var n = rf(this.$$search), t = this.$$hash ? "#" + gr(this.$$hash) : "";
            this.$$url = ne(this.$$path) + (n ? "?" + n : "") + t;
            this.$$absUrl = r + this.$$url.substr(1)
        };
        this.$$parseLinkUrl = function (u, f) {
            if (f && f[0] === "#") return this.hash(f.slice(1)), !0;
            var e, s, o;
            return (e = kt(n, u)) !== i ? (s = e, o = (e = kt(t, e)) !== i ? r + (kt("/", e) || e) : n + s) : (e = kt(r, u)) !== i ? o = r + e : r == u + "/" && (o = r), o && this.$$parse(o), !!o
        }
    }

    function re(n, t) {
        var i = te(n);
        cs(n, this);
        this.$$parse = function (r) {
            function o(n, t, i) {
                var u = /^\/[A-Z]:(\/.*)/, r;
                return (t.indexOf(i) === 0 && (t = t.replace(i, "")), u.exec(t)) ? n : (r = u.exec(n), r ? r[1] : n)
            }

            var f = kt(n, r) || kt(i, r), u;
            f.charAt(0) === "#" ? (u = kt(t, f), e(u) && (u = f)) : u = this.$$html5 ? f : "";
            ls(u, this);
            this.$$path = o(this.$$path, u, n);
            this.$$compose()
        };
        this.$$compose = function () {
            var i = rf(this.$$search), r = this.$$hash ? "#" + gr(this.$$hash) : "";
            this.$$url = ne(this.$$path) + (i ? "?" + i : "") + r;
            this.$$absUrl = n + (this.$$url ? t + this.$$url : "")
        };
        this.$$parseLinkUrl = function (t) {
            return fi(n) == fi(t) ? (this.$$parse(t), !0) : !1
        }
    }

    function vs(n, t) {
        this.$$html5 = !0;
        re.apply(this, arguments);
        var i = te(n);
        this.$$parseLinkUrl = function (r, u) {
            if (u && u[0] === "#") return this.hash(u.slice(1)), !0;
            var f, e;
            return n == fi(r) ? f = r : (e = kt(i, r)) ? f = n + t + e : i === r + "/" && (f = i), f && this.$$parse(f), !!f
        };
        this.$$compose = function () {
            var i = rf(this.$$search), r = this.$$hash ? "#" + gr(this.$$hash) : "";
            this.$$url = ne(this.$$path) + (i ? "?" + i : "") + r;
            this.$$absUrl = n + t + this.$$url
        }
    }

    function yu(n) {
        return function () {
            return this[n]
        }
    }

    function ps(n, t) {
        return function (i) {
            return e(i) ? this[n] : (this[n] = t(i), this.$$compose(), this)
        }
    }

    function av() {
        var t = "", n = {enabled: !1, requireBase: !0, rewriteLinks: !0};
        this.hashPrefix = function (n) {
            return u(n) ? (t = n, this) : t
        };
        this.html5Mode = function (t) {
            return tr(t) ? (n.enabled = t, this) : h(t) ? (tr(t.enabled) && (n.enabled = t.enabled), tr(t.requireBase) && (n.requireBase = t.requireBase), tr(t.rewriteLinks) && (n.rewriteLinks = t.rewriteLinks), this) : n
        };
        this.$get = ["$rootScope", "$browser", "$sniffer", "$rootElement", "$window", function (i, r, u, e, o) {
            function w(n, t, i) {
                var u = s.url(), f = s.$$state;
                try {
                    r.url(n, t, i);
                    s.$$state = r.state()
                } catch (e) {
                    s.url(u);
                    s.$$state = f;
                    throw e;
                }
            }

            function b(n, t) {
                i.$broadcast("$locationChangeSuccess", s.absUrl(), n, s.$$state, t)
            }

            var s, a, y = r.baseHref(), c = r.url(), v, p, l;
            if (n.enabled) {
                if (!y && n.requireBase) throw vu("nobase", "$location in HTML5 mode requires a <base> tag to be present!");
                v = lv(c) + (y || "/");
                a = u.history ? ie : vs
            } else v = fi(c), a = re;
            s = new a(v, "#" + t);
            s.$$parseLinkUrl(c, c);
            s.$$state = r.state();
            p = /^\s*(javascript|mailto):/i;
            e.on("click", function (t) {
                var u, c, l;
                if (n.rewriteLinks && !t.ctrlKey && !t.metaKey && !t.shiftKey && t.which != 2 && t.button != 2) {
                    for (u = f(t.target); pt(u[0]) !== "a";) if (u[0] === e[0] || !(u = u.parent())[0]) return;
                    (c = u.prop("href"), l = u.attr("href") || u.attr("xlink:href"), h(c) && c.toString() === "[object SVGAnimatedString]" && (c = gt(c.animVal).href), p.test(c)) || !c || u.attr("target") || t.isDefaultPrevented() || s.$$parseLinkUrl(c, l) && (t.preventDefault(), s.absUrl() != r.url() && (i.$apply(), o.angular["ff-684208-preventDefault"] = !0))
                }
            });
            s.absUrl() != c && r.url(s.absUrl(), !0);
            l = !0;
            r.onUrlChange(function (n, t) {
                i.$evalAsync(function () {
                    var r = s.absUrl(), u = s.$$state, f;
                    (s.$$parse(n), s.$$state = t, f = i.$broadcast("$locationChangeStart", n, r, t, u).defaultPrevented, s.absUrl() === n) && (f ? (s.$$parse(r), s.$$state = u, w(r, !1, u)) : (l = !1, b(r, u)))
                });
                i.$$phase || i.$digest()
            });
            return i.$watch(function () {
                var t = as(r.url()), e = as(s.absUrl()), n = r.state(), o = s.$$replace,
                    f = t !== e || s.$$html5 && u.history && n !== s.$$state;
                (l || f) && (l = !1, i.$evalAsync(function () {
                    var r = s.absUrl(), u = i.$broadcast("$locationChangeStart", r, t, s.$$state, n).defaultPrevented;
                    s.absUrl() === r && (u ? (s.$$parse(t), s.$$state = n) : (f && w(r, o, n === s.$$state ? null : s.$$state), b(t, n)))
                }));
                s.$$replace = !1
            }), s
        }]
    }

    function vv() {
        var n = !0, t = this;
        this.debugEnabled = function (t) {
            return u(t) ? (n = t, this) : n
        };
        this.$get = ["$window", function (i) {
            function f(n) {
                return n instanceof Error && (n.stack ? n = n.message && n.stack.indexOf(n.message) === -1 ? "Error: " + n.message + "\n" + n.stack : n.stack : n.sourceURL && (n = n.message + "\n" + n.sourceURL + ":" + n.line)), n
            }

            function u(n) {
                var t = i.console || {}, u = t[n] || t.log || s, e = !1;
                try {
                    e = !!u.apply
                } catch (o) {
                }
                return e ? function () {
                    var n = [];
                    return r(arguments, function (t) {
                        n.push(f(t))
                    }), u.apply(t, n)
                } : function (n, t) {
                    u(n, t == null ? "" : t)
                }
            }

            return {
                log: u("log"), info: u("info"), warn: u("warn"), error: u("error"), debug: function () {
                    var i = u("debug");
                    return function () {
                        n && i.apply(t, arguments)
                    }
                }()
            }
        }]
    }

    function yt(n, t) {
        if (n === "__defineGetter__" || n === "__defineSetter__" || n === "__lookupGetter__" || n === "__lookupSetter__" || n === "__proto__") throw it("isecfld", "Attempting to access a disallowed field in Angular expressions! Expression: {0}", t);
        return n
    }

    function ht(n, t) {
        if (n) if (n.constructor === n) throw it("isecfn", "Referencing Function in Angular expressions is disallowed! Expression: {0}", t); else if (n.window === n) throw it("isecwindow", "Referencing the Window in Angular expressions is disallowed! Expression: {0}", t); else if (n.children && (n.nodeName || n.prop && n.attr && n.find)) throw it("isecdom", "Referencing DOM nodes in Angular expressions is disallowed! Expression: {0}", t); else if (n === Object) throw it("isecobj", "Referencing Object in Angular expressions is disallowed! Expression: {0}", t);
        return n
    }

    function bv(n, t) {
        if (n) if (n.constructor === n) throw it("isecfn", "Referencing Function in Angular expressions is disallowed! Expression: {0}", t); else if (n === yv || n === pv || n === wv) throw it("isecff", "Referencing call, apply or bind in Angular expressions is disallowed! Expression: {0}", t);
    }

    function fe(n) {
        return n.constant
    }

    function lr(n, t, i, r, u) {
        var o, f, s, e;
        for (ht(n, u), ht(t, u), o = i.split("."), s = 0; o.length > 1; s++) f = yt(o.shift(), u), e = s === 0 && t && t[f] || n[f], e || (e = {}, n[f] = e), n = ht(e, u);
        return f = yt(o.shift(), u), ht(n[f], u), n[f] = r, r
    }

    function pi(n) {
        return n == "constructor"
    }

    function ks(n, t, r, u, f, e, o) {
        yt(n, e);
        yt(t, e);
        yt(r, e);
        yt(u, e);
        yt(f, e);
        var s = function (n) {
                return ht(n, e)
            }, h = o || pi(n) ? s : ct, c = o || pi(t) ? s : ct, l = o || pi(r) ? s : ct, a = o || pi(u) ? s : ct,
            v = o || pi(f) ? s : ct;
        return function (e, o) {
            var s = o && o.hasOwnProperty(n) ? o : e;
            return s == null ? s : (s = h(s[n]), !t) ? s : s == null ? i : (s = c(s[t]), !r) ? s : s == null ? i : (s = l(s[r]), !u) ? s : s == null ? i : (s = a(s[u]), !f) ? s : s == null ? i : v(s[f])
        }
    }

    function dv(n, t) {
        return function (i, r) {
            return n(i, r, ht, t)
        }
    }

    function gv(n, t, u) {
        var o = t.expensiveChecks, a = o ? bs : ws, e = a[n], f, c, s, l, h;
        return e ? e : (f = n.split("."), c = f.length, t.csp ? e = c < 6 ? ks(f[0], f[1], f[2], f[3], f[4], u, o) : function (n, t) {
            var r = 0, e;
            do e = ks(f[r++], f[r++], f[r++], f[r++], f[r++], u, o)(n, t), t = i, n = e; while (r < c);
            return e
        } : (s = "", o && (s += "s = eso(s, fe);\nl = eso(l, fe);\n"), l = o, r(f, function (n, t) {
            yt(n, u);
            var i = (t ? "s" : '((l&&l.hasOwnProperty("' + n + '"))?l:s)') + "." + n;
            (o || pi(n)) && (i = "eso(" + i + ", fe)", l = !0);
            s += "if(s == null) return undefined;\ns=" + i + ";\n"
        }), s += "return s;", h = new Function("s", "l", "eso", "fe", s), h.toString = nt(s), l && (h = dv(h, u)), e = h), e.sharedGetter = !0, e.assign = function (t, i, r) {
            return lr(t, r, n, i, n)
        }, a[n] = e, e)
    }

    function ee(n) {
        return l(n.valueOf) ? n.valueOf() : ds.call(n)
    }

    function ny() {
        var n = ot(), t = ot();
        this.$get = ["$filter", "$sniffer", function (i, f) {
            function w(n) {
                var t = n;
                return n.sharedGetter && (t = function (t, i) {
                    return n(t, i)
                }, t.literal = n.literal, t.constant = n.constant, t.assign = n.assign), t
            }

            function c(n, t) {
                for (var i, r = 0, u = n.length; r < u; r++) i = n[r], i.constant || (i.inputs ? c(i.inputs, t) : t.indexOf(i) === -1 && t.push(i));
                return t
            }

            function e(n, t) {
                return n == null || t == null ? n === t : typeof n == "object" && (n = ee(n), typeof n == "object") ? !1 : n === t || n !== n && t !== t
            }

            function o(n, t, i, r) {
                var u = r.$$inputs || (r.$$inputs = c(r.inputs, [])), f, h, o, s, l;
                if (u.length === 1) return h = e, u = u[0], n.$watch(function (n) {
                    var t = u(n);
                    return e(t, h) || (f = r(n), h = t && ee(t)), f
                }, t, i);
                for (o = [], s = 0, l = u.length; s < l; s++) o[s] = e;
                return n.$watch(function (n) {
                    for (var i, s = !1, t = 0, h = u.length; t < h; t++) i = u[t](n), (s || (s = !e(i, o[t]))) && (o[t] = i && ee(i));
                    return s && (f = r(n)), f
                }, t, i)
            }

            function a(n, t, i, r) {
                var f, e;
                return f = n.$watch(function (n) {
                    return r(n)
                }, function (n, i, r) {
                    e = n;
                    l(t) && t.apply(this, arguments);
                    u(n) && r.$$postDigest(function () {
                        u(e) && f()
                    })
                }, i)
            }

            function v(n, t, i, f) {
                function s(n) {
                    var t = !0;
                    return r(n, function (n) {
                        u(n) || (t = !1)
                    }), t
                }

                var e, o;
                return e = n.$watch(function (n) {
                    return f(n)
                }, function (n, i, r) {
                    o = n;
                    l(t) && t.call(this, n, i, r);
                    s(n) && r.$$postDigest(function () {
                        s(o) && e()
                    })
                }, i)
            }

            function b(n, t, i, r) {
                var u;
                return u = n.$watch(function (n) {
                    return r(n)
                }, function () {
                    l(t) && t.apply(this, arguments);
                    u()
                }, i)
            }

            function h(n, t) {
                if (!t) return n;
                var r = n.$$watchDelegate, f = r !== v && r !== a, i = f ? function (i, r) {
                    var u = n(i, r);
                    return t(u, i, r)
                } : function (i, r) {
                    var f = n(i, r), e = t(f, i, r);
                    return u(f) ? e : f
                };
                return n.$$watchDelegate && n.$$watchDelegate !== o ? i.$$watchDelegate = n.$$watchDelegate : t.$stateful || (i.$$watchDelegate = o, i.inputs = [n]), i
            }

            var y = {csp: f.csp, expensiveChecks: !1}, p = {csp: f.csp, expensiveChecks: !0};
            return function (r, u, f) {
                var e, k, c, l;
                switch (typeof r) {
                    case"string":
                        if (c = r = r.trim(), l = f ? t : n, e = l[c], !e) {
                            r.charAt(0) === ":" && r.charAt(1) === ":" && (k = !0, r = r.substring(2));
                            var d = f ? p : y, g = new ue(d), nt = new yi(g, i, d);
                            e = nt.parse(r);
                            e.constant ? e.$$watchDelegate = b : k ? (e = w(e), e.$$watchDelegate = e.literal ? v : a) : e.inputs && (e.$$watchDelegate = o);
                            l[c] = e
                        }
                        return h(e, u);
                    case"function":
                        return h(r, u);
                    default:
                        return h(s, u)
                }
            }
        }]
    }

    function ty() {
        this.$get = ["$rootScope", "$exceptionHandler", function (n, t) {
            return gs(function (t) {
                n.$evalAsync(t)
            }, t)
        }]
    }

    function iy() {
        this.$get = ["$browser", "$exceptionHandler", function (n, t) {
            return gs(function (t) {
                n.defer(t)
            }, t)
        }]
    }

    function gs(n, t) {
        function k(n, t, i) {
            function u(t) {
                return function (i) {
                    r || (r = !0, t.call(n, i))
                }
            }

            var r = !1;
            return [u(t), u(i)]
        }

        function y() {
            this.$$state = {status: 0}
        }

        function s(n, t) {
            return function (i) {
                t.call(n, i)
            }
        }

        function g(n) {
            var e, r, f, u, o;
            for (f = n.pending, n.processScheduled = !1, n.pending = i, u = 0, o = f.length; u < o; ++u) {
                r = f[u][0];
                e = f[u][n.status];
                try {
                    l(e) ? r.resolve(e(n.value)) : n.status === 1 ? r.resolve(n.value) : r.reject(n.value)
                } catch (s) {
                    r.reject(s);
                    t(s)
                }
            }
        }

        function c(t) {
            !t.processScheduled && t.pending && (t.processScheduled = !0, n(function () {
                g(t)
            }))
        }

        function u() {
            this.promise = new y;
            this.resolve = s(this, this.resolve);
            this.reject = s(this, this.reject);
            this.notify = s(this, this.notify)
        }

        function tt(n) {
            var i = new u, f = 0, t = o(n) ? [] : {};
            return r(n, function (n, r) {
                f++;
                w(n).then(function (n) {
                    t.hasOwnProperty(r) || (t[r] = n, --f || i.resolve(t))
                }, function (n) {
                    t.hasOwnProperty(r) || i.reject(n)
                })
            }), f === 0 && i.resolve(t), i.promise
        }

        var a = v("$q", TypeError), d = function () {
            return new u
        }, f;
        y.prototype = {
            then: function (n, t, i) {
                var r = new u;
                return this.$$state.pending = this.$$state.pending || [], this.$$state.pending.push([r, n, t, i]), this.$$state.status > 0 && c(this.$$state), r.promise
            }, "catch": function (n) {
                return this.then(null, n)
            }, "finally": function (n, t) {
                return this.then(function (t) {
                    return p(t, !0, n)
                }, function (t) {
                    return p(t, !1, n)
                }, t)
            }
        };
        u.prototype = {
            resolve: function (n) {
                this.promise.$$state.status || (n === this.promise ? this.$$reject(a("qcycle", "Expected promise to be resolved with value other than itself '{0}'", n)) : this.$$resolve(n))
            }, $$resolve: function (n) {
                var i, r = k(this, this.$$resolve, this.$$reject);
                try {
                    (h(n) || l(n)) && (i = n && n.then);
                    l(i) ? (this.promise.$$state.status = -1, i.call(n, r[0], r[1], this.notify)) : (this.promise.$$state.value = n, this.promise.$$state.status = 1, c(this.promise.$$state))
                } catch (u) {
                    r[1](u);
                    t(u)
                }
            }, reject: function (n) {
                this.promise.$$state.status || this.$$reject(n)
            }, $$reject: function (n) {
                this.promise.$$state.value = n;
                this.promise.$$state.status = 2;
                c(this.promise.$$state)
            }, notify: function (i) {
                var r = this.promise.$$state.pending;
                this.promise.$$state.status <= 0 && r && r.length && n(function () {
                    for (var u, f, n = 0, e = r.length; n < e; n++) {
                        f = r[n][0];
                        u = r[n][3];
                        try {
                            f.notify(l(u) ? u(i) : i)
                        } catch (o) {
                            t(o)
                        }
                    }
                })
            }
        };
        var nt = function (n) {
            var t = new u;
            return t.reject(n), t.promise
        }, e = function (n, t) {
            var i = new u;
            return t ? i.resolve(n) : i.reject(n), i.promise
        }, p = function (n, t, i) {
            var r = null;
            try {
                l(i) && (r = i())
            } catch (u) {
                return e(u, !1)
            }
            return dr(r) ? r.then(function () {
                return e(n, t)
            }, function (n) {
                return e(n, !1)
            }) : e(n, t)
        }, w = function (n, t, i, r) {
            var f = new u;
            return f.resolve(n), f.promise.then(t, i, r)
        };
        return f = function b(n) {
            function i(n) {
                t.resolve(n)
            }

            function r(n) {
                t.reject(n)
            }

            if (!l(n)) throw a("norslvr", "Expected resolverFn, got '{0}'", n);
            if (!(this instanceof b)) return new b(n);
            var t = new u;
            return n(i, r), t.promise
        }, f.defer = d, f.reject = nt, f.when = w, f.all = tt, f
    }

    function ry() {
        this.$get = ["$window", "$timeout", function (n, t) {
            var i = n.requestAnimationFrame || n.webkitRequestAnimationFrame,
                f = n.cancelAnimationFrame || n.webkitCancelAnimationFrame || n.webkitCancelRequestAnimationFrame,
                r = !!i, u = r ? function (n) {
                    var t = i(n);
                    return function () {
                        f(t)
                    }
                } : function (n) {
                    var i = t(n, 16.66, !1);
                    return function () {
                        t.cancel(i)
                    }
                };
            return u.supported = r, u
        }]
    }

    function uy() {
        var i = 10, u = v("$rootScope"), n = null, t = null;
        this.digestTtl = function (n) {
            return arguments.length && (i = n), i
        };
        this.$get = ["$injector", "$exceptionHandler", "$parse", "$browser", function (f, o, c, a) {
            function p() {
                this.$id = br();
                this.$$phase = this.$parent = this.$$watchers = this.$$nextSibling = this.$$prevSibling = this.$$childHead = this.$$childTail = null;
                this.$root = this;
                this.$$destroyed = !1;
                this.$$listeners = {};
                this.$$listenerCount = {};
                this.$$isolateBindings = null
            }

            function d(n) {
                if (v.$$phase) throw u("inprog", "{0} already in progress", v.$$phase);
                v.$$phase = n
            }

            function k() {
                v.$$phase = null
            }

            function g(n, t, i) {
                do n.$$listenerCount[i] -= t, n.$$listenerCount[i] === 0 && delete n.$$listenerCount[i]; while (n = n.$parent)
            }

            function nt() {
            }

            function tt() {
                while (b.length) try {
                    b.shift()()
                } catch (n) {
                    o(n)
                }
                t = null
            }

            function it() {
                t === null && (t = a.defer(function () {
                    v.$apply(tt)
                }))
            }

            p.prototype = {
                constructor: p, $new: function (n, t) {
                    function r() {
                        i.$$destroyed = !0
                    }

                    var i;
                    return t = t || this, n ? (i = new p, i.$root = this.$root) : (this.$$ChildScope || (this.$$ChildScope = function () {
                        this.$$watchers = this.$$nextSibling = this.$$childHead = this.$$childTail = null;
                        this.$$listeners = {};
                        this.$$listenerCount = {};
                        this.$id = br();
                        this.$$ChildScope = null
                    }, this.$$ChildScope.prototype = this), i = new this.$$ChildScope), i.$parent = t, i.$$prevSibling = t.$$childTail, t.$$childHead ? (t.$$childTail.$$nextSibling = i, t.$$childTail = i) : t.$$childHead = t.$$childTail = i, (n || t != this) && i.$on("$destroy", r), i
                }, $watch: function (t, i, r) {
                    var u = c(t);
                    if (u.$$watchDelegate) return u.$$watchDelegate(this, i, r, u);
                    var o = this, f = o.$$watchers, e = {fn: i, last: nt, get: u, exp: t, eq: !!r};
                    return n = null, l(i) || (e.fn = s), f || (f = o.$$watchers = []), f.unshift(e), function () {
                        ir(f, e);
                        n = null
                    }
                }, $watchGroup: function (n, t) {
                    function c() {
                        o = !1;
                        h ? (h = !1, t(i, i, u)) : t(i, f, u)
                    }

                    var f = new Array(n.length), i = new Array(n.length), e = [], u = this, o = !1, h = !0, s;
                    return n.length ? n.length === 1 ? this.$watch(n[0], function (n, r, u) {
                        i[0] = n;
                        f[0] = r;
                        t(i, n === r ? i : f, u)
                    }) : (r(n, function (n, t) {
                        var r = u.$watch(n, function (n, r) {
                            i[t] = n;
                            f[t] = r;
                            o || (o = !0, u.$evalAsync(c))
                        });
                        e.push(r)
                    }), function () {
                        while (e.length) e.shift()()
                    }) : (s = !0, u.$evalAsync(function () {
                        s && t(i, i, u)
                    }), function () {
                        s = !1
                    })
                }, $watchCollection: function (n, t) {
                    function y(n) {
                        var c, o, y, t, s, v;
                        if (i = n, !e(i)) {
                            if (h(i)) if (di(i)) for (r !== l && (r = l, f = r.length = 0, u++), c = i.length, f !== c && (u++, r.length = f = c), v = 0; v < c; v++) s = r[v], t = i[v], y = s !== s && t !== t, y || s === t || (u++, r[v] = t); else {
                                r !== a && (r = a = {}, f = 0, u++);
                                c = 0;
                                for (o in i) i.hasOwnProperty(o) && (c++, t = i[o], s = r[o], o in r ? (y = s !== s && t !== t, y || s === t || (u++, r[o] = t)) : (f++, r[o] = t, u++));
                                if (f > c) {
                                    u++;
                                    for (o in r) i.hasOwnProperty(o) || (f--, delete r[o])
                                }
                            } else r !== i && (r = i, u++);
                            return u
                        }
                    }

                    function b() {
                        var n, r;
                        if (v ? (v = !1, t(i, i, s)) : t(i, o, s), p) if (h(i)) if (di(i)) for (o = new Array(i.length), n = 0; n < i.length; n++) o[n] = i[n]; else {
                            o = {};
                            for (r in i) ye.call(i, r) && (o[r] = i[r])
                        } else o = i
                    }

                    y.$stateful = !0;
                    var s = this, i, r, o, p = t.length > 1, u = 0, w = c(n, y), l = [], a = {}, v = !0, f = 0;
                    return this.$watch(w, b)
                }, $digest: function () {
                    var r, e, s, g, it, h, rt = i, ut, f, ft = this, c = [], p, b;
                    d("$digest");
                    a.$$checkUrlChange();
                    this === v && t !== null && (a.defer.cancel(t), tt());
                    n = null;
                    do {
                        for (h = !1, f = ft; y.length;) {
                            try {
                                b = y.shift();
                                b.scope.$eval(b.expression, b.locals)
                            } catch (ot) {
                                o(ot)
                            }
                            n = null
                        }
                        n:do {
                            if (g = f.$$watchers) for (it = g.length; it--;) try {
                                if (r = g[it], r) if ((e = r.get(f)) === (s = r.last) || (r.eq ? et(e, s) : typeof e == "number" && typeof s == "number" && isNaN(e) && isNaN(s))) {
                                    if (r === n) {
                                        h = !1;
                                        break n
                                    }
                                } else h = !0, n = r, r.last = r.eq ? ti(e, null) : e, r.fn(e, s === nt ? e : s, f), rt < 5 && (p = 4 - rt, c[p] || (c[p] = []), c[p].push({
                                    msg: l(r.exp) ? "fn: " + (r.exp.name || r.exp.toString()) : r.exp,
                                    newVal: e,
                                    oldVal: s
                                }))
                            } catch (ot) {
                                o(ot)
                            }
                            if (!(ut = f.$$childHead || f !== ft && f.$$nextSibling)) while (f !== ft && !(ut = f.$$nextSibling)) f = f.$parent
                        } while (f = ut);
                        if ((h || y.length) && !rt--) {
                            k();
                            throw u("infdig", "{0} $digest() iterations reached. Aborting!\nWatchers fired in the last 5 iterations: {1}", i, c);
                        }
                    } while (h || y.length);
                    for (k(); w.length;) try {
                        w.shift()()
                    } catch (ot) {
                        o(ot)
                    }
                }, $destroy: function () {
                    var n, t;
                    if (!this.$$destroyed && (n = this.$parent, this.$broadcast("$destroy"), this.$$destroyed = !0, this !== v)) {
                        for (t in this.$$listenerCount) g(this, this.$$listenerCount[t], t);
                        n.$$childHead == this && (n.$$childHead = this.$$nextSibling);
                        n.$$childTail == this && (n.$$childTail = this.$$prevSibling);
                        this.$$prevSibling && (this.$$prevSibling.$$nextSibling = this.$$nextSibling);
                        this.$$nextSibling && (this.$$nextSibling.$$prevSibling = this.$$prevSibling);
                        this.$destroy = this.$digest = this.$apply = this.$evalAsync = this.$applyAsync = s;
                        this.$on = this.$watch = this.$watchGroup = function () {
                            return s
                        };
                        this.$$listeners = {};
                        this.$parent = this.$$nextSibling = this.$$prevSibling = this.$$childHead = this.$$childTail = this.$root = this.$$watchers = null
                    }
                }, $eval: function (n, t) {
                    return c(n)(this, t)
                }, $evalAsync: function (n, t) {
                    v.$$phase || y.length || a.defer(function () {
                        y.length && v.$digest()
                    });
                    y.push({scope: this, expression: n, locals: t})
                }, $$postDigest: function (n) {
                    w.push(n)
                }, $apply: function (n) {
                    try {
                        return d("$apply"), this.$eval(n)
                    } catch (t) {
                        o(t)
                    } finally {
                        k();
                        try {
                            v.$digest()
                        } catch (t) {
                            o(t);
                            throw t;
                        }
                    }
                }, $applyAsync: function (n) {
                    function i() {
                        t.$eval(n)
                    }

                    var t = this;
                    n && b.push(i);
                    it()
                }, $on: function (n, t) {
                    var r = this.$$listeners[n], i, u;
                    r || (this.$$listeners[n] = r = []);
                    r.push(t);
                    i = this;
                    do i.$$listenerCount[n] || (i.$$listenerCount[n] = 0), i.$$listenerCount[n]++; while (i = i.$parent);
                    return u = this, function () {
                        var i = r.indexOf(t);
                        i !== -1 && (r[i] = null, g(u, 1, n))
                    }
                }, $emit: function (n) {
                    var s = [], u, r = this, e = !1, t = {
                        name: n, targetScope: r, stopPropagation: function () {
                            e = !0
                        }, preventDefault: function () {
                            t.defaultPrevented = !0
                        }, defaultPrevented: !1
                    }, h = rr([t], arguments, 1), i, f;
                    do {
                        for (u = r.$$listeners[n] || s, t.currentScope = r, i = 0, f = u.length; i < f; i++) {
                            if (!u[i]) {
                                u.splice(i, 1);
                                i--;
                                f--;
                                continue
                            }
                            try {
                                u[i].apply(null, h)
                            } catch (c) {
                                o(c)
                            }
                        }
                        if (e) return t.currentScope = null, t;
                        r = r.$parent
                    } while (r);
                    return t.currentScope = null, t
                }, $broadcast: function (n) {
                    var r = this, t = r, e = r, u = {
                        name: n, targetScope: r, preventDefault: function () {
                            u.defaultPrevented = !0
                        }, defaultPrevented: !1
                    }, h, f, i, s;
                    if (!r.$$listenerCount[n]) return u;
                    for (h = rr([u], arguments, 1); t = e;) {
                        for (u.currentScope = t, f = t.$$listeners[n] || [], i = 0, s = f.length; i < s; i++) {
                            if (!f[i]) {
                                f.splice(i, 1);
                                i--;
                                s--;
                                continue
                            }
                            try {
                                f[i].apply(null, h)
                            } catch (c) {
                                o(c)
                            }
                        }
                        if (!(e = t.$$listenerCount[n] && t.$$childHead || t !== r && t.$$nextSibling)) while (t !== r && !(e = t.$$nextSibling)) t = t.$parent
                    }
                    return u.currentScope = null, u
                }
            };
            var v = new p, y = v.$$asyncQueue = [], w = v.$$postDigestQueue = [], b = v.$$applyAsyncQueue = [];
            return v
        }]
    }

    function fy() {
        var n = /^\s*(https?|ftp|mailto|tel|file):/, t = /^\s*((https?|ftp|file|blob):|data:image\/)/;
        this.aHrefSanitizationWhitelist = function (t) {
            return u(t) ? (n = t, this) : n
        };
        this.imgSrcSanitizationWhitelist = function (n) {
            return u(n) ? (t = n, this) : t
        };
        this.$get = function () {
            return function (i, r) {
                var f = r ? t : n, u;
                return (u = gt(i).href, u !== "" && !u.match(f)) ? "unsafe:" + u : i
            }
        }
    }

    function ey(n) {
        if (n === "self") return n;
        if (c(n)) {
            if (n.indexOf("***") > -1) throw dt("iwcard", "Illegal sequence *** in string matcher.  String: {0}", n);
            return n = nf(n).replace("\\*\\*", ".*").replace("\\*", "[^:/.?&;]*"), new RegExp("^" + n + "$")
        }
        if (kr(n)) return new RegExp("^" + n.source + "$");
        throw dt("imatcher", 'Matchers may only be "self", string patterns or RegExp objects');
    }

    function nh(n) {
        var t = [];
        return u(n) && r(n, function (n) {
            t.push(ey(n))
        }), t
    }

    function oy() {
        this.SCE_CONTEXTS = rt;
        var n = ["self"], t = [];
        this.resourceUrlWhitelist = function (t) {
            return arguments.length && (n = nh(t)), n
        };
        this.resourceUrlBlacklist = function (n) {
            return arguments.length && (t = nh(n)), t
        };
        this.$get = ["$injector", function (r) {
            function s(n, t) {
                return n === "self" ? th(t) : !!n.exec(t.href)
            }

            function h(i) {
                for (var e = gt(i.toString()), f = !1, r = 0, u = n.length; r < u; r++) if (s(n[r], e)) {
                    f = !0;
                    break
                }
                if (f) for (r = 0, u = t.length; r < u; r++) if (s(t[r], e)) {
                    f = !1;
                    break
                }
                return f
            }

            function f(n) {
                var t = function (n) {
                    this.$$unwrapTrustedValue = function () {
                        return n
                    }
                };
                return n && (t.prototype = new n), t.prototype.valueOf = function () {
                    return this.$$unwrapTrustedValue()
                }, t.prototype.toString = function () {
                    return this.$$unwrapTrustedValue().toString()
                }, t
            }

            function c(n, t) {
                var r = u.hasOwnProperty(n) ? u[n] : null;
                if (!r) throw dt("icontext", "Attempted to trust a value in invalid context. Context: {0}; Value: {1}", n, t);
                if (t === null || t === i || t === "") return t;
                if (typeof t != "string") throw dt("itype", "Attempted to trust a non-string value in a content requiring a string: Context: {0}", n);
                return new r(t)
            }

            function l(n) {
                return n instanceof e ? n.$$unwrapTrustedValue() : n
            }

            function a(n, t) {
                if (t === null || t === i || t === "") return t;
                var r = u.hasOwnProperty(n) ? u[n] : null;
                if (r && t instanceof r) return t.$$unwrapTrustedValue();
                if (n === rt.RESOURCE_URL) {
                    if (h(t)) return t;
                    throw dt("insecurl", "Blocked loading resource from url not allowed by $sceDelegate policy.  URL: {0}", t.toString());
                } else if (n === rt.HTML) return o(t);
                throw dt("unsafe", "Attempting to use an unsafe value in a safe context.");
            }

            var o = function () {
                throw dt("unsafe", "Attempting to use an unsafe value in a safe context.");
            }, e, u;
            return r.has("$sanitize") && (o = r.get("$sanitize")), e = f(), u = {}, u[rt.HTML] = f(e), u[rt.CSS] = f(e), u[rt.URL] = f(e), u[rt.JS] = f(e), u[rt.RESOURCE_URL] = f(u[rt.URL]), {
                trustAs: c,
                getTrusted: a,
                valueOf: l
            }
        }]
    }

    function sy() {
        var n = !0;
        this.enabled = function (t) {
            return arguments.length && (n = !!t), n
        };
        this.$get = ["$parse", "$sceDelegate", function (t, i) {
            var u;
            if (n && si < 8) throw dt("iequirks", "Strict Contextual Escaping does not support Internet Explorer version < 11 in quirks mode.  You can fix this by adding the text <!doctype html> to the top of your HTML document.  See http://docs.angularjs.org/api/ng.$sce for more information.");
            u = at(rt);
            u.isEnabled = function () {
                return n
            };
            u.trustAs = i.trustAs;
            u.getTrusted = i.getTrusted;
            u.valueOf = i.valueOf;
            n || (u.trustAs = u.getTrusted = function (n, t) {
                return t
            }, u.valueOf = ct);
            u.parseAs = function (n, i) {
                var r = t(i);
                return r.literal && r.constant ? r : t(i, function (t) {
                    return u.getTrusted(n, t)
                })
            };
            var f = u.parseAs, e = u.getTrusted, o = u.trustAs;
            return r(rt, function (n, t) {
                var i = y(t);
                u[or("parse_as_" + i)] = function (t) {
                    return f(n, t)
                };
                u[or("get_trusted_" + i)] = function (t) {
                    return e(n, t)
                };
                u[or("trust_as_" + i)] = function (t) {
                    return o(n, t)
                }
            }), u
        }]
    }

    function hy() {
        this.$get = ["$window", "$document", function (n, t) {
            var s = {}, h = g((/android (\d+)/.exec(y((n.navigator || {}).userAgent)) || [])[1]),
                v = /Boxee/i.test((n.navigator || {}).userAgent), u = t[0] || {}, i, r = u.body && u.body.style, f = !1,
                o = !1, l, a;
            if (r) {
                for (a in r) if (l = /^(Moz|webkit|ms)(?=[A-Z])/.exec(a)) {
                    i = l[0];
                    i = i.substr(0, 1).toUpperCase() + i.substr(1);
                    break
                }
                i || (i = "WebkitOpacity" in r && "webkit");
                f = !!("transition" in r || i + "Transition" in r);
                o = !!("animation" in r || i + "Animation" in r);
                !h || f && o || (f = c(u.body.style.webkitTransition), o = c(u.body.style.webkitAnimation))
            }
            return {
                history: !!(n.history && n.history.pushState && !(h < 4) && !v), hasEvent: function (n) {
                    if (n === "input" && si <= 11) return !1;
                    if (e(s[n])) {
                        var t = u.createElement("div");
                        s[n] = "on" + n in t
                    }
                    return s[n]
                }, csp: ci(), vendorPrefix: i, transitions: f, animations: o, android: h
            }
        }]
    }

    function cy() {
        this.$get = ["$templateCache", "$http", "$q", function (n, t, i) {
            function r(u, f) {
                function h(n) {
                    if (!f) throw tt("tpload", "Failed to load template: {0}", u);
                    return i.reject(n)
                }

                var e, s;
                return r.totalPendingRequests++, e = t.defaults && t.defaults.transformResponse, o(e) ? e = e.filter(function (n) {
                    return n !== df
                }) : e === df && (e = null), s = {cache: n, transformResponse: e}, t.get(u, s).finally(function () {
                    r.totalPendingRequests--
                }).then(function (n) {
                    return n.data
                }, h)
            }

            return r.totalPendingRequests = 0, r
        }]
    }

    function ly() {
        this.$get = ["$rootScope", "$browser", "$location", function (n, t, i) {
            var u = {};
            return u.findBindings = function (n, t, i) {
                var f = n.getElementsByClassName("ng-binding"), u = [];
                return r(f, function (n) {
                    var f = ft.element(n).data("$binding");
                    f && r(f, function (r) {
                        if (i) {
                            var f = new RegExp("(^|\\s)" + nf(t) + "(\\s|\\||$)");
                            f.test(r) && u.push(n)
                        } else r.indexOf(t) != -1 && u.push(n)
                    })
                }), u
            }, u.findModels = function (n, t, i) {
                for (var u = ["ng-", "data-ng-", "ng\\:"], r = 0; r < u.length; ++r) {
                    var e = i ? "=" : "*=", o = "[" + u[r] + "model" + e + '"' + t + '"]', f = n.querySelectorAll(o);
                    if (f.length) return f
                }
            }, u.getLocation = function () {
                return i.url()
            }, u.setLocation = function (t) {
                t !== i.url() && (i.url(t), n.$digest())
            }, u.whenStable = function (n) {
                t.notifyWhenNoOutstandingRequests(n)
            }, u
        }]
    }

    function ay() {
        this.$get = ["$rootScope", "$browser", "$q", "$$q", "$exceptionHandler", function (n, t, i, r, f) {
            function o(o, s, h) {
                var v = u(h) && !h, c = (v ? r : i).defer(), l = c.promise, a;
                return a = t.defer(function () {
                    try {
                        c.resolve(o())
                    } catch (t) {
                        c.reject(t);
                        f(t)
                    } finally {
                        delete e[l.$$timeoutId]
                    }
                    v || n.$apply()
                }, s), l.$$timeoutId = a, e[a] = c, l
            }

            var e = {};
            return o.cancel = function (n) {
                return n && n.$$timeoutId in e ? (e[n.$$timeoutId].reject("canceled"), delete e[n.$$timeoutId], t.defer.cancel(n.$$timeoutId)) : !1
            }, o
        }]
    }

    function gt(n) {
        var t = n;
        return si && (b.setAttribute("href", t), t = b.href), b.setAttribute("href", t), {
            href: b.href,
            protocol: b.protocol ? b.protocol.replace(/:$/, "") : "",
            host: b.host,
            search: b.search ? b.search.replace(/^\?/, "") : "",
            hash: b.hash ? b.hash.replace(/^#/, "") : "",
            hostname: b.hostname,
            port: b.port,
            pathname: b.pathname.charAt(0) === "/" ? b.pathname : "/" + b.pathname
        }
    }

    function th(n) {
        var t = c(n) ? gt(n) : n;
        return t.protocol === oe.protocol && t.host === oe.host
    }

    function vy() {
        this.$get = nt(n)
    }

    function ih(n) {
        function t(u, f) {
            if (h(u)) {
                var e = {};
                return r(u, function (n, i) {
                    e[i] = t(i, n)
                }), e
            }
            return n.factory(u + i, f)
        }

        var i = "Filter";
        this.register = t;
        this.$get = ["$injector", function (n) {
            return function (t) {
                return n.get(t + i)
            }
        }];
        t("currency", rh);
        t("date", sh);
        t("filter", yy);
        t("json", tp);
        t("limitTo", ip);
        t("lowercase", hh);
        t("number", uh);
        t("orderBy", lh);
        t("uppercase", ch)
    }

    function yy() {
        return function (n, t, i) {
            if (!o(n)) return n;
            var r, u;
            switch (typeof t) {
                case"function":
                    r = t;
                    break;
                case"boolean":
                case"number":
                case"string":
                    u = !0;
                case"object":
                    r = py(t, i, u);
                    break;
                default:
                    return n
            }
            return n.filter(r)
        }
    }

    function py(n, t, i) {
        var r = h(n) && "$" in n;
        return t === !0 ? t = et : l(t) || (t = function (n, t) {
            return h(n) || h(t) ? !1 : (n = y("" + n), t = y("" + t), n.indexOf(t) !== -1)
        }), function (u) {
            return r && !h(u) ? ei(u, n.$, t, !1) : ei(u, n, t, i)
        }
    }

    function ei(n, t, i, r, u) {
        var a = typeof n, h = typeof t, f, s, e, c;
        if (h === "string" && t.charAt(0) === "!") return !ei(n, t.substring(1), i, r);
        if (o(n)) return n.some(function (n) {
            return ei(n, t, i, r)
        });
        switch (a) {
            case"object":
                if (r) {
                    for (f in n) if (f.charAt(0) !== "$" && ei(n[f], t, i, !0)) return !0;
                    return u ? !1 : ei(n, t, i, !1)
                }
                if (h === "object") {
                    for (f in t) if ((s = t[f], !l(s)) && (e = f === "$", c = e ? n : n[f], !ei(c, s, i, e, e))) return !1;
                    return !0
                }
                return i(n, t);
            case"function":
                return !1;
            default:
                return i(n, t)
        }
    }

    function rh(n) {
        var t = n.NUMBER_FORMATS;
        return function (n, i, r) {
            return e(i) && (i = t.CURRENCY_SYM), e(r) && (r = t.PATTERNS[1].maxFrac), n == null ? n : fh(n, t.PATTERNS[1], t.GROUP_SEP, t.DECIMAL_SEP, r).replace(/\u00A4/g, i)
        }
    }

    function uh(n) {
        var t = n.NUMBER_FORMATS;
        return function (n, i) {
            return n == null ? n : fh(n, t.PATTERNS[0], t.GROUP_SEP, t.DECIMAL_SEP, i)
        }
    }

    function fh(n, t, i, r, u) {
        var l, v, k, s, c;
        if (!isFinite(n) || h(n)) return "";
        l = n < 0;
        n = Math.abs(n);
        var a = n + "", o = "", w = [], b = !1;
        if (a.indexOf("e") !== -1 && (v = a.match(/([\d\.]+)e(-?)(\d+)/), v && v[2] == "-" && v[3] > u + 1 ? n = 0 : (o = a, b = !0)), b) u > 0 && n < 1 && (o = n.toFixed(u), n = parseFloat(o)); else {
            k = (a.split(se)[1] || "").length;
            e(u) && (u = Math.min(Math.max(t.minFrac, k), t.maxFrac));
            n = +(Math.round(+(n.toString() + "e" + u)).toString() + "e" + -u);
            s = ("" + n).split(se);
            c = s[0];
            s = s[1] || "";
            var f, y = 0, p = t.lgSize, d = t.gSize;
            if (c.length >= p + d) for (y = c.length - p, f = 0; f < y; f++) (y - f) % d == 0 && f !== 0 && (o += i), o += c.charAt(f);
            for (f = y; f < c.length; f++) (c.length - f) % p == 0 && f !== 0 && (o += i), o += c.charAt(f);
            while (s.length < u) s += "0";
            u && u !== "0" && (o += r + s.substr(0, u))
        }
        return n === 0 && (l = !1), w.push(l ? t.negPre : t.posPre, o, l ? t.negSuf : t.posSuf), w.join("")
    }

    function pu(n, t, i) {
        var r = "";
        for (n < 0 && (r = "-", n = -n), n = "" + n; n.length < t;) n = "0" + n;
        return i && (n = n.substr(n.length - t)), r + n
    }

    function d(n, t, i, r) {
        return i = i || 0, function (u) {
            var f = u["get" + n]();
            return (i > 0 || f > -i) && (f += i), f === 0 && i == -12 && (f = 12), pu(f, t, r)
        }
    }

    function wu(n, t) {
        return function (i, r) {
            var u = i["get" + n](), f = bi(t ? "SHORT" + n : n);
            return r[f][u]
        }
    }

    function wy(n) {
        var t = -1 * n.getTimezoneOffset(), i = t >= 0 ? "+" : "";
        return i + (pu(Math[t > 0 ? "floor" : "ceil"](t / 60), 2) + pu(Math.abs(t % 60), 2))
    }

    function eh(n) {
        var t = new Date(n, 0, 1).getDay();
        return new Date(n, 0, (t <= 4 ? 5 : 12) - t)
    }

    function by(n) {
        return new Date(n.getFullYear(), n.getMonth(), n.getDate() + (4 - n.getDay()))
    }

    function oh(n) {
        return function (t) {
            var i = eh(t.getFullYear()), r = by(t), u = +r - +i, f = 1 + Math.round(u / 6048e5);
            return pu(f, n)
        }
    }

    function ky(n, t) {
        return n.getHours() < 12 ? t.AMPMS[0] : t.AMPMS[1]
    }

    function sh(n) {
        function i(n) {
            var i;
            if (i = n.match(t)) {
                var r = new Date(0), u = 0, f = 0, e = i[8] ? r.setUTCFullYear : r.setFullYear,
                    o = i[8] ? r.setUTCHours : r.setHours;
                i[9] && (u = g(i[9] + i[10]), f = g(i[9] + i[11]));
                e.call(r, g(i[1]), g(i[2]) - 1, g(i[3]));
                var s = g(i[4] || 0) - u, h = g(i[5] || 0) - f, c = g(i[6] || 0),
                    l = Math.round(parseFloat("0." + (i[7] || 0)) * 1e3);
                return o.call(r, s, h, c, l), r
            }
            return n
        }

        var t = /^(\d{4})-?(\d\d)-?(\d\d)(?:T(\d\d)(?::?(\d\d)(?::?(\d\d)(?:\.(\d+))?)?)?(Z|([+-])(\d\d):?(\d\d))?)?$/;
        return function (t, u, f) {
            var h = "", e = [], o, s;
            if (u = u || "mediumDate", u = n.DATETIME_FORMATS[u] || u, c(t) && (t = np.test(t) ? g(t) : i(t)), k(t) && (t = new Date(t)), !lt(t)) return t;
            while (u) s = gy.exec(u), s ? (e = rr(e, s, 1), u = e.pop()) : (e.push(u), u = null);
            return f && f === "UTC" && (t = new Date(t.getTime()), t.setMinutes(t.getMinutes() + t.getTimezoneOffset())), r(e, function (i) {
                o = dy[i];
                h += o ? o(t, n.DATETIME_FORMATS) : i.replace(/(^'|'$)/g, "").replace(/''/g, "'")
            }), h
        }
    }

    function tp() {
        return function (n, t) {
            return e(t) && (t = 2), ur(n, t)
        }
    }

    function ip() {
        return function (n, t) {
            return (k(n) && (n = n.toString()), !o(n) && !c(n)) ? n : (t = Math.abs(Number(t)) === Infinity ? Number(t) : g(t), t ? t > 0 ? n.slice(0, t) : n.slice(t) : c(n) ? "" : [])
        }
    }

    function lh(n) {
        return function (t, i, r) {
            function h(n, t) {
                for (var u, r = 0; r < i.length; r++) if (u = i[r](n, t), u !== 0) return u;
                return 0
            }

            function u(n, t) {
                return t ? function (t, i) {
                    return n(i, t)
                } : n
            }

            function e(n) {
                switch (typeof n) {
                    case"number":
                    case"boolean":
                    case"string":
                        return !0;
                    default:
                        return !1
                }
            }

            function s(n) {
                return n === null ? "null" : typeof n.valueOf == "function" && (n = n.valueOf(), e(n)) ? n : typeof n.toString == "function" && (n = n.toString(), e(n)) ? n : ""
            }

            function f(n, t) {
                var i = typeof n, r = typeof t;
                return i === r && i === "object" && (n = s(n), t = s(t)), i === r ? (i === "string" && (n = n.toLowerCase(), t = t.toLowerCase()), n === t) ? 0 : n < t ? -1 : 1 : i < r ? -1 : 1
            }

            return di(t) ? (i = o(i) ? i : [i], i.length === 0 && (i = ["+"]), i = i.map(function (t) {
                var r = !1, i = t || ct, e;
                if (c(t)) {
                    if ((t.charAt(0) == "+" || t.charAt(0) == "-") && (r = t.charAt(0) == "-", t = t.substring(1)), t === "") return u(f, r);
                    if (i = n(t), i.constant) return e = i(), u(function (n, t) {
                        return f(n[e], t[e])
                    }, r)
                }
                return u(function (n, t) {
                    return f(i(n), i(t))
                }, r)
            }), gu.call(t).sort(u(h, r))) : t
        }
    }

    function oi(n) {
        return l(n) && (n = {link: n}), n.restrict = n.restrict || "AC", nt(n)
    }

    function rp(n, t) {
        n.$name = t
    }

    function vh(n, t, u, f, e) {
        var o = this, s = [], h = o.$$parentForm = n.parent().controller("form") || vr;
        o.$error = {};
        o.$$success = {};
        o.$pending = i;
        o.$name = e(t.name || t.ngForm || "")(u);
        o.$dirty = !1;
        o.$pristine = !0;
        o.$valid = !0;
        o.$invalid = !1;
        o.$submitted = !1;
        h.$addControl(o);
        o.$rollbackViewValue = function () {
            r(s, function (n) {
                n.$rollbackViewValue()
            })
        };
        o.$commitViewValue = function () {
            r(s, function (n) {
                n.$commitViewValue()
            })
        };
        o.$addControl = function (n) {
            li(n.$name, "input");
            s.push(n);
            n.$name && (o[n.$name] = n)
        };
        o.$$renameControl = function (n, t) {
            var i = n.$name;
            o[i] === n && delete o[i];
            o[t] = n;
            n.$name = t
        };
        o.$removeControl = function (n) {
            n.$name && o[n.$name] === n && delete o[n.$name];
            r(o.$pending, function (t, i) {
                o.$setValidity(i, null, n)
            });
            r(o.$error, function (t, i) {
                o.$setValidity(i, null, n)
            });
            r(o.$$success, function (t, i) {
                o.$setValidity(i, null, n)
            });
            ir(s, n)
        };
        ec({
            ctrl: this, $element: n, set: function (n, t, i) {
                var r = n[t], u;
                r ? (u = r.indexOf(i), u === -1 && r.push(i)) : n[t] = [i]
            }, unset: function (n, t, i) {
                var r = n[t];
                r && (ir(r, i), r.length === 0 && delete n[t])
            }, parentForm: h, $animate: f
        });
        o.$setDirty = function () {
            f.removeClass(n, wi);
            f.addClass(n, ku);
            o.$dirty = !0;
            o.$pristine = !1;
            h.$setDirty()
        };
        o.$setPristine = function () {
            f.setClass(n, wi, ku + " " + he);
            o.$dirty = !1;
            o.$pristine = !0;
            o.$submitted = !1;
            r(s, function (n) {
                n.$setPristine()
            })
        };
        o.$setUntouched = function () {
            r(s, function (n) {
                n.$setUntouched()
            })
        };
        o.$setSubmitted = function () {
            f.addClass(n, he);
            o.$submitted = !0;
            h.$setSubmitted()
        }
    }

    function le(n) {
        n.$formatters.push(function (t) {
            return n.$isEmpty(t) ? t : t.toString()
        })
    }

    function cp(n, t, i, r, u, f) {
        yr(n, t, i, r, u, f);
        le(r)
    }

    function yr(n, t, i, r, u, f) {
        var c = y(t[0].type), s, o, e, h;
        if (!u.android) {
            s = !1;
            t.on("compositionstart", function () {
                s = !0
            });
            t.on("compositionend", function () {
                s = !1;
                o()
            })
        }
        if (o = function (n) {
            if (e && (f.defer.cancel(e), e = null), !s) {
                var u = t.val(), o = n && n.type;
                c === "password" || i.ngTrim && i.ngTrim === "false" || (u = p(u));
                (r.$viewValue !== u || u === "" && r.$$hasNativeValidators) && r.$setViewValue(u, o)
            }
        }, u.hasEvent("input")) t.on("input", o); else {
            h = function (n, t, i) {
                e || (e = f.defer(function () {
                    e = null;
                    t && t.value === i || o(n)
                }))
            };
            t.on("keydown", function (n) {
                var t = n.keyCode;
                t === 91 || 15 < t && t < 19 || 37 <= t && t <= 40 || h(n, this, this.value)
            });
            if (u.hasEvent("paste")) t.on("paste cut", h)
        }
        t.on("change", o);
        r.$render = function () {
            t.val(r.$isEmpty(r.$viewValue) ? "" : r.$viewValue)
        }
    }

    function lp(n, t) {
        var i;
        if (lt(n)) return n;
        if (c(n) && (ce.lastIndex = 0, i = ce.exec(n), i)) {
            var r = +i[1], s = +i[2], u = 0, f = 0, e = 0, o = 0, h = eh(r), l = (s - 1) * 7;
            return t && (u = t.getHours(), f = t.getMinutes(), e = t.getSeconds(), o = t.getMilliseconds()), new Date(r, 0, h.getDate() + l, u, f, e, o)
        }
        return NaN
    }

    function bu(n, t) {
        return function (i, u) {
            var e, f;
            if (lt(i)) return i;
            if (c(i)) {
                if (i.charAt(0) == '"' && i.charAt(i.length - 1) == '"' && (i = i.substring(1, i.length - 1)), ep.test(i)) return new Date(i);
                if (n.lastIndex = 0, e = n.exec(i), e) return e.shift(), f = u ? {
                    yyyy: u.getFullYear(),
                    MM: u.getMonth() + 1,
                    dd: u.getDate(),
                    HH: u.getHours(),
                    mm: u.getMinutes(),
                    ss: u.getSeconds(),
                    sss: u.getMilliseconds() / 1e3
                } : {yyyy: 1970, MM: 1, dd: 1, HH: 0, mm: 0, ss: 0, sss: 0}, r(e, function (n, i) {
                    i < t.length && (f[t[i]] = +n)
                }), new Date(f.yyyy, f.MM - 1, f.dd, f.HH, f.mm, f.ss || 0, f.sss * 1e3 || 0)
            }
            return NaN
        }
    }

    function pr(n, t, r, f) {
        return function (o, s, h, c, l, a, v) {
            function k(n) {
                return n && !(n.getTime && n.getTime() !== n.getTime())
            }

            function d(n) {
                return u(n) ? lt(n) ? n : r(n) : i
            }

            var p, y, w, b;
            gh(o, s, h, c);
            yr(o, s, h, c, l, a);
            p = c && c.$options && c.$options.timezone;
            c.$$parserName = n;
            c.$parsers.push(function (n) {
                if (c.$isEmpty(n)) return null;
                if (t.test(n)) {
                    var u = r(n, y);
                    return p === "UTC" && u.setMinutes(u.getMinutes() - u.getTimezoneOffset()), u
                }
                return i
            });
            c.$formatters.push(function (n) {
                if (n && !lt(n)) throw du("datefmt", "Expected `{0}` to be a date", n);
                if (k(n)) {
                    if (y = n, y && p === "UTC") {
                        var t = 6e4 * y.getTimezoneOffset();
                        y = new Date(y.getTime() + t)
                    }
                    return v("date")(n, f, p)
                }
                return y = null, ""
            });
            (u(h.min) || h.ngMin) && (c.$validators.min = function (n) {
                return !k(n) || e(w) || r(n) >= w
            }, h.$observe("min", function (n) {
                w = d(n);
                c.$validate()
            }));
            (u(h.max) || h.ngMax) && (c.$validators.max = function (n) {
                return !k(n) || e(b) || r(n) <= b
            }, h.$observe("max", function (n) {
                b = d(n);
                c.$validate()
            }))
        }
    }

    function gh(n, t, r, u) {
        var f = t[0], e = u.$$hasNativeValidators = h(f.validity);
        e && u.$parsers.push(function (n) {
            var r = t.prop(pc) || {};
            return r.badInput && !r.typeMismatch ? i : n
        })
    }

    function ap(n, t, r, f, o, s) {
        var h, c;
        gh(n, t, r, f);
        yr(n, t, r, f, o, s);
        f.$$parserName = "number";
        f.$parsers.push(function (n) {
            return f.$isEmpty(n) ? null : hp.test(n) ? parseFloat(n) : i
        });
        f.$formatters.push(function (n) {
            if (!f.$isEmpty(n)) {
                if (!k(n)) throw du("numfmt", "Expected `{0}` to be a number", n);
                n = n.toString()
            }
            return n
        });
        (r.min || r.ngMin) && (f.$validators.min = function (n) {
            return f.$isEmpty(n) || e(h) || n >= h
        }, r.$observe("min", function (n) {
            u(n) && !k(n) && (n = parseFloat(n, 10));
            h = k(n) && !isNaN(n) ? n : i;
            f.$validate()
        }));
        (r.max || r.ngMax) && (f.$validators.max = function (n) {
            return f.$isEmpty(n) || e(c) || n <= c
        }, r.$observe("max", function (n) {
            u(n) && !k(n) && (n = parseFloat(n, 10));
            c = k(n) && !isNaN(n) ? n : i;
            f.$validate()
        }))
    }

    function vp(n, t, i, r, u, f) {
        yr(n, t, i, r, u, f);
        le(r);
        r.$$parserName = "url";
        r.$validators.url = function (n, t) {
            var i = n || t;
            return r.$isEmpty(i) || op.test(i)
        }
    }

    function yp(n, t, i, r, u, f) {
        yr(n, t, i, r, u, f);
        le(r);
        r.$$parserName = "email";
        r.$validators.email = function (n, t) {
            var i = n || t;
            return r.$isEmpty(i) || sp.test(i)
        }
    }

    function pp(n, t, i, r) {
        e(i.name) && t.attr("name", br());
        var u = function (n) {
            t[0].checked && r.$setViewValue(i.value, n && n.type)
        };
        t.on("click", u);
        r.$render = function () {
            var n = i.value;
            t[0].checked = n == r.$viewValue
        };
        i.$observe("value", r.$render)
    }

    function nc(n, t, i, r, f) {
        var e;
        if (u(r)) {
            if (e = n(r), !e.constant) throw v("ngModel")("constexpr", "Expected constant expression for `{0}`, but saw `{1}`.", i, r);
            return e(t)
        }
        return f
    }

    function wp(n, t, i, r, u, f, e, o) {
        var s = nc(o, n, "ngTrueValue", i.ngTrueValue, !0), h = nc(o, n, "ngFalseValue", i.ngFalseValue, !1),
            c = function (n) {
                r.$setViewValue(t[0].checked, n && n.type)
            };
        t.on("click", c);
        r.$render = function () {
            t[0].checked = r.$viewValue
        };
        r.$isEmpty = function (n) {
            return n === !1
        };
        r.$formatters.push(function (n) {
            return et(n, s)
        });
        r.$parsers.push(function (n) {
            return n ? s : h
        })
    }

    function ae(n, t) {
        return n = "ngClass" + n, ["$animate", function (i) {
            function f(n, t) {
                var f = [], i, u, r;
                n:for (i = 0; i < n.length; i++) {
                    for (u = n[i], r = 0; r < t.length; r++) if (u == t[r]) continue n;
                    f.push(u)
                }
                return f
            }

            function u(n) {
                if (o(n)) return n;
                if (c(n)) return n.split(" ");
                if (h(n)) {
                    var t = [];
                    return r(n, function (n, i) {
                        n && (t = t.concat(i.split(" ")))
                    }), t
                }
                return n
            }

            return {
                restrict: "AC", link: function (e, o, s) {
                    function l(n) {
                        var t = c(n, 1);
                        s.$addClass(t)
                    }

                    function v(n) {
                        var t = c(n, -1);
                        s.$removeClass(t)
                    }

                    function c(n, t) {
                        var i = o.data("$classCounts") || {}, u = [];
                        return r(n, function (n) {
                            (t > 0 || i[n]) && (i[n] = (i[n] || 0) + t, i[n] === +(t > 0) && u.push(n))
                        }), o.data("$classCounts", i), u.join(" ")
                    }

                    function y(n, t) {
                        var r = f(t, n), u = f(n, t);
                        r = c(r, 1);
                        u = c(u, -1);
                        r && r.length && i.addClass(o, r);
                        u && u.length && i.removeClass(o, u)
                    }

                    function a(n) {
                        var i, r;
                        (t === !0 || e.$index % 2 === t) && (i = u(n || []), h ? et(n, h) || (r = u(h), y(r, i)) : l(i));
                        h = at(n)
                    }

                    var h;
                    e.$watch(s[n], a, !0);
                    s.$observe("class", function () {
                        a(e.$eval(s[n]))
                    });
                    n !== "ngClass" && e.$watch("$index", function (i, r) {
                        var o = i & 1, f;
                        o !== (r & 1) && (f = u(e.$eval(s[n])), o === t ? l(f) : v(f))
                    })
                }
            }
        }]
    }

    function ec(n) {
        function l(n, r, e) {
            r === i ? a("$pending", n, e) : v("$pending", n, e);
            tr(r) ? r ? (u(t.$error, n, e), o(t.$$success, n, e)) : (o(t.$error, n, e), u(t.$$success, n, e)) : (u(t.$error, n, e), u(t.$$success, n, e));
            t.$pending ? (f(fc, !0), t.$valid = t.$invalid = i, s("", null)) : (f(fc, !1), t.$valid = oc(t.$error), t.$invalid = !t.$valid, s("", t.$valid));
            var h;
            h = t.$pending && t.$pending[n] ? i : t.$error[n] ? !1 : t.$$success[n] ? !0 : null;
            s(n, h);
            c.$setValidity(n, h, t)
        }

        function a(n, i, r) {
            t[n] || (t[n] = {});
            o(t[n], i, r)
        }

        function v(n, r, f) {
            t[n] && u(t[n], r, f);
            oc(t[n]) && (t[n] = i)
        }

        function f(n, t) {
            t && !r[n] ? (h.addClass(e, n), r[n] = !0) : !t && r[n] && (h.removeClass(e, n), r[n] = !1)
        }

        function s(n, t) {
            n = n ? "-" + eo(n, "-") : "";
            f(wr + n, t === !0);
            f(rc + n, t === !1)
        }

        var t = n.ctrl, e = n.$element, r = {}, o = n.set, u = n.unset, c = n.parentForm, h = n.$animate;
        r[rc] = !(r[wr] = e.hasClass(wr));
        t.$setValidity = l
    }

    function oc(n) {
        if (n) for (var t in n) return !1;
        return !0
    }

    var yc = /^\/(.+)\/([a-z]*)$/, pc = "validity", y = function (n) {
            return c(n) ? n.toLowerCase() : n
        }, ye = Object.prototype.hasOwnProperty, bi = function (n) {
            return c(n) ? n.toUpperCase() : n
        }, wc = function (n) {
            return c(n) ? n.replace(/[A-Z]/g, function (n) {
                return String.fromCharCode(n.charCodeAt(0) | 32)
            }) : n
        }, bc = function (n) {
            return c(n) ? n.replace(/[a-z]/g, function (n) {
                return String.fromCharCode(n.charCodeAt(0) & -33)
            }) : n
        }, o, p, nf, ci, fr, fo, uf, ff, lo, ri, sr, vf, yf, ts, is, bf, au, ys, it, vi, yi, ws, bs, ds, dt, rt, tt, b, oe,
        se, hh, ch, ah, ar, vr, he;
    "i" !== "I".toLowerCase() && (y = wc, bi = bc);
    var si, f, ut, gu = [].slice, kc = [].splice, dc = [].push, ni = Object.prototype.toString, hi = v("ng"),
        ft = n.angular || (n.angular = {}), ki, gc = 0;
    si = t.documentMode;
    s.$inject = [];
    ct.$inject = [];
    o = Array.isArray;
    p = function (n) {
        return c(n) ? n.trim() : n
    };
    nf = function (n) {
        return n.replace(/([-()\[\]{}+?*.$\^|,:#<!\\])/g, "\\$1").replace(/\x08/g, "\\x08")
    };
    ci = function () {
        if (u(ci.isActive_)) return ci.isActive_;
        var n = !!(t.querySelector("[ng-csp]") || t.querySelector("[data-ng-csp]"));
        if (!n) try {
            new Function("")
        } catch (i) {
            n = !0
        }
        return ci.isActive_ = n
    };
    fr = ["ng-", "data-ng-", "ng:", "x-ng-"];
    fo = /[A-Z]/g;
    uf = !1;
    var vt = 1, iu = 3, so = 8, ho = 9, of = 11;
    lo = {full: "1.3.11", major: 1, minor: 3, dot: 11, codeName: "spiffy-manatee"};
    w.expando = "ng339";
    var ru = w.cache = {}, vl = 1, uu = function (n, t, i) {
        n.addEventListener(t, i, !1)
    }, er = function (n, t, i) {
        n.removeEventListener(t, i, !1)
    };
    w._data = function (n) {
        return this.cache[n[this.expando]] || {}
    };
    var pl = /([\:\-\_]+(.))/g, wl = /^moz([A-Z])/, bl = {mouseleave: "mouseout", mouseenter: "mouseover"},
        sf = v("jqLite");
    var kl = /^<(\w+)\s*\/?>(?:<\/\1>|)$/, dl = /<|&#?\w+;/, gl = /<([\w:]+)/,
        na = /<(?!area|br|col|embed|hr|img|input|link|meta|param)(([\w:]+)[^>]*)\/>/gi, st = {
            option: [1, '<select multiple="multiple">', "<\/select>"],
            thead: [1, "<table>", "<\/table>"],
            col: [2, "<table><colgroup>", "<\/colgroup><\/table>"],
            tr: [2, "<table><tbody>", "<\/tbody><\/table>"],
            td: [3, "<table><tbody><tr>", "<\/tr><\/tbody><\/table>"],
            _default: [0, "", ""]
        };
    st.optgroup = st.option;
    st.tbody = st.tfoot = st.colgroup = st.caption = st.thead;
    st.th = st.td;
    ri = w.prototype = {
        ready: function (i) {
            function r() {
                u || (u = !0, i())
            }

            var u = !1;
            if (t.readyState === "complete") setTimeout(r); else {
                this.on("DOMContentLoaded", r);
                w(n).on("load", r)
            }
        }, toString: function () {
            var n = [];
            return r(this, function (t) {
                n.push("" + t)
            }), "[" + n.join(", ") + "]"
        }, eq: function (n) {
            return n >= 0 ? f(this[n]) : f(this[this.length + n])
        }, length: 0, push: dc, sort: [].sort, splice: [].splice
    };
    sr = {};
    r("multiple,selected,checked,disabled,readOnly,required,open".split(","), function (n) {
        sr[y(n)] = n
    });
    vf = {};
    r("input,select,option,textarea,button,form,details".split(","), function (n) {
        vf[n] = !0
    });
    yf = {ngMinlength: "minlength", ngMaxlength: "maxlength", ngMin: "min", ngMax: "max", ngPattern: "pattern"};
    r({data: lf, removeData: eu}, function (n, t) {
        w[t] = n
    });
    r({
        data: lf, inheritedData: lu, scope: function (n) {
            return f.data(n, "$scope") || lu(n.parentNode || n, ["$isolateScope", "$scope"])
        }, isolateScope: function (n) {
            return f.data(n, "$isolateScope") || f.data(n, "$isolateScopeNoTemplate")
        }, controller: po, injector: function (n) {
            return lu(n, "$injector")
        }, removeAttr: function (n, t) {
            n.removeAttribute(t)
        }, hasClass: su, css: function (n, t, i) {
            if (t = or(t), u(i)) n.style[t] = i; else return n.style[t]
        }, attr: function (n, t, r) {
            var f = y(t), e;
            if (sr[f]) if (u(r)) r ? (n[t] = !0, n.setAttribute(t, f)) : (n[t] = !1, n.removeAttribute(f)); else return n[t] || (n.attributes.getNamedItem(t) || s).specified ? f : i; else if (u(r)) n.setAttribute(t, r); else if (n.getAttribute) return e = n.getAttribute(t, 2), e === null ? i : e
        }, prop: function (n, t, i) {
            if (u(i)) n[t] = i; else return n[t]
        }, text: function () {
            function n(n, t) {
                if (e(t)) {
                    var i = n.nodeType;
                    return i === vt || i === iu ? n.textContent : ""
                }
                n.textContent = t
            }

            return n.$dv = "", n
        }(), val: function (n, t) {
            if (e(t)) {
                if (n.multiple && pt(n) === "select") {
                    var i = [];
                    return r(n.options, function (n) {
                        n.selected && i.push(n.value || n.text)
                    }), i.length === 0 ? null : i
                }
                return n.value
            }
            n.value = t
        }, html: function (n, t) {
            if (e(t)) return n.innerHTML;
            fu(n, !0);
            n.innerHTML = t
        }, empty: wo
    }, function (n, t) {
        w.prototype[t] = function (t, r) {
            var u, s, e = this.length, f, l, o, c;
            if (n !== wo && (n.length == 2 && n !== su && n !== po ? t : r) === i) {
                if (h(t)) {
                    for (u = 0; u < e; u++) if (n === lf) n(this[u], t); else for (s in t) n(this[u], s, t[s]);
                    return this
                }
                for (f = n.$dv, l = f === i ? Math.min(e, 1) : e, o = 0; o < l; o++) c = n(this[o], t, r), f = f ? f + c : c;
                return f
            }
            for (u = 0; u < e; u++) n(this[u], t, r);
            return this
        }
    });
    r({
        removeData: eu, on: function fa(n, t, i, r) {
            var h, c, o;
            if (u(r)) throw sf("onargs", "jqLite#on() does not support the `selector` or `eventData` parameters");
            if (ao(n)) {
                var s = ou(n, !0), f = s.events, e = s.handle;
                for (e || (e = s.handle = ua(n, f)), h = t.indexOf(" ") >= 0 ? t.split(" ") : [t], c = h.length; c--;) t = h[c], o = f[t], o || (f[t] = [], t === "mouseenter" || t === "mouseleave" ? fa(n, bl[t], function (n) {
                    var r = this, i = n.relatedTarget;
                    i && (i === r || r.contains(i)) || e(n, t)
                }) : t !== "$destroy" && uu(n, t, e), o = f[t]), o.push(i)
            }
        }, off: yo, one: function (n, t, i) {
            n = f(n);
            n.on(t, function r() {
                n.off(t, i);
                n.off(t, r)
            });
            n.on(t, i)
        }, replaceWith: function (n, t) {
            var i, u = n.parentNode;
            fu(n);
            r(new w(t), function (t) {
                i ? u.insertBefore(t, i.nextSibling) : u.replaceChild(t, n);
                i = t
            })
        }, children: function (n) {
            var t = [];
            return r(n.childNodes, function (n) {
                n.nodeType === vt && t.push(n)
            }), t
        }, contents: function (n) {
            return n.contentDocument || n.childNodes || []
        }, append: function (n, t) {
            var r = n.nodeType, i, u, f;
            if (r === vt || r === of) for (t = new w(t), i = 0, u = t.length; i < u; i++) f = t[i], n.appendChild(f)
        }, prepend: function (n, t) {
            if (n.nodeType === vt) {
                var i = n.firstChild;
                r(new w(t), function (t) {
                    n.insertBefore(t, i)
                })
            }
        }, wrap: function (n, t) {
            t = f(t).eq(0).clone()[0];
            var i = n.parentNode;
            i && i.replaceChild(t, n);
            t.appendChild(n)
        }, remove: bo, detach: function (n) {
            bo(n, !0)
        }, after: function (n, t) {
            var u = n, e = n.parentNode, i, f, r;
            for (t = new w(t), i = 0, f = t.length; i < f; i++) r = t[i], e.insertBefore(r, u.nextSibling), u = r
        }, addClass: cu, removeClass: hu, toggleClass: function (n, t, i) {
            t && r(t.split(" "), function (t) {
                var r = i;
                e(r) && (r = !su(n, t));
                (r ? cu : hu)(n, t)
            })
        }, parent: function (n) {
            var t = n.parentNode;
            return t && t.nodeType !== of ? t : null
        }, next: function (n) {
            return n.nextElementSibling
        }, find: function (n, t) {
            return n.getElementsByTagName ? n.getElementsByTagName(t) : []
        }, clone: cf, triggerHandler: function (n, t, i) {
            var u, f, e, o = t.type || t, h = ou(n), c = h && h.events, l = c && c[o];
            l && (u = {
                preventDefault: function () {
                    this.defaultPrevented = !0
                }, isDefaultPrevented: function () {
                    return this.defaultPrevented === !0
                }, stopImmediatePropagation: function () {
                    this.immediatePropagationStopped = !0
                }, isImmediatePropagationStopped: function () {
                    return this.immediatePropagationStopped === !0
                }, stopPropagation: s, type: o, target: n
            }, t.type && (u = a(u, t)), f = at(l), e = i ? [u].concat(i) : [u], r(f, function (t) {
                u.isImmediatePropagationStopped() || t.apply(n, e)
            }))
        }
    }, function (n, t) {
        w.prototype[t] = function (t, i, r) {
            for (var o, s = 0, h = this.length; s < h; s++) e(o) ? (o = n(this[s], t, i, r), u(o) && (o = f(o))) : af(o, n(this[s], t, i, r));
            return u(o) ? o : this
        };
        w.prototype.bind = w.prototype.on;
        w.prototype.unbind = w.prototype.off
    });
    hr.prototype = {
        put: function (n, t) {
            this[ai(n, this.nextUid)] = t
        }, get: function (n) {
            return this[ai(n, this.nextUid)]
        }, remove: function (n) {
            var t = this[n = ai(n, this.nextUid)];
            return delete this[n], t
        }
    };
    var go = /^function\s*[^\(]*\(\s*([^\)]*)\)/m, oa = /,/, sa = /^\s*(_?)(\S+?)\1\s*$/,
        ns = /((\/\/.*$)|(\/\*[\s\S]*?\*\/))/mg, ui = v("$injector");
    wf.$$annotate = pf;
    ts = v("$animate");
    is = ["$provide", function (n) {
        this.$$selectors = {};
        this.register = function (t, i) {
            var r = t + "-animation";
            if (t && t.charAt(0) != ".") throw ts("notcsel", "Expecting class selector starting with '.' got '{0}'.", t);
            this.$$selectors[t.substr(1)] = r;
            n.factory(r, i)
        };
        this.classNameFilter = function (n) {
            return arguments.length === 1 && (this.$$classNameFilter = n instanceof RegExp ? n : null), this.$$classNameFilter
        };
        this.$get = ["$$q", "$$asyncCallback", "$rootScope", function (n, t, i) {
            function v(t) {
                var r, u = n.defer();
                return u.promise.$$cancelFn = function () {
                    r && r()
                }, i.$$postDigest(function () {
                    r = t(function () {
                        u.resolve()
                    })
                }), u.promise
            }

            function y(n, t) {
                var i = [], u = [], f = ot();
                return r((n.attr("class") || "").split(/\s+/), function (n) {
                    f[n] = !0
                }), r(t, function (n, t) {
                    var r = f[t];
                    n === !1 && r ? u.push(t) : n !== !0 || r || i.push(t)
                }), i.length + u.length > 0 && [i.length ? i : null, u.length ? u : null]
            }

            function l(n, t, i) {
                for (var f, r = 0, u = t.length; r < u; ++r) f = t[r], n[f] = i
            }

            function u() {
                return e || (e = n.defer(), t(function () {
                    e.resolve();
                    e = null
                })), e.promise
            }

            function h(n, t) {
                if (ft.isObject(t)) {
                    var i = a(t.from || {}, t.to || {});
                    n.css(i)
                }
            }

            var e;
            return {
                animate: function (n, t, i) {
                    return h(n, {from: t, to: i}), u()
                }, enter: function (n, t, i, r) {
                    return h(n, r), i ? i.after(n) : t.prepend(n), u()
                }, leave: function (n) {
                    return n.remove(), u()
                }, move: function (n, t, i, r) {
                    return this.enter(n, t, i, r)
                }, addClass: function (n, t, i) {
                    return this.setClass(n, t, [], i)
                }, $$addClassImmediately: function (n, t, i) {
                    return n = f(n), t = c(t) ? t : o(t) ? t.join(" ") : "", r(n, function (n) {
                        cu(n, t)
                    }), h(n, i), u()
                }, removeClass: function (n, t, i) {
                    return this.setClass(n, [], t, i)
                }, $$removeClassImmediately: function (n, t, i) {
                    return n = f(n), t = c(t) ? t : o(t) ? t.join(" ") : "", r(n, function (n) {
                        hu(n, t)
                    }), h(n, i), u()
                }, setClass: function (n, t, i, r) {
                    var c = this, e = "$$animateClasses", h = !1, u, s;
                    return n = f(n), u = n.data(e), u ? r && u.options && (u.options = ft.extend(u.options || {}, r)) : (u = {
                        classes: {},
                        options: r
                    }, h = !0), s = u.classes, t = o(t) ? t : t.split(" "), i = o(i) ? i : i.split(" "), l(s, t, !0), l(s, i, !1), h && (u.promise = v(function (t) {
                        var r = n.data(e), i;
                        n.removeData(e);
                        r && (i = y(n, r.classes), i && c.$$setClassImmediately(n, i[0], i[1], r.options));
                        t()
                    }), n.data(e, u)), u.promise
                }, $$setClassImmediately: function (n, t, i, r) {
                    return t && this.$$addClassImmediately(n, t), i && this.$$removeClassImmediately(n, i), h(n, r), u()
                }, enabled: s, cancel: s
            }
        }]
    }];
    tt = v("$compile");
    rs.$inject = ["$provide", "$$sanitizeUriProvider"];
    bf = /^((?:x|data)[\:\-_])/i;
    var es = "application/json", kf = {"Content-Type": es + ";charset=utf-8"}, da = /^\[|^\{(?!\{)/,
        ga = {"[": /]$/, "{": /}$/}, nv = /^\)\]\}',?\n/;
    au = v("$interpolate");
    var hv = /^([^\?#]*)(\?([^#]*))?(#(.*))?$/, cv = {http: 80, https: 443, ftp: 21}, vu = v("$location");
    ys = {
        $$html5: !1, $$replace: !1, absUrl: yu("$$absUrl"), url: function (n) {
            if (e(n)) return this.$$url;
            var t = hv.exec(n);
            return (t[1] || n === "") && this.path(decodeURIComponent(t[1])), (t[2] || t[1] || n === "") && this.search(t[3] || ""), this.hash(t[5] || ""), this
        }, protocol: yu("$$protocol"), host: yu("$$host"), port: yu("$$port"), path: ps("$$path", function (n) {
            return n = n !== null ? n.toString() : "", n.charAt(0) == "/" ? n : "/" + n
        }), search: function (n, t) {
            switch (arguments.length) {
                case 0:
                    return this.$$search;
                case 1:
                    if (c(n) || k(n)) n = n.toString(), this.$$search = ro(n); else if (h(n)) n = ti(n, {}), r(n, function (t, i) {
                        t == null && delete n[i]
                    }), this.$$search = n; else throw vu("isrcharg", "The first argument of the `$location#search()` call must be a string or an object.");
                    break;
                default:
                    e(t) || t === null ? delete this.$$search[n] : this.$$search[n] = t
            }
            return this.$$compose(), this
        }, hash: ps("$$hash", function (n) {
            return n !== null ? n.toString() : ""
        }), replace: function () {
            return this.$$replace = !0, this
        }
    };
    r([vs, re, ie], function (n) {
        n.prototype = Object.create(ys);
        n.prototype.state = function (t) {
            if (!arguments.length) return this.$$state;
            if (n !== ie || !this.$$html5) throw vu("nostate", "History API state support is available only in HTML5 mode and only in browsers supporting HTML5 History API");
            return this.$$state = e(t) ? null : t, this
        }
    });
    it = v("$parse");
    var yv = Function.prototype.call, pv = Function.prototype.apply, wv = Function.prototype.bind;
    vi = ot();
    r({
        "null": function () {
            return null
        }, "true": function () {
            return !0
        }, "false": function () {
            return !1
        }, undefined: function () {
        }
    }, function (n, t) {
        n.constant = n.literal = n.sharedGetter = !0;
        vi[t] = n
    });
    vi["this"] = function (n) {
        return n
    };
    vi["this"].sharedGetter = !0;
    var cr = a(ot(), {
        "+": function (n, t, r, f) {
            return (r = r(n, t), f = f(n, t), u(r)) ? u(f) ? r + f : r : u(f) ? f : i
        }, "-": function (n, t, i, r) {
            return i = i(n, t), r = r(n, t), (u(i) ? i : 0) - (u(r) ? r : 0)
        }, "*": function (n, t, i, r) {
            return i(n, t) * r(n, t)
        }, "/": function (n, t, i, r) {
            return i(n, t) / r(n, t)
        }, "%": function (n, t, i, r) {
            return i(n, t) % r(n, t)
        }, "===": function (n, t, i, r) {
            return i(n, t) === r(n, t)
        }, "!==": function (n, t, i, r) {
            return i(n, t) !== r(n, t)
        }, "==": function (n, t, i, r) {
            return i(n, t) == r(n, t)
        }, "!=": function (n, t, i, r) {
            return i(n, t) != r(n, t)
        }, "<": function (n, t, i, r) {
            return i(n, t) < r(n, t)
        }, ">": function (n, t, i, r) {
            return i(n, t) > r(n, t)
        }, "<=": function (n, t, i, r) {
            return i(n, t) <= r(n, t)
        }, ">=": function (n, t, i, r) {
            return i(n, t) >= r(n, t)
        }, "&&": function (n, t, i, r) {
            return i(n, t) && r(n, t)
        }, "||": function (n, t, i, r) {
            return i(n, t) || r(n, t)
        }, "!": function (n, t, i) {
            return !i(n, t)
        }, "=": !0, "|": !0
    }), kv = {n: "\n", f: "\f", r: "\r", t: "\t", v: "\v", "'": "'", '"': '"'}, ue = function (n) {
        this.options = n
    };
    ue.prototype = {
        constructor: ue, lex: function (n) {
            var t, r;
            for (this.text = n, this.index = 0, this.tokens = []; this.index < this.text.length;) if (t = this.text.charAt(this.index), t === '"' || t === "'") this.readString(t); else if (this.isNumber(t) || t === "." && this.isNumber(this.peek())) this.readNumber(); else if (this.isIdent(t)) this.readIdent(); else if (this.is(t, "(){}[].,;:?")) this.tokens.push({
                index: this.index,
                text: t
            }), this.index++; else if (this.isWhitespace(t)) this.index++; else {
                var i = t + this.peek(), u = i + this.peek(2), o = cr[t], f = cr[i], e = cr[u];
                o || f || e ? (r = e ? u : f ? i : t, this.tokens.push({
                    index: this.index,
                    text: r,
                    operator: !0
                }), this.index += r.length) : this.throwError("Unexpected next character ", this.index, this.index + 1)
            }
            return this.tokens
        }, is: function (n, t) {
            return t.indexOf(n) !== -1
        }, peek: function (n) {
            var t = n || 1;
            return this.index + t < this.text.length ? this.text.charAt(this.index + t) : !1
        }, isNumber: function (n) {
            return "0" <= n && n <= "9" && typeof n == "string"
        }, isWhitespace: function (n) {
            return n === " " || n === "\r" || n === "\t" || n === "\n" || n === '\v' || n === " "
        }, isIdent: function (n) {
            return "a" <= n && n <= "z" || "A" <= n && n <= "Z" || "_" === n || n === "$"
        }, isExpOperator: function (n) {
            return n === "-" || n === "+" || this.isNumber(n)
        }, throwError: function (n, t, i) {
            i = i || this.index;
            var r = u(t) ? "s " + t + "-" + this.index + " [" + this.text.substring(t, i) + "]" : " " + i;
            throw it("lexerr", "Lexer Error: {0} at column{1} in expression [{2}].", n, r, this.text);
        }, readNumber: function () {
            for (var n = "", r = this.index, t, i; this.index < this.text.length;) {
                if (t = y(this.text.charAt(this.index)), t == "." || this.isNumber(t)) n += t; else if (i = this.peek(), t == "e" && this.isExpOperator(i)) n += t; else if (this.isExpOperator(t) && i && this.isNumber(i) && n.charAt(n.length - 1) == "e") n += t; else if (!this.isExpOperator(t) || i && this.isNumber(i) || n.charAt(n.length - 1) != "e") break; else this.throwError("Invalid exponent");
                this.index++
            }
            this.tokens.push({index: r, text: n, constant: !0, value: Number(n)})
        }, readIdent: function () {
            for (var t = this.index, n; this.index < this.text.length;) {
                if (n = this.text.charAt(this.index), !(this.isIdent(n) || this.isNumber(n))) break;
                this.index++
            }
            this.tokens.push({index: t, text: this.text.slice(t, this.index), identifier: !0})
        }, readString: function (n) {
            var f = this.index, t, r, o;
            this.index++;
            for (var i = "", e = n, u = !1; this.index < this.text.length;) {
                if (t = this.text.charAt(this.index), e += t, u) t === "u" ? (r = this.text.substring(this.index + 1, this.index + 5), r.match(/[\da-f]{4}/i) || this.throwError("Invalid unicode escape [\\u" + r + "]"), this.index += 4, i += String.fromCharCode(parseInt(r, 16))) : (o = kv[t], i = i + (o || t)), u = !1; else if (t === "\\") u = !0; else {
                    if (t === n) {
                        this.index++;
                        this.tokens.push({index: f, text: e, constant: !0, value: i});
                        return
                    }
                    i += t
                }
                this.index++
            }
            this.throwError("Unterminated quote", f)
        }
    };
    yi = function (n, t, i) {
        this.lexer = n;
        this.$filter = t;
        this.options = i
    };
    yi.ZERO = a(function () {
        return 0
    }, {sharedGetter: !0, constant: !0});
    yi.prototype = {
        constructor: yi, parse: function (n) {
            this.text = n;
            this.tokens = this.lexer.lex(n);
            var t = this.statements();
            return this.tokens.length !== 0 && this.throwError("is an unexpected token", this.tokens[0]), t.literal = !!t.literal, t.constant = !!t.constant, t
        }, primary: function () {
            var n, t, i;
            for (this.expect("(") ? (n = this.filterChain(), this.consume(")")) : this.expect("[") ? n = this.arrayDeclaration() : this.expect("{") ? n = this.object() : this.peek().identifier && (this.peek().text in vi) ? n = vi[this.consume().text] : this.peek().identifier ? n = this.identifier() : this.peek().constant ? n = this.constant() : this.throwError("not a primary expression", this.peek()); t = this.expect("(", "[", ".");) t.text === "(" ? (n = this.functionCall(n, i), i = null) : t.text === "[" ? (i = n, n = this.objectIndex(n)) : t.text === "." ? (i = n, n = this.fieldAccess(n)) : this.throwError("IMPOSSIBLE");
            return n
        }, throwError: function (n, t) {
            throw it("syntax", "Syntax Error: Token '{0}' {1} at column {2} of the expression [{3}] starting at [{4}].", t.text, n, t.index + 1, this.text, this.text.substring(t.index));
        }, peekToken: function () {
            if (this.tokens.length === 0) throw it("ueoe", "Unexpected end of expression: {0}", this.text);
            return this.tokens[0]
        }, peek: function (n, t, i, r) {
            return this.peekAhead(0, n, t, i, r)
        }, peekAhead: function (n, t, i, r, u) {
            if (this.tokens.length > n) {
                var e = this.tokens[n], f = e.text;
                if (f === t || f === i || f === r || f === u || !t && !i && !r && !u) return e
            }
            return !1
        }, expect: function (n, t, i, r) {
            var u = this.peek(n, t, i, r);
            return u ? (this.tokens.shift(), u) : !1
        }, consume: function (n) {
            if (this.tokens.length === 0) throw it("ueoe", "Unexpected end of expression: {0}", this.text);
            var t = this.expect(n);
            return t || this.throwError("is unexpected, expecting [" + n + "]", this.peek()), t
        }, unaryFn: function (n, t) {
            var i = cr[n];
            return a(function (n, r) {
                return i(n, r, t)
            }, {constant: t.constant, inputs: [t]})
        }, binaryFn: function (n, t, i, r) {
            var u = cr[t];
            return a(function (t, r) {
                return u(t, r, n, i)
            }, {constant: n.constant && i.constant, inputs: !r && [n, i]})
        }, identifier: function () {
            for (var n = this.consume().text; this.peek(".") && this.peekAhead(1).identifier && !this.peekAhead(2, "(");) n += this.consume().text + this.consume().text;
            return gv(n, this.options, this.text)
        }, constant: function () {
            var n = this.consume().value;
            return a(function () {
                return n
            }, {constant: !0, literal: !0})
        }, statements: function () {
            for (var n = []; ;) if (this.tokens.length > 0 && !this.peek("}", ")", ";", "]") && n.push(this.filterChain()), !this.expect(";")) return n.length === 1 ? n[0] : function (t, i) {
                for (var u, r = 0, f = n.length; r < f; r++) u = n[r](t, i);
                return u
            }
        }, filterChain: function () {
            for (var n = this.expression(), t; t = this.expect("|");) n = this.filter(n);
            return n
        }, filter: function (n) {
            var u = this.$filter(this.consume().text), t, r, f;
            if (this.peek(":")) for (t = [], r = []; this.expect(":");) t.push(this.expression());
            return f = [n].concat(t || []), a(function (f, e) {
                var s = n(f, e), o;
                if (r) {
                    for (r[0] = s, o = t.length; o--;) r[o + 1] = t[o](f, e);
                    return u.apply(i, r)
                }
                return u(s)
            }, {constant: !u.$stateful && f.every(fe), inputs: !u.$stateful && f})
        }, expression: function () {
            return this.assignment()
        }, assignment: function () {
            var n = this.ternary(), t, i;
            return (i = this.expect("=")) ? (n.assign || this.throwError("implies assignment but [" + this.text.substring(0, i.index) + "] can not be assigned to", i), t = this.ternary(), a(function (i, r) {
                return n.assign(i, t(i, r), r)
            }, {inputs: [n, t]})) : n
        }, ternary: function () {
            var n = this.logicalOR(), t, r, i;
            return (r = this.expect("?")) && (t = this.assignment(), this.consume(":")) ? (i = this.assignment(), a(function (r, u) {
                return n(r, u) ? t(r, u) : i(r, u)
            }, {constant: n.constant && t.constant && i.constant})) : n
        }, logicalOR: function () {
            for (var n = this.logicalAND(), t; t = this.expect("||");) n = this.binaryFn(n, t.text, this.logicalAND(), !0);
            return n
        }, logicalAND: function () {
            for (var n = this.equality(), t; t = this.expect("&&");) n = this.binaryFn(n, t.text, this.equality(), !0);
            return n
        }, equality: function () {
            for (var n = this.relational(), t; t = this.expect("==", "!=", "===", "!==");) n = this.binaryFn(n, t.text, this.relational());
            return n
        }, relational: function () {
            for (var n = this.additive(), t; t = this.expect("<", ">", "<=", ">=");) n = this.binaryFn(n, t.text, this.additive());
            return n
        }, additive: function () {
            for (var n = this.multiplicative(), t; t = this.expect("+", "-");) n = this.binaryFn(n, t.text, this.multiplicative());
            return n
        }, multiplicative: function () {
            for (var n = this.unary(), t; t = this.expect("*", "/", "%");) n = this.binaryFn(n, t.text, this.unary());
            return n
        }, unary: function () {
            var n;
            return this.expect("+") ? this.primary() : (n = this.expect("-")) ? this.binaryFn(yi.ZERO, n.text, this.unary()) : (n = this.expect("!")) ? this.unaryFn(n.text, this.unary()) : this.primary()
        }, fieldAccess: function (n) {
            var t = this.identifier();
            return a(function (r, u, f) {
                var e = f || n(r, u);
                return e == null ? i : t(e)
            }, {
                assign: function (i, r, u) {
                    var f = n(i, u);
                    return f || n.assign(i, f = {}, u), t.assign(f, r)
                }
            })
        }, objectIndex: function (n) {
            var t = this.text, r = this.expression();
            return this.consume("]"), a(function (u, f) {
                var e = n(u, f), o = r(u, f);
                return (yt(o, t), !e) ? i : ht(e[o], t)
            }, {
                assign: function (i, u, f) {
                    var o = yt(r(i, f), t), e = ht(n(i, f), t);
                    return e || n.assign(i, e = {}, f), e[o] = u
                }
            })
        }, functionCall: function (n, t) {
            var e = [], f, r;
            if (this.peekToken().text !== ")") do e.push(this.expression()); while (this.expect(","));
            return this.consume(")"), f = this.text, r = e.length ? [] : null, function (o, h) {
                var a = t ? t(o, h) : u(t) ? i : o, c = n(o, h, a) || s, l, v;
                if (r) for (l = e.length; l--;) r[l] = ht(e[l](o, h), f);
                return ht(a, f), bv(c, f), v = c.apply ? c.apply(a, r) : c(r[0], r[1], r[2], r[3], r[4]), ht(v, f)
            }
        }, arrayDeclaration: function () {
            var n = [];
            if (this.peekToken().text !== "]") do {
                if (this.peek("]")) break;
                n.push(this.expression())
            } while (this.expect(","));
            return this.consume("]"), a(function (t, i) {
                for (var u = [], r = 0, f = n.length; r < f; r++) u.push(n[r](t, i));
                return u
            }, {literal: !0, constant: n.every(fe), inputs: n})
        }, object: function () {
            var i = [], t = [], n;
            if (this.peekToken().text !== "}") do {
                if (this.peek("}")) break;
                n = this.consume();
                n.constant ? i.push(n.value) : n.identifier ? i.push(n.text) : this.throwError("invalid key", n);
                this.consume(":");
                t.push(this.expression())
            } while (this.expect(","));
            return this.consume("}"), a(function (n, r) {
                for (var f = {}, u = 0, e = t.length; u < e; u++) f[i[u]] = t[u](n, r);
                return f
            }, {literal: !0, constant: t.every(fe), inputs: t})
        }
    };
    ws = ot();
    bs = ot();
    ds = Object.prototype.valueOf;
    dt = v("$sce");
    rt = {HTML: "html", CSS: "css", URL: "url", RESOURCE_URL: "resourceUrl", JS: "js"};
    tt = v("$compile");
    b = t.createElement("a");
    oe = gt(n.location.href);
    ih.$inject = ["$provide"];
    rh.$inject = ["$locale"];
    uh.$inject = ["$locale"];
    se = ".";
    var dy = {
        yyyy: d("FullYear", 4),
        yy: d("FullYear", 2, 0, !0),
        y: d("FullYear", 1),
        MMMM: wu("Month"),
        MMM: wu("Month", !0),
        MM: d("Month", 2, 1),
        M: d("Month", 1, 1),
        dd: d("Date", 2),
        d: d("Date", 1),
        HH: d("Hours", 2),
        H: d("Hours", 1),
        hh: d("Hours", 2, -12),
        h: d("Hours", 1, -12),
        mm: d("Minutes", 2),
        m: d("Minutes", 1),
        ss: d("Seconds", 2),
        s: d("Seconds", 1),
        sss: d("Milliseconds", 3),
        EEEE: wu("Day"),
        EEE: wu("Day", !0),
        a: ky,
        Z: wy,
        ww: oh(2),
        w: oh(1)
    }, gy = /((?:[^yMdHhmsaZEw']+)|(?:'(?:[^']|'')*')|(?:E+|y+|M+|d+|H+|h+|m+|s+|a|Z|w+))(.*)/, np = /^\-?\d+$/;
    sh.$inject = ["$locale"];
    hh = nt(y);
    ch = nt(bi);
    lh.$inject = ["$parse"];
    ah = nt({
        restrict: "E", compile: function (n, t) {
            if (!t.href && !t.xlinkHref && !t.name) return function (n, t) {
                if (t[0].nodeName.toLowerCase() === "a") {
                    var i = ni.call(t.prop("href")) === "[object SVGAnimatedString]" ? "xlink:href" : "href";
                    t.on("click", function (n) {
                        t.attr(i) || n.preventDefault()
                    })
                }
            }
        }
    });
    ar = {};
    r(sr, function (n, t) {
        if (n != "multiple") {
            var i = bt("ng-" + t);
            ar[i] = function () {
                return {
                    restrict: "A", priority: 100, link: function (n, r, u) {
                        n.$watch(u[i], function (n) {
                            u.$set(t, !!n)
                        })
                    }
                }
            }
        }
    });
    r(yf, function (n, t) {
        ar[t] = function () {
            return {
                priority: 100, link: function (n, i, r) {
                    if (t === "ngPattern" && r.ngPattern.charAt(0) == "/") {
                        var u = r.ngPattern.match(yc);
                        if (u) {
                            r.$set("ngPattern", new RegExp(u[1], u[2]));
                            return
                        }
                    }
                    n.$watch(r[t], function (n) {
                        r.$set(t, n)
                    })
                }
            }
        }
    });
    r(["src", "srcset", "href"], function (n) {
        var t = bt("ng-" + n);
        ar[t] = function () {
            return {
                priority: 99, link: function (i, r, u) {
                    var e = n, f = n;
                    n === "href" && ni.call(r.prop("href")) === "[object SVGAnimatedString]" && (f = "xlinkHref", u.$attr[f] = "xlink:href", e = null);
                    u.$observe(t, function (t) {
                        if (!t) {
                            n === "href" && u.$set(f, null);
                            return
                        }
                        u.$set(f, t);
                        si && e && r.prop(e, u[f])
                    })
                }
            }
        }
    });
    vr = {
        $addControl: s,
        $$renameControl: rp,
        $removeControl: s,
        $setValidity: s,
        $setDirty: s,
        $setPristine: s,
        $setSubmitted: s
    };
    he = "ng-submitted";
    vh.$inject = ["$element", "$attrs", "$scope", "$animate", "$interpolate"];
    var yh = function (n) {
            return ["$timeout", function (t) {
                return {
                    name: "form", restrict: n ? "EAC" : "E", controller: vh, compile: function (n) {
                        return n.addClass(wi).addClass(wr), {
                            pre: function (n, r, u, f) {
                                var o, s, e;
                                if (!("action" in u)) {
                                    o = function (t) {
                                        n.$apply(function () {
                                            f.$commitViewValue();
                                            f.$setSubmitted()
                                        });
                                        t.preventDefault()
                                    };
                                    uu(r[0], "submit", o);
                                    r.on("$destroy", function () {
                                        t(function () {
                                            er(r[0], "submit", o)
                                        }, 0, !1)
                                    })
                                }
                                s = f.$$parentForm;
                                e = f.$name;
                                e && (lr(n, null, e, f, e), u.$observe(u.name ? "name" : "ngForm", function (t) {
                                    e !== t && (lr(n, null, e, i, e), e = t, lr(n, null, e, f, e), s.$$renameControl(f, e))
                                }));
                                r.on("$destroy", function () {
                                    s.$removeControl(f);
                                    e && lr(n, null, e, i, e);
                                    a(f, vr)
                                })
                            }
                        }
                    }
                }
            }]
        }, up = yh(), fp = yh(!0), ep = /\d{4}-[01]\d-[0-3]\dT[0-2]\d:[0-5]\d:[0-5]\d\.\d+([+-][0-2]\d:[0-5]\d|Z)/,
        op = /^(ftp|http|https):\/\/(\w+:{0,1}\w*@)?(\S+)(:[0-9]+)?(\/|\/([\w#!:.?+=&%@!\-\/]))?$/,
        sp = /^[a-z0-9!#$%&'*+\/=?^_`{|}~.-]+@[a-z0-9]([a-z0-9-]*[a-z0-9])?(\.[a-z0-9]([a-z0-9-]*[a-z0-9])?)*$/i,
        hp = /^\s*(\-|\+)?(\d+|(\d*(\.\d*)))\s*$/, ph = /^(\d{4})-(\d{2})-(\d{2})$/,
        wh = /^(\d{4})-(\d\d)-(\d\d)T(\d\d):(\d\d)(?::(\d\d)(\.\d{1,3})?)?$/, ce = /^(\d{4})-W(\d\d)$/,
        bh = /^(\d{4})-(\d\d)$/, kh = /^(\d\d):(\d\d)(?::(\d\d)(\.\d{1,3})?)?$/, dh = {
            text: cp,
            date: pr("date", ph, bu(ph, ["yyyy", "MM", "dd"]), "yyyy-MM-dd"),
            "datetime-local": pr("datetimelocal", wh, bu(wh, ["yyyy", "MM", "dd", "HH", "mm", "ss", "sss"]), "yyyy-MM-ddTHH:mm:ss.sss"),
            time: pr("time", kh, bu(kh, ["HH", "mm", "ss", "sss"]), "HH:mm:ss.sss"),
            week: pr("week", ce, lp, "yyyy-Www"),
            month: pr("month", bh, bu(bh, ["yyyy", "MM"]), "yyyy-MM"),
            number: ap,
            url: vp,
            email: yp,
            radio: pp,
            checkbox: wp,
            hidden: s,
            button: s,
            submit: s,
            reset: s,
            file: s
        };
    var tc = ["$browser", "$sniffer", "$filter", "$parse", function (n, t, i, r) {
        return {
            restrict: "E", require: ["?ngModel"], link: {
                pre: function (u, f, e, o) {
                    o[0] && (dh[y(e.type)] || dh.text)(u, f, e, o[0], t, n, i, r)
                }
            }
        }
    }], bp = /^(true|false|\d+)$/, kp = function () {
        return {
            restrict: "A", priority: 100, compile: function (n, t) {
                return bp.test(t.ngValue) ? function (n, t, i) {
                    i.$set("value", n.$eval(i.ngValue))
                } : function (n, t, i) {
                    n.$watch(i.ngValue, function (n) {
                        i.$set("value", n)
                    })
                }
            }
        }
    }, dp = ["$compile", function (n) {
        return {
            restrict: "AC", compile: function (t) {
                return n.$$addBindingClass(t), function (t, r, u) {
                    n.$$addBindingInfo(r, u.ngBind);
                    r = r[0];
                    t.$watch(u.ngBind, function (n) {
                        r.textContent = n === i ? "" : n
                    })
                }
            }
        }
    }], gp = ["$interpolate", "$compile", function (n, t) {
        return {
            compile: function (r) {
                return t.$$addBindingClass(r), function (r, u, f) {
                    var e = n(u.attr(f.$attr.ngBindTemplate));
                    t.$$addBindingInfo(u, e.expressions);
                    u = u[0];
                    f.$observe("ngBindTemplate", function (n) {
                        u.textContent = n === i ? "" : n
                    })
                }
            }
        }
    }], nw = ["$sce", "$parse", "$compile", function (n, t, i) {
        return {
            restrict: "A", compile: function (r, u) {
                var f = t(u.ngBindHtml), e = t(u.ngBindHtml, function (n) {
                    return (n || "").toString()
                });
                return i.$$addBindingClass(r), function (t, r, u) {
                    i.$$addBindingInfo(r, u.ngBindHtml);
                    t.$watch(e, function () {
                        r.html(n.getTrustedHtml(f(t)) || "")
                    })
                }
            }
        }
    }], tw = nt({
        restrict: "A", require: "ngModel", link: function (n, t, i, r) {
            r.$viewChangeListeners.push(function () {
                n.$eval(i.ngChange)
            })
        }
    });
    var iw = ae("", !0), rw = ae("Odd", 0), uw = ae("Even", 1), fw = oi({
        compile: function (n, t) {
            t.$set("ngCloak", i);
            n.removeClass("ng-cloak")
        }
    }), ew = [function () {
        return {restrict: "A", scope: !0, controller: "@", priority: 500}
    }], ic = {}, ow = {blur: !0, focus: !0};
    r("click dblclick mousedown mouseup mouseover mouseout mousemove mouseenter mouseleave keydown keyup keypress submit focus blur copy cut paste".split(" "), function (n) {
        var t = bt("ng-" + n);
        ic[t] = ["$parse", "$rootScope", function (i, r) {
            return {
                restrict: "A", compile: function (u, f) {
                    var e = i(f[t], null, !0);
                    return function (t, i) {
                        i.on(n, function (i) {
                            var u = function () {
                                e(t, {$event: i})
                            };
                            ow[n] && r.$$phase ? t.$evalAsync(u) : t.$apply(u)
                        })
                    }
                }
            }
        }]
    });
    var sw = ["$animate", function (n) {
            return {
                multiElement: !0,
                transclude: "element",
                priority: 600,
                terminal: !0,
                restrict: "A",
                $$tlb: !0,
                link: function (i, r, u, f, e) {
                    var h, s, o;
                    i.$watch(u.ngIf, function (i) {
                        i ? s || e(function (i, f) {
                            s = f;
                            i[i.length++] = t.createComment(" end ngIf: " + u.ngIf + " ");
                            h = {clone: i};
                            n.enter(i, r.parent(), r)
                        }) : (o && (o.remove(), o = null), s && (s.$destroy(), s = null), h && (o = tu(h.clone), n.leave(o).then(function () {
                            o = null
                        }), h = null))
                    })
                }
            }
        }], hw = ["$templateRequest", "$anchorScroll", "$animate", "$sce", function (n, t, i, r) {
            return {
                restrict: "ECA",
                priority: 400,
                terminal: !0,
                transclude: "element",
                controller: ft.noop,
                compile: function (f, e) {
                    var s = e.ngInclude || e.src, h = e.onload || "", o = e.autoscroll;
                    return function (f, e, c, l, a) {
                        var w = 0, v, y, p, b = function () {
                            y && (y.remove(), y = null);
                            v && (v.$destroy(), v = null);
                            p && (i.leave(p).then(function () {
                                y = null
                            }), y = p, p = null)
                        };
                        f.$watch(r.parseAsResourceUrl(s), function (r) {
                            var c = function () {
                                u(o) && (!o || f.$eval(o)) && t()
                            }, s = ++w;
                            r ? (n(r, !0).then(function (n) {
                                var t, u;
                                s === w && (t = f.$new(), l.template = n, u = a(t, function (n) {
                                    b();
                                    i.enter(n, null, e).then(c)
                                }), v = t, p = u, v.$emit("$includeContentLoaded", r), f.$eval(h))
                            }, function () {
                                s === w && (b(), f.$emit("$includeContentError", r))
                            }), f.$emit("$includeContentRequested", r)) : (b(), l.template = null)
                        })
                    }
                }
            }
        }], cw = ["$compile", function (n) {
            return {
                restrict: "ECA", priority: -400, require: "ngInclude", link: function (i, r, u, f) {
                    if (/SVG/.test(r[0].toString())) {
                        r.empty();
                        n(vo(f.template, t).childNodes)(i, function (n) {
                            r.append(n)
                        }, {futureParentElement: r});
                        return
                    }
                    r.html(f.template);
                    n(r.contents())(i)
                }
            }
        }], lw = oi({
            priority: 450, compile: function () {
                return {
                    pre: function (n, t, i) {
                        n.$eval(i.ngInit)
                    }
                }
            }
        }), aw = function () {
            return {
                restrict: "A", priority: 100, require: "ngModel", link: function (n, t, u, f) {
                    var s = t.attr(u.$attr.ngList) || ", ", h = u.ngTrim !== "false", c = h ? p(s) : s, l = function (n) {
                        if (!e(n)) {
                            var t = [];
                            return n && r(n.split(c), function (n) {
                                n && t.push(h ? p(n) : n)
                            }), t
                        }
                    };
                    f.$parsers.push(l);
                    f.$formatters.push(function (n) {
                        return o(n) ? n.join(s) : i
                    });
                    f.$isEmpty = function (n) {
                        return !n || !n.length
                    }
                }
            }
        }, wr = "ng-valid", rc = "ng-invalid", wi = "ng-pristine", ku = "ng-dirty", ve = "ng-untouched", uc = "ng-touched",
        fc = "ng-pending", du = new v("ngModel"),
        vw = ["$scope", "$exceptionHandler", "$attrs", "$element", "$parse", "$animate", "$timeout", "$rootScope", "$q", "$interpolate", function (n, t, f, o, h, c, a, v, y, p) {
            var tt, d;
            this.$viewValue = Number.NaN;
            this.$modelValue = Number.NaN;
            this.$$rawModelValue = i;
            this.$validators = {};
            this.$asyncValidators = {};
            this.$parsers = [];
            this.$formatters = [];
            this.$viewChangeListeners = [];
            this.$untouched = !0;
            this.$touched = !1;
            this.$pristine = !0;
            this.$dirty = !1;
            this.$valid = !0;
            this.$invalid = !1;
            this.$error = {};
            this.$$success = {};
            this.$pending = i;
            this.$name = p(f.name || "", !1)(n);
            var b = h(f.ngModel), it = b.assign, nt = b, rt = it, g = null, w = this;
            this.$$setOptions = function (n) {
                if (w.$options = n, n && n.getterSetter) {
                    var t = h(f.ngModel + "()"), i = h(f.ngModel + "($$$p)");
                    nt = function (n) {
                        var i = b(n);
                        return l(i) && (i = t(n)), i
                    };
                    rt = function (n) {
                        l(b(n)) ? i(n, {$$$p: w.$modelValue}) : it(n, w.$modelValue)
                    }
                } else if (!b.assign) throw du("nonassign", "Expression '{0}' is non-assignable. Element: {1}", f.ngModel, wt(o));
            };
            this.$render = s;
            this.$isEmpty = function (n) {
                return e(n) || n === "" || n === null || n !== n
            };
            tt = o.inheritedData("$formController") || vr;
            d = 0;
            ec({
                ctrl: this, $element: o, set: function (n, t) {
                    n[t] = !0
                }, unset: function (n, t) {
                    delete n[t]
                }, parentForm: tt, $animate: c
            });
            this.$setPristine = function () {
                w.$dirty = !1;
                w.$pristine = !0;
                c.removeClass(o, ku);
                c.addClass(o, wi)
            };
            this.$setDirty = function () {
                w.$dirty = !0;
                w.$pristine = !1;
                c.removeClass(o, wi);
                c.addClass(o, ku);
                tt.$setDirty()
            };
            this.$setUntouched = function () {
                w.$touched = !1;
                w.$untouched = !0;
                c.setClass(o, ve, uc)
            };
            this.$setTouched = function () {
                w.$touched = !0;
                w.$untouched = !1;
                c.setClass(o, uc, ve)
            };
            this.$rollbackViewValue = function () {
                a.cancel(g);
                w.$viewValue = w.$$lastCommittedViewValue;
                w.$render()
            };
            this.$validate = function () {
                if (!k(w.$modelValue) || !isNaN(w.$modelValue)) {
                    var t = w.$$lastCommittedViewValue, n = w.$$rawModelValue, r = w.$$parserName || "parse",
                        u = w.$error[r] ? !1 : i, f = w.$valid, e = w.$modelValue,
                        o = w.$options && w.$options.allowInvalid;
                    w.$$runValidators(u, n, t, function (t) {
                        o || f === t || (w.$modelValue = t ? n : i, w.$modelValue !== e && w.$$writeModelToScope())
                    })
                }
            };
            this.$$runValidators = function (n, t, u, f) {
                function c(n) {
                    var t = w.$$parserName || "parse";
                    if (n === i) e(t, null); else if (e(t, n), !n) return r(w.$validators, function (n, t) {
                        e(t, null)
                    }), r(w.$asyncValidators, function (n, t) {
                        e(t, null)
                    }), !1;
                    return !0
                }

                function l() {
                    var n = !0;
                    return (r(w.$validators, function (i, r) {
                        var f = i(t, u);
                        n = n && f;
                        e(r, f)
                    }), !n) ? (r(w.$asyncValidators, function (n, t) {
                        e(t, null)
                    }), !1) : !0
                }

                function a() {
                    var n = [], f = !0;
                    r(w.$asyncValidators, function (r, o) {
                        var s = r(t, u);
                        if (!dr(s)) throw du("$asyncValidators", "Expected asynchronous validator to return a promise but got '{0}' instead.", s);
                        e(o, i);
                        n.push(s.then(function () {
                            e(o, !0)
                        }, function () {
                            f = !1;
                            e(o, !1)
                        }))
                    });
                    n.length ? y.all(n).then(function () {
                        o(f)
                    }, s) : o(!0)
                }

                function e(n, t) {
                    h === d && w.$setValidity(n, t)
                }

                function o(n) {
                    h === d && f(n)
                }

                d++;
                var h = d;
                if (!c(n)) {
                    o(!1);
                    return
                }
                if (!l()) {
                    o(!1);
                    return
                }
                a()
            };
            this.$commitViewValue = function () {
                var n = w.$viewValue;
                (a.cancel(g), w.$$lastCommittedViewValue !== n || n === "" && w.$$hasNativeValidators) && (w.$$lastCommittedViewValue = n, w.$pristine && this.$setDirty(), this.$$parseAndValidate())
            };
            this.$$parseAndValidate = function () {
                function s() {
                    w.$modelValue !== o && w.$$writeModelToScope()
                }

                var h = w.$$lastCommittedViewValue, t = h, u = e(t) ? i : !0, r, o, f;
                if (u) for (r = 0; r < w.$parsers.length; r++) if (t = w.$parsers[r](t), e(t)) {
                    u = !1;
                    break
                }
                k(w.$modelValue) && isNaN(w.$modelValue) && (w.$modelValue = nt(n));
                o = w.$modelValue;
                f = w.$options && w.$options.allowInvalid;
                w.$$rawModelValue = t;
                f && (w.$modelValue = t, s());
                w.$$runValidators(u, t, w.$$lastCommittedViewValue, function (n) {
                    f || (w.$modelValue = n ? t : i, s())
                })
            };
            this.$$writeModelToScope = function () {
                rt(n, w.$modelValue);
                r(w.$viewChangeListeners, function (n) {
                    try {
                        n()
                    } catch (i) {
                        t(i)
                    }
                })
            };
            this.$setViewValue = function (n, t) {
                w.$viewValue = n;
                (!w.$options || w.$options.updateOnDefault) && w.$$debounceViewValueCommit(t)
            };
            this.$$debounceViewValueCommit = function (t) {
                var r = 0, f = w.$options, i;
                f && u(f.debounce) && (i = f.debounce, k(i) ? r = i : k(i[t]) ? r = i[t] : k(i["default"]) && (r = i["default"]));
                a.cancel(g);
                r ? g = a(function () {
                    w.$commitViewValue()
                }, r) : v.$$phase ? w.$commitViewValue() : n.$apply(function () {
                    w.$commitViewValue()
                })
            };
            n.$watch(function () {
                var t = nt(n);
                if (t !== w.$modelValue) {
                    w.$modelValue = w.$$rawModelValue = t;
                    for (var u = w.$formatters, f = u.length, r = t; f--;) r = u[f](r);
                    w.$viewValue !== r && (w.$viewValue = w.$$lastCommittedViewValue = r, w.$render(), w.$$runValidators(i, t, r, s))
                }
                return t
            })
        }], yw = ["$rootScope", function (n) {
            return {
                restrict: "A",
                require: ["ngModel", "^?form", "^?ngModelOptions"],
                controller: vw,
                priority: 1,
                compile: function (t) {
                    return t.addClass(wi).addClass(ve).addClass(wr), {
                        pre: function (n, t, i, r) {
                            var u = r[0], f = r[1] || vr;
                            u.$$setOptions(r[2] && r[2].$options);
                            f.$addControl(u);
                            i.$observe("name", function (n) {
                                u.$name !== n && f.$$renameControl(u, n)
                            });
                            n.$on("$destroy", function () {
                                f.$removeControl(u)
                            })
                        }, post: function (t, i, r, u) {
                            var f = u[0];
                            if (f.$options && f.$options.updateOn) i.on(f.$options.updateOn, function (n) {
                                f.$$debounceViewValueCommit(n && n.type)
                            });
                            i.on("blur", function () {
                                f.$touched || (n.$$phase ? t.$evalAsync(f.$setTouched) : t.$apply(f.$setTouched))
                            })
                        }
                    }
                }
            }
        }], pw = /(\s+|^)default(\s+|$)/, ww = function () {
            return {
                restrict: "A", controller: ["$scope", "$attrs", function (n, t) {
                    var r = this;
                    this.$options = n.$eval(t.ngModelOptions);
                    this.$options.updateOn !== i ? (this.$options.updateOnDefault = !1, this.$options.updateOn = p(this.$options.updateOn.replace(pw, function () {
                        return r.$options.updateOnDefault = !0, " "
                    }))) : this.$options.updateOnDefault = !0
                }]
            }
        };
    var bw = oi({terminal: !0, priority: 1e3}), kw = ["$locale", "$interpolate", function (n, t) {
        var i = /{}/g, u = /^when(Minus)?(.+)$/;
        return {
            restrict: "EA", link: function (f, e, o) {
                function d(n) {
                    e.text(n || "")
                }

                var c = o.count, p = o.$attr.when && e.attr(o.$attr.when), l = o.offset || 0, s = f.$eval(p) || {},
                    a = {}, w = t.startSymbol(), b = t.endSymbol(), k = w + c + "-" + l + b, v = ft.noop, h;
                r(o, function (n, t) {
                    var i = u.exec(t), r;
                    i && (r = (i[1] ? "-" : "") + y(i[2]), s[r] = e.attr(o.$attr[t]))
                });
                r(s, function (n, r) {
                    a[r] = t(n.replace(i, k))
                });
                f.$watch(c, function (t) {
                    var i = parseFloat(t), r = isNaN(i);
                    r || i in s || (i = n.pluralCat(i - l));
                    i === h || r && isNaN(h) || (v(), v = f.$watch(a[i], d), h = i)
                })
            }
        }
    }], dw = ["$parse", "$animate", function (n, u) {
        var o = "$$NG_REMOVED", e = v("ngRepeat"), s = function (n, t, i, r, u, f, e) {
            n[i] = r;
            u && (n[u] = f);
            n.$index = t;
            n.$first = t === 0;
            n.$last = t === e - 1;
            n.$middle = !(n.$first || n.$last);
            n.$odd = !(n.$even = (t & 1) == 0)
        }, h = function (n) {
            return n.clone[0]
        }, c = function (n) {
            return n.clone[n.clone.length - 1]
        };
        return {
            restrict: "A",
            multiElement: !0,
            transclude: "element",
            priority: 1e3,
            terminal: !0,
            $$tlb: !0,
            compile: function (l, a) {
                var b = a.ngRepeat, ut = t.createComment(" end ngRepeat: " + b + " "),
                    v = b.match(/^\s*([\s\S]+?)\s+in\s+([\s\S]+?)(?:\s+as\s+([\s\S]+?))?(?:\s+track\s+by\s+([\s\S]+?))?\s*$/),
                    k, p, d, g, it, rt, w;
                if (!v) throw e("iexp", "Expected expression in form of '_item_ in _collection_[ track by _id_]' but got '{0}'.", b);
                var nt = v[1], ft = v[2], y = v[3], tt = v[4];
                if (v = nt.match(/^(?:(\s*[\$\w]+)|\(\s*([\$\w]+)\s*,\s*([\$\w]+)\s*\))$/), !v) throw e("iidexp", "'_item_' in '_item_ in _collection_' should be an identifier or '(_key_, _value_)' expression, but got '{0}'.", nt);
                if (k = v[3] || v[1], p = v[2], y && (!/^[$a-zA-Z_][$a-zA-Z0-9_]*$/.test(y) || /^(null|undefined|this|\$index|\$first|\$middle|\$last|\$even|\$odd|\$parent|\$root|\$id)$/.test(y))) throw e("badident", "alias '{0}' is invalid --- must be a valid JS identifier which is not a reserved name.", y);
                return w = {$id: ai}, tt ? d = n(tt) : (it = function (n, t) {
                    return ai(t)
                }, rt = function (n) {
                    return n
                }), function (n, t, l, a, v) {
                    d && (g = function (t, i, r) {
                        return p && (w[p] = t), w[k] = i, w.$index = r, d(n, w)
                    });
                    var nt = ot();
                    n.$watchCollection(ft, function (l) {
                        var a, wt, ct = t[0], et, lt = ot(), st, ft, ht, d, pt, tt, w, at, vt, yt, bt;
                        if (y && (n[y] = l), di(l)) tt = l, pt = g || it; else {
                            pt = g || rt;
                            tt = [];
                            for (yt in l) l.hasOwnProperty(yt) && yt.charAt(0) != "$" && tt.push(yt);
                            tt.sort()
                        }
                        for (st = tt.length, at = new Array(st), a = 0; a < st; a++) if (ft = l === tt ? a : tt[a], ht = l[ft], d = pt(ft, ht, a), nt[d]) w = nt[d], delete nt[d], lt[d] = w, at[a] = w; else if (lt[d]) {
                            r(at, function (n) {
                                n && n.scope && (nt[n.id] = n)
                            });
                            throw e("dupes", "Duplicates in a repeater are not allowed. Use 'track by' expression to specify unique keys. Repeater: {0}, Duplicate key: {1}, Duplicate value: {2}", b, d, ht);
                        } else at[a] = {id: d, scope: i, clone: i}, lt[d] = !0;
                        for (bt in nt) {
                            if (w = nt[bt], vt = tu(w.clone), u.leave(vt), vt[0].parentNode) for (a = 0, wt = vt.length; a < wt; a++) vt[a][o] = !0;
                            w.scope.$destroy()
                        }
                        for (a = 0; a < st; a++) if (ft = l === tt ? a : tt[a], ht = l[ft], w = at[a], w.scope) {
                            et = ct;
                            do et = et.nextSibling; while (et && et[o]);
                            h(w) != et && u.move(tu(w.clone), null, f(ct));
                            ct = c(w);
                            s(w.scope, a, k, ht, p, ft, st)
                        } else v(function (n, t) {
                            w.scope = t;
                            var i = ut.cloneNode(!1);
                            n[n.length++] = i;
                            u.enter(n, null, f(ct));
                            ct = i;
                            w.clone = n;
                            lt[w.id] = w;
                            s(w.scope, a, k, ht, p, ft, st)
                        });
                        nt = lt
                    })
                }
            }
        }
    }], sc = "ng-hide", hc = "ng-hide-animate", gw = ["$animate", function (n) {
        return {
            restrict: "A", multiElement: !0, link: function (t, i, r) {
                t.$watch(r.ngShow, function (t) {
                    n[t ? "removeClass" : "addClass"](i, sc, {tempClasses: hc})
                })
            }
        }
    }], nb = ["$animate", function (n) {
        return {
            restrict: "A", multiElement: !0, link: function (t, i, r) {
                t.$watch(r.ngHide, function (t) {
                    n[t ? "addClass" : "removeClass"](i, sc, {tempClasses: hc})
                })
            }
        }
    }], tb = oi(function (n, t, i) {
        n.$watchCollection(i.ngStyle, function (n, i) {
            i && n !== i && r(i, function (n, i) {
                t.css(i, "")
            });
            n && t.css(n)
        })
    }), ib = ["$animate", function (n) {
        return {
            restrict: "EA", require: "ngSwitch", controller: ["$scope", function () {
                this.cases = {}
            }], link: function (i, u, f, e) {
                var l = f.ngSwitch || f.on, c = [], h = [], o = [], s = [], a = function (n, t) {
                    return function () {
                        n.splice(t, 1)
                    }
                };
                i.$watch(l, function (i) {
                    for (var l, v, u = 0, f = o.length; u < f; ++u) n.cancel(o[u]);
                    for (o.length = 0, u = 0, f = s.length; u < f; ++u) l = tu(h[u].clone), s[u].$destroy(), v = o[u] = n.leave(l), v.then(a(o, u));
                    h.length = 0;
                    s.length = 0;
                    (c = e.cases["!" + i] || e.cases["?"]) && r(c, function (i) {
                        i.transclude(function (r, u) {
                            var f, e;
                            s.push(u);
                            f = i.element;
                            r[r.length++] = t.createComment(" end ngSwitchWhen: ");
                            e = {clone: r};
                            h.push(e);
                            n.enter(r, f.parent(), f)
                        })
                    })
                })
            }
        }
    }], rb = oi({
        transclude: "element",
        priority: 1200,
        require: "^ngSwitch",
        multiElement: !0,
        link: function (n, t, i, r, u) {
            r.cases["!" + i.ngSwitchWhen] = r.cases["!" + i.ngSwitchWhen] || [];
            r.cases["!" + i.ngSwitchWhen].push({transclude: u, element: t})
        }
    }), ub = oi({
        transclude: "element",
        priority: 1200,
        require: "^ngSwitch",
        multiElement: !0,
        link: function (n, t, i, r, u) {
            r.cases["?"] = r.cases["?"] || [];
            r.cases["?"].push({transclude: u, element: t})
        }
    }), fb = oi({
        restrict: "EAC", link: function (n, t, i, r, u) {
            if (!u) throw v("ngTransclude")("orphan", "Illegal use of ngTransclude directive in the template! No parent directive that requires a transclusion found. Element: {0}", wt(t));
            u(function (n) {
                t.empty();
                t.append(n)
            })
        }
    }), eb = ["$templateCache", function (n) {
        return {
            restrict: "E", terminal: !0, compile: function (t, i) {
                if (i.type == "text/ng-template") {
                    var r = i.id, u = t[0].text;
                    n.put(r, u)
                }
            }
        }
    }], ob = v("ngOptions"), sb = nt({restrict: "A", terminal: !0}), hb = ["$compile", "$parse", function (n, h) {
        var c = /^\s*([\s\S]+?)(?:\s+as\s+([\s\S]+?))?(?:\s+group\s+by\s+([\s\S]+?))?\s+for\s+(?:([\$\w][\$\w]*)|(?:\(\s*([\$\w][\$\w]*)\s*,\s*([\$\w][\$\w]*)\s*\)))\s+in\s+([\s\S]+?)(?:\s+track\s+by\s+([\s\S]+?))?$/,
            l = {$setViewValue: s};
        return {
            restrict: "E",
            require: ["select", "?ngModel"],
            controller: ["$element", "$scope", "$attrs", function (n, t, i) {
                var r = this, f = {}, e = l, o, u;
                r.databound = i.ngModel;
                r.init = function (n, t, i) {
                    e = n;
                    o = t;
                    u = i
                };
                r.addOption = function (t, i) {
                    li(t, '"option value"');
                    f[t] = !0;
                    e.$viewValue == t && (n.val(t), u.parent() && u.remove());
                    i && i[0].hasAttribute("selected") && (i[0].selected = !0)
                };
                r.removeOption = function (n) {
                    this.hasOption(n) && (delete f[n], e.$viewValue === n && this.renderUnknownOption(n))
                };
                r.renderUnknownOption = function (t) {
                    var i = "? " + ai(t) + " ?";
                    u.val(i);
                    n.prepend(u);
                    n.val(i);
                    u.prop("selected", !0)
                };
                r.hasOption = function (n) {
                    return f.hasOwnProperty(n)
                };
                t.$on("$destroy", function () {
                    r.renderUnknownOption = s
                })
            }],
            link: function (s, l, a, v) {
                function ot(n, t, i, r) {
                    i.$render = function () {
                        var n = i.$viewValue;
                        r.hasOption(n) ? (b.parent() && b.remove(), t.val(n), n === "" && nt.prop("selected", !0)) : e(n) && nt ? t.val("") : r.renderUnknownOption(n)
                    };
                    t.on("change", function () {
                        n.$apply(function () {
                            b.parent() && b.remove();
                            i.$setViewValue(t.val())
                        })
                    })
                }

                function st(n, t, i) {
                    var f;
                    i.$render = function () {
                        var n = new hr(i.$viewValue);
                        r(t.find("option"), function (t) {
                            t.selected = u(n.get(t.value))
                        })
                    };
                    n.$watch(function () {
                        et(f, i.$viewValue) || (f = at(i.$viewValue), i.$render())
                    });
                    t.on("change", function () {
                        n.$apply(function () {
                            var n = [];
                            r(t.find("option"), function (t) {
                                t.selected && n.push(t.value)
                            });
                            i.$setViewValue(n)
                        })
                    })
                }

                function ht(t, f, e) {
                    function a(n, i, r) {
                        return ft[ht] = r, v && (ft[v] = i), n(t, ft)
                    }

                    function pt() {
                        t.$apply(function () {
                            var u = nt(t) || [], n, i;
                            p ? (n = [], r(f.val(), function (t) {
                                t = l ? it[t] : t;
                                n.push(at(t, u[t]))
                            })) : (i = l ? it[f.val()] : f.val(), n = at(i, u[i]));
                            e.$setViewValue(n);
                            st()
                        })
                    }

                    function at(n, t) {
                        if (n === "?") return i;
                        if (n === "") return null;
                        var r = d ? d : lt;
                        return a(r, n, t)
                    }

                    function bt() {
                        var n = nt(t), i, r, f, u;
                        if (n && o(n)) {
                            for (i = new Array(n.length), r = 0, f = n.length; r < f; r++) i[r] = a(et, r, n[r]);
                            return i
                        }
                        if (n) {
                            i = {};
                            for (u in n) n.hasOwnProperty(u) && (i[u] = a(et, u, n[u]))
                        }
                        return i
                    }

                    function kt(n) {
                        var t, i;
                        if (p) if (l && o(n)) for (t = new hr([]), i = 0; i < n.length; i++) t.put(a(l, null, n[i]), !0); else t = new hr(n); else l && (n = a(l, null, n));
                        return function (i, r) {
                            var f;
                            return f = l ? l : d ? d : lt, p ? u(t.remove(a(f, i, r))) : n === a(f, i, r)
                        }
                    }

                    function ot() {
                        tt || (t.$$postDigest(st), tt = !0)
                    }

                    function b(n, t, i) {
                        n[t] = n[t] || 0;
                        n[t] += i ? 1 : -1
                    }

                    function st() {
                        tt = !1;
                        var lt = {"": []}, bt = [""], c, o, n, d, g, s, ii = e.$viewValue, dt = nt(t) || [],
                            gt = v ? pe(dt) : dt, ot, yt, ri, pt, st, i, ht = {}, ni, ui = kt(ii), wt = !1, h, ct, at,
                            ti;
                        for (it = {}, i = 0; pt = gt.length, i < pt; i++) (ot = i, v && (ot = gt[i], ot.charAt(0) === "$")) || (yt = dt[ot], c = a(vt, ot, yt) || "", (o = lt[c]) || (o = lt[c] = [], bt.push(c)), ni = ui(ot, yt), wt = wt || ni, at = a(et, ot, yt), at = u(at) ? at : "", ti = l ? l(t, ft) : v ? gt[i] : i, l && (it[ti] = ot), o.push({
                            id: ti,
                            label: at,
                            selected: ni
                        }));
                        for (p || (y || ii === null ? lt[""].unshift({
                            id: "",
                            label: "",
                            selected: !wt
                        }) : wt || lt[""].unshift({
                            id: "?",
                            label: "",
                            selected: !0
                        })), st = 0, ri = bt.length; st < ri; st++) {
                            for (c = bt[st], o = lt[c], w.length <= st ? (d = {
                                element: ut.clone().attr("label", c),
                                label: o.label
                            }, g = [d], w.push(g), f.append(d.element)) : (g = w[st], d = g[0], d.label != c && d.element.attr("label", d.label = c)), h = null, i = 0, pt = o.length; i < pt; i++) n = o[i], (s = g[i + 1]) ? (h = s.element, s.label !== n.label && (b(ht, s.label, !1), b(ht, n.label, !0), h.text(s.label = n.label), h.prop("label", s.label)), s.id !== n.id && h.val(s.id = n.id), h[0].selected !== n.selected && (h.prop("selected", s.selected = n.selected), si && h.prop("selected", s.selected))) : (n.id === "" && y ? ct = y : (ct = rt.clone()).val(n.id).prop("selected", n.selected).attr("selected", n.selected).prop("label", n.label).text(n.label), g.push(s = {
                                element: ct,
                                label: n.label,
                                id: n.id,
                                selected: n.selected
                            }), b(ht, n.label, !0), h ? h.after(ct) : d.element.append(ct), h = ct);
                            for (i++; g.length > i;) n = g.pop(), b(ht, n.label, !1), n.element.remove()
                        }
                        while (w.length > st) {
                            for (o = w.pop(), i = 1; i < o.length; ++i) b(ht, o[i].label, !1);
                            o[0].element.remove()
                        }
                        r(ht, function (n, t) {
                            n > 0 ? k.addOption(t) : n < 0 && k.removeOption(t)
                        })
                    }

                    var s;
                    if (!(s = g.match(c))) throw ob("iexp", "Expected expression in form of '_select_ (as _label_)? for (_key_,)?_value_ in _collection_' but got '{0}'. Element: {1}", g, wt(f));
                    var et = h(s[2] || s[1]), ht = s[4] || s[6], ct = / as /.test(s[0]) && s[1], d = ct ? h(ct) : null,
                        v = s[5], vt = h(s[3] || ""), lt = h(s[2] ? s[1] : ht), nt = h(s[7]), yt = s[8],
                        l = yt ? h(s[8]) : null, it = {}, w = [[{element: f, label: ""}]], ft = {};
                    y && (n(y)(t), y.removeClass("ng-scope"), y.remove());
                    f.empty();
                    f.on("change", pt);
                    e.$render = st;
                    t.$watchCollection(nt, ot);
                    t.$watchCollection(bt, ot);
                    p && t.$watchCollection(function () {
                        return e.$modelValue
                    }, ot)
                }

                if (v[1]) {
                    for (var k = v[0], w = v[1], p = a.multiple, g = a.ngOptions, y = !1, nt, tt = !1, rt = f(t.createElement("option")), ut = f(t.createElement("optgroup")), b = rt.clone(), d = 0, it = l.children(), ft = it.length; d < ft; d++) if (it[d].value === "") {
                        nt = y = it.eq(d);
                        break
                    }
                    k.init(w, y, b);
                    p && (w.$isEmpty = function (n) {
                        return !n || n.length === 0
                    });
                    g ? ht(s, l, w) : p ? st(s, l, w) : ot(s, l, w, k)
                }
            }
        }
    }], cb = ["$interpolate", function (n) {
        var t = {addOption: s, removeOption: s};
        return {
            restrict: "E", priority: 100, compile: function (i, r) {
                if (e(r.value)) {
                    var u = n(i.text(), !0);
                    u || r.$set("value", i.text())
                }
                return function (n, i, r) {
                    var e = "$selectController", o = i.parent(), f = o.data(e) || o.parent().data(e);
                    f && f.databound || (f = t);
                    u ? n.$watch(u, function (n, t) {
                        r.$set("value", n);
                        t !== n && f.removeOption(t);
                        f.addOption(n, i)
                    }) : f.addOption(r.value, i);
                    i.on("$destroy", function () {
                        f.removeOption(r.value)
                    })
                }
            }
        }
    }], lb = nt({restrict: "E", terminal: !1}), cc = function () {
        return {
            restrict: "A", require: "?ngModel", link: function (n, t, i, r) {
                r && (i.required = !0, r.$validators.required = function (n, t) {
                    return !i.required || !r.$isEmpty(t)
                }, i.$observe("required", function () {
                    r.$validate()
                }))
            }
        }
    }, lc = function () {
        return {
            restrict: "A", require: "?ngModel", link: function (n, t, r, u) {
                if (u) {
                    var f, o = r.ngPattern || r.pattern;
                    r.$observe("pattern", function (n) {
                        if (c(n) && n.length > 0 && (n = new RegExp("^" + n + "$")), n && !n.test) throw v("ngPattern")("noregexp", "Expected {0} to be a RegExp but was {1}. Element: {2}", o, n, wt(t));
                        f = n || i;
                        u.$validate()
                    });
                    u.$validators.pattern = function (n) {
                        return u.$isEmpty(n) || e(f) || f.test(n)
                    }
                }
            }
        }
    }, ac = function () {
        return {
            restrict: "A", require: "?ngModel", link: function (n, t, i, r) {
                if (r) {
                    var u = -1;
                    i.$observe("maxlength", function (n) {
                        var t = g(n);
                        u = isNaN(t) ? -1 : t;
                        r.$validate()
                    });
                    r.$validators.maxlength = function (n, t) {
                        return u < 0 || r.$isEmpty(n) || t.length <= u
                    }
                }
            }
        }
    }, vc = function () {
        return {
            restrict: "A", require: "?ngModel", link: function (n, t, i, r) {
                if (r) {
                    var u = 0;
                    i.$observe("minlength", function (n) {
                        u = g(n) || 0;
                        r.$validate()
                    });
                    r.$validators.minlength = function (n, t) {
                        return r.$isEmpty(t) || t.length >= u
                    }
                }
            }
        }
    };
    if (n.angular.bootstrap) {
        console.log("WARNING: Tried to load angular more than once.");
        return
    }
    hl();
    al(ft);
    f(t).ready(function () {
        el(t, uo)
    })
})(window, document);
window.angular.$$csp() || window.angular.element(document).find("head").prepend('<style type="text/css">@charset "UTF-8";[ng\\:cloak],[ng-cloak],[data-ng-cloak],[x-ng-cloak],.ng-cloak,.x-ng-cloak,.ng-hide:not(.ng-hide-animate){display:none !important;}ng\\:form{display:block;}<\/style>');
//# sourceMappingURL=angular.min.js.map
;
/**
 * @license AngularJS v1.4.0-rc.0
 * (c) 2010-2015 Google, Inc. http://angularjs.org
 * License: MIT
 */
(function (window, angular, undefined) {
    'use strict';

    /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
     *     Any commits to this file should be reviewed with security in mind.  *
     *   Changes to this file can potentially create security vulnerabilities. *
     *          An approval from 2 Core members with history of modifying      *
     *                         this file is required.                          *
     *                                                                         *
     *  Does the change somehow allow for arbitrary javascript to be executed? *
     *    Or allows for someone to change the prototype of built-in objects?   *
     *     Or gives undesired access to variables likes document or window?    *
     * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

    var $sanitizeMinErr = angular.$$minErr('$sanitize');

    /**
     * @ngdoc module
     * @name ngSanitize
     * @description
     *
     * # ngSanitize
     *
     * The `ngSanitize` module provides functionality to sanitize HTML.
     *
     *
     * <div doc-module-components="ngSanitize"></div>
     *
     * See {@link ngSanitize.$sanitize `$sanitize`} for usage.
     */

    /*
     * HTML Parser By Misko Hevery (misko@hevery.com)
     * based on:  HTML Parser By John Resig (ejohn.org)
     * Original code by Erik Arvidsson, Mozilla Public License
     * http://erik.eae.net/simplehtmlparser/simplehtmlparser.js
     *
     * // Use like so:
     * htmlParser(htmlString, {
     *     start: function(tag, attrs, unary) {},
     *     end: function(tag) {},
     *     chars: function(text) {},
     *     comment: function(text) {}
     * });
     *
     */


    /**
     * @ngdoc service
     * @name $sanitize
     * @kind function
     *
     * @description
     *   The input is sanitized by parsing the HTML into tokens. All safe tokens (from a whitelist) are
     *   then serialized back to properly escaped html string. This means that no unsafe input can make
     *   it into the returned string, however, since our parser is more strict than a typical browser
     *   parser, it's possible that some obscure input, which would be recognized as valid HTML by a
     *   browser, won't make it through the sanitizer. The input may also contain SVG markup.
     *   The whitelist is configured using the functions `aHrefSanitizationWhitelist` and
     *   `imgSrcSanitizationWhitelist` of {@link ng.$compileProvider `$compileProvider`}.
     *
     * @param {string} html HTML input.
     * @returns {string} Sanitized HTML.
     *
     * @example
     <example module="sanitizeExample" deps="angular-sanitize.js">
     <file name="index.html">
     <script>
     angular.module('sanitizeExample', ['ngSanitize'])
     .controller('ExampleController', ['$scope', '$sce', function($scope, $sce) {
     $scope.snippet =
     '<p style="color:blue">an html\n' +
     '<em onmouseover="this.textContent=\'PWN3D!\'">click here</em>\n' +
     'snippet</p>';
     $scope.deliberatelyTrustDangerousSnippet = function() {
     return $sce.trustAsHtml($scope.snippet);
     };
     }]);
     </script>
     <div ng-controller="ExampleController">
     Snippet: <textarea ng-model="snippet" cols="60" rows="3"></textarea>
     <table>
     <tr>
     <td>Directive</td>
     <td>How</td>
     <td>Source</td>
     <td>Rendered</td>
     </tr>
     <tr id="bind-html-with-sanitize">
     <td>ng-bind-html</td>
     <td>Automatically uses $sanitize</td>
     <td><pre>&lt;div ng-bind-html="snippet"&gt;<br/>&lt;/div&gt;</pre></td>
     <td><div ng-bind-html="snippet"></div></td>
     </tr>
     <tr id="bind-html-with-trust">
     <td>ng-bind-html</td>
     <td>Bypass $sanitize by explicitly trusting the dangerous value</td>
     <td>
     <pre>&lt;div ng-bind-html="deliberatelyTrustDangerousSnippet()"&gt;
     &lt;/div&gt;</pre>
     </td>
     <td><div ng-bind-html="deliberatelyTrustDangerousSnippet()"></div></td>
     </tr>
     <tr id="bind-default">
     <td>ng-bind</td>
     <td>Automatically escapes</td>
     <td><pre>&lt;div ng-bind="snippet"&gt;<br/>&lt;/div&gt;</pre></td>
     <td><div ng-bind="snippet"></div></td>
     </tr>
     </table>
     </div>
     </file>
     <file name="protractor.js" type="protractor">
     it('should sanitize the html snippet by default', function() {
     expect(element(by.css('#bind-html-with-sanitize div')).getInnerHtml()).
     toBe('<p>an html\n<em>click here</em>\nsnippet</p>');
     });

     it('should inline raw snippet if bound to a trusted value', function() {
     expect(element(by.css('#bind-html-with-trust div')).getInnerHtml()).
     toBe("<p style=\"color:blue\">an html\n" +
     "<em onmouseover=\"this.textContent='PWN3D!'\">click here</em>\n" +
     "snippet</p>");
     });

     it('should escape snippet without any filter', function() {
     expect(element(by.css('#bind-default div')).getInnerHtml()).
     toBe("&lt;p style=\"color:blue\"&gt;an html\n" +
     "&lt;em onmouseover=\"this.textContent='PWN3D!'\"&gt;click here&lt;/em&gt;\n" +
     "snippet&lt;/p&gt;");
     });

     it('should update', function() {
     element(by.model('snippet')).clear();
     element(by.model('snippet')).sendKeys('new <b onclick="alert(1)">text</b>');
     expect(element(by.css('#bind-html-with-sanitize div')).getInnerHtml()).
     toBe('new <b>text</b>');
     expect(element(by.css('#bind-html-with-trust div')).getInnerHtml()).toBe(
     'new <b onclick="alert(1)">text</b>');
     expect(element(by.css('#bind-default div')).getInnerHtml()).toBe(
     "new &lt;b onclick=\"alert(1)\"&gt;text&lt;/b&gt;");
     });
     </file>
     </example>
     */
    function $SanitizeProvider() {
        this.$get = ['$$sanitizeUri', function ($$sanitizeUri) {
            return function (html) {
                var buf = [];
                htmlParser(html, htmlSanitizeWriter(buf, function (uri, isImage) {
                    return !/^unsafe/.test($$sanitizeUri(uri, isImage));
                }));
                return buf.join('');
            };
        }];
    }

    function sanitizeText(chars) {
        var buf = [];
        var writer = htmlSanitizeWriter(buf, angular.noop);
        writer.chars(chars);
        return buf.join('');
    }


    // Regular Expressions for parsing tags and attributes
    var START_TAG_REGEXP =
            /^<((?:[a-zA-Z])[\w:-]*)((?:\s+[\w:-]+(?:\s*=\s*(?:(?:"[^"]*")|(?:'[^']*')|[^>\s]+))?)*)\s*(\/?)\s*(>?)/,
        END_TAG_REGEXP = /^<\/\s*([\w:-]+)[^>]*>/,
        ATTR_REGEXP = /([\w:-]+)(?:\s*=\s*(?:(?:"((?:[^"])*)")|(?:'((?:[^'])*)')|([^>\s]+)))?/g,
        BEGIN_TAG_REGEXP = /^</,
        BEGING_END_TAGE_REGEXP = /^<\//,
        COMMENT_REGEXP = /<!--(.*?)-->/g,
        DOCTYPE_REGEXP = /<!DOCTYPE([^>]*?)>/i,
        CDATA_REGEXP = /<!\[CDATA\[(.*?)]]>/g,
        SURROGATE_PAIR_REGEXP = /[\uD800-\uDBFF][\uDC00-\uDFFF]/g,
        // Match everything outside of normal chars and " (quote character)
        NON_ALPHANUMERIC_REGEXP = /([^\#-~| |!])/g;


    // Good source of info about elements and attributes
    // http://dev.w3.org/html5/spec/Overview.html#semantics
    // http://simon.html5.org/html-elements

    // Safe Void Elements - HTML5
    // http://dev.w3.org/html5/spec/Overview.html#void-elements
    var voidElements = makeMap("area,br,col,hr,img,wbr");

    // Elements that you can, intentionally, leave open (and which close themselves)
    // http://dev.w3.org/html5/spec/Overview.html#optional-tags
    var optionalEndTagBlockElements = makeMap("colgroup,dd,dt,li,p,tbody,td,tfoot,th,thead,tr"),
        optionalEndTagInlineElements = makeMap("rp,rt"),
        optionalEndTagElements = angular.extend({},
            optionalEndTagInlineElements,
            optionalEndTagBlockElements);

    // Safe Block Elements - HTML5
    var blockElements = angular.extend({}, optionalEndTagBlockElements, makeMap("address,article," +
        "aside,blockquote,caption,center,del,dir,div,dl,figure,figcaption,footer,h1,h2,h3,h4,h5," +
        "h6,header,hgroup,hr,ins,map,menu,nav,ol,pre,script,section,table,ul"));

    // Inline Elements - HTML5
    var inlineElements = angular.extend({}, optionalEndTagInlineElements, makeMap("a,abbr,acronym,b," +
        "bdi,bdo,big,br,cite,code,del,dfn,em,font,i,img,ins,kbd,label,map,mark,q,ruby,rp,rt,s," +
        "samp,small,span,strike,strong,sub,sup,time,tt,u,var"));

    // SVG Elements
    // https://wiki.whatwg.org/wiki/Sanitization_rules#svg_Elements
    // Note: the elements animate,animateColor,animateMotion,animateTransform,set are intentionally omitted.
    // They can potentially allow for arbitrary javascript to be executed. See #11290
    var svgElements = makeMap("circle,defs,desc,ellipse,font-face,font-face-name,font-face-src,g,glyph," +
        "hkern,image,linearGradient,line,marker,metadata,missing-glyph,mpath,path,polygon,polyline," +
        "radialGradient,rect,stop,svg,switch,text,title,tspan,use");

    // Special Elements (can contain anything)
    var specialElements = makeMap("script,style");

    var validElements = angular.extend({},
        voidElements,
        blockElements,
        inlineElements,
        optionalEndTagElements,
        svgElements);

    //Attributes that have href and hence need to be sanitized
    var uriAttrs = makeMap("background,cite,href,longdesc,src,usemap,xlink:href");

    var htmlAttrs = makeMap('abbr,align,alt,axis,bgcolor,border,cellpadding,cellspacing,class,clear,' +
        'color,cols,colspan,compact,coords,dir,face,headers,height,hreflang,hspace,' +
        'ismap,lang,language,nohref,nowrap,rel,rev,rows,rowspan,rules,' +
        'scope,scrolling,shape,size,span,start,summary,target,title,type,' +
        'valign,value,vspace,width');

    // SVG attributes (without "id" and "name" attributes)
    // https://wiki.whatwg.org/wiki/Sanitization_rules#svg_Attributes
    var svgAttrs = makeMap('accent-height,accumulate,additive,alphabetic,arabic-form,ascent,' +
        'baseProfile,bbox,begin,by,calcMode,cap-height,class,color,color-rendering,content,' +
        'cx,cy,d,dx,dy,descent,display,dur,end,fill,fill-rule,font-family,font-size,font-stretch,' +
        'font-style,font-variant,font-weight,from,fx,fy,g1,g2,glyph-name,gradientUnits,hanging,' +
        'height,horiz-adv-x,horiz-origin-x,ideographic,k,keyPoints,keySplines,keyTimes,lang,' +
        'marker-end,marker-mid,marker-start,markerHeight,markerUnits,markerWidth,mathematical,' +
        'max,min,offset,opacity,orient,origin,overline-position,overline-thickness,panose-1,' +
        'path,pathLength,points,preserveAspectRatio,r,refX,refY,repeatCount,repeatDur,' +
        'requiredExtensions,requiredFeatures,restart,rotate,rx,ry,slope,stemh,stemv,stop-color,' +
        'stop-opacity,strikethrough-position,strikethrough-thickness,stroke,stroke-dasharray,' +
        'stroke-dashoffset,stroke-linecap,stroke-linejoin,stroke-miterlimit,stroke-opacity,' +
        'stroke-width,systemLanguage,target,text-anchor,to,transform,type,u1,u2,underline-position,' +
        'underline-thickness,unicode,unicode-range,units-per-em,values,version,viewBox,visibility,' +
        'width,widths,x,x-height,x1,x2,xlink:actuate,xlink:arcrole,xlink:role,xlink:show,xlink:title,' +
        'xlink:type,xml:base,xml:lang,xml:space,xmlns,xmlns:xlink,y,y1,y2,zoomAndPan', true);

    var validAttrs = angular.extend({},
        uriAttrs,
        svgAttrs,
        htmlAttrs);

    function makeMap(str, lowercaseKeys) {
        var obj = {}, items = str.split(','), i;
        for (i = 0; i < items.length; i++) {
            obj[lowercaseKeys ? angular.lowercase(items[i]) : items[i]] = true;
        }
        return obj;
    }


    /**
     * @example
     * htmlParser(htmlString, {
     *     start: function(tag, attrs, unary) {},
     *     end: function(tag) {},
     *     chars: function(text) {},
     *     comment: function(text) {}
     * });
     *
     * @param {string} html string
     * @param {object} handler
     */
    function htmlParser(html, handler) {
        if (typeof html !== 'string') {
            if (html === null || typeof html === 'undefined') {
                html = '';
            } else {
                html = '' + html;
            }
        }
        var index, chars, match, stack = [], last = html, text;
        stack.last = function () {
            return stack[stack.length - 1];
        };

        while (html) {
            text = '';
            chars = true;

            // Make sure we're not in a script or style element
            if (!stack.last() || !specialElements[stack.last()]) {

                // Comment
                if (html.indexOf("<!--") === 0) {
                    // comments containing -- are not allowed unless they terminate the comment
                    index = html.indexOf("--", 4);

                    if (index >= 0 && html.lastIndexOf("-->", index) === index) {
                        if (handler.comment) handler.comment(html.substring(4, index));
                        html = html.substring(index + 3);
                        chars = false;
                    }
                    // DOCTYPE
                } else if (DOCTYPE_REGEXP.test(html)) {
                    match = html.match(DOCTYPE_REGEXP);

                    if (match) {
                        html = html.replace(match[0], '');
                        chars = false;
                    }
                    // end tag
                } else if (BEGING_END_TAGE_REGEXP.test(html)) {
                    match = html.match(END_TAG_REGEXP);

                    if (match) {
                        html = html.substring(match[0].length);
                        match[0].replace(END_TAG_REGEXP, parseEndTag);
                        chars = false;
                    }

                    // start tag
                } else if (BEGIN_TAG_REGEXP.test(html)) {
                    match = html.match(START_TAG_REGEXP);

                    if (match) {
                        // We only have a valid start-tag if there is a '>'.
                        if (match[4]) {
                            html = html.substring(match[0].length);
                            match[0].replace(START_TAG_REGEXP, parseStartTag);
                        }
                        chars = false;
                    } else {
                        // no ending tag found --- this piece should be encoded as an entity.
                        text += '<';
                        html = html.substring(1);
                    }
                }

                if (chars) {
                    index = html.indexOf("<");

                    text += index < 0 ? html : html.substring(0, index);
                    html = index < 0 ? "" : html.substring(index);

                    if (handler.chars) handler.chars(decodeEntities(text));
                }

            } else {
                // IE versions 9 and 10 do not understand the regex '[^]', so using a workaround with [\W\w].
                html = html.replace(new RegExp("([\\W\\w]*)<\\s*\\/\\s*" + stack.last() + "[^>]*>", 'i'),
                    function (all, text) {
                        text = text.replace(COMMENT_REGEXP, "$1").replace(CDATA_REGEXP, "$1");

                        if (handler.chars) handler.chars(decodeEntities(text));

                        return "";
                    });

                parseEndTag("", stack.last());
            }

            if (html == last) {
                throw $sanitizeMinErr('badparse', "The sanitizer was unable to parse the following block " +
                    "of html: {0}", html);
            }
            last = html;
        }

        // Clean up any remaining tags
        parseEndTag();

        function parseStartTag(tag, tagName, rest, unary) {
            tagName = angular.lowercase(tagName);
            if (blockElements[tagName]) {
                while (stack.last() && inlineElements[stack.last()]) {
                    parseEndTag("", stack.last());
                }
            }

            if (optionalEndTagElements[tagName] && stack.last() == tagName) {
                parseEndTag("", tagName);
            }

            unary = voidElements[tagName] || !!unary;

            if (!unary) {
                stack.push(tagName);
            }

            var attrs = {};

            rest.replace(ATTR_REGEXP,
                function (match, name, doubleQuotedValue, singleQuotedValue, unquotedValue) {
                    var value = doubleQuotedValue
                        || singleQuotedValue
                        || unquotedValue
                        || '';

                    attrs[name] = decodeEntities(value);
                });
            if (handler.start) handler.start(tagName, attrs, unary);
        }

        function parseEndTag(tag, tagName) {
            var pos = 0, i;
            tagName = angular.lowercase(tagName);
            if (tagName) {
                // Find the closest opened tag of the same type
                for (pos = stack.length - 1; pos >= 0; pos--) {
                    if (stack[pos] == tagName) break;
                }
            }

            if (pos >= 0) {
                // Close all the open elements, up the stack
                for (i = stack.length - 1; i >= pos; i--)
                    if (handler.end) handler.end(stack[i]);

                // Remove the open elements from the stack
                stack.length = pos;
            }
        }
    }

    var hiddenPre = document.createElement("pre");

    /**
     * decodes all entities into regular string
     * @param value
     * @returns {string} A string with decoded entities.
     */
    function decodeEntities(value) {
        if (!value) {
            return '';
        }

        hiddenPre.innerHTML = value.replace(/</g, "&lt;");
        // innerText depends on styling as it doesn't display hidden elements.
        // Therefore, it's better to use textContent not to cause unnecessary reflows.
        return hiddenPre.textContent;
    }

    /**
     * Escapes all potentially dangerous characters, so that the
     * resulting string can be safely inserted into attribute or
     * element text.
     * @param value
     * @returns {string} escaped text
     */
    function encodeEntities(value) {
        return value.replace(/&/g, '&amp;').replace(SURROGATE_PAIR_REGEXP, function (value) {
            var hi = value.charCodeAt(0);
            var low = value.charCodeAt(1);
            return '&#' + (((hi - 0xD800) * 0x400) + (low - 0xDC00) + 0x10000) + ';';
        }).replace(NON_ALPHANUMERIC_REGEXP, function (value) {
            return '&#' + value.charCodeAt(0) + ';';
        }).replace(/</g, '&lt;').replace(/>/g, '&gt;');
    }

    /**
     * create an HTML/XML writer which writes to buffer
     * @param {Array} buf use buf.jain('') to get out sanitized html string
     * @returns {object} in the form of {
     *     start: function(tag, attrs, unary) {},
     *     end: function(tag) {},
     *     chars: function(text) {},
     *     comment: function(text) {}
     * }
     */
    function htmlSanitizeWriter(buf, uriValidator) {
        var ignore = false;
        var out = angular.bind(buf, buf.push);
        return {
            start: function (tag, attrs, unary) {
                tag = angular.lowercase(tag);
                if (!ignore && specialElements[tag]) {
                    ignore = tag;
                }
                if (!ignore && validElements[tag] === true) {
                    out('<');
                    out(tag);
                    angular.forEach(attrs, function (value, key) {
                        var lkey = angular.lowercase(key);
                        var isImage = (tag === 'img' && lkey === 'src') || (lkey === 'background');
                        if (validAttrs[lkey] === true &&
                            (uriAttrs[lkey] !== true || uriValidator(value, isImage))) {
                            out(' ');
                            out(key);
                            out('="');
                            out(encodeEntities(value));
                            out('"');
                        }
                    });
                    out(unary ? '/>' : '>');
                }
            },
            end: function (tag) {
                tag = angular.lowercase(tag);
                if (!ignore && validElements[tag] === true) {
                    out('</');
                    out(tag);
                    out('>');
                }
                if (tag == ignore) {
                    ignore = false;
                }
            },
            chars: function (chars) {
                if (!ignore) {
                    out(encodeEntities(chars));
                }
            }
        };
    }


    // define ngSanitize module and register $sanitize service
    angular.module('ngSanitize', []).provider('$sanitize', $SanitizeProvider);

    /* global sanitizeText: false */

    /**
     * @ngdoc filter
     * @name linky
     * @kind function
     *
     * @description
     * Finds links in text input and turns them into html links. Supports http/https/ftp/mailto and
     * plain email address links.
     *
     * Requires the {@link ngSanitize `ngSanitize`} module to be installed.
     *
     * @param {string} text Input text.
     * @param {string} target Window (_blank|_self|_parent|_top) or named frame to open links in.
     * @returns {string} Html-linkified text.
     *
     * @usage
     <span ng-bind-html="linky_expression | linky"></span>
     *
     * @example
     <example module="linkyExample" deps="angular-sanitize.js">
     <file name="index.html">
     <script>
     angular.module('linkyExample', ['ngSanitize'])
     .controller('ExampleController', ['$scope', function($scope) {
     $scope.snippet =
     'Pretty text with some links:\n'+
     'http://angularjs.org/,\n'+
     'mailto:us@somewhere.org,\n'+
     'another@somewhere.org,\n'+
     'and one more: ftp://127.0.0.1/.';
     $scope.snippetWithTarget = 'http://angularjs.org/';
     }]);
     </script>
     <div ng-controller="ExampleController">
     Snippet: <textarea ng-model="snippet" cols="60" rows="3"></textarea>
     <table>
     <tr>
     <td>Filter</td>
     <td>Source</td>
     <td>Rendered</td>
     </tr>
     <tr id="linky-filter">
     <td>linky filter</td>
     <td>
     <pre>&lt;div ng-bind-html="snippet | linky"&gt;<br>&lt;/div&gt;</pre>
     </td>
     <td>
     <div ng-bind-html="snippet | linky"></div>
     </td>
     </tr>
     <tr id="linky-target">
     <td>linky target</td>
     <td>
     <pre>&lt;div ng-bind-html="snippetWithTarget | linky:'_blank'"&gt;<br>&lt;/div&gt;</pre>
     </td>
     <td>
     <div ng-bind-html="snippetWithTarget | linky:'_blank'"></div>
     </td>
     </tr>
     <tr id="escaped-html">
     <td>no filter</td>
     <td><pre>&lt;div ng-bind="snippet"&gt;<br>&lt;/div&gt;</pre></td>
     <td><div ng-bind="snippet"></div></td>
     </tr>
     </table>
     </file>
     <file name="protractor.js" type="protractor">
     it('should linkify the snippet with urls', function() {
     expect(element(by.id('linky-filter')).element(by.binding('snippet | linky')).getText()).
     toBe('Pretty text with some links: http://angularjs.org/, us@somewhere.org, ' +
     'another@somewhere.org, and one more: ftp://127.0.0.1/.');
     expect(element.all(by.css('#linky-filter a')).count()).toEqual(4);
     });

     it('should not linkify snippet without the linky filter', function() {
     expect(element(by.id('escaped-html')).element(by.binding('snippet')).getText()).
     toBe('Pretty text with some links: http://angularjs.org/, mailto:us@somewhere.org, ' +
     'another@somewhere.org, and one more: ftp://127.0.0.1/.');
     expect(element.all(by.css('#escaped-html a')).count()).toEqual(0);
     });

     it('should update', function() {
     element(by.model('snippet')).clear();
     element(by.model('snippet')).sendKeys('new http://link.');
     expect(element(by.id('linky-filter')).element(by.binding('snippet | linky')).getText()).
     toBe('new http://link.');
     expect(element.all(by.css('#linky-filter a')).count()).toEqual(1);
     expect(element(by.id('escaped-html')).element(by.binding('snippet')).getText())
     .toBe('new http://link.');
     });

     it('should work with the target property', function() {
     expect(element(by.id('linky-target')).
     element(by.binding("snippetWithTarget | linky:'_blank'")).getText()).
     toBe('http://angularjs.org/');
     expect(element(by.css('#linky-target a')).getAttribute('target')).toEqual('_blank');
     });
     </file>
     </example>
     */
    angular.module('ngSanitize').filter('linky', ['$sanitize', function ($sanitize) {
        var LINKY_URL_REGEXP =
                /((ftp|https?):\/\/|(www\.)|(mailto:)?[A-Za-z0-9._%+-]+@)\S*[^\s.;,(){}<>"”’]/,
            MAILTO_REGEXP = /^mailto:/;

        return function (text, target) {
            if (!text) return text;
            var match;
            var raw = text;
            var html = [];
            var url;
            var i;
            while ((match = raw.match(LINKY_URL_REGEXP))) {
                // We can not end in these as they are sometimes found at the end of the sentence
                url = match[0];
                // if we did not match ftp/http/www/mailto then assume mailto
                if (!match[2] && !match[4]) {
                    url = (match[3] ? 'http://' : 'mailto:') + url;
                }
                i = match.index;
                addText(raw.substr(0, i));
                addLink(url, match[0].replace(MAILTO_REGEXP, ''));
                raw = raw.substring(i + match[0].length);
            }
            addText(raw);
            return $sanitize(html.join(''));

            function addText(text) {
                if (!text) {
                    return;
                }
                html.push(sanitizeText(text));
            }

            function addLink(url, text) {
                html.push('<a ');
                if (angular.isDefined(target)) {
                    html.push('target="',
                        target,
                        '" ');
                }
                html.push('href="',
                    url.replace(/"/g, '&quot;'),
                    '">');
                addText(text);
                html.push('</a>');
            }
        };
    }]);


})(window, window.angular);
;
/*
 AngularJS v1.2.0rc1
 (c) 2010-2012 Google, Inc. http://angularjs.org
 License: MIT
*/
(function (p, b, E) {
    'use strict';

    function u(b, d) {
        return l(new (l(function () {
        }, {prototype: b})), d)
    }

    var v = b.copy, A = b.equals, l = b.extend, t = b.forEach, n = b.isDefined, w = b.isFunction, x = b.isString,
        B = b.element;
    p = b.module("ngRoute", ["ng"]).provider("$route", function () {
        function b(c, q) {
            var d = q.caseInsensitiveMatch, m = {originalPath: c, regexp: c}, l = m.keys = [];
            c = c.replace(/([().])/g, "\\$1").replace(/(\/)?:(\w+)([\?|\*])?/g, function (c, b, q, d) {
                c = "?" === d ? d : null;
                d = "*" === d ? d : null;
                l.push({name: q, optional: !!c});
                b = b || "";
                return "" +
                    (c ? "" : b) + "(?:" + (c ? b : "") + (d && "(.+)?" || "([^/]+)?") + ")" + (c || "")
            }).replace(/([\/$\*])/g, "\\$1");
            m.regexp = RegExp("^" + c + "$", d ? "i" : "");
            return m
        }

        var d = {};
        this.when = function (c, q) {
            d[c] = l({reloadOnSearch: !0}, q, c && b(c, q));
            if (c) {
                var h = "/" == c[c.length - 1] ? c.substr(0, c.length - 1) : c + "/";
                d[h] = l({redirectTo: c}, b(h, q))
            }
            return this
        };
        this.otherwise = function (c) {
            this.when(null, c);
            return this
        };
        this.$get = ["$rootScope", "$location", "$routeParams", "$q", "$injector", "$http", "$templateCache", "$sce", function (c, b, h, m, y, p, C, D) {
            function r() {
                var a =
                    s(), e = k.current;
                if (a && e && a.$$route === e.$$route && A(a.pathParams, e.pathParams) && !a.reloadOnSearch && !f) e.params = a.params, v(e.params, h), c.$broadcast("$routeUpdate", e); else if (a || e) f = !1, c.$broadcast("$routeChangeStart", a, e), (k.current = a) && a.redirectTo && (x(a.redirectTo) ? b.path(g(a.redirectTo, a.params)).search(a.params).replace() : b.url(a.redirectTo(a.pathParams, b.path(), b.search())).replace()), m.when(a).then(function () {
                    if (a) {
                        var c = l({}, a.resolve), b, e;
                        t(c, function (a, b) {
                            c[b] = x(a) ? y.get(a) : y.invoke(a)
                        });
                        n(b =
                            a.template) ? w(b) && (b = b(a.params)) : n(e = a.templateUrl) && (w(e) && (e = e(a.params)), e = D.getTrustedResourceUrl(e), n(e) && (a.loadedTemplateUrl = e, b = p.get(e, {cache: C}).then(function (a) {
                            return a.data
                        })));
                        n(b) && (c.$template = b);
                        return m.all(c)
                    }
                }).then(function (b) {
                    a == k.current && (a && (a.locals = b, v(a.params, h)), c.$broadcast("$routeChangeSuccess", a, e))
                }, function (b) {
                    a == k.current && c.$broadcast("$routeChangeError", a, e, b)
                })
            }

            function s() {
                var a, c;
                t(d, function (d, k) {
                    var f;
                    if (f = !c) {
                        var g = b.path();
                        f = d.keys;
                        var m = {};
                        if (d.regexp) if (g =
                            d.regexp.exec(g)) {
                            for (var h = 1, p = g.length; h < p; ++h) {
                                var r = f[h - 1], n = "string" == typeof g[h] ? decodeURIComponent(g[h]) : g[h];
                                r && n && (m[r.name] = n)
                            }
                            f = m
                        } else f = null; else f = null;
                        f = a = f
                    }
                    f && (c = u(d, {params: l({}, b.search(), a), pathParams: a}), c.$$route = d)
                });
                return c || d[null] && u(d[null], {params: {}, pathParams: {}})
            }

            function g(a, c) {
                var b = [];
                t((a || "").split(":"), function (a, d) {
                    if (0 == d) b.push(a); else {
                        var f = a.match(/(\w+)(.*)/), g = f[1];
                        b.push(c[g]);
                        b.push(f[2] || "");
                        delete c[g]
                    }
                });
                return b.join("")
            }

            var f = !1, k = {
                routes: d, reload: function () {
                    f =
                        !0;
                    c.$evalAsync(r)
                }
            };
            c.$on("$locationChangeSuccess", r);
            return k
        }]
    });
    p.provider("$routeParams", function () {
        this.$get = function () {
            return {}
        }
    });
    var z = 500;
    p.directive("ngView", ["$route", "$anchorScroll", "$compile", "$controller", "$animate", function (b, d, c, q, h) {
        return {
            restrict: "ECA", terminal: !0, priority: z, compile: function (m, l) {
                var p = l.onload || "";
                m.html("");
                var n = B(document.createComment(" ngView "));
                m.replaceWith(n);
                return function (l) {
                    function r() {
                        g && (g.$destroy(), g = null);
                        f && (h.leave(f), f = null)
                    }

                    function s() {
                        var k =
                            b.current && b.current.locals, a = k && k.$template;
                        if (a) {
                            r();
                            g = l.$new();
                            f = m.clone();
                            f.html(a);
                            h.enter(f, null, n);
                            var a = c(f, !1, z - 1), e = b.current;
                            e.controller && (k.$scope = g, k = q(e.controller, k), e.controllerAs && (g[e.controllerAs] = k), f.data("$ngControllerController", k), f.children().data("$ngControllerController", k));
                            e.scope = g;
                            a(g);
                            g.$emit("$viewContentLoaded");
                            g.$eval(p);
                            d()
                        } else r()
                    }

                    var g, f;
                    l.$on("$routeChangeSuccess", s);
                    s()
                }
            }
        }
    }])
})(window, window.angular);
/*
//@ sourceMappingURL=angular-route.min.js.map
*/

/*
 * angular-ui-bootstrap
 * http://angular-ui.github.io/bootstrap/

 * Version: 0.12.0 - 2014-11-16
 * License: MIT
 */
angular.module("ui.bootstrap", ["ui.bootstrap.tpls", "ui.bootstrap.transition", "ui.bootstrap.collapse", "ui.bootstrap.accordion", "ui.bootstrap.alert", "ui.bootstrap.bindHtml", "ui.bootstrap.buttons", "ui.bootstrap.carousel", "ui.bootstrap.dateparser", "ui.bootstrap.position", "ui.bootstrap.datepicker", "ui.bootstrap.dropdown", "ui.bootstrap.modal", "ui.bootstrap.pagination", "ui.bootstrap.tooltip", "ui.bootstrap.popover", "ui.bootstrap.progressbar", "ui.bootstrap.rating", "ui.bootstrap.tabs", "ui.bootstrap.timepicker", "ui.bootstrap.typeahead"]), angular.module("ui.bootstrap.tpls", ["template/accordion/accordion-group.html", "template/accordion/accordion.html", "template/alert/alert.html", "template/carousel/carousel.html", "template/carousel/slide.html", "template/datepicker/datepicker.html", "template/datepicker/day.html", "template/datepicker/month.html", "template/datepicker/popup.html", "template/datepicker/year.html", "template/modal/backdrop.html", "template/modal/window.html", "template/pagination/pager.html", "template/pagination/pagination.html", "template/tooltip/tooltip-html-unsafe-popup.html", "template/tooltip/tooltip-popup.html", "template/popover/popover.html", "template/progressbar/bar.html", "template/progressbar/progress.html", "template/progressbar/progressbar.html", "template/rating/rating.html", "template/tabs/tab.html", "template/tabs/tabset.html", "template/timepicker/timepicker.html", "template/typeahead/typeahead-match.html", "template/typeahead/typeahead-popup.html"]), angular.module("ui.bootstrap.transition", []).factory("$transition", ["$q", "$timeout", "$rootScope", function (a, b, c) {
    function d(a) {
        for (var b in a) if (void 0 !== f.style[b]) return a[b]
    }

    var e = function (d, f, g) {
        g = g || {};
        var h = a.defer(), i = e[g.animation ? "animationEndEventName" : "transitionEndEventName"], j = function () {
            c.$apply(function () {
                d.unbind(i, j), h.resolve(d)
            })
        };
        return i && d.bind(i, j), b(function () {
            angular.isString(f) ? d.addClass(f) : angular.isFunction(f) ? f(d) : angular.isObject(f) && d.css(f), i || h.resolve(d)
        }), h.promise.cancel = function () {
            i && d.unbind(i, j), h.reject("Transition cancelled")
        }, h.promise
    }, f = document.createElement("trans"), g = {
        WebkitTransition: "webkitTransitionEnd",
        MozTransition: "transitionend",
        OTransition: "oTransitionEnd",
        transition: "transitionend"
    }, h = {
        WebkitTransition: "webkitAnimationEnd",
        MozTransition: "animationend",
        OTransition: "oAnimationEnd",
        transition: "animationend"
    };
    return e.transitionEndEventName = d(g), e.animationEndEventName = d(h), e
}]), angular.module("ui.bootstrap.collapse", ["ui.bootstrap.transition"]).directive("collapse", ["$transition", function (a) {
    return {
        link: function (b, c, d) {
            function e(b) {
                function d() {
                    j === e && (j = void 0)
                }

                var e = a(c, b);
                return j && j.cancel(), j = e, e.then(d, d), e
            }

            function f() {
                k ? (k = !1, g()) : (c.removeClass("collapse").addClass("collapsing"), e({height: c[0].scrollHeight + "px"}).then(g))
            }

            function g() {
                c.removeClass("collapsing"), c.addClass("collapse in"), c.css({height: "auto"})
            }

            function h() {
                if (k) k = !1, i(), c.css({height: 0}); else {
                    c.css({height: c[0].scrollHeight + "px"});
                    {
                        c[0].offsetWidth
                    }
                    c.removeClass("collapse in").addClass("collapsing"), e({height: 0}).then(i)
                }
            }

            function i() {
                c.removeClass("collapsing"), c.addClass("collapse")
            }

            var j, k = !0;
            b.$watch(d.collapse, function (a) {
                a ? h() : f()
            })
        }
    }
}]), angular.module("ui.bootstrap.accordion", ["ui.bootstrap.collapse"]).constant("accordionConfig", {closeOthers: !0}).controller("AccordionController", ["$scope", "$attrs", "accordionConfig", function (a, b, c) {
    this.groups = [], this.closeOthers = function (d) {
        var e = angular.isDefined(b.closeOthers) ? a.$eval(b.closeOthers) : c.closeOthers;
        e && angular.forEach(this.groups, function (a) {
            a !== d && (a.isOpen = !1)
        })
    }, this.addGroup = function (a) {
        var b = this;
        this.groups.push(a), a.$on("$destroy", function () {
            b.removeGroup(a)
        })
    }, this.removeGroup = function (a) {
        var b = this.groups.indexOf(a);
        -1 !== b && this.groups.splice(b, 1)
    }
}]).directive("accordion", function () {
    return {
        restrict: "EA",
        controller: "AccordionController",
        transclude: !0,
        replace: !1,
        templateUrl: "template/accordion/accordion.html"
    }
}).directive("accordionGroup", function () {
    return {
        require: "^accordion",
        restrict: "EA",
        transclude: !0,
        replace: !0,
        templateUrl: "template/accordion/accordion-group.html",
        scope: {heading: "@", isOpen: "=?", isDisabled: "=?"},
        controller: function () {
            this.setHeading = function (a) {
                this.heading = a
            }
        },
        link: function (a, b, c, d) {
            d.addGroup(a), a.$watch("isOpen", function (b) {
                b && d.closeOthers(a)
            }), a.toggleOpen = function () {
                a.isDisabled || (a.isOpen = !a.isOpen)
            }
        }
    }
}).directive("accordionHeading", function () {
    return {
        restrict: "EA",
        transclude: !0,
        template: "",
        replace: !0,
        require: "^accordionGroup",
        link: function (a, b, c, d, e) {
            d.setHeading(e(a, function () {
            }))
        }
    }
}).directive("accordionTransclude", function () {
    return {
        require: "^accordionGroup", link: function (a, b, c, d) {
            a.$watch(function () {
                return d[c.accordionTransclude]
            }, function (a) {
                a && (b.html(""), b.append(a))
            })
        }
    }
}), angular.module("ui.bootstrap.alert", []).controller("AlertController", ["$scope", "$attrs", function (a, b) {
    a.closeable = "close" in b, this.close = a.close
}]).directive("alert", function () {
    return {
        restrict: "EA",
        controller: "AlertController",
        templateUrl: "template/alert/alert.html",
        transclude: !0,
        replace: !0,
        scope: {type: "@", close: "&"}
    }
}).directive("dismissOnTimeout", ["$timeout", function (a) {
    return {
        require: "alert", link: function (b, c, d, e) {
            a(function () {
                e.close()
            }, parseInt(d.dismissOnTimeout, 10))
        }
    }
}]), angular.module("ui.bootstrap.bindHtml", []).directive("bindHtmlUnsafe", function () {
    return function (a, b, c) {
        b.addClass("ng-binding").data("$binding", c.bindHtmlUnsafe), a.$watch(c.bindHtmlUnsafe, function (a) {
            b.html(a || "")
        })
    }
}), angular.module("ui.bootstrap.buttons", []).constant("buttonConfig", {
    activeClass: "active",
    toggleEvent: "click"
}).controller("ButtonsController", ["buttonConfig", function (a) {
    this.activeClass = a.activeClass || "active", this.toggleEvent = a.toggleEvent || "click"
}]).directive("btnRadio", function () {
    return {
        require: ["btnRadio", "ngModel"], controller: "ButtonsController", link: function (a, b, c, d) {
            var e = d[0], f = d[1];
            f.$render = function () {
                b.toggleClass(e.activeClass, angular.equals(f.$modelValue, a.$eval(c.btnRadio)))
            }, b.bind(e.toggleEvent, function () {
                var d = b.hasClass(e.activeClass);
                (!d || angular.isDefined(c.uncheckable)) && a.$apply(function () {
                    f.$setViewValue(d ? null : a.$eval(c.btnRadio)), f.$render()
                })
            })
        }
    }
}).directive("btnCheckbox", function () {
    return {
        require: ["btnCheckbox", "ngModel"], controller: "ButtonsController", link: function (a, b, c, d) {
            function e() {
                return g(c.btnCheckboxTrue, !0)
            }

            function f() {
                return g(c.btnCheckboxFalse, !1)
            }

            function g(b, c) {
                var d = a.$eval(b);
                return angular.isDefined(d) ? d : c
            }

            var h = d[0], i = d[1];
            i.$render = function () {
                b.toggleClass(h.activeClass, angular.equals(i.$modelValue, e()))
            }, b.bind(h.toggleEvent, function () {
                a.$apply(function () {
                    i.$setViewValue(b.hasClass(h.activeClass) ? f() : e()), i.$render()
                })
            })
        }
    }
}), angular.module("ui.bootstrap.carousel", ["ui.bootstrap.transition"]).controller("CarouselController", ["$scope", "$timeout", "$interval", "$transition", function (a, b, c, d) {
    function e() {
        f();
        var b = +a.interval;
        !isNaN(b) && b > 0 && (h = c(g, b))
    }

    function f() {
        h && (c.cancel(h), h = null)
    }

    function g() {
        var b = +a.interval;
        i && !isNaN(b) && b > 0 ? a.next() : a.pause()
    }

    var h, i, j = this, k = j.slides = a.slides = [], l = -1;
    j.currentSlide = null;
    var m = !1;
    j.select = a.select = function (c, f) {
        function g() {
            if (!m) {
                if (j.currentSlide && angular.isString(f) && !a.noTransition && c.$element) {
                    c.$element.addClass(f);
                    {
                        c.$element[0].offsetWidth
                    }
                    angular.forEach(k, function (a) {
                        angular.extend(a, {direction: "", entering: !1, leaving: !1, active: !1})
                    }), angular.extend(c, {
                        direction: f,
                        active: !0,
                        entering: !0
                    }), angular.extend(j.currentSlide || {}, {
                        direction: f,
                        leaving: !0
                    }), a.$currentTransition = d(c.$element, {}), function (b, c) {
                        a.$currentTransition.then(function () {
                            h(b, c)
                        }, function () {
                            h(b, c)
                        })
                    }(c, j.currentSlide)
                } else h(c, j.currentSlide);
                j.currentSlide = c, l = i, e()
            }
        }

        function h(b, c) {
            angular.extend(b, {
                direction: "",
                active: !0,
                leaving: !1,
                entering: !1
            }), angular.extend(c || {}, {
                direction: "",
                active: !1,
                leaving: !1,
                entering: !1
            }), a.$currentTransition = null
        }

        var i = k.indexOf(c);
        void 0 === f && (f = i > l ? "next" : "prev"), c && c !== j.currentSlide && (a.$currentTransition ? (a.$currentTransition.cancel(), b(g)) : g())
    }, a.$on("$destroy", function () {
        m = !0
    }), j.indexOfSlide = function (a) {
        return k.indexOf(a)
    }, a.next = function () {
        var b = (l + 1) % k.length;
        return a.$currentTransition ? void 0 : j.select(k[b], "next")
    }, a.prev = function () {
        var b = 0 > l - 1 ? k.length - 1 : l - 1;
        return a.$currentTransition ? void 0 : j.select(k[b], "prev")
    }, a.isActive = function (a) {
        return j.currentSlide === a
    }, a.$watch("interval", e), a.$on("$destroy", f), a.play = function () {
        i || (i = !0, e())
    }, a.pause = function () {
        a.noPause || (i = !1, f())
    }, j.addSlide = function (b, c) {
        b.$element = c, k.push(b), 1 === k.length || b.active ? (j.select(k[k.length - 1]), 1 == k.length && a.play()) : b.active = !1
    }, j.removeSlide = function (a) {
        var b = k.indexOf(a);
        k.splice(b, 1), k.length > 0 && a.active ? j.select(b >= k.length ? k[b - 1] : k[b]) : l > b && l--
    }
}]).directive("carousel", [function () {
    return {
        restrict: "EA",
        transclude: !0,
        replace: !0,
        controller: "CarouselController",
        require: "carousel",
        templateUrl: "template/carousel/carousel.html",
        scope: {interval: "=", noTransition: "=", noPause: "="}
    }
}]).directive("slide", function () {
    return {
        require: "^carousel",
        restrict: "EA",
        transclude: !0,
        replace: !0,
        templateUrl: "template/carousel/slide.html",
        scope: {active: "=?"},
        link: function (a, b, c, d) {
            d.addSlide(a, b), a.$on("$destroy", function () {
                d.removeSlide(a)
            }), a.$watch("active", function (b) {
                b && d.select(a)
            })
        }
    }
}), angular.module("ui.bootstrap.dateparser", []).service("dateParser", ["$locale", "orderByFilter", function (a, b) {
    function c(a) {
        var c = [], d = a.split("");
        return angular.forEach(e, function (b, e) {
            var f = a.indexOf(e);
            if (f > -1) {
                a = a.split(""), d[f] = "(" + b.regex + ")", a[f] = "$";
                for (var g = f + 1, h = f + e.length; h > g; g++) d[g] = "", a[g] = "$";
                a = a.join(""), c.push({index: f, apply: b.apply})
            }
        }), {regex: new RegExp("^" + d.join("") + "$"), map: b(c, "index")}
    }

    function d(a, b, c) {
        return 1 === b && c > 28 ? 29 === c && (a % 4 === 0 && a % 100 !== 0 || a % 400 === 0) : 3 === b || 5 === b || 8 === b || 10 === b ? 31 > c : !0
    }

    this.parsers = {};
    var e = {
        yyyy: {
            regex: "\\d{4}", apply: function (a) {
                this.year = +a
            }
        }, yy: {
            regex: "\\d{2}", apply: function (a) {
                this.year = +a + 2e3
            }
        }, y: {
            regex: "\\d{1,4}", apply: function (a) {
                this.year = +a
            }
        }, MMMM: {
            regex: a.DATETIME_FORMATS.MONTH.join("|"), apply: function (b) {
                this.month = a.DATETIME_FORMATS.MONTH.indexOf(b)
            }
        }, MMM: {
            regex: a.DATETIME_FORMATS.SHORTMONTH.join("|"), apply: function (b) {
                this.month = a.DATETIME_FORMATS.SHORTMONTH.indexOf(b)
            }
        }, MM: {
            regex: "0[1-9]|1[0-2]", apply: function (a) {
                this.month = a - 1
            }
        }, M: {
            regex: "[1-9]|1[0-2]", apply: function (a) {
                this.month = a - 1
            }
        }, dd: {
            regex: "[0-2][0-9]{1}|3[0-1]{1}", apply: function (a) {
                this.date = +a
            }
        }, d: {
            regex: "[1-2]?[0-9]{1}|3[0-1]{1}", apply: function (a) {
                this.date = +a
            }
        }, EEEE: {regex: a.DATETIME_FORMATS.DAY.join("|")}, EEE: {regex: a.DATETIME_FORMATS.SHORTDAY.join("|")}
    };
    this.parse = function (b, e) {
        if (!angular.isString(b) || !e) return b;
        e = a.DATETIME_FORMATS[e] || e, this.parsers[e] || (this.parsers[e] = c(e));
        var f = this.parsers[e], g = f.regex, h = f.map, i = b.match(g);
        if (i && i.length) {
            for (var j, k = {year: 1900, month: 0, date: 1, hours: 0}, l = 1, m = i.length; m > l; l++) {
                var n = h[l - 1];
                n.apply && n.apply.call(k, i[l])
            }
            return d(k.year, k.month, k.date) && (j = new Date(k.year, k.month, k.date, k.hours)), j
        }
    }
}]), angular.module("ui.bootstrap.position", []).factory("$position", ["$document", "$window", function (a, b) {
    function c(a, c) {
        return a.currentStyle ? a.currentStyle[c] : b.getComputedStyle ? b.getComputedStyle(a)[c] : a.style[c]
    }

    function d(a) {
        return "static" === (c(a, "position") || "static")
    }

    var e = function (b) {
        for (var c = a[0], e = b.offsetParent || c; e && e !== c && d(e);) e = e.offsetParent;
        return e || c
    };
    return {
        position: function (b) {
            var c = this.offset(b), d = {top: 0, left: 0}, f = e(b[0]);
            f != a[0] && (d = this.offset(angular.element(f)), d.top += f.clientTop - f.scrollTop, d.left += f.clientLeft - f.scrollLeft);
            var g = b[0].getBoundingClientRect();
            return {
                width: g.width || b.prop("offsetWidth"),
                height: g.height || b.prop("offsetHeight"),
                top: c.top - d.top,
                left: c.left - d.left
            }
        }, offset: function (c) {
            var d = c[0].getBoundingClientRect();
            return {
                width: d.width || c.prop("offsetWidth"),
                height: d.height || c.prop("offsetHeight"),
                top: d.top + (b.pageYOffset || a[0].documentElement.scrollTop),
                left: d.left + (b.pageXOffset || a[0].documentElement.scrollLeft)
            }
        }, positionElements: function (a, b, c, d) {
            var e, f, g, h, i = c.split("-"), j = i[0], k = i[1] || "center";
            e = d ? this.offset(a) : this.position(a), f = b.prop("offsetWidth"), g = b.prop("offsetHeight");
            var l = {
                center: function () {
                    return e.left + e.width / 2 - f / 2
                }, left: function () {
                    return e.left
                }, right: function () {
                    return e.left + e.width
                }
            }, m = {
                center: function () {
                    return e.top + e.height / 2 - g / 2
                }, top: function () {
                    return e.top
                }, bottom: function () {
                    return e.top + e.height
                }
            };
            switch (j) {
                case"right":
                    h = {top: m[k](), left: l[j]()};
                    break;
                case"left":
                    h = {top: m[k](), left: e.left - f};
                    break;
                case"bottom":
                    h = {top: m[j](), left: l[k]()};
                    break;
                default:
                    h = {top: e.top - g, left: l[k]()}
            }
            return h
        }
    }
}]), angular.module("ui.bootstrap.datepicker", ["ui.bootstrap.dateparser", "ui.bootstrap.position"]).constant("datepickerConfig", {
    formatDay: "dd",
    formatMonth: "MMMM",
    formatYear: "yyyy",
    formatDayHeader: "EEE",
    formatDayTitle: "MMMM yyyy",
    formatMonthTitle: "yyyy",
    datepickerMode: "day",
    minMode: "day",
    maxMode: "year",
    showWeeks: !0,
    startingDay: 0,
    yearRange: 20,
    minDate: null,
    maxDate: null
}).controller("DatepickerController", ["$scope", "$attrs", "$parse", "$interpolate", "$timeout", "$log", "dateFilter", "datepickerConfig", function (a, b, c, d, e, f, g, h) {
    var i = this, j = {$setViewValue: angular.noop};
    this.modes = ["day", "month", "year"], angular.forEach(["formatDay", "formatMonth", "formatYear", "formatDayHeader", "formatDayTitle", "formatMonthTitle", "minMode", "maxMode", "showWeeks", "startingDay", "yearRange"], function (c, e) {
        i[c] = angular.isDefined(b[c]) ? 8 > e ? d(b[c])(a.$parent) : a.$parent.$eval(b[c]) : h[c]
    }), angular.forEach(["minDate", "maxDate"], function (d) {
        b[d] ? a.$parent.$watch(c(b[d]), function (a) {
            i[d] = a ? new Date(a) : null, i.refreshView()
        }) : i[d] = h[d] ? new Date(h[d]) : null
    }), a.datepickerMode = a.datepickerMode || h.datepickerMode, a.uniqueId = "datepicker-" + a.$id + "-" + Math.floor(1e4 * Math.random()), this.activeDate = angular.isDefined(b.initDate) ? a.$parent.$eval(b.initDate) : new Date, a.isActive = function (b) {
        return 0 === i.compare(b.date, i.activeDate) ? (a.activeDateId = b.uid, !0) : !1
    }, this.init = function (a) {
        j = a, j.$render = function () {
            i.render()
        }
    }, this.render = function () {
        if (j.$modelValue) {
            var a = new Date(j.$modelValue), b = !isNaN(a);
            b ? this.activeDate = a : f.error('Datepicker directive: "ng-model" value must be a Date object, a number of milliseconds since 01.01.1970 or a string representing an RFC2822 or ISO 8601 date.'), j.$setValidity("date", b)
        }
        this.refreshView()
    }, this.refreshView = function () {
        if (this.element) {
            this._refreshView();
            var a = j.$modelValue ? new Date(j.$modelValue) : null;
            j.$setValidity("date-disabled", !a || this.element && !this.isDisabled(a))
        }
    }, this.createDateObject = function (a, b) {
        var c = j.$modelValue ? new Date(j.$modelValue) : null;
        return {
            date: a,
            label: g(a, b),
            selected: c && 0 === this.compare(a, c),
            disabled: this.isDisabled(a),
            current: 0 === this.compare(a, new Date)
        }
    }, this.isDisabled = function (c) {
        return this.minDate && this.compare(c, this.minDate) < 0 || this.maxDate && this.compare(c, this.maxDate) > 0 || b.dateDisabled && a.dateDisabled({
            date: c,
            mode: a.datepickerMode
        })
    }, this.split = function (a, b) {
        for (var c = []; a.length > 0;) c.push(a.splice(0, b));
        return c
    }, a.select = function (b) {
        if (a.datepickerMode === i.minMode) {
            var c = j.$modelValue ? new Date(j.$modelValue) : new Date(0, 0, 0, 0, 0, 0, 0);
            c.setFullYear(b.getFullYear(), b.getMonth(), b.getDate()), j.$setViewValue(c), j.$render()
        } else i.activeDate = b, a.datepickerMode = i.modes[i.modes.indexOf(a.datepickerMode) - 1]
    }, a.move = function (a) {
        var b = i.activeDate.getFullYear() + a * (i.step.years || 0),
            c = i.activeDate.getMonth() + a * (i.step.months || 0);
        i.activeDate.setFullYear(b, c, 1), i.refreshView()
    }, a.toggleMode = function (b) {
        b = b || 1, a.datepickerMode === i.maxMode && 1 === b || a.datepickerMode === i.minMode && -1 === b || (a.datepickerMode = i.modes[i.modes.indexOf(a.datepickerMode) + b])
    }, a.keys = {
        13: "enter",
        32: "space",
        33: "pageup",
        34: "pagedown",
        35: "end",
        36: "home",
        37: "left",
        38: "up",
        39: "right",
        40: "down"
    };
    var k = function () {
        e(function () {
            i.element[0].focus()
        }, 0, !1)
    };
    a.$on("datepicker.focus", k), a.keydown = function (b) {
        var c = a.keys[b.which];
        if (c && !b.shiftKey && !b.altKey) if (b.preventDefault(), b.stopPropagation(), "enter" === c || "space" === c) {
            if (i.isDisabled(i.activeDate)) return;
            a.select(i.activeDate), k()
        } else !b.ctrlKey || "up" !== c && "down" !== c ? (i.handleKeyDown(c, b), i.refreshView()) : (a.toggleMode("up" === c ? 1 : -1), k())
    }
}]).directive("datepicker", function () {
    return {
        restrict: "EA",
        replace: !0,
        templateUrl: "template/datepicker/datepicker.html",
        scope: {datepickerMode: "=?", dateDisabled: "&"},
        require: ["datepicker", "?^ngModel"],
        controller: "DatepickerController",
        link: function (a, b, c, d) {
            var e = d[0], f = d[1];
            f && e.init(f)
        }
    }
}).directive("daypicker", ["dateFilter", function (a) {
    return {
        restrict: "EA",
        replace: !0,
        templateUrl: "template/datepicker/day.html",
        require: "^datepicker",
        link: function (b, c, d, e) {
            function f(a, b) {
                return 1 !== b || a % 4 !== 0 || a % 100 === 0 && a % 400 !== 0 ? i[b] : 29
            }

            function g(a, b) {
                var c = new Array(b), d = new Date(a), e = 0;
                for (d.setHours(12); b > e;) c[e++] = new Date(d), d.setDate(d.getDate() + 1);
                return c
            }

            function h(a) {
                var b = new Date(a);
                b.setDate(b.getDate() + 4 - (b.getDay() || 7));
                var c = b.getTime();
                return b.setMonth(0), b.setDate(1), Math.floor(Math.round((c - b) / 864e5) / 7) + 1
            }

            b.showWeeks = e.showWeeks, e.step = {months: 1}, e.element = c;
            var i = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
            e._refreshView = function () {
                var c = e.activeDate.getFullYear(), d = e.activeDate.getMonth(), f = new Date(c, d, 1),
                    i = e.startingDay - f.getDay(), j = i > 0 ? 7 - i : -i, k = new Date(f);
                j > 0 && k.setDate(-j + 1);
                for (var l = g(k, 42), m = 0; 42 > m; m++) l[m] = angular.extend(e.createDateObject(l[m], e.formatDay), {
                    secondary: l[m].getMonth() !== d,
                    uid: b.uniqueId + "-" + m
                });
                b.labels = new Array(7);
                for (var n = 0; 7 > n; n++) b.labels[n] = {
                    abbr: a(l[n].date, e.formatDayHeader),
                    full: a(l[n].date, "EEEE")
                };
                if (b.title = a(e.activeDate, e.formatDayTitle), b.rows = e.split(l, 7), b.showWeeks) {
                    b.weekNumbers = [];
                    for (var o = h(b.rows[0][0].date), p = b.rows.length; b.weekNumbers.push(o++) < p;) ;
                }
            }, e.compare = function (a, b) {
                return new Date(a.getFullYear(), a.getMonth(), a.getDate()) - new Date(b.getFullYear(), b.getMonth(), b.getDate())
            }, e.handleKeyDown = function (a) {
                var b = e.activeDate.getDate();
                if ("left" === a) b -= 1; else if ("up" === a) b -= 7; else if ("right" === a) b += 1; else if ("down" === a) b += 7; else if ("pageup" === a || "pagedown" === a) {
                    var c = e.activeDate.getMonth() + ("pageup" === a ? -1 : 1);
                    e.activeDate.setMonth(c, 1), b = Math.min(f(e.activeDate.getFullYear(), e.activeDate.getMonth()), b)
                } else "home" === a ? b = 1 : "end" === a && (b = f(e.activeDate.getFullYear(), e.activeDate.getMonth()));
                e.activeDate.setDate(b)
            }, e.refreshView()
        }
    }
}]).directive("monthpicker", ["dateFilter", function (a) {
    return {
        restrict: "EA",
        replace: !0,
        templateUrl: "template/datepicker/month.html",
        require: "^datepicker",
        link: function (b, c, d, e) {
            e.step = {years: 1}, e.element = c, e._refreshView = function () {
                for (var c = new Array(12), d = e.activeDate.getFullYear(), f = 0; 12 > f; f++) c[f] = angular.extend(e.createDateObject(new Date(d, f, 1), e.formatMonth), {uid: b.uniqueId + "-" + f});
                b.title = a(e.activeDate, e.formatMonthTitle), b.rows = e.split(c, 3)
            }, e.compare = function (a, b) {
                return new Date(a.getFullYear(), a.getMonth()) - new Date(b.getFullYear(), b.getMonth())
            }, e.handleKeyDown = function (a) {
                var b = e.activeDate.getMonth();
                if ("left" === a) b -= 1; else if ("up" === a) b -= 3; else if ("right" === a) b += 1; else if ("down" === a) b += 3; else if ("pageup" === a || "pagedown" === a) {
                    var c = e.activeDate.getFullYear() + ("pageup" === a ? -1 : 1);
                    e.activeDate.setFullYear(c)
                } else "home" === a ? b = 0 : "end" === a && (b = 11);
                e.activeDate.setMonth(b)
            }, e.refreshView()
        }
    }
}]).directive("yearpicker", ["dateFilter", function () {
    return {
        restrict: "EA",
        replace: !0,
        templateUrl: "template/datepicker/year.html",
        require: "^datepicker",
        link: function (a, b, c, d) {
            function e(a) {
                return parseInt((a - 1) / f, 10) * f + 1
            }

            var f = d.yearRange;
            d.step = {years: f}, d.element = b, d._refreshView = function () {
                for (var b = new Array(f), c = 0, g = e(d.activeDate.getFullYear()); f > c; c++) b[c] = angular.extend(d.createDateObject(new Date(g + c, 0, 1), d.formatYear), {uid: a.uniqueId + "-" + c});
                a.title = [b[0].label, b[f - 1].label].join(" - "), a.rows = d.split(b, 5)
            }, d.compare = function (a, b) {
                return a.getFullYear() - b.getFullYear()
            }, d.handleKeyDown = function (a) {
                var b = d.activeDate.getFullYear();
                "left" === a ? b -= 1 : "up" === a ? b -= 5 : "right" === a ? b += 1 : "down" === a ? b += 5 : "pageup" === a || "pagedown" === a ? b += ("pageup" === a ? -1 : 1) * d.step.years : "home" === a ? b = e(d.activeDate.getFullYear()) : "end" === a && (b = e(d.activeDate.getFullYear()) + f - 1), d.activeDate.setFullYear(b)
            }, d.refreshView()
        }
    }
}]).constant("datepickerPopupConfig", {
    datepickerPopup: "yyyy-MM-dd",
    currentText: "Today",
    clearText: "Clear",
    closeText: "Done",
    closeOnDateSelection: !0,
    appendToBody: !1,
    showButtonBar: !0
}).directive("datepickerPopup", ["$compile", "$parse", "$document", "$position", "dateFilter", "dateParser", "datepickerPopupConfig", function (a, b, c, d, e, f, g) {
    return {
        restrict: "EA",
        require: "ngModel",
        scope: {isOpen: "=?", currentText: "@", clearText: "@", closeText: "@", dateDisabled: "&"},
        link: function (h, i, j, k) {
            function l(a) {
                return a.replace(/([A-Z])/g, function (a) {
                    return "-" + a.toLowerCase()
                })
            }

            function m(a) {
                if (a) {
                    if (angular.isDate(a) && !isNaN(a)) return k.$setValidity("date", !0), a;
                    if (angular.isString(a)) {
                        var b = f.parse(a, n) || new Date(a);
                        return isNaN(b) ? void k.$setValidity("date", !1) : (k.$setValidity("date", !0), b)
                    }
                    return void k.$setValidity("date", !1)
                }
                return k.$setValidity("date", !0), null
            }

            var n,
                o = angular.isDefined(j.closeOnDateSelection) ? h.$parent.$eval(j.closeOnDateSelection) : g.closeOnDateSelection,
                p = angular.isDefined(j.datepickerAppendToBody) ? h.$parent.$eval(j.datepickerAppendToBody) : g.appendToBody;
            h.showButtonBar = angular.isDefined(j.showButtonBar) ? h.$parent.$eval(j.showButtonBar) : g.showButtonBar, h.getText = function (a) {
                return h[a + "Text"] || g[a + "Text"]
            }, j.$observe("datepickerPopup", function (a) {
                n = a || g.datepickerPopup, k.$render()
            });
            var q = angular.element("<div datepicker-popup-wrap><div datepicker></div></div>");
            q.attr({"ng-model": "date", "ng-change": "dateSelection()"});
            var r = angular.element(q.children()[0]);
            j.datepickerOptions && angular.forEach(h.$parent.$eval(j.datepickerOptions), function (a, b) {
                r.attr(l(b), a)
            }), h.watchData = {}, angular.forEach(["minDate", "maxDate", "datepickerMode"], function (a) {
                if (j[a]) {
                    var c = b(j[a]);
                    if (h.$parent.$watch(c, function (b) {
                        h.watchData[a] = b
                    }), r.attr(l(a), "watchData." + a), "datepickerMode" === a) {
                        var d = c.assign;
                        h.$watch("watchData." + a, function (a, b) {
                            a !== b && d(h.$parent, a)
                        })
                    }
                }
            }), j.dateDisabled && r.attr("date-disabled", "dateDisabled({ date: date, mode: mode })"), k.$parsers.unshift(m), h.dateSelection = function (a) {
                angular.isDefined(a) && (h.date = a), k.$setViewValue(h.date), k.$render(), o && (h.isOpen = !1, i[0].focus())
            }, i.bind("input change keyup", function () {
                h.$apply(function () {
                    h.date = k.$modelValue
                })
            }), k.$render = function () {
                var a = k.$viewValue ? e(k.$viewValue, n) : "";
                i.val(a), h.date = m(k.$modelValue)
            };
            var s = function (a) {
                h.isOpen && a.target !== i[0] && h.$apply(function () {
                    h.isOpen = !1
                })
            }, t = function (a) {
                h.keydown(a)
            };
            i.bind("keydown", t), h.keydown = function (a) {
                27 === a.which ? (a.preventDefault(), a.stopPropagation(), h.close()) : 40 !== a.which || h.isOpen || (h.isOpen = !0)
            }, h.$watch("isOpen", function (a) {
                a ? (h.$broadcast("datepicker.focus"), h.position = p ? d.offset(i) : d.position(i), h.position.top = h.position.top + i.prop("offsetHeight"), c.bind("click", s)) : c.unbind("click", s)
            }), h.select = function (a) {
                if ("today" === a) {
                    var b = new Date;
                    angular.isDate(k.$modelValue) ? (a = new Date(k.$modelValue), a.setFullYear(b.getFullYear(), b.getMonth(), b.getDate())) : a = new Date(b.setHours(0, 0, 0, 0))
                }
                h.dateSelection(a)
            }, h.close = function () {
                h.isOpen = !1, i[0].focus()
            };
            var u = a(q)(h);
            q.remove(), p ? c.find("body").append(u) : i.after(u), h.$on("$destroy", function () {
                u.remove(), i.unbind("keydown", t), c.unbind("click", s)
            })
        }
    }
}]).directive("datepickerPopupWrap", function () {
    return {
        restrict: "EA",
        replace: !0,
        transclude: !0,
        templateUrl: "template/datepicker/popup.html",
        link: function (a, b) {
            b.bind("click", function (a) {
                a.preventDefault(), a.stopPropagation()
            })
        }
    }
}), angular.module("ui.bootstrap.dropdown", []).constant("dropdownConfig", {openClass: "open"}).service("dropdownService", ["$document", function (a) {
    var b = null;
    this.open = function (e) {
        b || (a.bind("click", c), a.bind("keydown", d)), b && b !== e && (b.isOpen = !1), b = e
    }, this.close = function (e) {
        b === e && (b = null, a.unbind("click", c), a.unbind("keydown", d))
    };
    var c = function (a) {
        if (b) {
            var c = b.getToggleElement();
            a && c && c[0].contains(a.target) || b.$apply(function () {
                b.isOpen = !1
            })
        }
    }, d = function (a) {
        27 === a.which && (b.focusToggleElement(), c())
    }
}]).controller("DropdownController", ["$scope", "$attrs", "$parse", "dropdownConfig", "dropdownService", "$animate", function (a, b, c, d, e, f) {
    var g, h = this, i = a.$new(), j = d.openClass, k = angular.noop, l = b.onToggle ? c(b.onToggle) : angular.noop;
    this.init = function (d) {
        h.$element = d, b.isOpen && (g = c(b.isOpen), k = g.assign, a.$watch(g, function (a) {
            i.isOpen = !!a
        }))
    }, this.toggle = function (a) {
        return i.isOpen = arguments.length ? !!a : !i.isOpen
    }, this.isOpen = function () {
        return i.isOpen
    }, i.getToggleElement = function () {
        return h.toggleElement
    }, i.focusToggleElement = function () {
        h.toggleElement && h.toggleElement[0].focus()
    }, i.$watch("isOpen", function (b, c) {
        f[b ? "addClass" : "removeClass"](h.$element, j), b ? (i.focusToggleElement(), e.open(i)) : e.close(i), k(a, b), angular.isDefined(b) && b !== c && l(a, {open: !!b})
    }), a.$on("$locationChangeSuccess", function () {
        i.isOpen = !1
    }), a.$on("$destroy", function () {
        i.$destroy()
    })
}]).directive("dropdown", function () {
    return {
        controller: "DropdownController", link: function (a, b, c, d) {
            d.init(b)
        }
    }
}).directive("dropdownToggle", function () {
    return {
        require: "?^dropdown", link: function (a, b, c, d) {
            if (d) {
                d.toggleElement = b;
                var e = function (e) {
                    e.preventDefault(), b.hasClass("disabled") || c.disabled || a.$apply(function () {
                        d.toggle()
                    })
                };
                b.bind("click", e), b.attr({
                    "aria-haspopup": !0,
                    "aria-expanded": !1
                }), a.$watch(d.isOpen, function (a) {
                    b.attr("aria-expanded", !!a)
                }), a.$on("$destroy", function () {
                    b.unbind("click", e)
                })
            }
        }
    }
}), angular.module("ui.bootstrap.modal", ["ui.bootstrap.transition"]).factory("$$stackedMap", function () {
    return {
        createNew: function () {
            var a = [];
            return {
                add: function (b, c) {
                    a.push({key: b, value: c})
                }, get: function (b) {
                    for (var c = 0; c < a.length; c++) if (b == a[c].key) return a[c]
                }, keys: function () {
                    for (var b = [], c = 0; c < a.length; c++) b.push(a[c].key);
                    return b
                }, top: function () {
                    return a[a.length - 1]
                }, remove: function (b) {
                    for (var c = -1, d = 0; d < a.length; d++) if (b == a[d].key) {
                        c = d;
                        break
                    }
                    return a.splice(c, 1)[0]
                }, removeTop: function () {
                    return a.splice(a.length - 1, 1)[0]
                }, length: function () {
                    return a.length
                }
            }
        }
    }
}).directive("modalBackdrop", ["$timeout", function (a) {
    return {
        restrict: "EA", replace: !0, templateUrl: "template/modal/backdrop.html", link: function (b, c, d) {
            b.backdropClass = d.backdropClass || "", b.animate = !1, a(function () {
                b.animate = !0
            })
        }
    }
}]).directive("modalWindow", ["$modalStack", "$timeout", function (a, b) {
    return {
        restrict: "EA",
        scope: {index: "@", animate: "="},
        replace: !0,
        transclude: !0,
        templateUrl: function (a, b) {
            return b.templateUrl || "template/modal/window.html"
        },
        link: function (c, d, e) {
            d.addClass(e.windowClass || ""), c.size = e.size, b(function () {
                c.animate = !0, d[0].querySelectorAll("[autofocus]").length || d[0].focus()
            }), c.close = function (b) {
                var c = a.getTop();
                c && c.value.backdrop && "static" != c.value.backdrop && b.target === b.currentTarget && (b.preventDefault(), b.stopPropagation(), a.dismiss(c.key, "backdrop click"))
            }
        }
    }
}]).directive("modalTransclude", function () {
    return {
        link: function (a, b, c, d, e) {
            e(a.$parent, function (a) {
                b.empty(), b.append(a)
            })
        }
    }
}).factory("$modalStack", ["$transition", "$timeout", "$document", "$compile", "$rootScope", "$$stackedMap", function (a, b, c, d, e, f) {
    function g() {
        for (var a = -1, b = n.keys(), c = 0; c < b.length; c++) n.get(b[c]).value.backdrop && (a = c);
        return a
    }

    function h(a) {
        var b = c.find("body").eq(0), d = n.get(a).value;
        n.remove(a), j(d.modalDomEl, d.modalScope, 300, function () {
            d.modalScope.$destroy(), b.toggleClass(m, n.length() > 0), i()
        })
    }

    function i() {
        if (k && -1 == g()) {
            var a = l;
            j(k, l, 150, function () {
                a.$destroy(), a = null
            }), k = void 0, l = void 0
        }
    }

    function j(c, d, e, f) {
        function g() {
            g.done || (g.done = !0, c.remove(), f && f())
        }

        d.animate = !1;
        var h = a.transitionEndEventName;
        if (h) {
            var i = b(g, e);
            c.bind(h, function () {
                b.cancel(i), g(), d.$apply()
            })
        } else b(g)
    }

    var k, l, m = "modal-open", n = f.createNew(), o = {};
    return e.$watch(g, function (a) {
        l && (l.index = a)
    }), c.bind("keydown", function (a) {
        var b;
        27 === a.which && (b = n.top(), b && b.value.keyboard && (a.preventDefault(), e.$apply(function () {
            o.dismiss(b.key, "escape key press")
        })))
    }), o.open = function (a, b) {
        n.add(a, {deferred: b.deferred, modalScope: b.scope, backdrop: b.backdrop, keyboard: b.keyboard});
        var f = c.find("body").eq(0), h = g();
        if (h >= 0 && !k) {
            l = e.$new(!0), l.index = h;
            var i = angular.element("<div modal-backdrop></div>");
            i.attr("backdrop-class", b.backdropClass), k = d(i)(l), f.append(k)
        }
        var j = angular.element("<div modal-window></div>");
        j.attr({
            "template-url": b.windowTemplateUrl,
            "window-class": b.windowClass,
            size: b.size,
            index: n.length() - 1,
            animate: "animate"
        }).html(b.content);
        var o = d(j)(b.scope);
        n.top().value.modalDomEl = o, f.append(o), f.addClass(m)
    }, o.close = function (a, b) {
        var c = n.get(a);
        c && (c.value.deferred.resolve(b), h(a))
    }, o.dismiss = function (a, b) {
        var c = n.get(a);
        c && (c.value.deferred.reject(b), h(a))
    }, o.dismissAll = function (a) {
        for (var b = this.getTop(); b;) this.dismiss(b.key, a), b = this.getTop()
    }, o.getTop = function () {
        return n.top()
    }, o
}]).provider("$modal", function () {
    var a = {
        options: {backdrop: !0, keyboard: !0},
        $get: ["$injector", "$rootScope", "$q", "$http", "$templateCache", "$controller", "$modalStack", function (b, c, d, e, f, g, h) {
            function i(a) {
                return a.template ? d.when(a.template) : e.get(angular.isFunction(a.templateUrl) ? a.templateUrl() : a.templateUrl, {cache: f}).then(function (a) {
                    return a.data
                })
            }

            function j(a) {
                var c = [];
                return angular.forEach(a, function (a) {
                    (angular.isFunction(a) || angular.isArray(a)) && c.push(d.when(b.invoke(a)))
                }), c
            }

            var k = {};
            return k.open = function (b) {
                var e = d.defer(), f = d.defer(), k = {
                    result: e.promise, opened: f.promise, close: function (a) {
                        h.close(k, a)
                    }, dismiss: function (a) {
                        h.dismiss(k, a)
                    }
                };
                if (b = angular.extend({}, a.options, b), b.resolve = b.resolve || {}, !b.template && !b.templateUrl) throw new Error("One of template or templateUrl options is required.");
                var l = d.all([i(b)].concat(j(b.resolve)));
                return l.then(function (a) {
                    var d = (b.scope || c).$new();
                    d.$close = k.close, d.$dismiss = k.dismiss;
                    var f, i = {}, j = 1;
                    b.controller && (i.$scope = d, i.$modalInstance = k, angular.forEach(b.resolve, function (b, c) {
                        i[c] = a[j++]
                    }), f = g(b.controller, i), b.controllerAs && (d[b.controllerAs] = f)), h.open(k, {
                        scope: d,
                        deferred: e,
                        content: a[0],
                        backdrop: b.backdrop,
                        keyboard: b.keyboard,
                        backdropClass: b.backdropClass,
                        windowClass: b.windowClass,
                        windowTemplateUrl: b.windowTemplateUrl,
                        size: b.size
                    })
                }, function (a) {
                    e.reject(a)
                }), l.then(function () {
                    f.resolve(!0)
                }, function () {
                    f.reject(!1)
                }), k
            }, k
        }]
    };
    return a
}), angular.module("ui.bootstrap.pagination", []).controller("PaginationController", ["$scope", "$attrs", "$parse", function (a, b, c) {
    var d = this, e = {$setViewValue: angular.noop}, f = b.numPages ? c(b.numPages).assign : angular.noop;
    this.init = function (f, g) {
        e = f, this.config = g, e.$render = function () {
            d.render()
        }, b.itemsPerPage ? a.$parent.$watch(c(b.itemsPerPage), function (b) {
            d.itemsPerPage = parseInt(b, 10), a.totalPages = d.calculateTotalPages()
        }) : this.itemsPerPage = g.itemsPerPage
    }, this.calculateTotalPages = function () {
        var b = this.itemsPerPage < 1 ? 1 : Math.ceil(a.totalItems / this.itemsPerPage);
        return Math.max(b || 0, 1)
    }, this.render = function () {
        a.page = parseInt(e.$viewValue, 10) || 1
    }, a.selectPage = function (b) {
        a.page !== b && b > 0 && b <= a.totalPages && (e.$setViewValue(b), e.$render())
    }, a.getText = function (b) {
        return a[b + "Text"] || d.config[b + "Text"]
    }, a.noPrevious = function () {
        return 1 === a.page
    }, a.noNext = function () {
        return a.page === a.totalPages
    }, a.$watch("totalItems", function () {
        a.totalPages = d.calculateTotalPages()
    }), a.$watch("totalPages", function (b) {
        f(a.$parent, b), a.page > b ? a.selectPage(b) : e.$render()
    })
}]).constant("paginationConfig", {
    itemsPerPage: 10,
    boundaryLinks: !1,
    directionLinks: !0,
    firstText: "First",
    previousText: "Previous",
    nextText: "Next",
    lastText: "Last",
    rotate: !0
}).directive("pagination", ["$parse", "paginationConfig", function (a, b) {
    return {
        restrict: "EA",
        scope: {totalItems: "=", firstText: "@", previousText: "@", nextText: "@", lastText: "@"},
        require: ["pagination", "?ngModel"],
        controller: "PaginationController",
        templateUrl: "template/pagination/pagination.html",
        replace: !0,
        link: function (c, d, e, f) {
            function g(a, b, c) {
                return {number: a, text: b, active: c}
            }

            function h(a, b) {
                var c = [], d = 1, e = b, f = angular.isDefined(k) && b > k;
                f && (l ? (d = Math.max(a - Math.floor(k / 2), 1), e = d + k - 1, e > b && (e = b, d = e - k + 1)) : (d = (Math.ceil(a / k) - 1) * k + 1, e = Math.min(d + k - 1, b)));
                for (var h = d; e >= h; h++) {
                    var i = g(h, h, h === a);
                    c.push(i)
                }
                if (f && !l) {
                    if (d > 1) {
                        var j = g(d - 1, "...", !1);
                        c.unshift(j)
                    }
                    if (b > e) {
                        var m = g(e + 1, "...", !1);
                        c.push(m)
                    }
                }
                return c
            }

            var i = f[0], j = f[1];
            if (j) {
                var k = angular.isDefined(e.maxSize) ? c.$parent.$eval(e.maxSize) : b.maxSize,
                    l = angular.isDefined(e.rotate) ? c.$parent.$eval(e.rotate) : b.rotate;
                c.boundaryLinks = angular.isDefined(e.boundaryLinks) ? c.$parent.$eval(e.boundaryLinks) : b.boundaryLinks, c.directionLinks = angular.isDefined(e.directionLinks) ? c.$parent.$eval(e.directionLinks) : b.directionLinks, i.init(j, b), e.maxSize && c.$parent.$watch(a(e.maxSize), function (a) {
                    k = parseInt(a, 10), i.render()
                });
                var m = i.render;
                i.render = function () {
                    m(), c.page > 0 && c.page <= c.totalPages && (c.pages = h(c.page, c.totalPages))
                }
            }
        }
    }
}]).constant("pagerConfig", {
    itemsPerPage: 10,
    previousText: "« Previous",
    nextText: "Next »",
    align: !0
}).directive("pager", ["pagerConfig", function (a) {
    return {
        restrict: "EA",
        scope: {totalItems: "=", previousText: "@", nextText: "@"},
        require: ["pager", "?ngModel"],
        controller: "PaginationController",
        templateUrl: "template/pagination/pager.html",
        replace: !0,
        link: function (b, c, d, e) {
            var f = e[0], g = e[1];
            g && (b.align = angular.isDefined(d.align) ? b.$parent.$eval(d.align) : a.align, f.init(g, a))
        }
    }
}]), angular.module("ui.bootstrap.tooltip", ["ui.bootstrap.position", "ui.bootstrap.bindHtml"]).provider("$tooltip", function () {
    function a(a) {
        var b = /[A-Z]/g, c = "-";
        return a.replace(b, function (a, b) {
            return (b ? c : "") + a.toLowerCase()
        })
    }

    var b = {placement: "top", animation: !0, popupDelay: 0},
        c = {mouseenter: "mouseleave", click: "click", focus: "blur"}, d = {};
    this.options = function (a) {
        angular.extend(d, a)
    }, this.setTriggers = function (a) {
        angular.extend(c, a)
    }, this.$get = ["$window", "$compile", "$timeout", "$document", "$position", "$interpolate", function (e, f, g, h, i, j) {
        return function (e, k, l) {
            function m(a) {
                var b = a || n.trigger || l, d = c[b] || b;
                return {show: b, hide: d}
            }

            var n = angular.extend({}, b, d), o = a(e), p = j.startSymbol(), q = j.endSymbol(),
                r = "<div " + o + '-popup title="' + p + "title" + q + '" content="' + p + "content" + q + '" placement="' + p + "placement" + q + '" animation="animation" is-open="isOpen"></div>';
            return {
                restrict: "EA", compile: function () {
                    var a = f(r);
                    return function (b, c, d) {
                        function f() {
                            D.isOpen ? l() : j()
                        }

                        function j() {
                            (!C || b.$eval(d[k + "Enable"])) && (s(), D.popupDelay ? z || (z = g(o, D.popupDelay, !1), z.then(function (a) {
                                a()
                            })) : o()())
                        }

                        function l() {
                            b.$apply(function () {
                                p()
                            })
                        }

                        function o() {
                            return z = null, y && (g.cancel(y), y = null), D.content ? (q(), w.css({
                                top: 0,
                                left: 0,
                                display: "block"
                            }), A ? h.find("body").append(w) : c.after(w), E(), D.isOpen = !0, D.$digest(), E) : angular.noop
                        }

                        function p() {
                            D.isOpen = !1, g.cancel(z), z = null, D.animation ? y || (y = g(r, 500)) : r()
                        }

                        function q() {
                            w && r(), x = D.$new(), w = a(x, angular.noop)
                        }

                        function r() {
                            y = null, w && (w.remove(), w = null), x && (x.$destroy(), x = null)
                        }

                        function s() {
                            t(), u()
                        }

                        function t() {
                            var a = d[k + "Placement"];
                            D.placement = angular.isDefined(a) ? a : n.placement
                        }

                        function u() {
                            var a = d[k + "PopupDelay"], b = parseInt(a, 10);
                            D.popupDelay = isNaN(b) ? n.popupDelay : b
                        }

                        function v() {
                            var a = d[k + "Trigger"];
                            F(), B = m(a), B.show === B.hide ? c.bind(B.show, f) : (c.bind(B.show, j), c.bind(B.hide, l))
                        }

                        var w, x, y, z, A = angular.isDefined(n.appendToBody) ? n.appendToBody : !1, B = m(void 0),
                            C = angular.isDefined(d[k + "Enable"]), D = b.$new(!0), E = function () {
                                var a = i.positionElements(c, w, D.placement, A);
                                a.top += "px", a.left += "px", w.css(a)
                            };
                        D.isOpen = !1, d.$observe(e, function (a) {
                            D.content = a, !a && D.isOpen && p()
                        }), d.$observe(k + "Title", function (a) {
                            D.title = a
                        });
                        var F = function () {
                            c.unbind(B.show, j), c.unbind(B.hide, l)
                        };
                        v();
                        var G = b.$eval(d[k + "Animation"]);
                        D.animation = angular.isDefined(G) ? !!G : n.animation;
                        var H = b.$eval(d[k + "AppendToBody"]);
                        A = angular.isDefined(H) ? H : A, A && b.$on("$locationChangeSuccess", function () {
                            D.isOpen && p()
                        }), b.$on("$destroy", function () {
                            g.cancel(y), g.cancel(z), F(), r(), D = null
                        })
                    }
                }
            }
        }
    }]
}).directive("tooltipPopup", function () {
    return {
        restrict: "EA",
        replace: !0,
        scope: {content: "@", placement: "@", animation: "&", isOpen: "&"},
        templateUrl: "template/tooltip/tooltip-popup.html"
    }
}).directive("tooltip", ["$tooltip", function (a) {
    return a("tooltip", "tooltip", "mouseenter")
}]).directive("tooltipHtmlUnsafePopup", function () {
    return {
        restrict: "EA",
        replace: !0,
        scope: {content: "@", placement: "@", animation: "&", isOpen: "&"},
        templateUrl: "template/tooltip/tooltip-html-unsafe-popup.html"
    }
}).directive("tooltipHtmlUnsafe", ["$tooltip", function (a) {
    return a("tooltipHtmlUnsafe", "tooltip", "mouseenter")
}]), angular.module("ui.bootstrap.popover", ["ui.bootstrap.tooltip"]).directive("popoverPopup", function () {
    return {
        restrict: "EA",
        replace: !0,
        scope: {title: "@", content: "@", placement: "@", animation: "&", isOpen: "&"},
        templateUrl: "template/popover/popover.html"
    }
}).directive("popover", ["$tooltip", function (a) {
    return a("popover", "popover", "click")
}]), angular.module("ui.bootstrap.progressbar", []).constant("progressConfig", {
    animate: !0,
    max: 100
}).controller("ProgressController", ["$scope", "$attrs", "progressConfig", function (a, b, c) {
    var d = this, e = angular.isDefined(b.animate) ? a.$parent.$eval(b.animate) : c.animate;
    this.bars = [], a.max = angular.isDefined(b.max) ? a.$parent.$eval(b.max) : c.max, this.addBar = function (b, c) {
        e || c.css({transition: "none"}), this.bars.push(b), b.$watch("value", function (c) {
            b.percent = +(100 * c / a.max).toFixed(2)
        }), b.$on("$destroy", function () {
            c = null, d.removeBar(b)
        })
    }, this.removeBar = function (a) {
        this.bars.splice(this.bars.indexOf(a), 1)
    }
}]).directive("progress", function () {
    return {
        restrict: "EA",
        replace: !0,
        transclude: !0,
        controller: "ProgressController",
        require: "progress",
        scope: {},
        templateUrl: "template/progressbar/progress.html"
    }
}).directive("bar", function () {
    return {
        restrict: "EA",
        replace: !0,
        transclude: !0,
        require: "^progress",
        scope: {value: "=", type: "@"},
        templateUrl: "template/progressbar/bar.html",
        link: function (a, b, c, d) {
            d.addBar(a, b)
        }
    }
}).directive("progressbar", function () {
    return {
        restrict: "EA",
        replace: !0,
        transclude: !0,
        controller: "ProgressController",
        scope: {value: "=", type: "@"},
        templateUrl: "template/progressbar/progressbar.html",
        link: function (a, b, c, d) {
            d.addBar(a, angular.element(b.children()[0]))
        }
    }
}), angular.module("ui.bootstrap.rating", []).constant("ratingConfig", {
    max: 5,
    stateOn: null,
    stateOff: null
}).controller("RatingController", ["$scope", "$attrs", "ratingConfig", function (a, b, c) {
    var d = {$setViewValue: angular.noop};
    this.init = function (e) {
        d = e, d.$render = this.render, this.stateOn = angular.isDefined(b.stateOn) ? a.$parent.$eval(b.stateOn) : c.stateOn, this.stateOff = angular.isDefined(b.stateOff) ? a.$parent.$eval(b.stateOff) : c.stateOff;
        var f = angular.isDefined(b.ratingStates) ? a.$parent.$eval(b.ratingStates) : new Array(angular.isDefined(b.max) ? a.$parent.$eval(b.max) : c.max);
        a.range = this.buildTemplateObjects(f)
    }, this.buildTemplateObjects = function (a) {
        for (var b = 0, c = a.length; c > b; b++) a[b] = angular.extend({index: b}, {
            stateOn: this.stateOn,
            stateOff: this.stateOff
        }, a[b]);
        return a
    }, a.rate = function (b) {
        !a.readonly && b >= 0 && b <= a.range.length && (d.$setViewValue(b), d.$render())
    }, a.enter = function (b) {
        a.readonly || (a.value = b), a.onHover({value: b})
    }, a.reset = function () {
        a.value = d.$viewValue, a.onLeave()
    }, a.onKeydown = function (b) {
        /(37|38|39|40)/.test(b.which) && (b.preventDefault(), b.stopPropagation(), a.rate(a.value + (38 === b.which || 39 === b.which ? 1 : -1)))
    }, this.render = function () {
        a.value = d.$viewValue
    }
}]).directive("rating", function () {
    return {
        restrict: "EA",
        require: ["rating", "ngModel"],
        scope: {readonly: "=?", onHover: "&", onLeave: "&"},
        controller: "RatingController",
        templateUrl: "template/rating/rating.html",
        replace: !0,
        link: function (a, b, c, d) {
            var e = d[0], f = d[1];
            f && e.init(f)
        }
    }
}), angular.module("ui.bootstrap.tabs", []).controller("TabsetController", ["$scope", function (a) {
    var b = this, c = b.tabs = a.tabs = [];
    b.select = function (a) {
        angular.forEach(c, function (b) {
            b.active && b !== a && (b.active = !1, b.onDeselect())
        }), a.active = !0, a.onSelect()
    }, b.addTab = function (a) {
        c.push(a), 1 === c.length ? a.active = !0 : a.active && b.select(a)
    }, b.removeTab = function (a) {
        var e = c.indexOf(a);
        if (a.active && c.length > 1 && !d) {
            var f = e == c.length - 1 ? e - 1 : e + 1;
            b.select(c[f])
        }
        c.splice(e, 1)
    };
    var d;
    a.$on("$destroy", function () {
        d = !0
    })
}]).directive("tabset", function () {
    return {
        restrict: "EA",
        transclude: !0,
        replace: !0,
        scope: {type: "@"},
        controller: "TabsetController",
        templateUrl: "template/tabs/tabset.html",
        link: function (a, b, c) {
            a.vertical = angular.isDefined(c.vertical) ? a.$parent.$eval(c.vertical) : !1, a.justified = angular.isDefined(c.justified) ? a.$parent.$eval(c.justified) : !1
        }
    }
}).directive("tab", ["$parse", function (a) {
    return {
        require: "^tabset",
        restrict: "EA",
        replace: !0,
        templateUrl: "template/tabs/tab.html",
        transclude: !0,
        scope: {active: "=?", heading: "@", onSelect: "&select", onDeselect: "&deselect"},
        controller: function () {
        },
        compile: function (b, c, d) {
            return function (b, c, e, f) {
                b.$watch("active", function (a) {
                    a && f.select(b)
                }), b.disabled = !1, e.disabled && b.$parent.$watch(a(e.disabled), function (a) {
                    b.disabled = !!a
                }), b.select = function () {
                    b.disabled || (b.active = !0)
                }, f.addTab(b), b.$on("$destroy", function () {
                    f.removeTab(b)
                }), b.$transcludeFn = d
            }
        }
    }
}]).directive("tabHeadingTransclude", [function () {
    return {
        restrict: "A", require: "^tab", link: function (a, b) {
            a.$watch("headingElement", function (a) {
                a && (b.html(""), b.append(a))
            })
        }
    }
}]).directive("tabContentTransclude", function () {
    function a(a) {
        return a.tagName && (a.hasAttribute("tab-heading") || a.hasAttribute("data-tab-heading") || "tab-heading" === a.tagName.toLowerCase() || "data-tab-heading" === a.tagName.toLowerCase())
    }

    return {
        restrict: "A", require: "^tabset", link: function (b, c, d) {
            var e = b.$eval(d.tabContentTransclude);
            e.$transcludeFn(e.$parent, function (b) {
                angular.forEach(b, function (b) {
                    a(b) ? e.headingElement = b : c.append(b)
                })
            })
        }
    }
}), angular.module("ui.bootstrap.timepicker", []).constant("timepickerConfig", {
    hourStep: 1,
    minuteStep: 1,
    showMeridian: !0,
    meridians: null,
    readonlyInput: !1,
    mousewheel: !0
}).controller("TimepickerController", ["$scope", "$attrs", "$parse", "$log", "$locale", "timepickerConfig", function (a, b, c, d, e, f) {
    function g() {
        var b = parseInt(a.hours, 10), c = a.showMeridian ? b > 0 && 13 > b : b >= 0 && 24 > b;
        return c ? (a.showMeridian && (12 === b && (b = 0), a.meridian === p[1] && (b += 12)), b) : void 0
    }

    function h() {
        var b = parseInt(a.minutes, 10);
        return b >= 0 && 60 > b ? b : void 0
    }

    function i(a) {
        return angular.isDefined(a) && a.toString().length < 2 ? "0" + a : a
    }

    function j(a) {
        k(), o.$setViewValue(new Date(n)), l(a)
    }

    function k() {
        o.$setValidity("time", !0), a.invalidHours = !1, a.invalidMinutes = !1
    }

    function l(b) {
        var c = n.getHours(), d = n.getMinutes();
        a.showMeridian && (c = 0 === c || 12 === c ? 12 : c % 12), a.hours = "h" === b ? c : i(c), a.minutes = "m" === b ? d : i(d), a.meridian = n.getHours() < 12 ? p[0] : p[1]
    }

    function m(a) {
        var b = new Date(n.getTime() + 6e4 * a);
        n.setHours(b.getHours(), b.getMinutes()), j()
    }

    var n = new Date, o = {$setViewValue: angular.noop},
        p = angular.isDefined(b.meridians) ? a.$parent.$eval(b.meridians) : f.meridians || e.DATETIME_FORMATS.AMPMS;
    this.init = function (c, d) {
        o = c, o.$render = this.render;
        var e = d.eq(0), g = d.eq(1),
            h = angular.isDefined(b.mousewheel) ? a.$parent.$eval(b.mousewheel) : f.mousewheel;
        h && this.setupMousewheelEvents(e, g), a.readonlyInput = angular.isDefined(b.readonlyInput) ? a.$parent.$eval(b.readonlyInput) : f.readonlyInput, this.setupInputEvents(e, g)
    };
    var q = f.hourStep;
    b.hourStep && a.$parent.$watch(c(b.hourStep), function (a) {
        q = parseInt(a, 10)
    });
    var r = f.minuteStep;
    b.minuteStep && a.$parent.$watch(c(b.minuteStep), function (a) {
        r = parseInt(a, 10)
    }), a.showMeridian = f.showMeridian, b.showMeridian && a.$parent.$watch(c(b.showMeridian), function (b) {
        if (a.showMeridian = !!b, o.$error.time) {
            var c = g(), d = h();
            angular.isDefined(c) && angular.isDefined(d) && (n.setHours(c), j())
        } else l()
    }), this.setupMousewheelEvents = function (b, c) {
        var d = function (a) {
            a.originalEvent && (a = a.originalEvent);
            var b = a.wheelDelta ? a.wheelDelta : -a.deltaY;
            return a.detail || b > 0
        };
        b.bind("mousewheel wheel", function (b) {
            a.$apply(d(b) ? a.incrementHours() : a.decrementHours()), b.preventDefault()
        }), c.bind("mousewheel wheel", function (b) {
            a.$apply(d(b) ? a.incrementMinutes() : a.decrementMinutes()), b.preventDefault()
        })
    }, this.setupInputEvents = function (b, c) {
        if (a.readonlyInput) return a.updateHours = angular.noop, void (a.updateMinutes = angular.noop);
        var d = function (b, c) {
            o.$setViewValue(null), o.$setValidity("time", !1), angular.isDefined(b) && (a.invalidHours = b), angular.isDefined(c) && (a.invalidMinutes = c)
        };
        a.updateHours = function () {
            var a = g();
            angular.isDefined(a) ? (n.setHours(a), j("h")) : d(!0)
        }, b.bind("blur", function () {
            !a.invalidHours && a.hours < 10 && a.$apply(function () {
                a.hours = i(a.hours)
            })
        }), a.updateMinutes = function () {
            var a = h();
            angular.isDefined(a) ? (n.setMinutes(a), j("m")) : d(void 0, !0)
        }, c.bind("blur", function () {
            !a.invalidMinutes && a.minutes < 10 && a.$apply(function () {
                a.minutes = i(a.minutes)
            })
        })
    }, this.render = function () {
        var a = o.$modelValue ? new Date(o.$modelValue) : null;
        isNaN(a) ? (o.$setValidity("time", !1), d.error('Timepicker directive: "ng-model" value must be a Date object, a number of milliseconds since 01.01.1970 or a string representing an RFC2822 or ISO 8601 date.')) : (a && (n = a), k(), l())
    }, a.incrementHours = function () {
        m(60 * q)
    }, a.decrementHours = function () {
        m(60 * -q)
    }, a.incrementMinutes = function () {
        m(r)
    }, a.decrementMinutes = function () {
        m(-r)
    }, a.toggleMeridian = function () {
        m(720 * (n.getHours() < 12 ? 1 : -1))
    }
}]).directive("timepicker", function () {
    return {
        restrict: "EA",
        require: ["timepicker", "?^ngModel"],
        controller: "TimepickerController",
        replace: !0,
        scope: {},
        templateUrl: "template/timepicker/timepicker.html",
        link: function (a, b, c, d) {
            var e = d[0], f = d[1];
            f && e.init(f, b.find("input"))
        }
    }
}), angular.module("ui.bootstrap.typeahead", ["ui.bootstrap.position", "ui.bootstrap.bindHtml"]).factory("typeaheadParser", ["$parse", function (a) {
    var b = /^\s*([\s\S]+?)(?:\s+as\s+([\s\S]+?))?\s+for\s+(?:([\$\w][\$\w\d]*))\s+in\s+([\s\S]+?)$/;
    return {
        parse: function (c) {
            var d = c.match(b);
            if (!d) throw new Error('Expected typeahead specification in form of "_modelValue_ (as _label_)? for _item_ in _collection_" but got "' + c + '".');
            return {itemName: d[3], source: a(d[4]), viewMapper: a(d[2] || d[1]), modelMapper: a(d[1])}
        }
    }
}]).directive("typeahead", ["$compile", "$parse", "$q", "$timeout", "$document", "$position", "typeaheadParser", function (a, b, c, d, e, f, g) {
    var h = [9, 13, 27, 38, 40];
    return {
        require: "ngModel", link: function (i, j, k, l) {
            var m, n = i.$eval(k.typeaheadMinLength) || 1, o = i.$eval(k.typeaheadWaitMs) || 0,
                p = i.$eval(k.typeaheadEditable) !== !1, q = b(k.typeaheadLoading).assign || angular.noop,
                r = b(k.typeaheadOnSelect), s = k.typeaheadInputFormatter ? b(k.typeaheadInputFormatter) : void 0,
                t = k.typeaheadAppendToBody ? i.$eval(k.typeaheadAppendToBody) : !1,
                u = i.$eval(k.typeaheadFocusFirst) !== !1, v = b(k.ngModel).assign, w = g.parse(k.typeahead),
                x = i.$new();
            i.$on("$destroy", function () {
                x.$destroy()
            });
            var y = "typeahead-" + x.$id + "-" + Math.floor(1e4 * Math.random());
            j.attr({"aria-autocomplete": "list", "aria-expanded": !1, "aria-owns": y});
            var z = angular.element("<div typeahead-popup></div>");
            z.attr({
                id: y,
                matches: "matches",
                active: "activeIdx",
                select: "select(activeIdx)",
                query: "query",
                position: "position"
            }), angular.isDefined(k.typeaheadTemplateUrl) && z.attr("template-url", k.typeaheadTemplateUrl);
            var A = function () {
                x.matches = [], x.activeIdx = -1, j.attr("aria-expanded", !1)
            }, B = function (a) {
                return y + "-option-" + a
            };
            x.$watch("activeIdx", function (a) {
                0 > a ? j.removeAttr("aria-activedescendant") : j.attr("aria-activedescendant", B(a))
            });
            var C = function (a) {
                var b = {$viewValue: a};
                q(i, !0), c.when(w.source(i, b)).then(function (c) {
                    var d = a === l.$viewValue;
                    if (d && m) if (c.length > 0) {
                        x.activeIdx = u ? 0 : -1, x.matches.length = 0;
                        for (var e = 0; e < c.length; e++) b[w.itemName] = c[e], x.matches.push({
                            id: B(e),
                            label: w.viewMapper(x, b),
                            model: c[e]
                        });
                        x.query = a, x.position = t ? f.offset(j) : f.position(j), x.position.top = x.position.top + j.prop("offsetHeight"), j.attr("aria-expanded", !0)
                    } else A();
                    d && q(i, !1)
                }, function () {
                    A(), q(i, !1)
                })
            };
            A(), x.query = void 0;
            var D, E = function (a) {
                D = d(function () {
                    C(a)
                }, o)
            }, F = function () {
                D && d.cancel(D)
            };
            l.$parsers.unshift(function (a) {
                return m = !0, a && a.length >= n ? o > 0 ? (F(), E(a)) : C(a) : (q(i, !1), F(), A()), p ? a : a ? void l.$setValidity("editable", !1) : (l.$setValidity("editable", !0), a)
            }), l.$formatters.push(function (a) {
                var b, c, d = {};
                return s ? (d.$model = a, s(i, d)) : (d[w.itemName] = a, b = w.viewMapper(i, d), d[w.itemName] = void 0, c = w.viewMapper(i, d), b !== c ? b : a)
            }), x.select = function (a) {
                var b, c, e = {};
                e[w.itemName] = c = x.matches[a].model, b = w.modelMapper(i, e), v(i, b), l.$setValidity("editable", !0), r(i, {
                    $item: c,
                    $model: b,
                    $label: w.viewMapper(i, e)
                }), A(), d(function () {
                    j[0].focus()
                }, 0, !1)
            }, j.bind("keydown", function (a) {
                0 !== x.matches.length && -1 !== h.indexOf(a.which) && (-1 != x.activeIdx || 13 !== a.which && 9 !== a.which) && (a.preventDefault(), 40 === a.which ? (x.activeIdx = (x.activeIdx + 1) % x.matches.length, x.$digest()) : 38 === a.which ? (x.activeIdx = (x.activeIdx > 0 ? x.activeIdx : x.matches.length) - 1, x.$digest()) : 13 === a.which || 9 === a.which ? x.$apply(function () {
                    x.select(x.activeIdx)
                }) : 27 === a.which && (a.stopPropagation(), A(), x.$digest()))
            }), j.bind("blur", function () {
                m = !1
            });
            var G = function (a) {
                j[0] !== a.target && (A(), x.$digest())
            };
            e.bind("click", G), i.$on("$destroy", function () {
                e.unbind("click", G), t && H.remove()
            });
            var H = a(z)(x);
            t ? e.find("body").append(H) : j.after(H)
        }
    }
}]).directive("typeaheadPopup", function () {
    return {
        restrict: "EA",
        scope: {matches: "=", query: "=", active: "=", position: "=", select: "&"},
        replace: !0,
        templateUrl: "template/typeahead/typeahead-popup.html",
        link: function (a, b, c) {
            a.templateUrl = c.templateUrl, a.isOpen = function () {
                return a.matches.length > 0
            }, a.isActive = function (b) {
                return a.active == b
            }, a.selectActive = function (b) {
                a.active = b
            }, a.selectMatch = function (b) {
                a.select({activeIdx: b})
            }
        }
    }
}).directive("typeaheadMatch", ["$http", "$templateCache", "$compile", "$parse", function (a, b, c, d) {
    return {
        restrict: "EA", scope: {index: "=", match: "=", query: "="}, link: function (e, f, g) {
            var h = d(g.templateUrl)(e.$parent) || "template/typeahead/typeahead-match.html";
            a.get(h, {cache: b}).success(function (a) {
                f.replaceWith(c(a.trim())(e))
            })
        }
    }
}]).filter("typeaheadHighlight", function () {
    function a(a) {
        return a.replace(/([.?*+^$[\]\\(){}|-])/g, "\\$1")
    }

    return function (b, c) {
        return c ? ("" + b).replace(new RegExp(a(c), "gi"), "<strong>$&</strong>") : b
    }
}), angular.module("template/accordion/accordion-group.html", []).run(["$templateCache", function (a) {
    a.put("template/accordion/accordion-group.html", '<div class="panel panel-default">\n  <div class="panel-heading">\n    <h4 class="panel-title">\n      <a href class="accordion-toggle" ng-click="toggleOpen()" accordion-transclude="heading"><span ng-class="{\'text-muted\': isDisabled}">{{heading}}</span></a>\n    </h4>\n  </div>\n  <div class="panel-collapse" collapse="!isOpen">\n	  <div class="panel-body" ng-transclude></div>\n  </div>\n</div>\n')
}]), angular.module("template/accordion/accordion.html", []).run(["$templateCache", function (a) {
    a.put("template/accordion/accordion.html", '<div class="panel-group" ng-transclude></div>')
}]), angular.module("template/alert/alert.html", []).run(["$templateCache", function (a) {
    a.put("template/alert/alert.html", '<div class="alert" ng-class="[\'alert-\' + (type || \'warning\'), closeable ? \'alert-dismissable\' : null]" role="alert">\n    <button ng-show="closeable" type="button" class="close" ng-click="close()">\n        <span aria-hidden="true">&times;</span>\n        <span class="sr-only">Close</span>\n    </button>\n    <div ng-transclude></div>\n</div>\n')
}]), angular.module("template/carousel/carousel.html", []).run(["$templateCache", function (a) {
    a.put("template/carousel/carousel.html", '<div ng-mouseenter="pause()" ng-mouseleave="play()" class="carousel" ng-swipe-right="prev()" ng-swipe-left="next()">\n    <ol class="carousel-indicators" ng-show="slides.length > 1">\n        <li ng-repeat="slide in slides track by $index" ng-class="{active: isActive(slide)}" ng-click="select(slide)"></li>\n    </ol>\n    <div class="carousel-inner" ng-transclude></div>\n    <a class="left carousel-control" ng-click="prev()" ng-show="slides.length > 1"><span class="glyphicon glyphicon-chevron-left"></span></a>\n    <a class="right carousel-control" ng-click="next()" ng-show="slides.length > 1"><span class="glyphicon glyphicon-chevron-right"></span></a>\n</div>\n')
}]), angular.module("template/carousel/slide.html", []).run(["$templateCache", function (a) {
    a.put("template/carousel/slide.html", "<div ng-class=\"{\n    'active': leaving || (active && !entering),\n    'prev': (next || active) && direction=='prev',\n    'next': (next || active) && direction=='next',\n    'right': direction=='prev',\n    'left': direction=='next'\n  }\" class=\"item text-center\" ng-transclude></div>\n")
}]), angular.module("template/datepicker/datepicker.html", []).run(["$templateCache", function (a) {
    a.put("template/datepicker/datepicker.html", '<div ng-switch="datepickerMode" role="application" ng-keydown="keydown($event)">\n  <daypicker ng-switch-when="day" tabindex="0"></daypicker>\n  <monthpicker ng-switch-when="month" tabindex="0"></monthpicker>\n  <yearpicker ng-switch-when="year" tabindex="0"></yearpicker>\n</div>')
}]), angular.module("template/datepicker/day.html", []).run(["$templateCache", function (a) {
    a.put("template/datepicker/day.html", '<table role="grid" aria-labelledby="{{uniqueId}}-title" aria-activedescendant="{{activeDateId}}">\n  <thead>\n    <tr>\n      <th><button type="button" class="btn btn-default btn-sm pull-left" ng-click="move(-1)" tabindex="-1"><i class="glyphicon glyphicon-chevron-left"></i></button></th>\n      <th colspan="{{5 + showWeeks}}"><button id="{{uniqueId}}-title" role="heading" aria-live="assertive" aria-atomic="true" type="button" class="btn btn-default btn-sm" ng-click="toggleMode()" tabindex="-1" style="width:100%;"><strong>{{title}}</strong></button></th>\n      <th><button type="button" class="btn btn-default btn-sm pull-right" ng-click="move(1)" tabindex="-1"><i class="glyphicon glyphicon-chevron-right"></i></button></th>\n    </tr>\n    <tr>\n      <th ng-show="showWeeks" class="text-center"></th>\n      <th ng-repeat="label in labels track by $index" class="text-center"><small aria-label="{{label.full}}">{{label.abbr}}</small></th>\n    </tr>\n  </thead>\n  <tbody>\n    <tr ng-repeat="row in rows track by $index">\n      <td ng-show="showWeeks" class="text-center h6"><em>{{ weekNumbers[$index] }}</em></td>\n      <td ng-repeat="dt in row track by dt.date" class="text-center" role="gridcell" id="{{dt.uid}}" aria-disabled="{{!!dt.disabled}}">\n        <button type="button" style="width:100%;" class="btn btn-default btn-sm" ng-class="{\'btn-info\': dt.selected, active: isActive(dt)}" ng-click="select(dt.date)" ng-disabled="dt.disabled" tabindex="-1"><span ng-class="{\'text-muted\': dt.secondary, \'text-info\': dt.current}">{{dt.label}}</span></button>\n      </td>\n    </tr>\n  </tbody>\n</table>\n')
}]), angular.module("template/datepicker/month.html", []).run(["$templateCache", function (a) {
    a.put("template/datepicker/month.html", '<table role="grid" aria-labelledby="{{uniqueId}}-title" aria-activedescendant="{{activeDateId}}">\n  <thead>\n    <tr>\n      <th><button type="button" class="btn btn-default btn-sm pull-left" ng-click="move(-1)" tabindex="-1"><i class="glyphicon glyphicon-chevron-left"></i></button></th>\n      <th><button id="{{uniqueId}}-title" role="heading" aria-live="assertive" aria-atomic="true" type="button" class="btn btn-default btn-sm" ng-click="toggleMode()" tabindex="-1" style="width:100%;"><strong>{{title}}</strong></button></th>\n      <th><button type="button" class="btn btn-default btn-sm pull-right" ng-click="move(1)" tabindex="-1"><i class="glyphicon glyphicon-chevron-right"></i></button></th>\n    </tr>\n  </thead>\n  <tbody>\n    <tr ng-repeat="row in rows track by $index">\n      <td ng-repeat="dt in row track by dt.date" class="text-center" role="gridcell" id="{{dt.uid}}" aria-disabled="{{!!dt.disabled}}">\n        <button type="button" style="width:100%;" class="btn btn-default" ng-class="{\'btn-info\': dt.selected, active: isActive(dt)}" ng-click="select(dt.date)" ng-disabled="dt.disabled" tabindex="-1"><span ng-class="{\'text-info\': dt.current}">{{dt.label}}</span></button>\n      </td>\n    </tr>\n  </tbody>\n</table>\n')
}]), angular.module("template/datepicker/popup.html", []).run(["$templateCache", function (a) {
    a.put("template/datepicker/popup.html", '<ul class="dropdown-menu" ng-style="{display: (isOpen && \'block\') || \'none\', top: position.top+\'px\', left: position.left+\'px\'}" ng-keydown="keydown($event)">\n	<li ng-transclude></li>\n	<li ng-if="showButtonBar" style="padding:10px 9px 2px">\n		<span class="btn-group pull-left">\n			<button type="button" class="btn btn-sm btn-info" ng-click="select(\'today\')">{{ getText(\'current\') }}</button>\n			<button type="button" class="btn btn-sm btn-danger" ng-click="select(null)">{{ getText(\'clear\') }}</button>\n		</span>\n		<button type="button" class="btn btn-sm btn-success pull-right" ng-click="close()">{{ getText(\'close\') }}</button>\n	</li>\n</ul>\n')
}]), angular.module("template/datepicker/year.html", []).run(["$templateCache", function (a) {
    a.put("template/datepicker/year.html", '<table role="grid" aria-labelledby="{{uniqueId}}-title" aria-activedescendant="{{activeDateId}}">\n  <thead>\n    <tr>\n      <th><button type="button" class="btn btn-default btn-sm pull-left" ng-click="move(-1)" tabindex="-1"><i class="glyphicon glyphicon-chevron-left"></i></button></th>\n      <th colspan="3"><button id="{{uniqueId}}-title" role="heading" aria-live="assertive" aria-atomic="true" type="button" class="btn btn-default btn-sm" ng-click="toggleMode()" tabindex="-1" style="width:100%;"><strong>{{title}}</strong></button></th>\n      <th><button type="button" class="btn btn-default btn-sm pull-right" ng-click="move(1)" tabindex="-1"><i class="glyphicon glyphicon-chevron-right"></i></button></th>\n    </tr>\n  </thead>\n  <tbody>\n    <tr ng-repeat="row in rows track by $index">\n      <td ng-repeat="dt in row track by dt.date" class="text-center" role="gridcell" id="{{dt.uid}}" aria-disabled="{{!!dt.disabled}}">\n        <button type="button" style="width:100%;" class="btn btn-default" ng-class="{\'btn-info\': dt.selected, active: isActive(dt)}" ng-click="select(dt.date)" ng-disabled="dt.disabled" tabindex="-1"><span ng-class="{\'text-info\': dt.current}">{{dt.label}}</span></button>\n      </td>\n    </tr>\n  </tbody>\n</table>\n')
}]), angular.module("template/modal/backdrop.html", []).run(["$templateCache", function (a) {
    a.put("template/modal/backdrop.html", '<div class="modal-backdrop fade {{ backdropClass }}"\n     ng-class="{in: animate}"\n     ng-style="{\'z-index\': 1040 + (index && 1 || 0) + index*10}"\n></div>\n')
}]), angular.module("template/modal/window.html", []).run(["$templateCache", function (a) {
    a.put("template/modal/window.html", '<div tabindex="-1" role="dialog" class="modal fade" ng-class="{in: animate}" ng-style="{\'z-index\': 1050 + index*10, display: \'block\'}" ng-click="close($event)">\n    <div class="modal-dialog" ng-class="{\'modal-sm\': size == \'sm\', \'modal-lg\': size == \'lg\'}"><div class="modal-content" modal-transclude></div></div>\n</div>')
}]), angular.module("template/pagination/pager.html", []).run(["$templateCache", function (a) {
    a.put("template/pagination/pager.html", '<ul class="pager">\n  <li ng-class="{disabled: noPrevious(), previous: align}"><a href ng-click="selectPage(page - 1)">{{getText(\'previous\')}}</a></li>\n  <li ng-class="{disabled: noNext(), next: align}"><a href ng-click="selectPage(page + 1)">{{getText(\'next\')}}</a></li>\n</ul>')
}]), angular.module("template/pagination/pagination.html", []).run(["$templateCache", function (a) {
    a.put("template/pagination/pagination.html", '<ul class="pagination">\n  <li ng-if="boundaryLinks" ng-class="{disabled: noPrevious()}"><a href ng-click="selectPage(1)">{{getText(\'first\')}}</a></li>\n  <li ng-if="directionLinks" ng-class="{disabled: noPrevious()}"><a href ng-click="selectPage(page - 1)">{{getText(\'previous\')}}</a></li>\n  <li ng-repeat="page in pages track by $index" ng-class="{active: page.active}"><a href ng-click="selectPage(page.number)">{{page.text}}</a></li>\n  <li ng-if="directionLinks" ng-class="{disabled: noNext()}"><a href ng-click="selectPage(page + 1)">{{getText(\'next\')}}</a></li>\n  <li ng-if="boundaryLinks" ng-class="{disabled: noNext()}"><a href ng-click="selectPage(totalPages)">{{getText(\'last\')}}</a></li>\n</ul>')
}]), angular.module("template/tooltip/tooltip-html-unsafe-popup.html", []).run(["$templateCache", function (a) {
    a.put("template/tooltip/tooltip-html-unsafe-popup.html", '<div class="tooltip {{placement}}" ng-class="{ in: isOpen(), fade: animation() }">\n  <div class="tooltip-arrow"></div>\n  <div class="tooltip-inner" bind-html-unsafe="content"></div>\n</div>\n')
}]), angular.module("template/tooltip/tooltip-popup.html", []).run(["$templateCache", function (a) {
    a.put("template/tooltip/tooltip-popup.html", '<div class="tooltip {{placement}}" ng-class="{ in: isOpen(), fade: animation() }">\n  <div class="tooltip-arrow"></div>\n  <div class="tooltip-inner" ng-bind="content"></div>\n</div>\n')
}]), angular.module("template/popover/popover.html", []).run(["$templateCache", function (a) {
    a.put("template/popover/popover.html", '<div class="popover {{placement}}" ng-class="{ in: isOpen(), fade: animation() }">\n  <div class="arrow"></div>\n\n  <div class="popover-inner">\n      <h3 class="popover-title" ng-bind="title" ng-show="title"></h3>\n      <div class="popover-content" ng-bind="content"></div>\n  </div>\n</div>\n')
}]), angular.module("template/progressbar/bar.html", []).run(["$templateCache", function (a) {
    a.put("template/progressbar/bar.html", '<div class="progress-bar" ng-class="type && \'progress-bar-\' + type" role="progressbar" aria-valuenow="{{value}}" aria-valuemin="0" aria-valuemax="{{max}}" ng-style="{width: percent + \'%\'}" aria-valuetext="{{percent | number:0}}%" ng-transclude></div>')
}]), angular.module("template/progressbar/progress.html", []).run(["$templateCache", function (a) {
    a.put("template/progressbar/progress.html", '<div class="progress" ng-transclude></div>')
}]), angular.module("template/progressbar/progressbar.html", []).run(["$templateCache", function (a) {
    a.put("template/progressbar/progressbar.html", '<div class="progress">\n  <div class="progress-bar" ng-class="type && \'progress-bar-\' + type" role="progressbar" aria-valuenow="{{value}}" aria-valuemin="0" aria-valuemax="{{max}}" ng-style="{width: percent + \'%\'}" aria-valuetext="{{percent | number:0}}%" ng-transclude></div>\n</div>')
}]), angular.module("template/rating/rating.html", []).run(["$templateCache", function (a) {
    a.put("template/rating/rating.html", '<span ng-mouseleave="reset()" ng-keydown="onKeydown($event)" tabindex="0" role="slider" aria-valuemin="0" aria-valuemax="{{range.length}}" aria-valuenow="{{value}}">\n    <i ng-repeat="r in range track by $index" ng-mouseenter="enter($index + 1)" ng-click="rate($index + 1)" class="glyphicon" ng-class="$index < value && (r.stateOn || \'glyphicon-star\') || (r.stateOff || \'glyphicon-star-empty\')">\n        <span class="sr-only">({{ $index < value ? \'*\' : \' \' }})</span>\n    </i>\n</span>')
}]), angular.module("template/tabs/tab.html", []).run(["$templateCache", function (a) {
    a.put("template/tabs/tab.html", '<li ng-class="{active: active, disabled: disabled}">\n  <a href ng-click="select()" tab-heading-transclude>{{heading}}</a>\n</li>\n')
}]), angular.module("template/tabs/tabset.html", []).run(["$templateCache", function (a) {
    a.put("template/tabs/tabset.html", '<div>\n  <ul class="nav nav-{{type || \'tabs\'}}" ng-class="{\'nav-stacked\': vertical, \'nav-justified\': justified}" ng-transclude></ul>\n  <div class="tab-content">\n    <div class="tab-pane" \n         ng-repeat="tab in tabs" \n         ng-class="{active: tab.active}"\n         tab-content-transclude="tab">\n    </div>\n  </div>\n</div>\n')
}]), angular.module("template/timepicker/timepicker.html", []).run(["$templateCache", function (a) {
    a.put("template/timepicker/timepicker.html", '<table>\n	<tbody>\n		<tr class="text-center">\n			<td><a ng-click="incrementHours()" class="btn btn-link"><span class="glyphicon glyphicon-chevron-up"></span></a></td>\n			<td>&nbsp;</td>\n			<td><a ng-click="incrementMinutes()" class="btn btn-link"><span class="glyphicon glyphicon-chevron-up"></span></a></td>\n			<td ng-show="showMeridian"></td>\n		</tr>\n		<tr>\n			<td style="width:50px;" class="form-group" ng-class="{\'has-error\': invalidHours}">\n				<input type="text" ng-model="hours" ng-change="updateHours()" class="form-control text-center" ng-mousewheel="incrementHours()" ng-readonly="readonlyInput" maxlength="2">\n			</td>\n			<td>:</td>\n			<td style="width:50px;" class="form-group" ng-class="{\'has-error\': invalidMinutes}">\n				<input type="text" ng-model="minutes" ng-change="updateMinutes()" class="form-control text-center" ng-readonly="readonlyInput" maxlength="2">\n			</td>\n			<td ng-show="showMeridian"><button type="button" class="btn btn-default text-center" ng-click="toggleMeridian()">{{meridian}}</button></td>\n		</tr>\n		<tr class="text-center">\n			<td><a ng-click="decrementHours()" class="btn btn-link"><span class="glyphicon glyphicon-chevron-down"></span></a></td>\n			<td>&nbsp;</td>\n			<td><a ng-click="decrementMinutes()" class="btn btn-link"><span class="glyphicon glyphicon-chevron-down"></span></a></td>\n			<td ng-show="showMeridian"></td>\n		</tr>\n	</tbody>\n</table>\n')
}]), angular.module("template/typeahead/typeahead-match.html", []).run(["$templateCache", function (a) {
    a.put("template/typeahead/typeahead-match.html", '<a tabindex="-1" bind-html-unsafe="match.label | typeaheadHighlight:query"></a>')
}]), angular.module("template/typeahead/typeahead-popup.html", []).run(["$templateCache", function (a) {
    a.put("template/typeahead/typeahead-popup.html", '<ul class="dropdown-menu" ng-show="isOpen()" ng-style="{top: position.top+\'px\', left: position.left+\'px\'}" style="display: block;" role="listbox" aria-hidden="{{!isOpen()}}">\n    <li ng-repeat="match in matches track by $index" ng-class="{active: isActive($index) }" ng-mouseenter="selectActive($index)" ng-click="selectMatch($index)" role="option" id="{{match.id}}">\n        <div typeahead-match index="$index" match="match" query="query" template-url="templateUrl"></div>\n    </li>\n</ul>\n')
}]);
!function (n, e, t) {
    "use strict";

    function r(n, e, r, u) {
        if (n === t || null === n || "" === n) return 0;
        var o = "";
        return o = "," === e ? String(n).replace(".", ",") : String(n), a(o, r, u)
    }

    function u(n, e, t) {
        return "," === e ? String(n).replace(/['\.\s]/g, "").replace(",", ".") : "." === e ? String(n).replace(/[',\s]/g, "") : void 0
    }

    function a(n, e, t) {
        var r = n;
        return t && (r += t), e && (r = /^\-.+/.test(r) ? r.replace("-", "-" + e) : /^\-/.test(r) ? r : e + r), r
    }

    function o(n, e) {
        if (n >= 0) {
            var t = parseInt(n, 10);
            if (isNaN(t) === !1 && isFinite(t) && t >= 0) return t
        }
        return e
    }

    function i(n, e) {
        if (n >= 0) {
            var t = parseInt(n, 10);
            if (isNaN(t) === !1 && isFinite(t) && t >= 0) return t
        }
        return e
    }

    function c(n, e) {
        return "," === n ? "," : "." === n ? "." : e
    }

    function m(n, e) {
        return "false" === n || n === !1 ? !1 : "true" === n || n === !0 ? !0 : e
    }

    function p(n, e) {
        return "false" === n || n === !1 ? !1 : "true" === n || n === !0 ? !0 : e
    }

    function s(n, e) {
        return "floor" === n ? Math.floor : "ceil" === n ? Math.ceil : "round" === n ? Math.round : e
    }

    function d(n, e) {
        return "false" === n || n === !1 ? !1 : "true" === n || n === !0 ? !0 : e
    }

    function f(n, e) {
        return "false" === n || n === !1 ? !1 : "true" === n || n === !0 ? !0 : e
    }

    function g(n, e, t) {
        if (!n) return t;
        var r;
        return r = "." === e ? new RegExp("^[',\\s]$") : new RegExp("^['\\.\\s]$"), r.test(n) ? n : t
    }

    function l(n) {
        var e = new RegExp("[^\\d,\\.\\s\\-]{1}");
        return e.test(n) ? n : null
    }

    function S(n, e, t, r, u) {
        var a = "-?";
        r === !1 && u === !0 ? a = "-" : r === !0 && u === !1 && (a = "");
        var o = "[0-9]{0," + n + "}";
        0 === n && (o = "0");
        var i = "(\\" + t + "([0-9]){0," + e + "})";
        return 0 === e && (i = ""), new RegExp("^" + a + o + i + "?$")
    }

    function h(n) {
        return String(n).replace(/^0+/g, "").replace(/^-0(\d+)/g, "-$1").replace(new RegExp("^-([\\.,\\s])", "g"), "-0$1").replace(new RegExp("^[\\.,\\s]", "g"), "0$&")
    }

    function v(n, e, t) {
        var r = n;
        return e && (r = r.replace(new RegExp("[\\" + e + "]", "g"), "")), t && (r = r.replace(new RegExp("[\\" + t + "]", "g"), "")), r
    }

    function w(n, e) {
        return "." === e ? String(n).replace(/\./g, "") : "," === e ? String(n).replace(/,/g, "") : String(n).replace(new RegExp("['\\s]", "g"), "")
    }

    function $(n, e, t) {
        return n = String(n).split(e), n[0] = n[0].replace(/\B(?=(\d{3})+(?!\d))/g, t), n.join(e)
    }

    function N(n, e, t, r, u, o) {
        o && (u.enable = !1), n.$setViewValue(a(String(e), t, r)), n.$render()
    }

    function R(n, e, u, a, o, i, c, m, p) {
        if ("" === n || n === t || null === n) return "";
        if (n = Number(n), !isNaN(n) && isFinite(n)) {
            var s = Math.pow(10, e);
            return n = o ? r((a(n * s) / s).toFixed(e), u, m, p) : r(String(a(n * s) / s), u, m, p), i && (n = $(n, u, c)), n
        }
        return o ? 0..toFixed(e) : "0"
    }

    function T(n) {
        var e = 0;
        if (document.selection) {
            n.focus();
            var t = document.selection.createRange();
            t.moveStart("character", -n.value.length), e = t.text.length
        } else (n.selectionStart || "0" == n.selectionStart) && (e = "backward" == n.selectionDirection ? n.selectionStart : n.selectionEnd);
        return e
    }

    function P(n, e) {
        if (null !== n) if (n.createTextRange) {
            var t = n.createTextRange();
            t.move("character", e), t.select()
        } else n.selectionStart ? (n.focus(), n.setSelectionRange(e, e)) : n.focus()
    }

    function x(n, e, t) {
        for (var r = 0, u = 0, a = 0; a < n.length; a++) if (n[a] !== e) {
            if (r++, r >= t) break
        } else u++;
        return u
    }

    function b(n) {
        return Number(n)
    }

    function F(n, e, t) {
        var r = {
            awnum: n.awnum,
            numInt: n.numInt,
            numFract: n.numFract,
            numSep: n.numSep,
            numPos: n.numPos,
            numNeg: n.numNeg,
            numRound: n.numRound,
            numThousand: n.numThousand,
            numThousandSep: n.numThousandSep,
            numPrepend: n.numPrepend,
            numAppend: n.numAppend
        };
        return e && (r[e] = t), r
    }

    function y(n, e, r, u, a) {
        var d = {};
        n.awnum && (d = a.getStrategy(n.awnum));
        var h = o(n.numInt !== t ? n.numInt : d.numInt, 6), v = i(n.numFract !== t ? n.numFract : d.numFract, 2),
            w = c(n.numSep !== t ? n.numSep : d.numSep, "."), $ = m(n.numPos !== t ? n.numPos : d.numPos, !0),
            N = p(n.numNeg !== t ? n.numNeg : d.numNeg, !0),
            R = s(n.numRound !== t ? n.numRound : d.numRound, Math.round),
            T = f(n.numThousand !== t ? n.numThousand : d.numThousand, !1),
            P = g(n.numThousandSep !== t ? n.numThousandSep : d.numThousandSep, w, "." === w ? "," : "."),
            x = l(n.numPrepend !== t ? n.numPrepend : d.numPrepend),
            b = l(n.numAppend !== t ? n.numAppend : d.numAppend);
        if ($ === !1 && N === !1) throw new Error("Number is set to not be positive and not be negative. Change num_pos attr or/and num_neg attr to true");
        var F = S(h, v, w, $, N);
        return {
            element: e,
            attrs: r,
            ngModelController: u,
            viewRegexTest: F,
            integerPart: h,
            fractionPart: v,
            fractionSeparator: w,
            isPositiveNumber: $,
            isNegativeNumber: N,
            roundFunction: R,
            isThousandSeparator: T,
            thousandSeparator: P,
            prepend: x,
            append: b
        }
    }

    function E(n, e, a) {
        var o = e.element, i = (e.attrs, e.ngModelController), c = e.viewRegexTest, m = (e.integerPart, e.fractionPart),
            p = e.fractionSeparator, s = e.isPositiveNumber, d = e.isNegativeNumber,
            f = (e.roundFunction, e.isThousandSeparator), g = e.thousandSeparator, l = e.prepend, S = e.append,
            R = String(n);
        if (V) {
            V = !1;
            var b = new RegExp("[^" + (d ? "-" : "") + p + g + "0-9]+", "g");
            R = R.replace(b, ""), b = new RegExp("^[" + p + g + "]"), R = R.replace(b, ""), b = new RegExp("[" + p + g + "]([0-9]{" + m + "})$"), R = R.replace(b, p + "$1")
        }
        if (R = v(R, l, S), new RegExp("^[.," + g + "]{2,}").test(R)) return N(i, 0, l, S, a), 0;
        var F = T(o[0]);
        l && F--;
        var y = R.slice(0, F);
        y = w(y, g), R = w(R, g), y = h(y);
        var E = R;
        if (R = h(R), R === "0" + p && E === p && s) return m ? (N(i, "0" + p, l, S, a, !0), P(o[0], 2), 0) : (N(i, "", l, S, a), 0);
        if ("" === R && "0" === String(n).charAt(0)) return N(i, 0, l, S), 0;
        if (R === t || "" === R) return 0;
        if ("-" === R) return s && !d ? N(i, "", l, S, a) : N(i, "-", l, S, a), 0;
        if (c.test(R) === !1) {
            var A = r(i.$modelValue, p, l, S);
            return f && (A = $(A, p, g)), N(i, A, l, S, a), P(o[0], F - 1), i.$modelValue
        }
        var I = 0, M = y.length;
        return f && (R = $(R, p, g), I = x(R, g, M)), l && (I++, new RegExp("^(\\-\\d)$").test(R) && (I += 2), new RegExp("^(\\d)$").test(R) && I++), N(i, R, l, S, a), P(o[0], M + I), u(R, p, g)
    }

    function A(n, e) {
        n.$setViewValue(""), n.$render(), n.$setViewValue(e), n.$render()
    }

    function I(n, e) {
        var t = R(n.$modelValue, e.fractionPart, e.fractionSeparator, e.roundFunction, !1, e.isThousandSeparator, e.thousandSeparator, e.prepend, e.append);
        A(n, t)
    }

    function M(n) {
        return {
            restrict: "A",
            require: "?ngModel",
            scope: {
                awnum: "@",
                numInt: "@",
                numFract: "@",
                numSep: "@",
                numPos: "@",
                numNeg: "@",
                numRound: "@",
                numThousand: "@",
                numThousandSep: "@",
                numPrepend: "@",
                numAppend: "@"
            },
            link: function (e, t, r, u) {
                if (!t[0] || "INPUT" !== t[0].tagName || "text" !== t[0].type && "tel" !== t[0].type) return void console.warn("Directive angular-dynamic-number works only for 'input' tag with type = 'text' or type = 'tel'");
                if (!u) return void console.warn("Directive angular-dynamic-number need ngModel attribute");
                var a = y(F(e), t, r, u, n);
                t.on("paste", function () {
                    V = !0
                }), e.$watch("numInt", function (o, i) {
                    i !== o && (a = y(F(e, "numInt", o), t, r, u, n), I(u, a))
                }), e.$watch("numFract", function (o, i) {
                    i !== o && (a = y(F(e, "numFract", o), t, r, u, n), I(u, a))
                }), e.$watch("numSep", function (o, i) {
                    i !== o && (a = y(F(e, "numSep", o), t, r, u, n), I(u, a))
                }), e.$watch("numPos", function (o, i) {
                    i !== o && (a = y(F(e, "numPos", o), t, r, u, n), I(u, a))
                }), e.$watch("numNeg", function (o, i) {
                    i !== o && (a = y(F(e, "numNeg", o), t, r, u, n), I(u, a))
                }), e.$watch("numThousand", function (o, i) {
                    i !== o && (a = y(F(e, "numThousand", o), t, r, u, n), I(u, a))
                }), e.$watch("numThousandSep", function (o, i) {
                    i !== o && (a = y(F(e, "numThousandSep", o), t, r, u, n), I(u, a))
                }), e.$watch("numAppend", function (o, i) {
                    i !== o && (a = y(F(e, "numAppend", o), t, r, u, n), I(u, a))
                }), e.$watch("numPrepend", function (o, i) {
                    i !== o && (a = y(F(e, "numPrepend", o), t, r, u, n), I(u, a))
                });
                var o = {enable: !0, count: 0};
                u.$parsers.unshift(function (n) {
                    return o.enable ? (o.count++, b(E(n, a, o))) : (o.enable = !0, n)
                }), u.$formatters.push(function (n) {
                    return R(n, a.fractionPart, a.fractionSeparator, a.roundFunction, !1, a.isThousandSeparator, a.thousandSeparator, a.prepend, a.append)
                })
            }
        }
    }

    var V = !1;
    e.module("dynamicNumber", []).provider("dynamicNumberStrategy", function () {
        var n = {};
        this.addStrategy = function (e, t) {
            n[e] = t
        }, this.getStrategy = function (e) {
            return n[e]
        }, this.$get = function () {
            return {
                getStrategy: function (e) {
                    return n[e]
                }
            }
        }
    }).filter("awnum", function (n) {
        return function (r, u, a, o, m, p, S, h, v) {
            var w, $ = {};
            e.isString(u) && ($ = n.getStrategy(u), u = $.numFract);
            var w = i(u, 2), N = c(a !== t ? a : $.numSep, "."), T = s(o !== t ? o : $.numRound, Math.round),
                P = d(m !== t ? m : $.numFixed, !1), x = f(p !== t ? p : $.numThousand, !1),
                b = g(S !== t ? S : $.numThousandSep, N, "." === N ? "," : "."), F = l(h !== t ? h : $.numPrepend),
                y = l(v !== t ? v : $.numAppend), E = R(r, w, N, T, P, x, b, F, y);
            return "" === E ? "0" : E
        }
    }).directive("awnum", ["dynamicNumberStrategy", M])
}(window, window.angular);
var topupApp = angular.module("topupApp", ["ui.bootstrap", "ngSanitize", "ngRoute", "dynamicNumber"]);
topupApp.config(['dynamicNumberStrategyProvider', function (dynamicNumberStrategyProvider) {
    dynamicNumberStrategyProvider.addStrategy('price', {
        numInt: 6,
        numFract: 4,
        numSep: '.',
        numPos: true,
        numNeg: false,
        numRound: 'round',
        numThousand: true,
        numThousandSep: ' '
    });
}]);

function getParameterByName(name, url) {
    if (!url) {
        url = window.location.href;
    }
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}

function removeunicode(elem) {
    $(elem).val($(elem).val().replace(/[^A-Za-z0-9\.,\?""!@#\$%\^&\*\(\)-_=\+;:<>\/\\\|\}\{\[\]`~]*/g, ''));
}

function validateNumber(event) {
    var key = window.event ? event.keyCode : event.which;
    if (event.keyCode === 8 || event.keyCode === 46) {
        return true;
    } else if (key < 48 || key > 57) {
        return false;
    } else {
        return true;
    }
}

function isCharacterValid(str) {
    return !/[~`!#$%\^&*+=\-\[\]\\';,/{}|\\":<>\?]/g.test(str);
}

function setCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}

function readCookie(name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
    }
    return null;
}

function deleteCookie(name) {
    console.log(name);
    document.cookie = name + '=;expires=Thu, 01 Jan 1970 00:00:01 GMT;';
}

topupApp.directive("ngTopMenu", function ($compile) {
    return {
        restrict: "A",
        terminal: true,
        priority: 1000,
        link: function (scope, element, attrs) {
            $.ajax({
                url: "/get-top-menu",
                data: {
                    currentUrl: scope.CurrentUrl
                },
                type: "POST",
                beforeSend: function () {
                },
                complete: function () {
                },
                success: function (data) {
                    if (data.Code === '00') {
                        element.append(data.Data);
                        if ($(window).width() > 1199) {
                            $('.dropdown-sub--hover > a').css('font-size', 'inherit');
                            $('.dropdown-sub--hover__a').removeAttr('data-toggle');
                        }
                    }
                },
                error: ""
            });
        }
    }
});

topupApp.directive("ngRightAds", function ($compile) {
    return {
        restrict: "A",
        terminal: true,
        priority: 1000,
        link: function (scope, element, attrs) {
            var target = $(element);
            var fromsource = "";
            if (target.is('[ng-topup]')) {
                fromsource = "Tin-noi-bat-nap-tien-dien-thoai";
            }
            if (target.is('[ng-bill]')) {
                fromsource = "Tin-noi-bat-hoa-don";
            }
            if (target.is('[ng-cardshopping]')) {
                fromsource = "Tin-noi-bat-mua-ma-the";
            }
            if (target.is('[ng-booking]')) {
                fromsource = "Tin-noi-bat-ve-may-bay";
            }
            if (target.is('[ng-fecredit]')) {
                fromsource = "Tin-noi-bat-Fecredit";
            }
            $(document).ready(function () {
                $.ajax({
                    url: "/get-right-ads",
                    data: {
                        fromSource: fromsource
                    },
                    type: "POST",
                    beforeSend: function () {
                    },
                    complete: function () {
                    },
                    success: function (data) {
                        if (data.Code === '00') {
                            element.append(data.Data);
                        }
                    },
                    error: ""
                });
            });
        }
    }
});

topupApp.directive('numbersOnly', function () {
    return {
        require: 'ngModel',
        link: function (scope, element, attr, ngModelCtrl) {
            function fromUser(text) {
                if (text) {
                    var transformedInput = text.replace(/[^0-9]/g, '');

                    if (transformedInput !== text) {
                        ngModelCtrl.$setViewValue(transformedInput);
                        ngModelCtrl.$render();
                    }
                    return transformedInput;
                }
                return undefined;
            }

            ngModelCtrl.$parsers.push(fromUser);
        }
    };
});

topupApp.directive('alphabetViOnly', function () {
    return {
        require: 'ngModel',
        link: function (scope, element, attr, ngModelCtrl) {
            function fromUser(text) {
                if (text) {
                    var transformedInput = text.replace(/[^A-Za-z0-9àáạảãâầấậẩẫăằắặẳẵèéẹẻẽêềếệểễìíịỉĩòóọỏõôồốộổỗơờớợởỡùúụủũưừứựửữỳýỵỷỹđÀÁẠẢÃÂẦẤẬẨẪĂẰẮẶẲẴÈÉẸẺẼÊỀẾỆỂỄÌÍỊỈĨÒÓỌỎÕÔỒỐỘỔỖƠỜỚỢỞỠÙÚỤỦŨƯỪỨỰỬỮỲÝỴỶỸĐ\s]/g, '');

                    if (transformedInput !== text) {
                        ngModelCtrl.$setViewValue(transformedInput);
                        ngModelCtrl.$render();
                    }
                    return transformedInput;
                }
                return undefined;
            }

            ngModelCtrl.$parsers.push(fromUser);
        }
    };
});

topupApp.directive('alphabetEnOnly', function () {
    return {
        require: 'ngModel',
        link: function (scope, element, attr, ngModelCtrl) {
            function fromUser(text) {
                if (text) {
                    var transformedInput = text.replace(/[^A-Za-z0-9]/g, '').trim();

                    ngModelCtrl.$setViewValue(transformedInput);
                    ngModelCtrl.$render();
                    return transformedInput;
                }
                return undefined;
            }

            ngModelCtrl.$parsers.push(fromUser);
        }
    };
});

topupApp.directive("ngChangeSelect2", function () {
    return {
        link: function (scope, element, attr) {
            var target = $(element);
            target.on("change", function (e) {
                target.valid();
            });
        }
    };
});
