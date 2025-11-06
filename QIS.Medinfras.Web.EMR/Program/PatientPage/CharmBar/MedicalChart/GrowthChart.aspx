<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPFrame.Master" AutoEventWireup="true" 
    CodeBehind="GrowthChart.aspx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.GrowthChart" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhMPFrame" runat="server">
    <link rel="stylesheet" type="text/css" href='<%= ResolveUrl("~/Libs/Scripts/jquery/jqplot/jquery.jqplot.min.css")%>' /> 
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/jquery/jqplot/jquery.jqplot.min.js")%>'></script>
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/jquery/jqplot/plugins/jqplot.highlighter.min.js")%>'></script>
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/jquery/jqplot/plugins/jqplot.blockRenderer.min.js")%>'></script>
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/jquery/jqplot/plugins/jqplot.enhancedLegendRenderer.min.js")%>'></script>
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/jquery/jqplot/plugins/jqplot.pointLabels.min.js")%>'></script>
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/jquery/jqplot/plugins/jqplot.cursor.min.js")%>'></script>
    <script type="text/javascript">
        function initializeChart(chartData) {
            var param = chartData.split('|');

            var title = param[0];
            var xLabel = param[1];
            var yLabel = param[2];
            var seriesData = [];
            var seriesLabel = [];
            var actualPoint = param[5];
            var seriesType = param[6];

            seriesLabel = param[3].split(';');
            seriesData = eval(param[4]);

            if (actualPoint != '')
                seriesData.push(eval(actualPoint));

            var plot = $.jqplot('chart', seriesData, CreateChart(title, seriesType,xLabel, yLabel, seriesLabel)).replot();
        }

        function getLabels(seriesLabel) {
            var arr = new Array(seriesLabel.length + 1);
            for (i = 0; i < seriesLabel.length; i++) {
                arr[i] = { label: seriesLabel[i], showLabel: true, lineWidth: 1 };
            }
            arr[seriesLabel.length] = { lineWidth: 4, showLabel: false, pointLabels: { show: true} };
            return arr;
        };

        function CreateChart(title, type, xLabel, yLabel, seriesLabel) {
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
                series: getLabels(seriesLabel),
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

        $('#btnSave').bind('click', { chart: $('div.jqplot-target') }, function (evt) {
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

        $(function () {
            $('#<%=ddlChartType.ClientID %>').change(function () {
                cbpCDCGrowthChartProcess.PerformCallback();
            });
            $('#<%=ddlChartType.ClientID %>').change();
            $("#<%=rblAgeGroup.ClientID%> input").change(function () {
                cbpCDCGrowthChartProcess.PerformCallback();
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

        <dxcp:ASPxCallbackPanel ID="cbpCDCGrowthChartProcess" runat="server" ClientInstanceName="cbpCDCGrowthChartProcess" Height="0px" ShowLoadingPanel="false"
            OnCallback="cbpCDCGrowthChartProcess_Callback" >
            <ClientSideEvents BeginCallback="function(s,e){
                showLoadingPanel();
            }" EndCallback="function(s,e){
                var result = s.cpResult;
                initializeChart(result);
                hideLoadingPanel();    
            }" />
        </dxcp:ASPxCallbackPanel>
            <table border="0" cellpadding="0" cellspacing="0">
                <colgroup>
                    <col width="50%" />
                    <col width="50%" />
                </colgroup>
                <tr>
                    <td>
                        <div class="example-content">
                            <div id="chart" style="width:500px;height:550px"></div>
                        </div>
                    </td>
                    <td>
                        <div class="example-content">
                            <div id="chart2" style="width:500px;height:550px"></div>
                        </div>
                    </td>
                </tr>
            </table>
        <input type="button" value="<%=GetLabel("Save") %>" id="btnSave" style="display:none" />
    </div>

</asp:Content>
