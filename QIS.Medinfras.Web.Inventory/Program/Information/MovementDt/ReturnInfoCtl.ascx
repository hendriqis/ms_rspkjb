<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReturnInfoCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Inventory.Program.ReturnInfoCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascript" id="dxss_ReturnInfoCtl">
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
<input type="hidden" id="hdnItemID" runat="server" />
<input type="hidden" id="hdnLocationID" runat="server" />
<input type="hidden" id="hdnDateFrom" runat="server" />
<input type="hidden" id="hdnDateTo" runat="server" />
<input type="hidden" id="hdnIsChargesIN" runat="server" />

<table class="tblContentArea">
    <tr>
        <td>
            <table class="tblEntryContent" style="width:70%">
                <colgroup>
                    <col style="width:120px"/>
                    <col style="width:120px"/>
                    <col/>
                </colgroup>
                <tr>
                    <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Lokasi")%></label></td>
                    <td colspan="2"><asp:TextBox ID="txtLocation" ReadOnly="true" Width="400px" runat="server" /></td>
                </tr>  
                <tr>
                    <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Item")%></label></td>
                    <td colspan="2"><asp:TextBox ID="txtItemName" ReadOnly="true" Width="400px" runat="server" /></td>
                </tr>  
                <tr>
                    <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Periode")%></label></td>
                    <td><asp:TextBox ID="txtDateFrom" ReadOnly="true" Width="120px" runat="server" style="text-align:center" /></td>
                    <td><asp:TextBox ID="txtDateTo" ReadOnly="true" Width="120px" runat="server" style="text-align:center" /></td>
                </tr>  
            </table>

            <div style="position: relative;">
                <dxcp:ASPxCallbackPanel ID="cbpPopupView" runat="server" Width="100%" ClientInstanceName="cbpPopupView"
                    ShowLoadingPanel="false" OnCallback="cbpPopupView_Callback">
                    <ClientSideEvents EndCallback="function(s,e){onCbpPopupViewEndCallback()}" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height:400px; overflow-y: scroll;">
                                <asp:GridView ID="grdPopupView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:BoundField DataField="MovementID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:BoundField DataField="MovementDateInString" HeaderText="Tanggal Transaksi" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" />
                                        <asp:BoundField DataField="TransactionNo" HeaderText="No Transaksi" HeaderStyle-HorizontalAlign="Left"/>
                                        <asp:BoundField DataField="cfQuantityINInString" HeaderText="Jumlah Masuk" ItemStyle-HorizontalAlign="Right"
                                            HeaderStyle-HorizontalAlign="Right" HeaderStyle-Width="50px" />
                                        <asp:BoundField DataField="cfQuantityOUTInString" HeaderText="Jumlah Keluar" ItemStyle-HorizontalAlign="Right"
                                            HeaderStyle-HorizontalAlign="Right" HeaderStyle-Width="50px" />
                                        <asp:BoundField DataField="ItemUnit" HeaderText="Satuan" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="70px"  />
                                        <asp:BoundField DataField="CreatedByUserName" HeaderText="Dibuat Oleh" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("Tidak ada data retur barang untuk lokasi ini dan tanggal tersebut")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>    
                <div class="imgLoadingGrdView" id="containerImgLoadingView" >
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
                <div class="containerPaging">
                    <div class="wrapperPaging">
                        <div id="pagingPopup"></div>
                    </div>
                </div> 
            </div>
        </td>
    </tr>
</table>