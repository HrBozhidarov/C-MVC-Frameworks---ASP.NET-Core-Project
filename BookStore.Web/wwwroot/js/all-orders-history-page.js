let pageSize = 1;
let pageIndex = 0;
let maxPage = 0;
let scrolling = true;
let initial = false;

getNumberOfOrders();
setTimeout(getData(), 300);

$(document).ready(function () {
    $(window).scroll(function () {
        let scrollTop = $(document).scrollTop();
        let windowHeight = $(window).height();
        let bodyHeight = $(document).height() - windowHeight;
        let scrollPercentage = (scrollTop / bodyHeight);

        if (scrollPercentage > 0.8 && pageIndex <= maxPage && !scrolling) {
            getData();

            scrolling = true;
        }
    });
});

function getData() {
    $.ajax({
        type: 'POST',
        url: '/admin/orders/GetOrders',
        data: { "pageindex": pageIndex, "pagesize": pageSize },
        dataType: 'html',
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN", $('input:hidden[name="__RequestVerificationToken"]').val());
            $("#progressOrderLoader").show();
        },
        success: function (data) {
            if (data.trim() != '') {
                $("#allOrders").append(data);
                scrolling = false;
                initial = true;
                pageIndex++;
            }
            else if (!initial) {
                $("#allOrders").append('<h3>No Orders yet.</h3>' +
                    '<hr class= "hr-bg-color-second"/> ');
                initial = true;
            }
            else {
                $("#allOrders").append('<div class="text-center">No more Orders.</div> <hr class= "hr-bg-color-second"/> ');
            }
        },
        complete: function () {
            $("#progressOrderLoader").hide();
        },
        error: function () {
            alert("Error while retrieving data!");
        }
    });
}

function getNumberOfOrders() {
    $.ajax({
        type: 'GET',
        url: '/api/ApiOrders/countOrders',
        data: {},
        dataType: 'text',
        success: function (data) {
            maxPage = Math.floor(parseInt(data) / pageSize);
        },
        error: function () {
            alert("Error in Order Api Controllers With Action CountOrders");
        }
    })
}

function startChange() {
    var endPicker = $("#end").data("kendoDatePicker"),
        startDate = this.value();

    if (startDate) {
        startDate = new Date(startDate);
        startDate.setDate(startDate.getDate() + 1);
        endPicker.min(startDate);
    }
}

function endChange() {
    var startPicker = $("#start").data("kendoDatePicker"),
        endDate = this.value();

    if (endDate) {
        endDate = new Date(endDate);
        endDate.setDate(endDate.getDate() - 1);
        startPicker.max(endDate);
    }
}

$("#ViewIncomes").click(function () {
    var startDate = $('#start').val();
    var endDate = $('#end').val();

    var object = JSON.stringify({ MinDate: startDate, MaxDate: endDate })

    $.ajax({
        type: 'POST',
        url: '/api/ApiOrders/Income',
        data: object,
        contentType: "application/json",
        accepts: "json",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN", $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        success: function (data) {
            if (data != null) {
                swal(`Yours income is ${data.Income} $`, `From start date: ${data.StartDate}\n\nTo end date: ${data.EndDate}`, 'success');
            }
            else {
                swal("Something wrong", "Try again!", "error");
            }
        },
        error: function () {
            swal("Something wrong", "Try again!", "error");
        }
    })
})