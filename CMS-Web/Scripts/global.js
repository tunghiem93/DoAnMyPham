/// <reference path="jquery-2.0.0.intellisense.js" />
/// <reference path="jquery-2.0.0.js" />
$(document).ready(function () {
    //$('.mainnav .dropdown').mouseover(function () {
    //    $(this).find('.subnav').stop().slideDown();
    //})
    //$('.mainnav .dropdown').mouseout(function () {
    //    $(this).find('.subnav').slideUp();
    //})

    $('.mainnav .dropdown').hover(function () {
        $(this).find('.subnav').stop(true, true).slideDown("fast");
    }, // <-- this comma is important. 
//the anonymous function after this will run on mouse out
    function () {
        $(this).find('.subnav').stop(true, true).slideUp("fast");
        });

    $('.mainnav .dropdown2').hover(function () {
        $(this).find('.subnav2').stop(true, true).slideDown("fast");
    }, // <-- this comma is important. 
        //the anonymous function after this will run on mouse out
        function () {
            $(this).find('.subnav2').stop(true, true).slideUp("fast");
        });
    $('.mainnav .dropdown2').hover(function () {
        $(this).find('.subnav3').stop(true, true).slideDown("fast");
    }, // <-- this comma is important. 
        //the anonymous function after this will run on mouse out
        function () {
            $(this).find('.subnav3').stop(true, true).slideUp("fast");
        });
    $('.mainnav .dropdown2').hover(function () {
        $(this).find('.subnav4').stop(true, true).slideDown("fast");
    }, // <-- this comma is important. 
        //the anonymous function after this will run on mouse out
        function () {
            $(this).find('.subnav4').stop(true, true).slideUp("fast");
        });
    $('.mainnav .dropdown2').hover(function () {
        $(this).find('.subnav5').stop(true, true).slideDown("fast");
    }, // <-- this comma is important. 
        //the anonymous function after this will run on mouse out
        function () {
            $(this).find('.subnav5').stop(true, true).slideUp("fast");
        });
    $('.mainnav .dropdown2').hover(function () {
        $(this).find('.subnav6').stop(true, true).slideDown("fast");
    }, // <-- this comma is important. 
        //the anonymous function after this will run on mouse out
        function () {
            $(this).find('.subnav6').stop(true, true).slideUp("fast");
        });
    $('.mainnav .dropdown2').hover(function () {
        $(this).find('.subnav7').stop(true, true).slideDown("fast");
    }, // <-- this comma is important. 
        //the anonymous function after this will run on mouse out
        function () {
            $(this).find('.subnav7').stop(true, true).slideUp("fast");
        });
    $('.btnmenu').click(function () {
        $(this).toggleClass('mbopen');
        $("html, body").animate({ scrollTop: 0 }, "slow");
        var top = parseInt($('footer').offset().top);
        $('.mobilemenu').css("height", top);
        $('.mobilemenu').toggleClass('menuopen');

    })
    $('.closesearch').click(function () {
        $('.right-nav').removeClass('open');
    })

    $(".gototop").click(function () {
        $("html, body").animate({ scrollTop: 0 }, "slow");
        return false;
    });
})

$(window).scroll(function () {
    var hrpos = $('hr').position().top;
    var totop = $(document).scrollTop();
    if (totop >= hrpos) {
        $('.mainnav').addClass('fixed-top');
        //$('.fixheader').addClass('bb');
        //$('.fixheader').addClass('whitebg');

        $('.gototop').fadeIn();
    }
    else {
        $('.gototop').fadeOut();
        $('.mainnav').removeClass('fixed-top');
        //$('.fixheader').removeClass('bb');
        //$('.fixheader').removeClass('whitebg');

    }

})
