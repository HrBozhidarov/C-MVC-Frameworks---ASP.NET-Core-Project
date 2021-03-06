﻿$(document).ajaxComplete(function () {
    $.validator.unobtrusive.parse(document);
});

const pathCategories = "/admin/categories/edit";
const pathAuthor = "/admin/authors/edit";
const pathBookEdit = "/admin/books/edit";
const pathBookDelete = "/admin/books/delete";
const ajaxBookRequest = '/admin/books/editdeleteBook';
const ajaxCategoryRequest = '/admin/categories/editcategory';
const ajaxAuthorRequest = '/admin/authors/editauthor';
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
    getCount();
    notificationAdmin();

    function getCount() {
        $.ajax({
            type: 'GET',
            dataType: 'text',
            url: '/api/ApiShoppingCart/NumberItemsInCart',
            data: {},
            success: function (data) {
                $('#notification').html(data)
            },
            error: function (data) {
                alert(data);
            }
        });
    }

    function notificationAdmin() {
        $.ajax({
            type: 'GET',
            url: '/api/ApiQuestions/notificationCount',
            data: {},
            dataType: 'text',
            success: function (data) {
                $('#requestNotification').html(data)
            },
            error: function () {
                alert("Error in Question Api Controllers With Action CountOrders");
            }
        })
    }

    $('.show').hide().show(1000);

    if (path == pathCategories || path == pathAuthor || path == pathBookDelete || path == pathBookEdit) {
        ChooseCategory();
    }
})