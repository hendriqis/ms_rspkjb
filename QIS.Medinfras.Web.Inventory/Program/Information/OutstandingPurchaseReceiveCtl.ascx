<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OutstandingPurchaseReceiveCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Inventory.Program.Information.OutstandingPurchaseReceiveCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<div style="height: 440px; overflow-y: auto">
    <input type="hidden" value="0" runat="server" id="hdnItemID" />
    <input type="hidden" value="0" runat="server" id="hdnFromLocationID" />
    <input type="hidden" value="0" runat="server" id="hdnStartDate" />
    <input type="hidden" value="0" runat="server" id="hdnEndDate" />
    <table class="tableContentArea" style="width: 100%">
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <asp:GridView ID="grdView" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true"
                    CssClass="grdView notAllowSelect" EmptyDataRowStyle-CssClass="trEmpty">
                    <Columns>
                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                        <asp:BoundField DataField="PurchaseReceiveNo" HeaderText="No. Penerimaan Pembelian" HeaderStyle-Width="155px"
                            HeaderStyle-CssClass="gridColumnText" ItemStyle-CssClass="gridColumnText" />
                        <asp:BoundField DataField="PurchaseOrderNo" HeaderText="No. Pemesanan Barang" HeaderStyle-Width="155px"
                            HeaderStyle-CssClass="gridColumnText" ItemStyle-CssClass="gridColumnText" />
                        <asp:BoundField DataField="LocationName" HeaderText="Lokasi" HeaderStyle-Width="180px"
                            HeaderStyle-CssClass="gridColumnText" ItemStyle-CssClass="gridColumnText" />
                        <asp:BoundField DataField="SupplierCode" HeaderText="Kode Supplier" HeaderStyle-Width="100px"
                            HeaderStyle-CssClass="gridColumnText" ItemStyle-CssClass="gridColumnText" />
                        <asp:BoundField DataField="SupplierName" HeaderText="Nama Supplier" HeaderStyle-Width="180px"
                            HeaderStyle-CssClass="gridColumnText" ItemStyle-CssClass="gridColumnText" />
                        <asp:BoundField DataField="ReceivedDateInString" HeaderText="Tanggal Penerimaan" HeaderStyle-Width="150px"
                            HeaderStyle-CssClass="gridColumnText" ItemStyle-CssClass="gridColumnText" />
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px">
                            <HeaderTemplate>
                                <%=GetLabel("Jumlah") %>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <table cellpadding="0" cellspacing="0" style="width: 100%">
                                    <tr>
                                        <td style="width: 55px" align="right">
                                            <%#:Eval("Quantity") %>
                                        </td>
                                        <td style="width: 5px">
                                            &nbsp;
                                        </td>
                                        <td align="left">
                                            <%#:Eval("ItemUnit") %>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ItemDetailStatus" HeaderText="Status" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" />
                        <asp:BoundField DataField="CreatedByName" HeaderText="Dibuat Oleh" HeaderStyle-HorizontalAlign="Center"
                            HeaderStyle-Width="100px" />
                        <asp:BoundField DataField="LastUpdatedByName" HeaderText="Terakhir Diubah Oleh" HeaderStyle-HorizontalAlign="Center"
                            HeaderStyle-Width="100px" />
                    </Columns>
                    <EmptyDataTemplate>
                        <%=GetLabel("No Data To Display") %>
                    </EmptyDataTemplate>
                </asp:GridView>
            </td>
        </tr>
    </table>
    <div style="width: 100%; text-align: right">
        <input type="button" value='<%= GetLabel("Tutup")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>
