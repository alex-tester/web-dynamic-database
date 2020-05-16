// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function initValidation() {
    jQuery('.js-validation').validate({
        ignore: [],
        errorClass: 'invalid-feedback animated fadeIn text-right',
        errorElement: 'div',
        errorPlacement: function (error, el) {
            jQuery(el).addClass('is-invalid');
            jQuery(el).parents('.form-group').append(error);
        },
        highlight: function (el) {
            jQuery(el).parents('.form-group').find('.is-invalid').removeClass('is-invalid').addClass('is-invalid');
        },
        success: function (el) {
            jQuery(el).parents('.form-group').find('.is-invalid').removeClass('is-invalid');
            jQuery(el).remove();
        }
    });


}
function initValidationHorizontal() {
    jQuery('.js-validation').validate({
        ignore: [],
        errorClass: 'invalid-feedback animated fadeIn text-right offset-sm-3 col-sm-7',
        errorElement: 'div',
        errorPlacement: function (error, el) {
            jQuery(el).addClass('is-invalid');
            jQuery(el).parents('.form-group').append(error);
        },
        highlight: function (el) {
            jQuery(el).parents('.form-group').find('.is-invalid').removeClass('is-invalid').addClass('is-invalid');
        },
        success: function (el) {
            jQuery(el).parents('.form-group').find('.is-invalid').removeClass('is-invalid');
            jQuery(el).remove();
        }
    });

}


function ClosePopupEditor() {
    $('#globalContentModal').modal('hide');
}

function ShowPopUpDelete(viewName, controller, objectID, objectName, reloadViewName, UseId, reloadObjectID) {
    swal({
        title: 'Are you sure?',
        text: 'Do you really want to delete <b> ' + objectName + '</b>?',
        type: 'question',
        showCancelButton: true,
        confirmButtonClass: 'btn btn-outline-danger m-1',
        cancelButtonClass: 'btn btn-outline-secondary m-1',
        confirmButtonText: 'Yes, delete it!',
        html: 'Do you really want to delete <br><br><b> ' + objectName + '</b>?<br><br>',
        preConfirm: function (e) {
            return new Promise(function (resolve) {
                setTimeout(function () {
                    resolve();
                }, 50);
            });
        }
    }).then(function (result) {
        //debugger;
        if (UseId == true) {
            var pData = { id: objectID, ParentId: reloadObjectID };
        }
        else {
            var pData = { id: objectID };
        }

        if (result.value) {
            var url = getActionURL(viewName, controller);
            $.ajax({
                url: url,
                type: 'POST',
                data: pData,
                success: function (result) {
                    if (result.failure) {
                        ShowPopUpUserError(result.message);
                    }
                    else if (result === false) { //default message if we havent defined the failure reason

                        ShowPopUpUserError('Something went wrong when attempting to delete. Please contact the site admin for assistance.');
                    }
                    else {
                        LoadPageBody(reloadViewName, controller, UseId, reloadObjectID);
                        ShowPopUpSuccessWithTimer('Deleted!', '<b>' + objectName + '</b> has been deleted.');
                        //swal('Deleted!', '<b>' + objectName + '</b> has been deleted.', 'success');

                        
                    }
                },
                failure: function (result) {
                }
            });

            // result.dismiss can be 'overlay', 'cancel', 'close', 'esc', 'timer'
        } else if (result.dismiss === 'cancel') {
            //swal('Cancelled', 'Your imaginary file is safe :)', 'error');
        }
    });
}

function ShowPopUpUserError(htmlMessage) {
    swal({
        title: 'Failed!',
        type: 'error',
        html: "<b>" + htmlMessage + "</b>"
    });
}


function ShowPopUpEditor(viewName, controller, objectID, objectType, parentObjectID, saveMethod) {
    //get modal object from DOM
    modal = $('#globalContentModal');

    $('#contentModalTitleText').text(objectType);

    $('#saveBtnModelContent').attr("onclick", saveMethod)

    var url = getActionURL(viewName, controller);
    url = url + "/" + objectID + '?ParentID=' + parentObjectID;

    $('#contentModalContent').load(url);


    $(modal).modal({
        backdrop: 'static',
        keyboard: false
    });



    $(modal).modal('show');
}

function ShowPopUpEditorDynamic(viewName, controller, objectID, objectType, ProcessMethod, ProcessController, ReloadAction, ReloadController, useId, Id) {
    //get modal object from DOM
    modal = $('#globalContentModal');

    $('#contentModalTitleText').text(objectType);

    //debugger;
    $('#saveBtnModelContent').attr("onclick", "ProcessForm('" + ProcessMethod + "','" + ProcessController + "','" + ReloadAction + "','" + ReloadController + "','" + useId + "','" + Id + "')");
    var url = getActionURL(viewName, controller);
    url = url + "/" + objectID
    if (useId) {
        url = url + "?ParentId=" + Id;
    }

    $('#contentModalContent').load(url);

   
    $(modal).modal({
        backdrop: 'static',
        keyboard: false
    });


    
    $(modal).modal('show');
}

function ProcessForm(Action, Controller, ReloadAction, ReloadController, UseId, Id) {
    
    if ($("#frmEditor").valid()) {

        var url = getActionURL(Action, Controller);
        var fData = $("#frmEditor").serialize();

        $("#frmEditor").submit(function (e) {
            e.preventDefault();
        });


        $.ajax({

            type: 'POST',
            url: url,
            data: fData,
            success: function (result) {
                //debugger;
                if (!result.failure) {
                    ClosePopupEditor();
                    
                    if (UseId) {
                        if (Id == 'new') {
                            var newId = result.id;
                            LoadPageBody(ReloadAction, ReloadController, true, newId);
                        }
                        else {
                            LoadPageBody(ReloadAction, ReloadController, UseId, Id);
                        }

                    }
                    else {
                        LoadPageBody(ReloadAction, ReloadController, false, Id);
                    }
                    ShowPopUpSuccessWithTimer("Success!", "Your changes have been saved.");
                }
                else {
                    //swal.close();
                    ShowPopUpUserError(result.message);
                }
                
                
            },
            fail: function (data) {
                alert("fail");
            }
        });
    }

}

function ShowPopUpSuccessWithTimer(title, htmlMessage) {
    Swal.fire({
        //position: 'top-end',
        type: 'success',
        title: title,
        html: "<b>" + htmlMessage + "</b",
        showConfirmButton: false,
        timer: 2000
    });
}

function LoadPageBody(LoadAction, LoadController, UseId, Id) {



    var url = getActionURL(LoadAction, LoadController);
    if (UseId) {
        url = url + "/" + Id;
    }
    

    $("#divGrid").load(url, function () {
        //this is one sure fire way to always ensure datatables are loaded in partialviews. need to find out why scripts on page aren't working.
        //$('#detailsTable').dataTable({
        //    "aaSorting": [],
        //    "pageLength": 10
        //});
    });

    $("html, body").animate({ scrollTop: 0 }, "slow");


}