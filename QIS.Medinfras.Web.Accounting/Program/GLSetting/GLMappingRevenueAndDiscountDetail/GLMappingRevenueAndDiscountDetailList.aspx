<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPList.master" AutoEventWireup="true"
    CodeBehind="GLMappingRevenueAndDiscountDetailList.aspx.cs" Inherits="QIS.Medinfras.Web.Accounting.Program.GLMappingRevenueAndDiscountDetailList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">
        //#region link
        $('.lnkItemGroup').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            var url = ResolveUrl("~/Program/GLSetting/GLMappingRevenueAndDiscountDetail/GLMappingItemGroupDetailEntryCtl.ascx");
            openUserControlPopup(url, id, 'ItemGroup', 1200, 500);
        });
        $('.lnkItemMaster').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            var url = ResolveUrl("~/Program/GLSetting/GLMappingRevenueAndDiscountDetail/GLMappingItemMasterDetailEntryCtl.ascx");
            openUserControlPopup(url, id, 'Item', 1200, 500);
        });
        $('.lnkItemGroupServiceUnit').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            var url = ResolveUrl("~/Program/GLSetting/GLMappingRevenueAndDiscountDetail/GLMappingItemGroupServiceUnitDetailEntryCtl.ascx");
            openUserControlPopup(url, id, 'ItemGroup & ServiceUnit', 1200, 500);
        });
        $('.lnkItemMasterServiceUnit').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            var url = ResolveUrl("~/Program/GLSetting/GLMappingRevenueAndDiscountDetail/GLMappingItemMasterServiceUnitDetailEntryCtl.ascx");
            openUserControlPopup(url, id, 'Item & ServiceUnit', 1200, 500);
        });

        $('.lnkItemGroupClass').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            var url = ResolveUrl("~/Program/GLSetting/GLMappingRevenueAndDiscountDetail/GLMappingItemGroupClassDetailEntryCtl.ascx");
            openUserControlPopup(url, id, 'ItemGroup & Class', 1200, 500);
        });

        $('.lnkItemMasterClass').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            var url = ResolveUrl("~/Program/GLSetting/GLMappingRevenueAndDiscountDetail/GLMappingItemMasterClassDetailEntryCtl.ascx");
            openUserControlPopup(url, id, 'Item & Class', 1200, 500);
        });

        $('.lnkItemGroupClassServiceUnit').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            var url = ResolveUrl("~/Program/GLSetting/GLMappingRevenueAndDiscountDetail/GLRevenueItemGroupClassServiceUnitDetailEntryCtl.ascx");
            openUserControlPopup(url, id, 'ItemGroup, Class & ServiceUnit', 1200, 500);
        });

        $('.lnkItemGroupServiceUnitBySourceUnit').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            var url = ResolveUrl("~/Program/GLSetting/GLMappingRevenueAndDiscountDetail/GLRevenueItemGroupServiceUnitBySourceUnitDetailCtl.ascx");
            openUserControlPopup(url, id, 'ItemGroup, ServiceUnit & Source', 1200, 500);
        });

        $('.lnkItemGroupServiceUnitBySourceUnitbyClass').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            var url = ResolveUrl("~/Program/GLSetting/GLMappingRevenueAndDiscountDetail/GLRevenueItemGroupServiceUnitBySourceUnitbyClassDetailCtl.ascx");
            openUserControlPopup(url, id, 'ItemGroup, ServiceUnit, Source & Class', 1200, 500);
        });

        $('.lnkItemGroupServiceUnitBySourceUnitbyClassbyParamedic').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            var url = ResolveUrl("~/Program/GLSetting/GLMappingRevenueAndDiscountDetail/GLRevenueItemGroupServiceUnitBySourceUnitbyClassbyParamedicDetailCtl.ascx");
            openUserControlPopup(url, id, 'ItemGroup, ServiceUnit, Source, Class & Paramedic', 1200, 500);
        });

        $('.lnkItemGroupServiceUnitbyClassbyParamedic').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            var url = ResolveUrl("~/Program/GLSetting/GLMappingRevenueAndDiscountDetail/GLRevenueItemGroupServiceUnitbyClassbyParamedicDetailCtl.ascx");
            openUserControlPopup(url, id, 'ItemGroup, ServiceUnit, Class & Paramedic', 1200, 500);
        });

        $('.lnkItemGroupServiceUnitCustomerLine').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            var url = ResolveUrl("~/Program/GLSetting/GLMappingRevenueAndDiscountDetail/GLRevenueItemGroupServiceUnitCustomerLineDetailCtl.ascx");
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
                                        <th style="width: 130px; text-align: center">
                                            <%=GetLabel("Item Group")%>
                                        </th>
                                        <th style="width: 130px; text-align: center">
                                            <%=GetLabel("Item")%>
                                        </th>
                                        <th style="width: 130px; text-align: center">
                                            <%=GetLabel("ItemGroup & ServiceUnit")%>
                                        </th>
                                        <th style="width: 130px; text-align: center">
                                            <%=GetLabel("Item & ServiceUnit")%>
                                        </th>
                                        <th style="width: 130px; text-align: center">
                                            <%=GetLabel("ItemGroup & Class")%>
                                        </th>
                                        <th style="width: 130px; text-align: center">
                                            <%=GetLabel("Item & Class")%>
                                        </th>
                                    </tr>
                                    <tr class="trEmpty">
                                        <td colspan="14">
                                            <%=GetLabel("Tidak ada kunjungan pada tanggal yang dipilih")%>
                                        </td>
                                    </tr>
                                </table>
                            </EmptyDataTemplate>
                            <LayoutTemplate>
                                <table id="tblView" runat="server" class="grdCollapsible lvwView" cellspacing="0"
                                    rules="all">
                                    <tr>
                                        <th style="width: 130px; text-align: center">
                                            <%=GetLabel("Item Group")%>
                                        </th>
                                        <th style="width: 130px; text-align: center">
                                            <%=GetLabel("Item")%>
                                        </th>
                                        <th style="width: 130px; text-align: center">
                                            <%=GetLabel("ItemGroup & ServiceUnit")%>
                                        </th>
                                        <th style="width: 130px; text-align: center">
                                            <%=GetLabel("Item & ServiceUnit")%>
                                        </th>
                                        <th style="width: 130px; text-align: center">
                                            <%=GetLabel("ItemGroup & Class")%>
                                        </th>
                                        <th style="width: 130px; text-align: center">
                                            <%=GetLabel("Item & Class")%>
                                        </th>
                                    </tr>
                                    <tr runat="server" id="itemPlaceholder">
                                    </tr>
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td align="center">
                                        <label class="lblLink lnkItemGroup">
                                            <%=GetLabel("Item Group")%>
                                    </td>
                                    <td align="center">
                                        <label class="lblLink lnkItemMaster">
                                            <%=GetLabel("Item")%>
                                    </td>
                                    <td align="center">
                                        <label class="lblLink lnkItemGroupServiceUnit">
                                            <%=GetLabel("ItemGroup & ServiceUnit")%>
                                    </td>
                                    <td align="center">
                                        <label class="lblLink lnkItemMasterServiceUnit">
                                            <%=GetLabel("Item & ServiceUnit")%>
                                    </td>
                                    <td align="center">
                                        <label class="lblLink lnkItemGroupClass">
                                            <%=GetLabel("ItemGroup & Class")%>
                                    </td>
                                    <td align="center">
                                        <label class="lblLink lnkItemMasterClass">
                                            <%=GetLabel("Item & Class")%>
                                    </td>
                                </tr>
                                <tr class="trDetail">
                                    <td align="center">
                                        <label class="lblLink lnkItemGroupClassServiceUnit">
                                            <%=GetLabel("ItemGroup, Class & ServiceUnit")%>
                                    </td>
                                    <td>
                                    </td>
                                    <td align="center">
                                        <label class="lblLink lnkItemGroupServiceUnitBySourceUnit">
                                            <%=GetLabel("ItemGroup, ServiceUnit & Source")%>
                                    </td>
                                    <td>
                                    </td>
                                    <td align="center">
                                        <label class="lblLink lnkItemGroupServiceUnitBySourceUnitbyClass">
                                            <%=GetLabel("ItemGroup, ServiceUnit, Source & Class")%>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr class="trDetail">
                                    <td align="center">
                                        <label class="lblLink lnkItemGroupServiceUnitBySourceUnitbyClassbyParamedic">
                                            <%=GetLabel("ItemGroup, ServiceUnit, Source, Class & Paramedic")%>
                                    </td>
                                    <td>
                                    </td>
                                    <td align="center">
                                        <label class="lblLink lnkItemGroupServiceUnitbyClassbyParamedic">
                                            <%=GetLabel("ItemGroup, ServiceUnit, Class & Paramedic")%>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr class="trDetail">
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td align="center">
                                        <label class="lblLink lnkItemGroupServiceUnitCustomerLine">
                                            <%=GetLabel("ItemGroup, ServiceUnit & Customer Line")%>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:ListView>
                    </asp:Panel>
                </dx:PanelContent>
            </PanelCollection>
        </dxcp:ASPxCallbackPanel>
    </div>
</asp:Content>
