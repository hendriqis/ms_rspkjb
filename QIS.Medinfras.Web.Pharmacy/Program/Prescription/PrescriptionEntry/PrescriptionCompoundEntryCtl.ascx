<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PrescriptionCompoundEntryCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.Pharmacy.Program.PrescriptionCompoundEntryCtl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
    <%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" 
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
    
<script type="text/javascript" id="dxis_prescriptioncompoundentryctl" src='<%= ResolveUrl("~/Libs/Scripts/inlineEditing-1.0.js")%>'></script>
<script type="text/javascript" id="dxss_prescriptioncompoundentryctl">
    setDatePicker('<%=txtStartDate.ClientID %>');

    $('#btnPopupCancel').click(function () {
        $('#divEntryProduct').hide();
    });

    $('#btnPopupSave').click(function () {
        if (IsValid(null, 'fsTrxPopup', 'mpTrxPopup'))
            cbpPopupProcess.PerformCallback("save");
    });

    $('#lblPopupAddData').die('click');
    $('#lblPopupAddData').live('click', function () {
        if (IsValid(null, 'fsMPEntryPopup', 'mpEntryPopup')) {
            $('#<%=hdnPrescriptionOrderDetailID.ClientID %>').val('');
            $('#<%=txtGenericName.ClientID %>').val('');
            $('#<%=hdnItemID.ClientID %>').val('');
            $('#<%=txtItemCode.ClientID %>').val('');
            $('#<%=txtItemName1.ClientID %>').val('');
            $('#<%=txtDose.ClientID %>').val('');
            $('#<%=txtDoseUnit.ClientID %>').val('');
            $('#<%=txtCompoundQty.ClientID %>').val('');
            $('#<%=hdnGCBaseUnit.ClientID %>').val('');
            $('#<%=hdnBaseUnit.ClientID %>').val('');
            cboCompoundUnit.SetValue('');
            $('#<%=txtConversionFactor.ClientID %>').val('');
            $('#divEntryProduct').show();
            $('#<%=hdnIsAdd.ClientID %>').val(1);
            cboPopupChargeClass.SetValue($('#<%=hdnChargeClassID.ClientID %>').val());
            $('#<%=hdnResultQty.ClientID %>').val('');
            $('#<%=hdnItemDose.ClientID %>').val(0);
            $('#<%=txtPopupTotalAmount.ClientID %>').val('').trigger('changeValue');
            $('#<%=txtPopupDiscountAmount.ClientID %>').val('').trigger('changeValue');
            $('#<%=txtPopupPatientAmount.ClientID %>').val('').trigger('changeValue');
            $('#<%=txtPopupPayerAmount.ClientID %>').val('').trigger('changeValue');
            $('#<%=txtPopupLineAmount.ClientID %>').val('').trigger('changeValue');
            $('#<%=txtPopupTariff.ClientID %>').val('').trigger('changeValue');
            var parentID = $('#<%=hdnParentID.ClientID %>').val();
            if (parentID == '' || parentID == '0')
                $('#<%=txtEmbalaceAmount.ClientID %>').val($('#<%=hdnEmbalaceAmount.ClientID %>').val()).trigger('changeValue');
            else
                $('#<%=txtEmbalaceAmount.ClientID %>').val(0).trigger('changeValue');
        }
    });

    function setConversionText() {
        var baseItemUnit = $('#<%=txtDoseUnit.ClientID %>').val();
        var dose = $('#<%=txtDose.ClientID %>').val();
        var ItemUnit = $('#<%=hdnBaseUnit.ClientID %>').val();

        var compound = $('#<%=txtCompoundQty.ClientID %>').val();
        var compoundUnit = cboCompoundUnit.GetText();

        var conversion = '';
        var conversionFactor = 0;
        if (compoundUnit == ItemUnit) {
            conversionFactor = 1;
            conversion = '1 ' + ItemUnit + " = 1 " + ItemUnit;
        }
        else {
            conversionFactor = 1 / dose;
            conversion = '1 ' + ItemUnit + " = " + dose + " " + baseItemUnit;
        }

        $('#<%=txtDtDispenseUnit.ClientID %>').val(ItemUnit);
        $('#<%=hdnConversionFactor.ClientID %>').val(conversionFactor);
        $('#<%=txtConversionFactor.ClientID %>').val(conversion);

        calculateDispenseQty();
    }

    function onCboCompoundUnitValueChanged() {
        setConversionText();
    }

    function onCboCompoundUnitEndCallback() {
        var gcItemUnit = $('#<%=hdnGCItemUnit.ClientID %>').val();
        cboCompoundUnit.SetValue(gcItemUnit);
        var dose = $('#<%=txtDose.ClientID %>').val();
        if (dose == '0') {
            $('#<%=txtDose.ClientID %>').val(1);
            $('#<%=txtDoseUnit.ClientID %>').val(cboCompoundUnit.GetText());
            $('#<%=hdnGCDoseUnit.ClientID %>').val(cboCompoundUnit.GetValue());
            $('#<%=hdnItemDose.ClientID %>').val(1);
        }
        setConversionText();
    }

    $('.imgPopupDelete.imgLink').die('click');
    $('.imgPopupDelete.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        showToastConfirmation('Are You Sure Want To Delete?', function (result) {
            if (result) {
                var entity = rowToObject($row);
                $('#<%=hdnPrescriptionOrderDetailID.ClientID %>').val(entity.PrescriptionOrderDetailID);
                $('#<%=hdnTransactionDetailID.ClientID %>').val(entity.TransactionDetailID);
                cbpPopupProcess.PerformCallback("delete");
            }
        });
    });

    $('.imgPopupEdit.imgLink').die('click');
    $('.imgPopupEdit.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        var entity = rowToObject($row);

        $('#<%=hdnPrescriptionOrderDetailID.ClientID %>').val(entity.PrescriptionOrderDetailID);
        $('#<%=hdnTransactionDetailID.ClientID %>').val(entity.TransactionDetailID);
        $('#<%=txtGenericName.ClientID %>').val(entity.GenericName);
        $('#<%=hdnItemID.ClientID %>').val(entity.ItemID);
        $('#<%=txtItemCode.ClientID %>').val(entity.ItemCode);
        $('#<%=txtItemName1.ClientID %>').val(entity.ItemName1);
        $('#<%=txtDose.ClientID %>').val(entity.Dose);
        $('#<%=txtDoseUnit.ClientID %>').val(entity.DoseUnit)
        $('#<%=hdnGCDoseUnit.ClientID %>').val(entity.GCDoseUnit);
        $('#<%=txtCompoundQty.ClientID %>').val(entity.CompoundQty);
        $('#<%=hdnGCBaseUnit.ClientID %>').val(entity.GCBaseUnit);
        $('#<%=hdnBaseUnit.ClientID %>').val(entity.BaseUnit);
        $('#<%=hdnGCItemUnit.ClientID %>').val(entity.GCCompoundUnit);

        $('#<%=hdnResultQty.ClientID %>').val(entity.ResultQty);
        $('#<%=txtDtDispenseQty.ClientID %>').val(entity.ResultQty);

        if (entity.ParentID == '0')
            $('#<%=txtEmbalaceAmount.ClientID %>').val($('#<%=hdnEmbalaceAmount.ClientID %>').val()).trigger('changeValue');
        else
            $('#<%=txtEmbalaceAmount.ClientID %>').val('0').trigger('changeValue');

        $('#<%=hdnIsAdd.ClientID %>').val(0);
        $('#<%=hdnConversionFactor.ClientID %>').val(entity.ConversionFactor);
        $('#<%=hdnItemDose.ClientID %>').val(entity.ItemDose);
        cboPopupChargeClass.SetValue(entity.ChargeClassID);
        $('#<%=txtPopupDiscountAmount.ClientID %>').val(entity.DiscountAmount).trigger('changeValue');
        $('#<%=txtPopupPatientAmount.ClientID %>').val(entity.PatientAmount).trigger('changeValue');
        $('#<%=txtPopupPayerAmount.ClientID %>').val(entity.PayerAmount).trigger('changeValue');
        $('#<%=txtPopupLineAmount.ClientID %>').val(entity.LineAmount).trigger('changeValue');
        $('#<%=txtPopupTariff.ClientID %>').val(entity.Tariff).trigger('changeValue');
        cboCompoundUnit.PerformCallback();
        calculateResultQty();
        getPopupDrugTariff();
        $('#divEntryProduct').show();
    });

    function onCbpPopupViewEndCallback(s) {
        hideLoadingPanel();
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else {
                $('#divEntryProduct').hide();
            }
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail')
                showToast('Delete Failed', 'Error Message : ' + param[2]);
        }
    }

    function onCbpPopupProcesEndCallback(s) {
        hideLoadingPanel();
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else {
                var transactionID = s.cpTransactionID;
                var parentID = s.cpParentID;
                var prescriptionOrderID = s.cpPrescriptionOrderID;
                $('#<%=hdnParentID.ClientID %>').val(parentID);
                $('#<%=hdnTransactionID.ClientID %>').val(transactionID);
                $('#<%=hdnPrescriptionOrderID.ClientID %>').val(prescriptionOrderID);
                $('#<%=hdnTransactionNo.ClientID %>').val(s.cpTransactionNo);
                $('#divEntryProduct').hide();
                onAfterSaveRecordDtSuccess(transactionID);
                cbpPopupView.PerformCallback('refresh');
            }
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail')
                showToast('Delete Failed', 'Error Message : ' + param[2]);
            else {
                var transactionID = s.cpTransactionID;
                var parentID = s.cpParentID;
                var prescriptionOrderID = s.cpPrescriptionOrderID
                $('#<%=hdnParentID.ClientID %>').val(parentID);
                $('#<%=hdnTransactionID.ClientID %>').val(transactionID);
                $('#<%=hdnPrescriptionOrderID.ClientID %>').val(prescriptionOrderID);
                $('#<%=hdnTransactionNo.ClientID %>').val(s.cpTransactionNo);
                cbpPopupView.PerformCallback('refresh');
            }
        }
    }

    //#region Item
    $('#lblProduct.lblLink').click(function () {
        var filterExpression = onGetDrugFilterExpression();
        openSearchDialog('druginfoitembalance', filterExpression, function (value) {
            $('#<%=txtItemCode.ClientID %>').val(value);
            ontxtItemCodeChanged(value);
        });
    });

    $('#<%=txtItemCode.ClientID %>').change(function () {
        ontxtItemCodeChanged($(this).val());
    });

    function ontxtItemCodeChanged(value) {
        var filterExpression = onGetDrugFilterExpression() + " AND ItemCode = '" + value + "'";
        Methods.getObject('GetvDrugInfoItemBalanceList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnItemID.ClientID %>').val(result.ItemID);
                $('#<%=txtItemName1.ClientID %>').val(result.ItemName1);
                $('#<%=hdnBaseUnit.ClientID %>').val(result.ItemUnit);
                $('#<%=hdnGCBaseUnit.ClientID %>').val(result.GCItemUnit);
                $('#<%=hdnGCItemUnit.ClientID %>').val(result.GCItemUnit);
                
                $('#<%=txtDose.ClientID %>').val(result.Dose);
                $('#<%=txtDoseUnit.ClientID %>').val(result.DoseUnit);
                $('#<%=hdnGCDoseUnit.ClientID %>').val(result.GCDoseUnit);
                $('#<%=hdnItemDose.ClientID %>').val(result.Dose);

                cboCompoundUnit.PerformCallback();
                getPopupDrugTariff();
            }
            else {
                $('#<%=hdnItemID.ClientID %>').val('');
                $('#<%=txtItemCode.ClientID %>').val('');
                $('#<%=txtItemName1.ClientID %>').val('');
                $('#<%=hdnBaseUnit.ClientID %>').val('');
                $('#<%=hdnGCBaseUnit.ClientID %>').val('');
                $('#<%=txtDose.ClientID %>').val('');
                $('#<%=txtDoseUnit.ClientID %>').val('');
                $('#<%=hdnGCDoseUnit.ClientID %>').val('');
                $('#<%=hdnItemDose.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Signa
    $('#lblPopupSigna.lblLink').click(function () {
        var filterExpression = "IsDeleted = 0";
        openSearchDialog('signa', filterExpression, function (value) {
            $('#<%=txtPopupSignaLabel.ClientID %>').val(value);
            txtSignaLabelChanged(value);
        });
    });

    $('#<%=txtPopupSignaLabel.ClientID %>').change(function () {
        txtSignaLabelChanged($(this).val());
    });

    function txtSignaLabelChanged(value) {
        var filterExpression = "IsDeleted = 0 AND SignaLabel = '" + value + "'";
        Methods.getObject('GetvSignaList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnPopupSignaID.ClientID %>').val(result.SignaID);
                $('#<%=txtPopupSignaName1.ClientID %>').val(result.SignaName1);
                $('#<%=txtFrequencyNumber.ClientID %>').val(result.Frequency);
                $('#<%=txtDosingDose.ClientID %>').val(result.Dose);
                cboPopupDrugForm.SetValue(result.GCDrugForm);
                cboFrequencyTimeline.SetValue(result.GCDosingFrequency);
                cboPopupCoenamRule.SetValue(result.GCCoenamRule);
                $('#<%=txtDosingDurationTimeline.ClientID %>').val(cboFrequencyTimeline.GetText());
                $('#<%=txtDispenseUnit.ClientID %>').val(result.DrugForm);
                $('#<%=txtTakenUnit.ClientID %>').val(result.DrugForm);
                cboPopupCoenamRule.SetValue(result.GCCoenamRule);
            } else {
                $('#<%=hdnPopupSignaID.ClientID %>').val('');
                $('#<%=txtPopupSignaName1.ClientID %>').val('');
                $('#<%=txtFrequencyNumber.ClientID %>').val('');
                $('#<%=txtDosingDose.ClientID %>').val('');
                cboPopupDrugForm.SetValue(result.GCDrugForm);
                cboFrequencyTimeline.SetValue('');
                cboPopupCoenamRule.SetValue('');
                $('#<%=txtDosingDurationTimeline.ClientID %>').val('');
                $('#<%=txtDispenseUnit.ClientID %>').val('');
                $('#<%=txtTakenUnit.ClientID %>').val('');
                cboPopupCoenamRule.SetValue('');
            }
        });
        calculateDispenseQty();
    }
    //#endregion

    $('#<%=txtDosingDose.ClientID %>').change(function () {
        calculateDispenseQty();
    });

    $('#<%=txtFrequencyNumber.ClientID %>').change(function () {
        calculateDispenseQty();
    });

    $('#<%=txtDosingDuration.ClientID %>').change(function () {
        calculateDispenseQty();
    });

    $('#<%=txtCompoundQty.ClientID %>').change(function () {
        calculateResultQty();
    });

    function calculateDispenseQty() {
        var dosing = $('#<%=txtDosingDose.ClientID %>').val();
        var duration = $('#<%=txtDosingDuration.ClientID %>').val();
        var frequency = $('#<%=txtFrequencyNumber.ClientID %>').val();
        var dispense = duration * frequency * dosing;

        $('#<%=txtDispenseQty.ClientID %>').val(dispense);
        $('#<%=txtTakenQty.ClientID %>').val(dispense);
    }

    function calculateResultQty() {
        var compoundQty = $('#<%=txtCompoundQty.ClientID %>').val();
        var resultQty = 0;
        var ItemUnit = $('#<%=hdnGCBaseUnit.ClientID %>').val();
        var takenQty = $('#<%=txtTakenQty.ClientID %>').val();
        var conversionFactor = parseFloat($('#<%=hdnConversionFactor.ClientID %>').val());
        resultQty = conversionFactor * takenQty * compoundQty;
        $('#<%=hdnResultQty.ClientID %>').val(resultQty);
        $('#<%=txtDtDispenseQty.ClientID %>').val(resultQty);

        calculatePopup();
    }

    //#region Patient and Payer change
    $('#<%=txtPopupPayerAmount.ClientID %>').die('change');
    $('#<%=txtPopupPayerAmount.ClientID %>').live('change', function () {
        var payer = parseFloat($(this).val());
        var lineAmount = parseFloat($('#<%=txtPopupLineAmount.ClientID %>').attr('hiddenVal'));
        var patient = lineAmount - payer;
        $('#<%=txtPopupPatientAmount.ClientID %>').val(patient).trigger('changeValue');
    });

    $('#<%=txtPopupPatientAmount.ClientID %>').die('change');
    $('#<%=txtPopupPatientAmount.ClientID %>').live('change', function () {
        var patient = parseFloat($(this).val());
        var lineAmount = parseFloat($('#<%=txtPopupLineAmount.ClientID %>').attr('hiddenVal'));
        var payer = lineAmount - patient;
        $('#<%=txtPopupPayerAmount.ClientID %>').val(payer).trigger('changeValue');
    });
    //#endregion

    //#region Get Popup Tariff
    function getPopupDrugTariff() {
        showLoadingPanel();
        var itemID = $('#<%=hdnItemID.ClientID %>').val();
        var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
        var visitID = $('#<%=hdnVisitID.ClientID %>').val();
        var classID = cboPopupChargeClass.GetValue();
        var trxDate = getTrxDate();
        if (itemID != "") {
            Methods.getItemTariff(registrationID, visitID, classID, itemID, trxDate, function (result) {
                if (result != null) {
                    $('#<%=txtPopupTariff.ClientID %>').val(result.Price).trigger('changeValue');
                    $('#<%=txtPopupDiscountAmount.ClientID %>').val(result.DiscountAmount);
                    $('#<%=hdnCoverageAmount.ClientID %>').val(result.CoverageAmount);
                    $('#<%=hdnDiscountInPercentage.ClientID %>').val(result.IsDiscountInPercentage ? '1' : '0');
                    $('#<%=hdnCoverageInPercentage.ClientID %>').val(result.IsCoverageInPercentage ? '1' : '0');
                    $('#<%=hdnBaseTariff.ClientID %>').val(result.BasePrice);
                    calculatePopup();
                }
                else {
                    $('#<%=txtPopupTariff.ClientID %>').val('');
                    $('#<%=txtPopupDiscountAmount.ClientID %>').val('');
                    $('#<%=hdnCoverageAmount.ClientID %>').val('');
                    $('#<%=hdnDiscountInPercentage.ClientID %>').val('');
                    $('#<%=hdnCoverageInPercentage.ClientID %>').val('');
                    $('#<%=hdnBaseTariff.ClientID %>').val('');
                }
            });
        }
        hideLoadingPanel();
    }

    function calculatePopup() {
        calculatePopupTotal();
        calculatePopupDiscount();
        calculatePopupAllTotal();
    }

    function calculatePopupTotal() {
        var resultQty = parseFloat($('#<%=hdnResultQty.ClientID %>').val());
        var price = parseFloat($('#<%=txtPopupTariff.ClientID %>').attr('hiddenVal'));
        var itemUnit = $('#<%=hdnGCBaseUnit.ClientID %>').val();
        var total = 0;
        total = resultQty * price;
        $('#<%=txtPopupTotalAmount.ClientID %>').val(total).trigger('changeValue');
    }

    function calculatePopupDiscount() {
        var discountAmount = parseFloat($('#<%=txtPopupDiscountAmount.ClientID %>').attr('hiddenVal'));
        var isDicountInPercentage = ($('#<%=hdnDiscountInPercentage.ClientID %>').val() == '1');

        var discountTotal = 0;
        if (discountAmount > 0) {
            var tariff = parseFloat($('#<%=txtPopupTotalAmount.ClientID %>').attr('hiddenVal'));
            if (isDicountInPercentage)
                discountTotal = (tariff * discountAmount) / 100;
            else {
                var qty = parseFloat($('#<%=txtCompoundQty.ClientID %>').val());
                discountTotal = discountAmount * qty;
            }
            if (discountTotal > tariff)
                discountTotal = tariff;
        }
        $('#<%=txtPopupDiscountAmount.ClientID %>').val(discountTotal).trigger('changeValue');
    }

    function calculatePopupAllTotal() {
        var tariff = $('#<%=txtPopupTotalAmount.ClientID %>').attr('hiddenVal');
        var discount = $('#<%=txtPopupDiscountAmount.ClientID %>').attr('hiddenVal');
        var embalace = parseFloat($('#<%=txtEmbalaceAmount.ClientID %>').attr('hiddenVal'));
        var total = tariff - discount + embalace;

        var coverageAmount = parseFloat($('#<%=hdnCoverageAmount.ClientID %>').val());
        var isCoverageInPercentage = ($('#<%=hdnCoverageInPercentage.ClientID %>').val() == '1');
        var totalPayer = 0;
        if (isCoverageInPercentage)
            totalPayer = (total * coverageAmount) / 100;
        else {
            var qty = parseFloat($('#<%=txtCompoundQty.ClientID %>').val());
            totalPayer = coverageAmount * qty;
        }

        if (total > 0 && totalPayer > total)
            totalPayer = total;

        var totalPatient = total - totalPayer;

        $('#<%=txtPopupPatientAmount.ClientID %>').val(totalPatient).trigger('changeValue');
        $('#<%=txtPopupPayerAmount.ClientID %>').val(totalPayer).trigger('changeValue');
        $('#<%=txtPopupLineAmount.ClientID %>').val(total).trigger('changeValue');
    }
    //#endregion

    //#region Embalace
    $('#lblPopupEmbalace.lblLink').die('click');
    $('#lblPopupEmbalace.lblLink').live('click', function () {
        openSearchDialog('embalace', "IsDeleted = 0", function (value) {
            $('#<%=txtEmbalaceCode.ClientID %>').val(value);
            ontxtEmbalaceCodeChanged(value);
        });
    });

    $('#<%=txtEmbalaceCode.ClientID %>').change(function () {
        ontxtEmbalaceCodeChanged($(this).val());
    });

    function ontxtEmbalaceCodeChanged(value) {
        var filterExpression = "IsDeleted = 0 AND EmbalaceCode = '" + value + "'";
        Methods.getObject('GetEmbalaceHdList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=txtEmbalaceName.ClientID %>').val(result.EmbalaceName);
                $('#<%=hdnEmbalaceID.ClientID %>').val(result.EmbalaceID);
            }
            else {
                $('#<%=txtEmbalaceName.ClientID %>').val('');
                $('#<%=txtEmbalaceCode.ClientID %>').val('');
                $('#<%=hdnEmbalaceID.ClientID %>').val('');
            }
            getPopupEmbalaceTariff();
        });
    }

    $('#<%=txtEmbalaceQty.ClientID %>').change(function () {
        getPopupEmbalaceTariff();
    });

    function getPopupEmbalaceTariff() {
        if ($('#<%=hdnEmbalaceID.ClientID %>').val() != '' && $('#<%=hdnEmbalaceID.ClientID %>').val() != '0') {
            var qty = parseInt($('#<%=txtEmbalaceQty.ClientID %>').val());
            var filterExpression = "EmbalaceID = " + $('#<%=hdnEmbalaceID.ClientID %>').val() + " AND StartingQty <= " + qty + " AND EndingQty >= " + qty;
            var embalacePrice = 0;
            Methods.getObject('GetEmbalaceDtList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnEmbalaceAmount.ClientID %>').val(result.Tariff);
                    $('#<%=txtEmbalaceAmount.ClientID %>').val($('#<%=hdnEmbalaceAmount.ClientID %>').val()).trigger('changeValue');
                }
                else {
                    $('#<%=hdnEmbalaceAmount.ClientID %>').val(0);
                    $('#<%=txtEmbalaceAmount.ClientID %>').val($('#<%=hdnEmbalaceAmount.ClientID %>').val()).trigger('changeValue');
                }
                calculatePopup();
            });
        }
    }
    //#endregion

    function cboPopupFrequencyTimelineChanged()
    {
        var duration = cboPopupFrequencyTimeline.GetText();
        $('#<%=txtDosingDurationTimeline.ClientID %>').val(duration);
    }

    function cboPopupDrugFormChanged() {
        var dispenseUnit = cboPopupDrugForm.GetText();
        $('#<%=txtDispenseUnit.ClientID %>').val(dispenseUnit);
    }
</script>
<input type="hidden" value="" id="hdnTransactionID" runat="server" />
<input type="hidden" value="" id="hdnTransactionNo" runat="server" />
<input type="hidden" value="" id="hdnPrescriptionOrderID" runat="server" />
<input type="hidden" value="" id="hdnPopupSignaID" runat="server" />
<input type="hidden" value="" id="hdnInlineEditingData" runat="server" />
<input type="hidden" value="" id="hdnQueryString" runat="server" />
<input type="hidden" value="" id="hdnFilterExpression" runat="server" />
<input type="hidden" value="" id="hdnIsAdd" runat="server" />
<input type="hidden" value="" id="hdnGCItemUnit" runat="server" />
<input type="hidden" value="" id="hdnGCBaseUnit" runat="server" />
<input type="hidden" value="" id="hdnPrescriptionOrderDetailID" runat="server" />
<input type="hidden" value="" id="hdnTransactionDetailID" runat="server" />
<input type="hidden" value="" id="hdnItemDose" runat="server" />
<input type="hidden" value="" id="hdnGCDoseUnit" runat="server" />
<input type="hidden" value="" id="hdnChargeClassID" runat="server" />
<input type="hidden" value="" id="hdnRegistrationID" runat="server" />
<input type="hidden" value="" id="hdnVisitID" runat="server" />
<input type="hidden" value="" id="hdnCoverageAmount" runat="server" />
<input type="hidden" value="" id="hdnDiscountInPercentage" runat="server" />
<input type="hidden" value="" id="hdnCoverageInPercentage" runat="server" />
<input type="hidden" value="" id="hdnBaseTariff" runat="server" />
<input type="hidden" value="" id="hdnBaseQuantity" runat="server" />
<input type="hidden" value="" id="hdnResultQty" runat="server" />
<input type="hidden" value="" id="hdnEmbalaceID" runat="server" />
<input type="hidden" value="" id="hdnPrescriptionFeeAmount" runat="server" />
<input type="hidden" value="" id="hdnLocationID" runat="server" />
<input type="hidden" value="" id="hdnPhysicianID" runat="server" />
<input type="hidden" value="" id="hdnDepartmentID" runat="server" />
<input type="hidden" value="" id="hdnDispensaryServiceUnitID" runat="server" />
<input type="hidden" value="" id="hdnImagingServiceUnitID" runat="server" />
<input type="hidden" value="" id="hdnLaboratoryServiceUnitID" runat="server" />
<input type="hidden" value="" id="hdnIsEditable" runat="server" />

<div style="height: 450px; overflow-y: auto">
    <table class="tblContentArea">
        <tr>
            <td>
                <table style="width:100%">
                    <colgroup>
                        <col style="width:50%"/>
                    </colgroup>
                    <tr>
                        <td valign="top">
                            <table style="width:100%">
                                <colgroup>
                                    <col style="width:150px"/>
                                </colgroup>
                                <tr>
                                    <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Nama Racikan")%></label></td>
                                    <td><asp:TextBox runat="server" ID="txtCompoundDrugName" Width="300px" /></td>
                                </tr>
                                <tr>
                                    <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Rute Obat")%></label></td>
                                    <td><dxe:ASPxComboBox runat="server" ID="cboMedicationRoute" Width="300px" /></td>
                                </tr>
                                <tr>
                                    <td class="tdLabel"><label class="lblNormal lblLink" id="lblPopupSigna"><%=GetLabel("Aturan Makan/Pakai")%></label></td>
                                    <td>
                                        <table cellpadding="0px" cellspacing="0px" width="300px">
                                            <colgroup>
                                                <col width="100px" />
                                                <col width="3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td><asp:TextBox runat="server" ID="txtPopupSignaLabel" Width="100%" /></td>
                                                <td>&nbsp;</td>
                                                <td><asp:TextBox runat="server" ID="txtPopupSignaName1" Width="100%" ReadOnly="true" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Bentuk")%></label></td>
                                    <td>
                                        <table border="0" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col width="120px" />
                                                <col width="100px" />
                                                <col width="80px" />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <dxe:ASPxComboBox ID="cboPopupDrugForm" ClientInstanceName="cboPopupDrugForm" runat="server" Width="120px">
                                                        <ClientSideEvents SelectedIndexChanged="function(s,e){cboPopupDrugFormChanged()}"/>
                                                    </dxe:ASPxComboBox>
                                                </td>
                                                <td style="padding-left:5px">
                                                    <label class="lblNormal"><%=GetLabel("Dosis")%>  
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDosingDose" runat="server" Width="100%" CssClass="number" />
                                                </td>
                                            </tr>
                                        </table>

                                    </td>
                                </tr>
                                <tr>
                                    <td><label class="lblNormal"><%=GetLabel("AC/DC/PC")%></label></td>
                                    <td>
                                        <dxe:ASPxComboBox runat="server" ID="cboPopupCoenamRule" ClientInstanceName="cboPopupCoenamRule" Width="120px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel"><label class="lblNormal lblLink" id="lblPopupEmbalace"><%=GetLabel("Embalase")%></label></td>
                                    <td>
                                        <input type="hidden" runat="server" id="hdnEmbalaceAmount" value="0"/>
                                        <table cellpadding="0" cellspacing="0" width="300px">
                                            <colgroup>
                                                <col style="width:100px"/>
                                                <col style="width:3px"/>
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td><asp:TextBox runat="server" ID="txtEmbalaceCode" Width="100%" /></td>
                                                <td>&nbsp;</td>
                                                <td><asp:TextBox runat="server" ID="txtEmbalaceName" Width="100%" ReadOnly="true" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Jumlah Embalase")%></label></td>
                                    <td><asp:TextBox runat="server" ID="txtEmbalaceQty" CssClass="number" Width="300px" /></td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top">
                            <table style="width:100%">
                                <colgroup>
                                    <col style="width:170px"/>
                                </colgroup>
                                <tr>
                                    <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Frekuensi")%></label></td>
                                    <td>
                                        <table cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width:100px"/>
                                                <col style="width:3px"/>
                                                <col style="width:100px"/>
                                            </colgroup>
                                            <tr>
                                                <td><asp:TextBox ID="txtFrequencyNumber" runat="server" Width="100%" CssClass="number" /></td>
                                                <td>&nbsp;</td>
                                                <td>
                                                    <dxe:ASPxComboBox ID="cboPopupFrequencyTimeline" ClientInstanceName="cboPopupFrequencyTimeline" runat="server" Width="100%">
                                                        <ClientSideEvents SelectedIndexChanged="function(s,e){cboPopupFrequencyTimelineChanged()}" />
                                                    </dxe:ASPxComboBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Mulai Diberikan")%></label></td>
                                    <td>
                                        <table cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width:140px"/>
                                                <col style="width:5px"/>
                                                <col style="width:80px"/>
                                            </colgroup>
                                            <tr>
                                                <td><asp:TextBox runat="server" ID="txtStartDate" CssClass="datepicker" Width="110px" /></td>
                                                <td>&nbsp;</td> 
                                                <td><asp:TextBox runat="server" ID="txtStartTime" CssClass="time" Width="100%" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Durasi")%></label></td>
                                    <td>
                                        <table cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width:100px"/>
                                                <col style="width:3px"/>
                                                <col style="width:100px"/>
                                            </colgroup>
                                            <tr>
                                                <td><asp:TextBox runat="server" ID="txtDosingDuration" Width="100px" CssClass="number" /></td>
                                                <td>&nbsp;</td>
                                                <td><asp:TextBox runat="server" ID="txtDosingDurationTimeline" Width="100px" ReadOnly="true"/></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Jumlah Diresepkan")%></label></td>
                                    <td>
                                        <table cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width:100px"/>
                                                <col style="width:3px"/>
                                                <col style="width:100px"/>
                                            </colgroup>
                                            <tr>
                                                <td><asp:TextBox runat="server" ID="txtDispenseQty" Width="100px" CssClass="number" /></td>
                                                <td>&nbsp;</td>
                                                <td><asp:TextBox runat="server" ID="txtDispenseUnit" Width="100px" ReadOnly="true" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Jumlah Diambil")%></label></td>
                                    <td>
                                        <table cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width:100px"/>
                                                <col style="width:3px"/>
                                                <col style="width:100px"/>
                                            </colgroup>
                                            <tr>
                                                <td><asp:TextBox runat="server" ID="txtTakenQty" Width="100px" CssClass="number" /></td>
                                                <td>&nbsp;</td>
                                                <td><asp:TextBox runat="server" ID="txtTakenUnit" Width="100px" ReadOnly="true" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel" style="vertical-align: top; padding-top: 5px;"><label class="lblNormal"><%=GetLabel("Catatan Khusus")%></label></td>
                                    <td><asp:TextBox ID="txtMedicationAdministration" Width="300px" runat="server" TextMode="MultiLine" /></td>
                                </tr>
                                <tr>
                                    <td class="tdLabel"></td>
                                    <td><asp:CheckBox runat="server" ID="chkIsUsingSweetener" Text="Tambahkan pemanis" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <div style="display:none;" id="divEntryProduct">
                    <div class="pageTitle"><%=GetLabel("Komposisi Racikan")%></div>
                    <table class="tblEntryDetail">
                        <colgroup>
                            <col style="width:50%" />
                            <col />
                        </colgroup>
                        <tr>
                            <td valign="top">
                                <table class="tblEntryDetail" >
                                    <colgroup>
                                        <col width="30%" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td><label class="lblMandatory"><%=GetLabel("Nama Generik")%></label></td>
                                        <td><asp:TextBox runat="server" ID="txtGenericName" Width="100%" /></td>
                                    </tr>
                                    <tr>
                                        <td><label class="lblNormal lblLink" id="lblProduct"><%=GetLabel("Obat")%></label></td>
                                        <td>
                                            <table cellpadding="0px" cellspacing="0px" width="100%" >
                                                <tr>
                                                    <td style="width:100px">
                                                        <input type="hidden" id="hdnItemID" runat="server" value="" />
                                                        <asp:TextBox runat="server" ID="txtItemCode" Width="100%" />
                                                    </td>
                                                    <td>&nbsp;</td>
                                                    <td><asp:TextBox runat="server" ID="txtItemName1" ReadOnly="true" Width="100%" /></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2"><div class="lblComponent" ><%=GetLabel("Kadar")%></div></td>
                                    </tr>
                                    <tr>
                                        <td><label class="lblNormal"><%=GetLabel("Jumlah")%></label></td>
                                        <td>
                                            <table cellpadding="0px" cellspacing="0px" width="100%" >
                                                <tr>
                                                    <td><asp:TextBox runat="server" ID="txtDose" CssClass="number" Width="100%" ReadOnly="true" /></td>
                                                    <td>&nbsp;</td>
                                                    <td><asp:TextBox runat="server" ID="txtDoseUnit" Width="100%" ReadOnly="true" /></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2"><div class="lblComponent" ><%=GetLabel("Kadar Racikan")%></div></td>
                                    </tr>
                                    <tr>
                                        <td><label class="lblNormal"><%=GetLabel("Jumlah")%></label></td>
                                        <td>
                                            <table cellpadding="0px" cellspacing="0px" width="100%">
                                                <colgroup>
                                                    <col width="120px"/>
                                                </colgroup>
                                                <tr>
                                                    <td><asp:TextBox runat="server" ID="txtCompoundQty" CssClass="number" Width="100%" /></td>
                                                    <td>&nbsp;</td>
                                                    <td>
                                                        <input type="hidden" runat="server" id="hdnBaseUnit" value="" />
                                                        <dxe:ASPxComboBox runat="server" ID="cboCompoundUnit" ClientInstanceName="cboCompoundUnit"
                                                            OnCallback="cboCompoundUnit_Callback"  Width="100%" >
                                                            <ClientSideEvents ValueChanged="function(s,e){ onCboCompoundUnitValueChanged(); }" 
                                                                EndCallback="function(s,e){ onCboCompoundUnitEndCallback() }" />
                                                        </dxe:ASPxComboBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><label class="lblNormal"><%=GetLabel("Faktor Konversi") %></label></td>
                                        <td>
                                            <input type="hidden" id="hdnConversionFactor" runat="server" />
                                            <asp:TextBox runat="server" ID="txtConversionFactor" ReadOnly="true" Width="100%"  />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><label class="lblNormal"><%=GetLabel("Jumlah yang digunakan") %></label></td>
                                        <td>
                                            <table cellpadding="0px" cellspacing="0px" width="100%">
                                                <tr>
                                                    <td><asp:TextBox runat="server" ID="txtDtDispenseQty" CssClass="number" ReadOnly="true" Width="100%" /></td>
                                                    <td>&nbsp;</td>
                                                    <td><asp:TextBox runat="server" ID="txtDtDispenseUnit" Width="100%" ReadOnly="true" /></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <input type="button" id="btnPopupSave" value='<%= GetLabel("Save")%>' />
                                                    </td>
                                                    <td>
                                                        <input type="button" id="btnPopupCancel" value='<%= GetLabel("Cancel")%>' />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td valign="top">
                                <table class="tblEntryDetail" >
                                    <colgroup>
                                        <col style="width:90px"/>
                                        <col />
                                    </colgroup>
                                    <tr style="display:none">
                                        <td><label class="lblNormal"><%=GetLabel("Charge Class")%></label></td>
                                        <td>
                                            <dxe:ASPxComboBox runat="server" ID="cboPopupChargeClass" ClientInstanceName="cboPopupChargeClass" Width="120px">
                                                <ClientSideEvents SelectedIndexChanged="function(s,e){getPopupDrugTariff();}" />
                                            </dxe:ASPxComboBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><label class="lblNormal"><%=GetLabel("Harga Satuan") %></label></td>
                                        <td><asp:TextBox runat="server" ID="txtPopupTariff" CssClass="txtCurrency" Width="120px" ReadOnly="true" /></td>
                                    </tr>
                                    <tr>
                                        <td><label class="lblNormal"><%=GetLabel("HARGA") %></label></td>
                                        <td>
                                            <table width="100%">
                                                <colgroup>
                                                    <col width="33%" />
                                                    <col width="33%"  />
                                                    <col />
                                                </colgroup>
                                                <tr>
                                                    <td><div class="lblComponent"><%=GetLabel("Harga") %></div></td>
                                                    <td><div class="lblComponent"><%=GetLabel("Discount") %></div></td>
                                                    <td><div class="lblComponent"><%=GetLabel("Embalace") %></div></td>
                                                </tr>
                                                <tr>
                                                    <td><asp:TextBox runat="server" ID="txtPopupTotalAmount" CssClass="txtCurrency" ReadOnly="true" Width="100%" /></td>        
                                                    <td><asp:TextBox runat="server" ID="txtPopupDiscountAmount" CssClass="txtCurrency" ReadOnly="true" Width="100%" /></td>
                                                    <td><asp:TextBox runat="server" ID="txtEmbalaceAmount" CssClass="txtCurrency" ReadOnly="true" Width="100%" /></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><label class="lblNormal"><%=GetLabel("Total") %></label></td>
                                        <td>
                                            <table width="100%">
                                                <colgroup>
                                                    <col width="33%" />
                                                    <col width="33%"  />
                                                    <col />
                                                </colgroup>
                                                <tr>
                                                    <td><div class="lblComponent"><%=GetLabel("Pasien") %></div></td>
                                                    <td><div class="lblComponent"><%=GetLabel("Instansi") %></div></td>
                                                    <td><div class="lblComponent"><%=GetLabel("Total") %></div></td>
                                                </tr>
                                                <tr>
                                                    <td><asp:TextBox runat="server" ID="txtPopupPatientAmount" CssClass="txtCurrency" Width="100%" /></td>
                                                    <td><asp:TextBox runat="server" ID="txtPopupPayerAmount" CssClass="txtCurrency" Width="100%" /></td>
                                                    <td><asp:TextBox runat="server" ID="txtPopupLineAmount" CssClass="txtCurrency"  ReadOnly="true" Width="100%" /></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <dxcp:ASPxCallbackPanel ID="cbpPopupView" runat="server" Width="100%" ClientInstanceName="cbpPopupView"
                    ShowLoadingPanel="false" OnCallback="cbpPopupView_Callback" >
                    <ClientSideEvents BeginCallback="function() { showLoadingPanel(); }"
                        EndCallback="function(s,e){ onCbpPopupViewEndCallback(s) }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="max-height: 300px">
                                <input type="hidden" value="" id="hdnParentID" runat="server" />
                                <asp:ListView ID="lvwView" runat="server">
                                    <EmptyDataTemplate>
                                        <table class="grdNormal" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width:90px"/>
                                                <col/>
                                                <col style="width:70px"/>
                                                <col style="width:100px"/>      
                                                <col style="width:70px"/>
                                                <col style="width:100px"/>
                                                <col style="width:170px"/>
                                            </colgroup>
                                            <tr>
                                                <th align="center" rowspan="2">#</th>
                                                <th align="left" rowspan="2"><%=GetLabel("Obat")%></th>
                                                <th align="center" colspan="2"><%=GetLabel("Kadar")%></th>
                                                <th align="center" colspan="4"><%=GetLabel("Komposisi Racikan")%></th>
                                            </tr>
                                            <tr>
                                                <th align="center"><%=GetLabel("Kadar")%></th>
                                                <th align="center"><%=GetLabel("Satuan")%></th>
                                                <th align="center"><%=GetLabel("Jumlah")%></th>
                                                <th align="center"><%=GetLabel("Satuan")%></th>
                                                <th align="center"><%=GetLabel("Formula Konversi")%></th>
                                                <th align="center"><%=GetLabel("Hasil")%></th>
                                            </tr>
                                            <tr>
                                                <td colspan="8" align="center" style="height:45px; vertical-align:middle">
                                                    <%=GetLabel("Daftar Komposisi Racikan")%>
                                                </td>
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                    <LayoutTemplate>
                                        <table class="grdNormal" runat="server" id="tblView" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width:90px"/>
                                                <col/>
                                                <col style="width:70px"/>
                                                <col style="width:100px"/>
                                                <col style="width:70px"/>
                                                <col style="width:100px"/>
                                                <col style="width:70px"/>
                                                <col style="width:100px"/>
                                                <col style="width:100px"/>
                                                <col style="width:100px"/>
                                                <col style="width:100px"/>
                                            </colgroup>
                                            <tr>
                                                <th align="center" rowspan="2">#</th>
                                                <th align="left"   rowspan="2"><%=GetLabel("Obat")%></th>
                                                <th align="center" colspan="2"><%=GetLabel("Kadar")%></th>
                                                <th align="center" colspan="4"><%=GetLabel("Komposisi Racikan")%></th>
                                                <th align="center" colspan="3"><%=GetLabel("Jumlah") %></th>
                                            </tr>
                                            <tr>
                                                <th align="center"><%=GetLabel("Kadar")%></th>
                                                <th align="center"><%=GetLabel("Satuan")%></th>
                                                <th align="center"><%=GetLabel("Jumlah")%></th>
                                                <th align="center"><%=GetLabel("Satuan")%></th>
                                                <th align="center"><%=GetLabel("Formula Konversi")%></th>
                                                <th align="center"><%=GetLabel("Hasil")%></th>
                                                <th align="center"><%=GetLabel("Instansi") %></th>
                                                <th align="center"><%=GetLabel("Pasien") %></th>
                                                <th align="center"><%=GetLabel("Total") %></th>
                                            </tr>
                                            <tr runat="server" id="itemPlaceholder" ></tr>
                                            <tr class="trFooter">  
                                                <td colspan="8"><div style="text-align:right;padding:0px 3px"><%=GetLabel("Total")%></div></td>
                                                <td><div style="text-align:right;padding:0px 3px" id="tdTotalAllPayer" runat="server">Instansi</div></td>
                                                <td><div style="text-align:right;padding:0px 3px" id="tdTotalAllPatient" runat="server">Pasien</div></td>
                                                <td><div style="text-align:right;padding:0px 3px" id="tdTotalAll" runat="server">Total</div></td>
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td align="center">
                                                <img class="imgPopupEdit imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>' alt="" />
                                                <img class="imgPopupDelete <%# IsEditable() == "0" ? "imgDisabled" : "imgLink"%>" src='<%# IsEditable() == "0" ? ResolveUrl("~/Libs/Images/Button/delete_disabled.png") : ResolveUrl("~/Libs/Images/Button/delete.png")%>' alt="" />
                                            </td>
                                            <td style="display:none">
                                                <input type="hidden" value="<%#:Eval("PrescriptionOrderDetailID") %>" bindingfield="PrescriptionOrderDetailID" />
                                                <input type="hidden" value="<%#:Eval("TransactionDetailID") %>" bindingfield="TransactionDetailID" />
                                                <input type="hidden" value="<%#:Eval("GenericName") %>" bindingfield="GenericName" />
                                                <input type="hidden" value="<%#:Eval("ItemID") %>" bindingfield="ItemID" />
                                                <input type="hidden" value="<%#:Eval("ItemCode") %>" bindingfield="ItemCode" />
                                                <input type="hidden" value="<%#:Eval("ItemName1") %>" bindingfield="ItemName1" />
                                                <input type="hidden" value="<%#:Eval("GCItemUnit") %>" bindingfield="GCItemUnit" />
                                                <input type="hidden" value="<%#:Eval("ItemUnit") %>" bindingfield="ItemUnit" />
                                                <input type="hidden" value="<%#:Eval("GCBaseUnit") %>" bindingfield="GCBaseUnit" />
                                                <input type="hidden" value="<%#:Eval("BaseUnit") %>" bindingfield="BaseUnit" />
                                                <input type="hidden" value="<%#:Eval("ItemDose") %>" bindingfield="ItemDose" />

                                                <input type="hidden" value="<%#:Eval("Dose") %>" bindingfield="Dose" />
                                                <input type="hidden" value="<%#:Eval("GCDoseUnit") %>" bindingfield="GCDoseUnit" />
                                                <input type="hidden" value="<%#:Eval("DoseUnit") %>" bindingfield="DoseUnit" />
                                                
                                                <input type="hidden" value="<%#:Eval("CompoundQty") %>" bindingfield="CompoundQty" />
                                                <input type="hidden" value="<%#:Eval("TakenQty") %>" bindingfield="TakenQty" />
                                                <input type="hidden" value="<%#:Eval("GCCompoundUnit") %>" bindingfield="GCCompoundUnit" />
                                                <input type="hidden" value="<%#:Eval("ParentID") %>" bindingfield="ParentID" />
                                                <input type="hidden" value="<%#:Eval("ConversionFactor") %>" bindingfield="ConversionFactor">
                                                
                                                <input type="hidden" value="<%#:Eval("EmbalaceAmount") %>" bindingfield="EmbalaceAmount" />
                                                <input type="hidden" value="<%#:Eval("ChargeClassID") %>" bindingfield="ChargeClassID" />
                                                <input type="hidden" value="<%#:Eval("DiscountAmount") %>" bindingfield="DisocuntAmount" />
                                                <input type="hidden" value="<%#:Eval("PatientAmount") %>" bindingfield="PatientAmount" />
                                                <input type="hidden" value="<%#:Eval("PayerAmount") %>" bindingfield="PayerAmount" />
                                                <input type="hidden" value="<%#:Eval("LineAmount") %>" bindingfield="LineAmount" />
                                                <input type="hidden" value="<%#:Eval("BaseTariff") %>" bindingfield="BaseTariff" />
                                                <input type="hidden" value="<%#:Eval("Tariff") %>" bindingfield="Tariff" />
                                            </td>
                                            <td>
                                                <%#:Eval("ItemName1") %>
                                                <%#:Eval("GenericName") %>
                                            </td>
                                            <td align="right"><%#:Eval("DoseInString") %></td>
                                            <td align="center"><%#:Eval("DoseUnit") %></td>
                                            <td align="right" style="color:Blue"><%#:Eval("CompoundQtyInString") %></td>
                                            <td align="center" style="color:Blue"><%#:Eval("CompoundUnit") %></td>
                                            <td align="center"><%#:Eval("CustomConversion")%></td>
                                            <td align="right" style="color:Red"><%#:Eval("ResultQtyInString")%></td>
                                            <td align="right"><%#:Eval("PayerAmount","{0:N}")%></td>
                                            <td align="right"><%#:Eval("PatientAmount","{0:N}")%></td>
                                            <td align="right"><%#:Eval("LineAmount","{0:N}")%></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                                <div style="width:100%;text-align:center" id="divContainerAddData" runat="server">
                                    <span class="lblLink" id="lblPopupAddData"><%= GetLabel("Tambah Obat")%></span>
                                </div>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div class="imgLoadingGrdView" id="containerImgLoadingView" >
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
            </td>
        </tr>
    </table>

    <dxcp:ASPxCallbackPanel ID="cbpPopupProcess" runat="server" Width="100%" ClientInstanceName="cbpPopupProcess"
        ShowLoadingPanel="false" OnCallback="cbpPopupProcess_Callback">
        <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }"
            EndCallback="function(s,e) { onCbpPopupProcesEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</div>
   

