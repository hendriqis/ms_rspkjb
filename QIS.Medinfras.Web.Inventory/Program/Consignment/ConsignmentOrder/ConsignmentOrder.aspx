<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx2.master"
    AutoEventWireup="true" CodeBehind="ConsignmentOrder.aspx.cs" Inherits="QIS.Medinfras.Web.Inventory.Program.ConsignmentOrder" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        var lastTransactionAmount = 0;
        var editedLineAmount = 0;

        $(function () {
            setRightPanelButtonEnabled();
        });

        function onLoad() {
            if ($('#<%=hdnIsAPConsignmentFromOrder.ClientID %>').val() == '0') { // PO -> POR
                $('#lblAddData').show();

                $('#lblAddDataFromReceive').hide();
                $('#lblAddDataFromReceivePerItem').hide();
            } else { // POR -> PO
                $('#lblAddData').hide();

                if ($('#<%=hdnIsEditable.ClientID %>').val() == '1') {
                    $('#lblAddDataFromReceive').show();
                    $('#lblAddDataFromReceivePerItem').show();
                }
                else {
                    $('#lblAddDataFromReceive').hide();
                    $('#lblAddDataFromReceivePerItem').hide();
                }
            }

            setDatePicker('<%=txtItemOrderDate.ClientID %>');

            setDatePicker('<%=txtItemOrderDeliveryDate.ClientID %>');

            setDatePicker('<%=txtItemOrderExpiredDate.ClientID %>');

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
                            $('#<%=hdnLocationItemGroupID.ClientID %>').val(result.ItemGroupID);
                            $('#<%=hdnGCLocationGroup.ClientID %>').val(result.GCLocationGroup);
                        });
                    }
                    else {
                        $('#<%=hdnLocationID.ClientID %>').val('');
                        $('#<%=txtLocationCode.ClientID %>').val('');
                        $('#<%=txtLocationName.ClientID %>').val('');
                        $('#<%=hdnLocationItemGroupID.ClientID %>').val('');
                    }
                });
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
                });
            }
            //#endregion

            //#region Order No
            $('#lblOrderNo.lblLink').click(function () {
                openSearchDialog('purchaseorderhd', "<%=GetFilterExpression() %>", function (value) {
                    $('#<%=txtOrderNo.ClientID %>').val(value);
                    onTxtOrderNoChanged(value);
                });
            });

            $('#<%=txtOrderNo.ClientID %>').change(function () {
                onTxtOrderNoChanged($(this).val());
            });

            function onTxtOrderNoChanged(value) {
                onLoadObject(value);
            }
            //#endregion

            //#region Supplier
            function getSupplierFilterExpression() {
                var filterExpression = "<%:filterExpressionSupplier %>";
                return filterExpression;
            }

            $('#<%=lblSupplier.ClientID %>.lblLink').live('click', function () {
                openSearchDialog('businesspartners', getSupplierFilterExpression(), function (value) {
                    $('#<%=txtSupplierCode.ClientID %>').val(value);
                    onTxtSupplierChanged(value);
                });
            });

            $('#<%=txtSupplierCode.ClientID %>').change(function () {
                onTxtSupplierChanged($(this).val());
            });

            function onTxtSupplierChanged(value) {
                var isEditablePPN = $('#<%=hdnIsPpnAllowChanged.ClientID %>').val();
                var filterExpression = getSupplierFilterExpression() + " AND BusinessPartnerCode = '" + value + "'";
                Methods.getObject('GetBusinessPartnersList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnSupplierID.ClientID %>').val(result.BusinessPartnerID);
                        $('#<%=txtSupplierName.ClientID %>').val(result.BusinessPartnerName);
                        if (result.IsTaxable) {
                            $('#<%=chkPPN.ClientID %>').attr('checked', true);
                            if (isEditablePPN == '1') {
                                $('#<%:txtVATPercentageDefault.ClientID %>').removeAttr('readonly');
                            }
                            else {
                                $('#<%:txtVATPercentageDefault.ClientID %>').attr('readonly', 'readonly');
                            }
                        }
                        else {
                            $('#<%=chkPPN.ClientID %>').attr('checked', false);
                        }
                        cboTerm.SetValue(result.TermID);
                    }
                    else {
                        $('#<%=hdnSupplierID.ClientID %>').val('');
                        $('#<%=txtSupplierCode.ClientID %>').val('');
                        $('#<%=txtSupplierName.ClientID %>').val('');
                        $('#<%=chkPPN.ClientID %>').attr('checked', false);
                        $('#<%:txtVATPercentageDefault.ClientID %>').attr('readonly', 'readonly');
                        cboTerm.SetSelectedIndex(0);
                    }
                });
            }
            //#endregion

            //#region Item Group
            function onGetItemGroupFilterExpression() {
                var ProductLineID = $('#<%=hdnProductLineID.ClientID %>').val();
                var GCItemType = $('#<%=hdnProductLineItemType.ClientID %>').val();
                var GCLocationGroup = $('#<%=hdnGCLocationGroup.ClientID %>').val();
                var filterExpression = "IsDeleted = 0";

                if ($('#<%=hdnLocationItemGroupID.ClientID %>').val() != '' && $('#<%=hdnLocationItemGroupID.ClientID %>').val() != '0') {
                    filterExpression += " AND ItemGroupID IN (SELECT ItemGroupID FROM vItemGroupMaster1 WHERE DisplayPath like '%/" + $('#<%=hdnLocationItemGroupID.ClientID %>').val() + "/%')";
                }

                if (ProductLineID != "" && ProductLineID != "0") {
                    filterExpression += " AND ItemGroupID IN (SELECT ItemGroupID FROM ItemMaster WHERE ProductLineID = " + ProductLineID + ")";
                } else {
                    if (GCLocationGroup != "") {
                        if (GCLocationGroup == Constant.LocationGroup.DRUGS_AND_MEDICALSUPPLIES) {
                            filterExpression += " AND GCItemType IN ('X001^002','X001^003')";
                        } else if (GCLocationGroup == Constant.LocationGroup.LOGISTICS) {
                            filterExpression += " AND GCItemType IN ('X001^008')";
                        } else {
                            filterExpression += " AND GCItemType IN ('X001^002','X001^003','X001^008')";
                        }
                    } else {
                        filterExpression += " AND GCItemType IN ('X001^002','X001^003','X001^008')";
                    }
                }

                return filterExpression;
            }

            $('#lblItemGroup.lblLink').live('click', function () {
                openSearchDialog('itemgroup', onGetItemGroupFilterExpression(), function (value) {
                    $('#<%=txtItemGroupCode.ClientID %>').val(value);
                    onTxtItemGroupCodeChanged(value);
                });
            });

            $('#<%=txtItemGroupCode.ClientID %>').live('change', function () {
                onTxtItemGroupCodeChanged($(this).val());
            });

            function onTxtItemGroupCodeChanged(value) {
                var filterExpression = onGetItemGroupFilterExpression() + " AND ItemGroupCode = '" + value + "'";
                $('#<%=txtItemCode.ClientID %>').val('');
                $('#<%=txtItemName.ClientID %>').val('');
                Methods.getObject('GetItemGroupMasterList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnItemGroupID.ClientID %>').val(result.ItemGroupID);
                        $('#<%=txtItemGroupName.ClientID %>').val(result.ItemGroupName1);
                    }
                    else {
                        $('#<%=hdnItemGroupID.ClientID %>').val('');
                        $('#<%=txtItemGroupCode.ClientID %>').val('');
                        $('#<%=txtItemGroupName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Item
            function getItemFilterExpression() {
                var ProductLineID = $('#<%=hdnProductLineID.ClientID %>').val();
                var GCItemType = $('#<%=hdnProductLineItemType.ClientID %>').val();
                var GCLocationGroup = $('#<%=hdnGCLocationGroup.ClientID %>').val();
                var filterExpression = "IsDeleted = 0 AND ItemID IN (SELECT ItemID FROM ItemProduct WHERE IsConsigmentItem = 1) AND GCItemStatus != 'X181^999'";
                var orderID = $('#<%=hdnOrderID.ClientID %>').val();

                if ($('#<%=txtItemGroupCode.ClientID %>').val() != '') {
                    filterExpression += " AND ItemGroupID IN (SELECT ItemGroupID FROM vItemGroupMaster1 WHERE DisplayPath like '%/" + $('#<%=hdnItemGroupID.ClientID %>').val() + "/%')";
                }
                else if ($('#<%=hdnLocationItemGroupID.ClientID %>').val() != '' && $('#<%=hdnLocationItemGroupID.ClientID %>').val() != '0') {
                    filterExpression += " AND ItemGroupID IN (SELECT ItemGroupID FROM vItemGroupMaster1 WHERE DisplayPath like '%/" + $('#<%=hdnLocationItemGroupID.ClientID %>').val() + "/%')";
                }

                if (orderID != '') {
                    filterExpression += " AND ItemID NOT IN (SELECT ItemID FROM PurchaseOrderDt WHERE PurchaseOrderID = " + orderID + " AND IsDeleted = 0)";
                }

                if (ProductLineID != "" && ProductLineID != "0") {
                    filterExpression += " AND ProductLineID = " + ProductLineID;
                } else {
                    if (GCLocationGroup != "") {
                        if (GCLocationGroup == Constant.LocationGroup.DRUGS_AND_MEDICALSUPPLIES) {
                            filterExpression += " AND GCItemType IN ('X001^002','X001^003')";
                        } else if (GCLocationGroup == Constant.LocationGroup.LOGISTICS) {
                            filterExpression += " AND GCItemType IN ('X001^008')";
                        } else {
                            filterExpression += " AND GCItemType IN ('X001^002','X001^003','X001^008')";
                        }
                    } else {
                        filterExpression += " AND GCItemType IN ('X001^002','X001^003','X001^008')";
                    }
                }

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
                var filterExpressionItemGroup = "ItemCode = '" + value + "'";
                Methods.getObject('GetvItemMasterList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnItemID.ClientID %>').val(result.ItemID);
                        $('#<%=txtItemName.ClientID %>').val(result.ItemName1);
                        Methods.getItemMasterPurchase(result.ItemID, $('#<%=hdnSupplierID.ClientID %>').val(), function (result2) {
                            if (result2 != null) {
                                $('#<%=hdnItemGroupID.ClientID %>').val(result2.ItemGroupID);
                                $('#<%=txtItemGroupCode.ClientID %>').val(result2.ItemGroupCode);
                                $('#<%=txtItemGroupName.ClientID %>').val(result2.ItemGroupName1);
                                $('#<%=txtSupplierItemCode.ClientID %>').val(result2.SupplierItemCode);
                                $('#<%=txtSupplierItemName.ClientID %>').val(result2.SupplierItemName);
                                $('#<%=txtDiscount.ClientID %>').val(result2.Discount);
                                $('#<%=hdnUnitPrice.ClientID %>').val(result2.Price);
                                $('#<%=hdnPurchaseUnitPrice.ClientID %>').val(result2.UnitPrice);
                                $('#<%=hdnGCBaseUnit.ClientID %>').val(result2.ItemUnit);
                                $('#<%=hdnGCItemUnit.ClientID %>').val(result2.PurchaseUnit);
                            }
                            else {
                                $('#<%=txtSupplierItemCode.ClientID %>').val('');
                                $('#<%=txtSupplierItemName.ClientID %>').val('');
                                $('#<%=txtDiscount.ClientID %>').val('0');
                                $('#<%=hdnUnitPrice.ClientID %>').val('0');
                                $('#<%=hdnPurchaseUnitPrice.ClientID %>').val('0');
                            }
                            cboItemUnit.PerformCallback();
                        });
                        Methods.getItemQtyOnOrder(result.ItemID, $('#<%=hdnLocationID.ClientID %>').val(), 3, function (result3) {
                            if (result3 != null)
                                $('#<%=txtQtyOnProcess.ClientID %>').val(result3.QtyOnOrder + " " + result.ItemUnit);
                            else
                                $('#<%=txtQtyOnProcess.ClientID %>').val("0 " + result.ItemUnit);
                            GetItemQtyFromLocation();
                        });
                    }
                    else {
                        $('#<%=hdnGCBaseUnit.ClientID %>').val('');
                        $('#<%=hdnItemID.ClientID %>').val('');
                        $('#<%=txtItemName.ClientID %>').val('');
                        $('#<%=txtStockLocation.ClientID %>').val('');
                    }
                });

            }
            //#endregion

            //#region Discount Detail
            $('#<%=chkIsDiscountInPercentage1.ClientID %>').die('change');
            $('#<%:chkIsDiscountInPercentage1.ClientID %>').live('change', function () {
                if ($(this).is(':checked')) {
                    $('#<%=txtDiscount.ClientID%>').removeAttr('readonly');
                    $('#<%=txtDiscountAmount.ClientID%>').attr('readonly', 'readonly');
                } else {
                    $('#<%=txtDiscount.ClientID%>').attr('readonly', 'readonly');
                    $('#<%=txtDiscountAmount.ClientID%>').removeAttr('readonly');
                }
            });

            $('#<%=txtDiscount.ClientID %>').die('change');
            $('#<%=txtDiscount.ClientID %>').live('change', function () {
                if ($(this).val() == '') {
                    $(this).val('0');
                }

                if ($('#<%:chkIsDiscountInPercentage1.ClientID %>').is(':checked')) {
                    var price = parseFloat($('#<%=txtPrice.ClientID %>').val().replace('.00', '').split(',').join(''));
                    var qty = parseFloat($('#<%=txtQuantity.ClientID %>').val().replace('.00', '').split(',').join(''));
                    var subTotal = parseFloat(price * qty);
                    var discountAmount = ((parseFloat($(this).val()) * subTotal) / 100);
                    $('#<%=hdnDiscountAmount1.ClientID %>').val(discountAmount);
                    $('#<%=txtDiscountAmount.ClientID %>').val(discountAmount).trigger('changeValue');
                } else {
                    $('#<%=hdnDiscountAmount1.ClientID %>').val("0");
                    $('#<%=txtDiscountAmount.ClientID %>').val(0).trigger('changeValue');
                }
                calculateSubTotal();
            });

            $('#<%=txtDiscountAmount.ClientID %>').die('change');
            $('#<%=txtDiscountAmount.ClientID %>').live('change', function () {
                if ($(this).val() == '') {
                    $(this).val('0');
                    $('#<%=hdnDiscountAmount2.ClientID %>').val(0);
                    $('#<%=txtDiscountAmount2.ClientID %>').val(0).trigger('changeValue');
                }

                if ($('#<%:chkIsDiscountInPercentage1.ClientID %>').is(':checked')) {
                } else {
                    var price = parseFloat($('#<%=txtPrice.ClientID %>').val().replace('.00', '').split(',').join(''));
                    var qty = parseFloat($('#<%=txtQuantity.ClientID %>').val().replace('.00', '').split(',').join(''));
                    var subTotal = parseFloat(price * qty);
                    var discountAmount = parseFloat($(this).val());
                    var discountPercent = parseFloat(discountAmount / subTotal * 100).toFixed(2);
                    $('#<%=hdnDiscountAmount1.ClientID %>').val(discountAmount);
                    $('#<%=txtDiscount.ClientID %>').val(discountPercent).trigger('changeValue');
                }
                calculateSubTotal();
            });

            $('#<%=chkIsDiscountInPercentage2.ClientID %>').die('change');
            $('#<%:chkIsDiscountInPercentage2.ClientID %>').live('change', function () {
                if ($(this).is(':checked')) {
                    $('#<%=txtDiscount2.ClientID%>').removeAttr('readonly');
                    $('#<%=txtDiscountAmount2.ClientID%>').attr('readonly', 'readonly');
                } else {
                    $('#<%=txtDiscount2.ClientID%>').attr('readonly', 'readonly');
                    $('#<%=txtDiscountAmount2.ClientID%>').removeAttr('readonly');
                }
            });

            $('#<%=txtDiscount2.ClientID %>').die('change');
            $('#<%=txtDiscount2.ClientID %>').live('change', function () {
                if ($(this).val() == '' || $('#<%=txtDiscount.ClientID %>').val() == 0 || $('#<%=txtDiscountAmount.ClientID %>').val() == 0) {
                    $(this).val('0');
                }

                var price = parseFloat($('#<%=txtPrice.ClientID %>').val().replace('.00', '').split(',').join(''));
                var qty = parseFloat($('#<%=txtQuantity.ClientID %>').val().replace('.00', '').split(',').join(''));
                var subTotal = parseFloat(price * qty);
                subTotal = subTotal - parseFloat($('#<%=txtDiscountAmount.ClientID %>').attr('hiddenVal')); ;
                var discountAmount2 = (parseFloat($(this).val()) * subTotal / 100);
                $('#<%=hdnDiscountAmount2.ClientID %>').val(discountAmount2);
                $('#<%=txtDiscountAmount2.ClientID %>').val(discountAmount2).trigger('changeValue');
                calculateSubTotal();
            });

            $('#<%=txtDiscountAmount2.ClientID %>').die('change');
            $('#<%=txtDiscountAmount2.ClientID %>').live('change', function () {
                if ($(this).val() == '' || $('#<%=txtDiscount.ClientID %>').val() == 0 || $('#<%=txtDiscountAmount.ClientID %>').val() == 0) {
                    $(this).val('0').trigger('changeValue');
                }

                if ($('#<%:chkIsDiscountInPercentage2.ClientID %>').is(':checked')) {
                } else {
                    var price = parseFloat($('#<%=txtPrice.ClientID %>').val().replace('.00', '').split(',').join(''));
                    var qty = parseFloat($('#<%=txtQuantity.ClientID %>').val().replace('.00', '').split(',').join(''));
                    var subTotal = parseFloat(price * qty);
                    var discountAmount = parseFloat($(this).val());
                    var discountPercent = parseFloat(discountAmount / subTotal * 100).toFixed(2);
                    $('#<%=hdnDiscountAmount2.ClientID %>').val(discountAmount);
                    $('#<%=txtDiscount2.ClientID %>').val(discountPercent).trigger('changeValue');
                }
                calculateSubTotal();
            });
            //#endregion

            $('#lblAddData').click(function (evt) {
                if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                    $('#<%=txtQuantity.ClientID %>').val('1');
                    $('#<%=hdnEntryID.ClientID %>').val('');
                    $('#<%=hdnItemID.ClientID %>').val('');
                    $('#<%=hdnGCItemUnit.ClientID %>').val('');
                    $('#<%=hdnGCBaseUnit.ClientID %>').val('');
                    $('#<%=txtItemGroupCode.ClientID %>').val('');
                    $('#<%=txtItemGroupName.ClientID %>').val('');
                    $('#<%=txtItemCode.ClientID %>').val('');
                    $('#<%=txtItemName.ClientID %>').val('');
                    $('#<%=hdnUnitPrice.ClientID %>').val('0');
                    $('#<%=hdnPurchaseUnitPrice.ClientID %>').val('0');
                    $('#<%=txtBaseUnit.ClientID %>').val('');
                    $('#<%=txtDiscount.ClientID %>').val('0');
                    $('#<%=txtDiscount2.ClientID %>').val('0');
                    $('#<%=txtSupplierItemCode.ClientID %>').val('');
                    $('#<%=txtSupplierItemName.ClientID %>').val('');
                    $('#<%=txtSubTotalPrice.ClientID %>').val('').trigger('changeValue');
                    $('#<%=lblSupplier.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%=txtSupplierCode.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=lblLocation.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%=txtLocationCode.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtStockLocation.ClientID %>').val('');
                    lastTransactionAmount = $('#<%=txtTotalOrder.ClientID %>').attr('hiddenVal');
                    editedLineAmount = 0;
                    cboItemUnit.SetValue('');
                    $('#<%=txtConversion.ClientID %>').val('');

                    $('#containerEntry').show();
                }
            });


            $('#lblAddDataFromReceive').click(function (evt) {
                if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                    $tr = $(this).closest('tr');
                    var param = $tr.find('.keyField').html();
                    var url = ResolveUrl("~/Program/Consignment/ConsignmentOrder/ConsignmentOrderAddFromReceiveCtl.ascx");
                    openUserControlPopup(url, param, 'Penerimaan Tanpa PO', 800, 500);
                }
            });

            $('#lblAddDataFromReceivePerItem').click(function (evt) {
                if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                    $tr = $(this).closest('tr');
                    var param = $tr.find('.keyField').html();
                    var url = ResolveUrl("~/Program/Consignment/ConsignmentOrder/ConsignmentOrderAddFromReceivePerItemCtl.ascx");
                    openUserControlPopup(url, param, 'Penerimaan Tanpa PO per Item', 800, 500);
                }
            });

            $('#btnCancel').click(function () {
                $('#containerEntry').hide();
                //cbpView.PerformCallback('refresh');
            });

            $('#btnSave').click(function (evt) {
                if (IsValid(evt, 'fsTrxPopup', 'mpTrxPopup'))
                    cbpProcess.PerformCallback('save');
            });

            $('#<%=txtQuantity.ClientID %>').die('change');
            $('#<%=txtQuantity.ClientID %>').live('change', function () {
                if ($(this).val() == '') {
                    $(this).val('1');
                }

                if ($('#<%:chkIsDiscountInPercentage1.ClientID %>').is(':checked')) {
                    $('#<%=txtDiscount.ClientID %>').trigger('change');
                } else {
                    $('#<%=txtDiscountAmount.ClientID %>').trigger('change');
                }

                if ($('#<%:chkIsDiscountInPercentage2.ClientID %>').is(':checked')) {
                    $('#<%=txtDiscount2.ClientID %>').trigger('change');
                } else {
                    $('#<%=txtDiscountAmount2.ClientID %>').trigger('change');
                }

                calculateSubTotal();
            });

            $('#<%=txtPrice.ClientID %>').die('change');
            $('#<%=txtPrice.ClientID %>').live('change', function () {
                $(this).blur();

                if ($('#<%:chkIsDiscountInPercentage1.ClientID %>').is(':checked')) {
                    $('#<%=txtDiscount.ClientID %>').trigger('change');
                } else {
                    $('#<%=txtDiscountAmount.ClientID %>').trigger('change');
                }

                if ($('#<%:chkIsDiscountInPercentage2.ClientID %>').is(':checked')) {
                    $('#<%=txtDiscount2.ClientID %>').trigger('change');
                } else {
                    $('#<%=txtDiscountAmount2.ClientID %>').trigger('change');
                }

                calculateSubTotal();
            });

            $('#<%=chkPPN.ClientID %>').change(function () {
                var isEditable = $('#<%=hdnIsPpnAllowChanged.ClientID %>').val();
                if ($(this).is(':checked')) {
                    if (isEditable == '1') {
                        $('#<%:txtVATPercentageDefault.ClientID %>').removeAttr('readonly');
                    }
                    else {
                        $('#<%:txtVATPercentageDefault.ClientID %>').attr('readonly', 'readonly');
                    }
                }
                else {
                    $('#<%:txtVATPercentageDefault.ClientID %>').attr('readonly', 'readonly');
                }
                calculateTotal();
            });

            $('#<%=txtVATPercentageDefault.ClientID %>').die('change');
            $('#<%=txtVATPercentageDefault.ClientID %>').live('change', function () {
                $('#<%:hdnVATPercentage.ClientID %>').val($('#<%=txtVATPercentageDefault.ClientID %>').val());
                $(this).trigger('changeValue');
                calculateTotal();
            });

            $('#<%=txtDP.ClientID %>').change(function () {
                $(this).trigger('changeValue');
                calculateTotal();
            });

            $('#<%=txtFinalDiscountInPercentage.ClientID %>').change(function () {
                $(this).trigger('changeValue');
                calculateFinalDiscount("fromPctg");
            });

            $('#<%=txtFinalDiscount.ClientID %>').change(function () {
                $(this).trigger('changeValue');
                calculateFinalDiscount("fromTxt");
            });

            function calculateFinalDiscount(kode) {
                var totalTrans = parseFloat($('#<%=txtTotalOrder.ClientID %>').attr('hiddenVal'));
                if (kode == "fromPctg") {
                    var finalDiscount = (parseFloat($('#<%=txtFinalDiscountInPercentage.ClientID %>').attr('hiddenVal')) / 100) * totalTrans;
                    $('#<%=txtFinalDiscount.ClientID %>').val(finalDiscount).trigger('changeValue');
                }
                else if (kode == "fromTxt") {
                    var finalDiscountInPercentage = (parseFloat($('#<%=txtFinalDiscount.ClientID %>').attr('hiddenVal')) / totalTrans) * 100;
                    $('#<%=txtFinalDiscountInPercentage.ClientID %>').val(finalDiscountInPercentage).trigger('changeValue');
                }
                calculateTotal();
            }

            $('.txtCurrency').each(function () {
                $(this).trigger('changeValue');
            });

            var pageCount = parseInt($('#<%=hdnPageCount.ClientID %>').val());
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
        }

        function GetItemQtyFromLocation() {
            var filterExpression = "LocationID = " + $('#<%=hdnLocationID.ClientID %>').val() + " AND ItemID = " + $('#<%=hdnItemID.ClientID %>').val() + " AND IsDeleted = 0";
            Methods.getObject('GetvItemBalanceList', filterExpression, function (result) {
                if (result != null)
                    $('#<%=txtStockLocation.ClientID %>').val(result.QuantityEND + ' ' + result.ItemUnit);
                else
                    $('#<%=txtStockLocation.ClientID %>').val('');
            });
        }

        //#region edit and delete
        $('#<%=grdView.ClientID %> .imgDelete.imgLink').live('click', function () {
            $row = $(this).closest('tr').parent().closest('tr');
            showToastConfirmation('Are You Sure Want To Delete?', function (result) {
                if (result) {
                    var entity = rowToObject($row);
                    $('#<%=hdnEntryID.ClientID %>').val(entity.ID);
                    cbpProcess.PerformCallback('delete');
                }
            });
        });

        $('#<%=grdView.ClientID %> .imgEdit.imgLink').live('click', function () {
            $row = $(this).closest('tr').parent().closest('tr');
            var entity = rowToObject($row);
            $('#<%=hdnEntryID.ClientID %>').val(entity.ID);
            $('#<%=hdnGCBaseUnit.ClientID %>').val(entity.GCBaseUnit);
            $('#<%=hdnGCItemUnit.ClientID %>').val(entity.GCPurchaseUnit);
            $('#<%=txtSupplierItemCode.ClientID %>').val(entity.SupplierItemCode);
            $('#<%=txtSupplierItemName.ClientID %>').val(entity.SupplierItemName);
            $('#<%=hdnUnitPrice.ClientID %>').val(parseFloat(entity.UnitPrice) / parseFloat(entity.ConversionFactor));
            $('#<%=hdnPurchaseUnitPrice.ClientID %>').val(parseFloat(entity.UnitPrice));

            var IsDiscountInPercentage1 = entity.IsDiscountInPercentage1;
            var IsDiscountInPercentage2 = entity.IsDiscountInPercentage2;
            if (IsDiscountInPercentage1 == 'False') {
                $('#<%=chkIsDiscountInPercentage1.ClientID %>').prop('checked', false);
                $('#<%=txtDiscount.ClientID%>').attr('readonly', 'readonly');
            } else {
                $('#<%=chkIsDiscountInPercentage1.ClientID %>').prop('checked', true);
                $('#<%=txtDiscountAmount.ClientID%>').attr('readonly', 'readonly');
            }
            if (IsDiscountInPercentage2 == 'False') {
                $('#<%=chkIsDiscountInPercentage2.ClientID %>').prop('checked', false);
                $('#<%=txtDiscount2.ClientID%>').attr('readonly', 'readonly');
            } else {
                $('#<%=chkIsDiscountInPercentage2.ClientID %>').prop('checked', true);
                $('#<%=txtDiscountAmount2.ClientID%>').attr('readonly', 'readonly');
            }

            $('#<%=txtDiscount.ClientID %>').val(entity.DiscountPercentage1).trigger('changeValue');
            $('#<%=hdnDiscountAmount1.ClientID %>').val(entity.DiscountAmount1);
            $('#<%=txtDiscountAmount.ClientID %>').val(entity.DiscountAmount1).trigger('changeValue');

            $('#<%=txtDiscount2.ClientID %>').val(entity.DiscountPercentage2).trigger('changeValue');
            $('#<%=hdnDiscountAmount2.ClientID %>').val(entity.DiscountAmount2);
            $('#<%=txtDiscountAmount2.ClientID %>').val(entity.DiscountAmount2).trigger('changeValue');

            $('#<%=hdnItemID.ClientID %>').val(entity.ItemID);
            $('#<%=txtItemCode.ClientID %>').val(entity.ItemCode);
            $('#<%=txtItemName.ClientID %>').val(entity.ItemName1);
            $('#<%=hdnItemGroupID.ClientID %>').val(entity.ItemGroupID);
            $('#<%=txtItemGroupCode.ClientID %>').val(entity.ItemGroupCode);
            $('#<%=txtItemGroupName.ClientID %>').val(entity.ItemGroupName1);
            $('#<%=txtQuantity.ClientID %>').val(entity.Quantity);
            $('#<%=hdnTempFactor.ClientID %>').val(entity.ConversionFactor);
            Methods.getItemQtyOnOrder(entity.ItemID, $('#<%=hdnLocationID.ClientID %>').val(), 3, function (result3) {
                if (result3 != null)
                    $('#<%=txtQtyOnProcess.ClientID %>').val((parseFloat(result3.QtyOnOrder) - parseFloat(entity.CustomTotal)) + " " + entity.BaseUnit);
                else
                    $('#<%=txtQtyOnProcess.ClientID %>').val("0 " + entity.BaseUnit);
                GetItemQtyFromLocation();
            });
            lastTransactionAmount = $('#<%=txtTotalOrder.ClientID %>').attr('hiddenVal');
            editedLineAmount = entity.CustomSubTotal;
            cboItemUnit.PerformCallback("edit");
            $('#containerEntry').show();
        });

        //#endregion

        function calculateTotal() {
            var totalKotor = parseFloat($('#<%=txtTotalOrder.ClientID %>').attr('hiddenVal'));
            var Discount = parseFloat($('#<%=txtFinalDiscount.ClientID %>').attr('hiddenVal'));
            if ($('#<%=chkPPN.ClientID %>').is(':checked')) {
                var temp = totalKotor - Discount;
                var PPN = parseFloat($('#<%=hdnVATPercentage.ClientID %>').val()) / 100 * parseFloat(temp);
                $('#<%=txtPPN.ClientID %>').val(PPN).trigger('changeValue');
            }
            else
                $('#<%=txtPPN.ClientID %>').val('0').trigger('changeValue');

            var PPN = parseFloat($('#<%=txtPPN.ClientID %>').attr('hiddenVal'));
            var DP = parseFloat($('#<%=txtDP.ClientID %>').attr('hiddenVal'));
            var totalHarga = totalKotor - (Discount + DP) + PPN;
            $('#<%=txtTotalOrderSaldo.ClientID %>').val(totalHarga).trigger('changeValue');
        }

        function calculateSubTotal() {
            var price = $('#<%=txtPrice.ClientID %>').attr('hiddenVal');
            var qty = $('#<%=txtQuantity.ClientID %>').val();
            var subTotal = price * qty;
            var subTotalTemp = 0;

            var odiscPercent = parseFloat($('#<%=txtDiscount.ClientID %>').val().replace('.00', '').split(',').join(''));
            var odiscAmount = parseFloat($('#<%=txtDiscountAmount.ClientID %>').val().replace('.00', '').split(',').join(''));
            var discount1 = 0;

            if ($('#<%:chkIsDiscountInPercentage1.ClientID %>').is(':checked')) {
                discount1 = parseFloat((odiscPercent * subTotal) / 100).toFixed(2);
            } else {
                discount1 = odiscAmount;
            }

            var subTotalTemp = subTotal - discount1;

            var odiscPercent2 = parseFloat($('#<%=txtDiscount2.ClientID %>').val().replace('.00', '').split(',').join(''));
            var odiscAmount2 = parseFloat($('#<%=txtDiscountAmount2.ClientID %>').val().replace('.00', '').split(',').join(''));
            var discount2 = 0;

            if ($('#<%:chkIsDiscountInPercentage2.ClientID %>').is(':checked')) {
                discount2 = parseFloat((odiscPercent2 * subTotalTemp) / 100).toFixed(2);
            } else {
                discount2 = odiscAmount2;
            }

            subTotal = subTotal - discount1 - discount2;
            $('#<%=txtSubTotalPrice.ClientID %>').val(subTotal).trigger('changeValue');

            calculateTotal();
        }

        //#region cboItemUnit
        function onCboItemUnitEndCallBack(s) {
            var param = s.cpResult.split('|');

            if ($('#<%=hdnGCItemUnit.ClientID %>').val() == '') {
                cboItemUnit.SetValue($('#<%=hdnGCBaseUnit.ClientID %>').val());
            }
            else {
                cboItemUnit.SetValue($('#<%=hdnGCItemUnit.ClientID %>').val());
            }

            onCboItemUnitChanged(param[0]);
        }

        function onCboItemUnitChanged(paramProcess) {
            var purchaseUnit = cboItemUnit.GetValue();
            var tempPricePerPurchaseUnit = 0;
            var baseValue = $('#<%=hdnGCBaseUnit.ClientID %>').val();
            var toUnitItem = cboItemUnit.GetValue();
            var baseText = getItemUnitName(baseValue);
            var factor = 1;
            var factorOld = $('#<%=hdnTempFactor.ClientID %>').val();
            var qtyBegin = $('#<%=txtQuantity.ClientID %>').val();
            var qtyEnd = parseFloat("1");
            //change price per unit
            var purchaseUnit = cboItemUnit.GetValue();
            Methods.getItemMasterPurchaseList($('#<%=hdnItemID.ClientID %>').val(), $('#<%=hdnSupplierID.ClientID %>').val(), function (result) {
                if (result.length > 0) {
                    for (i = 0; i < result.length; i++) {
                        if (result[i].ItemUnit == purchaseUnit) {
                            var conversion = "1 " + result[i].ItemUnitText + " = " + 1 + " " + result[i].ItemUnitText;
                            $('#<%=txtConversion.ClientID %>').val(conversion);
                            $('#<%=hdnConversionFactor.ClientID %>').val('1');
                            $('#<%=txtPrice.ClientID %>').val(result[i].Price).trigger('changeValue');
                            $('#<%=hdnPurchaseUnitPrice.ClientID %>').val(result[i].Price);
                            $('#<%=hdnGCBaseUnit.ClientID %>').val(result[i].ItemUnit);
                            $('#<%=hdnGCItemUnit.ClientID %>').val(result[i].PurchaseUnit);

                            if (paramProcess != "edit") {
                                $('#<%=txtDiscount.ClientID%>').removeAttr('readonly');
                                $('#<%=txtDiscountAmount.ClientID%>').attr('readonly', 'readonly');
                                $('#<%=txtDiscount2.ClientID%>').removeAttr('readonly');
                                $('#<%=txtDiscountAmount2.ClientID%>').attr('readonly', 'readonly');

                                $('#<%=txtDiscount.ClientID %>').val(result[i].Discount);
                                $('#<%=txtDiscount2.ClientID %>').val(result[i].Discount2);
                                $('#<%=txtDiscount.ClientID %>').trigger('change');
                                $('#<%=txtDiscount2.ClientID %>').trigger('change');
                            }
                        } else if (result[i].PurchaseUnit == purchaseUnit) {
                            var conversion = "1 " + result[i].PurchaseUnitText + " = " + result[i].ConversionFactor + " " + result[i].ItemUnitText;
                            $('#<%=txtConversion.ClientID %>').val(conversion);
                            $('#<%=hdnConversionFactor.ClientID %>').val(result[i].ConversionFactor);

                            $('#<%=txtPrice.ClientID %>').val(result[i].UnitPrice).trigger('changeValue');
                            $('#<%=hdnPurchaseUnitPrice.ClientID %>').val(result[i].UnitPrice);
                            $('#<%=hdnGCBaseUnit.ClientID %>').val(result[i].ItemUnit);
                            $('#<%=hdnGCItemUnit.ClientID %>').val(result[i].PurchaseUnit);

                            if (paramProcess != "edit") {
                                $('#<%=txtDiscount.ClientID%>').removeAttr('readonly');
                                $('#<%=txtDiscountAmount.ClientID%>').attr('readonly', 'readonly');
                                $('#<%=txtDiscount2.ClientID%>').removeAttr('readonly');
                                $('#<%=txtDiscountAmount2.ClientID%>').attr('readonly', 'readonly');

                                $('#<%=txtDiscount.ClientID %>').val(result[i].Discount);
                                $('#<%=txtDiscount2.ClientID %>').val(result[i].Discount2);
                                $('#<%=txtDiscount.ClientID %>').trigger('change');
                                $('#<%=txtDiscount2.ClientID %>').trigger('change');
                            }
                        } else {
                            //change conversion factor
                            $('#<%=txtBaseUnit.ClientID %>').val("per " + cboItemUnit.GetText());
                            if (baseValue == toUnitItem) {
                                $('#<%=hdnConversionFactor.ClientID %>').val('1');
                                var conversion = "1 " + baseText + " = 1 " + baseText;
                                $('#<%=txtConversion.ClientID %>').val(conversion);
                            }
                            else {
                                var itemID = $('#<%=hdnItemID.ClientID %>').val();
                                var filterExpression = "ItemID = " + itemID + " AND GCAlternateUnit = '" + toUnitItem + "'";
                                Methods.getObjectValue('GetvItemAlternateUnitList', filterExpression, 'ConversionFactor', function (result) {
                                    var toConversion = getItemUnitName(toUnitItem);
                                    $('#<%=hdnConversionFactor.ClientID %>').val(result);
                                    var conversion = "1 " + toConversion + " = " + result + " " + baseText;
                                    $('#<%=txtConversion.ClientID %>').val(conversion);
                                    factor = result;
                                });
                            }

                            //change price per unit
                            var value = cboItemUnit.GetValue();
                            var priceperitemunit = parseFloat(($('#<%=hdnUnitPrice.ClientID %>').val()));
                            var pricepurchaseunit = parseFloat(($('#<%=hdnPurchaseUnitPrice.ClientID %>').val()));
                            var pricePerPurchaseUnit = 0;
                            var convFactor = $('#<%=hdnConversionFactor.ClientID %>').val();

                            if (convFactor == "1") {
                                pricePerPurchaseUnit = priceperitemunit;
                            }
                            else if (purchaseUnit == toUnitItem) {
                                if (tempPricePerPurchaseUnit != 0) {
                                    pricePerPurchaseUnit = tempPricePerPurchaseUnit;
                                } else {
                                    if (pricepurchaseunit == priceperitemunit) {
                                        pricePerPurchaseUnit = pricepurchaseunit * convFactor;
                                        $('#<%=hdnPurchaseUnitPrice.ClientID %>').val(pricePerPurchaseUnit);
                                    } else {
                                        pricePerPurchaseUnit = pricepurchaseunit;
                                    }
                                }
                            }
                            else {
                                if (pricepurchaseunit == priceperitemunit) {
                                    pricePerPurchaseUnit = pricepurchaseunit * convFactor;
                                    $('#<%=hdnPurchaseUnitPrice.ClientID %>').val(pricePerPurchaseUnit);
                                } else {
                                    pricePerPurchaseUnit = priceperitemunit * convFactor;
                                    $('#<%=hdnPurchaseUnitPrice.ClientID %>').val(pricePerPurchaseUnit);
                                }
                            }

                            $('#<%=txtPrice.ClientID %>').val(pricePerPurchaseUnit).trigger('changeValue');
                            if (paramProcess != "edit") {
                                $('#<%=txtDiscount.ClientID %>').trigger('change');
                                $('#<%=txtDiscount2.ClientID %>').trigger('change');
                            }
                        }
                    }
                } else {
                    //change conversion factor
                    $('#<%=txtBaseUnit.ClientID %>').val("per " + cboItemUnit.GetText());
                    if (baseValue == toUnitItem) {
                        $('#<%=hdnConversionFactor.ClientID %>').val('1');
                        var conversion = "1 " + baseText + " = 1 " + baseText;
                        $('#<%=txtConversion.ClientID %>').val(conversion);
                    }
                    else {
                        var itemID = $('#<%=hdnItemID.ClientID %>').val();
                        var filterExpression = "ItemID = " + itemID + " AND GCAlternateUnit = '" + toUnitItem + "'";
                        Methods.getObjectValue('GetvItemAlternateUnitList', filterExpression, 'ConversionFactor', function (result) {
                            var toConversion = getItemUnitName(toUnitItem);
                            $('#<%=hdnConversionFactor.ClientID %>').val(result);
                            var conversion = "1 " + toConversion + " = " + result + " " + baseText;
                            $('#<%=txtConversion.ClientID %>').val(conversion);
                            factor = result;
                        });
                    }

                    //change price per unit
                    var value = cboItemUnit.GetValue();
                    var priceperitemunit = parseFloat(($('#<%=hdnUnitPrice.ClientID %>').val()));
                    var pricepurchaseunit = parseFloat(($('#<%=hdnPurchaseUnitPrice.ClientID %>').val()));
                    var pricePerPurchaseUnit = 0;
                    var convFactor = $('#<%=hdnConversionFactor.ClientID %>').val();

                    if (convFactor == "1") {
                        pricePerPurchaseUnit = priceperitemunit;
                    }
                    else if (purchaseUnit == toUnitItem) {
                        if (tempPricePerPurchaseUnit != 0) {
                            pricePerPurchaseUnit = tempPricePerPurchaseUnit;
                        } else {
                            if (pricepurchaseunit == priceperitemunit) {
                                pricePerPurchaseUnit = pricepurchaseunit * convFactor;
                                $('#<%=hdnPurchaseUnitPrice.ClientID %>').val(pricePerPurchaseUnit);
                            } else {
                                pricePerPurchaseUnit = pricepurchaseunit;
                            }
                        }
                    }
                    else {
                        if (pricepurchaseunit == priceperitemunit) {
                            pricePerPurchaseUnit = pricepurchaseunit * convFactor;
                            $('#<%=hdnPurchaseUnitPrice.ClientID %>').val(pricePerPurchaseUnit);
                        } else {
                            pricePerPurchaseUnit = priceperitemunit * convFactor;
                            $('#<%=hdnPurchaseUnitPrice.ClientID %>').val(pricePerPurchaseUnit);
                        }
                    }

                    $('#<%=txtPrice.ClientID %>').val(pricePerPurchaseUnit).trigger('changeValue');
                    if (paramProcess != "edit") {
                        $('#<%=txtDiscount.ClientID %>').trigger('change');
                        $('#<%=txtDiscount2.ClientID %>').trigger('change');
                    }
                }
            });

            $('#<%=txtBaseUnit.ClientID %>').val("per " + cboItemUnit.GetText());

            factor = $('#<%=hdnConversionFactor.ClientID %>').val();

            //change qty
            if (factor != 1) {
                if (factorOld == 1) {
                    qtyEnd = qtyBegin / factor;
                } else {
                    qtyEnd = qtyBegin;
                }
            } else {
                if (factorOld != 0) {
                    qtyEnd = qtyBegin * factorOld;
                } else {
                    qtyEnd = qtyBegin;
                }
            }
            $('#<%=txtQuantity.ClientID %>').val(parseFloat(qtyEnd).toFixed(2));

            $('#<%=hdnTempFactor.ClientID %>').val(factor);

            calculateSubTotal();
        }

        function getItemUnitName(baseValue) {
            var value = cboItemUnit.GetValue();
            cboItemUnit.SetValue(baseValue);
            var text = cboItemUnit.GetText();
            cboItemUnit.SetValue(value);
            return text;
        }
        //#endregion

        function onAfterSaveRecordDtSuccess(OrderID) {
            if ($('#<%=hdnOrderID.ClientID %>').val() == '0') {
                $('#<%=hdnOrderID.ClientID %>').val(OrderID);
                var filterExpression = 'PurchaseOrderID = ' + OrderID;
                Methods.getObject('GetPurchaseOrderHdList', filterExpression, function (result) {
                    $('#<%=txtOrderNo.ClientID %>').val(result.PurchaseOrderNo);
                });
                onAfterCustomSaveSuccess();
                setRightPanelButtonEnabled();
            }
        }

        function onCbpProcesEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'save') {
                if (param[1] == 'fail') {
                    showToast('Save Failed', 'Error Message : ' + param[2]);
                }
                else {
                    var OrderID = s.cpOrderID;
                    onAfterSaveRecordDtSuccess(OrderID);
                    $('#lblAddData').click();
                    cbpView.PerformCallback('refresh');
                }
            }
            else if (param[0] == 'delete') {
                if (param[1] == 'fail') {
                    showToast('Delete Failed', 'Error Message : ' + param[2]);
                }
                else {
                    cbpView.PerformCallback('refresh');
                }
            }
            $('#containerEntry').hide();
        }

        //#region Paging
        function onCbpViewEndCallback(s) {
            hideLoadingPanel();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                var totalPurchase = parseFloat(param[2]);
                $('#<%=txtTotalOrder.ClientID %>').val(totalPurchase).trigger('changeValue');
                calculateTotal();
                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
        }
        //#endregion

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var purchaseOrderID = $('#<%=hdnOrderID.ClientID %>').val();
            var printStatus = $('#<%=hdnPrintStatus.ClientID %>').val();
            if (printStatus == 'true') {
                if (purchaseOrderID == '' || purchaseOrderID == '0') {
                    errMessage.text = 'Please Set Transaction First!';
                    return false;
                }
                else {
                    filterExpression.text = "PurchaseOrderID = " + purchaseOrderID;
                    return true;
                }
            }
            else {
                errMessage.text = "Data is not approved or closed yet.";
                return false;
            }
        }

        function onAfterSaveAddRecordEntryPopup(param) {
            onAfterCustomSaveSuccess();
            onLoadObject(param);
        }

        function onBeforeLoadRightPanelContent(code) {
            if (code == 'infoReceived' || code == 'infoInvoice') {
                var param = $('#<%:hdnOrderID.ClientID %>').val();
                return param;
            }
            else {
                return $('#<%:hdnOrderID.ClientID %>').val();
            }
        }

        function setRightPanelButtonEnabled() {
            if ($('#<%:hdnOrderID.ClientID %>').val() == '' || $('#<%:hdnOrderID.ClientID %>').val() == '0') {
                $('#btnInfoPurchaseOrder').attr('enabled', 'false');
                $('#btnInvoiceInfo').attr('enabled', 'false');
            }
            else {
                $('#btnInfoPurchaseOrder').removeAttr('enabled');
                $('#btnInvoiceInfo').removeAttr('enabled');
            }
        }
    </script>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnIsAPConsignmentFromOrder" runat="server" />
    <input type="hidden" value="" id="hdnPrintStatus" runat="server" />
    <input type="hidden" value="" id="hdnVATPercentageFromSetvar" runat="server" />
    <input type="hidden" value="" id="hdnVATPercentage" runat="server" />
    <input type="hidden" value="" id="hdnOrderID" runat="server" />
    <input type="hidden" value="" id="hdnPageCount" runat="server" />
    <input type="hidden" value="1" id="hdnIsEditable" runat="server" />
    <input type="hidden" value="" id="hdnRecordFilterExpression" runat="server" />
    <input type="hidden" value="0" id="hdnIsUsedProductLine" runat="server" />
    <input type="hidden" value="0" id="hdnTempFactor" runat="server" />
    <input type="hidden" value="0" id="hdnIsPpnAllowChanged" runat="server" />
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
                                <label id="lblOrderNo" class="lblLink">
                                    <%=GetLabel("No. Pemesanan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtOrderNo" Width="150px" ReadOnly="true" runat="server" />
                            </td>
                        </tr>
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
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory lblLink" id="lblSupplier" runat="server">
                                    <%=GetLabel("Supplier/Penyedia")%></label>
                            </td>
                            <td>
                                <input type="hidden" value="" id="hdnSupplierID" runat="server" />
                                <table cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                        <col style="width: 250px" />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtSupplierCode" CssClass="required" ValidationGroup="mpEntry" Width="100%"
                                                runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSupplierName" ReadOnly="true" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="hdnLocation" runat="server">
                            <td class="tdLabel">
                                <label class="lblMandatory lblLink" runat="server" id="lblLocation">
                                    <%=GetLabel("Dari Lokasi")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnLocationID" value="" runat="server" />
                                <input type="hidden" id="hdnLocationItemGroupID" value="" runat="server" />
                                <input type="hidden" id="hdnGCLocationGroup" value="" runat="server" />
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
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Jenis Persediaan")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboPurchaseOrderType" ClientInstanceName="cboPurchaseOrderType"
                                    Width="45%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Waktu Pembayaran")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboTerm" ClientInstanceName="cboTerm" Width="45%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Tipe Franco")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboFrancoRegion" ClientInstanceName="cboFrancoRegion" Width="45%"
                                    runat="server" />
                            </td>
                        </tr>
                        <tr style="display: none">
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Mata Uang")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboCurrency" ClientInstanceName="cboCurrency" Width="45%" runat="server" />
                            </td>
                        </tr>
                        <tr style="display: none">
                            <td class="tdLabel">
                                <%=GetLabel("Nilai Kurs (Rp)") %>
                            </td>
                            <td>
                                <asp:TextBox ID="txtKurs" Width="45%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("No. Referensi") %>
                            </td>
                            <td>
                                <asp:TextBox ID="txtReferenceNo" Width="45%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("No. Penerimaan") %>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPurchaseReceiveNo" Width="100%" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div id="containerEntry" style="margin-top: 4px; display: none;">
                        <div class="pageTitle">
                            <%=GetLabel("Edit atau Tambah Item Pemesanan Barang")%></div>
                        <fieldset id="fsTrxPopup" style="margin: 0">
                            <input type="hidden" value="" id="hdnEntryID" runat="server" />
                            <table style="width: 100%" class="tblEntryDetail">
                                <colgroup>
                                    <col style="width: 50%" />
                                </colgroup>
                                <tr>
                                    <td valign="top">
                                        <table style="width: 100%">
                                            <colgroup>
                                                <col style="width: 100px" />
                                            </colgroup>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblLink" id="lblItemGroup">
                                                        <%=GetLabel("Kelompok Item")%></label>
                                                </td>
                                                <td>
                                                    <input type="hidden" value="" id="hdnItemGroupID" runat="server" />
                                                    <table cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width: 120px" />
                                                            <col style="width: 3px" />
                                                            <col style="width: 250px" />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtItemGroupCode" Width="100%" runat="server" />
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtItemGroupName" ReadOnly="true" Width="100%" runat="server" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblLink lblMandatory" id="lblItem">
                                                        <%=GetLabel("Item")%></label>
                                                </td>
                                                <td colspan="2">
                                                    <input type="hidden" value="" id="hdnItemID" runat="server" />
                                                    <input type="hidden" value="" id="hdnGCBaseUnit" runat="server" />
                                                    <input type="hidden" value="" id="hdnGCItemUnit" runat="server" />
                                                    <input type="hidden" value="" id="hdnConversionFactor" runat="server" />
                                                    <input type="hidden" value="" id="hdnUnitPrice" runat="server" />
                                                    <input type="hidden" value="" id="hdnPurchaseUnitPrice" runat="server" />
                                                    <table cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width: 120px" />
                                                            <col style="width: 3px" />
                                                            <col style="width: 250px" />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtItemCode" Width="100%" runat="server" />
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtItemName" ReadOnly="true" Width="100%" runat="server" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label>
                                                        <%=GetLabel("Stok")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtStockLocation" ReadOnly="true" CssClass="number" Width="120px"
                                                        runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblNormal" id="lblSupplierItem">
                                                        <%=GetLabel("Supplier Item")%></label>
                                                </td>
                                                <td colspan="2">
                                                    <table cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width: 120px" />
                                                            <col style="width: 3px" />
                                                            <col style="width: 250px" />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtSupplierItemCode" ReadOnly="true" Width="100%" runat="server" />
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtSupplierItemName" ReadOnly="true" Width="100%" runat="server" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("Jumlah")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtQuantity" Width="120px" CssClass="number" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("Satuan Item")%></label>
                                                </td>
                                                <td>
                                                    <dxe:ASPxComboBox runat="server" ID="cboItemUnit" ClientInstanceName="cboItemUnit"
                                                        Width="300px" OnCallback="cboItemUnit_Callback">
                                                        <ClientSideEvents EndCallback="function(s,e){ onCboItemUnitEndCallBack(s); }" ValueChanged="function(s,e){ onCboItemUnitChanged(); }" />
                                                    </dxe:ASPxComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("Konversi")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtConversion" Width="180px" runat="server" ReadOnly="true" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td valign="top">
                                        <table style="width: 100%">
                                            <colgroup>
                                                <col style="width: 150px" />
                                            </colgroup>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Harga")%></label>
                                                </td>
                                                <td>
                                                    <table cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width: 120px" />
                                                            <col style="width: 3px" />
                                                            <col style="width: 250px" />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtPrice" CssClass="txtCurrency" Width="100%" runat="server" />
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtBaseUnit" ReadOnly="true" Width="100%" runat="server" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Qty On Order")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtQtyOnProcess" ReadOnly="true" Width="180px" runat="server" Style="text-align: right" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Diskon 1 %")%></label>
                                                </td>
                                                <td>
                                                    <table cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width: 10%" />
                                                            <col style="width: 100px" />
                                                            <col style="width: 10px" />
                                                            <col />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <asp:CheckBox ID="chkIsDiscountInPercentage1" Checked="true" Width="100%" runat="server" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtDiscount" CssClass="number" Width="100%" runat="server" />
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <input type="hidden" value="0" id="hdnDiscountAmount1" runat="server" />
                                                                <asp:TextBox ID="txtDiscountAmount" CssClass="txtCurrency" Width="100%" runat="server" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Diskon 2 %")%></label>
                                                </td>
                                                <td>
                                                    <table cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width: 10%" />
                                                            <col style="width: 100px" />
                                                            <col style="width: 10px" />
                                                            <col />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <asp:CheckBox ID="chkIsDiscountInPercentage2" Checked="true" Width="100%" runat="server" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtDiscount2" CssClass="number" Width="100%" runat="server" />
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <input type="hidden" value="0" id="hdnDiscountAmount2" runat="server" />
                                                                <asp:TextBox ID="txtDiscountAmount2" CssClass="txtCurrency" Width="100%" runat="server" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Catatan")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtNotesDt" Width="100%" runat="server" TextMode="MultiLine" Rows="2" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Total Harga")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtSubTotalPrice" Width="180px" ReadOnly="true" runat="server" CssClass="txtCurrency" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table style="width: 100%">
                                            <tr>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <input type="button" id="btnSave" value='<%= GetLabel("Save")%>' />
                                                            </td>
                                                            <td>
                                                                <input type="button" id="btnCancel" value='<%= GetLabel("Cancel")%>' />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                    position: relative; font-size: 0.95em;">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdService grdNormal notAllowSelect"
                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <img class="imgEdit <%# IsEditable() == "0" || Eval("IsAllowEditItem").ToString() == "False" ? "imgDisabled" : "imgLink"%>"
                                                                    title='<%=GetLabel("Edit")%>' src='<%# IsEditable() == "0" || Eval("IsAllowEditItem").ToString() == "False" ? ResolveUrl("~/Libs/Images/Button/edit_disabled.png") : ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                    alt="" />
                                                            </td>
                                                            <td style="width: 1px">
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <img class="imgDelete <%# IsEditable() == "0" || Eval("IsAllowEditItem").ToString() == "False" ? "imgDisabled" : "imgLink"%>"
                                                                    title='<%=GetLabel("Delete")%>' src='<%# IsEditable() == "0" || Eval("IsAllowEditItem").ToString() == "False" ? ResolveUrl("~/Libs/Images/Button/delete_disabled.png") : ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                    alt="" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                    <input type="hidden" value="<%#:Eval("ItemID") %>" bindingfield="ItemID" />
                                                    <input type="hidden" value="<%#:Eval("ItemCode") %>" bindingfield="ItemCode" />
                                                    <input type="hidden" value="<%#:Eval("ItemName1") %>" bindingfield="ItemName1" />
                                                    <input type="hidden" value="<%#:Eval("ItemGroupID") %>" bindingfield="ItemGroupID" />
                                                    <input type="hidden" value="<%#:Eval("ItemGroupCode") %>" bindingfield="ItemGroupCode" />
                                                    <input type="hidden" value="<%#:Eval("ItemGroupName1") %>" bindingfield="ItemGroupName1" />
                                                    <input type="hidden" value="<%#:Eval("SupplierItemCode") %>" bindingfield="SupplierItemCode" />
                                                    <input type="hidden" value="<%#:Eval("SupplierItemName") %>" bindingfield="SupplierItemName" />
                                                    <input type="hidden" value="<%#:Eval("Quantity") %>" bindingfield="Quantity" />
                                                    <input type="hidden" value="<%#:Eval("GCPurchaseUnit") %>" bindingfield="GCPurchaseUnit" />
                                                    <input type="hidden" value="<%#:Eval("GCBaseUnit") %>" bindingfield="GCBaseUnit" />
                                                    <input type="hidden" value="<%#:Eval("PurchaseUnit") %>" bindingfield="PurchaseUnit" />
                                                    <input type="hidden" value="<%#:Eval("BaseUnit") %>" bindingfield="BaseUnit" />
                                                    <input type="hidden" value="<%#:Eval("UnitPrice") %>" bindingfield="UnitPrice" />
                                                    <input type="hidden" value="<%#:Eval("IsDiscountInPercentage1")%>" bindingfield="IsDiscountInPercentage1" />
                                                    <input type="hidden" value="<%#:Eval("IsDiscountInPercentage2")%>" bindingfield="IsDiscountInPercentage2" />
                                                    <input type="hidden" value="<%#:Eval("DiscountPercentage1") %>" bindingfield="DiscountPercentage1" />
                                                    <input type="hidden" value="<%#:Eval("DiscountPercentage2") %>" bindingfield="DiscountPercentage2" />
                                                    <input type="hidden" value="<%#:Eval("DiscountAmount1") %>" bindingfield="DiscountAmount1" />
                                                    <input type="hidden" value="<%#:Eval("DiscountAmount2") %>" bindingfield="DiscountAmount2" />
                                                    <input type="hidden" value="<%#:Eval("ConversionFactor") %>" bindingfield="ConversionFactor" />
                                                    <input type="hidden" value="<%#:Eval("GCItemDetailStatus") %>" bindingfield="GCItemDetailStatus" />
                                                    <input type="hidden" value="<%#:Eval("CustomSubTotal") %>" bindingfield="CustomSubTotal" />
                                                    <input type="hidden" value="<%#:Eval("CustomTotal") %>" bindingfield="CustomTotal" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Detail Barang" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <label style="font-weight: bold">
                                                        <%#:Eval("ItemName1") %></label>
                                                    <br />
                                                    <label style="font-style: italic">
                                                        <%#:Eval("ItemCode") %></label>
                                                    <br />
                                                    <label style="font-size: smaller">
                                                        <%#: IsAPConsignmentFromOrder() == "1" ? Eval("PurchaseReceiveNo") : GetLabel("") %></label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="CustomPurchaseUnit" HeaderText="Dipesan" HeaderStyle-Width="150px"
                                                HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="CustomUnitPrice" HeaderText="Harga / Satuan" HeaderStyle-Width="170px"
                                                HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="DiscountPercentage1" HeaderText="Disc1 (%)" HeaderStyle-Width="60px"
                                                ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" />
                                            <asp:BoundField DataField="DiscountAmount1" HeaderText="Disc1" HeaderStyle-Width="100px"
                                                ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" />
                                            <asp:BoundField DataField="DiscountPercentage2" HeaderText="Disc2 (%)" HeaderStyle-Width="60px"
                                                ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" />
                                            <asp:BoundField DataField="DiscountAmount2" HeaderText="Disc2" HeaderStyle-Width="100px"
                                                ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" />
                                            <asp:BoundField DataField="CustomConversion" HeaderText="Konversi" HeaderStyle-Width="200px"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="CustomSubTotal" HeaderText="Sub Total" HeaderStyle-Width="200px"
                                                HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" />
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
                    <div style="width: 100%; text-align: center">
                        <span class="lblLink" id="lblAddData" style="margin-right: 200px;">
                            <%= GetLabel("Add Data")%></span> <span class="lblLink" id="lblAddDataFromReceive"
                                style="margin-right: 200px;">
                                <%= GetLabel("Tambah Item dari Penerimaan")%></span> <span class="lblLink" id="lblAddDataFromReceivePerItem"
                                    style="margin-right: 200px;">
                                    <%= GetLabel("Tambah Item dari Penerimaan per Item")%></span>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div id="containerTotalOrder" style="margin-top: 20px;">
                        <fieldset id="fsTotalOrder" style="margin: 0">
                            <table style="width: 100%;">
                                <colgroup>
                                    <col style="width: 50%" />
                                    <col style="width: 40px" />
                                </colgroup>
                                <tr>
                                    <td valign="top">
                                        <table style="width: 100%;">
                                            <colgroup>
                                                <col style="width: 100px" />
                                            </colgroup>
                                            <tr>
                                                <td class="tdLabel" style="width: 120px; vertical-align: top; padding-top: 5px;">
                                                    <label class="lblNormal" id="lblPaymentRemarks">
                                                        <%=GetLabel("Syarat Pembayaran")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPaymentRemarks" Width="100%" runat="server" TextMode="MultiLine"
                                                        Rows="5" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel" style="width: 120px; vertical-align: top; padding-top: 5px;">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Catatan")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtNotes" Width="100%" runat="server" TextMode="MultiLine" Rows="2" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td valign="top">
                                        <table style="width: 100%;">
                                            <colgroup>
                                                <col style="width: 180px" />
                                            </colgroup>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Jumlah Nilai Pemesanan")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtTotalOrder" CssClass="txtCurrency" ReadOnly="true" Width="180px"
                                                        runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Diskon Final %")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtFinalDiscountInPercentage" CssClass="txtCurrency" Width="180px"
                                                        runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Diskon Final")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtFinalDiscount" CssClass="txtCurrency" Width="180px" runat="server"
                                                        hiddenVal="0" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <table style="width: 100%">
                                                        <colgroup>
                                                            <col style="width: 90%" />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <label class="lblNormal">
                                                                    <%=GetLabel("PPN")%>
                                                                    <asp:TextBox ID="txtVATPercentageDefault" CssClass="txtCurrency" ReadOnly="true"
                                                                        Width="40px" runat="server" />
                                                                    %</label>
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="chkPPN" Width="100%" runat="server" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPPN" CssClass="txtCurrency" ReadOnly="true" Width="180px" runat="server"
                                                        hiddenVal="0" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Uang Muka")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDP" CssClass="txtCurrency" Width="180px" runat="server" hiddenVal="0" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Saldo Nilai Pemesanan")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtTotalOrderSaldo" CssClass="txtCurrency" ReadOnly="true" Width="180px"
                                                        runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>
                    <div>
                        <table width="100%">
                            <tr>
                                <td>
                                    <div style="width: 600px;">
                                        <div class="pageTitle" style="text-align: center">
                                            <%=GetLabel("Informasi")%></div>
                                        <div style="background-color: #EAEAEA;">
                                            <table width="600px" cellpadding="0" cellspacing="0">
                                                <colgroup>
                                                    <col width="150px" />
                                                    <col width="30px" />
                                                </colgroup>
                                                <tr>
                                                    <td align="left">
                                                        <%=GetLabel("Dibuat Oleh") %>
                                                    </td>
                                                    <td align="center">
                                                        :
                                                    </td>
                                                    <td>
                                                        <div runat="server" id="divCreatedBy">
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <%=GetLabel("Dibuat Pada") %>
                                                    </td>
                                                    <td align="center">
                                                        :
                                                    </td>
                                                    <td>
                                                        <div runat="server" id="divCreatedDate">
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr id="trApprovedBy" runat="server">
                                                    <td align="left">
                                                        <%=GetLabel("Disetujui Oleh")%>
                                                    </td>
                                                    <td align="center">
                                                        :
                                                    </td>
                                                    <td>
                                                        <div runat="server" id="divApprovedBy">
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr id="trApprovedDate" runat="server">
                                                    <td align="left">
                                                        <%=GetLabel("Disetujui Pada")%>
                                                    </td>
                                                    <td align="center">
                                                        :
                                                    </td>
                                                    <td>
                                                        <div runat="server" id="divApprovedDate">
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <%=GetLabel("Terakhir Diubah Oleh") %>
                                                    </td>
                                                    <td align="center">
                                                        :
                                                    </td>
                                                    <td>
                                                        <div runat="server" id="divLastUpdatedBy">
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <%=GetLabel("Terakhir Diubah Pada")%>
                                                    </td>
                                                    <td align="center">
                                                        :
                                                    </td>
                                                    <td>
                                                        <div runat="server" id="divLastUpdatedDate">
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr id="trClosedBy" runat="server">
                                                    <td align="left">
                                                        <%=GetLabel("Ditutup Oleh")%>
                                                    </td>
                                                    <td align="center">
                                                        :
                                                    </td>
                                                    <td>
                                                        <div runat="server" id="divClosedBy">
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr id="trClosedDate" runat="server">
                                                    <td align="left">
                                                        <%=GetLabel("Ditutup Pada")%>
                                                    </td>
                                                    <td align="center">
                                                        :
                                                    </td>
                                                    <td>
                                                        <div runat="server" id="divClosedDate">
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr id="trClosedReason" runat="server">
                                                    <td align="left">
                                                        <%=GetLabel("Alasan Ditutup")%>
                                                    </td>
                                                    <td align="center">
                                                        :
                                                    </td>
                                                    <td>
                                                        <div runat="server" id="divClosedReason">
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <dxcp:ASPxCallbackPanel ID="cbpProcess" runat="server" Width="100%" ClientInstanceName="cbpProcess"
        ShowLoadingPanel="false" OnCallback="cbpProcess_Callback">
        <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpProcesEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</asp:Content>
