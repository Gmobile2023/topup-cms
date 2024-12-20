!function () {
    "use strict";

    function f(n) {
        n.fn.swiper = function (i) {
            var r;
            return n(this).each(function () {
                var n = new t(this, i);
                r || (r = n)
            }), r
        }
    }

    var n, t = function (i, r) {
            function b(n) {
                return Math.floor(n)
            }

            function rt() {
                var n = u.params.autoplay,
                    t = u.slides.eq(u.activeIndex);
                t.attr("data-swiper-autoplay") && (n = t.attr("data-swiper-autoplay") || u.params.autoplay);
                u.autoplayTimeoutId = setTimeout(function () {
                    u.params.loop ? (u.fixLoop(), u._slideNext(), u.emit("onAutoplay", u)) : u.isEnd ? r.autoplayStopOnLast ? u.stopAutoplay() : (u._slideTo(0), u.emit("onAutoplay", u)) : (u._slideNext(), u.emit("onAutoplay", u))
                }, n)
            }

            function ut(t, i) {
                var r = n(t.target),
                    u;
                if (!r.is(i))
                    if ("string" == typeof i) r = r.parents(i);
                    else if (i.nodeType) return r.parents().each(function (n, t) {
                        t === i && (u = i)
                    }), u ? i : void 0;
                if (0 !== r.length) return r[0]
            }

            function ft(n, t) {
                t = t || {};
                var r = window.MutationObserver || window.WebkitMutationObserver,
                    i = new r(function (n) {
                        n.forEach(function (n) {
                            u.onResize(!0);
                            u.emit("onObserverUpdate", u, n)
                        })
                    });
                i.observe(n, {
                    attributes: "undefined" == typeof t.attributes || t.attributes,
                    childList: "undefined" == typeof t.childList || t.childList,
                    characterData: "undefined" == typeof t.characterData || t.characterData
                });
                u.observers.push(i)
            }

            function vt(n) {
                var t, o, s, e, r;
                if ((n.originalEvent && (n = n.originalEvent), t = n.keyCode || n.charCode, !u.params.allowSwipeToNext && (u.isHorizontal() && 39 === t || !u.isHorizontal() && 40 === t)) || !u.params.allowSwipeToPrev && (u.isHorizontal() && 37 === t || !u.isHorizontal() && 38 === t)) return !1;
                if (!(n.shiftKey || n.altKey || n.ctrlKey || n.metaKey || document.activeElement && document.activeElement.nodeName && ("input" === document.activeElement.nodeName.toLowerCase() || "textarea" === document.activeElement.nodeName.toLowerCase()))) {
                    if (37 === t || 39 === t || 38 === t || 40 === t) {
                        if (o = !1, u.container.parents("." + u.params.slideClass).length > 0 && 0 === u.container.parents("." + u.params.slideActiveClass).length) return;
                        var f = {
                                left: window.pageXOffset,
                                top: window.pageYOffset
                            },
                            h = window.innerWidth,
                            c = window.innerHeight,
                            i = u.container.offset();
                        for (u.rtl && (i.left = i.left - u.container[0].scrollLeft), s = [
                            [i.left, i.top],
                            [i.left + u.width, i.top],
                            [i.left, i.top + u.height],
                            [i.left + u.width, i.top + u.height]
                        ], e = 0; e < s.length; e++) r = s[e], r[0] >= f.left && r[0] <= f.left + h && r[1] >= f.top && r[1] <= f.top + c && (o = !0);
                        if (!o) return
                    }
                    u.isHorizontal() ? (37 !== t && 39 !== t || (n.preventDefault ? n.preventDefault() : n.returnValue = !1), (39 === t && !u.rtl || 37 === t && u.rtl) && u.slideNext(), (37 === t && !u.rtl || 39 === t && u.rtl) && u.slidePrev()) : (38 !== t && 40 !== t || (n.preventDefault ? n.preventDefault() : n.returnValue = !1), 40 === t && u.slideNext(), 38 === t && u.slidePrev())
                }
            }

            function bt() {
                var t = "onwheel",
                    n = t in document,
                    i;
                return n || (i = document.createElement("div"), i.setAttribute(t, "return;"), n = "function" == typeof i[t]), !n && document.implementation && document.implementation.hasFeature && document.implementation.hasFeature("", "") !== !0 && (n = document.implementation.hasFeature("Events.wheel", "3.0")), n
            }

            function yt(n) {
                n.originalEvent && (n = n.originalEvent);
                var i = 0,
                    f = u.rtl ? -1 : 1,
                    t = kt(n);
                if (u.params.mousewheelForceToAxis)
                    if (u.isHorizontal()) {
                        if (!(Math.abs(t.pixelX) > Math.abs(t.pixelY))) return;
                        i = t.pixelX * f
                    } else {
                        if (!(Math.abs(t.pixelY) > Math.abs(t.pixelX))) return;
                        i = t.pixelY
                    }
                else i = Math.abs(t.pixelX) > Math.abs(t.pixelY) ? -t.pixelX * f : -t.pixelY;
                if (0 !== i) {
                    if (u.params.mousewheelInvert && (i = -i), u.params.freeMode) {
                        var r = u.getWrapperTranslate() + i * u.params.mousewheelSensitivity,
                            e = u.isBeginning,
                            o = u.isEnd;
                        if (r >= u.minTranslate() && (r = u.minTranslate()), r <= u.maxTranslate() && (r = u.maxTranslate()), u.setWrapperTransition(0), u.setWrapperTranslate(r), u.updateProgress(), u.updateActiveIndex(), (!e && u.isBeginning || !o && u.isEnd) && u.updateClasses(), u.params.freeModeSticky ? (clearTimeout(u.mousewheel.timeout), u.mousewheel.timeout = setTimeout(function () {
                            u.slideReset()
                        }, 300)) : u.params.lazyLoading && u.lazy && u.lazy.load(), u.emit("onScroll", u, n), u.params.autoplay && u.params.autoplayDisableOnInteraction && u.stopAutoplay(), 0 === r || r === u.maxTranslate()) return
                    } else {
                        if ((new window.Date).getTime() - u.mousewheel.lastScrollTime > 60)
                            if (i < 0)
                                if (u.isEnd && !u.params.loop || u.animating) {
                                    if (u.params.mousewheelReleaseOnEdges) return !0
                                } else u.slideNext(), u.emit("onScroll", u, n);
                            else if (u.isBeginning && !u.params.loop || u.animating) {
                                if (u.params.mousewheelReleaseOnEdges) return !0
                            } else u.slidePrev(), u.emit("onScroll", u, n);
                        u.mousewheel.lastScrollTime = (new window.Date).getTime()
                    }
                    return n.preventDefault ? n.preventDefault() : n.returnValue = !1, !1
                }
            }

            function kt(n) {
                var f = 10,
                    e = 40,
                    o = 800,
                    u = 0,
                    t = 0,
                    i = 0,
                    r = 0;
                return "detail" in n && (t = n.detail), "wheelDelta" in n && (t = -n.wheelDelta / 120), "wheelDeltaY" in n && (t = -n.wheelDeltaY / 120), "wheelDeltaX" in n && (u = -n.wheelDeltaX / 120), "axis" in n && n.axis === n.HORIZONTAL_AXIS && (u = t, t = 0), i = u * f, r = t * f, "deltaY" in n && (r = n.deltaY), "deltaX" in n && (i = n.deltaX), (i || r) && n.deltaMode && (1 === n.deltaMode ? (i *= e, r *= e) : (i *= o, r *= o)), i && !u && (u = i < 1 ? -1 : 1), r && !t && (t = r < 1 ? -1 : 1), {
                    spinX: u,
                    spinY: t,
                    pixelX: i,
                    pixelY: r
                }
            }

            function pt(t, i) {
                t = n(t);
                var e, r, f, o = u.rtl ? -1 : 1;
                e = t.attr("data-swiper-parallax") || "0";
                r = t.attr("data-swiper-parallax-x");
                f = t.attr("data-swiper-parallax-y");
                r || f ? (r = r || "0", f = f || "0") : u.isHorizontal() ? (r = e, f = "0") : (f = e, r = "0");
                r = r.indexOf("%") >= 0 ? parseInt(r, 10) * i * o + "%" : r * i * o + "px";
                f = f.indexOf("%") >= 0 ? parseInt(f, 10) * i + "%" : f * i + "px";
                t.transform("translate3d(" + r + ", " + f + ",0px)")
            }

            function et(n) {
                return 0 !== n.indexOf("on") && (n = n[0] !== n[0].toUpperCase() ? "on" + n[0].toUpperCase() + n.substring(1) : "on" + n), n
            }

            var v, wt, y, e, ot, s, k, u, st, w, it, lt, at;
            if (!(this instanceof t)) return new t(i, r);
            v = {
                direction: "horizontal",
                touchEventsTarget: "container",
                initialSlide: 0,
                speed: 300,
                autoplay: !1,
                autoplayDisableOnInteraction: !0,
                autoplayStopOnLast: !1,
                iOSEdgeSwipeDetection: !1,
                iOSEdgeSwipeThreshold: 20,
                freeMode: !1,
                freeModeMomentum: !0,
                freeModeMomentumRatio: 1,
                freeModeMomentumBounce: !0,
                freeModeMomentumBounceRatio: 1,
                freeModeMomentumVelocityRatio: 1,
                freeModeSticky: !1,
                freeModeMinimumVelocity: .02,
                autoHeight: !1,
                setWrapperSize: !1,
                virtualTranslate: !1,
                effect: "slide",
                coverflow: {
                    rotate: 50,
                    stretch: 0,
                    depth: 100,
                    modifier: 1,
                    slideShadows: !0
                },
                flip: {
                    slideShadows: !0,
                    limitRotation: !0
                },
                cube: {
                    slideShadows: !0,
                    shadow: !0,
                    shadowOffset: 20,
                    shadowScale: .94
                },
                fade: {
                    crossFade: !1
                },
                parallax: !1,
                zoom: !1,
                zoomMax: 3,
                zoomMin: 1,
                zoomToggle: !0,
                scrollbar: null,
                scrollbarHide: !0,
                scrollbarDraggable: !1,
                scrollbarSnapOnRelease: !1,
                keyboardControl: !1,
                mousewheelControl: !1,
                mousewheelReleaseOnEdges: !1,
                mousewheelInvert: !1,
                mousewheelForceToAxis: !1,
                mousewheelSensitivity: 1,
                mousewheelEventsTarged: "container",
                hashnav: !1,
                hashnavWatchState: !1,
                history: !1,
                replaceState: !1,
                breakpoints: void 0,
                spaceBetween: 0,
                slidesPerView: 1,
                slidesPerColumn: 1,
                slidesPerColumnFill: "column",
                slidesPerGroup: 1,
                centeredSlides: !1,
                slidesOffsetBefore: 0,
                slidesOffsetAfter: 0,
                roundLengths: !1,
                touchRatio: 1,
                touchAngle: 45,
                simulateTouch: !0,
                shortSwipes: !0,
                longSwipes: !0,
                longSwipesRatio: .5,
                longSwipesMs: 300,
                followFinger: !0,
                onlyExternal: !1,
                threshold: 0,
                touchMoveStopPropagation: !0,
                touchReleaseOnEdges: !1,
                uniqueNavElements: !0,
                pagination: null,
                paginationElement: "span",
                paginationClickable: !1,
                paginationHide: !1,
                paginationBulletRender: null,
                paginationProgressRender: null,
                paginationFractionRender: null,
                paginationCustomRender: null,
                paginationType: "bullets",
                resistance: !0,
                resistanceRatio: .85,
                nextButton: null,
                prevButton: null,
                watchSlidesProgress: !1,
                watchSlidesVisibility: !1,
                grabCursor: !1,
                preventClicks: !0,
                preventClicksPropagation: !0,
                slideToClickedSlide: !1,
                lazyLoading: !1,
                lazyLoadingInPrevNext: !1,
                lazyLoadingInPrevNextAmount: 1,
                lazyLoadingOnTransitionStart: !1,
                preloadImages: !0,
                updateOnImagesReady: !0,
                loop: !1,
                loopAdditionalSlides: 0,
                loopedSlides: null,
                control: void 0,
                controlInverse: !1,
                controlBy: "slide",
                normalizeSlideIndex: !0,
                allowSwipeToPrev: !0,
                allowSwipeToNext: !0,
                swipeHandler: null,
                noSwiping: !0,
                noSwipingClass: "swiper-no-swiping",
                passiveListeners: !0,
                containerModifierClass: "swiper-container-",
                slideClass: "swiper-slide",
                slideActiveClass: "swiper-slide-active",
                slideDuplicateActiveClass: "swiper-slide-duplicate-active",
                slideVisibleClass: "swiper-slide-visible",
                slideDuplicateClass: "swiper-slide-duplicate",
                slideNextClass: "swiper-slide-next",
                slideDuplicateNextClass: "swiper-slide-duplicate-next",
                slidePrevClass: "swiper-slide-prev",
                slideDuplicatePrevClass: "swiper-slide-duplicate-prev",
                wrapperClass: "swiper-wrapper",
                bulletClass: "swiper-pagination-bullet",
                bulletActiveClass: "swiper-pagination-bullet-active",
                buttonDisabledClass: "swiper-button-disabled",
                paginationCurrentClass: "swiper-pagination-current",
                paginationTotalClass: "swiper-pagination-total",
                paginationHiddenClass: "swiper-pagination-hidden",
                paginationProgressbarClass: "swiper-pagination-progressbar",
                paginationClickableClass: "swiper-pagination-clickable",
                paginationModifierClass: "swiper-pagination-",
                lazyLoadingClass: "swiper-lazy",
                lazyStatusLoadingClass: "swiper-lazy-loading",
                lazyStatusLoadedClass: "swiper-lazy-loaded",
                lazyPreloaderClass: "swiper-lazy-preloader",
                notificationClass: "swiper-notification",
                preloaderClass: "preloader",
                zoomContainerClass: "swiper-zoom-container",
                observer: !1,
                observeParents: !1,
                a11y: !1,
                prevSlideMessage: "Previous slide",
                nextSlideMessage: "Next slide",
                firstSlideMessage: "This is the first slide",
                lastSlideMessage: "This is the last slide",
                paginationBulletMessage: "Go to slide {{index}}",
                runCallbacksOnInit: !0
            };
            wt = r && r.virtualTranslate;
            r = r || {};
            y = {};
            for (e in r)
                if ("object" != typeof r[e] || null === r[e] || r[e].nodeType || r[e] === window || r[e] === document || "undefined" != typeof Dom7 && r[e] instanceof Dom7 || "undefined" != typeof jQuery && r[e] instanceof jQuery) y[e] = r[e];
                else {
                    y[e] = {};
                    for (ot in r[e]) y[e][ot] = r[e][ot]
                }
            for (s in v)
                if ("undefined" == typeof r[s]) r[s] = v[s];
                else if ("object" == typeof r[s])
                    for (k in v[s]) "undefined" == typeof r[s][k] && (r[s][k] = v[s][k]);
            if (u = this, u.params = r, u.originalParams = y, u.classNames = [], "undefined" != typeof n && "undefined" != typeof Dom7 && (n = Dom7), ("undefined" != typeof n || (n = "undefined" == typeof Dom7 ? window.Dom7 || window.Zepto || window.jQuery : Dom7)) && (u.$ = n, u.currentBreakpoint = void 0, u.getActiveBreakpoint = function () {
                var n, i, t, r;
                if (!u.params.breakpoints) return !1;
                i = !1;
                t = [];
                for (n in u.params.breakpoints) u.params.breakpoints.hasOwnProperty(n) && t.push(n);
                for (t.sort(function (n, t) {
                    return parseInt(n, 10) > parseInt(t, 10)
                }), r = 0; r < t.length; r++) n = t[r], n >= window.innerWidth && !i && (i = n);
                return i || "max"
            }, u.setBreakpoint = function () {
                var n = u.getActiveBreakpoint(),
                    t, r, i;
                if (n && u.currentBreakpoint !== n) {
                    t = n in u.params.breakpoints ? u.params.breakpoints[n] : u.originalParams;
                    r = u.params.loop && t.slidesPerView !== u.params.slidesPerView;
                    for (i in t) u.params[i] = t[i];
                    u.currentBreakpoint = n;
                    r && u.destroyLoop && u.reLoop(!0)
                }
            }, u.params.breakpoints && u.setBreakpoint(), u.container = n(i), 0 !== u.container.length)) {
                if (u.container.length > 1) return st = [], u.container.each(function () {
                    st.push(new t(this, r))
                }), st;
                u.container[0].swiper = u;
                u.container.data("swiper", u);
                u.classNames.push(u.params.containerModifierClass + u.params.direction);
                u.params.freeMode && u.classNames.push(u.params.containerModifierClass + "free-mode");
                u.support.flexbox || (u.classNames.push(u.params.containerModifierClass + "no-flexbox"), u.params.slidesPerColumn = 1);
                u.params.autoHeight && u.classNames.push(u.params.containerModifierClass + "autoheight");
                (u.params.parallax || u.params.watchSlidesVisibility) && (u.params.watchSlidesProgress = !0);
                u.params.touchReleaseOnEdges && (u.params.resistanceRatio = 0);
                ["cube", "coverflow", "flip"].indexOf(u.params.effect) >= 0 && (u.support.transforms3d ? (u.params.watchSlidesProgress = !0, u.classNames.push(u.params.containerModifierClass + "3d")) : u.params.effect = "slide");
                "slide" !== u.params.effect && u.classNames.push(u.params.containerModifierClass + u.params.effect);
                "cube" === u.params.effect && (u.params.resistanceRatio = 0, u.params.slidesPerView = 1, u.params.slidesPerColumn = 1, u.params.slidesPerGroup = 1, u.params.centeredSlides = !1, u.params.spaceBetween = 0, u.params.virtualTranslate = !0, u.params.setWrapperSize = !1);
                "fade" !== u.params.effect && "flip" !== u.params.effect || (u.params.slidesPerView = 1, u.params.slidesPerColumn = 1, u.params.slidesPerGroup = 1, u.params.watchSlidesProgress = !0, u.params.spaceBetween = 0, u.params.setWrapperSize = !1, "undefined" == typeof wt && (u.params.virtualTranslate = !0));
                u.params.grabCursor && u.support.touch && (u.params.grabCursor = !1);
                u.wrapper = u.container.children("." + u.params.wrapperClass);
                u.params.pagination && (u.paginationContainer = n(u.params.pagination), u.params.uniqueNavElements && "string" == typeof u.params.pagination && u.paginationContainer.length > 1 && 1 === u.container.find(u.params.pagination).length && (u.paginationContainer = u.container.find(u.params.pagination)), "bullets" === u.params.paginationType && u.params.paginationClickable ? u.paginationContainer.addClass(u.params.paginationModifierClass + "clickable") : u.params.paginationClickable = !1, u.paginationContainer.addClass(u.params.paginationModifierClass + u.params.paginationType));
                (u.params.nextButton || u.params.prevButton) && (u.params.nextButton && (u.nextButton = n(u.params.nextButton), u.params.uniqueNavElements && "string" == typeof u.params.nextButton && u.nextButton.length > 1 && 1 === u.container.find(u.params.nextButton).length && (u.nextButton = u.container.find(u.params.nextButton))), u.params.prevButton && (u.prevButton = n(u.params.prevButton), u.params.uniqueNavElements && "string" == typeof u.params.prevButton && u.prevButton.length > 1 && 1 === u.container.find(u.params.prevButton).length && (u.prevButton = u.container.find(u.params.prevButton))));
                u.isHorizontal = function () {
                    return "horizontal" === u.params.direction
                };
                u.rtl = u.isHorizontal() && ("rtl" === u.container[0].dir.toLowerCase() || "rtl" === u.container.css("direction"));
                u.rtl && u.classNames.push(u.params.containerModifierClass + "rtl");
                u.rtl && (u.wrongRTL = "-webkit-box" === u.wrapper.css("display"));
                u.params.slidesPerColumn > 1 && u.classNames.push(u.params.containerModifierClass + "multirow");
                u.device.android && u.classNames.push(u.params.containerModifierClass + "android");
                u.container.addClass(u.classNames.join(" "));
                u.translate = 0;
                u.progress = 0;
                u.velocity = 0;
                u.lockSwipeToNext = function () {
                    u.params.allowSwipeToNext = !1;
                    u.params.allowSwipeToPrev === !1 && u.params.grabCursor && u.unsetGrabCursor()
                };
                u.lockSwipeToPrev = function () {
                    u.params.allowSwipeToPrev = !1;
                    u.params.allowSwipeToNext === !1 && u.params.grabCursor && u.unsetGrabCursor()
                };
                u.lockSwipes = function () {
                    u.params.allowSwipeToNext = u.params.allowSwipeToPrev = !1;
                    u.params.grabCursor && u.unsetGrabCursor()
                };
                u.unlockSwipeToNext = function () {
                    u.params.allowSwipeToNext = !0;
                    u.params.allowSwipeToPrev === !0 && u.params.grabCursor && u.setGrabCursor()
                };
                u.unlockSwipeToPrev = function () {
                    u.params.allowSwipeToPrev = !0;
                    u.params.allowSwipeToNext === !0 && u.params.grabCursor && u.setGrabCursor()
                };
                u.unlockSwipes = function () {
                    u.params.allowSwipeToNext = u.params.allowSwipeToPrev = !0;
                    u.params.grabCursor && u.setGrabCursor()
                };
                u.setGrabCursor = function (n) {
                    u.container[0].style.cursor = "move";
                    u.container[0].style.cursor = n ? "-webkit-grabbing" : "-webkit-grab";
                    u.container[0].style.cursor = n ? "-moz-grabbin" : "-moz-grab";
                    u.container[0].style.cursor = n ? "grabbing" : "grab"
                };
                u.unsetGrabCursor = function () {
                    u.container[0].style.cursor = ""
                };
                u.params.grabCursor && u.setGrabCursor();
                u.imagesToLoad = [];
                u.imagesLoaded = 0;
                u.loadImage = function (n, t, i, r, u, f) {
                    function o() {
                        f && f()
                    }

                    var e;
                    n.complete && u ? o() : t ? (e = new window.Image, e.onload = o, e.onerror = o, r && (e.sizes = r), i && (e.srcset = i), t && (e.src = t)) : o()
                };
                u.preloadImages = function () {
                    function t() {
                        "undefined" != typeof u && null !== u && u && (void 0 !== u.imagesLoaded && u.imagesLoaded++, u.imagesLoaded === u.imagesToLoad.length && (u.params.updateOnImagesReady && u.update(), u.emit("onImagesReady", u)))
                    }

                    u.imagesToLoad = u.container.find("img");
                    for (var n = 0; n < u.imagesToLoad.length; n++) u.loadImage(u.imagesToLoad[n], u.imagesToLoad[n].currentSrc || u.imagesToLoad[n].getAttribute("src"), u.imagesToLoad[n].srcset || u.imagesToLoad[n].getAttribute("srcset"), u.imagesToLoad[n].sizes || u.imagesToLoad[n].getAttribute("sizes"), !0, t)
                };
                u.autoplayTimeoutId = void 0;
                u.autoplaying = !1;
                u.autoplayPaused = !1;
                u.startAutoplay = function () {
                    return "undefined" == typeof u.autoplayTimeoutId && !!u.params.autoplay && !u.autoplaying && (u.autoplaying = !0, u.emit("onAutoplayStart", u), void rt())
                };
                u.stopAutoplay = function () {
                    u.autoplayTimeoutId && (u.autoplayTimeoutId && clearTimeout(u.autoplayTimeoutId), u.autoplaying = !1, u.autoplayTimeoutId = void 0, u.emit("onAutoplayStop", u))
                };
                u.pauseAutoplay = function (n) {
                    u.autoplayPaused || (u.autoplayTimeoutId && clearTimeout(u.autoplayTimeoutId), u.autoplayPaused = !0, 0 === n ? (u.autoplayPaused = !1, rt()) : u.wrapper.transitionEnd(function () {
                        u && (u.autoplayPaused = !1, u.autoplaying ? rt() : u.stopAutoplay())
                    }))
                };
                u.minTranslate = function () {
                    return -u.snapGrid[0]
                };
                u.maxTranslate = function () {
                    return -u.snapGrid[u.snapGrid.length - 1]
                };
                u.updateAutoHeight = function () {
                    var n, t = [],
                        i = 0,
                        r, f;
                    if ("auto" !== u.params.slidesPerView && u.params.slidesPerView > 1)
                        for (n = 0; n < Math.ceil(u.params.slidesPerView); n++) {
                            if (r = u.activeIndex + n, r > u.slides.length) break;
                            t.push(u.slides.eq(r)[0])
                        } else t.push(u.slides.eq(u.activeIndex)[0]);
                    for (n = 0; n < t.length; n++) "undefined" != typeof t[n] && (f = t[n].offsetHeight, i = f > i ? f : i);
                    i && u.wrapper.css("height", i + "px")
                };
                u.updateContainerSize = function () {
                    var n, t;
                    n = "undefined" != typeof u.params.width ? u.params.width : u.container[0].clientWidth;
                    t = "undefined" != typeof u.params.height ? u.params.height : u.container[0].clientHeight;
                    0 === n && u.isHorizontal() || 0 === t && !u.isHorizontal() || (n = n - parseInt(u.container.css("padding-left"), 10) - parseInt(u.container.css("padding-right"), 10), t = t - parseInt(u.container.css("padding-top"), 10) - parseInt(u.container.css("padding-bottom"), 10), u.width = n, u.height = t, u.size = u.isHorizontal() ? u.width : u.height)
                };
                u.updateSlidesSize = function () {
                    var o, h, c, e, f, l;
                    u.slides = u.wrapper.children("." + u.params.slideClass);
                    u.snapGrid = [];
                    u.slidesGrid = [];
                    u.slidesSizesGrid = [];
                    var n, i = u.params.spaceBetween,
                        r = -u.params.slidesOffsetBefore,
                        y = 0,
                        v = 0;
                    if ("undefined" != typeof u.size) {
                        "string" == typeof i && i.indexOf("%") >= 0 && (i = parseFloat(i.replace("%", "")) / 100 * u.size);
                        u.virtualSize = -i;
                        u.rtl ? u.slides.css({
                            marginLeft: "",
                            marginTop: ""
                        }) : u.slides.css({
                            marginRight: "",
                            marginBottom: ""
                        });
                        u.params.slidesPerColumn > 1 && (o = Math.floor(u.slides.length / u.params.slidesPerColumn) === u.slides.length / u.params.slidesPerColumn ? u.slides.length : Math.ceil(u.slides.length / u.params.slidesPerColumn) * u.params.slidesPerColumn, "auto" !== u.params.slidesPerView && "row" === u.params.slidesPerColumnFill && (o = Math.max(o, u.params.slidesPerView * u.params.slidesPerColumn)));
                        var t, s = u.params.slidesPerColumn,
                            a = o / s,
                            p = a - (u.params.slidesPerColumn * a - u.slides.length);
                        for (n = 0; n < u.slides.length; n++) t = 0, h = u.slides.eq(n), u.params.slidesPerColumn > 1 && ("column" === u.params.slidesPerColumnFill ? (e = Math.floor(n / s), f = n - e * s, (e > p || e === p && f === s - 1) && ++f >= s && (f = 0, e++), c = e + f * o / s, h.css({
                            "-webkit-box-ordinal-group": c,
                            "-moz-box-ordinal-group": c,
                            "-ms-flex-order": c,
                            "-webkit-order": c,
                            order: c
                        })) : (f = Math.floor(n / a), e = n - f * a), h.css("margin-" + (u.isHorizontal() ? "top" : "left"), 0 !== f && u.params.spaceBetween && u.params.spaceBetween + "px").attr("data-swiper-column", e).attr("data-swiper-row", f)), "none" !== h.css("display") && ("auto" === u.params.slidesPerView ? (t = u.isHorizontal() ? h.outerWidth(!0) : h.outerHeight(!0), u.params.roundLengths && (t = b(t))) : (t = (u.size - (u.params.slidesPerView - 1) * i) / u.params.slidesPerView, u.params.roundLengths && (t = b(t)), u.isHorizontal() ? u.slides[n].style.width = t + "px" : u.slides[n].style.height = t + "px"), u.slides[n].swiperSlideSize = t, u.slidesSizesGrid.push(t), u.params.centeredSlides ? (r = r + t / 2 + y / 2 + i, 0 === n && (r = r - u.size / 2 - i), Math.abs(r) < .001 && (r = 0), v % u.params.slidesPerGroup == 0 && u.snapGrid.push(r), u.slidesGrid.push(r)) : (v % u.params.slidesPerGroup == 0 && u.snapGrid.push(r), u.slidesGrid.push(r), r = r + t + i), u.virtualSize += t + i, y = t, v++);
                        if (u.virtualSize = Math.max(u.virtualSize, u.size) + u.params.slidesOffsetAfter, u.rtl && u.wrongRTL && ("slide" === u.params.effect || "coverflow" === u.params.effect) && u.wrapper.css({
                            width: u.virtualSize + u.params.spaceBetween + "px"
                        }), u.support.flexbox && !u.params.setWrapperSize || (u.isHorizontal() ? u.wrapper.css({
                            width: u.virtualSize + u.params.spaceBetween + "px"
                        }) : u.wrapper.css({
                            height: u.virtualSize + u.params.spaceBetween + "px"
                        })), u.params.slidesPerColumn > 1 && (u.virtualSize = (t + u.params.spaceBetween) * o, u.virtualSize = Math.ceil(u.virtualSize / u.params.slidesPerColumn) - u.params.spaceBetween, u.isHorizontal() ? u.wrapper.css({
                            width: u.virtualSize + u.params.spaceBetween + "px"
                        }) : u.wrapper.css({
                            height: u.virtualSize + u.params.spaceBetween + "px"
                        }), u.params.centeredSlides)) {
                            for (l = [], n = 0; n < u.snapGrid.length; n++) u.snapGrid[n] < u.virtualSize + u.snapGrid[0] && l.push(u.snapGrid[n]);
                            u.snapGrid = l
                        }
                        if (!u.params.centeredSlides) {
                            for (l = [], n = 0; n < u.snapGrid.length; n++) u.snapGrid[n] <= u.virtualSize - u.size && l.push(u.snapGrid[n]);
                            u.snapGrid = l;
                            Math.floor(u.virtualSize - u.size) - Math.floor(u.snapGrid[u.snapGrid.length - 1]) > 1 && u.snapGrid.push(u.virtualSize - u.size)
                        }
                        0 === u.snapGrid.length && (u.snapGrid = [0]);
                        0 !== u.params.spaceBetween && (u.isHorizontal() ? u.rtl ? u.slides.css({
                            marginLeft: i + "px"
                        }) : u.slides.css({
                            marginRight: i + "px"
                        }) : u.slides.css({
                            marginBottom: i + "px"
                        }));
                        u.params.watchSlidesProgress && u.updateSlidesOffset()
                    }
                };
                u.updateSlidesOffset = function () {
                    for (var n = 0; n < u.slides.length; n++) u.slides[n].swiperSlideOffset = u.isHorizontal() ? u.slides[n].offsetLeft : u.slides[n].offsetTop
                };
                u.currentSlidesPerView = function () {
                    var n, t, r = 1,
                        f, i;
                    if (u.params.centeredSlides) {
                        for (i = u.slides[u.activeIndex].swiperSlideSize, n = u.activeIndex + 1; n < u.slides.length; n++) u.slides[n] && !f && (i += u.slides[n].swiperSlideSize, r++, i > u.size && (f = !0));
                        for (t = u.activeIndex - 1; t >= 0; t--) u.slides[t] && !f && (i += u.slides[t].swiperSlideSize, r++, i > u.size && (f = !0))
                    } else
                        for (n = u.activeIndex + 1; n < u.slides.length; n++) u.slidesGrid[n] - u.slidesGrid[u.activeIndex] < u.size && r++;
                    return r
                };
                u.updateSlidesProgress = function (n) {
                    var r, t, i, e;
                    if ("undefined" == typeof n && (n = u.translate || 0), 0 !== u.slides.length)
                        for ("undefined" == typeof u.slides[0].swiperSlideOffset && u.updateSlidesOffset(), r = -n, u.rtl && (r = n), u.slides.removeClass(u.params.slideVisibleClass), t = 0; t < u.slides.length; t++) {
                            if (i = u.slides[t], e = (r + (u.params.centeredSlides ? u.minTranslate() : 0) - i.swiperSlideOffset) / (i.swiperSlideSize + u.params.spaceBetween), u.params.watchSlidesVisibility) {
                                var f = -(r - i.swiperSlideOffset),
                                    o = f + u.slidesSizesGrid[t],
                                    s = f >= 0 && f < u.size || o > 0 && o <= u.size || f <= 0 && o >= u.size;
                                s && u.slides.eq(t).addClass(u.params.slideVisibleClass)
                            }
                            i.progress = u.rtl ? -e : e
                        }
                };
                u.updateProgress = function (n) {
                    "undefined" == typeof n && (n = u.translate || 0);
                    var t = u.maxTranslate() - u.minTranslate(),
                        i = u.isBeginning,
                        r = u.isEnd;
                    0 === t ? (u.progress = 0, u.isBeginning = u.isEnd = !0) : (u.progress = (n - u.minTranslate()) / t, u.isBeginning = u.progress <= 0, u.isEnd = u.progress >= 1);
                    u.isBeginning && !i && u.emit("onReachBeginning", u);
                    u.isEnd && !r && u.emit("onReachEnd", u);
                    u.params.watchSlidesProgress && u.updateSlidesProgress(n);
                    u.emit("onProgress", u, u.progress)
                };
                u.updateActiveIndex = function () {
                    for (var t, r, i = u.rtl ? u.translate : -u.translate, n = 0; n < u.slidesGrid.length; n++) "undefined" != typeof u.slidesGrid[n + 1] ? i >= u.slidesGrid[n] && i < u.slidesGrid[n + 1] - (u.slidesGrid[n + 1] - u.slidesGrid[n]) / 2 ? t = n : i >= u.slidesGrid[n] && i < u.slidesGrid[n + 1] && (t = n + 1) : i >= u.slidesGrid[n] && (t = n);
                    u.params.normalizeSlideIndex && (t < 0 || "undefined" == typeof t) && (t = 0);
                    r = Math.floor(t / u.params.slidesPerGroup);
                    r >= u.snapGrid.length && (r = u.snapGrid.length - 1);
                    t !== u.activeIndex && (u.snapIndex = r, u.previousIndex = u.activeIndex, u.activeIndex = t, u.updateClasses(), u.updateRealIndex())
                };
                u.updateRealIndex = function () {
                    u.realIndex = parseInt(u.slides.eq(u.activeIndex).attr("data-swiper-slide-index") || u.activeIndex, 10)
                };
                u.updateClasses = function () {
                    var o, i, f, t, e;
                    if (u.slides.removeClass(u.params.slideActiveClass + " " + u.params.slideNextClass + " " + u.params.slidePrevClass + " " + u.params.slideDuplicateActiveClass + " " + u.params.slideDuplicateNextClass + " " + u.params.slideDuplicatePrevClass), o = u.slides.eq(u.activeIndex), o.addClass(u.params.slideActiveClass), r.loop && (o.hasClass(u.params.slideDuplicateClass) ? u.wrapper.children("." + u.params.slideClass + ":not(." + u.params.slideDuplicateClass + ')[data-swiper-slide-index="' + u.realIndex + '"]').addClass(u.params.slideDuplicateActiveClass) : u.wrapper.children("." + u.params.slideClass + "." + u.params.slideDuplicateClass + '[data-swiper-slide-index="' + u.realIndex + '"]').addClass(u.params.slideDuplicateActiveClass)), i = o.next("." + u.params.slideClass).addClass(u.params.slideNextClass), u.params.loop && 0 === i.length && (i = u.slides.eq(0), i.addClass(u.params.slideNextClass)), f = o.prev("." + u.params.slideClass).addClass(u.params.slidePrevClass), u.params.loop && 0 === f.length && (f = u.slides.eq(-1), f.addClass(u.params.slidePrevClass)), r.loop && (i.hasClass(u.params.slideDuplicateClass) ? u.wrapper.children("." + u.params.slideClass + ":not(." + u.params.slideDuplicateClass + ')[data-swiper-slide-index="' + i.attr("data-swiper-slide-index") + '"]').addClass(u.params.slideDuplicateNextClass) : u.wrapper.children("." + u.params.slideClass + "." + u.params.slideDuplicateClass + '[data-swiper-slide-index="' + i.attr("data-swiper-slide-index") + '"]').addClass(u.params.slideDuplicateNextClass), f.hasClass(u.params.slideDuplicateClass) ? u.wrapper.children("." + u.params.slideClass + ":not(." + u.params.slideDuplicateClass + ')[data-swiper-slide-index="' + f.attr("data-swiper-slide-index") + '"]').addClass(u.params.slideDuplicatePrevClass) : u.wrapper.children("." + u.params.slideClass + "." + u.params.slideDuplicateClass + '[data-swiper-slide-index="' + f.attr("data-swiper-slide-index") + '"]').addClass(u.params.slideDuplicatePrevClass)), u.paginationContainer && u.paginationContainer.length > 0) {
                        if (e = u.params.loop ? Math.ceil((u.slides.length - 2 * u.loopedSlides) / u.params.slidesPerGroup) : u.snapGrid.length, u.params.loop ? (t = Math.ceil((u.activeIndex - u.loopedSlides) / u.params.slidesPerGroup), t > u.slides.length - 1 - 2 * u.loopedSlides && (t -= u.slides.length - 2 * u.loopedSlides), t > e - 1 && (t -= e), t < 0 && "bullets" !== u.params.paginationType && (t = e + t)) : t = "undefined" != typeof u.snapIndex ? u.snapIndex : u.activeIndex || 0, "bullets" === u.params.paginationType && u.bullets && u.bullets.length > 0 && (u.bullets.removeClass(u.params.bulletActiveClass), u.paginationContainer.length > 1 ? u.bullets.each(function () {
                            n(this).index() === t && n(this).addClass(u.params.bulletActiveClass)
                        }) : u.bullets.eq(t).addClass(u.params.bulletActiveClass)), "fraction" === u.params.paginationType && (u.paginationContainer.find("." + u.params.paginationCurrentClass).text(t + 1), u.paginationContainer.find("." + u.params.paginationTotalClass).text(e)), "progress" === u.params.paginationType) {
                            var s = (t + 1) / e,
                                h = s,
                                c = 1;
                            u.isHorizontal() || (c = s, h = 1);
                            u.paginationContainer.find("." + u.params.paginationProgressbarClass).transform("translate3d(0,0,0) scaleX(" + h + ") scaleY(" + c + ")").transition(u.params.speed)
                        }
                        "custom" === u.params.paginationType && u.params.paginationCustomRender && (u.paginationContainer.html(u.params.paginationCustomRender(u, t + 1, e)), u.emit("onPaginationRendered", u, u.paginationContainer[0]))
                    }
                    u.params.loop || (u.params.prevButton && u.prevButton && u.prevButton.length > 0 && (u.isBeginning ? (u.prevButton.addClass(u.params.buttonDisabledClass), u.params.a11y && u.a11y && u.a11y.disable(u.prevButton)) : (u.prevButton.removeClass(u.params.buttonDisabledClass), u.params.a11y && u.a11y && u.a11y.enable(u.prevButton))), u.params.nextButton && u.nextButton && u.nextButton.length > 0 && (u.isEnd ? (u.nextButton.addClass(u.params.buttonDisabledClass), u.params.a11y && u.a11y && u.a11y.disable(u.nextButton)) : (u.nextButton.removeClass(u.params.buttonDisabledClass), u.params.a11y && u.a11y && u.a11y.enable(u.nextButton))))
                };
                u.updatePagination = function () {
                    var n, i, t;
                    if (u.params.pagination && u.paginationContainer && u.paginationContainer.length > 0) {
                        if (n = "", "bullets" === u.params.paginationType) {
                            for (i = u.params.loop ? Math.ceil((u.slides.length - 2 * u.loopedSlides) / u.params.slidesPerGroup) : u.snapGrid.length, t = 0; t < i; t++) n += u.params.paginationBulletRender ? u.params.paginationBulletRender(u, t, u.params.bulletClass) : "<" + u.params.paginationElement + ' class="' + u.params.bulletClass + '"><\/' + u.params.paginationElement + ">";
                            u.paginationContainer.html(n);
                            u.bullets = u.paginationContainer.find("." + u.params.bulletClass);
                            u.params.paginationClickable && u.params.a11y && u.a11y && u.a11y.initPagination()
                        }
                        "fraction" === u.params.paginationType && (n = u.params.paginationFractionRender ? u.params.paginationFractionRender(u, u.params.paginationCurrentClass, u.params.paginationTotalClass) : '<span class="' + u.params.paginationCurrentClass + '"><\/span> / <span class="' + u.params.paginationTotalClass + '"><\/span>', u.paginationContainer.html(n));
                        "progress" === u.params.paginationType && (n = u.params.paginationProgressRender ? u.params.paginationProgressRender(u, u.params.paginationProgressbarClass) : '<span class="' + u.params.paginationProgressbarClass + '"><\/span>', u.paginationContainer.html(n));
                        "custom" !== u.params.paginationType && u.emit("onPaginationRendered", u, u.paginationContainer[0])
                    }
                };
                u.update = function (n) {
                    function t() {
                        u.rtl ? -u.translate : u.translate;
                        r = Math.min(Math.max(u.translate, u.maxTranslate()), u.minTranslate());
                        u.setWrapperTranslate(r);
                        u.updateActiveIndex();
                        u.updateClasses()
                    }

                    if (u)
                        if (u.updateContainerSize(), u.updateSlidesSize(), u.updateProgress(), u.updatePagination(), u.updateClasses(), u.params.scrollbar && u.scrollbar && u.scrollbar.set(), n) {
                            var i, r;
                            u.controller && u.controller.spline && (u.controller.spline = void 0);
                            u.params.freeMode ? (t(), u.params.autoHeight && u.updateAutoHeight()) : (i = ("auto" === u.params.slidesPerView || u.params.slidesPerView > 1) && u.isEnd && !u.params.centeredSlides ? u.slideTo(u.slides.length - 1, 0, !1, !0) : u.slideTo(u.activeIndex, 0, !1, !0), i || t())
                        } else u.params.autoHeight && u.updateAutoHeight()
                };
                u.onResize = function (n) {
                    var i, r, t, f;
                    u.params.breakpoints && u.setBreakpoint();
                    i = u.params.allowSwipeToPrev;
                    r = u.params.allowSwipeToNext;
                    u.params.allowSwipeToPrev = u.params.allowSwipeToNext = !0;
                    u.updateContainerSize();
                    u.updateSlidesSize();
                    ("auto" === u.params.slidesPerView || u.params.freeMode || n) && u.updatePagination();
                    u.params.scrollbar && u.scrollbar && u.scrollbar.set();
                    u.controller && u.controller.spline && (u.controller.spline = void 0);
                    t = !1;
                    u.params.freeMode ? (f = Math.min(Math.max(u.translate, u.maxTranslate()), u.minTranslate()), u.setWrapperTranslate(f), u.updateActiveIndex(), u.updateClasses(), u.params.autoHeight && u.updateAutoHeight()) : (u.updateClasses(), t = ("auto" === u.params.slidesPerView || u.params.slidesPerView > 1) && u.isEnd && !u.params.centeredSlides ? u.slideTo(u.slides.length - 1, 0, !1, !0) : u.slideTo(u.activeIndex, 0, !1, !0));
                    u.params.lazyLoading && !t && u.lazy && u.lazy.load();
                    u.params.allowSwipeToPrev = i;
                    u.params.allowSwipeToNext = r
                };
                u.touchEventsDesktop = {
                    start: "mousedown",
                    move: "mousemove",
                    end: "mouseup"
                };
                window.navigator.pointerEnabled ? u.touchEventsDesktop = {
                    start: "pointerdown",
                    move: "pointermove",
                    end: "pointerup"
                } : window.navigator.msPointerEnabled && (u.touchEventsDesktop = {
                    start: "MSPointerDown",
                    move: "MSPointerMove",
                    end: "MSPointerUp"
                });
                u.touchEvents = {
                    start: u.support.touch || !u.params.simulateTouch ? "touchstart" : u.touchEventsDesktop.start,
                    move: u.support.touch || !u.params.simulateTouch ? "touchmove" : u.touchEventsDesktop.move,
                    end: u.support.touch || !u.params.simulateTouch ? "touchend" : u.touchEventsDesktop.end
                };
                (window.navigator.pointerEnabled || window.navigator.msPointerEnabled) && ("container" === u.params.touchEventsTarget ? u.container : u.wrapper).addClass("swiper-wp8-" + u.params.direction);
                u.initEvents = function (n) {
                    var f = n ? "off" : "on",
                        t = n ? "removeEventListener" : "addEventListener",
                        i = "container" === u.params.touchEventsTarget ? u.container[0] : u.wrapper[0],
                        s = u.support.touch ? i : document,
                        e = !!u.params.nested,
                        o;
                    u.browser.ie ? (i[t](u.touchEvents.start, u.onTouchStart, !1), s[t](u.touchEvents.move, u.onTouchMove, e), s[t](u.touchEvents.end, u.onTouchEnd, !1)) : (u.support.touch && (o = !("touchstart" !== u.touchEvents.start || !u.support.passiveListener || !u.params.passiveListeners) && {
                        passive: !0,
                        capture: !1
                    }, i[t](u.touchEvents.start, u.onTouchStart, o), i[t](u.touchEvents.move, u.onTouchMove, e), i[t](u.touchEvents.end, u.onTouchEnd, o)), (r.simulateTouch && !u.device.ios && !u.device.android || r.simulateTouch && !u.support.touch && u.device.ios) && (i[t]("mousedown", u.onTouchStart, !1), document[t]("mousemove", u.onTouchMove, e), document[t]("mouseup", u.onTouchEnd, !1)));
                    window[t]("resize", u.onResize);
                    u.params.nextButton && u.nextButton && u.nextButton.length > 0 && (u.nextButton[f]("click", u.onClickNext), u.params.a11y && u.a11y && u.nextButton[f]("keydown", u.a11y.onEnterKey));
                    u.params.prevButton && u.prevButton && u.prevButton.length > 0 && (u.prevButton[f]("click", u.onClickPrev), u.params.a11y && u.a11y && u.prevButton[f]("keydown", u.a11y.onEnterKey));
                    u.params.pagination && u.params.paginationClickable && (u.paginationContainer[f]("click", "." + u.params.bulletClass, u.onClickIndex), u.params.a11y && u.a11y && u.paginationContainer[f]("keydown", "." + u.params.bulletClass, u.a11y.onEnterKey));
                    (u.params.preventClicks || u.params.preventClicksPropagation) && i[t]("click", u.preventClicks, !0)
                };
                u.attachEvents = function () {
                    u.initEvents()
                };
                u.detachEvents = function () {
                    u.initEvents(!0)
                };
                u.allowClick = !0;
                u.preventClicks = function (n) {
                    u.allowClick || (u.params.preventClicks && n.preventDefault(), u.params.preventClicksPropagation && u.animating && (n.stopPropagation(), n.stopImmediatePropagation()))
                };
                u.onClickNext = function (n) {
                    n.preventDefault();
                    u.isEnd && !u.params.loop || u.slideNext()
                };
                u.onClickPrev = function (n) {
                    n.preventDefault();
                    u.isBeginning && !u.params.loop || u.slidePrev()
                };
                u.onClickIndex = function (t) {
                    t.preventDefault();
                    var i = n(this).index() * u.params.slidesPerGroup;
                    u.params.loop && (i += u.loopedSlides);
                    u.slideTo(i)
                };
                u.updateClickedSlide = function (t) {
                    var r = ut(t, "." + u.params.slideClass),
                        s = !1,
                        f, o, i, e;
                    if (r)
                        for (f = 0; f < u.slides.length; f++) u.slides[f] === r && (s = !0);
                    if (!r || !s) return u.clickedSlide = void 0, void (u.clickedIndex = void 0);
                    if (u.clickedSlide = r, u.clickedIndex = n(r).index(), u.params.slideToClickedSlide && void 0 !== u.clickedIndex && u.clickedIndex !== u.activeIndex)
                        if (i = u.clickedIndex, e = "auto" === u.params.slidesPerView ? u.currentSlidesPerView() : u.params.slidesPerView, u.params.loop) {
                            if (u.animating) return;
                            o = parseInt(n(u.clickedSlide).attr("data-swiper-slide-index"), 10);
                            u.params.centeredSlides ? i < u.loopedSlides - e / 2 || i > u.slides.length - u.loopedSlides + e / 2 ? (u.fixLoop(), i = u.wrapper.children("." + u.params.slideClass + '[data-swiper-slide-index="' + o + '"]:not(.' + u.params.slideDuplicateClass + ")").eq(0).index(), setTimeout(function () {
                                u.slideTo(i)
                            }, 0)) : u.slideTo(i) : i > u.slides.length - e ? (u.fixLoop(), i = u.wrapper.children("." + u.params.slideClass + '[data-swiper-slide-index="' + o + '"]:not(.' + u.params.slideDuplicateClass + ")").eq(0).index(), setTimeout(function () {
                                u.slideTo(i)
                            }, 0)) : u.slideTo(i)
                        } else u.slideTo(i)
                };
                var h, c, d, g, a, f, o, nt, p, tt, ht = "input, select, textarea, button, video",
                    ct = Date.now(),
                    l = [];
                u.animating = !1;
                u.touches = {
                    startX: 0,
                    startY: 0,
                    currentX: 0,
                    currentY: 0,
                    diff: 0
                };
                u.onTouchStart = function (t) {
                    var i, f, r;
                    if (t.originalEvent && (t = t.originalEvent), w = "touchstart" === t.type, w || !("which" in t) || 3 !== t.which) {
                        if (u.params.noSwiping && ut(t, "." + u.params.noSwipingClass)) return void (u.allowClick = !0);
                        (!u.params.swipeHandler || ut(t, u.params.swipeHandler)) && (i = u.touches.currentX = "touchstart" === t.type ? t.targetTouches[0].pageX : t.pageX, f = u.touches.currentY = "touchstart" === t.type ? t.targetTouches[0].pageY : t.pageY, u.device.ios && u.params.iOSEdgeSwipeDetection && i <= u.params.iOSEdgeSwipeThreshold || ((h = !0, c = !1, d = !0, a = void 0, it = void 0, u.touches.startX = i, u.touches.startY = f, g = Date.now(), u.allowClick = !0, u.updateContainerSize(), u.swipeDirection = void 0, u.params.threshold > 0 && (nt = !1), "touchstart" !== t.type) && (r = !0, n(t.target).is(ht) && (r = !1), document.activeElement && n(document.activeElement).is(ht) && document.activeElement.blur(), r && t.preventDefault()), u.emit("onTouchStart", u, t)))
                    }
                };
                u.onTouchMove = function (t) {
                    var s, i, e;
                    if (t.originalEvent && (t = t.originalEvent), !w || "mousemove" !== t.type) {
                        if (t.preventedByNestedSwiper) return u.touches.startX = "touchmove" === t.type ? t.targetTouches[0].pageX : t.pageX, void (u.touches.startY = "touchmove" === t.type ? t.targetTouches[0].pageY : t.pageY);
                        if (u.params.onlyExternal) return u.allowClick = !1, void (h && (u.touches.startX = u.touches.currentX = "touchmove" === t.type ? t.targetTouches[0].pageX : t.pageX, u.touches.startY = u.touches.currentY = "touchmove" === t.type ? t.targetTouches[0].pageY : t.pageY, g = Date.now()));
                        if (w && u.params.touchReleaseOnEdges && !u.params.loop)
                            if (u.isHorizontal()) {
                                if (u.touches.currentX < u.touches.startX && u.translate <= u.maxTranslate() || u.touches.currentX > u.touches.startX && u.translate >= u.minTranslate()) return
                            } else if (u.touches.currentY < u.touches.startY && u.translate <= u.maxTranslate() || u.touches.currentY > u.touches.startY && u.translate >= u.minTranslate()) return;
                        if (w && document.activeElement && t.target === document.activeElement && n(t.target).is(ht)) return c = !0, void (u.allowClick = !1);
                        if ((d && u.emit("onTouchMove", u, t), !(t.targetTouches && t.targetTouches.length > 1)) && ((u.touches.currentX = "touchmove" === t.type ? t.targetTouches[0].pageX : t.pageX, u.touches.currentY = "touchmove" === t.type ? t.targetTouches[0].pageY : t.pageY, "undefined" == typeof a) && (u.isHorizontal() && u.touches.currentY === u.touches.startY || !u.isHorizontal() && u.touches.currentX === u.touches.startX ? a = !1 : (s = 180 * Math.atan2(Math.abs(u.touches.currentY - u.touches.startY), Math.abs(u.touches.currentX - u.touches.startX)) / Math.PI, a = u.isHorizontal() ? s > u.params.touchAngle : 90 - s > u.params.touchAngle)), a && u.emit("onTouchMoveOpposite", u, t), "undefined" == typeof it && u.browser.ieTouch && (u.touches.currentX === u.touches.startX && u.touches.currentY === u.touches.startY || (it = !0)), h)) {
                            if (a) return void (h = !1);
                            if (it || !u.browser.ieTouch) {
                                if (u.allowClick = !1, u.emit("onSliderMove", u, t), t.preventDefault(), u.params.touchMoveStopPropagation && !u.params.nested && t.stopPropagation(), c || (r.loop && u.fixLoop(), o = u.getWrapperTranslate(), u.setWrapperTransition(0), u.animating && u.wrapper.trigger("webkitTransitionEnd transitionend oTransitionEnd MSTransitionEnd msTransitionEnd"), u.params.autoplay && u.autoplaying && (u.params.autoplayDisableOnInteraction ? u.stopAutoplay() : u.pauseAutoplay()), tt = !1, !u.params.grabCursor || u.params.allowSwipeToNext !== !0 && u.params.allowSwipeToPrev !== !0 || u.setGrabCursor(!0)), c = !0, i = u.touches.diff = u.isHorizontal() ? u.touches.currentX - u.touches.startX : u.touches.currentY - u.touches.startY, i *= u.params.touchRatio, u.rtl && (i = -i), u.swipeDirection = i > 0 ? "prev" : "next", f = i + o, e = !0, i > 0 && f > u.minTranslate() ? (e = !1, u.params.resistance && (f = u.minTranslate() - 1 + Math.pow(-u.minTranslate() + o + i, u.params.resistanceRatio))) : i < 0 && f < u.maxTranslate() && (e = !1, u.params.resistance && (f = u.maxTranslate() + 1 - Math.pow(u.maxTranslate() - o - i, u.params.resistanceRatio))), e && (t.preventedByNestedSwiper = !0), !u.params.allowSwipeToNext && "next" === u.swipeDirection && f < o && (f = o), !u.params.allowSwipeToPrev && "prev" === u.swipeDirection && f > o && (f = o), u.params.threshold > 0) {
                                    if (!(Math.abs(i) > u.params.threshold || nt)) return void (f = o);
                                    if (!nt) return nt = !0, u.touches.startX = u.touches.currentX, u.touches.startY = u.touches.currentY, f = o, void (u.touches.diff = u.isHorizontal() ? u.touches.currentX - u.touches.startX : u.touches.currentY - u.touches.startY)
                                }
                                u.params.followFinger && ((u.params.freeMode || u.params.watchSlidesProgress) && u.updateActiveIndex(), u.params.freeMode && (0 === l.length && l.push({
                                    position: u.touches[u.isHorizontal() ? "startX" : "startY"],
                                    time: g
                                }), l.push({
                                    position: u.touches[u.isHorizontal() ? "currentX" : "currentY"],
                                    time: (new window.Date).getTime()
                                })), u.updateProgress(f), u.setWrapperTranslate(f))
                            }
                        }
                    }
                };
                u.onTouchEnd = function (t) {
                    var b, v, s, nt, it, y, w, a, r, e, rt, ft;
                    if (t.originalEvent && (t = t.originalEvent), d && u.emit("onTouchEnd", u, t), d = !1, h) {
                        if (u.params.grabCursor && c && h && (u.params.allowSwipeToNext === !0 || u.params.allowSwipeToPrev === !0) && u.setGrabCursor(!1), b = Date.now(), v = b - g, u.allowClick && (u.updateClickedSlide(t), u.emit("onTap", u, t), v < 300 && b - ct > 300 && (p && clearTimeout(p), p = setTimeout(function () {
                            u && (u.params.paginationHide && u.paginationContainer.length > 0 && !n(t.target).hasClass(u.params.bulletClass) && u.paginationContainer.toggleClass(u.params.paginationHiddenClass), u.emit("onClick", u, t))
                        }, 300)), v < 300 && b - ct < 300 && (p && clearTimeout(p), u.emit("onDoubleTap", u, t))), ct = Date.now(), setTimeout(function () {
                            u && (u.allowClick = !0)
                        }, 0), !h || !c || !u.swipeDirection || 0 === u.touches.diff || f === o) return void (h = c = !1);
                        if (h = c = !1, s = u.params.followFinger ? u.rtl ? u.translate : -u.translate : -f, u.params.freeMode) {
                            if (s < -u.minTranslate()) return void u.slideTo(u.activeIndex);
                            if (s > -u.maxTranslate()) return void (u.slides.length < u.snapGrid.length ? u.slideTo(u.snapGrid.length - 1) : u.slideTo(u.slides.length - 1));
                            if (u.params.freeModeMomentum) {
                                if (l.length > 1) {
                                    var ut = l.pop(),
                                        et = l.pop(),
                                        st = ut.position - et.position,
                                        ot = ut.time - et.time;
                                    u.velocity = st / ot;
                                    u.velocity = u.velocity / 2;
                                    Math.abs(u.velocity) < u.params.freeModeMinimumVelocity && (u.velocity = 0);
                                    (ot > 150 || (new window.Date).getTime() - ut.time > 300) && (u.velocity = 0)
                                } else u.velocity = 0;
                                u.velocity = u.velocity * u.params.freeModeMomentumVelocityRatio;
                                l.length = 0;
                                var k = 1e3 * u.params.freeModeMomentumRatio,
                                    ht = u.velocity * k,
                                    i = u.translate + ht;
                                if (u.rtl && (i = -i), it = !1, y = 20 * Math.abs(u.velocity) * u.params.freeModeMomentumBounceRatio, i < u.maxTranslate()) u.params.freeModeMomentumBounce ? (i + u.maxTranslate() < -y && (i = u.maxTranslate() - y), nt = u.maxTranslate(), it = !0, tt = !0) : i = u.maxTranslate();
                                else if (i > u.minTranslate()) u.params.freeModeMomentumBounce ? (i - u.minTranslate() > y && (i = u.minTranslate() + y), nt = u.minTranslate(), it = !0, tt = !0) : i = u.minTranslate();
                                else if (u.params.freeModeSticky) {
                                    for (a = 0, a = 0; a < u.snapGrid.length; a += 1)
                                        if (u.snapGrid[a] > -i) {
                                            w = a;
                                            break
                                        }
                                    i = Math.abs(u.snapGrid[w] - i) < Math.abs(u.snapGrid[w - 1] - i) || "next" === u.swipeDirection ? u.snapGrid[w] : u.snapGrid[w - 1];
                                    u.rtl || (i = -i)
                                }
                                if (0 !== u.velocity) k = u.rtl ? Math.abs((-i - u.translate) / u.velocity) : Math.abs((i - u.translate) / u.velocity);
                                else if (u.params.freeModeSticky) return void u.slideReset();
                                u.params.freeModeMomentumBounce && it ? (u.updateProgress(nt), u.setWrapperTransition(k), u.setWrapperTranslate(i), u.onTransitionStart(), u.animating = !0, u.wrapper.transitionEnd(function () {
                                    u && tt && (u.emit("onMomentumBounce", u), u.setWrapperTransition(u.params.speed), u.setWrapperTranslate(nt), u.wrapper.transitionEnd(function () {
                                        u && u.onTransitionEnd()
                                    }))
                                })) : u.velocity ? (u.updateProgress(i), u.setWrapperTransition(k), u.setWrapperTranslate(i), u.onTransitionStart(), u.animating || (u.animating = !0, u.wrapper.transitionEnd(function () {
                                    u && u.onTransitionEnd()
                                }))) : u.updateProgress(i);
                                u.updateActiveIndex()
                            }
                            return void ((!u.params.freeModeMomentum || v >= u.params.longSwipesMs) && (u.updateProgress(), u.updateActiveIndex()))
                        }
                        for (e = 0, rt = u.slidesSizesGrid[0], r = 0; r < u.slidesGrid.length; r += u.params.slidesPerGroup) "undefined" != typeof u.slidesGrid[r + u.params.slidesPerGroup] ? s >= u.slidesGrid[r] && s < u.slidesGrid[r + u.params.slidesPerGroup] && (e = r, rt = u.slidesGrid[r + u.params.slidesPerGroup] - u.slidesGrid[r]) : s >= u.slidesGrid[r] && (e = r, rt = u.slidesGrid[u.slidesGrid.length - 1] - u.slidesGrid[u.slidesGrid.length - 2]);
                        if (ft = (s - u.slidesGrid[e]) / rt, v > u.params.longSwipesMs) {
                            if (!u.params.longSwipes) return void u.slideTo(u.activeIndex);
                            "next" === u.swipeDirection && (ft >= u.params.longSwipesRatio ? u.slideTo(e + u.params.slidesPerGroup) : u.slideTo(e));
                            "prev" === u.swipeDirection && (ft > 1 - u.params.longSwipesRatio ? u.slideTo(e + u.params.slidesPerGroup) : u.slideTo(e))
                        } else {
                            if (!u.params.shortSwipes) return void u.slideTo(u.activeIndex);
                            "next" === u.swipeDirection && u.slideTo(e + u.params.slidesPerGroup);
                            "prev" === u.swipeDirection && u.slideTo(e)
                        }
                    }
                };
                u._slideTo = function (n, t) {
                    return u.slideTo(n, t, !0, !0)
                };
                u.slideTo = function (n, t, i, r) {
                    var f, e;
                    if ("undefined" == typeof i && (i = !0), "undefined" == typeof n && (n = 0), n < 0 && (n = 0), u.snapIndex = Math.floor(n / u.params.slidesPerGroup), u.snapIndex >= u.snapGrid.length && (u.snapIndex = u.snapGrid.length - 1), f = -u.snapGrid[u.snapIndex], u.params.autoplay && u.autoplaying && (r || !u.params.autoplayDisableOnInteraction ? u.pauseAutoplay(t) : u.stopAutoplay()), u.updateProgress(f), u.params.normalizeSlideIndex)
                        for (e = 0; e < u.slidesGrid.length; e++) -Math.floor(100 * f) >= Math.floor(100 * u.slidesGrid[e]) && (n = e);
                    return !(!u.params.allowSwipeToNext && f < u.translate && f < u.minTranslate()) && !(!u.params.allowSwipeToPrev && f > u.translate && f > u.maxTranslate() && (u.activeIndex || 0) !== n) && ("undefined" == typeof t && (t = u.params.speed), u.previousIndex = u.activeIndex || 0, u.activeIndex = n, u.updateRealIndex(), u.rtl && -f === u.translate || !u.rtl && f === u.translate ? (u.params.autoHeight && u.updateAutoHeight(), u.updateClasses(), "slide" !== u.params.effect && u.setWrapperTranslate(f), !1) : (u.updateClasses(), u.onTransitionStart(i), 0 === t || u.browser.lteIE9 ? (u.setWrapperTranslate(f), u.setWrapperTransition(0), u.onTransitionEnd(i)) : (u.setWrapperTranslate(f), u.setWrapperTransition(t), u.animating || (u.animating = !0, u.wrapper.transitionEnd(function () {
                        u && u.onTransitionEnd(i)
                    }))), !0))
                };
                u.onTransitionStart = function (n) {
                    "undefined" == typeof n && (n = !0);
                    u.params.autoHeight && u.updateAutoHeight();
                    u.lazy && u.lazy.onTransitionStart();
                    n && (u.emit("onTransitionStart", u), u.activeIndex !== u.previousIndex && (u.emit("onSlideChangeStart", u), u.activeIndex > u.previousIndex ? u.emit("onSlideNextStart", u) : u.emit("onSlidePrevStart", u)))
                };
                u.onTransitionEnd = function (n) {
                    u.animating = !1;
                    u.setWrapperTransition(0);
                    "undefined" == typeof n && (n = !0);
                    u.lazy && u.lazy.onTransitionEnd();
                    n && (u.emit("onTransitionEnd", u), u.activeIndex !== u.previousIndex && (u.emit("onSlideChangeEnd", u), u.activeIndex > u.previousIndex ? u.emit("onSlideNextEnd", u) : u.emit("onSlidePrevEnd", u)));
                    u.params.history && u.history && u.history.setHistory(u.params.history, u.activeIndex);
                    u.params.hashnav && u.hashnav && u.hashnav.setHash()
                };
                u.slideNext = function (n, t, i) {
                    return u.params.loop ? u.animating ? !1 : (u.fixLoop(), u.container[0].clientLeft, u.slideTo(u.activeIndex + u.params.slidesPerGroup, t, n, i)) : u.slideTo(u.activeIndex + u.params.slidesPerGroup, t, n, i)
                };
                u._slideNext = function (n) {
                    return u.slideNext(!0, n, !0)
                };
                u.slidePrev = function (n, t, i) {
                    return u.params.loop ? u.animating ? !1 : (u.fixLoop(), u.container[0].clientLeft, u.slideTo(u.activeIndex - 1, t, n, i)) : u.slideTo(u.activeIndex - 1, t, n, i)
                };
                u._slidePrev = function (n) {
                    return u.slidePrev(!0, n, !0)
                };
                u.slideReset = function (n, t) {
                    return u.slideTo(u.activeIndex, t, n)
                };
                u.disableTouchControl = function () {
                    return u.params.onlyExternal = !0, !0
                };
                u.enableTouchControl = function () {
                    return u.params.onlyExternal = !1, !0
                };
                u.setWrapperTransition = function (n, t) {
                    u.wrapper.transition(n);
                    "slide" !== u.params.effect && u.effects[u.params.effect] && u.effects[u.params.effect].setTransition(n);
                    u.params.parallax && u.parallax && u.parallax.setTransition(n);
                    u.params.scrollbar && u.scrollbar && u.scrollbar.setTransition(n);
                    u.params.control && u.controller && u.controller.setTransition(n, t);
                    u.emit("onSetTransition", u, n)
                };
                u.setWrapperTranslate = function (n, t, i) {
                    var r = 0,
                        f = 0,
                        o, e;
                    u.isHorizontal() ? r = u.rtl ? -n : n : f = n;
                    u.params.roundLengths && (r = b(r), f = b(f));
                    u.params.virtualTranslate || (u.support.transforms3d ? u.wrapper.transform("translate3d(" + r + "px, " + f + "px, 0px)") : u.wrapper.transform("translate(" + r + "px, " + f + "px)"));
                    u.translate = u.isHorizontal() ? r : f;
                    e = u.maxTranslate() - u.minTranslate();
                    o = 0 === e ? 0 : (n - u.minTranslate()) / e;
                    o !== u.progress && u.updateProgress(n);
                    t && u.updateActiveIndex();
                    "slide" !== u.params.effect && u.effects[u.params.effect] && u.effects[u.params.effect].setTranslate(u.translate);
                    u.params.parallax && u.parallax && u.parallax.setTranslate(u.translate);
                    u.params.scrollbar && u.scrollbar && u.scrollbar.setTranslate(u.translate);
                    u.params.control && u.controller && u.controller.setTranslate(u.translate, i);
                    u.emit("onSetTranslate", u, u.translate)
                };
                u.getTranslate = function (n, t) {
                    var f, i, r, e;
                    return "undefined" == typeof t && (t = "x"), u.params.virtualTranslate ? u.rtl ? -u.translate : u.translate : (r = window.getComputedStyle(n, null), window.WebKitCSSMatrix ? (i = r.transform || r.webkitTransform, i.split(",").length > 6 && (i = i.split(", ").map(function (n) {
                        return n.replace(",", ".")
                    }).join(", ")), e = new window.WebKitCSSMatrix("none" === i ? "" : i)) : (e = r.MozTransform || r.OTransform || r.MsTransform || r.msTransform || r.transform || r.getPropertyValue("transform").replace("translate(", "matrix(1, 0, 0, 1,"), f = e.toString().split(",")), "x" === t && (i = window.WebKitCSSMatrix ? e.m41 : 16 === f.length ? parseFloat(f[12]) : parseFloat(f[4])), "y" === t && (i = window.WebKitCSSMatrix ? e.m42 : 16 === f.length ? parseFloat(f[13]) : parseFloat(f[5])), u.rtl && i && (i = -i), i || 0)
                };
                u.getWrapperTranslate = function (n) {
                    return "undefined" == typeof n && (n = u.isHorizontal() ? "x" : "y"), u.getTranslate(u.wrapper[0], n)
                };
                u.observers = [];
                u.initObservers = function () {
                    if (u.params.observeParents)
                        for (var t = u.container.parents(), n = 0; n < t.length; n++) ft(t[n]);
                    ft(u.container[0], {
                        childList: !1
                    });
                    ft(u.wrapper[0], {
                        attributes: !1
                    })
                };
                u.disconnectObservers = function () {
                    for (var n = 0; n < u.observers.length; n++) u.observers[n].disconnect();
                    u.observers = []
                };
                u.createLoop = function () {
                    var i, t, r, f;
                    for (u.wrapper.children("." + u.params.slideClass + "." + u.params.slideDuplicateClass).remove(), i = u.wrapper.children("." + u.params.slideClass), "auto" !== u.params.slidesPerView || u.params.loopedSlides || (u.params.loopedSlides = i.length), u.loopedSlides = parseInt(u.params.loopedSlides || u.params.slidesPerView, 10), u.loopedSlides = u.loopedSlides + u.params.loopAdditionalSlides, u.loopedSlides > i.length && (u.loopedSlides = i.length), r = [], f = [], i.each(function (t, e) {
                        var o = n(this);
                        t < u.loopedSlides && f.push(e);
                        t < i.length && t >= i.length - u.loopedSlides && r.push(e);
                        o.attr("data-swiper-slide-index", t)
                    }), t = 0; t < f.length; t++) u.wrapper.append(n(f[t].cloneNode(!0)).addClass(u.params.slideDuplicateClass));
                    for (t = r.length - 1; t >= 0; t--) u.wrapper.prepend(n(r[t].cloneNode(!0)).addClass(u.params.slideDuplicateClass))
                };
                u.destroyLoop = function () {
                    u.wrapper.children("." + u.params.slideClass + "." + u.params.slideDuplicateClass).remove();
                    u.slides.removeAttr("data-swiper-slide-index")
                };
                u.reLoop = function (n) {
                    var t = u.activeIndex - u.loopedSlides;
                    u.destroyLoop();
                    u.createLoop();
                    u.updateSlidesSize();
                    n && u.slideTo(t + u.loopedSlides, 0, !1)
                };
                u.fixLoop = function () {
                    var n;
                    u.activeIndex < u.loopedSlides ? (n = u.slides.length - 3 * u.loopedSlides + u.activeIndex, n += u.loopedSlides, u.slideTo(n, 0, !1, !0)) : ("auto" === u.params.slidesPerView && u.activeIndex >= 2 * u.loopedSlides || u.activeIndex > u.slides.length - 2 * u.params.slidesPerView) && (n = -u.slides.length + u.activeIndex + u.loopedSlides, n += u.loopedSlides, u.slideTo(n, 0, !1, !0))
                };
                u.appendSlide = function (n) {
                    if (u.params.loop && u.destroyLoop(), "object" == typeof n && n.length)
                        for (var t = 0; t < n.length; t++) n[t] && u.wrapper.append(n[t]);
                    else u.wrapper.append(n);
                    u.params.loop && u.createLoop();
                    u.params.observer && u.support.observer || u.update(!0)
                };
                u.prependSlide = function (n) {
                    var i, t;
                    if (u.params.loop && u.destroyLoop(), i = u.activeIndex + 1, "object" == typeof n && n.length) {
                        for (t = 0; t < n.length; t++) n[t] && u.wrapper.prepend(n[t]);
                        i = u.activeIndex + n.length
                    } else u.wrapper.prepend(n);
                    u.params.loop && u.createLoop();
                    u.params.observer && u.support.observer || u.update(!0);
                    u.slideTo(i, 0, !1)
                };
                u.removeSlide = function (n) {
                    var i, t, r;
                    if (u.params.loop && (u.destroyLoop(), u.slides = u.wrapper.children("." + u.params.slideClass)), t = u.activeIndex, "object" == typeof n && n.length) {
                        for (r = 0; r < n.length; r++) i = n[r], u.slides[i] && u.slides.eq(i).remove(), i < t && t--;
                        t = Math.max(t, 0)
                    } else i = n, u.slides[i] && u.slides.eq(i).remove(), i < t && t--, t = Math.max(t, 0);
                    u.params.loop && u.createLoop();
                    u.params.observer && u.support.observer || u.update(!0);
                    u.params.loop ? u.slideTo(t + u.loopedSlides, 0, !1) : u.slideTo(t, 0, !1)
                };
                u.removeAllSlides = function () {
                    for (var t = [], n = 0; n < u.slides.length; n++) t.push(n);
                    u.removeSlide(t)
                };
                u.effects = {
                    fade: {
                        setTranslate: function () {
                            for (var r, f, n = 0; n < u.slides.length; n++) {
                                var t = u.slides.eq(n),
                                    e = t[0].swiperSlideOffset,
                                    i = -e;
                                u.params.virtualTranslate || (i -= u.translate);
                                r = 0;
                                u.isHorizontal() || (r = i, i = 0);
                                f = u.params.fade.crossFade ? Math.max(1 - Math.abs(t[0].progress), 0) : 1 + Math.min(Math.max(t[0].progress, -1), 0);
                                t.css({
                                    opacity: f
                                }).transform("translate3d(" + i + "px, " + r + "px, 0px)")
                            }
                        },
                        setTransition: function (n) {
                            if (u.slides.transition(n), u.params.virtualTranslate && 0 !== n) {
                                var t = !1;
                                u.slides.transitionEnd(function () {
                                    if (!t && u) {
                                        t = !0;
                                        u.animating = !1;
                                        for (var i = ["webkitTransitionEnd", "transitionend", "oTransitionEnd", "MSTransitionEnd", "msTransitionEnd"], n = 0; n < i.length; n++) u.wrapper.trigger(i[n])
                                    }
                                })
                            }
                        }
                    },
                    flip: {
                        setTranslate: function () {
                            for (var t, i, r, f, o = 0; o < u.slides.length; o++) {
                                t = u.slides.eq(o);
                                i = t[0].progress;
                                u.params.flip.limitRotation && (i = Math.max(Math.min(t[0].progress, 1), -1));
                                var l = t[0].swiperSlideOffset,
                                    a = -180 * i,
                                    e = a,
                                    h = 0,
                                    s = -l,
                                    c = 0;
                                (u.isHorizontal() ? u.rtl && (e = -e) : (c = s, s = 0, h = -e, e = 0), t[0].style.zIndex = -Math.abs(Math.round(i)) + u.slides.length, u.params.flip.slideShadows) && (r = u.isHorizontal() ? t.find(".swiper-slide-shadow-left") : t.find(".swiper-slide-shadow-top"), f = u.isHorizontal() ? t.find(".swiper-slide-shadow-right") : t.find(".swiper-slide-shadow-bottom"), 0 === r.length && (r = n('<div class="swiper-slide-shadow-' + (u.isHorizontal() ? "left" : "top") + '"><\/div>'), t.append(r)), 0 === f.length && (f = n('<div class="swiper-slide-shadow-' + (u.isHorizontal() ? "right" : "bottom") + '"><\/div>'), t.append(f)), r.length && (r[0].style.opacity = Math.max(-i, 0)), f.length && (f[0].style.opacity = Math.max(i, 0)));
                                t.transform("translate3d(" + s + "px, " + c + "px, 0px) rotateX(" + h + "deg) rotateY(" + e + "deg)")
                            }
                        },
                        setTransition: function (t) {
                            if (u.slides.transition(t).find(".swiper-slide-shadow-top, .swiper-slide-shadow-right, .swiper-slide-shadow-bottom, .swiper-slide-shadow-left").transition(t), u.params.virtualTranslate && 0 !== t) {
                                var i = !1;
                                u.slides.eq(u.activeIndex).transitionEnd(function () {
                                    if (!i && u && n(this).hasClass(u.params.slideActiveClass)) {
                                        i = !0;
                                        u.animating = !1;
                                        for (var r = ["webkitTransitionEnd", "transitionend", "oTransitionEnd", "MSTransitionEnd", "msTransitionEnd"], t = 0; t < r.length; t++) u.wrapper.trigger(r[t])
                                    }
                                })
                            }
                        }
                    },
                    cube: {
                        setTranslate: function () {
                            var t, e = 0,
                                i, y, h, c, b;
                            for (u.params.cube.shadow && (u.isHorizontal() ? (t = u.wrapper.find(".swiper-cube-shadow"), 0 === t.length && (t = n('<div class="swiper-cube-shadow"><\/div>'), u.wrapper.append(t)), t.css({
                                height: u.width + "px"
                            })) : (t = u.container.find(".swiper-cube-shadow"), 0 === t.length && (t = n('<div class="swiper-cube-shadow"><\/div>'), u.container.append(t)))), i = 0; i < u.slides.length; i++) {
                                var f = u.slides.eq(i),
                                    o = 90 * i,
                                    l = Math.floor(o / 360);
                                u.rtl && (o = -o, l = Math.floor(-o / 360));
                                var s = Math.max(Math.min(f[0].progress, 1), -1),
                                    r = 0,
                                    v = 0,
                                    a = 0;
                                i % 4 == 0 ? (r = 4 * -l * u.size, a = 0) : (i - 1) % 4 == 0 ? (r = 0, a = 4 * -l * u.size) : (i - 2) % 4 == 0 ? (r = u.size + 4 * l * u.size, a = u.size) : (i - 3) % 4 == 0 && (r = -u.size, a = 3 * u.size + 4 * u.size * l);
                                u.rtl && (r = -r);
                                u.isHorizontal() || (v = r, r = 0);
                                y = "rotateX(" + (u.isHorizontal() ? 0 : -o) + "deg) rotateY(" + (u.isHorizontal() ? o : 0) + "deg) translate3d(" + r + "px, " + v + "px, " + a + "px)";
                                (s <= 1 && s > -1 && (e = 90 * i + 90 * s, u.rtl && (e = 90 * -i - 90 * s)), f.transform(y), u.params.cube.slideShadows) && (h = u.isHorizontal() ? f.find(".swiper-slide-shadow-left") : f.find(".swiper-slide-shadow-top"), c = u.isHorizontal() ? f.find(".swiper-slide-shadow-right") : f.find(".swiper-slide-shadow-bottom"), 0 === h.length && (h = n('<div class="swiper-slide-shadow-' + (u.isHorizontal() ? "left" : "top") + '"><\/div>'), f.append(h)), 0 === c.length && (c = n('<div class="swiper-slide-shadow-' + (u.isHorizontal() ? "right" : "bottom") + '"><\/div>'), f.append(c)), h.length && (h[0].style.opacity = Math.max(-s, 0)), c.length && (c[0].style.opacity = Math.max(s, 0)))
                            }
                            if (u.wrapper.css({
                                "-webkit-transform-origin": "50% 50% -" + u.size / 2 + "px",
                                "-moz-transform-origin": "50% 50% -" + u.size / 2 + "px",
                                "-ms-transform-origin": "50% 50% -" + u.size / 2 + "px",
                                "transform-origin": "50% 50% -" + u.size / 2 + "px"
                            }), u.params.cube.shadow)
                                if (u.isHorizontal()) t.transform("translate3d(0px, " + (u.width / 2 + u.params.cube.shadowOffset) + "px, " + -u.width / 2 + "px) rotateX(90deg) rotateZ(0deg) scale(" + u.params.cube.shadowScale + ")");
                                else {
                                    var p = Math.abs(e) - 90 * Math.floor(Math.abs(e) / 90),
                                        k = 1.5 - (Math.sin(2 * p * Math.PI / 360) / 2 + Math.cos(2 * p * Math.PI / 360) / 2),
                                        d = u.params.cube.shadowScale,
                                        w = u.params.cube.shadowScale / k,
                                        g = u.params.cube.shadowOffset;
                                    t.transform("scale3d(" + d + ", 1, " + w + ") translate3d(0px, " + (u.height / 2 + g) + "px, " + -u.height / 2 / w + "px) rotateX(-90deg)")
                                }
                            b = u.isSafari || u.isUiWebView ? -u.size / 2 : 0;
                            u.wrapper.transform("translate3d(0px,0," + b + "px) rotateX(" + (u.isHorizontal() ? 0 : e) + "deg) rotateY(" + (u.isHorizontal() ? -e : 0) + "deg)")
                        },
                        setTransition: function (n) {
                            u.slides.transition(n).find(".swiper-slide-shadow-top, .swiper-slide-shadow-right, .swiper-slide-shadow-bottom, .swiper-slide-shadow-left").transition(n);
                            u.params.cube.shadow && !u.isHorizontal() && u.container.find(".swiper-cube-shadow").transition(n)
                        }
                    },
                    coverflow: {
                        setTranslate: function () {
                            for (var w, r, f, b, a = u.translate, v = u.isHorizontal() ? -a + u.width / 2 : -a + u.height / 2, y = u.isHorizontal() ? u.params.coverflow.rotate : -u.params.coverflow.rotate, k = u.params.coverflow.depth, e = 0, d = u.slides.length; e < d; e++) {
                                var i = u.slides.eq(e),
                                    p = u.slidesSizesGrid[e],
                                    g = i[0].swiperSlideOffset,
                                    t = (v - g - p / 2) / p * u.params.coverflow.modifier,
                                    o = u.isHorizontal() ? y * t : 0,
                                    s = u.isHorizontal() ? 0 : y * t,
                                    h = -k * Math.abs(t),
                                    c = u.isHorizontal() ? 0 : u.params.coverflow.stretch * t,
                                    l = u.isHorizontal() ? u.params.coverflow.stretch * t : 0;
                                Math.abs(l) < .001 && (l = 0);
                                Math.abs(c) < .001 && (c = 0);
                                Math.abs(h) < .001 && (h = 0);
                                Math.abs(o) < .001 && (o = 0);
                                Math.abs(s) < .001 && (s = 0);
                                w = "translate3d(" + l + "px," + c + "px," + h + "px)  rotateX(" + s + "deg) rotateY(" + o + "deg)";
                                (i.transform(w), i[0].style.zIndex = -Math.abs(Math.round(t)) + 1, u.params.coverflow.slideShadows) && (r = u.isHorizontal() ? i.find(".swiper-slide-shadow-left") : i.find(".swiper-slide-shadow-top"), f = u.isHorizontal() ? i.find(".swiper-slide-shadow-right") : i.find(".swiper-slide-shadow-bottom"), 0 === r.length && (r = n('<div class="swiper-slide-shadow-' + (u.isHorizontal() ? "left" : "top") + '"><\/div>'), i.append(r)), 0 === f.length && (f = n('<div class="swiper-slide-shadow-' + (u.isHorizontal() ? "right" : "bottom") + '"><\/div>'), i.append(f)), r.length && (r[0].style.opacity = t > 0 ? t : 0), f.length && (f[0].style.opacity = -t > 0 ? -t : 0))
                            }
                            u.browser.ie && (b = u.wrapper[0].style, b.perspectiveOrigin = v + "px 50%")
                        },
                        setTransition: function (n) {
                            u.slides.transition(n).find(".swiper-slide-shadow-top, .swiper-slide-shadow-right, .swiper-slide-shadow-bottom, .swiper-slide-shadow-left").transition(n)
                        }
                    }
                };
                u.lazy = {
                    initialImageLoaded: !1,
                    loadImageInSlide: function (t, i) {
                        if ("undefined" != typeof t && ("undefined" == typeof i && (i = !0), 0 !== u.slides.length)) {
                            var r = u.slides.eq(t),
                                f = r.find("." + u.params.lazyLoadingClass + ":not(." + u.params.lazyStatusLoadedClass + "):not(." + u.params.lazyStatusLoadingClass + ")");
                            !r.hasClass(u.params.lazyLoadingClass) || r.hasClass(u.params.lazyStatusLoadedClass) || r.hasClass(u.params.lazyStatusLoadingClass) || (f = f.add(r[0]));
                            0 !== f.length && f.each(function () {
                                var t = n(this);
                                t.addClass(u.params.lazyStatusLoadingClass);
                                var f = t.attr("data-background"),
                                    e = t.attr("data-src"),
                                    o = t.attr("data-srcset"),
                                    s = t.attr("data-sizes");
                                u.loadImage(t[0], e || f, o, s, !1, function () {
                                    var n, h, c;
                                    (f ? (t.css("background-image", 'url("' + f + '")'), t.removeAttr("data-background")) : (o && (t.attr("srcset", o), t.removeAttr("data-srcset")), s && (t.attr("sizes", s), t.removeAttr("data-sizes")), e && (t.attr("src", e), t.removeAttr("data-src"))), t.addClass(u.params.lazyStatusLoadedClass).removeClass(u.params.lazyStatusLoadingClass), r.find("." + u.params.lazyPreloaderClass + ", ." + u.params.preloaderClass).remove(), u.params.loop && i) && (n = r.attr("data-swiper-slide-index"), r.hasClass(u.params.slideDuplicateClass) ? (h = u.wrapper.children('[data-swiper-slide-index="' + n + '"]:not(.' + u.params.slideDuplicateClass + ")"), u.lazy.loadImageInSlide(h.index(), !1)) : (c = u.wrapper.children("." + u.params.slideDuplicateClass + '[data-swiper-slide-index="' + n + '"]'), u.lazy.loadImageInSlide(c.index(), !1)));
                                    u.emit("onLazyImageReady", u, r[0], t[0])
                                });
                                u.emit("onLazyImageLoad", u, r[0], t[0])
                            })
                        }
                    },
                    load: function () {
                        var t, i = u.params.slidesPerView,
                            f, e;
                        if ("auto" === i && (i = 0), u.lazy.initialImageLoaded || (u.lazy.initialImageLoaded = !0), u.params.watchSlidesVisibility) u.wrapper.children("." + u.params.slideVisibleClass).each(function () {
                            u.lazy.loadImageInSlide(n(this).index())
                        });
                        else if (i > 1)
                            for (t = u.activeIndex; t < u.activeIndex + i; t++) u.slides[t] && u.lazy.loadImageInSlide(t);
                        else u.lazy.loadImageInSlide(u.activeIndex);
                        if (u.params.lazyLoadingInPrevNext)
                            if (i > 1 || u.params.lazyLoadingInPrevNextAmount && u.params.lazyLoadingInPrevNextAmount > 1) {
                                var o = u.params.lazyLoadingInPrevNextAmount,
                                    r = i,
                                    s = Math.min(u.activeIndex + r + Math.max(o, r), u.slides.length),
                                    h = Math.max(u.activeIndex - Math.max(r, o), 0);
                                for (t = u.activeIndex + i; t < s; t++) u.slides[t] && u.lazy.loadImageInSlide(t);
                                for (t = h; t < u.activeIndex; t++) u.slides[t] && u.lazy.loadImageInSlide(t)
                            } else f = u.wrapper.children("." + u.params.slideNextClass), f.length > 0 && u.lazy.loadImageInSlide(f.index()), e = u.wrapper.children("." + u.params.slidePrevClass), e.length > 0 && u.lazy.loadImageInSlide(e.index())
                    },
                    onTransitionStart: function () {
                        u.params.lazyLoading && (u.params.lazyLoadingOnTransitionStart || !u.params.lazyLoadingOnTransitionStart && !u.lazy.initialImageLoaded) && u.lazy.load()
                    },
                    onTransitionEnd: function () {
                        u.params.lazyLoading && !u.params.lazyLoadingOnTransitionStart && u.lazy.load()
                    }
                };
                u.scrollbar = {
                    isTouched: !1,
                    setDragPosition: function (n) {
                        var i = u.scrollbar,
                            e = u.isHorizontal() ? "touchstart" === n.type || "touchmove" === n.type ? n.targetTouches[0].pageX : n.pageX || n.clientX : "touchstart" === n.type || "touchmove" === n.type ? n.targetTouches[0].pageY : n.pageY || n.clientY,
                            t = e - i.track.offset()[u.isHorizontal() ? "left" : "top"] - i.dragSize / 2,
                            r = -u.minTranslate() * i.moveDivider,
                            f = -u.maxTranslate() * i.moveDivider;
                        t < r ? t = r : t > f && (t = f);
                        t = -t / i.moveDivider;
                        u.updateProgress(t);
                        u.setWrapperTranslate(t, !0)
                    },
                    dragStart: function (n) {
                        var t = u.scrollbar;
                        t.isTouched = !0;
                        n.preventDefault();
                        n.stopPropagation();
                        t.setDragPosition(n);
                        clearTimeout(t.dragTimeout);
                        t.track.transition(0);
                        u.params.scrollbarHide && t.track.css("opacity", 1);
                        u.wrapper.transition(100);
                        t.drag.transition(100);
                        u.emit("onScrollbarDragStart", u)
                    },
                    dragMove: function (n) {
                        var t = u.scrollbar;
                        t.isTouched && (n.preventDefault ? n.preventDefault() : n.returnValue = !1, t.setDragPosition(n), u.wrapper.transition(0), t.track.transition(0), t.drag.transition(0), u.emit("onScrollbarDragMove", u))
                    },
                    dragEnd: function () {
                        var n = u.scrollbar;
                        n.isTouched && (n.isTouched = !1, u.params.scrollbarHide && (clearTimeout(n.dragTimeout), n.dragTimeout = setTimeout(function () {
                            n.track.css("opacity", 0);
                            n.track.transition(400)
                        }, 1e3)), u.emit("onScrollbarDragEnd", u), u.params.scrollbarSnapOnRelease && u.slideReset())
                    },
                    draggableEvents: function () {
                        return u.params.simulateTouch !== !1 || u.support.touch ? u.touchEvents : u.touchEventsDesktop
                    }(),
                    enableDraggable: function () {
                        var t = u.scrollbar,
                            i = u.support.touch ? t.track : document;
                        n(t.track).on(t.draggableEvents.start, t.dragStart);
                        n(i).on(t.draggableEvents.move, t.dragMove);
                        n(i).on(t.draggableEvents.end, t.dragEnd)
                    },
                    disableDraggable: function () {
                        var t = u.scrollbar,
                            i = u.support.touch ? t.track : document;
                        n(t.track).off(t.draggableEvents.start, t.dragStart);
                        n(i).off(t.draggableEvents.move, t.dragMove);
                        n(i).off(t.draggableEvents.end, t.dragEnd)
                    },
                    set: function () {
                        if (u.params.scrollbar) {
                            var t = u.scrollbar;
                            t.track = n(u.params.scrollbar);
                            u.params.uniqueNavElements && "string" == typeof u.params.scrollbar && t.track.length > 1 && 1 === u.container.find(u.params.scrollbar).length && (t.track = u.container.find(u.params.scrollbar));
                            t.drag = t.track.find(".swiper-scrollbar-drag");
                            0 === t.drag.length && (t.drag = n('<div class="swiper-scrollbar-drag"><\/div>'), t.track.append(t.drag));
                            t.drag[0].style.width = "";
                            t.drag[0].style.height = "";
                            t.trackSize = u.isHorizontal() ? t.track[0].offsetWidth : t.track[0].offsetHeight;
                            t.divider = u.size / u.virtualSize;
                            t.moveDivider = t.divider * (t.trackSize / u.size);
                            t.dragSize = t.trackSize * t.divider;
                            u.isHorizontal() ? t.drag[0].style.width = t.dragSize + "px" : t.drag[0].style.height = t.dragSize + "px";
                            t.track[0].style.display = t.divider >= 1 ? "none" : "";
                            u.params.scrollbarHide && (t.track[0].style.opacity = 0)
                        }
                    },
                    setTranslate: function () {
                        if (u.params.scrollbar) {
                            var t, n = u.scrollbar,
                                i = (u.translate || 0, n.dragSize);
                            t = (n.trackSize - n.dragSize) * u.progress;
                            u.rtl && u.isHorizontal() ? (t = -t, t > 0 ? (i = n.dragSize - t, t = 0) : -t + n.dragSize > n.trackSize && (i = n.trackSize + t)) : t < 0 ? (i = n.dragSize + t, t = 0) : t + n.dragSize > n.trackSize && (i = n.trackSize - t);
                            u.isHorizontal() ? (u.support.transforms3d ? n.drag.transform("translate3d(" + t + "px, 0, 0)") : n.drag.transform("translateX(" + t + "px)"), n.drag[0].style.width = i + "px") : (u.support.transforms3d ? n.drag.transform("translate3d(0px, " + t + "px, 0)") : n.drag.transform("translateY(" + t + "px)"), n.drag[0].style.height = i + "px");
                            u.params.scrollbarHide && (clearTimeout(n.timeout), n.track[0].style.opacity = 1, n.timeout = setTimeout(function () {
                                n.track[0].style.opacity = 0;
                                n.track.transition(400)
                            }, 1e3))
                        }
                    },
                    setTransition: function (n) {
                        u.params.scrollbar && u.scrollbar.drag.transition(n)
                    }
                };
                u.controller = {
                    LinearSpline: function (n, t) {
                        var i, r, u;
                        this.x = n;
                        this.y = t;
                        this.lastIndex = n.length - 1;
                        this.x.length;
                        this.interpolate = function (n) {
                            return n ? (r = u(this.x, n), i = r - 1, (n - this.x[i]) * (this.y[r] - this.y[i]) / (this.x[r] - this.x[i]) + this.y[i]) : 0
                        };
                        u = function () {
                            var n, t, i;
                            return function (r, u) {
                                for (t = -1, n = r.length; n - t > 1;) r[i = n + t >> 1] <= u ? t = i : n = i;
                                return n
                            }
                        }()
                    },
                    getInterpolateFunction: function (n) {
                        u.controller.spline || (u.controller.spline = u.params.loop ? new u.controller.LinearSpline(u.slidesGrid, n.slidesGrid) : new u.controller.LinearSpline(u.snapGrid, n.snapGrid))
                    },
                    setTranslate: function (n, i) {
                        function o(t) {
                            n = t.rtl && "horizontal" === t.params.direction ? -u.translate : u.translate;
                            "slide" === u.params.controlBy && (u.controller.getInterpolateFunction(t), f = -u.controller.spline.interpolate(-n));
                            f && "container" !== u.params.controlBy || (s = (t.maxTranslate() - t.minTranslate()) / (u.maxTranslate() - u.minTranslate()), f = (n - u.minTranslate()) * s + t.minTranslate());
                            u.params.controlInverse && (f = t.maxTranslate() - f);
                            t.updateProgress(f);
                            t.setWrapperTranslate(f, !1, u);
                            t.updateActiveIndex()
                        }

                        var s, f, r = u.params.control,
                            e;
                        if (u.isArray(r))
                            for (e = 0; e < r.length; e++) r[e] !== i && r[e] instanceof t && o(r[e]);
                        else r instanceof t && i !== r && o(r)
                    },
                    setTransition: function (n, i) {
                        function e(t) {
                            t.setWrapperTransition(n, u);
                            0 !== n && (t.onTransitionStart(), t.wrapper.transitionEnd(function () {
                                r && (t.params.loop && "slide" === u.params.controlBy && t.fixLoop(), t.onTransitionEnd())
                            }))
                        }

                        var f, r = u.params.control;
                        if (u.isArray(r))
                            for (f = 0; f < r.length; f++) r[f] !== i && r[f] instanceof t && e(r[f]);
                        else r instanceof t && i !== r && e(r)
                    }
                };
                u.hashnav = {
                    onHashCange: function () {
                        var n = document.location.hash.replace("#", ""),
                            t = u.slides.eq(u.activeIndex).attr("data-hash");
                        n !== t && u.slideTo(u.wrapper.children("." + u.params.slideClass + '[data-hash="' + n + '"]').index())
                    },
                    attachEvents: function (t) {
                        var i = t ? "off" : "on";
                        n(window)[i]("hashchange", u.hashnav.onHashCange)
                    },
                    setHash: function () {
                        if (u.hashnav.initialized && u.params.hashnav)
                            if (u.params.replaceState && window.history && window.history.replaceState) window.history.replaceState(null, null, "#" + u.slides.eq(u.activeIndex).attr("data-hash") || "");
                            else {
                                var n = u.slides.eq(u.activeIndex),
                                    t = n.attr("data-hash") || n.attr("data-history");
                                document.location.hash = t || ""
                            }
                    },
                    init: function () {
                        var t, n, r, f;
                        if (u.params.hashnav && !u.params.history) {
                            if (u.hashnav.initialized = !0, t = document.location.hash.replace("#", ""), t)
                                for (var i = 0, e = u.slides.length; i < e; i++) n = u.slides.eq(i), r = n.attr("data-hash") || n.attr("data-history"), r !== t || n.hasClass(u.params.slideDuplicateClass) || (f = n.index(), u.slideTo(f, 0, u.params.runCallbacksOnInit, !0));
                            u.params.hashnavWatchState && u.hashnav.attachEvents()
                        }
                    },
                    destroy: function () {
                        u.params.hashnavWatchState && u.hashnav.attachEvents(!0)
                    }
                };
                u.history = {
                    init: function () {
                        if (u.params.history) {
                            if (!window.history || !window.history.pushState) return u.params.history = !1, void (u.params.hashnav = !0);
                            u.history.initialized = !0;
                            this.paths = this.getPathValues();
                            (this.paths.key || this.paths.value) && (this.scrollToSlide(0, this.paths.value, u.params.runCallbacksOnInit), u.params.replaceState || window.addEventListener("popstate", this.setHistoryPopState))
                        }
                    },
                    setHistoryPopState: function () {
                        u.history.paths = u.history.getPathValues();
                        u.history.scrollToSlide(u.params.speed, u.history.paths.value, !1)
                    },
                    getPathValues: function () {
                        var n = window.location.pathname.slice(1).split("/"),
                            t = n.length,
                            i = n[t - 2],
                            r = n[t - 1];
                        return {
                            key: i,
                            value: r
                        }
                    },
                    setHistory: function (n, t) {
                        if (u.history.initialized && u.params.history) {
                            var r = u.slides.eq(t),
                                i = this.slugify(r.attr("data-history"));
                            window.location.pathname.includes(n) || (i = n + "/" + i);
                            u.params.replaceState ? window.history.replaceState(null, null, i) : window.history.pushState(null, null, i)
                        }
                    },
                    slugify: function (n) {
                        return n.toString().toLowerCase().replace(/\s+/g, "-").replace(/[^\w\-]+/g, "").replace(/\-\-+/g, "-").replace(/^-+/, "").replace(/-+$/, "")
                    },
                    scrollToSlide: function (n, t, i) {
                        var r, e, f, o, s;
                        if (t)
                            for (r = 0, e = u.slides.length; r < e; r++) f = u.slides.eq(r), o = this.slugify(f.attr("data-history")), o !== t || f.hasClass(u.params.slideDuplicateClass) || (s = f.index(), u.slideTo(s, n, i));
                        else u.slideTo(0, n, i)
                    }
                };
                u.disableKeyboardControl = function () {
                    u.params.keyboardControl = !1;
                    n(document).off("keydown", vt)
                };
                u.enableKeyboardControl = function () {
                    u.params.keyboardControl = !0;
                    n(document).on("keydown", vt)
                };
                u.mousewheel = {
                    event: !1,
                    lastScrollTime: (new window.Date).getTime()
                };
                u.params.mousewheelControl && (u.mousewheel.event = navigator.userAgent.indexOf("firefox") > -1 ? "DOMMouseScroll" : bt() ? "wheel" : "mousewheel");
                u.disableMousewheelControl = function () {
                    if (!u.mousewheel.event) return !1;
                    var t = u.container;
                    return "container" !== u.params.mousewheelEventsTarged && (t = n(u.params.mousewheelEventsTarged)), t.off(u.mousewheel.event, yt), !0
                };
                u.enableMousewheelControl = function () {
                    if (!u.mousewheel.event) return !1;
                    var t = u.container;
                    return "container" !== u.params.mousewheelEventsTarged && (t = n(u.params.mousewheelEventsTarged)), t.on(u.mousewheel.event, yt), !0
                };
                u.parallax = {
                    setTranslate: function () {
                        u.container.children("[data-swiper-parallax], [data-swiper-parallax-x], [data-swiper-parallax-y]").each(function () {
                            pt(this, u.progress)
                        });
                        u.slides.each(function () {
                            var t = n(this);
                            t.find("[data-swiper-parallax], [data-swiper-parallax-x], [data-swiper-parallax-y]").each(function () {
                                var n = Math.min(Math.max(t[0].progress, -1), 1);
                                pt(this, n)
                            })
                        })
                    },
                    setTransition: function (t) {
                        "undefined" == typeof t && (t = u.params.speed);
                        u.container.find("[data-swiper-parallax], [data-swiper-parallax-x], [data-swiper-parallax-y]").each(function () {
                            var i = n(this),
                                r = parseInt(i.attr("data-swiper-parallax-duration"), 10) || t;
                            0 === t && (r = 0);
                            i.transition(r)
                        })
                    }
                };
                u.zoom = {
                    scale: 1,
                    currentScale: 1,
                    isScaling: !1,
                    gesture: {
                        slide: void 0,
                        slideWidth: void 0,
                        slideHeight: void 0,
                        image: void 0,
                        imageWrap: void 0,
                        zoomMax: u.params.zoomMax
                    },
                    image: {
                        isTouched: void 0,
                        isMoved: void 0,
                        currentX: void 0,
                        currentY: void 0,
                        minX: void 0,
                        minY: void 0,
                        maxX: void 0,
                        maxY: void 0,
                        width: void 0,
                        height: void 0,
                        startX: void 0,
                        startY: void 0,
                        touchesStart: {},
                        touchesCurrent: {}
                    },
                    velocity: {
                        x: void 0,
                        y: void 0,
                        prevPositionX: void 0,
                        prevPositionY: void 0,
                        prevTime: void 0
                    },
                    getDistanceBetweenTouches: function (n) {
                        if (n.targetTouches.length < 2) return 1;
                        var t = n.targetTouches[0].pageX,
                            i = n.targetTouches[0].pageY,
                            r = n.targetTouches[1].pageX,
                            u = n.targetTouches[1].pageY;
                        return Math.sqrt(Math.pow(r - t, 2) + Math.pow(u - i, 2))
                    },
                    onGestureStart: function (t) {
                        var i = u.zoom;
                        if (!u.support.gestures) {
                            if ("touchstart" !== t.type || "touchstart" === t.type && t.targetTouches.length < 2) return;
                            i.gesture.scaleStart = i.getDistanceBetweenTouches(t)
                        }
                        return i.gesture.slide && i.gesture.slide.length || (i.gesture.slide = n(this), 0 === i.gesture.slide.length && (i.gesture.slide = u.slides.eq(u.activeIndex)), i.gesture.image = i.gesture.slide.find("img, svg, canvas"), i.gesture.imageWrap = i.gesture.image.parent("." + u.params.zoomContainerClass), i.gesture.zoomMax = i.gesture.imageWrap.attr("data-swiper-zoom") || u.params.zoomMax, 0 !== i.gesture.imageWrap.length) ? (i.gesture.image.transition(0), void (i.isScaling = !0)) : void (i.gesture.image = void 0)
                    },
                    onGestureChange: function (n) {
                        var t = u.zoom;
                        if (!u.support.gestures) {
                            if ("touchmove" !== n.type || "touchmove" === n.type && n.targetTouches.length < 2) return;
                            t.gesture.scaleMove = t.getDistanceBetweenTouches(n)
                        }
                        t.gesture.image && 0 !== t.gesture.image.length && (t.scale = u.support.gestures ? n.scale * t.currentScale : t.gesture.scaleMove / t.gesture.scaleStart * t.currentScale, t.scale > t.gesture.zoomMax && (t.scale = t.gesture.zoomMax - 1 + Math.pow(t.scale - t.gesture.zoomMax + 1, .5)), t.scale < u.params.zoomMin && (t.scale = u.params.zoomMin + 1 - Math.pow(u.params.zoomMin - t.scale + 1, .5)), t.gesture.image.transform("translate3d(0,0,0) scale(" + t.scale + ")"))
                    },
                    onGestureEnd: function (n) {
                        var t = u.zoom;
                        !u.support.gestures && ("touchend" !== n.type || "touchend" === n.type && n.changedTouches.length < 2) || t.gesture.image && 0 !== t.gesture.image.length && (t.scale = Math.max(Math.min(t.scale, t.gesture.zoomMax), u.params.zoomMin), t.gesture.image.transition(u.params.speed).transform("translate3d(0,0,0) scale(" + t.scale + ")"), t.currentScale = t.scale, t.isScaling = !1, 1 === t.scale && (t.gesture.slide = void 0))
                    },
                    onTouchStart: function (n, t) {
                        var i = n.zoom;
                        i.gesture.image && 0 !== i.gesture.image.length && (i.image.isTouched || ("android" === n.device.os && t.preventDefault(), i.image.isTouched = !0, i.image.touchesStart.x = "touchstart" === t.type ? t.targetTouches[0].pageX : t.pageX, i.image.touchesStart.y = "touchstart" === t.type ? t.targetTouches[0].pageY : t.pageY))
                    },
                    onTouchMove: function (n) {
                        var t = u.zoom,
                            i, r;
                        if (t.gesture.image && 0 !== t.gesture.image.length && (u.allowClick = !1, t.image.isTouched && t.gesture.slide) && (t.image.isMoved || (t.image.width = t.gesture.image[0].offsetWidth, t.image.height = t.gesture.image[0].offsetHeight, t.image.startX = u.getTranslate(t.gesture.imageWrap[0], "x") || 0, t.image.startY = u.getTranslate(t.gesture.imageWrap[0], "y") || 0, t.gesture.slideWidth = t.gesture.slide[0].offsetWidth, t.gesture.slideHeight = t.gesture.slide[0].offsetHeight, t.gesture.imageWrap.transition(0), u.rtl && (t.image.startX = -t.image.startX), u.rtl && (t.image.startY = -t.image.startY)), i = t.image.width * t.scale, r = t.image.height * t.scale, !(i < t.gesture.slideWidth && r < t.gesture.slideHeight))) {
                            if ((t.image.minX = Math.min(t.gesture.slideWidth / 2 - i / 2, 0), t.image.maxX = -t.image.minX, t.image.minY = Math.min(t.gesture.slideHeight / 2 - r / 2, 0), t.image.maxY = -t.image.minY, t.image.touchesCurrent.x = "touchmove" === n.type ? n.targetTouches[0].pageX : n.pageX, t.image.touchesCurrent.y = "touchmove" === n.type ? n.targetTouches[0].pageY : n.pageY, !t.image.isMoved && !t.isScaling) && (u.isHorizontal() && Math.floor(t.image.minX) === Math.floor(t.image.startX) && t.image.touchesCurrent.x < t.image.touchesStart.x || Math.floor(t.image.maxX) === Math.floor(t.image.startX) && t.image.touchesCurrent.x > t.image.touchesStart.x || !u.isHorizontal() && Math.floor(t.image.minY) === Math.floor(t.image.startY) && t.image.touchesCurrent.y < t.image.touchesStart.y || Math.floor(t.image.maxY) === Math.floor(t.image.startY) && t.image.touchesCurrent.y > t.image.touchesStart.y)) return void (t.image.isTouched = !1);
                            n.preventDefault();
                            n.stopPropagation();
                            t.image.isMoved = !0;
                            t.image.currentX = t.image.touchesCurrent.x - t.image.touchesStart.x + t.image.startX;
                            t.image.currentY = t.image.touchesCurrent.y - t.image.touchesStart.y + t.image.startY;
                            t.image.currentX < t.image.minX && (t.image.currentX = t.image.minX + 1 - Math.pow(t.image.minX - t.image.currentX + 1, .8));
                            t.image.currentX > t.image.maxX && (t.image.currentX = t.image.maxX - 1 + Math.pow(t.image.currentX - t.image.maxX + 1, .8));
                            t.image.currentY < t.image.minY && (t.image.currentY = t.image.minY + 1 - Math.pow(t.image.minY - t.image.currentY + 1, .8));
                            t.image.currentY > t.image.maxY && (t.image.currentY = t.image.maxY - 1 + Math.pow(t.image.currentY - t.image.maxY + 1, .8));
                            t.velocity.prevPositionX || (t.velocity.prevPositionX = t.image.touchesCurrent.x);
                            t.velocity.prevPositionY || (t.velocity.prevPositionY = t.image.touchesCurrent.y);
                            t.velocity.prevTime || (t.velocity.prevTime = Date.now());
                            t.velocity.x = (t.image.touchesCurrent.x - t.velocity.prevPositionX) / (Date.now() - t.velocity.prevTime) / 2;
                            t.velocity.y = (t.image.touchesCurrent.y - t.velocity.prevPositionY) / (Date.now() - t.velocity.prevTime) / 2;
                            Math.abs(t.image.touchesCurrent.x - t.velocity.prevPositionX) < 2 && (t.velocity.x = 0);
                            Math.abs(t.image.touchesCurrent.y - t.velocity.prevPositionY) < 2 && (t.velocity.y = 0);
                            t.velocity.prevPositionX = t.image.touchesCurrent.x;
                            t.velocity.prevPositionY = t.image.touchesCurrent.y;
                            t.velocity.prevTime = Date.now();
                            t.gesture.imageWrap.transform("translate3d(" + t.image.currentX + "px, " + t.image.currentY + "px,0)")
                        }
                    },
                    onTouchEnd: function (n) {
                        var t = n.zoom,
                            e, o, s;
                        if (t.gesture.image && 0 !== t.gesture.image.length) {
                            if (!t.image.isTouched || !t.image.isMoved) return t.image.isTouched = !1, void (t.image.isMoved = !1);
                            t.image.isTouched = !1;
                            t.image.isMoved = !1;
                            var i = 300,
                                r = 300,
                                h = t.velocity.x * i,
                                u = t.image.currentX + h,
                                c = t.velocity.y * r,
                                f = t.image.currentY + c;
                            0 !== t.velocity.x && (i = Math.abs((u - t.image.currentX) / t.velocity.x));
                            0 !== t.velocity.y && (r = Math.abs((f - t.image.currentY) / t.velocity.y));
                            e = Math.max(i, r);
                            t.image.currentX = u;
                            t.image.currentY = f;
                            o = t.image.width * t.scale;
                            s = t.image.height * t.scale;
                            t.image.minX = Math.min(t.gesture.slideWidth / 2 - o / 2, 0);
                            t.image.maxX = -t.image.minX;
                            t.image.minY = Math.min(t.gesture.slideHeight / 2 - s / 2, 0);
                            t.image.maxY = -t.image.minY;
                            t.image.currentX = Math.max(Math.min(t.image.currentX, t.image.maxX), t.image.minX);
                            t.image.currentY = Math.max(Math.min(t.image.currentY, t.image.maxY), t.image.minY);
                            t.gesture.imageWrap.transition(e).transform("translate3d(" + t.image.currentX + "px, " + t.image.currentY + "px,0)")
                        }
                    },
                    onTransitionEnd: function (n) {
                        var t = n.zoom;
                        t.gesture.slide && n.previousIndex !== n.activeIndex && (t.gesture.image.transform("translate3d(0,0,0) scale(1)"), t.gesture.imageWrap.transform("translate3d(0,0,0)"), t.gesture.slide = t.gesture.image = t.gesture.imageWrap = void 0, t.scale = t.currentScale = 1)
                    },
                    toggleZoom: function (t, i) {
                        var r = t.zoom,
                            s, h, y, p, w, b, u, f, k, d, g, nt, e, o, c, l, a, v;
                        (r.gesture.slide || (r.gesture.slide = t.clickedSlide ? n(t.clickedSlide) : t.slides.eq(t.activeIndex), r.gesture.image = r.gesture.slide.find("img, svg, canvas"), r.gesture.imageWrap = r.gesture.image.parent("." + t.params.zoomContainerClass)), r.gesture.image && 0 !== r.gesture.image.length) && ("undefined" == typeof r.image.touchesStart.x && i ? (s = "touchend" === i.type ? i.changedTouches[0].pageX : i.pageX, h = "touchend" === i.type ? i.changedTouches[0].pageY : i.pageY) : (s = r.image.touchesStart.x, h = r.image.touchesStart.y), r.scale && 1 !== r.scale ? (r.scale = r.currentScale = 1, r.gesture.imageWrap.transition(300).transform("translate3d(0,0,0)"), r.gesture.image.transition(300).transform("translate3d(0,0,0) scale(1)"), r.gesture.slide = void 0) : (r.scale = r.currentScale = r.gesture.imageWrap.attr("data-swiper-zoom") || t.params.zoomMax, i ? (a = r.gesture.slide[0].offsetWidth, v = r.gesture.slide[0].offsetHeight, y = r.gesture.slide.offset().left, p = r.gesture.slide.offset().top, w = y + a / 2 - s, b = p + v / 2 - h, k = r.gesture.image[0].offsetWidth, d = r.gesture.image[0].offsetHeight, g = k * r.scale, nt = d * r.scale, e = Math.min(a / 2 - g / 2, 0), o = Math.min(v / 2 - nt / 2, 0), c = -e, l = -o, u = w * r.scale, f = b * r.scale, u < e && (u = e), u > c && (u = c), f < o && (f = o), f > l && (f = l)) : (u = 0, f = 0), r.gesture.imageWrap.transition(300).transform("translate3d(" + u + "px, " + f + "px,0)"), r.gesture.image.transition(300).transform("translate3d(0,0,0) scale(" + r.scale + ")")))
                    },
                    attachEvents: function (t) {
                        var i = t ? "off" : "on",
                            r;
                        u.params.zoom && (r = (u.slides, !("touchstart" !== u.touchEvents.start || !u.support.passiveListener || !u.params.passiveListeners) && {
                            passive: !0,
                            capture: !1
                        }), u.support.gestures ? (u.slides[i]("gesturestart", u.zoom.onGestureStart, r), u.slides[i]("gesturechange", u.zoom.onGestureChange, r), u.slides[i]("gestureend", u.zoom.onGestureEnd, r)) : "touchstart" === u.touchEvents.start && (u.slides[i](u.touchEvents.start, u.zoom.onGestureStart, r), u.slides[i](u.touchEvents.move, u.zoom.onGestureChange, r), u.slides[i](u.touchEvents.end, u.zoom.onGestureEnd, r)), u[i]("touchStart", u.zoom.onTouchStart), u.slides.each(function (t, r) {
                            n(r).find("." + u.params.zoomContainerClass).length > 0 && n(r)[i](u.touchEvents.move, u.zoom.onTouchMove)
                        }), u[i]("touchEnd", u.zoom.onTouchEnd), u[i]("transitionEnd", u.zoom.onTransitionEnd), u.params.zoomToggle && u.on("doubleTap", u.zoom.toggleZoom))
                    },
                    init: function () {
                        u.zoom.attachEvents()
                    },
                    destroy: function () {
                        u.zoom.attachEvents(!0)
                    }
                };
                u._plugins = [];
                for (lt in u.plugins) at = u.plugins[lt](u, u.params[lt]), at && u._plugins.push(at);
                return u.callPlugins = function (n) {
                    for (var t = 0; t < u._plugins.length; t++) n in u._plugins[t] && u._plugins[t][n](arguments[1], arguments[2], arguments[3], arguments[4], arguments[5])
                }, u.emitterEventListeners = {}, u.emit = function (n) {
                    u.params[n] && u.params[n](arguments[1], arguments[2], arguments[3], arguments[4], arguments[5]);
                    var t;
                    if (u.emitterEventListeners[n])
                        for (t = 0; t < u.emitterEventListeners[n].length; t++) u.emitterEventListeners[n][t](arguments[1], arguments[2], arguments[3], arguments[4], arguments[5]);
                    u.callPlugins && u.callPlugins(n, arguments[1], arguments[2], arguments[3], arguments[4], arguments[5])
                }, u.on = function (n, t) {
                    return n = et(n), u.emitterEventListeners[n] || (u.emitterEventListeners[n] = []), u.emitterEventListeners[n].push(t), u
                }, u.off = function (n, t) {
                    var i;
                    if (n = et(n), "undefined" == typeof t) return u.emitterEventListeners[n] = [], u;
                    if (u.emitterEventListeners[n] && 0 !== u.emitterEventListeners[n].length) {
                        for (i = 0; i < u.emitterEventListeners[n].length; i++) u.emitterEventListeners[n][i] === t && u.emitterEventListeners[n].splice(i, 1);
                        return u
                    }
                }, u.once = function (n, t) {
                    n = et(n);
                    var i = function () {
                        t(arguments[0], arguments[1], arguments[2], arguments[3], arguments[4]);
                        u.off(n, i)
                    };
                    return u.on(n, i), u
                }, u.a11y = {
                    makeFocusable: function (n) {
                        return n.attr("tabIndex", "0"), n
                    },
                    addRole: function (n, t) {
                        return n.attr("role", t), n
                    },
                    addLabel: function (n, t) {
                        return n.attr("aria-label", t), n
                    },
                    disable: function (n) {
                        return n.attr("aria-disabled", !0), n
                    },
                    enable: function (n) {
                        return n.attr("aria-disabled", !1), n
                    },
                    onEnterKey: function (t) {
                        13 === t.keyCode && (n(t.target).is(u.params.nextButton) ? (u.onClickNext(t), u.isEnd ? u.a11y.notify(u.params.lastSlideMessage) : u.a11y.notify(u.params.nextSlideMessage)) : n(t.target).is(u.params.prevButton) && (u.onClickPrev(t), u.isBeginning ? u.a11y.notify(u.params.firstSlideMessage) : u.a11y.notify(u.params.prevSlideMessage)), n(t.target).is("." + u.params.bulletClass) && n(t.target)[0].click())
                    },
                    liveRegion: n('<span class="' + u.params.notificationClass + '" aria-live="assertive" aria-atomic="true"><\/span>'),
                    notify: function (n) {
                        var t = u.a11y.liveRegion;
                        0 !== t.length && (t.html(""), t.html(n))
                    },
                    init: function () {
                        u.params.nextButton && u.nextButton && u.nextButton.length > 0 && (u.a11y.makeFocusable(u.nextButton), u.a11y.addRole(u.nextButton, "button"), u.a11y.addLabel(u.nextButton, u.params.nextSlideMessage));
                        u.params.prevButton && u.prevButton && u.prevButton.length > 0 && (u.a11y.makeFocusable(u.prevButton), u.a11y.addRole(u.prevButton, "button"), u.a11y.addLabel(u.prevButton, u.params.prevSlideMessage));
                        n(u.container).append(u.a11y.liveRegion)
                    },
                    initPagination: function () {
                        u.params.pagination && u.params.paginationClickable && u.bullets && u.bullets.length && u.bullets.each(function () {
                            var t = n(this);
                            u.a11y.makeFocusable(t);
                            u.a11y.addRole(t, "button");
                            u.a11y.addLabel(t, u.params.paginationBulletMessage.replace(/{{index}}/, t.index() + 1))
                        })
                    },
                    destroy: function () {
                        u.a11y.liveRegion && u.a11y.liveRegion.length > 0 && u.a11y.liveRegion.remove()
                    }
                }, u.init = function () {
                    u.params.loop && u.createLoop();
                    u.updateContainerSize();
                    u.updateSlidesSize();
                    u.updatePagination();
                    u.params.scrollbar && u.scrollbar && (u.scrollbar.set(), u.params.scrollbarDraggable && u.scrollbar.enableDraggable());
                    "slide" !== u.params.effect && u.effects[u.params.effect] && (u.params.loop || u.updateProgress(), u.effects[u.params.effect].setTranslate());
                    u.params.loop ? u.slideTo(u.params.initialSlide + u.loopedSlides, 0, u.params.runCallbacksOnInit) : (u.slideTo(u.params.initialSlide, 0, u.params.runCallbacksOnInit), 0 === u.params.initialSlide && (u.parallax && u.params.parallax && u.parallax.setTranslate(), u.lazy && u.params.lazyLoading && (u.lazy.load(), u.lazy.initialImageLoaded = !0)));
                    u.attachEvents();
                    u.params.observer && u.support.observer && u.initObservers();
                    u.params.preloadImages && !u.params.lazyLoading && u.preloadImages();
                    u.params.zoom && u.zoom && u.zoom.init();
                    u.params.autoplay && u.startAutoplay();
                    u.params.keyboardControl && u.enableKeyboardControl && u.enableKeyboardControl();
                    u.params.mousewheelControl && u.enableMousewheelControl && u.enableMousewheelControl();
                    u.params.hashnavReplaceState && (u.params.replaceState = u.params.hashnavReplaceState);
                    u.params.history && u.history && u.history.init();
                    u.params.hashnav && u.hashnav && u.hashnav.init();
                    u.params.a11y && u.a11y && u.a11y.init();
                    u.emit("onInit", u)
                }, u.cleanupStyles = function () {
                    u.container.removeClass(u.classNames.join(" ")).removeAttr("style");
                    u.wrapper.removeAttr("style");
                    u.slides && u.slides.length && u.slides.removeClass([u.params.slideVisibleClass, u.params.slideActiveClass, u.params.slideNextClass, u.params.slidePrevClass].join(" ")).removeAttr("style").removeAttr("data-swiper-column").removeAttr("data-swiper-row");
                    u.paginationContainer && u.paginationContainer.length && u.paginationContainer.removeClass(u.params.paginationHiddenClass);
                    u.bullets && u.bullets.length && u.bullets.removeClass(u.params.bulletActiveClass);
                    u.params.prevButton && n(u.params.prevButton).removeClass(u.params.buttonDisabledClass);
                    u.params.nextButton && n(u.params.nextButton).removeClass(u.params.buttonDisabledClass);
                    u.params.scrollbar && u.scrollbar && (u.scrollbar.track && u.scrollbar.track.length && u.scrollbar.track.removeAttr("style"), u.scrollbar.drag && u.scrollbar.drag.length && u.scrollbar.drag.removeAttr("style"))
                }, u.destroy = function (n, t) {
                    u.detachEvents();
                    u.stopAutoplay();
                    u.params.scrollbar && u.scrollbar && u.params.scrollbarDraggable && u.scrollbar.disableDraggable();
                    u.params.loop && u.destroyLoop();
                    t && u.cleanupStyles();
                    u.disconnectObservers();
                    u.params.zoom && u.zoom && u.zoom.destroy();
                    u.params.keyboardControl && u.disableKeyboardControl && u.disableKeyboardControl();
                    u.params.mousewheelControl && u.disableMousewheelControl && u.disableMousewheelControl();
                    u.params.a11y && u.a11y && u.a11y.destroy();
                    u.params.history && !u.params.replaceState && window.removeEventListener("popstate", u.history.setHistoryPopState);
                    u.params.hashnav && u.hashnav && u.hashnav.destroy();
                    u.emit("onDestroy");
                    n !== !1 && (u = null)
                }, u.init(), u
            }
        },
        u, r, i;
    for (t.prototype = {
        isSafari: function () {
            var n = window.navigator.userAgent.toLowerCase();
            return n.indexOf("safari") >= 0 && n.indexOf("chrome") < 0 && n.indexOf("android") < 0
        }(),
        isUiWebView: /(iPhone|iPod|iPad).*AppleWebKit(?!.*Safari)/i.test(window.navigator.userAgent),
        isArray: function (n) {
            return "[object Array]" === Object.prototype.toString.apply(n)
        },
        browser: {
            ie: window.navigator.pointerEnabled || window.navigator.msPointerEnabled,
            ieTouch: window.navigator.msPointerEnabled && window.navigator.msMaxTouchPoints > 1 || window.navigator.pointerEnabled && window.navigator.maxTouchPoints > 1,
            lteIE9: function () {
                var n = document.createElement("div");
                return n.innerHTML = "<!--[if lte IE 9]><i><\/i><![endif]-->", 1 === n.getElementsByTagName("i").length
            }()
        },
        device: function () {
            var n = window.navigator.userAgent,
                i = n.match(/(Android);?[\s\/]+([\d.]+)?/),
                t = n.match(/(iPad).*OS\s([\d_]+)/),
                r = n.match(/(iPod)(.*OS\s([\d_]+))?/),
                u = !t && n.match(/(iPhone\sOS|iOS)\s([\d_]+)/);
            return {
                ios: t || u || r,
                android: i
            }
        }(),
        support: {
            touch: window.Modernizr && Modernizr.touch === !0 || function () {
                return !!("ontouchstart" in window || window.DocumentTouch && document instanceof DocumentTouch)
            }(),
            transforms3d: window.Modernizr && Modernizr.csstransforms3d === !0 || function () {
                var n = document.createElement("div").style;
                return "webkitPerspective" in n || "MozPerspective" in n || "OPerspective" in n || "MsPerspective" in n || "perspective" in n
            }(),
            flexbox: function () {
                for (var i = document.createElement("div").style, t = "alignItems webkitAlignItems webkitBoxAlign msFlexAlign mozBoxAlign webkitFlexDirection msFlexDirection mozBoxDirection mozBoxOrient webkitBoxDirection webkitBoxOrient".split(" "), n = 0; n < t.length; n++)
                    if (t[n] in i) return !0
            }(),
            observer: function () {
                return "MutationObserver" in window || "WebkitMutationObserver" in window
            }(),
            passiveListener: function () {
                var n = !1,
                    t;
                try {
                    t = Object.defineProperty({}, "passive", {
                        get: function () {
                            n = !0
                        }
                    });
                    window.addEventListener("testPassiveListener", null, t)
                } catch (n) {
                }
                return n
            }(),
            gestures: function () {
                return "ongesturestart" in window
            }()
        },
        plugins: {}
    }, u = ["jQuery", "Zepto", "Dom7"], r = 0; r < u.length; r++) window[u[r]] && f(window[u[r]]);
    i = "undefined" == typeof Dom7 ? window.Dom7 || window.Zepto || window.jQuery : Dom7;
    i && ("transitionEnd" in i.fn || (i.fn.transitionEnd = function (n) {
        function r(f) {
            if (f.target === this)
                for (n.call(this, f), t = 0; t < i.length; t++) u.off(i[t], r)
        }

        var t, i = ["webkitTransitionEnd", "transitionend", "oTransitionEnd", "MSTransitionEnd", "msTransitionEnd"],
            u = this;
        if (n)
            for (t = 0; t < i.length; t++) u.on(i[t], r);
        return this
    }), "transform" in i.fn || (i.fn.transform = function (n) {
        for (var t, i = 0; i < this.length; i++) t = this[i].style, t.webkitTransform = t.MsTransform = t.msTransform = t.MozTransform = t.OTransform = t.transform = n;
        return this
    }), "transition" in i.fn || (i.fn.transition = function (n) {
        var i, t;
        for ("string" != typeof n && (n += "ms"), i = 0; i < this.length; i++) t = this[i].style, t.webkitTransitionDuration = t.MsTransitionDuration = t.msTransitionDuration = t.MozTransitionDuration = t.OTransitionDuration = t.transitionDuration = n;
        return this
    }), "outerWidth" in i.fn || (i.fn.outerWidth = function (n) {
        return this.length > 0 ? n ? this[0].offsetWidth + parseFloat(this.css("margin-right")) + parseFloat(this.css("margin-left")) : this[0].offsetWidth : null
    }));
    window.Swiper = t
}();
"undefined" != typeof module ? module.exports = window.Swiper : "function" == typeof define && define.amd && define([], function () {
    "use strict";
    return window.Swiper
});
