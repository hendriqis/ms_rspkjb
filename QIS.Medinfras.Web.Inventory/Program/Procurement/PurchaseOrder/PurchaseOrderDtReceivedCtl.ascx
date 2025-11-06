<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PurchaseOrderDtReceivedCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Inventory.Program.PurchaseOrderDtReceivedCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<input type="hidden" value="" id="hdnPOID" runat="server" />
<input type="hidden" value="" id="hdnItemID" runat="server" />
<input type="hidden" value="" id="hdnItemName" runat="server" />
<input type="hidden" id="hdnFilterExpression" runat="server" value="" />
<table style="width: 100%">
    <colgroup>
        <col style="width: 40%" />
        <col style="width: 60%" />
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
                                        <col style="width: 30%" />
                                        <col style="width: 70%" />
                                    </colgroup>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal">
                                                <%=GetLabel("No. Pemesanan")%></label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPurchaseOrderNo" ReadOnly="true" Width="50%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                                <asp:GridView ID="grdViewPODT" runat="server" CssClass="grdSelected grdPODT" AutoGenerateColumns="false"
                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:TemplateField HeaderStyle-Width="10px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <input type="hidden" value="<%#:Eval("ItemID") %>" class="ItemID" bindingfield="ItemID" />
                                                <input type="hidden" value="<%#:Eval("ItemName1") %>" class="ItemName1" bindingfield="ItemName1" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ItemName1" HeaderText="Item" HeaderStyle-Width="200px"
                                            ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="Quantity" HeaderText="Qty Pesan" HeaderStyle-Width="80px"
                                            ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="PurchaseUnit" HeaderText="Satuan Pesan" HeaderStyle-Width="100px"
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
        <td valign="top">
            <div style="position: relative;">
                <dxcp:ASPxCallbackPanel ID="cbpViewPRDT" runat="server" Width="100%" ClientInstanceName="cbpViewPRDT"
                    ShowLoadingPanel="false" OnCallback="cbpViewPRDT_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt').show(); }"
                        EndCallback="function(s,e){ oncbpViewPRDTEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent2" runat="server">
                            <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGridPatientPage">
                                <table style="width: 100%">
                                    <colgroup>
                                        <col style="width: 10%" />
                                        <col style="width: 90%" />
                                    </colgroup>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal">
                                                <%=GetLabel("Item")%></label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtItemName" ReadOnly="true" Width="80%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                                <asp:GridView ID="grdViewPRDT" runat="server" CssClass="grdSelected grdPRDT" AutoGenerateColumns="false"
                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:BoundField DataField="PurchaseReceiveNo" HeaderText="No. BPB" HeaderStyle-Width="200px"
                                            ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="ReceivedDateInString" HeaderText="Tanggal Terima" HeaderStyle-Width="120px"
                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="Quantity" HeaderText="Qty Terima" HeaderStyle-Width="80px"
                                            ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="ItemUnit" HeaderText="Satuan Terima" HeaderStyle-Width="100px"
                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("No Data To Display") %>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div class="imgLoadingGrdView" id="containerImgLoadingViewDt">
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
                <div class="containerPaging">
                    <div class="wrapperPaging">
                        <div id="pagingDt">
                        </div>
                    </div>
                </div>
            </div>
        </td>
    </tr>
</table>
<script type="text/javascript" id="dxss_infopurchaseorderctl">
    $(function () {
        $('#<%=grdViewPODT.ClientID %> > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
            if ($(this).attr('class') != 'selected') {
                $('#<%=grdViewPODT.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnItemID.ClientID %>').val($(this).find('.ItemID').val());
                $('#<%=hdnItemName.ClientID %>').val($(this).find('.ItemName1').val());
                cbpViewPRDT.PerformCallback('refresh');
            }
        });

        $('#<%=grdViewPRDT.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
            if ($(this).attr('class') != 'selected') {
                $('#<%=grdViewPRDT.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
            }
        });

        $('#<%=grdViewPODT.ClientID %> tr:eq(1)').click();
    });

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
                $('#<%=grdViewPODT.ClientID %> tr:eq(1)').click();

            setPaging($("#paging"), pageCount, function (page) {
                cbpViewPODT.PerformCallback('changepage|' + page);
            });
        }
        else
            $('#<%=grdViewPODT.ClientID %> tr:eq(1)').click();
    }
    //#endregion

    //#region Paging Dt
    function oncbpViewPRDTEndCallback(s) {
        $('#containerImgLoadingViewDt').hide();
        $('#<%=txtItemName.ClientID %>').val($('#<%=hdnItemName.ClientID %>').val());
        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {

            var pageCount = parseInt(param[1]);
            if (pageCount > 0)
                $('#<%=grdViewPRDT.ClientID %> tr:eq(1)').click();

            setPaging($("#pagingDt"), pageCount, function (page) {
                cbpViewPRDT.PerformCallback('changepage|' + page);
            });
        }
        else
            $('#<%=grdViewPRDT.ClientID %> tr:eq(1)').click();
    }
    //#endregion
</script>
