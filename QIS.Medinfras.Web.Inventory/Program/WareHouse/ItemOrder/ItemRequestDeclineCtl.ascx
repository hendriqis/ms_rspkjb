<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ItemRequestDeclineCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Inventory.Program.ItemRequestDeclineCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<input type="hidden" value="" id="hdnIRID" runat="server" />
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
                <dxcp:ASPxCallbackPanel ID="cbpViewIRDT" runat="server" Width="100%" ClientInstanceName="cbpViewIRDT"
                    ShowLoadingPanel="false" OnCallback="cbpViewIRDT_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                        EndCallback="function(s,e){ oncbpViewIRDTEndCallback(s); }" />
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
                                            <asp:TextBox ID="txtItemRequestNo" ReadOnly="true" Width="20%" runat="server" />
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
                                        <asp:BoundField DataField="CustomItemUnit" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
                                                HeaderText="Diminta" HeaderStyle-Width="120px" />
                                        <asp:BoundField DataField="BaseUnit" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            HeaderText="Satuan Dasar" HeaderStyle-Width="100px" />
                                        <asp:BoundField DataField="CustomConversion" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            HeaderText="Konversi" HeaderStyle-Width="120px" />
                                        <asp:BoundField DataField="CustomItemRequest" HeaderStyle-HorizontalAlign="Right"
                                            ItemStyle-HorizontalAlign="Right" HeaderText="Total Diminta" HeaderStyle-Width="120px" />
                                        <asp:BoundField DataField="cfLastUpdateInfo" HeaderText="Terakhir Diubah" HeaderStyle-Width="120px"
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
                cbpViewIRDT.PerformCallback('changepage|' + page);
            });
        });

        function oncbpViewIRDTEndCallback(s) {
            $('#containerImgLoadingView').hide(); ;
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdViewPRDT.ClientID %> tr:eq(1)').click();

                setPaging($("#paging"), pageCount, function (page) {
                    cbpViewIRDT.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewPRDT.ClientID %> tr:eq(1)').click();
        }
        //#endregion
</script>
