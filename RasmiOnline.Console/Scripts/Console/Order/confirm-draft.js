/// <reference path="../jquery-1.10.2.min.js" />
$(document).on('ready', function () {
    $(document).on('change', '#IsConfirmedByClient', function () {
        console.log($(this).prop('checked'));
        $(this).val(($(this).prop('checked')).toString());
    });

    $(document).on('click', '#btn-confirm-draft', function () {
        let $btn = $(this);
        submitAjaxForm($btn, function (rep) {
            if (rep.IsSuccessful) {
                $('.confirm-draft').html('<div style="width:100%;" clas="w-100 p-4"><p class="text-center alert alert-success p-4">' + 'عملیات با موفقیت انجام شد' + '</p></div>');
            }
            else {
                notify(false, rep.Message);
            }
        });
    });

});