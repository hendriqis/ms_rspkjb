<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx2.master" AutoEventWireup="true"
    CodeBehind="PrescriptionEntryDetail.aspx.cs" Inherits="QIS.Medinfras.Web.Pharmacy.Program.PrescriptionEntryDetail"
    ValidateRequest="false" %>

<%@ Register Src="~/Libs/Controls/PatientBannerCtl.ascx" TagName="PatientBannerCtl"
    TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content4" ContentPlaceHolderID="plhCustomButtonToolbarLeft" runat="server">
    <li id="btnPrescriptionBack" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/back.png")%>' alt="" /><div>
            <%=GetLabel("Back")%></div>
    </li>
    <li id="btnVoid" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Void")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnPrescriptionCompoundEntry" runat="server" crudmode="C">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbCompound.png")%>' alt="" /><div>
            <%=GetLabel("Racikan")%></div>
    </li>
    <li id="btnClinicTransactionTestOrder" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbnotes.png")%>' alt="" /><div>
            <%=GetLabel("Informasi Order")%></div>
    </li>
    <li id="btnPrintPrescription" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprint.png")%>' alt="" /><div>
            <%=GetLabel("Print Etiket")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <table style="float: right" cellpadding="0" cellspacing="0">
        <colgroup>
            <col width="100px" />
        </colgroup>
        <tr>
            <td>
                <div id="divVisitNote" runat="server" style="text-align: left">
                    <img class="imgLink" id="imgVisitNote" src='<%= ResolveUrl("~/Libs/Images/Toolbar/registration_notes.png")%>'
                        alt="" title="<%=GetLabel("Catatan Pasien")%>" width="32" height="32" />
                </div>
            </td>
            <td>
                <div class="menuTitle">
                    <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">
    <input type="hidden" value="" id="hdnChargeDtID" runat="server" />
    <input type="hidden" value="" id="hdnDefaultPrescriptionOrderID" runat="server" />
    <input type="hidden" value="" id="hdnLinkedRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnGCRegistrationStatus" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnVisitID" runat="server" />
    <input type="hidden" value="" id="hdnClassID" runat="server" />
    <input type="hidden" value="" id="hdnMRN" runat="server" />
    <input type="hidden" value="" id="hdnImagingServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnLaboratoryServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnChargeClassID" runat="server" />
    <input type="hidden" value="" id="hdnDefaultLocationID" runat="server" />
    <input type="hidden" value="" id="hdnPrescriptionFeeAmount" runat="server" />
    <input type="hidden" value="0" id="hdnIsAllowEntryNonMaster" runat="server" />
    <input type="hidden" value="" id="hdnDefaultEmbalaceID" runat="server" />
    <input type="hidden" value="" id="hdnDefaultParamedicID" runat="server" />
    <input type="hidden" value="" id="hdnDefaultParamedicCode" runat="server" />
    <input type="hidden" value="" id="hdnDefaultParamedicName" runat="server" />
    <input type="hidden" value="0" id="hdnIsEntryMode" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="0" id="hdnIsSignaChanged" runat="server" />
    <input type="hidden" value="0" id="hdnIsUsingUDD" runat="server" />
    <input type="hidden" value="0" id="hdnIsControlChronicDrug" runat="server" />
    <input type="hidden" value="0" id="hdnChronicDrugDuration" runat="server" />
    <input type="hidden" value="0" id="hdnIsBPJSPatient" runat="server" />
    <input type="hidden" value="0" id="hdnIsDischarges" runat="server" />
    <input type="hidden" value="0" id="hdnIsAutoCreateBillAfterProposedOP" runat="server" />
    <input type="hidden" value="0" id="hdnIsAutoCreateBillAfterProposedER" runat="server" />
    <input type="hidden" value="0" id="hdnIsAutoCreateBillAfterProposedIP" runat="server" />
    <input type="hidden" value="0" id="hdnIsAutoCreateBillAfterProposedLB" runat="server" />
    <input type="hidden" value="0" id="hdnIsAutoCreateBillAfterProposedIS" runat="server" />
    <input type="hidden" value="0" id="hdnIsAutoCreateBillAfterProposedMD" runat="server" />
    <input type="hidden" value="0" id="hdnIsAutoCreateBillAfterProposedPH" runat="server" />
    <input type="hidden" value="0" id="hdnIsAutoInsertEmbalace" runat="server" />
    <input type="hidden" value="0" id="hdnPembuatanTagihanTidakAdaOutstandingOrder" runat="server" />
    <input type="hidden" value="0" id="hdnPatientAdminFee" runat="server" />
    <input type="hidden" value="0" id="hdnIsPatientAdminFeeInPercentage" runat="server" />
    <input type="hidden" runat="server" id="hdnSelectedItem" value="" />
    <input type="hidden" id="hdnIsAutoGenerateReferenceNo" value="0" runat="server" />
    <input type="hidden" id="hdnIsGenerateQueueLabel" value="0" runat="server" />
    <input type="hidden" id="hdnItemQtyWithSpecialQueuePrefix" value="0" runat="server" />
    <input type="hidden" id="hdnMenggunakanPembulatan" runat="server" value="" />
    <input type="hidden" id="hdnNilaiPembulatan" runat="server" value="" />
    <input type="hidden" id="hdnPembulatanKemana" runat="server" value="" />
    <input type="hidden" id="hdnIsEndingAmountRoundingTo100" runat="server" value="0" />
    <input type="hidden" id="hdnIsEndingAmountRoundingTo1" runat="server" value="0" />
    <input type="hidden" id="hdnIsReviewPrescriptionMandatoryForProposedTransaction"
        runat="server" value="0" />
    <uc1:PatientBannerCtl ID="ctlPatientBanner" runat="server" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#lblAddNonMaster').hide();
            setRightPanelButtonEnabled();
            setCustomToolbarVisibility();

            $('#<%=btnClinicTransactionTestOrder.ClientID %>').click(function () {
                var prescriptionOrderID = $('#<%=hdnPrescriptionOrderID.ClientID %>').val();
                if (prescriptionOrderID == '') prescriptionOrderID = $('#<%=hdnDefaultPrescriptionOrderID.ClientID %>').val();
                var departmentID = $('#<%=hdnDepartmentID.ClientID %>').val();
                var imagingServiceUnitID = $('#<%=hdnImagingServiceUnitID.ClientID %>').val();
                var laboratoryServiceUnitID = $('#<%=hdnLaboratoryServiceUnitID.ClientID %>').val();
                var isEntryMode = $('#<%=hdnIsEntryMode.ClientID %>').val();
                var param = prescriptionOrderID + '|' + departmentID + '|' + imagingServiceUnitID + '|' + laboratoryServiceUnitID + '|' + isEntryMode;
                var title = "Informasi Detail Order Resep";
                var url = ResolveUrl("~/Program/Prescription/PrescriptionOrder/PrescriptionOrderDtCtl1.ascx");
                if ($('#<%=hdnIsBPJS.ClientID %>').val() == "1" && $('#<%=hdnDepartmentID.ClientID %>').val() == Constant.Facility.OUTPATIENT && $('#<%=hdnIsControlChronicDrug.ClientID %>').val() == "1") {
                    title = "Detail Order Resep : Pasien BPJS";
                    url = ResolveUrl("~/Program/Prescription/PrescriptionOrder/PrescriptionOrderDtCtl2.ascx");
                }
                openUserControlPopup(url, param, title, 900, 700);
            });

            $('#imgVisitNote.imgLink').click(function () {
                var id = $('#<%=hdnRegistrationID.ClientID %>').val();
                var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Registration/ViewNotesCtl.ascx");
                openUserControlPopup(url, id, 'Catatan Kunjungan Pasien', 900, 500);
            });

            $('#<%=btnPrintPrescription.ClientID %>').click(function () {
                var prescriptionOrderID = $('#<%=hdnPrescriptionOrderID.ClientID %>').val();
                var transactionNo = $('#<%=txtTransactionNo.ClientID %>').val();
                var date = $('#<%=txtPrescriptionDate.ClientID %>').val();
                var time = $('#<%=txtPrescriptionTime.ClientID %>').val();
                var prescriptionType = cboPrescriptionType.GetValue();
                if (prescriptionOrderID != "") {
                    var param = prescriptionOrderID + '|' + transactionNo + '|' + date + '|' + time + '|' + prescriptionType;
                    var url = ResolveUrl("~/Program/Prescription/PrescriptionEntry/PrintPrescriptionList.ascx");
                    openUserControlPopup(url, param, 'Cetak Etiket Obat', 800, 600);
                }
                else showToast('Warning', 'Belum ada transaksi resep yang dientry');
            });

            $('#<%=btnPrescriptionCompoundEntry.ClientID %>').click(function (evt) {
                if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                    openCompoundEntry('add');
                }
            });

            if ($('#<%=btnClinicTransactionTestOrder.ClientID %>').is(':visible')) {
                var transactionID = $('#<%=hdnTransactionID.ClientID %>').val();
                if (transactionID == "0")
                    $('#<%=btnClinicTransactionTestOrder.ClientID %>').click();
            }
        });

        function openCompoundEntry(value) {
            var prescriptionOrderID = $('#<%=hdnPrescriptionOrderID.ClientID %>').val();
            var transactionID = $('#<%=hdnTransactionID.ClientID %>').val();
            var prescriptionDetailID = $('#<%=hdnPrescriptionDetailID.ClientID %>').val();
            var date = Methods.dateToString(Methods.getDatePickerDate($('#<%=txtPrescriptionDate.ClientID %>').val()));
            var time = $('#<%=txtPrescriptionTime.ClientID %>').val();
            var physician = $('#<%=hdnPhysicianID.ClientID %>').val();
            var visitID = $('#<%=hdnVisitID.ClientID %>').val();
            var classID = $('#<%=hdnClassID.ClientID %>').val();
            var refillInstruction = 'X138^001';
            var prescriptionFeeAmount = $('#<%=hdnPrescriptionFeeAmount.ClientID %>').val();
            var location = cboLocation.GetValue();
            var physicianID = $('#<%=hdnPhysicianID.ClientID %>').val();
            var dispensaryServiceUnitID = $('#<%=hdnDispensaryServiceUnitID.ClientID %>').val();
            var departmentID = $('#<%=hdnDepartmentID.ClientID %>').val();
            var detailID = $('#<%=hdnTransactionID.ClientID %>').val();
            var prescriptionType = cboPrescriptionType.GetValue();
            var imagingServiceUnitID = $('#<%=hdnImagingServiceUnitID.ClientID %>').val();
            var laboratoryServiceUnitID = $('#<%=hdnLaboratoryServiceUnitID.ClientID %>').val();
            var drugTransactionType = $('#<%=hdnIsDrugChargesJustDistribution.ClientID %>').val();
            if (value == "add") prescriptionDetailID = "";

            var queryString = value + "|" + prescriptionOrderID + '|' + transactionID + '|' + date + '|' + time + '|' + prescriptionDetailID + '|' + refillInstruction +
                              '|' + visitID + '|' + prescriptionFeeAmount + '|' + location + '|' + physicianID + '|' + dispensaryServiceUnitID + '|' + departmentID +
                              '|' + prescriptionType + '|' + classID + '|' + imagingServiceUnitID + '|' + laboratoryServiceUnitID + '|' + drugTransactionType;

            var url = ResolveUrl("~/Program/Prescription/PrescriptionEntry/PrescriptionCompoundEntryCtl2.ascx");
            openUserControlPopup(url, queryString, 'Prescription Compound', 950, 500);
        }

        function onAfterSaveRecordDtSuccess(TransactionID) {
            if ($('#<%=hdnTransactionID.ClientID %>').val() == '0') {
                $('#<%=hdnTransactionID.ClientID %>').val(TransactionID);
                setRightPanelButtonEnabled();
                var filterExpression = 'TransactionID = ' + TransactionID;
                Methods.getObject('GetPatientChargesHdList', filterExpression, function (result) {
                    $('#<%=hdnPrescriptionOrderID.ClientID %>').val(result.PrescriptionOrderID);
                    $('#<%=txtTransactionNo.ClientID %>').val(result.TransactionNo);
                    onLoadObject(result.TransactionNo);
                });
                onAfterCustomSaveSuccess();
                setCustomToolbarVoid();
            }
            else
                cbpView.PerformCallback('refresh');
        }

        function onGetDrugFilterExpression() {
            var LocationID = cboLocation.GetValue();
            var filterExpression = "LocationID = " + LocationID + " AND IsDeleted = 0";
            var drugTransactionType = $('#<%=hdnIsDrugChargesJustDistribution.ClientID %>').val();
            var transactionID = $('#<%=hdnTransactionID.ClientID %>').val();

            if (drugTransactionType == "1") {
                filterExpression += " AND GCItemRequestType = 'X217^01'";
            }

            var isBPJS = $('#<%=hdnIsBPJS.ClientID %>').val();
            var isBPJSFormulariumOnly = $('#<%=hdnIsLimitedCPOEItemForBPJS.ClientID %>').val();
            var isInHealth = $('#<%=hdnIsInHealth.ClientID %>').val();
            var isInHealthFormulariumOnly = $('#<%=hdnIsLimitedCPOEItemForInHealth.ClientID %>').val();

            if (isBPJS == "1" && isBPJSFormulariumOnly == "1") {
                filterExpression += " AND IsBPJSFormularium = 1";
            }

            if (isInHealth == "1" && isInHealthFormulariumOnly == "1") {
                filterExpression += " AND IsInhealthFormularium = 1";
            }

            filterExpression += "AND ISNULL(GCItemStatus,'') != 'X181^999'";

            return filterExpression;
        }

        function ontxtEmbalaceCodeChangedOnAddData(value) {
            var filterExpression = "IsDeleted = 0 AND EmbalaceID = '" + value + "'";
            Methods.getObject('GetEmbalaceHdList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=txtEmbalaceCode.ClientID %>').val(result.EmbalaceCode);
                    $('#<%=txtEmbalaceName.ClientID %>').val(result.EmbalaceName);
                    $('#<%=hdnEmbalaceID.ClientID %>').val(result.EmbalaceID);
                    $('#<%=hdnEmbalaceIsUsingRangePricing.ClientID %>').val(result.IsUsingRangePricing);
                    if (!result.IsUsingRangePricing) {
                        $('#<%=txtEmbalaceQty.ClientID %>').removeAttr('readonly');
                        $('#<%=txtEmbalaceQty.ClientID %>').val($('#<%=txtTakenQty.ClientID %>').val()).trigger('changeValue');
                    } else {
                        $('#<%=txtEmbalaceQty.ClientID %>').attr('readonly', 'readonly');
                        $('#<%=txtEmbalaceQty.ClientID %>').val('0').trigger('changeValue');
                    }
                    getEmbalaceTariff();
                }
                else {
                    $('#<%=txtEmbalaceName.ClientID %>').val('');
                    $('#<%=txtEmbalaceCode.ClientID %>').val('');
                    $('#<%=hdnEmbalaceID.ClientID %>').val('');
                    $('#<%=hdnEmbalaceIsUsingRangePricing.ClientID %>').val('0');
                    $('#<%=txtEmbalaceQty.ClientID %>').val('0').trigger('changeValue');
                }
            });
        }

        function onLoad() {
            if ($('#<%=hdnIsEditable.ClientID %>').val() == "1") {
                $('#lblAddData').show();
                if ($('#<%=hdnIsAllowEntryNonMaster.ClientID %>').val() == "1") {
                    $('#lblAddNonMaster').show();
                }
                else {
                    $('#lblAddNonMaster').hide();
                }
                $('#lblQuickPick').show();
                $('#lblQuickPicksHistory').show();
                $('#lblQuickPicksFromScheduled').show();
                $('#lblTemplate').show();
            }
            else {
                $('#lblAddData').hide();
                $('#lblAddNonMaster').hide();
                $('#lblQuickPick').hide();
                $('#lblQuickPicksHistory').hide();
                $('#lblQuickPicksFromScheduled').hide();
                $('#lblTemplate').hide;
            }

            setDatePicker('<%=txtPrescriptionDate.ClientID %>');
            $('#<%=txtPrescriptionDate.ClientID %>').datepicker('option', 'maxDate', '0');

            $('#<%=txtPhysicianName.ClientID %>').attr('readonly', 'readonly');

            $('#<%=btnPrescriptionBack.ClientID %>').live('click', function () {
                showLoadingPanel();
                document.location = "<%=OnGetUrlReferrer() %>";
            });

            setDatePicker('<%=txtStartDate.ClientID %>');
            setDatePicker('<%=txtExpiredDate.ClientID %>');
            $('#<%=txtStartDate.ClientID %>').datepicker('option', 'minDate', '0');
            $('#<%=txtExpiredDate.ClientID %>').datepicker('option', 'minDate', '0');

            //#region Drug
            $('#<%=lblDrug.ClientID %>.lblLink').live('click', function () {
                $('#<%=hdnGCDosingUnit.ClientID %>').val('');
                $('#<%=hdnGCBaseUnit.ClientID %>').val('');
                $('#<%=hdnGCItemUnit.ClientID %>').val('');
                openSearchDialog('vItemPrescriptionLookup', onGetDrugFilterExpression(), function (value) {
                    $('#<%=txtDrugCode.ClientID %>').val(value);
                    ontxtDrugCodeChanged(value);
                });
            });

            $('#<%=txtDrugCode.ClientID %>').live('change', function () {
                ontxtDrugCodeChanged($(this).val());
            });

            function ontxtDrugCodeChanged(value) {
                var prescriptionFeeAmount = parseFloat($('#<%=hdnPrescriptionFeeAmount.ClientID %>').val());
                var filterExpression = onGetDrugFilterExpression() + " AND ItemCode = '" + value + "'";
                Methods.getObject('GetvDrugInfoPerLocation1List', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnDrugID.ClientID %>').val(result.ItemID);
                        $('#<%=txtDrugCode.ClientID %>').val(result.ItemCode);
                        $('#<%=txtDrugName.ClientID %>').val(result.ItemName1);
                        $('#<%=txtGenericName.ClientID %>').val(result.GenericName);
                        $('#<%=txtStrengthAmount.ClientID %>').val(result.Dose);
                        $('#<%=txtStrengthUnit.ClientID %>').val(result.DoseUnit);
                        $('#<%=hdnGCDoseUnit.ClientID %>').val(result.GCDoseUnit);
                        $('#<%=hdnGCBaseUnit.ClientID %>').val(result.GCItemUnit);
                        $('#<%=hdnGCStockDeductionType.ClientID %>').val(result.GCStockDeductionType);
                        $('#<%=hdnGCConsumptionDeductionType.ClientID %>').val(result.GCConsumptionDeductionType);
                        $('#<%=hdnGCItemUnit.ClientID %>').val('');
                        //$('#<%=txtExpiredDate.ClientID %>').val(Methods.getJSONDateValue(result.ExpiredDate));
                        cboForm.SetValue(result.GCDrugForm);
                        $('#<%=txtFrequencyNumber.ClientID %>').val('1');
                        $('#<%=txtDosingDose.ClientID %>').val('1');
                        $('#<%=txtDosingDuration.ClientID %>').val('1');
                        $('#<%=txtDispenseQty.ClientID %>').val('1');
                        $('#<%=txtTakenQty.ClientID %>').val('1');
                        $('#<%=txtDispenseUnit.ClientID %>').val(result.ItemUnit);
                        $('#<%=txtTakenUnit.ClientID %>').val(result.ItemUnit);
                        cboCoenamRule.SetValue(result.GCCoenamRule);
                        cboMedicationRoute.SetValue(result.GCMedicationRoute);
                        cboDosingUnit.PerformCallback();
                        getDrugTariff();
                        $('#<%=txtEmbalaceAmount.ClientID %>').val(prescriptionFeeAmount).trigger('changeValue');
                        calculate();
                        $('#<%=txtPurposeOfMedication.ClientID %>').val(result.MedicationPurpose);
                        $('#<%=txtFrequencyNumber.ClientID %>').focus();
                        $('#<%=hdnDefaultSignaID.ClientID %>').val(result.SignaID);
                        if ($('#<%=hdnDefaultSignaID.ClientID %>').val() != "" && $('#<%=hdnDefaultSignaID.ClientID %>').val() != "0" && $('#<%=hdnDefaultSignaID.ClientID %>').val() != null) {
                            getSigna();
                        }
                    }
                    else {
                        $('#<%=hdnDrugID.ClientID %>').val('');
                        $('#<%=txtDrugCode.ClientID %>').val('');
                        $('#<%=txtDrugName.ClientID %>').val('');
                        $('#<%=txtGenericName.ClientID %>').val('');
                        $('#<%=txtStrengthAmount.ClientID %>').val('');
                        $('#<%=txtStrengthUnit.ClientID %>').val('');
                        $('#<%=hdnGCDoseUnit.ClientID %>').val('');
                        $('#<%=hdnGCItemUnit.ClientID %>').val('');
                        $('#<%=hdnGCBaseUnit.ClientID %>').val('');
                        $('#<%=txtExpiredDate.ClientID %>').val('');
                        $('#<%=txtDispenseUnit.ClientID %>').val('');
                        $('#<%=txtTakenUnit.ClientID %>').val('');
                        $('#<%=hdnGCStockDeductionType.ClientID %>').val('');
                        $('#<%=hdnGCConsumptionDeductionType.ClientID %>').val('');
                        cboForm.SetValue('');
                        cboCoenamRule.SetValue('');
                        cboMedicationRoute.SetValue('');
                        $('#<%=hdnSignaID.ClientID %>').val();
                        $('#<%=txtSignaLabel.ClientID %>').val();
                        $('#<%=txtSignaName1.ClientID %>').val();
                        $('#<%=txtPurposeOfMedication.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Signa
            $('#lblSigna.lblLink').live('click', function () {
                var filterExpression = "IsDeleted = 0";
                openSearchDialog('signa', filterExpression, function (value) {
                    $('#<%=txtSignaLabel.ClientID %>').val(value);
                    txtSignaLabelChanged(value);
                });
            });

            $('#<%=txtSignaLabel.ClientID %>').live('change', function () {
                txtSignaLabelChanged($(this).val());
            });

            function txtSignaLabelChanged(value) {
                var itemID = $('#<%=hdnDrugID.ClientID %>').val();
                var filterExpression = "IsDeleted = 0 AND SignaLabel = '" + value + "'";
                var dispQty = $('#<%=txtDispenseQty.ClientID %>').val();
                Methods.getObject('GetvSignaList', filterExpression, function (result) {
                    if (result != null) {
                        var drugFormOK = "1";
                        var itemDrugForm = cboForm.GetValue();

                        if (itemID != null && itemID != 0) {
                            var filterDrugInfo = "ItemID = " + itemID;
                            Methods.getObject('GetDrugInfoList', filterDrugInfo, function (resultDrugInfo) {
                                if (resultDrugInfo != null) {
                                    itemDrugForm = resultDrugInfo.GCDrugForm;
                                }
                            });

                            if (itemDrugForm != result.GCDrugForm) {
                                displayConfirmationMessageBox("KONFIRMASI", "Bentuk obat <b>" + result.DrugForm + "</b> dari signa template terpilih ini tidak sesuai dengan bentuk obat di master obat/alkes nya, Lanjutkan?", function (resultDrugForm) {
                                    if (resultDrugForm) {
                                        $('#<%=hdnSignaID.ClientID %>').val(result.SignaID);
                                        $('#<%=txtSignaName1.ClientID %>').val(result.SignaName1);
                                        cboForm.SetValue(result.GCDrugForm);
                                        cboCoenamRule.SetValue(result.GCCoenamRule);
                                        $('#<%=txtFrequencyNumber.ClientID %>').val(result.Frequency);
                                        $('#<%=txtFrequencyNumber.ClientID %>').change();
                                        cboFrequencyTimeline.SetValue(result.GCDosingFrequency);
                                        $('#<%=hdnGCDosingUnit.ClientID %>').val(result.GCDoseUnit);
                                        cboDosingUnit.SetValue(result.GCDoseUnit);
                                        $('#<%=txtDosingDose.ClientID %>').val(result.Dose);
                                        $('#<%=txtDosingDurationTimeline.ClientID %>').val(cboFrequencyTimeline.GetText());
                                    } else {
                                        $('#<%=hdnSignaID.ClientID %>').val('');
                                        $('#<%=txtSignaLabel.ClientID %>').val('');
                                        $('#<%=txtSignaName1.ClientID %>').val('');
                                    }
                                });
                            } else {
                                $('#<%=hdnSignaID.ClientID %>').val(result.SignaID);
                                $('#<%=txtSignaName1.ClientID %>').val(result.SignaName1);
                                cboForm.SetValue(result.GCDrugForm);
                                cboCoenamRule.SetValue(result.GCCoenamRule);
                                $('#<%=txtFrequencyNumber.ClientID %>').val(result.Frequency);
                                $('#<%=txtFrequencyNumber.ClientID %>').change();
                                cboFrequencyTimeline.SetValue(result.GCDosingFrequency);
                                $('#<%=hdnGCDosingUnit.ClientID %>').val(result.GCDoseUnit);
                                cboDosingUnit.SetValue(result.GCDoseUnit);
                                $('#<%=txtDosingDose.ClientID %>').val(result.Dose);
                                $('#<%=txtDosingDurationTimeline.ClientID %>').val(cboFrequencyTimeline.GetText());
                            }
                        }
                    } else {
                        $('#<%=hdnSignaID.ClientID %>').val('');
                        $('#<%=txtSignaLabel.ClientID %>').val('');
                        $('#<%=txtSignaName1.ClientID %>').val('');
                        cboForm.SetValue('');
                        $('#<%=txtFrequencyNumber.ClientID %>').val('');
                        cboFrequencyTimeline.SetValue('');
                        $('#<%=hdnGCDosingUnit.ClientID %>').val('');
                        cboDosingUnit.SetValue('');
                        cboCoenamRule.SetValue('');
                        $('#<%=txtDosingDose.ClientID %>').val('0');
                        $('#<%=txtDispenseUnit.ClientID %>').val('');
                        $('#<%=txtTakenUnit.ClientID %>').val('0');
                        $('#<%=txtDosingDurationTimeline.ClientID %>').val('');
                    }
                });

                if ((dispQty <= 0)) {
                    calculateDispenseQty();
                }
            }

            function getSigna() {
                showLoadingPanel();
                var filterExpression = "IsDeleted = 0 AND SignaID = " + $('#<%=hdnDefaultSignaID.ClientID %>').val() + "";
                var dispQty = $('#<%=txtDispenseQty.ClientID %>').val();
                Methods.getObject('GetvSignaList', filterExpression, function (result) {
                    if (result != null) {
                        var drugFormOK = "1";
                        var itemDrugForm = cboForm.GetValue();
                        var itemID = $('#<%=hdnDrugID.ClientID %>').val();

                        if (itemID != null && itemID != 0) {
                            var filterDrugInfo = "ItemID = " + itemID;
                            Methods.getObject('GetDrugInfoList', filterDrugInfo, function (resultDrugInfo) {
                                if (resultDrugInfo != null) {
                                    itemDrugForm = resultDrugInfo.GCDrugForm;
                                }
                            });

                            if (itemDrugForm != result.GCDrugForm) {
                                displayConfirmationMessageBox("KONFIRMASI", "Bentuk obat <b>" + result.DrugForm + "</b> dari signa template terpilih ini tidak sesuai dengan bentuk obat di master obat/alkes nya, Lanjutkan?", function (resultDrugForm) {
                                    if (resultDrugForm) {
                                        $('#<%=hdnSignaID.ClientID %>').val(result.SignaID);
                                        $('#<%=txtSignaName1.ClientID %>').val(result.SignaName1);
                                        cboForm.SetValue(result.GCDrugForm);
                                        cboCoenamRule.SetValue(result.GCCoenamRule);
                                        $('#<%=txtFrequencyNumber.ClientID %>').val(result.Frequency);
                                        $('#<%=txtFrequencyNumber.ClientID %>').change();
                                        cboFrequencyTimeline.SetValue(result.GCDosingFrequency);
                                        $('#<%=hdnGCDosingUnit.ClientID %>').val(result.GCDoseUnit);
                                        cboDosingUnit.SetValue(result.GCDoseUnit);
                                        $('#<%=txtDosingDose.ClientID %>').val(result.Dose);
                                        $('#<%=txtDosingDurationTimeline.ClientID %>').val(cboFrequencyTimeline.GetText());
                                    } else {
                                        $('#<%=hdnSignaID.ClientID %>').val('');
                                        $('#<%=txtSignaLabel.ClientID %>').val('');
                                        $('#<%=txtSignaName1.ClientID %>').val('');
                                    }
                                });
                            } else {
                                $('#<%=hdnSignaID.ClientID %>').val(result.SignaID);
                                $('#<%=txtSignaName1.ClientID %>').val(result.SignaName1);
                                cboForm.SetValue(result.GCDrugForm);
                                cboCoenamRule.SetValue(result.GCCoenamRule);
                                $('#<%=txtFrequencyNumber.ClientID %>').val(result.Frequency);
                                $('#<%=txtFrequencyNumber.ClientID %>').change();
                                cboFrequencyTimeline.SetValue(result.GCDosingFrequency);
                                $('#<%=hdnGCDosingUnit.ClientID %>').val(result.GCDoseUnit);
                                cboDosingUnit.SetValue(result.GCDoseUnit);
                                $('#<%=txtDosingDose.ClientID %>').val(result.Dose);
                                $('#<%=txtDosingDurationTimeline.ClientID %>').val(cboFrequencyTimeline.GetText());
                            }
                        }
                    } else {
                        $('#<%=hdnSignaID.ClientID %>').val('');
                        $('#<%=txtSignaLabel.ClientID %>').val('');
                        $('#<%=txtSignaName1.ClientID %>').val('');
                        cboForm.SetValue('');
                        $('#<%=txtFrequencyNumber.ClientID %>').val('');
                        cboFrequencyTimeline.SetValue('');
                        $('#<%=hdnGCDosingUnit.ClientID %>').val('');
                        cboDosingUnit.SetValue('');
                        cboCoenamRule.SetValue('');
                        $('#<%=txtDosingDose.ClientID %>').val('0');
                        $('#<%=txtDispenseUnit.ClientID %>').val('');
                        $('#<%=txtTakenUnit.ClientID %>').val('0');
                        $('#<%=txtDosingDurationTimeline.ClientID %>').val('');
                    }
                    hideLoadingPanel();
                });
                if ((dispQty <= 0)) {
                    calculateDispenseQty();
                }
            }
            //#endregion

            //#region Transaction No
            $('#lblPrescriptionNo.lblLink').live('click', function () {
                var filterExpression = "<%:OnGetFilterExpression() %>";
                openSearchDialog('patientchargeshd', filterExpression, function (value) {
                    $('#<%=txtTransactionNo.ClientID %>').val(value);
                    onTxtPrescriptionNoChanged(value);
                });
            });

            $('#<%=txtTransactionNo.ClientID %>').live('change', function () {
                onTxtPrescriptionNoChanged($(this).val());
            });

            function onTxtPrescriptionNoChanged(value) {
                onLoadObject(value);
            }
            //#endregion

            //#region Physician
            $('#<%=lblPhysician.ClientID %>.lblLink').live('click', function () {
                var filterExpression = 'IsDeleted = 0';
                openSearchDialog('paramedic', filterExpression, function (value) {
                    $('#<%=txtPhysicianCode.ClientID %>').val(value);
                    onTxtPhysicianCodeChanged(value);
                });
            });

            $('#<%=txtPhysicianCode.ClientID %>').live('change', function () {
                onTxtPhysicianCodeChanged($(this).val());
            });

            function onTxtPhysicianCodeChanged(value) {
                var filterExpression = "ParamedicCode = '" + value + "' AND IsDeleted = 0";
                Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnPhysicianID.ClientID %>').val(result.ParamedicID);
                        $('#<%=txtPhysicianName.ClientID %>').val(result.ParamedicName);
                    }
                    else {
                        $('#<%=hdnPhysicianID.ClientID %>').val('');
                        $('#<%=txtPhysicianCode.ClientID %>').val('');
                        $('#<%=txtPhysicianName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Embalace
            $('#lblEmbalace.lblLink').live('click', function () {
                openSearchDialog('embalace', "IsDeleted = 0", function (value) {
                    $('#<%=txtEmbalaceCode.ClientID %>').val(value);
                    ontxtEmbalaceCodeChanged(value);
                });
            });

            $('#<%=txtEmbalaceCode.ClientID %>').live('change', function () {
                ontxtEmbalaceCodeChanged($(this).val());
            });

            function ontxtEmbalaceCodeChanged(value) {
                var filterExpression = "IsDeleted = 0 AND EmbalaceCode = '" + value + "'";
                Methods.getObject('GetEmbalaceHdList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=txtEmbalaceName.ClientID %>').val(result.EmbalaceName);
                        $('#<%=hdnEmbalaceID.ClientID %>').val(result.EmbalaceID);
                        $('#<%=hdnEmbalaceIsUsingRangePricing.ClientID %>').val(result.IsUsingRangePricing);
                        if (!result.IsUsingRangePricing) {
                            $('#<%=txtEmbalaceQty.ClientID %>').removeAttr('readonly');
                            $('#<%=txtEmbalaceQty.ClientID %>').val($('#<%=txtTakenQty.ClientID %>').val()).trigger('changeValue');
                        } else {
                            $('#<%=txtEmbalaceQty.ClientID %>').attr('readonly', 'readonly');
                            $('#<%=txtEmbalaceQty.ClientID %>').val('0').trigger('changeValue');
                        }
                        getEmbalaceTariff();
                    }
                    else {
                        $('#<%=txtEmbalaceName.ClientID %>').val('');
                        $('#<%=txtEmbalaceCode.ClientID %>').val('');
                        $('#<%=hdnEmbalaceID.ClientID %>').val('');
                        $('#<%=hdnEmbalaceIsUsingRangePricing.ClientID %>').val('0');
                        $('#<%=txtEmbalaceQty.ClientID %>').val('0').trigger('changeValue');
                    }
                });
            }

            $('#<%=txtDispenseQty.ClientID %>').change(function () {
                var dispenseQty = parseFloat($('#<%=txtDispenseQty.ClientID %>').val());

                if (dispenseQty > 0) {
                    $('#<%=txtTakenQty.ClientID %>').val($(this).val());
                    $('#<%=txtTakenQty.ClientID %>').change();
                }
                else {
                    $('#<%=txtDispenseQty.ClientID %>').val('');
                    $('#<%=txtTakenQty.ClientID %>').val('');
                }
            });

            $('#<%=txtTakenQty.ClientID %>').change(function () {
                var takenQty = parseFloat($('#<%=txtTakenQty.ClientID %>').val());

                if (takenQty != '' && takenQty > 0) {
                    var prescriptionFeeAmount = $('#<%=hdnPrescriptionFeeAmount.ClientID %>').val();
                    $('#<%=txtEmbalaceAmount.ClientID %>').val(prescriptionFeeAmount).trigger('changeValue');
                    if (takenQty % 1 != 0) {
                        if ($('#<%=hdnGCStockDeductionType.ClientID %>').val() == 'X207^001') {
                            takenQty = Math.ceil(takenQty);
                            $('#<%=txtTakenQty.ClientID %>').val(takenQty);
                        }
                    }
                    calculate();
                }
                else {
                    $('#<%=txtTakenQty.ClientID %>').val('0');
                    $('#<%=txtEmbalaceQty.ClientID %>').val('0');
                    $('#<%=txtEmbalaceAmount.ClientID %>').val('0.00');
                    calculate();
                }
            });

            $('#<%=txtEmbalaceQty.ClientID %>').change(function () {
                var embalaceID = $('#<%=hdnEmbalaceID.ClientID %>').val();
                var qty = parseFloat($(this).val());
                if (qty < 0 || $(this).val() == "") {
                    $(this).val(0);
                }
                if (embalaceID != "" && $(this).val() != "") {
                    getEmbalaceTariff();
                }

            });
            //#endregion

            $('#<%=txtTakenUnit.ClientID %>').live('change', function () {
                calculate();
            });

            $('#btnCancel').live('click', function () {
                $('#containerEntry').hide();
            });

            $('#btnSave').live('click', function (evt) {
                if (IsValid(evt, 'fsTrx', 'mpTrx')) {
                    cbpProcess.PerformCallback('save');
                }
            });

            $('#lblAddNonMaster').live('click', function () {
                if (IsValid(null, 'fsTrx', 'mpTrx')) {
                    showLoadingPanel();
                    var url = ResolveUrl('~/Program/Prescription/PrescriptionEntry/PrescriptionNonMasterEntryCtl.ascx');
                    var transactionID = $('#<%=hdnTransactionID.ClientID %>').val();
                    var locationID = $('#<%=hdnLocationID.ClientID %>').val();
                    var defaultGCMedicationRoute = $('#<%=hdnDefaultGCMedicationRoute.ClientID %>').val();
                    var paramedicID = $('#<%=hdnPhysicianID.ClientID %>').val();
                    var registration = $('#<%=hdnRegistrationID.ClientID %>').val();
                    var visitID = $('#<%=hdnVisitID.ClientID %>').val();
                    var chargeClassID = $('#<%=hdnChargeClassID.ClientID %>').val();
                    var id = 'add|' + transactionID + '|' + locationID + '|' + defaultGCMedicationRoute + '|' + paramedicID + '|' + registration + '|' + visitID + '|' + chargeClassID;
                    openUserControlPopup(url, id, 'Non Master', 600, 500);
                }
            });

            $('#lblQuickPick').live('click', function (evt) {
                if (IsValid(null, 'fsTrx', 'mpTrx')) {
                    if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                        showLoadingPanel();
                        var url = ResolveUrl('~/Program/Prescription/PrescriptionEntry/PrescriptionEntryQuickPicksCtl.ascx');
                        var transactionID = $('#<%=hdnTransactionID.ClientID %>').val();
                        var locationID = $('#<%=hdnLocationID.ClientID %>').val();
                        var defaultGCMedicationRoute = $('#<%=hdnDefaultGCMedicationRoute.ClientID %>').val();
                        var paramedicID = $('#<%=hdnPhysicianID.ClientID %>').val();
                        var registration = $('#<%=hdnRegistrationID.ClientID %>').val();
                        var visitID = $('#<%=hdnVisitID.ClientID %>').val();
                        var chargeClassID = $('#<%=hdnChargeClassID.ClientID %>').val();
                        var orderNotes = $('#<%=txtNotes.ClientID %>').val().replace("|", "-");
                        var drugTransactionType = $('#<%=hdnIsDrugChargesJustDistribution.ClientID %>').val();
                        var businessPartnerID = $('#<%=hdnBusinessPartnerID.ClientID %>').val();
                        var id = transactionID + '|' + locationID + '|' + defaultGCMedicationRoute + '|' + paramedicID + '|' + registration + '|' + visitID + '|' + chargeClassID + '|' + orderNotes + '|' + drugTransactionType + '|' + businessPartnerID;
                        openUserControlPopup(url, id, 'Quick Picks', 1200, 600);
                    }
                }
            });

            $('#lblQuickPicksHistory').live('click', function (evt) {
                var isHistoryForNewTransaction = $('#<%=hdnIsQPHistoryForNewTransaction.ClientID %>').val();
                var transactionID = $('#<%=hdnTransactionID.ClientID %>').val();
                if (transactionID != "0" && isHistoryForNewTransaction == "1") {
                    var messageBody = "Maaf, Fasilitas ini hanya untuk pembuatan Nota Transaksi Baru, Silahkan klik Tombol <b> New </b> terlebih dahulu.";
                    displayMessageBox('ERROR : Quick Picks History', messageBody);
                }
                else {
                    if (IsValid(null, 'fsTrx', 'mpTrx')) {
                        if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                            showLoadingPanel();
                            var url = ResolveUrl('~/Program/Prescription/PrescriptionEntry/OrderQuickPicksHistoryCtl1.ascx');
                            var transactionID = $('#<%=hdnTransactionID.ClientID %>').val();
                            var dispensaryServiceUnitID = $('#<%=hdnDispensaryServiceUnitID.ClientID %>').val();
                            var locationID = $('#<%=hdnLocationID.ClientID %>').val();
                            var defaultGCMedicationRoute = $('#<%=hdnDefaultGCMedicationRoute.ClientID %>').val();
                            var paramedicID = $('#<%=hdnPhysicianID.ClientID %>').val();
                            var registration = $('#<%=hdnRegistrationID.ClientID %>').val();
                            var visitID = $('#<%=hdnVisitID.ClientID %>').val();
                            var chargeClassID = $('#<%=hdnChargeClassID.ClientID %>').val();
                            var orderNotes = '';
                            var dispensaryUnitID = '';
                            var refillInstruction = '';

                            var prescriptionType = cboPrescriptionType.GetValue();
                            var date = Methods.dateToString(Methods.getDatePickerDate($('#<%=txtPrescriptionDate.ClientID %>').val()));
                            var time = $('#<%=txtPrescriptionTime.ClientID %>').val();
                            var id = transactionID + '|' + locationID + '|' + defaultGCMedicationRoute + '|' + paramedicID + '|' + registration + '|' + visitID + '|' + chargeClassID + '|' + orderNotes + '|' + dispensaryServiceUnitID + '|' + date + '|' + time + '|' + prescriptionType;
                            openUserControlPopup(url, id, 'Quick Picks - Order History', 1200, 600);
                        }
                    }
                }
            });

            $('#lblQuickPicksFromScheduled').live('click', function (evt) {
                var transactionID = $('#<%=hdnTransactionID.ClientID %>').val();
                if (transactionID != "0") {
                    var messageBody = "Maaf, Fasilitas ini hanya untuk pembuatan Nota Transaksi Baru, Silahkan klik Tombol <b> New </b> terlebih dahulu.";
                    displayMessageBox('ERROR : Quick Picks History', messageBody);
                }
                else {
                    if (IsValid(null, 'fsTrx', 'mpTrx')) {
                        if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                            showLoadingPanel();
                            var url = ResolveUrl('~/Program/Prescription/PrescriptionEntry/OrderQuickPicksFromScheduledCtl.ascx');
                            var transactionID = $('#<%=hdnTransactionID.ClientID %>').val();
                            var dispensaryServiceUnitID = $('#<%=hdnDispensaryServiceUnitID.ClientID %>').val();
                            var locationID = $('#<%=hdnLocationID.ClientID %>').val();
                            var defaultGCMedicationRoute = $('#<%=hdnDefaultGCMedicationRoute.ClientID %>').val();
                            var paramedicID = $('#<%=hdnPhysicianID.ClientID %>').val();
                            var registration = $('#<%=hdnRegistrationID.ClientID %>').val();
                            var visitID = $('#<%=hdnVisitID.ClientID %>').val();
                            var chargeClassID = $('#<%=hdnChargeClassID.ClientID %>').val();
                            var orderNotes = '';
                            var dispensaryUnitID = '';
                            var refillInstruction = '';

                            var prescriptionType = cboPrescriptionType.GetValue();
                            var date = Methods.dateToString(Methods.getDatePickerDate($('#<%=txtPrescriptionDate.ClientID %>').val()));
                            var time = $('#<%=txtPrescriptionTime.ClientID %>').val();
                            var id = transactionID + '|' + locationID + '|' + defaultGCMedicationRoute + '|' + paramedicID + '|' + registration + '|' + visitID + '|' + chargeClassID + '|' + orderNotes + '|' + dispensaryServiceUnitID + '|' + date + '|' + time + '|' + prescriptionType;
                            openUserControlPopup(url, id, 'Quick Picks - Dijadwalkan', 1200, 600);
                        }
                    }
                }
            });

            $('#lblTemplate').live('click', function (evt) {
                if (IsValid(null, 'fsTrx', 'mpTrx')) {
                    if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                        showLoadingPanel();
                        var url = ResolveUrl('~/Program/Prescription/PrescriptionEntry/PrescriptionEntryTemplateCtl.ascx');
                        var transactionID = $('#<%=hdnTransactionID.ClientID %>').val();
                        var locationID = $('#<%=hdnLocationID.ClientID %>').val();
                        var defaultGCMedicationRoute = $('#<%=hdnDefaultGCMedicationRoute.ClientID %>').val();
                        var paramedicID = $('#<%=hdnPhysicianID.ClientID %>').val();
                        var registration = $('#<%=hdnRegistrationID.ClientID %>').val();
                        var visitID = $('#<%=hdnVisitID.ClientID %>').val();
                        var chargeClassID = $('#<%=hdnChargeClassID.ClientID %>').val();
                        var orderNotes = $('#<%=txtNotes.ClientID %>').val();
                        var drugTransactionType = $('#<%=hdnIsDrugChargesJustDistribution.ClientID %>').val();
                        var businessPartnerID = $('#<%=hdnBusinessPartnerID.ClientID %>').val();
                        var id = transactionID + '|' + locationID + '|' + defaultGCMedicationRoute + '|' + paramedicID + '|' + registration + '|' + visitID + '|' + chargeClassID + '|' + orderNotes + '|' + drugTransactionType + '|' + businessPartnerID;
                        openUserControlPopup(url, id, 'Quick Picks', 1200, 600);
                    }
                }
            });
        }

        function setCustomToolbarVisibility() {
            var transactionStatus = $('#<%=hdnTransactionStatus.ClientID %>').val();
            if (transactionStatus != 'X121^001' && transactionStatus != '') {
                $('#<%=btnPrescriptionCompoundEntry.ClientID %>').hide();
            }
            else if (transactionStatus == 'X121^001' || transactionStatus == '') {
                $('#<%=btnPrescriptionCompoundEntry.ClientID %>').show();
            }

            var isVoid = $('#<%:hdnIsAllowVoid.ClientID %>').val();
            if (isVoid == 1) {
                if (getIsAdd()) {
                    $('#<%=btnVoid.ClientID %>').hide();
                }
                else {
                    if (transactionStatus == Constant.TransactionStatus.OPEN) {
                        $('#<%=btnVoid.ClientID %>').show();
                    } else {
                        $('#<%=btnVoid.ClientID %>').hide();
                    }
                }
            } else {
                $('#<%=btnVoid.ClientID %>').hide();
            }
        }

        function setCustomToolbarVoid() {
            var isVoid = $('#<%:hdnIsAllowVoid.ClientID %>').val();
            var transactionNo = $('#<%=txtTransactionNo.ClientID %>').val();
            if (transactionNo != '') {
                var filterExpression = "TransactionNo = '" + transactionNo + "'";
                Methods.getObject('GetPatientChargesHdList', filterExpression, function (result) {
                    if (result != null) {
                        if (result.GCTransactionStatus == Constant.TransactionStatus.OPEN) {
                            $('#<%=btnVoid.ClientID %>').show();
                        }
                        else {
                            $('#<%=btnVoid.ClientID %>').hide();
                        }
                    }
                    else {
                        $('#<%=btnVoid.ClientID %>').hide();
                    }
                });
            }
        }

        $('#<%=btnVoid.ClientID %>').live('click', function (evt) {
            if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                showDeleteConfirmation(function (data) {
                    var param = 'void;' + data.GCDeleteReason + ';' + data.Reason;
                    onCustomButtonClick(param);
                });
            }
        });

        function onAfterCustomClickSuccess(type) {
            onRefreshControl();
        }

        function getEmbalaceTariff() {
            var isUsingRangePricing = $('#<%=hdnEmbalaceIsUsingRangePricing.ClientID %>').val();
            var qty = 0;
            var filterExpression = "EmbalaceID = " + $('#<%=hdnEmbalaceID.ClientID %>').val();
            var embalacePrice = 0;
            var prescriptionFeeAmount = parseFloat($('#<%=hdnPrescriptionFeeAmount.ClientID %>').val());
            if (isUsingRangePricing == "true") {
                if ($('#<%=txtDispenseQty.ClientID %>').val() != null && $('#<%=txtDispenseQty.ClientID %>').val() != "") {
                    qty = parseFloat($('#<%=txtDispenseQty.ClientID %>').val());
                }
                filterExpression += " AND StartingQty <= " + qty + " AND EndingQty >= " + qty;
                Methods.getObject('GetEmbalaceDtList', filterExpression, function (result) {
                    if (result != null) {
                        embalacePrice = (result.Tariff + prescriptionFeeAmount);
                    }
                    else {
                        embalacePrice = (0 + prescriptionFeeAmount);
                    }
                });
            } else {
                qty = parseFloat($('#<%=txtEmbalaceQty.ClientID %>').val());
                Methods.getObject('GetEmbalaceHdList', filterExpression, function (result) {
                    if (result != null) {
                        embalacePrice = qty * (result.Tariff + prescriptionFeeAmount);
                    }
                    else {
                        embalacePrice = qty * (0 + prescriptionFeeAmount);
                    }
                });
            }
            $('#<%=txtEmbalaceAmount.ClientID %>').val(embalacePrice).trigger('changeValue');
            calculate();
        }


        function setRightPanelButtonEnabled() {
            var transactionID = $('#<%:hdnTransactionID.ClientID %>').val();
            if (transactionID != '0') {
                $('#btnChangeSigna').removeAttr('enabled');
                $('#btnChangesLog').removeAttr('enabled');
                $('#btnJournalLog').removeAttr('enabled');
                $('#btnPrescriptionChecklist').removeAttr('enabled');
                $('#btnPrescriptionChargesChecklist').removeAttr('enabled');
            }
            else {
                $('#btnChangeSigna').attr('enabled', 'false');
                $('#btnChangesLog').attr('enabled', 'false');
                $('#btnJournalLog').attr('enabled', 'false');
                $('#btnPrescriptionChecklist').attr('enabled', 'false');
                $('#btnPrescriptionChargesChecklist').attr('enabled', 'false');
            }
        }

        function onBeforeLoadRightPanelContent(code) {
            if (code == 'pharmacyNotes') {
                return $('#<%:hdnRegistrationID.ClientID %>').val() + '|' + $('#<%:hdnDispensaryServiceUnitID.ClientID %>').val();
            }
            else if (code == 'orderChangesLog') {
                var param = $('#<%:hdnRegistrationID.ClientID %>').val() + '|' + $('#<%:hdnPrescriptionOrderID.ClientID %>').val() + '|' + $('#<%:hdnDispensaryServiceUnitID.ClientID %>').val();
                return param;
            }
            else if (code == 'pharmacyJournal') {
                var param = $('#<%:hdnRegistrationID.ClientID %>').val() + '|' + $('#<%:hdnTransactionID.ClientID %>').val();
                return param;
            }
            else if (code == 'ChangeSigna') {
                return $('#<%:hdnTransactionID.ClientID %>').val() + '|' + $('#<%:hdnPrescriptionOrderID.ClientID %>').val();
            }
            else if (code == 'registrationNotes') {
                return $('#<%:hdnRegistrationID.ClientID %>').val();
            }
            else if (code == 'prescriptionChecklist') {
                return $('#<%:hdnTransactionID.ClientID %>').val() + '|' + $('#<%:hdnPrescriptionOrderID.ClientID %>').val() + '|' + $('#<%:hdnIsReviewPrescriptionMandatoryForProposedTransaction.ClientID %>').val();
            }
            else if (code == 'patientChargesChecklist') {
                return $('#<%:hdnTransactionID.ClientID %>').val() + '|' + $('#<%:hdnPrescriptionOrderID.ClientID %>').val() + '|' + $('#<%:hdnIsReviewPrescriptionMandatoryForProposedTransaction.ClientID %>').val();
            }
            else if (code == 'printPrescription') {
                return $('#<%:hdnTransactionID.ClientID %>').val() + '|' + $('#<%:txtTransactionNo.ClientID %>').val();
            }
            else if (code == 'UpdateStatusItter') {
                var prescriptionOrderID = $('#<%=hdnPrescriptionOrderID.ClientID %>').val();
                var transactionNo = $('#<%=txtTransactionNo.ClientID %>').val();
                var date = $('#<%=txtPrescriptionDate.ClientID %>').val();
                var time = $('#<%=txtPrescriptionTime.ClientID %>').val();
                var prescriptionType = cboPrescriptionType.GetValue();

                if (prescriptionOrderID != "") {
                    return prescriptionOrderID + '|' + transactionNo + '|' + date + '|' + time + '|' + prescriptionType;
                }
                else {
                    return false;
                    showToast('Warning', 'Belum ada transaksi resep yang dientry');
                }
            }
            else if (code == 'getNoAntrianMedinlink' || code == 'getNoAntrianNonBridging') {
                var txtTransactionNo = $('#<%:txtTransactionNo.ClientID %>').val();
                if (txtTransactionNo == "") {
                    return false;
                    showToast('Warning', 'Belum ada transaksi resep yang dientry');
                }
                return $('#<%:hdnTransactionID.ClientID %>').val() + '|' + $('#<%:txtTransactionNo.ClientID %>').val();
            }
        }

        //#region Operasi
        $('#lblAddData').live('click', function (evt) {
            $('#<%=hdnIsEntryMode.ClientID %>').val('1');
            if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                var prescriptionFeeAmount = parseFloat($('#<%=hdnPrescriptionFeeAmount.ClientID %>').val());
                $('#<%=lblPhysician.ClientID %>').attr('class', 'lblDisabled');
                $('#<%=txtPhysicianCode.ClientID %>').attr('readonly', 'readonly');
                cboLocation.SetValue($('#<%=hdnDefaultLocationID.ClientID %>').val());
                $('#containerEntry').show();
                $('#<%=hdnEntryID.ClientID %>').val('');
                $('#<%=chkIsRx.ClientID %>').prop('checked', true);
                $('#<%=txtGenericName.ClientID %>').val('');
                cboForm.SetValue('');
                $('#<%=txtPurposeOfMedication.ClientID %>').val('');
                $('#<%=txtStrengthAmount.ClientID %>').val('');
                $('#<%=txtStrengthUnit.ClientID %>').val('');
                cboFrequencyTimeline.SetValue(Constant.DosingFrequency.DAY);
                $('#<%=txtFrequencyNumber.ClientID %>').val('0');
                $('#<%=txtDosingDose.ClientID %>').val('0');
                $('#<%=hdnGCDosingUnit.ClientID %>').val('');
                cboDosingUnit.SetValue('');
                cboMedicationRoute.SetValue('');
                $('#<%=txtMedicationAdministration.ClientID %>').val('');
                $('#<%=txtDosingDuration.ClientID %>').val('0');
                $('#<%=txtDrugName.ClientID %>').val('');
                $('#<%=txtDrugCode.ClientID %>').val('');
                $('#<%=hdnDrugID.ClientID %>').val('');
                $('#<%=hdnGCBaseUnit.ClientID %>').val('');
                $('#<%=hdnGCItemUnit.ClientID %>').val('');
                $('#<%=txtSignaLabel.ClientID %>').val('');
                $('#<%=txtSignaName1.ClientID %>').val('');
                $('#<%=hdnSignaID.ClientID %>').val('');
                $('#<%=txtDispenseQty.ClientID %>').val('');
                $('#<%=txtDispenseUnit.ClientID %>').val('');
                $('#<%=txtTakenQty.ClientID %>').val('');
                $('#<%=txtTakenUnit.ClientID %>').val('');

                $('#<%=hdnEmbalaceID.ClientID %>').val('');
                $('#<%=txtEmbalaceCode.ClientID %>').val('');
                $('#<%=txtEmbalaceName.ClientID %>').val('');

                $('#<%=txtEmbalaceQty.ClientID %>').val('0');
                $('#<%=txtTotalAmount.ClientID %>').val('').trigger('changeValue');
                $('#<%=txtDiscountAmount.ClientID %>').val('').trigger('changeValue');
                $('#<%=txtEmbalaceAmount.ClientID %>').val(prescriptionFeeAmount).trigger('changeValue');
                $('#<%=txtPatientAmount.ClientID %>').val('').trigger('changeValue');
                $('#<%=txtPayerAmount.ClientID %>').val('').trigger('changeValue');
                $('#<%=txtLineAmount.ClientID %>').val('').trigger('changeValue');
                $('#<%=txtTariff.ClientID %>').val('').trigger('changeValue');

                $('#<%=chkIsMorning.ClientID %>').prop('checked', false);
                $('#<%=chkIsNoon.ClientID %>').prop('checked', false);
                $('#<%=chkIsEvening.ClientID %>').prop('checked', false);
                $('#<%=chkIsNight.ClientID %>').prop('checked', false);
                $('#<%=chkIsAsRequired.ClientID %>').prop('checked', false);
                $('#<%=chkIsIMM.ClientID %>').prop('checked', false);

                cboChargeClass.SetValue($('#<%=hdnChargeClassID.ClientID %>').val());

                $('#<%=txtDrugCode.ClientID %>').focus();

                if ($('#<%=hdnIsAutoInsertEmbalace.ClientID %>').val() == '1') {
                    ontxtEmbalaceCodeChangedOnAddData($('#<%=hdnDefaultEmbalaceID.ClientID %>').val());
                }
            }
        });

        $('.imgEdit.imgLink').die('click');
        $('.imgEdit.imgLink').live('click', function () {
            $row = $(this).closest('tr');
            var entity = rowToObject($row);
            if (entity.IsNonMaster == 'True') {
                showLoadingPanel();
                var url = ResolveUrl('~/Program/Prescription/PrescriptionEntry/PrescriptionNonMasterEntryCtl.ascx');
                var locationID = $('#<%=hdnLocationID.ClientID %>').val();
                var id = 'edit|' + entity.PrescriptionOrderDetailID + '|' + locationID;
                openUserControlPopup(url, id, 'Non Master', 600, 500);
            }
            else if (entity.IsCompound != 'True') {
                $('#<%=hdnChargeDtID.ClientID %>').val(entity.ID);
                $('#<%=hdnEntryID.ClientID %>').val(entity.PrescriptionOrderDetailID);
                //$('#<%=hdnTransactionID.ClientID %>').val(entity.ID);
                $('#<%=lvwView.ClientID %> tr.focus').addClass('selected');
                $('#<%=chkIsRx.ClientID %>').prop('checked', (entity.IsRFlag == 'True'));
                $('#<%=txtDrugCode.ClientID %>').val(entity.ItemCode);
                $('#<%=txtDrugName.ClientID %>').val(entity.DrugName);
                $('#<%=txtGenericName.ClientID %>').val(entity.GenericName);
                $('#<%=hdnDrugID.ClientID %>').val(entity.ItemID);
                $('#<%=hdnGCBaseUnit.ClientID %>').val(entity.GCBaseUnit);
                $('#<%=hdnGCItemUnit.ClientID %>').val(entity.GCItemUnit);
                cboLocation.SetValue(entity.LocationID);
                cboForm.SetValue(entity.GCDrugForm);
                $('#<%=txtPurposeOfMedication.ClientID %>').val(entity.MedicationPurpose);
                $('#<%=txtStrengthAmount.ClientID %>').val(entity.Dose);
                $('#<%=txtStrengthUnit.ClientID %>').val(entity.DoseUnit);
                $('#<%=hdnGCDoseUnit.ClientID %>').val(entity.GCDoseUnit);
                cboFrequencyTimeline.SetValue(entity.GCDosingFrequency);
                $('#<%=txtFrequencyNumber.ClientID %>').val(entity.Frequency);
                $('#<%=txtDosingDose.ClientID %>').val(entity.NumberOfDosage);

                cboDosingUnit.PerformCallback("edit");

                $('#<%=hdnGCDosingUnit.ClientID %>').val(entity.GCDosingUnit);
                cboDosingUnit.SetValue(entity.GCDosingUnit);
                cboMedicationRoute.SetValue(entity.GCRoute);
                if (entity.GCCeonamRule == null && entity.GCCeonamRule == "")
                    cboCoenamRule.SetValue("");
                else
                    cboCoenamRule.SetValue(entity.GCCoenamRule);

                $('#<%=txtStartDate.ClientID %>').val(entity.StartDateInDatePickerFormat);
                $('#<%=txtStartTime.ClientID %>').val(entity.StartTime);

                if (entity.IsMorning == "True")
                    $('#<%=chkIsMorning.ClientID %>').prop('checked', true);
                else
                    $('#<%=chkIsMorning.ClientID %>').prop('checked', false);
                if (entity.IsNoon == "True")
                    $('#<%=chkIsNoon.ClientID %>').prop('checked', true);
                else
                    $('#<%=chkIsNoon.ClientID %>').prop('checked', false);
                if (entity.IsEvening == "True")
                    $('#<%=chkIsEvening.ClientID %>').prop('checked', true);
                else {
                    $('#<%=chkIsEvening.ClientID %>').prop('checked', false);
                }
                if (entity.IsNight == "True")
                    $('#<%=chkIsNight.ClientID %>').prop('checked', true);
                else
                    $('#<%=chkIsNight.ClientID %>').prop('checked', false);
                if (entity.IsAsRequired == "True")
                    $('#<%=chkIsAsRequired.ClientID %>').prop('checked', true);
                else
                    $('#<%=chkIsAsRequired.ClientID %>').prop('checked', false);
                if (entity.IsIMM == "True")
                    $('#<%=chkIsIMM.ClientID %>').prop('checked', true);
                else
                    $('#<%=chkIsIMM.ClientID %>').prop('checked', false);

                $('#<%=txtMedicationAdministration.ClientID %>').val(entity.MedicationAdministration);

                $('#<%=txtDosingDuration.ClientID %>').val(entity.DosingDuration);
                $('#<%=txtDispenseQty.ClientID %>').val(entity.DispenseQty);
                $('#<%=txtDispenseUnit.ClientID %>').val(entity.ItemUnit);
                $('#<%=txtTakenQty.ClientID %>').val(entity.TakenQty);
                $('#<%=txtTakenUnit.ClientID %>').val(entity.ItemUnit);
                $('#<%=hdnSignaID.ClientID %>').val(entity.SignaID);
                $('#<%=txtSignaName1.ClientID %>').val(entity.SignaName1);
                $('#<%=txtSignaLabel.ClientID %>').val(entity.SignaLabel);
                cboChargeClass.SetValue(entity.ChargeClassID);
                $('#<%=txtTariff.ClientID %>').val(entity.Tariff).trigger('changeValue');

                $('#<%=txtTotalAmount.ClientID %>').val(entity.Tariff * entity.TakenQty).trigger('changeValue');
                $('#<%=txtDiscountAmount.ClientID %>').val(entity.DiscountAmount).trigger('changeValue');
                $('#<%=txtPatientAmount.ClientID %>').val(entity.PatientAmount).trigger('changeValue');
                $('#<%=txtPayerAmount.ClientID %>').val(entity.PayerAmount).trigger('changeValue');
                $('#<%=txtLineAmount.ClientID %>').val(entity.LineAmount).trigger('changeValue');

                $('#<%=txtEmbalaceName.ClientID %>').val(entity.EmbalaceName);
                $('#<%=txtEmbalaceCode.ClientID %>').val(entity.EmbalaceCode);
                $('#<%=hdnEmbalaceID.ClientID %>').val(entity.EmbalaceID);
                $('#<%=txtEmbalaceQty.ClientID %>').val(entity.EmbalaceQty);

                var filterExpressionSetvar = "ParameterCode = 'PH0001'";
                Methods.getObject('GetSettingParameterDtList', filterExpressionSetvar, function (resultSP) {
                    if (resultSP != null) {
                        $('#<%=hdnPrescriptionFeeAmount.ClientID %>').val(resultSP.ParameterValue);
                    }
                    else {
                        $('#<%=hdnPrescriptionFeeAmount.ClientID %>').val('0');
                    }
                });

                var embalaceR = parseFloat(entity.EmbalaceAmount) + parseFloat(entity.PrescriptionFeeAmount);
                $('#<%=txtEmbalaceAmount.ClientID %>').val(embalaceR).trigger('changeValue');
                getDrugTariff();
                $('#containerEntry').show();
                var filterExpression = "ItemID = '" + entity.ItemID + "'";
                Methods.getObject('GetItemProductList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnGCStockDeductionType.ClientID %>').val(result.GCStockDeductionType);
                        $('#<%=hdnGCConsumptionDeductionType.ClientID %>').val(result.GCConsumptionDeductionType);
                    }
                    else {
                        $('#<%=hdnGCStockDeductionType.ClientID %>').val('');
                        $('#<%=hdnGCConsumptionDeductionType.ClientID %>').val('');
                    }
                });
            } else {
                $('#<%=hdnPrescriptionDetailID.ClientID %>').val(entity.PrescriptionOrderDetailID);
                openCompoundEntry('edit');
            }
        });

        $('.imgDelete.imgLink').die('click');
        $('.imgDelete.imgLink').live('click', function () {
            $row = $(this).closest('tr');
            showDeleteConfirmation(function (data) {
                var obj = rowToObject($row);
                $('#<%=hdnChargeDtID.ClientID %>').val(obj.ID);
                $('#<%=hdnEntryID.ClientID %>').val(obj.PrescriptionOrderDetailID);
                $('#<%=hdnIsEntryMode.ClientID %>').val('0');
                var param = 'delete|' + obj.ID + ';' + data.GCDeleteReason + ';' + data.Reason;
                cbpProcess.PerformCallback(param);
            });
        });

        $('.imgSwitch.imgLink').die('click');
        $('.imgSwitch.imgLink').live('click', function () {
            $row = $(this).closest('tr');
            var entity = rowToObject($row);
            $('#<%=hdnEntryID.ClientID %>').val(entity.PrescriptionOrderDetailID);
            cbpProcess.PerformCallback('switch');
        });
        //#endregion

        function onCbpProcesEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'save') {
                if (param[1] == 'fail') {
                    if (param[2] == 'confirm') {
                        var messageBody = param[3];
                        displayMessageBox('Pelayanan Resep', messageBody);
                        var transactionID = s.cpTransactionID;
                        onAfterSaveRecordDtSuccess(transactionID);
                        $('#containerEntry').hide();
                    }
                    else {
                        var messageBody = param[3];
                        displayErrorMessageBox('Pelayanan Resep', messageBody);
                    }
                }
                else {
                    var transactionID = s.cpTransactionID;
                    onAfterSaveRecordDtSuccess(transactionID);
                    $('#containerEntry').hide();
                }
            }
            else if (param[0] == 'delete') {
                if (param[1] == 'fail') {
                    var messageBody = param[2];
                    displayErrorMessageBox('Pelayanan Resep', messageBody);
                }
                else
                    cbpView.PerformCallback('refresh');
            }
            else if (param[0] == 'switch') {
                if (param[1] == 'fail') {
                    var messageBody = param[2];
                    displayErrorMessageBox('Pelayanan Resep', messageBody);
                }
                else
                    cbpView.PerformCallback('refresh');
            }
        }

        function onAfterSaveRecordPatientPageEntry(param) {
            onAfterSaveRecordDtSuccess(param);
        }

        function onAfterSaveAddRecordEntryPopup(param) {
            onAfterSaveRecordDtSuccess(param);
        }

        function onAfterSaveEditRecord(param) {
            onAfterSaveRecordDtSuccess(param);
        }

        function onAfterSaveEditRecordEntryPopup(param) {
            var GCTransactionStatus = $('#<%=hdnGCTransactionStatus.ClientID%>').val();
            if (GCTransactionStatus == '' || GCTransactionStatus == 'X121^001') {
                cbpView.PerformCallback('refresh');
            }
        }

        //#region calculate Dispense Qty
        $('#<%=txtFrequencyNumber.ClientID %>').live('change', function () {
            calculateDispenseQty();
        });

        $('#<%=txtFrequencyNumber.ClientID %>').live('input', function () {
            $('#<%=txtSignaName1.ClientID %>').val('');
            $('#<%=txtSignaLabel.ClientID %>').val('');
            $('#<%=hdnSignaID.ClientID %>').val('');
        });

        $('#<%=txtDosingDose.ClientID %>').live('change', function () {
            calculateDispenseQty();
        });

        $('#<%=txtDosingDose.ClientID %>').live('input', function () {
            $('#<%=txtSignaName1.ClientID %>').val('');
            $('#<%=txtSignaLabel.ClientID %>').val('');
            $('#<%=hdnSignaID.ClientID %>').val('');
        });

        $('#<%=txtDosingDuration.ClientID %>').live('change', function () {
            var dosingUnit = cboDosingUnit.GetText();
            var itemUnit = $('#<%=txtDispenseUnit.ClientID %>').val();

            calculateDispenseQty();
        });

        function cboFrequencyTimelineChanged() {
            var frequencyTimeLine = cboFrequencyTimeline.GetText();
            $('#<%=txtDosingDurationTimeline.ClientID %>').val(frequencyTimeLine);
            $('#<%=txtSignaName1.ClientID %>').val('');
            $('#<%=txtSignaLabel.ClientID %>').val('');
            $('#<%=hdnSignaID.ClientID %>').val('');
        }

        function cboDosingUnitChanged(paramProcess) {
            if (paramProcess != "edit") {
                calculateDispenseQty();
                $('#<%=txtSignaName1.ClientID %>').val('');
                $('#<%=txtSignaLabel.ClientID %>').val('');
                $('#<%=hdnSignaID.ClientID %>').val('');
            }
        }

        function calculateDispenseQty() {
            var frequency = $('#<%=txtFrequencyNumber.ClientID %>').val();
            var frequencyTimeLine = cboFrequencyTimeline.GetValue();
            var dose = $('#<%=txtDosingDose.ClientID %>').val();
            var dosingDuration = $('#<%=txtDosingDuration.ClientID %>').val();
            var strengthAmount = $('#<%=txtStrengthAmount.ClientID %>').val();
            var strengthUnit = $('#<%=txtStrengthUnit.ClientID %>').val();
            var dosingUnit = cboDosingUnit.GetText();
            var itemUnit = $('#<%=txtDispenseUnit.ClientID %>').val();

            var frequencyInt = parseInt(frequency);
            var doseInt = parseInt(dose);
            var dosingDurationInt = parseInt(dosingDuration);

            if (frequencyInt < 0) {
                $('#<%=txtFrequencyNumber.ClientID %>').val('0');
            }

            if (doseInt < 0) {
                $('#<%=txtDosingDose.ClientID %>').val('0');
            }

            if (dosingDurationInt < 0) {
                $('#<%=txtDosingDuration.ClientID %>').val('0');
            }

            if (frequency != '' && dose != '' && dosingDuration != '' && frequencyInt > 0 && doseInt > 0 && dosingDurationInt > 0) {
                if (itemUnit == dosingUnit) {
                    dispenseQty = Math.ceil(dosingDuration * frequency * dose);
                } else {
                    if (strengthAmount != 0 && strengthUnit == dosingUnit) {
                        dispenseQty = Math.ceil((dosingDuration * frequency * dose) / strengthAmount);
                    } else {
                        dispenseQty = 1;
                    }
                }

                $('#<%=txtDispenseQty.ClientID %>').val(dispenseQty);
                $('#<%=txtDispenseQty.ClientID %>').change();
            } else {
                dispenseQty = 1;
                $('#<%=txtDispenseQty.ClientID %>').val(dispenseQty);
                $('#<%=txtDispenseQty.ClientID %>').change();
            }
        }
        //#endregion


        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var isPrintMustProposedCharges = $('#<%=hdnIsRightPanelPrintMustProposedCharges.ClientID %>').val();
            var prescriptionOrderID = $('#<%=hdnPrescriptionOrderID.ClientID %>').val();
            var GCTransactionStatus = $('#<%=hdnGCTransactionStatus.ClientID%>').val();
            var transactionID = $('#<%=hdnTransactionID.ClientID %>').val();
            var RegistrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
            if (code == 'PM-00105') {
                filterExpression.text = RegistrationID;
                return true;
            }
            if (prescriptionOrderID == '' || prescriptionOrderID == '0') {
                errMessage.text = 'Transaksi Resep harap diselesaikan terlebih dahulu.';
                return false;
            }
            else {
                if (code == 'PH-00018' || code == 'PH-00029' || code == 'PM-00214' || code == 'PH-00007' || code == 'PH-00050' || code == 'PM-00589' || code == 'PH-00078') {
                    if (isPrintMustProposedCharges == "1" && GCTransactionStatus == Constant.TransactionStatus.OPEN) {
                        isAllowPrint = "0";
                    } else {
                        isAllowPrint = "1";
                    }
                    if (isAllowPrint == "1") {
                        filterExpression.text = transactionID;
                    } else {
                        errMessage.text = 'Transaksi harus dipropose terlebih dahulu sebelum proses bisa dilakukan.';
                        return false;
                    }
                    return true;
                }
                else if (code == 'PH-00038' || code == 'PH-00041' || code == 'PH-00044') {
                    filterExpression.text = prescriptionOrderID;
                    return true;
                }
                else if (code == 'PH-00038' || code == 'PH-00041' || code == 'PH-00061') {
                    if (isPrintMustProposedCharges == "1" && GCTransactionStatus == Constant.TransactionStatus.OPEN) {
                        isAllowPrint = "0";
                    } else {
                        isAllowPrint = "1";
                    }
                    if (isAllowPrint == "1") {
                        filterExpression.text = prescriptionOrderID;
                    } else {
                        errMessage.text = 'Transaksi harus dipropose terlebih dahulu sebelum proses bisa dilakukan.';
                        return false;
                    }
                    return true;
                }
                else if (code == 'PH-00056' || code == 'PH-00060' || code == 'PH-00070' || code == 'PH-00055' || code == 'PH-00044' || code == 'PH-00045'
                        || code == 'PH-00028' || code == 'PH-00083') {
                    filterExpression.text = prescriptionOrderID;
                    return true;
                }
                else if (code == 'PH-00028' || code == 'PH-00038' || code == 'PH-00041' || code == 'PH-00060'
                        || code == 'PH-00061' || code == 'PH-00055' || code == 'PH-00070' || code == 'PH-00083') {
                    if (isPrintMustProposedCharges == "1" && GCTransactionStatus == Constant.TransactionStatus.OPEN) {
                        isAllowPrint = "0";
                    } else {
                        isAllowPrint = "1";
                    }
                    if (isAllowPrint == "1") {
                        filterExpression.text = prescriptionOrderID;
                    } else {
                        errMessage.text = 'Transaksi harus dipropose terlebih dahulu sebelum proses bisa dilakukan.';
                        return false;
                    }
                    return true;
                }
                else {
                    var isAllowPrint = "1";

                    if (GCTransactionStatus != Constant.TransactionStatus.OPEN && GCTransactionStatus != Constant.TransactionStatus.VOID) {
                        isAllowPrint = "1";
                    } else {
                        if (isPrintMustProposedCharges == "1") {
                            isAllowPrint = "0";
                        } else {
                            isAllowPrint = "1";
                        }
                    }

                    if (isAllowPrint == "1") {
                        if (code == 'PH-00003' || code == 'PH-00023' || code == 'PH-00048'
                            || code == 'PH-00067' || code == 'PH-00068' || code == 'PH-00074' || code == 'PH-00075') {
                            filterExpression.text = prescriptionOrderID;
                            return true;
                        }
                        else if (code == 'PH-00002' || code == 'PH-00005' || code == 'PH-00006' || code == 'PH-00010'
                                    || code == 'PH-00015' || code == 'PH-00017' || code == 'PH-00019' || code == 'PH-00087'
                                    || code == 'PH-00021' || code == 'PH-00022' || code == 'PH-00024'
                                    || code == 'PH-00011' || code == 'PH-00012' || code == 'PH-00014'
                                    || code == 'PH-00031' || code == 'PM-00201' || code == 'PM-00236'
                                    || code == 'PM-002361' || code == 'PM-002362' || code == 'PM-00239'
                                    || code == 'PM-00288' || code == 'PM-00413' || code == 'PH-00033'
                                    || code == 'PH-00034' || code == 'PM-90027' || code == 'PH-00036'
                                    || code == 'PH-00040' || code == 'PH-00043' || code == 'PH-00035'
                                    || code == 'PH-00047' || code == 'PH-00046' || code == 'PH-00051'
                                    || code == 'PH-00052' || code == 'PH-00054' || code == 'PM-00358'
                                    || code == 'PM-00361' || code == 'PH-00058' || code == 'PH-00059'
                                    || code == 'PH-00062' || code == 'PH-00063' || code == 'PH-00064'
                                    || code == 'PH-00065' || code == 'PH-00066' || code == 'PH-00076'
                                    || code == 'PM-00685' || code == 'PH-00072' || code == 'PH-00071' || code == 'PH-00092'
                                    || code == 'PH-00080' || code == 'PH-00081' || code == 'PH-00085' || code == 'PM-002363'
                                    || code == 'PH-00091' || code == 'PH-00086' || code == 'PH-00090' || code == 'PH-00089'
                                    || code == 'PH-00108'
                                    ) {
                            filterExpression.text = transactionID;
                            return true;
                        }
                        else {
                            filterExpression.text = "PrescriptionOrderID = " + prescriptionOrderID;
                            return true;
                        }
                    }
                    else if (code == 'PH-00035' || code == 'PH-00091') {
                        filterExpression.text = transactionID;
                        return true;
                    }
                    else {
                        errMessage.text = 'Transaksi harus dipropose terlebih dahulu sebelum proses bisa dilakukan.';
                        return false;
                    }
                }
            }
        }

        function onCboDosingUnitEndCallback(s) {
            var param = s.cpResult.split('|');
            var filter = "ParentID = 'X003' AND (TagProperty LIKE '%1%' OR TagProperty LIKE '%PRE%')";
            if ($('#<%=hdnGCDosingUnit.ClientID %>').val() == '') {
                if ($('#<%=hdnGCItemUnit.ClientID %>').val() == '') {
                    filter += " AND StandardCodeID = '" + $('#<%=hdnGCBaseUnit.ClientID %>').val() + "'";
                    Methods.getObject('GetStandardCodeList', filter, function (result) {
                        if (result != null) {
                            cboDosingUnit.SetValue($('#<%=hdnGCBaseUnit.ClientID %>').val());
                        }
                    });
                }
                else {
                    filter += " AND StandardCodeID = '" + $('#<%=hdnGCItemUnit.ClientID %>').val() + "'";
                    Methods.getObject('GetStandardCodeList', filter, function (result) {
                        if (result != null) {
                            cboDosingUnit.SetValue($('#<%=hdnGCItemUnit.ClientID %>').val());
                        }
                    });
                }
            }
            else {
                filter += " AND StandardCodeID = '" + $('#<%=hdnGCDosingUnit.ClientID %>').val() + "'";
                Methods.getObject('GetStandardCodeList', filter, function (result) {
                    if (result != null) {
                        cboDosingUnit.SetValue($('#<%=hdnGCDosingUnit.ClientID %>').val());
                    }
                });
            }

            cboDosingUnitChanged(param[0]);
        }

        //#region Drug Tariff
        function getTrxDate() {
            var date = Methods.getDatePickerDate($('#<%=txtPrescriptionDate.ClientID %>').val());
            var dateInYMD = Methods.dateToYMD(date);
            return dateInYMD;
        }

        function getDrugTariff() {
            showLoadingPanel();
            var itemID = $('#<%=hdnDrugID.ClientID %>').val();
            var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
            var visitID = $('#<%=hdnVisitID.ClientID %>').val();
            var classID = cboChargeClass.GetValue();
            var trxDate = getTrxDate();
            Methods.getItemTariff(registrationID, visitID, classID, itemID, trxDate, function (result) {
                if (result != null) {
                    $('#<%=txtTariff.ClientID %>').val(result.Price).trigger('changeValue');
                    $('#<%=hdnDiscountAmount.ClientID %>').val(result.DiscountAmount);
                    $('#<%=hdnCoverageAmount.ClientID %>').val(result.CoverageAmount);
                    $('#<%=hdnDiscountInPercentage.ClientID %>').val(result.IsDiscountInPercentage ? '1' : '0');
                    $('#<%=hdnCoverageInPercentage.ClientID %>').val(result.IsCoverageInPercentage ? '1' : '0');
                    $('#<%=hdnBaseTariff.ClientID %>').val(result.BasePrice);
                    $('#<%=hdnBaseComp1.ClientID %>').val(result.BasePrice);
                    $('#<%=hdnBaseComp2.ClientID %>').val(0);
                    $('#<%=hdnBaseComp3.ClientID %>').val(0);

                    $('#<%=hdnTariff.ClientID %>').val(result.Price);
                    $('#<%=hdnTariffComp1.ClientID %>').val(result.Price);
                    $('#<%=hdnTariffComp2.ClientID %>').val(0);
                    $('#<%=hdnTariffComp3.ClientID %>').val(0);
                }
                else {
                    $('#<%=txtTariff.ClientID %>').val('');
                    $('#<%=hdnDiscountAmount.ClientID %>').val('');
                    $('#<%=hdnCoverageAmount.ClientID %>').val('');
                    $('#<%=hdnDiscountInPercentage.ClientID %>').val('');
                    $('#<%=hdnCoverageInPercentage.ClientID %>').val('');
                    $('#<%=hdnBaseTariff.ClientID %>').val('');
                    $('#<%=hdnBaseComp1.ClientID %>').val('');
                    $('#<%=hdnBaseComp2.ClientID %>').val('');
                    $('#<%=hdnBaseComp3.ClientID %>').val('');

                    $('#<%=hdnTariff.ClientID %>').val('');
                    $('#<%=hdnTariffComp1.ClientID %>').val('');
                    $('#<%=hdnTariffComp2.ClientID %>').val('');
                    $('#<%=hdnTariffComp3.ClientID %>').val('');
                }
                hideLoadingPanel();
            });

        }

        function cboChargeClassChanged() {
            if ($('#<%=hdnDrugID.ClientID %>').val() != '') {
                getDrugTariff();
                calculate();
            }
        }

        //#endregion

        //#region Calculate Total
        function calculate() {
            calculateTotal();
            calculateDiscount();
            calculateAllTotal();
        }
        function calculateTotal() {
            var takenQty = parseFloat($('#<%=txtTakenQty.ClientID %>').val());
            var price = parseFloat($('#<%=txtTariff.ClientID %>').attr('hiddenVal'));

            var total = takenQty * price;
            $('#<%=txtTotalAmount.ClientID %>').val(total).trigger('changeValue');
        }

        function calculateDiscount() {
            var discountAmount = parseFloat($('#<%=hdnDiscountAmount.ClientID %>').val());
            var isDicountInPercentage = ($('#<%=hdnDiscountInPercentage.ClientID %>').val() == '1');

            var discountTotal = 0;
            if (discountAmount > 0) {
                var tariff = parseFloat($('#<%=txtTotalAmount.ClientID %>').attr('hiddenVal'));
                if (isDicountInPercentage)
                    discountTotal = (tariff * discountAmount) / 100;
                else {
                    var qty = parseFloat($('#<%=txtTakenQty.ClientID %>').val());
                    discountTotal = discountAmount * qty;
                }
                if (discountTotal > tariff)
                    discountTotal = tariff;
            }
            $('#<%=txtDiscountAmount.ClientID %>').val(discountTotal).trigger('changeValue');
        }

        function calculateAllTotal() {
            var tariff = parseFloat($('#<%=txtTotalAmount.ClientID %>').attr('hiddenVal').replace('.00', '').split(',').join(''));
            var discount = parseFloat($('#<%=txtDiscountAmount.ClientID %>').val().replace('.00', '').split(',').join(''));
            //var prescriptionFeeAmount = parseFloat($('#<%=hdnPrescriptionFeeAmount.ClientID %>').val());
            var embalace = parseFloat($('#<%=txtEmbalaceAmount.ClientID %>').attr('hiddenVal').replace('.00', '').split(',').join(''));
            var takenqty = parseFloat($('#<%=txtTakenQty.ClientID %>').val());

            if (takenqty <= 0) {
                embalace = parseFloat(0);
            }

            var total = (tariff + embalace) - discount;
            var coverageAmount = parseFloat($('#<%=hdnCoverageAmount.ClientID %>').val());
            var isCoverageInPercentage = ($('#<%=hdnCoverageInPercentage.ClientID %>').val() == '1');
            var totalPayer = 0;

            if (isCoverageInPercentage) {
                totalPayer = (total * coverageAmount) / 100;
            }
            else {
                var qty = parseFloat($('#<%=txtTakenQty.ClientID %>').val());
                totalPayer = coverageAmount * qty;

                if (totalPayer != 0) {
                    totalPayer = totalPayer + embalace;
                }
            }

            if (total > 0 && totalPayer > total) {
                totalPayer = total;
            }

            var totalPatient = total - totalPayer;

            $('#<%=txtPatientAmount.ClientID %>').val(totalPatient).trigger('changeValue');
            $('#<%=txtPayerAmount.ClientID %>').val(totalPayer).trigger('changeValue');
            $('#<%=txtLineAmount.ClientID %>').val(total).trigger('changeValue');
        }
        //#endregion

        $('#<%=txtDiscountAmount.ClientID %>').live('change', function () {
            if ($(this).val() == "") {
                $('#<%=txtDiscountAmount.ClientID %>').val("0");
            }
            $('#<%=hdnDiscountAmount.ClientID %>').val($(this).val());
            calculateAllTotal();
        });

        $('#<%=txtPayerAmount.ClientID %>').live('change', function () {
            if ($(this).val() == "") {
                $('#<%=txtPayerAmount.ClientID %>').val("0");
            }
            $(this).trigger('changeValue');
            var totalPatient = parseFloat($('#<%=txtPatientAmount.ClientID %>').attr('hiddenVal'));
            var totalPayer = parseFloat($('#<%=txtPayerAmount.ClientID %>').attr('hiddenVal'));
            var lineAmount = parseFloat($('#<%=txtLineAmount.ClientID %>').attr('hiddenVal'));

            if (totalPayer > lineAmount) {
                totalPayer = lineAmount;
                $(this).val(totalPayer).trigger('changeValue');

            }
            totalPatient = lineAmount - totalPayer;

            $('#<%=txtPatientAmount.ClientID %>').val(totalPatient).trigger('changeValue');

            var bpID = $('#<%=hdnBusinessPartnerID.ClientID %>').val();
            var patientAmount = parseFloat($('#<%=txtPatientAmount.ClientID %>').attr('hiddenVal'));
            if (bpID != 1) {
                if (patientAmount > 0) {
                    showToast('Informasi', 'Ada yang ditanggung PASIEN');
                }
            }
        });

        $('#<%=txtPatientAmount.ClientID %>').live('change', function () {
            if ($(this).val() == "") {
                $('#<%=txtPatientAmount.ClientID %>').val("0");
            }
            $(this).trigger('changeValue');
            var totalPatient = parseFloat($('#<%=txtPatientAmount.ClientID %>').attr('hiddenVal'));
            var totalPayer = parseFloat($('#<%=txtPayerAmount.ClientID %>').attr('hiddenVal'));
            var lineAmount = parseFloat($('#<%=txtLineAmount.ClientID %>').attr('hiddenVal'));
            if (totalPatient > lineAmount) {
                totalPatient = lineAmount;
                $(this).val(totalPatient).trigger('changeValue');
            }
            totalPayer = lineAmount - totalPatient;

            $('#<%=txtPayerAmount.ClientID %>').val(totalPayer).trigger('changeValue');

            var bpID = $('#<%=hdnBusinessPartnerID.ClientID %>').val();
            var patientAmount = parseFloat($('#<%=txtPatientAmount.ClientID %>').attr('hiddenVal'));
            if (bpID != 1) {
                if (patientAmount > 0) {
                    showToast('Informasi', 'Ada yang ditanggung PASIEN');
                }
            }
        });

        function onCboPrescriptionType() {
        }

        function onCboBPJSTransType() {
        }

        $('#imgDrugAlertMain').live('click', function () {
            var url = ResolveUrl("~/Libs/Program/Information/DrugAlertInformationCtl.ascx");
            var id = $('#<%:hdnPrescriptionOrderID.ClientID %>').val() + "|0";
            openUserControlPopup(url, id, 'Drug Alert Information', 700, 600);
        });
    </script>
    <input type="hidden" value="" id="hdnIsShowSwitchIcon" runat="server" />
    <input type="hidden" value="1" id="hdnIsEditable" runat="server" />
    <input type="hidden" value="" id="hdnLocationID" runat="server" />
    <input type="hidden" value="" id="hdnSignaID" runat="server" />
    <input type="hidden" value="" id="hdnDefaultSignaID" runat="server" />
    <input type="hidden" value="" id="hdnDefaultGCMedicationRoute" runat="server" />
    <input type="hidden" value="" id="hdnTransactionID" runat="server" />
    <input type="hidden" value="" id="hdnPrescriptionDetailID" runat="server" />
    <input type="hidden" value="" id="hdnPrescriptionOrderID" runat="server" />
    <input type="hidden" value="" id="hdnDispensaryServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnGCDoseUnit" runat="server" />
    <input type="hidden" value="" id="hdnGCTransactionStatus" runat="server" />
    <input type="hidden" value="" id="hdnGCBaseUnit" runat="server" />
    <input type="hidden" value="" id="hdnGCItemUnit" runat="server" />
    <input type="hidden" value="" id="hdnGCStockDeductionType" runat="server" />
    <input type="hidden" value="" id="hdnGCConsumptionDeductionType" runat="server" />
    <input type="hidden" value="" id="hdnGCDosingUnit" runat="server" />
    <input type="hidden" value="" id="hdnCoverageAmount" runat="server" />
    <input type="hidden" value="" id="hdnDiscountAmount" runat="server" />
    <input type="hidden" value="" id="hdnDiscountInPercentage" runat="server" />
    <input type="hidden" value="" id="hdnCoverageInPercentage" runat="server" />
    <input type="hidden" value="" id="hdnTransactionStatus" runat="server" />
    <input type="hidden" value="" id="hdnBaseTariff" runat="server" />
    <input type="hidden" value="" id="hdnBaseComp1" runat="server" />
    <input type="hidden" value="" id="hdnBaseComp2" runat="server" />
    <input type="hidden" value="" id="hdnBaseComp3" runat="server" />
    <input type="hidden" value="" id="hdnTariff" runat="server" />
    <input type="hidden" value="" id="hdnTariffComp1" runat="server" />
    <input type="hidden" value="" id="hdnTariffComp2" runat="server" />
    <input type="hidden" value="" id="hdnTariffComp3" runat="server" />
    <input type="hidden" value="" id="hdnEmbalaceID" runat="server" />
    <input type="hidden" value="0" id="hdnEmbalaceIsUsingRangePricing" runat="server" />
    <input type="hidden" value="" id="hdnIsAllowVoid" runat="server" />
    <input type="hidden" value="" id="hdnBusinessPartnerID" runat="server" />
    <input type="hidden" value="0" id="hdnIsDrugChargesJustDistribution" runat="server" />
    <input type="hidden" value="0" id="hdnIsLimitedCPOEItemForBPJS" runat="server" />
    <input type="hidden" value="0" id="hdnIsLimitedCPOEItemForInHealth" runat="server" />
    <input type="hidden" value="0" id="hdnIsBPJS" runat="server" />
    <input type="hidden" value="0" id="hdnIsInHealth" runat="server" />
    <input type="hidden" value="" id="hdnDefaultJenisTransaksiBPJS" runat="server" />
    <input type="hidden" value="" id="hdnIsQPHistoryForNewTransaction" runat="server" />
    <input type="hidden" value="1" id="hdnIsRightPanelPrintMustProposedCharges" runat="server" />
    <input type="hidden" value="1" id="hdnIsUsingDrugAlertMain" runat="server" />
    <input type="hidden" id="hdnIsBridgingToMobileJKN" value="0" runat="server" />
    <div style="overflow-y: auto; overflow-x: hidden;">
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
                                <label class="lblLink" id="lblPrescriptionNo">
                                    <%=GetLabel("No. Resep")%></label>
                            </td>
                            <td>
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="padding-right: 1px; width: 145px">
                                            <asp:TextBox ID="txtTransactionNo" Width="150px" ReadOnly="true" runat="server" />
                                        </td>
                                        <td style="width: 5px">
                                            &nbsp;
                                        </td>
                                        <td>
                                            <div id="tdImageDrugAlertInfo" runat="server" style="display: none">
                                                <img class="imgDrugAlertMain imgLink blink-alert" id="imgDrugAlertMain" height="25px"
                                                    src='<%= ResolveUrl("~/Libs/Images/Status/drug_alert.png")%>' alt='' title='Drug Alert Information' />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal lblMandatory" />
                                <%=GetLabel("Tanggal ") %>
                                -
                                <%=GetLabel("Jam ") %>
                            </td>
                            <td>
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="padding-right: 1px; width: 145px">
                                            <asp:TextBox ID="txtPrescriptionDate" Width="120px" CssClass="datepicker" runat="server" />
                                        </td>
                                        <td style="width: 5px">
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPrescriptionTime" Width="80px" CssClass="time" runat="server"
                                                Style="text-align: center" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink lblMandatory" runat="server" id="lblPhysician">
                                    <%=GetLabel("Dokter ")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnPhysicianID" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 120px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtPhysicianCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPhysicianName" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Jenis Resep")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboPrescriptionType" ClientInstanceName="cboPrescriptionType"
                                    Width="233px" runat="server">
                                    <ClientSideEvents ValueChanged="function(s,e){ onCboPrescriptionType(); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" id="lblReferenceNo">
                                    <%=GetLabel("Nomor Referensi")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtReferenceNo" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Jenis Transaksi BPJS")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboBPJSTransType" ClientInstanceName="cboBPJSTransType" Width="233px"
                                    runat="server">
                                    <ClientSideEvents ValueChanged="function(s,e){ onCboBPJSTransType(); }" />
                                </dxe:ASPxComboBox>
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
                                <label class="lblNormal" id="lblTestOrderInfo">
                                    <%=GetLabel("Informasi Order")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPrescriptionOrderInfo" Width="100%" ReadOnly="true" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                <label>
                                    <%=GetLabel("Catatan Order")%></label>
                            </td>
                            <td colspan="5">
                                <asp:TextBox ID="txtNotes" ReadOnly="true" Width="100%" TextMode="MultiLine" Height="110px"
                                    runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div id="containerEntry" style="margin-top: 4px; display: none;">
                        <div class="pageTitle">
                            <%=GetLabel("Rx")%></div>
                        <fieldset id="fsTrx" style="margin: 0">
                            <input type="hidden" value="" id="hdnEntryID" runat="server" />
                            <table style="width: 100%" class="tblEntryDetail">
                                <colgroup>
                                    <col style="width: 50%" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td valign="top">
                                        <table style="width: 100%">
                                            <colgroup>
                                                <col style="width: 160px" />
                                                <col width="40px" />
                                                <col width="60px" />
                                                <col width="40px" />
                                                <col width="65px" />
                                                <col width="65px" />
                                                <col width="90px" />
                                            </colgroup>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblLink lblMandatory" id="lblDrug" runat="server">
                                                        <%=GetLabel("Obat")%></label>
                                                </td>
                                                <td colspan="2">
                                                    <input type="hidden" value="" id="hdnDrugID" runat="server" />
                                                    <asp:TextBox ID="txtDrugCode" CssClass="required" Width="100%" runat="server" />
                                                </td>
                                                <td colspan="4">
                                                    <asp:TextBox ID="txtDrugName" ReadOnly="true" CssClass="required" Width="100%" runat="server"
                                                        TabIndex="999" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Nama Generik")%></label>
                                                </td>
                                                <td colspan="6">
                                                    <asp:TextBox runat="server" ReadOnly="true" ID="txtGenericName" Width="100%" TabIndex="999" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Kadar")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtStrengthAmount" runat="server" Width="100%" CssClass="number"
                                                        ReadOnly="true" TabIndex="999" />
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtStrengthUnit" Width="100%" ReadOnly="true" TabIndex="999" />
                                                </td>
                                                <td class="tdLabel" colspan="2" style="padding-left: 10px">
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("Bentuk")%></label>
                                                </td>
                                                <td colspan="2">
                                                    <dxe:ASPxComboBox runat="server" ID="cboForm" ClientInstanceName="cboForm" Width="100%"
                                                        TabIndex="999" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblNormal lblLink" id="lblSigna">
                                                        <%=GetLabel("Signa Template")%></label>
                                                </td>
                                                <td colspan="2">
                                                    <asp:TextBox runat="server" ID="txtSignaLabel" Width="100%" />
                                                </td>
                                                <td colspan="4">
                                                    <asp:TextBox runat="server" ID="txtSignaName1" Width="100%" ReadOnly="true" TabIndex="999" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("Frekuensi dan Dosis")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtFrequencyNumber" runat="server" Width="100%" CssClass="number" />
                                                </td>
                                                <td>
                                                    <dxe:ASPxComboBox ID="cboFrequencyTimeline" ClientInstanceName="cboFrequencyTimeline"
                                                        runat="server" Width="100%">
                                                        <ClientSideEvents SelectedIndexChanged="function(s,e){cboFrequencyTimelineChanged()}" />
                                                    </dxe:ASPxComboBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDosingDose" runat="server" Width="100%" CssClass="number" />
                                                </td>
                                                <td>
                                                    <dxe:ASPxComboBox ID="cboDosingUnit" ClientInstanceName="cboDosingUnit" runat="server"
                                                        Width="100%" OnCallback="cboDosingUnit_Callback">
                                                        <ClientSideEvents EndCallback="function(s,e){ onCboDosingUnitEndCallback(s); }" ValueChanged="function(s,e){ cboDosingUnitChanged(); }" />
                                                    </dxe:ASPxComboBox>
                                                </td>
                                                <td class="tdLabel">
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("Rute Obat")%></label>
                                                </td>
                                                <td colspan="6">
                                                    <dxe:ASPxComboBox runat="server" ID="cboMedicationRoute" ClientInstanceName="cboMedicationRoute"
                                                        Width="100%" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("Mulai diberikan")%></label>
                                                </td>
                                                <td colspan="2">
                                                    <asp:TextBox runat="server" ID="txtStartDate" CssClass="datepicker" Width="110px" />
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtStartTime" CssClass="time" Width="100%" />
                                                </td>
                                                <td class="tdLabel" style="padding-left: 10px">
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("Durasi")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtDosingDuration" Width="100%" CssClass="number" />
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtDosingDurationTimeline" Width="100%" ReadOnly="true"
                                                        runat="server" TabIndex="999" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Waktu Pemberian Obat")%></label>
                                                </td>
                                                <td colspan="6">
                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                        <tr>
                                                            <td style="width: 18%">
                                                                <asp:CheckBox ID="chkIsMorning" runat="server" Text="Pagi" Checked="false" />
                                                            </td>
                                                            <td style="width: 18%">
                                                                <asp:CheckBox ID="chkIsNoon" runat="server" Text="Siang" Checked="false" />
                                                            </td>
                                                            <td style="width: 18%">
                                                                <asp:CheckBox ID="chkIsEvening" runat="server" Text="Sore" Checked="false" />
                                                            </td>
                                                            <td style="width: 18%">
                                                                <asp:CheckBox ID="chkIsNight" runat="server" Text="Malam" Checked="false" />
                                                            </td>
                                                            <td style="width: 18%">
                                                                <asp:CheckBox ID="chkIsAsRequired" runat="server" Text="PRN" Checked="false" />
                                                            </td>
                                                            <td style="width: 18%">
                                                                <asp:CheckBox ID="chkIsIMM" runat="server" Text="IMM" Checked="false" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("Jumlah Resep")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtDispenseQty" Width="100%" CssClass="number" max="100000" />
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtDispenseUnit" Width="100%" ReadOnly="true" TabIndex="999" />
                                                </td>
                                                <td class="tdLabel" style="padding-left: 10px" colspan="2">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("AC/DC/PC")%></label>
                                                </td>
                                                <td colspan="2">
                                                    <dxe:ASPxComboBox runat="server" ID="cboCoenamRule" ClientInstanceName="cboCoenamRule"
                                                        Width="110px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("Jumlah Diambil")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtTakenQty" Width="100%" CssClass="number" />
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtTakenUnit" Width="100%" ReadOnly="true" TabIndex="999" />
                                                </td>
                                                <td class="tdLabel" colspan="2" style="padding-left: 10px">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Expired Date")%></label>
                                                </td>
                                                <td colspan="2">
                                                    <asp:TextBox runat="server" ID="txtExpiredDate" CssClass="datepicker" Width="110px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Instruksi Khusus")%></label>
                                                </td>
                                                <td colspan="6">
                                                    <asp:TextBox ID="txtMedicationAdministration" Width="100%" runat="server" TextMode="MultiLine" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Alasan Pengobatan")%>
                                                        <br />
                                                        <%=GetLabel("(Fungsi Obat)")%></label>
                                                </td>
                                                <td colspan="6">
                                                    <asp:TextBox ID="txtPurposeOfMedication" Width="100%" runat="server" TextMode="MultiLine" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td valign="top" style="padding-left: 20px">
                                        <table style="width: 100%">
                                            <colgroup>
                                                <col style="width: 150px" />
                                                <col style="width: 120px" />
                                                <col style="width: 120px" />
                                                <col style="width: 120px" />
                                                <col style="width: 80px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Lokasi")%></label>
                                                </td>
                                                <td colspan="2">
                                                    <dxe:ASPxComboBox runat="server" ID="cboLocation" ClientInstanceName="cboLocation"
                                                        Width="100%" />
                                                </td>
                                                <td>
                                                    <asp:CheckBox runat="server" ID="chkIsRx" Visible="false" /><%=GetLabel("Rx")%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblNormal lblLink" id="lblEmbalace">
                                                        <%=GetLabel("Embalase")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtEmbalaceCode" Width="100%" />
                                                </td>
                                                <td colspan="2">
                                                    <asp:TextBox runat="server" ID="txtEmbalaceName" Width="100%" ReadOnly="true" TabIndex="999" />
                                                </td>
                                            </tr>
                                            <tr style="display: none">
                                                <td>
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Kelas Tagihan")%></label>
                                                </td>
                                                <td>
                                                    <dxe:ASPxComboBox runat="server" ID="cboChargeClass" ClientInstanceName="cboChargeClass"
                                                        Width="120px">
                                                        <ClientSideEvents SelectedIndexChanged="function(s,e){cboChargeClassChanged();}" />
                                                    </dxe:ASPxComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Harga Satuan") %></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtTariff" CssClass="txtCurrency" Width="120px" ReadOnly="true"
                                                        TabIndex="999" />
                                                </td>
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Jumlah Embalase")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtEmbalaceQty" CssClass="number" Width="100%" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="6" />
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="lblNormal">
                                                        <%=GetLabel("SUB TOTAL") %></label>
                                                </td>
                                                <td>
                                                    <div class="lblComponent">
                                                        <%=GetLabel("HARGA") %></div>
                                                </td>
                                                <td>
                                                    <div class="lblComponent">
                                                        <%=GetLabel("DISCOUNT") %></div>
                                                </td>
                                                <td>
                                                    <div class="lblComponent">
                                                        <%=GetLabel("EMBALASE + R/") %></div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtTotalAmount" CssClass="txtCurrency" ReadOnly="true"
                                                        TabIndex="999" />
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtDiscountAmount" CssClass="txtCurrency" TabIndex="999" />
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtEmbalaceAmount" CssClass="txtCurrency" ReadOnly="true"
                                                        TabIndex="999" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="lblNormal">
                                                        <%=GetLabel("TOTAL") %></label>
                                                </td>
                                                <td>
                                                    <div class="lblComponent">
                                                        <%=GetLabel("PASIEN") %></div>
                                                </td>
                                                <td>
                                                    <div class="lblComponent">
                                                        <%=GetLabel("INSTANSI") %></div>
                                                </td>
                                                <td>
                                                    <div class="lblComponent">
                                                        <%=GetLabel("TOTAL") %></div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtPatientAmount" CssClass="txtCurrency" />
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtPayerAmount" CssClass="txtCurrency" />
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtLineAmount" CssClass="txtCurrency" ReadOnly="true"
                                                        TabIndex="999" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table>
                                            <tr>
                                                <td colspan="3">
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
                    <input type="hidden" value="" id="hdnID" runat="server" />
                    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
                    <input type="hidden" id="hdnPrescriptionFlag" runat="server" value="" />
                    <div style="position: relative;">
                        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ hideLoadingPanel(); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <%--                                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">--%>
                                    <asp:Panel runat="server" ID="pnlView">
                                        <asp:ListView ID="lvwView" runat="server">
                                            <EmptyDataTemplate>
                                                <table id="tblView" runat="server" class="grdNormal notAllowSelect" cellspacing="0"
                                                    rules="all">
                                                    <tr>
                                                        <th rowspan="2" style="width: 70px;">
                                                        </th>
                                                        <th rowspan="2" align="left">
                                                            <div>
                                                                <%=GetLabel("Obat")%>
                                                                -
                                                                <%=GetLabel("Kadar")%>
                                                                -
                                                                <%=GetLabel("Bentuk")%></div>
                                                            <div>
                                                                <div style="color: Blue; width: 35px; float: left;">
                                                                    <%=GetLabel("DOSIS")%>
                                                                </div>
                                                                <%=GetLabel("Jumlah")%>
                                                                -
                                                                <%=GetLabel("Rute")%>
                                                                -
                                                                <%=GetLabel("Aturan Pemakaian")%></div>
                                                        </th>
                                                        <th rowspan="2" align="right" style="padding: 3px; width: 80px;">
                                                            <div>
                                                                <%=GetLabel("Harga Satuan")%></div>
                                                        </th>
                                                        <th rowspan="2" align="right" style="padding: 3px; width: 80px;">
                                                            <div>
                                                                <%=GetLabel("Jasa R/")%></div>
                                                        </th>
                                                        <th rowspan="2" align="right" style="padding: 3px; width: 80px;">
                                                            <div>
                                                                <%=GetLabel("Embalase")%></div>
                                                        </th>
                                                        <th rowspan="2" align="right" style="padding: 3px; width: 80px;">
                                                            <div>
                                                                <%=GetLabel("Jumlah")%></div>
                                                        </th>
                                                        <th colspan="3">
                                                            <div>
                                                                <%=GetLabel("TOTAL") %></div>
                                                        </th>
                                                        <th rowspan="2" style="width: 80px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Petugas")%>
                                                            </div>
                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        <th style="width: 120px;">
                                                            <div>
                                                                <%=GetLabel("Instansi") %></div>
                                                        </th>
                                                        <th style="width: 120px;">
                                                            <div>
                                                                <%=GetLabel("Pasien") %></div>
                                                        </th>
                                                        <th style="width: 120px;">
                                                            <div>
                                                                <%=GetLabel("Total") %></div>
                                                        </th>
                                                    </tr>
                                                    <tr align="center" style="height: 50px; vertical-align: middle;">
                                                        <td colspan="10">
                                                        </td>
                                                    </tr>
                                                </table>
                                            </EmptyDataTemplate>
                                            <LayoutTemplate>
                                                <table id="tblView" runat="server" class="grdNormal notAllowSelect" cellspacing="0"
                                                    rules="all">
                                                    <tr>
                                                        <th rowspan="2" style="width: 70px;">
                                                        </th>
                                                        <th rowspan="2" align="left">
                                                            <div>
                                                                <%=GetLabel("Obat")%>
                                                                -
                                                                <%=GetLabel("Kadar")%>
                                                                -
                                                                <%=GetLabel("Bentuk")%></div>
                                                            <div>
                                                                <div style="color: Blue; width: 35px; float: left;">
                                                                    <%=GetLabel("DOSIS")%></div>
                                                                <%=GetLabel("Jumlah")%>
                                                                -
                                                                <%=GetLabel("Rute")%>
                                                                -
                                                                <%=GetLabel("Aturan Pemakaian")%></div>
                                                        </th>
                                                        <th rowspan="2" align="right" style="padding: 3px; width: 80px;">
                                                            <div>
                                                                <%=GetLabel("Harga Satuan")%></div>
                                                        </th>
                                                        <th rowspan="2" align="right" style="padding: 3px; width: 80px;">
                                                            <div>
                                                                <%=GetLabel("Jasa R/")%></div>
                                                        </th>
                                                        <th rowspan="2" align="right" style="padding: 3px; width: 80px;">
                                                            <div>
                                                                <%=GetLabel("Embalase")%></div>
                                                        </th>
                                                        <th rowspan="2" align="right" style="padding: 3px; width: 80px;">
                                                            <div>
                                                                <%=GetLabel("Jumlah")%></div>
                                                        </th>
                                                        <th colspan="3">
                                                            <div>
                                                                <%=GetLabel("TOTAL") %></div>
                                                        </th>
                                                        <th rowspan="2" style="width: 80px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Petugas")%>
                                                            </div>
                                                        </th>
                                                        <th rowspan="2" style="width: 70px;">
                                                            &nbsp;
                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        <th style="width: 100px;">
                                                            <div>
                                                                <%=GetLabel("INSTANSI") %></div>
                                                        </th>
                                                        <th style="width: 100px;">
                                                            <div>
                                                                <%=GetLabel("PASIEN") %></div>
                                                        </th>
                                                        <th style="width: 100px;">
                                                            <div>
                                                                <%=GetLabel("TOTAL") %></div>
                                                        </th>
                                                    </tr>
                                                    <tr runat="server" id="itemPlaceholder">
                                                    </tr>
                                                    <tr class="trFooter">
                                                        <td colspan="6">
                                                            <div style="text-align: right; padding: 0px 3px">
                                                                <%=GetLabel("Total")%>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="text-align: right; padding: 0px 3px" id="tdTotalAllPayer" runat="server">
                                                                Instansi
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="text-align: right; padding: 0px 3px" id="tdTotalAllPatient" runat="server">
                                                                Pasien
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="text-align: right; padding: 0px 3px" id="tdTotalAll" runat="server">
                                                                Total
                                                            </div>
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
                                                    <td>
                                                        <img class="imgEdit <%# IsEditable() == "0" || Eval("IsVerified").ToString() == "True" ? "imgDisabled" : "imgLink"%>"
                                                            src='<%# IsEditable() == "0" || Eval("IsVerified").ToString() == "True" ? ResolveUrl("~/Libs/Images/Button/edit_disabled.png") : ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                            alt="" style="float: left; margin-left: 7px" />
                                                        &nbsp;
                                                        <img class="imgDelete <%# IsEditable() == "0" || Eval("IsVerified").ToString() == "True" ? "imgDisabled" : "imgLink"%>"
                                                            src='<%# IsEditable() == "0" || Eval("IsVerified").ToString() == "True" ? ResolveUrl("~/Libs/Images/Button/delete_disabled.png") : ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                            alt="" />
                                                        <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                        <input type="hidden" value="<%#:Eval("SignaID") %>" bindingfield="SignaID" />
                                                        <input type="hidden" value="<%#:Eval("SignaLabel") %>" bindingfield="SignaLabel" />
                                                        <input type="hidden" value="<%#:Eval("SignaName1") %>" bindingfield="SignaName1" />
                                                        <input type="hidden" value="<%#:Eval("PrescriptionOrderDetailID") %>" bindingfield="PrescriptionOrderDetailID" />
                                                        <input type="hidden" value="<%#:Eval("GenericName") %>" bindingfield="GenericName" />
                                                        <input type="hidden" value="<%#:Eval("IsRFlag") %>" bindingfield="IsRFlag" />
                                                        <input type="hidden" value="<%#:Eval("IsNonMaster") %>" bindingfield="IsNonMaster" />
                                                        <input type="hidden" value="<%#:Eval("IsCompound") %>" bindingfield="IsCompound" />
                                                        <input type="hidden" value="<%#:Eval("ItemCode") %>" bindingfield="ItemCode" />
                                                        <input type="hidden" value="<%#:Eval("ItemID") %>" bindingfield="ItemID" />
                                                        <input type="hidden" value="<%#:Eval("LocationID") %>" bindingfield="LocationID" />
                                                        <input type="hidden" value="<%#:Eval("GCItemUnit") %>" bindingfield="GCItemUnit" />
                                                        <input type="hidden" value="<%#:Eval("ItemUnit") %>" bindingfield="ItemUnit" />
                                                        <input type="hidden" value="<%#:Eval("GCBaseUnit") %>" bindingfield="GCBaseUnit" />
                                                        <input type="hidden" value="<%#:Eval("DrugName") %>" bindingfield="DrugName" />
                                                        <input type="hidden" value="<%#:Eval("GCCoenamRule") %>" bindingfield="GCCoenamRule" />
                                                        <input type="hidden" value="<%#:Eval("GCDrugForm") %>" bindingfield="GCDrugForm" />
                                                        <input type="hidden" value="<%#:Eval("Dose") %>" bindingfield="Dose" />
                                                        <input type="hidden" value="<%#:Eval("GCDoseUnit") %>" bindingfield="GCDoseUnit" />
                                                        <input type="hidden" value="<%#:Eval("DoseUnit") %>" bindingfield="DoseUnit" />
                                                        <input type="hidden" value="<%#:Eval("GCDosingFrequency") %>" bindingfield="GCDosingFrequency" />
                                                        <input type="hidden" value="<%#:Eval("Frequency") %>" bindingfield="Frequency" />
                                                        <input type="hidden" value="<%#:Eval("NumberOfDosage") %>" bindingfield="NumberOfDosage" />
                                                        <input type="hidden" value="<%#:Eval("GCDosingUnit") %>" bindingfield="GCDosingUnit" />
                                                        <input type="hidden" value="<%#:Eval("DosingDuration") %>" bindingfield="DosingDuration" />
                                                        <input type="hidden" value="<%#:Eval("GCRoute") %>" bindingfield="GCRoute" />
                                                        <input type="hidden" value="<%#:Eval("MedicationPurpose") %>" bindingfield="MedicationPurpose" />
                                                        <input type="hidden" value="<%#:Eval("StartDate") %>" bindingfield="StartDate" />
                                                        <input type="hidden" value="<%#:Eval("StartTime") %>" bindingfield="StartTime" />
                                                        <input type="hidden" value="<%#:Eval("DispenseQty") %>" bindingfield="DispenseQty" />
                                                        <input type="hidden" value="<%#:Eval("MedicationAdministration") %>" bindingfield="MedicationAdministration" />
                                                        <input type="hidden" value="<%#:Eval("cfStartDateInDatePickerFormat") %>" bindingfield="StartDateInDatePickerFormat" />
                                                        <input type="hidden" value="<%#:Eval("TakenQty") %>" bindingfield="TakenQty" />
                                                        <input type="hidden" value="<%#:Eval("ChargedQuantity") %>" bindingfield="ChargedQuantity" />
                                                        <input type="hidden" value="<%#:Eval("ChargeClassID") %>" bindingfield="ChargeClassID" />
                                                        <input type="hidden" value="<%#:Eval("Tariff") %>" bindingfield="Tariff" />
                                                        <input type="hidden" value="<%#:Eval("DiscountAmount") %>" bindingfield="DiscountAmount" />
                                                        <input type="hidden" value="<%#:Eval("PatientAmount") %>" bindingfield="PatientAmount" />
                                                        <input type="hidden" value="<%#:Eval("PayerAmount") %>" bindingfield="PayerAmount" />
                                                        <input type="hidden" value="<%#:Eval("LineAmount") %>" bindingfield="LineAmount" />
                                                        <input type="hidden" value="<%#:Eval("EmbalaceID") %>" bindingfield="EmbalaceID" />
                                                        <input type="hidden" value="<%#:Eval("EmbalaceName") %>" bindingfield="EmbalaceName" />
                                                        <input type="hidden" value="<%#:Eval("EmbalaceCode") %>" bindingfield="EmbalaceCode" />
                                                        <input type="hidden" value="<%#:Eval("EmbalaceQty") %>" bindingfield="EmbalaceQty" />
                                                        <input type="hidden" value="<%#:Eval("EmbalaceAmount") %>" bindingfield="EmbalaceAmount" />
                                                        <input type="hidden" value="<%#:Eval("PrescriptionFeeAmount") %>" bindingfield="PrescriptionFeeAmount" />
                                                        <input type="hidden" value="<%#:Eval("IsMorning") %>" bindingfield="IsMorning" />
                                                        <input type="hidden" value="<%#:Eval("IsNoon") %>" bindingfield="IsNoon" />
                                                        <input type="hidden" value="<%#:Eval("IsEvening") %>" bindingfield="IsEvening" />
                                                        <input type="hidden" value="<%#:Eval("IsNight") %>" bindingfield="IsNight" />
                                                        <input type="hidden" value="<%#:Eval("IsAsRequired") %>" bindingfield="IsAsRequired" />
                                                        <input type="hidden" value="<%#:Eval("IsIMM") %>" bindingfield="IsIMM" />
                                                    </td>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <b>
                                                                        <div style="<%# Eval("GCDrugClass").ToString() == "X123^O" ? "color: Red": Eval("GCDrugClass").ToString() == "X123^P" ? "color: Blue" : "color: Black"%>">
                                                                            <%#: Eval("cfItemNameWithNumero")%></div>
                                                                    </b>
                                                                    <div style='<%# Eval("IsCompound").ToString() == "False" ? "display:none;": "white-space: pre-line;font-style:italic; padding-top:5px" %>'>
                                                                        <%#: Eval("cfCompoundDetail")%></div>
                                                                </td>
                                                                <td rowspan="2">
                                                                    &nbsp;
                                                                </td>
                                                                <td rowspan="2">
                                                                    <div>
                                                                        <img src='<%# ResolveUrl("~/Libs/Images/Misc/compound.png")%>' alt="" style='<%# Eval("IsCompound").ToString() == "False" ? "display:none;": "" %> max-width:30px;
                                                                            min-width: 30px; float: left;' /></div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <div>
                                                                        <div style="color: Blue; width: 35px; float: left;">
                                                                            <%=GetLabel("DOSE")%></div>
                                                                        <%#: Eval("NumberOfDosage")%>
                                                                        <%#: Eval("DosingUnit")%>
                                                                        -
                                                                        <%#: Eval("Route")%>
                                                                        -
                                                                        <%#: Eval("cfDoseFrequency")%>
                                                                    </div>
                                                                    <div>
                                                                        <%#: Eval("MedicationAdministration")%>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td style="padding: 3px;" align="right">
                                                        <div style='<%# Eval("IsCompound").ToString() == "True" ? "display:none;": "" %>'>
                                                            <%#: Eval("Tariff", "{0:N}")%></div>
                                                    </td>
                                                    <td style="padding: 3px;" align="right">
                                                        <%#:Eval("PrescriptionFeeAmount", "{0:N}")%>
                                                    </td>
                                                    <td style="padding: 3px;" align="right">
                                                        <%--<div style='<%# Eval("IsCompound").ToString() == "False" ? "display:none;": "" %>'>
                                                            <%#: Eval("EmbalaceAmount", "{0:N}")%></div>--%>
                                                        <%#: Eval("EmbalaceAmount", "{0:N}")%>
                                                    </td>
                                                    <td style="padding: 3px;" align="right">
                                                        <div>
                                                            <%#: Eval("TakenQty")%>
                                                            <%#: Eval("cfItemUnit")%></div>
                                                    </td>
                                                    <td align="right">
                                                        <%#:Eval("PayerAmount", "{0:N}")%>
                                                    </td>
                                                    <td align="right">
                                                        <%#:Eval("PatientAmount", "{0:N}")%>
                                                    </td>
                                                    <td align="right">
                                                        <%#:Eval("LineAmount", "{0:N}")%>
                                                    </td>
                                                    <td>
                                                        <div style="padding-right: 3px; text-align: right;">
                                                            <div>
                                                                <%#: Eval("ChargesByUserName")%></div>
                                                        </div>
                                                    </td>
                                                    <td valign="middle">
                                                        <div style="text-align: center">
                                                            <img <%# IsShowSwitchIcon() == "0" || IsEditable() == "0" || Eval("IsVerified").ToString() == "True" ? "style='display:none'" : "" %>
                                                                class="imgSwitch imgLink" title='Switch' src='<%# ResolveUrl("~/Libs/Images/Button/switch.png")%>'
                                                                alt="" />
                                                        </div>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:ListView>
                                        <div style="width: 100%; text-align: center">
                                            <span class="lblLink" id="lblAddData" style="margin-right: 300px;">
                                                <%= GetLabel("Tambah Obat")%></span> <span class="lblLink" id="lblAddNonMaster" style='margin-right: 200px;'>
                                                    <%= GetLabel("Tambah Obat Non Master")%></span> <span class="lblLink" id="lblQuickPick"
                                                        style='margin-right: 200px;'>
                                                        <%= GetLabel("Quick Picks")%></span> <span class="lblLink" id="lblQuickPicksHistory"
                                                            style="padding-right: 200px">
                                                            <%= GetLabel("Quick Picks History")%></span> <span class="lblLink" id="lblQuickPicksFromScheduled"
                                                                style="padding-right: 200px">
                                                                <%= GetLabel("Quick Picks from Scheduled")%></span> <span class="lblLink" id="lblTemplate"
                                                                    style="display: none">
                                                                    <%= GetLabel("")%>
                                                                </span>
                                        </div>
                                    </asp:Panel>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>
                    </div>
                    <div>
                        <table width="100%">
                            <tr>
                                <td colspan="2">
                                    <div>
                                        <table width="100%">
                                            <tr>
                                                <td colspan="2">
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
                                                                                <tr id="trProposedBy" style="display: none" runat="server">
                                                                                    <td align="left">
                                                                                        <%=GetLabel("Dipropose Oleh") %>
                                                                                    </td>
                                                                                    <td align="center">
                                                                                        :
                                                                                    </td>
                                                                                    <td>
                                                                                        <div runat="server" id="divProposedBy">
                                                                                        </div>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr id="trProposedDate" style="display: none" runat="server">
                                                                                    <td align="left">
                                                                                        <%=GetLabel("Dipropose Pada") %>
                                                                                    </td>
                                                                                    <td align="center">
                                                                                        :
                                                                                    </td>
                                                                                    <td>
                                                                                        <div runat="server" id="divProposedDate">
                                                                                        </div>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr id="trVoidBy" style="display: none" runat="server">
                                                                                    <td align="left">
                                                                                        <%=GetLabel("Divoid Oleh") %>
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
                                                                                        <%=GetLabel("Divoid Pada") %>
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
                                                                                <tr id="trVoidReason" style="display: none" runat="server">
                                                                                    <td align="left">
                                                                                        <%=GetLabel("Alasan Batal")%>
                                                                                    </td>
                                                                                    <td align="center">
                                                                                        :
                                                                                    </td>
                                                                                    <td>
                                                                                        <div runat="server" id="divVoidReason">
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
