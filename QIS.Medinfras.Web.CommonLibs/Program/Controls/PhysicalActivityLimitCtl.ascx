<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PhysicalActivityLimitCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.MedicalSickLeaveBilingualCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_medicalsickleave">

    setDatePicker('<%=txtValueDateFrom.ClientID %>');

    setDatePicker('<%=txtValueDateTo.ClientID %>');

    $(function () {
        hideLoadingPanel();
    });

    $('#<%=btnPrint.ClientID %>').click(function (evt) {
        if (IsValid(evt, 'fsTrxPopup', 'mpTrxPopup')) {
            cbpMedicalSickLeave.PerformCallback('Print');
        }
    });

    $('#<%=txtValueDateFrom.ClientID %>').change(function () {
        var date = $(this).val();
        var YYYY = date.substring(6, 10);
        var MM = date.substring(3, 5);
        var DD = date.substring(0, 2);
        var dateALL = YYYY + '-' + MM + '-' + DD;
        $('#<%=hdnTanggalDari.ClientID %>').val(dateALL);
        $('#<%=hdnTanggalDariString.ClientID %>').val(date);
    });

    $('#<%=txtValueDateTo.ClientID %>').change(function () {
        var date = $(this).val();
        var YYYY = date.substring(6, 10);
        var MM = date.substring(3, 5);
        var DD = date.substring(0, 2);
        var dateALL = YYYY + '-' + MM + '-' + DD;
        $('#<%=hdnTanggalSampai.ClientID %>').val(dateALL);
        $('#<%=hdnTanggalSampaiString.ClientID %>').val(date);
    });

    function onReprintPatientPaymentReceiptSuccess() {
        var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
        var from = $('#<%=hdnTanggalDari.ClientID %>').val();
        var to = $('#<%=hdnTanggalSampai.ClientID %>').val();
        var fromS = $('#<%=hdnTanggalDariString.ClientID %>').val();
        var toS = $('#<%=hdnTanggalSampaiString.ClientID %>').val();

        var errMessage = { text: "" };
        var filterExpression = registrationID + "|" + from + "|" + to + "|" + fromS + "|" + toS;
        var reportCode = "PM-00140";
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
        <input type="hidden" id="hdnTanggalDari" runat="server" value="" />
        <input type="hidden" id="hdnTanggalSampai" runat="server" value="" />
        <input type="hidden" id="hdnTanggalDariString" runat="server" value="" />
        <input type="hidden" id="hdnTanggalSampaiString" runat="server" value="" />
        <table>
            <colgroup>
                <col style="width: 100px" />
                <col style="width: 80px" />
                <col />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Tanggal Ijin")%></label>
                </td>
                <td>
                    <td class="tdCustomDate">
                        <asp:TextBox ID="txtValueDateFrom" CssClass="txtValueDateFrom datepicker" runat="server"
                            Width="120px" />
                        -
                        <asp:TextBox ID="txtValueDateTo" CssClass="txtValueDateTo datepicker" runat="server"
                            Width="120px" />
                    </td>
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
