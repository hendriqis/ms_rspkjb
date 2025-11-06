<%@ Page Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageList.master"
    AutoEventWireup="true" CodeBehind="MedicalResumeEntry1.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.MedicalResumeEntry1" %>

<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionPage/TransactionPageToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnBackToList" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/back.png")%>' alt="" /><div>
            <%=GetLabel("Back to List")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" id="dxss_erpatientstatus1">
        $(function () {
            $('#<%=btnBackToList.ClientID %>').click(function () {
                showLoadingPanel();
                document.location = document.referrer;
            });

            //#region Left Navigation Panel
            $('#leftPageNavPanel ul li').click(function () {
                $('#leftPageNavPanel ul li.selected').removeClass('selected');
                $(this).addClass('selected');
                var contentID = $(this).attr('contentID');

                showContent(contentID);
            });

            function showContent(contentID) {
                var i, x, tablinks;
                x = document.getElementsByClassName("divPageNavPanelContent");
                for (i = 0; i < x.length; i++) {
                    x[i].style.display = "none";
                }
                document.getElementById(contentID).style.display = "block";
             }
             //#endregion

            setDatePicker('<%=txtResumeDate.ClientID %>');
            $('#<%=txtResumeDate.ClientID %>').datepicker('option', 'maxDate', '0');

            setDatePicker('<%=txtDateOfDeath.ClientID %>');
            $('#<%=txtDateOfDeath.ClientID %>').datepicker('option', 'maxDate', '0');

            setDatePicker('<%=txtPlanFollowUpVisitDate.ClientID %>');
            $('#<%=txtPlanFollowUpVisitDate.ClientID %>').datepicker('option', 'minDate', '0');

            $('#<%:txtSubjectiveResumeText.ClientID %>').focus();

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

            $('#<%=txtSubjectiveResumeText.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });

            $('#<%:txtSubjectiveResumeText.ClientID %>').blur(function () {
                ontxtSubjectiveResumeTextChanged($(this).val());
            });

            $('#<%=txtSubjectiveResumeText.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });

            $('#<%:txtSubjectiveResumeText.ClientID %>').blur(function () {
                onTxtHPIChanged($(this).val());
            });

            $('#<%=txtMedicalHistory.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });

            $('#<%=txtMedicationResumeText.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });

            registerCollapseExpandHandler();
            onCboPatientOutcomeChanged();
            onCboDischargeRoutineChanged();

            $('#leftPageNavPanel ul li').first().click();
        });

        //#region Chief Complaint
        function ontxtSubjectiveResumeTextChanged(value) {
            if (value.length <= 6 && value.slice(-1) == ";") {
                var filterExpression = " UserID = " + <%=HttpUtility.HtmlEncode(GetUserID())%> + " AND TemplateCode = '" + value.slice(0, -1) + "' AND GCTextTemplateGroup ='X058^01'";
                Methods.getObject('GetvPhysicianTextList', filterExpression, function (obj) {
                    if (obj != null)
                        if ($('#<%=txtSubjectiveResumeText.ClientID %>').val() != '') {
                            var message = "Are you sure to replace the Chief Complaint Text from your template text ?";
                            showToastConfirmation(message, function (result) {
                                if (result) {
                                    $('#<%=txtSubjectiveResumeText.ClientID %>').val(obj.TemplateText);
                                }
                            });
                        }
                });
            }
        }
        //#endregion

        //#region History of Present Illness
        function onTxtHPIChanged(value) {
            if (value.length <= 6 && value.slice(-1) == ";") {
                var filterExpression = " UserID = " + <%=HttpUtility.HtmlEncode(GetUserID())%> + " AND TemplateCode = '" + value.slice(0, -1) + "' AND GCTextTemplateGroup ='X058^02'";
                Methods.getObject('GetvPhysicianTextList', filterExpression, function (obj) {
                    if (obj != null)
                        if ($('#<%=txtSubjectiveResumeText.ClientID %>').val() != '') {
                            var message = "Ganti catatan di Riwayat Penyakit sekarang dengan teks dari template ? ?";
                            showToastConfirmation(message, function (result) {
                                if (result) {
                                    $('#<%=txtSubjectiveResumeText.ClientID %>').val(obj.TemplateText);
                                }
                            });
                        }
                });
            }
        }
        //#endregion

        //#region Vital Sign
        var pageCount = parseInt('<%=gridVitalSignPageCount %>');
        $(function () {
            setPaging($("#vitalSignPaging"), pageCount, function (page) {
                cbpVitalSignView.PerformCallback('changepage|' + page);
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
        //#endregion

        //#region Diagnosis
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
        }

        function ResetDiagnosisEntryControls(s) {
            if ($('#<%=hdnIsMainDiagnosisExists.ClientID %>').val() == "0")
                cboDiagnosisType.SetValue(Constant.DiagnosisType.MAIN_DIAGNOSIS);
            else
                cboDiagnosisType.SetValue(Constant.DiagnosisType.COMPLICATION);

            cboDiagnosisStatus.SetValue(Constant.DiagnosisStatus.UNDER_INVESTIGATION);
            ledDiagnose.SetValue('');
        }

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

        //#region Pemeriksaan Fisik
        function onSearchPatientVisitNote(value) {
            var filterExpression = "ID = " + value;
            Methods.getObject('GetPatientVisitNoteList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=txtObjectiveResumeText.ClientID %>').val(result.ObjectiveText);
                }
                else {
                    $('#<%=txtObjectiveResumeText.ClientID %>').val('');
                }
            });
        }
        //#endregion

        function onCboPatientOutcomeChanged() {
            if (cboPatientOutcome.GetValue() != null && (cboPatientOutcome.GetValue() == Constant.PatientOutcome.DEAD_BEFORE_48 || cboPatientOutcome.GetValue() == Constant.PatientOutcome.DEAD_AFTER_48)) {
                $('#trDeathInfo').removeAttr('style');
                cboPatientOutcome.SetEnabled(false);
            }
            else {
                $('#trDeathInfo').attr('style', 'display:none');
                cboPatientOutcome.SetEnabled(false);
            }
        }

        function onCboDischargeReasonValueChanged() {
            if (cboDischargeReason.GetValue() == Constant.DischargeReasonToOtherHospital.OTHER) {
                $('#trDischargeOtherReason').removeAttr('style');
                cboDischargeReason.SetEnabled(false);
            }
            else {
                $('#trDischargeOtherReason').attr('style', 'display:none');
                cboDischargeReason.SetEnabled(false);
            }
        }

        function onCboDischargeRoutineChanged() {
            if (cboDischargeRoutine.GetValue() != null && (cboDischargeRoutine.GetValue() == Constant.DischargeRoutine.OTHER_HOSPITAL || cboDischargeRoutine.GetValue() == Constant.DischargeRoutine.REFER_TO_OUTPATIENT)) {
                if (cboDischargeRoutine.GetValue() == Constant.DischargeRoutine.OTHER_HOSPITAL) {
                    $("#tblDischarge tr.trAppointment").hide();
                    $("#tblReferralNotes").hide();
                    $('#trReferrerGroup').removeAttr('style');
                    $('#trReferrer').removeAttr('style');
                    $('#trDischargeReason').removeAttr('style');
                    $('#trDischargeOtherReason').attr('style', 'display:none');
                    cboDischargeRoutine.SetEnabled(false);
                }
                else {
                    $("#tblDischarge tr.trAppointment").show();
                    $("#tblReferralNotes").show();
                    $('#trReferrerGroup').attr('style', 'display:none');
                    $('#trReferrer').attr('style', 'display:none');
                    $('#trDischargeReason').attr('style', 'display:none');
                    $('#trDischargeOtherReason').attr('style', 'display:none');
                    cboDischargeRoutine.SetEnabled(false);
                }
            }
            else {
                $("#tblDischarge tr.trAppointment").hide();
                $("#tblReferralNotes").hide();
                $('#trReferrerGroup').attr('style', 'display:none');
                $('#trReferrer').attr('style', 'display:none');
                $('#trDischargeReason').attr('style', 'display:none');
                $('#trDischargeOtherReason').attr('style', 'display:none');
                cboDischargeRoutine.SetEnabled(false);
            }
        }

        //#region Referrer
        function onCboReferrerGroupValueChanged() {
            if ($('#<%=hdnGCReferrerGroup.ClientID %>').val() != cboReferrerGroup.GetValue()) {
                $('#<%=hdnReferrerID.ClientID %>').val('');
                $('#<%=txtReferrerCode.ClientID %>').val('');
                $('#<%=txtReferrerName.ClientID %>').val('');    
            }
            $('#<%=hdnGCReferrerGroup.ClientID %>').val(cboReferrerGroup.GetValue());
            return "GCReferrerGroup = '" + cboReferrerGroup.GetValue() + "'";
        }

        $('#<%=txtReferrerCode.ClientID %>').live('change', function () {
            onTxtReferrerCodeChanged($(this).val());
        });

        function onTxtReferrerCodeChanged(value) {
            var filterExpression = onCboReferrerGroupValueChanged() + " AND BusinessPartnerCode = '" + value + "' AND IsDeleted = 0";
            Methods.getObject('GetvReferrerList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnReferrerID.ClientID %>').val(result.BusinessPartnerID);
                    $('#<%=txtReferrerCode.ClientID %>').val(result.BusinessPartnerCode);
                    $('#<%=txtReferrerName.ClientID %>').val(result.BusinessPartnerName);
                }
                else {
                    $('#<%=hdnReferrerID.ClientID %>').val('');
                    $('#<%=txtReferrerCode.ClientID %>').val('');
                    $('#<%=txtReferrerName.ClientID %>').val('');
                }
            });
        }
        //#endregion
		
        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
            var visitID = $('#<%=hdnVisitID.ClientID %>').val();
            var hdnID = $('#<%=hdnMedicalResumeID.ClientID %>').val();

            if (hdnID == '' || hdnID == '0') {
                if (code == 'PM-00425' || code == 'MR000016' || code == 'PM-00160' || code == 'PM-00524') {
                    filterExpression.text = registrationID;
                    return true;
                }
                else if (code == 'MR000013' || code == 'MR000017') {
                    filterExpression.text = visitID;
                    return true;
                }
                else {
                    errMessage.text = 'Pasien tidak memiliki Catatan Perawat';
                    return false;
                }
            }
            else if (code == 'MR000013' || code == 'MR000014' || code == 'MR000017' || code == 'MR000019' || code == 'PM-90008' || code == 'PM-90009' ||
                     code == 'PM-90010' || code == 'PM-90011' || code == 'PM-90012' || code == 'PM-90013' || code == 'PM-90014' || code == 'PM-90015' ||
                     code == 'PM-90019' || code == 'PM-90020' || code == 'PM-90022' || code == 'PM-90023') {
                filterExpression.text = visitID;
                return true;
            }
            else if (code == 'PM-00425' || code == 'MR000016' || code == 'PM-00522' || code == 'PM-00523' || code == 'PM-00159') {
                filterExpression.text = registrationID;
                return true;
            }
            else {
                filterExpression.text = 'VisitID = ' + visitID;
                return true;
            }
        }

        function onBeforeLoadRightPanelContent(code) {
            var registrationID = $('#<%:hdnRegistrationID.ClientID %>').val();
            if (code == 'healthyinformation' || code == 'medicalSickLeave' || code == 'medicalSickLeaveBilingual') {
                return $('#<%:hdnRegistrationID.ClientID %>').val();
            }
            else {
                return $('#<%:hdnRegistrationID.ClientID %>').val();
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
            gotoNextPage();
        }
        function onBeforeBackToListPage() {
            backToPatientList();
        }
        //#endregion     
                
        function onAfterLookUpDiagnosticTest(param) {
            if ($('#<%=txtPlanningResumeText.ClientID %>').val() != "") {
                var previousText = $('#<%=txtPlanningResumeText.ClientID %>').val();
                var newText = previousText + "\n"+param;
                $('#<%=txtPlanningResumeText.ClientID %>').val(newText);
            }
            else
            {
                $('#<%=txtPlanningResumeText.ClientID %>').val(param);
            }
        }

        function onAfterLookUpSurgeryProcedure(param) {
            if ($('#<%=txtSurgeryResumeText.ClientID %>').val() != "") {
                var previousText = $('#<%=txtSurgeryResumeText.ClientID %>').val();
                var newText = previousText + "\n"+param;
                $('#<%=txtSurgeryResumeText.ClientID %>').val(newText);
            }
            else
            {
                $('#<%=txtSurgeryResumeText.ClientID %>').val(param);
            }
        }

        function onAfterLookUpEpisodePrescription(param) {
            if ($('#<%=txtMedicationResumeText.ClientID %>').val() != "") {
                var message1 = "Ganti catatan yang sudah dientry di Ringkasan Terapi Obat Selama Perawatan ?";
                var message2 = "<i>"+param+"</i>";
                displayConfirmationMessageBox('Terapi Obat Selama Perawatan', message1+"<br/><br/>"+message2, function (result) {
                    if (result) {
                        $('#<%=txtMedicationResumeText.ClientID %>').val(param);
                    }
                });
            }
            else
            {
                $('#<%=txtMedicationResumeText.ClientID %>').val(param);
            }
        } 
        
        function onAfterLookUpDischargePrescription(param) {
            if ($('#<%=txtDischargeMedicationResumeText.ClientID %>').val() != "") {
                var message1 = "Ganti catatan yang sudah dientry di Ringkasan Terapi Obat Pulang ?";
                var message2 = "<i>"+param+"</i>";
                displayConfirmationMessageBox('Terapi Obat Pulang', message1+"<br/><br/>"+message2, function (result) {
                    if (result) {
                        $('#<%=txtDischargeMedicationResumeText.ClientID %>').val(param);
                    }
                });
            }
            else
            {
                $('#<%=txtDischargeMedicationResumeText.ClientID %>').val(param);
            }
        } 

        function onAfterSelectFromInstructionTemplate(param) {
            if ($('#<%=txtInstructionResumeText.ClientID %>').val() != "") {
                var message1 = "Ganti catatan yang sudah dientry di Instruksi dan Rencana Tindak Lanjut ?";
                var message2 = "<i>"+param+"</i>";
                displayConfirmationMessageBox('Instruksi dan Rencana Tindak Lanjut', message1+"<br/><br/>"+message2, function (result) {
                    if (result) {
                        $('#<%=txtInstructionResumeText.ClientID %>').val(param);
                    }
                });
            }
            else
            {
                $('#<%=txtInstructionResumeText.ClientID %>').val(param);
            }
        } 
        
        function onAfterCustomClickSuccess(type, retval) {
            if ($('#<%=hdnMedicalResumeID.ClientID %>').val() == '' || $('#<%=hdnMedicalResumeID.ClientID %>').val() == '0') {
                $('#<%=hdnMedicalResumeID.ClientID %>').val(retval);
            }
        }                                   
    </script>
    <div>
        <input type="hidden" value="" id="hdnPageTitle" runat="server" />
        <input type="hidden" id="hdnRegistrationID" value="" runat="server" />
        <input type="hidden" id="hdnVisitID" value="" runat="server" />
        <input type="hidden" id="hdnLinkedVisitID" value="" runat="server" />
        <input type="hidden" runat="server" id="hdnMedicalResumeID" value="0" />
        <input type="hidden" value="" id="hdnVitalSignRecordID" runat="server" />
        <input type="hidden" value="" id="hdnMenuType" runat="server" />
        <input type="hidden" value="" id="hdnDeptType" runat="server" />
        <input type="hidden" value="" id="hdnDiagnosisID" runat="server" />
        <input type="hidden" value="1" id="hdnDiagnosisProcessMode" runat="server" />
        <input type="hidden" value="" id="hdnReviewOfSystemID" runat="server" />
        <input type="hidden" value="" id="hdnImagingServiceUnitID" runat="server" />
        <input type="hidden" value="" id="hdnLaboratoryServiceUnitID" runat="server" />
        <input type="hidden" value="" id="hdnTestOrderHealthcareServiceUnitID" runat="server" />
        <input type="hidden" value="" id="hdnLaboratoryTestOrderID" runat="server" />
        <input type="hidden" value="" id="hdnImagingTestOrderID" runat="server" />
        <input type="hidden" value="" id="hdnDiagnosticTestOrderID" runat="server" />
        <input type="hidden" value="" id="hdnPrescriptionOrderID" runat="server" />
        <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
        <input type="hidden" runat="server" id="hdnPastMedicalID" value="" />
        <input type="hidden" runat="server" id="hdnTimeElapsed0hour" value="00" />
        <input type="hidden" runat="server" id="hdnTimeElapsed0minute" value="00" />
        <input type="hidden" runat="server" id="hdnTimeElapsed1hour" value="00" />
        <input type="hidden" runat="server" id="hdnTimeElapsed1minute" value="00" />
        <input type="hidden" runat="server" id="hdnIsSaved" value="0" />
        <input type="hidden" runat="server" id="hdnIsChanged" value="0" />
        <input type="hidden" id="hdnPageCount" runat="server" value='0' />
        <input type="hidden" id="hdnPageIndex" runat="server" value='0' />
        <input type="hidden" id="hdnIsMainDiagnosisExists" runat="server" value='0' />
        <input type="hidden" value="1" id="hdnInstructionProcessMode" runat="server" />
        <input type="hidden" value="" id="hdnInstructionID" runat="server" />
        <input type="hidden" value="0" id="hdnPatientVisitNoteID" runat="server" />
        <input type="hidden" value="" id="hdnSubjectiveText" runat="server" />
        <input type="hidden" value="" id="hdnObjectiveText" runat="server" />
        <input type="hidden" value="" id="hdnAssessmentText" runat="server" />
        <input type="hidden" value="" id="hdnPlanningText" runat="server" />
        <input type="hidden" value="" id="hdnInstructionText" runat="server" />
        <input type="hidden" value="" id="hdnLaboratorySummary" runat="server" />
        <input type="hidden" value="" id="hdnImagingSummary" runat="server" />
        <input type="hidden" value="" id="hdnDiagnosticSummary" runat="server" />
        <input type="hidden" id="hdnDiagnosisSummary" runat="server" />
        <input type="hidden" id="hdnPatientInformation" runat="server" />
        <input type="hidden" id="hdnParamedicID" runat="server" />
        <input type="hidden" id="hdnGCReferrerGroup" value="" runat="server" />        
        <input type="hidden" runat="server" id="hdnVisitNoteID" value="0" />
        <fieldset id="mpPatientStatus">
            <table class="tblContentArea">
                <colgroup>
                    <col style="width: 20%" />
                    <col style="width: 80%" />
                </colgroup>
                <tr>
                <td style="vertical-align:top">
                    <div id="leftPageNavPanel" class="w3-border">
                        <ul>
                            <li contentID="divPage1" title="Ringkasan Riwayat Penyakit" class="w3-hover-red">Ringkasan Riwayat Penyakit</li>
                            <li contentID="divPage2" title="Pemeriksaan Fisik" class="w3-hover-red">Pemeriksaan Fisik</li>  
                            <li contentID="divPage3" title="Tanda Vital dan Indikator Lainnya" class="w3-hover-red">Tanda Vital dan Indikator Lainnya</li>                                                   
                            <li contentID="divPage4" title="Pemeriksaan Penunjang" class="w3-hover-red">Pemeriksaan Penunjang</li>
                            <li contentID="divPage5" title="Diagnosa" class="w3-hover-red">Diagnosa</li>
                            <li contentID="divPage6" title="Ringkasan Terapi Pengobatan" class="w3-hover-red">Ringkasan Terapi Pengobatan</li>
                            <li contentID="divPage7" title="Perkembangan selama Perawatan" class="w3-hover-red">Perkembangan selama Perawatan</li>
                            <li contentID="divPage8" title="Prosedur Terapi dan Tindakan" class="w3-hover-red">Prosedur Terapi dan Tindakan</li>
                            <li contentID="divPage9" title="Kondisi dan Cara Pulang" class="w3-hover-red">Kondisi dan Cara Pulang</li>
                            <li contentID="divPage10" title="Instruksi dan Rencana Tindak Lanjut" class="w3-hover-red">Instruksi dan Rencana Tindak Lanjut</li>
                        </ul>     
                    </div> 
                </td>
                    <td style="vertical-align:top">
                        <div id="divPage1" class="w3-border divPageNavPanelContent w3-animate-left" style="display:none"> 
                            <table class="tblEntryContent" style="width: 100%">
                                <colgroup>
                                    <col style="width: 220px" />
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
                                                    <asp:TextBox ID="txtResumeDate" Width="120px" CssClass="datepicker" runat="server" Enabled="false" />
                                                </td>
                                                <td style="padding-left: 5px">
                                                    <asp:TextBox ID="txtResumeTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" MaxLength="5" Enabled="false" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel" valign="top">
                                        <table border="0" cellpadding="0" cellspacing="1">
                                            <tr>
                                                <td class="tdLabel" valign="top" style="width:190px">
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("Indikasi Rawat Inap")%></label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtHospitalIndication" runat="server" TextMode="MultiLine" Rows="2" Width="100%" ReadOnly/>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel" style="width: 150px; vertical-align: top">
                                        <table border="0" cellpadding="0" cellspacing="1">
                                            <tr>
                                                <td class="tdLabel" valign="top" style="width:190px">
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("Anamnesa/Riwayat Penyakit")%></label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSubjectiveResumeText" runat="server" Width="100%" TextMode="Multiline"
                                            Rows="8" ReadOnly/>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel" valign="top">
                                        <table border="0" cellpadding="0" cellspacing="1">
                                            <tr>
                                                <td class="tdLabel" valign="top" style="width:190px">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Komorbiditas")%></label>
                                                </td>
                                             </tr>
                                        </table>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtComorbiditiesText" runat="server" TextMode="MultiLine" Rows="2" Width="100%" ReadOnly />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divPage2" class="w3-border divPageNavPanelContent w3-animate-left" style="display:none"> 
                            <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 5px">
                                <colgroup>
                                    <col width="150px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td colspan="2">
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
                                                                    <asp:Panel runat="server" ID="Panel3" CssClass="pnlContainerGridPatientPage5" Style="height: 300px">
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
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                                                    <HeaderTemplate>
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
                                                                                                        <span <%# Eval("IsNormal").ToString() == "False" && Eval("IsNotExamined").ToString() == "False" ? "Style='color:red;font-style:italic'":"" %>>
                                                                                                            <strong>
                                                                                                                <%#: DataBinder.Eval(Container.DataItem, "ROSystem") %>
                                                                                                                : </strong></span>&nbsp; <span <%# Eval("IsNormal").ToString() == "False" && Eval("IsNotExamined").ToString() == "False" ? "Style='color:red;font-style:italic'":"" %>>
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
                                                                                <%=GetLabel("Belum ada pemeriksaan fisik untuk resume medis ini") %>
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
                                                                <div id="rosPaging" style="display:none">
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                        <label class="lblNormal">
                                            <%=GetLabel("Catatan Tambahan Pemeriksaan Fisik") %></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtObjectiveResumeText" runat="server" Width="98%" TextMode="Multiline"
                                            Rows="5" ReadOnly />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divPage3" class="w3-border divPageNavPanelContent w3-animate-left" style="display:none"> 
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
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                                        <HeaderTemplate>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <div>
                                                                                <b>
                                                                                    <%#: Eval("ObservationDateInString")%>,
                                                                                    <%#: Eval("ObservationTime") %>,
                                                                                    <%#: Eval("ParamedicName") %>
                                                                                </b>
                                                                                <br />
                                                                                <span style="font-style:italic">
                                                                                    <%#: Eval("Remarks") %>
                                                                                </span>
                                                                                <br />
                                                                            </div>
                                                                            <div>
                                                                                <asp:Repeater ID="rptVitalSignDt" runat="server">
                                                                                    <ItemTemplate>
                                                                                        <div style="padding-left: 20px; float: left; width: 350px;">
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
                                                                    <%=GetLabel("Belum ada pemeriksaan tanda vital untuk resume medis ini") %>
                                                                </EmptyDataTemplate>
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </dx:PanelContent>
                                                </PanelCollection>
                                            </dxcp:ASPxCallbackPanel>
                                            <div class="containerPaging">
                                                <div class="wrapperPaging">
                                                    <div id="vitalSignPaging" style="display:none">
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divPage4" class="w3-border divPageNavPanelContent w3-animate-left" style="display:none"> 
                            <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                                <colgroup>
                                    <col width="200px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                        <table border="0" cellpadding="1" cellspacing="0">
                                            <tr>
                                                <td style="text-align:left">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Catatan Pemeriksaan Penunjang") %></label>
                                                </td>

                                            </tr>
                                            <tr>
                                                <td style="text-align:right">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Laboratorium") %></label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align:right">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Radiologi") %></label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align:right">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Lain-lain") %></label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPlanningResumeText" runat="server" Width="98%" TextMode="Multiline"
                                            Rows="15" ReadOnly/>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divPage5" class="w3-border divPageNavPanelContent w3-animate-left" style="display:none"> 
                            <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px;">
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
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20px">
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                                        <HeaderTemplate>
                                                                            <%=GetLabel("Diagnose Information")%>
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
                        <div id="divPage6" class="w3-border divPageNavPanelContent w3-animate-left" style="display:none"> 
                            <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                                <colgroup>
                                    <col width="150px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                        <label class="lblNormal">
                                            <%=GetLabel("Ringkasan Terapi Pengobatan") %></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMedicationResumeText" runat="server" Width="98%" TextMode="Multiline"
                                            Rows="6" ReadOnly />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                        <label class="lblNormal">
                                            <%=GetLabel("Terapi Obat Pulang") %></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDischargeMedicationResumeText" runat="server" Width="98%" TextMode="Multiline"
                                            Rows="4" ReadOnly />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divPage7" class="w3-border divPageNavPanelContent w3-animate-left" style="display:none"> 
                            <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 5px">
                                <colgroup>
                                    <col width="150px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                        <%=GetLabel("Perkembangan Selama Perawatan") %>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMedicalResumeText" runat="server" Width="98%" TextMode="Multiline"
                                            Rows="15" ReadOnly />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divPage8" class="w3-border divPageNavPanelContent w3-animate-left" style="display:none"> 
                            <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                                <colgroup>
                                    <col width="200px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                        <table border="0" cellpadding="1" cellspacing="0">
                                            <tr>
                                                <td style="text-align:left">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Prosedur Terapi dan Tindakan") %></label>
                                                </td>

                                            </tr>
                                            <tr>
                                                <td style="text-align:right">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Kamar Operasi") %></label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSurgeryResumeText" runat="server" Width="98%" TextMode="Multiline"
                                            Rows="18" ReadOnly />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divPage9" class="w3-border divPageNavPanelContent w3-animate-left" style="display:none">
                            <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                                <colgroup>
                                    <col width="190px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                        <%=GetLabel("Kondisi Klinis saat Pulang") %>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDischargeMedicalSummary" runat="server" Width="98%" TextMode="Multiline"
                                            Rows="10" ReadOnly />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblMandatory">
                                            <%=GetLabel("Keadaan Keluar")%></label>
                                    </td>
                                    <td>
                                        <dxe:ASPxComboBox ID="cboPatientOutcome" ClientInstanceName="cboPatientOutcome" Width="350px"
                                            runat="server">
                                            <ClientSideEvents ValueChanged="function(s,e){ onCboPatientOutcomeChanged(); }" />
                                        </dxe:ASPxComboBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblMandatory">
                                            <%=GetLabel("Cara Keluar")%></label>
                                    </td>
                                    <td>
                                        <dxe:ASPxComboBox ID="cboDischargeRoutine" ClientInstanceName="cboDischargeRoutine"
                                            Width="350px" runat="server">
                                            <ClientSideEvents ValueChanged="function(s,e){ onCboDischargeRoutineChanged(); }" />
                                        </dxe:ASPxComboBox>
                                    </td>
                                </tr>
                                <tr id="trDeathInfo" style="display: none">
                                    <td class="tdLabel">
                                        <label class="lblMandatory">
                                            <%=GetLabel("Tanggal dan Jam Meninggal")%></label>
                                    </td>
                                    <td>
                                        <table border="0" cellpadding="1" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtDateOfDeath" CssClass="datepicker" Width="120px" ReadOnly/>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtTimeOfDeath" CssClass="time" Width="80px" ReadOnly/>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr id="trReferrerGroup" style="display: none">
                                    <td class="tdLabel">
                                        <label class="lblMandatory">
                                            <%=GetLabel("Rujuk Ke")%></label>
                                    </td>
                                    <td colspan="2">
                                        <dxe:ASPxComboBox ID="cboReferrerGroup" Width="100%" runat="server" Enabled="false" ClientInstanceName="cboReferrerGroup">
                                            <ClientSideEvents Init="function(s,e){ onCboReferrerGroupValueChanged(s); }" ValueChanged="function(s,e){ onCboReferrerGroupValueChanged(); }" />
                                        </dxe:ASPxComboBox>
                                    </td>
                                </tr>
                                <tr id="trReferrer" style="display: none">
                                    <td class="tdLabel">
                                        <label class="lblNormal" runat="server">
                                            <%:GetLabel("Rumah Sakit / Faskes")%></label>
                                    </td>
                                    <td colspan="5">
                                        <table border="0" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <input type="hidden" id="hdnReferrerID" value="" runat="server" />
                                                    <asp:TextBox ID="txtReferrerCode" Width="150px" runat="server" ReadOnly />
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtReferrerName" Width="330px" ReadOnly="true" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr id="trDischargeReason" style="display: none">
                                    <td class="tdLabel">
                                        <label class="lblMandatory">
                                            <%=GetLabel("Alasan Pasien Dirujuk")%></label>
                                    </td>
                                    <td colspan="2">
                                        <dxe:ASPxComboBox ID="cboDischargeReason" Width="100%" runat="server" ClientInstanceName="cboDischargeReason">
                                            <ClientSideEvents Init="function(s,e){ onCboDischargeReasonValueChanged(s); }" ValueChanged="function(s,e){ onCboDischargeReasonValueChanged(); }" />
                                        </dxe:ASPxComboBox>
                                    </td>
                                </tr>
                                <tr id="trDischargeOtherReason" style="display: none">
                                    <td>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtDischargeOtherReason" Width="100%" runat="server" ReadOnly/>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divPage10" class="w3-border divPageNavPanelContent w3-animate-left" style="display:none">
                            <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                                <colgroup>
                                    <col width="150px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel"><label><%=GetLabel("Surat Keterangan Sakit ")%></label></td>
                                    <td>
                                        <table border="0" cellpadding="0" cellspacing="1">
                                            <tr>
                                                <td>
                                                    <asp:RadioButtonList ID="rblIsHasSickLetter" runat="server" RepeatDirection="Horizontal" Enabled="false">
                                                        <asp:ListItem Text=" Tidak" Value="0" Selected="True" />
                                                        <asp:ListItem Text=" Ya" Value="1"  />
                                                    </asp:RadioButtonList>
                                                </td>
                                                <td><asp:TextBox ID="txtNoOfDays" Width="60px" runat="server" CssClass="number" ReadOnly/></td>
                                                <td class="tdLabel"><label><%=GetLabel("hari ")%></label></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel"><label><%=GetLabel("Rencana Kontrol Kembali ")%></label></td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtPlanFollowUpVisitDate" CssClass="datepicker" Width="120px" ReadOnly />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                        <label class="lblNormal">
                                            <%=GetLabel("Instruksi dan Rencana Tindak Lanjut") %></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtInstructionResumeText" runat="server" Width="98%" TextMode="Multiline"
                                            Rows="10" ReadOnly />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
            <table class="tblContentArea" style="display:none">
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
                                                 
                        </table>
                    </td>
                    <td style="vertical-align: top">
                        <h4 class="h4collapsed">
                            <%=GetLabel("Riwayat Penyakit Dahulu")%></h4>
                        <div class="containerTblEntryContent">
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
                            <%=GetLabel("Diagnosa")%></h4>
                        <div class="containerTblEntryContent containerEntryPanel1">
                        </div>
                    </td>
                </tr>
            </table>
        </fieldset>
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
            <dxcp:ASPxCallbackPanel ID="cbpDiagnosis" runat="server" Width="100%" ClientInstanceName="cbpDiagnosis"
                ShowLoadingPanel="false" OnCallback="cbpDiagnosis_Callback">
                <ClientSideEvents EndCallback="function(s,e){ onCbpDiagnosisEndCallback(s); }" />
            </dxcp:ASPxCallbackPanel>
        </div>
    </div>
</asp:Content>
