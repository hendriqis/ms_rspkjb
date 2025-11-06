<%@ Page Language="C#" MasterPageFile="~/MasterPage/MPPatientPageList.master" AutoEventWireup="true"
    CodeBehind="PatientDischargeList.aspx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.PatientDischargeList" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
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

            $('#<%=txtDischargeDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));
            $('#<%=txtDateOfDeath.ClientID %>').datepicker('option', 'maxDate', '0');
            $('#<%=txtAppointmentDate.ClientID %>').datepicker('option', 'minDate', '0');
            $('#<%=txtReferralToDate.ClientID %>').datepicker('option', 'minDate', '0');

            $('#<%=btnPatientDischargeProcess.ClientID %>').click(function (evt) {
                if (IsValid(evt, 'fsPatientDischarge', 'mpPatientDischarge')) {
                    var message = "Lanjutkan proses discharge pasien ?";
                    if (cboDischargeRoutine.GetValue() == Constant.DischargeRoutine.REFER_TO_OUTPATIENT) {
                        var polyclinicID = cboClinic.GetValue();
                        var paramedicID = $('#<%=hdnPhysicianID.ClientID %>').val();
                        if (polyclinicID != '' && polyclinicID != null && paramedicID != '') {
                            showToastConfirmation(message, function (result) {
                                if (result) onCustomButtonClick('process'); ;
                            });
                        }
                        else {
                            showToast("Warning", "Silahkan Pilih Klinik Rujukan dan dokter Terlebih Dahulu");
                        }
                    }
                    else if (cboDischargeRoutine.GetValue() == Constant.DischargeRoutine.OTHER_HOSPITAL) {
                        var hdnReferrerID = $('#<%=hdnReferrerID.ClientID %>').val();
                        if (validateTime($('#<%=txtReferralToTime.ClientID %>').val()) == true) {
                            if ($('#<%=txtReferralToDate.ClientID %>').val() != '') {
                                if (cboDischargeReason.GetValue() != '') {
                                    showToastConfirmation(message, function (result) {
                                        if (result) onCustomButtonClick('process'); ;
                                    });
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
                    else if (cboDischargeRoutine.GetValue() == Constant.DischargeRoutine.REFER_TO_INPATIENT) {
                        if ($('#<%=txtParamedicCode.ClientID %>').val() != '') {
                            onCustomButtonClick('process');
                        }
                        else {
                            displayErrorMessageBox('ERROR', "Dokter DPJP tidak boleh kosong");
                        }
                    }
                    else {
                        showToastConfirmation(message, function (result) {
                            if (result) onCustomButtonClick('process'); ;
                        });
                    }
                }
            });

            $('#<%=txtDischargeDate.ClientID %>').change(function () {
                onDischargeDateTimeChange();
            });
            $('#<%=txtDischargeTime.ClientID %>').change(function () {
                onDischargeDateTimeChange();
            });

            onDischargeDateTimeChange();
            onCboPatientOutcomeChanged();
            onCboDischargeRoutineChanged();

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

        function onCboPatientOutcomeChanged() {
            if (cboPatientOutcome.GetValue() != null && (cboPatientOutcome.GetValue() == Constant.PatientOutcome.DEAD_BEFORE_48 || cboPatientOutcome.GetValue() == Constant.PatientOutcome.DEAD_AFTER_48)) {
                $('#tblDischarge tr.trDeathInfo').show();
            }
            else {
                $('#tblDischarge tr.trDeathInfo').hide();
            }
        }

        function onCboDischargeReasonValueChanged() {
            if (cboDischargeReason.GetValue() == Constant.DischargeReasonToOtherHospital.OTHER) {
                $('#trDischargeOtherReason').removeAttr('style');
            }
            else {
                $('#trDischargeOtherReason').attr('style', 'display:none');
            }
        }

        function onCboDischargeRoutineChanged() {
            var isParamedicInRegistrationUseSchedule = $('#<%=hdnIsParamedicInRegistrationUseSchedule.ClientID %>').val();
            if (cboDischargeRoutine.GetValue() != null && (cboDischargeRoutine.GetValue() == Constant.DischargeRoutine.OTHER_HOSPITAL || cboDischargeRoutine.GetValue() == Constant.DischargeRoutine.REFER_TO_OUTPATIENT || cboDischargeRoutine.GetValue() == Constant.DischargeRoutine.REFER_TO_INPATIENT || cboDischargeRoutine.GetValue() == Constant.DischargeRoutine.FORCE_DISCHARGE)) {
                if (cboDischargeRoutine.GetValue() == Constant.DischargeRoutine.OTHER_HOSPITAL) {
                    $('#tblDischarge tr.trInpatientPhysician').hide();
                    $('#tblDischarge tr.trInpatientPhysician1').hide();
                    $('#tblDischarge tr.trInpatientPhysician2').hide();
                    $('#tblDischarge tr.trInpatientPhysician3').hide();
                    $('#tblDischarge tr.trInpatientPhysician4').hide();
                    $("#tblDischarge tr.trAppointment").hide();
                    $("#tblReferralNotes").show();
                    $('#<%=hdnParamedicID2.ClientID %>').val('');
                    $('#<%=txtParamedicCode.ClientID %>').val('');
                    $('#<%=txtParamedicName.ClientID %>').val('');
                    $('#trReferrerGroup').removeAttr('style');
                    $('#trReferrer').removeAttr('style');
                    $('#trDischargeReason').removeAttr('style');
                    $('#trReferralToDateTime').removeAttr('style');
                    $('#trDischargeOtherReason').attr('style', 'display:none');
                    $('#trParamedicHasSchedule').attr('style', 'display:none');
                    $('#trDischargeRemarks').attr('style', 'display:none');
                }
                else if (cboDischargeRoutine.GetValue() == Constant.DischargeRoutine.REFER_TO_OUTPATIENT) {
                    $('#tblDischarge tr.trInpatientPhysician').hide();
                    $('#tblDischarge tr.trInpatientPhysician1').hide();
                    $('#tblDischarge tr.trInpatientPhysician2').hide();
                    $('#tblDischarge tr.trInpatientPhysician3').hide();
                    $('#tblDischarge tr.trInpatientPhysician4').hide();
                    $("#tblDischarge tr.trAppointment").show();
                    $("#tblReferralNotes").show();
                    $('#<%=hdnParamedicID2.ClientID %>').val('');
                    $('#<%=txtParamedicCode.ClientID %>').val('');
                    $('#<%=txtParamedicName.ClientID %>').val('');
                    $('#trReferrerGroup').attr('style', 'display:none');
                    $('#trReferrer').attr('style', 'display:none');
                    $('#trDischargeReason').attr('style', 'display:none');
                    $('#trReferralToDateTime').attr('style', 'display:none');
                    $('#trDischargeOtherReason').attr('style', 'display:none');
                    $('#trDischargeRemarks').attr('style', 'display:none');

                    if (isParamedicInRegistrationUseSchedule == "1") {
                        $('#trParamedicHasSchedule').removeAttr('style');
                    }
                }
                else if (cboDischargeRoutine.GetValue() == Constant.DischargeRoutine.REFER_TO_INPATIENT) {
                    $("#tblDischarge tr.trAppointment").hide();
                    $("#tblReferralNotes").hide();
                    $('#tblDischarge tr.trInpatientPhysician').show();
                    $('#tblDischarge tr.trInpatientPhysician1').show();
                    $('#tblDischarge tr.trInpatientPhysician2').show();
                    $('#tblDischarge tr.trInpatientPhysician3').show();
                    $('#tblDischarge tr.trInpatientPhysician4').show();
                    $('#<%=hdnParamedicID2.ClientID %>').val($('#<%=hdnIPRegisteredPhysicianID.ClientID %>').val());
                    $('#<%=txtParamedicCode.ClientID %>').val($('#<%=hdnIPRegisteredPhysicianCode.ClientID %>').val());
                    $('#<%=txtParamedicName.ClientID %>').val($('#<%=hdnIPRegisteredPhysicianName.ClientID %>').val());
                    $('#trReferrerGroup').attr('style', 'display:none');
                    $('#trReferrer').attr('style', 'display:none');
                    $('#trDischargeReason').attr('style', 'display:none');
                    $('#trReferralToDateTime').attr('style', 'display:none');
                    $('#trDischargeOtherReason').attr('style', 'display:none');
                    $('#trParamedicHasSchedule').attr('style', 'display:none');
                    $('#trDischargeRemarks').attr('style', 'display:none');
                }
                else if (cboDischargeRoutine.GetValue() == Constant.DischargeRoutine.FORCE_DISCHARGE) {
                    $("#tblDischarge tr.trAppointment").hide();
                    $("#tblReferralNotes").hide();
                    $('#tblDischarge tr.trInpatientPhysician').hide();
                    $('#tblDischarge tr.trInpatientPhysician1').hide();
                    $('#tblDischarge tr.trInpatientPhysician2').hide();
                    $('#tblDischarge tr.trInpatientPhysician3').hide();
                    $('#tblDischarge tr.trInpatientPhysician4').hide();
                    $('#<%=hdnParamedicID2.ClientID %>').val('');
                    $('#<%=txtParamedicCode.ClientID %>').val('');
                    $('#<%=txtParamedicName.ClientID %>').val('');
                    $('#trReferrerGroup').attr('style', 'display:none');
                    $('#trReferrer').attr('style', 'display:none');
                    $('#trDischargeReason').attr('style', 'display:none');
                    $('#trReferralToDateTime').attr('style', 'display:none');
                    $('#trParamedicHasSchedule').attr('style', 'display:none');
                    $('#trDischargeRemarks').removeAttr('style');
                }
                else {
                    $("#tblDischarge tr.trAppointment").hide();
                    $("#tblReferralNotes").hide();
                    $('#tblDischarge tr.trInpatientPhysician').show();
                    $('#tblDischarge tr.trInpatientPhysician1').show();
                    $('#tblDischarge tr.trInpatientPhysician2').show();
                    $('#tblDischarge tr.trInpatientPhysician3').show();
                    $('#tblDischarge tr.trInpatientPhysician4').show();
                    $('#<%=hdnParamedicID2.ClientID %>').val($('').val());
                    $('#<%=txtParamedicCode.ClientID %>').val($('').val());
                    $('#<%=txtParamedicName.ClientID %>').val($('').val());
                    $('#trReferrerGroup').attr('style', 'display:none');
                    $('#trReferrer').attr('style', 'display:none');
                    $('#trDischargeReason').attr('style', 'display:none');
                    $('#trReferralToDateTime').attr('style', 'display:none');
                    $('#trDischargeOtherReason').attr('style', 'display:none');
                    $('#trParamedicHasSchedule').attr('style', 'display:none');
                    $('#trDischargeRemarks').attr('style', 'display:none');
                }
            }
            else {
                $("#tblDischarge tr.trAppointment").hide();
                $("#tblReferralNotes").hide();
                $('#tblDischarge tr.trInpatientPhysician').hide();
                $('#tblDischarge tr.trInpatientPhysician1').hide();
                $('#tblDischarge tr.trInpatientPhysician2').hide();
                $('#tblDischarge tr.trInpatientPhysician3').hide();
                $('#tblDischarge tr.trInpatientPhysician4').hide();
                $('#<%=hdnParamedicID2.ClientID %>').val('');
                $('#<%=txtParamedicCode.ClientID %>').val('');
                $('#<%=txtParamedicName.ClientID %>').val('');
                $('#trReferrerGroup').attr('style', 'display:none');
                $('#trReferrer').attr('style', 'display:none');
                $('#trDischargeReason').attr('style', 'display:none');
                $('#trReferralToDateTime').attr('style', 'display:none');
                $('#trParamedicHasSchedule').attr('style', 'display:none');
                $('#trDischargeRemarks').attr('style', 'display:none');
            }
        }

        //#region Referrer
        function onCboReferrerGroupValueChanged() {
            if ($('#<%=hdnGCReferrerGroup.ClientID %>').val() != cboReferrerGroup.GetValue()) {
                var visitID = $('#<%=hdnVisitID.ClientID %>').val();
                var filterExpression = "VisitID = '" + visitID + "'";
                Methods.getObject('GetConsultVisitList', filterExpression, function (result) {
                    if (result != null) {
                        if (result.ReferralTo == "" || result.ReferralTo == "0") {
                            $('#<%=hdnReferrerID.ClientID %>').val('');
                            $('#<%=txtReferrerCode.ClientID %>').val('');
                            $('#<%=txtReferrerName.ClientID %>').val('');
                        }
                    }
                });
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

        $('#lblParamedic2.lblLink').live('click', function () {
            openSearchDialog('paramedic', " GCParamedicMasterType = '" + Constant.ParamedicType.Physician + "'", function (value) {
                $('#<%=txtParamedicCode.ClientID %>').val(value);
                onTxtParamedicCodeChanged(value);
            });
        });

        $('#lblPhysician.lblLink').live('click', function () {
            var rblSelected = $('#<%=rblReferralType.ClientID %> input:checked').val();
            var isCheckedFilter = $('#<%=chkParamedicHasSchedule.ClientID %>').is(":checked");
            var polyclinicID = cboClinic.GetValue();
            if (polyclinicID != '' && polyclinicID != null) {
                if (rblSelected == "2") {
                    if ($('#<%:txtAppointmentDate.ClientID %>').val() != '') {
                        openSearchDialog('paramedic', getPhysicianFilterExpression(), function (value) {
                            $('#<%=txtPhysicianCode.ClientID %>').val(value);
                            onTxtPatientVisitPhysicianCodeChanged(value);
                        });
                    }
                    else {
                        showToast("Warning", "Silahkan Pilih Tanggal Perjanjian Terlebih Dahulu");
                    }
                }
                else {
                    openSearchDialog('paramedic', getPhysicianFilterExpression(), function (value) {
                        $('#<%=txtPhysicianCode.ClientID %>').val(value);
                        onTxtPatientVisitPhysicianCodeChanged(value);
                    });
                }
            }
            else {
                showToast("Warning", "Silahkan Pilih Klinik Rujukan Terlebih Dahulu");
            }
        });

        $('#<%=txtPhysicianCode.ClientID %>').live('change', function () {
            var rblSelected = $('#<%=rblReferralType.ClientID %> input:checked').val();
            var isCheckedFilter = $('#<%=chkParamedicHasSchedule.ClientID %>').is(":checked");
            var polyclinicID = cboClinic.GetValue();
            if (polyclinicID != '' && polyclinicID != null) {
                if (rblSelected == "2") {
                    if ($('#<%:txtAppointmentDate.ClientID %>').val() != '') {
                        onTxtPatientVisitPhysicianCodeChanged($(this).val());
                    }
                    else {
                        $('#<%=txtPhysicianCode.ClientID %>').val('');
                        showToast("Warning", "Silahkan Pilih Tanggal Perjanjian Terlebih Dahulu");
                    }
                }
                else {
                    onTxtPatientVisitPhysicianCodeChanged($(this).val());
                }
            }
            else {
                $('#<%=txtPhysicianCode.ClientID %>').val('');
                showToast("Warning", "Silahkan Pilih Klinik Rujukan Terlebih Dahulu");
            }
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
        //#endregion

        function onAfterCustomClickSuccess(type) {
            if (cboDischargeRoutine.GetValue() == Constant.DischargeRoutine.OTHER_HOSPITAL) {
                Methods.getObject("GetSettingParameterDtList", "ParameterCode = 'EM0053'", function (resultSetvar) {
                    if (resultSetvar != null) {
                        if (resultSetvar.ParameterValue == "1") {
                            var registrationID = $('#<%:hdnRegistrationIDMainPage.ClientID %>').val();
                            var from = $('#<%=txtReferralToDate.ClientID %>').val();
                            var dateDay = from.substring(0, 2);
                            var dateMonth = from.substring(3, 5);
                            var dateYear = from.substring(6);
                            var newDate = dateYear + '-' + dateMonth + '-' + dateDay;
                            var referralphysicianText = "";
                            var therapyText = $('#<%=txtPlanningResumeText.ClientID %>').val();
                            var registrationID = $('#<%=hdnRegistrationIDMainPage.ClientID %>').val();
                            var visitID = $('#<%=hdnVisitID.ClientID %>').val();
                            var reportCode = 'PM-00545';
                            var filterExpression = visitID + "|" + referralphysicianText + "|" + therapyText + "|" + newDate + "|";
                            if (reportCode != "") {
                                window.setTimeout(function () {
                                    showLoadingPanel();
                                    openReportViewer(reportCode, filterExpression);
                                    window.setTimeout(function () {
                                        hideLoadingPanel();
                                        exitPatientPage();
                                    }, 8000);
                                    void 0;
                                }, 0);
                            }
                            else {
                                exitPatientPage();
                            }
                        }
                        else {
                            exitPatientPage();
                        }
                    }
                    else {
                        exitPatientPage();
                    }
                });
            }
            else {
                exitPatientPage();
            }
        }

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

        function onBeforeLoadRightPanelContent(code) {
            if (code == 'keteranganButaWarna' || code == 'keteranganIstirahat1') {
                var regID = $('#<%:hdnRegistrationIDMainPage.ClientID %>').val();
                Methods.getObject("GetvRegistrationList", "RegistrationID = " + regID, function (resultReg) {
                    if (resultReg != null) {
                        visitID = resultReg.VisitID
                    }
                });
                var param = visitID;
                return param;
            }
            else if (code == 'medicalSickLeave' || code == 'medicalSickLeaveBilingual') {
                var regID = $('#<%:hdnRegistrationIDMainPage.ClientID %>').val();
                Methods.getObject("GetvRegistrationList", "RegistrationID = " + regID, function (resultReg) {
                    if (resultReg != null) {
                        visitID = resultReg.RegistrationID
                    }
                });
                var param = visitID;
                return param;
            }
            else if (code == 'rujukanrslain' || code == 'referallLetter' || code == 'generateControlLetter') {
                var param = $('#<%:hdnVisitID.ClientID %>').val();
                return param;
            }
        }

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var visitID = $('#<%=hdnVisitID.ClientID %>').val();
            var registration_id = $('#<%:hdnRegistrationIDMainPage.ClientID %>').val();
            if (code == 'PM-90008' || code == 'PM-90009' || code == 'PM-90010' || code == 'PM-90011' || code == 'PM-90012' || code == 'PM-90013'
                || code == 'PM-90014' || code == 'PM-90015' || code == 'PM-90019' || code == 'PM-00575' || code == 'PM-90020' || code == 'PM-90022'
                || code == 'PM-90023' || code == 'PM-90035' || code == 'PM-90022' || code == 'PM-90040' || code == 'PM-90017' || code == 'MR000009'
                || code == 'MR000050' || code == 'MR000051' || code == 'PM-90063' || code == 'MR000042' || code == 'PM-90114' || code == 'PM-90117'
                || code == 'PM-90118') {
                var id = "";
                Methods.getObject("GetvMedicalResumeList", "VisitID = '" + visitID + "' AND IsDeleted = 0", function (result) {
                    if (result != null) {
                        id = result.ID;
                    }
                });
                if (id == '' || id == '0') {
                    errMessage.text = 'Pasien tidak memiliki Resume Medis';
                    return false;
                }
                else {
                    filterExpression.text = visitID;
                    return true;
                }
            } else if (code == 'PM-00751') {
                filterExpression.text = registration_id;
                return true;
            }
            else if (code == 'PM-90074' || code == 'PM-90075' || code == 'PM-90076') {
                filterExpression.text = registrationID;
                return true;
            }
            else if (code == 'PM-00545') {
                if ($('#<%=hdnVisitStatus.ClientID %>').val() == 'X020^004' && cboDischargeRoutine.GetValue() == "X052^003") {
                    var registrationID = $('#<%:hdnRegistrationIDMainPage.ClientID %>').val();
                    var from = $('#<%=txtReferralToDate.ClientID %>').val();
                    var dateDay = from.substring(0, 2);
                    var dateMonth = from.substring(3, 5);
                    var dateYear = from.substring(6);
                    var newDate = dateYear + '-' + dateMonth + '-' + dateDay;
                    var referralphysicianText = "";
                    var therapyText = $('#<%=txtPlanningResumeText.ClientID %>').val();
                    var registrationID = $('#<%=hdnRegistrationIDMainPage.ClientID %>').val();
                    var visitID = $('#<%=hdnVisitID.ClientID %>').val();
                    filterExpression.text = visitID + "|" + referralphysicianText + "|" + therapyText + "|" + newDate + "|" + "" + "|" + "";
                    return true;
                }
                else {
                    errMessage.text = "Harap cek ulang apakah registrasi sudah disposisi dan cara keluar Pindah ke Rumah Sakit Lain / Faskes Lain";
                    return false;
                }
            }
            else if (code == 'PM-00595') {
                if ($('#<%=hdnVisitStatus.ClientID %>').val() == 'X020^004' && cboDischargeRoutine.GetValue() == "X052^003") {
                    var registrationID = $('#<%:hdnRegistrationIDMainPage.ClientID %>').val();
                    var from = $('#<%=txtReferralToDate.ClientID %>').val();
                    var dateDay = from.substring(0, 2);
                    var dateMonth = from.substring(3, 5);
                    var dateYear = from.substring(6);
                    var newDate = dateYear + '-' + dateMonth + '-' + dateDay;
                    var referralphysicianText = "";
                    var therapyText = $('#<%=txtPlanningResumeText.ClientID %>').val();
                    var registrationID = $('#<%=hdnRegistrationIDMainPage.ClientID %>').val();
                    var visitID = $('#<%=hdnVisitID.ClientID %>').val();
                    filterExpression.text = visitID + "|" + referralphysicianText + "|" + therapyText + "|" + newDate + "|" + "" + "|" + "";
                    return true;
                }
                else {
                    errMessage.text = "Harap cek ulang apakah registrasi sudah disposisi dan cara keluar Pindah ke Rumah Sakit Lain / Faskes Lain";
                    return false;
                }
            }
        }

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

        function onAfterSaveAddRecordEntryPopup(param) {
            window.location.reload(true);
        }
    </script>
    <input type="hidden" id="hdnLOSInDay" runat="server" />
    <input type="hidden" id="hdnLOSInHour" runat="server" />
    <input type="hidden" id="hdnLOSInMinute" runat="server" />
    <input type="hidden" id="hdnRegistrationDateTime" runat="server" />
    <input type="hidden" value="" id="hdnDiagnosisID" runat="server" />
    <input type="hidden" value="1" id="hdnDiagnosisProcessMode" runat="server" />
    <input type="hidden" id="hdnIsMainDiagnosisExists" runat="server" value='0' />
    <input type="hidden" id="hdnDiagnosisSummary" runat="server" />
    <input type="hidden" id="hdnIPRegisteredPhysicianID" runat="server" />
    <input type="hidden" id="hdnIPRegisteredPhysicianCode" runat="server" />
    <input type="hidden" id="hdnIPRegisteredPhysicianName" runat="server" />
    <input type="hidden" value="0" runat="server" id="hdnIsValidateParamedicSchedule" />
    <input type="hidden" id="hdnRegistrationIDMainPage" runat="server" />
    <input type="hidden" id="hdnGCReferrerGroup" value="" runat="server" />
    <input type="hidden" value="" id="hdnIsParamedicInRegistrationUseSchedule" runat="server" />
    <input type="hidden" value="0" id="hdnDateToday" runat="server" />
    <input type="hidden" value="0" id="hdnIsAllowCreateAppointment" runat="server" />
    <input type="hidden" value="0" id="hdnVisitID" runat="server" />
    <input type="hidden" value="" id="hdnVisitStatus" runat="server" />
    <input type="hidden" value="" id="hdnIsUsingFlagWithoutInitialPhysicianAssessment" runat="server" />
    <input type="hidden" value="0" id="hdnIsHasChiefComplaint" runat="server" />
    <input type="hidden" value="0" id="hdnIsBridgingToMJKN" runat="server" />
    <input type="hidden" value="0" id="hdnIsDefaultDischargeConditionAndMethod" runat="server" />
    <fieldset id="fsPatientDischarge">
        <table class="tblEntryContent" border="0" cellpadding="0" cellspacing="0" style="width: 100%">
            <colgroup>
                <col width="45%" />
                <col />
            </colgroup>
            <tr>
                <td style="vertical-align: top;">
                    <table id="tblDischarge" style="width: 100%">
                        <colgroup>
                            <col style="width: 180px" />
                            <col style="width: 150px" />
                            <col style="width: 80px" />
                            <col style="width: 120px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Tanggal ")%>
                                    -
                                    <%=GetLabel("Jam ")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDischargeDate" Width="120px" CssClass="datepicker" runat="server" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtDischargeTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtLengthOfVisit" ReadOnly="true" Width="100%" runat="server" Style="text-align: center" />
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkIsComplexVisit" runat="server" Text=" Kasus Kompleks" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td colspan="4">
                                <asp:CheckBox ID="chkIsTransferredToInpatient" runat="server" Text=" Pengantar Rawat Inap" />
                            </td>
                        </tr>
                        <tr id="trIsWithoutInitialPhysicianAssessment" runat="server">
                            <td>
                            </td>
                            <td colspan="4">
                                <asp:CheckBox ID="chkIsWithoutInitialPhysicianAssessment" runat="server" Text=" Proses pulang mengabaikan isian kajian awal dan diagnosa dari DPJP ? " />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Keadaan Keluar")%></label>
                            </td>
                            <td colspan="4">
                                <dxe:ASPxComboBox ID="cboPatientOutcome" ClientInstanceName="cboPatientOutcome" Width="100%"
                                    runat="server">
                                    <ClientSideEvents ValueChanged="function(s,e){ onCboPatientOutcomeChanged(); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Cara Keluar / Tindak Lanjut")%></label>
                            </td>
                            <td colspan="4">
                                <dxe:ASPxComboBox ID="cboDischargeRoutine" ClientInstanceName="cboDischargeRoutine"
                                    Width="100%" runat="server">
                                    <ClientSideEvents ValueChanged="function(s,e){ onCboDischargeRoutineChanged(); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr id="trDischargeRemarks" style="display: none">
                            <td>
                                <label>
                                    <%=GetLabel("Keterangan Cara Keluar")%></label>
                            </td>
                            <td colspan="4">
                                <asp:TextBox ID="txtDischargeRemarks" Width="100%" runat="server" TextMode="MultiLine"
                                    Rows="3" />
                            </td>
                        </tr>
                        <tr class="trDeathInfo" style="display: none">
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Death Date - Time")%></label>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtDateOfDeath" CssClass="datepicker" Width="120px" />
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtTimeOfDeath" CssClass="time" Width="80px" />
                            </td>
                        </tr>
                        <tr class="trInpatientPhysician" style="display: none">
                            <td class="tdLabel">
                                <label class="lblMandatory lblLink" id="lblParamedic2">
                                    <%=GetLabel("DPJP Utama")%></label>
                            </td>
                            <td colspan="4">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                    <tr>
                                        <td style="width: 100px">
                                            <input type="hidden" id="hdnParamedicID2" value="" runat="server" />
                                            <asp:TextBox runat="server" ID="txtParamedicCode" Width="100%" />
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtParamedicName" ReadOnly="true" Width="100%" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr class="trInpatientPhysician1" style="display: none">
                            <td class="tdLabel">
                                <label class="lblNormal" id="Label1">
                                    <%=GetLabel("Jenis Kamar")%></label>
                            </td>
                            <td colspan="4">
                                <dxe:ASPxComboBox ID="cboRoomType" ClientInstanceName="cboRoomType" Width="100%"
                                    runat="server">
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr class="trInpatientPhysician2" style="display: none">
                            <td class="tdLabel" style="vertical-align: top">
                                <label class="lblNormal" id="Label2">
                                    <%=GetLabel("Indikasi")%></label>
                            </td>
                            <td colspan="4">
                                <asp:TextBox runat="server" ID="txtHospitalizedIndication" Width="100%" Rows="3"
                                    TextMode="MultiLine" />
                            </td>
                        </tr>
                        <tr class="trInpatientPhysician3" style="display: none">
                            <td class="tdLabel" style="vertical-align: top">
                                <label class="lblNormal" id="Label4">
                                    <%=GetLabel("Jenis Pelayanan")%></label>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkIsPreventiveCare" runat="server" />
                                <%=GetLabel("Preventif")%>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkIsCurativeCare" runat="server" />
                                <%=GetLabel("Kuratif")%>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkIsRehabilitationCare" runat="server" />
                                <%=GetLabel("Rehabilitatif")%>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkIsPalliativeCare" runat="server" />
                                <%=GetLabel("Paliatif")%>
                            </td>
                        </tr>
                        <tr class="trInpatientPhysician4" style="display: none">
                            <td />
                            <td colspan="4">
                                <asp:CheckBox ID="chkIsHospitalizedByPatientRequest" runat="server" />
                                <%=GetLabel("Masuk Rumah Sakit atas Permintaan Sendiri (Pasien/Keluarga)")%>
                            </td>
                        </tr>
                        <tr class="trAppointment" style="display: none">
                            <td colspan="5">
                                <div id="divAppointment">
                                    <table style="width: 100%" cellpadding="0" cellspacing="1">
                                        <colgroup>
                                            <col style="width: 180px" />
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
                                        <tr id="trParamedicHasSchedule">
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
                                            <td colspan="2">
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
                                </div>
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
                            <td colspan="5">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <input type="hidden" id="hdnReferrerID" value="" runat="server" />
                                            <asp:TextBox ID="txtReferrerCode" Width="150px" runat="server" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtReferrerName" Width="330px" ReadOnly="true" runat="server" />
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
                        <tr id="trRemarksToOP" style="display: none">
                            <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                <label class="lblNormal">
                                    <%=GetLabel("Remarks")%></label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtRemarksToOP" Width="100%" runat="server" TextMode="MultiLine"
                                    Height="80px" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="vertical-align: top" rowspan="2">
                    <table id="tblReferralNotes" cellpadding="0" cellspacing="0" style="width: 100%;
                        display: none">
                        <colgroup>
                            <col style="width: 150px" />
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
                </td>
            </tr>
        </table>
    </fieldset>
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
