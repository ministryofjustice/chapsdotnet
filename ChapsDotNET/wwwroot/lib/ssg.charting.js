$(document).ready(function () {
    $(".ssg_bar_graph").bind("plothover", function (event, pos, item) {
        if (item) {
            if (previousPoint != item.datapoint) {
                previousPoint = item.datapoint;
                $("#flot-tooltip").remove();

                var itemType;

                for (var i = 0; i < item.series.data.length ; i++) {
                    if (item.datapoint[0] == item.series.data[i][3]) {
                        itemType = item.series.data[i][0];
                        break;
                    }
                }
                
                target = item.series.label;
                if (target == "Rec'd") {
                    target = 'were ' + target;
                } else {
                    target = 'met ' + target;
                }
                count = item.datapoint[1];
                colour = item.series.color;

                showTooltip(item.pageX, item.pageY, count + " <b>" + itemType + "</b>'s " + target, colour);
            }
        } else {
            $("#flot-tooltip").remove();
            previousPoint = null;
        }
    });
    $("#from").datepicker({
        defaultDate: "+1w",
        dateFormat: 'dd/mm/yy',
        changeMonth: true,
        numberOfMonths: 3,
        onClose: function (selectedDate) {
            $("#to").datepicker("option", "minDate", selectedDate);
        }
    });
    $("#to").datepicker({
        defaultDate: "+1w",
        dateFormat: 'dd/mm/yy',
        changeMonth: true,
        numberOfMonths: 3,
        onClose: function (selectedDate) {
            $("#from").datepicker("option", "maxDate", selectedDate);
        }
    });
});

function showTooltip(x, y, contents, z) {
	$('<div id="flot-tooltip">' + contents + '</div>').css({
		top: y - 20,
		left: x - 90,
		'border-color': z
	}).appendTo("body").show();
}
// A custom label formatter used by several of the plots
function labelFormatter(label, series) {
    return "<div style='font-size:8pt; text-align:center; padding:2px; color:white;'>" + label + "<br/>" + Math.round(series.percent) + "</div>";
}

function columnCount(tableName) {
    var colCount = 0;

    $('#' + tableName + ' tr:nth-child(1) th').each(function () {
        if ($(this).attr('colspan')) {
            colCount += +$(this).attr('colspan');
        } else {
            colCount++;
        }
    });
    return colCount;
}


function flotPie(targetDiv, sourceTable, hideSource) {
    /// <summary>Generate a pie chart using flot</summary>
    /// <param name="targetDiv" optional="false" type="string">
    ///     ID of the DIV tag that the pie chart will be drawn into
    /// </param>
    /// <param name="sourceTable" optional="false" type="string">
    ///     ID of the table that the data will be sourced from
    /// </param>
    /// <param name="hideSource" optional="true" type="boolean">
    ///     Set to true if you want the sourceTable to be hidden, will also hide any IDs that start with the sourceTable prefix
    ///     e.g. if table ID is pieData and it is in a div called pieDataContainer, both will be hidden
    /// </param>

    var dataSet = [];

    $("#" + sourceTable + " tr").each(function () {
        var tmp = {};
        tmp.label = $("th", this).text() + ' (' + parseInt($("td", this).text(), 10) +')';
        tmp.data = parseInt($("td", this).text(), 10);
        dataSet.push(tmp);
    });
    if (hideSource == true) {
        $('[id^="' + sourceTable + '"]').hide();
    }
    $.plot('#' + targetDiv, dataSet, {
        series: {
            pie: {
                show: true,
                label: {
                    show: false,
                    radius: 1,
                    formatter: labelFormatter,
                    threshold: 0.01,
                    background: {
                        opacity: 0.5,
                        color: '#000'
                    }
                }
            }
        },
        legend: {
            show: true
        },
        grid: {
            hoverable: true
        }
    });
    $('#' + targetDiv).addClass("ssg_pie_chart");
}

function flotBar(targetDiv, sourceTable, hideSource) {
    /// <summary>Generate a bar graph using flot</summary>
    /// <param name="targetDiv" optional="false" type="string">
    ///     ID of the DIV tag that the pie chart will be drawn into
    /// </param>
    /// <param name="sourceTable" optional="false" type="string">
    ///     ID of the table that the data will be sourced from
    /// </param>
    /// <param name="hideSource" optional="true" type="boolean">
    ///     Set to true if you want the sourceTable to be hidden, will also hide any IDs that start with the sourceTable prefix
    ///     e.g. if table ID is pieData and it is in a div called pieDataContainer, both will be hidden
    /// </param>

    var barData = [];
    var ccc = columnCount(sourceTable);

    for (var i = 2; i < ccc; i++) { //each column
        var row = {};
        var data = [];
        var key = "";
        $("#" + sourceTable + " tr > :nth-child(" + i + ")").each(function () {//:gt(0) = skip header row
            //dostuffhere
            key = $(this).data('key');
            var value = parseInt($(this).text(), 10);
            if (value == 0) {
                value = "";
            }
            var type = $(this).data('type');
            var item = [];
            item.push(type);
            item.push(value);
            if (key != undefined) {
                data.push(item);
            }
        });
        row.label = key;
        row.data = data;
        barData.push(row);
    }
    
    if (hideSource == true) {
        $('[id^="' + sourceTable + '"]').hide();
    }
    $.plot('#' + targetDiv, barData, {
		series: {
			bars: {
				show: true,
				barWidth: 0.10,
				lineWidth: 0,
				order: 1, // Include this line to ensure bars will appear side by side.
				fillColor: {
					colors: [{
						opacity: 1
					}, {
						opacity: 0.7
					}]
				}
			}
		},
		xaxis: {
			mode: "categories"
		},
		grid: {
			hoverable: true,
			borderWidth: 0
		},
		colors: ["#058DC7", "#BE0E24", "#C6AF0F", "#4DA74D" ]
	});
    
}