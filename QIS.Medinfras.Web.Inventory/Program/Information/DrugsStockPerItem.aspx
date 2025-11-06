<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx2.master"
    AutoEventWireup="true" CodeBehind="DrugsStockPerItem.aspx.cs" Inherits="QIS.Medinfras.Web.Inventory.Program.DrugsStockPerItem" %>

<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnRefresh" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbrefresh.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Refresh")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $('#<%=btnRefresh.ClientID %>').live('click', function () {
            onRefreshGrid()
        });

        $('.lblExpiredDate.lblLink').live('click', function () {
            showLoadingPanel();
            var url = ResolveUrl('~/Program/Information/LocationItemExpiredDateInfoCtl.ascx');

            $row = $(this).closest('tr');
            var ID = $row.find('.ID').val();
            var locationID = $row.find('.LocationID').val();
            var itemID = $row.find('.ItemID').val();
            var id = ID + '|' + locationID + '|' + itemID;
            openUserControlPopup(url, id, 'Expired Date', 800, 500);
        });

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        $(function () {
            setPagingDetailItem(pageCount);
        });

        function setPagingDetailItem(pageCount) {
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            }, 8);
        }

        function onCbpViewEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                setPagingDetailItem(pageCount);
            }
            hideLoadingPanel();
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

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var QuickSearch = $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val();
            var Stock = cboStockStatus.GetValue();
            var filter = '';

            if (Stock == 'X224^002') {

                filter = 'AND QuantityEND > 0 ';
            } else if (Stock == 'X224^003') {

                filter = 'AND QuantityEND <= 0 ';
            }

            filterExpression.text = "IsDeleted = 0 " + filter + " AND " + QuickSearch;
            return true;
        }
        
    </script>
    <div>
        <input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
        <input type="hidden" value="" id="hdnFilter" runat="server" />
        <input type="hidden" value="" id="hdnPageTitle" runat="server" />
        <table width="100%">
            <colgroup>
                <col width="120px" />
                <col />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label>
                        <%=GetLabel("Quick Filter")%></label>
                </td>
                <td>
                    <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView" ID="txtSearchView"
                        Width="378px" Watermark="Search">
                        <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                        <IntellisenseHints>
                            <qis:QISIntellisenseHint Text="Nama" FieldName="ItemName1" />
                            <qis:QISIntellisenseHint Text="Kode" FieldName="ItemCode" />
                            <qis:QISIntellisenseHint Text="Nama Generik" FieldName="GenericName" />
                        </IntellisenseHints>
                    </qis:QISIntellisenseTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <label class="lblNormal lblLink" id="Label1">
                        <%=GetLabel("Status Stock") %></label>
                </td>
                <td>
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <dxe:ASPxComboBox ID="cboStockStatus" ClientInstanceName="cboStockStatus" Width="200px"
                                    runat="server">
                                    <ClientSideEvents ValueChanged="function(s,e) { onRefreshGrid(); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                    position: relative; font-size: 0.95em;">
                                    <asp:ListView runat="server" ID="lvwView" OnItemDataBound="lvwView_ItemDataBound">
                                        <EmptyDataTemplate>
                                            <table id="tblView" runat="server" class="grdView notAllowSelect grdItemBalance"
                                                cellspacing="0" rules="all">
                                                <tr>
                                                    <th style="width: 10px" align="Left">
                                                    </th>
                                                    <th align="Left">
                                                        <%=GetLabel("Item")%>
                                                    </th>
                                                    <th style="width: 250px" align="Left">
                                                        <%=GetLabel("Location")%>
                                                    </th>
                                                    <th style="width: 100px" align="Center">
                                                        <%=GetLabel("Satuan")%>
                                                    </th>
                                                    <th style="width: 80px" align="Right">
                                                        <%=GetLabel("Min")%>
                                                    </th>
                                                    <th style="width: 80px" align="Right">
                                                        <%=GetLabel("Max")%>
                                                    </th>
                                                    <th style="width: 80px" align="Right">
                                                        <%=GetLabel("On Hand")%>
                                                    </th>
                                                    <th style="width: 80px" align="Right">
                                                        <%=GetLabel("On Purchase Request")%>
                                                    </th>
                                                    <th style="width: 80px" align="Right">
                                                        <%=GetLabel("On Purchase Order")%>
                                                    </th>
                                                    <th style="width: 80px" align="Right">
                                                        <%=GetLabel("On Unit Request")%>
                                                    </th>
                                                    <th style="width: 80px" align="Right">
                                                        <%=GetLabel("On Delivery")%>
                                                    </th>
                                                    <th style="width: 80px" align="Center">
                                                        <%=GetLabel("Expired")%>
                                                    </th>
                                                </tr>
                                                <tr class="trEmpty">
                                                    <td colspan="20">
                                                        <%=GetLabel("No data to display")%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <table id="tblView" runat="server" class="grdView notAllowSelect grdItemBalance"
                                                cellspacing="0" rules="all">
                                                <tr>
                                                    <th style="width: 10px" align="Left">
                                                    </th>
                                                    <th align="Left">
                                                        <%=GetLabel("Item")%>
                                                    </th>
                                                    <th style="width: 250px" align="Left">
                                                        <%=GetLabel("Location")%>
                                                    </th>
                                                    <th style="width: 100px" align="Center">
                                                        <%=GetLabel("Satuan")%>
                                                    </th>
                                                    <th style="width: 80px" align="Right">
                                                        <%=GetLabel("Min")%>
                                                    </th>
                                                    <th style="width: 80px" align="Right">
                                                        <%=GetLabel("Max")%>
                                                    </th>
                                                    <th style="width: 80px" align="Right">
                                                        <%=GetLabel("On Hand")%>
                                                    </th>
                                                    <th style="width: 80px" align="Right">
                                                        <%=GetLabel("On Purchase Request")%>
                                                    </th>
                                                    <th style="width: 80px" align="Right">
                                                        <%=GetLabel("On Purchase Order")%>
                                                    </th>
                                                    <th style="width: 80px" align="Right">
                                                        <%=GetLabel("On Unit Request")%>
                                                    </th>
                                                    <th style="width: 80px" align="Right">
                                                        <%=GetLabel("On Delivery")%>
                                                    </th>
                                                    <th style="width: 80px" align="Center">
                                                        <%=GetLabel("Expired")%>
                                                    </th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td align="center">
                                                    <input type="hidden" class="ID" value="<%#: Eval("ID")%>" />
                                                    <input type="hidden" class="LocationID" value="<%#: Eval("LocationID")%>" />
                                                    <input type="hidden" class="ItemID" value="<%#: Eval("ItemID")%>" />
                                                </td>
                                                <td>
                                                    <label style="font-size: smaller; font-style: italic">
                                                        <%#: Eval("ItemCode")%></label><br />
                                                    <label>
                                                        <%#: Eval("ItemName1")%></label>
                                                </td>
                                                <td>
                                                    <label style="font-size: smaller; font-style: italic">
                                                        <%#: Eval("LocationCode")%></label><br />
                                                    <label>
                                                        <%#: Eval("LocationName")%></label>
                                                </td>
                                                <td align="center">
                                                    <%#: Eval("ItemUnit")%>
                                                </td>
                                                <td align="right">
                                                    <%#: Eval("CustomMinimum2")%>
                                                </td>
                                                <td align="right">
                                                    <%#: Eval("CustomMaximum2")%>
                                                </td>
                                                <td align="right">
                                                    <%#: Eval("CustomEndingBalance2")%>
                                                </td>
                                                <td align="right">
                                                    <%#: Eval("CustomPurchaseRequestQtyOnOrder2")%>
                                                </td>
                                                <td align="right">
                                                    <%#: Eval("CustomQtyOnOrderPurchaseOrder2")%>
                                                </td>
                                                <td align="right">
                                                    <%#: Eval("CustomQtyOnOrderItemRequest")%>
                                                </td>
                                                <td align="right">
                                                    <%#: Eval("CustomItemOnDeliveryQty2")%>
                                                </td>
                                                <td align="center">
                                                    <label id="lblExpiredDate" runat="server" class="lblExpiredDate lblLink">
                                                        <%=GetLabel("Expired Date") %></label>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
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
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
