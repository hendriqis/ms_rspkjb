<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ARReceiptReprintCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.ARReceiptReprintCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_ARReceiptReprintCtl">
    $(function () {
        hideLoadingPanel();
    });

    $('#<%=btnReprintReceipt.ClientID %>').click(function (evt) {
        if (IsValid(evt, 'fsTrxPopup', 'mpTrxPopup')) {
            cbpARReceiptReprint.PerformCallback('Reprint');
        }
    });

    function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
        var receiptID = $('#<%=hdnARReceiptIDCtl.ClientID%>').val();

        if (receiptID == '') {
            errMessage.text = 'Cannot Print Payment Receipt';
            return false;
        }
        else {
            filterExpression.text = receiptID;
            return true;
        }
    }

    function onReprintARReceiptSuccess() {
        var arReceiptID = $('#<%=hdnARReceiptIDCtl.ClientID %>').val();
        var errMessage = { text: "" };
        var filterExpression = { text: "ARReceiptID = " + arReceiptID };
        var reportCode = $('#<%=hdnReportCodeCtl.ClientID%>').val();

        openReportViewer(reportCode, filterExpression.text);
        pcRightPanelContent.Hide();
        cbpView.PerformCallback('refresh');
    }

    function onCboReprintReasonChanged() {
        if (cboReprintReason.GetValue() != 'X236^999')
            $('#<%=trReason.ClientID %>').attr('style', 'display:none');
        else
            $('#<%=trReason.ClientID %>').removeAttr('style');
    }
</script>
<div class="toolbarArea">
    <ul id="ulMPListToolbar" runat="server">
        <li id="btnReprintReceipt" runat="server">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbset.png")%>' alt="" /><br style="clear: both" />
            <div>
                <%=GetLabel("Process")%></div>
        </li>
    </ul>
</div>
<div style="padding: 10px;">
    <fieldset id="fsTrxPopup" style="margin: 0">
        <input type="hidden" id="hdnARReceiptIDCtl" runat="server" value="" />
        <input type="hidden" id="hdnReportCodeCtl" runat="server" value="0" />
        <table>
            <colgroup>
                <col style="width: 250px" />
                <col style="width: 200px" />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Reprint Reason")%></label>
                </td>
                <td>
                    <dxe:ASPxComboBox runat="server" ID="cboReprintReason" ClientInstanceName="cboReprintReason"
                        Width="200px">
                        <ClientSideEvents ValueChanged="function(s,e){ onCboReprintReasonChanged(); }" />
                    </dxe:ASPxComboBox>
                </td>
            </tr>
            <tr id="trReason" runat="server" style="display: none">
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Reason")%></label>
                </td>
                <td>
                    <asp:textbox id="txtReason" width="200px" runat="server" />
                </td>
            </tr>
        </table>
        <div class="imgLoadingGrdView" id="containerImgLoadingView">
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
        <dxcp:ASPxCallbackPanel ID="cbpARReceiptReprint" runat="server" Width="100%"
            ClientInstanceName="cbpARReceiptReprint" ShowLoadingPanel="false"
            OnCallback="cbpARReceiptReprint_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel();}" EndCallback="function(s,e){
                    var result = s.cpResult.split('|');
                    if(result[0] == 'fail')
                        showToast('Save Failed', result[1]);
                    else
                        onReprintARReceiptSuccess();
                    hideLoadingPanel();
                }" />
        </dxcp:ASPxCallbackPanel>
    </fieldset>
</div>
