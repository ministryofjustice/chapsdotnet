$(document).ready(function () {
    var model;
    let currentPage = 1

    updatePaginationControls(initialPaginationData)

    $('body').on('input', '#nameFilter', debounce(function () {
        var mpsUrl = $('body').data('mps-url');
        model = {
            PageNumber: currentPage,
            PageSize: 10,
            NoPaging: false,
            ShowActiveAndInactive: !$('#onlyActive').is(':checked'),
            nameFilterTerm: $('#nameFilter').val(),
            addressFilterTerm: $('#addressFilter').val(),
            emailFilterTerm: $('#emailFilter').val(),
            sortOrder: sortOrder,
            activeFilter: !$('#onlyActive').is(':checked')
        }
        console.log(model.nameFilterTerm);
        filterAndSortMps(model, mpsUrl);
    }, 300));


    // pagination link click event
    $('body').on('click', '.page-link', function (e) {
        e.preventDefault(); //prevents the default link action
        currentPage = parseInt($(this).text());
        var mpsUrl = $('body').data('mps-url');
        model = {
            PageNumber: currentPage,
            PageSize: 10,
            NoPaging: false,
            //ShowActiveAndInactive: $('#onlyActive').is(':checked'),
            nameFilterTerm: $('#nameFilter').val(),
            addressFilterTerm: $('#addressFilter').val(),
            emailFilterTerm: $('#emailFilter').val(),
            sortOrder: sortOrder,
            activeFilter: $('#onlyActive').is(':checked')
        }

        filterAndSortMps({ ...model, PageNumber: parseInt(pageNumber) }, mpsUrl)
    })
});

function filterAndSortMps(model, mpsUrl) {
    var nameFilterValue = $('#nameFilter').val();
    console.log("name filter: " + nameFilterValue);
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
        contentType: "application/json;in the success:  charset=utf-8",
        dataType: 'json',
        success: function (data) {
            console.log(data);
            let html = buildMPHtmlTable(data);
            $("#mpListContainer").html(html);
            $('#nameFilter').val(nameFilterValue);
            $('#addressFilter').val(addressFilterValue);
            $('#emailFilter').val(emailFilterValue);
            $('#' + focusedElementId).focus();
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
    if (!paginationInfo) {
        console.error("pagination info not defined!")
        return;
    }
    // display the current page and total pages
    $('#currentPageSpan').text(paginationInfo.currentPage);
    $('#totalPagesSpan').text(paginationInfo.totalPages)

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
                    <a href="/Admin/MPs/Edit/${mp.mPId}">
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