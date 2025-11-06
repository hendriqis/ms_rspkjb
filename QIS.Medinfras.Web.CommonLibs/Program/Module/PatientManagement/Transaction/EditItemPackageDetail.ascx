<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditItemPackageDetail.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.EditItemPackageDetail" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_edititempackaegctl">
    $('#ulTabLabResult li').click(function () {
        $('#ulTabLabResult li.selected').removeAttr('class');
        $('.containerTransDtCtl').filter(':visible').hide();
        $contentID = $(this).attr('contentid');
        $('#' + $contentID).show();
        $(this).addClass('selected');
    });

    //#region SERVICES
    $('.imgDelete.imgLink').die('click');
    $('.imgDelete.imgLink').live('click', function (evt) {
        evt.stopPropagation();
        evt.preventDefault();
        if (confirm("Are You Sure Want To Delete This Data?")) {
            $row = $(this).closest('tr');
            var ID = $row.find('.ID').val();
            $('#<%=hdnID.ClientID %>').val(ID);

            cbpEntryPopupView.PerformCallback('delete');
        }
    });

    $('.imgEdit.imgLink').die('click');
    $('.imgEdit.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        var ID = $row.find('.ID').val();
        var ItemID = $row.find('.ItemID').val();
        var ItemCode = $row.find('.ItemCode').val();
        var ItemName1 = $row.find('.ItemName1').val();
        var ParamedicID = $row.find('.ParamedicID').val();
        var ParamedicCode = $row.find('.ParamedicCode').val();
        var ParamedicName = $row.find('.ParamedicName').val();
        var ParamedicParentID = $row.find('.ParamedicParentID').val();
        var RevenueSharingID = $row.find('.RevenueSharingID').val();
        var RevenueSharingCode = $row.find('.RevenueSharingCode').val();
        var RevenueSharingName = $row.find('.RevenueSharingName').val();
        var ChargedQuantity = $row.find('.ChargedQuantity').val();
        var baseComp1 = $row.find('.BaseComp1').val();
        var baseComp2 = $row.find('.BaseComp2').val();
        var baseComp3 = $row.find('.BaseComp3').val();
        var baseTariff = $row.find('.BaseTariff').val();
        var tariffComp1 = $row.find('.TariffComp1').val();
        var tariffComp2 = $row.find('.TariffComp2').val();
        var tariffComp3 = $row.find('.TariffComp3').val();
        var tariff = $row.find('.Tariff').val();
        var isAllowDiscount = $row.find('.IsAllowDiscount').val();
        var isAllowDiscountTariffComp1 = $row.find('.IsAllowDiscountTariffComp1').val();
        var isAllowDiscountTariffComp2 = $row.find('.IsAllowDiscountTariffComp2').val();
        var isAllowDiscountTariffComp3 = $row.find('.IsAllowDiscountTariffComp3').val();
        var discountComp1 = $row.find('.DiscountComp1').val();
        var discountComp2 = $row.find('.DiscountComp2').val();
        var discountComp3 = $row.find('.DiscountComp3').val();
        var discountPercentageComp1 = $row.find('.DiscountPercentageComp1').val();
        var discountPercentageComp2 = $row.find('.DiscountPercentageComp2').val();
        var discountPercentageComp3 = $row.find('.DiscountPercentageComp3').val();
        var discount = $row.find('.Discount').val();

        $('#<%=hdnID.ClientID %>').val(ID);
        $('#<%=hdnDetailItemID.ClientID %>').val(ItemID);
        $('#<%=txtDetailItemCode.ClientID %>').val(ItemCode);
        $('#<%=txtDetailItemName1.ClientID %>').val(ItemName1);
        $('#<%=hdnParamedicID.ClientID %>').val(ParamedicID);
        $('#<%=txtParamedicCode.ClientID %>').val(ParamedicCode);
        $('#<%=txtParamedicName.ClientID %>').val(ParamedicName);
        $('#<%=hdnRevenueSharingID.ClientID %>').val(RevenueSharingID);
        $('#<%=txtChargedQuantity.ClientID %>').val(ChargedQuantity);
        $('#<%=txtTariff.ClientID %>').val(tariff);
        $('#<%=txtServiceDiscTotalCtl.ClientID %>').val(discount).trigger('changeValue');
        $('#<%=txtServiceDiscComp1Ctl.ClientID %>').val(discountComp1).trigger('changeValue');
        $('#<%=txtServiceDiscComp2Ctl.ClientID %>').val(discountComp2).trigger('changeValue');
        $('#<%=txtServiceDiscComp3Ctl.ClientID %>').val(discountComp3).trigger('changeValue');
        $('#<%=hdnTariffAmount1Ctl.ClientID %>').val(tariffComp1);
        $('#<%=hdnTariffAmount2Ctl.ClientID %>').val(tariffComp2);
        $('#<%=hdnTariffAmount3Ctl.ClientID %>').val(tariffComp3);
        $('#<%=txtServiceDiscTariffComp1Ctl.ClientID %>').val(tariffComp1);
        $('#<%=txtServiceDiscTariffComp2Ctl.ClientID %>').val(tariffComp2);
        $('#<%=txtServiceDiscTariffComp3Ctl.ClientID %>').val(tariffComp3);

        var comp1 = tariffComp1 - discountComp1;
        var comp2 = tariffComp2 - discountComp2;
        var comp3 = tariffComp3 - discountComp3;

        $('#<%=txtServiceAfterTariffComp1Ctl.ClientID %>').val(comp1.formatMoney(2, '.', ','));
        $('#<%=txtServiceAfterTariffComp2Ctl.ClientID %>').val(comp2.formatMoney(2, '.', ','));
        $('#<%=txtServiceAfterTariffComp3Ctl.ClientID %>').val(comp3.formatMoney(2, '.', ','));

        $('#<%=txtServiceDiscPercentComp1Ctl.ClientID %>').val(discountPercentageComp1).trigger('changeValue');
        $('#<%=txtServiceDiscPercentComp2Ctl.ClientID %>').val(discountPercentageComp2).trigger('changeValue');
        $('#<%=txtServiceDiscPercentComp3Ctl.ClientID %>').val(discountPercentageComp3).trigger('changeValue');

        if (isAllowDiscountTariffComp1 == 'True') {
            $('#<%=txtServiceDiscPercentComp1Ctl.ClientID %>').removeAttr('readonly');
            $('#<%=txtServiceDiscComp1Ctl.ClientID %>').removeAttr('readonly');
        }
        else {
            $('#<%=txtServiceDiscPercentComp1Ctl.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtServiceDiscComp1Ctl.ClientID %>').attr('readonly', 'readonly');
        }

        if (isAllowDiscountTariffComp2 == 'True') {
            $('#<%=txtServiceDiscPercentComp2Ctl.ClientID %>').removeAttr('readonly');
            $('#<%=txtServiceDiscComp2Ctl.ClientID %>').removeAttr('readonly');
        }
        else {
            $('#<%=txtServiceDiscPercentComp2Ctl.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtServiceDiscComp2Ctl.ClientID %>').attr('readonly', 'readonly');
        }

        if (isAllowDiscountTariffComp3 == 'True') {
            $('#<%=txtServiceDiscPercentComp3Ctl.ClientID %>').removeAttr('readonly');
            $('#<%=txtServiceDiscComp3Ctl.ClientID %>').removeAttr('readonly');
        }
        else {
            $('#<%=txtServiceDiscPercentComp3Ctl.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtServiceDiscComp3Ctl.ClientID %>').attr('readonly', 'readonly');
        }

        $('#containerPopupEntryData').show();
    });

    $('#btnEntryPopupCancel').live('click', function () {
        $('#containerPopupEntryData').hide();
    });

    $('#btnEntryPopupSave').click(function (evt) {
        if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup'))
            cbpEntryPopupView.PerformCallback('save');
        return false;
    });

    $('#btnEntryPopupSaveAll').click(function (evt) {
        if (IsValid(evt, 'fsEntryPopupAll', 'mpEntryPopup'))
            cbpEntryPopupView.PerformCallback('saveAll');
        return false;
    });

    $('#<%=txtServiceDiscPercentComp1CtlAll.ClientID %>').change(function () {
        $(this).blur();
        var totalPercentComp1 = parseFloat($(this).attr('hiddenVal'));
        if (totalPercentComp1 > 100 || totalPercentComp1 < 0) {
            $('#<%=txtServiceDiscPercentComp1CtlAll.ClientID %>').val(0);
        }
    });

    $('#<%=txtServiceDiscPercentComp2CtlAll.ClientID %>').change(function () {
        $(this).blur();
        var totalPercentComp2 = parseFloat($(this).attr('hiddenVal'));
        if (totalPercentComp2 > 100 || totalPercentComp2 < 0) {
            $('#<%=txtServiceDiscPercentComp2CtlAll.ClientID %>').val(0);
        }
    });

    $('#<%=txtServiceDiscPercentComp3CtlAll.ClientID %>').change(function () {
        $(this).blur();
        var totalPercentComp3 = parseFloat($(this).attr('hiddenVal'));
        if (totalPercentComp3 > 100 || totalPercentComp3 < 0) {
            $('#<%=txtServiceDiscPercentComp3CtlAll.ClientID %>').val(0);
        }
    });

    $('#<%=txtServiceDiscPercentComp1Ctl.ClientID %>').change(function () {
        $(this).blur();
        var totalPercentComp1 = parseFloat($(this).attr('hiddenVal'));
        var qty = parseFloat($('#<%=txtChargedQuantity.ClientID %>').val().replace('.00', '').split(',').join(''));

        if (totalPercentComp1 > 100 || totalPercentComp1 < 0) {
            $('#<%=txtServiceDiscPercentComp1Ctl.ClientID %>').val(0);
            totalPercentComp1 = 0;
        }
        var tariffComp1ORI = parseFloat($('#<%=hdnTariffAmount1Ctl.ClientID %>').val().replace('.00', '').split(',').join(''));


        var discountComp1 = 0;
        if (totalPercentComp1 > 0) {
            discountComp1 = parseFloat(totalPercentComp1 * tariffComp1ORI / 100).toFixed(2);

            $('#<%=txtServiceDiscComp1Ctl.ClientID %>').val(discountComp1).trigger('changeValue');
            var tariffFinal = tariffComp1ORI - discountComp1;
            $('#<%=txtServiceAfterTariffComp1Ctl.ClientID %>').val(tariffFinal.formatMoney(2, '.', ',')).trigger('changeValue');
        }
        else {
            $('#<%=txtServiceDiscComp1Ctl.ClientID %>').val(0).trigger('changeValue');
            $('#<%=txtServiceAfterTariffComp1Ctl.ClientID %>').val(tariffComp1ORI.formatMoney(2, '.', ',')).trigger('changeValue');
        }

        var tempDiscountTotal = parseFloat($('#<%=txtServiceDiscComp1Ctl.ClientID %>').val().split(',').join(''))
                                        + parseFloat($('#<%=txtServiceDiscComp2Ctl.ClientID %>').val().split(',').join(''))
                                        + parseFloat($('#<%=txtServiceDiscComp3Ctl.ClientID %>').val().split(',').join(''));

        var discFinal = tempDiscountTotal * qty
        $('#<%=txtServiceDiscTotalCtl.ClientID %>').val(discFinal.formatMoney(2, '.', ','));
    });

    $('#<%=txtServiceDiscPercentComp2Ctl.ClientID %>').change(function () {
        $(this).blur();
        var totalPercentComp2 = parseFloat($(this).attr('hiddenVal'));
        var qty = parseFloat($('#<%=txtChargedQuantity.ClientID %>').val().replace('.00', '').split(',').join(''));

        if (totalPercentComp2 > 100 || totalPercentComp2 < 0) {
            $('#<%=txtServiceDiscPercentComp2Ctl.ClientID %>').val(0);
            totalPercentComp2 = 0;
        }
        var tariffComp2ORI = parseFloat($('#<%=hdnTariffAmount2Ctl.ClientID %>').val().replace('.00', '').split(',').join(''));


        var discountComp2 = 0;
        if (totalPercentComp2 > 0) {
            discountComp2 = parseFloat(totalPercentComp2 * tariffComp2ORI / 100).toFixed(2);

            $('#<%=txtServiceDiscComp2Ctl.ClientID %>').val(discountComp2).trigger('changeValue');
            var tariffFinal = tariffComp2ORI - discountComp2;
            $('#<%=txtServiceAfterTariffComp2Ctl.ClientID %>').val(tariffFinal.formatMoney(2, '.', ',')).trigger('changeValue');
        }
        else {
            $('#<%=txtServiceDiscComp2Ctl.ClientID %>').val(0).trigger('changeValue');
            $('#<%=txtServiceAfterTariffComp2Ctl.ClientID %>').val(tariffComp2ORI.formatMoney(2, '.', ',')).trigger('changeValue');
        }

        var tempDiscountTotal = parseFloat($('#<%=txtServiceDiscComp1Ctl.ClientID %>').val().split(',').join(''))
                                        + parseFloat($('#<%=txtServiceDiscComp2Ctl.ClientID %>').val().split(',').join(''))
                                        + parseFloat($('#<%=txtServiceDiscComp3Ctl.ClientID %>').val().split(',').join(''));

        var discFinal = tempDiscountTotal * qty
        $('#<%=txtServiceDiscTotalCtl.ClientID %>').val(discFinal.formatMoney(2, '.', ','));
    });

    $('#<%=txtServiceDiscPercentComp3Ctl.ClientID %>').change(function () {
        $(this).blur();
        var totalPercentComp3 = parseFloat($(this).attr('hiddenVal'));
        var qty = parseFloat($('#<%=txtChargedQuantity.ClientID %>').val().replace('.00', '').split(',').join(''));

        if (totalPercentComp3 > 100 || totalPercentComp3 < 0) {
            $('#<%=txtServiceDiscPercentComp3Ctl.ClientID %>').val(0);
            totalPercentComp3 = 0;
        }
        var tariffComp3ORI = parseFloat($('#<%=hdnTariffAmount3Ctl.ClientID %>').val().replace('.00', '').split(',').join(''));


        var discountComp3 = 0;
        if (totalPercentComp3 > 0) {
            discountComp3 = parseFloat(totalPercentComp3 * tariffComp3ORI / 100).toFixed(2);

            $('#<%=txtServiceDiscComp3Ctl.ClientID %>').val(discountComp3).trigger('changeValue');
            var tariffFinal = tariffComp3ORI - discountComp3;
            $('#<%=txtServiceAfterTariffComp3Ctl.ClientID %>').val(tariffFinal.formatMoney(2, '.', ',')).trigger('changeValue');
        }
        else {
            $('#<%=txtServiceDiscComp3Ctl.ClientID %>').val(0).trigger('changeValue');
            $('#<%=txtServiceAfterTariffComp3Ctl.ClientID %>').val(tariffComp3ORI.formatMoney(2, '.', ',')).trigger('changeValue');
        }

        var tempDiscountTotal = parseFloat($('#<%=txtServiceDiscComp1Ctl.ClientID %>').val().split(',').join(''))
                                        + parseFloat($('#<%=txtServiceDiscComp2Ctl.ClientID %>').val().split(',').join(''))
                                        + parseFloat($('#<%=txtServiceDiscComp3Ctl.ClientID %>').val().split(',').join(''));

        var discFinal = tempDiscountTotal * qty
        $('#<%=txtServiceDiscTotalCtl.ClientID %>').val(discFinal.formatMoney(2, '.', ','));
    });

    $('#<%=txtServiceDiscComp1Ctl.ClientID %>').change(function () {
        $('#<%=txtServiceDiscPercentComp1Ctl.ClientID %>').val(0).trigger('changeValue');

        var disc1 = parseFloat(checkMinusDecimalOK($('#<%=txtServiceDiscComp1Ctl.ClientID %>').val()));
        var disc2 = parseFloat(checkMinusDecimalOK($('#<%=txtServiceDiscComp2Ctl.ClientID %>').val()));
        var disc3 = parseFloat(checkMinusDecimalOK($('#<%=txtServiceDiscComp3Ctl.ClientID %>').val()));

        var tariffComp1 = parseFloat($('#<%=hdnTariffAmount1Ctl.ClientID %>').val().replace('.00', '').split(',').join(''));
        var tariffComp2 = parseFloat($('#<%=hdnTariffAmount2Ctl.ClientID %>').val().replace('.00', '').split(',').join(''));
        var tariffComp3 = parseFloat($('#<%=hdnTariffAmount3Ctl.ClientID %>').val().replace('.00', '').split(',').join(''));

        var qty = parseFloat($('#<%=txtChargedQuantity.ClientID %>').val().replace('.00', '').split(',').join(''));

        if (disc1 > tariffComp1) {
            disc1 = tariffComp1;
            $('#<%=txtServiceDiscComp1Ctl.ClientID %>').val(tariffComp1);
        }
        else {
            $('#<%=txtServiceDiscComp1Ctl.ClientID %>').val(disc1);
        }

        var totalDisc = (disc1 + disc2 + disc3) * qty;

        var tariffFinal = tariffComp1 - disc1;
        $('#<%=txtServiceAfterTariffComp1Ctl.ClientID %>').val(tariffFinal.formatMoney(2, '.', ',')).trigger('changeValue');
        $('#<%=txtServiceDiscTotalCtl.ClientID %>').val(totalDisc.formatMoney(2, '.', ','));
    });

    $('#<%=txtServiceDiscComp2Ctl.ClientID %>').change(function () {
        $('#<%=txtServiceDiscPercentComp2Ctl.ClientID %>').val(0).trigger('changeValue');

        var disc1 = parseFloat(checkMinusDecimalOK($('#<%=txtServiceDiscComp1Ctl.ClientID %>').val()));
        var disc2 = parseFloat(checkMinusDecimalOK($('#<%=txtServiceDiscComp2Ctl.ClientID %>').val()));
        var disc3 = parseFloat(checkMinusDecimalOK($('#<%=txtServiceDiscComp3Ctl.ClientID %>').val()));

        var tariffComp1 = parseFloat($('#<%=hdnTariffAmount1Ctl.ClientID %>').val().replace('.00', '').split(',').join(''));
        var tariffComp2 = parseFloat($('#<%=hdnTariffAmount2Ctl.ClientID %>').val().replace('.00', '').split(',').join(''));
        var tariffComp3 = parseFloat($('#<%=hdnTariffAmount3Ctl.ClientID %>').val().replace('.00', '').split(',').join(''));

        var qty = parseFloat($('#<%=txtChargedQuantity.ClientID %>').val().replace('.00', '').split(',').join(''));

        if (disc2 > tariffComp2) {
            disc2 = tariffComp2;
            $('#<%=txtServiceDiscComp2Ctl.ClientID %>').val(tariffComp2);
        }
        else {
            $('#<%=txtServiceDiscComp2Ctl.ClientID %>').val(disc2);
        }

        var totalDisc = (disc1 + disc2 + disc3) * qty;

        var tariffFinal = tariffComp2 - disc2;
        $('#<%=txtServiceAfterTariffComp2Ctl.ClientID %>').val(tariffFinal.formatMoney(2, '.', ',')).trigger('changeValue');
        $('#<%=txtServiceDiscTotalCtl.ClientID %>').val(totalDisc.formatMoney(2, '.', ','));
    });

    $('#<%=txtServiceDiscComp3Ctl.ClientID %>').change(function () {
        $('#<%=txtServiceDiscPercentComp3Ctl.ClientID %>').val(0).trigger('changeValue');

        var disc1 = parseFloat(checkMinusDecimalOK($('#<%=txtServiceDiscComp1Ctl.ClientID %>').val()));
        var disc2 = parseFloat(checkMinusDecimalOK($('#<%=txtServiceDiscComp2Ctl.ClientID %>').val()));
        var disc3 = parseFloat(checkMinusDecimalOK($('#<%=txtServiceDiscComp3Ctl.ClientID %>').val()));

        var tariffComp1 = parseFloat($('#<%=hdnTariffAmount1Ctl.ClientID %>').val().replace('.00', '').split(',').join(''));
        var tariffComp2 = parseFloat($('#<%=hdnTariffAmount2Ctl.ClientID %>').val().replace('.00', '').split(',').join(''));
        var tariffComp3 = parseFloat($('#<%=hdnTariffAmount3Ctl.ClientID %>').val().replace('.00', '').split(',').join(''));

        var qty = parseFloat($('#<%=txtChargedQuantity.ClientID %>').val().replace('.00', '').split(',').join(''));

        if (disc3 > tariffComp3) {
            disc3 = tariffComp3;
            $('#<%=txtServiceDiscComp3Ctl.ClientID %>').val(tariffComp3);
        }
        else {
            $('#<%=txtServiceDiscComp3Ctl.ClientID %>').val(disc3);
        }

        var totalDisc = (disc1 + disc2 + disc3) * qty;

        var tariffFinal = tariffComp3 - disc3;
        $('#<%=txtServiceAfterTariffComp3Ctl.ClientID %>').val(tariffFinal.formatMoney(2, '.', ',')).trigger('changeValue');
        $('#<%=txtServiceDiscTotalCtl.ClientID %>').val(totalDisc.formatMoney(2, '.', ','));
    });

    $('#<%=txtChargedQuantity.ClientID %>').change(function () {
        var qty = parseFloat(checkMinusDecimalOK($('#<%=txtChargedQuantity.ClientID %>').val()));
        $('#<%=txtChargedQuantity.ClientID %>').val(qty);

        var disc1 = parseFloat(checkMinusDecimalOK($('#<%=txtServiceDiscComp1Ctl.ClientID %>').val()));
        var disc2 = parseFloat(checkMinusDecimalOK($('#<%=txtServiceDiscComp2Ctl.ClientID %>').val()));
        var disc3 = parseFloat(checkMinusDecimalOK($('#<%=txtServiceDiscComp3Ctl.ClientID %>').val()));

        var totalDisc = (disc1 + disc2 + disc3) * qty;
        $('#<%=txtServiceDiscTotalCtl.ClientID %>').val(totalDisc.formatMoney(2, '.', ','));
    });

    //#region Paramedic
    $('#lblParamedicDetail.lblLink').click(function () {
        openSearchDialog('paramedic', "IsDeleted = 0", function (value) {
            $('#<%=txtParamedicCode.ClientID %>').val(value);
            onTxtParamedicCodeChanged(value);
        });
    });

    $('#<%=txtParamedicCode.ClientID %>').change(function () {
        onTxtParamedicCodeChanged($(this).val());
    });

    function onTxtParamedicCodeChanged(value) {
        var filterExpression = "ParamedicCode = '" + value + "'";
        Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnParamedicID.ClientID %>').val(result.ParamedicID);
                $('#<%=txtParamedicName.ClientID %>').val(result.ParamedicName);

                var today = new Date();
                var date = today.getDate() + '-' + (today.getMonth() + 1) + '-' + today.getFullYear();
                var time = today.getHours() + ":" + today.getMinutes();

                var hdnTransactionDate = $('#<%=hdnTransactionDate.ClientID %>').val() == "" ? date : $('#<%=hdnTransactionDate.ClientID %>').val();
                var hdnTransactionTime = $('#<%=hdnTransactionTime.ClientID %>').val() == "" ? time : $('#<%=hdnTransactionTime.ClientID %>').val();
                var hdnChargesHealthcareServiceUnitID = $('#<%=hdnChargesHealthcareServiceUnitID.ClientID %>').val();

                var chargesClassID = $('#<%=hdnChargesClassID.ClientID %>').val();
                var itemCode = $('#<%=txtDetailItemCode.ClientID %>').val();
                var paramedicRole = "X084^001"; // Constant.ParamedicRole.PELAKSANA
                if (itemCode != '') {
                    Methods.getItemRevenueSharing(itemCode, result.ParamedicID, chargesClassID, paramedicRole, $('#<%=hdnVisitIDCtl.ClientID %>').val(), hdnChargesHealthcareServiceUnitID, hdnTransactionDate, hdnTransactionTime, function (result) {
                        if (result != null)
                            $('#<%=hdnRevenueSharingID.ClientID %>').val(result.RevenueSharingID);
                        else
                            $('#<%=hdnRevenueSharingID.ClientID %>').val('');
                    });
                }
            }
            else {
                $('#<%=hdnParamedicID.ClientID %>').val('');
                $('#<%=txtParamedicCode.ClientID %>').val('');
                $('#<%=txtParamedicName.ClientID %>').val('');
                $('#<%=hdnRevenueSharingID.ClientID %>').val('');
            }
        });
    }
    //#endregion

    function onCbpEntryPopupViewEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save' || param[0] == 'saveAll') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else {
                $('#containerPopupEntryData').hide();
                cbpService.PerformCallback();

                if (param[0] == 'saveAll') {
                    cbpEntryPopupView.PerformCallback();
                    cbpEntryPopupViewObat.PerformCallback();
                    cbpEntryPopupViewBarang.PerformCallback();
                }
            }
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail')
                showToast('Delete Failed', 'Error Message : ' + param[2]);
        }
        $('#containerImgLoadingViewPopup').hide();
    }
    //#endregion

    //#region DRUGS
    $('.imgDeleteObat.imgLink').die('click');
    $('.imgDeleteObat.imgLink').live('click', function (evt) {
        evt.stopPropagation();
        evt.preventDefault();
        if (confirm("Are You Sure Want To Delete This Data?")) {
            $row = $(this).closest('tr');
            var ID = $row.find('.IDObat').val();
            $('#<%=hdnIDObat.ClientID %>').val(ID);

            cbpEntryPopupView.PerformCallback('delete');
        }
    });

    $('.imgEditObat.imgLink').die('click');
    $('.imgEditObat.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        var ID = $row.find('.IDObat').val();
        var ItemID = $row.find('.ItemIDObat').val();
        var ItemCode = $row.find('.ItemCodeObat').val();
        var ItemName1 = $row.find('.ItemName1Obat').val();
        var ParamedicID = $row.find('.ParamedicIDObat').val();
        var ParamedicCode = $row.find('.ParamedicCodeObat').val();
        var ParamedicName = $row.find('.ParamedicNameObat').val();
        var ParamedicParentID = $row.find('.ParamedicParentIDObat').val();
        var RevenueSharingID = $row.find('.RevenueSharingIDObat').val();
        var RevenueSharingCode = $row.find('.RevenueSharingCodeObat').val();
        var RevenueSharingName = $row.find('.RevenueSharingNameObat').val();
        var ChargedQuantity = $row.find('.ChargedQuantityObat').val();

        var baseComp1 = $row.find('.BaseComp1Obat').val();
        var baseComp2 = $row.find('.BaseComp2Obat').val();
        var baseComp3 = $row.find('.BaseComp3Obat').val();
        var baseTariff = $row.find('.BaseTariffObat').val();
        var tariffComp1 = $row.find('.TariffComp1Obat').val();
        var tariffComp2 = $row.find('.TariffComp2Obat').val();
        var tariffComp3 = $row.find('.TariffComp3Obat').val();
        var tariff = $row.find('.TariffObat').val();
        var isAllowDiscount = $row.find('.IsAllowDiscountObat').val();
        var isAllowDiscountTariffComp1 = $row.find('.IsAllowDiscountTariffComp1Obat').val();
        var isAllowDiscountTariffComp2 = $row.find('.IsAllowDiscountTariffComp2Obat').val();
        var isAllowDiscountTariffComp3 = $row.find('.IsAllowDiscountTariffComp3Obat').val();
        var discountComp1 = $row.find('.DiscountComp1Obat').val();
        var discountComp2 = $row.find('.DiscountComp2Obat').val();
        var discountComp3 = $row.find('.DiscountComp3Obat').val();
        var discountPercentageComp1 = $row.find('.DiscountPercentageComp1Obat').val();
        var discountPercentageComp2 = $row.find('.DiscountPercentageComp2Obat').val();
        var discountPercentageComp3 = $row.find('.DiscountPercentageComp3Obat').val();
        var discount = $row.find('.DiscountObat').val();

        $('#<%=hdnIDObat.ClientID %>').val(ID);
        $('#<%=hdnDetailItemIDObat.ClientID %>').val(ItemID);
        $('#<%=txtDetailItemCodeObat.ClientID %>').val(ItemCode);
        $('#<%=txtDetailItemName1Obat.ClientID %>').val(ItemName1);
        $('#<%=hdnParamedicIDObat.ClientID %>').val(ParamedicID);
        $('#<%=txtParamedicCodeObat.ClientID %>').val(ParamedicCode);
        $('#<%=txtParamedicNameObat.ClientID %>').val(ParamedicName);
        $('#<%=hdnRevenueSharingIDObat.ClientID %>').val(RevenueSharingID);
        $('#<%=txtChargedQuantityObat.ClientID %>').val(ChargedQuantity);
        $('#<%=txtObatDiscTotalCtl.ClientID %>').val(discount).trigger('changeValue');
        $('#<%=txtObatDiscComp1Ctl.ClientID %>').val(discountComp1).trigger('changeValue');
        $('#<%=txtObatDiscComp2Ctl.ClientID %>').val(discountComp2).trigger('changeValue');
        $('#<%=txtObatDiscComp3Ctl.ClientID %>').val(discountComp3).trigger('changeValue');
        $('#<%=hdnTariffAmount1CtlObat.ClientID %>').val(tariffComp1);
        $('#<%=hdnTariffAmount2CtlObat.ClientID %>').val(tariffComp2);
        $('#<%=hdnTariffAmount3CtlObat.ClientID %>').val(tariffComp3);
        $('#<%=txtObatDiscTariffComp1Ctl.ClientID %>').val(tariffComp1);
        $('#<%=txtObatDiscTariffComp2Ctl.ClientID %>').val(tariffComp2);
        $('#<%=txtObatDiscTariffComp3Ctl.ClientID %>').val(tariffComp3);

        var comp1 = tariffComp1 - discountComp1;
        var comp2 = tariffComp2 - discountComp2;
        var comp3 = tariffComp3 - discountComp3;

        $('#<%=txtObatAfterTariffComp1Ctl.ClientID %>').val(comp1.formatMoney(2, '.', ','));
        $('#<%=txtObatAfterTariffComp2Ctl.ClientID %>').val(comp2.formatMoney(2, '.', ','));
        $('#<%=txtObatAfterTariffComp3Ctl.ClientID %>').val(comp3.formatMoney(2, '.', ','));

        $('#<%=txtObatDiscPercentComp1Ctl.ClientID %>').val(discountPercentageComp1).trigger('changeValue');
        $('#<%=txtObatDiscPercentComp2Ctl.ClientID %>').val(discountPercentageComp2).trigger('changeValue');
        $('#<%=txtObatDiscPercentComp3Ctl.ClientID %>').val(discountPercentageComp3).trigger('changeValue');

        if (isAllowDiscountTariffComp1 == 'True') {
            $('#<%=txtObatDiscPercentComp1Ctl.ClientID %>').removeAttr('readonly');
            $('#<%=txtObatDiscComp1Ctl.ClientID %>').removeAttr('readonly');
        }
        else {
            $('#<%=txtObatDiscPercentComp1Ctl.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtObatDiscComp1Ctl.ClientID %>').attr('readonly', 'readonly');
        }

        if (isAllowDiscountTariffComp2 == 'True') {
            $('#<%=txtObatDiscPercentComp2Ctl.ClientID %>').removeAttr('readonly');
            $('#<%=txtObatDiscComp2Ctl.ClientID %>').removeAttr('readonly');
        }
        else {
            $('#<%=txtObatDiscPercentComp2Ctl.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtObatDiscComp2Ctl.ClientID %>').attr('readonly', 'readonly');
        }

        if (isAllowDiscountTariffComp3 == 'True') {
            $('#<%=txtObatDiscPercentComp3Ctl.ClientID %>').removeAttr('readonly');
            $('#<%=txtObatDiscComp3Ctl.ClientID %>').removeAttr('readonly');
        }
        else {
            $('#<%=txtObatDiscPercentComp3Ctl.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtObatDiscComp3Ctl.ClientID %>').attr('readonly', 'readonly');
        }

        $('#containerPopupEntryDataObat').show();
    });

    $('#btnEntryPopupCancelObat').live('click', function () {
        $('#containerPopupEntryDataObat').hide();
    });

    $('#btnEntryPopupSaveObat').click(function (evt) {
        if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup'))
            cbpEntryPopupViewObat.PerformCallback('save');
        return false;
    });

    $('#<%=txtObatDiscPercentComp1Ctl.ClientID %>').change(function () {
        $(this).blur();
        var totalPercentComp1 = parseFloat($(this).attr('hiddenVal'));
        var qty = parseFloat($('#<%=txtChargedQuantityObat.ClientID %>').val().replace('.00', '').split(',').join(''));

        if (totalPercentComp1 > 100 || totalPercentComp1 < 0) {
            $('#<%=txtObatDiscPercentComp1Ctl.ClientID %>').val(0);
            totalPercentComp1 = 0;
        }
        var tariffComp1ORI = parseFloat($('#<%=hdnTariffAmount1CtlObat.ClientID %>').val().replace('.00', '').split(',').join(''));


        var discountComp1 = 0;
        if (totalPercentComp1 > 0) {
            discountComp1 = parseFloat(totalPercentComp1 * tariffComp1ORI / 100).toFixed(2);

            $('#<%=txtObatDiscComp1Ctl.ClientID %>').val(discountComp1).trigger('changeValue');
            var tariffFinal = tariffComp1ORI - discountComp1;
            $('#<%=txtObatAfterTariffComp1Ctl.ClientID %>').val(tariffFinal.formatMoney(2, '.', ',')).trigger('changeValue');
        }
        else {
            $('#<%=txtObatDiscComp1Ctl.ClientID %>').val(0).trigger('changeValue');
            $('#<%=txtObatAfterTariffComp1Ctl.ClientID %>').val(tariffComp1ORI.formatMoney(2, '.', ',')).trigger('changeValue');
        }

        var tempDiscountTotal = parseFloat($('#<%=txtObatDiscComp1Ctl.ClientID %>').val().split(',').join(''))
                                        + parseFloat($('#<%=txtObatDiscComp2Ctl.ClientID %>').val().split(',').join(''))
                                        + parseFloat($('#<%=txtObatDiscComp3Ctl.ClientID %>').val().split(',').join(''));

        var discFinal = tempDiscountTotal * qty
        $('#<%=txtObatDiscTotalCtl.ClientID %>').val(discFinal.formatMoney(2, '.', ','));
    });

    $('#<%=txtObatDiscPercentComp2Ctl.ClientID %>').change(function () {
        $(this).blur();
        var totalPercentComp2 = parseFloat($(this).attr('hiddenVal'));
        var qty = parseFloat($('#<%=txtChargedQuantityObat.ClientID %>').val().replace('.00', '').split(',').join(''));

        if (totalPercentComp2 > 100 || totalPercentComp2 < 0) {
            $('#<%=txtObatDiscPercentComp2Ctl.ClientID %>').val(0);
            totalPercentComp2 = 0;
        }
        var tariffComp2ORI = parseFloat($('#<%=hdnTariffAmount2CtlObat.ClientID %>').val().replace('.00', '').split(',').join(''));


        var discountComp2 = 0;
        if (totalPercentComp2 > 0) {
            discountComp2 = parseFloat(totalPercentComp2 * tariffComp2ORI / 100).toFixed(2);

            $('#<%=txtObatDiscComp2Ctl.ClientID %>').val(discountComp2).trigger('changeValue');
            var tariffFinal = tariffComp2ORI - discountComp2;
            $('#<%=txtObatAfterTariffComp2Ctl.ClientID %>').val(tariffFinal.formatMoney(2, '.', ',')).trigger('changeValue');
        }
        else {
            $('#<%=txtObatDiscComp2Ctl.ClientID %>').val(0).trigger('changeValue');
            $('#<%=txtObatAfterTariffComp2Ctl.ClientID %>').val(tariffComp2ORI.formatMoney(2, '.', ',')).trigger('changeValue');
        }

        var tempDiscountTotal = parseFloat($('#<%=txtObatDiscComp1Ctl.ClientID %>').val().split(',').join(''))
                                        + parseFloat($('#<%=txtObatDiscComp2Ctl.ClientID %>').val().split(',').join(''))
                                        + parseFloat($('#<%=txtObatDiscComp3Ctl.ClientID %>').val().split(',').join(''));

        var discFinal = tempDiscountTotal * qty
        $('#<%=txtObatDiscTotalCtl.ClientID %>').val(discFinal.formatMoney(2, '.', ','));
    });

    $('#<%=txtObatDiscPercentComp3Ctl.ClientID %>').change(function () {
        $(this).blur();
        var totalPercentComp3 = parseFloat($(this).attr('hiddenVal'));
        var qty = parseFloat($('#<%=txtChargedQuantityObat.ClientID %>').val().replace('.00', '').split(',').join(''));

        if (totalPercentComp3 > 100 || totalPercentComp3 < 0) {
            $('#<%=txtObatDiscPercentComp3Ctl.ClientID %>').val(0);
            totalPercentComp3 = 0;
        }
        var tariffComp3ORI = parseFloat($('#<%=hdnTariffAmount3CtlObat.ClientID %>').val().replace('.00', '').split(',').join(''));


        var discountComp3 = 0;
        if (totalPercentComp3 > 0) {
            discountComp3 = parseFloat(totalPercentComp3 * tariffComp3ORI / 100).toFixed(2);

            $('#<%=txtObatDiscComp3Ctl.ClientID %>').val(discountComp3).trigger('changeValue');
            var tariffFinal = tariffComp3ORI - discountComp3;
            $('#<%=txtObatAfterTariffComp3Ctl.ClientID %>').val(tariffFinal.formatMoney(2, '.', ',')).trigger('changeValue');
        }
        else {
            $('#<%=txtObatDiscComp3Ctl.ClientID %>').val(0).trigger('changeValue');
            $('#<%=txtObatAfterTariffComp3Ctl.ClientID %>').val(tariffComp3ORI.formatMoney(2, '.', ',')).trigger('changeValue');
        }

        var tempDiscountTotal = parseFloat($('#<%=txtObatDiscComp1Ctl.ClientID %>').val().split(',').join(''))
                                        + parseFloat($('#<%=txtObatDiscComp2Ctl.ClientID %>').val().split(',').join(''))
                                        + parseFloat($('#<%=txtObatDiscComp3Ctl.ClientID %>').val().split(',').join(''));

        var discFinal = tempDiscountTotal * qty
        $('#<%=txtObatDiscTotalCtl.ClientID %>').val(discFinal.formatMoney(2, '.', ','));
    });

    $('#<%=txtObatDiscComp1Ctl.ClientID %>').change(function () {
        $('#<%=txtObatDiscPercentComp1Ctl.ClientID %>').val(0).trigger('changeValue');

        var disc1 = parseFloat(checkMinusDecimalOK($('#<%=txtObatDiscComp1Ctl.ClientID %>').val()));
        var disc2 = parseFloat(checkMinusDecimalOK($('#<%=txtObatDiscComp2Ctl.ClientID %>').val()));
        var disc3 = parseFloat(checkMinusDecimalOK($('#<%=txtObatDiscComp3Ctl.ClientID %>').val()));

        var tariffComp1 = parseFloat($('#<%=hdnTariffAmount1CtlObat.ClientID %>').val().replace('.00', '').split(',').join(''));
        var tariffComp2 = parseFloat($('#<%=hdnTariffAmount2CtlObat.ClientID %>').val().replace('.00', '').split(',').join(''));
        var tariffComp3 = parseFloat($('#<%=hdnTariffAmount3CtlObat.ClientID %>').val().replace('.00', '').split(',').join(''));

        var qty = parseFloat($('#<%=txtChargedQuantityObat.ClientID %>').val().replace('.00', '').split(',').join(''));

        if (disc1 > tariffComp1) {
            disc1 = tariffComp1;
            $('#<%=txtObatDiscComp1Ctl.ClientID %>').val(tariffComp1);
        }
        else {
            $('#<%=txtObatDiscComp1Ctl.ClientID %>').val(disc1);
        }

        var totalDisc = (disc1 + disc2 + disc3) * qty;

        var tariffFinal = tariffComp1 - disc1;
        $('#<%=txtObatAfterTariffComp1Ctl.ClientID %>').val(tariffFinal.formatMoney(2, '.', ',')).trigger('changeValue');
        $('#<%=txtObatDiscTotalCtl.ClientID %>').val(totalDisc.formatMoney(2, '.', ','));
    });

    $('#<%=txtObatDiscComp2Ctl.ClientID %>').change(function () {
        $('#<%=txtObatDiscPercentComp2Ctl.ClientID %>').val(0).trigger('changeValue');

        var disc1 = parseFloat(checkMinusDecimalOK($('#<%=txtObatDiscComp1Ctl.ClientID %>').val()));
        var disc2 = parseFloat(checkMinusDecimalOK($('#<%=txtObatDiscComp2Ctl.ClientID %>').val()));
        var disc3 = parseFloat(checkMinusDecimalOK($('#<%=txtObatDiscComp3Ctl.ClientID %>').val()));

        var tariffComp1 = parseFloat($('#<%=hdnTariffAmount1CtlObat.ClientID %>').val().replace('.00', '').split(',').join(''));
        var tariffComp2 = parseFloat($('#<%=hdnTariffAmount2CtlObat.ClientID %>').val().replace('.00', '').split(',').join(''));
        var tariffComp3 = parseFloat($('#<%=hdnTariffAmount3CtlObat.ClientID %>').val().replace('.00', '').split(',').join(''));

        var qty = parseFloat($('#<%=txtChargedQuantityObat.ClientID %>').val().replace('.00', '').split(',').join(''));

        if (disc2 > tariffComp2) {
            disc2 = tariffComp2;
            $('#<%=txtObatDiscComp2Ctl.ClientID %>').val(tariffComp2);
        }
        else {
            $('#<%=txtObatDiscComp2Ctl.ClientID %>').val(disc2);
        }

        var totalDisc = (disc1 + disc2 + disc3) * qty;

        var tariffFinal = tariffComp2 - disc2;
        $('#<%=txtObatAfterTariffComp2Ctl.ClientID %>').val(tariffFinal.formatMoney(2, '.', ',')).trigger('changeValue');
        $('#<%=txtObatDiscTotalCtl.ClientID %>').val(totalDisc.formatMoney(2, '.', ','));
    });

    $('#<%=txtObatDiscComp3Ctl.ClientID %>').change(function () {
        $('#<%=txtObatDiscPercentComp3Ctl.ClientID %>').val(0).trigger('changeValue');

        var disc1 = parseFloat(checkMinusDecimalOK($('#<%=txtObatDiscComp1Ctl.ClientID %>').val()));
        var disc2 = parseFloat(checkMinusDecimalOK($('#<%=txtObatDiscComp2Ctl.ClientID %>').val()));
        var disc3 = parseFloat(checkMinusDecimalOK($('#<%=txtObatDiscComp3Ctl.ClientID %>').val()));

        var tariffComp1 = parseFloat($('#<%=hdnTariffAmount1CtlObat.ClientID %>').val().replace('.00', '').split(',').join(''));
        var tariffComp2 = parseFloat($('#<%=hdnTariffAmount2CtlObat.ClientID %>').val().replace('.00', '').split(',').join(''));
        var tariffComp3 = parseFloat($('#<%=hdnTariffAmount3CtlObat.ClientID %>').val().replace('.00', '').split(',').join(''));

        var qty = parseFloat($('#<%=txtChargedQuantityObat.ClientID %>').val().replace('.00', '').split(',').join(''));

        if (disc3 > tariffComp3) {
            disc3 = tariffComp3;
            $('#<%=txtObatDiscComp3Ctl.ClientID %>').val(tariffComp3);
        }
        else {
            $('#<%=txtObatDiscComp3Ctl.ClientID %>').val(disc3);
        }

        var totalDisc = (disc1 + disc2 + disc3) * qty;

        var tariffFinal = tariffComp3 - disc3;
        $('#<%=txtObatAfterTariffComp3Ctl.ClientID %>').val(tariffFinal.formatMoney(2, '.', ',')).trigger('changeValue');
        $('#<%=txtObatDiscTotalCtl.ClientID %>').val(totalDisc.formatMoney(2, '.', ','));
    });

    $('#<%=txtChargedQuantityObat.ClientID %>').change(function () {
        var qty = parseFloat(checkMinusDecimalOK($('#<%=txtChargedQuantityObat.ClientID %>').val()));
        $('#<%=txtChargedQuantityObat.ClientID %>').val(qty);

        var disc1 = parseFloat(checkMinusDecimalOK($('#<%=txtObatDiscComp1Ctl.ClientID %>').val()));
        var disc2 = parseFloat(checkMinusDecimalOK($('#<%=txtObatDiscComp2Ctl.ClientID %>').val()));
        var disc3 = parseFloat(checkMinusDecimalOK($('#<%=txtObatDiscComp3Ctl.ClientID %>').val()));

        var totalDisc = (disc1 + disc2 + disc3) * qty;
        $('#<%=txtObatDiscTotalCtl.ClientID %>').val(totalDisc.formatMoney(2, '.', ','));
    });

    //#region Paramedic
    $('#lblParamedicDetailObat.lblLink').click(function () {
        openSearchDialog('paramedic', "IsDeleted = 0", function (value) {
            $('#<%=txtParamedicCodeObat.ClientID %>').val(value);
            onTxtParamedicCodeObatChanged(value);
        });
    });

    $('#<%=txtParamedicCodeObat.ClientID %>').change(function () {
        onTxtParamedicCodeObatChanged($(this).val());
    });

    function onTxtParamedicCodeObatChanged(value) {
        var filterExpression = "ParamedicCode = '" + value + "'";
        Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnParamedicIDObat.ClientID %>').val(result.ParamedicID);
                $('#<%=txtParamedicNameObat.ClientID %>').val(result.ParamedicName);

                var today = new Date();
                var date = today.getDate() + '-' + (today.getMonth() + 1) + '-' + today.getFullYear();
                var time = today.getHours() + ":" + today.getMinutes();

                var hdnTransactionDate = $('#<%=hdnTransactionDate.ClientID %>').val() == "" ? date : $('#<%=hdnTransactionDate.ClientID %>').val();
                var hdnTransactionTime = $('#<%=hdnTransactionTime.ClientID %>').val() == "" ? time : $('#<%=hdnTransactionTime.ClientID %>').val();
                var hdnChargesHealthcareServiceUnitID = $('#<%=hdnChargesHealthcareServiceUnitID.ClientID %>').val();

                var chargesClassID = $('#<%=hdnChargesClassID.ClientID %>').val();
                var itemCode = $('#<%=txtDetailItemCodeObat.ClientID %>').val();
                var paramedicRole = "X084^001"; // Constant.ParamedicRole.PELAKSANA
                if (itemCode != '') {
                    Methods.getItemRevenueSharing(itemCode, result.ParamedicID, chargesClassID, paramedicRole, $('#<%=hdnVisitIDCtl.ClientID %>').val(), hdnChargesHealthcareServiceUnitID, hdnTransactionDate, hdnTransactionTime, function (result) {
                        if (result != null)
                            $('#<%=hdnRevenueSharingIDObat.ClientID %>').val(result.RevenueSharingID);
                        else
                            $('#<%=hdnRevenueSharingIDObat.ClientID %>').val('');
                    });
                }
            }
            else {
                $('#<%=hdnParamedicIDObat.ClientID %>').val('');
                $('#<%=txtParamedicCodeObat.ClientID %>').val('');
                $('#<%=txtParamedicNameObat.ClientID %>').val('');
                $('#<%=hdnRevenueSharingIDObat.ClientID %>').val('');
            }
        });
    }
    //#endregion

    function onCbpEntryPopupViewObatEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else {
                $('#containerPopupEntryDataObat').hide();
                cbpService.PerformCallback();
            }
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail')
                showToast('Delete Failed', 'Error Message : ' + param[2]);
        }
        $('#containerImgLoadingViewPopupObat').hide();
    }
    //#endregion

    //#region LOGISTICS
    $('.imgDeleteBarang.imgLink').die('click');
    $('.imgDeleteBarang.imgLink').live('click', function (evt) {
        evt.stopPropagation();
        evt.preventDefault();
        if (confirm("Are You Sure Want To Delete This Data?")) {
            $row = $(this).closest('tr');
            var ID = $row.find('.IDBarang').val();
            $('#<%=hdnIDBarang.ClientID %>').val(ID);

            cbpEntryPopupView.PerformCallback('delete');
        }
    });

    $('.imgEditBarang.imgLink').die('click');
    $('.imgEditBarang.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        var ID = $row.find('.IDBarang').val();
        var ItemID = $row.find('.ItemIDBarang').val();
        var ItemCode = $row.find('.ItemCodeBarang').val();
        var ItemName1 = $row.find('.ItemName1Barang').val();
        var ParamedicID = $row.find('.ParamedicIDBarang').val();
        var ParamedicCode = $row.find('.ParamedicCodeBarang').val();
        var ParamedicName = $row.find('.ParamedicNameBarang').val();
        var ParamedicParentID = $row.find('.ParamedicParentIDBarang').val();
        var RevenueSharingID = $row.find('.RevenueSharingIDBarang').val();
        var RevenueSharingCode = $row.find('.RevenueSharingCodeBarang').val();
        var RevenueSharingName = $row.find('.RevenueSharingNameBarang').val();
        var ChargedQuantity = $row.find('.ChargedQuantityBarang').val();
        var baseComp1 = $row.find('.BaseComp1Barang').val();
        var baseComp2 = $row.find('.BaseComp2Barang').val();
        var baseComp3 = $row.find('.BaseComp3Barang').val();
        var baseTariff = $row.find('.BaseTariffBarang').val();
        var tariffComp1 = $row.find('.TariffComp1Barang').val();
        var tariffComp2 = $row.find('.TariffComp2Barang').val();
        var tariffComp3 = $row.find('.TariffComp3Barang').val();
        var tariff = $row.find('.TariffBarang').val();
        var isAllowDiscount = $row.find('.IsAllowDiscountBarang').val();
        var isAllowDiscountTariffComp1 = $row.find('.IsAllowDiscountTariffComp1Barang').val();
        var isAllowDiscountTariffComp2 = $row.find('.IsAllowDiscountTariffComp2Barang').val();
        var isAllowDiscountTariffComp3 = $row.find('.IsAllowDiscountTariffComp3Barang').val();
        var discountComp1 = $row.find('.DiscountComp1Barang').val();
        var discountComp2 = $row.find('.DiscountComp2Barang').val();
        var discountComp3 = $row.find('.DiscountComp3Barang').val();
        var discountPercentageComp1 = $row.find('.DiscountPercentageComp1Barang').val();
        var discountPercentageComp2 = $row.find('.DiscountPercentageComp2Barang').val();
        var discountPercentageComp3 = $row.find('.DiscountPercentageComp3Barang').val();
        var discount = $row.find('.DiscountBarang').val();

        $('#<%=hdnIDBarang.ClientID %>').val(ID);
        $('#<%=hdnDetailItemIDBarang.ClientID %>').val(ItemID);
        $('#<%=txtDetailItemCodeBarang.ClientID %>').val(ItemCode);
        $('#<%=txtDetailItemName1Barang.ClientID %>').val(ItemName1);
        $('#<%=hdnParamedicIDBarang.ClientID %>').val(ParamedicID);
        $('#<%=txtParamedicCodeBarang.ClientID %>').val(ParamedicCode);
        $('#<%=txtParamedicNameBarang.ClientID %>').val(ParamedicName);
        $('#<%=hdnRevenueSharingIDBarang.ClientID %>').val(RevenueSharingID);
        $('#<%=txtChargedQuantityBarang.ClientID %>').val(ChargedQuantity);
        $('#<%=txtBarangDiscTotalCtl.ClientID %>').val(discount).trigger('changeValue');
        $('#<%=txtBarangDiscComp1Ctl.ClientID %>').val(discountComp1).trigger('changeValue');
        $('#<%=txtBarangDiscComp2Ctl.ClientID %>').val(discountComp2).trigger('changeValue');
        $('#<%=txtBarangDiscComp3Ctl.ClientID %>').val(discountComp3).trigger('changeValue');
        $('#<%=hdnTariffAmount1CtlBarang.ClientID %>').val(tariffComp1);
        $('#<%=hdnTariffAmount2CtlBarang.ClientID %>').val(tariffComp2);
        $('#<%=hdnTariffAmount3CtlBarang.ClientID %>').val(tariffComp3);
        $('#<%=txtBarangDiscTariffComp1Ctl.ClientID %>').val(tariffComp1);
        $('#<%=txtBarangDiscTariffComp2Ctl.ClientID %>').val(tariffComp2);
        $('#<%=txtBarangDiscTariffComp3Ctl.ClientID %>').val(tariffComp3);

        var comp1 = tariffComp1 - discountComp1;
        var comp2 = tariffComp2 - discountComp2;
        var comp3 = tariffComp3 - discountComp3;

        $('#<%=txtBarangAfterTariffComp1Ctl.ClientID %>').val(comp1.formatMoney(2, '.', ','));
        $('#<%=txtBarangAfterTariffComp2Ctl.ClientID %>').val(comp2.formatMoney(2, '.', ','));
        $('#<%=txtBarangAfterTariffComp3Ctl.ClientID %>').val(comp3.formatMoney(2, '.', ','));

        $('#<%=txtBarangDiscPercentComp1Ctl.ClientID %>').val(discountPercentageComp1).trigger('changeValue');
        $('#<%=txtBarangDiscPercentComp2Ctl.ClientID %>').val(discountPercentageComp2).trigger('changeValue');
        $('#<%=txtBarangDiscPercentComp3Ctl.ClientID %>').val(discountPercentageComp3).trigger('changeValue');

        if (isAllowDiscountTariffComp1 == 'True') {
            $('#<%=txtBarangDiscPercentComp1Ctl.ClientID %>').removeAttr('readonly');
            $('#<%=txtBarangDiscComp1Ctl.ClientID %>').removeAttr('readonly');
        }
        else {
            $('#<%=txtBarangDiscPercentComp1Ctl.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtBarangDiscComp1Ctl.ClientID %>').attr('readonly', 'readonly');
        }

        if (isAllowDiscountTariffComp2 == 'True') {
            $('#<%=txtBarangDiscPercentComp2Ctl.ClientID %>').removeAttr('readonly');
            $('#<%=txtBarangDiscComp2Ctl.ClientID %>').removeAttr('readonly');
        }
        else {
            $('#<%=txtBarangDiscPercentComp2Ctl.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtBarangDiscComp2Ctl.ClientID %>').attr('readonly', 'readonly');
        }

        if (isAllowDiscountTariffComp3 == 'True') {
            $('#<%=txtBarangDiscPercentComp3Ctl.ClientID %>').removeAttr('readonly');
            $('#<%=txtBarangDiscComp3Ctl.ClientID %>').removeAttr('readonly');
        }
        else {
            $('#<%=txtBarangDiscPercentComp3Ctl.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtBarangDiscComp3Ctl.ClientID %>').attr('readonly', 'readonly');
        }

        $('#containerPopupEntryDataBarang').show();
    });

    $('#btnEntryPopupCancelBarang').live('click', function () {
        $('#containerPopupEntryDataBarang').hide();
    });

    $('#btnEntryPopupSaveBarang').click(function (evt) {
        if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup'))
            cbpEntryPopupViewBarang.PerformCallback('save');
        return false;
    });

    $('#<%=txtBarangDiscPercentComp1Ctl.ClientID %>').change(function () {
        $(this).blur();
        var totalPercentComp1 = parseFloat($(this).attr('hiddenVal'));
        var qty = parseFloat($('#<%=txtChargedQuantityBarang.ClientID %>').val().replace('.00', '').split(',').join(''));

        if (totalPercentComp1 > 100 || totalPercentComp1 < 0) {
            $('#<%=txtBarangDiscPercentComp1Ctl.ClientID %>').val(0);
            totalPercentComp1 = 0;
        }
        var tariffComp1ORI = parseFloat($('#<%=hdnTariffAmount1CtlBarang.ClientID %>').val().replace('.00', '').split(',').join(''));


        var discountComp1 = 0;
        if (totalPercentComp1 > 0) {
            discountComp1 = parseFloat(totalPercentComp1 * tariffComp1ORI / 100).toFixed(2);

            $('#<%=txtBarangDiscComp1Ctl.ClientID %>').val(discountComp1).trigger('changeValue');
            var tariffFinal = tariffComp1ORI - discountComp1;
            $('#<%=txtBarangAfterTariffComp1Ctl.ClientID %>').val(tariffFinal.formatMoney(2, '.', ',')).trigger('changeValue');
        }
        else {
            $('#<%=txtBarangDiscComp1Ctl.ClientID %>').val(0).trigger('changeValue');
            $('#<%=txtBarangAfterTariffComp1Ctl.ClientID %>').val(tariffComp1ORI.formatMoney(2, '.', ',')).trigger('changeValue');
        }

        var tempDiscountTotal = parseFloat($('#<%=txtBarangDiscComp1Ctl.ClientID %>').val().split(',').join(''))
                                        + parseFloat($('#<%=txtBarangDiscComp2Ctl.ClientID %>').val().split(',').join(''))
                                        + parseFloat($('#<%=txtBarangDiscComp3Ctl.ClientID %>').val().split(',').join(''));

        var discFinal = tempDiscountTotal * qty
        $('#<%=txtBarangDiscTotalCtl.ClientID %>').val(discFinal.formatMoney(2, '.', ','));
    });

    $('#<%=txtBarangDiscPercentComp2Ctl.ClientID %>').change(function () {
        $(this).blur();
        var totalPercentComp2 = parseFloat($(this).attr('hiddenVal'));
        var qty = parseFloat($('#<%=txtChargedQuantityBarang.ClientID %>').val().replace('.00', '').split(',').join(''));

        if (totalPercentComp2 > 100 || totalPercentComp2 < 0) {
            $('#<%=txtBarangDiscPercentComp2Ctl.ClientID %>').val(0);
            totalPercentComp2 = 0;
        }
        var tariffComp2ORI = parseFloat($('#<%=hdnTariffAmount2CtlBarang.ClientID %>').val().replace('.00', '').split(',').join(''));


        var discountComp2 = 0;
        if (totalPercentComp2 > 0) {
            discountComp2 = parseFloat(totalPercentComp2 * tariffComp2ORI / 100).toFixed(2);

            $('#<%=txtBarangDiscComp2Ctl.ClientID %>').val(discountComp2).trigger('changeValue');
            var tariffFinal = tariffComp2ORI - discountComp2;
            $('#<%=txtBarangAfterTariffComp2Ctl.ClientID %>').val(tariffFinal.formatMoney(2, '.', ',')).trigger('changeValue');
        }
        else {
            $('#<%=txtBarangDiscComp2Ctl.ClientID %>').val(0).trigger('changeValue');
            $('#<%=txtBarangAfterTariffComp2Ctl.ClientID %>').val(tariffComp2ORI.formatMoney(2, '.', ',')).trigger('changeValue');
        }

        var tempDiscountTotal = parseFloat($('#<%=txtBarangDiscComp1Ctl.ClientID %>').val().split(',').join(''))
                                        + parseFloat($('#<%=txtBarangDiscComp2Ctl.ClientID %>').val().split(',').join(''))
                                        + parseFloat($('#<%=txtBarangDiscComp3Ctl.ClientID %>').val().split(',').join(''));

        var discFinal = tempDiscountTotal * qty
        $('#<%=txtBarangDiscTotalCtl.ClientID %>').val(discFinal.formatMoney(2, '.', ','));
    });

    $('#<%=txtBarangDiscPercentComp3Ctl.ClientID %>').change(function () {
        $(this).blur();
        var totalPercentComp3 = parseFloat($(this).attr('hiddenVal'));
        var qty = parseFloat($('#<%=txtChargedQuantityBarang.ClientID %>').val().replace('.00', '').split(',').join(''));

        if (totalPercentComp3 > 100 || totalPercentComp3 < 0) {
            $('#<%=txtBarangDiscPercentComp3Ctl.ClientID %>').val(0);
            totalPercentComp3 = 0;
        }
        var tariffComp3ORI = parseFloat($('#<%=hdnTariffAmount3CtlBarang.ClientID %>').val().replace('.00', '').split(',').join(''));


        var discountComp3 = 0;
        if (totalPercentComp3 > 0) {
            discountComp3 = parseFloat(totalPercentComp3 * tariffComp3ORI / 100).toFixed(2);

            $('#<%=txtBarangDiscComp3Ctl.ClientID %>').val(discountComp3).trigger('changeValue');
            var tariffFinal = tariffComp3ORI - discountComp3;
            $('#<%=txtBarangAfterTariffComp3Ctl.ClientID %>').val(tariffFinal.formatMoney(2, '.', ',')).trigger('changeValue');
        }
        else {
            $('#<%=txtBarangDiscComp3Ctl.ClientID %>').val(0).trigger('changeValue');
            $('#<%=txtBarangAfterTariffComp3Ctl.ClientID %>').val(tariffComp3ORI.formatMoney(2, '.', ',')).trigger('changeValue');
        }

        var tempDiscountTotal = parseFloat($('#<%=txtBarangDiscComp1Ctl.ClientID %>').val().split(',').join(''))
                                        + parseFloat($('#<%=txtBarangDiscComp2Ctl.ClientID %>').val().split(',').join(''))
                                        + parseFloat($('#<%=txtBarangDiscComp3Ctl.ClientID %>').val().split(',').join(''));

        var discFinal = tempDiscountTotal * qty
        $('#<%=txtBarangDiscTotalCtl.ClientID %>').val(discFinal.formatMoney(2, '.', ','));
    });

    $('#<%=txtBarangDiscComp1Ctl.ClientID %>').change(function () {
        $('#<%=txtBarangDiscPercentComp1Ctl.ClientID %>').val(0).trigger('changeValue');

        var disc1 = parseFloat(checkMinusDecimalOK($('#<%=txtBarangDiscComp1Ctl.ClientID %>').val()));
        var disc2 = parseFloat(checkMinusDecimalOK($('#<%=txtBarangDiscComp2Ctl.ClientID %>').val()));
        var disc3 = parseFloat(checkMinusDecimalOK($('#<%=txtBarangDiscComp3Ctl.ClientID %>').val()));

        var tariffComp1 = parseFloat($('#<%=hdnTariffAmount1CtlBarang.ClientID %>').val().replace('.00', '').split(',').join(''));
        var tariffComp2 = parseFloat($('#<%=hdnTariffAmount2CtlBarang.ClientID %>').val().replace('.00', '').split(',').join(''));
        var tariffComp3 = parseFloat($('#<%=hdnTariffAmount3CtlBarang.ClientID %>').val().replace('.00', '').split(',').join(''));

        var qty = parseFloat($('#<%=txtChargedQuantityBarang.ClientID %>').val().replace('.00', '').split(',').join(''));

        if (disc1 > tariffComp1) {
            disc1 = tariffComp1;
            $('#<%=txtBarangDiscComp1Ctl.ClientID %>').val(tariffComp1);
        }
        else {
            $('#<%=txtBarangDiscComp1Ctl.ClientID %>').val(disc1);
        }

        var totalDisc = (disc1 + disc2 + disc3) * qty;

        var tariffFinal = tariffComp1 - disc1;
        $('#<%=txtBarangAfterTariffComp1Ctl.ClientID %>').val(tariffFinal.formatMoney(2, '.', ',')).trigger('changeValue');
        $('#<%=txtBarangDiscTotalCtl.ClientID %>').val(totalDisc.formatMoney(2, '.', ','));
    });

    $('#<%=txtBarangDiscComp2Ctl.ClientID %>').change(function () {
        $('#<%=txtBarangDiscPercentComp2Ctl.ClientID %>').val(0).trigger('changeValue');

        var disc1 = parseFloat(checkMinusDecimalOK($('#<%=txtBarangDiscComp1Ctl.ClientID %>').val()));
        var disc2 = parseFloat(checkMinusDecimalOK($('#<%=txtBarangDiscComp2Ctl.ClientID %>').val()));
        var disc3 = parseFloat(checkMinusDecimalOK($('#<%=txtBarangDiscComp3Ctl.ClientID %>').val()));

        var tariffComp1 = parseFloat($('#<%=hdnTariffAmount1CtlBarang.ClientID %>').val().replace('.00', '').split(',').join(''));
        var tariffComp2 = parseFloat($('#<%=hdnTariffAmount2CtlBarang.ClientID %>').val().replace('.00', '').split(',').join(''));
        var tariffComp3 = parseFloat($('#<%=hdnTariffAmount3CtlBarang.ClientID %>').val().replace('.00', '').split(',').join(''));

        var qty = parseFloat($('#<%=txtChargedQuantityBarang.ClientID %>').val().replace('.00', '').split(',').join(''));

        if (disc2 > tariffComp2) {
            disc2 = tariffComp2;
            $('#<%=txtBarangDiscComp2Ctl.ClientID %>').val(tariffComp2);
        }
        else {
            $('#<%=txtBarangDiscComp2Ctl.ClientID %>').val(disc2);
        }

        var totalDisc = (disc1 + disc2 + disc3) * qty;

        var tariffFinal = tariffComp2 - disc2;
        $('#<%=txtBarangAfterTariffComp2Ctl.ClientID %>').val(tariffFinal.formatMoney(2, '.', ',')).trigger('changeValue');
        $('#<%=txtBarangDiscTotalCtl.ClientID %>').val(totalDisc.formatMoney(2, '.', ','));
    });

    $('#<%=txtBarangDiscComp3Ctl.ClientID %>').change(function () {
        $('#<%=txtBarangDiscPercentComp3Ctl.ClientID %>').val(0).trigger('changeValue');

        var disc1 = parseFloat(checkMinusDecimalOK($('#<%=txtBarangDiscComp1Ctl.ClientID %>').val()));
        var disc2 = parseFloat(checkMinusDecimalOK($('#<%=txtBarangDiscComp2Ctl.ClientID %>').val()));
        var disc3 = parseFloat(checkMinusDecimalOK($('#<%=txtBarangDiscComp3Ctl.ClientID %>').val()));

        var tariffComp1 = parseFloat($('#<%=hdnTariffAmount1CtlBarang.ClientID %>').val().replace('.00', '').split(',').join(''));
        var tariffComp2 = parseFloat($('#<%=hdnTariffAmount2CtlBarang.ClientID %>').val().replace('.00', '').split(',').join(''));
        var tariffComp3 = parseFloat($('#<%=hdnTariffAmount3CtlBarang.ClientID %>').val().replace('.00', '').split(',').join(''));

        var qty = parseFloat($('#<%=txtChargedQuantityBarang.ClientID %>').val().replace('.00', '').split(',').join(''));

        if (disc3 > tariffComp3) {
            disc3 = tariffComp3;
            $('#<%=txtBarangDiscComp3Ctl.ClientID %>').val(tariffComp3);
        }
        else {
            $('#<%=txtBarangDiscComp3Ctl.ClientID %>').val(disc3);
        }

        var totalDisc = (disc1 + disc2 + disc3) * qty;

        var tariffFinal = tariffComp3 - disc3;
        $('#<%=txtBarangAfterTariffComp3Ctl.ClientID %>').val(tariffFinal.formatMoney(2, '.', ',')).trigger('changeValue');
        $('#<%=txtBarangDiscTotalCtl.ClientID %>').val(totalDisc.formatMoney(2, '.', ','));
    });

    $('#<%=txtChargedQuantityBarang.ClientID %>').change(function () {
        var qty = parseFloat(checkMinusDecimalOK($('#<%=txtChargedQuantityBarang.ClientID %>').val()));
        $('#<%=txtChargedQuantityBarang.ClientID %>').val(qty);

        var disc1 = parseFloat(checkMinusDecimalOK($('#<%=txtBarangDiscComp1Ctl.ClientID %>').val()));
        var disc2 = parseFloat(checkMinusDecimalOK($('#<%=txtBarangDiscComp2Ctl.ClientID %>').val()));
        var disc3 = parseFloat(checkMinusDecimalOK($('#<%=txtBarangDiscComp3Ctl.ClientID %>').val()));

        var totalDisc = (disc1 + disc2 + disc3) * qty;
        $('#<%=txtBarangDiscTotalCtl.ClientID %>').val(totalDisc.formatMoney(2, '.', ','));
    });

    //#region Paramedic
    $('#lblParamedicDetailBarang.lblLink').click(function () {
        openSearchDialog('paramedic', "IsDeleted = 0", function (value) {
            $('#<%=txtParamedicCodeBarang.ClientID %>').val(value);
            onTxtParamedicCodeBarangChanged(value);
        });
    });

    $('#<%=txtParamedicCodeBarang.ClientID %>').change(function () {
        onTxtParamedicCodeBarangChanged($(this).val());
    });

    function onTxtParamedicCodeBarangChanged(value) {
        var filterExpression = "ParamedicCode = '" + value + "'";
        Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnParamedicIDBarang.ClientID %>').val(result.ParamedicID);
                $('#<%=txtParamedicNameBarang.ClientID %>').val(result.ParamedicName);

                var today = new Date();
                var date = today.getDate() + '-' + (today.getMonth() + 1) + '-' + today.getFullYear();
                var time = today.getHours() + ":" + today.getMinutes();

                var hdnTransactionDate = $('#<%=hdnTransactionDate.ClientID %>').val() == "" ? date : $('#<%=hdnTransactionDate.ClientID %>').val();
                var hdnTransactionTime = $('#<%=hdnTransactionTime.ClientID %>').val() == "" ? time : $('#<%=hdnTransactionTime.ClientID %>').val();
                var hdnChargesHealthcareServiceUnitID = $('#<%=hdnChargesHealthcareServiceUnitID.ClientID %>').val();

                var chargesClassID = $('#<%=hdnChargesClassID.ClientID %>').val();
                var itemCode = $('#<%=txtDetailItemCodeBarang.ClientID %>').val();
                var paramedicRole = "X084^001"; // Constant.ParamedicRole.PELAKSANA
                if (itemCode != '') {
                    Methods.getItemRevenueSharing(itemCode, result.ParamedicID, chargesClassID, paramedicRole, $('#<%=hdnVisitIDCtl.ClientID %>').val(), hdnChargesHealthcareServiceUnitID, hdnTransactionDate, hdnTransactionTime, function (result) {
                        if (result != null)
                            $('#<%=hdnRevenueSharingIDBarang.ClientID %>').val(result.RevenueSharingID);
                        else
                            $('#<%=hdnRevenueSharingIDBarang.ClientID %>').val('');
                    });
                }
            }
            else {
                $('#<%=hdnParamedicIDBarang.ClientID %>').val('');
                $('#<%=txtParamedicCodeBarang.ClientID %>').val('');
                $('#<%=txtParamedicNameBarang.ClientID %>').val('');
                $('#<%=hdnRevenueSharingIDBarang.ClientID %>').val('');
            }
        });
    }
    //#endregion

    function onCbpEntryPopupViewBarangEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else {
                $('#containerPopupEntryDataBarang').hide();
                cbpService.PerformCallback();
            }
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail')
                showToast('Delete Failed', 'Error Message : ' + param[2]);
        }
        $('#containerImgLoadingViewPopupBarang').hide();
    }
    //#endregion
</script>
<div style="height: 450px; overflow-y: auto; overflow-x: hidden">
    <input type="hidden" id="hdnChargesHealthcareServiceUnitID" value="" runat="server" />
    <input type="hidden" id="hdnTransactionDate" value="" runat="server" />
    <input type="hidden" id="hdnTransactionTime" value="" runat="server" />
    <input type="hidden" id="hdnChargesDtID" runat="server" value="" />
    <input type="hidden" id="hdnItemID" runat="server" value="" />
    <input type="hidden" id="hdnIsUsingAccumulatedPrice" runat="server" value="" />
    <input type="hidden" id="hdnChargesClassID" runat="server" value="" />
    <input type="hidden" id="hdnPatientChargesDtID" runat="server" value="" />
    <input type="hidden" id="hdnVisitIDCtl" runat="server" value="" />
    <input type="hidden" id="hdnRegistrationIDCtl" runat="server" value="" />
    <input type="hidden" id="hdnTariffComp1TextCtl" runat="server" value="" />
    <input type="hidden" id="hdnTariffComp2TextCtl" runat="server" value="" />
    <input type="hidden" id="hdnTariffComp3TextCtl" runat="server" value="" />
    <input type="hidden" id="hdnTariffAmount1Ctl" runat="server" value="" />
    <input type="hidden" id="hdnTariffAmount2Ctl" runat="server" value="" />
    <input type="hidden" id="hdnTariffAmount3Ctl" runat="server" value="" />
    <input type="hidden" id="hdnTariffComp1TextCtlObat" runat="server" value="" />
    <input type="hidden" id="hdnTariffComp2TextCtlObat" runat="server" value="" />
    <input type="hidden" id="hdnTariffComp3TextCtlObat" runat="server" value="" />
    <input type="hidden" id="hdnTariffAmount1CtlObat" runat="server" value="" />
    <input type="hidden" id="hdnTariffAmount2CtlObat" runat="server" value="" />
    <input type="hidden" id="hdnTariffAmount3CtlObat" runat="server" value="" />
    <input type="hidden" id="hdnTariffComp1TextCtlBarang" runat="server" value="" />
    <input type="hidden" id="hdnTariffComp2TextCtlBarang" runat="server" value="" />
    <input type="hidden" id="hdnTariffComp3TextCtlBarang" runat="server" value="" />
    <input type="hidden" id="hdnTariffAmount1CtlBarang" runat="server" value="" />
    <input type="hidden" id="hdnTariffAmount2CtlBarang" runat="server" value="" />
    <input type="hidden" id="hdnTariffAmount3CtlBarang" runat="server" value="" />
    <input type="hidden" id="hdnIsEndingAmountRoundingTo1" runat="server" value="0" />
    <div>
        <table class="tblEntryContent" style="width: 90%">
            <colgroup>
                <col style="width: 120px" />
                <col style="width: 400px" />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Paket Pelayanan")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtItemServiceName" ReadOnly="true" Width="100%" runat="server" />
                </td>
            </tr>
        </table>
        <fieldset id="fsEntryPopupAll" style="margin: 0">
            <table id="tblDiscountAll" runat="server">
                <tr>
                    <td class="blink-alert" colspan="2">
                        <table id="tblinfo" runat="server">
                            <tr>
                                <td>
                                    <label id="Label1" class="lblNormal" runat="server">
                                        <%=GetLabel("Kolom Discount berikut diperuntukkan untuk item-item dibawah ini")%></label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="tdLabel">
                        <label class="lblNormal">
                            <%=GetLabel("Discount (%) ")%><%=GetTariffComponent1Text()%></label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtServiceDiscPercentComp1CtlAll" runat="server" CssClass="txtCurrency txtServiceDiscPercentComp1CtlAll" />
                    </td>
                </tr>
                <tr>
                    <td class="tdLabel">
                        <label class="lblNormal">
                            <%=GetLabel("Discount (%) ")%><%=GetTariffComponent2Text()%></label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtServiceDiscPercentComp2CtlAll" runat="server" CssClass="txtCurrency txtServiceDiscPercentComp2CtlAll" />
                    </td>
                </tr>
                <tr>
                    <td class="tdLabel">
                        <label class="lblNormal">
                            <%=GetLabel("Discount (%) ")%><%=GetTariffComponent3Text()%></label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtServiceDiscPercentComp3CtlAll" runat="server" CssClass="txtCurrency txtServiceDiscPercentComp3CtlAll" />
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <input type="button" id="btnEntryPopupSaveAll" value='<%= GetLabel("SAVE")%>' class="btnEntryPopupSaveAll"
                            style="width: 50px;" />
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
    <div class="containerUlTabPage">
        <ul class="ulTabPage" id="ulTabLabResult">
            <li class="selected" contentid="containerServiceCtl">
                <%=GetLabel("PELAYANAN") %></li>
            <li contentid="containerDrugMSCtl">
                <%=GetLabel("OBAT & ALKES") %></li>
            <li contentid="containerLogisticsCtl">
                <%=GetLabel("BARANG UMUM") %></li>
        </ul>
    </div>
    <div id="containerServiceCtl" class="containerTransDtCtl">
        <table class="tblContentArea">
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <div id="containerPopupEntryData" style="margin-top: 10px; display: none;">
                        <input type="hidden" id="hdnID" runat="server" value="" />
                        <div class="pageTitle">
                            <%=GetLabel("Entry")%></div>
                        <fieldset id="fsEntryPopup" style="margin: 0">
                            <table class="tblEntryDetail" style="width: 100%">
                                <colgroup>
                                    <col style="width: 200px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal" id="lblItem">
                                            <%=GetLabel("Detail Paket")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" value="" id="hdnDetailItemID" runat="server" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 120px" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtDetailItemCode" ReadOnly="true" Width="100%" runat="server" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDetailItemName1" ReadOnly="true" Width="80%" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblLink lblMandatory" id="lblParamedicDetail">
                                            <%=GetLabel("Dokter/Tenaga Medis")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" value="" id="hdnParamedicID" runat="server" />
                                        <input type="hidden" value="" id="hdnRevenueSharingID" runat="server" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 120px" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtParamedicCode" CssClass="required" Width="100%" runat="server" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtParamedicName" ReadOnly="true" CssClass="required" Width="80%"
                                                        runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblMandatory">
                                            <%=GetLabel("Jumlah")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtChargedQuantity" CssClass="number" Min="0" Width="120px" runat="server" />
                                    </td>
                                </tr>
                                <tr style="display: none">
                                    <td class="tdLabel">
                                        <label class="lblMandatory">
                                            <%=GetLabel("Tariff")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTariff" CssClass="number" Min="0" Width="120px" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <table>
                                            <colgroup>
                                                <col style="width: 450px" />
                                            </colgroup>
                                            <tr>
                                                <td valign="top">
                                                    <table>
                                                        <colgroup>
                                                            <col style="width: 100px" />
                                                            <col style="width: 150px" />
                                                            <col style="width: 150px" />
                                                            <col style="width: 150px" />
                                                        </colgroup>
                                                        <tr>
                                                            <td class="tdLabel" colspan="2">
                                                                <label class="lblNormal">
                                                                    <%=GetLabel("Total Discount per Unit x Qty")%></label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtServiceDiscTotalCtl" ReadOnly="true" runat="server" Width="100%"
                                                                    CssClass="txtCurrency txtServiceDiscTotalCtl" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="tdLabel" colspan="2">
                                                                <label class="lblNormal">
                                                                    <%=GetLabel("Discount (%) ")%><%=GetTariffComponent1Text()%></label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtServiceDiscPercentComp1Ctl" ReadOnly="true" runat="server" Width="100%"
                                                                    CssClass="txtCurrency txtServiceDiscPercentComp1Ctl" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="tdLabel" colspan="2">
                                                                <label class="lblNormal">
                                                                    <%=GetLabel("Discount (%) ")%><%=GetTariffComponent2Text()%></label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtServiceDiscPercentComp2Ctl" ReadOnly="true" runat="server" Width="100%"
                                                                    CssClass="txtCurrency txtServiceDiscPercentComp2Ctl" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="tdLabel" colspan="2">
                                                                <label class="lblNormal">
                                                                    <%=GetLabel("Discount (%) ")%><%=GetTariffComponent3Text()%></label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtServiceDiscPercentComp3Ctl" ReadOnly="true" runat="server" Width="100%"
                                                                    CssClass="txtCurrency txtServiceDiscPercentComp3Ctl" />
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
                                                                <input id="txtServiceDiscTariffComp1Ctl" class="txtCurrency txtServiceDiscTariffComp1Ctl txtUnitTariffPrevCtl"
                                                                    readonly="readonly" style="width: 100%" runat="server" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtServiceDiscComp1Ctl" CssClass="txtCurrency txtServiceDiscComp1Ctl txtDiscComp"
                                                                    Width="100%" runat="server" />
                                                            </td>
                                                            <td>
                                                                <input id="txtServiceAfterTariffComp1Ctl" class="txtCurrency txtServiceAfterTariffComp1Ctl txtUnitTariffAfterCtl"
                                                                    readonly="readonly" style="width: 100%" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="tdLabel">
                                                                <label class="lblNormal">
                                                                    <%=GetTariffComponent2Text()%></label>
                                                            </td>
                                                            <td>
                                                                <input id="txtServiceDiscTariffComp2Ctl" class="txtCurrency txtServiceDiscTariffComp2Ctl txtUnitTariffPrevCtl"
                                                                    readonly="readonly" style="width: 100%" runat="server" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtServiceDiscComp2Ctl" CssClass="txtCurrency txtServiceDiscComp2Ctl txtDiscComp"
                                                                    Width="100%" runat="server" />
                                                            </td>
                                                            <td>
                                                                <input id="txtServiceAfterTariffComp2Ctl" class="txtCurrency txtServiceAfterTariffComp2Ctl txtUnitTariffAfterCtl"
                                                                    readonly="readonly" style="width: 100%" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="tdLabel">
                                                                <label class="lblNormal">
                                                                    <%=GetTariffComponent3Text()%></label>
                                                            </td>
                                                            <td>
                                                                <input id="txtServiceDiscTariffComp3Ctl" class="txtCurrency txtServiceDiscTariffComp3Ctl txtUnitTariffPrevCtl"
                                                                    readonly="readonly" style="width: 100%" runat="server" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtServiceDiscComp3Ctl" CssClass="txtCurrency txtServiceDiscComp3Ctl txtServiceDiscComp3Ctl txtDiscComp"
                                                                    Width="100%" runat="server" />
                                                            </td>
                                                            <td>
                                                                <input id="txtServiceAfterTariffComp3Ctl" class="txtCurrency txtServiceAfterTariffComp3Ctl txtUnitTariffAfterCtl"
                                                                    readonly="readonly" style="width: 100%" runat="server" />
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
                                        <center>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <input type="button" id="btnEntryPopupSave" value='<%= GetLabel("SAVE")%>' class="btnEntryPopupSave w3-btn w3-hover-blue"
                                                            style="width: 80px;" />
                                                    </td>
                                                    <td>
                                                        <input type="button" id="btnEntryPopupCancel" value='<%= GetLabel("CANCEL")%>' class="btnEntryPopupCancel w3-btn w3-hover-blue"
                                                            style="width: 80px;" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>
                    <div>
                        <dxcp:ASPxCallbackPanel ID="cbpEntryPopupView" runat="server" Width="100%" ClientInstanceName="cbpEntryPopupView"
                            ShowLoadingPanel="false" OnCallback="cbpEntryPopupView_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewPopup').show(); }"
                                EndCallback="function(s,e){ onCbpEntryPopupViewEndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto;
                                        margin-right: auto; position: relative; font-size: 0.95em;">
                                        <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                            OnRowDataBound="grdView_RowDataBound" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                            <Columns>
                                                <asp:TemplateField HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <img class="imgEdit imgLink" <%#: Eval("IsUsingAccumulatedPrice").ToString() == "True" ?  "" : "style='display:none'" %>
                                                            src='<%# Eval("IsUsingAccumulatedPrice").ToString() == "True" ? ResolveUrl("~/Libs/Images/Button/edit.png") : ""%>'
                                                            alt="" style="float: left; margin-left: 7px" />
                                                        <img class="imgDelete imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                            alt="" style="display: none" />
                                                        <input type="hidden" class="ID" value="<%#: Eval("ID")%>" />
                                                        <input type="hidden" class="ItemID" value="<%#: Eval("ItemID")%>" />
                                                        <input type="hidden" class="ItemCode" value="<%#: Eval("ItemCode")%>" />
                                                        <input type="hidden" class="ItemName1" value="<%#: Eval("ItemName1")%>" />
                                                        <input type="hidden" class="ParamedicID" value="<%#: Eval("ParamedicID")%>" />
                                                        <input type="hidden" class="ParamedicCode" value="<%#: Eval("ParamedicCode")%>" />
                                                        <input type="hidden" class="ParamedicName" value="<%#: Eval("ParamedicName")%>" />
                                                        <input type="hidden" class="RevenueSharingID" value="<%#: Eval("RevenueSharingID")%>" />
                                                        <input type="hidden" class="RevenueSharingName" value="<%#: Eval("RevenueSharingCode")%>" />
                                                        <input type="hidden" class="ChargedQuantity" value="<%#: Eval("ChargedQuantity")%>" />
                                                        <input type="hidden" class="BaseTariff" value="<%#: Eval("BaseTariff")%>" />
                                                        <input type="hidden" class="BaseComp1" value="<%#: Eval("BaseComp1")%>" />
                                                        <input type="hidden" class="BaseComp2" value="<%#: Eval("BaseComp2")%>" />
                                                        <input type="hidden" class="BaseComp3" value="<%#: Eval("BaseComp3")%>" />
                                                        <input type="hidden" class="Tariff" value="<%#: Eval("Tariff")%>" />
                                                        <input type="hidden" class="TariffComp1" value="<%#: Eval("TariffComp1")%>" />
                                                        <input type="hidden" class="TariffComp2" value="<%#: Eval("TariffComp2")%>" />
                                                        <input type="hidden" class="TariffComp3" value="<%#: Eval("TariffComp3")%>" />
                                                        <input type="hidden" class="Discount" value="<%#: Eval("DiscountAmount")%>" />
                                                        <input type="hidden" class="DiscountComp1" value="<%#: Eval("DiscountComp1")%>" />
                                                        <input type="hidden" class="DiscountComp2" value="<%#: Eval("DiscountComp2")%>" />
                                                        <input type="hidden" class="DiscountComp3" value="<%#: Eval("DiscountComp3")%>" />
                                                        <input type="hidden" class="DiscountPercentageComp1" value="<%#: Eval("DiscountPercentageComp1")%>" />
                                                        <input type="hidden" class="DiscountPercentageComp2" value="<%#: Eval("DiscountPercentageComp2")%>" />
                                                        <input type="hidden" class="DiscountPercentageComp3" value="<%#: Eval("DiscountPercentageComp3")%>" />
                                                        <input type="hidden" class="IsAllowDiscount" value="<%#: Eval("IsAllowDiscount")%>" />
                                                        <input type="hidden" class="IsAllowDiscountTariffComp1" value="<%#: Eval("IsAllowDiscountTariffComp1")%>" />
                                                        <input type="hidden" class="IsAllowDiscountTariffComp2" value="<%#: Eval("IsAllowDiscountTariffComp2")%>" />
                                                        <input type="hidden" class="IsAllowDiscountTariffComp3" value="<%#: Eval("IsAllowDiscountTariffComp3")%>" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <%=GetLabel("Pelayanan")%>
                                                        <br />
                                                        <%=GetLabel("Dokter/Tenaga Medis")%>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <%#: Eval("ItemName1")%>
                                                        <br />
                                                        <span style="font-size: small; font-style: oblique">
                                                            <%#: Eval("ParamedicName")%>
                                                        </span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Quantity" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                    HeaderStyle-Width="80px">
                                                    <ItemTemplate>
                                                        <%#: Eval("ChargedQuantity", "{0:N}")%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Sarana" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                    HeaderStyle-Width="80px">
                                                    <ItemTemplate>
                                                        <%#: Eval("TariffComp1", "{0:N}")%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Pelayanan" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                    HeaderStyle-Width="80px">
                                                    <ItemTemplate>
                                                        <%#: Eval("TariffComp2", "{0:N}")%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Lain-lain" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                    HeaderStyle-Width="80px">
                                                    <ItemTemplate>
                                                        <%#: Eval("TariffComp3", "{0:N}")%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Discount" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                    HeaderStyle-Width="80px">
                                                    <ItemTemplate>
                                                        <%#: Eval("DiscountAmount", "{0:N}")%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Total" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                    HeaderStyle-Width="80px">
                                                    <ItemTemplate>
                                                        <%#: Eval("cfLineAmount", "{0:N}")%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <%=GetLabel("Data Tidak Tersedia")%>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                        <div class="imgLoadingGrdView" id="containerImgLoadingViewPopup">
                                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                        </div>
                                    </asp:Panel>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div id="containerDrugMSCtl" style="display: none" class="containerTransDtCtl">
        <table class="tblContentArea">
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <div id="containerPopupEntryDataObat" style="margin-top: 10px; display: none;">
                        <input type="hidden" id="hdnIDObat" runat="server" value="" />
                        <div class="pageTitle">
                            <%=GetLabel("Entry")%></div>
                        <fieldset id="fsEntryPopupObat" style="margin: 0">
                            <table class="tblEntryDetail" style="width: 100%">
                                <colgroup>
                                    <col style="width: 150px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal" id="lblItemObat">
                                            <%=GetLabel("Detail Paket")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" value="" id="hdnDetailItemIDObat" runat="server" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 120px" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtDetailItemCodeObat" ReadOnly="true" Width="100%" runat="server" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDetailItemName1Obat" ReadOnly="true" Width="80%" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblLink lblMandatory" id="lblParamedicDetailObat">
                                            <%=GetLabel("Dokter/Tenaga Medis")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" value="" id="hdnParamedicIDObat" runat="server" />
                                        <input type="hidden" value="" id="hdnRevenueSharingIDObat" runat="server" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 120px" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtParamedicCodeObat" CssClass="required" Width="100%" runat="server" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtParamedicNameObat" ReadOnly="true" CssClass="required" Width="80%"
                                                        runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblMandatory">
                                            <%=GetLabel("Jumlah")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtChargedQuantityObat" CssClass="number" Min="0" Width="120px"
                                            runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <table>
                                            <colgroup>
                                                <col style="width: 450px" />
                                            </colgroup>
                                            <tr>
                                                <td valign="top">
                                                    <table>
                                                        <colgroup>
                                                            <col style="width: 100px" />
                                                            <col style="width: 150px" />
                                                            <col style="width: 150px" />
                                                            <col style="width: 150px" />
                                                        </colgroup>
                                                        <tr>
                                                            <td class="tdLabel" colspan="2">
                                                                <label class="lblNormal">
                                                                    <%=GetLabel("Total Discount per Unit x Qty")%></label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtObatDiscTotalCtl" ReadOnly="true" runat="server" Width="100%"
                                                                    CssClass="txtCurrency txtObatDiscTotalCtl" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="tdLabel" colspan="2">
                                                                <label class="lblNormal">
                                                                    <%=GetLabel("Discount (%) ")%><%=GetTariffComponent1Text()%></label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtObatDiscPercentComp1Ctl" ReadOnly="true" runat="server" Width="100%"
                                                                    CssClass="txtCurrency txtObatDiscPercentComp1Ctl" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="tdLabel" colspan="2">
                                                                <label class="lblNormal">
                                                                    <%=GetLabel("Discount (%) ")%><%=GetTariffComponent2Text()%></label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtObatDiscPercentComp2Ctl" ReadOnly="true" runat="server" Width="100%"
                                                                    CssClass="txtCurrency txtObatDiscPercentComp2Ctl" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="tdLabel" colspan="2">
                                                                <label class="lblNormal">
                                                                    <%=GetLabel("Discount (%) ")%><%=GetTariffComponent3Text()%></label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtObatDiscPercentComp3Ctl" ReadOnly="true" runat="server" Width="100%"
                                                                    CssClass="txtCurrency txtObatDiscPercentComp3Ctl" />
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
                                                                <input id="txtObatDiscTariffComp1Ctl" class="txtCurrency txtObatDiscTariffComp1Ctl txtUnitTariffPrevCtl"
                                                                    readonly="readonly" style="width: 100%" runat="server" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtObatDiscComp1Ctl" CssClass="txtCurrency txtObatDiscComp1Ctl txtDiscComp"
                                                                    Width="100%" runat="server" />
                                                            </td>
                                                            <td>
                                                                <input id="txtObatAfterTariffComp1Ctl" class="txtCurrency txtObatAfterTariffComp1Ctl txtUnitTariffAfterCtl"
                                                                    readonly="readonly" style="width: 100%" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="tdLabel">
                                                                <label class="lblNormal">
                                                                    <%=GetTariffComponent2Text()%></label>
                                                            </td>
                                                            <td>
                                                                <input id="txtObatDiscTariffComp2Ctl" class="txtCurrency txtObatDiscTariffComp2Ctl txtUnitTariffPrevCtl"
                                                                    readonly="readonly" style="width: 100%" runat="server" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtObatDiscComp2Ctl" CssClass="txtCurrency txtObatDiscComp2Ctl txtDiscComp"
                                                                    Width="100%" runat="server" />
                                                            </td>
                                                            <td>
                                                                <input id="txtObatAfterTariffComp2Ctl" class="txtCurrency txtObatAfterTariffComp2Ctl txtUnitTariffAfterCtl"
                                                                    readonly="readonly" style="width: 100%" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="tdLabel">
                                                                <label class="lblNormal">
                                                                    <%=GetTariffComponent3Text()%></label>
                                                            </td>
                                                            <td>
                                                                <input id="txtObatDiscTariffComp3Ctl" class="txtCurrency txtObatDiscTariffComp3Ctl txtUnitTariffPrevCtl"
                                                                    readonly="readonly" style="width: 100%" runat="server" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtObatDiscComp3Ctl" CssClass="txtCurrency txtObatDiscComp3Ctl txtServiceDiscComp3Ctl txtDiscComp"
                                                                    Width="100%" runat="server" />
                                                            </td>
                                                            <td>
                                                                <input id="txtObatAfterTariffComp3Ctl" class="txtCurrency txtObatAfterTariffComp3Ctl txtUnitTariffAfterCtl"
                                                                    readonly="readonly" style="width: 100%" runat="server" />
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
                                        <center>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <input type="button" id="btnEntryPopupSaveObat" value='<%= GetLabel("SAVE")%>' class="btnEntryPopupSaveObat w3-btn w3-hover-blue"
                                                            style="width: 80px;" />
                                                    </td>
                                                    <td>
                                                        <input type="button" id="btnEntryPopupCancelObat" value='<%= GetLabel("CANCEL")%>'
                                                            class="btnEntryPopupCancelObat w3-btn w3-hover-blue" style="width: 80px;" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>
                    <dxcp:ASPxCallbackPanel ID="cbpEntryPopupViewObat" runat="server" Width="100%" ClientInstanceName="cbpEntryPopupViewObat"
                        ShowLoadingPanel="false" OnCallback="cbpEntryPopupViewObat_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewPopupObat').show(); }"
                            EndCallback="function(s,e){ onCbpEntryPopupViewObatEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent2" runat="server">
                                <asp:Panel runat="server" ID="pnlEntryPopupgrdViewObat" Style="width: 100%; margin-left: auto;
                                    margin-right: auto; position: relative; font-size: 0.95em;">
                                    <asp:GridView ID="grdViewObat" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                        OnRowDataBound="grdViewObat_RowDataBound" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <img class="imgEditObat imgLink" <%#: Eval("IsUsingAccumulatedPrice").ToString() == "True" ?  "" : "style='display:none'" %>
                                                        src='<%# Eval("IsUsingAccumulatedPrice").ToString() == "True" ? ResolveUrl("~/Libs/Images/Button/edit.png") : ""%>'
                                                        alt="" style="float: left; margin-left: 7px" />
                                                    <img class="imgDeleteObat imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                        alt="" style="display: none" />
                                                    <input type="hidden" class="IDObat" value="<%#: Eval("ID")%>" />
                                                    <input type="hidden" class="ItemIDObat" value="<%#: Eval("ItemID")%>" />
                                                    <input type="hidden" class="ItemCodeObat" value="<%#: Eval("ItemCode")%>" />
                                                    <input type="hidden" class="ItemName1Obat" value="<%#: Eval("ItemName1")%>" />
                                                    <input type="hidden" class="ParamedicIDObat" value="<%#: Eval("ParamedicID")%>" />
                                                    <input type="hidden" class="ParamedicCodeObat" value="<%#: Eval("ParamedicCode")%>" />
                                                    <input type="hidden" class="ParamedicNameObat" value="<%#: Eval("ParamedicName")%>" />
                                                    <input type="hidden" class="RevenueSharingIDObat" value="<%#: Eval("RevenueSharingID")%>" />
                                                    <input type="hidden" class="RevenueSharingNameObat" value="<%#: Eval("RevenueSharingCode")%>" />
                                                    <input type="hidden" class="ChargedQuantityObat" value="<%#: Eval("ChargedQuantity")%>" />
                                                    <input type="hidden" class="BaseTariffObat" value="<%#: Eval("BaseTariff")%>" />
                                                    <input type="hidden" class="BaseComp1Obat" value="<%#: Eval("BaseComp1")%>" />
                                                    <input type="hidden" class="BaseComp2Obat" value="<%#: Eval("BaseComp2")%>" />
                                                    <input type="hidden" class="BaseComp3Obat" value="<%#: Eval("BaseComp3")%>" />
                                                    <input type="hidden" class="TariffObat" value="<%#: Eval("Tariff")%>" />
                                                    <input type="hidden" class="TariffComp1Obat" value="<%#: Eval("TariffComp1")%>" />
                                                    <input type="hidden" class="TariffComp2Obat" value="<%#: Eval("TariffComp2")%>" />
                                                    <input type="hidden" class="TariffComp3Obat" value="<%#: Eval("TariffComp3")%>" />
                                                    <input type="hidden" class="DiscountObat" value="<%#: Eval("DiscountAmount")%>" />
                                                    <input type="hidden" class="DiscountComp1Obat" value="<%#: Eval("DiscountComp1")%>" />
                                                    <input type="hidden" class="DiscountComp2Obat" value="<%#: Eval("DiscountComp2")%>" />
                                                    <input type="hidden" class="DiscountComp3Obat" value="<%#: Eval("DiscountComp3")%>" />
                                                    <input type="hidden" class="DiscountPercentageComp1Obat" value="<%#: Eval("DiscountPercentageComp1")%>" />
                                                    <input type="hidden" class="DiscountPercentageComp2Obat" value="<%#: Eval("DiscountPercentageComp2")%>" />
                                                    <input type="hidden" class="DiscountPercentageComp3Obat" value="<%#: Eval("DiscountPercentageComp3")%>" />
                                                    <input type="hidden" class="IsAllowDiscountObat" value="<%#: Eval("IsAllowDiscount")%>" />
                                                    <input type="hidden" class="IsAllowDiscountTariffComp1Obat" value="<%#: Eval("IsAllowDiscountTariffComp1")%>" />
                                                    <input type="hidden" class="IsAllowDiscountTariffComp2Obat" value="<%#: Eval("IsAllowDiscountTariffComp2")%>" />
                                                    <input type="hidden" class="IsAllowDiscountTariffComp3Obat" value="<%#: Eval("IsAllowDiscountTariffComp3")%>" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <%=GetLabel("Pelayanan")%>
                                                    <br />
                                                    <%=GetLabel("Dokter/Tenaga Medis")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%#: Eval("ItemName1")%>
                                                    <br />
                                                    <span style="font-size: small; font-style: oblique">
                                                        <%#: Eval("ParamedicName")%>
                                                    </span>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Quantity" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="80px">
                                                <ItemTemplate>
                                                    <%#: Eval("ChargedQuantity", "{0:N}")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Sarana" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="80px">
                                                <ItemTemplate>
                                                    <%#: Eval("TariffComp1", "{0:N}")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Pelayanan" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="80px">
                                                <ItemTemplate>
                                                    <%#: Eval("TariffComp2", "{0:N}")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Lain-lain" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="80px">
                                                <ItemTemplate>
                                                    <%#: Eval("TariffComp3", "{0:N}")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Discount" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="80px">
                                                <ItemTemplate>
                                                    <%#: Eval("DiscountAmount", "{0:N}")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Total" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="80px">
                                                <ItemTemplate>
                                                    <%#: Eval("cfLineAmount", "{0:N}")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Data Tidak Tersedia")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                </td>
            </tr>
        </table>
    </div>
    <div id="containerLogisticsCtl" style="display: none" class="containerTransDtCtl">
        <table class="tblContentArea">
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <div id="containerPopupEntryDataBarang" style="margin-top: 10px; display: none;">
                        <input type="hidden" id="hdnIDBarang" runat="server" value="" />
                        <div class="pageTitle">
                            <%=GetLabel("Entry")%></div>
                        <fieldset id="fsEntryPopupBarang" style="margin: 0">
                            <table class="tblEntryDetail" style="width: 100%">
                                <colgroup>
                                    <col style="width: 150px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal" id="lblItemBarang">
                                            <%=GetLabel("Detail Item Name")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" value="" id="hdnDetailItemIDBarang" runat="server" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 120px" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtDetailItemCodeBarang" ReadOnly="true" Width="100%" runat="server" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDetailItemName1Barang" ReadOnly="true" Width="80%" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblLink lblMandatory" id="lblParamedicDetailBarang">
                                            <%=GetLabel("Paramedic")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" value="" id="hdnParamedicIDBarang" runat="server" />
                                        <input type="hidden" value="" id="hdnRevenueSharingIDBarang" runat="server" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 120px" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtParamedicCodeBarang" CssClass="required" Width="100%" runat="server" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtParamedicNameBarang" ReadOnly="true" CssClass="required" Width="80%"
                                                        runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblMandatory">
                                            <%=GetLabel("Quantity")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtChargedQuantityBarang" CssClass="number" Min="0" Width="120px"
                                            runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <table>
                                            <colgroup>
                                                <col style="width: 450px" />
                                            </colgroup>
                                            <tr>
                                                <td valign="top">
                                                    <table>
                                                        <colgroup>
                                                            <col style="width: 100px" />
                                                            <col style="width: 150px" />
                                                            <col style="width: 150px" />
                                                            <col style="width: 150px" />
                                                        </colgroup>
                                                        <tr>
                                                            <td class="tdLabel" colspan="2">
                                                                <label class="lblNormal">
                                                                    <%=GetLabel("Total Discount per Unit x Qty")%></label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtBarangDiscTotalCtl" ReadOnly="true" runat="server" Width="100%"
                                                                    CssClass="txtCurrency txtBarangDiscTotalCtl" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="tdLabel" colspan="2">
                                                                <label class="lblNormal">
                                                                    <%=GetLabel("Discount (%) ")%><%=GetTariffComponent1Text()%></label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtBarangDiscPercentComp1Ctl" ReadOnly="true" runat="server" Width="100%"
                                                                    CssClass="txtCurrency txtBarangDiscPercentComp1Ctl" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="tdLabel" colspan="2">
                                                                <label class="lblNormal">
                                                                    <%=GetLabel("Discount (%) ")%><%=GetTariffComponent2Text()%></label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtBarangDiscPercentComp2Ctl" ReadOnly="true" runat="server" Width="100%"
                                                                    CssClass="txtCurrency txtBarangDiscPercentComp2Ctl" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="tdLabel" colspan="2">
                                                                <label class="lblNormal">
                                                                    <%=GetLabel("Discount (%) ")%><%=GetTariffComponent3Text()%></label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtBarangDiscPercentComp3Ctl" ReadOnly="true" runat="server" Width="100%"
                                                                    CssClass="txtCurrency txtBarangDiscPercentComp3Ctl" />
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
                                                                <input id="txtBarangDiscTariffComp1Ctl" class="txtCurrency txtBarangDiscTariffComp1Ctl txtUnitTariffPrevCtl"
                                                                    readonly="readonly" style="width: 100%" runat="server" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtBarangDiscComp1Ctl" CssClass="txtCurrency txtBarangDiscComp1Ctl txtDiscComp"
                                                                    Width="100%" runat="server" />
                                                            </td>
                                                            <td>
                                                                <input id="txtBarangAfterTariffComp1Ctl" class="txtCurrency txtBarangAfterTariffComp1Ctl txtUnitTariffAfterCtl"
                                                                    readonly="readonly" style="width: 100%" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="tdLabel">
                                                                <label class="lblNormal">
                                                                    <%=GetTariffComponent2Text()%></label>
                                                            </td>
                                                            <td>
                                                                <input id="txtBarangDiscTariffComp2Ctl" class="txtCurrency txtBarangDiscTariffComp2Ctl txtUnitTariffPrevCtl"
                                                                    readonly="readonly" style="width: 100%" runat="server" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtBarangDiscComp2Ctl" CssClass="txtCurrency txtBarangDiscComp2Ctl txtDiscComp"
                                                                    Width="100%" runat="server" />
                                                            </td>
                                                            <td>
                                                                <input id="txtBarangAfterTariffComp2Ctl" class="txtCurrency txtBarangAfterTariffComp2Ctl txtUnitTariffAfterCtl"
                                                                    readonly="readonly" style="width: 100%" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="tdLabel">
                                                                <label class="lblNormal">
                                                                    <%=GetTariffComponent3Text()%></label>
                                                            </td>
                                                            <td>
                                                                <input id="txtBarangDiscTariffComp3Ctl" class="txtCurrency txtBarangDiscTariffComp3Ctl txtUnitTariffPrevCtl"
                                                                    readonly="readonly" style="width: 100%" runat="server" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtBarangDiscComp3Ctl" CssClass="txtCurrency txtBarangDiscComp3Ctl txtServiceDiscComp3Ctl txtDiscComp"
                                                                    Width="100%" runat="server" />
                                                            </td>
                                                            <td>
                                                                <input id="txtBarangAfterTariffComp3Ctl" class="txtCurrency txtBarangAfterTariffComp3Ctl txtUnitTariffAfterCtl"
                                                                    readonly="readonly" style="width: 100%" runat="server" />
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
                                        <center>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <input type="button" id="btnEntryPopupSaveBarang" value='<%= GetLabel("SAVE")%>'
                                                            class="btnEntryPopupSaveBarang w3-btn w3-hover-blue" style="width: 80px;" />
                                                    </td>
                                                    <td>
                                                        <input type="button" id="btnEntryPopupCancelBarang" value='<%= GetLabel("CANCEL")%>'
                                                            class="btnEntryPopupCancelBarang w3-btn w3-hover-blue" style="width: 80px;" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>
                    <dxcp:ASPxCallbackPanel ID="cbpEntryPopupViewBarang" runat="server" Width="100%"
                        ClientInstanceName="cbpEntryPopupViewBarang" ShowLoadingPanel="false" OnCallback="cbpEntryPopupViewBarang_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewPopupBarang').show(); }"
                            EndCallback="function(s,e){ onCbpEntryPopupViewBarangEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent3" runat="server">
                                <asp:Panel runat="server" ID="pnlEntryPopupgrdViewBarang" Style="width: 100%; margin-left: auto;
                                    margin-right: auto; position: relative; font-size: 0.95em;">
                                    <asp:GridView ID="grdViewBarang" runat="server" CssClass="grdView notAllowSelect"
                                        AutoGenerateColumns="false" OnRowDataBound="grdViewBarang_RowDataBound" ShowHeaderWhenEmpty="true"
                                        EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <img class="imgEditBarang imgLink" <%#: Eval("IsUsingAccumulatedPrice").ToString() == "True" ?  "" : "style='display:none'" %>
                                                        src='<%# Eval("IsUsingAccumulatedPrice").ToString() == "True" ? ResolveUrl("~/Libs/Images/Button/edit.png") : ""%>'
                                                        alt="" style="float: left; margin-left: 7px" />
                                                    <img class="imgDeleteBarang imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                        alt="" style="display: none" />
                                                    <input type="hidden" class="IDBarang" value="<%#: Eval("ID")%>" />
                                                    <input type="hidden" class="ItemIDBarang" value="<%#: Eval("ItemID")%>" />
                                                    <input type="hidden" class="ItemCodeBarang" value="<%#: Eval("ItemCode")%>" />
                                                    <input type="hidden" class="ItemName1Barang" value="<%#: Eval("ItemName1")%>" />
                                                    <input type="hidden" class="ParamedicIDBarang" value="<%#: Eval("ParamedicID")%>" />
                                                    <input type="hidden" class="ParamedicCodeBarang" value="<%#: Eval("ParamedicCode")%>" />
                                                    <input type="hidden" class="ParamedicNameBarang" value="<%#: Eval("ParamedicName")%>" />
                                                    <input type="hidden" class="RevenueSharingIDBarang" value="<%#: Eval("RevenueSharingID")%>" />
                                                    <input type="hidden" class="RevenueSharingNameBarang" value="<%#: Eval("RevenueSharingCode")%>" />
                                                    <input type="hidden" class="ChargedQuantityBarang" value="<%#: Eval("ChargedQuantity")%>" />
                                                    <input type="hidden" class="BaseTariffBarang" value="<%#: Eval("BaseTariff")%>" />
                                                    <input type="hidden" class="BaseComp1Barang" value="<%#: Eval("BaseComp1")%>" />
                                                    <input type="hidden" class="BaseComp2Barang" value="<%#: Eval("BaseComp2")%>" />
                                                    <input type="hidden" class="BaseComp3Barang" value="<%#: Eval("BaseComp3")%>" />
                                                    <input type="hidden" class="TariffBarang" value="<%#: Eval("Tariff")%>" />
                                                    <input type="hidden" class="TariffComp1Barang" value="<%#: Eval("TariffComp1")%>" />
                                                    <input type="hidden" class="TariffComp2Barang" value="<%#: Eval("TariffComp2")%>" />
                                                    <input type="hidden" class="TariffComp3Barang" value="<%#: Eval("TariffComp3")%>" />
                                                    <input type="hidden" class="DiscountBarang" value="<%#: Eval("DiscountAmount")%>" />
                                                    <input type="hidden" class="DiscountComp1Barang" value="<%#: Eval("DiscountComp1")%>" />
                                                    <input type="hidden" class="DiscountComp2Barang" value="<%#: Eval("DiscountComp2")%>" />
                                                    <input type="hidden" class="DiscountComp3Barang" value="<%#: Eval("DiscountComp3")%>" />
                                                    <input type="hidden" class="DiscountPercentageComp1Barang" value="<%#: Eval("DiscountPercentageComp1")%>" />
                                                    <input type="hidden" class="DiscountPercentageComp2Barang" value="<%#: Eval("DiscountPercentageComp2")%>" />
                                                    <input type="hidden" class="DiscountPercentageComp3Barang" value="<%#: Eval("DiscountPercentageComp3")%>" />
                                                    <input type="hidden" class="IsAllowDiscountBarang" value="<%#: Eval("IsAllowDiscount")%>" />
                                                    <input type="hidden" class="IsAllowDiscountTariffComp1Barang" value="<%#: Eval("IsAllowDiscountTariffComp1")%>" />
                                                    <input type="hidden" class="IsAllowDiscountTariffComp2Barang" value="<%#: Eval("IsAllowDiscountTariffComp2")%>" />
                                                    <input type="hidden" class="IsAllowDiscountTariffComp3Barang" value="<%#: Eval("IsAllowDiscountTariffComp3")%>" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <%=GetLabel("Pelayanan")%>
                                                    <br />
                                                    <%=GetLabel("Dokter/Tenaga Medis")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%#: Eval("ItemName1")%>
                                                    <br />
                                                    <span style="font-size: small; font-style: oblique">
                                                        <%#: Eval("ParamedicName")%>
                                                    </span>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Quantity" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="80px">
                                                <ItemTemplate>
                                                    <%#: Eval("ChargedQuantity", "{0:N}")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Sarana" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="80px">
                                                <ItemTemplate>
                                                    <%#: Eval("TariffComp1", "{0:N}")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Pelayanan" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="80px">
                                                <ItemTemplate>
                                                    <%#: Eval("TariffComp2", "{0:N}")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Lain-lain" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="80px">
                                                <ItemTemplate>
                                                    <%#: Eval("TariffComp3", "{0:N}")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Discount" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="80px">
                                                <ItemTemplate>
                                                    <%#: Eval("DiscountAmount", "{0:N}")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Total" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="80px">
                                                <ItemTemplate>
                                                    <%#: Eval("cfLineAmount", "{0:N}")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Data Tidak Tersedia")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                </td>
            </tr>
        </table>
    </div>
</div>
