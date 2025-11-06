<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReferallLeterCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.EMR.Program.PatientPage.ReferallLeterCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<script type="text/javascript" id="dxss_referallleterctl">
    $(function () {
        setDatePicker('<%=txtDischargeDateCtl.ClientID %>');
        setDatePicker('<%=txtReferralToDateCtl.ClientID %>');
        setDatePicker('<%=txtDateOfDeathCtl.ClientID %>');
        onDischargeDateTimeChangeCtl();
    });

    //#region Referrer
    function onCboReferrerGroupCtlValueChanged() {
        if ($('#<%=hdnGCReferrerGroupCtl.ClientID %>').val() != cboReferrerGroupCtl.GetValue()) {
            $('#<%=hdnReferrerIDCtl.ClientID %>').val('');
            $('#<%=txtReferrerCodeCtl.ClientID %>').val('');
            $('#<%=txtReferrerNameCtl.ClientID %>').val('');
        }
        $('#<%=hdnGCReferrerGroupCtl.ClientID %>').val(cboReferrerGroupCtl.GetValue());
        return "GCReferrerGroup = '" + cboReferrerGroupCtl.GetValue() + "'";
    }

    $('#<%:lbReferrerCodeCtl.ClientID %>.lblLink').live('click', function () {
        var filterExpression = onCboReferrerGroupCtlValueChanged() + " AND IsDeleted = 0"
        openSearchDialog('referrer2', filterExpression, function (value) {
            $('#<%=txtReferrerCodeCtl.ClientID %>').val(value);
            onTxtReferrerCodeCtlChanged(value);
        });
    });

    $('#<%=txtReferrerCodeCtl.ClientID %>').live('change', function () {
        onTxtReferrerCodeCtlChanged($(this).val());
    });

    function onTxtReferrerCodeCtlChanged(value) {
        var filterExpression = onCboReferrerGroupCtlValueChanged() + " AND BusinessPartnerCode = '" + value + "' AND IsDeleted = 0";
        Methods.getObject('GetvReferrerList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnReferrerIDCtl.ClientID %>').val(result.BusinessPartnerID);
                $('#<%=txtReferrerCodeCtl.ClientID %>').val(result.BusinessPartnerCode);
                $('#<%=txtReferrerNameCtl.ClientID %>').val(result.BusinessPartnerName);
            }
            else {
                $('#<%=hdnReferrerIDCtl.ClientID %>').val('');
                $('#<%=txtReferrerCodeCtl.ClientID %>').val('');
                $('#<%=txtReferrerNameCtl.ClientID %>').val('');
            }
        });
    }
    //#endregion

    function onCboDischargeReasonCtlValueChanged() {
        if (cboDischargeReasonCtl.GetValue() == Constant.DischargeReasonToOtherHospital.OTHER) {
            $('#trDischargeOtherReasonCtl').removeAttr('style');
        }
        else {
            $('#trDischargeOtherReasonCtl').attr('style', 'display:none');
        }
    }

    function onLedDiagnoseCtlLostFocus(led) {
        var diagnoseID = led.GetValueText();
        $('#<%=hdnDiagnoseIDCtl.ClientID %>').val(diagnoseID);
        if ($('#<%=txtDiagnosisTextCtl.ClientID %>').val() == "") {
            $('#<%=txtDiagnosisTextCtl.ClientID %>').val(led.GetDisplayText());
        }
    }

    $('#<%=txtDischargeDateCtl.ClientID %>').change(function () {
        onDischargeDateTimeChangeCtl();
    });
    $('#<%=txtDischargeTimeCtl.ClientID %>').change(function () {
        onDischargeDateTimeChangeCtl();
    });

    function dateDiffCtl(date1, date2) {
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

    function onDischargeDateTimeChangeCtl() {
        var dischargeDate = Methods.getDatePickerDate($('#<%=txtDischargeDateCtl.ClientID %>').val());
        var dischargeTime = $('#<%=txtDischargeTimeCtl.ClientID %>').val();
        var dischargeDateTimeInString = Methods.dateToString(dischargeDate) + dischargeTime.replace(':', '');
        var diff = dateDiffCtl(registrationDateTime, Methods.stringToDateTime(dischargeDateTimeInString));
        $('#<%=hdnLOSInDayCtl.ClientID %>').val(diff.d);
        $('#<%=hdnLOSInHourCtl.ClientID %>').val(diff.h);
        $('#<%=hdnLOSInMinuteCtl.ClientID %>').val(diff.m);
        $('#<%=txtLengthOfVisitCtl.ClientID %>').val(diff.d + 'dd ' + diff.h + 'hh ' + diff.m + 'mm');
    }

    function onCboPatientOutcomeCtlChanged() {
        if (cboPatientOutcomeCtl.GetValue() != null && (cboPatientOutcomeCtl.GetValue() == Constant.PatientOutcome.DEAD_BEFORE_48 || cboPatientOutcomeCtl.GetValue() == Constant.PatientOutcome.DEAD_AFTER_48)) {
            $('#tblDischargeCtl tr.trDeathInfoCtl').show();
        }
        else {
            $('#tblDischargeCtl tr.trDeathInfoCtl').hide();
        }
    }
</script>
<div style="height: 500px; border: 0px">
    <input type="hidden" value="" id="hdnVisitIDCtl" runat="server" />
    <input type="hidden" value="" id="hdnRegistrationIDCtl" runat="server" />
    <input type="hidden" id="hdnGCReferrerGroupCtl" value="" runat="server" />
    <input type="hidden" id="hdnLOSInDayCtl" runat="server" />
    <input type="hidden" id="hdnLOSInHourCtl" runat="server" />
    <input type="hidden" id="hdnLOSInMinuteCtl" runat="server" />
    <table style="width: 100%" id="tblDischargeCtl">
        <colgroup>
            <col style="width: 150px" />
            <col style="width: 150px" />
            <col style="width: 100px" />
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
                <asp:TextBox ID="txtDischargeDateCtl" Width="120px" CssClass="datepicker" runat="server" />
            </td>
            <td>
                <asp:TextBox ID="txtDischargeTimeCtl" Width="80px" CssClass="time" runat="server"
                    Style="text-align: center" />
            </td>
            <td>
                <asp:TextBox ID="txtLengthOfVisitCtl" ReadOnly="true" Width="100%" runat="server"
                    Style="text-align: center" />
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:CheckBox ID="chkIsComplexVisitCtl" runat="server" Text=" Kasus Kompleks" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblMandatory">
                    <%=GetLabel("Keadaan Keluar")%></label>
            </td>
            <td colspan="4">
                <dxe:ASPxComboBox ID="cboPatientOutcomeCtl" ClientInstanceName="cboPatientOutcomeCtl"
                    Width="100%" runat="server">
                    <ClientSideEvents ValueChanged="function(s,e){ onCboPatientOutcomeCtlChanged(); }" />
                </dxe:ASPxComboBox>
            </td>
        </tr>
        <tr class="trDeathInfoCtl" style="display: none">
            <td class="tdLabel">
                <label class="lblMandatory">
                    <%=GetLabel("Death Date - Time")%></label>
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtDateOfDeathCtl" CssClass="datepicker" Width="120px" />
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtTimeOfDeathCtl" CssClass="time" Width="80px" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblMandatory">
                    <%=GetLabel("Rujuk Ke")%></label>
            </td>
            <td colspan="2">
                <dxe:ASPxComboBox ID="cboReferrerGroupCtl" Width="100%" runat="server" ClientInstanceName="cboReferrerGroupCtl">
                    <ClientSideEvents Init="function(s,e){ onCboReferrerGroupCtlValueChanged(s); }" ValueChanged="function(s,e){ onCboReferrerGroupCtlValueChanged(); }" />
                </dxe:ASPxComboBox>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblLink" id="lbReferrerCodeCtl" runat="server">
                    <%:GetLabel("Rumah Sakit / Faskes")%></label>
            </td>
            <td colspan="5">
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <input type="hidden" id="hdnReferrerIDCtl" value="" runat="server" />
                            <asp:TextBox ID="txtReferrerCodeCtl" Width="150px" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtReferrerNameCtl" Width="330px" ReadOnly="true" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblMandatory">
                    <%=GetLabel("Tanggal")%>
                    -
                    <%=GetLabel("Jam Rujukan")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtReferralToDateCtl" Width="120px" CssClass="datepicker" runat="server" />
            </td>
            <td>
                <asp:TextBox ID="txtReferralToTimeCtl" Width="80px" CssClass="time" runat="server"
                    Style="text-align: center" />
            </td>
            <td>
            </td>
            <td>
            </td>
        </tr>
        <tr id="trDischargeReasonCtl">
            <td class="tdLabel">
                <label class="lblMandatory">
                    <%=GetLabel("Alasan Ke Rumah Sakit lain")%></label>
            </td>
            <td colspan="2">
                <dxe:ASPxComboBox ID="cboDischargeReasonCtl" Width="100%" runat="server" ClientInstanceName="cboDischargeReasonCtl">
                    <ClientSideEvents Init="function(s,e){ onCboDischargeReasonCtlValueChanged(s); }"
                        ValueChanged="function(s,e){ onCboDischargeReasonCtlValueChanged(); }" />
                </dxe:ASPxComboBox>
            </td>
        </tr>
        <tr id="trDischargeOtherReasonCtl" style="display: none">
            <td>
            </td>
            <td colspan="2">
                <asp:TextBox ID="txtDischargeOtherReasonCtl" Width="100%" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <label class="lblNormal">
                    <%=GetLabel("Diagnosis Pasien :")%></label>
            </td>
        </tr>
        <tr>
            <td>
                <label class="lblNormal" id="lblDiagnoseIDCtl" runat="server">
                    <%:GetLabel("ICD X")%></label>
            </td>
            <td>
                <table style="width: 400%" cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col style="width: 50px" />
                    </colgroup>
                    <tr>
                        <td>
                            <input type="hidden" id="hdnDiagnoseIDCtl" value="" runat="server" />
                            <qis:QISSearchTextBox ID="ledDiagnoseCtl" ClientInstanceName="ledDiagnoseCtl" runat="server"
                                Width="100%" ValueText="DiagnoseID" FilterExpression="IsDeleted = 0 AND IsNutritionDiagnosis = 0"
                                DisplayText="DiagnoseName" MethodName="GetDiagnosisList">
                                <ClientSideEvents ValueChanged="function(s){ onLedDiagnoseCtlLostFocus(s); }" />
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
                            <asp:TextBox ID="txtDiagnosisTextCtl" Width="100%" runat="server" TextMode="MultiLine"
                                Rows="2" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <label class="lblNormal">
                    <%=GetLabel("Pemeriksaan Fisik / Pemeriksaan Penunjang / Catatan Medis lain yang perlu mendapat perhatian :")%></label>
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <asp:TextBox ID="txtMedicalResumeTextCtl" Width="95%" runat="server" TextMode="MultiLine"
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
            <td colspan="4">
                <asp:TextBox ID="txtPlanningResumeTextCtl" Width="95%" runat="server" TextMode="MultiLine"
                    Rows="6" />
            </td>
        </tr>
    </table>
</div>
