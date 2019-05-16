

var postdata_SubmitParameterTextChanged;

function PrepareParameterTextChanged(creatorGuid, parameterGuid, newValue) {
    postdata_SubmitParameterTextChanged = {};
    postdata_SubmitParameterTextChanged["FlowIdentifier"] = currentFlowIdentifier;
    postdata_SubmitParameterTextChanged["CreatorGuid"] = creatorGuid;
    postdata_SubmitParameterTextChanged["ParameterGuid"] = parameterGuid;
    postdata_SubmitParameterTextChanged["NewValue"] = newValue;
}

function SubmitParameterTextChanged() {

    OnFlowChange();

    $.ajax({
        url: "Action/ParameterTextChanged",
        type: 'post',
        data: JSON.stringify(postdata_SubmitParameterTextChanged),
        contentType: "application/json",
        success: function (data) {
            if (data.isSuccess) {
                SubmitCompile();
                return;
            }
            alert(data.message);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert(errorThrown);
        }
    });
}

function SubmitCompile() {
    var postData = {};
    postData["FlowIdentifier"] = currentFlowIdentifier;

    $.ajax({
        url: "Action/CompileFlow",
        type: 'post',
        data: JSON.stringify(postData),
        contentType: "application/json",
        success: function (data) {
            if (data.isSuccess) {

                $(".compile-error").removeAttr("title").removeClass("compile-error");

                if (data.listValues && data.listValues.length > 0) {

                    $("#flow-name").addClass("compile-error");

                    $.each(data.listValues, function (i, entry) {
                        var guid = entry.guid;
                        var message = entry.value;

                        $("tr[data-id='" + guid + "']").addClass("compile-error").attr("title", message)
                            .parents(".codecreator").first().addClass("compile-error");

                        $(".codecreator[data-id='" + guid + "'").addClass("compile-error");
                    });
                }

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

    OnFlowChange();

    var postData = {};
    postData["FlowIdentifier"] = currentFlowIdentifier;
    postData["Id"] = creatorGuid;
    postData["Value"] = newValue;

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

function SubmitFlowReferencedNamespaceChanged(pAddValue, pValue) {

    OnFlowChange();

    var postData = {};
    postData["FlowIdentifier"] = currentFlowIdentifier;
    postData["AddValue"] = pAddValue;
    postData["Value"] = pValue;

    $.ajax({
        url: "Action/FlowReferencedNamespaceChanged",
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

function SubmitFlowArgumentChanged(pAddValue, pName, pType, pValue) {

    OnFlowChange();

    var postData = {};
    postData["FlowIdentifier"] = currentFlowIdentifier;
    postData["AddValue"] = pAddValue;
    postData["Name"] = pName;
    postData["Type"] = pType;
    postData["Value"] = pValue;

    $.ajax({
        url: "Action/FlowArgumentChanged",
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


function SubmitMoveAfter(sourceId, destinationAfterId, destinationContainerId, sequenceIndex) {

    OnFlowChange();

    var postData = {};
    postData["FlowIdentifier"] = currentFlowIdentifier;
    postData["SourceId"] = sourceId;
    postData["DestinationAfterId"] = destinationAfterId;
    postData["DestinationContainerId"] = destinationContainerId;
    postData["SequenceIndex"] = sequenceIndex;

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

function SubmitCreateCodeCreator(newCodeCreator, destinationAfterId, destinationContainerId, sequenceIndex, type, factory) {

    OnFlowChange();

    var postData = {};
    postData["FlowIdentifier"] = currentFlowIdentifier;
    postData["DestinationAfterId"] = destinationAfterId;
    postData["DestinationContainerId"] = destinationContainerId;
    postData["Type"] = type;
    postData["CustomFactory"] = factory;
    postData["SequenceIndex"] = sequenceIndex;

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

                SubmitCompile();

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

    OnFlowChange();

    var postData = {};
    postData["FlowIdentifier"] = currentFlowIdentifier;
    postData["Id"] = id;

    $.ajax({
        url: "Action/CodeCreatorDeleted",
        type: 'post',
        data: JSON.stringify(postData),
        contentType: "application/json",
        success: function (data) {
            if (data.isSuccess) {
                SubmitCompile();
                return;
            }
            alert(data.message);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert(errorThrown);
        }
    });
}

function SubmitRunFlow() {

    OnFlowSave();

    var postData = {};
    postData["FlowIdentifier"] = currentFlowIdentifier;

    $.ajax({
        url: "Action/RunFlow",
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


function SubmitSaveFlow() {

    OnFlowSave();

    var postData = {};
    postData["FlowIdentifier"] = currentFlowIdentifier;

    $.ajax({
        url: "Action/SaveFlow",
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

function SubmitResetFlow() {
    var postData = {};
    postData["FlowIdentifier"] = currentFlowIdentifier;

    $.ajax({
        url: "Action/ResetFlow",
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
}

function SubmitGetGeneratedCode(id) {
    var postData = {};
    postData["FlowIdentifier"] = currentFlowIdentifier;
    postData["Id"] = id;

    $.ajax({
        url: "Action/GetGeneratedCode",
        type: 'post',
        data: JSON.stringify(postData),
        contentType: "application/json",
        success: function (data) {
            $('#modalEditor').modal('show');

            dialogeditor.setValue(data.message);

            setTimeout(function () {
                dialogeditor.layout();
            }, 200);

            //navigator.clipboard.writeText(data.message);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert(errorThrown);
        }
    });
}

function SubmitGetCodeCreatorDisplayNames() {
    $.ajax({
        url: "Action/GetCodeCreatorDisplayNames",
        type: 'get',
        contentType: "application/json",
        success: function (data) {
            $.each(data.listValues, function (i, entry) {
                var key = entry.guid;
                var value = entry.value;
                var codecreator = ($(".codecreator").filter((i, c) => $(c).data("customfactory") == key));
                codecreator.parent().parent().find(".displayname").each((i, e) => $(e).html(value));
            });
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert(errorThrown);
        }
    });
}


function SubmitUpdateNote(pId, pNewText) {

    OnFlowChange();

    var postData = {};
    postData["FlowIdentifier"] = currentFlowIdentifier;
    postData["Id"] = pId;
    postData["Value"] = pNewText;

    $.ajax({
        url: "Action/UpdateNote",
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

function SubmitUpdateColor(pId, pNewColor) {

    OnFlowChange();

    var postData = {};
    postData["FlowIdentifier"] = currentFlowIdentifier;
    postData["Id"] = pId;
    postData["Value"] = pNewColor;

    $.ajax({
        url: "Action/UpdateColor",
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


function SubmitDebuggerAttach(pId) {
    var postData = {};
    postData["FlowIdentifier"] = currentFlowIdentifier;
    postData["Id"] = pId;

    $.ajax({
        url: "Action/DebuggerAttach",
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

function SubmitDebuggerChangeBreakpoint(pIdentifier, pAdd) {
    var postData = {};
    postData["FlowIdentifier"] = currentFlowIdentifier;
    postData["Id"] = pIdentifier;
    postData["Bool"] = pAdd;

    $.ajax({
        url: "Action/DebuggerChangeBreakpoint",
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

function SubmitDebuggerRunCommand(pCommand) {
    var postData = {};
    postData["FlowIdentifier"] = currentFlowIdentifier;
    postData["Value"] = pCommand;

    $.ajax({
        url: "Action/DebuggerRunCommand",
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


