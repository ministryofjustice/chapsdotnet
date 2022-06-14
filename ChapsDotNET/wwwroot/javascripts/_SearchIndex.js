function PostFailure() {
    alert("Failure");
}
$(document).ready(function () {
    var sourceSwap = function () {
        var $this = $(this);
        var newSource = $this.data('alt-src');
        $this.data('alt-src', $this.attr('src'));
        $this.attr('src', newSource);
    }
    $('#navigation H3').hover(
        function () {
            $(this).find('.imgSwap').css('visibility', 'visible');
        },
        function () {
            $(this).find('.imgSwap').css('visibility', 'hidden');
        }
    );
    $('.imgSwap').hover(sourceSwap, sourceSwap).css('visibility', 'hidden');
    $(".imgSwap").click(function (e) {
        e.preventDefault();
        e.stopImmediatePropagation();
        var region = $(this).data('region');
        resetSection(region);
    });
    function resetSection(name) {
        switch (name) {
            case "Item":
                $('#CorrespondenceTypeID').prop('selectedIndex', 0).trigger('change');
                $('#DateEnteredSearch').prop('selectedIndex', 0).trigger('change');
                $('#DateReceivedSearch').prop('selectedIndex', 0).trigger('change');
                $('#ItemDateSearch').prop('selectedIndex', 0).trigger('change');
                $('#MoJMinisterID').prop('selectedIndex', 0);
                $('#SignatoryID').prop('selectedIndex', 0);
                $('#Subject').val('');
                $('#TheirRef').val('');
                $('#CampaignID').prop('selectedIndex', 0);
                $('#LeadSubjectID').prop('selectedIndex', 0);
                $('#itemCount').text('(0/11)');
                break;
            case "Correspondent":
                $('#CorrespondentFirstName').val('');
                $('#CorrespondentLastName').val('');
                $('#CorrespondentAddress').val('');
                $('#CorrespondentOrganisation').val('');
                $('#SubmittedBy').val('');
                $('#Priority').prop('checked', false);
                $('#correspondentCount').text('(0/6)');
                break;
            case "MP":
                $('#MPForenames').val('');
                $('#MPSurname').val('');
                $('#mpCount').text('(0/2)');
                break;
            case "Stage":
                $('#RagStatus').prop('selectedIndex', 0);
                $('#StatusID').prop('selectedIndex', 0);
                $('#TargetDateSearch').prop('selectedIndex', 0).trigger('change');
                $('#stageCount').text('(0/3)');
                break;
            default:
                alert("Cannot find section " + name + "to reset");
        }
    };
    $(".SaveSearch").on("click", function (e) {
        e.preventDefault();
        $.ajax({
            url: this.href,
            type: "post",
            data: $('#form0').serialize(),
            success: function (result) {
                // The AJAX call succeeded and the server returned a JSON 
                // with a property "s" => we can use this property
                // and set the html of the target div
                $('#ShowResultHere').html(result.s).show();
                $('#SearchName').val('');
            }
        });
        // it is important to return false in order to 
        // cancel the default submission of the form
        // and perform the AJAX call
        return false;
    });
    $(".ExportXL").on("click", function (e) {
        e.preventDefault();
        $('#exportXLForm').submit(); //this works
        return false;
    });
    $(".toolTip").tooltip();
    $("select").mouseleave(function (event) {
        event.stopPropagation();
    });
    $("#SaveSearch").click(function () {
        alert("Handler for .click() called.");
    });
    $(".ItemDate").change(function () {
        var name = $(this).attr('name').replace("Search", "");
        var optionPicked = $(this).val();
        $('#' + name + 'Min').val('');
        $('#' + name + 'Max').val('');
        switch ($(this).val()) {
            case "100": //before
                $('#' + name + 'EndLI').hide();
                $('#' + name + 'StartLI').show();
                $('#' + name + 'MinLBL').text("Pick a date");
                break;
            case "101": //after
                $('#' + name + 'EndLI').hide();
                $('#' + name + 'StartLI').show();
                $('#' + name + 'MinLBL').text("Pick a date");
                break;
            case "102": //between
                $('#' + name + 'EndLI').show();
                $('#' + name + 'StartLI').show();
                $('#' + name + 'MinLBL').text("Pick a start date");
                $('#' + name + 'MaxLBL').text("Pick an end date");
                break;
            default:
                $('#' + name + 'StartLI').hide();
                $('#' + name + 'EndLI').hide();
        }
    });
    $("#accordion").accordion(
        {
            collapsible: true,
            active: false,
            heightStyle: "content"
        });
    function hideSearch(event) {
        $('#navigation').stop().animate({ 'marginLeft': '-480px' }, 200);
        $('#results').stop().animate({ 'marginLeft': '30px' }, 200);
        $('#searchShow').show();
        $('#searchHide').hide();
        return false;
    }
    function showSearch(event) {
        $('#navigation').stop().animate({ 'marginLeft': '20px' }, 200);
        $('#results').stop().animate({ 'marginLeft': '535px' }, 200);
        $('#searchShow').hide();
        $('#searchHide').show();
        return false;
    }
    $('#searchHide').click(hideSearch);
    $('#searchShow').click(showSearch);
});
