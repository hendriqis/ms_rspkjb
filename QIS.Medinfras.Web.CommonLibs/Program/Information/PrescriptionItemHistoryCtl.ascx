<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PrescriptionItemHistoryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PrescriptionItemHistoryCtl" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_itemstockperlocationctl">
    //#region Paging
    var pageCount = parseInt('<%=PageCount %>');
    $(function () {
        setPaging($("#paging"), pageCount, function (page) {
            cbpViewPopUpHistoryCtl.PerformCallback('changepage|' + page);
        });

        $('#<%=txtDuration.ClientID %>').change(function () {
            onRefreshGrid();
        });
    });

    function oncbpViewPopUpHistoryCtlEndCallback(s) {
        hideLoadingPanel();
        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            if (pageCount > 0)
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
            setPaging($("#paging"), pageCount, function (page) {
                cbpViewPopUpHistoryCtl.PerformCallback('changepage|' + page);
            });
        }
        else
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
    }
    //#endregion
    function onRefreshGrid() {
        $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
        cbpViewPopUpHistoryCtl.PerformCallback('refresh');
    }

    function onTxtSearchViewSearchClick(s) {
        setTimeout(function () {
            s.SetBlur();
            onRefreshGrid();
        }, 0);
    }
</script>
<input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
<div style="height: 440px; overflow-y: auto; overflow-x:hidden;">
    <input type="hidden" value="" runat="server" id="hdnVisitID" />
    <table class="tblContentArea">
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 200px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Nama Item")%></label>
                        </td>
                        <td>
                            <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView" ID="txtSearchView"
                                Width="378px" Watermark="Search">
                                <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                                <IntellisenseHints>
                                    <qis:QISIntellisenseHint Text="Nama" FieldName="ItemName1" />
                                    <qis:QISIntellisenseHint Text="Kode" FieldName="ItemCode" />
                                </IntellisenseHints>
                            </qis:QISIntellisenseTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label class="lblNormal">
                                <%=GetLabel("Periode Pencarian") %></label>
                        </td>   
                        <td>
                            <asp:TextBox runat="server" ID="txtDuration" CssClass="number" Width="60px" Text="0" /> <%=GetLabel("hari ")%>
                        </td>                     
                    </tr>
                </table>
                <dxcp:ASPxCallbackPanel ID="cbpViewPopUpHistoryCtl" runat="server" Width="100%" ClientInstanceName="cbpViewPopUpHistoryCtl"
                    ShowLoadingPanel="false" OnCallback="cbpViewPopUpHistoryCtl_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ oncbpViewPopUpHistoryCtlEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlItemStockPerLocationView" Style="width: 100%; margin-left: auto;
                                margin-top: 20px; margin-right: auto; position: relative; font-size: 0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:BoundField DataField="ItemCode" ItemStyle-HorizontalAlign="Left" HeaderText="KODE ITEM"
                                            HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="80px"  />
                                        <asp:BoundField DataField="ItemName1" ItemStyle-HorizontalAlign="Left" HeaderText="NAMA ITEM"
                                            HeaderStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="cfTransactionDate" ItemStyle-HorizontalAlign="Center" HeaderText="TANGGAL"
                                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" />
                                        <asp:BoundField DataField="TransactionNo" ItemStyle-HorizontalAlign="Left" HeaderText="NO. RESEP"
                                            HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px" />
                                        <asp:TemplateField HeaderText="JUMLAH" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                            HeaderStyle-Width="90px">
                                            <ItemTemplate>
                                                <%#:Eval("cfChargeQty")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("Tidak ada informasi penulisan resep obat yang dicari untuk pasien ini")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div class="imgLoadingGrdView" id="containerImgLoadingPatientFamily">
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
                <div class="containerPaging">
                    <div class="wrapperPaging">
                        <div id="paging">
                        </div>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</div>
