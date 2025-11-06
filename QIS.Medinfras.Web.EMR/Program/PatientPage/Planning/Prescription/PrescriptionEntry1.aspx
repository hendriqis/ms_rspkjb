<%@ Page Language="C#" MasterPageFile="~/MasterPage/MPPatientPageListEntry2.master" EnableEventValidation="false"
    AutoEventWireup="true" CodeBehind="PrescriptionEntry1.aspx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.PrescriptionEntry1" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnBackPrescriptionList" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/back.png")%>' alt="" /><div>
            <%=GetLabel("Back to List")%></div>
    </li>
    <li id="btnSendOrder" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbProcess.png")%>' alt="" /><div>
            <%=GetLabel("Send Order")%></div>
    </li>
    <li id="btnSaveTemplate" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbSave.png")%>' alt="" /><div>
            <%=GetLabel("Save As Template")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhRightButtonToolbar" runat="server">
    <li id="btnQuickPicksEntry" runat="server" crudmode="C">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbitems.png")%>' alt="" /><div>
            <%=GetLabel("Quick Picks")%></div>
    </li>
    <li id="btnQuickPicksHistoryEntry" runat="server" crudmode="C" visible="true">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbitems.png")%>' alt="" /><div>
            <%=GetLabel("Quick Picks History")%></div>
    </li>
    <li id="btnQuickPicksTemplate" runat="server" crudmode="C" visible="true">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbitems.png")%>' alt="" /><div>
            <%=GetLabel("Quick Picks Template")%></div>
    </li>
    <li id="btnPrescriptionCompoundEntry" runat="server" crudmode="C">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbcompound.png")%>' alt="" /><div>
            <%=GetLabel("Compound")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhScript" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#<%=grdView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
            });
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        });

        function onRefreshControl(filterExpression) {
            if ($('#<%=hdnPrescriptionOrderID.ClientID %>').val() == '') {
                $('#<%=hdnPrescriptionOrderID.ClientID %>').val($('#hdnEntryOrderID').val());
                cboParamedicID.SetEnabled(false);
                cboRefillInstruction.SetEnabled(false);
                cboDispensaryUnit.SetEnabled(false);
                cboPrescriptionType.SetEnabled(false);
                cboLocation.SetEnabled(false);
            }
            $('#<%=hdnFilterExpression.ClientID %>').val(filterExpression);
            cbpView.PerformCallback('refresh');
        }

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
        });

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
            $('#containerImgLoadingView').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdView.ClientID %> tr:eq(1)').click();

                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });

                $('#<%=txtPrescriptionNo.ClientID %>').val(param[2]);
            }
            else
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        function onLedDrugNameLostFocus(led) {
            var drugID = led.GetValueText();
            if (drugID != '') {
                $('#<%=hdnDrugID.ClientID %>').val(drugID);
                $('#<%=hdnDrugName.ClientID %>').val(led.GetDisplayText());
                var filterExpression = "ItemID = " + drugID;
                Methods.getObject('GetvDrugInfoList', filterExpression, function (result) {
                    $('#<%=txtGenericName.ClientID %>').val(result.GenericName);
                    $('#<%=txtStrengthAmount.ClientID %>').val(result.Dose);
                    $('#<%=txtStrengthUnit.ClientID %>').val(result.GCDoseUnit.substring(5));
                    $('#<%=hdnStrengthAmount.ClientID %>').val(result.Dose);
                    $('#<%=hdnStrengthUnit.ClientID %>').val(result.GCDoseUnit);
                    $('#<%=txtDosingDose.ClientID %>').val('1');
                    cboMedicationRoute.SetValue(result.GCMedicationRoute);
                    cboDosingUnit.SetValue(result.GCItemUnit);
                    $('#<%=hdnGCBaseUnit.ClientID %>').val(result.GCBaseUnit);
                    $('#<%=txtDispenseUnit.ClientID %>').val(result.ItemUnit);
                    cboDrugForm.SetValue(result.GCDrugForm);
                    $('#<%=txtFrequencyNumber.ClientID %>').focus();
                    calculateDispenseQty();
                });
            }
        }

        function onSelectedRowChanged(value) {
            var idx = $('#<%=grdView.ClientID %> tr').index($('#<%=grdView.ClientID %> tr.selected'));
            idx += value;
            if (idx < 1)
                idx = 1;
            if (idx == $('#<%=grdView.ClientID %> tr').length)
                idx--;
            $('#<%=grdView.ClientID %> tr:eq(' + idx + ')').click();
        }

        function getSelectedRow() {
            return $('#<%=grdView.ClientID %> tr.selected');
        }

        function onButtonCancelClick() {
            $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
        }

        //#region Entity To Control
        function entityToControl(entity) {
            $('#<%=chkIsRx.ClientID %>').focus();
            $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
            if (entity != null) {
                $('#<%=grdView.ClientID %> tr.selected').addClass('selected');
                $('#<%=hdnEntryID.ClientID %>').val(entity.PrescriptionOrderDetailID);
                $('#<%=chkIsRx.ClientID %>').prop('checked', (entity.IsRFlag == 'True'));
                $('#<%=txtGenericName.ClientID %>').val(entity.GenericName);
                $('#<%=hdnDrugID.ClientID %>').val(entity.ItemID);
                $('#<%=hdnDrugName.ClientID %>').val(entity.DrugName);
                cboDrugForm.SetValue(entity.GCDrugForm);
                $('#<%=txtPurposeOfMedication.ClientID %>').val(entity.MedicationPurpose);
                $('#<%=txtStrengthAmount.ClientID %>').val(entity.Dose);
                $('#<%=txtStrengthUnit.ClientID %>').val(entity.DoseUnit);
                $('#<%=hdnStrengthAmount.ClientID %>').val(entity.Dose);
                $('#<%=hdnStrengthUnit.ClientID %>').val(entity.GCDoseUnit);
                cboFrequencyTimeline.SetValue(entity.GCDosingFrequency);
                $('#<%=txtFrequencyNumber.ClientID %>').val(entity.Frequency);
                $('#<%=txtDosingDose.ClientID %>').val(entity.NumberOfDosage);
                $('#<%=txtDosingDuration.ClientID %>').val(entity.DosingDuration);
                cboDosingUnit.SetValue(entity.GCDosingUnit);
                cboCoenamRule.SetValue(entity.GCCoenamRule);
                cboMedicationRoute.SetValue(entity.GCRoute);
                $('#<%=txtStartDate.ClientID %>').val(entity.StartDate);
                $('#<%=txtStartTime.ClientID %>').val(entity.StartTime);
                $('#<%=txtMedicationAdministration.ClientID %>').val(entity.MedicationAdministration);
                $('#<%=txtDispenseQty.ClientID %>').val(entity.DispenseQtyInString);
                $('#<%=txtDispenseUnit.ClientID %>').val(entity.ItemUnit);
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

                $('#<%=hdnIsAllergyAlert.ClientID %>').val(entity.IsAllergyAlert);
                $('#<%=hdnIsAdverseReactionAlert.ClientID %>').val(entity.IsAdverseReactionAlert);
                $('#<%=hdnIsDuplicateTheraphyAlert.ClientID %>').val(entity.IsDuplicateTheraphyAlert);

                if (entity.Frequency > 0) {
                    if (entity.Sequence1Time == "" || entity.Sequence1Time == "-") {
                        SetMedicationDefaultTime(entity.Frequency);
                    }
                    else {
                        $('#<%=txtStartTime1.ClientID %>').val(entity.Sequence1Time);
                        $('#<%=txtStartTime2.ClientID %>').val(entity.Sequence2Time);
                        $('#<%=txtStartTime3.ClientID %>').val(entity.Sequence3Time);
                        $('#<%=txtStartTime4.ClientID %>').val(entity.Sequence4Time);
                        $('#<%=txtStartTime5.ClientID %>').val(entity.Sequence5Time);
                        $('#<%=txtStartTime6.ClientID %>').val(entity.Sequence6Time);
                    }
                }
                else {
                    $('#<%=txtStartTime.ClientID %>').val(entity.StartTime);
                    $('#<%=txtStartTime1.ClientID %>').val(entity.StartTime);
                    $('#<%=txtStartTime2.ClientID %>').val("-");
                    $('#<%=txtStartTime3.ClientID %>').val("-");
                    $('#<%=txtStartTime4.ClientID %>').val("-");
                    $('#<%=txtStartTime5.ClientID %>').val("-");
                    $('#<%=txtStartTime6.ClientID %>').val("-");
                }

                $('#<%=txtSignaName1.ClientID %>').val(entity.SignaName1);
                $('#<%=txtSignaLabel.ClientID %>').val(entity.SignaLabel);
                $('#<%=hdnSignaID.ClientID %>').val(entity.SignaID);

                var filterExpression = '';
                if ($('#<%=hdnPrescriptionOrderID.ClientID %>').val() != '')
                    filterExpression = $('#<%=hdnFilterExpressionItemEdit.ClientID %>').val().replace('{ItemID}', entity.ItemID);
                else
                    filterExpression = $('#<%=hdnFilterExpressionItemNewTransHd.ClientID %>').val();

                if (filterExpression != "") {
                    filterExpression += " AND ";
                }

                if ($('#<%=hdnIsBPJS.ClientID %>').val() == '1') {
                    if ($('#<%=hdnIsLimitedCPOEItemForBPJS.ClientID %>').val() == '1') {
                        filterExpression += " IsBPJSFormularium = 1 AND ";
                    }
                }

                filterExpression += $('#<%=hdnFilterExpressionItem.ClientID %>').val();

                ledDrugName.SetFilterExpression(filterExpression);
                ledDrugName.SetValue(entity.ItemID);

            }
            else {
                $('#<%=hdnEntryID.ClientID %>').val('');
                $('#<%=chkIsRx.ClientID %>').prop('checked', true);
                $('#<%=txtGenericName.ClientID %>').val('');
                $('#<%=hdnDrugID.ClientID %>').val('');
                $('#<%=hdnDrugName.ClientID %>').val('');
                cboDrugForm.SetValue('');
                $('#<%=txtPurposeOfMedication.ClientID %>').val('');
                $('#<%=txtStrengthAmount.ClientID %>').val('');
                $('#<%=txtStrengthUnit.ClientID %>').val('');
                $('#<%=hdnStrengthAmount.ClientID %>').val('');
                $('#<%=hdnStrengthUnit.ClientID %>').val('');
                cboFrequencyTimeline.SetValue(Constant.DosingFrequency.DAY);
                $('#<%=txtFrequencyNumber.ClientID %>').val('1');
                $('#<%=txtDosingDose.ClientID %>').val('1');
                $('#<%=txtDosingDuration.ClientID %>').val('1');
                cboDosingUnit.SetValue('');
                cboCoenamRule.SetValue('');
                cboMedicationRoute.SetValue('');
                //$('#<%=txtStartDate.ClientID %>').val('');
                //$('#<%=txtStartTime.ClientID %>').val(entity.StartTime);
                $('#<%=txtMedicationAdministration.ClientID %>').val('');
                $('#<%=txtDispenseQty.ClientID %>').val('0');
                $('#<%=txtDispenseUnit.ClientID %>').val('');
                $('#<%=chkIsMorning.ClientID %>').prop('checked', false);
                $('#<%=chkIsNoon.ClientID %>').prop('checked', false);
                $('#<%=chkIsEvening.ClientID %>').prop('checked', false);
                $('#<%=chkIsNight.ClientID %>').prop('checked', false);
                $('#<%=chkIsAsRequired.ClientID %>').prop('checked', false);
                $('#<%=chkIsIMM.ClientID %>').prop('checked', false);
                $('#<%=hdnIsAllergyAlert.ClientID %>').val('0');
                $('#<%=hdnIsAdverseReactionAlert.ClientID %>').val('0');
                $('#<%=hdnIsDuplicateTheraphyAlert.ClientID %>').val('0');


                $('#<%=txtSignaName1.ClientID %>').val('');
                $('#<%=txtSignaLabel.ClientID %>').val('');
                $('#<%=hdnSignaID.ClientID %>').val('');

                var filterExpression = '';
                if ($('#<%=hdnPrescriptionOrderID.ClientID %>').val() != '')
                    filterExpression = $('#<%=hdnFilterExpressionItemAdd.ClientID %>').val();
                else
                    filterExpression = $('#<%=hdnFilterExpressionItemNewTransHd.ClientID %>').val();

                if (filterExpression != "") {
                    filterExpression += " AND ";
                }

                if ($('#<%=hdnIsBPJS.ClientID %>').val() == '1') {
                    if ($('#<%=hdnIsLimitedCPOEItemForBPJS.ClientID %>').val() == '1') {
                        filterExpression += " IsBPJSFormularium = 1 AND ";
                    }
                }

                filterExpression += $('#<%=hdnFilterExpressionItem.ClientID %>').val();

                ledDrugName.SetFilterExpression(filterExpression);
                ledDrugName.SetValue('');
            }
        }

        function setControlValue(entity) {
            $('#<%=chkIsRx.ClientID %>').focus();
            if (entity != null) {
                $('#<%=hdnEntryID.ClientID %>').val(entity.PrescriptionOrderDetailID);
                $('#<%=chkIsRx.ClientID %>').prop('checked', (entity.IsRFlag == 'True'));
                $('#<%=txtGenericName.ClientID %>').val(entity.GenericName);
                $('#<%=hdnDrugID.ClientID %>').val(entity.ItemID);
                $('#<%=hdnDrugName.ClientID %>').val(entity.DrugName);
                cboDrugForm.SetValue(entity.GCDrugForm);
                $('#<%=txtPurposeOfMedication.ClientID %>').val(entity.MedicationPurpose);
                $('#<%=txtStrengthAmount.ClientID %>').val(entity.Dose);
                $('#<%=txtStrengthUnit.ClientID %>').val(entity.DoseUnit);
                $('#<%=hdnStrengthAmount.ClientID %>').val(entity.Dose);
                $('#<%=hdnStrengthUnit.ClientID %>').val(entity.GCDoseUnit);
                cboFrequencyTimeline.SetValue(entity.GCDosingFrequency);
                $('#<%=txtFrequencyNumber.ClientID %>').val(entity.Frequency);
                $('#<%=txtDosingDose.ClientID %>').val(entity.NumberOfDosage);
                $('#<%=txtDosingDuration.ClientID %>').val(entity.DosingDuration);
                cboDosingUnit.SetValue(entity.GCDosingUnit);
                cboCoenamRule.SetValue(entity.GCCoenamRule);
                cboMedicationRoute.SetValue(entity.GCRoute);
                $('#<%=txtStartDate.ClientID %>').val(entity.StartDate);
                $('#<%=txtStartTime.ClientID %>').val(entity.StartTime);
                $('#<%=txtMedicationAdministration.ClientID %>').val(entity.MedicationAdministration);
                $('#<%=txtDispenseQty.ClientID %>').val(entity.DispenseQtyInString);
                ledDrugName.SetValue(entity.ItemID);
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

                $('#<%=hdnIsAllergyAlert.ClientID %>').val(entity.IsAllergyAlert);
                $('#<%=hdnIsAdverseReactionAlert.ClientID %>').val(entity.IsAdverseReactionAlert);
                $('#<%=hdnIsDuplicateTheraphyAlert.ClientID %>').val(entity.IsDuplicateTheraphyAlert);
            }
            else {
                $('#<%=hdnEntryID.ClientID %>').val('');
                $('#<%=chkIsRx.ClientID %>').prop('checked', true);
                $('#<%=txtGenericName.ClientID %>').val('');
                $('#<%=hdnDrugID.ClientID %>').val('');
                $('#<%=hdnDrugName.ClientID %>').val('');
                cboDrugForm.SetValue('');
                $('#<%=txtPurposeOfMedication.ClientID %>').val('');
                $('#<%=txtStrengthAmount.ClientID %>').val('');
                $('#<%=txtStrengthUnit.ClientID %>').val('');
                $('#<%=hdnStrengthAmount.ClientID %>').val('');
                $('#<%=hdnStrengthUnit.ClientID %>').val('');
                cboFrequencyTimeline.SetValue(Constant.DosingFrequency.DAY);
                $('#<%=txtFrequencyNumber.ClientID %>').val('1');
                $('#<%=txtDosingDose.ClientID %>').val('1');
                $('#<%=txtDosingDuration.ClientID %>').val('1');
                cboDosingUnit.SetValue('');
                cboCoenamRule.SetValue('');
                cboMedicationRoute.SetValue('');
                //$('#<%=txtStartDate.ClientID %>').val('');
                //$('#<%=txtStartTime.ClientID %>').val(entity.StartTime);
                $('#<%=txtMedicationAdministration.ClientID %>').val('');
                $('#<%=txtDispenseQty.ClientID %>').val('0');
                $('#<%=chkIsMorning.ClientID %>').prop('checked', false);
                $('#<%=chkIsNoon.ClientID %>').prop('checked', false);
                $('#<%=chkIsEvening.ClientID %>').prop('checked', false);
                $('#<%=chkIsNight.ClientID %>').prop('checked', false);
                $('#<%=chkIsAsRequired.ClientID %>').prop('checked', false);
                $('#<%=chkIsIMM.ClientID %>').prop('checked', false);
                $('#<%=hdnIsAllergyAlert.ClientID %>').val('0');
                $('#<%=hdnIsAdverseReactionAlert.ClientID %>').val('0');
                $('#<%=hdnIsDuplicateTheraphyAlert.ClientID %>').val('0');
                ledDrugName.SetValue('');
            }
        }
        //#endregion

        $(function () {
            $('#<%=btnBackPrescriptionList.ClientID %>').click(function () {
                if (($('#<%:hdnPrescriptionOrderID.ClientID %>').val() != "") && $('#<%:hdnIsProposed.ClientID %>').val() == "0") {
                    PromptUserToSave();
                }
                else {
                    showLoadingPanel();
                    document.location = document.referrer;
                }
            });

            $('#<%=btnPrescriptionCompoundEntry.ClientID %>').click(function () {
                var prescriptionOrderID = $('#<%=hdnPrescriptionOrderID.ClientID %>').val();
                var date = Methods.dateToString(Methods.getDatePickerDate($('#<%=txtPrescriptionDate.ClientID %>').val()));
                var time = $('#<%=txtPrescriptionTime.ClientID %>').val();
                var physician = cboParamedicID.GetValue();
                var prescriptionType = cboPrescriptionType.GetValue();
                var dispensaryServiceUnitID = cboDispensaryUnit.GetValue();
                var refillInstruction = cboRefillInstruction.GetValue();
                var location = cboLocation.GetValue();

                var queryString = prescriptionOrderID + '|' + date + '|' + time + '|' + physician + '|' + prescriptionType + '|' + dispensaryServiceUnitID + '|' + location + '|' + refillInstruction;

                var url = ResolveUrl("~/Program/PatientPage/Planning/Prescription/PrescriptionCompoundEntryCtl.ascx");
                openUserControlPopup(url, queryString, 'Compound Prescription', 900, 650);
            });

            $('#<%=btnQuickPicksHistoryEntry.ClientID %>').live('click', function (evt) {
                if (IsValid(null, 'fsTrx', 'mpTrx')) {
                    if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                        showLoadingPanel();
                        var url = ResolveUrl('~/Program/PatientPage/_PopupEntry/CPOE/MedicationQuickPicksHistoryCtl1.ascx');
                        var orderID = $('#<%=hdnPrescriptionOrderID.ClientID %>').val();
                        var dispensaryServiceUnitID = cboDispensaryUnit.GetValue();
                        var locationID = cboLocation.GetValue();
                        var defaultGCMedicationRoute = $('#<%=hdnDefaultGCMedicationRoute.ClientID %>').val();
                        var paramedicID = $('#<%=hdnPhysicianID.ClientID %>').val();
                        var registration = $('#<%=hdnRegistrationID.ClientID %>').val();
                        var visitID = $('#<%=hdnVisitID.ClientID %>').val();
                        var chargeClassID = $('#<%=hdnChargeClassID.ClientID %>').val();
                        var orderNotes = $('#<%=txtRemarks.ClientID %>').val();
                        var dispensaryUnitID = $('#<%=hdnDefaultDispensaryServiceUnitID.ClientID %>').val();
                        var refillInstruction = cboRefillInstruction.GetValue();

                        var prescriptionType = cboPrescriptionType.GetValue();
                        var date = Methods.dateToString(Methods.getDatePickerDate($('#<%=txtPrescriptionDate.ClientID %>').val()));
                        var time = $('#<%=txtPrescriptionTime.ClientID %>').val();
                        var id = orderID + '|' + locationID + '|' + defaultGCMedicationRoute + '|' + paramedicID + '|' + registration + '|' + visitID + '|' + chargeClassID + '|' + orderNotes + '|' + dispensaryServiceUnitID + '|' + date + '|' + time + '|' + prescriptionType;
                        openUserControlPopup(url, id, 'Quick Picks', 1200, 600);
                    }
                }
            });

            $('#<%=btnQuickPicksEntry.ClientID %>').live('click', function (evt) {
                if (IsValid(null, 'fsTrx', 'mpTrx')) {
                    if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                        showLoadingPanel();
                        var url = ResolveUrl('~/Program/PatientPage/_PopupEntry/CPOE/MedicationQuickPicksCtl1.ascx');
                        var orderID = $('#<%=hdnPrescriptionOrderID.ClientID %>').val();
                        var dispensaryServiceUnitID = cboDispensaryUnit.GetValue();
                        var locationID = cboLocation.GetValue();
                        var defaultGCMedicationRoute = $('#<%=hdnDefaultGCMedicationRoute.ClientID %>').val();
                        var paramedicID = $('#<%=hdnPhysicianID.ClientID %>').val();
                        var registration = $('#<%=hdnRegistrationID.ClientID %>').val();
                        var visitID = $('#<%=hdnVisitID.ClientID %>').val();
                        var chargeClassID = $('#<%=hdnChargeClassID.ClientID %>').val();
                        var orderNotes = $('#<%=txtRemarks.ClientID %>').val();
                        var dispensaryUnitID = $('#<%=hdnDefaultDispensaryServiceUnitID.ClientID %>').val();
                        var refillInstruction = cboRefillInstruction.GetValue();

                        var prescriptionType = cboPrescriptionType.GetValue();
                        var date = Methods.dateToString(Methods.getDatePickerDate($('#<%=txtPrescriptionDate.ClientID %>').val()));
                        var time = $('#<%=txtPrescriptionTime.ClientID %>').val();
                        var id = orderID + '|' + locationID + '|' + defaultGCMedicationRoute + '|' + paramedicID + '|' + registration + '|' + visitID + '|' + chargeClassID + '|' + orderNotes + '|' + dispensaryServiceUnitID + '|' + date + '|' + time + '|' + prescriptionType;
                        openUserControlPopup(url, id, 'Quick Picks', 1300, 600);
                    }
                }
            });

            $('#<%=btnQuickPicksTemplate.ClientID %>').live('click', function (evt) {
                if (IsValid(null, 'fsTrx', 'mpTrx')) {
                    if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                        showLoadingPanel();
                        var url = ResolveUrl('~/Program/PatientPage/_PopupEntry/CPOE/MedicationQuickPicksTemplateCtl1.ascx');
                        var orderID = $('#<%=hdnPrescriptionOrderID.ClientID %>').val();
                        var dispensaryServiceUnitID = cboDispensaryUnit.GetValue();
                        var locationID = cboLocation.GetValue();
                        var defaultGCMedicationRoute = $('#<%=hdnDefaultGCMedicationRoute.ClientID %>').val();
                        var paramedicID = $('#<%=hdnPhysicianID.ClientID %>').val();
                        var registration = $('#<%=hdnRegistrationID.ClientID %>').val();
                        var visitID = $('#<%=hdnVisitID.ClientID %>').val();
                        var chargeClassID = $('#<%=hdnChargeClassID.ClientID %>').val();
                        var orderNotes = $('#<%=txtRemarks.ClientID %>').val();
                        var dispensaryUnitID = $('#<%=hdnDefaultDispensaryServiceUnitID.ClientID %>').val();
                        var refillInstruction = cboRefillInstruction.GetValue();

                        var prescriptionType = cboPrescriptionType.GetValue();
                        var date = Methods.dateToString(Methods.getDatePickerDate($('#<%=txtPrescriptionDate.ClientID %>').val()));
                        var time = $('#<%=txtPrescriptionTime.ClientID %>').val();
                        var id = orderID + '|' + locationID + '|' + defaultGCMedicationRoute + '|' + paramedicID + '|' + registration + '|' + visitID + '|' + chargeClassID + '|' + orderNotes + '|' + dispensaryServiceUnitID + '|' + date + '|' + time + '|' + prescriptionType;
                        openUserControlPopup(url, id, 'Quick Picks : Template', 1200, 600);
                    }
                }
            });

            $('#<%=btnSendOrder.ClientID %>').click(function () {
                if ($('#<%:hdnPrescriptionOrderID.ClientID %>').val() == "") {
                    var messageBody = "Tidak ada order yang bisa dikirim ke farmasi.";
                    displayErrorMessageBox('ERROR : SEND ORDER', messageBody);
                }
                else {
                    var isUsingDrugAlert = $('#<%:hdnIsUsingDrugAlert.ClientID %>').val();
                    if (isUsingDrugAlert == "1") {
                        var presNo = $('#<%=txtPrescriptionNo.ClientID %>').val();

                        DrugAlertService.validateDrugs(presNo, 'Validate', function (resultF1) {
                            var url = ResolveUrl("~/Libs/Program/Information/DrugAlertInformationCtl.ascx");
                            var id = $('#<%:hdnPrescriptionOrderID.ClientID %>').val();

                            var isShowAlertForm = 0;
                            var filterExpressionPresInfo = "PrescriptionOrderID = '" + id + "'";
                            var toFound = "No results found. Absence of interaction result should in no way be interpreted as safe. Clinical judgment should be exercised";
                            Methods.getObject('GetPrescriptionOrderHdInfoList', filterExpressionPresInfo, function (resultInfo) {
                                if (resultInfo != null) {
                                    if (!resultInfo.DrugAlertResultInfo1.includes(toFound)) {
                                        isShowAlertForm = 1;
                                    }
                                    else {
                                        isShowAlertForm = 0;
                                    }
                                }
                                else {
                                    isShowAlertForm = 0;
                                }
                            });

                            if (isShowAlertForm == 1) {
                                openUserControlPopup(url, id, 'Drug Alert Information', 700, 600);
                            }
                            else {
                                cbpSendOrder.PerformCallback('sendOrder');
                            }                        
                        });
                    }
                    else {
                        displayConfirmationMessageBox('SEND ORDER : FARMASI', 'Kirim order resep ke farmasi ?', function (result) {
                            if (result) cbpSendOrder.PerformCallback('sendOrder');
                        });
                    }
                }
            });

            $('#<%=btnSaveTemplate.ClientID %>').click(function () {
                if ($('#<%:hdnPrescriptionOrderID.ClientID %>').val() == "") {
                    var messageBody = "Tidak ada transaksi resep yang bisa disimpan sebagai Template";
                    displayErrorMessageBox('SAVE TEMPLATE', messageBody);
                }
                else {
                    displayConfirmationMessageBox('SAVE TEMPLATE', 'Simpan sebagai Template Peresepan ?', function (result) {
                        if (result) {
                            var param = $('#<%=hdnPrescriptionOrderID.ClientID %>').val();
                            openUserControlPopup("~/Program/PatientPage/_PopupEntry/Template/PrescriptionTemplateCtl1.ascx", param, "Template Resep", 700, 200);
                        }
                    });
                }
            });

            setDatePicker('<%=txtStartDate.ClientID %>');

            function GetCurrentSelectedItem(s) {
                var $tr = $(s).closest('tr').parent().closest('tr');
                var idx = $('#<%=grdView.ClientID %> tr').index($tr);
                $('#<%=grdView.ClientID %> tr:eq(' + idx + ')').click();

                $row = $('#<%=grdView.ClientID %> tr.selected');
                var selectedObj = {};

                $row.find('input[type=hidden]').each(function () {
                    selectedObj[$(this).attr('bindingfield')] = $(this).val();
                });

                return selectedObj;
            }

            function SetEntityToControl(param) {
                var selectedObj = {};
                selectedObj = GetCurrentSelectedItem(param);
            }

            $('.imgEdit.imgLink').die('click');
            $('.imgEdit.imgLink').live('click', function () {
                SetEntityToControl(this);
                onPatientListEntryEditRecord();
            });

            $('.imgDelete.imgLink').die('click');
            $('.imgDelete.imgLink').live('click', function () {
                onPatientListEntryDeleteRecord();
            });
        });

        function onBeforeChangePage() {
            if (($('#<%:hdnPrescriptionOrderID.ClientID %>').val() != "") && $('#<%:hdnIsProposed.ClientID %>').val() == "0") {
                PromptUserToSave();
            }
            else {
                gotoNextPage();
            }
        }

        function onBeforeBackToListPage() {
            if (($('#<%:hdnPrescriptionOrderID.ClientID %>').val() != "") && $('#<%:hdnIsProposed.ClientID %>').val() == "0") {
                PromptUserToSave();
            }
            else {
                backToPatientList();
            }
        }

        function PromptUserToSave() {
            var isUsingDrugAlert = $('#<%:hdnIsUsingDrugAlert.ClientID %>').val();
            displayConfirmationMessageBox('SEND ORDER : FARMASI', 'Kirim order resep ke farmasi ?', function (result) {
                if (result) {
                    if (isUsingDrugAlert == "1") {
                        var presNo = $('#<%=txtPrescriptionNo.ClientID %>').val();

                        DrugAlertService.validateDrugs(presNo, 'Validate', function (resultF1) {
                            var url = ResolveUrl("~/Libs/Program/Information/DrugAlertInformationCtl.ascx");
                            var id = $('#<%:hdnPrescriptionOrderID.ClientID %>').val();

                            var isShowAlertForm = 0;
                            var filterExpressionPresInfo = "PrescriptionOrderID = '" + id + "'";
                            var toFound = "No results found. Absence of interaction result should in no way be interpreted as safe. Clinical judgment should be exercised";
                            Methods.getObject('GetPrescriptionOrderHdInfoList', filterExpressionPresInfo, function (resultInfo) {
                                if (resultInfo != null) {
                                    if (!resultInfo.DrugAlertResultInfo1.includes(toFound)) {
                                        isShowAlertForm = 1;
                                    }
                                    else {
                                        isShowAlertForm = 0;
                                    }
                                }
                                else {
                                    isShowAlertForm = 0;
                                }
                            });

                            if (isShowAlertForm == 1) {
                                openUserControlPopup(url, id, 'Drug Alert Information', 700, 600);
                            }
                            else {
                                cbpSendOrder.PerformCallback('sendOrder');
                            }                        
                         });
                    }
                    else {
                        cbpSendOrder.PerformCallback('sendOrder');                    
                    }
                } else {
                    cbpSendOrder.PerformCallback('saveHeader');
                }
            });
        }

        function onBeforeEditRecordIsUsingCustomEdit(entity) {
            return (entity.IsCompound == 'True')
        }

        function SaveFromDrugAlertPopUp() {
            cbpSendOrder.PerformCallback('sendOrder'); 
        }

        function onCustomEditRecord(entity) {
            var prescriptionOrderID = $('#<%=hdnPrescriptionOrderID.ClientID %>').val();
            var queryString = prescriptionOrderID + '|' + entity.PrescriptionOrderDetailID + '|' + entity.LocationID;

            var url = ResolveUrl("~/Program/PatientPage/Planning/Prescription/PrescriptionCompoundEntryCtl.ascx");
            openUserControlPopup(url, queryString, 'Prescription Compound', 900, 650);
        }

        function onAfterSaveRecord(param) {
            onAfterSaveDetail(param);
        }

        function onAfterSaveRecordPatientPageEntry(param) {
            onAfterSaveDetail(param);
        }

        function onAfterSaveDetail(param) {
            var paramInfo = param.split('|');
            if ($('#<%=hdnPrescriptionOrderID.ClientID %>').val() == '') {
                $('#<%=hdnFilterExpressionItemAdd.ClientID %>').val($('#<%=hdnFilterExpressionItemAdd.ClientID %>').val().replace('{PrescriptionOrderID}', paramInfo[0]));
                $('#<%=hdnFilterExpressionItemEdit.ClientID %>').val($('#<%=hdnFilterExpressionItemEdit.ClientID %>').val().replace('{PrescriptionOrderID}', paramInfo[0]));
                $('#<%=hdnPrescriptionOrderID.ClientID %>').val(param);
                var filterExpression = $('#<%=hdnFilterExpressionItem.ClientID %>').val();
                txtQuickEntry.SetFilterExpression(0, filterExpression);
                ledDrugName.SetFilterExpression(filterExpression);
                var filterExpressionPrescription = 'PrescriptionOrderID = ' + param;
                Methods.getObject('GetPrescriptionOrderHdList', filterExpressionPrescription, function (result) {
                    $('#<%=txtPrescriptionNo.ClientID %>').val(result.PrescriptionOrderNo);
                    $('#<%=txtRemarks.ClientID %>').val(result.Remarks);
                });
                cboParamedicID.SetEnabled(false);
                cboRefillInstruction.SetEnabled(false);
                cboDispensaryUnit.SetEnabled(false);
                cboPrescriptionType.SetEnabled(false);
                cboLocation.SetEnabled(false);
                hideLoadingPanel();
            }
            cbpView.PerformCallback('refresh');
        }

        function onAfterUpdateOrderStatus(param) {
            if (param != '') {
                location.reload(true);
//                var filterExpressionPrescription = 'PrescriptionOrderID = ' + param;
//                Methods.getObject('GetPrescriptionOrderHdList', filterExpressionPrescription, function (result) {
//                    cboPrescriptionType.SetValue(result.GCPrescriptionType);
//                    $('#<%=txtRemarks.ClientID %>').val(result.Remarks);
//                });
//                hideLoadingPanel();
            }
        }

        function onBeforeSaveRecord(errMessage) {
            var ledValidate = ledDrugName.Validate();
            var result = ledValidate;
            var isValidate = $('#<%=hdnValidationEmptyStock.ClientID %>').val();
            if (ledValidate) {
                if (isValidate == '0') {
                   
                    var locationID = cboLocation.GetValue();
                    var itemID = $('#<%=hdnDrugID.ClientID %>').val();
                    var itemname = '';

                    var filterItem = "ItemID = '" + itemID + "'";
                    Methods.getObject("GetItemMasterList", filterItem, function (resultItem) {
                        if (resultItem != null) {
                            itemname = resultItem.ItemName1;
                        }
                    });

                    var filterBalance = "ItemID = '" + itemID + "' AND LocationID = '" + locationID + "' AND IsDeleted = 0";
                    Methods.getObject("GetItemBalanceList", filterBalance, function (resultBalance) {
                        if (resultBalance != null) {
                            if (resultBalance.QuantityEND <= 0) {
                                errMessage.text = 'showconfirm|Stok untuk item ' + itemname + ' tidak mencukupi, Lanjutkan proses ?';
                                result = false;
                            }
                        }
                        else {
                            errMessage.text = 'showconfirm|Stok untuk item ' + itemname + ' tidak mencukupi, Lanjutkan proses ?';
                            result = false;
                        }
                    });
                    return result;
                }
                else {
                    return true;
                }
            }
            else {
                errMessage.text = "Silahkan diisi dahulu nama obat (Rx Drug Name)";
                return ledDrugName.Validate();
            }
        }

        function onTxtQuickEntrySearchClick(s) {
            var ledValidate = ledDrugName.Validate();
            var isValidate = $('#<%=hdnValidationEmptyStock.ClientID %>').val();
            if (isValidate == '0') {
                var locationID = cboLocation.GetValue();
                var itemID = s.GetValue().split(';')[0];
                var itemname = '';

                var filterItem = "ItemID = '" + itemID + "'";
                Methods.getObject("GetItemMasterList", filterItem, function (resultItem) {
                    if (resultItem != null) {
                        itemname = resultItem.ItemName1;
                    }
                });

                var message = 'Stok untuk item ' + itemname + ' tidak mencukupi, Lanjutkan proses ?';
                var filterBalance = "ItemID = '" + itemID + "' AND LocationID = '" + locationID + "' AND IsDeleted = 0";
                Methods.getObject("GetItemBalanceList", filterBalance, function (resultBalance) {
                    if (resultBalance != null) {
                        if (resultBalance.QuantityEND <= 0) {
                            showToastConfirmation(message, function (result) {
                                if (result) {
                                    onPatientPageListEntryQuickEntrySave(s.GetValue());
                                }
                            });
                        }
                        else {
                            onPatientPageListEntryQuickEntrySave(s.GetValue());
                        }
                    }
                    else {
                        showToastConfirmation(message, function (result) {
                            if (result) {
                                onPatientPageListEntryQuickEntrySave(s.GetValue());
                            }
                        });
                    }
                });
            }
            else {
                onPatientPageListEntryQuickEntrySave(s.GetValue());
            }

        }

        function onAfterSaveQuickEntryRecord(val) {
            txtQuickEntry.SetText('');
            if ($('#<%=hdnPrescriptionOrderID.ClientID %>').val() == '') {
                $('#<%=hdnPrescriptionOrderID.ClientID %>').val(val);
                var filterExpression = $('#<%=hdnFilterExpressionItem.ClientID %>').val().replace("{PrescriptionID}", val);

                txtQuickEntry.SetFilterExpression(0, filterExpression);
                ledDrugName.SetFilterExpression(filterExpression);
                cboParamedicID.SetEnabled(false);
                cboRefillInstruction.SetEnabled(false);
                cboDispensaryUnit.SetEnabled(false);
                cboPrescriptionType.SetEnabled(false);
                cboLocation.SetEnabled(false);
            }
        }

        function SetMedicationDefaultTime(frequency) {
            Methods.getMedicationSequenceTime(frequency, function (result) {
                if (result != null) {
                    var medicationTimeInfo = result.split('|');
                    if (medicationTimeInfo[0] == "-") {
                        $('#<%=txtStartTime.ClientID %>').val("00:00");
                        $('#<%=txtStartTime1.ClientID %>').val("00:00");
                    }
                    else {
                        $('#<%=txtStartTime.ClientID %>').val(medicationTimeInfo[0]);
                        $('#<%=txtStartTime1.ClientID %>').val(medicationTimeInfo[0]);
                    }
                    $('#<%=txtStartTime2.ClientID %>').val(medicationTimeInfo[1]);
                    $('#<%=txtStartTime3.ClientID %>').val(medicationTimeInfo[2]);
                    $('#<%=txtStartTime4.ClientID %>').val(medicationTimeInfo[3]);
                    $('#<%=txtStartTime5.ClientID %>').val(medicationTimeInfo[4]);
                    $('#<%=txtStartTime6.ClientID %>').val(medicationTimeInfo[5]);
                }
                else {
                    $('#<%=txtStartTime.ClientID %>').val('00:00');
                    $('#<%=txtStartTime1.ClientID %>').val('00:00');
                    $('#<%=txtStartTime2.ClientID %>').val('-');
                    $('#<%=txtStartTime3.ClientID %>').val('-');
                    $('#<%=txtStartTime4.ClientID %>').val('-');
                    $('#<%=txtStartTime5.ClientID %>').val('-');
                    $('#<%=txtStartTime6.ClientID %>').val('-');
                }
            });
        }

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
            var filterExpression = "IsDeleted = 0 AND SignaLabel = '" + value + "'";
            Methods.getObject('GetvSignaList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnSignaID.ClientID %>').val(result.SignaID);
                    $('#<%=txtSignaName1.ClientID %>').val(result.SignaName1);
                    cboCoenamRule.SetValue(result.GCCoenamRule);
                    cboFrequencyTimeline.SetValue(result.GCDosingFrequency);
                    $('#<%=txtFrequencyNumber.ClientID %>').val(result.Frequency);
                    $('#<%=txtFrequencyNumber.ClientID %>').change();
                    cboDosingUnit.SetValue(result.GCDoseUnit);
                    $('#<%=txtDosingDose.ClientID %>').val(result.Dose);
                    $('#<%=txtDosingDose.ClientID %>').change();
                } else {
                    $('#<%=txtSignaLabel.ClientID %>').val('');
                    $('#<%=txtSignaName1.ClientID %>').val('');
                }
            });
        }
        //#endregion

        //#region calculate Dispense Qty
        $('#<%=txtFrequencyNumber.ClientID %>').live('change', function () {
            SetMedicationDefaultTime($('#<%=txtFrequencyNumber.ClientID %>').val());
            var dispQty = $('#<%=txtDispenseQty.ClientID %>').val();
            calculateDispenseQty();
        });

        $('#<%=txtStartTime1.ClientID %>').live('input', function () {
            $('#<%=txtStartTime.ClientID %>').val($('#<%=txtStartTime1.ClientID %>').val());
        });

        $('#<%=txtFrequencyNumber.ClientID %>').live('input', function () {
            $('#<%=txtSignaName1.ClientID %>').val('');
            $('#<%=txtSignaLabel.ClientID %>').val('');
            $('#<%=hdnSignaID.ClientID %>').val('');
        });

        $('#<%=txtDosingDose.ClientID %>').live('change', function () {
            var dispQty = $('#<%=txtDispenseQty.ClientID %>').val();
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
            $('#<%=txtSignaName1.ClientID %>').val('');
            $('#<%=txtSignaLabel.ClientID %>').val('');
            $('#<%=hdnSignaID.ClientID %>').val('');
        }

        function cboDosingUnitChanged() {
            $('#<%=txtSignaName1.ClientID %>').val('');
            $('#<%=txtSignaLabel.ClientID %>').val('');
            $('#<%=hdnSignaID.ClientID %>').val('');
        }

        function calculateDispenseQty() {
            var frequency = $('#<%=txtFrequencyNumber.ClientID %>').val();
            var frequencyTimeLine = cboFrequencyTimeline.GetValue();
            var dose = $('#<%=txtDosingDose.ClientID %>').val();
            var dosingDuration = $('#<%=txtDosingDuration.ClientID %>').val();
            var strengthAmount = $('#<%=txtStrengthAmount.ClientID %>').val();
            var strengthUnit = $('#<%=hdnStrengthUnit.ClientID %>').val();
            var dosingUnit = cboDosingUnit.GetText();
            var itemUnit = $('#<%=txtDispenseUnit.ClientID %>').val();

            var dispenseQty = 0;
            if (dosingUnit == itemUnit) {
                dispenseQty = Math.ceil(dosingDuration * frequency * dose);
            }
            else {
                if (strengthAmount != 0 && strengthUnit == dosingUnit) {
                    dispenseQty = Math.ceil((dosingDuration * frequency * dose) / strengthAmount);
                }
                else {
                    dispenseQty = 1;
                }
            }
            $('#<%=txtDispenseQty.ClientID %>').val(dispenseQty);
            $('#<%=txtDispenseQty.ClientID %>').change();
        }
        //#endregion

        function onCbpSendOrderEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'sendOrder') {
                if (param[1] == 'success') {
                    displayMessageBox('SEND ORDER', 'Order telah berhasil dikirim ke farmasi');
                    document.location = document.referrer;
                }
                else {
                    if (param[1] == 'confirm') {
                        var prescriptionOrderID = $('#<%=hdnPrescriptionOrderID.ClientID %>').val();
                        var queryString = prescriptionOrderID;

                        var url = ResolveUrl("~/Program/PatientPage/Planning/Prescription/Process/PrescriptionAlertConfirmCtl.ascx");
                        openUserControlPopup(url, queryString, 'Confirm : Medication Alert', 800, 600);
                        displayMessageBox('SEND ORDER : WARNING :', param[2]);
                    }
                    else if (param[1] == 'confirmPPRA') {
                        var prescriptionOrderID = $('#<%=hdnPrescriptionOrderID.ClientID %>').val();
                        var queryString = prescriptionOrderID;

                        var url = ResolveUrl("~/Program/PatientPage/Planning/Prescription/Process/PPRAAlertNotificationCtl.ascx");
                        openUserControlPopup(url, queryString, 'Program Pengendalian Resistensi Antimikroba (PPRA)', 800, 600);
                        displayMessageBox('SEND ORDER : Peresepan PPRA :', param[2]);
                    }
                    else {
                        displayErrorMessageBox('SEND ORDER', param[2]);
                    }
                }
            } else if (param[0] == 'saveHeader') {
                showLoadingPanel();
                document.location = document.referrer;
            }
        }

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var prescriptionOrderID = $('#<%=hdnPrescriptionOrderID.ClientID %>').val();
            if (prescriptionOrderID == '' || prescriptionOrderID == '0') {
                errMessage.text = 'Transaksi Resep harap diselesaikan terlebih dahulu.';
                return false;
            }
            else {
                if (code == 'PH-00028' || code == 'PH-00012' || code == 'PH-00044') {
                    filterExpression.text = prescriptionOrderID;
                    return true;
                }
            }
        }

        function onBeforeLoadRightPanelContent(code) {
            if (code == 'changeOrderStatus') {
                var param = $('#<%:hdnRegistrationID.ClientID %>').val() + '|' + $('#<%:hdnPrescriptionOrderID.ClientID %>').val() + '|' + $('#<%:txtPrescriptionNo.ClientID %>').val();
                return param;
            }
        }

        $('#<%=txtStartDate.ClientID %>').live('change', function () {
            var dateOrderItem1 = $('#<%=txtStartDate.ClientID %>').val();
            var dateToday = $('#<%=hdnPrescriptionDate.ClientID %>').val();

            if (dateOrderItem1 != '') {
                var from = dateOrderItem1.split("-");
                var f = new Date(from[2], from[1] - 1, from[0]);

                var to = dateToday.split("-");
                var t = new Date(to[2], to[1] - 1, to[0]);

                if (f < t) {
                    $('#<%=txtStartDate.ClientID %>').val(dateToday);
                }
            }
            else {
                $('#<%=txtStartDate.ClientID %>').val(dateToday);
            }
        });

        $('#<%=txtStartTime.ClientID %>').live('change', function () {
            var dateOrderItem1 = $('#<%=txtStartTime.ClientID %>').val();
            var timeToday = $('#<%=hdnPrescriptionTime.ClientID %>').val();

            if (dateOrderItem1 == '') {
                $('#<%=txtStartTime.ClientID %>').val(timeToday);
            }
            else {
                var isValid = /^([0-1]?[0-9]|2[0-4]):([0-5][0-9])(:[0-5][0-9])?$/.test(dateOrderItem1);

                if (!isValid) {
                    $('#<%=txtStartTime.ClientID %>').val(timeToday);
                }
            }
        });
        function onCboLocationEndCallback(s) {
            var baseFilter = $('#<%=hdnFilterExpressionItemBase.ClientID %>').val();
            var location = s.GetValue();

            $('#<%=hdnFilterExpressionItem.ClientID %>').val(baseFilter + " AND LocationID = '" + location + "' AND GCItemType IN ('X001^002','X001^003') AND GCItemStatus = 'X181^001' AND IsDeleted = 0 AND TotalQtyOnHand > 0");
            $('#<%=hdnFilterExpressionItemDrugInfo.ClientID %>').val(baseFilter + " AND LocationID = '" + location + "' AND GCItemType IN ('X001^002','X001^003') AND GCItemStatus = 'X181^001' AND IsDeleted = 0");

            txtQuickEntry.SetFilterExpression(0, $('#<%=hdnFilterExpressionItem.ClientID %>').val());
            ledDrugName.SetFilterExpression($('#<%=hdnFilterExpressionItemDrugInfo.ClientID %>').val());

            hideLoadingPanel()
        }
    </script>
    <input type="hidden" id="hdnFilterExpressionItemNewTransHd" runat="server" value="" />
    <input type="hidden" id="hdnFilterExpressionItemAdd" runat="server" value="" />
    <input type="hidden" id="hdnFilterExpressionItemEdit" runat="server" value="" />
    <input type="hidden" id="hdnRegistrationOrderID" runat="server" value="" />
    <input type="hidden" id="hdnPrescriptionOrderID" runat="server" value="" />
    <input type="hidden" id="hdnFilterExpressionItem" runat="server" value="" />
    <input type="hidden" id="hdnFilterExpressionItemDrugInfo" runat="server" value="" />
    <input type="hidden" id="hdnIsProposed" runat="server" value="0" />
    <input type="hidden" value="" id="hdnDefaultDispensaryServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnDefaultGCMedicationRoute" runat="server" />
    <input type="hidden" value="" id="hdnLocationID" runat="server" />
    <input type="hidden" value="" id="hdnSignaID" runat="server" />
    <input type="hidden" value="" id="hdnGCBaseUnit" runat="server" />
    <input type="hidden" value="" id="hdnStrengthUnit" runat="server" />
    <input type="hidden" value="0" id="hdnStrengthAmount" runat="server" />
    <input type="hidden" value="" id="hdnChargeClassID" runat="server" />
    <input type="hidden" value="" id="hdnPhysicianID" runat="server" />
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnVisitID" runat="server" />
    <input type="hidden" value="" id="hdnGCPrescriptionType" runat="server" />
    <input type="hidden" value="" id="hdnPrescriptionDate" runat="server" />
    <input type="hidden" value="" id="hdnPrescriptionTime" runat="server" />
    <input type="hidden" value="" id="hdnIsAllergyAlert" runat="server" />
    <input type="hidden" value="" id="hdnIsAdverseReactionAlert" runat="server" />
    <input type="hidden" value="" id="hdnIsDuplicateTheraphyAlert" runat="server" />
    <input type="hidden" value="" id="hdnDefaultDispensary" runat="server" />
    <input type="hidden" value="" id="hdnIsUsingUDD" runat="server" />
    <input type="hidden" id="hdnPrescriptionValidateStockAllRS" value="" runat="server" />
    <input type="hidden" id="hdnIsAutoMedicationFrequency" runat="server" value="" />
    <input type="hidden" id="hdnValidationEmptyStock" value="" runat="server" />
    <input type="hidden" id="hdnIsUsingDrugAlert" value="" runat="server" />
    <input type="hidden" id="hdnIsLimitedCPOEItemForBPJS" value="" runat="server" />
    <input type="hidden" id="hdnIsBPJS" value="" runat="server" />
    <input type="hidden" id="hdnFilterExpressionItemBase" value="" runat="server" />
    <table style="width: 100%" cellpadding="0" cellspacing="0">
        <colgroup>
            <col width="60%" />
            <col width="40%" />
        </colgroup>
        <tr>
            <td valign="top">
                <table cellpadding="0">
                    <colgroup>
                        <col style="width: 120px" />
                        <col style="width: 120px" />
                        <col style="width: 80px" />
                        <col style="width: 120px" />
                        <col style="width: 320px" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <%=GetLabel("Prescription No") %>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtPrescriptionNo" Width="99%" ReadOnly="true" runat="server" />
                        </td>
                        <td class="tdLabel" style="padding-left: 10px">
                            <%=GetLabel("Physician") %>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboParamedicID" ClientInstanceName="cboParamedicID" Width="100%"
                                runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <%=GetLabel("Date") %>
                            -
                            <%=GetLabel("Time") %>
                        </td>
                        <td style="padding-right: 1px; width: 120px">
                            <asp:TextBox ID="txtPrescriptionDate" ReadOnly="true" Width="100%" CssClass="datepicker"
                                runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtPrescriptionTime" Width="98%" CssClass="time" ReadOnly="true"
                                runat="server" Style="text-align: center" />
                        </td>
                        <td class="tdLabel" style="padding-left: 10px">
                            <label class="lblMandatory">
                                <%=GetLabel("Dispensary Unit") %></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboDispensaryUnit" ClientInstanceName="cboDispensaryUnit" runat="server"
                                Width="100%">
                                <ClientSideEvents ValueChanged="function(s,e){cboLocation.PerformCallback(); }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <%=GetLabel("Prescription Type") %>
                        </td>
                        <td colspan="2">
                            <dxe:ASPxComboBox ID="cboPrescriptionType" ClientInstanceName="cboPrescriptionType"
                                Width="100%" runat="server" />
                        </td>
                        <td class="tdLabel" style="padding-left: 10px">
                            <label class="lblNormal">
                                <%=GetLabel("Location")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox runat="server" ID="cboLocation" ClientInstanceName="cboLocation"
                                Width="100%" OnCallback="cboLocation_Callback">
                                <ClientSideEvents BeginCallback="function() { showLoadingPanel(); }" EndCallback="function(s,e) { onCboLocationEndCallback(s); }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                </table>
            </td>
            <td style="width: 360px; text-align: right" valign="top">
                <table cellpadding="0" style="text-align: left;" width="100%">
                    <colgroup>
                        <col style="width: 120px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel" style="vertical-align: top">
                            <label class="lblNormal">
                                <%=GetLabel("Order Remarks") %></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRemarks" Width="100%" runat="server" TextMode="MultiLine" Height="45px" />
                        </td>
                    </tr>
                    <tr style="display:none">
                        <td class="tdLabel">
                            <%=GetLabel("Refill Instruction") %>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboRefillInstruction" ClientInstanceName="cboRefillInstruction"
                                Width="130px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top" style="padding-top: 5px;">
                            <label id="lblAllergy" runat="server"><%=GetLabel("Alergi Pasien") %></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAllergyInfo" runat="server" Width="100%" TextMode="Multiline" Height="45px" ReadOnly="true" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="ctnQuickEntry" ContentPlaceHolderID="plhQuickEntry" runat="server">
    <table>
        <tr>
            <td style="width: 117px" class="tdLabel">
                <%=GetLabel("Quick Entry")%>
            </td>
            <td>
                <qis:QISQuickEntry runat="server" ClientInstanceName="txtQuickEntry" ID="txtQuickEntry"
                    Width="650px">
                    <ClientSideEvents SearchClick="function(s){ onTxtQuickEntrySearchClick(s); }" />
                    <QuickEntryHints>
                        <qis:QISQuickEntryHint Text="Drug Name" ValueField="ItemID" TextField="ItemName1"
                            Description="Item Name" MethodName="GetvItemBalanceQuickPick3List"
                            FilterExpression="GCItemType IN ('X001^002','X001^003') AND GCItemStatus = 'X181^001' AND IsDeleted = 0">
                            <Columns>
                                <qis:QISQuickEntryHintColumn Caption="Item Code" FieldName="ItemCode" Width="80px" />
                                <qis:QISQuickEntryHintColumn Caption="Item Name" FieldName="ItemName1" Width="600px" />
                                <qis:QISQuickEntryHintColumn Caption="Employee Formularium" FieldName="IsEmployeeFormularium"
                                    Width="70px" />
                                <qis:QISQuickEntryHintColumn Caption="Qty On Hand" FieldName="TotalQtyOnHand"
                                    Width="80px" />
                            </Columns>
                        </qis:QISQuickEntryHint>
                        <qis:QISQuickEntryHint Text="Frequency" ValueField="StandardCodeID" TextField="StandardCodeName"
                            Description="i.e. QD / BID / TID / QID / QH# / #dd / prn" MethodName="GetStandardCodeList"
                            FilterExpression="ParentID = 'X233'">
                            <Columns>
                                <qis:QISQuickEntryHintColumn Caption="Frequency" FieldName="StandardCodeName" Width="300px" />
                            </Columns>
                        </qis:QISQuickEntryHint>
                        <qis:QISQuickEntryHint Text="Dosing" Description="Dosing Quantity" />
                        <qis:QISQuickEntryHint Text="Dispense Quantity" />
                        <qis:QISQuickEntryHint Text="Dosing Unit" ValueField="StandardCodeID" TextField="StandardCodeName"
                            Description="Dosing Unit" MethodName="GetStandardCodeList"
                            FilterExpression="ParentID = 'X003' AND (TagProperty LIKE '%PRE%' OR TagProperty LIKE '%1%')">
                            <Columns>
                                <qis:QISQuickEntryHintColumn Caption="Dosing Unit" FieldName="StandardCodeName" Width="300px" />
                            </Columns>
                        </qis:QISQuickEntryHint>
                        <qis:QISQuickEntryHint Text="Special Instruction" Description="Special Instruction" />
                    </QuickEntryHints>
                </qis:QISQuickEntry>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="ctnEntry" ContentPlaceHolderID="plhEntry" runat="server">
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
                        <col width="40px" />
                        <col width="60px" />
                        <col width="40px" />
                        <col width="60px" />
                        <col width="100px" />
                        <col width="60px" />
                    </colgroup>
                    <tr>
                        <td>
                            <table border="0" cellpadding="0" cellspacing="1">
                                <tr>
                                    <td style="padding-right: 10px">
                                        <asp:CheckBox runat="server" ID="chkIsRx" Text="Rx" Checked="true" Enabled="false" />
                                    </td>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Drug Name")%></label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td colspan="6">
                            <input type="hidden" value="" id="hdnDrugID" runat="server" />
                            <input type="hidden" value="" id="hdnDrugName" runat="server" />
                            <qis:QISSearchTextBox ID="ledDrugName" ClientInstanceName="ledDrugName" runat="server"
                                Width="100%" ValueText="ItemID" FilterExpression="GCItemType IN ('X001^002','X001^003') AND GCItemStatus = 'X181^001' AND IsDeleted = 0" 
                                DisplayText="ItemName1" MethodName="GetvItemBalanceQuickPick3List">
                                <ClientSideEvents ValueChanged="function(s){ onLedDrugNameLostFocus(s); }" />
                                <Columns>
                                    <qis:QISSearchTextBoxColumn Caption="Drug Name" FieldName="ItemName1" Description="i.e. Panadol"
                                        Width="400px" />
                                    <qis:QISSearchTextBoxColumn Caption="Formularium" FieldName="IsFormularium" Description="Formularium"
                                        Width="80px" />
                                    <qis:QISSearchTextBoxColumn Caption="Employee" FieldName="IsEmployeeFormularium"
                                        Description="Employee Formularium" Width="80px" />
                                    <qis:QISSearchTextBoxColumn Caption="Qty On Hand (All)" FieldName="TotalQtyOnHand"
                                        Description="Qty On Hand" Width="80px" />
                                </Columns>
                            </qis:QISSearchTextBox>
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Generic Name")%></label>
                        </td>
                        <td colspan="6">
                            <asp:TextBox runat="server" ID="txtGenericName" Width="100%" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Strength")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtStrengthAmount" runat="server" Width="100%" CssClass="number"
                                ReadOnly="true" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtStrengthUnit" runat="server" Width="100%" ReadOnly="true" />
                        </td>
                        <td />
                        <td />
                        <td class="tdLabel" style="display: none">
                            <label class="lblNormal">
                                <%=GetLabel("Form")%></label>
                        </td>
                        <td style="display: none">
                            <dxe:ASPxComboBox runat="server" ID="cboDrugForm" ClientInstanceName="cboDrugForm"
                                Width="100%" />
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
                                Width="100%">
                                <ClientSideEvents ValueChanged="function(s,e){ cboDosingUnitChanged(); }" />
                            </dxe:ASPxComboBox>
                        </td>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Duration")%></label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtDosingDuration" Width="100%" CssClass="number" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Dispense Qty")%></label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtDispenseQty" Width="99%" CssClass="number" />
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtDispenseUnit" Width="100%" ReadOnly="true" TabIndex="999" />
                        </td>
                        <td colspan="2">
                            <asp:CheckBox ID="chkIsAsRequired" runat="server" Text="As Required" Checked="false" />
                        </td>
                        <td colspan="2">
                            <asp:CheckBox ID="chkIsIMM" runat="server" Text="IMM" Checked="false" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Medication Route")%></label>
                        </td>
                        <td colspan="4">
                            <dxe:ASPxComboBox runat="server" ID="cboMedicationRoute" ClientInstanceName="cboMedicationRoute"
                                Width="100%" />
                        </td>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("AC/DC/PC")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox runat="server" ID="cboCoenamRule" ClientInstanceName="cboCoenamRule"
                                Width="100%" />
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
                            <label class="lblMandatory">
                                <%=GetLabel("Start Date / Time")%></label>
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
                                        <asp:TextBox runat="server" ID="txtStartTime" CssClass="time" Width="100%" Enabled="false" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trTakenTime" runat="server">
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Taken Time")%></label>
                        </td>
                        <td>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td style="width: 25%">
                                        <asp:CheckBox ID="chkIsMorning" runat="server" Text="Morning" Checked="false" />
                                    </td>
                                    <td style="width: 25%">
                                        <asp:CheckBox ID="chkIsNoon" runat="server" Text="Noon" Checked="false" />
                                    </td>
                                    <td style="width: 25%">
                                        <asp:CheckBox ID="chkIsEvening" runat="server" Text="Evening" Checked="false" />
                                    </td>
                                    <td style="width: 25%">
                                        <asp:CheckBox ID="chkIsNight" runat="server" Text="Night" Checked="false" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Waktu Pemberian Obat")%></label>
                        </td>
                        <td>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td style="width: 15%" align="center"">
                                        <label class="lblNormal">1</label>
                                    </td>
                                    <td style="width: 15%" align="center"">
                                        <label class="lblNormal">2</label>
                                    </td>
                                    <td style="width: 15%" align="center"">
                                        <label class="lblNormal">3</label>
                                    </td>
                                    <td style="width: 15%" align="center"">
                                        <label class="lblNormal">4</label>
                                    </td>
                                    <td style="width: 15%" align="center"">
                                        <label class="lblNormal">5</label>
                                    </td>
                                    <td style="width: 15%" align="center"">
                                        <label class="lblNormal">6</label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%">
                                        <asp:TextBox runat="server" ID="txtStartTime1" CssClass="time" Width="100%" Text="00:00" />
                                    </td>
                                    <td style="width: 15%">
                                        <asp:TextBox runat="server" ID="txtStartTime2" CssClass="time" Width="100%" Text="00:00" />
                                    </td>
                                    <td style="width: 15%">
                                        <asp:TextBox runat="server" ID="txtStartTime3" CssClass="time" Width="100%" Text="00:00" />
                                    </td>
                                    <td style="width: 15%">
                                        <asp:TextBox runat="server" ID="txtStartTime4" CssClass="time" Width="100%" Text="00:00"/>
                                    </td>
                                    <td style="width: 15%">
                                        <asp:TextBox runat="server" ID="txtStartTime5" CssClass="time" Width="100%" Text="00:00"/>
                                    </td>
                                    <td style="width: 15%">
                                        <asp:TextBox runat="server" ID="txtStartTime6" CssClass="time" Width="100%" Text="00:00"/>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                            <label class="lblNormal">
                                <%=GetLabel("Administration Instruction")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMedicationAdministration" Width="400px" runat="server" TextMode="MultiLine" />
                        </td>
                    </tr>
                    <tr id="trAlertRemarks" runat="server" style="display: none">
                        <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                            <label class="lblNormal">
                                <%=GetLabel("Alert Remarks")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPurposeOfMedication" Width="400px" runat="server" TextMode="MultiLine" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="ctnList" ContentPlaceHolderID="plhList" runat="server">
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <div style="position: relative;">
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height: 300px">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false"
                            ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdView_RowDataBound">
                            <Columns>
                                <asp:BoundField DataField="PrescriptionOrderDetailID" HeaderStyle-CssClass="keyField"
                                    ItemStyle-CssClass="keyField" />
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20px">
                                    <ItemTemplate>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <img class="imgEdit imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                        alt="" />
                                                </td>
                                                <td style="width: 1px">
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <img class="imgDelete imgLink" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                        alt="" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                    <ItemTemplate>
                                        <input type="hidden" value="<%#:Eval("PrescriptionOrderDetailID") %>" bindingfield="PrescriptionOrderDetailID" />
                                        <input type="hidden" value="<%#:Eval("GenericName") %>" bindingfield="GenericName" />
                                        <input type="hidden" value="<%#:Eval("IsRFlag") %>" bindingfield="IsRFlag" />
                                        <input type="hidden" value="<%#:Eval("IsCompound") %>" bindingfield="IsCompound" />
                                        <input type="hidden" value="<%#:Eval("ItemID") %>" bindingfield="ItemID" />
                                        <input type="hidden" value="<%#:Eval("DrugName") %>" bindingfield="DrugName" />
                                        <input type="hidden" value="<%#:Eval("GCDrugForm") %>" bindingfield="GCDrugForm" />
                                        <input type="hidden" value="<%#:Eval("Dose") %>" bindingfield="Dose" />
                                        <input type="hidden" value="<%#:Eval("DoseUnit") %>" bindingfield="DoseUnit" />
                                        <input type="hidden" value="<%#:Eval("GCDoseUnit") %>" bindingfield="GCDoseUnit" />
                                        <input type="hidden" value="<%#:Eval("GCDosingFrequency") %>" bindingfield="GCDosingFrequency" />
                                        <input type="hidden" value="<%#:Eval("Frequency") %>" bindingfield="Frequency" />
                                        <input type="hidden" value="<%#:Eval("NumberOfDosage") %>" bindingfield="NumberOfDosage" />
                                        <input type="hidden" value="<%#:Eval("GCDosingUnit") %>" bindingfield="GCDosingUnit" />
                                        <input type="hidden" value="<%#:Eval("DosingDuration") %>" bindingfield="DosingDuration" />
                                        <input type="hidden" value="<%#:Eval("GCRoute") %>" bindingfield="GCRoute" />
                                        <input type="hidden" value="<%#:Eval("MedicationPurpose") %>" bindingfield="MedicationPurpose" />
                                        <input type="hidden" value="<%#:Eval("StartDateInDatePickerFormat") %>" bindingfield="StartDate" />
                                        <input type="hidden" value="<%#:Eval("StartTime") %>" bindingfield="StartTime" />
                                        <input type="hidden" value="<%#:Eval("DispenseQty") %>" bindingfield="DispenseQty" />
                                        <input type="hidden" value="<%#:Eval("DispenseQtyInString") %>" bindingfield="DispenseQtyInString" />
                                        <input type="hidden" value="<%#:Eval("ItemUnit") %>" bindingfield="ItemUnit" />
                                        <input type="hidden" value="<%#:Eval("GCCoenamRule") %>" bindingfield="GCCoenamRule" />
                                        <input type="hidden" value="<%#:Eval("MedicationAdministration") %>" bindingfield="MedicationAdministration" />
                                        <input type="hidden" value="<%#:Eval("IsMorning") %>" bindingfield="IsMorning" />
                                        <input type="hidden" value="<%#:Eval("IsNoon") %>" bindingfield="IsNoon" />
                                        <input type="hidden" value="<%#:Eval("IsEvening") %>" bindingfield="IsEvening" />
                                        <input type="hidden" value="<%#:Eval("IsNight") %>" bindingfield="IsNight" />
                                        <input type="hidden" value="<%#:Eval("IsAsRequired") %>" bindingfield="IsAsRequired" />
                                        <input type="hidden" value="<%#:Eval("SignaID") %>" bindingfield="SignaID" />
                                        <input type="hidden" value="<%#:Eval("SignaLabel") %>" bindingfield="SignaLabel" />
                                        <input type="hidden" value="<%#:Eval("SignaName1") %>" bindingfield="SignaName1" />
                                        <input type="hidden" value="<%#:Eval("IsIMM") %>" bindingfield="IsIMM" />
                                        <input type="hidden" value="<%#:Eval("IsAllergyAlert") %>" bindingfield="IsAllergyAlert" />
                                        <input type="hidden" value="<%#:Eval("IsDuplicateTheraphyAlert") %>" bindingfield="IsDuplicateTheraphyAlert" />
                                        <input type="hidden" value="<%#:Eval("IsAdverseReactionAlert") %>" bindingfield="IsAdverseReactionAlert" />
                                        <input type="hidden" value="<%#:Eval("Sequence1Time") %>" bindingfield="Sequence1Time" />
                                        <input type="hidden" value="<%#:Eval("Sequence2Time") %>" bindingfield="Sequence2Time" />
                                        <input type="hidden" value="<%#:Eval("Sequence3Time") %>" bindingfield="Sequence3Time" />
                                        <input type="hidden" value="<%#:Eval("Sequence4Time") %>" bindingfield="Sequence4Time" />
                                        <input type="hidden" value="<%#:Eval("Sequence5Time") %>" bindingfield="Sequence5Time" />
                                        <input type="hidden" value="<%#:Eval("Sequence6Time") %>" bindingfield="Sequence6Time" />
                                        <input type="hidden" value="<%#:Eval("LocationID") %>" bindingfield="LocationID" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="itemName">
                                    <HeaderTemplate>
                                        <div>
                                            <%=GetLabel("Drug Name")%>
                                        </div>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div>
                                            <%#: Eval("cfMedicationName")%></div>
                                        <div style='<%# Eval("IsCompound").ToString() == "False" ? "display:none;": "white-space: pre-line;font-style:italic; padding-top:5px" %>'>
                                            <%#: Eval("cfCompoundDetail")%></div>
                                    </ItemTemplate>
                                </asp:TemplateField>
<%--                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                    <HeaderTemplate>
                                        <div>
                                            <%=GetLabel("i.m.m")%></div>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div>
                                            <asp:CheckBox ID="chkIsIMM" runat="server" Enabled="false" Checked='<%# Eval("IsIMM")%>' /></div>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                <asp:BoundField DataField="Frequency" HeaderText="Frequency" HeaderStyle-HorizontalAlign="Right"
                                    HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField DataField="DosingFrequency" HeaderText="Timeline" HeaderStyle-HorizontalAlign="Left"
                                    HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="cfNumberOfDosage" HeaderText="Dose" HeaderStyle-HorizontalAlign="Right"
                                    HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField DataField="DosingUnit" HeaderText="Unit" HeaderStyle-HorizontalAlign="Left"
                                    HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Left" />
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                    <HeaderTemplate>
                                        <div>
                                            <%=GetLabel("Signa")%></div>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div>
                                            <%#: Eval("cfConsumeMethod3")%></div>
                                        <div style="font-style:italic"> 
                                            <%#: Eval("MedicationAdministration")%></div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Route" HeaderText="Route" HeaderStyle-HorizontalAlign="Left"
                                    HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="StartDateInDatePickerFormat" HeaderText="Start Date" HeaderStyle-HorizontalAlign="Center"
                                    HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="DispenseQtyInString" HeaderText="Quantity" HeaderStyle-HorizontalAlign="Right"
                                    HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Right" />
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px">
                                    <ItemTemplate>
                                        <div><img id="imgHAM" runat="server" class="blink-icon" src='<%# ResolveUrl("~/Libs/Images/Status/ham.png") %>' title='High Alert Medication' alt="" style ="height:24px; width:24px;" /></div>
                                        <img src='<%# ResolveUrl("~/Libs/Images/Status/warning.png") %>' <%# Eval("IsAllergyAlert").ToString() == "True" ? "" : "Style ='display:none'" %>
                                            title='<%=GetLabel("Allergy Alert") %>' alt="" />
                                        <img src='<%# ResolveUrl("~/Libs/Images/Status/warning.png") %>' <%# Eval("IsAdverseReactionAlert").ToString() == "True" ? "" : "Style ='display:none'" %>
                                            title='<%=GetLabel("Adverse Reaction") %>' alt="" />
                                        <img src='<%# ResolveUrl("~/Libs/Images/Status/warning.png") %>' <%# Eval("IsDuplicateTheraphyAlert").ToString() == "True" ? "" : "Style ='display:none'" %>
                                            title='<%=GetLabel("Duplicate Theraphy") %>' alt="" />      
                                        <div><img id="imgIsHasRestriction" runat="server" class="blink-icon" src='<%# ResolveUrl("~/Libs/Images/Status/drug_alert.png") %>' title='Drug Restriction' alt="" style ="height:24px; width:24px;" /></div>       
                                        <div>
                                            <img id="imgPPRA" runat="server" class="blink-icon" src='<%# ResolveUrl("~/Libs/Images/Status/ppra.png") %>'
                                                title='Termasuk dalam Kategori Program Pengendalian Resistensi Antimikroba (PPRA)' alt="" style="height: 24px; width: 24px;" /></div>                                                                  
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-CssClass="hiddenColumn isHasRestrictionInformation" HeaderStyle-CssClass="hiddenColumn">
                                    <ItemTemplate>
                                        <input type="text" id="lblHasRestrictionInformation" runat="server" value="0" style="width: 20px"
                                            class="lblHasRestrictionInformation" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("No Data To Display")%>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </asp:Panel>
                </dx:PanelContent>
            </PanelCollection>
        </dxcp:ASPxCallbackPanel>
        <dxcp:ASPxCallbackPanel ID="cbpSendOrder" runat="server" Width="100%" ClientInstanceName="cbpSendOrder"
            ShowLoadingPanel="false" OnCallback="cbpSendOrder_Callback">
            <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpSendOrderEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
        <div class="imgLoadingGrdView" id="containerImgLoadingView">
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
        <div class="containerPaging">
            <div class="wrapperPaging">
                <div id="paging">
                </div>
            </div>
        </div>
    </div>
</asp:Content>
