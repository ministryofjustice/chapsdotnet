(function () {
    'use strict';

    let todaysDate = new Date();

    //  +---------------------------------------------------------------------------+

    $(document).ready(function () {

        // Form submission validations ----------------------------------------------



        $('#create_correspondence_form').submit(function () {
            if (characterCount($('#Item_Subject')) <= $('#Item_Subject').attr('data-limit-input') &&
                characterCount($('#Note_Text')) <= $('#Note_Text').attr('data-limit-input') &&
                isValidDateFormat($('#Item_DateReceived').val()) == true &&
                isValidReceivedDate($('#Item_DateReceived').val()) == true
            ) {
                $('#SaveBtn').value = 'Saving...';
                return true;
            } else {
                return false;
            }
        });
    });
}());
