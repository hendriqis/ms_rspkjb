<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PrintPatientLabelMCUCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PrintPatientLabelMCUCtl" %>
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
            cbpPrintPatientLabel.PerformCallback('Print');
        }
    });

    function onReprintPatientPaymentReceiptSuccess() {
        
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
        <input type="hidden" id="hdnDepartmentID" runat="server" value="" />
        <input type="hidden" id="hdnIsLaboratoryUnit" runat="server" value="" />
        <input type="hidden" id="hdnIsImagingUnit" runat="server" value="" />
        <input type="hidden" id="hdnJumlahPrint" runat="server" value="" />
        <input type="hidden" id="hdnRegistrationStatus" runat="server" value="" />
        <input type="hidden" id="hdnMaxLabelNo" runat="server" value="" />
        <input type="hidden" id="hdnIsMultiLocation" runat="server" value="" />
        <input type="hidden" id="hdnRegistrationDate" runat="server" value="" />
        <input type="hidden" id="hdnGCCustomerType" runat="server" value="" />
        <input type="hidden" id="hdnBusinessPartnerID" runat="server" value="" />
        <input type="hidden" id="hdnHealthcareServiceUnitImagingID" runat="server" value="" />
        <input type="hidden" id="hdnHealthcareServiceUnitLaboratoryID" runat="server" value="" />
        <input type="hidden" id="hdnPrintFormatLabel" runat="server" value="" />
        <input type="hidden" id="hdnReportCodeCustomLabel" runat="server" value="" />
        <input type="hidden" id="hdnRMFormatLabel2" runat="server" value="" />
        <table>
            <colgroup>
                <col style="width: 15%" />
                <col style="width: 85%" />
                <col />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label>
                        <%=GetLabel("Jumlah Print")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtJmlLabel" runat="server" TextMode="Number"/>
                </td>
            </tr>
            <tr id="trPrinterLocation" runat="server">
                <td class="tdLabel">
                    <label>
                        <%=GetLabel("Lokasi")%></label>
                </td>
                <td>
                    <dxe:ASPxComboBox ID="cboPrinterUrl" ClientInstanceName="cboPrinterUrl"
                        Width="38%" runat="server">
                    </dxe:ASPxComboBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:RadioButtonList ID="rbReportCode" runat="server">
                        <asp:ListItem Text="Label Rekam Medis" Value="PM-00105" Selected="True"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
        </table>
        <div class="imgLoadingGrdView" id="containerImgLoadingView">
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
        <dxcp:ASPxCallbackPanel ID="cbpPrintPatientLabel" runat="server" Width="100%" ClientInstanceName="cbpPrintPatientLabel"
            ShowLoadingPanel="false" OnCallback="cbpPrintPatientLabel_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel();}" EndCallback="function(s,e){
                    onReprintPatientPaymentReceiptSuccess();
                    hideLoadingPanel();
                }" />
        </dxcp:ASPxCallbackPanel>
    </fieldset>
</div>
