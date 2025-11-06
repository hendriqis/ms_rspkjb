<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx2.master"
    AutoEventWireup="true" CodeBehind="ItemBalanceInformation.aspx.cs" Inherits="QIS.Medinfras.Web.Inventory.Program.ItemBalanceInformation" %>

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
    <div>
        <input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
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
                <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView" ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                        EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                             <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                position: relative; font-size: 0.95em;">
                                <asp:ListView runat="server" ID="lvwView">
                                    <EmptyDataTemplate>
                                        <table id="tblView" runat="server" class="grdStockDetail grdSelected" cellspacing="0" rules="all" >
                                            <tr>  
                                                <th class="keyField" rowspan="2">&nbsp;</th>
                                                <th rowspan="2" align="left"><%=GetLabel("LOKASI")%></th>
                                                <th rowspan="2" align="left"><%=GetLabel("NAMA ITEM")%></th>
                                                <th rowspan="2" style="width:60px"><%=GetLabel("SATUAN")%></th>
                                                <th colspan="3" style="width:60px"><%=GetLabel("BALANCE")%></th>
                                                <th colspan="2"><%=GetLabel("PROCUREMENT")%></th>
                                                <th colspan="3"><%=GetLabel("DISTRIBUTION")%></th>
                                            </tr>
                                            <tr>
                                                <th style="width:60px" align="right"><%=GetLabel("MIN")%></th> 
                                                <th style="width:60px" align="right"><%=GetLabel("MAX")%></th> 
                                                <th style="width:60px" align="right"><%=GetLabel("ON HAND")%></th> 
                                                <th style="width:60px" align="right"><%=GetLabel("ON PR")%></th> 
                                                <th style="width:60px" align="right"><%=GetLabel("ON PO")%></th> 
                                                <th style="width:60px" align="right"><%=GetLabel("UNIT REQUEST")%></th> 
                                                <th style="width:60px" align="right"><%=GetLabel("DELIVERY")%></th> 
                                                <th style="width:60px" align="right"><%=GetLabel("CONFIRMATION")%></th> 
                                            </tr>
                                            <tr class="trEmpty">
                                                <td colspan="25">
                                                    <%=GetLabel("Tidak ada informasi saldo persediaan di lokasi ini")%>
                                                </td>
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                    <LayoutTemplate>
                                        <table id="tblView" runat="server" class="grdStockDetail grdSelected" cellspacing="0" rules="all" >
                                            <tr>  
                                                <th class="keyField" rowspan="2">&nbsp;</th>
                                                <th rowspan="2" align="left"><%=GetLabel("LOKASI")%></th>
                                                <th rowspan="2" align="left"><%=GetLabel("NAMA ITEM")%></th>
                                                <th rowspan="2" style="width:60px"><%=GetLabel("SATUAN")%></th>
                                                <th colspan="3" style="width:60px"><%=GetLabel("BALANCE")%></th>
                                                <th colspan="2"><%=GetLabel("PROCUREMENT")%></th>
                                                <th colspan="3"><%=GetLabel("DISTRIBUTION")%></th>
                                            </tr>
                                            <tr>
                                                <th style="width:60px" align="right"><%=GetLabel("MIN")%></th> 
                                                <th style="width:60px" align="right"><%=GetLabel("MAX")%></th> 
                                                <th style="width:60px" align="right"><%=GetLabel("ON HAND")%></th> 
                                                <th style="width:60px" align="right"><%=GetLabel("ON PR")%></th> 
                                                <th style="width:60px" align="right"><%=GetLabel("ON PO")%></th> 
                                                <th style="width:60px" align="right"><%=GetLabel("UNIT REQUEST")%></th> 
                                                <th style="width:60px" align="right"><%=GetLabel("DELIVERY")%></th> 
                                                <th style="width:60px" align="right"><%=GetLabel("CONFIRMATION")%></th> 
                                            </tr>
                                            <tr class="trEmpty">
                                                <td colspan="25">
                                                    <%=GetLabel("Tidak ada informasi saldo persediaan di lokasi ini")%>
                                                </td>
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td class="keyField"><%#: Eval("ItemID")%></td>
                                            <td><%#: Eval("LocationName")%></td>
                                            <td><%#: Eval("ItemName1")%></td>
                                            <td><%#: Eval("ItemUnit")%></td>
                                            <td align="right"><%#: Eval("QuantityMin", "{0:N}")%></td>
                                            <td align="right"><%#: Eval("QuantityMax", "{0:N}")%></td>
                                            <td align="right"><%#: Eval("QuantityEND", "{0:N}")%></td>
                                            <td align="right"><%#: Eval("PurchaseRequestQtyOnOrder", "{0:N}")%></td>
                                            <td align="right"><%#: Eval("PurchaseOrderQtyOnOrder", "{0:N}")%></td>
                                            <td align="right"><%#: Eval("ItemRequestQtyOnOrder", "{0:N}")%></td>
                                            <td align="right"><%#: Eval("ItemOnDeliveryQty", "{0:N}")%></td>
                                            <td align="right"><%#: Eval("ItemOnDistributionInQty", "{0:N}")%></td>
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
