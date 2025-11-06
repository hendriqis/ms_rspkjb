<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPFrame.Master" AutoEventWireup="true" 
    CodeBehind="ChildrenGrowthChart.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.ChildrenGrowthChart" %>

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

    <style type="text/css">
        .keyUser
        {
            display: none;
        }
        
        .boysChart { background-color: #0984e3!important;}
        .girlsChart { background-color: #fd79a8!important;}
        
        #contentDetailNavPane > a       { margin:0; font-size:11px}
        #contentDetailNavPane > a.selected { color:#fff!important;background-color:#f44336!important }                  
    </style>

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

        function initializeChart2(chartData) {
            var param = chartData.split('|');
            var title = param[0];
            var xLabel = param[1];
            var yLabel = param[2];
            var seriesData = [];
            var seriesLabel = [];
            var actualPoint = param[5];
            var seriesType = param[6];
            var seriesColor = [];

//            if (param[8] == Constant.Gender.MALE) {
//                $("#chart2").removeClass("girlsChart");
//                $("#chart2").addClass("boysChart");
//            }
//            else {
//                $("#chart2").removeClass("boysChart");
//                $("#chart2").addClass("girlsChart");
//            }

            seriesLabel = param[3].split(';');
            seriesColor = param[7].split(';');
            seriesData = eval(param[4]);
 
            if (actualPoint != '')
                seriesData.push(eval(actualPoint));

            var plot = $.jqplot('chart2', seriesData, CreateChart2(title, seriesType, xLabel, yLabel, seriesLabel, seriesColor)).replot();

            if ($('#<%=hdnIsWHOChartInitiate.ClientID %>').val() == "0") {
                $('#<%=hdnIsWHOChartInitiate.ClientID %>').val("1");
            }
        }

        function getLabels(seriesLabel) {
            var arr = new Array(seriesLabel.length + 1);
            for (i = 0; i < seriesLabel.length; i++) {
                arr[i] = { label: seriesLabel[i], showLabel: true, lineWidth: 1 };
            }
            arr[seriesLabel.length] = { lineWidth: 4, showLabel: false, pointLabels: { show: true} };
            return arr;
        };

        function getWHOChartLabels(seriesLabel, seriesColor) {
            var arr = new Array(seriesLabel.length + 1);
            for (i = 0; i < seriesLabel.length; i++) {
                arr[i] = { label: seriesLabel[i], showLabel: true, lineWidth: 1, color: seriesColor[i] };
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

        function CreateChart2(title, type, xLabel, yLabel, seriesLabel, seriesColor) {
            var ageGroup = $("#<%=rblAgeGroup2.ClientID%> input:checked").val();
            var min = 0;
            var max = 0;
            var yMin = 0;
            var yMax = 0;
            var tickIntervalX = 0
            var tickIntervalY = 0;

            if (ageGroup == 'W01') {
                min = 0;
                max = 180;
                if (type == "wfa") {
                    yMin = 0;
                    yMax = 10;
                    tickIntervalY = 1;
                }
                else if (type == "lfa") {
                    yMin = 40;
                    yMax = 75;
                    tickIntervalY = 5;
                }
                else if (type == "lfa") {
                    yMin = 40;
                    yMax = 75;
                    tickIntervalY = 5;
                }
                else {
                    yMin = 0;
                    yMax = 10;
                    tickIntervalY = 1;              
                }

                tickIntervalX = 30;
            }
            else if (ageGroup == "W02") {
                min = 0;
                max = 720;
                yMin = 0;
                yMax = 18;
                tickIntervalX = 30;
                tickIntervalY = 1;
            }
            else if (ageGroup == "W03") {
                min = 180;
                max = 720;
                yMin = 5;
                yMax = 18;
                tickIntervalX = 30;
                tickIntervalY = 1;
            }
            else if (ageGroup == "W04") {
                min = 720;
                max = 1800;
                yMin = 7;
                yMax = 30;
                tickIntervalX = 365;
                tickIntervalY = 1;
            }
            else {
                if (type == "wfa") {
                    min = 0;
                    max = 60;
                    yMin = 0;
                    yMax = 30;
                    tickIntervalY = 2;
                    tickIntervalX = 2;
                }
                else if (type == "lfa") {
                    min = 0;
                    max = 60;
                    yMin = 40;
                    yMax = 125;
                    tickIntervalX = 2;
                    tickIntervalY = 5;
                }
                else if (type == "bfa") {
                    min = 0;
                    max = 60;
                    yMin = 9;
                    yMax = 25;
                    tickIntervalX = 2;
                    tickIntervalY = 1;
                }
                else if (type == "hca") {
                    min = 0;
                    max = 60;
                    yMin = 30;
                    yMax = 60;
                    tickIntervalX = 2;
                    tickIntervalY = 2;
                }
                else {
                    min = 0;
                    max = 1800;
                    yMin = 0;
                    yMax = 32;
                    tickIntervalX = 2;
                    tickIntervalY = 2;
                }
            }

                var grid = {
                    gridLineWidth: 1,
                    gridLineColor: 'rgb(235,235,235)',
                    drawGridlines: true
                };

                var optionsObj = {
                grid: grid,
                animate: true,
                title: title,
                series: getWHOChartLabels(seriesLabel, seriesColor),
                seriesDefaults: {
                    showMarker: false,
                    showLabel: true,
                    rendererOptions: {
                        showDataLabels: false,
                        smooth:true
                    }
                },
                legend: {
                    renderer: $.jqplot.EnhancedLegendRenderer,
                    show: true
                },
                axes: {
                    xaxis: {
                        label: xLabel,
                        labelRenderer: $.jqplot.CanvasAxisLabelRenderer,
                        labelOptions: {
                            fontFamily: 'Tahoma',
                            fontSize: '12pt'
                        },
                        tickInterval: tickIntervalX,
                        min: min,
                        max: max
                    },
                    yaxis: {
                        label: yLabel,
                        labelRenderer: $.jqplot.CanvasAxisLabelRenderer,
                        labelOptions: {
                            fontFamily: 'Tahoma',
                            fontSize: '12pt'
                        },
                        min: yMin,
                        max: yMax,
                        tickInterval: tickIntervalY
                    }
                },
                cursor: {
                    show: true,
                    zoom: true
                },
                canvasOverlay: {
                    show: true,
                    objects: [
                        { horizontalLine: {
                            name: 'barney',
                            y: 12,
                            lineWidth: 12,
                            color: 'rgb(100, 55, 124)',
                            shadow: false
                        }},
                        {verticalLine: {
                            name: "line1",
                            x: 12,
                            color: "#636e72",
                            lineWidth: 10,
                            yOffset: 0,
                            shadow: false,
                            showTooltip: true
                        }},
                        {verticalLine: {
                            name: "line2",
                            x: 24,
                            color: "#636e72",
                            color: 'rgb(133,120,24)',
                            lineWidth: 10,
                            shadow: false,
                            showTooltip: true
                        }}
                    ]
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
            $('#contentDetailNavPane a').click(function () {
                $('#contentDetailNavPane a.selected').removeClass('selected');
                $(this).addClass('selected');
                var contentID = $(this).attr('contentID');

                if (contentID != null) {
                    showDetailContent(contentID);
                    if (contentID == 'contentDetailPage2') {
                        if ($('#<%=hdnIsWHOChartInitiate.ClientID %>').val() == "0") {
                            $('#<%=ddlChartType2.ClientID %>').change();
                        }
                    }
                }
            });

            $('#contentDetailNavPane a').first().click();

            $('#<%=ddlChartType.ClientID %>').change(function () {
                cbpCDCGrowthChartProcess.PerformCallback();
            });

            $("#<%=rblAgeGroup.ClientID%> input").change(function () {
                cbpCDCGrowthChartProcess.PerformCallback();
            });

            $("#<%=rblAgeGroup2.ClientID%> input").change(function () {
                cbpWHOGrowthChartProcess.PerformCallback();
            });

            $('#<%=ddlChartType2.ClientID %>').change(function () {
                cbpWHOGrowthChartProcess.PerformCallback();
            });

            $('#<%=ddlChartType.ClientID %>').change();
        });

        function showDetailContent(contentID) {
            var i, x, tablinks;
            x = document.getElementsByClassName("contentDetail");
            for (i = 0; i < x.length; i++) {
                x[i].style.display = "none";
            }
            document.getElementById(contentID).style.display = "block";
        }

        $(document).unload(function () { $('*').unbind(); });    
    </script>
    <input type="hidden" value="" id="hdnGender" runat="server" />
    <input type="hidden" value="0" id="hdnIsWHOChartInitiate" runat="server" />

    <div id="contentDetailNavPane" class="w3-bar w3-black">
        <a contentID="contentDetailPage1" class="w3-bar-item w3-button tablink selected">CDC</a>
        <a contentID="contentDetailPage2" class="w3-bar-item w3-button tablink">WHO</a>
    </div>

    <div id="contentDetailPage1" class="container contentDetail  w3-animate-top" style="height:600px;display:none">
        <div id="toolbarArea1">
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
        <div class="example-content">
            <div id="chart" style="width:500px;height:550px"></div>
        </div>
        <input type="button" value="<%=GetLabel("Save") %>" id="btnSave" style="display:none" />
    </div>

    <div id="contentDetailPage2" class="container contentDetail  w3-animate-top" style="height:600px;display:none">
        <div id="toolbarArea2">
            <table id="tblToolbarArea2" runat="server" cellpadding="0">
                <tr>
                    <td style="width:90px; vertical-align:top; font-weight:bold" >
                        <label><%=GetLabel("Age Group") %></label>
                    </td>
                    <td style="width:400px;" >
                        <asp:RadioButtonList ID="rblAgeGroup2" runat="server" RepeatDirection="Horizontal"  RepeatColumns="3" CellPadding="2">
<%--                            <asp:ListItem Text=" Birth to 6 months" Value="W01" Selected="True" />   
                            <asp:ListItem Text=" Birth to 2 years" Value="W02" />                                    
                            <asp:ListItem Text=" 2 to 5 years" Value="W04" />
                            <asp:ListItem Text=" 6 months to 2 years" Value="W03" />   --%>
                            <asp:ListItem Text=" Birth to 5 years" Value="W05" Selected="True" />
                        </asp:RadioButtonList>
                    </td>
                    <td style="width:200px;vertical-align:top" class="labelColumn">
                        <label class="lblMandatory"><%=GetLabel("Standard Score : ( z-score )")%></label>
                    </td>
                    <td style="width:120px; vertical-align:top; font-weight:bold" >
                        <label><%=GetLabel("Chart Type") %></label>
                    </td>
                    <td style="width:350px; vertical-align:top;" >
                        <asp:DropDownList ID="ddlChartType2" runat="server" Width="200px" />
                    </td>
                </tr>
            </table>

            <dxcp:ASPxCallbackPanel ID="cbpWHOGrowthChartProcess" runat="server" ClientInstanceName="cbpWHOGrowthChartProcess" Height="0px" ShowLoadingPanel="false"
                OnCallback="cbpWHOGrowthChartProcess_Callback" >
                <ClientSideEvents BeginCallback="function(s,e){
                    showLoadingPanel();
                }" EndCallback="function(s,e){
                    var result = s.cpResult;
                    initializeChart2(result);
                    hideLoadingPanel();    
                }" />
            </dxcp:ASPxCallbackPanel>

            <div class="example-content">
                <div id="chart2" style="width:700px;height:550px;"></div>
            </div>
        </div>
    </div>
</asp:Content>
