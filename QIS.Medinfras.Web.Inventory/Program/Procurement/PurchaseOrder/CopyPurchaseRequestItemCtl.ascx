<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CopyPurchaseRequestItemCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Inventory.Program.CopyPurchaseRequestItemCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_CopyPurchaseRequestItemCtl">

    function onBeforeSaveRecord(errMessage) {
        var result = false;
        getCheckedItem();
        if ($('#<%=hdnDataSave.ClientID %>').val() == '') {
            errMessage.text = 'Please Select Item First';
        }
        else {
            result = true;
        }
        return result;
    }

    function getCheckedItem() {
        var param = '';

        $('#<%=grdView.ClientID %> .chkIsSelected input').each(function () {
            if ($(this).is(':checked')) {
                var key = $(this).closest('tr').find('.keyField').html();
                var qty = parseFloat($(this).closest('tr').find('.txtQty').val());
                var tempMaxPO = parseFloat($(this).closest('tr').find('.hdnRemainingQtyBaseUnit').val());
                var amount = $(this).closest('tr').find('.txtUnitPrice').val();
                var GCPurchaseUnitORI = $(this).closest('tr').find('.hdnGCPurchaseUnitORI').val();
                var conversionFactorORI = $(this).closest('tr').find('.hdnConversionFactorORI').val();
                var GCPurchaseUnit = $(this).closest('tr').find('.hdnGCPurchaseUnit').val();
                var conversionFactor = $(this).closest('tr').find('.hdnConversionFactor').val();
                var disc1 = $(this).closest('tr').find('.txtDiscount1').val();
                var disc2 = $(this).closest('tr').find('.txtDiscount2').val();

                if (GCPurchaseUnitORI != GCPurchaseUnit) {
                    tempMaxPO = ((tempMaxPO * conversionFactorORI) / conversionFactor);
                }

                if (qty > tempMaxPO) {
                    if ($('#<%=hdnIsPOQtyCannotOverPRQty.ClientID %>').val() == "1") {
                        if (GCPurchaseUnitORI == GCPurchaseUnit) {
                            qty = parseFloat($tr.find('.txtQty').attr("max"));
                        }
                        else {
                            qty = ((parseFloat($tr.find('.txtQty').attr("max")) * conversionFactorORI) / conversionFactor);
                        }
                    } else {
                        qty = parseFloat($tr.find('.txtQty').val());
                    }
                }
                if (param == '') {
                    param = '$setData|' + key + '|' + qty + '|' + amount + '|' + GCPurchaseUnit + '|' + conversionFactor + '|' + disc1 + '|' + disc2;
                }
                else {
                    param += '$setData|' + key + '|' + qty + '|' + amount + '|' + GCPurchaseUnit + '|' + conversionFactor + '|' + disc1 + '|' + disc2;
                }
            }
        });
        $('#<%=hdnDataSave.ClientID %>').val(param);
    }

    $(function () {
        //#region PurchaseRequestNo
        function getFilterExp() {
            var locationID = $('#<%=hdnLocationIDCtl.ClientID %>').val();
            var productLineID = $('#<%=hdnProductLineIDCtl.ClientID %>').val();
            var purchaseOrderType = $('#<%=hdnPurchaseOrderTypeCtl.ClientID %>').val();
            var supplierID = $('#<%=hdnSupplierIDCtl.ClientID %>').val();

            var filter = "FromLocationID = '" + locationID + "' AND GCTransactionStatus = 'X121^003'";

            if ($('#<%=hdnIsUsedPurchaseOrderType.ClientID %>').val() == "1") {
                filter += " AND GCPurchaseOrderType = '" + purchaseOrderType + "'";
            }

            if (productLineID != "" && productLineID != "0") {
                filter += " AND ProductLineID = '" + productLineID + "'";
            }

            filter += " AND PurchaseRequestID IN (SELECT PurchaseRequestID FROM PurchaseRequestDt WITH(NOLOCK) WHERE GCItemDetailStatus = 'X121^003' AND OrderedQuantity < Quantity AND (BusinessPartnerID IS NULL OR BusinessPartnerID = " + supplierID + "))";
            
            return filter;
        }

        $('#lbPurchaseRequestNo.lblLink').click(function () {
            openSearchDialog('purchaserequesthd', getFilterExp(), function (value) {
                onTxtPurchaseRequestNoChanged(value);
            });
        });

        function onTxtPurchaseRequestNoChanged(value) {
            var filterExpression = getFilterExp() + " AND PurchaseRequestNo = '" + value + "'";
            Methods.getObject('GetvPurchaseRequestHdList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnPurchaseRequestID.ClientID %>').val(result.PurchaseRequestID);
                    $('#<%=txtPurhcaseRequestNo.ClientID %>').val(result.PurchaseRequestNo);
                    cbpPopup.PerformCallback('refresh');
                }
                else {
                    $('#<%=hdnPurchaseRequestID.ClientID %>').val('');
                    $('#<%=txtPurhcaseRequestNo.ClientID %>').val('');
                }
            });
        }
        //#endregion
    });

    function onCbpPopupEndCallback(s) {
        hideLoadingPanel();

        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            setPaging($("#pagingPopup"), pageCount, function (page) {
                cbpPopup.PerformCallback('changepage|' + page);
            });
        }
    }

    $('#chkSelectAll').die('change');
    $('#chkSelectAll').live('change', function () {
        var isChecked = $(this).is(":checked");
        $('.chkIsSelected input').each(function () {
            $(this).prop('checked', isChecked);
            $(this).change();
        });
    });

    $('#<%=grdView.ClientID %> .chkIsSelected input').die('change');
    $('#<%=grdView.ClientID %> .chkIsSelected input').live('change', function () {
        $tr = $(this).closest('tr');
        var id = $(this).closest('tr').find('.keyField').html();
        $lblPurchaseUnitCtl = $tr.find('.lblPurchaseUnitCtl');

        if ($(this).is(':checked')) {
            $tr.find('.txtQty').removeAttr('readonly');
            $tr.find('.txtUnitPrice').removeAttr('readonly');
            $tr.find('.txtDiscount1').removeAttr('readonly');
            $tr.find('.txtDiscount2').removeAttr('readonly');

            $lblPurchaseUnitCtl.removeClass('lblDisabled');
            $lblPurchaseUnitCtl.addClass('lblLink');
        }
        else {
            $tr.find('.txtQty').attr('readonly', 'readonly');
            $tr.find('.txtUnitPrice').attr('readonly', 'readonly');
            $tr.find('.txtDiscount1').attr('readonly', 'readonly');
            $tr.find('.txtDiscount2').attr('readonly', 'readonly');

            $lblPurchaseUnitCtl.removeClass('lblLink');
            $lblPurchaseUnitCtl.addClass('lblDisabled');
        }
    });

    var itemID = 0;
    var rowID = "";
    $('.lblPurchaseUnitCtl.lblLink').live('click', function () {
        $tr = $(this).closest('tr').parent().closest('tr');
        $td = $(this).parent();
        rowID = $td.attr('class');

        itemID = $(this).closest('tr').find('.keyField').html();
        var supplierID = $('#<%=hdnSupplierIDCtl.ClientID %>').val();
        var filter = getPurchaseUnitFilterExpression();
        var filterExpressionIP = "ItemID = " + itemID + " AND IsDeleted = 0 AND IsUsingSupplierCatalog = 1";
        var isUsingSupplierCatalog = 0;
        Methods.getObject('GetItemPlanningList', filterExpressionIP, function (result) {
            if (result != null) {
                isUsingSupplierCatalog = 1;
            }
        });

        var filterItem = "ItemID = " + itemID;
        var itemUnit = "";
        Methods.getObject('GetItemMasterList', filterItem, function (resultItem) {
            if (resultItem != null) {
                itemUnit = resultItem.GCItemUnit;
            }
        });

        if (isUsingSupplierCatalog == 1) {
            var lstUnit = "";

            var hdnIM0131 = $('#<%=hdnIM0131Ctl.ClientID %>').val();
            var orderDate = $('#<%=hdnOrderDateCtl.ClientID %>').val();
            if (hdnIM0131 == "0") {
                Methods.getItemMasterPurchaseList(itemID, supplierID, function (result1) {
                    if (result1.length > 0) {
                        for (i = 0; i < result1.length; i++) {
                            if (lstUnit == "") {
                                lstUnit = "'" + result1[i].PurchaseUnit + "'";
                            }
                            else {
                                lstUnit += ",'" + result1[i].PurchaseUnit + "'";
                            }
                        }
                    }
                });
            }
            else {
                Methods.getItemMasterPurchaseWithDateList(itemID, supplierID, orderDate, function (result1) {
                    if (result1.length > 0) {
                        for (i = 0; i < result1.length; i++) {
                            if (lstUnit == "") {
                                lstUnit = "'" + result1[i].PurchaseUnit + "'";
                            }
                            else {
                                lstUnit += ",'" + result1[i].PurchaseUnit + "'";
                            }
                        }
                    }
                });
            }

            if (lstUnit == "") {
                lstUnit = "'" + itemUnit + "'";
            }
            else {
                lstUnit += ",'" + itemUnit + "'";
            }

            filter += " AND GCAlternateUnit IN (" + lstUnit + ")";
        }

        openSearchDialog('itemalternateitemunit', filter, function (value) {
            onTxtPurchaseUnitChanged(value);
        });
    });

    //#region Purchase Unit
    function getPurchaseUnitFilterExpression() {
        var filterExpression = "IsDeleted = 0 AND IsActive = 1 AND ItemID = " + itemID;
        return filterExpression;
    }

    function onTxtPurchaseUnitChanged(value) {
        var conversionOld = $('.' + rowID).find('.hdnConversionFactor').val();
        var hdnGCPurchaseUnitOld = $('.' + rowID).find('.hdnGCPurchaseUnit').val();

        var filterExpression = getPurchaseUnitFilterExpression() + " AND GCAlternateUnit = '" + value + "'";
        Methods.getObject('GetvItemAlternateItemUnitList', filterExpression, function (result) {
            $lblPurchaseUnitCtlPrice = $td.parent().find('.lblPurchaseUnitCtlPrice');
            if (result != null) {
                if (hdnGCPurchaseUnitOld != result.GCAlternateUnit) {
                    var qtyBegin = parseFloat($('.' + rowID).find('.txtQty').val().replace(/,/g, ''));
                    var conversion = parseFloat(result.ConversionFactor);
                    var qtyEnd = 0;

                    var price = parseFloat($('.' + rowID).find('.txtUnitPrice').val().replace(/,/g, ''));
                    var totalPrice = 0;
                    var supplierID = $('#<%=hdnSupplierIDCtl.ClientID %>').val();
                    var isFound = 0;

                    var hdnIM0131 = $('#<%=hdnIM0131Ctl.ClientID %>').val();
                    var orderDate = $('#<%=hdnOrderDateCtl.ClientID %>').val();
                    if (hdnIM0131 == "0") {
                        Methods.getItemMasterPurchaseList(itemID, supplierID, function (result2) {
                            for (i = 0; i < result2.length; i++) {
                                if (result2[i].ItemUnit == value) {
                                    totalPrice = result2[i].Price;
                                    conversion = 1;
                                    isFound = 1;
                                } else if (result2[i].PurchaseUnit == value) {
                                    totalPrice = result2[i].UnitPrice;
                                    conversion = result2[i].ConversionFactor;
                                    isFound = 1;
                                }
                            }
                        });
                    }
                    else {
                        Methods.getItemMasterPurchaseWithDateList(itemID, supplierID, orderDate, function (result2) {
                            for (i = 0; i < result2.length; i++) {
                                if (result2[i].ItemUnit == value) {
                                    totalPrice = result2[i].Price;
                                    conversion = 1;
                                    isFound = 1;
                                } else if (result2[i].PurchaseUnit == value) {
                                    totalPrice = result2[i].UnitPrice;
                                    conversion = result2[i].ConversionFactor;
                                    isFound = 1;
                                }
                            }
                        });
                    }

                    $('.' + rowID).find('.hdnGCPurchaseUnit').val(result.GCAlternateUnit);
                    $('.' + rowID).find('.lblPurchaseUnitCtl').html(result.AlternateUnit);
                    $('.' + rowID).find('.hdnConversionFactor').val(result.ConversionFactor);
                    $lblPurchaseUnitCtlPrice.html(result.AlternateUnit);

                    if (conversion == 1) {
                        conversion = conversionOld;
                        qtyEnd = parseFloat(qtyBegin) * parseFloat(conversion);
                    } else if (conversionOld != 1) {
                        qtyEnd = parseFloat(qtyBegin) * parseFloat(conversionOld / conversion);
                    } else {
                        conversion = result.ConversionFactor;
                        qtyEnd = parseFloat(qtyBegin) / parseFloat(conversion);
                    }

                    $('.' + rowID).find('.txtQty').val(qtyEnd.toFixed(2)).trigger('changeValue');
                    if (isFound == 1) {
                        $('.' + rowID).find('.txtUnitPrice').val(totalPrice).trigger('changeValue');
                    }
                    else {
                        $('.' + rowID).find('.txtUnitPrice').val(price * conversion).trigger('changeValue');
                    }
                }
            }
            else {
                $('.' + rowID).find('.hdnGCPurchaseUnit').val('');
                $('.' + rowID).find('.lblPurchaseUnitCtl').html('');
                $('.' + rowID).find('.hdnConversionFactor').val('');
                $lblPurchaseUnitCtlPrice.html('');
            }
        });
    }
    //#endregion

    $('.txtQty').die('change');
    $('.txtQty').live('change', function () {
        var $tr = $(this).closest('tr');
        var qty = parseFloat($(this).val());

        var GCPurchaseUnitORI = $(this).closest('tr').find('.hdnGCPurchaseUnitORI').val();
        var conversionFactorORI = $(this).closest('tr').find('.hdnConversionFactorORI').val();
        var GCPurchaseUnit = $(this).closest('tr').find('.hdnGCPurchaseUnit').val();
        var conversionFactor = $(this).closest('tr').find('.hdnConversionFactor').val();
        var maxQty = parseFloat($(this).closest('tr').find('.hdnRemainingQtyBaseUnit').val());

        if (GCPurchaseUnitORI != GCPurchaseUnit) {
            maxQty = ((maxQty * conversionFactorORI) / conversionFactor);
        }

        if ($('#<%=hdnIsPOQtyCannotOverPRQty.ClientID %>').val() == "1") {
            if (qty > maxQty) {
                $tr.find('.txtQty').val(maxQty);
            }
            else if (qty < 0) {
                $tr.find('.txtQty').val(maxQty);
            }
        } else {
            $tr.find('.txtQty').val(qty);
        }
    });

    $('.txtDiscount1').die('change');
    $('.txtDiscount1').live('change', function () {
        var $tr = $(this).closest('tr');
        var disc1 = parseFloat($(this).val());

        if (disc1 > 100 || disc1 < 0) {
            $tr.find('.txtDiscount1').val('0.00');
        }
    });

    $('.txtDiscount2').die('change');
    $('.txtDiscount2').live('change', function () {
        var $tr = $(this).closest('tr');
        var disc2 = parseFloat($(this).val());

        if (disc2 > 100 || disc2 < 0) {
            $tr.find('.txtDiscount2').val('0.00');
        }
    });
</script>
<div style="padding: 10px;">
    <script id="tmplSelectedTestItem" type="text/x-jquery-tmpl">
    </script>
    <input type="hidden" value="0" id="hdnIsUsedPurchaseOrderType" runat="server" />
    <input type="hidden" id="hdnDataSave" value="" runat="server" />
    <input type="hidden" id="hdnPurchaseOrderIDCtl" runat="server" value="" />
    <input type="hidden" id="hdnSupplierIDCtl" runat="server" value="" />
    <input type="hidden" id="hdnLocationIDCtl" runat="server" value="" />
    <input type="hidden" id="hdnProductLineIDCtl" runat="server" value="" />
    <input type="hidden" id="hdnPurchaseOrderTypeCtl" runat="server" value="" />
    <input type="hidden" value="0" id="hdnIsPOQtyCannotOverPRQty" runat="server" />
    <input type="hidden" value="0" id="hdnIM0131Ctl" runat="server" />
    <input type="hidden" value="0" id="hdnOrderDateCtl" runat="server" />
    <table>
        <colgroup>
            <col style="width: 150px" />
            <col style="width: 400px" />
        </colgroup>
        <tr>
            <td class="tdLabel">
                <label class="lblLink lblMandatory" id="lbPurchaseRequestNo">
                    <%=GetLabel("Nomor Permintaan")%></label>
            </td>
            <td>
                <input type="hidden" id="hdnPurchaseRequestID" value="" runat="server" />
                <table style="width: 100%" cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col style="width: 50%" />
                        <col style="width: 3px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtPurhcaseRequestNo" Width="100%" runat="server" ReadOnly="true" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table style="width: 100%">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <div>
                    <dxcp:ASPxCallbackPanel ID="cbpPopup" runat="server" Width="100%" ClientInstanceName="cbpPopup"
                        ShowLoadingPanel="false" OnCallback="cbpPopup_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel();}" EndCallback="function(s,e){ onCbpPopupEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                    position: relative; font-size: 0.95em;">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdView grdNormal notAllowSelect"
                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                        OnRowDataBound="grdView_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="ItemID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    <input id="chkSelectAll" type="checkbox" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div class="<%#: Eval("ItemID")%>">
                                                        <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                                        <input type="hidden" class='hdnGCPurchaseUnit' id="hdnGCPurchaseUnit" runat="server"
                                                            value='<%#: Eval("GCPurchaseUnit")%>' />
                                                        <input type="hidden" class="hdnConversionFactor" id="hdnConversionFactor" value='<%#: Eval("ConversionFactor")%>'
                                                            runat="server" />
                                                        <input type="hidden" class='hdnGCPurchaseUnitORI' id="hdnGCPurchaseUnitORI" runat="server"
                                                            value='<%#: Eval("GCPurchaseUnit")%>' />
                                                        <input type="hidden" class="hdnConversionFactorORI" id="hdnConversionFactorORI" value='<%#: Eval("ConversionFactor")%>'
                                                            runat="server" />
                                                        <input type="hidden" class="hdnRemainingQtyBaseUnit" id="hdnRemainingQtyBaseUnit"
                                                            runat="server" value='<%#: Eval("cfRemainingQtyBaseUnit")%>' />
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ItemName1" HeaderText="Nama Barang" ItemStyle-CssClass="tdItemName1" />
                                            <asp:TemplateField HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    <div style="text-align: right; padding-left: 3px">
                                                        <%=GetLabel("Pesan")%>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div class="<%#: Eval("ItemID")%>">
                                                        <asp:TextBox ID="txtQty" class="txtQty number min" min="0" value="0" ReadOnly="true"
                                                            Style="width: 40px" runat="server" />
                                                        <label runat="server" id='lblPurchaseUnitCtl' class='lblPurchaseUnitCtl'>
                                                        </label>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    <div style="text-align: right; padding-left: 3px">
                                                        <%=GetLabel("Harga Satuan")%>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div class="<%#: Eval("ItemID")%>">
                                                        <asp:TextBox ID="txtUnitPrice" ValidationGroup="mpDrugsQuickPicks" class="txtUnitPrice number min"
                                                            min="0" value="0" ReadOnly="true" Style="width: 100px" runat="server" />
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    <div style="text-align: right; padding-left: 3px">
                                                        <%=GetLabel("Disc.1(%)")%>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div class="<%#: Eval("ItemID")%>">
                                                        <asp:TextBox ID="txtDiscount1" ValidationGroup="mpDrugsQuickPicks" class="txtDiscount1 number min"
                                                            min="0" value="0" ReadOnly="true" Style="width: 60px" runat="server" />
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    <div style="text-align: right; padding-left: 3px">
                                                        <%=GetLabel("Disc.2(%)")%>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div class="<%#: Eval("ItemID")%>">
                                                        <asp:TextBox ID="txtDiscount2" ValidationGroup="mpDrugsQuickPicks" class="txtDiscount2 number min"
                                                            min="0" value="0" ReadOnly="true" Style="width: 60px" runat="server" />
                                                    </div>
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
                </div>
            </td>
        </tr>
    </table>
    <div class="imgLoadingGrdView" id="containerImgLoadingView">
        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
    </div>
</div>
