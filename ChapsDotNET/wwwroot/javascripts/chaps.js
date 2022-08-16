(function () {
    'use strict';

    let todaysDate = new Date().toISOString().slice(0, 10);
    let tomorrow = new Date(todaysDate);

    tomorrow.setDate(tomorrow.getDate() + 1);
    var tomorrowsDate = tomorrow.toISOString().slice(0, 10);

    var displayPastDateError = function () {
        $("#Date").addClass("input-validation-error");
        $("#Date").attr("aria-describedby", "Date-error");
        $("#Date").attr("aria-invalid", "true");
        $('span[data-valmsg-for="Date"]').switchClass("field-validation-valid", "field-validation-error");
        $('span[data-valmsg-for="Date"]').html('<span id="Date-error">The date must be set in the future.</span>');
    }

    //  +---------------------------------------------------------------------------+

    $(document).ready(function () {
        $('#create_public_holiday').submit(function () {
            if ($('#Date').val() < tomorrowsDate) {
                displayPastDateError();
                return false;
            }
            else {
                return true;
            }
        });
        $('#edit_public_holiday').submit(function () {
            if ($('#Date').val() < tomorrowsDate) {
                displayPastDateError();
                return false;
            }
            else {
                return true;
            }
        });
    });
}());
