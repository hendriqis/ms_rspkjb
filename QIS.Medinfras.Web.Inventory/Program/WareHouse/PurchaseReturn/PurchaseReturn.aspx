<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx2.master"
    AutoEventWireup="true" CodeBehind="PurchaseReturn.aspx.cs" Inherits="QIS.Medinfras.Web.Inventory.Program.PurchaseReturn" %>

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

            setDatePicker('<%=txtPurchaseReturnDate.ClientID %>');
            $('#<%=txtPurchaseReturnDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

            setDatePicker('<%=txtTaxInvoiceDate.ClientID %>');
            $('#<%=txtTaxInvoiceDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

            //#region Purchase Return No
            function onGetPurchaseReturnNoFilterExpression() {
                var filterExpression = "<%:GetFilterExpression() %>";
                return filterExpression;
            }

            $('#lblReturnNo.lblLink').click(function () {
                openSearchDialog('purchasereturnhd', onGetPurchaseReturnNoFilterExpression(), function (value) {
                    $('#<%=txtReturnNo.ClientID %>').val(value);
                    onTxtPurchaseReturnNoChanged(value);
                });
            });

            $('#<%=txtReturnNo.ClientID %>').change(function () {
                onTxtPurchaseReturnNoChanged($(this).val());
            });

            function onTxtPurchaseReturnNoChanged(value) {
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
                var filterExpression = getSupplierFilterExpression() + " AND BusinessPartnerCode = '" + value + "'";
                Methods.getObject('GetBusinessPartnersList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnSupplierID.ClientID %>').val(result.BusinessPartnerID);
                        $('#<%=txtSupplierName.ClientID %>').val(result.BusinessPartnerName);
                        $('#<%=hdnPurchaseReceiveID.ClientID %>').val('0');
                        $('#<%=txtPurchaseReceiveNo.ClientID %>').val('');
                        if (result.IsTaxable.toString() == "true") {
                            $('#<%=hdnIsSupplierIsPKP.ClientID %>').val("1");
                        } else {
                            $('#<%=hdnIsSupplierIsPKP.ClientID %>').val("0");
                        }
                        checkTaxInformationForMandatory();
                    }
                    else {
                        $('#<%=hdnSupplierID.ClientID %>').val('');
                        $('#<%=txtSupplierCode.ClientID %>').val('');
                        $('#<%=txtSupplierName.ClientID %>').val('');
                        $('#<%=hdnPurchaseReceiveID.ClientID %>').val('0');
                        $('#<%=txtPurchaseReceiveNo.ClientID %>').val('');
                        $('#<%=hdnIsSupplierIsPKP.ClientID %>').val("0");
                    }
                });
            }
            //#endregion

            //#region Purchase Receive No
            function onGetPurchaseReceiveFilterExpression() {
                var filterExpression = "TransactionCode = '4203' AND GCTransactionStatus IN ('X121^003','X121^004','X121^005')";

                if ($('#<%=hdnSupplierID.ClientID %>').val() != "") {
                    filterExpression += " AND SupplierID = " + $('#<%=hdnSupplierID.ClientID %>').val();
                }

                return filterExpression;
            }

            $('#<%=lblPurchaseReceiveNo.ClientID %>.lblLink').live('click', function () {
                openSearchDialog('purchasereceivehd', onGetPurchaseReceiveFilterExpression(), function (value) {
                    $('#<%=txtPurchaseReceiveNo.ClientID %>').val(value);
                    onTxtPurchaseReceiveNoChanged(value);
                });
            });

            $('#<%=txtPurchaseReceiveNo.ClientID %>').change(function () {
                onTxtPurchaseReceiveNoChanged($(this).val());
            });

            function onTxtPurchaseReceiveNoChanged(value) {
                var isEditablePPN = $('#<%=hdnIsPpnAllowChanged.ClientID %>').val();
                var IM0128 = $('#<%=hdnIM0128.ClientID %>').val();
                var filterExpression = onGetPurchaseReceiveFilterExpression() + " AND PurchaseReceiveNo = '" + value + "'";
                Methods.getObject('GetvPurchaseReceiveHdList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnPurchaseReceiveID.ClientID %>').val(result.PurchaseReceiveID);
                        $('#<%=hdnPurchaseReceiveDtID.ClientID %>').val(result.ID);
                        $('#<%=txtPurchaseReceiveNo.ClientID %>').val(result.PurchaseReceiveNo);
                        $('#<%=hdnLocationID.ClientID %>').val(result.LocationID);
                        $('#<%=txtLocationCode.ClientID %>').val(result.LocationCode);
                        $('#<%=txtLocationName.ClientID %>').val(result.LocationName);
                        $('#<%=hdnSupplierID.ClientID %>').val(result.SupplierID);
                        $('#<%=txtSupplierCode.ClientID %>').val(result.SupplierCode);
                        $('#<%=txtSupplierName.ClientID %>').val(result.SupplierName);
                        $('#<%=txtReferenceNo.ClientID %>').val(result.ReferenceNo);
                        $('#<%=txtReferenceDate.ClientID %>').val(Methods.getJSONDateValue(result.ReferenceDate));

                        if (IM0128 == "1") {
                            $('#<%=chkPPN.ClientID %>').prop('checked', result.IsIncludeVAT);
                        }

                        if (result.IsTaxable.toString() == "true") {
                            $('#<%=hdnIsSupplierIsPKP.ClientID %>').val("1");
                            if (isEditablePPN == '1') {
                                $('#<%:txtVATPercentageDefault.ClientID %>').removeAttr('readonly');
                            }
                            else {
                                $('#<%:txtVATPercentageDefault.ClientID %>').attr('readonly', 'readonly');
                            }
                        } else {
                            $('#<%=hdnIsSupplierIsPKP.ClientID %>').val("0");
                            $('#<%:txtVATPercentageDefault.ClientID %>').attr('readonly', 'readonly');
                        }
                        checkTaxInformationForMandatory();
                    }
                    else {
                        $('#<%=hdnPurchaseReceiveID.ClientID %>').val('0');
                        $('#<%=hdnPurchaseReceiveDtID.ClientID %>').val('0');
                        $('#<%=txtPurchaseReceiveNo.ClientID %>').val('');
                        $('#<%=hdnLocationID.ClientID %>').val('');
                        $('#<%=txtLocationCode.ClientID %>').val('');
                        $('#<%=txtLocationName.ClientID %>').val('');
                        $('#<%=hdnSupplierID.ClientID %>').val('');
                        $('#<%=txtSupplierCode.ClientID %>').val('');
                        $('#<%=txtSupplierName.ClientID %>').val('');
                        $('#<%=txtReferenceNo.ClientID %>').val('');
                        $('#<%=chkPPN.ClientID %>').prop('checked', false);
                        $('#<%:txtVATPercentageDefault.ClientID %>').attr('readonly', 'readonly');
                        $('#<%=hdnIsSupplierIsPKP.ClientID %>').val("0");
                    }
                });
            }
            //#endregion

            $('#<%=txtPrice.ClientID %>').die('change');
            $('#<%=txtPrice.ClientID %>').live('change', function () {
                $(this).blur();
                calculateSubTotal();
            });

            $('#<%=txtQuantity.ClientID %>').die('change');
            $('#<%=txtQuantity.ClientID %>').live('change', function () {
                var unitPrice = parseFloat($('#<%=txtPrice.ClientID %>').val().replace('.00', '').split(',').join(''));
                var qtyORI = parseFloat($('#<%=hdnQtyORI.ClientID %>').val());
                var convFactor = parseFloat($('#<%=hdnConversionFactor.ClientID %>').val());
                var convFactorORI = parseFloat($('#<%=hdnConversionFactorORI.ClientID %>').val());
                var inputqty = parseFloat($('#<%=txtQuantity.ClientID %>').val());
                if (inputqty > 0) {
                    var discPct1 = parseFloat($('#<%=txtDiscount.ClientID %>').val());
                    var discAmount1 = parseFloat($('#<%=txtDiscountAmount.ClientID %>').val().replace('.00', '').split(',').join(''));

                    discAmount1 = parseFloat((unitPrice * inputqty) * discPct1 / 100).toFixed(2);
                    $('#<%=txtDiscountAmount.ClientID %>').val(discAmount1).trigger('changeValue');

                    var discPct2 = parseFloat($('#<%=txtDiscount2.ClientID %>').val());
                    var discAmount2 = parseFloat($('#<%=txtDiscountAmount2.ClientID %>').val().replace('.00', '').split(',').join(''));

                    discAmount2 = parseFloat(((unitPrice * inputqty) - discAmount1) * discPct2 / 100).toFixed(2);
                    $('#<%=txtDiscountAmount2.ClientID %>').val(discAmount2).trigger('changeValue');

                } else {
                    $('#<%=txtQuantity.ClientID %>').val('1');
                    showToast('Input Failed', 'Quantity harus lebih besar dari 0 !');
                }

                $('#<%=hdnQtyORI.ClientID %>').val($('#<%=txtQuantity.ClientID %>').val());

                calculateSubTotal();
            });

            $('#<%=chkPPN.ClientID %>').die('change');
            $('#<%=chkPPN.ClientID %>').live('change', function () {
                var isEditable = $('#<%=hdnIsPpnAllowChanged.ClientID %>').val();
                checkTaxInformationForMandatory();
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

                $('#<%=txtVATPercentageDefault.ClientID %>').trigger('change');
            });

            $('#<%=txtVATPercentageDefault.ClientID %>').die('change');
            $('#<%=txtVATPercentageDefault.ClientID %>').live('change', function () {
                var vatPct = $('#<%=txtVATPercentageDefault.ClientID %>').val();
                if (vatPct != "") {
                    var position = vatPct.search(",");
                    if (parseInt(position) > 0) {
                        vatPct = vatPct.replace(",", ".");
                        $('#<%=txtVATPercentageDefault.ClientID %>').val(vatPct);
                    }
                }
                $('#<%:hdnVATPercentage.ClientID %>').val(vatPct);
                $('#<%=txtVATPercentageDefault.ClientID %>').trigger('changeValue');

                var subTotal = parseFloat($('#<%=txtTransactionAmount.ClientID %>').attr('hiddenVal'));
                var vatAmt = 0;
                if ($('#<%=chkPPN.ClientID %>').is(':checked')) {
                    vatAmt = parseFloat(vatPct / 100 * subTotal).toFixed(2);
                }
                $('#<%=txtPPN.ClientID %>').val(vatAmt).trigger('changeValue');


                $('#<%=txtReturnCostPct.ClientID %>').trigger('change');
            });

            $('#<%=chkIsReturnCostInPct.ClientID %>').die('change');
            $('#<%=chkIsReturnCostInPct.ClientID %>').live('change', function () {
                if ($(this).is(':checked')) {
                    $('#<%:txtReturnCostPct.ClientID %>').removeAttr('readonly');
                    $('#<%:txtReturnCostAmount.ClientID %>').attr('readonly', 'readonly');
                }
                else {
                    $('#<%:txtReturnCostPct.ClientID %>').attr('readonly', 'readonly');
                    $('#<%:txtReturnCostAmount.ClientID %>').removeAttr('readonly');
                }
            });

            $('#<%=txtReturnCostPct.ClientID %>').die('change');
            $('#<%=txtReturnCostPct.ClientID %>').live('change', function () {
                var subTotal = parseFloat($('#<%=txtTransactionAmount.ClientID %>').attr('hiddenVal').replace('.00', '').split(',').join(''));
                var vatAmt = parseFloat($('#<%=txtPPN.ClientID %>').attr('hiddenVal').replace('.00', '').split(',').join(''));
                var totalTemp = subTotal + vatAmt;

                var costPct = parseFloat($('#<%:txtReturnCostPct.ClientID %>').val().replace('.00', '').split(',').join(''));
                var costAmt = parseFloat(totalTemp * costPct / 100).toFixed(2);

                if (costPct > 100) {
                    costPct = parseFloat(100);
                    $('#<%:txtReturnCostPct.ClientID %>').val(costPct).trigger('change');
                } else {
                    $('#<%:txtReturnCostAmount.ClientID %>').val(costAmt).trigger('changeValue');
                }

                calculateTotal();
            });

            $('#<%=txtReturnCostAmount.ClientID %>').die('change');
            $('#<%=txtReturnCostAmount.ClientID %>').live('change', function () {
                var subTotal = parseFloat($('#<%=txtTransactionAmount.ClientID %>').attr('hiddenVal').replace('.00', '').split(',').join(''));
                var vatAmt = parseFloat($('#<%=txtPPN.ClientID %>').attr('hiddenVal').replace('.00', '').split(',').join(''));
                var totalTemp = subTotal + vatAmt;

                var costAmt = parseFloat($('#<%:txtReturnCostAmount.ClientID %>').val().replace('.00', '').split(',').join(''));
                var costPct = parseFloat(costAmt / totalTemp * 100).toFixed(2);

                if (costPct > 100) {
                    costPct = parseFloat(100);
                }
                $('#<%:txtReturnCostPct.ClientID %>').val(costPct).trigger('changeValue');

                calculateTotal();
            });

            if ($('#<%=chkIsReturnCostInPct.ClientID %>').is(':checked')) {
                $('#<%:txtReturnCostPct.ClientID %>').removeAttr('readonly');
                $('#<%:txtReturnCostAmount.ClientID %>').attr('readonly', 'readonly');
            }
            else {
                $('#<%:txtReturnCostPct.ClientID %>').attr('readonly', 'readonly');
                $('#<%:txtReturnCostAmount.ClientID %>').removeAttr('readonly');
            }


            var pageCount = parseInt($('#<%=hdnPageCount.ClientID %>').val());
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });

            $('.txtCurrency').each(function () {
                $(this).trigger('changeValue');
            });

            calculateTotal();
        }

        //#region Item
        function getItemFilterExpression() {
            var returnID = $('#<%=hdnPRID.ClientID %>').val();
            var receiveID = $('#<%=hdnPurchaseReceiveID.ClientID %>').val();

            var filterExpression = "";

            if (receiveID != '') {
                filterExpression += "PurchaseReceiveID = " + receiveID + " AND GCItemDetailStatus != 'X121^999' AND (Quantity - ReturnedQuantity) > 0";
            }
            if (returnID != '') {
                filterExpression += " AND ID NOT IN (SELECT PurchaseReceiveDtID FROM PurchaseReturnDt WHERE PurchaseReturnID = " + returnID + " AND GCItemDetailStatus != 'X121^999')";
            }

            return filterExpression;
        }

        $('#lblItem.lblLink').live('click', function () {
            openSearchDialog('purchasereceivedt', getItemFilterExpression(), function (value) {
                onTxtItemCodeChanged(value);
            });
        });

        function onTxtItemCodeChanged(value) {
            var filterExpression = "ID = " + value;
            Methods.getObject('GetvPurchaseReceiveDtList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnPurchaseReceiveDtID.ClientID %>').val(result.ID);
                    $('#<%=hdnItemID.ClientID %>').val(result.ItemID);
                    $('#<%=txtItemCode.ClientID %>').val(result.ItemCode);
                    $('#<%=txtItemName.ClientID %>').val(result.ItemName1);
                    $('#<%=hdnGCBaseUnit.ClientID %>').val(result.GCBaseUnit);
                    $('#<%=hdnGCItemUnit.ClientID %>').val(result.GCItemUnit);

                    var receiveQtyBaseUnit = parseFloat(result.ReceiveForReturnQuantity) * parseFloat(result.ConversionFactor);
                    $('#<%=hdnReceiveQtyBaseUnit.ClientID %>').val(receiveQtyBaseUnit);

                    var pricePerBaseUnit = parseFloat(result.UnitPrice) / parseFloat(result.ConversionFactor);
                    $('#<%=hdnUnitPrice.ClientID %>').val(pricePerBaseUnit);
                    $('#<%=txtPrice.ClientID %>').val(result.UnitPrice).trigger('changeValue');

                    if (result.IsDiscountInPercentage1.toString() == "false") {
                        $('#<%=chkIsDiscountInPercentage1.ClientID %>').prop('checked', false);
                    } else {
                        $('#<%=chkIsDiscountInPercentage1.ClientID %>').prop('checked', true);
                    }
                    $('#<%=txtDiscount.ClientID %>').val(result.DiscountPercentage1).trigger('changeValue');
                    var discAmount1 = parseFloat(result.DiscountAmount1 / result.Quantity).toFixed(2);
                    $('#<%=txtDiscountAmount.ClientID %>').val(discAmount1).trigger('changeValue');

                    if (result.IsDiscountInPercentage2.toString() == "false") {
                        $('#<%=chkIsDiscountInPercentage2.ClientID %>').prop('checked', false);
                    } else {
                        $('#<%=chkIsDiscountInPercentage2.ClientID %>').prop('checked', true);
                    }
                    $('#<%=txtDiscount2.ClientID %>').val(result.DiscountPercentage2).trigger('changeValue');
                    var discAmount2 = parseFloat(result.DiscountAmount2 / result.Quantity).toFixed(2);
                    $('#<%=txtDiscountAmount2.ClientID %>').val(discAmount2).trigger('changeValue');


                    $('#<%=txtReceivedQty.ClientID %>').val(result.Quantity);
                    $('#<%=txtReceivedUnit.ClientID %>').val(result.ItemUnit);

                    $('#<%=txtQuantity.ClientID %>').attr('min', '0');
                    $('#<%=txtQuantity.ClientID %>').attr('max', result.ReceiveForReturnQuantity);

                    $('#<%=hdnConversionFactor.ClientID %>').val(result.ConversionFactor);
                    $('#<%=hdnConversionFactorORI.ClientID %>').val($('#<%=hdnConversionFactor.ClientID %>').val());
                    $('#<%=txtQuantity.ClientID %>').val('1');
                    $('#<%=hdnQtyORI.ClientID %>').val($('#<%=txtQuantity.ClientID %>').val());

                    cboItemUnit.PerformCallback();
                }
                else {
                    $('#<%=hdnPurchaseReceiveDtID.ClientID %>').val('');
                    $('#<%=hdnItemID.ClientID %>').val('');
                    $('#<%=txtItemName.ClientID %>').val('');
                    $('#<%=chkIsDiscountInPercentage1.ClientID %>').prop('checked', false);
                    $('#<%=txtDiscount.ClientID %>').val('0.00');
                    $('#<%=txtDiscountAmount.ClientID %>').val('0.00');
                    $('#<%=chkIsDiscountInPercentage2.ClientID %>').prop('checked', false);
                    $('#<%=txtDiscount2.ClientID %>').val('0.00');
                    $('#<%=txtDiscountAmount2.ClientID %>').val('0.00');
                    $('#<%=hdnUnitPrice.ClientID %>').val('0');
                    $('#<%=txtPrice.ClientID %>').val('0.00');
                    $('#<%=txtReceivedQty.ClientID %>').val('0');
                    $('#<%=txtReceivedUnit.ClientID %>').val('');
                    $('#<%=hdnGCBaseUnit.ClientID %>').val('');
                    $('#<%=hdnGCItemUnit.ClientID %>').val('');
                    $('#<%=hdnConversionFactor.ClientID %>').val('1');
                    $('#<%=hdnConversionFactorORI.ClientID %>').val($('#<%=hdnConversionFactor.ClientID %>').val());

                    $('#<%=txtQuantity.ClientID %>').attr('min', '0');
                    $('#<%=txtQuantity.ClientID %>').attr('max', '1');

                    $('#<%=txtQuantity.ClientID %>').val('1');
                    $('#<%=hdnQtyORI.ClientID %>').val($('#<%=txtQuantity.ClientID %>').val());
                }
            });
        }
        //#endregion

        $('#lblAddData').live('click', function (evt) {
            if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                $('#<%=hdnPurchaseReceiveDtID.ClientID %>').val('0');
                $('#<%=hdnIsEdit.ClientID %>').val('0');
                $('#<%=hdnConversionFactor.ClientID %>').val('1');
                $('#<%=hdnConversionFactorORI.ClientID %>').val($('#<%=hdnConversionFactor.ClientID %>').val());
                $('#<%=txtQuantity.ClientID %>').val('1');
                $('#<%=hdnQtyORI.ClientID %>').val($('#<%=txtQuantity.ClientID %>').val());
                $('#<%=hdnEntryID.ClientID %>').val('');
                $('#<%=hdnItemID.ClientID %>').val('');
                $('#<%=hdnGCItemUnit.ClientID %>').val('');
                $('#<%=hdnGCBaseUnit.ClientID %>').val('');
                $('#<%=txtItemCode.ClientID %>').val('');
                $('#<%=txtItemName.ClientID %>').val('');
                $('#<%=txtPrice.ClientID %>').val('0');
                $('#<%=txtBaseUnit.ClientID %>').val('');
                $('#<%=txtReceivedQty.ClientID %>').val('0');
                $('#<%=txtReceivedUnit.ClientID %>').val('');
                $('#<%=lblSupplier.ClientID %>').attr('class', 'lblDisabled');
                $('#<%=lblPurchaseReceiveNo.ClientID %>').attr('class', 'lblDisabled');
                $('#<%=txtSupplierCode.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtPurchaseReceiveNo.ClientID %>').attr('readonly', 'readonly');
                cboReturnType.SetEnabled(false);
                $('#<%=txtQuantity.ClientID %>').attr('min', '0');
                $('#<%=txtQuantity.ClientID %>').removeAttr('max');
                lastTransactionAmount = parseFloat($('#<%=txtTransactionAmount.ClientID %>').attr('hiddenVal'));
                editedLineAmount = 0;
                cboItemUnit.SetValue('');
                cboReason.SetValue('');
                $('#<%=txtConversion.ClientID %>').val('');
                $('#<%=chkIsDiscountInPercentage1.ClientID %>').prop('checked', false);
                $('#<%=txtDiscount.ClientID %>').val('0.00');
                $('#<%=txtDiscountAmount.ClientID %>').val('0.00');
                $('#<%=chkIsDiscountInPercentage2.ClientID %>').prop('checked', false);
                $('#<%=txtDiscount2.ClientID %>').val('0.00');
                $('#<%=txtDiscountAmount2.ClientID %>').val('0.00');

                $('#<%=txtSubTotalPrice.ClientID %>').val('0').trigger('changeValue');
                $('#<%=txtReturnCostAmount.ClientID %>').val('0').trigger('changeValue');
                $('#<%=txtGrandTotal.ClientID %>').val('0').trigger('changeValue');

                $('#containerEntry').show();
            }
        });

        $('#<%=grdView.ClientID %> .imgEdit.imgLink').live('click', function () {
            $row = $(this).closest('tr').parent().closest('tr');
            var entity = rowToObject($row);
            $('#<%=hdnIsEdit.ClientID %>').val('1');
            $('#<%=hdnPurchaseReceiveDtID.ClientID %>').val(entity.PurchaseReceiveDtID);
            $('#<%=hdnEntryID.ClientID %>').val(entity.ID);
            $('#<%=hdnItemID.ClientID %>').val(entity.ItemID);
            $('#<%=txtItemCode.ClientID %>').val(entity.ItemCode);
            $('#<%=txtItemName.ClientID %>').val(entity.ItemName1);
            $('#<%=hdnConversionFactor.ClientID %>').val(entity.ConversionFactor);
            $('#<%=hdnConversionFactorORI.ClientID %>').val($('#<%=hdnConversionFactor.ClientID %>').val());
            $('#<%=txtQuantity.ClientID %>').val(entity.Quantity);
            $('#<%=hdnQtyORI.ClientID %>').val($('#<%=txtQuantity.ClientID %>').val());
            $('#<%=hdnGCBaseUnit.ClientID %>').val(entity.GCBaseUnit);
            $('#<%=hdnGCItemUnit.ClientID %>').val(entity.GCItemUnit);
            $('#<%=txtReceivedQty.ClientID %>').val(entity.ReceivedQuantity);
            $('#<%=txtReceivedUnit.ClientID %>').val(entity.ReceivedItemUnit);

            if (entity.IsDiscountInPercentage1.toString() == "False") {
                $('#<%=chkIsDiscountInPercentage1.ClientID %>').prop('checked', false);
            } else {
                $('#<%=chkIsDiscountInPercentage1.ClientID %>').prop('checked', true);
            }

            if (entity.IsDiscountInPercentage2.toString() == "False") {
                $('#<%=chkIsDiscountInPercentage2.ClientID %>').prop('checked', false);
            } else {
                $('#<%=chkIsDiscountInPercentage2.ClientID %>').prop('checked', true);
            }

            $('#<%=txtDiscount.ClientID %>').val(entity.DiscountPercentage1);
            $('#<%=txtDiscountAmount.ClientID %>').val(entity.DiscountAmount1).trigger('changeValue');
            $('#<%=txtDiscount2.ClientID %>').val(entity.DiscountPercentage2);
            $('#<%=txtDiscountAmount2.ClientID %>').val(entity.DiscountAmount2).trigger('changeValue');

            var receiveQtyBaseUnit = parseFloat(entity.ReceiveForReturnQuantity) * parseFloat(entity.ReceivedConversionFactor);
            $('#<%=hdnReceiveQtyBaseUnit.ClientID %>').val(receiveQtyBaseUnit);

            var pricePerBaseUnit = parseFloat(entity.UnitPrice) / parseFloat(entity.ConversionFactor);
            $('#<%=hdnUnitPrice.ClientID %>').val(pricePerBaseUnit);
            cboReason.SetValue(entity.GCPurchaseReturnReason);
            $('#<%=txtReason.ClientID %>').val(entity.PurchaseReturnReason);
            lastTransactionAmount = parseFloat($('#<%=txtTransactionAmount.ClientID %>').attr('hiddenVal'));
            editedLineAmount = entity.CustomSubTotal;
            cboItemUnit.PerformCallback();
            $('#containerEntry').show();
        });

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

        $('#btnCancel').live('click', function () {
            $('#containerEntry').hide();
        });

        $('#btnSave').live('click', function (evt) {
            if (IsValid(evt, 'fsTrxPopup', 'mpTrxPopup'))
                cbpProcess.PerformCallback('save');
        });

        function checkTaxInformationForMandatory() {
            var isTaxInvoiceMandatory = $('#<%=hdnIsTaxInvoiceMandatory.ClientID %>').val();
            var isSupplierPKP = $('#<%=hdnIsSupplierIsPKP.ClientID %>').val();

            if (isTaxInvoiceMandatory == "1") {
                if (isSupplierPKP == "1") {
                    $('#<%=txtTaxInvoiceNo.ClientID %>').addClass('required');
                    $('#<%=txtTaxInvoiceNo.ClientID %>').attr('validationgroup', 'mpEntry');
                    $('#<%=txtTaxInvoiceDate.ClientID %>').addClass('required');
                    $('#<%=txtTaxInvoiceDate.ClientID %>').attr('validationgroup', 'mpEntry');

                    var today = new Date();
                    var date = today.getDate();
                    if (parseInt(date) < 10) {
                        date = "0" + date;
                    }
                    var month = (today.getMonth() + 1);
                    if (parseInt(month) < 10) {
                        month = "0" + month;
                    }
                    var year = today.getFullYear();

                    var datePickerNow = date + '-' + month + '-' + year;
                    $('#<%=txtTaxInvoiceDate.ClientID %>').val(datePickerNow);

                } else {
                    $('#<%=txtTaxInvoiceNo.ClientID %>').removeClass('required');
                    $('#<%=txtTaxInvoiceNo.ClientID %>').removeAttr('validationgroup');
                    $('#<%=txtTaxInvoiceDate.ClientID %>').removeClass('required');
                    $('#<%=txtTaxInvoiceDate.ClientID %>').removeAttr('validationgroup');
                }
            } else {
                $('#<%=txtTaxInvoiceNo.ClientID %>').removeClass('required');
                $('#<%=txtTaxInvoiceNo.ClientID %>').removeAttr('validationgroup');
                $('#<%=txtTaxInvoiceDate.ClientID %>').removeClass('required');
                $('#<%=txtTaxInvoiceDate.ClientID %>').removeAttr('validationgroup');
            }
        }

        function calculateSubTotal() {
            var price = parseFloat($('#<%=txtPrice.ClientID %>').attr('hiddenVal').replace('.00', '').split(',').join(''));
            var qty = parseFloat($('#<%=txtQuantity.ClientID %>').val());
            var subTotal = price * qty;
            var discount1 = parseFloat($('#<%=txtDiscountAmount.ClientID %>').val().replace('.00', '').split(',').join(''));
            var discount2 = parseFloat($('#<%=txtDiscountAmount2.ClientID %>').val().replace('.00', '').split(',').join(''));

            subTotal = subTotal - discount1 - discount2;

            $('#<%=txtSubTotalPrice.ClientID %>').val(subTotal).trigger('changeValue');

            var totalOrder = lastTransactionAmount - editedLineAmount + subTotal;
            $('#<%=txtTransactionAmount.ClientID %>').val(totalOrder).trigger('changeValue');

            $('#<%=txtVATPercentageDefault.ClientID %>').trigger('change');

        }

        function calculateTotal() {
            var subTotal = parseFloat($('#<%=txtTransactionAmount.ClientID %>').attr('hiddenVal').replace('.00', '').split(',').join(''));
            var vatAmt = parseFloat($('#<%=txtPPN.ClientID %>').attr('hiddenVal').replace('.00', '').split(',').join(''));
            var costAmt = parseFloat($('#<%=txtReturnCostAmount.ClientID %>').val().replace('.00', '').split(',').join(''));

            var grandTotal = subTotal + vatAmt + costAmt;

            $('#<%=txtGrandTotal.ClientID %>').val(grandTotal).trigger('changeValue');
        }

        //#region cboItemUnit
        function onCboItemUnitEndCallBack() {
            if ($('#<%=hdnGCItemUnit.ClientID %>').val() == '') {
                cboItemUnit.SetValue($('#<%=hdnGCBaseUnit.ClientID %>').val());
            }
            else {
                cboItemUnit.SetValue($('#<%=hdnGCItemUnit.ClientID %>').val());
            }

            onCboItemUnitChanged();
        }

        function onCboItemUnitChanged() {
            var qtyBefore = $('#<%=txtQuantity.ClientID %>').val();
            var qtyAfter = parseFloat(qtyBefore.replace('.00', '').split(',').join(''));

            $('#<%=hdnConversionFactorORI.ClientID %>').val($('#<%=hdnConversionFactor.ClientID %>').val());
            var conversionBefore = parseFloat($('#<%=hdnConversionFactorORI.ClientID %>').val());

            var itemID = $('#<%=hdnItemID.ClientID %>').val();
            var baseValue = $('#<%=hdnGCBaseUnit.ClientID %>').val();
            var toUnitItem = cboItemUnit.GetValue();
            var baseText = getItemUnitName(baseValue);
            $('#<%=txtBaseUnit.ClientID %>').val("per " + cboItemUnit.GetText());

            var filterExpression = "ItemID = " + itemID + " AND GCAlternateUnit = '" + toUnitItem + "' AND IsDeleted = 0";
            Methods.getObjectValue('GetvItemAlternateItemUnitList', filterExpression, 'ConversionFactor', function (result) {
                var toConversion = getItemUnitName(toUnitItem);
                $('#<%=hdnConversionFactor.ClientID %>').val(result);
                var conversion = "1 " + toConversion + " = " + result + " " + baseText;
                $('#<%=txtConversion.ClientID %>').val(conversion);

                qtyAfter = parseFloat((qtyBefore / result * conversionBefore).toFixed(2));

                var qty = parseFloat(parseFloat($('#<%=hdnReceiveQtyBaseUnit.ClientID %>').val()) / result * conversionBefore).toFixed(2);
                $('#<%=txtQuantity.ClientID %>').attr('max', qty);
            });

            var conversion = parseFloat($('#<%=hdnConversionFactor.ClientID %>').val());
            var priceperitemunit = parseFloat(($('#<%=hdnUnitPrice.ClientID %>').val()));
            var pricePerPurchaseUnit = conversion * priceperitemunit;
            $('#<%=txtPrice.ClientID %>').val(pricePerPurchaseUnit).trigger('changeValue');

            $('#<%=txtQuantity.ClientID %>').val(qtyAfter);
            $('#<%=txtQuantity.ClientID %>').trigger('change');

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

        //#region Paging
        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                var totalOrder = parseFloat(param[2]);
                $('#<%=txtTransactionAmount.ClientID %>').val(totalOrder).trigger('changeValue');
                calculateTotal();
                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
        }
        //#endregion

        function onAfterSaveAddRecordEntryPopup(param) {
            onLoadObject(param);
        }

        function onAfterSaveRecordDtSuccess(PRID) {
            if ($('#<%=hdnPRID.ClientID %>').val() == '0') {
                $('#<%=hdnPRID.ClientID %>').val(PRID);
                var filterExpression = 'PurchaseReturnID = ' + PRID;
                Methods.getObject('GetPurchaseReturnHdList', filterExpression, function (result) {
                    $('#<%=txtReturnNo.ClientID %>').val(result.PurchaseReturnNo);
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
                    $('#lblAddData').trigger("click");
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
            $('#containerEntry').hide();
        }

        function onCboReasonValueChanged() {
            if (cboReason.GetValue() != "X162^999") {
                $('#trReason').attr('style', 'display:none');
            }
            else {
                $('#trReason').removeAttr('style');
            }
        }

        function onCboReturnTypeValueChanged(s) {
            if (s.GetValue() == '<%=GetPurchaseReturnCreditNote() %>') {
                $('#<%=chkIsAutoUpdateStock.ClientID %>').prop('checked', true);
                $('#<%=chkIsAutoUpdateStock.ClientID %>').removeAttr("disabled");
            }
            else {
                $('#<%=chkIsAutoUpdateStock.ClientID %>').prop('checked', true);
                $('#<%=chkIsAutoUpdateStock.ClientID %>').attr("disabled", true);
            }
        }

        function setRightPanelButtonEnabled() {
            var param = $('#<%:hdnPRID.ClientID %>').val();
            if (param != null && param != "0") {

                $('#btnReturnInvoice').removeAttr('enabled');
            } else {

                $('#btnReturnInvoice').attr('enabled', 'false');
            }
        }

        function onBeforeLoadRightPanelContent(code) {
            if (code == 'infoReturn') {
                var param = $('#<%:hdnPRID.ClientID %>').val();
                return param;
            }
        }

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var purchaseReturnID = $('#<%=hdnPRID.ClientID %>').val();
            var printStatus = $('#<%=hdnPrintStatus.ClientID %>').val();
            if (printStatus == 'true') {
                if (purchaseReturnID == '' || purchaseReturnID == '0') {
                    errMessage.text = 'Please Set Transaction First!';
                    return false;
                }
                else {
                    filterExpression.text = "PurchaseReturnID = " + purchaseReturnID;
                    return true;
                }
            } else {
                errMessage.text = "Data Doesn't Approved or Closed";
                return false;
            }
        }
        
    </script>
    <input type="hidden" value="" id="hdnIsDiscountAppliedToAveragePrice" runat="server" />
    <input type="hidden" value="" id="hdnIsPPNAppliedToAveragePrice" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnPrintStatus" runat="server" />
    <input type="hidden" value="0" id="hdnPRID" runat="server" />
    <input type="hidden" value="0" id="hdnPurchaseReceiveID" runat="server" />
    <input type="hidden" value="0" id="hdnPurchaseReceiveDtID" runat="server" />
    <input type="hidden" value="" id="hdnPageCount" runat="server" />
    <input type="hidden" value="0" id="hdnConfirm" runat="server" />
    <input type="hidden" value="0" id="hdnIsEdit" runat="server" />
    <input type="hidden" value="1" id="hdnIsEditable" runat="server" />
    <input type="hidden" value="" id="hdnVATPercentageFromSetvar" runat="server" />
    <input type="hidden" value="" id="hdnVATPercentage" runat="server" />
    <input type="hidden" value="0" id="hdnIsUsedProductLine" runat="server" />
    <input type="hidden" value="0" id="hdnIsTaxInvoiceMandatory" runat="server" />
    <input type="hidden" value="0" id="hdnIsSupplierIsPKP" runat="server" />
    <input type="hidden" value="0" id="hdnIsPpnAllowChanged" runat="server" />
    <input type="hidden" value="0" id="hdnIsCalculateHNA" runat="server" />
    <input type="hidden" value="0" id="hdnIM0128" runat="server" />
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
                                <label class="lblLink" id="lblReturnNo">
                                    <%=GetLabel("No. Retur")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtReturnNo" Width="150px" ReadOnly="true" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Tanggal") %>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPurchaseReturnDate" Width="120px" CssClass="datepicker" runat="server" />
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
                                            <asp:TextBox ID="txtSupplierCode" ReadOnly="true" Width="100%" runat="server" />
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
                                <label class="lblMandatory lblLink" id="lblPurchaseReceiveNo" runat="server">
                                    <%=GetLabel("No. BPB")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPurchaseReceiveNo" Width="150px" ReadOnly="true" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="padding: 5px; vertical-align: top">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 160px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory" runat="server" id="lblLocation">
                                    <%=GetLabel("Dari Lokasi")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnLocationID" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 120px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtLocationCode" Width="100%" ReadOnly="true" runat="server" />
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
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Penggantian")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboReturnType" ClientInstanceName="cboReturnType" Width="100%"
                                    runat="server">
                                    <ClientSideEvents ValueChanged="function(s,e){ onCboReturnTypeValueChanged(s); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("No Faktur/Kirim (POR)") %>
                            </td>
                            <td>
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 120px" />
                                        <col style="width: 3px" />
                                        <col style="width: 130px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtReferenceNo" Width="120px" runat="server" ReadOnly="true" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td class="tdLabel">
                                            <%=GetLabel("Tgl Faktur/Kirim (POR)")%>
                                        </td>
                                        <td>
                                            <div style="text-align: left">
                                                <asp:TextBox ID="txtReferenceDate" Width="120px" runat="server" ReadOnly="true" Style="text-align: center" /></div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="trTaxInvoice" runat="server">
                            <td class="tdLabel">
                                <%=GetLabel("No Faktur Pajak") %>
                            </td>
                            <td>
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 120px" />
                                        <col style="width: 3px" />
                                        <col style="width: 130px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtTaxInvoiceNo" Width="120px" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td class="tdLabel">
                                            <%=GetLabel("Tgl Faktur Pajak")%>
                                        </td>
                                        <td>
                                            <div style="text-align: left">
                                                <asp:TextBox ID="txtTaxInvoiceDate" Width="120px" CssClass="datepicker" runat="server" /></div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkPPN" Width="100%" runat="server" Text="PPN" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkIsAutoUpdateStock" Width="100%" runat="server" Text="Otomatis Mengurangi Stok" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div id="containerEntry" style="margin-top: 4px; display: none;">
                        <div class="pageTitle">
                            <%=GetLabel("Edit atau Tambah Item Retur Pembelian")%></div>
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
                                                <col style="width: 150px" />
                                            </colgroup>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblLink lblMandatory" id="lblItem">
                                                        <%=GetLabel("Item")%></label>
                                                </td>
                                                <td colspan="2">
                                                    <input type="hidden" value="" id="hdnItemID" runat="server" />
                                                    <input type="hidden" value="" id="hdnGCBaseUnit" runat="server" />
                                                    <input type="hidden" value="" id="hdnGCItemUnit" runat="server" />
                                                    <input type="hidden" value="" id="hdnReceiveQtyBaseUnit" runat="server" />
                                                    <input type="hidden" value="" id="hdnConversionFactor" runat="server" />
                                                    <input type="hidden" value="" id="hdnConversionFactorORI" runat="server" />
                                                    <input type="hidden" value="" id="hdnUnitPrice" runat="server" />
                                                    <table cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width: 150px" />
                                                            <col style="width: 3px" />
                                                            <col style="width: 250px" />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtItemCode" ReadOnly="true" Width="100%" runat="server" />
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
                                                                <asp:TextBox ID="txtReceivedQty" ReadOnly="true" Width="120px" CssClass="number"
                                                                    runat="server" />
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtReceivedUnit" ReadOnly="true" Width="150px" runat="server" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("Jumlah Diretur")%></label>
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
                                                                <input type="hidden" value="0" id="hdnQtyORI" runat="server" />
                                                                <asp:TextBox ID="txtQuantity" Width="120px" CssClass="number max" runat="server" />
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <dxe:ASPxComboBox runat="server" ID="cboItemUnit" ClientInstanceName="cboItemUnit"
                                                                    Width="150px" OnCallback="cboItemUnit_Callback">
                                                                    <ClientSideEvents EndCallback="function(s,e){ onCboItemUnitEndCallBack(); }" ValueChanged="function(s,e){ onCboItemUnitChanged(); }" />
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
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("Alasan Retur")%></label>
                                                </td>
                                                <td>
                                                    <dxe:ASPxComboBox ID="cboReason" ClientInstanceName="cboReason" Width="300px" runat="server">
                                                        <ClientSideEvents ValueChanged="function(s,e){ onCboReasonValueChanged(); }" />
                                                    </dxe:ASPxComboBox>
                                                </td>
                                            </tr>
                                            <tr id="trReason" style="display: none">
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Alasan")%></label>
                                                </td>
                                                <td colspan="2">
                                                    <asp:TextBox ID="txtReason" runat="server" Width="300px" TextMode="MultiLine" Rows="2" />
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
                                                                <asp:TextBox ID="txtPrice" CssClass="txtCurrency" Width="100%" runat="server" ReadOnly="true" />
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
                                                                <asp:CheckBox ID="chkIsDiscountInPercentage1" Width="100%" runat="server" Enabled="false" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtDiscount" CssClass="number" Width="100%" runat="server" ReadOnly="true" />
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <input type="hidden" value="0" id="hdnDiscountAmount1" runat="server" />
                                                                <asp:TextBox ID="txtDiscountAmount" CssClass="txtCurrency" Width="100%" runat="server"
                                                                    ReadOnly="true" />
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
                                                                <asp:CheckBox ID="chkIsDiscountInPercentage2" Width="100%" runat="server" Enabled="false" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtDiscount2" CssClass="number" Width="100%" runat="server" ReadOnly="true" />
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <input type="hidden" value="0" id="hdnDiscountAmount2" runat="server" />
                                                                <asp:TextBox ID="txtDiscountAmount2" CssClass="txtCurrency" Width="100%" runat="server"
                                                                    ReadOnly="true" />
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
                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <img class="imgEdit <%#: Eval("IsAllowEditItem").ToString() == "False" ? "imgDisabled" : "imgLink"%>"
                                                                    title='<%=GetLabel("Edit")%>' src='<%# Eval("IsAllowEditItem").ToString() == "False" ? ResolveUrl("~/Libs/Images/Button/edit_disabled.png") : ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                    alt="" />
                                                            </td>
                                                            <td style="width: 1px">
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <img class="imgDelete <%#: Eval("IsAllowEditItem").ToString() == "False" ? "imgDisabled" : "imgLink"%>"
                                                                    title='<%=GetLabel("Delete")%>' src='<%# Eval("IsAllowEditItem").ToString() == "False" ? ResolveUrl("~/Libs/Images/Button/delete_disabled.png") : ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                    alt="" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                    <input type="hidden" value="<%#:Eval("PurchaseReceiveDtID") %>" bindingfield="PurchaseReceiveDtID" />
                                                    <input type="hidden" value="<%#:Eval("ItemID") %>" bindingfield="ItemID" />
                                                    <input type="hidden" value="<%#:Eval("ItemCode") %>" bindingfield="ItemCode" />
                                                    <input type="hidden" value="<%#:Eval("ItemName1") %>" bindingfield="ItemName1" />
                                                    <input type="hidden" value="<%#:Eval("ItemGroupID") %>" bindingfield="ItemGroupID" />
                                                    <input type="hidden" value="<%#:Eval("ItemGroupCode") %>" bindingfield="ItemGroupCode" />
                                                    <input type="hidden" value="<%#:Eval("ItemGroupName1") %>" bindingfield="ItemGroupName1" />
                                                    <input type="hidden" value="<%#:Eval("ReceivedQuantity") %>" bindingfield="ReceivedQuantity" />
                                                    <input type="hidden" value="<%#:Eval("ReceiveForReturnQuantity") %>" bindingfield="ReceiveForReturnQuantity" />
                                                    <input type="hidden" value="<%#:Eval("ReceivedItemUnit") %>" bindingfield="ReceivedItemUnit" />
                                                    <input type="hidden" value="<%#:Eval("ReceivedConversionFactor") %>" bindingfield="ReceivedConversionFactor" />
                                                    <input type="hidden" value="<%#:Eval("Quantity") %>" bindingfield="Quantity" />
                                                    <input type="hidden" value="<%#:Eval("GCItemUnit") %>" bindingfield="GCItemUnit" />
                                                    <input type="hidden" value="<%#:Eval("GCBaseUnit") %>" bindingfield="GCBaseUnit" />
                                                    <input type="hidden" value="<%#:Eval("ItemUnit") %>" bindingfield="ItemUnit" />
                                                    <input type="hidden" value="<%#:Eval("BaseUnit") %>" bindingfield="BaseUnit" />
                                                    <input type="hidden" value="<%#:Eval("ConversionFactor") %>" bindingfield="ConversionFactor" />
                                                    <input type="hidden" value="<%#:Eval("UnitPrice") %>" bindingfield="UnitPrice" />
                                                    <input type="hidden" value="<%#:Eval("IsDiscountInPercentage1") %>" bindingfield="IsDiscountInPercentage1" />
                                                    <input type="hidden" value="<%#:Eval("DiscountPercentage1") %>" bindingfield="DiscountPercentage1" />
                                                    <input type="hidden" value="<%#:Eval("DiscountAmount1") %>" bindingfield="DiscountAmount1" />
                                                    <input type="hidden" value="<%#:Eval("IsDiscountInPercentage2") %>" bindingfield="IsDiscountInPercentage2" />
                                                    <input type="hidden" value="<%#:Eval("DiscountPercentage2") %>" bindingfield="DiscountPercentage2" />
                                                    <input type="hidden" value="<%#:Eval("DiscountAmount2") %>" bindingfield="DiscountAmount2" />
                                                    <input type="hidden" value="<%#:Eval("GCPurchaseReturnReason") %>" bindingfield="GCPurchaseReturnReason" />
                                                    <input type="hidden" value="<%#:Eval("PurchaseReturnReason") %>" bindingfield="PurchaseReturnReason" />
                                                    <input type="hidden" value="<%#:Eval("CustomSubTotal") %>" bindingfield="CustomSubTotal" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ItemCode" HeaderText="Kode Item" HeaderStyle-Width="100px" />
                                            <asp:BoundField DataField="ItemName1" HeaderText="Nama Item" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="Quantity" HeaderText="Qty" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="ItemUnit" HeaderText="Satuan" HeaderStyle-Width="120px"
                                                HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="PurchaseReturnReason" HeaderText="Alasan Retur" HeaderStyle-Width="180px"
                                                HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="CustomUnitPrice" HeaderText="Harga / Satuan" ItemStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="CustomTotalDiscount" HeaderText="Total Discount" HeaderStyle-Width="100px"
                                                ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N}" HeaderStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="CustomSubTotal" HeaderText="SubTotal" ItemStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="100px" DataFormatString="{0:N}" HeaderStyle-HorizontalAlign="Right" />
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
                        <span class="lblLink" id="lblAddData">
                            <%= GetLabel("Tambah Barang")%></span>
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
                                                    <asp:TextBox ID="txtNotes" Width="100%" runat="server" TextMode="MultiLine" Rows="3" />
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
                                                <col style="width: 200px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Jumlah Nilai Retur")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtTransactionAmount" CssClass="txtCurrency" ReadOnly="true" Width="180px"
                                                        runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("PPN")%>
                                                        <asp:TextBox ID="txtVATPercentageDefault" CssClass="txtCurrency" ReadOnly="true"
                                                            Width="40px" runat="server" />
                                                        %</label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPPN" CssClass="txtCurrency" ReadOnly="true" Width="180px" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Return Cost")%>
                                                    </label>
                                                    <asp:CheckBox ID="chkIsReturnCostInPct" runat="server" />
                                                    <asp:TextBox ID="txtReturnCostPct" CssClass="txtCurrency" Width="40px" runat="server" />
                                                    <label>
                                                        %</label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtReturnCostAmount" CssClass="txtCurrency" Width="180px" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Total Nilai Retur")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtGrandTotal" CssClass="txtCurrency" ReadOnly="true" Width="180px"
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
