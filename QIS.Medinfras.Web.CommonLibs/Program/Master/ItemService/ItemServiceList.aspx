<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPList.master" AutoEventWireup="true"
    CodeBehind="ItemServiceList.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.ItemServiceList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">
        $(function () {
            var grd = new customGridView();
            grd.init('<%=grdView.ClientID %>', '<%=hdnID.ClientID %>', '<%=pnlView.ClientID %>', cbpView, 'paging');

        });

        //#region Item Group
        function onGetItemGroupFilterExpression() {
            var filterExpression = "<%:OnGetItemGroupFilterExpression() %>";
            return filterExpression;
        }

        $('#lblItemGroup.lblLink').live('click', function () {
            openSearchDialog('itemgroup1', onGetItemGroupFilterExpression(), function (value) {
                $('#<%=txtItemGroupCode.ClientID %>').val(value);
                onTxtItemGroupCodeChanged(value);
            });
        });

        $('#<%=txtItemGroupCode.ClientID %>').live('change', function () {
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
                cbpView.PerformCallback('refresh');
            });
        }
        //#endregion

        function onRefreshControl(filterExpression) {
            $('#<%=hdnFilterExpression.ClientID %>').val(filterExpression);
            cbpView.PerformCallback('refresh');
        }

        function onGetEntryPopupReturnValue() {
            return '';
        }

        function onAfterSaveRightPanelContent(code, value) {
            cbpViewDetail.PerformCallback();
        }

        function onGetCurrID() {
            return $('#<%=hdnID.ClientID %>').val();
        }

        function onGetFilterExpression() {
            return $('#<%=hdnFilterExpression.ClientID %>').val();
        }

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        var currPage = parseInt('<%=CurrPage %>');
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            }, null, currPage);
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

        $('#<%=grdView.ClientID %> tr:gt(0) td.tdExpand').live('click', function () {
            $tr = $(this).parent();
            $trDetail = $(this).parent().next();
            if ($trDetail.attr('class') != 'trDetail') {
                $trCollapse = $('.trDetail');

                $(this).find('.imgExpand').attr('src', '<%= ResolveUrl("~/Libs/Images/down-arrow.png")%>');

                var a = $('#containerGrdDetail').html();
                $newTr = $("<tr><td></td><td colspan='8'>" + a + "</td></tr>").attr('class', 'trDetail');
                $newTr.insertAfter($tr);

                if ($trCollapse != null) {
                    $trCollapse.prev().find('.imgExpand').attr('src', '<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>');
                    $trCollapse.remove();
                }

                $('#<%=hdnExpandID.ClientID %>').val($tr.find('.keyField').html());
            }
            else {
                $(this).find('.imgExpand').attr('src', '<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>');

                $trDetail.remove();
            }
        });

        $('.lnkEdit').live('click', function () {
            var itemCostID = $(this).closest('tr').find('.keyField').html();
            var url = ResolveUrl("~/Libs/Program/Master/ItemService/ItemServiceItemCostEntryCtl.ascx");
            openUserControlPopup(url, itemCostID, 'Item Cost', 600, 400);
        });

        $('.lnkItem a').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html() + '|' + $('#<%=hdnModuleID.ClientID %>').val();
            var url = ResolveUrl("~/Libs/Program/Master/ItemService/ItemServiceDtEntryCtl.ascx");
            showLoadingPanel();
            openUserControlPopup(url, id, 'Item Detail Package', 900, 500);
        });

        $('.lnkProcedure a').live('click', function () {
            var ItemID = $(this).closest('tr').find('.keyField').html();
            openMatrixControl('ProcedureItemService', ItemID, 'Tindakan');
        });

        //#region Link GLRevenue

        $('.lnkItemMasterCOA').live('click', function () {
            var id = $('#<%=hdnExpandID.ClientID %>').val();
            var url = ResolveUrl("~/Libs/Program/Master/ItemService/GLRevenue/GLMappingItemMasterEntryCtl.ascx");
            openUserControlPopup(url, id, 'Item', 1200, 500);
        });

        $('.lnkItemMasterServiceUnitCOA').live('click', function () {
            var id = $('#<%=hdnExpandID.ClientID %>').val();
            var url = ResolveUrl("~/Libs/Program/Master/ItemService/GLRevenue/GLMappingItemMasterServiceUnitEntryCtl.ascx");
            openUserControlPopup(url, id, 'Item & ServiceUnit', 1200, 500);
        });

        $('.lnkItemMasterClassCOA').live('click', function () {
            var id = $('#<%=hdnExpandID.ClientID %>').val();
            var url = ResolveUrl("~/Libs/Program/Master/ItemService/GLRevenue/GLMappingItemMasterClassEntryCtl.ascx");
            openUserControlPopup(url, id, 'Item & Class', 1200, 500);
        });

        //#endregion

    </script>
    <input type="hidden" id="hdnID" runat="server" value="" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <input type="hidden" id="hdnExpandID" runat="server" value="" />
    <input type="hidden" id="hdnModuleID" runat="server" value="" />
    <input type="hidden" value="" id="hdnSubMenuType" runat="server" />
    <input type="hidden" value="" id="hdnGCSubItemType" runat="server" />
    <table cellpadding="0" cellspacing="0">
        <tr>
            <td style="width: 130px">
                <label class="lblLink" id="lblItemGroup">
                    <%=GetLabel("Kelompok Item") %></label>
            </td>
            <td>
                <input type="hidden" value="" id="hdnItemGroupID" runat="server" />
                <table style="width: 100%" cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col style="width: 30%" />
                        <col style="width: 3px" />
                        <col />
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
    </table>
    <div style="position: relative;">
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ $('#tempContainerGrdDetail').append($('#containerGrdDetail')); showLoadingPanel(); }"
                EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="overflow-y: scroll;">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                            ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                            <Columns>
                                <asp:BoundField DataField="ItemID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:TemplateField ItemStyle-Width="20px" ItemStyle-CssClass="tdExpand" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <img class="imgExpand imgLink" src='<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>'
                                            alt='' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="ItemCode" HeaderText="Kode Item" HeaderStyle-CssClass="gridColumnText"
                                    ItemStyle-CssClass="gridColumnText" HeaderStyle-Width="100px" />
                                <asp:BoundField DataField="OldItemCode" HeaderText="Kode Referensi" HeaderStyle-CssClass="gridColumnText"
                                    ItemStyle-CssClass="gridColumnText" HeaderStyle-Width="100px" />
                                <asp:BoundField DataField="ItemName1" HeaderText="Nama Item" HeaderStyle-CssClass="gridColumnText"
                                    ItemStyle-CssClass="gridColumnText" />
                                <asp:BoundField DataField="ItemName2" HeaderText="Item Name 2" Visible="false" HeaderStyle-CssClass="gridColumnText"
                                    ItemStyle-CssClass="gridColumnText" />
                                <asp:TemplateField ItemStyle-CssClass="gridColumnLink lnkItem" HeaderText="Paket"
                                    HeaderStyle-Width="80px">
                                    <ItemTemplate>
                                        <a <%#: Eval("IsPackageItem").ToString() == "False" ? "style=display:none" : ""%>>Detail
                                            Item</a>
                                        <img id="imgRegHasPackageAIO" class="imgRegHasPackageAIO" src='<%= ResolveUrl("~/Libs/Images/Status/has_aio_package.png")%>'
                                            title="Paket All-in-One" alt="" style='<%# Eval("IsPackageAllInOne").ToString() == "True" ? "width:24px;heght:24px": "width:24px;heght:24px;display:none" %>'>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-CssClass="gridColumnLink lnkProcedure" HeaderText="Tindakan"
                                    HeaderStyle-Width="80px">
                                    <ItemTemplate>
                                        <a>Tindakan</a>
                                    </ItemTemplate>
                                </asp:TemplateField>
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
    <div id="containerGrdDetail" class="borderBox" style="width: 100%; padding: 10px 5px 10px 5px;
        display: none">
        <table style="width: 90%; border-style: dotted; border-width: thin; background-color: transparent">
            <thead>
                <tr>
                    <th style="text-align: center" colspan="5">
                        <%=GetLabel("Mapping Pendapatan dan Diskon")%>
                    </th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td align="center">
                        <label class="lblLink lnkItemMasterCOA">
                            <%=GetLabel("Item COA")%>
                    </td>
                    <td align="center">
                        <label class="lblLink lnkItemMasterServiceUnitCOA">
                            <%=GetLabel("Item & ServiceUnit COA")%>
                    </td>
                    <td align="center">
                        <label class="lblLink lnkItemMasterClassCOA">
                            <%=GetLabel("Item & Class COA")%>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</asp:Content>
