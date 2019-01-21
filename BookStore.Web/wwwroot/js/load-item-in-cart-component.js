$(document).ready(function () {

    $('.deleteFromCart').click(function () {
        var delBookId = $(this).find('span').text();
        let deleteItemUrl = '/ShoppingCart/DeleteItem';

        $.ajax({
            url: deleteItemUrl,
            data: { 'bookId': delBookId },
            type: 'GET',
            dataType: 'html',
            success: function (data) {
                if (data != null) {
                    $('#shoppingTable').html(data)
                }
            },
            complete: notification,
            error: function () {
                alert('Error occurred in Card Details');
            }
        })
    })

    $('.updateUp').click(function () {
        var bookId = $(this).find('span').text();
        let updateUpUrl = '/ShoppingCart/UpdateUp'
        $.ajax({
            url: updateUpUrl,
            data: { 'bookId': bookId },
            type: 'POST',
            dataType: 'json',
            success: function (data) {
                console.log(data);
                if (data != null) {
                    $('.quantityNumber' + data.BookId).html(data.Quantity);
                    notification(data);
                }
            },
            error: function () {
                alert('Error occurred in Card Details');
            }
        })
    })

    $('.updateDown').click(function () {
        var bookId = $(this).find('span').text();
        let updateUpUrl = '/ShoppingCart/UpdateDown'
        $.ajax({
            url: updateUpUrl,
            data: { 'bookId': bookId },
            type: 'POST',
            dataType: 'json',
            success: function (data) {
                if (data != undefined) {
                    $('.quantityNumber' + data.BookId).html(data.Quantity);
                    notification(data);
                }
            },
            error: function () {
                alert('Error occurred in Card Details');
            }
        }).then(function (data) {

            if (data == undefined) {
                $.ajax({
                    type: 'POST',
                    dataType: 'html',
                    url: '/ShoppingCart/GetShoppingTable',
                    data: {},
                    success: function (data) {
                        $('#shoppingTable').html(data)
                    },
                    complete: notification,
                    error: function (data) {
                        alert(data);
                    }
                });
            }

        })
    })

    function notification(data) {
        if (data != null) {
            $.ajax({
                type: 'GET',
                dataType: 'text',
                url: '/ShoppingCart/NumberItemsInCart',
                data: {},
                success: function (data) {
                    $('#notification').html(data)
                },
                error: function (data) {
                    alert(data);
                }
            });
        }
    }
})