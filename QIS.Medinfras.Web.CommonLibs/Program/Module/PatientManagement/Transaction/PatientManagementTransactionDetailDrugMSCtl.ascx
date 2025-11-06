<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PatientManagementTransactionDetailDrugMSCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientManagementTransactionDetailDrugMSCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_ptdrgmsctl">
    //#region Drug MS
    function onLoadDrugMS() {
        if ($('#<%=hdnIsEditable.ClientID %>').val() == '1') {
            $('#lblDrugMSAddData').show();
            $('#lblDrugMSQuickPick').show();

            if ($('#<%=hdnIsAIOTransactionDrugCtl.ClientID %>').val() == '1' || $('#<%=hdnIsChargesGenerateMCUDrugCtl.ClientID %>').val() == '1') {
                $('#lblDrugMSAddData').hide();
                $('#lblDrugMSQuickPick').hide();
            }
        }
        else {
            $('#lblDrugMSAddData').hide();
            $('#lblDrugMSQuickPick').hide();
        }

        $('#btnDrugMSSave').click(function (evt) {
            var itemID = $('#<%=hdnDrugMSItemID.ClientID %>').val();
            if (IsValid(evt, 'fsTrxDrugMS', 'mpTrxDrugMS')) {
                if (itemID == 0) {
                    showToast('Warning', 'Mohon Isi Obat Terlebih Dahulu!');
                }
                else {
                    cbpDrugMS.PerformCallback('save');
                }
            }
            return false;
        });

        $('#btnDrugMSCancel').click(function () {
            $('#containerEntryDrugMS').hide();

            var totalAllPayer = parseFloat($('#<%=hdnTempTotalPayer.ClientID %>').val());
            $('#<%=hdnDrugMSAllTotalPayer.ClientID %>').val(totalAllPayer);
            $('.tdDrugMSTotalPayer').html(totalAllPayer.formatMoney(2, '.', ','));

            var totalAllPatient = parseFloat($('#<%=hdnTempTotalPatient.ClientID %>').val());
            $('#<%=hdnDrugMSAllTotalPatient.ClientID %>').val(totalAllPatient);
            $('.tdDrugMSTotalPatient').html(totalAllPatient.formatMoney(2, '.', ','));

            var totalLineAmount = totalAllPatient + totalAllPayer;
            $('.tdDrugMSTotal').html(totalLineAmount.formatMoney(2, '.', ','));

            calculateAllTotal();
        });

        $('#lblDrugMSQuickPick').live('click', function () {
            if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
                showLoadingPanel();
                var url = ResolveUrl('~/Libs/Program/Module/PatientManagement/Transaction/DrugsLogisticsQuickPicksCtl.ascx');
                var transactionID = getTransactionHdID();
                var locationID = getLocationID();
                var registrationID = getRegistrationID();
                var visitID = getVisitID();
                var GCItemType = getGCItemType();
                var departmentID = getDepartmentID();
                var serviceUnitID = getHealthcareServiceUnitID();
                var drugTransactionType = $('#<%=hdnIsDrugChargesJustDistribution.ClientID %>').val();
                var isAccompany = "0";
                if (typeof isAccompanyChargesPage == 'function') {
                    if (isAccompanyChargesPage()) {
                        isAccompany = "1";
                    }
                }

                var id = transactionID + '|' + locationID + '|' + visitID + '|' + registrationID + '|' + GCItemType + '|' + departmentID + '|' + serviceUnitID + '|' + isAccompany + '|' + drugTransactionType;
                openUserControlPopup(url, id, 'Quick Picks', 1000, 600);
            }
        });

        $('#<%=txtDrugMSQtyUsed.ClientID %>').change(function () {
            var qty = parseFloat($('#<%=txtDrugMSQtyUsed.ClientID %>').val());

            var isQtyChecked = "0";
            var isUsingValidateDigitDecimal = $('#<%=hdnIsUsingValidateDigitDecimal.ClientID %>').val();

            if ($('#<%=txtDrugMSQtyUsed.ClientID %>').val().includes(".")) {
                var qtyCheckDesimalList = $('#<%=txtDrugMSQtyUsed.ClientID %>').val().split(".");
                var qtyCheckDesimal = qtyCheckDesimalList[1];
                if (qtyCheckDesimal.length > 2 && isUsingValidateDigitDecimal == "1") {
                    isQtyChecked = "1";
                } else {
                    $('#<%=txtDrugMSQtyUsed.ClientID %>').val(qty.toFixed(2)).trigger('changeValue');
                }
            }

            if (isQtyChecked == "1") {
                ShowSnackbarError("Maksimal digit desimal belakang koma adalah 2 digit.");
                $('#<%=txtDrugMSQtyUsed.ClientID %>').val(1);
                $('#<%=txtDrugMSQtyUsed.ClientID %>').change();
            } else {
                $('#<%=txtDrugMSQtyCharged.ClientID %>').val(qty.toFixed(2));
                $('#<%=txtDrugMSQtyCharged.ClientID %>').change();

                var priceUnitTariff = parseFloat($('#<%=txtDrugMSUnitTariff.ClientID %>').attr('hiddenVal'));
                $('#txtDrugMSUnitTariff').val(priceUnitTariff).trigger('changeValue');

                calculateDrugMSTariffTotal();
                calculateDrugMSDiscountTotal();
                calculateDrugMSTotal();
            }
        });

        $('#<%=txtDrugMSQtyCharged.ClientID %>').change(function () {
            var conversionValue = parseFloat($('#<%=hdnDrugMSConversionValue.ClientID %>').val());
            var qty = parseFloat($('#<%=txtDrugMSQtyCharged.ClientID %>').val());

            var isQtyChecked = "0";
            var isUsingValidateDigitDecimal = $('#<%=hdnIsUsingValidateDigitDecimal.ClientID %>').val();

            if ($('#<%=txtDrugMSQtyCharged.ClientID %>').val().includes(".")) {
                var qtyCheckDesimalList = $('#<%=txtDrugMSQtyCharged.ClientID %>').val().split(".");
                var qtyCheckDesimal = qtyCheckDesimalList[1];
                if (qtyCheckDesimal.length > 2 && isUsingValidateDigitDecimal == "1") {
                    isQtyChecked = "1";
                } else {
                    $('#<%=txtDrugMSQtyCharged.ClientID %>').val(qty.toFixed(2)).trigger('changeValue');
                }
            }

            if (isQtyChecked == "1") {
                ShowSnackbarError("Maksimal digit desimal belakang koma adalah 2 digit.");
                $('#<%=txtDrugMSQtyCharged.ClientID %>').val(1);
                $('#<%=txtDrugMSQtyCharged.ClientID %>').change();
            } else {
                $('#<%=txtDrugMSBaseQty.ClientID %>').val((conversionValue * qty).toFixed(2));
                var priceUnitTariff = parseFloat($('#<%=txtDrugMSUnitTariff.ClientID %>').attr('hiddenVal'));
                $('#txtDrugMSUnitTariff').val(priceUnitTariff).trigger('changeValue');

                calculateDrugMSTariffTotal();
                calculateDrugMSDiscountTotal();
                calculateDrugMSTotal();
            }
        });

        $('#<%=txtDrugMSPriceDiscount.ClientID %>').change(function () {
            var isDicountInPercentage = ($('#<%=hdnDrugMSIsDicountInPercentage.ClientID %>').val() == '1');
            var discount = parseFloat($(this).val());
            var tariff = parseFloat($('#<%=txtDrugMSPriceTariff.ClientID %>').attr('hiddenVal'));

            if (isDicountInPercentage) {
                var discountTotal = (discount * 100) / tariff;
                $('#<%=hdnDrugMSDiscountAmount.ClientID %>').val(discountTotal);
                calculateDrugMSDiscountTotal();
                calculateDrugMSTotal();
            } else {
                if (discount > tariff) {
                    discount = tariff;
                }

                $('#<%=txtDrugMSPriceDiscount.ClientID %>').val(discount).trigger('changeValue');
                calculateDrugMSTotal();
            }
        });

        $('#<%=txtDrugMSPatient.ClientID %>').change(function () {
            var patientTotal = parseFloat($(this).val());
            var total = parseFloat($('#<%=txtDrugMSTotal.ClientID %>').attr('hiddenVal'));
            var payerTotal = parseFloat($('#<%=txtDrugMSPayer.ClientID %>').attr('hiddenVal'));
            if (patientTotal > total) {
                patientTotal = total;
                $(this).val(patientTotal);
            }
            payerTotal = total - patientTotal;
            $('#<%=txtDrugMSPayer.ClientID %>').val(payerTotal).trigger('changeValue');

            var bpID = $('#<%=hdnBusinessPartnerID.ClientID %>').val();
            var patientAmount = parseFloat($('#<%=txtDrugMSPatient.ClientID %>').val());
            if (bpID != 1) {
                if (patientAmount > 0) {
                    showToast('Informasi', 'Ada yang ditanggung PASIEN');
                }
            }
        });

        $('#<%=txtDrugMSPayer.ClientID %>').change(function () {
            var payerTotal = parseFloat($(this).val());
            var total = parseFloat($('#<%=txtDrugMSTotal.ClientID %>').attr('hiddenVal'));
            var patientTotal = parseFloat($('#<%=txtDrugMSPatient.ClientID %>').attr('hiddenVal'));
            if (payerTotal > total) {
                payerTotal = total;
                $(this).val(payerTotal);
            }
            patientTotal = total - payerTotal;
            $('#<%=txtDrugMSPatient.ClientID %>').val(patientTotal).trigger('changeValue');

            var bpID = $('#<%=hdnBusinessPartnerID.ClientID %>').val();
            var patientAmount = parseFloat($('#<%=txtDrugMSPatient.ClientID %>').val());
            if (bpID != 1) {
                if (patientAmount > 0) {
                    showToast('Informasi', 'Ada yang ditanggung PASIEN');
                }
            }
        });
    }

    function onDrugAIOClicked(param) {
        if (param) {
            $('#lblDrugMSAddData').hide();
            $('#lblDrugMSQuickPick').hide();
        }
        else {
            $('#lblDrugMSAddData').show();
            $('#lblDrugMSQuickPick').show();
        }
    }

    //#region Entry Drug MS
    $('.imgDrugMSApprove.imgLink').die('click');
    $('.imgDrugMSApprove.imgLink').live('click', function () {
        $row = $(this).closest('tr').parent().closest('tr');
        var obj = rowToObject($row);
        cbpDrugMS.PerformCallback('approve|' + obj.ID);
    });

    $('.imgDrugMSVoid.imgLink').die('click');
    $('.imgDrugMSVoid.imgLink').live('click', function () {
        $row = $(this).closest('tr').parent().closest('tr');
        var obj = rowToObject($row);
        cbpDrugMS.PerformCallback('void|' + obj.ID);
    });

    $('.imgDrugMSDelete.imgLink').die('click');
    $('.imgDrugMSDelete.imgLink').live('click', function () {
        if ($('#<%=hdnIsChargesGenerateMCUDrugCtl.ClientID %>').val() == "1") {
            displayErrorMessageBox("WARNING", "Maaf, transaksi hasil Generate MCU tidak dapat diubah.");
        } else {
            $row = $(this).closest('tr').parent().closest('tr');
            showDeleteConfirmation(function (data) {
                var obj = rowToObject($row);
                var param = 'delete|' + obj.ID + ';' + data.GCDeleteReason + ';' + data.Reason;
                cbpDrugMS.PerformCallback(param);
            });
        }
    });

    $('.imgDrugMSSwitch.imgLink').die('click');
    $('.imgDrugMSSwitch.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        var obj = rowToObject($row);
        cbpDrugMS.PerformCallback('switch|' + obj.ID);
    });

    $('.imgDrugMSEdit.imgLink').die('click');
    $('.imgDrugMSEdit.imgLink').live('click', function () {
        if ($('#<%=hdnIsChargesGenerateMCUDrugCtl.ClientID %>').val() == "1") {
            displayErrorMessageBox("WARNING", "Maaf, transaksi hasil Generate MCU tidak dapat diubah.");
        } else {
            $('#containerEntryDrugMS').show();
            showLoadingPanel();

            $('#<%=hdnTempTotalPatient.ClientID %>').val(getDrugMSTotalPatient());
            $('#<%=hdnTempTotalPayer.ClientID %>').val(getDrugMSTotalPayer());

            cboDrugMSLocation.SetEnabled(false);
            $row = $(this).closest('tr').parent().closest('tr');
            var obj = rowToObject($row);

            cboDrugMSLocation.SetValue(obj.LocationID);
            cboDrugMSChargeClassID.SetValue(obj.ChargeClassID);

            $('#<%=txtDrugMSBaseQty.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtDrugMSItemCode.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtDrugMSConversion.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtDrugMSTotal.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtDrugMSPriceTariff.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtDrugMSUnitTariff.ClientID %>').attr('readonly', 'readonly');

            $('#<%=hdnDrugMSItemID.ClientID %>').val(obj.ItemID);
            $('#<%=txtDrugMSItemCode.ClientID %>').val(obj.ItemCode);
            $('#<%=txtDrugMSItemName.ClientID %>').val(obj.ItemName1);
            $('#<%=hdnDrugMSTransactionDtID.ClientID %>').val(obj.ID);

            $('#<%=hdnIsFromEdit.ClientID %>').val("1");
            objDrugMS = obj;
            cboDrugMSUoM.PerformCallback();

            $('#<%=txtDrugMSPatient.ClientID %>').val(obj.PatientAmount).trigger('changeValue');
            $('#<%=txtDrugMSPayer.ClientID %>').val(obj.PayerAmount).trigger('changeValue');
            $('#<%=txtDrugMSTotal.ClientID %>').val(obj.LineAmount).trigger('changeValue');

            $('#<%=txtDrugMSUnitTariff.ClientID %>').val(parseFloat(obj.Tariff)).trigger('changeValue');

            $('#<%=txtDrugMSQtyUsed.ClientID %>').val(obj.UsedQuantity);
            $('#<%=txtDrugMSQtyCharged.ClientID %>').val(obj.ChargedQuantity);
            $('#<%=txtDrugMSBaseQty.ClientID %>').val(obj.BaseQuantity);
            $('#<%=txtDrugMSPriceTariff.ClientID %>').val(parseFloat(obj.GrossLineAmount)).trigger('changeValue');

            $('#<%=txtDrugMSPriceDiscount.ClientID %>').val(obj.DiscountAmount).trigger('changeValue');

            cboDrugMSUoM.SetValue(obj.GCItemUnit);
            $('#<%=hdnDrugMSConversionValue.ClientID %>').val(obj.ConversionFactor);
            $('#<%=hdnDrugMSConversionValueOld.ClientID %>').val(obj.ConversionFactor);
            $('#<%=hdnDrugMSDefaultUoM.ClientID %>').val(obj.GCBaseUnit);

            var conversionFactor = $('#<%=hdnDrugMSConversionValue.ClientID %>').val();
            var defaultUoM = $('#<%=hdnDrugMSDefaultUoM.ClientID %>').val();
            var currUoM = cboDrugMSUoM.GetValue();
            var fromConversion = getDrugMSItemUnitName(defaultUoM);
            var toConversion = getDrugMSItemUnitName(currUoM);

            var conversion = "1 " + toConversion + " = " + conversionFactor + " " + fromConversion;
            $('#<%=txtDrugMSConversion.ClientID %>').val(conversion);

            $('#<%=hdnDrugMSDiscountAmount.ClientID %>').val(obj.DiscountAmount);
            $('#<%=hdnDrugMSDiscountAmountComp1.ClientID %>').val(obj.DiscountComp1);
            $('#<%=hdnDrugMSDiscountAmountComp2.ClientID %>').val(obj.DiscountComp2);
            $('#<%=hdnDrugMSDiscountAmountComp3.ClientID %>').val(obj.DiscountComp3);

            $('#<%=hdnDrugMSIsDiscountUsedComp.ClientID %>').val(obj.IsDiscountUsedComp ? '1' : '0');

            $('#<%=hdnDrugMSIsDicountInPercentage.ClientID %>').val(obj.IsDiscountInPercentage ? '1' : '0');
            $('#<%=hdnDrugMSIsDicountInPercentageComp1.ClientID %>').val(obj.IsDiscountInPercentageComp1 ? '1' : '0');
            $('#<%=hdnDrugMSIsDicountInPercentageComp2.ClientID %>').val(obj.IsDiscountInPercentageComp2 ? '1' : '0');
            $('#<%=hdnDrugMSIsDicountInPercentageComp3.ClientID %>').val(obj.IsDiscountInPercentageComp3 ? '1' : '0');

            $('#<%=hdnDrugMSCoverageAmount.ClientID %>').val(obj.PayerAmount);
            $('#<%=hdnDrugMSCostAmount.ClientID %>').val(obj.CostAmount);

            $('#<%=txtDrugMSQtyUsed.ClientID %>').focus();

            hideLoadingPanel();
        }
    });

    var objDrugMS = null;
    $('#lblDrugMSAddData').live('click', function () {
        if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
            $('#<%=hdnTempTotalPatient.ClientID %>').val(getDrugMSTotalPatient());
            $('#<%=hdnTempTotalPayer.ClientID %>').val(getDrugMSTotalPayer());

            if (typeof onAddRecordSetControlDisabled == 'function')
                onAddRecordSetControlDisabled();
            $('#containerEntryDrugMS').show();
            //showLoadingPanel();
            cboDrugMSLocation.SetEnabled(true);

            cboDrugMSUoM.ClearItems();
            cboDrugMSUoM.SetValue('');

            $('#<%=hdnIsFromEdit.ClientID %>').val("0");

            //$('#lblDrugMSItem').attr('class', 'lblLink');
            $('#<%=txtDrugMSItemCode.ClientID %>').removeAttr('readonly');
            $('#<%=txtDrugMSBaseQty.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtDrugMSConversion.ClientID %>').attr('readonly', 'readonly');
            //$('#<%=txtDrugMSPatient.ClientID %>').attr('readonly', 'readonly');
            //$('#<%=txtDrugMSPayer.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtDrugMSTotal.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtDrugMSPriceTariff.ClientID %>').attr('readonly', 'readonly');
            //            $('#<%=txtDrugMSPriceDiscount.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtDrugMSUnitTariff.ClientID %>').attr('readonly', 'readonly');

            $('#<%=hdnDrugMSTransactionDtID.ClientID %>').val('');
            $('#<%=hdnDrugMSItemID.ClientID %>').val('');
            $('#<%=txtDrugMSItemCode.ClientID %>').val('');
            $('#<%=txtDrugMSItemName.ClientID %>').val('');
            cboDrugMSChargeClassID.SetValue(getClassID());

            $('#<%=hdnDrugMSDiscountAmount.ClientID %>').val('0');
            $('#<%=hdnDrugMSDiscountAmountComp1.ClientID %>').val('0');
            $('#<%=hdnDrugMSDiscountAmountComp2.ClientID %>').val('0');
            $('#<%=hdnDrugMSDiscountAmountComp3.ClientID %>').val('0');

            $('#<%=hdnDrugMSIsDiscountUsedComp.ClientID %>').val('0');

            $('#<%=hdnDrugMSIsDicountInPercentage.ClientID %>').val('0');
            $('#<%=hdnDrugMSIsDicountInPercentageComp1.ClientID %>').val('0');
            $('#<%=hdnDrugMSIsDicountInPercentageComp2.ClientID %>').val('0');
            $('#<%=hdnDrugMSIsDicountInPercentageComp3.ClientID %>').val('0');

            $('#<%=txtDrugMSPatient.ClientID %>').val('0').trigger('changeValue');
            $('#<%=txtDrugMSPayer.ClientID %>').val('0').trigger('changeValue');
            $('#<%=txtDrugMSTotal.ClientID %>').val('0').trigger('changeValue');
            $('#<%=txtDrugMSUnitTariff.ClientID %>').val('0').trigger('changeValue');

            $('#<%=hdnDrugMSConversionValue.ClientID %>').val('0');
            $('#<%=hdnDrugMSConversionValueOld.ClientID %>').val('0');
            $('#<%=txtDrugMSQtyUsed.ClientID %>').val('1');
            $('#<%=txtDrugMSQtyCharged.ClientID %>').val('1');
            $('#<%=txtDrugMSBaseQty.ClientID %>').val('');
            $('#<%=txtDrugMSPriceTariff.ClientID %>').val('0').trigger('changeValue');
            $('#<%=txtDrugMSPriceDiscount.ClientID %>').val('0').trigger('changeValue');

            $('#<%=hdnDrugMSDefaultUoM.ClientID %>').val('');
            $('#<%=txtDrugMSConversion.ClientID %>').val('');
            $('#<%=hdnDrugMSCostAmount.ClientID %>').val('0');

            cboDrugMSLocation.SetFocus();
            hideLoadingPanel();
        }
    });
    //#endregion

    //#region Item
    function onGetDrugMSItemFilterExpression() {
        var locationID = cboDrugMSLocation.GetValue();
        var filterExpression = $('#<%=hdnLedDrugMSItemFilterExpression.ClientID %>').val().replace('[LocationID]', locationID);

        if ($('#<%=hdnIsAllowOverIssued.ClientID %>').val() == '0') {
            filterExpression += " AND QuantityEND > 0 ";
        }

        filterExpression += " AND GCItemStatus != 'X181^999'";

        if ($('#<%=hdnIsDrugChargesJustDistribution.ClientID %>').val() == "1") {
            filterExpression += " AND GCItemRequestType = 'X217^01'";
        }

        return filterExpression;
    }

    $('#lblDrugMSItem.lblLink').live('click', function () {
        openSearchDialog('itembalance1', onGetDrugMSItemFilterExpression(), function (value) {
            $('#<%=txtDrugMSItemCode.ClientID %>').val(value);
            onTxtDrugMSItemCodeChanged(value);
        });
    });

    $('#<%=txtDrugMSItemCode.ClientID %>').live('change', function () {
        onTxtDrugMSItemCodeChanged($(this).val());
    });

    function onTxtDrugMSItemCodeChanged(value) {
        var filterExpression = onGetDrugMSItemFilterExpression() + " AND ItemCode = '" + value + "'";
        Methods.getObject('GetvItemBalanceList', filterExpression, function (result) {
            if (result != null) {
                objDrugMS = result;
                $('#<%=hdnDrugMSItemID.ClientID %>').val(result.ItemID);
                $('#<%=hdnDrugMSDefaultUoM.ClientID %>').val(result.GCItemUnit);
                $('#<%=hdnDrugMSItemID.ClientID %>').val(result.ItemID);
                $('#<%=txtDrugMSItemName.ClientID %>').val(result.ItemName1);

                cboDrugMSUoM.PerformCallback();
            }
            else {
                $('#<%=hdnDrugMSItemID.ClientID %>').val('');
                $('#<%=txtDrugMSItemCode.ClientID %>').val('');
                $('#<%=txtDrugMSItemName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    function setDrugMSItemFilterExpression(transactionID) {
        $('#<%=hdnLedDrugMSItemFilterExpression.ClientID %>').val("LocationID = [LocationID] AND GCItemType = '" + Constant.ItemGroupMaster.DRUGS + "' AND ItemID NOT IN (SELECT ItemID FROM PatientChargesDt WHERE TransactionID = " + transactionID + " AND IsDeleted = 0) AND IsDeleted = 0");
    }

    function onCboDrugMSUomEndCallback() {
        if ($('#<%=hdnIsFromEdit.ClientID %>').val() == "1") {
            cboDrugMSUoM.SetValue(objDrugMS.GCItemUnit);
            var conversionFactor = $('#<%=hdnDrugMSConversionValue.ClientID %>').val();
            var defaultUoM = $('#<%=hdnDrugMSDefaultUoM.ClientID %>').val();
            var currUoM = cboDrugMSUoM.GetValue();
            var fromConversion = getDrugMSItemUnitName(defaultUoM);
            var toConversion = getDrugMSItemUnitName(currUoM);

            var conversion = "1 " + toConversion + " = " + conversionFactor + " " + fromConversion;
            $('#<%=txtDrugMSConversion.ClientID %>').val(conversion);
        }
        else {
            getDrugMSTariff();
        }

        if ($('#<%=hdnIsAIOTransactionDrugCtl.ClientID %>').val() == '1' || $('#<%=hdnIsChargesGenerateMCUDrugCtl.ClientID %>').val() == '1') {
            $('#lblDrugMSAddData').hide();
            $('#lblDrugMSQuickPick').hide();
        } else {
            $('#lblDrugMSAddData').show();
            $('#lblDrugMSQuickPick').show();
        }

        $('#<%=hdnIsFromEdit.ClientID %>').val("0");
        hideLoadingPanel();
    }

    function getDrugMSTariff() {
        showLoadingPanel();
        var itemID = $('#<%=hdnDrugMSItemID.ClientID %>').val();
        var registrationID = getRegistrationID();
        var visitID = getVisitID();
        var classID = cboDrugMSChargeClassID.GetValue();
        var trxDate = getTrxDate();

        Methods.getItemTariff(registrationID, visitID, classID, itemID, trxDate, function (result) {
            if (result != null) {
                cboDrugMSUoM.SetValue(result.GCItemUnit);
                $('#<%=hdnDrugMSBaseTariff.ClientID %>').val(result.BasePrice);
                $('#<%=txtDrugMSUnitTariff.ClientID %>').val(result.Price).trigger('changeValue');
                $('#<%=hdnDrugMSDefaultUoM.ClientID %>').val(result.GCItemUnit);
                $('#<%=hdnDrugMSCostAmount.ClientID %>').val(result.CostAmount);

                $('#<%=hdnDrugMSDiscountAmount.ClientID %>').val(result.DiscountAmount);
                $('#<%=hdnDrugMSDiscountAmountComp1.ClientID %>').val(result.DiscountAmountComp1);
                $('#<%=hdnDrugMSDiscountAmountComp2.ClientID %>').val(result.DiscountAmountComp2);
                $('#<%=hdnDrugMSDiscountAmountComp3.ClientID %>').val(result.DiscountAmountComp3);

                $('#<%=hdnDrugMSIsDiscountUsedComp.ClientID %>').val(result.IsDiscountUsedComp ? '1' : '0');

                $('#<%=hdnDrugMSIsDicountInPercentage.ClientID %>').val(result.IsDiscountInPercentage ? '1' : '0');
                $('#<%=hdnDrugMSIsDicountInPercentageComp1.ClientID %>').val(result.IsDiscountInPercentageComp1 ? '1' : '0');
                $('#<%=hdnDrugMSIsDicountInPercentageComp2.ClientID %>').val(result.IsDiscountInPercentageComp2 ? '1' : '0');
                $('#<%=hdnDrugMSIsDicountInPercentageComp3.ClientID %>').val(result.IsDiscountInPercentageComp3 ? '1' : '0');

                $('#<%=hdnDrugMSCoverageAmount.ClientID %>').val(result.CoverageAmount);
                $('#<%=hdnDrugMSIsCoverageInPercentage.ClientID %>').val(result.IsCoverageInPercentage ? '1' : '0');

                setDrugMSItemUnitConversionText();
            }
            else {
                $('#<%=hdnDrugMSDefaultUoM.ClientID %>').val('');
                $('#<%=hdnDrugMSBaseTariff.ClientID %>').val('0');
                $('#<%=txtDrugMSUnitTariff.ClientID %>').val('0').trigger('changeValue');
                $('#<%=hdnDrugMSConversionValue.ClientID %>').val('0');
                $('#<%=hdnDrugMSConversionValueOld.ClientID %>').val('0');
                $('#<%=txtDrugMSConversion.ClientID %>').val('');

                $('#<%=hdnDrugMSCostAmount.ClientID %>').val('0');

                $('#<%=hdnDrugMSDiscountAmount.ClientID %>').val('0');
                $('#<%=hdnDrugMSDiscountAmountComp1.ClientID %>').val('0');
                $('#<%=hdnDrugMSDiscountAmountComp2.ClientID %>').val('0');
                $('#<%=hdnDrugMSDiscountAmountComp3.ClientID %>').val('0');

                $('#<%=hdnDrugMSIsDiscountUsedComp.ClientID %>').val('0');

                $('#<%=hdnDrugMSIsDicountInPercentage.ClientID %>').val('0');
                $('#<%=hdnDrugMSIsDicountInPercentageComp1.ClientID %>').val('0');
                $('#<%=hdnDrugMSIsDicountInPercentageComp2.ClientID %>').val('0');
                $('#<%=hdnDrugMSIsDicountInPercentageComp3.ClientID %>').val('0');

                $('#<%=hdnDrugMSCoverageAmount.ClientID %>').val('0');
                $('#<%=hdnDrugMSIsCoverageInPercentage.ClientID %>').val('0');
                cboDrugMSUoM.SetValue('');
            }
            calculateDrugMSTariffTotal();
            calculateDrugMSTotal();
        });
        hideLoadingPanel();
    }

    function onCboDrugMSChargeClassIDValueChanged() {
        var oItemID = $('#<%=hdnDrugMSItemID.ClientID %>').val();
        if (oItemID != null && oItemID != "") {
            getDrugMSTariff();
        }
    }

    function setDrugMSItemUnitConversionText() {
        var defaultUoM = $('#<%=hdnDrugMSDefaultUoM.ClientID %>').val();
        var currUoM = cboDrugMSUoM.GetValue();
        var fromConversion = getDrugMSItemUnitName(defaultUoM);
        if (defaultUoM == currUoM) {
            $('#<%=hdnDrugMSConversionValueOld.ClientID %>').val($('#<%=hdnDrugMSConversionValue.ClientID %>').val());
            $('#<%=hdnDrugMSConversionValue.ClientID %>').val('1');
            $('#<%=txtDrugMSQtyCharged.ClientID %>').change();
            var conversion = "1 " + fromConversion + " = 1 " + fromConversion;
            $('#<%=txtDrugMSConversion.ClientID %>').val(conversion);
        }
        else {
            var itemID = $('#<%=hdnDrugMSItemID.ClientID %>').val();
            var filterExpression = "IsDeleted = 0 AND IsActive = 1 AND ItemID = " + itemID + " AND GCAlternateUnit = '" + currUoM + "'";
            Methods.getObjectValue('GetvItemAlternateUnitList', filterExpression, 'ConversionFactor', function (result) {
                if (result == "" || result == null) {
                    result = "1";
                }
                var toConversion = getDrugMSItemUnitName(currUoM);
                $('#<%=hdnDrugMSConversionValueOld.ClientID %>').val($('#<%=hdnDrugMSConversionValue.ClientID %>').val());
                $('#<%=hdnDrugMSConversionValue.ClientID %>').val(result);
                $('#<%=txtDrugMSQtyCharged.ClientID %>').change();
                var conversion = "1 " + toConversion + " = " + result + " " + fromConversion;
                $('#<%=txtDrugMSConversion.ClientID %>').val(conversion);
            });
        }
    }

    function calculateDrugMSTariffTotal() {
        var tariff = parseFloat($('#<%=txtDrugMSUnitTariff.ClientID %>').attr('hiddenVal'));
        var qty = parseFloat($('#<%=txtDrugMSBaseQty.ClientID %>').val());
        $('#<%=txtDrugMSPriceTariff.ClientID %>').val(tariff * qty).trigger('changeValue');
    }

    function calculateDrugMSDiscountTotal() {
        var itemID = $('#<%=hdnDrugMSItemID.ClientID %>').val();
        var registrationID = getRegistrationID();
        var visitID = getVisitID();
        var classID = cboDrugMSChargeClassID.GetValue();
        var trxDate = getTrxDate();
        Methods.getItemTariff(registrationID, visitID, classID, itemID, trxDate, function (result) {
            if (result != null) {
                $('#<%=hdnDrugMSDiscountAmount.ClientID %>').val(result.DiscountAmount);
                $('#<%=hdnDrugMSDiscountAmountComp1.ClientID %>').val(result.DiscountAmountComp1);
                $('#<%=hdnDrugMSDiscountAmountComp2.ClientID %>').val(result.DiscountAmountComp2);
                $('#<%=hdnDrugMSDiscountAmountComp3.ClientID %>').val(result.DiscountAmountComp3);

                $('#<%=hdnDrugMSIsDiscountUsedComp.ClientID %>').val(result.IsDiscountUsedComp ? '1' : '0');

                $('#<%=hdnDrugMSIsDicountInPercentage.ClientID %>').val(result.IsDiscountInPercentage ? '1' : '0');
                $('#<%=hdnDrugMSIsDicountInPercentageComp1.ClientID %>').val(result.IsDiscountInPercentageComp1 ? '1' : '0');
                $('#<%=hdnDrugMSIsDicountInPercentageComp2.ClientID %>').val(result.IsDiscountInPercentageComp2 ? '1' : '0');
                $('#<%=hdnDrugMSIsDicountInPercentageComp3.ClientID %>').val(result.IsDiscountInPercentageComp3 ? '1' : '0');
            }
            else {
                $('#<%=hdnDrugMSDiscountAmount.ClientID %>').val('0');
                $('#<%=hdnDrugMSDiscountAmountComp1.ClientID %>').val('0');
                $('#<%=hdnDrugMSDiscountAmountComp2.ClientID %>').val('0');
                $('#<%=hdnDrugMSDiscountAmountComp3.ClientID %>').val('0');

                $('#<%=hdnDrugMSIsDiscountUsedComp.ClientID %>').val('0');

                $('#<%=hdnDrugMSIsDicountInPercentage.ClientID %>').val('0');
                $('#<%=hdnDrugMSIsDicountInPercentageComp1.ClientID %>').val('0');
                $('#<%=hdnDrugMSIsDicountInPercentageComp2.ClientID %>').val('0');
                $('#<%=hdnDrugMSIsDicountInPercentageComp3.ClientID %>').val('0');
            }
        });

        var discountAmount = parseFloat($('#<%=hdnDrugMSDiscountAmount.ClientID %>').val());
        var discountAmountComp1 = parseFloat($('#<%=hdnDrugMSDiscountAmountComp1.ClientID %>').val());

        var isDiscountUsedComp = ($('#<%=hdnDrugMSIsDiscountUsedComp.ClientID %>').val() == '1');

        var isDicountInPercentage = ($('#<%=hdnDrugMSIsDicountInPercentage.ClientID %>').val() == '1');
        var isDicountInPercentageComp1 = ($('#<%=hdnDrugMSIsDicountInPercentageComp1.ClientID %>').val() == '1');

        var value = $('#<%=txtDrugMSPriceTariff.ClientID %>').val();
        var token = ",";
        var newToken = "";
        value = value.split(token).join(newToken);
        var tariff = parseFloat(value);

        var qty = parseFloat($('#<%=txtDrugMSBaseQty.ClientID %>').val());

        var discountTotal = 0;
        var discountComp1 = 0;
        var discountComp2 = 0;
        var discountComp3 = 0;
        if (isDicountInPercentage == "1" || isDicountInPercentageComp1 == "1") {
            if (isDiscountUsedComp == "1") {
                if (tariff > 0) {
                    if (isDicountInPercentageComp1 == "1") {
                        discountComp1 = parseFloat(tariff * (discountAmountComp1 / 100));
                    } else {
                        discountComp1 = parseFloat(discountAmountComp1);
                    }
                }
            } else {
                if (tariff > 0) {
                    if (isDicountInPercentageComp1 == "1") {
                        discountComp1 = parseFloat(tariff * (discountAmountComp1 / 100));
                    } else {
                        discountComp1 = parseFloat(tariff * (discountAmount / 100));
                    }
                }
            }
        } else {
            discountComp1 = parseFloat(discountAmountComp1);
        }

        discountTotal = (discountComp1 + discountComp2 + discountComp3);

        $('#<%=txtDrugMSPriceDiscount.ClientID %>').val(discountTotal).trigger('changeValue');
    }

    function calculateDrugMSTotal() {
        var tariff = parseFloat($('#<%=txtDrugMSPriceTariff.ClientID %>').attr('hiddenVal'));
        var discount = parseFloat($('#<%=txtDrugMSPriceDiscount.ClientID %>').attr('hiddenVal'));
        var total = tariff - discount;

        var coverageAmount = parseFloat($('#<%=hdnDrugMSCoverageAmount.ClientID %>').val());
        var isCoverageInPercentage = ($('#<%=hdnDrugMSIsCoverageInPercentage.ClientID %>').val() == '1');
        var totalPayer = 0;

        var qty = parseFloat($('#<%=txtDrugMSBaseQty.ClientID %>').val());
        if (isCoverageInPercentage) {
            totalPayer = (total * coverageAmount) / 100;
        }
        else {
            totalPayer = coverageAmount * qty;
        }

        if (total == 0) {
            totalPayer = total;
        } else {
            if (totalPayer < 0 && totalPayer < total) {
                totalPayer = total;
            } else {
                if (total > 0 && totalPayer > total) {
                    totalPayer = total;
                }
            }
        }

        var totalPatient = total - totalPayer;

        var totalAllPayer = parseFloat($('#<%=hdnDrugMSAllTotalPayer.ClientID %>').val());
        totalAllPayer = (totalAllPayer - parseFloat($('#<%=txtDrugMSPayer.ClientID %>').attr('hiddenVal')));
        totalAllPayer += totalPayer;
        $('#<%=hdnDrugMSAllTotalPayer.ClientID %>').val(totalAllPayer);
        $('.tdDrugMSTotalPayer').html(totalAllPayer.formatMoney(2, '.', ','));

        var totalAllPatient = parseFloat($('#<%=hdnDrugMSAllTotalPatient.ClientID %>').val());
        totalAllPatient = (totalAllPatient - parseFloat($('#<%=txtDrugMSPatient.ClientID %>').attr('hiddenVal')));
        totalAllPatient += totalPatient;
        $('#<%=hdnDrugMSAllTotalPatient.ClientID %>').val(totalAllPatient);
        $('.tdDrugMSTotalPatient').html(totalAllPatient.formatMoney(2, '.', ','));

        var totalLineAmount = totalAllPatient + totalAllPayer;
        $('.tdDrugMSTotal').html(totalLineAmount.formatMoney(2, '.', ','));

        $('#<%=txtDrugMSPayer.ClientID %>').val(totalPayer).trigger('changeValue');
        $('#<%=txtDrugMSPatient.ClientID %>').val(totalPatient).trigger('changeValue');
        $('#<%=txtDrugMSTotal.ClientID %>').val(total).trigger('changeValue');

        calculateAllTotal();
    }

    function getDrugMSItemUnitName(itemUnitID) {
        var value = cboDrugMSUoM.GetValue();
        cboDrugMSUoM.SetValue(itemUnitID);
        var text = cboDrugMSUoM.GetText();
        cboDrugMSUoM.SetValue(value);
        return text;
    }

    function onCbpDrugMSEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else {
                var transactionID = s.cpTransactionID;
                onAfterSaveRecordDtSuccess(transactionID);
                $('#containerEntryDrugMS').hide();
                setCustomToolbarVisibility();
            }
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail')
                showToast('Delete Failed', 'Error Message : ' + param[2]);
        }
        else if (param[0] == 'switch') {
            if (param[1] == 'fail')
                showToast('Switch Failed', 'Error Message : ' + param[2]);
        }
        else if (param[0] == 'void') {
            if (param[1] == 'fail')
                showToast('Void Failed', 'Error Message : ' + param[2]);
        }
        else if (param[0] == 'approve') {
            if (param[1] == 'fail')
                showToast('Approve Failed', 'Error Message : ' + param[2]);
        }
        calculateAllTotal();
        hideLoadingPanel();
    }

    //#endregion

    function getDrugMSTotalPatient() {
        return parseFloat($('#<%=hdnDrugMSAllTotalPatient.ClientID %>').val());
    }
    function getDrugMSTotalPayer() {
        return parseFloat($('#<%=hdnDrugMSAllTotalPayer.ClientID %>').val());
    }
</script>
<input type="hidden" id="hdnTempTotalPatient" runat="server" value="" />
<input type="hidden" id="hdnTempTotalPayer" runat="server" value="" />
<input type="hidden" id="hdnIsEditable" runat="server" value="" />
<input type="hidden" id="hdnIsAIOTransactionDrugCtl" runat="server" value="" />
<input type="hidden" id="hdnIsChargesGenerateMCUDrugCtl" runat="server" value="" />
<input type="hidden" id="hdnIsAllowOverIssued" runat="server" value="0" />
<input type="hidden" id="hdnDrugMSTransactionDtID" runat="server" value="" />
<input type="hidden" id="hdnIsDrugChargesJustDistribution" runat="server" value="0" />
<input type="hidden" id="hdnIsEndingAmountRoundingTo100" runat="server" value="0" />
<input type="hidden" id="hdnIsEndingAmountRoundingTo1" runat="server" value="0" />
<input type="hidden" id="hdnIsUsingValidateDigitDecimal" runat="server" value="0" />
<div id="containerEntryDrugMS" style="margin-top: 4px; display: none;">
    <div class="pageTitle">
        <%=GetLabel("Tambah / Ubah Data")%></div>
    <fieldset id="fsTrxDrugMS" style="margin: 0">
        <table class="tblEntryDetail">
            <tr>
                <td>
                    <table>
                        <colgroup>
                            <col style="width: 150px" />
                            <col style="width: 200px" />
                            <col style="width: 200px" />
                            <col style="width: 200px" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label>
                                    <%=GetLabel("Lokasi")%></label>
                            </td>
                            <td colspan="3">
                                <dxe:ASPxComboBox ID="cboDrugMSLocation" ClientInstanceName="cboDrugMSLocation" Width="100%"
                                    runat="server" OnCallback="cboDrugMSLocation_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ hideLoadingPanel(); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink lblMandatory" id="lblDrugMSItem">
                                    <%=GetLabel("Obat")%></label>
                            </td>
                            <td colspan="3">
                                <input type="hidden" value="" id="hdnBusinessPartnerID" runat="server" />
                                <input type="hidden" value="" id="hdnLedDrugMSItemFilterExpression" runat="server" />
                                <input type="hidden" value="" id="hdnDrugMSItemID" runat="server" />
                                <input type="hidden" value="" id="hdnDrugMSBaseTariff" runat="server" />
                                <input type="hidden" value="" id="hdnDrugMSConversionValue" runat="server" />
                                <input type="hidden" value="" id="hdnDrugMSConversionValueOld" runat="server" />
                                <input type="hidden" value="" id="hdnDrugMSCoverageAmount" runat="server" />
                                <input type="hidden" value="0" id="hdnDrugMSCostAmount" runat="server" />
                                <input type="hidden" value="" id="hdnDrugMSIsCoverageInPercentage" runat="server" />
                                <input type="hidden" value="0" id="hdnDrugMSDiscountAmount" runat="server" />
                                <input type="hidden" value="0" id="hdnDrugMSDiscountAmountComp1" runat="server" />
                                <input type="hidden" value="0" id="hdnDrugMSDiscountAmountComp2" runat="server" />
                                <input type="hidden" value="0" id="hdnDrugMSDiscountAmountComp3" runat="server" />
                                <input type="hidden" value="0" id="hdnDrugMSIsDiscountUsedComp" runat="server" />
                                <input type="hidden" value="0" id="hdnDrugMSIsDicountInPercentage" runat="server" />
                                <input type="hidden" value="0" id="hdnDrugMSIsDicountInPercentageComp1" runat="server" />
                                <input type="hidden" value="0" id="hdnDrugMSIsDicountInPercentageComp2" runat="server" />
                                <input type="hidden" value="0" id="hdnDrugMSIsDicountInPercentageComp3" runat="server" />
                                <input type="hidden" value="0" id="hdnIsFromEdit" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtDrugMSItemCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDrugMSItemName" ReadOnly="true" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label>
                                    <%=GetLabel("Kelas Tagihan")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboDrugMSChargeClassID" ClientInstanceName="cboDrugMSChargeClassID"
                                    Width="200px" runat="server">
                                    <ClientSideEvents ValueChanged="function(s,e){ onCboDrugMSChargeClassIDValueChanged(); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label>
                                    <%=GetLabel("Harga Satuan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDrugMSUnitTariff" ReadOnly="true" CssClass="txtCurrency" Width="100%"
                                    runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <div class="lblComponent">
                                    <%=GetLabel("Digunakan")%></div>
                            </td>
                            <td>
                                <div class="lblComponent">
                                    <%=GetLabel("Dibebankan")%></div>
                            </td>
                            <td>
                                <div class="lblComponent">
                                    <%=GetLabel("Satuan")%></div>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label>
                                    <%=GetLabel("Jumlah")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDrugMSQtyUsed" Width="100%" CssClass="number min" min="0.01"
                                    runat="server" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtDrugMSQtyCharged" Width="100%" CssClass="number min" min="0.01"
                                    runat="server" />
                            </td>
                            <td>
                                <input type="hidden" value="" id="hdnDrugMSDefaultUoM" runat="server" />
                                <dxe:ASPxComboBox ID="cboDrugMSUoM" runat="server" ClientInstanceName="cboDrugMSUoM"
                                    Width="200px" OnCallback="cboDrugMSUoM_Callback">
                                    <ClientSideEvents EndCallback="function(s,e) { onCboDrugMSUomEndCallback(); }" ValueChanged="function(s,e){ setDrugMSItemUnitConversionText(); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <div class="lblComponent">
                                    <%=GetLabel("Jumlah")%></div>
                            </td>
                            <td>
                                <div class="lblComponent">
                                    <%=GetLabel("Konversi")%></div>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label>
                                    <%=GetLabel("Jumlah Satuan Kecil")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDrugMSBaseQty" ReadOnly="true" CssClass="number" Width="100%"
                                    runat="server" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtDrugMSConversion" ReadOnly="true" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <div class="lblComponent">
                                    <%=GetLabel("Harga")%></div>
                            </td>
                            <td>
                                <div class="lblComponent">
                                    <%=GetLabel("Diskon")%></div>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label>
                                    <%=GetLabel("Harga")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDrugMSPriceTariff" ReadOnly="true" CssClass="txtCurrency" Width="100%"
                                    runat="server" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtDrugMSPriceDiscount" CssClass="txtCurrency" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <div class="lblComponent">
                                    <%=GetLabel("Pasien")%></div>
                            </td>
                            <td>
                                <div class="lblComponent">
                                    <%=GetLabel("Instansi")%></div>
                            </td>
                            <td>
                                <div class="lblComponent">
                                    <%=GetLabel("Total")%></div>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label>
                                    <%=GetLabel("Total")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDrugMSPatient" CssClass="txtCurrency" Width="100%" runat="server" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtDrugMSPayer" CssClass="txtCurrency" Width="100%" runat="server" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtDrugMSTotal" ReadOnly="true" CssClass="txtCurrency" Width="100%"
                                    runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table>
                        <tr>
                            <td>
                                <input type="button" id="btnDrugMSSave" value='<%= GetLabel("Save")%>' />
                            </td>
                            <td>
                                <input type="button" id="btnDrugMSCancel" value='<%= GetLabel("Cancel")%>' />
                            </td>
                        </tr>
                    </table>
                    <img style="float: left; margin-right: 10px;" src='<%= ResolveUrl("~/Libs/Images/Button/info.png")%>'
                        alt='' />
                    <label class="lblInfo">
                        Obat dan Alkes yang di-Save akan langsung di-Approve.</label>
                </td>
            </tr>
        </table>
    </fieldset>
</div>
<dxcp:ASPxCallbackPanel ID="cbpDrugMS" runat="server" Width="100%" ClientInstanceName="cbpDrugMS"
    ShowLoadingPanel="false" OnCallback="cbpDrugMS_Callback">
    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e) { onCbpDrugMSEndCallback(s); }" />
    <PanelCollection>
        <dx:PanelContent ID="PanelContent2" runat="server">
            <asp:Panel runat="server" ID="Panel1" Style="width: 100%; margin-left: auto; margin-right: auto;
                position: relative; font-size: 0.95em;">
                <input type="hidden" id="hdnDrugMSAllTotalPatient" runat="server" value="" />
                <input type="hidden" id="hdnDrugMSAllTotalPayer" runat="server" value="" />
                <asp:ListView ID="lvwDrugMS" runat="server">
                    <LayoutTemplate>
                        <table id="tblView" runat="server" class="grdDrugMS grdNormal notAllowSelect" cellspacing="0"
                            rules="all">
                            <tr>
                                <th style="width: 80px" rowspan="2">
                                </th>
                                <th rowspan="2">
                                    <div style="text-align: left; padding-left: 3px">
                                        <%=GetLabel("Item")%>
                                    </div>
                                </th>
                                <th rowspan="2" style="width: 70px">
                                    <div style="text-align: left; padding-left: 3px">
                                        <%=GetLabel("Kelas Tagihan")%>
                                    </div>
                                </th>
                                <th style="width: 80px" rowspan="2">
                                    <div style="text-align: right; padding-right: 3px">
                                        <%=GetLabel("Harga Satuan")%>
                                    </div>
                                </th>
                                <th colspan="3">
                                    <%=GetLabel("JUMLAH")%>
                                </th>
                                <th colspan="2" style="display: none">
                                    <%=GetLabel("Jumlah Satuan Kecil")%>
                                </th>
                                <th rowspan="2" style="width: 55px">
                                    <div style="text-align: right; padding-right: 3px">
                                        <%=GetLabel("Harga")%>
                                    </div>
                                </th>
                                <th rowspan="2" style="width: 55px">
                                    <div style="text-align: right; padding-right: 3px">
                                        <%=GetLabel("Diskon")%>
                                    </div>
                                </th>
                                <th colspan="3">
                                    <%=GetLabel("TOTAL")%>
                                </th>
                                <th rowspan="2" style="width: 90px">
                                    <div style="text-align: right; padding-right: 3px">
                                        <%=GetLabel("Petugas")%>
                                    </div>
                                </th>
                                <th rowspan="2">
                                    &nbsp;
                                </th>
                            </tr>
                            <tr>
                                <th style="width: 50px">
                                    <div style="text-align: center; padding-right: 3px">
                                        <%=GetLabel("Digunakan")%>
                                    </div>
                                </th>
                                <th style="width: 50px">
                                    <div style="text-align: center; padding-right: 3px">
                                        <%=GetLabel("Dibebankan")%>
                                    </div>
                                </th>
                                <th style="width: 70px">
                                    <div style="text-align: left; padding-right: 3px">
                                        <%=GetLabel("Satuan")%>
                                    </div>
                                </th>
                                <th style="width: 50px; display: none">
                                    <div style="text-align: center; padding-right: 3px">
                                        <%=GetLabel("Jumlah")%>
                                    </div>
                                </th>
                                <th style="width: 150px; display: none">
                                    <div style="text-align: center; padding-right: 3px">
                                        <%=GetLabel("Konversi")%>
                                    </div>
                                </th>
                                <th style="width: 80px">
                                    <div style="text-align: right; padding-right: 3px">
                                        <%=GetLabel("Instansi")%>
                                    </div>
                                </th>
                                <th style="width: 80px">
                                    <div style="text-align: right; padding-right: 3px">
                                        <%=GetLabel("Pasien")%>
                                    </div>
                                </th>
                                <th style="width: 80px">
                                    <div style="text-align: right; padding-right: 3px">
                                        <%=GetLabel("Total")%>
                                    </div>
                                </th>
                            </tr>
                            <tr runat="server" id="itemPlaceholder">
                            </tr>
                            <tr id="Tr1" class="trFooter" runat="server">
                                <td colspan="9" align="right" style="padding-right: 3px">
                                    <%=GetLabel("TOTAL") %>
                                </td>
                                <td align="right" style="padding-right: 9px" id="tdDrugMSTotalPayer" class="tdDrugMSTotalPayer"
                                    runat="server">
                                </td>
                                <td align="right" style="padding-right: 9px" id="tdDrugMSTotalPatient" class="tdDrugMSTotalPatient"
                                    runat="server">
                                </td>
                                <td align="right" style="padding-right: 9px" id="tdDrugMSTotal" class="tdDrugMSTotal"
                                    runat="server">
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr>
                            <td align="center">
                                <div>
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="width: 24px">
                                                <img class="imgDrugMSVerified" <%# IsEditable.ToString() == "True" && Eval("IsVerified").ToString() == "True" ? "" : "style='display:none'"%>
                                                    title='<%=GetLabel("Verified")%>' src='<%# ResolveUrl("~/Libs/Images/Button/verify.png")%>'
                                                    alt="" />
                                                <img class="imgDrugMSApprove imgLink" <%# IsEditable.ToString() == "False" || Eval("IsVerified").ToString() == "True" || (Eval("IsVerified").ToString() == "False" && Eval("IsApproved").ToString() == "True") ? "style='display:none'" : ""%>
                                                    title='<%=GetLabel("Approve This Item")%>' src='<%# ResolveUrl("~/Libs/Images/Button/unlock.png")%>'
                                                    alt="" />
                                                <img class="imgDrugMSVoid imgLink" <%# IsEditable.ToString() == "False" || Eval("IsVerified").ToString() == "True" || (Eval("IsVerified").ToString() == "False" && Eval("IsApproved").ToString() == "False") ? "style='display:none'" : ""%>
                                                    title='<%=GetLabel("Void This Item")%>' src='<%# ResolveUrl("~/Libs/Images/Button/lock.png")%>'
                                                    alt="" />
                                            </td>
                                            <td style="width: 1px">
                                                &nbsp;
                                            </td>
                                            <td style="width: 24px">
                                                <img class="imgDrugMSEdit imgLink" <%# Eval("IsApproved").ToString() == "True" || Eval("IsReviewed").ToString() == "True" || Eval("IsVerified").ToString() == "True" ? "style='display:none'" : ""%>
                                                    title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                    alt="" />
                                            </td>
                                            <td style="width: 1px">
                                                &nbsp;
                                            </td>
                                            <td style="width: 24px">
                                                <img class="imgDrugMSDelete imgLink" <%# Eval("IsApproved").ToString() == "True" || Eval("IsReviewed").ToString() == "True" || Eval("IsVerified").ToString() == "True" ? "style='display:none'" : ""%>
                                                    title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                    alt="" />
                                            </td>
                                        </tr>
                                    </table>
                                    <input type="hidden" value='<%#: Eval("ID") %>' bindingfield="ID" />
                                    <input type="hidden" value='<%#: Eval("LocationID") %>' bindingfield="LocationID" />
                                    <input type="hidden" value='<%#: Eval("ItemID") %>' bindingfield="ItemID" />
                                    <input type="hidden" value='<%#: Eval("ItemCode") %>' bindingfield="ItemCode" />
                                    <input type="hidden" value='<%#: Eval("ItemName1") %>' bindingfield="ItemName1" />
                                    <input type="hidden" value='<%#: Eval("ChargeClassID") %>' bindingfield="ChargeClassID" />
                                    <input type="hidden" value='<%#: Eval("GCItemUnit") %>' bindingfield="GCItemUnit" />
                                    <input type="hidden" value='<%#: Eval("ItemName1") %>' bindingfield="ItemName1" />
                                    <input type="hidden" value='<%#: Eval("BaseQuantity") %>' bindingfield="BaseQuantity" />
                                    <input type="hidden" value='<%#: Eval("UsedQuantity") %>' bindingfield="UsedQuantity" />
                                    <input type="hidden" value='<%#: Eval("ChargedQuantity") %>' bindingfield="ChargedQuantity" />
                                    <input type="hidden" value='<%#: Eval("BaseTariff") %>' bindingfield="BaseTariff" />
                                    <input type="hidden" value='<%#: Eval("CostAmount") %>' bindingfield="CostAmount" />
                                    <input type="hidden" value='<%#: Eval("Tariff") %>' bindingfield="Tariff" />
                                    <input type="hidden" value='<%#: Eval("ConversionFactor") %>' bindingfield="ConversionFactor" />
                                    <input type="hidden" value='<%#: Eval("IsDiscount") %>' bindingfield="IsDiscount" />
                                    <input type="hidden" value='<%#: Eval("DiscountAmount") %>' bindingfield="DiscountAmount" />
                                    <input type="hidden" value='<%#: Eval("IsDiscountInPercentageComp1") %>' bindingfield="IsDiscountInPercentageComp1" />
                                    <input type="hidden" value='<%#: Eval("DiscountPercentageComp1") %>' bindingfield="DiscountPercentageComp1" />
                                    <input type="hidden" value='<%#: Eval("DiscountComp1") %>' bindingfield="DiscountComp1" />
                                    <input type="hidden" value='<%#: Eval("IsDiscountInPercentageComp2") %>' bindingfield="IsDiscountInPercentageComp2" />
                                    <input type="hidden" value='<%#: Eval("DiscountPercentageComp2") %>' bindingfield="DiscountPercentageComp2" />
                                    <input type="hidden" value='<%#: Eval("DiscountComp2") %>' bindingfield="DiscountComp2" />
                                    <input type="hidden" value='<%#: Eval("IsDiscountInPercentageComp3") %>' bindingfield="IsDiscountInPercentageComp3" />
                                    <input type="hidden" value='<%#: Eval("DiscountPercentageComp3") %>' bindingfield="DiscountPercentageComp3" />
                                    <input type="hidden" value='<%#: Eval("DiscountComp3") %>' bindingfield="DiscountComp3" />
                                    <input type="hidden" value='<%#: Eval("PatientAmount") %>' bindingfield="PatientAmount" />
                                    <input type="hidden" value='<%#: Eval("PayerAmount") %>' bindingfield="PayerAmount" />
                                    <input type="hidden" value='<%#: Eval("LineAmount") %>' bindingfield="LineAmount" />
                                    <input type="hidden" value='<%#: Eval("GCBaseUnit") %>' bindingfield="GCBaseUnit" />
                                    <input type="hidden" value='<%#: Eval("GrossLineAmount") %>' bindingfield="GrossLineAmount" />
                                </div>
                            </td>
                            <td>
                                <div style="padding: 3px">
                                    <div>
                                        <%#: Eval("ItemName1")%></div>
                                    <div>
                                        <span style="font-style: italic">
                                            <%#: Eval("ItemCode") %>
                                        </span>
                                    </div>
                                </div>
                            </td>
                            <td>
                                <div style="padding: 3px;">
                                    <div>
                                        <%#: Eval("ChargeClassName")%></div>
                                </div>
                            </td>
                            <td>
                                <div style="padding: 3px; text-align: right;">
                                    <div>
                                        <%#: Eval("Tariff", "{0:N}")%></div>
                                </div>
                            </td>
                            <td>
                                <div style="padding: 3px; text-align: right;">
                                    <div>
                                        <%#: Eval("UsedQuantity")%></div>
                                </div>
                            </td>
                            <td>
                                <div style="padding: 3px; text-align: right;">
                                    <div>
                                        <%#: Eval("ChargedQuantity")%></div>
                                </div>
                            </td>
                            <td>
                                <div style="padding: 3px;">
                                    <div>
                                        <%#: Eval("ItemUnit")%></div>
                                </div>
                            </td>
                            <td style="display: none">
                                <div style="padding: 3px; text-align: right;">
                                    <div>
                                        <%#: Eval("BaseQuantity")%></div>
                                </div>
                            </td>
                            <td style="display: none">
                                <div style="padding: 3px;">
                                    <div>
                                        <%#: Eval("Conversion")%></div>
                                </div>
                            </td>
                            <td>
                                <div style="padding: 3px; text-align: right;">
                                    <div>
                                        <%#: Eval("GrossLineAmount", "{0:N}")%></div>
                                </div>
                            </td>
                            <td>
                                <div style="padding: 3px; text-align: right;">
                                    <div>
                                        <%#: Eval("DiscountAmount", "{0:N}")%></div>
                                </div>
                            </td>
                            <td>
                                <div style="padding: 3px; text-align: right;">
                                    <div>
                                        <%#: Eval("PayerAmount", "{0:N}")%></div>
                                </div>
                            </td>
                            <td>
                                <div style="padding: 3px; text-align: right;">
                                    <div>
                                        <%#: Eval("PatientAmount", "{0:N}")%></div>
                                </div>
                            </td>
                            <td>
                                <div style="padding: 3px; text-align: right;">
                                    <div>
                                        <%#: Eval("LineAmount", "{0:N}")%></div>
                                </div>
                            </td>
                            <td>
                                <div style="padding-right: 3px; text-align: right;">
                                    <div>
                                        <%#: Eval("CreatedByUserName")%></div>
                                    <div>
                                        <%#: Eval("CreatedDateInString")%></div>
                                </div>
                            </td>
                            <td <%# IsShowSwitchIcon.ToString() == "True" && IsEditable.ToString() == "True" ?  "" : "style='display:none'" %>
                                valign="middle">
                                <img class="imgDrugMSSwitch imgLink" title='<%=GetLabel("Switch")%>' src='<%# ResolveUrl("~/Libs/Images/Button/switch.png")%>'
                                    alt="" />
                            </td>
                            <td <%# Eval("IsUnitPriceOverLimit").ToString() == "True" ? "" : "style='display:none'" %>
                                valign="middle">
                                <img src='<%# Eval("IsUnitPriceOverLimit").ToString() == "True" && Eval("IsConfirmed").ToString() == "True" ? ResolveUrl("~/Libs/Images/Status/coverage_ok.png") : ResolveUrl("~/Libs/Images/Status/coverage_warning.png")%>'
                                    title='<%=GetLabel("ServiceUnitPrice = ")%><%# Eval("cfServiceUnitPriceInString") %><%=GetLabel("\nDrugSuppliesUnitPrice = ")%><%# Eval("cfDrugSuppliesUnitPriceInString") %><%=GetLabel("\nLogisticUnitPrice = ")%><%# Eval("cfLogisticUnitPriceInString") %>'
                                    alt="" style="width: 30px" />
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
                <div class="imgLoadingGrdView" id="Div2">
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
                <div style="width: 100%; text-align: center">
                    <span class="lblLink" id="lblDrugMSAddData" style="margin-right: 200px;">
                        <%= GetLabel("Tambah Data")%></span> 
                    <span class="lblLink" id="lblDrugMSQuickPick">
                        <%= GetLabel("Quick Picks")%></span>
                </div>
            </asp:Panel>
        </dx:PanelContent>
    </PanelCollection>
</dxcp:ASPxCallbackPanel>
