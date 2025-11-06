<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PatientManagementTransactionDetailServiceCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientManagementTransactionDetailServiceCtl" %>
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
        if ($('#<%=txtExpiredDate.ClientID %>').attr('readonly') == null) {
            setDatePicker('<%=txtExpiredDate.ClientID %>');
            $('#<%:txtExpiredDate.ClientID %>').datepicker('option', 'minDate', '-' + 0);
        }

        if ($('#<%=hdnIsEditable.ClientID %>').val() == '1') {
            if ($('#<%=hdnDepartmentID.ClientID %>').val() == 'OUTPATIENT') {
                $('#lblServiceAddData').show();
                $('#lblServiceQuickPick').show();
                $('#lblTemplatePanel').hide();
                $('#lblCopyMultiVisitScheduleOrder').hide();
                $('#lblServicePackageQuickPick').hide();
                $('#lblServiceAIOTransactionQuickPick').hide();
            }
            else {
                $('#lblServiceAddData').show();
                $('#lblServiceQuickPick').show();
                $('#lblTemplatePanel').show();
                $('#lblCopyMultiVisitScheduleOrder').hide();
                $('#lblServicePackageQuickPick').hide();
                $('#lblServiceAIOTransactionQuickPick').hide();
            }

            if ($('#<%=hdnDepartmentID.ClientID %>').val() == 'OUTPATIENT' || $('#<%=hdnDepartmentID.ClientID %>').val() == 'DIAGNOSTIC') {
                if ('<%=SA0198() %>' == "1") {
                    $('#lblServicePackageQuickPick').show();
                }
                else {
                    $('#lblServicePackageQuickPick').hide();
                }
            }
            else {
                $('#lblServicePackageQuickPick').hide();
            }

            if ($('#<%=hdnIsAIOTransactionServiceCtl.ClientID %>').val() == '1') {
                $('#lblServiceAddData').hide();
                $('#lblServiceQuickPick').hide();
                $('#lblTemplatePanel').hide();
                $('#lblCopyMultiVisitScheduleOrder').hide();
                $('#lblServicePackageQuickPick').hide();
                $('#lblServiceAIOTransactionQuickPick').show();
            }

            if ($('#<%=hdnIsChargesGenerateMCUServiceCtl.ClientID %>').val() == '1') {
                $('#lblServiceAddData').hide();
                $('#lblServiceQuickPick').hide();
                $('#lblTemplatePanel').hide();
                $('#lblCopyMultiVisitScheduleOrder').hide();
                $('#lblServicePackageQuickPick').hide();
                $('#lblServiceAIOTransactionQuickPick').hide();
            }
        }
        else {
            $('#lblServiceAddData').hide();
            $('#lblServiceQuickPick').hide();
            $('#lblTemplatePanel').hide();
            $('#lblCopyMultiVisitScheduleOrder').hide();
            $('#lblServicePackageQuickPick').hide();
            $('#lblServiceAIOTransactionQuickPick').hide();
        }

        $('#btnServiceSave').click(function (evt) {
            var isBalanceItem = $('#<%=hdnIsPackageBalanceItem.ClientID %>').val();
            var maxQty = $('#<%=txtPackageQty.ClientID %>').val();
            var qty = $('#<%=txtPackageQtyTaken.ClientID %>').val();

            if (IsValid(evt, 'fsTrxService', 'mpTrxService')) {
                if ($('#<%=hdnServiceItemID.ClientID %>').val() == $('#<%=hdnPrescriptionReturnItem.ClientID %>').val() && parseFloat($('#<%=txtServiceQty.ClientID %>').val()) >= 0) {
                    showToast('Warning', 'Jumlah Pelayanan Retur Resep Harus Minus');
                }
                else {
                    var isAllowSave = "1";
                    var oID = $('#<%=hdnServiceTransactionDtID.ClientID %>').val();
                    if (oID != null && oID != "" && oID != "0") {
                        isAllowSave = "1";
                    } else {
                        var today = new Date();
                        var dd = String(today.getDate()).padStart(2, '0');
                        var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
                        var yyyy = today.getFullYear();
                        today = yyyy + mm + dd;

                        var oTransactionDate = today;
                        if (oTransactionDate != null && oTransactionDate != "") {
                            var oCountData = 0;
                            var oVisitID = $('#<%=hdnVisitIDCtl.ClientID %>').val();
                            var filterChargesDt = "VisitID = " + oVisitID + " AND CONVERT(VARCHAR(8), TransactionDate, 112) = '" + oTransactionDate + "' AND ItemID IN (" + $('#<%=hdnServiceItemID.ClientID %>').val() + ")";
                            Methods.getObject('GetvPatientChargesDtCheckItemDoubleList', filterChargesDt, function (resultDouble) {
                                if (resultDouble != null) {
                                    oCountData = parseInt(resultDouble.CountDt);
                                } else {
                                    oCountData = 0;
                                }
                            });

                            if (oCountData > 0) {
                                var isConfirm = confirm("Item yang dipilih sudah ada dibuat di tanggal " + dd + "-" + mm + "-" + yyyy + ", lanjutkan proses simpan?");
                                if (isConfirm == false) {
                                    isAllowSave = "0";
                                }
                            }
                        }
                    }

                    if (!isBalanceItem) {

                    }
                    else {
                        var qtySisa = maxQty - qty;
                        if (qtySisa >= 0) {
                            var expiredDate = $('#<%:txtExpiredDate.ClientID %>').val();
                            var expiredDateInDatePicker = Methods.getDatePickerDate(expiredDate);
                            var expirednDateFormatString = Methods.dateToString(expiredDateInDatePicker);

                            var defaultDate = $('#<%:hdnDateToday.ClientID %>').val();
                            var defaultDateInDatePicker = Methods.getDatePickerDate(defaultDate);
                            var defaultDateFormatString = Methods.dateToString(defaultDateInDatePicker);

                            if (expiredDateInDatePicker >= defaultDateInDatePicker) {

                            }
                            else {
                                isAllowSave = "0";
                                showToast('Warning', 'Tanggal Kadaluarsa sudah lewat');
                            }
                        }
                        else {
                            isAllowSave = "0";
                            showToast('Warning', 'Jumlah yang diambil Melebihi Jumlah Paket');
                        }
                    }

                    if (isAllowSave == "1") {
                        cbpService.PerformCallback('save');
                    }
                }
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
                var serviceUnitID = $('#<%=hdnHealthcareServiceUnitIDServiceCtl.ClientID %>').val();
                var labServiceUnitID = $('#<%=hdnLaboratoryServiceUnitID.ClientID %>').val();
                var radiologyServiceUnitID = $('#<%=hdnImagingServiceUnitID.ClientID %>').val();
                var isLabUnit = $('#<%=hdnIsLaboratoryUnit.ClientID %>').val();
                var url = '';
                var width = 0;

                if (serviceUnitID == labServiceUnitID || serviceUnitID == radiologyServiceUnitID) {
                    url = ResolveUrl('~/Libs/Program/Module/PatientManagement/Transaction/ChargesLabQuickPicksCtl.ascx');
                    width = 1200;
                }
                else if (isLabUnit == "1") {
                    url = ResolveUrl('~/Libs/Program/Module/PatientManagement/Transaction/ChargesLabQuickPicksCtl.ascx');
                    width = 1200;
                }
                else {
                    url = ResolveUrl('~/Libs/Program/Module/PatientManagement/Transaction/ServiceQuickPicksCtl.ascx');
                    width = 1200;
                }
                var transactionID = getTransactionHdID();
                var serviceUnitID = getHealthcareServiceUnitID();
                var registrationID = getRegistrationID();
                var visitID = getVisitID();
                var departmentID = getDepartmentID();
                var isAccompany = "0";

                var today = new Date();
                var tempDate = "00";
                if (today.getDate() < 10) {
                    tempDate = "0" + today.getDate();
                } else {
                    tempDate = today.getDate();
                }
                var pad = "00";
                var tmpMonth = (today.getMonth() + 1).toString();
                var month = pad.substring(0, pad.length - tmpMonth.length) + tmpMonth;
                var date = tempDate + "-" + month + "-" + today.getFullYear();
                var transactionDate = $('#<%=hdnTransactionDateServiceCtl.ClientID %>').val() == "" ? date : $('#<%=hdnTransactionDateServiceCtl.ClientID %>').val();
                var chkPATest = 0;

                if (typeof getchkIsPATest != 'undefined')
                    chkPATest = getchkIsPATest();

                if (typeof isAccompanyChargesPage == 'function') {
                    if (isAccompanyChargesPage()) {
                        isAccompany = "1";
                    }
                }

                var id = transactionID + '|' + serviceUnitID + '|' + visitID + '|' + registrationID + '|' + departmentID + '|' + isAccompany + '|' + transactionDate + '|' + chkPATest;
                openUserControlPopup(url, id, 'Quick Picks', width, 600);
            }
        });

        $('#lblCopyMultiVisitScheduleOrder').live('click', function () {
            if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
                showLoadingPanel();
                var serviceUnitID = $('#<%=hdnHealthcareServiceUnitIDServiceCtl.ClientID %>').val();
                var labServiceUnitID = $('#<%=hdnLaboratoryServiceUnitID.ClientID %>').val();
                var radiologyServiceUnitID = $('#<%=hdnImagingServiceUnitID.ClientID %>').val();
                var isLabUnit = $('#<%=hdnIsLaboratoryUnit.ClientID %>').val();
                var url = '';
                var width = 0;

                url = ResolveUrl('~/Libs/Program/Module/PatientManagement/Transaction/MultiVisitQuickPicksCtl.ascx');
                width = 1200;

                var transactionID = getTransactionHdID();
                var serviceUnitID = getHealthcareServiceUnitID();
                var registrationID = getRegistrationID();
                var visitID = getVisitID();
                var departmentID = getDepartmentID();
                var isAccompany = "0";

                var today = new Date();
                var tempDate = "00";
                if (today.getDate() < 10) {
                    tempDate = "0" + today.getDate();
                } else {
                    tempDate = today.getDate();
                }
                var pad = "00";
                var tmpMonth = (today.getMonth() + 1).toString();
                var month = pad.substring(0, pad.length - tmpMonth.length) + tmpMonth;
                var date = tempDate + "-" + month + "-" + today.getFullYear();
                var transactionDate = $('#<%=hdnTransactionDateServiceCtl.ClientID %>').val() == "" ? date : $('#<%=hdnTransactionDateServiceCtl.ClientID %>').val();
                var chkPATest = 0;

                if (typeof getchkIsPATest != 'undefined')
                    chkPATest = getchkIsPATest();

                if (typeof isAccompanyChargesPage == 'function') {
                    if (isAccompanyChargesPage()) {
                        isAccompany = "1";
                    }
                }

                var id = transactionID + '|' + serviceUnitID + '|' + visitID + '|' + registrationID + '|' + departmentID + '|' + isAccompany + '|' + transactionDate + '|' + chkPATest;
                openUserControlPopup(url, id, 'Quick Picks Multi Kunjungan', width, 600);
            }
        });

        $('#lblServicePackageQuickPick').live('click', function () {
            if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
                showLoadingPanel();
                var url = ResolveUrl('~/Libs/Program/Module/PatientManagement/Transaction/ServicePackageQuickPicksCtl.ascx');
                var width = 1200;

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
                openUserControlPopup(url, id, 'Paket Kunjungan', width, 600);
            }
        });

        $('#lblServiceAIOTransactionQuickPick').live('click', function () {
            if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
                showLoadingPanel();
                var url = ResolveUrl('~/Libs/Program/Module/PatientManagement/Transaction/PatientManagementTransactionCopyDetailAIO.ascx');
                var width = 1200;

                var transactionID = getTransactionHdID();
                var serviceUnitID = getHealthcareServiceUnitID();
                var registrationID = getRegistrationID();
                var visitID = getVisitID();
                var departmentID = getDepartmentID();
                var isAccompany = "0";

                var today = new Date();
                var tempDate = "00";
                if (today.getDate() < 10) {
                    tempDate = "0" + today.getDate();
                } else {
                    tempDate = today.getDate();
                }
                var pad = "00";
                var tmpMonth = (today.getMonth() + 1).toString();
                var month = pad.substring(0, pad.length - tmpMonth.length) + tmpMonth;
                var date = tempDate + "-" + month + "-" + today.getFullYear();
                var transactionDate = $('#<%=hdnTransactionDateServiceCtl.ClientID %>').val() == "" ? date : $('#<%=hdnTransactionDateServiceCtl.ClientID %>').val();

                if (typeof isAccompanyChargesPage == 'function') {
                    if (isAccompanyChargesPage()) {
                        isAccompany = "1";
                    }
                }
                var id = transactionID + '|' + serviceUnitID + '|' + visitID + '|' + registrationID + '|' + departmentID + '|' + isAccompany + '|' + transactionDate;
                openUserControlPopup(url, id, 'Salin Detail AIO', width, 600);
            }
        });

        $('#lblTemplatePanel').live('click', function () {
            if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
                showLoadingPanel();
                var url = ResolveUrl('~/Libs/Program/Module/PatientManagement/Transaction/TemplateChargesQuickPicksCtl2.ascx');
                var width = 1000;
                var transactionID = getTransactionHdID();
                var serviceUnitID = getHealthcareServiceUnitID();
                var registrationID = getRegistrationID();
                var visitID = getVisitID();
                var departmentID = getDepartmentID();
                var id = transactionID + '|' + serviceUnitID + '|' + visitID + '|' + registrationID + '|' + departmentID;
                openUserControlPopup(url, id, 'Template', width, 600);
            }
        });

        $('#<%=chkServiceIsVariable.ClientID %>').change(function () {
            if ($(this).is(':checked')) {
                $('#<%=chkServiceIsUnbilledItem.ClientID %>').prop('checked', false);
                $('#<%=txtServicePatient.ClientID %>').removeAttr('readonly');
                if ($('#<%=hdnBusinessPartnerIDServiceCtl.ClientID %>').val() != "1") {
                    $('#<%=txtServicePayer.ClientID %>').removeAttr('readonly');
                }

                var hdnIsAllowVariableTariffComp1 = $('#<%=hdnIsAllowVariableTariffComp1.ClientID %>').val();
                var hdnIsAllowVariableTariffComp2 = $('#<%=hdnIsAllowVariableTariffComp2.ClientID %>').val();
                var hdnIsAllowVariableTariffComp3 = $('#<%=hdnIsAllowVariableTariffComp3.ClientID %>').val();

                var countIsVariable = 0;

                if (hdnIsAllowVariableTariffComp1 == "1") {
                    countIsVariable = countIsVariable + 1;
                    $('#<%=txtServiceTariffComp1.ClientID %>').removeAttr('readonly');
                } else {
                    $('#<%=txtServiceTariffComp1.ClientID %>').attr('readonly', 'readonly');
                }

                if (hdnIsAllowVariableTariffComp2 == "1") {
                    countIsVariable = countIsVariable + 1;
                    $('#<%=txtServiceTariffComp2.ClientID %>').removeAttr('readonly');
                } else {
                    $('#<%=txtServiceTariffComp2.ClientID %>').attr('readonly', 'readonly');
                }

                if (hdnIsAllowVariableTariffComp3 == "1") {
                    countIsVariable = countIsVariable + 1;
                    $('#<%=txtServiceTariffComp3.ClientID %>').removeAttr('readonly');
                } else {
                    $('#<%=txtServiceTariffComp3.ClientID %>').attr('readonly', 'readonly');
                }

                $('#<%=trItemNameVariable.ClientID %>').removeAttr('style');
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

                $('#<%=trItemNameVariable.ClientID %>').attr('style', 'display:none');

                var qty = parseFloat($('#<%=txtServiceQty.ClientID %>').val());
                var priceComp1 = parseFloat($('#<%=hdnServicePriceComp1.ClientID %>').val());
                var priceComp2 = parseFloat($('#<%=hdnServicePriceComp2.ClientID %>').val());
                var priceComp3 = parseFloat($('#<%=hdnServicePriceComp3.ClientID %>').val());
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
            var patientTotal = parseFloat($(this).val());
            var total = parseFloat($('#<%=txtServiceTotal.ClientID %>').attr('hiddenVal'));
            var payerTotal = 0;
            if ($('#<%=hdnBusinessPartnerIDServiceCtl.ClientID %>').val() != "1") {
                payerTotal = total - patientTotal;
            } else {
                $('#<%=txtServicePatient.ClientID %>').val(total).trigger('changeValue');
            }
            $('#<%=txtServicePayer.ClientID %>').val(payerTotal).trigger('changeValue');
        });

        $('#<%=txtServicePayer.ClientID %>').change(function () {
            var payerTotal = parseFloat($(this).val());
            var total = parseFloat($('#<%=txtServiceTotal.ClientID %>').attr('hiddenVal'));
            var patientTotal = total - payerTotal;
            $('#<%=txtServicePatient.ClientID %>').val(patientTotal).trigger('changeValue');
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

        $('#<%=txtServiceQty.ClientID %>').change(function () {
            $('#<%=chkServiceIsDiscount.ClientID %>').prop('checked', false);
            $('#<%=txtServiceDiscount.ClientID %>').attr('readonly', 'readonly');

            var qty = parseFloat($('#<%=txtServiceQty.ClientID %>').val());
            
            var isQtyChecked = "0";
            var isUsingValidateDigitDecimal = $('#<%=hdnIsUsingValidateDigitDecimal.ClientID %>').val();

            if ($('#<%=txtServiceQty.ClientID %>').val().includes(".")) {
                var qtyCheckDesimalList = $('#<%=txtServiceQty.ClientID %>').val().split(".");
                var qtyCheckDesimal = qtyCheckDesimalList[1];
                if (qtyCheckDesimal.length > 2 && isUsingValidateDigitDecimal == "1") {
                    isQtyChecked = "1";
                }
            }

            if (isQtyChecked == "1") {
                alert("Maksimal digit desimal belakang koma adalah 2 digit.");
                $('#<%=txtServiceQty.ClientID %>').val("1");
            } else {
                var priceComp1 = parseFloat($('#<%=txtServiceTariffComp1.ClientID %>').attr('hiddenVal'));
                var priceComp2 = parseFloat($('#<%=txtServiceTariffComp2.ClientID %>').attr('hiddenVal'));
                var priceComp3 = parseFloat($('#<%=txtServiceTariffComp3.ClientID %>').attr('hiddenVal'));
                var cito = parseFloat($('#<%=txtServiceCITO.ClientID %>').attr('hiddenVal'));

                $('#txtServiceDiscTariffComp1').val(priceComp1).trigger('changeValue');
                $('#txtServiceDiscTariffComp2').val(priceComp2).trigger('changeValue');
                $('#txtServiceDiscTariffComp3').val(priceComp3).trigger('changeValue');
                $('#txtServiceCITOAmount').val(cito).trigger('changeValue');

                calculateServiceTariffTotal();

                $('#<%=chkServiceIsCITO.ClientID %>').change();
                $('#<%=chkServiceIsComplication.ClientID %>').change();

                calculateServiceDiscountTotal();
                calculateServiceTotal();
            }
        });

        //#region Unit Tariff Component
        $('#<%=txtServiceTotalTariff.ClientID %>').change(function () {
            $(this).blur();
            var total = parseFloat($(this).attr('hiddenVal'));
            var baseTariff = parseFloat($('#<%=hdnServiceBaseTariff.ClientID %>').val());

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
                $total += parseFloat($(this).attr('hiddenVal'));
            });
            $('#<%=txtServiceTotalTariff.ClientID %>').val($total).trigger('changeValue');
            $('#<%=txtServiceTotalTariff.ClientID %>').change();
        }

        $('#btnEditUnitTariff').click(function () {
            $('#<%=txtServiceTotalTariff.ClientID %>').val($('#<%=txtServiceUnitTariff.ClientID %>').attr('hiddenVal')).trigger('changeValue');
            if ($('#<%=chkServiceIsVariable.ClientID %>').prop('checked')) {
                var hdnIsAllowVariableTariffComp1 = $('#<%=hdnIsAllowVariableTariffComp1.ClientID %>').val();
                var hdnIsAllowVariableTariffComp2 = $('#<%=hdnIsAllowVariableTariffComp2.ClientID %>').val();
                var hdnIsAllowVariableTariffComp3 = $('#<%=hdnIsAllowVariableTariffComp3.ClientID %>').val();

                if (hdnIsAllowVariableTariffComp1 == "1") {
                    $('#<%=txtServiceTariffComp1.ClientID %>').removeAttr('readonly');
                } else {
                    $('#<%=txtServiceTariffComp1.ClientID %>').attr('readonly', 'readonly');
                }
                if (hdnIsAllowVariableTariffComp2 == "1") {
                    $('#<%=txtServiceTariffComp2.ClientID %>').removeAttr('readonly');
                } else {
                    $('#<%=txtServiceTariffComp2.ClientID %>').attr('readonly', 'readonly');
                }
                if (hdnIsAllowVariableTariffComp3 == "1") {
                    $('#<%=txtServiceTariffComp3.ClientID %>').removeAttr('readonly');
                } else {
                    $('#<%=txtServiceTariffComp3.ClientID %>').attr('readonly', 'readonly');
                }
            } else {
                $('#<%=txtServiceTariffComp1.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtServiceTariffComp2.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtServiceTariffComp3.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtServiceTotalTariff.ClientID %>').attr('readonly', 'readonly');

                $('#<%=txtServiceUnitTariff.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtServicePatient.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtServicePayer.ClientID %>').attr('readonly', 'readonly');

                $('#<%=trItemNameVariable.ClientID %>').attr('style', 'display:none');
            }
            pcUnitTariff.Show();
        });
        //#endregion

        //#region Discount Component
        $('#<%=txtServiceDiscTotal.ClientID %>').change(function () {
            $(this).blur();
            var total = parseFloat($(this).attr('hiddenVal'));

            var hdnIsAllowDiscountTariffComp1 = $('#<%=hdnIsAllowDiscountTariffComp1.ClientID %>').val();
            var hdnIsAllowDiscountTariffComp2 = $('#<%=hdnIsAllowDiscountTariffComp2.ClientID %>').val();
            var hdnIsAllowDiscountTariffComp3 = $('#<%=hdnIsAllowDiscountTariffComp3.ClientID %>').val();

            var tariffComp1 = parseFloat($('#<%=txtServiceTariffComp1.ClientID %>').attr('hiddenVal') * $('#<%=txtServiceQty.ClientID %>').val());
            var tariffComp2 = parseFloat($('#<%=txtServiceTariffComp2.ClientID %>').attr('hiddenVal') * $('#<%=txtServiceQty.ClientID %>').val());
            var tariffComp3 = parseFloat($('#<%=txtServiceTariffComp3.ClientID %>').attr('hiddenVal') * $('#<%=txtServiceQty.ClientID %>').val());
            var tariffCITO = parseFloat($('#txtServiceCITOAmount').attr('hiddenVal'));

            $totalTariffCITO = parseFloat($('#<%=txtServiceTariff.ClientID %>').attr('hiddenVal')) + parseFloat($('#<%=txtServiceCITO.ClientID %>').attr('hiddenVal'));
            if (parseFloat($('#<%=txtServiceDiscTotal.ClientID %>').attr('hiddenVal')) > $totalTariffCITO) {
                total = $totalTariffCITO;
            }

            var hdnDisc1 = $('#<%=hdnDiscComp1.ClientID %>').val();
            var hdnDisc2 = $('#<%=hdnDiscComp2.ClientID %>').val();
            var hdnDisc3 = $('#<%=hdnDiscComp3.ClientID %>').val();
            var hdnDiscCito = $('#<%=hdnDiscCompCito.ClientID %>').val();

            var discountComp1 = 0;
            var discountComp2 = 0;
            var discountComp3 = 0;
            var discountCITO = 0;

            var tempDiscountTotal = total;
            var tempServiceTotal = tariffComp1 + tariffComp2 + tariffComp3;

            if ((hdnDisc1 == "0.00" || hdnDisc1 == "") && (hdnDisc2 == "0.00" || hdnDisc2 == "") && (hdnDisc3 == "0.00" || hdnDisc3 == "")) {
                discountComp1 = tariffComp1 / tempServiceTotal * total;
                discountComp2 = tariffComp2 / tempServiceTotal * total;
                discountComp3 = tariffComp3 / tempServiceTotal * total;
                discountCITO = tariffCITO / tempServiceTotal * total;
            } else {
                discountComp1 = hdnDisc1;
                discountComp2 = hdnDisc2;
                discountComp3 = hdnDisc3;
                discountCITO = hdnDiscCito;
            }

            if (isChangeFromServiceDiscount) {
                discountComp1 = tariffComp1 / tempServiceTotal * total;
                discountComp2 = tariffComp2 / tempServiceTotal * total;
                discountComp3 = tariffComp3 / tempServiceTotal * total;
                discountCITO = tariffCITO / tempServiceTotal * total;
            }

            hdnDisc1 = Math.round(discountComp1);
            hdnDisc2 = Math.round(discountComp2);
            hdnDisc3 = Math.round(discountComp3);
            hdnDiscCito = Math.round(discountCITO);

            if (hdnIsAllowDiscountTariffComp1 == "1") {
                $('#<%=hdnDiscComp1.ClientID %>').val(hdnDisc1);
                $('#<%=txtServiceDiscComp1.ClientID %>').val(hdnDisc1).trigger('changeValue');
                $('#txtServiceAfterTariffComp1').val($('#txtServiceDiscTariffComp1').attr('hiddenVal') - $('#<%=txtServiceDiscComp1.ClientID %>').attr('hiddenVal')).trigger('changeValue');
            }

            if (hdnIsAllowDiscountTariffComp2 == "1") {
                $('#<%=hdnDiscComp2.ClientID %>').val(hdnDisc2);
                $('#<%=txtServiceDiscComp2.ClientID %>').val(hdnDisc2).trigger('changeValue');
                $('#txtServiceAfterTariffComp2').val($('#txtServiceDiscTariffComp2').attr('hiddenVal') - $('#<%=txtServiceDiscComp2.ClientID %>').attr('hiddenVal')).trigger('changeValue');
            }

            if (hdnIsAllowDiscountTariffComp3 == "1") {
                $('#<%=hdnDiscComp3.ClientID %>').val(hdnDisc3);
                $('#<%=txtServiceDiscComp3.ClientID %>').val(hdnDisc3).trigger('changeValue');
                $('#txtServiceAfterTariffComp3').val($('#txtServiceDiscTariffComp3').attr('hiddenVal') - $('#<%=txtServiceDiscComp3.ClientID %>').attr('hiddenVal')).trigger('changeValue');
            }

            $('#<%=hdnDiscCompCito.ClientID %>').val(hdnDiscCito);
            $('#<%=txtServiceCITODisc.ClientID %>').val(hdnDiscCito).trigger('changeValue');
            $('#txtServiceAfterCITOAmount').val($('#txtServiceCITOAmount').attr('hiddenVal') - $('#<%=txtServiceCITODisc.ClientID %>').attr('hiddenVal')).trigger('changeValue');

            $('#<%=txtServiceDiscTotal.ClientID %>').val(total).trigger('changeValue');

            calculateServiceTotal();

        });

        $('#<%=txtServiceDiscPercentComp1.ClientID %>').change(function () {
            $(this).blur();
            var totalPercentComp1 = parseFloat($(this).attr('hiddenVal'));

            if (totalPercentComp1 > 100 || totalPercentComp1 < 0) {
                $('#<%=txtServiceDiscPercentComp1.ClientID %>').val(0);
                totalPercentComp1 = 0;
            }

            var qty = parseFloat($('#<%=txtServiceQty.ClientID %>').val().split(',').join(''));

            var hdnIsAllowDiscountTariffComp1 = $('#<%=hdnIsAllowDiscountTariffComp1.ClientID %>').val();

            var tariffComp1ORI = parseFloat($('#<%=txtServiceTariffComp1.ClientID %>').attr('hiddenVal').split(',').join(''));

            var tariffComp1 = parseFloat($('#<%=txtServiceTariffComp1.ClientID %>').attr('hiddenVal').split(',').join('') * qty);

            var hdnDisc1 = $('#<%=hdnDiscComp1.ClientID %>').val();

            var discountComp1 = 0;

            if (totalPercentComp1 > 0) {
                discountComp1 = parseFloat(totalPercentComp1 * tariffComp1ORI / 100).toFixed(2);

                hdnDisc1 = Math.round(discountComp1);

                if (hdnIsAllowDiscountTariffComp1 == "1") {
                    $('#<%=hdnDiscComp1.ClientID %>').val(hdnDisc1);
                    $('#<%=txtServiceDiscComp1.ClientID %>').val(hdnDisc1).trigger('changeValue');
                    $('#txtServiceAfterTariffComp1').val($('#txtServiceDiscTariffComp1').attr('hiddenVal') - $('#<%=txtServiceDiscComp1.ClientID %>').attr('hiddenVal')).trigger('changeValue');
                } else {
                    $('#<%=txtServiceDiscPercentComp1.ClientID %>').val(0).trigger('changeValue');
                }
            } else {
                $('#<%=txtServiceDiscPercentComp1.ClientID %>').val(0).trigger('changeValue');

                hdnDisc1 = Math.round(discountComp1);

                if (hdnIsAllowDiscountTariffComp1 == "1") {
                    $('#<%=hdnDiscComp1.ClientID %>').val(hdnDisc1);
                    $('#<%=txtServiceDiscComp1.ClientID %>').val(hdnDisc1).trigger('changeValue');
                    $('#txtServiceAfterTariffComp1').val($('#txtServiceDiscTariffComp1').attr('hiddenVal') - $('#<%=txtServiceDiscComp1.ClientID %>').attr('hiddenVal')).trigger('changeValue');
                } else {
                    $('#<%=txtServiceDiscPercentComp1.ClientID %>').val(0).trigger('changeValue');
                }
            }

            var tempDiscountTotal = parseFloat($('#<%=txtServiceDiscComp1.ClientID %>').val().split(',').join(''))
                                        + parseFloat($('#<%=txtServiceDiscComp2.ClientID %>').val().split(',').join(''))
                                        + parseFloat($('#<%=txtServiceDiscComp3.ClientID %>').val().split(',').join(''))
                                        + parseFloat($('#<%=txtServiceCITODisc.ClientID %>').val().split(',').join(''));

            tempDiscountTotal = tempDiscountTotal * qty;

            $('#<%=txtServiceDiscTotal.ClientID %>').val(tempDiscountTotal).trigger('changeValue');
            $('#<%=txtServiceDiscount.ClientID %>').val(tempDiscountTotal).trigger('changeValue');

            calculateServiceTotal();
        });

        $('#<%=txtServiceDiscPercentComp2.ClientID %>').change(function () {
            $(this).blur();
            var totalPercentComp2 = parseFloat($(this).attr('hiddenVal'));

            if (totalPercentComp2 > 100 || totalPercentComp2 < 0) {
                $('#<%=txtServiceDiscPercentComp2.ClientID %>').val(0);
                totalPercentComp2 = 0;
            }

            var qty = parseFloat($('#<%=txtServiceQty.ClientID %>').val().split(',').join(''));

            var hdnIsAllowDiscountTariffComp2 = $('#<%=hdnIsAllowDiscountTariffComp2.ClientID %>').val();

            var tariffComp2ORI = parseFloat($('#<%=txtServiceTariffComp2.ClientID %>').attr('hiddenVal').split(',').join(''));

            var tariffComp2 = parseFloat($('#<%=txtServiceTariffComp2.ClientID %>').attr('hiddenVal').split(',').join('') * qty);

            var hdnDisc2 = $('#<%=hdnDiscComp2.ClientID %>').val();

            var discountComp2 = 0;

            if (totalPercentComp2 > 0) {
                discountComp2 = parseFloat(totalPercentComp2 * tariffComp2ORI / 100).toFixed(2);

                hdnDisc2 = Math.round(discountComp2);

                if (hdnIsAllowDiscountTariffComp2 == "1") {
                    $('#<%=hdnDiscComp2.ClientID %>').val(hdnDisc2);
                    $('#<%=txtServiceDiscComp2.ClientID %>').val(hdnDisc2).trigger('changeValue');
                    $('#txtServiceAfterTariffComp2').val($('#txtServiceDiscTariffComp2').attr('hiddenVal') - $('#<%=txtServiceDiscComp2.ClientID %>').attr('hiddenVal')).trigger('changeValue');
                } else {
                    $('#<%=txtServiceDiscPercentComp2.ClientID %>').val(0).trigger('changeValue');
                }
            } else {
                $('#<%=txtServiceDiscPercentComp2.ClientID %>').val(0).trigger('changeValue');

                hdnDisc2 = Math.round(discountComp2);

                if (hdnIsAllowDiscountTariffComp2 == "1") {
                    $('#<%=hdnDiscComp2.ClientID %>').val(hdnDisc2);
                    $('#<%=txtServiceDiscComp2.ClientID %>').val(hdnDisc2).trigger('changeValue');
                    $('#txtServiceAfterTariffComp2').val($('#txtServiceDiscTariffComp2').attr('hiddenVal') - $('#<%=txtServiceDiscComp2.ClientID %>').attr('hiddenVal')).trigger('changeValue');
                } else {
                    $('#<%=txtServiceDiscPercentComp2.ClientID %>').val(0).trigger('changeValue');
                }
            }

            var tempDiscountTotal = parseFloat($('#<%=txtServiceDiscComp1.ClientID %>').val().split(',').join(''))
                                        + parseFloat($('#<%=txtServiceDiscComp2.ClientID %>').val().split(',').join(''))
                                        + parseFloat($('#<%=txtServiceDiscComp3.ClientID %>').val().split(',').join(''))
                                        + parseFloat($('#<%=txtServiceCITODisc.ClientID %>').val().split(',').join(''));

            tempDiscountTotal = tempDiscountTotal * qty;

            $('#<%=txtServiceDiscTotal.ClientID %>').val(tempDiscountTotal).trigger('changeValue');
            $('#<%=txtServiceDiscount.ClientID %>').val(tempDiscountTotal).trigger('changeValue');

            calculateServiceTotal();
        });

        $('#<%=txtServiceDiscPercentComp3.ClientID %>').change(function () {
            $(this).blur();
            var totalPercentComp3 = parseFloat($(this).attr('hiddenVal'));

            if (totalPercentComp3 > 100 || totalPercentComp3 < 0) {
                $('#<%=txtServiceDiscPercentComp3.ClientID %>').val(0);
                totalPercentComp3 = 0;
            }

            var qty = parseFloat($('#<%=txtServiceQty.ClientID %>').val().split(',').join(''));

            var hdnIsAllowDiscountTariffComp3 = $('#<%=hdnIsAllowDiscountTariffComp3.ClientID %>').val();

            var tariffComp3ORI = parseFloat($('#<%=txtServiceTariffComp3.ClientID %>').attr('hiddenVal').split(',').join(''));

            var tariffComp3 = parseFloat($('#<%=txtServiceTariffComp3.ClientID %>').attr('hiddenVal').split(',').join('') * qty);

            var hdnDisc3 = $('#<%=hdnDiscComp3.ClientID %>').val();

            var discountComp3 = 0;

            if (totalPercentComp3 > 0) {
                discountComp3 = parseFloat(totalPercentComp3 * tariffComp3ORI / 100).toFixed(2);

                hdnDisc3 = Math.round(discountComp3);

                if (hdnIsAllowDiscountTariffComp3 == "1") {
                    $('#<%=hdnDiscComp3.ClientID %>').val(hdnDisc3);
                    $('#<%=txtServiceDiscComp3.ClientID %>').val(hdnDisc3).trigger('changeValue');
                    $('#txtServiceAfterTariffComp3').val($('#txtServiceDiscTariffComp3').attr('hiddenVal') - $('#<%=txtServiceDiscComp3.ClientID %>').attr('hiddenVal')).trigger('changeValue');
                } else {
                    $('#<%=txtServiceDiscPercentComp3.ClientID %>').val(0).trigger('changeValue');
                }
            } else {
                $('#<%=txtServiceDiscPercentComp3.ClientID %>').val(0).trigger('changeValue');

                hdnDisc3 = Math.round(discountComp3);

                if (hdnIsAllowDiscountTariffComp3 == "1") {
                    $('#<%=hdnDiscComp3.ClientID %>').val(hdnDisc3);
                    $('#<%=txtServiceDiscComp3.ClientID %>').val(hdnDisc3).trigger('changeValue');
                    $('#txtServiceAfterTariffComp3').val($('#txtServiceDiscTariffComp3').attr('hiddenVal') - $('#<%=txtServiceDiscComp3.ClientID %>').attr('hiddenVal')).trigger('changeValue');
                } else {
                    $('#<%=txtServiceDiscPercentComp3.ClientID %>').val(0).trigger('changeValue');
                }
            }

            var tempDiscountTotal = parseFloat($('#<%=txtServiceDiscComp1.ClientID %>').val().split(',').join(''))
                                        + parseFloat($('#<%=txtServiceDiscComp2.ClientID %>').val().split(',').join(''))
                                        + parseFloat($('#<%=txtServiceDiscComp3.ClientID %>').val().split(',').join(''))
                                        + parseFloat($('#<%=txtServiceCITODisc.ClientID %>').val().split(',').join(''));

            tempDiscountTotal = tempDiscountTotal * qty;

            $('#<%=txtServiceDiscTotal.ClientID %>').val(tempDiscountTotal).trigger('changeValue');
            $('#<%=txtServiceDiscount.ClientID %>').val(tempDiscountTotal).trigger('changeValue');

            calculateServiceTotal();
        });

        $('.txtDiscComp').change(function () {
            $(this).blur();
            $base = parseFloat($(this).closest('tr').find('.txtUnitTariffPrev').attr('hiddenVal'));
            $disc = parseFloat($(this).attr('hiddenVal'));
            if ($disc > $base)
                $disc = $base;
            $(this).val($disc).trigger('changeValue');
            calculateDiscountTotal();
        });

        function calculateDiscountTotal() {
            $total = 0;
            $('.txtDiscComp').each(function () {
                $base = parseFloat($(this).closest('tr').find('.txtUnitTariffPrev').attr('hiddenVal'));
                $disc = parseFloat($(this).attr('hiddenVal') * $('#<%=txtServiceQty.ClientID %>').val());
                $discPerOne = parseFloat($(this).attr('hiddenVal'));
                $after = $base - $discPerOne;
                $total += parseFloat($disc);
                $(this).closest('tr').find('.txtUnitTariffAfter').val($after).trigger('changeValue');
            });
            $('#<%=txtServiceDiscTotal.ClientID %>').val($total).trigger('changeValue');
            $('#<%=txtServiceDiscount.ClientID %>').val($total).trigger('changeValue');

            calculateServiceTotal();
        }

        $('#<%:chkServiceIsDiscount.ClientID %>').live('change', function () {
            if ($(this).is(':checked')) {
            }
            else {
                $('#<%=txtServiceDiscPercentComp1.ClientID %>').val(0).trigger('changeValue');
                $('#<%=txtServiceDiscPercentComp2.ClientID %>').val(0).trigger('changeValue');
                $('#<%=txtServiceDiscPercentComp3.ClientID %>').val(0).trigger('changeValue');
                $('#<%=txtServiceDiscComp1.ClientID %>').val(0).trigger('changeValue');
                $('#<%=txtServiceDiscComp2.ClientID %>').val(0).trigger('changeValue');
                $('#<%=txtServiceDiscComp3.ClientID %>').val(0).trigger('changeValue');
                $('#<%=txtServiceCITODisc.ClientID %>').val(0).trigger('changeValue');

                $('#<%=txtServiceDiscTotal.ClientID %>').val(0).trigger('changeValue');
                $('#<%=txtServiceDiscount.ClientID %>').val(0).trigger('changeValue');

                $('#<%=txtServiceDiscPercentComp1.ClientID %>').val(0).trigger('change');
                $('#<%=txtServiceDiscPercentComp2.ClientID %>').val(0).trigger('change');
                $('#<%=txtServiceDiscPercentComp3.ClientID %>').val(0).trigger('change');
                $('#<%=txtServiceDiscComp1.ClientID %>').val(0).trigger('change');
                $('#<%=txtServiceDiscComp2.ClientID %>').val(0).trigger('change');
                $('#<%=txtServiceDiscComp3.ClientID %>').val(0).trigger('change');
                $('#<%=txtServiceCITODisc.ClientID %>').val(0).trigger('change');

                $('#<%=txtServiceDiscTotal.ClientID %>').val(0).trigger('change');
                $('#<%=txtServiceDiscount.ClientID %>').val(0).trigger('change');

                calculateServiceTotal();
            }
        });

        $('.btnEditDiscount').click(function () {
            var isPackageItem = $('#<%=hdnIsPackageItem.ClientID %>').val();
            var isUsingAccumulatedPriceItem = $('#<%=hdnIsUsingAccumulatedPriceItem.ClientID %>').val();
            if (isPackageItem == "1" && isUsingAccumulatedPriceItem == "1") {
                $('#<%=txtServiceDiscPercentComp1.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtServiceDiscPercentComp2.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtServiceDiscPercentComp3.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtServiceDiscComp1.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtServiceDiscComp2.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtServiceDiscComp3.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtServiceCITODisc.ClientID %>').attr('readonly', 'readonly');
            }
            else {
                if ($('#<%=chkServiceIsDiscount.ClientID %>').prop('checked')) {

                    $('#<%=txtServiceDiscPercentComp1.ClientID %>').removeAttr('readonly');
                    $('#<%=txtServiceDiscPercentComp2.ClientID %>').removeAttr('readonly');
                    $('#<%=txtServiceDiscPercentComp3.ClientID %>').removeAttr('readonly');

                    var hdnIsAllowDiscountTariffComp1 = $('#<%=hdnIsAllowDiscountTariffComp1.ClientID %>').val();
                    var hdnIsAllowDiscountTariffComp2 = $('#<%=hdnIsAllowDiscountTariffComp2.ClientID %>').val();
                    var hdnIsAllowDiscountTariffComp3 = $('#<%=hdnIsAllowDiscountTariffComp3.ClientID %>').val();

                    var countIsDiscount = 0;

                    if (hdnIsAllowDiscountTariffComp1 == "1") {
                        countIsDiscount = countIsDiscount + 1;
                        $('#<%=txtServiceDiscPercentComp1.ClientID %>').removeAttr('readonly');
                        $('#<%=txtServiceDiscComp1.ClientID %>').removeAttr('readonly');
                    } else {
                        $('#<%=txtServiceDiscPercentComp1.ClientID %>').attr('readonly', 'readonly');
                        $('#<%=txtServiceDiscComp1.ClientID %>').attr('readonly', 'readonly');
                    }

                    if (hdnIsAllowDiscountTariffComp2 == "1") {
                        countIsDiscount = countIsDiscount + 1;
                        $('#<%=txtServiceDiscPercentComp2.ClientID %>').removeAttr('readonly');
                        $('#<%=txtServiceDiscComp2.ClientID %>').removeAttr('readonly');
                    } else {
                        $('#<%=txtServiceDiscPercentComp2.ClientID %>').attr('readonly', 'readonly');
                        $('#<%=txtServiceDiscComp2.ClientID %>').attr('readonly', 'readonly');
                    }

                    if (hdnIsAllowDiscountTariffComp3 == "1") {
                        countIsDiscount = countIsDiscount + 1;
                        $('#<%=txtServiceDiscPercentComp3.ClientID %>').removeAttr('readonly');
                        $('#<%=txtServiceDiscComp3.ClientID %>').removeAttr('readonly');
                    } else {
                        $('#<%=txtServiceDiscPercentComp3.ClientID %>').attr('readonly', 'readonly');
                        $('#<%=txtServiceDiscComp3.ClientID %>').attr('readonly', 'readonly');
                    }

                    if (countIsDiscount == 1) {
                        $('#<%=txtServiceDiscTotal.ClientID %>').removeAttr('readonly');
                    } else {
                        $('#<%=txtServiceDiscTotal.ClientID %>').attr('readonly', 'readonly');
                    }

                    $('#<%=txtServiceCITODisc.ClientID %>').removeAttr('readonly');
                }
                else {
                    $('#<%=txtServiceDiscPercentComp1.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtServiceDiscPercentComp2.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtServiceDiscPercentComp3.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtServiceDiscComp1.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtServiceDiscComp2.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtServiceDiscComp3.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtServiceCITODisc.ClientID %>').attr('readonly', 'readonly');
                }
            }

            var comp1 = parseFloat($('#<%=txtServiceTariffComp1.ClientID %>').attr('hiddenVal'));
            var comp2 = parseFloat($('#<%=txtServiceTariffComp2.ClientID %>').attr('hiddenVal'));
            var comp3 = parseFloat($('#<%=txtServiceTariffComp3.ClientID %>').attr('hiddenVal'));
            var cito = parseFloat($('#<%=txtServiceCITO.ClientID %>').attr('hiddenVal'));

            var qty = parseFloat($('#<%=txtServiceQty.ClientID %>').val());

            $('#txtServiceDiscTariffComp1').val(comp1).trigger('changeValue');
            $('#txtServiceDiscTariffComp2').val(comp2).trigger('changeValue');
            $('#txtServiceDiscTariffComp3').val(comp3).trigger('changeValue');
            $('#txtServiceCITOAmount').val(cito).trigger('changeValue');

            $('#txtServiceAfterTariffComp1').val($('#txtServiceDiscTariffComp1').attr('hiddenVal') - $('#<%=txtServiceDiscComp1.ClientID %>').attr('hiddenVal')).trigger('changeValue');
            $('#txtServiceAfterTariffComp2').val($('#txtServiceDiscTariffComp2').attr('hiddenVal') - $('#<%=txtServiceDiscComp2.ClientID %>').attr('hiddenVal')).trigger('changeValue');
            $('#txtServiceAfterTariffComp3').val($('#txtServiceDiscTariffComp3').attr('hiddenVal') - $('#<%=txtServiceDiscComp3.ClientID %>').attr('hiddenVal')).trigger('changeValue');
            $('#txtServiceAfterCITOAmount').val($('#txtServiceCITOAmount').attr('hiddenVal') - $('#<%=txtServiceCITODisc.ClientID %>').attr('hiddenVal')).trigger('changeValue');

            $('#<%=txtServiceDiscTotal.ClientID %>').val($('#<%=txtServiceDiscount.ClientID %>').attr('hiddenVal')).trigger('changeValue');
            $('#<%=txtServiceDiscTotal.ClientID %>').change();

            pcDiscount.Show();
        });

        $('#<%=txtServiceDiscComp1.ClientID %>').change(function () {
            var disc1 = parseFloat($(this).attr('hiddenVal'));
            $('#<%=hdnDiscComp1.ClientID %>').val(disc1);
            $('#<%=txtServiceDiscPercentComp1.ClientID %>').val(0).trigger('changeValue');
        });

        $('#<%=txtServiceDiscComp2.ClientID %>').change(function () {
            var disc2 = parseFloat($(this).attr('hiddenVal'));
            $('#<%=hdnDiscComp2.ClientID %>').val(disc2);
            $('#<%=txtServiceDiscPercentComp2.ClientID %>').val(0).trigger('changeValue');
        });

        $('#<%=txtServiceDiscComp3.ClientID %>').change(function () {
            var disc3 = parseFloat($(this).attr('hiddenVal'));
            $('#<%=hdnDiscComp3.ClientID %>').val(disc3);
            $('#<%=txtServiceDiscPercentComp3.ClientID %>').val(0).trigger('changeValue');
        });

        $('#<%=txtServiceCITODisc.ClientID %>').change(function () {
            var discCito = parseFloat($(this).attr('hiddenVal'));
            $('#<%=hdnDiscCompCito.ClientID %>').val(discCito);
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
                //                if ('<%=SA0198() %>' == "1") {
                //                    $('#lblServicePackageQuickPick').show();
                //                }
                //                else {
                //                    $('#lblServicePackageQuickPick').hide();
                //                }
                $('#lblServicePackageQuickPick').hide();
                setCustomToolbarVisibility();
            }
        }
        if (param[0] == 'editAIO') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else {
                var transactionID = s.cpTransactionID;
                onAfterSaveRecordDtSuccess(transactionID);
                $('#containerEntryService').hide();
                //                if ('<%=SA0198() %>' == "1") {
                //                    $('#lblServicePackageQuickPick').show();
                //                }
                //                else {
                //                    $('#lblServicePackageQuickPick').hide();
                //                }
                $('#lblServicePackageQuickPick').hide();
                pcAIOEdit.Hide();
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

        if ($('#<%=hdnIsAIOTransactionServiceCtl.ClientID %>').val() == '1') {
            $('#lblServiceAddData').hide();
            $('#lblServiceQuickPick').hide();
            $('#lblTemplatePanel').hide();
            $('#lblServiceAIOTransactionQuickPick').show();
            $('#lblServicePackageQuickPick').hide();
        }
        else if ($('#<%=hdnIsChargesGenerateMCUServiceCtl.ClientID %>').val() == '1') {
            $('#lblServiceAddData').hide();
            $('#lblServiceQuickPick').hide();
            $('#lblTemplatePanel').hide();
            $('#lblServiceAIOTransactionQuickPick').hide();
            $('#lblServicePackageQuickPick').hide();
        }
        else {
            $('#lblServiceAddData').show();
            $('#lblServiceQuickPick').show();
            $('#lblTemplatePanel').show();
            $('#lblServiceAIOTransactionQuickPick').hide();

            if ($('#<%=hdnDepartmentID.ClientID %>').val() == 'OUTPATIENT' || $('#<%=hdnDepartmentID.ClientID %>').val() == 'DIAGNOSTIC') {
                if ('<%=SA0198() %>' == "1") {
                    $('#lblServicePackageQuickPick').show();
                }
                else {
                    $('#lblServicePackageQuickPick').hide();
                }
            }
            else {
                $('#lblServicePackageQuickPick').hide();
            }
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
        if ($('#<%=hdnIsChargesGenerateMCUServiceCtl.ClientID %>').val() == "1") {
            displayErrorMessageBox("WARNING", "Maaf, transaksi hasil Generate MCU tidak dapat diubah.");
        } else {
            $row = $(this).closest('tr').parent().closest('tr');
            showDeleteConfirmation(function (data) {
                var obj = rowToObject($row);
                var param = 'delete|' + obj.ID + ';' + data.GCDeleteReason + ';' + data.Reason;
                cbpService.PerformCallback(param);
            });
        }
    });

    $('#<%:txtPackageQtyTaken.ClientID %>').live('change', function () {
        var value = $(this).val();
        $(this).val(checkMinus(value)).trigger('changeValue');
        var valueBase = $('#<%:txtPackageQty.ClientID %>').val();

        if (isNaN(value)) {
            $('#<%:txtPackageQtyTaken.ClientID %>').val('0.00');
        }

        if (!isNaN(valueBase)) {
            if (valueBase - value < 0) {
                $('#<%:txtPackageQtyTaken.ClientID %>').val('0.00');
            }
        }
    });

    function onItemServiceAIOClicked(param) {
        if (param) {
            $('#lblServiceAddData').hide();
            $('#lblServiceQuickPick').hide();
            $('#lblTemplatePanel').hide();
            $('#lblServicePackageQuickPick').hide();
            $('#lblServiceAIOTransactionQuickPick').show();
        }
        else {
            $('#lblServiceAddData').show();
            $('#lblServiceQuickPick').show();
            $('#lblTemplatePanel').show();
            $('#lblServicePackageQuickPick').hide();
            $('#lblServiceAIOTransactionQuickPick').hide();
        }
    }

    function onChkIsCopyMultiVisitSchedule(param) {
        if (param) {
            $('#lblServiceAddData').hide();
            $('#lblServiceQuickPick').hide();
            $('#lblTemplatePanel').hide();
            $('#lblServicePackageQuickPick').hide();
            $('#lblServiceAIOTransactionQuickPick').hide();
            $('#lblCopyMultiVisitScheduleOrder').show();
            
        }
        else {
            if ($('#<%=hdnIsChargesGenerateMCUServiceCtl.ClientID %>').val() == '1') {
                $('#lblServiceAddData').hide();
                $('#lblServiceQuickPick').hide();
                $('#lblTemplatePanel').hide();
                $('#lblCopyMultiVisitScheduleOrder').hide();
                $('#lblServicePackageQuickPick').hide();
                $('#lblServiceAIOTransactionQuickPick').hide();
            } else {
                $('#lblServiceAddData').show();
                $('#lblServiceQuickPick').show();
                $('#lblTemplatePanel').show();
                $('#lblCopyMultiVisitScheduleOrder').hide();

                if ($('#<%=hdnDepartmentID.ClientID %>').val() == 'OUTPATIENT' || $('#<%=hdnDepartmentID.ClientID %>').val() == 'DIAGNOSTIC') {
                    if ('<%=SA0198() %>' == "1") {
                        $('#lblServicePackageQuickPick').show();
                    }
                    else {
                        $('#lblServicePackageQuickPick').hide();
                    }
                }
                else {
                    $('#lblServicePackageQuickPick').hide();
                }

                if ($('#<%=hdnIsAIOTransactionServiceCtl.ClientID %>').val() == '1') {
                    $('#lblServiceAIOTransactionQuickPick').show();
                }
                else {
                    $('#lblServiceAIOTransactionQuickPick').hide();
                }
            }
        }
    }

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

            cboServiceChargeClassID.SetValue(getClassID());

            $('#<%=hdnIsPackageItem.ClientID %>').val('0');
            $('#<%=hdnIsUsingAccumulatedPriceItem.ClientID %>').val('0');
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

            $('#<%:txtServiceQty.ClientID %>').removeAttr("disabled");
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
            $('#<%=hdnDiscComp1.ClientID %>').val('0');
            $('#<%=hdnDiscComp2.ClientID %>').val('0');
            $('#<%=hdnDiscComp3.ClientID %>').val('0');
            $('#<%=hdnDiscCompCito.ClientID %>').val('0');
            $('#txtServiceDiscTariffComp1').val('0').trigger('changeValue');
            $('#txtServiceDiscTariffComp2').val('0').trigger('changeValue');
            $('#txtServiceDiscTariffComp3').val('0').trigger('changeValue');
            $('#txtServiceCitoDisc').val('0').trigger('changeValue');
            $('#<%=hdnServiceBaseTariff.ClientID %>').val('');
            $('#<%=hdnServiceItemUnit.ClientID %>').val('');

            cboGCDiscountReasonChargesDt.SetValue("X550^999");
            $('#<%=txtDiscountReasonChargesDt.ClientID %>').val("");
            $('#<%=txtDiscountReasonChargesDt.ClientID %>').removeAttr('readonly');

            $('#<%=hdnServiceDiscountAmount.ClientID %>').val('0');
            $('#<%=hdnServiceDiscountAmountComp1.ClientID %>').val('0');
            $('#<%=hdnServiceDiscountAmountComp2.ClientID %>').val('0');
            $('#<%=hdnServiceDiscountAmountComp3.ClientID %>').val('0');

            $('#<%=hdnServiceCoverageAmount.ClientID %>').val('0');

            $('#<%=hdnServiceIsDiscountUsedComp.ClientID %>').val('0');

            $('#<%=hdnServiceIsDicountInPercentage.ClientID %>').val('0');
            $('#<%=hdnServiceIsDicountInPercentageComp1.ClientID %>').val('0');
            $('#<%=hdnServiceIsDicountInPercentageComp2.ClientID %>').val('0');
            $('#<%=hdnServiceIsDicountInPercentageComp3.ClientID %>').val('0');

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

            $('#<%=trItemNameVariable.ClientID %>').attr('style', 'display:none');
            $('#<%=txtItemNameVariable.ClientID %>').val('');

            $('#<%=hdnIsAllowVariableTariffComp1.ClientID %>').val('0');
            $('#<%=hdnIsAllowVariableTariffComp2.ClientID %>').val('0');
            $('#<%=hdnIsAllowVariableTariffComp3.ClientID %>').val('0');

            $('#<%=hdnIsAllowDiscountTariffComp1.ClientID %>').val('0');
            $('#<%=hdnIsAllowDiscountTariffComp2.ClientID %>').val('0');
            $('#<%=hdnIsAllowDiscountTariffComp3.ClientID %>').val('0');

            $('#<%=trPackageQty.ClientID %>').attr('style', 'display:none');
            $('#<%=trPackageQtyTaken.ClientID %>').attr('style', 'display:none');
            $('#<%=trExpiredDate.ClientID %>').attr('style', 'display:none');
            $('#<%=txtExpiredDate.ClientID %>').val($('#<%=hdnDefaultExpiredDate.ClientID %>').val());

            $('#lblTestPartner').attr('class', 'lblDisabled');
            $('#<%=txtTestPartnerCode.ClientID %>').val('');
            $('#<%=txtTestPartnerName.ClientID %>').val('');
            $('#<%=hdnTestPartnerID.ClientID %>').val('');
            $('#<%=txtTestPartnerCode.ClientID %>').attr('readonly', 'readonly');

            hideLoadingPanel();
        }
    });
    //#endregion

    //#endregion

    //#region Edit
    $('.imgServiceEdit.imgLink').die('click');
    $('.imgServiceEdit.imgLink').live('click', function () {
        $row = $(this).closest('tr').parent().closest('tr');
        var obj = rowToObject($row);
        if ($('#<%=hdnIsChargesGenerateMCUServiceCtl.ClientID %>').val() == "1") {
            displayErrorMessageBox("WARNING", "Maaf, transaksi hasil Generate MCU tidak dapat diubah.");
        } else {
            if ($('#<%=hdnIsAIOTransactionServiceCtl.ClientID %>').val() == "0") {
                $('#containerEntryService').show();
                showLoadingPanel();

                cboServiceChargeClassID.SetValue(obj.ChargeClassID);

                $('#<%=hdnTempTotalPatient.ClientID %>').val(getServiceTotalPatient());
                $('#<%=hdnTempTotalPayer.ClientID %>').val(getServiceTotalPayer());

                if (obj.IsSubContractItem == 'True') {
                    $('#lblTestPartner').attr('class', 'lblLink');
                    $('#<%=txtTestPartnerCode.ClientID %>').removeAttr('readonly');
                }
                else {
                    $('#lblTestPartner').attr('class', 'lblDisabled');
                    $('#<%=txtTestPartnerCode.ClientID %>').attr('readonly', 'readonly');
                }

                if (obj.IsPackageBalanceItem == 'True') {
                    $('#<%=trJumlah.ClientID %>').attr('style', 'display:none');
                    $('#<%=hdnIsPackageBalanceItem.ClientID %>').val('True');

                    if (obj.IsFirstPackageBalance == 'True') {
                        $('#<%=trExpiredDate.ClientID %>').attr('style', '');
                        $('#<%=txtExpiredDate.ClientID %>').val(obj.ExpiredDate);
                    }
                    else {
                        $('#<%=trExpiredDate.ClientID %>').attr('style', 'display:none');
                    }

                    var MRN = '<%=MRN() %>';
                    var filterVisitBalance = "ItemID = '" + obj.ItemID + "' AND MRN = '" + MRN + "' AND Quantity > 0";
                    Methods.getObject('GetVisitPackageBalanceHdList', filterVisitBalance, function (result1) {
                        if (result1 != null) {
                            $('#<%=txtPackageQty.ClientID %>').val(result1.Quantity.toFixed(2));
                            $('#<%=trPackageQty.ClientID %>').attr('style', '');
                            $('#<%=trPackageQtyTaken.ClientID %>').attr('style', '');
                        }
                        else {
                            var filterService = "ItemID = '" + obj.ItemID + "'";
                            Methods.getObject('GetItemServiceList', filterService, function (result1) {
                                if (result1 != null) {
                                    if (result1.IsPackageBalanceItem) {
                                        $('#<%=txtPackageQty.ClientID %>').val(result1.DefaultPackageBalanceQty.toFixed(2));
                                        $('#<%=trPackageQty.ClientID %>').attr('style', '');
                                        $('#<%=trPackageQtyTaken.ClientID %>').attr('style', '');
                                    }
                                    else {
                                        $('#<%=txtPackageQty.ClientID %>').val('');
                                        $('#<%=trPackageQty.ClientID %>').attr('style', 'display:none');
                                        $('#<%=trPackageQtyTaken.ClientID %>').attr('style', 'display:none');
                                    }
                                }
                            });
                        }
                    });
                }
                else {
                    $('#<%=txtPackageQty.ClientID %>').val('');
                    $('#<%=trPackageQty.ClientID %>').attr('style', 'display:none');
                    $('#<%=trPackageQtyTaken.ClientID %>').attr('style', 'display:none');
                    $('#<%=trExpiredDate.ClientID %>').attr('style', 'display:none');
                    $('#<%=trJumlah.ClientID %>').attr('style', '');
                }

                $('#<%=txtPackageQtyTaken.ClientID %>').val(obj.PackageBalanceQtyTaken);
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

                $('#<%=hdnDefaultTariffComp.ClientID %>').val(obj.DefaultTariffComp);

                setCheckBoxEnabled($('#<%=chkServiceIsCITO.ClientID %>'), obj.IsAllowCITO == 'True');
                setCheckBoxEnabled($('#<%=chkServiceIsVariable.ClientID %>'), obj.IsAllowVariable == 'True');
                setCheckBoxEnabled($('#<%=chkServiceIsUnbilledItem.ClientID %>'), obj.IsAllowUnbilledItem == 'True');
                if (obj.IsUsingAccumulatedPrice == 'True') {
                    setCheckBoxEnabled($('#<%=chkServiceIsDiscount.ClientID %>'), false);
                }
                else {
                    setCheckBoxEnabled($('#<%=chkServiceIsDiscount.ClientID %>'), obj.IsAllowDiscount == 'True');
                }
                setCheckBoxEnabled($('#<%=chkServiceIsComplication.ClientID %>'), obj.IsAllowComplication == 'True');

                if (obj.IsAllowChangeQty == 'True') {
                    $('#<%:txtServiceQty.ClientID %>').removeAttr("disabled");
                }
                else {
                    $('#<%:txtServiceQty.ClientID %>').attr("disabled", true);
                }

                $('#<%=hdnIsPackageItem.ClientID %>').val(obj.IsPackageItem == "True" ? '1' : '0');
                $('#<%=hdnIsUsingAccumulatedPriceItem.ClientID %>').val(obj.IsUsingAccumulatedPrice == "True" ? '1' : '0');
                $('#<%=chkServiceIsCITO.ClientID %>').prop('checked', obj.IsCITO == 'True');
                $('#<%=chkServiceIsVariable.ClientID %>').prop('checked', obj.IsVariable == 'True');
                $('#<%=chkServiceIsUnbilledItem.ClientID %>').prop('checked', obj.IsUnbilledItem == 'True');
                $('#<%=chkServiceIsDiscount.ClientID %>').prop('checked', obj.IsDiscount == 'True');
                $('#<%=chkServiceIsComplication.ClientID %>').prop('checked', obj.IsComplication == 'True');

                $('#<%=hdnServiceIsCITOInPercentage.ClientID %>').val(obj.IsCITOInPercentage == 'True' ? '1' : '0');
                $('#<%=hdnServiceBaseCITOAmount.ClientID %>').val(obj.BaseCITOAmount);
                $('#<%=hdnServiceIsComplicationInPercentage.ClientID %>').val(obj.IsComplicationInPercentage == 'True' ? '1' : '0');
                $('#<%=hdnServiceBaseComplicationAmount.ClientID %>').val(obj.BaseComplicationAmount);

                $('#<%=txtServiceUnitTariff.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtServiceCITO.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtServiceComplication.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtServiceDiscount.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtServicePatient.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtServicePayer.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtServiceTotal.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtServiceCostAmount.ClientID %>').attr('readonly', 'readonly');

                $('#<%=hdnIsAllowVariableTariffComp1.ClientID %>').val(obj.IsAllowVariableTariffComp1 == 'True' ? '1' : '0');
                $('#<%=hdnIsAllowVariableTariffComp2.ClientID %>').val(obj.IsAllowVariableTariffComp2 == 'True' ? '1' : '0');
                $('#<%=hdnIsAllowVariableTariffComp3.ClientID %>').val(obj.IsAllowVariableTariffComp3 == 'True' ? '1' : '0');

                if ($('#<%=chkServiceIsVariable.ClientID %>').is(':checked')) {
                    $('#<%=trItemNameVariable.ClientID %>').removeAttr('style');
                    $('#<%=txtItemNameVariable.ClientID %>').val(obj.ItemName1);

                    $('#<%=txtServicePatient.ClientID %>').removeAttr('readonly');
                    if ($('#<%=hdnBusinessPartnerIDServiceCtl.ClientID %>').val() != "1") {
                        $('#<%=txtServicePayer.ClientID %>').removeAttr('readonly');
                    }

                    var hdnIsAllowVariableTariffComp1 = $('#<%=hdnIsAllowVariableTariffComp1.ClientID %>').val();
                    var hdnIsAllowVariableTariffComp2 = $('#<%=hdnIsAllowVariableTariffComp2.ClientID %>').val();
                    var hdnIsAllowVariableTariffComp3 = $('#<%=hdnIsAllowVariableTariffComp3.ClientID %>').val();

                    var countIsVariable = 0;

                    if (hdnIsAllowVariableTariffComp1 == "1") {
                        countIsVariable = countIsVariable + 1;
                        $('#<%=txtServiceTariffComp1.ClientID %>').removeAttr('readonly');
                    } else {
                        $('#<%=txtServiceTariffComp1.ClientID %>').attr('readonly', 'readonly');
                    }

                    if (hdnIsAllowVariableTariffComp2 == "1") {
                        countIsVariable = countIsVariable + 1;
                        $('#<%=txtServiceTariffComp2.ClientID %>').removeAttr('readonly');
                    } else {
                        $('#<%=txtServiceTariffComp2.ClientID %>').attr('readonly', 'readonly');
                    }

                    if (hdnIsAllowVariableTariffComp3 == "1") {
                        countIsVariable = countIsVariable + 1;
                        $('#<%=txtServiceTariffComp3.ClientID %>').removeAttr('readonly');
                    } else {
                        $('#<%=txtServiceTariffComp3.ClientID %>').attr('readonly', 'readonly');
                    }

                }
                else {
                    $('#<%=trItemNameVariable.ClientID %>').attr('style', 'display:none');
                    $('#<%=txtItemNameVariable.ClientID %>').val('');

                    $('#<%=txtServiceUnitTariff.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtServicePatient.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtServicePayer.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtServiceTariffComp1.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtServiceTariffComp2.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtServiceTariffComp3.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtServiceTotalTariff.ClientID %>').attr('readonly', 'readonly');
                }

                $('#<%=hdnIsAllowDiscountTariffComp1.ClientID %>').val(obj.IsAllowDiscountTariffComp1 == 'True' ? '1' : '0');
                $('#<%=hdnIsAllowDiscountTariffComp2.ClientID %>').val(obj.IsAllowDiscountTariffComp2 == 'True' ? '1' : '0');
                $('#<%=hdnIsAllowDiscountTariffComp3.ClientID %>').val(obj.IsAllowDiscountTariffComp3 == 'True' ? '1' : '0');

                if ($('#<%=chkServiceIsDiscount.ClientID %>').is(':checked')) {
                    var hdnIsAllowDiscountTariffComp1 = $('#<%=hdnIsAllowDiscountTariffComp1.ClientID %>').val();
                    var hdnIsAllowDiscountTariffComp2 = $('#<%=hdnIsAllowDiscountTariffComp2.ClientID %>').val();
                    var hdnIsAllowDiscountTariffComp3 = $('#<%=hdnIsAllowDiscountTariffComp3.ClientID %>').val();

                    var countIsDiscount = 0;

                    if (hdnIsAllowDiscountTariffComp1 == "1") {
                        countIsDiscount = countIsDiscount + 1;
                        $('#<%=txtServiceDiscPercentComp1.ClientID %>').removeAttr('readonly');
                        $('#<%=txtServiceDiscComp1.ClientID %>').removeAttr('readonly');
                    } else {
                        $('#<%=txtServiceDiscPercentComp1.ClientID %>').attr('readonly', 'readonly');
                        $('#<%=txtServiceDiscComp1.ClientID %>').attr('readonly', 'readonly');
                    }

                    if (hdnIsAllowDiscountTariffComp2 == "1") {
                        countIsDiscount = countIsDiscount + 1;
                        $('#<%=txtServiceDiscPercentComp2.ClientID %>').removeAttr('readonly');
                        $('#<%=txtServiceDiscComp2.ClientID %>').removeAttr('readonly');
                    } else {
                        $('#<%=txtServiceDiscPercentComp2.ClientID %>').attr('readonly', 'readonly');
                        $('#<%=txtServiceDiscComp2.ClientID %>').attr('readonly', 'readonly');
                    }

                    if (hdnIsAllowDiscountTariffComp3 == "1") {
                        countIsDiscount = countIsDiscount + 1;
                        $('#<%=txtServiceDiscPercentComp3.ClientID %>').removeAttr('readonly');
                        $('#<%=txtServiceDiscComp3.ClientID %>').removeAttr('readonly');
                    } else {
                        $('#<%=txtServiceDiscPercentComp3.ClientID %>').attr('readonly', 'readonly');
                        $('#<%=txtServiceDiscComp3.ClientID %>').attr('readonly', 'readonly');
                    }

                } else {
                    $('#<%=txtServiceDiscPercentComp1.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtServiceDiscComp1.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtServiceDiscPercentComp2.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtServiceDiscComp2.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtServiceDiscPercentComp3.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtServiceDiscComp3.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtServiceDiscTotal.ClientID %>').attr('readonly', 'readonly');
                }

                $('#<%=txtServiceUnitTariff.ClientID %>').val(obj.Tariff).trigger('changeValue');
                $('#<%=txtServiceTariffComp1.ClientID %>').val(obj.TariffComp1).trigger('changeValue');
                $('#<%=txtServiceTariffComp2.ClientID %>').val(obj.TariffComp2).trigger('changeValue');
                $('#<%=txtServiceTariffComp3.ClientID %>').val(obj.TariffComp3).trigger('changeValue');
                $('#<%=txtServiceCITO.ClientID %>').val(obj.CITOAmount).trigger('changeValue');

                cboGCDiscountReasonChargesDt.SetValue(obj.GCDiscountReason);
                if (obj.GCDiscountReason == "X550^999") {
                    $('#<%=txtDiscountReasonChargesDt.ClientID %>').removeAttr('readonly');
                } else {
                    $('#<%=txtDiscountReasonChargesDt.ClientID %>').attr('readonly', 'readonly');
                }
                $('#<%=txtDiscountReasonChargesDt.ClientID %>').val(obj.DiscountReason);

                $('#<%=hdnDiscCompCito.ClientID %>').val(obj.CITODiscount);
                $('#<%=hdnDiscComp1.ClientID %>').val(obj.DiscountComp1);
                $('#<%=hdnDiscComp2.ClientID %>').val(obj.DiscountComp2);
                $('#<%=hdnDiscComp3.ClientID %>').val(obj.DiscountComp3);
                $('#<%=txtServiceDiscount.ClientID %>').val(obj.DiscountAmount).trigger('changeValue');
                $('#<%=txtServiceDiscComp1.ClientID %>').val(obj.DiscountComp1).trigger('changeValue');
                $('#<%=txtServiceDiscComp2.ClientID %>').val(obj.DiscountComp2).trigger('changeValue');
                $('#<%=txtServiceDiscComp3.ClientID %>').val(obj.DiscountComp3).trigger('changeValue');
                $('#<%=txtServiceDiscPercentComp1.ClientID %>').val(obj.DiscountPercentageComp1).trigger('changeValue');
                $('#<%=txtServiceDiscPercentComp2.ClientID %>').val(obj.DiscountPercentageComp2).trigger('changeValue');
                $('#<%=txtServiceDiscPercentComp3.ClientID %>').val(obj.DiscountPercentageComp3).trigger('changeValue');
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
                    $('#<%=hdnServiceIsCoverageInPercentage.ClientID %>').val(result.IsCoverageInPercentage ? '1' : '0');
                });

                var hdnIsChargesGenerateMCUServiceCtl = $('#<%=hdnIsChargesGenerateMCUServiceCtl.ClientID %>').val();
                if (hdnIsChargesGenerateMCUServiceCtl == "1") {
                    $('#<%=chkServiceIsCITO.ClientID %>').attr('disabled', 'disabled');
                    $('#<%=chkServiceIsVariable.ClientID %>').attr('disabled', 'disabled');
                    $('#<%=chkServiceIsUnbilledItem.ClientID %>').attr('disabled', 'disabled');
                    $('#<%=chkServiceIsDiscount.ClientID %>').attr('disabled', 'disabled');
                    $('#<%=chkServiceIsComplication.ClientID %>').attr('disabled', 'disabled');
                    $('#lblTestPartner').attr('class', 'lblDisabled');
                    $('#<%:txtServiceQty.ClientID %>').attr("disabled", true);
                    cboServiceChargeClassID.SetEnabled(false);
                }

                hideLoadingPanel();
            }
            else {
                pcAIOEdit.Show();

                cboServiceChargeClassID.SetValue(obj.ChargeClassID);

                $('#<%=hdnTempTotalPatient.ClientID %>').val(getServiceTotalPatient());
                $('#<%=hdnTempTotalPayer.ClientID %>').val(getServiceTotalPayer());

                if (obj.IsSubContractItem == 'True') {
                    $('#lblTestPartner').attr('class', 'lblLink');
                    $('#<%=txtTestPartnerCode.ClientID %>').removeAttr('readonly');
                }
                else {
                    $('#lblTestPartner').attr('class', 'lblDisabled');
                    $('#<%=txtTestPartnerCode.ClientID %>').attr('readonly', 'readonly');
                }

                if (obj.IsPackageBalanceItem == 'True') {
                    $('#<%=trJumlah.ClientID %>').attr('style', 'display:none');
                    $('#<%=hdnIsPackageBalanceItem.ClientID %>').val('True');

                    if (obj.IsFirstPackageBalance == 'True') {
                        $('#<%=trExpiredDate.ClientID %>').attr('style', '');
                        $('#<%=txtExpiredDate.ClientID %>').val(obj.ExpiredDate);
                    }
                    else {
                        $('#<%=trExpiredDate.ClientID %>').attr('style', 'display:none');
                    }

                    var MRN = '<%=MRN() %>';
                    var filterVisitBalance = "ItemID = '" + obj.ItemID + "' AND MRN = '" + MRN + "' AND Quantity > 0";
                    Methods.getObject('GetVisitPackageBalanceHdList', filterVisitBalance, function (result1) {
                        if (result1 != null) {
                            $('#<%=txtPackageQty.ClientID %>').val(result1.Quantity.toFixed(2));
                            $('#<%=trPackageQty.ClientID %>').attr('style', '');
                            $('#<%=trPackageQtyTaken.ClientID %>').attr('style', '');
                        }
                        else {
                            var filterService = "ItemID = '" + obj.ItemID + "'";
                            Methods.getObject('GetItemServiceList', filterService, function (result1) {
                                if (result1 != null) {
                                    if (result1.IsPackageBalanceItem) {
                                        $('#<%=txtPackageQty.ClientID %>').val(result1.DefaultPackageBalanceQty.toFixed(2));
                                        $('#<%=trPackageQty.ClientID %>').attr('style', '');
                                        $('#<%=trPackageQtyTaken.ClientID %>').attr('style', '');
                                    }
                                    else {
                                        $('#<%=txtPackageQty.ClientID %>').val('');
                                        $('#<%=trPackageQty.ClientID %>').attr('style', 'display:none');
                                        $('#<%=trPackageQtyTaken.ClientID %>').attr('style', 'display:none');
                                    }
                                }
                            });
                        }
                    });
                }
                else {
                    $('#<%=txtPackageQty.ClientID %>').val('');
                    $('#<%=trPackageQty.ClientID %>').attr('style', 'display:none');
                    $('#<%=trPackageQtyTaken.ClientID %>').attr('style', 'display:none');
                    $('#<%=trExpiredDate.ClientID %>').attr('style', 'display:none');
                    $('#<%=trJumlah.ClientID %>').attr('style', '');
                }

                $('#<%=txtPackageQtyTaken.ClientID %>').val(obj.PackageBalanceQtyTaken);
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

                $('#<%=hdnDefaultTariffComp.ClientID %>').val(obj.DefaultTariffComp);

                setCheckBoxEnabled($('#<%=chkServiceIsCITO.ClientID %>'), obj.IsAllowCITO == 'True');
                setCheckBoxEnabled($('#<%=chkServiceIsVariable.ClientID %>'), obj.IsAllowVariable == 'True');
                setCheckBoxEnabled($('#<%=chkServiceIsUnbilledItem.ClientID %>'), obj.IsAllowUnbilledItem == 'True');
                if (obj.IsUsingAccumulatedPrice == 'True') {
                    setCheckBoxEnabled($('#<%=chkServiceIsDiscount.ClientID %>'), false);
                }
                else {
                    setCheckBoxEnabled($('#<%=chkServiceIsDiscount.ClientID %>'), obj.IsAllowDiscount == 'True');
                }
                setCheckBoxEnabled($('#<%=chkServiceIsComplication.ClientID %>'), obj.IsAllowComplication == 'True');

                if (obj.IsAllowChangeQty == 'True') {
                    $('#<%:txtServiceQty.ClientID %>').removeAttr("disabled");
                }
                else {
                    $('#<%:txtServiceQty.ClientID %>').attr("disabled", true);
                }

                $('#<%=hdnIsPackageItem.ClientID %>').val(obj.IsPackageItem == "True" ? '1' : '0');
                $('#<%=hdnIsUsingAccumulatedPriceItem.ClientID %>').val(obj.IsUsingAccumulatedPrice == "True" ? '1' : '0');
                $('#<%=chkServiceIsCITO.ClientID %>').prop('checked', obj.IsCITO == 'True');
                $('#<%=chkServiceIsVariable.ClientID %>').prop('checked', obj.IsVariable == 'True');
                $('#<%=chkServiceIsUnbilledItem.ClientID %>').prop('checked', obj.IsUnbilledItem == 'True');
                $('#<%=chkServiceIsDiscount.ClientID %>').prop('checked', obj.IsDiscount == 'True');
                $('#<%=chkServiceIsComplication.ClientID %>').prop('checked', obj.IsComplication == 'True');

                $('#<%=hdnServiceIsCITOInPercentage.ClientID %>').val(obj.IsCITOInPercentage == 'True' ? '1' : '0');
                $('#<%=hdnServiceBaseCITOAmount.ClientID %>').val(obj.BaseCITOAmount);
                $('#<%=hdnServiceIsComplicationInPercentage.ClientID %>').val(obj.IsComplicationInPercentage == 'True' ? '1' : '0');
                $('#<%=hdnServiceBaseComplicationAmount.ClientID %>').val(obj.BaseComplicationAmount);

                $('#<%=txtServiceUnitTariff.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtServiceCITO.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtServiceComplication.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtServiceDiscount.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtServicePatient.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtServicePayer.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtServiceTotal.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtServiceCostAmount.ClientID %>').attr('readonly', 'readonly');

                $('#<%=hdnIsAllowVariableTariffComp1.ClientID %>').val(obj.IsAllowVariableTariffComp1 == 'True' ? '1' : '0');
                $('#<%=hdnIsAllowVariableTariffComp2.ClientID %>').val(obj.IsAllowVariableTariffComp2 == 'True' ? '1' : '0');
                $('#<%=hdnIsAllowVariableTariffComp3.ClientID %>').val(obj.IsAllowVariableTariffComp3 == 'True' ? '1' : '0');

                if ($('#<%=chkServiceIsVariable.ClientID %>').is(':checked')) {
                    $('#<%=trItemNameVariable.ClientID %>').removeAttr('style');
                    $('#<%=txtItemNameVariable.ClientID %>').val(obj.ItemName1);

                    $('#<%=txtServicePatient.ClientID %>').removeAttr('readonly');
                    if ($('#<%=hdnBusinessPartnerIDServiceCtl.ClientID %>').val() != "1") {
                        $('#<%=txtServicePayer.ClientID %>').removeAttr('readonly');
                    }

                    var hdnIsAllowVariableTariffComp1 = $('#<%=hdnIsAllowVariableTariffComp1.ClientID %>').val();
                    var hdnIsAllowVariableTariffComp2 = $('#<%=hdnIsAllowVariableTariffComp2.ClientID %>').val();
                    var hdnIsAllowVariableTariffComp3 = $('#<%=hdnIsAllowVariableTariffComp3.ClientID %>').val();

                    var countIsVariable = 0;

                    if (hdnIsAllowVariableTariffComp1 == "1") {
                        countIsVariable = countIsVariable + 1;
                        $('#<%=txtServiceTariffComp1.ClientID %>').removeAttr('readonly');
                    } else {
                        $('#<%=txtServiceTariffComp1.ClientID %>').attr('readonly', 'readonly');
                    }

                    if (hdnIsAllowVariableTariffComp2 == "1") {
                        countIsVariable = countIsVariable + 1;
                        $('#<%=txtServiceTariffComp2.ClientID %>').removeAttr('readonly');
                    } else {
                        $('#<%=txtServiceTariffComp2.ClientID %>').attr('readonly', 'readonly');
                    }

                    if (hdnIsAllowVariableTariffComp3 == "1") {
                        countIsVariable = countIsVariable + 1;
                        $('#<%=txtServiceTariffComp3.ClientID %>').removeAttr('readonly');
                    } else {
                        $('#<%=txtServiceTariffComp3.ClientID %>').attr('readonly', 'readonly');
                    }

                }
                else {
                    $('#<%=trItemNameVariable.ClientID %>').attr('style', 'display:none');
                    $('#<%=txtItemNameVariable.ClientID %>').val('');

                    $('#<%=txtServiceUnitTariff.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtServicePatient.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtServicePayer.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtServiceTariffComp1.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtServiceTariffComp2.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtServiceTariffComp3.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtServiceTotalTariff.ClientID %>').attr('readonly', 'readonly');
                }

                $('#<%=hdnIsAllowDiscountTariffComp1.ClientID %>').val(obj.IsAllowDiscountTariffComp1 == 'True' ? '1' : '0');
                $('#<%=hdnIsAllowDiscountTariffComp2.ClientID %>').val(obj.IsAllowDiscountTariffComp2 == 'True' ? '1' : '0');
                $('#<%=hdnIsAllowDiscountTariffComp3.ClientID %>').val(obj.IsAllowDiscountTariffComp3 == 'True' ? '1' : '0');

                if ($('#<%=chkServiceIsDiscount.ClientID %>').is(':checked')) {
                    var hdnIsAllowDiscountTariffComp1 = $('#<%=hdnIsAllowDiscountTariffComp1.ClientID %>').val();
                    var hdnIsAllowDiscountTariffComp2 = $('#<%=hdnIsAllowDiscountTariffComp2.ClientID %>').val();
                    var hdnIsAllowDiscountTariffComp3 = $('#<%=hdnIsAllowDiscountTariffComp3.ClientID %>').val();

                    var countIsDiscount = 0;

                    if (hdnIsAllowDiscountTariffComp1 == "1") {
                        countIsDiscount = countIsDiscount + 1;
                        $('#<%=txtServiceDiscPercentComp1.ClientID %>').removeAttr('readonly');
                        $('#<%=txtServiceDiscComp1.ClientID %>').removeAttr('readonly');
                    } else {
                        $('#<%=txtServiceDiscPercentComp1.ClientID %>').attr('readonly', 'readonly');
                        $('#<%=txtServiceDiscComp1.ClientID %>').attr('readonly', 'readonly');
                    }

                    if (hdnIsAllowDiscountTariffComp2 == "1") {
                        countIsDiscount = countIsDiscount + 1;
                        $('#<%=txtServiceDiscPercentComp2.ClientID %>').removeAttr('readonly');
                        $('#<%=txtServiceDiscComp2.ClientID %>').removeAttr('readonly');
                    } else {
                        $('#<%=txtServiceDiscPercentComp2.ClientID %>').attr('readonly', 'readonly');
                        $('#<%=txtServiceDiscComp2.ClientID %>').attr('readonly', 'readonly');
                    }

                    if (hdnIsAllowDiscountTariffComp3 == "1") {
                        countIsDiscount = countIsDiscount + 1;
                        $('#<%=txtServiceDiscPercentComp3.ClientID %>').removeAttr('readonly');
                        $('#<%=txtServiceDiscComp3.ClientID %>').removeAttr('readonly');
                    } else {
                        $('#<%=txtServiceDiscPercentComp3.ClientID %>').attr('readonly', 'readonly');
                        $('#<%=txtServiceDiscComp3.ClientID %>').attr('readonly', 'readonly');
                    }

                } else {
                    $('#<%=txtServiceDiscPercentComp1.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtServiceDiscComp1.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtServiceDiscPercentComp2.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtServiceDiscComp2.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtServiceDiscPercentComp3.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtServiceDiscComp3.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtServiceDiscTotal.ClientID %>').attr('readonly', 'readonly');
                }

                $('#<%=txtServiceUnitTariff.ClientID %>').val(obj.Tariff).trigger('changeValue');
                $('#<%=txtServiceTariffComp1.ClientID %>').val(obj.TariffComp1).trigger('changeValue');
                $('#<%=txtServiceTariffComp2.ClientID %>').val(obj.TariffComp2).trigger('changeValue');
                $('#<%=txtServiceTariffComp3.ClientID %>').val(obj.TariffComp3).trigger('changeValue');
                $('#<%=txtServiceCITO.ClientID %>').val(obj.CITOAmount).trigger('changeValue');

                cboGCDiscountReasonChargesDt.SetValue(obj.GCDiscountReason);
                if (obj.GCDiscountReason == "X550^999") {
                    $('#<%=txtDiscountReasonChargesDt.ClientID %>').removeAttr('readonly');
                } else {
                    $('#<%=txtDiscountReasonChargesDt.ClientID %>').attr('readonly', 'readonly');
                }
                $('#<%=txtDiscountReasonChargesDt.ClientID %>').val(obj.DiscountReason);

                $('#<%=hdnDiscCompCito.ClientID %>').val(obj.CITODiscount);
                $('#<%=hdnDiscComp1.ClientID %>').val(obj.DiscountComp1);
                $('#<%=hdnDiscComp2.ClientID %>').val(obj.DiscountComp2);
                $('#<%=hdnDiscComp3.ClientID %>').val(obj.DiscountComp3);
                $('#<%=txtServiceDiscount.ClientID %>').val(obj.DiscountAmount).trigger('changeValue');
                $('#<%=txtServiceDiscComp1.ClientID %>').val(obj.DiscountComp1).trigger('changeValue');
                $('#<%=txtServiceDiscComp2.ClientID %>').val(obj.DiscountComp2).trigger('changeValue');
                $('#<%=txtServiceDiscComp3.ClientID %>').val(obj.DiscountComp3).trigger('changeValue');
                $('#<%=txtServiceDiscPercentComp1.ClientID %>').val(obj.DiscountPercentageComp1).trigger('changeValue');
                $('#<%=txtServiceDiscPercentComp2.ClientID %>').val(obj.DiscountPercentageComp2).trigger('changeValue');
                $('#<%=txtServiceDiscPercentComp3.ClientID %>').val(obj.DiscountPercentageComp3).trigger('changeValue');
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
                    $('#<%=hdnServiceIsCoverageInPercentage.ClientID %>').val(result.IsCoverageInPercentage ? '1' : '0');
                });

                cboEditAIOChargeClass.SetValue(obj.ChargeClassID);
                $('#<%=txtAIOQty.ClientID %>').val(obj.ChargedQuantity);
            }
        }
    });
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

    $('.imgIsEditPackageItemAccumulated.imgLink').live('click', function () {
        $row = $(this).closest('tr').parent().closest('tr');
        var entity = rowToObject($row);
        var id = entity.ID;
        var itemID = entity.ItemID;
        var hdnTariffComp1Text = $('#<%=hdnTariffComp1Text.ClientID %>').val();
        var hdnTariffComp2Text = $('#<%=hdnTariffComp2Text.ClientID %>').val();
        var hdnTariffComp3Text = $('#<%=hdnTariffComp3Text.ClientID %>').val();
        var param = id + "|" + itemID + "|" + hdnTariffComp1Text + "|" + hdnTariffComp2Text + "|" + hdnTariffComp3Text;
        var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Transaction/EditItemPackageDetail.ascx");
        openUserControlPopup(url, param, 'Item Paket - Akumulasi', 1000, 500);
    });

    $('.imgIsEditPackageItemNonAccumulated.imgLink').live('click', function () {
        $row = $(this).closest('tr').parent().closest('tr');
        var entity = rowToObject($row);
        var id = entity.ID;
        var itemID = entity.ItemID;
        var param = id + "|" + itemID;
        var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Transaction/EditQtyItemPackageDetail.ascx");
        openUserControlPopup(url, param, 'Item Paket - NonAkumulasi', 1000, 500);
    });

    $('.imgPreconditionNotesCtl.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        var entity = rowToObject($row);
        var notes = entity.PreconditionNotes;
        var itemName = entity.ItemName1;
        var tittle = "Informasi Test (" + itemName + ")";
        showToast(tittle, notes);
    });


    //#region Item Service
    //#region Item
    function onGetServiceItemFilterExpression() {
        return $('#<%=hdnServiceItemFilterExpression.ClientID %>').val();
    }

    $('#lblServiceItem.lblLink').live('click', function () {
        var filterExpression = onGetServiceItemFilterExpression() + " AND GCItemStatus != 'X181^999'";
        var serviceUnitID = $('#<%=hdnHealthcareServiceUnitIDServiceCtl.ClientID %>').val();
        var isLabUnit = $('#<%=hdnIsLaboratoryUnit.ClientID %>').val();
        var radiologyServiceUnitID = $('#<%=hdnImagingServiceUnitID.ClientID %>').val();

        if (isLabUnit == "1") {
            if ($('#<%=hdnBPJSRegistration.ClientID %>').val() == '1') {
                if ($('#<%=hdnIsOnlyBPJSLab.ClientID %>').val() == '1') {
                    filterExpression += " AND IsBPJS = 1";
                }
            }
        }
        else if (serviceUnitID == radiologyServiceUnitID) {
            if ($('#<%=hdnBPJSRegistration.ClientID %>').val() == '1') {
                if ($('#<%=hdnIsOnlyBPJSRad.ClientID %>').val() == '1') {
                    filterExpression += " AND IsBPJS = 1";
                }
            }
        }
        else {
            if ($('#<%=hdnBPJSRegistration.ClientID %>').val() == '1') {
                if ($('#<%=hdnIsOnlyBPJSOth.ClientID %>').val() == '1') {
                    filterExpression += " AND IsBPJS = 1";
                }
            }
        }

        openSearchDialog('serviceunititem', filterExpression, function (value) {
            $('#<%=txtServiceItemCode.ClientID %>').val(value);
            onTxtServiceItemCodeChanged(value);
        });
    });

    $('#<%=txtServiceItemCode.ClientID %>').live('change', function () {
        onTxtServiceItemCodeChanged($(this).val());
    });

    function onTxtServiceItemCodeChanged(value) {
        var isValidItem = false;
        var isPackageItem = '0';
        var isAllowDiscount = '0';
        var isUsingAccumulatedPrice = '0';
        var isAllowChangeQty = '1';
        if (value != '') {
            var filterItem = "ItemID IN (SELECT ItemID FROM vServiceUnitItem WHERE " + onGetServiceItemFilterExpression() + ") AND ItemCode = '" + value + "' AND GCItemStatus != 'X181^999'";
            Methods.getObject('GetvItemMasterList', filterItem, function (resultp) {
                if (resultp != null) {
                    isValidItem = true;
                    if (resultp.DefaultParamedicID != null && resultp.DefaultParamedicID != 0) {
                        $('#<%=hdnServicePhysicianID.ClientID %>').val(resultp.DefaultParamedicID);
                        $('#<%=txtServicePhysicianCode.ClientID %>').val(resultp.DefaultParamedicCode);
                        $('#<%=txtServicePhysicianName.ClientID %>').val(resultp.DefaultParamedicName);
                    }

                    $('#<%=txtServiceDiscPercentComp1.ClientID %>').val('0');
                    $('#<%=txtServiceDiscPercentComp2.ClientID %>').val('0');
                    $('#<%=txtServiceDiscPercentComp3.ClientID %>').val('0');

                    $('#<%=txtServiceQty.ClientID %>').val('1');

                    var filterItemService = "ItemID = " + resultp.ItemID;
                    Methods.getObject('GetItemServiceList', filterItemService, function (resultsc) {
                        if (resultsc != null) {
                            $('#<%=hdnIsAllowVariableTariffComp1.ClientID %>').val(resultsc.IsAllowVariableTariffComp1 ? '1' : '0');
                            $('#<%=hdnIsAllowVariableTariffComp2.ClientID %>').val(resultsc.IsAllowVariableTariffComp2 ? '1' : '0');
                            $('#<%=hdnIsAllowVariableTariffComp3.ClientID %>').val(resultsc.IsAllowVariableTariffComp3 ? '1' : '0');

                            $('#<%=hdnIsAllowDiscountTariffComp1.ClientID %>').val(resultsc.IsAllowDiscountTariffComp1 ? '1' : '0');
                            $('#<%=hdnIsAllowDiscountTariffComp2.ClientID %>').val(resultsc.IsAllowDiscountTariffComp2 ? '1' : '0');
                            $('#<%=hdnIsAllowDiscountTariffComp3.ClientID %>').val(resultsc.IsAllowDiscountTariffComp3 ? '1' : '0');

                            isPackageItem = resultsc.IsPackageItem == true ? '1' : '0';
                            isUsingAccumulatedPrice = resultsc.IsUsingAccumulatedPrice == true ? '1' : '0';
                            isAllowDiscount = resultsc.IsAllowDiscount == true ? '1' : '0';
                            isAllowChangeQty = resultsc.IsAllowChangeQty == true ? '1' : '0';

                            if (isAllowChangeQty == '1') {
                                $('#<%:txtServiceQty.ClientID %>').removeAttr("disabled");
                            }
                            else {
                                $('#<%:txtServiceQty.ClientID %>').attr("disabled", true);
                            }

                            $('#<%=hdnIsPackageBalanceItem.ClientID %>').val(resultsc.IsPackageBalanceItem);
                            if (resultsc.IsPackageBalanceItem) {
                                $('#<%=txtPackageQty.ClientID %>').val(resultsc.DefaultPackageBalanceQty.toFixed(2));
                                $('#<%=trPackageQty.ClientID %>').attr('style', '');
                                $('#<%=trPackageQtyTaken.ClientID %>').attr('style', '');
                                $('#<%=trExpiredDate.ClientID %>').attr('style', '');
                                $('#<%=txtPackageQtyTaken.ClientID %>').val(0.00);
                                $('#<%=trJumlah.ClientID %>').attr('style', 'display:none');
                            }
                            else {
                                $('#<%=txtPackageQty.ClientID %>').val('');
                                $('#<%=trPackageQty.ClientID %>').attr('style', 'display:none');
                                $('#<%=trPackageQtyTaken.ClientID %>').attr('style', 'display:none');
                                $('#<%=trExpiredDate.ClientID %>').attr('style', 'display:none');
                                $('#<%=trJumlah.ClientID %>').attr('style', '');
                            }
                        } else {
                            $('#<%=hdnIsAllowVariableTariffComp1.ClientID %>').val('0');
                            $('#<%=hdnIsAllowVariableTariffComp2.ClientID %>').val('0');
                            $('#<%=hdnIsAllowVariableTariffComp3.ClientID %>').val('0');

                            $('#<%=hdnIsAllowDiscountTariffComp1.ClientID %>').val('0');
                            $('#<%=hdnIsAllowDiscountTariffComp2.ClientID %>').val('0');
                            $('#<%=hdnIsAllowDiscountTariffComp3.ClientID %>').val('0');

                            isPackageItem = '0';
                            isUsingAccumulatedPrice = '0';
                        }
                    });
                }
                else {
                    isValidItem = false;
                }
            });
        }

        var paramedicID = $('#<%=hdnServicePhysicianID.ClientID %>').val();
        if (paramedicID == '') {
            paramedicID = '0';
        }

        if (isValidItem) {
            var today = new Date();
            var tempDate = "00";
            if (today.getDate() < 10) {
                tempDate = "0" + today.getDate();
            } else {
                tempDate = today.getDate();
            }
            var pad = "00";
            var tmpMonth = (today.getMonth() + 1).toString();
            var month = pad.substring(0, pad.length - tmpMonth.length) + tmpMonth;
            var date = tempDate + '-' + month + '-' + today.getFullYear();
            var time = today.getHours() + ":" + today.getMinutes();

            var hdnTransactionDateServiceCtl = $('#<%=hdnTransactionDateServiceCtl.ClientID %>').val() == "" ? date : $('#<%=hdnTransactionDateServiceCtl.ClientID %>').val();
            var hdnTransactionTime = $('#<%=hdnTransactionTime.ClientID %>').val() == "" ? time : $('#<%=hdnTransactionTime.ClientID %>').val();
            var hdnChargesHealthcareServiceUnitID = $('#<%=hdnHealthcareServiceUnitIDServiceCtl.ClientID %>').val();

            Methods.getItemRevenueSharing(value, paramedicID, cboServiceChargeClassID.GetValue(), '<%=GetMainParamedicRole() %>', $('#<%=hdnVisitIDCtl.ClientID %>').val(), hdnChargesHealthcareServiceUnitID, hdnTransactionDateServiceCtl, hdnTransactionTime, function (result) {
                if (result != null) {
                    $('#<%=hdnServiceItemID.ClientID %>').val(result.ItemID);
                    $('#<%=hdnServiceRevenueSharingID.ClientID %>').val(result.RevenueSharingID);
                    $('#<%=txtServiceItemName.ClientID %>').val(result.ItemName1);
                    $('#<%=txtItemNameVariable.ClientID %>').val(result.ItemName1);
                    if (result.IsSubContractItem) {
                        $('#lblTestPartner').attr('class', 'lblLink');
                        $('#<%=txtTestPartnerCode.ClientID %>').removeAttr('readonly');

                        $('#<%=hdnIsSubContractItem.ClientID %>').val('1');
                    }
                    else {
                        $('#lblTestPartner').attr('class', 'lblDisabled');
                        $('#<%=txtTestPartnerCode.ClientID %>').val('');
                        $('#<%=txtTestPartnerName.ClientID %>').val('');
                        $('#<%=hdnTestPartnerID.ClientID %>').val('');
                        $('#<%=txtTestPartnerCode.ClientID %>').attr('readonly', 'readonly');

                        $('#<%=hdnIsSubContractItem.ClientID %>').val('0');
                    }
                }
                else {
                    $('#<%=hdnServiceItemID.ClientID %>').val('');
                    $('#<%=hdnServiceRevenueSharingID.ClientID %>').val('');
                    $('#<%=txtServiceItemCode.ClientID %>').val('');
                    $('#<%=txtServiceItemName.ClientID %>').val('');
                    $('#<%=txtItemNameVariable.ClientID %>').val('');
                }
                getServiceTariff();

                $('#<%=hdnIsPackageItem.ClientID %>').val(isPackageItem);
                $('#<%=hdnIsUsingAccumulatedPriceItem.ClientID %>').val(isUsingAccumulatedPrice);

                if (isPackageItem == '1' && isUsingAccumulatedPrice == '1') {
                    setCheckBoxEnabled($('#<%=chkServiceIsDiscount.ClientID %>'), false);
                }
                else {
                    if(isAllowDiscount == '1') {
                        setCheckBoxEnabled($('#<%=chkServiceIsDiscount.ClientID %>'), true);
                    }
                    else {
                        setCheckBoxEnabled($('#<%=chkServiceIsDiscount.ClientID %>'), false);
                    }
                }
            });
        }
        else {
            $('#lblTestPartner').attr('class', 'lblDisabled');
            $('#<%=txtTestPartnerCode.ClientID %>').val('');
            $('#<%=txtTestPartnerName.ClientID %>').val('');
            $('#<%=hdnTestPartnerID.ClientID %>').val('');
            $('#<%=txtTestPartnerCode.ClientID %>').attr('readonly', 'readonly');

            $('#<%=hdnServiceItemID.ClientID %>').val('');
            $('#<%=hdnServiceRevenueSharingID.ClientID %>').val('');
            $('#<%=txtServiceItemCode.ClientID %>').val('');
            $('#<%=txtServiceItemName.ClientID %>').val('');
            $('#<%:txtServiceQty.ClientID %>').removeAttr("disabled");
            $('#<%=txtServiceQty.ClientID %>').val('1');

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

            $('#<%=hdnServiceCoverageAmount.ClientID %>').val('0');

            $('#<%=hdnServiceIsCoverageInPercentage.ClientID %>').val('0');

            $('#<%=hdnServiceDiscountAmount.ClientID %>').val('0');
            $('#<%=hdnServiceDiscountAmountComp1.ClientID %>').val('0');
            $('#<%=hdnServiceDiscountAmountComp2.ClientID %>').val('0');
            $('#<%=hdnServiceDiscountAmountComp3.ClientID %>').val('0');

            $('#<%=hdnServiceIsDiscountUsedComp.ClientID %>').val('0');

            $('#<%=hdnServiceIsDicountInPercentage.ClientID %>').val('0');
            $('#<%=hdnServiceIsDicountInPercentageComp1.ClientID %>').val('0');
            $('#<%=hdnServiceIsDicountInPercentageComp2.ClientID %>').val('0');
            $('#<%=hdnServiceIsDicountInPercentageComp3.ClientID %>').val('0');

            $('#<%=hdnServiceIsCITOInPercentage.ClientID %>').val('0');
            $('#<%=hdnServiceBaseCITOAmount.ClientID %>').val('0');
            $('#<%=hdnServiceIsComplicationInPercentage.ClientID %>').val('0');
            $('#<%=hdnServiceBaseComplicationAmount.ClientID %>').val('0');

            $('#<%=hdnIsAllowVariableTariffComp1.ClientID %>').val('0');
            $('#<%=hdnIsAllowVariableTariffComp2.ClientID %>').val('0');
            $('#<%=hdnIsAllowVariableTariffComp3.ClientID %>').val('0');

            $('#<%=hdnIsAllowDiscountTariffComp1.ClientID %>').val('0');
            $('#<%=hdnIsAllowDiscountTariffComp2.ClientID %>').val('0');
            $('#<%=hdnIsAllowDiscountTariffComp3.ClientID %>').val('0');

            calculateServiceTariffTotal();

            $('#<%=txtServiceCITO.ClientID %>').val('0').trigger('changeValue');
            $('#<%=txtServiceDiscount.ClientID %>').val('0').trigger('changeValue');
            $('#<%=txtServiceComplication.ClientID %>').val('0').trigger('changeValue');

            $('#<%=txtServiceUnitTariff.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtServiceDiscount.ClientID %>').attr('readonly', 'readonly');

            calculateServiceDiscountTotal();
            calculateServiceTotal();
        }
    }
    //#endregion

    function getServiceTariff() {
        var itemID = $('#<%=hdnServiceItemID.ClientID %>').val();
        if (itemID != '') {
            showLoadingPanel();
            var registrationID = getRegistrationID();
            var visitID = getVisitID();
            var classID = cboServiceChargeClassID.GetValue();
            var trxDate = getTrxDate();
            Methods.getItemTariff(registrationID, visitID, classID, itemID, trxDate, function (result) {
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
                    $('#<%=txtServiceTariffComp1.ClientID %>').val(result.PriceComp1).trigger('changeValue');
                    $('#<%=txtServiceTariffComp2.ClientID %>').val(result.PriceComp2).trigger('changeValue');
                    $('#<%=txtServiceTariffComp3.ClientID %>').val(result.PriceComp3).trigger('changeValue');

                    $('#<%=hdnServiceIsCoverageInPercentage.ClientID %>').val(result.IsCoverageInPercentage ? '1' : '0');
                    $('#<%=hdnServiceCoverageAmount.ClientID %>').val(result.CoverageAmount);

                    $('#<%=hdnServiceDiscountAmount.ClientID %>').val(result.DiscountAmount);
                    $('#<%=hdnServiceDiscountAmountComp1.ClientID %>').val(result.DiscountAmountComp1);
                    $('#<%=hdnServiceDiscountAmountComp2.ClientID %>').val(result.DiscountAmountComp2);
                    $('#<%=hdnServiceDiscountAmountComp3.ClientID %>').val(result.DiscountAmountComp3);

                    $('#<%=hdnServiceIsDiscountUsedComp.ClientID %>').val(result.IsDiscountUsedComp ? '1' : '0');

                    $('#<%=hdnServiceIsDicountInPercentage.ClientID %>').val(result.IsDiscountInPercentage ? '1' : '0');
                    $('#<%=hdnServiceIsDicountInPercentageComp1.ClientID %>').val(result.IsDiscountInPercentageComp1 ? '1' : '0');
                    $('#<%=hdnServiceIsDicountInPercentageComp2.ClientID %>').val(result.IsDiscountInPercentageComp2 ? '1' : '0');
                    $('#<%=hdnServiceIsDicountInPercentageComp3.ClientID %>').val(result.IsDiscountInPercentageComp3 ? '1' : '0');

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
                    $('#<%=txtServiceCostAmount.ClientID %>').val('0').trigger('changeValue');
                    $('#<%=txtServiceTariffComp1.ClientID %>').val('0').trigger('changeValue');
                    $('#<%=txtServiceTariffComp2.ClientID %>').val('0').trigger('changeValue');
                    $('#<%=txtServiceTariffComp3.ClientID %>').val('0').trigger('changeValue');

                    $('#<%=hdnServiceDiscountAmount.ClientID %>').val('0');
                    $('#<%=hdnServiceDiscountAmountComp1.ClientID %>').val('0');
                    $('#<%=hdnServiceDiscountAmountComp2.ClientID %>').val('0');
                    $('#<%=hdnServiceDiscountAmountComp3.ClientID %>').val('0');

                    $('#<%=hdnServiceCoverageAmount.ClientID %>').val('0');

                    $('#<%=hdnServiceIsDiscountUsedComp.ClientID %>').val('0');

                    $('#<%=hdnServiceIsDicountInPercentage.ClientID %>').val('0');
                    $('#<%=hdnServiceIsDicountInPercentageComp1.ClientID %>').val('0');
                    $('#<%=hdnServiceIsDicountInPercentageComp2.ClientID %>').val('0');
                    $('#<%=hdnServiceIsDicountInPercentageComp3.ClientID %>').val('0');

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

                calculateServiceDiscountTotal();
                calculateServiceTotal();
            });
            hideLoadingPanel();
        }
    }
    //#endregion

    //#region Physician
    $('#lblPhysician.lblLink').die('click');
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
                    var tempDate = "00";
                    if (today.getDate() < 10) {
                        tempDate = "0" + today.getDate();
                    } else {
                        tempDate = today.getDate();
                    }
                    var pad = "00";
                    var tmpMonth = (today.getMonth() + 1).toString();
                    var month = pad.substring(0, pad.length - tmpMonth.length) + tmpMonth;
                    var date = tempDate + '-' + month + '-' + today.getFullYear();
                    var time = today.getHours() + ":" + today.getMinutes();

                    var hdnTransactionDateServiceCtl = $('#<%=hdnTransactionDateServiceCtl.ClientID %>').val() == "" ? date : $('#<%=hdnTransactionDateServiceCtl.ClientID %>').val();
                    var hdnTransactionTime = $('#<%=hdnTransactionTime.ClientID %>').val() == "" ? time : $('#<%=hdnTransactionTime.ClientID %>').val();
                    var hdnChargesHealthcareServiceUnitID = $('#<%=hdnHealthcareServiceUnitIDServiceCtl.ClientID %>').val();

                    Methods.getItemRevenueSharing(itemCode, $('#<%=hdnServicePhysicianID.ClientID %>').val(), cboServiceChargeClassID.GetValue(), '<%=GetMainParamedicRole() %>', $('#<%=hdnVisitIDCtl.ClientID %>').val(), hdnChargesHealthcareServiceUnitID, hdnTransactionDateServiceCtl, hdnTransactionTime, function (result) {
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
        openSearchDialog('testpartner', onGetTestPartnerFilterExpression(), function (value) {
            $('#<%=txtTestPartnerCode.ClientID %>').val(value);
            onTxtTestPartnerCodeChanged(value);
        });
    });

    $('#<%=txtTestPartnerCode.ClientID %>').live('change', function () {
        onTxtTestPartnerCodeChanged($(this).val());
    });

    function onTxtTestPartnerCodeChanged(value) {
        var filterExpression = onGetTestPartnerFilterExpression() + " AND BusinessPartnerCode = '" + value + "'";
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

    function oncboGCDiscountReasonChargesDtValueChanged() {
        var discountReason = cboGCDiscountReasonChargesDt.GetValue();

        if (discountReason == "X550^999") {
            $('#<%=txtDiscountReasonChargesDt.ClientID %>').removeAttr('readonly');
        } else {
            $('#<%=txtDiscountReasonChargesDt.ClientID %>').val("");
            $('#<%=txtDiscountReasonChargesDt.ClientID %>').attr('readonly', 'readonly');
        }
    }

    function onCboServiceChargeClassIDValueChanged() {
        var itemCode = $('#<%=txtServiceItemCode.ClientID %>').val();
        if (itemCode != '') {
            var today = new Date();
            var tempDate = "00";
            if (today.getDate() < 10) {
                tempDate = "0" + today.getDate();
            } else {
                tempDate = today.getDate();
            }
            var pad = "00";
            var tmpMonth = (today.getMonth() + 1).toString();
            var month = pad.substring(0, pad.length - tmpMonth.length) + tmpMonth;
            var date = tempDate + '-' + month + '-' + today.getFullYear();
            var time = today.getHours() + ":" + today.getMinutes();

            var hdnTransactionDateServiceCtl = $('#<%=hdnTransactionDateServiceCtl.ClientID %>').val() == "" ? date : $('#<%=hdnTransactionDateServiceCtl.ClientID %>').val();
            var hdnTransactionTime = $('#<%=hdnTransactionTime.ClientID %>').val() == "" ? time : $('#<%=hdnTransactionTime.ClientID %>').val();
            var hdnChargesHealthcareServiceUnitID = $('#<%=hdnHealthcareServiceUnitIDServiceCtl.ClientID %>').val();

            if ($('#<%=hdnServicePhysicianID.ClientID %>').val() != '') {
                Methods.getItemRevenueSharing(itemCode, $('#<%=hdnServicePhysicianID.ClientID %>').val(), cboServiceChargeClassID.GetValue(), '<%=GetMainParamedicRole() %>', $('#<%=hdnVisitIDCtl.ClientID %>').val(), hdnChargesHealthcareServiceUnitID, hdnTransactionDateServiceCtl, hdnTransactionTime, function (result) {
                    if (result != null)
                        $('#<%=hdnServiceRevenueSharingID.ClientID %>').val(result.RevenueSharingID);
                    else
                        $('#<%=hdnServiceRevenueSharingID.ClientID %>').val('');
                });
            }
            else {
                $('#<%=hdnServiceRevenueSharingID.ClientID %>').val('');
            }

            getServiceTariff();
        }
    }

    function calculateServiceTariffTotal() {
        var tariff = parseFloat($('#<%=txtServiceUnitTariff.ClientID %>').attr('hiddenVal'));
        var qty = parseFloat($('#<%=txtServiceQty.ClientID %>').val());
        $('#<%=txtServiceTariff.ClientID %>').val(tariff * qty).trigger('changeValue');
    }

    function calculateServiceDiscountTotal() {
        var discountAmount = parseFloat($('#<%=hdnServiceDiscountAmount.ClientID %>').val());
        var discountAmountComp1 = parseFloat($('#<%=hdnServiceDiscountAmountComp1.ClientID %>').val());
        var discountAmountComp2 = parseFloat($('#<%=hdnServiceDiscountAmountComp2.ClientID %>').val());
        var discountAmountComp3 = parseFloat($('#<%=hdnServiceDiscountAmountComp3.ClientID %>').val());

        var isDiscountUsedComp = ($('#<%=hdnServiceIsDiscountUsedComp.ClientID %>').val() == '1');

        var isDicountInPercentage = ($('#<%=hdnServiceIsDicountInPercentage.ClientID %>').val() == '1');
        var isDicountInPercentageComp1 = ($('#<%=hdnServiceIsDicountInPercentageComp1.ClientID %>').val() == '1');
        var isDicountInPercentageComp2 = ($('#<%=hdnServiceIsDicountInPercentageComp2.ClientID %>').val() == '1');
        var isDicountInPercentageComp3 = ($('#<%=hdnServiceIsDicountInPercentageComp3.ClientID %>').val() == '1');

        var tariff = parseFloat($('#<%=txtServiceTariff.ClientID %>').attr('hiddenVal'));
        var tariffComp1 = parseFloat($('#<%=txtServiceTariffComp1.ClientID %>').attr('hiddenVal'));
        var tariffComp2 = parseFloat($('#<%=txtServiceTariffComp2.ClientID %>').attr('hiddenVal'));
        var tariffComp3 = parseFloat($('#<%=txtServiceTariffComp3.ClientID %>').attr('hiddenVal'));
        var tariffCITO = parseFloat($('#<%=txtServiceCITO.ClientID %>').attr('hiddenVal'));

        var qty = parseFloat($('#<%=txtServiceQty.ClientID %>').val());

        var discountTotal = 0;
        var discountComp1 = 0;
        var discountComp2 = 0;
        var discountComp3 = 0;
        var discountCITO = 0;

        if (discountAmount > 0) {
            if (isDicountInPercentage) {
                if (discountAmount > 100) {
                    discountAmount = 100;
                }
                discountCITO = (tariffCITO * discountAmount) / 100;
            }
            else {
                discountCITO = 0
            }
        }

        if (isDiscountUsedComp == "1") {
            if (tariffComp1 > 0) {
                if (isDicountInPercentageComp1 == "1") {
                    discountComp1 = tariffComp1 * discountAmountComp1 / 100;

                    $('#<%=txtServiceDiscPercentComp1.ClientID %>').val(discountAmountComp1).trigger('changeValue');
                } else {
                    discountComp1 = discountAmountComp1;
                }
            }

            if (tariffComp2 > 0) {
                if (isDicountInPercentageComp2 == "1") {
                    discountComp2 = tariffComp2 * discountAmountComp2 / 100;

                    $('#<%=txtServiceDiscPercentComp2.ClientID %>').val(discountAmountComp2).trigger('changeValue');
                } else {
                    discountComp2 = discountAmountComp2;
                }
            }

            if (tariffComp3 > 0) {
                if (isDicountInPercentageComp3 == "1") {
                    discountComp3 = tariffComp3 * discountAmountComp3 / 100;

                    $('#<%=txtServiceDiscPercentComp3.ClientID %>').val(discountAmountComp3).trigger('changeValue');
                } else {
                    discountComp3 = discountAmountComp3;
                }
            }
        } else {
            if (tariffComp1 > 0) {
                if (isDicountInPercentage) {
                    discountComp1 = tariffComp1 * discountAmount / 100;
                } else {
                    discountComp1 = discountAmount;
                }
            }
            if (tariffComp2 > 0) {
                if (isDicountInPercentage) {
                    discountComp2 = tariffComp2 * discountAmount / 100;
                } else {
                    discountComp2 = discountAmount;
                }
            }
            if (tariffComp3 > 0) {
                if (isDicountInPercentage) {
                    discountComp3 = tariffComp3 * discountAmount / 100;
                } else {
                    discountComp3 = discountAmount;
                }
            }

            $('#<%=txtServiceDiscPercentComp1.ClientID %>').val(0).trigger('changeValue');
            $('#<%=txtServiceDiscPercentComp2.ClientID %>').val(0).trigger('changeValue');
            $('#<%=txtServiceDiscPercentComp3.ClientID %>').val(0).trigger('changeValue');
        }

        discountTotal = (discountComp1 + discountComp2 + discountComp3) * qty;

        if (discountTotal != 0) {
            $('#<%=chkServiceIsDiscount.ClientID %>').prop('checked', true);
        } else {
            $('#<%=chkServiceIsDiscount.ClientID %>').prop('checked', false);
        }

        $('#<%=hdnDiscComp1.ClientID %>').val(discountComp1);
        $('#<%=hdnDiscComp2.ClientID %>').val(discountComp2);
        $('#<%=hdnDiscComp3.ClientID %>').val(discountComp3);

        $('#<%=txtServiceDiscount.ClientID %>').val(discountTotal).trigger('changeValue');
        $('#<%=txtServiceDiscComp1.ClientID %>').val(discountComp1).trigger('changeValue');
        $('#<%=txtServiceDiscComp2.ClientID %>').val(discountComp2).trigger('changeValue');
        $('#<%=txtServiceDiscComp3.ClientID %>').val(discountComp3).trigger('changeValue');

        $('#<%=txtServiceCITODisc.ClientID %>').val(discountCITO).trigger('changeValue');
    }

    function calculateServiceTotal() {
        var tariff = parseFloat($('#<%=txtServiceTariff.ClientID %>').attr('hiddenVal'));
        var cito = parseFloat($('#<%=txtServiceCITO.ClientID %>').attr('hiddenVal'));
        var complication = parseFloat($('#<%=txtServiceComplication.ClientID %>').attr('hiddenVal'));
        var discount = parseFloat($('#<%=txtServiceDiscount.ClientID %>').attr('hiddenVal'));
        var qty = parseFloat($('#<%=txtServiceQty.ClientID %>').val());
        var total = tariff + cito + complication - discount;

        var coverageAmount = parseFloat($('#<%=hdnServiceCoverageAmount.ClientID %>').val());
        var isCoverageInPercentage = ($('#<%=hdnServiceIsCoverageInPercentage.ClientID %>').val() == '1');
        var totalPayer = 0;

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

        var totalPatient = 0;

        if ($('#<%=hdnBusinessPartnerIDServiceCtl.ClientID %>').val() == "1") {
            totalPatient = total;
        } else {
            totalPatient = total - totalPayer;
        }

        var totalAllPayer = parseFloat($('#<%=hdnServiceAllTotalPayer.ClientID %>').val());
        totalAllPayer = (totalAllPayer - parseFloat($('#<%=txtServicePayer.ClientID %>').attr('hiddenVal')));
        totalAllPayer += totalPayer;
        $('#<%=hdnServiceAllTotalPayer.ClientID %>').val(totalAllPayer);
        $('.tdServiceTotalPayer').html(totalAllPayer.formatMoney(2, '.', ','));

        var totalAllPatient = parseFloat($('#<%=hdnServiceAllTotalPatient.ClientID %>').val());
        totalAllPatient = (totalAllPatient - parseFloat($('#<%=txtServicePatient.ClientID %>').attr('hiddenVal')));
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
<input type="hidden" id="hdnIsLaboratoryUnit" runat="server" value="" />
<input type="hidden" id="hdnLabHealthcareServiceUnitID" runat="server" value="" />
<input type="hidden" id="hdnImagingServiceUnitID" runat="server" value="" />
<input type="hidden" id="hdnLaboratoryServiceUnitID" runat="server" value="" />
<input type="hidden" id="hdnServiceTransactionDtID" runat="server" value="" />
<input type="hidden" id="hdnServiceDtTotal" runat="server" value="" />
<input type="hidden" id="hdnRegistrationID" runat="server" value="" />
<input type="hidden" id="hdnVisitIDCtl" runat="server" value="" />
<input type="hidden" id="hdnTariffComp1Text" runat="server" value="" />
<input type="hidden" id="hdnTariffComp2Text" runat="server" value="" />
<input type="hidden" id="hdnTariffComp3Text" runat="server" value="" />
<input type="hidden" id="hdnPrescriptionReturnItem" runat="server" value="" />
<input type="hidden" id="hdnIsEditable" runat="server" value="" />
<input type="hidden" id="hdnIsHasProcedure" runat="server" value="" />
<input type="hidden" id="hdnBPJSRegistration" runat="server" value="" />
<input type="hidden" id="hdnBusinessPartnerIDServiceCtl" runat="server" value="" />
<input type="hidden" id="hdnIsOnlyBPJS" runat="server" value="" />
<input type="hidden" id="hdnTempTotalPatient" runat="server" value="" />
<input type="hidden" id="hdnTempTotalPayer" runat="server" value="" />
<input type="hidden" id="hdnDiscTotal" runat="server" value="" />
<input type="hidden" id="hdnDiscComp1" runat="server" value="" />
<input type="hidden" id="hdnDiscComp2" runat="server" value="" />
<input type="hidden" id="hdnDiscComp3" runat="server" value="" />
<input type="hidden" id="hdnDiscCompCito" runat="server" value="" />
<input type="hidden" id="hdnDepartmentID" runat="server" value="" />
<input type="hidden" id="hdnIsPackageItem" runat="server" value="" />
<input type="hidden" id="hdnIsUsingAccumulatedPriceItem" runat="server" value="" />
<input type="hidden" id="hdnDateToday" runat="server" value="" />
<input type="hidden" id="hdnDefaultExpiredDateSetvar" runat="server" value="" />
<input type="hidden" id="hdnDefaultExpiredDate" runat="server" value="" />
<input type="hidden" id="hdnDefaultExpiredDateAgeUnit" runat="server" value="" />
<input type="hidden" id="hdnIsAIOTransactionServiceCtl" runat="server" value="" />
<input type="hidden" id="hdnIsChargesGenerateMCUServiceCtl" runat="server" value="" />
<input type="hidden" id="hdnIsOnlyBPJSLab" runat="server" value="" />
<input type="hidden" id="hdnIsOnlyBPJSRad" runat="server" value="" />
<input type="hidden" id="hdnIsOnlyBPJSOth" runat="server" value="" />
<input type="hidden" id="hdnIsEndingAmountRoundingTo1" runat="server" value="0" />
<input type="hidden" id="hdnIsUsingValidateDigitDecimal" runat="server" value="0" />
<div id="containerEntryService" style="margin-top: 4px; display: none;">
    <div class="pageTitle">
        <%=GetLabel("Tambah / Ubah Data")%></div>
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
                                <input type="hidden" value="" id="hdnServiceItemID" runat="server" />
                                <input type="hidden" value="" id="hdnServiceRevenueSharingID" runat="server" />
                                <input type="hidden" value="" id="hdnServiceItemUnit" runat="server" />
                                <input type="hidden" value="" id="hdnServiceBaseTariff" runat="server" />
                                <input type="hidden" value="" id="hdnServiceCoverageAmount" runat="server" />
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
                                <input type="hidden" value="" id="hdnHealthcareServiceUnitIDServiceCtl" runat="server" />
                                <input type="hidden" value="" id="hdnTransactionDateServiceCtl" runat="server" />
                                <input type="hidden" value="" id="hdnTransactionTime" runat="server" />
                                <input type="hidden" value="" id="hdnServiceVisitID" runat="server" />
                                <input type="hidden" value="" id="hdnServiceUnitName" runat="server" />
                                <input type="hidden" value="0" id="hdnIsAllowVariableTariffComp1" runat="server" />
                                <input type="hidden" value="0" id="hdnIsAllowVariableTariffComp2" runat="server" />
                                <input type="hidden" value="0" id="hdnIsAllowVariableTariffComp3" runat="server" />
                                <input type="hidden" value="0" id="hdnServiceDiscountAmount" runat="server" />
                                <input type="hidden" value="0" id="hdnServiceDiscountAmountComp1" runat="server" />
                                <input type="hidden" value="0" id="hdnServiceDiscountAmountComp2" runat="server" />
                                <input type="hidden" value="0" id="hdnServiceDiscountAmountComp3" runat="server" />
                                <input type="hidden" value="0" id="hdnServiceIsDiscountUsedComp" runat="server" />
                                <input type="hidden" value="0" id="hdnServiceIsDicountInPercentage" runat="server" />
                                <input type="hidden" value="0" id="hdnServiceIsDicountInPercentageComp1" runat="server" />
                                <input type="hidden" value="0" id="hdnServiceIsDicountInPercentageComp2" runat="server" />
                                <input type="hidden" value="0" id="hdnServiceIsDicountInPercentageComp3" runat="server" />
                                <input type="hidden" value="0" id="hdnIsAllowDiscountTariffComp1" runat="server" />
                                <input type="hidden" value="0" id="hdnIsAllowDiscountTariffComp2" runat="server" />
                                <input type="hidden" value="0" id="hdnIsAllowDiscountTariffComp3" runat="server" />
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
                        <tr id="trItemNameVariable" runat="server" style="display: none">
                            <td class="tdLabel">
                                <label class="lblNormal" id="lblItemVariable">
                                    <%=GetLabel("Nama Item (Variable)")%></label>
                            </td>
                            <td class="tdLabel">
                                <asp:TextBox ID="txtItemNameVariable" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink lblMandatory" id="lblPhysician">
                                    <%=GetLabel("Dokter/Paramedis")%></label>
                            </td>
                            <td>
                                <input type="hidden" value="" id="hdnIsPackageBalanceItem" runat="server" />
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
                                <input type="hidden" value="" id="hdnIsSubContractItem" runat="server" />
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
                                <asp:TextBox ID="txtServiceUnitTariff" Width="100px" CssClass="txtCurrency" runat="server"
                                    ReadOnly="true" />
                                <input type="button" id="btnEditUnitTariff" title='<%=GetLabel("Unit Tariff Component") %>'
                                    value="..." style="width: 10%" />
                            </td>
                        </tr>
                        <tr id="trJumlah" runat="server">
                            <td class="tdLabel">
                                <label>
                                    <%=GetLabel("Jumlah")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtServiceQty" Width="100px" CssClass="number" runat="server" />
                            </td>
                        </tr>
                        <tr id="trExpiredDate" style="display: none" runat="server">
                            <td class="tdLabel">
                                <label>
                                    <%=GetLabel("Tgl Kadaluarsa")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtExpiredDate" Width="100px" CssClass="datepicker" runat="server" />
                            </td>
                        </tr>
                        <tr id="trPackageQty" style="display: none;" runat="server">
                            <td class="tdLabel">
                                <label>
                                    <%=GetLabel("Jumlah Paket")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPackageQty" Width="100px" CssClass="number" ReadOnly="true" runat="server" />
                            </td>
                        </tr>
                        <tr id="trPackageQtyTaken" style="display: none" runat="server">
                            <td class="tdLabel">
                                <label>
                                    <%=GetLabel("Jumlah Diambil")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPackageQtyTaken" Width="100px" CssClass="number" runat="server" />
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
<dxcp:ASPxCallbackPanel ID="cbpService" runat="server" Width="100%" ClientInstanceName="cbpService"
    ShowLoadingPanel="false" OnCallback="cbpService_Callback">
    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e) { onCbpServiceEndCallback(s); }" />
    <PanelCollection>
        <dx:PanelContent ID="PanelContent1" runat="server">
            <asp:Panel runat="server" ID="pnlService" Style="width: 100%; margin-left: auto;
                margin-right: auto; position: relative; font-size: 0.95em;">
                <input type="hidden" id="hdnServiceAllTotalPatient" runat="server" value="" />
                <input type="hidden" id="hdnServiceAllTotalPayer" runat="server" value="" />
                <input type="hidden" value='<%#: Eval("ID") %>' bindingfield="ID" runat="server"
                    id="hdnChargesDt" />
                <asp:ListView ID="lvwService" runat="server">
                    <LayoutTemplate>
                        <table id="tblView" runat="server" class="grdService grdNormal notAllowSelect" cellspacing="0"
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
                                <th rowspan="2">
                                    &nbsp;
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
                                            <td>
                                                <img class="imgServiceParamedic imgLink" <%# (IsEditable.ToString() == "True" && IsShowParamedicTeam.ToString() == "True") || (IsEditable.ToString() == "True")  ?  "" : "style='display:none'" %>
                                                    title='<%=GetLabel("Tim Medis/Tenaga Medis")%>' src='<%# IsEditable.ToString() == "False" || Eval("IsVerified").ToString() == "True" || Eval("IsReviewed").ToString() == "True" ? ResolveUrl("~/Libs/Images/Button/paramedic_team_disabled.png") : ResolveUrl("~/Libs/Images/Button/paramedic_team.png")%>'
                                                    alt="" style="margin-right: 2px" />
                                            </td>
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
                                                    title='<%=GetLabel("Lihat Paket")%>' src='<%# ResolveUrl("~/Libs/Images/Button/verify.png")%>'
                                                    alt="" style="display: none" />
                                            </td>
                                            <td>
                                                <img class="imgIsEditPackageItemAccumulated imgLink" <%# (IsEditable.ToString() == "True" && Eval("IsPackageItem").ToString() == "True" && Eval("IsUsingAccumulatedPrice").ToString() == "True") ? "imgLink" : "style='display:none'"%>
                                                    title='<%=GetLabel("Edit Paket Akumulasi")%>' src='<%# (IsEditable.ToString() == "False" && Eval("IsPackageItem").ToString() == "True" && Eval("IsUsingAccumulatedPrice").ToString() == "True") ? ResolveUrl("~/Libs/Images/Button/package_disabled.png") : ResolveUrl("~/Libs/Images/Button/package.png")%>'
                                                    alt="" />
                                            </td>
                                            <td>
                                                <img class="imgIsEditPackageItemNonAccumulated imgLink" <%# (IsEditable.ToString() == "True" && Eval("IsPackageItem").ToString() == "True" && Eval("IsUsingAccumulatedPrice").ToString() == "False") ? "imgLink" : "style='display:none'"%>
                                                    title='<%=GetLabel("Edit Paket Non-Akumulasi")%>' src='<%# (IsEditable.ToString() == "False" && Eval("IsPackageItem").ToString() == "True" && Eval("IsUsingAccumulatedPrice").ToString() == "False") ? ResolveUrl("~/Libs/Images/Button/package_disabled.png") : ResolveUrl("~/Libs/Images/Button/package.png")%>'
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
                                    <input type="hidden" value='<%#: Eval("IsAllowChangeQty") %>' bindingfield="IsAllowChangeQty" />
                                    <input type="hidden" value='<%#: Eval("IsAllowDiscountTariffComp1") %>' bindingfield="IsAllowDiscountTariffComp1" />
                                    <input type="hidden" value='<%#: Eval("IsAllowDiscountTariffComp2") %>' bindingfield="IsAllowDiscountTariffComp2" />
                                    <input type="hidden" value='<%#: Eval("IsAllowDiscountTariffComp3") %>' bindingfield="IsAllowDiscountTariffComp3" />
                                    <input type="hidden" value='<%#: Eval("IsAllowVariable") %>' bindingfield="IsAllowVariable" />
                                    <input type="hidden" value='<%#: Eval("IsAllowVariableTariffComp1") %>' bindingfield="IsAllowVariableTariffComp1" />
                                    <input type="hidden" value='<%#: Eval("IsAllowVariableTariffComp2") %>' bindingfield="IsAllowVariableTariffComp2" />
                                    <input type="hidden" value='<%#: Eval("IsAllowVariableTariffComp3") %>' bindingfield="IsAllowVariableTariffComp3" />
                                    <input type="hidden" value='<%#: Eval("IsAllowUnbilledItem") %>' bindingfield="IsAllowUnbilledItem" />
                                    <input type="hidden" value='<%#: Eval("IsCITO") %>' bindingfield="IsCITO" />
                                    <input type="hidden" value='<%#: Eval("IsCITOInPercentage") %>' bindingfield="IsCITOInPercentage" />
                                    <input type="hidden" value='<%#: Eval("IsComplication") %>' bindingfield="IsComplication" />
                                    <input type="hidden" value='<%#: Eval("IsComplicationInPercentage") %>' bindingfield="IsComplicationInPercentage" />
                                    <input type="hidden" value='<%#: Eval("IsVariable") %>' bindingfield="IsVariable" />
                                    <input type="hidden" value='<%#: Eval("DefaultTariffComp") %>' bindingfield="DefaultTariffComp" />
                                    <input type="hidden" value='<%#: Eval("IsUnbilledItem") %>' bindingfield="IsUnbilledItem" />
                                    <input type="hidden" value='<%#: Eval("BaseCITOAmount") %>' bindingfield="BaseCITOAmount" />
                                    <input type="hidden" value='<%#: Eval("CITOAmount") %>' bindingfield="CITOAmount" />
                                    <input type="hidden" value='<%#: Eval("CITODiscount") %>' bindingfield="CITODiscount" />
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
                                    <input type="hidden" value='<%#: Eval("BaseComplicationAmount") %>' bindingfield="BaseComplicationAmount" />
                                    <input type="hidden" value='<%#: Eval("ComplicationAmount") %>' bindingfield="ComplicationAmount" />
                                    <input type="hidden" value='<%#: Eval("PatientAmount") %>' bindingfield="PatientAmount" />
                                    <input type="hidden" value='<%#: Eval("PayerAmount") %>' bindingfield="PayerAmount" />
                                    <input type="hidden" value='<%#: Eval("LineAmount") %>' bindingfield="LineAmount" />
                                    <input type="hidden" value='<%#: Eval("IsSubContractItem") %>' bindingfield="IsSubContractItem" />
                                    <input type="hidden" value='<%#: Eval("PreconditionNotes") %>' bindingfield="PreconditionNotes" />
                                    <input type="hidden" value='<%#: Eval("GCDiscountReason") %>' bindingfield="GCDiscountReason" />
                                    <input type="hidden" value='<%#: Eval("DiscountReason") %>' bindingfield="DiscountReason" />
                                    <input type="hidden" value='<%#: Eval("IsPackageItem") %>' bindingfield="IsPackageItem" />
                                    <input type="hidden" value='<%#: Eval("IsUsingAccumulatedPrice") %>' bindingfield="IsUsingAccumulatedPrice" />
                                    <input type="hidden" value='<%#: Eval("IsPackageBalanceItem") %>' bindingfield="IsPackageBalanceItem" />
                                    <input type="hidden" value='<%#: Eval("PackageBalanceQtyTaken") %>' bindingfield="PackageBalanceQtyTaken" />
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
                                    <div>
                                        <i>
                                            <%#: Eval("cfTestPartnerNameWithCaption")%>
                                        </i>
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
                                <img class="imgServiceSwitch imgLink" title='<%=GetLabel("Switch")%>' src='<%# ResolveUrl("~/Libs/Images/Button/switch.png")%>'
                                    alt="" />
                            </td>
                            <td <%# Eval("IsUnitPriceOverLimit").ToString() == "True" ? "" : "style='display:none'" %>
                                valign="middle">
                                <img src='<%# Eval("IsUnitPriceOverLimit").ToString() == "True" && Eval("IsConfirmed").ToString() == "True" ? ResolveUrl("~/Libs/Images/Status/coverage_ok.png") : ResolveUrl("~/Libs/Images/Status/coverage_warning.png")%>'
                                    title='<%=GetLabel("ServiceUnitPrice = ")%><%# Eval("cfServiceUnitPriceInString") %><%=GetLabel("\nDrugSuppliesUnitPrice = ")%><%# Eval("cfDrugSuppliesUnitPriceInString") %><%=GetLabel("\nLogisticUnitPrice = ")%><%# Eval("cfLogisticUnitPriceInString") %>'
                                    alt="" style="width: 30px" />
                            </td>
                            <td <%# Eval("PreconditionNotes").ToString() != "" ?  "" : "style='display:none'" %>
                                valign="middle">
                                <img class="imgPreconditionNotesCtl imgLink" <%# Eval("PreconditionNotes").ToString() != "" ?  "" : "style='display:none'" %>
                                    title='<%=GetLabel("Precondition Notes")%>' src='<%# ResolveUrl("~/Libs/Images/Button/info.png")%>'
                                    alt="" />
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
                <div class="imgLoadingGrdView" id="containerImgLoadingService">
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
                <div style="width: 100%; text-align: center" id="divListAddButton" runat="server">
                    <span class="lblLink" id="lblServiceAddData" style="margin-right: 200px;">
                        <%= GetLabel("Tambah Data")%></span> <span class="lblLink" id="lblServiceQuickPick"
                            style="margin-right: 200px;">
                            <%= GetLabel("Quick Picks")%></span> <span class="lblLink" id="lblTemplatePanel"
                                style="margin-right: 200px;">
                                <%= GetLabel("Template")%></span> <span class="lblLink" id="lblServicePackageQuickPick"
                                    style="margin-right: 200px;">
                                    <%= GetLabel("Paket Kunjungan")%></span> <span class="lblLink" id="lblServiceAIOTransactionQuickPick"
                                        style="margin-right: 200px;">
                                        <%= GetLabel("Salin Detail AIO")%></span> <span class="lblLink" id="lblCopyMultiVisitScheduleOrder"
                                        style="margin-right: 200px; display:none">
                                        <%= GetLabel("Quick Picks Multi Kunjungan")%></span>
                </div>
            </asp:Panel>
        </dx:PanelContent>
    </PanelCollection>
</dxcp:ASPxCallbackPanel>
<!-- Pop Up Unit Tariff-->
<dxpc:ASPxPopupControl ID="pcUnitTariff" runat="server" ClientInstanceName="pcUnitTariff"
    CloseAction="CloseButton" Height="200px" HeaderText="Harga Satuan" Width="200px"
    Modal="True" PopupAction="None" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
    <ContentCollection>
        <dxpc:PopupControlContentControl runat="server" ID="pccc1">
            <dx:ASPxPanel ID="pnlTariffComp" runat="server" Width="100%">
                <PanelCollection>
                    <dx:PanelContent ID="PanelContent2" runat="server">
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
                                                            <asp:TextBox ID="txtServiceTotalTariff" runat="server" ReadOnly="true" Width="100%"
                                                                CssClass="txtCurrency txtServiceTotal" /></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdLabel">
                                                        <label class="lblNormal">
                                                            <%=GetLabel("Cost")%></label>
                                                    </td>
                                                    <td>
                                                        <span>
                                                            <asp:TextBox ID="txtServiceCostAmount" runat="server" ReadOnly="true" Width="100%"
                                                                CssClass="txtCurrency txtServiceCostAmount" /></span>
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
    CloseAction="CloseButton" Height="200px" HeaderText="Diskon" Width="700px" Modal="True"
    PopupAction="None" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
    <ContentCollection>
        <dxpc:PopupControlContentControl runat="server" ID="pccc2">
            <dx:ASPxPanel ID="pnlDiscount" runat="server" Width="100%">
                <PanelCollection>
                    <dx:PanelContent ID="PanelContent3" runat="server">
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
                                                        <asp:TextBox ID="txtServiceDiscTotal" ReadOnly="true" runat="server" Width="100%"
                                                            CssClass="txtCurrency txtServiceDiscTotal" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdLabel" colspan="2">
                                                        <label class="lblNormal">
                                                            <%=GetLabel("Discount (%) ")%><%=GetTariffComponent1Text()%></label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtServiceDiscPercentComp1" ReadOnly="true" runat="server" Width="100%"
                                                            CssClass="txtCurrency txtServiceDiscPercentComp1" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdLabel" colspan="2">
                                                        <label class="lblNormal">
                                                            <%=GetLabel("Discount (%) ")%><%=GetTariffComponent2Text()%></label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtServiceDiscPercentComp2" ReadOnly="true" runat="server" Width="100%"
                                                            CssClass="txtCurrency txtServiceDiscPercentComp2" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdLabel" colspan="2">
                                                        <label class="lblNormal">
                                                            <%=GetLabel("Discount (%) ")%><%=GetTariffComponent3Text()%></label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtServiceDiscPercentComp3" ReadOnly="true" runat="server" Width="100%"
                                                            CssClass="txtCurrency txtServiceDiscPercentComp3" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdLabel" colspan="2">
                                                        <label class="lblNormal">
                                                            <%=GetLabel("Discount Reason")%></label>
                                                    </td>
                                                    <td>
                                                        <dxe:ASPxComboBox ID="cboGCDiscountReasonChargesDt" ClientInstanceName="cboGCDiscountReasonChargesDt"
                                                            Width="100%" runat="server">
                                                            <ClientSideEvents ValueChanged="function(s,e){ oncboGCDiscountReasonChargesDtValueChanged(); }" />
                                                        </dxe:ASPxComboBox>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="txtDiscountReasonChargesDt" runat="server" Width="100%" CssClass="txtDiscountReasonChargesDt" />
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
<!-- Pop Up Kelas Tagihan AIO-->
<dxpc:ASPxPopupControl ID="pcAIOEdit" runat="server" ClientInstanceName="pcAIOEdit"
    CloseAction="CloseButton" Height="120px" HeaderText="Edit Transaksi AIO" Width="350px"
    Modal="True" PopupAction="None" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
    <ContentCollection>
        <dxpc:PopupControlContentControl runat="server" ID="pcccAIO">
            <dx:ASPxPanel ID="ASPxPanel1" runat="server" Width="100%">
                <PanelCollection>
                    <dx:PanelContent ID="PanelContent4" runat="server">
                        <fieldset id="Fieldset1" style="margin: 0">
                            <div style="text-align: left; width: 100%;">
                                <table>
                                    <colgroup>
                                        <col style="width: 130px" />
                                        <col style="width: 180px" />
                                    </colgroup>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal">
                                                <%=GetLabel("Kelas Tagihan")%></label>
                                        </td>
                                        <td>
                                            <dxe:ASPxComboBox ID="cboEditAIOChargeClass" ClientInstanceName="cboEditAIOChargeClass"
                                                Width="100%" runat="server">
                                            </dxe:ASPxComboBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal">
                                                <%=GetLabel("Qty")%></label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAIOQty" runat="server" Width="100%" CssClass="txtAIOQty number" />
                                        </td>
                                    </tr>
                                    <tr style="margin-top: 20px; height: 100px; vertical-align: middle">
                                        <td align="center" colspan="2">
                                            <input type="button" id="btnSaveEditAIO" value='<%= GetLabel("Simpan")%>' onclick="cbpService.PerformCallback('editAIO');" />
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
