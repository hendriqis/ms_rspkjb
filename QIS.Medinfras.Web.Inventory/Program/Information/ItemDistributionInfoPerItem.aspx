<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx2.master"
    AutoEventWireup="true" CodeBehind="ItemDistributionInfoPerItem.aspx.cs" Inherits="QIS.Medinfras.Web.Inventory.Program.ItemDistributionInfoPerItem" %>

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
    <li id="btnRefresh" CRUDMode="R" runat="server"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbrefresh.png")%>' alt="" /><br style="clear:both"/><div><%=GetLabel("Refresh")%></div></li>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle"><%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $(function () {
            setDatePicker('<%=txtDateFrom.ClientID %>');
            setDatePicker('<%=txtDateTo.ClientID %>');

//            $('#<%=txtDateFrom.ClientID %>').change(function () {
//                cbpView.PerformCallback('refresh');
//            });
//            $('#<%=txtDateTo.ClientID %>').change(function () {
//                cbpView.PerformCallback('refresh');
//            });

            $('#<%=btnRefresh.ClientID %>').click(function () {
                if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
                    cbpView.PerformCallback('refresh');
                }
            });

        });

        //#region Location From
        function getLocationFilterExpression() {
            var filterExpression = "<%:filterExpressionLocation %>";
            return filterExpression;
        }

        $('#lblFromLocation.lblLink').live('click', function () {
            openSearchDialog('locationroleuser', getLocationFilterExpression(), function (value) {
                $('#<%=txtFromLocationCode.ClientID %>').val(value);
                onTxtFromLocationCodeChanged(value);
            });
        });

        $('#<%=txtFromLocationCode.ClientID %>').live('change', function () {
            onTxtFromLocationCodeChanged($(this).val());
        });

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
        function getLocationFilterExpressionTo() {
            var filterExpression = "<%:filterExpressionLocationTo %>";
            return filterExpression;
        }

        $('#lblToLocation.lblLink').live('click', function () {
            openSearchDialog('locationroleuser', getLocationFilterExpressionTo(), function (value) {
                $('#<%=txtToLocationCode.ClientID %>').val(value);
                onTxtLocationToCodeChanged(value);
            });
        });

        $('#<%=txtToLocationCode.ClientID %>').live('change', function () {
            onTxtLocationToCodeChanged($(this).val());
        });

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

        function onCboTypeValueChanged() {
            onRefreshGrid();
        }

        function onCboDepartmentValueChanged() {
            onRefreshGrid();
        }

        $('.lblDetail').die('click');
        $('.lblDetail').live('click', function () {
            $tr = $(this).closest('tr');
            var id = $tr.find('.keyField').html();

            var url = ResolveUrl("~/Program/Information/ItemDistributionInfoPerItemDetailCtl.ascx");
            openUserControlPopup(url, id, 'Item Distribution Detail Information', 1000, 400);
        });
    </script>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
    <table width="100%">
        <tr>
            <td>
                <table class="tblEntryContent" style="width:550px;">
                    <colgroup>
                        <col style="width: 150px" />
                        <col style="width: 400px" />
                    </colgroup>
                    <tr>
                        <td><label class="lblLink" id="lblFromLocation"><%=GetLabel("Dari Lokasi") %></label></td>
                        <td>
                            <input type="hidden" id="hdnFromLocationID" runat="server" />
                            <table cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col width="100px" />
                                    <col width="3px" />
                                    <col width="250px"/>
                                </colgroup>
                                <tr>
                                    <td><asp:TextBox runat="server" ID="txtFromLocationCode" Width="100%" /></td>
                                    <td>&nbsp;</td>
                                    <td><asp:TextBox runat="server" ID="txtFromLocationName" Width="100%" Enabled="false" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td><label class="lblNormal lblLink" id="lblToLocation"><%=GetLabel("Kepada Lokasi") %></label></td>
                        <td>
                            <input type="hidden" id="hdnToLocationID" runat="server" />
                            <table cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col width="100px" />
                                    <col width="3px" />
                                    <col width="250px"/>
                                </colgroup>
                                <tr>
                                    <td><asp:TextBox runat="server" ID="txtToLocationCode" Width="100%" /></td>
                                    <td>&nbsp;</td>
                                    <td><asp:TextBox runat="server" ID="txtToLocationName" Width="100%"  Enabled="false" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td><label><%=GetLabel("Tanggal") %></label></td>
                        <td>
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td><asp:TextBox runat="server" CssClass="datepicker" ID="txtDateFrom" Width="120px" /></td>
                                    <td>&nbsp;</td>
                                    <td><asp:TextBox runat="server" CssClass="datepicker" ID="txtDateTo" Width="120px" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label><%=GetLabel("Quick Filter")%></label></td>
                        <td>
                            <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView" ID="txtSearchView" Width="300px" Watermark="Search">
                                <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                                <IntellisenseHints>
                                    <qis:QISIntellisenseHint Text="Nama" FieldName="ItemName1" />
                                    <qis:QISIntellisenseHint Text="No. Distribusi" FieldName="DistributionNo" />
                                </IntellisenseHints>
                            </qis:QISIntellisenseTextBox>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView" ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                        EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                position: relative; font-size: 0.95em;">
                                <input type="hidden" id="hdnFilterExpression" value="" runat="server" />
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect"
                                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" >
                                    <Columns>
                                        <asp:BoundField DataField="ItemID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:BoundField DataField="ItemCode" HeaderText="Kode" HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="ItemName1" HeaderText="Nama Item" HeaderStyle-HorizontalAlign="Center"  />
                                        <asp:TemplateField ItemStyle-HorizontalAlign="center" HeaderStyle-Width="100px">
                                            <ItemTemplate>
                                                <label class="lblLink lblDetail"><%=GetLabel("Detail") %></label>
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
