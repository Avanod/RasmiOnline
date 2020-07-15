/// <reference path="../../../jquery-1.10.2.min.js" />

$(document).on('ready', function () {
    //init addreses plugin
    var o = $('.flickity-enabled').flickity({
        rightToLeft: true,
        contain: true,
        pageDots: false
    });
    fireGoogleMap();
    $('#order-payment').on('click', '#btn-submit', function () {
        let $btn = $(this);
        let $frm = $btn.closest('form');
        if (!$frm.valid()) return;
        //if (assets.length === 0) {
        //    notify(false, 'لطفا حداقل یک فایل جهت آپلود انتخاب نمایید');
        //    return;
        //}
        let model = customSerialize($frm);
        console.log(model);
        //let addresses = $('[name="AddressId"]');
        if ($('[name="AddressId"]:checked').length === 0) {
            notify(false, 'لطفا آدرس را مشخص کنید');
            return;
        }
        ajaxBtn.inProgress($btn);

        $.ajax({
            type: 'POST',
            url: $frm.attr('action'),
            data: model,
            success: function (rep) {
                console.log(rep);
                ajaxBtn.normal();
                if (rep.IsSuccessful) window.location.href = rep.Result;
                else notify(false, rep.Message);
            },
            error: function (e) {
                ajaxBtn.normal();
                notify(false, 'خطایی رخ داده است، لطفا دوباره تلاش نمایید');
            }
        });
    });

});


