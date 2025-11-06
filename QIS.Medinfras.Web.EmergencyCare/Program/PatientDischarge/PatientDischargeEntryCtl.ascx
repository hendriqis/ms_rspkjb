<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PatientDischargeEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.EmergencyCare.Program.PatientDischargeEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_patientdischargectl">
</script>
<input type="hidden" id="hdnRegistrationID" runat="server" />
<input type="hidden" id="hdnBedID" runat="server" />
<table class="tblEntryContent" id="tblEntry" style="width: 500px">
    <colgroup>
        <col style="width: 180px" />
        <col style="width: 230px" />
        <col />
    </colgroup>
    <tr>
        <td class="tdLabel">
            <label class="lblNormal">
                <%=GetLabel("No. Registrasi")%></label>
        </td>
        <td colspan="2">
            <asp:TextBox ID="txtRegistrationNo" ReadOnly="true" Width="300px" runat="server" />
        </td>
    </tr>
    <tr>
        <td class="tdLabel">
            <label class="lblNormal">
                <%=GetLabel("Tanggal & Jam Masuk")%></label>
        </td>
        <td colspan="2">
            <asp:TextBox ID="txtRegistrationDateTime" ReadOnly="true" Width="300px" runat="server" />
        </td>
    </tr>
    <tr>
        <td class="tdLabel">
            <label class="lblNormal">
                <%=GetLabel("Pasien")%></label>
        </td>
        <td colspan="2">
            <asp:TextBox ID="txtPatientInfo" ReadOnly="true" Width="300px" runat="server" />
        </td>
    </tr>
    <tr>
        <td class="tdLabel">
            <label class="lblNormal">
                <%=GetLabel("Ruang Perawatan")%></label>
        </td>
        <td colspan="2">
            <asp:TextBox ID="txtServiceUnitName" ReadOnly="true" Width="300px" runat="server" />
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
</table>
