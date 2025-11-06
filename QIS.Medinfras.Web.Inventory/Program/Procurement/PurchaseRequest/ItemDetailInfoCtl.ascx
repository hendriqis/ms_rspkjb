<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ItemDetailInfoCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Inventory.Program.ItemDetailInfoCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascript" id="dxss_serviceunithealthcareentryctl">
    $(function () {
        setDatePicker('<%=txtDateFrom.ClientID %>');
        setDatePicker('<%=txtDateTo.ClientID %>');
        $('#<%=txtDateFrom.ClientID %>').datepicker('option', 'maxDate', '0');
        $('#<%=txtDateTo.ClientID %>').datepicker('option', 'maxDate', '0');
    });

    $('#btnEntryPopupCancel').live('click', function () {
        cbpView.PerformCallback('refresh');
    });

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

<table class="tblContentArea">
    <tr>
        <td>
            <table class="tblEntryContent" style="width:100%">
                <colgroup>
                    <col style="width:160px"/>
                    <col/>
                </colgroup>
                <tr>
                    <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Item")%></label></td>
                    <td><asp:TextBox ID="txtItemName" ReadOnly="true" Width="100%" runat="server" /></td>
                </tr>  
                <tr>
                    <td><label><%=GetLabel("Tanggal Transaksi") %></label></td>
                    <td>
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                                <td><asp:TextBox runat="server" CssClass="datepicker" ID="txtDateFrom" Width="110px" /></td>
                                <td>&nbsp;&nbsp</td>
                                <td><asp:TextBox runat="server" CssClass="datepicker" ID="txtDateTo" Width="110px" /></td>
                                <td style="padding-left:10px"><input type="button" id="btnRefresh" value='<%= GetLabel("Refresh")%>' /> </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>

            <div style="position: relative;">
                <dxcp:ASPxCallbackPanel ID="cbpPopupView" runat="server" Width="100%" ClientInstanceName="cbpPopupView"
                    ShowLoadingPanel="false" OnCallback="cbpPopupView_Callback">
                    <ClientSideEvents EndCallback="function(s,e){onCbpPopupViewEndCallback()}" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height:430px; overflow-y: scroll;">
                                <asp:GridView ID="grdPopupView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:BoundField DataField="MovementID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:BoundField DataField="MovementDateInString" HeaderText="Tanggal" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="90px"  />
                                        <asp:TemplateField HeaderText="Keterangan Transaksi">
                                            <ItemTemplate>
                                                <%#:Eval("TransactionDescription")%><br />
                                                <%#:Eval("DetailDesc")%> <%#:Eval("TransactionNo")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="QuantityBEGIN" HeaderText="QTY AWAL" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" HeaderStyle-Width="70px" />
                                        <asp:BoundField DataField="QuantityIN" HeaderText="MASUK" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" HeaderStyle-Width="70px" />
                                        <asp:BoundField DataField="QuantityOUT" HeaderText="KELUAR" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" HeaderStyle-Width="70px" />
                                        <asp:BoundField DataField="QuantityEND" HeaderText="QTY AKHIR" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" HeaderStyle-Width="70px" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("Tidak ada transaksi mutasi persediaan untuk item ini")%>
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