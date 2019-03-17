
function SubmitParameterTextChanged(creatorGuid, parameterGuid, newValue) {
    var postData = {};
    postData["CreatorGuid"] = creatorGuid;
    postData["ParameterGuid"] = parameterGuid;
    postData["NewValue"] = newValue;

    $.ajax({
        url: "Action/ParameterTextChanged",
        type: 'post',
        data: JSON.stringify(postData),
        contentType: "application/json",
        success: function (data) {

            $(".compile-error").removeAttr("title").removeClass("compile-error");

            if (data.listValues) {

                $.each(data.listValues, function (i, entry) {
                    var guid = entry.guid;
                    var message = entry.value;

                    //.tooltip()
                    $("tr[data-id='" + guid + "']").addClass("compile-error").attr("title", message)
                        .parents(".codecreator").first().addClass("compile-error");
                });

            }

            if (data.isSuccess) {
                return;
            }
            alert(data.message);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert(errorThrown);
        }
    });
}

function SubmitUserDisplayNameChanged(creatorGuid, newValue) {
    var postData = {};
    postData["CreatorGuid"] = creatorGuid;
    postData["NewValue"] = newValue;

    $.ajax({
        url: "Action/UserDisplayNameChanged",
        type: 'post',
        data: JSON.stringify(postData),
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
}


function SubmitWorkflowReferencedAssemblyChanged(pAddValue, pValue) {
    var postData = {};
    postData["AddValue"] = pAddValue;
    postData["Value"] = pValue;

    $.ajax({
        url: "Action/WorkflowReferencedAssemblyChanged",
        type: 'post',
        data: JSON.stringify(postData),
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
}

function SubmitWorkflowReferencedNamespaceChanged(pAddValue, pValue) {
    var postData = {};
    postData["AddValue"] = pAddValue;
    postData["Value"] = pValue;

    $.ajax({
        url: "Action/WorkflowReferencedNamespaceChanged",
        type: 'post',
        data: JSON.stringify(postData),
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
}

function SubmitMoveAfter(sourceId, destinationAfterId, destinationContainerId) {
    var postData = {};
    postData["SourceId"] = sourceId;
    postData["DestinationAfterId"] = destinationAfterId;
    postData["DestinationContainerId"] = destinationContainerId;

    $.ajax({
        url: "Action/CodeCreatorMoved",
        type: 'post',
        data: JSON.stringify(postData),
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
}

function SubmitCreateCodeCreator(newCodeCreator, destinationAfterId, destinationContainerId, type) {
    var postData = {};

    postData["DestinationAfterId"] = destinationAfterId;
    postData["DestinationContainerId"] = destinationContainerId;
    postData["Type"] = type;

    $.ajax({
        url: "Action/CodeCreatorCreated",
        type: 'post',
        data: JSON.stringify(postData),
        contentType: "application/json",
        success: function (data) {
            if (data.isSuccess) {

                newCodeCreator.attr("data-id", data.message);
                newCodeCreator.find("table").attr('data-creator-id', data.message);
                newCodeCreator.addClass("ui-draggable ui-draggable-handle");

                $.each(newCodeCreator.find("table").children().children().filter(":gt(0)"), function (i, entry) {
                    var guid = data.listValues[$(entry).data("index")].guid;
                    $(entry).attr("data-id", guid);
                    $(entry).children().last().children().first().attr("data-id", guid);
                });

                return;
            }
            alert(data.message);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert(errorThrown);
        }
    });
}

function SubmitDeleteCodeCreator(id) {
    var postData = {};
    postData["Id"] = id;

    $.ajax({
        url: "Action/CodeCreatorDeleted",
        type: 'post',
        data: JSON.stringify(postData),
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
}

function SubmitRunWorkflow() {

    var postData = {};

    $.ajax({
        url: "Action/RunWorkflow",
        type: 'post',
        data: JSON.stringify(postData),
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
}


