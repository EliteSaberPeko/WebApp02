// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function searchAutor(ids, arrayIds, url) {
    let dataArray = [];
    ids.forEach((value, index, array) => {
        dataArray.push($(`#${value}`).val());
    });
    arrayIds.forEach((value, index, array) => {
        dataArray.push($(`#${value}`).val().join('|'));
    });
    let data = { "data": dataArray };

    $.ajax({
        url: url,
        type: 'POST',
        data: data,
        success: function (data) {
            $('#panelSearch').html(data);
        },
        failure: function (response) {
            alert("failure");
            console.log(response);
        },
        error: function (response) {
            alert("error");
            console.log(response);
        },
    });
}

function ChangePage(url) {
    $(document).on('click', '#previous-page,#next-page', function () {
        let page = $(this).val();
        $.ajax({
            url: url,
            type: 'POST',
            cache: false,
            data: { "page": page },
            success: function (data) {
                $('#panelPage').html(data);
            },
            failure: function (response) {
                alert("failure");
                console.log(response);
            },
            error: function (response) {
                alert("error");
                console.log(response);
            },
        });
    });
}