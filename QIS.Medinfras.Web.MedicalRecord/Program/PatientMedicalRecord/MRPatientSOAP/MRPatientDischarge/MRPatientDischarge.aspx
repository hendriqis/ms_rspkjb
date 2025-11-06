<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageList.master"
    AutoEventWireup="true" CodeBehind="MRPatientDischarge.aspx.cs" Inherits="QIS.Medinfras.Web.MedicalRecord.Program.MRPatientDischarge" %>

<%@ Register Src="~/Program/PatientMedicalRecord/MRPatientSOAP/MRPatientSOAPToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnPatientDischargeProcess" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><div>
            <%=GetLabel("Process")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript">
        var registrationDateTimeInString = '<%=RegistrationDateTime%>';
        var registrationDateTime = Methods.stringToDateTime(registrationDateTimeInString);
        $(function () {
            setDatePicker('<%=txtDischargeDate.ClientID %>');
            setDatePicker('<%=txtDateOfDeath.ClientID %>');
            setDatePicker('<%=txtAppointmentDate.ClientID %>');
            setDatePicker('<%=txtReferralToDate.ClientID %>');

            $('#<%=txtDischargeDate.ClientID %>').datepicker('option', 'maxDate', '0');
            $('#<%=txtDateOfDeath.ClientID %>').datepicker('option', 'maxDate', '0');
            $('#<%=txtAppointmentDate.ClientID %>').datepicker('option', 'minDate', '0');
            $('#<%=txtReferralToDate.ClientID %>').datepicker('option', 'minDate', '0');

            if (($('#<%=chkIsDead.ClientID %>').attr('checked'))) {
                cboPatientOutcome.SetVisible(false);
                cboPatientOutcomeDead.SetVisible(true);
                $('#trDeathInfo').show();
            } else {
                cboPatientOutcome.SetVisible(true);
                cboPatientOutcomeDead.SetVisible(false);
                $('#trDeathInfo').hide();
            }

            $('#<%=btnPatientDischargeProcess.ClientID %>').click(function (evt) {
                if (cboDischargeRoutine.GetValue() == Constant.DischargeRoutine.REFER_TO_INPATIENT) {
                    var hdnParamedicID2 = $('#<%=hdnParamedicID2.ClientID %>').val();
                    if (hdnParamedicID2 == "" || hdnParamedicID2 == "0") {
                        displayErrorMessageBox('ERROR', "Mohon isi DPJP terlebih dahulu.");
                    }
                    else {
                        if (IsValid(evt, 'fsPatientDischarge', 'mpPatientDischarge')) {
                            onCustomButtonClick('process');
                        }
                    }
                }
                else if (cboDischargeRoutine.GetValue() == Constant.DischargeRoutine.REFER_TO_OUTPATIENT) {
                    var hdnPhysicianID = $('#<%=hdnPhysicianID.ClientID %>').val();
                    if (hdnPhysicianID == "" || hdnPhysicianID == "0") {
                        displayErrorMessageBox('ERROR', "Mohon isi Dokter terlebih dahulu.");
                    }
                    else {
                        if (IsValid(evt, 'fsPatientDischarge', 'mpPatientDischarge')) {
                            onCustomButtonClick('process');
                        }
                    }
                }
                else if (cboDischargeRoutine.GetValue() == Constant.DischargeRoutine.OTHER_HOSPITAL) {
                    var hdnReferrerID = $('#<%=hdnReferrerID.ClientID %>').val();
                    if (hdnReferrerID == "" || hdnReferrerID == "0") {
                        displayErrorMessageBox('ERROR', "Mohon isi detail rujukan kemana terlebih dahulu.");
                    }
                    else {
                        if (IsValid(evt, 'fsPatientDischarge', 'mpPatientDischarge')) {
                            if (validateTime($('#<%=txtReferralToTime.ClientID %>').val()) == true) {
                                if ($('#<%=txtReferralToDate.ClientID %>').val() != '') {
                                    if (cboDischargeReason.GetValue() != '') {
                                        onCustomButtonClick('process');
                                    }
                                    else {
                                        displayErrorMessageBox('ERROR', "Alasan ke Rumah Sakit Lain tidak boleh kosong");
                                    }
                                }
                                else {
                                    displayErrorMessageBox('ERROR', "Tanggal Rujukan tidak boleh kosong");
                                }
                            }
                        }
                    }
                }
                else {
                    if (IsValid(evt, 'fsPatientDischarge', 'mpPatientDischarge')) {
                        onCustomButtonClick('process');
                    }
                }
            });

            function validateTime(x) {
                if (x.length == 5) {
                    //var newreg = /^[0-2][0-3]:[0-5][0-9]$/;
                    var newreg = /^(([0-1][0-9])|(2[0-3])):[0-5][0-9]$/;

                    var first = x.split(":")[0];
                    var second = x.split(":")[1];

                    if (first > 24 || second > 59) {
                        displayErrorMessageBox('ERROR', "Format jam tidak sesuai. (HH:mm)");
                        return false;
                    }
                    else if (!newreg.test(x)) {
                        displayErrorMessageBox('ERROR', "Format jam tidak sesuai. (HH:mm)");
                        return false;
                    }
                }
                else if (x != 0) {
                    displayErrorMessageBox('ERROR', "Format jam tidak sesuai. (HH:mm)");
                    return false;
                } else {
                    displayErrorMessageBox('ERROR', "Format jam tidak sesuai. (HH:mm)");
                    return false;
                }

                return true;
            }

            $('#<%=txtDischargeDate.ClientID %>').change(function () {
                onDischargeDateTimeChange();
            });

            $('#<%=txtDischargeTime.ClientID %>').change(function () {
                onDischargeDateTimeChange();
            });

            onDischargeDateTimeChange();

            $('#<%=chkIsDead.ClientID %>').change(function () {
                if ($(this).is(':checked')) {
                    cboPatientOutcome.SetVisible(false);
                    cboPatientOutcomeDead.SetVisible(true);
                    $('#tblEntry tr.trDeathInfo').show();
                }
                else {
                    cboPatientOutcome.SetVisible(true);
                    cboPatientOutcomeDead.SetVisible(false);
                    $('#tblEntry tr.trDeathInfo').hide();
                }
            });
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

        //        function onAfterCustomClickSuccess(type) {
        //            exitPatientPage();
        //        }

        function onCboDischargeReasonValueChanged() {
            if (cboDischargeReason.GetValue() == Constant.DischargeReasonToOtherHospital.OTHER) {
                $('#trDischargeOtherReason').removeAttr('style');
            }
            else {
                $('#trDischargeOtherReason').attr('style', 'display:none');
            }
        }

        function onCboDischargeRoutineValueChanged() {
            var isParamedicInRegistrationUseSchedule = $('#<%=hdnIsParamedicInRegistrationUseSchedule.ClientID %>').val();
            if (cboDischargeRoutine.GetValue() == Constant.DischargeRoutine.OTHER_HOSPITAL) {
                cboDischargeReason.SetValue('');
                $('#trDischargeReason').removeAttr('style');
                $('#trReferralToDateTime').removeAttr('style');
                $('#trReferrerGroup').removeAttr('style');
                $('#trReferrer').removeAttr('style');
                $("#trAppointment").hide();
                $('#trInpatientPhysician').attr('style', 'display:none');
                $('#trParamedicHasSchedule').attr('style', 'display:none');
                $("#tblReferralNotes").show();
                $('#trDischargeRemarks').attr('style', 'display:none');
            }
            else if (cboDischargeRoutine.GetValue() == Constant.DischargeRoutine.REFER_TO_OUTPATIENT) {
                $("#trAppointment").show();

                if (isParamedicInRegistrationUseSchedule == "1") {
                    $('#trParamedicHasSchedule').removeAttr('style');
                }

                $('#trDischargeReason').attr('style', 'display:none');
                $('#trReferralToDateTime').attr('style', 'display:none');
                $('#trReferrerGroup').attr('style', 'display:none');
                $('#trReferrer').attr('style', 'display:none');
                $('#trDischargeOtherReason').attr('style', 'display:none');
                $('#trInpatientPhysician').attr('style', 'display:none');
                $("#tblReferralNotes").hide();
                $('#trDischargeRemarks').attr('style', 'display:none');
            }
            else if (cboDischargeRoutine.GetValue() == Constant.DischargeRoutine.REFER_TO_INPATIENT) {
                $('#trDischargeReason').attr('style', 'display:none');
                $('#trReferralToDateTime').attr('style', 'display:none');
                $('#trReferrerGroup').attr('style', 'display:none');
                $('#trReferrer').attr('style', 'display:none');
                $('#trDischargeOtherReason').attr('style', 'display:none');
                $("#trAppointment").hide();
                $('#trInpatientPhysician').removeAttr('style');
                $('#trParamedicHasSchedule').attr('style', 'display:none');
                $("#tblReferralNotes").hide();
                $('#trDischargeRemarks').attr('style', 'display:none');
            }
            else if (cboDischargeRoutine.GetValue() == Constant.DischargeRoutine.FORCE_DISCHARGE) {
                $('#trReferrerGroup').attr('style', 'display:none');
                $('#trDischargeReason').attr('style', 'display:none');
                $('#trReferralToDateTime').attr('style', 'display:none');
                $('#trReferrer').attr('style', 'display:none');
                $('#trDischargeOtherReason').attr('style', 'display:none');
                $("#trAppointment").hide();
                $('#trInpatientPhysician').attr('style', 'display:none');
                $('#trAppointment').attr('style', 'display:none');
                $("#tblReferralNotes").hide();
                $('#trDischargeRemarks').removeAttr('style');
            }
            else {
                $('#trDischargeReason').attr('style', 'display:none');
                $('#trReferralToDateTime').attr('style', 'display:none');
                $('#trReferrerGroup').attr('style', 'display:none');
                $('#trReferrer').attr('style', 'display:none');
                $('#trDischargeOtherReason').attr('style', 'display:none');
                $("#trAppointment").hide();
                $('#trInpatientPhysician').attr('style', 'display:none');
                $('#trParamedicHasSchedule').attr('style', 'display:none');
                $("#tblReferralNotes").hide();
                $('#trDischargeRemarks').attr('style', 'display:none');
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

        $('#<%:lbReferrerCode.ClientID %>.lblLink').live('click', function () {
            var filterExpression = onCboReferrerGroupValueChanged() + " AND IsDeleted = 0"
            openSearchDialog('referrer2', filterExpression, function (value) {
                $('#<%=txtReferrerCode.ClientID %>').val(value);
                onTxtReferrerCodeChanged(value);
            });
        });

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

        //#region Physician
        function getPhysicianFilterExpression() {
            var isCheckedFilter = $('#<%=chkParamedicHasSchedule.ClientID %>').is(":checked");
            var rblSelected = $('#<%=rblReferralType.ClientID %> input:checked').val();
            var polyclinicID = cboClinic.GetValue();
            var filterExpression = '';

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
                    filterExpression = "ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = '" + polyclinicID + "') AND IsDeleted = 0 AND (IsHasPhysicianRole = 1) AND ParamedicID IN (SELECT ParamedicID FROM vParamedicSchedule WHERE HealthcareServiceUnitID = '" + polyclinicID + "' AND DayNumber = '" + daynumber + "' UNION ALL SELECT ParamedicID FROM vParamedicScheduleDate WHERE HealthcareServiceUnitID = '" + polyclinicID + "' AND ScheduleDate = '" + registrationDateFormatString + "')";
                }
                else {
                    filterExpression = "ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = '" + polyclinicID + "') AND IsDeleted = 0 AND (IsHasPhysicianRole = 1) AND ParamedicID IN (SELECT ParamedicID FROM vParamedicSchedule WHERE HealthcareServiceUnitID = '" + polyclinicID + "' AND DayNumber = '" + daynumber + "' AND (('" + formattedTime + "' BETWEEN StartTime1 AND EndTime1) OR ('" + formattedTime + "' BETWEEN StartTime2 AND EndTime2) OR ('" + formattedTime + "' BETWEEN StartTime3 AND EndTime3) OR ('" + formattedTime + "' BETWEEN StartTime4 AND EndTime4) OR ('" + formattedTime + "' BETWEEN StartTime5 AND EndTime5)) UNION ALL SELECT ParamedicID FROM vParamedicScheduleDate WHERE HealthcareServiceUnitID = '" + polyclinicID + "' AND ScheduleDate = '" + registrationDateFormatString + "' AND (('" + formattedTime + "' BETWEEN StartTime1 AND EndTime1) OR ('" + formattedTime + "' BETWEEN StartTime2 AND EndTime2) OR ('" + formattedTime + "' BETWEEN StartTime3 AND EndTime3) OR ('" + formattedTime + "' BETWEEN StartTime4 AND EndTime4) OR ('" + formattedTime + "' BETWEEN StartTime5 AND EndTime5)))";
                }
            }
            else {
                if (polyclinicID != '' && polyclinicID != null) {
                    filterExpression = "ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = " + polyclinicID + ") AND IsDeleted = 0 AND (IsHasPhysicianRole = 1)";
                }
            }
            return filterExpression;
        }

        $('#lblPhysician.lblLink').live('click', function () {
            var isCheckedFilter = $('#<%=chkParamedicHasSchedule.ClientID %>').is(":checked");
            var polyclinicID = cboClinic.GetValue();
            if (polyclinicID != '' && polyclinicID != null) {
                openSearchDialog('paramedic', getPhysicianFilterExpression(), function (value) {
                    $('#<%=txtPhysicianCode.ClientID %>').val(value);
                    onTxtPatientVisitPhysicianCodeChanged(value);
                });
            }
            else {
                showToast("Warning", "Silahkan Pilih Klinik Rujukan Terlebih Dahulu");
            }
        });

        $('#<%=txtPhysicianCode.ClientID %>').live('change', function () {
            var isCheckedFilter = $('#<%=chkParamedicHasSchedule.ClientID %>').is(":checked");
            var polyclinicID = cboClinic.GetValue();
            if (polyclinicID != '' && polyclinicID != null) {
                onTxtPatientVisitPhysicianCodeChanged($(this).val());
            }
            else {
                $('#<%=txtPhysicianCode.ClientID %>').val('');
                showToast("Warning", "Silahkan Pilih Klinik Rujukan Terlebih Dahulu");
            }
        });

        function onTxtPatientVisitPhysicianCodeChanged(value) {
            var rblSelected = $('#<%=rblReferralType.ClientID %> input:checked').val();
            var filterExpression = getPhysicianFilterExpression() + " AND ParamedicCode = '" + value + "' AND IsDeleted = 0";
            Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
                if (result != null) {
                    var date = new Date();
                    var registrationDateFormatString = Methods.dateToString(date);
                    if (rblSelected == "2") {
                        registrationDate = $('#<%:txtAppointmentDate.ClientID %>').val();
                        registrationDateInDatePicker = Methods.getDatePickerDate(registrationDate);
                        registrationDateFormatString = Methods.dateToString(registrationDateInDatePicker);
                    }

                    var filterExpressionLeave = "IsDeleted = 0 AND ('" + registrationDateFormatString + "' BETWEEN StartDate AND EndDate ) AND ParamedicCode = '" + value + "'";
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

        $('#lblParamedic2.lblLink').live('click', function () {
            openSearchDialog('paramedic', " GCParamedicMasterType = '" + Constant.ParamedicType.Physician + "'", function (value) {
                $('#<%=txtParamedicCode.ClientID %>').val(value);
                onTxtParamedicCodeChanged(value);
            });
        });

        function onTxtParamedicCodeChanged(value) {
            var filterExpression = " ParamedicCode = '" + value + "' AND IsDeleted = 0";
            Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnParamedicID2.ClientID %>').val(result.ParamedicID);
                    $('#<%=txtParamedicName.ClientID %>').val(result.ParamedicName);
                }
                else {
                    $('#<%=hdnParamedicID2.ClientID %>').val('');
                    $('#<%=txtParamedicCode.ClientID %>').val('');
                    $('#<%=txtParamedicName.ClientID %>').val('');
                }
            });
        }
        //#endregion

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
                    cbpView.PerformCallback('getdaynumber');
                }
                else {
                    cbpView.PerformCallback('getdaynumber');
                }
            }
            else {
                $('#<%=txtAppointmentDate.ClientID %>').val(dateToday);
                cbpView.PerformCallback('getdaynumber');
            }
        });

        $('#<%:chkParamedicHasSchedule.ClientID %>').live('change', function () {
            $('#<%:hdnPhysicianID.ClientID %>').val('');
            $('#<%:txtPhysicianCode.ClientID %>').val('');
            $('#<%:txtPhysicianName.ClientID %>').val('');
            cboClinic.SetValue('');
        });

        function onLedDiagnoseLostFocus(led) {
            var diagnoseID = led.GetValueText();
            $('#<%=hdnDiagnoseID.ClientID %>').val(diagnoseID);
            if ($('#<%=txtDiagnosisText.ClientID %>').val() == "") {
                $('#<%=txtDiagnosisText.ClientID %>').val(led.GetDisplayText());
            }
        }

        $('.lnkHistoryClosedReopenBilling').live('click', function () {
            var param = $('#<%=hdnRegistrationID.ClientID %>').val();
            var url = ResolveUrl("~/Libs/Program/Information/InfoHistoryRegistrationCtl.ascx");
            openUserControlPopup(url, param, 'Registration History', 900, 500);
        });

    </script>
    <div>
        <input type="hidden" id="hdnLOSInDay" runat="server" />
        <input type="hidden" id="hdnLOSInHour" runat="server" />
        <input type="hidden" id="hdnLOSInMinute" runat="server" />
        <input type="hidden" id="hdnGCReferrerGroup" value="" runat="server" />
        <input type="hidden" id="hdnRegistrationID" runat="server" />
        <input type="hidden" id="hdnRegistrationDate" runat="server" />
        <input type="hidden" id="hdnRegistrationTime" runat="server" />
        <input type="hidden" id="hdnIsValidateParamedicSchedule" value="0" runat="server" />
        <input type="hidden" id="hdnIsHasPatientReferral" value="0" runat="server" />
        <input type="hidden" id="hdnIsHasAppointmentRequest" value="0" runat="server" />
        <input type="hidden" id="hdnIsParamedicInRegistrationUseSchedule" value="0" runat="server" />
        <input type="hidden" id="hdnDateToday" value="0" runat="server" />
        <table class="tblContentArea">
            <colgroup>
                <col style="width: 450px" />
                <col />
            </colgroup>
            <tr>
                <td style="padding: 5px; vertical-align: top;">
                    <fieldset id="fsPatientDischarge">
                        <table class="tblEntryContent" id="tblEntry" style="width: 600px">
                            <colgroup>
                                <col style="width: 200px" />
                                <col style="width: 150px" />
                            </colgroup>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkIsNeedCodification" runat="server" Text=" Diperlukan Proses Kodefikasi" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Tanggal")%>
                                        -
                                        <%=GetLabel("Jam")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDischargeDate" Width="120px" CssClass="datepicker" runat="server" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDischargeTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Lama Kunjungan")%></label>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtLengthOfVisit" ReadOnly="true" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkIsDead" runat="server" Text="Meninggal" />
                                </td>
                            </tr>
                            <tr class="trDeathInfo" id="trDeathInfo" runat="server" style="display: none">
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Death Date - Time")%></label>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtDateOfDeath" CssClass="datepicker" Width="120px" />
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtTimeOfDeath" CssClass="time" Width="80px" MaxLength="5" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Keadaan Keluar")%></label>
                                </td>
                                <td colspan="2">
                                    <dxe:ASPxComboBox ID="cboPatientOutcome" ClientInstanceName="cboPatientOutcome" Width="100%"
                                        runat="server" />
                                    <dxe:ASPxComboBox ID="cboPatientOutcomeDead" ClientInstanceName="cboPatientOutcomeDead"
                                        Width="100%" runat="server" ClientVisible="false">
                                        <ClientSideEvents Init="function(s,e){ onDischargeDateTimeChange(); }" />
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Cara Keluar")%></label>
                                </td>
                                <td colspan="2">
                                    <dxe:ASPxComboBox ID="cboDischargeRoutine" Width="100%" runat="server" ClientInstanceName="cboDischargeRoutine">
                                        <ClientSideEvents Init="function(s,e){ onCboDischargeRoutineValueChanged(s); }" ValueChanged="function(s,e){ onCboDischargeRoutineValueChanged(); }" />
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr id="trDischargeRemarks" style="display: none">
                                <td>
                                    <label>
                                        <%=GetLabel("Keterangan Cara Keluar")%></label>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtDischargeRemarks" Width="100%" runat="server" TextMode="MultiLine"
                                        Rows="3" />
                                </td>
                            </tr>
                            <tr id="trAppointment" style="display: none">
                                <td colspan="5">
                                    <table style="width: 100%" cellpadding="1" cellspacing="1">
                                        <colgroup>
                                            <col style="width: 200px" />
                                            <col style="width: 150px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblNormal" id="Label3">
                                                    <%=GetLabel("Jenis Rujukan")%></label>
                                            </td>
                                            <td colspan="2">
                                                <asp:RadioButtonList ID="rblReferralType" runat="server" RepeatDirection="Horizontal">
                                                    <asp:ListItem Text="Kunjungan Langsung" Value="1" />
                                                    <asp:ListItem Text="Perjanjian (Appointment)" Value="2" Selected="True" />
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblMandatory">
                                                    <%=GetLabel("Klinik Rujukan")%></label>
                                            </td>
                                            <td colspan="2">
                                                <dxe:ASPxComboBox runat="server" ID="cboClinic" ClientInstanceName="cboClinic" Width="100%" />
                                            </td>
                                        </tr>
                                        <tr id="trParamedicHasSchedule" runat="server">
                                            <td>
                                            </td>
                                            <td colspan="2">
                                                <asp:CheckBox ID="chkParamedicHasSchedule" runat="server" /><%:GetLabel("Tampilkan Hanya Dokter Yang Punya Jadwal")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblLink lblMandatory" id="lblPhysician">
                                                    <%=GetLabel("Dokter ")%></label>
                                            </td>
                                            <td>
                                                <input type="hidden" id="hdnPhysicianID" value="" runat="server" />
                                                <asp:TextBox ID="txtPhysicianCode" CssClass="required" Width="100%" runat="server" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPhysicianName" ReadOnly="true" Width="100%" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblMandatory">
                                                    <%=GetLabel("Tanggal Perjanjian")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtAppointmentDate" CssClass="datepicker" Width="120px" />
                                            </td>
                                        </tr>
                                        <tr style="display: none">
                                            <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                                <label class="lblNormal">
                                                    <%=GetLabel("Remarks")%></label>
                                            </td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtRemarks" Width="100%" runat="server" TextMode="MultiLine" Height="220px" />
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
                                    <dxe:ASPxComboBox ID="cboReferrerGroup" Width="100%" runat="server" ClientInstanceName="cboReferrerGroup">
                                        <ClientSideEvents Init="function(s,e){ onCboReferrerGroupValueChanged(s); }" ValueChanged="function(s,e){ onCboReferrerGroupValueChanged(); }" />
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr id="trReferrer" style="display: none">
                                <td class="tdLabel">
                                    <label class="lblLink" id="lbReferrerCode" runat="server">
                                        <%:GetLabel("Rumah Sakit / Faskes")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnReferrerID" value="" runat="server" />
                                    <table style="width: 180%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 80px" />
                                            <col style="width: 3px" />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtReferrerCode" Width="100%" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtReferrerName" Width="100%" ReadOnly="true" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="trReferralToDateTime" style="display: none">
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Tanggal Rujukan")%>
                                        -
                                        <%=GetLabel("Jam Rujukan")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtReferralToDate" Width="120px" CssClass="datepicker" runat="server" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtReferralToTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" />
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr id="trDischargeReason" style="display: none">
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Alasan Ke Rumah Sakit lain")%></label>
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
                                    <asp:TextBox ID="txtDischargeOtherReason" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr id="trInpatientPhysician" style="display: none">
                                <td class="tdLabel">
                                    <label class="lblNormal lblLink" id="lblParamedic2">
                                        <%=GetLabel("DPJP")%></label>
                                </td>
                                <td colspan="4">
                                    <table border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="width: 100px">
                                                <input type="hidden" id="hdnParamedicID2" value="" runat="server" />
                                                <asp:TextBox runat="server" ID="txtParamedicCode" Width="100%" />
                                            </td>
                                            <td style="width: 255px">
                                                <asp:TextBox runat="server" ID="txtParamedicName" ReadOnly="true" Width="100%" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </td>
                <%--<td style="vertical-align: top" rowspan="2">
                    <div id="divAppointment" style="display: none">
                        <table style="width: 100%">
                            <colgroup>
                                <col style="width: 180px" />
                                <col style="width: 150px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Clinic")%></label>
                                </td>
                                <td colspan="2">
                                    <dxe:ASPxComboBox runat="server" ID="cboClinic" ClientInstanceName="cboClinic" Width="100%" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink lblMandatory" id="lblPhysician">
                                        <%=GetLabel("Physician")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnPhysicianID" value="" runat="server" />
                                    <asp:textbox id="txtPhysicianCode" cssclass="required" width="100%" runat="server" />
                                </td>
                                <td colspan="2">
                                    <asp:textbox id="txtPhysicianName" readonly="true" width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Appointment Date")%></label>
                                </td>
                                <td>
                                    <asp:textbox runat="server" id="txtAppointmentDate" cssclass="datepicker" width="120px" />
                                </td>
                            </tr>
                            <tr style="display: none">
                                <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                    <label class="lblNormal">
                                        <%=GetLabel("Remarks")%></label>
                                </td>
                                <td colspan="2">
                                    <asp:textbox id="txtRemarks" width="100%" runat="server" textmode="MultiLine" height="220px" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>--%>
                <td style="vertical-align: top" rowspan="2">
                    <div id="tblReferralNotes" style="display: none">
                        <table style="width: 100%">
                            <colgroup>
                                <col style="width: 50px" />
                                <col style="width: 150px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td colspan="2">
                                    <label class="lblNormal">
                                        <%=GetLabel("Diagnosis Pasien :")%></label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 30%" />
                                            <col style="width: 70%" />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <label class="lblNormal" id="lblDiagnoseID" runat="server">
                                                    <%:GetLabel("ICD X")%></label>
                                            </td>
                                            <td>
                                                <table style="width: 400%" cellpadding="0" cellspacing="0">
                                                    <colgroup>
                                                        <col style="width: 50px" />
                                                    </colgroup>
                                                    <tr>
                                                        <td>
                                                            <input type="hidden" id="hdnDiagnoseID" value="" runat="server" />
                                                            <qis:QISSearchTextBox ID="ledDiagnose" ClientInstanceName="ledDiagnose" runat="server"
                                                                Width="100%" ValueText="DiagnoseID" FilterExpression="IsDeleted = 0 AND IsNutritionDiagnosis = 0"
                                                                DisplayText="DiagnoseName" MethodName="GetDiagnosisList">
                                                                <ClientSideEvents ValueChanged="function(s){ onLedDiagnoseLostFocus(s); }" />
                                                                <Columns>
                                                                    <qis:QISSearchTextBoxColumn Caption="Diagnose Name (Code)" FieldName="DiagnoseName"
                                                                        Description="i.e. Cholera" Width="500px" />
                                                                </Columns>
                                                            </qis:QISSearchTextBox>
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
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 30%" />
                                            <col style="width: 70%" />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <label class="lblNormal" id="Label5" runat="server">
                                                    <%:GetLabel("Diagnosis Text")%></label>
                                            </td>
                                            <td>
                                                <table style="width: 400%" cellpadding="0" cellspacing="0">
                                                    <colgroup>
                                                        <col style="width: 50px" />
                                                    </colgroup>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:TextBox ID="txtDiagnosisText" Width="100%" runat="server" TextMode="MultiLine"
                                                                Rows="2" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <label class="lblNormal">
                                        <%=GetLabel("Pemeriksaan Fisik / Pemeriksaan Penunjang / Catatan Medis lain yang perlu mendapat perhatian :")%></label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:TextBox ID="txtMedicalResumeText" Width="95%" runat="server" TextMode="MultiLine"
                                        Rows="6" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <label class="lblNormal">
                                        <%=GetLabel("Terapi yang telah diberikan :")%></label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:TextBox ID="txtPlanningResumeText" Width="95%" runat="server" TextMode="MultiLine"
                                        Rows="6" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div>
        <table width="100%">
            <tr>
                <td>
                    <div style="width: 70%;">
                        <div class="pageTitle" style="text-align: center">
                            <%=GetLabel("Informasi")%></div>
                        <div style="background-color: #EAEAEA;">
                            <table width="70%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col width="300px" />
                                    <col width="20px" />
                                    <col width="400px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td align="left">
                                        <%=GetLabel("Status Pendaftaran")%>
                                    </td>
                                    <td align="center">
                                        :
                                    </td>
                                    <td>
                                        <div style="font-size: medium; font-style: oblique; font-weight:bold" runat="server" id="divRegistrationStatus" />
                                    </td>
                                    <td align="left">
                                        <img class="lnkHistoryClosedReopenBilling imgLink" title="<%=GetLabel("Registration History") %>"
                                            src='<%= ResolveUrl("~/Libs/Images/Toolbar/registration_notes.png")%>' alt=""
                                            width="25px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <%=GetLabel("Dipulangkan (Dokter) Oleh")%>
                                    </td>
                                    <td align="center">
                                        :
                                    </td>
                                    <td>
                                        <div runat="server" id="divPhysicianDischargedBy" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <%=GetLabel("Dipulangkan (Dokter) Pada")%>
                                    </td>
                                    <td align="center">
                                        :
                                    </td>
                                    <td>
                                        <div runat="server" id="divPhysicianDischargedDate" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <%=GetLabel("Dipulangkan (Ruangan) Oleh")%>
                                    </td>
                                    <td align="center">
                                        :
                                    </td>
                                    <td>
                                        <div runat="server" id="divRoomDischargedBy" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <%=GetLabel("Dipulangkan (Ruangan) Pada")%>
                                    </td>
                                    <td align="center">
                                        :
                                    </td>
                                    <td>
                                        <div runat="server" id="divRoomDischargedDate" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <%=GetLabel("Dipulangkan (Perawat/Admin/Rekam Medis) Oleh")%>
                                    </td>
                                    <td align="center">
                                        :
                                    </td>
                                    <td>
                                        <div runat="server" id="divAdminDischargedBy" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <%=GetLabel("Dipulangkan (Perawat/Admin/Rekam Medis) Pada")%>
                                    </td>
                                    <td align="center">
                                        :
                                    </td>
                                    <td>
                                        <div runat="server" id="divAdminDischargedDate" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="0%" ClientInstanceName="cbpView"
        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
        <ClientSideEvents EndCallback="function(s,e) { hideLoadingPanel(); }" />
        <PanelCollection>
            <dx:PanelContent ID="PanelContent1" runat="server">
                <asp:Panel runat="server" ID="pnlReferrerGrdView" Style="width: 0%">
                    <input type="hidden" runat="server" id="hdnDayNumber" value="" />
                </asp:Panel>
            </dx:PanelContent>
        </PanelCollection>
    </dxcp:ASPxCallbackPanel>
</asp:Content>
