<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PurchaseOrderQtyDtCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.Inventory.Program.PurchaseOrderQtyDtCtl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
    
<script type="text/javascript" id="dxss_purchaserequestaddtopoctl">
    //#region Paging
    var pageCount = parseInt('<%=PageCount %>');
    $(function () {
        setPaging($("#pagingPopup"), pageCount, function (page) {
            cbpEntryPopupView.PerformCallback('changepage|' + page);
        }, null, 1);
    });

    function onCbpEntryPopupViewEndCallback(s) {
        hideLoadingPanel();

        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            if (pageCount > 0)
                $('#<%=grdView.ClientID %> tr:eq(1)').click();

            setPaging($("#pagingPopup"), pageCount, function (page) {
                cbpEntryPopupView.PerformCallback('changepage|' + page);
            });
        }
        else
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
    }
    //#endregion
</script>

<div style="height:440px; overflow-y:auto;overflow-x: hidden">
    <input type="hidden" id="hdnPurchaseOrderID" runat="server" />
    <input type="hidden" id="hdnItemID" runat="server" />
    <input type="hidden" id="hdnLocationID" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width:100%"/>
        </colgroup>
        <tr>            
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:70%">
                    <colgroup>
                        <col style="width:160px"/>
                        <col/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Item")%></label></td>
                        <td colspan="2"><asp:TextBox ID="txtItemName" ReadOnly="true" Width="100%" runat="server" /></td>
                    </tr>  
                </table>

                <dxcp:ASPxCallbackPanel ID="cbpEntryPopupView" runat="server" Width="100%" ClientInstanceName="cbpEntryPopupView"
                    ShowLoadingPanel="false" OnCallback="cbpEntryPopupView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                        EndCallback="function(s,e){ onCbpEntryPopupViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto; margin-right: auto; position: relative;font-size:0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdSelected"
                                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:BoundField DataField="OrderDateInString" HeaderText="Date" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                        <asp:TemplateField HeaderText="Purchase Order No." HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <%#: Eval("PurchaseOrderNo")%>
                                                <br />
                                                <%#: Eval("SupplierName")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Quantity" HeaderText="Order Quantity" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" HeaderStyle-Width="60px" />
                                        <asp:BoundField DataField="ReceivedQuantity" HeaderText="Received Quantity" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" HeaderStyle-Width="60px" />
                                        <asp:TemplateField HeaderText="Balance Quantity" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="60px" >
                                            <ItemTemplate>
                                                <table cellpadding="0" cellspacing="0" style="width:100%">
                                                    <colgroup>
                                                        <col />
                                                    </colgroup>
                                                    <tr>
                                                        <td class="lblReadOnlyText" align="right"><%#: Eval("CustomQtyRemaining")%></td>
                                                    </tr>
                                                </table>  
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="PurchaseUnit" HeaderText="Unit" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="60px"/>
                                        <asp:BoundField DataField="CustomConversion" HeaderText="Conversion Factor" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="120px" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("No Data To Display")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div class="containerPaging">
                    <div class="wrapperPaging">
                        <div id="pagingPopup"></div>
                    </div>
                </div> 
            </td>
        </tr>
    </table>
</div>