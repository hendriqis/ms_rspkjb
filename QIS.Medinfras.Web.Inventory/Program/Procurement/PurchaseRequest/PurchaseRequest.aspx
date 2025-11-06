<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx4.master"
    AutoEventWireup="true" CodeBehind="PurchaseRequest.aspx.cs" Inherits="QIS.Medinfras.Web.Inventory.Program.PurchaseRequest" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">

        function getSupplierFilterExpression() {
            var filterExpression = "<%:filterExpressionSupplier %>";
            return filterExpression;
        }

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

        function onLoad() {
            setRightPanelButtonEnabled();

            if ($('#<%=hdnIsEditable.ClientID %>').val() == '1') {
                $('#lblAddData').show();
                $('#lblQuickPick').show();
                $('#lblEntryPackage').show();
            }
            else {
                $('#lblAddData').hide();
                $('#lblQuickPick').hide();
                $('#lblEntryPackage').hide();
            }

            if ($('#<%=hdnRequestID.ClientID %>').val() == "" || $('#<%=hdnRequestID.ClientID %>').val() == "0") {
                setDatePicker('<%=txtItemOrderDate.ClientID %>');
            }
            $('#<%=txtItemOrderDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

            //#region Order No
            $('#lblOrderNo.lblLink').click(function () {
                openSearchDialog('purchaserequesthd', "<%=GetFilterExpression() %>", function (value) {
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

            //#region Location From
            function getLocationFilterExpression() {
                var filterExpression = "<%:filterExpressionLocation %>";
                if ($('#<%=hdnLocationItemGroupID.ClientID %>').val() != '' && $('#<%=hdnLocationItemGroupID.ClientID %>').val() != '0')
                    filterExpression += " AND ParentID = " + $('#<%=hdnLocationItemGroupID.ClientID %>').val();
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

                        filterExpression = "LocationID = " + result.LocationID;
                        Methods.getObject('GetLocationList', filterExpression, function (result) {
                            $('#<%=hdnLocationItemGroupID.ClientID %>').val(result.ItemGroupID);
                            $('#<%=hdnGCLocationGroup.ClientID %>').val(result.GCLocationGroup);
                        });
                    }
                    else {
                        $('#<%=hdnLocationIDFrom.ClientID %>').val('');
                        $('#<%=txtLocationCode.ClientID %>').val('');
                        $('#<%=txtLocationName.ClientID %>').val('');
                        $('#<%=hdnLocationItemGroupID.ClientID %>').val('');
                        $('#<%=hdnGCLocationGroup.ClientID %>').val('');
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

            //#region Supplier
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
                var hdnIM0131 = $('#<%=hdnIM0131.ClientID %>').val();
                var orderDate = $('#<%:txtItemOrderDate.ClientID %>').val();
                var orderDateInDatePicker = Methods.getDatePickerDate(orderDate);
                var orderDateFormatString = Methods.dateToString(orderDateInDatePicker);

                var filterExpression = getSupplierFilterExpression() + " AND BusinessPartnerCode = '" + value + "'";
                Methods.getObject('GetBusinessPartnersList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnSupplierID.ClientID %>').val(result.BusinessPartnerID);
                        $('#<%=txtSupplierName.ClientID %>').val(result.BusinessPartnerName);
                        var ItemID = $('#<%=hdnItemID.ClientID %>').val();

                        if (hdnIM0131 == "0") {
                            Methods.getItemMasterPurchase(ItemID, result.BusinessPartnerID, function (result2) {
                                if (result2 != null) {
                                    $('#<%=hdnItemGroupID.ClientID %>').val(result2.ItemGroupID);
                                    $('#<%=txtItemGroupCode.ClientID %>').val(result2.ItemGroupCode);
                                    $('#<%=txtItemGroupName.ClientID %>').val(result2.ItemGroupName1);
                                    $('#<%=hdnSupplierID.ClientID %>').val(result2.BusinessPartnerID);
                                    $('#<%=txtSupplierCode.ClientID %>').val(result2.BusinessPartnerCode);
                                    $('#<%=txtSupplierName.ClientID %>').val(result2.BusinessPartnerName);
                                    $('#<%=txtDiscount.ClientID %>').val(result2.Discount);
                                    $('#<%=txtDiscount2.ClientID %>').val(result2.Discount2);
                                    $('#<%=hdnPrice.ClientID %>').val(result2.Price);
                                    $('#<%=hdnPurchaseUnitPrice.ClientID %>').val(result2.UnitPrice);
                                    $('#<%=txtPrice.ClientID %>').val(result2.UnitPrice).trigger('changeValue');
                                    $('#<%=hdnGCBaseUnit.ClientID %>').val(result2.ItemUnit);
                                    $('#<%=hdnGCItemUnit.ClientID %>').val(result2.PurchaseUnit);

                                    if ($('#<%=hdnSupplierID.ClientID %>').val() != '') {
                                        Methods.getDateDiffPOPORPerSupplier($('#<%=hdnSupplierID.ClientID %>').val(), function (result) {
                                            if (result != null) {
                                                $('#<%=txtETA.ClientID %>').val(result.Hasil + ' Hari');
                                            }
                                            else {
                                                $('#<%=txtETA.ClientID %>').val('');
                                            }
                                        });
                                    }
                                }
                                else {
                                    $('#<%=txtDiscount.ClientID %>').val('0');
                                    $('#<%=txtDiscount2.ClientID %>').val('0');
                                    $('#<%=txtPrice.ClientID %>').val('0.00');
                                    $('#<%=hdnPrice.ClientID %>').val('0.00');
                                    $('#<%=hdnPurchaseUnitPrice.ClientID %>').val('0.00');
                                    $('#<%=txtETA.ClientID %>').val('');
                                }
                            });
                        }
                        else {
                            Methods.getItemMasterPurchaseWithDate(ItemID, result.BusinessPartnerID, orderDateFormatString, function (result2) {
                                if (result2 != null) {
                                    $('#<%=hdnItemGroupID.ClientID %>').val(result2.ItemGroupID);
                                    $('#<%=txtItemGroupCode.ClientID %>').val(result2.ItemGroupCode);
                                    $('#<%=txtItemGroupName.ClientID %>').val(result2.ItemGroupName1);
                                    $('#<%=hdnSupplierID.ClientID %>').val(result2.BusinessPartnerID);
                                    $('#<%=txtSupplierCode.ClientID %>').val(result2.BusinessPartnerCode);
                                    $('#<%=txtSupplierName.ClientID %>').val(result2.BusinessPartnerName);
                                    $('#<%=txtDiscount.ClientID %>').val(result2.Discount);
                                    $('#<%=txtDiscount2.ClientID %>').val(result2.Discount2);
                                    $('#<%=hdnPrice.ClientID %>').val(result2.Price);
                                    $('#<%=hdnPurchaseUnitPrice.ClientID %>').val(result2.UnitPrice);
                                    $('#<%=txtPrice.ClientID %>').val(result2.UnitPrice).trigger('changeValue');
                                    $('#<%=hdnGCBaseUnit.ClientID %>').val(result2.ItemUnit);
                                    $('#<%=hdnGCItemUnit.ClientID %>').val(result2.PurchaseUnit);

                                    if ($('#<%=hdnSupplierID.ClientID %>').val() != '') {
                                        Methods.getDateDiffPOPORPerSupplier($('#<%=hdnSupplierID.ClientID %>').val(), function (result) {
                                            if (result != null) {
                                                $('#<%=txtETA.ClientID %>').val(result.Hasil + ' Hari');
                                            }
                                            else {
                                                $('#<%=txtETA.ClientID %>').val('');
                                            }
                                        });
                                    }
                                }
                                else {
                                    $('#<%=txtDiscount.ClientID %>').val('0');
                                    $('#<%=txtDiscount2.ClientID %>').val('0');
                                    $('#<%=txtPrice.ClientID %>').val('0.00');
                                    $('#<%=hdnPrice.ClientID %>').val('0.00');
                                    $('#<%=hdnPurchaseUnitPrice.ClientID %>').val('0.00');
                                    $('#<%=txtETA.ClientID %>').val('');
                                }
                            });
                        }
                        cboItemUnit.PerformCallback();
                    }
                    else {
                        $('#<%=hdnSupplierID.ClientID %>').val('');
                        $('#<%=txtSupplierCode.ClientID %>').val('');
                        $('#<%=txtSupplierName.ClientID %>').val('');
                        $('#<%=txtETA.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Item Group
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
                var filterExpression = "IsDeleted = 0 AND GCItemStatus != 'X181^999'";
                var requestID = $('#<%=hdnRequestID.ClientID %>').val();

                if ($('#<%=txtItemGroupCode.ClientID %>').val() != '') {
                    filterExpression += " AND ItemGroupID IN (SELECT ItemGroupID FROM vItemGroupMaster1 WHERE DisplayPath like '%/" + $('#<%=hdnItemGroupID.ClientID %>').val() + "/%')";
                }
                else if ($('#<%=hdnLocationItemGroupID.ClientID %>').val() != '' && $('#<%=hdnLocationItemGroupID.ClientID %>').val() != '0') {
                    filterExpression += " AND ItemGroupID IN (SELECT ItemGroupID FROM vItemGroupMaster1 WHERE DisplayPath like '%/" + $('#<%=hdnLocationItemGroupID.ClientID %>').val() + "/%')";
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

                if (requestID != '') {
                    filterExpression += " AND ItemID NOT IN (SELECT ItemID FROM PurchaseRequestDt WHERE PurchaseRequestID = " + requestID + " AND IsDeleted = 0)";
                }

                filterExpression += " AND ItemID IN (SELECT ItemID FROM ItemBalance WHERE IsDeleted = 0 AND LocationID = " + $('#<%=hdnLocationIDFrom.ClientID %>').val() + ")";

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
                var hdnIM0131 = $('#<%=hdnIM0131.ClientID %>').val();
                var filterExpression = getItemFilterExpression() + " AND ItemCode = '" + value + "'";
                Methods.getObject('GetvItemMasterList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnItemID.ClientID %>').val(result.ItemID);
                        $('#<%=txtItemName.ClientID %>').val(result.ItemName1);

                        if (hdnIM0131 == "0") {
                            Methods.getItemMasterPurchase(result.ItemID, 0, function (result2) {
                                if (result2 != null) {
                                    $('#<%=hdnItemGroupID.ClientID %>').val(result2.ItemGroupID);
                                    $('#<%=txtItemGroupCode.ClientID %>').val(result2.ItemGroupCode);
                                    $('#<%=txtItemGroupName.ClientID %>').val(result2.ItemGroupName1);
                                    $('#<%=hdnSupplierID.ClientID %>').val(result2.BusinessPartnerID);
                                    $('#<%=txtSupplierCode.ClientID %>').val(result2.BusinessPartnerCode);
                                    $('#<%=txtSupplierName.ClientID %>').val(result2.BusinessPartnerName);
                                    $('#<%=txtDiscount.ClientID %>').val(result2.Discount);
                                    $('#<%=txtDiscount2.ClientID %>').val(result2.Discount2);
                                    $('#<%=hdnPrice.ClientID %>').val(result2.Price);
                                    $('#<%=hdnPurchaseUnitPrice.ClientID %>').val(result2.UnitPrice);
                                    $('#<%=hdnGCBaseUnit.ClientID %>').val(result2.ItemUnit);
                                    $('#<%=hdnGCItemUnit.ClientID %>').val(result2.PurchaseUnit);

                                    if ($('#<%=hdnSupplierID.ClientID %>').val() != '') {
                                        Methods.getDateDiffPOPORPerSupplier($('#<%=hdnSupplierID.ClientID %>').val(), function (result) {
                                            if (result != null) {
                                                $('#<%=txtETA.ClientID %>').val(result.Hasil + ' Hari');
                                            }
                                            else {
                                                $('#<%=txtETA.ClientID %>').val('');
                                            }
                                        });
                                    }
                                }
                                else {
                                    $('#<%=txtDiscount.ClientID %>').val('0');
                                    $('#<%=txtDiscount2.ClientID %>').val('0');
                                    $('#<%=txtPrice.ClientID %>').val('0.00');
                                    $('#<%=hdnPrice.ClientID %>').val('0.00');
                                    $('#<%=hdnPurchaseUnitPrice.ClientID %>').val('0.00');
                                    $('#<%=txtETA.ClientID %>').val('');
                                    $('#<%=hdnGCBaseUnit.ClientID %>').val(result.GCItemUnit);
                                    $('#<%=hdnGCItemUnit.ClientID %>').val(result.GCItemUnit);
                                }
                            });
                        }
                        else {
                            var orderDate = $('#<%:txtItemOrderDate.ClientID %>').val();
                            var orderDateInDatePicker = Methods.getDatePickerDate(orderDate);
                            var orderDateFormatString = Methods.dateToString(orderDateInDatePicker);

                            Methods.getItemMasterPurchaseWithDate(result.ItemID, 0, orderDateFormatString, function (result3) {
                                if (result3 != null) {
                                    $('#<%=hdnItemGroupID.ClientID %>').val(result3.ItemGroupID);
                                    $('#<%=txtItemGroupCode.ClientID %>').val(result3.ItemGroupCode);
                                    $('#<%=txtItemGroupName.ClientID %>').val(result3.ItemGroupName1);
                                    $('#<%=hdnSupplierID.ClientID %>').val(result3.BusinessPartnerID);
                                    $('#<%=txtSupplierCode.ClientID %>').val(result3.BusinessPartnerCode);
                                    $('#<%=txtSupplierName.ClientID %>').val(result3.BusinessPartnerName);
                                    $('#<%=txtDiscount.ClientID %>').val(result3.Discount);
                                    $('#<%=txtDiscount2.ClientID %>').val(result3.Discount2);
                                    $('#<%=hdnPrice.ClientID %>').val(result3.Price);
                                    $('#<%=hdnPurchaseUnitPrice.ClientID %>').val(result3.UnitPrice);
                                    $('#<%=hdnGCBaseUnit.ClientID %>').val(result3.ItemUnit);
                                    $('#<%=hdnGCItemUnit.ClientID %>').val(result3.PurchaseUnit);

                                    if ($('#<%=hdnSupplierID.ClientID %>').val() != '') {
                                        Methods.getDateDiffPOPORPerSupplier($('#<%=hdnSupplierID.ClientID %>').val(), function (result) {
                                            if (result != null) {
                                                $('#<%=txtETA.ClientID %>').val(result.Hasil + ' Hari');
                                            }
                                            else {
                                                $('#<%=txtETA.ClientID %>').val('');
                                            }
                                        });
                                    }
                                }
                                else {
                                    $('#<%=txtDiscount.ClientID %>').val('0');
                                    $('#<%=txtDiscount2.ClientID %>').val('0');
                                    $('#<%=txtPrice.ClientID %>').val('0.00');
                                    $('#<%=hdnPrice.ClientID %>').val('0.00');
                                    $('#<%=hdnPurchaseUnitPrice.ClientID %>').val('0.00');
                                    $('#<%=txtETA.ClientID %>').val('');
                                    $('#<%=hdnGCBaseUnit.ClientID %>').val(result.GCItemUnit);
                                    $('#<%=hdnGCItemUnit.ClientID %>').val(result.GCItemUnit);
                                }
                            });
                        }

                        Methods.getItemQtyOnOrder(result.ItemID, $('#<%=hdnLocationIDFrom.ClientID %>').val(), 3, function (result3) {
                            if (result3 != null) {
                                $('#<%=txtQtyOnProcess.ClientID %>').val(result3.QtyOnOrder + " " + result.ItemUnit);
                            }
                            else {
                                $('#<%=txtQtyOnProcess.ClientID %>').val("0 " + result.ItemUnit);
                            }
                            GetItemQtyFromLocation();
                        });

                        Methods.getItemQtyOnOrder(result.ItemID, $('#<%=hdnLocationIDFrom.ClientID %>').val(), 2, function (result4) {
                            if (result4 != null) {
                                $('#<%=txtQtyOnRequest.ClientID %>').val(result4.QtyOnOrder + " " + result.ItemUnit);
                            }
                            else {
                                $('#<%=txtQtyOnRequest.ClientID %>').val("0 " + result.ItemUnit);
                            }
                            GetItemQtyFromLocation();
                        });

                        var filterItemBalance = "ItemID = " + result.ItemID + " AND LocationID = " + $('#<%=hdnLocationIDFrom.ClientID %>').val();
                        Methods.getObject('GetItemBalanceList', filterItemBalance, function (result5) {
                            if (result5 != null) {
                                $('#<%=txtMinMax.ClientID %>').val(result5.QuantityMIN + " - " + result5.QuantityMAX);
                            } else {
                                $('#<%=txtMinMax.ClientID %>').val("0");
                            }
                        });

                        cboItemUnit.PerformCallback('addItem');
                    }
                    else {
                        $('#<%=hdnGCBaseUnit.ClientID %>').val('');
                        $('#<%=hdnGCItemUnit.ClientID %>').val('');
                        $('#<%=hdnItemID.ClientID %>').val('');
                        $('#<%=txtItemCode.ClientID %>').val('');
                        $('#<%=txtItemName.ClientID %>').val('');
                        $('#<%=txtQtyOnProcess.ClientID %>').val('');
                        $('#<%=txtQtyOnRequest.ClientID %>').val('');
                        $('#<%=txtStockLocation.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            $('#lblAddData').click(function (evt) {
                if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                    $('#<%=hdnIsEdit.ClientID %>').val('0');
                    $('#<%=lblLocation.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%=txtLocationCode.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtQuantity.ClientID %>').val('1');
                    $('#<%=hdnEntryID.ClientID %>').val('');
                    $('#<%=hdnItemGroupID.ClientID %>').val('');
                    $('#<%=txtItemGroupCode.ClientID %>').val('');
                    $('#<%=txtItemGroupName.ClientID %>').val('');
                    $('#<%=hdnItemID.ClientID %>').val('');
                    $('#<%=txtItemCode.ClientID %>').val('');
                    $('#<%=txtItemName.ClientID %>').val('');
                    $('#<%=txtSupplierCode.ClientID %>').val('');
                    $('#<%=txtSupplierName.ClientID %>').val('');
                    $('#<%=txtStockLocation.ClientID %>').val('');
                    $('#<%=txtMinMax.ClientID %>').val('');
                    $('#<%=txtQtyOnProcess.ClientID %>').val('');
                    $('#<%=txtQtyOnRequest.ClientID %>').val('');
                    $('#<%=hdnPrice.ClientID %>').val('0');
                    $('#<%=hdnPurchaseUnitPrice.ClientID %>').val('0');
                    $('#<%=txtPrice.ClientID %>').val('0.00');
                    $('#<%=txtBaseUnit.ClientID %>').val('');
                    $('#<%=txtDiscount.ClientID %>').val('0');
                    $('#<%=txtDiscount2.ClientID %>').val('0');
                    $('#<%=txtNotesDt.ClientID %>').val('');
                    cboItemUnit.SetValue('');
                    $('#<%=txtConversion.ClientID %>').val('');
                    $('#<%=hdnConversionFactor.ClientID %>').val(1);

                    $('#containerEntry').show();
                }
            });

            $('#lblQuickPick').click(function () {
                if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
                    showLoadingPanel();
                    var url = ResolveUrl('~/Program/Procurement/PurchaseRequest/PurchaseRequestQuickPicksCtl.ascx');
                    var transactionID = $('#<%=hdnRequestID.ClientID %>').val();
                    var locationID = $('#<%=hdnLocationIDFrom.ClientID %>').val();
                    var locationItemGroupID = $('#<%=hdnLocationItemGroupID.ClientID %>').val();
                    var GCLocationGroup = $('#<%=hdnGCLocationGroup.ClientID %>').val();
                    var ProductLineID = $('#<%=hdnProductLineID.ClientID %>').val();
                    var GCItemType = $('#<%=hdnProductLineItemType.ClientID %>').val();
                    var hdnIM0131 = $('#<%=hdnIM0131.ClientID %>').val();
                    var id = transactionID + '|' + locationID + '|' + locationItemGroupID + '|' + GCLocationGroup + '|' + ProductLineID + '|' + GCItemType + '|' + hdnIM0131;
                    openUserControlPopup(url, id, 'Quick Picks', 1000, 600);
                }
            });

            $('#lblEntryPackage').click(function () {
                if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
                    showLoadingPanel();
                    var url = ResolveUrl('~/Program/Procurement/PurchaseRequest/PurchaseRequestEntryPackageCtl.ascx');
                    var transactionID = $('#<%=hdnRequestID.ClientID %>').val();
                    var locationID = $('#<%=hdnLocationIDFrom.ClientID %>').val();
                    var locationItemGroupID = $('#<%=hdnLocationItemGroupID.ClientID %>').val();
                    var GCLocationGroup = $('#<%=hdnGCLocationGroup.ClientID %>').val();
                    var ProductLineID = $('#<%=hdnProductLineID.ClientID %>').val();
                    var GCItemType = $('#<%=hdnProductLineItemType.ClientID %>').val();
                    var id = transactionID + '|' + locationID + '|' + locationItemGroupID + '|' + GCLocationGroup + '|' + ProductLineID + '|' + GCItemType;
                    openUserControlPopup(url, id, 'Entry Production Package', 1100, 500);
                }
            });

            $('#btnCancel').click(function () {
                $('#<%=lblLocation.ClientID %>').removeClass('lblDisabled');
                $('#<%=txtLocationCode.ClientID %>').removeClass('lblDisabled');
                $('#containerEntry').hide();
            });

            $('#btnSave').click(function (evt) {
                if (IsValid(evt, 'fsTrxPopup', 'mpTrxPopup'))
                    cbpProcess.PerformCallback('save');
            });

            var pageCount = parseInt($('#<%=hdnPageCount.ClientID %>').val());
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
        }

        function GetItemQtyFromLocation(param) {
            if (param != "edit") {
                var filterExpression = "LocationID = " + $('#<%=hdnLocationIDFrom.ClientID %>').val() + " AND ItemID = " + $('#<%=hdnItemID.ClientID %>').val() + " AND IsDeleted = 0";
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

        //#region delete and edit
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
            $('#<%=hdnIsEdit.ClientID %>').val('1');
            $('#<%=hdnEntryID.ClientID %>').val(entity.ID);
            $('#<%=hdnItemGroupID.ClientID %>').val(entity.ItemGroupID);
            $('#<%=hdnGCBaseUnit.ClientID %>').val(entity.GCBaseUnit);
            $('#<%=hdnGCItemUnit.ClientID %>').val(entity.GCItemUnit);
            $('#<%=txtItemGroupCode.ClientID %>').val(entity.ItemGroupCode);
            $('#<%=txtItemGroupName.ClientID %>').val(entity.ItemGroupName1);
            var pricePerPurchaseUnit = parseFloat(entity.UnitPrice);
            var convertion = parseFloat(entity.ConversionFactor);
            var price = pricePerPurchaseUnit / convertion;
            $('#<%=hdnPrice.ClientID %>').val(price);
            $('#<%=hdnPurchaseUnitPrice.ClientID %>').val(pricePerPurchaseUnit);
            $('#<%=txtPrice.ClientID %>').val(pricePerPurchaseUnit).trigger('changeValue');
            $('#<%=txtDiscount.ClientID %>').val(entity.DiscountPercentage);
            $('#<%=txtDiscount2.ClientID %>').val(entity.DiscountPercentage2);
            $('#<%=hdnItemID.ClientID %>').val(entity.ItemID);
            $('#<%=txtItemCode.ClientID %>').val(entity.ItemCode);
            $('#<%=txtItemName.ClientID %>').val(entity.ItemName1);
            $('#<%=txtQuantity.ClientID %>').val(entity.Quantity);
            $('#<%=hdnConversionFactor.ClientID %>').val(convertion);
            var endLocation = entity.QtyENDLocation + ' ' + entity.ItemUnitQtyEndLocation;
            $('#<%=hdnQtyEndLocation.ClientID %>').val(entity.QtyENDLocation);
            $('#<%=txtStockLocation.ClientID %>').val(endLocation);
            var MinMax = entity.QuantityMIN + " - " + entity.QuantityMAX;
            $('#<%=txtMinMax.ClientID %>').val(MinMax);
            $('#<%=hdnSupplierID.ClientID %>').val(entity.BusinessPartnerID);
            $('#<%=txtSupplierCode.ClientID %>').val(entity.BusinessPartnerCode);
            $('#<%=txtSupplierName.ClientID %>').val(entity.BusinessPartnerName);
            $('#<%=txtNotesDt.ClientID %>').val(entity.Remarks);
            cboItemUnit.PerformCallback("edit");
            Methods.getItemQtyOnOrder(entity.ItemID, $('#<%=hdnLocationIDFrom.ClientID %>').val(), 3, function (result3) {
                if (result3 != null)
                    $('#<%=txtQtyOnProcess.ClientID %>').val((result3.QtyOnOrder - entity.CustomTotal) + " " + entity.BaseUnit);
                else
                    $('#<%=txtQtyOnProcess.ClientID %>').val('');
                GetItemQtyFromLocation("edit");
            });

            Methods.getItemQtyOnOrder(entity.ItemID, $('#<%=hdnLocationIDFrom.ClientID %>').val(), 2, function (result4) {
                if (result4 != null)
                    $('#<%=txtQtyOnRequest.ClientID %>').val((result4.QtyOnOrder - entity.CustomTotal) + " " + entity.BaseUnit);
                else
                    $('#<%=txtQtyOnRequest.ClientID %>').val('');
                GetItemQtyFromLocation("edit");
            });
            $('#containerEntry').show();
        });
        //#endregion

        //#region cboItemUnit
        function onCboItemUnitEndCallBack(s) {
            var param = s.cpResult.split('|');
            if (param[0] == 'edit') {
                cboItemUnit.SetValue($('#<%=hdnGCItemUnit.ClientID %>').val());
            }
            else {
                if ($('#<%=hdnGCItemUnit.ClientID %>').val() == '') {
                    if (cboItemUnit.GetValue() == $('#<%=hdnGCBaseUnit.ClientID %>').val()) {
                        cboItemUnit.SetValue($('#<%=hdnGCBaseUnit.ClientID %>').val());
                    }
                }
                else {
                    if (cboItemUnit.GetValue() == $('#<%=hdnGCItemUnit.ClientID %>').val()) {
                        cboItemUnit.SetValue($('#<%=hdnGCItemUnit.ClientID %>').val());
                    }
                }
            }
            onCboItemUnitChanged(param[0]);
        }

        function onCboItemUnitChanged(paramProcess) {
            var purchaseUnit = cboItemUnit.GetValue();
            var itemID = $('#<%=hdnItemID.ClientID %>').val();
            var qtyBefore = $('#<%=txtQuantity.ClientID %>').val().replace('.00', '').split(',').join('');
            var convFactorBefore = $('#<%=hdnConversionFactor.ClientID %>').val();

            var newQty = 0;

            var hdnIM0131 = $('#<%=hdnIM0131.ClientID %>').val();

            var supplierID = 0;
            if ($('#<%=hdnSupplierID.ClientID %>').val() != "" && $('#<%=hdnSupplierID.ClientID %>').val() != 0) {
                supplierID = $('#<%=hdnSupplierID.ClientID %>').val();
            }

            if (paramProcess == "edit") {
                var isFound = 0;

                var filterExpression = "ItemID = " + itemID + " AND GCAlternateUnit = '" + purchaseUnit + "' AND IsActive = 1 AND IsDeleted = 0";
                Methods.getObject('GetvItemAlternateUnitList', filterExpression, function (result) {
                    if (result != null) {
                        var toConversion = getItemUnitName(purchaseUnit);
                        $('#<%=hdnItemUnitValue.ClientID %>').val(result.ConversionFactor);
                        var conversion = "1 " + toConversion + " = " + result.ConversionFactor + " " + result.ItemUnit;
                        $('#<%=txtConversion.ClientID %>').val(conversion);
                        $('#<%=hdnConversionFactor.ClientID %>').val(result.ConversionFactor);
                        factor = result.ConversionFactor;

                        isFound = 1;
                    }
                });

                if (isFound == 0) {
                    var toConversion = getItemUnitName(purchaseUnit);
                    $('#<%=hdnItemUnitValue.ClientID %>').val(1);
                    var conversion = "1 " + toConversion + " = 1 " + " " + toConversion;
                    $('#<%=txtConversion.ClientID %>').val(conversion);
                    $('#<%=hdnConversionFactor.ClientID %>').val(1);
                    factor = 1;
                }

            } else {
                var orderDate = $('#<%:txtItemOrderDate.ClientID %>').val();
                var orderDateInDatePicker = Methods.getDatePickerDate(orderDate);
                var orderDateFormatString = Methods.dateToString(orderDateInDatePicker);

                if (hdnIM0131 == "0") {
                    Methods.getItemMasterPurchaseList($('#<%=hdnItemID.ClientID %>').val(), supplierID, function (result) {
                        if (result.length > 0) {
                            for (i = 0; i < result.length; i++) {
                                if (result[i].ItemUnit == purchaseUnit) {
                                    $('#<%=hdnItemUnitValue.ClientID %>').val("1");
                                    var conversion = "1 " + result[i].ItemUnitText + " = " + "1 " + " " + result[i].ItemUnitText;
                                    $('#<%=txtConversion.ClientID %>').val(conversion);
                                    $('#<%=hdnConversionFactor.ClientID %>').val(1);
                                    factor = 1;

                                    if (paramProcess != "edit") {
                                        $('#<%=txtDiscount.ClientID %>').val(result[i].Discount);
                                        $('#<%=txtDiscount2.ClientID %>').val(result[i].Discount2);
                                        $('#<%=hdnPrice.ClientID %>').val(result[i].Price);
                                        $('#<%=txtPrice.ClientID %>').val(result[i].Price).trigger('changeValue');
                                        $('#<%=hdnPurchaseUnitPrice.ClientID %>').val(result[i].Price);
                                        $('#<%=hdnGCBaseUnit.ClientID %>').val(result[i].ItemUnit);
                                        $('#<%=hdnGCItemUnit.ClientID %>').val(result[i].PurchaseUnit);
                                    }
                                    break;
                                } else if (result[i].PurchaseUnit == purchaseUnit) {
                                    $('#<%=hdnItemUnitValue.ClientID %>').val(result[i].ConversionFactor);
                                    var conversion = "1 " + result[i].PurchaseUnitText + " = " + result[i].ConversionFactor + " " + result[i].ItemUnitText;
                                    $('#<%=txtConversion.ClientID %>').val(conversion);
                                    $('#<%=hdnConversionFactor.ClientID %>').val(result[i].ConversionFactor);
                                    factor = result[i].ConversionFactor;

                                    if (paramProcess != "edit") {
                                        $('#<%=txtDiscount.ClientID %>').val(result[i].Discount);
                                        $('#<%=txtDiscount2.ClientID %>').val(result[i].Discount2);
                                        $('#<%=hdnPrice.ClientID %>').val(result[i].Price);
                                        $('#<%=txtPrice.ClientID %>').val(result[i].UnitPrice).trigger('changeValue');
                                        $('#<%=hdnPurchaseUnitPrice.ClientID %>').val(result[i].UnitPrice);
                                        $('#<%=hdnGCBaseUnit.ClientID %>').val(result[i].ItemUnit);
                                        $('#<%=hdnGCItemUnit.ClientID %>').val(result[i].PurchaseUnit);
                                    }
                                    break;
                                }
                                else {
                                    Methods.getListObject('GetItemAlternateUnitList', "ItemID = " + $('#<%=hdnItemID.ClientID %>').val() + " AND GCAlternateUnit = '" + purchaseUnit + "' AND IsActive = 1 AND IsDeleted = 0", function (resultAlt) {
                                        for (j = 0; j < resultAlt.length; j++) {
                                            var price = parseFloat(result[i].Price);
                                            var unitPrice = parseFloat(result[i].Price * resultAlt[j].ConversionFactor).toFixed(2);

                                            var tempPricePerPurchaseUnit = 0;
                                            var baseValue = $('#<%=hdnGCBaseUnit.ClientID %>').val();
                                            var toUnitItem = cboItemUnit.GetValue();
                                            var baseText = getItemUnitName(baseValue);

                                            var factor = 1;
                                            $('#<%=txtBaseUnit.ClientID %>').val("Per " + cboItemUnit.GetText());
                                            if (baseValue == toUnitItem) {
                                                $('#<%=hdnItemUnitValue.ClientID %>').val('1');
                                                var conversion = "1 " + baseText + " = 1 " + baseText;
                                                $('#<%=txtConversion.ClientID %>').val(conversion);
                                                $('#<%=hdnConversionFactor.ClientID %>').val(1);
                                                factor = 1;
                                            }
                                            else {
                                                var itemID = $('#<%=hdnItemID.ClientID %>').val();
                                                var filterExpression = "ItemID = " + itemID + " AND GCAlternateUnit = '" + toUnitItem + "' AND IsActive = 1 AND IsDeleted = 0";
                                                Methods.getObject('GetvItemAlternateUnitList', filterExpression, function (resultAlt1) {
                                                    if (resultAlt1 != null) {
                                                        var toConversion = getItemUnitName(toUnitItem);
                                                        $('#<%=hdnItemUnitValue.ClientID %>').val(resultAlt1.ConversionFactor);
                                                        var conversion = "1 " + toConversion + " = " + resultAlt1.ConversionFactor + " " + resultAlt1.ItemUnit;
                                                        $('#<%=txtConversion.ClientID %>').val(conversion);
                                                        $('#<%=hdnConversionFactor.ClientID %>').val(resultAlt1.ConversionFactor);
                                                        factor = resultAlt1.ConversionFactor;
                                                    }
                                                });
                                            }

                                            $('#<%=txtDiscount.ClientID %>').val(result[i].Discount);
                                            $('#<%=txtDiscount2.ClientID %>').val(result[i].Discount2);
                                            $('#<%=hdnPrice.ClientID %>').val(price);
                                            $('#<%=txtPrice.ClientID %>').val(unitPrice).trigger('changeValue');
                                            $('#<%=hdnPurchaseUnitPrice.ClientID %>').val(unitPrice);
                                            $('#<%=hdnGCBaseUnit.ClientID %>').val(result[i].ItemUnit);
                                            $('#<%=hdnGCItemUnit.ClientID %>').val(result[i].PurchaseUnit);

                                        }
                                    });
                                    break;
                                }
                            }
                        } else {
                            var tempPricePerPurchaseUnit = 0;
                            var baseValue = $('#<%=hdnGCBaseUnit.ClientID %>').val();
                            var toUnitItem = cboItemUnit.GetValue();
                            var baseText = getItemUnitName(baseValue);
                            var factor = 1;
                            $('#<%=txtBaseUnit.ClientID %>').val("Per " + cboItemUnit.GetText());
                            if (baseValue == toUnitItem) {
                                $('#<%=hdnItemUnitValue.ClientID %>').val('1');
                                var conversion = "1 " + baseText + " = 1 " + baseText;
                                $('#<%=txtConversion.ClientID %>').val(conversion);
                                $('#<%=hdnConversionFactor.ClientID %>').val(1);
                                factor = 1;
                            }
                            else {
                                var itemID = $('#<%=hdnItemID.ClientID %>').val();
                                var filterExpression = "ItemID = " + itemID + " AND GCAlternateUnit = '" + toUnitItem + "'";
                                Methods.getObjectValue('GetvItemAlternateUnitList', filterExpression, 'ConversionFactor', function (result) {
                                    var toConversion = getItemUnitName(toUnitItem);
                                    $('#<%=hdnItemUnitValue.ClientID %>').val(result);
                                    var conversion = "1 " + toConversion + " = " + result + " " + baseText;
                                    $('#<%=txtConversion.ClientID %>').val(conversion);
                                    $('#<%=hdnConversionFactor.ClientID %>').val(result);
                                    factor = result;
                                });
                            }
                            var convertion = parseFloat($('#<%=hdnConversionFactor.ClientID %>').val());
                            var priceperitemunit = parseFloat($('#<%=hdnPrice.ClientID %>').val().replace('.00', '').split(',').join(''));
                            var pricepurchaseunit = parseFloat($('#<%=hdnPurchaseUnitPrice.ClientID %>').val().replace('.00', '').split(',').join(''));
                            var pricePerPurchaseUnit = 0;

                            if (convertion == "1") {
                                pricePerPurchaseUnit = priceperitemunit;
                            }
                            else if (purchaseUnit == toUnitItem) {
                                if (tempPricePerPurchaseUnit != 0) {
                                    pricePerPurchaseUnit = tempPricePerPurchaseUnit;
                                } else {
                                    if (pricepurchaseunit == priceperitemunit) {
                                        pricePerPurchaseUnit = (pricepurchaseunit * convertion).toFixed(2);
                                    } else {
                                        pricePerPurchaseUnit = pricepurchaseunit;
                                    }
                                }
                            }
                            else {
                                if (pricepurchaseunit == priceperitemunit) {
                                    pricePerPurchaseUnit = (pricepurchaseunit * convertion).toFixed(2);
                                } else {
                                    pricePerPurchaseUnit = (priceperitemunit * convertion).toFixed(2);
                                }
                            }

                            $('#<%=hdnPurchaseUnitPrice.ClientID %>').val(pricePerPurchaseUnit);

                            $('#<%=txtPrice.ClientID %>').val($('#<%=hdnPurchaseUnitPrice.ClientID %>').val()).trigger('changeValue');
                        }
                    });
                }
                else {
                    Methods.getItemMasterPurchaseWithDateList($('#<%=hdnItemID.ClientID %>').val(), supplierID, orderDateFormatString, function (result) {
                        if (result.length > 0) {
                            for (i = 0; i < result.length; i++) {
                                if (result[i].ItemUnit == purchaseUnit) {
                                    $('#<%=hdnItemUnitValue.ClientID %>').val("1");
                                    var conversion = "1 " + result[i].ItemUnitText + " = " + "1 " + " " + result[i].ItemUnitText;
                                    $('#<%=txtConversion.ClientID %>').val(conversion);
                                    $('#<%=hdnConversionFactor.ClientID %>').val(1);
                                    factor = 1;

                                    if (paramProcess != "edit") {
                                        $('#<%=txtDiscount.ClientID %>').val(result[i].Discount);
                                        $('#<%=txtDiscount2.ClientID %>').val(result[i].Discount2);
                                        $('#<%=hdnPrice.ClientID %>').val(result[i].Price);
                                        $('#<%=txtPrice.ClientID %>').val(result[i].Price).trigger('changeValue');
                                        $('#<%=hdnPurchaseUnitPrice.ClientID %>').val(result[i].Price);
                                        $('#<%=hdnGCBaseUnit.ClientID %>').val(result[i].ItemUnit);
                                        $('#<%=hdnGCItemUnit.ClientID %>').val(result[i].PurchaseUnit);
                                    }
                                    break;
                                } else if (result[i].PurchaseUnit == purchaseUnit) {
                                    $('#<%=hdnItemUnitValue.ClientID %>').val(result[i].ConversionFactor);
                                    var conversion = "1 " + result[i].PurchaseUnitText + " = " + result[i].ConversionFactor + " " + result[i].ItemUnitText;
                                    $('#<%=txtConversion.ClientID %>').val(conversion);
                                    $('#<%=hdnConversionFactor.ClientID %>').val(result[i].ConversionFactor);
                                    factor = result[i].ConversionFactor;

                                    if (paramProcess != "edit") {
                                        $('#<%=txtDiscount.ClientID %>').val(result[i].Discount);
                                        $('#<%=txtDiscount2.ClientID %>').val(result[i].Discount2);
                                        $('#<%=hdnPrice.ClientID %>').val(result[i].Price);
                                        $('#<%=txtPrice.ClientID %>').val(result[i].UnitPrice).trigger('changeValue');
                                        $('#<%=hdnPurchaseUnitPrice.ClientID %>').val(result[i].UnitPrice);
                                        $('#<%=hdnGCBaseUnit.ClientID %>').val(result[i].ItemUnit);
                                        $('#<%=hdnGCItemUnit.ClientID %>').val(result[i].PurchaseUnit);
                                    }
                                    break;
                                }
                                else {
                                    Methods.getListObject('GetItemAlternateUnitList', "ItemID = " + $('#<%=hdnItemID.ClientID %>').val() + " AND GCAlternateUnit = '" + purchaseUnit + "' AND IsActive = 1 AND IsDeleted = 0", function (resultAlt) {
                                        for (j = 0; j < resultAlt.length; j++) {
                                            var price = parseFloat(result[i].Price * resultAlt[j].ConversionFactor);
                                            var unitPrice = parseFloat((result[i].UnitPrice / result[i].ConversionFactor) * resultAlt[j].ConversionFactor);

                                            var tempPricePerPurchaseUnit = 0;
                                            var baseValue = $('#<%=hdnGCBaseUnit.ClientID %>').val();
                                            var toUnitItem = cboItemUnit.GetValue();
                                            var baseText = getItemUnitName(baseValue);

                                            var factor = 1;
                                            $('#<%=txtBaseUnit.ClientID %>').val("Per " + cboItemUnit.GetText());
                                            if (baseValue == toUnitItem) {
                                                $('#<%=hdnItemUnitValue.ClientID %>').val('1');
                                                var conversion = "1 " + baseText + " = 1 " + baseText;
                                                $('#<%=txtConversion.ClientID %>').val(conversion);
                                                $('#<%=hdnConversionFactor.ClientID %>').val(1);
                                                factor = 1;
                                            }
                                            else {
                                                var itemID = $('#<%=hdnItemID.ClientID %>').val();
                                                var filterExpression = "ItemID = " + itemID + " AND GCAlternateUnit = '" + toUnitItem + "' AND IsActive = 1 AND IsDeleted = 0";
                                                Methods.getObject('GetvItemAlternateUnitList', filterExpression, function (resultAlt1) {
                                                    if (resultAlt1 != null) {
                                                        var toConversion = getItemUnitName(toUnitItem);
                                                        $('#<%=hdnItemUnitValue.ClientID %>').val(resultAlt1.ConversionFactor);
                                                        var conversion = "1 " + toConversion + " = " + resultAlt1.ConversionFactor + " " + resultAlt1.ItemUnit;
                                                        $('#<%=txtConversion.ClientID %>').val(conversion);
                                                        $('#<%=hdnConversionFactor.ClientID %>').val(resultAlt1.ConversionFactor);
                                                        factor = resultAlt1.ConversionFactor;
                                                    }
                                                });
                                            }
                                            $('#<%=txtDiscount.ClientID %>').val(result[i].Discount);
                                            $('#<%=txtDiscount2.ClientID %>').val(result[i].Discount2);
                                            $('#<%=hdnPrice.ClientID %>').val(price);
                                            $('#<%=txtPrice.ClientID %>').val(unitPrice).trigger('changeValue');
                                            $('#<%=hdnPurchaseUnitPrice.ClientID %>').val(unitPrice);
                                            $('#<%=hdnGCBaseUnit.ClientID %>').val(result[i].ItemUnit);
                                            $('#<%=hdnGCItemUnit.ClientID %>').val(result[i].PurchaseUnit);
                                        }
                                    });
                                }
                            }
                        } else {
                            var tempPricePerPurchaseUnit = 0;
                            var baseValue = $('#<%=hdnGCBaseUnit.ClientID %>').val();
                            var toUnitItem = cboItemUnit.GetValue();
                            var baseText = getItemUnitName(baseValue);
                            var factor = 1;
                            $('#<%=txtBaseUnit.ClientID %>').val("Per " + cboItemUnit.GetText());
                            if (baseValue == toUnitItem) {
                                $('#<%=hdnItemUnitValue.ClientID %>').val('1');
                                var conversion = "1 " + baseText + " = 1 " + baseText;
                                $('#<%=txtConversion.ClientID %>').val(conversion);
                                $('#<%=hdnConversionFactor.ClientID %>').val(1);
                                factor = 1;
                            }
                            else {
                                var itemID = $('#<%=hdnItemID.ClientID %>').val();
                                var filterExpression = "ItemID = " + itemID + " AND GCAlternateUnit = '" + toUnitItem + "'";
                                Methods.getObjectValue('GetvItemAlternateUnitList', filterExpression, 'ConversionFactor', function (result) {
                                    var toConversion = getItemUnitName(toUnitItem);
                                    $('#<%=hdnItemUnitValue.ClientID %>').val(result);
                                    var conversion = "1 " + toConversion + " = " + result + " " + baseText;
                                    $('#<%=txtConversion.ClientID %>').val(conversion);
                                    $('#<%=hdnConversionFactor.ClientID %>').val(result);
                                    factor = result;
                                });
                            }
                            var convertion = parseFloat($('#<%=hdnItemUnitValue.ClientID %>').val());
                            var priceperitemunit = parseFloat($('#<%=hdnPrice.ClientID %>').val().replace('.00', '').split(',').join(''));
                            var pricepurchaseunit = parseFloat($('#<%=hdnPurchaseUnitPrice.ClientID %>').val().replace('.00', '').split(',').join(''));
                            var pricePerPurchaseUnit = 0;

                            if (convertion == "1") {
                                pricePerPurchaseUnit = priceperitemunit;
                            }
                            else if (purchaseUnit == toUnitItem) {
                                if (tempPricePerPurchaseUnit != 0) {
                                    pricePerPurchaseUnit = tempPricePerPurchaseUnit;
                                } else {
                                    if (pricepurchaseunit == priceperitemunit) {
                                        pricePerPurchaseUnit = pricepurchaseunit * convertion;
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

                            $('#<%=txtPrice.ClientID %>').val($('#<%=hdnPurchaseUnitPrice.ClientID %>').val()).trigger('changeValue');
                        }
                    });
                }

            }


            factor = $('#<%=hdnConversionFactor.ClientID %>').val();

            if (paramProcess == 'addItem') {
                newQty = qtyBefore;
            } else {
                newQty = qtyBefore / factor * convFactorBefore;
            }

            $('#<%=txtQuantity.ClientID %>').val(newQty).trigger('changeValue');

            $('#<%=txtBaseUnit.ClientID %>').val("per " + cboItemUnit.GetText());
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
            if ($('#<%=hdnRequestID.ClientID %>').val() == '0') {
                $('#<%=hdnRequestID.ClientID %>').val(OrderID);
                var filterExpression = 'PurchaseRequestID = ' + OrderID;
                Methods.getObject('GetPurchaseRequestHdList', filterExpression, function (result) {
                    $('#<%=txtOrderNo.ClientID %>').val(result.PurchaseRequestNo);
                    cbpView.PerformCallback('refresh');
                });
                onAfterCustomSaveSuccess();
                setRightPanelButtonEnabled();
            }
            else
                cbpView.PerformCallback('refresh');
        }

        function onAfterSaveAddRecordEntryPopup(param) {
            onAfterSaveRecordDtSuccess(param);
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

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var purchaseRequestID = $('#<%=hdnRequestID.ClientID %>').val();
            var printStatus = $('#<%=hdnPrintStatus.ClientID %>').val();
            var transaction = $('#<%=hdnGCTransactionStatus.ClientID %>').val();

            if (purchaseRequestID == '' || purchaseRequestID == '0') {
                errMessage.text = 'Please Save Transaction First!';
                return false;
            }
            else {
                if (code == 'IM-00052' || code == 'IM-00135' || code == 'IM-00150' || code == 'IM-00160' || code == 'IM-00169' || code == 'IM-00180' || code == 'IM-00198'
                    || code == 'IM-00222' || code == 'IM-00248') {
                    if (transaction == Constant.TransactionStatus.APPROVED || transaction == Constant.TransactionStatus.CLOSED) {
                        filterExpression.text = "PurchaseRequestID = " + purchaseRequestID;
                        return true;
                    }
                    else {
                        errMessage.text = "Data Doesn't Approved or Closed";
                        return false;
                    }
                }
                else if (code == 'IM-00028' || code == 'IM-00084') {
                    if (transaction == Constant.TransactionStatus.APPROVED || transaction == Constant.TransactionStatus.CLOSED) {
                        filterExpression.text = purchaseRequestID;
                        return true;
                    }
                    else {
                        errMessage.text = "Data Doesn't Approved or Closed";
                        return false;
                    }
                }
                else {
                    filterExpression.text = "PurchaseRequestID = " + purchaseRequestID;
                    return true;
                }
            }

        }

        function onBeforeLoadRightPanelContent(code) {
            if (code == 'infoPurchaseRequestDetail' || code == 'infoPurchaseRequest') {
                var param = $('#<%:hdnRequestID.ClientID %>').val();
                return param;
            }
            else if (code == "sendInventoryNotification") {
                var param = "00" + "|" + $('#<%:hdnRequestID.ClientID %>').val();
                return param;
            }
            else {
                return $('#<%:hdnRequestID.ClientID %>').val();
            }
        }

        function setRightPanelButtonEnabled() {
            if ($('#<%:hdnRequestID.ClientID %>').val() == '' || $('#<%:hdnRequestID.ClientID %>').val() == '0') {
                $('#btnInfoHeader').attr('enabled', 'false');
                $('#btnInfoDetail').attr('enabled', 'false');
                $('#btnInfoDecline').attr('enabled', 'false');
                $('#btnInfoClosed').attr('enabled', 'false');
                $('#btnpurchaseRequestRegistration').attr('enabled', 'false');
                $('#btnSendInventoryNotification').attr('enabled', 'false');
            }
            else {
                $('#btnInfoHeader').removeAttr('enabled');
                $('#btnInfoDetail').removeAttr('enabled');
                $('#btnInfoDecline').removeAttr('enabled');
                $('#btnInfoClosed').removeAttr('enabled');
                $('#btnpurchaseRequestRegistration').removeAttr('enabled');
                $('#btnSendInventoryNotification').removeAttr('enabled');
            }
        }
    </script>
    <style>
        .classDisplay
        {
        }
        .classDisplayNone
        {
            display: none;
        }
    </style>
    <input type="hidden" value="false" id="hdnPrintStatus" runat="server" />
    <input type="hidden" value="" id="hdnRequestID" runat="server" />
    <input type="hidden" value="" id="hdnPageCount" runat="server" />
    <input type="hidden" value="1" id="hdnIsEditable" runat="server" />
    <input type="hidden" value="" id="hdnRecordFilterExpression" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="0" id="hdnIsControlPrint" runat="server" />
    <input type="hidden" value="0" id="hdnIsEdit" runat="server" />
    <input type="hidden" value="0" id="hdnIsUsedProductLine" runat="server" />
    <input type="hidden" value="0" id="hdnIsUsedPurchaseOrderType" runat="server" />
    <input type="hidden" value="0" id="hdnGCTransactionStatus" runat="server" />
    <input type="hidden" value="" id="hdnIsShowTotalPrice" runat="server" />
    <input type="hidden" value="" id="hdnQtyEndLocation" runat="server" />
    <input type="hidden" value="" id="hdnGCItemUnitQtyEndLocation" runat="server" />
    <input type="hidden" value="" id="hdnIM0131" runat="server" />
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
                                <label class="lblLink" id="lblOrderNo">
                                    <%=GetLabel("No. Permintaan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtOrderNo" Width="150px" ReadOnly="true" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory lblLink" runat="server" id="lblLocation">
                                    <%=GetLabel("Dari Lokasi")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnLocationIDFrom" value="" runat="server" />
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
                        <tr id="trPurchaseOrderType" runat="server" style="display: none">
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Jenis Permintaan")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboPurchaseOrderType" ClientInstanceName="cboPurchaseOrderType"
                                    Width="45%" runat="server">
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkIsUrgent" Text="CITO" Font-Bold="true" Checked="false" Width="100%"
                                    runat="server" />
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
                                <%=GetLabel("Tanggal") %>
                                -
                                <%=GetLabel("Waktu") %>
                            </td>
                            <td>
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="padding-right: 1px; width: 145px">
                                            <asp:TextBox ID="txtItemOrderDate" Width="120px" CssClass="datepicker" runat="server" />
                                        </td>
                                        <td style="width: 5px">
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtItemOrderTime" Width="100px" CssClass="time" runat="server" Style="text-align: center" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="vertical-align: top">
                                <%=GetLabel("Keterangan") %>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNotes" Width="100%" runat="server" TextMode="MultiLine" Rows="2" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="vertical-align: top">
                                <%=GetLabel("Informasi Permintaan Barang") %>
                            </td>
                            <td>
                                <asp:TextBox ID="txtItemRequestNo" ReadOnly='true' Width="100%" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div id="containerEntry" style="margin-top: 4px; display: none;">
                        <div class="pageTitle">
                            <%=GetLabel("Edit atau Tambah Item Permintaan Pembelian")%></div>
                        <fieldset id="fsTrxPopup" style="margin: 0">
                            <input type="hidden" value="" id="hdnEntryID" runat="server" />
                            <table style="width: 100%" class="tblEntryDetail">
                                <colgroup>
                                    <col style="width: 50%" />
                                    <col style="width: 30px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td valign="top">
                                        <table>
                                            <colgroup>
                                                <col style="width: 120px" />
                                                <col style="width: 120px" />
                                                <col style="width: 120px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td class="tdLabel">
                                                    <input type="hidden" value="" id="hdnItemGroupID" runat="server" />
                                                    <label class="lblLink" id="lblItemGroup">
                                                        <%=GetLabel("Kelompok Item")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtItemGroupCode" Width="100%" runat="server" />
                                                </td>
                                                <td colspan="2" style="padding-left: 3px">
                                                    <asp:TextBox ID="txtItemGroupName" ReadOnly="true" Width="100%" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblLink lblMandatory" id="lblItem">
                                                        <%=GetLabel("Item")%></label>
                                                </td>
                                                <td>
                                                    <input type="hidden" value="" id="hdnItemID" runat="server" />
                                                    <input type="hidden" value="" id="hdnGCBaseUnit" runat="server" />
                                                    <input type="hidden" value="" id="hdnGCItemUnit" runat="server" />
                                                    <input type="hidden" value="" id="hdnConversionFactor" runat="server" />
                                                    <asp:TextBox ID="txtItemCode" Width="100%" runat="server" />
                                                </td>
                                                <td colspan="2" style="padding-left: 3px">
                                                    <asp:TextBox ID="txtItemName" ReadOnly="true" Width="100%" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label>
                                                        <%=GetLabel("Stok")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtStockLocation" ReadOnly="true" Width="100%" runat="server" Style="text-align: right" />
                                                </td>
                                                <td class="tdLabel" style="padding-left: 10px">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("MIN - MAX")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtMinMax" ReadOnly="true" Width="100%" runat="server" Style="text-align: center" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Qty On Order")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtQtyOnProcess" ReadOnly="true" Width="100%" runat="server" Style="text-align: right" />
                                                </td>
                                                <td class="tdLabel" style="padding-left: 10px">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Qty On Request")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtQtyOnRequest" ReadOnly="true" Width="100%" runat="server" Style="text-align: right" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("Jumlah")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtQuantity" value="1" CssClass="number" Min="0" Width="100%" runat="server" />
                                                </td>
                                                <td class="tdLabel" style="padding-left: 10px">
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("Satuan")%></label>
                                                </td>
                                                <td>
                                                    <dxe:ASPxComboBox runat="server" ID="cboItemUnit" ClientInstanceName="cboItemUnit"
                                                        Width="100%" OnCallback="cboItemUnit_Callback">
                                                        <ClientSideEvents EndCallback="function(s,e){ onCboItemUnitEndCallBack(s); }" ValueChanged="function(s,e){ onCboItemUnitChanged(); }" />
                                                    </dxe:ASPxComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <input type="hidden" value="" id="hdnItemUnitValue" runat="server" />
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("Konversi")%></label>
                                                </td>
                                                <td colspan="3">
                                                    <asp:TextBox ID="txtConversion" Width="100%" runat="server" ReadOnly="true" />
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
                                                <td class="tdLabel" style="vertical-align: top">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Catatan")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtNotesDt" Width="100%" runat="server" TextMode="MultiLine" Rows="2" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblLink" id="lblSupplier" runat="server">
                                                        <%=GetLabel("Supplier/Penyedia")%></label>
                                                </td>
                                                <td>
                                                    <input type="hidden" value="" id="hdnSupplierID" runat="server" />
                                                    <table cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width: 120px" />
                                                            <col style="width: 3px" />
                                                            <col style="width: 250px" />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtSupplierCode" Width="100%" runat="server" />
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
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblNormal" id="lblETA" runat="server">
                                                        <%=GetLabel("Rata Rata Waktu Pengiriman")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtETA" ReadOnly="true" Width="25%" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
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
                                                                <input type="hidden" value="0" id="hdnPrice" runat="server" />
                                                                <input type="hidden" value="0" id="hdnPurchaseUnitPrice" runat="server" />
                                                                <asp:TextBox ID="txtPrice" Width="100%" value="0.00" runat="server" CssClass="txtCurrency" />
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
                                            <tr style="display: none">
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Diskon")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDiscount" Width="100px" runat="server" value="0" CssClass="number" />
                                                    %
                                                </td>
                                            </tr>
                                            <tr style="display: none">
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Diskon 2")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDiscount2" Width="100px" runat="server" value="0" CssClass="number" />
                                                    %
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
                                                    <input type="hidden" value="<%#:Eval("ItemID") %>" bindingfield="ItemID" />
                                                    <input type="hidden" value="<%#:Eval("ItemCode") %>" bindingfield="ItemCode" />
                                                    <input type="hidden" value="<%#:Eval("ItemName1") %>" bindingfield="ItemName1" />
                                                    <input type="hidden" value="<%#:Eval("ItemGroupID") %>" bindingfield="ItemGroupID" />
                                                    <input type="hidden" value="<%#:Eval("ItemGroupCode") %>" bindingfield="ItemGroupCode" />
                                                    <input type="hidden" value="<%#:Eval("ItemGroupName1") %>" bindingfield="ItemGroupName1" />
                                                    <input type="hidden" value="<%#:Eval("BusinessPartnerID") %>" bindingfield="BusinessPartnerID" />
                                                    <input type="hidden" value="<%#:Eval("BusinessPartnerCode") %>" bindingfield="BusinessPartnerCode" />
                                                    <input type="hidden" value="<%#:Eval("BusinessPartnerName") %>" bindingfield="BusinessPartnerName" />
                                                    <input type="hidden" value="<%#:Eval("QuantityMIN") %>" bindingfield="QuantityMIN" />
                                                    <input type="hidden" value="<%#:Eval("QuantityMAX") %>" bindingfield="QuantityMAX" />
                                                    <input type="hidden" value="<%#:Eval("Quantity") %>" bindingfield="Quantity" />
                                                    <input type="hidden" value="<%#:Eval("QtyENDLocation") %>" bindingfield="QtyENDLocation" />
                                                    <input type="hidden" value="<%#:Eval("ItemUnitQtyEndLocation") %>" bindingfield="ItemUnitQtyEndLocation" />
                                                    <input type="hidden" value="<%#:Eval("GCPurchaseUnit") %>" bindingfield="GCItemUnit" />
                                                    <input type="hidden" value="<%#:Eval("GCBaseUnit") %>" bindingfield="GCBaseUnit" />
                                                    <input type="hidden" value="<%#:Eval("BaseUnit") %>" bindingfield="BaseUnit" />
                                                    <input type="hidden" value="<%#:Eval("UnitPrice") %>" bindingfield="UnitPrice" />
                                                    <input type="hidden" value="<%#:Eval("DiscountPercentage") %>" bindingfield="DiscountPercentage" />
                                                    <input type="hidden" value="<%#:Eval("DiscountPercentage2") %>" bindingfield="DiscountPercentage2" />
                                                    <input type="hidden" value="<%#:Eval("ConversionFactor") %>" bindingfield="ConversionFactor" />
                                                    <input type="hidden" value="<%#:Eval("GCItemDetailStatus") %>" bindingfield="GCItemDetailStatus" />
                                                    <input type="hidden" value="<%#:Eval("Remarks") %>" bindingfield="Remarks" />
                                                    <input type="hidden" value="<%#:Eval("CustomTotal") %>" bindingfield="CustomTotal" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Kode Item" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                                                HeaderStyle-Width="100px">
                                                <ItemTemplate>
                                                    <%#:Eval("ItemCode") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Nama Item" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <%#:Eval("ItemName1") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="MIN" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="50px">
                                                <ItemTemplate>
                                                    <%#:Eval("QuantityMIN") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="MAX" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="50px">
                                                <ItemTemplate>
                                                    <%#:Eval("QuantityMAX") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Supplier" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                                                HeaderStyle-Width="200px" Visible="false">
                                                <ItemTemplate>
                                                    <%#:Eval("BusinessPartnerName") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Diminta" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="120px">
                                                <ItemTemplate>
                                                    <%#:Eval("CustomPurchaseUnit") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Faktor Konversi" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="200px">
                                                <ItemTemplate>
                                                    <%#:Eval("CustomConversion") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Total Diminta" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="120px">
                                                <ItemTemplate>
                                                    <%#:Eval("CustomPurchaseRequest") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Harga Satuan" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="120px">
                                                <ItemTemplate>
                                                    <%#:Eval("cfUnitPriceInString") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Disc 1(%)" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="80px">
                                                <ItemTemplate>
                                                    <%#:Eval("DiscountPercentage") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Disc 2(%)" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="80px">
                                                <ItemTemplate>
                                                    <%#:Eval("DiscountPercentage2") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Sub Total" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="120px">
                                                <ItemTemplate>
                                                    <%#:Eval("cfTotalPriceInString") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Status Dt" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="100px">
                                                <ItemTemplate>
                                                    <%#:Eval("ItemDetailStatus") %>
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
                        <span class="lblLink" id="lblAddData" style="margin-right: 200px;">
                            <%= GetLabel("Add Data")%></span> <span class="lblLink" id="lblQuickPick" style="margin-right: 200px;">
                                <%= GetLabel("Quick Picks")%></span> <span class="lblLink" id="lblEntryPackage">
                                    <%= GetLabel("Add Production Package")%></span>
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
                                                <tr>
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
                                                <tr>
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
