<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx2.master"
    AutoEventWireup="true" CodeBehind="OutstandingItemRequest.aspx.cs" Inherits="QIS.Medinfras.Web.Inventory.Program.OutstandingItemRequest" %>

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
    <script type="text/javascript">
        $('#<%=btnRefresh.ClientID %>').live('click', function () {
            cbpView.PerformCallback('refresh');
        });

        //#region Location From
        $('#lblFromLocation.lblLink').live('click', function () {
            openSearchDialog('locationroleuser', getLocationFilterExpression(), function (value) {
                $('#<%=txtFromLocationCode.ClientID %>').val(value);
                onTxtFromLocationCodeChanged(value);
            });
        });

        $('#<%=txtFromLocationCode.ClientID %>').live('change', function () {
            onTxtFromLocationCodeChanged($(this).val());
        });

        function getLocationFilterExpression() {
            var filterExpression = "<%:filterExpressionLocation %>";
            return filterExpression;
        }

        function onTxtFromLocationCodeChanged(value) {
            var filterExpression = getLocationFilterExpression() + "LocationCode = '" + value + "'";
            Methods.getObject('GetLocationUserAccessList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnFromLocationID.ClientID %>').val(result.LocationID);
                    $('#<%=txtFromLocationName.ClientID %>').val(result.LocationName);
                }
                else {
                    $('#<%=hdnFromLocationID.ClientID %>').val('');
                    $('#<%=txtFromLocationCode.ClientID %>').val('');
                    $('#<%=txtFromLocationName.ClientID %>').val('');
                }
            });

//            cbpView.PerformCallback('refresh');
        }
        //#endregion

        //#region Location To
        $('#lblToLocation.lblLink').live('click', function () {
            openSearchDialog('locationroleuser', getLocationFilterExpressionTo(), function (value) {
                $('#<%=txtToLocationCode.ClientID %>').val(value);
                onTxtLocationToCodeChanged(value);
            });
        });

        $('#<%=txtToLocationCode.ClientID %>').live('change', function () {
            onTxtLocationToCodeChanged($(this).val());
        });

        function getLocationFilterExpressionTo() {
            var filterExpression = "<%:filterExpressionLocationTo %>";
            return filterExpression;
        }

        function onTxtLocationToCodeChanged(value) {
            var filterExpression = getLocationFilterExpressionTo() + "LocationCode = '" + value + "'";
            Methods.getObject('GetLocationUserAccessList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnToLocationID.ClientID %>').val(result.LocationID);
                    $('#<%=txtToLocationName.ClientID %>').val(result.LocationName);
                }
                else {
                    $('#<%=hdnToLocationID.ClientID %>').val('');
                    $('#<%=txtToLocationCode.ClientID %>').val('');
                    $('#<%=txtToLocationName.ClientID %>').val('');
                }
            });

//            cbpView.PerformCallback('refresh');
        }
        //#endregion


        //#region IsClose
//        $('#<%=chkIsClose.ClientID %>').live('click', function () {
//            cbpView.PerformCallback('refresh');
//        });
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

        $('.trRequest.lblLink').live('click', function () {
            $tr = $(this);
            var itemID = $tr.parent().find('.keyField').html();
            var locationID = $('#<%=hdnFromLocationID.ClientID %>').val();
            var param = itemID + '|' + locationID;
            var url = ResolveUrl('~/Program/Information/OutstandingItemRequestCtl.ascx');
            openUserControlPopup(url, param, 'Permintaan Barang Pending', 1000, 600);
        });
    </script>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <table width="100%">
        <tr>
            <td>
                <table class="tblEntryContent" style="width: 100%;">
                    <colgroup>
                        <col style="width: 100px" />
                        <col style="width: 400px" />
                        <col style="width: 100px" />
                        <col style="width: 400px" />
                    </colgroup>
                    <tr>
                        <td>
                            <label class="lblLink" id="lblFromLocation">
                                <%=GetLabel("Dari Lokasi") %></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnFromLocationID" runat="server" />
                            <table cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col width="100px" />
                                    <col width="3px" />
                                    <col width="250px" />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtFromLocationCode" Width="100%" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtFromLocationName" Width="100%" Enabled="false" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            <label class="lblLink" id="lblToLocation">
                                <%=GetLabel("Ke Lokasi") %></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnToLocationID" runat="server" />
                            <table cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col width="100px" />
                                    <col width="3px" />
                                    <col width="250px" />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtToLocationCode" Width="100%" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtToLocationName" Width="100%" Enabled="false" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>
                                <%=GetLabel("Nama Item") %></label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtItemName" Width="100%" />
                        </td>
                    </tr>
                    <tr>
                    <td></td>
                        <td>
                            <asp:CheckBox ID="chkIsClose" Text="Termasuk Status Close" Font-Bold="true" Checked="true"
                                Width="100%" runat="server" />
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
                                        <asp:BoundField DataField="FromLocationName" HeaderText="Dari Lokasi" HeaderStyle-HorizontalAlign="Left"
                                            HeaderStyle-Width="200px" />
                                        <asp:BoundField DataField="ToLocationName" HeaderText="Kepada Lokasi" HeaderStyle-HorizontalAlign="Left"
                                            HeaderStyle-Width="200px" />
                                        <asp:BoundField DataField="ItemID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:BoundField DataField="ItemCode" HeaderText="Kode Item" HeaderStyle-HorizontalAlign="Left"
                                            HeaderStyle-Width="100px" />
                                        <asp:BoundField DataField="ItemName1" HeaderText="Nama Item" HeaderStyle-HorizontalAlign="Left" />
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="100px" ItemStyle-CssClass="trRequest lblLink"
                                            HeaderStyle-HorizontalAlign="Right">
                                            <HeaderTemplate>
                                                <%=GetLabel("Diminta") %>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr class="lblLink">
                                                        <td style="width: 70px" align="right">
                                                            <%#:Eval("RequestQty") %>
                                                        </td>
                                                        <td style="width: 5px">
                                                            &nbsp;
                                                        </td>
                                                        <td align="left">
                                                            <%#:Eval("ItemUnit") %>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Right">
                                            <HeaderTemplate>
                                                <%=GetLabel("Dikirim") %>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td style="width: 70px" align="right">
                                                            <%#:Eval("DistributionQty") %>
                                                        </td>
                                                        <td style="width: 5px">
                                                            &nbsp;
                                                        </td>
                                                        <td align="left">
                                                            <%#:Eval("ItemUnit") %>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Right">
                                            <HeaderTemplate>
                                                <%=GetLabel("Distribusi") %>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td style="width: 70px" align="right">
                                                            <%#:Eval("DraftDistribution") %>
                                                        </td>
                                                        <td style="width: 5px">
                                                            &nbsp;
                                                        </td>
                                                        <td align="left">
                                                            <%#:Eval("ItemUnit") %>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Right">
                                            <HeaderTemplate>
                                                <%=GetLabel("Pemakaian") %>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td style="width: 70px" align="right">
                                                            <%#:Eval("DraftConsumption") %>
                                                        </td>
                                                        <td style="width: 5px">
                                                            &nbsp;
                                                        </td>
                                                        <td align="left">
                                                            <%#:Eval("ItemUnit") %>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Right">
                                            <HeaderTemplate>
                                                <%=GetLabel("Minta Beli") %>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td style="width: 70px" align="right">
                                                            <%#:Eval("DraftPurchaseRequest") %>
                                                        </td>
                                                        <td style="width: 5px">
                                                            &nbsp;
                                                        </td>
                                                        <td align="left">
                                                            <%#:Eval("ItemUnit") %>
                                                        </td>
                                                    </tr>
                                                </table>
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
            </td>
        </tr>
    </table>
</asp:Content>
