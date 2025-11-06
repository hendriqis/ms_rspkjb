<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PurchaseReceiveInfoDetailCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Pharmacy.Program.PurchaseReceiveInfoDetailCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_PurchaseReceiveInfoDetailCtl">
    //#region Paging
    var pageCount = parseInt('<%=PageCount %>');
    $(function () {
        setPaging($("#pagingPopup"), pageCount, function (page) {
            cbpPopupView.PerformCallback('changepage|' + page);
        });
    });

    function onCbpPopupViewEndCallback(s) {
        hideLoadingPanel();

        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            setPaging($("#pagingPopup"), pageCount, function (page) {
                cbpPopupView.PerformCallback('changepage|' + page);
            });
        }
    }
    //#endregion

    $('.lblExpiredDate').click(function () {
        $tr = $(this).closest('tr');
        var param = $tr.find('.keyField').html();
        var url = ResolveUrl("~/Program/Information/PurchaseReceiveExpiredDtCtl.ascx");
        openUserControlPopup(url, param, 'Batch and Expired Date Information', 550, 450);
    });
</script>
<input type="hidden" id="hdnID" runat="server" />
<input type="hidden" id="hdnItemID" runat="server" />
<input type="hidden" id="hdnReferenceNo" runat="server" />
<input type="hidden" id="hdnTaxInvoiceNo" runat="server" />
<input type="hidden" id="hdnBatchNoExpiredDate" runat="server" />
<input type="hidden" id="hdnLocationID" runat="server" />
<input type="hidden" id="hdnDateFrom" runat="server" />
<input type="hidden" id="hdnDateTo" runat="server" />
<table class="tblContentArea">
    <tr>
        <td>
            <table class="tblEntryContent" style="width: 70%">
                <colgroup>
                    <col style="width: 120px" />
                    <col />
                </colgroup>
                <tr>
                    <td class="tdLabel">
                        <label class="lblNormal">
                            <%=GetLabel("Item Name")%></label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtItemName" ReadOnly="true" Width="100%" runat="server" />
                    </td>
                </tr>
            </table>
            <div style="position: relative;">
                <dxcp:ASPxCallbackPanel ID="cbpPopupView" runat="server" Width="100%" ClientInstanceName="cbpPopupView"
                    ShowLoadingPanel="false" OnCallback="cbpPopupView_Callback">
                    <ClientSideEvents EndCallback="function(s,e){onCbpPopupViewEndCallback()}" />
                    <ClientSideEvents EndCallback="function(s,e){onCbpPopupViewEndCallback()}"></ClientSideEvents>
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height: 350px;
                                overflow-y: scroll;">
                                <asp:GridView ID="grdPopupView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField">
                                            <HeaderStyle CssClass="keyField"></HeaderStyle>
                                            <ItemStyle CssClass="keyField"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="No. Penerimaan" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <div style="font-weight: bold">
                                                    <%#:Eval("PurchaseReceiveNo") %></div>
                                                <div>
                                                    <%#:Eval("SupplierName") %></div>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="PurchaseOrderNo" HeaderText="No. Pemesanan" HeaderStyle-Width="120px"
                                            HeaderStyle-HorizontalAlign="Left">
                                            <HeaderStyle HorizontalAlign="Left" Width="120px"></HeaderStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ReferenceNo" HeaderText="No. Faktur/Kirim" HeaderStyle-Width="90px"
                                            HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" Width="90px"></HeaderStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TaxInvoiceNo" HeaderText="No. Faktur Pajak" HeaderStyle-Width="90px"
                                            HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" Width="90px"></HeaderStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ReceivedDateInString" HeaderStyle-Width="80px" HeaderText="Tanggal"
                                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="center">
                                            <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CustomUnitPrice" HeaderStyle-HorizontalAlign="Right" HeaderText="Harga/Satuan"
                                            ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="100px">
                                            <HeaderStyle HorizontalAlign="Right" Width="100px"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CustomTotalDiscount" HeaderStyle-HorizontalAlign="Right"
                                            HeaderText="Total Discount" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right"
                                            DataFormatString="{0:N}">
                                            <HeaderStyle HorizontalAlign="Right" Width="80px"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CustomSubTotal" HeaderStyle-HorizontalAlign="Right" HeaderText="Sub Total"
                                            ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="100px" DataFormatString="{0:N}">
                                            <HeaderStyle HorizontalAlign="Right" Width="100px"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Right" HeaderStyle-Width="110px" ItemStyle-HorizontalAlign="Right">
                                            <HeaderTemplate>
                                                <%=GetLabel("Jumlah") %>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%#:Eval("cfQtyReceiveInfo") %>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Right" Width="110px"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="BatchNoExpiredDate" HeaderStyle-Width="100px" HeaderText="No. Batch | Expired"
                                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="center">
                                            <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="70px"
                                            ItemStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                <%=GetLabel("Dibuat Oleh") %>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div>
                                                    <%#:Eval("CreatedByName") %></div>
                                                <div>
                                                    <%#:Eval("cfCreatedDate") %></div>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="75px"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataRowStyle CssClass="trEmpty"></EmptyDataRowStyle>
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
                        <div id="pagingPopup">
                        </div>
                    </div>
                </div>
            </div>
        </td>
    </tr>
</table>
