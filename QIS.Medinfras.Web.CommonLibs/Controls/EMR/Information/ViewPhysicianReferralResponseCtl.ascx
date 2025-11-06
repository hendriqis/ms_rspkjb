<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewPhysicianReferralResponseCtl.ascx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientPage.ViewPhysicianReferralResponseCtl" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" 
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>

<script type="text/javascript" id="dxss_diagnosisentryctl">
    setDatePicker('<%=txtReplyDate.ClientID %>');
    $('#<%=txtReplyDate.ClientID %>').datepicker('option', 'maxDate', '0');

    function onAfterSaveRecordPatientPageEntry(value) {
        cbpView.PerformCallback('refresh');
    }

</script>

<div style="height: 500px; overflow-y: scroll; border: 0px">
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnDefaultParamedicID" runat="server" />
    <table style="width:100%">
        <colgroup>
            <col style="width:150px"/>
            <col style="width:150px"/>
            <col />
        </colgroup>
        <tr>
            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Tanggal ")%> - <%=GetLabel("Jam ")%></label></td>
            <td><asp:TextBox ID="txtReplyDate" Width="120px" CssClass="datepicker" runat="server" ReadOnly="true" /></td>
            <td><asp:TextBox ID="txtReplyTime" Width="80px" CssClass="time" runat="server" ReadOnly="true"/></td>
        </tr>
        <tr>
            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Dokter ")%></label></td>
            <td colspan="2"><dxe:ASPxComboBox runat="server" ID="cboPhysician" ClientInstanceName="cboPhysician" Width="300px" ReadOnly="true" /></td>
        </tr>
        <tr>
            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Dokter Rujukan")%></label></td>
            <td colspan="2"><dxe:ASPxComboBox runat="server" ID="cboPhysician2" ClientInstanceName="cboPhysician2" Width="300px" ReadOnly="true" /></td>
        </tr>
        <tr>
            <td colspan="3">
                <label class="lblNormal"><%=GetLabel("Diagnosis Pasien:")%></label>
            </td>
        </tr>
        <tr>
            <td colspan="3"><asp:TextBox ID="txtDiagnosisText" Width="95%" runat="server" TextMode="MultiLine" Rows="2" ReadOnly="true" /></td>
        </tr>
        <tr>
            <td colspan="3">
                <label class="lblNormal"><%=GetLabel("Pemeriksaan Fisik / Pemeriksaan Penunjang / Catatan Medis lain yang perlu mendapat perhatian :")%></label>
            </td>
        </tr>
        <tr>
            <td colspan="3"><asp:TextBox ID="txtMedicalResumeText" Width="95%" runat="server" TextMode="MultiLine" Rows="6" ReadOnly="true" /></td>
        </tr>
        <tr>
            <td colspan="3">
                <label class="lblNormal"><%=GetLabel("Terapi yang disarankan :")%></label>
            </td>
        </tr>
        <tr>
            <td colspan="3"><asp:TextBox ID="txtPlanningResumeText" Width="95%" runat="server" TextMode="MultiLine" Rows="6" ReadOnly="true" /></td>
        </tr>
    </table>
</div>
