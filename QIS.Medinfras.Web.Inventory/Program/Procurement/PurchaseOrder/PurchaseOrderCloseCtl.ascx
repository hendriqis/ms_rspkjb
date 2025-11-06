<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PurchaseOrderCloseCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Inventory.Program.PurchaseOrderCloseCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_poclosectl">
    $(function () {
        hideLoadingPanel();
    });

    $('#<%=btnClose.ClientID %>').click(function (evt) {
        if (IsValid(evt, 'fsTrxPopup', 'mpTrxPopup')) {
            showToastConfirmation("Are You Sure Want To Close?", function (result) {
                if (result) {
                    cbpClosePO.PerformCallback('void');
                }
            })
        }
    });

    function oncboClosedPOReasonValueChanged(evt) {
        if (cboClosedPOReason.GetValue() == 'X416^999') {
            $('#<%=trReason.ClientID %>').show();
        } else {
            $('#<%=trReason.ClientID %>').hide();
        }
    }

    function onCloseAndNewSuccess() {
        pcRightPanelContent.Hide();
        onRefreshControl();
    }
</script>
<div class="toolbarArea">
    <ul id="ulMPListToolbar" runat="server">
        <li id="btnClose" runat="server">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbset.png")%>' alt="" /><br style="clear: both" />
            <div>
                <%=GetLabel("Close")%></div>
        </li>
    </ul>
</div>
<input type="hidden" id="hdnPurchaseOrderIDCtlClosed" runat="server" value="" />
<input type="hidden" id="hdnNewPurchaseOrderNo" runat="server" value="" />
<div style="padding: 10px;">
    <fieldset id="fsTrxPopup" style="margin: 0">
        <table>
            <colgroup>
                <col style="width: 150px" />
                <col />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Alasan Close PO") %></label>
                </td>
                <td>
                    <dxe:ASPxComboBox ID="cboClosedPOReason" ClientInstanceName="cboClosedPOReason" runat="server"
                        Width="300px">
                        <ClientSideEvents ValueChanged="function(s,e) { oncboClosedPOReasonValueChanged(e); }" />
                    </dxe:ASPxComboBox>
                </td>
            </tr>
            <tr id="trReason" runat="server" style="display: none">
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Alasan Lain")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtReason" Width="300px" runat="server"/>
                </td>
            </tr>
        </table>
        <div class="imgLoadingGrdView" id="containerImgLoadingView">
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
        <dxcp:ASPxCallbackPanel ID="cbpClosePO" runat="server" Width="100%" ClientInstanceName="cbpClosePO"
            ShowLoadingPanel="false" OnCallback="cbpClosePO_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel();}" EndCallback="function(s,e){
                    var result = s.cpResult.split('|');
                    if(result[0] == 'fail')
                        showToast('Save Failed', result[1]);
                    else
                        onCloseAndNewSuccess();
                    hideLoadingPanel();
                }" />
        </dxcp:ASPxCallbackPanel>
    </fieldset>
</div>
