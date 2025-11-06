<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OpenBillingCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.OpenBillingCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_updatetaxctl">
    $(function () {
        hideLoadingPanel();
    });

    $('#<%=btnOpenBilling.ClientID %>').click(function (evt) {
        if (IsValid(evt, 'fsTrxPopup', 'mpTrxPopup')) {
            cbpOpenBillingCtl.PerformCallback('update');
        } else {
            cbpOpenBillingCtl.PerformCallback('refresh');
        }
    });

    function onAfterOpenBilling() {
        pcRightPanelContent.Hide();
        onRefreshControl();
    }
</script>
<div class="toolbarArea">
    <ul id="ulMPListToolbar" runat="server">
        <li id="btnOpenBilling" runat="server">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><br style="clear: both" />
            <div>
                <%=GetLabel("Buka")%></div>
        </li>
    </ul>
</div>
<div style="padding: 10px;">
    <fieldset id="fsTrxPopup" style="margin: 0">
        <input type="hidden" id="hdnRegistrationIDCtl" runat="server" value="" />
        <input type="hidden" value="" id="hdnRegistrationNoCtl" runat="server" />
        <table class="tblContentArea" style="width: 600px">
            <colgroup>
                <col style="width: 30%" />
                <col style="width: 70%" />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Status Lock")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtLockStatusCtl" ReadOnly="true" Width="100%" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Terakhir Lock Oleh")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtLockByCtl" ReadOnly="true" Width="100%" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Terakhir Lock Pada")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtLockDateCtl" ReadOnly="true" Width="100%" runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <hr />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Status Billing")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtBillingStatusCtl" ReadOnly="true" Width="100%" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Terakhir Billing Ditutup Oleh")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtBillingClosedByCtl" ReadOnly="true" Width="100%" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Terakhir Billing Ditutup Pada")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtBillingClosedDateCtl" ReadOnly="true" Width="100%" runat="server" />
                </td>
            </tr>
        </table>
        <div class="imgLoadingGrdView" id="containerImgLoadingView">
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
        <dxcp:ASPxCallbackPanel ID="cbpOpenBillingCtl" runat="server" Width="100%" ClientInstanceName="cbpOpenBillingCtl"
            ShowLoadingPanel="false" OnCallback="cbpOpenBillingCtl_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel();}" EndCallback="function(s,e){
                    var result = s.cpResult.split('|');
                    if(result[0] == 'fail')
                        showToast('Save Failed', result[1]);
                    else
                        onAfterOpenBilling();
                    hideLoadingPanel();
                }" />
        </dxcp:ASPxCallbackPanel>
    </fieldset>
</div>
