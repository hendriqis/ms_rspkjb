<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LocationItemExpiredDateInfoCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Inventory.Program.LocationItemExpiredDateInfoCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_LocationItemExpiredDateInfoCtl">
    $('#btnRefresh').live('click', function () {
        hideLoadingPanel();
        cbpPopupView2.PerformCallback('refresh');
    });

    function oncbpPopupView2EndCallback(s) {
        hideLoadingPanel();
    }

    //#region tab
    $(function () {
        $('#ulTabLabResult li').click(function () {
            $('#ulTabLabResult li.selected').removeAttr('class');
            $('.containerInfo').filter(':visible').hide();
            $contentID = $(this).attr('contentid');
            $('#' + $contentID).show();
            $(this).addClass('selected');
            onRefreshGrid();
        });
    });
    //#endregion
</script>
<input type="hidden" runat="server" id="hdnID" value="" />
<input type="hidden" runat="server" id="hdnLocationID" value="" />
<input type="hidden" runat="server" id="hdnItemID" value="" />
<div style="position: relative;">
    <div class="containerUlTabPage" style="margin-bottom: 3px;">
        <ul class="ulTabPage" id="ulTabLabResult">
            <li contentid="container1" class="selected">
                <%=GetLabel("Informasi Expired Balance")%></li>
            <li contentid="container2" runat="server">
                <%=GetLabel("Informasi Expired Penerimaan")%></li>
        </ul>
    </div>
    <div id="container1" class="containerInfo">
        <dxcp:ASPxCallbackPanel ID="cbpPopupView" runat="server" Width="100%" ClientInstanceName="cbpPopupView"
            ShowLoadingPanel="false" OnCallback="cbpPopupView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                            ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:BoundField DataField="BatchNumber" HeaderStyle-CssClass="batchNumber" ItemStyle-CssClass="batchNumber"
                                    HeaderText="Batch Number" HeaderStyle-Width="100px" />
                                <asp:BoundField DataField="ExpiredDateInString" HeaderText="Expired Date" HeaderStyle-Width="100px" />
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("No Data To Display")%>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </asp:Panel>
                </dx:PanelContent>
            </PanelCollection>
        </dxcp:ASPxCallbackPanel>
    </div>
    <div id="container2" class="containerInfo" style="display: none" overflow-y: auto">
        <table class="tblEntryContent" style="width: 100%">
            <colgroup>
                <col style="width: 140px" />
                <col style="width: 80px" />
                <col />
            </colgroup>
            <tr>
                <td>
                    <label class="lblNormal">
                        <%=GetLabel("Filter (x POR Terakhir)")%></label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtLastPOR" CssClass="number" Width="80px" Text="6" Style="text-align:center"/>
                </td>
                <td>
                    <input type="button" id="btnRefresh" value="Refresh" class="btnRefresh w3-button w3-blue w3-border w3-border-blue w3-round-medium w3-ripple" />
                </td>
            </tr>
        </table>
        <dxcp:ASPxCallbackPanel ID="cbpPopupView2" runat="server" Width="100%" ClientInstanceName="cbpPopupView2"
            ShowLoadingPanel="false" OnCallback="cbpPopupView2_Callback">
            <ClientSideEvents EndCallback="function(s,e){ oncbpPopupView2EndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent2" runat="server">
                    <asp:Panel runat="server" ID="pnlViewDt" CssClass="pnlContainerGrid">
                        <asp:GridView ID="grdView2" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                            ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:BoundField DataField="PurchaseReceiveNo" HeaderStyle-CssClass="PurchaseReceiveNo"
                                    HeaderText="No. Penerimaan" HeaderStyle-Width="80px" />
                                <asp:BoundField DataField="cfQty2" HeaderText="Qty" HeaderStyle-Width="20px" HeaderStyle-HorizontalAlign="Right"
                                    ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField DataField="ItemUnit" HeaderText="Satuan" HeaderStyle-Width="30px"
                                    HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField DataField="BatchNumber" HeaderText="Batch Number" HeaderStyle-Width="80px" />
                                <asp:BoundField DataField="cfExpiredDateInSring" HeaderText="Expired Date" HeaderStyle-Width="80px" />
                                <asp:BoundField DataField="ItemDetailStatus" HeaderText="Status" HeaderStyle-Width="40px"/>
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("No Data To Display")%>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </asp:Panel>
                </dx:PanelContent>
            </PanelCollection>
        </dxcp:ASPxCallbackPanel>
    </div>
    <div class="imgLoadingGrdView" id="containerImgLoadingView">
        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
    </div>
</div>
