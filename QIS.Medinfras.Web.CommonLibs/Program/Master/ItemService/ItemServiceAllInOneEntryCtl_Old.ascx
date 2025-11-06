<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ItemServiceAllInOneEntryCtl_Old.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.ItemServiceAllInOneEntryCtl_Old" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_ItemServiceAllInOneEntryCtl_Old">
    $('#lblEntryPopupAddData').live('click', function () {
        $('#<%=hdnIsEdit.ClientID %>').val('0');
        $('#<%=hdnID.ClientID %>').val('');

        $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val('');
        $('#<%=hdnServiceUnitID.ClientID %>').val('');
        $('#<%=txtServiceUnitCode.ClientID %>').val('');
        $('#<%=txtServiceUnitName.ClientID %>').val('');

        $('#<%=txtDetailItemCode.ClientID %>').val('');
        $('#<%=txtDetailItemName.ClientID %>').val('');
        $('#<%=txtParamedicCode.ClientID %>').val('');
        $('#<%=txtParamedicName.ClientID %>').val('');
        $('#<%=txtDetailItemQuantity.ClientID %>').val('1');
        $('#<%=chkIsControlAmount.ClientID%>').prop("checked", false);

        resetTariff();

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

    //#region Tariff
    $('#<%=txtDiscountInPercentage.ClientID %>').die('change');
    $('#<%=txtDiscountInPercentage.ClientID %>').live('change', function () {
        $val = $(this).val();
        $('#<%=hdnDiscountInPercentage.ClientID %>').val($val);
        $price = 0;
        if ($('#<%=txtPrice.ClientID %>').val() != '') {
            $price = parseFloat($('#<%=txtPrice.ClientID %>').attr('hiddenVal'));
        }
        $discount = ($val * $price / 100).toFixed(2);
        $('#<%=hdnDiscountAmount.ClientID %>').val($discount);
        $('#<%=txtDiscountAmount.ClientID %>').val($discount).trigger('changeValue');

        var discAmount = $('#<%=hdnDiscountAmount.ClientID %>').val();
        var price = parseFloat($('#<%=txtPrice.ClientID %>').attr('hiddenVal'));
        var priceEnd = price - discAmount;
        $('#<%=txtPriceFinal.ClientID %>').val(priceEnd).trigger('changeValue');
        $('#<%=hdnPriceFinal.ClientID %>').val(priceEnd);
    });

    $('#<%=txtDiscountAmount.ClientID %>').die('change');
    $('#<%=txtDiscountAmount.ClientID %>').live('change', function () {
        $('#<%=hdnDiscountAmount.ClientID %>').val($(this).val());
        var discAmount = $('#<%=hdnDiscountAmount.ClientID %>').val();
        var price = parseFloat($('#<%=txtPrice.ClientID %>').attr('hiddenVal'));
        var priceEnd = price - discAmount;
        $('#<%=txtPriceFinal.ClientID %>').val(priceEnd).trigger('changeValue');
        $('#<%=hdnPriceFinal.ClientID %>').val(priceEnd);

        var discInPercentageFinal = ((discAmount / price) * 100).toFixed(2);
        $('#<%=txtDiscountInPercentage.ClientID %>').val(discInPercentageFinal).trigger('changeValue');
        $('#<%=hdnDiscountInPercentage.ClientID %>').val(discInPercentageFinal)
    });

    $('#<%=txtPriceFinal.ClientID %>').die('change');
    $('#<%=txtPriceFinal.ClientID %>').live('change', function () {
        var priceFinal = $(this).val();
        $('#<%=hdnPriceFinal.ClientID %>').val(priceFinal);
        var price = parseFloat($('#<%=txtPrice.ClientID %>').attr('hiddenVal'));

        var disc = price - priceFinal;

        $('#<%=hdnDiscountAmount.ClientID %>').val(disc);
        $('#<%=txtDiscountAmount.ClientID %>').val(disc).trigger('changeValue');

        var discInPercentageFinal = ((disc / price) * 100).toFixed(2);
        $('#<%=txtDiscountInPercentage.ClientID %>').val(discInPercentageFinal).trigger('changeValue');
        $('#<%=hdnDiscountInPercentage.ClientID %>').val(discInPercentageFinal)
    });
    //#endregion

    //#region Tariff Comp 1
    $('#<%=txtDiscountInPercentage1.ClientID %>').die('change');
    $('#<%=txtDiscountInPercentage1.ClientID %>').live('change', function () {
        $val = $(this).val();
        $('#<%=hdnDiscountInPercentage1.ClientID %>').val($val);
        $PriceComp1 = 0;
        if ($('#<%=txtPriceComp1.ClientID %>').val() != '') {
            $PriceComp1 = parseFloat($('#<%=txtPriceComp1.ClientID %>').attr('hiddenVal'));
        }
        $discount1 = ($val * $PriceComp1 / 100).toFixed(2);
        $('#<%=hdnDiscountAmount1.ClientID %>').val($discount1);
        $('#<%=txtDiscountAmount1.ClientID %>').val($discount1).trigger('changeValue');

        var discAmount1 = $('#<%=hdnDiscountAmount1.ClientID %>').val();
        var PriceComp1 = parseFloat($('#<%=txtPriceComp1.ClientID %>').attr('hiddenVal'));
        var priceEnd1 = PriceComp1 - discAmount1;
        $('#<%=txtPriceFinalComp1.ClientID %>').val(priceEnd1).trigger('changeValue');
        $('#<%=hdnPriceFinalComp1.ClientID %>').val(priceEnd1);

        calculateDiscountTotal();
        calculateDiscountPercentTotal();
        calculateTariffTotal();
    });

    $('#<%=txtDiscountAmount1.ClientID %>').die('change');
    $('#<%=txtDiscountAmount1.ClientID %>').live('change', function () {
        $('#<%=hdnDiscountAmount1.ClientID %>').val($(this).val());
        var discAmount1 = $('#<%=hdnDiscountAmount1.ClientID %>').val();
        var PriceComp1 = parseFloat($('#<%=txtPriceComp1.ClientID %>').attr('hiddenVal'));
        var priceEnd1 = PriceComp1 - discAmount1;
        $('#<%=txtPriceFinalComp1.ClientID %>').val(priceEnd1).trigger('changeValue');
        $('#<%=hdnPriceFinalComp1.ClientID %>').val(priceEnd1);

        var discInPercentageFinal1 = ((discAmount1 / PriceComp1) * 100).toFixed(2);
        $('#<%=txtDiscountInPercentage1.ClientID %>').val(discInPercentageFinal1).trigger('changeValue');
        $('#<%=hdnDiscountInPercentage1.ClientID %>').val(discInPercentageFinal1);

        calculateDiscountTotal();
        calculateDiscountPercentTotal();
        calculateTariffTotal();
    });

    $('#<%=txtPriceFinalComp1.ClientID %>').die('change');
    $('#<%=txtPriceFinalComp1.ClientID %>').live('change', function () {
        var priceFinal1 = $(this).val();
        $('#<%=hdnPriceFinalComp1.ClientID %>').val(priceFinal1);
        var PriceComp1 = parseFloat($('#<%=txtPriceComp1.ClientID %>').attr('hiddenVal'));

        var disc1 = PriceComp1 - priceFinal1;

        $('#<%=hdnDiscountAmount1.ClientID %>').val(disc1);
        $('#<%=txtDiscountAmount1.ClientID %>').val(disc1).trigger('changeValue');

        var discInPercentageFinal1 = ((disc1 / PriceComp1) * 100).toFixed(2);
        $('#<%=txtDiscountInPercentage1.ClientID %>').val(discInPercentageFinal1).trigger('changeValue');
        $('#<%=hdnDiscountInPercentage1.ClientID %>').val(discInPercentageFinal1);

        calculateDiscountTotal();
        calculateDiscountPercentTotal();
        calculateTariffTotal();
    });
    //#endregion

    //#region Tariff Comp 2
    $('#<%=txtDiscountInPercentage2.ClientID %>').die('change');
    $('#<%=txtDiscountInPercentage2.ClientID %>').live('change', function () {
        $val = $(this).val();
        $('#<%=hdnDiscountInPercentage2.ClientID %>').val($val);
        $PriceComp2 = 0;
        if ($('#<%=txtPriceComp2.ClientID %>').val() != '') {
            $PriceComp2 = parseFloat($('#<%=txtPriceComp2.ClientID %>').attr('hiddenVal'));
        }
        $discount2 = ($val * $PriceComp2 / 100).toFixed(2);
        $('#<%=hdnDiscountAmount2.ClientID %>').val($discount2);
        $('#<%=txtDiscountAmount2.ClientID %>').val($discount2).trigger('changeValue');

        var discAmount2 = $('#<%=hdnDiscountAmount2.ClientID %>').val();
        var PriceComp2 = parseFloat($('#<%=txtPriceComp2.ClientID %>').attr('hiddenVal'));
        var priceEnd2 = PriceComp2 - discAmount2;
        $('#<%=txtPriceFinalComp2.ClientID %>').val(priceEnd2).trigger('changeValue');
        $('#<%=hdnPriceFinalComp2.ClientID %>').val(priceEnd2);

        calculateDiscountTotal();
        calculateDiscountPercentTotal();
        calculateTariffTotal();
    });

    $('#<%=txtDiscountAmount2.ClientID %>').die('change');
    $('#<%=txtDiscountAmount2.ClientID %>').live('change', function () {
        $('#<%=hdnDiscountAmount2.ClientID %>').val($(this).val());
        var discAmount2 = $('#<%=hdnDiscountAmount2.ClientID %>').val();
        var PriceComp2 = parseFloat($('#<%=txtPriceComp2.ClientID %>').attr('hiddenVal'));
        var priceEnd2 = PriceComp2 - discAmount2;
        $('#<%=txtPriceFinalComp2.ClientID %>').val(priceEnd2).trigger('changeValue');
        $('#<%=hdnPriceFinalComp2.ClientID %>').val(priceEnd2);

        var discInPercentageFinal2 = ((discAmount2 / PriceComp2) * 100).toFixed(2);
        $('#<%=txtDiscountInPercentage2.ClientID %>').val(discInPercentageFinal2).trigger('changeValue');
        $('#<%=hdnDiscountInPercentage2.ClientID %>').val(discInPercentageFinal2);

        calculateDiscountTotal();
        calculateDiscountPercentTotal();
        calculateTariffTotal();
    });

    $('#<%=txtPriceFinalComp2.ClientID %>').die('change');
    $('#<%=txtPriceFinalComp2.ClientID %>').live('change', function () {
        var priceFinal2 = $(this).val();
        $('#<%=hdnPriceFinalComp2.ClientID %>').val(priceFinal2);
        var PriceComp2 = parseFloat($('#<%=txtPriceComp2.ClientID %>').attr('hiddenVal'));

        var disc2 = PriceComp2 - priceFinal2;

        $('#<%=hdnDiscountAmount2.ClientID %>').val(disc2);
        $('#<%=txtDiscountAmount2.ClientID %>').val(disc2).trigger('changeValue');

        var discInPercentageFinal2 = ((disc2 / PriceComp2) * 100).toFixed(2);
        $('#<%=txtDiscountInPercentage2.ClientID %>').val(discInPercentageFinal2).trigger('changeValue');
        $('#<%=hdnDiscountInPercentage2.ClientID %>').val(discInPercentageFinal2);

        calculateDiscountTotal();
        calculateDiscountPercentTotal();
        calculateTariffTotal();
    });
    //#endregion

    //#region Tariff Comp 3
    $('#<%=txtDiscountInPercentage3.ClientID %>').die('change');
    $('#<%=txtDiscountInPercentage3.ClientID %>').live('change', function () {
        $val = $(this).val();
        $('#<%=hdnDiscountInPercentage3.ClientID %>').val($val);
        calculateDiscountPercentTotal();
        $PriceComp3 = 0;
        if ($('#<%=txtPriceComp3.ClientID %>').val() != '') {
            $PriceComp3 = parseFloat($('#<%=txtPriceComp3.ClientID %>').attr('hiddenVal'));
        }
        $discount3 = ($val * $PriceComp3 / 100).toFixed(2);
        $('#<%=hdnDiscountAmount3.ClientID %>').val($discount2);
        $('#<%=txtDiscountAmount3.ClientID %>').val($discount3).trigger('changeValue');

        var discAmount3 = $('#<%=hdnDiscountAmount3.ClientID %>').val();
        var PriceComp3 = parseFloat($('#<%=txtPriceComp3.ClientID %>').attr('hiddenVal'));
        var priceEnd3 = PriceComp3 - discAmount3;
        $('#<%=txtPriceFinalComp3.ClientID %>').val(priceEnd3).trigger('changeValue');
        $('#<%=hdnPriceFinalComp3.ClientID %>').val(priceEnd3);

        calculateDiscountTotal();
        calculateDiscountPercentTotal();
        calculateTariffTotal();
    });

    $('#<%=txtDiscountAmount3.ClientID %>').die('change');
    $('#<%=txtDiscountAmount3.ClientID %>').live('change', function () {
        $('#<%=hdnDiscountAmount3.ClientID %>').val($(this).val());
        var discAmount3 = $('#<%=hdnDiscountAmount3.ClientID %>').val();
        var PriceComp3 = parseFloat($('#<%=txtPriceComp3.ClientID %>').attr('hiddenVal'));
        var priceEnd3 = PriceComp3 - discAmount3;
        $('#<%=txtPriceFinalComp3.ClientID %>').val(priceEnd3).trigger('changeValue');
        $('#<%=hdnPriceFinalComp3.ClientID %>').val(priceEnd3);

        var discInPercentageFinal3 = ((discAmount3 / PriceComp3) * 100).toFixed(2);
        $('#<%=txtDiscountInPercentage3.ClientID %>').val(discInPercentageFinal3).trigger('changeValue');
        $('#<%=hdnDiscountInPercentage3.ClientID %>').val(discInPercentageFinal3);

        calculateDiscountTotal();
        calculateDiscountPercentTotal();
        calculateTariffTotal();
    });

    $('#<%=txtPriceFinalComp3.ClientID %>').die('change');
    $('#<%=txtPriceFinalComp3.ClientID %>').live('change', function () {
        var priceFinal3 = $(this).val();
        $('#<%=hdnPriceFinalComp3.ClientID %>').val(priceFinal3);
        var PriceComp3 = parseFloat($('#<%=txtPriceComp3.ClientID %>').attr('hiddenVal'));

        var disc3 = PriceComp3 - priceFinal3;

        $('#<%=hdnDiscountAmount3.ClientID %>').val(disc3);
        $('#<%=txtDiscountAmount3.ClientID %>').val(disc3).trigger('changeValue');

        var discInPercentageFinal3 = ((disc3 / PriceComp3) * 100).toFixed(2);
        $('#<%=txtDiscountInPercentage3.ClientID %>').val(discInPercentageFinal3).trigger('changeValue');
        $('#<%=hdnDiscountInPercentage3.ClientID %>').val(discInPercentageFinal3);

        calculateDiscountTotal();
        calculateDiscountPercentTotal();
        calculateTariffTotal();
    });
    //#endregion

    function calculateTariffTotal() {
        var total = 0;
        var comp1 = parseFloat($('#<%=hdnPriceFinalComp1.ClientID %>').val());
        var comp2 = parseFloat($('#<%=hdnPriceFinalComp2.ClientID %>').val());
        var comp3 = parseFloat($('#<%=hdnPriceFinalComp3.ClientID %>').val());

        total = comp1 + comp2 + comp3;
        $('#<%=txtPriceFinal.ClientID %>').val(total).trigger('changeValue');
        $('#<%=hdnPriceFinal.ClientID %>').val(total);
    }

    function calculateDiscountPercentTotal() {
        var price = parseFloat($('#<%=txtPrice.ClientID %>').attr('hiddenVal'));
        var discount = parseFloat($('#<%=hdnDiscountAmount.ClientID %>').val());
        var discountInPercent = (discount / price * 100).toFixed(2);
        $('#<%=txtDiscountInPercentage.ClientID %>').val(discountInPercent).trigger('changeValue');
        $('#<%=hdnDiscountInPercentage.ClientID %>').val(discountInPercent)
    }

    function calculateDiscountTotal() {
        var total = 0;
        var comp1 = parseFloat($('#<%=hdnDiscountAmount1.ClientID %>').val());
        var comp2 = parseFloat($('#<%=hdnDiscountAmount2.ClientID %>').val());
        var comp3 = parseFloat($('#<%=hdnDiscountAmount3.ClientID %>').val());

        total = comp1 + comp2 + comp3;
        $('#<%=txtDiscountAmount.ClientID %>').val(total).trigger('changeValue');
        $('#<%=hdnDiscountAmount.ClientID %>').val(total);
    }

    function resetTariff() {
        $('#<%=txtPrice.ClientID %>').val("0").trigger('changeValue');
        $('#<%=txtPriceComp1.ClientID %>').val("0").trigger('changeValue');
        $('#<%=txtPriceComp2.ClientID %>').val("0").trigger('changeValue');
        $('#<%=txtPriceComp3.ClientID %>').val("0").trigger('changeValue');

        $('#<%=txtDiscountInPercentage.ClientID %>').val("0").trigger('changeValue');
        $('#<%=hdnDiscountInPercentage.ClientID %>').val("0");
        $('#<%=txtDiscountInPercentage1.ClientID %>').val("0").trigger('changeValue');
        $('#<%=hdnDiscountInPercentage1.ClientID %>').val("0");
        $('#<%=txtDiscountInPercentage2.ClientID %>').val("0").trigger('changeValue');
        $('#<%=hdnDiscountInPercentage2.ClientID %>').val("0");
        $('#<%=txtDiscountInPercentage3.ClientID %>').val("0").trigger('changeValue');
        $('#<%=hdnDiscountInPercentage3.ClientID %>').val("0");

        $('#<%=txtDiscountAmount.ClientID %>').val("0").trigger('changeValue');
        $('#<%=hdnDiscountAmount.ClientID %>').val("0");
        $('#<%=txtDiscountAmount1.ClientID %>').val("0").trigger('changeValue');
        $('#<%=hdnDiscountAmount1.ClientID %>').val("0");
        $('#<%=txtDiscountAmount2.ClientID %>').val("0").trigger('changeValue');
        $('#<%=hdnDiscountAmount2.ClientID %>').val("0");
        $('#<%=txtDiscountAmount3.ClientID %>').val("0").trigger('changeValue');
        $('#<%=hdnDiscountAmount3.ClientID %>').val("0");

        $('#<%=txtPriceFinal.ClientID %>').val("0").trigger('changeValue');
        $('#<%=hdnPriceFinal.ClientID %>').val("0");
        $('#<%=txtPriceFinalComp1.ClientID %>').val("0").trigger('changeValue');
        $('#<%=hdnPriceFinalComp1.ClientID %>').val("0");
        $('#<%=txtPriceFinalComp2.ClientID %>').val("0").trigger('changeValue');
        $('#<%=hdnPriceFinalComp2.ClientID %>').val("0");
        $('#<%=txtPriceFinalComp3.ClientID %>').val("0").trigger('changeValue');
        $('#<%=hdnPriceFinalComp3.ClientID %>').val("0");

    }

    //#region Edit & Delete Rows
    $('.imgEdit.imgLink').die('click');
    $('.imgEdit.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        var entity = rowToObject($row);
        $('#<%=hdnIsEdit.ClientID %>').val('1');
        $('#<%=hdnID.ClientID %>').val(entity.ID);
        $('#<%=hdnGCItemType.ClientID %>').val(entity.GCItemType);

        cboDepartment.SetValue(entity.DepartmentID); 
        $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val(entity.HealthcareServiceUnitID);
        $('#<%=hdnServiceUnitID.ClientID %>').val(entity.ServiceUnitID);
        $('#<%=txtServiceUnitCode.ClientID %>').val(entity.ServiceUnitCode);
        $('#<%=txtServiceUnitName.ClientID %>').val(entity.ServiceUnitName);

        $('#<%=hdnDetailItemID.ClientID %>').val(entity.DetailItemID);
        $('#<%=txtDetailItemCode.ClientID %>').val(entity.DetailItemCode);
        $('#<%=txtDetailItemName.ClientID %>').val(entity.DetailItemName1);
        $('#<%=txtParamedicCode.ClientID %>').val(entity.ParamedicCode);
        $('#<%=txtParamedicName.ClientID %>').val(entity.ParamedicName);
        $('#<%=txtDetailItemQuantity.ClientID %>').val(entity.Quantity);
        $('#<%=chkIsControlAmount.ClientID %>').prop("checked", entity.IsControlAmount == "True");

        if (entity.GCItemType != Constant.ItemType.OBAT_OBATAN && entity.GCItemType != Constant.ItemType.BARANG_MEDIS && entity.GCItemType != Constant.ItemType.BARANG_UMUM && entity.GCItemType != Constant.ItemType.BAHAN_MAKANAN) {
            $('#<%=tdIsControlAmount.ClientID %>').attr('style', '');
        } else {
            $('#<%=tdIsControlAmount.ClientID %>').attr('style', 'display:none');
        }

        var discountPercent = 0;
        if (entity.Price != 0) {
            discountPercent = parseFloat((entity.DiscountAmount / entity.Price) * 100).toFixed(2);
        }
        if (discountPercent == null || discountPercent.toString() == "NaN") {
            discountPercent = parseFloat(0);
        }
        $('#<%=txtDiscountInPercentage.ClientID %>').val(discountPercent).trigger('changeValue');
        $('#<%=hdnDiscountInPercentage.ClientID %>').val(discountPercent);

        var discountPercent1 = 0;
        if (entity.PriceComp1 != 0) {
            discountPercent1 = parseFloat((entity.DiscountComp1 / entity.PriceComp1) * 100).toFixed(2);
        }
        if (discountPercent1 == null || discountPercent1.toString() == "NaN") {
            discountPercent1 = parseFloat(0);
        }
        $('#<%=txtDiscountInPercentage1.ClientID %>').val(discountPercent1).trigger('changeValue');
        $('#<%=hdnDiscountInPercentage1.ClientID %>').val(discountPercent1);

        var discountPercent2 = 0;
        if (entity.PriceComp2 != 0) {
            discountPercent2 = parseFloat((entity.DiscountComp2 / entity.PriceComp2) * 100).toFixed(2);
        }
        if (discountPercent2 == null || discountPercent2.toString() == "NaN") {
            discountPercent2 = parseFloat(0);
        }
        $('#<%=txtDiscountInPercentage2.ClientID %>').val(discountPercent2).trigger('changeValue');
        $('#<%=hdnDiscountInPercentage2.ClientID %>').val(discountPercent2);

        var discountPercent3 = 0;
        if (entity.PriceComp3 != 0) {
            discountPercent3 = parseFloat((entity.DiscountComp3 / entity.PriceComp3) * 100).toFixed(2);
        }
        if (discountPercent3 == null || discountPercent3.toString() == "NaN") {
            discountPercent3 = parseFloat(0);
        }
        $('#<%=txtDiscountInPercentage3.ClientID %>').val(discountPercent3).trigger('changeValue');
        $('#<%=hdnDiscountInPercentage3.ClientID %>').val(discountPercent3);

        $('#<%=txtDiscountAmount.ClientID %>').val(entity.DiscountAmount).trigger('changeValue');
        $('#<%=hdnDiscountAmount.ClientID %>').val(entity.DiscountAmount);
        $('#<%=txtDiscountAmount1.ClientID %>').val(entity.DiscountComp1).trigger('changeValue');
        $('#<%=hdnDiscountAmount1.ClientID %>').val(entity.DiscountComp1);
        $('#<%=txtDiscountAmount2.ClientID %>').val(entity.DiscountComp2).trigger('changeValue');
        $('#<%=hdnDiscountAmount2.ClientID %>').val(entity.DiscountComp2);
        $('#<%=txtDiscountAmount3.ClientID %>').val(entity.DiscountComp3).trigger('changeValue');
        $('#<%=hdnDiscountAmount3.ClientID %>').val(entity.DiscountComp3);

        $('#<%=txtPrice.ClientID %>').val(entity.Price).trigger('changeValue');
        $('#<%=txtPriceComp1.ClientID %>').val(entity.PriceComp1).trigger('changeValue');
        $('#<%=txtPriceComp2.ClientID %>').val(entity.PriceComp2).trigger('changeValue');
        $('#<%=txtPriceComp3.ClientID %>').val(entity.PriceComp3).trigger('changeValue');

        $('#<%=txtPriceFinal.ClientID %>').val(entity.Price - entity.DiscountAmount).trigger('changeValue');
        $('#<%=hdnPriceFinal.ClientID %>').val(entity.Price - entity.DiscountAmount);
        $('#<%=txtPriceFinalComp1.ClientID %>').val(entity.PriceComp1 - entity.DiscountComp1).trigger('changeValue');
        $('#<%=hdnPriceFinalComp1.ClientID %>').val(entity.PriceComp1 - entity.DiscountComp1);
        $('#<%=txtPriceFinalComp2.ClientID %>').val(entity.PriceComp2 - entity.DiscountComp2).trigger('changeValue');
        $('#<%=hdnPriceFinalComp2.ClientID %>').val(entity.PriceComp2 - entity.DiscountComp2);
        $('#<%=txtPriceFinalComp3.ClientID %>').val(entity.PriceComp3 - entity.DiscountComp3).trigger('changeValue');
        $('#<%=hdnPriceFinalComp3.ClientID %>').val(entity.PriceComp3 - entity.DiscountComp3);

        $('#containerPopupEntryData').show();
    });

    $('.imgDelete.imgLink').die('click');
    $('.imgDelete.imgLink').live('click', function (evt) {
        evt.stopPropagation();
        evt.preventDefault();
        if (confirm("Are You Sure Want To Delete This Data?")) {
            $row = $(this).closest('tr');
            var entity = rowToObject($row);
            $('#<%=hdnID.ClientID %>').val(entity.ID);

            cbpEntryPopupView.PerformCallback('delete');
        }
    });
    //#endregion

    function onCbpProcessEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'getTariff') {
            $('#<%=txtPrice.ClientID %>').val(param[1]).trigger('changeValue');
            $('#<%=txtPriceComp1.ClientID %>').val(param[2]).trigger('changeValue');
            $('#<%=txtPriceComp2.ClientID %>').val(param[3]).trigger('changeValue');
            $('#<%=txtPriceComp3.ClientID %>').val(param[4]).trigger('changeValue');
        }
        $('#containerImgLoadingViewPopup').hide();
    }

    function onCbpEntryPopupViewEndCallback(s) {
        var param = s.cpResult.split('|');
        var pageCount = parseInt(param[2]);
        if (param[0] == 'save') {
            if (param[1] == 'fail') {
                showToast('Save Failed', 'Error Message : ' + param[2]);
            }
            else {
                var isEdit = $('#<%=hdnIsEdit.ClientID %>').val();
                if (isEdit == '1') {
                    $('#containerPopupEntryData').hide();
                    setPagingDetailItem(pageCount);
                } else if (isEdit == '0') {
                    setPagingDetailItem(pageCount);
                    $('#lblEntryPopupAddData').click();
                }
                $('#<%=txtTotalPrice.ClientID %>').val(param[2]);
                $('#<%=txtGapPrice.ClientID %>').val(param[3]);
                cbpEntryPopupView.PerformCallback('refresh');
            }
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail') {
                showToast('Delete Failed', 'Error Message : ' + param[2]);
            }
            else {
                $('#containerPopupEntryData').hide();
                $('#<%=txtTotalPrice.ClientID %>').val(param[2]);
                $('#<%=txtGapPrice.ClientID %>').val(param[3]);
                cbpEntryPopupView.PerformCallback('refresh');
            }
        }
        else if (param[0] == 'refresh') {
            var pageCountRefresh = parseInt(param[1]);
            setPagingDetailItem(pageCountRefresh);
        }
        $('#containerPopupEntryData').hide();
        $('#containerImgLoadingViewPopup').hide();
    }

    function oncboFilterDepartmentValueChanged() {
        var department = cboFilterDepartment.GetValue();

        cbpEntryPopupView.PerformCallback('refresh');
    }

    function oncboDepartmentValueChanged() {
        var department = cboDepartment.GetValue();
        $('#<%=hdnDepartmentID.ClientID %>').val(department);
    }

    //#region HealthcareServiceUnit
    function onGetHealthcareServiceUnitFilterExpression() {
        var filterExpression = "IsUsingRegistration = 1 AND IsDeleted = 0 AND DepartmentID IN ('" + $('#<%=hdnDepartmentID.ClientID %>').val() + "')";
        return filterExpression;
    }

    $('#<%:lblHealthcareServiceUnit.ClientID %>.lblLink').live('click', function () {
        openSearchDialog('serviceunitperhealthcare', onGetHealthcareServiceUnitFilterExpression(), function (value) {
            $('#<%=txtServiceUnitCode.ClientID %>').val(value);
            ontxtServiceUnitCodeChanged(value);
        });
    });

    $('#<%=txtServiceUnitCode.ClientID %>').live('change', function () {
        ontxtServiceUnitCodeChanged($(this).val());
    });

    function ontxtServiceUnitCodeChanged(value) {
        var filterExpression = onGetHealthcareServiceUnitFilterExpression() + " AND ServiceUnitCode = '" + value + "'";
        Methods.getObject('GetvHealthcareServiceUnitList', filterExpression, function (result) {
            if (result != null) {
                if (result.DepartmentID == Constant.Facility.INPATIENT) {
                    $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val('');
                    $('#<%=hdnServiceUnitID.ClientID %>').val('');
                    $('#<%=txtServiceUnitCode.ClientID %>').val('');
                    $('#<%=txtServiceUnitName.ClientID %>').val('');
                } else {
                    $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val(result.HealthcareServiceUnitID);
                    $('#<%=hdnServiceUnitID.ClientID %>').val(result.ServiceUnitID);
                    $('#<%=txtServiceUnitName.ClientID %>').val(result.ServiceUnitName);
                }
            }
            else {
                $('#<%=hdnDepartmentID.ClientID %>').val(cboDepartment.GetValue());
                $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val('');
                $('#<%=hdnServiceUnitID.ClientID %>').val('');
                $('#<%=txtServiceUnitCode.ClientID %>').val('');
                $('#<%=txtServiceUnitName.ClientID %>').val('');
            }

            $('#<%=hdnGCItemType.ClientID %>').val('');
            $('#<%=hdnDetailItemID.ClientID %>').val('');
            $('#<%=txtDetailItemName.ClientID %>').val('');
            $('#<%=txtDetailItemCode.ClientID %>').val('');

            cbpProcess.PerformCallback('getTariff|0');
            resetTariff();

        });
    }
    //#endregion  

    //#region Item
    function onGetItemServiceDtFilterExpression() {
        var itemID = $('#<%=hdnItemID.ClientID %>').val();
        var itemType = "'" + Constant.ItemType.OBAT_OBATAN + "','" + Constant.ItemType.BARANG_MEDIS + "','" + Constant.ItemType.BARANG_UMUM + "','" + Constant.ItemType.BAHAN_MAKANAN + "'";

        var deptID = $('#<%=hdnDepartmentID.ClientID %>').val();
        var suID = $('#<%=hdnServiceUnitID.ClientID %>').val();
        var hsuID = $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val();

        var filterExpression = "IsDeleted = 0 AND GCItemStatus != '" + Constant.ItemStatus.ITEM_STATUS_IN_ACTIVE + "' AND GCItemType NOT IN (" + itemType + ")";
        filterExpression += " AND ItemID IN (SELECT ItemID FROM ItemService WHERE IsPackageItem = 0 AND IsPackageAllInOne = 0)";

        if (deptID == Constant.Facility.INPATIENT) {
            filterExpression += " AND ItemID NOT IN (SELECT DetailItemID FROM ItemServiceDt WITH(NOLOCK) WHERE IsDeleted = 0 AND ItemID = " + itemID + ")";
            filterExpression += " AND ItemID IN (SELECT ItemID FROM vServiceUnitItem WITH(NOLOCK) WHERE IsDeleted = 0 AND DepartmentID = '" + deptID + "')";
        } else {
            if (hsuID != null && hsuID != 0) {
                filterExpression += " AND ItemID NOT IN (SELECT DetailItemID FROM ItemServiceDt WITH(NOLOCK) WHERE IsDeleted = 0 AND ServiceUnitID = " + suID + " AND ItemID = " + itemID + ")";
                filterExpression += " AND ItemID IN (SELECT ItemID FROM vServiceUnitItem WITH(NOLOCK) WHERE IsDeleted = 0 AND HealthcareServiceUnitID = " + hsuID + ")";
            }
        }
        return filterExpression;
    }

    $('#lblItemCtl.lblLink').live('click', function () {
        var oHealthcareServiceUnitID = $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val();
        var deptID = $('#<%=hdnDepartmentID.ClientID %>').val();
        if (deptID != Constant.Facility.INPATIENT) {
            if (oHealthcareServiceUnitID != null && oHealthcareServiceUnitID != "" && oHealthcareServiceUnitID != "0") {
                openSearchDialog('item', onGetItemServiceDtFilterExpression(), function (value) {
                    $('#<%=txtDetailItemCode.ClientID %>').val(value);
                    onTxtDetailItemCodeChanged(value);
                });
            } else {
                displayMessageBox("INFORMATION", "Harap pilih unit pelayanan terlebih dahulu.");
            }
        } else {
            openSearchDialog('item', onGetItemServiceDtFilterExpression(), function (value) {
                $('#<%=txtDetailItemCode.ClientID %>').val(value);
                onTxtDetailItemCodeChanged(value);
            });
        }
    });

    $('#<%=txtDetailItemCode.ClientID %>').live('change', function () {
        onTxtDetailItemCodeChanged($(this).val());
    });

    function onTxtDetailItemCodeChanged(value) {
        var filterExpression = onGetItemServiceDtFilterExpression() + " AND ItemCode = '" + value + "'";
        Methods.getObject('GetItemMasterList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnGCItemType.ClientID %>').val(result.GCItemType);
                $('#<%=hdnDetailItemID.ClientID %>').val(result.ItemID);
                $('#<%=txtDetailItemName.ClientID %>').val(result.ItemName1);

                if (result.GCItemType != Constant.ItemType.OBAT_OBATAN && result.GCItemType != Constant.ItemType.BARANG_MEDIS && result.GCItemType != Constant.ItemType.BARANG_UMUM && result.GCItemType != Constant.ItemType.BAHAN_MAKANAN) {
                    $('#<%=tdIsControlAmount.ClientID %>').attr('style', '');
                } else {
                    $('#<%=tdIsControlAmount.ClientID %>').attr('style', 'display:none');
                }

                cbpProcess.PerformCallback('getTariff|' + result.ItemID);
            }
            else {
                $('#<%=hdnGCItemType.ClientID %>').val('');
                $('#<%=hdnDetailItemID.ClientID %>').val('');
                $('#<%=txtDetailItemName.ClientID %>').val('');
                $('#<%=txtDetailItemCode.ClientID %>').val('');
            }
        });
    }
    //#endregion  

    //#region Paramedic
    function onGetParamedicMasterFilterExpression() {
        var serviceUnitID = $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val();
        if (serviceUnitID == '') {
            serviceUnitID = '0';
        }

        var filterExpression = "GCParamedicMasterType = 'X019^001' AND IsDeleted = 0 AND IsAvailable = 1";
        if (serviceUnitID != '0') {
            filterExpression += " AND ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = " + serviceUnitID + ")";
        }

        return filterExpression;
    }

    $('#lblParamedic.lblLink').live('click', function () {
        var filterExpression = onGetParamedicMasterFilterExpression();
        openSearchDialog('paramedic', filterExpression, function (value) {
            $('#<%=txtParamedicCode.ClientID %>').val(value);
            onTxtParamedicCodeChanged(value);
        });
    });

    $('#<%=txtParamedicCode.ClientID %>').live('change', function () {
        onTxtParamedicCodeChanged($(this).val());
    });

    function onTxtParamedicCodeChanged(value) {
        var filterExpression = onGetParamedicMasterFilterExpression() + " AND ParamedicCode = '" + value + "'";
        Methods.getObject('GetParamedicMasterList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnParamedicID.ClientID %>').val(result.ParamedicID);
                $('#<%=txtParamedicCode.ClientID %>').val(result.ParamedicCode);
                $('#<%=txtParamedicName.ClientID %>').val(result.FullName);
            }
            else {
                $('#<%=hdnParamedicID.ClientID %>').val('');
                $('#<%=txtParamedicCode.ClientID %>').val('');
                $('#<%=txtParamedicName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Paging
    var pageCountAvailable = parseInt('<%=PageCount %>');
    $(function () {
        setPagingDetailItem(pageCountAvailable);
    });

    function setPagingDetailItem(pageCount) {
        if (pageCount > 0)
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        setPaging($("#pagingPopup"), pageCount, function (page) {
            cbpEntryPopupView.PerformCallback('changepage|' + page);
        }, 8);
    }
    //#endregion 

</script>
<div style="height: 400px; overflow-y: auto; overflow-x: hidden;">
    <input type="hidden" id="hdnIsEdit" value="0" runat="server" />
    <input type="hidden" id="hdnItemID" value="" runat="server" />
    <input type="hidden" id="hdnIsUsingAccumulatedPrice" value="" runat="server" />
    <input type="hidden" id="hdnPackagePrice" value="" runat="server" />
    <input type="hidden" id="hdnAIOClassID" value="" runat="server" />
    <input type="hidden" id="hdnDepartmentID" runat="server" value="" />
    <input type="hidden" id="hdnHealthcareServiceUnitID" runat="server" value="" />
    <input type="hidden" id="hdnServiceUnitID" runat="server" value="" />
    <input type="hidden" id="hdnDetailItemID" runat="server" value="" />
    <input type="hidden" id="hdnGCItemType" runat="server" value="" />
    <input type="hidden" id="hdnParamedicID" runat="server" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 70%">
                    <colgroup>
                        <col style="width: 120px" />
                        <col style="width: 50px" />
                        <col style="width: 150px" />
                        <col style="width: 150px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Paket Pemeriksaan")%></label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtItemServiceName" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Harga")%></label>
                        </td>
                        <td colspan="3">
                            <table border="1px solid">
                                <tr>
                                    <th style="text-align: right">
                                        HARGA PAKET
                                    </th>
                                    <th style="text-align: right">
                                        TOTAL DETAIL
                                    </th>
                                    <th style="text-align: right">
                                        SELISIH
                                    </th>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox CssClass="txtCurrency" ID="txtPackagePrice" ReadOnly="true" Width="100%"
                                            runat="server" />
                                    </td>
                                    <td>
                                        <asp:TextBox CssClass="txtCurrency" ID="txtTotalPrice" ReadOnly="true" Width="100%"
                                            runat="server" />
                                    </td>
                                    <td>
                                        <asp:TextBox CssClass="txtCurrency" ID="txtGapPrice" ReadOnly="true" Width="100%"
                                            runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trDepartment" runat="server">
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Filter Instalasi")%></label>
                        </td>
                        <td colspan="3">
                            <dxe:ASPxComboBox ID="cboFilterDepartment" ClientInstanceName="cboFilterDepartment" runat="server"
                                Width="100%">
                                <ClientSideEvents ValueChanged="function(s,e) { oncboFilterDepartmentValueChanged(); }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                </table>
                <div id="containerPopupEntryData" style="margin-top: 5px; display: none;">
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
                                <td style="width: 120px">
                                    <label class="lblNormal" id="lblDepartment" runat="server">
                                        <%=GetLabel("Department")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboDepartment" ClientInstanceName="cboDepartment" runat="server"
                                        Width="350px">
                                        <ClientSideEvents ValueChanged="function(s,e) { oncboDepartmentValueChanged(); }" />
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 120px">
                                    <label class="lblLink" id="lblHealthcareServiceUnit" runat="server">
                                        <%=GetLabel("Unit Pelayanan")%></label>
                                </td>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtServiceUnitCode" CssClass="required" Width="150px" runat="server" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtServiceUnitName" ReadOnly="true" Width="500px" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 120px">
                                    <label class="lblMandatory lblLink" id="lblItemCtl">
                                        <%=GetLabel("Item")%></label>
                                </td>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtDetailItemCode" CssClass="required" Width="150px" runat="server" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDetailItemName" ReadOnly="true" Width="500px" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 120px">
                                    <label class="lblLink" id="lblParamedic">
                                        <%=GetLabel("Default Paramedic")%></label>
                                </td>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtParamedicCode" CssClass="required" Width="150px" runat="server" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtParamedicName" ReadOnly="true" Width="500px" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 120px">
                                    <label class="lblNormal">
                                        <%=GetLabel("Quantity")%></label>
                                </td>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtDetailItemQuantity" CssClass="number" runat="server" Width="150px" />
                                            </td>
                                            <td id="tdIsControlAmount" runat="server" style="display: none">
                                                <asp:CheckBox ID="chkIsControlAmount" runat="server" /><%=GetLabel("Kontrol Batasan Nilai Obat Alkes")%>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 120px">
                                    <label class="lblNormal">
                                        <%=GetLabel("Tariff")%></label>
                                </td>
                                <td>
                                    <table border="1px solid">
                                        <colgroup>
                                            <col style="width: 200px" />
                                            <col style="width: 200px" />
                                            <col style="width: 200px" />
                                            <col style="width: 200px" />
                                        </colgroup>
                                        <tr>
                                            <th style="text-align: center; background-color: Silver">
                                                TOTAL
                                            </th>
                                            <th style="text-align: center; background-color: Silver">
                                                Sarana
                                            </th>
                                            <th style="text-align: center; background-color: Silver">
                                                Pelayanan
                                            </th>
                                            <th style="text-align: center; background-color: Silver">
                                                Lain-lain
                                            </th>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtPrice" CssClass="txtCurrency" runat="server" Width="100%" ReadOnly="true" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPriceComp1" CssClass="txtCurrency" runat="server" Width="100%"
                                                    ReadOnly="true" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPriceComp2" CssClass="txtCurrency" runat="server" Width="100%"
                                                    ReadOnly="true" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPriceComp3" CssClass="txtCurrency" runat="server" Width="100%"
                                                    ReadOnly="true" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <input type="hidden" value="0" id="hdnPriceFinal" runat="server" />
                                                <asp:TextBox ID="txtPriceFinal" CssClass="txtCurrency" runat="server" Width="100%"
                                                    ReadOnly="true" />
                                            </td>
                                            <td>
                                                <input type="hidden" value="0" id="hdnPriceFinalComp1" runat="server" />
                                                <asp:TextBox ID="txtPriceFinalComp1" CssClass="txtCurrency" runat="server" Width="100%" />
                                            </td>
                                            <td>
                                                <input type="hidden" value="0" id="hdnPriceFinalComp2" runat="server" />
                                                <asp:TextBox ID="txtPriceFinalComp2" CssClass="txtCurrency" runat="server" Width="100%" />
                                            </td>
                                            <td>
                                                <input type="hidden" value="0" id="hdnPriceFinalComp3" runat="server" />
                                                <asp:TextBox ID="txtPriceFinalComp3" CssClass="txtCurrency" runat="server" Width="100%" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 120px">
                                    <label class="lblNormal">
                                        <%=GetLabel("Discount")%></label>
                                </td>
                                <td>
                                    <table border="1px solid">
                                        <colgroup>
                                            <col style="width: 200px" />
                                            <col style="width: 200px" />
                                            <col style="width: 200px" />
                                            <col style="width: 200px" />
                                        </colgroup>
                                        <tr>
                                            <td align="right">
                                                <input type="hidden" value="0" id="hdnDiscountAmount" runat="server" />
                                                <input type="hidden" value="0" id="hdnDiscountInPercentage" runat="server" />
                                                <asp:TextBox ID="txtDiscountInPercentage" CssClass="number" runat="server" value="0"
                                                    ReadOnly="true" Width="50px" />
                                            </td>
                                            <td align="right">
                                                <input type="hidden" value="0" id="hdnDiscountAmount1" runat="server" />
                                                <input type="hidden" value="0" id="hdnDiscountInPercentage1" runat="server" />
                                                <asp:TextBox ID="txtDiscountInPercentage1" CssClass="number" runat="server" value="0"
                                                    Width="50px" />
                                            </td>
                                            <td align="right">
                                                <input type="hidden" value="0" id="hdnDiscountAmount2" runat="server" />
                                                <input type="hidden" value="0" id="hdnDiscountInPercentage2" runat="server" />
                                                <asp:TextBox ID="txtDiscountInPercentage2" CssClass="number" runat="server" value="0"
                                                    Width="50px" />
                                            </td>
                                            <td align="right">
                                                <input type="hidden" value="0" id="hdnDiscountAmount3" runat="server" />
                                                <input type="hidden" value="0" id="hdnDiscountInPercentage3" runat="server" />
                                                <asp:TextBox ID="txtDiscountInPercentage3" CssClass="number" runat="server" value="0"
                                                    Width="50px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtDiscountAmount" CssClass="txtCurrency" runat="server" value="0"
                                                    ReadOnly="true" Width="100%" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDiscountAmount1" CssClass="txtCurrency" runat="server" value="0"
                                                    Width="100%" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDiscountAmount2" CssClass="txtCurrency" runat="server" value="0"
                                                    Width="100%" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDiscountAmount3" CssClass="txtCurrency" runat="server" value="0"
                                                    Width="100%" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 120px">
                                </td>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <input type="button" id="btnEntryPopupSave" style="width: 60px" value='<%= GetLabel("Save")%>' />
                                            </td>
                                            <td>
                                                <input type="button" id="btnEntryPopupCancel" style="width: 60px" value='<%= GetLabel("Cancel")%>' />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
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
                                                <img class="imgEdit imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                    alt="" style="float: left; margin-left: 7px" />
                                                <img class="imgDelete imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                    alt="" />
                                                <input type="hidden" bindingfield="ID" value="<%#: Eval("ID")%>" />
                                                <input type="hidden" bindingfield="ItemID" value="<%#: Eval("ItemID")%>" />
                                                <input type="hidden" bindingfield="DepartmentID" value="<%#: Eval("DepartmentID")%>" />
                                                <input type="hidden" bindingfield="DepartmentName" value="<%#: Eval("DepartmentName")%>" />
                                                <input type="hidden" bindingfield="HealthcareServiceUnitID" value="<%#: Eval("HealthcareServiceUnitID")%>" />
                                                <input type="hidden" bindingfield="ServiceUnitID" value="<%#: Eval("ServiceUnitID")%>" />
                                                <input type="hidden" bindingfield="ServiceUnitCode" value="<%#: Eval("ServiceUnitCode")%>" />
                                                <input type="hidden" bindingfield="ServiceUnitName" value="<%#: Eval("ServiceUnitName")%>" />
                                                <input type="hidden" bindingfield="GCItemType" value="<%#: Eval("GCItemType")%>" />
                                                <input type="hidden" bindingfield="ItemType" value="<%#: Eval("ItemType")%>" />
                                                <input type="hidden" bindingfield="DetailItemID" value="<%#: Eval("DetailItemID")%>" />
                                                <input type="hidden" bindingfield="DetailItemCode" value="<%#: Eval("DetailItemCode")%>" />
                                                <input type="hidden" bindingfield="DetailItemName1" value="<%#: Eval("DetailItemName1")%>" />
                                                <input type="hidden" bindingfield="Quantity" value="<%#: Eval("Quantity")%>" />
                                                <input type="hidden" bindingfield="IsControlAmount" value="<%#: Eval("IsControlAmount")%>" />
                                                <input type="hidden" bindingfield="ParamedicCode" value="<%#: Eval("ParamedicCode")%>" />
                                                <input type="hidden" bindingfield="ParamedicName" value="<%#: Eval("ParamedicName")%>" />
                                                <input type="hidden" bindingfield="Price" value="<%#: Eval("Price")%>" />
                                                <input type="hidden" bindingfield="PriceComp1" value="<%#: Eval("PriceComp1")%>" />
                                                <input type="hidden" bindingfield="PriceComp2" value="<%#: Eval("PriceComp2")%>" />
                                                <input type="hidden" bindingfield="PriceComp3" value="<%#: Eval("PriceComp3")%>" />
                                                <input type="hidden" bindingfield="DiscountAmount" value="<%#: Eval("DiscountAmount")%>" />
                                                <input type="hidden" bindingfield="DiscountComp1" value="<%#: Eval("DiscountComp1")%>" />
                                                <input type="hidden" bindingfield="DiscountComp2" value="<%#: Eval("DiscountComp2")%>" />
                                                <input type="hidden" bindingfield="DiscountComp3" value="<%#: Eval("DiscountComp3")%>" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Left" HeaderText="Unit Pelayanan"
                                            HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <label style="font-size: small">
                                                    <b>
                                                        <%#: Eval("ServiceUnitName")%></b></label><br />
                                                <label style="font-size: x-small">
                                                    <%#: Eval("ServiceUnitCode")%></label><br />
                                                <label style="font-size: small; font-style: italic">
                                                    <%#: Eval("DepartmentID") %></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderText="Detail Item" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <label style="font-size: small; font-weight: bold">
                                                    <%#: Eval("DetailItemName1")%></label>
                                                <img class="lblIsControlAmount" title="<%=GetLabel("Kontrol Batasan Nilai Obat Alkes") %>"
                                                    src='<%# ResolveUrl("~/Libs/Images/Status/coverage_ok.png")%>' alt="" style='<%# Eval("IsControlAmount").ToString() == "True" ? "": "display:none" %>'
                                                    width="20px" />
                                                <br />
                                                <label style="font-size: small; font-style: italic">
                                                    <%#: Eval("DetailItemCode") %></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ParamedicName" HeaderText="Default Paramedic" HeaderStyle-HorizontalAlign="Left"
                                            HeaderStyle-Width="180px" />
                                        <asp:BoundField DataField="Quantity" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                            HeaderText="Quantity" HeaderStyle-Width="70px" />
                                        <asp:TemplateField HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Right" HeaderText="Harga"
                                            HeaderStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <%#: Eval("Price", "{0:N2}")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" HeaderText="Discount"
                                            HeaderStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <%#: Eval("DiscountAmount", "{0:N2}")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="MCUPrice" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                            HeaderText="Harga AIO" HeaderStyle-Width="100px" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("No Data To Display")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                                <div class="imgLoadingGrdView" id="containerImgLoadingViewPopup">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div class="containerPaging">
                    <div class="wrapperPaging">
                        <div id="pagingPopup">
                        </div>
                    </div>
                </div>
                <div style="width: 100%; text-align: center" id="divContainerAddData" runat="server">
                    <span class="lblLink" id="lblEntryPopupAddData">
                        <%= GetLabel("Add Data")%></span>
                </div>
                <br />
                <div style="width: 100%; text-align: right">
                    <input type="button" value='<%= GetLabel("Close")%>' onclick="pcRightPanelContent.Hide();"
                        style="width: 80px; height: 24px" />
                </div>
                <dxcp:ASPxCallbackPanel runat="server" ID="cbpProcess" ClientInstanceName="cbpProcess"
                    ShowLoadingPanel="false" OnCallback="cbpProcess_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewPopup').show(); }"
                        EndCallback="function(s,e) { onCbpProcessEndCallback(s); }" />
                </dxcp:ASPxCallbackPanel>
            </td>
        </tr>
    </table>
</div>
