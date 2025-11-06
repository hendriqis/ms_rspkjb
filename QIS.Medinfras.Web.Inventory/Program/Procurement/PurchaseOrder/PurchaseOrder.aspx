<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx2.master"
    AutoEventWireup="true" CodeBehind="PurchaseOrder.aspx.cs" Inherits="QIS.Medinfras.Web.Inventory.Program.PurchaseOrder" %>

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

        function onLoadCurrentRecord() {
            onLoadObject($('#<%=txtOrderNo.ClientID %>').val());
        }

        function onLoad() {
            if ($('#<%=hdnIsEditable.ClientID %>').val() == '1') {
                $('#lblSalinPR').show();
                $('#lblAddData').show();
            }
            else {
                $('#lblSalinPR').hide();
                $('#lblAddData').hide();
            }

            if ($('#<%=hdnOrderID.ClientID %>').val() == "" || $('#<%=hdnOrderID.ClientID %>').val() == "0") {
                setDatePicker('<%=txtItemOrderDate.ClientID %>');
            }

            setDatePicker('<%=txtItemOrderDeliveryDate.ClientID %>');

            setDatePicker('<%=txtItemOrderExpiredDate.ClientID %>');

            if ($('#<%=chkIsFinalDiscountInPercentage.ClientID %>').is(':checked')) {
                if ($('#<%=hdnGCTransactionStatus.ClientID %>').val() == "X121^001") {
                    $('#<%=txtFinalDiscountInPercentage.ClientID%>').change();
                    $('#<%=txtFinalDiscountInPercentage.ClientID%>').removeAttr('readonly');
                    $('#<%=txtFinalDiscount.ClientID%>').attr('readonly', 'readonly');
                }
            } else {
                if ($('#<%=hdnGCTransactionStatus.ClientID %>').val() == "X121^001") {
                    $('#<%=txtFinalDiscount.ClientID%>').change();
                    $('#<%=txtFinalDiscountInPercentage.ClientID%>').attr('readonly', 'readonly');
                    $('#<%=txtFinalDiscount.ClientID%>').removeAttr('readonly');
                }
            }

            if ($('#<%=chkIsPPHInPercentage.ClientID %>').is(':checked')) {
                if ($('#<%=hdnGCTransactionStatus.ClientID %>').val() == "X121^001") {
                    $('#<%=txtPPHPercentage.ClientID %>').change();
                    $('#<%=txtPPHPercentage.ClientID%>').removeAttr('readonly');
                    $('#<%=txtPPHAmount.ClientID%>').attr('readonly', 'readonly');
                }
            } else {
                if ($('#<%=hdnGCTransactionStatus.ClientID %>').val() == "X121^001") {
                    $('#<%=txtPPHAmount.ClientID %>').change();
                    $('#<%=txtPPHPercentage.ClientID%>').attr('readonly', 'readonly');
                    $('#<%=txtPPHAmount.ClientID%>').removeAttr('readonly');
                }
            }

            //#region Order No
            $('#lblOrderNo.lblLink').live('click', function () {
                var locID = $('#<%=hdnLocationID.ClientID %>').val();
                if (locID != "" && locID != null) {
                    var filterExpression = 'LocationID = ' + locID + ' AND TransactionCode = 4202';
                    openSearchDialog($('#<%:hdnSearchDialogType.ClientID %>').val(), filterExpression, function (value) {
                        $('#<%=txtOrderNo.ClientID %>').val(value);
                        onTxtOrderNoChanged(value);
                    });
                } else {
                    displayErrorMessageBox("Pencarian Gagal", "Harap pilih lokasi terlebih dahulu.");
                }
            });

            $('#<%=txtOrderNo.ClientID %>').live('change', function () {
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

            $('#<%=txtSupplierCode.ClientID %>').live('change', function () {
                onTxtSupplierChanged($(this).val());
            });

            function onTxtSupplierChanged(value) {
                var isEditablePPN = $('#<%=hdnIsPpnAllowChanged.ClientID %>').val();
                var filterExpression = getSupplierFilterExpression() + " AND BusinessPartnerCode = '" + value + "'";
                Methods.getObject('GetBusinessPartnersList', filterExpression, function (result) {
                    if (result != null) {
                        var hdnSupplier = $('#<%=hdnSupplierID.ClientID %>').val();
                        var orderID = $('#<%=hdnOrderID.ClientID %>').val();

                        if (hdnSupplier == '' || hdnSupplier == 0 || hdnSupplier == null) {
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

                            if (orderID != 0) {
                                $('#<%=tdRecalculate.ClientID %>').removeAttr('style');
                                $('#<%=trWarning.ClientID %>').removeAttr('style');
                            }
                        }

                        Methods.getDateDiffPOPORPerSupplier($('#<%=hdnSupplierID.ClientID %>').val(), function (result) {
                            if (result != null) {
                                $('#<%=txtETA.ClientID %>').val(result.Hasil + ' Hari');
                            }
                            else {
                                $('#<%=txtETA.ClientID %>').val('');
                            }
                        });
                    }
                    else {
                        $('#<%=hdnSupplierID.ClientID %>').val('');
                        $('#<%=txtSupplierCode.ClientID %>').val('');
                        $('#<%=txtSupplierName.ClientID %>').val('');
                        cboTerm.SetSelectedIndex(0);
                        $('#<%=txtETA.ClientID %>').val('');
                    }
                });
            }
            //#endregion

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

            //#region Revenue Cost Center
            function onRevenueCostCenterFilterExpression() {
                var filterExpression = "IsDeleted = 0";
                return filterExpression;
            }

            $('#<%=lblRevenueCostCenter.ClientID %>.lblLink').live('click', function () {
                openSearchDialog('revenuecostcenter', onRevenueCostCenterFilterExpression(), function (value) {
                    $('#<%=txtRevenueCostCenterCode.ClientID %>').val(value);
                    ontxtRevenueCostCenterCodeChanged(value);
                });
            });

            $('#<%=txtRevenueCostCenterCode.ClientID %>').live('change', function () {
                var param = $('#<%=txtRevenueCostCenterCode.ClientID %>').val();
                ontxtRevenueCostCenterCodeChanged(param);
            });

            function ontxtRevenueCostCenterCodeChanged(value) {
                var filterExpression = "RevenueCostCenterCode = '" + $('#<%=txtRevenueCostCenterCode.ClientID %>').val() + "'";
                Methods.getObject('GetRevenueCostCenterList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnRevenueCostCenterID.ClientID %>').val(result.RevenueCostCenterID);
                        $('#<%=txtRevenueCostCenterName.ClientID %>').val(result.RevenueCostCenterName);
                    }
                    else {
                        $('#<%=hdnRevenueCostCenterID.ClientID %>').val('');
                        $('#<%=txtRevenueCostCenterCode.ClientID %>').val('');
                        $('#<%=txtRevenueCostCenterName.ClientID %>').val('');
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
                            filterExpression += " AND GCItemType IN ('X001^002','X001^003','X001^008','X001^009')";
                        }
                    } else {
                        filterExpression += " AND GCItemType IN ('X001^002','X001^003','X001^008','X001^009')";
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
                var PRInfo = $('#<%=hdnPurchaseRequestNoInfo.ClientID %>').val();
                if (PRInfo == null || PRInfo == "") {
                    var filterExpression = onGetItemGroupFilterExpression() + " AND ItemGroupCode = '" + value + "'";
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

                    $('#<%=txtQuantity.ClientID %>').val('1');
                    $('#<%=hdnEntryID.ClientID %>').val('');
                    $('#<%=hdnPurchaseRequestNoInfo.ClientID %>').val('');
                    $('#<%=hdnItemID.ClientID %>').val('');
                    $('#<%=hdnGCItemUnit.ClientID %>').val('');
                    $('#<%=hdnGCBaseUnit.ClientID %>').val('');
                    $('#<%=txtItemGroupCode.ClientID %>').val('');
                    $('#<%=txtItemGroupName.ClientID %>').val('');
                    $('#<%=txtItemCode.ClientID %>').val('');
                    $('#<%=txtItemName.ClientID %>').val('');
                    $('#<%=hdnUnitPrice.ClientID %>').val('0');
                    $('#<%=hdnPurchaseUnitPrice.ClientID %>').val('0');

                    $('#<%=txtQtyOnProcess.ClientID %>').val('');
                    $('#<%=hdnDiscountAmount1.ClientID %>').val('0');
                    $('#<%=hdnDiscountAmount2.ClientID %>').val('0');
                    $('#<%=txtDiscount.ClientID %>').val('0');
                    $('#<%=txtDiscount2.ClientID %>').val('0');
                    $('#<%=txtDiscountAmount.ClientID %>').val('0').trigger('changeValue');
                    $('#<%=txtDiscountAmount2.ClientID %>').val('0').trigger('changeValue');

                    $('#<%=chkIsDiscountInPercentage1.ClientID %>').prop('checked', true);
                    $('#<%=chkIsDiscountInPercentage2.ClientID %>').prop('checked', true);

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

                    $('#<%=txtPrice.ClientID %>').val('');
                    $('#<%=txtBaseUnit.ClientID %>').val('');
                    $('#<%=txtNotesDt.ClientID %>').val('');
                    $('#<%=txtSupplierItemCode.ClientID %>').val('');
                    $('#<%=txtSupplierItemName.ClientID %>').val('');
                    $('#<%=txtSubTotalPrice.ClientID %>').val('').trigger('changeValue');
                    $('#<%=txtSupplierCode.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=lblProductLine.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%=txtProductLineCode.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=lblLocation.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%=txtLocationCode.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtStockLocation.ClientID %>').val('');
                    lastTransactionAmount = $('#<%=txtTotalOrder.ClientID %>').attr('hiddenVal');
                    editedLineAmount = 0;
                    cboItemUnit.SetValue('');
                    $('#<%=txtConversion.ClientID %>').val('');
                    $('#<%=hdnTempFactor.ClientID %>').val('0');
                    $('#<%=hdnConversionFactor.ClientID %>').val('1');

                } else {
                    var messageInfo = "Dilarang mengubah kelompok item <b>" + $('#<%=txtItemGroupName.ClientID %>').val() + "</b> karena detail pemesanan ini berasal dari permintaan nomor <b>" + PRInfo + "</b>.";
                    displayMessageBox('Information', messageInfo);
                }
            }
            //#endregion

            //#region Item
            function getItemFilterExpression() {
                var ProductLineID = $('#<%=hdnProductLineID.ClientID %>').val();
                var GCItemType = $('#<%=hdnProductLineItemType.ClientID %>').val();
                var GCLocationGroup = $('#<%=hdnGCLocationGroup.ClientID %>').val();
                var filterExpression = "IsDeleted = 0 AND GCItemStatus != 'X181^999'";
                var orderID = $('#<%=hdnOrderID.ClientID %>').val();
                var GCPurchasingType = $('#<%=hdnGCPurchasingType.ClientID %>').val();

                if ($('#<%=txtItemGroupCode.ClientID %>').val() != '') {
                    filterExpression += " AND ItemGroupID IN (SELECT ItemGroupID FROM vItemGroupMaster1 WHERE DisplayPath like '%/" + $('#<%=hdnItemGroupID.ClientID %>').val() + "/%')";
                }
                else if ($('#<%=hdnLocationItemGroupID.ClientID %>').val() != '' && $('#<%=hdnLocationItemGroupID.ClientID %>').val() != '0') {
                    filterExpression += " AND ItemGroupID IN (SELECT ItemGroupID FROM vItemGroupMaster1 WHERE DisplayPath like '%/" + $('#<%=hdnLocationItemGroupID.ClientID %>').val() + "/%')";
                }

                if (orderID != '') {
                    if (GCPurchasingType == Constant.PurchasingType.RUTIN) {
                        filterExpression += " AND ItemID NOT IN (SELECT ItemID FROM PurchaseOrderDt WHERE PurchaseOrderID = " + orderID + " AND IsDeleted = 0)";
                    }
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
                            filterExpression += " AND GCItemType IN ('X001^002','X001^003','X001^008','X001^009')";
                        }
                    } else {
                        filterExpression += " AND GCItemType IN ('X001^002','X001^003','X001^008','X001^009')";
                    }
                }

                return filterExpression;
            }

            $('#lblItem.lblLink').live('click', function () {
                openSearchDialog('vitemmasterproduct', getItemFilterExpression(), function (value) {
                    $('#<%=txtItemCode.ClientID %>').val(value);
                    onTxtItemCodeChanged(value);
                });
            });

            $('#<%=txtItemCode.ClientID %>').die('change');
            $('#<%=txtItemCode.ClientID %>').live('change', function () {
                onTxtItemCodeChanged($(this).val());
            });

            function onTxtItemCodeChanged(value) {
                var PRInfo = $('#<%=hdnPurchaseRequestNoInfo.ClientID %>').val();
                if (PRInfo == null || PRInfo == "") {
                    var filterExpression = getItemFilterExpression() + " AND ItemCode = '" + value + "'";
                    Methods.getObject('GetvItemMasterProductList', filterExpression, function (result) {
                        if (result != null) {
                            $('#<%=hdnItemID.ClientID %>').val(result.ItemID);
                            $('#<%=txtItemName.ClientID %>').val(result.ItemName1);

                            var hdnIM0131 = $('#<%=hdnIM0131.ClientID %>').val();
                            var orderDate = $('#<%:txtItemOrderDate.ClientID %>').val();
                            var orderDateInDatePicker = Methods.getDatePickerDate(orderDate);
                            var orderDateFormatString = Methods.dateToString(orderDateInDatePicker);
                            if (hdnIM0131 == "0") {
                                Methods.getItemMasterPurchase(result.ItemID, $('#<%=hdnSupplierID.ClientID %>').val(), function (result2) {
                                    if (result2 != null) {
                                        $('#<%=hdnItemGroupID.ClientID %>').val(result2.ItemGroupID);
                                        $('#<%=txtItemGroupCode.ClientID %>').val(result2.ItemGroupCode);
                                        $('#<%=txtItemGroupName.ClientID %>').val(result2.ItemGroupName1);
                                        $('#<%=txtSupplierItemCode.ClientID %>').val(result2.SupplierItemCode);
                                        $('#<%=txtSupplierItemName.ClientID %>').val(result2.SupplierItemName);
                                        $('#<%=hdnUnitPrice.ClientID %>').val(result2.Price);
                                        $('#<%=hdnPurchaseUnitPrice.ClientID %>').val(result2.UnitPrice);
                                        cboItemUnit.SetValue(result2.PurchaseUnit);
                                        $('#<%=hdnGCBaseUnit.ClientID %>').val(result2.ItemUnit);
                                        $('#<%=hdnGCItemUnit.ClientID %>').val(result2.PurchaseUnit);
                                        $('#<%=txtDiscount.ClientID %>').val(result2.Discount).trigger('changeValue');
                                        $('#<%=txtDiscount2.ClientID %>').val(result2.Discount2).trigger('changeValue');
                                    }
                                    else {
                                        $('#<%=txtSupplierItemCode.ClientID %>').val('');
                                        $('#<%=txtSupplierItemName.ClientID %>').val('');
                                        $('#<%=hdnUnitPrice.ClientID %>').val('0');
                                        $('#<%=hdnPurchaseUnitPrice.ClientID %>').val('0');
                                        cboItemUnit.SetValue(result.GCItemUnit);
                                        $('#<%=hdnGCBaseUnit.ClientID %>').val(result.GCItemUnit);
                                        $('#<%=hdnGCItemUnit.ClientID %>').val(result.GCItemUnit);
                                        $('#<%=txtDiscount.ClientID %>').val("0").trigger('changeValue');
                                        $('#<%=txtDiscount2.ClientID %>').val("0").trigger('changeValue');
                                    }
                                });
                            }
                            else {
                                Methods.getItemMasterPurchaseWithDate(result.ItemID, $('#<%=hdnSupplierID.ClientID %>').val(), orderDateFormatString, function (result2) {
                                    if (result2 != null) {
                                        $('#<%=hdnItemGroupID.ClientID %>').val(result2.ItemGroupID);
                                        $('#<%=txtItemGroupCode.ClientID %>').val(result2.ItemGroupCode);
                                        $('#<%=txtItemGroupName.ClientID %>').val(result2.ItemGroupName1);
                                        $('#<%=txtSupplierItemCode.ClientID %>').val(result2.SupplierItemCode);
                                        $('#<%=txtSupplierItemName.ClientID %>').val(result2.SupplierItemName);
                                        $('#<%=hdnUnitPrice.ClientID %>').val(result2.Price);
                                        $('#<%=hdnPurchaseUnitPrice.ClientID %>').val(result2.UnitPrice);
                                        cboItemUnit.SetValue(result2.PurchaseUnit);
                                        $('#<%=hdnGCBaseUnit.ClientID %>').val(result2.ItemUnit);
                                        $('#<%=hdnGCItemUnit.ClientID %>').val(result2.PurchaseUnit);
                                        $('#<%=txtDiscount.ClientID %>').val(result2.Discount).trigger('changeValue');
                                        $('#<%=txtDiscount2.ClientID %>').val(result2.Discount2).trigger('changeValue');
                                    }
                                    else {
                                        $('#<%=txtSupplierItemCode.ClientID %>').val('');
                                        $('#<%=txtSupplierItemName.ClientID %>').val('');
                                        $('#<%=hdnUnitPrice.ClientID %>').val('0');
                                        $('#<%=hdnPurchaseUnitPrice.ClientID %>').val('0');
                                        cboItemUnit.SetValue('');
                                        $('#<%=hdnGCBaseUnit.ClientID %>').val('');
                                        $('#<%=hdnGCItemUnit.ClientID %>').val('');
                                        $('#<%=txtDiscount.ClientID %>').val("0").trigger('changeValue');
                                        $('#<%=txtDiscount2.ClientID %>').val("0").trigger('changeValue');
                                    }
                                });
                            }

                            Methods.getItemQtyOnOrder(result.ItemID, $('#<%=hdnLocationID.ClientID %>').val(), 3, function (result3) {
                                if (result3 != null)
                                    $('#<%=txtQtyOnProcess.ClientID %>').val(result3.QtyOnOrder.toFixed(2) + " " + result.ItemUnit);
                                else
                                    $('#<%=txtQtyOnProcess.ClientID %>').val("0 " + result.ItemUnit);
                                GetItemQtyFromLocation();
                            });

                            cboItemUnit.PerformCallback('addItem');
                        }
                        else {
                            $('#<%=chkIsBonus.ClientID %>').prop('checked', false);
                            $('#<%=txtQuantity.ClientID %>').val('1');
                            $('#<%=hdnEntryID.ClientID %>').val('');
                            $('#<%=hdnPurchaseRequestNoInfo.ClientID %>').val('');
                            $('#<%=hdnItemID.ClientID %>').val('');
                            $('#<%=hdnGCItemUnit.ClientID %>').val('');
                            $('#<%=hdnGCBaseUnit.ClientID %>').val('');
                            $('#<%=txtItemGroupCode.ClientID %>').val('');
                            $('#<%=txtItemGroupName.ClientID %>').val('');
                            $('#<%=txtItemCode.ClientID %>').val('');
                            $('#<%=txtItemName.ClientID %>').val('');
                            $('#<%=hdnUnitPrice.ClientID %>').val('0');
                            $('#<%=hdnPurchaseUnitPrice.ClientID %>').val('0');

                            $('#<%=txtQtyOnProcess.ClientID %>').val('');
                            $('#<%=hdnDiscountAmount1.ClientID %>').val('0');
                            $('#<%=hdnDiscountAmount2.ClientID %>').val('0');
                            $('#<%=txtDiscount.ClientID %>').val('0');
                            $('#<%=txtDiscount2.ClientID %>').val('0');
                            $('#<%=txtDiscountAmount.ClientID %>').val('0').trigger('changeValue');
                            $('#<%=txtDiscountAmount2.ClientID %>').val('0').trigger('changeValue');

                            $('#<%=chkIsDiscountInPercentage1.ClientID %>').prop('checked', true);
                            $('#<%=chkIsDiscountInPercentage2.ClientID %>').prop('checked', true);

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

                            $('#<%=txtPrice.ClientID %>').val('');
                            $('#<%=txtBaseUnit.ClientID %>').val('');
                            $('#<%=txtNotesDt.ClientID %>').val('');
                            $('#<%=txtSupplierItemCode.ClientID %>').val('');
                            $('#<%=txtSupplierItemName.ClientID %>').val('');
                            $('#<%=txtSubTotalPrice.ClientID %>').val('').trigger('changeValue');
                            $('#<%=txtSupplierCode.ClientID %>').attr('readonly', 'readonly');
                            $('#<%=lblProductLine.ClientID %>').attr('class', 'lblDisabled');
                            $('#<%=txtProductLineCode.ClientID %>').attr('readonly', 'readonly');
                            $('#<%=lblLocation.ClientID %>').attr('class', 'lblDisabled');
                            $('#<%=txtLocationCode.ClientID %>').attr('readonly', 'readonly');
                            $('#<%=txtStockLocation.ClientID %>').val('');
                            lastTransactionAmount = $('#<%=txtTotalOrder.ClientID %>').attr('hiddenVal');
                            editedLineAmount = 0;
                            cboItemUnit.SetValue('');
                            $('#<%=txtConversion.ClientID %>').val('');
                            $('#<%=hdnTempFactor.ClientID %>').val('0');
                            $('#<%=hdnConversionFactor.ClientID %>').val('1');
                        }
                    });
                } else {
                    var messageInfo = "Dilarang mengubah item <b>" + $('#<%=txtItemName.ClientID %>').val() + "</b> karena detail pemesanan ini berasal dari permintaan nomor <b>" + PRInfo + "</b>.";
                    displayMessageBox('Information', messageInfo);
                }
            }
            //#endregion

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
                var discPct = $('#<%=txtDiscount.ClientID %>').val();
                if (discPct > 100) {
                    ShowSnackbarError("Harap isikan nilai persentase Diskon_1 dari angka 0 s/d angka 100.");
                    $('#<%=txtDiscount.ClientID %>').val("0");
                    $('#<%=hdnDiscountAmount1.ClientID %>').val("0");
                    $('#<%=txtDiscountAmount.ClientID %>').val(0).trigger('changeValue');
                } else {
                    if ($(this).val() == '' || parseFloat($(this).val()) > 100 || parseFloat($(this).val()) < 0) {
                        $(this).val('0');
                    }

                    var price = parseFloat($('#<%=txtPrice.ClientID %>').val().replace('.00', '').split(',').join(''));
                    var qty = parseFloat($('#<%=txtQuantity.ClientID %>').val().replace('.00', '').split(',').join(''));
                    var subTotal = parseFloat(price * qty);
                    var discountAmount = ((parseFloat($(this).val()) * subTotal) / 100);
                    $('#<%=hdnDiscountAmount1.ClientID %>').val(discountAmount);
                    $('#<%=txtDiscountAmount.ClientID %>').val(discountAmount).trigger('changeValue');
                }
                $('#<%=txtDiscount2.ClientID %>').trigger('change');
            });

            $('#<%=txtDiscountAmount.ClientID %>').die('change');
            $('#<%=txtDiscountAmount.ClientID %>').live('change', function () {
                var price = parseFloat($('#<%=txtPrice.ClientID %>').val().replace('.00', '').split(',').join(''));
                var qty = parseFloat($('#<%=txtQuantity.ClientID %>').val().replace('.00', '').split(',').join(''));
                var subTotal = parseFloat(price * qty);

                if ($(this).val() == '' || parseFloat($(this).val()) > subTotal || parseFloat($(this).val()) < 0) {
                    $(this).val('0');
                    $('#<%=hdnDiscountAmount2.ClientID %>').val(0);
                    $('#<%=txtDiscountAmount2.ClientID %>').val(0).trigger('changeValue');
                }

                var discountAmount = parseFloat($(this).val().replace('.00', '').split(',').join(''));
                var discountPercent = parseFloat(discountAmount / subTotal * 100).toFixed(2);
                $('#<%=hdnDiscountAmount1.ClientID %>').val(discountAmount);
                $('#<%=txtDiscount.ClientID %>').val(discountPercent).trigger('changeValue');

                $('#<%=txtDiscount2.ClientID %>').trigger('change');
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
                var discPct = $('#<%=txtDiscount2.ClientID %>').val();
                if (discPct > 100) {
                    ShowSnackbarError("Harap isikan nilai persentase Diskon_2 dari angka 0 s/d angka 100.");
                    $('#<%=txtDiscount2.ClientID %>').val("0");
                    $('#<%=hdnDiscountAmount2.ClientID %>').val("0");
                    $('#<%=txtDiscountAmount2.ClientID %>').val(0).trigger('changeValue');
                } else {
                    if ($(this).val() == '' || parseFloat($(this).val()) > 100 || parseFloat($(this).val()) < 0) {
                        $(this).val('0');
                    }

                    if ($('#<%:chkIsDiscountInPercentage1.ClientID %>').is(':checked')) {
                        if ($('#<%=txtDiscount.ClientID %>').val() == 0) {
                            $(this).val('0');
                        }
                    }
                    else {
                        if ($('#<%=txtDiscountAmount.ClientID %>').val() == 0) {
                            $(this).val('0');
                        }
                    }

                    var price = parseFloat($('#<%=txtPrice.ClientID %>').val().replace('.00', '').split(',').join(''));
                    var qty = parseFloat($('#<%=txtQuantity.ClientID %>').val().replace('.00', '').split(',').join(''));
                    var subTotal = parseFloat(price * qty);

                    if ($('#<%:chkIsDiscountInPercentage1.ClientID %>').is(':checked')) {
                        subTotal = subTotal - parseFloat($('#<%=txtDiscountAmount.ClientID %>').attr('hiddenVal'));
                    } else {
                        subTotal = subTotal - parseFloat($('#<%=txtDiscountAmount.ClientID %>').val().replace('.00', '').split(',').join(''));
                    }
                    var discountAmount2 = (parseFloat($(this).val()) * subTotal / 100);

                    $('#<%=hdnDiscountAmount2.ClientID %>').val(discountAmount2);
                    $('#<%=txtDiscountAmount2.ClientID %>').val(discountAmount2).trigger('changeValue');

                }
                calculateSubTotal();
            });

            $('#<%=txtDiscountAmount2.ClientID %>').die('change');
            $('#<%=txtDiscountAmount2.ClientID %>').live('change', function () {
                var price = parseFloat($('#<%=txtPrice.ClientID %>').val().replace('.00', '').split(',').join(''));
                var qty = parseFloat($('#<%=txtQuantity.ClientID %>').val().replace('.00', '').split(',').join(''));
                var subTotal = parseFloat(price * qty);
                subTotal = subTotal - parseFloat($('#<%=txtDiscountAmount.ClientID %>').attr('hiddenVal'));

                if ($(this).val() == '' || parseFloat($(this).val()) > subTotal || parseFloat($(this).val()) < 0) {
                    $(this).val('0');
                }

                if ($('#<%:chkIsDiscountInPercentage1.ClientID %>').is(':checked')) {
                    if ($('#<%=txtDiscount.ClientID %>').val() == 0) {
                        $(this).val('0');
                    }
                }
                else {
                    if ($('#<%=txtDiscountAmount.ClientID %>').val() == 0) {
                        $(this).val('0');
                    }
                }

                if ($('#<%:chkIsDiscountInPercentage2.ClientID %>').is(':checked')) {
                } else {
                    var price = parseFloat($('#<%=txtPrice.ClientID %>').val().replace('.00', '').split(',').join(''));
                    var qty = parseFloat($('#<%=txtQuantity.ClientID %>').val().replace('.00', '').split(',').join(''));
                    var discountAmount1 = parseFloat($('#<%=txtDiscountAmount.ClientID %>').val().replace('.00', '').split(',').join(''));
                    var subTotal = parseFloat((price * qty) - discountAmount1);
                    var discountAmount2 = parseFloat($(this).val().replace('.00', '').split(',').join(''));
                    var discountPercent2 = parseFloat(discountAmount2 / subTotal * 100).toFixed(2);

                    $('#<%=hdnDiscountAmount2.ClientID %>').val(discountAmount2);
                    $('#<%=txtDiscount2.ClientID %>').val(discountPercent2).trigger('changeValue');
                }
                calculateSubTotal();
            });
            //#endregion

            $('#btnCancel').die('click');
            $('#btnCancel').live('click', function () {
                $('#containerEntry').hide();
            });

            $('#btnSave').die('click');
            $('#btnSave').live('click', function (evt) {
                if (IsValid(evt, 'fsTrxPopup', 'mpTrxPopup'))
                    cbpProcess.PerformCallback('save');
            });

            $('#<%=txtQuantity.ClientID %>').die('change');
            $('#<%=txtQuantity.ClientID %>').live('change', function () {
                if ($(this).val() == '') {
                    $(this).val('1');
                }

                $('#<%=txtDiscount.ClientID %>').trigger('change');

                calculateSubTotal();
            });

            $('#<%=txtPrice.ClientID %>').die('change');
            $('#<%=txtPrice.ClientID %>').live('change', function () {
                $(this).blur();

                $('#<%=txtDiscount.ClientID %>').trigger('change');

                calculateSubTotal();
            });

            $('#<%=chkPPN.ClientID %>').die('change');
            $('#<%=chkPPN.ClientID %>').live('change', function () {
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
                var ppnPct = $('#<%=txtVATPercentageDefault.ClientID %>').val();
                if (ppnPct > 100) {
                    ShowSnackbarError("Harap isikan nilai persentase PPN dari angka 0 s/d angka 100.");
                    $('#<%=txtVATPercentageDefault.ClientID %>').val("0");
                    $('#<%=txtVATPercentageDefault.ClientID %>').trigger('changeValue');
                    $('#<%=txtPPN.ClientID %>').val("0");
                    $('#<%=txtPPN.ClientID %>').trigger('changeValue');
                }
                $('#<%:hdnVATPercentage.ClientID %>').val($('#<%=txtVATPercentageDefault.ClientID %>').val());
                calculateTotal();
            });

            $('#<%=txtDP.ClientID %>').die('change');
            $('#<%=txtDP.ClientID %>').live('change', function () {
                $(this).trigger('changeValue');
                calculateTotal();
            });

            $('#<%=txtCharges.ClientID %>').die('change');
            $('#<%=txtCharges.ClientID %>').live('change', function () {
                $(this).trigger('changeValue');
                calculateTotal();
            });

            $('#<%=chkIsFinalDiscountInPercentage.ClientID %>').die('change');
            $('#<%:chkIsFinalDiscountInPercentage.ClientID %>').live('change', function () {
                if ($(this).is(':checked')) {
                    if ($('#<%=hdnGCTransactionStatus.ClientID %>').val() == "X121^001") {
                        $('#<%=txtFinalDiscountInPercentage.ClientID %>').change();
                        $('#<%=txtFinalDiscountInPercentage.ClientID%>').removeAttr('readonly');
                        $('#<%=txtFinalDiscount.ClientID%>').attr('readonly', 'readonly');
                    }
                } else {
                    if ($('#<%=hdnGCTransactionStatus.ClientID %>').val() == "X121^001") {
                        $('#<%=txtFinalDiscount.ClientID %>').change();
                        $('#<%=txtFinalDiscountInPercentage.ClientID%>').attr('readonly', 'readonly');
                        $('#<%=txtFinalDiscount.ClientID%>').removeAttr('readonly');
                    }
                }
            });

            $('#<%=txtFinalDiscountInPercentage.ClientID %>').die('change');
            $('#<%=txtFinalDiscountInPercentage.ClientID %>').live('change', function () {
                var finalDiscPct = $('#<%=txtFinalDiscountInPercentage.ClientID %>').val();
                if (finalDiscPct > 100) {
                    ShowSnackbarError("Harap isikan nilai persentase Diskon Final dari angka 0 s/d angka 100.");
                    $('#<%=txtFinalDiscountInPercentage.ClientID %>').val("0");
                    $(this).trigger('changeValue');
                    calculateFinalDiscount("fromPctg");
                    calculateTotal();
                } else {
                    if ($('#<%=chkIsFinalDiscountInPercentage.ClientID %>').is(':checked')) {
                        $(this).trigger('changeValue');
                        calculateFinalDiscount("fromPctg");
                        calculateTotal();
                    } else {
                        $(this).trigger('changeValue');
                        calculateFinalDiscount("fromTxt");
                        calculateTotal();
                    }
                }
            });

            $('#<%=txtFinalDiscount.ClientID %>').die('change');
            $('#<%=txtFinalDiscount.ClientID %>').live('change', function () {
                if ($('#<%=chkIsFinalDiscountInPercentage.ClientID %>').is(':checked')) {
                    $(this).trigger('changeValue');
                    calculateFinalDiscount("fromPctg");
                    calculateTotal();
                } else {
                    $(this).trigger('changeValue');
                    calculateFinalDiscount("fromTxt");
                    calculateTotal();
                }
            });

            function calculateFinalDiscount(kode) {
                var transactionAmount = parseFloat($('#<%=txtTotalOrder.ClientID %>').attr('hiddenVal'));

                if (kode == "fromPctg") {
                    var finalDiscount = parseFloat($('#<%=txtFinalDiscountInPercentage.ClientID %>').attr('hiddenVal'));
                    var totaltrans = transactionAmount * (finalDiscount / 100);
                    $('#<%=txtFinalDiscount.ClientID %>').val(totaltrans).trigger('changeValue');
                    $('#<%=txtFinalDiscountInPercentage.ClientID %>').val(finalDiscount).trigger('changeValue');

                    var pctg2 = parseFloat($('#<%=txtFinalDiscountInPercentage.ClientID %>').attr('hiddenVal'));
                    var totaltrans1 = transactionAmount * (pctg2 / 100);

                    $('#<%=txtFinalDiscount.ClientID %>').val(totaltrans1).trigger('changeValue');
                    $('#<%=txtFinalDiscountInPercentage.ClientID %>').val(pctg2).trigger('changeValue');
                } else if (kode == "fromTxt") {
                    var discount = parseFloat($('#<%=txtFinalDiscount.ClientID %>').attr('hiddenVal'));
                    var pctg2 = discount / (transactionAmount / 100);

                    $('#<%=txtFinalDiscount.ClientID %>').val(discount).trigger('changeValue');
                    $('#<%=txtFinalDiscountInPercentage.ClientID %>').val(pctg2).trigger('changeValue');
                }
                var pctg2 = parseFloat($('#<%=txtFinalDiscount.ClientID %>').attr('hiddenVal'));
                calculateTotal()
            }

            $('.txtCurrency').each(function () {
                $(this).trigger('changeValue');
            });

            var pageCount = parseInt($('#<%=hdnPageCount.ClientID %>').val());
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });

            $('#<%=txtFinalDiscountInPercentage.ClientID %>').change();
        }

        function GetItemQtyFromLocation(param) {
            if (param != "edit") {
                var filterExpression = "LocationID = " + $('#<%=hdnLocationID.ClientID %>').val() + " AND ItemID = " + $('#<%=hdnItemID.ClientID %>').val() + " AND IsDeleted = 0";
                Methods.getObject('GetvItemBalanceList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=txtStockLocation.ClientID %>').val(result.QuantityEND + ' ' + result.ItemUnit);
                        $('#<%=hdnQtyEndLocation.ClientID %>').val(result.QuantityEND);
                        $('#<%=hdnGCItemUnitQtyEndLocation.ClientID %>').val(result.GCItemUnit);
                    }
                    else {
                        $('#<%=txtStockLocation.ClientID %>').val('');
                        $('#<%=hdnQtyEndLocation.ClientID %>').val('0');
                        $('#<%=hdnGCItemUnitQtyEndLocation.ClientID %>').val('');
                    }
                });
            }
        }

        //#region PPH
        function oncboPPHOptionsValueChanged(evt) {
            if ($('#<%=chkIsPPHInPercentage.ClientID %>').is(':checked')) {
                $(this).trigger('changeValue');
                calculatePPH("fromPctg");
                calculateTotal();
            } else {
                $(this).trigger('changeValue');
                calculatePPH("fromTxt");
                calculateTotal();
            }
        }

        $('#<%:chkIsPPHInPercentage.ClientID %>').live('change', function () {
            if ($(this).is(':checked')) {
                if ($('#<%=hdnGCTransactionStatus.ClientID %>').val() == "X121^001") {
                    $('#<%=txtPPHPercentage.ClientID %>').change();
                    $('#<%=txtPPHPercentage.ClientID%>').removeAttr('readonly');
                    $('#<%=txtPPHAmount.ClientID%>').attr('readonly', 'readonly');
                }
            } else {
                if ($('#<%=hdnGCTransactionStatus.ClientID %>').val() == "X121^001") {
                    $('#<%=txtPPHAmount.ClientID %>').change();
                    $('#<%=txtPPHPercentage.ClientID%>').attr('readonly', 'readonly');
                    $('#<%=txtPPHAmount.ClientID%>').removeAttr('readonly');
                }
            }
        });

        $('#<%=txtPPHPercentage.ClientID %>').live('change', function () {
            var pphPct = $('#<%=txtPPHPercentage.ClientID %>').val();
            if (pphPct > 100) {
                ShowSnackbarError("Harap isikan nilai persentase PPH dari angka 0 s/d angka 100.");
                $('#<%=txtPPHPercentage.ClientID %>').val("0");
                $(this).trigger('changeValue');
                calculatePPH("fromPctg");
                calculateTotal();
            } else {
                if ($('#<%=chkIsPPHInPercentage.ClientID %>').is(':checked')) {
                    $(this).trigger('changeValue');
                    calculatePPH("fromPctg");
                    calculateTotal();
                } else {
                    $(this).trigger('changeValue');
                    calculatePPH("fromTxt");
                    calculateTotal();
                }
            }
        });

        $('#<%=txtPPHAmount.ClientID %>').live('change', function () {
            if ($('#<%=chkIsPPHInPercentage.ClientID %>').is(':checked')) {
                $(this).trigger('changeValue');
                calculatePPH("fromPctg");
                calculateTotal();
            } else {
                $(this).trigger('changeValue');
                calculatePPH("fromTxt");
                calculateTotal();
            }
        });

        function calculatePPH(kode) {
            var transactionAmount = parseFloat($('#<%=txtTotalOrder.ClientID %>').val());
            if (kode == "fromPctg") {
                var pctg1 = parseFloat($('#<%=txtPPHPercentage.ClientID %>').attr('hiddenVal'));
                var totalPPH1 = parseFloat((transactionAmount * (pctg1 / 100)).toFixed(2));
                if (cboPPHOptions.GetText() == 'Minus') {
                    totalPPH1 *= -1;
                    if (pctg < 0) {
                        pctg *= -1;
                    }
                }
                $('#<%=txtPPHAmount.ClientID %>').val(totalPPH1).trigger('changeValue');
            } else if (kode == "fromTxt") {
                var pph = parseFloat($('#<%=txtPPHAmount.ClientID %>').attr('hiddenVal'));
                var pctg = parseFloat((pph / (transactionAmount / 100)).toFixed(2));

                if (cboPPHOptions.GetText() == 'Minus') {
                    pph *= -1;
                    if (pctg < 0) {
                        pctg *= -1;
                    }
                }

                $('#<%=txtPPHAmount.ClientID %>').val(pph).trigger('changeValue');
                $('#<%=txtPPHPercentage.ClientID %>').val(pctg).trigger('changeValue');
            }
            var PPH = parseFloat($('#<%=txtPPHAmount.ClientID %>').attr('hiddenVal'));
        }

        //#endregion

        //#region AddData
        $('#lblAddData').live('click', function (evt) {
            var checkPOType = cboPurchaseOrderType.GetValue();
            if (checkPOType != null) {
                if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                    $('#<%=hdnIsEdit.ClientID %>').val('0');
                    $('#<%=chkIsBonus.ClientID %>').prop('checked', false);
                    $('#<%=txtQuantity.ClientID %>').val('1');
                    $('#<%=hdnEntryID.ClientID %>').val('');
                    $('#<%=hdnPurchaseRequestNoInfo.ClientID %>').val('');
                    $('#<%=hdnItemID.ClientID %>').val('');
                    $('#<%=hdnGCItemUnit.ClientID %>').val('');
                    $('#<%=hdnGCBaseUnit.ClientID %>').val('');
                    $('#<%=txtItemGroupCode.ClientID %>').val('');
                    $('#<%=txtItemGroupName.ClientID %>').val('');
                    $('#<%=txtItemCode.ClientID %>').val('');
                    $('#<%=txtItemName.ClientID %>').val('');
                    $('#<%=hdnUnitPrice.ClientID %>').val('0');
                    $('#<%=hdnPurchaseUnitPrice.ClientID %>').val('0');

                    $('#<%=txtQtyOnProcess.ClientID %>').val('');
                    $('#<%=hdnDiscountAmount1.ClientID %>').val('0');
                    $('#<%=hdnDiscountAmount2.ClientID %>').val('0');
                    $('#<%=txtDiscount.ClientID %>').val('0');
                    $('#<%=txtDiscount2.ClientID %>').val('0');
                    $('#<%=txtDiscountAmount.ClientID %>').val('0').trigger('changeValue');
                    $('#<%=txtDiscountAmount2.ClientID %>').val('0').trigger('changeValue');

                    $('#<%=chkIsDiscountInPercentage1.ClientID %>').prop('checked', true);
                    $('#<%=chkIsDiscountInPercentage2.ClientID %>').prop('checked', true);

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

                    $('#<%=txtPrice.ClientID %>').val('');
                    $('#<%=txtBaseUnit.ClientID %>').val('');
                    $('#<%=txtNotesDt.ClientID %>').val('');
                    $('#<%=txtSupplierItemCode.ClientID %>').val('');
                    $('#<%=txtSupplierItemName.ClientID %>').val('');
                    $('#<%=txtSubTotalPrice.ClientID %>').val('').trigger('changeValue');
                    $('#<%=txtSupplierCode.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=lblProductLine.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%=txtProductLineCode.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=lblLocation.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%=txtLocationCode.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtStockLocation.ClientID %>').val('');
                    lastTransactionAmount = $('#<%=txtTotalOrder.ClientID %>').attr('hiddenVal');
                    editedLineAmount = 0;
                    cboItemUnit.SetValue('');
                    $('#<%=txtConversion.ClientID %>').val('');
                    $('#<%=hdnTempFactor.ClientID %>').val('0');
                    $('#<%=hdnConversionFactor.ClientID %>').val('1');
                    $('#containerEntry').show();
                }
            } else {
                displayErrorMessageBox('Silahkan Coba Lagi', "Silahkan pilih Jenis Pemesanan terlebih dahulu.");
            }
        });
        //#endregion

        //#region AddData Salin PR
        $('#lblSalinPR').live('click', function (evt) {
            var checkPOType = cboPurchaseOrderType.GetValue();
            if (checkPOType != null) {
                if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                    var url = ResolveUrl('~/Program/Procurement/PurchaseOrder/CopyPurchaseRequestItemCtl.ascx');
                    var purchaseOrderID = $('#<%=hdnOrderID.ClientID %>').val();
                    var supplierID = $('#<%=hdnSupplierID.ClientID %>').val();
                    var locationID = $('#<%=hdnLocationID.ClientID %>').val();
                    var productLineID = $('#<%=hdnProductLineID.ClientID %>').val();
                    var hdnIM0131 = $('#<%=hdnIM0131.ClientID %>').val();
                    var orderDate = $('#<%:txtItemOrderDate.ClientID %>').val();
                    var orderDateInDatePicker = Methods.getDatePickerDate(orderDate);
                    var orderDateFormatString = Methods.dateToString(orderDateInDatePicker);

                    var param = purchaseOrderID + '|' + supplierID + '|' + locationID + '|' + productLineID + '|' + checkPOType + '|' + hdnIM0131 + '|' + orderDateFormatString;
                    openUserControlPopup(url, param, 'Salin Permintaan Pembelian', 1000, 600);
                }
            }
            else {
                displayErrorMessageBox('Silahkan Coba Lagi', "Silahkan pilih Jenis Pemesanan terlebih dahulu.");
            }
        });
        //#endregion

        //#region edit and delete
        $('.imgDelete.imgLink').die('click');
        $('.imgDelete.imgLink').live('click', function () {
            $row = $(this).closest('tr').parent().closest('tr');
            showToastConfirmation('Are You Sure Want To Delete?', function (result) {
                if (result) {
                    var entity = rowToObject($row);
                    $('#<%=hdnEntryID.ClientID %>').val(entity.ID);
                    cbpProcess.PerformCallback('delete');
                }
            });
        });

        $('.imgEdit.imgLink').die('click');
        $('.imgEdit.imgLink').live('click', function () {
            $row = $(this).closest('tr').parent().closest('tr');
            var entity = rowToObject($row);
            $('#<%=hdnIsEdit.ClientID %>').val('1');

            $('#<%=chkIsBonus.ClientID %>').prop('checked', (entity.IsBonusItem == 'True'));

            $('#<%=hdnEntryID.ClientID %>').val(entity.ID);

            $('#<%=hdnPurchaseRequestNoInfo.ClientID %>').val(entity.PurchaseRequestNoInfo);

            $('#<%=hdnGCBaseUnit.ClientID %>').val(entity.GCBaseUnit);
            $('#<%=hdnGCItemUnit.ClientID %>').val(entity.GCPurchaseUnit);
            $('#<%=txtSupplierItemCode.ClientID %>').val(entity.SupplierItemCode);
            $('#<%=txtSupplierItemName.ClientID %>').val(entity.SupplierItemName);
            $('#<%=hdnUnitPrice.ClientID %>').val(parseFloat(entity.UnitPrice) / parseFloat(entity.ConversionFactor));
            $('#<%=hdnPurchaseUnitPrice.ClientID %>').val(parseFloat(entity.UnitPrice));

            $('#<%=hdnItemID.ClientID %>').val(entity.ItemID);
            $('#<%=txtItemCode.ClientID %>').val(entity.ItemCode);
            $('#<%=txtItemName.ClientID %>').val(entity.ItemName1);
            $('#<%=hdnItemGroupID.ClientID %>').val(entity.ItemGroupID);
            $('#<%=txtItemGroupCode.ClientID %>').val(entity.ItemGroupCode);
            $('#<%=txtItemGroupName.ClientID %>').val(entity.ItemGroupName1);
            $('#<%=txtQuantity.ClientID %>').val(entity.Quantity);
            var endLocation = entity.QtyENDLocation + ' ' + entity.ItemUnitQtyEndLocation;
            $('#<%=txtStockLocation.ClientID %>').val(endLocation);
            $('#<%=hdnTempFactor.ClientID %>').val(entity.ConversionFactor);
            Methods.getItemQtyOnOrder(entity.ItemID, $('#<%=hdnLocationID.ClientID %>').val(), 3, function (result3) {
                if (result3 != null) {
                    if (parseFloat(result3.QtyOnOrder.toFixed(2)) != 0) {
                        $('#<%=txtQtyOnProcess.ClientID %>').val((parseFloat(result3.QtyOnOrder.toFixed(2)) - parseFloat(entity.CustomTotal)) + " " + entity.BaseUnit);
                    } else {
                        $('#<%=txtQtyOnProcess.ClientID %>').val((parseFloat(entity.CustomTotal)) + " " + entity.BaseUnit);
                    }
                }
                else {
                    $('#<%=txtQtyOnProcess.ClientID %>').val("0 " + entity.BaseUnit);
                }
                GetItemQtyFromLocation("edit");
            });
            lastTransactionAmount = $('#<%=txtTotalOrder.ClientID %>').attr('hiddenVal');
            $('#<%=txtNotesDt.ClientID %>').val(entity.Remarks);
            editedLineAmount = entity.CustomSubTotal;

            cboItemUnit.PerformCallback("edit");

            $('#<%=txtPrice.ClientID %>').val(entity.UnitPrice).trigger('changeValue');
            $('#<%=hdnPurchaseUnitPrice.ClientID %>').val(entity.UnitPrice);

            $('#<%=txtDiscount.ClientID %>').val(entity.DiscountPercentage1).trigger('changeValue');
            $('#<%=hdnDiscountAmount1.ClientID %>').val(entity.DiscountAmount1);
            $('#<%=txtDiscountAmount.ClientID %>').val(entity.DiscountAmount1).trigger('changeValue');

            $('#<%=txtDiscount2.ClientID %>').val(entity.DiscountPercentage2).trigger('changeValue');
            $('#<%=hdnDiscountAmount2.ClientID %>').val(entity.DiscountAmount2);
            $('#<%=txtDiscountAmount2.ClientID %>').val(entity.DiscountAmount2).trigger('changeValue');

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
            else {
                $('#<%=txtPPN.ClientID %>').val('0').trigger('changeValue');
            }

            if ($('#<%=chkIsPPHInPercentage.ClientID %>').is(':checked')) {
                var pctg1 = parseFloat($('#<%=txtPPHPercentage.ClientID %>').attr('hiddenVal'));
                var totalPPH1 = totalKotor * (pctg1 / 100);
                if (cboPPHOptions.GetText() == 'Minus') {
                    totalPPH1 *= -1;
                    if (pctg < 0) {
                        pctg *= -1;
                    }
                }
                $('#<%=txtPPHAmount.ClientID %>').val(totalPPH1).trigger('changeValue');
            } else {
                var pph = parseFloat($('#<%=txtPPHAmount.ClientID %>').attr('hiddenVal'));
                var pctg = pph / (totalKotor / 100);

                if (cboPPHOptions.GetText() == 'Minus') {
                    pph *= -1;
                    if (pctg < 0) {
                        pctg *= -1;
                    }
                }
                $('#<%=txtPPHPercentage.ClientID %>').val(pctg).trigger('changeValue');
            }

            var PPN = parseFloat($('#<%=txtPPN.ClientID %>').attr('hiddenVal'));
            var PPH = parseFloat($('#<%=txtPPHAmount.ClientID %>').val().replace('.00', '').split(',').join(''));
            var DP = parseFloat($('#<%=txtDP.ClientID %>').attr('hiddenVal'));
            var Charge = parseFloat($('#<%=txtCharges.ClientID %>').attr('hiddenVal'));

            var totalHarga = totalKotor - (Discount + DP) + PPN + PPH + Charge;
            $('#<%=txtTotalOrderSaldo.ClientID %>').val(totalHarga).trigger('changeValue');
        }

        function calculateSubTotal() {
            var price = parseFloat($('#<%=txtPrice.ClientID %>').val().replace('.00', '').split(',').join(''));
            var qty = parseFloat($('#<%=txtQuantity.ClientID %>').val().replace('.00', '').split(',').join(''));
            var subTotal = parseFloat(price * qty);
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
            cboItemUnit.SetValue($('#<%=hdnGCItemUnit.ClientID %>').val());
            onCboItemUnitChanged(param[0]);
        }

        function onCboItemUnitChanged(paramProcess) {
            var purchaseUnit = cboItemUnit.GetValue();
            var tempPricePerPurchaseUnit = 0;
            var baseValue = $('#<%=hdnGCBaseUnit.ClientID %>').val();
            var toUnitItem = cboItemUnit.GetValue();
            var factor = 1;
            var factorOld = $('#<%=hdnTempFactor.ClientID %>').val();
            var qtyBegin = $('#<%=txtQuantity.ClientID %>').val();
            var qtyEnd = parseFloat("1");

            //change price per unit
            var hdnIM0131 = $('#<%=hdnIM0131.ClientID %>').val();

            var orderDate = $('#<%:txtItemOrderDate.ClientID %>').val();
            var orderDateInDatePicker = Methods.getDatePickerDate(orderDate);
            var orderDateFormatString = Methods.dateToString(orderDateInDatePicker);

            if (hdnIM0131 == "0") {
                Methods.getItemMasterPurchaseList($('#<%=hdnItemID.ClientID %>').val(), $('#<%=hdnSupplierID.ClientID %>').val(), function (result) {
                    if (result.length > 0) {
                        for (i = 0; i < result.length; i++) {
                            if (result[i].ItemUnit == purchaseUnit && result[i].PurchaseUnit == purchaseUnit) {
                                var conversion = "1 " + result[i].ItemUnitText + " = " + 1 + " " + result[i].ItemUnitText;
                                $('#<%=txtConversion.ClientID %>').val(conversion);
                                $('#<%=hdnConversionFactor.ClientID %>').val('1');
                                if (paramProcess != "edit") {
                                    $('#<%=txtPrice.ClientID %>').val(result[i].Price).trigger('changeValue');
                                    $('#<%=hdnPurchaseUnitPrice.ClientID %>').val(result[i].Price);
                                }

                                $('#<%=hdnGCBaseUnit.ClientID %>').val(result[i].ItemUnit);
                                $('#<%=hdnGCItemUnit.ClientID %>').val(result[i].PurchaseUnit);

                                if (paramProcess != "edit") {
                                    $('#<%=chkIsDiscountInPercentage1.ClientID %>').prop('checked', true);
                                    $('#<%=txtDiscount.ClientID %>').val(result[i].Discount);
                                    $('#<%=txtDiscount.ClientID%>').removeAttr('readonly');

                                    var price = parseFloat($('#<%=txtPrice.ClientID %>').val().replace('.00', '').split(',').join(''));
                                    var qty = parseFloat($('#<%=txtQuantity.ClientID %>').val().replace('.00', '').split(',').join(''));
                                    var subTotal = parseFloat(price * qty);
                                    var discountAmount = (parseFloat(result[i].Discount * subTotal) / 100);
                                    $('#<%=hdnDiscountAmount1.ClientID %>').val(discountAmount);
                                    $('#<%=txtDiscountAmount.ClientID %>').val(discountAmount).trigger('changeValue');
                                    $('#<%=txtDiscountAmount.ClientID%>').attr('readonly', 'readonly');

                                    $('#<%=chkIsDiscountInPercentage2.ClientID %>').prop('checked', true);
                                    $('#<%=txtDiscount2.ClientID %>').val(result[i].Discount2);
                                    $('#<%=txtDiscount2.ClientID%>').removeAttr('readonly');

                                    price = parseFloat($('#<%=txtPrice.ClientID %>').val().replace('.00', '').split(',').join('')) - discountAmount;
                                    qty = parseFloat($('#<%=txtQuantity.ClientID %>').val().replace('.00', '').split(',').join(''));
                                    subTotal = parseFloat(price * qty);
                                    var discountAmount2 = ((result[i].Discount2 * subTotal) / 100);
                                    $('#<%=hdnDiscountAmount2.ClientID %>').val(discountAmount2);
                                    $('#<%=txtDiscountAmount2.ClientID %>').val(discountAmount2).trigger('changeValue');
                                    $('#<%=txtDiscountAmount2.ClientID%>').attr('readonly', 'readonly');
                                }
                                break;
                            } else if (result[i].PurchaseUnit == purchaseUnit) {
                                var conversion = "1 " + result[i].PurchaseUnitText + " = " + result[i].ConversionFactor + " " + result[i].ItemUnitText;
                                $('#<%=txtConversion.ClientID %>').val(conversion);
                                $('#<%=hdnConversionFactor.ClientID %>').val(result[i].ConversionFactor);
                                if (paramProcess != "edit") {
                                    $('#<%=txtPrice.ClientID %>').val(result[i].UnitPrice).trigger('changeValue');
                                    $('#<%=hdnPurchaseUnitPrice.ClientID %>').val(result[i].UnitPrice);
                                }
                                $('#<%=hdnGCBaseUnit.ClientID %>').val(result[i].ItemUnit);
                                $('#<%=hdnGCItemUnit.ClientID %>').val(result[i].PurchaseUnit);

                                if (paramProcess != "edit") {
                                    $('#<%=chkIsDiscountInPercentage1.ClientID %>').prop('checked', true);
                                    $('#<%=txtDiscount.ClientID %>').val(result[i].Discount);
                                    $('#<%=txtDiscount.ClientID%>').removeAttr('readonly');

                                    var price = parseFloat($('#<%=txtPrice.ClientID %>').val().replace('.00', '').split(',').join(''));
                                    var qty = parseFloat($('#<%=txtQuantity.ClientID %>').val().replace('.00', '').split(',').join(''));
                                    var subTotal = parseFloat(price * qty);
                                    var discountAmount = (parseFloat(result[i].Discount * subTotal) / 100);
                                    $('#<%=hdnDiscountAmount1.ClientID %>').val(discountAmount);
                                    $('#<%=txtDiscountAmount.ClientID %>').val(discountAmount).trigger('changeValue');
                                    $('#<%=txtDiscountAmount.ClientID%>').attr('readonly', 'readonly');

                                    $('#<%=chkIsDiscountInPercentage2.ClientID %>').prop('checked', true);
                                    $('#<%=txtDiscount2.ClientID %>').val(result[i].Discount2);
                                    $('#<%=txtDiscount2.ClientID%>').removeAttr('readonly');

                                    price = parseFloat($('#<%=txtPrice.ClientID %>').val().replace('.00', '').split(',').join('')) - discountAmount;
                                    qty = parseFloat($('#<%=txtQuantity.ClientID %>').val().replace('.00', '').split(',').join(''));
                                    subTotal = parseFloat(price * qty);
                                    var discountAmount2 = ((result[i].Discount2 * subTotal) / 100);
                                    $('#<%=hdnDiscountAmount2.ClientID %>').val(discountAmount2);
                                    $('#<%=txtDiscountAmount2.ClientID %>').val(discountAmount2).trigger('changeValue');
                                    $('#<%=txtDiscountAmount2.ClientID%>').attr('readonly', 'readonly');
                                }
                                break;
                            }
                            else {
                                //change conversion factor
                                $('#<%=txtBaseUnit.ClientID %>').val("per " + cboItemUnit.GetText());
                                if (baseValue == toUnitItem) {
                                    $('#<%=hdnConversionFactor.ClientID %>').val('1');
                                    var conversion = "1 " + cboItemUnit.GetText() + " = 1 " + cboItemUnit.GetText();
                                    $('#<%=txtConversion.ClientID %>').val(conversion);
                                }
                                else {
                                    var itemID = $('#<%=hdnItemID.ClientID %>').val();
                                    var filterExpression = "ItemID = " + itemID + " AND GCAlternateUnit = '" + toUnitItem + "' AND IsActive = 1 AND IsDeleted = 0";
                                    Methods.getObject('GetvItemAlternateUnitList', filterExpression, function (resultAlt) {
                                        if (resultAlt != null) {
                                            var toConversion = resultAlt.AlternateUnit;
                                            $('#<%=hdnConversionFactor.ClientID %>').val(resultAlt.ConversionFactor);
                                            var conversion = "1 " + toConversion + " = " + resultAlt.ConversionFactor + " " + resultAlt.ItemUnit;
                                            $('#<%=txtConversion.ClientID %>').val(conversion);
                                            factor = resultAlt.ConversionFactor;
                                        }
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
                                            pricePerPurchaseUnit = result[i].Price * convFactor;
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

                                if (paramProcess != "edit") {
                                    $('#<%=txtPrice.ClientID %>').val(pricePerPurchaseUnit).trigger('changeValue');

                                    $('#<%=chkIsDiscountInPercentage1.ClientID %>').prop('checked', true);
                                    $('#<%=txtDiscount.ClientID %>').val(result[i].Discount);
                                    $('#<%=txtDiscount.ClientID%>').removeAttr('readonly');

                                    var price = parseFloat($('#<%=txtPrice.ClientID %>').val().replace('.00', '').split(',').join(''));
                                    var qty = parseFloat($('#<%=txtQuantity.ClientID %>').val().replace('.00', '').split(',').join(''));
                                    var subTotal = parseFloat(price * qty);
                                    var discountAmount = (parseFloat(result[i].Discount * subTotal) / 100);
                                    $('#<%=hdnDiscountAmount1.ClientID %>').val(discountAmount);
                                    $('#<%=txtDiscountAmount.ClientID %>').val(discountAmount).trigger('changeValue');
                                    $('#<%=txtDiscountAmount.ClientID%>').attr('readonly', 'readonly');

                                    $('#<%=chkIsDiscountInPercentage2.ClientID %>').prop('checked', true);
                                    $('#<%=txtDiscount2.ClientID %>').val(result[i].Discount2);
                                    $('#<%=txtDiscount2.ClientID%>').removeAttr('readonly');

                                    price = parseFloat($('#<%=txtPrice.ClientID %>').val().replace('.00', '').split(',').join('')) - discountAmount;
                                    qty = parseFloat($('#<%=txtQuantity.ClientID %>').val().replace('.00', '').split(',').join(''));
                                    subTotal = parseFloat(price * qty);
                                    var discountAmount2 = ((result[i].Discount2 * subTotal) / 100);
                                    $('#<%=hdnDiscountAmount2.ClientID %>').val(discountAmount2);
                                    $('#<%=txtDiscountAmount2.ClientID %>').val(discountAmount2).trigger('changeValue');
                                    $('#<%=txtDiscountAmount2.ClientID%>').attr('readonly', 'readonly');
                                }
                            }
                        }
                    }
                    else {
                        //change conversion factor
                        $('#<%=txtBaseUnit.ClientID %>').val("per " + cboItemUnit.GetText());
                        if (baseValue == toUnitItem) {
                            $('#<%=hdnConversionFactor.ClientID %>').val('1');
                            var conversion = "1 " + cboItemUnit.GetText() + " = 1 " + cboItemUnit.GetText();
                            $('#<%=txtConversion.ClientID %>').val(conversion);
                        }
                        else {
                            var itemID = $('#<%=hdnItemID.ClientID %>').val();

                            var filterExpression = "ItemID = " + itemID + " AND GCAlternateUnit = '" + toUnitItem + "' AND IsActive = 1 AND IsDeleted = 0";
                            Methods.getObject('GetvItemAlternateUnitList', filterExpression, function (resultAlt) {
                                if (resultAlt != null) {
                                    var toConversion = resultAlt.AlternateUnit;
                                    $('#<%=hdnConversionFactor.ClientID %>').val(resultAlt.ConversionFactor);
                                    var conversion = "1 " + toConversion + " = " + resultAlt.ConversionFactor + " " + resultAlt.ItemUnit;
                                    $('#<%=txtConversion.ClientID %>').val(conversion);
                                    factor = resultAlt.ConversionFactor;
                                }
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
            }
            else {
                Methods.getItemMasterPurchaseWithDateList($('#<%=hdnItemID.ClientID %>').val(), $('#<%=hdnSupplierID.ClientID %>').val(), orderDateFormatString, function (result) {
                    if (result.length > 0) {
                        for (i = 0; i < result.length; i++) {
                            if (result[i].ItemUnit == purchaseUnit && result[i].PurchaseUnit == purchaseUnit) {
                                var conversion = "1 " + result[i].ItemUnitText + " = " + 1 + " " + result[i].ItemUnitText;
                                $('#<%=txtConversion.ClientID %>').val(conversion);
                                $('#<%=hdnConversionFactor.ClientID %>').val('1');
                                if (paramProcess != "edit") {
                                    $('#<%=txtPrice.ClientID %>').val(result[i].Price).trigger('changeValue');
                                    $('#<%=hdnPurchaseUnitPrice.ClientID %>').val(result[i].Price);
                                }
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
                                break;
                            } else if (result[i].PurchaseUnit == purchaseUnit) {
                                var conversion = "1 " + result[i].PurchaseUnitText + " = " + result[i].ConversionFactor + " " + result[i].ItemUnitText;
                                $('#<%=txtConversion.ClientID %>').val(conversion);
                                $('#<%=hdnConversionFactor.ClientID %>').val(result[i].ConversionFactor);
                                if (paramProcess != "edit") {
                                    $('#<%=txtPrice.ClientID %>').val(result[i].UnitPrice).trigger('changeValue');
                                    $('#<%=hdnPurchaseUnitPrice.ClientID %>').val(result[i].UnitPrice);
                                }
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
                                break;
                            }
                            else {
                                //change conversion factor
                                $('#<%=txtBaseUnit.ClientID %>').val("per " + cboItemUnit.GetText());
                                if (baseValue == toUnitItem) {
                                    $('#<%=hdnConversionFactor.ClientID %>').val('1');
                                    var conversion = "1 " + cboItemUnit.GetText() + " = 1 " + cboItemUnit.GetText();
                                    $('#<%=txtConversion.ClientID %>').val(conversion);
                                }
                                else {
                                    var itemID = $('#<%=hdnItemID.ClientID %>').val();
                                    var filterExpression = "ItemID = " + itemID + " AND GCAlternateUnit = '" + toUnitItem + "' AND IsActive = 1 AND IsDeleted = 0";
                                    Methods.getObject('GetvItemAlternateUnitList', filterExpression, function (resultAlt) {
                                        if (resultAlt != null) {
                                            var toConversion = resultAlt.AlternateUnit;
                                            $('#<%=hdnConversionFactor.ClientID %>').val(resultAlt.ConversionFactor);
                                            var conversion = "1 " + toConversion + " = " + resultAlt.ConversionFactor + " " + resultAlt.ItemUnit;
                                            $('#<%=txtConversion.ClientID %>').val(conversion);
                                            factor = resultAlt.ConversionFactor;
                                        }
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
                                            pricePerPurchaseUnit = result[i].Price * convFactor;
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
                    }
                    else {
                        //change conversion factor
                        $('#<%=txtBaseUnit.ClientID %>').val("per " + cboItemUnit.GetText());
                        if (baseValue == toUnitItem) {
                            $('#<%=hdnConversionFactor.ClientID %>').val('1');
                            var conversion = "1 " + cboItemUnit.GetText() + " = 1 " + cboItemUnit.GetText();
                            $('#<%=txtConversion.ClientID %>').val(conversion);
                        }
                        else {
                            var itemID = $('#<%=hdnItemID.ClientID %>').val();
                            var filterExpression = "ItemID = " + itemID + " AND GCAlternateUnit = '" + toUnitItem + "' AND IsActive = 1 AND IsDeleted = 0";
                            Methods.getObject('GetvItemAlternateUnitList', filterExpression, function (resultAlt) {
                                if (resultAlt != null) {
                                    $('#<%=txtPrice.ClientID %>').val(((result[i].UnitPrice / result[i].ConversionFactor) * resultAlt.ConversionFactor)).trigger('changeValue');
                                    $('#<%=hdnPurchaseUnitPrice.ClientID %>').val(((result[i].UnitPrice / result[i].ConversionFactor) * resultAlt.ConversionFactor));

                                    var toConversion = resultAlt.AlternateUnit;
                                    $('#<%=hdnConversionFactor.ClientID %>').val(resultAlt.ConversionFactor);
                                    var conversion = "1 " + toConversion + " = " + resultAlt.ConversionFactor + " " + resultAlt.ItemUnit;
                                    $('#<%=txtConversion.ClientID %>').val(conversion);
                                    factor = resultAlt.ConversionFactor;
                                }
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
            }

            $('#<%=txtBaseUnit.ClientID %>').val("per " + cboItemUnit.GetText());

            factor = $('#<%=hdnConversionFactor.ClientID %>').val();

            if (factor != 1) {
                if (factorOld == 1) {
                    qtyEnd = qtyBegin / factor;
                } else {
                    if (factorOld != 0) {
                        qtyEnd = (qtyBegin * factorOld) / factor;
                    }
                    else {
                        qtyBegin;
                    }
                }
            } else {
                if (factorOld != 0) {
                    qtyEnd = qtyBegin * factorOld;
                } else {
                    qtyEnd = qtyBegin;
                }
            }

            if (paramProcess != "edit") {
                $('#<%=txtQuantity.ClientID %>').val(parseFloat(qtyEnd).toFixed(2));
                $('#<%=txtQuantity.ClientID %>').trigger('change');
            }

            $('#<%=hdnTempFactor.ClientID %>').val(factor);

            calculateSubTotal();
        }

        //#endregion

        function onAfterSaveRecordDtSuccess(OrderID) {
            if ($('#<%=hdnOrderID.ClientID %>').val() == '0') {
                $('#<%=hdnOrderID.ClientID %>').val(OrderID);
                var filterExpression = 'PurchaseOrderID = ' + OrderID;
                Methods.getObject('GetPurchaseOrderHdList', filterExpression, function (result) {
                    $('#<%=txtOrderNo.ClientID %>').val(result.PurchaseOrderNo);
                    onLoadObject(result.PurchaseOrderNo);
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
                    var isEdit = $('#<%=hdnIsEdit.ClientID %>').val();
                    if (isEdit == '1') {
                        $('#containerEntry').hide();
                    } else if (isEdit == '0') {
                        $('#lblAddData').click();
                    }
                    cbpView.PerformCallback('refresh');
                }
            }
            else if (param[0] == 'recalculate') {
                $('#<%=tdRecalculate.ClientID %>').attr('style', 'display:none');
                $('#<%=trWarning.ClientID %>').attr('style', 'display:none');

                if (param[1] == 'fail') {
                    showToast('Recalculate Failed', 'Error Message : ' + param[2]);
                }
                cbpView.PerformCallback('refresh');
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

        //#region Right Panel
        function setRightPanelButtonEnabled() {
            if ($('#<%:hdnOrderID.ClientID %>').val() == '' || $('#<%:hdnOrderID.ClientID %>').val() == '0') {
                $('#btnEntryTermPO').attr('enabled', 'false');
                $('#btnDocumentNotes').attr('enabled', 'false');
                $('#btnInfoSupplier').attr('enabled', 'false');
                $('#btnInfoPurchaseOrder').attr('enabled', 'false');
                $('#btnInfoPurchaseOrderCloseNew').attr('enabled', 'false');
            }
            else {
                var transStatus = $('#<%:hdnGCTransactionStatus.ClientID %>').val();
                var isUsingTermPO = $('#<%:hdnIsUsingTermPO.ClientID %>').val();
                if (transStatus != "X121^999" && isUsingTermPO == "1") {
                    $('#btnEntryTermPO').removeAttr('enabled');
                } else {
                    $('#btnEntryTermPO').attr('enabled', 'false');
                }
                $('#btnInfoPurchaseOrder').removeAttr('enabled');
                $('#btnDocumentNotes').removeAttr('enabled');
                $('#btnInfoSupplier').removeAttr('enabled');
                $('#btnInfoPurchaseOrderCloseNew').removeAttr('enabled');
            }
        }

        function onBeforeLoadRightPanelContent(code) {
            if (code == 'infoSupplier') {
                var param = $('#<%:hdnSupplierID.ClientID %>').val();
                return param;
            }
            else if (code == 'infoStockPerLocation') {
                var param = $('#<%:hdnLocationID.ClientID %>').val();
                return param;
            }
            else if (code == 'documentNotes') {
                var param = 'PO' + '|' + $('#<%:hdnOrderID.ClientID %>').val();
                return param;
            }
            else if (code == 'infoCloseAndNew') {
                var param = $('#<%=txtOrderNo.ClientID %>').val();
                return param;
            }
            else if (code == 'termPO') {
                var param = $('#<%:hdnOrderID.ClientID %>').val();
                return param;
            }
            else {
                return $('#<%:hdnOrderID.ClientID %>').val();
            }
        }

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var purchaseOrderID = $('#<%=hdnOrderID.ClientID %>').val();
            var printStatus = $('#<%=hdnPrintStatus.ClientID %>').val();
            var purchasingType = $('#<%=hdnPurchasingType.ClientID %>').val();
            var purchaseorderType = $('#<%=hdnPurchaseOrderType.ClientID %>').val();
            if (GCTransactionStatus = Constant.TransactionStatus.OPEN) {
                if (code == 'IM-00124' || code == 'IM-00125' || code == 'IM-00126' || code == 'IM-00127' || code == 'IM-00139' ||
                    code == 'IM-00142' || code == "IM-00210" || code == "IM-00211") {
                    filterExpression.text = "PurchaseOrderID = " + purchaseOrderID;
                    return true;
                }
            } else {
                return false;
            }
            if (printStatus == 'true') {
                if (code == 'IM-00002' || code == 'IM-00003' || code == 'IM-00025' || code == 'IM-00029' || code == 'IM-00129' ||
                    code == 'IM-00130' || code == 'IM-00161' || code == 'IM-00170' || code == 'IM-00171' || code == 'IM-00172' ||
                    code == 'IM-00185' || code == 'IM-00189' || code == 'IM-00190') {
                    if (purchaseOrderID == '' || purchaseOrderID == '0') {
                        errMessage.text = 'Please Set Transaction First!';
                        return false;
                    }
                    else {
                        filterExpression.text = "PurchaseOrderID = " + purchaseOrderID;
                        cbpProcess.PerformCallback('updatePrintNo');
                        return true;
                    }
                }
                else if (purchaseorderType == '') {
                    filterExpression.text = "PurchaseOrderID = " + purchaseOrderID;
                    cbpProcess.PerformCallback('updatePrintNo');
                    return true;
                }
                else if (code == 'IM-00132' || code == 'IM-00133') { //Jenis Pemesanan COVID
                    if (purchaseorderType == Constant.PurchaseOrderType.COVID) {
                        filterExpression.text = "PurchaseOrderID = " + purchaseOrderID;
                        cbpProcess.PerformCallback('updatePrintNo');
                        return true;
                    }
                    else {
                        errMessage.text = "Tidak bisa cetak karena bukan Pemesanan COVID";
                        return false;
                    }
                }
                else if (purchasingType == '') {
                    filterExpression.text = "PurchaseOrderID = " + purchaseOrderID;
                    cbpProcess.PerformCallback('updatePrintNo');
                    return true;
                }
                else if (code == 'IM-00050' || code == 'IM-00051' || code == 'IM-00136' || code == 'IM-00146' || code == 'IM-00148' || code == 'IM-00181'
                        || code == 'IM-00258') { //Pembelian Rutin
                    if (purchasingType == Constant.PurchasingType.RUTIN) {
                        filterExpression.text = "PurchaseOrderID = " + purchaseOrderID;
                        cbpProcess.PerformCallback('updatePrintNo');
                        return true;
                    }
                    else {
                        errMessage.text = 'Tidak bisa cetak karena bukan Pembelian Rutin';
                        return false;
                    }
                }
                else if (code == 'IM-00095' || code == 'IM-00096' || code == 'IM-00141' || code == 'IM-00147' || code == 'IM-00149' || code == 'IM-00259') { //Pembelian Non Rutin
                    if (purchasingType == Constant.PurchasingType.NON_RUTIN) {
                        filterExpression.text = "PurchaseOrderID = " + purchaseOrderID;
                        cbpProcess.PerformCallback('updatePrintNo');
                        return true;
                    }
                    else {
                        errMessage.text = 'Tidak bisa cetak karena bukan Pembelian Non Rutin';
                        return false;
                    }
                }
                else if (code == 'IM-00162' || code == 'IM-00163' || code == 'IM-00164' || code == "IM-00165") {
                    filterExpression.text = "PurchaseOrderID = " + purchaseOrderID + "|" + code;
                    return true;
                }
                else {
                    if (purchaseOrderID == '' || purchaseOrderID == '0') {
                        errMessage.text = 'Please Set Transaction First!';
                        return false;
                    } else {
                        filterExpression.text = "PurchaseOrderID = " + purchaseOrderID;
                        cbpProcess.PerformCallback('updatePrintNo');
                        return true;
                    }
                }
            }
            else {
                errMessage.text = "Tidak dapat cetak karena status pemesanan belum diperbolehkan untuk cetak sekarang.";
                return false;
            }
        }

        //#endregion

        $('#btnRecalculate').live('click', function () {
            cbpProcess.PerformCallback('recalculate');
        });

        function onCboPurchasingType() {
            $('#<%=hdnGCPurchasingType.ClientID %>').val(cboPurchasingType.GetValue());
        }

        function onAfterSaveAddRecordEntryPopup(param) {
            var filterExpression = 'PurchaseOrderID = ' + param;
            Methods.getObject('GetPurchaseOrderHdList', filterExpression, function (result) {
                if (result != null) {
                    onLoadObject(result.PurchaseOrderNo);
                }
            });
        }
    </script>
    <input type="hidden" value="" id="hdnIsShowDefaultLocation" runat="server" />
    <input type="hidden" value="" id="hdnPrintStatus" runat="server" />
    <input type="hidden" value="" id="hdnSearchDialogType" runat="server" />
    <input type="hidden" value="" id="hdnVATPercentageFromSetvar" runat="server" />
    <input type="hidden" value="" id="hdnVATPercentage" runat="server" />
    <input type="hidden" value="" id="hdnPPHPercentage" runat="server" />
    <input type="hidden" value="" id="hdnFinalDiscount" runat="server" />
    <input type="hidden" value="" id="hdnOrderID" runat="server" />
    <input type="hidden" value="" id="hdnPageCount" runat="server" />
    <input type="hidden" value="1" id="hdnIsEditable" runat="server" />
    <input type="hidden" value="" id="hdnRecordFilterExpression" runat="server" />
    <input type="hidden" value="0" id="hdnIsEdit" runat="server" />
    <input type="hidden" value="0" id="hdnTempFactor" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnPurchaseOrderType" runat="server" />
    <input type="hidden" value="0" id="hdnIsUsedRevenueCostCenter" runat="server" />
    <input type="hidden" value="0" id="hdnIsUsedProductLine" runat="server" />
    <input type="hidden" value="0" id="hdnGCTransactionStatus" runat="server" />
    <input type="hidden" value="0" id="hdnIsUsingTermPO" runat="server" />
    <input type="hidden" value="0" id="hdnIsAllowPrintOrderReceiptAfterProposed" runat="server" />
    <input type="hidden" value="0" id="hdnIsPpnAllowChanged" runat="server" />
    <input type="hidden" id="hdnGCPurchaseOrderType" value="" runat="server" />
    <input type="hidden" value="" id="hdnPurchasingType" runat="server" />
    <input type="hidden" id="hdnGCPurchasingType" value="" runat="server" />
    <input type="hidden" value="" id="hdnQtyEndLocation" runat="server" />
    <input type="hidden" value="" id="hdnGCItemUnitQtyEndLocation" runat="server" />
    <input type="hidden" value="" id="hdnIsUsingDownPayment" runat="server" />
    <input type="hidden" value="" id="hdnIM0131" runat="server" />
    <input type="hidden" value="" id="hdnIsPOQtyCannotOverPRQty" runat="server" />
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
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td id="tdRecalculate" style="display: none" runat="server">
                                            <input type="button" id="btnRecalculate" value="Rekalkulasi Harga" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" id="lblETA" runat="server">
                                    <%=GetLabel("Rata Rata Waktu Pengiriman")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtETA" ReadOnly="true" Width="25%" runat="server" />
                            </td>
                        </tr>
                        <tr id="hdnLocation" runat="server">
                            <td class="tdLabel">
                                <label class="lblMandatory lblLink" runat="server" id="lblLocation">
                                    <%=GetLabel("Lokasi")%></label>
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
                        <tr id="trRevenueCostCenter" runat="server">
                            <td class="tdLabel">
                                <label class="lblLink" runat="server" id="lblRevenueCostCenter">
                                    <%=GetLabel("Revenue Cost Center")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnRevenueCostCenterID" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtRevenueCostCenterCode" Width="100%" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtRevenueCostCenterName" ReadOnly="true" Width="100%" />
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
                                    <%=GetLabel("Jenis Pembelian")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboPurchasingType" ClientInstanceName="cboPurchasingType" Width="45%"
                                    runat="server">
                                    <ClientSideEvents ValueChanged="function(s,e){ onCboPurchasingType(); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Jenis Pemesanan")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboPurchaseOrderType" ClientInstanceName="cboPurchaseOrderType"
                                    Width="45%" runat="server">
                                </dxe:ASPxComboBox>
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
                                <%=GetLabel("No. Referensi (Lainnya)") %>
                            </td>
                            <td>
                                <asp:TextBox ID="txtOtherReferenceNo" Width="45%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("No. Referensi Permintaan (Lainnya)") %>
                            </td>
                            <td>
                                <asp:TextBox ID="txtOtherRequestReferenceNo" Width="45%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkIsUrgent" Text=" CITO" Font-Bold="true" Checked="false" Width="100%"
                                    runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkIsCampaign" Text=" CAMPAIGN" Font-Bold="true" Checked="false"
                                    Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr id="trWarning" runat="server" style="display: none;">
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <table class="tblEntryContent" style="width: 100%">
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td style="vertical-align: top" align="right">
                                            <label class="lblWarning">
                                                <%=GetLabel("Mohon Lakukan Rekalkulasi Setelah Ubah Supplier") %></label>
                                        </td>
                                        <td rowspan="1">
                                            <img height="50" src='<%= ResolveUrl("~/Libs/Images/warning.png")%>' alt='' />
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
                                                <col style="width: 170px" />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkIsBonus" Width="100%" runat="server" Checked="true" Text=" Bonus" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblLink" id="lblItemGroup">
                                                        <%=GetLabel("Kelompok Item")%></label>
                                                </td>
                                                <td>
                                                    <input type="hidden" value="" id="hdnPurchaseRequestNoInfo" runat="server" />
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
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("Kategori Budget")%></label>
                                                </td>
                                                <td colspan="2">
                                                    <dxe:ASPxComboBox ID="cboBudgetCategory" ClientInstanceName="cboBudgetCategory" Width="250px"
                                                        runat="server">
                                                    </dxe:ASPxComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label>
                                                        <%=GetLabel("Stok | Qty on Order")%></label>
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
                                                                <asp:TextBox ID="txtStockLocation" ReadOnly="true" CssClass="number" Width="120px"
                                                                    runat="server" />
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtQtyOnProcess" ReadOnly="true" Width="120px" runat="server" Style="text-align: right" />
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
                                                    <asp:TextBox ID="txtQuantity" Width="120px" CssClass="number" Min="0" runat="server" />
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
                                                    <asp:TextBox ID="txtConversion" Width="300px" runat="server" ReadOnly="true" />
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
                                                <td colspan="2">
                                                    <asp:TextBox ID="txtSubTotalPrice" Width="180px" ReadOnly="true" runat="server" CssClass="txtCurrency" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Catatan")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtNotesDt" Width="100%" runat="server" TextMode="MultiLine" Rows="3" />
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
                                        OnRowDataBound="grdView_RowDataBound" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true"
                                        EmptyDataRowStyle-CssClass="trEmpty">
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
                                                    <input type="hidden" value="<%#:Eval("QtyENDLocation") %>" bindingfield="QtyENDLocation" />
                                                    <input type="hidden" value="<%#:Eval("ItemUnitQtyEndLocation") %>" bindingfield="ItemUnitQtyEndLocation" />
                                                    <input type="hidden" value="<%#:Eval("GCPurchaseUnit") %>" bindingfield="GCPurchaseUnit" />
                                                    <input type="hidden" value="<%#:Eval("GCBaseUnit") %>" bindingfield="GCBaseUnit" />
                                                    <input type="hidden" value="<%#:Eval("PurchaseUnit") %>" bindingfield="PurchaseUnit" />
                                                    <input type="hidden" value="<%#:Eval("BaseUnit") %>" bindingfield="BaseUnit" />
                                                    <input type="hidden" value="<%#:Eval("UnitPrice") %>" bindingfield="UnitPrice" />
                                                    <input type="hidden" value="<%#:Eval("IsDiscountInPercentage1")%>" bindingfield="IsDiscountInPercentage1" />
                                                    <input type="hidden" value="<%#:Eval("IsDiscountInPercentage2")%>" bindingfield="IsDiscountInPercentage2" />
                                                    <input type="hidden" value="<%#:Eval("DiscountPercentage1") %>" bindingfield="DiscountPercentage1" />
                                                    <input type="hidden" value="<%#:Eval("DiscountPercentage2") %>" bindingfield="DiscountPercentage2" />
                                                    <input type="hidden" value="<%#:Eval("cfDiscountPercentage1") %>" bindingfield="cfDiscountPercentage1" />
                                                    <input type="hidden" value="<%#:Eval("cfDiscountPercentage2") %>" bindingfield="cfDiscountPercentage2" />
                                                    <input type="hidden" value="<%#:Eval("DiscountAmount1") %>" bindingfield="DiscountAmount1" />
                                                    <input type="hidden" value="<%#:Eval("DiscountAmount2") %>" bindingfield="DiscountAmount2" />
                                                    <input type="hidden" value="<%#:Eval("ConversionFactor") %>" bindingfield="ConversionFactor" />
                                                    <input type="hidden" value="<%#:Eval("GCItemDetailStatus") %>" bindingfield="GCItemDetailStatus" />
                                                    <input type="hidden" value="<%#:Eval("CustomSubTotal") %>" bindingfield="CustomSubTotal" />
                                                    <input type="hidden" value="<%#:Eval("CustomTotal") %>" bindingfield="CustomTotal" />
                                                    <input type="hidden" value="<%#:Eval("Remarks") %>" bindingfield="Remarks" />
                                                    <input type="hidden" value="<%#:Eval("IsBonusItem") %>" bindingfield="IsBonusItem" />
                                                    <input type="hidden" value="<%#:Eval("PurchaseRequestNoInfo") %>" bindingfield="PurchaseRequestNoInfo" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center" HeaderText="Bonus"
                                                HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkIsBonus" Enabled="false" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Nama Barang" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
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
                                                                <%#: Eval("PurchaseRequestNoInfo")%></span></div>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Quantity" HeaderText="Quantity" HeaderStyle-Width="80px"
                                                HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" />
                                            <asp:BoundField DataField="PurchaseUnit" HeaderText="Satuan Pemesanan" HeaderStyle-Width="100px"
                                                ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="CustomConversion" HeaderText="Faktor Konversi" HeaderStyle-Width="200px"
                                                ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="UnitPrice" HeaderText="Harga / Satuan" HeaderStyle-Width="120px"
                                                ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" />
                                            <asp:TemplateField HeaderStyle-Width="80px" HeaderText="Disc1(%)" ItemStyle-HorizontalAlign="Right"
                                                HeaderStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <div>
                                                        <asp:CheckBox ID="chkIsDiscInPct1" runat="server" CssClass="chkIsDiscInPct1" Enabled="false" />
                                                        <span style="font-style: normal;">
                                                            <%#: Eval("cfDiscPct1InString")%></span></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="80px" HeaderText="Disc1 (Rp)" ItemStyle-HorizontalAlign="Right"
                                                HeaderStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <div>
                                                        <span style="font-style: normal;">
                                                            <%#: Eval("cfDiscAmount1InString")%></span></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="80px" HeaderText="Disc2(%)" ItemStyle-HorizontalAlign="Right"
                                                HeaderStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <div>
                                                        <asp:CheckBox ID="chkIsDiscInPct2" runat="server" CssClass="chkIsDiscInPct2" Enabled="false" />
                                                        <span style="font-style: normal;">
                                                            <%#: Eval("cfDiscPct2InString")%></span></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="80px" HeaderText="Disc2 (Rp)" ItemStyle-HorizontalAlign="Right"
                                                HeaderStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <div>
                                                        <span style="font-style: normal;">
                                                            <%#: Eval("cfDiscAmount2InString")%></span></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="LineAmount" HeaderText="Sub Total" ItemStyle-HorizontalAlign="Right"
                                                HeaderStyle-HorizontalAlign="Right" HeaderStyle-Width="150px" DataFormatString="{0:N2}" />
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Tidak ada data pemesanan barang")%>
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
                            <%= GetLabel("Tambah Barang")%></span> <span class="lblLink" id="lblSalinPR" style="margin-right: 200px;">
                                <%= GetLabel("Salin Permintaan Pembelian")%></span>
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
                                            <tr id="trTotalOrder" runat="server">
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Jumlah Nilai Pemesanan")%></label>
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
                                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width: 5%" />
                                                            <col />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <asp:CheckBox ID="chkIsFinalDiscountInPercentage" Checked="true" Width="100%" runat="server" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtFinalDiscountInPercentage" CssClass="txtCurrency" Width="160px"
                                                                    runat="server" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr id="trDiscountFinal" runat="server" style="display: none">
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Diskon Final")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtFinalDiscount" CssClass="txtCurrency" ReadOnly="true" Width="180px"
                                                        runat="server" hiddenval="0" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("PPN")%>
                                                        <asp:TextBox ID="txtVATPercentageDefault" CssClass="txtCurrency" ReadOnly="true"
                                                            Width="40px" runat="server" />
                                                        %
                                                    </label>
                                                </td>
                                                <td>
                                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width: 5%" />
                                                            <col />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <asp:CheckBox ID="chkPPN" Width="100%" runat="server" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtPPN" CssClass="txtCurrency" Width="160px" runat="server" hiddenval="0" />
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
                                                <td>
                                                    <dxe:ASPxComboBox ID="cboPPHType" ClientInstanceName="cboPPHType" Width="185px" runat="server" />
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
                                                            <col style="width: 25%" />
                                                            <col />
                                                        </colgroup>
                                                        <tr>
                                                            <td class="tdLabel" style="text-align: right; width: 100px;">
                                                                <dxe:ASPxComboBox ID="cboPPHOptions" ClientInstanceName="cboPPHOptions" runat="server"
                                                                    Width="80px">
                                                                    <ClientSideEvents ValueChanged="function(s,e) { oncboPPHOptionsValueChanged(e); }"
                                                                        SelectedIndexChanged="calculateTotal" />
                                                                </dxe:ASPxComboBox>
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="chkIsPPHInPercentage" Checked="true" Width="100%" runat="server"
                                                                    Style="display: none" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtPPHPercentage" CssClass="txtCurrency" Width="70px" runat="server" />
                                                                <%=GetLabel("%")%>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtPPHAmount" CssClass="txtCurrency" ReadOnly="true" Width="150px"
                                                                    runat="server" />
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
                                            <tr id="trTotalOrderSaldo" runat="server">
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Total Nilai Pemesanan")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtTotalOrderSaldo" CssClass="txtCurrency" ReadOnly="true" Width="180px"
                                                        runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Menggunakan Termin")%></label>
                                                </td>
                                                <td>
                                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width: 5%" />
                                                            <col />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <asp:CheckBox ID="chkIsUsingTermPO" Checked="false" Width="100%" runat="server" />
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
                                                        <%=GetLabel("Jumlah Cetak") %>
                                                    </td>
                                                    <td align="center">
                                                        :
                                                    </td>
                                                    <td>
                                                        <div runat="server" id="divPrintNumber">
                                                        </div>
                                                    </td>
                                                </tr>
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
