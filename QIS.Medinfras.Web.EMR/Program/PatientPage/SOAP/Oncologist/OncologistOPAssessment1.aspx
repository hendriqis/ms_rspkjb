<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MPPatientPageList.master"
    AutoEventWireup="true" CodeBehind="OncologistOPAssessment1.aspx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.OncologistOPAssessment1" %>

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
    <li id="btnDischargePatient" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbset.png")%>' alt="" /><div>
            <%=GetLabel("Complete Session")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script id="dxis_OPAssessment3" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.ui.accordion.js")%>' type='text/javascript'></script>

    <style type="text/css">
        #divCloseShortcutPreviousVisit                { position: absolute;right: 10px;top: 5px;display: none;padding: 0px 5px;background: #CC0000;cursor: pointer;color: #FFFFFF;-webkit-border-radius: 999px;-moz-border-radius: 999px;border-radius: 999px;font-weight:bolder;font-size:12px; }
        #divOpenShortcutPreviousVisit                 { cursor:pointer; }
        #divContainerShortcutPreviousVisit            { bottom:0;right:670px;position:fixed;height:30px;overflow:hidden; }
        #divContainerShortcutPreviousVisit > div      { border:1px solid #ecf0f1;padding:5px; background-color: #bdc3c7; width:650px;height:550px;border-bottom:0px;-webkit-border-radius: 5px; -moz-border-radius: 5px; border-radius: 5px; }
        
        #divContainerShortcutMainVisit            { bottom:0;right:10px;position:fixed;height:30px;overflow:hidden; }
        #divCloseShortcutMainVisit                { position: absolute;right: 10px;top: 5px;display: none;padding: 0px 5px;background: #CC0000;cursor: pointer;color: #FFFFFF;-webkit-border-radius: 999px;-moz-border-radius: 999px;border-radius: 999px;font-weight:bolder;font-size:12px; }
        #divOpenShortcutMainVisit                 { cursor:pointer; }
        #divContainerShortcutMainVisit > div  { border:1px solid #ecf0f1;padding:5px; background-color: #00cec9; width:650px;height:550px;border-bottom:0px;-webkit-border-radius: 5px; -moz-border-radius: 5px; border-radius: 5px; }
             
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
            setRightPanelButtonEnabled();
            if ($('#<%=hdnDisableServiceDateTime.ClientID %>').val() == "1") {
                setDatePicker('<%=txtServiceDate.ClientID %>');
            }
            ////$('#<%=txtServiceDate.ClientID %>').datepicker('option', 'maxDate', '0');
            $('#<%=txtServiceDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

            setDatePicker('<%=txtFollowupVisitDate.ClientID %>');
           /// $('#<%=txtFollowupVisitDate.ClientID %>').datepicker('option', 'minDate', '0');
            $('#<%=txtFollowupVisitDate.ClientID %>').datepicker('option', 'minDate',  '0');

            setDatePicker('<%=txtDischargeDate.ClientID %>');
           ///// $('#<%=txtDischargeDate.ClientID %>').datepicker('option', 'maxDate', '0');
            $('#<%=txtDischargeDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

            $('#<%:txtChiefComplaint.ClientID %>').focus();

            $('.btnViewPDF').live('click', function () {
                var textResultValue = $(this).attr('pdfValue');
                window.open("data:application/pdf;base64, " + textResultValue, "popupWindow", "width=600, height=600,scrollbars=yes");
            });

            $('#<%=btnPatientStatusSave.ClientID %>').click(function (evt) {
                if (IsValid(evt, 'fsPatientStatus', 'mpPatientStatus')) {
                    if (validateTime($('#<%=txtServiceTime.ClientID %>').val())) {
                        $('#<%=hdnIsChanged.ClientID %>').val('0');
                        if ($('#<%=txtChiefComplaint.ClientID %>').val() != '') {
                            onCustomButtonClick('save');
                            $('#<%=hdnIsChanged.ClientID %>').val('0');
                        }
                        else {
                            showToast('Warning', 'Anamnesa Harus diisi.'); 
                        }
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
                    displayConfirmationMessageBox('DISCARD CHANGES', message, function (result) {
                        if (result) {
                            $('#<%=hdnIsChanged.ClientID %>').val('0');
                            location.reload(true);
                        }
                    });
                }
            });

            $('#btnAddTemplate.imgLink').click(function () {
                if ($('#<%=txtChiefComplaint.ClientID %>').val() != '') {
                    onBeforeOpenTrxPopup();
                    var text = $('#<%=txtChiefComplaint.ClientID %>').val();
                    openUserControlPopup("~/Program/PatientPage/_PopupEntry/TemplateText/UpdatePhysicianTextCtl1.ascx", "X058^01|" + text, "Physician Template Text", 700, 500);
                }           
            });

            $('#btnAddHPITemplate.imgLink').click(function () {
                if ($('#<%=txtHPISummary.ClientID %>').val() != '') {
                    onBeforeOpenTrxPopup();
                    var text = $('#<%=txtHPISummary.ClientID %>').val();
                    openUserControlPopup("~/Program/PatientPage/_PopupEntry/TemplateText/UpdatePhysicianTextCtl1.ascx", "X058^02|" + text, "Physician Template Text", 700, 500);
                }
            });
            
            $('#btnAddInstructionTemplate.imgLink').click(function () {
                if ($('#<%=txtInstructionText.ClientID %>').val() != '') {
                    onBeforeOpenTrxPopup();
                    var text = $('#<%=txtInstructionText.ClientID %>').val();
                    openUserControlPopup("~/Program/PatientPage/_PopupEntry/TemplateText/UpdatePhysicianTextCtl1.ascx", "X058^07|" + text, "Physician Template Text - Instruction", 700, 500);
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
            
            $('#<%=grdProcedureView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdProcedureView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnProcedureID.ClientID %>').val($(this).find('.keyField').html());
            });
            $('#<%=grdProcedureView.ClientID %> tr:eq(1)').click();

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

            $('#<%=grdCTSimulatorView.ClientID %> > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
                if ($(this).attr('class') != 'selected') {
                    $('#<%=grdCTSimulatorView.ClientID %> tr.selected').removeClass('selected');
                    $(this).addClass('selected');
                    $('#<%=hdnSimulationRequestID.ClientID %>').val($(this).find('.keyField').html());
                }
            });

            $('#<%=grdRTProgramView.ClientID %> > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
                if ($(this).attr('class') != 'selected') {
                    $('#<%=grdRTProgramView.ClientID %> tr.selected').removeClass('selected');
                    $(this).addClass('selected');
                    $('#<%=hdnProgramID.ClientID %>').val($(this).find('.keyField').html());
                }
            });

            $('.btnApplyAllergy').click(function () {
                submitAllergy();
                $('#<%=txtAllergenName.ClientID %>').focus();
            });

            $('.btnCancelAllergy').click(function () {
                ResetAllergyEntryControls();
            });

            function submitAllergy()
            {
                if (($('#<%=txtAllergenName.ClientID %>').val() != '' && $('#<%=txtReaction.ClientID %>').val() != '')) {
                    if ($('#<%=hdnAllergyProcessMode.ClientID %>').val() == "1")
                        cbpAllergy.PerformCallback('add');
                    else
                        cbpAllergy.PerformCallback('edit');
                }
                else {
                    showToast("ERROR", "You should fill allergen name and allergy reaction !");
                }
            }

            $('#btnBodyDiagramContainerPrev').click(function () {
                if ($('#<%=hdnPageCount.ClientID %>').val() != '0')
                    cbpBodyDiagramView.PerformCallback('prev');
            });
            $('#btnBodyDiagramContainerNext').click(function () {
                if ($('#<%=hdnPageCount.ClientID %>').val() != '0')
                    cbpBodyDiagramView.PerformCallback('next');
            });

            $('#imgEditBodyDiagram').live('click', function () {
                onBeforeOpenTrxPopup();
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
                submitDiagnosis();
                $('#<%=ledDiagnose.ClientID %>').focus();
            });

            $('.btnCancelDiagnosis').click(function () {
                ResetDiagnosisEntryControls();
            });

            function submitDiagnosis()
            {
                if ((cboDiagnosisType.GetValue() != '' && $('#<%=txtDiagnosisText.ClientID %>').val() != '')) {
                    if ($('#<%=hdnDiagnosisProcessMode.ClientID %>').val() == "1")
                        cbpDiagnosis.PerformCallback('add');
                    else
                        cbpDiagnosis.PerformCallback('edit');
                }
                else {
                    showToast("ERROR", "You should fill Diagnosis Type and Name field !");
                }
            }
            
            $('.btnApplyProcedure').click(function () {
                submitProcedure();
                $('#<%=ledProcedure.ClientID %>').focus();
            });

            $('.btnCancelProcedure').click(function () {
                ResetProcedureEntryControls();
            });

            function submitProcedure()
            {
                onBeforeOpenTrxPopup();
                if ($('#<%=txtProcedureText.ClientID %>').val() != '') {
                    if ($('#<%=hdnProcedureProcessMode.ClientID %>').val() == "1")
                        cbpProcedure.PerformCallback('add');
                    else
                        cbpProcedure.PerformCallback('edit');
                }
                else {
                    displayErrorMessageBox("SOAP", "Nama/Text Tindakan tidak boleh kosong !");
                }
            }

            $('#<%=txtChiefComplaint.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });

            $('#<%:txtChiefComplaint.ClientID %>').blur(function () {
                onTxtChiefComplaintChanged($(this).val());
            });

            $('#<%=txtHPISummary.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });

            $('#<%:txtHPISummary.ClientID %>').blur(function () {
                onTxtHPIChanged($(this).val());
            });

            $('#<%=txtMedicalHistory.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });

            $('#<%=txtInstructionText.ClientID %>').keypress(function (e) {
             var key = e.which;
             if(key == 13)  // the enter key code
              {
                submitInstruction();
              }
            }); 

            $('#<%=txtMedicationHistory.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });

            $('#<%=txtPlanningNotes.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });

            $('#<%=txtDischargeDate.ClientID %>').change(function () {
                onDischargeDateTimeChange();
            });
            $('#<%=txtDischargeTime.ClientID %>').change(function () {
                onDischargeDateTimeChange();
            });

            onDischargeDateTimeChange();

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

            $('#divOpenShortcutMainVisit').click(function () {
                $('#divContainerShortcutMainVisit').animate({ height: 550 }, 550, function () {
                    $('#divCloseShortcutMainVisit').show();
                });
            });

            $('#divCloseShortcutMainVisit').click(function () {
                $('#divContainerShortcutMainVisit').animate({ height: 30 }, 550, function () {
                    $('#divCloseShortcutMainVisit').hide();
                });
            });

            $('#<%=txtReaction.ClientID %>').keypress(function (e) {
             var key = e.which;
             if(key == 13)  // the enter key code
              {
                submitAllergy();
              }
            }); 

            $('#<%=txtDiagnosisText.ClientID %>').keypress(function (e) {
             var key = e.which;
             if(key == 13)  // the enter key code
              {
                submitDiagnosis();
              }
            }); 
            
            $('#<%=txtProcedureText.ClientID %>').keypress(function (e) {
             var key = e.which;
             if(key == 13)  // the enter key code
              {
                submitProcedure();
              }
            }); 

            $('#listInstruction').show();
            $('#listInstruction').accordion({ fillSpace: true });

            registerCollapseExpandHandler();
        });

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
//            if ($('#<%:hdnIsCopySOAPFromPrevious.ClientID %>').val() == "1") {
//                $('#<%=hdnIsChanged.ClientID %>').val('0');
//                location.reload();
//            }
        }

        function onCopyFromPreviousVisit() {
            var message = "Are you sure to copy from Previous Visit ?";
            displayConfirmationMessageBox("SOAP COPY",message, function (result) {
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
//////            location.reload(true);
        }
        
       
        //#region Chief Complaint
        $('#lblChiefComplaint').die('click');
        $('#lblChiefComplaint').live('click', function (evt) {
            var visitNoteID = 0;
            var param = "X058^01|" + visitNoteID + "|1|1";
            openUserControlPopup("~/libs/Controls/EMR/Lookup/SOAPTemplateLookupCtl1.ascx", param, "SOAP Template Text", 700, 500);
        });

        function onTxtChiefComplaintChanged(value) {
            if (value.length <= 6 && value.slice(-1) == ";") {
                var filterExpression = " UserID = " + <%=HttpUtility.HtmlEncode(GetUserID())%> + " AND GCTextTemplateGroup = 'X058^01' AND TemplateCode = '" + value.slice(0, -1) + "'";
                Methods.getObject('GetvPhysicianTextList', filterExpression, function (obj) {
                    if (obj != null)
                        if ($('#<%=txtChiefComplaint.ClientID %>').val() != '') {
                            var message1 = "Ganti catatan di keluhan pasien (anamnesa) dengan teks dari template ?";
                            var message2 = "<i>"+obj.TemplateText+"</i>";
                            displayConfirmationMessageBox('TEMPLATE TEXT', message1+"<br/><br/>"+message2, function (result) {
                                if (result) {
                                    $('#<%=txtChiefComplaint.ClientID %>').val(obj.TemplateText);
                                }
                            });
                        }
                });
            }
        }
        //#endregion

        //#region History of Present Illness
        $('#lblHPI').die('click');
        $('#lblHPI').live('click', function (evt) {
            alert("Sorry, this feature is currently in development process (Physician Template Text Lookup)");
        });

        function onTxtHPIChanged(value) {
            if (value.length <= 6 && value.slice(-1) == ";") {
                var filterExpression = " UserID = " + <%=HttpUtility.HtmlEncode(GetUserID())%> + " AND TemplateCode = '" + value.slice(0, -1) + "'";
                Methods.getObject('GetvPhysicianTextList', filterExpression, function (obj) {
                    if (obj != null)
                        if ($('#<%=txtHPISummary.ClientID %>').val() != '') {
                            var message = "Ganti catatan di Riwayat Penyakit sekarang dengan teks dari template ? ?";
                            showToastConfirmation(message, function (result) {
                                if (result) {
                                    $('#<%=txtHPISummary.ClientID %>').val(obj.TemplateText);
                                }
                            });
                        }
                });
            }
        }
        //#endregion

        $('#lblCCLink').die('click');
        $('#lblCCLink').live('click', function () {
            openUserControlPopup("~/Program/PatientPage/_PopupEntry/Instruction/PhysicianInstructionQuickPicks1Ctl.ascx", "", "Planning Instruction", 700, 600);
        });

        function onAfterSelectFromInstructionTemplate(param) {
            if (param != '') {
                var item = param.split("|").join("\n");
                var newText = item;
                if ($('#<%=txtChiefComplaint.ClientID %>').val() != "") {
                    newText = $('#<%=txtChiefComplaint.ClientID %>').val() + "\n" + item;
                }
                $('#<%=txtChiefComplaint.ClientID %>').val(newText);
            }
        }

        function onAfterGenerateBPSFollowupVisit(param) {
            if (param != '') {
                $('#<%=hdnAppointmentID.ClientID %>').val(param);
                var filterExpression = "AppointmentID = " + param;
                Methods.getObject('GetvAppointmentList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=txtFollowupVisitDate.ClientID %>').val(result.StartDateInDatePickerFormat);
                        cboFollowupVisitType.SetValue(result.VisitTypeID);
                        $('#<%=txtFollowupVisitRemarks.ClientID %>').val(result.Notes);
                    }
                });
            }
        }

        var pageCountVitalSign = parseInt('<%=gridVitalSignPageCount %>');
        var procedurePageCountProcedure = parseInt('<%=gridProcedurePageCount %>');
        $(function () {
            setPagingDetailItemVitalSign(pageCountVitalSign);
            setPagingDetailItemProcedure(procedurePageCountProcedure);
        });

        function setPagingDetailItemVitalSign(pageCount) {
            setPaging($("#vitalSignPaging"), pageCount, function (page) {
                cbpVitalSignView.PerformCallback('changepage|' + page);
            });
        }
        
        function setPagingDetailItemProcedure(pageCount) {
            setPaging($("#procedurePaging"), pageCount, function (page) {
                cbpProcedureView.PerformCallback('changepage|' + page);
            });
        }

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

                if (param[2]=="1") {
                    $('#<%=chkIsPatientAllergyExists.ClientID %>').attr("disabled", "disabled");
                    $('#<%=chkIsPatientAllergyExists.ClientID %>').prop( "checked", false );
                }
                else
                {
                    $('#<%=chkIsPatientAllergyExists.ClientID %>').removeAttr("disabled");
                    $('#<%=chkIsPatientAllergyExists.ClientID %>').prop( "checked", true );
                }

                if (typeof RefreshPatientBanner == 'function')
                    RefreshPatientBanner();
            }
            else if (param[0] == '0') {
                displayErrorMessageBox("Alergi Pasien", 'Error Message : ' + param[2]);
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
            $('#<%=hdnMIMSAllergenID.ClientID %>').val(selectedObj.MIMSAllergenID);
            $('#<%=txtMIMSAllergenName.ClientID %>').val(selectedObj.ItemDescription);
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
            cboAllergenType.SetValue(Constant.AllergenType.DRUG);
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
        var pageCount = parseInt('<%=gridVitalSignPageCount %>');
        $(function () {
            setPaging($("#vitalSignPaging"), pageCount, function (page) {
                cbpVitalSignView.PerformCallback('changepage|' + page);
            });
        });

        $('#lblAddVitalSign').die('click');
        $('#lblAddVitalSign').live('click', function (evt) {
            onBeforeOpenTrxPopup();                
            var param = "0||0|1|3";
            openUserControlPopup("~/Program/PatientPage/_PopupEntry/VitalSignEntry1Ctl.ascx", param, "Vital Sign & Indicator", 700, 500);
        });


        $('#lblAddFromVitalSignLookup').die('click');
        $('#lblAddFromVitalSignLookup').live('click', function (evt) {
            onBeforeOpenTrxPopup();
            var chiefComplaintID = $('#<%=hdnChiefComplaintID.ClientID %>').val();
            var visitNoteID = $('#<%=hdnPatientVisitNoteID.ClientID %>').val();
            var param = "0|" + visitNoteID + "|1|1||" + chiefComplaintID;
            openUserControlPopup("~/libs/Controls/EMR/Lookup/VitalSignLookupCtl1.ascx", param, "Tanda Vital & Indikator Lainnya", 700, 500);
        });

        $('.imgEditVitalSign.imgLink').die('click');
        $('.imgEditVitalSign.imgLink').live('click', function () {
            onBeforeOpenTrxPopup();
            $row = $(this).closest('tr').parent().closest('tr');
            var obj = rowToObject($row);
            $('#<%=hdnVitalSignRecordID.ClientID %>').val(obj.ID);
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
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            }
            else {
                showToast("ERROR", 'Error Message : ' + param[1]);
            }
        }

        function onRefreshVitalSignGrid() {
            cbpVitalSignView.PerformCallback('refresh');
            $('#<%=hdnIsChanged.ClientID %>').val('1');
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
            onBeforeOpenTrxPopup();
            var param = "0|0|1|3";
            openUserControlPopup("~/Program/PatientPage/_PopupEntry/ROSEntry1Ctl.ascx", param, "Review of System", 700, 500);
        });

        $('.imgEditROS.imgLink').die('click');
        $('.imgEditROS.imgLink').live('click', function () {
            onBeforeOpenTrxPopup();
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
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            }
            else {
                showToast("ERROR", param[1]);
            }
        }

        function onRefreshROSGrid() {
            cbpROSView.PerformCallback('refresh');
            $('#<%=hdnIsChanged.ClientID %>').val('1');
        }
        //#endregion

        //#region Body Diagram
        $('#lblAddBodyDiagram').die('click');
        $('#lblAddBodyDiagram').live('click', function (evt) {
            onBeforeOpenTrxPopup();
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
        //#endregion

        //#region Diagnosis
        $('#lblAddFromDxHistory').die('click');
        $('#lblAddFromDxHistory').live('click', function (evt) {
            if ($('#<%=hdnIsChanged.ClientID %>').val()=="0") {  
                var visitID = $('#<%:hdnPageVisitID.ClientID %>').val();  
                var mrn = $('#<%:hdnPageMRN.ClientID %>').val(); 
                var visitDate = $('#<%=txtServiceDate.ClientID %>').val();
                var visitTime = $('#<%=txtServiceTime.ClientID %>').val(); 
                var param = visitID + "|" + mrn + "|" + visitDate + "|" + visitTime;
                openUserControlPopup("~/libs/Controls/EMR/Lookup/PatientDiagnosisHistoryCtl1.ascx", param, "Riwayat Diagnosa Pasien", 700, 500);
            }
            else
            {
                displayMessageBox("SOAP","Mohon simpan data terlebih dahulu, sebelum melakukan proses ini.");
            }
        });

        $('#lblAddFromDxHistory2').die('click');
        $('#lblAddFromDxHistory2').live('click', function (evt) {
            if ($('#<%=hdnIsChanged.ClientID %>').val()=="0") {  
                var visitID = $('#<%:hdnPageVisitID.ClientID %>').val();  
                var mrn = $('#<%:hdnPageMRN.ClientID %>').val(); 
                var visitDate = $('#<%=txtServiceDate.ClientID %>').val();
                var visitTime = $('#<%=txtServiceTime.ClientID %>').val(); 
                var param = visitID + "|" + mrn + "|" + visitDate + "|" + visitTime;
                openUserControlPopup("~/libs/Controls/EMR/Lookup/PatientDiagnosisHistoryCtl1.ascx", param, "Riwayat Diagnosa Pasien", 700, 500);
            }
            else
            {
                displayMessageBox("SOAP","Mohon simpan data terlebih dahulu, sebelum melakukan proses ini.");
            }
        });

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
            $('#<%=hdnEntryDiagnoseID.ClientID %>').val('');
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
            var diagnosis = s.cpDiagnosis;
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
            $('#<%=hdnDiagnosisSummary.ClientID %>').val(diagnosis);
        }

        function onCbpDiagnosisEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == '1') {
                if (param[1] == "edit")
                    $('#<%=hdnDiagnosisProcessMode.ClientID %>').val('1');

                ResetDiagnosisEntryControls();
                cbpDiagnosisView.PerformCallback('refresh');
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            }
            else if (param[0] == '0') {
                displayErrorMessageBox("SOAP", 'Error Message : ' + param[2]);
            }
            else
                $('#<%=grdDiagnosisView.ClientID %> tr:eq(1)').click();
        }

        function onRefreshDiagnosisGrid() {          
            cbpDiagnosisView.PerformCallback('refresh');
            $('#<%=hdnIsChanged.ClientID %>').val('1');
        }
        //#endregion
        
        //#region Procedure
        function onLedProcedureLostFocus(led) {
            var procedureID = led.GetValueText();
            $('#<%=hdnEntryProcedureID.ClientID %>').val(procedureID);
            $('#<%=hdnEntryProcedureText.ClientID %>').val(led.GetDisplayText());
            $('#<%=txtProcedureText.ClientID %>').val($('#<%=hdnEntryProcedureText.ClientID %>').val());
        }

        function GetCurrentSelectedProcedure(s) {
            var $tr = $(s).closest('tr').parent().closest('tr');
            var idx = $('#<%=grdProcedureView.ClientID %> tr').index($tr);
            $('#<%=grdProcedureView.ClientID %> tr:eq(' + idx + ')').click();

            $row = $('#<%=grdProcedureView.ClientID %> tr.selected');
            var selectedObj = {};

            $row.find('input[type=hidden]').each(function () {
                selectedObj[$(this).attr('bindingfield')] = $(this).val();
            });

            return selectedObj;
        }

        function SetProcedureEntityToControl(param) {
            var selectedObj = {};
            selectedObj = GetCurrentSelectedProcedure(param);

            ledProcedure.SetValue(selectedObj.ProcedureID);
            $('#<%=txtProcedureText.ClientID %>').val(selectedObj.ProcedureText);

        }

        function ResetProcedureEntryControls(s) {
            ledProcedure.SetValue('');
            $('#<%=txtProcedureText.ClientID %>').val('');
      
        }

        $('.imgEditProcedure.imgLink').die('click');
        $('.imgEditProcedure.imgLink').live('click', function () {
            SetProcedureEntityToControl(this);
            $('#<%=hdnProcedureProcessMode.ClientID %>').val('0');
        });

        $('.imgDeleteProcedure.imgLink').die('click');
        $('.imgDeleteProcedure.imgLink').live('click', function () {
            var selectedObj = {};
            selectedObj = GetCurrentSelectedProcedure(this);

            var message = "Are you sure to delete this procedure <b>'" + selectedObj.ProcedureText + "'</b> for this patient ?";
            displayConfirmationMessageBox("SOAP",message, function (result) {
                if (result) {
                    onBeforeOpenTrxPopup();
                    cbpProcedure.PerformCallback('delete');
                    $('#<%=hdnIsChanged.ClientID %>').val('1');
                }
            });
        });

        var pageCount = parseInt('<%=gridProcedurePageCount %>');
        $(function () {
            setPaging($("#procedurePaging"), pageCount, function (page) {
                cbpProcedureView.PerformCallback('changepage|' + page);
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });
        });

        function onCbpProcedureViewEndCallback(s) {
            var param = s.cpResult.split('|');
            var summaryText = s.cpSummary;

            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdProcedureView.ClientID %> tr:eq(1)').click();

                $('#<%=hdnProcedureSummary.ClientID %>').val(summaryText);

                setPaging($("#procedurePaging"), pageCount, function (page) {
                    cbpProcedureView.PerformCallback('changepage|' + page);
                    $('#<%=hdnIsChanged.ClientID %>').val('1');
                });
            }
            else
                $('#<%=grdProcedureView.ClientID %> tr:eq(1)').click();

            $('#<%=hdnProcedureSummary.ClientID %>').val(summaryText);
        }

        function onCbpProcedureEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == '1') {
                if (param[1] == "edit")
                    $('#<%=hdnProcedureProcessMode.ClientID %>').val('1');

                ResetProcedureEntryControls();
                cbpProcedureView.PerformCallback('refresh');
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            }
            else if (param[0] == '0') {
                displayErrorMessageBox("SOAP", 'Error Message : ' + param[2]);
            }
            else
                $('#<%=grdProcedureView.ClientID %> tr:eq(1)').click();
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
            var chiefComplaint = $('#<%=txtChiefComplaint.ClientID %>').val();
            var testOrderID = "0";

            var param = "X001^004" + "|" + testOrderID + "|" + clinicalNotes + "|" + chiefComplaint + "|" + "X125^001" + "|" + 'Langsung Dikerjakan' + "|" + performDate + "|" + performTime + "|" + "" + "|" + labServiceUnitID;
            var title = "Laboratory Test Order - " + $('#<%=hdnPatientInformation.ClientID %>').val();
            openUserControlPopup("~/Program/PatientPage/_PopupEntry/CPOE/TestOrderItemQuickPicksCtl1.ascx", param, title, 1200, 600);
        });


        $('#lblAddLabOrder2').die('click');
        $('#lblAddLabOrder2').live('click', function (evt) {
            onBeforeOpenTrxPopup();
            var width = 1150;
            var clinicalNotes = $('#<%=hdnDiagnosisSummary.ClientID %>').val();
            var chiefComplaint = $('#<%=txtChiefComplaint.ClientID %>').val();
            var laboratoryServiceUnitID = $('#<%=hdnLaboratoryServiceUnitID.ClientID %>').val();
            var param = "X001^004|0|" + clinicalNotes + "|" + chiefComplaint + "|" + laboratoryServiceUnitID;
            var title = "Laboratory Test Order - " + $('#<%=hdnPatientInformation.ClientID %>').val();
            openUserControlPopup('~/Program/PatientPage/_PopupEntry/CPOE/TestOrderLabQuickPicksCtl1.ascx', param, title, width, 600);
        });

        $('.imgAddLabOrderDt.imgLink').die('click');
        $('.imgAddLabOrderDt.imgLink').live('click', function () {
            onBeforeOpenTrxPopup();
            var selectedObj = {};
            selectedObj = GetCurrentSelectedLaboratory(this);
            $('#<%=hdnLaboratoryTestOrderID.ClientID %>').val(selectedObj.TestOrderID);
            var clinicalNotes = $('#<%=hdnDiagnosisSummary.ClientID %>').val();
            var chiefComplaint = $('#<%=txtChiefComplaint.ClientID %>').val();
            var param = "X001^004|" + $('#<%=hdnLaboratoryTestOrderID.ClientID %>').val() + "|" + clinicalNotes + "|" + chiefComplaint;
            var title = "Laboratory Test Order - " + $('#<%=hdnPatientInformation.ClientID %>').val();
            openUserControlPopup("~/Program/PatientPage/_PopupEntry/CPOE/TestOrderLabQuickPicksCtl1.ascx", param, title, 1200, 600);
        });

        $('.imgEditLabOrder.imgLink').die('click');
        $('.imgEditLabOrder.imgLink').live('click', function () {
            onBeforeOpenTrxPopup();
            var selectedObj = {};
            selectedObj = GetCurrentSelectedLaboratory(this);
            $('#<%=hdnTestOrderHealthcareServiceUnitID.ClientID %>').val(selectedObj.HealthcareServiceUnitID);
            $('#<%=hdnLaboratoryTestOrderID.ClientID %>').val(selectedObj.TestOrderID);

            var param = "LB|" + $('#<%=hdnLaboratoryTestOrderID.ClientID %>').val() + "|" + $('#<%=hdnTestOrderHealthcareServiceUnitID.ClientID %>').val();
            var title = "Laboratory Test Order - " + $('#<%=hdnPatientInformation.ClientID %>').val();
            openUserControlPopup("~/Program/PatientPage/_PopupEntry/CPOE/TestOrderItemEditCtl1.ascx", param, title, 700, 500);
        });

        $('.imgDeleteLabOrder.imgLink').die('click');
        $('.imgDeleteLabOrder.imgLink').live('click', function () {
            var message = "Are you sure to delete this Laboratory Test Order record ?";
            showToastConfirmation(message, function (result) {
                if (result) {
                    onBeforeOpenTrxPopup();
                    cbpDeleteTestOrder.PerformCallback('LB');
                    $('#<%=hdnIsChanged.ClientID %>').val('1');
                }
            });
        });

        $('.imgSendLabOrder.imgLink').die('click');
        $('.imgSendLabOrder.imgLink').live('click', function () {
            var selectedObj = {};
            selectedObj = GetCurrentSelectedLaboratory(this);
            $('#<%=hdnLaboratoryTestOrderID.ClientID %>').val(selectedObj.TestOrderID);

            if ($('#<%:hdnLaboratoryTestOrderID.ClientID %>').val() == "") {
                displayErrorMessageBox("ORDER PEMERIKSAAN LABORATORIUM", "Tidak ada order permintaan pemeriksaan yang dapat dikirim !");
            }
            else {

                var message = "Kirim order permintaan pemeriksaan ke unit ?";
                displayConfirmationMessageBox('ORDER PEMERIKSAAN LABORATORIUM',message, function (result) {
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
            var summaryText = s.cpSummary;
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdLaboratoryView.ClientID %> tr:eq(1)').click();
                    
               
                    $('#<%=hdnLaboratoryOrderSummaryText.ClientID %>').val(param[2]);
               

                setPaging($("#laboratoryPaging"), pageCount, function (page) {
                    cbpLaboratoryView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdLaboratoryView.ClientID %> tr:eq(1)').click();

            $('#<%=hdnLaboratoryOrderSummaryText.ClientID %>').val(summaryText);     
        }

        function onRefreshLaboratoryGrid() {          
            cbpLaboratoryView.PerformCallback('refresh');
            $('#<%=hdnIsChanged.ClientID %>').val('1');
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
            var chiefComplaint = $('#<%=txtChiefComplaint.ClientID %>').val();
            var testOrderID = "0";
            var param = "X001^005" + "|" + testOrderID + "|" + clinicalNotes + "|" + chiefComplaint + "|" + "X125^001" + "|" + 'Langsung Dikerjakan' + "|" + performDate + "|" + performTime;
            var title = "Pemeriksaan Radiologi - " + $('#<%=hdnPatientInformation.ClientID %>').val();
            openUserControlPopup("~/Program/PatientPage/_PopupEntry/CPOE/TestOrderItemQuickPicksCtl1.ascx", param, title, 1200, 600);
        });

        $('#lblAddImagingOrder2').die('click');
        $('#lblAddImagingOrder2').live('click', function (evt) {
            onBeforeOpenTrxPopup();
            var width = 400;
            var clinicalNotes = $('#<%=hdnDiagnosisSummary.ClientID %>').val();
            var chiefComplaint = $('#<%=txtChiefComplaint.ClientID %>').val();
            var healthcareServiceUnitID = $('#<%=hdnImagingServiceUnitID.ClientID %>').val();
            var param = "X001^005|0|" + healthcareServiceUnitID + "|" + clinicalNotes + "|" + chiefComplaint;
            var title = "Pemeriksaan Radiologi - " + $('#<%=hdnPatientInformation.ClientID %>').val();
            openUserControlPopup('~/Program/PatientPage/_PopupEntry/CPOE/TestOrderHeaderCtl1.ascx', param, title, width, 500);
        });

        $('#lblAddImagingOrder3').die('click');
        $('#lblAddImagingOrder3').live('click', function (evt) {
            onBeforeOpenTrxPopup();
            var width = 1150;
            var clinicalNotes = $('#<%=hdnDiagnosisSummary.ClientID %>').val();
            var chiefComplaint = $('#<%=txtChiefComplaint.ClientID %>').val();
            var imagingServiceUnitID = $('#<%=hdnImagingServiceUnitID.ClientID %>').val();
            var param = "X001^005|0|" + clinicalNotes + "|" + chiefComplaint + "|" + imagingServiceUnitID;
            var title = "Pemeriksaan Radiologi - " + $('#<%=hdnPatientInformation.ClientID %>').val();
            openUserControlPopup('~/Program/PatientPage/_PopupEntry/CPOE/TestOrderLabQuickPicksCtl1.ascx', param, title, width, 600);
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
            var chiefComplaint = $('#<%=txtChiefComplaint.ClientID %>').val();
            var testOrderID = "0";
            var param = "X001^005" + "|" + testOrderID + "|" + clinicalNotes + "|" + chiefComplaint + "|" + "X125^001" + "|" + 'Langsung Dikerjakan' + "|" + performDate + "|" + performTime;
            var title = "Pemeriksaan Radiologi - " + $('#<%=hdnPatientInformation.ClientID %>').val();
            openUserControlPopup("~/Program/PatientPage/_PopupEntry/CPOE/TestOrderItemQuickPicksCtl1.ascx", param, title, 1200, 600);
        });

        $('.imgEditImagingOrder.imgLink').die('click');
        $('.imgEditImagingOrder.imgLink').live('click', function () {
            onBeforeOpenTrxPopup();
            var selectedObj = {};
            selectedObj = GetCurrentSelectedImaging(this);
            $('#<%=hdnTestOrderHealthcareServiceUnitID.ClientID %>').val(selectedObj.HealthcareServiceUnitID);
            $('#<%=hdnImagingTestOrderID.ClientID %>').val(selectedObj.TestOrderID);

            var noOfDetails = selectedObj.NoOfDetails;

            if (noOfDetails <= 0) {
                var width = 400;
                var clinicalNotes = $('#<%=hdnDiagnosisSummary.ClientID %>').val();
                var chiefComplaint = $('#<%=txtChiefComplaint.ClientID %>').val();
                var healthcareServiceUnitID = $('#<%=hdnImagingServiceUnitID.ClientID %>').val();
                var param = "X001^005|"+$('#<%=hdnImagingTestOrderID.ClientID %>').val()+"|" + healthcareServiceUnitID + "|" + clinicalNotes + "|" + chiefComplaint;
                var title = "Pemeriksaan Radiologi - " + $('#<%=hdnPatientInformation.ClientID %>').val();
                openUserControlPopup('~/Program/PatientPage/_PopupEntry/CPOE/TestOrderHeaderCtl1.ascx', param, title, width, 500);    
            }
            else
            {
                var param = "IS|" + $('#<%=hdnImagingTestOrderID.ClientID %>').val() + "|" + $('#<%=hdnTestOrderHealthcareServiceUnitID.ClientID %>').val();
                var title = "Pemeriksaan Radiologi - " + $('#<%=hdnPatientInformation.ClientID %>').val();
                openUserControlPopup("~/Program/PatientPage/_PopupEntry/CPOE/TestOrderItemEditCtl1.ascx", param, title, 700, 500);
            }
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
                displayErrorMessageBox("ORDER PEMERIKSAAN RADIOLOGI", "Tidak ada order permintaan pemeriksaan yang dapat dikirim !");
            }
            else {
                var message = "Kirim order permintaan pemeriksaan ke unit ?";
                displayConfirmationMessageBox('ORDER PEMERIKSAAN RADIOLOGI',message, function (result) {
                    if (result) cbpSendOrder.PerformCallback('sendOrder|IS|' + $('#<%:hdnImagingTestOrderID.ClientID %>').val());
                });
            }
        });
        //#endregion

        //#region lblDiagnosticResultSummary
            $('#lblDiagnosticResultSummary').die('click');
            $('#lblDiagnosticResultSummary').live('click',  function() {          
              var param  ="";
               openUserControlPopup("~/Program/PatientPage/_PopupEntry/DiagnosticResultSummaryHistoryCtl.ascx",param, "History Catatan Hasil Pemeriksaan Penunjang", 1200, 600);
            });
            

        var pageCount = parseInt('<%=gridImagingPageCount %>');
        $(function () {
            setPaging($("#imagingPaging"), pageCount, function (page) {
                cbpImagingView.PerformCallback('changepage|' + page);
            });
        });

        function onCbpImagingViewEndCallback(s) {
            var param = s.cpResult.split('|');
            var summaryText = s.cpSummary;
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdImagingView.ClientID %> tr:eq(1)').click();
                                   
                $('#<%=hdnImagingSummaryText.ClientID %>').val(param[2]);

                setPaging($("#imagingPaging"), pageCount, function (page) {
                    cbpImagingView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdImagingView.ClientID %> tr:eq(1)').click();

            $('#<%=hdnImagingSummaryText.ClientID %>').val(summaryText);   
        }

        function onRefreshImagingGrid() {
            cbpImagingView.PerformCallback('refresh');
            $('#<%=hdnIsChanged.ClientID %>').val('1');
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
            var chiefComplaint = $('#<%=txtChiefComplaint.ClientID %>').val();
            var testOrderID = "0";
            var param = "X001^006" + "|" + serviceUnitID + "|" + testOrderID + "|" + clinicalNotes + "|" + chiefComplaint + "|" + "X125^001" + "|" + 'Langsung Dikerjakan' + "|" + performDate + "|" + performTime;
            var title = "Diagnostic Test Order - " + $('#<%=hdnPatientInformation.ClientID %>').val();
            openUserControlPopup("~/Program/PatientPage/_PopupEntry/CPOE/OtherTestOrderItemQuickPicksCtl1.ascx", param, title, 1200, 600);
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
            var chiefComplaint = $('#<%=txtChiefComplaint.ClientID %>').val();
            var testOrderID = selectedObj.TestOrderID;

            var param = "X001^006" + "|" + serviceUnitID + "|" + testOrderID + "|" + clinicalNotes + "|" + chiefComplaint + "|" + "X125^001" + "|" + 'Langsung Dikerjakan' + "|" + performDate + "|" + performTime;
            var title = "Diagnostic Test Order - " + $('#<%=hdnPatientInformation.ClientID %>').val();
            openUserControlPopup("~/Program/PatientPage/_PopupEntry/CPOE/OtherTestOrderItemQuickPicksCtl1.ascx", param, title, 1200, 600);
        });

        $('.imgEditDiagnosticOrder.imgLink').die('click');
        $('.imgEditDiagnosticOrder.imgLink').live('click', function () {
            onBeforeOpenTrxPopup();
            var selectedObj = {};
            selectedObj = GetCurrentSelectedDiagnostic(this);
            $('#<%=hdnTestOrderHealthcareServiceUnitID.ClientID %>').val(selectedObj.HealthcareServiceUnitID);
            $('#<%=hdnDiagnosticTestOrderID.ClientID %>').val(selectedObj.TestOrderID);

            var param = "MD|" + $('#<%=hdnDiagnosticTestOrderID.ClientID %>').val() + "|" + $('#<%=hdnTestOrderHealthcareServiceUnitID.ClientID %>').val();
            var title = "Diagnostic Test Order - " + $('#<%=hdnPatientInformation.ClientID %>').val();
            openUserControlPopup("~/Program/PatientPage/_PopupEntry/CPOE/TestOrderItemEditCtl1.ascx", param, title, 700, 500);
        });

        $('.imgDeleteDiagnosticOrder.imgLink').die('click');
        $('.imgDeleteDiagnosticOrder.imgLink').live('click', function () {
            var message = "Are you sure to delete this Diagnostic Test Order record ?";
            showToastConfirmation(message, function (result) {
                if (result) {
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
            var summaryText = s.cpSummary;
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdDiagnosticView.ClientID %> tr:eq(1)').click();
                                   
                $('#<%=hdnOtherTestSummaryText.ClientID %>').val(param[2]);

                setPaging($("#diagnosticPaging"), pageCount, function (page) {
                    cbpDiagnosticView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdDiagnosticView.ClientID %> tr:eq(1)').click();

            $('#<%=hdnOtherTestSummaryText.ClientID %>').val(summaryText);
        }

        function onRefreshDiagnosticGrid() {
            cbpDiagnosticView.PerformCallback('refresh');
            $('#<%=hdnIsChanged.ClientID %>').val('1');
        }
        //#endregion


        //#region CT Simulator View
        $('.imgAddCTSimulatorRequest.imgLink').die('click');
        $('.imgAddCTSimulatorRequest.imgLink').live('click', function (evt) {
            var allow = true;
            if (allow) {
                var title = "Permintaan CT Simulator : " + $('#<%=hdnPagePatientName.ClientID %>').val();
                var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/Radiotherapy/CTSimulationRequestEntryCtl.ascx");
                var param = "0" + "|" + $('#<%=hdnPageVisitID.ClientID %>').val() + "|" + $('#<%=hdnPageMRN.ClientID %>').val() + "|" + $('#<%=hdnPageMedicalNo.ClientID %>').val() + "|" + $('#<%=hdnPagePatientName.ClientID %>').val() + "|" + $('#<%=hdnParamedicID.ClientID %>').val();
                openUserControlPopup(url, param, title, 800, 500, "");
            }
        });

        $('.imgEditCTSimulatorRequest.imgLink').die('click');
        $('.imgEditCTSimulatorRequest.imgLink').live('click', function (evt) {
            var allow = true;
            if (allow) {
                var title = "Permintaan CT Simulator : " + $('#<%=hdnPagePatientName.ClientID %>').val();
                var recordID = $(this).attr('recordID');
                var paramedicID = $(this).attr('paramedicID');
                if (onBeforeEditDeleteRTProgram(paramedicID)) {
                    var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/Radiotherapy/CTSimulationRequestEntryCtl.ascx");
                    var param = recordID + "|" + $('#<%=hdnPageVisitID.ClientID %>').val() + "|" + $('#<%=hdnPageMRN.ClientID %>').val() + "|" + $('#<%=hdnPageMedicalNo.ClientID %>').val() + "|" + $('#<%=hdnPagePatientName.ClientID %>').val() + "|" + $('#<%=hdnParamedicID.ClientID %>').val();
                    openUserControlPopup(url, param, title, 800, 500, "");
                }
            }
        });

        $('.imgDeleteCTSimulatorRequest.imgLink').die('click');
        $('.imgDeleteCTSimulatorRequest.imgLink').live('click', function () {
            var recordID = $(this).attr('recordID');
            var paramedicID = $(this).attr('paramedicID');
            if (onBeforeEditDeleteCTSimulation(paramedicID)) {
                var message = "Hapus permintaan CT Simulator untuk pasien ini ?";
                displayConfirmationMessageBox("Permintaan CT Simulator", message, function (result) {
                    if (result) {
                        var param = "delete"+"|"+recordID;
                        cbpCTSimulation.PerformCallback(param);
                    }
                });
            }
        });

        $('.imgSendCTSimulatorRequest.imgLink').die('click');
        $('.imgSendCTSimulatorRequest.imgLink').live('click', function () {
            var recordID = $(this).attr('recordID');
            var paramedicID = $(this).attr('paramedicID');
            if (onBeforeEditDeleteCTSimulation(paramedicID)) {
                var message = "Kirim permintaan CT Simulator untuk pasien ini ?";
                displayConfirmationMessageBox("Permintaan CT Simulator", message, function (result) {
                    if (result) {
                        var param = "send"+"|"+recordID;
                        cbpCTSimulation.PerformCallback(param);
                    }
                });
            }
        });

        function onCbpCTSimulatorViewEndCallback(s) {
            var param = s.cpResult.split('|');
            var summaryText = s.cpSummary;
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdCTSimulatorView.ClientID %> tr:eq(1)').click();
                                   
                setPaging($("#simulatorPaging"), pageCount, function (page) {
                    cbpCTSimulatorView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdCTSimulatorView.ClientID %> tr:eq(1)').click();

            $('#<%=hdnCTSimulatorSummaryText.ClientID %>').val(summaryText);   
        }

        function onCbpCTSimulationEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == '1') {
                    cbpCTSimulatorView.PerformCallback('refresh');
            }
            else {
                displayErrorMessageBox('Program Radiasi', param[1]);
            }

        }

        function onBeforeEditDeleteCTSimulation(paramedicID) {
            if ($('#<%=hdnParamedicID.ClientID %>').val() == paramedicID) {
                return true;
            }
            else {
                displayErrorMessageBox('EDIT/DELETE', 'Maaf, data hanya bisa diubah/dihapus oleh user yang melakukan pengentrian data tersebut.');
                return false;
            }
        }
        function onRefreshCTSimulatorViewGrid() {
            cbpCTSimulatorView.PerformCallback('refresh');
            $('#<%=hdnIsChanged.ClientID %>').val('1');
        }
        //#endregion

        //#region Radiotherapy Program View
        $('.imgAddRTProgram.imgLink').die('click');
        $('.imgAddRTProgram.imgLink').live('click', function (evt) {
            var allow = true;
            if (allow) {
                var title = "Program Radiasi : " + $('#<%=hdnPagePatientName.ClientID %>').val();
                var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/Radiotherapy/RTProgramEntryCtl.ascx");
                var param = "0" + "|" + $('#<%=hdnPageVisitID.ClientID %>').val() + "|" + $('#<%=hdnPageMRN.ClientID %>').val() + "|" + $('#<%=hdnPageMedicalNo.ClientID %>').val() + "|" + $('#<%=hdnPagePatientName.ClientID %>').val() + "|" + $('#<%=hdnParamedicID.ClientID %>').val();
                openUserControlPopup(url, param, title, 800, 500, "");
            }
        });

        $('.imgEditRTProgram.imgLink').die('click');
        $('.imgEditRTProgram.imgLink').live('click', function (evt) {
            var allow = true;
            if (allow) {
                var title = "Program Radiasi : " + $('#<%=hdnPagePatientName.ClientID %>').val();
                var recordID = $(this).attr('recordID');
                var paramedicID = $(this).attr('paramedicID');
                if (onBeforeEditDeleteRTProgram(paramedicID)) {
                    var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/Radiotherapy/RTProgramEntryCtl.ascx");
                    var param = recordID + "|" + $('#<%=hdnPageVisitID.ClientID %>').val() + "|" + $('#<%=hdnPageMRN.ClientID %>').val() + "|" + $('#<%=hdnPageMedicalNo.ClientID %>').val() + "|" + $('#<%=hdnPagePatientName.ClientID %>').val() + "|" + paramedicID;
                    openUserControlPopup(url, param, title, 800, 500, "");
                }
            }
        });

        $('.imgDeleteRTProgram.imgLink').die('click');
        $('.imgDeleteRTProgram.imgLink').live('click', function () {
            var recordID = $(this).attr('recordID');
            var paramedicID = $(this).attr('paramedicID');
            if (onBeforeEditDeleteRTProgram(paramedicID)) {
                var message = "Hapus Program Radiasi untuk pasien ini ?";
                displayConfirmationMessageBox("Program Radiasi", message, function (result) {
                    if (result) {
                        var param = "delete"+"|"+recordID;
                        cbpRTProgram.PerformCallback(param);
                    }
                });
            }
        });

        $('.imgSendRTProgram.imgLink').die('click');
        $('.imgSendRTProgram.imgLink').live('click', function () {
            var recordID = $(this).attr('recordID');
            var paramedicID = $(this).attr('paramedicID');
            if (onBeforeEditDeleteRTProgram(paramedicID)) {
                var message = "Kirim program radiasi untuk pasien ini ?";
                displayConfirmationMessageBox("Program Radiasi", message, function (result) {
                    if (result) {
                        var param = "send"+"|"+recordID;
                        cbpRTProgram.PerformCallback(param);
                    }
                });
            }
        });

        function onCbpRTProgramViewEndCallback(s) {
            var param = s.cpResult.split('|');
            var summaryText = s.cpSummary;
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdRTProgramView.ClientID %> tr:eq(1)').click();
                                   
                setPaging($("#programPaging"), pageCount, function (page) {
                    cbpRTProgramView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdRTProgramView.ClientID %> tr:eq(1)').click();

            $('#<%=hdnRTProgramSummaryText.ClientID %>').val(summaryText);   
        }

        function onCbpRTProgramEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == '1') {
                cbpRTProgramView.PerformCallback('refresh');
            }
            else {
                displayErrorMessageBox('Program Radiasi',"Program Radiasi tidak bisa dilakukan perubahan karena sudah memiliki Catatan Log Penyinaran");
            }
        }

        function onBeforeEditDeleteRTProgram(paramedicID) {
            alert(paramedicID);
            alert($('#<%=hdnParamedicID.ClientID %>').val());
            if ($('#<%=hdnParamedicID.ClientID %>').val() == paramedicID) {
                return true;
            }
            else {
                displayErrorMessageBox('EDIT/DELETE', 'Maaf, data hanya bisa diubah/dihapus oleh user yang melakukan pengentrian data tersebut.');
                return false;
            }
        }
        function onRefreshRTProgramViewGrid() {
            cbpRTProgramView.PerformCallback('refresh');
            $('#<%=hdnIsChanged.ClientID %>').val('1');
        }
        //#endregion

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
                    displayMessageBox('ORDER PEMERIKSAAN', 'Permintaan Order Pemeriksaan Penunjang telah berhasil dikirim kepada unit penunjang.');

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
                    displayErrorMessageBox('SEND ORDER : FAILED', 'Error Message : ' + param[3]);
                }
            }
        }

        function onCbpIntegrationNoteEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'integrationNote') {
                if (param[2] != 'success') {
                    var messageBody = param[3];
                    displayErrorMessageBox('Salin ke Catatan Terintegrasi', messageBody);
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
                var message = "Catatan SOAP Dokter belum disimpan, Disimpan terlebih dahulu sebelum meninggalkan halaman ?";
                displayConfirmationMessageBox('SOAP', message, function (result) {
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
            var message = "Catatan SOAP Dokter belum disimpan, Disimpan terlebih dahulu sebelum meninggalkan halaman ?";
            displayConfirmationMessageBox("SOAP",message, function (result) {
                if (result) {
                    onCustomButtonClick('save');
                    $('#<%=hdnIsChanged.ClientID %>').val('0');
                }
            });
        }

        function PrintEpisodeSummary(reportCode, param, callback) {
            openReportViewer(reportCode, param);
            callback();
        }
        //#endregion    

        function setRightPanelButtonEnabled() {
            var gcCustomerType = $('#<%:hdnGCCustomerType.ClientID %>').val();
            if (gcCustomerType == Constant.CustomerType.BPJS) {
                $('#btnBPJSFollowup').removeAttr('enabled');
            }
            else {
                $('#btnBPJSFollowup').attr('enabled', 'false');
            }
        }

        //#region Instruction
        $('#lblInstructionText').die('click');
        $('#lblInstructionText').live('click', function (evt) {
            var visitNoteID = 0;
            var param = "X058^07|" + visitNoteID + "|1|1";
            openUserControlPopup("~/libs/Controls/EMR/Lookup/SOAPTemplateLookupCtl1.ascx", param, "SOAP Template Text", 700, 500);
        });

        function onTxtInstructionTextChanged(value) {
            if (value.slice(-1) == ";") {
                var filterExpression = " UserID = " + <%=HttpUtility.HtmlEncode(GetUserID())%> + " AND TemplateCode = '" + value.slice(0, -1) + "' AND GCTextTemplateGroup ='X058^07'";
                Methods.getObject('GetvPhysicianTextList', filterExpression, function (obj) {
                    if (obj != null)
                        if ($('#<%=txtInstructionText.ClientID %>').val() != '') {
                            var message1 = "Ganti catatan di instruksi dokter dengan teks dari template ?";
                            var message2 = "<i>"+obj.TemplateText+"</i>";
                            displayConfirmationMessageBox('TEMPLATE TEXT', message1+"<br/><br/>"+message2, function (result) {
                                if (result) {
                                    $('#<%=txtInstructionText.ClientID %>').val(obj.TemplateText);
                                    $('#<%=txtInstructionText.ClientID %>').focus();
                                }
                            });
                        }
                });
            }
        }
        //#endregion

        //#region MIMS Allergen
        $('#lblMIMSAllergen').die('click');
        $('#lblMIMSAllergen').live('click', function (evt) {
            openSearchDialog('mimsallergen', 'IsDeleted = 0', function (value) {
                onTxtMIMSAllergenChanged(value);
            });
        });

        $('#<%=txtMIMSAllergenName.ClientID %>').live('change', function (evt) {
            onTxtMIMSAllergenValueChanged($(this).val());
        });

        function onTxtMIMSAllergenChanged(value) {
            var filterExpression = "ID = '" + value + "' AND IsDeleted = 0";
            Methods.getObject('GetMIMSAllergenList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnMIMSAllergenID.ClientID %>').val(result.ID);
                    $('#<%=txtMIMSAllergenName.ClientID %>').val(result.ItemDescription);
                }
                else {
                    $('#<%=hdnMIMSAllergenID.ClientID %>').val('');
                    $('#<%=txtMIMSAllergenName.ClientID %>').val('');
                }
            });
        }

        function onTxtMIMSAllergenValueChanged(value) {
            var filterExpression = "ItemDescription = '" + value + "' AND IsDeleted = 0";
            Methods.getObject('GetMIMSReferenceList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnMIMSAllergenID.ClientID %>').val(result.ID);
                    $('#<%=txtMIMSAllergenName.ClientID %>').val(result.ItemDescription);
                }
                else {
                    $('#<%=hdnMIMSAllergenID.ClientID %>').val('');
                    $('#<%=txtMIMSAllergenName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        function onBeforeLoadRightPanelContent(code) {
            var registrationID = $('#<%:hdnRegistrationID.ClientID %>').val();
            if (code == 'patientVisit') {
                return $('#<%:hdnRegistrationID.ClientID %>').val();
            }
            else if (code == 'healthyinformation' || code == 'medicalSickLeave' || code == 'medicalSickLeaveBilingual' || code == 'keteranganSehat'
                     || code == 'keteranganSehat1' || code == 'rujukanrslain' || code == 'keteranganIstirahat'
                     || code == 'keteranganIstirahat2') {
                return $('#<%:hdnRegistrationID.ClientID %>').val();
            }
            else {
                return $('#<%:hdnPageVisitID.ClientID %>').val();
            }
        }

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var visitID = $('#<%:hdnPageVisitID.ClientID %>').val();
            var registrationID = $('#<%:hdnRegistrationID.ClientID %>').val();
            var gcCustomerType = $('#<%:hdnGCCustomerType.ClientID %>').val();
            if (visitID == '') {
                errMessage.text = 'Please Select Patient Visit First!';
                return false;
            }
            else {
                if (code == 'MR000010') { //Surat Keterangan Kontrol BPJS
                    if (gcCustomerType == Constant.CustomerType.BPJS) {
                        filterExpression.text = registrationID;
                        return true;
                    }
                    else {
                        errMessage.text = "Tidak bisa cetak karena penjamin bayar bukan BPJS";
                        return false;
                    }
                }
                else {
                    filterExpression.text = visitID;
                    return true;
                }
            }
        }    
        
        function onAfterLookUpSOAPTemplate(value) {
            var valueInfo = value.split('|');
            switch (valueInfo[1]) {
                case 'X058^01':
                    $('#<%=txtChiefComplaint.ClientID %>').val(valueInfo[0]+';');
                    onTxtChiefComplaintChanged($('#<%=txtChiefComplaint.ClientID %>').val());
                    break;
                case 'X058^02':
                    $('#<%=txtHPISummary.ClientID %>').val(valueInfo[0]+';');
                    onTxtHPIChanged($('#<%=txtHPISummary.ClientID %>').val());
                    break;
                case 'X058^07':
                    $('#<%=txtInstructionText.ClientID %>').val(valueInfo[0]+';');
                    onTxtInstructionTextChanged($('#<%=txtInstructionText.ClientID %>').val());
                    break;
                default:
                    displayErrorMessageBox("ERROR : TEMPLATE TEXT","Unhandled Template Text Type");
                    break;
            }
        }    
        
        function onAfterDiagnosticResultSummary(value){
           var filterExpression= "ID='"+value+"' AND IsDeleted=0";
            Methods.getObject('GetChiefComplaintList', filterExpression, function (obj) {
                    if (obj != null)
                    { 
                            var message1 = "Ganti catatan hasil pemeriksaan dari history ?";
                            var message2 = "<i>"+obj.DiagnosticResultSummary+"</i>";
                            displayConfirmationMessageBox('TEMPLATE TEXT', message1+"<br/><br/>"+message2, function (result) {
                                if (result) {
                                    $('#<%=txtDiagnosticResultSummary.ClientID %>').val(obj.DiagnosticResultSummary);
                                }
                            });
                    }    
                });
        }
 
    function onCboServiceUnitValueChanged(param) {
        $('#<%=hdnLaboratoryServiceUnitID.ClientID %>').val(param);
    }   
          
    </script>
    <div>
        <input type="hidden" runat="server" id="hdnRegistrationID" value="" />
        <input type="hidden" runat="server" id="hdnPageVisitID" value="" />
        <input type="hidden" runat="server" id="hdnPageMRN" value="" />
        <input type="hidden" runat="server" id="hdnPageMedicalNo" value="" />
        <input type="hidden" runat="server" id="hdnPagePatientName" value="" />
        <input type="hidden" runat="server" id="hdnHealthcareServiceUnitID" value="" />
        <input type="hidden" runat="server" id="hdnChiefComplaintID" value="0" />
        <input type="hidden" runat="server" id="hdnAppointmentID" value="" />
        <input type="hidden" runat="server" id="hdnSavedAppointment" value="" />
        <input type="hidden" value="" id="hdnVitalSignRecordID" runat="server" />
        <input type="hidden" value="" id="hdnAllergyID" runat="server" />
        <input type="hidden" value="1" id="hdnAllergyProcessMode" runat="server" />
        <input type="hidden" value="" id="hdnDiagnosisID" runat="server" />
        <input type="hidden" value="1" id="hdnDiagnosisProcessMode" runat="server" />
        <input type="hidden" value="" id="hdnProcedureID" runat="server" />
        <input type="hidden" value="1" id="hdnProcedureProcessMode" runat="server" />
        <input type="hidden" value="" id="hdnReviewOfSystemID" runat="server" />
        <input type="hidden" value="" id="hdnImagingServiceUnitID" runat="server" />
        <input type="hidden" value="" id="hdnLaboratoryServiceUnitID" runat="server" />
        <input type="hidden" value="" id="hdnTestOrderHealthcareServiceUnitID" runat="server" />
        <input type="hidden" value="" id="hdnLaboratoryTestOrderID" runat="server" />
        <input type="hidden" value="" id="hdnImagingTestOrderID" runat="server" />
        <input type="hidden" value="" id="hdnDiagnosticTestOrderID" runat="server" />
        <input type="hidden" value="" id="hdnPrescriptionOrderID" runat="server" />
        <input type="hidden" value="" id="hdnSimulationRequestID" runat="server" />
        <input type="hidden" value="" id="hdnProgramID" runat="server" />
        <input type="hidden" value="" id="hdnGCCustomerType" runat="server" />
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
        <input type="hidden" value="1" id="hdnInstructionProcessMode" runat="server" />
        <input type="hidden" value="" id="hdnInstructionID" runat="server" />
        <input type="hidden" value="" id="hdnInstructionText" runat="server" />
        <input type="hidden" value="" id="hdnPatientVisitNoteID" runat="server" />
        <input type="hidden" value="" id="hdnSubjectiveText" runat="server" />
        <input type="hidden" value="" id="hdnObjectiveText" runat="server" />
        <input type="hidden" value="" id="hdnAssessmentText" runat="server" />
        <input type="hidden" value="" id="hdnImagingSummary" runat="server" />
        <input type="hidden" value="" id="hdnDiagnosticSummary" runat="server" />
        <input type="hidden" value="" id="hdnProcedureSummary" runat="server" />
        <input type="hidden" value="" id="hdnSelectedTabPageID" runat="server" />
        <input type="hidden" value="0" id="hdnSourceReferralVisitID" runat="server" />
        <input type="hidden" value="0" id="hdnPreviousVisitID" runat="server" />
        <input type="hidden" value="0" id="hdnIsCopySOAPFromPrevious" runat="server" />
        <input type="hidden" value="0" id="hdnIsCopyChiefComplaint" runat="server" />
        <input type="hidden" value="0" id="hdnIsCopyHPI" runat="server" />
        <input type="hidden" value="0" id="hdnIsCopyROS" runat="server" />
        <input type="hidden" value="0" id="hdnIsCopyVitalSign" runat="server" />
        <input type="hidden" value="" id="hdnPlanningText" runat="server" />
        <input type="hidden" id="hdnLOSInDay" runat="server" />
        <input type="hidden" id="hdnLOSInHour" runat="server" />
        <input type="hidden" id="hdnLOSInMinute" runat="server" />
        <input type="hidden" id="hdnRegistrationDateTime" runat="server" />
        <input type="hidden" id="hdnIsPhysicianDischarge" runat="server" />
        <input type="hidden" id="hdnPatientInformation" runat="server" />
        <input type="hidden" id="hdnIsHPIVisible" runat="server" value = "0" />
        <input type="hidden" id="hdnLaboratoryOrderSummaryText" runat="server" />
        <input type="hidden" id="hdnImagingSummaryText" runat="server" />
        <input type="hidden" id="hdnOtherTestSummaryText" runat="server" />
        <input type="hidden" id="hdnCTSimulatorSummaryText" runat="server" />
        <input type="hidden" id="hdnRTProgramSummaryText" runat="server" />
        <input type="hidden" id="hdnIsCCCopyFromNCC" runat="server" value = "0" />
        <input type="hidden" id="hdnParamedicID" runat="server" />
        <input type="hidden" id="hdnChiefComplaintParamedicID" runat="server" />
        <input type="hidden" id="hdnIsDefaultChecklistCopySoapDiagnosis" runat="server"/>
        <input type="hidden" id="hdnDefaultInstructionType" runat="server" />
        <input type="hidden" value="" id="hdnDatePickerToday" runat="server" />
        <input type="hidden" value="" id="hdnTimeToday" runat="server" />
        <input type="hidden" value="" id="hdnIsUsingButtomCompleteSession" runat="server" />
        <input type="hidden" value="" id="hdnMIMSAllergenID" runat="server" />         
        <input type="hidden" value="" id="hdnIsUsingDrugAlert" runat="server" />       
        <input type="hidden" value="" id="hdnAssessmentDateIsUsingRegDate" runat="server" />  
        <input type="hidden" value="" id="hdnIsUsingMultiVisitScheduleOrder" runat="server" />   
        <input type="hidden" runat="server" id="hdnDisableServiceDateTime" value="" />     
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
                                <td>
                                    <label class="lblMandatory">
                                        <%=GetLabel("Jenis Kunjungan") %></label>
                                </td>
                                <td>
                                    <table border="0" cellpadding="0" cellspacing="1">
                                        <tr>
                                            <td>
                                                <dxe:ASPxComboBox runat="server" ID="cboVisitType" ClientInstanceName="cboVisitType"
                                                    Width="230px" />                                                
                                            </td>
                                            <td />
                                        </tr>
                                    </table>

                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" valign="top">
                                    <table border="0" cellpadding="0" cellspacing="1">
                                        <tr>
                                            <td class="tdLabel" valign="top" style="width:120px">
                                                <label class="lblMandatory lblLink" id="lblChiefComplaint">
                                                    <%=GetLabel("Anamnesa")%></label>
                                            </td>
                                            <td><img class="imgLink" id="btnAddTemplate" title='<%=GetLabel("Add to My Template")%>' src='<%= ResolveUrl("~/Libs/Images/button/vitalsign.png")%>' alt="" /></td>
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtChiefComplaint" runat="server" TextMode="MultiLine" Rows="15"
                                        Width="100%" />
                                </td>
                            </tr>
                            <tr style="display:none">
                                <td class="tdLabel" style="width: 150px; vertical-align: top">
                                    <table border="0" cellpadding="0" cellspacing="1">
                                        <tr>
                                            <td class="tdLabel" valign="top" style="width:120px">
                                                <label class="lblMandatory lblLink" id="lblHPI">
                                                    <%=GetLabel("Riwayat Penyakit Sekarang")%></label>
                                            </td>
                                            <td><img class="imgLink" id="btnAddHPITemplate" title='<%=GetLabel("Add to My Template")%>' src='<%= ResolveUrl("~/Libs/Images/button/vitalsign.png")%>' alt="" /></td>
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtHPISummary" runat="server" Width="100%" TextMode="Multiline"
                                        Rows="10" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    &nbsp;
                                </td>
                                <td>
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td style="width: 40%">
                                                <asp:CheckBox ID="chkAutoAnamnesis" runat="server" Text=" Autoanamnesis" Checked="false" />
                                            </td>
                                            <td style="width: 40%">
                                                <asp:CheckBox ID="chkAlloAnamnesis" runat="server" Text=" Alloanamnesis / Heteroanamnesis"
                                                    Checked="false" />
                                            </td>
                                            <td style="width: 40%">
                                                <asp:CheckBox ID="chkIsFastTrack" runat="server" Text=" Fast Track" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="vertical-align: top">
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
                            <%=GetLabel("Riwayat Penyakit Dahulu")%></h4>
                        <div class="containerTblEntryContent" style="display:none">
                            <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 5px">
                                <colgroup>
                                    <col width="150px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                        <%=GetLabel("Riwayat Penyakit Dahulu") %>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMedicalHistory" runat="server" Width="100%" TextMode="Multiline"
                                            Rows="5" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <h4 class="h4collapsed">
                            <%=GetLabel("Riwayat Penggunaan Obat")%></h4>
                        <div class="containerTblEntryContent" style="display:none">
                            <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 5px">
                                <colgroup>
                                    <col width="150px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                        <%=GetLabel("Riwayat Penggunaan Obat") %>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMedicationHistory" runat="server" Width="100%" TextMode="Multiline"
                                            Rows="5" />
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
                                                    <td id="tdlblMIMSAllergen" runat="server">
                                                        <div style="display:none">
                                                            <label class="lblLink" id="lblMIMSAllergen">
                                                                <%=GetLabel("MIMS Allergen")%></label>
                                                            </div>
                                                    </td>
                                                    <td id="tdMIMSAllergenName" runat="server">
                                                        <div style="display:none">
                                                            <asp:TextBox ID="txtMIMSAllergenName" runat="server" Width="150px" />
                                                        </div>
                                                    </td>
                                                    <td style="padding-left: 5px">
                                                        <table border="0" cellpadding="0" cellspacing="1">
                                                            <tr>
                                                                <td>
                                                                    <img class="btnApplyAllergy imgLink" title='<%=GetLabel("Apply")%>' src='<%= ResolveUrl("~/Libs/Images/status/done.png")%>'
                                                                        alt="" />
                                                                </td>
                                                                <td>
                                                                    <img class="btnCancelAllergy imgLink" title='<%=GetLabel("Cancel")%>' src='<%= ResolveUrl("~/Libs/Images/status/cancel.png")%>'
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
                                                                            <input type="hidden" value="<%#:Eval("MIMSAllergenID") %>" bindingfield="MIMSAllergenID" />
                                                                            <input type="hidden" value="<%#:Eval("ItemDescription") %>" bindingfield="ItemDescription" />
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
                                                                            <input type="hidden" value='<%#: Eval("ID") %>' bindingfield="ID" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                                        <HeaderTemplate>
                                                                            <div style="text-align: right">
                                                                                <span class="lblLink" id="lblAddVitalSign">
                                                                                    <%= GetLabel("+ Tambah Tanda Vital")%></span>
                                                                            </div>
                                                                            <div style="text-align: right">
                                                                                <span class="lblLink" id="lblAddFromVitalSignLookup">
                                                                                    <%= GetLabel("+ Salin Tanda Vital")%></span>
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
                                            <div class="containerPaging" style="max-width: 730px">
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
                                                                                                    : </strong></span>&nbsp; <span <%# Eval("IsNormal").ToString() == "False" && Eval("IsNotExamined").ToString() == "False" && Eval("IsOther").ToString() == "False"  ? "Style='color:red;font-style:italic'":"" %>>
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
                                            style="display: none; float: right; margin-top: 0px;">
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
                                                <col width="160px" />
                                                <col width="150px" />
                                                <col width="100px" />
                                                <col width="150px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Medical Problem")%></label>
                                                </td>
                                                <td colspan="4">
                                                    <asp:TextBox ID="txtMedicalProblem" runat="server" Width="98%" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Stadium")%></label>
                                                </td>
                                                <td>
                                                    <dxe:ASPxComboBox runat="server" ID="cboCancerStaging" ClientInstanceName="cboCancerStaging"
                                                        Width="100%">
                                                    </dxe:ASPxComboBox>                                                    
                                                </td>
                                                <td>
                                                    <label class="lblNormal">
                                                        <%=GetLabel("TNM")%></label>
                                                </td>
                                                <td colspan="2">
                                                    <table border="0" cellpadding="1" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width: 30px" />
                                                            <col style="width: 40px" />
                                                            <col style="width: 30px" />
                                                            <col style="width: 40px" />
                                                            <col style="width: 30px" />
                                                            <col style="width: 40px" />
                                                        </colgroup>
                                                        <tr>
                                                            <td style="text-align:right">T</td>
                                                            <td><asp:TextBox ID="txtTValue" runat="server" Width="60px" style="text-align:right" /></td>
                                                            <td style="text-align:right">N</td>
                                                            <td><asp:TextBox ID="txtNValue" runat="server" Width="60px" style="text-align:right" /></td>
                                                            <td style="text-align:right">M</td>
                                                            <td><asp:TextBox ID="txtMValue" runat="server" Width="60px" style="text-align:right" /></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
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
                                            <tr>
                                                <td colspan="4">
                                                    <asp:CheckBox ID="chkIsComplexVisit" runat="server" Text=" Kasus Kompleks (Multi-Diagnosa)" Checked="false" />
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
                                                        <input type="hidden" id="hdnDiagnosisSummary" runat="server" />
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
                                                                            <table border="0" cellpadding="1" cellspacing="0" style="width:100%">
                                                                                <colgroup>
                                                                                    <col  />
                                                                                    <col width="250px" />
                                                                                </colgroup>
                                                                                <tr>
                                                                                    <td><%=GetLabel("Diagnosis")%></td>
                                                                                    <td><span class="lblLink" id="lblAddFromDxHistory2"><%= GetLabel("+ Salin dari Riwayat Diagnosa Pasien")%></span></td>
                                                                                </tr>
                                                                            </table>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <div <%# Eval("cfIsRuledOut").ToString() == "True" ? "Style='text-decoration: line-through'":"" %>>
                                                                                <%#: Eval("DifferentialDateInString")%>,
                                                                                <%#: Eval("DifferentialTime")%>,
                                                                                <%#: Eval("cfParamedicNameEarlyDiagnosis")%></div>
                                                                            <div <%# Eval("cfIsRuledOut").ToString() == "True" ? "Style='text-decoration: line-through'":"" %>>
                                                                                <span style="color: Blue; font-size: 1.1em">
                                                                                    <%#: Eval("DiagnosisText")%></span> (<b><%#: Eval("DiagnoseID")%></b>)
                                                                            </div>
                                                                            <div <%# Eval("cfIsRuledOut").ToString() == "True" ? "Style='text-decoration: line-through'":"" %>>
                                                                                <%#: Eval("ICDBlockName")%></div>
                                                                            <div <%# Eval("cfIsRuledOut").ToString() == "True" ? "Style='text-decoration: line-through'":"" %>>
                                                                                <b>
                                                                                    <%#: Eval("DiagnoseType")%></b> -
                                                                                <%#: Eval("DifferentialStatus")%></div>
                                                                            <div <%# Eval("cfIsRuledOut").ToString() == "True" ? "Style='text-decoration: line-through'":"" %>>
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
                                                                    <br />
                                                                    <span class="lblLink" id="lblAddFromDxHistory">
                                                                        <%= GetLabel("+ Salin dari Riwayat Diagnosa Pasien")%></span>
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
                            <%=GetLabel("Pemeriksaan : Laboratorium")%></h4>
                        <div class="containerTblEntryContent containerEntryPanel1">
                            <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                                <tr>
                                    <td>
                                        <table border="0" cellpadding="1" cellspacing="0" style="width:100%">
                                            <colgroup>
                                                <col style="width:120px"/>
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td class="tdLabel"><%=GetLabel("Unit Layanan") %></td>
                                                <td colspan="3">
                                                    <dxe:ASPxComboBox ID="cboServiceUnit" ClientInstanceName="cboServiceUnit" Width="350px" runat="server">
                                                        <ClientSideEvents ValueChanged="function(s,e){ onCboServiceUnitValueChanged(s.GetValue()); }"
                                                            Init="function(s,e){ onCboServiceUnitValueChanged(s.GetValue()); }" />
                                                    </dxe:ASPxComboBox>
                                                </td>
                                            </tr>                                     
                                        </table>
                                    </td>
                                </tr>
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
                                                                            <input type="hidden" value="<%#:Eval("HealthcareServiceUnitID") %>" bindingfield="HealthcareServiceUnitID" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="center" HeaderStyle-Width="100px" ItemStyle-Width="20px">
                                                                        <HeaderTemplate>
                                                                            <div style="text-align: center">
                                                                                <span class="lblLink" id="lblAddLabOrder2"><%= GetLabel("+ Order (Form)")%></span>
                                                                            </div>                                                                            
                                                                        </HeaderTemplate>
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
                                                                                    <col style="width: 60%" />
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
                                                                                                <%= GetLabel("+ Tambah Order (List)")%></span>
                                                                                        </div>
                                                                                    </td>
                                                                                    <td>
                                  
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
                            <%=GetLabel("Pemeriksaan : Radiologi")%></h4>
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
                                                                            <input type="hidden" value="<%#:Eval("HealthcareServiceUnitID") %>" bindingfield="HealthcareServiceUnitID" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                                        <ItemTemplate>
                                                                            <input type="hidden" value="<%#:Eval("NoOfDetails") %>" bindingfield="NoOfDetails" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" ItemStyle-Width="20px">
                                                                        <HeaderTemplate>
                                                                            <div style="text-align: center">
                                                                                <span class="lblLink" id="lblAddImagingOrder2"><%= GetLabel("+ Order (Tanpa Detail)")%></span>
                                                                                <BR>
                                                                                <span class="lblLink" id="lblAddImagingOrder3"><%= GetLabel("+ Order (Form)")%></span>
                                                                            </div>   
                                                                        </HeaderTemplate>
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
                            <%=GetLabel("Pemeriksaan : Penunjang Medis Lain-lain")%></h4>
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
                                        <label id="lblDiagnosticResultSummary" class="lblNormal lblLink" >
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
                            <%=GetLabel("Permintaan CT Simulator")%></h4>
                        <div class="containerTblEntryContent containerEntryPanel2">
                            <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                                <tr>
                                    <td>
                                        <div style="position: relative;">
                                            <dxcp:ASPxCallbackPanel ID="cbpCTSimulatorView" runat="server" Width="100%" ClientInstanceName="cbpCTSimulatorView"
                                                ShowLoadingPanel="false" OnCallback="cbpCTSimulatorView_Callback">
                                                <ClientSideEvents EndCallback="function(s,e){ onCbpCTSimulatorViewEndCallback(s); }" />
                                                <PanelCollection>
                                                    <dx:PanelContent ID="PanelContentCTSimulatorView" runat="server">
                                                        <asp:Panel runat="server" ID="PanelCTSimulatorView" CssClass="pnlContainerGridPatientPage">
                                                            <asp:GridView ID="grdCTSimulatorView" runat="server" CssClass="grdSelected grdPatientPage"
                                                                AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                                <Columns>
                                                                    <asp:BoundField DataField="SimulationRequestID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                    <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                                        <ItemTemplate>
                                                                            <input type="hidden" value="<%#:Eval("SimulationRequestID") %>" bindingfield="SimulationRequestID" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" ItemStyle-Width="20px">
                                                                        <HeaderTemplate>
                                                                            <div style="text-align: center">
                                                                            <img class="imgAddCTSimulatorRequest imgLink" title='<%=GetLabel("+ Permintaan Simulasi")%>' src='<%# ResolveUrl("~/Libs/Images/Button/plus.png")%>'
                                                                                alt="" />
                                                                            </div>   
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <table cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td>
                                                                                        <img class="imgEditCTSimulatorRequest imgLink" title='<%=GetLabel("Edit Permintaan Simulasi")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                            alt="" recordid="<%#:Eval("SimulationRequestID") %>" paramedicID="<%#:Eval("ParamedicID") %>" <%# Eval("GCRequestStatus").ToString() != "X121^001" ? "Style='display:none'":"" %> />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgDeleteCTSimulatorRequest imgLink" title='<%=GetLabel("Delete Permintaan Simulasi")%>'
                                                                                            src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>' recordid="<%#:Eval("SimulationRequestID") %>" paramedicID="<%#:Eval("ParamedicID") %>" <%# Eval("GCRequestStatus").ToString() != "X121^001" ? "Style='display:none'":"" %> />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgSendCTSimulatorRequest imgLink" title='<%=GetLabel("Send Request")%>' src='<%# ResolveUrl("~/Libs/Images/Button/done.png")%>'
                                                                                            alt="" recordid="<%#:Eval("SimulationRequestID") %>" paramedicID="<%#:Eval("ParamedicID") %>" <%# Eval("GCRequestStatus").ToString() == "X121^003" ? "Style='display:none'":"" %>/>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField HeaderText="Tanggal" DataField="cfRequestDateText" HeaderStyle-HorizontalAlign="Center"
                                                                        ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="cfRequestDateText" HeaderStyle-Width="100px" />
                                                                    <asp:BoundField HeaderText="Jenis Permintaan" DataField="SimulationRequestType" HeaderStyle-HorizontalAlign="Left"
                                                                        ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="simulationRequestType" HeaderStyle-Width="200px" />
                                                                    <asp:BoundField HeaderText="Scan Area" DataField="cfScanArea" HeaderStyle-HorizontalAlign="Left"
                                                                        ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="cfScanArea" />
                                                                </Columns>
                                                                <EmptyDataTemplate>
                                                                    <%=GetLabel("Tidak ada data permintaan CT Simulator Pasien untuk kunjungan ini")%>
                                                                </EmptyDataTemplate>
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </dx:PanelContent>
                                                </PanelCollection>
                                            </dxcp:ASPxCallbackPanel>
                                            <div class="containerPaging">
                                                <div class="wrapperPaging">
                                                    <div id="Div2">
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <h4 class="h4collapsed">
                            <%=GetLabel("Program Radiasi")%></h4>
                        <div class="containerTblEntryContent containerEntryPanel2">
                            <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                                <tr>
                                    <td>
                                        <div style="position: relative;">
                                            <dxcp:ASPxCallbackPanel ID="cbpRTProgramView" runat="server" Width="100%" ClientInstanceName="cbpRTProgramView"
                                                ShowLoadingPanel="false" OnCallback="cbpRTProgramView_Callback">
                                                <ClientSideEvents EndCallback="function(s,e){ onCbpRTProgramViewEndCallback(s); }" />
                                                <PanelCollection>
                                                    <dx:PanelContent ID="PanelContentRTProgramView" runat="server">
                                                        <asp:Panel runat="server" ID="PanelRTProgramView" CssClass="pnlContainerGridPatientPage">
                                                            <asp:GridView ID="grdRTProgramView" runat="server" CssClass="grdSelected grdPatientPage"
                                                                AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                                <Columns>
                                                                    <asp:BoundField DataField="ProgramID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" ItemStyle-Width="20px">
                                                                        <HeaderTemplate>
                                                                            <div style="text-align: center">
                                                                            <img class="imgAddRTProgram imgLink" title='<%=GetLabel("+ Program Radioterapi")%>' src='<%# ResolveUrl("~/Libs/Images/Button/plus.png")%>'
                                                                                alt="" />
                                                                            </div>   
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <table cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td>
                                                                                        <img class="imgEditRTProgram imgLink" title='<%=GetLabel("Edit Program Radioterapi")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                            alt="" recordid="<%#:Eval("ProgramID") %>" paramedicID="<%#:Eval("ParamedicID") %>" <%# Eval("GCProgramStatus").ToString() != "X121^001" ? "Style='display:none'":"" %> />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgDeleteRTProgram imgLink" title='<%=GetLabel("Delete Program Radioterapi")%>'
                                                                                            src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>' recordid="<%#:Eval("ProgramID") %>" paramedicID="<%#:Eval("ParamedicID") %>" <%# Eval("GCProgramStatus").ToString() != "X121^001" ? "Style='display:none'":"" %> />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgSendRTProgram imgLink" title='<%=GetLabel("Send Request")%>' src='<%# ResolveUrl("~/Libs/Images/Button/done.png")%>'
                                                                                            alt="" recordid="<%#:Eval("ProgramID") %>" paramedicID="<%#:Eval("ParamedicID") %>" <%# Eval("GCProgramStatus").ToString() == "X121^003" || Eval("GCProgramStatus").ToString() == "X121^005" ? "Style='display:none'":"" %>/>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="cfProgramDate" HeaderText="Tanggal" HeaderStyle-Width="90px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"/>
                                                                    <asp:TemplateField HeaderText="Tipe Radioterapi" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <div>
                                                                                <span style="font-weight:bold"><%#Eval("TherapyType")%></span>
                                                                            </div>
                                                                            <div style="<%# Eval("GCTherapyType").ToString() != "X582^001" ? "display:none": "" %>">
                                                                                <span style="font-style:italic">Teknik : </span><span style="font-weight:bold"><%#Eval("cfRadiotherapyBeamTechnique")%></span>
                                                                            </div>
                                                                            <div style="<%# Eval("GCTherapyType").ToString() != "X582^002" ? "display:none": "" %>">
                                                                                <span style="font-style:italic">Jenis : </span><span style="font-weight:bold"><%#Eval("BrachytherapyType")%></span>
                                                                            </div>
                                                                            <div style="<%# Eval("GCTherapyType").ToString() != "X582^002" ? "display:none": "" %>">
                                                                                <span style="font-style:italic">Aplikator : </span><span style="font-weight:bold"><%#Eval("ApplicatorType")%></span>
                                                                            </div>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="TherapyPurpose" HeaderText="Tujuan Radiasi" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
                                                                </Columns>
                                                                <EmptyDataTemplate>
                                                                    <%=GetLabel("Tidak ada data Program Radioterapi Pasien untuk kunjungan ini")%>
                                                                </EmptyDataTemplate>
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </dx:PanelContent>
                                                </PanelCollection>
                                            </dxcp:ASPxCallbackPanel>
                                            <div class="containerPaging">
                                                <div class="wrapperPaging">
                                                    <div id="Div3">
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <h4 class="h4collapsed">
                            <%=GetLabel("Prosedur/Tindakan")%></h4>
                        <div class="containerTblEntryContent containerEntryPanel2">
                            <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px;">
                                <tr>
                                    <td>
                                        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                            <colgroup>
                                                <col width="150px" />
                                                <col width="150px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td style="padding-left: 5px">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Prosedur/Tindakan")%></label>
                                                </td>

                                                <td colspan="3">
                                                    <input type="hidden" value="" id="hdnEntryProcedureID" runat="server" />
                                                    <input type="hidden" value="" id="hdnEntryProcedureText" runat="server" />
                                                    <qis:QISSearchTextBox ID="ledProcedure" ClientInstanceName="ledProcedure" runat="server"
                                                        Width="99%" ValueText="ProcedureID" FilterExpression="" DisplayText="ProcedureName"
                                                        MethodName="GetvProceduresList">
                                                        <ClientSideEvents ValueChanged="function(s){ onLedProcedureLostFocus(s); }" />
                                                        <Columns>
                                                            <qis:QISSearchTextBoxColumn Caption="Procedure Name (Code)" FieldName="ProcedureName"
                                                                Description="i.e. Isolation" Width="500px" />
                                                        </Columns>
                                                    </qis:QISSearchTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="padding-left: 5px">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Prosedur/Tindakan Text")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtProcedureText" runat="server" Width="370px" />
                                                </td>
                                                <td style="padding-left: 5px">
                                                    <table border="0" cellpadding="0" cellspacing="1">
                                                        <tr>
                                                            <td>
                                                                <img class="btnApplyProcedure imgLink" title='<%=GetLabel("Apply")%>' src='<%= ResolveUrl("~/Libs/Images/status/done.png")%>'
                                                                    alt="" />
                                                            </td>
                                                            <td>
                                                                <img class="btnCancelProcedure imgLink" title='<%=GetLabel("Cancel")%>' src='<%= ResolveUrl("~/Libs/Images/status/cancel.png")%>'
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
                                            <dxcp:ASPxCallbackPanel ID="cbpProcedureView" runat="server" Width="100%" ClientInstanceName="cbpProcedureView"
                                                ShowLoadingPanel="false" OnCallback="cbpProcedureView_Callback">
                                                <ClientSideEvents EndCallback="function(s,e){ onCbpProcedureViewEndCallback(s); }" />
                                                <PanelCollection>
                                                    <dx:PanelContent ID="PanelContent11" runat="server">
                                                        <asp:Panel runat="server" ID="Panel11" CssClass="pnlContainerGrid" Style="height: 300px">
                                                            <asp:GridView ID="grdProcedureView" runat="server" CssClass="grdSelected grdPatientPage"
                                                                AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                                <Columns>
                                                                    <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                    <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                                        <ItemTemplate>
                                                                            <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                                            <input type="hidden" value="<%#:Eval("ProcedureID") %>" bindingfield="ProcedureID" />
                                                                            <input type="hidden" value="<%#:Eval("ProcedureText") %>" bindingfield="ProcedureText" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20px">
                                                                        <ItemTemplate>
                                                                            <table cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td>
                                                                                        <img class="imgEditProcedure imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                            alt="" />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgDeleteProcedure imgLink" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                            alt="" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                                        <HeaderTemplate>
                                                                            <%=GetLabel("Procedure Information")%>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <div <%# Eval("IsDeleted").ToString() == "True" ? "Style='text-decoration: line-through'":"" %>>
                                                                                <%#: Eval("ProcedureDateInString")%>,
                                                                                <%#: Eval("ProcedureTime")%>,
                                                                                <%#: Eval("ParamedicName")%></div>
                                                                            <div <%# Eval("IsDeleted").ToString() == "True" ? "Style='text-decoration: line-through'":"" %>>
                                                                                <span style="color: Blue; font-size: 1.1em">
                                                                                    <%#: Eval("ProcedureText")%></span> (<b><%#: Eval("ProcedureID")%></b>)
                                                                            </div>
                                                                            <div <%# Eval("IsDeleted").ToString() == "True" ? "Style='text-decoration: line-through'":"" %>>
                                                                                <%#: Eval("Remarks")%></div>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                                        <ItemTemplate>
                                                                            <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                                            <input type="hidden" value="<%#:Eval("ProcedureID") %>" bindingfield="ProcedureID" />
                                                                            <input type="hidden" value="<%#:Eval("ProcedureText") %>" bindingfield="ProcedureText" />
                                                                            <input type="hidden" value="<%#:Eval("ParamedicID") %>" bindingfield="ParamedicID" />
                                                                            <input type="hidden" value="<%#:Eval("ProcedureDateInDatePickerFormat") %>" bindingfield="ProcedureDate" />
                                                                            <input type="hidden" value="<%#:Eval("ProcedureTime") %>" bindingfield="ProcedureTime" />
                                                                            <input type="hidden" value="<%#:Eval("Remarks") %>" bindingfield="Remarks" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <EmptyDataTemplate>
                                                                    <%=GetLabel("Belum ada informasi Prosedur/Tindakan untuk pasien ini") %>
                                                                </EmptyDataTemplate>
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </dx:PanelContent>
                                                </PanelCollection>
                                            </dxcp:ASPxCallbackPanel>
                                            <div class="containerPaging">
                                                <div class="wrapperPaging">
                                                    <div id="procedurePaging">
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <h4 class="h4collapsed">
                            <%=GetLabel("Catatan Tindakan/Terapi")%></h4>
                        <div class="containerTblEntryContent">
                            <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                                <colgroup>
                                    <col width="150px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                        <label class="lblNormal" id="Label2">
                                            <%=GetLabel("Catatan Tindakan/Terapi") %></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPlanningNotes" runat="server" Width="100%" TextMode="Multiline"
                                            Rows="10" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <h4 class="h4collapsed">
                            <%=GetLabel("Instruksi Dokter")%></h4>
                        <div class="containerTblEntryContent containerEntryPanel1">
                            <div style="position: relative;">
                                <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 5px">
                                    <colgroup>
                                        <col width="150px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td class="tdLabel" valign="top">
                                            <table border="0" cellpadding="0" cellspacing="1">
                                                <tr>
                                                    <td class="tdLabel" valign="top" style="width: 120px">
                                                        <label class="lblNormal lblLink" id="lblInstructionText">
                                                            <%=GetLabel("Instruksi Dokter")%></label>
                                                    </td>
                                                    <td>
                                                        <img class="imgLink" id="btnAddInstructionTemplate" title='<%=GetLabel("Add to My Template")%>'
                                                            src='<%= ResolveUrl("~/Libs/Images/button/vitalsign.png")%>' alt="" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtInstructionText" runat="server" Width="100%" TextMode="Multiline"
                                                Rows="8" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                       <div id="DisplayNextVisitSchedule" runat="server">
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
                                <tr id="trKunjunganBerikutnya" runat="server"> 
                                    <td>
                                        <label class="lblNormal" id="lblJenisKunjungan">
                                            <%=GetLabel("Jenis Kunjungan Berikutnya") %></label>
                                    </td>
                                    <td>
                                        <dxe:ASPxComboBox runat="server" ID="cboFollowupVisitType" ClientInstanceName="cboFollowupVisitType"
                                            Width="100%" />

                                    </td>
                                </tr>
                                <tr id="trTanggalKunjungan" runat="server">
                                    <td class="tdLabel" id="lblTanggalKunjungan"><label class="lblNormal"><%=GetLabel("Tanggal Kunjungan Berikutnya")%></label></td>
                                    <td>
                                        <table border="0" cellpadding = "0" cellspacing="0">
                                            <tr>
                                                <td><asp:TextBox runat="server" ID="txtFollowupVisitDate" CssClass="datepicker" Width="120px" /></td>
                                                <%--<td><asp:TextBox ID="txtFollowupVisitTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" /></td>--%>
                                             </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr id="trCatatanKunjungan" runat="server">
                                    <td class="tdLabel" style="vertical-align:top">
                                        <label class="lblNormal" runat="server" id="Label1">
                                            <%:GetLabel("Catatan Kunjungan Berikutnya")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFollowupVisitRemarks" runat="server" Width="100%" TextMode="Multiline"
                                            Rows="3" />
                                    </td>
                                </tr>
                               <tr id="trCatatanPerjanjian" runat="server">
                                    <td class="tdLabel" style="vertical-align:top">
                                        <label class="lblNormal" runat="server" id="Label4">
                                            <%:GetLabel("Catatan Perjanjian Selanjutnya")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAppointmentInfo" runat="server" Width="100%" TextMode="Multiline"
                                            Rows="3" />
                                    </td>
                                </tr>
                            </table>
                        </div>
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
            <dxcp:ASPxCallbackPanel ID="cbpProcedure" runat="server" Width="100%" ClientInstanceName="cbpProcedure"
                ShowLoadingPanel="false" OnCallback="cbpProcedure_Callback">
                <ClientSideEvents EndCallback="function(s,e){ onCbpProcedureEndCallback(s); }" />
            </dxcp:ASPxCallbackPanel>
        </div>
        <div style="display: none">
            <dxcp:ASPxCallbackPanel ID="cbpDeleteTestOrder" runat="server" Width="100%" ClientInstanceName="cbpDeleteTestOrder"
                ShowLoadingPanel="false" OnCallback="cbpDeleteTestOrder_Callback">
                <ClientSideEvents EndCallback="function(s,e){ onCbpDeleteTestOrderEndCallback(s); }" />
            </dxcp:ASPxCallbackPanel>
        </div>
        <div>
            <dxcp:ASPxCallbackPanel ID="cbpIntegrationNote" runat="server" Width="100%" ClientInstanceName="cbpIntegrationNote"
                ShowLoadingPanel="false" OnCallback="cbpIntegrationNote_Callback">
                <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpIntegrationNoteEndCallback(s); }" />
            </dxcp:ASPxCallbackPanel>
        </div>
        <dxcp:ASPxCallbackPanel ID="cbpSendOrder" runat="server" Width="100%" ClientInstanceName="cbpSendOrder"
            ShowLoadingPanel="false" OnCallback="cbpSendOrder_Callback">
            <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpSendOrderEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
        <dxcp:ASPxCallbackPanel ID="cbpDoctorFee" runat="server" Width="100%" ClientInstanceName="cbpDoctorFee"
            ShowLoadingPanel="false" OnCallback="cbpDoctorFee_Callback">
            <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpDoctorFeeEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
        <dxcp:ASPxCallbackPanel ID="cbpSOAPCopy" runat="server" Width="100%" ClientInstanceName="cbpSOAPCopy"
            ShowLoadingPanel="false" OnCallback="cbpSOAPCopy_Callback">
            <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpSOAPCopyEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
        <div style="display: none">
            <dxcp:ASPxCallbackPanel ID="cbpCTSimulation" runat="server" Width="100%" ClientInstanceName="cbpCTSimulation"
                ShowLoadingPanel="false" OnCallback="cbpCTSimulation_Callback">
                <ClientSideEvents EndCallback="function(s,e){ onCbpCTSimulationEndCallback(s); }" />
            </dxcp:ASPxCallbackPanel>
        </div>
        <div style="display: none">
            <dxcp:ASPxCallbackPanel ID="cbpRTProgram" runat="server" Width="100%" ClientInstanceName="cbpRTProgram"
                ShowLoadingPanel="false" OnCallback="cbpRTProgram_Callback">
                <ClientSideEvents EndCallback="function(s,e){ onCbpRTProgramEndCallback(s); }" />
            </dxcp:ASPxCallbackPanel>
        </div>
    </div>
    <div id="divContainerShortcutPreviousVisit">
        <div>
            <div id="divCloseShortcutPreviousVisit">X</div>
            <div id="divOpenShortcutPreviousVisit"><b>Kunjungan Sebelumnya : <span style="color:Blue;"><asp:Label ID="lblVisitHistoryInfo" runat="server" Text="dd-MMM-yyyy, Physician Name" /></span></b></div>
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
                                                <span <%# (Eval("RefRange").ToString() == "" || Eval("IsResultInPDF").ToString() == "True") ? "Style='display:none'":"Style='color:black;font-style:italic'" %>>(<%#: DataBinder.Eval(Container.DataItem, "RefRange") %>)</span>
                                                <span <%# Eval("IsResultInPDF").ToString() == "False" ? "Style='display:none'":"" %>><input type="button" id="btnViewPDF" runat="server" class="btnViewPDF" value="View PDF"
                                                    style="height: 25px; width: 100px; background-color: Red; color: White;" pdfValue = '<%#: DataBinder.Eval(Container.DataItem, "PDFValue") %>' /></span>
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
                                            <div style="padding-left:10px;">
                                                <span>
                                                    <%#: DataBinder.Eval(Container.DataItem, "Resultvalue") %>        
                                                </span>                                                                                        
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
                    <a style="font-size:12px;"><%= GetLabel("CATATAN : HASIL PEMERIKSAAN PENUNJANG DAN TINDAKAN")%></a> 
                    <div class="containerPreviousVisitUl">
                        <div class="divPreviousVisitContentTitle" style="margin-top:10px; margin-bottom: 5px"><%= GetLabel("Catatan : Hasil Pemeriksaan Penunjang")%></div>
                        <div id="divDiagnosticResultSummaryContent" class="divPreviousVisitContent" runat="server" style="white-space:nowrap; overflow:auto"></div>
                        <div class="divPreviousVisitContentTitle" style="margin-top:10px; margin-bottom: 5px"><%= GetLabel("Catatan Tindakan")%></div>
                        <div id="divPlanningSummary" class="divPreviousVisitContent" runat="server" style="white-space:nowrap; overflow:auto"></div>
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
                                    -
                                    <%#: Eval("MedicationAdministration")%>
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

                    <a style="font-size:12px;"><%= GetLabel("PROCEDURE ATAU TINDAKAN")%></a> 
                    <div class="containerPreviousVisitUl">
                        <asp:Repeater ID="rptPatientProcedure" runat="server">
                            <HeaderTemplate>
                                <ul>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <li>
                                    <span><%#: Eval("ProcedureDate")%> <%#: Eval("ProcedureTime")%> <%#: Eval("ParamedicName")%></span>
                                    <div style="white-space:normal; overflow:auto">
                                        <%#: Eval("ProcedureText")%>
                                    </div>
                                </li>
                            </ItemTemplate>
                            <FooterTemplate>
                                </ul>
                            </FooterTemplate>
                        </asp:Repeater>
                    </div>

                    <a style="font-size:12px;"><%= GetLabel("INSTRUKSI DOKTER")%></a> 
                    <div class="containerPreviousVisitUl">
                        <asp:Repeater ID="rptPatientInstruction" runat="server">
                            <HeaderTemplate>
                                <ul>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <li>
                                    <span><%#: Eval("InstructionDate")%> <%#: Eval("InstructionTime")%> <%#: Eval("PhysicianName")%></span>
                                    <div style="white-space:normal; overflow:auto">
                                        <%#: Eval("Description")%>
                                    </div>
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
                    <div style="padding:5px;">
                        <table cellpadding="0" cellspacing="10" style="margin-left:auto;margin-right:auto;text-align:left">
                            <tr>
                                <td><input type="button" value='<%= GetLabel("SOAP COPY")%>' onclick="pcSOAPCopy.Show();" style="width:100px; height:25px" /></td>
                            </tr>
                        </table>                
                    </div>
                </div>
            </div>
            <div style="padding:5px;">
                <table cellpadding="0" cellspacing="10" style="margin-left:auto;margin-right:auto;text-align:left">
                    <tr>
                        <td><input type="button" value='<%= GetLabel("SOAP COPY")%>' onclick="pcSOAPCopy.Show();" style="width:100px;" class="w3-btn w3-hover-blue" /></td>
                    </tr>
                </table>                
            </div>
        </div>
    </div>

    <div id="divFooterTab2" runat="server">
        <div id="divContainerShortcutMainVisit">
            <div>
                <div id="divCloseShortcutMainVisit">
                    X</div>
                <div id="divOpenShortcutMainVisit">
                    <b>Informasi Rujukan : <span style="color: Blue;">
                        <asp:Label ID="lblReferralVisitHistoryInfo" runat="server" Text="dd-MMM-yyyy, Physician Name" /></span></b></div>
                <div id="divReferralVisitDetailInfo" style="margin-top: 5px">
                    <b>Nomor Registrasi: <span style="color: Red;">
                        <asp:Label ID="lblReferralRegistrationNo" runat="server" Text="" /></span></b></div>
                <div style="padding: 2px; width: 100%; height: 100%; background-color: #dfe6e9">
                    <table style="width:100%">
                        <colgroup>
                            <col style="width:150px"/>
                            <col style="width:150px"/>
                            <col />
                        </colgroup>
                        <tr>
                            <td colspan="3">
                                <label class="lblMandatory"><%=GetLabel("Diagnosis Pasien:")%></label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3"><asp:TextBox ID="txtSourceDiagnosisText" Width="95%" runat="server" TextMode="MultiLine" Rows="2" ReadOnly=true/></td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <label class="lblMandatory"><%=GetLabel("Pemeriksaan Fisik / Pemeriksaan Penunjang / Catatan Medis lain yang perlu mendapat perhatian :")%></label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3"><asp:TextBox ID="txtSourceMedicalResumeText" Width="95%" runat="server" TextMode="MultiLine" Rows="8" ReadOnly=true /></td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <label class="lblMandatory"><%=GetLabel("Terapi yang telah diberikan :")%></label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3"><asp:TextBox ID="txtSourcePlanningResumeText" Width="95%" runat="server" TextMode="MultiLine" Rows="8" ReadOnly=true /></td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>

    <dx:ASPxPopupControl ID="pcSOAPCopy" runat="server" ClientInstanceName="pcSOAPCopy"
        EnableHierarchyRecreation="True" FooterText="" HeaderText="Copy SOAP From Previous Visit" HeaderStyle-HorizontalAlign="Left" Height="250px"
        Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        Width="400px" CloseAction="CloseButton" AllowDragging="true">
        <ClientSideEvents Closing="function(s,e){ onClosingCopyPopupDialog(); }"/>
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                <div style="width: 100%;">
                    <dxcp:ASPxCallbackPanel ID="cbpCopyProcess" runat="server" Width="100%" ClientInstanceName="cbpCopyProcess"
                        ShowLoadingPanelImage="true">
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent111" runat="server">
                                <asp:Panel ID="Panel1" Style="width: 100%;" runat="server" Height="280px">
                                    <table class="tblContentArea">
                                        <tr>
                                            <td>
                                                <h4 style="background-color:transparent;color:red;font-weight:bold"><%=GetLabel("Silahkan pilih opsi yang akan disalin :")%></h4>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="padding:5px;vertical-align:top">
                                                <table class="tblEntryContent" style="width:100%">
                                                    <tr>
                                                        <td><asp:CheckBox runat="server" id="chkIsCopyChiefComplaint" Text= " Anamnesa" /></td>
                                                    </tr>
                                                    <tr style="display: none">
                                                        <td><asp:CheckBox runat="server" id="chkIsCopyHPI" Text= " Riwayat Penyakit Sekarang" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td><asp:CheckBox runat="server" id="chkIsCopyPastMedicalHistory" Text= " Riwayat Penyakit Dahulu" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td><asp:CheckBox runat="server" id="chkIsCopyPastMedicationHistory" Text= " Riwayat Penggunaan Obat" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td><asp:CheckBox runat="server" id="chkIsCopyVitalSign" Text= " Tanda Vital" /></td>  
                                                    </tr>
                                                    <tr>
                                                        <td><asp:CheckBox runat="server" id="chkIsCopyROS" Text= " Pemeriksaan Fisik" /></td>  
                                                    </tr>
                                                    <tr>
                                                        <td><asp:CheckBox runat="server" id="chkIsCopyDiagnosticResultSummary" Text= " Catatan Hasil Pemeriksaan Penunjang" /></td>  
                                                    </tr>
                                                    <tr>
                                                        <td><asp:CheckBox runat="server" id="chkIsCopyDiagnosis" Text= " Diagnosis" /></td>  
                                                    </tr>
                                                    <tr>
                                                        <td style="padding-left:30px"><asp:CheckBox runat="server" id="chkIsCopyEarlyDiagnose" Text= " Termasuk *Diagnosis Awal/Masuk* ?" /></td>  
                                                    </tr>
                                                    <tr>
                                                        <td><asp:CheckBox runat="server" id="chkIsCopyInstruction" Text= " Instruksi Dokter" /></td>  
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                    <div style="padding:5px;">
                                        <table cellpadding="0" cellspacing="10" style="margin-left:auto;margin-right:auto;text-align:left">
                                            <tr>
                                                <td><input type="button" value='<%= GetLabel("COPY")%>' onclick="onCopyFromPreviousVisit();" style="width:100px;" class="w3-btn w3-hover-blue" /></td>
                                                <td><input type="button" value='<%= GetLabel("CANCEL")%>' onclick="pcSOAPCopy.Hide();" style="width:100px;" class="w3-btn w3-hover-blue" /></td>
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
