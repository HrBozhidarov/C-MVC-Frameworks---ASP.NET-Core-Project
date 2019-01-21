function onClose() {
    $("#orderFromCart").show();
}

let visualizeOfCreateUrl = "/Orders/VisualizeCreate";

$(document).ready(function () {
    $("#orderFromCart").bind("click", function () {
        $("#windowOrder").data("kendoWindow").open().center();

        $.ajax({
            url: visualizeOfCreateUrl,
            data: {},
            type: 'POST',
            dataType: 'html',
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN", $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            success: function (data) {
                if (data != null) {
                    $("#contentOrder").html(data)
                }
            },
            error: function () {
                alert('Error occurred in VISUALIZE CREATE');
            }
        })
    });
});