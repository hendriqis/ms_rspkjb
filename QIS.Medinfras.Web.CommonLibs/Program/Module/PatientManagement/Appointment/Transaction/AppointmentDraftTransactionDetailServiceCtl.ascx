<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AppointmentDraftTransactionDetailServiceCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.AppointmentDraftTransactionDetailServiceCtl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dxpc" %>

<script type="text/javascript" id="dxss_ptservicectl">
    //#region Service
    function onLoadService() {
        if ($('#<%=hdnIsEditable.ClientID %>').val() == '1') {
            $('#lblServiceAddData').show();
            $('#lblServiceQuickPick').show();
        }
        else {
            $('#lblServiceAddData').hide();
            $('#lblServiceQuickPick').hide();
        }

        $('#btnServiceSave').click(function (evt) {
            if (IsValid(evt, 'fsTrxService', 'mpTrxService')) {
                if ($('#<%=hdnServiceItemID.ClientID %>').val() == $('#<%=hdnPrescriptionReturnItem.ClientID %>').val() && parseFloat($('#<%=txtServiceQty.ClientID %>').val()) >= 0)
                    showToast('Warning', 'Jumlah Pelayanan Retur Resep Harus Minus');
                else
                    cbpService.PerformCallback('save');
            }
            return false;
        });
        $('#btnServiceCancel').click(function () {
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

        $('#lblServiceQuickPick').live('click', function () {
            if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
                showLoadingPanel();
                var serviceUnitID = $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val();
                var labServiceUnitID = $('#<%=hdnLaboratoryServiceUnitID.ClientID %>').val();
                var radiologyServiceUnitID = $('#<%=hdnImagingServiceUnitID.ClientID %>').val();
                var url = '';
                var width = 0;
                if (serviceUnitID == labServiceUnitID || serviceUnitID == radiologyServiceUnitID) {
                    url = ResolveUrl('~/Libs/Program/Module/PatientManagement/Transaction/ChargesLabQuickPicksCtl.ascx');
                    width = 1150;
                }
                else {
                    url = ResolveUrl('~/Libs/Program/Module/PatientManagement/Transaction/ServiceQuickPicksCtl.ascx');
                    width = 1000;
                }
                var transactionID = getTransactionHdID();
                var serviceUnitID = getHealthcareServiceUnitID();
                var registrationID = getRegistrationID();
                var visitID = getVisitID();
                var departmentID = getDepartmentID();
                var isAccompany = "0";
                if (typeof isAccompanyChargesPage == 'function') {
                    if (isAccompanyChargesPage()) {
                        isAccompany = "1";
                    }
                }
                var id = transactionID + '|' + serviceUnitID + '|' + visitID + '|' + registrationID + '|' + departmentID + '|' + isAccompany;
                openUserControlPopup(url, id, 'Quick Picks', width, 600);
            }
        });

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

        $('#<%=chkServiceIsUnbilledItem.ClientID %>').change(function () {
            if ($(this).is(':checked')) {
                $('#<%=chkServiceIsVariable.ClientID %>').prop('checked', false);
                $('#<%=chkServiceIsVariable.ClientID %>').change();
                $('#<%=txtServiceUnitTariff.ClientID %>').val('0').trigger('changeValue');
                $('#<%=txtServiceUnitTariff.ClientID %>').change();
            }
            else {
                $('#<%=txtServiceUnitTariff.ClientID %>').val($('#<%=hdnServicePrice.ClientID %>').val()).trigger('changeValue');
                $('#<%=txtServiceUnitTariff.ClientID %>').change();
            }
        });

        $('#<%=txtServicePatient.ClientID %>').change(function () {
            var patientTotal = parseInt($(this).val());
            var total = parseInt($('#<%=txtServiceTotal.ClientID %>').attr('hiddenVal'));
            var payerTotal = total - patientTotal;
            $('#<%=txtServicePayer.ClientID %>').val(payerTotal).trigger('changeValue');
        });

        $('#<%=txtServicePayer.ClientID %>').change(function () {
            var payerTotal = parseInt($(this).val());
            var total = parseInt($('#<%=txtServiceTotal.ClientID %>').attr('hiddenVal'));
            var patientTotal = total - payerTotal;
            $('#<%=txtServicePatient.ClientID %>').val(patientTotal).trigger('changeValue');
        });

        $('#<%=chkServiceIsDiscount.ClientID %>').change(function () {
            if ($(this).is(':checked'))
                $('#<%=txtServiceDiscount.ClientID %>').removeAttr('readonly');
            else
                $('#<%=txtServiceDiscount.ClientID %>').attr('readonly', 'readonly');
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

        $('#<%=chkServiceIsComplication.ClientID %>').change(function () {
            if ($(this).is(':checked')) {
                var isComplicationInPercentage = ($('#<%=hdnServiceIsComplicationInPercentage.ClientID %>').val() == '1');
                var complicationAmount = parseFloat($('#<%=hdnServiceBaseComplicationAmount.ClientID %>').val());
                var complication = 0;
                if (isComplicationInPercentage) {
                    var tariff = parseFloat($('#<%=txtServiceTariff.ClientID %>').attr('hiddenVal'));
                    complication = (tariff * complicationAmount) / 100;
                }
                else {
                    var qty = parseFloat($('#<%=txtServiceQty.ClientID %>').val());
                    complication = complicationAmount * qty;
                }
                $('#<%=txtServiceComplication.ClientID %>').val(complication).trigger('changeValue');
            }
            else
                $('#<%=txtServiceComplication.ClientID %>').val('0').trigger('changeValue');
            calculateServiceTotal();
        });

        $('#<%=txtServiceUnitTariff.ClientID %>').change(function () {
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

        var isChangeFromServiceUnitTariff = false;
        var isChangeFromTariffCompTotal = false;
        var isChangeFromServiceDiscount = false;
        var isChangeFromDiscCompTotal = false;

        $('#<%=txtServiceDiscount.ClientID %>').change(function () {
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

        $('#<%=txtServiceQty.ClientID %>').change(function () {
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

        $('.txtTariffComp').change(function () {
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

        $('#btnEditUnitTariff').click(function () {
            $('#<%=txtServiceTotalTariff.ClientID %>').val($('#<%=txtServiceUnitTariff.ClientID %>').attr('hiddenVal')).trigger('changeValue');
            pcUnitTariff.Show();
        });
        //#endregion

        //#region Discount Component
        $('#<%=txtServiceDiscTotal.ClientID %>').change(function () {
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

        $('.txtDiscComp').change(function () {
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

        $('.btnEditDiscount').click(function () {
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
    }

    function onCbpServiceEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else {
                var transactionID = s.cpTransactionID;
                onAfterSaveRecordDtSuccess(transactionID);
                $('#containerEntryService').hide();
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
        calculateAllTotal();
        hideLoadingPanel();
    }

    $('.imgServiceSwitch.imgLink').die('click');
    $('.imgServiceSwitch.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        var obj = rowToObject($row);
        cbpService.PerformCallback('switch|' + obj.ID);
    });

    //#region Entry Service
    $('.imgServiceDelete.imgLink').die('click');
    $('.imgServiceDelete.imgLink').live('click', function () {
        $row = $(this).closest('tr').parent().closest('tr');
        showDeleteConfirmation(function (data) {
            var obj = rowToObject($row);
            var param = 'delete|' + obj.ID + ';' + data.GCDeleteReason + ';' + data.Reason;
            cbpService.PerformCallback(param);
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

        var registrationID = getRegistrationID();
        var visitID = getVisitID();
        var classID = obj.ChargeClassID;
        var trxDate = getTrxDate();
        Methods.getItemTariff(registrationID, visitID, classID, obj.ItemID, trxDate, function (result) {
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
    $('#lblServiceAddData').live('click', function () {
        if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
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

    $('.imgServiceParamedic.imgLink').live('click', function () {
        $row = $(this).closest('tr').parent().closest('tr');
        var entity = rowToObject($row);
        var id = entity.ID;
        var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Transaction/ParamedicTeamCtl.ascx");
        openUserControlPopup(url, id, 'Tim Medis', 600, 500);
    });

    $('.imgIsPackageItem.imgLink').live('click', function () {
        $row = $(this).closest('tr').parent().closest('tr');
        var entity = rowToObject($row);
        var id = entity.ItemID;
        var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Transaction/ViewItemPackageDetail.ascx");
        openUserControlPopup(url, id, 'Item Paket', 600, 500);
    });
    

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

    function onTxtServiceItemCodeChanged(value) {
        var today = new Date();
        var date = today.getDate() + '-' + (today.getMonth() + 1) + '-' + today.getFullYear();
        var time = today.getHours() + ":" + today.getMinutes();

        var hdnTransactionDate = $('#<%=hdnTransactionDate.ClientID %>').val() == "" ? date : $('#<%=hdnTransactionDate.ClientID %>').val();
        var hdnTransactionTime = $('#<%=hdnTransactionTime.ClientID %>').val() == "" ? time : $('#<%=hdnTransactionTime.ClientID %>').val();
        var hdnChargesHealthcareServiceUnitID = $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val();

        var paramedicID = $('#<%=hdnServicePhysicianID.ClientID %>').val();
        if (paramedicID == '')
            paramedicID = '0';
        Methods.getItemRevenueSharing(value, paramedicID, cboServiceChargeClassID.GetValue(), '<%=GetMainParamedicRole() %>', 0, hdnChargesHealthcareServiceUnitID, hdnTransactionDate, hdnTransactionTime, function (result) {
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

    function getServiceTariff() {
        var itemID = $('#<%=hdnServiceItemID.ClientID %>').val();
        if (itemID != '') {
            showLoadingPanel();
            var appointmentID = getAppointmentID();
            var classID = cboServiceChargeClassID.GetValue();
            var trxDate = getTrxDate();
            Methods.getItemTariff(classID, itemID, trxDate, function (result) {
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

                    var hdnTransactionDate = $('#<%=hdnTransactionDate.ClientID %>').val() == "" ? date : $('#<%=hdnTransactionDate.ClientID %>').val();
                    var hdnTransactionTime = $('#<%=hdnTransactionTime.ClientID %>').val() == "" ? time : $('#<%=hdnTransactionTime.ClientID %>').val();
                    var hdnChargesHealthcareServiceUnitID = $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val();

                    Methods.getItemRevenueSharing(itemCode, $('#<%=hdnServicePhysicianID.ClientID %>').val(), cboServiceChargeClassID.GetValue(), '<%=GetMainParamedicRole() %>', 0, hdnChargesHealthcareServiceUnitID, hdnTransactionDate, hdnTransactionTime, function (result) {
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
    function setServiceItemFilterExpression(healthcareServiceUnitID, transactionID) {
        //if (typeof transactionID != 'undefined') {
            //var filterExpression = $('#<%=hdnServiceItemFilterExpression.ClientID %>').val().replace('{HealthcareServiceUnitID}', healthcareServiceUnitID);
            //$('#<%=hdnServiceItemFilterExpression.ClientID %>').val(filterExpression);
        //}
        //else {
            //$('#<%=hdnServiceItemFilterExpression.ClientID %>').val("HealthcareServiceUnitID = " + healthcareServiceUnitID + " AND IsDeleted = 0");
        //}
        $('#<%=hdnServiceItemFilterExpression.ClientID %>').val("HealthcareServiceUnitID = " + healthcareServiceUnitID + " AND IsDeleted = 0");
    }

    function onCboServiceChargeClassIDValueChanged() {
        var today = new Date();
        var date = today.getDate() + '-' + (today.getMonth() + 1) + '-' + today.getFullYear();
        var time = today.getHours() + ":" + today.getMinutes();

        var hdnTransactionDate = $('#<%=hdnTransactionDate.ClientID %>').val() == "" ? date : $('#<%=hdnTransactionDate.ClientID %>').val();
        var hdnTransactionTime = $('#<%=hdnTransactionTime.ClientID %>').val() == "" ? time : $('#<%=hdnTransactionTime.ClientID %>').val();
        var hdnChargesHealthcareServiceUnitID = $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val();

        var itemCode = $('#<%=txtServiceItemCode.ClientID %>').val();
        if (itemCode != '') {
            Methods.getItemRevenueSharing(itemCode, $('#<%=hdnServicePhysicianID.ClientID %>').val(), cboServiceChargeClassID.GetValue(), '<%=GetMainParamedicRole() %>', 0, hdnChargesHealthcareServiceUnitID, hdnTransactionDate, hdnTransactionTime, function (result) {
                if (result != null)
                    $('#<%=hdnServiceRevenueSharingID.ClientID %>').val(result.RevenueSharingID);
                else
                    $('#<%=hdnServiceRevenueSharingID.ClientID %>').val('');
                getServiceTariff();
            });
        }
    }

    function calculateServiceTariffTotal() {
        var tariff = parseFloat($('#<%=txtServiceUnitTariff.ClientID %>').attr('hiddenVal'));
        var qty = parseFloat($('#<%=txtServiceQty.ClientID %>').val());
        $('#<%=txtServiceTariff.ClientID %>').val(tariff * qty).trigger('changeValue');
    }

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
    //#endregion

    function getServiceTotalPatient() {
        return parseFloat($('#<%=hdnServiceAllTotalPatient.ClientID %>').val());
    }
    function getServiceTotalPayer() {
        return parseFloat($('#<%=hdnServiceAllTotalPayer.ClientID %>').val());
    }
</script> 
    
<input type="hidden" id="hdnLabHealthcareServiceUnitID" runat="server" value="" />
<input type="hidden" id="hdnImagingServiceUnitID" runat="server" value="" />
<input type="hidden" id="hdnLaboratoryServiceUnitID" runat="server" value="" />
<input type="hidden" id="hdnServiceTransactionDtID" runat="server" value="" />
<input type="hidden" id="hdnServiceDtTotal" runat="server" value="" />
<input type="hidden" id="hdnRegistrationID" runat="server" value="" />
<input type="hidden" id="hdnVisitID" runat="server" value="" />
<input type="hidden" id="hdnTariffComp1Text" runat="server" value="" />
<input type="hidden" id="hdnTariffComp2Text" runat="server" value="" />
<input type="hidden" id="hdnTariffComp3Text" runat="server" value="" />
<input type="hidden" id="hdnPrescriptionReturnItem" runat="server" value="" />
<input type="hidden" id="hdnIsEditable" runat="server" value="" />
<input type="hidden" id="hdnBPJSRegistration" runat="server" value="" />
<input type="hidden" id="hdnIsOnlyBPJS" runat="server" value="" />

<input type="hidden" id="hdnTempTotalPatient" runat="server" value="" />
<input type="hidden" id="hdnTempTotalPayer" runat="server" value="" />
<div id="containerEntryService" style="margin-top:4px;display:none;">
    <div class="pageTitle"><%=GetLabel("Tambah Atau Ubah Data")%></div>
    <fieldset id="fsTrxService" style="margin:0"> 
        <table class="tblEntryDetail" style="width:100%">
            <colgroup>
                <col style="width:40%"/>
                <col style="width:33%"/>
                <col />
            </colgroup>
            <tr>
                <td valign="top">
                    <table>
                        <colgroup>
                            <col style="width:130px"/>
                        </colgroup>
                        <tr>
                            <td class="tdLabel"><label class="lblLink lblMandatory" id="lblServiceItem"><%=GetLabel("Pelayanan")%></label></td>
                            <td>
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
                                <input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />
                                <input type="hidden" value="" id="hdnTransactionDate" runat="server" />
                                <input type="hidden" value="" id="hdnTransactionTime" runat="server" />
                                <input type="hidden" value="" id="hdnServiceVisitID" runat="server" />
                                <input type="hidden" value="" id="hdnServiceUnitName" runat="server" />

                                <table style="width:100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width:30%"/>
                                        <col style="width:3px"/>
                                        <col/>
                                    </colgroup>
                                    <tr>
                                        <td><asp:TextBox ID="txtServiceItemCode" Width="100%" runat="server" /></td>
                                        <td>&nbsp;</td>
                                        <td><asp:TextBox ID="txtServiceItemName" ReadOnly="true" Width="100%" runat="server" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblLink lblMandatory" id="lblPhysician"><%=GetLabel("Dokter/Paramedis")%></label></td>
                            <td>
                                <input type="hidden" value="" id="hdnServicePhysicianID" runat="server" />
                                <table style="width:100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width:30%"/>
                                        <col style="width:3px"/>
                                        <col/>
                                    </colgroup>
                                    <tr>
                                        <td><asp:TextBox ID="txtServicePhysicianCode" Width="100%" runat="server" /></td>
                                        <td>&nbsp;</td>
                                        <td><asp:TextBox ID="txtServicePhysicianName" ReadOnly="true" Width="100%" runat="server" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblDisabled" id="lblTestPartner"><%=GetLabel("Test Partner")%></label></td>
                            <td>
                                <input type="hidden" value="" id="hdnTestPartnerID" runat="server" />
                                <table style="width:100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width:30%"/>
                                        <col style="width:3px"/>
                                        <col/>
                                    </colgroup>
                                    <tr>
                                        <td><asp:TextBox ID="txtTestPartnerCode" Width="100%" runat="server" ReadOnly="true" /></td>
                                        <td>&nbsp;</td>
                                        <td><asp:TextBox ID="txtTestPartnerName" ReadOnly="true" Width="100%" runat="server" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label></label></td>
                            <td>
                                <input type="hidden" id="hdnDefaultTariffComp" runat="server" value="1" />
                                <table style="width:100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width:20px"/>
                                        <col style="width:100px"/>
                                        <col style="width:20px"/>
                                    </colgroup>
                                    <tr>
                                        <td><asp:CheckBox ID="chkServiceIsVariable" runat="server" /></td>
                                        <td class="tdLabel"><label><%=GetLabel("Variable")%></label></td>
                                        <td><asp:CheckBox ID="chkServiceIsUnbilledItem" runat="server" /></td>
                                        <td class="tdLabel"><label><%=GetLabel("Tidak Ditagihkan")%></label></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label><%=GetLabel("Kelas Tagihan")%></label></td>
                            <td>
                                <dxe:ASPxComboBox ID="cboServiceChargeClassID" ClientInstanceName="cboServiceChargeClassID" Width="100%" runat="server">
                                    <ClientSideEvents ValueChanged="function(s,e){ onCboServiceChargeClassIDValueChanged(); }" />
                                </dxe:ASPxComboBox>
                            </td> 
                        </tr> 
                        <tr>
                            <td class="tdLabel"><label><%=GetLabel("Harga Satuan")%></label></td>
                            <td><asp:TextBox ID="txtServiceUnitTariff" Width="100px" CssClass="txtCurrency" runat="server" />
                            <input type="button" id="btnEditUnitTariff" title='<%=GetLabel("Unit Tariff Component") %>' value="..." style="width:10%"  /></td>
                        </tr>                                        
                        <tr>
                            <td class="tdLabel"><label><%=GetLabel("Jumlah")%></label></td>
                            <td><asp:TextBox ID="txtServiceQty" Width="100px" CssClass="number" runat="server" /></td>
                        </tr>
                    </table>
                </td>
                <td valign="top">
                    <table>    
                        <colgroup>
                            <col style="width:100px"/>
                            <col style="width:20px"/>
                            <col />
                        </colgroup>   
                        
                        <tr>
                            <td class="tdLabel"><label><%=GetLabel("Harga")%></label></td>
                            <td>&nbsp;</td>
                            <td><asp:TextBox ReadOnly="true" ID="txtServiceTariff" Width="200px" CssClass="txtCurrency" runat="server" /></td>
                        </tr>   
                        <tr>
                            <td class="tdLabel"><label><%=GetLabel("CITO")%></label></td>
                            <td><asp:CheckBox ID="chkServiceIsCITO" runat="server" /></td>
                            <td><asp:TextBox ID="txtServiceCITO" ReadOnly="true" Width="200px" CssClass="txtCurrency" runat="server" /></td>
                        </tr>
                        <tr style="display:none">
                            <td class="tdLabel"><label><%=GetLabel("Penyulit")%></label></td>
                            <td><asp:CheckBox ID="chkServiceIsComplication" runat="server" /></td>
                            <td><asp:TextBox ID="txtServiceComplication" ReadOnly="true" Width="200px" CssClass="txtCurrency" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label><%=GetLabel("Diskon")%></label></td>
                            <td><asp:CheckBox ID="chkServiceIsDiscount" runat="server" /></td>
                            <td><asp:TextBox ID="txtServiceDiscount" Width="200px" CssClass="txtCurrency" runat="server" />
                            <input type="button" class="btnEditDiscount" title='<%=GetLabel("Discount Component") %>' value="..." style="width:10%"  /></td>
                        </tr>
                    </table>
                </td>
                <td valign="top">
                    <table>    
                        <colgroup>
                            <col style="width:100px"/>
                        </colgroup>    
                        <tr>
                            <td class="tdLabel"><label><%=GetLabel("Pasien")%></label></td>
                            <td><asp:TextBox ID="txtServicePatient" ReadOnly="true" Width="200px" CssClass="txtCurrency" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label><%=GetLabel("Instansi")%></label></td>
                            <td><asp:TextBox ID="txtServicePayer" ReadOnly="true" Width="200px" CssClass="txtCurrency" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label><%=GetLabel("Total")%></label></td>
                            <td><asp:TextBox ID="txtServiceTotal" ReadOnly="true" Width="200px" CssClass="txtCurrency" runat="server" /></td>
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
                <col style="width:100px"/>
                <col />
            </colgroup>
        </table>
    </fieldset>
</div>
<dxcp:ASPxCallbackPanel ID="cbpService" runat="server" Width="100%" ClientInstanceName="cbpService"
    ShowLoadingPanel="false" OnCallback="cbpService_Callback">
    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
        EndCallback="function(s,e) { onCbpServiceEndCallback(s); }" />
    <PanelCollection>
        <dx:PanelContent ID="PanelContent1" runat="server">
            <asp:Panel runat="server" ID="pnlService" Style="width: 100%; margin-left: auto; margin-right: auto; position: relative;font-size:0.95em;">
                <input type="hidden" id="hdnServiceAllTotalPatient" runat="server" value="" />
                <input type="hidden" id="hdnServiceAllTotalPayer" runat="server" value="" />
                <input type="hidden" value='<%#: Eval("ID") %>' bindingfield="ID" runat="server" id="hdnChargesDt" />
                <asp:ListView ID="lvwService" runat="server">
                    <LayoutTemplate>                                
                        <table id="tblView" runat="server" class="grdService grdNormal notAllowSelect" cellspacing="0" rules="all" >
                            <tr>  
                                <th style="width:80px" rowspan="2"></th>
<%--                                <th style="width:80px" rowspan="2">
                                    <div style="text-align:left;padding-left:3px">
                                        <%=GetLabel("Kode")%>
                                    </div>
                                </th>
--%>                                <th rowspan="2">
                                    <div style="text-align:left;padding-left:3px">
                                        <%=GetLabel("Item")%>
                                    </div>
                                </th>
                                <th rowspan="2" style="width:70px">
                                    <div style="text-align:left;padding-left:3px">
                                        <%=GetLabel("Kelas Tagihan")%>
                                    </div>
                                </th>
                                <th rowspan="2" style="width:80px">
                                    <div style="text-align:right;padding-right:3px">
                                        <%=GetLabel("Harga Satuan")%>
                                    </div>
                                </th>
                                <th rowspan="2" style="width:50px">
                                    <div style="text-align:right;padding-right:3px">
                                        <%=GetLabel("Jumlah")%>
                                    </div>
                                </th>
                                <th colspan="3"><%=GetLabel("HARGA")%></th>
                                <th colspan="3"><%=GetLabel("TOTAL")%></th>
                                <th rowspan="2" style="width:90px">
                                    <div style="text-align:right;padding-right:3px">
                                        <%=GetLabel("Petugas")%>
                                    </div>
                                </th>    
                                <th rowspan="2">&nbsp;</th>                            
                            </tr>
                            <tr>
                                <th style="width:80px">
                                    <div style="text-align:right;padding-right:3px">
                                        <%=GetLabel("Harga")%>
                                    </div>
                                </th>
                                <th style="width:90px">
                                    <div style="text-align:right;padding-right:3px">
                                        <%=GetLabel("CITO")%>
                                    </div>
                                </th>
                                <th style="width:80px;display:none">
                                    <div style="text-align:right;padding-right:3px">
                                        <%=GetLabel("Penyulit")%>
                                    </div>
                                </th>
                                <th style="width:80px">
                                    <div style="text-align:right;padding-right:3px">
                                        <%=GetLabel("Diskon")%>
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
                            <tr id="Tr1" class="trFooter" runat="server">
                                <td colspan="8" align="right" style="padding-right:3px"><%=GetLabel("TOTAL") %></td>
                                <td align="right" style="padding-right:9px" id="tdServiceTotalPayer" class="tdServiceTotalPayer" runat="server"></td>
                                <td align="right" style="padding-right:9px" id="tdServiceTotalPatient" class="tdServiceTotalPatient" runat="server"></td>
                                <td align="right" style="padding-right:9px" id="tdServiceTotal" class="tdServiceTotal" runat="server"></td>
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
                                            <td><img class="imgServiceParamedic imgLink" <%# IsEditable.ToString() == "True" && IsShowParamedicTeam.ToString() == "True" ?  "" : "style='display:none'" %> title='<%=GetLabel("Tim Dokter")%>' src='<%# ResolveUrl("~/Libs/Images/Button/paramedic_team.png")%>' alt="" style="margin-right:2px" /></td>
                                            <td><img class="imgServiceHasil" <%# Eval("IsHasTestResult").ToString() == "True" ?  "" : "style='display:none'" %> title='<%=GetLabel("Sudah Ada Hasil")%>' src='<%# ResolveUrl("~/Libs/Images/Button/verify.png")%>' alt="" style="margin-right:2px" /></td>
                                            <td><img class="imgServiceEdit <%# IsEditable.ToString() == "False" || Eval("IsVerified").ToString() == "True" || Eval("IsReviewed").ToString() == "True" ? "imgDisabled" : "imgLink"%>" title='<%=GetLabel("Edit")%>' src='<%# IsEditable.ToString() == "False" || Eval("IsVerified").ToString() == "True" || Eval("IsReviewed").ToString() == "True" ? ResolveUrl("~/Libs/Images/Button/edit_disabled.png") : ResolveUrl("~/Libs/Images/Button/edit.png")%>' alt="" style="margin-right:2px" /></td>
                                            <td><img class="imgServiceDelete <%# IsEditable.ToString() == "False" || Eval("IsHasTestResult").ToString() == "True" || Eval("IsReviewed").ToString() == "True" || Eval("IsVerified").ToString() == "True" ? "imgDisabled" : "imgLink"%>" title='<%=GetLabel("Delete")%>' src='<%# IsEditable.ToString() == "False" || Eval("IsHasTestResult").ToString() == "True" || Eval("IsReviewed").ToString() == "True" || Eval("IsVerified").ToString() == "True" ? ResolveUrl("~/Libs/Images/Button/delete_disabled.png") : ResolveUrl("~/Libs/Images/Button/delete.png")%>' alt="" style="margin-right:2px" /></td>
                                            <td><img class="imgServiceVerified" <%# Eval("IsVerified").ToString() == "True" ?  "" : "style='display:none'" %> title='<%=GetLabel("Verified")%>' src='<%# ResolveUrl("~/Libs/Images/Button/verify.png")%>' alt="" /></td>
                                            <td><img class="imgIsPackageItem imgLink" <%# Eval("IsPackageItem").ToString() == "True" ?  "" : "style='display:none'" %> title='<%=GetLabel("Lihat Paket")%>' src='<%# ResolveUrl("~/Libs/Images/Button/verify.png")%>' alt="" /></td>
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
                                <div style="padding:3px">
                                    <div><%#: Eval("ItemName1")%></div>
                                    <div><span style="font-style:italic"><%#: Eval("ItemCode") %></span>, <span style="color:Blue"> <%#: Eval("ParamedicName")%></span>
                                    </div> 
                                    <div <%# Eval("BusinessPartnerName").ToString() != "" ?  "" : "style='display:none'" %>><%#: Eval("BusinessPartnerName")%></div>
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
                                    <div><%#: Eval("ChargedQuantity")%></div>                                                   
                                </div>
                            </td>
                            <td>
                                <div style="padding:3px;text-align:right;">
                                    <div><%#: Eval("GrossLineAmount", "{0:N}")%></div>                                                   
                                </div>
                            </td>
                            <td>
                                <div style="padding:3px;text-align:right;">
                                    <div><%#: Eval("CITOAmount", "{0:N}")%></div>                                                   
                                </div>
                            </td>
                            <td style="display:none">
                                <div style="padding:3px;text-align:right;">
                                    <div><%#: Eval("ComplicationAmount", "{0:N}")%></div>                                                   
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
                                    <div><%#: Eval("CreatedByFullName")%></div>
                                    <div><%#: Eval("CreatedDateInString")%></div>  
                                </div>
                            </td>
                            <td <%# IsShowSwitchIcon.ToString() == "True" && IsEditable.ToString() == "True" ?  "" : "style='display:none'" %> valign="middle" >
                                <img style="margin-left: 2px" class="imgServiceSwitch imgLink" title='<%=GetLabel("Switch")%>' src='<%# ResolveUrl("~/Libs/Images/Button/switch.png")%>' alt="" />
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
                <div class="imgLoadingGrdView" id="containerImgLoadingService">
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
                <div style="width:100%;text-align:center">
                    <span class="lblLink" id="lblServiceAddData" style="margin-right: 200px;" > <%= GetLabel("Tambah Data")%></span>
                    <span class="lblLink" id="lblServiceQuickPick" > <%= GetLabel("Quick Picks")%></span>
                </div>
            </asp:Panel>
        </dx:PanelContent>
    </PanelCollection>
</dxcp:ASPxCallbackPanel>

<!-- Pop Up Unit Tariff-->
<dxpc:ASPxPopupControl ID="pcUnitTariff" runat="server" ClientInstanceName="pcUnitTariff" CloseAction="CloseButton"
    Height="200px" HeaderText="Harga Satuan" Width="200px" Modal="True" PopupAction="None"
    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
    <ContentCollection>
        <dxpc:PopupControlContentControl runat="server" ID="pccc1">
            <dx:ASPxPanel ID="pnlTariffComp" runat="server" Width="100%">
                <PanelCollection>
                    <dx:PanelContent ID="PanelContent2" runat="server">
                        <fieldset id="fsUnitTariff" style="margin:0"> 
                            <div style="text-align: left; width: 100%;">
                                <table>
                                    <colgroup>
                                        <col style="width: 200px"/>
                                    </colgroup>
                                    <tr>
                                        <td valign="top">
                                            <table>
                                                <colgroup>
                                                    <col style="width:70px"/>
                                                    <col style="width:120px"/>
                                                </colgroup>
                                                <tr>
                                                    <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Total")%></label></td>
                                                    <td><span><asp:TextBox ID="txtServiceTotalTariff" runat="server" Width="100%" CssClass="txtCurrency txtServiceTotal"/></span></td>
                                                </tr>
                                                <tr>
                                                    <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Cost")%></label></td>
                                                    <td><span><asp:TextBox ID="txtServiceCostAmount" runat="server" Width="100%" CssClass="txtCurrency txtServiceCostAmount" ReadOnly="true"/></span></td>
                                                </tr>
                                                <tr><td colspan="2"><hr /></td></tr>  
                                                <tr>
                                                    <td class="tdLabel"><label class="lblNormal"><%=GetTariffComponent1Text()%></label></td>
                                                    <td><asp:TextBox ID="txtServiceTariffComp1" CssClass="txtCurrency txtServiceTariffComp1 txtTariffComp" Width="100%" runat="server"/></td>
                                                </tr>                          
                                                <tr>
                                                    <td class="tdLabel"><label class="lblNormal"><%=GetTariffComponent2Text()%></label></td>
                                                    <td><asp:TextBox ID="txtServiceTariffComp2" CssClass="txtCurrency txtServiceTariffComp2 txtTariffComp" Width="100%" runat="server"/></td>
                                                </tr>                           
                                                <tr>    
                                                    <td class="tdLabel"><label class="lblNormal"><%=GetTariffComponent3Text()%></label></td>
                                                    <td><asp:TextBox ID="txtServiceTariffComp3" CssClass="txtCurrency txtServiceTariffComp3 txtTariffComp" Width="100%" runat="server"/></td>
                                                </tr>                             
                                            </table>  
                                        </td>
                                    </tr>
                                </table>
                                <table style="margin-left: auto; margin-right: auto; margin-top: 10px;">
                                    <tr>
                                        <td>
                                            <input type="button" id="btnUnitTariffOK" value='<%= GetLabel("OK")%>' onclick="pcUnitTariff.Hide();"/>
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
<dxpc:ASPxPopupControl ID="pcDiscount" runat="server" ClientInstanceName="pcDiscount" CloseAction="CloseButton"
    Height="200px" HeaderText="Diskon" Width="450px" Modal="True" PopupAction="None"
    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
    <ContentCollection>
        <dxpc:PopupControlContentControl runat="server" ID="pccc2">
            <dx:ASPxPanel ID="pnlDiscount" runat="server" Width="100%">
                <PanelCollection>
                    <dx:PanelContent ID="PanelContent3" runat="server">
                        <fieldset id="fsDiscount" style="margin:0"> 
                            <div style="text-align: left; width: 100%;">
                                <table>
                                    <colgroup>
                                        <col style="width: 450px"/>
                                    </colgroup>
                                    <tr>
                                        <td valign="top">
                                            <table>
                                                <colgroup>
                                                    <col style="width:70px"/>
                                                    <col style="width:120px"/>
                                                    <col style="width:120px"/>
                                                    <col style="width:120px"/>
                                                </colgroup>
                                                <tr>
                                                    <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Total")%></label></td>
                                                    <td colspan="2"><span><asp:TextBox ID="txtServiceDiscTotal" runat="server" Width="100%" CssClass="txtCurrency txtServiceDiscTotal"/></span></td>
                                                </tr>
                                                <tr><td colspan="4"><hr /></td></tr> 
                                                <tr>
                                                    <td>&nbsp;</td>
                                                    <td align="center"><div class="lblComponent"><%=GetLabel("Base Tariff")%></div></td>
                                                    <td align="center"><div class="lblComponent"><%=GetLabel("Discount")%></div></td>
                                                    <td align="center"><div class="lblComponent"><%=GetLabel("Tariff")%></div></td>
                                                </tr>  
                                                <tr>
                                                    <td class="tdLabel"><label class="lblNormal"><%=GetTariffComponent1Text()%></label></td>
                                                    <td><input id="txtServiceDiscTariffComp1" class="txtCurrency txtUnitTariffPrev" readonly="readonly" style="width:100%" /></td>
                                                    <td><asp:TextBox ID="txtServiceDiscComp1" CssClass="txtCurrency txtServiceDiscComp1 txtDiscComp" Width="100%" runat="server"/></td>
                                                    <td><input id="txtServiceAfterTariffComp1" class="txtCurrency txtUnitTariffAfter" readonly="readonly" style="width:100%" /></td>
                                                </tr>                          
                                                <tr>
                                                    <td class="tdLabel"><label class="lblNormal"><%=GetTariffComponent2Text()%></label></td>
                                                    <td><input id="txtServiceDiscTariffComp2" class="txtCurrency txtUnitTariffPrev" readonly="readonly" style="width:100%" /></td>
                                                    <td><asp:TextBox ID="txtServiceDiscComp2" CssClass="txtCurrency txtServiceDiscComp2 txtDiscComp" Width="100%" runat="server"/></td>
                                                    <td><input id="txtServiceAfterTariffComp2" class="txtCurrency txtUnitTariffAfter" readonly="readonly" style="width:100%" /></td>          
                                                </tr>                           
                                                <tr>    
                                                    <td class="tdLabel"><label class="lblNormal"><%=GetTariffComponent3Text()%></label></td>
                                                    <td><input id="txtServiceDiscTariffComp3" class="txtCurrency txtUnitTariffPrev" readonly="readonly" style="width:100%" /></td>    
                                                    <td><asp:TextBox ID="txtServiceDiscComp3" CssClass="txtCurrency txtServiceDiscComp3 txtDiscComp" Width="100%" runat="server"/></td>
                                                    <td><input id="txtServiceAfterTariffComp3" class="txtCurrency txtUnitTariffAfter" readonly="readonly" style="width:100%" /></td>
                                                </tr>
                                                <tr>    
                                                    <td class="tdLabel"><label class="lblNormal"><%=GetLabel("CITO")%></label></td>
                                                    <td><input id="txtServiceCITOAmount" class="txtCurrency txtUnitTariffPrev" readonly="readonly" style="width:100%" /></td>
                                                    <td><asp:TextBox ID="txtServiceCITODisc" CssClass="txtCurrency txtServiceCITODisc txtDiscComp" Width="100%" runat="server"/></td>
                                                    <td><input id="txtServiceAfterCITOAmount" class="txtCurrency txtUnitTariffAfter" readonly="readonly" style="width:100%" /></td>
                                                </tr>                             
                                            </table>  
                                        </td>
                                    </tr>
                                </table>
                                <table style="margin-left: auto; margin-right: auto; margin-top: 10px;">
                                    <tr>
                                        <td>
                                            <input type="button" id="btnDiscountOK" value='<%= GetLabel("OK")%>' onclick="pcDiscount.Hide();"/>
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