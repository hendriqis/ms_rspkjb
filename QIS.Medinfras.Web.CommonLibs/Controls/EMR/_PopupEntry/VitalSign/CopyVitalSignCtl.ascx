<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CopyVitalSignCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.CopyVitalSignCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_copyvitalsignentryctl">
    $(function () {
    });

    function onAfterProcessPopupEntry(param) {
        if (typeof onRefreshControl == 'function')
            onRefreshControl();
        if (typeof onRefreshVitalSignGrid == 'function')
            onRefreshVitalSignGrid();    
        pcRightPanelContent.Hide();
    }
</script>
<style type="text/css">
</style>

<input type="hidden" id="hdnRecordID" runat="server" />
<input type="hidden" id="hdnVisitID" runat="server" />
<input type="hidden" id="hdnUserParamedicName" runat="server" />
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
                <td colspan="2">
                    <br />
                </td>
            </tr>
        </table>
    </div>
</div>
