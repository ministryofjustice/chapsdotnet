'use strict';
var totalPages;
var mpsUrl;
let currentSortColumn = '';
let currentSortDirection = '';

$(document).ready(function () {

    var model;
    totalPages = initialPaginationData.totalPages;
    mpsUrl = $('body').data('mps-url');
    updatePaginationControls(initialPaginationData)

    $('body').on('input change', '#nameFilter, #addressFilter, #emailFilter', debounce(function () {
        currentPage = 1;
        model = generateModel(currentPage)
        filterAndSortMps(model, mpsUrl);
    }, 600));

    // Active only checkbox
    $('#onlyActive').on('change', function () {
        event.stopPropagation();
        currentPage = 1;
        model = generateModel(currentPage);
        filterAndSortMps(model, mpsUrl);
        toggleDeactivatedColumn();
    });

    // pagination link click event
    $('body').on('click', '.mpPageButton', function (e) {
        e.preventDefault(); //prevents the default link action
        currentPage = parseInt($(this).text());
        var model = generateModel(currentPage);
        filterAndSortMps(model, mpsUrl)
    })

    //next, previous, first, last button listeners
    $('#firstButton').click(handleFirstButtonClick);
    $('#nextButton').click(handleNextButtonClick);
    $('#prevButton').click(handlePrevButtonClick);
    $('#lastButton').click(handleLastButtonClick);

    //Clear filters
    $('#clearFilters').on('click', function () {
        $('#nameFilter').val('');
        $('#addressFilter').val('');
        $('#emailFilter').val('');
        $('#onlyActive').prop('checked', false);

        currentPage = 1;
        model = generateModel(currentPage);
        filterAndSortMps(model, mpsUrl);
    });

    //sort columns
    $('table').on('click', '.sortable', function (event) {
        event.preventDefault();
        const column = $(this).data('sort'); // gets the name of the clicked column
        const newSortDirection = currentSortColumn === column ?
            (currentSortDirection === 'asc' ? 'desc' : 'asc') : 'desc';

        //reset all sort icons
        $('.sort-button').attr('src', '/images/bullet_arrow_up.png');

        //set sort icon on clicked column
        $(this).find('.sort-button').attr('src', newSortDirection === 'asc'
            ? '/images/bullet_arrow_up.png' : '/images/bullet_arrow_down.png');
        currentSortColumn = column;
        currentSortDirection = newSortDirection;
        model = generateModel(currentPage);
        filterAndSortMps(model, mpsUrl);
    })
});

function filterAndSortMps(model, mpsUrl) {
    var nameFilterValue = $('#nameFilter').val();
    var addressFilterValue = $('#addressFilter').val();
    var emailFilterValue = $('#emailFilter').val();
    var activeFilterValue = $('#onlyActive').val();
    var focusedElementId = $(':focus').attr('id');
    $.ajax({
        type: 'POST',
        url: mpsUrl,
        data: JSON.stringify(model),
        headers: {
            __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val(),
        },
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        success: function (data) {
            let html = buildMPHtmlTable(data);
            $("#mpListContainer").html(html);
            $('#nameFilter').val(nameFilterValue);
            $('#addressFilter').val(addressFilterValue);
            $('#emailFilter').val(emailFilterValue);
            $('#' + focusedElementId).focus();

            totalPages = data.pagination.totalPages;
            updatePaginationControls(data.pagination);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.error("AJAX call failed fetching Mps. Status:", textStatus, "Error: ", errorThrown);
            if (jqXHR.responseText) {
                console.error("Server response:", jqXHR.responseText);
            }
        }
    });
}

function updatePaginationControls(paginationInfo) {

    // display the current page and total pages
    $('#currentPageSpan').text(paginationInfo.currentPage);
    $('#totalPagesSpan').text(paginationInfo.totalPages)

    if (paginationInfo.currentPage >= paginationInfo.totalPages) {
        $("#nextButton")
            .addClass('pageButton-img-next-disabled')
            .removeClass('pageButton-img-next-enabled')
    } else {
        $("#nextButton")
            .addClass('pageButton-img-next-enabled')
            .removeClass('pageButton-img-next-disabled')
    }

    if (paginationInfo.currentPage <= 1) {
        $("#prevButton")
            .addClass('pageButton-img-previous-disabled')
            .removeClass('pageButton-img-previous-enabled');
    } else {
        $("#prevButton")
            .addClass('pageButton-img-previous-enabled')
            .removeClass('pageButton-img-previous-disabled');
    }

    if (paginationInfo.currentPage === paginationInfo.totalPages) {
        $("#lastButton")
            .addClass('pageButton-img-last-disabled')
            .removeClass('pageButton-img-last-enabled')
    } else {
        $("#lastButton")
            .addClass('pageButton-img-last-enabled')
            .removeClass('pageButton-img-last-disabled')
    }

    if (paginationInfo.currentPage === 1) {
        $("#firstButton")
            .addClass('pageButton-img-first-disabled')
            .removeClass('pageButton-img-first-enabled')
    } else {
        $("#firstButton")
            .addClass('pageButton-img-first-enabled')
            .removeClass('pageButton-img-first-disabled')
    }

    // enable or diasble the First, Last, Next and Previous Buttons
    $('#firstButton').prop('disabled', paginationInfo.currentPage === 1);
    $('#prevButton').prop('disabled', paginationInfo.currentPage === 1);
    $('#nextButton').prop('disabled', paginationInfo.currentPage === paginationInfo.totalPages);
    $('#lastButton').prop('disabled', paginationInfo.currentPage === paginationInfo.totalPages);

    // update the direct page number links
    let pageLinksHtml = '';
    for (let i = 1; i <= paginationInfo.totalPages; i++) {
        if (i === paginationInfo.currentPage) {
            pageLinksHtml += `<span class="mpPageButton pagenumber-disabled">${i}</span>`;
        } else {
            const url = `/GetFilteredMPs?page=${i}`;
            pageLinksHtml += `<a href="${url}" class="mpPageButton">${i}</a>`
        }
    }
    $('#pageLinksContainer').html(pageLinksHtml);
}

function debounce(func, wait) {
    var timeout;
    return function () {
        var context = this, args = arguments;
        var later = function () {
            timeout = null;
            func.apply(context, args);
        };
        clearTimeout(timeout);
        timeout = setTimeout(later, wait);
    };
}

function buildMPHtmlTable(data) {
    let htmlString = "";
    const showDeactivated = !$('#onlyActive').prop('checked');
    data.mPs.forEach(mp => {
        let activeCheck = mp.active ? '<input type="checkbox" checked disabled />' : '<input type="checkbox" disabled />';
        let deactivTd = showDeactivated ? (mp.deactivatedDisplay ? `<td>${mp.deactivatedDisplay}</td>` : '<td></td>') : '';
        htmlString += `
            <tr>
                <td>
                    <a href="/Admin/MPs/Edit/${mp.mpId}">
                        ${mp.displayFullName}
                    </a>
                </td>
                <td>
                    ${mp.displaySingleLineAddress}
                </td>
                <td>
                    ${mp.email ? mp.email : ""}
                </td>
                ${deactivTd}
                <td class="align-right">
                    ${activeCheck}
                </td>
            </tr>
        `;
    });

    return htmlString;
}

function generateModel(pageNumber) {
    return {
        PageNumber: pageNumber,
        PageSize: 10,
        NoPaging: false,
        ShowActiveAndInactive: !$('#onlyActive').is(':checked'),
        nameFilterTerm: $('#nameFilter').val(),
        addressFilterTerm: $('#addressFilter').val(),
        emailFilterTerm: $('#emailFilter').val(),
        sortOrder: currentSortDirection,
        SortColumn: currentSortColumn
    }
}

function handleNextButtonClick(e) {
    e.preventDefault
    if ($("#nextButton").hasClass('page-button-img-next-disabled')) {
        return;
    }
    if (currentPage < totalPages) {
        currentPage += 1;
    } else { return; }
    var model = generateModel(currentPage);
    filterAndSortMps(model, mpsUrl)
}

function handlePrevButtonClick(e) {
    if ($("#nextButton").hasClass('page-button-img-previous-disabled')) {
        return;
    }
    e.preventDefault();
    if (currentPage > 1) {
        currentPage -= 1;
    } else { return; }
    var model = generateModel(currentPage);
    filterAndSortMps(model, mpsUrl);
}

function handleLastButtonClick(e) {
    if ($("#nextButton").hasClass('page-button-img-last-disabled')) {
        return;
    }
    e.preventDefault();
    currentPage = totalPages;
    var model = generateModel(currentPage);
    filterAndSortMps(model, mpsUrl);
}

function handleFirstButtonClick(e) {
    if ($("#nextButton").hasClass('page-button-img-first-disabled')) {
        return;
    }
    e.preventDefault();
    currentPage = 1;
    var model = generateModel(currentPage);
    filterAndSortMps(model, mpsUrl);
}

function toggleDeactivatedColumn() {
    if ($('#onlyActive').prop('checked')) {
        $('.deactivatedByUserNameDate, .spacerData').hide();
        $('#deactivatedByUserNameDate, #spacerHeader').hide();
    } else {
        $('.deactivatedByUserNameDate, .spacerData').show();
        $('#deactivatedByUserNameDate, #spacerHeader').show();
    }
}
