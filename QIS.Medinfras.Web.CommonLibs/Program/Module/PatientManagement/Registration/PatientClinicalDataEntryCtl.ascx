<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PatientClinicalDataEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientClinicalDataEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_PatientClinicalDataEntryCtl">
    
</script>
<div style="height: 250px; overflow-y: auto;">
    <input type="hidden" runat="server" id="hdnRegistrationIDCtl" value="" />
    <input type="hidden" runat="server" id="hdnMRNCtl" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 20%" />
            <col style="width: 80%" />
        </colgroup>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("No. RM")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtMedicalNoCtl" ReadOnly="true" Width="100%" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Nama Pasien")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtPatientNameCtl" ReadOnly="true" Width="100%" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <%=GetLabel("Has Infectious")%>
            </td>
            <td>
                <asp:CheckBox ID="chkIsHasInfectiousCtl" Width="100%" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <%=GetLabel("Has Allergy")%>
            </td>
            <td>
                <asp:CheckBox ID="chkIsHasAllergyCtl" Width="100%" runat="server" />
            </td>
        </tr>
    </table>
</div>
