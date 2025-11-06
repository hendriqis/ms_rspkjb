<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MPPatientPageList.master"
    AutoEventWireup="true" CodeBehind="OPAssessment2.aspx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.OPAssessment2" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>

<asp:Content ID="Content4" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnPatientStatusSave" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbsave.png")%>' alt="" /><div>
            <%=GetLabel("Save")%></div>
    </li>
    <li id="btnDiscardChanges" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbcancel.png")%>' alt="" /><div>
            <%=GetLabel("Discard Changes")%></div>
    </li>
    <li id="btnDischargePatient" runat="server" crudmode="R" visible="false">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbset.png")%>' alt="" /><div>
            <%=GetLabel("Complete Session")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script id="dxis_OPSOAPAssesment1" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.ui.core.js")%>' type='text/javascript'></script>
    <script id="dxis_OPSOAPAssesment2" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.ui.widget2.js")%>' type='text/javascript'></script>
    <script id="dxis_OPSOAPAssesment3" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.ui.accordion.js")%>' type='text/javascript'></script>

    <style type="text/css">
        #divCloseShortcutPreviousVisit                { position: absolute;right: 10px;top: 5px;display: none;padding: 0px 5px;background: #CC0000;cursor: pointer;color: #FFFFFF;-webkit-border-radius: 999px;-moz-border-radius: 999px;border-radius: 999px;font-weight:bolder;font-size:12px; }
        #divOpenShortcutPreviousVisit                 { cursor:pointer; }
        #divContainerShortcutPreviousVisit            { bottom:0;right:470px;position:fixed;height:30px;overflow:hidden; }
        #divContainerShortcutPreviousVisit > div      { border:1px solid #ecf0f1;padding:5px; background-color: #bdc3c7; width:650px;height:550px;border-bottom:0px;-webkit-border-radius: 5px; -moz-border-radius: 5px; border-radius: 5px; }
             
        div.containerPreviousVisitUl ul  { margin:0; padding:0; margin-left:25px;}
        div.containerPreviousVisitUl ul:not(:first-child) { margin-top: 10px; }
        div.containerPreviousVisitUl ul li  { list-style-type:none; font-size: 12px; padding-bottom:5px; }
        div.containerPreviousVisitUl ul li span  { color:#3E3EE3; }
        div.containerPreviousVisitUl a        { font-size:12px; color:#3E93E3; cursor: pointer; float: right; margin-right: 20px; }
        div.containerPreviousVisitUl a:hover  { text-decoration: underline; }
        div.headerHistoryContent  { text-align: left; border-bottom:1px groove black; font-size: 13px; margin:0; padding:0; margin-bottom: 5px; }
    
        .divPreviousVisitContentTitle { margin-left:25px; font-weight:bold; font-size:11px; text-decoration:underline}
        .divPreviousVisitContent { margin-left:25px;  font-size:12px}
    
        .divNotAvailableContent { margin-left:25px; font-size:11px; font-style:italic; color:red}
                
        .templatePatientBodyDiagram
        {
            padding: 10px;
        }
        .templatePatientBodyDiagram .containerImage
        {
            float: left;
            display: table-cell;
            vertical-align: middle;
            border: 1px solid #AAA;
            width: 300px;
            height: 300px;
            margin-right: 20px;
            text-align: center;
            position: relative;
        }
        .templatePatientBodyDiagram .containerImage img
        {
            max-height: 300px;
            max-width: 300px;
            position: absolute;
            top: 0;
            bottom: 0;
            left: 0;
            right: 0;
            margin: auto;
        }
        .templatePatientBodyDiagram .spLabel
        {
            display: inline-block;
            width: 120px;
            font-weight: bolder;
        }
        .templatePatientBodyDiagram .spValue
        {
            margin-left: 10px;
        }
    </style>
    <script type="text/javascript" id="dxss_erpatientstatus1">
        var registrationDateTimeInString = '<%=RegistrationDateTime%>';
        var registrationDateTime = Methods.stringToDateTime(registrationDateTimeInString);
        $(function () {
            setDatePicker('<%=txtServiceDate.ClientID %>');
            $('#<%=txtServiceDate.ClientID %>').datepicker('option', 'maxDate', '0');

            setDatePicker('<%=txtFollowupVisitDate.ClientID %>');
            $('#<%=txtFollowupVisitDate.ClientID %>').datepicker('option', 'minDate', '0');

            setDatePicker('<%=txtDischargeDate.ClientID %>');
            $('#<%=txtDischargeDate.ClientID %>').datepicker('option', 'maxDate', '0');

            $('#<%:txtSubjectiveText.ClientID %>').focus();

            $('#<%=btnPatientStatusSave.ClientID %>').click(function (evt) {
                if (IsValid(evt, 'fsPatientStatus', 'mpPatientStatus')) {
                    if (validateTime($('#<%=txtServiceTime.ClientID %>').val())) {
                        onCustomButtonClick('save');
                        $('#<%=hdnIsChanged.ClientID %>').val('0');
                    }
                    else {
                        showToast('Warning', 'Format Waktu yang diinput salah');                              
                    }
                }
            });

            function validateTime(timeValue) {
                var result = true;
                if (timeValue == "" || timeValue.indexOf(":") < 0 || timeValue.length != 5) {
                    result = false;
                }
                else {
                    var sHours = timeValue.split(':')[0];
                    var sMinutes = timeValue.split(':')[1];

                    if (sHours == "" || isNaN(sHours) || parseInt(sHours) > 23) {
                        result = false;
                    }
                    else if (parseInt(sHours) == 0)
                        sHours = "00";
                    else if (sHours < 10)
                        sHours = "0" + sHours;

                    if (sMinutes == "" || isNaN(sMinutes) || parseInt(sMinutes) > 59) {
                        result = false;
                    }
                    else if (parseInt(sMinutes) == 0)
                        sMinutes = "00";
                    else if (sMinutes < 10)
                        sMinutes = "0" + sMinutes;
                }
                return result;
            }

            $('#<%=btnDiscardChanges.ClientID %>').click(function (evt) {
                if ($('#<%=hdnIsChanged.ClientID %>').val() == '1') {
                    var message = "Are you sure to discard your changes ?";
                    showToastConfirmation(message, function (result) {
                        if (result) {
                            $('#<%=hdnIsChanged.ClientID %>').val('0');
                            location.reload(true);
                        }
                    });
                }
            });

            $('#<%=btnDischargePatient.ClientID %>').click(function (evt) {
                var message = "Do you want to discharge this patient (Complete Patient Session) ?";
                showToastConfirmation(message, function (result) {
                    if (result) {
//                        var url = ResolveUrl("~/Program/PatientPage/_PopupEntry/Charges/DoctorFeeEntryCtl1.ascx");
//                        openUserControlPopup(url, '', 'Doctor Fee Entry', 800, 600);
                        onCustomButtonClick('discharge');
                    }
                });
            });

            $('#<%=grdAllergyView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdAllergyView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnAllergyID.ClientID %>').val($(this).find('.keyField').html());
            });
            $('#<%=grdAllergyView.ClientID %> tr:eq(1)').click();

            $('#<%=grdVitalSignView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdVitalSignView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnVitalSignRecordID.ClientID %>').val($(this).find('.keyField').html());
            });
            $('#<%=grdVitalSignView.ClientID %> tr:eq(1)').click();

            $('#<%=grdROSView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdROSView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnReviewOfSystemID.ClientID %>').val($(this).find('.keyField').html());
            });
            $('#<%=grdROSView.ClientID %> tr:eq(1)').click();

            $('#<%=grdDiagnosisView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdDiagnosisView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnDiagnosisID.ClientID %>').val($(this).find('.keyField').html());
            });
            $('#<%=grdDiagnosisView.ClientID %> tr:eq(1)').click();

            $('#<%=grdLaboratoryView.ClientID %> > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
                if ($(this).attr('class') != 'selected') {
                    $('#<%=grdLaboratoryView.ClientID %> tr.selected').removeClass('selected');
                    $(this).addClass('selected');
                    $('#<%=hdnLaboratoryTestOrderID.ClientID %>').val($(this).find('.keyField').html());
                }
            });

            $('#<%=grdImagingView.ClientID %> > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
                if ($(this).attr('class') != 'selected') {
                    $('#<%=grdImagingView.ClientID %> tr.selected').removeClass('selected');
                    $(this).addClass('selected');
                    $('#<%=hdnImagingTestOrderID.ClientID %>').val($(this).find('.keyField').html());
                }
            });

            $('#<%=grdDiagnosticView.ClientID %> > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
                if ($(this).attr('class') != 'selected') {
                    $('#<%=grdDiagnosticView.ClientID %> tr.selected').removeClass('selected');
                    $(this).addClass('selected');
                    $('#<%=hdnDiagnosticTestOrderID.ClientID %>').val($(this).find('.keyField').html());
                }
            });

            $('.btnAllergyButton').click(function () {
                if (($('#<%=txtAllergenName.ClientID %>').val() != '' && $('#<%=txtReaction.ClientID %>').val() != '')) {
                    if ($('#<%=hdnAllergyProcessMode.ClientID %>').val() == "1")
                        cbpAllergy.PerformCallback('add');
                    else
                        cbpAllergy.PerformCallback('edit');
                }
                else {
                    showToast("ERROR", "You should fill allergen name and allergy reaction !");
                }
            });

            $('#btnBodyDiagramContainerPrev').click(function () {
                if ($('#<%=hdnPageCount.ClientID %>').val() != '0')
                    cbpBodyDiagramView.PerformCallback('prev');
            });
            $('#btnBodyDiagramContainerNext').click(function () {
                if ($('#<%=hdnPageCount.ClientID %>').val() != '0')
                    cbpBodyDiagramView.PerformCallback('next');
            });

            $('#imgEditBodyDiagram').live('click', function () {
                openUserControlPopup("~/Program/PatientPage/_PopupEntry/BodyDiagram/BodyDiagramSOAPEdit1Ctl.ascx", $('#<%=hdnBodyDiagramID.ClientID %>').val(), "Body Diagram", 1200, 600);
            });

            $('#imgDeleteBodyDiagram').live('click', function () {
                var message = "Are you sure to delete this body diagram ?";
                showToastConfirmation(message, function (result) {
                    if (result) {
                        cbpDeleteBodyDiagram.PerformCallback();
                    }
                });
            });

            $('.btnApplyDiagnosis').click(function () {
                if ((cboDiagnosisType.GetValue() != '' && $('#<%=txtDiagnosisText.ClientID %>').val() != '')) {
                    if ($('#<%=hdnDiagnosisProcessMode.ClientID %>').val() == "1")
                        cbpDiagnosis.PerformCallback('add');
                    else
                        cbpDiagnosis.PerformCallback('edit');
                }
                else {
                    showToast("ERROR", "You should fill Diagnosis Type and Name field !");
                }
            });

            $('.btnCancelDiagnosis').click(function () {
                ResetDiagnosisEntryControls();
            });

            $('#<%=txtSubjectiveText.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });

            $('#<%=txtObjectiveText.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });

            $('#<%=txtAssessmentText.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });

            $('#<%=txtPlanningText.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });

            $('#<%=txtDischargeDate.ClientID %>').change(function () {
                onDischargeDateTimeChange();
            });
            $('#<%=txtDischargeTime.ClientID %>').change(function () {
                onDischargeDateTimeChange();
            });

            onDischargeDateTimeChange();

            $('#<%=chkIsNewChiefComplaint.ClientID %>').change(function () {
                var isChecked = $(this).is(":checked");
                if (isChecked) 
                    $('#<%:txtPreviousChiefComplaint.ClientID %>').removeAttr('readonly');
                else
                    $('#<%:txtPreviousChiefComplaint.ClientID %>').attr('readonly', 'readonly');                  
            });

            //#region Subjective Template
            $('#btnAddSubjectiveTemplate.imgLink').click(function () {
                if ($('#<%=txtSubjectiveText.ClientID %>').val() != '') {
                    var text = $('#<%=txtSubjectiveText.ClientID %>').val();
                    openUserControlPopup("~/Program/PatientPage/_PopupEntry/TemplateText/UpdatePhysicianTextCtl1.ascx", "X058^03|" + text, "Physician Subjective Template Text", 700, 500);
                }
            });

            $('#<%:txtSubjectiveText.ClientID %>').blur(function () {
                onTxtSubjectiveTextChanged($(this).val());
            });
            //#endregion

            //#region Objective Template
            $('#btnAddObjectiveTemplate.imgLink').click(function () {
                if ($('#<%=txtObjectiveText.ClientID %>').val() != '') {
                    var text = $('#<%=txtObjectiveText.ClientID %>').val();
                    openUserControlPopup("~/Program/PatientPage/_PopupEntry/TemplateText/UpdatePhysicianTextCtl1.ascx", "X058^04|" + text, "Physician Objective Template Text", 700, 500);
                }
            });

            $('#<%:txtObjectiveText.ClientID %>').blur(function () {
                onTxtObjectiveTextChanged($(this).val());
            });
            //#endregion

            //#region Assessment Template
            $('#btnAddAssessmentTemplate.imgLink').click(function () {
                if ($('#<%=txtAssessmentText.ClientID %>').val() != '') {
                    var text = $('#<%=txtAssessmentText.ClientID %>').val();
                    openUserControlPopup("~/Program/PatientPage/_PopupEntry/TemplateText/UpdatePhysicianTextCtl1.ascx", "X058^05|" + text, "Physician Objective Template Text", 700, 500);
                }
            });

            $('#<%:txtAssessmentText.ClientID %>').blur(function () {
                onTxtAssessmentTextChanged($(this).val());
            });
            //#endregion

            //#region Planning Template
            $('#btnAddPlanningTemplate.imgLink').click(function () {
                if ($('#<%=txtPlanningText.ClientID %>').val() != '') {
                    var text = $('#<%=txtPlanningText.ClientID %>').val();
                    openUserControlPopup("~/Program/PatientPage/_PopupEntry/TemplateText/UpdatePhysicianTextCtl1.ascx", "X058^06|" + text, "Physician Objective Template Text", 700, 500);
                }
            });

            $('#<%:txtPlanningText.ClientID %>').blur(function () {
                onTxtPlanningTextChanged($(this).val());
            });
            //#endregion

            $('#divOpenShortcutPreviousVisit').click(function () {
                $('#divContainerShortcutPreviousVisit').animate({ height: 550 }, 550, function () {
                    $('#divCloseShortcutPreviousVisit').show();
                });
            });


            $('#divCloseShortcutPreviousVisit').click(function () {
                $('#divContainerShortcutPreviousVisit').animate({ height: 30 }, 550, function () {
                    $('#divCloseShortcutPreviousVisit').hide();
                });
            });

            $('#listInstruction').show();
            $('#listInstruction').accordion({ fillSpace: true });

            registerCollapseExpandHandler();
        });


        //#region Subjective
        $('#lblSubjective').die('click');
        $('#lblSubjective').live('click', function (evt) {
            var filterExpression = " UserID = " + <%=HttpUtility.HtmlEncode(GetUserID())%> + " AND GCTextTemplateGroup ='X058^03'";
            openSearchDialog('physicianText', filterExpression, function (value) {
                onTxtSubjectiveTextChanged(value+";");
            });
        });

        function onTxtSubjectiveTextChanged(value) {
            if (value.length <= 6 && value.slice(-1) == ";") {
                var filterExpression = " UserID = " + <%=HttpUtility.HtmlEncode(GetUserID())%> + " AND TemplateCode = '" + value.slice(0, -1) + "' AND GCTextTemplateGroup ='X058^03'";
                Methods.getObject('GetvPhysicianTextList', filterExpression, function (obj) {
                    if (obj != null)
                        if ($('#<%=txtSubjectiveText.ClientID %>').val() != '') {
                            var message = "Are you sure to replace the Subjective Text from your template text ?";
                            displayConfirmationMessageBox("SOAP : AUTO TEXT", message, function (result) {
                                if (result) {
                                    $('#<%=txtSubjectiveText.ClientID %>').val(obj.TemplateText);
                                }
                            });
                        }
                        else {
                            $('#<%=txtSubjectiveText.ClientID %>').val(obj.TemplateText);
                        }
                });
            }
        }
        //#endregion

        //#region Objective
        $('#lblObjective').die('click');
        $('#lblObjective').live('click', function (evt) {
            var filterExpression = " UserID = " + <%=HttpUtility.HtmlEncode(GetUserID())%> + " AND GCTextTemplateGroup ='X058^04'";
            openSearchDialog('physicianText', filterExpression, function (value) {
                onTxtObjectiveTextChanged(value+";");
            });
        });

        function onTxtObjectiveTextChanged(value) {
            if (value.length <= 6 && value.slice(-1) == ";") {
                var filterExpression = " UserID = " + <%=HttpUtility.HtmlEncode(GetUserID())%> + " AND TemplateCode = '" + value.slice(0, -1) + "' AND GCTextTemplateGroup ='X058^04'";
                Methods.getObject('GetvPhysicianTextList', filterExpression, function (obj) {
                    if (obj != null)
                        if ($('#<%=txtObjectiveText.ClientID %>').val() != '') {
                            var message = "Are you sure to replace the Objective Text from your template text ?";
                            displayConfirmationMessageBox("SOAP : AUTO TEXT", message, function (result) {
                                if (result) {
                                    $('#<%=txtObjectiveText.ClientID %>').val(obj.TemplateText);
                                }
                            });
                        }
                        else {
                            $('#<%=txtObjectiveText.ClientID %>').val(obj.TemplateText);
                        }
                });
            }
        }
        //#endregion

        //#region Assessment
        $('#lblAssessment').die('click');
        $('#lblAssessment').live('click', function (evt) {
            var filterExpression = " UserID = " + <%=HttpUtility.HtmlEncode(GetUserID())%> + " AND GCTextTemplateGroup ='X058^05'";
            openSearchDialog('physicianText', filterExpression, function (value) {
                onTxtAssessmentTextChanged(value+";");
            });
        });

        function onTxtAssessmentTextChanged(value) {
            if (value.length <= 6 && value.slice(-1) == ";") {
                var filterExpression = " UserID = " + <%=HttpUtility.HtmlEncode(GetUserID())%> + " AND TemplateCode = '" + value.slice(0, -1) + "' AND GCTextTemplateGroup ='X058^05'";
                Methods.getObject('GetvPhysicianTextList', filterExpression, function (obj) {
                    if (obj != null)
                        if ($('#<%=txtAssessmentText.ClientID %>').val() != '') {
                            var message = "Are you sure to replace the Assessment Text from your template text ?";
                            displayConfirmationMessageBox("SOAP : AUTO TEXT", message, function (result) {
                                if (result) {
                                    $('#<%=txtAssessmentText.ClientID %>').val(obj.TemplateText);
                                }
                            });
                        }
                        else {
                            $('#<%=txtAssessmentText.ClientID %>').val(obj.TemplateText);
                        }
                });
            }
        }
        //#endregion

        //#region Planning
        $('#lblPlanning').die('click');
        $('#lblPlanning').live('click', function (evt) {
            var filterExpression = " UserID = " + <%=HttpUtility.HtmlEncode(GetUserID())%> + " AND GCTextTemplateGroup ='X058^06'";
            openSearchDialog('physicianText', filterExpression, function (value) {
                onTxtPlanningTextChanged(value+";");
            });
        });

        function onTxtPlanningTextChanged(value) {
            if (value.length <= 6 && value.slice(-1) == ";") {
                var filterExpression = " UserID = " + <%=HttpUtility.HtmlEncode(GetUserID())%> + " AND TemplateCode = '" + value.slice(0, -1) + "' AND GCTextTemplateGroup ='X058^06'";
                Methods.getObject('GetvPhysicianTextList', filterExpression, function (obj) {
                    if (obj != null)
                        if ($('#<%=txtPlanningText.ClientID %>').val() != '') {
                            var message = "Are you sure to replace the Planning Text from your template text ?";
                            displayConfirmationMessageBox("SOAP : AUTO TEXT", message, function (result) {
                                if (result) {
                                    $('#<%=txtPlanningText.ClientID %>').val(obj.TemplateText);
                                }
                            });
                        }
                        else {
                            $('#<%=txtPlanningText.ClientID %>').val(obj.TemplateText);
                        }
                });
            }
        }
        //#endregion

        function dateDiff(date1, date2) {
            var diff = date2 - date1;
            return isNaN(diff) ? NaN : {
                diff: diff,
                ms: Math.floor(diff % 1000),
                s: Math.floor(diff / 1000 % 60),
                m: Math.floor(diff / 60000 % 60),
                h: Math.floor(diff / 3600000 % 24),
                d: Math.floor(diff / 86400000)
            };
        }

        function onDischargeDateTimeChange() {
            var dischargeDate = Methods.getDatePickerDate($('#<%=txtDischargeDate.ClientID %>').val());
            var dischargeTime = $('#<%=txtDischargeTime.ClientID %>').val();
            var dischargeDateTimeInString = Methods.dateToString(dischargeDate) + dischargeTime.replace(':', '');
            var diff = dateDiff(registrationDateTime, Methods.stringToDateTime(dischargeDateTimeInString));
            $('#<%=hdnLOSInDay.ClientID %>').val(diff.d);
            $('#<%=hdnLOSInHour.ClientID %>').val(diff.h);
            $('#<%=hdnLOSInMinute.ClientID %>').val(diff.m);
            $('#<%=txtLengthOfVisit.ClientID %>').val(diff.d + 'dd ' + diff.h + 'hh ' + diff.m + 'mm');
        }

        function onClosingCopyPopupDialog() {
        }

        function onCopyFromPreviousVisit() {
            var message = "Are you sure to copy from Previous Visit ?";
            showToastConfirmation(message, function (result) {
                if (result) cbpSOAPCopy.PerformCallback(); ;
            });
        }

        function onCbpSOAPCopyEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'success') {
                location.reload(true);
            }
            else {
                showToast('Copy Process : FAILED', 'Error Message : ' + param[1]);
            }
        }

        function onAfterCustomClickSuccess(type) {
            if (type == "discharge") {
                exitPatientPage();
            }
        }

        function onAfterSelectFromInstructionTemplate(param) {
            if (param != '') {
                var item = param.split("|").join("\n");
                var newText = item;
                if ($('#<%=txtSubjectiveText.ClientID %>').val() != "") {
                    newText = $('#<%=txtSubjectiveText.ClientID %>').val() + "\n" + item;
                }
                $('#<%=txtSubjectiveText.ClientID %>').val(newText);
            }
        }

        var pageCount = parseInt('<%=gridVitalSignPageCount %>');
        var procedurePageCount = parseInt('<%=gridProcedurePageCount %>');
        $(function () {
            setPaging($("#vitalSignPaging"), pageCount, function (page) {
                cbpVitalSignView.PerformCallback('changepage|' + page);
            });
            setPaging($("#procedurePaging"), procedurePageCount, function (page) {
                cbpProcedureView.PerformCallback('changepage|' + page);
            });
        });

        //#region Allergy

        var pageCount = parseInt('<%=gridAllergyPageCount %>');
        $(function () {
            setPaging($("#llergyPaging"), pageCount, function (page) {
                cbpAllergyView.PerformCallback('changepage|' + page);
            });
        });

        function getSelectedAllergy() {
            return $('#<%=grdAllergyView.ClientID %> tr.selected');
        }

        function onCbpAllergyEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == '1') {
                if (param[1] == "edit")
                    $('#<%=hdnAllergyProcessMode.ClientID %>').val('1');

                ResetAllergyEntryControls();
                cbpAllergyView.PerformCallback('refresh');
            }
            else if (param[0] == '0') {
                showToast("ERROR", 'Error Message : ' + param[2]);
            }
            else
                $('#<%=grdAllergyView.ClientID %> tr:eq(1)').click();
        }

        function GetCurrentSelectedAllergy(s) {
            var $tr = $(s).closest('tr').parent().closest('tr');
            var idx = $('#<%=grdAllergyView.ClientID %> tr').index($tr);
            $('#<%=grdAllergyView.ClientID %> tr:eq(' + idx + ')').click();

            $row = $('#<%=grdAllergyView.ClientID %> tr.selected');
            var selectedObj = {};

            $row.find('input[type=hidden]').each(function () {
                selectedObj[$(this).attr('bindingfield')] = $(this).val();
            });

            return selectedObj;
        }

        function SetAllergyEntityToControl(param) {
            var selectedObj = {};
            selectedObj = GetCurrentSelectedAllergy(param);

            cboAllergenType.SetValue(selectedObj.GCAllergenType);
            $('#<%=txtAllergenName.ClientID %>').val(selectedObj.Allergen);
            $('#<%=txtReaction.ClientID %>').val(selectedObj.Reaction);
        }


        $('.imgEditAllergy.imgLink').die('click');
        $('.imgEditAllergy.imgLink').live('click', function () {
            SetAllergyEntityToControl(this);
            $('#<%=hdnAllergyProcessMode.ClientID %>').val('0');
        });

        $('.imgDeleteAllergy.imgLink').die('click');
        $('.imgDeleteAllergy.imgLink').live('click', function () {
            var selectedObj = {};
            selectedObj = GetCurrentSelectedAllergy(this);

            var message = "Are you sure to delete this allergy record for this patient with allergen name <b>'" + selectedObj.Allergen + "'</b> ?";
            showToastConfirmation(message, function (result) {
                if (result) {
                    cbpAllergy.PerformCallback('delete');
                }
            });
        });

        function ResetAllergyEntryControls(s) {
            cboAllergenType.SetValue('');
            $('#<%=txtAllergenName.ClientID %>').val('');
            $('#<%=txtReaction.ClientID %>').val('');
        }

        function onCbpAllergyViewEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdAllergyView.ClientID %> tr:eq(1)').click();

                setPaging($("#allergyPaging"), pageCount, function (page) {
                    cbpAllergyView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdAllergyView.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        //#region Vital Sign Paging
        $('#lblAddVitalSign').die('click');
        $('#lblAddVitalSign').live('click', function (evt) {
            openUserControlPopup("~/Program/PatientPage/_PopupEntry/VitalSignEntry1Ctl.ascx", "0|", "Vital Sign & Indicator", 700, 500);
        });

        $('.imgEditVitalSign.imgLink').die('click');
        $('.imgEditVitalSign.imgLink').live('click', function () {
            openUserControlPopup("~/Program/PatientPage/_PopupEntry/VitalSignEntry1Ctl.ascx", "0|" + $('#<%=hdnVitalSignRecordID.ClientID %>').val(), "Vital Sign & Indicator", 700, 500);
        });

        $('.imgDeleteVitalSign.imgLink').die('click');
        $('.imgDeleteVitalSign.imgLink').live('click', function () {
            var message = "Are you sure to delete this vital sign record ?";
            showToastConfirmation(message, function (result) {
                if (result) {
                    cbpDeleteVitalSign.PerformCallback();
                }
            });
        });

        function onCbpVitalSignViewEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdVitalSignView.ClientID %> tr:eq(1)').click();

                setPaging($("#vitalSignPaging"), pageCount, function (page) {
                    cbpVitalSignView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdVitalSignView.ClientID %> tr:eq(1)').click();
        }

        function onCbpVitalSignDeleteEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == '1') {
                cbpVitalSignView.PerformCallback('refresh');
            }
            else {
                showToast("ERROR", 'Error Message : ' + param[1]);
            }
        }

        function onRefreshVitalSignGrid() {
            cbpVitalSignView.PerformCallback('refresh');
        }

        function onAfterAddVitalSign(param) {
            if (param != '') {
                var newText = param;
                if ($('#<%=txtObjectiveText.ClientID %>').val() != "") {
                    newText = $('#<%=txtObjectiveText.ClientID %>').val() + "\n" + newText;
                }
                $('#<%=txtObjectiveText.ClientID %>').val(newText);
            }
        }
        //#endregion

        //#region Review of System
        function GetCurrentSelectedROS(s) {
            var $tr = $(s).closest('tr').parent().closest('tr');
            var idx = $('#<%=grdROSView.ClientID %> tr').index($tr);
            $('#<%=grdROSView.ClientID %> tr:eq(' + idx + ')').click();

            $row = $('#<%=grdROSView.ClientID %> tr.selected');
            var selectedObj = {};

            $row.find('input[type=hidden]').each(function () {
                selectedObj[$(this).attr('bindingfield')] = $(this).val();
            });

            return selectedObj;
        }

        $('#lblAddROS').die('click');
        $('#lblAddROS').live('click', function (evt) {
            openUserControlPopup("~/Program/PatientPage/_PopupEntry/ROSEntry1Ctl.ascx", "", "Review of System", 700, 500);
        });

        $('.imgEditROS.imgLink').die('click');
        $('.imgEditROS.imgLink').live('click', function () {
            var selectedObj = {};
            selectedObj = GetCurrentSelectedROS(this);
            $('#<%=hdnReviewOfSystemID.ClientID %>').val(selectedObj.ID);
            openUserControlPopup("~/Program/PatientPage/_PopupEntry/ROSEntry1Ctl.ascx", $('#<%=hdnReviewOfSystemID.ClientID %>').val(), "Review of System", 700, 500);
        });

        $('.imgDeleteROS.imgLink').die('click');
        $('.imgDeleteROS.imgLink').live('click', function () {
            var message = "Are you sure to delete this physical examination record ?";
            showToastConfirmation(message, function (result) {
                if (result) {
                    cbpDeleteROS.PerformCallback();
                }
            });
        });

        var pageCount = parseInt('<%=gridROSPageCount %>');
        $(function () {
            setPaging($("#rosPaging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
        });

        function onCbpROSViewEndCallback(s) {
            $('#containerImgLoadingView').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdROSView.ClientID %> tr:eq(1)').click();

                setPaging($("#rosPaging"), pageCount, function (page) {
                    cbpROSView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdROSView.ClientID %> tr:eq(1)').click();
        }

        function onCbpROSDeleteEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == '1') {
                cbpROSView.PerformCallback('refresh');
            }
            else {
                showToast("ERROR", param[1]);
            }
        }

        function onRefreshROSGrid() {
            cbpROSView.PerformCallback('refresh');
        }

        function onAfterAddReviewOfSystem(param) {
            if (param != '') {
                var newText = param;
                if ($('#<%=txtObjectiveText.ClientID %>').val() != "") {
                    newText = $('#<%=txtObjectiveText.ClientID %>').val() + "\n" + newText;
                }
                $('#<%=txtObjectiveText.ClientID %>').val(newText);
            }
        }
        //#endregion

        //#region Body Diagram
        $('#lblAddBodyDiagram').die('click');
        $('#lblAddBodyDiagram').live('click', function (evt) {
            openUserControlPopup("~/Program/PatientPage/_PopupEntry/BodyDiagram/BodyDiagramSOAPAdd1Ctl.ascx", "", "Body Diagram", 1200, 600);
        });

        function onCbpBodyDiagramViewEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == 'count') {
                if (param[1] != '0') {
                    $('#<%=divBodyDiagram.ClientID %>').show();
                    $('#<%=tblEmpty.ClientID %>').hide();
                }
                else {
                    $('#<%=divBodyDiagram.ClientID %>').hide();
                    $('#<%=tblEmpty.ClientID %>').show();
                }

                $('#<%=hdnPageCount.ClientID %>').val(param[1]);
                $('#<%=hdnPageIndex.ClientID %>').val('0');
            }
            else if (param[0] == 'index')
                $('#<%=hdnPageIndex.ClientID %>').val(param[1]);
            hideLoadingPanel();
        }

        function onRefreshBodyDiagram(filterExpression) {
            if (filterExpression == 'edit')
                cbpBodyDiagramView.PerformCallback('edit');
            else
                cbpBodyDiagramView.PerformCallback('refresh');
        }

        function onCbpBodyDiagramDeleteEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == '1') {
                cbpBodyDiagramView.PerformCallback('refresh');
            }
            else {
                showToast("ERROR", param[1]);
            }
        }
        //endregion

        //#region Diagnosis
        function onLedDiagnoseLostFocus(led) {
            var diagnoseID = led.GetValueText();
            $('#<%=hdnEntryDiagnoseID.ClientID %>').val(diagnoseID);
            $('#<%=hdnEntryDiagnoseText.ClientID %>').val(led.GetDisplayText());
            if ($('#<%=txtDiagnosisText.ClientID %>').val() == "") {
                $('#<%=txtDiagnosisText.ClientID %>').val($('#<%=hdnEntryDiagnoseText.ClientID %>').val());    
            }
        }

        function GetCurrentSelectedDiagnosis(s) {
            var $tr = $(s).closest('tr').parent().closest('tr');
            var idx = $('#<%=grdDiagnosisView.ClientID %> tr').index($tr);
            $('#<%=grdDiagnosisView.ClientID %> tr:eq(' + idx + ')').click();

            $row = $('#<%=grdDiagnosisView.ClientID %> tr.selected');
            var selectedObj = {};

            $row.find('input[type=hidden]').each(function () {
                selectedObj[$(this).attr('bindingfield')] = $(this).val();
            });

            return selectedObj;
        }

        function SetDiagnosisEntityToControl(param) {
            var selectedObj = {};
            selectedObj = GetCurrentSelectedDiagnosis(param);

            cboDiagnosisType.SetValue(selectedObj.GCDiagnoseType);
            cboDiagnosisStatus.SetValue(selectedObj.GCDifferentialStatus);
            ledDiagnose.SetValue(selectedObj.DiagnoseID);
            $('#<%=txtDiagnosisText.ClientID %>').val(selectedObj.DiagnosisText);
        }

        function ResetDiagnosisEntryControls(s) {
            if ($('#<%=hdnIsMainDiagnosisExists.ClientID %>').val() == "0")
                cboDiagnosisType.SetValue(Constant.DiagnosisType.MAIN_DIAGNOSIS);
            else
                cboDiagnosisType.SetValue(Constant.DiagnosisType.COMPLICATION);

            cboDiagnosisStatus.SetValue(Constant.DiagnosisStatus.UNDER_INVESTIGATION);
            ledDiagnose.SetValue('');
            $('#<%=txtDiagnosisText.ClientID %>').val('');
        }

        $('.imgEditDiagnosis.imgLink').die('click');
        $('.imgEditDiagnosis.imgLink').live('click', function () {
            SetDiagnosisEntityToControl(this);
            $('#<%=hdnDiagnosisProcessMode.ClientID %>').val('0');
        });

        $('.imgDeleteDiagnosis.imgLink').die('click');
        $('.imgDeleteDiagnosis.imgLink').live('click', function () {
            var selectedObj = {};
            selectedObj = GetCurrentSelectedDiagnosis(this);

            var message = "Are you sure to delete this diagnosis <b>'" + selectedObj.DiagnosisText + "'</b> for this patient ?";
            showToastConfirmation(message, function (result) {
                if (result) {
                    cbpDiagnosis.PerformCallback('delete');
                }
            });
        });

        var pageCount = parseInt('<%=gridDiagnosisPageCount %>');
        $(function () {
            setPaging($("#diagnosisPaging"), pageCount, function (page) {
                cbpDiagnosisView.PerformCallback('changepage|' + page);
            });
        });

        function onCbpDiagnosisViewEndCallback(s) {
            var param = s.cpResult.split('|');
            var isMainDiagnosisExists = s.cpRetval;

            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdDiagnosisView.ClientID %> tr:eq(1)').click();

                setPaging($("#diagnosisPaging"), pageCount, function (page) {
                    cbpDiagnosisView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdDiagnosisView.ClientID %> tr:eq(1)').click();

            if (isMainDiagnosisExists == "0")
                cboDiagnosisType.SetValue(Constant.DiagnosisType.MAIN_DIAGNOSIS);
            else
                cboDiagnosisType.SetValue(Constant.DiagnosisType.COMPLICATION);
            cboDiagnosisStatus.SetValue(Constant.DiagnosisStatus.UNDER_INVESTIGATION);
            ledDiagnose.SetValue('');
            $('#<%=txtDiagnosisText.ClientID %>').val('');

            $('#<%=hdnIsMainDiagnosisExists.ClientID %>').val(isMainDiagnosisExists);
        }

        function onCbpDiagnosisEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == '1') {
                if (param[1] == "edit")
                    $('#<%=hdnDiagnosisProcessMode.ClientID %>').val('1');

                ResetDiagnosisEntryControls();
                cbpDiagnosisView.PerformCallback('refresh');
            }
            else if (param[0] == '0') {
                showToast("ERROR", 'Error Message : ' + param[2]);
            }
            else
                $('#<%=grdDiagnosisView.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        //#region Laboratory
        function GetCurrentSelectedLaboratory(s) {
            var $tr = $(s).closest('tr').parent().closest('tr');
            var idx = $('#<%=grdLaboratoryView.ClientID %> tr').index($tr);
            $('#<%=grdLaboratoryView.ClientID %> tr:eq(' + idx + ')').click();

            $row = $('#<%=grdLaboratoryView.ClientID %> tr.selected');
            var selectedObj = {};

            $row.find('input[type=hidden]').each(function () {
                selectedObj[$(this).attr('bindingfield')] = $(this).val();
            });

            return selectedObj;
        }

        $('#lblAddLabOrder').die('click');
        $('#lblAddLabOrder').live('click', function (evt) {
            onBeforeOpenTrxPopup();
            var labServiceUnitID = $('#<%=hdnLaboratoryServiceUnitID.ClientID %>').val();
            var performDate = $('#<%=hdnDatePickerToday.ClientID %>').val()
            var performTime = $('#<%=hdnTimeToday.ClientID %>').val();
            var clinicalNotes = $('#<%=hdnDiagnosisSummary.ClientID %>').val();
            var chiefComplaint = $('#<%=txtAssessmentText.ClientID %>').val();
            var testOrderID = "0";

            var param = "X001^004" + "|" + testOrderID + "|" + clinicalNotes + "|" + chiefComplaint + "|" + "X125^001" + "|" + 'Langsung Dikerjakan' + "|" + performDate + "|" + performTime;
            openUserControlPopup("~/Program/PatientPage/_PopupEntry/CPOE/TestOrderItemQuickPicksCtl1.ascx", param, "Laboratory Test Order", 1200, 600);
        });

        $('.imgAddLabOrderDt.imgLink').die('click');
        $('.imgAddLabOrderDt.imgLink').live('click', function () {
            onBeforeOpenTrxPopup();
            var selectedObj = {};
            selectedObj = GetCurrentSelectedLaboratory(this);
            $('#<%=hdnLaboratoryTestOrderID.ClientID %>').val(selectedObj.TestOrderID);
            var labServiceUnitID = $('#<%=hdnLaboratoryServiceUnitID.ClientID %>').val();
            var performDate = $('#<%=hdnDatePickerToday.ClientID %>').val()
            var performTime = $('#<%=hdnTimeToday.ClientID %>').val();
            var clinicalNotes = $('#<%=hdnDiagnosisSummary.ClientID %>').val();
            var chiefComplaint = $('#<%=txtAssessmentText.ClientID %>').val();
            var testOrderID = "0";
            var param = "X001^004" + "|" + testOrderID + "|" + clinicalNotes + "|" + chiefComplaint + "|" + "X125^001" + "|" + 'Langsung Dikerjakan' + "|" + performDate + "|" + performTime;
            openUserControlPopup("~/Program/PatientPage/_PopupEntry/CPOE/TestOrderItemQuickPicksCtl1.ascx", param, "Laboratory Test Order", 1200, 600);
        });

        $('.imgEditLabOrder.imgLink').die('click');
        $('.imgEditLabOrder.imgLink').live('click', function () {
            onBeforeOpenTrxPopup();
            var selectedObj = {};
            selectedObj = GetCurrentSelectedLaboratory(this);
            $('#<%=hdnLaboratoryTestOrderID.ClientID %>').val(selectedObj.TestOrderID);

            var param = "LB|" + $('#<%=hdnLaboratoryTestOrderID.ClientID %>').val() + "|" + $('#<%=hdnLaboratoryServiceUnitID.ClientID %>').val();
            openUserControlPopup("~/Program/PatientPage/_PopupEntry/CPOE/TestOrderItemEditCtl1.ascx", param, "Edit Laboratory Order", 700, 500);
        });

        $('.imgDeleteLabOrder.imgLink').die('click');
        $('.imgDeleteLabOrder.imgLink').live('click', function () {
            var message = "Are you sure to delete this Laboratory Test Order record ?";
            showToastConfirmation(message, function (result) {
                if (result) {
                    onBeforeOpenTrxPopup();
                    cbpDeleteTestOrder.PerformCallback('LB');
                }
            });
        });

        $('.imgSendLabOrder.imgLink').die('click');
        $('.imgSendLabOrder.imgLink').live('click', function () {
            var selectedObj = {};
            selectedObj = GetCurrentSelectedLaboratory(this);
            $('#<%=hdnLaboratoryTestOrderID.ClientID %>').val(selectedObj.TestOrderID);

            if ($('#<%:hdnLaboratoryTestOrderID.ClientID %>').val() == "") {
                showToast("ERROR", "There is no order to be sent !");
            }
            else {
                var message = "Send your order to Service Unit ?";
                showToastConfirmation(message, function (result) {
                    if (result) cbpSendOrder.PerformCallback('sendOrder|LB|' + $('#<%:hdnLaboratoryTestOrderID.ClientID %>').val());
                });
            }
        });

        var pageCount = parseInt('<%=gridLaboratoryPageCount %>');
        $(function () {
            setPaging($("#laboratoryPaging"), pageCount, function (page) {
                cbpLaboratoryView.PerformCallback('changepage|' + page);
            });
        });

        function onCbpLaboratoryViewEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdLaboratoryView.ClientID %> tr:eq(1)').click();

                setPaging($("#laboratoryPaging"), pageCount, function (page) {
                    cbpLaboratoryView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdLaboratoryView.ClientID %> tr:eq(1)').click();
        }

        function onRefreshLaboratoryGrid() {
            cbpLaboratoryView.PerformCallback('refresh');
        }

        function onAfterAddLabTestOrder(param) {
            if (param != '') {
                var item = param.split(";").join("\n");
                var newText = item;
                if ($('#<%=txtPlanningText.ClientID %>').val() != "") {
                    newText = $('#<%=txtPlanningText.ClientID %>').val() + "\n" + "LAB:" + "\n" + newText;
                }
                else {
                    newText = "LAB:" + "\n" + newText;
                }
                $('#<%=txtPlanningText.ClientID %>').val(newText);
            }
        }
        //#endregion

        //#region Imaging
        function GetCurrentSelectedImaging(s) {
            var $tr = $(s).closest('tr').parent().closest('tr');
            var idx = $('#<%=grdImagingView.ClientID %> tr').index($tr);
            $('#<%=grdImagingView.ClientID %> tr:eq(' + idx + ')').click();

            $row = $('#<%=grdImagingView.ClientID %> tr.selected');
            var selectedObj = {};

            $row.find('input[type=hidden]').each(function () {
                selectedObj[$(this).attr('bindingfield')] = $(this).val();
            });

            return selectedObj;
        }

        $('#lblAddImagingOrder').die('click');
        $('#lblAddImagingOrder').live('click', function (evt) {
            onBeforeOpenTrxPopup();
            var radiologyServiceUnitID = $('#<%=hdnImagingServiceUnitID.ClientID %>').val();
            var performDate = $('#<%=hdnDatePickerToday.ClientID %>').val()
            var performTime = $('#<%=hdnTimeToday.ClientID %>').val();
            var clinicalNotes = $('#<%=hdnDiagnosisSummary.ClientID %>').val();
            var chiefComplaint = $('#<%=txtAssessmentText.ClientID %>').val();
            var testOrderID = "0";
            var param = "X001^005" + "|" + testOrderID + "|" + clinicalNotes + "|" + chiefComplaint + "|" + "X125^001" + "|" + 'Langsung Dikerjakan' + "|" + performDate + "|" + performTime;
            openUserControlPopup("~/Program/PatientPage/_PopupEntry/CPOE/TestOrderItemQuickPicksCtl1.ascx", param, "Imaging Test Order", 1200, 600);
        });

        $('.imgAddImagingOrderDt.imgLink').die('click');
        $('.imgAddImagingOrderDt.imgLink').live('click', function () {
            onBeforeOpenTrxPopup();
            var selectedObj = {};
            selectedObj = GetCurrentSelectedImaging(this);
            $('#<%=hdnImagingTestOrderID.ClientID %>').val(selectedObj.TestOrderID);
            var radiologyServiceUnitID = $('#<%=hdnImagingServiceUnitID.ClientID %>').val();
            var performDate = $('#<%=hdnDatePickerToday.ClientID %>').val()
            var performTime = $('#<%=hdnTimeToday.ClientID %>').val();
            var clinicalNotes = $('#<%=hdnDiagnosisSummary.ClientID %>').val();
            var chiefComplaint = $('#<%=txtAssessmentText.ClientID %>').val();
            var testOrderID = "0";
            var param = "X001^005" + "|" + testOrderID + "|" + clinicalNotes + "|" + chiefComplaint + "|" + "X125^001" + "|" + 'Langsung Dikerjakan' + "|" + performDate + "|" + performTime;
            openUserControlPopup("~/Program/PatientPage/_PopupEntry/CPOE/TestOrderItemQuickPicksCtl1.ascx", param, "Imaging Test Order", 1200, 600);
        });

        $('.imgEditImagingOrder.imgLink').die('click');
        $('.imgEditImagingOrder.imgLink').live('click', function () {
            onBeforeOpenTrxPopup();
            var selectedObj = {};
            selectedObj = GetCurrentSelectedImaging(this);
            $('#<%=hdnImagingTestOrderID.ClientID %>').val(selectedObj.TestOrderID);

            var param = "LB|" + $('#<%=hdnImagingTestOrderID.ClientID %>').val() + "|" + $('#<%=hdnImagingServiceUnitID.ClientID %>').val();
            openUserControlPopup("~/Program/PatientPage/_PopupEntry/CPOE/TestOrderItemEditCtl1.ascx", param, "Edit Imaging Order", 700, 500);
        });

        $('.imgDeleteImagingOrder.imgLink').die('click');
        $('.imgDeleteImagingOrder.imgLink').live('click', function () {
            var message = "Are you sure to delete this Imaging Test Order record ?";
            showToastConfirmation(message, function (result) {
                if (result) {
                    onBeforeOpenTrxPopup();
                    cbpDeleteTestOrder.PerformCallback('IS');
                }
            });
        });

        $('.imgSendImagingOrder.imgLink').die('click');
        $('.imgSendImagingOrder.imgLink').live('click', function () {
            var selectedObj = {};
            selectedObj = GetCurrentSelectedImaging(this);
            $('#<%=hdnImagingTestOrderID.ClientID %>').val(selectedObj.TestOrderID);

            if ($('#<%:hdnImagingTestOrderID.ClientID %>').val() == "") {
                showToast("ERROR", 'Error Message : ' + "There is no order to be sent !");
            }
            else {
                var message = "Send your order to Service Unit ?";
                showToastConfirmation(message, function (result) {
                    if (result) cbpSendOrder.PerformCallback('sendOrder|IS|' + $('#<%:hdnImagingTestOrderID.ClientID %>').val());
                });
            }
        });

        var pageCount = parseInt('<%=gridImagingPageCount %>');
        $(function () {
            setPaging($("#imagingPaging"), pageCount, function (page) {
                cbpImagingView.PerformCallback('changepage|' + page);
            });
        });

        function onCbpImagingViewEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdImagingView.ClientID %> tr:eq(1)').click();

                setPaging($("#imagingPaging"), pageCount, function (page) {
                    cbpImagingView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdImagingView.ClientID %> tr:eq(1)').click();
        }

        function onRefreshImagingGrid() {
            cbpImagingView.PerformCallback('refresh');
        }

        function onAfterAddImagingTestOrder(param) {
            if (param != '') {
                var item = param.split(";").join("\n");
                var newText = item;
                if ($('#<%=txtPlanningText.ClientID %>').val() != "") {
                    newText = $('#<%=txtPlanningText.ClientID %>').val() + "\n" + "RAD:" + "\n" + newText;
                }
                else {
                    newText = "RAD:" + "\n" + newText;
                }
                $('#<%=txtPlanningText.ClientID %>').val(newText);
            }
        }
        //#endregion

        //#region Diagnostic
        function GetCurrentSelectedDiagnostic(s) {
            var $tr = $(s).closest('tr').parent().closest('tr');
            var idx = $('#<%=grdDiagnosticView.ClientID %> tr').index($tr);
            $('#<%=grdDiagnosticView.ClientID %> tr:eq(' + idx + ')').click();

            $row = $('#<%=grdDiagnosticView.ClientID %> tr.selected');
            var selectedObj = {};

            $row.find('input[type=hidden]').each(function () {
                selectedObj[$(this).attr('bindingfield')] = $(this).val();
            });

            return selectedObj;
        }

        $('#lblAddDiagnosticOrder').die('click');
        $('#lblAddDiagnosticOrder').live('click', function (evt) {
            onBeforeOpenTrxPopup();
            var serviceUnitID = $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val();
            var performDate = $('#<%=hdnDatePickerToday.ClientID %>').val()
            var performTime = $('#<%=hdnTimeToday.ClientID %>').val();
            var clinicalNotes = $('#<%=hdnDiagnosisSummary.ClientID %>').val();
            var chiefComplaint = $('#<%=txtAssessmentText.ClientID %>').val();
            var testOrderID = "0";
            var param = "X001^006" + "|" + serviceUnitID + "|" + testOrderID + "|" + clinicalNotes + "|" + chiefComplaint + "|" + "X125^001" + "|" + 'Langsung Dikerjakan' + "|" + performDate + "|" + performTime;
            openUserControlPopup("~/Program/PatientPage/_PopupEntry/CPOE/OtherTestOrderItemQuickPicksCtl1.ascx", param, "Diagnostic Order", 1200, 600);
        });

        $('.imgAddDiagnosticOrderDt.imgLink').die('click');
        $('.imgAddDiagnosticOrderDt.imgLink').live('click', function () {
            onBeforeOpenTrxPopup();
            var selectedObj = {};
            selectedObj = GetCurrentSelectedDiagnostic(this);
            $('#<%=hdnTestOrderHealthcareServiceUnitID.ClientID %>').val(selectedObj.HealthcareServiceUnitID);
            $('#<%=hdnDiagnosticTestOrderID.ClientID %>').val(selectedObj.TestOrderID);
            var serviceUnitID = $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val();
            var performDate = $('#<%=hdnDatePickerToday.ClientID %>').val()
            var performTime = $('#<%=hdnTimeToday.ClientID %>').val();
            var clinicalNotes = $('#<%=hdnDiagnosisSummary.ClientID %>').val();
            var chiefComplaint = $('#<%=txtAssessmentText.ClientID %>').val();
            var testOrderID = selectedObj.TestOrderID;
            var param = "X001^006" + "|" + serviceUnitID + "|" + testOrderID + "|" + clinicalNotes + "|" + chiefComplaint + "|" + "X125^001" + "|" + 'Langsung Dikerjakan' + "|" + performDate + "|" + performTime;
            openUserControlPopup("~/Program/PatientPage/_PopupEntry/CPOE/OtherTestOrderItemQuickPicksCtl1.ascx", param, "Diagnostic Test Order", 1200, 600);
        });

        $('.imgEditDiagnosticOrder.imgLink').die('click');
        $('.imgEditDiagnosticOrder.imgLink').live('click', function () {
            onBeforeOpenTrxPopup();
            var selectedObj = {};
            selectedObj = GetCurrentSelectedDiagnostic(this);
            $('#<%=hdnTestOrderHealthcareServiceUnitID.ClientID %>').val(selectedObj.HealthcareServiceUnitID);
            $('#<%=hdnDiagnosticTestOrderID.ClientID %>').val(selectedObj.TestOrderID);

            var param = "MD|" + $('#<%=hdnDiagnosticTestOrderID.ClientID %>').val() + "|" + $('#<%=hdnTestOrderHealthcareServiceUnitID.ClientID %>').val();
            openUserControlPopup("~/Program/PatientPage/_PopupEntry/CPOE/TestOrderItemEditCtl1.ascx", param, "Edit Diagnostic Order", 700, 500);
        });

        $('.imgDeleteDiagnosticOrder.imgLink').die('click');
        $('.imgDeleteDiagnosticOrder.imgLink').live('click', function () {
            var message = "Are you sure to delete this Diagnostic Test Order record ?";
            showToastConfirmation(message, function (result) {
                if (result) {
                    onBeforeOpenTrxPopup();
                    cbpDeleteTestOrder.PerformCallback('MD');
                }
            });
        });

        $('.imgSendDiagnosticOrder.imgLink').die('click');
        $('.imgSendDiagnosticOrder.imgLink').live('click', function () {
            var selectedObj = {};
            selectedObj = GetCurrentSelectedDiagnostic(this);
            $('#<%=hdnTestOrderHealthcareServiceUnitID.ClientID %>').val(selectedObj.HealthcareServiceUnitID);
            $('#<%=hdnDiagnosticTestOrderID.ClientID %>').val(selectedObj.TestOrderID);

            if ($('#<%:hdnDiagnosticTestOrderID.ClientID %>').val() == "") {
                showToast("ERROR", 'Error Message : ' + "There is no order to be sent !");
            }
            else {
                var message = "Send your order to Service Unit ?";
                displayConfirmationMessageBox('ORDER PEMERIKSAAN',message, function (result) {
                    if (result) {
                        cbpSendOrder.PerformCallback('sendOrder|MD|' + $('#<%:hdnDiagnosticTestOrderID.ClientID %>').val());
                    }
                });
            }
        });

        var pageCount = parseInt('<%=gridDiagnosticPageCount %>');
        $(function () {
            setPaging($("#diagnosticPaging"), pageCount, function (page) {
                cbpDiagnosticView.PerformCallback('changepage|' + page);
            });
        });

        function onCbpDiagnosticViewEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdDiagnosticView.ClientID %> tr:eq(1)').click();

                setPaging($("#diagnosticPaging"), pageCount, function (page) {
                    cbpDiagnosticView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdDiagnosticView.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        function onRefreshDiagnosticGrid() {
            cbpDiagnosticView.PerformCallback('refresh');
        }

        function onAfterAddDiagnosticTestOrder(param) {
            if (param != '') {
                var item = param.split(";").join("\n");
                var newText = item;
                if ($('#<%=txtPlanningText.ClientID %>').val() != "") {
                    newText = $('#<%=txtPlanningText.ClientID %>').val() + "\n" + "PTx:" + "\n" + newText;
                }
                else {
                    newText = "PTx:" + "\n" + newText;
                }
                $('#<%=txtPlanningText.ClientID %>').val(newText);
            }
        }

        function onCbpDeleteTestOrderEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == '1') {
                if (param[1] == 'LB') {
                    cbpLaboratoryView.PerformCallback('refresh');
                }
                else if (param[1] == 'IS') {
                    cbpImagingView.PerformCallback('refresh');
                }
                else {
                    cbpDiagnosticView.PerformCallback('refresh');
                }
            }
            else {
                showToast("ERROR", param[1]);
            }
        }

        function onCbpSendOrderEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'sendOrder') {
                if (param[2] == 'success') {
                    showToast('Send Success', 'The test order was successfully sent to Service Unit.');

                    if (param[1] == 'LB') {
                        onRefreshLaboratoryGrid();
                    }
                    else if (param[1] == 'IS') {
                        onRefreshImagingGrid();
                    }
                    else if (param[1] == 'MD') {
                        onRefreshDiagnosticGrid();
                    }
                }
                else {
                    showToast('SEND ORDER : FAILED', 'Error Message : ' + param[3]);
                }
            }
        }

        //#region Change Page - Save
        function onBeforeOpenTrxPopup() {
            if ($('#<%=hdnIsSaved.ClientID %>').val() == '0' && $('#<%=hdnIsChanged.ClientID %>').val() == '1') {
                onCustomButtonClick('save');
                $('#<%=hdnIsChanged.ClientID %>').val('0');
            }
        }

        function onBeforeChangePage() {
            if ($('#<%=hdnIsSaved.ClientID %>').val() == '0' && $('#<%=hdnIsChanged.ClientID %>').val() == '1') {
                var message = "Your record is not saved yet, Do you want to save ?";
                showToastConfirmation(message, function (result) {
                    if (result) {
                        onCustomButtonClick('save');
                        $('#<%=hdnIsChanged.ClientID %>').val('0');
                    }
                });
            }
            else {
                gotoNextPage();
            }
        }
        function onBeforeBackToListPage() {
            if ($('#<%=hdnIsSaved.ClientID %>').val() == '0' && $('#<%=hdnIsChanged.ClientID %>').val() == '1') {
                PromptUserToSave();
            }
            else {
                backToPatientList();
            }
        }

        function PromptUserToSave() {
            var message = "Your record is not saved yet, Do you want to save ?";
            showToastConfirmation(message, function (result) {
                if (result) {
                    onCustomButtonClick('save');
                    $('#<%=hdnIsChanged.ClientID %>').val('0');
                }
            });
        }
        //#endregion   

        function onBeforeLoadRightPanelContent(code) {
            if (code == 'patientVisit') {
                return $('#<%:hdnRegistrationID.ClientID %>').val();
            }
            else {
                return $('#<%:hdnRegistrationID.ClientID %>').val();
            }
        }

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var visitID = $('#<%:hdnVisitID.ClientID %>').val();
            var registrationID = $('#<%:hdnRegistrationID.ClientID %>').val();
            if (visitID == '') {
                errMessage.text = 'Please Select Patient Visit First!';
                return false;
            }
            else {
                if (code == 'MR000010') {
                    filterExpression.text = registrationID;
                    return true;
                }
                else {
                    filterExpression.text = visitID;
                    return true;
                }
            }
        }              
    </script>
    <div>
        <input type="hidden" id="hdnRegistrationID" value="" runat="server" />
        <input type="hidden" runat="server" id="hdnVisitID" value="" />
        <input type="hidden" runat="server" id="hdnChiefComplaintID" value="" />
        <input type="hidden" runat="server" id="hdnAppointmentID" value="" />
        <input type="hidden" value="" id="hdnVitalSignRecordID" runat="server" />
        <input type="hidden" value="" id="hdnAllergyID" runat="server" />
        <input type="hidden" value="1" id="hdnAllergyProcessMode" runat="server" />
        <input type="hidden" value="" id="hdnProcedureID" runat="server" />
        <input type="hidden" value="" id="hdnDiagnosisID" runat="server" />
        <input type="hidden" value="1" id="hdnDiagnosisProcessMode" runat="server" />
        <input type="hidden" value="" id="hdnROSText" runat="server" />
        <input type="hidden" value="" id="hdnVitalSignText" runat="server" />
        <input type="hidden" value="" id="hdnReviewOfSystemID" runat="server" />
        <input type="hidden" value="" id="hdnImagingServiceUnitID" runat="server" />
        <input type="hidden" value="" id="hdnLaboratoryServiceUnitID" runat="server" />
        <input type="hidden" value="" id="hdnTestOrderHealthcareServiceUnitID" runat="server" />
        <input type="hidden" runat="server" id="hdnHealthcareServiceUnitID" value="" />
        <input type="hidden" value="" id="hdnLaboratoryTestOrderID" runat="server" />
        <input type="hidden" value="" id="hdnImagingTestOrderID" runat="server" />
        <input type="hidden" value="" id="hdnDiagnosticTestOrderID" runat="server" />
        <input type="hidden" value="" id="hdnPrescriptionOrderID" runat="server" />
        <input type="hidden" runat="server" id="hdnPastMedicalID" value="" />
        <input type="hidden" runat="server" id="hdnTimeElapsed0hour" value="00" />
        <input type="hidden" runat="server" id="hdnTimeElapsed0minute" value="00" />
        <input type="hidden" runat="server" id="hdnTimeElapsed1hour" value="00" />
        <input type="hidden" runat="server" id="hdnTimeElapsed1minute" value="00" />
        <input type="hidden" runat="server" id="hdnDepartmentID" value="" />
        <input type="hidden" runat="server" id="hdnIsSaved" value="0" />
        <input type="hidden" runat="server" id="hdnIsChanged" value="0" />
        <input type="hidden" id="hdnPageCount" runat="server" value='0' />
        <input type="hidden" id="hdnPageIndex" runat="server" value='0' />
        <input type="hidden" id="hdnIsMainDiagnosisExists" runat="server" value='0' />
        <input type="hidden" id="hdnLOSInDay" runat="server" />
        <input type="hidden" id="hdnLOSInHour" runat="server" />
        <input type="hidden" id="hdnLOSInMinute" runat="server" />
        <input type="hidden" id="hdnIsPhysicianDischarge" runat="server" />
        <input type="hidden" id="hdnDiagnosisSummary" runat="server" />
        <input type="hidden" value="0" id="hdnPreviousVisitID" runat="server" />
        <input type="hidden" id="hdnLaboratoryOrderSummaryText" runat="server" />
        <input type="hidden" id="hdnImagingSummaryText" runat="server" />
        <input type="hidden" id="hdnOtherTestSummaryText" runat="server" />
        <input type="hidden" value="" id="hdnDatePickerToday" runat="server" />
        <input type="hidden" value="" id="hdnTimeToday" runat="server" />
        <fieldset id="mpPatientStatus">
            <table class="tblContentArea">
                <colgroup>
                    <col style="width: 45%" />
                    <col style="width: 55%" />
                </colgroup>
                <tr>
                    <td style="padding: 5px; vertical-align: top">
                        <table class="tblEntryContent">
                            <colgroup>
                                <col style="width: 150px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel" valign="top">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Tanggal dan Waktu")%></label>
                                </td>
                                <td>
                                    <table border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtServiceDate" Width="120px" CssClass="datepicker" runat="server" />
                                            </td>
                                            <td style="padding-left: 5px">
                                                <asp:TextBox ID="txtServiceTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" MaxLength="5" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" valign="top">
                                    <table border="0" cellpadding="0" cellspacing="1">
                                        <tr>
                                            <td class="tdLabel" valign="top" style="width:120px">
                                                <label id="lblSubjective" class="lblMandatory lblLink">
                                                    <%=GetLabel("Subjective")%></label>
                                            </td>
                                            <td><img class="imgLink" id="btnAddSubjectiveTemplate" title='<%=GetLabel("Add to My Subjective Template")%>' src='<%= ResolveUrl("~/Libs/Images/button/vitalsign.png")%>' alt="" /></td>
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSubjectiveText" runat="server" TextMode="MultiLine" Rows="4"
                                        Width="100%" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" valign="top">
                                    <table border="0" cellpadding="0" cellspacing="1">
                                        <tr>
                                            <td class="tdLabel" valign="top" style="width:120px">
                                                <label id="lblObjective" class="lblMandatory lblLink">
                                                    <%=GetLabel("Objective")%></label>
                                            </td>
                                            <td><img class="imgLink" id="btnAddObjectiveTemplate" title='<%=GetLabel("Add to My Objective Template")%>' src='<%= ResolveUrl("~/Libs/Images/button/vitalsign.png")%>' alt="" /></td>
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtObjectiveText" runat="server" TextMode="MultiLine" Rows="4"
                                        Width="100%" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" valign="top">
                                    <table border="0" cellpadding="0" cellspacing="1">
                                        <tr>
                                            <td class="tdLabel" valign="top" style="width:120px">
                                                <label id="lblAssessment" class="lblMandatory lblLink">
                                                    <%=GetLabel("Assessment")%></label>
                                            </td>
                                            <td><img class="imgLink" id="btnAddAssessmentTemplate" title='<%=GetLabel("Add to My Assessment Template")%>' src='<%= ResolveUrl("~/Libs/Images/button/vitalsign.png")%>' alt="" /></td>
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAssessmentText" runat="server" TextMode="MultiLine" Rows="4"
                                        Width="100%" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" valign="top">
                                    <table border="0" cellpadding="0" cellspacing="1">
                                        <tr>
                                            <td class="tdLabel" valign="top" style="width:120px">
                                                <label id="lblPlanning" class="lblMandatory lblLink">
                                                    <%=GetLabel("Planning")%></label>
                                            </td>
                                            <td><img class="imgLink" id="btnAddPlanningTemplate" title='<%=GetLabel("Add to My Planning Template")%>' src='<%= ResolveUrl("~/Libs/Images/button/vitalsign.png")%>' alt="" /></td>
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPlanningText" runat="server" TextMode="MultiLine" Rows="4"
                                        Width="100%" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    &nbsp;
                                </td>
                                <td>
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <td style="width: 50%">
                                            <asp:CheckBox ID="chkAutoAnamnesis" runat="server" Text="Autoanamnesis" Checked="false" />
                                        </td>
                                        <td style="width: 50%">
                                            <asp:CheckBox ID="chkAlloAnamnesis" runat="server" Text="Alloanamnesis / Heteroanamnesis"
                                                Checked="false" />
                                        </td>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="vertical-align: top">
                        <h4 class="h4expanded" style="display:none">
                            <%=GetLabel("Keluhan Kunjungan")%>                        
                            <b><span style="color:Red;"><asp:Label ID="lblVisitHistoryInfo1" runat="server" Text=" : dd-MMM-yyyy, Physician Name" /></span></b>
                        </h4>
                        <div class="containerTblEntryContent" style="display:none">
                            <table id="tbtPreviousVisit" style="width: 100%" runat="server">
                                <tr>
                                    <td style="width:200px" valign="top">
                                        <label class="lblMandatory" id = "lblPreviousChiefComplaint" runat="server">
                                            <%=GetLabel("Keluhan Utama")%></label>
                                        <br />
                                        <br />
                                         <asp:CheckBox ID="chkIsNewChiefComplaint" runat="server" Text="Keluhan Baru" Checked="false" Visible="false" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPreviousChiefComplaint" runat="server" TextMode="MultiLine" Rows="5"
                                            Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <h4 class="h4expanded">
                            <%=GetLabel("Pengkajian Perawat")%></h4>
                        <div class="containerTblEntryContent">
                            <table id="tblNurseAnamnesis" style="width: 100%" runat="server">
                                <tr>
                                    <td class="tdLabel" style="width: 150px; vertical-align: top">
                                        <label class="lblNormal">
                                            <%=GetLabel("Anamnesa Perawat")%></label>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtNurseAnamnesis" runat="server" Width="100%" TextMode="Multiline"
                                            Rows="5" ReadOnly="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal" runat="server" id="lblRegistrationDiagnosis">
                                            <%:GetLabel("Diagnosa Rujukan")%></label>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtDiagnosis" Width="100%" runat="server" ReadOnly="true" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <h4 class="h4collapsed">
                            <%=GetLabel("Riwayat Alergi")%></h4>
                        <div class="containerTblEntryContent containerEntryPanel1">
                            <div style="position: relative;">
                                <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                                    <tr>
                                        <td>
                                            <table border="0" cellpadding="1" cellspacing="0">
                                                <colgroup>
                                                    <col width="100px" />
                                                    <col width="100px" />
                                                    <col width="100px" />
                                                    <col width="100px" />
                                                    <col width="100px" />
                                                    <col />
                                                </colgroup>
                                                <tr>
                                                    <td colspan="6">
                                                        <asp:CheckBox ID="chkIsPatientAllergyExists" runat="server" Text="Tidak ada Alergi"
                                                            Checked="false" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label class="lblNormal">
                                                            <%=GetLabel("Tipe Alergi")%></label>
                                                    </td>
                                                    <td>
                                                        <dxe:ASPxComboBox runat="server" ID="cboAllergenType" ClientInstanceName="cboAllergenType"
                                                            Width="100px">
                                                        </dxe:ASPxComboBox>
                                                    </td>
                                                    <td>
                                                        <label class="lblNormal">
                                                            <%=GetLabel("Jenis/Nama")%></label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtAllergenName" runat="server" Width="150px" />
                                                    </td>
                                                    <td>
                                                        <label class="lblNormal">
                                                            <%=GetLabel("Reaksi")%></label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtReaction" runat="server" Width="150px" />
                                                    </td>
                                                    <td>
                                                        <input id="btnAllergyButton" type="button" value='<%= GetLabel("Simpan")%>' style="width: 80px"
                                                            class="btnAllergyButton" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <dxcp:ASPxCallbackPanel ID="cbpAllergyView" runat="server" Width="100%" ClientInstanceName="cbpAllergyView"
                                                ShowLoadingPanel="false" OnCallback="cbpAllergyView_Callback">
                                                <ClientSideEvents EndCallback="function(s,e){ onCbpAllergyViewEndCallback(s); }" />
                                                <PanelCollection>
                                                    <dx:PanelContent ID="PanelContent3" runat="server">
                                                        <asp:Panel runat="server" ID="Panel2" CssClass="pnlContainerGrid" Style="height: 300px">
                                                            <asp:GridView ID="grdAllergyView" runat="server" CssClass="grdSelected grdPatientPage"
                                                                AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                                <Columns>
                                                                    <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                    <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                                        <ItemTemplate>
                                                                            <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                                            <input type="hidden" value="<%#:Eval("Allergen") %>" bindingfield="Allergen" />
                                                                            <input type="hidden" value="<%#:Eval("GCAllergenType") %>" bindingfield="GCAllergenType" />
                                                                            <input type="hidden" value="<%#:Eval("GCAllergySource") %>" bindingfield="GCAllergySource" />
                                                                            <input type="hidden" value="<%#:Eval("GCAllergySeverity") %>" bindingfield="GCAllergySeverity" />
                                                                            <input type="hidden" value="<%#:Eval("KnownDate") %>" bindingfield="KnownDate" />
                                                                            <input type="hidden" value="<%#:Eval("Reaction") %>" bindingfield="Reaction" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20px">
                                                                        <ItemTemplate>
                                                                            <table cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td>
                                                                                        <img class="imgEditAllergy imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                            alt="" />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgDeleteAllergy imgLink" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                            alt="" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="Allergen" HeaderText="Allergen Name" HeaderStyle-Width="200px"
                                                                        HeaderStyle-HorizontalAlign="Left" />
                                                                    <asp:BoundField DataField="AllergySource" HeaderText="Finding Source" HeaderStyle-Width="150px"
                                                                        HeaderStyle-HorizontalAlign="Left" />
                                                                    <asp:BoundField DataField="DisplayDate" HeaderText="Since" ItemStyle-HorizontalAlign="Left"
                                                                        HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Left" />
                                                                    <asp:BoundField DataField="AllergySeverity" HeaderText="Severity" HeaderStyle-Width="120px"
                                                                        HeaderStyle-HorizontalAlign="Left" />
                                                                    <asp:BoundField DataField="Reaction" HeaderText="Reaction" HeaderStyle-HorizontalAlign="Left" />
                                                                </Columns>
                                                                <EmptyDataTemplate>
                                                                    <%=GetLabel("Tidak ada data alergi pasien dalam episode ini")%>
                                                                </EmptyDataTemplate>
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </dx:PanelContent>
                                                </PanelCollection>
                                            </dxcp:ASPxCallbackPanel>
                                            <div class="containerPaging">
                                                <div class="wrapperPaging">
                                                    <div id="allergyPaging">
                                                    </div>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <h4 class="h4collapsed">
                            <%=GetLabel("Pemeriksaan Tanda Vital")%></h4>
                        <div class="containerTblEntryContent">
                            <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 5px">
                                <colgroup>
                                    <col width="150px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td colspan="2">
                                        <div>
                                            <dxcp:ASPxCallbackPanel ID="cbpVitalSignView" runat="server" Width="100%" ClientInstanceName="cbpVitalSignView"
                                                ShowLoadingPanel="false" OnCallback="cbpVitalSignView_Callback">
                                                <ClientSideEvents EndCallback="function(s,e){ onCbpVitalSignViewEndCallback(s); }" />
                                                <PanelCollection>
                                                    <dx:PanelContent ID="PanelContent1" runat="server">
                                                        <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage">
                                                            <asp:GridView ID="grdVitalSignView" runat="server" CssClass="grdSelected grdPatientPage"
                                                                AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                                                OnRowDataBound="grdVitalSignView_RowDataBound">
                                                                <Columns>
                                                                    <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20px">
                                                                        <ItemTemplate>
                                                                            <table cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td>
                                                                                        <img class="imgEditVitalSign imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                            alt="" />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgDeleteVitalSign imgLink" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                            alt="" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                                        <HeaderTemplate>
                                                                            <div style="text-align: right">
                                                                                <span class="lblLink" id="lblAddVitalSign">
                                                                                    <%= GetLabel("+ Tambah Tanda Vital")%></span>
                                                                            </div>
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
                                                                                <asp:Repeater ID="rptVitalSignDt" runat="server">
                                                                                    <ItemTemplate>
                                                                                        <div style="padding-left: 20px; float: left; width: 300px;">
                                                                                            <strong>
                                                                                                <div style="width: 110px; float: left;" class="labelColumn">
                                                                                                    <%#: DataBinder.Eval(Container.DataItem, "VitalSignLabel") %></div>
                                                                                                <div style="width: 20px; float: left;">
                                                                                                    :</div>
                                                                                            </strong>
                                                                                            <div style="float: left;">
                                                                                                <%#: DataBinder.Eval(Container.DataItem, "DisplayVitalSignValue") %></div>
                                                                                        </div>
                                                                                    </ItemTemplate>
                                                                                    <FooterTemplate>
                                                                                        <br style="clear: both" />
                                                                                    </FooterTemplate>
                                                                                </asp:Repeater>
                                                                            </div>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <EmptyDataTemplate>
                                                                    <%=GetLabel("No Data To Display") %>
                                                                </EmptyDataTemplate>
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </dx:PanelContent>
                                                </PanelCollection>
                                            </dxcp:ASPxCallbackPanel>
                                            <div class="containerPaging">
                                                <div class="wrapperPaging">
                                                    <div id="vitalSignPaging">
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <h4 class="h4collapsed">
                            <%=GetLabel("Pemeriksaan Fisik")%></h4>
                        <div class="containerTblEntryContent">
                            <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 5px">
                                <tr>
                                    <td>
                                        <div style="position: relative;">
                                            <dxcp:ASPxCallbackPanel ID="cbpROSView" runat="server" Width="100%" ClientInstanceName="cbpROSView"
                                                ShowLoadingPanel="false" OnCallback="cbpROSView_Callback">
                                                <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                                                    EndCallback="function(s,e){ onCbpROSViewEndCallback(s); }" />
                                                <PanelCollection>
                                                    <dx:PanelContent ID="PanelContent4" runat="server">
                                                        <asp:Panel runat="server" ID="Panel3" CssClass="pnlContainerGridPatientPage">
                                                            <asp:GridView ID="grdROSView" runat="server" CssClass="grdSelected grdPatientPage"
                                                                AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                                                OnRowDataBound="grdROSView_RowDataBound">
                                                                <Columns>
                                                                    <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                    <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                                        <ItemTemplate>
                                                                            <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20px">
                                                                        <ItemTemplate>
                                                                            <table cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td>
                                                                                        <img class="imgEditROS imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                            alt="" />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgDeleteROS imgLink" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                            alt="" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                                        <HeaderTemplate>
                                                                            <div style="text-align: right">
                                                                                <span class="lblLink" id="lblAddROS">
                                                                                    <%= GetLabel("+ Tambah Pemeriksaan Fisik")%></span>
                                                                            </div>
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
                                                                                <asp:Repeater ID="rptReviewOfSystemDt" runat="server">
                                                                                    <ItemTemplate>
                                                                                        <div style="padding-left: 20px; float: left; width: 300px;">
                                                                                            <span <%# Eval("IsNormal").ToString() == "False" && Eval("IsNotExamined").ToString() == "False" && Eval("IsOther").ToString() == "False" ? "Style='color:red;font-style:italic'":"" %>>
                                                                                                <strong>
                                                                                                    <%#: DataBinder.Eval(Container.DataItem, "ROSystem") %>
                                                                                                    : </strong></span>&nbsp; <span <%# Eval("IsNormal").ToString() == "False" && Eval("IsNotExamined").ToString() == "False" && Eval("IsOther").ToString() == "False" ? "Style='color:red;font-style:italic'":"" %>>
                                                                                                        <%#: DataBinder.Eval(Container.DataItem, "cfRemarks")%></span>
                                                                                        </div>
                                                                                    </ItemTemplate>
                                                                                    <FooterTemplate>
                                                                                        <br style="clear: both" />
                                                                                    </FooterTemplate>
                                                                                </asp:Repeater>
                                                                            </div>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <EmptyDataTemplate>
                                                                    <%=GetLabel("Tidak ada data pemeriksaan fisik untuk pasien ini") %>
                                                                </EmptyDataTemplate>
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </dx:PanelContent>
                                                </PanelCollection>
                                            </dxcp:ASPxCallbackPanel>
                                            <div class="imgLoadingGrdView" id="containerImgLoadingView">
                                                <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                            </div>
                                            <div class="containerPaging">
                                                <div class="wrapperPaging">
                                                    <div id="rosPaging">
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <h4 class="h4collapsed">
                            <%=GetLabel("Body Diagram")%></h4>
                        <div class="containerTblEntryContent">
                            <table border="0" cellpadding="1" cellspacing="0" width="99%" style="margin-top: 1px">
                                <tr>
                                    <td>
                                        <table border="0" cellpadding="0" cellspacing="0" style="float: left; margin-top: 0px;">
                                            <tr>
                                                <td>
                                                    <span class="lblLink" id="lblAddBodyDiagram">
                                                        <%= GetLabel("+ Tambah Body Diagram")%></span>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td>
                                        <table id="tblBodyDiagramNavigation" runat="server" border="0" cellpadding="0" cellspacing="0"
                                            style="float: right; margin-top: 0px;">
                                            <tr>
                                                <td>
                                                    <img src='<%=ResolveUrl("~/Libs/Images/Icon/prev_record.png") %>' title="Prev" width="25px"
                                                        alt="" class="imgLink" id="btnBodyDiagramContainerPrev" style="margin-left: 5px;" />
                                                </td>
                                                <td>
                                                    <img src='<%=ResolveUrl("~/Libs/Images/Icon/next_record.png") %>' title="Next" width="25px"
                                                        alt="" class="imgLink" id="btnBodyDiagramContainerNext" style="margin-left: 5px;" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div style="position: relative;" id="divBodyDiagram" runat="server">
                                            <dxcp:ASPxCallbackPanel ID="cbpBodyDiagramView" runat="server" Width="100%" ClientInstanceName="cbpBodyDiagramView"
                                                ShowLoadingPanel="false" OnCallback="cbpBodyDiagramView_Callback">
                                                <ClientSideEvents EndCallback="function(s,e){ onCbpBodyDiagramViewEndCallback(s); }" />
                                                <PanelCollection>
                                                    <dx:PanelContent ID="PanelContent5" runat="server">
                                                        <asp:Panel runat="server" ID="Panel4" CssClass="pnlContainerGrid">
                                                            <div class="templatePatientBodyDiagram">
                                                                <input type="hidden" id="hdnBodyDiagramID" runat="server" value='' />
                                                                <div class="containerImage boxShadow">
                                                                    <img src='' alt="" id="imgBodyDiagram" runat="server" />
                                                                </div>
                                                                <div>
                                                                    <table border="0" cellpadding="0" cellspacing="0" style="float: right; margin-top: 0px;">
                                                                        <tr>
                                                                            <td>
                                                                                <img src='<%=ResolveUrl("~/Libs/Images/Button/edit.png") %>' title="Edit" width="25px"
                                                                                    alt="" class="imgLink" id="imgEditBodyDiagram" style="margin-left: 5px;" />
                                                                            </td>
                                                                            <td>
                                                                                <img src='<%=ResolveUrl("~/Libs/Images/Button/delete.png") %>' title="Delete" width="25px"
                                                                                    alt="" class="imgLink" id="imgDeleteBodyDiagram" style="margin-left: 5px;" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                                <span class="spLabel">
                                                                    <%=GetLabel("Diagram Name") %></span>:<span class="spValue" id="spnDiagramName" runat="server"></span><br />
                                                                <span class="spLabel">
                                                                    <%=GetLabel("Remarks") %></span>:
                                                                <br />
                                                                <asp:Repeater ID="rptRemarks" runat="server">
                                                                    <HeaderTemplate>
                                                                        <table>
                                                                            <colgroup width="20px" />
                                                                            <colgroup width="2px" />
                                                                            <colgroup width="15px" />
                                                                            <colgroup width="2px" />
                                                                            <colgroup width="60px" />
                                                                            <colgroup width="2px" />
                                                                            <colgroup width="*" />
                                                                            <colgroup width="16px" />
                                                                            <colgroup width="16px" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <tr>
                                                                            <td>
                                                                                <img alt="" style="width: 16px; height: 16px" src="<%#: ResolveUrl((string)Eval("SymbolImageUrl"))%>" />
                                                                            </td>
                                                                            <td>
                                                                                :
                                                                            </td>
                                                                            <td>
                                                                                <%#: DataBinder.Eval(Container.DataItem, "SymbolCode")%>
                                                                            </td>
                                                                            <td>
                                                                                :
                                                                            </td>
                                                                            <td>
                                                                                <%#: DataBinder.Eval(Container.DataItem, "SymbolName")%>
                                                                            </td>
                                                                            <td>
                                                                                :
                                                                            </td>
                                                                            <td>
                                                                                <%#: DataBinder.Eval(Container.DataItem, "Remarks")%>
                                                                            </td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        </table>
                                                                    </FooterTemplate>
                                                                </asp:Repeater>
                                                                <br />
                                                                <span class="spLabel">
                                                                    <%=GetLabel("Physician") %></span>:<span class="spValue" id="spnParamedicName" runat="server"></span><br />
                                                                <span class="spLabel">
                                                                    <%=GetLabel("Date/Time")%></span>:<span class="spValue" id="spnObservationDateTime"
                                                                        runat="server"></span><br />
                                                            </div>
                                                        </asp:Panel>
                                                    </dx:PanelContent>
                                                </PanelCollection>
                                            </dxcp:ASPxCallbackPanel>
                                        </div>
                                        <table id="tblEmpty" style="display: none; width: 100%" runat="server">
                                            <tr class="trEmpty">
                                                <td align="center" valign="middle">
                                                    <%=GetLabel("Tidak ada data penanda gambar untuk pasien ini") %>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <h4 class="h4collapsed">
                            <%=GetLabel("Diagnosis")%></h4>
                        <div class="containerTblEntryContent containerEntryPanel1">
                            <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px;">
                                <tr>
                                    <td>
                                        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                            <colgroup>
                                                <col width="100px" />
                                                <col width="150px" />
                                                <col width="100px" />
                                                <col width="150px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Tipe Diagnosa")%></label>
                                                </td>
                                                <td>
                                                    <dxe:ASPxComboBox runat="server" ID="cboDiagnosisType" ClientInstanceName="cboDiagnosisType"
                                                        Width="100%">
                                                    </dxe:ASPxComboBox>
                                                </td>
                                                <td style="padding-left: 5px">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("ICD X")%></label>
                                                </td>
                                                <td colspan="3">
                                                    <input type="hidden" value="" id="hdnEntryDiagnoseID" runat="server" />
                                                    <input type="hidden" value="" id="hdnEntryDiagnoseText" runat="server" />
                                                    <qis:QISSearchTextBox ID="ledDiagnose" ClientInstanceName="ledDiagnose" runat="server"
                                                        Width="99%" ValueText="DiagnoseID" FilterExpression="IsDeleted = 0 AND IsNutritionDiagnosis = 0" DisplayText="DiagnoseName"
                                                        MethodName="GetDiagnosisList">
                                                        <ClientSideEvents ValueChanged="function(s){ onLedDiagnoseLostFocus(s); }" />
                                                        <Columns>
                                                            <qis:QISSearchTextBoxColumn Caption="Diagnose Name (Code)" FieldName="DiagnoseName"
                                                                Description="i.e. Cholera" Width="500px" />
                                                        </Columns>
                                                    </qis:QISSearchTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Status Diagnosa")%></label>
                                                </td>
                                                <td>
                                                    <dxe:ASPxComboBox runat="server" ID="cboDiagnosisStatus" ClientInstanceName="cboDiagnosisStatus"
                                                        Width="100%">
                                                    </dxe:ASPxComboBox>
                                                </td>
                                                <td style="padding-left: 5px">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Diagnosa Text")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDiagnosisText" runat="server" Width="370px" />
                                                </td>
                                                <td style="padding-left: 5px">
                                                    <table border="0" cellpadding="0" cellspacing="1">
                                                        <tr>
                                                            <td>
                                                                <img class="btnApplyDiagnosis imgLink" title='<%=GetLabel("Apply")%>' src='<%= ResolveUrl("~/Libs/Images/status/done.png")%>'
                                                                    alt="" />
                                                            </td>
                                                            <td>
                                                                <img class="btnCancelDiagnosis imgLink" title='<%=GetLabel("Cancel")%>' src='<%= ResolveUrl("~/Libs/Images/status/cancel.png")%>'
                                                                    alt="" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div style="position: relative;">
                                            <dxcp:ASPxCallbackPanel ID="cbpDiagnosisView" runat="server" Width="100%" ClientInstanceName="cbpDiagnosisView"
                                                ShowLoadingPanel="false" OnCallback="cbpDiagnosisView_Callback">
                                                <ClientSideEvents EndCallback="function(s,e){ onCbpDiagnosisViewEndCallback(s); }" />
                                                <PanelCollection>
                                                    <dx:PanelContent ID="PanelContent6" runat="server">
                                                        <asp:Panel runat="server" ID="Panel5" CssClass="pnlContainerGrid" Style="height: 300px">
                                                            <asp:GridView ID="grdDiagnosisView" runat="server" CssClass="grdSelected grdPatientPage"
                                                                AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                                <Columns>
                                                                    <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                    <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                                        <ItemTemplate>
                                                                            <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                                            <input type="hidden" value="<%#:Eval("DiagnoseID") %>" bindingfield="DiagnoseID" />
                                                                            <input type="hidden" value="<%#:Eval("GCDiagnoseType") %>" bindingfield="GCDiagnoseType" />
                                                                            <input type="hidden" value="<%#:Eval("GCDifferentialStatus") %>" bindingfield="GCDifferentialStatus" />
                                                                            <input type="hidden" value="<%#:Eval("DiagnosisText") %>" bindingfield="DiagnosisText" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20px">
                                                                        <ItemTemplate>
                                                                            <table cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td>
                                                                                        <img class="imgEditDiagnosis imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                            alt="" />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgDeleteDiagnosis imgLink" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                            alt="" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                                        <HeaderTemplate>
                                                                            <%=GetLabel("Diagnose Information")%>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <div>
                                                                                <%#: Eval("DifferentialDateInString")%>,
                                                                                <%#: Eval("DifferentialTime")%>,
                                                                                <%#: Eval("cfParamedicNameEarlyDiagnosis")%></div>
                                                                            <div>
                                                                                <span style="color: Blue; font-size: 1.1em">
                                                                                    <%#: Eval("DiagnosisText")%></span> (<b><%#: Eval("DiagnoseID")%></b>)
                                                                            </div>
                                                                            <div>
                                                                                <%#: Eval("ICDBlockName")%></div>
                                                                            <div>
                                                                                <b>
                                                                                    <%#: Eval("DiagnoseType")%></b> -
                                                                                <%#: Eval("DifferentialStatus")%></div>
                                                                            <div>
                                                                                <%#: Eval("Remarks")%></div>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                                        <ItemTemplate>
                                                                            <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                                            <input type="hidden" value="<%#:Eval("DiagnoseID") %>" bindingfield="DiagnoseID" />
                                                                            <input type="hidden" value="<%#:Eval("DiagnosisText") %>" bindingfield="DiagnosisText" />
                                                                            <input type="hidden" value="<%#:Eval("GCDiagnoseType") %>" bindingfield="GCDiagnoseType" />
                                                                            <input type="hidden" value="<%#:Eval("ParamedicID") %>" bindingfield="ParamedicID" />
                                                                            <input type="hidden" value="<%#:Eval("GCDifferentialStatus") %>" bindingfield="GCDifferentialStatus" />
                                                                            <input type="hidden" value="<%#:Eval("MorphologyID") %>" bindingfield="MorphologyID" />
                                                                            <input type="hidden" value="<%#:Eval("DifferentialDateInDatePickerFormat") %>" bindingfield="DifferentialDate" />
                                                                            <input type="hidden" value="<%#:Eval("DifferentialTime") %>" bindingfield="DifferentialTime" />
                                                                            <input type="hidden" value="<%#:Eval("IsChronicDisease") %>" bindingfield="IsChronicDisease" />
                                                                            <input type="hidden" value="<%#:Eval("IsFollowUpCase") %>" bindingfield="IsFollowUpCase" />
                                                                            <input type="hidden" value="<%#:Eval("Remarks") %>" bindingfield="Remarks" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <EmptyDataTemplate>
                                                                    <%=GetLabel("Belum ada informasi diagnosa untuk pasien ini") %>
                                                                </EmptyDataTemplate>
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </dx:PanelContent>
                                                </PanelCollection>
                                            </dxcp:ASPxCallbackPanel>
                                            <div class="containerPaging">
                                                <div class="wrapperPaging">
                                                    <div id="diagnosisPaging">
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <h4 class="h4collapsed">
                            <%=GetLabel("Pemeriksaan Laboratorium")%></h4>
                        <div class="containerTblEntryContent containerEntryPanel1">
                            <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                                <tr>
                                    <td>
                                        <div style="position: relative;">
                                            <dxcp:ASPxCallbackPanel ID="cbpLaboratoryView" runat="server" Width="100%" ClientInstanceName="cbpLaboratoryView"
                                                ShowLoadingPanel="false" OnCallback="cbpLaboratoryView_Callback">
                                                <ClientSideEvents EndCallback="function(s,e){ onCbpLaboratoryViewEndCallback(s); }" />
                                                <PanelCollection>
                                                    <dx:PanelContent ID="PanelContent7" runat="server">
                                                        <asp:Panel runat="server" ID="Panel6" CssClass="pnlContainerGridPatientPage">
                                                            <asp:GridView ID="grdLaboratoryView" runat="server" CssClass="grdSelected grdPatientPage"
                                                                AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                                                OnRowDataBound="grdLaboratoryView_RowDataBound">
                                                                <Columns>
                                                                    <asp:BoundField DataField="TestOrderID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                    <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                                        <ItemTemplate>
                                                                            <input type="hidden" value="<%#:Eval("TestOrderID") %>" bindingfield="TestOrderID" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20px">
                                                                        <ItemTemplate>
                                                                            <table cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td>
                                                                                        <img class="imgAddLabOrderDt imgLink" title='<%=GetLabel("Add Detail")%>' src='<%# ResolveUrl("~/Libs/Images/Button/add.png")%>'
                                                                                            alt="" <%# Eval("IsEditable").ToString() == "False" ? "Style='display:none'":"" %> />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgEditLabOrder imgLink" title='<%=GetLabel("Edit Order")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                            alt="" <%# Eval("IsEditable").ToString() == "False" ? "Style='display:none'":"" %> />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgDeleteLabOrder imgLink" title='<%=GetLabel("Delete Order")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                            alt="" <%# Eval("IsEditable").ToString() == "False" ? "Style='display:none'":"" %> />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgSendLabOrder imgLink" title='<%=GetLabel("Send Order")%>' src='<%# ResolveUrl("~/Libs/Images/Button/done.png")%>'
                                                                                            alt="" <%# Eval("IsEditable").ToString() == "False" ? "Style='display:none'":"" %> />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                                        <HeaderTemplate>
                                                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                                                <colgroup>
                                                                                    <col style="width: 70%" />
                                                                                    <col />
                                                                                </colgroup>
                                                                                <tr>
                                                                                    <td>
                                                                                        <div>
                                                                                            <%=GetLabel("Pemeriksaan") %></div>
                                                                                    </td>
                                                                                    <td>
                                                                                        <div style="text-align: right">
                                                                                            <span class="lblLink" id="lblAddLabOrder">
                                                                                                <%= GetLabel("+ Tambah Order")%></span>
                                                                                        </div>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <table width="100%">
                                                                                <tr>
                                                                                    <td>
                                                                                        <div>
                                                                                            <%#: Eval("TestOrderDateTimeInString")%>,
                                                                                            <%#: Eval("ServiceUnitName")%>,
                                                                                            <%#: Eval("ParamedicName") %>, <span style="font-style: italic; font-weight: bold">
                                                                                                <%#: Eval("TestOrderNo")%></span></div>
                                                                                        <div style="font-style: italic">
                                                                                            <asp:Repeater ID="rptLaboratoryDt" runat="server">
                                                                                                <ItemTemplate>
                                                                                                    <div style="padding-left: 10px;">
                                                                                                        <%#: DataBinder.Eval(Container.DataItem, "ItemName1") %>
                                                                                                    </div>
                                                                                                </ItemTemplate>
                                                                                                <FooterTemplate>
                                                                                                    <br style="clear: both" />
                                                                                                </FooterTemplate>
                                                                                            </asp:Repeater>
                                                                                        </div>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <EmptyDataTemplate>
                                                                    <%=GetLabel("Tidak ada data order pemeriksaan laboratorium untuk pasien ini")%>
                                                                </EmptyDataTemplate>
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </dx:PanelContent>
                                                </PanelCollection>
                                            </dxcp:ASPxCallbackPanel>
                                            <div class="containerPaging">
                                                <div class="wrapperPaging">
                                                    <div id="laboratoryPaging">
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <h4 class="h4collapsed">
                            <%=GetLabel("Pemeriksaan Radiologi")%></h4>
                        <div class="containerTblEntryContent containerEntryPanel1">
                            <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                                <tr>
                                    <td>
                                        <div style="position: relative;">
                                            <dxcp:ASPxCallbackPanel ID="cbpImagingView" runat="server" Width="100%" ClientInstanceName="cbpImagingView"
                                                ShowLoadingPanel="false" OnCallback="cbpImagingView_Callback">
                                                <ClientSideEvents EndCallback="function(s,e){ onCbpImagingViewEndCallback(s); }" />
                                                <PanelCollection>
                                                    <dx:PanelContent ID="PanelContent8" runat="server">
                                                        <asp:Panel runat="server" ID="Panel7" CssClass="pnlContainerGridPatientPage">
                                                            <asp:GridView ID="grdImagingView" runat="server" CssClass="grdSelected grdPatientPage"
                                                                AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                                                OnRowDataBound="grdImagingView_RowDataBound">
                                                                <Columns>
                                                                    <asp:BoundField DataField="TestOrderID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                    <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                                        <ItemTemplate>
                                                                            <input type="hidden" value="<%#:Eval("TestOrderID") %>" bindingfield="TestOrderID" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20px">
                                                                        <ItemTemplate>
                                                                            <table cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td>
                                                                                        <img class="imgAddImagingOrderDt imgLink" title='<%=GetLabel("Add Detail")%>' src='<%# ResolveUrl("~/Libs/Images/Button/add.png")%>'
                                                                                            alt="" <%# Eval("IsEditable").ToString() == "False" ? "Style='display:none'":"" %> />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgEditImagingOrder imgLink" title='<%=GetLabel("Edit Order")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                            alt="" <%# Eval("IsEditable").ToString() == "False" ? "Style='display:none'":"" %> />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgDeleteImagingOrder imgLink" title='<%=GetLabel("Delete Order")%>'
                                                                                            src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>' alt="" <%# Eval("IsEditable").ToString() == "False" ? "Style='display:none'":"" %> />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgSendImagingOrder imgLink" title='<%=GetLabel("Send Order")%>' src='<%# ResolveUrl("~/Libs/Images/Button/done.png")%>'
                                                                                            alt="" <%# Eval("IsEditable").ToString() == "False" ? "Style='display:none'":"" %> />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                                        <HeaderTemplate>
                                                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                                                <colgroup>
                                                                                    <col style="width: 70%" />
                                                                                    <col />
                                                                                </colgroup>
                                                                                <tr>
                                                                                    <td>
                                                                                        <div>
                                                                                            <%=GetLabel("Pemeriksaan") %></div>
                                                                                    </td>
                                                                                    <td>
                                                                                        <div style="text-align: right">
                                                                                            <span class="lblLink" id="lblAddImagingOrder">
                                                                                                <%= GetLabel("+ Tambah Order")%></span>
                                                                                        </div>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <table width="100%">
                                                                                <tr>
                                                                                    <td>
                                                                                        <div>
                                                                                            <%#: Eval("TestOrderDateTimeInString")%>,
                                                                                            <%#: Eval("ServiceUnitName")%>,
                                                                                            <%#: Eval("ParamedicName") %>, <span style="font-style: italic; font-weight: bold">
                                                                                                <%#: Eval("TestOrderNo")%></span></div>
                                                                                        <div style="font-style: italic">
                                                                                            <asp:Repeater ID="rptImagingDt" runat="server">
                                                                                                <ItemTemplate>
                                                                                                    <div style="padding-left: 10px;">
                                                                                                        <%#: DataBinder.Eval(Container.DataItem, "ItemName1") %>
                                                                                                    </div>
                                                                                                </ItemTemplate>
                                                                                                <FooterTemplate>
                                                                                                    <br style="clear: both" />
                                                                                                </FooterTemplate>
                                                                                            </asp:Repeater>
                                                                                        </div>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <EmptyDataTemplate>
                                                                    <%=GetLabel("Tidak ada data order pemeriksaan radiologi untuk pasien ini")%>
                                                                </EmptyDataTemplate>
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </dx:PanelContent>
                                                </PanelCollection>
                                            </dxcp:ASPxCallbackPanel>
                                            <div class="containerPaging">
                                                <div class="wrapperPaging">
                                                    <div id="imagingPaging">
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <h4 class="h4collapsed">
                            <%=GetLabel("Pemeriksaan Penunjang Medis Lain-lain")%></h4>
                        <div class="containerTblEntryContent containerEntryPanel1">
                            <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                                <tr>
                                    <td>
                                        <div style="position: relative;">
                                            <dxcp:ASPxCallbackPanel ID="cbpDiagnosticView" runat="server" Width="100%" ClientInstanceName="cbpDiagnosticView"
                                                ShowLoadingPanel="false" OnCallback="cbpDiagnosticView_Callback">
                                                <ClientSideEvents EndCallback="function(s,e){ onCbpDiagnosticViewEndCallback(s); }" />
                                                <PanelCollection>
                                                    <dx:PanelContent ID="PanelContent9" runat="server">
                                                        <asp:Panel runat="server" ID="Panel8" CssClass="pnlContainerGridPatientPage">
                                                            <asp:GridView ID="grdDiagnosticView" runat="server" CssClass="grdSelected grdPatientPage"
                                                                AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdDiagnosticView_RowDataBound">
                                                                <Columns>
                                                                    <asp:BoundField DataField="TestOrderID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                    <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                                        <ItemTemplate>
                                                                            <input type="hidden" value="<%#:Eval("HealthcareServiceUnitID") %>" bindingfield="HealthcareServiceUnitID" />
                                                                            <input type="hidden" value="<%#:Eval("TestOrderID") %>" bindingfield="TestOrderID" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20px">
                                                                        <ItemTemplate>
                                                                            <table cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td>
                                                                                        <img class="imgAddDiagnosticOrderDt imgLink" title='<%=GetLabel("Add Detail")%>' src='<%# ResolveUrl("~/Libs/Images/Button/add.png")%>'
                                                                                            alt="" <%# Eval("IsEditable").ToString() == "False" ? "Style='display:none'":"" %> />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgEditDiagnosticOrder imgLink" title='<%=GetLabel("Edit Order")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                            alt="" <%# Eval("IsEditable").ToString() == "False" ? "Style='display:none'":"" %> />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgDeleteDiagnosticOrder imgLink" title='<%=GetLabel("Delete Order")%>'
                                                                                            src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>' alt="" <%# Eval("IsEditable").ToString() == "False" ? "Style='display:none'":"" %> />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgSendDiagnosticOrder imgLink" title='<%=GetLabel("Send Order")%>' src='<%# ResolveUrl("~/Libs/Images/Button/done.png")%>'
                                                                                            alt="" <%# Eval("IsEditable").ToString() == "False" ? "Style='display:none'":"" %> />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                                        <HeaderTemplate>
                                                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                                                <colgroup>
                                                                                    <col style="width: 70%" />
                                                                                    <col />
                                                                                </colgroup>
                                                                                <tr>
                                                                                    <td>
                                                                                        <div>
                                                                                            <%=GetLabel("Pemeriksaan") %></div>
                                                                                    </td>
                                                                                    <td>
                                                                                        <div style="text-align: right">
                                                                                            <span class="lblLink" id="lblAddDiagnosticOrder">
                                                                                                <%= GetLabel("+ Tambah Order")%></span>
                                                                                        </div>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <table width="100%">
                                                                                <tr>
                                                                                    <td>
                                                                                        <div>
                                                                                            <%#: Eval("TestOrderDateTimeInString")%>,
                                                                                            <%#: Eval("ServiceUnitName")%>,
                                                                                            <%#: Eval("ParamedicName") %>, <span style="font-style: italic; font-weight: bold">
                                                                                                <%#: Eval("TestOrderNo")%></span>
                                                                                        </div>
                                                                                        <div style="font-style: italic">
                                                                                            <asp:Repeater ID="rptDiagnosticDt" runat="server">
                                                                                                <ItemTemplate>
                                                                                                    <div style="padding-left: 10px;">
                                                                                                        <%#: DataBinder.Eval(Container.DataItem, "ItemName1") %>
                                                                                                    </div>
                                                                                                </ItemTemplate>
                                                                                                <FooterTemplate>
                                                                                                    <br style="clear: both" />
                                                                                                </FooterTemplate>
                                                                                            </asp:Repeater>
                                                                                        </div>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <EmptyDataTemplate>
                                                                    <%=GetLabel("Tidak ada data order pemeriksaan penunjang untuk pasien ini")%>
                                                                </EmptyDataTemplate>
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </dx:PanelContent>
                                                </PanelCollection>
                                            </dxcp:ASPxCallbackPanel>
                                            <div class="containerPaging">
                                                <div class="wrapperPaging">
                                                    <div id="diagnosticPaging">
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <h4 class="h4collapsed">
                            <%=GetLabel("Catatan : Hasil Pemeriksaan Penunjang")%></h4>
                        <div class="containerTblEntryContent">
                            <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                                <colgroup>
                                    <col width="150px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                        <label class="lblNormal" id="Label3">
                                            <%=GetLabel("Catatan Hasil Pemeriksaan") %></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDiagnosticResultSummary" runat="server" Width="100%" TextMode="Multiline"
                                            Rows="10" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <h4 class="h4collapsed">
                            <%=GetLabel("Catatan Tindakan")%></h4>
                        <div class="containerTblEntryContent">
                            <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                                <colgroup>
                                    <col width="150px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                        <label class="lblNormal" id="Label2">
                                            <%=GetLabel("Catatan Tindakan") %></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPlanningNotes" runat="server" Width="100%" TextMode="Multiline"
                                            Rows="10" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <h4 class="h4collapsed" style="display: none">
                            <%=GetLabel("Planning : Resep Farmasi")%></h4>
                        <h4 class="h4collapsed">
                            <%=GetLabel("Jadwal Kunjungan Berikutnya")%></h4>
                        <div class="containerTblEntryContent">
                            <table id="tblFollowupVisit" style="width: 100%" runat="server">
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblMandatory">
                                            <%=GetLabel("Tanggal dan Jam Disposisi")%></label>
                                    </td>
                                    <td>
                                        <table border="0" cellpadding = "0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtDischargeDate" Width="120px" CssClass="datepicker" runat="server" />
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDischargeTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" />
                                                </td>
                                                <td style="display:none">
                                                    <asp:TextBox ID="txtLengthOfVisit" ReadOnly="true" Width="100%" runat="server" Style="text-align:center" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblMandatory">
                                            <%=GetLabel("Keadaan Keluar")%></label>
                                    </td>
                                    <td colspan="3">
                                        <dxe:ASPxComboBox ID="cboPatientOutcome" ClientInstanceName="cboPatientOutcome" Width="100%" runat="server">
                                        </dxe:ASPxComboBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label class="lblNormal">
                                            <%=GetLabel("Jenis Kunjungan Berikutnya") %></label>
                                    </td>
                                    <td>
                                        <dxe:ASPxComboBox runat="server" ID="cboFollowupVisitType" ClientInstanceName="cboFollowupVisitType"
                                            Width="100%" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Tanggal Kunjungan Berikutnya")%></label></td>
                                    <td><asp:TextBox runat="server" ID="txtFollowupVisitDate" CssClass="datepicker" Width="120px" /></td>
                                </tr>
                                <tr>
                                    <td class="tdLabel" style="vertical-align:top">
                                        <label class="lblNormal" runat="server" id="Label1">
                                            <%:GetLabel("Catatan Kunjungan Berikutnya")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFollowupVisitRemarks" runat="server" Width="100%" TextMode="Multiline"
                                            Rows="3" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
        </fieldset>
        <div style="display: none">
            <dxcp:ASPxCallbackPanel ID="cbpAllergy" runat="server" Width="100%" ClientInstanceName="cbpAllergy"
                ShowLoadingPanel="false" OnCallback="cbpAllergy_Callback">
                <ClientSideEvents EndCallback="function(s,e){ onCbpAllergyEndCallback(s); }" />
            </dxcp:ASPxCallbackPanel>
        </div>
        <div style="display: none">
            <dxcp:ASPxCallbackPanel ID="cbpDeleteVitalSign" runat="server" Width="100%" ClientInstanceName="cbpDeleteVitalSign"
                ShowLoadingPanel="false" OnCallback="cbpDeleteVitalSign_Callback">
                <ClientSideEvents EndCallback="function(s,e){ onCbpVitalSignDeleteEndCallback(s); }" />
            </dxcp:ASPxCallbackPanel>
        </div>
        <div style="display: none">
            <dxcp:ASPxCallbackPanel ID="cbpDeleteROS" runat="server" Width="100%" ClientInstanceName="cbpDeleteROS"
                ShowLoadingPanel="false" OnCallback="cbpDeleteROS_Callback">
                <ClientSideEvents EndCallback="function(s,e){ onCbpROSDeleteEndCallback(s); }" />
            </dxcp:ASPxCallbackPanel>
        </div>
        <div style="display: none">
            <dxcp:ASPxCallbackPanel ID="cbpDeleteBodyDiagram" runat="server" Width="100%" ClientInstanceName="cbpDeleteBodyDiagram"
                ShowLoadingPanel="false" OnCallback="cbpDeleteBodyDiagram_Callback">
                <ClientSideEvents EndCallback="function(s,e){ onCbpBodyDiagramDeleteEndCallback(s); }" />
            </dxcp:ASPxCallbackPanel>
        </div>
        <div style="display: none">
            <dxcp:ASPxCallbackPanel ID="cbpDiagnosis" runat="server" Width="100%" ClientInstanceName="cbpDiagnosis"
                ShowLoadingPanel="false" OnCallback="cbpDiagnosis_Callback">
                <ClientSideEvents EndCallback="function(s,e){ onCbpDiagnosisEndCallback(s); }" />
            </dxcp:ASPxCallbackPanel>
        </div>
        <div style="display: none">
            <dxcp:ASPxCallbackPanel ID="cbpDeleteTestOrder" runat="server" Width="100%" ClientInstanceName="cbpDeleteTestOrder"
                ShowLoadingPanel="false" OnCallback="cbpDeleteTestOrder_Callback">
                <ClientSideEvents EndCallback="function(s,e){ onCbpDeleteTestOrderEndCallback(s); }" />
            </dxcp:ASPxCallbackPanel>f
        </div>
        <dxcp:ASPxCallbackPanel ID="cbpSendOrder" runat="server" Width="100%" ClientInstanceName="cbpSendOrder"
            ShowLoadingPanel="false" OnCallback="cbpSendOrder_Callback">
            <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpSendOrderEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
        <dxcp:ASPxCallbackPanel ID="cbpSOAPCopy" runat="server" Width="100%" ClientInstanceName="cbpSOAPCopy"
            ShowLoadingPanel="false" OnCallback="cbpSOAPCopy_Callback">
            <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpSOAPCopyEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
    </div>

    <div id="divContainerShortcutPreviousVisit">
        <div>
            <div id="divCloseShortcutPreviousVisit">X</div>
            <div id="divOpenShortcutPreviousVisit"><b>Kunjungan Sebelumnya : <span style="color:Blue;"><asp:Label ID="lblVisitHistoryInfo2" runat="server" Text="dd-MMM-yyyy, Physician Name" /></span></b></div>
            <div id="divVisitDetailInfo" style="margin-top:5px"><b>Diagnosis: <span style="color:Red;"><asp:Label ID="lblPreviousDiagnosis" runat="server" Text="" /></span></b></div>
            <div style="padding:2px;width:100%;height:100%">
                <div class="accordion" id="listInstruction" style="display:none;text-align:left;">
                    <a style="font-size:12px;"><%= GetLabel("SUBJECTIVE")%></a> 
                    <div class="containerPreviousVisitUl">
                        <div class="divPreviousVisitContentTitle"><%= GetLabel("ANAMNESA/KELUHAN UTAMA:")%></div>
                        <asp:Repeater ID="rptChiefComplaint" runat="server">
                            <HeaderTemplate>
                                <ul>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <li>
                                    <span><%#: Eval("HistoryDate")%> <%#: Eval("HistoryTime")%> <%#: Eval("ParamedicName")%></span>
                                    <div style="white-space:normal; overflow:auto">
                                        <%#: Eval("ChiefComplaintText")%>
                                    </div>
                                </li>
                            </ItemTemplate>
                            <FooterTemplate>
                                </ul>
                            </FooterTemplate>
                        </asp:Repeater>
                        <div class="divPreviousVisitContentTitle" style="margin-top:10px; margin-bottom: 5px"><%= GetLabel("HPI:")%></div>
                        <div id="divHPI" class="divPreviousVisitContent" runat="server" style="white-space:nowrap; overflow:auto"></div>
                    </div>
                    <a style="font-size:12px;"><%= GetLabel("OBJECTIVE")%></a> 
                    <div class="containerPreviousVisitUl">
                        <div class="divPreviousVisitContentTitle"><%= GetLabel("PEMERIKSAAN FISIK")%></div>
                        <asp:Repeater ID="rptReviewOfSystemHd" runat="server" OnItemDataBound="rptReviewOfSystemHd_ItemDataBound">
                            <HeaderTemplate>
                                <ul>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <li>
                                    <span><%#: Eval("ObservationDateInString")%> <%#: Eval("ObservationTime")%> <br /> <%#: Eval("ParamedicName")%></span>
                                    <asp:Repeater ID="rptReviewOfSystemDt" runat="server">
                                        <ItemTemplate>
                                               <div style="padding-left:5px; white-space:normal">
                                                <span <%# Eval("IsNormal").ToString() == "False" && Eval("IsNotExamined").ToString() == "False" ? "Style='color:red;font-style:italic'":"" %>>
                                                    <strong>
                                                        <%#: DataBinder.Eval(Container.DataItem, "ROSystem") %>
                                                        : </strong></span>&nbsp; <span <%# Eval("IsNormal").ToString() == "False" && Eval("IsNotExamined").ToString() == "False" ? "Style='color:red;font-style:italic'":"" %>>
                                                            <%#: DataBinder.Eval(Container.DataItem, "cfRemarks")%></span>
                                            </div>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <br style="clear:both"/>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </li>
                            </ItemTemplate>
                            <FooterTemplate>
                                </ul>
                            </FooterTemplate>
                        </asp:Repeater>
                        <hr />
                        <div class="divPreviousVisitContentTitle"><%= GetLabel("TANDA VITAL")%></div>
                        <asp:Repeater ID="rptVitalSignHd" runat="server" OnItemDataBound="rptVitalSignHd_ItemDataBound">
                            <HeaderTemplate>
                                <ul>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <li>
                                   <span><%#: Eval("ObservationDateInString")%> <%#: Eval("ObservationTime")%> <br /> <%#: Eval("ParamedicName")%></span>
                                   <asp:Repeater ID="rptVitalSignDt" runat="server">
                                        <ItemTemplate>
                                            <div style="padding-left:5px;">
                                                <strong><%#: DataBinder.Eval(Container.DataItem, "VitalSignLabel") %> : </strong>&nbsp; 
                                                <%#: DataBinder.Eval(Container.DataItem, "DisplayVitalSignValue") %>
                                            </div>
                                        </ItemTemplate>
                                        <FooterTemplate> 
                                            <br style="clear:both" />
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </li>
                            </ItemTemplate>
                            <FooterTemplate>
                                </ul>
                            </FooterTemplate>
                        </asp:Repeater>
                    </div>
                    <a style="font-size:12px;"><%= GetLabel("ASSESSMENT")%></a> 
                    <div class="containerPreviousVisitUl">
                        <asp:Repeater ID="rptDifferentDiagnosis" runat="server">
                            <HeaderTemplate>
                                <ul>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <li>
                                    <span><%#: Eval("DiagnosisDate")%> <%#: Eval("DiagnosisTime")%> <%#: Eval("PhysicianName")%></span>
                                    <div style="font-weight:bold"><%#:Eval("DiagnosisText")%></div>
                                    <div><%#: Eval("DiagnosisType")%></div>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                    <a style="font-size:12px;"><%= GetLabel("PEMERIKSAAN : LABORATORIUM")%></a> 
                    <div class="containerPreviousVisitUl">
                        <asp:Repeater ID="rptLabTestOrder" runat="server" OnItemDataBound="rptLabTestOrder_ItemDataBound">
                            <HeaderTemplate>
                                <ul>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <li>
                                    <span><%#: Eval("TestDate")%> <%#: Eval("TestTime")%>, <%#: Eval("ItemName") %></span>
                                    <asp:Repeater ID="rptLaboratoryDt" runat="server">
                                        <ItemTemplate>
                                            <div style="padding-left:10px;">
                                                <strong><%#: DataBinder.Eval(Container.DataItem, "FractionName") %> : </strong>&nbsp;
                                                <span <%# Eval("ResultFlag").ToString() != "N" ? "Style='color:red'":"Style='color:black'" %>>
                                                    <%#: DataBinder.Eval(Container.DataItem, "ResultValue") %>
                                                </span>
                                                <%#: DataBinder.Eval(Container.DataItem, "ResultUnit") %>&nbsp;&nbsp;
                                                <span <%# Eval("RefRange").ToString() == "" ? "Style='display:none'":"Style='color:black;font-style:italic'" %>>(<%#: DataBinder.Eval(Container.DataItem, "RefRange") %>)</span>
                                            </div>
                                        </ItemTemplate>
                                        <FooterTemplate> 
                                            <br style="clear:both" />
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </li>
                            </ItemTemplate>
                            <FooterTemplate>
                                </ul>
                            </FooterTemplate>
                        </asp:Repeater>
                        <div id="divLaboratoryNA" class="divNotAvailableContent" runat="server">This information is not available</div>
                    </div>
                    <a style="font-size:12px;"><%= GetLabel("PEMERIKSAAN : RADIOLOGI")%></a> 
                    <div class="containerPreviousVisitUl">
                        <asp:Repeater ID="rptImagingTestOrder" runat="server" OnItemDataBound="rptImagingTestOrder_ItemDataBound">
                            <HeaderTemplate>
                                <ul>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <li>
                                    <span><%#: Eval("TestDate")%> <%#: Eval("TestTime")%>, <%#: Eval("ItemName") %></span>
                                    <asp:Repeater ID="rptImagingTestOrderDt" runat="server">
                                        <ItemTemplate>
                                            <div style="white-space:nowrap; overflow:auto">
                                                <%#: DataBinder.Eval(Container.DataItem, "Resultvalue") %>
                                            </div>
                                        </ItemTemplate>
                                        <FooterTemplate> 
                                            <br style="clear:both" />
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </li>
                            </ItemTemplate>
                            <FooterTemplate>
                                </ul>
                            </FooterTemplate>
                        </asp:Repeater>
                        <div id="divImagingNA" class="divNotAvailableContent" runat="server">This information is not available</div>
                    </div>
                    <a style="font-size:12px;"><%= GetLabel("PEMERIKSAAN : PENUNJANG MEDIS LAINNYA")%></a> 
                    <div class="containerPreviousVisitUl">
                        <asp:Repeater ID="rptDiagnosticTestOrder" runat="server">
                            <HeaderTemplate>
                                <ul>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <li>
                                    <span><%#: Eval("TestDate")%> <%#: Eval("TestTime")%>, <%#: Eval("ItemName") %></span>
                                    <asp:Repeater ID="rptDiagnosticTestOrderDt" runat="server">
                                        <ItemTemplate>
                                            <div style="white-space:nowrap; overflow:auto">
                                                <%#: DataBinder.Eval(Container.DataItem, "Resultvalue") %>
                                            </div>
                                        </ItemTemplate>
                                        <FooterTemplate> 
                                            <br style="clear:both" />
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </li>
                            </ItemTemplate>
                            <FooterTemplate>
                                </ul>
                            </FooterTemplate>
                        </asp:Repeater>
                        <div id="div1" class="divNotAvailableContent" runat="server">This information is not available</div>
                    </div>
                    <a style="font-size:12px;"><%= GetLabel("PLANNING : MEDICATION")%></a> 
                    <div class="containerPreviousVisitUl">
                        <asp:Repeater ID="rptMedication" runat="server">
                            <HeaderTemplate>
                                <ul>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <li>
                                    <div>
                                        <span <%# Eval("IsRFlag").ToString() == "0" ? "Style='display:none'":"Style='color:black;font-weight:bold'" %>>R/&nbsp;&nbsp</span>
                                        <strong><%#: Eval("cfItemName")%></strong>
                                    </div>
                                    <div <%# Eval("IsCompound").ToString() == "0" ? "Style='display:none'":"Style='white-space: pre-line;color:black;font-style:italic;margin-left:20px'" %>>
                                        <%#: Eval("cfCompoundDetail")%>
                                    </div>
                                    <div style="color: Blue; width: 35px; float: left;margin-left:20px">DOSE</div>
                                    <%#: Eval("NumberOfDosage")%>
                                    <%#: Eval("DosingUnit")%>
                                    -
                                    <%#: Eval("Route")%>
                                    -
                                    <%#: Eval("cfDoseFrequency")%>
                                </li>
                            </ItemTemplate>
                            <FooterTemplate>
                                </ul>
                            </FooterTemplate>
                        </asp:Repeater>
                        <div id="divMedicationNA" class="divNotAvailableContent" runat="server">This information is not available</div>
                    </div>
                    <a style="font-size:12px;display:none"><%= GetLabel("CATATAN TERINTEGRASI")%></a> 
                    <div class="containerPreviousVisitUl">
                        <asp:Repeater ID="rptNotes" runat="server">
                            <HeaderTemplate>
                                <ul>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <li>
                                    <span><%#: Eval("cfNoteDate")%> <%#: Eval("NoteTime")%>, <%#: Eval("ParamedicName")%></span>
                                    <textarea style="padding-left:10px;border:0; width:350px; height:150px" readonly><%#: DataBinder.Eval(Container.DataItem, "NoteText") %> </textarea>
                                </li>
                            </ItemTemplate>
                            <FooterTemplate>
                                </ul>
                            </FooterTemplate>
                        </asp:Repeater>
                    </div>
                    <a style="font-size:12px;;display:none"><%= GetLabel("DISCHARGE DAN JADWAL KUNJUNGAN BERIKUTNYA")%></a> 
                    <div class="containerPreviousVisitUl">
                        <asp:Repeater ID="rptDischargeInformation" runat="server">
                            <HeaderTemplate>
                                <ul>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <li>
                                    <div><span><%#: Eval("StartDateTimeInString")%>, <%#: Eval("ParamedicName")%></span></div>
                                    <%#: Eval("VisitTypeName")%>; <%#: Eval("Notes")%> 
                                </li>
                            </ItemTemplate>
                            <FooterTemplate>
                                </ul>
                            </FooterTemplate>
                        </asp:Repeater>
                        <asp:Repeater ID="rptFollowUpVisit" runat="server">
                            <HeaderTemplate>
                                <ul>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <li>
                                    <div><span><%#: Eval("StartDateTimeInString")%>, <%#: Eval("ParamedicName")%></span></div>
                                    <%#: Eval("VisitTypeName")%>; <%#: Eval("Notes")%> 
                                </li>
                            </ItemTemplate>
                            <FooterTemplate>
                                </ul>
                            </FooterTemplate>
                        </asp:Repeater>
                    </div>
                    <div style="padding:5px">
                        <table cellpadding="0" cellspacing="10" style="margin-left:auto;margin-right:auto;text-align:left">
                            <tr>
                                <td><input type="button" value='<%= GetLabel("SOAP COPY")%>' onclick="pcSOAPCopy.Show();" style="width:100px; height:25px" /></td>
                            </tr>
                        </table>                
                    </div>
                </div>
            </div>
        </div>
    </div>

    <dx:ASPxPopupControl ID="pcSOAPCopy" runat="server" ClientInstanceName="pcSOAPCopy"
        EnableHierarchyRecreation="True" FooterText="" HeaderText="Copy SOAP From Previous Visit" HeaderStyle-HorizontalAlign="Left" Height="250px"
        Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        Width="300px" CloseAction="CloseButton" AllowDragging="true">
        <ClientSideEvents Closing="function(s,e){ onClosingCopyPopupDialog(); }"/>
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                <div style="width: 100%;">
                    <dxcp:ASPxCallbackPanel ID="cbpCopyProcess" runat="server" Width="100%" ClientInstanceName="cbpCopyProcess"
                        ShowLoadingPanelImage="true">
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent111" runat="server">
                                <asp:Panel ID="Panel1" Style="width: 100%;" runat="server" Height="200px">
                                    <table class="tblContentArea">
                                        <tr>
                                            <td>
                                                <h4 style="background-color:transparent;color:red;font-weight:bold"><%=GetLabel("Please select the option :")%></h4>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="padding:5px;vertical-align:top">
                                                <table class="tblEntryContent" style="width:100%">
                                                    <tr>
                                                        <td><asp:CheckBox runat="server" id="chkIsCopyChiefComplaint" Text= "Chief Complaint" Checked="true" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td><asp:CheckBox runat="server" id="chkIsCopyHPI" Text= "History of Present Illness" Checked="true" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td><asp:CheckBox runat="server" id="chkIsCopyVitalSign" Text= "Vital Sign" Checked="true" /></td>  
                                                    </tr>
                                                    <tr>
                                                        <td><asp:CheckBox runat="server" id="chkIsCopyROS" Text= "Review of System" Checked="true" /></td>  
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                    <div style="padding:5px;">
                                        <table cellpadding="0" cellspacing="10" style="margin-left:auto;margin-right:auto;text-align:left">
                                            <tr>
                                                <td><input type="button" value='<%= GetLabel("COPY")%>' onclick="onCopyFromPreviousVisit();" style="width:100px; height:25px" /></td>
                                                <td><input type="button" value='<%= GetLabel("CANCEL")%>' onclick="pcSOAPCopy.Hide();" style="width:100px; height:25px" /></td>
                                            </tr>
                                        </table>                
                                    </div>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
</asp:Content>
