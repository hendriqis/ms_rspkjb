<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DownloadExcelTelkomPerARInvoiceCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.DownloadExcelTelkomPerARInvoiceCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_DownloadExcelTelkomPerARInvoiceCtl">
    $(function () {
        hideLoadingPanel();
    });

    function downloadTelkomDocument(stringparam) {
        var fileName = $('#<%=hdnFileName.ClientID %>').val();

        var link = document.createElement("a");
        link.href = 'data:text/csv,' + encodeURIComponent(stringparam);
        link.download = fileName;
        link.click();
    }

    function onAfterSaveAddRecordEntryPopup(retval) {
        downloadTelkomDocument(retval);
    }
</script>
<div style="padding: 10px;">
    <fieldset id="fsTrxPopup" style="margin: 0">
        <input type="hidden" id="hdnARInvoiceID" runat="server" value="" />
        <input type="hidden" id="hdnFileName" runat="server" value="" />
        <table>
            <colgroup>
                <col style="width: 30%" />
                <col />
            </colgroup>
            <tr>
                <td>
                    <label class="lblNormal lblLink" id="lblARInvoiceNo">
                        <%=GetLabel("Nomor Invoice")%></label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtARInvoiceNo" Width="150px" ReadOnly="true" />
                </td>
            </tr>
            <tr>
                <td>
                    <label class="lblNormal">
                        <%=GetLabel("Tanggal Invoice")%></label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtARInvoiceDate" Width="150px" CssClass="datepicker"
                        ReadOnly="true" />
                </td>
            </tr>
        </table>
        <div class="imgLoadingGrdView" id="containerImgLoadingView">
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
    </fieldset>
</div>
