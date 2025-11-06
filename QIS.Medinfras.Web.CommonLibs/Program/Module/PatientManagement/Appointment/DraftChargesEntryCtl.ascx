<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DraftChargesEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.DraftChargesEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dxpc" %>
<script type="text/javascript" id="dxss_drafttransactionctl">
    var isEditDrugMS = false;
    var isEditLogistic = false;
    setDatePicker('<%=txtDraftChargesDate.ClientID %>');
    calculateAllTotal();
    $('#<%=hdnIsServiceTab.ClientID %>').val('1');
    $('#<%=hdnIsDrugTab.ClientID %>').val('0');
    $('#<%=hdnIsLogisticTab.ClientID %>').val('0');


    function getPhysicianID() {
        return $('#<%=hdnPhysicianID.ClientID %>').val();
    }
    function onGetPhysicianFilterExpression() {
        var filterExpression = "ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = '" + $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val() + "') AND IsDeleted = 0";
        return filterExpression;
    }
    function getPhysicianCode() {
        return $('#<%=hdnPhysicianCode.ClientID %>').val();
    }
    function getPhysicianName() {
        return $('#<%=hdnPhysicianName.ClientID %>').val();
    }
    function getHealthcareServiceUnitID() {
        return $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val();
    }
    function getHealthcareServiceUnitAppointmentID() {
        return $('#<%=hdnHealthcareServiceUnitFromID.ClientID %>').val();
    }
    function getDepartmentID() {
        return $('#<%=hdnDepartmentFromID.ClientID %>').val();
    }

    $('#ulTabClinicTransactionCtl li').live('click', function () {
        var name = $(this).attr('contentid');
        if (name == 'containerService') {
            $('#<%=hdnIsServiceTab.ClientID %>').val('1');
            $('#<%=hdnIsDrugTab.ClientID %>').val('0');
            $('#<%=hdnIsLogisticTab.ClientID %>').val('0');
        }
        else if (name == 'containerDrug') {
            $('#<%=hdnIsServiceTab.ClientID %>').val('0');
            $('#<%=hdnIsDrugTab.ClientID %>').val('1');
            $('#<%=hdnIsLogisticTab.ClientID %>').val('0');
        }
        else {
            $('#<%=hdnIsServiceTab.ClientID %>').val('0');
            $('#<%=hdnIsDrugTab.ClientID %>').val('0');
            $('#<%=hdnIsLogisticTab.ClientID %>').val('1');
        }

        $('#containerEntryService').hide();
        $('#containerEntryDrugMS').hide();
        $('#containerEntryLogistic').hide();

        $(this).addClass('selected');
        $('#' + name).removeAttr('style');
        $('#ulTabClinicTransactionCtl li').each(function () {
            var tempNameContainer = $(this).attr('contentid');
            if (tempNameContainer != name) {
                $(this).removeClass('selected');
                $('#' + tempNameContainer).attr('style', 'display:none');
            }
        });
    });

    //#region pelayanan
    function getTrxDate() {
        var date = Methods.getDatePickerDate($('#<%=txtDraftChargesDate.ClientID %>').val());
        var dateInYMD = Methods.dateToYMD(date);
        return dateInYMD;
    }

    function calculateServiceTariffTotal() {
        var tariff = parseFloat($('#<%=txtServiceUnitTariff.ClientID %>').attr('hiddenVal'));
        var qty = parseFloat($('#<%=txtServiceQty.ClientID %>').val());
        $('#<%=txtServiceTariff.ClientID %>').val(tariff * qty).trigger('changeValue');
    }

    $('#<%=chkServiceIsVariable.ClientID %>').change(function () {
        if ($(this).is(':checked')) {
            $('#<%=chkServiceIsUnbilledItem.ClientID %>').prop('checked', false);
            $('#<%=txtServiceUnitTariff.ClientID %>').removeAttr('readonly');
            $('#<%=txtServicePatient.ClientID %>').removeAttr('readonly');
            $('#<%=txtServicePayer.ClientID %>').removeAttr('readonly');

            $('#<%=txtServiceTariffComp1.ClientID %>').removeAttr('readonly');
            $('#<%=txtServiceTariffComp2.ClientID %>').removeAttr('readonly');
            $('#<%=txtServiceTariffComp3.ClientID %>').removeAttr('readonly');
            $('#<%=txtServiceTotalTariff.ClientID %>').removeAttr('readonly');

            calculateServiceTotal();
        }
        else {
            $('#<%=txtServiceTariffComp1.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtServiceTariffComp2.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtServiceTariffComp3.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtServiceTotalTariff.ClientID %>').attr('readonly', 'readonly');

            $('#<%=txtServiceUnitTariff.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtServicePatient.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtServicePayer.ClientID %>').attr('readonly', 'readonly');

            var qty = parseFloat($('#<%=txtServiceQty.ClientID %>').val());
            var priceComp1 = parseInt($('#<%=hdnServicePriceComp1.ClientID %>').val());
            var priceComp2 = parseInt($('#<%=hdnServicePriceComp2.ClientID %>').val());
            var priceComp3 = parseInt($('#<%=hdnServicePriceComp3.ClientID %>').val());
            $('#txtServiceDiscTariffComp1').val(priceComp1).trigger('changeValue');
            $('#txtServiceDiscTariffComp2').val(priceComp2).trigger('changeValue');
            $('#txtServiceDiscTariffComp3').val(priceComp3).trigger('changeValue');
            $('#<%=txtServiceTariffComp1.ClientID %>').val($('#<%=hdnServicePriceComp1.ClientID %>').val()).trigger('changeValue');
            $('#<%=txtServiceTariffComp2.ClientID %>').val($('#<%=hdnServicePriceComp2.ClientID %>').val()).trigger('changeValue');
            $('#<%=txtServiceTariffComp3.ClientID %>').val($('#<%=hdnServicePriceComp3.ClientID %>').val()).trigger('changeValue');
            $('#<%=txtServiceUnitTariff.ClientID %>').val($('#<%=hdnServicePrice.ClientID %>').val()).trigger('changeValue');
            $('#<%=txtServiceUnitTariff.ClientID %>').change();
        }
    });

    $('#<%=chkServiceIsVariable.ClientID %>').live('change', function () {
        if ($(this).is(':checked')) {
            $('#<%=chkServiceIsUnbilledItem.ClientID %>').prop('checked', false);
            $('#<%=txtServiceUnitTariff.ClientID %>').removeAttr('readonly');
            $('#<%=txtServicePatient.ClientID %>').removeAttr('readonly');
            $('#<%=txtServicePayer.ClientID %>').removeAttr('readonly');

            $('#<%=txtServiceTariffComp1.ClientID %>').removeAttr('readonly');
            $('#<%=txtServiceTariffComp2.ClientID %>').removeAttr('readonly');
            $('#<%=txtServiceTariffComp3.ClientID %>').removeAttr('readonly');
            $('#<%=txtServiceTotalTariff.ClientID %>').removeAttr('readonly');

            calculateServiceTotal();
        }
        else {
            $('#<%=txtServiceTariffComp1.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtServiceTariffComp2.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtServiceTariffComp3.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtServiceTotalTariff.ClientID %>').attr('readonly', 'readonly');

            $('#<%=txtServiceUnitTariff.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtServicePatient.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtServicePayer.ClientID %>').attr('readonly', 'readonly');

            var qty = parseFloat($('#<%=txtServiceQty.ClientID %>').val());
            var priceComp1 = parseInt($('#<%=hdnServicePriceComp1.ClientID %>').val());
            var priceComp2 = parseInt($('#<%=hdnServicePriceComp2.ClientID %>').val());
            var priceComp3 = parseInt($('#<%=hdnServicePriceComp3.ClientID %>').val());
            $('#txtServiceDiscTariffComp1').val(priceComp1).trigger('changeValue');
            $('#txtServiceDiscTariffComp2').val(priceComp2).trigger('changeValue');
            $('#txtServiceDiscTariffComp3').val(priceComp3).trigger('changeValue');
            $('#<%=txtServiceTariffComp1.ClientID %>').val($('#<%=hdnServicePriceComp1.ClientID %>').val()).trigger('changeValue');
            $('#<%=txtServiceTariffComp2.ClientID %>').val($('#<%=hdnServicePriceComp2.ClientID %>').val()).trigger('changeValue');
            $('#<%=txtServiceTariffComp3.ClientID %>').val($('#<%=hdnServicePriceComp3.ClientID %>').val()).trigger('changeValue');
            $('#<%=txtServiceUnitTariff.ClientID %>').val($('#<%=hdnServicePrice.ClientID %>').val()).trigger('changeValue');
            $('#<%=txtServiceUnitTariff.ClientID %>').change();
        }
    });

    $('#<%=chkServiceIsCITO.ClientID %>').change(function () {
        if ($(this).is(':checked')) {
            var isCITOInPercentage = ($('#<%=hdnServiceIsCITOInPercentage.ClientID %>').val() == '1');
            var CITOAmount = parseFloat($('#<%=hdnServiceBaseCITOAmount.ClientID %>').val());
            var CITO = 0;
            if (isCITOInPercentage) {
                var tariff = parseFloat($('#<%=txtServiceTariff.ClientID %>').attr('hiddenVal'));
                CITO = (tariff * CITOAmount) / 100;
            }
            else {
                var qty = parseFloat($('#<%=txtServiceQty.ClientID %>').val());
                CITO = CITOAmount * qty;
            }
            $('#<%=txtServiceCITO.ClientID %>').val(CITO).trigger('changeValue');
            $('#txtServiceCITOAmount').val(CITO).trigger('changeValue');
        }
        else {
            $('#<%=txtServiceCITO.ClientID %>').val('0').trigger('changeValue');
            $('#txtServiceCITOAmount').val('0').trigger('changeValue');
        }
        calculateServiceTotal();
    });

    window.onCboServiceChargeClassIDValueChanged = function (s, e) {
        var itemCode = $('#<%=txtServiceItemCode.ClientID %>').val();
        if (itemCode != '') {
            var today = new Date();
            var date = today.getDate() + '-' + (today.getMonth() + 1) + '-' + today.getFullYear();
            var time = today.getHours() + ":" + today.getMinutes();

            var txtDraftChargesDate = $('#<%=txtDraftChargesDate.ClientID %>').val() == "" ? date : $('#<%=txtDraftChargesDate.ClientID %>').val();
            var txtDraftChargesTime = $('#<%=txtDraftChargesTime.ClientID %>').val() == "" ? time : $('#<%=txtDraftChargesTime.ClientID %>').val();
            var hdnChargesHealthcareServiceUnitID = $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val();

            Methods.getItemRevenueSharing(itemCode, $('#<%=hdnServicePhysicianID.ClientID %>').val(), cboServiceChargeClassID.GetValue(), '<%=GetMainParamedicRole() %>', 0, hdnChargesHealthcareServiceUnitID, txtDraftChargesDate, txtDraftChargesTime, function (result) {
                if (result != null)
                    $('#<%=hdnServiceRevenueSharingID.ClientID %>').val(result.RevenueSharingID);
                else
                    $('#<%=hdnServiceRevenueSharingID.ClientID %>').val('');
                getServiceTariff();
            });
        }
    }

    $('#<%=chkServiceIsDiscount.ClientID %>').live('change', function () {
        if ($(this).is(':checked')) {
            $('#<%=txtServiceDiscount.ClientID %>').removeAttr('readonly');
        }
        else {
            $('#<%=txtServiceDiscount.ClientID %>').attr('readonly', 'readonly');
        }
    });

    $('.imgServiceSwitch.imgLink').die('click');
    $('.imgServiceSwitch.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        var obj = rowToObject($row);
        cbpViewServiceCtl.PerformCallback('switch|' + obj.ID);
    });

    function calculateServiceDiscountTotal() {
        var discountAmount = parseInt($('#<%=hdnServiceDiscountAmount.ClientID %>').val());
        var isDicountInPercentage = ($('#<%=hdnServiceIsDicountInPercentage.ClientID %>').val() == '1');

        var discountTotal = 0;
        var discountComp1 = 0;
        var discountComp2 = 0;
        var discountComp3 = 0;
        var discountCITO = 0;
        if (discountAmount > 0) {
            var tariff = parseFloat($('#<%=txtServiceTariff.ClientID %>').attr('hiddenVal'));
            var tariffComp1 = parseFloat($('#<%=txtServiceTariffComp1.ClientID %>').attr('hiddenVal'));
            var tariffComp2 = parseFloat($('#<%=txtServiceTariffComp2.ClientID %>').attr('hiddenVal'));
            var tariffComp3 = parseFloat($('#<%=txtServiceTariffComp3.ClientID %>').attr('hiddenVal'));
            var tariffCITO = parseFloat($('#<%=txtServiceCITO.ClientID %>').attr('hiddenVal'));
            if (isDicountInPercentage) {
                if (discountAmount > 100)
                    discountAmount = 100;
                discountTotal = (tariff * discountAmount) / 100;
                discountComp1 = (tariffComp1 * discountAmount) / 100;
                discountComp2 = (tariffComp2 * discountAmount) / 100;
                discountComp3 = (tariffComp3 * discountAmount) / 100;
                discountCITO = (tariffCITO * discountAmount) / 100;
            }
            else {
                var qty = parseFloat($('#<%=txtServiceQty.ClientID %>').val());
                discountTotal = discountAmount * qty;
                if (discountTotal > tariff)
                    discountTotal = tariff;

                var tempDiscountTotal = discountTotal;
                discountComp1 = tempDiscountTotal;
                if (discountComp1 > tariffComp1) {
                    discountComp1 = tariffComp1;
                    tempDiscountTotal -= tariffComp1;
                    discountComp2 = tempDiscountTotal;
                    if (discountComp2 > tariffComp2) {
                        discountComp2 = tariffComp2;
                        tempDiscountTotal -= tariffComp2;
                        discountComp3 = tempDiscountTotal;
                        if (discountComp3 > tariffComp3) {
                            discountComp3 = tariffComp3;
                            tempDiscountTotal -= tariffComp3;
                            discountCITO = tempDiscountTotal;
                        }
                    }
                }
            }
        }
        $('#<%=txtServiceDiscount.ClientID %>').val(discountTotal).trigger('changeValue');
        $('#<%=txtServiceDiscComp1.ClientID %>').val(discountComp1).trigger('changeValue');
        $('#<%=txtServiceDiscComp2.ClientID %>').val(discountComp2).trigger('changeValue');
        $('#<%=txtServiceDiscComp3.ClientID %>').val(discountComp3).trigger('changeValue');
        $('#<%=txtServiceCITODisc.ClientID %>').val(discountCITO).trigger('changeValue');
        //$('#<%=txtServiceDiscTotal.ClientID %>').change();
    }

    function calculateServiceTotal() {
        var tariff = parseInt($('#<%=txtServiceTariff.ClientID %>').attr('hiddenVal'));
        var cito = parseInt($('#<%=txtServiceCITO.ClientID %>').attr('hiddenVal'));
        var complication = parseInt($('#<%=txtServiceComplication.ClientID %>').attr('hiddenVal'));
        var discount = parseInt($('#<%=txtServiceDiscount.ClientID %>').attr('hiddenVal'));
        var qty = parseFloat($('#<%=txtServiceQty.ClientID %>').val());
        var total = tariff + cito + complication - discount;

        var coverageAmount = parseInt($('#<%=hdnServiceCoverageAmount.ClientID %>').val());
        var isCoverageInPercentage = ($('#<%=hdnServiceIsCoverageInPercentage.ClientID %>').val() == '1');
        var totalPayer = 0;
        if (isCoverageInPercentage)
            totalPayer = (total * coverageAmount) / 100;
        else {
            var qty = parseFloat($('#<%=txtServiceQty.ClientID %>').val());
            totalPayer = coverageAmount * qty;
        }
        if (total > 0 && totalPayer > total)
            totalPayer = total;
        var totalPatient = total - totalPayer;

        var totalAllPayer = parseFloat($('#<%=hdnServiceAllTotalPayer.ClientID %>').val());
        totalAllPayer = (totalAllPayer - parseFloat($('#<%=txtServicePayer.ClientID %>').attr('hiddenVal')));
        totalAllPayer += totalPayer;
        $('#<%=hdnServiceAllTotalPayer.ClientID %>').val(totalAllPayer);
        $('.tdServiceTotalPayer').html(totalAllPayer.formatMoney(2, '.', ','));

        var totalAllPatient = parseFloat($('#<%=hdnServiceAllTotalPatient.ClientID %>').val());
        totalAllPatient = (totalAllPatient - parseInt($('#<%=txtServicePatient.ClientID %>').attr('hiddenVal')));
        totalAllPatient += totalPatient;
        $('#<%=hdnServiceAllTotalPatient.ClientID %>').val(totalAllPatient);
        $('.tdServiceTotalPatient').html(totalAllPatient.formatMoney(2, '.', ','));

        var totalLineAmount = totalAllPatient + totalAllPayer;
        $('.tdServiceTotal').html(totalLineAmount.formatMoney(2, '.', ','));

        $('#<%=txtServicePatient.ClientID %>').val(totalPatient).trigger('changeValue');
        $('#<%=txtServicePayer.ClientID %>').val(totalPayer).trigger('changeValue');
        $('#<%=txtServiceTotal.ClientID %>').val(total).trigger('changeValue');
        calculateAllTotal();
    }

    function calculateAllTotal() {
        var serviceTotalPatient = getServiceTotalPatient();
        var serviceTotalPayer = getServiceTotalPayer();

        var drugMSTotalPatient = getDrugMSTotalPatient();
        var drugMSTotalPayer = getDrugMSTotalPayer();

        var logisticTotalPatient = getLogisticTotalPatient();
        var logisticTotalPayer = getLogisticTotalPayer();

        var totalPatient = (serviceTotalPatient + drugMSTotalPatient + logisticTotalPatient);
        var totalPayer = (serviceTotalPayer + drugMSTotalPayer + logisticTotalPayer);

        $('#<%=txtTotalPatientAll.ClientID %>').val(totalPatient).trigger('changeValue');
        $('#<%=txtTotalPayerAll.ClientID %>').val(totalPayer).trigger('changeValue');
    }

    $('#btnServiceCancel').live('click', function () {
        $('#containerEntryService').hide();

        var totalAllPayer = parseFloat($('#<%=hdnTempTotalPayer.ClientID %>').val());
        $('#<%=hdnServiceAllTotalPayer.ClientID %>').val(totalAllPayer);
        $('.tdServiceTotalPayer').html(totalAllPayer.formatMoney(2, '.', ','));

        var totalAllPatient = parseFloat($('#<%=hdnTempTotalPatient.ClientID %>').val());
        $('#<%=hdnServiceAllTotalPatient.ClientID %>').val(totalAllPatient);
        $('.tdServiceTotalPatient').html(totalAllPatient.formatMoney(2, '.', ','));

        var totalLineAmount = totalAllPatient + totalAllPayer;
        $('.tdServiceTotal').html(totalLineAmount.formatMoney(2, '.', ','));

        calculateAllTotal();
    });

    //#region Entry Service
    $('.imgServiceDelete.imgLink').die('click');
    $('.imgServiceDelete.imgLink').live('click', function () {
        $row = $(this).closest('tr').parent().closest('tr');
        var message = 'Are You Sure ?';
        showToastConfirmation(message, function (result) {
            if (result) {
                var obj = rowToObject($row);
                var param = 'delete|' + obj.ID;
                cbpViewServiceCtl.PerformCallback(param);
            }
        });
    });

    //#region Edit
    $('.imgServiceEdit.imgLink').die('click');
    $('.imgServiceEdit.imgLink').live('click', function () {
        $('#containerEntryService').show();
        showLoadingPanel();
        $('#<%=hdnTempTotalPatient.ClientID %>').val(getServiceTotalPatient());
        $('#<%=hdnTempTotalPayer.ClientID %>').val(getServiceTotalPayer());
        $row = $(this).closest('tr').parent().closest('tr');
        var obj = rowToObject($row);
        $('#<%=hdnEntryID.ClientID %>').val(obj.ID);

        cboServiceChargeClassID.SetValue(obj.ChargeClassID);
        if (obj.IsSubContractItem == 'True') {
            $('#lblTestPartner').attr('class', 'lblLink');
            $('#<%=txtTestPartnerCode.ClientID %>').removeAttr('readonly');
        }
        else {
            $('#lblTestPartner').attr('class', 'lblDisabled');
            $('#<%=txtTestPartnerCode.ClientID %>').attr('readonly', 'readonly');
        }
        $('#<%=hdnServicePhysicianID.ClientID %>').val(obj.ParamedicID);
        $('#<%=txtServicePhysicianCode.ClientID %>').val(obj.ParamedicCode);
        $('#<%=txtServicePhysicianName.ClientID %>').val(obj.ParamedicName);
        $('#<%=hdnTestPartnerID.ClientID %>').val(obj.TestPartnerID);
        $('#<%=txtTestPartnerCode.ClientID %>').val(obj.TestPartnerCode);
        $('#<%=txtTestPartnerName.ClientID %>').val(obj.TestPartnerName);
        $('#lblServiceItem').attr('class', 'lblDisabled');
        $('#<%=txtServiceItemCode.ClientID %>').attr('readonly', 'readonly');
        $('#<%=hdnServiceItemID.ClientID %>').val(obj.ItemID);
        $('#<%=hdnServiceRevenueSharingID.ClientID %>').val(obj.RevenueSharingID);
        $('#<%=txtServiceItemCode.ClientID %>').val(obj.ItemCode);
        $('#<%=txtServiceItemName.ClientID %>').val(obj.ItemName1);
        $('#<%=hdnServiceTransactionDtID.ClientID %>').val(obj.ID);
        $('#<%=hdnServiceBaseTariff.ClientID %>').val(obj.BaseTariff);
        $('#<%=txtServiceQty.ClientID %>').val(obj.ChargedQuantity);
        setCheckBoxEnabled($('#<%=chkServiceIsCITO.ClientID %>'), obj.IsAllowCITO == 'True');
        setCheckBoxEnabled($('#<%=chkServiceIsVariable.ClientID %>'), obj.IsAllowVariable == 'True');
        setCheckBoxEnabled($('#<%=chkServiceIsUnbilledItem.ClientID %>'), obj.IsAllowUnbilledItem == 'True');
        setCheckBoxEnabled($('#<%=chkServiceIsDiscount.ClientID %>'), obj.IsAllowDiscount == 'True');
        setCheckBoxEnabled($('#<%=chkServiceIsComplication.ClientID %>'), obj.IsAllowComplication == 'True');
        $('#<%=hdnDefaultTariffComp.ClientID %>').val(obj.DefaultTariffComp);
        $('#<%=chkServiceIsCITO.ClientID %>').prop('checked', obj.IsCITO == 'True');
        $('#<%=chkServiceIsVariable.ClientID %>').prop('checked', obj.IsVariable == 'True');
        $('#<%=chkServiceIsUnbilledItem.ClientID %>').prop('checked', obj.IsUnbilledItem == 'True');
        $('#<%=chkServiceIsDiscount.ClientID %>').prop('checked', obj.IsDiscount == 'True');
        $('#<%=chkServiceIsComplication.ClientID %>').prop('checked', obj.IsComplication == 'True');
        $('#<%=hdnServiceIsCITOInPercentage.ClientID %>').val(obj.IsCITOInPercentage == 'True' ? '1' : '0');
        $('#<%=hdnServiceBaseCITOAmount.ClientID %>').val(obj.BaseCITOAmount);
        $('#<%=hdnServiceIsComplicationInPercentage.ClientID %>').val(obj.IsComplicationInPercentage == 'True' ? '1' : '0');
        $('#<%=hdnServiceBaseComplicationAmount.ClientID %>').val(obj.BaseComplicationAmount);
        //$('#<%=chkServiceIsCITO.ClientID %>').change();
        //$('#<%=chkServiceIsVariable.ClientID %>').change();
        //$('#<%=chkServiceIsUnbilledItem.ClientID %>').change();
        $('#<%=txtServiceTariff.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtServiceUnitTariff.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtServiceCITO.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtServiceComplication.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtServiceDiscount.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtServicePatient.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtServicePayer.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtServiceTotal.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtServiceCostAmount.ClientID %>').attr('readonly', 'readonly');

        if ($('#<%=chkServiceIsVariable.ClientID %>').is(':checked')) {
            $('#<%=txtServiceUnitTariff.ClientID %>').removeAttr('readonly');
            $('#<%=txtServicePatient.ClientID %>').removeAttr('readonly');
            $('#<%=txtServicePayer.ClientID %>').removeAttr('readonly');
            $('#<%=txtServiceTariffComp1.ClientID %>').removeAttr('readonly');
            $('#<%=txtServiceTariffComp2.ClientID %>').removeAttr('readonly');
            $('#<%=txtServiceTariffComp3.ClientID %>').removeAttr('readonly');
            $('#<%=txtServiceTotalTariff.ClientID %>').removeAttr('readonly');
        }
        else {
            $('#<%=txtServiceUnitTariff.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtServicePatient.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtServicePayer.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtServiceTariffComp1.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtServiceTariffComp2.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtServiceTariffComp3.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtServiceTotalTariff.ClientID %>').attr('readonly', 'readonly');
        }

        if ($('#<%=chkServiceIsDiscount.ClientID %>').is(':checked')) {
            $('#<%=txtServiceDiscount.ClientID %>').removeAttr('readonly');
        }
        else {
            $('#<%=txtServiceDiscount.ClientID %>').attr('readonly', 'readonly');
        }

        $('#<%=txtServiceUnitTariff.ClientID %>').val(obj.Tariff).trigger('changeValue');
        $('#<%=txtServiceTariffComp1.ClientID %>').val(obj.TariffComp1).trigger('changeValue');
        $('#<%=txtServiceTariffComp2.ClientID %>').val(obj.TariffComp2).trigger('changeValue');
        $('#<%=txtServiceTariffComp3.ClientID %>').val(obj.TariffComp3).trigger('changeValue');
        $('#<%=txtServiceCITO.ClientID %>').val(obj.CITOAmount).trigger('changeValue');
        $('#<%=txtServiceDiscount.ClientID %>').val(obj.DiscountAmount).trigger('changeValue');
        $('#<%=txtServiceDiscComp1.ClientID %>').val(obj.DiscountComp1).trigger('changeValue');
        $('#<%=txtServiceDiscComp2.ClientID %>').val(obj.DiscountComp2).trigger('changeValue');
        $('#<%=txtServiceDiscComp3.ClientID %>').val(obj.DiscountComp3).trigger('changeValue');
        $('#<%=txtServiceCostAmount.ClientID %>').val(obj.CostAmount).trigger('changeValue');
        $('#<%=txtServiceComplication.ClientID %>').val(obj.ComplicationAmount).trigger('changeValue');
        $('#<%=txtServiceTariff.ClientID %>').val(parseFloat(obj.GrossLineAmount)).trigger('changeValue');
        $('#<%=txtServicePatient.ClientID %>').val(obj.PatientAmount).trigger('changeValue');
        $('#<%=txtServicePayer.ClientID %>').val(obj.PayerAmount).trigger('changeValue');
        $('#<%=txtServiceTotal.ClientID %>').val(obj.LineAmount).trigger('changeValue');
        $('#<%=txtServicePhysicianCode.ClientID %>').focus();

        var appointmentID = $('#<%=hdnAppointmentID.ClientID %>').val();
        var classID = obj.ChargeClassID;
        var trxDate = getTrxDate();
        Methods.getDraftItemTariff(appointmentID, classID, obj.ItemID, trxDate, function (result) {
            $('#<%=hdnServiceDiscountAmount.ClientID %>').val(result.DiscountAmount);
            $('#<%=hdnServiceCoverageAmount.ClientID %>').val(result.CoverageAmount);
            $('#<%=hdnServicePrice.ClientID %>').val(result.Price);
            $('#<%=hdnServicePriceComp1.ClientID %>').val(result.PriceComp1);
            $('#<%=hdnServicePriceComp2.ClientID %>').val(result.PriceComp2);
            $('#<%=hdnServicePriceComp3.ClientID %>').val(result.PriceComp3);
            $('#<%=hdnServiceIsDicountInPercentage.ClientID %>').val(result.IsDiscountInPercentage ? '1' : '0');
            $('#<%=hdnServiceIsCoverageInPercentage.ClientID %>').val(result.IsCoverageInPercentage ? '1' : '0');
            hideLoadingPanel();
            $('#<%=chkServiceIsDiscount.ClientID %>').change();
            $('#<%=chkServiceIsComplication.ClientID %>').change();
        });
    });
    //#endregion

    //#region Add
    $('#lblServiceAddData').die('click');
    $('#lblServiceAddData').live('click', function (evt) {
        if (IsValid(null, 'fsMPEntryPopup', 'mpTrxService')) {
            $('#<%=hdnTempTotalPatient.ClientID %>').val(getServiceTotalPatient());
            $('#<%=hdnTempTotalPayer.ClientID %>').val(getServiceTotalPayer());

            if (typeof onAddRecordSetControlDisabled == 'function')
                onAddRecordSetControlDisabled();

            $('#containerEntryService').show();
            showLoadingPanel();
            $('#<%=hdnServiceTransactionDtID.ClientID %>').val('');

            $('#lblServiceItem').attr('class', 'lblLink');
            $('#<%=txtServiceItemCode.ClientID %>').removeAttr('readonly');

            var physicianID = getPhysicianID();
            if (physicianID != '') {
                $('#<%=hdnServicePhysicianID.ClientID %>').val(getPhysicianID());
                $('#<%=txtServicePhysicianCode.ClientID %>').val(getPhysicianCode());
                $('#<%=txtServicePhysicianName.ClientID %>').val(getPhysicianName());
            }

            $('#<%=hdnServiceItemID.ClientID %>').val('');
            $('#<%=hdnServiceRevenueSharingID.ClientID %>').val('');
            $('#<%=txtServiceItemCode.ClientID %>').val('');
            $('#<%=txtServiceItemName.ClientID %>').val('');

            $('#<%=hdnServiceBaseTariff.ClientID %>').val('0');
            $('#<%=txtServiceQty.ClientID %>').val('1');
            $('#<%=hdnServiceItemUnit.ClientID %>').val('');
            $('#<%=hdnServiceItemID.ClientID %>').val('');

            $('#<%=chkServiceIsCITO.ClientID %>').attr('disabled', 'disabled');
            $('#<%=chkServiceIsVariable.ClientID %>').attr('disabled', 'disabled');
            $('#<%=chkServiceIsUnbilledItem.ClientID %>').attr('disabled', 'disabled');
            $('#<%=chkServiceIsDiscount.ClientID %>').attr('disabled', 'disabled');
            $('#<%=chkServiceIsComplication.ClientID %>').attr('disabled', 'disabled');

            $('#<%=chkServiceIsCITO.ClientID %>').prop('checked', false);
            $('#<%=chkServiceIsVariable.ClientID %>').prop('checked', false);
            $('#<%=chkServiceIsUnbilledItem.ClientID %>').prop('checked', false);
            $('#<%=chkServiceIsDiscount.ClientID %>').prop('checked', false);
            $('#<%=chkServiceIsComplication.ClientID %>').prop('checked', false);

            $('#<%=txtServiceUnitTariff.ClientID %>').val('0').trigger('changeValue');
            $('#<%=txtServiceCITO.ClientID %>').val('0').trigger('changeValue');
            $('#<%=txtServiceDiscount.ClientID %>').val('0').trigger('changeValue');
            $('#<%=txtServiceComplication.ClientID %>').val('0').trigger('changeValue');

            $('#<%=txtServicePatient.ClientID %>').val('0').trigger('changeValue');
            $('#<%=txtServicePayer.ClientID %>').val('0').trigger('changeValue');
            $('#<%=txtServiceTotal.ClientID %>').val('0').trigger('changeValue');

            $('#<%=txtServiceUnitTariff.ClientID %>').val('0').trigger('changeValue');
            $('#<%=txtServiceTariffComp1.ClientID %>').val('0').trigger('changeValue');
            $('#<%=txtServiceTariffComp2.ClientID %>').val('0').trigger('changeValue');
            $('#<%=txtServiceTariffComp3.ClientID %>').val('0').trigger('changeValue');
            $('#<%=hdnServicePrice.ClientID %>').val('0');
            $('#<%=hdnServicePriceComp1.ClientID %>').val('0');
            $('#<%=hdnServicePriceComp2.ClientID %>').val('0');
            $('#<%=hdnServicePriceComp3.ClientID %>').val('0');
            $('#txtServiceDiscTariffComp1').val('0').trigger('changeValue');
            $('#txtServiceDiscTariffComp2').val('0').trigger('changeValue');
            $('#txtServiceDiscTariffComp3').val('0').trigger('changeValue');
            $('#<%=hdnServiceBaseTariff.ClientID %>').val('');
            $('#<%=hdnServiceItemUnit.ClientID %>').val('');

            $('#<%=hdnServiceDiscountAmount.ClientID %>').val('0');
            $('#<%=hdnServiceCoverageAmount.ClientID %>').val('0');
            $('#<%=hdnServiceIsDicountInPercentage.ClientID %>').val('0');
            $('#<%=hdnServiceIsCoverageInPercentage.ClientID %>').val('0');

            $('#<%=hdnServiceIsCITOInPercentage.ClientID %>').val('0');
            $('#<%=hdnServiceBaseCITOAmount.ClientID %>').val('0');
            $('#<%=hdnServiceIsComplicationInPercentage.ClientID %>').val('0');
            $('#<%=hdnServiceBaseComplicationAmount.ClientID %>').val('0');

            $('#<%=txtServiceCITO.ClientID %>').val('0').trigger('changeValue');
            $('#<%=txtServiceComplication.ClientID %>').val('0').trigger('changeValue');

            $('#<%=txtServiceTariff.ClientID %>').val('0').trigger('changeValue');

            $('#<%=txtServiceItemCode.ClientID %>').focus();
            $('#<%=txtServiceUnitTariff.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtServiceTariff.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtServiceCITO.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtServiceComplication.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtServiceDiscount.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtServicePatient.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtServicePayer.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtServiceTotal.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtServiceUnitTariff.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtServicePatient.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtServicePayer.ClientID %>').attr('readonly', 'readonly');

            $('#<%=txtServiceTariffComp1.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtServiceTariffComp2.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtServiceTariffComp3.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtServiceTotalTariff.ClientID %>').attr('readonly', 'readonly');

            hideLoadingPanel();
        }
    });
    //#endregion
    //#endregion

    //#region Item Service
    //#region Item
    function onGetServiceItemFilterExpression() {
        return $('#<%=hdnServiceItemFilterExpression.ClientID %>').val();
    }

    $('#lblServiceItem.lblLink').live('click', function () {
        var filterExpression = onGetServiceItemFilterExpression() + " AND GCItemStatus != 'X181^999'";
        openSearchDialog('serviceunititem', filterExpression, function (value) {
            $('#<%=txtServiceItemCode.ClientID %>').val(value);
            onTxtServiceItemCodeChanged(value);
        });
    });

    $('#<%=txtServiceItemCode.ClientID %>').live('change', function () {
        onTxtServiceItemCodeChanged($(this).val());
    });

    function setServiceItemFilterExpression(healthcareServiceUnitID, transactionID) {
        $('#<%=hdnServiceItemFilterExpression.ClientID %>').val("HealthcareServiceUnitID = " + healthcareServiceUnitID + " AND IsDeleted = 0");
    }

    function onTxtServiceItemCodeChanged(value) {
        var paramedicID = $('#<%=hdnServicePhysicianID.ClientID %>').val();
        if (paramedicID == '')
            paramedicID = '0';

        var today = new Date();
        var date = today.getDate() + '-' + (today.getMonth() + 1) + '-' + today.getFullYear();
        var time = today.getHours() + ":" + today.getMinutes();

        var txtDraftChargesDate = $('#<%=txtDraftChargesDate.ClientID %>').val() == "" ? date : $('#<%=txtDraftChargesDate.ClientID %>').val();
        var txtDraftChargesTime = $('#<%=txtDraftChargesTime.ClientID %>').val() == "" ? time : $('#<%=txtDraftChargesTime.ClientID %>').val();
        var hdnChargesHealthcareServiceUnitID = $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val();

        Methods.getItemRevenueSharing(value, paramedicID, cboServiceChargeClassID.GetValue(), '<%=GetMainParamedicRole() %>', 0, hdnChargesHealthcareServiceUnitID, txtDraftChargesDate, txtDraftChargesTime, function (result) {
            if (result != null) {
                $('#<%=hdnServiceItemID.ClientID %>').val(result.ItemID);
                $('#<%=hdnServiceRevenueSharingID.ClientID %>').val(result.RevenueSharingID);
                $('#<%=txtServiceItemName.ClientID %>').val(result.ItemName1);
                if (result.IsSubContractItem) {
                    $('#lblTestPartner').attr('class', 'lblLink');
                    $('#<%=txtTestPartnerCode.ClientID %>').removeAttr('readonly');
                }
                else {
                    $('#lblTestPartner').attr('class', 'lblDisabled');
                    $('#<%=txtTestPartnerCode.ClientID %>').val('');
                    $('#<%=txtTestPartnerName.ClientID %>').val('');
                    $('#<%=hdnTestPartnerID.ClientID %>').val('');
                    $('#<%=txtTestPartnerCode.ClientID %>').attr('readonly', 'readonly');
                }
            }
            else {
                $('#<%=hdnServiceItemID.ClientID %>').val('');
                $('#<%=hdnServiceRevenueSharingID.ClientID %>').val('');
                $('#<%=txtServiceItemCode.ClientID %>').val('');
                $('#<%=txtServiceItemName.ClientID %>').val('');
            }
            getServiceTariff();
        });
    }
    //#endregion

    $('#btnServiceSave').live('click', function (evt) {
        if (IsValid(evt, 'fsMPEntryPopup', 'mpTrxService')) {
            cbpViewServiceCtl.PerformCallback('save');
        }
        return false;
    });

    var isChangeFromServiceUnitTariff = false;
    var isChangeFromTariffCompTotal = false;
    var isChangeFromServiceDiscount = false;
    var isChangeFromDiscCompTotal = false;

    $('#<%=txtServiceDiscount.ClientID %>').live('change', function () {
        $(this).blur();

        $totalTariffCITO = parseFloat($('#<%=txtServiceTariff.ClientID %>').attr('hiddenVal')) + parseFloat($('#<%=txtServiceCITO.ClientID %>').attr('hiddenVal'));
        if ($totalTariffCITO < parseInt($('#<%=txtServiceDiscount.ClientID %>').attr('hiddenVal')))
            $('#<%=txtServiceDiscount.ClientID %>').val($totalTariffCITO).trigger('changeValue');

        calculateServiceTariffTotal();
        calculateServiceTotal();

        if (!isChangeFromDiscCompTotal) {
            $('.txtServiceDiscTotal').val($('#<%=txtServiceDiscount.ClientID %>').attr('hiddenVal')).trigger('changeValue');
            isChangeFromServiceDiscount = true;
            $('.txtServiceDiscTotal').change();
        }
        isChangeFromDiscCompTotal = false;
    });

    $('#<%=txtServiceUnitTariff.ClientID %>').live('change', function () {
        $(this).blur();

        $('#<%=chkServiceIsDiscount.ClientID %>').prop('checked', false);
        $('#<%=txtServiceDiscount.ClientID %>').attr('readonly', 'readonly');

        calculateServiceTariffTotal();
        $('#<%=chkServiceIsCITO.ClientID %>').change();
        $('#<%=chkServiceIsComplication.ClientID %>').change();
        calculateServiceDiscountTotal();
        calculateServiceTotal();

        if (!isChangeFromTariffCompTotal) {
            $('#<%=txtServiceTotalTariff.ClientID %>').val($('#<%=txtServiceUnitTariff.ClientID %>').attr('hiddenVal')).trigger('changeValue');
            isChangeFromServiceUnitTariff = true;
            $('#<%=txtServiceTotalTariff.ClientID %>').change();
        }
        isChangeFromTariffCompTotal = false;
    });

    $('#<%=txtServicePatient.ClientID %>').live('change', function () {
        var patientTotal = parseInt($(this).val());
        var total = parseInt($('#<%=txtServiceTotal.ClientID %>').attr('hiddenVal'));
        var payerTotal = total - patientTotal;
        $('#<%=txtServicePayer.ClientID %>').val(payerTotal).trigger('changeValue');
    });

    $('#<%=txtServicePayer.ClientID %>').live('change', function () {
        var payerTotal = parseInt($(this).val());
        var total = parseInt($('#<%=txtServiceTotal.ClientID %>').attr('hiddenVal'));
        var patientTotal = total - payerTotal;
        $('#<%=txtServicePatient.ClientID %>').val(patientTotal).trigger('changeValue');
    });

    $('#<%=txtServiceQty.ClientID %>').live('change', function () {
        $('#<%=chkServiceIsDiscount.ClientID %>').prop('checked', false);
        $('#<%=txtServiceDiscount.ClientID %>').attr('readonly', 'readonly');

        var qty = parseFloat($('#<%=txtServiceQty.ClientID %>').val());
        //sini
        var priceComp1 = parseInt($('#<%=txtServiceTariffComp1.ClientID %>').attr('hiddenVal'));
        var priceComp2 = parseInt($('#<%=txtServiceTariffComp2.ClientID %>').attr('hiddenVal'));
        var priceComp3 = parseInt($('#<%=txtServiceTariffComp3.ClientID %>').attr('hiddenVal'));
        $('#txtServiceDiscTariffComp1').val(priceComp1).trigger('changeValue');
        $('#txtServiceDiscTariffComp2').val(priceComp2).trigger('changeValue');
        $('#txtServiceDiscTariffComp3').val(priceComp3).trigger('changeValue');
        //sini2
        calculateServiceTariffTotal();
        $('#<%=chkServiceIsCITO.ClientID %>').change();
        $('#<%=chkServiceIsComplication.ClientID %>').change();
        calculateServiceDiscountTotal();
        calculateServiceTotal();
    });

    function getServiceTariff() {
        var itemID = $('#<%=hdnServiceItemID.ClientID %>').val();
        if (itemID != '') {
            showLoadingPanel();
            var appointmentID = $('#<%=hdnAppointmentID.ClientID %>').val();
            var classID = cboServiceChargeClassID.GetValue();
            var trxDate = getTrxDate();
            Methods.getDraftItemTariff(appointmentID, classID, itemID, trxDate, function (result) {
                $('#<%=chkServiceIsCITO.ClientID %>').prop('checked', false);
                $('#<%=chkServiceIsVariable.ClientID %>').prop('checked', false);
                $('#<%=chkServiceIsUnbilledItem.ClientID %>').prop('checked', false);
                $('#<%=chkServiceIsDiscount.ClientID %>').prop('checked', false);
                $('#<%=chkServiceIsComplication.ClientID %>').prop('checked', false);
                if (result != null) {
                    $('#<%=hdnServiceItemUnit.ClientID %>').val(result.GCItemUnit);
                    $('#<%=hdnDefaultTariffComp.ClientID %>').val(result.DefaultTariffComp);

                    setCheckBoxEnabled($('#<%=chkServiceIsCITO.ClientID %>'), result.IsAllowCito);
                    setCheckBoxEnabled($('#<%=chkServiceIsVariable.ClientID %>'), result.IsAllowVariable);
                    setCheckBoxEnabled($('#<%=chkServiceIsUnbilledItem.ClientID %>'), result.IsUnbilledItem);
                    setCheckBoxEnabled($('#<%=chkServiceIsDiscount.ClientID %>'), result.IsAllowDiscount);
                    setCheckBoxEnabled($('#<%=chkServiceIsComplication.ClientID %>'), result.IsAllowComplication);
                    $('#<%=txtServiceUnitTariff.ClientID %>').val(result.Price).trigger('changeValue');
                    $('#<%=hdnServicePrice.ClientID %>').val(result.Price);
                    $('#<%=hdnServiceBaseTariff.ClientID %>').val(result.BasePrice);
                    $('#<%=hdnServicePriceComp1.ClientID %>').val(result.PriceComp1);
                    $('#<%=hdnServicePriceComp2.ClientID %>').val(result.PriceComp2);
                    $('#<%=hdnServicePriceComp3.ClientID %>').val(result.PriceComp3);
                    $('#<%=hdnServiceBasePriceComp1.ClientID %>').val(result.BasePriceComp1);
                    $('#<%=hdnServiceBasePriceComp2.ClientID %>').val(result.BasePriceComp2);
                    $('#<%=hdnServiceBasePriceComp3.ClientID %>').val(result.BasePriceComp3);
                    $('#<%=txtServiceUnitTariff.ClientID %>').change();
                    $('#<%=txtServiceCostAmount.ClientID %>').val(result.CostAmount).trigger('changeValue');

                    $('#<%=hdnServiceDiscountAmount.ClientID %>').val(result.DiscountAmount);
                    $('#<%=hdnServiceCoverageAmount.ClientID %>').val(result.CoverageAmount);
                    $('#<%=hdnServiceIsDicountInPercentage.ClientID %>').val(result.IsDiscountInPercentage ? '1' : '0');
                    $('#<%=hdnServiceIsCoverageInPercentage.ClientID %>').val(result.IsCoverageInPercentage ? '1' : '0');

                    $('#<%=hdnServiceIsCITOInPercentage.ClientID %>').val(result.IsCITOInPercentage ? '1' : '0');
                    $('#<%=hdnServiceBaseCITOAmount.ClientID %>').val(result.CITOAmount);
                    $('#<%=hdnServiceIsComplicationInPercentage.ClientID %>').val(result.IsComplicationInPercentage ? '1' : '0');
                    $('#<%=hdnServiceBaseComplicationAmount.ClientID %>').val(result.ComplicationAmount);
                }
                else {
                    $('#<%=hdnDefaultTariffComp.ClientID %>').val('1');
                    $('#<%=chkServiceIsCITO.ClientID %>').attr('disabled', 'disabled');
                    $('#<%=chkServiceIsVariable.ClientID %>').attr('disabled', 'disabled');
                    $('#<%=chkServiceIsUnbilledItem.ClientID %>').attr('disabled', 'disabled');
                    $('#<%=chkServiceIsDiscount.ClientID %>').attr('disabled', 'disabled');
                    $('#<%=chkServiceIsComplication.ClientID %>').attr('disabled', 'disabled');

                    $('#<%=txtServiceUnitTariff.ClientID %>').val('0').trigger('changeValue');
                    $('#<%=hdnServicePrice.ClientID %>').val('0');
                    $('#<%=hdnServiceBaseTariff.ClientID %>').val('');
                    $('#<%=hdnServiceItemUnit.ClientID %>').val('');
                    $('#<%=txtServiceCostAmount.ClientID %>').val('0').trigger('changeValue'); ;

                    $('#<%=hdnServiceDiscountAmount.ClientID %>').val('0');
                    $('#<%=hdnServiceCoverageAmount.ClientID %>').val('0');
                    $('#<%=hdnServiceIsDicountInPercentage.ClientID %>').val('0');
                    $('#<%=hdnServiceIsCoverageInPercentage.ClientID %>').val('0');

                    $('#<%=hdnServiceIsCITOInPercentage.ClientID %>').val('0');
                    $('#<%=hdnServiceBaseCITOAmount.ClientID %>').val('0');
                    $('#<%=hdnServiceIsComplicationInPercentage.ClientID %>').val('0');
                    $('#<%=hdnServiceBaseComplicationAmount.ClientID %>').val('0');
                }

                calculateServiceTariffTotal();

                $('#<%=txtServiceCITO.ClientID %>').val('0').trigger('changeValue');
                $('#<%=txtServiceDiscount.ClientID %>').val('0').trigger('changeValue');
                $('#<%=txtServiceComplication.ClientID %>').val('0').trigger('changeValue');

                $('#<%=txtServiceUnitTariff.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtServiceDiscount.ClientID %>').attr('readonly', 'readonly');

                /*$('#<%=chkServiceIsCITO.ClientID %>').change();
                $('#<%=chkServiceIsVariable.ClientID %>').change();
                $('#<%=chkServiceIsDiscount.ClientID %>').change();
                $('#<%=chkServiceIsComplication.ClientID %>').change();*/

                calculateServiceDiscountTotal();
                calculateServiceTotal();
            });
            hideLoadingPanel();
        }
    }
    //#endregion

    //#region Physician
    $('#lblPhysician.lblLink').live('click', function () {
        openSearchDialog('paramedic', onGetPhysicianFilterExpression(), function (value) {
            $('#<%=txtServicePhysicianCode.ClientID %>').val(value);
            onTxtServicePhysicianCodeChanged(value);
        });
    });

    $('#<%=txtServicePhysicianCode.ClientID %>').live('change', function () {
        onTxtServicePhysicianCodeChanged($(this).val());
    });

    function onTxtServicePhysicianCodeChanged(value) {
        var filterExpression = onGetPhysicianFilterExpression() + " AND ParamedicCode = '" + value + "'";
        Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnServicePhysicianID.ClientID %>').val(result.ParamedicID);
                $('#<%=txtServicePhysicianName.ClientID %>').val(result.ParamedicName);
                var itemCode = $('#<%=txtServiceItemCode.ClientID %>').val();
                if (itemCode != '') {
                    var today = new Date();
                    var date = today.getDate() + '-' + (today.getMonth() + 1) + '-' + today.getFullYear();
                    var time = today.getHours() + ":" + today.getMinutes();

                    var txtDraftChargesDate = $('#<%=txtDraftChargesDate.ClientID %>').val() == "" ? date : $('#<%=txtDraftChargesDate.ClientID %>').val();
                    var txtDraftChargesTime = $('#<%=txtDraftChargesTime.ClientID %>').val() == "" ? time : $('#<%=txtDraftChargesTime.ClientID %>').val();
                    var hdnChargesHealthcareServiceUnitID = $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val();

                    Methods.getItemRevenueSharing(itemCode, $('#<%=hdnServicePhysicianID.ClientID %>').val(), cboServiceChargeClassID.GetValue(), '<%=GetMainParamedicRole() %>', 0, hdnChargesHealthcareServiceUnitID, txtDraftChargesDate, txtDraftChargesTime, function (result) {
                        if (result != null)
                            $('#<%=hdnServiceRevenueSharingID.ClientID %>').val(result.RevenueSharingID);
                        else
                            $('#<%=hdnServiceRevenueSharingID.ClientID %>').val('');
                    });
                }
            }
            else {
                $('#<%=hdnServicePhysicianID.ClientID %>').val('');
                $('#<%=txtServicePhysicianCode.ClientID %>').val('');
                $('#<%=txtServicePhysicianName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Test Partner
    $('#lblTestPartner.lblLink').live('click', function () {
        openSearchDialog('testpartner', '', function (value) {
            $('#<%=txtTestPartnerCode.ClientID %>').val(value);
            onTxtTestPartnerCodeChanged(value);
        });
    });

    $('#<%=txtTestPartnerCode.ClientID %>').live('change', function () {
        onTxtTestPartnerCodeChanged($(this).val());
    });

    function onTxtTestPartnerCodeChanged(value) {
        var filterExpression = "BusinessPartnerCode = '" + value + "'";
        Methods.getObject('GetvTestPartnerList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnTestPartnerID.ClientID %>').val(result.BusinessPartnerID);
                $('#<%=txtTestPartnerName.ClientID %>').val(result.BusinessPartnerName);
            }
            else {
                $('#<%=hdnTestPartnerID.ClientID %>').val('');
                $('#<%=txtTestPartnerName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Unit Tariff Component
    $('#<%=txtServiceTotalTariff.ClientID %>').change(function () {
        $(this).blur();
        var total = parseInt($(this).attr('hiddenVal'));
        var baseTariff = parseInt($('#<%=hdnServiceBaseTariff.ClientID %>').val());

        $txtComponent1 = null;
        $txtComponent2 = null;
        $txtComponent3 = null;

        var defaultComp = $('#<%=hdnDefaultTariffComp.ClientID %>').val();
        if (defaultComp == '1') {
            $txtComponent1 = $('#<%=txtServiceTariffComp1.ClientID %>');
            $txtComponent2 = $('#<%=txtServiceTariffComp2.ClientID %>');
            $txtComponent3 = $('#<%=txtServiceTariffComp3.ClientID %>');
        }
        else if (defaultComp == '2') {
            $txtComponent1 = $('#<%=txtServiceTariffComp2.ClientID %>');
            $txtComponent2 = $('#<%=txtServiceTariffComp1.ClientID %>');
            $txtComponent3 = $('#<%=txtServiceTariffComp3.ClientID %>');
        }
        else {
            $txtComponent1 = $('#<%=txtServiceTariffComp3.ClientID %>');
            $txtComponent2 = $('#<%=txtServiceTariffComp2.ClientID %>');
            $txtComponent3 = $('#<%=txtServiceTariffComp1.ClientID %>');
        }
        if (baseTariff != total) {
            var component2 = parseInt($txtComponent2.attr('hiddenVal'));
            var component3 = parseInt($txtComponent3.attr('hiddenVal'));
            var component1 = total - (component2 + component3);
            if (component1 < 0) {
                $txtComponent1.val(total).trigger('changeValue');
                $txtComponent2.val('0').trigger('changeValue');
                $txtComponent3.val('0').trigger('changeValue');
            }
            else {
                $txtComponent1.val(component1).trigger('changeValue');
            }
        }
        else {
            $txtComponent1.val($('#<%=hdnServicePriceComp1.ClientID %>').val()).trigger('changeValue');
            $txtComponent2.val($('#<%=hdnServicePriceComp2.ClientID %>').val()).trigger('changeValue');
            $txtComponent3.val($('#<%=hdnServicePriceComp3.ClientID %>').val()).trigger('changeValue');
        }

        if (!isChangeFromServiceUnitTariff) {
            $('#<%=txtServiceUnitTariff.ClientID %>').val($('#<%=txtServiceTotalTariff.ClientID %>').attr('hiddenVal')).trigger('changeValue');
            isChangeFromTariffCompTotal = true;
            $('#<%=txtServiceUnitTariff.ClientID %>').change();
        }
        isChangeFromServiceUnitTariff = false;
    });

    $('.txtTariffComp').live('change', function () {
        $(this).blur();
        calculateTariffTotal();
    });

    function calculateTariffTotal() {
        $total = 0;
        $('.txtTariffComp').each(function () {
            $total += parseInt($(this).attr('hiddenVal'));
        });
        $('#<%=txtServiceTotalTariff.ClientID %>').val($total).trigger('changeValue');
        $('#<%=txtServiceTotalTariff.ClientID %>').change();
    }

    $('#btnEditUnitTariff').live('click', function () {
        $('#<%=txtServiceTotalTariff.ClientID %>').val($('#<%=txtServiceUnitTariff.ClientID %>').attr('hiddenVal')).trigger('changeValue');
        pcUnitTariff.Show();
    });
    //#endregion

    //#region Discount Component
    $('#<%=txtServiceDiscTotal.ClientID %>').live('change', function () {
        $(this).blur();
        var total = parseInt($(this).attr('hiddenVal'));
        var discountComp1 = 0;
        var discountComp2 = 0;
        var discountComp3 = 0;
        var discountCITO = 0;

        var tariffComp1 = parseFloat($('#<%=txtServiceTariffComp1.ClientID %>').attr('hiddenVal') * $('#<%=txtServiceQty.ClientID %>').val());
        var tariffComp2 = parseFloat($('#<%=txtServiceTariffComp2.ClientID %>').attr('hiddenVal') * $('#<%=txtServiceQty.ClientID %>').val());
        var tariffComp3 = parseFloat($('#<%=txtServiceTariffComp3.ClientID %>').attr('hiddenVal') * $('#<%=txtServiceQty.ClientID %>').val());
        var tariffCITO = parseFloat($('#txtServiceCITOAmount').attr('hiddenVal'));

        $totalTariffCITO = parseFloat($('#<%=txtServiceTariff.ClientID %>').attr('hiddenVal')) + parseFloat($('#<%=txtServiceCITO.ClientID %>').attr('hiddenVal'));
        if (parseFloat($('#<%=txtServiceDiscTotal.ClientID %>').attr('hiddenVal')) > $totalTariffCITO) {
            total = $totalTariffCITO;
        }

        var tempDiscountTotal = total;
        discountComp1 = tempDiscountTotal;
        if (discountComp1 > tariffComp1) {
            discountComp1 = tariffComp1;
            tempDiscountTotal -= tariffComp1;
            discountComp2 = tempDiscountTotal;
            if (discountComp2 > tariffComp2) {
                discountComp2 = tariffComp2;
                tempDiscountTotal -= tariffComp2;
                discountComp3 = tempDiscountTotal;
                if (discountComp3 > tariffComp3) {
                    discountComp3 = tariffComp3;
                    tempDiscountTotal -= tariffComp3;
                    discountCITO = tempDiscountTotal;
                }
            }
        }

        $('#<%=txtServiceDiscTotal.ClientID %>').val(total).trigger('changeValue');
        $('#<%=txtServiceDiscComp1.ClientID %>').val(discountComp1).trigger('changeValue');
        $('#<%=txtServiceDiscComp2.ClientID %>').val(discountComp2).trigger('changeValue');
        $('#<%=txtServiceDiscComp3.ClientID %>').val(discountComp3).trigger('changeValue');
        $('#<%=txtServiceCITODisc.ClientID %>').val(discountCITO).trigger('changeValue');

        $('#txtServiceAfterTariffComp1').val($('#txtServiceDiscTariffComp1').attr('hiddenVal') - $('#<%=txtServiceDiscComp1.ClientID %>').attr('hiddenVal')).trigger('changeValue');
        $('#txtServiceAfterTariffComp2').val($('#txtServiceDiscTariffComp2').attr('hiddenVal') - $('#<%=txtServiceDiscComp2.ClientID %>').attr('hiddenVal')).trigger('changeValue');
        $('#txtServiceAfterTariffComp3').val($('#txtServiceDiscTariffComp3').attr('hiddenVal') - $('#<%=txtServiceDiscComp3.ClientID %>').attr('hiddenVal')).trigger('changeValue');
        $('#txtServiceAfterCITOAmount').val($('#txtServiceCITOAmount').attr('hiddenVal') - $('#<%=txtServiceCITODisc.ClientID %>').attr('hiddenVal')).trigger('changeValue');

        if (!isChangeFromServiceDiscount) {
            $('#<%=txtServiceDiscount.ClientID %>').val(total).trigger('changeValue');
            isChangeFromDiscCompTotal = true;
            $('#<%=txtServiceDiscount.ClientID %>').change();
        }
        isChangeFromServiceDiscount = false;
    });

    $('.txtDiscComp').live('change', function () {
        $(this).blur();
        $base = parseInt($(this).closest('tr').find('.txtUnitTariffPrev').attr('hiddenVal'));
        $disc = parseInt($(this).attr('hiddenVal'));
        if ($disc > $base)
            $disc = $base;
        $(this).val($disc).trigger('changeValue');
        calculateDiscountTotal();
    });

    function calculateDiscountTotal() {
        $total = 0;
        $('.txtDiscComp').each(function () {
            $base = parseInt($(this).closest('tr').find('.txtUnitTariffPrev').attr('hiddenVal'));
            $disc = parseInt($(this).attr('hiddenVal'));
            $after = $base - $disc;
            $total += parseInt($(this).attr('hiddenVal'));
            $(this).closest('tr').find('.txtUnitTariffAfter').val($after).trigger('changeValue');
        });
        $('#<%=txtServiceDiscTotal.ClientID %>').val($total).trigger('changeValue');
        if (!isChangeFromServiceDiscount) {
            $('#<%=txtServiceDiscount.ClientID %>').val($('#<%=txtServiceDiscTotal.ClientID %>').attr('hiddenVal')).trigger('changeValue');
            isChangeFromDiscCompTotal = true;
            $('#<%=txtServiceDiscount.ClientID %>').change();
        }
        isChangeFromServiceDiscount = false;
    }

    $('#btnEditDiscount').live('click', function () {
        if ($('#<%=chkServiceIsDiscount.ClientID %>').prop('checked')) {
            $('#<%=txtServiceDiscComp1.ClientID %>').removeAttr('readonly');
            $('#<%=txtServiceDiscComp2.ClientID %>').removeAttr('readonly');
            $('#<%=txtServiceDiscComp3.ClientID %>').removeAttr('readonly');
            $('#<%=txtServiceDiscTotal.ClientID %>').removeAttr('readonly');
            $('#<%=txtServiceCITODisc.ClientID %>').removeAttr('readonly');
        }
        else {
            $('#<%=txtServiceDiscComp1.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtServiceDiscComp2.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtServiceDiscComp3.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtServiceDiscTotal.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtServiceCITODisc.ClientID %>').attr('readonly', 'readonly');
        }

        var comp1 = parseFloat($('#<%=txtServiceTariffComp1.ClientID %>').attr('hiddenVal'));
        var comp2 = parseFloat($('#<%=txtServiceTariffComp2.ClientID %>').attr('hiddenVal'));
        var comp3 = parseFloat($('#<%=txtServiceTariffComp3.ClientID %>').attr('hiddenVal'));
        var qty = parseFloat($('#<%=txtServiceQty.ClientID %>').val());
        $('#txtServiceDiscTariffComp1').val(comp1 * qty).trigger('changeValue');
        $('#txtServiceDiscTariffComp2').val(comp2 * qty).trigger('changeValue');
        $('#txtServiceDiscTariffComp3').val(comp3 * qty).trigger('changeValue');

        $('#txtServiceAfterTariffComp1').val($('#txtServiceDiscTariffComp1').attr('hiddenVal') - $('#<%=txtServiceDiscComp1.ClientID %>').attr('hiddenVal')).trigger('changeValue');
        $('#txtServiceAfterTariffComp2').val($('#txtServiceDiscTariffComp2').attr('hiddenVal') - $('#<%=txtServiceDiscComp2.ClientID %>').attr('hiddenVal')).trigger('changeValue');
        $('#txtServiceAfterTariffComp3').val($('#txtServiceDiscTariffComp3').attr('hiddenVal') - $('#<%=txtServiceDiscComp3.ClientID %>').attr('hiddenVal')).trigger('changeValue');
        $('#txtServiceAfterCITOAmount').val($('#txtServiceCITOAmount').attr('hiddenVal') - $('#<%=txtServiceCITODisc.ClientID %>').attr('hiddenVal')).trigger('changeValue');

        $('#<%=txtServiceDiscTotal.ClientID %>').val($('#<%=txtServiceDiscount.ClientID %>').attr('hiddenVal')).trigger('changeValue');
        $('#<%=txtServiceDiscTotal.ClientID %>').change();

        pcDiscount.Show();
    });
    //#endregion

    var isChangeFromServiceUnitTariff = false;
    var isChangeFromTariffCompTotal = false;
    var isChangeFromServiceDiscount = false;
    var isChangeFromDiscCompTotal = false;

    window.onCbpViewServiceCtlEndCallback = function (s, e) {
        $('#<%=hdnEntryID.ClientID %>').val('');
        hideLoadingPanel();
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail') {
                showToast('Save Failed', 'Error Message : ' + param[2]);
            }
            else {
                $('#containerEntry').hide();
                $('#<%=hdnDraftChargesID.ClientID %>').val(s.cpTransactionID);
                cbpMainPopup.PerformCallback('loadaftersave');
            }
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail') {
                showToast('Delete Failed', 'Error Message : ' + param[2]);
                $('#<%=hdnDraftChargesID.ClientID %>').val(s.cpTransactionID);
                cbpMainPopup.PerformCallback('loadaftersave');
            }
            else {
                $('#containerEntry').hide();
                $('#<%=hdnDraftChargesID.ClientID %>').val(s.cpTransactionID);
                cbpMainPopup.PerformCallback('loadaftersave');
            }
        }
        else if (param[0] == 'switch') {
            if (param[1] == 'fail') {
                showToast('Switch Failed', 'Error Message : ' + param[2]);
                $('#<%=hdnDraftChargesID.ClientID %>').val(s.cpTransactionID);
                cbpMainPopup.PerformCallback('loadaftersave');
            }
            else {
                cbpMainPopup.PerformCallback('load');
            }
        }
        calculateAllTotal();
    }
    //#End Region Pelayanan

    //#Region Drug
    //#region Entry Drug MS
    function getDrugMSItemUnitName(itemUnitID) {
        var value = cboDrugMSUoM.GetValue();
        cboDrugMSUoM.SetValue(itemUnitID);
        var text = cboDrugMSUoM.GetText();
        cboDrugMSUoM.SetValue(value);
        return text;
    }

    $('#<%=txtDrugMSQtyUsed.ClientID %>').live('change', function () {
        $('#<%=txtDrugMSQtyCharged.ClientID %>').val($(this).val());
        $('#<%=txtDrugMSQtyCharged.ClientID %>').change();
    });

    $('#<%=txtDrugMSQtyCharged.ClientID %>').live('change', function () {
        var conversionValue = parseFloat($('#<%=hdnDrugMSConversionValue.ClientID %>').val());
        var qty = parseFloat($(this).val());
        $('#<%=txtDrugMSBaseQty.ClientID %>').val(conversionValue * qty);
        calculateDrugMSTariffTotal();
        calculateDrugMSDiscountTotal();
        calculateDrugMSTotal();
    });

    $('#<%=txtDrugMSPatient.ClientID %>').live('change', function () {
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

    $('#<%=txtDrugMSPayer.ClientID %>').live('change', function () {
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

    function onAfterSaveRecordDtSuccess(transactionID) {
        var hdnTransactionID = $('#<%=hdnDraftChargesID.ClientID %>').val();
        if (hdnTransactionID == '0' || hdnTransactionID == '') {
            $('#<%=hdnDraftChargesID.ClientID %>').val(transactionID);
            var filterExpression = 'TransactionID = ' + transactionID;
            Methods.getObject('GetDraftPatientChargesHdList', filterExpression, function (result) {
                $('#<%=txtDraftChargesNo.ClientID %>').val(result.TransactionNo);
                $('#<%=txtReferenceNo.ClientID %>').val(result.ReferenceNo);
            });

            setServiceItemFilterExpression($('#<%=hdnHealthcareServiceUnitID.ClientID %>').val(), transactionID);
            setDrugMSItemFilterExpression(transactionID);
            setLogisticItemFilterExpression(transactionID);
            onAfterCustomSaveSuccess();
        }
    }

    $('#<%=txtDrugMSQtyCharged.ClientID %>').live('change', function () {
        var conversionValue = parseFloat($('#<%=hdnDrugMSConversionValue.ClientID %>').val());
        var qty = parseFloat($(this).val());
        $('#<%=txtDrugMSBaseQty.ClientID %>').val(conversionValue * qty);
        calculateDrugMSTariffTotal();
        calculateDrugMSDiscountTotal();
        calculateDrugMSTotal();
    });

    function calculateDrugMSTariffTotal() {
        var tariff = parseFloat($('#<%=txtDrugMSUnitTariff.ClientID %>').attr('hiddenVal'));
        var qty = parseFloat($('#<%=txtDrugMSBaseQty.ClientID %>').val());
        $('#<%=txtDrugMSPriceTariff.ClientID %>').val(tariff * qty).trigger('changeValue');
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

    $('#btnDrugMSSave').live('click', function (evt) {
        if (IsValid(evt, 'fsTrxDrugMS', 'mpTrxDrugMS')) {
            cbpDrugMS.PerformCallback('save');
        }
        return false;
    });

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
        showToastConfirmation('Are You Sure Want To Delete?', function (result) {
            if (result) {
                var obj = rowToObject($row);
                var param = 'delete|' + obj.ID;
                cbpDrugMS.PerformCallback(param);
            }
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

        var appointmentID = $('#<%=hdnAppointmentID.ClientID %>').val();
        var classID = obj.ChargeClassID;
        var trxDate = getTrxDate();
        Methods.getDraftItemTariff(appointmentID, classID, obj.ItemID, trxDate, function (result) {
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
        if (IsValid(null, 'fsMPEntryPopup', 'mpEntryPopup')) {
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

    window.onCboDrugMSUomEndCallback = function (s, e) {
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

    window.onCbpDrugMSEndCallback = function (s, e) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else {
                $('#containerEntryDrugMS').hide();
                $('#<%=hdnDraftChargesID.ClientID %>').val(s.cpTransactionID);
                cbpMainPopup.PerformCallback('loadaftersave');
            }
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail') {
                showToast('Delete Failed', 'Error Message : ' + param[2]);
                $('#<%=hdnDraftChargesID.ClientID %>').val(s.cpTransactionID);
                cbpMainPopup.PerformCallback('loadaftersave');
            }
            else {
                $('#containerEntry').hide();
                $('#<%=hdnDraftChargesID.ClientID %>').val(s.cpTransactionID);
                cbpMainPopup.PerformCallback('loadaftersave');
            }
        }
        else if (param[0] == 'switch') {
            if (param[1] == 'fail') {
                showToast('Switch Failed', 'Error Message : ' + param[2]);
                $('#<%=hdnDraftChargesID.ClientID %>').val(s.cpTransactionID);
                cbpMainPopup.PerformCallback('loadaftersave');
            }
            else {
                cbpMainPopup.PerformCallback('load');
            }
        }
        else if (param[0] == 'void') {
            if (param[1] == 'fail') {
                showToast('Void Failed', 'Error Message : ' + param[2]);
            }
            else {
                cbpMainPopup.PerformCallback('load');
            }
        }
        else if (param[0] == 'approve') {
            if (param[1] == 'fail') {
                showToast('Approve Failed', 'Error Message : ' + param[2]);
            }
            else {
                cbpMainPopup.PerformCallback('load');
            }
        }

        calculateAllTotal();
        hideLoadingPanel();
        var isAdd = $('#<%=hdnIsAdd.ClientID %>').val();
        var isEditable = $('#<%=hdnIsEditable.ClientID %>').val();
        SetCustomVisibilityControl(isAdd, isEditable);
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

    function getDrugMSTotalPatient() {
        return parseFloat($('#<%=hdnDrugMSAllTotalPatient.ClientID %>').val());
    }
    function getDrugMSTotalPayer() {
        return parseFloat($('#<%=hdnDrugMSAllTotalPayer.ClientID %>').val());
    }

    window.onCboDrugMSChargeClassIDValueChanged = function (s, e) {
        getDrugMSTariff();
    }

    function getDrugMSTariff() {
        showLoadingPanel();
        var itemID = $('#<%=hdnDrugMSItemID.ClientID %>').val();
        var appointmentID = $('#<%=hdnAppointmentID.ClientID %>').val();
        var classID = cboDrugMSChargeClassID.GetValue();
        var trxDate = getTrxDate();

        Methods.getDraftItemTariff(appointmentID, classID, itemID, trxDate, function (result) {
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

    $('#btnDrugMSCancel').live('click', function () {
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

    //#End Region Drug

    //#Region Logistic
    var objLogistic = null;
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
        showToastConfirmation('Are You Sure Want To Delete?', function (result) {
            if (result) {
                var obj = rowToObject($row);
                var param = 'delete|' + obj.ID;
                cbpLogistic.PerformCallback(param);
            }
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

        var appointmentID = $('#<%=hdnAppointmentID.ClientID %>').val();
        var classID = obj.ChargeClassID;
        var trxDate = getTrxDate();
        Methods.getDraftItemTariff(appointmentID, classID, obj.ItemID, trxDate, function (result) {
            $('#<%=hdnLogisticDiscountAmount.ClientID %>').val(result.DiscountAmount);
            $('#<%=hdnLogisticCoverageAmount.ClientID %>').val(result.CoverageAmount);
            $('#<%=hdnLogisticIsDicountInPercentage.ClientID %>').val(result.IsDiscountInPercentage ? '1' : '0');
            $('#<%=hdnLogisticIsCoverageInPercentage.ClientID %>').val(result.IsCoverageInPercentage ? '1' : '0');
            $('#<%=hdnLogisticCostAmount.ClientID %>').val(result.CostAmount);
            hideLoadingPanel();
        });
    });

    $('#lblLogisticAddData').live('click', function () {
        if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
            $('#<%=hdnTempTotalPatient.ClientID %>').val(getLogisticTotalPatient());
            $('#<%=hdnTempTotalPayer.ClientID %>').val(getLogisticTotalPayer());

            if (typeof onAddRecordSetControlDisabled == 'function')
                onAddRecordSetControlDisabled();
            $('#containerEntryLogistic').show();
            //showLoadingPanel();
            cboLogisticLocation.SetEnabled(true);
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

    //#region Item
    function onGetLogisticItemFilterExpression() {
        var locationID = cboLogisticLocation.GetValue();
        var filterExpression = $('#<%=hdnLedLogisticItemFilterExpression.ClientID %>').val().replace('[LocationID]', locationID);
        if ($('#<%=hdnIsAllowOverIssued.ClientID %>').val() == '0') {
            filterExpression += " AND QuantityEND > 0 ";
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

    function onCboLogisticChargeClassIDValueChanged() {
        getLogisticTariff();
    }

    function getLogisticTariff() {
        showLoadingPanel();
        var itemID = $('#<%=hdnLogisticItemID.ClientID %>').val();
        var appointmentID = $('#<%=hdnAppointmentID.ClientID %>').val();
        var classID = cboLogisticChargeClassID.GetValue();
        var trxDate = getTrxDate();
        Methods.getDraftItemTariff(appointmentID, classID, itemID, trxDate, function (result) {
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

    function getLogisticItemUnitName(itemUnitID) {
        var value = cboLogisticUoM.GetValue();
        cboLogisticUoM.SetValue(itemUnitID);
        var text = cboLogisticUoM.GetText();
        cboLogisticUoM.SetValue(value);
        return text;
    }

    function getLogisticTotalPatient() {
        return parseFloat($('#<%=hdnLogisticAllTotalPatient.ClientID %>').val());
    }
    function getLogisticTotalPayer() {
        return parseFloat($('#<%=hdnLogisticAllTotalPayer.ClientID %>').val());
    }

    window.onCboLogisticUomEndCallback = function (s, e) {
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

    $('#<%=txtLogisticQtyUsed.ClientID %>').live('change', function () {
        $('#<%=txtLogisticQtyCharged.ClientID %>').val($(this).val());
        $('#<%=txtLogisticQtyCharged.ClientID %>').change();
    });

    $('#<%=txtLogisticQtyCharged.ClientID %>').live('change', function () {
        var conversionValue = parseFloat($('#<%=hdnLogisticConversionValue.ClientID %>').val());
        var qty = parseFloat($(this).val());
        $('#<%=txtLogisticBaseQty.ClientID %>').val(conversionValue * qty);
        calculateLogisticTariffTotal();
        calculateLogisticDiscountTotal();
        calculateLogisticTotal();
    });

    $('#<%=txtLogisticPatient.ClientID %>').live('change', function () {
        var patientTotal = parseInt($(this).val());
        var total = parseInt($('#<%=txtLogisticTotal.ClientID %>').attr('hiddenVal'));
        var payerTotal = total - patientTotal;
        $('#<%=txtLogisticPayer.ClientID %>').val(payerTotal).trigger('changeValue');
    });

    $('#<%=txtLogisticPayer.ClientID %>').live('change', function () {
        var payerTotal = parseInt($(this).val());
        var total = parseInt($('#<%=txtLogisticTotal.ClientID %>').attr('hiddenVal'));
        var patientTotal = total - payerTotal;
        $('#<%=txtLogisticPatient.ClientID %>').val(patientTotal).trigger('changeValue');
    });

    $('#btnLogisticSave').live('click', function (evt) {
        if (IsValid(evt, 'fsTrxLogistic', 'mpTrxLogistic'))
            cbpLogistic.PerformCallback('save');
        return false;
    });

    $('#btnLogisticCancel').live('click', function () {
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

    window.onCbpLogisticEndCallback = function (s, e) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else {
                var transactionID = s.cpTransactionID;
                onAfterSaveRecordDtSuccess(transactionID);
                $('#containerEntryLogistic').hide();
                cbpMainPopup.PerformCallback('loadaftersave');
            }
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail') {
                showToast('Delete Failed', 'Error Message : ' + param[2]);
                $('#<%=hdnDraftChargesID.ClientID %>').val(s.cpTransactionID);
                cbpMainPopup.PerformCallback('loadaftersave');
            }
            else {
                $('#containerEntry').hide();
                $('#<%=hdnDraftChargesID.ClientID %>').val(s.cpTransactionID);
                cbpMainPopup.PerformCallback('loadaftersave');
            }
        }
        else if (param[0] == 'switch') {
            if (param[1] == 'fail') {
                showToast('Switch Failed', 'Error Message : ' + param[2]);
                $('#<%=hdnDraftChargesID.ClientID %>').val(s.cpTransactionID);
                cbpMainPopup.PerformCallback('loadaftersave');
            }
            else {
                cbpMainPopup.PerformCallback('load');
            }
        }
        else if (param[0] == 'void') {
            if (param[1] == 'fail') {
                showToast('Void Failed', 'Error Message : ' + param[2]);
            }
            else {
                cbpMainPopup.PerformCallback('load');
            }
        }
        else if (param[0] == 'approve') {
            if (param[1] == 'fail') {
                showToast('Approve Failed', 'Error Message : ' + param[2]);
            }
            else {
                cbpMainPopup.PerformCallback('load');
            }
        }

        calculateAllTotal();
        hideLoadingPanel();

        var isAdd = $('#<%=hdnIsAdd.ClientID %>').val();
        var isEditable = $('#<%=hdnIsEditable.ClientID %>').val();
        SetCustomVisibilityControl(isAdd, isEditable);
    }

    function setLogisticItemFilterExpression(transactionID) {
        $('#<%=hdnLedLogisticItemFilterExpression.ClientID %>').val("LocationID = [LocationID] AND GCItemType = '" + Constant.ItemGroupMaster.LOGISTIC + "' AND ItemID NOT IN (SELECT ItemID FROM PatientChargesDt WHERE TransactionID = " + transactionID + " AND IsDeleted = 0) AND IsDeleted = 0");
    }
    //End Region Logistic

    //#region Transaction No
    $('#lblDraftChargesNo.lblLink').live('click', function () {
        var filterExpression = "AppointmentID = '" + $('#<%=hdnAppointmentID.ClientID %>').val() + "'";
        openSearchDialog('draftpatientchargeshd', filterExpression, function (value) {
            $('#<%=txtDraftChargesNo.ClientID %>').val(value);
            ontxtDraftChargesNoChanged(value);
        });
    });

    $('#<%=txtDraftChargesNo.ClientID %>').live('change', function () {
        ontxtDraftChargesNoChanged($(this).val());
    });

    function ontxtDraftChargesNoChanged(value) {
        cbpMainPopup.PerformCallback('load');
    }
    //#endregion

    //#region Service Unit
    function getServiceUnitFilterFilterExpression() {
        var filterExpression = "<%:GetServiceUnitFilterFilterExpression() %>";
        return filterExpression;
    }
    $('#<%=lblServiceUnit.ClientID %>.lblLink').live('click', function () {
        if ($('#<%=txtDraftChargesNo.ClientID %>').val() != '')
            return;
        openSearchDialog('serviceunitperhealthcare', getServiceUnitFilterFilterExpression(), function (value) {
            $('#<%=txtServiceUnitCode.ClientID %>').val(value);
            onTxtServiceUnitCodeChanged(value);
        });
    });

    $('#<%=txtServiceUnitCode.ClientID %>').live('change', function () {
        onTxtServiceUnitCodeChanged($(this).val());
    });

    function onTxtServiceUnitCodeChanged(value) {
        var filterExpression = getServiceUnitFilterFilterExpression() + " AND ServiceUnitCode = '" + value + "'";
        Methods.getObject('GetvHealthcareServiceUnitList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val(result.HealthcareServiceUnitID);
                $('#<%=hdnServiceUnitID.ClientID %>').val(result.ServiceUnitID);
                $('#<%=txtServiceUnitName.ClientID %>').val(result.ServiceUnitName);
            }
            else {
                $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val('');
                $('#<%=hdnServiceUnitID.ClientID %>').val('');
                $('#<%=txtServiceUnitCode.ClientID %>').val('');
                $('#<%=txtServiceUnitName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    $('#<%=lvwService.ClientID %> .imgDelete.imgLink').live('click', function () {
        $row = $(this).closest('tr').parent().closest('tr');
        showToastConfirmation('Are You Sure Want To Delete?', function (result) {
            if (result) {
                var entity = rowToObject($row);
                $('#<%=hdnEntryID.ClientID %>').val(entity.ID);
                cbpViewServiceCtl.PerformCallback('delete');
            }
        });
    });

    $('#<%=lvwService.ClientID %> .imgEdit.imgLink').live('click', function () {
        $row = $(this).closest('tr').parent().closest('tr');
        var entity = rowToObject($row);

        $('#<%=hdnEntryID.ClientID %>').val(entity.ID);
        $('#containerEntry').show();
    });

    function onCbpMainPopupEndCallback(s, e) {
        var result = s.cpResult.split('|');
        if (result[0] == 'save') {
            if (result[1] != 'success') {
                if (result[2] != '')
                    showToast('Save Failed', 'Error Message : ' + result[2]);
                else
                    showToast('Save Failed', '');
            } else {
                $('#containerEntry').hide();
                $('#<%=hdnDraftChargesID.ClientID %>').val(s.cpTransactionID);
                cbpMainPopup.PerformCallback('loadaftersave');
            }
        }
        if (result[0] == 'proposed') {
            if (result[1] != 'success') {
                if (result[2] != '')
                    showToast('Proposed Failed', 'Error Message : ' + result[2]);
                else
                    showToast('Proposed Failed', '');
            } else {
                $('#containerEntry').hide();
                $('#<%=hdnDraftChargesID.ClientID %>').val(s.cpTransactionID);
                cbpMainPopup.PerformCallback('loadaftersave');
            }
        }
        if (result[0] == 'void') {
            if (result[1] != 'success') {
                if (result[2] != '')
                    showToast('Void Failed', 'Error Message : ' + result[2]);
                else
                    showToast('Void Failed', '');
            } else {
                $('#containerEntry').hide();
                $('#<%=hdnDraftChargesID.ClientID %>').val(s.cpTransactionID);
                cbpMainPopup.PerformCallback('loadaftersave');
            }
        }
        calculateAllTotal();
        hideLoadingPanel();

        var name = $(this).attr('contentid');
        if ($('#<%=hdnIsServiceTab.ClientID %>').val() == '1') {
            name = 'containerService';
        }
        else if ($('#<%=hdnIsDrugTab.ClientID %>').val() == '1') {
            name = 'containerDrug';
        }
        else if ($('#<%=hdnIsLogisticTab.ClientID %>').val() == '1') {
            name = 'containerLogistic';
        }

        $('#containerEntryService').hide();
        $('#containerEntryDrugMS').hide();

        $(this).addClass('selected');
        $('#' + name).removeAttr('style');
        $('#ulTabClinicTransactionCtl li').each(function () {
            var tempNameContainer = $(this).attr('contentid');
            if (tempNameContainer != name) {
                $(this).removeClass('selected');
                $('#' + tempNameContainer).attr('style', 'display:none');
            }
            else {
                $(this).addClass('selected');
            }
        });

        var isAdd = $('#<%=hdnIsAdd.ClientID %>').val();
        var isEditable = $('#<%=hdnIsEditable.ClientID %>').val();
        SetCustomVisibilityControl(isAdd, isEditable);
    }

    function onAddRecordSetControlDisabled() {
        $('#<%=lblServiceUnit.ClientID %>').attr('class', 'lblDisabled');
        $('#<%=txtServiceUnitCode.ClientID %>').attr('readonly', 'readonly');
    }


    function onAfterSaveAddRecordEntryPopup(param) {
        $('#containerEntry').hide();
        $('#<%=hdnDraftChargesID.ClientID %>').val(param);
        cbpMainPopup.PerformCallback('loadaftersave');
    }

    function onAfterProposedRecordEntryPopup(param) {
        $('#containerEntry').hide();
        $('#<%=hdnDraftChargesID.ClientID %>').val(param);
        cbpMainPopup.PerformCallback('loadaftersave');
    }

    function onAfterVoidRecordEntryPopup(param) {
        $('#containerEntry').hide();
        $('#<%=hdnDraftChargesID.ClientID %>').val(param);
        cbpMainPopup.PerformCallback('loadaftersave');
    }

    function getServiceTotalPatient() {
        return parseFloat($('#<%=hdnServiceAllTotalPatient.ClientID %>').val());
    }
    function getServiceTotalPayer() {
        return parseFloat($('#<%=hdnServiceAllTotalPayer.ClientID %>').val());
    }       
</script>
<input type="hidden" id="hdnParamHealthcareServiceUnitID" runat="server" value="" />
<input type="hidden" id="hdnLabHealthcareServiceUnitID" runat="server" value="" />
<input type="hidden" id="hdnServiceTransactionDtID" runat="server" value="" />
<input type="hidden" id="hdnServiceDtTotal" runat="server" value="" />
<input type="hidden" id="hdnTariffComp1Text" runat="server" value="" />
<input type="hidden" id="hdnTariffComp2Text" runat="server" value="" />
<input type="hidden" id="hdnTariffComp3Text" runat="server" value="" />
<input type="hidden" id="hdnGCCustomerType" runat="server" value="" />
<input type="hidden" id="hdnServiceUnitCodeFrom" runat="server" value="" />
<input type="hidden" id="hdnServiceUnitNameFrom" runat="server" value="" />
<input type="hidden" id="hdnTempTotalPatient" runat="server" value="" />
<input type="hidden" id="hdnTempTotalPayer" runat="server" value="" />
<input type="hidden" id="hdnIsServiceTab" runat="server" value="" />
<input type="hidden" id="hdnIsDrugTab" runat="server" value="" />
<input type="hidden" id="hdnIsLogisticTab" runat="server" value="" />
<input type="hidden" id="hdnDrugMSTransactionDtID" runat="server" value="" />
<input type="hidden" id="hdnLocationDrugID" runat="server" value="" />
<input type="hidden" id="hdnLocationLogisticID" runat="server" value="" />
<input type="hidden" id="hdnLogisticTransactionDtID" runat="server" value="" />
<input type="hidden" value="0" id="hdnIsAllowOverIssued" runat="server" />
<div style="height: 600px; overflow-y: scroll;">
    <dxcp:ASPxCallbackPanel ID="cbpMainPopup" runat="server" Width="100%" ClientInstanceName="cbpMainPopup"
        ShowLoadingPanel="false" OnCallback="cbpMainPopup_Callback">
        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpMainPopupEndCallback(s,e); }" />
        <PanelCollection>
            <dx:PanelContent ID="PanelContent2">
                <fieldset id="fsMPEntry" style="margin: 0">
                    <asp:Panel runat="server" ID="Panel1" Style="width: 100%; margin-left: auto; margin-right: auto;
                        position: relative; font-size: 0.95em;">
                        <input type="hidden" value="" id="hdnDraftChargesID" runat="server" />
                        <input type="hidden" value="" id="hdnDepartmentFromID" runat="server" />
                        <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
                        <input type="hidden" value="" id="hdnGCTransactionStatus" runat="server" />
                        <input type="hidden" value="" id="hdnHealthcareServiceUnitFromID" runat="server" />
                        <input type="hidden" value="" id="hdnImagingServiceUnitID" runat="server" />
                        <input type="hidden" value="" id="hdnPhysicianID" runat="server" />
                        <input type="hidden" value="" id="hdnPhysicianCode" runat="server" />
                        <input type="hidden" value="" id="hdnPhysicianName" runat="server" />
                        <input type="hidden" value="" id="hdnLaboratoryServiceUnitID" runat="server" />
                        <input type="hidden" value="" id="hdnVisitID" runat="server" />
                        <input type="hidden" value="" id="hdnAppointmentID" runat="server" />
                        <input type="hidden" value="" id="hdnIsAdd" runat="server" />
                        <input type="hidden" value="" id="hdnIsEditable" runat="server" />
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
                                                <label class="lblLink" id="lblDraftChargesNo">
                                                    <%=GetLabel("No. Draft Transaksi")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDraftChargesNo" Width="232px" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <%=GetLabel("Tanggal") %>
                                                -
                                                <%=GetLabel("Jam") %>
                                            </td>
                                            <td>
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td style="padding-right: 1px; width: 145px">
                                                            <asp:TextBox ID="txtDraftChargesDate" Width="120px" CssClass="datepicker" runat="server" />
                                                        </td>
                                                        <td style="width: 5px">
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtDraftChargesTime" Width="80px" CssClass="time" runat="server"
                                                                Style="text-align: center" />
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
                                                <label class="lblNormal">
                                                    <%=GetLabel("No. Referensi")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtReferenceNo" Width="100%" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblLink lblMandatory lblDisabled" runat="server" id="lblServiceUnit">
                                                    <%=GetLabel("Unit Perawatan")%></label>
                                            </td>
                                            <td>
                                                <input type="hidden" id="hdnHealthcareServiceUnitID" value="" runat="server" />
                                                <input type="hidden" id="hdnServiceUnitID" value="" runat="server" />
                                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                    <colgroup>
                                                        <col style="width: 120px" />
                                                        <col style="width: 3px" />
                                                        <col />
                                                    </colgroup>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtServiceUnitCode" ReadOnly="true" Width="120px" runat="server" />
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtServiceUnitName" ReadOnly="true" Width="100%" runat="server" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                                <label>
                                                    <%=GetLabel("Catatan")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtNotes" Width="100%" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label>
                                                    <%=GetLabel("Status")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtStatus" Width="120px" runat="server" Style="text-align: center;
                                                    color: Red" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <div class="containerUlTabPage">
                            <ul class="ulTabPage" id="ulTabClinicTransactionCtl">
                                <li class="selected" contentid="containerService">
                                    <%=GetLabel("PELAYANAN") %></li>
                                <li contentid="containerDrug">
                                    <%=GetLabel("OBAT & ALKES") %></li>
                                <li contentid="containerLogistic">
                                    <%=GetLabel("BARANG UMUM") %></li>
                            </ul>
                        </div>
                        <div id="containerService" class="containerTransDt">
                            <table class="tblService" style="width: 100%">
                                <tr>
                                    <td colspan="2">
                                        <div id="containerEntryService" style="margin-top: 4px; display: none;">
                                            <div class="pageTitle">
                                                <%=GetLabel("Tambah Atau Ubah Data")%></div>
                                            <fieldset id="fsTrxService" style="margin: 0">
                                                <table class="tblEntryDetail" style="width: 100%">
                                                    <colgroup>
                                                        <col style="width: 40%" />
                                                        <col style="width: 33%" />
                                                        <col />
                                                    </colgroup>
                                                    <tr>
                                                        <td valign="top">
                                                            <table>
                                                                <colgroup>
                                                                    <col style="width: 130px" />
                                                                </colgroup>
                                                                <tr>
                                                                    <td class="tdLabel">
                                                                        <label class="lblLink lblMandatory" id="lblServiceItem">
                                                                            <%=GetLabel("Pelayanan")%></label>
                                                                    </td>
                                                                    <td>
                                                                        <input type="hidden" value="" id="hdnEntryID" runat="server" />
                                                                        <input type="hidden" value="" id="hdnServiceItemID" runat="server" />
                                                                        <input type="hidden" value="" id="hdnServiceRevenueSharingID" runat="server" />
                                                                        <input type="hidden" value="" id="hdnServiceItemUnit" runat="server" />
                                                                        <input type="hidden" value="" id="hdnServiceBaseTariff" runat="server" />
                                                                        <input type="hidden" value="" id="hdnServiceDiscountAmount" runat="server" />
                                                                        <input type="hidden" value="" id="hdnServiceCoverageAmount" runat="server" />
                                                                        <input type="hidden" value="" id="hdnServiceIsDicountInPercentage" runat="server" />
                                                                        <input type="hidden" value="" id="hdnServiceIsCoverageInPercentage" runat="server" />
                                                                        <input type="hidden" value="" id="hdnServiceBaseCITOAmount" runat="server" />
                                                                        <input type="hidden" value="" id="hdnServiceIsCITOInPercentage" runat="server" />
                                                                        <input type="hidden" value="" id="hdnServiceBaseComplicationAmount" runat="server" />
                                                                        <input type="hidden" value="" id="hdnServiceIsComplicationInPercentage" runat="server" />
                                                                        <input type="hidden" value="" id="hdnServiceItemFilterExpression" runat="server" />
                                                                        <input type="hidden" value="" id="hdnServicePrice" runat="server" />
                                                                        <input type="hidden" value="" id="hdnServicePriceComp1" runat="server" />
                                                                        <input type="hidden" value="" id="hdnServicePriceComp2" runat="server" />
                                                                        <input type="hidden" value="" id="hdnServicePriceComp3" runat="server" />
                                                                        <input type="hidden" value="" id="hdnServiceBasePriceComp1" runat="server" />
                                                                        <input type="hidden" value="" id="hdnServiceBasePriceComp2" runat="server" />
                                                                        <input type="hidden" value="" id="hdnServiceBasePriceComp3" runat="server" />
                                                                        <input type="hidden" value="" id="hdnServiceTransactionID" runat="server" />
                                                                        <input type="hidden" value="" id="hdnServiceTransactionDate" runat="server" />
                                                                        <input type="hidden" value="" id="hdnServiceTransactionTime" runat="server" />
                                                                        <input type="hidden" value="" id="Hidden2" runat="server" />
                                                                        <input type="hidden" value="" id="hdnServiceVisitID" runat="server" />
                                                                        <input type="hidden" value="" id="hdnServiceUnitName" runat="server" />
                                                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                                            <colgroup>
                                                                                <col style="width: 30%" />
                                                                                <col style="width: 3px" />
                                                                                <col />
                                                                            </colgroup>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtServiceItemCode" Width="100%" runat="server" />
                                                                                </td>
                                                                                <td>
                                                                                    &nbsp;
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtServiceItemName" ReadOnly="true" Width="100%" runat="server" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="tdLabel">
                                                                        <label class="lblLink lblMandatory" id="lblPhysician">
                                                                            <%=GetLabel("Dokter/Paramedis")%></label>
                                                                    </td>
                                                                    <td>
                                                                        <input type="hidden" value="" id="hdnServicePhysicianID" runat="server" />
                                                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                                            <colgroup>
                                                                                <col style="width: 30%" />
                                                                                <col style="width: 3px" />
                                                                                <col />
                                                                            </colgroup>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtServicePhysicianCode" Width="100%" runat="server" />
                                                                                </td>
                                                                                <td>
                                                                                    &nbsp;
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtServicePhysicianName" ReadOnly="true" Width="100%" runat="server" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="tdLabel">
                                                                        <label class="lblDisabled" id="lblTestPartner">
                                                                            <%=GetLabel("Test Partner")%></label>
                                                                    </td>
                                                                    <td>
                                                                        <input type="hidden" value="" id="hdnTestPartnerID" runat="server" />
                                                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                                            <colgroup>
                                                                                <col style="width: 30%" />
                                                                                <col style="width: 3px" />
                                                                                <col />
                                                                            </colgroup>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtTestPartnerCode" Width="100%" runat="server" ReadOnly="true" />
                                                                                </td>
                                                                                <td>
                                                                                    &nbsp;
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtTestPartnerName" ReadOnly="true" Width="100%" runat="server" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="tdLabel">
                                                                        <label>
                                                                        </label>
                                                                    </td>
                                                                    <td>
                                                                        <input type="hidden" id="hdnDefaultTariffComp" runat="server" value="1" />
                                                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                                            <colgroup>
                                                                                <col style="width: 20px" />
                                                                                <col style="width: 100px" />
                                                                                <col style="width: 20px" />
                                                                            </colgroup>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:CheckBox ID="chkServiceIsVariable" runat="server" />
                                                                                </td>
                                                                                <td class="tdLabel">
                                                                                    <label>
                                                                                        <%=GetLabel("Variable")%></label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:CheckBox ID="chkServiceIsUnbilledItem" runat="server" />
                                                                                </td>
                                                                                <td class="tdLabel">
                                                                                    <label>
                                                                                        <%=GetLabel("Tidak Ditagihkan")%></label>
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
                                                                        <dxe:ASPxComboBox ID="cboServiceChargeClassID" ClientInstanceName="cboServiceChargeClassID"
                                                                            Width="100%" runat="server">
                                                                            <ClientSideEvents ValueChanged="function(s,e){ onCboServiceChargeClassIDValueChanged(); }" />
                                                                        </dxe:ASPxComboBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="tdLabel">
                                                                        <label>
                                                                            <%=GetLabel("Harga Satuan")%></label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtServiceUnitTariff" Width="100px" CssClass="txtCurrency" runat="server" />
                                                                        <input type="button" id="btnEditUnitTariff" title='<%=GetLabel("Unit Tariff Component") %>'
                                                                            value="..." style="width: 10%" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="tdLabel">
                                                                        <label>
                                                                            <%=GetLabel("Jumlah")%></label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtServiceQty" Width="100px" CssClass="number" runat="server" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td valign="top">
                                                            <table>
                                                                <colgroup>
                                                                    <col style="width: 100px" />
                                                                    <col style="width: 20px" />
                                                                    <col />
                                                                </colgroup>
                                                                <tr>
                                                                    <td class="tdLabel">
                                                                        <label>
                                                                            <%=GetLabel("Harga")%></label>
                                                                    </td>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ReadOnly="true" ID="txtServiceTariff" Width="200px" CssClass="txtCurrency"
                                                                            runat="server" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="tdLabel">
                                                                        <label>
                                                                            <%=GetLabel("CITO")%></label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:CheckBox ID="chkServiceIsCITO" runat="server" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtServiceCITO" ReadOnly="true" Width="200px" CssClass="txtCurrency"
                                                                            runat="server" />
                                                                    </td>
                                                                </tr>
                                                                <tr style="display: none">
                                                                    <td class="tdLabel">
                                                                        <label>
                                                                            <%=GetLabel("Penyulit")%></label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:CheckBox ID="chkServiceIsComplication" runat="server" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtServiceComplication" ReadOnly="true" Width="200px" CssClass="txtCurrency"
                                                                            runat="server" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="tdLabel">
                                                                        <label>
                                                                            <%=GetLabel("Diskon")%></label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:CheckBox ID="chkServiceIsDiscount" runat="server" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtServiceDiscount" Width="200px" CssClass="txtCurrency" runat="server" />
                                                                        <input type="button" class="btnEditDiscount" title='<%=GetLabel("Discount Component") %>'
                                                                            value="..." style="width: 10%" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td valign="top">
                                                            <table>
                                                                <colgroup>
                                                                    <col style="width: 100px" />
                                                                </colgroup>
                                                                <tr>
                                                                    <td class="tdLabel">
                                                                        <label>
                                                                            <%=GetLabel("Pasien")%></label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtServicePatient" ReadOnly="true" Width="200px" CssClass="txtCurrency"
                                                                            runat="server" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="tdLabel">
                                                                        <label>
                                                                            <%=GetLabel("Instansi")%></label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtServicePayer" ReadOnly="true" Width="200px" CssClass="txtCurrency"
                                                                            runat="server" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="tdLabel">
                                                                        <label>
                                                                            <%=GetLabel("Total")%></label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtServiceTotal" ReadOnly="true" Width="200px" CssClass="txtCurrency"
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
                                                                    <td colspan="2">
                                                                        <table>
                                                                            <tr>
                                                                                <td>
                                                                                    <input type="button" id="btnServiceSave" value='<%= GetLabel("Save")%>' />
                                                                                </td>
                                                                                <td>
                                                                                    <input type="button" id="btnServiceCancel" value='<%= GetLabel("Cancel")%>' />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <colgroup>
                                                        <col style="width: 100px" />
                                                        <col />
                                                    </colgroup>
                                                </table>
                                            </fieldset>
                                        </div>
                                        <dxcp:ASPxCallbackPanel ID="cbpViewServiceCtl" runat="server" Width="100%" ClientInstanceName="cbpViewServiceCtl"
                                            ShowLoadingPanel="false" OnCallback="cbpViewServiceCtl_Callback">
                                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewServiceCtlEndCallback(s,e); }" />
                                            <PanelCollection>
                                                <dx:PanelContent ID="PanelContent1" runat="server">
                                                    <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                                        position: relative; font-size: 0.95em;">
                                                        <input type="hidden" id="hdnServiceAllTotalPatient" runat="server" value="" />
                                                        <input type="hidden" id="hdnServiceAllTotalPayer" runat="server" value="" />
                                                        <asp:ListView ID="lvwService" runat="server">
                                                            <LayoutTemplate>
                                                                <table id="tblView" runat="server" class="grdService grdNormal notAllowSelect" cellspacing="0"
                                                                    rules="all">
                                                                    <tr>
                                                                        <th style="width: 80px" rowspan="2">
                                                                        </th>
                                                                        <%--                                <th style="width:80px" rowspan="2">
                                    <div style="text-align:left;padding-left:3px">
                                        <%=GetLabel("Kode")%>
                                    </div>
                                </th>
                                                                        --%>
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
                                                                        <th rowspan="2" style="width: 80px">
                                                                            <div style="text-align: right; padding-right: 3px">
                                                                                <%=GetLabel("Harga Satuan")%>
                                                                            </div>
                                                                        </th>
                                                                        <th rowspan="2" style="width: 50px">
                                                                            <div style="text-align: right; padding-right: 3px">
                                                                                <%=GetLabel("Jumlah")%>
                                                                            </div>
                                                                        </th>
                                                                        <th colspan="3">
                                                                            <%=GetLabel("HARGA")%>
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
                                                                        <th style="width: 80px">
                                                                            <div style="text-align: right; padding-right: 3px">
                                                                                <%=GetLabel("Harga")%>
                                                                            </div>
                                                                        </th>
                                                                        <th style="width: 90px">
                                                                            <div style="text-align: right; padding-right: 3px">
                                                                                <%=GetLabel("CITO")%>
                                                                            </div>
                                                                        </th>
                                                                        <th style="width: 80px; display: none">
                                                                            <div style="text-align: right; padding-right: 3px">
                                                                                <%=GetLabel("Penyulit")%>
                                                                            </div>
                                                                        </th>
                                                                        <th style="width: 80px">
                                                                            <div style="text-align: right; padding-right: 3px">
                                                                                <%=GetLabel("Diskon")%>
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
                                                                        <td colspan="8" align="right" style="padding-right: 3px">
                                                                            <%=GetLabel("TOTAL") %>
                                                                        </td>
                                                                        <td align="right" style="padding-right: 9px" id="tdServiceTotalPayer" class="tdServiceTotalPayer"
                                                                            runat="server">
                                                                        </td>
                                                                        <td align="right" style="padding-right: 9px" id="tdServiceTotalPatient" class="tdServiceTotalPatient"
                                                                            runat="server">
                                                                        </td>
                                                                        <td align="right" style="padding-right: 9px" id="tdServiceTotal" class="tdServiceTotal"
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
                                                                                    <%--<td>
                                                                                        <img class="imgServiceParamedic imgLink" <%# IsEditable.ToString() == "True" && IsShowParamedicTeam.ToString() == "True" ?  "" : "style='display:none'" %>
                                                                                            title='<%=GetLabel("Tim Dokter")%>' src='<%# ResolveUrl("~/Libs/Images/Button/paramedic_team.png")%>'
                                                                                            alt="" style="margin-right: 2px" />
                                                                                    </td>--%>
                                                                                    <td>
                                                                                        <img class="imgServiceHasil" <%# Eval("IsHasTestResult").ToString() == "True" ?  "" : "style='display:none'" %>
                                                                                            title='<%=GetLabel("Sudah Ada Hasil")%>' src='<%# ResolveUrl("~/Libs/Images/Button/verify.png")%>'
                                                                                            alt="" style="margin-right: 2px" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgServiceEdit <%# IsEditable.ToString() == "False" || Eval("IsVerified").ToString() == "True" || Eval("IsReviewed").ToString() == "True" ? "imgDisabled" : "imgLink"%>"
                                                                                            title='<%=GetLabel("Edit")%>' src='<%# IsEditable.ToString() == "False" || Eval("IsVerified").ToString() == "True" || Eval("IsReviewed").ToString() == "True" ? ResolveUrl("~/Libs/Images/Button/edit_disabled.png") : ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                            alt="" style="margin-right: 2px" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgServiceDelete <%# IsEditable.ToString() == "False" || Eval("IsHasTestResult").ToString() == "True" || Eval("IsReviewed").ToString() == "True" || Eval("IsVerified").ToString() == "True" ? "imgDisabled" : "imgLink"%>"
                                                                                            title='<%=GetLabel("Delete")%>' src='<%# IsEditable.ToString() == "False" || Eval("IsHasTestResult").ToString() == "True" || Eval("IsReviewed").ToString() == "True" || Eval("IsVerified").ToString() == "True" ? ResolveUrl("~/Libs/Images/Button/delete_disabled.png") : ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                            alt="" style="margin-right: 2px" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgServiceVerified" <%# Eval("IsVerified").ToString() == "True" ?  "" : "style='display:none'" %>
                                                                                            title='<%=GetLabel("Verified")%>' src='<%# ResolveUrl("~/Libs/Images/Button/verify.png")%>'
                                                                                            alt="" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgIsPackageItem imgLink" <%# Eval("IsPackageItem").ToString() == "True" ?  "" : "style='display:none'" %>
                                                                                            title='<%#: Eval("DetailItemInfo")%>' src='<%# IsEditable.ToString() == "False" || Eval("IsHasTestResult").ToString() == "True" || Eval("IsReviewed").ToString() == "True" || Eval("IsVerified").ToString() == "True" ? ResolveUrl("~/Libs/Images/Button/package_disabled.png") : ResolveUrl("~/Libs/Images/Button/package.png")%>'
                                                                                            alt="" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                            <input type="hidden" value='<%#: Eval("ID") %>' bindingfield="ID" />
                                                                            <input type="hidden" value='<%#: Eval("ItemID") %>' bindingfield="ItemID" />
                                                                            <input type="hidden" value='<%#: Eval("ItemCode") %>' bindingfield="ItemCode" />
                                                                            <input type="hidden" value='<%#: Eval("ItemName1") %>' bindingfield="ItemName1" />
                                                                            <input type="hidden" value='<%#: Eval("RevenueSharingID") %>' bindingfield="RevenueSharingID" />
                                                                            <input type="hidden" value='<%#: Eval("ParamedicID") %>' bindingfield="ParamedicID" />
                                                                            <input type="hidden" value='<%#: Eval("ParamedicCode") %>' bindingfield="ParamedicCode" />
                                                                            <input type="hidden" value='<%#: Eval("ParamedicName") %>' bindingfield="ParamedicName" />
                                                                            <input type="hidden" value='<%#: Eval("BusinessPartnerID") %>' bindingfield="TestPartnerID" />
                                                                            <input type="hidden" value='<%#: Eval("BusinessPartnerCode") %>' bindingfield="TestPartnerCode" />
                                                                            <input type="hidden" value='<%#: Eval("BusinessPartnerName") %>' bindingfield="TestPartnerName" />
                                                                            <input type="hidden" value='<%#: Eval("ChargeClassID") %>' bindingfield="ChargeClassID" />
                                                                            <input type="hidden" value='<%#: Eval("ChargedQuantity") %>' bindingfield="ChargedQuantity" />
                                                                            <input type="hidden" value='<%#: Eval("BaseTariff") %>' bindingfield="BaseTariff" />
                                                                            <input type="hidden" value='<%#: Eval("Tariff") %>' bindingfield="Tariff" />
                                                                            <input type="hidden" value='<%#: Eval("TariffComp1") %>' bindingfield="TariffComp1" />
                                                                            <input type="hidden" value='<%#: Eval("TariffComp2") %>' bindingfield="TariffComp2" />
                                                                            <input type="hidden" value='<%#: Eval("TariffComp3") %>' bindingfield="TariffComp3" />
                                                                            <input type="hidden" value='<%#: Eval("CostAmount") %>' bindingfield="CostAmount" />
                                                                            <input type="hidden" value='<%#: Eval("GrossLineAmount") %>' bindingfield="GrossLineAmount" />
                                                                            <input type="hidden" value='<%#: Eval("IsAllowCITO") %>' bindingfield="IsAllowCITO" />
                                                                            <input type="hidden" value='<%#: Eval("IsAllowComplication") %>' bindingfield="IsAllowComplication" />
                                                                            <input type="hidden" value='<%#: Eval("IsAllowDiscount") %>' bindingfield="IsAllowDiscount" />
                                                                            <input type="hidden" value='<%#: Eval("IsAllowVariable") %>' bindingfield="IsAllowVariable" />
                                                                            <input type="hidden" value='<%#: Eval("IsAllowUnbilledItem") %>' bindingfield="IsAllowUnbilledItem" />
                                                                            <input type="hidden" value='<%#: Eval("IsCITO") %>' bindingfield="IsCITO" />
                                                                            <input type="hidden" value='<%#: Eval("IsCITOInPercentage") %>' bindingfield="IsCITOInPercentage" />
                                                                            <input type="hidden" value='<%#: Eval("IsComplication") %>' bindingfield="IsComplication" />
                                                                            <input type="hidden" value='<%#: Eval("IsComplicationInPercentage") %>' bindingfield="IsComplicationInPercentage" />
                                                                            <input type="hidden" value='<%#: Eval("IsDiscount") %>' bindingfield="IsDiscount" />
                                                                            <input type="hidden" value='<%#: Eval("IsVariable") %>' bindingfield="IsVariable" />
                                                                            <input type="hidden" value='<%#: Eval("DefaultTariffComp") %>' bindingfield="DefaultTariffComp" />
                                                                            <input type="hidden" value='<%#: Eval("IsUnbilledItem") %>' bindingfield="IsUnbilledItem" />
                                                                            <input type="hidden" value='<%#: Eval("BaseCITOAmount") %>' bindingfield="BaseCITOAmount" />
                                                                            <input type="hidden" value='<%#: Eval("CITOAmount") %>' bindingfield="CITOAmount" />
                                                                            <input type="hidden" value='<%#: Eval("DiscountAmount") %>' bindingfield="DiscountAmount" />
                                                                            <input type="hidden" value='<%#: Eval("BaseComplicationAmount") %>' bindingfield="BaseComplicationAmount" />
                                                                            <input type="hidden" value='<%#: Eval("ComplicationAmount") %>' bindingfield="ComplicationAmount" />
                                                                            <input type="hidden" value='<%#: Eval("PatientAmount") %>' bindingfield="PatientAmount" />
                                                                            <input type="hidden" value='<%#: Eval("PayerAmount") %>' bindingfield="PayerAmount" />
                                                                            <input type="hidden" value='<%#: Eval("LineAmount") %>' bindingfield="LineAmount" />
                                                                            <input type="hidden" value='<%#: Eval("IsSubContractItem") %>' bindingfield="IsSubContractItem" />
                                                                        </div>
                                                                    </td>
                                                                    <td>
                                                                        <div style="padding: 3px">
                                                                            <div>
                                                                                <%#: Eval("ItemName1")%></div>
                                                                            <div>
                                                                                <span style="font-style: italic">
                                                                                    <%#: Eval("ItemCode") %></span>, <span style="color: Blue">
                                                                                        <%#: Eval("ParamedicName")%></span>
                                                                            </div>
                                                                            <div <%# Eval("BusinessPartnerName").ToString() != "" ?  "" : "style='display:none'" %>>
                                                                                <%#: Eval("BusinessPartnerName")%></div>
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
                                                                                <%#: Eval("ChargedQuantity")%></div>
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
                                                                                <%#: Eval("CITOAmount", "{0:N}")%></div>
                                                                        </div>
                                                                    </td>
                                                                    <td style="display: none">
                                                                        <div style="padding: 3px; text-align: right;">
                                                                            <div>
                                                                                <%#: Eval("ComplicationAmount", "{0:N}")%></div>
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
                                                                                <%#: Eval("CreatedByFullName")%></div>
                                                                            <div>
                                                                                <%#: Eval("CreatedDateInString")%></div>
                                                                        </div>
                                                                    </td>
                                                                    <td <%# IsShowSwitchIcon.ToString() == "True" && IsEditable.ToString() == "True" ?  "" : "style='display:none'" %>
                                                                        valign="middle">
                                                                        <img style="margin-left: 2px" class="imgServiceSwitch imgLink" title='<%=GetLabel("Switch")%>'
                                                                            src='<%# ResolveUrl("~/Libs/Images/Button/switch.png")%>' alt="" />
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                        </asp:ListView>
                                                    </asp:Panel>
                                                </dx:PanelContent>
                                            </PanelCollection>
                                        </dxcp:ASPxCallbackPanel>
                                        <div class="imgLoadingGrdView" id="containerImgLoadingView">
                                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div style="width: 100%; text-align: center">
                                <span class="lblLink" id="lblServiceAddData" style="margin-right: 200px;">
                                    <%= GetLabel("Tambah Data")%></span>
                            </div>
                        </div>
                        <div id="containerDrug" style="display: none" class="containerTransDt">
                            <table class="tblDrug" style="width: 100%">
                                <tr>
                                    <td colspan="2">
                                        <div id="containerEntryDrugMS" style="margin-top: 4px; display: none;">
                                            <div class="pageTitle">
                                                <%=GetLabel("Tambah Atau Ubah Data")%></div>
                                            <fieldset id="fsTrxDrugMS" style="margin: 0">
                                                <table class="tblEntryDetail">
                                                    <tr>
                                                        <td>
                                                            <table>
                                                                <colgroup>
                                                                    <col style="width: 100px" />
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
                                                                        <dxe:ASPxComboBox ID="cboDrugMSLocation" ClientInstanceName="cboDrugMSLocation" Width="200px"
                                                                            runat="server">
                                                                        </dxe:ASPxComboBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="tdLabel">
                                                                        <label class="lblLink" id="lblDrugMSItem">
                                                                            <%=GetLabel("Obat")%></label>
                                                                    </td>
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
                                                                            <ClientSideEvents ValueChanged="function(s,e){ onCboDrugMSChargeClassIDValueChanged(s,e); }" />
                                                                        </dxe:ASPxComboBox>
                                                                        <input type="hidden" id="hdnDrugMSAllTotalPatient" runat="server" value="" />
                                                                        <input type="hidden" id="hdnDrugMSAllTotalPayer" runat="server" value="" />
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
                                                                        <asp:TextBox ID="txtDrugMSQtyUsed" Width="100%" CssClass="number min" min="0.1" runat="server" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtDrugMSQtyCharged" Width="100%" CssClass="number min" min="0.1"
                                                                            runat="server" />
                                                                    </td>
                                                                    <td>
                                                                        <input type="hidden" value="" id="hdnDrugMSDefaultUoM" runat="server" />
                                                                        <dxe:ASPxComboBox ID="cboDrugMSUoM" runat="server" ClientInstanceName="cboDrugMSUoM"
                                                                            Width="200px" OnCallback="cboDrugMSUoM_Callback">
                                                                            <ClientSideEvents EndCallback="function(s,e) { onCboDrugMSUomEndCallback(s,e); }"
                                                                                ValueChanged="function(s,e){ setDrugMSItemUnitConversionText(); }" />
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
                                                                        <asp:TextBox ID="txtDrugMSPriceDiscount" ReadOnly="true" CssClass="txtCurrency" Width="100%"
                                                                            runat="server" />
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
                                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e) { onCbpDrugMSEndCallback(s,e); }" />
                                            <PanelCollection>
                                                <dx:PanelContent ID="PanelContent5" runat="server">
                                                    <asp:Panel runat="server" ID="Panel2" Style="width: 100%; margin-left: auto; margin-right: auto;
                                                        position: relative; font-size: 0.95em;">
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
                                                                    <tr id="Tr2" class="trFooter" runat="server">
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
                                                                        <img style="margin-left: 2px" class="imgDrugMSSwitch imgLink" title='<%=GetLabel("Switch")%>'
                                                                            src='<%# ResolveUrl("~/Libs/Images/Button/switch.png")%>' alt="" />
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                        </asp:ListView>
                                                    </asp:Panel>
                                                </dx:PanelContent>
                                            </PanelCollection>
                                        </dxcp:ASPxCallbackPanel>
                                    </td>
                                </tr>
                            </table>
                            <div style="width: 100%; text-align: center">
                                <span class="lblLink" id="lblDrugMSAddData" style="margin-right: 200px;">
                                    <%= GetLabel("Tambah Data")%></span>
                            </div>
                        </div>
                        <div id="containerLogistic" style="display: none" class="containerTransDt">
                            <table class="tblLogistic" style="width: 100%">
                                <tr>
                                    <td colspan="2">
                                        <div id="containerEntryLogistic" style="margin-top: 4px; display: none;">
                                            <div class="pageTitle">
                                                <%=GetLabel("Tambah Atau Ubah Data")%></div>
                                            <fieldset id="fsTrxLogistic" style="margin: 0">
                                                <table class="tblEntryDetail">
                                                    <tr>
                                                        <td>
                                                            <table>
                                                                <colgroup>
                                                                    <col style="width: 120px" />
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
                                                                        <dxe:ASPxComboBox ID="cboLogisticLocation" ClientInstanceName="cboLogisticLocation"
                                                                            Width="100%" runat="server" OnCallback="cboLogisticLocation_Callback">
                                                                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ hideLoadingPanel(); }" />
                                                                        </dxe:ASPxComboBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="tdLabel">
                                                                        <label class="lblMandatory lblLink" id="lblLogisticItem">
                                                                            <%=GetLabel("Barang Umum")%></label>
                                                                    </td>
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
                                                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                                            <colgroup>
                                                                                <col style="width: 30%" />
                                                                                <col style="width: 3px" />
                                                                                <col />
                                                                            </colgroup>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtLogisticItemCode" Width="100%" runat="server" />
                                                                                </td>
                                                                                <td>
                                                                                    &nbsp;
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtLogisticItemName" ReadOnly="true" Width="100%" runat="server" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="tdLabel">
                                                                        <label>
                                                                            <%=GetLabel("Charge Class")%></label>
                                                                    </td>
                                                                    <td>
                                                                        <dxe:ASPxComboBox ID="cboLogisticChargeClassID" ClientInstanceName="cboLogisticChargeClassID"
                                                                            Width="200px" runat="server">
                                                                            <ClientSideEvents ValueChanged="function(s,e){ onCboLogisticChargeClassIDValueChanged(); }" />
                                                                        </dxe:ASPxComboBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="tdLabel">
                                                                        <label>
                                                                            <%=GetLabel("Harga Satuan")%></label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtLogisticUnitTariff" ReadOnly="true" CssClass="txtCurrency" Width="100%"
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
                                                                        <asp:TextBox ID="txtLogisticQtyUsed" Width="100%" CssClass="number min" min="0.1"
                                                                            runat="server" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtLogisticQtyCharged" Width="100%" CssClass="number min" min="0.1"
                                                                            runat="server" />
                                                                    </td>
                                                                    <td>
                                                                        <input type="hidden" value="" id="hdnLogisticDefaultUoM" runat="server" />
                                                                        <dxe:ASPxComboBox ID="cboLogisticUoM" runat="server" ClientInstanceName="cboLogisticUoM"
                                                                            Width="200px" OnCallback="cboLogisticUoM_Callback">
                                                                            <ClientSideEvents EndCallback="function(s,e) { onCboLogisticUomEndCallback(); }"
                                                                                ValueChanged="function(s,e){ setLogisticItemUnitConversionText(); }" />
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
                                                                            <%=GetLabel("Base Quantity")%></label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtLogisticBaseQty" ReadOnly="true" CssClass="number" Width="100%"
                                                                            runat="server" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtLogisticConversion" ReadOnly="true" Width="100%" runat="server" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                    <td>
                                                                        <div class="lblComponent">
                                                                            <%=GetLabel("Tarif")%></div>
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
                                                                        <asp:TextBox ID="txtLogisticPriceTariff" ReadOnly="true" CssClass="txtCurrency" Width="100%"
                                                                            runat="server" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtLogisticPriceDiscount" ReadOnly="true" CssClass="txtCurrency"
                                                                            Width="100%" runat="server" />
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
                                                                        <asp:TextBox ID="txtLogisticPatient" CssClass="txtCurrency" Width="100%" runat="server" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtLogisticPayer" CssClass="txtCurrency" Width="100%" runat="server" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtLogisticTotal" ReadOnly="true" CssClass="txtCurrency" Width="100%"
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
                                                                        <input type="button" id="btnLogisticSave" value='<%= GetLabel("Save")%>' />
                                                                    </td>
                                                                    <td>
                                                                        <input type="button" id="btnLogisticCancel" value='<%= GetLabel("Cancel")%>' />
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
                                        <dxcp:ASPxCallbackPanel ID="cbpLogistic" runat="server" Width="100%" ClientInstanceName="cbpLogistic"
                                            ShowLoadingPanel="false" OnCallback="cbpLogistic_Callback">
                                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e) { onCbpLogisticEndCallback(s,e); }" />
                                            <PanelCollection>
                                                <dx:PanelContent ID="PanelContent6" runat="server">
                                                    <asp:Panel runat="server" ID="Panel3" Style="width: 100%; margin-left: auto; margin-right: auto;
                                                        position: relative; font-size: 0.95em;">
                                                        <input type="hidden" id="hdnLogisticAllTotalPatient" runat="server" value="" />
                                                        <input type="hidden" id="hdnLogisticAllTotalPayer" runat="server" value="" />
                                                        <asp:ListView ID="lvwLogistic" runat="server">
                                                            <LayoutTemplate>
                                                                <table id="tblView" runat="server" class="grdLogistic grdNormal notAllowSelect" cellspacing="0"
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
                                                                            <%=GetLabel("Jumlah")%>
                                                                        </th>
                                                                        <th colspan="2" style="display: none">
                                                                            <%=GetLabel("Jumlah Satuan Kecil")%>
                                                                        </th>
                                                                        <th rowspan="2" style="width: 55px">
                                                                            <div style="text-align: right; padding-right: 3px">
                                                                                <%=GetLabel("Tariff")%>
                                                                            </div>
                                                                        </th>
                                                                        <th rowspan="2" style="width: 55px">
                                                                            <div style="text-align: right; padding-right: 3px">
                                                                                <%=GetLabel("Diskon")%>
                                                                            </div>
                                                                        </th>
                                                                        <th colspan="3">
                                                                            <%=GetLabel("Total")%>
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
                                                                            <div style="text-align: center; padding-right: 3px">
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
                                                                    <tr id="Tr3" class="trFooter" runat="server">
                                                                        <td colspan="9" align="right" style="padding-right: 3px">
                                                                            <%=GetLabel("TOTAL") %>
                                                                        </td>
                                                                        <td align="right" style="padding-right: 9px" class="tdLogisticTotalPayer" id="tdLogisticTotalPayer"
                                                                            runat="server">
                                                                        </td>
                                                                        <td align="right" style="padding-right: 9px" class="tdLogisticTotalPatient" id="tdLogisticTotalPatient"
                                                                            runat="server">
                                                                        </td>
                                                                        <td align="right" style="padding-right: 9px" class="tdLogisticTotal" id="tdLogisticTotal"
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
                                                                                        <img class="imgLogisticVerified" <%# IsEditable.ToString() == "True" && Eval("IsVerified").ToString() == "True" ? "" : "style='display:none'"%>
                                                                                            title='<%=GetLabel("Verified")%>' src='<%# ResolveUrl("~/Libs/Images/Button/verify.png")%>'
                                                                                            alt="" />
                                                                                        <img class="imgLogisticApprove imgLink" <%# IsEditable.ToString() == "False" || Eval("IsVerified").ToString() == "True" || (Eval("IsVerified").ToString() == "False" && Eval("IsApproved").ToString() == "True") ? "style='display:none'" : ""%>
                                                                                            title='<%=GetLabel("Approve This Item")%>' src='<%# ResolveUrl("~/Libs/Images/Button/unlock.png")%>'
                                                                                            alt="" />
                                                                                        <img class="imgLogisticVoid imgLink" <%# IsEditable.ToString() == "False" || Eval("IsVerified").ToString() == "True" || (Eval("IsVerified").ToString() == "False" && Eval("IsApproved").ToString() == "False") ? "style='display:none'" : ""%>
                                                                                            title='<%=GetLabel("Void This Item")%>' src='<%# ResolveUrl("~/Libs/Images/Button/lock.png")%>'
                                                                                            alt="" />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td style="width: 24px">
                                                                                        <img class="imgLogisticEdit imgLink" <%# IsEditable.ToString() == "False" || Eval("IsApproved").ToString() == "True" || Eval("IsReviewed").ToString() == "True" ? "style='display:none'" : ""%>
                                                                                            title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                            alt="" />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td style="width: 24px">
                                                                                        <img class="imgLogisticDelete imgLink" <%# IsEditable.ToString() == "False" || Eval("IsApproved").ToString() == "True" || Eval("IsReviewed").ToString() == "True" ? "style='display:none'" : ""%>
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
                                                                        <img style="margin-left: 2px" class="imgLogisticSwitch imgLink" title='<%=GetLabel("Switch")%>'
                                                                            src='<%# ResolveUrl("~/Libs/Images/Button/switch.png")%>' alt="" />
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                        </asp:ListView>
                                                        <div class="imgLoadingGrdView" id="Div2">
                                                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                                        </div>
                                                    </asp:Panel>
                                                </dx:PanelContent>
                                            </PanelCollection>
                                        </dxcp:ASPxCallbackPanel>
                                    </td>
                                </tr>
                            </table>
                            <div style="width: 100%; text-align: center">
                                <span class="lblLink" id="lblLogisticAddData" style="margin-right: 200px;">
                                    <%= GetLabel("Tambah Data")%></span>
                            </div>
                        </div>
                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                            <colgroup>
                                <col style="width: 15%" />
                                <col style="width: 35%" />
                                <col style="width: 15%" />
                                <col style="width: 35%" />
                            </colgroup>
                            <tr>
                                <td>
                                    <div class="lblComponent" style="text-align: left; padding-left: 5px;">
                                        <%=GetLabel("Total Instansi") %>
                                        :
                                    </div>
                                </td>
                                <td style="text-align: right; padding-right: 10px;">
                                    Rp.
                                    <asp:TextBox ID="txtTotalPayerAll" ReadOnly="true" CssClass="txtCurrency" runat="server"
                                        Width="200px" />
                                </td>
                                <td>
                                    <div class="lblComponent" style="text-align: left; padding-left: 5px;">
                                        <%=GetLabel("Total Pasien") %>
                                        :
                                    </div>
                                </td>
                                <td style="text-align: right; padding-right: 10px;">
                                    Rp.
                                    <asp:TextBox ID="txtTotalPatientAll" ReadOnly="true" CssClass="txtCurrency" runat="server"
                                        Width="200px" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </fieldset>
            </dx:PanelContent>
        </PanelCollection>
    </dxcp:ASPxCallbackPanel>
</div>
<!-- Pop Up Unit Tariff-->
<dxpc:ASPxPopupControl ID="pcUnitTariff" runat="server" ClientInstanceName="pcUnitTariff"
    CloseAction="CloseButton" Height="200px" HeaderText="Harga Satuan" Width="200px"
    Modal="True" PopupAction="None" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
    <ContentCollection>
        <dxpc:PopupControlContentControl runat="server" ID="pccc1">
            <dx:ASPxPanel ID="pnlTariffComp" runat="server" Width="100%">
                <PanelCollection>
                    <dx:PanelContent ID="PanelContent3" runat="server">
                        <fieldset id="fsUnitTariff" style="margin: 0">
                            <div style="text-align: left; width: 100%;">
                                <table>
                                    <colgroup>
                                        <col style="width: 200px" />
                                    </colgroup>
                                    <tr>
                                        <td valign="top">
                                            <table>
                                                <colgroup>
                                                    <col style="width: 70px" />
                                                    <col style="width: 120px" />
                                                </colgroup>
                                                <tr>
                                                    <td class="tdLabel">
                                                        <label class="lblNormal">
                                                            <%=GetLabel("Total")%></label>
                                                    </td>
                                                    <td>
                                                        <span>
                                                            <asp:TextBox ID="txtServiceTotalTariff" runat="server" Width="100%" CssClass="txtCurrency txtServiceTotal" /></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdLabel">
                                                        <label class="lblNormal">
                                                            <%=GetLabel("Cost")%></label>
                                                    </td>
                                                    <td>
                                                        <span>
                                                            <asp:TextBox ID="txtServiceCostAmount" runat="server" Width="100%" CssClass="txtCurrency txtServiceCostAmount"
                                                                ReadOnly="true" /></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <hr />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdLabel">
                                                        <label class="lblNormal">
                                                            <%=GetTariffComponent1Text()%></label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtServiceTariffComp1" CssClass="txtCurrency txtServiceTariffComp1 txtTariffComp"
                                                            Width="100%" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdLabel">
                                                        <label class="lblNormal">
                                                            <%=GetTariffComponent2Text()%></label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtServiceTariffComp2" CssClass="txtCurrency txtServiceTariffComp2 txtTariffComp"
                                                            Width="100%" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdLabel">
                                                        <label class="lblNormal">
                                                            <%=GetTariffComponent3Text()%></label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtServiceTariffComp3" CssClass="txtCurrency txtServiceTariffComp3 txtTariffComp"
                                                            Width="100%" runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <table style="margin-left: auto; margin-right: auto; margin-top: 10px;">
                                    <tr>
                                        <td>
                                            <input type="button" id="btnUnitTariffOK" value='<%= GetLabel("OK")%>' onclick="pcUnitTariff.Hide();" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </fieldset>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxPanel>
        </dxpc:PopupControlContentControl>
    </ContentCollection>
</dxpc:ASPxPopupControl>
<!-- Pop Up Discount-->
<dxpc:ASPxPopupControl ID="pcDiscount" runat="server" ClientInstanceName="pcDiscount"
    CloseAction="CloseButton" Height="200px" HeaderText="Diskon" Width="450px" Modal="True"
    PopupAction="None" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
    <ContentCollection>
        <dxpc:PopupControlContentControl runat="server" ID="pccc2">
            <dx:ASPxPanel ID="pnlDiscount" runat="server" Width="100%">
                <PanelCollection>
                    <dx:PanelContent ID="PanelContent4" runat="server">
                        <fieldset id="fsDiscount" style="margin: 0">
                            <div style="text-align: left; width: 100%;">
                                <table>
                                    <colgroup>
                                        <col style="width: 450px" />
                                    </colgroup>
                                    <tr>
                                        <td valign="top">
                                            <table>
                                                <colgroup>
                                                    <col style="width: 70px" />
                                                    <col style="width: 120px" />
                                                    <col style="width: 120px" />
                                                    <col style="width: 120px" />
                                                </colgroup>
                                                <tr>
                                                    <td class="tdLabel">
                                                        <label class="lblNormal">
                                                            <%=GetLabel("Total")%></label>
                                                    </td>
                                                    <td colspan="2">
                                                        <span>
                                                            <asp:TextBox ID="txtServiceDiscTotal" runat="server" Width="100%" CssClass="txtCurrency txtServiceDiscTotal" /></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4">
                                                        <hr />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td align="center">
                                                        <div class="lblComponent">
                                                            <%=GetLabel("Base Tariff")%></div>
                                                    </td>
                                                    <td align="center">
                                                        <div class="lblComponent">
                                                            <%=GetLabel("Discount")%></div>
                                                    </td>
                                                    <td align="center">
                                                        <div class="lblComponent">
                                                            <%=GetLabel("Tariff")%></div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdLabel">
                                                        <label class="lblNormal">
                                                            <%=GetTariffComponent1Text()%></label>
                                                    </td>
                                                    <td>
                                                        <input id="txtServiceDiscTariffComp1" class="txtCurrency txtUnitTariffPrev" readonly="readonly"
                                                            style="width: 100%" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtServiceDiscComp1" CssClass="txtCurrency txtServiceDiscComp1 txtDiscComp"
                                                            Width="100%" runat="server" />
                                                    </td>
                                                    <td>
                                                        <input id="txtServiceAfterTariffComp1" class="txtCurrency txtUnitTariffAfter" readonly="readonly"
                                                            style="width: 100%" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdLabel">
                                                        <label class="lblNormal">
                                                            <%=GetTariffComponent2Text()%></label>
                                                    </td>
                                                    <td>
                                                        <input id="txtServiceDiscTariffComp2" class="txtCurrency txtUnitTariffPrev" readonly="readonly"
                                                            style="width: 100%" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtServiceDiscComp2" CssClass="txtCurrency txtServiceDiscComp2 txtDiscComp"
                                                            Width="100%" runat="server" />
                                                    </td>
                                                    <td>
                                                        <input id="txtServiceAfterTariffComp2" class="txtCurrency txtUnitTariffAfter" readonly="readonly"
                                                            style="width: 100%" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdLabel">
                                                        <label class="lblNormal">
                                                            <%=GetTariffComponent3Text()%></label>
                                                    </td>
                                                    <td>
                                                        <input id="txtServiceDiscTariffComp3" class="txtCurrency txtUnitTariffPrev" readonly="readonly"
                                                            style="width: 100%" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtServiceDiscComp3" CssClass="txtCurrency txtServiceDiscComp3 txtDiscComp"
                                                            Width="100%" runat="server" />
                                                    </td>
                                                    <td>
                                                        <input id="txtServiceAfterTariffComp3" class="txtCurrency txtUnitTariffAfter" readonly="readonly"
                                                            style="width: 100%" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdLabel">
                                                        <label class="lblNormal">
                                                            <%=GetLabel("CITO")%></label>
                                                    </td>
                                                    <td>
                                                        <input id="txtServiceCITOAmount" class="txtCurrency txtUnitTariffPrev" readonly="readonly"
                                                            style="width: 100%" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtServiceCITODisc" CssClass="txtCurrency txtServiceCITODisc txtDiscComp"
                                                            Width="100%" runat="server" />
                                                    </td>
                                                    <td>
                                                        <input id="txtServiceAfterCITOAmount" class="txtCurrency txtUnitTariffAfter" readonly="readonly"
                                                            style="width: 100%" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <table style="margin-left: auto; margin-right: auto; margin-top: 10px;">
                                    <tr>
                                        <td>
                                            <input type="button" id="btnDiscountOK" value='<%= GetLabel("OK")%>' onclick="pcDiscount.Hide();" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </fieldset>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxPanel>
        </dxpc:PopupControlContentControl>
    </ContentCollection>
</dxpc:ASPxPopupControl>
