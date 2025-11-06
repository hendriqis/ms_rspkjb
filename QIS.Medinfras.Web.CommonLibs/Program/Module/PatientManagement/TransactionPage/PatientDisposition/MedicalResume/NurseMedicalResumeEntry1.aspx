<%@ Page Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageList.master"
    AutoEventWireup="true" CodeBehind="NurseMedicalResumeEntry1.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.NurseMedicalResumeEntry1" %>

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
    <li id="btnPatientStatusSave" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbsave.png")%>' alt="" /><div>
            <%=GetLabel("Save")%></div>
    </li>
    <li id="btnDiscardChanges" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbcancel.png")%>' alt="" /><div>
            <%=GetLabel("Batal Perubahan")%></div>
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
            $('#<%=txtResumeDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

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

            $('#<%=rblIsPatientFamily.ClientID %> input').change(function () {
                if ($(this).val() == "1") {
                    $('#<%=trFamilyInfo.ClientID %>').removeAttr("style");
                }
                else {
                    $('#<%=trFamilyInfo.ClientID %>').attr("style", "display:none");
                }
            });

            $('#<%=btnPatientStatusSave.ClientID %>').click(function (evt) {
                if (IsValid(evt, 'fsPatientStatus', 'mpPatientStatus')) {
                    if (validateTime($('#<%=txtResumeTime.ClientID %>').val())) {
                        onCustomButtonClick('save');
                        $('#<%=hdnIsChanged.ClientID %>').val('0');
                    }
                    else {
                        showToast('Warning', 'Format Waktu yang diinput salah');                      
                    }
                }
            });

            registerCollapseExpandHandler();

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

        //#region Pemeriksaan Fisik

        //#endregion

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
                var message = "Anda belum menyimpan data perubahan yang sudah anda lakukan, lanjutkan dengan proses simpan data terlebih dahulu ?";
                displayConfirmationMessageBox("Resume Keperawatan",message, function (result) {
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
            if ($('#<%=txtInstructionResumeText1.ClientID %>').val() != "") {
                var message1 = "Ganti catatan yang sudah dientry di Instruksi dan Rencana Tindak Lanjut ?";
                var message2 = "<i>"+param+"</i>";
                displayConfirmationMessageBox('Instruksi dan Rencana Tindak Lanjut', message1+"<br/><br/>"+message2, function (result) {
                    if (result) {
                        $('#<%=txtInstructionResumeText1.ClientID %>').val(param);
                    }
                });
            }
            else
            {
                $('#<%=txtInstructionResumeText1.ClientID %>').val(param);
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
                    <td></td>
                    <td>
                        <table class="tblEntryContent" style="width: 100%">
                            <colgroup>
                                <col style="width: 220px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel" valign="top">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Tanggal dan Waktu Resume")%></label>
                                </td>
                                <td>
                                    <table border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtResumeDate" Width="120px" CssClass="datepicker" runat="server" />
                                            </td>
                                            <td style="padding-left: 5px">
                                                <asp:TextBox ID="txtResumeTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" MaxLength="5" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" valign="top">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Perawat")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboParamedicID" Width="300px" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                <td style="vertical-align:top">
                    <div id="leftPageNavPanel" class="w3-border">
                        <ul>
                            <li contentID="divPage1" title="Masalah Keperawatan" class="w3-hover-red">Masalah Keperawatan</li>      
                            <li contentID="divPage2" title="Tindakan Perawatan selama dirawat" class="w3-hover-red">Tindakan Perawatan selama dirawat</li>      
                            <li contentID="divPage3" title="Evaluasi Keperawatan" class="w3-hover-red">Evaluasi Keperawatan</li>     
                            <li contentID="divPage4" title="Kondisi dan Cara Pulang" class="w3-hover-red">Kondisi dan Cara Pulang</li>                                                                                                
                            <li contentID="divPage5" title="Catatan Terapi Pengobatan" class="w3-hover-red">Catatan Terapi Pengobatan</li>
                            <li contentID="divPage6" title="Catatan Khusus Diet Pasien" class="w3-hover-red">Catatan Khusus Diet Pasien</li>
                            <li contentID="divPage10" title="Edukasi dan Rencana Perawatan Di Rumah" class="w3-hover-red">Edukasi dan Rencana Perawatan Di Rumah</li>
                        </ul>     
                    </div> 
                </td>
                    <td style="vertical-align:top">
                        <div id="divPage1" class="w3-border divPageNavPanelContent w3-animate-left" style="display:none"> 
                            <table class="tblEntryContent" style="width: 100%">
                                <colgroup>
                                    <col style="width: 215px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel" valign="top">
                                        <table border="0" cellpadding="0" cellspacing="1">
                                            <tr>
                                                <td class="tdLabel" valign="top" style="width:190px">
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("Masalah Keperawatan pada saat perawatan")%></label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSubjectiveResumeText" runat="server" Width="100%" TextMode="Multiline"
                                            Rows="12"/>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divPage2" class="w3-border divPageNavPanelContent w3-animate-left" style="display:none"> 
                            <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                                <colgroup>
                                    <col width="220px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                        <table border="0" cellpadding="1" cellspacing="0">
                                            <tr>
                                                <td style="text-align:left">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Tindakan Perawat selama Perawatan") %></label>
                                                </td>

                                            </tr>
                                        </table>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPlanningResumeText" runat="server" TextMode="MultiLine" Rows="13" Width="98%" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divPage3" class="w3-border divPageNavPanelContent w3-animate-left" style="display:none"> 
                            <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                                <colgroup>
                                    <col width="220px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel" valign="top">
                                        <label class="lblNormal">
                                            <%=GetLabel("Evaluasi Keperawatan") %></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEvaluationResumeText" runat="server" Width="98%" TextMode="Multiline"
                                            Rows="15"  />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divPage4" class="w3-border divPageNavPanelContent w3-animate-left" style="display:none">
                            <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                                <colgroup>
                                    <col width="220px" />
                                    <col width="200px" />
                                    <col />
                                    <col width="120px" />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel"><label><%=GetLabel("Usia Lanjut (60 tahun atau lebih) ")%></label></td>
                                    <td>
                                        <asp:RadioButtonList ID="rblIsGeriatric" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Text=" Tidak" Value="0" Selected="True" />
                                            <asp:ListItem Text=" Ya" Value="1"  />
                                        </asp:RadioButtonList>
                                    </td>
                                    <td>
                                         <label><%=GetLabel("Membutuhkan Pelayanan Medis dan Perawatan berkelanjutan ")%></label>
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rblIsNeedFollowupCare" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Text=" Tidak" Value="0" Selected="True" />
                                            <asp:ListItem Text=" Ya" Value="1"  />
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel"><label><%=GetLabel("Hambatan Mobilisasi Fisik ")%></label></td>
                                    <td>
                                        <asp:RadioButtonList ID="rblIsImmobility" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Text=" Tidak" Value="0" Selected="True" />
                                            <asp:ListItem Text=" Ya" Value="1"  />
                                        </asp:RadioButtonList>
                                    </td>
                                    <td>
                                         <label><%=GetLabel("Tergantung dengan orang lain dalam aktifitas harian ")%></label>
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rblIsHasDependency" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Text=" Tidak" Value="0" Selected="True" />
                                            <asp:ListItem Text=" Ya" Value="1"  />
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblMandatory">
                                            <%=GetLabel("Transportasi")%></label>
                                    </td>
                                    <td>
                                        <dxe:ASPxComboBox ID="cboDischargeTransportation" ClientInstanceName="cboDischargeTransportation" Width="100%"
                                            runat="server">
                                        </dxe:ASPxComboBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                        <%=GetLabel("Kondisi Pasien saat Pulang") %>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtDischargeMedicalSummary" runat="server" Width="98%" TextMode="Multiline"
                                            Rows="12" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divPage5" class="w3-border divPageNavPanelContent w3-animate-left" style="display:none"> 
                            <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                                <colgroup>
                                    <col width="220px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                        <label class="lblNormal">
                                            <%=GetLabel("Catatan Terapi Pengobatan") %></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDischargeMedicationResumeText" runat="server" Width="98%" TextMode="Multiline"
                                            Rows="12" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divPage6" class="w3-border divPageNavPanelContent w3-animate-left" style="display:none"> 
                            <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                                <colgroup>
                                    <col width="220px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                        <table border="0" cellpadding="1" cellspacing="0">
                                            <tr>
                                                <td style="text-align:left">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Catatan Khusus Diet Pasien") %></label>
                                                </td>

                                            </tr>
                                        </table>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNutritionistResumeText" runat="server" Width="98%" TextMode="Multiline"
                                            Rows="12"  />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divPage10" class="w3-border divPageNavPanelContent w3-animate-left" style="display:none">
                            <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                                <colgroup>
                                    <col width="220px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel"><label><%=GetLabel("Edukasi diberikan kepada ")%></label></td>
                                    <td>
                                        <asp:RadioButtonList ID="rblIsPatientFamily" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="Pasien" Value="0" Selected="True" />
                                            <asp:ListItem Text="Keluarga / Lain-lain" Value="1"  />
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr id="trFamilyInfo" runat="server" style="display: none">
                                    <td class="tdLabel">
                                        <label class="lblMandatory">
                                            <%=GetLabel("Nama Penerima Edukasi")%></label>
                                    </td>
                                    <td>
                                        <table border="0" cellpadding="1" cellspacing="0" style="width:100%">
                                            <colgroup>
                                                <col style="width:140px"/>
                                                <col style="width:80px"/>
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtFamilyName" CssClass="txtFamilyName" runat="server" Width="100%"  />
                                                </td>
                                                <td class="tdLabel" style="padding-left:5px">
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("Hubungan")%></label>
                                                </td>
                                                <td>
                                                    <dxe:ASPxComboBox runat="server" ID="cboFamilyRelation" ClientInstanceName="cboFamilyRelation"
                                                        Width="99%" ToolTip = "Hubungan dengan Pasien" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>   
                                <tr>
                                    <td class="tdLabel"><label><%=GetLabel("Yang merawat pasien di rumah ")%></label></td>
                                    <td>
                                        <asp:TextBox ID="txtHomecarePIC" runat="server" Width="295px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                        <label class="lblNormal">
                                            <%=GetLabel("Alat medis yang dilanjutkan di rumah") %></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtInstructionResumeText1" runat="server" Width="98%" TextMode="Multiline"
                                            Rows="4" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                        <label class="lblNormal">
                                            <%=GetLabel("Alat bantu yang dilanjutkan di rumah") %></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtInstructionResumeText2" runat="server" Width="98%" TextMode="Multiline"
                                            Rows="3" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                        <label class="lblNormal">
                                            <%=GetLabel("Pendidikan kesehatan untuk di rumah")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEducationSummaryText" runat="server" Width="98%" TextMode="Multiline"
                                            Rows="4" />
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
    </div>
</asp:Content>
