<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MedicalChartCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.EMR.Program.MedicalChartCtl" %>

<link rel="stylesheet" type="text/css" href='<%= ResolveUrl("~/Libs/Scripts/jquery/jqplot/jquery.jqplot.min.css")%>' /> 
<script id="dxis_medicalchartctl1" type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/jquery/jqplot/jquery.jqplot.min.js")%>'></script>
<script id="dxis_medicalchartctl2" type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/jquery/jqplot/plugins/jqplot.highlighter.min.js")%>'></script>
<script id="dxis_medicalchartctl3" type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/jquery/jqplot/plugins/jqplot.blockRenderer.min.js")%>'></script>
<script id="dxis_medicalchartctl4" type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/jquery/jqplot/plugins/jqplot.enhancedLegendRenderer.min.js")%>'></script>
<script id="dxis_medicalchartctl5" type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/jquery/jqplot/plugins/jqplot.pointLabels.min.js")%>'></script>
<script id="dxis_medicalchartctl6" type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/jquery/jqplot/plugins/jqplot.cursor.min.js")%>'></script>
<script id="dxis_medicalchartctl7" type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/jquery/jqplot/plugins/jqplot.dateAxisRenderer.min.js")%>'></script>

<script id="dxis_medicalchartctl8" type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.ui.core.js")%>'></script>
<script id="dxis_medicalchartctl9" type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.ui.widget2.js")%>'></script>
<script id="dxis_medicalchartctl10" type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.ui.mouse.js")%>'></script>
<script id="dxis_medicalchartctl11" type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.ui.draggable.js")%>'></script>
<script id="dxis_medicalchartctl12" type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.ui.droppable.js")%>'></script>
<script id="dxis_medicalchartctl13" type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.effects.core.js")%>'></script>

<script type="text/javascript" id="dxss_chartctl">
    $(function () {
        //#region DragImage
        var charmBarTotalContentHeight = 0; // Total Height daerah image yang sudah diolah
        var charmBarFadeSpeed = 200; // Speed Fading List Image
        var charmBarAnimSpeed = 600; //ease amount
        var charmBarEaseType = 'easeOutCirc'; //tipe animasi

        /* Set Height Scroller Image Yang Sudah Diolah */
        sliderHeight = $('#tdcharmBarDraggableImage').css('height').replace("px", "");
        $('#charmBarDraggableScroller').css('height', sliderHeight);

        $('#charmBarDraggableScroller  .content').draggable({
            helper: 'clone',
            appendTo: 'body',
            zIndex: 20005
        });
        $('#charmBarChart').droppable({
            drop: function (event, ui) {
                var $clone = ui.helper.clone();
                var idxImage = $clone.find('.indexVal').val();
                if (visibleChart != '') {
                    var listVisibleChart = visibleChart.split('|');
                    if (listVisibleChart.indexOf(idxImage.toString()) < 0) {
                        visibleChart += '|' + idxImage;
                        CharmBarCreateChart();
                    }
                }
                else {
                    visibleChart += idxImage;
                    CharmBarCreateChart();
                }
            }
        });


        function CharmBarGetTotalContentHeight() {
            charmBarTotalContentHeight = 0;
            $('#charmBarDraggableScroller .content').each(function () {
                charmBarTotalContentHeight += $(this).innerHeight();
                charmBarTotalContentHeight += 9;
                $('#charmBarDraggableScroller .charmBarImgContainer').css('height', charmBarTotalContentHeight);
            });
        }

        //#region Image
        $('#charmBarDraggableScroller').mousemove(OnDraggableScrollerMouseMove);
        function OnDraggableScrollerMouseMove(e) {
            if ($('#charmBarDraggableScroller .charmBarImgContainer').height() > sliderHeight) {
                var mouseCoords = (e.pageY - this.offsetTop) - 250;
                var mousePercentY = mouseCoords / sliderHeight;
                var destY = -(((charmBarTotalContentHeight - (sliderHeight)) - sliderHeight) * (mousePercentY));
                var thePosA = mouseCoords - destY;
                var thePosB = destY - mouseCoords;
                if (mouseCoords == destY) {
                    $('#charmBarDraggableScroller .charmBarImgContainer').stop();
                }
                else if (mouseCoords > destY) {
                    //$('#thumbScroller .container').css('left',-thePosA); //without easing
                    $('#charmBarDraggableScroller .charmBarImgContainer').stop().animate({ top: -thePosA }, charmBarAnimSpeed, charmBarEaseType); //with easing
                }
                else if (mouseCoords < destY) {
                    //$('#thumbScroller .container').css('left',thePosB); //without easing
                    $('#charmBarDraggableScroller .charmBarImgContainer').stop().animate({ top: thePosB }, charmBarAnimSpeed, charmBarEaseType); //with easing
                }
            }
        }

        function CharmBarCreateThumbChart() {
            var ctrThumbChart = 0;
            $('#charmBarDraggableScroller  .content').each(function () {
                var idChart = $(this).find('.idChart').val();
                var seriesData = [];
                var seriesColors = [];
                var seriesLabel = new Array(1);
                var charmBarThumbChartData = allThumbChartData[ctrThumbChart].split('^'); // Title^yMin^yMax
                seriesData.push(allSeriesData[ctrThumbChart]);
                seriesColors.push(listColor[ctrThumbChart]);
                seriesLabel[0] = { label: allSeriesLabel[ctrThumbChart] };
                //var line1=[['2008-06-30 8:00AM',4], ['2008-7-30 8:00AM',6.5], ['2008-8-30 8:00AM',5.7], ['2008-9-30 8:00AM',9], ['2008-10-30 8:00AM',8.2]];
                    
                $.jqplot(idChart, seriesData, CharmBarThumbChartOption(charmBarThumbChartData[0], xLabel, yLabel, seriesLabel, seriesColors));
                    
                ctrThumbChart++;
            });

            $('#charmBarDraggableScroller  .content').each(function () {
                $(this).fadeTo(charmBarFadeSpeed, 0.6);
            });
            $('#charmBarDraggableScroller .content').hover(
		    function () { //mouse over
		        $(this).fadeTo(charmBarFadeSpeed, 1);
		    },
		    function () { //mouse out
		        $(this).fadeTo(charmBarFadeSpeed, 0.6);
		    }
	    );

            CharmBarGetTotalContentHeight();
        }

        //#endregion
        //#endregion

        //#region Chart
        var allThumbChartData = [];
        var title;
        var xLabel;
        var yLabel;
        var allSeriesData = [];
        var allSeriesLabel = [];
        var visibleChart = '';
        var listColor = ["#4bb2c5", "#c5b47f", "#EAA228", "#579575", "#839557", "#958c12", "#953579", "#4b5de4", "#d8b83f", "#ff5800", "#0085cc"]

        function CharmBarCreateChart() {
            if (visibleChart != '') {
                var yMin = 999999;
                var yMax = -999999;
                var seriesData = [];
                var seriesColors = [];
                var listVisibleChart = visibleChart.split('|');
                for (var i = 0; i < listVisibleChart.length; ++i) {
                    var charmBarThumbChartData = allThumbChartData[listVisibleChart[i]].split('^'); // Title^yMin^yMax
                    var yMinThisSeries = parseFloat(charmBarThumbChartData[1]);
                    var yMaxThisSeries = parseFloat(charmBarThumbChartData[2]);
                    if (yMinThisSeries < yMin)
                        yMin = yMinThisSeries;
                    if (yMaxThisSeries > yMax)
                        yMax = yMaxThisSeries;
                }

                var rasioTotal = yMax - yMin;
                for (var i = 0; i < listVisibleChart.length; ++i) {
                    var charmBarThumbChartData = allThumbChartData[listVisibleChart[i]].split('^'); // Title^yMin^yMax
                    var rasioThisSeries = charmBarThumbChartData[2] - charmBarThumbChartData[1];
                    for (var j = 0; j < allSeriesData[listVisibleChart[i]].length; ++j) {
                        allSeriesData[listVisibleChart[i]][j][1] = parseFloat(((allSeriesData[listVisibleChart[i]][j][2] - charmBarThumbChartData[1]) / rasioThisSeries * rasioTotal) + yMin);
                    }
                    seriesData.push(allSeriesData[listVisibleChart[i]]);
                    seriesColors.push(listColor[listVisibleChart[i]]);
                }
                CharmBarCreateListCheckBox();
                $.jqplot('charmBarChart', seriesData, CharmBarChartOption(title, xLabel, yLabel, CharmBarGetLabels(allSeriesLabel), seriesColors)).replot();
            }
        }

        function CharmBarCreateListCheckBox() {
            var container = document.getElementById('charmBarContainerCheckbox');
            while (container.hasChildNodes()) {
                container.removeChild(container.lastChild);
            }
            var listVisibleChart = visibleChart.split('|');
            for (var i = 0; i < listVisibleChart.length; ++i) {
                container.appendChild(CharmBarCreateCheckBox(allSeriesLabel[listVisibleChart[i]], listVisibleChart[i]));
            }

        }

        function CharmBarCreateCheckBox(text, idx) {
            var container = document.createElement('div');

            var checkbox = document.createElement('input');
            checkbox.type = "checkbox";
            checkbox.name = "name";
            checkbox.value = "value";
            checkbox.id = "id";
            $(checkbox).click({ idx: idx }, function (evt) {
                var idx = evt.data.idx;
                var listVisibleChart = visibleChart.split('|');
                var newVisibleChart = '';
                for (var i = 0; i < listVisibleChart.length; ++i) {
                    if (parseInt(listVisibleChart[i]) == idx) {
                        continue;
                    }
                    if (newVisibleChart != '')
                        newVisibleChart += '|';
                    newVisibleChart += listVisibleChart[i];
                }
                visibleChart = newVisibleChart;
                CharmBarCreateChart();
            });

            var label = document.createElement('label')
            label.htmlFor = "id";
            label.appendChild(document.createTextNode(text));

            container.appendChild(checkbox);
            container.appendChild(label);

            return container;
        }

        function CharmBarGetLabels(allSeriesLabel) {
            var listVisibleChart = visibleChart.split('|');
            var arr = new Array(listVisibleChart.length);
            for (var i = 0; i < listVisibleChart.length; ++i) {
                arr[i] = { label: allSeriesLabel[listVisibleChart[i]] };
            }
            return arr;
        };

        function CharmBarThumbChartOption(title, xLabel, yLabel, seriesLabel, seriesColors) {
            var optionsObj = {
                seriesColors: seriesColors,
                series: seriesLabel,
                height: 140,
                width: 140,
                seriesDefaults: {
                    showMarker: false
                },
                highlighter: {
                    show: true,
                    showLabel: true,
                    tooltipAxes: 'y',
                    sizeAdjust: 2.5, tooltipLocation: 'ne'
                },
                title: {
                    text: title,
                    show: true,
                    fontFamily: 'Georgia, Serif',
                    fontSize: '8pt',
                    textColor: '#FFFFFF'
                },
                axes: {
                    xaxis: {
                        renderer:$.jqplot.DateAxisRenderer
                    },
                }
            };
            return optionsObj;
        }

        function CharmBarChartOption(title, xLabel, yLabel, seriesLabel, seriesColors) {
            var optionsObj = {
                title: title,
                seriesColors: seriesColors,
                series: seriesLabel,
                legend: {
                    renderer: $.jqplot.EnhancedLegendRenderer,
                    show: true
                },
                seriesDefaults: {
                    showMarker: false,
                    pointLabels: { show: true }
                },
                axes: {
                    xaxis: {
                        renderer:$.jqplot.DateAxisRenderer
                    },
                    yaxis: {
                        label: yLabel,
                        labelRenderer: $.jqplot.CanvasAxisLabelRenderer,
                        labelOptions: {
                            fontFamily: 'Georgia, Serif',
                            fontSize: '12pt'
                        }
                    }
                },
                cursor: {
                    show: true,
                    zoom: true
                },
                highlighter: {
                    show: true,
                    showLabel: true,
                    tooltipAxes: 'y',
                    sizeAdjust: 7.5, tooltipLocation: 'ne',
                    yvalues: 2,
                    formatString: '<div><div style="display:none;">%s</div><div>%s</div></div>'

                }
            };
            return optionsObj;
        }
        //#endregion

        function CharmBarInitializeChart(chartData) {
            var param = chartData.split('|');

            allThumbChartData = param[0].split(';');

            title = param[1];
            xLabel = param[2];
            yLabel = param[3];

            allSeriesLabel = param[4].split(';');
            allSeriesData = eval(param[5]);
            CharmBarCreateChart();
            CharmBarCreateThumbChart();
        }
        var chartData = "<%=chartData%>";
        setTimeout(function () {
            CharmBarInitializeChart(chartData);
        }, 0);
    });
</script>

<style>
    
    #draggableOuterContainer
    {
        background-color:#555; 
        opacity: 0.65;
	    padding:0;
	    height:100%;
    }
    #charmBarDraggableScroller
    {
        height:100%;
        overflow: hidden;
    }
    #charmBarDraggableScroller .charmBarImgContainer
    {
        position:relative;
	    left:0;
    }
    #charmBarDraggableScroller .content{
        vertical-align:middle;
        text-align:center;
        margin:2px 0;
    }
    div.charmBarThumbChart{
	    border:3px solid #fff;
	    width:140px;
	    height:100px;
        margin:0 auto;
        cursor: pointer;
    }
    #charmBarDraggableScroller a{
	    padding:2px;
	    outline:none;
    }
    .charmBarContainerDropZone{width:100%;margin:0 auto;padding-top:10px;text-align:center;}
    .clear{clear:both;}
</style>

<div class="container" style="height:540px">
    <table style="width:100%;height:auto;border-collapse:collapse;" cellpadding="0" cellspacing="0" class="boxShadow">
        <tr>
            <td style="min-width:150px;width:150px;height:520px;border:1px solid #EAEAEA" id="tdcharmBarDraggableImage">
                <div id="draggableOuterContainer">
			        <div id="charmBarDraggableScroller">
				        <div class="charmBarImgContainer" id="charmBarImgContainer" runat="server">
                        </div>
			        </div>
		        </div>
            </td>
            <td style="text-align:center;vertical-align:top;width:auto;border:1px solid #EAEAEA;">            
                <div class="charmBarContainerDropZone">
                    <center>
                    <table>
                        <tr>
                            <td style="text-align:right;">
                                <div id="charmBarChart" style="height:500px;width:800px;border:1px solid #EAEAEA;float:left;" class="boxShadow"></div>                            
                            </td>
                            <td style="text-align:left;vertical-align:top;width:100px;">
                                <div id="charmBarContainerCheckbox">
                                </div>
                            </td>
                        </tr>
                    </table>
                    </center>
                </div>
            </td>
        </tr>
    </table>
</div>