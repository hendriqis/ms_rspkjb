<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LaboratoryChargesVoidCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.Laboratory.Program.LaboratoryChargesVoidCtl" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_chargesvoidctl">
    $(function () {
        hideLoadingPanel();
    });

    $('#<%=btnVoidCharges.ClientID %>').click(function (evt) {
        if (IsValid(evt, 'fsTrxPopup', 'mpTrxPopup')) {
            cbpChargesVoid.PerformCallback('void');
        }
    });

    function onVoidChargesSuccess() {
        pcRightPanelContent.Hide();
        onRefreshControl();
    }

    function onCboVoidReasonChanged() {
        if (cboVoidReason.GetValue() != 'X129^999')
            $('#<%=trReason.ClientID %>').attr('style', 'display:none');
        else
            $('#<%=trReason.ClientID %>').removeAttr('style');
    }
</script>
<div class="toolbarArea">
    <ul id="ulMPListToolbar" runat="server">
        <li id="btnVoidCharges" runat="server">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbset.png")%>' alt="" /><br style="clear: both" />
            <div>
                <%=GetLabel("Process")%></div>
        </li>
    </ul>
</div>
<div style="padding: 10px;">
    <fieldset id="fsTrxPopup" style="margin: 0">
        <input type="hidden" id="hdnRegistrationID" runat="server" value="" />
        <input type="hidden" id="hdnTransactionID" runat="server" value="" />
        <input type="hidden" id="hdnParam" runat="server" value="" />
        <table>
            <colgroup>
                <col style="width: 250px" />
                <col style="width: 200px" />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Void Reason")%></label>
                </td>
                <td>
                    <dxe:ASPxComboBox runat="server" ID="cboVoidReason" ClientInstanceName="cboVoidReason"
                        Width="200px">
                        <ClientSideEvents ValueChanged="function(s,e){ onCboVoidReasonChanged(); }" />
                    </dxe:ASPxComboBox>
                </td>
            </tr>
            <tr id="trReason" runat="server" style="display: none">
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Reason")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtReason" Width="200px" runat="server" />
                </td>
            </tr>
        </table>
        <div class="imgLoadingGrdView" id="containerImgLoadingView">
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
        <dxcp:ASPxCallbackPanel ID="cbpChargesVoid" runat="server" Width="100%" ClientInstanceName="cbpChargesVoid"
            ShowLoadingPanel="false" OnCallback="cbpChargesVoid_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel();}" EndCallback="function(s,e){
                    var result = s.cpResult.split('|');
                    if(result[0] == 'fail')
                        showToast('Save Failed', result[1]);
                    else
                        onVoidChargesSuccess();
                    hideLoadingPanel();
                }" />
        </dxcp:ASPxCallbackPanel>
    </fieldset>
</div>
