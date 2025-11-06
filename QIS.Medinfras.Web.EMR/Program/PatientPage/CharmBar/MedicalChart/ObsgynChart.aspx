<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPFrame.Master" AutoEventWireup="true" 
    CodeBehind="ObsgynChart.aspx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.ObsgynChart" %>

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

            var plot = $.jqplot('chart', seriesData, CreateChart(title,seriesType, xLabel, yLabel, seriesLabel)).replot();
        }

        function getLabels(seriesLabel) {
            var arr = new Array(seriesLabel.length + 1);
            for (i = 0; i < seriesLabel.length; i++) {
                arr[i] = { label: seriesLabel[i], showLabel: true, lineWidth: 1 };
            }
            arr[seriesLabel.length] = { lineWidth: 4, showLabel: false, pointLabels: { show: true} };
            return arr;
        };

        function CreateChart(title,type, xLabel, yLabel, seriesLabel) {
            var ageGroup = $("#<%=rblAgeGroup.ClientID%> input:checked").val();
            var xMin = 11;
            var xMax = 42;
            var xTickInterval = 2;
            var yMin = 40;
            var yMax = 400;
            var yTickInterval = 40;
            var tickInterval = 0;
            var xTitle = xLabel;
            var yTitle = yLabel;
            if (type == 'AC') {
                xMin = 11;
                xMax = 42;
                xTickInterval = 2;
                yMin = 40;
                yMax = 400;
                yTickInterval = 40;
                xTitle = "weeks";
                yTitle = "mm";
            }
            else if (type == 'BPD') {
                xMin = 11;
                xMax = 42;
                xTickInterval = 2;
                yMin = 0;
                yMax = 140;
                yTickInterval = 20;
                xTitle = "weeks";
                yTitle = "mm";
            }
            else if (type == 'FL') {
                xMin = 10;
                xMax = 42;
                xTickInterval = 2;
                yMin = 0;
                yMax = 120;
                yTickInterval = 20;
                xTitle = "weeks";
                yTitle = "mm";
            }
            else if (type == 'HL') {
                xMin = 10;
                xMax = 42;
                xTickInterval = 2;
                yMin = 0;
                yMax = 90;
                yTickInterval = 10;
                xTitle = "weeks";
                yTitle = "mm";
            }
            else if (type == 'HC') {
                xMin = 10;
                xMax = 42;
                xTickInterval = 2;
                yMin = 0;
                yMax = 450;
                yTickInterval = 50;
                xTitle = "weeks";
                yTitle = "mm";
            }
            else if (type == 'EFW') {
                xMin = 16;
                xMax = 42;
                xTickInterval = 1;
                yMin = 0;
                yMax = 5000;
                yTickInterval = 1000;
                xTitle = "weeks";
                yTitle = "gram";
            }
            else if (type == 'OFD') {
                xMin = 10;
                xMax = 42;
                xTickInterval = 2;
                yMin = 0;
                yMax = 160;
                yTickInterval = 20;
                xTitle = "weeks";
                yTitle = "mm";
            }
            else {
                xMin = 11;
                xMax = 42;
                xTickInterval = 2;
                yMin = 40;
                yMax = 400;
                yTickInterval = 40;
                xTitle = "weeks";
                yTitle = "mm";
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
                        smooth: true,
                        showDataLabels: true
                    }
                },
                axes: {
                    xaxis: {
                        label: xTitle,
                        labelRenderer: $.jqplot.CanvasAxisLabelRenderer,
                        labelOptions: {
                            fontFamily: 'Georgia, Serif',
                            fontSize: '12pt'
                        },
                        tickInterval: xTickInterval,
                        min: xMin,
                        max: xMax 
                    },
                    yaxis: {
                        label: yTitle,
                        labelRenderer: $.jqplot.CanvasAxisLabelRenderer,
                        labelOptions: {
                            fontFamily: 'Georgia, Serif',
                            fontSize: '12pt'
                        },
                        tickInterval: yTickInterval,
                        min: yMin,
                        max: yMax 
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
                cbpChartProcess.PerformCallback();
            });
            $('#<%=ddlChartType.ClientID %>').change();
            $("#<%=rblAgeGroup.ClientID%> input").change(function () {
                cbpChartProcess.PerformCallback();
            });
        });

        $(document).unload(function () { $('*').unbind(); });
    
    </script>
    <input type="hidden" value="" id="hdnGender" runat="server" />
    <div>
        <div id="toolbarArea">
            <table id="tblToolbarArea" runat="server" cellpadding="0">
                <tr>
                    <td style="width:100px;" class="labelColumn">
                        <label class="lblMandatory"><%=GetLabel("Chart Type")%></label>
                    </td>
                    <td style="width:390px;" >
                        <asp:DropDownList ID="ddlChartType" runat="server" Width="100%" />
                    </td>
                    <td style="width:90px; padding-left:10px" >
                        <label><%=GetLabel("Pregnancy No") %></label>
                    </td>
                    <td style="width:200px;" >
                        <asp:DropDownList ID="ddlPregnancyNo" runat="server" Width="50px" />
                        <asp:RadioButtonList ID="rblAgeGroup" runat="server" RepeatDirection="Horizontal" Visible="false" >
                            <asp:ListItem Text="36 Months" Value="36M" Selected="True" />
                            <asp:ListItem Text="20 Years" Value="20Y" />
                        </asp:RadioButtonList>
                    </td>
                </tr>
            </table>
        </div>

        <dxcp:ASPxCallbackPanel ID="cbpChartProcess" runat="server" ClientInstanceName="cbpChartProcess" Height="0px" ShowLoadingPanel="false"
            OnCallback="cbpChartProcess_Callback" >
            <ClientSideEvents BeginCallback="function(s,e){
                showLoadingPanel();
            }" EndCallback="function(s,e){
                var result = s.cpResult;
                initializeChart(result);
                hideLoadingPanel();    
            }" />
        </dxcp:ASPxCallbackPanel>
            <div class="example-content">
                <div id="chart" style="width:500px;height:550px"></div>
            </div>
        <input type="button" value="<%=GetLabel("Save") %>" id="btnSave" style="display:none" />
    </div>

</asp:Content>
