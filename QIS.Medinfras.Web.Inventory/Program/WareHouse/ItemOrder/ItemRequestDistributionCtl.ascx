<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ItemRequestDistributionCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Inventory.Program.ItemRequestDistributionCtl" %>
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
        <col style="width: 40%" />
        <col style="width: 60%" />
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
                                        <col style="width: 30%" />
                                        <col style="width: 70%" />
                                    </colgroup>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal">
                                                <%=GetLabel("No. Permintaan")%></label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtItemRequestNo" ReadOnly="true" Width="50%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                                <asp:GridView ID="grdViewIRDT" runat="server" CssClass="grdSelected grdIRDT" AutoGenerateColumns="false"
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
                                        <asp:BoundField DataField="Quantity" HeaderText="Qty Minta" HeaderStyle-Width="80px"
                                            ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="ItemUnit" HeaderText="Satuan Minta" HeaderStyle-Width="100px"
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
                <dxcp:ASPxCallbackPanel ID="cbpViewDistributionDt" runat="server" Width="100%" ClientInstanceName="cbpViewDistributionDt"
                    ShowLoadingPanel="false" OnCallback="cbpViewDistributionDt_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt').show(); }"
                        EndCallback="function(s,e){ oncbpViewDistributionDtEndCallback(s); }" />
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
                                <asp:GridView ID="grdViewDistributionDt" runat="server" CssClass="grdSelected grdDistributionDt" AutoGenerateColumns="false"
                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:BoundField DataField="DistributionNo" HeaderText="No. Distribusi" HeaderStyle-Width="200px"
                                            ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="cfDeliveryDateInString" HeaderText="Tanggal Distribusi" HeaderStyle-Width="120px"
                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="Quantity" HeaderText="Qty Distribusi" HeaderStyle-Width="80px"
                                            ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="ItemUnit" HeaderText="Satuan Distribusi" HeaderStyle-Width="100px"
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
        $('#<%=grdViewIRDT.ClientID %> > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
            if ($(this).attr('class') != 'selected') {
                $('#<%=grdViewIRDT.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnItemID.ClientID %>').val($(this).find('.ItemID').val());
                $('#<%=hdnItemName.ClientID %>').val($(this).find('.ItemName1').val());
                cbpViewDistributionDt.PerformCallback('refresh');
            }
        });

        $('#<%=grdViewDistributionDt.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
            if ($(this).attr('class') != 'selected') {
                $('#<%=grdViewDistributionDt.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
            }
        });

        $('#<%=grdViewIRDT.ClientID %> tr:eq(1)').click();
    });

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
                $('#<%=grdViewIRDT.ClientID %> tr:eq(1)').click();

            setPaging($("#paging"), pageCount, function (page) {
                cbpViewIRDT.PerformCallback('changepage|' + page);
            });
        }
        else
            $('#<%=grdViewIRDT.ClientID %> tr:eq(1)').click();
    }
    //#endregion

    //#region Paging Dt
    function oncbpViewDistributionDtEndCallback(s) {
        $('#containerImgLoadingViewDt').hide();
        $('#<%=txtItemName.ClientID %>').val($('#<%=hdnItemName.ClientID %>').val());
        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {

            var pageCount = parseInt(param[1]);
            if (pageCount > 0)
                $('#<%=grdViewDistributionDt.ClientID %> tr:eq(1)').click();

            setPaging($("#pagingDt"), pageCount, function (page) {
                cbpViewDistributionDt.PerformCallback('changepage|' + page);
            });
        }
        else
            $('#<%=grdViewDistributionDt.ClientID %> tr:eq(1)').click();
    }
    //#endregion
</script>
