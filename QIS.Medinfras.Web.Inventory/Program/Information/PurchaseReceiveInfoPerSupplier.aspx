<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx2.master"
    AutoEventWireup="true" CodeBehind="PurchaseReceiveInfoPerSupplier.aspx.cs" Inherits="QIS.Medinfras.Web.Inventory.Program.PurchaseReceiveInfoPerSupplier" %>

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

            //#region Supplier
            $('#lblSupplier.lblLink').click(function () {
                openSearchDialog('supplier', "", function (value) {
                    $('#<%=txtBusinessPartnerCode.ClientID %>').val(value);
                    onTxtBusinessPartnerCodeChanged(value);
                });
            });

            $('#<%=txtBusinessPartnerCode.ClientID %>').change(function () {
                onTxtBusinessPartnerCodeChanged($(this).val());
            });

            function onTxtBusinessPartnerCodeChanged(value) {
                var filterExpression = "BusinessPartnerCode = '" + value + "'";
                Methods.getObject('GetvSupplierList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnBusinessPartnerID.ClientID %>').val(result.BusinessPartnerID);
                        $('#<%=txtBusinessPartnerName.ClientID %>').val(result.BusinessPartnerName);
                    }
                    else {
                        $('#<%=hdnBusinessPartnerID.ClientID %>').val('');
                        $('#<%=txtBusinessPartnerCode.ClientID %>').val('');
                        $('#<%=txtBusinessPartnerName.ClientID %>').val('');
                    }
//                    cbpView.PerformCallback('refresh');
                });
            }
            //#endregion

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


        $('.lblDetail').die('click');
        $('.lblDetail').live('click', function () {
            $tr = $(this).closest('tr');
            var id = $tr.find('.keyField').html();
            var locationID = $('#<%=hdnLocationID.ClientID %>').val();
            var movementDate = $('#<%=txtDateFrom.ClientID %>').val() + ";" + $('#<%=txtDateTo.ClientID %>').val();
            $hdnPurchaseReceiveID = $tr.find('.hdnPurchaseReceiveID');
            var PurchaseReceiveID = $hdnPurchaseReceiveID.val();

            var param = locationID + '|' + movementDate + '|' + PurchaseReceiveID;
            var url = ResolveUrl("~/Program/Information/PurchaseOrderInfoDetailCtl2.ascx");
            openUserControlPopup(url, param, 'Purchase Order Detail Information', 1200, 400);
        });


    </script>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
    <table width="100%">
        <tr>
            <td>
                <table class="tblEntryContent" style="width: 550px;">
                    <colgroup>
                        <col style="width: 120px" />
                        <col style="width: 400px" />
                    </colgroup>
                    <tr>
                        <td>
                            <label class="lblNormal lblLink" id="lblSupplier">
                                <%=GetLabel("Supplier")%></label>
                        </td>
                        <td>
                            <input type="hidden" value="" id="hdnBusinessPartnerID" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 30%" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtBusinessPartnerCode" Width="100%" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtBusinessPartnerName" Width="100%" Enabled="false" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label class="lblNormal lblLink" id="lblLocation">
                                <%=GetLabel("Location")%></label>
                        </td>
                        <td>
                            <input type="hidden" value="" id="hdnLocationID" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 30%" />
                                    <col style="width: 3px" />
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
                                        <asp:TextBox runat="server" ID="txtLocationName" Width="100%" Enabled="false" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
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
                        <td class="tdLabel">
                            <label>
                                <%=GetLabel("Quick Filter")%></label>
                        </td>
                        <td>
                            <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView" ID="txtSearchView"
                                Width="300px" Watermark="Search">
                                <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                                <IntellisenseHints>
                                    <qis:QISIntellisenseHint Text="No. Penerimaan" FieldName="PurchaseReceiveNo" />
                                    <qis:QISIntellisenseHint Text="No. Faktur" FieldName="ReferenceNo" />
                                    <qis:QISIntellisenseHint Text="No. Faktur Pajak" FieldName="TaxInvoiceNo" />
                                    <qis:QISIntellisenseHint Text="No. Batch" FieldName="BatchNoExpiredDate" />
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
                        <ClientSideEvents BeginCallback="function() { showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s) }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height: 320px;
                                    overflow-y: scroll;">
                                    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                        ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="PurchaseReceiveNo" HeaderText="No. Penerimaan" HeaderStyle-Width="150px"
                                                HeaderStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="ReferenceNo" HeaderText="No. Faktur" HeaderStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="ItemName1" HeaderText="Nama Item" HeaderStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="ReceivedDateInString" HeaderText="Tanggal" HeaderStyle-Width="150px"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="LocationName" HeaderText="Lokasi" HeaderStyle-Width="150px"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                            <asp:TemplateField HeaderText="Jumlah" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center"
                                                HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <%#:Eval("Quantity")%>&nbsp;<%#:Eval("ItemUnit") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ItemDetailStatus" HeaderText="Status" HeaderStyle-Width="150px"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="100px"
                                                ItemStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    <%=GetLabel("Dibuat Oleh") %>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div>
                                                        <%#:Eval("CreatedByName") %></div>
                                                    <div>
                                                        <%#:Eval("cfCreatedDate") %></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="center" HeaderStyle-Width="100px">
                                                <ItemTemplate>
                                                    <input type="hidden" value='<%#:Eval("PurchaseReceiveID") %>' class="hdnPurchaseReceiveID"/>
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
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
