/// <reference path="jquery-2.0.0.intellisense.js" />
/// <reference path="jquery-2.0.0.js" />

$(document).ready(function () {
    $('.mainnav ul.nav > li:nth-child(2)').addClass('active');
    new WOW().init();
    //var $easyzoom = $('.easyzoom').easyZoom();
    //$('.addtocart').click(function (e) {
    //    var qty = 1;
    //    if ($('#slColor').length > 0) {
    //        var colorid = $('#slColor').val()
    //        if (colorid < 1)
    //        {
    //            alert("Vui lòng chọn màu sắc của sản phẩm !");
    //            return false;
    //        }
    //    }
    //    $.ajax({
    //        url: "/aj/Order/AddToCart",
    //        data: { id: productid, qty: qty, colorid: colorid },
    //        beforeSend: function () { showLoading(); },
    //        success: function (e) {
    //            if (e._result>0) {
    //                location.href = "/gio-hang";
    //            }
    //            else {
    //                hideLoading();
    //                alert(e._error);
    //            }
    //        }
    //    })
    //})


    var productdetailthumbimageslider = new Swiper('.product-detail-gallerythumb-slider', {
        spaceBetween: 10,
        slidesPerView: 4,
        freeMode: true,
        watchSlidesVisibility: true,
        watchSlidesProgress: true,
    });
    var productdetailimageslider = new Swiper('.product-detail-gallery-slider', {
        spaceBetween: 10,
        zoom: true,
        autoHeight: true,
        thumbs: {
            swiper: productdetailthumbimageslider,
        },
    });
   

})
