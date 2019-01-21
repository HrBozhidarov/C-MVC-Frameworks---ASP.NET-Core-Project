$(document).ready(function () {
    totalMoney();

    $('#clearShoppingCart').click(function () {
        let shoppingCartClearUrl = '/api/ApiShoppingCart/ClearShoppingCart';

        $.ajax({
            url: shoppingCartClearUrl,
            data: {},
            type: 'POST',
            dataType: 'json',
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            success: function (data) {
                $('#emptyBody').html(`
                            <tr>
                                <td>
                                    <div class="h4 text-center">Yours shopping cart is empty!</div>
                                </td>
                            </tr>`);

                $('#orderFromCart').hide();
            },
            complete: notification,
            error: function () {
                alert('Error occurred while Empty shopping cart');
            }
        }).then(totalMoney)
    })

    $('.deleteFromCart').click(function () {
        var delBookId = $(this).find('span').text();
        let deleteItemUrl = '/ShoppingCart/DeleteItem';

        $.ajax({
            url: deleteItemUrl,
            data: { 'bookId': delBookId },
            type: 'POST',
            dataType: 'html',
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN", $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            success: function (data) {
                if (data != null) {
                    $('#shoppingTable').html(data);
                    $('#orderFromCart').hide();
                }
            },
            complete: notification,
            error: function () {
                alert('Error occurred in Card Details');
            }
        }).then(totalMoney)
    })

    $('.updateUp').click(function () {
        let bookId = $(this).find('span').text();

        let updateUpUrl = '/api/ApiShoppingCart/UpdateUp'
        $.ajax({
            url: updateUpUrl,
            data: JSON.stringify(bookId),
            contentType: "application/json",
            type: 'PUT',
            dataType: 'json',
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN", $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            success: function (data) {

                if (data != null) {
                    $('.quantityNumber' + data.BookId).html(data.Quantity);
                    notification(data);
                }
            },
            error: function () {
                alert('Error occurred in Card Details');
            }
        }).then(totalMoney)
    })

    $('.updateDown').click(function () {
        var bookId = $(this).find('span').text();
        let updateUpUrl = '/api/ApiShoppingCart/UpdateDown'
        $.ajax({
            url: updateUpUrl,
            data: JSON.stringify(bookId),
            type: 'PUT',
            contentType: 'application/json',
            dataType: 'json',
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN", $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            success: function (data) {

                if (data != undefined) {
                    $('.quantityNumber' + data.BookId).html(data.Quantity);
                    notification(data);
                }
                else {
                    $('#orderFromCart').hide();
                }
            },
            error: function () {
                alert('Error occurred in Card Details');
            }
        }).then(function (data) {
            console.log(JSON.stringify(data));
            if (data == undefined) {
                $.ajax({
                    type: 'POST',
                    dataType: 'html',
                    url: '/ShoppingCart/GetShoppingTable',
                    data: {},
                    beforeSend: function (xhr) {
                        xhr.setRequestHeader("XSRF-TOKEN", $('input:hidden[name="__RequestVerificationToken"]').val());
                    },
                    success: function (data) {
                        $('#shoppingTable').html(data)
                    },
                    complete: notification,
                    error: function (data) {
                        alert(data);
                    }
                });
            }
        }).then(totalMoney)
    })

    function totalMoney() {
        let totalPriceUrl = '/api/ApiShoppingCart/TotalPrice';

        $.ajax({
            url: totalPriceUrl,
            data: {},
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                var totalPrice = parseFloat(data);
                $('#totalPrice').html(totalPrice);
            },
            error: function () {
                alert('Error occurred in Get Total money');
            }
        })
    }

    function notification(data) {
        if (data != null) {
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
    }
})