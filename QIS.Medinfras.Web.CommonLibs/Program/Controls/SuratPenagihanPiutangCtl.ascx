<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SuratPenagihanPiutangCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.SuratPenagihanPiutangCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_SuratPenagihan">

    $(function () {
        hideLoadingPanel();
    });

    $('#<%=btnPrint.ClientID %>').click(function (evt) {
        if (IsValid(evt, 'fsTrxPopup', 'mpTrxPopup')) {
            cbpSuratPenagihan.PerformCallback('Print');
        }
    });

    function onReprint() {
        var invoiceID = $('#<%=hdnInvoiceID.ClientID %>').val();
        var language = $('#<%=rblLanguage.ClientID %>').find('input:checked').val();
        var type = $('#<%=rblType.ClientID %>').find('input:checked').val();
        var diskon = $('#<%=rblDiskon.ClientID %>').find('input:checked').val();
        var lainlain = $('#<%=txtKurs.ClientID %>').val();

        var reportCodeIndo = "FN-00152";
        var reportCodeIng = "FN-00153";

        var filterExpression = "ARInvoiceID = " + invoiceID + "|" + language + "|" + type + "|" + diskon + "|" + lainlain;
        if (language == '0') {
            if (reportCodeIndo != "") {
                openReportViewer(reportCodeIndo, filterExpression);
            }
        } else {
            if (reportCodeIng != "") {
                openReportViewer(reportCodeIng, filterExpression);
            }
        }
        pcRightPanelContent.Hide();
        cbpSuratPenagihan.PerformCallback('refresh');

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
        <input type="hidden" id="hdnInvoiceID" runat="server" value="" />
        <table>
            <tr id="trType" runat="server">
                <td>
                    <asp:RadioButtonList ID="rblLanguage" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="Bahasa Indonesia" Value="0" Selected="True" />
                        <asp:ListItem Text="Bahasa Inggris" Value="1" />
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr id="tr1" runat="server">
                <td>
                    <asp:RadioButtonList ID="rblType" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="Rawat Jalan" Value="0" Selected="True" />
                        <asp:ListItem Text="Rawat Inap" Value="1" />
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr id="trDiskon" runat="server">
                <td>
                    <asp:RadioButtonList ID="rblDiskon" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="Tanpa Diskon" Value="0" Selected="True" />
                        <asp:ListItem Text="Dengan Diskon" Value="1" />
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Kurs :")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtKurs" Width="200px" CssClass="required" runat="server" />
                </td>
            </tr>
        </table>
        <div class="imgLoadingGrdView" id="containerImgLoadingView">
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
        <dxcp:ASPxCallbackPanel ID="cbpSuratPenagihan" runat="server" Width="100%" ClientInstanceName="cbpSuratPenagihan"
            ShowLoadingPanel="false" OnCallback="cbpSuratPenagihan_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel();}" EndCallback="function(s,e){
                    onReprint();
                    hideLoadingPanel();
                }" />
        </dxcp:ASPxCallbackPanel>
    </fieldset>
</div>
