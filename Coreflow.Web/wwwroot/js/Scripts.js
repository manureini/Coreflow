


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

});