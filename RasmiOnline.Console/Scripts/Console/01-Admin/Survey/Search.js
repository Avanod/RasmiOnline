/// <reference path="../../../jquery-1.10.2.min.js" />
var options = [];
$(document).ready(function () {
    $('#modal-default').on('click', '#btn-add-opt', function () {
        let txt = $('#opt_text').val();
        let $btn = $(this);
        let $template = '<tr class="tr-opt"><td>##text##</td><td class="btn-delete-opt"><i class="zmdi zmdi-close font-5x"></i></td><tr>';
        if (txt) {
            options.push({ SurveyOptionId: 0, Text: txt });
            $('#opt_items').append($template.replace('##text##', txt));
            $('#opt_text').val('');
        }
        else {
            notify(false, 'خطایی رخ داده است، دوباره سعی نمایید.');
        }
    });
    $('#modal-default').on('click', '.btn-delete-opt', function () {
        let $elm = $(this);
        let $wrapper = $elm.closest('.btn-delete-opt');
        console.log($elm.data('url'));
        if (!$elm.data('url')) {
            let idx = $('.tr-opt').index($wrapper.closest('tr'));
            $wrapper.closest('tr').remove();
            options.splice(idx, 1);
        }
        else {
            let $wrapperContent = $wrapper.html();
            $wrapper.html($circularLoader);
            $.post($elm.data('url'))
                .done(function (rep) {
                    if (rep.IsSuccessful) $wrapper.closest('tr').remove();
                    else {
                        $wrapper.html($wrapperContent);
                        notify(false, rep.Message);
                    }
                })
                .fail(function () {
                    notify(false, errorMsg);
                });
        }


    });
    $('#modal-default').on('click', '#btn-submit-survey', function () {
        let $btn = $(this);
        let $frm = $btn.closest('form');
        if (!$frm.valid()) return;
        ajaxBtn.inProgress($btn);
        let model = customSerialize($frm);
        model.SurveyOptions = options.map(function (opt) {
            return { SurveyOptionId: opt.SurveyOptionId, Text: opt.Text };
        });
        delete model.opt_text;
        $.ajax({
            type: 'post',
            url: $frm.attr('action'),
            data: JSON.stringify(model),
            contentType: 'application/json; charset=utf-8;',
            success: function (rep) {
                ajaxBtn.normal();
                if (rep.IsSuccessful) {
                    options = [];
                    $('#modal-default').modal('hide');
                    $('button[data-element-name="SubmitSearchForm"]').trigger('click');
                }
                else {
                    notify(false, rep.Message);
                }
            },
            error: function () {
                ajaxBtn.normal();
                notify(false, 'خطایی رخ داده است، دوباره سعی نمایید.');
            }

        });
    });
    $('#modal-default').on('keypress', '#opt_text', function (e) {
        if (e.keyCode === 13) {
            $('#btn-add-opt').trigger('click');
        }
    })
});