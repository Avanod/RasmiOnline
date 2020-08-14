/// <reference path="../../jquery-1.10.2.min.js" />
const maxImageSize = 5;//bytes
const extensions = ['png','jpeg','pdf','jpg'];
var $fileBox = '<div class="uploader-item uploaded new m-b">' +
    '<button class="btn-remove"><i class="fa fa-times"></i></button>' +
    '<img src="{0}"/>' +
    '</div>';
var assets = [];
$(document).ready(function () {
    $('#input-file').prop('accept', extensions.map(function (ext) { return '.' + ext; }).toString());

    $(document).on('click', '.btn-remove', function (e) {
        e.stopPropagation();
        let $btn = $(this);
        let $box = $btn.closest('.uploaded');

        let removeUrl = $btn.data('url');
        console.log(removeUrl);
        if (removeUrl) {
            ajaxBtn.inProgress($btn);
            $.post(removeUrl)
                .done(function (data) {
                    console.log(data);
                    ajaxBtn.normal();
                    if (data.IsSuccessful) {
                        $box.remove();
                    }
                    else showNotif(notifyType.danger, data.Message);
                })
                .fail(function (e) {
                    ajaxBtn.normal();
                });
        }
        else {
            let idx = $('#modal .uploaded.new').index($box);
            $box.remove();
            assets.splice(idx, 1);
        }
    });
    $(document).on('click', '#uploader', function (e) {
        e.stopPropagation();
        $('#input-file').trigger('click');
    });
    $(document).on('change', '#input-file', function (event) {
        event.stopPropagation();
        var $i = $(this);
        var file = this.files[0];
        selectFile(file);
        //$('').trigger('click');
    });

    $('#uploader').on({
        'dragover dragenter': function (e) {
            e.preventDefault();
            e.stopPropagation();
        },
        'drop': function (e) {
            //console.log(e.originalEvent instanceof DragEvent);
            var dataTransfer = e.originalEvent.dataTransfer;
            if (dataTransfer && dataTransfer.files.length) {
                e.preventDefault();
                e.stopPropagation();
                $.each(dataTransfer.files, function (i, file) {
                    selectFile(file);
                    //var reader = new FileReader();
                    //reader.onload = $.proxy(function (file, $fileList, event) {
                    //    var img = file.type.match('image.*') ? "<img src='" + event.target.result + "' /> " : "";
                    //    $fileList.prepend($("<li>").append(img + file.name));
                    //}, this, file, $("#fileList"));
                    //reader.readAsDataURL(file);
                });
            }
        }
    });


});

function selectFile(file) {
    if (file.size > maxImageSize * 1024 * 1024) {
        notify(false, 'حداکثر حجم هر تصویر ' + maxImageSize + ' مگا بایت می باشد');
        return;
    }
    let ext = file.name.split('.').reverse()[0];
    if (extensions.indexOf(ext) === -1) {
        notify(false, 'فرمت مورد قبول ' + extensions.toString() + 'می باشد ');
        return;
    }
    var reader = new FileReader();
    reader.onload = function (e) {
        var fileType = getFileType(file.name);
        var url = '';
        if (fileType === fileTypes.Image) url = e.target.result;
        else url = getDefaultImageUrl(file.name);

        $('#files').append($fileBox.replace('{0}', url));
        $('#input-file').val('');
        assets.push(file);
    };
    reader.readAsDataURL(file);
}