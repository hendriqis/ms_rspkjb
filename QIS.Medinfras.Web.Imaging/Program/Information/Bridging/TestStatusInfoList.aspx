<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx2.master"
    AutoEventWireup="true" CodeBehind="TestStatusInfoList.aspx.cs" Inherits="QIS.Medinfras.Web.Imaging.Program.RISTestStatusInfoList" %>

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
        $(function () {

            $('#<%=btnRefresh.ClientID %>').click(function () {
                if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
                    cbpView.PerformCallback('refresh');
                }
            });

//            function onCboBinLocationEndCallBackChanged() {
//                cbpView.PerformCallback('refresh');
//            }

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
//                    cbpView.PerformCallback('refresh');
                });
            }
            //#endregion

            //#region Item Group
            function onGetItemGroupFilterExpression() {
                var filterExpression = "<%:OnGetItemGroupFilterExpression() %>";
                if (cboItemType.GetValue() != "") {
                    filterExpression = "GCItemType = '" + cboItemType.GetValue() + "'";
                }
                return filterExpression;
            }

            $('#lblItemGroup.lblLink').click(function () {
                openSearchDialog('itemgroup', onGetItemGroupFilterExpression(), function (value) {
                    $('#<%=txtItemGroupCode.ClientID %>').val(value);
                    onTxtItemGroupCodeChanged(value);
                });
            });

            $('#<%=txtItemGroupCode.ClientID %>').change(function () {
                onTxtItemGroupCodeChanged($(this).val());
            });

            function onTxtItemGroupCodeChanged(value) {
                var filterExpression = onGetItemGroupFilterExpression() + " AND ItemGroupCode = '" + value + "'";
                Methods.getObject('GetItemGroupMasterList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnItemGroupID.ClientID %>').val(result.ItemGroupID);
                        $('#<%=txtItemGroupName.ClientID %>').val(result.ItemGroupName1);
                    }
                    else {
                        $('#<%=hdnItemGroupID.ClientID %>').val('');
                        $('#<%=txtItemGroupCode.ClientID %>').val('');
                        $('#<%=txtItemGroupName.ClientID %>').val('');
                    }
//                    cbpView.PerformCallback('refresh');
                });
            }
            //#endregion
        });

        function onCboItemTypeChanged(s) {
            $('#<%=hdnItemGroupID.ClientID %>').val('');
            $('#<%=txtItemGroupCode.ClientID %>').val('');
            $('#<%=txtItemGroupName.ClientID %>').val('');
//            cbpView.PerformCallback('refresh');
        }

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

        $('.lblPurchaseRequestQty.lblLink').live('click', function () {
            $tr = $(this).closest('tr').closest('tr');
            var locationID = $('#<%=hdnLocationID.ClientID %>').val();
            var itemID = $tr.find('.hiddenColumn').html();
            var id = itemID + '|' + locationID;
            var url = ResolveUrl("~/Program/Information/QtyDetailInfo/ItemPurchaseRequestQtyDtCtl.ascx");
            openUserControlPopup(url, id, 'On Purchase Request - Detail', 800, 500);
        });

        $('.lblPurchaseOrderQtyOnOrder.lblLink').live('click', function () {
            $tr = $(this).closest('tr').closest('tr');
            var locationID = $('#<%=hdnLocationID.ClientID %>').val();
            var itemID = $tr.find('.hiddenColumn').html();
            var id = itemID + '|' + locationID;
            var url = ResolveUrl("~/Program/Information/QtyDetailInfo/PurchaseOrderQtyDtCtl.ascx");
            openUserControlPopup(url, id, 'On Order - Detail', 800, 500);
        });

        $('.lblItemRequestQtyOnOrder.lblLink').live('click', function () {
            $tr = $(this).closest('tr').closest('tr');
            var locationID = $('#<%=hdnLocationID.ClientID %>').val();
            var itemID = $tr.find('.hiddenColumn').html();
            var id = itemID + '|' + locationID;
            var url = ResolveUrl("~/Program/Information/QtyDetailInfo/ItemRequestQtyDtCtl.ascx");
            openUserControlPopup(url, id, 'On Unit Request - Detail', 800, 500);
        });

        $('.lbOnDeliveryQty.lblLink').live('click', function () {
            $tr = $(this).closest('tr').closest('tr');
            var locationID = $('#<%=hdnLocationID.ClientID %>').val();
            var itemID = $tr.find('.hiddenColumn').html();
            var id = itemID + '|' + locationID;
            var url = ResolveUrl("~/Program/Information/QtyDetailInfo/DeliveryQtyDtCtl.ascx");
            openUserControlPopup(url, id, 'On Delivery - Detail', 800, 500);
        });

        $('.lbOnDistributionInQty.lblLink').live('click', function () {
            $tr = $(this).closest('tr').closest('tr');
            var locationID = $('#<%=hdnLocationID.ClientID %>').val();
            var itemID = $tr.find('.hiddenColumn').html();
            var id = itemID + '|' + locationID;
            var url = ResolveUrl("~/Program/Information/QtyDetailInfo/DistributionInQtyDtCtl.ascx");
            openUserControlPopup(url, id, 'On Need Confirmation - Detail', 800, 500);
        });

        function onTxtSearchViewSearchClick(s) {
            setTimeout(function () {
                s.SetBlur();
                onRefreshGrid();
            }, 0);
        }

//        function onCboBinLocationEndCallBackChanged() {
//            cbpView.PerformCallback('refresh');
//        }
    </script>
    <div>
        <input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
        <input type="hidden" id="hdnLocationID" runat="server" />
        <input type="hidden" value="" id="hdnPageTitle" runat="server" />
        <table class="tblContentArea">
            <colgroup>
                <col style="width: 50%" />
                <col style="width: 50%" />
            </colgroup>
            <tr>
                <td>
                    <table width="100%">
                        <colgroup>
                            <col width="120px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td>
                                <label class="lblNormal lblLink" id="lblLocation">
                                    <%=GetLabel("Lokasi") %></label>
                            </td>
                            <td>
                                <table cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col width="120px" />
                                        <col width="3px" />
                                        <col width="250px" />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtLocationCode" Width="100%" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtLocationName" Width="100%" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
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
                                                <%--<ClientSideEvents ValueChanged="function(s,e) { onRefreshGrid(); }" />--%>
                                            </dxe:ASPxComboBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="vertical-align: top">
                    <table width="100%">
                        <colgroup>
                            <col width="120px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td>
                                <label class="lblNormal lblLink" id="lblItemType">
                                    <%=GetLabel("Jenis Item") %></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboItemType" ClientInstanceName="cboItemType" Width="200px"
                                    runat="server">
                                    <ClientSideEvents ValueChanged="function(s,e) { onCboItemTypeChanged(); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblItemGroup">
                                    <%=GetLabel("Kelompok Item")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnItemGroupID" value="" runat="server" />
                                <table cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col width="120px" />
                                        <col width="3px" />
                                        <col width="250px" />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtItemGroupCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtItemGroupName" ReadOnly="true" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                <label class="lblNormal">
                                    <%=GetLabel("Rak")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox runat="server" ID="cboBinLocation" ClientInstanceName="cboBinLocation"
                                    Width="200px">
                                    <%--<ClientSideEvents ValueChanged="function(s,e){ onCboBinLocationEndCallBackChanged(); }" />--%>
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
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                        ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="ItemID" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn" />
                                            <asp:BoundField DataField="BinLocationName" ItemStyle-HorizontalAlign="Left" HeaderText="KODE RAK"
                                                HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" />
                                            <asp:BoundField DataField="ItemCode" ItemStyle-HorizontalAlign="Left" HeaderText="KODE BARANG"
                                                HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" />
                                            <asp:BoundField DataField="ItemName1" ItemStyle-HorizontalAlign="Left" HeaderText="NAMA BARANG"
                                                HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="ItemUnit" ItemStyle-HorizontalAlign="Left" HeaderText="SATUAN"
                                                HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" />
                                            <asp:BoundField DataField="QuantityMin" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                HeaderText="MIN" DataFormatString="{0:N}" HeaderStyle-Width="90px" />
                                            <asp:BoundField DataField="QuantityMax" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                HeaderText="MAX" DataFormatString="{0:N}" HeaderStyle-Width="90px" />
                                            <asp:TemplateField HeaderText="QTY ON HAND" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="80px">
                                                <ItemTemplate>
                                                    <%#:Eval("QuantityEND", "{0:N}")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ON PURCHASE REQUEST" ItemStyle-HorizontalAlign="Right"
                                                HeaderStyle-HorizontalAlign="Right" HeaderStyle-Width="90px">
                                                <ItemTemplate>
                                                    <label class="lblPurchaseRequestQty lblLink">
                                                        <%#:Eval("PurchaseRequestQtyOnOrder", "{0:N}")%></label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ON PURCHASE ORDER" ItemStyle-HorizontalAlign="Right"
                                                HeaderStyle-HorizontalAlign="Right" HeaderStyle-Width="90px">
                                                <ItemTemplate>
                                                    <label class="lblPurchaseOrderQtyOnOrder lblLink">
                                                        <%#:Eval("PurchaseOrderQtyOnOrder", "{0:N}")%></label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ON UNIT REQUEST" ItemStyle-HorizontalAlign="Right"
                                                HeaderStyle-HorizontalAlign="Right" HeaderStyle-Width="90px">
                                                <ItemTemplate>
                                                    <label class="lblItemRequestQtyOnOrder lblLink">
                                                        <%#:Eval("ItemRequestQtyOnOrder", "{0:N}")%></label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ON DELIVERY" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="90px">
                                                <ItemTemplate>
                                                    <label class="lbOnDeliveryQty lblLink">
                                                        <%#:Eval("ItemOnDeliveryQty", "{0:N}")%></label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="NEED CONFIRMATION" ItemStyle-HorizontalAlign="Right"
                                                HeaderStyle-HorizontalAlign="Right" HeaderStyle-Width="90px">
                                                <ItemTemplate>
                                                    <label class="lbOnDistributionInQty lblLink">
                                                        <%#:Eval("ItemOnDistributionInQty", "{0:N}")%></label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Tidak ada informasi saldo persediaan di lokasi ini")%>
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
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
