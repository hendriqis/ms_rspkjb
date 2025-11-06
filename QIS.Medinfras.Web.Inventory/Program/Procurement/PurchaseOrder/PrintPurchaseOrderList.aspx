<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="PrintPurchaseOrderList.aspx.cs" Inherits="QIS.Medinfras.Web.Inventory.Program.PrintPurchaseOrderList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnPurchaseRequestHdItem" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbitems.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Item")%></div>
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
            var grd = new customGridView();
            grd.init('<%=grdView.ClientID %>', '<%=hdnID.ClientID %>', '<%=pnlView.ClientID %>', cbpView, 'paging');

            $('#<%=btnPurchaseRequestHdItem.ClientID %>').click(function () {
                showLoadingPanel();
                var id = $('#<%=hdnID.ClientID %>').val() + "|" + "0";
                var url = ResolveUrl('~/Program/Procurement/PurchaseOrder/ApprovedPurchaseOrderDetailList.aspx?id=' + id);
                document.location = url;
            });
        });
        function onLoad() {

            //#region Location From
            function getLocationFilterExpression() {
                var filterExpression = "<%:filterExpressionLocation %>";
                return filterExpression;
            }

            $('#<%=lblLocation.ClientID %>.lblLink').live('click', function () {
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
                        $('#<%=hdnLocationIDFrom.ClientID %>').val(result.LocationID);
                        $('#<%=txtLocationName.ClientID %>').val(result.LocationName);
                        cbpView.PerformCallback('refresh');
                    }
                    else {
                        $('#<%=hdnLocationIDFrom.ClientID %>').val('');
                        $('#<%=txtLocationCode.ClientID %>').val('');
                        $('#<%=txtLocationName.ClientID %>').val('');
                        cbpView.PerformCallback('refresh');
                    }
                });
            }
            //#endregion
        }

        //#region Supplier
        function getSupplierFilterExpression() {
            var filterExpression = "GCBusinessPartnerType = 'X017^003' AND IsDeleted = 0 AND IsBlackList = 0 AND IsActive = 1";
            return filterExpression;
        }

        $('#<%=lblSupplier.ClientID %>.lblLink').live('click', function () {
            openSearchDialog('businesspartners', getSupplierFilterExpression(), function (value) {
                $('#<%=txtSupplierCode.ClientID %>').val(value);
                onTxtSupplierChanged(value);
            });
        });

        $('#<%=txtSupplierCode.ClientID %>').live('change', function () {
            onTxtSupplierChanged($(this).val());
        });

        function onTxtSupplierChanged(value) {
            var filterExpression = getSupplierFilterExpression() + " AND BusinessPartnerCode = '" + value + "'";
            Methods.getObject('GetBusinessPartnersList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnSupplierID.ClientID %>').val(result.BusinessPartnerID);
                    $('#<%=txtSupplierName.ClientID %>').val(result.BusinessPartnerName);
                    cbpView.PerformCallback('refresh');
                }
                else {
                    $('#<%=hdnSupplierID.ClientID %>').val('');
                    $('#<%=txtSupplierCode.ClientID %>').val('');
                    $('#<%=txtSupplierName.ClientID %>').val('');
                    cbpView.PerformCallback('refresh');
                }
            });
        }
        //#endregion

        function onAfterCustomClickSuccess(type) {
            cbpView.PerformCallback('refresh');
        }

        function onRefreshControl(filterExpression) {
            $('#<%=hdnFilterExpression.ClientID %>').val(filterExpression);
            cbpView.PerformCallback('refresh');
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
                    $('#<%=grdView.ClientID %> tr:eq(1)').click();

                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }
        //#endregion
    </script>
    <input type="hidden" value="" id="hdnParam" runat="server" />
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <div style="height: 550px; overflow-y: auto; overflow-x: hidden;">
        <table class="tblContentArea">
            <colgroup>
                <col style="width: 50%" />
                <col style="width: 50%" />
            </colgroup>
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 120px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory lblLink" runat="server" id="lblLocation">
                                    <%=GetLabel("Lokasi")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnLocationIDFrom" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 120px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtLocationCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtLocationName" Width="100%" runat="server" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal lblLink" runat="server" id="lblSupplier">
                                    <%=GetLabel("Supplier")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnSupplierID" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 120px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtSupplierCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSupplierName" Width="100%" runat="server" ReadOnly="true" />
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
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                        ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="PurchaseOrderID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="OrderDateInString" HeaderText="Tanggal Pemesanan" ItemStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="120px" />
                                            <asp:BoundField DataField="BusinessPartnerName" HeaderText="Supplier" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="PurchaseOrderNo" HeaderText="No. Pemesanan" HeaderStyle-Width="150px"
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="DeliveryDateInString" HeaderText="Tanggal Pengiriman"
                                                ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" />
                                            <asp:BoundField DataField="ExpiredDateInString" HeaderText="Tanggal Expired" ItemStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="120px" />
                                            <asp:BoundField DataField="cfTransactionAmount" HeaderText="Jumlah Transaksi" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
                                                DataFormatString="{0:N}" HeaderStyle-Width="150px" />
                                            <asp:BoundField DataField="CreatedByName" HeaderText="Dibuat Oleh" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left" />
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
    </div>
</asp:Content>
