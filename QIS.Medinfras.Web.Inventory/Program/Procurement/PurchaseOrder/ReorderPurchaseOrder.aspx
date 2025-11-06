<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx2.master"
    AutoEventWireup="true" CodeBehind="ReorderPurchaseOrder.aspx.cs" Inherits="QIS.Medinfras.Web.Inventory.Program.ReorderPurchaseOrder" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnReorderPurchaseOrderProcess" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Process")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            setDatePicker('<%=txtItemOrderDate.ClientID %>');
            $('#<%=txtItemOrderDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

            setDatePicker('<%=txtItemOrderDeliveryDate.ClientID %>');
            $('#<%=txtItemOrderDeliveryDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

            setDatePicker('<%=txtItemOrderExpiredDate.ClientID %>');
            $('#<%=txtItemOrderExpiredDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

            $('#<%=btnReorderPurchaseOrderProcess.ClientID %>').click(function () {
                if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
                    var errMessage = { text: '' };
                    getCheckedMember(errMessage);
                    if (errMessage.text == '') {
                        if ($('#<%=hdnSelectedMember.ClientID %>').val() == '') {
                            showToast('Warning', 'Please Select Item First');
                        }
                        else {
                            onCustomButtonClick('approve');
                        }
                    }
                    else
                        showToast('Warning', errMessage.text);
                }
            });

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
                        $('#<%=hdnLocationID.ClientID %>').val(result.LocationID);
                        $('#<%=txtLocationName.ClientID %>').val(result.LocationName);

                        filterExpression = "LocationID = " + result.LocationID;
                        Methods.getObject('GetLocationList', filterExpression, function (result) {
                            $('#<%=hdnGCLocationGroup.ClientID %>').val(result.GCLocationGroup);
                        });
                    }
                    else {
                        $('#<%=hdnLocationID.ClientID %>').val('');
                        $('#<%=txtLocationCode.ClientID %>').val('');
                        $('#<%=txtLocationName.ClientID %>').val('');
                    }
                });
                cbpView.PerformCallback('refresh');
            }
            //#endregion

            //#region Product Line
            function getProductLineFilterExpression() {
                var filterExpression = "IsDeleted = 0";
                return filterExpression;
            }

            $('#<%=lblProductLine.ClientID %>.lblLink').click(function () {
                openSearchDialog('productlineitemtype', getProductLineFilterExpression(), function (value) {
                    $('#<%=txtProductLineCode.ClientID %>').val(value);
                    onTxtProductLineCodeChanged(value);
                });
            });

            $('#<%=txtProductLineCode.ClientID %>').change(function () {
                onTxtProductLineCodeChanged($(this).val());
            });

            function onTxtProductLineCodeChanged(value) {
                var filterExpression = getProductLineFilterExpression() + " AND ProductLineCode = '" + value + "'";
                Methods.getObject('GetProductLineList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnProductLineID.ClientID %>').val(result.ProductLineID);
                        $('#<%=txtProductLineName.ClientID %>').val(result.ProductLineName);
                        $('#<%=hdnProductLineItemType.ClientID %>').val(result.GCItemType);
                    }
                    else {
                        $('#<%=hdnProductLineID.ClientID %>').val('');
                        $('#<%=txtProductLineCode.ClientID %>').val('');
                        $('#<%=txtProductLineName.ClientID %>').val('');
                        $('#<%=hdnProductLineItemType.ClientID %>').val('');
                    }
                    cbpView.PerformCallback('refresh');
                });
            }
            //#endregion

            var pageCount = parseInt($('#<%=hdnPageCount.ClientID %>').val());
            setPaging($("#paging"), pageCount, function (page) {
                getCheckedMember();
                cbpView.PerformCallback('changepage|' + page);
            });
        }

        $('#chkSelectAll').die('change');
        $('#chkSelectAll').live('change', function () {
            var isChecked = $(this).is(":checked");
            $('.chkIsSelected input').each(function () {
                $(this).prop('checked', isChecked);
                $(this).change();
            });
        });

        $('.chkIsSelected input').live('change', function () {
            $tr = $(this).closest('tr');
            $lblSupplier = $tr.find('.lblSupplier');
            if ($(this).is(':checked')) {
                $tr.find('.txtPurchaseOrder').removeAttr('readonly');
                $lblSupplier.removeClass('lblDisabled');
                $lblSupplier.addClass('lblLink');
            }
            else {
                $tr.find('.txtPurchaseOrder').attr('readonly', 'readonly');
                $lblSupplier.removeClass('lblLink');
                $lblSupplier.addClass('lblDisabled');
            }
        });

        $td = null;
        $('.lblSupplier.lblLink').live('click', function () {
            $td = $(this).parent();
            var itemID = $td.find('.hdnItemID').val();
            openSearchDialog('supplierforreorderpo', itemID, function (value) {
                onTxtSupplierChanged(value);
            });
        });

        function onTxtSupplierChanged(value) {
            var filterExpression = "BusinessPartnerID = '" + value + "'";
            Methods.getObject('GetvSupplierList', filterExpression, function (result) {
                if (result != null) {
                    $td.find('.hdnSupplierID').val(result.BusinessPartnerID);
                    if (result.BusinessPartnerName != "")
                        $td.find('.lblSupplier').html(result.BusinessPartnerName);
                    else
                        $td.find('.lblSupplier').html(result.BusinessPartnerName);
                }
                else {
                    $td.find('.hdnSupplierID').val('0');
                }
            });
        }

        function onAfterCustomClickSuccess(type, retval) {
            var orderPerSupplier = retval.split(';');
            var tempText = "";
            for (var a = 0; a < orderPerSupplier.length - 1; a++) {
                var paramDetail = orderPerSupplier[a].split('^');
                if (tempText != '')
                    tempText += "<br />";
                tempText += "Pemesanan Barang Untuk Supplier <b>" + paramDetail[1] + "</b> Dengan Nomor Pemesanan <b>" + paramDetail[0] + "</b>";
            }
            showToast('Save Success', tempText, function () {
                $('#<%=hdnPurchaseOrder.ClientID %>').val('');
                $('#<%=hdnSelectedMember.ClientID %>').val('');
                $('#<%=hdnListSupplierID.ClientID %>').val('');
                cbpView.PerformCallback('refresh');
            });
        }

        function getCheckedMember(errMessage) {
            var lstSelectedMember = $('#<%=hdnSelectedMember.ClientID %>').val().split(',');
            var lstPurchaseOrder = $('#<%=hdnPurchaseOrder.ClientID %>').val().split(',');
            var lstSupplierID = $('#<%=hdnListSupplierID.ClientID %>').val().split(',');
            var result = '';
            var itemEmptySupplier = '';
            var itemEmptyQty = '';
            $('#<%=grdView.ClientID %> .chkIsSelected input').each(function () {
                if ($(this).is(':checked')) {
                    var key = $(this).closest('tr').find('.keyField').html();
                    var purchaseOrder = $(this).closest('tr').find('.txtPurchaseOrder').val();
                    var supplierID = $(this).closest('tr').find('.hdnSupplierID').val();
                    var itemName = $(this).closest('tr').find('.tdItemName').html();
                    var idx = lstSelectedMember.indexOf(key);
                    if (idx < 0) {
                        lstSelectedMember.push(key);
                        lstPurchaseOrder.push(purchaseOrder);
                        lstSupplierID.push(supplierID);
                    }
                    else {
                        lstPurchaseOrder[idx] = purchaseOrder;
                        lstSupplierID[idx] = supplierID;
                    }
                    if (supplierID == '0') {
                        if (itemEmptySupplier != '')
                            itemEmptySupplier += ', ';
                        itemEmptySupplier += '<b>' + itemName + '</b>';
                    }
                    if (purchaseOrder == '0') {
                        if (itemEmptyQty != '')
                            itemEmptyQty += ', ';
                        itemEmptyQty += '<b>' + itemName + '</b>';
                    }
                }
                else {
                    var key = $(this).closest('tr').find('.keyField').html();
                    var idx = lstSelectedMember.indexOf(key);
                    if (idx > -1) {
                        lstSelectedMember.splice(idx, 1);
                        lstPurchaseOrder.splice(idx, 1);
                        lstSupplierID.splice(idx, 1);
                    }
                }
            });
            if (errMessage != null) {
                if (itemEmptySupplier != '')
                    errMessage.text = 'Silakan Pilih Supplier Untuk Item ' + itemEmptySupplier + ' Terlebih Dahulu';
                if (itemEmptyQty != '') {
                    if (errMessage.text != '')
                        errMessage.text += '<br>';
                    errMessage.text += 'Silakan Isi Qty Untuk Item ' + itemEmptySupplier + ' Terlebih Dahulu';
                }
            }
            $('#<%=hdnPurchaseOrder.ClientID %>').val(lstPurchaseOrder.join(','));
            $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedMember.join(','));
            $('#<%=hdnListSupplierID.ClientID %>').val(lstSupplierID.join(','));
        }

        function onRefreshControl(filterExpression) {
            cbpView.PerformCallback('refresh');
        }

        //#region Paging
        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdView.ClientID %> tr:eq(1)').click();

                setPaging($("#paging"), pageCount, function (page) {
                    getCheckedMember();
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }
        //#endregion
    </script>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnPageCount" runat="server" />
    <input type="hidden" value="0" id="hdnIsUsedProductLine" runat="server" />
    <%--<input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />--%>
    <input type="hidden" value="" id="hdnQuickText" runat="server" />
    <input type="hidden" id="hdnSelectedMember" runat="server" value="" />
    <input type="hidden" id="hdnPurchaseOrder" runat="server" value="" />
    <input type="hidden" id="hdnListSupplierID" runat="server" value="" />
    <input type="hidden" id="hdnIM0131" runat="server" value="" />
    <div style="overflow-x: hidden;">
        <table class="tblContentArea">
            <colgroup>
                <col style="width: 50%" />
                <col style="width: 50%" />
            </colgroup>
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Tanggal Pesan") %>
                            </td>
                            <td style="padding-right: 1px; width: 140px">
                                <asp:TextBox ID="txtItemOrderDate" Width="120px" CssClass="datepicker" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Tanggal Pengiriman") %>
                            </td>
                            <td style="padding-right: 1px; width: 140px">
                                <asp:TextBox ID="txtItemOrderDeliveryDate" Width="120px" CssClass="datepicker" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Tanggal Expired") %>
                            </td>
                            <td style="padding-right: 1px; width: 140px">
                                <asp:TextBox ID="txtItemOrderExpiredDate" Width="120px" CssClass="datepicker" runat="server" />
                            </td>
                        </tr>
                        <tr id="trLocation" runat="server">
                            <td class="tdLabel">
                                <label class="lblMandatory lblLink" runat="server" id="lblLocation">
                                    <%=GetLabel("Dari Lokasi")%></label>
                            </td>
                            <td>
                                <input type="hidden" value="0" id="hdnLocationID" runat="server" />
                                <input type="hidden" value="0" id="hdnGCLocationGroup" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
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
                        <tr id="trProductLine" runat="server" style="display: none">
                            <td>
                                <label class="lblLink lblMandatory" runat="server" id="lblProductLine">
                                    <%=GetLabel("Product Line")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnProductLineID" value="" runat="server" />
                                <input type="hidden" id="hdnProductLineItemType" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtProductLineCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtProductLineName" Width="100%" runat="server" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="padding: 5px; vertical-align: top">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Jenis Persediaan")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboPurchaseOrderType" ClientInstanceName="cboPurchaseOrderType"
                                    Width="50%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Waktu Pembayaran")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboTerm" ClientInstanceName="cboTerm" Width="50%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Tipe Franco")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboFrancoRegion" ClientInstanceName="cboFrancoRegion" Width="50%"
                                    runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Display")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboFilter" ClientInstanceName="cboFilter" Width="50%" runat="server">
                                    <ClientSideEvents ValueChanged="function(s,e){ cbpView.PerformCallback('refresh'); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr style="display: none;">
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Mata Uang")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboCurrency" ClientInstanceName="cboCurrency" Width="50%" runat="server" />
                            </td>
                        </tr>
                        <tr style="display: none;">
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Nilai Kurs (Rp)") %></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtKurs" Width="50%" runat="server" />
                            </td>
                        </tr>
                        <%--<tr>
                            <td class="tdLabel">
                                <label>
                                    <%=GetLabel("Quick Filter")%></label>
                            </td>
                            <td>
                                <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView" ID="txtSearchView"
                                    Width="80%" Watermark="Search">
                                    <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                                    <IntellisenseHints>
                                        <qis:QISIntellisenseHint Text="Nama Item" FieldName="ItemName1" />
                                        <qis:QISIntellisenseHint Text="Nama Supplier" FieldName="BusinessPartnerName" />
                                    </IntellisenseHints>
                                </qis:QISIntellisenseTextBox>
                            </td>
                        </tr>--%>
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
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdService grdNormal notAllowSelect"
                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                        OnRowDataBound="grdView_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    <input id="chkSelectAll" type="checkbox" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ItemName1" HeaderText="Nama Barang" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-CssClass="tdItemName" />
                                            <asp:BoundField DataField="ItemUnit" HeaderText="Satuan" HeaderStyle-Width="100px"
                                                HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="QuantityMIN" HeaderText="Minimum" HeaderStyle-Width="100px"
                                                HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" />
                                            <asp:BoundField DataField="QuantityMAX" HeaderText="Maximum" HeaderStyle-Width="100px"
                                                HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" />
                                            <asp:BoundField DataField="QuantityEND" HeaderText="Stok Saat Ini" HeaderStyle-Width="100px"
                                                HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" />
                                            <asp:BoundField DataField="PurchaseOrderQtyOnOrder" HeaderText="Qty On Order" HeaderStyle-Width="100px"
                                                HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" />
                                            <asp:TemplateField ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Left" HeaderText="Qty Pesan"
                                                HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtPurchaseOrder" ReadOnly="true" Width="65%" runat="server" CssClass="number txtPurchaseOrder" Min="0" />
                                                    &nbsp;
                                                    <%#: Eval("ItemUnit")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Left" HeaderText="Supplier"
                                                HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <input type="hidden" value="<%#:Eval("ItemID") %>" class="hdnItemID" />
                                                    <input type="hidden" value="0" class="hdnSupplierID" id="hdnSupplierID" runat="server" />
                                                    <label runat="server" id="lblSupplier" class="lblSupplier">
                                                    </label>
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
    </div>
</asp:Content>
