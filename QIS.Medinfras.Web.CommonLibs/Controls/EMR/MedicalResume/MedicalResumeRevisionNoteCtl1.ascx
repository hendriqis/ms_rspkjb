<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MedicalResumeRevisionNoteCtl1.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.MedicalResumeRevisionNoteCtl1" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>

<%@ Register assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxPanel" tagprefix="dx" %>

<script id="dxss_medicalResumeRevision" type="text/javascript">
    $(function () {
    });

    function onAfterProcessPopupEntry(param) {
        if (typeof onRefreshControl == 'function')
            onRefreshControl();
        pcRightPanelContent.Hide();
    }
</script>
<style type="text/css">
    .divEditPatientPhotoBtnImage    { border: 1px solid #d3d3d3; }
</style>

<input type="hidden" id="hdnRecordID" runat="server" />
<input type="hidden" id="hdnVisitID" runat="server" />
<input type="hidden" id="hdnUserParamedicName" runat="server" />
<input type="hidden" id="hdnSignatureIndex" runat="server" />
<div id="divBody">
    <div width="500px" height="auto">
        <table class="tblEntryContent" style="width:100%">
            <colgroup>
                <col style="width:175px"/>
                <col style="width:325px"/>
            </colgroup>
            <tr>
                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("PPA")%></label></td>
                <td><asp:TextBox ID="txtParamedicName" ReadOnly="true" Width="100%" runat="server" /></td>
            </tr>  
            <tr>
                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Tanggal - Waktu")%></label></td>
                <td><asp:TextBox ID="txtNoteDateTime" ReadOnly="true" Width="175px" runat="server" /></td>
            </tr> 
            <tr>
                <td class="tdLabel" style="vertical-align:top"><label class="lblNormal"><%=GetLabel("Catatan Revisi")%></label></td>
                <td>
                    <asp:TextBox ID="txtRemarks" runat="server" Width="100%" TextMode="Multiline"
                        Rows="5" />
                </td>
            </tr>
            <tr>
                <td />
                <td>
                    <asp:CheckBox ID="chkIsCasemixRevision" Width="250px" runat="server" Text=" Revisi untuk kelengkapan BPJS" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <br />
                </td>
            </tr>
        </table>
    </div>
</div>
