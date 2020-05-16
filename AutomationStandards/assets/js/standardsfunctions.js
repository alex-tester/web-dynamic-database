
//#region Form Fields
window.makeFieldRequired = function (fieldName, showField, showConfirmation) {
    //set required attribute
    $('#' + fieldName).attr('required', 'required');

    //Custom fields that have required attributes set
    if ($("#" + fieldName + "TargetSelectList").length > 0) {
        $('#' + fieldName + "TargetSelectList").attr('required', 'required');
    }

    if ($("#search_" + fieldName).length > 0) {
        $('#search_' + fieldName).attr('required', 'required');
        $("#searchCheck_" + fieldName).attr('required', 'required');
    }

    //set confirmation attribute if desired
    if (showConfirmation) {

        if ($("#display_" + fieldName).length > 0) {
            $("#display_" + fieldName).attr('data-displayconfirmation', 'true');
        }
        else {

            $('#' + fieldName).attr('data-displayconfirmation', 'true');
        }
    }



    if (showField) {
        $('#div_' + fieldName).fadeIn('fast');
    }


    //show asterisk

    $('#req_' + fieldName).show();



}


window.makeFieldNotRequired = function (fieldName, hideField, hideConfirmation, clearField) {
    //remove required attribute
    $('#' + fieldName).removeAttr('required');

    //Custom fields that have required attributes set
    if ($("#" + fieldName + "TargetSelectList").length > 0) {
        $('#' + fieldName + "TargetSelectList").removeAttr('required');
    }

    if ($("#search_" + fieldName).length > 0) {
        $('#search_' + fieldName).removeAttr('required');
        $("#searchCheck_" + fieldName).removeAttr('required');
    }

    //set confirmation attribute if desired
    if (hideConfirmation) {
        if ($("#display_" + fieldName).length > 0) {
            $("#display_" + fieldName).attr('data-displayconfirmation', 'false');
        }
        else {

            $('#' + fieldName).attr('data-displayconfirmation', 'false');
        }
    }

    if (hideField) {

        $('#div_' + fieldName).fadeOut('fast');
    }

    if (clearField) {

        setFieldValue(fieldName, '');

        //if this is a select2 searchable dropdown, there are 2 locations to remove the value from - the above command removes it from the hidden field, the one below removes it from visibility
        if ($(field).siblings('select').hasClass('js-select2')) {
            $(field).siblings('select').select2("val", "");

        }

        if ($(field).hasClass('js-select2')) {
            $(field).select2("val", "");
        }

    }



    //remove asterisk

    $('#req_' + fieldName).hide();






}

window.setFieldValue = function (fieldName, fieldValue, setDefaultValue) {
    field = $('#' + fieldName)
    $(field).val(fieldValue);

    if (typeof setDefaultValue === 'undefined') {
        setDefaultValue = false;
    }

    if (setDefaultValue) {
        setFieldDefaultValue(fieldName, fieldValue);
    }

    //if span
    if ($(field).is('span')) {
        $('#' + fieldName).text(fieldValue);
    }

    //if this is a label, also clear the text
    if ($(field).siblings('span').is('#' + fieldName + "l")) {
        $('#' + fieldName + "l").html(fieldValue);
    }

    //if this is a select2 searchable dropdown, there are 2 locations to remove the value from - the above command removes it from the hidden field, the one below removes it from visibility
    if ($(field).siblings('select').hasClass('js-select2')) {
        $(field).siblings('select').select2("val", fieldValue).trigger('change');

    }



    if ($(field).hasClass('js-select2')) {
        if (fieldValue != null && fieldValue != "") {
            if (fieldValue.toString().indexOf(",") >= 0) {
                var sValues = fieldValue.split(",");
                $(field).select2().val(sValues).trigger('change')
            }
            else {
                $(field).select2().val(fieldValue).trigger('change');
            }
        }
    }

    if ($("#" + fieldName + "TargetSelectList").length > 0 && $("#" + fieldName + "SourceSelectList").length > 0) {
        var sourceList = $('#' + fieldName + 'SourceSelectList');
        var targetList = $("#" + fieldName + 'TargetSelectList');
        var tbholdingvalue = $("#" + fieldName);


        //$('#' + fieldName + 'RefreshList').click();

        $.each(fieldValue.split(","), function (i, e) {
            $("#" + fieldName + "SourceSelectList  option[value='" + e + "']").prop("selected", true);
            //var t = "stop really quick";
        });

        moveItemOver(sourceList, targetList);
        UpdateListSelectDisplay(tbholdingvalue, targetList.find("option"));
    }

    if ($("input[name=" + fieldName + "YNB]").length > 0) {
        if (fieldValue == "yes") {
            $("#" + fieldName + "YNB_Yes").prop('checked', true).trigger('click');
        }
        else {
            $("#" + fieldName + "YNB_No").prop('checked', true).trigger('click');
        }
    }

    if ($("#search_" + fieldName).length) {
        x = 'setField_' + fieldName;
        var fn = window[x];
        fn(fieldValue);
    }


    var fieldID = $('#' + fieldName).attr('name');
    if ($('.rangeslider-box_' + fieldID).length > 0)
    {
        var tempFieldValue = fieldValue;
        if (tempFieldValue == '')
        {
            tempFieldValue = getFieldDefaultValue(fieldName);
        }

        $('.rangeslider-box_' + fieldID).val(tempFieldValue).change();
    }

}
window.setFieldDefaultValue = function (fieldName, fieldValue) {
    field = $('#' + fieldName)
    $(field).data("defaultvalue", fieldValue);
}

window.getFieldValue = function (fieldName) {

    var returnValue;

    field = $('#' + fieldName)
    returnValue = $(field).val();

    return returnValue;
}

window.getFieldDisplayValue = function (fieldName) {
    var returnValue;

    field = $('#' + fieldName)

    returnValue = $(field).val();

    if ($(field).is('select')) {
        returnValue = $('#' + fieldName + " option:selected").text();
    }

    return returnValue;
}

window.getFieldDefaultValue = function (fieldName) {
    var returnValue;

    field = $('#' + fieldName)
    returnValue = $(field).data("defaultvalue");

    return returnValue;
}

window.makeFieldVisible = function (fieldName) {
    $('#div_' + fieldName).fadeIn('fast');
}

window.makeFieldHidden = function (fieldName) {
    $('#div_' + fieldName).fadeOut('fast');
}



window.enableNextButton = function (modalIdToClose) {
    //enable the next button, and close whichever modal name was passed in

    $('#wizardNextBtn').removeAttr("disabled");

    if (modalIdToClose != "") {
        $('#' + modalIdToClose).modal('hide');
        $('.modal-backdrop').hide();
        $('body').removeClass('modal-open');
    }
}

window.disableNextButton = function (modalIdToClose) {
    //enable the next button, and close whichever modal name was passed in

    $('#wizardNextBtn').attr("disabled", "disabled");

    if (modalIdToClose != "") {
        $('#' + modalIdToClose).modal('hide');
        $('.modal-backdrop').hide();
        $('body').removeClass('modal-open');
    }
}


window.hideWizardTab = function (tIndex, resetFields) {

    //make wizard modules are loaded and attached 
    //$('.js-wizard-validation').bootstrapWizard();

    if (resetFields) {
        alert('is resetFields has been reached');
        var tID = $(".navtab:eq(" + tIndex + ")").attr("id").split('-')[1]


        //get all InputFields
        var allInputs = $("#validation-step" + tID).find(':input');



        allInputs.each(function (index) {
            if ($(this).is('input:text') || $(this).is('input:hidden') || $(this).is('select') || $(this).is('textarea')) {

                var fName = $(this).attr('id');
                var defVal = $(this).attr('data-defaultvalue');
                console.log(fName + " : " + defVal);
                setFieldValue(fName, defVal);

            }
        });
    }
    //the last thing we do is hide
    $('.js-wizard-validation').bootstrapWizard('hide', tIndex);
}

window.showWizardTab = function (tIndex) {
    //make wizard modules are loaded and attached 
    //$('.js-wizard-validation').bootstrapWizard();


    $('.js-wizard-validation').bootstrapWizard('display', tIndex);
}

    // Builds the HTML Table out of json data
    //pass in a json string and an ID to use for the html element

function buildHtmlTable(myList, randomDivId) {
    $("#" + randomDivId).html("");
    myList = JSON.parse(myList);
    var columns = addAllColumnHeaders(myList, randomDivId);

    $("#" + randomDivId).append('<tbody id="tbody_' + randomDivId + '">');

    for (var i = 0; i < myList.length; i++) {
        var row$ = $('<tr/>');
        for (var colIndex = 0; colIndex < columns.length; colIndex++) {
            var cellValue = myList[i][columns[colIndex]];

            if (cellValue == null) { cellValue = ""; }

            row$.append($('<td/>').html(cellValue));
        }

        row$.append('</tbody>')
        $("#" + randomDivId).append(row$);
    }
}

// Adds a header row to the table and returns the set of columns.
// Need to do union of keys from all records as some records may not contain
// all records
function addAllColumnHeaders(myList, randomDivId) {
    var columnSet = [];
    $("#" + randomDivId).append('<thead id="thead_' + randomDivId + '">');

    var headerTr$ = $('<tr/>');

    for (var i = 0; i < myList.length; i++) {
        var rowHash = myList[i];

        for (var key in rowHash) {

            if ($.inArray(key, columnSet) == -1) {
                columnSet.push(key);
                headerTr$.append($('<th/>').html(key));
            }

        }

    }
    headerTr$.append('</thead>');
    $("#thead_" + randomDivId).append(headerTr$);

    return columnSet;
}

//#endregion

//Views/Standard/Viewer.cshtml




//Views/Standard/Index.cshtml


//#region Views/Manage/TaskType.cshtml
function RefreshTaskTypeGrid() {
    $("#divGrid").fadeOut("fast").fadeIn("fast").load(getActionURL("TaskTypeGrid", "Manage"));
}



function ShowTaskTypeEditor(TaskTypeID) {
    ShowContent(TaskTypeID, 'left', 'right', 'TaskTypeEditor', 'Manage');
}

function CloseTaskTypeEditor() {


    ShowContent(null, 'right', 'left', 'TaskTypeGrid', 'Manage');

}

function ProcessTaskTypeForm() {
    var v = $("#frmEditor").kendoValidator().data("kendoValidator");

    var isValid = v.validate();
    if (isValid) {
        $.ajax({
            type: "POST",
            url: getActionURL("ProcessTaskTypeModel", "Manage"),
            data: $("#frmEditor").serialize(), // serializes the form's elements.
            success: function (result) {
                CloseTaskTypeEditor();
            },
            failure: function (result) {
                alert(result);
            }
        })
    }
}

function DeleteTaskType(id) {

    $.ajax({
        type: "POST",
        url: getActionURL("DeleteTaskTypeModel", "Manage"),
        data: $.param({ "id": id }), // serializes the form's elements.
        success: function (result) {

            RefreshTaskTypeGrid();
        },
        failure: function (result) {
            alert(result);
        }
    });

}
//#endregion

//#region Views/Manage/SubCategory.cshtml

function RefreshSubCategoryGrid() {
    $("#divGrid").fadeOut("fast").fadeIn("fast").load(getActionURL("SubCategoryGrid", "Manage"));
}

function ShowSubCategoryEditor(CategoryID) {
    ShowContent(CategoryID, 'left', 'right', 'SubCategoryEditor', 'Manage');
}

function CloseSubCategoryEditor() {


    ShowContent(null, 'right', 'left', 'SubCategoryGrid', 'Manage');

}

function ProcessSubCategoryForm() {
    var v = $("#frmEditor").kendoValidator().data("kendoValidator");

    var isValid = v.validate();
    if (isValid) {
        $.ajax({
            type: "POST",
            url: getActionURL("ProcessSubCategoryModel", "Manage"),
            data: $("#frmEditor").serialize(), // serializes the form's elements.
            success: function (result) {
                CloseSubCategoryEditor();
            },
            failure: function (result) {
                alert(result);
            }
        })
    }
}

function DeleteSubCategory(id) {

    $.ajax({
        type: "POST",
        url: getActionURL("DeleteSubCategoryModel", "Manage"),
        data: $.param({ "id": id }), // serializes the form's elements.
        success: function (result) {

            RefreshSubCategoryGrid();
        },
        failure: function (result) {
            alert(result);
        }
    });

}

//#endregion

//#region Views/Manage/Status.cshtml

function RefreshStatusGrid() {
    $("#divGrid").fadeOut("fast").fadeIn("fast").load(getActionURL("StatusGrid", "Manage"));
}



function ShowStatusEditor(StatusID) {
    ShowContent(StatusID, 'left', 'right', 'StatusEditor', 'Manage');
}

function CloseStatusEditor() {


    ShowContent(null, 'right', 'left', 'StatusGrid', 'Manage');

}

function ProcessStatusForm() {
    var v = $("#frmEditor").kendoValidator().data("kendoValidator");

    var isValid = v.validate();
    if (isValid) {
        $.ajax({
            type: "POST",
            url: getActionURL("ProcessStatusModel", "Manage"),
            data: $("#frmEditor").serialize(), // serializes the form's elements.
            success: function (result) {
                CloseStatusEditor();
            },
            failure: function (result) {
                alert(result);
            }
        })
    }
}

function DeleteStatus(id) {

    $.ajax({
        type: "POST",
        url: getActionURL("DeleteStatusModel", "Manage"),
        data: $.param({ "id": id }), // serializes the form's elements.
        success: function (result) {

            RefreshStatusGrid();
        },
        failure: function (result) {
            alert(result);
        }
    });

}

//#endregion

//#region Views/Manage/StandardGroup.cshtml

function RefreshStandardGroupGrid() {
    $("#divGrid").fadeOut("fast").fadeIn("fast").load(getActionURL("StandardGroupGrid", "Manage"));
}



function ShowStandardGroupEditor(StandardGroupID) {
    ShowContent(StandardGroupID, 'left', 'right', 'StandardGroupEditor', 'Manage');
}

function CloseStandardGroupEditor() {


    ShowContent(null, 'right', 'left', 'StandardGroupGrid', 'Manage');

}

function ProcessStandardGroupForm() {
    if ($("#frmEditor").valid()) {
        $.ajax({
            type: "POST",
            url: getActionURL("ProcessStandardGroupModel", "Manage"),
            data: $("#frmEditor").serialize(), // serializes the form's elements.
            success: function (result) {
                CloseStandardGroupEditor();
            },
            failure: function (result) {
                alert(result);
            }
        })
    }
}

function DeleteStandardGroup(id) {

    $.ajax({
        type: "POST",
        url: getActionURL("DeleteStandardGroupModel", "Manage"),
        data: $.param({ "id": id }), // serializes the form's elements.
        success: function (result) {

            RefreshStandardGroupGrid();
        },
        failure: function (result) {
            alert(result);
        }
    });

}
//#endregion

//#region Views/Manage/StandardDataType.cshtml

function RefreshStandardDataTypeGrid() {
    $("#divGrid").fadeOut("fast").fadeIn("fast").load(getActionURL("StandardDataTypeGrid", "Manage"));
}



function ShowStandardDataTypeEditor(StandardDataTypeID) {
    ShowContent(StandardDataTypeID, 'left', 'right', 'StandardDataTypeEditor', 'Manage');
}

function CloseStandardDataTypeEditor() {


    ShowContent(null, 'right', 'left', 'StandardDataTypeGrid', 'Manage');

}

function ProcessStandardDataTypeForm() {
    var v = $("#frmEditor").kendoValidator().data("kendoValidator");

    var isValid = v.validate();
    if (isValid) {
        $.ajax({
            type: "POST",
            url: getActionURL("ProcessStandardDataTypeModel", "Manage"),
            data: $("#frmEditor").serialize(), // serializes the form's elements.
            success: function (result) {
                CloseStandardDataTypeEditor();
            },
            failure: function (result) {
                alert(result);
            }
        })
    }
}

function DeleteStandardDataType(id) {

    $.ajax({
        type: "POST",
        url: getActionURL("DeleteStandardDataTypeModel", "Manage"),
        data: $.param({ "id": id }), // serializes the form's elements.
        success: function (result) {

            RefreshStandardDataTypeGrid();
        },
        failure: function (result) {
            alert(result);
        }
    });

}
//#endregion

//#region Views/Manage/Standard.cshtml

function RefreshStandardGrid() {
    $("#divGrid").fadeOut("fast").fadeIn("fast").load(getActionURL("StandardGrid", "Manage"));
}

function ShowStandardEditor(StandardID) {
    ShowContent(StandardID, 'left', 'right', 'StandardEditor', 'Manage');
}

function CloseStandardEditor() {


    ShowContent(null, 'right', 'left', 'StandardGrid', 'Manage');

}

function ProcessStandardStandard() {
    var v = $("#frmEditor").kendoValidator().data("kendoValidator");

    var isValid = v.validate();
    if (isValid) {
        $.ajax({
            type: "POST",
            url: getActionURL("ProcessStandardModel", "Manage"),
            data: $("#frmEditor").serialize(), // serializes the Standard's elements.
            success: function (result) {
                CloseStandardEditor();
            },
            failure: function (result) {
                alert(result);
            }
        })
    }
}

function DeleteStandard(id) {

    $.ajax({
        type: "POST",
        url: getActionURL("DeleteStandardModel", "Manage"),
        data: $.param({ "id": id }), // serializes the Standard's elements.
        success: function (result) {

            RefreshStandardGrid();
        },
        failure: function (result) {
            alert(result);
        }
    });

}


function RefreshStandardConfigGrid(StandardID) {
    $("#divGrid").fadeOut("fast").fadeIn("fast").load(getActionURL("StandardConfigGrid", "Manage"), { id: StandardID });
}
function ShowStandardConfigGrid(StandardID) {
    ShowContent(StandardID, 'left', 'right', 'StandardConfigGrid', 'Manage');
}

function CloseStandardConfigGrid() {
    ShowContent(null, 'right', 'left', 'StandardGrid', 'Manage');
}


function ShowStandardConfigEditor(id, StandardID) {

    $('#divGrid').hide('slide', { direction: 'left' }, 500).promise().done(function () {

        $.ajax({
            type: "POST",
            url: getActionURL("StandardConfigEditor", "Manage"),
            data: $.param({ "id": id, StandardID: StandardID }),
            success: function (result) {

                $("#divGrid").html(result);
            },
            failure: function (result) {
                alert(result);
            }
        });


        $('#divGrid').show('slide', { direction: 'right' }, 500);
    });
}



function CloseStandardConfigEditor(StandardID) {

    ShowContent(StandardID, 'right', 'left', 'StandardConfigGrid', 'Manage');

}



function ProcessStandardConfigStandard(StandardID) {
    var v = $("#frmEditor").kendoValidator().data("kendoValidator");

    var isValid = v.validate();

    if (isValid) {
        $.ajax({
            type: "POST",
            url: getActionURL("ProcessStandardConfigModel", "Manage"),
            data: $("#frmEditor").serialize(), // serializes the Standard's elements.
            success: function (result) {
                CloseStandardConfigEditor(StandardID);
            },
            failure: function (result) {
                alert(result);
            }
        })
    }
}

function DeleteStandardConfig(id, StandardID) {

    $.ajax({
        type: "POST",
        url: getActionURL("DeleteStandardConfigModel", "Manage"),
        data: $.param({ "id": id }), // serializes the Standard's elements.
        success: function (result) {

            RefreshStandardConfigGrid(StandardID);
        },
        failure: function (result) {
            alert(result);
        }
    });

}
//#endregion

//Views/Manage/RequestType.cshtml

function RefreshRequestTypeGrid() {
    $("#divGrid").fadeOut("fast").fadeIn("fast").load(getActionURL("RequestTypeGrid", "Manage"));
}



function ShowRequestTypeEditor(RequestTypeID) {
    ShowContent(RequestTypeID, 'left', 'right', 'RequestTypeEditor', 'Manage');
}

function CloseRequestTypeEditor() {


    ShowContent(null, 'right', 'left', 'RequestTypeGrid', 'Manage');

}

function ProcessRequestTypeForm() {
    var v = $("#frmEditor").kendoValidator().data("kendoValidator");

    var isValid = v.validate();
    if (isValid) {
        $.ajax({
            type: "POST",
            url: getActionURL("ProcessRequestTypeModel", "Manage"),
            data: $("#frmEditor").serialize(), // serializes the form's elements.
            success: function (result) {
                CloseRequestTypeEditor();
            },
            failure: function (result) {
                alert(result);
            }
        })
    }
}

function DeleteRequestType(id) {

    $.ajax({
        type: "POST",
        url: getActionURL("DeleteRequestTypeModel", "Manage"),
        data: $.param({ "id": id }), // serializes the form's elements.
        success: function (result) {

            RefreshRequestTypeGrid();
        },
        failure: function (result) {
            alert(result);
        }
    });

}

//Views/Manage/Request.cshtml

var Temp;
var htmlTemplate;
var editSource;
var detailSource;

function ShowInitialGrid() {
    var rID = $('#RequestID').val();
    ShowContent(null, 'left', 'right', 'RequestInitial', 'Manage');
}

function LoadRequestGrid() {
    var rID = $('#RequestID').val();
    var rType = $('#RequestType').val();
    var rBy = $('#RequestBy').val();

    $('#divGrid').hide('slide', { direction: 'left' }, 500).promise().done(function () {
        var NavUrl = getActionURL("RequestGridNew", "Manage");

        $.ajax({
            type: "POST",
            url: NavUrl,
            data: $.param({ RequestID: rID, RequestType: rType, RequestBy: rBy }),
            success: function (result) {
                $("#divGrid").html(result);
            },
            failure: function (result) {
                alert(result);
            }
        });

        $('#divGrid').show('slide', { direction: 'right' }, 500);
    });
}

//function LoadRequestGrid() {
    //var rID = $('#RequestID').val();
    //ShowContent(rID, 'left', 'right', 'RequestInitial', 'Manage');
//}

function ShowRequestEditor(FormID,url) {
    editSource = url;
    ShowContent(FormID, 'left', 'right', 'RequestEditor', 'Manage');
}

function CloseRequestWindow(url) {
    window.location.replace(url);
}

function ShowRequestDetails(RequestID, url) {
    //ShowContent(FormID, 'left', 'right', 'RequestDetails', 'Catalog');

    detailSource = url;

    console.log(detailSource);

    htmlTemplate = '<div> <div class="block "> \
                        <div class="col-md-12"> \
                        <div id="RequestStatus">{RequestStatus} \
                        </div> \
                    </div> \
                </div> \
                <div class="block "> \
                    <div class="col-md-12"> \
                        <div id="RequestValues"> {RequestValues} \
                        </div> \
                    </div> \
                </div> \
                <div class="block "> \
                    <div class="col-md-12"> \
                        <div id="RequestTasks"> {RequestTasks} \
                        </div> \
                    </div> \
                </div> \
            </div>'



    $.ajax({
        type: "POST",
        url: getActionURL("RequestStatus", "Catalog"),
        data: $.param({ "id": RequestID }),
        async: false,
        success: function (result) {
            console.log(result);
            htmlTemplate = htmlTemplate.replace("{RequestStatus}", result);
        },
        failure: function (result) {
            alert(result);
        }
    });

    $.ajax({
        type: "POST",
        url: getActionURL("RequestValues", "Catalog"),
        data: $.param({ "id": RequestID }),
        async: false,
        success: function (result) {
            htmlTemplate = htmlTemplate.replace("{RequestValues}", result);
        },
        failure: function (result) {
            alert(result);
        }
    });

    $.ajax({
        type: "POST",
        url: getActionURL("RequestTasks", "Catalog"),
        data: $.param({ "id": RequestID }),
        async: false,
        success: function (result) {
            htmlTemplate = htmlTemplate.replace("{RequestTasks}", result);
        },
        failure: function (result) {
            alert(result);
        }
    });


    $("#divGrid").html(htmlTemplate);

}
function toggleView(tr) {

    $('#' + tr).toggle('slow');

}


function SaveRequestValue(rID, ffID) {
    var nv = $('#txt' + ffID).val();

    $.ajax({
        type: "POST",
        url: getActionURL("UpdateRequestValue", "Manage"),
        data: { id: rID, FormFieldID: ffID, NewValue: nv },
        success: function (result) {

            var content;
            if (result.result) {
                content = "<p>The Request Value was updated Successfully.</p>";
                ConfigureAndShowGlobalModal("success", "Update Request Value", content, true, "OK", "", "", false, "", "", "");
            }
            else {
                content = "<p>The Request Value was not updated.</p>";
                ConfigureAndShowGlobalModal("error", "Update Request Value", content, false, "", "", "", true, "OK", "", "");
            }

        },
        failure: function (result) {
            alert(result);
        }
    })
}

function RestartRequest(id) {
    $.ajax({
        type: "POST",
        url: getActionURL("RestartRequest", "Manage"),
        data: { id: id },
        success: function (result) {

            var content;
            if (result.result) {
                content = "<p>The Request was Successfully Restarted.</p>";
                ConfigureAndShowGlobalModal("success", "Restart Request", content, true, "OK", "", "", false, "", "", "");
            }
            else {
                content = "<p>The Request was not restarted.</p>";
                ConfigureAndShowGlobalModal("error", "Restart Request", content, false, "", "", "", true, "OK", "", "");
            }

            ShowInitialGrid();

        },
        failure: function (result) {
            alert(result);
        }
    })

}

function TerminateRequest(id) {

    $.ajax({
        type: "POST",
        url: getActionURL("TerminateRequest", "Manage"),
        data: { id: id },
        success: function (result) {

            var content;
            if (result.result) {
                content = "<p>The Request was Successfully Terminated.</p>";
                ConfigureAndShowGlobalModal("success", "Terminate Request", content, true, "OK", "", "", false, "", "", "");
            }
            else {
                content = "<p>The Request was not Terminated completed.</p>";
                ConfigureAndShowGlobalModal("error", "Terminate Request", content, false, "", "", "", true, "OK", "", "");
            }

            ShowInitialGrid();
        },
        failure: function (result) {
            alert(result);
        }
    })
}
function DeleteRequest(id) {
    $.ajax({
        type: "POST",
        url: getActionURL("DeleteRequest", "Manage"),
        data: { id: id },
        success: function (result) {

            var content;
            if (result.result) {
                content = "<p>The Request was Successfully Deleted.</p>";
                ConfigureAndShowGlobalModal("success", "Delete Request", content, true, "OK", "", "", false, "", "", "");
            }
            else {
                content = "<p>The Request was not Deleted.</p>";
                ConfigureAndShowGlobalModal("error", "Delete Request", content, false, "", "", "", true, "OK", "", "");
            }

            ShowInitialGrid();

        },
        failure: function (result) {
            alert(result);
        }
    })
}

function CompleteRequest(id) {
    $.ajax({
        type: "POST",
        url: getActionURL("CompleteRequest", "Manage"),
        data: { id: id },
        success: function (result) {

            var content;
            if (result.result) {
                content = "<p>The Request was Successfully Marked Completed.</p>";
                ConfigureAndShowGlobalModal("success", "Complete Request", content, true, "OK", "", "", false, "", "", "");
            }
            else {
                content = "<p>The Request was not marked Completed.</p>";
                ConfigureAndShowGlobalModal("error", "Completed Request", content, false, "", "", "", true, "OK", "", "");
            }

            ShowInitialGrid();

        },
        failure: function (result) {
            alert(result);
        }
    })
}


//Views/Manage/FormCollection.cshtml

function RefreshFormCollectionGrid() {
    $("#divGrid").fadeOut("fast").fadeIn("fast").load(getActionURL("FormCollectionGrid", "Manage"));
}

function ShowFormCollectionEditor(FormCollectionID) {
    ShowContent(FormCollectionID, 'left', 'right', 'FormCollectionEditor', 'Manage');
}

function CloseFormCollectionEditor() {


    ShowContent(null, 'right', 'left', 'FormCollectionGrid', 'Manage');

}

function ProcessFormCollectionForm() {
    var v = $("#frmEditor").kendoValidator().data("kendoValidator");

    var isValid = v.validate();
    if (isValid) {
        $.ajax({
            type: "POST",
            url: getActionURL("ProcessFormCollectionModel", "Manage"),
            data: $("#frmEditor").serialize(), // serializes the form's elements.
            success: function (result) {
                CloseFormCollectionEditor();
            },
            failure: function (result) {
                alert(result);
            }
        })
    }
}

function DeleteFormCollection(id) {

    $.ajax({
        type: "POST",
        url: getActionURL("DeleteFormCollectionModel", "Manage"),
        data: $.param({ "id": id }), // serializes the form's elements.
        success: function (result) {

            RefreshFormCollectionGrid();
        },
        failure: function (result) {
            alert(result);
        }
    });

}




//Views/Manage/NotificationTemplate.cshtml

function ShowNotificationTemplateViewer(id) {
    $('#divGrid').hide('slide', { direction: 'left' }, 500).promise().done(function () {

        $.ajax({
            type: "GET",
            url: getActionURL("NotificationTemplateEditor", "Manage"),
            data: $.param({ "id": id, FormMode: "Viewer" }),
            success: function (result) {

                $("#divGrid").html(result);
            },
            failure: function (result) {
                alert(result);
            }
        });


        $('#divGrid').show('slide', { direction: 'right' }, 500);
    });

}

function ShowNotificationTemplateEditor(id) {



    $('#divGrid').hide('slide', { direction: 'left' }, 500).promise().done(function () {

        $.ajax({
            type: "GET",
            url: getActionURL("NotificationTemplateEditor", "Manage"),
            data: $.param({ "id": id, FormMode: "Edit" }),
            success: function (result) {

                $("#divGrid").html(result);
            },
            failure: function (result) {
                alert(result);
            }
        });


        $('#divGrid').show('slide', { direction: 'right' }, 500);
    });
}

function CloseNotificationTemplateEditor() {


    $("#divGrid").fadeOut("fast");

}

function ProcessTemplateModel() {
    var v = $("#frmEditor").kendoValidator().data("kendoValidator");
    //workaround because notification body isnt populating in controller
    $('#NotificationBody').html($('#NotificationBody').val());
    var isValid = v.validate();

    if (isValid) {
        $.ajax({
            type: "POST",
            url: getActionURL("ProcessNotificationTemplateModel", "Manage"),
            data: $("#frmEditor").serialize(), // serializes the form's elements.
            success: function (result) {

                RefreshTemplateList();
                ShowNotificationTemplateViewer(result.result);
            },
            failure: function (result) {
                alert(result);
            }
        })
    }
}

function DeleteNotificationTemplate(id) {

    $.ajax({
        type: "POST",
        url: getActionURL("DeleteNotificationTemplateModel", "Manage"),
        data: $.param({ "id": id }), // serializes the form's elements.
        success: function (result) {

            RefreshTemplateList();
        },
        failure: function (result) {
            alert(result);
        }
    });

}

//Form Collectin Set Section
function RefreshFormCollectionSetGrid(FormCollectionID) {
    $("#divGrid").fadeOut("fast").fadeIn("fast").load(getActionURL("FormCollectionSetGrid", "Manage"), { id: FormCollectionID });
}
function ShowFormCollectionSetGrid(FormCollectionID) {
    ShowContent(FormCollectionID, 'left', 'right', 'FormCollectionSetGrid', 'Manage');
}

function CloseFormCollectionSetGrid() {
    ShowContent(null, 'right', 'left', 'FormCollectionGrid', 'Manage');
}


function ShowFormCollectionSetEditor(id, FormCollectionID) {

    $('#divGrid').hide('slide', { direction: 'left' }, 500).promise().done(function () {

        $.ajax({
            type: "POST",
            url: getActionURL("FormCollectionSetEditor", "Manage"),
            data: $.param({ "id": id, FormCollectionID: FormCollectionID }),
            success: function (result) {

                $("#divGrid").html(result);
            },
            failure: function (result) {
                alert(result);
            }
        });


        $('#divGrid').show('slide', { direction: 'right' }, 500);
    });;
}



function CloseFormCollectionSetEditor(FormCollectionID) {

    ShowContent(FormCollectionID, 'right', 'left', 'FormCollectionSetGrid', 'Manage');

}



function ProcessFormCollectionSetForm(FormCollectionID) {
    var v = $("#frmEditor").kendoValidator().data("kendoValidator");

    var isValid = v.validate();

    if (isValid) {
        $.ajax({
            type: "POST",
            url: getActionURL("ProcessFormCollectionSetModel", "Manage"),
            data: $("#frmEditor").serialize(), // serializes the form's elements.
            success: function (result) {
                CloseFormCollectionSetEditor(FormCollectionID);
            },
            failure: function (result) {
                alert(result);
            }
        })
    }
}

function DeleteFormCollectionSet(id, FormCollectionID) {

    $.ajax({
        type: "POST",
        url: getActionURL("DeleteFormCollectionSetModel", "Manage"),
        data: $.param({ "id": id }), // serializes the form's elements.
        success: function (result) {

            RefreshFormCollectionSetGrid(FormCollectionID);
        },
        failure: function (result) {
            alert(result);
        }
    });

}



//#region Views/Manage/Form.cshtml

function RefreshFormGrid() {
    $("#divGrid").fadeOut("fast").fadeIn("fast").load(getActionURL("FormGrid", "Manage"));
}

function ShowFormEditor(FormID) {
    ShowContent(FormID, 'left', 'right', 'FormEditor', 'Manage');
}

function CloseFormEditor() {


    ShowContent(null, 'right', 'left', 'FormGrid', 'Manage');

}

function ProcessFormForm() {
    var v = $("#frmEditor").kendoValidator().data("kendoValidator");

    var isValid = v.validate();
    if (isValid) {
        $.ajax({
            type: "POST",
            url: getActionURL("ProcessFormModel", "Manage"),
            data: $("#frmEditor").serialize(), // serializes the form's elements.
            success: function (result) {
                CloseFormEditor();
            },
            failure: function (result) {
                alert(result);
            }
        })
    }
}

function DeleteForm(id) {

    $.ajax({
        type: "POST",
        url: getActionURL("DeleteFormModel", "Manage"),
        data: $.param({ "id": id }), // serializes the form's elements.
        success: function (result) {

            RefreshFormGrid();
        },
        failure: function (result) {
            alert(result);
        }
    });

}


function RefreshFormFieldGrid(FormID) {
    $("#divGrid").fadeOut("fast").fadeIn("fast").load(getActionURL("FormFieldGrid", "Manage"), { id: FormID });
}
function ShowFormFieldGrid(FormID) {
    ShowContent(FormID, 'left', 'right', 'FormFieldGrid', 'Manage');
}

function CloseFormFieldGrid() {
    ShowContent(null, 'right', 'left', 'FormGrid', 'Manage');
}


function ShowFormFieldEditor(id, FormID, FieldID) {

    $('#divGrid').hide('slide', { direction: 'left' }, 500).promise().done(function () {

        $.ajax({
            type: "POST",
            url: getActionURL("FormFieldEditor", "Manage"),
            data: $.param({ "id": id, FormID: FormID, FieldID: FieldID }),
            success: function (result) {

                $("#divGrid").html(result);
            },
            failure: function (result) {
                alert(result);
            }
        });


        $('#divGrid').show('slide', { direction: 'right' }, 500);
    });;
}



function CloseFormFieldEditor(FormID) {

    var path = window.location.pathname
    if (path.includes("/Field")) //More verbose than if FormID == 0
    {
        //Means we came here from /Manage/Field to add a new field to an existing form. Return there.
        CloseFieldEditor()
    }
    else
    {
        ShowContent(FormID, 'right', 'left', 'FormFieldGrid', 'Manage');
    }

}



function ProcessFormFieldForm(FormID) {
    var v = $("#frmEditor").kendoValidator().data("kendoValidator");

    var isValid = v.validate();

    if (isValid) {
        $.ajax({
            type: "POST",
            url: getActionURL("ProcessFormFieldModel", "Manage"),
            data: $("#frmEditor").serialize(), // serializes the form's elements.
            success: function (result) {
                CloseFormFieldEditor(FormID);
            },
            failure: function (result) {
                alert(result);
            }
        })
    }
}
function ProcessFormFieldFormUpdateOnly(FormID) {
    var v = $("#frmEditor").kendoValidator().data("kendoValidator");

    var isValid = v.validate();

    if (isValid) {
        $.ajax({
            type: "POST",
            url: getActionURL("ProcessFormFieldModel", "Manage"),
            data: $("#frmEditor").serialize(), // serializes the form's elements.
            success: function (result) {
                //CloseFormFieldEditor(FormID);
            },
            failure: function (result) {
                alert(result);
            }
        })
    }
}

function DeleteFormField(id, FormID) {

    $.ajax({
        type: "POST",
        url: getActionURL("DeleteFormFieldModel", "Manage"),
        data: $.param({ "id": id }), // serializes the form's elements.
        success: function (result) {

            RefreshFormFieldGrid(FormID);
        },
        failure: function (result) {
            alert(result);
        }
    });

}


function ShowDuplicateForm() {
    ShowContent(null, 'left', 'right', 'FormDuplicate', 'Manage');
}

function ProcessDuplicateForm() {
        var v = $("#frmEditor").kendoValidator().data("kendoValidator");

        var isValid = v.validate();
        if (isValid) {

            var navurl = getActionURL("ProcessDuplicateForm", "Manage");
            
            $.ajax({
                type: "POST",
                url: navurl,
                data: $("#frmEditor").serialize(), // serializes the form's elements.
                success: function (result) {
                    CloseFormEditor();
                },
                failure: function (result) {
                    alert(result);
                }
            })
        }
    }


function GetControllerDDL() {

    var selected = document.getElementById('varControllerName').value;
    $.ajax({
        type: "GET",
        url: getActionURL("GetControllerDDL", "Manage"),
        dataType: "json",
        success: function (obj) {
            var listItems = "<option value=''>Please Select an Option</option>";
            for (var i = 0; i < obj.length; i++) {
                if (obj[i].Text == selected) {
                    listItems += "<option selected='true' value='" + obj[i].Value + "'>" + obj[i].Text + "</option>";
                }
                else {
                    listItems += "<option value='" + obj[i].Value + "'>" + obj[i].Text + "</option>";
                }
            }
            $("#ControllerName").html(listItems);
            $("#ControllerName").val(selected).trigger('change');
            GetControllerActionDDL();
        }
    });
}

function GetControllerActionDDL() {

    var controller = document.getElementById('ControllerName').value;
    var selected = document.getElementById('varActionName').value;
    $.ajax({
        type: "POST",
        url: getActionURL("GetControllerActionDDL", "Manage"),
        data: { "controller": controller },
        dataType: "json",
        success: function (obj) {
            var listItems = "<option value=''>Please Select an Option</option>";
            for (var i = 0; i < obj.length; i++) {
                if (obj[i].Text == selected) {
                    listItems += "<option selected='true' value='" + obj[i].Value + "'>" + obj[i].Text + "</option>";
                }
                else {
                    listItems += "<option value='" + obj[i].Value + "'>" + obj[i].Text + "</option>";
                }
            }
            $("#ActionName").html(listItems);
            $("#ActionName").val(selected).trigger('change');
        }
    });
}

function GetChildFieldDDL() {
    var FormID = document.getElementById('FormID').value;
    var selected = document.getElementById('varChildFields').value;
    $.ajax({
        type: "POST",
        url: getActionURL("GetChildFieldDDL", "Manage"),
        data: { FormID: FormID, selected: selected },
        success: function (listItems) {
            $("#ChildFieldsForm").html(listItems);
            $("#ChildFieldsForm").val(selected.split(",")).trigger('change');
            //alert(selected.split(','));
        }
    });
}

//#endregion
//Views/Manage/FieldType.cshtml

function RefreshFieldTypeGrid() {
    $("#divGrid").fadeOut("fast").fadeIn("fast").load(getActionURL("FieldTypeGrid", "Manage"));
}

function ShowFieldTypeEditor(id) {

    ShowContent(id, 'left', 'right', 'FieldTypeEditor', 'Manage');
}

function CloseFieldTypeEditor() {


    ShowContent(null, 'right', 'left', 'FieldTypeGrid', 'Manage');

}

function ProcessFieldTypeForm() {
    var v = $("#frmEditor").kendoValidator().data("kendoValidator");

    var isValid = v.validate();
    if (isValid) {
        $.ajax({
            type: "POST",
            url: getActionURL("ProcessFieldTypeModel", "Manage"),
            data: $("#frmEditor").serialize(), // serializes the form's elements.
            success: function (result) {
                CloseFieldTypeEditor();
            },
            failure: function (result) {
                alert(result);
            }
        })
    }
}

function DeleteFieldType(id) {

    $.ajax({
        type: "POST",
        url: getActionURL("DeleteFieldTypeModel", "Manage"),
        data: $.param({ "id": id }), // serializes the form's elements.
        success: function (result) {

            RefreshFieldTypeGrid();
        },
        failure: function (result) {
            alert(result);
        }
    });

}



//Views/Manage/Field.cshtml

function RefreshFieldGrid() {
    $("#divGrid").fadeOut("fast").fadeIn("fast").load(getActionURL("FieldGrid", "Manage"));
}



function ShowFieldEditor(FieldID) {
    ShowContent(FieldID, 'left', 'right', 'FieldEditor', 'Manage');
}

function CloseFieldEditor() {


    ShowContent(null, 'right', 'left', 'FieldGrid', 'Manage');

}

function ProcessFieldForm(AddFieldToForm) {
    var v = $("#frmEditor").kendoValidator().data("kendoValidator");

    var isValid = v.validate();
    if (isValid) {
        $.ajax({
            type: "POST",
            url: getActionURL("ProcessFieldModel", "Manage"),
            data: $("#frmEditor").serialize(), // serializes the form's elements.
            success: function (result) {
                if (AddFieldToForm) {
                    var FieldID = result.result;
                    ShowFormFieldEditor('0', '0', FieldID)
                }
                else {
                    CloseFieldEditor();
                }
            },
            failure: function (result) {
                alert(result);
            }
        })
    }
}

function DeleteField(id) {

    $.ajax({
        type: "POST",
        url: getActionURL("DeleteFieldModel", "Manage"),
        data: $.param({ "id": id }), // serializes the form's elements.
        success: function (result) {

            RefreshFieldGrid();
        },
        failure: function (result) {
            alert(result);
        }
    });

}

function ShowFieldUsage(id, name) {
    showLoadingSpinner('Please Wait')
    $.ajax({
        type: "POST",
        url: getActionURL("GetFieldUsage", "Manage"),
        data: $.param({ "FieldID": id }), // serializes the form's elements.
        success: function (result) {
            hideLoadingSpinner()
            ConfigureAndShowGlobalModal("success", "Instances of " + name, result, true, 'OK', 'javascript:CloseGlobalModal()', '', false, '', '', '', true);
        },
        failure: function (result) {
            hideLoadingSpinner()
            alert(result);
        }
    });

}


//Views/Manage/Category.cshtml

function RefreshCategoryGrid() {
    $("#divGrid").fadeOut("fast").fadeIn("fast").load(getActionURL("CategoryGrid", "Manage"));
}

function ShowCategoryEditor(CategoryID) {
    ShowContent(CategoryID, 'left', 'right', 'CategoryEditor', 'Manage');
}

function CloseCategoryEditor() {


    ShowContent(null, 'right', 'left', 'CategoryGrid', 'Manage');

}

function ProcessCategoryForm() {
    var v = $("#frmEditor").kendoValidator().data("kendoValidator");

    var isValid = v.validate();
    if (isValid) {
        $.ajax({
            type: "POST",
            url: getActionURL("ProcessCategoryModel", "Manage"),
            data: $("#frmEditor").serialize(), // serializes the form's elements.
            success: function (result) {
                CloseCategoryEditor();
            },
            failure: function (result) {
                alert(result);
            }
        })
    }
}

function DeleteCategory(id) {

    $.ajax({
        type: "POST",
        url: getActionURL("DeleteCategoryModel", "Manage"),
        data: $.param({ "id": id }), // serializes the form's elements.
        success: function (result) {

            RefreshCategoryGrid();
        },
        failure: function (result) {
            alert(result);
        }
    });

}


function RefreshCategoryConfigGrid(CategoryID) {
    $("#divGrid").fadeOut("fast").fadeIn("fast").load(getActionURL("CategoryConfigGrid", "Manage"), { id: CategoryID });
}
function ShowCategoryConfigGrid(CategoryID) {
    ShowContent(CategoryID, 'left', 'right', 'CategoryConfigGrid', 'Manage');
}

function CloseCategoryConfigGrid() {
    ShowContent(null, 'right', 'left', 'CategoryGrid', 'Manage');
}


function ShowCategoryConfigEditor(id, CategoryID) {

    $('#divGrid').hide('slide', { direction: 'left' }, 500).promise().done(function () {

        $.ajax({
            type: "POST",
            url: getActionURL("CategoryConfigEditor", "Manage"),
            data: $.param({ "id": id, CategoryID: CategoryID }),
            success: function (result) {

                $("#divGrid").html(result);
            },
            failure: function (result) {
                alert(result);
            }
        });


        $('#divGrid').show('slide', { direction: 'right' }, 500);
    });
}



function CloseCategoryConfigEditor(CategoryID) {

    ShowContent(CategoryID, 'right', 'left', 'CategoryConfigGrid', 'Manage');

}



function ProcessCategoryConfigForm(CategoryID) {
    var v = $("#frmEditor").kendoValidator().data("kendoValidator");

    var isValid = v.validate();

    if (isValid) {
        $.ajax({
            type: "POST",
            url: getActionURL("ProcessCategoryConfigModel", "Manage"),
            data: $("#frmEditor").serialize(), // serializes the form's elements.
            success: function (result) {
                CloseCategoryConfigEditor(CategoryID);
            },
            failure: function (result) {
                alert(result);
            }
        })
    }
}

function DeleteCategoryConfig(id, CategoryID) {

    $.ajax({
        type: "POST",
        url: getActionURL("DeleteCategoryConfigModel", "Manage"),
        data: $.param({ "id": id }), // serializes the form's elements.
        success: function (result) {

            RefreshCategoryConfigGrid(CategoryID);
        },
        failure: function (result) {
            alert(result);
        }
    });

}

function GetCategoryIconDDL() {

    var selected = document.getElementById('varIconName').value;

    $.ajax({
        type: "GET",
        url: getActionURL("GetIconDDL", "Manage"),
        dataType: "json",
        success: function (obj) {
            var listItems = "<option value=''>Please Select an Option</option>";
            for (var i = 0; i < obj.length; i++) {
                if (obj[i].Text == selected) {
                    listItems += "<option selected='true' value='" + obj[i].Value + "' " + "data-icon='" + obj[i].Value + "'>" + obj[i].Text + "</option>";
                }
                else {
                    listItems += "<option value='" + obj[i].Value + "' " + "data-icon='" + obj[i].Value + "'>" + obj[i].Text + "</option>";
                }
            }
            $("#IconName").html(listItems);
            //initialize icons on DDL
            $('#IconName').select2({
                width: "100%",
                templateSelection: iformat,
                templateResult: iformat,
                allowHtml: true
            });

            $("#IconName").val(selected).trigger('change');

        }
    });
}
//helper function to initialize icons on select2 DDL
function iformat(icon) {
    var originalOption = icon.element;
    var i = $(originalOption).data('icon');
    if (i != null) {
        var c = i.match('^[^-]*');
    }
    return $('<span><i class="' + c + ' ' + $(originalOption).data('icon') + '"></i> ' + icon.text + '</span>');
}

//Notification Code Section.
function RefreshNotificationGrid(ID) {
    $("#divGrid").fadeOut("fast").fadeIn("fast").load(getActionURL("NotificationGrid", "Manage"), { id: ID });
}
function ShowNotificationGrid(ID) {
    ShowContent(ID, 'left', 'right', 'NotificationGrid', 'Manage');
}

function CloseNotificationGrid() {
    ShowContent(null, 'right', 'left', 'Grid', 'Manage');
}

function ShowNotificationViewer(id, CatID) {
    $('#divNotificationShell').hide('slide', { direction: 'left' }, 500).promise().done(function () {

        $.ajax({
            type: "GET",
            url: getActionURL("NotificationEditor", "Manage"),
            data: $.param({ "id": id, CategoryID: CatID, FormMode: "Viewer" }),
            success: function (result) {

                $("#divNotificationShell").html(result);
            },
            failure: function (result) {
                alert(result);
            }
        });


        $('#divNotificationShell').show('slide', { direction: 'right' }, 500);
    });

}

function ShowNotificationEditor(id, ID) {



    $('#divNotificationShell').hide('slide', { direction: 'left' }, 500).promise().done(function () {

        $.ajax({
            type: "GET",
            url: getActionURL("NotificationEditor", "Manage"),
            data: $.param({ "id": id, CategoryID: ID, FormMode: "Edit" }),
            success: function (result) {

                $("#divNotificationShell").html(result);
            },
            failure: function (result) {
                alert(result);
            }
        });


        $('#divNotificationShell').show('slide', { direction: 'right' }, 500);
    });
}

function CloseNotificationEditor() {
    $("#divNotificationShell").fadeOut("fast");
}

function ProcessNotificationForm(ID) {


    var v = $("#frmEditor").kendoValidator().data("kendoValidator");

    var isValid = v.validate();

    var HTMLBody = $('#Body').summernote('code');

    if (isValid) {

        $.ajax({
            type: "POST",
            url: getActionURL("ProcessNotificationModel", "Manage"),
            data: $("#frmEditor").serialize() + "&" + $.param({ "Body": HTMLBody }), // serializes the form's elements.
            success: function (result) {
                //CloseNotificationEditor(ID);
                RefreshNotificationGrid(ID);
                ShowNotificationViewer(result.result, ID);
            },
            failure: function (result) {
                alert(result);
            }
        })
    }
}

function DeleteNotification(id, ID) {

    $.ajax({
        type: "POST",
        url: getActionURL("DeleteNotificationModel", "Manage"),
        data: $.param({ "id": id }), // serializes the form's elements.
        success: function (result) {

            //ShowNotificationViewer(ID);
            RefreshNotificationGrid(ID);
        },
        failure: function (result) {
            alert(result);
        }
    });

}

function ShowNotificationPreview(CatName, notName) {
    var rID = $("#RequestID").val();
    $.ajax({
        type: "POST",
        url: getActionURL("PreviewNotification", "Manage"),
        data: $.param({ "id": rID, "CatName": CatName, "NotName": notName }), // serializes the form's elements.
        success: function (result) {


            var b = '<table class="table table-condensed"><tbody>';
            b = b + '<tr><td>' + result.result.To + '</td></tr>';
            b = b + '<tr><td>' + result.result.Subject + '</td></tr>';
            b = b + '<tr><td>' + result.result.Body + '</td></tr></tbody></table>';

            ConfigureAndShowGlobalModal("info", "Preview Notification", b, true, "OK", "", "", false, "", "", "");
            $("#globalModal").find('.modal-dialog').css("width", "70%");
            $("#globalModal").find('.modal-dialog').css("margin-top", "5%");

        },
        failure: function (result) {
            alert(result);
        }
    });

}

function ShowDuplicateNotification(id) {

    $('#divNotificationShell').hide('slide', { direction: 'left' }, 500).promise().done(function () {

        $.ajax({
            type: "GET",
            url: getActionURL("NotificationDuplicate", "Manage"),
            data: $.param({ CatID: id }),
            success: function (result) {

                $("#divNotificationShell").html(result);
            },
            failure: function (result) {
                alert(result);
            }
        });


        $('#divNotificationShell').show('slide', { direction: 'right' }, 500);
    });
}

function ProcessDuplicateNotification(catID) {
    var v = $("#frmEditor").kendoValidator().data("kendoValidator");

    var isValid = v.validate();
    if (isValid) {
        $.ajax({
            type: "POST",
            url: getActionURL("ProcessDuplicateNotification", "Manage"),
            data: $("#frmEditor").serialize(), // serializes the form's elements.
            success: function (result) {
                RefreshNotificationGrid(catID);
            },
            failure: function (result) {
                alert(result);
            }
        })
    }
}


//requestStatus.cshtml

function ShowFullDetailsModal(details) {
    var htmldetails = "<div style='text-align:left;'>" + details + '</div>';
    ConfigureAndShowGlobalModal("info", "Full Details", htmldetails, true, "Close", "", "", false, "", "", "", false);
}


//tasksbycategory functions



function RestartRequest(id) {

    var link = getActionURL("RestartRequest", "Request");

    $.ajax({
        type: "POST",
        url: link,
        data: { id: id },
        success: function (result) {

            var content;
            if (result.result) {
                content = "<p>The Request was Successfully Restarted.</p>";
                ConfigureAndShowGlobalModal("success", "Restart Request", content, true, "OK", "", "", false, "", "", "");
            }
            else {
                content = "<p>The Request was not restarted.</p>";
                ConfigureAndShowGlobalModal("error", "Restart Request", content, false, "", "", "", true, "OK", "", "");
            }

            loadCategoryGrid();

        },
        failure: function (result) {
            alert(result);
        }
    })

}

function TerminateRequest(id) {

    var link = getActionURL("TerminateRequest", "Request");

    $.ajax({
        type: "POST",
        url: link,
        data: { id: id },
        success: function (result) {

            var content;
            if (result.result) {
                content = "<p>The Request was Successfully Terminated.</p>";
                ConfigureAndShowGlobalModal("success", "Terminate Request", content, true, "OK", "", "", false, "", "", "");
            }
            else {
                content = "<p>The Request was not Terminated completed.</p>";
                ConfigureAndShowGlobalModal("error", "Terminate Request", content, false, "", "", "", true, "OK", "", "");
            }

            loadCategoryGrid();
        },
        failure: function (result) {
            alert(result);
        }
    })
}

function DeleteRequest(id) {
    $.ajax({
        type: "POST",
        url: getActionURL("DeleteRequest", "Manage"),
        data: { id: id },
        success: function (result) {

            var content;
            if (result.result) {
                content = "<p>The Request was Successfully Deleted.</p>";
                ConfigureAndShowGlobalModal("success", "Delete Request", content, true, "OK", "", "", false, "", "", "");
            }
            else {
                content = "<p>The Request was not Deleted.</p>";
                ConfigureAndShowGlobalModal("error", "Delete Request", content, false, "", "", "", true, "OK", "", "");
            }

            RefreshData();

        },
        failure: function (result) {
            alert(result);
        }
    })
}

function CompleteRequest(id) {
    $.ajax({
        type: "POST",
        url: getActionURL("CompleteRequest", "Manage"),
        data: { id: id },
        success: function (result) {

            var content;
            if (result.result) {
                content = "<p>The Request was Successfully Marked Completed.</p>";
                ConfigureAndShowGlobalModal("success", "Complete Request", content, true, "OK", "", "", false, "", "", "");
            }
            else {
                content = "<p>The Request was not marked Completed.</p>";
                ConfigureAndShowGlobalModal("error", "Completed Request", content, false, "", "", "", true, "OK", "", "");
            }

            RefreshData();

        },
        failure: function (result) {
            alert(result);
        }
    })
}

function SaveRequestValue(rID, ffID) {
    var nv = $('#txt' + ffID).val();

    $.ajax({
        type: "POST",
        url: getActionURL("UpdateRequestValue", "Manage"),
        data: { id: rID, FormFieldID: ffID, NewValue: nv },
        success: function (result) {

            var content;
            if (result.result) {
                content = "<p>The Request Value was updated Successfully.</p>";
                ConfigureAndShowGlobalModal("success", "Update Request Value", content, true, "OK", "", "", false, "", "", "");
            }
            else {
                content = "<p>The Request Value was not updated.</p>";
                ConfigureAndShowGlobalModal("error", "Update Request Value", content, false, "", "", "", true, "OK", "", "");
            }

        },
        failure: function (result) {
            alert(result);
        }
    })
}

//Reporting_general& detailed functions

//Searching an item based on the text of textbox
var Tablesearch = $('#tblDetailed').DataTable();
Tablesearch.columns().every(function () {

    var that = this;
    $('input', this.footer()).on('keyup change', function () {
        if (that.search() !== this.value) {
            that
                .search(this.value)
                .draw();
        }
    });
});


function exportTableToCSV($table, filename) {

    var $rows = $table.find('tr:has(td),tr:has(th)'),

        // Temporary delimiter characters unlikely to be typed by keyboard
        // This is to avoid accidentally splitting the actual contents
        tmpColDelim = String.fromCharCode(11), // vertical tab character
        tmpRowDelim = String.fromCharCode(0), // null character

        // actual delimiter characters for CSV format
        colDelim = '","',
        rowDelim = '"\r\n"',

        // Grab text from table into CSV formatted string

        csv = '"' + $rows.map(function (i, row) {
            var $row = $(row), $cols = $row.find('td,th');

            return $cols.map(function (j, col) {
                var $col = $(col), text = $col.text();

                return text.replace(/"/g, '""').replace(/\s\s+/g, ""); // escape double quotes and remove extra spaces

            }).get().join(tmpColDelim);

        }).get().join(tmpRowDelim)
            .split(tmpRowDelim).join(rowDelim)
            .split(tmpColDelim).join(colDelim) + '"',





        // Data URI
        csvData = 'data:application/csv;charset=utf-8,' + encodeURIComponent(csv);

    console.log(csv);



    if (window.navigator.msSaveBlob) { // IE 10+
        //alert('IE' + csv);
        window.navigator.msSaveOrOpenBlob(new Blob([csv], { type: "text/plain;charset=utf-8;" }), "csvname.csv")
    }
    else {
        $(this).attr({ 'download': filename, 'href': csvData, 'target': '_blank' });
    }
};

//Functions for Reporting Overview

function RequestCount_blockmethod() {
    //Place the obs spinning logo in the box until the ajax completes (ajax will replace this html once completed)
    $('#BlockData_RequestCount').html('<img src="https://util.obs.org/SharedImages/hmm.gif" height="32px" class="center-block"/>');

    var cat = $("#ddlCategory option:selected").text();
    var fc = $("#ddlFormCollection option:selected").text();
    var sDate = $("#dtStart").val();
    var eDate = $("#dtEnd").val()

    $.ajax({
        type: "GET",
        url: $("#RequestCount").val(),
        data: { Category: cat, FormCollection: fc, StartDate: sDate, EndDate: eDate },
        dataType: 'json',
        contenttype: 'application/json',
        success: function (data) {

            $('#BlockData_RequestCount').html(data);

        },
        error: processError
    });




}

function AverageClose_blockmethod() {
    //Place the obs spinning logo in the box until the ajax completes (ajax will replace this html once completed)
    $('#BlockData_AverageClose').html('<img src="https://util.obs.org/SharedImages/hmm.gif" height="32px" class="center-block"/>');

    var cat = $("#ddlCategory option:selected").text();
    var fc = $("#ddlFormCollection option:selected").text();
    var sDate = $("#dtStart").val();
    var eDate = $("#dtEnd").val()

    $.ajax({
        type: "GET",
        url: $("#AverageClose").val(),
        data: { Category: cat, FormCollection: fc, StartDate: sDate, EndDate: eDate },
        dataType: 'json',
        contenttype: 'application/json',
        success: function (data) {
            Temp = data;
            $('#BlockData_AverageClose').html(data);

        },
        error: processError
    });




}

function RequestStatusBreakDown_blockmethod() {

    var cat = $("#ddlCategory option:selected").text();
    var fc = $("#ddlFormCollection option:selected").text();
    var sDate = $("#dtStart").val();
    var eDate = $("#dtEnd").val();

    var AnimationEnabled = true;

    $.ajax({
        type: "GET",
        //Url.Action(action, controller)
        url: $("#RequestStatusBreakdown").val(),
        data: { Category: cat, FormCollection: fc, StartDate: sDate, EndDate: eDate },
        dataType: 'json',
        contenttype: 'application/json',
        success: function (list) {

            tempError = list;
            var arr = [];

            var dataset = [];
            for (var i = 0; i < list.length; i++) {
                var obj = {};
                obj.color = list[i].Color + ".7)",
                    obj.highlight = list[i].Color + "1)",
                    obj.value = list[i].Value;
                obj.label = list[i].Label;

                dataset.push(obj);

            }




            var ctx = $("#BlockData_RequestStatusBreakDown").get(0).getContext('2d');

            var linechart = new Chart(ctx).Pie(dataset, { responside: true, animation: AnimationEnabled });

            var tbl = '<thead>';
            tbl += '<tr><th>Label</th><th>Value</th></tr></thead><tbody>';
            for (var i = 0; i < list.length; i++) {
                tbl += '<tr><td align="left">' + list[i].Label + '</td><td align="left">' + list[i].Value + '</td></tr>';
            }
            tbl += '</tbody>';

            $('#TableBlockData_RequestStatusBreakDown').html(tbl);

        },
        error: processError
    });

}

function RequestRequestorBreakDown_blockmethod() {

    var cat = $("#ddlCategory option:selected").text();
    var fc = $("#ddlFormCollection option:selected").text();
    var sDate = $("#dtStart").val();
    var eDate = $("#dtEnd").val();

    var AnimationEnabled = true;

    $.ajax({
        type: "GET",
        //Url.Action(action, controller)
        url: $("#RequestorBreakdown").val(),
        data: { Category: cat, FormCollection: fc, StartDate: sDate, EndDate: eDate },
        dataType: 'json',
        contenttype: 'application/json',
        success: function (list) {

            tempError = list;
            var arr = [];

            var dataset = [];
            for (var i = 0; i < list.length; i++) {
                var obj = {};
                obj.color = list[i].Color + ".7)",
                    obj.highlight = list[i].Color + "1)",
                    obj.value = list[i].Value;
                obj.label = list[i].Label;

                dataset.push(obj);

            }




            var ctx = $("#BlockData_RequestRequestorBreakDown").get(0).getContext('2d');

            var linechart = new Chart(ctx).Pie(dataset, { responside: true, animation: AnimationEnabled });

            var tbl = '<thead>';
            tbl += '<tr><th>Label</th><th>Value</th></tr></thead><tbody>';
            for (var i = 0; i < list.length; i++) {
                tbl += '<tr><td align="left">' + list[i].Label + '</td><td align="left">' + list[i].Value + '</td></tr>';
            }
            tbl += '</tbody>';

            $('#TableBlockData_RequestRequestorBreakDown').html(tbl);

        },
        error: processError
    });

}

//functions for reporting page

//TOGGLES THE FILTER FORMS
function hideFilter() {
    var x = document.getElementById("reqFilter");
    var y = document.getElementById("simpleSearch");
    if (y.style.display === "none") {
        y.style.display = "block";
        x.style.display = "none";
    } else {
        y.style.display = "none";
    }
};

function hideAdvFilter() {
    var x = document.getElementById("reqFilter");
    var y = document.getElementById("simpleSearch");
    if (x.style.display === "none") {
        x.style.display = "block";
        y.style.display = "none";
    } else {
        x.style.display = "none";
    }
};

function hideRptFilter() {
    var x = document.getElementById("reqFilter");
    if (x.style.display === "none") {
        x.style.display = "block";
    } else {
        x.style.display = "none";
    }
};


//Function to keep the page current with request progress

function LoadFCList() {
    var catID = $("#ddlCategory").val();
    var link = getActionURL("GetFormCollectionsByCategoryID", "Catalog");
    $.ajax({
        url: link,
        cache: false,
        async: false,
        data: { CategoryID: catID },
        success: function (result) {
            $('#ddlFormCollection').html(result);
        }
    });
}
function LoadFCFieldname() {
    var fcVal = $("#ddlFormCollection").val();

    var link = getActionURL("GetFieldNameList", "Catalog");
    $.ajax({
        url: link,
        cache: false,
        async: false,
        data: { FormCollection: fcVal },
        success: function (result) {
            $('#ddlColumns').html(result);
            $('#divReportingDetailed').html('');
        }
    });

    jQuery(function () {
        App.initHelpers(['select2', 'tags-inputs']);
    });
}

function processError(data, status, req) {

    tempError = data;

    //alert("ERROR " + data);
}


function LoadReport() {
    var btn = $('.filterbtn.btn-primary').attr('id');
    if (btn == "btnOverview") {
        LoadReportDashboard();
    }
    else {
        //ShowDetailedReport();
    }
}

function LoadReportDashboard() {

    var link = getActionURL("Reporting_Overview", "Catalog");

    $.ajax({
        url: link,
        cache: false,
        async: true,
        success: function (result) {
            $('#divOverview').html(result);
        }
    });

}
function ShowGeneralReport(source) {
    var cat = $("#ddlCategory option:selected").text();
    var fc = $("#ddlFormCollection option:selected").text();
    var sDate = $("#dtStart").val();
    var eDate = $("#dtEnd").val();

    var AnimationEnabled = true;

    $.ajax({
        type: "GET",

        url: getActionURL("Reporting_General", "Catalog"),
        data: { Source: source, Category: cat, FormCollection: fc, StartDate: sDate, EndDate: eDate },
        cache: false,
        async: true,
        success: function (result) {
            Temp = result;
            $('#divOverview').html(result);
        },
        error: processError
    });
}
function Chart_ToggleView(blockname) {

    if ($("#TableData_" + blockname).is(":visible")) {

        $('#TableData_' + blockname).fadeOut('fast', function () {
            $("#GraphData_" + blockname).fadeIn('fast');
        });
    }
    else {
        $('#GraphData_' + blockname).fadeOut('fast', function () {
            $("#TableData_" + blockname).fadeIn('fast');
        });
    }
}


function ShowDetailedReport() {
    var cat = $("#ddlCategory option:selected").text();
    var fc = $("#ddlFormCollection").val();
    var sDate = $("#dtStart").val();
    var eDate = $("#dtEnd").val();
    var cols = $("#ddlColumns").val();

    var AnimationEnabled = true;

    if (cols == null) {
        var body = 'Please select at least one column to show in the report.';
        ConfigureAndShowGlobalModal("error", "Select At Least One Column", body, true, "Got It", "", "", false, "", "", "", false);

    }
    else {

        $.ajax({
            type: "GET",

            url: getActionURL("Reporting_Detailed", "Catalog"),
            data: { ColumnList: cols.toString(), Category: cat, FormCollection: fc, StartDate: sDate, EndDate: eDate },
            cache: false,
            async: true,
            success: function (result) {
                $('#divReportingDetailed').html(result);
            },
            error: processError
        });
    }
}


//functions for request pages

function startChange() {
    var endPicker = $("#dtEnd").data("kendoDatePicker"),
        startDate = $("#dtStart").val();

    if (startDate) {
        startDate = new Date(startDate);
        startDate.setDate(startDate.getDate() + 1);
        endPicker.min(startDate);
    }
}

function endChange() {
    var startPicker = $("#dtStart").data("kendoDatePicker"),
        endDate = $("#dtEnd").val();

    if (endDate) {
        endDate = new Date(endDate);
        endDate.setDate(endDate.getDate() - 1);
        startPicker.max(endDate);
    }
}

function showColumnFilters() {

    $("#displayheading").toggle();

    if ($("#displayheading").is(":visible")) {
        $("#togglebutton").text('Hide Column Filters');
    } else {
        $("#togglebutton").text('Show Column Filters');
    }


}


var myInterval;
//Function to keep the page current with request progress
function autoRefreshMyRequests(seconds) {
    //clear current interval
    clearInterval(myInterval);
    //set a new one
    myInterval = setInterval(function () {
        LoadMyRequest();
    }, seconds * 1000); // 60 * 1000 milsec

}

var allInterval;
//Function to keep the page current with request progress
function autoRefreshMyRequests(seconds) {
    //clear current interval
    clearInterval(allInterval);
    //set a new one
    allInterval = setInterval(function () {
        LoadAllRequest();
    }, seconds * 1000); // 60 * 1000 milsec

}

function autoRefreshOff() {
    clearInterval(myInterval);
}

//AllRequests.cshtml


function LoadAllRequestAlex() {

    var rFilter = $("#RequestSearch").val();
    var idFilter;
    var r2Filter = $('#RequestorSearch').val();
    var status = $('#ddlStatus').val();
    var sDate = $("#dtStart").val();
    var eDate = $("#dtEnd").val();
    var simpleStat = document.getElementById("simpleSearch").style.display;
    var link = getActionURL("AllRequestGrid", "Catalog");

    if (simpleStat == "none") {
        idFilter = $('#IDSearchA').val();
    }
    else {
        idFilter = $('#IDSearchS').val();
    }

    $.ajax({
        url: link,
        cache: false,
        data: { FilterText: rFilter, IDFilter: idFilter, RequestorFilter: r2Filter, Status: status, StartDate: sDate, EndDate: eDate, simpleStatus: simpleStat, Source: 'AllRequestsTab' },
        success: function (result) {
            $('#MyRequest').html(result);
            $("#displayheading").hide();
            $("#togglebutton").text('Show Column Filters');
        }
    });

}
function LoadAllRequest() {

    var searchBy = $("#ddlSearchBy").val();
    var searchValue = $('#txtSearchValue').val();
    var status = $('#ddlStatus').val();
    var sDate = $("#dtStart").val();
    var eDate = $("#dtEnd").val();
    var link = getActionURL("AllRequestGrid", "Catalog");

    var isSimple = $("#simpleSearch").is(":visible");
    if (isSimple) {
        searchValue = $('#txtRequestID').val();
    }

    $.ajax({
        url: link,
        cache: false,
        data: { IsSimple: isSimple, SearchBy: searchBy, SearchValue: searchValue, Status: status, StartDate: sDate, EndDate: eDate, Source: 'AllRequestsTab' },
        success: function (result) {
            $('#MyRequest').html(result);

            $("#displayheading").hide();
            $("#togglebutton").text('Show Column Filters');
        }
    });
}


//myRequest.cshtml

function LoadMyRequestAlex() {

    var rFilter = $("#RequestSearch").val();
    var idFilter;
    var r2Filter = $('#RequestorSearch').val();
    var status = $('#ddlStatus').val();
    var sDate = $("#dtStart").val();
    var eDate = $("#dtEnd").val();
    var simpleStat = document.getElementById("simpleSearch").style.display;
    var link = getActionURL("MyRequestGridAlex", "Catalog");

    if (simpleStat == "none") {
        idFilter = $('#IDSearchA').val();
    }
    else {
        idFilter = $('#IDSearchS').val();
    }

    $.ajax({
        url: link,
        cache: false,
        data: { FilterText: rFilter, IDFilter: idFilter, RequestorFilter: r2Filter, Status: status, StartDate: sDate, EndDate: eDate, simpleStatus: simpleStat, Source: 'MyRequestsTab' },
        success: function (result) {
            $('#MyRequest').html(result);

            $("#displayheading").hide();
            $("#togglebutton").text('Show Column Filters');
        }
    });

}
function LoadMyRequest() {

    var searchBy = $("#ddlSearchBy").val();
    var searchValue = $('#txtSearchValue').val();
    var status = $('#ddlStatus').val();
    var sDate = $("#dtStart").val();
    var eDate = $("#dtEnd").val();
    var link = getActionURL("MyRequestGrid", "Catalog");

    var isSimple = $("#simpleSearch").is(":visible");
    if (isSimple) {
        searchValue = $('#txtRequestID').val();
    }

    $.ajax({
        url: link,
        cache: false,
        data: { IsSimple: isSimple, SearchBy: searchBy, SearchValue: searchValue, Status: status, StartDate: sDate, EndDate: eDate, Source: 'MyRequestsTab'},
        success: function (result) {
            $('#MyRequest').html(result);

            $("#displayheading").hide();
            $("#togglebutton").text('Show Column Filters');
        }
    });
}


//functions for MyRequestGrid

var Tablesearch = $('#allreq_datatable').DataTable();
Tablesearch.columns().every(function () {

    var that = this;
    var buttonvalue = $("genrebutton").val();

    $('input', this.footer()).on('keyup change', function () {
        if (that.search() !== this.value) {
            that
                .search(this.value)

                .draw();
        }
    });
});


//functions for master request layout

function ShowHideRequestDetails() {
    if ($("#RequestValues").is(":visible")) {
        $("#RequestValues").hide("slow");
        // $('#MainForm').removeClass('col-lg-offset-2',100).addClass('col-lg-offset-3',100);
        $("#MainForm").switchClass("col-lg-offset-2", "col-lg-offset-3");
    }
    else {
        $("#RequestValues").show("slow");
        // $('#MainForm').removeClass('col-lg-offset-3',500).addClass('col-lg-offset-2',500);
        $("#MainForm").switchClass("col-lg-offset-3", "col-lg-offset-2");
    }


    //$("#RequestValues").toggle("slow");
    //$("#MainForm").switchClass("col-lg-offset-3", "col-lg-offset-2");


}

function showSubmittingSpinner() {
    //force spinner to show

    $('#loadingModalText').text('Submitting...')
    $('body').addClass("loading");
}

function hideSubmittingSpinner() {
    //force spinner to show

    $('#loadingModalText').text('')
    $('body').removeClass("loading");
}

function ShowHideWizardButtons() {
    //hide the submit button by default
    $("#wizardSubmitBtn").hide('fast');

    //when any wizard button is clicked, check if we're on the confirmation page. If so, show submit and hide next. If not, hide submit and show next.
    $(".wizardBtn").click(function () {

        setTimeout(
            function () {
                if ($("#finalStep").is(":visible")) {
                    $("#wizardNextBtn").hide('fast');
                    $("#wizardSubmitBtn").show('slow');

                }
                else {

                    $("#wizardNextBtn").show('slow');
                    $("#wizardSubmitBtn").hide('fast');
                }
            }, 500);



    });

    $(document).mousemove(function () {

        setTimeout(
            function () {
                if ($("#finalStep").is(":visible")) {
                    $("#wizardNextBtn").hide('fast');
                    $("#wizardSubmitBtn").show('slow');

                }
                else {

                    $("#wizardNextBtn").show('slow');
                    $("#wizardSubmitBtn").hide('fast');
                }
            }, 500);



    });


}

var ft;
function CreateConfirmationPage() {


    //when any wizard button is clicked, check if we're on the confirmation page. If so, show generate the confirmation page.
    $(".wizardBtn").click(function () {

        //get input form object
        var form = $('.tab-pane');
        //var form = $('#InputForm');

        var Temp;
        var oldheading = "";
        //Start table body
        $("#confirmationTable").html("<tbody>");

        for (var i = 0; i < form.length - 1; i++) {

            var tpID = form[i].getAttribute("id").split('-')[1].replace("step", "");
            var tabVisible = $("#navtab-" + tpID).is(":visible");

            var k = i;

            if (tabVisible) {
                //get elements with DisplayConfirmation data set to true
                $(form[i]).find('input, select, textarea, hidden').each(function () {

                    Temp = $(this);

                    var displayheading = true;

                    //added $(this).is(":visible") to only show visible items
                    if ($(this).attr("data-displayconfirmation") == "true")// && $(this).is(":visible"))
                    {
                        //get the value
                        var currentvalue = "";
                        var label;
                        var sum = 0;

                        if ($(this).is('input') || $(this).is('text') || $(this).is('textarea')) {

                            if ($(this).val() === "") {
                                displayheading = false;
                            }
                            else {
                                currentvalue = $(this).val();

                            }

                        }

                        //else if ($(this).is('text')) {

                        //    if ($(this).val() === "") {
                        //        displayheading = false;
                        //    }
                        //    else {
                        //        currentvalue = $(this).val();

                        //    }
                        //}
                        else if ($(this).is('hidden')) {

                            if (($(this).val() === "no")) {
                                displayheading = false;
                            }
                            else {
                                currentvalue = $(this).val();
                            }

                        }
                        else {
                            if ($(this).find('option:selected').text() === "") {
                                displayheading = false;
                            }
                            else {
                                //currentvalue =  $(this).find('option:selected').text();
                                displayheading = true;
                                //if ($(this).find('option:selected').length == null) //means that it’s single select
                                //{
                                //    currentvalue = $(this).find('option:selected').text();
                                //}
                                //else it’s a multi-select


                                for (var index = 0; index < $(this).find('option:selected').length; index++) {
                                    currentvalue = currentvalue + $(this).find('option:selected')[index].text + "<br>";
                                }


                            }
                        }


                        //find the label for this field
                        labels = $(form[i]).find('label');

                        elementName = $(this).attr('name');

                        $(labels).each(function () {
                            if ($(this).attr("for") == elementName) {
                                label = $(this).text();

                            }
                        })

                        if (displayheading === true) {

                            if (oldheading === "" || oldheading != form[i].getAttribute("tabdisplayname")) {
                                $("#confirmationTable").append("<tr><th class='text-primary' colspan = 2><h4><b>" + form[i].getAttribute("tabdisplayname") + "</tr></th></h4></b>");
                                oldheading = form[i].getAttribute("tabdisplayname");
                            }
                            $("#confirmationTable").append("<tr><td><b>" + label + "</b></td><td>" + currentvalue + "</td></tr>");
                        }


                    }


                })
            }
        }
        //finish table body
        $("#confirmationTable").append("</tbody>");
    });
}

function ConfigureAndShowWizardModal(modalType, titleText, contentHtml, greenButtonText, redButtonText, showGreenButton, showRedButton, redButtonOnClick, greenButtonOnClick, redButtonHref, greenButtonHref) {


    //get modal object from DOM
    modal = $('#wizardModal');

    //set modal header color
    //modalType can be error, success. Anything else defaults to dark blue notification color.
    $('#wizardModalHeader').removeClass();
    if (modalType == "error") {
        $('#wizardModalHeader').addClass("block-header bg-danger");
    }
    else if (modalType == "success") {

        $('#wizardModalHeader').addClass("block-header bg-success");
    }
    else {
        $('#wizardModalHeader').addClass("block-header bg-primary-dark");
    }


    //set wizard modal title text
    $('#wizardModalTitleText').text(titleText);

    //set buttons

    if (showRedButton) {
        $('#wizardModalRedBtn').text(redButtonText);
        $('#wizardModalRedBtn').show('fast');
        $('#wizardModalRedBtn').removeAttr("onclick");
        $('#wizardModalRedBtn').removeAttr("href");
        if (redButtonOnClick != "") {
            $('#wizardModalRedBtn').removeAttr("data-dismiss");
            $('#wizardModalRedBtn').attr("onclick", redButtonOnClick);
        }

        if (redButtonHref != "") {
            $('#wizardModalRedBtn').removeAttr("data-dismiss");
            $('#wizardModalRedBtn').attr("href", redButtonHref);
        }



    }
    else {
        $('#wizardModalRedBtn').hide('fast');
    }

    if (showGreenButton) {
        $('#wizardModalGreenBtn').text(greenButtonText);
        $('#wizardModalGreenBtn').show('fast');
        $('#wizardModalGreenBtn').removeAttr("onclick");
        $('#wizardModalGreenBtn').removeAttr("href");

        if (greenButtonOnClick != "" && greenButtonOnClick != null) {
            $('#wizardModalGreenBtn').removeAttr("data-dismiss");
            $('#wizardModalGreenBtn').attr("onclick", greenButtonOnClick);
        }

        if (greenButtonHref != "" && greenButtonHref != null) {
            $('#wizardModalGreenBtn').attr("href", greenButtonHref);
            $('#wizardModalGreenBtn').removeAttr("data-dismiss");
        }

        //if(greenButtonOnClick == "" || greenButtonOnClick == null)
        //{
        //    $('#wizardModalGreenBtn').attr("data-dismiss");
        //    $('#wizardModalGreenBtn').removeAttr("onclick",greenButtonOnClick);
        //}


    }
    else {
        $('#wizardModalGreenBtn').hide('fast');
    }


    //set content html

    $('#wizardModalContent').html(contentHtml);




    $(modal).modal({
        backdrop: 'static',
        keyboard: false
    })
    $(modal).modal('show');



}


function ConfigureAndShowFullScreenWizardModal(modalType, titleText, contentHtml, greenButtonText, redButtonText, showGreenButton, showRedButton, redButtonOnClick, greenButtonOnClick, redButtonHref, greenButtonHref) {


    //get modal object from DOM
    modal = $('#fullScreenWizardModal');

    //set modal header color
    //modalType can be error, success. Anything else defaults to dark blue notification color.
    $('#fullScreenWizardModalHeader').removeClass();
    if (modalType == "error") {
        $('#fullScreenWizardModalHeader').addClass("modal-header block-header bg-danger");
    }
    else if (modalType == "success") {

        $('#fullScreenWizardModalHeader').addClass("modal-header block-header bg-success");
    }
    else {
        $('#fullScreenWizardModalHeader').addClass("modal-header block-header bg-primary-dark");
    }


    //set wizard modal title text
    $('#fullScreenWizardModalTitleText').text(titleText);

    //set buttons

    if (showRedButton) {
        $('#fullScreenWizardModalRedBtn').text(redButtonText);
        $('#fullScreenWizardModalRedBtn').show('fast');
        $('#fullScreenWizardModalRedBtn').removeAttr("onclick");
        $('#fullScreenWizardModalRedBtn').removeAttr("href");
        if (redButtonOnClick != "") {
            $('#fullScreenWizardModalRedBtn').removeAttr("data-dismiss");
            $('#fullScreenWizardModalRedBtn').attr("onclick", redButtonOnClick);
        }

        if (redButtonHref != "") {
            $('#fullScreenWizardModalRedBtn').removeAttr("data-dismiss");
            $('#fullScreenWizardModalRedBtn').attr("href", redButtonHref);
        }



    }
    else {
        $('#fullScreenWizardModalRedBtn').hide('fast');
    }

    if (showGreenButton) {
        $('#fullScreenWizardModalGreenBtn').text(greenButtonText);
        $('#fullScreenWizardModalGreenBtn').show('fast');
        $('#fullScreenWizardModalGreenBtn').removeAttr("onclick");
        $('#fullScreenWizardModalGreenBtn').removeAttr("href");

        if (greenButtonOnClick != "" && greenButtonOnClick != null) {
            $('#fullScreenWizardModalGreenBtn').removeAttr("data-dismiss");
            $('#fullScreenWizardModalGreenBtn').attr("onclick", greenButtonOnClick);
        }

        if (greenButtonHref != "" && greenButtonHred != null) {
            $('#fullScreenWizardModalGreenBtn').attr("href", greenButtonHref);
            $('#fullScreenWizardModalGreenBtn').removeAttr("data-dismiss");
        }

        //if(greenButtonOnClick == "" || greenButtonOnClick == null)
        //{
        //    $('#wizardModalGreenBtn').attr("data-dismiss");
        //    $('#wizardModalGreenBtn').removeAttr("onclick",greenButtonOnClick);
        //}


    }
    else {
        $('#fullScreenWizardModalGreenBtn').hide('fast');
    }


    //set content html

    $('#fullScreenWizardModalBody').html(contentHtml);




    $(modal).modal({
        backdrop: 'static',
        keyboard: false
    })
    $(modal).modal('show');



}

//functions for standard & manage layouts
function loadPartialView(pageName, section) {

    var link = getActionURL("REPLACEME", "Manage");
    link = link.replace("REPLACEME", pageName);


    $.ajax({
        url: link,
        cache: false,
        success: function (result) {
            $('#MainBody').html(result);
            $('.topleveltab').removeClass('active');
            $('.refreshpageicon').hide();
            $('#' + section).addClass('active')
            $('#' + section).children('a').children('.refreshpageicon').show('slow');
            $('#MasterSearchBox').fadeIn('200');

        }
    });
}

function ShowContent(ObjectID, InitDirection, FinalDirection, Action, Controller) {
    $('#divGrid').hide('slide', { direction: InitDirection }, 500).promise().done(function () {
        var NavUrl = getActionURL("Action", "Controller");
        NavUrl = NavUrl.replace('Action', Action).replace('Controller', Controller);


        if (ObjectID != null) {


            $.ajax({
                type: "POST",
                url: NavUrl,
                data: $.param({ "id": ObjectID }),
                success: function (result) {
                    $("#divGrid").html(result);
                },
                failure: function (result) {
                    alert(result);
                }
            });
        }
        else {
            $.ajax({
                type: "POST",
                url: NavUrl,
                success: function (result) {

                    $("#divGrid").html(result);
                },
                failure: function (result) {
                    alert(result);
                }
            });
        }

        $('#divGrid').show('slide', { direction: FinalDirection }, 500);
    });
}

function highlightMenuTab(section) {
    $('#' + section).addClass('active')
    $('#' + section).children('a').children('.refreshpageicon').show('slow');
    //$('#MasterSearchBox').fadeIn('200');
}


function loadForm(id) {

    var link = getActionURL("GetForm", "Request");

    $.ajax({
        url: link,
        //async: false,
        data: { id: id },
        cache: false,
        success: function (result) {
            $('#MainBody').html(result);
        }
    });
}

function loadResponseForm(fID, rID, tID) {

    var link = getActionURL("GetResponseForm", "Request");

    $.ajax({
        url: link,
        //async: false,
        data: { id: fID, RequestID: rID, TaskID: tID },
        cache: false,
        success: function (result) {
            $('#MainBody').html(result);
        }
    });
}

function loadResponseFormCollection(fID, rID, tID) {

    var link = getActionURL("GetResponseFormCollection", "Request");

    $.ajax({
        url: link,
        //async: false,
        data: { id: fID, RequestID: rID, TaskID: tID },
        cache: false,
        success: function (result) {
            $('#MainBody').html(result);
        }
    });
}


function loadExternalNavigation(desc, url, owner, contact) {
    var contentHTML = "<div class='text-left'>";
    contentHTML += "<p>The request, <b>" + desc + "</b>, exists on a different site.  Click the 'Go' below to contact or 'Nevermind' to cancel.</p>";
    contentHTML += "<p>Owner Info: <b>" + owner + "</b><br/>";
    contentHTML += "Owner Contact: <b>" + contact + "</b></p > "
    contentHTML += "</div>"

    ConfigureAndShowGlobalModal("info", "External Site Warning", contentHTML, true, "Go", "navigateToExternalSite('" + url + "');", "", true, "Nevermind", "", "");
}

function toggleView(tr) {

    $('#' + tr).toggle('normal');

}

//gets a list of active server hostnames, for SID search
function getAllActiveServersList() {
    var link = getActionURL("SearchBox_SIDObjectSearch", "Home");
    $.ajax({
        url: link,
        cache: false,
        success: function (result) {
            $('#SIDServerSearch_Placeholder').html(result);
        }

    });


}


//functions for  _layout.cshtml
function LoadUserDetails() {

    var link = getActionURL("GetUserDetails", "Home");
    $.ajax({
        url: link,
        cache: false,
        success: function (result) {
            $("#UserImageUrl").attr('src', result.ImageUrl);
            $("#UserFullName").html(result.UserFullName);
            $("#UserTaskCount").html(result.TaskCount);
            $("#UserRequestCount").html(result.RequestCount);
        }

    });


}

function LoadSiteValues() {

    var link = getActionURL("GetSiteValues", "Home");

    $.ajax({
        url: link,
        cache: false,
        success: function (result) {

            $("#CompileDate").html(result);

        }

    });


}



function getUrlParameter(name) {
    var results = new RegExp('[\\?&]' + name + '=([^&#]*)').exec(window.location.href);
    if (results == null) {
        return null;
    }
    else {
        return results[1] || 0;
    }
}

function getActionURL(action, controller) {
    //var s ='@Url.Action("GetClusterOptions", "VM")'

    var s = $("#getUrlAction").val();
    s = s.replace("Action", action);
    s = s.replace("Controller", controller);

    return s;
}

function loadingSpinner() {
    body = $("body");

    $(document).on({
        ajaxStart: function () {
            if ($('#loadingModalText').text() == "") {
                $('#loadingModalText').text('Loading...')
            }
            body.addClass("loading");
        },
        ajaxStop: function () {

            body.removeClass("loading");
            $('#loadingModalText').text('')
        }
    });
}

function showLoadingSpinner(message) {
    $('#loadingModalText').text(message + '...')
    $('body').addClass("loading");
}

function hideLoadingSpinner(){
    $('#loadingModalText').text('')
    $('body').removeClass("loading");
}

function ConfigureAndShowGlobalModal(modalType, titleText, contentHtml, showGreenButton, greenButtonText, greenButtonOnClick, greenButtonHref, showRedButton, redButtonText, redButtonOnClick, redButtonHref, wideModal) {


    //get modal object from DOM
    modal = $('#globalModal');

    //set modal header color
    //modalType can be error, success. Anything else defaults to dark blue notification color.
    $('#globalModalHeader').removeClass();
    if (modalType == "error") {
        $('#globalModalHeader').addClass("block-header bg-danger");
    }
    else if (modalType == "success") {

        $('#globalModalHeader').addClass("block-header bg-success");
    }
    else {
        $('#globalModalHeader').addClass("block-header bg-primary-dark");
    }


    //set wizard modal title text
    $('#globalModalTitleText').text(titleText);

    //set buttons

    if (showRedButton) {
        $('#globalModalRedBtn').text(redButtonText);
        $('#globalModalRedBtn').show('fast');
        $('#globalModalRedBtn').removeAttr("onclick");
        $('#globalModalRedBtn').removeAttr("href");
        if (redButtonOnClick != "") {
            $('#globalModalRedBtn').removeAttr("data-dismiss");
            $('#globalModalRedBtn').attr("onclick", redButtonOnClick);
        }

        if (redButtonHref != "") {
            $('#globalModalRedBtn').removeAttr("data-dismiss");
            $('#globalModalRedBtn').attr("href", redButtonHref);
        }



    }
    else {
        $('#globalModalRedBtn').hide('fast');
    }

    if (showGreenButton) {
        $('#globalModalGreenBtn').text(greenButtonText);
        $('#globalModalGreenBtn').show('fast');
        $('#globalModalGreenBtn').removeAttr("onclick");
        $('#globalModalGreenBtn').removeAttr("href");

        if (greenButtonOnClick != "" && greenButtonOnClick != null) {
            $('#globalModalGreenBtn').removeAttr("data-dismiss");
            $('#globalModalGreenBtn').attr("onclick", greenButtonOnClick);
        }

        if (greenButtonHref != "" && greenButtonHref != null) {
            $('#globalModalGreenBtn').attr("href", greenButtonHref);
            $('#globalModalGreenBtn').removeAttr("data-dismiss");
        }

        //if(greenButtonOnClick == "" || greenButtonOnClick == null)
        //{
        //    $('#globalModalGreenBtn').attr("data-dismiss");
        //    $('#globalModalGreenBtn').removeAttr("onclick",greenButtonOnClick);
        //}


    }
    else {
        $('#globalModalGreenBtn').hide('fast');
    }


    //set content html

    $('#globalModalContent').html(contentHtml);


    $(modal).modal({
        backdrop: 'static',
        keyboard: false
    });


    $(modal).modal('show');



}

function CloseGlobalModal() {
    var modal = $('#globalModal');
    $(modal).modal('hide');
}

function navigateToExternalSite(url) {
    window.open(url, "_blank");
    $('#globalModal').modal('toggle');
}

function fakeAjax() {


    $.ajax({
        url: '@Url.Action("FakeAjax", "Home")',
        cache: false,
        success: function (result) {

        }

    });



}

function UpdateListSelectBox(tb, selectlistoption) {
    tb.val('');
    selectlistoption.each(
        function () {
            // var tb = tb;
            if (tb.val() == "") {
                tb.val(tb.val() + this.value);
            }
            else {
                tb.val(tb.val() + ',' + this.value);
            }
        });
}

function UpdateListSelectDisplay(tb, selectlistoption) {
    tb.val('');
    selectlistoption.each(
        function () {
            // var tb = tb;
            if (tb.val() == "") {
                tb.val(tb.val() + this.value);
            }
            else {
                tb.val(tb.val() + '<br/>' + this.value);
            }
        });
}

function moveAllItemsOver(source, target) {
    target.append(source.find("option"));
    SortSelectList(target);
}

function moveItemOver(source, target) {

    target.append(source.find(":selected"));
    SortSelectList(target);
}

function SortSelectList(selectList) {

    var selectOptions = selectList.find("option")


    selectOptions.sort(function (a, b) {
        if (a.text > b.text) {
            return 1;
        }
        else if (a.text < b.text) {
            return -1;
        }
        else {
            return 0
        }
    });


    selectList.empty().append(selectOptions);
}

//Beta feedback function
function sendFeedback() {

    var user = "@User.Identity.Name";
    var message = $('#BetaFeedbackMessage').val();
    var feedbacktype = $('#BetaFeedbackType').val();

    $.ajax({
        url: '@Url.Action("SendBetaFeedback", "Home")',
        cache: false,
        data: { FeedbackType: feedbacktype, Message: message, User: user },
        success: function (data) {


            if (data == "success") {
                $('#BetaFeedbackMessage').val('');
                $('#BetaFeedbackType').val('');
                $('#BetaFeedbackSent').modal('show');
            }
            else {
                var subject = "Manual Beta Feedback. Type: " + feedbacktype + ". From " + user;
                $('#ManualSendFeedback').attr('href', "mailto:ESEAutomation@obs.org?subject=" + subject + "&body=" + message)


                $('#BetaFeedbackFailed').modal('show');
            }



        },
        error: function () {
            $('#BetaFeedbackFailed').modal('show');



        }

    });
}
//MasterRequestView.cshtml


function SubmitTheForms(id) {

    var form = $('#InputForm');

    $.ajax({
        type: "POST",
        //async: false,
        url: getActionURL("InsertFormValuesToDB", "Request"),
        data: form.serialize() + "&" + $.param({ "RequestID": id }),

        success: function (data) {

            InitiateFormControllerAction(id);


        },
        failure: function (data) {
            errorHTML = "<p>There was an error submitting your request during the 'SubmitTheForms' function.</p>";
            errorHTML += "<p>Please try filling the form out again</p>";
            errorHTML += "<p>If the problem persists, please <a href='#'>click here</a> to submit a heat ticket to the ESE Automation team</p>";
            errorHTML += "<br/>";
            errorHTML += "Error data:" + data;

            ConfigureAndShowWizardModal("error", "Error submitting request", errorHTML, "OK", "", true, false);
        }
    });
}

function SubmitResponseForms(id, TaskID) {

    var form = $('#InputForm');


    $.ajax({
        type: "POST",
        //async: false,
        url: getActionURL("InsertFormValuesToDB", "Request"),
        data: form.serialize() + "&" + $.param({ "RequestID": id }),

        success: function (data) {
            InitiateResponseFormControllerAction(TaskID, id);

        },
        failure: function (data) {
            errorHTML = "<p>There was an error submitting your request during the 'SubmitTheForms' function.</p>";
            errorHTML += "<p>Please try filling the form out again</p>";
            errorHTML += "<p>If the problem persists, please <a href='#'>click here</a> to submit a heat ticket to the ESE Automation team</p>";
            errorHTML += "<br/>";
            errorHTML += "Error data:" + data;

            ConfigureAndShowWizardModal("error", "Error submitting request", errorHTML, "OK", "", true, false);
        }
    });
}

function InitiateFormControllerAction(id) {



    $.ajax({
        type: "POST",
        async: false,
        url: getActionURL("InitiateCPOAction", "Request"),
        data: { "RequestID": id },

        success: function (data) {
            successHTML = "<h2>Your request has been successfully submitted!</h2>";
            successHTML += "<br/>";
            successHTML += "<h3>Your Request ID is: " + id + "</h3>";
            successHTML += "<br/>";
            successHTML += "<p>You can <a href='" + getActionURL("RequestDetails", "Catalog") + "/" + id + "'>click here</a> to see the progress of your request</p>";
            //successHTML += "<p>You will also receive an E-mail with this information.</p>";


            //ConfigureAndShowWizardModal("success","Request Submitted Successfully",successHTML,"OK","",true,false,"","loadCatalog('wizardModal')","","");

            var NavHome = "window.location.href = '" + getActionURL("Index", "Catalog") + "';";
            ConfigureAndShowWizardModal("success", "Request Submitted Successfully", successHTML, "OK", "", true, false, "", NavHome, "", "");

        },
        failure: function (data) {
            errorHTML = "<p>There was an error submitting your request during the 'InitiateFormControllerAction' function.</p>";
            errorHTML += "<p>Your data has been stored, but the process has not been initiated</p>";
            errorHTML += "<p>Please <a href='#'>click here</a> to submit a heat ticket to the ESE Automation team</p>";
            errorHTML += "<br/>";
            errorHTML += "Error data:" + data;

            ConfigureAndShowWizardModal("error", "Error submitting request", errorHTML, "OK", "", true, false);
        }
    });
}

//FormType.cshtml

function ProcessFormTypeForm() {
    var v = $("#frmEditor").kendoValidator().data("kendoValidator");

    var isValid = v.validate();
    if (isValid) {
        $.ajax({
            type: "POST",
            url: getActionURL("ProcessFormTypeModel", "Manage"),
            data: $("#frmEditor").serialize(), // serializes the form's elements.
            success: function (result) {
                CloseFormTypeEditor();
            },
            failure: function (result) {
                alert(result);
            }
        })
    }
}

function DeleteFormType(id) {

    $.ajax({
        type: "POST",
        url: getActionURL("DeleteFormTypeModel", "Manage"),
        data: $.param({ "id": id }), // serializes the form's elements.
        success: function (result) {

            RefreshFormTypeGrid();
        },
        failure: function (result) {
            alert(result);
        }
    });

}

//requestdetails.cshtml
function autoRefreshRequestDetails(seconds) {
    setInterval(function () {
        RefreshData()
    }, seconds * 1000); // 60 * 1000 milsec

}


//User Reports
function ShowSaveReportModal() {
    var cat = $("#ddlCategory option:selected").text();
    var fc = $("#ddlFormCollection option:selected").text();

    var blockName = cat + " - " + fc;

    $('.reports-block-name').text(blockName);
    $("#reportname").val('');

    var link = getActionURL("GetUserReportsDDL", "Catalog");
    $.ajax({
        url: link,
        cache: false,
        async: true,
        success: function (result) {
            $('#reportid').html(result);
        }
    });

    $('#report-modal').modal('show');
}


function LoadUserReportsGrid() {
    var link = getActionURL("MyReportsGrid", "Catalog");
    $.ajax({
        url: link,
        cache: false,

        success: function (result) {
            $('#MyReports').html(result);

            $("#displayheading").hide();
            $("#togglebutton").text('Show Column Filters');
        }
    });
}

var Temp;
function LoadUserReport(id) {
    var link = getActionURL("GetUserReportDetails", "Catalog");
    $.ajax({
        url: link,
        type: "POST",
        cache: false,
        data: { ReportID: id },
        success: function (result) {
            Temp = result;

            $("#divOverview").fadeOut("fast");
            $("#divMyReports").fadeOut("fast");
            $("#divReportFilter").fadeIn("fast");
            $("#divReporting").fadeIn("fast");
            $("#reqFilter").fadeIn("fast");
            $("#divReportBuilder").fadeIn("fast");

            $("#btnOverview").removeClass("btn-primary");
            $("#btnOverview").addClass("btn-default");

            $("#btnDetailed").removeClass("btn-default");
            $("#btnDetailed").addClass("btn-primary");

            //$('#ddlCategory option').filter(function () { return $.trim($(this).text()) == result.data.Category; }).attr('selected', 'selected').change();
            //$('#ddlFormCollection option').filter(function () { return $.trim($(this).text()) == result.data.FormCollection; }).attr('selected', 'selected').change();

            $("#ddlCategory").val(result.data.CategoryID).change();
            $("#ddlFormCollection").val(result.data.FormCollectionName).change();

            var startdateString = result.data.StartDate.substr(6);
            var startTime = new Date(parseInt(startdateString));
            var startDate = (startTime.getMonth() + 1) + "/" + startTime.getDate() + "/" + startTime.getFullYear();
            $("#dtStart").val(startDate);

            var enddateString = result.data.EndDate.substr(6);
            var endTime = new Date(parseInt(enddateString));
            var endDate = (endTime.getMonth() + 1) + "/" + endTime.getDate() + "/" + endTime.getFullYear();
            $("#dtEnd").val(endDate);
            

            setFieldValue("ddlColumns", "");
            $('#ddlFormCollection').change();
            setFieldValue("ddlColumns", result.data.SelectedColumns);
            ShowDetailedReport();


        }
    });
}
function setReportSave(value) {
    //$("input[name=reportinputtype][value=" + value + "]").attr('checked', 'checked');

    if (value == "new") {
        $("#div_reportname").show("fast");
        $("#div_reportid").hide("fast");
    }
    else
    {
        $("#div_reportname").hide("fast");
        $("#div_reportid").show("fast");
    }
}
function SaveUserReport() {
    var reportname = $("#reportname").val();

    //var cat = $("#ddlCategory option:selected").text();
    var input = $("input:radio[name ='reportinputtype']:checked").val();

    
    var rID = $("#reportid").val();
    var cat = $("#ddlCategory").val();
    var fc = $("#ddlFormCollection").val();
    var sDate = $("#dtStart").val();
    var eDate = $("#dtEnd").val();
    var cols = $("#ddlColumns").val().toString();

    var cont = true;
    if (input == "update") {
        reportname = $("#reportid option:selected").text();
        if (rID == null || rID === "") {
            cont = false;
            var body = "Please select an Existing Report to Update.";
            ConfigureAndShowGlobalModal("error", "Existing Report Missing", body, true, "OK", "", "", false, "", "", "", false);

        }
    }
    else {
        if (reportname == null || reportname === "") {
            cont = false;
            var body = "Please provide name for the Report";
            ConfigureAndShowGlobalModal("error", "Report Name Missing", body, true, "OK", "", "", false, "", "", "", false);

        }
    }






    if (cont) {

        $.ajax({
            url: getActionURL("ProcessUserReportModel", "Catalog"),
            type: "POST",
            data: { ReportID: rID, ReportName: reportname, CategoryID: cat, FormCollectionName: fc, StartDate: sDate, EndDate: eDate, SelectedColumns: cols, InputType: input },
            async: true,
            cache: false,

            success: function (result) {
                $('#report-modal').modal('hide');
                var body = "Your report was successfully saved.  <br><br>You can manage this and other reports by expanding the 'My Reports' Section of this page.";
                ConfigureAndShowGlobalModal("success", "Report Saved : " + reportname, body, true, "OK", "", "", false, "", "", "", false);
                LoadUserReportsGrid();
            }
        });
    }
}

function UpdateUserReport() {
    var reportname = $("#reportname").val();

    var cat = $("#Category").text();
    var fc = $("#FormCollection").val();
    var sDate = $("#dtStart").val();
    var eDate = $("#dtEnd").val();
    var cols = $("#ddlColumns").val();


    $.ajax({
        url: getActionURL("ProcessUserReportModel", "Catalog"),
        type: "POST",
        data: { ReportName: reportname, Category: cat, FormCollection: fc, StartDate: sDate, EndDate: eDate, SelectedColumns: cols },
        async: true,
        cache: false,

        success: function (result) {
            $('#report-modal').modal('hide');
            var body = "Your report was successfully saved.  <br><br>You can manage this and other reports under the <a href=' " + getActionURL("MyReports", "Catalog") + "'><i class='fa fa-file-text'></i> My Reports </a>";
            ConfigureAndShowGlobalModal("success", "Report Saved : " + reportname, body, true, "OK", "", "", false, "", "", "", false);
        }
    });
}

function DeleteUserReport(id) {

    $.ajax({
        type: "POST",
        url: getActionURL("DeleteUserReportModel", "Catalog"),
        data: $.param({ "id": id }), // serializes the form's elements.
        success: function (result) {

            LoadUserReportsGrid();
        },
        failure: function (result) {
            alert(result);
        }
    });

}

