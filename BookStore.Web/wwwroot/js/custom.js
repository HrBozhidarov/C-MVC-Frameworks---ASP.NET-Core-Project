$(document).ready(function () {
    let pathname = window.location.pathname;

    if (pathname.toLowerCase() === "/categories/edit") {
        ChooseCategory();
    }

    $('#selCategory').on("change", ChooseCategory)

    function ChooseCategory() {

        $(".empty").hide(7000);

        $("#editCategory").hide(500);

        let selectedName = $('#selCategory').find(":selected").text();

        $.ajax({
            url: '/categories/editcategory',
            data: { category: selectedName },
            type: 'GET',
            success: function (data) {
                $('#editCategory').show(1000).html(data);
            },
            error: function () {
                alert('Error occurred');
            }
        });
    }
})