(function () {
    'use strict';

    let todaysDate = new Date();

    var convertToShortDateFormat = function (dateToFormat) {
        // Convert a date formatted as dd/mm/yyy to mm/dd/yyyy
        var shortFormattedDate = dateToFormat.substring(3, 5) + "/" + dateToFormat.substring(0, 2) + "/" + dateToFormat.substring(6, 10);
        return shortFormattedDate
    }

    var isValidDateFormat = function (dateInput) {
        // regular expression to match required date format dd/mm/yyyy
        var dateFormatRegEx = /^\d{2}\/\d{2}\/\d{4}$/;

        if (dateInput != '' && dateInput.match(dateFormatRegEx)) {
            return true;
        } else {
            return false;
        }
    }

    var isValidReceivedDate = function (inputDateValue) {
        var dateReceived = new Date(convertToShortDateFormat(inputDateValue));

        if (dateReceived > todaysDate) {
            $('#date-received-section div span')
                .switchClass("field-validation-valid", "field-validation-error")
                .html("<span id='Item_DateReceived-error'>The date received should be either today or a date in the past</span>");
            return false;
        } else {
            $('#date-received-section div span')
                .switchClass("field-validation-error", "field-validation-valid")
                .html("");
            return true;
        }
    }

    var characterCount = function (textArea) {
        var numberOfCharacters = $(textArea).val().length;
        var textAreaContent = $(textArea).val();
        var hasControlCharacters = textAreaContent.match(/(\r\n|\n|\r|\t)/g);

        if (hasControlCharacters != null) {
            numberOfCharacters += hasControlCharacters.length;
        }

        return numberOfCharacters;
    }

    var updateCharCounterMsg = function (textArea) {
        const msgPrefix = "You have ";
        const charsRemainingPlural = " characters remaining";
        const charsRemainingSingular = " character remaining";
        const charsTooManyPlural = " characters too many";
        const charsTooManySingular = " character too many";

        var characterLimit = $(textArea).attr('data-limit-input');
        var characterLimitMinusOne = characterLimit;
        var characterLimitPlusOne = characterLimit;
        var counter = 0;
        var msgPostfix = "";
        var totalNumberOfCharecters = characterCount($(textArea));

        counter = Math.abs(characterLimit - totalNumberOfCharecters);

        characterLimitMinusOne = --characterLimitMinusOne;
        characterLimitPlusOne = ++characterLimitPlusOne;

        if (totalNumberOfCharecters < characterLimitMinusOne) {
            msgPostfix = charsRemainingPlural;
        } else if (totalNumberOfCharecters == characterLimitMinusOne) {
            msgPostfix = charsRemainingSingular;
        } else if (totalNumberOfCharecters == characterLimit) {
            msgPostfix = charsRemainingPlural;
        } else if (totalNumberOfCharecters == characterLimitPlusOne) {
            msgPostfix = charsTooManySingular;
        } else if (totalNumberOfCharecters > characterLimitPlusOne) {
            msgPostfix = charsTooManyPlural;
        }

        var counterMsg = `${msgPrefix + counter + msgPostfix}`;

        if (totalNumberOfCharecters > characterLimit) {
            $(textArea).siblings('span.counter').addClass('warning');
        } else {
            $(textArea).siblings('span.counter').removeClass('warning');
        }

        $(textArea).siblings('span.counter').html(counterMsg);
    }

    //  +---------------------------------------------------------------------------+

    $(document).ready(function () {
        var $dateReceivedField = $('#Item_DateReceived');
        var $textAreaWithcharCounter = $('#Item_Subject, #Note_Text, #Text');

        if ($(document.body).has('form')) {
            if ($('form').is($('#create_correspondence_form'))) {
                $dateReceivedField.on('change', function (event) {
                    isValidReceivedDate($(this).val());
                });

                $textAreaWithcharCounter.on('keydown keyup paste', function (event) {
                    updateCharCounterMsg($(this));
                });
            }
            else if ($('form').is($('#edit_correspondence_form'))) {
                updateCharCounterMsg($('#Item_Subject'));

                $dateReceivedField.on('change', function (event) {
                    isValidReceivedDate($(this).val());
                });

                $textAreaWithcharCounter.on('keydown keyup paste', function (event) {
                    updateCharCounterMsg($(this));
                });
            }
            else if ($('form').is($('#create_note_form'))) {
                $textAreaWithcharCounter.on('keydown keyup paste', function (event) {
                    updateCharCounterMsg($(this));
                });
            }
        }

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

        $('#edit_correspondence_form').submit(function () {
            if (characterCount($('#Item_Subject')) <= $('#Item_Subject').attr('data-limit-input') &&
                isValidDateFormat($('#Item_DateReceived').val()) == true &&
                isValidReceivedDate($('#Item_DateReceived').val()) == true
            ) {
                $('#SaveBtn').value = 'Saving...';
                return true;
            } else {
                return false;
            }
        });

        $('#create_note_form').submit(function () {
            if (characterCount($('#Text')) > $('#Text').attr('data-limit-input')) {
                return false;
            } else {
                return true;
            }
        });
    });
}());
