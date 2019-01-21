$('.del').click(function () {
    $(this).parent().parent().hide();
    let arr = $(this).parent().parent().children().children().attr('class').split(' ');
    let id = $(this).next().text();
    let deleteUrl = `/api/ApiQuestions/deleteQuestion`;

    $.ajax({
        url: deleteUrl,
        data: JSON.stringify(id),
        type: 'DELETE',
        contentType: "application/json",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN", $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        success: function (data) {
            let number = parseInt($("#requestNotification").text());
            let isNotificated = $.inArray('btn-warning', arr);

            if (number > 0 && isNotificated > 0) {
                number--;

                $("#requestNotification").html(number);
            }
        },
        error: function () {
            alert('Error occurred in VISUALIZE CREATE');
        }
    });
})

function onClose() {
    $("#sednEmail").show();
}

$(document).ready(function () {
    $(".click").bind("click", function () {
        $("#windowSend").data("kendoWindow").open().center();

        let visualizeOfSendEmail = "/Questions/VisualizeSendEmail";

        let id = parseInt($(this).next().text());
        $(this).removeClass("btn-warning").addClass("btn-success")

        let number = parseInt($("#requestNotification").text());

        if (number > 0) {
            number--;
            $("#requestNotification").html(number);
        }

        $.ajax({
            url: visualizeOfSendEmail,
            data: { id: id },
            type: 'POST',
            dataType: 'html',
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN", $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            success: function (data) {
                if (data != null) {
                    $("#sednEmail").html(data);
                }
            },
            error: function () {
                alert('Error occurred in VISUALIZE CREATE');
            }
        }).then(function () {
            let putUrl = `/api/ApiQuestions/updateState`;

            $.ajax({
                url: putUrl,
                data: JSON.stringify(id),
                type: 'PUT',
                contentType: "application/json",
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("XSRF-TOKEN", $('input:hidden[name="__RequestVerificationToken"]').val());
                },
                success: function (data) {

                },
                error: function (data) {
                    console.log(data);
                }
            })
        })
    });
});