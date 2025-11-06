<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PatientDischargeEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Inpatient.Program.PatientDischargeEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<script type="text/javascript" id="dxss_patientdischargectl">
    var registrationDateTimeInString = '<%=RegistrationDateTime%>';
    var registrationDateTime = Methods.stringToDateTime(registrationDateTimeInString);

    $(function () {
        setDatePicker('<%=txtDischargeDate.ClientID %>');
        setDatePicker('<%=txtDateOfDeath.ClientID %>');
        setDatePicker('<%=txtAppointmentDate.ClientID %>');

        $('#<%=txtDischargeDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));
        $('#<%=txtDateOfDeath.ClientID %>').datepicker('option', 'maxDate', '0');
        $('#<%=txtAppointmentDate.ClientID %>').datepicker('option', 'minDate', '0');

        $('#<%=txtDischargeDate.ClientID %>').change(function () {
            onDischargeDateTimeChange();
        });
        $('#<%=txtDischargeTime2.ClientID %>').change(function () {
            onDischargeDateTimeChange();
        });

        $('#<%=chkIsDead.ClientID %>').change(function () {
            if ($(this).is(':checked')) {
                cboPatientOutcome.SetVisible(false);
                cboPatientOutcomeDead.SetVisible(true);
                $('#tblEntry tr.trDeathInfo').show();
                cboDischargeRoutine.SetValue('<%=GetDischargeMethodToMortuary() %>');
            }
            else {
                cboPatientOutcome.SetVisible(true);
                cboPatientOutcomeDead.SetVisible(false);
                $('#tblEntry tr.trDeathInfo').hide();
                cboDischargeRoutine.SetValue('');
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

    $('#<%=btnDischargePatientBPJS.ClientID %>').live('click', function (evt) {
        onDischargePasien();
    });

    function onDischargeDateTimeChange() {
        var dischargeDate = Methods.getDatePickerDate($('#<%=txtDischargeDate.ClientID %>').val());
        var dischargeTime = $('#<%=txtDischargeTime1.ClientID %>').val() + ":" + $('#<%=txtDischargeTime2.ClientID %>').val();
        var dischargeDateTimeInString = Methods.dateToString(dischargeDate) + dischargeTime.replace(':', '');
        var diff = dateDiff(registrationDateTime, Methods.stringToDateTime(dischargeDateTimeInString));
        $('#<%=hdnLOSInDay.ClientID %>').val(diff.d);
        $('#<%=hdnLOSInHour.ClientID %>').val(diff.h);
        $('#<%=hdnLOSInMinute.ClientID %>').val(diff.m);
        $('#<%=txtLengthOfVisit.ClientID %>').val(diff.d + 'dd ' + diff.h + 'hh ' + diff.m + 'mm');

        if (diff.d < 2)
            cboPatientOutcomeDead.SetValue('<%=GetPatientOutcomeDeadBefore48() %>');
        else
            cboPatientOutcomeDead.SetValue('<%=GetPatientOutcomeDeadAfter48() %>');
    }

    function onGetEntryPopupReturnValue() {
        return '1';
    }

    function onAfterSaveRightPanelContent(code, value, isAdd) {
        var isBridgingAplicares = $('#<%=hdnIsBridgingToAplicares.ClientID %>').val();
        if (isBridgingAplicares == "1") {
            UpdateRoomAplicares();
        }
    }

    //#region APLICARES

    function UpdateRoomAplicares() {
        var paramRoomID = $('#<%=hdnRoomID.ClientID %>').val();
        var paramClassID = $('#<%=hdnClassID.ClientID %>').val();
        var filterSUR = "RoomID = " + paramRoomID + " AND ClassID = " + paramClassID + " AND IsDeleted = 0 AND IsAplicares = 1";
        Methods.getListObject('GetServiceUnitRoomList', filterSUR, function (resultSUR) {
            for (i = 0; i < resultSUR.length; i++) {
                var hsuID = resultSUR[i].HealthcareServiceUnitID;
                var classID = resultSUR[i].ClassID;
                var filterExpression = "HealthcareServiceUnitID = " + hsuID + " AND ClassID = " + classID;
                Methods.getObject('GetvServiceUnitAplicaresList', filterExpression, function (result) {
                    if (result != null) {
                        var kodeKelas = result.AplicaresClassCode;
                        var kodeRuang = result.ServiceUnitCode;
                        var namaRuang = result.ServiceUnitName;
                        var jumlahKapasitas = result.CountBedAll;
                        var jumlahKosong = result.CountBedEmpty;
                        var jumlahKosongPria = result.CountBedEmptyMale;
                        var jumlahKosongWanita = result.CountBedEmptyFemale;
                        var jumlahKosongPriaWanita = result.CountBedEmptyMale + result.CountBedEmptyFemale;

                        if (result.CountIsSendToAplicares == 0) {
                            AplicaresService.createRoom(kodeKelas, kodeRuang, namaRuang, jumlahKapasitas, jumlahKosong, jumlahKosongPria, jumlahKosongWanita, jumlahKosongPriaWanita, function (resultRoom) {
                                if (resultRoom != null) {
                                    try {
                                        AplicaresService.updateStatusSendToAplicares(result.HealthcareServiceUnitID, result.ClassID, function (resultUpdate) {
                                            if (resultUpdate != null) {
                                                try {
                                                    var resultUpdate = resultUpdate.split('|');
                                                    if (resultUpdate[0] == "1") {
                                                        showToast('INFORMATION', "SUCCESS");
                                                    }
                                                    else {
                                                        showToast('FAILED', resultUpdate[2]);
                                                    }
                                                } catch (error) {
                                                    showToast('FAILED', error);
                                                }
                                            }
                                        });
                                    } catch (err) {
                                        showToast('FAILED', err);
                                    }
                                }
                            });
                        } else {
                            AplicaresService.updateRoomStatus(kodeKelas, kodeRuang, namaRuang, jumlahKapasitas, jumlahKosong, jumlahKosongPria, jumlahKosongWanita, jumlahKosongPriaWanita, function (resultRoom) {
                                if (resultRoom != null) {
                                    try {
                                        AplicaresService.updateStatusSendToAplicares(result.HealthcareServiceUnitID, result.ClassID, function (resultUpdate) {
                                            if (resultUpdate != null) {
                                                try {
                                                    var resultUpdate = resultUpdate.split('|');
                                                    if (resultUpdate[0] == "1") {
                                                        showToast('INFORMATION', "SUCCESS");
                                                    }
                                                    else {
                                                        showToast('FAILED', resultUpdate[2]);
                                                    }
                                                } catch (error) {
                                                    showToast('FAILED', error);
                                                }
                                            }
                                        });
                                    } catch (err) {
                                        showToast('FAILED', err);
                                    }
                                }
                            });
                        }
                    }
                });
            }
        });
    }

    //#endregion

    function onCboDischargeReasonValueChanged() {
        if (cboDischargeReason.GetValue() == Constant.DischargeReasonToOtherHospital.OTHER) {
            $('#trRemarksToOP').removeAttr('style');
        }
        else {
            $('#trRemarksToOP').attr('style', 'display:none');
        }
    }

    function onCboDischargeRoutineValueChanged() {
        var isParamedicInRegistrationUseSchedule = $('#<%=hdnIsParamedicInRegistrationUseScheduleCtl.ClientID %>').val();
        if (cboDischargeRoutine.GetValue() == Constant.DischargeRoutine.OTHER_HOSPITAL) {
            cboDischargeReason.SetValue('');
            $('#trReferrerGroup').removeAttr('style');
            $('#trClinicReferralToOP').attr('style', 'display:none');
            $('#trParamedicHasScheduleCtl').attr('style', 'display:none');
            $('#trPhysicianReferralToOP').attr('style', 'display:none');
            $('#trAppointmentDateToOP').attr('style', 'display:none');
            $('#trDischargeReason').removeAttr('style');
            $('#trReferrer').removeAttr('style');
            $("#divAppointment").hide();
            $('#trInpatientPhysician').attr('style', 'display:none');
            $('#trAppointment').attr('style', 'display:none');
            $("#tblReferralNotes").hide();
            $('#trDischargeRemarks').attr('style', 'display:none');
        }
        else if (cboDischargeRoutine.GetValue() == Constant.DischargeRoutine.REFER_TO_OUTPATIENT) {
            $('#trReferrerGroup').attr('style', 'display:none');
            $('#trClinicReferralToOP').removeAttr('style');

            if (isParamedicInRegistrationUseSchedule == "1") {
                $('#trParamedicHasScheduleCtl').removeAttr('style');
            }

            $('#trPhysicianReferralToOP').removeAttr('style');
            $('#trAppointmentDateToOP').removeAttr('style');
            $('#trDischargeReason').attr('style', 'display:none');
            $('#trReferrer').attr('style', 'display:none');
            $('#trDischargeOtherReason').attr('style', 'display:none');
            $('#trInpatientPhysician').attr('style', 'display:none');
            $('#trAppointment').removeAttr('style');
            $("#tblReferralNotes").show();
            $('#trDischargeRemarks').attr('style', 'display:none');
        }
        else if (cboDischargeRoutine.GetValue() == Constant.DischargeRoutine.REFER_TO_INPATIENT) {
            $('#trReferrerGroup').attr('style', 'display:none');
            $('#trClinicReferralToOP').attr('style', 'display:none');
            $('#trParamedicHasScheduleCtl').attr('style', 'display:none');
            $('#trPhysicianReferralToOP').attr('style', 'display:none');
            $('#trAppointmentDateToOP').attr('style', 'display:none');
            $('#trDischargeReason').attr('style', 'display:none');
            $('#trReferrer').attr('style', 'display:none');
            $('#trDischargeOtherReason').attr('style', 'display:none');
            $("#divAppointment").hide();
            $('#trInpatientPhysician').removeAttr('style');
            $('#trAppointment').attr('style', 'display:none');
            $("#tblReferralNotes").hide();
            $('#trDischargeRemarks').attr('style', 'display:none');
        }
        else if (cboDischargeRoutine.GetValue() == Constant.DischargeRoutine.FORCE_DISCHARGE) {
            $('#trReferrerGroup').attr('style', 'display:none');
            $('#trClinicReferralToOP').attr('style', 'display:none');
            $('#trParamedicHasScheduleCtl').attr('style', 'display:none');
            $('#trPhysicianReferralToOP').attr('style', 'display:none');
            $('#trAppointmentDateToOP').attr('style', 'display:none');
            $('#trDischargeReason').attr('style', 'display:none');
            $('#trReferrer').attr('style', 'display:none');
            $('#trDischargeOtherReason').attr('style', 'display:none');
            $("#divAppointment").hide();
            $('#trInpatientPhysician').attr('style', 'display:none');
            $('#trAppointment').attr('style', 'display:none');
            $("#tblReferralNotes").hide();
            $('#trDischargeRemarks').removeAttr('style');
        }
        else {
            $('#trReferrerGroup').attr('style', 'display:none');
            $('#trClinicReferralToOP').attr('style', 'display:none');
            $('#trParamedicHasScheduleCtl').attr('style', 'display:none');
            $('#trPhysicianReferralToOP').attr('style', 'display:none');
            $('#trAppointmentDateToOP').attr('style', 'display:none');
            $('#trDischargeReason').attr('style', 'display:none');
            $('#trReferrer').attr('style', 'display:none');
            $('#trDischargeOtherReason').attr('style', 'display:none');
            $("#divAppointment").hide();
            $('#trInpatientPhysician').attr('style', 'display:none');
            $('#trAppointment').attr('style', 'display:none');
            $("#tblReferralNotes").hide();
            $('#trDischargeRemarks').attr('style', 'display:none');
        }
    }

    $('#<%=txtAppointmentDate.ClientID %>').live('change', function () {
        cbpViewPopUpCtl.PerformCallback('getdaynumber');
    });

    //#region Referrer
    function onCboReferrerGroupValueChanged() {
        $('#<%=hdnReferrerID.ClientID %>').val('');
        $('#<%=txtReferrerCode.ClientID %>').val('');
        $('#<%=txtReferrerName.ClientID %>').val('');
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
        var polyclinicID = cboClinic.GetValue();
        var isCheckedFilter = $('#<%=chkParamedicHasSchedulePopUpCtl.ClientID %>').is(":checked");
        var filterExpression = '';

        var registrationDate = $('#<%:txtAppointmentDate.ClientID %>').val();
        var registrationDateInDatePicker = Methods.getDatePickerDate(registrationDate);
        var registrationDateFormatString = Methods.dateToString(registrationDateInDatePicker);
        var daynumber = $('#<%=hdnDayNumber.ClientID %>').val();
        if (isCheckedFilter) {
            filterExpression = "ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = '" + polyclinicID + "') AND IsDeleted = 0 AND (IsHasPhysicianRole = 1) AND ParamedicID IN (SELECT ParamedicID FROM vParamedicSchedule WHERE HealthcareServiceUnitID = '" + polyclinicID + "' AND DayNumber = '" + daynumber + "' UNION ALL SELECT ParamedicID FROM vParamedicScheduleDate WHERE HealthcareServiceUnitID = '" + polyclinicID + "' AND ScheduleDate = '" + registrationDateFormatString + "')";
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
        var polyclinicID = cboClinic.GetValue();
        var isCheckedFilter = $('#<%=chkParamedicHasSchedulePopUpCtl.ClientID %>').is(":checked");
        if (polyclinicID != '' && polyclinicID != null) {
            openSearchDialog('paramedic', getPhysicianFilterExpression(), function (value) {
                $('#<%=txtPhysicianCode.ClientID %>').val(value);
                onTxtPatientVisitPhysicianCodeChanged(value);
            });
        }
        else {
            showToast("Warning", "Silahkan Pilih Klinik Terlebih Dahulu");
        }
    });

    $('#<%=txtPhysicianCode.ClientID %>').live('change', function () {
        var polyclinicID = cboClinic.GetValue();
        var isCheckedFilter = $('#<%=chkParamedicHasSchedulePopUpCtl.ClientID %>').is(":checked");
        if (polyclinicID != '' && polyclinicID != null) {
            onTxtPatientVisitPhysicianCodeChanged($(this).val());
        }
        else {
            $('#<%=txtPhysicianCode.ClientID %>').val('');
            showToast("Warning", "Silahkan Pilih Klinik Terlebih Dahulu");
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
        var filterExpression = getPhysicianFilterExpression() + " AND ParamedicCode = '" + value + "' AND IsDeleted = 0";
        Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
            if (result != null) {
                var registrationDate = $('#<%:txtAppointmentDate.ClientID %>').val();
                var registrationDateInDatePicker = Methods.getDatePickerDate(registrationDate);
                var registrationDateFormatString = Methods.dateToString(registrationDateInDatePicker);
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

    $('#<%:chkParamedicHasSchedulePopUpCtl.ClientID %>').live('change', function () {
        $('#<%:hdnPhysicianID.ClientID %>').val('');
        $('#<%:txtPhysicianCode.ClientID %>').val('');
        $('#<%:txtPhysicianName.ClientID %>').val('');
        cboClinic.SetValue('');
    });
</script>
<input type="hidden" id="hdnRegistrationID" runat="server" />
<input type="hidden" id="hdnBedID" runat="server" />
<input type="hidden" id="hdnRoomID" runat="server" />
<input type="hidden" id="hdnGCRegistrationStatus" runat="server" />
<input type="hidden" id="hdnRegistrationDate" runat="server" />
<input type="hidden" id="hdnRegistrationTime" runat="server" />
<input type="hidden" id="hdnLOSInDay" runat="server" />
<input type="hidden" id="hdnLOSInHour" runat="server" />
<input type="hidden" id="hdnNoSEP" runat="server" />
<input type="hidden" id="hdnIsBPJSBridging" runat="server" />
<input type="hidden" id="hdnIsBridgingToAplicares" runat="server" />
<input type="hidden" id="hdnLOSInMinute" runat="server" />
<input type="hidden" id="hdnIsDischargeDateFromPlanning" runat="server" />
<input type="hidden" runat="server" id="hdnHealthcareServiceUnitID" value="" />
<input type="hidden" runat="server" id="hdnClassID" value="" />
<input type="hidden" runat="server" id="hdnHealthcareServiceUnitICUID" value="" />
<input type="hidden" runat="server" id="hdnHealthcareServiceUnitPICUID" value="" />
<input type="hidden" runat="server" id="hdnHealthcareServiceUnitNICUID" value="" />
<input type="hidden" runat="server" id="hdnIsParamedicInRegistrationUseScheduleCtl"
    value="" />
<input type="hidden" value="" id="hdnIsBridgingToIPTV" runat="server" />
<div style="width: 900px; height: 450px; overflow-y: scroll;">
    <div id="divOrderStatus" runat="server" style="float: right; margin-top: 3px;">
        <img id="imgOrderStatus" class="imgStatus" src='<%= ResolveUrl("~/Libs/Images/Toolbar/outstanding_order.png")%>'
            title="<%=GetLabel("Outstanding/Pending Order")%>" style="width: 50px; height: 50px"
            alt="" />
    </div>
    <table class="tblEntryContent" id="tblEntry" style="width: 700px">
        <colgroup>
            <col style="width: 200px" />
            <col style="width: 230px" />
            <col />
        </colgroup>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("No. Registrasi")%></label>
            </td>
            <td colspan="2">
                <asp:TextBox ID="txtRegistrationNo" ReadOnly="true" Width="150px" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Tanggal & Jam Masuk")%></label>
            </td>
            <td colspan="2">
                <asp:TextBox ID="txtRegistrationDateTime" ReadOnly="true" Width="150px" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Pasien")%></label>
            </td>
            <td colspan="2">
                <asp:TextBox ID="txtPatientInfo" ReadOnly="true" Width="100%" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Ruang Perawatan")%></label>
            </td>
            <td colspan="2">
                <asp:TextBox ID="txtServiceUnitName" ReadOnly="true" Width="100%" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Kamar")%></label>
            </td>
            <td colspan="2">
                <asp:TextBox ID="txtRoomName" ReadOnly="true" Width="150px" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Tempat Tidur")%></label>
            </td>
            <td colspan="2">
                <asp:TextBox ID="txtBedCode" ReadOnly="true" Width="100px" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <h4>
                    <%=GetLabel("Data Entry")%>
                </h4>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal" />
                <%=GetLabel("Tanggal & Jam Rencana Keluar")%>
            </td>
            <td colspan="2">
                <table>
                    <colgroup>
                        <col style="width: 150px" />
                        <col style="width: 100px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtPlanningDate" Width="120px" CssClass="datepicker" runat="server"
                                ReadOnly="true" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtPlanningTime" Width="80px" CssClass="time" runat="server" Style="text-align: center"
                                ReadOnly="true" MaxLength="5" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblMandatory" />
                <%=GetLabel("Tanggal & Jam Keluar")%>
            </td>
            <td colspan="2">
                <table>
                    <colgroup>
                        <col style="width: 150px" />
                        <col style="width: 100px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtDischargeDate" Width="120px" CssClass="datepicker" runat="server" disabled />
                        </td>
                        <td>
                            <table>
                                <colgroup>
                                    <col style="width: 50px" />
                                    <col style="width: 10px" />
                                    <col style="width: 50px" />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtDischargeTime1" Width="80px" CssClass="number" runat="server"
                                            Style="text-align: center" MaxLength="2" max="24" min="0" />
                                    </td>
                                    <td>
                                        <label class="lblNormal" />
                                        <%=GetLabel(":")%>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDischargeTime2" Width="80px" CssClass="number" runat="server"
                                            Style="text-align: center" MaxLength="2" max="59" min="0" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
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
        <tr class="trDeathInfo" style="display: none">
            <td class="tdLabel">
                <label class="lblMandatory" />
                <%=GetLabel("Tanggal - Jam Meninggal")%>
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
                    <%=GetLabel("Kondisi Keluar")%></label>
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
            <td>
                <asp:TextBox ID="txtDischargeRemarks" Width="95%" runat="server" TextMode="MultiLine"
                    Rows="3" />
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
                <label class="lblMandatory lblLink" id="lbReferrerCode" runat="server">
                    <%:GetLabel("Rumah Sakit / Faskes")%></label>
            </td>
            <td colspan="4">
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width: 100px">
                            <input type="hidden" id="hdnReferrerID" value="" runat="server" />
                            <asp:TextBox ID="txtReferrerCode" Width="100%" runat="server" />
                        </td>
                        <td style="width: 255px">
                            <asp:TextBox ID="txtReferrerName" Width="100%" ReadOnly="true" runat="server" />
                        </td>
                    </tr>
                </table>
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
                <label class="lblMandatory lblLink" id="lblParamedic2">
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
        <tr id="trAppointment" style="display: none">
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
        <tr id="trClinicReferralToOP" style="display: none">
            <td class="tdLabel">
                <label class="lblMandatory">
                    <%=GetLabel("Clinic")%></label>
            </td>
            <td colspan="2">
                <dxe:ASPxComboBox runat="server" ID="cboClinic" ClientInstanceName="cboClinic" Width="100%" />
            </td>
        </tr>
        <tr id="trParamedicHasScheduleCtl" style="display: none">
            <td>
            </td>
            <td colspan="2">
                <asp:CheckBox ID="chkParamedicHasSchedulePopUpCtl" runat="server" /><%:GetLabel("Tampilkan Hanya Dokter Yang Punya Jadwal")%>
            </td>
        </tr>
        <tr id="trPhysicianReferralToOP" style="display: none">
            <td class="tdLabel">
                <label class="lblLink lblMandatory" id="lblPhysician">
                    <%=GetLabel("Physician")%></label>
            </td>
            <td>
                <input type="hidden" id="hdnPhysicianID" value="" runat="server" />
                <asp:TextBox ID="txtPhysicianCode" CssClass="required" Width="100%" runat="server" />
            </td>
            <td colspan="2">
                <asp:TextBox ID="txtPhysicianName" ReadOnly="true" Width="100%" runat="server" />
            </td>
        </tr>
        <tr id="trAppointmentDateToOP" style="display: none">
            <td class="tdLabel">
                <label class="lblMandatory">
                    <%=GetLabel("Appointment Date")%></label>
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtAppointmentDate" CssClass="datepicker" Width="120px" />
            </td>
        </tr>
        <tr id="trRemarksToOP" style="display: none">
            <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                <label class="lblNormal">
                    <%=GetLabel("Remarks")%></label>
            </td>
            <td colspan="2">
                <asp:TextBox ID="txtRemarks" Width="100%" runat="server" TextMode="MultiLine" Height="80px" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblMandatory">
                    <%=GetLabel("Status Tempat Tidur")%></label>
            </td>
            <td colspan="2">
                <dxe:ASPxComboBox ID="cboBedStatus" Width="100%" runat="server" />
            </td>
        </tr>
        <tr style="display: none">
            <td>
            </td>
            <td>
                <input type="button" id="btnDischargePatientBPJS" value="Pulangin Pasien" style="margin-left: 5px;
                    width: 150px" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="chkIsAllowFollowUp" runat="server" Text="Dilakukan Follow-Up Visit/Call" />
            </td>
        </tr>
    </table>
    <table id="tblReferralNotes" cellpadding="0" cellspacing="0" style="width: 700px;
        display: none">
        <tr>
            <td colspan="3">
                <h4>
                    <%=GetLabel("Referral Notes")%>
                </h4>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <label class="lblNormal">
                    <%=GetLabel("Diagnosis Pasien:")%></label>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:TextBox ID="txtDiagnosisText" Width="95%" runat="server" TextMode="MultiLine"
                    Rows="2" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <label class="lblNormal">
                    <%=GetLabel("Pemeriksaan Fisik / Pemeriksaan Penunjang / Catatan Medis lain yang perlu mendapat perhatian:")%></label>
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
    <dxcp:ASPxCallbackPanel ID="cbpViewPopUpCtl" runat="server" Width="0%" ClientInstanceName="cbpViewPopUpCtl"
        ShowLoadingPanel="false" OnCallback="cbpViewPopUpCtl_Callback">
        <ClientSideEvents EndCallback="function(s,e) { hideLoadingPanel(); }" />
        <PanelCollection>
            <dx:PanelContent ID="PanelContent1" runat="server">
                <asp:Panel runat="server" ID="pnlReferrerGrdView" Style="width: 0%">
                    <input type="hidden" runat="server" id="hdnDayNumber" value="" />
                </asp:Panel>
            </dx:PanelContent>
        </PanelCollection>
    </dxcp:ASPxCallbackPanel>
</div>
