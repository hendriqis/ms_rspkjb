<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MPPatientPageList.master"
    AutoEventWireup="true" CodeBehind="MedicalResumeEntry2.aspx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.MedicalResumeEntry2" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<asp:Content ID="Content4" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnBackToList" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/back.png")%>' alt="" /><div>
            <%=GetLabel("Back to List")%></div>
    </li>
    <li id="btnGenerate" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><div>
            <%=GetLabel("Tarik Data")%></div>
    </li>
    <li id="btnPatientStatusSave" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbsave.png")%>' alt="" /><div>
            <%=GetLabel("Save")%></div>
    </li>
    <li id="btnDiscardChanges" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbcancel.png")%>' alt="" /><div>
            <%=GetLabel("Batal Perubahan")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhRightToolbar" runat="server">
    <table cellpadding="0" cellspacing="0" style="width: 100%">
        <colgroup>
            <col />
        </colgroup>
        <tr>
            <td>
                <div class="menuTitle">
                    Resume Medis</div>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" id="dxss_erpatientstatus1">
        $(function () {
            $('#<%=btnBackToList.ClientID %>').click(function () {
                if ($('#<%=hdnIsSaved.ClientID %>').val() == '0' && $('#<%=hdnIsChanged.ClientID %>').val() == '1') {
                    PromptUserToSave();
                }
                else {
                    if ($('#<%=txtSubjectiveResumeText.ClientID %>').val() != '') {
                        $('#<%=hdnIsChanged.ClientID %>').val('0');
                        showLoadingPanel();
                        document.location = document.referrer;
                    }
                    else {
                        displayErrorMessageBox('Resume Medis', 'Anamnesa/Riwayat Penyakit Harus diisi.'); 
                    }
                }
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
            $('#<%=txtResumeDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

            $('#<%:txtSubjectiveResumeText.ClientID %>').focus();


            $('#<%=btnPatientStatusSave.ClientID %>').click(function (evt) {
                if (IsValid(evt, 'fsPatientStatus', 'mpPatientStatus')) {
                    if (validateTime($('#<%=txtResumeTime.ClientID %>').val())) {
                        $('#<%=hdnIsChanged.ClientID %>').val('0');
                            if ($('#<%=txtSubjectiveResumeText.ClientID %>').val() != '') {
                                onCustomButtonClick('save');
                                $('#<%=hdnIsChanged.ClientID %>').val('0');
                            }
                            else {
                                displayErrorMessageBox('Resume Medis', 'Anamnesa/Riwayat Penyakit Harus diisi.'); 
                            }
                    }
                    else {
                        displayErrorMessageBox('Resume Medis', 'Format Waktu yang diinput salah');                      
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

            $('#btnAddTemplate.imgLink').click(function () {
                if ($('#<%=txtSubjectiveResumeText.ClientID %>').val() != '') {
                    onBeforeOpenTrxPopup();
                    var text = $('#<%=txtSubjectiveResumeText.ClientID %>').val();
                    openUserControlPopup("~/Program/PatientPage/_PopupEntry/TemplateText/UpdatePhysicianTextCtl1.ascx", "X058^01|" + text, "Physician Template Text", 700, 500);
                }
            });

            $('#btnSubjectiveResumeTest.imgLink').click(function () {
                if ($('#<%=txtSubjectiveResumeText.ClientID %>').val() != '') {
                    onBeforeOpenTrxPopup();
                    var text = $('#<%=txtSubjectiveResumeText.ClientID %>').val();
                    openUserControlPopup("~/Program/PatientPage/_PopupEntry/TemplateText/UpdatePhysicianTextCtl1.ascx", "X058^02|" + text, "Physician Template Text", 700, 500);
                }
            });

            $('#<%=btnGenerate.ClientID %>').click(function (evt) {
                var message = "Lakukan penarikan data ulang dari pencatatan yang sudah ada ?";
                displayConfirmationMessageBox("Resume Medis", message, function (result) {
                    if (result) {
                        cbpGenerate.PerformCallback('generate');
                    }
                });
            });

            $('#<%=btnDiscardChanges.ClientID %>').click(function (evt) {
                if ($('#<%=hdnIsChanged.ClientID %>').val() == '1') {
                    var message = "Lakukan batal perubahan yang sudah dilakukan ?";
                    displayConfirmationMessageBox("Resume Medis", message, function (result) {
                        if (result) {
                            $('#<%=hdnIsChanged.ClientID %>').val('0');
                            location.reload();
                        }
                    });
                }
            });

            $('#<%=txtSubjectiveResumeText.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });

            $('#<%:txtSubjectiveResumeText.ClientID %>').blur(function () {
                ontxtSubjectiveResumeTextChanged($(this).val());
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

            $('#<%=txtObjectiveResumeText.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });

            $('#<%=txtPlanningResumeText.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });
            
            $('#<%=txtDischargeMedicationResumeText.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });
            
            $('#<%=txtMedicalResumeText.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });

            $('#<%=txtInstructionResumeText.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });

            registerCollapseExpandHandler();

            $('#leftPageNavPanel ul li').first().click();
        });

        //#region Chief Complaint
        $('#lblIndication').die('click');
        $('#lblIndication').live('click', function (evt) {
            alert("Sorry, this feature is currently in development process (Physician Template Text Lookup)");
        });

        function ontxtSubjectiveResumeTextChanged(value) {
            if (value.length <= 6 && value.slice(-1) == ";") {
                var filterExpression = " UserID = " + <%=HttpUtility.HtmlEncode(GetUserID())%> + " AND TemplateCode = '" + value.slice(0, -1) + "' AND GCTextTemplateGroup ='X058^01'";
                Methods.getObject('GetvPhysicianTextList', filterExpression, function (obj) {
                    if (obj != null)
                        if ($('#<%=txtSubjectiveResumeText.ClientID %>').val() != '') {
                            var message = "Are you sure to replace the Chief Complaint Text from your template text ?";
                            displayConfirmationMessageBox("Resume Medis", message, function (result) {
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
        $('#lblHPI').die('click');
        $('#lblHPI').live('click', function (evt) {
            alert("Sorry, this feature is currently in development process (Physician Template Text Lookup)");
        });

        function onTxtHPIChanged(value) {
            if (value.length <= 6 && value.slice(-1) == ";") {
                var filterExpression = " UserID = " + <%=HttpUtility.HtmlEncode(GetUserID())%> + " AND TemplateCode = '" + value.slice(0, -1) + "' AND GCTextTemplateGroup ='X058^02'";
                Methods.getObject('GetvPhysicianTextList', filterExpression, function (obj) {
                    if (obj != null)
                        if ($('#<%=txtSubjectiveResumeText.ClientID %>').val() != '') {
                            var message = "Ganti catatan di Riwayat Penyakit sekarang dengan teks dari template ? ?";
                            displayConfirmationMessageBox("Resume Medis",message, function (result) {
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
        //#endregion

        //#region Review of System In
        //#endregion

        //#region Review of System
        //#endregion

        //#region Diagnosis

        //#endregion

        //#region Medication

        $('#lblEpisodeMedication').die('click');
        $('#lblEpisodeMedication').live('click', function (evt) {
            var visitID = $('#<%=hdnVisitID.ClientID %>').val();
            if (visitID != '0' && visitID != '') {
                openUserControlPopup("~/Program/PatientPage/_PopupEntry/MedicalResume/EpisodeMedicationPicksCtl1.ascx", visitID, "Terapi selama Perawatan", 1000, 500);
            }
        });

        $('#lblDischargePrescription').die('click');
        $('#lblDischargePrescription').live('click', function (evt) {
            var visitID = $('#<%=hdnVisitID.ClientID %>').val();
            if (visitID != '0' && visitID != '') {
                openUserControlPopup("~/Program/PatientPage/_PopupEntry/MedicalResume/DischargeMedicationPicksCtl1.ascx", visitID, "Resep Pulang", 700, 500);
            }
        });
        //#endregion Medication

        //#region Progress Note
        $('#lblProgressNote').die('click');
        $('#lblProgressNote').live('click', function (evt) {
            var visitID = $('#<%=hdnVisitID.ClientID %>').val();
            var paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            if (visitID != '0' && visitID != '') {
                openUserControlPopup("~/Program/PatientPage/_PopupEntry/MedicalResume/EpisodeProgressNotePicksCtl1.ascx", visitID+"|"+paramedicID, "Riwayat Catatan Perkembangan : CPPT", 700, 500);
            }
        });

        function onAfterLookUpProgressNote(param) {
            if ($('#<%=txtMedicalResumeText.ClientID %>').val() != "") {
                var previousText = $('#<%=txtMedicalResumeText.ClientID %>').val();
                var newText = previousText + "\n"+param;

                var message1 = "Ganti catatan yang sudah dientry di Catatan Perkembangan Pasien ?";
                var message2 = "<i>"+newText+"</i>";
                displayConfirmationMessageBox('Catatan Perkembangan Pasien', message1+"<br/><br/>"+message2, function (result) {
                    if (result) {
                        $('#<%=txtMedicalResumeText.ClientID %>').val(newText);
                    }
                });
            }
            else
            {
                $('#<%=txtMedicalResumeText.ClientID %>').val(param);
            }
        }
        //#endregion Progress Note

        //#region Pemeriksaan Fisik
        $('#lblObjectiveResumeText').die('click');
        $('#lblObjectiveResumeText').live('click', function (evt) {
            var visitID = $('#<%=hdnVisitID.ClientID %>').val();
            var filterExpression = "VisitID = " + $('#<%=hdnVisitID.ClientID %>').val() + " AND ParamedicID = " + $('#<%=hdnParamedicID.ClientID %>').val() + " AND GCPatientNoteType IN ('X011^011') AND ObjectiveText IS NOT NULL";
            openSearchDialog('planningNote', filterExpression, function (value) {
                $('#<%=hdnVisitNoteID.ClientID %>').val(value);
                onSearchPatientVisitNote(value);
            });
        });

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

        //#region Pemeriksaan Penunjang

        $('#lblLaboratory').die('click');
        $('#lblLaboratory').live('click', function (evt) {
            var visitID = $('#<%=hdnVisitID.ClientID %>').val();
            if (visitID != '0' && visitID != '') {
                var param = "X001^004"+"|"+visitID;
                openUserControlPopup("~/Program/PatientPage/_PopupEntry/MedicalResume/EpisodeTestOrderCtl1.ascx", param, "Pemeriksaan Penunjang - Laboratorium", 1000, 500);
            }
        });

        $('#lblImaging').die('click');
        $('#lblImaging').live('click', function (evt) {
            var visitID = $('#<%=hdnVisitID.ClientID %>').val();
            if (visitID != '0' && visitID != '') {
                var param = "X001^005"+"|"+visitID;
                openUserControlPopup("~/Program/PatientPage/_PopupEntry/MedicalResume/EpisodeTestOrderCtl1.ascx", param, "Pemeriksaan Penunjang - Radiologi", 1000, 500);
            }
        });

        $('#lblOtherDiagnostic').die('click');
        $('#lblOtherDiagnostic').live('click', function (evt) {
            var visitID = $('#<%=hdnVisitID.ClientID %>').val();
            if (visitID != '0' && visitID != '') {
                var param = "X001^006"+"|"+visitID;
                openUserControlPopup("~/Program/PatientPage/_PopupEntry/MedicalResume/EpisodeTestOrderCtl1.ascx", param, "Pemeriksaan Penunjang - Lain - lain", 1000, 500);
            }
        });
        //#endregion Pemeriksaan Penunjang


        //#region Prosedur Terapi dan Tindakan

        $('#lblSurgeryProcedure').die('click');
        $('#lblSurgeryProcedure').live('click', function (evt) {
            var visitID = $('#<%=hdnVisitID.ClientID %>').val();
            if (visitID != '0' && visitID != '') {
                var param = visitID;
                openUserControlPopup("~/Program/PatientPage/_PopupEntry/MedicalResume/EpisodeSurgeryProcedureCtl1.ascx", param, "Tindakan Kamar Operasi", 1000, 500);
            }
        });

        //#endregion Prosedur Terapi dan Tindakan


        $('#lblPhysicianInstruction').die('click');
        $('#lblPhysicianInstruction').live('click', function (evt) {
            var visitID = $('#<%=hdnVisitID.ClientID %>').val();
            if (visitID != '0' && visitID != '') {
                openUserControlPopup("~/Program/PatientPage/_PopupEntry/Instruction/PhysicianInstructionQuickPicks1Ctl.ascx", "", "Template Instruksi dan Rencana Tindak Lanjut", 700, 600);
            }
        });


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
            var message = "Terjadi perubahan terhadap resume medis tetapi belum disimpan, Lanjutkan dengan proses simpan ?";
            displayConfirmationMessageBox("Resume Medis", message, function (result) {
                if (result) {
                    onCustomButtonClick('save');
                    $('#<%=hdnIsChanged.ClientID %>').val('0');
                }
            });
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
            $('#<%=hdnMedicalResumeID.ClientID %>').val(retval);
        }                                      

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
            var visitID = $('#<%=hdnVisitID.ClientID %>').val();

            if (code == 'MR000013' || code == 'MR000014' || code == 'MR000017' || code == 'MR000019' || code == 'PM-90008' || code == 'PM-90009' ||
                code == 'PM-90010' || code == 'PM-90011' || code == 'PM-90012' || code == 'PM-90013' || code == 'PM-90014' || code == 'PM-90015' || 
                code == 'PM-90019' || code == 'PM-90020' || code == 'PM-90022' || code == 'PM-90023' || code == "PM-90056" || code == "PM-90085" ||
                code == 'PM-90117' || code == 'PM-90118') {
                filterExpression.text = visitID;
                return true;
            }
            else {
                filterExpression.text = 'VisitID = ' + visitID;
                return true;
            }
        }

        function onCbpGenerateEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[1] == '1') {
               $('#<%=txtSubjectiveResumeText.ClientID %>').val(param[2]);
               $('#<%=txtObjectiveResumeText.ClientID %>').val(param[3]);
               $('#<%=txtAssessmentResumeText.ClientID %>').val(param[4]);               
               $('#<%=txtPlanningResumeText.ClientID %>').val(param[5]);
               $('#<%=txtMedicationResumeText.ClientID %>').val(param[6]);
               $('#<%=txtMedicalResumeText.ClientID %>').val(param[7]);
               $('#<%=txtInstructionResumeText.ClientID %>').val(param[8]);
               $('#<%=hdnIsChanged.ClientID %>').val('1');
            }
            else {
                displayErrorMessageBox("Resume Medis", 'Error Message : ' + param[1]);
            }
        }
    </script>
    <div>
        <input type="hidden" id="hdnRegistrationID" value="" runat="server" />
        <input type="hidden" id="hdnVisitID" value="" runat="server" />
        <input type="hidden" id="hdnLinkedVisitID" value="" runat="server" />
        <input type="hidden" runat="server" id="hdnMedicalResumeID" value="0" />
        <input type="hidden" value="" id="hdnVitalSignRecordID" runat="server" />
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
        <input type="hidden" value="0" id="hdnPatientVisitNoteID" runat="server" />
        <input type="hidden" value="" id="hdnSubjectiveText" runat="server" />
        <input type="hidden" value="" id="hdnObjectiveText" runat="server" />
        <input type="hidden" value="" id="hdnAssessmentText" runat="server" />
        <input type="hidden" value="" id="hdnPlanningText" runat="server" />
        <input type="hidden" value="" id="hdnMedicationResumeText" runat="server" />
        <input type="hidden" value="" id="hdnDischargeMedicationResumeText" runat="server" />
        <input type="hidden" value="" id="hdnMedicalResumeText" runat="server" />
        <input type="hidden" value="" id="hdnInstructionText" runat="server" />
        <input type="hidden" value="" id="hdnLaboratorySummary" runat="server" />
        <input type="hidden" value="" id="hdnImagingSummary" runat="server" />
        <input type="hidden" value="" id="hdnDiagnosticSummary" runat="server" />
        <input type="hidden" id="hdnDiagnosisSummary" runat="server" />
        <input type="hidden" id="hdnPatientInformation" runat="server" />
        <input type="hidden" id="hdnParamedicID" runat="server" />
        <input type="hidden" id="hdnRevisionParamedicID" runat="server" />
        <input type="hidden" id="hdnGCReferrerGroup" value="" runat="server" />
        <input type="hidden" runat="server" id="hdnVisitNoteID" value="0" />
        <input type="hidden" runat="server" id="hdnIsCasemixRevision" value="0" />
        <input type="hidden" runat="server" id="hdnIsCasemixAllowChange" value="0" />
        <fieldset id="mpPatientStatus">
            <table class="tblContentArea">
                <colgroup>
                    <col style="width: 20%" />
                    <col style="width: 80%" />
                </colgroup>
                <tr>
                    <td style="vertical-align: top">
                        <div id="leftPageNavPanel" class="w3-border">
                            <ul>
                                <li contentid="divPage1" title="Anamnesa/Keluhan Utama Pasien" class="w3-hover-red">
                                    Anamnesa/Keluhan Utama Pasien</li>
                                <li contentid="divPage2" title="Pemeriksaan Fisik" class="w3-hover-red">Pemeriksaan
                                    Fisik</li>
                                <li contentid="divPage3" title="Pemeriksaan Penunjang" class="w3-hover-red">Pemeriksaan
                                    Penunjang</li>
                                <li contentid="divPage4" title="Diagnosa" class="w3-hover-red">Diagnosa</li>
                                <li contentid="divPage5" title="Pengobatan/Terapi" class="w3-hover-red">Pengobatan/Terapi</li>
                                <li contentid="divPage6" title="Tindakan yang dilakukan" class="w3-hover-red">Tindakan yang dilakukan</li>
                                <li contentid="divPage7" title="Instruksi & Rencana Tindak Lanjut" class="w3-hover-red">Instruksi & Rencana Tindak Lanjut</li>
                            </ul>
                        </div>
                    </td>
                    <td style="vertical-align: top">
                        <div id="divPage1" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
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
                                                    <asp:TextBox ID="txtResumeDate" Width="120px" CssClass="datepicker" runat="server" />
                                                </td>
                                                <td style="padding-left: 5px">
                                                    <asp:TextBox ID="txtResumeTime" Width="80px" CssClass="time" runat="server" Style="text-align: center"
                                                        MaxLength="5" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel" style="width: 150px; vertical-align: top">
                                        <table border="0" cellpadding="0" cellspacing="1">
                                            <tr>
                                                <td class="tdLabel" valign="top" style="width: 190px">
                                                    <label class="lblMandatory lblNormal" id="lblHPI">
                                                        <%=GetLabel("Anamnesa/Riwayat Penyakit")%></label>
                                                </td>
                                                <td style="display: none">
                                                    <img class="imgLink" id="btnSubjectiveResumeTest" title='<%=GetLabel("Add to My Template")%>'
                                                        src='<%= ResolveUrl("~/Libs/Images/button/vitalsign.png")%>' alt="" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSubjectiveResumeText" runat="server" Width="98%" TextMode="Multiline"
                                            Rows="15" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label>
                                            <%=GetLabel("Surat Keterangan Sakit ")%></label>
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rblIsHasSickLetter" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Text=" Tidak" Value="0" Selected="True" />
                                            <asp:ListItem Text=" Ya" Value="1" />
                                        </asp:RadioButtonList>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNoOfDays" Width="60px" runat="server" CssClass="number" />
                                    </td>
                                    <td class="tdLabel">
                                        <label>
                                            <%=GetLabel("hari ")%></label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divPage2" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                            <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 5px">
                                <colgroup>
                                    <col width="150px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td colspan="2">
                                        <tr>
                                            <td style="vertical-align: top">
                                                <label class="lblNormal" id="Label5">
                                                    <%=GetLabel("Pemeriksaan Fisik") %></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtObjectiveResumeText" runat="server" Width="98%" TextMode="Multiline"
                                                    Rows="15" />
                                            </td>
                                        </tr>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divPage3" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                            <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 5px">
                                <colgroup>
                                    <col width="200px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td style="vertical-align: top">
                                        <label class="lblNormal" id="Label4">
                                            <%=GetLabel("Pemeriksaan Penunjang") %></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPlanningResumeText" runat="server" Width="98%" TextMode="Multiline"
                                            Rows="15" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divPage4" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                            <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                                <colgroup>
                                    <col width="200px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                        <label class="lblNormal" id="Label1">
                                            <%=GetLabel("Diagnosa") %></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAssessmentResumeText" runat="server" Width="98%" TextMode="Multiline"
                                            Rows="15" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divPage5" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                            <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                                <colgroup>
                                    <col width="150px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                        <label class="lblLink" id="lblEpisodeMedication">
                                            <%=GetLabel("Pengobatan/Terapi") %></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMedicationResumeText" runat="server" Width="98%" TextMode="Multiline"
                                            Rows="8" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                        <label class="lblLink" id="lblDischargePrescription">
                                            <%=GetLabel("Terapi Obat Pulang") %></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDischargeMedicationResumeText" runat="server" Width="98%" TextMode="Multiline"
                                            Rows="4" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divPage6" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                            <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                                <colgroup>
                                    <col width="200px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                        <table border="0" cellpadding="1" cellspacing="0">
                                            <tr>
                                                <td style="text-align: left">
                                                    <label class="lblNormal" id="Label3">
                                                        <%=GetLabel("Prosedur Terapi dan Tindakan") %></label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right">
                                                    <label class="lblLink" id="lblSurgeryProcedure" style="display: none">
                                                        <%=GetLabel("Kamar Operasi") %></label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMedicalResumeText" runat="server" Width="98%" TextMode="Multiline"
                                            Rows="15" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divPage7" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                            <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                                <colgroup>
                                    <col width="150px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                        <label class="lblNormal" id="Label2">
                                            <%=GetLabel("Instruksi & Rencana Tindak Lanjut")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtInstructionResumeText" runat="server" Width="98%" TextMode="Multiline"
                                            Rows="15" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
            <table class="tblContentArea" style="display: none">
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
    </div>
    <div style="display: none">
        <dxcp:ASPxCallbackPanel ID="cbpGenerate" runat="server" Width="100%" ClientInstanceName="cbpGenerate"
            ShowLoadingPanel="false" OnCallback="cbpGenerate_Callback">
            <ClientSideEvents EndCallback="function(s,e){ onCbpGenerateEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
    </div>
</asp:Content>
