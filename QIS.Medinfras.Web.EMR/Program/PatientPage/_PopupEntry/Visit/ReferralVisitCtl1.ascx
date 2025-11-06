<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReferralVisitCtl1.ascx.cs"
    Inherits="QIS.Medinfras.Web.EMR.Program.ReferralVisitCtl1" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_patientvisitctl">
    $('#lblPatientVisitAddData').die('click');
    $('#lblPatientVisitAddData').live('click', function () {
        var dateToday = $('#<%=hdnDateToday.ClientID %>').val();
        cboServiceUnit.SetValue('');
        $('#<%=hdnPhysicianID.ClientID %>').val('');
        $('#<%=txtPhysicianCode.ClientID %>').val('');
        $('#<%=txtPhysicianName.ClientID %>').val('');
        $('#<%=txtDiagnosisText.ClientID %>').val('');
        $('#<%=txtMedicalResumeText.ClientID %>').val('');
        $('#<%=txtPlanningResumeText.ClientID %>').val('');
        $('#<%=txtAppointmentDate.ClientID %>').val(dateToday);

        $('#containerPatientVisitEntryData').show();
        $('#leftPageNavPanel ul li').first().click();
    });

    $('#btnPatientVisitCancel').die('click');
    $('#btnPatientVisitCancel').live('click', function () {
        $('#containerPatientVisitEntryData').hide();
    });

    $('#btnPatientVisitSave').click(function (evt) {
        var rblSelected = $('#<%=rblReferralType.ClientID %> input:checked').val();
        if (IsValid(evt, 'fsPatientVisit', 'mpPatientVisit')) {
            if (rblSelected == "2") {
                if (cboServiceUnit.GetValue() != null && $('#<%=txtAppointmentDate.ClientID %>').val() != "") {
                    cbpPatientVisitTransHd.PerformCallback('save');
                }
                else {
                    displayErrorMessageBox("Rujukan Internal Pasien", "Harap isi klinik yang dituju dan tanggal appointment");
                }
            }
            else {
                var paramedicID = $('#<%=hdnPhysicianID.ClientID %>').val();
                if (paramedicID != '') {
                    cbpPatientVisitTransHd.PerformCallback('save');
                }
                else {
                    displayErrorMessageBox("Rujukan Internal Pasien", "Untuk tipe rujukan : Konsultasi Bersama atau Kunjungan di hari yang sama, Dokter yang dirujuk harus diisi.");
                }
            }
        }
        return false;
    });

    $('#btnSOAPCopy').click(function (evt) {
        onGenerateSummaryText();
    });

    //#region Physician
    function onGetPatientVisitParamedicFilterExpression() {
        var isCheckedFilter = $('#<%=chkParamedicHasSchedulePopUpCtl.ClientID %>').is(":checked");
        var polyclinicID = cboServiceUnit.GetValue();
        var filterExpression = "GCParamedicMasterType = 'X019^001' AND IsDeleted = 0";
        var rblSelected = $('#<%=rblReferralType.ClientID %> input:checked').val();

        var date = new Date();
        var hourInString = date.getHours().toString();
        var minutesInString = date.getMinutes().toString();

        if (hourInString.length == 1) {
            hourInString = '0' + hourInString;
        }

        if (minutesInString.length == 1) {
            minutesInString = '0' + minutesInString;
        }

        var formattedTime = hourInString + ":" + minutesInString;
        var registrationDateFormatString = Methods.dateToString(date);       

        if (rblSelected == "2") {
            registrationDate = $('#<%:txtAppointmentDate.ClientID %>').val();
            registrationDateInDatePicker = Methods.getDatePickerDate(registrationDate);
            registrationDateFormatString = Methods.dateToString(registrationDateInDatePicker);
        }
        var daynumber = $('#<%=hdnDayNumber.ClientID %>').val();

        if (isCheckedFilter) {
            if (rblSelected == "2") {
                filterExpression = "GCParamedicMasterType = 'X019^001' AND ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = '" + polyclinicID + "') AND IsDeleted = 0 AND (IsHasPhysicianRole = 1) AND ParamedicID IN (SELECT ParamedicID FROM vParamedicSchedule WHERE HealthcareServiceUnitID = '" + polyclinicID + "' AND DayNumber = '" + daynumber + "' UNION ALL SELECT ParamedicID FROM vParamedicScheduleDate WHERE HealthcareServiceUnitID = '" + polyclinicID + "' AND ScheduleDate = '" + registrationDateFormatString + "')";
            }
            else {
                filterExpression = "GCParamedicMasterType = 'X019^001' AND ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = '" + polyclinicID + "') AND IsDeleted = 0 AND (IsHasPhysicianRole = 1) AND ParamedicID IN (SELECT ParamedicID FROM vParamedicSchedule WHERE HealthcareServiceUnitID = '" + polyclinicID + "' AND DayNumber = '" + daynumber + "' AND (('" + formattedTime + "' BETWEEN StartTime1 AND EndTime1) OR ('" + formattedTime + "' BETWEEN StartTime2 AND EndTime2) OR ('" + formattedTime + "' BETWEEN StartTime3 AND EndTime3) OR ('" + formattedTime + "' BETWEEN StartTime4 AND EndTime4) OR ('" + formattedTime + "' BETWEEN StartTime5 AND EndTime5)) UNION ALL SELECT ParamedicID FROM vParamedicScheduleDate WHERE HealthcareServiceUnitID = '" + polyclinicID + "' AND ScheduleDate = '" + registrationDateFormatString + "' AND (('" + formattedTime + "' BETWEEN StartTime1 AND EndTime1) OR ('" + formattedTime + "' BETWEEN StartTime2 AND EndTime2) OR ('" + formattedTime + "' BETWEEN StartTime3 AND EndTime3) OR ('" + formattedTime + "' BETWEEN StartTime4 AND EndTime4) OR ('" + formattedTime + "' BETWEEN StartTime5 AND EndTime5)))";
            }
        }
        else {
            if (polyclinicID != '')
                filterExpression = "GCParamedicMasterType = 'X019^001' AND ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = " + polyclinicID + ") AND IsDeleted = 0 AND (IsHasPhysicianRole = 1)";
        }
        return filterExpression;
    }

    $('#lblPatientVisitPhysician.lblLink').live('click', function () {
        var polyclinicID = cboServiceUnit.GetValue();
        var isCheckedFilter = $('#<%=chkParamedicHasSchedulePopUpCtl.ClientID %>').is(":checked");
        if (isCheckedFilter) {
            if (polyclinicID != '' && polyclinicID != null) {
                openSearchDialog('paramedic', onGetPatientVisitParamedicFilterExpression(), function (value) {
                    $('#<%=txtPhysicianCode.ClientID %>').val(value);
                    onTxtPatientVisitPhysicianCodeChanged(value);
                });
            }
            else {
                showToast("Warning", "Silahkan Pilih Klinik Terlebih Dahulu");
            }
        }
        else {
            openSearchDialog('paramedic', onGetPatientVisitParamedicFilterExpression(), function (value) {
                $('#<%=txtPhysicianCode.ClientID %>').val(value);
                onTxtPatientVisitPhysicianCodeChanged(value);
            });
        }
    });

    $('#<%=txtPhysicianCode.ClientID %>').live('change', function () {
        var polyclinicID = cboServiceUnit.GetValue();
        var isCheckedFilter = $('#<%=chkParamedicHasSchedulePopUpCtl.ClientID %>').is(":checked");
        if (isCheckedFilter) {
            if (polyclinicID != '' && polyclinicID != null) {
                onTxtPatientVisitPhysicianCodeChanged($(this).val());
            }
            else {
                $('#<%=txtPhysicianCode.ClientID %>').val('');
                showToast("Warning", "Silahkan Pilih Klinik Terlebih Dahulu");              
            }
        }
        else {
            onTxtPatientVisitPhysicianCodeChanged($(this).val());
        }
    });

    function onTxtPatientVisitPhysicianCodeChanged(value) {
        var rblSelected = $('#<%=rblReferralType.ClientID %> input:checked').val();
        var filterExpression = onGetPatientVisitParamedicFilterExpression() + " AND ParamedicCode = '" + value + "'";
        Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
            if (result != null) {
                var date = new Date();
                var registrationDateFormatString = Methods.dateToString(date);
                if (rblSelected == "2") {
                    registrationDate = $('#<%:txtAppointmentDate.ClientID %>').val();
                    registrationDateInDatePicker = Methods.getDatePickerDate(registrationDate);
                    registrationDateFormatString = Methods.dateToString(registrationDateInDatePicker);
                }

                var filterExpressionLeave = "IsDeleted = 0 AND ('" + registrationDateFormatString + "' BETWEEN StartDate AND EndDate) AND ParamedicCode = '" + value + "'";
                Methods.getObject('GetvParamedicLeaveScheduleList', filterExpressionLeave, function (resultLeave) {
                    if (resultLeave == null) {
                        $('#<%=hdnPhysicianID.ClientID %>').val(result.ParamedicID);
                        $('#<%=txtPhysicianName.ClientID %>').val(result.ParamedicName);
                    }
                    else {
                        $('#<%=hdnPhysicianID.ClientID %>').val('');
                        $('#<%=txtPhysicianCode.ClientID %>').val('');
                        $('#<%=txtPhysicianName.ClientID %>').val('');

                        var info = result.ParamedicName + " Sedang Dalam Masa Cuti";
                        showToast("INFORMASI", info);
                    }
                });
            }
            else {
                $('#<%=hdnPhysicianID.ClientID %>').val('');
                $('#<%=txtPhysicianCode.ClientID %>').val('');
                $('#<%=txtPhysicianName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    $('.imgPatientVisitEdit.imgLink').die('click');
    $('.imgPatientVisitEdit.imgLink').live('click', function () {
        $row = $(this).parent().parent();

        var patientReferralID = $row.find('.hdnID').val();
        var visitID = $row.find('.hdnVisitID').val();
        var referralType = $row.find('.hdnGCRefferalType').val();
        var serviceUnitID = $row.find('.hdnToHealthcareServiceUnitID').val();
        var paramedicID = $row.find('.hdnToPhysicianID').val();
        var paramedicCode = $row.find('.hdnToPhysicianCode').val();
        var paramedicName = $row.find('.hdnToPhysicianName').val();
        var appointmentRequestDate = $row.find('.hdncfAppointmentRequestDateInString').val();
        var scheduleDate = $row.find('.hdncfScheduleDate').val();

        var visitTypeID = $row.find('.hdnVisitTypeID').val();
        var visitTypeCode = $row.find('.hdnVisitTypeCode').val();
        var visitTypeName = $row.find('.divVisitTypeName').html();
        var visitReason = $row.find('.hdnVisitReason').val();

        var diagnosisText = $row.find('.hdnDiagnosisText').val();
        var medicalResumeText = $row.find('.hdnMedicalResumeText').val();
        var planningResumeText = $row.find('.hdnPlanningResumeText').val();

        cboServiceUnit.SetValue(serviceUnitID);
        $('#<%=hdnPhysicianID.ClientID %>').val(paramedicID);
        $('#<%=txtPhysicianCode.ClientID %>').val(paramedicCode);
        $('#<%=txtPhysicianName.ClientID %>').val(paramedicName);
        $('#<%=hdnVisitID.ClientID %>').val(visitID);
        $('#<%=hdnPatientReferralID.ClientID %>').val(patientReferralID);
        $('#<%=txtDiagnosisText.ClientID %>').val(diagnosisText);
        $('#<%=txtMedicalResumeText.ClientID %>').val(medicalResumeText);
        $('#<%=txtPlanningResumeText.ClientID %>').val(planningResumeText);

        if (referralType == "X075^01") {
            $('#<%=rblReferralType.ClientID %>').find("input[value='0']").prop("checked", true);
            $('.trAppointmentDate').hide();
        }
        else if (referralType == "X075^04") {
            $('#<%=rblReferralType.ClientID %>').find("input[value='1']").prop("checked", true);
            $('.trAppointmentDate').hide();
        }
        else {
            $('#<%=rblReferralType.ClientID %>').find("input[value='2']").prop("checked", true);
            $('.trAppointmentDate').show();
        }


        if (appointmentRequestDate == "01-01-1900") {
            if (scheduleDate == "01-01-1900") {
                appointmentRequestDate = "";
            }
            else {
                appointmentRequestDate = scheduleDate;
            }
        }


        $('#<%=txtAppointmentDate.ClientID %>').val(appointmentRequestDate);

        $('#containerPatientVisitEntryData').show();
        $('#leftPageNavPanel ul li').first().click();
    });

    $('.imgPatientVisitDelete.imgLink').die('click');
    $('.imgPatientVisitDelete.imgLink').live('click', function () {
        $row = $(this).parent().parent();
        var patientReferralID = $row.find('.hdnID').val();
        displayConfirmationMessageBox('DELETE : RUJUKAN', 'Hapus data rujukan yang dipilih ?', function (result) {
            if (result) {
                $('#<%=hdnPatientReferralID.ClientID %>').val(patientReferralID);
                cbpPatientVisitTransHd.PerformCallback('delete');
            }
        });
    });

    function onCbpPatientVisitTransHdEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail') {
                if (param[2] == 'confirmation') {
                    showToastConfirmation(param[3] + '. Melanjutkan proses?', function (resultConfirm) {
                        if (resultConfirm) {
                            cbpConfirmSave.PerformCallback('confirm');
                        }
                    });
                }
                else {
                    displayErrorMessageBox('DELETE', param[2]);
                }
            }
            else {
                $('#containerPatientVisitEntryData').hide();
            }
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail') {
                var messageBody = param[2];
                displayErrorMessageBox('DELETE', messageBody);
            }
            else {
                $('#<%=hdnPatientReferralID.ClientID %>').val('');
            }
        }
        hideLoadingPanel();
    }

    function onCbpConfirmSaveEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'confirm') {
            if (param[1] == 'fail') {
                displayErrorMessageBox('DELETE', param[2]);
            }
            else {
                cbpPatientVisitTransHd.PerformCallback('refresh');
                $('#containerPatientVisitEntryData').hide();
            }
        }
        hideLoadingPanel();
    }

    function onGenerateSummaryText() {
        var message = "Generate / salin text diagnosa, pemeriksaaan dan terapi ?";
        displayConfirmationMessageBox('SALIN SOAP', message, function (result) {
            if (result) cbpSOAPCopy.PerformCallback();
        });
    }

    function onCbpSOAPCopyEndCallback(s) {
        hideLoadingPanel();
        var param = s.cpResult.split('|');
        if (param[0] == 'success') {
            var data = param[1].split('~');
            $('#<%=txtDiagnosisText.ClientID %>').val(data[0]);
            $('#<%=txtMedicalResumeText.ClientID %>').val(data[1]);
            $('#<%=txtPlanningResumeText.ClientID %>').val(data[2]);
        }
        else {
            displayErrorMessageBox('SOAP COPY : FAILED', param[1]);
        }
    }

    setDatePicker('<%=txtAppointmentDate.ClientID %>');
    $('#<%=txtAppointmentDate.ClientID %>').datepicker('option', 'minDate', '0');

    $('#<%=rblReferralType.ClientID %> input').change(function () {
        var rblSelected = $('#<%=rblReferralType.ClientID %> input:checked').val();
        if (rblSelected != "2") {
            $('.trAppointmentDate').hide();
        }
        else {
            $('.trAppointmentDate').show();
        }
    });

    $('#<%:chkParamedicHasSchedulePopUpCtl.ClientID %>').live('change', function () {
        $('#<%:hdnPhysicianID.ClientID %>').val('');
        $('#<%:txtPhysicianCode.ClientID %>').val('');
        $('#<%:txtPhysicianName.ClientID %>').val('');
        cboServiceUnit.SetValue('');
    });

    $('#<%=txtAppointmentDate.ClientID %>').live('change', function () {
        var dateToday = $('#<%=hdnDateToday.ClientID %>').val();
        var dateSelected = $('#<%=txtAppointmentDate.ClientID %>').val();
        var registrationDateInDatePicker = Methods.getDatePickerDate(dateSelected);

        if (new Date(registrationDateInDatePicker).toString() !== 'Invalid Date') {
            var from = dateSelected.split("-");
            var f = new Date(from[2], from[1] - 1, from[0]);

            var to = dateToday.split("-");
            var t = new Date(to[2], to[1] - 1, to[0]);

            if (f < t) {
                showToast('Warning', 'Tidak bisa pilih tanggal sebelum hari ini!');
                $('#<%=txtAppointmentDate.ClientID %>').val(dateToday);
                cbpViewPopUpCtl.PerformCallback('getdaynumber');
            }
            else {
                cbpViewPopUpCtl.PerformCallback('getdaynumber');
            }
        }
        else {
            $('#<%=txtAppointmentDate.ClientID %>').val(dateToday);
            cbpViewPopUpCtl.PerformCallback('getdaynumber');
        }
    });

    $('.btnResponse').live('click', function () {
        alert('Click');
        var $tr = $(this).closest('tr');
        var selectedID = $tr.find('.keyField').html();
        var param = selectedID + '|';
        openUserControlPopup("~/Program/PatientPage/Planning/Referral/ViewReferralResponseCtl.ascx", param, "Jawaban Konsul Rawat Bersama", 700, 400);
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
        x = document.getElementsByClassName("divPageNavPanel1Content");
        for (i = 0; i < x.length; i++) {
            x[i].style.display = "none";
        }
        document.getElementById(contentID).style.display = "block";
    }
    //#endregion
</script>
<div style="height: 600px; overflow-y: auto">
    <input type="hidden" value="0" id="hdnDateToday" runat="server" />
    <input type="hidden" value="" runat="server" id="hdnRegistrationID" />
    <input type="hidden" value="" runat="server" id="hdnClassID" />
    <input type="hidden" value="" runat="server" id="hdnBusinessPartnerID" />
    <input type="hidden" value="" runat="server" id="hdnItemCardFee" />
    <input type="hidden" value="" runat="server" id="hdnPatientReferralID" />
    <input type="hidden" value="0" runat="server" id="hdnIsValidateParamedicSchedule" />
    <input type="hidden" value="0" runat="server" id="hdnIsParamedicInRegistrationUseScheduleCtl" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 160px" />
                        <col style="width: 160px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No. Registrasi")%>
                                /
                                <%=GetLabel("No. RM")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRegistrationNo" Width="100%" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtMRN" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Nama Pasien")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtPatientName" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
                <div id="containerPatientVisitEntryData" style="margin-top: 10px; display: none;">
                    <input type="hidden" id="hdnVisitID" runat="server" value="" />
                    <table border="0" cellpadding="1" cellspacing="0" style="width:100%">
                         <colgroup>
                            <col style="width: 20%" />
                            <col style="width: 80%" />
                        </colgroup>          
                        <tr>
                            <td style="vertical-align:top">
                                <div id="leftPageNavPanel" class="w3-border">
                                    <ul>
                                        <li contentID="divPage1" contentIndex="1" title="Informasi Umum" class="w3-hover-red">Data Rujukan</li>
                                        <li contentID="divPage2" contentIndex="1" title="Catatan Rujukan" class="w3-hover-red" style="display:none">Catatan Rujukan</li>
                                        <li contentID="divPage3" contentIndex="2" title="Pasien BPJS" class="w3-hover-red" style="display:none">Surat Kontrol BPJS</li>
                                    </ul>
                                </div>
                            </td>
                            <td>
                                <div id="divPage1" class="w3-border divPageNavPanel1Content w3-animate-left" style="display: none">
                                    <fieldset id="fsPatientVisit" style="margin: 0">
                                        <table class="tblEntryDetail" style="width: 100%">
                                            <colgroup>
                                                <col style="width: 140px" />
                                                <col style="width: 120px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblNormal" id="Label3">
                                                        <%=GetLabel("Jenis Rujukan")%></label>
                                                </td>
                                                <td colspan="2">
                                                    <asp:RadioButtonList ID="rblReferralType" runat="server" RepeatDirection="Horizontal">
                                                        <asp:ListItem Text=" Konsultasi Bersama (Team)" Value="0" />
                                                        <asp:ListItem Text=" Kunjungan Hari yang sama" Value="1" />
                                                        <asp:ListItem Text=" Perjanjian" Value="2" Selected="True" />
                                                    </asp:RadioButtonList>
                                                </td>
                                            </tr>
                                            <tr id="trServiceUnitCtl" runat="server">
                                                <td class="tdLabel">
                                                    <label class="lblMandatory" id="Label1">
                                                        <%=GetLabel("Klinik")%></label>
                                                </td>
                                                <td colspan="2">
                                                    <dxe:ASPxComboBox ID="cboServiceUnit" ClientInstanceName="cboServiceUnit" Width="100%"
                                                        runat="server">
                                                    </dxe:ASPxComboBox>
                                                </td>
                                            </tr>
                                            <tr id="trParamedicHasScheduleCtl" runat="server">
                                                <td>
                                                </td>
                                                <td colspan="2">
                                                    <asp:CheckBox ID="chkParamedicHasSchedulePopUpCtl" runat="server" /><%:GetLabel("Tampilkan Hanya Dokter Yang Punya Jadwal")%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblLink" id="lblPatientVisitPhysician">
                                                        <%=GetLabel("Dokter ")%></label>
                                                </td>
                                                <td>
                                                    <input type="hidden" id="hdnPhysicianID" value="" runat="server" />
                                                    <asp:TextBox ID="txtPhysicianCode" CssClass="required" Width="100%" runat="server" />
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPhysicianName" ReadOnly="true" Width="99%" runat="server" />
                                                </td>
                                            </tr>
                                            <tr class="trAppointmentDate">
                                                <td class="tdLabel" valign="top">
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("Tanggal")%></label>
                                                </td>
                                                <td colspan="2">
                                                    <asp:TextBox ID="txtAppointmentDate" Width="100px" CssClass="datepicker" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("Diagnosis Pasien:")%></label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:TextBox ID="txtDiagnosisText" CssClass="required" Width="99%" runat="server"
                                                        TextMode="MultiLine" Rows="2" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("Pemeriksaan Fisik / Pemeriksaan Penunjang / Catatan Medis lain yang perlu mendapat perhatian :")%></label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:TextBox ID="txtMedicalResumeText" CssClass="required" Width="99%" runat="server"
                                                        TextMode="MultiLine" Rows="2" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("Terapi yang telah diberikan :")%></label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:TextBox ID="txtPlanningResumeText" CssClass="required" Width="99%" runat="server"
                                                        TextMode="MultiLine" Rows="2" />
                                                </td>
                                            </tr>
                                            <tr>                                          
                                                <td colspan="3" style="text-align:right;; width:100%" >
                                                    <input type="button" class="btnProcess w3-btn w3-hover-red" id="btnSOAPCopy" value='<%= GetLabel("Salin dari SOAP")%>'
                                                        style="width: 120px" />
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </div>
                                <div id="divPage2" class="w3-border divPageNavPanel1Content w3-animate-left" style="display: none">
                                    <fieldset id="fsPatientRefferalNotes" style="margin: 0">
                                        <table class="tblEntryDetail" style="width: 100%">
                                            <colgroup>
                                                <col style="width: 100px" />
                                                <col style="width: 120px" />
                                                <col />
                                            </colgroup>
                                        </table>
                                    </fieldset>
                                </div>
                                <div id="divPage3" class="w3-border divPageNavPanel1Content w3-animate-left" style="display: none">
                                    <fieldset id="fsPatientReferralBPJS" style="margin: 0">
                                        <table class="tblEntryDetail">
                                            <colgroup>
                                                <col style="width: 100px" />
                                                <col style="width: 120px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td class="tdLabel" style="vertical-align: top">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Jenis Rujukan")%></label>
                                                </td>
                                                <td colspan="2">
                                                    <asp:RadioButtonList ID="rblReferBackType" runat="server" RepeatDirection="Vertical">
                                                        <asp:ListItem Text=" Dapat Dikembalikan / Rujuk Balik" Value="1" Selected="True" />
                                                        <asp:ListItem Text=" Belum dapat dikembalikan / belum dapat dirujuk balik; dengan alasan"
                                                            Value="2" />
                                                    </asp:RadioButtonList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td />
                                                <td colspan="2" style="padding-left: 20px">
                                                    <asp:CheckBox ID="chkRefferalReasonMedication" runat="server" Text=" Terapi Farmakologis tidak tersedia di Faskes Tingkat 1"
                                                        Checked="false" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td />
                                                <td colspan="2" style="padding-left: 20px">
                                                    <asp:CheckBox ID="chkRefferalReasonFollowup" runat="server" Text=" Perlu Follow up hasil pemeriksaan dan terapi"
                                                        Checked="false" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td />
                                                <td colspan="2" style="padding-left: 20px">
                                                    <asp:CheckBox ID="chkRefferalReasonOther" runat="server" Text=" Lain-lain" Checked="false" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td />
                                                <td colspan="2" style="padding-left: 40px">
                                                    <asp:TextBox ID="txtRefferalReasonOtherText" Width="95%" runat="server" TextMode="MultiLine"
                                                        Rows="2" />
                                                </td>
                                            </tr>
                                            <tr id="trReferralNotes">
                                                <td class="tdLabel" style="vertical-align: top">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Catatan Rujukan Balik")%></label>
                                                </td>
                                                <td colspan="2">
                                                    <asp:TextBox ID="txtReferralNotes" Width="95%" runat="server" TextMode="MultiLine"
                                                        Rows="2" />
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </div>
                            </td>
                        </tr>     
                        <tr>
                            <td colspan="3" style="text-align:right;">
                                <table style="width: 100%">
                                    <colgroup>
                                        <col />
                                        <col style="width: 85px" />
                                        <col style="width: 85px" />
                                    </colgroup>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <input type="button" class="btnSave w3-btn w3-hover-blue" id="btnPatientVisitSave"
                                                value='<%= GetLabel("Save")%>' style="width: 80px" />
                                        </td>
                                        <td>
                                            <input type="button" class="btnCancel w3-btn w3-hover-blue" id="btnPatientVisitCancel"
                                                value='<%= GetLabel("Cancel")%>' style="width: 80px" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>                                
                    </table>
                </div>
                <dxcp:ASPxCallbackPanel ID="cbpPatientVisitTransHd" runat="server" Width="100%" ClientInstanceName="cbpPatientVisitTransHd"
                    ShowLoadingPanel="false" OnCallback="cbpPatientVisitTransHd_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpPatientVisitTransHdEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlPatientVisitTransHdGrdView" Style="width: 100%;
                                margin-left: auto; margin-right: auto; position: relative; font-size: 0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false"
                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                            <ItemTemplate>
                                                <input type="hidden" value="<%#:Eval("ID") %>" class="hdnID" bindingfield="ID" />
                                                <input type="hidden" value="<%#:Eval("cfReferralDate") %>" class="hdncfReferralDate"
                                                    bindingfield="cfReferralDate" />
                                                <input type="hidden" value="<%#:Eval("cfScheduleDate") %>" class="hdncfScheduleDate"
                                                    bindingfield="cfScheduleDate" />
                                                <input type="hidden" value="<%#:Eval("cfAppointmentRequestDateInString") %>" class="hdncfAppointmentRequestDateInString"
                                                    bindingfield="cfAppointmentRequestDateInString" />
                                                <input type="hidden" value="<%#:Eval("ReferralTime") %>" class="hdnReferralTime"
                                                    bindingfield="ReferralTime" />
                                                <input type="hidden" value="<%#:Eval("FromPhysicianID") %>" class="hdnFromPhysicianID"
                                                    bindingfield="FromPhysicianID" />
                                                <input type="hidden" value="<%#:Eval("FromPhysicianCode") %>" class="hdnFromPhysicianCode"
                                                    bindingfield="FromPhysicianCode" />
                                                <input type="hidden" value="<%#:Eval("FromPhysicianName") %>" class="hdnFromPhysicianName"
                                                    bindingfield="FromPhysicianName" />
                                                <input type="hidden" value="<%#:Eval("GCRefferalType") %>" class="hdnGCRefferalType"
                                                    bindingfield="GCRefferalType" />
                                                <input type="hidden" value="<%#:Eval("ToHealthcareServiceUnitID") %>" class="hdnToHealthcareServiceUnitID"
                                                    bindingfield="ToHealthcareServiceUnitID" />
                                                <input type="hidden" value="<%#:Eval("ToPhysicianID") %>" class="hdnToPhysicianID"
                                                    bindingfield="ToPhysicianID" />
                                                <input type="hidden" value="<%#:Eval("ToPhysicianCode") %>" class="hdnToPhysicianCode"
                                                    bindingfield="ToPhysicianCode" />
                                                <input type="hidden" value="<%#:Eval("ToPhysicianName") %>" class="hdnToPhysicianName"
                                                    bindingfield="ToPhysicianName" />
                                                <input type="hidden" value="<%#:Eval("cfResponseDateTime") %>" class="hdncfResponseDateTime"
                                                    bindingfield="cfResponseDateTime" />
                                                <input type="hidden" value="<%#:Eval("DiagnosisText") %>" class="hdnDiagnosisText"
                                                    bindingfield="DiagnosisText" />
                                                <input type="hidden" value="<%#:Eval("MedicalResumeText") %>" class="hdnMedicalResumeText"
                                                    bindingfield="MedicalResumeText" />
                                                <input type="hidden" value="<%#:Eval("PlanningResumeText") %>" class="hdnPlanningResumeText"
                                                    bindingfield="PlanningResumeText" />
                                                <input type="hidden" value="<%#:Eval("IsReply") %>" class="hdnIsReply" bindingfield="IsReply" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="50px">
                                            <ItemTemplate>
                                                <img class='imgPatientVisitEdit <%#: Eval("cfIsAllowEdit").ToString() == "False" ? "imgDisabled" : "imgLink"%>'
                                                    title='<%=GetLabel("Edit")%>' src='<%# Eval("cfIsAllowEdit").ToString() == "False" ? ResolveUrl("~/Libs/Images/Button/edit_disabled.png") : ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                    alt="" style="float: left; margin-right: 2px;" />
                                                <img class='imgPatientVisitDelete <%#: Eval("cfIsAllowEdit").ToString() == "False" ? "imgDisabled" : "imgLink"%>'
                                                    title='<%=GetLabel("Delete")%>' src='<%# Eval("cfIsAllowEdit").ToString() == "False" ? ResolveUrl("~/Libs/Images/Button/delete_disabled.png") : ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                    alt="" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="cfReferralDate" HeaderText="Tanggal" HeaderStyle-Width="80px"
                                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="ReferralType" HeaderText="Jenis Rujukan" HeaderStyle-Width="200px"
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="ToPhysicianName" HeaderText="Dirujuk/Konsultasi Ke" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="cfResponseDateTime" HeaderText="Tanggal Response" HeaderStyle-Width="120px"
                                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="cfIsResponse" HeaderText="Response" HeaderStyle-Width="80px"
                                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
<%--                                        <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="80px">
                                            <ItemTemplate>
                                                <div <%# Eval("IsReply").ToString() == "False" ? "Style='display:none'":"" %>>
                                                    <input type="button" id="btnResponse" runat="server" class="btnResponse" value="Baca Respon"
                                                        style="height: 25px; width: 100px; background-color: Red; color: White;" />
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("Tidak ada riwayat konsultasi / rawat bersama")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                                <div class="imgLoadingGrdView" id="containerImgLoadingView">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div style="width: 100%; text-align: center" id="divContainerAddData" runat="server">
                    <span class="lblLink" id="lblPatientVisitAddData">
                        <%= GetLabel("Tambah Kunjungan Rujukan")%></span>
                </div>
            </td>
        </tr>
    </table>
</div>
<div>
    <dxcp:ASPxCallbackPanel ID="cbpSOAPCopy" runat="server" Width="100%" ClientInstanceName="cbpSOAPCopy"
        ShowLoadingPanel="false" OnCallback="cbpSOAPCopy_Callback">
        <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpSOAPCopyEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
    <dxcp:ASPxCallbackPanel ID="cbpConfirmSave" runat="server" Width="100%" ClientInstanceName="cbpConfirmSave"
        ShowLoadingPanel="false" OnCallback="cbpConfirmSave_Callback">
        <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpConfirmSaveEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
    <dxcp:ASPxCallbackPanel ID="cbpViewPopUpCtl" runat="server" Width="0%" ClientInstanceName="cbpViewPopUpCtl"
        ShowLoadingPanel="false" OnCallback="cbpViewPopUpCtl_Callback">
        <ClientSideEvents EndCallback="function(s,e) { hideLoadingPanel(); }" />
        <PanelCollection>
            <dx:PanelContent ID="PanelContent2" runat="server">
                <asp:Panel runat="server" ID="pnlReferrerGrdView" Style="width: 0%">
                    <input type="hidden" runat="server" id="hdnDayNumber" value="" />
                </asp:Panel>
            </dx:PanelContent>
        </PanelCollection>
    </dxcp:ASPxCallbackPanel>
</div>
