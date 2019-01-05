$(document).ajaxComplete(function () {
    $.validator.unobtrusive.parse(document);
});

const pathCategories = "/categories/edit";
const pathAuthor = "/authors/edit";
const pathBookEdit = "/books/books/edit";
const pathBookDelete = "/books/books/delete";
const ajaxBookRequest = '/books/books/editdeleteBook';
const ajaxCategoryRequest = '/categories/editcategory';
const ajaxAuthorRequest = '/authors/editauthor';
const bookEditId = "#editBook";
const bookDeleteId = "#deleteBook";
const authorId = "#eidtAuthor";
const categoryId = "#editCategory";
const passDataBookId = '#selBook';
const passDataCategoryId = '#selCategory';
const passDataAuthorId = '#selAuthor';

var path = window.location.pathname.toLowerCase();
var id = "";
var currentRequest = "";
var currentPassId = "";

function ChooseCategory() {
    initializePath();

    let selectedName = $('span.k-input').text();

    $.ajax({
        url: currentRequest,
        data: { dataName: selectedName },
        type: 'GET',
        success: function (data) {
            $(id).hide(400);
            $(id).show(1000).html(data);
        },
        error: function () {
            alert('Error occurred');
        }
    })
}

function initializePath() {
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
        case pathBookEdit:
            id = bookEditId;
            currentRequest = ajaxBookRequest;
            currentPassId = passDataBookId;
            break;
        case pathBookDelete:
            id = bookDeleteId;
            currentRequest = ajaxBookRequest;
            currentPassId = passDataBookId;
            break;
    }
}

$(document).ready(function () {
    $('.show').hide().show(1000);

    ChooseCategory();
})