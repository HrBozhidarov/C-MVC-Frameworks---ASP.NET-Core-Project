$(document).ready(function () {
    $('#selCategory').on("change", ChooseCategory)

    function ChooseCategory() {
        $("#editCategory").hide(500);

        let selectedName = $('#selCategory').find(":selected").text();

        $.ajax({
            url: '/categories/editcategory',
            data: { category: selectedName },
            //dataType: 'html',
            type: 'GET',
            success: function (data) {
                $('#editCategory').show(1000).html(data);
            },
            error: function (data) {
                alert('Error occurred');
            }
        });
    }
})