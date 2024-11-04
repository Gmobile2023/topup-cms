$(document).ready(function () {
    $(".se-pre-con").fadeOut("slow", function () {
    });
});
$(window).on('load', function () {
    //$(".se-pre-con").fadeOut("slow", function() {});
    $(".bigCheck").fadeIn("slow");
});
$(document).ready(function () {
    // menu
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

    $(".active.sub-menu > a").css("color", "#00baf3");
    //$(".active.sub-menu > a").css("font-size", "16px");
    $(".active.sub-menu > a").css("font-size", "inherit");
    $(".active.sub-menu").click(function () {
        $(".active.sub-menu a:not('.dropdown-sub--hover__a')").removeAttr('data-toggle');
        $(this).toggleClass('active');
    });
    $('.sub-menu').on('show.bs.dropdown', function () {
        $(this).find('.dropdown-menu').first().stop(true, true).slideDown(150);
        $(this).toggleClass('open');
    });
    $('.sub-menu').on('hide.bs.dropdown', function () {
        $(this).find('.dropdown-menu').first().stop(true, true).slideUp(150);
        $(this).toggleClass('open');
    });
    $('nav ul.dropdown-menu').on('click', function (e) {
        e.stopPropagation();
    });

    $(".select2").on('select2:open', selectOpen);
    $(".select2").on('select2:closing', selectClosing);

});

//
function selectOpen(e) {
    //window.longld2 = $(this);
    var id = $(this).attr('id');
    if ($('.select2-container--default .select2-dropdown').innerWidth() < 708) {
        $('.select2-container--default .select2-dropdown').addClass('top0');
        $('.select2-container--default .select2-dropdown').prepend('<div class="close-select"><div class="close-select__btn" onclick="closeSelect(\'' + id + '\')"></div> Vui lòng chọn</div>');
    }
}

function closeSelect(id) {
    $("#" + id).select2('close');
}

function selectClosing(e) {
    //console.log(e, this);
    $('.select2-container--default .select2-dropdown').removeClass('top0');
    $('.select2-container--default .select2-dropdown .close-select').remove();
    $('body').css('position', 'relative');
}
