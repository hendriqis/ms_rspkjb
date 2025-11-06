<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx2.master"
    AutoEventWireup="true" CodeBehind="PurchaseReceiveInfoPerItem.aspx.cs" Inherits="QIS.Medinfras.Web.Inventory.Program.PurchaseReceiveInfoPerItem" %>

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
    <div class="menuTitle"><%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $('#<%=btnRefresh.ClientID %>').live('click', function () {
            onRefreshGrid();
        });

        $(function () {
            setDatePicker('<%=txtDateFrom.ClientID %>');
            setDatePicker('<%=txtDateTo.ClientID %>');

//            $('#<%=txtDateFrom.ClientID %>').change(function () {
//                cbpView.PerformCallback('refresh');
//            });
//            $('#<%=txtDateTo.ClientID %>').change(function () {
//                cbpView.PerformCallback('refresh');
//            });

            //#region Location
            function onGetLocationFilterExpression() {
                var filterExpression = "<%:filterExpressionLocation %>";
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

//        function onCboTypeValueChanged() {
//            onRefreshGrid();
//        }

//        function onCboDepartmentValueChanged() {
//            onRefreshGrid();
//        }

        $('.lblDetail').die('click');
        $('.lblDetail').live('click', function () {
            $tr = $(this).closest('tr');
            var id = $tr.find('.keyField').html();
            var locationID = $('#<%=hdnLocationID.ClientID %>').val();
            var movementDate = $('#<%=txtDateFrom.ClientID %>').val() + ";" + $('#<%=txtDateTo.ClientID %>').val();
            $hdnItemID = $tr.find('.hdnItemID');
            var itemID = $hdnItemID.val();

            var param = locationID + '|' + movementDate + '|' + itemID;
            var url = ResolveUrl("~/Program/Information/PurchaseReceiveInfoDetailCtl.ascx");
            openUserControlPopup(url, param, 'Purchase Receive Detail Information', 1200, 500);
        });
    </script>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
    <table width="100%">
        <tr>
            <td>
                <table class="tblEntryContent" style="width:550px;">
                    <colgroup>
                        <col style="width: 120px" />
                        <col style="width: 400px" />
                    </colgroup>
                    <tr>
                        <td><label class="lblNormal lblLink" id="lblLocation"><%=GetLabel("Location")%></label></td>
                        <td>
                            <input type="hidden" value="" id="hdnLocationID" runat="server" />
                            <table style="width:100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width:30%"/>
                                    <col style="width:3px"/>
                                    <col/>
                                </colgroup>
                                <tr>
                                    <td><asp:TextBox runat="server" ID="txtLocationCode" Width="100%" /></td>
                                    <td>&nbsp;</td>
                                    <td><asp:TextBox runat="server" ID="txtLocationName" Width="100%" Enabled="false" /></td>
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
                                    <qis:QISIntellisenseHint Text="Nama Item" FieldName="ItemName1" />
                                    <qis:QISIntellisenseHint Text="Kode Item" FieldName="ItemCode" />
                                    <qis:QISIntellisenseHint Text="No Faktur" FieldName="ReferenceNo" />
                                    <qis:QISIntellisenseHint Text="No Faktur Pajak" FieldName="TaxInvoiceNo" />
                                    <qis:QISIntellisenseHint Text="No Batch" FieldName="BatchNoExpiredDate" />
                                </IntellisenseHints>
                            </qis:QISIntellisenseTextBox>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <div style="position: relative;">
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function() { showLoadingPanel(); }"
                            EndCallback="function(s,e){ onCbpViewEndCallback(s) }"/>
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height:320px;overflow-y: scroll;">
                                    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdView_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="ItemCode" HeaderText="Kode" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="ItemName1" HeaderText="Nama Item" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="cfAveragePriceText" HeaderText="Harga Rata-Rata" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                                            <asp:TemplateField HeaderText="Jumlah" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" >
                                                <ItemTemplate>
                                                    <div id="divUsedQuantity" runat="server"></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="center" HeaderStyle-Width="100px">
                                                <ItemTemplate>
                                                    <input type="hidden" value='<%#:Eval("ItemID") %>' class="hdnItemID"/>
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
                    <div class="imgLoadingGrdView" id="containerImgLoadingView" >
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                    <div class="containerPaging">
                        <div class="wrapperPaging">
                            <div id="paging"></div>
                        </div>
                    </div> 
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
