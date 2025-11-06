<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx2.master"
    AutoEventWireup="true" CodeBehind="ConsignmentReceive.aspx.cs" Inherits="QIS.Medinfras.Web.Inventory.Program.ConsignmentReceive" %>

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
                $('#lblAddData').hide();
                $('#lblQuickPick').hide();

                if (isShowWatermark()) {
                    $('#btnPurchaseReceive').attr('enabled', false);
                }
                else {
                    $('#btnPurchaseReceive').attr('enabled', true);
                }
            } else { // POR -> PO
                if ($('#<%=hdnIsEditable.ClientID %>').val() == '1') {
                    $('#lblAddData').show();
                    $('#lblQuickPick').show();
                }
                else {
                    $('#lblAddData').hide();
                    $('#lblQuickPick').hide();
                }

                $('#btnPurchaseReceive').attr('enabled', false);
            }

            $('#btnPurchaseReceive').click(function () {
                if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
                    if ($('#<%=hdnIsAPConsignmentFromOrder.ClientID %>').val() == '0' && !isShowWatermark()) {
                        var param = $('#<%=hdnSupplierID.ClientID %>').val() + '|' + $('#<%=hdnProductLineID.ClientID %>').val();
                        var url = ResolveUrl("~/Program/Consignment/ConsignmentReceive/ConsignmentReceiveDetailCtl.ascx");
                        openUserControlPopup(url, param, 'Penerimaan Pembelian Konsinyasi - Daftar Item', 1200, 550);
                    }
                }
            });

            setDatePicker('<%=txtPurchaseReceiveDate.ClientID %>');
            $('#<%=txtPurchaseReceiveDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

            setDatePicker('<%=txtDateReferrence.ClientID %>');
            $('#<%=txtDateReferrence.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

            $('.txtCurrency').each(function () {
                $(this).trigger('changeValue');
            });

            calculateTotal();

            //#region Supplier
            function getSupplierFilterExpression() {
                var filterExpression = "<%:filterExpressionSupplier %>";
                return filterExpression;
            }

            $('#<%=lblSupplier.ClientID %>.lblLink').click(function () {
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
                            $('#<%:txtVATPercentageDefault.ClientID %>').attr('readonly', 'readonly');
                        }
                        cboTerm.SetValue(result.TermID);
                    }
                    else {
                        $('#<%=hdnSupplierID.ClientID %>').val('');
                        $('#<%=txtSupplierCode.ClientID %>').val('');
                        $('#<%=txtSupplierName.ClientID %>').val('');
                        $('#<%=chkPPN.ClientID %>;').attr('checked', false);
                        $('#<%:txtVATPercentageDefault.ClientID %>').attr('readonly', 'readonly');
                        cboTerm.SetValue('');
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

            //#region Location
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

                var receiveID = $('#<%=hdnPRID.ClientID %>').val();
                if ($('#<%=txtItemGroupCode.ClientID %>').val() != '') {
                    filterExpression += " AND ItemGroupID IN (SELECT ItemGroupID FROM vItemGroupMaster WHERE DisplayPath like '%/" + $('#<%=hdnItemGroupID.ClientID %>').val() + "/%')";
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
                        $('#<%=hdnGCBaseUnit.ClientID %>').val(result.GCItemUnit);
                        $('#<%=hdnItemGroupID.ClientID %>').val(result.ItemGroupID);
                        $('#<%=txtItemGroupCode.ClientID %>').val(result.ItemGroupCode);
                        $('#<%=txtItemGroupName.ClientID %>').val(result.ItemGroupName1);

                        Methods.getItemMasterPurchase(result.ItemID, $('#<%=hdnSupplierID.ClientID %>').val(), function (result2) {
                            if (result2 != null) {
                                $('#<%=hdnItemGroupID.ClientID %>').val(result2.ItemGroupID);
                                $('#<%=txtItemGroupCode.ClientID %>').val(result2.ItemGroupCode);
                                $('#<%=txtItemGroupName.ClientID %>').val(result2.ItemGroupName1);
                                $('#<%=txtSupplierItemCode.ClientID %>').val(result2.SupplierItemCode);
                                $('#<%=txtSupplierItemName.ClientID %>').val(result2.SupplierItemName);
                                $('#<%=hdnUnitPrice.ClientID %>').val(result2.Price);
                                $('#<%=hdnPurchaseUnitPrice.ClientID %>').val(result2.UnitPrice);
                                $('#<%=hdnGCBaseUnit.ClientID %>').val(result2.ItemUnit);
                                $('#<%=hdnGCItemUnit.ClientID %>').val(result2.PurchaseUnit);
                                $('#<%=txtDiscount.ClientID %>').val(result2.Discount != 0 ? result2.Discount : "0.00").trigger('changeValue');
                                $('#<%=txtDiscount2.ClientID %>').val(result2.Discount2 != 0 ? result2.Discount2 : "0.00").trigger('changeValue');
                            }
                            else {
                                $('#<%=txtSupplierItemCode.ClientID %>').val('');
                                $('#<%=txtSupplierItemName.ClientID %>').val('');
                                $('#<%=hdnUnitPrice.ClientID %>').val('0');
                                $('#<%=hdnPurchaseUnitPrice.ClientID %>');
                                $('#<%=txtDiscount.ClientID %>').val("0.00").trigger('changeValue');
                                $('#<%=txtDiscount2.ClientID %>').val("0.00").trigger('changeValue');
                            }
                        });
                        cboItemUnit.PerformCallback();
                    }
                    else {
                        $('#<%=hdnGCBaseUnit.ClientID %>').val('');
                        $('#<%=hdnItemID.ClientID %>').val('');
                        $('#<%=txtItemName.ClientID %>').val('');
                        $('#<%=hdnItemGroupID.ClientID %>').val('');
                        $('#<%=txtItemGroupCode.ClientID %>').val('');
                        $('#<%=txtItemGroupName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Purchase Receive No
            function onGetPurchaseReceiveNoFilterExpression() {
                var filterExpression = "<%:GetFilterExpression() %>";
                return filterExpression;
            }

            $('#lblPurchaseReceiveNo.lblLink').click(function () {
                openSearchDialog('purchasereceivehd', onGetPurchaseReceiveNoFilterExpression(), function (value) {
                    $('#<%=txtPurchaseReceiveNo.ClientID %>').val(value);
                    onTxtPurchaseReceiveNoChanged(value);
                });
            });

            $('#<%=txtPurchaseReceiveNo.ClientID %>').change(function () {
                onTxtPurchaseReceiveNoChanged($(this).val());
            });

            function onTxtPurchaseReceiveNoChanged(value) {
                onLoadObject(value);
            }
            //#endregion

            $('#lblAddData').click(function (evt) {
                if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                    $('#<%=chkIsBonus.ClientID %>').removeAttr("disabled");
                    $('#<%=txtQuantity.ClientID %>').val('1.00');
                    $('#<%=hdnEntryID.ClientID %>').val('');
                    $('#<%=hdnItemID.ClientID %>').val('');
                    $('#<%=hdnGCItemUnit.ClientID %>').val('');
                    $('#<%=hdnGCBaseUnit.ClientID %>').val('');
                    $('#<%=txtItemCode.ClientID %>').val('');
                    $('#<%=txtItemName.ClientID %>').val('');
                    $('#<%=hdnItemGroupID.ClientID %>').val('');
                    $('#<%=txtItemGroupCode.ClientID %>').val('');
                    $('#<%=txtItemGroupName.ClientID %>').val('');
                    $('#<%=txtOrderNo.ClientID %>').val('');
                    $('#<%=txtOrderQty.ClientID %>').val('');
                    $('#<%=txtOrderUnit.ClientID %>').val('');
                    $('#<%=lblSupplier.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%=txtSupplierCode.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=chkIsBonus.ClientID %>').prop('checked', false);
                    $('#<%=hdnUnitPrice.ClientID %>').val('0');
                    $('#<%=hdnPurchaseUnitPrice.ClientID %>').val('0');
                    $('#<%=txtBaseUnit.ClientID %>').val('');
                    $('#<%=txtPrice.ClientID %>').val('0.00');

                    $('#<%=txtDiscount.ClientID %>').val('0.00');
                    $('#<%=txtDiscount2.ClientID %>').val('0.00');
                    $('#<%=txtDiscountAmount.ClientID %>').val('0').trigger('changeValue');
                    $('#<%=txtDiscountAmount2.ClientID %>').val('0').trigger('changeValue');

                    if ($('#<%=chkIsDiscountInPercentage1.ClientID %>').is(':checked')) {
                        $('#<%=txtDiscount.ClientID%>').removeAttr('readonly');
                        $('#<%=txtDiscountAmount.ClientID%>').attr('readonly', 'readonly');
                    } else {
                        $('#<%=txtDiscount.ClientID%>').attr('readonly', 'readonly');
                        $('#<%=txtDiscountAmount.ClientID%>').removeAttr('readonly');
                    }

                    if ($('#<%=chkIsDiscountInPercentage2.ClientID %>').is(':checked')) {
                        $('#<%=txtDiscount2.ClientID%>').removeAttr('readonly');
                        $('#<%=txtDiscountAmount2.ClientID%>').attr('readonly', 'readonly');
                    } else {
                        $('#<%=txtDiscount2.ClientID%>').attr('readonly', 'readonly');
                        $('#<%=txtDiscountAmount2.ClientID%>').removeAttr('readonly');
                    }

                    $('#<%=txtSupplierItemCode.ClientID %>').val('');
                    $('#<%=txtSupplierItemName.ClientID %>').val('');
                    $('#<%=txtSubTotalPrice.ClientID %>').val('').trigger('changeValue');
                    lastTransactionAmount = $('#<%=txtTotalOrder.ClientID %>').attr('hiddenVal');
                    editedLineAmount = 0;
                    cboItemUnit.SetValue('');
                    //cboCurrency.SetEnabled(false);
                    cboTerm.SetEnabled(false);
                    $('#<%=txtConversion.ClientID %>').val('');
                    $('#containerEntry').show();
                }
            });

            $('#lblQuickPick').click(function () {
                if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
                    showLoadingPanel();
                    var url = ResolveUrl('~/Program/Consignment/ConsignmentReceive/ConsignmentReceiveQuickPicksCtl.ascx');
                    var transactionID = $('#<%=hdnPRID.ClientID %>').val();
                    var locationID = $('#<%=hdnLocationID.ClientID %>').val();
                    var locationItemGroupID = $('#<%=hdnLocationItemGroupID.ClientID %>').val();
                    var GCLocationGroup = $('#<%=hdnGCLocationGroup.ClientID %>').val();
                    var ProductLineID = $('#<%=hdnProductLineID.ClientID %>').val();
                    var GCItemType = $('#<%=hdnProductLineItemType.ClientID %>').val();
                    var supplierID = $('#<%=hdnSupplierID.ClientID %>').val();
                    var id = transactionID + '|' + locationID + '|' + locationItemGroupID + '|' + GCLocationGroup + '|' + ProductLineID + '|' + GCItemType + '|' + supplierID;
                    openUserControlPopup(url, id, 'Quick Picks', 1000, 600);
                }
            });

            $('#<%=txtQuantity.ClientID %>').change(function () {
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
            });

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
                    $(this).val('0.00');
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
                    $(this).val('0.00');
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
                    $(this).val('0.00').trigger('changeValue');
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

            $('#<%=txtCharges.ClientID %>').change(function () {
                $(this).trigger('changeValue');
                calculateTotal();
            });

            $('#<%=txtFinalDiscountInPercentage.ClientID %>').change(function () {
                $(this).trigger('changeValue');
                var finalDiscount = (parseFloat($(this).attr('hiddenVal')) / 100) * parseFloat($('#<%=txtTotalOrder.ClientID %>').attr('hiddenVal'));
                $('#<%=txtFinalDiscount.ClientID %>').val(finalDiscount).trigger('changeValue');
                calculateTotal();
            });

            $('#<%=txtFinalDiscount.ClientID %>').change(function () {
                $(this).trigger('changeValue');
                var finalDiscountInPercentage = (parseFloat($(this).attr('hiddenVal')) / parseFloat($('#<%=txtTotalOrder.ClientID %>').attr('hiddenVal'))) * 100;
                $('#<%=txtFinalDiscountInPercentage.ClientID %>').val(finalDiscountInPercentage).trigger('changeValue');
                calculateTotal();
            });

            $('#btnCancel').click(function () {
                $('#containerEntry').hide();
            });

            $('#btnSave').click(function (evt) {
                if (IsValid(evt, 'fsTrxPopup', 'mpTrxPopup'))
                    cbpProcess.PerformCallback('save');
            });

            $('#<%=txtFinalDiscount.ClientID %>').change();

            $('.lblExpiredDate').click(function () {
                if ($('#<%=hdnIsEditable.ClientID %>').val() == '1') {
                    $tr = $(this).closest('tr');
                    var param = $tr.find('.keyField').html();
                    var url = ResolveUrl("~/Program/Warehouse/PurchaseReceive/ExpiredDatePerItemCtl.ascx");
                    openUserControlPopup(url, param, 'Expired Date Per Item', 550, 450);
                }
            });

            var pageCount = parseInt($('#<%=hdnPageCount.ClientID %>').val());
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
        }


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
                $(this).val('0.00');
            }

            if ($('#<%:chkIsDiscountInPercentage1.ClientID %>').is(':checked')) {
                var price = parseFloat($('#<%=txtPrice.ClientID %>').attr('hiddenVal'));
                var qty = parseFloat($('#<%=txtQuantity.ClientID %>').val());
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
                $('#<%=hdnDiscountAmount1.ClientID %>').val(0);
                $('#<%=txtDiscountAmount.ClientID %>').val(0).trigger('changeValue');
            }

            if ($('#<%:chkIsDiscountInPercentage1.ClientID %>').is(':checked')) {
            } else {
                var price = $('#<%=txtPrice.ClientID %>').attr('hiddenVal');
                var qty = $('#<%=txtQuantity.ClientID %>').val();
                var subTotal = price * qty;
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
                $(this).val('0.00');
            }

            var price = $('#<%=txtPrice.ClientID %>').attr('hiddenVal');
            var qty = $('#<%=txtQuantity.ClientID %>').val();
            var subTotal = price * qty;
            subTotal = subTotal - parseFloat($('#<%=txtDiscountAmount.ClientID %>').attr('hiddenVal')); ;
            var discountAmount2 = (parseFloat($(this).val()) * subTotal / 100);
            $('#<%=hdnDiscountAmount2.ClientID %>').val(discountAmount2);
            $('#<%=txtDiscountAmount2.ClientID %>').val(discountAmount2).trigger('changeValue');
            calculateSubTotal();
        });

        $('#<%=txtDiscountAmount2.ClientID %>').die('change');
        $('#<%=txtDiscountAmount2.ClientID %>').live('change', function () {
            if ($(this).val() == '' || $('#<%=txtDiscount.ClientID %>').val() == 0 || $('#<%=txtDiscountAmount.ClientID %>').val() == 0) {
                $(this).val('0.00').trigger('changeValue');
            }

            if ($('#<%:chkIsDiscountInPercentage2.ClientID %>').is(':checked')) {
            } else {
                var price = $('#<%=txtPrice.ClientID %>').attr('hiddenVal');
                var qty = $('#<%=txtQuantity.ClientID %>').val();
                var subTotal = price * qty;
                var discountAmount = parseFloat($(this).val());
                var discountPercent = parseFloat(discountAmount / subTotal * 100).toFixed(2);
                $('#<%=hdnDiscountAmount2.ClientID %>').val(discountAmount);
                $('#<%=txtDiscount2.ClientID %>').val(discountPercent).trigger('changeValue');
            }
            calculateSubTotal();
        });
        //#endregion

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
            $('#<%=hdnItemGroupID.ClientID %>').val(entity.ItemGroupID);
            $('#<%=txtItemGroupCode.ClientID %>').val(entity.ItemGroupCode);
            $('#<%=txtItemGroupName.ClientID %>').val(entity.ItemGroupName1);

            $('#<%=hdnEntryID.ClientID %>').val(entity.ID);
            $('#<%=txtOrderNo.ClientID %>').val(entity.PurchaseOrderNo);
            $('#<%=chkIsBonus.ClientID %>').attr('disabled', true);
            $('#<%=chkIsBonus.ClientID %>').prop('checked', (entity.IsBonusItem == 'True'));
            $('#<%=hdnItemID.ClientID %>').val(entity.ItemID);
            $('#<%=txtItemCode.ClientID %>').val(entity.ItemCode);
            $('#<%=txtItemName.ClientID %>').val(entity.ItemName1);
            $('#<%=txtQuantity.ClientID %>').val(entity.Quantity);
            $('#<%=hdnGCItemUnit.ClientID %>').val(entity.GCItemUnit);
            $('#<%=hdnGCBaseUnit.ClientID %>').val(entity.GCBaseUnit);
            $('#<%=txtOrderQty.ClientID %>').val(entity.OrderQuantity);
            $('#<%=txtOrderUnit.ClientID %>').val(entity.OrderPurchaseUnit);
            $('#<%=hdnTempFactor.ClientID %>').val(entity.ConversionFactor);
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

            lastTransactionAmount = $('#<%=txtTotalOrder.ClientID %>').attr('hiddenVal');
            editedLineAmount = entity.CustomSubTotal;
            cboItemUnit.PerformCallback("edit");
            $('#containerEntry').show();
        });
        //#endregion

        function calculateSubTotal() {
            var price = parseFloat($('#<%=txtPrice.ClientID %>').attr('hiddenVal'));
            var qty = parseFloat($('#<%=txtQuantity.ClientID %>').val());
            var subTotal = price * qty;
            var discount1 = parseFloat($('#<%=txtDiscount.ClientID %>').val());
            var discountAmount1 = parseFloat($('#<%=txtDiscountAmount.ClientID %>').val().replace('.00', '').split(',').join(''));
            var discount2 = parseFloat($('#<%=txtDiscount2.ClientID %>').val());
            var discountAmount2 = parseFloat($('#<%=txtDiscountAmount2.ClientID %>').val().replace('.00', '').split(',').join(''));

            subTotal = subTotal - discountAmount1 - discountAmount2;

            $('#<%=txtSubTotalPrice.ClientID %>').val(subTotal).trigger('changeValue');
            var totalOrder = lastTransactionAmount - editedLineAmount + subTotal;
            $('#<%=txtTotalOrder.ClientID %>').val(totalOrder).trigger('changeValue');
            calculateTotal();
        }

        function calculateTotal() {
            var totalKotor = parseFloat($('#<%=txtTotalOrder.ClientID %>').attr('hiddenVal'));
            var Discount = parseFloat($('#<%=txtFinalDiscount.ClientID %>').attr('hiddenVal'));

            if ($('#<%=chkPPN.ClientID %>').is(':checked')) {
                var temp = totalKotor - Discount;
                var PPN = parseFloat($('#<%=hdnVATPercentage.ClientID %>').val()) / 100 * parseFloat(temp);
                $('#<%=txtPPN.ClientID %>').val(PPN).trigger('changeValue');
            }
            else {
                $('#<%=txtPPN.ClientID %>').val('0').trigger('changeValue');
            }
            var PPN = parseFloat($('#<%=txtPPN.ClientID %>').attr('hiddenVal'));
            var DP = parseFloat($('#<%=txtDP.ClientID %>').attr('hiddenVal'));
            var Charge = parseFloat($('#<%=txtCharges.ClientID %>').attr('hiddenVal'));
            var totalHarga = totalKotor - (Discount + DP) + PPN + Charge;
            $('#<%=txtTotalOrderSaldo.ClientID %>').val(totalHarga).trigger('changeValue');
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

        function onAfterSaveAddRecordEntryPopup(param) {
            onAfterCustomSaveSuccess();
            onLoadObject(param);
        }

        function onAfterSaveRecordDtSuccess(PRID) {
            if ($('#<%=hdnPRID.ClientID %>').val() == '0') {
                $('#<%=hdnPRID.ClientID %>').val(PRID);
                var filterExpression = 'PurchaseReceiveID = ' + PRID;
                Methods.getObject('GetPurchaseReceiveHdList', filterExpression, function (result) {
                    $('#<%=txtPurchaseReceiveNo.ClientID %>').val(result.PurchaseReceiveNo);
                });
                onAfterCustomSaveSuccess();
            }
        }

        function onCbpProcesEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'save') {
                if (param[1] == 'fail')
                    showToast('Save Failed', 'Error Message : ' + param[2]);
                else {
                    var PRID = s.cpOrderID;
                    onAfterSaveRecordDtSuccess(PRID);
                    $('#lblAddData').click();
                    $('#containerEntry').hide();
                    cbpView.PerformCallback('refresh');
                }
            }
            else if (param[0] == 'delete') {
                if (param[1] == 'fail')
                    showToast('Delete Failed', 'Error Message : ' + param[2]);
                else
                    cbpView.PerformCallback('refresh');
            }
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
            Methods.getItemMasterPurchase($('#<%=hdnItemID.ClientID %>').val(), $('#<%=hdnSupplierID.ClientID %>').val(), function (result) {
                if (result != null) {
                    purchaseUnit = result.PurchaseUnit;
                    tempPricePerPurchaseUnit = result.UnitPrice;
                }
            });
            var baseValue = $('#<%=hdnGCBaseUnit.ClientID %>').val();
            var toUnitItem = cboItemUnit.GetValue();
            var baseText = getItemUnitName(baseValue);
            var factor = 1;
            var factorOld = $('#<%=hdnTempFactor.ClientID %>').val();
            var qtyBegin = $('#<%=txtQuantity.ClientID %>').val();
            var qtyEnd = 1;

            var filterExpressionIP = "ItemID = " + $('#<%=hdnItemID.ClientID %>').val() + " AND IsDeleted = 0 AND IsUsingSupplierCatalog = 1";
            var isUsingSupplierCatalog = 0;
            Methods.getObject('GetItemPlanningList', filterExpressionIP, function (result) {
                if (result != null) {
                    isUsingSupplierCatalog = 1;
                }
            });

            //change price per unit
            var purchaseUnit = cboItemUnit.GetValue();
            Methods.getItemMasterPurchaseList($('#<%=hdnItemID.ClientID %>').val(), $('#<%=hdnSupplierID.ClientID %>').val(), function (result) {
                if (result.length > 0 && isUsingSupplierCatalog == 1) {
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

                                $('#<%=txtDiscount.ClientID %>').val(result[i].Discount != 0 ? result[i].Discount : "0.00");
                                $('#<%=txtDiscount2.ClientID %>').val(result[i].Discount2 != 0 ? result[i].Discount2 : "0.00");
                                $('#<%=txtDiscount.ClientID %>').trigger('change');
                                $('#<%=txtDiscount2.ClientID %>').trigger('change');
                            }
                        } else if (result[i].PurchaseUnit == purchaseUnit) {
                            var conversion = "1 " + result[i].ItemUnitText + " = " + result[i].ConversionFactor + " " + result[i].PurchaseUnitText;
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

                                $('#<%=txtDiscount.ClientID %>').val(result[i].Discount != 0 ? result[i].Discount : "0.00");
                                $('#<%=txtDiscount2.ClientID %>').val(result[i].Discount2 != 0 ? result[i].Discount2 : "0.00");
                                $('#<%=txtDiscount.ClientID %>').trigger('change');
                                $('#<%=txtDiscount2.ClientID %>').trigger('change');
                            }
                        }
                    }
                }
                else {
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
                    } else {
                        pricePerPurchaseUnit = factor * priceperitemunit;
                    }
                    ////            var pricePerPurchaseUnit = factor * priceperitemunit;

                    $('#<%=txtPrice.ClientID %>').val(pricePerPurchaseUnit).trigger('changeValue');
                }
            });

            $('#<%=txtBaseUnit.ClientID %>').val("per " + cboItemUnit.GetText());

            factor = $('#<%=hdnConversionFactor.ClientID %>').val();

            //change qty
            if (factor != 1) {
                if (factorOld == 1) {
                    qtyEnd = qtyBegin / factor;
                    $('#<%=txtQuantity.ClientID %>').val(qtyEnd);
                } else {
                    qtyEnd = qtyBegin;
                    $('#<%=txtQuantity.ClientID %>').val(qtyEnd);
                }
            } else {
                if (factorOld != 0) {
                    qtyEnd = qtyBegin * factorOld;
                    $('#<%=txtQuantity.ClientID %>').val(qtyEnd);
                } else {
                    qtyEnd = qtyBegin;
                    $('#<%=txtQuantity.ClientID %>').val(qtyEnd);
                }
            }

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

        function onBeforeLoadRightPanelContent(code) {
            if (code == 'updateTaxConsignment' || code == 'updateReferenceConsignment' || code == 'infoPurchased' || code == 'infoInvoice') {
                var param = $('#<%:hdnPRID.ClientID %>').val();
                return param;
            }
            else {
                return $('#<%:hdnPRID.ClientID %>').val();
            }
        }

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var purchaseReceiveID = $('#<%=hdnPRID.ClientID %>').val();
            var printStatus = $('#<%=hdnPrintStatus.ClientID %>').val();
            if (printStatus == 'true') {
                if (purchaseReceiveID == '' || purchaseReceiveID == '0') {
                    errMessage.text = 'Please Set Transaction First!';
                    return false;
                }
                else {
                    filterExpression.text = "PurchaseReceiveID = " + purchaseReceiveID;
                    return true;
                }
            } else {
                errMessage.text = "Data Doesn't Approved or Closed";
                return false;
            }
        }

        function setRightPanelButtonEnabled() {
            if ($('#<%:hdnPRID.ClientID %>').val() == '' || $('#<%:hdnPRID.ClientID %>').val() == '0') {
                $('#btnUpdateTaxConsignment').attr('enabled', 'false');
                $('#btnUpdateReferenceConsignment').attr('enabled', 'false');
                $('#btnInfoPurchaseReceive').attr('enabled', 'false');
                $('#btnInvoiceInfo').attr('enabled', 'false');
            } else {
                $('#btnUpdateTaxConsignment').removeAttr('enabled');
                $('#btnUpdateReferenceConsignment').removeAttr('enabled');
                $('#btnInfoPurchaseReceive').removeAttr('enabled');
                $('#btnInvoiceInfo').removeAttr('enabled');
            }
        }
    </script>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnIsAPConsignmentFromOrder" runat="server" />
    <input type="hidden" value="" id="hdnPrintStatus" runat="server" />
    <input type="hidden" value="" id="hdnVATPercentageFromSetvar" runat="server" />
    <input type="hidden" value="" id="hdnVATPercentage" runat="server" />
    <input type="hidden" value="" id="hdnPRID" runat="server" />
    <input type="hidden" value="" id="hdnPageCount" runat="server" />
    <input type="hidden" value="1" id="hdnIsEditable" runat="server" />
    <input type="hidden" value="0" id="hdnNeedConfirmation" runat="server" />
    <input type="hidden" value="0" id="hdnIsDiscountAppliedToAveragePrice" runat="server" />
    <input type="hidden" value="0" id="hdnIsDiscountAppliedToUnitPrice" runat="server" />
    <input type="hidden" value="0" id="hdnIsPPNAppliedToAveragePrice" runat="server" />
    <input type="hidden" value="0" id="hdnIsPPNAppliedToUnitPrice" runat="server" />
    <input type="hidden" value="" id="hdnRecordFilterExpression" runat="server" />
    <input type="hidden" value="0" id="hdnIsUsedProductLine" runat="server" />
    <input type="hidden" value="0" id="hdnTempFactor" runat="server" />
    <input type="hidden" value="0" id="hdnIsPpnAllowChanged" runat="server" />
    <input type="hidden" value="" id="hdnRoleOfficerLogistic" runat="server" />
    <input type="hidden" value="0" id="hdnIsReceiveUsingBaseUnit" runat="server" />
    <input type="hidden" value="1" id="hdnKapanPerubahanNilaiHargaKeItemPlanning" runat="server" />
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
                                <label class="lblLink" id="lblPurchaseReceiveNo">
                                    <%=GetLabel("No. BPB")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPurchaseReceiveNo" Width="210px" ReadOnly="true" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Tanggal") %>
                                -
                                <%=GetLabel("Waktu Penerimaan") %>
                            </td>
                            <td>
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="padding-right: 1px; width: 145px">
                                            <asp:TextBox ID="txtPurchaseReceiveDate" Width="120px" CssClass="datepicker" runat="server" />
                                        </td>
                                        <td style="width: 5px">
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPurchaseReceiveTime" Width="60px" CssClass="time" runat="server"
                                                Style="text-align: center" />
                                        </td>
                                    </tr>
                                </table>
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
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("No.Faktur/Kirim")%></label>
                            </td>
                            <td>
                                <table cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                        <col style="width: 250px" />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtFacturNo" CssClass="required" ValidationGroup="mpEntry" Width="100%"
                                                runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <input type="button" id="btnPurchaseReceive" value="Salin Pesanan Barang" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Tanggal di Faktur") %>
                            </td>
                            <td style="padding-right: 1px; width: 140px">
                                <asp:TextBox ID="txtDateReferrence" Width="120px" CssClass="datepicker" runat="server" />
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
                                    <%=GetLabel("Waktu Pembayaran")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboTerm" ClientInstanceName="cboTerm" Width="200px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory lblLink" runat="server" id="lblLocation">
                                    <%=GetLabel("Ke Lokasi")%></label>
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
                        <tr style="display: none">
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Mata Uang")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox Visible="false" ID="cboCurrency" ClientInstanceName="cboCurrency"
                                    Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr style="display: none">
                            <td class="tdLabel">
                                <%=GetLabel("Nilai Kurs (Rp)") %>
                            </td>
                            <td>
                                <asp:TextBox ID="txtKurs" Width="120px" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div id="containerEntry" style="margin-top: 4px; display: none;">
                        <div class="pageTitle">
                            <%=GetLabel("Edit atau Tambah Bonus Item Penerimaan Pembelian")%></div>
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
                                                <col style="width: 130px" />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkIsBonus" Width="100%" runat="server" />&nbsp;<%=GetLabel("Bonus")%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label>
                                                        <%=GetLabel("No. Pemesanan")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtOrderNo" Width="150px" ReadOnly="true" runat="server" />
                                                </td>
                                            </tr>
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
                                                <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Jumlah Dipesan")%></label>
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
                                                                <asp:TextBox ID="txtOrderQty" ReadOnly="true" Width="120px" CssClass="number" runat="server" />
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtOrderUnit" ReadOnly="true" Width="150px" runat="server" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("Jumlah Diterima")%></label>
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
                                                                <asp:TextBox ID="txtQuantity" CssClass="number" Width="120px" runat="server" />
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <dxe:ASPxComboBox runat="server" ID="cboItemUnit" ClientInstanceName="cboItemUnit"
                                                                    Width="300px" OnCallback="cboItemUnit_Callback">
                                                                    <ClientSideEvents EndCallback="function(s,e){ onCboItemUnitEndCallBack(s); }" ValueChanged="function(s,e){ onCboItemUnitChanged(); }" />
                                                                </dxe:ASPxComboBox>
                                                            </td>
                                                        </tr>
                                                    </table>
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
                                            <tr style="display: none">
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("No. Batch")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtBatchNo" runat="server" />
                                                </td>
                                            </tr>
                                            <tr style="display: none">
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Expired")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtExpired" value="0.00" CssClass="txtCurrency" ReadOnly="true"
                                                        Width="180px" runat="server" />
                                                </td>
                                            </tr>
                                            <tr style="display: none">
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Acc")%></label>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkAcc" Width="100%" runat="server" Checked="true" />
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
                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                        OnRowDataBound="grdView_RowDataBound">
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
                                                    <input type="hidden" value="<%#:Eval("IsBonusItem") %>" bindingfield="IsBonusItem" />
                                                    <input type="hidden" value="<%#:Eval("Quantity") %>" bindingfield="Quantity" />
                                                    <input type="hidden" value="<%#:Eval("PurchaseOrderNo") %>" bindingfield="PurchaseOrderNo" />
                                                    <input type="hidden" value="<%#:Eval("ItemID") %>" bindingfield="ItemID" />
                                                    <input type="hidden" value="<%#:Eval("ItemCode") %>" bindingfield="ItemCode" />
                                                    <input type="hidden" value="<%#:Eval("ItemName1") %>" bindingfield="ItemName1" />
                                                    <input type="hidden" value="<%#:Eval("ItemGroupID") %>" bindingfield="ItemGroupID" />
                                                    <input type="hidden" value="<%#:Eval("ItemGroupCode") %>" bindingfield="ItemGroupCode" />
                                                    <input type="hidden" value="<%#:Eval("ItemGroupName1") %>" bindingfield="ItemGroupName1" />
                                                    <input type="hidden" value="<%#:Eval("GCBaseUnit") %>" bindingfield="GCBaseUnit" />
                                                    <input type="hidden" value="<%#:Eval("GCItemUnit") %>" bindingfield="GCItemUnit" />
                                                    <input type="hidden" value="<%#:Eval("OrderQuantity") %>" bindingfield="OrderQuantity" />
                                                    <input type="hidden" value="<%#:Eval("OrderPurchaseUnit") %>" bindingfield="OrderPurchaseUnit" />
                                                    <input type="hidden" value="<%#:Eval("UnitPrice") %>" bindingfield="UnitPrice" />
                                                    <input type="hidden" value="<%#:Eval("ConversionFactor") %>" bindingfield="ConversionFactor" />
                                                    <input type="hidden" value="<%#:Eval("SupplierItemCode") %>" bindingfield="SupplierItemCode" />
                                                    <input type="hidden" value="<%#:Eval("SupplierItemName") %>" bindingfield="SupplierItemName" />
                                                    <input type="hidden" value="<%#:Eval("IsDiscountInPercentage1")%>" bindingfield="IsDiscountInPercentage1" />
                                                    <input type="hidden" value="<%#:Eval("IsDiscountInPercentage2")%>" bindingfield="IsDiscountInPercentage2" />
                                                    <input type="hidden" value="<%#:Eval("DiscountPercentage1") %>" bindingfield="DiscountPercentage1" />
                                                    <input type="hidden" value="<%#:Eval("DiscountPercentage2") %>" bindingfield="DiscountPercentage2" />
                                                    <input type="hidden" value="<%#:Eval("DiscountAmount1") %>" bindingfield="DiscountAmount1" />
                                                    <input type="hidden" value="<%#:Eval("DiscountAmount2") %>" bindingfield="DiscountAmount2" />
                                                    <input type="hidden" value="<%#:Eval("CustomSubTotal") %>" bindingfield="CustomSubTotal" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" HeaderText="Bonus"
                                                HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkIsBonus" Enabled="false" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ItemCode" HeaderText="Kode Item" HeaderStyle-Width="80px"
                                                HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="ItemName1" HeaderText="Nama Item" HeaderStyle-Width="240px"
                                                HeaderStyle-HorizontalAlign="Left" />
                                            <asp:TemplateField HeaderStyle-Width="100px" HeaderText="Dipesan" ItemStyle-HorizontalAlign="Right"
                                                HeaderStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <%#:Eval("OrderQuantity")%>
                                                    <span style="width: 60px">
                                                        <%#:Eval("OrderPurchaseUnit")%>
                                                    </span>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="100px" HeaderText="Diterima" ItemStyle-HorizontalAlign="Right"
                                                HeaderStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <%#:Eval("Quantity") %>
                                                    <%#:Eval("ItemUnit")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="CustomConversion" HeaderText="Konversi" HeaderStyle-Width="150px"
                                                ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="CustomUnitPrice" HeaderText="Harga / Satuan" ItemStyle-HorizontalAlign="Right"
                                                HeaderStyle-HorizontalAlign="Right" HeaderStyle-Width="120px" />
                                            <asp:TemplateField HeaderText="Disc1 (%)" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="50px">
                                                <ItemTemplate>
                                                    <label runat="server" id="lblDiscountPercentage1" class="lblDiscountPercentage1">
                                                        <%#:Eval("DiscountPercentage1", "{0:N2}")%>
                                                    </label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Disc1" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="80px">
                                                <ItemTemplate>
                                                    <label runat="server" id="lblDiscountAmount1" class="lblDiscountAmount1">
                                                        <%#:Eval("DiscountAmount1", "{0:N2}")%>
                                                    </label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Disc2 (%)" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="50px">
                                                <ItemTemplate>
                                                    <label runat="server" id="lblDiscountPercentage2" class="lblDiscountPercentage2">
                                                        <%#:Eval("DiscountPercentage2", "{0:N2}")%>
                                                    </label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Disc2" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="80px">
                                                <ItemTemplate>
                                                    <label runat="server" id="lblDiscountAmount2" class="lblDiscountAmount2">
                                                        <%#:Eval("DiscountAmount2", "{0:N2}")%>
                                                    </label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="PurchaseOrderNo" HeaderText="No. Pemesanan" HeaderStyle-Width="150px"
                                                HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="CustomSubTotal" HeaderText="Sub Total" ItemStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Right" DataFormatString="{0:N}" />
                                            <asp:TemplateField HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <label <%# IsEditable() == "1" ? "class='lblExpiredDate lblLink'":"class='lblExpiredDate lblLink lblDisabled'" %>>
                                                        <%=GetLabel("Expired Date")%></label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="CreatedByName" HeaderText="Penerima" />
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <img src='<%# ResolveUrl("~/Libs/Images/Button/verify.png") %>' <%# Eval("isConfirmed").ToString() == "True" ? "" : "Style ='display:none'" %>
                                                        title='<%=GetLabel("Confirmed") %>' alt="" />
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
                    <div style="width: 100%; text-align: center">
                        <span class="lblLink" id="lblAddData" style="margin-right: 300px;">
                            <%= GetLabel("Tambah Item")%></span> <span class="lblLink" id="lblQuickPick">
                                <%= GetLabel("Quick Picks")%></span>
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
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Catatan")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtNotes" Width="100%" runat="server" TextMode="MultiLine" Rows="5" />
                                                </td>
                                            </tr>
                                            <tr>
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
                                                        <%=GetLabel("Jumlah Nilai Pembelian")%></label>
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
                                                    <asp:TextBox ID="txtFinalDiscount" CssClass="txtCurrency" Width="180px" runat="server" />
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
                                                        <%=GetLabel("No. Reff Uang Muka")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDPReferrenceNo" Width="180px" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Uang Muka")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDP" CssClass="txtCurrency" Width="180px" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Jenis Pembiayaan")%></label>
                                                </td>
                                                <td>
                                                    <dxe:ASPxComboBox ID="cboChargesType" ClientInstanceName="cboChargesType" Width="180px"
                                                        runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Biaya")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtCharges" CssClass="txtCurrency" Width="180px" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Total Nilai Penerimaan")%></label>
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
                                                        <%=GetLabel("Approved Oleh") %>
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
                                                        <%=GetLabel("Approved Pada")%>
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
