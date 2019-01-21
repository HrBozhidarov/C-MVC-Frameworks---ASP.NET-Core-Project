$(document).ready(function () {
    $('#addToCart2,#addToCart1').on("click", add)
})

function add() {
    let bookId = $('#getitem').text();

    $.ajax({
        type: 'POST',
        dataType: 'text',
        contentType: 'application/json',
        url: '/api/ApiShoppingCart/AddToCart',
        data: JSON.stringify(bookId),
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN", $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        success: function (data) {
            $('#notification').html(data)
        },
        error: function (data) {
            alert(data);
        }
    });
}

function onClose() {
    $("#undo").show();
}

$(document).ready(function () {
    $("#windowCommens").kendoValidator({
        rules: {
            radio: function (input) {
                if (input.filter("[type=radio]") && input.attr("required")) {
                    return $("#windowCommens").find("[name=rating]").is(":checked");
                }
                return true;
            },
            text: function (input) {
                if (input.is("[name=title]")) {
                    return input.val().match(/^[a-zA-Z0-9 \.!?,]{5,40}$/) != null;
                }

                return true;
            },
            textarea: function (input) {
                if (input.is("[name=content]")) {
                    return input.val().match(/.{10,350}/) != null;
                }

                return true;
            }
        },
        messages: {
            radio: "You have to choose ratting from 1 to 5!",
            text: "Title have to be between 5 and 40 symbols!",
            textarea: "Content have to be between 10 and 350 symbols!"
        }
    });

    $("#undo1").bind("click", function () {
        $("#window").data("kendoWindow").open().center();

    });

    $("#undo").bind("click", function () {
        var a = 2;

        $("#window").data("kendoWindow").open().center();

    });
});