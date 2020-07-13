/// <reference path="../../../jquery-1.10.2.min.js" />

$(document).on('ready', function () {
    //init addreses plugin
    var o = $('.flickity-enabled').flickity({
        rightToLeft: true,
        contain: true,
        pageDots: false
    });

    $('#order-payment').on('click', '#btn-submit', function () {
        let $btn = $(this);
        let $frm = $btn.closest('form');
        if (!$frm.valid()) return;
        if (assets.length === 0) {
            notify(false, 'لطفا حداقل یک فایل جهت آپلود انتخاب نمایید');
            return;
        }
        let data = new FormData();
        let model = customSerialize($frm);

        ajaxBtn.inProgress($btn);

        //var xhr = new XMLHttpRequest();
        //xhr.upload.onprogress = function (e) {
        //    $('#progress').text('%' + ((e.loaded / e.total) * 100).toString());
        //};
        //xhr.onloadend = function () {
        //    $('#progress').text('');
        //};
        //xhr.open('POST', $frm.attr('action'));
        //xhr.responseType = 'json';
        //xhr.send(data);
        //xhr.onload = function () {
        //        ajaxBtn.normal();
        //    if (xhr.status === 200) {
        //        notify(true, 'ثبت سفارش با موفقیت انجام شد');
        //        setTimeout(function () { window.location.reload(); }, 3000);
        //    }
        //    else notify(false, 'خطایی رخ داده است، لطفا دوباره تلاش نمایید');
            
        //};

        $.ajax({
            type: 'POST',
            url: $frm.attr('action'),
            data: data,
            contentType: false,
            processData: false,
            success: function (rep) {
                console.log(rep);
                if (rep.IsSuccessful) {
                    ajaxBtn.normal();
                    notify(true, 'ثبت سفارش با موفقیت انجام شد');
                    setTimeout(function () { window.location.reload(); }, 3000);
                }
                else {
                    notify(false, rep.Message);
                }

            },
            error: function (e) {
                ajaxBtn.normal();
                notify(false, 'خطایی رخ داده است، لطفا دوباره تلاش نمایید');
            }
        });
    });

});


