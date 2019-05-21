
$(function () {
    $(".corrector-action").click(function () {

        var json = $(this).data("action");

        $.ajax({
            url: "Action/DoValidatorAction",
            type: 'post',
            data: JSON.stringify(json),
            contentType: "application/json",
            success: function (data) {
                if (data.isSuccess) {
                    return;
                }
                alert(data.message);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert(errorThrown);
            }
        });
    });
})