<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EpisodeSummaryMedicalChartContainerCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.EpisodeSummaryMedicalChartContainerCtl" %>

<link rel="stylesheet" type="text/css" href='<%= ResolveUrl("~/Libs/Scripts/jquery/jqplot/jquery.jqplot.min.css")%>' />
<script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/jquery/jqplot/jquery.jqplot.min.js")%>'></script>
<script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/jquery/jqplot/plugins/jqplot.highlighter.min.js")%>'></script>
<script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/jquery/jqplot/plugins/jqplot.blockRenderer.min.js")%>'></script>
<script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/jquery/jqplot/plugins/jqplot.enhancedLegendRenderer.min.js")%>'></script>
<script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/jquery/jqplot/plugins/jqplot.pointLabels.min.js")%>'></script>
<script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/jquery/jqplot/plugins/jqplot.cursor.min.js")%>'></script>
<script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/jquery/jqplot/plugins/jqplot.dateAxisRenderer.min.js")%>'></script>
<script type="text/javascript" id="dxss_episodesummaryctl">
    $(function () {
        $('#btnMedicalChartContainerPrev').click(function () {
            var idx = $('#<%=hdnChartIdx.ClientID %>').val();
            idx--;
            if (idx < 0)
                idx = allSeriesData.length - 1;
            $('#<%=hdnChartIdx.ClientID %>').val(idx);
            episodeSummaryMedicalChartLoadControl();
        });
        $('#btnMedicalChartContainerNext').click(function () {
            var idx = $('#<%=hdnChartIdx.ClientID %>').val();
            idx++;
            if (idx == allSeriesData.length)
                idx = 0;
            $('#<%=hdnChartIdx.ClientID %>').val(idx);
            episodeSummaryMedicalChartLoadControl();
        });
    });

    function episodeSummaryMedicalChartLoadControl() {
        if ($('#<%=hdnChartIdx.ClientID %>').val() != '') {
            $('#episodeSummaryChart').html('');
            var idx = $('#<%=hdnChartIdx.ClientID %>').val();
            CreateChart(idx);
        }
    }

    var listColor = ["#4bb2c5", "#c5b47f", "#EAA228", "#579575", "#839557", "#958c12", "#953579", "#4b5de4", "#d8b83f", "#ff5800", "#0085cc"];
    var allSeriesLabel = [];
    var allSeriesData = [];
    function CreateChart(idx) {
        var seriesData = [];
        var seriesColors = [];
        var seriesLabel = new Array(1);
        var thumbChartData = allThumbChartData[idx].split('^'); // Title^yMin^yMax
        seriesData.push(allSeriesData[idx]);
        seriesColors.push(listColor[idx]);
        seriesLabel[0] = { label: allSeriesLabel[idx] };

        var min = '2099-12-12';
        var max = '1000-01-01';
        for (var i = 0; i < seriesData[0].length; ++i) {
            if (min > seriesData[0][i][0])
                min = seriesData[0][i][0];
            if (max < seriesData[0][i][0])
                max = seriesData[0][i][0];
        }

        $.jqplot('episodeSummaryChart', seriesData, ThumbChartOption(thumbChartData[0], seriesLabel, seriesColors, min, max));

    }

    function initializeChart(chartData) {
        var param = chartData.split('|');

        allThumbChartData = param[0].split(';');

        title = param[1];
        xLabel = param[2];
        yLabel = param[3];

        allSeriesLabel = param[4].split(';');

        allSeriesData = eval(param[5]);
        episodeSummaryMedicalChartLoadControl();
    }

    $(function () {
        var chartData = "<%=chartData%>";
        setTimeout(function () {
            initializeChart(chartData);
        }, 0);
    });

    function ThumbChartOption(title, seriesLabel, seriesColors, min, max) {
        var optionsObj = {
            title: title,
            seriesColors: seriesColors,
            series: seriesLabel,
            seriesDefaults: {
                showMarker: true,
                pointLabels: { show: true },
                rendererOptions: { smooth: true }
            },
            axes: {
                xaxis: {
                    pad: 0,
                    renderer: $.jqplot.DateAxisRenderer,
                    min: min,
                    max: max,
                    tickOptions: { showGridline: false, formatString: '%d/%m' }
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
                sizeAdjust: 7, tooltipLocation: 'ne',
                yvalues: 2,
                formatString: '<div><div style="display:none;">%s</div><div>%s</div></div>'

            }
        };
        return optionsObj;
    }
</script>
<input type="hidden" value="0" runat="server" id="hdnChartIdx" />
<table style="float: right;margin-top: 0px;margin-right: 15px">
    <tr>
        <td><img src='<%=ResolveUrl("~/Libs/Images/Icon/prev_record.png") %>' title="Prev" width="25px" alt="" class="imgLink" id="btnMedicalChartContainerPrev" style="margin-left: 5px;" /></td>
        <td><img src='<%=ResolveUrl("~/Libs/Images/Icon/next_record.png") %>' title="Next" width="25px" alt="" class="imgLink" id="btnMedicalChartContainerNext" style="margin-left: 5px;" /></td>
    </tr>
</table>
<h3 class="headerContent" style="padding-left:5px;"><%=GetLabel("Medical Chart")%></h3>
<div style="clear:both;width:100%; height:100%; padding:5px;" class="borderBox" id="containerEpisodeSummaryMedicalChartCtn">
    <div id="episodeSummaryChart" style="height:360px;width:650px;border:1px solid;float:left;" class="boxShadow"></div> 

</div>