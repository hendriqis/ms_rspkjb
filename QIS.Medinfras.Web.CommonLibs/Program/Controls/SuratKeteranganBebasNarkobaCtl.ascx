<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SuratKeteranganBebasNarkobaCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.SuratKeteranganBebasNarkobaCtl" %>
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
            cbpDrugFreeCertificate.PerformCallback('Print');
        }
    });

    function onReprintPatientPaymentReceiptSuccess() {
        var visitID = $('#<%=hdnVisitID.ClientID %>').val();
        var remarks = $('#<%=txtRemarks.ClientID %>').val();
        var reason = $('#<%=txtReason.ClientID %>').val();
        var filterExpression = "VisitID = " + visitID + "|" + remarks + "|" + reason;
        var reportCode = "LB-00023";
        if (reportCode != "") {
            openReportViewer(reportCode, filterExpression);
        }
        pcRightPanelContent.Hide();
        cbpView.PerformCallback('refresh');


        if (reportCode != "") {
            openReportViewer(reportCode, filterExpression);
        }
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
        <input type="hidden" id="hdnDepartmentID" runat="server" value="" />
        <input type="hidden" id="hdnIsLaboratoryUnit" runat="server" value="" />
        <input type="hidden" id="hdnIsImagingUnit" runat="server" value="" />
        <input type="hidden" id="hdnReasonRemarks" runat="server" value="" />
        <input type="hidden" id="hdnRemarks" runat="server" value="" />
        <table>
            <colgroup>
                <col style="width: 250px" />
                <col style="width: 50px" />
                <col />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label>
                        <%=GetLabel("Notes / Remarks")%></label>
                </td>
                    <td>
                        <asp:TextBox ID="txtRemarks" runat="server" Width="300px" />
                    </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory" >
                        <%=GetLabel("Keterangan Surat")%></label>
                </td>
                    <td>
                        <asp:TextBox ID="txtReason" runat="server" Width="300px" />
                    </td>
            </tr>
        </table>
        <div class="imgLoadingGrdView" id="containerImgLoadingView">
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
        <dxcp:ASPxCallbackPanel ID="cbpDrugFreeCertificate" runat="server" Width="100%" ClientInstanceName="cbpDrugFreeCertificate"
            ShowLoadingPanel="false" OnCallback="cbpDrugFreeCertificate_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel();}" EndCallback="function(s,e){
                    onReprintPatientPaymentReceiptSuccess();
                    hideLoadingPanel();
                }" />
        </dxcp:ASPxCallbackPanel>
    </fieldset>
</div>
