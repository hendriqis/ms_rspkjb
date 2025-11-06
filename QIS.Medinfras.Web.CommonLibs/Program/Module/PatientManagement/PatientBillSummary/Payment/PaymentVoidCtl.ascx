<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PaymentVoidCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PaymentVoidCtl" %>
    
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascript" id="dxss_patientpaymentvoidctl">
    $(function () {
        hideLoadingPanel();
    });

    $('#<%=btnVoidPayment.ClientID %>').click(function (evt) {
        if (IsValid(evt, 'fsTrxPopup', 'mpTrxPopup')) {
            cbpPatientPaymentVoid.PerformCallback('void');
        }
    });

    function onVoidPatientPaymentSuccess() {
        pcRightPanelContent.Hide();
        onRefreshControl();
        getStatusPerRegOutstanding(); //general ctl
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
        <li id="btnVoidPayment" runat="server"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbset.png")%>' alt="" /><br style="clear:both"/>
            <div><%=GetLabel("Process")%></div>
        </li>
    </ul>
</div>
<style>
.hide{display:none;}
</style>
<div style="padding:10px;">
    <fieldset id="fsTrxPopup" style="margin:0"> 
                        
        <input type="hidden" id="hdnRegistrationIDCtlVoid" runat="server" value="" />
        <input type="hidden" id="hdnPaymentIDCtlVoid" runat="server" value="" />
        <input type="hidden" id="hdnOutstandingDPCtlVoid" runat="server" value="" />
        <input type="hidden" id="hdnParam" runat="server" value="" />
        <input type="hidden" id="hdnEdcTransaction" value="0" runat="server" />
        <input type="hidden" id="hdnIsSetEdcBridging" value="" runat="server" />
        <input type="hidden" id="hdnIsBridgingToPaymentGatewayCtlVoid" value="0" runat="server" />
        <input type="hidden" id="hdnIsUsingBlockerValidateRevenueSharing" value="0" runat="server" />
        <input type="hidden" id="hdnIsUsingBlockerValidatePaymentReconOrUserPaymentBalance" value="0" runat="server" />
        <table>
            <colgroup>
                <col style="width:250px"/>
                <col style="width:200px"/>
            </colgroup>
            <tr>
                <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Void Reason")%></label></td>
                <td>
                    <dxe:ASPxComboBox runat="server" ID="cboVoidReason" ClientInstanceName="cboVoidReason" Width="200px">
                        <ClientSideEvents ValueChanged="function(s,e){ onCboVoidReasonChanged(); }" />
                    </dxe:ASPxComboBox>
                </td>
            </tr>
            <tr id="trReason" runat="server" style="display:none">
                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Reason")%></label></td>
                <td><asp:TextBox ID="txtReason" Width="200px" runat="server" /></td>
            </tr>
             <tr id="trNotes" runat="server" class="hide">
                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Peringatan")%></label></td>
                <td><span id="NoteText" runat="server" style="color:Red"></span></td>
            </tr>
        </table>
    
        <div class="imgLoadingGrdView" id="containerImgLoadingView" >
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div> 

        <dxcp:ASPxCallbackPanel ID="cbpPatientPaymentVoid" runat="server" Width="100%" ClientInstanceName="cbpPatientPaymentVoid"
            ShowLoadingPanel="false" OnCallback="cbpPatientPaymentVoid_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel();}" 
                EndCallback="function(s,e){
                    var result = s.cpResult.split('|');
                    if(result[0] == 'fail')
                        showToast('Save Failed', result[1]);
                    else
                        onVoidPatientPaymentSuccess();
                    hideLoadingPanel();
                }" />
        </dxcp:ASPxCallbackPanel>
    </fieldset>
</div>

