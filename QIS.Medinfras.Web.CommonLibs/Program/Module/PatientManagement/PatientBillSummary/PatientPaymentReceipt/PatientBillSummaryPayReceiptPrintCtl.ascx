<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PatientBillSummaryPayReceiptPrintCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientBillSummaryPayReceiptPrintCtl" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascript" id="dxss_patientpaymentreceiptdetailctl">
    $(function () {
        hideLoadingPanel();
    });

    $('#<%=btnReceiptPrint.ClientID %>').click(function (evt) {
        if (IsValid(evt, 'fsTrxPopup', 'mpTrxPopup')) {
            cbpPatientPaymentReceiptProcess.PerformCallback('save');
        }
    });

    function onSavePatientPaymentReceiptSuccess() {
        //var errMessage = { text: "" };
        var filterExpression = "";
        var reportCode = "PM-00401";
        if (reportCode != '') {
            var isAllowPrint = true;
            //if (typeof onBeforeRightPanelPrint == 'function') {
              //  isAllowPrint = true; //onBeforeRightPanelPrint(reportCode, filterExpression, errMessage);
            //}
            //if (isAllowPrint) {
            filterExpression = "PaymentReceiptID = " + $('#<%=hdnPaymentReceipt.ClientID %>').val();
                openReportViewer(reportCode, filterExpression);
            //}
            //else
              //  showToast('Warning', errMessage.text);
        }
        pcRightPanelContent.Hide();
        cbpView.PerformCallback('refresh');
    }

</script>
<div class="toolbarArea">
    <ul id="ulMPListToolbar" runat="server">
        <li id="btnReceiptPrint" runat="server"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprint.png")%>' alt="" /><br style="clear:both"/><div><%=GetLabel("Print")%></div></li>
    </ul>
</div>

<div style="padding:10px;">
    <fieldset id="fsTrxPopup" style="margin:0"> 
                        
        <input type="hidden" id="hdnPaymentID" runat="server" value="" />
        <input type="hidden" id="hdnRegistrationID" runat="server" value="" />
        <input type="hidden" id="hdnDepartmentID" runat="server" value="" />
        <input type="hidden" id="hdnParam" runat="server" value="" />
        <input type="hidden" id="hdnPatientName" runat="server" value="" />
        <input type="hidden" id="hdnPaymentReceipt" runat="server" value="" />
        <input type="hidden" id="hdnParam1" runat="server" value="" />
        <input type="hidden" id="hdnParam2" runat="server" value="" />

        <table>
            <colgroup>
                <col style="width:150px"/>
                <col style="width:400px"/>
            </colgroup>
            <tr>
                <td class="tdLabel"><label class="lblMandatory" runat="server"><%=GetLabel("Cetak Atas Nama")%></label></td>
                <td><asp:TextBox ID="txtReceiptName" Width="200px" runat="server" /></td>
            </tr>
        </table>
    
        <div class="imgLoadingGrdView" id="containerImgLoadingView" >
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div> 

        <dxcp:ASPxCallbackPanel ID="cbpPatientPaymentReceiptProcess" runat="server" Width="100%" ClientInstanceName="cbpPatientPaymentReceiptProcess"
            ShowLoadingPanel="false" OnCallback="cbpPatientPaymentReceiptProcess_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel();}" 
                EndCallback="function(s,e){
                    var result = s.cpResult.split('|');
                    if(result[0] == 'fail')
                        showToast('Save Failed', result[1]);
                    else
                        onSavePatientPaymentReceiptSuccess();
                    hideLoadingPanel();
                }" />
        </dxcp:ASPxCallbackPanel>
    </fieldset>
</div>


