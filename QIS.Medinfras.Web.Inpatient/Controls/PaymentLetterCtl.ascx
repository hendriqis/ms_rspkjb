<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PaymentLetterCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Inpatient.Controls.PaymentLetterCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_paymentletterctl">

    $(function () {
        hideLoadingPanel();
    });

    $('#<%=btnPrint.ClientID %>').click(function (evt) {
        if (IsValid(evt, 'fsTrxPopup', 'mpTrxPopup')) {
            cbpPaymentLetter.PerformCallback('Print');
        }
    });

    function onReprintPatientPaymentReceiptSuccess() {
        var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
        var payementLetterAmount = $('#<%=txtPayerLetterAmount.ClientID %>').val();
        var id = $('#<%=hdnID.ClientID %>').val();
        var errMessage = { text: "" };
        var filterExpression = registrationID + "|" + payementLetterAmount;

        if (id == 'pl') {
            filterExpression += "|" + id;
        }
        else if (id == 'pdl') {
            filterExpression += "|" + id;
        }
        
        var reportCode = "IP-00205";
        if (reportCode != "") {
            openReportViewer(reportCode, filterExpression);
        }
        pcRightPanelContent.Hide();
        cbpView.PerformCallback('refresh');
    }

</script>
<div class="toolbarArea">
    <ul id="ulMPListToolbar" runat="server">
        <li id="btnPrint" runat="server">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprint.png")%>' alt="" /><br style="clear: both" />
            <div>
                <%=GetLabel("Print")%></div>
        </li>
    </ul>
</div>
<div style="padding: 10px;">
    <fieldset id="fsTrxPopup" style="margin: 0">
        <input type="hidden" id="hdnRegistrationID" runat="server" value="" />
        <input type="hidden" id="hdnPaymentLetterAmount" runat="server" value="" />
        <input type="hidden" id="hdnID" runat="server" value="" />
        <table>
            <colgroup>
                <col style="width: 120px" />
                <col style="width: 50px" />
                <col />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Jumlah Rupiah")%></label>
                </td>
                <td>
                    <td class="tdPayerLetter">
                        <asp:TextBox ID="txtPayerLetterAmount" CssClass="txtCurrency" runat="server" 
                            Width="150px" />
                    </td>
                </td>
            </tr>
        </table>
        <div class="imgLoadingGrdView" id="containerImgLoadingView">
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
        <dxcp:ASPxCallbackPanel ID="cbpPaymentLetter" runat="server" Width="100%" ClientInstanceName="cbpPaymentLetter"
            ShowLoadingPanel="false" OnCallback="cbpPaymentLetter_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel();}" EndCallback="function(s,e){
                    onReprintPatientPaymentReceiptSuccess();
                    hideLoadingPanel();
                }" />
        </dxcp:ASPxCallbackPanel>
    </fieldset>
</div>
