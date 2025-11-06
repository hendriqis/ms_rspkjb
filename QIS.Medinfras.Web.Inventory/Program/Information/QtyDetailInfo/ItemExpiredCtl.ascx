<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ItemExpiredCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Inventory.Program.Information.ItemExpiredCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<div style="height: 440px; overflow-y: auto">
    <input type="hidden" value="0" runat="server" id="hdnItemID" />
    <input type="hidden" value="0" runat="server" id="hdnLocationID" />
    <table class="tableContentArea" style="width: 100%">
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <asp:GridView ID="grdView" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true"
                    CssClass="grdView notAllowSelect" EmptyDataRowStyle-CssClass="trEmpty">
                    <Columns>
                        <asp:BoundField DataField="BatchNumber" HeaderText="Batch Number" HeaderStyle-Width="100px"
                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="ExpiredDateInString" HeaderText="Expired Date" HeaderStyle-Width="100px"
                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="Quantity" HeaderText="Quantity" HeaderStyle-Width="100px"
                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                    </Columns>
                    <EmptyDataTemplate>
                        <%=GetLabel("No Data To Display") %>
                    </EmptyDataTemplate>
                </asp:GridView>
            </td>
        </tr>
    </table>
</div>
