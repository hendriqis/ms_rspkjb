<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AppointmentDraftTransactionDetailLogisticCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.AppointmentDraftTransactionDetailLogisticCtl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<script type="text/javascript" id="dxss_ptdrgmsctl">
    //#region Logistic
    function onLoadLogistic() {
        if ($('#<%=hdnIsEditable.ClientID %>').val() == '1') {
            $('#lblLogisticAddData').show();
            $('#lblLogisticQuickPick').show();
        }
        else {
            $('#lblLogisticAddData').hide();
            $('#lblLogisticQuickPick').hide();
        }

        $('#btnLogisticSave').click(function (evt) {
            if (IsValid(evt, 'fsTrxLogistic', 'mpTrxLogistic'))
                cbpLogistic.PerformCallback('save');
            return false;
        });

        $('#btnLogisticCancel').click(function () {
            $('#containerEntryLogistic').hide();

            var totalAllPayer = parseFloat($('#<%=hdnTempTotalPayer.ClientID %>').val());
            $('#<%=hdnLogisticAllTotalPayer.ClientID %>').val(totalAllPayer);
            $('.tdLogisticTotalPayer').html(totalAllPayer.formatMoney(2, '.', ','));

            var totalAllPatient = parseFloat($('#<%=hdnTempTotalPatient.ClientID %>').val());
            $('#<%=hdnLogisticAllTotalPatient.ClientID %>').val(totalAllPatient);
            $('.tdLogisticTotalPatient').html(totalAllPatient.formatMoney(2, '.', ','));

            var totalLineAmount = totalAllPatient + totalAllPayer;
            $('.tdLogisticTotal').html(totalLineAmount.formatMoney(2, '.', ','));

            calculateAllTotal();
        });

        $('#lblLogisticQuickPick').live('click', function () {
            if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
                showLoadingPanel();
                var url = ResolveUrl('~/Libs/Program/Module/PatientManagement/Transaction/DrugsLogisticsQuickPicksCtl.ascx');
                var transactionID = getTransactionHdID();
                var locationID = getLogisticLocationID();
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

        $('#<%=txtLogisticQtyUsed.ClientID %>').change(function () {
            $('#<%=txtLogisticQtyCharged.ClientID %>').val($(this).val());
            $('#<%=txtLogisticQtyCharged.ClientID %>').change();
        });

        $('#<%=txtLogisticQtyCharged.ClientID %>').change(function () {
            var conversionValue = parseFloat($('#<%=hdnLogisticConversionValue.ClientID %>').val());
            var qty = parseFloat($(this).val());
            $('#<%=txtLogisticBaseQty.ClientID %>').val(conversionValue * qty);
            calculateLogisticTariffTotal();
            calculateLogisticDiscountTotal();
            calculateLogisticTotal();
        });

        $('#<%=txtLogisticPatient.ClientID %>').change(function () {
            var patientTotal = parseInt($(this).val());
            var total = parseInt($('#<%=txtLogisticTotal.ClientID %>').attr('hiddenVal'));
            var payerTotal = total - patientTotal;
            $('#<%=txtLogisticPayer.ClientID %>').val(payerTotal).trigger('changeValue');
        });

        $('#<%=txtLogisticPayer.ClientID %>').change(function () {
            var payerTotal = parseInt($(this).val());
            var total = parseInt($('#<%=txtLogisticTotal.ClientID %>').attr('hiddenVal'));
            var patientTotal = total - payerTotal;
            $('#<%=txtLogisticPatient.ClientID %>').val(patientTotal).trigger('changeValue');
        });
    }

    //#region Entry Drug MS

    var isEditLogistic = false;

    $('.imgLogisticApprove.imgLink').die('click');
    $('.imgLogisticApprove.imgLink').live('click', function () {
        $row = $(this).closest('tr').parent().closest('tr');
        var obj = rowToObject($row);
        cbpLogistic.PerformCallback('approve|' + obj.ID);
    });

    $('.imgLogisticVoid.imgLink').die('click');
    $('.imgLogisticVoid.imgLink').live('click', function () {
        $row = $(this).closest('tr').parent().closest('tr');
        var obj = rowToObject($row);
        cbpLogistic.PerformCallback('void|' + obj.ID);
    });

    $('.imgLogisticSwitch.imgLink').die('click');
    $('.imgLogisticSwitch.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        var obj = rowToObject($row);
        cbpLogistic.PerformCallback('switch|' + obj.ID);
    });

    $('.imgLogisticDelete.imgLink').die('click');
    $('.imgLogisticDelete.imgLink').live('click', function () {
        $row = $(this).closest('tr').parent().closest('tr');
        showDeleteConfirmation(function (data) {
            var obj = rowToObject($row);
            var param = 'delete|' + obj.ID + ';' + data.GCDeleteReason + ';' + data.Reason;
            cbpLogistic.PerformCallback(param);
        });
    });

    $('.imgLogisticEdit.imgLink').die('click');
    $('.imgLogisticEdit.imgLink').live('click', function () {
        $('#containerEntryLogistic').show();
        showLoadingPanel();

        $('#<%=hdnTempTotalPatient.ClientID %>').val(getLogisticTotalPatient());
        $('#<%=hdnTempTotalPayer.ClientID %>').val(getLogisticTotalPayer());

        cboLogisticLocation.SetEnabled(false);
        $row = $(this).closest('tr').parent().closest('tr');
        var obj = rowToObject($row);

        cboLogisticLocation.SetValue(obj.LocationID);
        cboLogisticChargeClassID.SetValue(obj.ChargeClassID);
        $('#<%=hdnLogisticItemID.ClientID %>').val(obj.ItemID);
        $('#<%=txtLogisticItemCode.ClientID %>').val(obj.ItemCode);
        $('#<%=txtLogisticItemName.ClientID %>').val(obj.ItemName1);
        $('#<%=hdnLogisticTransactionDtID.ClientID %>').val(obj.ID);
        $('#<%=txtLogisticPatient.ClientID %>').val(obj.PatientAmount).trigger('changeValue');
        $('#<%=txtLogisticPayer.ClientID %>').val(obj.PayerAmount).trigger('changeValue');
        $('#<%=txtLogisticTotal.ClientID %>').val(obj.LineAmount).trigger('changeValue');

        $('#<%=txtLogisticUnitTariff.ClientID %>').val(parseFloat(obj.Tariff)).trigger('changeValue');
        $('#<%=hdnLogisticConversionValue.ClientID %>').val(obj.BaseQuantity);
        $('#<%=txtLogisticQtyUsed.ClientID %>').val(obj.UsedQuantity);
        $('#<%=txtLogisticQtyCharged.ClientID %>').val(obj.ChargedQuantity);
        $('#<%=txtLogisticBaseQty.ClientID %>').val(obj.BaseQuantity);
        $('#<%=txtLogisticPriceTariff.ClientID %>').val(parseFloat(obj.GrossLineAmount)).trigger('changeValue');
        $('#<%=txtLogisticPriceDiscount.ClientID %>').val(obj.DiscountAmount).trigger('changeValue');
        $('#<%=hdnLogisticConversionValue.ClientID %>').val(obj.ConversionFactor);
        $('#<%=hdnLogisticDefaultUoM.ClientID %>').val(obj.GCBaseUnit);
        $('#<%=txtLogisticConversion.ClientID %>').val('');
        isEditLogistic = true;
        objLogistic = obj;
        cboLogisticUoM.PerformCallback();

        $('#<%=txtLogisticQtyUsed.ClientID %>').focus();

        var registrationID = getRegistrationID();
        var visitID = getVisitID();
        var classID = obj.ChargeClassID;
        var trxDate = getTrxDate();
        Methods.getItemTariff(registrationID, visitID, classID, obj.ItemID, trxDate, function (result) {
            $('#<%=hdnLogisticDiscountAmount.ClientID %>').val(result.DiscountAmount);
            $('#<%=hdnLogisticCoverageAmount.ClientID %>').val(result.CoverageAmount);
            $('#<%=hdnLogisticIsDicountInPercentage.ClientID %>').val(result.IsDiscountInPercentage ? '1' : '0');
            $('#<%=hdnLogisticIsCoverageInPercentage.ClientID %>').val(result.IsCoverageInPercentage ? '1' : '0');
            $('#<%=hdnLogisticCostAmount.ClientID %>').val(result.CostAmount);
            hideLoadingPanel();
        });
    });

    var objLogistic = null;
    $('#lblLogisticAddData').live('click', function () {
        if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
            $('#<%=hdnTempTotalPatient.ClientID %>').val(getLogisticTotalPatient());
            $('#<%=hdnTempTotalPayer.ClientID %>').val(getLogisticTotalPayer());

            if (typeof onAddRecordSetControlDisabled == 'function')
                onAddRecordSetControlDisabled();
            $('#containerEntryLogistic').show();
            //showLoadingPanel();
            cboLogisticLocation.SetEnabled(true);

            cboLogisticChargeClassID.SetValue(getClassID());
            cboLogisticUoM.ClearItems();
            cboLogisticUoM.SetValue('');

            $('#<%=hdnLogisticTransactionDtID.ClientID %>').val('');
            $('#<%=hdnLogisticItemID.ClientID %>').val('');
            $('#<%=txtLogisticItemCode.ClientID %>').val('');
            $('#<%=txtLogisticItemName.ClientID %>').val('');
            $('#<%=txtLogisticPatient.ClientID %>').val('0').trigger('changeValue');
            $('#<%=txtLogisticPayer.ClientID %>').val('0').trigger('changeValue');
            $('#<%=txtLogisticTotal.ClientID %>').val('0').trigger('changeValue');
            $('#<%=txtLogisticUnitTariff.ClientID %>').val('0').trigger('changeValue');

            $('#<%=hdnLogisticConversionValue.ClientID %>').val('0');
            $('#<%=txtLogisticQtyUsed.ClientID %>').val('1');
            $('#<%=txtLogisticQtyCharged.ClientID %>').val('1');
            $('#<%=txtLogisticBaseQty.ClientID %>').val('');
            $('#<%=txtLogisticPriceTariff.ClientID %>').val('0').trigger('changeValue');
            $('#<%=txtLogisticPriceDiscount.ClientID %>').val('0').trigger('changeValue');
            $('#<%=hdnLogisticConversionValue.ClientID %>').val('0');
            $('#<%=hdnLogisticDefaultUoM.ClientID %>').val('');
            $('#<%=txtLogisticConversion.ClientID %>').val('');
            $('#<%=hdnLogisticCostAmount.ClientID %>').val('0');

            cboLogisticLocation.SetFocus();
            hideLoadingPanel();
        }
    });
    //#endregion

    //#region Item
    function onGetLogisticItemFilterExpression() {
        var locationID = cboLogisticLocation.GetValue();
        var filterExpression = $('#<%=hdnLedLogisticItemFilterExpression.ClientID %>').val().replace('[LocationID]', locationID);
        if ($('#<%=hdnIsAllowOverIssued.ClientID %>').val() == '0') {
            filterExpression += " AND GCItemType = '" + Constant.ItemGroupMaster.LOGISTIC + "' AND QuantityEND > 0 ";
        }
        filterExpression += " AND GCItemStatus != 'X181^999'";
        return filterExpression;
    }

    $('#lblLogisticItem.lblLink').live('click', function () {
        openSearchDialog('itembalance', onGetLogisticItemFilterExpression(), function (value) {
            $('#<%=txtLogisticItemCode.ClientID %>').val(value);
            onTxtLogisticItemCodeChanged(value);
        });
    });

    $('#<%=txtLogisticItemCode.ClientID %>').live('change', function () {
        onTxtLogisticItemCodeChanged($(this).val());
    });

    function onTxtLogisticItemCodeChanged(value) {
        var filterExpression = onGetLogisticItemFilterExpression() + " AND ItemCode = '" + value + "'";
        Methods.getObject('GetvItemBalanceList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnLogisticItemID.ClientID %>').val(result.ItemID);
                $('#<%=txtLogisticItemName.ClientID %>').val(result.ItemName1);
                cboLogisticUoM.PerformCallback();
            }
            else {
                $('#<%=hdnLogisticItemID.ClientID %>').val('');
                $('#<%=txtLogisticItemCode.ClientID %>').val('');
                $('#<%=txtLogisticItemName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    function setLogisticItemFilterExpression(transactionID) {
        $('#<%=hdnLedLogisticItemFilterExpression.ClientID %>').val("LocationID = [LocationID] AND GCItemType = '" + Constant.ItemGroupMaster.LOGISTIC + "' AND ItemID NOT IN (SELECT ItemID FROM PatientChargesDt WHERE TransactionID = " + transactionID + " AND IsDeleted = 0) AND IsDeleted = 0");
    }

    function onCboLogisticUomEndCallback() {
        if (!isEditLogistic) {
            cboLogisticUoM.SetValue('');
            getLogisticTariff();
        }
        else {
            cboLogisticUoM.SetValue(objLogistic.GCItemUnit);
            var conversionFactor = $('#<%=hdnLogisticConversionValue.ClientID %>').val();
            var defaultUoM = $('#<%=hdnLogisticDefaultUoM.ClientID %>').val();
            var currUoM = cboLogisticUoM.GetValue();
            var fromConversion = getLogisticItemUnitName(defaultUoM);
            var toConversion = getLogisticItemUnitName(currUoM);

            var conversion = "1 " + toConversion + " = " + conversionFactor + " " + fromConversion;
            $('#<%=txtLogisticConversion.ClientID %>').val(conversion);
        }
        hideLoadingPanel();
    }

    function getLogisticTariff() {
        showLoadingPanel();
        var itemID = $('#<%=hdnLogisticItemID.ClientID %>').val();
        var registrationID = getRegistrationID();
        var visitID = getVisitID();
        var classID = cboLogisticChargeClassID.GetValue();
        var trxDate = getTrxDate();

        Methods.getItemTariff(registrationID, visitID, classID, itemID, trxDate, function (result) {
            if (result != null) {
                cboLogisticUoM.SetValue(result.GCItemUnit);
                $('#<%=hdnLogisticBaseTariff.ClientID %>').val(result.BasePrice)
                $('#<%=txtLogisticUnitTariff.ClientID %>').val(result.Price).trigger('changeValue');
                $('#<%=hdnLogisticDefaultUoM.ClientID %>').val(result.GCItemUnit);
                $('#<%=hdnLogisticCostAmount.ClientID %>').val(result.CostAmount)

                $('#<%=hdnLogisticDiscountAmount.ClientID %>').val(result.DiscountAmount);
                $('#<%=hdnLogisticCoverageAmount.ClientID %>').val(result.CoverageAmount);
                $('#<%=hdnLogisticIsDicountInPercentage.ClientID %>').val(result.IsDiscountInPercentage ? '1' : '0');
                $('#<%=hdnLogisticIsCoverageInPercentage.ClientID %>').val(result.IsCoverageInPercentage ? '1' : '0');
                setLogisticItemUnitConversionText();
            }
            else {
                $('#<%=hdnLogisticDefaultUoM.ClientID %>').val('');
                $('#<%=hdnLogisticBaseTariff.ClientID %>').val('0')
                $('#<%=txtLogisticUnitTariff.ClientID %>').val('0').trigger('changeValue');
                $('#<%=hdnLogisticConversionValue.ClientID %>').val('0');
                $('#<%=txtLogisticConversion.ClientID %>').val('');
                $('#<%=hdnLogisticCostAmount.ClientID %>').val('0');

                $('#<%=hdnLogisticDiscountAmount.ClientID %>').val('0');
                $('#<%=hdnLogisticCoverageAmount.ClientID %>').val('0');
                $('#<%=hdnLogisticIsDicountInPercentage.ClientID %>').val('0');
                $('#<%=hdnLogisticIsCoverageInPercentage.ClientID %>').val('0');
                cboLogisticUoM.SetValue('');
            }
            calculateLogisticTariffTotal();
            calculateLogisticDiscountTotal();

            calculateLogisticTotal();
        });
        hideLoadingPanel();
    }

    function onCboLogisticChargeClassIDValueChanged() {
        getLogisticTariff();
    }

    function setLogisticItemUnitConversionText() {
        var defaultUoM = $('#<%=hdnLogisticDefaultUoM.ClientID %>').val();
        var currUoM = cboLogisticUoM.GetValue();
        var fromConversion = getLogisticItemUnitName(defaultUoM);
        if (defaultUoM == currUoM) {
            $('#<%=hdnLogisticConversionValue.ClientID %>').val('1');
            $('#<%=txtLogisticQtyCharged.ClientID %>').change();
            var conversion = "1 " + fromConversion + " = 1 " + fromConversion;
            $('#<%=txtLogisticConversion.ClientID %>').val(conversion);
        }
        else {
            var itemID = $('#<%=hdnLogisticItemID.ClientID %>').val();
            var filterExpression = "ItemID = " + itemID + " AND GCAlternateUnit = '" + currUoM + "'";
            Methods.getObjectValue('GetvItemAlternateUnitList', filterExpression, 'ConversionFactor', function (result) {
                var toConversion = getLogisticItemUnitName(currUoM);
                $('#<%=hdnLogisticConversionValue.ClientID %>').val(result);
                $('#<%=txtLogisticQtyCharged.ClientID %>').change();
                var conversion = "1 " + toConversion + " = " + result + " " + fromConversion;
                $('#<%=txtLogisticConversion.ClientID %>').val(conversion);
            });
        }
    }

    function calculateLogisticTariffTotal() {
        var tariff = parseFloat($('#<%=txtLogisticUnitTariff.ClientID %>').attr('hiddenVal'));
        var qty = parseFloat($('#<%=txtLogisticBaseQty.ClientID %>').val());
        $('#<%=txtLogisticPriceTariff.ClientID %>').val(tariff * qty).trigger('changeValue');
    }

    function calculateLogisticDiscountTotal() {
        var discountAmount = parseInt($('#<%=hdnLogisticDiscountAmount.ClientID %>').val());
        var isDicountInPercentage = ($('#<%=hdnLogisticIsDicountInPercentage.ClientID %>').val() == '1');

        var discountTotal = 0;
        var tariff = parseFloat($('#<%=txtLogisticPriceTariff.ClientID %>').attr('hiddenVal'));
        if (isDicountInPercentage)
            discountTotal = (tariff * discountAmount) / 100;
        else {
            var qty = parseFloat($('#<%=txtLogisticBaseQty.ClientID %>').val());
            discountTotal = discountAmount * qty;
        }
        if (discountTotal > tariff)
            discountTotal = tariff;
        $('#<%=txtLogisticPriceDiscount.ClientID %>').val(discountTotal).trigger('changeValue');
    }

    function calculateLogisticTotal() {
        var tariff = parseInt($('#<%=txtLogisticPriceTariff.ClientID %>').attr('hiddenVal'));
        var discount = parseInt($('#<%=txtLogisticPriceDiscount.ClientID %>').attr('hiddenVal'));
        var total = tariff - discount;

        var coverageAmount = parseInt($('#<%=hdnLogisticCoverageAmount.ClientID %>').val());
        var isCoverageInPercentage = ($('#<%=hdnLogisticIsCoverageInPercentage.ClientID %>').val() == '1');
        var totalPayer = 0;
        if (isCoverageInPercentage)
            totalPayer = (total * coverageAmount) / 100;
        else {
            var qty = parseFloat($('#<%=txtLogisticBaseQty.ClientID %>').val());
            totalPayer = coverageAmount * qty;
        }
        if (totalPayer > total)
            totalPayer = total;
        var totalPatient = total - totalPayer;

        var totalAllPayer = parseFloat($('#<%=hdnLogisticAllTotalPayer.ClientID %>').val());
        totalAllPayer = (totalAllPayer - parseFloat($('#<%=txtLogisticPayer.ClientID %>').attr('hiddenVal')));
        totalAllPayer += totalPayer;
        $('#<%=hdnLogisticAllTotalPayer.ClientID %>').val(totalAllPayer);
        $('.tdLogisticTotalPayer').html(totalAllPayer.formatMoney(2, '.', ','));

        var totalAllPatient = parseFloat($('#<%=hdnLogisticAllTotalPatient.ClientID %>').val());
        totalAllPatient = (totalAllPatient - parseInt($('#<%=txtLogisticPatient.ClientID %>').attr('hiddenVal')));
        totalAllPatient += totalPatient;
        $('#<%=hdnLogisticAllTotalPatient.ClientID %>').val(totalAllPatient);
        $('.tdLogisticTotalPatient').html(totalAllPatient.formatMoney(2, '.', ','));

        var totalLineAmount = totalAllPatient + totalAllPayer;
        $('.tdLogisticTotal').html(totalLineAmount.formatMoney(2, '.', ','));

        $('#<%=txtLogisticPayer.ClientID %>').val(totalPayer).trigger('changeValue');
        $('#<%=txtLogisticPatient.ClientID %>').val(totalPatient).trigger('changeValue');
        $('#<%=txtLogisticTotal.ClientID %>').val(total).trigger('changeValue');

        calculateAllTotal();
    }

    function getLogisticItemUnitName(itemUnitID) {
        var value = cboLogisticUoM.GetValue();
        cboLogisticUoM.SetValue(itemUnitID);
        var text = cboLogisticUoM.GetText();
        cboLogisticUoM.SetValue(value);
        return text;
    }

    function onCbpLogisticEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else {
                var transactionID = s.cpTransactionID;
                onAfterSaveRecordDtSuccess(transactionID);
                $('#containerEntryLogistic').hide();
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

    function getLogisticTotalPatient() {
        return parseFloat($('#<%=hdnLogisticAllTotalPatient.ClientID %>').val());
    }
    function getLogisticTotalPayer() {
        return parseFloat($('#<%=hdnLogisticAllTotalPayer.ClientID %>').val());
    }
</script>


<input type="hidden" id="hdnTempTotalPatient" runat="server" value="" />
<input type="hidden" id="hdnTempTotalPayer" runat="server" value="" />
<input type="hidden" id="hdnLogisticTransactionDtID" runat="server" value="" />
<input type="hidden" id="hdnIsEditable" runat="server" value="" />
<input type="hidden" value="0" id="hdnIsAllowOverIssued" runat="server" />
<div id="containerEntryLogistic" style="margin-top:4px;display:none;">
    <div class="pageTitle"><%=GetLabel("Tambah Atau Ubah Data")%></div>
    <fieldset id="fsTrxLogistic" style="margin:0"> 
        <table class="tblEntryDetail">
            <tr>
                <td>
                    <table>
                        <colgroup>
                            <col style="width:120px"/>
                            <col style="width:200px"/>
                            <col style="width:200px"/>
                            <col style="width:200px"/>
                        </colgroup>
                        <tr>
                            <td class="tdLabel"><label><%=GetLabel("Lokasi")%></label></td>
                            <td colspan="3">
                                <dxe:ASPxComboBox ID="cboLogisticLocation" ClientInstanceName="cboLogisticLocation" Width="100%" runat="server" OnCallback="cboLogisticLocation_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" 
                                        EndCallback="function(s,e){ hideLoadingPanel(); }" />
                                </dxe:ASPxComboBox> 
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblMandatory lblLink" id="lblLogisticItem"><%=GetLabel("Barang Umum")%></label></td>
                            <td colspan="3">
                                <input type="hidden" value="" id="hdnLedLogisticItemFilterExpression" runat="server" />
                                <input type="hidden" value="" id="hdnLogisticItemID" runat="server" />
                                <input type="hidden" value="" id="hdnLogisticBaseTariff" runat="server" />
                                <input type="hidden" value="" id="hdnLogisticConversionValue" runat="server" />
                                <input type="hidden" value="" id="hdnLogisticDiscountAmount" runat="server" />
                                <input type="hidden" value="" id="hdnLogisticCoverageAmount" runat="server" />
                                <input type="hidden" value="0" id="hdnLogisticCostAmount" runat="server" />
                                <input type="hidden" value="" id="hdnLogisticIsDicountInPercentage" runat="server" />
                                <input type="hidden" value="" id="hdnLogisticIsCoverageInPercentage" runat="server" />
                                <table style="width:100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width:30%"/>
                                        <col style="width:3px"/>
                                        <col/>
                                    </colgroup>
                                    <tr>
                                        <td><asp:TextBox ID="txtLogisticItemCode" Width="100%" runat="server" /></td>
                                        <td>&nbsp;</td>
                                        <td><asp:TextBox ID="txtLogisticItemName" ReadOnly="true" Width="100%" runat="server" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label><%=GetLabel("Charge Class")%></label></td>
                            <td>
                                <dxe:ASPxComboBox ID="cboLogisticChargeClassID" ClientInstanceName="cboLogisticChargeClassID" Width="200px" runat="server">
                                    <ClientSideEvents ValueChanged="function(s,e){ onCboLogisticChargeClassIDValueChanged(); }" />
                                </dxe:ASPxComboBox>
                            </td> 
                        </tr> 
                        <tr>
                            <td class="tdLabel"><label><%=GetLabel("Harga Satuan")%></label></td>
                            <td><asp:TextBox ID="txtLogisticUnitTariff" ReadOnly="true" CssClass="txtCurrency" Width="100%" runat="server" /></td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td><div class="lblComponent"><%=GetLabel("Digunakan")%></div></td>
                            <td><div class="lblComponent"><%=GetLabel("Dibebankan")%></div></td>
                            <td><div class="lblComponent"><%=GetLabel("Satuan")%></div></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label><%=GetLabel("Jumlah")%></label></td>
                            <td><asp:TextBox ID="txtLogisticQtyUsed" Width="100%" CssClass="number min" min="0.1" runat="server" /></td>
                            <td><asp:TextBox ID="txtLogisticQtyCharged" Width="100%" CssClass="number min" min="0.1" runat="server" /></td>
                            <td>
                                <input type="hidden" value="" id="hdnLogisticDefaultUoM" runat="server" />
                                <dxe:ASPxComboBox ID="cboLogisticUoM" runat="server" ClientInstanceName="cboLogisticUoM" Width="200px" OnCallback="cboLogisticUoM_Callback">
                                    <ClientSideEvents EndCallback="function(s,e) { onCboLogisticUomEndCallback(); }" 
                                        ValueChanged="function(s,e){ setLogisticItemUnitConversionText(); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td><div class="lblComponent"><%=GetLabel("Jumlah")%></div></td>
                            <td><div class="lblComponent"><%=GetLabel("Konversi")%></div></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label><%=GetLabel("Base Quantity")%></label></td>
                            <td><asp:TextBox ID="txtLogisticBaseQty" ReadOnly="true" CssClass="number" Width="100%" runat="server" /></td>
                            <td><asp:TextBox ID="txtLogisticConversion" ReadOnly="true" Width="100%" runat="server" /></td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td><div class="lblComponent"><%=GetLabel("Tarif")%></div></td>
                            <td><div class="lblComponent"><%=GetLabel("Diskon")%></div></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label><%=GetLabel("Harga")%></label></td>
                            <td><asp:TextBox ID="txtLogisticPriceTariff" ReadOnly="true" CssClass="txtCurrency" Width="100%" runat="server" /></td>
                            <td><asp:TextBox ID="txtLogisticPriceDiscount" ReadOnly="true" CssClass="txtCurrency" Width="100%" runat="server" /></td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td><div class="lblComponent"><%=GetLabel("Pasien")%></div></td>
                            <td><div class="lblComponent"><%=GetLabel("Instansi")%></div></td>
                            <td><div class="lblComponent"><%=GetLabel("Total")%></div></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label><%=GetLabel("Total")%></label></td>
                            <td><asp:TextBox ID="txtLogisticPatient" CssClass="txtCurrency" Width="100%" runat="server" /></td>
                            <td><asp:TextBox ID="txtLogisticPayer" CssClass="txtCurrency" Width="100%" runat="server" /></td>
                            <td><asp:TextBox ID="txtLogisticTotal" ReadOnly="true" CssClass="txtCurrency" Width="100%" runat="server" /></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table>
                        <tr>
                            <td>
                                <input type="button" id="btnLogisticSave" value='<%= GetLabel("Save")%>' />
                            </td>
                            <td>
                                <input type="button" id="btnLogisticCancel" value='<%= GetLabel("Cancel")%>' />
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
<dxcp:ASPxCallbackPanel ID="cbpLogistic" runat="server" Width="100%" ClientInstanceName="cbpLogistic"
    ShowLoadingPanel="false" OnCallback="cbpLogistic_Callback">
    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
        EndCallback="function(s,e) { onCbpLogisticEndCallback(s); }" />
    <PanelCollection>
        <dx:PanelContent ID="PanelContent2" runat="server">
            <asp:Panel runat="server" ID="Panel1" Style="width: 100%; margin-left: auto; margin-right: auto; position: relative;font-size:0.95em;">
                <input type="hidden" id="hdnLogisticAllTotalPatient" runat="server" value="" />
                <input type="hidden" id="hdnLogisticAllTotalPayer" runat="server" value="" />
                <asp:ListView ID="lvwLogistic" runat="server">
                    <LayoutTemplate>                                
                        <table id="tblView" runat="server" class="grdLogistic grdNormal notAllowSelect" cellspacing="0" rules="all" >
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
                                <th colspan="3"><%=GetLabel("Jumlah")%></th>
                                <th colspan="2" style="display:none"><%=GetLabel("Jumlah Satuan Kecil")%></th>
                                <th rowspan="2" style="width:55px">
                                    <div style="text-align:right;padding-right:3px">
                                        <%=GetLabel("Tariff")%>
                                    </div>
                                </th>
                                <th rowspan="2" style="width:55px">
                                    <div style="text-align:right;padding-right:3px">
                                        <%=GetLabel("Diskon")%>
                                    </div>
                                </th>
                                <th colspan="3"><%=GetLabel("Total")%></th>
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
                                    <div style="text-align:center;padding-right:3px">
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
                                <td align="right" style="padding-right:9px" class="tdLogisticTotalPayer" id="tdLogisticTotalPayer" runat="server"></td>
                                <td align="right" style="padding-right:9px" class="tdLogisticTotalPatient" id="tdLogisticTotalPatient" runat="server"></td>
                                <td align="right" style="padding-right:9px" class="tdLogisticTotal" id="tdLogisticTotal" runat="server"></td>
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
                                                <img class="imgLogisticVerified" <%# IsEditable.ToString() == "True" && Eval("IsVerified").ToString() == "True" ? "" : "style='display:none'"%> title='<%=GetLabel("Verified")%>' src='<%# ResolveUrl("~/Libs/Images/Button/verify.png")%>' alt="" />
                                                <img class="imgLogisticApprove imgLink" <%# IsEditable.ToString() == "False" || Eval("IsVerified").ToString() == "True" || (Eval("IsVerified").ToString() == "False" && Eval("IsApproved").ToString() == "True") ? "style='display:none'" : ""%> title='<%=GetLabel("Approve This Item")%>' src='<%# ResolveUrl("~/Libs/Images/Button/unlock.png")%>' alt="" />
                                                <img class="imgLogisticVoid imgLink" <%# IsEditable.ToString() == "False" || Eval("IsVerified").ToString() == "True" || (Eval("IsVerified").ToString() == "False" && Eval("IsApproved").ToString() == "False") ? "style='display:none'" : ""%> title='<%=GetLabel("Void This Item")%>' src='<%# ResolveUrl("~/Libs/Images/Button/lock.png")%>' alt="" />
                                            </td>
                                            <td style="width:1px">&nbsp;</td>
                                            <td style="width:24px"><img class="imgLogisticEdit imgLink" <%# IsEditable.ToString() == "False" || Eval("IsApproved").ToString() == "True" || Eval("IsReviewed").ToString() == "True" ? "style='display:none'" : ""%> title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>' alt="" /></td>
                                            <td style="width:1px">&nbsp;</td>
                                            <td style="width:24px"><img class="imgLogisticDelete imgLink" <%# IsEditable.ToString() == "False" || Eval("IsApproved").ToString() == "True" || Eval("IsReviewed").ToString() == "True" ? "style='display:none'" : ""%> title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>' alt="" /></td>
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
                                <img style="margin-left: 2px" class="imgLogisticSwitch imgLink" title='<%=GetLabel("Switch")%>' src='<%# ResolveUrl("~/Libs/Images/Button/switch.png")%>' alt="" />
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
                <div class="imgLoadingGrdView" id="Div2">
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
                <div style="width:100%;text-align:center">
                    <span class="lblLink" id="lblLogisticAddData" style="margin-right: 200px;"><%= GetLabel("Tambah Data")%></span>
                    <span class="lblLink" id="lblLogisticQuickPick"><%= GetLabel("Quick Picks")%></span>
                </div>
            </asp:Panel>
        </dx:PanelContent>
    </PanelCollection>
</dxcp:ASPxCallbackPanel>