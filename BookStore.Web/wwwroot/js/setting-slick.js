﻿$(document).on('ready', function () {
    let url = "/api/apibooks/asc";
    let url1 = "/api/apibooks/desc";
    let firstSliderId = "#slider-index-one";
    let secondSliderId = "#slider-index-two";

    ajaxRequest(url1, secondSliderId);
    ajaxRequest(url, firstSliderId);

    function ajaxRequest(url, id) {
        $.get(url, function () {

        }).done(function (data) {
            $.ajax({
                data: JSON.stringify(data),
                dataType: 'html',
                url: '/home/sliderpictures',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                success: function (data) {

                    $(id).html(data);

                    $(id).slick({
                        infinite: false,
                        speed: 300,
                        respondTo: 'slider',
                        slidesToShow: 5,
                        slidesToScroll: 1,
                        responsive: [
                            {
                                breakpoint: 1024,
                                settings: {
                                    slidesToShow: 4,
                                    slidesToScroll: 1,
                                }
                            },
                            {
                                breakpoint: 800,
                                settings: {
                                    slidesToShow: 3,
                                    slidesToScroll: 1
                                }
                            },
                            {
                                breakpoint: 600,
                                settings: {
                                    slidesToShow: 2,
                                    slidesToScroll: 1
                                }
                            },
                            {
                                breakpoint: 450,
                                settings: {
                                    slidesToShow: 2,
                                    slidesToScroll: 1
                                }
                            },
                            {
                                breakpoint: 285,
                                settings: {
                                    slidesToShow: 1,
                                    slidesToScroll: 1
                                }
                            },
                        ]
                    });
                }
            })
        })
            .fail(function () {
                alert("error");
            })
    }
})