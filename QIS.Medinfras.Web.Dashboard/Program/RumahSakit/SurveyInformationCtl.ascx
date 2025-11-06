<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SurveyInformationCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Dashboard.Program.SurveyInformationCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<div class="main-wrapper">
    <main class="main users chart-page" id="skip-target">
        <div class="containerdb">
            <div class="d-sm-flex align-items-center justify-content-between m-2"></div>
            <div class="gridParent">
                <div class="top1SurveyDiv"> 
                    <div class="SurveyInfoDiv1">
                        <div class="surveyCBOInfo">
                            <p>
                                PILIH SURVEY
                            </p>
                        </div>
                        <dxe:ASPxComboBox ID="surveyGraphSelection" ClientInstanceName="surveyGraphSelection" runat="server"
                            Width="60%  " TextField="OptionValue" ValueField="OptionValue" ValueType="System.String">
                            <ClientSideEvents ValueChanged="" />
                        </dxe:ASPxComboBox>
                    </div>
                    <div class="SurveyInfoDiv2">
                        <div class="surveyCBOInfo">
                            <p>
                                PILIH PERIODE
                            </p>
                        </div>
                        <div class="surveyDatePeriod">
                            <dxcp:ASPxCallbackPanel ID="cbpSurveyInformation" runat="server" Width="100%" ClientInstanceName="cbpSurveyInformation"
                                ShowLoadingPanel="false" OnCallback="cbpSurveyInformation_Callback">
                                <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewPopup').show(); }"
                                    EndCallback="function(s,e){ oncbpSurveyInformationEndCallback(s); }" />
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent1" runat="server">
                                        <asp:Panel runat="server" ID="pnlPatientVisitTransHdGrdView" Style="width: 100%;
                                            margin-left: auto; margin-right: auto; position: relative; font-size: 0.95em; display: flex;">
                                            <asp:TextBox ID="txtValueDateFrom" TextMode="Date" CssClass="txtSurveyDateFrom" runat="server"
                                                Width="120px" Height="20px" onkeypress="return isNumberKey(event)"/>
                                            <p>-</p>
                                            <asp:TextBox ID="txtValueDateTo" TextMode="Date" CssClass="txtSurveyDateTo" runat="server"
                                                Width="120px" Height="20px" onkeypress="return isNumberKey(event)"/>
                                            <input type="hidden" value="" id="hdnSurveyName" class="hdnSurveyName" runat="server" />
                                            <input type="hidden" value="" id="hdnSurveyID" class="hdnSurveyName" runat="server" />
                                            <input type="hidden" value="" id="hdnDateFrom" class="hdnDateFrom" runat="server" />
                                            <input type="hidden" value="" id="hdnDateTo" class="hdnDateTo" runat="server" />
                                            <input type="hidden" value="" id="hdnData" class="hdnData" runat="server" />
                                        </asp:Panel>
                                    </dx:PanelContent>
                                </PanelCollection>
                            </dxcp:ASPxCallbackPanel>
                        </div>
                    </div>
                    <div class="SurveyInfoDiv3">
                        <button type="button" class="surveyDataInput" id="surveyDataBtnID" onclick="checkValue();">SEARCH</button>
                    </div>
                </div>
            </div>
            <div class="surveyGraphContainer" id="surveyGraphContainer">
                
            </div>
        </div>
    </main>
</div>
<script type="text/javascript">
    var defaultDateFrom = document.getElementById('<%= txtValueDateFrom.ClientID %>');
    var defaultDateTo = document.getElementById('<%= txtValueDateTo.ClientID %>');
    var today = new Date();
    var year = today.getFullYear();
    var month = ('0' + (today.getMonth() + 1)).slice(-2);
    var day = ('0' + today.getDate()).slice(-2);
    var defaultValue = year + '-' + month + '-' + day;

    defaultDateFrom.value = defaultValue;
    defaultDateTo.value = defaultValue;

    var graphContainer = document.getElementById("surveyGraphContainer");
    var graphCount;
    var clickCount = 0;
    
    function isNumberKey(evt) {
        var charCode = (evt.which) ? evt.which : evt.keyCode;
        if (charCode > 31 && (charCode < 48 || charCode > 57))
            return false;
        return true;
    }

    function dynamicColors() {
        var r = Math.floor(Math.random() * 255);
        var g = Math.floor(Math.random() * 255);
        var b = Math.floor(Math.random() * 255);
        return "rgba(" + r + "," + g + "," + b + ")";
    }

    function poolColors(a) {
        var pool = [];
        for(i = 0; i < a; i++) {
            pool.push(dynamicColors());
        }
        return pool;
    }

    function dynamicLabel(obj, i) {
        var labels = [];
        
        if (obj[i].label1 != "" && obj[i].label2 != "" && obj[i].label3 != "" && obj[i].label4 != "" && obj[i].label5 != ""){
            labels.push(obj[i].label1, obj[i].label2, obj[i].label3, obj[i].label4, obj[i].label5);
        } else if (obj.label1 != "" && obj[i].label2 != "" && obj[i].label3 != "" && obj[i].label4 != "" && obj[i].label5 == ""){
            labels.push(obj[i].label1, obj[i].label2, obj[i].label3, obj[i].label4);
        } else if (obj[i].label1 != "" && obj[i].label2 != "" && obj[i].label3 != "" && obj[i].label4 == "" && obj[i].label5 == ""){
            labels.push(obj[i].label1, obj[i].label2, obj[i].label3);
        } else if (obj[i].label1 != "" && obj[i].label2 != "" && obj[i].label3 == "" && obj[i].label4 == "" && obj[i].label5 == ""){
            labels.push(obj[i].label1, obj[i].label2);
        } else if (obj[i].label1 != "" && obj[i].label2 == "" && obj[i].label3 == "" && obj[i].label4 == "" && obj[i].label5 == ""){
            labels.push(obj[i].label1);
        }

        return labels;
    }

    
    function createChart(obj) {
        for (var i = 0; i < obj.length; i++) {
            var questionObj = [];
            var objSplitter1 = parseInt(obj[i].jawaban1);
            var objSplitter2 = parseInt(obj[i].jawaban2);
            var objSplitter3 = parseInt(obj[i].jawaban3);
            var objSplitter4 = parseInt(obj[i].jawaban4);
            var objSplitter5 = parseInt(obj[i].jawaban5);

            questionObj.push(objSplitter1);
            questionObj.push(objSplitter2);
            questionObj.push(objSplitter3);
            questionObj.push(objSplitter4);
            questionObj.push(objSplitter5);

            var graphQuestion = obj[i].questionName;
            var existingCanvas = $('#' + graphQuestion);
            var createChartCanvas;

            if (existingCanvas.length > 0) {
                // Chart element already exists, update the chart data
                createChartCanvas = existingCanvas.get(0);
                var chartInstance = Chart.getChart(createChartCanvas);
                chartInstance.data.labels = dynamicLabel(obj, i);
                chartInstance.data.datasets[0].data = questionObj;
                chartInstance.update();
            } else {
                // Create a new chart canvas
                var createChartDiv = document.createElement("div");
                createChartDiv.setAttribute("class", "surveyItem");
                graphContainer.appendChild(createChartDiv)

                createChartCanvas = document.createElement("canvas");
                createChartCanvas.setAttribute("class", "surveyCanvas");
                createChartCanvas.setAttribute("id", graphQuestion);
                createChartDiv.appendChild(createChartCanvas);

                var DataOption = {
                    type: 'doughnut',
                    data: {
                        labels: dynamicLabel(obj, i),
                        datasets: [{
                            data: questionObj,
                            backgroundColor: poolColors(questionObj.length),
                            borderColor: ['rgba(0, 0, 0)'],
                            hoverOffset: 4,
                        }]
                    },
                    options: {
                        plugins: {
                            legend: {
                                position: 'right',
                                labels: {
                                    boxWidth: 20,
                                    padding: 20
                                }
                            }
                        },
                        animation: {
                            animateScale: true
                        },
                        responsive: true,
                        parsing: {
                            xAxisKey: 'label',
                            yAxisKey: 'jmlJawaban'
                        }
                    }
                }

                var ctx = createChartCanvas.getContext('2d');
                var chart = new Chart(ctx, DataOption);
            }
        }
        graphCount = document.querySelectorAll('.surveyItem').length;
    }

    var previousValue = null;
    var buttonClicked = false;
    var searchButton = document.getElementById('surveyDataBtnID');
    searchButton.addEventListener('click', function() {
        buttonClicked = true;
        checkValue();
    });

    function splitData() {
        var dataJawaban = $('#<%:hdnData.ClientID %>').val();
        console.log(dataJawaban);
        if(dataJawaban == "0"){
            return;
        }
        var arrJawaban = [];
        arrJawaban = dataJawaban.split('[]')

        var objectJawaban = [];
        for (var i = 0; i < arrJawaban.length; i++) {
            var splitIndexJawaban = arrJawaban[i].split('|');
            var jawabanJSON =
            { questionName: splitIndexJawaban[1],
                label1: splitIndexJawaban[2],
                label2: splitIndexJawaban[3],
                label3: splitIndexJawaban[4],
                label4: splitIndexJawaban[5],
                label5: splitIndexJawaban[6],
                jawaban1: splitIndexJawaban[7],
                jawaban2: splitIndexJawaban[8],
                jawaban3: splitIndexJawaban[9],
                jawaban4: splitIndexJawaban[10],
                jawaban5: splitIndexJawaban[11]
            };
            objectJawaban.push(jawabanJSON);
        }
        createChart(objectJawaban);
    }

    var tempDateFromValue = "empty ";
    var tempDateToValue = "empty ";
    var newDateFromValue = "";
    var newDateToValue = "";
    var prevSelectedSurvey = "";
    var prevSelectedDate = "";

    function checkCBOChange(cboChange) {
        var currentValue = surveyGraphSelection.GetValue();
        var selectedSurvey = $("#<%:surveyGraphSelection.ClientID %> option:selected").text();
        var selectedDate = tempDateFromValue + tempDateToValue + newDateFromValue + newDateToValue;
        console.log(tempDateFromValue + " " + tempDateToValue + " " + newDateFromValue + " " + newDateToValue);
    
        if (((currentValue != previousValue) || (tempDateFromValue != newDateFromValue || tempDateToValue != newDateToValue)) && buttonClicked) {
            // Selected value has changed
            console.log("cboChange If Passed");
            var surveyItemDelete = document.querySelectorAll('.surveyItem');
            var surveyCanvasDelete = document.querySelectorAll('.surveyCanvas');

            surveyItemDelete.forEach(function(element) {
                element.remove();
            });

            surveyCanvasDelete.forEach(function(element) {
                element.remove();
            });

            previousValue = currentValue;
            buttonClicked = false; // Reset the buttonClicked flag
            tempDateFromValue = newDateFromValue;
            tempDateToValue = newDateToValue;

            prevSelectedSurvey = selectedSurvey;
            prevSelectedDate = selectedDate;

            return cboChange = "true";
        } else if (selectedSurvey === prevSelectedSurvey && selectedDate === prevSelectedDate) {
            // No change in survey selection or date
            return cboChange = "false";
        } else {
            // Selected value has not changed, but survey or date changed
            prevSelectedSurvey = selectedSurvey;
            prevSelectedDate = selectedDate;
            return cboChange = "true";
        }
    }

    function checkValue() {
        var cboChange = "";
        var Change = checkCBOChange(cboChange);
        var selectedValue = surveyGraphSelection.GetText();
        var selectedSurveyID = surveyGraphSelection.GetValue();
        var dateFrom = document.getElementById('<%= txtValueDateFrom.ClientID %>').value;
        var dateTo = document.getElementById('<%= txtValueDateTo.ClientID %>').value;
        newDateFromValue = dateFrom;
        newDateToValue = dateTo;

        if (Change != "true") {
            return;
        }
        else {
            var searchButton = document.getElementById('surveyDataBtnID');
            console.log("change is true");
            document.getElementById("<%= hdnSurveyName.ClientID %>").value = selectedValue;
            document.getElementById("<%= hdnSurveyID.ClientID %>").value = selectedSurveyID;
            document.getElementById("<%= hdnDateFrom.ClientID %>").value = dateFrom;
            document.getElementById("<%= hdnDateTo.ClientID %>").value = dateTo;

            console.log(dateFrom + " " + dateTo);

            cbpSurveyInformation.PerformCallback('save');
        }
    }

    function oncbpSurveyInformationEndCallback(s) {
        splitData();
    }
</script>
