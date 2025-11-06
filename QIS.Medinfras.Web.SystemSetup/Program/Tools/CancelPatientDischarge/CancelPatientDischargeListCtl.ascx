<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CancelPatientDischargeListCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.SystemSetup.Program.CancelPatientDischargeListCtl" %>
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

    $('#<%=btnReopenCharges.ClientID %>').click(function (evt) {
        if (IsValid(evt, 'fsTrxPopup', 'mpTrxPopup')) {
            cbpCancelDischargeReason.PerformCallback('reopen');
        }
    });

    function onReopenChargesSuccess() {
        pcRightPanelContent.Hide();
        onRefreshControl();
    }

    function oncboCancelDischargeReasonChanged() {
        if (cboCancelDischargeReason.GetValue() != 'X129^999')
            $('#<%=trReason.ClientID %>').attr('style', 'display:none');
        else
            $('#<%=trReason.ClientID %>').removeAttr('style');
    }
</script>
<div class="toolbarArea">
    <ul id="ulMPListToolbar" runat="server">
        <li id="btnReopenCharges" runat="server">
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
        <table>
            <colgroup>
                <col style="width: 250px" />
                <col style="width: 200px" />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Cancel Discharge Reason")%></label>
                </td>
                <td>
                    <dxe:ASPxComboBox runat="server" ID="cboCancelDischargeReason" ClientInstanceName="cboCancelDischargeReason"
                        Width="200px">
                        <ClientSideEvents ValueChanged="function(s,e){ oncboCancelDischargeReasonChanged(); }" />
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
        <dxcp:ASPxCallbackPanel ID="cbpCancelDischargeReason" runat="server" Width="100%" ClientInstanceName="cbpCancelDischargeReason"
            ShowLoadingPanel="false" OnCallback="cbpCancelDischargeReason_Callback">
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
