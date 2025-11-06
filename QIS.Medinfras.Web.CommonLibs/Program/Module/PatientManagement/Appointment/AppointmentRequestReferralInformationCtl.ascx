<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AppointmentRequestReferralInformationCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Inpatient.Program.PatientDischargeEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<script type="text/javascript" id="dxss_patientdischargectl">

    $(function () {
        
    });
</script>
<div style="width: 900px; height: 450px; overflow-y: scroll;">
    <table id="tblReferralNotes" cellpadding="0" cellspacing="0" style="width: 700px;>
        <tr>
            <td colspan="2">
                <label class="lblNormal">
                    <%=GetLabel("Registrasi Asal:")%></label>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:TextBox ID="txtRegistrationNo" Width="50%" runat="server" ReadOnly=true/>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <label class="lblNormal">
                    <%=GetLabel("Service Unit Asal:")%></label>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:TextBox ID="txtServiceUnit" Width="50%" runat="server" ReadOnly=true/>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <label class="lblNormal">
                    <%=GetLabel("Dokter DPJP Asal:")%></label>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:TextBox ID="txtParamedic" Width="50%" runat="server" ReadOnly=true/>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <label class="lblNormal">
                    <%=GetLabel("Kelas Tagihan Asal:")%></label>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:TextBox ID="txtChargeClass" Width="50%" runat="server" ReadOnly=true/>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <label class="lblNormal">
                    <%=GetLabel("Penjamin Bayar Asal:")%></label>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:TextBox ID="txtBusinessPartner" Width="50%" runat="server" ReadOnly=true/>
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
                    Rows="2" ReadOnly=true/>
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
                    Rows="6" ReadOnly=true />
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
                    Rows="6" ReadOnly=true />
            </td>
        </tr>
    </table>
</div>
