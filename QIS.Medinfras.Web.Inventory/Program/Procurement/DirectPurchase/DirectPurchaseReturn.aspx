<%@ Page Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx2.master" AutoEventWireup="true"
    CodeBehind="DirectPurchaseReturn.aspx.cs" Inherits="QIS.Medinfras.Web.Inventory.Program.DirectPurchaseReturn" %>

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
    <li id="btnDirectPurchaseReturnReopen" runat="server" crudmode="R">
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
                $('#<%=btnDirectPurchaseReturnReopen.ClientID %>').show();
            }
            else {
                $('#<%=btnDirectPurchaseReturnReopen.ClientID %>').hide();
            }
        }

        $('#<%=btnDirectPurchaseReturnReopen.ClientID %>').live('click', function (evt) {
            if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                onCustomButtonClick('reopen');
            }
        });

        function onAfterCustomClickSuccess(type, retval) {
            onLoadObject(retval);
        }

        function onLoad() {
            $('#<%=btnDirectPurchaseReturnReopen.ClientID %>').hide();

            $('#<%=txtFinalDiscountInPercentage.ClientID %>').change(function () {
                $(this).trigger('changeValue');
                calculateFinalDiscount("fromPctg");
            });

            $('#<%=txtFinalDiscount.ClientID %>').change(function () {
                $(this).trigger('changeValue');
                calculateFinalDiscount("fromTxt");
            });

            if ($('#<%=hdnIsEditable.ClientID %>').val() == '1')
                $('#lblAddData').show();
            else
                $('#lblAddData').hide();

            setRightPanelButtonEnabled();

            if ($('#<%=hdnPRID.ClientID %>').val() == "" || $('#<%=hdnPRID.ClientID %>').val() == "0") {
                setDatePicker('<%=txtPurchaseReturnDate.ClientID %>');
            }
            $('#<%=txtPurchaseReturnDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

            //#region Direct Purchase Return No
            $('#lblReturnNo.lblLink').click(function () {
                openSearchDialog('directpurchasereturnhd', '', function (value) {
                    $('#<%=txtDirectPurchaseReturnNo.ClientID %>').val(value);
                    onTxtPurchaseReturnNoChanged(value);
                });
            });

            $('#<%=txtDirectPurchaseReturnNo.ClientID %>').change(function () {
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
                        $('#<%=hdnDirectPurchaseID.ClientID %>').val('0');
                        $('#<%=txtDirectPurchaseNo.ClientID %>').val('');
                    }
                    else {
                        $('#<%=hdnSupplierID.ClientID %>').val('');
                        $('#<%=txtSupplierCode.ClientID %>').val('');
                        $('#<%=txtSupplierName.ClientID %>').val('');
                        $('#<%=hdnDirectPurchaseID.ClientID %>').val('0');
                        $('#<%=txtDirectPurchaseNo.ClientID %>').val('');
                    }
                });
                setRightPanelButtonEnabled();
            }
            //#endregion

            //#region Direct Purchase No
            function getDirectPurchaseFilterExpression() {
                var filterExpression = ""; ;
                if ($('#<%=hdnSupplierID.ClientID %>').val() != "") {
                    filterExpression += "BusinessPartnerID = " + $('#<%=hdnSupplierID.ClientID %>').val() + " AND ";
                }
                filterExpression += "GCTransactionStatus = 'X121^003'";
                //                filterExpression += "GCTransactionStatus = 'X121^003' AND DirectPurchaseID NOT IN (SELECT DirectPurchaseID FROM DirectPurchaseReturnHd WHERE GCTransactionStatus != 'X121^999')";
                return filterExpression;
            }

            $('#<%=lblDirectPurchaseNo.ClientID %>.lblLink').live('click', function () {
                openSearchDialog('directpurchase', getDirectPurchaseFilterExpression(), function (value) {
                    $('#<%=txtDirectPurchaseNo.ClientID %>').val(value);
                    ontxtDirectPurchaseNoChanged(value);
                });
            });

            $('#<%=txtDirectPurchaseNo.ClientID %>').change(function () {
                ontxtDirectPurchaseNoChanged($(this).val());
            });

            function ontxtDirectPurchaseNoChanged(value) {
                var isEditablePPN = $('#<%=hdnIsPpnAllowChanged.ClientID %>').val();
                var filterExpression = "DirectPurchaseNo = '" + value + "'";
                Methods.getObject('GetvDirectPurchaseHdList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnDirectPurchaseID.ClientID %>').val(result.DirectPurchaseID);
                        $('#<%=txtDirectPurchaseNo.ClientID %>').val(result.DirectPurchaseNo);
                        $('#<%=hdnLocationID.ClientID %>').val(result.LocationID);
                        $('#<%=txtLocationCode.ClientID %>').val(result.LocationCode);
                        $('#<%=txtLocationName.ClientID %>').val(result.LocationName);
                        $('#<%=hdnSupplierID.ClientID %>').val(result.BusinessPartnerID);
                        $('#<%=txtSupplierCode.ClientID %>').val(result.BusinessPartnerCode);
                        $('#<%=chkPPN.ClientID %>').prop('checked', result.IsIncludeVAT);
                        if (result.IsIncludeVAT) {
                            if (isEditablePPN == '1') {
                                $('#<%:txtVATPercentageDefault.ClientID %>').removeAttr('readonly');
                            }
                            else {
                                $('#<%:txtVATPercentageDefault.ClientID %>').attr('readonly', 'readonly');
                            }
                        }
                        else {
                            $('#<%:txtVATPercentageDefault.ClientID %>').attr('readonly', 'readonly');
                        }

                        $('#<%=txtSupplierName.ClientID %>').val(result.BusinessPartnerName);
                        $('#<%=txtReferenceNo.ClientID %>').val(result.ReferenceNo);
                        if (Methods.getJSONDateValue(result.ReferenceDate) != "01-01-1900")
                            $('#<%=txtReferenceDate.ClientID %>').val(Methods.getJSONDateValue(result.ReferenceDate));
                        else
                            $('#<%=txtReferenceDate.ClientID %>').val('');
                    }
                    else {
                        $('#<%=hdnDirectPurchaseID.ClientID %>').val('0');
                        $('#<%=txtDirectPurchaseNo.ClientID %>').val('');
                        $('#<%=hdnLocationID.ClientID %>').val('');
                        $('#<%=txtLocationCode.ClientID %>').val('');
                        $('#<%=txtLocationName.ClientID %>').val('');
                        $('#<%=hdnSupplierID.ClientID %>').val('');
                        $('#<%=txtSupplierCode.ClientID %>').val('');
                        $('#<%=txtSupplierName.ClientID %>').val('');
                        $('#<%=txtReferenceNo.ClientID %>').val('');
                        $('#<%=txtReferenceDate.ClientID %>').val('');
                        $('#<%=chkPPN.ClientID %>').prop('checked', false);
                        $('#<%:txtVATPercentageDefault.ClientID %>').attr('readonly', 'readonly');
                    }
                });
                setRightPanelButtonEnabled();
            }
            //#endregion

            $('#<%=txtQuantity.ClientID %>').die('change');
            $('#<%=txtQuantity.ClientID %>').live('change', function () {
                var unitPrice = parseFloat($('#<%=txtPrice.ClientID %>').val().replace('.00', '').split(',').join(''));
                var qtyORI = parseFloat($('#<%=hdnQtyORI.ClientID %>').val());
                var convFactor = parseFloat($('#<%=hdnConversionFactor.ClientID %>').val());
                var convFactorORI = parseFloat($('#<%=hdnConversionFactorORI.ClientID %>').val());
                var inputqty = parseFloat($('#<%=txtQuantity.ClientID %>').val());
                var qtyMax = parseFloat($('#<%=hdnQtyMax.ClientID %>').val());
                if (inputqty > 0 && inputqty <= qtyMax) {
                    var discPct1 = parseFloat($('#<%=txtDiscount.ClientID %>').val());
                    var discAmount1 = parseFloat($('#<%=txtDiscountAmount.ClientID %>').val().replace('.00', '').split(',').join(''));

                    if ($('#<%=chkIsDiscountInPercentage1.ClientID %>').is(':checked')) {
                        discAmount1 = (unitPrice * inputqty) * discPct1 / 100;
                    } else {
                        discAmount1 = (discAmount1 / (qtyORI * convFactorORI / convFactor)) * inputqty;
                    }
                    $('#<%=txtDiscountAmount.ClientID %>').val(discAmount1).trigger('changeValue');

                    var discPct2 = parseFloat($('#<%=txtDiscount2.ClientID %>').val());
                    var discAmount2 = parseFloat($('#<%=txtDiscountAmount2.ClientID %>').val().replace('.00', '').split(',').join(''));

                    if ($('#<%=chkIsDiscountInPercentage2.ClientID %>').is(':checked')) {
                        discAmount2 = ((unitPrice * inputqty) - discAmount1) * discPct2 / 100;
                    } else {
                        discAmount2 = (discAmount2 / (qtyORI * convFactorORI / convFactor)) * inputqty;
                    }
                    $('#<%=txtDiscountAmount2.ClientID %>').val(discAmount2).trigger('changeValue');

                    $('#<%=hdnQtyORI.ClientID %>').val($('#<%=txtQuantity.ClientID %>').val());
                } else {
                    $('#<%=txtQuantity.ClientID %>').val('1');
                    $('#<%=hdnQtyORI.ClientID %>').val($('#<%=txtQuantity.ClientID %>').val());
                    var errMessageQty = "Quantity harus lebih besar dari 0 dan maksimal berjumlah " + qtyMax + " !";
                    showToast('Input Failed', errMessageQty);
                }

                calculateSubTotal();
            });

            $('#<%=txtPrice.ClientID %>').die('change');
            $('#<%=txtPrice.ClientID %>').live('change', function () {
                $(this).blur();
                calculateSubTotal();
            });

            $('#<%=txtDiscount.ClientID %>').die('change');
            $('#<%=txtDiscount.ClientID %>').live('change', function () {
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

                $('#<%=txtVATPercentageDefault.ClientID %>').trigger('change');
            });

            $('#<%=txtVATPercentageDefault.ClientID %>').die('change');
            $('#<%=txtVATPercentageDefault.ClientID %>').live('change', function () {
                $('#<%:hdnVATPercentage.ClientID %>').val($('#<%=txtVATPercentageDefault.ClientID %>').val());
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

                var subTotal = parseFloat($('#<%=txtTotalRetur.ClientID %>').attr('hiddenVal'));
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
                var subTotal = parseFloat($('#<%=txtTotalRetur.ClientID %>').attr('hiddenVal').replace('.00', '').split(',').join(''));
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
                var subTotal = parseFloat($('#<%=txtTotalRetur.ClientID %>').attr('hiddenVal').replace('.00', '').split(',').join(''));
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

            $('.txtCurrency').each(function () {
                $(this).trigger('changeValue');
            });

            $('#btnCopyDirectPurchase').click(function () {
                if ($(this).attr('enabled') == null) {
                    var param = $('#<%=hdnDirectPurchaseID.ClientID %>').val() + '|' + $('#<%=hdnPRID.ClientID %>').val();
                    var url = ResolveUrl("~/Program/Procurement/DirectPurchase/DirectPurchaseReturnEntryCtl.ascx");
                    openUserControlPopup(url, param, 'Salin Pembelian Tunai', 1200, 550);
                }
            });

            var pageCount = parseInt($('#<%=hdnPageCount.ClientID %>').val());
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });

            $('#<%=txtFinalDiscountInPercentage.ClientID %>').change();
        }

        //#region Item
        function getItemFilterExpression() {
            var filterExpression = "(Quantity - ReturnedQuantity) > 0 AND GCItemDetailStatus = 'X121^003'";
            var returnID = $('#<%=hdnPRID.ClientID %>').val();
            var purchaseID = $('#<%=hdnDirectPurchaseID.ClientID %>').val();

            if (purchaseID != "0") {
                if (filterExpression != "") {
                    filterExpression += " AND ";
                }
                filterExpression += "DirectPurchaseID = " + purchaseID;
            }

            if (returnID != "0") {
                if (filterExpression != "") {
                    filterExpression += " AND ";
                }
                filterExpression += "ItemID NOT IN (SELECT ItemID FROM DirectPurchaseReturnDt WITH(NOLOCK) WHERE DirectPurchaseReturnID = " + returnID + " AND GCItemDetailStatus != '<%=GetTransactionStatusVoid() %>')";
            }

            return filterExpression;
        }

        $('#lblItem.lblLink').live('click', function () {
            openSearchDialog('directpurchasedt', getItemFilterExpression(), function (value) {
                onTxtItemCodeChanged(value);
            });
        });

        function onTxtItemCodeChanged(value) {
            var filterExpression = "DirectPurchaseID = " + $('#<%=hdnDirectPurchaseID.ClientID %>').val() + " AND ID = '" + value + "' AND GCItemDetailStatus = 'X121^003'";
            Methods.getObject('GetvDirectPurchaseDtList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnDirectPurchaseDtID.ClientID %>').val(result.ID);
                    $('#<%=hdnItemID.ClientID %>').val(result.ItemID);
                    $('#<%=txtItemCode.ClientID %>').val(result.ItemCode);
                    $('#<%=txtItemName.ClientID %>').val(result.ItemName1);
                    $('#<%=hdnGCBaseUnit.ClientID %>').val(result.GCBaseUnit);
                    $('#<%=hdnGCItemUnit.ClientID %>').val(result.GCItemUnit);

                    var receiveQtyBaseUnit = parseFloat(result.QuantityForReturn) * parseFloat(result.ConversionFactor);
                    $('#<%=hdnReceiveQtyBaseUnit.ClientID %>').val(receiveQtyBaseUnit);

                    var pricePerBaseUnit = parseFloat(result.UnitPrice) / parseFloat(result.ConversionFactor);
                    $('#<%=hdnUnitPrice.ClientID %>').val(pricePerBaseUnit);
                    $('#<%=txtPrice.ClientID %>').val(result.UnitPrice).trigger('changeValue');

                    if (result.IsDiscountInPercentage.toString() == "true") {
                        $('#<%=chkIsDiscountInPercentage1.ClientID %>').prop('checked', true);
                    } else {
                        $('#<%=chkIsDiscountInPercentage1.ClientID %>').prop('checked', false);
                    }
                    $('#<%=txtDiscount.ClientID %>').val(result.DiscountPercentage).trigger('changeValue');
                    var discAmount1 = parseFloat(result.DiscountAmount / result.Quantity).toFixed(2);
                    $('#<%=txtDiscountAmount.ClientID %>').val(discAmount1).trigger('changeValue');

                    if (result.IsDiscountInPercentage2.toString() == "true") {
                        $('#<%=chkIsDiscountInPercentage2.ClientID %>').prop('checked', true);
                    } else {
                        $('#<%=chkIsDiscountInPercentage2.ClientID %>').prop('checked', false);
                    }
                    $('#<%=txtDiscount2.ClientID %>').val(result.DiscountPercentage2).trigger('changeValue');
                    var discAmount2 = parseFloat(result.DiscountAmount2 / result.Quantity).toFixed(2);
                    $('#<%=txtDiscountAmount2.ClientID %>').val(discAmount2).trigger('changeValue');


                    $('#<%=txtReceivedQty.ClientID %>').val(result.Quantity);
                    $('#<%=txtReceivedUnit.ClientID %>').val(result.ItemUnit);

                    $('#<%=txtQuantity.ClientID %>').attr('min', '0');
                    $('#<%=txtQuantity.ClientID %>').attr('max', result.QuantityForReturn);
                    $('#<%=hdnQtyMax.ClientID %>').val(result.QuantityForReturn);

                    $('#<%=hdnConversionFactor.ClientID %>').val(result.ConversionFactor);
                    $('#<%=hdnConversionFactorORI.ClientID %>').val($('#<%=hdnConversionFactor.ClientID %>').val());
                    $('#<%=txtQuantity.ClientID %>').val('1');
                    $('#<%=hdnQtyORI.ClientID %>').val($('#<%=txtQuantity.ClientID %>').val());

                    cboItemUnit.PerformCallback();
                }
                else {
                    $('#<%=hdnDirectPurchaseDtID.ClientID %>').val('');
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
                    $('#<%=hdnQtyMax.ClientID %>').val(1);

                    $('#<%=txtQuantity.ClientID %>').val('1');
                    $('#<%=hdnQtyORI.ClientID %>').val($('#<%=txtQuantity.ClientID %>').val());
                }
            });
        }
        //#endregion

        $('#lblAddData').live('click', function (evt) {
            if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                $('#<%=hdnDirectPurchaseDtID.ClientID %>').val('');
                $('#<%=txtQuantity.ClientID %>').val('1.00');
                $('#<%=hdnQtyORI.ClientID %>').val($('#<%=txtQuantity.ClientID %>').val());
                $('#<%=hdnEntryID.ClientID %>').val('');
                $('#<%=hdnItemID.ClientID %>').val('');
                $('#<%=hdnGCItemUnit.ClientID %>').val('');
                $('#<%=hdnGCBaseUnit.ClientID %>').val('');
                $('#<%=txtItemCode.ClientID %>').val('');
                $('#<%=txtItemName.ClientID %>').val('');
                $('#<%=txtQuantity.ClientID %>').val('1');
                $('#<%=hdnQtyORI.ClientID %>').val($('#<%=txtQuantity.ClientID %>').val());
                $('#<%=hdnConversionFactor.ClientID %>').val('1');
                $('#<%=hdnConversionFactorORI.ClientID %>').val($('#<%=hdnConversionFactor.ClientID %>').val());
                $('#<%=txtPrice.ClientID %>').val('');

                $('#<%=chkIsDiscountInPercentage1.ClientID %>').prop('checked', false);
                $('#<%=txtDiscount.ClientID %>').val('0');
                $('#<%=txtDiscountAmount.ClientID %>').val('0');
                $('#<%=chkIsDiscountInPercentage2.ClientID %>').prop('checked', false);
                $('#<%=txtDiscount2.ClientID %>').val('0');
                $('#<%=txtDiscountAmount2.ClientID %>').val('0');

                $('#<%=txtBaseUnit.ClientID %>').val('');
                $('#<%=txtReceivedQty.ClientID %>').val('');
                $('#<%=txtReceivedUnit.ClientID %>').val('');
                $('#<%=txtConversion.ClientID %>').val('');
                $('#<%=txtSubTotalPrice.ClientID %>').val('0').trigger('changeValue');
                $('#<%=lblSupplier.ClientID %>').attr('class', 'lblDisabled');
                $('#<%=txtSupplierCode.ClientID %>').attr('readonly', 'readonly');
                $('#<%=lblDirectPurchaseNo.ClientID %>').attr('class', 'lblDisabled');
                $('#<%=txtDirectPurchaseNo.ClientID %>').attr('readonly', 'readonly');
                lastTransactionAmount = parseFloat($('#<%=txtTotalRetur.ClientID %>').attr('hiddenVal').replace('.00', '').split(',').join(''));
                editedLineAmount = 0;
                cboItemUnit.SetValue('');
                cboReason.SetValue('');

                $('#containerEntry').show();
            }
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

        $('#<%=grdView.ClientID %> .imgEdit.imgLink').live('click', function () {
            $row = $(this).closest('tr').parent().closest('tr');
            var entity = rowToObject($row);
            $('#<%=hdnDirectPurchaseDtID.ClientID %>').val(entity.DirectPurchaseDtID);
            $('#<%=hdnEntryID.ClientID %>').val(entity.ID);
            $('#<%=hdnGCBaseUnit.ClientID %>').val(entity.GCBaseUnit);
            $('#<%=hdnGCItemUnit.ClientID %>').val(entity.GCItemUnit);
            $('#<%=hdnItemID.ClientID %>').val(entity.ItemID);
            $('#<%=txtItemCode.ClientID %>').val(entity.ItemCode);
            $('#<%=txtItemName.ClientID %>').val(entity.ItemName1);
            $('#<%=txtQuantity.ClientID %>').val(entity.Quantity);
            $('#<%=hdnQtyORI.ClientID %>').val($('#<%=txtQuantity.ClientID %>').val());
            $('#<%=hdnConversionFactor.ClientID %>').val(entity.ConversionFactor);
            $('#<%=hdnConversionFactorORI.ClientID %>').val($('#<%=hdnConversionFactor.ClientID %>').val());
            if (entity.IsDiscountInPercentage1.toString() == "True") {
                $('#<%=chkIsDiscountInPercentage1.ClientID %>').prop('checked', true);
            } else {
                $('#<%=chkIsDiscountInPercentage1.ClientID %>').prop('checked', false);
            }
            $('#<%=txtDiscount.ClientID %>').val(entity.DiscountPercentage1);
            $('#<%=txtDiscountAmount.ClientID %>').val(entity.DiscountAmount1).trigger('changeValue');
            if (entity.IsDiscountInPercentage2.toString() == "True") {
                $('#<%=chkIsDiscountInPercentage1.ClientID %>').prop('checked', true);
            } else {
                $('#<%=chkIsDiscountInPercentage1.ClientID %>').prop('checked', false);
            }
            $('#<%=txtDiscount2.ClientID %>').val(entity.DiscountPercentage2);
            $('#<%=txtDiscountAmount2.ClientID %>').val(entity.DiscountAmount2).trigger('changeValue');
            $('#<%=txtReceivedQty.ClientID %>').val(entity.ReceivedQuantity);
            $('#<%=txtReceivedUnit.ClientID %>').val(entity.ReceivedItemUnit);
            var receiveQtyBaseUnit = parseFloat(entity.ReceivedQuantity) * parseFloat(entity.ConversionFactor);
            $('#<%=hdnReceiveQtyBaseUnit.ClientID %>').val(receiveQtyBaseUnit);

            var pricePerBaseUnit = parseFloat(entity.UnitPrice) / parseFloat(entity.ConversionFactor);
            $('#<%=hdnUnitPrice.ClientID %>').val(pricePerBaseUnit);
            cboReason.SetValue(entity.GCPurchaseReturnReason);
            $('#<%=txtReason.ClientID %>').val(entity.PurchaseReturnReason);

            lastTransactionAmount = parseFloat($('#<%=txtTotalRetur.ClientID %>').attr('hiddenVal'));
            editedLineAmount = entity.CustomSubTotal;
            cboItemUnit.PerformCallback();
            $('#containerEntry').show();
        });

        $('#btnCancel').live('click', function () {
            $('#containerEntry').hide();
        });

        $('#btnSave').live('click', function (evt) {
            if (IsValid(evt, 'fsTrxPopup', 'mpTrxPopup'))
                cbpProcess.PerformCallback('save');
        });

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
                $('#<%=hdnQtyMax.ClientID %>').val(qty);
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

        function calculateSubTotal() {
            var price = parseFloat($('#<%=txtPrice.ClientID %>').attr('hiddenVal').replace('.00', '').split(',').join(''));
            var qty = parseFloat($('#<%=txtQuantity.ClientID %>').val());
            var subTotal = price * qty;
            var discount1 = parseFloat($('#<%=txtDiscountAmount.ClientID %>').val().replace('.00', '').split(',').join(''));
            var discount2 = parseFloat($('#<%=txtDiscountAmount2.ClientID %>').val().replace('.00', '').split(',').join(''));

            subTotal = subTotal - discount1 - discount2;

            $('#<%=txtSubTotalPrice.ClientID %>').val(subTotal).trigger('changeValue');

            var totalOrder = lastTransactionAmount - editedLineAmount + subTotal;
            $('#<%=txtTotalRetur.ClientID %>').val(totalOrder).trigger('changeValue');

            calculateTotal();
        }

        function calculateTotal() {
            var subTotal = parseFloat($('#<%=txtTotalRetur.ClientID %>').attr('hiddenVal').replace('.00', '').split(',').join(''));

            if ($('#<%=chkPPN.ClientID %>').is(':checked')) {
                var temp = subTotal;

                var PPN = parseFloat($('#<%=hdnVATPercentage.ClientID %>').val()) / 100 * temp;

                $('#<%=txtPPN.ClientID %>').val(PPN).trigger('changeValue');
            }
            else {
                $('#<%=txtPPN.ClientID %>').val('0').trigger('changeValue');
            }

            var PPN = parseFloat($('#<%=txtPPN.ClientID %>').attr('hiddenVal'));

            var costAmt = parseFloat($('#<%=txtReturnCostAmount.ClientID %>').val().replace('.00', '').split(',').join(''));

            var grandTotal = subTotal + PPN + costAmt;

            $('#<%=txtGrandTotal.ClientID %>').val(grandTotal).trigger('changeValue');
        }

        function calculateFinalDiscount(kode) {
            var totalTrans = parseFloat($('#<%=txtTotalRetur.ClientID %>').attr('hiddenVal'));
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

        //#region Paging
        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                var totalPurchase = parseFloat(param[2]);
                var returnCostAmount = parseFloat(param[3]);
                $('#<%=txtTotalRetur.ClientID %>').val(totalPurchase).trigger('changeValue');
                $('#<%=txtReturnCostAmount.ClientID %>').val(returnCostAmount).trigger('changeValue');
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
                var filterExpression = 'DirectPurchaseReturnID = ' + PRID;
                Methods.getObject('GetDirectPurchaseReturnHdList', filterExpression, function (result) {
                    $('#<%=txtDirectPurchaseReturnNo.ClientID %>').val(result.DirectPurchaseReturnNo);

                    $('#<%=btnDirectPurchaseReturnReopen.ClientID %>').hide();
                });
                onAfterCustomSaveSuccess();
                onLoadObject($('#<%=txtDirectPurchaseReturnNo.ClientID %>').val());
            }
        }

        function onCbpProcesEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'save') {
                if (param[1] == 'fail') {
                    showToast('Save Failed', 'Error Message : ' + param[2]);
                    onTxtItemCodeChanged($('#<%=hdnDirectPurchaseDtID.ClientID %>').val());
                }
                else {
                    var PRID = s.cpPurchaseReturnID;
                    onAfterSaveRecordDtSuccess(PRID);
                    $('#containerEntry').hide();
                    cbpView.PerformCallback('refresh');
                }
            }
            else if (param[0] == 'delete') {
                if (param[1] == 'fail')
                    showToast('Delete Failed', 'Error Message : ' + param[2]);
                else
                    cbpView.PerformCallback('refresh');
                $('#containerEntry').hide();
            }
        }

        function onCboReasonValueChanged() {
            if (cboReason.GetValue() != "X162^999") {
                $('#trReason').attr('style', 'display:none');
            }
            else $('#trReason').removeAttr('style');
        }

        function onCboReturnTypeValueChanged() {
            setRightPanelButtonEnabled();
        }

        function setRightPanelButtonEnabled() {
            var isEnabled = false;
            if (cboReturnType.GetValue() != null && $('#<%=txtDirectPurchaseNo.ClientID %>').val() != '' && $('#<%=txtSupplierCode.ClientID %>').val() != '' && ($('#<%=hdnGCTransactionStatus.ClientID %>').val() != 'X121^003' &&
                $('#<%=hdnGCTransactionStatus.ClientID %>').val() != 'X121^999'))
                isEnabled = true;

            if (!isEnabled)
                $('#btnCopyDirectPurchase').attr('enabled', 'false');
            else
                $('#btnCopyDirectPurchase').removeAttr('enabled');
        }

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var directPurchasereturnID = $('#<%=hdnPRID.ClientID %>').val();
            var printStatus = $('#<%=hdnPrintStatus.ClientID %>').val();
            if (printStatus == 'true') {
                if (directPurchasereturnID == '' || directPurchasereturnID == '0') {
                    errMessage.text = 'Please Set Transaction First!';
                    return false;
                }
                else {
                    filterExpression.text = "DirectPurchaseReturnID = " + directPurchasereturnID;
                    return true;
                }
            } else {
                errMessage.text = "Data Doesn't Approved or Closed";
                return false;
            }
        }

    </script>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnPrintStatus" runat="server" />
    <input type="hidden" value="0" id="hdnPRID" runat="server" />
    <input type="hidden" value="0" id="hdnDirectPurchaseID" runat="server" />
    <input type="hidden" value="" id="hdnPageCount" runat="server" />
    <input type="hidden" value="" id="hdnGCTransactionStatus" runat="server" />
    <input type="hidden" value="1" id="hdnIsEditable" runat="server" />
    <input type="hidden" value="" id="hdnVATPercentageFromSetvar" runat="server" />
    <input type="hidden" value="" id="hdnVATPercentage" runat="server" />
    <input type="hidden" value="" id="hdnRecordFilterExpression" runat="server" />
    <input type="hidden" value="0" id="hdnDirectPurchaseDtID" runat="server" />
    <input type="hidden" value="0" id="hdnQuantityForReturn" runat="server" />
    <input type="hidden" value="0" id="hdnIsPpnAllowChanged" runat="server" />
    <input type="hidden" value="0" id="hdnIsCalculateHNA" runat="server" />
    <input type="hidden" value="0" id="hdnIsPPNAppliedToAveragePrice" runat="server" />
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
                                <asp:TextBox ID="txtDirectPurchaseReturnNo" Width="150px" ReadOnly="true" runat="server" />
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
                                            <asp:TextBox ID="txtSupplierCode" ReadOnly="true" Width="100%" runat="server" ValidationGroup="mpEntry" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSupplierName" ReadOnly="true" Width="100%" runat="server" ValidationGroup="mpEntry" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory lblLink" id="lblDirectPurchaseNo" runat="server">
                                    <%=GetLabel("No. Pembelian Tunai")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDirectPurchaseNo" Width="150px" ReadOnly="true" runat="server"
                                    ValidationGroup="mpEntry" />
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
                                <label class="lblMandatory" runat="server" id="lblLocation">
                                    <%=GetLabel("Dari Lokasi")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnLocationID" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
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
                                <dxe:ASPxComboBox ID="cboReturnType" ClientInstanceName="cboReturnType" Width="300px"
                                    runat="server" ValidationGroup="mpEntry">
                                    <ClientSideEvents ValueChanged="function(s,e){ onCboReturnTypeValueChanged(); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("No Referensi") %>
                            </td>
                            <td>
                                <asp:TextBox ID="txtReferenceNo" Width="120px" runat="server" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Tanggal di Faktur") %>
                            </td>
                            <td>
                                <asp:TextBox ID="txtReferenceDate" Width="120px" CssClass="datepicker" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:CheckBox ID="chkPPN" runat="server" />
                                <%=GetLabel("PPN")%>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div id="containerEntry" style="margin-top: 4px; display: none;">
                        <div class="pageTitle">
                            <%=GetLabel("Edit atau Tambah Item Retur Pembelian Tunai")%></div>
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
                                                    <input type="hidden" value="" id="hdnQtyMax" runat="server" />
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
                                                    <input type="hidden" value="<%#:Eval("DirectPurchaseDtID") %>" bindingfield="DirectPurchaseDtID" />
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
                                                    <input type="hidden" value="<%#:Eval("ReceivedQuantity") %>" bindingfield="ReceivedQuantity" />
                                                    <input type="hidden" value="<%#:Eval("ReceivedItemUnit") %>" bindingfield="ReceivedItemUnit" />
                                                    <input type="hidden" value="<%#:Eval("CustomSubTotal") %>" bindingfield="CustomSubTotal" />
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
                                            <asp:BoundField DataField="CustomTotalDiscount" HeaderText="Total Discount" HeaderStyle-Width="120px"
                                                HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" />
                                            <asp:BoundField DataField="CustomSubTotal" HeaderText="SubTotal" HeaderStyle-Width="180px"
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
                                                <col style="width: 180px" />
                                            </colgroup>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Jumlah Nilai Retur")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtTotalRetur" CssClass="txtCurrency" ReadOnly="true" Width="180px"
                                                        runat="server" />
                                                </td>
                                            </tr>
                                            <tr style="display: none">
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Diskon Final %")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtFinalDiscountInPercentage" CssClass="txtCurrency" Width="180px"
                                                        runat="server" />
                                                </td>
                                            </tr>
                                            <tr style="display: none">
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
                                                    <label class="lblNormal">
                                                        <%=GetLabel("PPN")%>
                                                        <asp:TextBox ID="txtVATPercentageDefault" CssClass="txtCurrency" ReadOnly="true"
                                                            Width="40px" runat="server" />
                                                        %
                                                    </label>
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
