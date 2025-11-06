<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CDCGrowthChartCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.EMR.Program.CDCGrowthChartCtl" %>
    
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>   

<link rel="stylesheet" type="text/css" href='<%= ResolveUrl("~/Libs/Scripts/jquery/jqplot/jquery.jqplot.min.css")%>' /> 
<script id="dxis_cdcgrowthchartctl1" type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/jquery/jqplot/jquery.jqplot.min.js")%>'></script>
<script id="dxis_cdcgrowthchartctl2" type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/jquery/jqplot/plugins/jqplot.highlighter.min.js")%>'></script>
<script id="dxis_cdcgrowthchartctl3" type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/jquery/jqplot/plugins/jqplot.blockRenderer.min.js")%>'></script>
<script id="dxis_cdcgrowthchartctl4" type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/jquery/jqplot/plugins/jqplot.enhancedLegendRenderer.min.js")%>'></script>
<script id="dxis_cdcgrowthchartctl5" type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/jquery/jqplot/plugins/jqplot.pointLabels.min.js")%>'></script>
<script id="dxis_cdcgrowthchartctl6" type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/jquery/jqplot/plugins/jqplot.cursor.min.js")%>'></script>
<script type="text/javascript">
    function charmBarInitializeChart(chartData) {
        if (chartData.Series) {
            var title = chartData.Title;
            var xLabel = chartData.XLabel;
            var yLabel = chartData.YLabel;
            var seriesData = [];
            var seriesLabel = [];
            var actualPoint = chartData.ActualPoint;

            seriesLabel = chartData.Legend.split(';');
            seriesData = eval(chartData.Series);

            if (actualPoint != '')
                seriesData.push(eval(actualPoint));

            var plot = $.jqplot('charmBarChart', seriesData, charmBarCreateChart(title, xLabel, yLabel, seriesLabel)).replot();
        }
        else {
            $('#charmBarChart').html('');
        }
    }

    function charmBarGetLabels(seriesLabel) {
        var arr = new Array(seriesLabel.length + 1);
        for (i = 0; i < seriesLabel.length; i++) {
            arr[i] = { label: seriesLabel[i], showLabel: true, lineWidth: 1 };
        }
        arr[seriesLabel.length] = { lineWidth: 4, showLabel: false, pointLabels: { show: true} };
        return arr;
    };

    function charmBarCreateChart(title, xLabel, yLabel, seriesLabel) {
        var ageGroup = $("#<%=rblAgeGroup.ClientID%> input:checked").val();
        var min = 0;
        var max = 0;
        var tickInterval = 0;
        if (ageGroup == '20Y') {
            min = 24;
            max = 264;
            tickInterval = 24;
        }
        else {
            min = 0;
            max = 42;
            tickInterval = 3;
        }
        var optionsObj = {
            animate: true,
            title: title,
            series: charmBarGetLabels(seriesLabel),
            legend: {
                renderer: $.jqplot.EnhancedLegendRenderer,
                show: true
            },
            seriesDefaults: {
                showMarker: false,
                rendererOptions: {
                    showDataLabels: true
                }
            },
            axes: {
                xaxis: {
                    label: xLabel,
                    labelRenderer: $.jqplot.CanvasAxisLabelRenderer,
                    labelOptions: {
                        fontFamily: 'Georgia, Serif',
                        fontSize: '12pt'
                    },
                    tickInterval: tickInterval,
                    min: min,
                    max: max
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
                sizeAdjust: 7.5, tooltipLocation: 'ne'
            }
        };
        return optionsObj;
    }

    $('#btnSaveCDCGrowtChartCtl').bind('click', { chart: $('div.jqplot-target') }, function (evt) {
        var imgelem = evt.data.chart.jqplotToImageElem();
        var image = $(imgelem).attr('src');
        image = image.replace('data:image/png;base64,', '');
        var url = '<%= ResolveUrl("~/Libs/Service/UploadService.asmx/UploadChartImage")%>';
        $.ajax({
            async: false,
            type: 'POST',
            url: url,
            data: '{ "imageData" : "' + image + '" }',
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            error: function (msg) {
                showToast('Failed', msg.responseText);
            },
            success: function (msg) {
                
            }
        });

        evt.preventDefault();
    });

    function charmBarLoadChart() {
        setTimeout(function () {
            showLoadingPanel();
        }, 0);
        var seriesType = $('#<%=ddlChartType.ClientID %>').val();
        var ageGroup = $('#<%=rblAgeGroup.ClientID %> input:checked').val();
        var gender = $('#<%=hdnGender.ClientID %>').val();
        $.ajax({
            // have to use synchronous here, else returns before data is fetched
            async: true,
            type: 'POST',
            url: ResolveUrl('~/Program/PatientPage/CharmBar/MedicalChart/CDCGrowthChartService.asmx/GetGrowthChartData'),
            contentType: 'application/json; charset=utf-8',
            data: '{ "seriesType" : "' + seriesType + '", "ageGroup" : "' + ageGroup + '", "gender" : "' + gender + '"}',
            dataType: 'json',
            error: function (msg) {
                showToast('Failed', msg.responseText);
                hideLoadingPanel();
            },
            success: function (msg) {
                charmBarInitializeChart(msg.d);
                setTimeout(function () {
                    hideLoadingPanel();
                }, 0);
            }
        });       //end ajax
    }

    $(function () {
        $('#<%=ddlChartType.ClientID %>').change(function () {
            charmBarLoadChart();
        });
        $('#<%=ddlChartType.ClientID %>').change();
        $("#<%=rblAgeGroup.ClientID%> input").change(function () {
            charmBarLoadChart();
        });
    });

    $(document).unload(function () { $('*').unbind(); });
    
</script>
<input type="hidden" value="" id="hdnGender" runat="server" />
<div>
    <div id="toolbarArea">
        <table id="tblToolbarArea" runat="server" cellpadding="0">
            <tr>
                <td style="width:90px;" >
                    <label><%=GetLabel("Age Group") %></label>
                </td>
                <td style="width:200px;" >
                    <asp:RadioButtonList ID="rblAgeGroup" runat="server" RepeatDirection="Horizontal" >
                        <asp:ListItem Text="36 Months" Value="36M" Selected="True" />
                        <asp:ListItem Text="20 Years" Value="20Y" />
                    </asp:RadioButtonList>
                </td>
                <td style="width:200px;" class="labelColumn">
                    <label class="lblMandatory"><%=GetLabel("Chart Type : ( Percentiles )")%></label>
                </td>
                <td style="width:350px;" >
                    <asp:DropDownList ID="ddlChartType" runat="server" Width="200px" />
                </td>
            </tr>
        </table>
    </div>
    <div class="example-content">
        <div id="charmBarChart" style="width:500px;height:500px"></div>
    </div>
    <input type="button" value="<%=GetLabel("Save") %>" id="btnSaveCDCGrowtChartCtl" style="display:none" />
</div>