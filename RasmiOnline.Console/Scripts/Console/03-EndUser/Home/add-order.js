/// <reference path="../../../jquery-1.10.2.min.js" />
var assets = [];
paceOptions = {
    ajax: {
        trackMethods: ['GET', 'POST', 'DELETE', 'PUT', 'PATCH']
    }
};
$(document).on('ready', function () {

    $('#add-order-page').on('click', '#btn-submit', function () {
        let $btn = $(this);
        let $frm = $btn.closest('form');
        if (!$frm.valid()) return;
        if (assets.length === 0) {
            notify(false, 'لطفا حداقل یک فایل جهت آپلود انتخاب نمایید');
            return;
        }
        let data = new FormData();
        let model = customSerialize($frm);
        console.log(model);
        let keys = Object.keys(model);
        for (var i = 0; i < keys.length; i++)
            data.append(keys[i], model[keys[i]]);
        for (var i = 0; i < assets.length; i++) 
            data.append('attachments', assets[i]);
        ajaxBtn.inProgress($btn);
        $.ajax({
            type: 'POST',
            url: $frm.attr('action'),
            data: data,
            contentType: false,
            processData:false,
            success: function (rep) {
                ajaxBtn.normal();
            },
            error: function (e) {
                ajaxBtn.normal();
            }
        })
    });

});


