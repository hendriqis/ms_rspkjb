<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageList.master"
    AutoEventWireup="true" CodeBehind="PatientDispositionPage.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientDispositionPage" %>

<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionPage/TransactionPageToolbarCtl.ascx"
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
<asp:Content ID="Content4" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnPatientDischargeProcess" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbsave.png")%>' alt="" /><div>
            <%=GetLabel("Save")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript">
        var registrationDateTimeInString = '<%=RegistrationDateTime%>';
        var registrationDateTime = Methods.stringToDateTime(registrationDateTimeInString);
        $(function () {
            if (($('#<%=chkIsDead.ClientID %>').attr('checked'))) {
                cboPatientOutcome.SetVisible(false);
                cboPatientOutcomeDead.SetVisible(true);
                $('#tblEntry tr.trDeathInfo').show();
            } else {
                cboPatientOutcome.SetVisible(true);
                cboPatientOutcomeDead.SetVisible(false);
                $('#tblEntry tr.trDeathInfo').hide();
            }

            setDatePicker('<%=txtDischargeDate.ClientID %>');
            setDatePicker('<%=txtDateOfDeath.ClientID %>');
            setDatePicker('<%=txtAppointmentDate.ClientID %>');
            setDatePicker('<%=txtReferralToDate.ClientID %>');

            $('#<%=txtDischargeDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));
            $('#<%=txtDateOfDeath.ClientID %>').datepicker('option', 'maxDate', '0');
            $('#<%=txtAppointmentDate.ClientID %>').datepicker('option', 'minDate', '0');
            $('#<%=txtReferralToDate.ClientID %>').datepicker('option', 'minDate', '0');

            $('#<%=btnPatientDischargeProcess.ClientID %>').click(function (evt) {
                if (cboDischargeRoutine.GetValue() == Constant.DischargeRoutine.OTHER_HOSPITAL) {
                    if (IsValid(evt, 'fsPatientDischarge', 'mpPatientDischarge')) {
                        var hdnReferrerID = $('#<%=hdnReferrerID.ClientID %>').val();
                        if (hdnReferrerID == "" || hdnReferrerID == "0") {
                            displayErrorMessageBox('ERROR', "Mohon isi detail rujukan kemana terlebih dahulu.");
                        }
                        else {
                            if (validateTime($('#<%=txtReferralToTime.ClientID %>').val()) == true) {
                                if ($('#<%=txtReferralToDate.ClientID %>').val() != '') {
                                    if (cboDischargeReason.GetValue() != '') {
                                        onCustomButtonClick('save');
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
                        onCustomButtonClick('save');
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

            onDischargeDateTimeChange();
            onCboDischargeRoutineValueChanged();

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

            if (diff.d < 2)
                cboPatientOutcomeDead.SetValue('<%=GetPatientOutcomeDeadBefore48() %>');
            else
                cboPatientOutcomeDead.SetValue('<%=GetPatientOutcomeDeadAfter48() %>');
        }

        function onCboDischargeReasonValueChanged() {
            if (cboDischargeReason.GetValue() == Constant.DischargeReasonToOtherHospital.OTHER) {
                $('#trDischargeOtherReason').removeAttr('style');
            }
            else {
                $('#trDischargeOtherReason').attr('style', 'display:none');
            }
        }

        function onCboDischargeRoutineValueChanged() {
            if (cboDischargeRoutine.GetValue() == Constant.DischargeRoutine.OTHER_HOSPITAL) {
                $("#tblReferralNotes").show();
                cboDischargeReason.SetValue('');
                $('#trDischargeReason').removeAttr('style');
                $('#trReferralToDateTime').removeAttr('style');
                $('#trReferrerGroup').removeAttr('style');
                $('#trReferrer').removeAttr('style');
                $("#divAppointment").hide();
                $('#trAppointment').attr('style', 'display:none');
                $('#trInpatientPhysician').hide();
                $('#trInpatientPhysician1').hide();
                $('#trInpatientPhysician2').hide();
                $('#trInpatientPhysician3').hide();
                $('#trDischargeRemarks').attr('style', 'display:none');

                //                $('#trInpatientPhysician').attr('style', 'display:none');
                //                $('#trInpatientPhysician1').attr('style', 'display:none');
                //                $('#trInpatientPhysician2').attr('style', 'display:none');
                //                $('#trInpatientPhysician3').attr('style', 'display:none');
            }
            else if (cboDischargeRoutine.GetValue() == Constant.DischargeRoutine.REFER_TO_OUTPATIENT) {
                $("#tblReferralNotes").show();
                $("#divAppointment").show();
                $('#trAppointment').removeAttr('style');
                $('#trDischargeReason').attr('style', 'display:none');
                $('#trReferralToDateTime').attr('style', 'display:none');
                $('#trReferrerGroup').attr('style', 'display:none');
                $('#trReferrer').attr('style', 'display:none');
                $('#trDischargeOtherReason').attr('style', 'display:none');
                $('#trInpatientPhysician').hide();
                $('#trInpatientPhysician1').hide();
                $('#trInpatientPhysician2').hide();
                $('#trInpatientPhysician3').hide();
                $('#trDischargeRemarks').attr('style', 'display:none');
                //                $('#trInpatientPhysician').attr('style', 'display:none');
                //                $('#trInpatientPhysician1').attr('style', 'display:none');
                //                $('#trInpatientPhysician2').attr('style', 'display:none');
                //                $('#trInpatientPhysician3').attr('style', 'display:none');
            }
            else if (cboDischargeRoutine.GetValue() == Constant.DischargeRoutine.REFER_TO_INPATIENT) {
                $('#trDischargeReason').attr('style', 'display:none');
                $('#trReferrerGroup').attr('style', 'display:none');
                $('#trReferrer').attr('style', 'display:none');
                $('#trDischargeOtherReason').attr('style', 'display:none');
                $('#trReferralToDateTime').attr('style', 'display:none');
                $("#divAppointment").hide();
                $('#trAppointment').attr('style', 'display:none');
                $('#trInpatientPhysician').show();
                $('#trInpatientPhysician1').show();
                $('#trInpatientPhysician2').show();
                $('#trInpatientPhysician3').show();
                $('#trDischargeRemarks').attr('style', 'display:none');
                //                $('#trInpatientPhysician').removeAttr('style');
                //                $('#trInpatientPhysician1').removeAttr('style');
                //                $('#trInpatientPhysician2').removeAttr('style');
                //                $('#trInpatientPhysician3').removeAttr('style');
            }
            else if (cboDischargeRoutine.GetValue() == Constant.DischargeRoutine.FORCE_DISCHARGE) {
                $("#tblReferralNotes").hide();
                $('#trDischargeReason').attr('style', 'display:none');
                $('#trReferralToDateTime').attr('style', 'display:none');
                $('#trReferrerGroup').attr('style', 'display:none');
                $('#trReferrer').attr('style', 'display:none');
                $('#trDischargeOtherReason').attr('style', 'display:none');
                $("#divAppointment").hide();
                $('#trAppointment').attr('style', 'display:none');
                $('#trInpatientPhysician').hide();
                $('#trInpatientPhysician1').hide();
                $('#trInpatientPhysician2').hide();
                $('#trInpatientPhysician3').hide();
                $('#trDischargeRemarks').removeAttr('style');
            }
            else {
                $("#tblReferralNotes").hide();
                $('#trDischargeReason').attr('style', 'display:none');
                $('#trReferralToDateTime').attr('style', 'display:none');
                $('#trReferrerGroup').attr('style', 'display:none');
                $('#trReferrer').attr('style', 'display:none');
                $('#trDischargeOtherReason').attr('style', 'display:none');
                $("#divAppointment").hide();
                $('#trAppointment').attr('style', 'display:none');
                $('#trInpatientPhysician').hide();
                $('#trInpatientPhysician1').hide();
                $('#trInpatientPhysician2').hide();
                $('#trInpatientPhysician3').hide();
                $('#trDischargeRemarks').attr('style', 'display:none');
                //                $('#trInpatientPhysician').attr('style', 'display:none');
                //                $('#trInpatientPhysician1').attr('style', 'display:none');
                //                $('#trInpatientPhysician2').attr('style', 'display:none');
                //                $('#trInpatientPhysician3').attr('style', 'display:none');
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
            var polyclinicID = cboClinic.GetValue();
            var filterExpression = '';
            if (polyclinicID != '' && polyclinicID != null)
                filterExpression = "ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = " + polyclinicID + ")";
            return filterExpression;
        }

        $('#lblPhysician.lblLink').live('click', function () {
            openSearchDialog('paramedic', getPhysicianFilterExpression(), function (value) {
                $('#<%=txtPhysicianCode.ClientID %>').val(value);
                onTxtPatientVisitPhysicianCodeChanged(value);
            });
        });

        $('#<%=txtPhysicianCode.ClientID %>').live('change', function () {
            onTxtPatientVisitPhysicianCodeChanged($(this).val());
        });

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

        function onTxtPatientVisitPhysicianCodeChanged(value) {
            var filterExpression = getPhysicianFilterExpression() + " AND ParamedicCode = '" + value + "' AND IsDeleted = 0";
            Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnPhysicianID.ClientID %>').val(result.ParamedicID);
                    $('#<%=txtPhysicianName.ClientID %>').val(result.ParamedicName);
                }
                else {
                    $('#<%=hdnPhysicianID.ClientID %>').val('');
                    $('#<%=txtPhysicianCode.ClientID %>').val('');
                    $('#<%=txtPhysicianName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var registrationID = $('#<%:hdnRegistrationID.ClientID %>').val();
            var from = $('#<%=txtReferralToDate.ClientID %>').val();
            var dateDay = from.substring(0, 2);
            var dateMonth = from.substring(3, 5);
            var dateYear = from.substring(6);
            var newDate = dateYear + '-' + dateMonth + '-' + dateDay;
            var referralphysicianText = "";
            var therapyText = $('#<%=txtPlanningResumeText.ClientID %>').val();
            var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
            var diagnose = $('#<%=txtDiagnosisText.ClientID%>').val();
            var visitID = $('#<%=hdnVisitID.ClientID %>').val();
            if (registrationID != '') {
                if (code == 'MR000008') {
                    filterExpression.text = registrationID;
                    return true;
                }
                else if (code == 'PM-00545') {
                    if (($('#<%=hdnVisitStatus.ClientID %>').val() == 'X020^004' || $('#<%=hdnVisitStatus.ClientID %>').val() == 'X020^005') && cboDischargeRoutine.GetValue() == "X052^003") {
                        filterExpression.text = visitID + "|" + referralphysicianText + "|" + therapyText + "|" + newDate + "|" + diagnose + "|";
                        return true;
                    }
                    else {
                        errMessage.text = "Harap cek ulang apakah registrasi sudah disposisi dan cara keluar Pindah ke Rumah Sakit Lain / Faskes Lain";
                        return false;
                    }
                }
                else if (code == 'PM-00595') {
                    if (($('#<%=hdnVisitStatus.ClientID %>').val() == 'X020^004' || $('#<%=hdnVisitStatus.ClientID %>').val() == 'X020^005') && cboDischargeRoutine.GetValue() == "X052^003") {
                        filterExpression.text = visitID + "|" + referralphysicianText + "|" + therapyText + "|" + newDate + "|" + diagnose + "|";
                        return true;
                    }
                    else {
                        errMessage.text = "Harap cek ulang apakah registrasi sudah disposisi dan cara keluar Pindah ke Rumah Sakit Lain / Faskes Lain";
                        return false;
                    }
                }
                else {
                    errMessage.text = "ERROR";
                    return false;
                }
            } else {
                errMessage.text = 'Please Select Registration First!';
                return false;
            }
        }

        function onLedDiagnoseLostFocus(led) {
            var diagnoseID = led.GetValueText();
            $('#<%=hdnDiagnoseID.ClientID %>').val(diagnoseID);
            if ($('#<%=txtDiagnosisText.ClientID %>').val() == "") {
                $('#<%=txtDiagnosisText.ClientID %>').val(led.GetDisplayText());
            }
        }
        function onAfterCustomClickSuccessSetRecordID(param) {
            $('#<%=hdnVisitStatus.ClientID %>').val(param);
        }
        
    </script>
    <div>
        <input type="hidden" value="" id="hdnPageTitle" runat="server" />
        <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
        <input type="hidden" id="hdnRegistrationID" value="" runat="server" />
        <input type="hidden" id="hdnLOSInDay" runat="server" />
        <input type="hidden" id="hdnLOSInHour" runat="server" />
        <input type="hidden" id="hdnLOSInMinute" runat="server" />
        <input type="hidden" value="0" runat="server" id="hdnIsValidateParamedicSchedule" />
        <input type="hidden" value="0" runat="server" id="hdnIsBridgingToMJKN" />
        <input type="hidden" id="hdnGCReferrerGroup" value="" runat="server" />
        <input type="hidden" id="hdnVisitID" value="" runat="server" />
        <input type="hidden" value="" id="hdnVisitStatus" runat="server" />
        <table class="tblContentArea">
            <colgroup>
                <col style="width: 45%" />
                <col />
            </colgroup>
            <tr>
                <td style="padding: 5px; vertical-align: top;">
                    <fieldset id="fsPatientDischarge">
                        <table class="tblEntryContent" id="tblEntry" style="width: 100%">
                            <colgroup>
                                <col style="width: 200px" />
                                <col style="width: 150px" />
                            </colgroup>
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
                                        <%=GetLabel("Lama Perawatan")%></label>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtLengthOfVisit" ReadOnly="true" Width="98%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkIsDead" runat="server" Text=" Meninggal" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td colspan="4">
                                    <asp:CheckBox ID="chkIsTransferredToInpatient" runat="server" Text=" Pengantar Rawat Inap" />
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
                                <td colspan="4">
                                    <asp:TextBox ID="txtDischargeRemarks" Width="100%" runat="server" TextMode="MultiLine"
                                        Rows="3" />
                                </td>
                            </tr>
                            <tr id="trAppointment" style="display: none">
                                <td colspan="5">
                                    <div id="divAppointment">
                                        <table style="width: 100%" cellpadding="0" cellspacing="1">
                                            <colgroup>
                                                <col style="width: 200px" />
                                                <col style="width: 150px" />
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
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblLink" id="lblPhysician">
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
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("Tanggal Perjanjian")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtAppointmentDate" CssClass="datepicker" Width="120px" />
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
                            <tr id="trReferralToDateTime" style="display:none">
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
                            <tr id="trInpatientPhysician1" style="display: none">
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
                            <tr id="trInpatientPhysician2" style="display: none">
                                <td class="tdLabel" style="vertical-align: top">
                                    <label class="lblNormal" id="Label2">
                                        <%=GetLabel("Indikasi")%></label>
                                </td>
                                <td colspan="4">
                                    <asp:TextBox runat="server" ID="txtHospitalizedIndication" Width="100%" Rows="3"
                                        TextMode="MultiLine" />
                                </td>
                            </tr>
                            <tr id="trInpatientPhysician3" style="display: none">
                                <td class="tdLabel" style="vertical-align: top">
                                    <label class="lblNormal" id="Label4">
                                        <%=GetLabel("Jenis Pelayanan")%></label>
                                </td>
                                <td colspan="4">
                                    <table border="0" cellpadding="1" cellspacing="0" style="width: 100%">
                                        <colgroup>
                                            <col style="width: 25%" />
                                            <col style="width: 25%" />
                                            <col style="width: 25%" />
                                            <col style="width: 25%" />
                                        </colgroup>
                                        <tr>
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
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </td>
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
                                                                Width="100%" ValueText="DiagnoseID" FilterExpression="IsDeleted = 0 AND IsNutritionDiagnosis = 0" DisplayText="DiagnoseName"
                                                                MethodName="GetDiagnosisList">
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
        </table>
    </div>
</asp:Content>
