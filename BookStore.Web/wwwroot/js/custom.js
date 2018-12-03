$(document).ready(function () {
    const pathCategories = "/categories/edit";
    const pathAuthor = "/authors/edit";
    const ajaxCategoryRequest = '/categories/editcategory';
    const ajaxAuthorRequest = '/authors/editauthor';
    const authorId = "#eidtAuthor";
    const categoryId = "#editCategory";
    const passDataCategoryId = '#selCategory';
    const passDataAuthorId = '#selAuthor';

    let path = window.location.pathname.toLowerCase();
    let id = "";
    let currentRequest = "";
    let currentPassId = "";

    switch (path) {
        case pathCategories:
            id = categoryId;
            currentRequest = ajaxCategoryRequest;
            currentPassId = passDataCategoryId;
            break;
        case pathAuthor:
            id = authorId;
            currentRequest = ajaxAuthorRequest;
            currentPassId = passDataAuthorId;
            break;
    }

    if (id !== "") {
        ChooseCategory();
    }

    $(currentPassId).on("change", ChooseCategory)

    function ChooseCategory() {
        $(document).ajaxComplete(function () {
            $.validator.unobtrusive.parse(document);
        });

        $(id).hide(500);

        let selectedName = $(currentPassId).find(":selected").text();

        $.ajax({
            url: currentRequest,
            data: { dataName: selectedName },
            type: 'GET',
            success: function (data) {

                $(id).show(1000).html(data);
            },
            error: function () {
                alert('Error occurred');
            }
        });
    }
})