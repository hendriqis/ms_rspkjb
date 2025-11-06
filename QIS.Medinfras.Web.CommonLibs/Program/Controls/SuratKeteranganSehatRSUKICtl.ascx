<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SuratKeteranganSehatRSUKICtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.SuratKeteranganSehatRSUKICtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_medicalsickleave">

    $(function () {
        hideLoadingPanel();
    });

    $('#<%=btnPrint.ClientID %>').click(function (evt) {
        if (IsValid(evt, 'fsTrxPopup', 'mpTrxPopup')) {
            cbpMedicalSickLeave.PerformCallback('Print');
        }
    });

    function onReprintPatientPaymentReceiptSuccess() {
        var resultText = $('#<%=txtDocumentResult.ClientID %>').val();
        var documentText = $('#<%=txtDocumentText.ClientID %>').val();
        var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
        var visitID = $('#<%=hdnVisitID.ClientID %>').val();
        var filterExpression = "VisitID = " + visitID + "|" + documentText + "|" + resultText;
        var reportCode = "PM-00576";
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
        <input type="hidden" id="hdnVisitID" runat="server" value="" />
        <table>
            <colgroup>
                <col style="width: 120px" />
                <col style="width: 80px" />
                <col />
            </colgroup>
             <tr>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Hasil :")%></label>
                </td>
                 <td>
                    <asp:TextBox ID="txtDocumentResult" Width="300px" CssClass="required" runat="server"
                        TextMode="Multiline" Rows="2" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Dipergunakan untuk :")%></label>
                </td>
                 <td>
                    <asp:TextBox ID="txtDocumentText" Width="300px" CssClass="required" runat="server"
                        TextMode="Multiline" Rows="2" />
                </td>
            </tr>
        </table>
        <div class="imgLoadingGrdView" id="containerImgLoadingView">
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
        <dxcp:ASPxCallbackPanel ID="cbpMedicalSickLeave" runat="server" Width="100%" ClientInstanceName="cbpMedicalSickLeave"
            ShowLoadingPanel="false" OnCallback="cbpMedicalSickLeave_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel();}" EndCallback="function(s,e){
                    onReprintPatientPaymentReceiptSuccess();
                    hideLoadingPanel();
                }" />
        </dxcp:ASPxCallbackPanel>
    </fieldset>
</div>
