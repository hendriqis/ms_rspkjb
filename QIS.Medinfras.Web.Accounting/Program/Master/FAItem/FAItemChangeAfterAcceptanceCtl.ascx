<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FAItemChangeAfterAcceptanceCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Accounting.Program.FAItemChangeAfterAcceptanceCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_FAItemChangeAfterAcceptanceCtl">
    $(function () {
        hideLoadingPanel();
    });

</script>
<div style="padding: 10px;">
    <fieldset id="fsTrxPopup" style="margin: 0">
        <input type="hidden" id="hdnFixedAssetIDCtl" runat="server" value="" />
        <table>
            <colgroup>
                <col style="width: 180px" />
                <col style="width: 500px" />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Kode Aset & Inventaris")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtFixedAssetCodeCtl" Width="100%" runat="server" ReadOnly="true" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Nama Aset & Inventaris")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtFixedAssetNameCtl" Width="100%" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Nomor Seri")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtSerialNumberCtl" Width="100%" runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <hr style="padding: 0 0 0 0; margin: 0 0 0 0;" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Nomor Anggaran")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtBudgetPlanNoCtl" Width="100%" runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <hr style="padding: 0 0 0 0; margin: 0 0 0 0;" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                    <label class="lblNormal">
                        <%=GetLabel("Keterangan")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtRemarksCtl" Width="100%" runat="server" TextMode="MultiLine"
                        Rows="2" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <hr style="padding: 0 0 0 0; margin: 0 0 0 0;" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("No. Permintaan Pembelian")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtPurchaseRequestNumberCtl" Width="220px" runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <hr style="padding: 0 0 0 0; margin: 0 0 0 0;" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("No. Berita Acara")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtFAAcceptanceNoCtl" Width="220px" runat="server" ReadOnly="true" />
                </td>
            </tr>
        </table>
        <div class="imgLoadingGrdView" id="containerImgLoadingView">
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
    </fieldset>
</div>
