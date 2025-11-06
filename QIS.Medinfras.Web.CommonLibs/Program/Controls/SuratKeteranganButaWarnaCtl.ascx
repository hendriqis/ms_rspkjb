<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SuratKeteranganButaWarnaCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.SuratKeteranganButaWarnaCtl" %>
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
        var visitID = $('#<%=hdnVisitID.ClientID %>').val();
        var mataKananTanpaKacamata = $('#<%=txtMataKananTanpaKacamata.ClientID %>').val();
        var mataKiriTanpaKacamata = $('#<%=txtMataKiriTanpaKacamata.ClientID %>').val();
        var mataKananDenganKacamata = $('#<%=txtMataKananDenganKacamata.ClientID %>').val();
        var mataKiriDenganKacamata = $('#<%=txtMataKiriDenganKacamata.ClientID %>').val();
        var type = $('#<%=rblItemType.ClientID %>').find('input:checked').val();
        var type2 = $('#<%=rblItemType2.ClientID %>').find('input:checked').val();

        var filterExpression = "VisitID = " + visitID + "|" + mataKananTanpaKacamata + "|" + mataKiriTanpaKacamata + "|" + mataKananDenganKacamata + "|" + mataKiriDenganKacamata + "|" + type + "|" + type2;
        var reportCode = "PM-00562";
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
        <input type="hidden" id="hdntype" runat="server" value="" />
        <table>
            <colgroup>
                <col style="width: 150px" />
                <col style="width: 30px" />
                <col style="width: 30px" />
                <col />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("")%></label>
                </td>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Mata Kanan")%></label>
                </td>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("")%></label>
                </td>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Mata Kiri")%></label>
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Tanpa Kacamata :")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtMataKananTanpaKacamata" Width="70px" CssClass="required" runat="server" />
                </td>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtMataKiriTanpaKacamata" Width="70px" CssClass="required" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Dengan Kacamata :")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtMataKananDenganKacamata" Width="70px" CssClass="required" runat="server" />
                </td>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtMataKiriDenganKacamata" Width="70px" CssClass="required" runat="server" />
                </td>
            </tr>
            <tr id="trType" runat="server">
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Buta Warna :")%></label>
                </td>
                <td>
                    <asp:RadioButtonList ID="rblItemType" runat="server" RepeatDirection="Horizontal" CssClass="required">
                        <asp:ListItem Text="Tidak" Value="0" Selected="True" />
                        <asp:ListItem Text="Ya" Value="1" />
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr id="trType2" runat="server">
                <td class="tdLabel">
                    <label class="">
                        <%=GetLabel("")%></label>
                </td>
                <td>
                    <asp:RadioButtonList ID="rblItemType2" runat="server" RepeatDirection="Horizontal" CssClass="required">
                        <asp:ListItem Text="Total" Value="0" />
                        <asp:ListItem Text="Parsial" Value="1" />
                    </asp:RadioButtonList>
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
