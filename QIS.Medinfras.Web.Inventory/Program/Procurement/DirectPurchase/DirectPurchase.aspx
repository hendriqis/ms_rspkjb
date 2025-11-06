<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/libs/MasterPage/MPTrx2.master"
    CodeBehind="DirectPurchase.aspx.cs" Inherits="QIS.Medinfras.Web.Inventory.DirectPurchase" %>

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
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnDirectPurchaseReopen" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><div>
            <%=GetLabel("Re-Open")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        var lastTransactionAmount = 0;
        var editedLineAmount = 0;

        function setCustomToolbarVisibility() {
            var GCTransactionStatus = $('#<%=hdnGCTransactionStatus.ClientID %>').val();
            if (GCTransactionStatus == 'X121^003') {
                $('#<%=btnDirectPurchaseReopen.ClientID %>').show();
            }
            else {
                $('#<%=btnDirectPurchaseReopen.ClientID %>').hide();
            }
        }

        $('#<%=btnDirectPurchaseReopen.ClientID %>').die('click');
        $('#<%=btnDirectPurchaseReopen.ClientID %>').live('click', function (evt) {
            if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                showToastConfirmation("Apakah yakin akan RE-OPEN pembelian tunai ini ?", function (result) {
                    if (result) {
                        onCustomButtonClick('reopen');
                    }
                });
            }
        });

        function onAfterCustomClickSuccess(type, retval) {
            onLoadObject(retval);
        }

        function onLoad() {
            $('#<%=btnDirectPurchaseReopen.ClientID %>').hide();

            if ($('#<%=hdnIsEditable.ClientID %>').val() == '1') {
                $('#lblAddData').show();
                $('#lblQuickPick').show();
            }
            else {
                $('#lblAddData').hide();
                $('#lblQuickPick').hide();
            }

            var displayOption = $('#<%=rblDiscountType.ClientID %>').find(":checked").val();
            if (displayOption == 0) { // persen
                $('#<%:trFinalDiskonPersentase.ClientID %>').removeAttr('style');
                $('#<%:trFinalDiskon.ClientID %>').attr('style', 'display:none');
            }
            else { // amount
                $('#<%:trFinalDiskon.ClientID %>').removeAttr('style');
                $('#<%:trFinalDiskonPersentase.ClientID %>').attr('style', 'display:none');
            }

            if ($('#<%=hdnPurchaseID.ClientID %>').val() == "" || $('#<%=hdnPurchaseID.ClientID %>').val() == "0") {
                setDatePicker('<%=txtDirectPurchaseDate.ClientID %>');
            }
            $('#<%=txtDirectPurchaseDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

            setDatePicker('<%=txtReferenceDate.ClientID %>');

            //#region Direct Purchase No
            $('#lblDirectPurchaseNo.lblLink').click(function () {
                openSearchDialog('directpurchase', "<%=GetFilterExpression() %>", function (value) {
                    $('#<%=txtDirectPurchaseNo.ClientID %>').val(value);
                    ontxtDirectPurchaseNoChanged(value);
                });
            });

            $('#<%=txtDirectPurchaseNo.ClientID %>').change(function () {
                ontxtDirectPurchaseNoChanged($(this).val());
            });

            function ontxtDirectPurchaseNoChanged(value) {
                onLoadObject(value);
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
                $('#<%=txtSubTotalPrice.ClientID %>').val('').trigger('changeValue');
                $('#<%=txtSupplierCode.ClientID %>').attr('readonly', 'readonly');
                $('#<%=lblProductLine.ClientID %>').attr('class', 'lblDisabled');
                $('#<%=txtProductLineCode.ClientID %>').attr('readonly', 'readonly');
                $('#<%=lblLocation.ClientID %>').attr('class', 'lblDisabled');
                $('#<%=txtLocationCode.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtStockLocation.ClientID %>').val('');
                lastTransactionAmount = $('#<%=txtTotalPurchase.ClientID %>').attr('hiddenVal');
                editedLineAmount = 0;
                cboItemUnit.SetValue('');
                $('#<%=txtConversion.ClientID %>').val('');
                $('#<%=hdnTempFactor.ClientID %>').val('0');
                $('#<%=hdnConversionFactor.ClientID %>').val('1');
            }
            //#endregion

            //#region Item
            function getItemFilterExpression() {
                var ProductLineID = $('#<%=hdnProductLineID.ClientID %>').val();
                var GCItemType = $('#<%=hdnProductLineItemType.ClientID %>').val();
                var GCLocationGroup = $('#<%=hdnGCLocationGroup.ClientID %>').val();
                var filterExpression = "IsDeleted = 0 AND GCItemStatus != 'X181^999'";
                var purchaseID = $('#<%=hdnPurchaseID.ClientID %>').val();

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

                if (purchaseID != '') {
                    filterExpression += " AND ItemID NOT IN (SELECT ItemID FROM DirectPurchaseDt WHERE DirectPurchaseID = " + purchaseID + " AND GCItemDetailStatus != 'X121^999')";
                }

                filterExpression += " AND ItemID IN (SELECT ItemID FROM ItemBalance WHERE IsDeleted = 0 AND LocationID = " + $('#<%=hdnLocationID.ClientID %>').val() + ")";

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
                Methods.getObject('GetvItemMasterProductList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnItemID.ClientID %>').val(result.ItemID);
                        $('#<%=txtItemName.ClientID %>').val(result.ItemName1);

                        Methods.getItemMasterPurchase(result.ItemID, $('#<%=hdnSupplierID.ClientID %>').val(), function (result2) {
                            if (result2 != null) {
                                $('#<%=hdnItemGroupID.ClientID %>').val(result2.ItemGroupID);
                                $('#<%=txtItemGroupCode.ClientID %>').val(result2.ItemGroupCode);
                                $('#<%=txtItemGroupName.ClientID %>').val(result2.ItemGroupName1);
                                $('#<%=hdnUnitPrice.ClientID %>').val(result2.Price);
                                $('#<%=hdnPurchaseUnitPrice.ClientID %>').val(result2.UnitPrice);
                                cboItemUnit.SetValue(result2.PurchaseUnit);
                                $('#<%=hdnGCBaseUnit.ClientID %>').val(result2.ItemUnit);
                                $('#<%=hdnGCItemUnit.ClientID %>').val(result2.PurchaseUnit);
                                $('#<%=txtDiscount.ClientID %>').val(result2.Discount).trigger('changeValue');
                                $('#<%=txtDiscount2.ClientID %>').val(result2.Discount2).trigger('changeValue');
                            }
                            else {
                                $('#<%=hdnUnitPrice.ClientID %>').val('0');
                                $('#<%=hdnPurchaseUnitPrice.ClientID %>').val('0');
                                cboItemUnit.SetValue(result.GCItemUnit);
                                $('#<%=hdnGCBaseUnit.ClientID %>').val(result.GCItemUnit);
                                $('#<%=hdnGCItemUnit.ClientID %>').val(result.GCItemUnit);
                                $('#<%=txtDiscount.ClientID %>').val("0").trigger('changeValue');
                                $('#<%=txtDiscount2.ClientID %>').val("0").trigger('changeValue');
                            }
                        });

                        cboItemUnit.PerformCallback('addItem');
                    }
                    else {
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
                        $('#<%=txtSubTotalPrice.ClientID %>').val('').trigger('changeValue');
                        $('#<%=txtSupplierCode.ClientID %>').attr('readonly', 'readonly');
                        $('#<%=lblProductLine.ClientID %>').attr('class', 'lblDisabled');
                        $('#<%=txtProductLineCode.ClientID %>').attr('readonly', 'readonly');
                        $('#<%=lblLocation.ClientID %>').attr('class', 'lblDisabled');
                        $('#<%=txtLocationCode.ClientID %>').attr('readonly', 'readonly');
                        $('#<%=txtStockLocation.ClientID %>').val('');
                        lastTransactionAmount = $('#<%=txtTotalPurchase.ClientID %>').attr('hiddenVal');
                        editedLineAmount = 0;
                        cboItemUnit.SetValue('');
                        $('#<%=txtConversion.ClientID %>').val('');
                        $('#<%=hdnTempFactor.ClientID %>').val('0');
                        $('#<%=hdnConversionFactor.ClientID %>').val('1');
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

                var price = parseFloat($('#<%=txtPrice.ClientID %>').val().replace('.00', '').split(',').join(''));
                var qty = parseFloat($('#<%=txtQuantity.ClientID %>').val().replace('.00', '').split(',').join(''));
                var discountAmount1 = parseFloat($('#<%=txtDiscountAmount.ClientID %>').val().replace('.00', '').split(',').join(''));
                var subTotal = parseFloat((price * qty) - discountAmount1);
                var discountAmount2 = parseFloat($(this).val().replace('.00', '').split(',').join(''));
                var discountPercent2 = parseFloat(discountAmount2 / subTotal * 100).toFixed(2);

                $('#<%=hdnDiscountAmount2.ClientID %>').val(discountAmount2);
                $('#<%=txtDiscount2.ClientID %>').val(discountPercent2).trigger('changeValue');

                calculateSubTotal();
            });
            //#endregion

            $('#<%=rblDiscountType.ClientID %>').live('change', function () {
                var displayOption = $('#<%=rblDiscountType.ClientID %>').find(":checked").val();
                if (displayOption == 0) { // persen
                    $('#<%:trFinalDiskonPersentase.ClientID %>').removeAttr('style');
                    $('#<%:trFinalDiskon.ClientID %>').attr('style', 'display:none');
                    $('#<%=txtFinalDiscount.ClientID %>').val(0).trigger('changeValue');
                    calculateTotal();
                }
                else { // amount
                    $('#<%:trFinalDiskon.ClientID %>').removeAttr('style');
                    $('#<%:trFinalDiskonPersentase.ClientID %>').attr('style', 'display:none');
                    $('#<%=txtFinalDiscountInPercentage.ClientID %>').val(0).trigger('changeValue');
                    calculateTotal();
                }
            });

            $('#lblAddData').click(function (evt) {
                if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                    $('#<%=hdnIsEdit.ClientID %>').val('0');
                    $('#<%=txtQuantity.ClientID %>').val('1');
                    $('#<%=hdnTempFactor.ClientID %>').val('0');
                    $('#<%=hdnEntryID.ClientID %>').val('');
                    $('#<%=hdnItemID.ClientID %>').val('');
                    $('#<%=hdnGCItemUnit.ClientID %>').val('');
                    $('#<%=hdnGCBaseUnit.ClientID %>').val('');
                    $('#<%=txtItemGroupCode.ClientID %>').val('');
                    $('#<%=txtItemGroupName.ClientID %>').val('');
                    $('#<%=txtItemCode.ClientID %>').val('');
                    $('#<%=txtItemName.ClientID %>').val('');
                    $('#<%=txtPrice.ClientID %>').val('').trigger('changeValue');
                    $('#<%=hdnPurchaseUnitPrice.ClientID %>').val(parseFloat(0));
                    $('#<%=txtBaseUnit.ClientID %>').val('');
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
                    $('#<%=txtSubTotalPrice.ClientID %>').val('').trigger('changeValue');
                    $('#<%=lblSupplier.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%=txtSupplierCode.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtLocationCode.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=lblLocation.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%=txtStockLocation.ClientID %>').val('');
                    lastTransactionAmount = parseFloat($('#<%=txtTotalPurchase.ClientID %>').attr('hiddenVal'));
                    editedLineAmount = 0;
                    cboItemUnit.SetValue('');
                    cboDirectPurchaseType.SetEnabled(false);
                    $('#<%=txtConversion.ClientID %>').val('');

                    $('#containerEntry').show();
                }
            });

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

                $('#<%=hdnEntryID.ClientID %>').val(entity.ID);

                $('#<%=hdnGCBaseUnit.ClientID %>').val(entity.GCBaseUnit);
                $('#<%=hdnGCItemUnit.ClientID %>').val(entity.GCItemUnit);
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

                lastTransactionAmount = $('#<%=txtTotalPurchase.ClientID %>').attr('hiddenVal');
                editedLineAmount = parseFloat(entity.CustomSubTotal2);

                GetItemQtyFromLocation("edit");
                cboItemUnit.PerformCallback("edit");

                $('#<%=txtPrice.ClientID %>').val(entity.UnitPrice).trigger('changeValue');
                $('#<%=hdnPurchaseUnitPrice.ClientID %>').val(entity.UnitPrice);

                $('#<%=txtDiscount.ClientID %>').val(entity.DiscountPercentage).trigger('changeValue');
                $('#<%=hdnDiscountAmount1.ClientID %>').val(entity.DiscountAmount);
                $('#<%=txtDiscountAmount.ClientID %>').val(entity.DiscountAmount).trigger('changeValue');

                $('#<%=txtDiscount2.ClientID %>').val(entity.DiscountPercentage2).trigger('changeValue');
                $('#<%=hdnDiscountAmount2.ClientID %>').val(entity.DiscountAmount2);
                $('#<%=txtDiscountAmount2.ClientID %>').val(entity.DiscountAmount2).trigger('changeValue');

                var IsDiscountInPercentage1 = entity.IsDiscountInPercentage;
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

            $('#lblQuickPick').die('click');
            $('#lblQuickPick').live('click', function () {
                if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
                    showLoadingPanel();
                    var url = ResolveUrl('~/Program/Procurement/DirectPurchase/DirectPurchaseQuickPicksCtl.ascx');
                    var purchaseID = $('#<%=hdnPurchaseID.ClientID %>').val();
                    var locationID = $('#<%=hdnLocationID.ClientID %>').val();
                    var supplierID = $('#<%=hdnSupplierID.ClientID %>').val();
                    var GCLocationGroup = $('#<%=hdnGCLocationGroup.ClientID %>').val();
                    var ProductLineID = $('#<%=hdnProductLineID.ClientID %>').val();
                    var GCItemType = $('#<%=hdnProductLineItemType.ClientID %>').val();
                    var locationItemGroupID = $('#<%=hdnLocationItemGroupID.ClientID %>').val();
                    var id = purchaseID + '|' + locationID + '|' + locationItemGroupID + '|' + supplierID + '|' + GCLocationGroup + '|' + ProductLineID + '|' + GCItemType;
                    openUserControlPopup(url, id, 'Quick Picks', 1000, 600);
                }
            });

            $('#btnCancel').click(function () {
                $('#containerEntry').hide();
                cbpView.PerformCallback('refresh');
            });

            $('#btnSave').click(function (evt) {
                if (IsValid(evt, 'fsTrxPopup', 'mpTrxPopup'))
                    cbpProcess.PerformCallback('save');
            });

            $('#<%=txtDiscount.ClientID %>').change(function () {
                calculateSubTotal();
            });

            $('#<%=txtQuantity.ClientID %>').change(function () {
                if ($(this).val() == '') {
                    $(this).val('1');
                }

                $('#<%=txtDiscount.ClientID %>').trigger('change');

                calculateSubTotal();
            });

            $('#<%=txtPrice.ClientID %>').change(function () {
                $(this).blur();

                $('#<%=txtDiscount.ClientID %>').trigger('change');

                calculateSubTotal();
            });

            $('#<%=txtDiscount.ClientID %>').change(function () {
                calculateSubTotal();
            });

            $('#<%=txtFinalDiscountInPercentage.ClientID %>').die('change');
            $('#<%=txtFinalDiscountInPercentage.ClientID %>').live('change', function () {
                var finalDiscPct = $('#<%=txtFinalDiscountInPercentage.ClientID %>').val();
                if (finalDiscPct > 100) {
                    ShowSnackbarError("Harap isikan nilai persentase Diskon Final dari angka 0 s/d angka 100.");
                    $('#<%=txtFinalDiscountInPercentage.ClientID %>').val("0").trigger('changeValue');
                    $('#<%=txtFinalDiscount.ClientID %>').val("0").trigger('changeValue');
                }
                calculateTotal();
            });

            $('#<%=txtFinalDiscount.ClientID %>').die('change');
            $('#<%=txtFinalDiscount.ClientID %>').live('change', function () {
                $(this).trigger('changeValue');
                var totalKotor = parseFloat($('#<%=txtTotalPurchase.ClientID %>').attr('hiddenVal').replace('.00', '').split(',').join(''));
                var finalDisc = parseFloat($('#<%=txtFinalDiscount.ClientID %>').val().attr('hiddenVal').replace('.00', '').split(',').join(''));
                if (finalDisc > totalKotor) {
                    ShowSnackbarError("Harap isikan nilai persentase Diskon Final tidak melebihi Jumlah Nilai Pembelian Tunai.");
                    $('#<%=txtFinalDiscountInPercentage.ClientID %>').val("0").trigger('changeValue');
                    $('#<%=txtFinalDiscount.ClientID %>').val("0").trigger('changeValue');
                }
                calculateTotal();
            });

            $('#<%=chkPPN.ClientID %>').die('change');
            $('#<%=chkPPN.ClientID %>').live('change', function () {
                var isEditable = $('#<%=hdnIsPpnAllowChanged.ClientID %>').val();
                if ($('#<%=chkPPN.ClientID %>').is(':checked')) {
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

            $('.txtCurrency').each(function () {
                $(this).trigger('changeValue');
            });

            var pageCount = parseInt($('#<%=hdnPageCount.ClientID %>').val());
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });

            $('#<%=txtFinalDiscountInPercentage.ClientID %>').change();
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

        function calculateTotal() {
            var totalKotor = parseFloat($('#<%=txtTotalPurchase.ClientID %>').attr('hiddenVal').replace('.00', '').split(',').join(''));
            var finalDisc = 0;
            var displayOption = $('#<%=rblDiscountType.ClientID %>').find(":checked").val();
            if (displayOption == 0) { // persen
                if (parseFloat($('#<%=txtFinalDiscountInPercentage.ClientID %>').val()) != 0) {
                    finalDisc = (parseFloat($('#<%=txtFinalDiscountInPercentage.ClientID %>').val()) / 100) * totalKotor;
                }
            }
            else { // amount
                if ($('#<%=txtFinalDiscount.ClientID %>').val() != 0) {
                    finalDisc = parseFloat($('#<%=txtFinalDiscount.ClientID %>').attr('hiddenVal'));
                }
            }

            if ($('#<%=chkPPN.ClientID %>').is(':checked')) {
                var temp = totalKotor - finalDisc;

                var PPN = parseFloat($('#<%=hdnVATPercentage.ClientID %>').val()) / 100 * temp;

                $('#<%=txtPPN.ClientID %>').val(PPN).trigger('changeValue');
            }
            else {
                $('#<%=txtPPN.ClientID %>').val('0').trigger('changeValue');
            }

            var PPN = parseFloat($('#<%=txtPPN.ClientID %>').attr('hiddenVal'));
            var totalHarga = totalKotor - finalDisc + PPN;

            $('#<%=txtTotalDirectPurchase.ClientID %>').val(totalHarga).trigger('changeValue');
        }

        function calculateSubTotal() {
            var price = parseFloat($('#<%=txtPrice.ClientID %>').attr('hiddenVal').replace('.00', '').split(',').join(''));
            var qty = $('#<%=txtQuantity.ClientID %>').val();
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

            var totalPurchase = lastTransactionAmount - editedLineAmount + subTotal;

            $('#<%=txtTotalPurchase.ClientID %>').val(totalPurchase).trigger('changeValue');
            calculateTotal();
        }

        function onAfterSaveRecordDtSuccess(PurchaseID) {
            var directPurchaseNo;
            if ($('#<%=hdnPurchaseID.ClientID %>').val() == '0') {
                $('#<%=hdnPurchaseID.ClientID %>').val(PurchaseID);
                var filterExpression = 'DirectPurchaseID = ' + PurchaseID;
                Methods.getObject('GetDirectPurchaseHdList', filterExpression, function (result) {
                    $('#<%=txtDirectPurchaseNo.ClientID %>').val(result.DirectPurchaseNo);
                    directPurchaseNo = result.DirectPurchaseNo;
                });
                onAfterCustomSaveSuccess();
                onLoadObject(directPurchaseNo);
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
                    var PurchaseID = s.cpPurchaseID;
                    onAfterSaveRecordDtSuccess(PurchaseID);
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

        //#region Paging
        function onCbpViewEndCallback(s) {
            hideLoadingPanel();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                var totalPurchase = parseFloat(param[2]);

                $('#<%=txtTotalPurchase.ClientID %>').val(totalPurchase).trigger('changeValue');
                calculateTotal();
                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
        }
        //#endregion

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var purchaseID = $('#<%=hdnPurchaseID.ClientID %>').val();
            var printStatus = $('#<%=hdnPrintStatus.ClientID %>').val();
            if (printStatus == 'true') {
                if (purchaseID == '' || purchaseID == '0') {
                    errMessage.text = 'Please Set Transaction First!';
                    return false;
                }
                else {
                    filterExpression.text = "DirectPurchaseID = " + purchaseID;
                    return true;
                }
            }
            else {
                errMessage.text = "Data Doesn't Approved or Closed";
                return false;
            }
        }

    </script>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnPrintStatus" runat="server" />
    <input type="hidden" value="" id="hdnPurchaseID" runat="server" />
    <input type="hidden" value="" id="hdnPageCount" runat="server" />
    <input type="hidden" value="" id="hdnGCTransactionStatus" runat="server" />
    <input type="hidden" value="1" id="hdnIsEditable" runat="server" />
    <input type="hidden" value="" id="hdnVATPercentageFromSetvar" runat="server" />
    <input type="hidden" value="" id="hdnVATPercentage" runat="server" />
    <input type="hidden" value="" id="hdnRecordFilterExpression" runat="server" />
    <input type="hidden" value="0" id="hdnIsEdit" runat="server" />
    <input type="hidden" value="0" id="hdnTempFactor" runat="server" />
    <input type="hidden" value="0" id="hdnIsUsedProductLine" runat="server" />
    <input type="hidden" value="0" id="hdnIsDiscountAppliedToAveragePrice" runat="server" />
    <input type="hidden" value="0" id="hdnIsDiscountAppliedToUnitPrice" runat="server" />
    <input type="hidden" value="0" id="hdnIsPPNAppliedToAveragePrice" runat="server" />
    <input type="hidden" value="0" id="hdnIsPPNAppliedToUnitPrice" runat="server" />
    <input type="hidden" value="1" id="hdnIsAutoUpdateToSupplierItem" runat="server" />
    <input type="hidden" value="0" id="hdnIsPpnAllowChanged" runat="server" />
    <input type="hidden" value="0" id="hdnIsUsingPurchaseDiscountShared" runat="server" />
    <input type="hidden" value="0" id="hdnIsCalculateHNA" runat="server" />
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
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblDirectPurchaseNo">
                                    <%=GetLabel("No. Pembelian")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDirectPurchaseNo" Width="150px" ReadOnly="true" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Tanggal Pembelian") %>
                            </td>
                            <td style="padding-right: 1px; width: 140px">
                                <asp:TextBox ID="txtDirectPurchaseDate" Width="120px" CssClass="datepicker" runat="server" />
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
                                        <col style="width: 5px" />
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
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Jenis Pembelian")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboDirectPurchaseType" ClientInstanceName="cboDirectPurchaseType"
                                    Width="51%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("No. Referensi")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtReferenceNo" Width="50%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Tanggal Referensi")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtReferenceDate" Width="120px" CssClass="datepicker" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Catatan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNotes" Width="100%" runat="server" TextMode="MultiLine" Rows="2" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div id="containerEntry" style="margin-top: 4px; display: none;">
                        <div class="pageTitle">
                            <%=GetLabel("Edit atau Tambah Item Pembelian Tunai")%></div>
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
                                                    <table style="width: 70%" cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width: 30%" />
                                                            <col style="width: 3px" />
                                                            <col />
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
                                                    <table style="width: 70%" cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width: 30%" />
                                                            <col style="width: 3px" />
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
                                                    <table style="width: 70%" cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width: 30%" />
                                                            <col />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtPrice" CssClass="txtCurrency" Width="180px" runat="server" />
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
                                                    <input type="hidden" value="<%#:Eval("Quantity") %>" bindingfield="Quantity" />
                                                    <input type="hidden" value="<%#:Eval("GCItemUnit") %>" bindingfield="GCItemUnit" />
                                                    <input type="hidden" value="<%#:Eval("GCBaseUnit") %>" bindingfield="GCBaseUnit" />
                                                    <input type="hidden" value="<%#:Eval("ItemUnit") %>" bindingfield="ItemUnit" />
                                                    <input type="hidden" value="<%#:Eval("BaseUnit") %>" bindingfield="BaseUnit" />
                                                    <input type="hidden" value="<%#:Eval("UnitPrice") %>" bindingfield="UnitPrice" />
                                                    <input type="hidden" value="<%#:Eval("IsDiscountInPercentage") %>" bindingfield="IsDiscountInPercentage" />
                                                    <input type="hidden" value="<%#:Eval("DiscountPercentage") %>" bindingfield="DiscountPercentage" />
                                                    <input type="hidden" value="<%#:Eval("DiscountAmount") %>" bindingfield="DiscountAmount" />
                                                    <input type="hidden" value="<%#:Eval("IsDiscountInPercentage2") %>" bindingfield="IsDiscountInPercentage2" />
                                                    <input type="hidden" value="<%#:Eval("DiscountPercentage2") %>" bindingfield="DiscountPercentage2" />
                                                    <input type="hidden" value="<%#:Eval("DiscountAmount2") %>" bindingfield="DiscountAmount2" />
                                                    <input type="hidden" value="<%#:Eval("ConversionFactor") %>" bindingfield="ConversionFactor" />
                                                    <input type="hidden" value="<%#:Eval("GCItemDetailStatus") %>" bindingfield="GCItemDetailStatus" />
                                                    <input type="hidden" value="<%#:Eval("CustomSubTotal2") %>" bindingfield="CustomSubTotal2" />
                                                    <input type="hidden" value="<%#:Eval("CustomDiscount") %>" bindingfield="CustomDiscount" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ItemCode" HeaderText="Kode Item" HeaderStyle-Width="110px"
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="ItemName1" HeaderText="Nama Item" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="Quantity" HeaderText="Qty" HeaderStyle-Width="70px" HeaderStyle-HorizontalAlign="Right"
                                                ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="ItemUnit" HeaderText="Satuan" HeaderStyle-Width="120px"
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="CustomUnitPrice" HeaderText="Harga / Satuan" HeaderStyle-Width="150px"
                                                HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
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
                                            <asp:BoundField DataField="CustomSubTotal2" HeaderText="SubTotal" HeaderStyle-Width="180px"
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
                        <span class="lblLink" id="lblAddData" style="margin-right: 300px;">
                            <%= GetLabel("Add Data")%></span> <span class="lblLink" id="lblQuickPick">
                                <%= GetLabel("Quick Picks")%></span>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div id="containerTotalPembelian" style="margin-top: 20px;">
                        <fieldset id="fsTotalOrder" style="margin: 0">
                            <table style="width: 100%;">
                                <colgroup>
                                    <col style="width: 50%" />
                                    <col style="width: 40px" />
                                </colgroup>
                                <tr>
                                    <td valign="top">
                                        &nbsp;
                                    </td>
                                    <td align="right">
                                        <table style="width: 100%;">
                                            <colgroup>
                                                <col style="width: 180px" />
                                            </colgroup>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Jumlah Nilai Pembelian Tunai")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtTotalPurchase" CssClass="txtCurrency" ReadOnly="true" Width="180px"
                                                        runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Jenis Diskon")%></label>
                                                </td>
                                                <td colspan="2">
                                                    <asp:RadioButtonList ID="rblDiscountType" runat="server" RepeatDirection="Horizontal">
                                                        <asp:ListItem Text="In Percentage" Value="0" Selected="True" />
                                                        <asp:ListItem Text="In Amount" Value="1" />
                                                    </asp:RadioButtonList>
                                                </td>
                                            </tr>
                                            <tr id="trFinalDiskonPersentase" runat="server" style="display: none">
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Diskon Final %")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtFinalDiscountInPercentage" CssClass="txtCurrency" Width="180px"
                                                        runat="server" />
                                                </td>
                                            </tr>
                                            <tr id="trFinalDiskon" runat="server" style="display: none">
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Diskon Final")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtFinalDiscount" CssClass="txtCurrency" Width="180px" runat="server"
                                                        hiddenval="0" />
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
                                                        hiddenval="0" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Total Nilai Pembelian Tunai")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtTotalDirectPurchase" CssClass="txtCurrency" ReadOnly="true" Width="180px"
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
                                                <tr id="trApprovedBy" style="display: none" runat="server">
                                                    <td align="left">
                                                        <%=GetLabel("Approved Oleh")%>
                                                    </td>
                                                    <td align="center">
                                                        :
                                                    </td>
                                                    <td>
                                                        <div runat="server" id="divApprovedBy">
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr id="trApprovedDate" style="display: none" runat="server">
                                                    <td align="left">
                                                        <%=GetLabel("Approved Pada") %>
                                                    </td>
                                                    <td align="center">
                                                        :
                                                    </td>
                                                    <td>
                                                        <div runat="server" id="divApprovedDate">
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr id="trVoidBy" style="display: none" runat="server">
                                                    <td align="left">
                                                        <%=GetLabel("Void Oleh")%>
                                                    </td>
                                                    <td align="center">
                                                        :
                                                    </td>
                                                    <td>
                                                        <div runat="server" id="divVoidBy">
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr id="trVoidDate" style="display: none" runat="server">
                                                    <td align="left">
                                                        <%=GetLabel("Void Pada") %>
                                                    </td>
                                                    <td align="center">
                                                        :
                                                    </td>
                                                    <td>
                                                        <div runat="server" id="divVoidDate">
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
