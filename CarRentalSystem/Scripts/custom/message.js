function MessageShow(msg, title, messagetype) {
    toastr.options = {
        "closeButton": false,
        "debug": false,
        "newestOnTop": true,
        "progressBar": true,
        "positionClass": "toast-top-right",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }
    // toastr['success'](msg, title);

    switch (messagetype) {
        case 'Success':
            toastr.success(msg, title);
            break;
        case 'Error':
            toastr.error(msg, title);
            break;
        case 'Warning':
            toastr.warning(msg, title);
            break;

        case 'Information':
            toastr.info(msg, title);
            break;
        default:
            toastr.info(msg, title);
    }

    return false;
}
function getConfirmation_verifys(sender, title, message) {
    $("#spnTitle3").text(title);
    $("#spnMsg3").text(message);
    $('#modalPopUp_Delete').modal('show');
    $('#btnConfirm3').attr('onclick', "$('#modalPopUp').modal('hide');setTimeout(function(){" + $(sender).prop('href') + "}, 50);");
    return false;
}