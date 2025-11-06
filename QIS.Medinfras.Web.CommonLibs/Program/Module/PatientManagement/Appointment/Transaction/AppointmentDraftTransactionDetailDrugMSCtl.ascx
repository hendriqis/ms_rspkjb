<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AppointmentDraftTransactionDetailDrugMSCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.AppointmentDraftTransactionDetailDrugMSCtl" %>

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
        }
        else {
            $('#lblDrugMSAddData').hide();
            $('#lblDrugMSQuickPick').hide();
        }

        $('#btnDrugMSSave').click(function (evt) {
            if (IsValid(evt, 'fsTrxDrugMS', 'mpTrxDrugMS')) {
                cbpDrugMS.PerformCallback('save');
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
                var isAccompany = "0";
                if (typeof isAccompanyChargesPage == 'function') {
                    if (isAccompanyChargesPage()) {
                        isAccompany = "1";
                    }
                }
                var id = transactionID + '|' + locationID + '|' + visitID + '|' + registrationID + '|' + GCItemType + '|' + departmentID + '|' + serviceUnitID + '|' + isAccompany;
                openUserControlPopup(url, id, 'Quick Picks', 1000, 600);
            }
        });

        $('#<%=txtDrugMSQtyUsed.ClientID %>').change(function () {
            $('#<%=txtDrugMSQtyCharged.ClientID %>').val($(this).val());
            $('#<%=txtDrugMSQtyCharged.ClientID %>').change();
        });

        $('#<%=txtDrugMSQtyCharged.ClientID %>').change(function () {
            var conversionValue = parseFloat($('#<%=hdnDrugMSConversionValue.ClientID %>').val());
            var qty = parseFloat($(this).val());
            $('#<%=txtDrugMSBaseQty.ClientID %>').val(conversionValue * qty);
            calculateDrugMSTariffTotal();
            calculateDrugMSDiscountTotal();
            calculateDrugMSTotal();
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

    //#region Entry Drug MS
    var isEditDrugMS = false;

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
        $row = $(this).closest('tr').parent().closest('tr');
        showDeleteConfirmation(function (data) {
            var obj = rowToObject($row);
            var param = 'delete|' + obj.ID + ';' + data.GCDeleteReason + ';' + data.Reason;
            cbpDrugMS.PerformCallback(param);
        });
    });

    $('.imgDrugMSSwitch.imgLink').die('click');
    $('.imgDrugMSSwitch.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        var obj = rowToObject($row);
        cbpDrugMS.PerformCallback('switch|' + obj.ID);
    });

    $('.imgDrugMSEdit.imgLink').die('click');
    $('.imgDrugMSEdit.imgLink').live('click', function () {
        $('#containerEntryDrugMS').show();
        showLoadingPanel();

        $('#<%=hdnTempTotalPatient.ClientID %>').val(getDrugMSTotalPatient());
        $('#<%=hdnTempTotalPayer.ClientID %>').val(getDrugMSTotalPayer());

        cboDrugMSLocation.SetEnabled(false);
        $row = $(this).closest('tr').parent().closest('tr');
        var obj = rowToObject($row);

        cboDrugMSLocation.SetValue(obj.LocationID);
        cboDrugMSChargeClassID.SetValue(obj.ChargeClassID);

        //$('#lblDrugMSItem').attr('class', 'lblDisabled');
        $('#<%=txtDrugMSBaseQty.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtDrugMSItemCode.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtDrugMSConversion.ClientID %>').attr('readonly', 'readonly');
        //$('#<%=txtDrugMSPatient.ClientID %>').attr('readonly', 'readonly');
        //$('#<%=txtDrugMSPayer.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtDrugMSTotal.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtDrugMSPriceTariff.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtDrugMSPriceDiscount.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtDrugMSUnitTariff.ClientID %>').attr('readonly', 'readonly');

        $('#<%=hdnDrugMSItemID.ClientID %>').val(obj.ItemID);
        $('#<%=txtDrugMSItemCode.ClientID %>').val(obj.ItemCode);
        $('#<%=txtDrugMSItemName.ClientID %>').val(obj.ItemName1);
        $('#<%=hdnDrugMSTransactionDtID.ClientID %>').val(obj.ID);
        $('#<%=txtDrugMSPatient.ClientID %>').val(obj.PatientAmount).trigger('changeValue');
        $('#<%=txtDrugMSPayer.ClientID %>').val(obj.PayerAmount).trigger('changeValue');
        $('#<%=txtDrugMSTotal.ClientID %>').val(obj.LineAmount).trigger('changeValue');

        $('#<%=txtDrugMSUnitTariff.ClientID %>').val(parseFloat(obj.Tariff)).trigger('changeValue');
        $('#<%=hdnDrugMSConversionValue.ClientID %>').val(obj.BaseQuantity);
        $('#<%=txtDrugMSQtyUsed.ClientID %>').val(obj.UsedQuantity);
        $('#<%=txtDrugMSQtyCharged.ClientID %>').val(obj.ChargedQuantity);
        $('#<%=txtDrugMSBaseQty.ClientID %>').val(obj.BaseQuantity);
        $('#<%=txtDrugMSPriceTariff.ClientID %>').val(parseFloat(obj.GrossLineAmount)).trigger('changeValue');
        $('#<%=txtDrugMSPriceDiscount.ClientID %>').val(obj.DiscountAmount).trigger('changeValue');
        $('#<%=hdnDrugMSConversionValue.ClientID %>').val(obj.ConversionFactor);
        $('#<%=hdnDrugMSDefaultUoM.ClientID %>').val(obj.GCBaseUnit);
        $('#<%=txtDrugMSConversion.ClientID %>').val('');
        isEditDrugMS = true;
        objDrugMS = obj;
        cboDrugMSUoM.PerformCallback();

        $('#<%=txtDrugMSQtyUsed.ClientID %>').focus();

        var registrationID = getRegistrationID();
        var visitID = getVisitID();
        var classID = obj.ChargeClassID;
        var trxDate = getTrxDate();
        Methods.getItemTariff(registrationID, visitID, classID, obj.ItemID, trxDate, function (result) {
            $('#<%=hdnDrugMSDiscountAmount.ClientID %>').val(result.DiscountAmount);
            $('#<%=hdnDrugMSCoverageAmount.ClientID %>').val(result.CoverageAmount);
            $('#<%=hdnDrugMSIsDicountInPercentage.ClientID %>').val(result.IsDiscountInPercentage ? '1' : '0');
            $('#<%=hdnDrugMSIsCoverageInPercentage.ClientID %>').val(result.IsCoverageInPercentage ? '1' : '0');
            $('#<%=hdnDrugMSCostAmount.ClientID %>').val(result.CostAmount);
            hideLoadingPanel();
        });
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

            //$('#lblDrugMSItem').attr('class', 'lblLink');
            $('#<%=txtDrugMSItemCode.ClientID %>').removeAttr('readonly');
            $('#<%=txtDrugMSBaseQty.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtDrugMSConversion.ClientID %>').attr('readonly', 'readonly');
            //$('#<%=txtDrugMSPatient.ClientID %>').attr('readonly', 'readonly');
            //$('#<%=txtDrugMSPayer.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtDrugMSTotal.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtDrugMSPriceTariff.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtDrugMSPriceDiscount.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtDrugMSUnitTariff.ClientID %>').attr('readonly', 'readonly');

            $('#<%=hdnDrugMSTransactionDtID.ClientID %>').val('');
            $('#<%=hdnDrugMSItemID.ClientID %>').val('');
            $('#<%=txtDrugMSItemCode.ClientID %>').val('');
            $('#<%=txtDrugMSItemName.ClientID %>').val('');
            cboDrugMSChargeClassID.SetValue(getClassID());
            $('#<%=txtDrugMSPatient.ClientID %>').val('0').trigger('changeValue');
            $('#<%=txtDrugMSPayer.ClientID %>').val('0').trigger('changeValue');
            $('#<%=txtDrugMSTotal.ClientID %>').val('0').trigger('changeValue');
            $('#<%=txtDrugMSUnitTariff.ClientID %>').val('0').trigger('changeValue');

            $('#<%=hdnDrugMSConversionValue.ClientID %>').val('0');
            $('#<%=txtDrugMSQtyUsed.ClientID %>').val('1');
            $('#<%=txtDrugMSQtyCharged.ClientID %>').val('1');
            $('#<%=txtDrugMSBaseQty.ClientID %>').val('');
            $('#<%=txtDrugMSPriceTariff.ClientID %>').val('0').trigger('changeValue');
            $('#<%=txtDrugMSPriceDiscount.ClientID %>').val('0').trigger('changeValue');
            $('#<%=hdnDrugMSConversionValue.ClientID %>').val('0');
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
        if (!isEditDrugMS) {
            cboDrugMSUoM.SetValue('');
            getDrugMSTariff();
        }
        else {
            cboDrugMSUoM.SetValue(objDrugMS.GCItemUnit);
            var conversionFactor = $('#<%=hdnDrugMSConversionValue.ClientID %>').val();
            var defaultUoM = $('#<%=hdnDrugMSDefaultUoM.ClientID %>').val();
            var currUoM = cboDrugMSUoM.GetValue();
            var fromConversion = getDrugMSItemUnitName(defaultUoM);
            var toConversion = getDrugMSItemUnitName(currUoM);

            var conversion = "1 " + toConversion + " = " + conversionFactor + " " + fromConversion;
            $('#<%=txtDrugMSConversion.ClientID %>').val(conversion);

            getDrugMSTariff();
        }
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
                $('#<%=hdnDrugMSCoverageAmount.ClientID %>').val(result.CoverageAmount);
                $('#<%=hdnDrugMSIsDicountInPercentage.ClientID %>').val(result.IsDiscountInPercentage ? '1' : '0');
                $('#<%=hdnDrugMSIsCoverageInPercentage.ClientID %>').val(result.IsCoverageInPercentage ? '1' : '0');
                setDrugMSItemUnitConversionText();
            }
            else {
                $('#<%=hdnDrugMSDefaultUoM.ClientID %>').val('');
                $('#<%=hdnDrugMSBaseTariff.ClientID %>').val('0');
                $('#<%=txtDrugMSUnitTariff.ClientID %>').val('0').trigger('changeValue');
                $('#<%=hdnDrugMSConversionValue.ClientID %>').val('0');
                $('#<%=txtDrugMSConversion.ClientID %>').val('');

                $('#<%=hdnDrugMSCostAmount.ClientID %>').val('0');

                $('#<%=hdnDrugMSDiscountAmount.ClientID %>').val('0');
                $('#<%=hdnDrugMSCoverageAmount.ClientID %>').val('0');
                $('#<%=hdnDrugMSIsDicountInPercentage.ClientID %>').val('0');
                $('#<%=hdnDrugMSIsCoverageInPercentage.ClientID %>').val('0');
                cboDrugMSUoM.SetValue('');
            }
            calculateDrugMSTariffTotal();
            calculateDrugMSDiscountTotal();

            calculateDrugMSTotal();
        });
        hideLoadingPanel();
    }

    function onCboDrugMSChargeClassIDValueChanged() {
        getDrugMSTariff();
    }

    function setDrugMSItemUnitConversionText() {
        var defaultUoM = $('#<%=hdnDrugMSDefaultUoM.ClientID %>').val();
        var currUoM = cboDrugMSUoM.GetValue();
        var fromConversion = getDrugMSItemUnitName(defaultUoM);
        if (defaultUoM == currUoM) {
            $('#<%=hdnDrugMSConversionValue.ClientID %>').val('1');
            $('#<%=txtDrugMSQtyCharged.ClientID %>').change();
            var conversion = "1 " + fromConversion + " = 1 " + fromConversion;
            $('#<%=txtDrugMSConversion.ClientID %>').val(conversion);
        }
        else {
            var itemID = $('#<%=hdnDrugMSItemID.ClientID %>').val();
            var filterExpression = "ItemID = " + itemID + " AND GCAlternateUnit = '" + currUoM + "'";
            Methods.getObjectValue('GetvItemAlternateUnitList', filterExpression, 'ConversionFactor', function (result) {
                var toConversion = getDrugMSItemUnitName(currUoM);
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
        var discountAmount = parseInt($('#<%=hdnDrugMSDiscountAmount.ClientID %>').val());
        var isDicountInPercentage = ($('#<%=hdnDrugMSIsDicountInPercentage.ClientID %>').val() == '1');

        var discountTotal = 0;
        if (discountAmount > 0) {
            var tariff = parseFloat($('#<%=txtDrugMSPriceTariff.ClientID %>').attr('hiddenVal'));
            if (isDicountInPercentage)
                discountTotal = (tariff * discountAmount) / 100;
            else {
                var qty = parseFloat($('#<%=txtDrugMSBaseQty.ClientID %>').val());
                discountTotal = discountAmount * qty;
            }
            if (discountTotal > tariff)
                discountTotal = tariff;
        }
        $('#<%=txtDrugMSPriceDiscount.ClientID %>').val(discountTotal).trigger('changeValue');
    }

    function calculateDrugMSTotal() {
        var tariff = parseInt($('#<%=txtDrugMSPriceTariff.ClientID %>').attr('hiddenVal'));
        var discount = parseInt($('#<%=txtDrugMSPriceDiscount.ClientID %>').attr('hiddenVal'));
        var total = tariff - discount;

        var coverageAmount = parseInt($('#<%=hdnDrugMSCoverageAmount.ClientID %>').val());
        var isCoverageInPercentage = ($('#<%=hdnDrugMSIsCoverageInPercentage.ClientID %>').val() == '1');
        var totalPayer = 0;
        if (isCoverageInPercentage)
            totalPayer = (total * coverageAmount) / 100;
        else {
            var qty = parseFloat($('#<%=txtDrugMSBaseQty.ClientID %>').val());
            totalPayer = coverageAmount * qty;
        }
        if (total > 0 && totalPayer > total)
            totalPayer = total;
        var totalPatient = total - totalPayer;

        var totalAllPayer = parseFloat($('#<%=hdnDrugMSAllTotalPayer.ClientID %>').val());
        totalAllPayer = (totalAllPayer - parseFloat($('#<%=txtDrugMSPayer.ClientID %>').attr('hiddenVal')));
        totalAllPayer += totalPayer;
        $('#<%=hdnDrugMSAllTotalPayer.ClientID %>').val(totalAllPayer);
        $('.tdDrugMSTotalPayer').html(totalAllPayer.formatMoney(2, '.', ','));

        var totalAllPatient = parseFloat($('#<%=hdnDrugMSAllTotalPatient.ClientID %>').val());
        totalAllPatient = (totalAllPatient - parseInt($('#<%=txtDrugMSPatient.ClientID %>').attr('hiddenVal')));
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
<input type="hidden" value="0" id="hdnIsAllowOverIssued" runat="server" />

<input type="hidden" id="hdnDrugMSTransactionDtID" runat="server" value="" />
<div id="containerEntryDrugMS" style="margin-top:4px;display:none;">
    <div class="pageTitle"><%=GetLabel("Tambah Atau Ubah Data")%></div>
    <fieldset id="fsTrxDrugMS" style="margin:0"> 
        <table class="tblEntryDetail">
            <tr>
                <td>
                    <table>
                        <colgroup>
                            <col style="width:100px"/>
                            <col style="width:200px"/>
                            <col style="width:200px"/>
                            <col style="width:200px"/>
                        </colgroup>
                        <tr>
                            <td class="tdLabel"><label><%=GetLabel("Lokasi")%></label></td>
                            <td colspan="3">
                                <dxe:ASPxComboBox ID="cboDrugMSLocation" ClientInstanceName="cboDrugMSLocation" Width="100%" runat="server" OnCallback="cboDrugMSLocation_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" 
                                        EndCallback="function(s,e){ hideLoadingPanel(); }" />
                                </dxe:ASPxComboBox> 
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblLink" id="lblDrugMSItem"><%=GetLabel("Obat")%></label></td>
                            <td colspan="3">
                                <input type="hidden" value="" id="hdnBusinessPartnerID" runat="server" />
                                <input type="hidden" value="" id="hdnLedDrugMSItemFilterExpression" runat="server" />
                                <input type="hidden" value="" id="hdnDrugMSItemID" runat="server" />
                                <input type="hidden" value="" id="hdnDrugMSBaseTariff" runat="server" />
                                <input type="hidden" value="" id="hdnDrugMSConversionValue" runat="server" />
                                <input type="hidden" value="" id="hdnDrugMSDiscountAmount" runat="server" />
                                <input type="hidden" value="" id="hdnDrugMSCoverageAmount" runat="server" />
                                <input type="hidden" value="0" id="hdnDrugMSCostAmount" runat="server" />
                                <input type="hidden" value="" id="hdnDrugMSIsDicountInPercentage" runat="server" />
                                <input type="hidden" value="" id="hdnDrugMSIsCoverageInPercentage" runat="server" />
                                <table style="width:100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width:30%"/>
                                        <col style="width:3px"/>
                                        <col/>
                                    </colgroup>
                                    <tr>
                                        <td><asp:TextBox ID="txtDrugMSItemCode" Width="100%" runat="server" /></td>
                                        <td>&nbsp;</td>
                                        <td><asp:TextBox ID="txtDrugMSItemName" ReadOnly="true" Width="100%" runat="server" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label><%=GetLabel("Kelas Tagihan")%></label></td>
                            <td>
                                <dxe:ASPxComboBox ID="cboDrugMSChargeClassID" ClientInstanceName="cboDrugMSChargeClassID" Width="200px" runat="server">
                                    <ClientSideEvents ValueChanged="function(s,e){ onCboDrugMSChargeClassIDValueChanged(); }" />
                                </dxe:ASPxComboBox>
                            </td> 
                        </tr> 
                        <tr>
                            <td class="tdLabel"><label><%=GetLabel("Harga Satuan")%></label></td>
                            <td><asp:TextBox ID="txtDrugMSUnitTariff" ReadOnly="true" CssClass="txtCurrency" Width="100%" runat="server" /></td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td><div class="lblComponent"><%=GetLabel("Digunakan")%></div></td>
                            <td><div class="lblComponent"><%=GetLabel("Dibebankan")%></div></td>
                            <td><div class="lblComponent"><%=GetLabel("Satuan")%></div></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label><%=GetLabel("Jumlah")%></label></td>
                            <td><asp:TextBox ID="txtDrugMSQtyUsed" Width="100%" CssClass="number min" min="0.1" runat="server" /></td>
                            <td><asp:TextBox ID="txtDrugMSQtyCharged" Width="100%" CssClass="number min" min="0.1" runat="server" /></td>
                            <td>
                                <input type="hidden" value="" id="hdnDrugMSDefaultUoM" runat="server" />
                                <dxe:ASPxComboBox ID="cboDrugMSUoM" runat="server" ClientInstanceName="cboDrugMSUoM" Width="200px" OnCallback="cboDrugMSUoM_Callback">
                                    <ClientSideEvents EndCallback="function(s,e) { onCboDrugMSUomEndCallback(); }" 
                                        ValueChanged="function(s,e){ setDrugMSItemUnitConversionText(); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td><div class="lblComponent"><%=GetLabel("Jumlah")%></div></td>
                            <td><div class="lblComponent"><%=GetLabel("Konversi")%></div></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label><%=GetLabel("Jumlah Satuan Kecil")%></label></td>
                            <td><asp:TextBox ID="txtDrugMSBaseQty" ReadOnly="true" CssClass="number" Width="100%" runat="server" /></td>
                            <td><asp:TextBox ID="txtDrugMSConversion" ReadOnly="true" Width="100%" runat="server" /></td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td><div class="lblComponent"><%=GetLabel("Harga")%></div></td>
                            <td><div class="lblComponent"><%=GetLabel("Diskon")%></div></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label><%=GetLabel("Harga")%></label></td>
                            <td><asp:TextBox ID="txtDrugMSPriceTariff" ReadOnly="true" CssClass="txtCurrency" Width="100%" runat="server" /></td>
                            <td><asp:TextBox ID="txtDrugMSPriceDiscount" ReadOnly="true" CssClass="txtCurrency" Width="100%" runat="server" /></td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td><div class="lblComponent"><%=GetLabel("Pasien")%></div></td>
                            <td><div class="lblComponent"><%=GetLabel("Instansi")%></div></td>
                            <td><div class="lblComponent"><%=GetLabel("Total")%></div></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label><%=GetLabel("Total")%></label></td>
                            <td><asp:TextBox ID="txtDrugMSPatient" CssClass="txtCurrency" Width="100%" runat="server" /></td>
                            <td><asp:TextBox ID="txtDrugMSPayer" CssClass="txtCurrency" Width="100%" runat="server" /></td>
                            <td><asp:TextBox ID="txtDrugMSTotal" ReadOnly="true" CssClass="txtCurrency" Width="100%" runat="server" /></td>
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
                    <img style="float:left;margin-right: 10px;" src='<%= ResolveUrl("~/Libs/Images/Button/info.png")%>' alt='' />
                    <label class="lblInfo">Obat dan Alkes yang di-Save akan langsung di-Approve.</label>
                </td>
            </tr>
        </table>
    </fieldset>
</div>
<dxcp:ASPxCallbackPanel ID="cbpDrugMS" runat="server" Width="100%" ClientInstanceName="cbpDrugMS"
    ShowLoadingPanel="false" OnCallback="cbpDrugMS_Callback">
    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
        EndCallback="function(s,e) { onCbpDrugMSEndCallback(s); }" />
    <PanelCollection>
        <dx:PanelContent ID="PanelContent2" runat="server">
            <asp:Panel runat="server" ID="Panel1" Style="width: 100%; margin-left: auto; margin-right: auto; position: relative;font-size:0.95em;">
                <input type="hidden" id="hdnDrugMSAllTotalPatient" runat="server" value="" />
                <input type="hidden" id="hdnDrugMSAllTotalPayer" runat="server" value="" />
                <asp:ListView ID="lvwDrugMS" runat="server">
                    <LayoutTemplate>                                
                        <table id="tblView" runat="server" class="grdDrugMS grdNormal notAllowSelect" cellspacing="0" rules="all" >
                            <tr>  
                                <th style="width:80px" rowspan="2"></th>
                                <th rowspan="2">
                                    <div style="text-align:left;padding-left:3px">
                                        <%=GetLabel("Item")%>
                                    </div>
                                </th>
                                <th rowspan="2" style="width:70px">
                                    <div style="text-align:left;padding-left:3px">
                                        <%=GetLabel("Kelas Tagihan")%>
                                    </div>
                                </th>
                                <th style="width:80px" rowspan="2">
                                    <div style="text-align:right;padding-right:3px">
                                        <%=GetLabel("Harga Satuan")%>
                                    </div>
                                </th>
                                <th colspan="3"><%=GetLabel("JUMLAH")%></th>
                                <th colspan="2" style="display:none"><%=GetLabel("Jumlah Satuan Kecil")%></th>
                                <th rowspan="2" style="width:55px">
                                    <div style="text-align:right;padding-right:3px">
                                        <%=GetLabel("Harga")%>
                                    </div>
                                </th>
                                <th rowspan="2" style="width:55px">
                                    <div style="text-align:right;padding-right:3px">
                                        <%=GetLabel("Diskon")%>
                                    </div>
                                </th>
                                <th colspan="3"><%=GetLabel("TOTAL")%></th>
                                <th rowspan="2" style="width:90px">
                                    <div style="text-align:right;padding-right:3px">
                                        <%=GetLabel("Petugas")%>
                                    </div>
                                </th> 
                                <th rowspan="2">&nbsp;</th>                                 
                            </tr>
                            <tr>                                
                                <th style="width:50px">
                                    <div style="text-align:center;padding-right:3px">
                                        <%=GetLabel("Digunakan")%>
                                    </div>
                                </th>
                                <th style="width:50px">
                                    <div style="text-align:center;padding-right:3px">
                                        <%=GetLabel("Dibebankan")%>
                                    </div>
                                </th>
                                <th style="width:70px">
                                    <div style="text-align:left;padding-right:3px">
                                        <%=GetLabel("Satuan")%>
                                    </div>
                                </th>
                                <th style="width:50px;display:none">
                                    <div style="text-align:center;padding-right:3px">
                                        <%=GetLabel("Jumlah")%>
                                    </div>
                                </th>
                                <th style="width:150px;display:none">
                                    <div style="text-align:center;padding-right:3px">
                                        <%=GetLabel("Konversi")%>
                                    </div>
                                </th>
                                <th style="width:80px">
                                    <div style="text-align:right;padding-right:3px">
                                        <%=GetLabel("Instansi")%>
                                    </div>
                                </th>
                                <th style="width:80px">
                                    <div style="text-align:right;padding-right:3px">
                                        <%=GetLabel("Pasien")%>
                                    </div>
                                </th>
                                <th style="width:80px">
                                    <div style="text-align:right;padding-right:3px">
                                        <%=GetLabel("Total")%>
                                    </div>
                                </th>
                            </tr>
                            <tr runat="server" id="itemPlaceholder" ></tr>
                            <tr class="trFooter" runat="server">
                                <td colspan="9" align="right" style="padding-right:3px"><%=GetLabel("TOTAL") %></td>
                                <td align="right" style="padding-right:9px" id="tdDrugMSTotalPayer" class="tdDrugMSTotalPayer" runat="server"></td>
                                <td align="right" style="padding-right:9px" id="tdDrugMSTotalPatient" class="tdDrugMSTotalPatient" runat="server"></td>
                                <td align="right" style="padding-right:9px" id="tdDrugMSTotal" class="tdDrugMSTotal" runat="server"></td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                            </tr>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr>
                            <td align="center">
                                <div>
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="width:24px">
                                                <img class="imgDrugMSVerified" <%# IsEditable.ToString() == "True" && Eval("IsVerified").ToString() == "True" ? "" : "style='display:none'"%> title='<%=GetLabel("Verified")%>' src='<%# ResolveUrl("~/Libs/Images/Button/verify.png")%>' alt="" />
                                                <img class="imgDrugMSApprove imgLink" <%# IsEditable.ToString() == "False" || Eval("IsVerified").ToString() == "True" || (Eval("IsVerified").ToString() == "False" && Eval("IsApproved").ToString() == "True") ? "style='display:none'" : ""%> title='<%=GetLabel("Approve This Item")%>' src='<%# ResolveUrl("~/Libs/Images/Button/unlock.png")%>' alt="" />
                                                <img class="imgDrugMSVoid imgLink" <%# IsEditable.ToString() == "False" || Eval("IsVerified").ToString() == "True" || (Eval("IsVerified").ToString() == "False" && Eval("IsApproved").ToString() == "False") ? "style='display:none'" : ""%> title='<%=GetLabel("Void This Item")%>' src='<%# ResolveUrl("~/Libs/Images/Button/lock.png")%>' alt="" />
                                            </td>
                                            <td style="width:1px">&nbsp;</td>
                                            <td style="width:24px"><img class="imgDrugMSEdit imgLink" <%# Eval("IsApproved").ToString() == "True" || Eval("IsReviewed").ToString() == "True" || Eval("IsVerified").ToString() == "True" ? "style='display:none'" : ""%> title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>' alt="" /></td>
                                            <td style="width:1px">&nbsp;</td>
                                            <td style="width:24px"><img class="imgDrugMSDelete imgLink" <%# Eval("IsApproved").ToString() == "True" || Eval("IsReviewed").ToString() == "True" || Eval("IsVerified").ToString() == "True" ? "style='display:none'" : ""%> title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>' alt="" /></td>
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
                                    <input type="hidden" value='<%#: Eval("PatientAmount") %>' bindingfield="PatientAmount" />
                                    <input type="hidden" value='<%#: Eval("PayerAmount") %>' bindingfield="PayerAmount" />
                                    <input type="hidden" value='<%#: Eval("LineAmount") %>' bindingfield="LineAmount" />
                                    <input type="hidden" value='<%#: Eval("GCBaseUnit") %>' bindingfield="GCBaseUnit" />
                                    <input type="hidden" value='<%#: Eval("GrossLineAmount") %>' bindingfield="GrossLineAmount" />                                 
                                </div>
                            </td>
                            <td>
                                <div style="padding:3px">
                                    <div><%#: Eval("ItemName1")%></div>
                                    <div><span style="font-style:italic"><%#: Eval("ItemCode") %> </span></div>                                                                                                                                    
                                </div>
                            </td>
                            <td>
                                <div style="padding:3px;">
                                    <div><%#: Eval("ChargeClassName")%></div>                                                   
                                </div>
                            </td>
                            <td>
                                <div style="padding:3px;text-align:right;">
                                    <div><%#: Eval("Tariff", "{0:N}")%></div>                                                   
                                </div>
                            </td>
                            <td>
                                <div style="padding:3px;text-align:right;">
                                    <div><%#: Eval("UsedQuantity")%></div>                                                   
                                </div>
                            </td>
                            <td>
                                <div style="padding:3px;text-align:right;">
                                    <div><%#: Eval("ChargedQuantity")%></div>                                                   
                                </div>
                            </td>
                            <td>
                                <div style="padding:3px;">
                                    <div><%#: Eval("ItemUnit")%></div>                                                   
                                </div>
                            </td>
                            <td style="display:none">
                                <div style="padding:3px;text-align:right;">
                                    <div><%#: Eval("BaseQuantity")%></div>                                                   
                                </div>
                            </td>
                            <td style="display:none">
                                <div style="padding:3px;">
                                    <div><%#: Eval("Conversion")%></div>                                                   
                                </div>
                            </td>
                            <td>
                                <div style="padding:3px;text-align:right;">
                                    <div><%#: Eval("GrossLineAmount", "{0:N}")%></div>                                                   
                                </div>
                            </td>
                            <td>
                                <div style="padding:3px;text-align:right;">
                                    <div><%#: Eval("DiscountAmount", "{0:N}")%></div>                                                   
                                </div>
                            </td>
                            <td>
                                <div style="padding:3px;text-align:right;">
                                    <div><%#: Eval("PayerAmount", "{0:N}")%></div>                                                   
                                </div>
                            </td>
                            <td>
                                <div style="padding:3px;text-align:right;">
                                    <div><%#: Eval("PatientAmount", "{0:N}")%></div>                                                   
                                </div>
                            </td>
                            <td>
                                <div style="padding:3px;text-align:right;">
                                    <div><%#: Eval("LineAmount", "{0:N}")%></div>                                                   
                                </div>
                            </td>
                            <td>
                                <div style="padding-right:3px;text-align:right;">
                                    <div><%#: Eval("CreatedByUserName")%></div>
                                    <div><%#: Eval("CreatedDateInString")%></div>                                                 
                                </div>
                            </td>
                            <td <%# IsShowSwitchIcon.ToString() == "True" && IsEditable.ToString() == "True" ?  "" : "style='display:none'" %> valign="middle" >
                                <img style="margin-left: 2px" class="imgDrugMSSwitch imgLink" title='<%=GetLabel("Switch")%>' src='<%# ResolveUrl("~/Libs/Images/Button/switch.png")%>' alt="" />
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
                <div class="imgLoadingGrdView" id="Div2">
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
                <div style="width:100%;text-align:center">
                    <span class="lblLink" id="lblDrugMSAddData" style="margin-right: 200px;"><%= GetLabel("Tambah Data")%></span>
                    <span class="lblLink" id="lblDrugMSQuickPick"><%= GetLabel("Quick Picks")%></span>
                </div>
            </asp:Panel>
        </dx:PanelContent>
    </PanelCollection>
</dxcp:ASPxCallbackPanel>