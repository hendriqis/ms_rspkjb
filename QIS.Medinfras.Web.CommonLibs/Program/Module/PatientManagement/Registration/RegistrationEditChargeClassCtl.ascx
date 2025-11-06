<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RegistrationEditChargeClassCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.RegistrationEditChargeClassCtl" %>
   
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<script type="text/javascript" id="dxss_registrationeditctl">
    function onGetEntryPopupReturnValue() {
        var result = cboRegistrationEditChargeClass.GetValue();
        return result;
    }
</script>

<div style="height: 250px; overflow-y: scroll;">
    <input type="hidden" runat="server" id="hdnRegistrationID" value="" />
    <input type="hidden" value="" id="hdnRegistrationNo" runat="server" />
    <input type="hidden" runat="server" id="hdnHealthcareServiceUnitID" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 25%" />
            <col style="width: 75%" />
        </colgroup>
        <tr>
            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("No Registrasi")%></label></td>
            <td><asp:TextBox ID="txtRegistrationID" ReadOnly="true" Width="100%" runat="server" /></td>
        </tr>
        <tr>
            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("No RM")%></label></td>
            <td><asp:TextBox ID="txtMRN" ReadOnly="true" Width="100%" runat="server" /></td>
        </tr>
        <tr>
            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Klinik")%></label></td>
            <td><asp:TextBox ID="txtServiceUnit" ReadOnly="true" Width="100%" runat="server" /></td>
        </tr>
        <tr>
            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Dokter / Paramedis")%></label></td>
            <td><asp:TextBox ID="txtParamedicName" ReadOnly="true" Width="100%" runat="server" /></td>
        </tr>
        <tr>
            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Spesialisasi")%></label></td>
            <td><asp:TextBox ID="txtSpecialty" ReadOnly="true" Width="100%" runat="server" /></td>
        </tr>
        <tr>
            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Kelas Tagihan")%></label></td>
            <td><dxe:ASPxComboBox ID="cboRegistrationEditChargeClass" ClientInstanceName="cboRegistrationEditChargeClass" Width="100%" runat="server" /></td>
        </tr>
    </table>
</div>
