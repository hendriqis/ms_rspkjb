<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StockInformationPerLocationCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Inventory.Program.StockInformationPerLocationCtl" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_StockInformationPerLocationCtl">
    //#region Location
    function onGetLocationFilterExpression() {
        var filterExpression = "<%:OnGetLocationFilterExpression() %>";
        return filterExpression;
    }

    $('#lblLocation.lblLink').click(function () {
        openSearchDialog('locationroleuser', onGetLocationFilterExpression(), function (value) {
            $('#<%=txtLocationCode.ClientID %>').val(value);
            onTxtLocationCodeChanged(value);
        });
    });

    $('#<%=txtLocationCode.ClientID %>').change(function () {
        onTxtLocationCodeChanged($(this).val());
    });

    function onTxtLocationCodeChanged(value) {
        var filterExpression = onGetLocationFilterExpression() + "LocationCode = '" + value + "'";
        Methods.getObject('GetLocationUserAccessList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnLocationID.ClientID %>').val(result.LocationID);
                $('#<%=txtLocationName.ClientID %>').val(result.LocationName);
            }
            else {
                $('#<%=hdnLocationID.ClientID %>').val('');
                $('#<%=txtLocationCode.ClientID %>').val('');
                $('#<%=txtLocationName.ClientID %>').val('');
            }
            cbpView.PerformCallback('refresh');
        });
    }
    //#endregion
    //#region Paging
    var pageCount = parseInt('<%=PageCount %>');
    $(function () {
        setPaging($("#paging"), pageCount, function (page) {
            cbpView.PerformCallback('changepage|' + page);
        });
    });

    function onCbpViewEndCallback(s) {
        hideLoadingPanel();
        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            if (pageCount > 0)
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
        }
        else
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
    }
    //#endregion
    function onRefreshGrid() {
        $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
        cbpView.PerformCallback('refresh');
    }

    function onTxtSearchViewSearchClick(s) {
        setTimeout(function () {
            s.SetBlur();
            onRefreshGrid();
        }, 0);
    }
</script>
<input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
<div style="height: 500px; overflow-y: auto; overflow-x: hidden">
    <input type="hidden" value="" runat="server" id="hdnVisitID" />
    <table class="tblContentArea">
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 150px" />
                        <col style="width: 120px" />
                        <col style="width: 400px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td>
                            <input type="hidden" value="" runat="server" id="hdnLocationID" />
                            <label class="lblLink" id="lblLocation">
                                <%=GetLabel("Lokasi")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtLocationCode" Width="100%" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtLocationName" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Quick Search")%></label>
                        </td>
                        <td colspan="2">
                            <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView" ID="txtSearchView"
                                Width="100%" Watermark="Search">
                                <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                                <IntellisenseHints>
                                    <qis:QISIntellisenseHint Text="ItemName1" FieldName="ItemName1" />
                                    <qis:QISIntellisenseHint Text="ItemCode" FieldName="ItemCode" />
                                </IntellisenseHints>
                            </qis:QISIntellisenseTextBox>
                        </td>
                    </tr>
                </table>
                <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                    ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlStockInformationPerLocationView" Style="width: 100%;
                                margin-left: auto; margin-top: 20px; margin-right: auto; position: relative;
                                font-size: 0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:BoundField DataField="ItemCode" ItemStyle-HorizontalAlign="Left" HeaderText="Kode Item"
                                            HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="80px" />
                                        <asp:BoundField DataField="ItemName1" ItemStyle-HorizontalAlign="Left" HeaderText="Nama Item"
                                            HeaderStyle-HorizontalAlign="Left" />
                                        <asp:TemplateField HeaderText="Stok" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                            HeaderStyle-Width="90px">
                                            <ItemTemplate>
                                                <%#:Eval("QuantityEND", "{0:N}")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ItemUnit" ItemStyle-HorizontalAlign="Left" HeaderText="Satuan"
                                            HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="90px" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("Tidak ada informasi stok di lokasi ini")%>
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
