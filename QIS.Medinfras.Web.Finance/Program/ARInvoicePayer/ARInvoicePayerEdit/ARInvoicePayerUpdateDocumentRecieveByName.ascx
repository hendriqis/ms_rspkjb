<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ARInvoicePayerUpdateDocumentRecieveByName.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.ARInvoicePayerUpdateDocumentRecieveByName" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_updateardocrecbynamepayerctl">

    $('#<%=btnSaveARDocRecByName.ClientID %>').click(function (evt) {
        if (IsValid(evt, 'fsTrxPopup', 'mpTrxPopup')) {
            cbpUpdateARDocRecieveByNamePayer.PerformCallback('void');
        }
    });

    function onSaveARDocRecByNamePayer() {
        pcRightPanelContent.Hide();
        onRefreshControl();
    }
</script>
<div class="toolbarArea">
    <ul id="ulMPListToolbar" runat="server">
        <li id="btnSaveARDocRecByName" runat="server">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbsave.png")%>' alt="" /><br style="clear: both" />
            <div>
                <%=GetLabel("Save")%></div>
        </li>
    </ul>
</div>
<div style="padding: 10px;">
    <fieldset id="fsTrxPopup" style="margin: 0">
        <input type="hidden" id="hdnARInvoiceID" runat="server" value="" />
        <input type="hidden" id="hdnParam" runat="server" value="" />
        <table>
            <colgroup>
                <col style="width: 200px" />
                <col style="width: 250px" />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Terima Dokumen Oleh") %></label>
                </td>
                <td>
                    <asp:TextBox runat="server" Width="100%" ID="txtARDocumentReceiveByName"/>
                </td>
            </tr>
        </table>
        <div class="imgLoadingGrdView" id="containerImgLoadingView">
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
        <dxcp:ASPxCallbackPanel ID="cbpUpdateARDocRecieveByNamePayer" runat="server" Width="100%" ClientInstanceName="cbpUpdateARDocRecieveByNamePayer"
            ShowLoadingPanel="false" OnCallback="cbpUpdateARDocRecieveByNamePayer_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel();}" EndCallback="function(s,e){
                    var result = s.cpResult.split('|');
                    if(result[0] == 'fail')
                        showToast('Save Failed', result[1]);
                    else
                        onSaveARDocRecByNamePayer();
                    hideLoadingPanel();
                }" />
        </dxcp:ASPxCallbackPanel>
    </fieldset>
</div>
