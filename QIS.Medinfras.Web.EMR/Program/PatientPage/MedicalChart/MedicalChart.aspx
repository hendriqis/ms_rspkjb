<%@ Page Language="C#" MasterPageFile="~/MasterPage/MPPatientPageList.master" AutoEventWireup="true" 
    CodeBehind="MedicalChart.aspx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.MedicalChart" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <link rel="stylesheet" type="text/css" href='<%= ResolveUrl("~/Libs/Scripts/jquery/jqplot/jquery.jqplot.min.css")%>' /> 
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/jquery/jqplot/jquery.jqplot.min.js")%>'></script>
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/jquery/jqplot/plugins/jqplot.highlighter.min.js")%>'></script>
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/jquery/jqplot/plugins/jqplot.blockRenderer.min.js")%>'></script>
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/jquery/jqplot/plugins/jqplot.enhancedLegendRenderer.min.js")%>'></script>
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/jquery/jqplot/plugins/jqplot.pointLabels.min.js")%>'></script>
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/jquery/jqplot/plugins/jqplot.cursor.min.js")%>'></script>
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/jquery/jqplot/plugins/jqplot.dateAxisRenderer.min.js")%>'></script>

    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.ui.core.js")%>'></script>
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.ui.widget2.js")%>'></script>
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.ui.mouse.js")%>'></script>
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.ui.draggable.js")%>'></script>
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.ui.droppable.js")%>'></script>
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.effects.core.js")%>'></script>

    <script type="text/javascript" id="dxss_chartctl">
        $(function () {
            //#region DragImage
            var totalContentHeight = 0; // Total Height daerah image yang sudah diolah
            var fadeSpeed = 200; // Speed Fading List Image
            var animSpeed = 600; //ease amount
            var easeType = 'easeOutCirc'; //tipe animasi

            /* Set Height Scroller Image Yang Sudah Diolah */
            sliderHeight = $('#tdDraggableImage').css('height').replace("px", "");
            $('#draggableScroller').css('height', sliderHeight);

            $('#draggableScroller  .content').draggable({
                helper: 'clone',
                appendTo: 'body',
                zIndex: 10000
            });
            $('#chart').droppable({
                drop: function (event, ui) {
                    var $clone = ui.helper.clone();
                    var idxImage = $clone.find('.indexVal').val();
                    if (visibleChart != '') {
                        var listVisibleChart = visibleChart.split('|');
                        if (listVisibleChart.indexOf(idxImage.toString()) < 0) {
                            visibleChart += '|' + idxImage;
                            CreateChart();
                        }
                    }
                    else {
                        visibleChart += idxImage;
                        CreateChart();
                    }
                }
            });


            function GetTotalContentHeight() {
                totalContentHeight = 0;
                $('#draggableScroller .content').each(function () {
                    totalContentHeight += $(this).innerHeight();
                    totalContentHeight += 9;
                    $('#draggableScroller .imgContainer').css('height', totalContentHeight);
                });
            }

            //#region Image
            $('#draggableScroller').mousemove(OnDraggableScrollerMouseMove);
            function OnDraggableScrollerMouseMove(e) {
                if ($('#draggableScroller .imgContainer').height() > sliderHeight) {
                    var mouseCoords = (e.pageY - this.offsetTop) - 250;
                    var mousePercentY = mouseCoords / sliderHeight;
                    var destY = -(((totalContentHeight - (sliderHeight)) - sliderHeight) * (mousePercentY));
                    var thePosA = mouseCoords - destY;
                    var thePosB = destY - mouseCoords;
                    if (mouseCoords == destY) {
                        $('#draggableScroller .imgContainer').stop();
                    }
                    else if (mouseCoords > destY) {
                        //$('#thumbScroller .container').css('left',-thePosA); //without easing
                        $('#draggableScroller .imgContainer').stop().animate({ top: -thePosA }, animSpeed, easeType); //with easing
                    }
                    else if (mouseCoords < destY) {
                        //$('#thumbScroller .container').css('left',thePosB); //without easing
                        $('#draggableScroller .imgContainer').stop().animate({ top: thePosB }, animSpeed, easeType); //with easing
                    }
                }
            }

            function createThumbChart() {
                var ctrThumbChart = 0;
                $('#draggableScroller  .content').each(function () {
                    var idChart = $(this).find('.idChart').val();
                    var seriesData = [];
                    var seriesColors = [];
                    var seriesLabel = new Array(1);
                    var thumbChartData = allThumbChartData[ctrThumbChart].split('^'); // Title^yMin^yMax
                    seriesData.push(allSeriesData[ctrThumbChart]);
                    seriesColors.push(listColor[ctrThumbChart]);
                    seriesLabel[0] = { label: allSeriesLabel[ctrThumbChart] };
                    //var line1=[['2008-06-30 8:00AM',4], ['2008-7-30 8:00AM',6.5], ['2008-8-30 8:00AM',5.7], ['2008-9-30 8:00AM',9], ['2008-10-30 8:00AM',8.2]];
                    
                    $.jqplot(idChart, seriesData, ThumbChartOption(thumbChartData[0], xLabel, yLabel, seriesLabel, seriesColors));
                    
                    ctrThumbChart++;
                });

                $('#draggableScroller  .content').each(function () {
                    $(this).fadeTo(fadeSpeed, 0.6);
                });
                $('#draggableScroller .content').hover(
		        function () { //mouse over
		            $(this).fadeTo(fadeSpeed, 1);
		        },
		        function () { //mouse out
		            $(this).fadeTo(fadeSpeed, 0.6);
		        }
	        );

                GetTotalContentHeight();
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

            function CreateChart() {
                if (visibleChart != '') {
                    var yMin = 999999;
                    var yMax = -999999;
                    var seriesData = [];
                    var seriesColors = [];
                    var listVisibleChart = visibleChart.split('|');
                    for (var i = 0; i < listVisibleChart.length; ++i) {
                        var thumbChartData = allThumbChartData[listVisibleChart[i]].split('^'); // Title^yMin^yMax
                        var yMinThisSeries = parseFloat(thumbChartData[1]);
                        var yMaxThisSeries = parseFloat(thumbChartData[2]);
                        if (yMinThisSeries < yMin)
                            yMin = yMinThisSeries;
                        if (yMaxThisSeries > yMax)
                            yMax = yMaxThisSeries;
                    }

                    var rasioTotal = yMax - yMin;
                    for (var i = 0; i < listVisibleChart.length; ++i) {
                        var thumbChartData = allThumbChartData[listVisibleChart[i]].split('^'); // Title^yMin^yMax
                        var rasioThisSeries = thumbChartData[2] - thumbChartData[1];
                        for (var j = 0; j < allSeriesData[listVisibleChart[i]].length; ++j) {
                            allSeriesData[listVisibleChart[i]][j][1] = parseFloat(((allSeriesData[listVisibleChart[i]][j][2] - thumbChartData[1]) / rasioThisSeries * rasioTotal) + yMin);
                        }
                        seriesData.push(allSeriesData[listVisibleChart[i]]);
                        seriesColors.push(listColor[listVisibleChart[i]]);
                    }
                    CreateListCheckBox();
                    $.jqplot('chart', seriesData, ChartOption(title, xLabel, yLabel, getLabels(allSeriesLabel), seriesColors)).replot();
                }
            }

            function CreateListCheckBox() {
                var container = document.getElementById('containerCheckbox');
                while (container.hasChildNodes()) {
                    container.removeChild(container.lastChild);
                }
                var listVisibleChart = visibleChart.split('|');
                for (var i = 0; i < listVisibleChart.length; ++i) {
                    container.appendChild(CreateCheckBox(allSeriesLabel[listVisibleChart[i]], listVisibleChart[i]));
                }

            }

            function CreateCheckBox(text, idx) {
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
                    CreateChart();
                });

                var label = document.createElement('label')
                label.htmlFor = "id";
                label.appendChild(document.createTextNode(text));

                container.appendChild(checkbox);
                container.appendChild(label);

                return container;
            }

            function getLabels(allSeriesLabel) {
                var listVisibleChart = visibleChart.split('|');
                var arr = new Array(listVisibleChart.length);
                for (var i = 0; i < listVisibleChart.length; ++i) {
                    arr[i] = { label: allSeriesLabel[listVisibleChart[i]] };
                }
                return arr;
            };

            function ThumbChartOption(title, xLabel, yLabel, seriesLabel, seriesColors) {
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

            function ChartOption(title, xLabel, yLabel, seriesLabel, seriesColors) {
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

            function initializeChart(chartData) {
                var param = chartData.split('|');

                allThumbChartData = param[0].split(';');

                title = param[1];
                xLabel = param[2];
                yLabel = param[3];

                allSeriesLabel = param[4].split(';');
                allSeriesData = eval(param[5]);
                CreateChart();
                createThumbChart();
            }
            var chartData = "<%=chartData%>";
            setTimeout(function () {
                initializeChart(chartData);
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
        #draggableScroller
        {
            height:100%;
            overflow: hidden;
        }
        #draggableScroller .imgContainer
        {
            position:relative;
	        left:0;
        }
        #draggableScroller .content{
            vertical-align:middle;
            text-align:center;
            margin:2px 0;
        }
        div.thumbChart{
	        border:3px solid #fff;
	        width:140px;
	        height:100px;
            margin:0 auto;
            cursor: pointer;
        }
        #draggableScroller a{
	        padding:2px;
	        outline:none;
        }
        .containerDropZone{width:100%;margin:0 auto;padding-top:10px;text-align:center;}
        .clear{clear:both;}
    </style>

    <div class="container" style="height:540px">
        <table style="width:100%;height:auto;border-collapse:collapse;" cellpadding="0" cellspacing="0" class="boxShadow">
            <tr>
                <td style="min-width:150px;width:150px;height:520px;border:1px solid #EAEAEA" id="tdDraggableImage">
                    <div id="draggableOuterContainer">
			            <div id="draggableScroller">
				            <div class="imgContainer" id="imgContainer" runat="server">
                            </div>
			            </div>
		            </div>
                </td>
                <td style="text-align:center;vertical-align:top;width:auto;border:1px solid #EAEAEA;">            
                    <div class="containerDropZone">
                        <center>
                        <table>
                            <tr>
                                <td style="text-align:right;">
                                    <div id="chart" style="height:500px;width:800px;border:1px solid #EAEAEA;float:left;" class="boxShadow"></div>                            
                                </td>
                                <td style="text-align:left;vertical-align:top;width:100px;">
                                    <div id="containerCheckbox">
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


</asp:Content>
