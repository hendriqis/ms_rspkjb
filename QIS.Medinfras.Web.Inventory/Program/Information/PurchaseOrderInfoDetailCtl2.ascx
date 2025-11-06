<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PurchaseOrderInfoDetailCtl2.ascx.cs"
    Inherits="QIS.Medinfras.Web.Pharmacy.Program.PurchaseOrderInfoDetailCtl2" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_serviceunithealthcareentryctl">
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

</script>
<input type="hidden" id="hdnID" runat="server" />
<input type="hidden" id="hdnPurchaseReceiveID" runat="server" />
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
            </table>
            <div style="position: relative;">
                <dxcp:ASPxCallbackPanel ID="cbpPopupView" runat="server" Width="100%" ClientInstanceName="cbpPopupView"
                    ShowLoadingPanel="false" OnCallback="cbpPopupView_Callback">
                    <ClientSideEvents EndCallback="function(s,e){onCbpPopupViewEndCallback()}" />
<ClientSideEvents EndCallback="function(s,e){onCbpPopupViewEndCallback()}"></ClientSideEvents>
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height: 240px;
                                overflow-y: scroll;">
                                <asp:GridView ID="grdPopupView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" 
                                            ItemStyle-CssClass="keyField" >
<HeaderStyle CssClass="keyField"></HeaderStyle>

<ItemStyle CssClass="keyField"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PurchaseOrderNo" HeaderText="No. Pemesanan" HeaderStyle-Width="80px"
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
<HeaderStyle HorizontalAlign="Left" Width="80px"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ItemName1" HeaderText="Nama Item" HeaderStyle-Width="150px"
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
<HeaderStyle HorizontalAlign="Left" Width="150px"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                         <asp:BoundField DataField="ReferenceNo" HeaderText="No. Faktur" HeaderStyle-Width="150px"
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
<HeaderStyle HorizontalAlign="Left" Width="150px"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                         <asp:BoundField DataField="TaxInvoiceNo" HeaderText="No. Faktur Pajak" HeaderStyle-Width="150px"
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
<HeaderStyle HorizontalAlign="Left" Width="150px"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                         <asp:BoundField DataField="BatchNoExpiredDate" 
                                            HeaderText="No. Batch | Expire Date" HeaderStyle-Width="150px"
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
<HeaderStyle HorizontalAlign="Left" Width="150px"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Quantity" HeaderText="Qty" HeaderStyle-Width="80px" 
                                            HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
<HeaderStyle HorizontalAlign="Right" Width="80px"></HeaderStyle>

<ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ItemUnit" HeaderText="Satuan" 
                                            HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Left" 
                                            ItemStyle-HorizontalAlign="Left">
<HeaderStyle HorizontalAlign="Left" Width="80px"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
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
