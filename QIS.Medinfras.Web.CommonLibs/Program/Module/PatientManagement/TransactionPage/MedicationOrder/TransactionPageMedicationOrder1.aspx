<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx2.master"
    AutoEventWireup="true" CodeBehind="TransactionPageMedicationOrder1.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.TransactionPageMedicationOrder1" %>

<%--<%@ Register Src="~/Libs/Controls/PatientBannerCtl.ascx" TagName="PatientBannerCtl"
    TagPrefix="uc1" %>--%>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionPage/TransactionPageToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <input type="hidden" value="" id="hdnDefaultDispensaryServiceUnitID" runat="server" />
    <li id="btnPrescriptionCompoundEntry" runat="server" crudmode="C" visible="false">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbcompound.png")%>' alt="" /><div>
            <%=GetLabel("Obat Racikan")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">
    <input type="hidden" value="" id="hdnGCRegistrationStatus" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnVisitID" runat="server" />
    <input type="hidden" value="" id="hdnClassID" runat="server" />
    <input type="hidden" value="0" id="hdnIsAllowOverIssued" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" runat="server" id="hdnSelectedItem" value="" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onGetDrugFilterExpression() {
            var LocationID = cboLocation.GetValue();
            var filterExpression = "LocationID = " + LocationID + " AND IsDeleted = 0";
            if ($('#<%=hdnIsAllowOverIssued.ClientID %>').val() == '0') {
                filterExpression += " AND QuantityEND > 0 ";
            }
            var presOrderID = $('#<%=hdnPrescriptionOrderID.ClientID %>').val();
            if ($('#<%=hdnPrescriptionOrderID.ClientID %>').val() != '0' && $('#<%=hdnPrescriptionOrderID.ClientID %>').val() != null) {
                var presOrderID = $('#<%=hdnPrescriptionOrderID.ClientID %>').val();
                filterExpression += " AND ItemID NOT IN (SELECT ItemID FROM PrescriptionOrderDt WHERE PrescriptionOrderID = " + presOrderID + " AND IsDeleted = 0)";
            }

            filterExpression += " AND GCItemStatus != 'X181^999'";

            var drugTransactionType = $('#<%=hdnIsDrugChargesJustDistribution.ClientID %>').val();
            if (drugTransactionType == "1") {
                filterExpression += " AND GCItemRequestType = 'X217^01'";
            }

            return filterExpression;
        }

        function onLoad() {
            setDatePicker('<%=txtPrescriptionDate.ClientID %>');
            $('#<%=txtPrescriptionDate.ClientID %>').datepicker('option', 'maxDate', '0');
            setDatePicker('<%=txtStartDate.ClientID %>');

            $('#<%=btnPrescriptionCompoundEntry.ClientID %>').click(function (evt) {
                if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                    openCompoundEntry('add');
                }
            });

            var editable = "<%=IsEditable %>";
            if (!isShowWatermark()) {
                $('#<%=btnPrescriptionCompoundEntry.ClientID %>').show();
            }
            else {
                $('#<%=btnPrescriptionCompoundEntry.ClientID %>').hide();
            }

            //#region Transaction No
            $('#lblPrescriptionOrderNo.lblLink').click(function () {
                var filterExpression = 'VisitID = ' + $('#<%=hdnVisitID.ClientID %>').val();
                openSearchDialog('prescriptionorderhd', filterExpression, function (value) {
                    $('#<%=txtPrescriptionOrderNo.ClientID %>').val(value);
                    onTxtPrescriptionOrderNoChanged(value);
                });
            });

            $('#<%=txtPrescriptionOrderNo.ClientID %>').change(function () {
                onTxtPrescriptionOrderNoChanged($(this).val());
            });

            function onTxtPrescriptionOrderNoChanged(value) {
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

            $('#<%=txtPhysicianCode.ClientID %>').change(function () {
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

            //#region Drug
            $('#lblDrug.lblLink').click(function () {
                openSearchDialog('druginfoitembalance', onGetDrugFilterExpression(), function (value) {
                    $('#<%=txtDrugCode.ClientID %>').val(value);
                    ontxtDrugCodeChanged(value);
                });
            });

            $('#<%=txtDrugCode.ClientID %>').change(function () {
                ontxtDrugCodeChanged($(this).val());
            });

            function ontxtDrugCodeChanged(value) {
                var filterExpression = onGetDrugFilterExpression() + " AND ItemCode = '" + value + "'";
                Methods.getObject('GetvDrugInfoItemBalanceList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnDrugID.ClientID %>').val(result.ItemID);
                        $('#<%=txtDrugCode.ClientID %>').val(result.ItemCode);
                        $('#<%=txtDrugName.ClientID %>').val(result.ItemName1);
                        $('#<%=txtGenericName.ClientID %>').val(result.GenericName);
                        cboForm.SetEnabled(true);
                        cboForm.SetValue(result.GCDrugForm);
                        cboForm.SetEnabled(false);
                        cboCoenamRule.SetValue(result.GCCoenamRule);
                        cboMedicationRoute.SetValue(result.GCMedicationRoute);
                        $('#<%=txtStrengthAmount.ClientID %>').val(result.Dose);
                        $('#<%=txtStrengthUnit.ClientID %>').val(result.DoseUnit);
                        $('#<%=hdnGCDoseUnit.ClientID %>').val(result.GCDoseUnit);
                        $('#<%=hdnGCItemUnit.ClientID %>').val(result.GCItemUnit);
                        $('#<%=hdnGCDosingUnit.ClientID %>').val(result.GCItemUnit);
                        $('#<%=txtDispenseUnit.ClientID %>').val(result.ItemUnit);
                        cboDosingUnit.PerformCallback();
                    }
                    else {
                        $('#<%=hdnDrugID.ClientID %>').val('');
                        $('#<%=txtDrugCode.ClientID %>').val('');
                        $('#<%=txtDrugName.ClientID %>').val('');
                        $('#<%=txtGenericName.ClientID %>').val('');
                        cboForm.SetValue('');
                        cboCoenamRule.SetValue('');
                        cboMedicationRoute.SetValue('');
                        $('#<%=txtStrengthAmount.ClientID %>').val('');
                        $('#<%=txtStrengthUnit.ClientID %>').val('');
                        $('#<%=hdnGCDoseUnit.ClientID %>').val('');
                        $('#<%=hdnGCItemUnit.ClientID %>').val('');
                        $('#<%=txtDispenseUnit.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Signa
            $('#lblSigna.lblLink').click(function () {
                var filterExpression = "IsDeleted = 0";
                openSearchDialog('signa', filterExpression, function (value) {
                    $('#<%=txtSignaLabel.ClientID %>').val(value);
                    txtSignaLabelChanged(value);
                });
            });

            $('#<%=txtSignaLabel.ClientID %>').change(function () {
                txtSignaLabelChanged($(this).val());
            });

            function txtSignaLabelChanged(value) {
                var filterExpression = "IsDeleted = 0 AND SignaLabel = '" + value + "'";
                var dispQty = $('#<%=txtDispenseQty.ClientID %>').val();
                Methods.getObject('GetvSignaList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnSignaID.ClientID %>').val(result.SignaID);
                        $('#<%=txtSignaName1.ClientID %>').val(result.SignaName1);
                        cboForm.SetValue(result.GCDrugForm);
                        $('#<%=txtFrequencyNumber.ClientID %>').val(result.Frequency);
                        cboFrequencyTimeline.SetValue(result.GCDosingFrequency);
                        $('#<%=txtDosingDose.ClientID %>').val(result.Dose);
                        cboDosingUnit.SetValue(result.GCDoseUnit);
                        cboCoenamRule.SetValue(result.GCCoenamRule);
                        $('#<%=hdnTakenUnit.ClientID %>').val(cboDosingUnit.GetText());
                        $('#<%=txtDosingDurationTimeline.ClientID %>').val(cboFrequencyTimeline.GetText());
                    } else {
                        $('#<%=txtSignaName1.ClientID %>').val('');
                        cboForm.SetValue('');
                        $('#<%=txtFrequencyNumber.ClientID %>').val('');
                        cboFrequencyTimeline.SetValue('');
                        $('#<%=txtDosingDose.ClientID %>').val('');
                        cboDosingUnit.SetValue('');
                        cboCoenamRule.SetValue('');
                        $('#<%=hdnTakenUnit.ClientID %>').val('');
                        $('#<%=txtDosingDurationTimeline.ClientID %>').val('');
                    }
                });
                if ((dispQty <= 0)) {
                    calculateDispenseQty();
                }
            }
            //#endregion

            //#region Operasi
            $('#lblAddData').die('click');
            $('#lblAddData').live('click', function (evt) {
                if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                    var startDate = $('#<%=txtPrescriptionDate.ClientID %>').val();
                    var startTime = $('#<%=txtPrescriptionTime.ClientID %>').val();

                    $('#<%=lblPhysician.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%=txtPhysicianCode.ClientID %>').attr('readonly', 'readonly');
                    cboDispensaryUnit.SetEnabled(false);
//                    cboLocation.SetEnabled(false);
                    $('#<%=txtPrescriptionDate.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtPrescriptionTime.ClientID %>').attr('readonly', 'readonly');

                    $('#containerEntry').show();
                    $('#<%=hdnEntryID.ClientID %>').val('');
                    $('#<%=chkIsRx.ClientID %>').prop('checked', true);
                    $('#<%=txtGenericName.ClientID %>').val('');
                    $('#<%=hdnDrugID.ClientID %>').val('');
                    cboForm.SetValue('');
                    cboCoenamRule.SetValue('');
                    $('#<%=txtPurposeOfMedication.ClientID %>').val('');
                    $('#<%=txtStrengthAmount.ClientID %>').val('');
                    $('#<%=txtStrengthUnit.ClientID %>').val('');
                    cboFrequencyTimeline.SetValue(Constant.DosingFrequency.DAY);
                    $('#<%=txtFrequencyNumber.ClientID %>').val('');
                    $('#<%=txtDosingDose.ClientID %>').val('');
                    cboDosingUnit.SetValue('');
                    cboMedicationRoute.SetValue('');
                    $('#<%=txtMedicationAdministration.ClientID %>').val('');
                    $('#<%=txtStartDate.ClientID %>').val(startDate);
                    $('#<%=txtStartTime.ClientID %>').val(startTime);
                    $('#<%=txtDosingDuration.ClientID %>').val('');
                    $('#<%=txtDosingDurationTimeline.ClientID %>').val(cboFrequencyTimeline.GetText());
                    $('#<%=txtDrugName.ClientID %>').val('');
                    $('#<%=txtDrugCode.ClientID %>').val('');
                    $('#<%=hdnDrugID.ClientID %>').val('');
                    $('#<%=txtSignaLabel.ClientID %>').val('');
                    $('#<%=txtSignaName1.ClientID %>').val('');
                    $('#<%=hdnSignaID.ClientID %>').val('');
                    $('#<%=txtDispenseQty.ClientID %>').val('');
                    $('#<%=txtDispenseUnit.ClientID %>').val('');
                    $('#<%=hdnTakenQty.ClientID %>').val('');
                    $('#<%=hdnTakenUnit.ClientID %>').val('');
                }
            });

            $('#btnCancel').click(function () {
                $('#containerEntry').hide();
            });

            $('#btnSave').click(function (evt) {
                if (IsValid(evt, 'fsTrx', 'mpTrx'))
                    cbpProcess.PerformCallback('save');
            });
            //#endregion
        }

        $('.imgEdit.imgLink').die('click');
        $('.imgEdit.imgLink').live('click', function () {
            $row = $(this).closest('tr').parent().closest('tr');
            var entity = rowToObject($row);
            if (entity.IsCompound != 'True') {
                $('#<%=hdnEntryID.ClientID %>').val(entity.PrescriptionOrderDetailID);
                $('#<%=chkIsRx.ClientID %>').prop('checked', (entity.IsRFlag == 'True'));
                $('#<%=txtDrugCode.ClientID %>').val(entity.ItemCode);
                $('#<%=txtDrugName.ClientID %>').val(entity.DrugName);
                $('#<%=txtGenericName.ClientID %>').val(entity.GenericName);
                $('#<%=hdnDrugID.ClientID %>').val(entity.ItemID);
                cboForm.SetValue(entity.GCDrugForm);
                $('#<%=txtPurposeOfMedication.ClientID %>').val(entity.MedicationPurpose);
                $('#<%=txtStrengthAmount.ClientID %>').val(entity.Dose);
                $('#<%=txtStrengthUnit.ClientID %>').val(entity.DoseUnit);
                $('#<%=hdnGCDoseUnit.ClientID %>').val(entity.GCDoseUnit);
                cboFrequencyTimeline.SetValue(entity.GCDosingFrequency);
                cboCoenamRule.SetValue(entity.GCCoenamRule);
                $('#<%=txtFrequencyNumber.ClientID %>').val(entity.Frequency);
                $('#<%=txtDosingDose.ClientID %>').val(entity.NumberOfDosage);
                $('#<%=hdnGCDosingUnit.ClientID %>').val(entity.GCDosingUnit);
                cboDosingUnit.PerformCallback();
                cboMedicationRoute.SetValue(entity.GCRoute);
                $('#<%=txtStartDate.ClientID %>').val(entity.StartDateInDatePickerFormat);
                $('#<%=txtStartTime.ClientID %>').val(entity.StartTime);
                $('#<%=txtMedicationAdministration.ClientID %>').val(entity.MedicationAdministration);
                $('#<%=txtDosingDuration.ClientID %>').val(entity.DosingDuration);
                $('#<%=txtDispenseQty.ClientID %>').val(entity.DispenseQty);
                $('#<%=txtDispenseUnit.ClientID %>').val(entity.ItemUnit);
                $('#<%=hdnTakenQty.ClientID %>').val(entity.TakenQty);
                $('#<%=hdnSignaID.ClientID %>').val(entity.SignaID);
                $('#<%=txtSignaName1.ClientID %>').val(entity.SignaName1);
                $('#<%=txtSignaLabel.ClientID %>').val(entity.SignaLabel);
                $('#<%=txtDosingDurationTimeline.ClientID %>').val(cboFrequencyTimeline.GetText());
                $('#containerEntry').show();
            } else {
                $('#<%=hdnEntryID.ClientID %>').val(entity.PrescriptionOrderDetailID);
                openCompoundEntry('edit');
            }
        });

        $('.imgDelete.imgLink').die('click');
        $('.imgDelete.imgLink').live('click', function () {
            $row = $(this).closest('tr').parent().closest('tr');
            showToastConfirmation('Are You Sure Want To Delete?', function (result) {
                if (result) {
                    var entity = rowToObject($row);
                    $('#<%=hdnEntryID.ClientID %>').val(entity.PrescriptionOrderDetailID);
                    cbpProcess.PerformCallback('delete');
                }
            });
        });

        $('#lblQuickPickHistoryResep').die('click');
        $('#lblQuickPickHistoryResep').live('click', function () {
            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/MedicationOrder/MedicationOrderHistoryCtl.ascx");
            var locationID = cboLocation.GetValue();
            var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
            var visitID = $('#<%=hdnVisitID.ClientID %>').val();
            var prescriptionOrderID = $('#<%=hdnPrescriptionOrderID.ClientID %>').val();
            var classID = $('#<%=hdnClassID.ClientID %>').val();
            var physician = $('#<%=hdnPhysicianID.ClientID %>').val();
            var prescriptionType = cboPrescriptionType.GetValue();
            var dispensaryUnit = cboDispensaryUnit.GetValue();
            var departmentID = $('#<%=hdnDepartmentID.ClientID %>').val();
            var drugTransactionType = $('#<%=hdnIsDrugChargesJustDistribution.ClientID %>').val();

            var queryString = locationID + '|' + registrationID + '|' + prescriptionOrderID + '|' + visitID + '|' + classID + '|' + physician + '|' + prescriptionType + '|' + dispensaryUnit + '|' + departmentID + '|' + drugTransactionType;
            openUserControlPopup(url, queryString, 'History Order Resep', 1300, 600);
        });
        $('#lblTestOrderQuickPick').die('click');
        $('#lblTestOrderQuickPick').live('click', function (evt) {
            if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                showLoadingPanel();

                var url = ResolveUrl('~/Libs/Program/Module/PatientManagement/TransactionPage/MedicationOrder/QuickPicksCtl1.ascx');
                var orderID = $('#<%=hdnPrescriptionOrderID.ClientID %>').val();

                var locationID = cboLocation.GetValue();
                var defaultGCMedicationRoute = $('#<%=hdnDefaultGCMedicationRoute.ClientID %>').val();
                var paramedicID = $('#<%=hdnPhysicianID.ClientID %>').val();
                var dispensaryServiceUnitID = cboDispensaryUnit.GetValue();

                var registration = $('#<%=hdnRegistrationID.ClientID %>').val();
                var visitID = $('#<%=hdnVisitID.ClientID %>').val();
                var chargeClassID = $('#<%=hdnChargeClassID.ClientID %>').val();
                var orderNotes = $('#<%=txtRemarks.ClientID %>').val();
                var dispensaryUnitID = $('#<%=hdnDefaultDispensaryServiceUnitID.ClientID %>').val();
                var prescriptionType = cboPrescriptionType.GetValue();
                var date = Methods.dateToString(Methods.getDatePickerDate($('#<%=txtPrescriptionDate.ClientID %>').val()));
                var time = $('#<%=txtPrescriptionTime.ClientID %>').val();
                var id = orderID + '|' + locationID + '|' + defaultGCMedicationRoute + '|' + paramedicID + '|' + registration + '|' + visitID + '|' + chargeClassID + '|' + orderNotes + '|' + dispensaryServiceUnitID + '|' + date + '|' + time + '|' + prescriptionType;

                openUserControlPopup(url, id, 'Quick Picks', 1200, 600);
            }
        });

        function onAfterPopupControlClosing() {
            cbpView.PerformCallback('refresh');
        }

        function onAfterSaveAddRecordEntryPopup(param) {
            onAfterSaveRecordDtSuccess(param);
        }

        function onAfterSaveRecordDtSuccess(PrescriptionOrcbpProcessderID) {
            if ($('#<%=hdnPrescriptionOrderID.ClientID %>').val() == '0') {
                cboDispensaryUnit.SetEnabled(false);
                cboLocation.SetEnabled(false);
                $('#<%=txtPrescriptionDate.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtPrescriptionTime.ClientID %>').attr('readonly', 'readonly');
                $('#<%=hdnPrescriptionOrderID.ClientID %>').val(PrescriptionOrderID);
                var filterExpression = 'PrescriptionOrderID = ' + PrescriptionOrderID;
                Methods.getObject('GetPrescriptionOrderHdList', filterExpression, function (result) {
                    $('#<%=txtPrescriptionOrderNo.ClientID %>').val(result.PrescriptionOrderNo);
                    cbpView.PerformCallback('refresh');
                });
            }
            else
                cbpView.PerformCallback('refresh');
        }

        function openCompoundEntry(value) {
            var prescriptionID = $('#<%=hdnPrescriptionOrderID.ClientID %>').val();
            var prescriptionDetailID = $('#<%=hdnEntryID.ClientID %>').val();
            var date = Methods.dateToString(Methods.getDatePickerDate($('#<%=txtPrescriptionDate.ClientID %>').val()));
            var time = $('#<%=txtPrescriptionTime.ClientID %>').val();
            var physician = $('#<%=hdnPhysicianID.ClientID %>').val();
            var visitID = $('#<%=hdnVisitID.ClientID %>').val();
            var classID = $('#<%=hdnClassID.ClientID %>').val();
            var prescriptionType = cboPrescriptionType.GetValue();
            var refillInstruction = '';
            var drugTransactionType = $('#<%=hdnIsDrugChargesJustDistribution.ClientID %>').val();

            var queryString = "";
            if (value == "add") {
                queryString = prescriptionID + '|' + date + '|' + time + '|' + physician + '|' + refillInstruction + '|' + visitID + '|' + classID + '|' + cboDispensaryUnit.GetValue() + '|' + cboLocation.GetValue() + '|' + prescriptionType + '|' + drugTransactionType;
            }
            else {
                queryString = prescriptionID + '|' + prescriptionDetailID + '|' + visitID + '|' + drugTransactionType;
            }
            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/MedicationOrder/PrescriptionCompoundEntryCtl.ascx");
            openUserControlPopup(url, queryString, 'Order Farmasi - Obat Racikan', 1000, 600);
        }

        function onCboDosingUnitEndCallback() {
            var gcDosingUnit = $('#<%=hdnGCDosingUnit.ClientID %>').val();
            cboDosingUnit.SetValue(gcDosingUnit);
            $('#<%=hdnTakenUnit.ClientID %>').val(cboDosingUnit.GetText());
        }

        $('#<%=txtDispenseQty.ClientID %>').live('change', function () {
            var dispenseqty = $('#<%=txtDispenseQty.ClientID %>').val();
            if (dispenseqty <= 0 || dispesneqty == "") {
                showToast('Error Message', 'Quantity Resep tidak boleh kurang dari atau sama dengan 0 !');
                $('#<%=txtDispenseQty.ClientID %>').val('');
            }
        });

        //#region calculate Dispense Qty
        $('#<%=txtFrequencyNumber.ClientID %>').live('change', function () {
            calculateDispenseQty();
        });

        $('#<%=txtDosingDose.ClientID %>').live('change', function () {
            calculateDispenseQty();
        });

        $('#<%=txtDosingDuration.ClientID %>').live('change', function () {
            calculateDispenseQty();
        });

        function cboFrequencyTimelineChanged() {
            var frequencyTimeLine = cboFrequencyTimeline.GetText();
            $('#<%=txtDosingDurationTimeline.ClientID %>').val(frequencyTimeLine);
        }

        function onCboDosingUnitValueChanged() {
            //            var dosingUnit = cboDosingUnit.GetText();
            //            $('#<%=txtDispenseUnit.ClientID %>').val(dosingUnit);
            //            $('#<%=hdnTakenUnit.ClientID %>').val(dosingUnit);
        }

        function calculateDispenseQty() {
            var frequency = $('#<%=txtFrequencyNumber.ClientID %>').val();
            var frequencyTimeLine = cboFrequencyTimeline.GetValue();
            var dose = $('#<%=txtDosingDose.ClientID %>').val();
            var dosingDuration = $('#<%=txtDosingDuration.ClientID %>').val();
            var doseUnit = cboDosingUnit.GetValue();
            var strengthAmount = $('#<%=txtStrengthAmount.ClientID %>').val();
            var strengthUnit = $('#<%=hdnGCDoseUnit.ClientID %>').val();
            if (strengthAmount == "0") {
                strengthAmount = "1";
            }

            var frequencyInt = parseInt(frequency);
            var doseInt = parseInt(dose);
            var dosingDurationInt = parseInt(dosingDuration);

            if (frequencyInt < 0) {
                $('#<%=txtFrequencyNumber.ClientID %>').val('');
            }

            if (doseInt < 0) {
                $('#<%=txtDosingDose.ClientID %>').val('');
            }

            if (dosingDurationInt < 0) {
                $('#<%=txtDosingDuration.ClientID %>').val('');
            }


            if (frequency != '' && dose != '' && dosingDuration != '' && frequencyInt > 0 && doseInt > 0 && dosingDurationInt > 0) {
                var dispenseQty = dosingDuration * frequency * dose;
                if (strengthUnit == doseUnit) {
                    dispenseQty = Math.ceil(dispenseQty / strengthAmount);
                }
                $('#<%=txtDispenseQty.ClientID %>').val(dispenseQty);
                $('#<%=hdnTakenQty.ClientID %>').val(dispenseQty);
            }
            else {
                $('#<%=txtDispenseQty.ClientID %>').val('');
                $('#<%=hdnTakenQty.ClientID %>').val('');
            }
        }
        //#endregion

        function onAfterSaveRecordDtSuccess(PrescriptionOrderID) {
            if ($('#<%=hdnPrescriptionOrderID.ClientID %>').val() == '0') {
                cboDispensaryUnit.SetEnabled(false);
                cboLocation.SetEnabled(false);
                $('#<%=txtPrescriptionDate.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtPrescriptionTime.ClientID %>').attr('readonly', 'readonly');
                $('#<%=hdnPrescriptionOrderID.ClientID %>').val(PrescriptionOrderID);
                var filterExpression = 'PrescriptionOrderID = ' + PrescriptionOrderID;
                Methods.getObject('GetPrescriptionOrderHdList', filterExpression, function (result) {
                    $('#<%=txtPrescriptionOrderNo.ClientID %>').val(result.PrescriptionOrderNo);
                    cbpView.PerformCallback('refresh');
                });
                onAfterCustomSaveSuccess();
            }
            else
                cbpView.PerformCallback('refresh');
        }

        function onCbpProcesEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'save') {
                if (param[1] == 'fail') {
                    if (param[2] == 'confirm') {
                        showToast('Save Success', 'Warning Message : ' + param[3]);
                        var PrescriptionOrderID = s.cpPrescriptionOrderID;
                        onAfterSaveRecordDtSuccess(PrescriptionOrderID);
                        $('#containerEntry').hide();

                    }
                    else {
                        showToast('Save Failed', 'Error Message : ' + param[3]);
                        cbpView.PerformCallback('refresh');
                    }
                }
                else {
                    var PrescriptionOrderID = s.cpPrescriptionOrderID;
                    onAfterSaveRecordDtSuccess(PrescriptionOrderID);
                    $('#containerEntry').hide();
                    cboDispensaryUnit.SetEnabled(false);
                    cboLocation.SetEnabled(false);
                }

            }
            else if (param[0] == 'delete') {
                if (param[1] == 'fail')
                    showToast('Delete Failed', 'Error Message : ' + param[2]);
                else
                    cbpView.PerformCallback('refresh');
            }
        }

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var prescriptionOrderID = $('#<%=hdnPrescriptionOrderID.ClientID %>').val();
            var TransactionStatus = $('#<%:hdnGCTransactionStatus.ClientID %>').val();

            if (code == 'PM-00529') {
                if (TransactionStatus == Constant.TransactionStatus.OPEN) {
                    errMessage.text = 'Transaksi harus dipropose terlebih dahulu sebelum proses bisa dilakukan.';
                    return false;
                } else {

                    //filterExpression.text = prescriptionOrderID;
                    filterExpression.text = 'prescriptionOrderID = ' + prescriptionOrderID;

                    return true;
                }
            }
            else {
                errMessage.text = 'Transaksi harus dipropose terlebih dahulu sebelum proses bisa dilakukan.';
                return false;
            }

        }
        //#endregion

        

    </script>
    <input type="hidden" value="" id="hdnPrescriptionOrderID" runat="server" />
    <input type="hidden" value="" id="hdnDefaultVisitParamedicID" runat="server" />
    <input type="hidden" value="" id="hdnDefaultVisitParamedicCode" runat="server" />
    <input type="hidden" value="" id="hdnDefaultVisitParamedicName" runat="server" />
    <input type="hidden" value="" id="hdnGCTransactionStatus" runat="server" />
    <input type="hidden" value="0" id="hdnIsDrugChargesJustDistribution" runat="server" />
    <input type="hidden" value="" id="hdnDefaultGCMedicationRoute" runat="server" />
    <input type="hidden" value="" id="hdnChargeClassID" runat="server" />
    <input type="hidden" value="" id="hdnLocationID" runat="server" />
    <div>
        <table class="tblContentArea">
            <colgroup>
                <col style="width: 50%" />
                <col style="width: 50%" />
            </colgroup>
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 150px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblPrescriptionOrderNo">
                                    <%=GetLabel("No. Order")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPrescriptionOrderNo" Width="233px" ReadOnly="true" runat="server" />
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
                                <label class="lblMandatory">
                                    <%=GetLabel("Instalasi Farmasi") %></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboDispensaryUnit" ClientInstanceName="cboDispensaryUnit" runat="server"
                                    Width="235px">
                                    <ClientSideEvents ValueChanged="function() { cboLocation.PerformCallback(); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Jenis Permintaan") %></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboPrescriptionType" ClientInstanceName="cboPrescriptionType"
                                    runat="server" Width="235px">
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="padding: 5px; vertical-align: top">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 150px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink lblMandatory" runat="server" id="lblPhysician">
                                    <%=GetLabel("Dokter")%></label>
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
                            <td class="tdLabel" style="vertical-align: top">
                                <label class="lblNormal">
                                    <%=GetLabel("Catatan Order") %></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRemarks" Width="100%" runat="server" TextMode="MultiLine" Height="110px" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div id="containerEntry" style="margin-top: 4px; display: none;">
                        <div class="pageTitle">
                            <%=GetLabel("Data Permintaan Obat dan Alkes")%></div>
                        <fieldset id="fsTrx" style="margin: 0">
                            <input type="hidden" value="" id="hdnEntryID" runat="server" />
                            <table style="width: 100%" class="tblEntryDetail">
                                <colgroup>
                                    <col style="width: 50%" />
                                </colgroup>
                                <tr>
                                    <td valign="top">
                                        <table style="width: 100%">
                                            <colgroup>
                                                <col style="width: 150px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Lokasi Permintaan")%></label>
                                                </td>
                                                <td>
                                                    <dxe:ASPxComboBox runat="server" ID="cboLocation" ClientInstanceName="cboLocation"
                                                        Width="235px" OnCallback="cboLocation_Callback">
                                                        <ClientSideEvents BeginCallback="function() { showLoadingPanel(); }" EndCallback="function() { hideLoadingPanel(); }" />
                                                    </dxe:ASPxComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <table border="0" cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width: 40px" />
                                                            <col />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <asp:CheckBox runat="server" ID="chkIsRx" Text='Rx' Visible="false" />
                                                            </td>
                                                            <td class="tdLabel">
                                                                <label class="lblLink lblMandatory" id="lblDrug">
                                                                    <%=GetLabel("Obat")%></label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                    <input type="hidden" value="" id="hdnDrugID" runat="server" />
                                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width: 100px" />
                                                            <col style="width: 3px" />
                                                            <col />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtDrugCode" CssClass="required" Width="100%" runat="server" />
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtDrugName" ReadOnly="true" CssClass="required" Width="100%" runat="server" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Nama Generik")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtGenericName" Width="100%" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Bentuk")%></label>
                                                </td>
                                                <td>
                                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width: 105px" />
                                                            <col style="width: 105px" />
                                                            <col />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <dxe:ASPxComboBox runat="server" ID="cboForm" ClientInstanceName="cboForm" Width="105px" />
                                                            </td>
                                                            <td class="tdLabel" style="text-align: right; padding-right: 10px">
                                                                <label class="lblNormal">
                                                                    <%=GetLabel("Kadar")%></label>
                                                            </td>
                                                            <td>
                                                                <input type="hidden" value="" id="hdnGCDoseUnit" runat="server" />
                                                                <input type="hidden" value="" id="hdnGCItemUnit" runat="server" />
                                                                <table cellpadding="0" cellspacing="0">
                                                                    <colgroup>
                                                                        <col style="width: 100px" />
                                                                        <col style="width: 3px" />
                                                                    </colgroup>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:TextBox ID="txtStrengthAmount" runat="server" Width="100%" CssClass="number"
                                                                                ReadOnly="true" />
                                                                        </td>
                                                                        <td>
                                                                            &nbsp;
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox runat="server" ID="txtStrengthUnit" Width="100%" ReadOnly="true" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Purpose Of Medication")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPurposeOfMedication" Width="100%" runat="server" TextMode="MultiLine" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td valign="top">
                                        <table style="width: 100%">
                                            <colgroup>
                                                <col style="width: 170px" />
                                            </colgroup>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblNormal lblLink" id="lblSigna">
                                                        <%=GetLabel("Aturan Pakai")%></label>
                                                </td>
                                                <td>
                                                    <input type="hidden" value="" id="hdnSignaID" runat="server" />
                                                    <table width="100%" cellpadding="0px" cellspacing="0px">
                                                        <colgroup>
                                                            <col style="width: 110px" />
                                                            <col style="width: 3px" />
                                                            <col />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtSignaLabel" Width="100%" />
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtSignaName1" Width="100%" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("Frekuensi")%></label>
                                                </td>
                                                <td>
                                                    <table cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width: 110px" />
                                                            <col style="width: 3px" />
                                                            <col style="width: 116px" />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtFrequencyNumber" runat="server" Width="100%" CssClass="number" />
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <dxe:ASPxComboBox ID="cboFrequencyTimeline" ClientInstanceName="cboFrequencyTimeline"
                                                                    runat="server" Width="100%" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("Dosis")%></label>
                                                </td>
                                                <td>
                                                    <input type="hidden" id="hdnGCDosingUnit" runat="server" />
                                                    <table cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width: 110px" />
                                                            <col style="width: 3px" />
                                                            <col style="width: 116px" />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtDosingDose" runat="server" Width="100%" CssClass="number" />
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <dxe:ASPxComboBox ID="cboDosingUnit" ClientInstanceName="cboDosingUnit" runat="server"
                                                                    Width="100%" OnCallback="cboDosingUnit_Callback">
                                                                    <ClientSideEvents EndCallback="function() { onCboDosingUnitEndCallback() }" SelectedIndexChanged="function(s,e){ onCboDosingUnitValueChanged() }" />
                                                                </dxe:ASPxComboBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("Rute Obat")%></label>
                                                </td>
                                                <td>
                                                    <dxe:ASPxComboBox runat="server" ID="cboMedicationRoute" ClientInstanceName="cboMedicationRoute"
                                                        Width="228px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Coenam Rule")%></label>
                                                </td>
                                                <td>
                                                    <dxe:ASPxComboBox runat="server" ID="cboCoenamRule" ClientInstanceName="cboCoenamRule"
                                                        Width="229px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("Mulai Tanggal / Jam")%></label>
                                                </td>
                                                <td>
                                                    <table cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width: 140px" />
                                                            <col style="width: 5px" />
                                                            <col style="width: 80px" />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtStartDate" CssClass="datepicker" Width="110px" />
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtStartTime" CssClass="time" Width="100%" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("Durasi")%></label>
                                                </td>
                                                <td>
                                                    <table cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width: 110px" />
                                                            <col style="width: 3px" />
                                                            <col style="width: 110px" />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtDosingDuration" Width="100%" CssClass="number" />
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtDosingDurationTimeline" Width="100%" ReadOnly="true"
                                                                    runat="server" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("Quantity Resep")%></label>
                                                </td>
                                                <td>
                                                    <input type="hidden" value="" id="hdnTakenQty" runat="server" />
                                                    <input type="hidden" value="" id="hdnTakenUnit" runat="server" />
                                                    <table cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width: 110px" />
                                                            <col style="width: 3px" />
                                                            <col style="width: 110px" />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtDispenseQty" Width="100%" CssClass="number" max="10000" />
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtDispenseUnit" Width="100%" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Aturan Lainnya")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtMedicationAdministration" Width="450px" runat="server" TextMode="MultiLine" />
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
                            <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { hideLoadingPanel(); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height: 300px">
                                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false"
                                            ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                            <Columns>
                                                <asp:BoundField DataField="PrescriptionOrderDetailID" HeaderStyle-CssClass="keyField"
                                                    ItemStyle-CssClass="keyField" />
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20px">
                                                    <ItemTemplate>
                                                        <table cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td>
                                                                    <img class="imgEdit <%# IsEditable.ToString() == "False" ? "imgDisabled" : "imgLink"%>"
                                                                        title='<%=GetLabel("Edit")%>' src='<%# IsEditable.ToString() == "False" ? ResolveUrl("~/Libs/Images/Button/edit_disabled.png") : ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                        alt="" />
                                                                </td>
                                                                <td style="width: 1px">
                                                                    &nbsp;
                                                                </td>
                                                                <td>
                                                                    <img class="imgDelete <%# IsEditable.ToString() == "False" ? "imgDisabled" : "imgLink"%>"
                                                                        title='<%=GetLabel("Delete")%>' src='<%# IsEditable.ToString() == "False" ? ResolveUrl("~/Libs/Images/Button/delete_disabled.png") : ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                        alt="" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <input type="hidden" value="<%#:Eval("SignaID") %>" bindingfield="SignaID" />
                                                        <input type="hidden" value="<%#:Eval("SignaLabel") %>" bindingfield="SignaLabel" />
                                                        <input type="hidden" value="<%#:Eval("SignaName1") %>" bindingfield="SignaName1" />
                                                        <input type="hidden" value="<%#:Eval("PrescriptionOrderDetailID") %>" bindingfield="PrescriptionOrderDetailID" />
                                                        <input type="hidden" value="<%#:Eval("GenericName") %>" bindingfield="GenericName" />
                                                        <input type="hidden" value="<%#:Eval("IsRFlag") %>" bindingfield="IsRFlag" />
                                                        <input type="hidden" value="<%#:Eval("IsCompound") %>" bindingfield="IsCompound" />
                                                        <input type="hidden" value="<%#:Eval("ItemCode") %>" bindingfield="ItemCode" />
                                                        <input type="hidden" value="<%#:Eval("ItemID") %>" bindingfield="ItemID" />
                                                        <input type="hidden" value="<%#:Eval("DrugName") %>" bindingfield="DrugName" />
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
                                                        <input type="hidden" value="<%#:Eval("MedicationAdministration") %>" bindingfield="MedicationAdministration" />
                                                        <input type="hidden" value="<%#:Eval("StartDateInDatePickerFormat") %>" bindingfield="StartDateInDatePickerFormat" />
                                                        <input type="hidden" value="<%#:Eval("DispenseQty") %>" bindingfield="DispenseQty" />
                                                        <input type="hidden" value="<%#:Eval("GCItemUnit") %>" bindingfield="GCItemUnit" />
                                                        <input type="hidden" value="<%#:Eval("ItemUnit") %>" bindingfield="ItemUnit" />
                                                        <input type="hidden" value="<%#:Eval("TakenQty") %>" bindingfield="TakenQty" />
                                                        <input type="hidden" value="<%#:Eval("GCCoenamRule") %>" bindingfield="GCCoenamRule" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                    <HeaderTemplate>
                                                        <div>
                                                            <%=GetLabel("generik")%>
                                                            -
                                                            <%=GetLabel("Nama Obat")%>
                                                            -
                                                            <%=GetLabel("Kadar")%>
                                                            -
                                                            <%=GetLabel("Bentuk")%></div>
                                                        <div>
                                                            <div style="color: Blue; width: 35px; float: left;">
                                                                <%=GetLabel("DOSIS")%></div>
                                                            <%=GetLabel("dosis")%>
                                                            -
                                                            <%=GetLabel("Rute")%>
                                                            -
                                                            <%=GetLabel("Frekuensi")%></div>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <div>
                                                            <img src='<%# ResolveUrl("~/Libs/Images/Misc/compound.png")%>' alt="" style='<%# Eval("IsCompound").ToString() == "False" ? "display:none;": "" %> max-width:30px;
                                                                min-width: 30px; float: left;' />
                                                            <%#: Eval("InformationLine1")%></div>
                                                        <div>
                                                            <div style="color: Blue; width: 35px; float: left;">
                                                                <%=GetLabel("DOSIS")%></div>
                                                            <%#: Eval("NumberOfDosage")%>
                                                            <%#: Eval("DosingUnit")%>
                                                            -
                                                            <%#: Eval("Route")%>
                                                            -
                                                            <%#: Eval("SignaName1")%></div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <EmptyDataTemplate>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                        <div class="imgLoadingGrdView" id="Div1">
                                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                        </div>
                                        <div class="imgLoadingGrdView" id="containerImgLoadingView">
                                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                        </div>
                                        <div style='width: 100%; text-align: center;'>
                                            <span class="lblLink" style="margin-right: 300px; <%=IsEditable.ToString() == "False" ? "display:none": "" %>"
                                                id="lblAddData">
                                                <%= GetLabel("Tambah Data")%></span> <span class="lblLink" id="lblTestOrderQuickPick"
                                                    <%=IsEditable.ToString() == "False" ? "style='display:none'" : "style='margin-right: 300px'" %>>
                                                    <%= GetLabel("Quick Picks")%></span> <span class="lblLink" style="<%=IsEditable.ToString() == "False" ? "display:none": "" %>"
                                                        id="lblQuickPickHistoryResep">
                                                        <%= GetLabel("Quick Picks History Resep")%></span>
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
    <dxcp:ASPxCallbackPanel ID="cbpProcess" runat="server" Width="100%" ClientInstanceName="cbpProcess"
        ShowLoadingPanel="false" OnCallback="cbpProcess_Callback">
        <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpProcesEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</asp:Content>
