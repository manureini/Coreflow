
$.fn.hasId = function (id) {
    return this.attr('id') == id;
};

function generateUuid() {
    return ([1e7] + -1e3 + -4e3 + -8e3 + -1e11).replace(/[018]/g, c =>
        (c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> c / 4).toString(16)
    )
}


var dialogeditor;
var currentParamEditor;

$(function () {


    $(".codecreator").disableSelection();
    $(".codecreator-space").disableSelection();

    var offsetY = 0;
    var offsetX = 0;

    var selected;

    $(document).on("click", ".codecreator", function (e) {
        //  alert($(this).data('id'));

        e.stopPropagation();

        var codeCreator = $(e.target);

        if (!codeCreator.is("div")) {
            codeCreator = codeCreator.parents("div")[0];
        }

        selected = codeCreator;

        $("#parameter-box").html("");

        if ($(codeCreator).children(".codecreator-parameters").length > 0) {

            $("#parameter-box").html($(codeCreator).children(".codecreator-parameters").html());

            $("#parameter-box #codecreator-parameters-table .codecreator-parameters-table-text").each(function (i, val) {


                var editor = monaco.editor.create(val, {
                    value: String($(val).data('value')),
                    language: 'csharp',
                    theme: 'vs-dark',
                    contextmenu: false,
                    minimap: {
                        enabled: false
                    },
                    lineNumbers: false,
                    scrollBeyondLastLine: false,
                    scrollbar: {
                        horizontalScrollbarSize: 0,
                        verticalScrollbarSize: 0
                    },
                });

                editor.onDidChangeModelContent(function (e) {
                    var creatorid = $(val.parentElement.parentElement.parentElement.parentElement).data("creator-id");
                    var parameter = $(val.parentElement.parentElement).data("id");
                    var newText = editor.getValue();

                    var htmlMapping = $("#main .codecreator-parameters-table-text[data-id='" + parameter + "']");

                    if (htmlMapping.length != 1) {
                        alert("error length of parameter html != 1");
                        return;
                    }

                    htmlMapping.attr('data-value', newText);

                    SubmitParameterTextChanged(creatorid, parameter, newText);
                });


                $(this).dblclick(function () {

                    $('#modalEditor').modal('show');
                    currentParamEditor = editor;

                    var text = currentParamEditor.getValue();

                    dialogeditor.setValue(text);

                    setTimeout(function () {
                        dialogeditor.layout();
                    }, 200);

                    $(document).one("modalEditorClosed", function () {
                        currentParamEditor.setValue(dialogeditor.getValue());
                    });

                });


            });
        }

        $(".codecreator-active").removeClass("codecreator-active");
        $(codeCreator).addClass("codecreator-active");
    });

    $(".codecreator").each(function (i, e) {
        SetDraggable(e);
    });

    var oldHtml = "";

    var firstRun = true;

    $("#codecreator-panel .codecreator-panel-parameter").draggable({

        revert: "invalid",

        distance: 20,
        delay: 30,

        create: function (event, ui) {
        },

        start: function (event, ui) {
            oldHtml = $(this).html();

            createDropSpace(event, ui, $(this));

            $(this).html($(this).children(".codecreator-panel-parameter-container"));
            $(this).find(".codecreator-panel-parameter-container").attr("style", "display: block !important; z-index: 999");
        },


        drag: function (event, ui) {

            //   $(this).css('position', 'absolute');
            if (firstRun) {

                var offset = $(this).offset();
                var relativeX = (event.pageX - offset.left);
                var relativeY = (event.pageY - offset.top);

                offsetX = relativeX - 5;
                offsetY = relativeY + 5;
                firstRun = false;
            }

            ui.position.top += offsetY;
            ui.position.left += offsetX;
        },

        cursor: "move",
        //   cursorAt: { top: 0, left: 0 },

        stop: function (event, ui) {
            $(".codecreator-space").remove();
            $(this).html(oldHtml);
            firstRun = true;
        }

    });

    $(document).keydown(function (e) {
        const key = event.key;
        if (key === "Delete"
            && $(e.target).parents("#parameter-box").length <= 0 //press inside textbox
            && $(e.target).parents("dialog").length <= 0 //press inside dialog
            && $(e.target).parents(".modal").length <= 0 //press inside modal
            && !$(e.target).is("input") //press inside input
        ) {
            SubmitDeleteCodeCreator($(selected).data("id"));
            $("#parameter-box").html("");
            $(selected).remove();
        }
    });

    $("#codecreator-panel").resizable({
        handles: 'e, w'
    });
    $("#codecreator-panel").resize(function () {
        RecalculateBoxSizes();
    });

    RecalculateBoxSizes();

    $("#parameter-box").resizable({
        handles: 'e, w'
    });
    $("#parameter-box").resize(function () {
        RecalculateBoxSizes();
    });


    $("#RunFlowbtn").click(SubmitRunFlow);

    $(document).on("change", ".input-displayname", function () {
        var ccid = $(this).parents("div").eq(0).data("id");
        //   var ccid = $(this).parent().closest('div').data("id");

        if ($(this).attr('id') == "input-flow-name") {
            ccid = "flow-name";
            setTimeout(() => SubmitGetCodeCreatorDisplayNames(), 1000);
        }
        SubmitUserDisplayNameChanged(ccid, $(this).val());
    });
    $(document).on("keydown", ".input-displayname", function () {
        RecalculateInputBox($(this));
    });

    $(document).on("click", ".add-referenced-namespace", function () {
        var value = $(this).next("input").val();
        $(this).next("input").val("");
        SubmitFlowReferencedNamespaceChanged(true, value);
    });
    $(document).on("click", ".remove-referenced-namespace", function () {
        $(this).parent().remove();
        SubmitFlowReferencedNamespaceChanged(false, $(this).data("id"));
    });

    /*
    $(document).on("click", ".add-referenced-assembly", function () {
        var value = $(this).next("input").val();
        $(this).next("input").val("");
        SubmitFlowReferencedAssemblyChanged(true, value);
    });
    $(document).on("click", ".remove-referenced-assembly", function () {
        $(this).parent().remove();
        SubmitFlowReferencedAssemblyChanged(false, $(this).data("id"));
    });*/

    $(document).on("click", ".add-argument", function () {
        var name = $("#add-argument-name").val();
        var type = $("#add-argument-type").val();
        var expression = $("#add-argument-expression").val();

        $("#add-argument-name").val("");
        $("#add-argument-type").val("");
        $("#add-argument-expression").val("");

        var newRow = "<tr><td><i class=\"fa fa-trash clickable remove-argument\" data-id=\"" + name + "\"></i></td><td>" + name + "</td><td>" + type + "</td><td>" + expression + "</td></tr>";
        $("#table-arguments tbody tr:nth-last-child(1)").before(newRow);

        SubmitFlowArgumentChanged(true, name, type, expression);
    });
    $(document).on("click", ".remove-argument", function () {
        $(this).closest("tr").remove();
        SubmitFlowArgumentChanged(false, $(this).data("id"));
    });




    $(document).on("click", ".nav a.active", function (e) {
        $(this).removeClass("active");
        $(".tab-content .active").removeClass("active");
        $(".tab-content .show").removeClass("show");
    });


    $("#btnShowGeneratedCode").click(function () {
        SubmitGetGeneratedCode("todo guid");
    });

    $("#modalEditor").on('hidden.bs.modal', function () {
        $(this).trigger("modalEditorClosed");
    });

    $(this).delay(50).queue(function () {
        dialogeditor = monaco.editor.create(document.getElementById("dialog-editor"), {
            value: "",
            language: 'csharp',
            theme: 'vs-dark',

            scrollBeyondLastLine: false,
            scrollbar: {
                horizontalScrollbarSize: 0,
                verticalScrollbarSize: 0
            },
        });
    });
});

$(window).resize(function () {
    RecalculateBoxSizes();
});

function RecalculateBoxSizes() {

    $("#editor-center").css("left", $("#codecreator-panel").width() + 20);
    $("#editor-center").width($(window).width() - $("#codecreator-panel").width() - $("#parameter-box").width() - 20);

    $('.input-displayname').each(function (i, e) {
        RecalculateInputBox(e);
    });
}

function RecalculateInputBox(pInput) {
    $("#hidden-span").html("<h4>" + $(pInput).val() + "</h4>");
    var hidden_span_scroll_width = $("#hidden-span")[0].scrollWidth;

    if (hidden_span_scroll_width > 50 || hidden_span_scroll_width < 600) { //TODO synchonize with css
        $(pInput).css("width", hidden_span_scroll_width + 20);
    }
}

function GetCodeCreatorSpaceHtml() {
    return '<div class="codecreator-space"></div>';
}

function createDropSpace(event, ui, selected) {
    $(".codecreator")
        .not(selected)  //can't add on it's own
        .not($(selected).prev(".codecreator")) //can't add one before
        /*  .filter(function (i, e) {
              return $(e).parent().parent().hasClass("codecreator-container"); // only possible if parent is codecreator container
          })*/
        .filter(function (i, e) {
            return !$(e).parent().hasId("main");
        })
        .filter(function (i, e) {
            return !$(e).hasClass("ui-draggable-dragging") && !$(e).parents().hasClass("ui-draggable-dragging"); // no dropspace inside currently dragging div
        })
        .filter(function (i, e) {
            return !$(e).hasClass("old-position") && !$(e).parents().hasClass("old-position"); // no dropspace inside old position div
        })
        .after(GetCodeCreatorSpaceHtml());


    //no childrens are in code container yet
    $("#main .codecreator-container-start")
        .filter(function (i, e) {
            return !$(e).parent().children().filter(".codecreator").first().is(selected); //currently selected is not the first
        })
        .filter(function (i, e) {
            return !$(e).hasClass("ui-draggable-dragging") && !$(e).parents().hasClass("ui-draggable-dragging"); // no dropspace inside currently dragging div
        })
        .filter(function (i, e) {
            return !$(e).hasClass("old-position") && !$(e).parents().hasClass("old-position"); // no dropspace inside old position div
        })
        .after(GetCodeCreatorSpaceHtml());


    //Flow is empty
    if ($("#main").children().length == 0) {
        $("#main").append(GetCodeCreatorSpaceHtml());
    }


    SetDroppable($(".codecreator-space"));
}

var offsetX = 0;
var offsetY = 0;
var oldScrollPosition = 0;


function SetDraggable(element) {

    $(element).draggable({

        revert: "invalid",

        distance: 20,
        delay: 30,

        create: function (event, ui) {
        },

        start: function (event, ui) {

            $(this).addClass("old-position");

            var y = $(this).offset().top;
            oldScrollPosition = $(document).scrollTop();

            $("#main").css("padding-bottom", "100vh");
            $("#center-box").hide();

            createDropSpace(event, ui, this);

            var offset = $(this).offset();

            var yOffsetChanged = (offset.top - y);

            var relativeX = (event.pageX - offset.left);
            var relativeY = (event.pageY - offset.top) + yOffsetChanged;

            ui.originalPosition.top += yOffsetChanged;

            offsetX = relativeX + 5;
            offsetY = relativeY + 5;

            $('html, body').animate({
                scrollTop: (oldScrollPosition + yOffsetChanged)
            }, 1);

            //This is a workaround for chrome. It scrolls sometimes 20px wrong. Is there a bug with margin: 20px?
            $('html, body').animate({
                scrollTop: (oldScrollPosition + yOffsetChanged)
            }, 2);
        },

        drag: function (event, ui) {
            ui.position.top += offsetY;
            ui.position.left += offsetX;
        },

        cursor: "grabbing",
        //   cursorAt: { top: 0, left: 0 },

        opacity: 1,

        helper: "clone",

        stop: function (event, ui) {
            $(this).removeClass("old-position");

            $("#main").css("padding-bottom", "");
            $("#center-box").show();

            $(".codecreator-space").remove();

            $('html, body').animate({
                scrollTop: (oldScrollPosition)
            }, 0);
        }
    });
}


function SetDroppable(element) {

    element.droppable({

        drop: function (event, ui) {

            var sourceId = $(ui.draggable).data("id");
            var destAfterId = $(this).prev().data("id");
            var destContainerId = $(this).closest(".codecreator-container").data("id");
            var sequenceIndex = $(this).prev(".codecreator-container-start").data("index");

            if (destAfterId == 0) {
                destAfterId = null;
            }

            ui.draggable.css('top', '').css('left', '');

            if ($(ui.draggable).is(".codecreator-panel-parameter")) { //new item droped

                //$(ui.draggable).html($(ui.draggable).html());

                var type = $(ui.draggable).children().children().data('type');
                var factory = $(ui.draggable).children().children().data("customfactory")

                $(this).before($(ui.draggable.children()[0]).children());

                SetDraggable($(this).prev());

                SubmitCreateCodeCreator($(this).prev(), destAfterId, destContainerId, sequenceIndex, type, factory);
                return;
            }

            $(this).before(ui.draggable);

            SubmitMoveAfter(sourceId, destAfterId, destContainerId, sequenceIndex);
        },

        over: function (event, ui) {

            $(this).css('background', '#777');
        },

        out: function (event, ui) {
            $(this).css('background', '');
        },

        tolerance: "pointer",

    });
}
