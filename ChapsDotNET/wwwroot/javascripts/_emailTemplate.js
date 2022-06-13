$(document).ready(function () {
    $('#emailTemplate_CorrespondenceTypeID').change(function () {
        var procemessage = "<option value=''>Please wait...</option>";
        $("#emailTemplate_StageID").html(procemessage).show();
        var url = $(this).data('url') + '/' + $(this).val();

        $.ajax({
            url: url,
            cache: false,
            type: "POST",
            success: function (data) {
                var markup = "<option value=''>Select Stage</option>";
                for (var x = 0; x < data.length; x++) {
                    markup += "<option value=" + data[x].Value + ">" + data[x].Text + "</option>";
                }
                $("#emailTemplate_StageID").html(markup).show();
            },
            error: function (reponse) {
                var procemessage = "<option value=''>Please select a Correspondence Type</option>";
                $("#emailTemplate_StageID").html(procemessage).show();
            }
        });
    });
    $("#emailTemplate_BodyText").htmlarea({
        toolbar: [
                    ["html"],
                    ["bold", "italic", "underline"],
                    ["h1", "h2"], ["orderedlist", "unorderedlist"]
                ],
        loaded: function () {
        }
    });
});
