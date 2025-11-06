<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PhysiotherapyActionPlanCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PhysiotherapyActionPlanCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_physiotherapyactionplan">

    setDatePicker('<%=txtValueDateFrom.ClientID %>');

    setDatePicker('<%=txtValueDateTo.ClientID %>');

    $(function () {
        hideLoadingPanel();
    });

    $('#<%=btnPrint.ClientID %>').click(function (evt) {
        if (IsValid(evt, 'fsTrxPopup', 'mpTrxPopup'))
            cbpMedicalSickLeave.PerformCallback('Print');
        return false;
    });

    $('#<%=txtValueDateFrom.ClientID %>').change(function () {
        var date = $(this).val();
        var YYYY = date.substring(6, 10);
        var MM = date.substring(3, 5);
        var DD = date.substring(0, 2);
        var dateALL = YYYY + MM + DD;
        $('#<%=hdnTanggalDari.ClientID %>').val(dateALL);
        $('#<%=hdnTanggalDariString.ClientID %>').val(date);
    });

    $('#<%=txtValueDateTo.ClientID %>').change(function () {
        var date = $(this).val();
        var YYYY = date.substring(6, 10);
        var MM = date.substring(3, 5);
        var DD = date.substring(0, 2);
        var dateALL = YYYY + MM + DD;
        $('#<%=hdnTanggalSampai.ClientID %>').val(dateALL);
        $('#<%=hdnTanggalSampaiString.ClientID %>').val(date);
    });

    function getPayerCompanyFilterExpression() {
        var filterExpression = "<%:GetCustomerFilterExpression()%> AND GCCustomerType = '" + cboAppointmentPayer.GetValue() + "' AND IsDeleted = 0 AND IsActive = 1 AND IsBlacklist = 0";
        return filterExpression;
    }

    function onReprintPatientPaymentReceiptSuccess() {
        var appointmentID = $('#<%=hdnAppointmentID.ClientID %>').val();
        var from = $('#<%=hdnTanggalDari.ClientID %>').val();
        var to = $('#<%=hdnTanggalSampai.ClientID %>').val();
        var fromS = $('#<%=hdnTanggalDariString.ClientID %>').val();
        var toS = $('#<%=hdnTanggalSampaiString.ClientID %>').val();
        var payer = cboAppointmentPayer.GetValue();

        var errMessage = { text: "" };
        var filterExpression = appointmentID + "|" + from + "|" + to + "|" + payer;
        var CustomerType = cboAppointmentPayer.GetValue();
        var reportCodeBPJS = "PM-00696";
        var reportCodePersonal = "PM-00697";
        if (CustomerType != 'X004^999') {
            if (appointmentID == 0) {
                errMessage.text = "Tidak ada appointment pasien";
            }
            else {
                openReportViewer(reportCodeBPJS, filterExpression);
            }
        }
        if (CustomerType == 'X004^999') {
            if (appointmentID == 0) {
                errMessage.text = "Tidak ada appointment pasien";
            }
            else {
                openReportViewer(reportCodePersonal, filterExpression);
            }
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
        <input type="hidden" id="hdnAppointmentID" runat="server" value="" />
        <input type="hidden" id="hdnTanggalDari" runat="server" value="" />
        <input type="hidden" id="hdnTanggalSampai" runat="server" value="" />
        <input type="hidden" id="hdnTanggalDariString" runat="server" value="" />
        <input type="hidden" id="hdnTanggalSampaiString" runat="server" value="" />
        <table>
            <colgroup>
                <col style="width: 80px" />
                <col style="width: 80px" />
                <col />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Nama")%></label>
                </td>
                <td>
                    <td>
                        <asp:TextBox ID="txtPatientName" runat="server" Width="260px" ReadOnly="true" />
                    </td>
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("No RM")%></label>
                </td>
                <td>
                    <td>
                        <asp:TextBox ID="txtMedicalNo" runat="server" Width="120px" ReadOnly="true" />
                    </td>
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Tanggal Rencana")%></label>
                </td>
                <td>
                    <td class="tdCustomDate">
                        <asp:TextBox ID="txtValueDateFrom" CssClass="txtValueDateFrom datepicker required"
                            runat="server" Width="120px" />
                        -
                        <asp:TextBox ID="txtValueDateTo" CssClass="txtValueDateTo datepicker required" runat="server"
                            Width="120px" />
                    </td>
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%:GetLabel("Pembayar")%></label>
                </td>
                <td>
                </td>
                <td>
                    <dxe:ASPxComboBox ID="cboAppointmentPayer" ClientInstanceName="cboAppointmentPayer"
                        Width="100%" runat="server">
                    </dxe:ASPxComboBox>
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
