<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FAPartsDetailCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Inventory.Program.FAPartsDetailCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_partsdetailfaitemctl">
    $(function () {
    });

    //#region Paging
    var pageCount = parseInt('<%=PageCount %>');
    var currPage = parseInt('<%=CurrPage %>');
    $(function () {
        setPaging($("#paging"), pageCount, function (page) {
            cbpPopupView.PerformCallback('changepage|' + page);
        }, null, currPage);
    });

    function onCbpPopupViewEndCallback(s) {
        hideLoadingPanel();

        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            if (pageCount > 0)
                $('#<%=grdPopupView.ClientID %> tr:eq(1)').click();

            setPaging($("#paging"), pageCount, function (page) {
                cbpPopupView.PerformCallback('changepage|' + page);
            });
        }
        else
            $('#<%=grdPopupView.ClientID %> tr:eq(1)').click();
    }

    function onCbpPopupProcessEndCallback(s) {
        hideLoadingPanel();
        cbpPopupView.PerformCallback('refresh');
    }
    //#endregion

</script>
<input type="hidden" id="hdnParentFixedAssetID" runat="server" />
<table class="tblContentArea">
    <colgroup>
        <col style="width: 100%" />
    </colgroup>
    <tr>
        <td>
            <table>
                <colgroup>
                    <col style="width: 200px" />
                    <col style="width: 400px" />
                </colgroup>
                <tr>
                    <td class="tdLabel">
                        <label class="lblNormal">
                            <%=GetLabel("Kode Aset & Inventaris Induk")%></label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtParentFixedAssetCode" runat="server" Width="100%" ReadOnly="true" />
                    </td>
                </tr>
                <tr>
                    <td class="tdLabel">
                        <label class="lblNormal">
                            <%=GetLabel("Nama Aset & Inventaris Induk")%></label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtParentFixedAssetName" runat="server" Width="100%" ReadOnly="true" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
            <div style="position: relative;">
                <dxcp:ASPxCallbackPanel ID="cbpPopupView" runat="server" Width="100%" ClientInstanceName="cbpPopupView"
                    ShowLoadingPanel="false" OnCallback="cbpPopupView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpPopupViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                                <asp:GridView ID="grdPopupView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:BoundField DataField="FixedAssetID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:BoundField DataField="FixedAssetCode" HeaderText="Kode Aset & Inventaris" HeaderStyle-Width="150px"
                                            HeaderStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="FixedAssetName" HeaderText="Nama Aset & Inventaris" HeaderStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="ItemCode" HeaderText="Kode Item" HeaderStyle-Width="100px"
                                            HeaderStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="ItemName1" HeaderText="Nama Item" HeaderStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="SerialNumber" HeaderText="Serial No" HeaderStyle-Width="100px"
                                            HeaderStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="Sequence" HeaderText="Sequence" HeaderStyle-Width="50px"
                                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="DisplayOrder" HeaderText="Urutan Tampilan" HeaderStyle-Width="50px"
                                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
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
