<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrxDashboard.master" AutoEventWireup="true"
    CodeBehind="SurveyPasien.aspx.cs" Inherits="QIS.Medinfras.Web.Dashboard.Program.SurveyPasien" %>

<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <div class="spContainer">
        <div class="surveyFormContainer">
            <div class="surveyRow">
                <div class="card">
                    <img src='<%=ResolveUrl("~/Libs/Images/Dashboard/surveyHeader.png") %>' class="card-img-top"
                        alt="...">
                </div>
            </div>
            <div class="surveyRow">
                <div class="surveySection" id="formSectionID">
                </div>
                <div id="submitDiv">
                    <button type="button" class="formSubmitBtn" id="formsubmitBtnID" onclick="checkValue();">
                        SUBMIT</button>
                </div>
            </div>
        </div>
    </div>
    <dxcp:ASPxCallbackPanel ID="cbpEntryPopupView" runat="server" Width="100%" ClientInstanceName="cbpEntryPopupView"
        ShowLoadingPanel="false" OnCallback="cbpEntryPopupView_Callback">
        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewPopup').show(); }"
            EndCallback="function(s,e){ onCbpEntryPopupViewEndCallback(s); }" />
        <PanelCollection>
            <dx:PanelContent ID="PanelContent1" runat="server">
                <asp:Panel runat="server" ID="pnlPatientVisitTransHdGrdView" Style="width: 100%;
                    margin-left: auto; margin-right: auto; position: relative; font-size: 0.95em;">
                        <input type="hidden" value="" id="hdnDataAnswerID" class="hdnDataAnswer" runat="server" />
                </asp:Panel>
            </dx:PanelContent>
        </PanelCollection>
    </dxcp:ASPxCallbackPanel>
    <input type="hidden" value="" id="JsonChartListPertanyaan" runat="server" />
    <script type="text/javascript">
        $(function () {
            CreateFormElement();
        });

        var data = JSON.parse($('#<%=JsonChartListPertanyaan.ClientID %>').val());
        var br = document.createElement("br");
        var formSection = document.getElementById("formSectionID");
        var param = '';
        var Answer01 = null;
        var Answer02 = null;
        var Answer03 = null;
        var Answer04 = null;
        var Answer05 = null;

        function checkValue() {
            for (var z = 0; z < data.length; z++) {
                var participantName = document.getElementById("surveyParticipantName");
                if (data[z].GCQuestionType == "X553^001" || data[z].GCQuestionType == "X553^002") {
                    var QuestionName = document.getElementsByName("Question" + z.toString());
                    for (var i = 0; i < QuestionName.length; i++) {
                        if (QuestionName[i].checked) {
                            eval('Answer0' + (i + 1).toString() + '=' + 'QuestionName[i].value' + ';');
                        }
                    }
                    if (param == '') {
                        param = '$qAnswer|' + data[z].QuestionID + '|' + participantName.value + '|' + Answer01 + '|' + Answer02 + '|' + Answer03 + '|' + Answer04 + '|' + Answer05;
                        Answer01 = null;
                        Answer02 = null;
                        Answer03 = null;
                        Answer04 = null;
                        Answer05 = null;
                    }
                    else {
                        param += '$qAnswer|' + data[z].QuestionID + '|' + participantName.value + '|' + Answer01 + '|' + Answer02 + '|' + Answer03 + '|' + Answer04 + '|' + Answer05;
                        Answer01 = null;
                        Answer02 = null;
                        Answer03 = null;
                        Answer04 = null;
                        Answer05 = null;
                    }
                } else if (data[z].GCQuestionType == "X553^003") {
                    var QuestionName = document.getElementById("surveyKPEssayInput" + z.toString());
                    if (QuestionName.value === '') {
                        alert("Some Question is still unanswered");
                    } else {
                        eval('Answer01' + '=' + 'QuestionName.value' + ';');
                        if (param == '') {
                            param = '$qAnswer|' + data[z].QuestionID + '|' + participantName.value + '|' + Answer01 + '|' + Answer02 + '|' + Answer03 + '|' + Answer04 + '|' + Answer05;
                            Answer01 = null;
                            Answer02 = null;
                            Answer03 = null;
                            Answer04 = null;
                            Answer05 = null;
                        }
                        else {
                            param += '$qAnswer|' + data[z].QuestionID + '|' + participantName.value + '|' + Answer01 + '|' + Answer02 + '|' + Answer03 + '|' + Answer04 + '|' + Answer05;
                            Answer01 = null;
                            Answer02 = null;
                            Answer03 = null;
                            Answer04 = null;
                            Answer05 = null;
                        }
                    }
                }
            }
            document.getElementById("<%= hdnDataAnswerID.ClientID %>").value = param;
            cbpEntryPopupView.PerformCallback('save');
        }

        function onCbpEntryPopupViewEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == 'save') {
                if (param[1] == 'fail')
                    showToast('Save Failed', 'Error Message : ' + param[2]);
                else
                    $('#containerPopupEntryData').hide();
            }
            else if (param[0] == 'delete') {
                if (param[1] == 'fail')
                    showToast('Delete Failed', 'Error Message : ' + param[2]);
            }
            $('#containerImgLoadingViewPopup').hide();
            location.reload();
        }

        function CreateFormElement() {
            var FN = document.createElement("input");
            FN.setAttribute("type", "text");
            FN.setAttribute("name", "FullName");
            FN.setAttribute("placeholder", "Full Name");
            FN.setAttribute("class", "surveyKPInput");
            FN.setAttribute("id", "surveyParticipantName");

            var fullName = document.createElement("label");
            fullName.setAttribute("class", "surveyKPLabel");
            fullName.innerText = 'Nama';

            formSection.appendChild(fullName);
            formSection.appendChild(FN);

            //Question generator loop
            for (var i = 0; i < data.length; i++) {
                // Set question name
                var qname = document.createElement("label");
                qname.setAttribute("class", "surveyKPLabel");
                qname.innerText = data[i].QuestionName;

                // Create checkbox container
                var cbDiv = document.createElement("div");
                cbDiv.setAttribute("class", "checkboxes");

                // create checkbox input label
                var cbLbl = document.createElement("label");
                cbLbl.setAttribute("class", "KPCheckboxLbl");

                // create checkbox input
                var cbInput = document.createElement("input");
                cbInput.setAttribute("type", "radio");
                cbInput.setAttribute("class", "KPCheckbox");

                if (data[i].GCQuestionType == "X553^001") {
                    formSection.appendChild(qname);
                    formSection.appendChild(cbDiv);
                    var radioInputCtr;
                    if (data[i].Answer01 != "" && data[i].Answer02 != "" && data[i].Answer03 != "" && data[i].Answer04 != "" && data[i].Answer05 != "") {
                        radioInputCtr = 5;
                    } else if (data[i].Answer01 != "" && data[i].Answer02 != "" && data[i].Answer03 != "" && data[i].Answer04 != "" && data[i].Answer05 == "" ||
                       data[i].Answer01 != "" && data[i].Answer02 != "" && data[i].Answer03 != "" && data[i].Answer04 == "" && data[i].Answer05 != "" ||
                       data[i].Answer01 != "" && data[i].Answer02 != "" && data[i].Answer03 == "" && data[i].Answer04 != "" && data[i].Answer05 != "" ||
                       data[i].Answer01 != "" && data[i].Answer02 == "" && data[i].Answer03 != "" && data[i].Answer04 != "" && data[i].Answer05 != "" ||
                       data[i].Answer01 == "" && data[i].Answer02 == "" && data[i].Answer03 != "" && data[i].Answer04 != "" && data[i].Answer05 != "") {
                        radioInputCtr = 4;
                    } else if (data[i].Answer01 != "" && data[i].Answer02 != "" && data[i].Answer03 != "" && data[i].Answer04 == "" && data[i].Answer05 == "" ||
                       data[i].Answer01 != "" && data[i].Answer02 != "" && data[i].Answer03 == "" && data[i].Answer04 == "" && data[i].Answer05 != "" ||
                       data[i].Answer01 != "" && data[i].Answer02 == "" && data[i].Answer03 == "" && data[i].Answer04 != "" && data[i].Answer05 != "" ||
                       data[i].Answer01 == "" && data[i].Answer02 == "" && data[i].Answer03 != "" && data[i].Answer04 != "" && data[i].Answer05 != "") {
                        radioInputCtr = 3;
                    } else if (data[i].Answer01 != "" && data[i].Answer02 != "" && data[i].Answer03 == "" && data[i].Answer04 == "" && data[i].Answer05 == "" ||
                       data[i].Answer01 != "" && data[i].Answer02 == "" && data[i].Answer03 == "" && data[i].Answer04 == "" && data[i].Answer05 != "" ||
                       data[i].Answer01 == "" && data[i].Answer02 == "" && data[i].Answer03 == "" && data[i].Answer04 != "" && data[i].Answer05 != "") {
                        radioInputCtr = 2;
                    } else {
                        radioInputCtr = 1;
                    }
                    for (var x = 1; x <= radioInputCtr; x++) {

                        // create checkbox input container
                        var cbInputGroup = document.createElement("div");
                        cbInputGroup.setAttribute("class", "checkboxgroup");

                        var cbInput = document.createElement("input");
                        cbInput.setAttribute("type", "radio");
                        cbInput.setAttribute("class", "KPCheckbox");
                        cbInput.setAttribute("name", "Question" + i.toString());
                        cbInput.setAttribute("id", "cbQuestionAnswer" + x.toString());
                        cbInput.setAttribute("value", eval('data[i].Answer0' + x.toString()));

                        // create checkbox input label
                        var cbLbl1 = document.createElement("label");
                        var cbLbl2 = document.createElement("label");
                        var cbLbl3 = document.createElement("label");
                        var cbLbl4 = document.createElement("label");
                        var cbLbl5 = document.createElement("label");
                        if (radioInputCtr == 5) {
                            cbLbl1.setAttribute("class", "KPCheckboxLbl");
                            cbLbl1.setAttribute("id", "RadioInputLbl" + x.toString());

                            cbLbl2.setAttribute("class", "KPCheckboxLbl");
                            cbLbl2.setAttribute("id", "RadioInputLbl" + x.toString());

                            cbLbl3.setAttribute("class", "KPCheckboxLbl");
                            cbLbl3.setAttribute("id", "RadioInputLbl" + x.toString());

                            cbLbl4.setAttribute("class", "KPCheckboxLbl");
                            cbLbl4.setAttribute("id", "RadioInputLbl" + x.toString());

                            cbLbl5.setAttribute("class", "KPCheckboxLbl");
                            cbLbl5.setAttribute("id", "RadioInputLbl" + x.toString());

                            cbLbl1.innerText = data[i].Answer01;
                            cbLbl2.innerText = data[i].Answer02;
                            cbLbl3.innerText = data[i].Answer03;
                            cbLbl4.innerText = data[i].Answer04;
                            cbLbl5.innerText = data[i].Answer05;

                            var cbLblString = "cbLbl" + x;
                            cbDiv.appendChild(cbInputGroup);
                            cbInputGroup.appendChild(cbInput);
                            cbInputGroup.appendChild(eval(cbLblString));
                        } else if (radioInputCtr == 4) {
                            cbLbl1.setAttribute("class", "KPCheckboxLbl");
                            cbLbl1.setAttribute("id", "RadioInputLbl" + x.toString());

                            cbLbl2.setAttribute("class", "KPCheckboxLbl");
                            cbLbl2.setAttribute("id", "RadioInputLbl" + x.toString());

                            cbLbl3.setAttribute("class", "KPCheckboxLbl");
                            cbLbl3.setAttribute("id", "RadioInputLbl" + x.toString());

                            cbLbl4.setAttribute("class", "KPCheckboxLbl");
                            cbLbl4.setAttribute("id", "RadioInputLbl" + x.toString());

                            cbLbl1.innerText = data[i].Answer01;
                            cbLbl2.innerText = data[i].Answer02;
                            cbLbl3.innerText = data[i].Answer03;
                            cbLbl4.innerText = data[i].Answer04;

                            var cbLblString = "cbLbl" + x;
                            cbDiv.appendChild(cbInputGroup);
                            cbInputGroup.appendChild(cbInput);
                            cbInputGroup.appendChild(eval(cbLblString));
                        } else if (radioInputCtr == 3) {
                            cbLbl1.setAttribute("class", "KPCheckboxLbl");
                            cbLbl1.setAttribute("id", "RadioInputLbl" + x.toString());

                            cbLbl2.setAttribute("class", "KPCheckboxLbl");
                            cbLbl2.setAttribute("id", "RadioInputLbl" + x.toString());

                            cbLbl3.setAttribute("class", "KPCheckboxLbl");
                            cbLbl3.setAttribute("id", "RadioInputLbl" + x.toString());

                            cbLbl1.innerText = data[i].Answer01;
                            cbLbl2.innerText = data[i].Answer02;
                            cbLbl3.innerText = data[i].Answer03;

                            var cbLblString = "cbLbl" + x;
                            cbDiv.appendChild(cbInputGroup);
                            cbInputGroup.appendChild(cbInput);
                            cbInputGroup.appendChild(eval(cbLblString));
                        } else if (radioInputCtr == 2) {
                            cbLbl1.setAttribute("class", "KPCheckboxLbl");
                            cbLbl1.setAttribute("id", "RadioInputLbl" + x.toString());

                            cbLbl2.setAttribute("class", "KPCheckboxLbl");
                            cbLbl2.setAttribute("id", "RadioInputLbl" + x.toString());

                            cbLbl1.innerText = data[i].Answer01;
                            cbLbl2.innerText = data[i].Answer02;

                            var cbLblString = "cbLbl" + x;
                            cbDiv.appendChild(cbInputGroup);
                            cbInputGroup.appendChild(cbInput);
                            cbInputGroup.appendChild(eval(cbLblString));
                        } else if (radioInputCtr == 1) {
                            cbLbl1.setAttribute("class", "KPCheckboxLbl");
                            cbLbl1.setAttribute("id", "RadioInputLbl" + x.toString());

                            cbLbl1.innerText = data[i].Answer01;

                            var cbLblString = "cbLbl" + x;
                            cbDiv.appendChild(cbInputGroup);
                            cbInputGroup.appendChild(cbInput);
                            cbInputGroup.appendChild(eval(cbLblString));
                        }
                    }
                } else if (data[i].GCQuestionType == "X553^002") {
                    formSection.appendChild(qname);
                    formSection.appendChild(cbDiv);

                    var radioInputCtr;
                    if (data[i].Answer01 != "" && data[i].Answer02 != "" && data[i].Answer03 != "" && data[i].Answer04 != "" && data[i].Answer05 != "") {
                        radioInputCtr = 5;
                    } else if (data[i].Answer01 != "" && data[i].Answer02 != "" && data[i].Answer03 != "" && data[i].Answer04 != "" && data[i].Answer05 == "" ||
                       data[i].Answer01 != "" && data[i].Answer02 != "" && data[i].Answer03 != "" && data[i].Answer04 == "" && data[i].Answer05 != "" ||
                       data[i].Answer01 != "" && data[i].Answer02 != "" && data[i].Answer03 == "" && data[i].Answer04 != "" && data[i].Answer05 != "" ||
                       data[i].Answer01 != "" && data[i].Answer02 == "" && data[i].Answer03 != "" && data[i].Answer04 != "" && data[i].Answer05 != "" ||
                       data[i].Answer01 == "" && data[i].Answer02 == "" && data[i].Answer03 != "" && data[i].Answer04 != "" && data[i].Answer05 != "") {
                        radioInputCtr = 4;
                    } else if (data[i].Answer01 != "" && data[i].Answer02 != "" && data[i].Answer03 != "" && data[i].Answer04 == "" && data[i].Answer05 == "" ||
                       data[i].Answer01 != "" && data[i].Answer02 != "" && data[i].Answer03 == "" && data[i].Answer04 == "" && data[i].Answer05 != "" ||
                       data[i].Answer01 != "" && data[i].Answer02 == "" && data[i].Answer03 == "" && data[i].Answer04 != "" && data[i].Answer05 != "" ||
                       data[i].Answer01 == "" && data[i].Answer02 == "" && data[i].Answer03 != "" && data[i].Answer04 != "" && data[i].Answer05 != "") {
                        radioInputCtr = 3;
                    } else if (data[i].Answer01 != "" && data[i].Answer02 != "" && data[i].Answer03 == "" && data[i].Answer04 == "" && data[i].Answer05 == "" ||
                       data[i].Answer01 != "" && data[i].Answer02 == "" && data[i].Answer03 == "" && data[i].Answer04 == "" && data[i].Answer05 != "" ||
                       data[i].Answer01 == "" && data[i].Answer02 == "" && data[i].Answer03 == "" && data[i].Answer04 != "" && data[i].Answer05 != "") {
                        radioInputCtr = 2;
                    } else {
                        radioInputCtr = 1;
                    }
                    for (var x = 1; x <= radioInputCtr; x++) {

                        // create checkbox input container
                        var mcbInputGroup = document.createElement("div");
                        mcbInputGroup.setAttribute("class", "checkboxgroup");

                        var cbInput = document.createElement("input");
                        cbInput.setAttribute("type", "Checkbox");
                        cbInput.setAttribute("class", "KPMCheckbox");
                        cbInput.setAttribute("name", "Question" + i.toString());
                        cbInput.setAttribute("value", eval('data[i].Answer0' + x.toString()));

                        // create checkbox input label
                        var cbLbl1 = document.createElement("label");
                        var cbLbl2 = document.createElement("label");
                        var cbLbl3 = document.createElement("label");
                        var cbLbl4 = document.createElement("label");
                        var cbLbl5 = document.createElement("label");
                        if (radioInputCtr == 5) {
                            cbLbl1.setAttribute("class", "KPCheckboxLbl");
                            cbLbl1.setAttribute("id", "RadioInputLbl" + x.toString());

                            cbLbl2.setAttribute("class", "KPCheckboxLbl");
                            cbLbl2.setAttribute("id", "RadioInputLbl" + x.toString());

                            cbLbl3.setAttribute("class", "KPCheckboxLbl");
                            cbLbl3.setAttribute("id", "RadioInputLbl" + x.toString());

                            cbLbl4.setAttribute("class", "KPCheckboxLbl");
                            cbLbl4.setAttribute("id", "RadioInputLbl" + x.toString());

                            cbLbl5.setAttribute("class", "KPCheckboxLbl");
                            cbLbl5.setAttribute("id", "RadioInputLbl" + x.toString());

                            cbLbl1.innerText = data[i].Answer01;
                            cbLbl2.innerText = data[i].Answer02;
                            cbLbl3.innerText = data[i].Answer03;
                            cbLbl4.innerText = data[i].Answer04;
                            cbLbl5.innerText = data[i].Answer05;

                            var cbLblString = "cbLbl" + x;
                            cbDiv.appendChild(mcbInputGroup);
                            mcbInputGroup.appendChild(cbInput);
                            mcbInputGroup.appendChild(eval(cbLblString));
                        } else if (radioInputCtr == 4) {
                            cbLbl1.setAttribute("class", "KPCheckboxLbl");
                            cbLbl1.setAttribute("id", "RadioInputLbl" + x.toString());

                            cbLbl2.setAttribute("class", "KPCheckboxLbl");
                            cbLbl2.setAttribute("id", "RadioInputLbl" + x.toString());

                            cbLbl3.setAttribute("class", "KPCheckboxLbl");
                            cbLbl3.setAttribute("id", "RadioInputLbl" + x.toString());

                            cbLbl4.setAttribute("class", "KPCheckboxLbl");
                            cbLbl4.setAttribute("id", "RadioInputLbl" + x.toString());

                            cbLbl1.innerText = data[i].Answer01;
                            cbLbl2.innerText = data[i].Answer02;
                            cbLbl3.innerText = data[i].Answer03;
                            cbLbl4.innerText = data[i].Answer04;

                            var cbLblString = "cbLbl" + x;
                            cbDiv.appendChild(mcbInputGroup);
                            mcbInputGroup.appendChild(cbInput);
                            mcbInputGroup.appendChild(eval(cbLblString));
                        } else if (radioInputCtr == 3) {
                            cbLbl1.setAttribute("class", "KPCheckboxLbl");
                            cbLbl1.setAttribute("id", "RadioInputLbl" + x.toString());

                            cbLbl2.setAttribute("class", "KPCheckboxLbl");
                            cbLbl2.setAttribute("id", "RadioInputLbl" + x.toString());

                            cbLbl3.setAttribute("class", "KPCheckboxLbl");
                            cbLbl3.setAttribute("id", "RadioInputLbl" + x.toString());

                            cbLbl1.innerText = data[i].Answer01;
                            cbLbl2.innerText = data[i].Answer02;
                            cbLbl3.innerText = data[i].Answer03;

                            var cbLblString = "cbLbl" + x;
                            cbDiv.appendChild(mcbInputGroup);
                            mcbInputGroup.appendChild(cbInput);
                            mcbInputGroup.appendChild(eval(cbLblString));
                        } else if (radioInputCtr == 2) {
                            cbLbl1.setAttribute("class", "KPCheckboxLbl");
                            cbLbl1.setAttribute("id", "RadioInputLbl" + x.toString());

                            cbLbl2.setAttribute("class", "KPCheckboxLbl");
                            cbLbl2.setAttribute("id", "RadioInputLbl" + x.toString());

                            cbLbl1.innerText = data[i].Answer01;
                            cbLbl2.innerText = data[i].Answer02;

                            var cbLblString = "cbLbl" + x;
                            cbDiv.appendChild(mcbInputGroup);
                            mcbInputGroup.appendChild(cbInput);
                            mcbInputGroup.appendChild(eval(cbLblString));
                        } else if (radioInputCtr == 1) {
                            cbLbl1.setAttribute("class", "KPCheckboxLbl");
                            cbLbl1.setAttribute("id", "RadioInputLbl" + x.toString());

                            cbLbl1.innerText = data[i].Answer01;

                            var cbLblString = "cbLbl" + x;
                            cbDiv.appendChild(mcbInputGroup);
                            mcbInputGroup.appendChild(cbInput);
                            mcbInputGroup.appendChild(eval(cbLblString));
                        }
                    }
                } else if (data[i].GCQuestionType == "X553^003") {
                    var qname = document.createElement("label");
                    qname.setAttribute("class", "surveyKPLabel")
                    qname.innerText = data[i].QuestionName;

                    var qTxtInput = document.createElement("input")
                    qTxtInput.setAttribute("type", "text");
                    qTxtInput.setAttribute("name", data[i].QuestionCode);
                    qTxtInput.setAttribute("class", "surveyKPInput");
                    qTxtInput.setAttribute("id", "surveyKPEssayInput" + i.toString());

                    formSection.appendChild(qname);
                    formSection.appendChild(qTxtInput);
                }
            }
        }

    
    </script>
</asp:Content>
