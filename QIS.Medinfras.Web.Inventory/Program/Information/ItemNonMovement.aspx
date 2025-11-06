<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx2.master"
    AutoEventWireup="true" CodeBehind="ItemNonMovement.aspx.cs" Inherits="QIS.Medinfras.Web.Inventory.Program.ItemNonMovement" %>

<%@ Register Assembly="DevExpress.Web.ASPxPivotGrid.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPivotGrid" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.ASPxPivotGrid.v11.1.Export, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPivotGrid.Export" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnRefresh" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbrefresh.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Refresh")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">
        $(function () {
            var grd = new customGridView2();
            grd.init('grdStockDetail', '<%=hdnID.ClientID %>', '<%=pnlView.ClientID %>', cbpView, 'paging');

            setDatePicker('<%=txtDateFrom.ClientID %>');
            setDatePicker('<%=txtDateTo.ClientID %>');

            $('#trTransactionTime').attr('style', 'display:none');

            $('#<%=btnRefresh.ClientID %>').click(function () {
                if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
                    cbpView.PerformCallback('refresh');
                }
            });
        });

        //#region Location
        function getLocationFilterExpression() {
            var filterExpression = "<%:filterExpressionLocation %>";
            return filterExpression;
        }

        $('#lblLocation.lblLink').live('click', function () {
            openSearchDialog('locationroleuser', getLocationFilterExpression(), function (value) {
                $('#<%=txtLocationCode.ClientID %>').val(value);
                onTxtLocationCodeChanged(value);
            });
        });

        $('#<%=txtLocationCode.ClientID %>').live('change', function () {
            onTxtLocationCodeChanged($(this).val());
        });

        function onTxtLocationCodeChanged(value) {
            var filterExpression = getLocationFilterExpression() + "LocationCode = '" + value + "'";
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
                    $('.grdStockDetail tr:eq(2)').click();
                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('.grdStockDetail tr:eq(2)').click();
        }
        //#endregion

        function onTxtSearchViewSearchClick(s) {
            setTimeout(function () {
                s.SetBlur();
                onRefreshGrid();
            }, 0);
        }
    </script>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
    <input type="hidden" value="" id="hdnID" runat="server" />
    <table width="100%">
        <tr>
            <td>
                <table class="tblEntryContent" style="width: 650px;">
                    <colgroup>
                        <col style="width: 150px" />
                        <col style="width: 600px" />
                    </colgroup>
                    <tr>
                        <td>
                            <label>
                                <%=GetLabel("Tanggal") %></label>
                        </td>
                        <td>
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:TextBox runat="server" CssClass="datepicker" ID="txtDateFrom" Width="120px" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" CssClass="datepicker" ID="txtDateTo" Width="120px" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label class="lblLink" id="lblLocation">
                                <%=GetLabel("Lokasi") %></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnLocationID" runat="server" />
                            <table cellpadding="0" cellspacing="0" style="width: 100%">
                                <colgroup>
                                    <col width="120px" />
                                    <col width="3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtLocationCode" Width="100%" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtLocationName" Width="335px" Enabled="false" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <div style="height: 450px; overflow-y: auto">
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
                                            <asp:BoundField DataField="ItemID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="ItemCode" HeaderText="Kode Item" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" />
                                            <asp:BoundField DataField="ItemName1" HeaderText="Nama Item" HeaderStyle-HorizontalAlign="Left"
                                                HeaderStyle-Width="200px" />
                                            <asp:BoundField DataField="LocationCode" HeaderText="Kode Lokasi" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" />
                                            <asp:BoundField DataField="LocationName" HeaderText="Nama Lokasi" HeaderStyle-HorizontalAlign="Left"
                                                HeaderStyle-Width="200px" />
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="150px">
                                                <HeaderTemplate>
                                                    <%=GetLabel("Qty Akhir") %>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%#:Eval("QuantityEnd") %>&nbsp;<%#:Eval("UnitName") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="BatchNumber" HeaderText="No Batch" HeaderStyle-HorizontalAlign="Left"
                                                HeaderStyle-Width="120px" />
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
</asp:Content>
