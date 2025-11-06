<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PaymentAccountReceivableCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PaymentAccountReceivableCtl" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
    
<script type="text/javascript" id="dxss_serviceunithealthcareentryctl">
    $(function () {
        $('#btnPopupYes').click(function () {
            cbpProcessPopup.PerformCallback();
        });
    });

    function onCbpProcessPopupEndCallback(s) {
        var param = s.cpResult.split('|');
        var isCloseRegistration = s.cpIsCloseRegistration;
        if (param[0] == 'fail')
            showToast('Process Failed', 'Error Message : ' + param[1]);
        else {
            showToast('Process Success', s.cpRetval, function () {
                var count = parseInt(s.cpCount);
                for (var i = 0; i < count; ++i)
                    onAfterAddRecordAddRowCount();
                pcRightPanelContent.Hide();
            });
        }

        hideLoadingPanel();
    }
     
</script>

<input type="hidden" id="hdnTotalPatientAmountCtlAR" value="0" runat="server" />
<input type="hidden" id="hdnTotalPayerAmountCtlAR" value="0" runat="server" />
<input type="hidden" id="hdnPatientBillingIDCtlAR" value="" runat="server" />
<input type="hidden" id="hdnRegistrationIDCtlAR" value="" runat="server" />
<input type="hidden" id="hdnDepartmentIDCtlAR" value="" runat="server" />
<input type="hidden" id="hdnCashierGroupCtlAR" value="" runat="server" />
<input type="hidden" id="hdnShiftCtlAR" value="" runat="server" />
<input type="hidden" id="hdnTxtPaymentDateCtlAR" value="" runat="server" />
<input type="hidden" id="hdnTxtPaymentTimeCtlAR" value="" runat="server" />
<input type="hidden" id="hdnTglPiutangPribadiCtlAR" value="" runat="server" />
<input type="hidden" value="" id="hdnIsGrouperAmountClaimDefaultZeroCtlAR" runat="server" />
<input type="hidden" value="" id="hdnIsFinalisasiKlaimAfterARInvoiceCtlAR" runat="server" />
<input type="hidden" value="" id="hdnIsFinalisasiKlaimBeforeARInvoiceAndSkipProcessKlaimCtlAR" runat="server" />

<table class="tblContentArea">
    <colgroup>
        <col style="width:170px"/>
    </colgroup>
    <tr id="trPatientAmount" runat="server">
        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Sisa Tagihan Pasien") %></label></td>
        <td><asp:TextBox ID="txtPatientBillAmount" Width="150px" ReadOnly="true" CssClass="number" runat="server" /></td> 
    </tr>
    <tr id="trPayerAmount" runat="server">
        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Sisa Tagihan Instansi") %></label></td>
        <td><asp:TextBox ID="txtPayerBillAmount" Width="150px" ReadOnly="true" CssClass="number" runat="server" /></td> 
    </tr>
</table>
<div style="margin: 5px;">
    <%=GetLabel("Proses Menjadi Piutang ?") %> 
    <input type="button" id="btnPopupYes" value='<%=GetLabel("Ya") %>' />
    <input type="button" value='<%=GetLabel("Tidak") %>' onclick="pcRightPanelContent.Hide();" />
</div>

<dxcp:ASPxCallbackPanel runat="server" ID="cbpProcessPopup" ClientInstanceName="cbpProcessPopup" ShowLoadingPanel="false"
    OnCallback="cbpProcessPopup_Callback">
    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e) { onCbpProcessPopupEndCallback(s); }" />
</dxcp:ASPxCallbackPanel>

