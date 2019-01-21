let pageSize = 1;
let pageIndex = 0;
let maxPage = 0;
let scrolling = true;
let initial = false;

getNumberOfComments();
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
        url: '/admin/comments/GetDataNotApprovalComments',
        data: { "pageindex": pageIndex, "pagesize": pageSize },
        dataType: 'html',
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN", $('input:hidden[name="__RequestVerificationToken"]').val());
            $("#progress").show();
        },
        success: function (data) {
            if (data.trim() != '') {
                $("#aprovalComments").append(data);
                scrolling = false;
                initial = true;
                pageIndex++;
            }
            else if (!initial) {
                $("#aprovalComments").append('<div class="normal-bold text-center">No Comments for Approve yet.</div> ');
                initial = true;
            }
            else {
                $("#aprovalComments").append('<div class="text-center">No more comments for approval.</div> <hr class= "hr-bg-color-second"/> ');
            }
        },
        complete: function () {
            $("#progress").hide();
        },
        error: function () {
            alert("Error while retrieving data!");
        }
    });
}

function getNumberOfComments() {
    $.ajax({
        type: 'GET',
        url: '/api/ApiComments/countNotAprovel',
        data: {},
        dataType: 'text',
        success: function (data) {
            maxPage = Math.floor(parseInt(data) / pageSize);
        },
        error: function () {
            alert("Error while retrieving data!");
        }
    })
}