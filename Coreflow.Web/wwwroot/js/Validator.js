
$(function () {
    $(".corrector-action").click(function () {
        var postData = {};
        postData["FlowIdentifier"] = currentFlowIdentifier;
        postData["Value"] = JSON.stringify($(this).data("action"));

        $.ajax({
            url: "Action/DoValidatorAction",
            type: 'post',
            data: JSON.stringify(postData),
            contentType: "application/json",
            success: function (data) {
                if (data.isSuccess) {
                    location.reload();
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