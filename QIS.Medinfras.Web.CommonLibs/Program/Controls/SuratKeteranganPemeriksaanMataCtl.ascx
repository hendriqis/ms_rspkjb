<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SuratKeteranganPemeriksaanMataCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.SuratKeteranganPemeriksaanMataCtl" %>
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
        var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
        var mataKanan = $('#<%=txtMataKanan.ClientID %>').val();
        var mataKananKoreksi = $('#<%=txtMataKananKoreksi.ClientID %>').val();
        var mataKiri = $('#<%=txtMataKiri.ClientID %>').val();
        var mataKiriKoreksi = $('#<%=txtMataKiriKoreksi.ClientID %>').val();
        var type = $('#<%=rblItemType.ClientID %>').find('input:checked').val();
        var lainlain = $('#<%=txtLainlain.ClientID %>').val();

        var filterExpression = "RegistrationID = " + registrationID + "|" + mataKanan + "|" + mataKananKoreksi + "|" + mataKiri + "|" + mataKiriKoreksi + "|" + type + "|" + lainlain;
        var reportCode = "PM-00518";
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
        <input type="hidden" id="hdntype" runat="server" value="" />
        <table>
            <colgroup>
                <col style="width: 150px" />
                <col style="width: 150px" />
                <col />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Mata Kanan :")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtMataKanan" Width="70px" CssClass="required" runat="server" />
                </td>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Koreksi :")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtMataKananKoreksi" Width="70px" CssClass="required" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Mata Kiri :")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtMataKiri" Width="70px" CssClass="required" runat="server" />
                </td>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Koreksi :")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtMataKiriKoreksi" Width="70px" CssClass="required" runat="server" />
                </td>
            </tr>
            <tr id="trType" runat="server">
                <td>
                    <asp:RadioButtonList ID="rblItemType" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="Tidak Buta Warna" Value="0" Selected="True" />
                        <asp:ListItem Text="Buta Warna" Value="1" />
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Lain - Lain :")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtLainlain" Width="150px" CssClass="required" runat="server" TextMode="Multiline"
                        Rows="2" />
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
