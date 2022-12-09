(function () {
    'use strict';

    var displayPastDateError = function () {
        $("#Date")
            .addClass("input-validation-error")
            .attr({ 'aria-describedby': "Date-error", 'aria-invalid': "true" });
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
        console.log(counterMsg);

        if (totalNumberOfCharecters > characterLimit) {
            $(textArea).siblings('span.counter').addClass('warning');
        } else {
            $(textArea).siblings('span.counter').removeClass('warning');
        }

        $('span.counter').html(counterMsg);
    }

    //  +---------------------------------------------------------------------------+

    $(document).ready(function () {


        //if ($(document.body).has('form')) {

        //    if ($('form').hasClass('filter')) {

        //        if (typeof (Storage) !== "undefined") {

        //            if ((sessionStorage.lastInputId === undefined) && (sessionStorage.lastInputValue === undefined)) {
        //                sessionStorage.lastInputId = "none";
        //                sessionStorage.lastInputValue = "none";
        //            }

        //            $('form.filter').on('change paste', function (event) {
        //                switch ($(event.target).attr('id')) {                            
        //                    case 'addressFilterTerm':
        //                        $('#emailFilterTerm, #nameFilterTerm').val('');
        //                        sessionStorage.lastInputValue = $('#addressFilterTerm').val();
        //                        break;
        //                    case 'emailFilterTerm':
        //                        $('#addressFilterTerm, #nameFilterTerm').val('');
        //                        sessionStorage.lastInputValue = $('#emailFilterTerm').val();
        //                        break;
        //                    case 'nameFilterTerm':
        //                        $('#addressFilterTerm, #emailFilterTerm').val('');
        //                        sessionStorage.lastInputValue = $('#nameFilterTerm').val();
        //                        break;
        //                }

        //                sessionStorage.lastInputId = $(event.target).attr('id');
        //            });
        //        }
        //    }
        //}
        // ----------------------------------------------------------------

        $('a.reset').click(function () {
            var form = $(this).parents('form');
            form.trigger("reset");
        });

        // ---- Form valuidations -----------------------------------------

        $('form').submit(function (event) {

            //if ($(event.target).has('form .filter')) {
            //    console.log("this is a filter form");

            //    //document.cookie = “lastInputId = ' +  + ";" path = /Admin/MPs /; secrure”;
            //    //document.cookie = “lastInputValue = ; path = /Admin/MPs /; secrure”;

            //    // TODO: move the session storage assignment to here!
            //    // sessionStorage.lastInputId = $(event.target).attr('id');
            //    // sessionStorage.lastInputValue = $(event.target).val();
            //}

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

        // --- Character counter -------------------------------------------

        var $textAreaWithCharCounter = $('#short-description');
        $textAreaWithCharCounter.on('keydown keyup paste', function (event) {
            updateCharCounterMsg($(this));
        });

        $('#create_note_form').submit(function () {
            if (characterCount($('#Text')) > $('#Text').attr('data-limit-input')) {
                return false;
            } else {
                return true;
            }
        });

        $('#short-description').htmlarea({
            toolbar: [
                "bold", "italic", "underline",
                "|",
                "h1", "h2", "p",
                "|",
                "orderedList", "unorderedList"
            ]
        });

        $('#long-description').htmlarea({
            toolbar: [
                "bold", "italic", "underline",
                "|",
                "h1", "h2", "p",
                "|",
                "orderedList", "unorderedList"
            ]
        });
    });
}());
