<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UpdateReferenceInfoCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Inventory.Program.UpdateReferenceInfoCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_updatereferencectl">
    $(function () {
        hideLoadingPanel();

        setDatePicker('<%=txtReferenceDate.ClientID %>');
    });

    $('#<%=btnSaveReferenceInfo.ClientID %>').click(function (evt) {
        if (IsValid(evt, 'fsTrxPopup', 'mpTrxPopup')) {
            cbpUpdateReferenceInfo.PerformCallback('refresh');
        }
    });

    function onSaveReferenceInfoCtl() {
        pcRightPanelContent.Hide();
        onRefreshControl();
    }
</script>
<div class="toolbarArea">
    <ul id="ulMPListToolbar" runat="server">
        <li id="btnSaveReferenceInfo" runat="server">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbsave.png")%>' alt="" /><br style="clear: both" />
            <div>
                <%=GetLabel("Save")%></div>
        </li>
    </ul>
</div>
<div style="padding: 10px;">
    <fieldset id="fsTrxPopup" style="margin: 0">
        <input type="hidden" id="hdnPurchaseReceiveID" runat="server" value="" />
        <input type="hidden" id="hdnParam" runat="server" value="" />
        <table>
            <colgroup>
                <col style="width: 250px" />
                <col style="width: 200px" />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("No. Faktur Kirim")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtReferenceNo" Width="200px" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Tanggal Faktur Kirim")%></label>
                </td>
                <td style="padding-right: 1px; width: 200px">
                    <asp:TextBox ID="txtReferenceDate" Width="120px" CssClass="datepicker" runat="server" />
                </td>
            </tr>
        </table>
        <div class="imgLoadingGrdView" id="containerImgLoadingView">
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
        <dxcp:ASPxCallbackPanel ID="cbpUpdateReferenceInfo" runat="server" Width="100%" ClientInstanceName="cbpUpdateReferenceInfo"
            ShowLoadingPanel="false" OnCallback="cbpUpdateReferenceInfo_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel();}" EndCallback="function(s,e){
                    var result = s.cpResult.split('|');
                    if(result[0] == 'fail')
                        showToast('Save Failed', result[1]);
                    else
                        onSaveReferenceInfoCtl();
                    hideLoadingPanel();
                }" />
        </dxcp:ASPxCallbackPanel>
    </fieldset>
</div>
