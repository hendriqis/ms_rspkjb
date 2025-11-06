<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PurchaseReceiveDetailCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Inventory.Program.PurchaseReceiveDetailCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_PurchaseReceiveDetailCtl">
    function onRefreshGrid() {
        cbpViewPopup.PerformCallback("refresh");
    }

    //#region Order No
    function getPurchaseOrderExpression() {
        var filterExpression = $('#<%=hdnFilterExpressionPurchaseOrderCtl.ClientID %>').val();

        if (cboPurchaseOrderType.GetValue() != null && cboPurchaseOrderType.GetValue() != "") {
            filterExpression += " AND ISNULL(GCPurchaseOrderType,'') = ('" + cboPurchaseOrderType.GetValue() + "')";
        }

        return filterExpression;
    }

    $('#lblOrderNo.lblLink').click(function () {
        openSearchDialog('purchaseorderhd', getPurchaseOrderExpression(), function (value) {
            $('#<%=txtOrderNo.ClientID %>').val(value);
            onTxtOrderNoChanged(value);
        });
    });

    $('#<%=txtOrderNo.ClientID %>').change(function () {
        onTxtOrderNoChanged($(this).val());
    });

    function onTxtOrderNoChanged(value) {
        var filterExpressionBegin = $('#<%=hdnFilterExpressionPurchaseOrderCtl.ClientID %>').val();
        var filterExpression = filterExpressionBegin + " AND PurchaseOrderNo = '" + value + "'";
        Methods.getObject('GetPurchaseOrderHdList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnOrderID.ClientID %>').val(result.PurchaseOrderID);
                $('#<%=hdnTotalDP.ClientID %>').val(result.DownPaymentAmount);
                $('#<%=hdnTotalCharges.ClientID %>').val(result.ChargesAmount);
            }
            else {
                $('#<%=hdnOrderID.ClientID %>').val('');
                $('#<%=hdnTotalDP.ClientID %>').val(0);
                $('#<%=hdnTotalCharges.ClientID %>').val(0);
            }
            cbpViewPopup.PerformCallback("refresh");
        });
    }
    //#endregion

    function onCbpViewPopupEndCallback(s) {
        hideLoadingPanel();
        $('#<%=grdView.ClientID %> .txtCurrency').each(function () {
            $(this).trigger('changeValue');
        });

        $('.txtReceivedItem').each(function () {
            $(this).change();
        });

        $('#<%=grdView.ClientID %> tr:gt(0)').each(function () {
            $txtExpired = $(this).find('.txtExpired');
            if ($txtExpired != null) {
                setDatePickerElement($txtExpired);
                $txtExpired.val('<%=DateTimeNowDatePicker() %>');
            }
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

    $('.chkIsSelected input').die('change');
    $('.chkIsSelected input').live('change', function () {
        $('#<%=hdnIsFirstBinding.ClientID %>').val("0");

        var isAllowChangePriceAndDisc = $('#<%=hdnIsCopyPOAllowChangePriceAndDiscount.ClientID %>').val();

        $tr = $(this).closest('tr');

        var isDiscountPercentage = $tr.find('.hdnIsDiscountInPercentage1').val();
        var isDiscountPercentage2 = $tr.find('.hdnIsDiscountInPercentage2').val();

        if ($(this).is(':checked')) {
            if (isAllowChangePriceAndDisc == "1") {

                $tr.find('.txtUnitPrice').removeAttr('readonly');

                $tr.find('.chkIsDiscInPct1').removeAttr('style');
                if (isDiscountPercentage == "True") {
                    $tr.find('.txtDiscountPercentage1').removeAttr('readonly');
                    $tr.find('.txtDiscountAmount1').attr('readonly', 'readonly');
                }
                else {
                    $tr.find('.txtDiscountPercentage1').attr('readonly', 'readonly');
                    $tr.find('.txtDiscountAmount1').removeAttr('readonly');
                }

                $tr.find('.chkIsDiscInPct2').removeAttr('style');
                if (isDiscountPercentage2 == "True") {
                    $tr.find('.txtDiscountPercentage2').removeAttr('readonly');
                    $tr.find('.txtDiscountAmount2').attr('readonly', 'readonly');
                }
                else {
                    $tr.find('.txtDiscountPercentage2').attr('readonly', 'readonly');
                    $tr.find('.txtDiscountAmount2').removeAttr('readonly');
                }
            } else {
                $tr.find('.txtUnitPrice').attr('readonly', 'readonly');
                $tr.find('.chkIsDiscInPct1').attr('style', 'display:none');
                $tr.find('.txtDiscountPercentage1').attr('readonly', 'readonly');
                $tr.find('.txtDiscountAmount1').attr('readonly', 'readonly');
                $tr.find('.chkIsDiscInPct2').attr('style', 'display:none');
                $tr.find('.txtDiscountPercentage2').attr('readonly', 'readonly');
                $tr.find('.txtDiscountAmount2').attr('readonly', 'readonly');
            }

            $tr.find('.txtReceivedItem').removeAttr('readonly');
            $tr.find('.txtBatchNo').removeAttr('readonly');
            $tr.find('.txtRemarksDetailCtl').removeAttr('readonly');
            $tr.find('.txtExpired').removeAttr('readonly');
            $tr.find('.lblPurchaseUnit').addClass('lblLink');

            var txtExpired = $tr.find('.txtExpired');
            if (txtExpired != null) {
                setDatePickerElement(txtExpired);
                txtExpired.val('<%=DateTimeNowDatePicker() %>');
                txtExpired.removeAttr('readonly');
            }
        }
        else {
            $tr.find('.txtUnitPrice').attr('readonly', 'readonly');
            $tr.find('.chkIsDiscInPct1').attr('style', 'display:none');
            $tr.find('.txtDiscountPercentage1').attr('readonly', 'readonly');
            $tr.find('.txtDiscountAmount1').attr('readonly', 'readonly');
            $tr.find('.chkIsDiscInPct2').attr('style', 'display:none');
            $tr.find('.txtDiscountPercentage2').attr('readonly', 'readonly');
            $tr.find('.txtDiscountAmount2').attr('readonly', 'readonly');
            $tr.find('.txtReceivedItem').attr('readonly', 'readonly');
            $tr.find('.txtBatchNo').attr('readonly', 'readonly');
            $tr.find('.txtRemarksDetailCtl').attr('readonly', 'readonly');
            $tr.find('.txtExpired').attr('readonly', 'readonly');
            $tr.find('.lblPurchaseUnit').removeClass('lblLink');
        }
    });

    $('.txtReceivedItem').die('change');
    $('.txtReceivedItem').live('change', function () {
        var $tr = $(this).closest('tr');
        $tr.find('.txtDiscountPercentage1').change();
    });

    $('.txtUnitPrice').die('change');
    $('.txtUnitPrice').live('change', function () {
        $(this).blur();
        var $tr = $(this).closest('tr');
        $tr.find('.txtDiscountPercentage1').change();
    });

    $('.chkIsDiscInPct1 input').die('change');
    $('.chkIsDiscInPct1 input').live('change', function () {
        var isAllowChangePriceAndDisc = $('#<%=hdnIsCopyPOAllowChangePriceAndDiscount.ClientID %>').val();

        $tr = $(this).closest('tr');

        if ($(this).is(':checked')) {
            if (isAllowChangePriceAndDisc == "1") {
                $tr.find('.txtIsDiscInPct1').val("1");
                $tr.find('.txtDiscountPercentage1').removeAttr('readonly');
                $tr.find('.txtDiscountAmount1').attr('readonly', 'readonly');
            } else {
                $tr.find('.txtIsDiscInPct1').val("0");
                $tr.find('.txtDiscountPercentage1').attr('readonly', 'readonly');
                $tr.find('.txtDiscountAmount1').removeAttr('readonly');
            }
        }
        else {
            $tr.find('.txtIsDiscInPct1').val("0");
            $tr.find('.txtDiscountPercentage1').attr('readonly', 'readonly');
            $tr.find('.txtDiscountAmount1').removeAttr('readonly');
        }
    });

    $('.txtDiscountPercentage1').die('change');
    $('.txtDiscountPercentage1').live('change', function () {
        var $tr = $(this).closest('tr');
        var receivedItem = parseFloat($tr.find('.txtReceivedItem').val().replace('.00', '').split(',').join(''));
        var unitPrice = parseFloat($tr.find('.txtUnitPrice').val().replace('.00', '').split(',').join(''));
        var isDiscountPercentage1 = $tr.find('.hdnIsDiscountInPercentage1').val();
        var discountPercentage1 = parseFloat($tr.find('.txtDiscountPercentage1').val());
        var discountAmount1 = 0;

        if ($tr.find('.chkIsDiscInPct1').is(':checked') || $('#<%=hdnIsFirstBinding.ClientID %>').val() == "0") {
            discountAmount1 = parseFloat((receivedItem * unitPrice) * discountPercentage1 / 100).toFixed(2);
            $tr.find('.txtDiscountAmount1').val(discountAmount1).trigger('changeValue');
        }

        $tr.find('.txtDiscountPercentage2').change();
    });

    $('.txtDiscountAmount1').die('change');
    $('.txtDiscountAmount1').live('change', function () {
        var $tr = $(this).closest('tr');
        var receivedItem = parseFloat($tr.find('.txtReceivedItem').val().replace('.00', '').split(',').join(''));
        var unitPrice = parseFloat($tr.find('.txtUnitPrice').val().replace('.00', '').split(',').join(''));
        var isDiscountPercentage1 = $tr.find('.hdnIsDiscountInPercentage1').val();
        var discountPercentage1 = 0;
        var discountAmount1 = parseFloat($tr.find('.txtDiscountAmount1').val().replace('.00', '').split(',').join(''));

        if (!$tr.find('.chkIsDiscInPct1').is(':checked') || $('#<%=hdnIsFirstBinding.ClientID %>').val() == "0") {
            discountPercentage1 = parseFloat((discountAmount1 * 100) / (receivedItem * unitPrice)).toFixed(2);
            $tr.find('.txtDiscountPercentage1').val(discountPercentage1).trigger('changeValue');
        }

        $tr.find('.txtDiscountPercentage2').change();
    });

    $('.chkIsDiscInPct2 input').die('change');
    $('.chkIsDiscInPct2 input').live('change', function () {
        var isAllowChangePriceAndDisc = $('#<%=hdnIsCopyPOAllowChangePriceAndDiscount.ClientID %>').val();

        $tr = $(this).closest('tr');

        if ($(this).is(':checked')) {
            if (isAllowChangePriceAndDisc == "1") {
                $tr.find('.txtIsDiscInPct2').val("1");
                $tr.find('.txtDiscountPercentage2').removeAttr('readonly');
                $tr.find('.txtDiscountAmount2').attr('readonly', 'readonly');
            } else {
                $tr.find('.txtIsDiscInPct2').val("0");
                $tr.find('.txtDiscountPercentage2').attr('readonly', 'readonly');
                $tr.find('.txtDiscountAmount2').removeAttr('readonly');
            }
        }
        else {
            $tr.find('.txtIsDiscInPct2').val("0");
            $tr.find('.txtDiscountPercentage2').attr('readonly', 'readonly');
            $tr.find('.txtDiscountAmount2').removeAttr('readonly');
        }
    });

    $('.txtDiscountPercentage2').die('change');
    $('.txtDiscountPercentage2').live('change', function () {
        var $tr = $(this).closest('tr');
        var receivedItem = parseFloat($tr.find('.txtReceivedItem').val().replace('.00', '').split(',').join(''));
        var unitPrice = parseFloat($tr.find('.txtUnitPrice').val().replace('.00', '').split(',').join(''));
        var discountAmount1 = parseFloat($tr.find('.txtDiscountAmount1').val().replace('.00', '').split(',').join(''));
        var isDiscountPercentage2 = $tr.find('.hdnIsDiscountInPercentage2').val();
        var discountPercentage2 = parseFloat($tr.find('.txtDiscountPercentage2').val());
        var discountAmount2 = 0;

        if ($tr.find('.chkIsDiscInPct2').is(':checked') || $('#<%=hdnIsFirstBinding.ClientID %>').val() == "0") {
            discountAmount2 = parseFloat(((receivedItem * unitPrice) - discountAmount1) * discountPercentage2 / 100).toFixed(2);
            $tr.find('.txtDiscountAmount2').val(discountAmount2).trigger('changeValue');
        }

    });

    $('.txtDiscountAmount2').die('change');
    $('.txtDiscountAmount2').live('change', function () {
        var $tr = $(this).closest('tr');
        var receivedItem = parseFloat($tr.find('.txtReceivedItem').val().replace('.00', '').split(',').join(''));
        var unitPrice = parseFloat($tr.find('.txtUnitPrice').val().replace('.00', '').split(',').join(''));
        var discountAmount1 = parseFloat($tr.find('.txtDiscountAmount1').val().replace('.00', '').split(',').join(''));
        var discountPercentage2 = 0;
        var discountAmount2 = parseFloat($tr.find('.txtDiscountAmount2').val().replace('.00', '').split(',').join(''));

        if (!$tr.find('.chkIsDiscInPct2').is(':checked') || $('#<%=hdnIsFirstBinding.ClientID %>').val() == "0") {
            discountPercentage2 = parseFloat((discountAmount2 * 100) / ((receivedItem * unitPrice) - discountAmount1)).toFixed(2);
            $tr.find('.txtDiscountPercentage2').val(discountPercentage2).trigger('changeValue');
        }

    });

    function onBeforeSaveRecord(errMessage) {
        var count = 0;
        $('.chkIsSelected input').each(function () {
            if ($(this).is(':checked')) {
                count += 1;
            }
        });
        if (count == 0) {
            errMessage.text = 'Please Select Item First';
            return false;
        }
        return true;
    }

    //#region Purchase Unit
    function getPurchaseUnitFilterExpression() {
        var filterExpression = "IsDeleted = 0 AND IsActive = 1 AND ItemID = " + itemID;
        return filterExpression;
    }

    var itemID = 0;
    $tr = null;
    $('.lblPurchaseUnit.lblLink').live('click', function () {
        $tr = $(this).closest('tr');
        itemID = $tr.find('.hdnItemID').val();
        var orderDate = $tr.find('.hdnOrderDate').val();
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
            var orderDateInDatePicker = Methods.getDatePickerDate(orderDate);
            var orderDateFormatString = Methods.dateToString(orderDateInDatePicker);

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
                Methods.getItemMasterPurchaseWithDateList(itemID, supplierID, orderDateFormatString, function (result1) {
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

        openSearchDialog('itemalternateunit2', filter, function (value) {
            onTxtPurchaseUnitChanged(value);
        });
    });

    function onTxtPurchaseUnitChanged(value) {
        var conversionOld = parseFloat($tr.find('.hdnConversionFactor').val());

        var filterExpression = getPurchaseUnitFilterExpression() + " AND GCAlternateUnit = '" + value + "'";
        Methods.getObject('GetvItemAlternateUnit2List', filterExpression, function (result) {
            if (result != null) {
                var baseUnit = $tr.find('.hdnBaseUnit').val();

                $tr.find('.lblPurchaseUnit').html(result.AlternateUnit);
                $tr.find('.lblConversion').html("1 " + result.AlternateUnit + " = " + result.ConversionFactor + " " + baseUnit);
                $tr.find('.hdnGCPurchaseUnit').val(result.GCAlternateUnit);
                $tr.find('.hdnConversionFactor').val(result.ConversionFactor);

                var hdnGCPurchaseUnit = $tr.find('.hdnGCPurchaseUnit').val();
                var hdnConversionFactor = $tr.find('.hdnConversionFactor').val();

                var qtyBegin = parseFloat($tr.find('.txtReceivedItem').val().replace('.00', '').split(',').join(''));
                var conversion = parseFloat(result.ConversionFactor);

                var price = 0;
                if ($('#<%=hdnMenuType.ClientID %>').val() == "v2" && $('#<%=hdnIsPORWithPriceInformation.ClientID %>').val() == "0") {
                    price = $tr.find('.hdnUnitPrice').val();
                } else {
                    price = parseFloat($tr.find('.txtUnitPrice').val().replace('.00', '').split(',').join(''));
                }

                var qtyEnd = parseFloat((qtyBegin * conversionOld / conversion).toFixed(2));

                var totalPrice = parseFloat((price / conversionOld * conversion).toFixed(2));

                $tr.find('.txtReceivedItem').val(qtyEnd).trigger('changeValue');
                $tr.find('.txtUnitPrice').val(totalPrice).trigger('changeValue');
            }
            else {
                $tr.find('.hdnGCPurchaseUnit').val('');
                $tr.find('.lblPurchaseUnit').html('');
                $tr.find('.hdnConversionFactor').val('');
                $tr.find('.lblConversion').val('');
            }
        });
    }
    //#endregion

</script>
<input type="hidden" id="hdnSupplierIDCtl" value="" runat="server" />
<input type="hidden" id="hdnIsFilterPurchaseOrderNoCtl" runat="server" value="0" />
<input type="hidden" id="hdnSelectedMember" runat="server" value="" />
<input type="hidden" id="hdnTotalDP" runat="server" value="" />
<input type="hidden" id="hdnTotalCharges" runat="server" value="" />
<input type="hidden" id="hdnFilterExpressionPurchaseOrderCtl" runat="server" value="" />
<input type="hidden" id="hdnIsAllowPORQtyBiggerThanPO" runat="server" value="0" />
<input type="hidden" id="hdnIsUsedProductLine" runat="server" value="0" />
<input type="hidden" id="hdnProductLineIDCtl" runat="server" value="0" />
<input type="hidden" id="hdnLocationIDCtl" runat="server" value="0" />
<input type="hidden" id="hdnMenuType" runat="server" value="" />
<input type="hidden" id="hdnIsPORWithPriceInformation" runat="server" value="" />
<input type="hidden" id="hdnPPH" runat="server" value="0" />
<input type="hidden" id="hdnIsValidateVAT" runat="server" value="0" />
<input type="hidden" id="hdnIsAllowPORAfterPrintPO" runat="server" value="0" />
<input type="hidden" id="hdnIsCopyPOAllowChangePriceAndDiscount" runat="server" value="0" />
<input type="hidden" id="hdnIM0131Ctl" runat="server" value="" />
<input type="hidden" id="hdnIsFirstBinding" runat="server" value="1" />
<table class="tblContentArea">
    <tr>
        <td style="padding: 5px; vertical-align: top">
            <table class="tblEntryContent" style="width: 50%">
                <colgroup>
                    <col style="width: 30%" />
                    <col />
                </colgroup>
                <tr>
                    <td class="tdLabel">
                        <label class="lblNormal">
                            <%=GetLabel("Jenis Pemesanan")%></label>
                    </td>
                    <td>
                        <input type="hidden" id="hdnPurchaseOrderType" value="" runat="server" />
                        <dxe:ASPxComboBox ID="cboPurchaseOrderType" ClientInstanceName="cboPurchaseOrderType"
                            Width="150px" runat="server">
                            <ClientSideEvents ValueChanged="function(s,e) { onRefreshGrid(); }" />
                        </dxe:ASPxComboBox>
                    </td>
                </tr>
                <tr>
                    <td class="tdLabel">
                        <label class="lblLink" id="lblOrderNo">
                            <%=GetLabel("No. Pemesanan")%></label>
                    </td>
                    <td>
                        <input type="hidden" id="hdnOrderID" value="" runat="server" />
                        <asp:TextBox ID="txtOrderNo" Width="150px" runat="server" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
            <dxcp:ASPxCallbackPanel ID="cbpViewPopup" runat="server" Width="100%" ClientInstanceName="cbpViewPopup"
                ShowLoadingPanel="false" OnCallback="cbpViewPopup_Callback">
                <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewPopupEndCallback(s); }" />
                <PanelCollection>
                    <dx:PanelContent ID="PanelContent1" runat="server">
                        <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                            position: relative; font-size: 0.90em; max-height: 300px; overflow-y: scroll;">
                            <asp:GridView ID="grdView" runat="server" CssClass="grdService grdNormal notAllowSelect"
                                AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                OnRowDataBound="grdView_RowDataBound">
                                <Columns>
                                    <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                    <asp:TemplateField HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                        <HeaderTemplate>
                                            <input id="chkSelectAll" type="checkbox" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                            <input type="hidden" class="keyField" id="keyField" runat="server" value='<%#: Eval("ID")%>' />
                                            <input type="hidden" id="hdnPOHdID" runat="server" value='<%#: Eval("PurchaseOrderID")%>' />
                                            <input type="hidden" id="hdnItemID" class="hdnItemID" runat="server" value='<%#: Eval("ItemID")%>' />
                                            <input type="hidden" id="hdnGCPurchaseUnitORI" class="hdnGCPurchaseUnitORI" runat="server"
                                                value='<%#: Eval("GCPurchaseUnit")%>' />
                                            <input type="hidden" id="hdnGCPurchaseUnit" class="hdnGCPurchaseUnit" runat="server"
                                                value='<%#: Eval("GCPurchaseUnit")%>' />
                                            <input type="hidden" id="hdnReceivedQuantity" class="hdnReceivedQuantity" runat="server"
                                                value='<%#: Eval("ReceivedQuantity")%>' />
                                            <input type="hidden" id="hdnQuantity" class="hdnQuantity" runat="server" value='<%#: Eval("DraftQuantity")%>' />
                                            <input type="hidden" id="hdnUnitPrice" class="hdnUnitPrice" runat="server" value='<%#: Eval("DraftUnitPrice")%>' />
                                            <input type="hidden" id="hdnIsDiscountInPercentage1" class="hdnIsDiscountInPercentage1"
                                                runat="server" value='<%#: Eval("IsDiscountInPercentage1")%>' />
                                            <input type="hidden" id="hdnDisc1Pct" class="hdnDisc1Pct" runat="server" value='<%#: Eval("DraftDiscountPercentage1")%>' />
                                            <input type="hidden" id="hdnDisc1" class="hdnDisc1" runat="server" value='<%#: Eval("DraftDiscountAmount1")%>' />
                                            <input type="hidden" id="hdnIsDiscountInPercentage2" class="hdnIsDiscountInPercentage2"
                                                runat="server" value='<%#: Eval("IsDiscountInPercentage2")%>' />
                                            <input type="hidden" id="hdnDisc2Pct" class="hdnDisc2Pct" runat="server" value='<%#: Eval("DraftDiscountPercentage2")%>' />
                                            <input type="hidden" id="hdnDisc2" class="hdnDisc2" runat="server" value='<%#: Eval("DraftDiscountAmount2")%>' />
                                            <input type="hidden" id="hdnDefaultConversionFactor" class="hdnDefaultConversionFactor"
                                                runat="server" value='<%#: Eval("ConversionFactor")%>' />
                                            <input type="hidden" id="hdnConversionFactor" class="hdnConversionFactor" runat="server"
                                                value='<%#: Eval("ConversionFactor")%>' />
                                            <input type="hidden" id="hdnBaseUnit" class="hdnBaseUnit" runat="server" value='<%#: Eval("BaseUnit")%>' />
                                            <input type="hidden" id="hdnItemName1" class="hdnItemName1" runat="server" value='<%#: Eval("ItemName1")%>' />
                                            <input type="hidden" id="hdnOrderDate" class="hdnOrderDate" runat="server" value='<%#: Eval("OrderDate")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Item" HeaderStyle-Width="250px" ItemStyle-HorizontalAlign="Left"
                                        HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <div style="padding: 1px">
                                                <div>
                                                    <span style="font-style: normal; font-weight: bold">
                                                        <%#: Eval("ItemName1")%></div>
                                                <div>
                                                    <span style="font-style: normal; font-size: smaller">
                                                        <%#: Eval("PurchaseOrderNo")%>
                                                        (<%#: Eval("OrderDateInString")%>)</span></div>
                                                <div>
                                                    <span style="font-style: italic">
                                                        <%#: Eval("CustomConversion")%></span></div>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="CustomQtyRemaining" HeaderText="Sisa" HeaderStyle-Width="50px"
                                        HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                                    <asp:TemplateField HeaderStyle-Width="70px" HeaderText="Diterima" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtReceivedItem" ReadOnly="true" Width="99%" value="0" runat="server"
                                                min="0" CssClass="number txtReceivedItem" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="60px" HeaderText="Satuan" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <label runat="server" id="lblPurchaseUnit" class="lblPurchaseUnit">
                                            </label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="90px" HeaderText="Harga / Satuan" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtUnitPrice" ReadOnly="true" Width="99%" runat="server" CssClass="txtCurrency txtUnitPrice" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="80px" HeaderText="Disc1(%)" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkIsDiscInPct1" runat="server" CssClass="chkIsDiscInPct1" style="display:none" />
                                            <asp:TextBox ID="txtIsDiscInPct1" ReadOnly="true" Width="99%" runat="server" Text="0" style="display:none"
                                                CssClass="txtIsDiscInPct1" />
                                            <asp:TextBox ID="txtDiscountPercentage1" ReadOnly="true" Width="60%" runat="server"
                                                Text="0" CssClass="number txtDiscountPercentage1" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="80px" HeaderText="Disc1 (Rp)" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDiscountAmount1" ReadOnly="true" Width="99%" runat="server" Text="0"
                                                CssClass="txtCurrency txtDiscountAmount1" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="80px" HeaderText="Disc2(%)" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkIsDiscInPct2" runat="server" CssClass="chkIsDiscInPct2" style="display:none" />
                                            <asp:TextBox ID="txtIsDiscInPct2" ReadOnly="true" Width="99%" runat="server" Text="0" style="display:none"
                                                CssClass="txtIsDiscInPct2" />
                                            <asp:TextBox ID="txtDiscountPercentage2" ReadOnly="true" Width="60%" runat="server"
                                                Text="0" CssClass="number txtDiscountPercentage2" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="80px" HeaderText="Disc2 (Rp)" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDiscountAmount2" ReadOnly="true" Width="99%" runat="server" Text="0"
                                                CssClass="txtCurrency txtDiscountAmount2" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="80px" Visible="false" HeaderText="Serial No"
                                        ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtSerialNo" ReadOnly="true" Width="50%" value="0" runat="server"
                                                CssClass="number txtSerialNo" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="70px" HeaderText="No Batch" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtBatchNo" ReadOnly="true" Width="100%" value="" runat="server"
                                                CssClass="txtBatchNo" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="90px" HeaderText="Remarks" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtRemarksDetailCtl" ReadOnly="true" Width="100%" TextMode="MultiLine"
                                                Rows="3" value="" runat="server" CssClass="txtRemarksDetailCtl" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="110px" HeaderText="Expired Date" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <div style='<%#: Eval("IsControlExpired").ToString() == "False" ? "display:none" : ""%>'>
                                                <asp:TextBox ID="txtExpired" ReadOnly="true" Width="80%" value="" runat="server"
                                                    CssClass="txtExpired datepicker" />
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="40px" HeaderText="PPN" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkIsPPN" runat="server" Enabled="false" CssClass="chkIsAsset" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="40px" HeaderText="Asset" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkIsAsset" runat="server" Enabled="false" CssClass="chkIsAsset" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <%=GetLabel("Tidak ada data pemesanan yang masih outstanding")%>
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </asp:Panel>
                    </dx:PanelContent>
                </PanelCollection>
            </dxcp:ASPxCallbackPanel>
            <div class="imgLoadingGrdView" id="containerImgLoadingView">
                <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
            </div>
        </td>
    </tr>
</table>
