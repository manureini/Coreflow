



$(function () {

    $("#btnUploadFlow").click(function () {
        $("#inpUploadFlow").click();
    });


    $("#inpUploadFlow").change(function () {
        var file = this.files[0];

        var data = new FormData();
        data.append("file", file);
        data.append("name", file.name);
        data.append("size", file.size);
        data.append("type", file.type);

        $.ajax({
            type: "POST",
            url: 'Upload',
            contentType: false,
            processData: false,
            data: data,
            beforeSend: function (xhr) {
                //          xhr.setRequestHeader('X-CSRF-TOKEN', Antiforgery_Token);
            },
            success: function (event) {
                location.reload();
            },
            error: function (event) {
                alert("Error");
            },
            xhr: function () {
                var xhr = new window.XMLHttpRequest();
                //                xhr.upload.addEventListener("progress", handleProgress, false);
                return xhr;
            },
        });
    });


    $("#btnDownloadBinary").click(function () {
        var postData = {};
        StartDownload("DownloadBinary", postData);

    });

});


function StartDownload(url, data) {

    var form = $('<form></form>').attr('action', url).attr('method', 'post'); //.attr('target', '_blank')

    Object.keys(data).forEach(function (key) {
        var value = data[key];

        if (value instanceof Array) {
            value.forEach(function (v) {
                form.append($("<input></input>").attr('type', 'hidden').attr('name', key).attr('value', v));
            });
        } else {
            form.append($("<input></input>").attr('type', 'hidden').attr('name', key).attr('value', value));
        }

    });

    //send request
    form.appendTo('body').submit().remove();
};


function ShowLoading() {
    clearTimeout($.data(document, 'loadtimer'));
    var wait = setTimeout(() => $("body").append('<div class="loadingoverlay loading"></div>'), 150);
    $(document).data('loadtimer', wait);
}

function HideLoading() {
    clearTimeout($.data(document, 'loadtimer'));
    $(".loadingoverlay").remove();
}
