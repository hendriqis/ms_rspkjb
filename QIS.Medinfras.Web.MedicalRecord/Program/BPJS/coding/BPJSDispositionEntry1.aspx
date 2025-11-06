<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageList.master"
    AutoEventWireup="true" CodeBehind="BPJSDispositionEntry1.aspx.cs" Inherits="QIS.Medinfras.Web.MedicalRecord.Program.BPJSDispositionEntry1" %>

<%@ Register Src="~/Program/BPJS/BPJSNavigationPaneCtl.ascx"
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

            $('#<%=txtDischargeDate.ClientID %>').datepicker('option', 'maxDate', '0');
            $('#<%=txtDateOfDeath.ClientID %>').datepicker('option', 'maxDate', '0');
            $('#<%=txtAppointmentDate.ClientID %>').datepicker('option', 'minDate', '0');

            $('#<%=btnPatientDischargeProcess.ClientID %>').click(function (evt) {
                if (IsValid(evt, 'fsPatientDischarge', 'mpPatientDischarge'))
                    onCustomButtonClick('process');
            });
            $('#<%=txtDischargeDate.ClientID %>').change(function () {
                onDischargeDateTimeChange();
            });
            $('#<%=txtDischargeTime.ClientID %>').change(function () {
                onDischargeDateTimeChange();
            });

            onDischargeDateTimeChange();
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
                cboDischargeRoutine.SetValue(Constant.DischargeRoutine.SEND_TO_MORTUARY);
            }
            else {
                $('#tblDischarge tr.trDeathInfo').hide();
                cboDischargeRoutine.SetValue('');
            }
        }
        function onCboDischargeReasonValueChanged() {
            if (cboDischargeReason.GetValue() == Constant.DischargeReasonToOtherHospital.OTHER) {
                $('#tblDischarge tr.trDischargeOtherReason').show();
            }
            else {
                $('#tblDischarge tr.trDischargeOtherReason').hide();
            }
        }

        function onCboDischargeRoutineValueChanged() {
            if (cboDischargeRoutine.GetValue() != null && (cboDischargeRoutine.GetValue() == Constant.DischargeRoutine.REFER_TO_OUTPATIENT || cboDischargeRoutine.GetValue() == Constant.DischargeRoutine.REFER_TO_INPATIENT || cboDischargeRoutine.GetValue() == Constant.DischargeRoutine.OTHER_HOSPITAL)) {
                if (cboDischargeRoutine.GetValue() == Constant.DischargeRoutine.REFER_TO_OUTPATIENT) {
                    $('#tblDischarge tr.trInpatientPhysician').hide();
                    $('#tblDischarge tr.trDischargeReason').hide();
                    $('#tblDischarge tr.trDischargeOtherReason').hide();
                    $('#tblDischarge tr.trReferToOtherProvider1').hide();
                    $('#tblDischarge tr.trReferToOtherProvider2').hide();
                    $("#divAppointment").show();
                }
                else if (cboDischargeRoutine.GetValue() == Constant.DischargeRoutine.OTHER_HOSPITAL) {
                    $("#divAppointment").hide();
                    $('#tblDischarge tr.trInpatientPhysician').hide();

                    cboDischargeReason.SetValue('');
                    $('#tblDischarge tr.trDischargeReason').show();

                    $('#tblDischarge tr.trReferToOtherProvider1').show();
                    $('#tblDischarge tr.trReferToOtherProvider2').show();
                }
                else if (cboDischargeRoutine.GetValue() == Constant.DischargeRoutine.REFER_TO_INPATIENT) {
                    $('#tblDischarge tr.trInpatientPhysician').show();

                    $("#divAppointment").hide();
                    $('#tblDischarge tr.trDischargeReason').hide();
                    $('#tblDischarge tr.trDischargeOtherReason').hide();
                    $('#tblDischarge tr.trReferToOtherProvider1').hide();
                    $('#tblDischarge tr.trReferToOtherProvider2').hide();

                }
                else {
                    $("#divAppointment").hide();
                    $('#tblDischarge tr.trDischargeReason').hide();
                    $('#tblDischarge tr.trDischargeOtherReason').hide();
                    $('#tblDischarge tr.trInpatientPhysician').hide();
                    $('#tblDischarge tr.trReferToOtherProvider1').hide();
                    $('#tblDischarge tr.trReferToOtherProvider2').hide();
                }
            }
            else {
                $("#divAppointment").hide();
                $('#tblDischarge tr.trInpatientPhysician').hide();
                $('#tblDischarge tr.trDischargeReason').hide();
                $('#tblDischarge tr.trDischargeOtherReason').hide();
                $('#tblDischarge tr.trReferToOtherProvider1').hide();
                $('#tblDischarge tr.trReferToOtherProvider2').hide();
            }
        }

        //#region Physician
        function getPhysicianFilterExpression() {
            var polyclinicID = cboClinic.GetValue();
            var filterExpression = '';
            if (polyclinicID != '' && polyclinicID != null)
                filterExpression = "ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = " + polyclinicID + ")";
            return filterExpression;
        }

        $('#lblParamedic2.lblLink').live('click', function () {
            openSearchDialog('paramedic', " GCParamedicMasterType = '" + Constant.ParamedicType.Physician + "'", function (value) {
                $('#<%=txtParamedicCode.ClientID %>').val(value);
                onTxtParamedicCodeChanged(value);
            });
        });

        $('#lblPhysician.lblLink').live('click', function () {
            openSearchDialog('paramedic', getPhysicianFilterExpression(), function (value) {
                $('#<%=txtPhysicianCode.ClientID %>').val(value);
                onTxtPatientVisitPhysicianCodeChanged(value);
            });
        });

        $('#<%=txtPhysicianCode.ClientID %>').live('change', function () {
            onTxtPatientVisitPhysicianCodeChanged($(this).val());
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

        function onCboReferralValueChanged(s) {
            $('#<%:hdnReferrerID.ClientID %>').val('');
            $('#<%:hdnReferrerParamedicID.ClientID %>').val('');
            $('#<%:txtReferralDescriptionCode.ClientID %>').val('');
            $('#<%:txtReferralDescriptionName.ClientID %>').val('');
            if (cboReferral.GetValue() != '' && cboReferral.GetValue() != null) {
                $('#<%:lblReferralDescription.ClientID %>').attr('class', 'lblLink');
                $('#<%:txtReferralDescriptionCode.ClientID %>').removeAttr('readonly');
            }
            else {
                $('#<%:lblReferralDescription.ClientID %>').attr('class', 'lblDisabled');
                $('#<%:txtReferralDescriptionCode.ClientID %>').attr('readonly', 'readonly');
            }
        }

        //#region Referral Description
        function getReferralDescriptionFilterExpression() {
            var filterExpression = "GCReferrerGroup = '" + cboReferral.GetValue() + "' AND IsDeleted = 0";
            return filterExpression;
        }

        $('#<%:lblReferralDescription.ClientID %>.lblLink').live('click', function () {
            var referral = cboReferral.GetValue();
            openSearchDialog('referrer1', getReferralDescriptionFilterExpression(), function (value) {
                $('#<%:txtReferralDescriptionCode.ClientID %>').val(value);
                onTxtReferralDescriptionCodeChanged(value);
            });
        });

        $('#<%:txtReferralDescriptionCode.ClientID %>').live('change', function () {
            onTxtReferralDescriptionCodeChanged($(this).val());
        });

        function onTxtReferralDescriptionCodeChanged(value) {
            var filterExpression = "";
            var referral = cboReferral.GetValue();
            filterExpression = getReferralDescriptionFilterExpression() + " AND BusinessPartnerCode = '" + value + "'";
            Methods.getObject('GetvReferrerList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%:hdnReferrerID.ClientID %>').val(result.BusinessPartnerID);
                    $('#<%:txtReferralDescriptionName.ClientID %>').val(result.BusinessPartnerName);
                }
                else {
                    $('#<%:hdnReferrerID.ClientID %>').val('');
                    $('#<%:txtReferralDescriptionCode.ClientID %>').val('');
                    $('#<%:txtReferralDescriptionName.ClientID %>').val('');
                }
            });
        }
        //#endregion
        //#endregion
        //        function onAfterCustomClickSuccess(type) {
        //            exitPatientPage();
        //        }
    </script>
    <input type="hidden" id="hdnLOSInDay" runat="server" />
    <input type="hidden" id="hdnLOSInHour" runat="server" />
    <input type="hidden" id="hdnLOSInMinute" runat="server" />
    <input type="hidden" id="hdnDepartmentID" runat="server" />
    <fieldset id="fsPatientDischarge">
        <table class="tblEntryContent" style="width: 100%">
            <colgroup>
                <col width="50%" />
                <col />
            </colgroup>
            <tr>
                <td style="vertical-align: top;">
                    <table id="tblDischarge" style="width: 100%">
                        <colgroup>
                            <col style="width: 235px" />
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
                                    <%=GetLabel("Jam Pulang")%></label>
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
                                <asp:TextBox ID="txtLengthOfVisit" ReadOnly="true" Width="99%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Keadaan Keluar")%></label>
                            </td>
                            <td colspan="2">
                                <dxe:ASPxComboBox ID="cboPatientOutcome" Width="100%" runat="server" ClientInstanceName="cboPatientOutcome">
                                    <ClientSideEvents ValueChanged="function(s,e){ onCboPatientOutcomeChanged(); }" />
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
                        <tr class="trReferToOtherProvider1" style="display: none">
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%:GetLabel("Dirujuk Ke")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboReferral" ClientInstanceName="cboReferral" Width="100%"
                                    runat="server">
                                    <ClientSideEvents ValueChanged="function(s){ onCboReferralValueChanged(s); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr class="trReferToOtherProvider2" style="display: none">
                            <td class="tdLabel">
                                <label class="lblLink" runat="server" id="lblReferralDescription">
                                    <%:GetLabel("Rumah Sakit / Faskes")%></label>
                            </td>
                            <td colspan="2">
                                <input type="hidden" id="hdnReferrerID" value="" runat="server" />
                                <input type="hidden" id="hdnReferrerParamedicID" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 80px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtReferralDescriptionCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtReferralDescriptionName" Width="100%" runat="server" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr class="trDischargeReason" style="display: none">
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Alasan Ke Rumah Sakit/Faskes lain")%></label>
                            </td>
                            <td colspan="2">
                                <dxe:ASPxComboBox ID="cboDischargeReason" Width="100%" runat="server" ClientInstanceName="cboDischargeReason">
                                    <ClientSideEvents Init="function(s,e){ onCboDischargeReasonValueChanged(s); }" ValueChanged="function(s,e){ onCboDischargeReasonValueChanged(); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr class="trDischargeOtherReason" style="display: none">
                            <td>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtDischargeOtherReason" Width="100%" runat="server" />
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
                </td>
                <td style="vertical-align:top">
                    <div ID="divAppointment" style="display:none">
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
                                    <asp:TextBox ID="txtPhysicianCode" CssClass="required" Width="100%" runat="server" />
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtPhysicianName" ReadOnly="true" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Appointment Date")%></label>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtAppointmentDate" CssClass="datepicker" Width="120px" />
                                </td>
                            </tr>
                            <tr style="display:none">
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
    </fieldset>
</asp:Content>
