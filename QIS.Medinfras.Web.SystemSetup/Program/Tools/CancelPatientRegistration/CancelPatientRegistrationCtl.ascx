<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CancelPatientRegistrationCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.SystemSetup.Program.CancelPatientRegistrationCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_canceldischargectl">
    $(function () {
        hideLoadingPanel();
    });

    $('#<%=btnVoidRegistration.ClientID %>').click(function (evt) {
        if (IsValid(evt, 'fsTrxPopup', 'mpTrxPopup')) {
            cbpCancelRegistration.PerformCallback('void');
        }
    });

    function onReopenChargesSuccess() {
        pcRightPanelContent.Hide();
        onRefreshControl();
    }

    function oncboCancelRegistrationReasonChanged() {
        if (cboCancelRegistrationReason.GetValue() != 'X129^999')
            $('#<%=trReason.ClientID %>').attr('style', 'display:none');
        else
            $('#<%=trReason.ClientID %>').removeAttr('style');
    }
</script>
<div class="toolbarArea">
    <ul id="ulMPListToolbar" runat="server">
        <li id="btnVoidRegistration" runat="server">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbset.png")%>' alt="" /><br style="clear: both" />
            <div>
                <%=GetLabel("Process")%></div>
        </li>
    </ul>
</div>
<div style="padding: 10px;">
    <fieldset id="fsTrxPopup" style="margin: 0">
        <input type="hidden" id="hdnVisitID" runat="server" value="" />
        <input type="hidden" id="hdnTransactionID" runat="server" value="" />
        <input type="hidden" id="hdnParam" runat="server" value="" />
        <input type="hidden" id="hdnRegID" runat="server" value="" />   
        <input type="hidden" id="hdnDeptID" runat="server" value="" />
        <input type="hidden" id="hdnIsBridgingToGateway" runat="server" value="" />
        <input type="hidden" id="hdnProviderGatewayService" runat="server" value="" />
        <input type="hidden" id="hdnMenggunakanValidasiChiefComplaint" runat="server" value="" />
        <input type="hidden" id="hdnIsVoidRegistrationDeleteLinkedReg" runat="server" value="" />
        <input type="hidden" id="hdnIsBridgingToMobileJKN" value="0" runat="server" />
        <input type="hidden" id="hdnIsSendNotifToMobileJKN" value="0" runat="server" />
        <table>
            <colgroup>
                <col style="width: 250px" />
                <col style="width: 200px" />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Cancel Registration Reason")%></label>
                </td>
                <td>
                    <dxe:ASPxComboBox runat="server" ID="cboCancelRegistrationReason" ClientInstanceName="cboCancelRegistrationReason"
                        Width="200px">
                        <ClientSideEvents ValueChanged="function(s,e){ oncboCancelRegistrationReasonChanged(); }" />
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
        <dxcp:ASPxCallbackPanel ID="cbpCancelRegistration" runat="server" Width="100%"
            ClientInstanceName="cbpCancelRegistration" ShowLoadingPanel="false" OnCallback="cbpCancelRegistration_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel();}" EndCallback="function(s,e){
                    var result = s.cpResult.split('|');
                    if(result[0] == 'fail')
                        showToast('Save Failed', result[1]);
                    else
                        onReopenChargesSuccess();
                    hideLoadingPanel();
                }" />
        </dxcp:ASPxCallbackPanel>
    </fieldset>
</div>
