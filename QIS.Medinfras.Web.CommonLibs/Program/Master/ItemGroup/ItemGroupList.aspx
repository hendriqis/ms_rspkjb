<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPList.master"
    AutoEventWireup="true" CodeBehind="ItemGroupList.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.ItemGroupList" %>

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

        function onRefreshControl(filterExpression) {
            $('#<%=hdnFilterExpression.ClientID %>').val(filterExpression);
            cbpView.PerformCallback('refresh');
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
                $newTr = $("<tr><td></td><td colspan='8'></td></tr>").attr('class', 'trDetail');
                $newTr.insertAfter($tr);
                $newTr.find('td').last().append($('#containerGrdDetail'));

                if ($trCollapse != null) {
                    $trCollapse.prev().find('.imgExpand').attr('src', '<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>');
                    $trCollapse.remove();
                }

                $('#<%=lvwViewDetail.ClientID %> tr:gt(0)').remove();
                $('#<%=hdnExpandID.ClientID %>').val($tr.find('.keyField').html());
                cbpViewDetail.PerformCallback();
            }
            else {
                $(this).find('.imgExpand').attr('src', '<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>');
                $('#tempContainerGrdDetail').append($('#containerGrdDetail'));

                $trDetail.remove();
            }
        });

        $('.lnkSubGroup a').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            var url = ResolveUrl("~/Libs/Program/Master/ItemGroup/ItemGroupDetailListCtl.ascx");
            openUserControlPopup(url, id, 'Kelompok Item', 1200, 500);
        });

        //#region Link GLRevenue

        $('.lnkItemGroupCOA').live('click', function () {
            var id = $(this).closest('tr').find('.keyFieldDt').html();
            var url = ResolveUrl("~/Libs/Program/Master/ItemGroup/GLRevenue/GLMappingItemGroupEntryCtl.ascx");
            openUserControlPopup(url, id, 'ItemGroup', 1200, 500);
        });

        $('.lnkItemGroupServiceUnitCOA').live('click', function () {
            var id = $(this).closest('tr').find('.keyFieldDt').html();
            var url = ResolveUrl("~/Libs/Program/Master/ItemGroup/GLRevenue/GLMappingItemGroupServiceUnitEntryCtl.ascx");
            openUserControlPopup(url, id, 'ItemGroup & ServiceUnit', 1200, 500);
        });

        $('.lnkItemGroupClassCOA').live('click', function () {
            var id = $(this).closest('tr').find('.keyFieldDt').html();
            var url = ResolveUrl("~/Libs/Program/Master/ItemGroup/GLRevenue/GLMappingItemGroupClassEntryCtl.ascx");
            openUserControlPopup(url, id, 'ItemGroup & Class', 1200, 500);
        });

        $('.lnkItemGroupClassServiceUnitCOA').live('click', function () {
            var id = $(this).closest('tr').find('.keyFieldDt').html();
            var url = ResolveUrl("~/Libs/Program/Master/ItemGroup/GLRevenue/GLRevenueItemGroupClassServiceUnitEntryCtl.ascx");
            openUserControlPopup(url, id, 'ItemGroup, Class & ServiceUnit', 1200, 500);
        });

        $('.lnkItemGroupServiceUnitbySourceUnitCOA').live('click', function () {
            var id = $(this).closest('tr').find('.keyFieldDt').html();
            var url = ResolveUrl("~/Libs/Program/Master/ItemGroup/GLRevenue/GLRevenueItemGroupServiceUnitBySourceUnitCtl.ascx");
            openUserControlPopup(url, id, 'ItemGroup, ServiceUnit & Source', 1200, 500);
        });

        $('.lnkItemGroupServiceUnitbySourceUnitbyClassCOA').live('click', function () {
            var id = $(this).closest('tr').find('.keyFieldDt').html();
            var url = ResolveUrl("~/Libs/Program/Master/ItemGroup/GLRevenue/GLRevenueItemGroupServiceUnitBySourceUnitbyClassCtl.ascx");
            openUserControlPopup(url, id, 'ItemGroup, ServiceUnit, Source & Class', 1200, 500);
        });

        $('.lnkItemGroupServiceUnitbySourceUnitbyClassbyParamedicCOA').live('click', function () {
            var id = $(this).closest('tr').find('.keyFieldDt').html();
            var url = ResolveUrl("~/Libs/Program/Master/ItemGroup/GLRevenue/GLRevenueItemGroupServiceUnitBySourceUnitbyClassbyParamedicCtl.ascx");
            openUserControlPopup(url, id, 'ItemGroup, ServiceUnit, Source, Class & Paramedic', 1200, 500);
        });

        $('.lnkItemGroupServiceUnitbyClassbyParamedicCOA').live('click', function () {
            var id = $(this).closest('tr').find('.keyFieldDt').html();
            var url = ResolveUrl("~/Libs/Program/Master/ItemGroup/GLRevenue/GLRevenueItemGroupServiceUnitbyClassbyParamedicCtl.ascx");
            openUserControlPopup(url, id, 'ItemGroup, ServiceUnit, Class & Paramedic', 1200, 500);
        });

        $('.lnkItemGroupServiceUnitCustomerLineCOA').live('click', function () {
            var id = $(this).closest('tr').find('.keyFieldDt').html();
            var url = ResolveUrl("~/Libs/Program/Master/ItemGroup/GLRevenue/GLRevenueItemGroupServiceUnitCustomerLineCtl.ascx");
            openUserControlPopup(url, id, 'ItemGroup, ServiceUnit & Customer Line', 1200, 500);
        });
        //#endregion

    </script>
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnGCItemType" runat="server" />
    <input type="hidden" value="" id="hdnQueryItem" runat="server" />
    <input type="hidden" value="" id="hdnFilterExpression" runat="server" />
    <input type="hidden" value="" id="hdnExpandID" runat="server" />
    <div style="position: relative;">
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                            ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdView_RowDataBound">
                            <Columns>
                                <asp:BoundField DataField="ItemGroupID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:TemplateField ItemStyle-Width="20px" ItemStyle-CssClass="tdExpand" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <img class="imgExpand imgLink" src='<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>'
                                            alt='' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Width="100px" HeaderStyle-CssClass="gridColumnText"
                                    ItemStyle-CssClass="gridColumnText">
                                    <HeaderTemplate>
                                        <div style="padding-left: 3px">
                                            <%=GetLabel("Kode Kelompok")%>
                                        </div>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div style='margin-left: <%#: Eval("Level") %>0px;'>
                                            <%#: Eval("ItemGroupCode") %></div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="ItemGroupName1" HeaderText="Nama Kelompok Item" HeaderStyle-CssClass="gridColumnText"
                                    ItemStyle-CssClass="gridColumnText" />
                                <asp:BoundField DataField="ItemGroupName2" HeaderText="Item Group Name 2" Visible="false"
                                    HeaderStyle-CssClass="gridColumnText" ItemStyle-CssClass="gridColumnText" />
                                <asp:BoundField DataField="ItemType" HeaderText="Jenis Item" HeaderStyle-Width="200px"
                                    HeaderStyle-CssClass="gridColumnText" ItemStyle-CssClass="gridColumnText" />
                                <asp:BoundField DataField="DisplayCITOAmount" HeaderText="CITO" HeaderStyle-Width="80px"
                                    HeaderStyle-CssClass="gridColumnBoolean" ItemStyle-CssClass="gridColumnBoolean" />
                                <asp:BoundField DataField="DisplayComplicationAmount" HeaderText="Complication" HeaderStyle-Width="80px"
                                    HeaderStyle-CssClass="gridColumnBoolean" ItemStyle-CssClass="gridColumnBoolean"
                                    Visible="false" />
                                <asp:BoundField DataField="PrintOrder" HeaderText="Urutan Cetak" HeaderStyle-Width="80px"
                                    ItemStyle-HorizontalAlign="Right" />
                                <asp:HyperLinkField HeaderStyle-Width="120px" Text="Sub Kelompok" ItemStyle-HorizontalAlign="Center"
                                    ItemStyle-CssClass="lnkSubGroup" HeaderStyle-HorizontalAlign="Center" />
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("Data tidak tersedia")%>
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
    <div id="tempContainerGrdDetail" style="display: none">
        <div id="containerGrdDetail" class="borderBox" style="width: 100%; padding: 10px 5px;">
            <div style="position: relative;">
                <dxcp:ASPxCallbackPanel ID="cbpViewDetail" runat="server" Width="100%" ClientInstanceName="cbpViewDetail"
                    ShowLoadingPanel="false" OnCallback="cbpViewDetail_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDetail').show(); }"
                        EndCallback="function(s,e){ $('#containerImgLoadingViewDetail').hide(); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent2" runat="server">
                            <asp:Panel runat="server" ID="Panel1" Style="width: 100%; margin-left: auto; margin-right: auto">
                                <asp:ListView runat="server" ID="lvwViewDetail">
                                    <EmptyDataTemplate>
                                        <table id="tblView" runat="server" class="lvwViewDt" cellspacing="0" rules="all">
                                            <tr>
                                                <th style="text-align: center" colspan="5">
                                                    <%=GetLabel("Mapping Pendapatan dan Diskon")%>
                                                </th>
                                            </tr>
                                            <tr>
                                                <th style="text-align: center">
                                                </th>
                                                <th style="text-align: center">
                                                </th>
                                                <th style="text-align: center">
                                                </th>
                                            </tr>
                                            <tr class="trEmpty">
                                                <td colspan="5">
                                                    <%=GetLabel("Tidak ada data")%>
                                                </td>
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                    <LayoutTemplate>
                                        <table id="tblView" runat="server" class="grdCollapsible lvwView" cellspacing="0"
                                            rules="all">
                                            <tr>
                                                <th style="text-align: center" colspan="3">
                                                    <%=GetLabel("Mapping Pendapatan dan Diskon")%>
                                                </th>
                                            </tr>
                                            <tr>
                                                <th style="text-align: center">
                                                </th>
                                                <th style="text-align: center">
                                                </th>
                                                <th style="text-align: center">
                                                </th>
                                            </tr>
                                            <tr runat="server" id="itemPlaceholder">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td class="keyFieldDt" style="display: none">
                                                <%#: Eval("ItemGroupID")%>
                                            </td>
                                            <td align="center">
                                                <label class="lblLink lnkItemGroupCOA">
                                                    <%=GetLabel("ItemGroup COA")%>
                                            </td>
                                            <td align="center">
                                                <label class="lblLink lnkItemGroupServiceUnitCOA">
                                                    <%=GetLabel("ItemGroup & ServiceUnit COA")%>
                                            </td>
                                            <td align="center">
                                                <label class="lblLink lnkItemGroupClassCOA">
                                                    <%=GetLabel("ItemGroup & Class COA")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="keyFieldDt" style="display: none">
                                                <%#: Eval("ItemGroupID")%>
                                            </td>
                                            <td align="center">
                                                <label class="lblLink lnkItemGroupClassServiceUnitCOA">
                                                    <%=GetLabel("ItemGroup,Class,ServiceUnit COA")%>
                                            </td>
                                            <td align="center">
                                                <label class="lblLink lnkItemGroupServiceUnitCustomerLineCOA">
                                                    <%=GetLabel("ItemGroup,ServiceUnit,CustomerLine COA")%>
                                            </td>
                                            <td align="center">
                                                <label class="lblLink lnkItemGroupServiceUnitbyClassbyParamedicCOA">
                                                    <%=GetLabel("ItemGroup,ServiceUnit,byClass,byParamedic COA")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="keyFieldDt" style="display: none">
                                                <%#: Eval("ItemGroupID")%>
                                            </td>
                                            <td align="center">
                                                <label class="lblLink lnkItemGroupServiceUnitbySourceUnitCOA">
                                                    <%=GetLabel("ItemGroup,ServiceUnit,bySource COA")%>
                                            </td>
                                            <td align="center">
                                                <label class="lblLink lnkItemGroupServiceUnitbySourceUnitbyClassCOA">
                                                    <%=GetLabel("ItemGroup,ServiceUnit,bySource,byClass COA")%>
                                            </td>
                                            <td align="center">
                                                <label class="lblLink lnkItemGroupServiceUnitbySourceUnitbyClassbyParamedicCOA">
                                                    <%=GetLabel("ItemGroup,ServiceUnit,bySource,byClass,byParamedic COA")%>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div class="imgLoadingGrdView" id="containerImgLoadingViewDetail">
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
