<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FAItemWithoutAcceptanceInformationCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Accounting.Program.FAItemWithoutAcceptanceInformationCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_itemstockperlocationctl">
    //#region Paging
    var pageCountAvailable = parseInt('<%=PageCount %>');
    $(function () {
        setPagingDetailItem(pageCountAvailable);
    });

    function setPagingDetailItem(pageCount) {
        if (pageCount > 0)
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        setPaging($("#paging"), pageCount, function (page) {
            cbpViewPopUpCtl.PerformCallback('changepage|' + page);
        }, 8);
    }
    //#endregion 

    function oncbpViewPopUpCtlEndCallback(s) {
        hideLoadingPanel();
        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            if (pageCount > 0)
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
            setPaging($("#paging"), pageCount, function (page) {
                cbpViewPopUpCtl.PerformCallback('changepage|' + page);
            });
        }
        else
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
    }
    //#endregion
</script>
<input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
<div style="height: 440px; overflow-y: auto; overflow-x:hidden;">
    <div class="pageTitle">
        <%=GetLabel("Informasi Aset dan Inventaris yang belum dibuat Berita Acara")%></div>
    <table class="tblContentArea" style="padding-bottom:-0px">
        <tr>
            <td style="vertical-align: top">
                <dxcp:ASPxCallbackPanel ID="cbpViewPopUpCtl" runat="server" Width="100%" style="margin-top:-15px" ClientInstanceName="cbpViewPopUpCtl"
                    ShowLoadingPanel="false" OnCallback="cbpViewPopUpCtl_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ oncbpViewPopUpCtlEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlItemStockPerLocationView" Style="width: 100%; margin-left: auto;
                                margin-top: 20px; margin-right: auto; position: relative; font-size: 0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:BoundField DataField="FAGroupName" ItemStyle-HorizontalAlign="Left" HeaderText="Kelompok"
                                            HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="80px" />
                                        <asp:BoundField DataField="FALocationName" ItemStyle-HorizontalAlign="Left" HeaderText="Lokasi Aset & Inventaris"
                                            HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="80px" />
                                        <asp:BoundField DataField="FixedAssetCode" ItemStyle-HorizontalAlign="Center" HeaderText="Kode Aset & Inventaris"
                                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" />
                                        <asp:BoundField DataField="FixedAssetName" ItemStyle-HorizontalAlign="Left" HeaderText="Nama Aset & Inventaris"
                                            HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px" />
                                        <asp:BoundField DataField="cfProcurementDateInString" ItemStyle-HorizontalAlign="Center" HeaderText="Tanggal Perolehan"
                                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("No Data To Display")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div class="imgLoadingGrdView" id="containerImgLoading">
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
