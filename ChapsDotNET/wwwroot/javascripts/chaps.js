(function () {
    'use strict';

    var displayPastDateError = function () {
        $("#Date")
            .addClass("input-validation-error")
            .attr( { 'aria-describedby': "Date-error", 'aria-invalid': "true" } );
        $('span[data-valmsg-for="Date"]')
            .switchClass("field-validation-valid", "field-validation-error")
            .html('<span id="Date-error">The date must be set in the future.</span>');
    }

    var yearMonthDayFormat = function (rawDate) {
        // returns date in yyyy-MM-dd format
        return rawDate.toISOString().slice(0, 10);
    }

    let todaysDate = yearMonthDayFormat(new Date());
    var tomorrow = new Date(todaysDate);
    var yesterday = new Date(todaysDate);

    tomorrow.setDate(tomorrow.getDate() + 1);
    let tomorrowsDate = yearMonthDayFormat(tomorrow);

    yesterday.setDate(yesterday.getDate() - 1)
    let yesterdaysDate = yearMonthDayFormat(yesterday);

    //  +---------------------------------------------------------------------------+

    $(document).ready(function () {

        if ($('table.withSort').length) {

            var previousSortOrder = localStorage.getItem('aria-sort');
            var previousSortby = "#" + localStorage.getItem('buttonID');

            switch (previousSortOrder) {
                case "ascending":
                    $(previousSortby).parents('th').attr('aria-sort', 'descending');
                    break;
                default:
                    $(previousSortby).parents('th').attr('aria-sort', 'ascending');
            }
            localStorage.clear();

            $('button').click(function (e) {
                localStorage.setItem('buttonID',  $(this).attr("id"));
                localStorage.setItem('aria-sort', $(this).parents('th').attr('aria-sort'));
            })
        }

        // Form post validations

        $('form').submit(function (event) {
            if ($(event.target).has('input .future-date')) {
                if ($('#Date').val() < tomorrowsDate) {
                    displayPastDateError();
                    return false;
                }
                else {
                    return true;
                }
            }
        });

    });
}());
