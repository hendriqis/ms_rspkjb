<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SuratKeteranganHariPerkiraanLahirCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.SuratKeteranganHariPerkiraanLahirCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_medicalsickleave">

    setDatePicker('<%=txtDate.ClientID %>');

    $(function () {
        hideLoadingPanel();
    });

    $('#<%=btnPrint.ClientID %>').click(function (evt) {
        if (IsValid(evt, 'fsTrxPopup', 'mpTrxPopup')) {
            cbpMedicalSickLeave.PerformCallback('Print');
        }
    });

    $('#<%=txtDate.ClientID %>').change(function () {
        var date = $(this).val();
        var YYYY = date.substring(6, 10);
        var MM = date.substring(3, 5);
        var DD = date.substring(0, 2);
        var dateALL = DD + '-' + MM + '-' + YYYY;
        var today = moment().format('DD + ' - ' + MM + ' - ' + YYYY');
        $('#<%=hdnTanggalDari.ClientID %>').val(today);
    });

    function onReprintPatientPaymentReceiptSuccess() {
        var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
        var from = $('#<%=txtDate.ClientID %>').val();
        var noinduk = $('#<%=txtNoInduk.ClientID %>').val();
        var bagian = $('#<%=txtBagian.ClientID %>').val();

        var errMessage = { text: "" };
        var filterExpression = "RegistrationID = " + registrationID + "|" + noinduk + "|" + bagian + "|" + from;
        var reportCode = "PM-00526";

        if ($('#<%=hdnHealthcareInitial.ClientID %>').val() == "rsdo-ska") {
            reportCode = "PM-00703";
        }

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
        <input type="hidden" id="hdnHealthcareInitial" runat="server" value="" />
        <input type="hidden" id="hdnRegistrationID" runat="server" value="" />
        <input type="hidden" id="hdnTanggalDari" runat="server" value="" />
        <table>
            <colgroup>
                <col style="width: 150px" />
                <col style="width: 150px" />
                <col />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Tanggal :")%></label>
                </td>
                <td class="tdCustomDate">
                    <asp:TextBox ID="txtDate" CssClass="txtDate datepicker" runat="server" Width="100px" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("No Induk :")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtNoInduk" Width="250px" CssClass="required" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Bagian :")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtBagian" Width="250px" CssClass="required" runat="server" />
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
