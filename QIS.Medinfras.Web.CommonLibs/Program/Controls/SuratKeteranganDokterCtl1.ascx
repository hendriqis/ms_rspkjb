<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SuratKeteranganDokterCtl1.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.SuratKeteranganDokterCtl1" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_medicalsickleave">

    setDatePicker('<%=txtValueDate.ClientID %>');


    $(function () {
        hideLoadingPanel();
    });

    $('#<%=btnPrint.ClientID %>').click(function (evt) {
        if (IsValid(evt, 'fsTrxPopup', 'mpTrxPopup')) {
            cbpMedicalSickLeave.PerformCallback('Print');
        }
    });

    $('#<%=txtMingguHamil.ClientID %>').change(function () {
        var MingguHamil = $('#<%=txtMingguHamil.ClientID %>').val();
        $('#<%=hdnMingguHamil.ClientID %>').val(MingguHamil);
    });

    $('#<%=txtHariHamil.ClientID %>').change(function () {
        var HariHamil = $('#<%=txtHariHamil.ClientID %>').val();
        $('#<%=hdnHariHamil.ClientID %>').val(HariHamil);
    });

    $('#<%=txtValueDate.ClientID %>').change(function () {
        var date = $(this).val();
        var YYYY = date.substring(6, 10);
        var MM = date.substring(3, 5);
        var DD = date.substring(0, 2);
        var dateALL = YYYY + '-' + MM + '-' + DD;
        $('#<%=hdnTanggal.ClientID %>').val(dateALL);
        $('#<%=hdnTanggalString.ClientID %>').val(date);
    });

    function onReprintPatientPaymentReceiptSuccess() {
        var from = $('#<%=hdnTanggal.ClientID %>').val();
        var txtMingguHamil = $('#<%=hdnMingguHamil.ClientID %>').val();
        var txtHariHamil = $('#<%=hdnHariHamil.ClientID %>').val();
        var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
        var visitID = $('#<%=hdnVisitID.ClientID %>').val();

        var filterExpression = "VisitID = " + visitID + "|" + txtMingguHamil + "|" + txtHariHamil + "|" + from
        var reportCode = "PM-00860";
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
        <input type="hidden" id="hdnTanggalString" runat="server" value="" />
        <input type="hidden" id="hdnTanggal" runat="server" value="" />
        <input type="hidden" id="hdnReportCode" runat="server" value="" />
        <input type="hidden" id="hdnReportID" runat="server" value="" />
        <input type="hidden" id="hdnInitialHealtcare" runat="server" value="" />
        <input type="hidden" id="hdnMingguHamil" runat="server" value="" />
        <input type="hidden" id="hdnHariHamil" runat="server" value="" />
        <table>
            <colgroup>
                <col style="width: 100px" />
                <col style="width: 100px" />
                <col />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Telah Hamil :")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtMingguHamil" Width="70px" CssClass="required" runat="server" TextMode="Number" oninput="this.value = this.value.replace(/[^0-9]/g, '');" />
                </td>
                <td class="tdLabel">
                    <label>
                        <%=GetLabel("Minggu")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtHariHamil" Width="70px" CssClass="required" runat="server" TextMode="Number" oninput="this.value = this.value.replace(/[^0-9]/g, '');" />
                </td>
                <td class="tdLabel">
                    <label>
                        <%=GetLabel("Hari")%></label>
                </td>
            </tr>
        </table>
        <table style="display:none">
            <colgroup>
                <col style="width: 150px" />
                <col style="width: 150px" />
                <col />
            </colgroup>
            <tr>
            <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Diperkiran Lahir :")%></label>
                </td>
             <td class="tdCustomDate">
              <asp:TextBox ID="txtValueDate" CssClass="txtValueDate datepicker" runat="server"
                            Width="100px" />
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
