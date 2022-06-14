var mdl = {
    CorrespondenceTypeID: $('#CorrespondenceTypeID').val(),
    Description: "@Model.Description",
    RecordCount: "@Model.RecordCount",
    UserID: "@Model.UserID",
    StatusID: $('#StatusID').val(),
    RagStatus: $('#RagStatus').val(),
    TargetDateMin: $('#TargetDateMin').val(),
    TargetDateMax: $('#TargetDateMax').val(),
    TargetDateSearch: $('#TargetDateSearch').val(),
    ItemDateMin: $('#ItemDateMin').val(),
    ItemDateMax: $('#ItemDateMax').val(),
    sortOrder: null
};

$("#lnkCorrespondent").on("click", function () {

    $('#reloading').show();

    $.ajax({
        url: "/search/GetSearchResults",
        type: "POST",
        dataType: "html",
        contentType: 'application/json',
        data: JSON.stringify(mdl),
        success: function (html) {
            $('#reloading').hide();
            $("#results").empty();
            $("#results").append(html);
        }
    });
});

$("#lnkTargetDate").on("click", function () {

    $('#reloading').show();

    $.ajax({
        url: "/search/GetSearchResults",
        type: "POST",
        dataType: "html",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(mdl),
        success: function (html) {
            $('#reloading').hide();
            $("#results").empty();
            $("#results").append(html);
        }
    });
});