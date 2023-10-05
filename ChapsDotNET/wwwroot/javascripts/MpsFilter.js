var totalPages;
var mpsUrl;

$(document).ready(function () {
    var model;
    totalPages = initialPaginationData.totalPages;
    mpsUrl = $('body').data('mps-url');
    updatePaginationControls(initialPaginationData)

    $('body').on('input', '#nameFilter', debounce(function () {
        currentPage = 1;
        model = generateModel(currentPage)
        filterAndSortMps(model, mpsUrl);
    }, 600));


    // pagination link click event
    $('body').on('click', '.page-link', function (e) {
        e.preventDefault(); //prevents the default link action
        currentPage = parseInt($(this).text());
        console.log(currentPage);
        var model = generateModel(currentPage);

        filterAndSortMps(model, mpsUrl)
    })

    //next, previous, first, last button listeners
    $('#firstButton').click(handleFirstButtonClick);
    $('#nextButton').click(handleNextButtonClick);
    $('#prevButton').click(handlePrevButtonClick);
    $('#lastButton').click(handleLastButtonClick);

});

function filterAndSortMps(model, mpsUrl) {
    var nameFilterValue = $('#nameFilter').val();
    var addressFilterValue = $('#addressFilter').val();
    var emailFilterValue = $('#emailFilter').val();
    var focusedElementId = $(':focus').attr('id');
    console.log(model);
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
            console.log(data);
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
            .off('click')
            .addClass('pageButton-img-next-disabled')
            .removeClass('pageButton-img-next-enabled')
    } else {
        $("#nextButton")
            .on('click', handleNextButtonClick)
            .addClass('pageButton-img-next-enabled')
            .removeClass('pageButton-img-next-disabled')
    }

    if (paginationInfo.currentPage <= 1) {
        $("#prevButton")
            .off('click')
            .addClass('pageButton-img-previous-disabled')
            .removeClass('pageButton-img-previous-enabled');
    } else {
        $("#prevButton")
            .on('click', handlePrevButtonClick)
            .addClass('pageButton-img-previous-enabled')
            .removeClass('pageButton-img-previous-disabled');
    }

    if (paginationInfo.currentPage === paginationInfo.totalPages) {
        $("#lastButton")
            .off('click')
            .addClass('pageButton-img-last-disabled')
            .removeClass('pageButton-img-last-enabled')
    } else {
        $("#lastButton")
            .on('click', handleLastButtonClick)
            .addClass('pageButton-img-last-enabled')
            .removeClass('pageButton-img-last-disabled')
    }

    if (paginationInfo.currentPage === 1) {
        $("#firstButton")
            .off('click')
            .addClass('pageButton-img-first-disabled')
            .removeClass('pageButton-img-first-enabled')
    } else {
        $("#firstButton")
            .on('click', handleFirstButtonClick)
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
            pageLinksHtml += `<span class="current-page">${i}</span> `;
        } else {
            const url = `/GetFilteredMPs?page=${i}`;
            pageLinksHtml += `<a href="${url}" class="page-link">${i}</a> `
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
    data.mPs.forEach(mp => {
        let activeCheck = mp.active ? '<input type="checkbox" checked disabled />' : '<input type="checkbox" disabled />';
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
                <td align="center">
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
        sortOrder: sortOrder,
        activeFilter: !$('#onlyActive').is(':checked')
    }
}

function handleNextButtonClick(e) {
    e.preventDefault();
    if (currentPage < totalPages) {
        currentPage += 1;
    } else { return; }

    var model = generateModel(currentPage);
    filterAndSortMps(model, mpsUrl)
}

function handlePrevButtonClick(e) {
    e.preventDefault();
    if (currentPage > 1) {
        currentPage -= 1;
    } else { return; }
    var model = generateModel(currentPage);
    filterAndSortMps(model, mpsUrl);
}

function handleLastButtonClick(e) {
    e.preventDefault();
    currentPage = totalPages;
    var model = generateModel(currentPage);
    filterAndSortMps(model, mpsUrl);
}

function handleFirstButtonClick(e) {
    e.preventDefault();
    currentPage = 1;
    var model = generateModel(currentPage);
    filterAndSortMps(model, mpsUrl);
}