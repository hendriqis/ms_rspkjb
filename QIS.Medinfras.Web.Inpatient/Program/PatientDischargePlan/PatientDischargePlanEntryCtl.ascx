<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PatientDischargePlanEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Inpatient.Program.PatientDischargePlanEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_patientdischargectl">
    $(function () {
        setDatePicker('<%=txtPlanDischargeDate.ClientID %>');
        $('#<%=txtPlanDischargeDate.ClientID %>').datepicker('option', 'minDate', 0);

        $('#<%=chkIsPlanDischarge.ClientID %>').change(function () {
            var isChecked = $(this).is(":checked");
            if (isChecked) {
                $('#<%=txtPlanDischargeDate.ClientID %>').datepicker('enable');
                $('#<%:txtPlanDischargeTime1.ClientID %>').attr("readonly", false);
                $('#<%:txtPlanDischargeTime2.ClientID %>').attr("readonly", false);
                $('#<%:txtPlanDischargeNotes.ClientID %>').attr("readonly", false);
            }
            else {
                $('#<%=txtPlanDischargeDate.ClientID %>').datepicker('disable');
                $('#<%:txtPlanDischargeTime1.ClientID %>').attr("readonly", true);
                $('#<%:txtPlanDischargeTime2.ClientID %>').attr("readonly", true);
                $('#<%:txtPlanDischargeNotes.ClientID %>').attr("readonly", true);
            }
        });

        $('#<%=chkIsPlanDischarge.ClientID %>').change();
    });

</script>
<input type="hidden" id="hdnVisitID" runat="server" />
<input type="hidden" id="hdnBedID" runat="server" />
<input type="hidden" id="hdnGCRegistrationStatus" runat="server" />
<input type="hidden" id="hdnRegistrationDate" runat="server" />
<input type="hidden" id="hdnRegistrationTime" runat="server" />
<input type="hidden" id="hdnLOSInDay" runat="server" />
<input type="hidden" id="hdnLOSInHour" runat="server" />
<input type="hidden" id="hdnLOSInMinute" runat="server" />
<table class="tblEntryContent" style="width: 100%">
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
                <%=GetLabel("Tanggal&Jam Masuk")%></label>
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
            <label class="lblNormal">
                <%=GetLabel("Rencana Pulang")%></label>
        </td>
        <td>
            <asp:CheckBox ID="chkIsPlanDischarge" runat="server" />
        </td>
    </tr>
    <tr>
        <td class="tdLabel">
            <label class="lblMandatory">
                <%=GetLabel("Tanggal&Jam Rencana Pulang")%></label>
        </td>
        <td>
            <asp:TextBox ID="txtPlanDischargeDate" Width="120px" CssClass="datepicker" runat="server" />
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
                        <asp:TextBox ID="txtPlanDischargeTime1" Width="80px" CssClass="number" runat="server"
                            Style="text-align: center" MaxLength="2" max="24" min="0" />
                    </td>
                    <td>
                        <label class="lblNormal" />
                        <%=GetLabel(":")%>
                    </td>
                    <td>
                        <asp:TextBox ID="txtPlanDischargeTime2" Width="80px" CssClass="number" runat="server"
                            Style="text-align: center" MaxLength="2" max="59" min="0" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td class="tdLabel">
            <label class="lblNormal">
                <%=GetLabel("Keterangan Rencana Pulang")%></label>
        </td>
        <td colspan="2">
            <dxe:ASPxComboBox ID="cboPlanDischargeNotesType" ClientInstanceName="cboPlanDischargeNotesType" Width="200" runat="server" />
        </td>
    </tr>
    <tr>
        <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
            <label class="lblNormal">
                <%:GetLabel("Detail Keterangan")%></label>
        </td>
        <td colspan="2">
            <asp:TextBox ID="txtPlanDischargeNotes" Width="100%" runat="server" TextMode="MultiLine"
                Rows="2" />
        </td>
    </tr>
    <tr id="trDischargePlanUpdatedDate" runat="server">
        <td class="tdLabel">
            <label class="lblNormal">
                <%=GetLabel("Terakhir Diubah")%></label>
        </td>
        <td>
            <asp:TextBox ID="txtDischargePlanUpdatedDate" ReadOnly="true" Width="250px" CssClass="text" runat="server" />
        </td>
    </tr>
    <tr id="trDischargePlanUpdatedBy" runat="server">
        <td class="tdLabel">
            <label class="lblNormal">
                <%=GetLabel("Diubah Oleh")%></label>
        </td>
        <td>
            <asp:TextBox ID="txtDischargePlanUpdatedBy" ReadOnly="true" Width="250px" CssClass="text" runat="server" />
        </td>
    </tr>
</table>
