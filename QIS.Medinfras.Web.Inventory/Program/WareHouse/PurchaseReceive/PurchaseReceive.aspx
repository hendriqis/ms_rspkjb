<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx2.master"
    ValidateRequest="false" AutoEventWireup="true" CodeBehind="PurchaseReceive.aspx.cs"
    Inherits="QIS.Medinfras.Web.Inventory.Program.PurchaseReceive" %>

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

        function onLoad() {
            setRightPanelButtonEnabled();

            if ($('#<%=hdnIsEditable.ClientID %>').val() == '1')
                $('#lblAddData').show();
            else
                $('#lblAddData').hide();

            if ($('#<%=hdnIsAllowBackdatePOR.ClientID %>').val() == '1') {
                setDatePicker('<%=txtPurchaseReceiveDate.ClientID %>');
                $('#<%=txtPurchaseReceiveDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));
            }

            setDatePicker('<%=txtDateReferrence.ClientID %>');
            $('#<%=txtDateReferrence.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

            setDatePicker('<%=txtTaxInvoiceDate.ClientID %>');

            setDatePicker('<%=txtPaymentDueDate.ClientID %>');

            $('.txtCurrency').each(function () {
                $(this).trigger('changeValue');
            });

            if ($('#<%=chkPPN.ClientID %>').is(':checked')) {
                $('#<%=trPPN.ClientID %>').attr('style', 'display:none');
            }
            else {
                var isVATMandatory = $('#<%=hdnIsVATMandatory.ClientID %>').val();

                if (isVATMandatory == "1") {
                    $('#<%:trPPN.ClientID %>').removeAttr('style');
                } else {
                    $('#<%=trPPN.ClientID %>').attr('style', 'display:none');
                }
            }

            if ($('#<%=hdnIsPPhDetailEdit.ClientID %>').val() == '1') {
                if ($('#<%=chkPPHPercentDetail.ClientID %>').is(':checked')) {
                    $('#<%=txtPPHPercentageDetail.ClientID %>').change();
                    $('#<%=txtPPHPercentageDetail.ClientID%>').removeAttr('readonly');
                    $('#<%=txtPPHAmountDetail.ClientID%>').attr('readonly', 'readonly');
                } else {
                    $('#<%=txtPPHAmountDetail.ClientID %>').change();
                    $('#<%=txtPPHPercentageDetail.ClientID%>').attr('readonly', 'readonly');
                    $('#<%=txtPPHAmountDetail.ClientID%>').removeAttr('readonly');
                }
            }

            calculateTotal();

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
                    $('#<%=chkPPN.ClientID %>').trigger('change');
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

            $('#<%=lblItemGroup.ClientID %>').live('click', function () {
                var cekLbl = $('#<%=lblItemGroup.ClientID %>').attr('class');
                if (cekLbl == "lblLink") {
                    openSearchDialog('itemgroup', onGetItemGroupFilterExpression(), function (value) {
                        $('#<%=txtItemGroupCode.ClientID %>').val(value);
                        onTxtItemGroupCodeChanged(value);
                    });
                }
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
                var filterExpression = "IsDeleted = 0 AND GCItemStatus != 'X181^999'";

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

            $('#<%=lblItem.ClientID %>').live('click', function () {
                var cekLbl = $('#<%=lblItem.ClientID %>').attr('class');
                if (cekLbl == "lblLink lblMandatory") {
                    openSearchDialog('item', getItemFilterExpression(), function (value) {
                        $('#<%=txtItemCode.ClientID %>').val(value);
                        $('#<%=txtItemName.ClientID %>').val(value);
                        onTxtItemCodeChanged(value);
                    });
                }
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
                                $('#<%=txtDiscount.ClientID %>').val(result2.Discount);
                                $('#<%=hdnUnitPrice.ClientID %>').val(result2.Price).trigger('changeValue');
                                $('#<%=hdnGCBaseUnit.ClientID %>').val(result2.ItemUnit);
                                $('#<%=hdnGCItemUnit.ClientID %>').val(result2.PurchaseUnit);
                            }
                            else {
                                $('#<%=txtSupplierItemCode.ClientID %>').val('');
                                $('#<%=txtSupplierItemName.ClientID %>').val('');
                                $('#<%=txtDiscount.ClientID %>').val('0');
                                $('#<%=hdnUnitPrice.ClientID %>').val('0').trigger('changeValue');
                            }
                        });

                        cboItemUnit.PerformCallback('addItem');
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

            //#region lblAddData
            $('#lblAddData').die('click');
            $('#lblAddData').live('click', function (evt) {
                if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                    $('#<%=lblItemGroup.ClientID %>').attr('class', 'lblLink');
                    $('#<%=txtItemGroupCode.ClientID %>').removeAttr('readonly');
                    $('#<%=lblItem.ClientID %>').attr('class', 'lblLink lblMandatory');
                    $('#<%=txtItemCode.ClientID %>').removeAttr('readonly');
                    $('#<%=txtItemName.ClientID %>').removeAttr('readonly');
                    $('#<%=hdnIsEdit.ClientID %>').val('0');
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
                    $('#<%=chkIsBonus.ClientID %>').prop('checked', true);
                    $('#<%=hdnUnitPrice.ClientID %>').val('0');
                    $('#<%=txtBaseUnit.ClientID %>').val('');
                    $('#<%=txtPrice.ClientID %>').val('0');

                    $('#<%=hdnQtyNow.ClientID %>').val('0');
                    $('#<%=hdnReceivedQty.ClientID %>').val('0');

                    $('#<%=txtDiscount.ClientID %>').val('0');
                    $('#<%=txtDiscount2.ClientID %>').val('0');
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

                    $('#<%=chkPPHPercentDetail.ClientID %>').prop('checked', true);
                    $('#<%=txtPPHPercentageDetail.ClientID %>').val('0');
                    $('#<%=txtPPHAmountDetail.ClientID %>').val('0');
                    $('#<%=txtSupplierItemCode.ClientID %>').val('');
                    $('#<%=txtSupplierItemName.ClientID %>').val('');
                    $('#<%=txtSubTotalPrice.ClientID %>').val('').trigger('changeValue');
                    $('#<%=txtRemarksDetail.ClientID %>').val('');
                    lastTransactionAmount = $('#<%=txtTotalOrder.ClientID %>').attr('hiddenVal');
                    editedLineAmount = 0;
                    cboItemUnit.SetValue('');
                    //cboCurrency.SetEnabled(false);
                    cboTerm.SetEnabled(false);
                    $('#<%=txtConversion.ClientID %>').val('');
                    $('#containerEntry').show();
                }
            });
            //#endregion

            //#region edit and delete
            $('#<%=grdView.ClientID %> .imgDelete.imgLink').die('click');
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

            $('#<%=grdView.ClientID %> .imgEdit.imgLink').die('click');
            $('#<%=grdView.ClientID %> .imgEdit.imgLink').live('click', function () {
                $row = $(this).closest('tr').parent().closest('tr');
                var entity = rowToObject($row);
                $('#<%=hdnIsEdit.ClientID %>').val('1');
                $('#<%=hdnItemGroupID.ClientID %>').val('');
                $('#<%=txtItemGroupCode.ClientID %>').val(entity.ItemGroupCode);
                $('#<%=txtItemGroupName.ClientID %>').val(entity.ItemGroupName1);

                $('#<%=hdnEntryID.ClientID %>').val(entity.ID);
                $('#<%=txtOrderNo.ClientID %>').val(entity.PurchaseOrderNo);
                $('#<%=chkIsBonus.ClientID %>').prop('checked', (entity.IsBonusItem == 'True'));
                $('#<%=hdnItemID.ClientID %>').val(entity.ItemID);
                $('#<%=txtItemCode.ClientID %>').val(entity.ItemCode);
                $('#<%=txtItemName.ClientID %>').val(entity.ItemName1);
                $('#<%=hdnReceivedQty.ClientID %>').val(entity.ReceivedQuantity);
                $('#<%=hdnQtyNow.ClientID %>').val(entity.Quantity);

                var changeQtyPOR = $('#<%=hdnChangeQtyPOR.ClientID %>').val();
                if (changeQtyPOR == '0') {
                    if (entity.PurchaseOrderID != 0) {
                        $('#<%=txtQuantity.ClientID %>').attr('readonly', 'readonly');
                    } else {
                        $('#<%=txtQuantity.ClientID %>').removeAttr('readonly');
                    }
                }
                $('#<%=txtQuantity.ClientID %>').val(entity.Quantity);
                $('#<%=hdnTempFactor.ClientID %>').val(entity.ConversionFactor);
                $('#<%=hdnTempFactorORI.ClientID %>').val(entity.ConversionFactor);
                $('#<%=hdnGCItemUnit.ClientID %>').val(entity.GCItemUnit);
                $('#<%=hdnGCBaseUnit.ClientID %>').val(entity.GCBaseUnit);
                $('#<%=txtOrderQty.ClientID %>').val(entity.OrderQuantity);
                $('#<%=txtOrderUnit.ClientID %>').val(entity.OrderPurchaseUnit);
                $('#<%=txtSupplierItemCode.ClientID %>').val(entity.SupplierItemCode);
                $('#<%=txtSupplierItemName.ClientID %>').val(entity.SupplierItemName);
                $('#<%=hdnConversionFactor.ClientID %>').val(entity.ConversionFactor);

                $('#<%=txtPrice.ClientID %>').val(entity.UnitPrice).trigger('changeValue');
                $('#<%=hdnUnitPrice.ClientID %>').val(parseFloat(entity.UnitPrice) / parseFloat(entity.ConversionFactor)).trigger('changeValue');

                cboItemUnit.PerformCallback("edit");

                cboPPhTypeDetail.SetValue(entity.GCPPHType);
                if (entity.PPHMode == "True") {
                    cboPPHOptionsDetail.SetValue('Plus');
                } else {
                    cboPPHOptionsDetail.SetValue('Minus');
                }
                if (entity.IsPPHInPercentage == "True") {
                    $('#<%=chkPPHPercentDetail.ClientID %>').prop('checked', true);
                } else {
                    $('#<%=chkPPHPercentDetail.ClientID %>').prop('checked', false);
                }
                $('#<%=txtPPHPercentageDetail.ClientID %>').val(entity.PPHPercentage).trigger('changeValue');
                $('#<%=hdnPPhPercentage.ClientID %>').val(entity.PPHPercentage);
                $('#<%=txtPPHAmountDetail.ClientID %>').val(entity.PPHAmount).trigger('changeValue');
                $('#<%=txtRemarksDetail.ClientID %>').val(entity.RemarksDetail);
                lastTransactionAmount = $('#<%=txtTotalOrder.ClientID %>').attr('hiddenVal');
                editedLineAmount = entity.CustomSubTotal;

                var IsDiscountInPercentage1 = entity.IsDiscountInPercentage1;
                var IsDiscountInPercentage2 = entity.IsDiscountInPercentage2;
                if (IsDiscountInPercentage1 == 'False') {
                    $('#<%=chkIsDiscountInPercentage1.ClientID %>').prop('checked', false);
                    $('#<%=txtDiscount.ClientID%>').attr('readonly', 'readonly');
                    $('#<%=txtDiscountAmount.ClientID%>').removeAttr('readonly');
                } else {
                    $('#<%=chkIsDiscountInPercentage1.ClientID %>').prop('checked', true);
                    $('#<%=txtDiscount.ClientID%>').removeAttr('readonly');
                    $('#<%=txtDiscountAmount.ClientID%>').attr('readonly', 'readonly');

                }
                if (IsDiscountInPercentage2 == 'False') {
                    $('#<%=chkIsDiscountInPercentage2.ClientID %>').prop('checked', false);
                    $('#<%=txtDiscount2.ClientID%>').attr('readonly', 'readonly');
                    $('#<%=txtDiscountAmount2.ClientID%>').removeAttr('readonly');
                } else {
                    $('#<%=chkIsDiscountInPercentage2.ClientID %>').prop('checked', true);
                    $('#<%=txtDiscount2.ClientID%>').removeAttr('readonly');
                    $('#<%=txtDiscountAmount2.ClientID%>').attr('readonly', 'readonly');
                }

                $('#<%=txtDiscount.ClientID %>').val(entity.DiscountPercentage1).trigger('changeValue');
                $('#<%=hdnDiscountAmount1.ClientID %>').val(entity.DiscountAmount1);
                $('#<%=txtDiscountAmount.ClientID %>').val(entity.DiscountAmount1).trigger('changeValue');

                $('#<%=txtDiscount2.ClientID %>').val(entity.DiscountPercentage2).trigger('changeValue');
                $('#<%=hdnDiscountAmount2.ClientID %>').val(entity.DiscountAmount2);
                $('#<%=txtDiscountAmount2.ClientID %>').val(entity.DiscountAmount2).trigger('changeValue');

                calculateSubTotal();

                if (entity.IsBonusItem == 'False') {
                    $('#<%=lblItemGroup.ClientID %>').attr('class', 'lblNormal');
                    $('#<%=txtItemGroupCode.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=lblItem.ClientID %>').attr('class', 'lblNormal');
                    $('#<%=txtItemCode.ClientID %>').attr('readonly', 'readonly');
                } else {
                    $('#<%=lblItemGroup.ClientID %>').attr('class', 'lblLink');
                    $('#<%=txtItemGroupCode.ClientID %>').removeAttr('readonly');
                    $('#<%=lblItem.ClientID %>').attr('class', 'lblLink lblMandatory');
                    $('#<%=txtItemCode.ClientID %>').removeAttr('readonly');
                }

                $('#containerEntry').show();
            });
            //#endregion


            $('#<%=txtQuantity.ClientID %>').die('change');
            $('#<%=txtQuantity.ClientID %>').live('change', function () {
                var isAllow = $('#<%=hdnAllowPORQtyBiggerThanPO.ClientID %>').val();
                var qtyPesan = $('#<%=txtOrderQty.ClientID %>').val();

                var itemID = $('#<%=hdnItemID.ClientID %>').val();

                var txtQtyNow = $('#<%=txtQuantity.ClientID %>').val();

                var qtyORI = $('#<%=hdnQtyNow.ClientID %>').val();
                var receivedQty = $('#<%=hdnReceivedQty.ClientID %>').val();

                var conversionFactorORI = $('#<%=hdnTempFactorORI.ClientID %>').val();

                var itemUnit = $('#<%=hdnGCItemUnit.ClientID %>').val();
                var toUnitItem = cboItemUnit.GetValue();

                var filterExpression = "IsDeleted = 0 AND IsActive = 1 AND ItemID = " + itemID + " AND GCAlternateUnit = '" + toUnitItem + "'";
                Methods.getObjectValue('GetvItemAlternateItemUnitList', filterExpression, 'ConversionFactor', function (result) {
                    $('#<%=hdnConversionFactor.ClientID %>').val(result);
                });

                $('#<%=txtDiscount.ClientID %>').trigger('change');

                calculateSubTotal();
            });

            $('#<%=txtPrice.ClientID %>').change(function () {
                $(this).blur();

                $('#<%=txtDiscount.ClientID %>').trigger('change');
            });

            $('#<%=chkPPN.ClientID %>').live('change', function () {
                var purchaseReceiveID = $('#<%=hdnPRID.ClientID %>').val();
                var isValidateVAT = $('#<%=hdnIsValidateVAT.ClientID %>').val();

                var isVATMandatory = $('#<%=hdnIsVATMandatory.ClientID %>').val();
                var isEditable = $('#<%=hdnIsPpnAllowChanged.ClientID %>').val();
                var defaultValue = $('#<%=hdnDefaultValueForNoFakturPajak.ClientID %>').val();

                var isValid = true;

                if ($(this).is(":checked")) {
                    if (isValidateVAT == "1") {
                        if (purchaseReceiveID != "0") {
                            var filterExpression = "PurchaseOrderID IN (SELECT DISTINCT PurchaseOrderID FROM PurchaseReceiveDt WHERE PurchaseReceiveID = '" + purchaseReceiveID + "' AND GCItemDetailStatus != 'X121^999')";
                            Methods.getListObject('GetPurchaseOrderHdList', filterExpression, function (result) {
                                if (result.length > 0) {
                                    for (i = 0; i < result.length; i++) {
                                        if (result[i].IsIncludeVAT == "0") {
                                            isValid = false;
                                            $('#<%=chkPPN.ClientID %>').attr('checked', false);
                                            showToast('Warning', "Penerimaan ini memiliki detail dari PO yang memiliki PPN. harap sesuaikan detail penerimaan terlebih dahulu.");
                                        }
                                    }
                                }
                            });
                        }
                    }

                    if (isValid) {
                        $('#<%=trTaxInvoice.ClientID %>').css('display', '');
                        $('#<%=txtTaxInvoiceNo.ClientID %>').addClass('required');
                        $('#<%=txtTaxInvoiceNo.ClientID %>').attr('validationgroup', 'mpEntry');
                        $('#<%=txtTaxInvoiceNo.ClientID %>').val(defaultValue);

                        if (isVATMandatory == "1") {
                            $('#<%=trPPN.ClientID %>').attr('style', 'display:none');
                        }

                        if (isEditable == '1') {
                            $('#<%:txtVATPercentageDefault.ClientID %>').removeAttr('readonly');
                        }
                        else {
                            $('#<%:txtVATPercentageDefault.ClientID %>').attr('readonly', 'readonly');
                        }
                    }
                }
                else {
                    if (isValidateVAT == "1") {
                        if (purchaseReceiveID != "0") {
                            var filterExpression = "PurchaseOrderID IN (SELECT DISTINCT PurchaseOrderID FROM PurchaseReceiveDt WHERE PurchaseReceiveID = '" + purchaseReceiveID + "' AND GCItemDetailStatus != 'X121^999')";
                            Methods.getListObject('GetPurchaseOrderHdList', filterExpression, function (result) {
                                if (result.length > 0) {
                                    for (i = 0; i < result.length; i++) {
                                        if (result[i].IsIncludeVAT == "1") {
                                            isValid = false;
                                            $('#<%=chkPPN.ClientID %>').attr('checked', true);
                                            showToast('Warning', "Penerimaan ini memiliki detail dari PO yang tidak memiliki PPN. harap sesuaikan detail penerimaan terlebih dahulu.");
                                        }
                                    }
                                }
                            });
                        }
                    }

                    if (isValid) {
                        $('#<%=trTaxInvoice.ClientID %>').css('display', 'none');
                        $('#<%=txtTaxInvoiceNo.ClientID %>').removeClass('required');
                        $('#<%=txtTaxInvoiceNo.ClientID %>').removeAttr('validationgroup');
                        $('#<%=txtTaxInvoiceNo.ClientID %>').val('');

                        if (isVATMandatory == "1") {
                            $('#<%:trPPN.ClientID %>').removeAttr('style');
                        } else {
                            $('#<%=trPPN.ClientID %>').attr('style', 'display:none');
                        }
                        $('#<%:txtVATPercentageDefault.ClientID %>').attr('readonly', 'readonly');
                    }
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

            $('#<%=txtStamp.ClientID %>').change(function () {
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

            $('.lblExpiredDate').live('click', function () {
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

            if (isShowWatermark())
                $('#btnPurchaseReceive').attr('enabled', false);
            else {
                $('#btnPurchaseReceive').click(function () {
                    if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
                        var param = $('#<%=hdnSupplierID.ClientID %>').val() + '|' + $('#<%=hdnIsFilterPurchaseOrderNo.ClientID %>').val() + '|' + $('#<%=hdnProductLineID.ClientID %>').val() + '|' + $('#<%=hdnLocationID.ClientID %>').val() + '|' + $('#<%=hdnMenuType.ClientID %>').val() + '|' + $('#<%=hdnIsPORWithPriceInformation.ClientID %>').val();
                        var url = ResolveUrl("~/Program/Warehouse/PurchaseReceive/PurchaseReceiveDetailCtl.ascx");
                        openUserControlPopup(url, param, 'Quick Picks - Pemesanan Barang', 1300, 550);
                    }
                });
            }
        }

        //#region Discount Detail
        $('#<%=chkIsDiscountInPercentage1.ClientID %>').die('change');
        $('#<%:chkIsDiscountInPercentage1.ClientID %>').live('change', function () {
            if ($(this).is(':checked')) {
                $('#<%=txtDiscount.ClientID%>').removeAttr('readonly');
                $('#<%=txtDiscountAmount.ClientID%>').attr('readonly', 'readonly');

                $('#<%=txtDiscount.ClientID %>').trigger('change');
            } else {
                $('#<%=txtDiscount.ClientID%>').attr('readonly', 'readonly');
                $('#<%=txtDiscountAmount.ClientID%>').removeAttr('readonly');
            }
        });

        $('#<%=txtDiscount.ClientID %>').die('change');
        $('#<%=txtDiscount.ClientID %>').live('change', function () {
            if ($(this).val() == '' || parseFloat($(this).val()) > 100 || parseFloat($(this).val()) < 0) {
                $(this).val('0');
            }

            var price = parseFloat($('#<%=txtPrice.ClientID %>').val().replace('.00', '').split(',').join(''));
            var qty = parseFloat($('#<%=txtQuantity.ClientID %>').val().replace('.00', '').split(',').join(''));
            var subTotal = parseFloat(price * qty);
            var discountAmount = ((parseFloat($(this).val()) * subTotal) / 100);

            $('#<%=hdnDiscountAmount1.ClientID %>').val(discountAmount);
            $('#<%=txtDiscountAmount.ClientID %>').val(discountAmount).trigger('changeValue');

            $('#<%=txtDiscount2.ClientID %>').trigger('change');

            calculateSubTotal();
        });

        $('#<%=txtDiscountAmount.ClientID %>').die('change');
        $('#<%=txtDiscountAmount.ClientID %>').live('change', function () {
            var price = parseFloat($('#<%=txtPrice.ClientID %>').val().replace('.00', '').split(',').join(''));
            var qty = parseFloat($('#<%=txtQuantity.ClientID %>').val().replace('.00', '').split(',').join(''));
            var subTotal = parseFloat(price * qty);

            if (subTotal > 0) {
                if ($(this).val() == '' || parseFloat($(this).val()) > subTotal || parseFloat($(this).val()) < 0) {
                    $(this).val('0');
                    $('#<%=hdnDiscountAmount2.ClientID %>').val(0);
                    $('#<%=txtDiscountAmount2.ClientID %>').val(0).trigger('changeValue');
                }
            }

            var discountAmount = parseFloat($('#<%=txtDiscountAmount.ClientID %>').val().replace('.00', '').split(',').join(''));
            var discountPercent = parseFloat(discountAmount / subTotal * 100).toFixed(2);
            $('#<%=hdnDiscountAmount1.ClientID %>').val(discountAmount);

            $('#<%=txtDiscount.ClientID %>').val(discountPercent).trigger('changeValue');

            $('#<%=txtDiscount2.ClientID %>').trigger('change');

            calculateSubTotal();
        });

        $('#<%=chkIsDiscountInPercentage2.ClientID %>').die('change');
        $('#<%:chkIsDiscountInPercentage2.ClientID %>').live('change', function () {
            if ($(this).is(':checked')) {
                $('#<%=txtDiscount2.ClientID%>').removeAttr('readonly');
                $('#<%=txtDiscountAmount2.ClientID%>').attr('readonly', 'readonly');

                $('#<%=txtDiscount2.ClientID %>').trigger('change');
            } else {
                $('#<%=txtDiscount2.ClientID%>').attr('readonly', 'readonly');
                $('#<%=txtDiscountAmount2.ClientID%>').removeAttr('readonly');
            }
        });

        $('#<%=txtDiscount2.ClientID %>').die('change');
        $('#<%=txtDiscount2.ClientID %>').live('change', function () {
            if ($(this).val() == '' || parseFloat($(this).val()) > 100 || parseFloat($(this).val()) < 0) {
                $(this).val('0');
            }

            var price = parseFloat($('#<%=txtPrice.ClientID %>').val().replace('.00', '').split(',').join(''));
            var qty = parseFloat($('#<%=txtQuantity.ClientID %>').val().replace('.00', '').split(',').join(''));
            var subTotal = parseFloat(price * qty);

            var discAmount = parseFloat($('#<%=txtDiscountAmount.ClientID %>').attr('hiddenVal').replace('.00', '').split(',').join(''));
            if (!$('#<%:chkIsDiscountInPercentage1.ClientID %>').is(':checked')) {
                discAmount = parseFloat($('#<%=txtDiscountAmount.ClientID %>').val().replace('.00', '').split(',').join(''));
            }

            subTotal = subTotal - discAmount;
            var discountAmount2 = (parseFloat($(this).val()) * subTotal / 100);

            $('#<%=hdnDiscountAmount2.ClientID %>').val(discountAmount2);
            $('#<%=txtDiscountAmount2.ClientID %>').val(discountAmount2).trigger('changeValue');

            calculateSubTotal();

        });

        $('#<%=txtDiscountAmount2.ClientID %>').die('change');
        $('#<%=txtDiscountAmount2.ClientID %>').live('change', function () {
            var price = parseFloat($('#<%=txtPrice.ClientID %>').val().replace('.00', '').split(',').join(''));
            var qty = parseFloat($('#<%=txtQuantity.ClientID %>').val().replace('.00', '').split(',').join(''));
            var subTotal = parseFloat(price * qty);
            var discountAmount = parseFloat($('#<%=txtDiscountAmount.ClientID %>').val().replace('.00', '').split(',').join(''));
            var discountAmount2 = parseFloat($('#<%=txtDiscountAmount2.ClientID %>').val().replace('.00', '').split(',').join(''));
            var discountPercent2 = parseFloat(discountAmount2 / (subTotal - discountAmount) * 100).toFixed(2);
            $('#<%=hdnDiscountAmount2.ClientID %>').val(discountAmount2);
            $('#<%=txtDiscount2.ClientID %>').val(discountPercent2).trigger('changeValue');

            calculateSubTotal();
        });
        //#endregion

        //#region PPH Detail
        $('#<%:chkPPHPercentDetail.ClientID %>').live('change', function () {
            if ($(this).is(':checked')) {
                $('#<%=txtPPHPercentageDetail.ClientID %>').change();
                $('#<%=txtPPHPercentageDetail.ClientID%>').removeAttr('readonly');
                $('#<%=txtPPHAmountDetail.ClientID%>').attr('readonly', 'readonly');
            } else {
                $('#<%=txtPPHAmountDetail.ClientID %>').change();
                $('#<%=txtPPHPercentageDetail.ClientID%>').attr('readonly', 'readonly');
                $('#<%=txtPPHAmountDetail.ClientID%>').removeAttr('readonly');
            }
        });
        //#endregion

        function calculateSubTotal() {
            var total = 0;
            var PPHAmount = 0;

            var price = parseFloat($('#<%=txtPrice.ClientID %>').attr('hiddenVal'));
            var qty = parseFloat($('#<%=txtQuantity.ClientID %>').val());
            var subTotal = price * qty;
            var discount1 = parseFloat($('#<%=txtDiscount.ClientID %>').val());
            var discountAmount1 = parseFloat($('#<%=txtDiscountAmount.ClientID %>').val().replace('.00', '').split(',').join(''));
            var discount2 = parseFloat($('#<%=txtDiscount2.ClientID %>').val());
            var discountAmount2 = parseFloat($('#<%=txtDiscountAmount2.ClientID %>').val().replace('.00', '').split(',').join(''));
            var PPHAmountPercent = parseFloat($('#<%=txtPPHPercentageDetail.ClientID %>').attr('hiddenVal'));

            subTotal = subTotal - discountAmount1 - discountAmount2;

            var PPHAmountDetail = subTotal * (PPHAmountPercent / 100);

            if (cboPPHOptionsDetail.GetValue() == 'Plus') {
                PPHAmount = PPHAmountDetail;
            } else {
                PPHAmount = PPHAmountDetail * -1;
            }

            $('#<%=txtPPHAmountDetail.ClientID %>').val(PPHAmount.toFixed(2));

            total = subTotal;

            $('#<%=txtSubTotalPrice.ClientID %>').val(total).trigger('changeValue');
            var totalOrder = lastTransactionAmount - editedLineAmount + total;
            $('#<%=txtTotalOrder.ClientID %>').val(totalOrder).trigger('changeValue');

            calculateTotal();
        }

        function calculateTotal() {
            var totalKotor = parseFloat($('#<%=txtTotalOrder.ClientID %>').attr('hiddenVal'));
            var Discount = parseFloat($('#<%=txtFinalDiscount.ClientID %>').attr('hiddenVal'));
            var PPH = parseFloat($('#<%=txtPPHPI.ClientID %>').attr('hiddenVal'));

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
            var Stamp = parseFloat($('#<%=txtStamp.ClientID %>').attr('hiddenVal'));
            var PPHPercentage = (PPH * 100) / totalKotor;
            var totalHarga = totalKotor - (Discount + DP) + PPN + PPH + Charge + Stamp;

            $('#<%=txtPPH.ClientID %>').val(PPHPercentage).trigger('changeValue');
            $('#<%=txtTotalOrderSaldo.ClientID %>').val(totalHarga).trigger('changeValue');

        }

        //#region Paging
        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                var totalPurchase = parseFloat(param[2]);
                var totalPPH = parseFloat(param[3]);
                $('#<%=txtTotalOrder.ClientID %>').val(totalPurchase).trigger('changeValue');
                $('#<%=txtPPHPI.ClientID %>').val(totalPPH).trigger('changeValue');
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
                if (param[1] == 'fail') {
                    showToast('Save Failed', 'Error Message : ' + param[2]);
                }
                else {
                    var PRID = s.cpOrderID;
                    onAfterSaveRecordDtSuccess(PRID);
                    var isEdit = $('#<%=hdnIsEdit.ClientID %>').val();
                    if (isEdit == '1') {
                        $('#containerEntry').hide();
                    } else if (isEdit == '0') {
                        $('#lblAddData').click();
                    }
                    cbpView.PerformCallback('refresh');
                }
            }
            else if (param[0] == 'delete') {
                if (param[1] == 'fail') {
                    showToast('Delete Failed', 'Error Message : ' + param[2]);
                }
                else {
                    $('#containerEntry').hide();
                    cbpView.PerformCallback('refresh');
                }
            }
        }

        //#region cbo Item Unit
        function onCboItemUnitEndCallBack(s) {
            var param = s.cpResult.split('|');
            cboItemUnit.SetValue($('#<%=hdnGCItemUnit.ClientID %>').val());
            onCboItemUnitChanged(param[0]);
        }

        function onCboItemUnitChanged(paramProcess) {
            var baseValue = $('#<%=hdnGCBaseUnit.ClientID %>').val();
            var toUnitItem = cboItemUnit.GetValue();
            var baseText = getItemUnitName(baseValue);

            var factor = 1;
            var factorOld = $('#<%=hdnTempFactor.ClientID %>').val();
            var qtyBegin = $('#<%=txtQuantity.ClientID %>').val();
            var qtyEnd = 1;

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
            var conversion = parseFloat($('#<%=hdnConversionFactor.ClientID %>').val());
            var priceperitemunit = parseFloat(($('#<%=hdnUnitPrice.ClientID %>').val()));
            var pricePerPurchaseUnit = conversion * priceperitemunit;

            if (factor != 1) {
                if (factorOld == 1) {
                    qtyEnd = parseFloat((qtyBegin / factor).toFixed(2));
                } else {
                    qtyEnd = qtyBegin;
                }
            } else {
                if (factorOld != 0) {
                    qtyEnd = parseFloat((qtyBegin * factorOld).toFixed(2));
                } else {
                    qtyEnd = qtyBegin;
                }
            }

            $('#<%=txtQuantity.ClientID %>').val(qtyEnd);
            $('#<%=hdnTempFactor.ClientID %>').val(factor);

            $('#<%=txtPrice.ClientID %>').val(pricePerPurchaseUnit).trigger('changeValue');

            if (paramProcess != "edit") {
                $('#<%=txtQuantity.ClientID %>').trigger('change');
                $('#<%=txtDiscount.ClientID %>').trigger('change');
            }
        }

        function getItemUnitName(baseValue) {
            var value = cboItemUnit.GetValue();
            cboItemUnit.SetValue(baseValue);
            var text = cboItemUnit.GetText();
            cboItemUnit.SetValue(value);
            return text;
        }
        //#endregion

        function setRightPanelButtonEnabled() {
            var param = $('#<%:hdnPRID.ClientID %>').val();
            if (param != null && param != "0") {
                $('#btnUpdateTax').removeAttr('enabled');
                $('#btnUpdateReference').removeAttr('enabled');
                $('#btnUpdateEDAndBatchNo').removeAttr('enabled');
                $('#btnInvoiceInfo').removeAttr('enabled');
                $('#btnInvoiceInfoWithoutPrice').removeAttr('enabled');
            } else {
                $('#btnUpdateTax').attr('enabled', 'false');
                $('#btnUpdateReference').attr('enabled', 'false');
                $('#btnUpdateEDAndBatchNo').attr('enabled', 'false');
                $('#btnInvoiceInfo').attr('enabled', 'false');
                $('#btnInvoiceInfoWithoutPrice').attr('enabled', 'false');
            }
        }

        function onBeforeLoadRightPanelContent(code) {
            if (code == 'updateTax' || code == 'updateReference' || code == 'updateEDAndBatchNo' || code == 'infoInvoice' || code == 'infoInvoiceWithoutPrice') {
                var param = $('#<%:hdnPRID.ClientID %>').val();
                return param;
            }
        }

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var purchaseReceiveID = $('#<%=hdnPRID.ClientID %>').val();
            var printStatus = $('#<%=hdnPrintStatus.ClientID %>').val();
            var transactionStatus = $('#<%=hdnGCTransactionStatus.ClientID %>').val();

            if (printStatus == 'true') {
                if (purchaseReceiveID == '' || purchaseReceiveID == '0') {
                    errMessage.text = 'Please Set Transaction First!';
                    return false;
                }
                else if (code == 'IM-00107' || code == 'IM-00120') {
                    if (transactionStatus == Constant.TransactionStatus.APPROVED || transactionStatus == Constant.TransactionStatus.CLOSED) {
                        filterExpression.text = "PurchaseReceiveID = " + purchaseReceiveID;
                        return true;
                    }
                    else {
                        errMessage.text = "Data Doesn't Approved or Closed";
                        return false;
                    }
                }
                else {
                    filterExpression.text = "PurchaseReceiveID = " + purchaseReceiveID;
                    return true;
                }
            }
            else {
                if (purchaseReceiveID == '' || purchaseReceiveID == '0') {
                    errMessage.text = 'Please Set Transaction First!';
                    return false;
                }
                else if (code == 'IM-00045') {
                    filterExpression.text = "PurchaseReceiveID = " + purchaseReceiveID;
                    return true;
                }
                else {
                    errMessage.text = "Data Doesn't Approved or Closed";
                    return false;
                }
            }
        }

        function onTxtFacturNoChanged() {
            var refNo = $('#<%=txtFacturNo.ClientID %>').val();
            var prNo = $('#<%=hdnPRID.ClientID %>').val();
            var isDontAllowDuplicateFacturNo = $('#<%=hndIsDontAllowDuplicateFacturNo.ClientID %>').val();

            if (isDontAllowDuplicateFacturNo == '1') {
                var filterExpression = "ReferenceNo = '" + refNo + "' AND GCTransactionStatus != '" + Constant.TransactionStatus.VOID + "'";

                if (prNo != 0) {
                    filterExpression += " AND PurchaseReceiveNo != '" + prNo + "'";
                }

                Methods.getObject('GetPurchaseReceiveHdList', filterExpression, function (result) {
                    if (result != null) {
                        showToast('Warning', 'No Faktur Sudah Ada.');
                    }
                });
            }
        }

    </script>
    <input type="hidden" value="" id="hdnIsShowDefaultLocation" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnPrintStatus" runat="server" />
    <input type="hidden" value="" id="hdnVATPercentageFromSetvar" runat="server" />
    <input type="hidden" value="" id="hdnVATPercentage" runat="server" />
    <input type="hidden" value="" id="hdnPRID" runat="server" />
    <input type="hidden" value="" id="hdnPageCount" runat="server" />
    <input type="hidden" value="" id="hdnIsEditable" runat="server" />
    <input type="hidden" value="0" id="hdnNeedConfirmation" runat="server" />
    <input type="hidden" value="0" id="hdnIsDiscountAppliedToAveragePrice" runat="server" />
    <input type="hidden" value="0" id="hdnIsDiscountAppliedToUnitPrice" runat="server" />
    <input type="hidden" value="0" id="hdnIsPPNAppliedToAveragePrice" runat="server" />
    <input type="hidden" value="0" id="hdnIsPPNAppliedToUnitPrice" runat="server" />
    <input type="hidden" value="0" id="hdnIsFilterPurchaseOrderNo" runat="server" />
    <input type="hidden" value="0" id="hdnAllowPORQtyBiggerThanPO" runat="server" />
    <input type="hidden" value="0" id="hdnTempFactor" runat="server" />
    <input type="hidden" value="0" id="hdnTempFactorORI" runat="server" />
    <input type="hidden" value="" id="hdnRecordFilterExpression" runat="server" />
    <input type="hidden" value="0" id="hdnIsEdit" runat="server" />
    <input type="hidden" value="0" id="hdnIsUsedProductLine" runat="server" />
    <input type="hidden" value="0" id="hdnIsPPhDetailEdit" runat="server" />
    <input type="hidden" value="0" id="hdnPPhPercentage" runat="server" />
    <input type="hidden" value="1" id="hdnIsAutoUpdateToSupplierItem" runat="server" />
    <input type="hidden" value="" id="hdnIsAllowBackdatePOR" runat="server" />
    <input type="hidden" value="" id="hndIsDontAllowDuplicateFacturNo" runat="server" />
    <input type="hidden" value="0" id="hdnIsVATMandatory" runat="server" />
    <input type="hidden" value="" id="hdnReferenceDate" runat="server" />
    <input type="hidden" value="" id="hdnGCTransactionStatus" runat="server" />
    <input type="hidden" value="" id="hdnMenuType" runat="server" />
    <input type="hidden" value="" id="hdnIsPORWithPriceInformation" runat="server" />
    <input type="hidden" value="" id="hdnChangeQtyPOR" runat="server" />
    <input type="hidden" value="0" id="hdnIsPpnAllowChanged" runat="server" />
    <input type="hidden" value="0" id="hdnIsUsingPurchaseDiscountShared" runat="server" />
    <input type="hidden" value="0" id="hdnDefaultValueForNoFakturPajak" runat="server" />
    <input type="hidden" value="0" id="hdnIsUsingDownPayment" runat="server" />
    <input type="hidden" value="0" id="hdnIsReceiveUsingBaseUnit" runat="server" />
    <input type="hidden" value="1" id="hdnKapanPerubahanNilaiHargaKeItemPlanning" runat="server" />
    <input type="hidden" id="hdnIsValidateVAT" runat="server" value="0" />
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
                                <asp:TextBox ID="txtPurchaseReceiveNo" Width="150px" ReadOnly="true" runat="server" />
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
                                            <asp:TextBox ID="txtFacturNo" CssClass="required" onchange="onTxtFacturNoChanged();"
                                                ValidationGroup="mpEntry" Width="100%" runat="server" />
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
                                <%=GetLabel("Tanggal Faktur/Kirim") %>
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
                                <dxe:ASPxComboBox ID="cboTerm" ClientInstanceName="cboTerm" Width="250px" runat="server" />
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
                        <tr style="display: none" id="trTaxInvoice" runat="server">
                            <td colspan="2">
                                <table width="100%">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblMandatory">
                                                <%=GetLabel("No. Faktur Pajak") %></label>
                                        </td>
                                        <td style="width: 150px">
                                            <asp:TextBox ID="txtTaxInvoiceNo" Width="120px" runat="server" />
                                        </td>
                                        <td class="tdLabel">
                                            <%=GetLabel("Tanggal Faktur") %>
                                        </td>
                                        <td>
                                            <table cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td style="padding-right: 1px; width: 145px">
                                                        <asp:TextBox ID="txtTaxInvoiceDate" Width="120px" CssClass="datepicker" runat="server" />
                                                    </td>
                                                    <td style="width: 5px">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr style="display: none" id="trPaymentDueDate" runat="server">
                            <td class="tdLabel">
                                <%=GetLabel("Tanggal Jatuh Tempo") %>
                            </td>
                            <td style="padding-right: 1px; width: 140px">
                                <asp:TextBox ID="txtPaymentDueDate" Width="120px" CssClass="datepicker" runat="server" />
                            </td>
                        </tr>
                        <tr id="trPPN" runat="server" style="display: none">
                            <td>
                            </td>
                            <td>
                                <table class="tblEntryContent" style="width: 100%">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td rowspan="1" style="vertical-align: top">
                                            <table id="tblInfoPPN" runat="server">
                                                <tr>
                                                    <td style="vertical-align: top">
                                                        <img height="40" src='<%= ResolveUrl("~/Libs/Images/Warning.png")%>' alt='' />
                                                    </td>
                                                    <td style="vertical-align: middle">
                                                        <label class="lblWarning" id="lblInfoPPN" width="100%" runat="server">
                                                            <%=GetLabel("Harap centang PPN, karena PO memiliki PPN") %></label>
                                                        <br />
                                                    </td>
                                                </tr>
                                            </table>
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
                                                    <asp:CheckBox ID="chkIsBonus" Width="100%" runat="server" Checked="true" Enabled="false"
                                                        Text="Bonus" />
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
                                                    <label class="lblLink" id="lblItemGroup" runat="server">
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
                                                    <label class="lblLink lblMandatory" id="lblItem" runat="server">
                                                        <%=GetLabel("Item")%></label>
                                                </td>
                                                <td colspan="2">
                                                    <input type="hidden" value="" id="hdnItemID" runat="server" />
                                                    <input type="hidden" value="" id="hdnGCBaseUnit" runat="server" />
                                                    <input type="hidden" value="" id="hdnGCItemUnit" runat="server" />
                                                    <input type="hidden" value="" id="hdnConversionFactor" runat="server" />
                                                    <input type="hidden" value="" id="hdnUnitPrice" runat="server" />
                                                    <input type="hidden" value="" id="hdnReceivedQty" runat="server" />
                                                    <input type="hidden" value="" id="hdnQtyNow" runat="server" />
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
                                                                    Width="150px" OnCallback="cboItemUnit_Callback">
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
                                            <tr id="trPrice" runat="server">
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
                                            <tr id="trDiscount" runat="server">
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
                                            <tr id="trDiscount2" runat="server">
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
                                                        <%=GetLabel("Jenis PPh")%></label>
                                                </td>
                                                <td class="tdLabel" style="text-align: right;">
                                                    <dxe:ASPxComboBox ID="cboPPhTypeDetail" ClientInstanceName="cboPPhTypeDetail" runat="server"
                                                        Width="200px">
                                                    </dxe:ASPxComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width: 20%" />
                                                            <col style="width: 5%" />
                                                            <col style="width: 20%" />
                                                        </colgroup>
                                                        <tr>
                                                            <td class="tdLabel" style="text-align: right; width: 100px;">
                                                                <dxe:ASPxComboBox ID="cboPPHOptionsDetail" ClientInstanceName="cboPPHOptionsDetail"
                                                                    runat="server" Width="80px">
                                                                    <ClientSideEvents ValueChanged="function(s,e) { oncboPPHOptionsDetailValueChanged(e); }"
                                                                        SelectedIndexChanged="calculateTotal" />
                                                                </dxe:ASPxComboBox>
                                                            </td>
                                                            <td style="width: 5px">
                                                                <asp:CheckBox ID="chkPPHPercentDetail" Checked="true" runat="server" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox class="txtPPHPercentageDetail txtCurrency" ID="txtPPHPercentageDetail"
                                                                    Width="70px" runat="server" hiddenVal="0" />
                                                                %
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtPPHAmountDetail" CssClass="txtCurrency" Width="100px" runat="server"
                                                                    hiddenVal="0" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr id="trSubTotalPrice" runat="server">
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Total Harga")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtSubTotalPrice" Width="120px" ReadOnly="true" runat="server" CssClass="txtCurrency" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Catatan")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtRemarksDetail" Width="380px" TextMode="MultiLine" Rows="3" runat="server" />
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
                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField"
                                                HeaderStyle-Width="70px" />
                                            <asp:TemplateField HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center">
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
                                                    <input type="hidden" value="<%#:Eval("PurchaseOrderID") %>" bindingfield="PurchaseOrderID" />
                                                    <input type="hidden" value="<%#:Eval("PurchaseOrderNo") %>" bindingfield="PurchaseOrderNo" />
                                                    <input type="hidden" value="<%#:Eval("ItemID") %>" bindingfield="ItemID" />
                                                    <input type="hidden" value="<%#:Eval("ItemCode") %>" bindingfield="ItemCode" />
                                                    <input type="hidden" value="<%#:Eval("ItemName1") %>" bindingfield="ItemName1" />
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
                                                    <input type="hidden" value="<%#:Eval("GCPPHType") %>" bindingfield="GCPPHType" />
                                                    <input type="hidden" value="<%#:Eval("PPHMode") %>" bindingfield="PPHMode" />
                                                    <input type="hidden" value="<%#:Eval("IsPPHInPercentage") %>" bindingfield="IsPPHInPercentage" />
                                                    <input type="hidden" value="<%#:Eval("PPHPercentage") %>" bindingfield="PPHPercentage" />
                                                    <input type="hidden" value="<%#:Eval("PPHAmount") %>" bindingfield="PPHAmount" />
                                                    <input type="hidden" value="<%#:Eval("CustomSubTotal") %>" bindingfield="CustomSubTotal" />
                                                    <input type="hidden" value="<%#:Eval("RemarksDetail") %>" bindingfield="RemarksDetail" />
                                                    <input type="hidden" value="<%#:Eval("ReceivedQuantity") %>" bindingfield="ReceivedQuantity" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center" HeaderText="Bonus"
                                                HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkIsBonus" Enabled="false" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Item" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                                                HeaderStyle-Width="250px">
                                                <ItemTemplate>
                                                    <div style="padding: 1px">
                                                        <div>
                                                            <span style="font-style: normal; font-weight: bold">
                                                                <%#: Eval("ItemName1")%></div>
                                                        <div>
                                                            <span style="font-style: italic;">
                                                                <%#: Eval("ItemCode")%></span></div>
                                                        <div>
                                                            <span style="font-style: normal;">
                                                                <%#: Eval("PurchaseOrderNo")%></span></div>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="70px" HeaderText="Diterima" ItemStyle-HorizontalAlign="Right"
                                                HeaderStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <%#:Eval("Quantity") %><br />
                                                    <%#:Eval("ItemUnit")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="70px" HeaderText="Dipesan" ItemStyle-HorizontalAlign="Right"
                                                HeaderStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <%#:Eval("OrderQuantity")%><br />
                                                    <span style="width: 60px">
                                                        <%#:Eval("OrderPurchaseUnit")%>
                                                    </span>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="CustomConversion" HeaderText="Konversi" HeaderStyle-Width="150px"
                                                ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                            <asp:TemplateField HeaderStyle-Width="130px" HeaderText="Harga / Satuan" ItemStyle-HorizontalAlign="Right"
                                                HeaderStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <label runat="server" id="lblCustomUnitPrice" class="lblCustomUnitPrice">
                                                        <%#:Eval("CustomUnitPrice")%>
                                                    </label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Disc1(%)" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="50px">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkIsDiscInPct1" runat="server" CssClass="chkIsDiscInPct1" Enabled="false" />
                                                    <label runat="server" id="lblDiscountPercentage1" class="lblDiscountPercentage1">
                                                        <%#:Eval("DiscountPercentage1", "{0:N2}")%>
                                                    </label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Disc1 (Rp)" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="80px">
                                                <ItemTemplate>
                                                    <label runat="server" id="lblDiscountAmount1" class="lblDiscountAmount1">
                                                        <%#:Eval("DiscountAmount1", "{0:N2}")%>
                                                    </label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Disc2(%)" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="50px">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkIsDiscInPct2" runat="server" CssClass="chkIsDiscInPct2" Enabled="false" />
                                                    <label runat="server" id="lblDiscountPercentage2" class="lblDiscountPercentage2">
                                                        <%#:Eval("DiscountPercentage2", "{0:N2}")%>
                                                    </label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Disc2 (Rp)" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="80px">
                                                <ItemTemplate>
                                                    <label runat="server" id="lblDiscountAmount2" class="lblDiscountAmount2">
                                                        <%#:Eval("DiscountAmount2", "{0:N2}")%>
                                                    </label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="PPH" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="50px">
                                                <ItemTemplate>
                                                    <label runat="server" id="lblPPHAmount" class="lblPPHAmount">
                                                        <%#:Eval("PPHAmount", "{0:N2}")%>
                                                    </label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Sub Total" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="100px">
                                                <ItemTemplate>
                                                    <label runat="server" id="lblCustomSubTotal" class="lblCustomSubTotal">
                                                        <%#:Eval("LineAmount", "{0:N2}")%>
                                                    </label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="150px" HeaderText="[No.Batch|Exp.Date|Qty]"
                                                ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <label <%# IsEditable() == "1" ? "class='lblExpiredDate lblLink'":"class='lblExpiredDate lblLink lblDisabled'" %>>
                                                        <%=GetLabel("Expired Date")%></label>
                                                    <br />
                                                    <%#:Eval("BatchNoExpiredDate")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="CreatedByName" HeaderText="Penerima" HeaderStyle-Width="80px"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <img src='<%# ResolveUrl("~/Libs/Images/Button/verify.png") %>' <%# Eval("isConfirmed").ToString() == "True" ? "" : "Style ='display:none'" %>
                                                        title='<%=GetLabel("Confirmed") %>' alt="" />
                                                    <img src='<%# ResolveUrl("~/Libs/Images/Status/warning.png") %>' <%# Eval("IsNeedConfirmation").ToString() == "True" ? "" : "Style ='display:none'" %>
                                                        title='<%=GetLabel("Need Confirmation") %>' alt="" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Tidak ada data penerimaan pembelian")%>
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
                        <span class="lblLink" id="lblAddData">
                            <%= GetLabel("Tambah Bonus Item")%></span>
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
                                            <tr id="trTotalOrder" runat="server">
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Jumlah Nilai Pembelian")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtTotalOrder" CssClass="txtCurrency" ReadOnly="true" Width="180px"
                                                        runat="server" />
                                                </td>
                                            </tr>
                                            <tr id="trDiscountFinalPercent" runat="server" style="display: none">
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Diskon Final %")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtFinalDiscountInPercentage" CssClass="txtCurrency" Width="180px"
                                                        runat="server" />
                                                </td>
                                            </tr>
                                            <tr id="trDiscountFinal" runat="server" style="display: none">
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
                                            <tr style="display: none">
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Jenis PPh")%></label>
                                                </td>
                                                <td class="tdLabel" style="text-align: right;">
                                                    <dxe:ASPxComboBox ID="cboPPhType" ClientInstanceName="cboPPhType" runat="server"
                                                        Width="185px">
                                                    </dxe:ASPxComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="lblNormal">
                                                        <%=GetLabel("PPh")%></label>
                                                </td>
                                                <td>
                                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width: 5%" />
                                                            <col style="width: 4%" />
                                                            <col style="width: 15%" />
                                                            <col style="width: 40%" />
                                                            <col />
                                                        </colgroup>
                                                        <tr>
                                                            <td class="tdLabel" style="display: none; text-align: right; width: 100px">
                                                                <dxe:ASPxComboBox ID="cboPPHOptions" ClientInstanceName="cboPPHOptions" runat="server"
                                                                    Width="80px">
                                                                </dxe:ASPxComboBox>
                                                            </td>
                                                            <td style="display: none; width: 5px">
                                                                <asp:CheckBox ID="chkPPHPercent" Checked="true" runat="server" />
                                                            </td>
                                                            <td style="display: none">
                                                                <asp:TextBox class="txtPPH" ReadOnly="true" CssClass="txtCurrency" ID="txtPPH" Width="70px"
                                                                    runat="server" hiddenVal="0" />
                                                                %
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtPPHPI" ReadOnly="true" CssClass="txtCurrency" Width="180px" runat="server"
                                                                    hiddenVal="0" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr id="trDPReferrenceNo" runat="server">
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("No. Reff Uang Muka")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDPReferrenceNo" Width="180px" runat="server" />
                                                </td>
                                            </tr>
                                            <tr id="trDP" runat="server">
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Uang Muka")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDP" CssClass="txtCurrency" Width="180px" runat="server" />
                                                </td>
                                            </tr>
                                            <tr id="trCharges" runat="server">
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Biaya Ongkos Kirim")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtCharges" CssClass="txtCurrency" Width="180px" runat="server" />
                                                </td>
                                            </tr>
                                            <tr id="trStamp" runat="server">
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Biaya Materai")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtStamp" CssClass="txtCurrency" Width="180px" runat="server" />
                                                </td>
                                            </tr>
                                            <tr id="trTotalOrderSaldo" runat="server">
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
