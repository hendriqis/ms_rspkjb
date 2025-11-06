<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPList.master" AutoEventWireup="true"
    CodeBehind="GLMappingRevenueAndDiscountList.aspx.cs" Inherits="QIS.Medinfras.Web.Accounting.Program.GLMappingRevenueAndDiscountList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">
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
                    $('.grdDetail tr:eq(2)').click();
                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('.grdDetail tr:eq(2)').click();
        }
        //#endregion

        //#region linkDT
        $('.lnkItemGroup').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html().trim();
            var url = ResolveUrl("~/Program/GLSetting/GLMappingRevenueAndDiscount/GLMappingItemGroupEntryCtl.ascx");
            openUserControlPopup(url, id, 'ItemGroup', 1200, 500);
        });

        $('.lnkItemMaster').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html().trim();
            var url = ResolveUrl("~/Program/GLSetting/GLMappingRevenueAndDiscount/GLMappingItemMasterEntryCtl.ascx");
            openUserControlPopup(url, id, 'Item', 1200, 500);
        });

        $('.lnkItemGroupServiceUnit').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html().trim();
            var url = ResolveUrl("~/Program/GLSetting/GLMappingRevenueAndDiscount/GLMappingItemGroupServiceUnitEntryCtl.ascx");
            openUserControlPopup(url, id, 'ItemGroup & ServiceUnit', 1200, 500);
        });

        $('.lnkItemMasterServiceUnit').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html().trim();
            var url = ResolveUrl("~/Program/GLSetting/GLMappingRevenueAndDiscount/GLMappingItemMasterServiceUnitEntryCtl.ascx");
            openUserControlPopup(url, id, 'Item & ServiceUnit', 1200, 500);
        });

        $('.lnkItemGroupClass').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html().trim();
            var url = ResolveUrl("~/Program/GLSetting/GLMappingRevenueAndDiscount/GLMappingItemGroupClassEntryCtl.ascx");
            openUserControlPopup(url, id, 'ItemGroup & Class', 1200, 500);
        });

        $('.lnkItemMasterClass').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html().trim();
            var url = ResolveUrl("~/Program/GLSetting/GLMappingRevenueAndDiscount/GLMappingItemMasterClassEntryCtl.ascx");
            openUserControlPopup(url, id, 'Item & Class', 1200, 500);
        });

        $('.lnkItemGroupClassServiceUnit').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html().trim();
            var url = ResolveUrl("~/Program/GLSetting/GLMappingRevenueAndDiscount/GLRevenueItemGroupClassServiceUnitEntryCtl.ascx");
            openUserControlPopup(url, id, 'ItemGroup, Class & ServiceUnit', 1200, 500);
        });

        $('.lnkItemGroupServiceUnitbySourceUnit').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html().trim();
            var url = ResolveUrl("~/Program/GLSetting/GLMappingRevenueAndDiscount/GLRevenueItemGroupServiceUnitBySourceUnitCtl.ascx");
            openUserControlPopup(url, id, 'ItemGroup, ServiceUnit & Source', 1200, 500);
        });

        $('.lnkItemGroupServiceUnitbySourceUnitbyClass').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html().trim();
            var url = ResolveUrl("~/Program/GLSetting/GLMappingRevenueAndDiscount/GLRevenueItemGroupServiceUnitBySourceUnitbyClassCtl.ascx");
            openUserControlPopup(url, id, 'ItemGroup, ServiceUnit, Source & Class', 1200, 500);
        });

        $('.lnkItemGroupServiceUnitbySourceDepartment').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html().trim();
            var url = ResolveUrl("~/Program/GLSetting/GLMappingRevenueAndDiscount/GLRevenueItemGroupServiceUnitBySourceDepartmentCtl.ascx");
            openUserControlPopup(url, id, 'ItemGroup, ServiceUnit & Source Department', 1200, 500);
        });

        $('.lnkItemGroupServiceUnitbySourceUnitbyClassbyParamedic').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html().trim();
            var url = ResolveUrl("~/Program/GLSetting/GLMappingRevenueAndDiscount/GLRevenueItemGroupServiceUnitBySourceUnitbyClassbyParamedicCtl.ascx");
            openUserControlPopup(url, id, 'ItemGroup, ServiceUnit, Source, Class & Paramedic', 1200, 500);
        });

        $('.lnkItemGroupServiceUnitbyClassbyParamedic').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html().trim();
            var url = ResolveUrl("~/Program/GLSetting/GLMappingRevenueAndDiscount/GLRevenueItemGroupServiceUnitbyClassbyParamedicCtl.ascx");
            openUserControlPopup(url, id, 'ItemGroup, ServiceUnit, Class & Paramedic', 1200, 500);
        });

        $('.lnkItemGroupServiceUnitCustomerLine').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html().trim();
            var url = ResolveUrl("~/Program/GLSetting/GLMappingRevenueAndDiscount/GLRevenueItemGroupServiceUnitCustomerLineCtl.ascx");
            openUserControlPopup(url, id, 'ItemGroup, ServiceUnit & Customer Line', 1200, 500);
        });
        //#endregion

    </script>
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <div style="position: relative;">
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlGridView" Style="width: 100%; margin-left: auto;
                        margin-right: auto; position: relative; font-size: 0.95em; height: 365px; overflow-y: scroll;">
                        <asp:ListView runat="server" ID="lvwView" OnItemDataBound="lvwView_ItemDataBound">
                            <EmptyDataTemplate>
                                <table id="tblView" runat="server" class="grdDetail lvwView" cellspacing="0" rules="all">
                                    <tr>
                                        <th class="keyField" rowspan="2">
                                            &nbsp;
                                        </th>
                                        <th style="width: 50px; text-align: left">
                                            <%=GetLabel("Kode COA")%>
                                        </th>
                                        <th style="text-align: left">
                                            <%=GetLabel("Nama COA")%>
                                        </th>
                                        <th style="width: 40px; text-align: center">
                                            <%=GetLabel("Saldo Normal")%>
                                        </th>
                                        <th colspan="4" style="text-align: center">
                                            <%=GetLabel("ItemGroup")%>
                                        </th>
                                        <th style="width: 150px; text-align: center">
                                            <%=GetLabel("Item")%>
                                        </th>
                                    </tr>
                                    <tr class="trEmpty">
                                        <td colspan="15">
                                            <%=GetLabel("Tidak ada data")%>
                                        </td>
                                    </tr>
                                </table>
                            </EmptyDataTemplate>
                            <LayoutTemplate>
                                <table id="tblView" runat="server" class="grdCollapsible lvwView" cellspacing="0"
                                    rules="all">
                                    <tr>
                                        <th class="keyField" rowspan="2">
                                            &nbsp;
                                        </th>
                                        <th style="width: 50px; text-align: left">
                                            <%=GetLabel("Kode COA")%>
                                        </th>
                                        <th style="text-align: left">
                                            <%=GetLabel("Nama COA")%>
                                        </th>
                                        <th style="width: 40px; text-align: center">
                                            <%=GetLabel("Saldo Normal")%>
                                        </th>
                                        <th colspan="3" style="width: 600px; text-align: center">
                                            <%=GetLabel("ItemGroup")%>
                                        </th>
                                        <th style="width: 170px; text-align: center">
                                            <%=GetLabel("Item")%>
                                        </th>
                                    </tr>
                                    <tr runat="server" id="itemPlaceholder">
                                    </tr>
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td class="keyField">
                                        <%#: Eval("GLAccountID")%>
                                    </td>
                                    <td id="Td1" align="left" runat="server">
                                        <div style='margin-left: <%#: Eval("Level") %>0px;'>
                                            <%#: Eval("GLAccountNo")%></div>
                                    </td>
                                    <td id="Td2" align="left" runat="server">
                                        <div style='margin-left: <%#: Eval("Level") %>0px;'>
                                            <%#: Eval("GLAccountName")%></div>
                                    </td>
                                    <td id="Td3" align="center" runat="server">
                                        <div>
                                            <%#: Eval("Position") %></div>
                                    </td>
                                    <td align="center">
                                        <label class="lblLink lnkItemGroup">
                                            <%=GetLabel("ItemGroup")%>
                                    </td>
                                    <td align="center">
                                        <label class="lblLink lnkItemGroupServiceUnit">
                                            <%=GetLabel("ItemGroup & ServiceUnit")%>
                                    </td>
                                    <td align="center">
                                        <label class="lblLink lnkItemGroupClass">
                                            <%=GetLabel("ItemGroup & Class")%>
                                    </td>
                                    <td align="center">
                                        <label class="lblLink lnkItemMaster">
                                            <%=GetLabel("Item")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="keyField">
                                        <%#: Eval("GLAccountID")%>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td align="center">
                                        <label class="lblLink lnkItemGroupServiceUnitCustomerLine">
                                            <%=GetLabel("ItemGroup,ServiceUnit,CustomerLine")%>
                                    </td>
                                    <td align="center">
                                        <label class="lblLink lnkItemGroupClassServiceUnit">
                                            <%=GetLabel("ItemGroup,Class,ServiceUnit")%>
                                    </td>
                                    <td align="center">
                                        <label class="lblLink lnkItemMasterServiceUnit">
                                            <%=GetLabel("Item & ServiceUnit")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="keyField">
                                        <%#: Eval("GLAccountID")%>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td align="center">
                                        <label class="lblLink lnkItemGroupServiceUnitbySourceUnit">
                                            <%=GetLabel("ItemGroup,ServiceUnit,bySource")%>
                                    </td>
                                    <td align="center">
                                        <label class="lblLink lnkItemGroupServiceUnitbyClassbyParamedic">
                                            <%=GetLabel("ItemGroup,ServiceUnit,byClass,byParamedic")%>
                                    </td>
                                    <td align="center">
                                        <label class="lblLink lnkItemMasterClass">
                                            <%=GetLabel("Item & Class")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="keyField">
                                        <%#: Eval("GLAccountID")%>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td align="center">
                                        <label class="lblLink lnkItemGroupServiceUnitbySourceUnitbyClass">
                                            <%=GetLabel("ItemGroup,ServiceUnit,bySource,byClass")%>
                                    </td>
                                    <td align="center">
                                        <label class="lblLink lnkItemGroupServiceUnitbySourceUnitbyClassbyParamedic">
                                            <%=GetLabel("ItemGroup,ServiceUnit,bySource,byClass,byParamedic")%>
                                    </td>
                                    <td align="center">
                                        <label class="lblLink lnkItemGroupServiceUnitbySourceDepartment">
                                            <%=GetLabel("ItemGroup,ServiceUnit,bySourceDepartment")%>
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
    </div>
</asp:Content>
