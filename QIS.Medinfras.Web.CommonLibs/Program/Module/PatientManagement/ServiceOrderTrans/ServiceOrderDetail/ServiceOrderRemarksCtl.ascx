<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ServiceOrderRemarksCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.ServiceOrderRemarksCtl" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
    
<script type="text/javascript" id="dxss_serviceunithealthcareentryctl">
    $('#btnServiceOrderApprove').click(function (evt) {
        cbpEntryPopupProcess.PerformCallback('decline');
    });

    function onCbpEntryPopupProcessEndCallback(s) {
        hideLoadingPanel();
        var param = s.cpResult.split('|');
        if (param[0] == 'fail')
            showToast('Approve Failed', 'Error Message : ' + param[1]);
        else 
            pcRightPanelContent.Hide();
    }

</script>

<input type="hidden" id="hdnServiceOrderID" value="" runat="server" />
<div class="pageTitle"><%=GetLabel("Catatan Order")%></div>
<div style="height:445px;overflow-y:auto;">
    <div class="pageTitle">
    </div>
    <table class="tblContentArea">
        <colgroup>
            <col style="width:150px"/>
        </colgroup>
        <tr>
            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("No Order")%></label></td>
            <td><asp:TextBox ID="txtServiceOrderNo" Width="350px" runat="server" ReadOnly="true"/></td> 
        </tr>
        <tr>
            <td class="tdLabel"><label class="lblNormal" id="lblOrderDate"><%=GetLabel("Tanggal") %> / <%=GetLabel("Jam") %></label></td>
            <td>
                <table style="width:100%" cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col style="width:30%"/>
                        <col style="width:3px"/>
                        <col/>
                    </colgroup>
                         <tr>
                             <td><asp:TextBox ID="txtServiceOrderDate" Width="120px" runat="server" Style="text-align:center" ReadOnly="true" /></td>
                             <td><asp:TextBox ID="txtServiceOrderTime" ReadOnly="true" Width="80px" runat="server" Style="text-align:center" /></td>
                         </tr>
                 </table>
             </td>
        </tr>
        <tr>
            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Dokter / Paramedis")%></label></td>
            <td><asp:TextBox ID="txtParamedic" Width="350px" runat="server" ReadOnly="true"/></td> 
        </tr>
        <tr>
            <td colspan="2" class="tdLabel">
                <label class="lblNormal" id="lblTestResult"><%=GetLabel("Catatan")%></label>
            </td>
        </tr>
    </table>
    <asp:TextBox ID="txtRemarks" ReadOnly="true" Width="99%" runat="server" TextMode="MultiLine" Rows="8" />
    
    <div style="width:100%;text-align:right">
        <input type="button" id="btnServiceOrderApprove" value='<%= GetLabel("Confirm")%>' />
    </div>

    <div style="display:none">
        <dxcp:ASPxCallbackPanel ID="cbpEntryPopupProcess" runat="server" Width="100%" ClientInstanceName="cbpEntryPopupProcess"
            ShowLoadingPanel="false" OnCallback="cbpEntryPopupProcess_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                EndCallback="function(s,e){ onCbpEntryPopupProcessEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
    </div>
</div>

