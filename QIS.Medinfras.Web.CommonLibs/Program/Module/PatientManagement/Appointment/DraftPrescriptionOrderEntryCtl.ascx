<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DraftPrescriptionOrderEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.DraftPrescriptionOrderEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_draftprescriptionorderctl">
    setDatePicker('<%=txtDraftPrescriptionOrderDate.ClientID %>');

    function onGetDrugFilterExpression() {
        var LocationID = cboLocation.GetValue();
        var filterExpression = "LocationID = " + LocationID + " AND IsDeleted = 0";
        if ($('#<%=hdnIsAllowOverIssued.ClientID %>').val() == '0') {
            filterExpression += " AND QuantityEND > 0 ";
        }
        var presOrderID = $('#<%=hdnDraftPrescriptionOrderID.ClientID %>').val();
        if ($('#<%=hdnDraftPrescriptionOrderID.ClientID %>').val() != '0' && $('#<%=hdnDraftPrescriptionOrderID.ClientID %>').val() != null) {
            var presOrderID = $('#<%=hdnDraftPrescriptionOrderID.ClientID %>').val();
            filterExpression += " AND ItemID NOT IN (SELECT ItemID FROM PrescriptionOrderDt WHERE PrescriptionOrderID = " + presOrderID + " AND IsDeleted = 0)";
        }

        filterExpression += " AND GCItemStatus != 'X181^999'";
        return filterExpression;
    }

    window.onCbpViewCtlEndCallback = function (s, e) {
        hideLoadingPanel();
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail') {
                showToast('Save Failed', 'Error Message : ' + param[2]);
            }
            else {
                $('#containerEntry').hide();
                $('#<%=hdnDraftPrescriptionOrderID.ClientID %>').val(s.cpPrescriptionOrderID);
                cbpMainPopup.PerformCallback('loadaftersave');
            }
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail') {
                showToast('Delete Failed', 'Error Message : ' + param[2]);
            }
            else {
                $('#containerEntry').hide();
                $('#<%=hdnDraftPrescriptionOrderID.ClientID %>').val(s.cpPrescriptionOrderID);
                cbpMainPopup.PerformCallback('loadaftersave');
            }
        }
    }

    //#region Prescription No
    $('#lblDraftPrescriptionOrderNo.lblLink').live('click', function () {
        var filterExpression = "AppointmentID = '" + $('#<%=hdnAppointmentID.ClientID %>').val() + "'";
        openSearchDialog('draftprescriptionorderhd', filterExpression, function (value) {
            $('#<%=txtDraftPrescriptionOrderNo.ClientID %>').val(value);
            ontxtDraftPrescriptionOrderNoChanged(value);
        });
    });

    $('#<%=txtDraftPrescriptionOrderNo.ClientID %>').change(function () {
        ontxtDraftPrescriptionOrderNoChanged($(this).val());
    });

    function ontxtDraftPrescriptionOrderNoChanged(value) {
        cbpMainPopup.PerformCallback('load');
    }
    //#endregion

    $('#lblAddData').live('click', function (evt) {
        if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup')) {
            var startDate = $('#<%=txtDraftPrescriptionOrderDate.ClientID %>').val();
            var startTime = $('#<%=txtDraftPrescriptionOrderTime.ClientID %>').val();

            $('#<%=lblPhysician.ClientID %>').attr('class', 'lblDisabled');
            $('#<%=txtPhysicianCode.ClientID %>').attr('readonly', 'readonly');
            cboDispensaryUnit.SetEnabled(false);
            cboLocation.SetEnabled(false);
            $('#<%=txtDraftPrescriptionOrderDate.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtDraftPrescriptionOrderTime.ClientID %>').attr('readonly', 'readonly');

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
            $('#<%=hdnSignaID.ClientID %>').val('');
            $('#<%=txtSignaLabel.ClientID %>').val('');
            $('#<%=txtSignaName1.ClientID %>').val('');
            $('#<%=hdnSignaID.ClientID %>').val('');
            $('#<%=txtDispenseQty.ClientID %>').val('');
            $('#<%=txtDispenseUnit.ClientID %>').val('');
            $('#<%=hdnTakenQty.ClientID %>').val('');
            $('#<%=hdnTakenUnit.ClientID %>').val('');
        }
    });

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
    $('#<%=lblDrug.ClientID %>.lblLink').live('click', function () {
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
                $('#<%=hdnSignaID.ClientID %>').val('');
                $('#<%=txtSignaName1.ClientID %>').val('');
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

    $('#btnCancel').live('click', function () {
        $('#containerEntry').hide();
    });

    $('#btnSave').live('click', function (evt) {
        if (IsValid(evt, 'fsTrxPopup', 'mpTrxPopup')) {
            cbpViewCtl.PerformCallback('save');
        }
    });

    $('#<%=grdView.ClientID %> .imgDelete.imgLink').live('click', function () {
        $row = $(this).closest('tr').parent().closest('tr');
        showToastConfirmation('Are You Sure Want To Delete?', function (result) {
            if (result) {
                var entity = rowToObject($row);
                $('#<%=hdnEntryID.ClientID %>').val(entity.DraftPrescriptionOrderDetailID);
                cbpViewCtl.PerformCallback('delete');
            }
        });
    });

    $('#<%=grdView.ClientID %> .imgEdit.imgLink').live('click', function () {
        $row = $(this).closest('tr').parent().closest('tr');
        var entity = rowToObject($row);
        if (entity.IsCompound != 'True') {
            $('#<%=hdnEntryID.ClientID %>').val(entity.DraftPrescriptionOrderDetailID);
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
            $('#<%=txtStartDate.ClientID %>').val(entity.cfStartDateInDatePickerFormat);
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
            $('#<%=hdnEntryID.ClientID %>').val(entity.DraftPrescriptionOrderDetailID);
            openCompoundEntry('edit');
        }
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
                $('#<%=hdnDraftPrescriptionOrderID.ClientID %>').val(s.cpPrescriptionOrderID);
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
                $('#<%=hdnDraftPrescriptionOrderID.ClientID %>').val(s.cpPrescriptionOrderID);
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
                $('#<%=hdnDraftPrescriptionOrderID.ClientID %>').val(s.cpPrescriptionOrderID);
                cbpMainPopup.PerformCallback('loadaftersave');
            }
        }

        hideLoadingPanel();
        var isAdd = $('#<%=hdnIsAdd.ClientID %>').val();
        var isEditable = $('#<%=hdnIsEditable.ClientID %>').val();
        SetCustomVisibilityControl(isAdd, isEditable);
    }
    window.onCboDosingUnitEndCallback = function (s, e) {
        var gcDosingUnit = $('#<%=hdnGCDosingUnit.ClientID %>').val();
        cboDosingUnit.SetValue(gcDosingUnit);
        $('#<%=hdnTakenUnit.ClientID %>').val(cboDosingUnit.GetText());
    }

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
        var dosingUnit = cboDosingUnit.GetText();
        $('#<%=txtDispenseUnit.ClientID %>').val(dosingUnit);
        $('#<%=hdnTakenUnit.ClientID %>').val(dosingUnit);
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


    function onAfterSaveAddRecordEntryPopup(param) {
        $('#containerEntry').hide();
        $('#<%=hdnDraftPrescriptionOrderID.ClientID %>').val(param);
        cbpMainPopup.PerformCallback('loadaftersave');
    }

    function onAfterProposedRecordEntryPopup(param) {
        $('#containerEntry').hide();
        $('#<%=hdnDraftPrescriptionOrderID.ClientID %>').val(param);
        cbpMainPopup.PerformCallback('loadaftersave');
    }

    function onAfterVoidRecordEntryPopup(param) {
        $('#containerEntry').hide();
        $('#<%=hdnDraftPrescriptionOrderID.ClientID %>').val(param);
        cbpMainPopup.PerformCallback('loadaftersave');
    }    
</script>
<div style="height: 800px; overflow-y: scroll;">
    <dxcp:ASPxCallbackPanel ID="cbpMainPopup" runat="server" Width="100%" ClientInstanceName="cbpMainPopup"
        ShowLoadingPanel="false" OnCallback="cbpMainPopup_Callback">
        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpMainPopupEndCallback(s,e); }" />
        <PanelCollection>
            <dx:PanelContent ID="PanelContent2">
                <fieldset id="fsEntryPopup" style="margin: 0">
                    <asp:Panel runat="server" ID="Panel1" Style="width: 100%; margin-left: auto; margin-right: auto;
                        position: relative; font-size: 0.95em;">
                        <input type="hidden" value="" id="hdnClassID" runat="server" />
                        <input type="hidden" value="" id="hdnDefaultDispensaryServiceUnitID" runat="server" />
                        <input type="hidden" value="" id="hdnDraftPrescriptionOrderID" runat="server" />
                        <input type="hidden" value="" id="hdnDepartmentFromID" runat="server" />
                        <input type="hidden" value="" id="hdnHealthcareServiceUnitFromID" runat="server" />
                        <input type="hidden" value="0" id="hdnIsAllowOverIssued" runat="server" />
                        <input type="hidden" value="" id="hdnImagingServiceUnitID" runat="server" />
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
                                                <label class="lblLink" id="lblDraftPrescriptionOrderNo">
                                                    <%=GetLabel("No. Draft Order")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDraftPrescriptionOrderNo" Width="232px" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <%=GetLabel("Tanggal") %>
                                                -
                                                <%=GetLabel("Jam Order") %>
                                            </td>
                                            <td>
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td style="padding-right: 1px; width: 145px">
                                                            <asp:TextBox ID="txtDraftPrescriptionOrderDate" Width="120px" CssClass="datepicker"
                                                                runat="server" />
                                                        </td>
                                                        <td style="width: 5px">
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtDraftPrescriptionOrderTime" Width="80px" CssClass="time" runat="server"
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
                                                <label class="lblNormal">
                                                    <%=GetLabel("Lokasi Obat")%></label>
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
                                                <label class="lblMandatory">
                                                    <%=GetLabel("Jenis Resep") %></label>
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
                                            <col style="width: 30%" />
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
                                                            <asp:TextBox ID="txtPhysicianCode" Width="120px" runat="server" />
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
                                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                                <label>
                                                    <%=GetLabel("Catatan")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtNotes" Width="100%" TextMode="MultiLine" Height="90px" runat="server" />
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
                            <tr>
                                <td colspan="2">
                                    <div id="containerEntry" style="margin-top: 4px; display: none;">
                                        <div class="pageTitle">
                                            <%=GetLabel("Entry")%></div>
                                        <fieldset id="fsTrxPopup" style="margin: 0">
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
                                                            </colgroup>
                                                            <tr>
                                                                <td class="tdLabel">
                                                                    <table border="0" cellpadding="0" cellspacing="0">
                                                                        <colgroup>
                                                                            <col style="width: 40px" />
                                                                            <col />
                                                                        </colgroup>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:CheckBox runat="server" ID="chkIsRx" Text='Rx' />
                                                                            </td>
                                                                            <td class="tdLabel">
                                                                                <label class="lblLink lblMandatory" id="lblDrug" runat="server">
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
                                                                    <label class="lblMandatory">
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
                                                                                    <ClientSideEvents EndCallback="function(s,e) { onCboDosingUnitEndCallback(s,e) }" />
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
                                    <dxcp:ASPxCallbackPanel ID="cbpViewCtl" runat="server" Width="100%" ClientInstanceName="cbpViewCtl"
                                        ShowLoadingPanel="false" OnCallback="cbpViewCtl_Callback">
                                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewCtlEndCallback(s,e); }" />
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent1" runat="server">
                                                <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                                    position: relative; font-size: 0.95em;">
                                                    <asp:GridView ID="grdView" runat="server" CssClass="grdService grdNormal notAllowSelect"
                                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                        <Columns>
                                                            <asp:BoundField DataField="DraftPrescriptionOrderDetailID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                            <asp:TemplateField ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
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
                                                                    <input type="hidden" value="<%#:Eval("DraftPrescriptionOrderDetailID") %>" bindingfield="DraftPrescriptionOrderDetailID" />
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
                                                                    <input type="hidden" value="<%#:Eval("cfStartDateInDatePickerFormat") %>" bindingfield="cfStartDateInDatePickerFormat" />       
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
                                                                        <%#: Eval("cfInformationLine1")%></div>
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
                                                            <%=GetLabel("No Data To Display")%>
                                                        </EmptyDataTemplate>
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </dx:PanelContent>
                                        </PanelCollection>
                                    </dxcp:ASPxCallbackPanel>
                                    <div class="imgLoadingGrdView" id="containerImgLoadingView">
                                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                    </div>
                                    <div id="divAddDataPrescriptionDraftOrder" style="display: none; width: 100%; text-align: center;" runat="server">
                                        <span class="lblLink" id="lblAddData" <%=IsEditable.ToString() == "False" ? "style='display:none'" : "" %>>
                                            <%= GetLabel("Add Data")%></span>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </fieldset>
            </dx:PanelContent>
        </PanelCollection>
    </dxcp:ASPxCallbackPanel>
</div>
