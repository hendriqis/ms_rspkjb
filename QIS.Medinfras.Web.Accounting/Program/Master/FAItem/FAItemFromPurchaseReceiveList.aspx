<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPList.master"
    AutoEventWireup="true" CodeBehind="FAItemFromPurchaseReceiveList.aspx.cs" Inherits="QIS.Medinfras.Web.Accounting.Program.FAItemFromPurchaseReceiveList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnDecline" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><div>
            <%=GetLabel("Void")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">
        $(function () {
            var grd = new customGridView();
            grd.init('<%=grdView.ClientID %>', '<%=hdnID.ClientID %>', '<%=pnlView.ClientID %>', cbpView, 'paging');

            setDatePicker('<%=txtReceivedDateFrom.ClientID %>');
            setDatePicker('<%=txtReceivedDateTo.ClientID %>');
        });

        //#region Supplier
        function getSupplierFilterExpression() {
            var filterExpression = "GCBusinessPartnerType = 'X017^003' AND IsBlackList = 0 AND IsDeleted = 0";
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
                }
                else {
                    $('#<%=hdnSupplierID.ClientID %>').val('');
                    $('#<%=txtSupplierCode.ClientID %>').val('');
                    $('#<%=txtSupplierName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        //#region Item
        function getItemFilterExpression() {
            var filterExpression = "IsDeleted = 0 AND GCItemStatus != 'X181^999'";
            filterExpression += " AND GCItemType IN ('X001^002','X001^003','X001^008','X001^009')";

            return filterExpression;
        }

        $('#lblItem.lblLink').live('click', function () {
            openSearchDialog('item', getItemFilterExpression(), function (value) {
                $('#<%=txtItemCode.ClientID %>').val(value);
                onTxtItemCodeChanged(value);
            });
        });

        $('#<%=txtItemCode.ClientID %>').live('change', function () {
            onTxtItemCodeChanged($(this).val());
        });

        function onTxtItemCodeChanged(value) {
            var filterExpression = getItemFilterExpression() + " AND ItemCode = '" + value + "'";
            Methods.getObject('GetvItemMasterList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnItemID.ClientID %>').val(result.ItemID);
                    $('#<%=txtItemName.ClientID %>').val(result.ItemName1);
                }
                else {
                    $('#<%=hdnItemID.ClientID %>').val('');
                    $('#<%=txtItemCode.ClientID %>').val('');
                    $('#<%=txtItemName.ClientID %>').val('');
                }
            });

        }
        //#endregion

        //#region Product Line
        function getProductLineFilterExpression() {
            var filterExpression = "IsDeleted = 0";
            return filterExpression;
        }

        $('#<%=lblProductLine.ClientID %>.lblLink').live('click', function () {
            openSearchDialog('productlineitemtype', getProductLineFilterExpression(), function (value) {
                $('#<%=txtProductLineCode.ClientID %>').val(value);
                onTxtProductLineCodeChanged(value);
            });
        });

        $('#<%=txtProductLineCode.ClientID %>').live('change', function () {
            onTxtProductLineCodeChanged($(this).val());
        });

        function onTxtProductLineCodeChanged(value) {
            var filterExpression = getProductLineFilterExpression() + " AND ProductLineCode = '" + value + "'";
            Methods.getObject('GetProductLineList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnProductLineID.ClientID %>').val(result.ProductLineID);
                    $('#<%=txtProductLineName.ClientID %>').val(result.ProductLineName);
                }
                else {
                    $('#<%=hdnProductLineID.ClientID %>').val('');
                    $('#<%=txtProductLineCode.ClientID %>').val('');
                    $('#<%=txtProductLineName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        $('#btnRefresh').live('click', function () {
            cbpView.PerformCallback('refresh');
        });

        $('#<%=btnDecline.ClientID %>').live('click', function () {
            var id = $('#<%=hdnID.ClientID %>').val();
            var isHasFAItem = "0";
            var transactionCode = $('#<%=hdnID.ClientID %>').val().split('|')[0];
            var id = $('#<%=hdnID.ClientID %>').val().split('|')[1];

            var filterExpression = "TransactionCode = '" + transactionCode + "' AND ID = " + id;
            Methods.getObject('GetvPurchaseReceiveDtFixedAssetList', filterExpression, function (result) {
                if (result != null) {
                    isHasFAItem = result.IsHasFAItem;
                    transactionCode = result.TransactionCode;
                }
            });

            id = id + "|" + transactionCode;

            if (isHasFAItem == "0") {
                var url = ResolveUrl("~/Program/Master/FAItem/VoidFAItemCtl.ascx");
                openUserControlPopup(url, id, 'Proses Batal Aset', 600, 200);
            } else {
                displayErrorMessageBox('ERROR', "Data item di penerimaan ini tidak dapat dibatalkan karena sudah diproses menjadi aset.");
            }
        });

        function onRefreshControl(filterExpression) {
            cbpView.PerformCallback('refresh');
        }

        function onGetCurrID() {
            return $('#<%=hdnID.ClientID %>').val();
        }

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
            }
            else {
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
            }
        }

    </script>
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnSupplierID" runat="server" />
    <input type="hidden" value="" id="hdnItemID" runat="server" />
    <input type="hidden" value="" id="hdnProductLineID" runat="server" />
    <div>
        <table class="tblContentArea" style="width: 100%">
            <colgroup>
                <col style="width: 70%" />
                <col style="width: 30%" />
            </colgroup>
            <tr>
                <td>
                    <table>
                        <colgroup>
                            <col style="width: 150px" />
                            <col style="width: 150px" />
                            <col style="width: 10px" />
                            <col style="width: 150px" />
                            <col style="width: 150px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td>
                                <label class="lblNormal">
                                    <%=GetLabel("Tgl Penerimaan") %></label>
                            </td>
                            <td>
                                <asp:TextBox runat="server" Width="120px" ID="txtReceivedDateFrom" CssClass="datepicker" />
                            </td>
                            <td style="text-align: center">
                                <label class="lblNormal">
                                    <%=GetLabel("s/d") %></label>
                            </td>
                            <td>
                                <asp:TextBox runat="server" Width="120px" ID="txtReceivedDateTo" CssClass="datepicker" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblSupplier" runat="server">
                                    <%=GetLabel("Supplier/Penyedia")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSupplierCode" Width="100%" runat="server" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtSupplierName" ReadOnly="true" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblItem">
                                    <%=GetLabel("Item")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtItemCode" Width="100%" runat="server" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtItemName" ReadOnly="true" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr id="trProductLine" runat="server">
                            <td>
                                <label class="lblLink" runat="server" id="lblProductLine">
                                    <%=GetLabel("Product Line")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtProductLineCode" Width="100%" runat="server" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtProductLineName" Width="100%" runat="server" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <input type="button" id="btnRefresh" value="R e f r e s h" class="btnRefresh w3-button w3-blue w3-border w3-border-blue w3-round-large" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                </td>
            </tr>
        </table>
    </div>
    <div>
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                            ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                            <Columns>
                                <asp:BoundField DataField="cfID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:BoundField DataField="ReceivedDateInString" HeaderText="Tanggal Terima" HeaderStyle-Width="120px"
                                    HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                    <HeaderTemplate>
                                        <%=GetLabel("Aset & Inventaris")%></HeaderTemplate>
                                    <ItemTemplate>
                                        <div style="font-size: 14px;">
                                            [<%#:Eval("ItemCode") %>]
                                            <%#:Eval("ItemName1") %></div>
                                        <div style="font-size: 12px;">
                                            <%#:Eval("RemarksDetail") %></div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Width="180px" HeaderStyle-HorizontalAlign="Left">
                                    <HeaderTemplate>
                                        <%=GetLabel("Informasi Penerimaan Barang")%></HeaderTemplate>
                                    <ItemTemplate>
                                        <div style="font-size: 14px;">
                                            <%#:Eval("PurchaseReceiveNo") %></div>
                                        <div style="font-size: 12px;">
                                            <%#:Eval("BusinessPartnerName") %></div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="ReferenceNo" HeaderText="Nomor Faktur" HeaderStyle-Width="120px"
                                    HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="ProductLineName" HeaderText="Product Line" HeaderStyle-Width="150px"
                                    HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="LocationName" HeaderText="Lokasi Penerimaan" HeaderStyle-Width="150px"
                                    HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
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
    </div>
</asp:Content>
