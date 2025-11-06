<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OutstandingItemRequestCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Inventory.Program.Information.OutstandingItemRequestCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<div style="height: 440px; overflow-y: auto">
    <input type="hidden" value="0" runat="server" id="hdnItemID" />
    <input type="hidden" value="0" runat="server" id="hdnLocationID" />    
    <table class="tableContentArea" style="width:100%">
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <asp:GridView ID="grdView" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true"
                     CssClass="grdView notAllowSelect" EmptyDataRowStyle-CssClass="trEmpty">
                    <Columns>
                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                        <asp:BoundField DataField="ItemRequestNo" HeaderText="No. Permintaan" HeaderStyle-Width="150px"
                            HeaderStyle-CssClass="gridColumnText" ItemStyle-CssClass="gridColumnText" />
                        <asp:BoundField DataField="TransactionDateInString" HeaderText="Tanggal Permintaan" HeaderStyle-Width="100px"
                            HeaderStyle-CssClass="gridColumnText" ItemStyle-CssClass="gridColumnText" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="ItemName1" HeaderText="Nama Item"
                            HeaderStyle-CssClass="gridColumnText" ItemStyle-CssClass="gridColumnText" />
                        <asp:TemplateField ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Right">
                            <HeaderTemplate>
                                <%=GetLabel("Diminta") %>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <table cellpadding="0" cellspacing="0">
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
                        <asp:TemplateField ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Right">
                            <HeaderTemplate>
                                <%=GetLabel("Dikirim") %>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="width: 55px" align="right">
                                            <%#:Eval("TotalDistributionQty") %>
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
                        <asp:BoundField DataField="CreatedByUserName" HeaderText="Diminta Oleh" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Center"
                            HeaderStyle-CssClass="gridColumnText" ItemStyle-CssClass="gridColumnText" ItemStyle-HorizontalAlign = "Center" />
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
