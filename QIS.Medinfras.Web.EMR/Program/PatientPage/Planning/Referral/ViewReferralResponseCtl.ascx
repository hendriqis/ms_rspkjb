<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewReferralResponseCtl.ascx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.PatientPage.ViewReferralResponseCtl" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" 
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>

<script type="text/javascript" id="dxss_diagnosisentryctl">
    setDatePicker('<%=txtReplyDate.ClientID %>');
    $('#<%=txtReplyDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

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
            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Dokter Perujuk")%></label></td>
            <td colspan="2"><dxe:ASPxComboBox runat="server" ID="cboPhysician" ClientInstanceName="cboPhysician" Width="300px" ReadOnly="true" /></td>
        </tr>
        <tr>
            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Dokter Rujukan")%></label></td>
            <td colspan="2"><dxe:ASPxComboBox runat="server" ID="cboPhysician2" ClientInstanceName="cboPhysician2" Width="300px" ReadOnly="true" /></td>
        </tr>
        <tr>
            <td colspan="3">
                <table border="0" cellpadding="1" cellspacing="0" width="100%">
                    <colgroup>
                        <col style="width:450px"/>
                        <col />
                    </colgroup>
                    <tr>
                        <td>
                            <label class="lblMandatory" style="font-weight:bold"><%=GetLabel("Sesuai permintaan konsultasi, pada kasus ini dijumpai : (S)")%></label>
                        </td>
                    </tr>
                </table>                        
            </td>
        </tr>
        <tr>
            <td colspan="3"><asp:TextBox ID="txtReplySubjectiveText" Width="95%" runat="server" TextMode="MultiLine" Rows="2"  ReadOnly="true"/></td>
        </tr>
        <tr>
            <td colspan="3">
                <label class="lblMandatory" style="font-weight:bold"><%=GetLabel("Catatan Pemeriksaan Fisik : (O)")%></label>
            </td>
        </tr>
        <tr>
            <td colspan="3"><asp:TextBox ID="txtReplyObjectiveText" Width="95%" runat="server" TextMode="MultiLine" Rows="2"  ReadOnly="true"/></td>
        </tr>
        <tr>
            <td colspan="3">
                <label class="lblNormal" style="font-weight:bold"><%=GetLabel("Pemeriksaan Penunjang / Catatan Medis lain yang perlu mendapat perhatian : (O)")%></label>
            </td>
        </tr>
        <tr>
            <td colspan="3"><asp:TextBox ID="txtReplyMedicalResumeText" Width="95%" runat="server" TextMode="MultiLine" Rows="2"  ReadOnly="true" /></td>
        </tr>
        <tr>
            <td colspan="3">
                <label class="lblNormal" style="font-weight:bold"><%=GetLabel("Diagnosis Pasien: (A)")%></label>
            </td>
        </tr>
        <tr>
            <td colspan="3"><asp:TextBox ID="txtReplyDiagnosisText" Width="95%" runat="server" TextMode="MultiLine" Rows="2" ReadOnly="true" /></td>
        </tr>
        <tr>
            <td colspan="3">
                <label class="lblNormal" style="font-weight:bold"><%=GetLabel("Tindakan atau Terapi yang disarankan : (P)")%></label>
            </td>
        </tr>
        <tr>
            <td colspan="3"><asp:TextBox ID="txtReplyPlanningResumeText" Width="95%" runat="server" TextMode="MultiLine" Rows="2" ReadOnly="true" /></td>
        </tr>
        <tr>
            <td colspan="3">
                <label class="lblNormal" style="font-weight:bold"><%=GetLabel("Catatan Instruksi : (I)")%></label>
            </td>
        </tr>
        <tr>
            <td colspan="3"><asp:TextBox ID="txtInstructionResumeText" Width="95%" runat="server" TextMode="MultiLine" Rows="2" ReadOnly="true"/></td>
        </tr>
    </table>
</div>
