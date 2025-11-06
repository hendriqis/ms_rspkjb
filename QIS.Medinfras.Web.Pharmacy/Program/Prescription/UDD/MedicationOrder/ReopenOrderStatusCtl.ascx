<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReopenOrderStatusCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Pharmacy.Program.ReopenOrderStatusCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<style type="text/css">
    .highlight
    {
        background-color: #FE5D15;
        color: White;
    }
</style>
<script type="text/javascript" id="dxss_ReopenOrderStatusCtl">
    $(function () {
    });

    function onBeforeProcess(param) {
        return true;
    }

    function onCbpViewEndCallback(s) {
        hideLoadingPanel();
    }
</script>
<input type="hidden" id="hdnPrescriptionOrderIDCtl" runat="server" />
<input type="hidden" id="hdnSelectedID" runat="server" />
<div style="height: 440px; overflow-y: auto">
    <table class="tblContentArea">
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table>
                    <colgroup>
                        <col width="150px" />
                        <col width="150px" />
                        <col width="5px" />
                        <col width="150px" />
                        <col width="150px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No. Order")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTransactionNo" runat="server" Width="99%" ReadOnly="true" />
                        </td>
                        <td />
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Tanggal/Jam Order")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtOrderDateTime" runat="server" Width="99%" ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Jenis Resep") %></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPrescripionType" runat="server" Width="99%" ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Status Transaksi") %></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTransactionStatus" runat="server" Width="99%" ReadOnly="true" />
                        </td>
                        <td />
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Status Order") %></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtOrderStatus" runat="server" Width="99%" ReadOnly="true" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
