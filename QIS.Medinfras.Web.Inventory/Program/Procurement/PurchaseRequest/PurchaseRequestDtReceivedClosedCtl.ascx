<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PurchaseRequestDtReceivedClosedCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Inventory.Program.PurchaseRequestDtReceivedClosedCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<input type="hidden" value="" id="hdnPRID" runat="server" />
<input type="hidden" value="" id="hdnItemID" runat="server" />
<input type="hidden" value="" id="hdnItemName" runat="server" />
<input type="hidden" id="hdnFilterExpression" runat="server" value="" />
<table style="width: 100%">
    <colgroup>
        <col style="width: 80%" />
    </colgroup>
    <tr>
        <td valign="top">
            <div style="position: relative;">
                <dxcp:ASPxCallbackPanel ID="cbpViewPODT" runat="server" Width="100%" ClientInstanceName="cbpViewPODT"
                    ShowLoadingPanel="false" OnCallback="cbpViewPODT_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                        EndCallback="function(s,e){ oncbpViewPODTEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage">
                                <table style="width: 100%">
                                    <colgroup>
                                        <col style="width: 10%" />
                                        <col style="width: 100%" />
                                    </colgroup>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal">
                                                <%=GetLabel("No. Permintaan")%></label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPurchaseRequestNo" ReadOnly="true" Width="20%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                                <asp:GridView ID="grdViewPRDT" runat="server" CssClass="grdSelected grdPODT" AutoGenerateColumns="false"
                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:TemplateField HeaderStyle-Width="10px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <input type="hidden" value="<%#:Eval("ItemID") %>" class="ItemID" bindingfield="ItemID" />
                                                <input type="hidden" value="<%#:Eval("ItemName1") %>" class="ItemName1" bindingfield="ItemName1" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                       <asp:BoundField DataField="ItemCode" HeaderText="Kode Item" HeaderStyle-Width="50px"
                                            HeaderStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="ItemName1" HeaderText="Nama Item" HeaderStyle-Width="200px"
                                            ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="CustomPurchaseUnit" HeaderText="Diminta" HeaderStyle-Width="80px"
                                                HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="CustomConversion" HeaderText="Faktor Konversi" HeaderStyle-Width="150px"
                                            ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="CustomPurchaseRequest" HeaderText="Total Diminta" HeaderStyle-Width="80px"
                                            ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="cfLastUpdateInformationDt" HeaderText="Terakhir Diubah" HeaderStyle-Width="200px"
                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
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
            </div>
        </td>
    </tr>
</table>
<script type="text/javascript" id="dxss_infopurchaseorderctl">
        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                cbpViewPODT.PerformCallback('changepage|' + page);
            });
        });

        function oncbpViewPODTEndCallback(s) {
            $('#containerImgLoadingView').hide(); ;
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdViewPRDT.ClientID %> tr:eq(1)').click();

                setPaging($("#paging"), pageCount, function (page) {
                    cbpViewPODT.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewPRDT.ClientID %> tr:eq(1)').click();
        }
        //#endregion
</script>
