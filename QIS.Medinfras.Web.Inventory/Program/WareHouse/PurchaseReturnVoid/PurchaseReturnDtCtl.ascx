<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PurchaseReturnDtCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Inventory.Program.PurchaseReturnDtCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascript" id="dxss_serviceunithealthcareentryctl">
    $('#containerPopup .txtCurrency').each(function () {
        $(this).trigger('changeValue');
    });
</script>
<input type="hidden" id="hdnID" runat="server" />
<input type="hidden" id="hdnItemID" runat="server" />
<input type="hidden" id="hdnLocationID" runat="server" />
<input type="hidden" id="hdnDateFrom" runat="server" />
<input type="hidden" id="hdnDateTo" runat="server" />
<input type="hidden" id="hdnPurchaseReturnID" runat="server" />
<input type="hidden" id="hdnVATPercentage" runat="server" />

<div class="pageTitle"><%=GetLabel("Detil Retur Pembelian")%></div>
<div style="max-height: 500px; overflow-y: auto" id="containerPopup">
    <table class="tblContentArea">
            <colgroup>
                <col style="width: 50%" />
                <col style="width: 50%" />
            </colgroup>
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel"><label><%=GetLabel("No. Retur")%></label></td>
                            <td><asp:TextBox ID="txtReturnNo" Width="150px" ReadOnly="true" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><%=GetLabel("Tanggal") %></td>
                            <td><asp:TextBox ID="txtPurchaseReturnDate" Width="120px" ReadOnly="true" CssClass="datepicker" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label><%=GetLabel("Supplier/Penyedia")%></label></td>
                            <td>
                                <input type="hidden" value="" id="hdnSupplierID" runat="server" />
                                <table cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                        <col style="width: 250px" />
                                    </colgroup>
                                    <tr>
                                        <td><asp:TextBox ID="txtSupplierCode" ReadOnly="true" Width="100%" runat="server" /></td>
                                        <td>&nbsp;</td>
                                        <td><asp:TextBox ID="txtSupplierName" ReadOnly="true" Width="100%" runat="server" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label><%=GetLabel("No. BPB")%></label></td>
                            <td><asp:TextBox ID="txtPurchaseReceiveNo" Width="150px" ReadOnly="true" runat="server" /></td>
                        </tr>
                    </table>
                </td>
                <td style="padding: 5px; vertical-align: top">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel"><label><%=GetLabel("Dari Lokasi")%></label></td>
                            <td>
                                <input type="hidden" id="Hidden1" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 120px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td><asp:TextBox ID="txtLocationCode" Width="100%" ReadOnly="true" runat="server" /></td>
                                        <td>&nbsp;</td>
                                        <td><asp:TextBox ID="txtLocationName" Width="100%" runat="server" ReadOnly="true" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label><%=GetLabel("Penggantian")%></label></td>
                            <td><asp:TextBox ID="txtReturnType" Width="100%" ReadOnly="true" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><%=GetLabel("No. Faktur/Kirim") %></td>
                            <td>
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 120px" />
                                        <col style="width: 3px" />
                                        <col style="width: 100px" />
                                        <col style="width: 100px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td><asp:TextBox ID="txtReferenceNo" Width="120px" runat="server" ReadOnly="true" /></td>
                                        <td>&nbsp;</td>
                                        <td class="tdLabel"><%=GetLabel("Tanggal Faktur") %></td>
                                        <td><div style="text-align:center"><asp:TextBox ID="txtReferenceDate" Width="80px" runat="server" ReadOnly="true"  /></div></td>
                                        <td><asp:CheckBox ID="chkPPN" Enabled="false" Width="100%" runat="server" Text="PPN" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td><asp:CheckBox ID="chkIsAutoUpdateStock" Enabled="false" Width="100%" runat="server" Text="Otomatis Mengurangi Stok" /></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                    position: relative; font-size: 0.95em;">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdService grdNormal notAllowSelect"
                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="ItemCode" HeaderText="Kode Item" HeaderStyle-Width="100px" />
                                            <asp:BoundField DataField="ItemName1" HeaderText="Nama Item" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="Quantity" HeaderText="Qty" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="ItemUnit" HeaderText="Satuan" HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="CustomUnitPrice" HeaderText="Harga / Satuan" ItemStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="CustomTotalDiscount" HeaderText="Total Discount" HeaderStyle-Width="100px"
                                                ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N}" HeaderStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="CustomSubTotal" HeaderText="SubTotal" ItemStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="100px" DataFormatString="{0:N}" HeaderStyle-HorizontalAlign="Right" />
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("No Data To Display")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                    <div class="imgLoadingGrdView" id="containerImgLoadingView">
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                    <div class="containerPaging">
                        <div class="wrapperPaging">
                            <div id="paging">
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div id="containerTotalOrder" style="margin-top: 20px;">
                        <fieldset id="fsTotalOrder" style="margin: 0">
                            <table style="width: 100%;">
                                <colgroup>
                                    <col style="width: 50%" />
                                    <col style="width: 40px" />
                                </colgroup>
                                <tr>
                                    <td valign="top">
                                        <table style="width: 100%;">
                                            <colgroup>
                                                <col style="width: 100px" />
                                            </colgroup>
                                            <tr>
                                                <td class="tdLabel" style="width: 120px; vertical-align: top; padding-top: 5px;"><label class="lblNormal"><%=GetLabel("Catatan")%></label></td>
                                                <td><asp:TextBox ID="txtNotes" Width="100%" runat="server" TextMode="MultiLine" Rows="3" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td valign="top">
                                        <table style="width: 100%;">
                                            <colgroup>
                                                <col style="width: 180px" />
                                            </colgroup>
                                            <tr>
                                                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Jumlah Nilai Retur")%></label></td>
                                                <td><asp:TextBox ID="txtTotalOrder" CssClass="txtCurrency" ReadOnly="true" Width="180px" runat="server" /></td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("PPN")%> (<%=GetVATPercentageLabel()%>%)</label></td>
                                                <td><asp:TextBox ID="txtPPN" CssClass="txtCurrency" ReadOnly="true" Width="180px" runat="server" /></td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Total Nilai Retur")%></label></td>
                                                <td><asp:TextBox ID="txtTotalOrderSaldo" CssClass="txtCurrency" ReadOnly="true" Width="180px" runat="server" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</div>