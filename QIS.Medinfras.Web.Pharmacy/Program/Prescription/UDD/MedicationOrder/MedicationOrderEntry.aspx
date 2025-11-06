<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx2.master" AutoEventWireup="true"
    CodeBehind="MedicationOrderEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Pharmacy.Program.MedicationOrderEntry" %>

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
    <li id="btnOrderInfo" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbnotes.png")%>' alt="" /><div>
            <%=GetLabel("Informasi Order")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
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
    <input type="hidden" value="" id="hdnDefaultParamedicID" runat="server" />
    <input type="hidden" value="" id="hdnDefaultParamedicCode" runat="server" />
    <input type="hidden" value="" id="hdnDefaultParamedicName" runat="server" />
    <input type="hidden" value="0" id="hdnIsEntryMode" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="0" id="hdnIsSignaChanged" runat="server" />
    <input type="hidden" value="0" id="hdnIsUsingUDD" runat="server" />
    <uc1:PatientBannerCtl ID="ctlPatientBanner" runat="server" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#<%=btnOrderInfo.ClientID %>').click(function () {
                var prescriptionOrderID = $('#<%=hdnPrescriptionOrderID.ClientID %>').val();
                if (prescriptionOrderID == '') prescriptionOrderID = $('#<%=hdnDefaultPrescriptionOrderID.ClientID %>').val();
                var departmentID = $('#<%=hdnDepartmentID.ClientID %>').val();
                var imagingServiceUnitID = $('#<%=hdnImagingServiceUnitID.ClientID %>').val();
                var laboratoryServiceUnitID = $('#<%=hdnLaboratoryServiceUnitID.ClientID %>').val();
                var isEntryMode = $('#<%=hdnIsEntryMode.ClientID %>').val();
                var param = prescriptionOrderID + '|' + departmentID + '|' + imagingServiceUnitID + '|' + laboratoryServiceUnitID + '|' + isEntryMode;
                var url = ResolveUrl("~/Program/Prescription/UDD/MedicationOrder/MedicationOrderNotesCtl.ascx");
                openUserControlPopup(url, param, 'Informasi Order Resep', 800, 600);
            });

            $('#<%=btnPrescriptionCompoundEntry.ClientID %>').click(function (evt) {
                if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                    openCompoundEntry('add');
                }
            });
            setCustomToolbarVisibility();
            if ($('#<%=btnOrderInfo.ClientID %>').is(':visible')) {
                var transactionID = $('#<%=hdnTransactionID.ClientID %>').val();
                var orderStatus = $('#<%=hdnTransactionStatus.ClientID %>').val();
                if (orderStatus != Constant.OrderStatus.RECEIVED)
                    $('#<%=btnOrderInfo.ClientID %>').click();
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


            var queryString = value + "|" + prescriptionOrderID + '|' + transactionID + '|' + date + '|' + time + '|' + prescriptionDetailID + '|' + refillInstruction +
                              '|' + visitID + '|' + prescriptionFeeAmount + '|' + location + '|' + physicianID + '|' + dispensaryServiceUnitID + '|' + departmentID +
                              '|' + prescriptionType + '|' + classID + '|' + imagingServiceUnitID + '|' + laboratoryServiceUnitID;

            var url = ResolveUrl("~/Program/Prescription/UDD/MedicationOrder/MedicationOrderCompoundCtl.ascx");
            openUserControlPopup(url, queryString, 'Compound Medication', 950, 600);
        }

        function onAfterSaveRecordDtSuccess(TransactionID) {
            if ($('#<%=hdnTransactionID.ClientID %>').val() == '0') {
                $('#<%=hdnTransactionID.ClientID %>').val(TransactionID);
                var filterExpression = 'PrescriptionOrderID = ' + TransactionID;
                Methods.getObject('GetPrescriptionOrderHdList', filterExpression, function (result) {
                    $('#<%=hdnPrescriptionOrderID.ClientID %>').val(result.PrescriptionOrderID);
                    $('#<%=txtTransactionNo.ClientID %>').val(result.PrescriptionOrderNo);
                    $('#<%=hdnTransactionStatus.ClientID %>').val(result.GCOrderStatus);
                    cbpView.PerformCallback('refresh');
                });
                onAfterCustomSaveSuccess();
                setCustomToolbarVisibility();
            }
            else
                cbpView.PerformCallback('refresh');
        }

        function onGetDrugFilterExpression() {
            var LocationID = cboLocation.GetValue();
            var filterExpression = "LocationID = " + LocationID + " AND IsDeleted = 0";
            return filterExpression;
        }

        function onLoad() {
            if ($('#<%=hdnIsEditable.ClientID %>').val() != "0") {
                $('#lblAddData').show();
                $('#lblQuickPick').show();
            }
            else {
                $('#lblAddData').hide();
                $('#lblQuickPick').hide();
            }

            setDatePicker('<%=txtPrescriptionDate.ClientID %>');

            $('#<%=txtPrescriptionDate.ClientID %>').datepicker('option', 'maxDate', '0');

            $('#<%=txtPhysicianName.ClientID %>').attr('readonly', 'readonly');

            $('#<%=btnPrescriptionBack.ClientID %>').click(function () {
                showLoadingPanel();
                document.location = "<%=OnGetUrlReferrer() %>";
            });

            setDatePicker('<%=txtStartDate.ClientID %>');
            setDatePicker('<%=txtExpiredDate.ClientID %>');
            $('#<%=txtStartDate.ClientID %>').datepicker('option', 'minDate', '0');
            $('#<%=txtExpiredDate.ClientID %>').datepicker('option', 'minDate', '0');

            //#region Drug
            $('#lblDrug.lblLink').click(function () {
                openSearchDialog('vItemPrescriptionLookup', onGetDrugFilterExpression(), function (value) {
                    $('#<%=txtDrugCode.ClientID %>').val(value);
                    ontxtDrugCodeChanged(value);
                });
            });

            $('#<%=txtDrugCode.ClientID %>').change(function () {
                ontxtDrugCodeChanged($(this).val());
            });

            function ontxtDrugCodeChanged(value) {
                var prescriptionFeeAmount = parseFloat($('#<%=hdnPrescriptionFeeAmount.ClientID %>').val());
                var filterExpression = onGetDrugFilterExpression() + " AND ItemCode = '" + value + "'";
                Methods.getObject('GetvDrugInfoPerLocationList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnDrugID.ClientID %>').val(result.ItemID);
                        $('#<%=txtDrugCode.ClientID %>').val(result.ItemCode);
                        $('#<%=txtDrugName.ClientID %>').val(result.ItemName1);
                        $('#<%=txtGenericName.ClientID %>').val(result.GenericName);
                        $('#<%=txtStrengthAmount.ClientID %>').val(result.Dose);
                        $('#<%=txtStrengthUnit.ClientID %>').val(result.DoseUnit);
                        $('#<%=hdnGCDoseUnit.ClientID %>').val(result.GCDoseUnit);
                        $('#<%=hdnGCBaseUnit.ClientID %>').val(result.GCItemUnit);
                        $('#<%=hdnGCItemUnit.ClientID %>').val('');
                        //                        $('#<%=txtExpiredDate.ClientID %>').val(Methods.getJSONDateValue(result.ExpiredDate));
                        cboForm.SetValue(result.GCDrugForm);
                        $('#<%=txtFrequencyNumber.ClientID %>').val('1');
                        $('#<%=txtDosingDose.ClientID %>').val('1');
                        $('#<%=txtDosingDuration.ClientID %>').val('1');
                        $('#<%=txtDispenseQty.ClientID %>').val('1');
                        $('#<%=txtDispenseUnit.ClientID %>').val(result.ItemUnit);
                        cboCoenamRule.SetValue(result.GCCoenamRule);
                        cboMedicationRoute.SetValue(result.GCMedicationRoute);
                        cboDosingUnit.PerformCallback();
                        $('#<%=txtFrequencyNumber.ClientID %>').focus();
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
                        cboForm.SetValue('');
                        cboCoenamRule.SetValue('');
                        cboMedicationRoute.SetValue('');
                    }
                });
            }
            //#endregion

            //#region signa
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
                        cboCoenamRule.SetValue(result.GCCoenamRule);
                        $('#<%=txtFrequencyNumber.ClientID %>').val(result.Frequency);
                        $('#<%=txtFrequencyNumber.ClientID %>').change();
                        cboFrequencyTimeline.SetValue(result.GCDosingFrequency);
                        $('#<%=hdnGCDosingUnit.ClientID %>').val(result.GCDoseUnit);
                        cboDosingUnit.SetValue(result.GCDoseUnit);
                        $('#<%=txtDosingDose.ClientID %>').val(result.Dose);
                        $('#<%=txtDosingDurationTimeline.ClientID %>').val(cboFrequencyTimeline.GetText());
                    } else {
                        $('#<%=txtSignaName1.ClientID %>').val('');
                        cboForm.SetValue('');
                        $('#<%=txtFrequencyNumber.ClientID %>').val('');
                        cboFrequencyTimeline.SetValue('');
                        $('#<%=hdnGCDosingUnit.ClientID %>').val('');
                        cboDosingUnit.SetValue('');
                        cboCoenamRule.SetValue('');
                        $('#<%=txtDosingDose.ClientID %>').val('0');
                        $('#<%=txtDispenseUnit.ClientID %>').val('');
                        $('#<%=txtDosingDurationTimeline.ClientID %>').val('');
                    }
                });
                if ((dispQty <= 0)) {
                    calculateDispenseQty();
                }
            }
            //#endregion

            //#region Transaction No
            $('#lblPrescriptionNo.lblLink').click(function () {
                var filterExpression = "<%:OnGetFilterExpression() %>";
                openSearchDialog('prescriptionorderhd', filterExpression, function (value) {
                    $('#<%=txtTransactionNo.ClientID %>').val(value);
                    onTxtPrescriptionNoChanged(value);
                });
            });

            $('#<%=txtTransactionNo.ClientID %>').change(function () {
                onTxtPrescriptionNoChanged($(this).val());
            });

            function onTxtPrescriptionNoChanged(value) {
                onLoadObject(value);
            }
            //#endregion

            //#region Physician
            $('#<%=lblPhysician.ClientID %>.lblLink').click(function () {
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

            //#region Embalace
            $('#lblEmbalace.lblLink').click(function () {
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
                        var embalaceQty = $('#<%=txtEmbalaceQty.ClientID %>').val();
                        $('#<%=txtEmbalaceQty.ClientID %>').val(embalaceQty);
                    }
                    else {
                        $('#<%=txtEmbalaceName.ClientID %>').val('');
                        $('#<%=txtEmbalaceCode.ClientID %>').val('');
                        $('#<%=hdnEmbalaceID.ClientID %>').val('');
                    }
                });
            }

            $('#<%=txtDispenseQty.ClientID %>').change(function () {
            });

            $('#<%=txtEmbalaceQty.ClientID %>').change(function () {
                var embalaceID = $('#<%=hdnEmbalaceID.ClientID %>').val();
                var qty = parseInt($(this).val());
                if (qty < 0 || $(this).val() == "") {
                    $(this).val(0);
                }
            });
            //#endregion


            $('#btnCancel').click(function () {
                $('#containerEntry').hide();
            });

            $('#btnSave').click(function (evt) {
                if (IsValid(evt, 'fsTrx', 'mpTrx')) {
                    cbpProcess.PerformCallback('save');
                }
            });

            $('#lblQuickPick').live('click', function (evt) {
                if (IsValid(null, 'fsTrx', 'mpTrx')) {
                    if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                        showLoadingPanel();
                        var url = ResolveUrl('~/Program/Prescription/UDD/MedicationOrder/MedicationOrderQuickPicksCtl.ascx');
                        var transactionID = $('#<%=hdnTransactionID.ClientID %>').val();
                        var locationID = $('#<%=hdnLocationID.ClientID %>').val();
                        var defaultGCMedicationRoute = $('#<%=hdnDefaultGCMedicationRoute.ClientID %>').val();
                        var paramedicID = $('#<%=hdnPhysicianID.ClientID %>').val();
                        var registration = $('#<%=hdnRegistrationID.ClientID %>').val();
                        var visitID = $('#<%=hdnVisitID.ClientID %>').val();
                        var chargeClassID = $('#<%=hdnChargeClassID.ClientID %>').val();
                        var orderNotes = $('#<%=txtNotes.ClientID %>').val();
                        var id = transactionID + '|' + locationID + '|' + defaultGCMedicationRoute + '|' + paramedicID + '|' + registration + '|' + visitID + '|' + chargeClassID + '|' + orderNotes;
                        openUserControlPopup(url, id, 'Quick Picks', 1200, 600);
                    }
                }
            });
        }

        function setCustomToolbarVisibility() {
            var transactionStatus = $('#<%=hdnTransactionStatus.ClientID %>').val();
            if (transactionStatus != 'X126^002' && transactionStatus != '') {
                $('#<%=btnPrescriptionCompoundEntry.ClientID %>').hide();
            }
            else if (transactionStatus == 'X126^002' || transactionStatus == '') {
                $('#<%=btnPrescriptionCompoundEntry.ClientID %>').show();
            }

            var isVoid = $('#<%:hdnIsAllowVoid.ClientID %>').val();
            if (isVoid == 1) {
                if (getIsAdd()) {
                    $('#<%=btnVoid.ClientID %>').hide();
                }
                else {
                    if (transactionStatus == Constant.OrderStatus.RECEIVED) {
                        $('#<%=btnVoid.ClientID %>').show();
                    } else {
                        $('#<%=btnVoid.ClientID %>').hide();
                    }
                }
            } else {
                $('#<%=btnVoid.ClientID %>').hide();
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

        function onBeforeLoadRightPanelContent(code) {
            if (code == 'pharmacyNotes') {
                return $('#<%:hdnRegistrationID.ClientID %>').val() + '|' + $('#<%:hdnDispensaryServiceUnitID.ClientID %>').val();
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
                $('#<%=chkIsUsingUDD.ClientID %>').prop('checked', true);
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

                $('#<%=hdnEmbalaceID.ClientID %>').val('');
                $('#<%=txtEmbalaceCode.ClientID %>').val('');
                $('#<%=txtEmbalaceName.ClientID %>').val('');

                $('#<%=txtEmbalaceQty.ClientID %>').val('0');

                $('#<%=chkIsMorning.ClientID %>').prop('checked', false);
                $('#<%=chkIsNoon.ClientID %>').prop('checked', false);
                $('#<%=chkIsEvening.ClientID %>').prop('checked', false);
                $('#<%=chkIsNight.ClientID %>').prop('checked', false);
                $('#<%=chkIsAsRequired.ClientID %>').prop('checked', false);

                cboChargeClass.SetValue($('#<%=hdnChargeClassID.ClientID %>').val());

                $('#<%=txtDrugCode.ClientID %>').focus();
            }
        });

        $('.imgEdit.imgLink').die('click');
        $('.imgEdit.imgLink').live('click', function () {
            $row = $(this).closest('tr');
            var entity = rowToObject($row);
            if (entity.IsCompound != 'True') {
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
                cboLocation.SetValue($('#<%=hdnLocationID.ClientID %>').val());
                cboForm.SetValue(entity.GCDrugForm);
                $('#<%=txtPurposeOfMedication.ClientID %>').val(entity.MedicationPurpose);
                $('#<%=txtStrengthAmount.ClientID %>').val(entity.Dose);
                $('#<%=txtStrengthUnit.ClientID %>').val(entity.DoseUnit);
                $('#<%=hdnGCDoseUnit.ClientID %>').val(entity.GCDoseUnit);
                cboFrequencyTimeline.SetValue(entity.GCDosingFrequency);
                $('#<%=txtFrequencyNumber.ClientID %>').val(entity.Frequency);
                $('#<%=txtDosingDose.ClientID %>').val(entity.NumberOfDosage);
                cboDosingUnit.PerformCallback();
                $('#<%=hdnGCDosingUnit.ClientID %>').val(entity.GCDosingUnit);
                cboDosingUnit.SetValue(entity.GCDosingUnit);
                cboMedicationRoute.SetValue(entity.GCRoute);
                if (entity.GCCeonamRule) cboCoenamRule.SetValue(entity.GCCoenamRule);

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

                if (entity.IsUsingUDD == "True")
                    $('#<%=chkIsUsingUDD.ClientID %>').prop('checked', true);
                else
                    $('#<%=chkIsUsingUDD.ClientID %>').prop('checked', false);

                $('#<%=txtMedicationAdministration.ClientID %>').val(entity.MedicationAdministration);

                $('#<%=txtDosingDuration.ClientID %>').val(entity.DosingDuration);
                $('#<%=txtDispenseQty.ClientID %>').val(entity.DispenseQty);
                $('#<%=hdnSignaID.ClientID %>').val(entity.SignaID);
                $('#<%=txtSignaName1.ClientID %>').val(entity.SignaName1);
                $('#<%=txtSignaLabel.ClientID %>').val(entity.SignaLabel);
                cboChargeClass.SetValue(entity.ChargeClassID);

                $('#<%=txtEmbalaceName.ClientID %>').val(entity.EmbalaceName);
                $('#<%=txtEmbalaceCode.ClientID %>').val(entity.EmbalaceCode);
                $('#<%=hdnEmbalaceID.ClientID %>').val(entity.EmbalaceID);
                $('#<%=txtEmbalaceQty.ClientID %>').val(entity.EmbalaceQty);
                $('#containerEntry').show();
            } else {
                $('#<%=hdnPrescriptionDetailID.ClientID %>').val(entity.PrescriptionOrderDetailID);
                openCompoundEntry('edit');
            }
        });

        $('.imgDelete.imgLink').die('click');
        $('.imgDelete.imgLink').live('click', function () {
            $row = $(this).closest('tr');
            var obj = rowToObject($row);
            var message = "Hapus obat " + obj.DrugName + " ?";
            var param = 'delete|' + obj.PrescriptionOrderDetailID + ';' + '' + ';';
            $('#<%=hdnEntryID.ClientID %>').val(obj.PrescriptionOrderDetailID);
            showToastConfirmation(message, function (result) {
                if (result) cbpProcess.PerformCallback(param);
            });
        });
        //#endregion

        function onCbpProcesEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'save') {
                if (param[1] == 'fail') {
                    if (param[2] == 'confirm') {
                        showToast('Save Success', 'Warning Message : ' + param[3]);
                        var transactionID = s.cpTransactionID;
                        onAfterSaveRecordDtSuccess(transactionID);
                        $('#containerEntry').hide();
                    }
                    else showToast('Save Failed', 'Error Message : ' + param[3]);
                }
                else {
                    var transactionID = s.cpTransactionID;
                    onAfterSaveRecordDtSuccess(transactionID);
                    $('#containerEntry').hide();
                }
            }
            else if (param[0] == 'delete') {
                if (param[1] == 'fail')
                    showToast('Delete Failed', 'Error Message : ' + param[2]);
                else
                    cbpView.PerformCallback('refresh');
            }
            else if (param[0] == 'switch') {
                if (param[1] == 'fail')
                    showToast('Switch Failed', 'Error Message : ' + param[2]);
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

        function onAfterSaveEditRecordEntryPopup(param) {
            cbpView.PerformCallback('refresh');
        }

        //#region calculate Dispense Qty
        $('#<%=txtFrequencyNumber.ClientID %>').live('change', function () {
            var dispQty = $('#<%=txtDispenseQty.ClientID %>').val();
            if ((dispQty <= 0)) {
                calculateDispenseQty();
            }
        });

        $('#<%=txtDosingDose.ClientID %>').live('change', function () {
            var dispQty = $('#<%=txtDispenseQty.ClientID %>').val();
            if ((dispQty <= 0)) {
                calculateDispenseQty();
            }
        });

        $('#<%=txtDosingDuration.ClientID %>').live('change', function () {
            var dispQty = $('#<%=txtDispenseQty.ClientID %>').val();
            if ((dispQty <= 0)) {
                calculateDispenseQty();
            }
        });

        function cboFrequencyTimelineChanged() {
            var frequencyTimeLine = cboFrequencyTimeline.GetText();
            $('#<%=txtDosingDurationTimeline.ClientID %>').val(frequencyTimeLine);
        }

        function cboDosingUnitChanged() {
            //            var dosingUnit = cboDosingUnit.GetText();
            //            $('#<%=txtDispenseUnit.ClientID %>').val(dosingUnit);
        }

        function calculateDispenseQty() {
            var frequency = $('#<%=txtFrequencyNumber.ClientID %>').val();
            var frequencyTimeLine = cboFrequencyTimeline.GetValue();
            var dose = $('#<%=txtDosingDose.ClientID %>').val();
            var dosingDuration = $('#<%=txtDosingDuration.ClientID %>').val();
            var strengthAmount = $('#<%=txtStrengthAmount.ClientID %>').val();
            var dosingUnit = cboDosingUnit.GetText();
            var itemUnit = $('#<%=txtDispenseUnit.ClientID %>').val();

            var dispenseQty = 0;
            if (dosingUnit == itemUnit) {
                dispenseQty = Math.ceil(dosingDuration * frequency * dose);
            }
            else {
                dispenseQty = Math.ceil((dosingDuration * frequency * dose) / strengthAmount);
            }
            $('#<%=txtDispenseQty.ClientID %>').val(dispenseQty);
            $('#<%=txtDispenseQty.ClientID %>').change();

        }
        //#endregion

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var prescriptionOrderID = $('#<%=hdnPrescriptionOrderID.ClientID %>').val();
            var transactionID = $('#<%=hdnTransactionID.ClientID %>').val();
            if (prescriptionOrderID == '' || prescriptionOrderID == '0') {
                errMessage.text = 'Transaksi Resep harap diselesaikan terlebih dahulu.';
                return false;
            }
            else {
                if (code == 'PH-00003' || code == 'PH-00028') {
                    filterExpression.text = prescriptionOrderID;
                    return true;
                }
                else if (code == 'PM-00201' || code == 'PM-00236') {
                    filterExpression.text = transactionID;
                    return true;
                }
                else {
                    filterExpression.text = "PrescriptionOrderID = " + prescriptionOrderID;
                    return true;
                }
            }
        }

        function onCboDosingUnitEndCallback() {
            if ($('#<%=hdnGCDosingUnit.ClientID %>').val() == '') {
                if ($('#<%=hdnGCItemUnit.ClientID %>').val() == '')
                    cboDosingUnit.SetValue($('#<%=hdnGCBaseUnit.ClientID %>').val());
                else
                    cboDosingUnit.SetValue($('#<%=hdnGCItemUnit.ClientID %>').val());
            }
            else {
                cboDosingUnit.SetValue($('#<%=hdnGCDosingUnit.ClientID %>').val());
            }

            cboDosingUnitChanged();
        }

        //#region Drug Tariff
        function getTrxDate() {
            var date = Methods.getDatePickerDate($('#<%=txtPrescriptionDate.ClientID %>').val());
            var dateInYMD = Methods.dateToYMD(date);
            return dateInYMD;
        }

        function cboChargeClassChanged() {
            if ($('#<%=hdnDrugID.ClientID %>').val() != '') {
                getDrugTariff();
                calculate();
            }
        }

        //#endregion
    </script>
    <input type="hidden" value="" id="hdnIsEditable" runat="server" />
    <input type="hidden" value="" id="hdnLocationID" runat="server" />
    <input type="hidden" value="" id="hdnSignaID" runat="server" />
    <input type="hidden" value="" id="hdnTransactionID" runat="server" />
    <input type="hidden" value="" id="hdnPrescriptionDetailID" runat="server" />
    <input type="hidden" value="" id="hdnPrescriptionOrderID" runat="server" />
    <input type="hidden" value="" id="hdnDispensaryServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnGCDoseUnit" runat="server" />
    <input type="hidden" value="" id="hdnDefaultGCMedicationRoute" runat="server" />
    <input type="hidden" value="" id="hdnGCBaseUnit" runat="server" />
    <input type="hidden" value="" id="hdnGCItemUnit" runat="server" />
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
    <input type="hidden" value="" id="hdnIsAllowVoid" runat="server" />
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
                                    <%=GetLabel("No. Order")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTransactionNo" Width="150px" ReadOnly="true" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal lblMandatory" />
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
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Jenis Resep")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboPrescriptionType" ClientInstanceName="cboPrescriptionType"
                                    Width="233px" runat="server" />
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
                                                    <label class="lblLink lblMandatory" id="lblDrug">
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
                                                        <%=GetLabel("Signa")%></label>
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
                                                        <ClientSideEvents EndCallback="onCboDosingUnitEndCallback" SelectedIndexChanged="function(s,e){cboDosingUnitChanged()}" />
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
                                                            <td style="width: 20%">
                                                                <asp:CheckBox ID="chkIsMorning" runat="server" Text="Pagi" Checked="false" />
                                                            </td>
                                                            <td style="width: 20%">
                                                                <asp:CheckBox ID="chkIsNoon" runat="server" Text="Siang" Checked="false" />
                                                            </td>
                                                            <td style="width: 20%">
                                                                <asp:CheckBox ID="chkIsEvening" runat="server" Text="Sore" Checked="false" />
                                                            </td>
                                                            <td style="width: 20%">
                                                                <asp:CheckBox ID="chkIsNight" runat="server" Text="Malam" Checked="false" />
                                                            </td>
                                                            <td style="width: 20%">
                                                                <asp:CheckBox ID="chkIsAsRequired" runat="server" Text="PRN" Checked="false" />
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
                                                    <asp:TextBox runat="server" ID="txtDispenseQty" Width="100%" CssClass="number" />
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtDispenseUnit" Width="100%" ReadOnly="true" TabIndex="999" />
                                                </td>
                                                <td class="tdLabel" style="padding-left: 10px" colspan="2">
                                                 <asp:CheckBox runat="server" ID="chkIsUsingUDD" Text='UDD'/>   <label class="lblNormal">
                                                        <%=GetLabel("AC/DC/PC")%></label>
                                                </td>
                                                <td colspan="2">
                                                    <dxe:ASPxComboBox runat="server" ID="cboCoenamRule" ClientInstanceName="cboCoenamRule"
                                                        Width="110px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Expired Date")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtExpiredDate" CssClass="datepicker" Width="110px" />
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
                                                <td colspan="5">
                                                    <dxe:ASPxComboBox runat="server" ID="cboLocation" ClientInstanceName="cboLocation"
                                                        Width="100%" />
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
                                                <td colspan="4">
                                                    <asp:TextBox runat="server" ID="txtEmbalaceName" Width="100%" ReadOnly="true" TabIndex="999" />
                                                </td>
                                            </tr>
                                            <tr>
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
                                                <td>
                                                    <asp:CheckBox runat="server" ID="chkIsRx" Visible="false" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Jumlah Embalase")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtEmbalaceQty" CssClass="number" Width="60px" />
                                                </td>
                                                <td>
                                                </td>
                                                <td>
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
                                                        <%=GetLabel("Alasan Pengobatan")%></label>
                                                </td>
                                                <td colspan="6">
                                                    <asp:TextBox ID="txtPurposeOfMedication" Width="100%" runat="server" TextMode="MultiLine" />
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
                                                                <input type="button" id="btnSave" value='<%= GetLabel("Save")%>' class="btnPageEntry" />
                                                            </td>
                                                            <td>
                                                                <input type="button" id="btnCancel" value='<%= GetLabel("Cancel")%>' class="btnPageEntry" />
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
                                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                                        <asp:ListView ID="lvwView" runat="server">
                                            <EmptyDataTemplate>
                                                <table id="tblView" runat="server" class="grdNormal notAllowSelect" cellspacing="0"
                                                    rules="all">
                                                    <tr>
                                                        <th rowspan="2" style="width: 70px;">
                                                        </th>
                                                        <th rowspan="2" align="left">
                                                            <div>
                                                                <%=GetLabel("Drug Name")%>
                                                                -
                                                                <%=GetLabel("Form")%></div>
                                                            <div>
                                                                <div style="color: Blue; float: left;">
                                                                    <%=GetLabel("Generic Name")%></div>
                                                                    </div>
                                                        </th>
                                                        <th colspan="6" align="center">
                                                            <div>
                                                                <%=GetLabel("Signa")%></div>
                                                        </th>
                                                        <th rowspan="2" align="center" style="padding: 3px; width: 90px;">
                                                            <div>
                                                                <%=GetLabel("Start Date")%></div>
                                                        </th>
                                                        <th colspan="4">
                                                            <div>
                                                                <%=GetLabel("Medication Time") %></div>
                                                        </th>
                                                        <th rowspan="2" style="width: 100px;">
                                                            <div style="text-align:left">
                                                                <%=GetLabel("Status") %></div>
                                                        </th>
                                                        <th rowspan="2" style="width: 120px">
                                                            <div style="text-align: left; padding-right: 3px">
                                                            </div>
                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        <th style="width: 40px;">
                                                            <div style="text-align:right">
                                                                <%=GetLabel("Frequency") %></div>
                                                        </th>
                                                        <th style="width: 40px;text-align:left">
                                                            <div>
                                                                <%=GetLabel("Timeline") %></div>
                                                        </th>
                                                        <th style="width: 40px;">
                                                            <div style="text-align:right">
                                                                <%=GetLabel("Dose") %></div>
                                                        </th>
                                                        <th style="width: 50px;">
                                                            <div style="text-align:left">
                                                                <%=GetLabel("Unit") %></div>
                                                        </th>
                                                        <th align="center" style="width: 30px;">
                                                            <div>
                                                                <%=GetLabel("PRN")%></div>
                                                        </th>
                                                        <th style="width: 100px;">
                                                            <div style="text-align:left">
                                                                <%=GetLabel("Route") %></div>
                                                        </th>
                                                        <th style="width: 40px;">
                                                            <div>
                                                                <%=GetLabel("Morning") %></div>
                                                        </th>
                                                        <th style="width: 40px;">
                                                            <div>
                                                                <%=GetLabel("Noon") %></div>
                                                        </th>
                                                        <th style="width: 40px;">
                                                            <div>
                                                                <%=GetLabel("Evening") %></div>
                                                        </th>
                                                        <th style="width: 40px;">
                                                            <div>
                                                                <%=GetLabel("Night") %></div>
                                                        </th>
                                                    </tr>
                                                    <tr runat="server" id="itemPlaceholder">
                                                    </tr>
                                                    <tr class="trFooter">
                                                        <td colspan="17">
                                                            <div style="text-align: right; padding: 0px 3px">
                                                            </div>
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
                                                                <%=GetLabel("Drug Name")%>
                                                                -
                                                                <%=GetLabel("Form")%></div>
                                                            <div>
                                                                <div style="color: Blue; float: left;">
                                                                    <%=GetLabel("Generic Name")%></div>
                                                                    </div>
                                                        </th>
                                                        <th colspan="6" align="center">
                                                            <div>
                                                                <%=GetLabel("Signa")%></div>
                                                        </th>
                                                        <th rowspan="2" align="center" style="padding: 3px; width: 90px;">
                                                            <div>
                                                                <%=GetLabel("Start Date")%></div>
                                                        </th>
                                                        <th colspan="4">
                                                            <div>
                                                                <%=GetLabel("Medication Time") %></div>
                                                        </th>
                                                        <th rowspan="2" style="width: 100px;">
                                                            <div style="text-align:left">
                                                                <%=GetLabel("Status") %></div>
                                                        </th>
                                                        <th rowspan="2" style="width: 120px">
                                                            <div style="text-align: left; padding-right: 3px">
                                                            </div>
                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        <th style="width: 40px;">
                                                            <div style="text-align:right">
                                                                <%=GetLabel("Frequency") %></div>
                                                        </th>
                                                        <th style="width: 40px;text-align:left">
                                                            <div>
                                                                <%=GetLabel("Timeline") %></div>
                                                        </th>
                                                        <th style="width: 40px;">
                                                            <div style="text-align:right">
                                                                <%=GetLabel("Dose") %></div>
                                                        </th>
                                                        <th style="width: 50px;">
                                                            <div style="text-align:left">
                                                                <%=GetLabel("Unit") %></div>
                                                        </th>
                                                        <th align="center" style="width: 30px;">
                                                            <div>
                                                                <%=GetLabel("PRN")%></div>
                                                        </th>
                                                        <th style="width: 100px;">
                                                            <div style="text-align:left">
                                                                <%=GetLabel("Route") %></div>
                                                        </th>
                                                        <th style="width: 40px;">
                                                            <div>
                                                                <%=GetLabel("Morning") %></div>
                                                        </th>
                                                        <th style="width: 40px;">
                                                            <div>
                                                                <%=GetLabel("Noon") %></div>
                                                        </th>
                                                        <th style="width: 40px;">
                                                            <div>
                                                                <%=GetLabel("Evening") %></div>
                                                        </th>
                                                        <th style="width: 40px;">
                                                            <div>
                                                                <%=GetLabel("Night") %></div>
                                                        </th>
                                                    </tr>
                                                    <tr runat="server" id="itemPlaceholder">
                                                    </tr>
                                                    <tr class="trFooter">
                                                        <td colspan="17">
                                                            <div style="text-align: right; padding: 0px 3px">
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td>
                                                        <img class="imgEdit <%# IsEditable() == "0" || Eval("cfIsEditable").ToString() == "False" ? "imgDisabled" : "imgLink"%>"
                                                            src='<%# IsEditable() == "0" || Eval("cfIsEditable").ToString() == "False" ? ResolveUrl("~/Libs/Images/Button/edit_disabled.png") : ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                            alt="" style="float: left; margin-left: 7px" />
                                                        &nbsp;
                                                        <img class="imgDelete <%# IsEditable() == "0" || Eval("cfIsEditable").ToString() == "False" ? "imgDisabled" : "imgLink"%>"
                                                            src='<%# IsEditable() == "0" || Eval("cfIsEditable").ToString() == "False" ? ResolveUrl("~/Libs/Images/Button/delete_disabled.png") : ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                            alt="" />
                                                        <input type="hidden" value="" bindingfield="ID" />
                                                        <input type="hidden" value="<%#:Eval("SignaID") %>" bindingfield="SignaID" />
                                                        <input type="hidden" value="<%#:Eval("SignaLabel") %>" bindingfield="SignaLabel" />
                                                        <input type="hidden" value="<%#:Eval("SignaName1") %>" bindingfield="SignaName1" />
                                                        <input type="hidden" value="<%#:Eval("PrescriptionOrderDetailID") %>" bindingfield="PrescriptionOrderDetailID" />
                                                        <input type="hidden" value="<%#:Eval("GenericName") %>" bindingfield="GenericName" />
                                                        <input type="hidden" value="<%#:Eval("IsRFlag") %>" bindingfield="IsRFlag" />
                                                        <input type="hidden" value="<%#:Eval("IsCompound") %>" bindingfield="IsCompound" />
                                                        <input type="hidden" value="<%#:Eval("ItemCode") %>" bindingfield="ItemCode" />
                                                        <input type="hidden" value="<%#:Eval("ItemID") %>" bindingfield="ItemID" />
                                                        <input type="hidden" value="" bindingfield="LocationID" />
                                                        <input type="hidden" value="" bindingfield="GCItemUnit" />
                                                        <input type="hidden" value="" bindingfield="GCBaseUnit" />
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
                                                        <input type="hidden" value="<%#:Eval("StartDateInDatePickerFormat") %>" bindingfield="StartDateInDatePickerFormat" />
                                                        <input type="hidden" value="<%#:Eval("DispenseQty") %>" bindingfield="DispenseQty" />
                                                        <input type="hidden" value="<%#:Eval("TakenQty") %>" bindingfield="TakenQty" />
                                                        <input type="hidden" value="" bindingfield="ChargeClassID" />
                                                        <input type="hidden" value="" bindingfield="Tariff" />
                                                        <input type="hidden" value="" bindingfield="DiscountAmount" />
                                                        <input type="hidden" value="" bindingfield="PatientAmount" />
                                                        <input type="hidden" value="" bindingfield="PayerAmount" />
                                                        <input type="hidden" value="" bindingfield="LineAmount" />
                                                        <input type="hidden" value="<%#:Eval("EmbalaceID") %>" bindingfield="EmbalaceID" />
                                                        <input type="hidden" value="" bindingfield="EmbalaceName" />
                                                        <input type="hidden" value="" bindingfield="EmbalaceCode" />
                                                        <input type="hidden" value="" bindingfield="EmbalaceQty" />
                                                        <input type="hidden" value="" bindingfield="EmbalaceAmount" />
                                                        <input type="hidden" value="" bindingfield="PrescriptionFeeAmount" />
                                                        <input type="hidden" value="<%#:Eval("IsMorning") %>" bindingfield="IsMorning" />
                                                        <input type="hidden" value="<%#:Eval("IsNoon") %>" bindingfield="IsNoon" />
                                                        <input type="hidden" value="<%#:Eval("IsEvening") %>" bindingfield="IsEvening" />
                                                        <input type="hidden" value="<%#:Eval("IsNight") %>" bindingfield="IsNight" />
                                                        <input type="hidden" value="<%#:Eval("IsAsRequired") %>" bindingfield="IsAsRequired" />
                                                        <input type="hidden" value="<%#:Eval("IsUsingUDD") %>" bindingfield="IsUsingUDD" />
                                                    </td>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <div>
                                                                        <b>
                                                                            <%#: Eval("InformationLine1")%></b></div>
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
                                                                        <div style="color: Blue; float: left;">
                                                                            <%#: Eval("GenericName")%></div>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td align="right">           
                                                        <div><%#: Eval("Frequency")%></div>                                             
                                                    </td>
                                                    <td align="left">           
                                                        <div><%#: Eval("DosingFrequency")%></div>                                             
                                                    </td>
                                                    <td align="right">
                                                        <div><%#: Eval("cfNumberOfDosage")%></div>
                                                    </td>
                                                    <td align="left">
                                                        <div> <%#: Eval("DosingUnit")%></div>
                                                    </td>
                                                    <td align="right">
                                                        <div style="text-align: center;">
                                                            <asp:CheckBox ID="chkIsRequired" runat="server" Enabled="false" Checked='<%# Eval("IsAsRequired")%>' />
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div><%#: Eval("Route")%></div>
                                                    </td>
                                                    <td align="center">
                                                        <div><%#: Eval("StartDateInDatePickerFormat")%></div>
                                                    </td>
                                                    <td align="center">
                                                        <div style="text-align: center;">
                                                            <asp:CheckBox ID="chkIsMorning" runat="server" Enabled="false" Checked='<%# Eval("IsMorning")%>' />
                                                        </div>
                                                    </td>
                                                    <td align="center">
                                                        <div style="text-align: center;">
                                                            <asp:CheckBox ID="chkIsNoon" runat="server" Enabled="false" Checked='<%# Eval("IsNoon")%>' />
                                                        </div>
                                                    </td>
                                                    <td align="center">
                                                        <div style="text-align: center;">
                                                            <asp:CheckBox ID="chkEvening" runat="server" Enabled="false" Checked='<%# Eval("IsEvening")%>' />
                                                        </div>
                                                    </td>
                                                    <td align="center">
                                                        <div style="text-align: center;">
                                                            <asp:CheckBox ID="chkIsNight" runat="server" Enabled="false" Checked='<%# Eval("IsNight")%>' />
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div><%#: Eval("PrescriptionOrderStatus")%></div>
                                                    </td>
                                                    <td valign="middle">
                                                        <div style="text-align: center">
                                                        </div>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:ListView>
                                        <div style="width: 100%; text-align: center">
                                            <span class="lblLink" id="lblAddData" style="margin-right: 300px;">
                                                <%= GetLabel("Tambah Obat")%></span> <span class="lblLink" id="lblQuickPick">
                                                        <%= GetLabel("Quick Picks")%></span>
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
