<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPFrame.Master" AutoEventWireup="true" 
    CodeBehind="VitalSignChart.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.VitalSignChart" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhMPFrame" runat="server">
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
        var pageCount = parseInt('<%=VitalSignChartPageCount %>');
        $(function () {
            $('#contentDetailNavPane a').click(function () {
                $('#contentDetailNavPane a.selected').removeClass('selected');
                $(this).addClass('selected');
                var contentID = $(this).attr('contentID');

                if (contentID != null) {
                    showDetailContent(contentID);
                    if (contentID == "contentDetailPage3") {
                        setPaging($("#pagingVitalSignView"), pageCount, function (page) {
                            cbpVitalSignView.PerformCallback('changepage|' + page);
                        });
                    }
                }
            });

            $('#<%=grdVitalSignView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdVitalSignView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
            });

            $('#<%=grdVitalSignView.ClientID %> tr:eq(1)').click();

            $('#contentDetailNavPane a').first().click();

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

            function createThumbChart() {
                var ctrThumbChart = 0;
                $('#draggableScroller  .content').each(function () {
                    if (allThumbChartData[ctrThumbChart] != null) {
                        var idChart = $(this).find('.idChart').val();
                        var seriesData = [];
                        var seriesColors = [];
                        var seriesLabel = new Array(1);
                        var thumbChartData = allThumbChartData[ctrThumbChart].split('^'); // Title^yMin^yMax
                        var minDate = $('#<%=hdnMinDate.ClientID %>').val();
                        if(allSeriesData[ctrThumbChart].length > 1)
                        {
                            seriesData.push(allSeriesData[ctrThumbChart]);
                            seriesColors.push(listColor[ctrThumbChart]);
                            seriesLabel[0] = { label: allSeriesLabel[ctrThumbChart] };
                            //var line1=[['2008-06-30 8:00AM',4], ['2008-7-30 8:00AM',6.5], ['2008-8-30 8:00AM',5.7], ['2008-9-30 8:00AM',9], ['2008-10-30 8:00AM',8.2]];
                            $.jqplot(idChart, seriesData, ThumbChartOption(thumbChartData[0], xLabel, yLabel, seriesLabel, seriesColors));
                        }
                    }
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

            //#region Chart
            var allThumbChartData = [];
            var title;
            var xLabel;
            var yLabel;
            var allSeriesData = [];
            var allSeriesLabel = [];
            var visibleChart = '';
            var listColor = ["#55efc4", "#e84393", "#81ecec", "#d63031", "#74b9ff", "#e17055", "#a29bfe", "#fdcb6e", "#00b894", "#fd79a8", "#00cec9","#ff7675","#0984e3","#fab1a0","#6c5ce7","#ffeaa7"]

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
                label.appendChild(document.createTextNode(" " + text));

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
                    grid: {
                        background: 'rgba(178, 190, 195,1.0)',
                        drawBorder: false,
                        shadow: false,
                        gridLineColor: '#666666',
                        gridLineWidth: 1
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
                        fontFamily: 'Tahoma, Serif',
                        fontSize: '8pt',
                        textColor: '#FFFFFF'
                    },
                    axes: {
                        xaxis: {
                            drawMajorGridlines: false,
                            renderer:$.jqplot.DateAxisRenderer
                        },
                        yaxis: {
                            drawMajorGridlines: false
                        },
                    }
                };
                return optionsObj;
            }

            function ChartOption(title, xLabel, yLabel, seriesLabel, seriesColors) {
                var optionsObj = {
                    seriesColors: seriesColors,
                    series: seriesLabel,
                    title: title,
                    grid: {
                        background: 'rgba(223, 230, 233,1.0)',
                        drawBorder: false,
                        shadow: false,
                        gridLineColor: '#2d3436',
                    },                                      
                    legend: {
                        renderer: $.jqplot.EnhancedLegendRenderer,
                        show: true
                    },
                    axesDefaults: {
                        pad: 0,
                        rendererOptions: {
                            baselineWidth: 1.5,
                            baselineColor: '#444444',
                            drawBaseline: false
                        }
                    },
                    seriesDefaults: {
                        rendererOptions: {
                            smooth: true,
                        },
                        pointLabels: { show: true }
                    },
                    axes: {
                        xaxis: {
                            label: xLabel,
                            renderer:$.jqplot.DateAxisRenderer,
                            tickOptions: {
                                 formatString: '%#d/%#m/%y \n%H:%M    '
                            },
                           labelOptions: {
                                fontFamily: 'Tahoma, Serif',
                                fontSize: '10pt'
                            },
                            drawMajorGridlines: false,
                            drawMinorGridlines: true,
                            drawMajorTickMarks: false
                        },
                        yaxis: {
                            label: yLabel,
                            labelRenderer: $.jqplot.CanvasAxisLabelRenderer,
                            labelOptions: {
                                fontFamily: 'Tahoma, Serif',
                                fontSize: '10pt'
                            },
                            drawMajorGridlines: false,
                            drawMinorGridlines: true,
                            drawMajorTickMarks: false
                        }
                    },
                    cursor: {
                        show: true,
                        zoom: true,
                        style: 'pointer'
                    },
                    highlighter: {
                        show: true,
                        tooltipAxes: 'both',
                        tooltipLocation: 'n',
                        yvalues: 1,
                        useAxesFormatters: true,
                        bringSeriesToFront: false,
                        formatString: '<b>%s</b>,<b style="color:blue"><i>%s</i></b>'
//                        formatString: 'Tanggal/Jam: <b>%s</b> <br />Nilai: <b>%s (%s)</b><br />Catatan: <i style="color:red">%s</i>'
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

        function showDetailContent(contentID) {
            var i, x, tablinks;
            x = document.getElementsByClassName("content3Detail");
            for (i = 0; i < x.length; i++) {
                x[i].style.display = "none";
            }
            document.getElementById(contentID).style.display = "block";
        }

        //#region Paging
        $(function () {
            setPaging($("#pagingVitalSignView"), pageCount, function (page) {
                cbpVitalSignView.PerformCallback('changepage|' + page);
            });
        });

        function oncbpVitalSignViewEndCallback(s) {
            $('#containerImgLoadingViewVitalSign').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdVitalSignView.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingVitalSignView"), pageCount, function (page) {
                    cbpVitalSignView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdVitalSignView.ClientID %> tr:eq(1)').click();
        }
        //#endregion

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
	        overflow-x: hidden;
	        overflow-y: auto;
        }
        #draggableScroller .content{
            vertical-align:middle;
            text-align:center;
            margin:2px 0;
        }
        div.thumbChart{
	        border:2px solid #fff;
	        width:100px;
	        height:80px;
            margin:0 auto;
            cursor: pointer;
        }
        #draggableScroller a{
	        padding:2px;
	        outline:none;
        }
        .containerDropZone{width:100%;margin:0 auto;padding-top:10px;text-align:center;}
        .clear{clear:both;}
        
        #contentDetailNavPane > a       { margin:0; font-size:11px}
        #contentDetailNavPane > a.selected { color:#fff!important;background-color:#f44336!important }               
    </style>

    <input type="hidden" value="" id="hdnMinDate" runat="server" />    
        <div id="contentDetailNavPane" class="w3-bar w3-black">
            <a contentID="contentDetailPage1" class="w3-bar-item w3-button tablink selected">Grafik Tanda Vital</a>
            <a contentID="contentDetailPage2" class="w3-bar-item w3-button tablink" style="display:none">Statistik Tanda Vital</a>
            <a contentID="contentDetailPage3" class="w3-bar-item w3-button tablink">Catatan Tanda Vital</a>
        </div>
    <div id="contentDetailPage1" class="container content3Detail  w3-animate-top" style="height:600px;display:none"">
        <table style="width:100%;height:auto;border-collapse:collapse;" cellpadding="0" cellspacing="0" class="boxShadow">
            <tr style="display:none">
                <td colspan="2">
                    <div style="position: relative;">
                        <table border="0" cellpadding="0" cellspacing="1">
                            <colgroup>
                                <col style="width:100px"/>
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Tampilan Data")%></label></td>
                                <td>
                                    <asp:RadioButtonList ID="rblItemType" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table">
                                        <asp:ListItem Text=" Grafik Harian (Tanggal Hari Ini) " Value="1" Selected="True"/>
                                        <asp:ListItem Text=" Kunjungan/Perawatan saat ini " Value="2"  />
                                        <asp:ListItem Text=" Riwayat Kunjungan" Value="3" Enabled= "false" />
                                    </asp:RadioButtonList>
                                </td>
                            </tr> 
                        </table>
                    </div>  
                </td>
            </tr>
            <tr>
                <td style="min-width:220px;width:220px;height:580px;border:1px solid #EAEAEA" id="tdDraggableImage">
                    <div id="draggableOuterContainer">
			            <div id="draggableScroller" style="overflow-y:auto;">
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
                                    <div id="chart" style="height:560px;width:900px;border:1px solid #EAEAEA;float:left;" class="boxShadow"></div>                            
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

    <div id="contentDetailPage2" class="container content3Detail  w3-animate-top" style="height:600px;display:none"">
        <asp:ListView runat="server" ID="lvwViewVS" OnItemDataBound="lvwViewVS_ItemDataBound">
            <EmptyDataTemplate>
                <table id="tblView" runat="server" class="grdItem grdSelected" cellspacing="0" rules="all"
                    style="font-size: 0.9em">
                    <tr>
                        <th rowspan="2" align="left">
                            <%=GetLabel("VITAL SIGN")%>
                        </th>
                        <th colspan="3" align="center">
                            <%=GetLabel("SUMMARY")%>
                        </th>
                        <th colspan="2" align="center">
                            <%=GetLabel("LAST")%>
                        </th>
                    </tr>
                    <tr>
                        <th style="width: 55px" align="center">
                            <%=GetLabel("MIN")%>
                        </th>
                        <th style="width: 55px" align="center">
                            <%=GetLabel("MAX")%>
                        </th>
                        <th style="width: 55px" align="center">
                            <%=GetLabel("AVERAGE")%>
                        </th>
                        <th style="width: 55px" align="center">
                            <%=GetLabel("VALUE")%>
                        </th>
                        <th style="width: 70px" align="center">
                            <%=GetLabel("DATE")%>
                        </th>
                    </tr>
                    <tr class="trEmpty">
                        <td colspan="5">
                            <%=GetLabel("There is no record to display")%>
                        </td>
                    </tr>
                </table>
            </EmptyDataTemplate>
            <LayoutTemplate>
                <table id="tblView" runat="server" class="grdItem grdSelected" cellspacing="0" rules="all"
                    style="font-size: 0.9em">
                    <tr>
                        <th rowspan="2" align="left">
                            <%=GetLabel("VITAL SIGN")%>
                        </th>
                        <th colspan="3" align="center">
                            <%=GetLabel("SUMMARY")%>
                        </th>
                        <th colspan="2" align="center">
                            <%=GetLabel("LAST")%>
                        </th>
                    </tr>
                    <tr>
                        <th style="width: 55px" align="center">
                            <%=GetLabel("MIN")%>
                        </th>
                        <th style="width: 55px" align="center">
                            <%=GetLabel("MAX")%>
                        </th>
                        <th style="width: 55px" align="center">
                            <%=GetLabel("AVERAGE")%>
                        </th>
                        <th style="width: 55px" align="center">
                            <%=GetLabel("VALUE")%>
                        </th>
                        <th style="width: 70px" align="center">
                            <%=GetLabel("DATE")%>
                        </th>
                    </tr>
                    <tr runat="server" id="itemPlaceholder">
                    </tr>
                </table>
            </LayoutTemplate>
            <ItemTemplate>
                <tr>
                    <td class="tdItemName">
                        <label class="lblItemName">
                            <%#: Eval("VitalSignLabel")%></label>
                    </td>
                    <td align="center">
                        <asp:TextBox ID="txtVSMinValue" Width="55px" runat="server" value="0" CssClass="number txtMinValue"
                            ReadOnly="true" />
                    </td>
                    <td align="center">
                        <asp:TextBox ID="txtVSMaxValue" Width="55px" runat="server" value="0" CssClass="number txtMaxValue"
                            ReadOnly="true" />
                    </td>
                    <td align="center">
                        <asp:TextBox ID="txtVSAverageValue" Width="55px" runat="server" value="0" CssClass="number txtAverageValue"
                            ReadOnly="true" />
                    </td>
                    <td align="center">
                        <asp:TextBox ID="txtVSLastValue" Width="55px" runat="server" value="0" CssClass="number txtAverageValue"
                            ReadOnly="true" />
                    </td>
                    <td align="center">
                        <asp:TextBox ID="txtVSLastDate" Width="70px" runat="server" value="" CssClass="date txtLastDate" style="text-align:center"
                            ReadOnly="true" />
                    </td>
                </tr>
            </ItemTemplate>
        </asp:ListView>
    </div>
    <div id="contentDetailPage3" class="container content3Detail  w3-animate-top" style="height:600px;display:none"">
        <dxcp:ASPxCallbackPanel ID="cbpVitalSignView" runat="server" Width="100%" ClientInstanceName="cbpVitalSignView"
            ShowLoadingPanel="false" OnCallback="cbpVitalSignView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){$('#containerImgLoadingViewVitalSign').show(); }"
                EndCallback="function(s,e){oncbpVitalSignViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage4">
                        <asp:GridView ID="grdVitalSignView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false"
                            ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" ShowHeader="false">
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                    <asp:BoundField DataField="CreatedBy" HeaderStyle-CssClass="hiddenColumn keyUser" ItemStyle-CssClass="hiddenColumn keyUser" />
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                    <HeaderTemplate>
                                        <div>
                                            Catatan Pemeriksaan Tanda Vital dan Indikator Lainnya</div>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div>
                                            <b>
                                                <%#: Eval("ObservationDateInString")%>,
                                                <%#: Eval("ObservationTime") %>,
                                                <%#: Eval("ParamedicName") %>
                                            </b>
                                        </div>
                                        <div>
                                            <span style="font-style:italic">
                                                <%#: Eval("Remarks")%>
                                            </span>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("Tidak ada data catatan pemeriksaaan Tanda Vital untuk pasien ini") %>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </asp:Panel>
                </dx:PanelContent>
            </PanelCollection>
        </dxcp:ASPxCallbackPanel>
        <div class="imgLoadingGrdView" id="containerImgLoadingViewVitalSign">
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
        <div class="containerPaging">
            <div class="wrapperPaging">
                <div id="pagingVitalSignView">
                </div>
            </div>
        </div>
    </div>
</asp:Content>
