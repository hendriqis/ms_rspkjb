<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx2.master"
    AutoEventWireup="true" CodeBehind="UDDMedicationOrderEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Pharmacy.Program.UDDMedicationOrderEntry" %>

<%@ Register Src="~/Libs/Controls/PatientBannerCtl.ascx" TagName="PatientBannerCtl"
    TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Src="~/Program/Prescription/UDD/UDDToolbarCtl.ascx" TagName="ToolbarCtl"
    TagPrefix="uc1" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnVoid" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Void")%></div>
    </li>
    <li id="btnRefresh" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbrefresh.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Refresh")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">
    <style type="text/css">
        #divCloseShortcutInformation
        {
            position: absolute;
            right: 10px;
            top: 5px;
            display: none;
            padding: 0px 5px;
            background: #CC0000;
            cursor: pointer;
            color: #FFFFFF;
            -webkit-border-radius: 999px;
            -moz-border-radius: 999px;
            border-radius: 999px;
            font-weight: bolder;
            font-size: 12px;
        }
        #divOpenShortcutInformation
        {
            cursor: pointer;
        }
        #divContainerShortcutInformation
        {
            bottom: 0;
            right: 470px;
            position: fixed;
            height: 30px;
            overflow: hidden;
        }
        #divContainerShortcutInformation > div
        {
            border: 1px solid #ecf0f1;
            padding: 5px;
            background-color: #bdc3c7;
            width: 650px;
            height: 550px;
            border-bottom: 0px;
            -webkit-border-radius: 5px;
            -moz-border-radius: 5px;
            border-radius: 5px;
        }
        
        div.containerInformationUl ul
        {
            margin: 0;
            padding: 0;
            margin-left: 25px;
        }
        div.containerInformationUl ul:not(:first-child)
        {
            margin-top: 10px;
        }
        div.containerInformationUl ul li
        {
            list-style-type: none;
            font-size: 12px;
            padding-bottom: 5px;
        }
        div.containerInformationUl ul li span
        {
            color: #3E3EE3;
        }
        div.containerInformationUl a
        {
            font-size: 12px;
            color: #3E93E3;
            cursor: pointer;
            float: right;
            margin-right: 20px;
        }
        div.containerInformationUl a:hover
        {
            text-decoration: underline;
        }
        div.headerHistoryContent
        {
            text-align: left;
            border-bottom: 1px groove black;
            font-size: 13px;
            margin: 0;
            padding: 0;
            margin-bottom: 5px;
        }
        
        .divInformationContentTitle
        {
            margin-left: 25px;
            font-weight: bold;
            font-size: 11px;
            text-decoration: underline;
        }
        .divInformationContent
        {
            margin-left: 25px;
            font-size: 12px;
        }
        
        .divNotAvailableContent
        {
            margin-left: 25px;
            font-size: 11px;
            font-style: italic;
            color: red;
        }
    </style>
    <input type="hidden" value="" id="hdnGCRegistrationStatus" runat="server" />
    <input type="hidden" value="" id="hdnModuleID" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnVisitID" runat="server" />
    <input type="hidden" value="" id="hdnClassID" runat="server" />
    <input type="hidden" value="0" id="hdnIsAllowOverIssued" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="1" id="hdnIsEditable" runat="server" />
    <input type="hidden" value="" id="hdnDefaultDispensaryServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnSelectedOrderID" runat="server" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript" id="UDDMedicationOrderEntry">
        function onGetDrugFilterExpression() {
            var LocationID = cboLocation.GetValue();
            var filterExpression = "LocationID = " + LocationID + " AND IsDeleted = 0 AND ISNULL(GCItemStatus,'') != 'X181^999'";
            if ($('#<%=hdnIsAllowOverIssued.ClientID %>').val() == '0') {
                filterExpression += " AND QuantityEND > 0 ";
            }
            return filterExpression;
        }

        function onLoad() {
            setDatePicker('<%=txtPrescriptionDate.ClientID %>');
            $('#<%=txtPrescriptionDate.ClientID %>').datepicker('option', 'maxDate', '0');
            setDatePicker('<%=txtStartDate.ClientID %>');

            var editable = "<%=IsEditable()%>";

            setRightPanelButtonEnabled();
            setCustomToolbarVisibility();

            $('#divOpenShortcutInformation').click(function () {
                $('#divContainerShortcutInformation').animate({ height: 550 }, 550, function () {
                    $('#divCloseShortcutInformation').show();
                });
            });


            $('#divCloseShortcutInformation').click(function () {
                $('#divContainerShortcutInformation').animate({ height: 30 }, 550, function () {
                    $('#divCloseShortcutInformation').hide();
                });
            });

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

            //#region PrescriptionNote
            $('#lblPrescriptionNote.lblLink').click(function () {
                alert('This feature is not available yet');
            });
            //#endregion

            //#region Drug
            $('#lblDrug.lblLink').click(function () {
                openSearchDialog('druginfowithbalance1', onGetDrugFilterExpression(), function (value) {
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
                        $('#<%=hdnTakenUnit.ClientID %>').val(result.GCItemUnit);
                        $('#<%=txtPurposeOfMedication.ClientID %>').val(result.MedicationPurpose);
                        cboDosingUnit.PerformCallback();

                        if (result.GCItemType == Constant.ItemType.BARANG_MEDIS) {
                            $('#<%=chkIsUsingUDD.ClientID %>').prop('checked', false);
                            $('#<%=chkIsUsingUDD.ClientID %>').attr('disabled', true);
                        }
                        else if (!result.IsAllowUDD) {
                            $('#<%=chkIsUsingUDD.ClientID %>').prop('checked', false);
                            $('#<%=chkIsUsingUDD.ClientID %>').attr('disabled', true);
                        }
                        else {
                            $('#<%=chkIsUsingUDD.ClientID %>').removeAttr('disabled');
                        }
                    }
                    else {
                        $('#<%=hdnDrugID.ClientID %>').val('');
                        $('#<%=txtDrugCode.ClientID %>').val('');
                        $('#<%=txtDrugName.ClientID %>').val('');
                        $('#<%=txtGenericName.ClientID %>').val('');
                        cboForm.SetValue('');
                        cboCoenamRule.SetValue('');
                        cboMedicationRoute.SetValue('');
                        cboDosingUnit.SetValue('');
                        $('#<%=txtStrengthAmount.ClientID %>').val('');
                        $('#<%=txtStrengthUnit.ClientID %>').val('');
                        $('#<%=hdnGCDoseUnit.ClientID %>').val('');
                        $('#<%=hdnGCItemUnit.ClientID %>').val('');
                        $('#<%=txtDispenseUnit.ClientID %>').val('');
                        $('#<%=hdnTakenUnit.ClientID %>').val('');
                        $('#<%=txtPurposeOfMedication.ClientID %>').val('');
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
                    SetMedicationDefaultTime($('#<%=txtFrequencyNumber.ClientID %>').val());
                });
            });

            $('#<%=txtSignaLabel.ClientID %>').change(function () {
                txtSignaLabelChanged($(this).val());
            });

            function txtSignaLabelChanged(value) {
                var itemID = $('#<%=hdnDrugID.ClientID %>').val();
                var filterExpression = "IsDeleted = 0 AND SignaLabel = '" + value + "'";
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
                calculateDispenseQty();
            }
            //#endregion

            $('#<%=btnVoid.ClientID %>').live('click', function (evt) {
                if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                    showDeleteConfirmation(function (data) {
                        var param = 'void;' + data.GCDeleteReason + ';' + data.Reason;
                        onCustomButtonClick(param);
                        onRefreshControl();
                    });
                }
            });

            $('#<%=btnRefresh.ClientID %>').live('click', function (evt) {
                onLoadObject($('#<%=txtPrescriptionOrderNo.ClientID %>').val());
            });

            function onAfterCustomClickSuccess(type) {
                onRefreshControl();
            }

            //#region Operasi
            $('#lblAddData').die('click');
            $('#lblAddData').live('click', function (evt) {
                if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                    var startDate = $('#<%=txtPrescriptionDate.ClientID %>').val();
                    var startTime = $('#<%=txtPrescriptionTime.ClientID %>').val();

                    $('#<%=lblPhysician.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%=txtPhysicianCode.ClientID %>').attr('readonly', 'readonly');
                    cboDispensaryUnit.SetEnabled(false);
                    cboLocation.SetEnabled(false);
                    $('#<%=txtPrescriptionDate.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtPrescriptionTime.ClientID %>').attr('readonly', 'readonly');

                    $('#containerEntry').show();
                    $('#<%=hdnEntryID.ClientID %>').val('');
                    $('#<%=txtGenericName.ClientID %>').val('');
                    $('#<%=hdnDrugID.ClientID %>').val('');
                    cboForm.SetValue('');
                    cboCoenamRule.SetValue('');
                    $('#<%=txtPurposeOfMedication.ClientID %>').val('');
                    $('#<%=txtStrengthAmount.ClientID %>').val('');
                    $('#<%=txtStrengthUnit.ClientID %>').val('');
                    cboFrequencyTimeline.SetValue(Constant.DosingFrequency.DAY);
                    $('#<%=txtFrequencyNumber.ClientID %>').val('1');
                    $('#<%=txtDosingDose.ClientID %>').val('1');
                    cboDosingUnit.SetValue('');
                    cboMedicationRoute.SetValue('');
                    $('#<%=txtMedicationAdministration.ClientID %>').val('');
                    $('#<%=txtStartDate.ClientID %>').val(startDate);
                    $('#<%=txtStartTime.ClientID %>').val(startTime);
                    $('#<%=txtDosingDuration.ClientID %>').val('1');
                    $('#<%=txtDrugName.ClientID %>').val('');
                    $('#<%=txtDrugCode.ClientID %>').val('');
                    $('#<%=hdnDrugID.ClientID %>').val('');
                    $('#<%=txtSignaLabel.ClientID %>').val('');
                    $('#<%=txtSignaName1.ClientID %>').val('');
                    $('#<%=hdnSignaID.ClientID %>').val('');
                    $('#<%=txtDispenseQty.ClientID %>').val('');
                    $('#<%=txtDispenseUnit.ClientID %>').val('');
                    $('#<%=txtTakenQty.ClientID %>').val('');
                    $('#<%=txtTakenUnit.ClientID %>').val('');
                    $('#<%=hdnTakenQty.ClientID %>').val('');
                    $('#<%=hdnTakenUnit.ClientID %>').val('');

                    if ((cboPrescriptionTypeMainPage.GetValue() != Constant.PrescriptionType.MEDICATION_ORDER) && (cboPrescriptionTypeMainPage.GetValue() != Constant.PrescriptionType.TERAPI_BARU) && (cboPrescriptionTypeMainPage.GetValue() != Constant.PrescriptionType.PASIEN_BARU)) {
                        $('#<%=chkIsUsingUDD.ClientID %>').prop('checked', false);
                        $('#<%=chkIsUsingUDD.ClientID %>').attr('disabled', true);
                        $('#<%=txtTakenQty.ClientID %>').removeAttr('disabled');
                    }
                    else {
                        $('#<%=chkIsUsingUDD.ClientID %>').prop('checked', true);
                        $('#<%=chkIsUsingUDD.ClientID %>').removeAttr('disabled');
                        $('#<%=txtTakenQty.ClientID %>').attr('disabled', true);
                    }

                    $('#<%=chkIsAsRequired.ClientID %>').prop('checked', false);
                    $('#<%=chkIsIMM.ClientID %>').prop('checked', false);

                    $('#<%=txtStartTime1.ClientID %>').val("08:00");
                    $('#<%=txtStartTime2.ClientID %>').val("-");
                    $('#<%=txtStartTime3.ClientID %>').val("-");
                    $('#<%=txtStartTime4.ClientID %>').val("-");
                    $('#<%=txtStartTime5.ClientID %>').val("-");
                    $('#<%=txtStartTime6.ClientID %>').val("-");

                    $('#<%=txtDrugCode.ClientID %>').focus();
                    calculateDispenseQty();
                }
            });

            $('#btnCancel').click(function () {
                $('#containerEntry').hide();
            });

            $('#btnSave').live('click', function (evt) {
                getSelectedCheckbox();
                if (evt) cbpProcess.PerformCallback('save');
            });
            //#endregion
        }

        function getSelectedCheckbox() {
            var time1 = $('#<%=txtStartTime1.ClientID %>').val();
            var time2 = $('#<%=txtStartTime2.ClientID %>').val();
            var time3 = $('#<%=txtStartTime3.ClientID %>').val();
            var time4 = $('#<%=txtStartTime4.ClientID %>').val();
            var time5 = $('#<%=txtStartTime5.ClientID %>').val();
            var time6 = $('#<%=txtStartTime6.ClientID %>').val();

            $('#<%=hdnSelectedTime1.ClientID %>').val(time1);
            $('#<%=hdnSelectedTime2.ClientID %>').val(time2);
            $('#<%=hdnSelectedTime3.ClientID %>').val(time3);
            $('#<%=hdnSelectedTime4.ClientID %>').val(time4);
            $('#<%=hdnSelectedTime5.ClientID %>').val(time5);
            $('#<%=hdnSelectedTime6.ClientID %>').val(time6);
        }

        $('.imgEdit.imgLink').die('click');
        $('.imgEdit.imgLink').live('click', function () {
            $row = $(this).parent().closest('tr');
            var entity = rowToObject($row);
            if (entity.IsCompound != 'True') {
                $('#<%=hdnEntryID.ClientID %>').val(entity.PrescriptionOrderDetailID);
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
                $('#<%=txtTakenQty.ClientID %>').val(entity.TakenQty);
                $('#<%=hdnTakenQty.ClientID %>').val(entity.TakenQty);
                $('#<%=hdnSignaID.ClientID %>').val(entity.SignaID);
                $('#<%=txtSignaName1.ClientID %>').val(entity.SignaName1);
                $('#<%=txtSignaLabel.ClientID %>').val(entity.SignaLabel);
                $('#<%=txtDispenseUnit.ClientID %>').val(entity.ItemUnit);
                $('#<%=txtTakenUnit.ClientID %>').val(entity.ItemUnit);
                $('#<%=hdnTakenUnit.ClientID %>').val(entity.GCItemUnit);

                if (entity.cfIsAsRequired == "1")
                    $('#<%=chkIsAsRequired.ClientID %>').prop('checked', true);
                else
                    $('#<%=chkIsAsRequired.ClientID %>').prop('checked', false);

                if (entity.cfIsIMM == "1")
                    $('#<%=chkIsIMM.ClientID %>').prop('checked', true);
                else
                    $('#<%=chkIsIMM.ClientID %>').prop('checked', false);

                if (entity.cfIsUsingUDD == "1")
                    $('#<%=chkIsUsingUDD.ClientID %>').prop('checked', true);
                else
                    $('#<%=chkIsUsingUDD.ClientID %>').prop('checked', false);

                if ((cboPrescriptionTypeMainPage.GetValue() != Constant.PrescriptionType.MEDICATION_ORDER) && (cboPrescriptionTypeMainPage.GetValue() != Constant.PrescriptionType.TERAPI_BARU) && (cboPrescriptionTypeMainPage.GetValue() != Constant.PrescriptionType.PASIEN_BARU)) {
                    $('#<%=chkIsUsingUDD.ClientID %>').prop('checked', false);
                    $('#<%=chkIsUsingUDD.ClientID %>').attr('disabled', true);
                    $('#<%=txtTakenQty.ClientID %>').removeAttr('disabled');
                }
                else {
                    if (entity.GCItemType == Constant.ItemType.BARANG_MEDIS) {
                        $('#<%=chkIsUsingUDD.ClientID %>').prop('checked', false);
                        $('#<%=chkIsUsingUDD.ClientID %>').attr('disabled', true);
                    }
                    else {
                        if (entity.cfIsAsRequired == "1") {
                            $('#<%=chkIsUsingUDD.ClientID %>').prop('checked', false);
                            $('#<%=chkIsUsingUDD.ClientID %>').attr('disabled', true);
                        }
                        else if (entity.cfIsIMM == "1") {
                            $('#<%=chkIsUsingUDD.ClientID %>').prop('checked', false);
                            $('#<%=chkIsUsingUDD.ClientID %>').attr('disabled', true);
                        }
                        else {
                            if (entity.cfIsUsingUDD == "1") {
                                $('#<%=chkIsUsingUDD.ClientID %>').prop('checked', true);
                                $('#<%=chkIsUsingUDD.ClientID %>').removeAttr('disabled');
                            }
                            else {
                                if (!entity.IsAllowUDD) {
                                    $('#<%=chkIsUsingUDD.ClientID %>').prop('checked', false);
                                    $('#<%=chkIsUsingUDD.ClientID %>').attr('disabled', true);
                                }
                                else {
                                    $('#<%=chkIsUsingUDD.ClientID %>').removeAttr('disabled');
                                }
                            }
                        }
                    }
                }

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
                $('#containerEntry').show();
            } else {
                $('#<%=hdnEntryID.ClientID %>').val(entity.PrescriptionOrderDetailID);
                openCompoundEntry('edit');
            }
        });

        $('.imgDelete.imgLink').die('click');
        $('.imgDelete.imgLink').live('click', function () {
            $row = $(this).parent().closest('tr');
            showToastConfirmation('Are You Sure Want To Delete?', function (result) {
                if (result) {
                    var entity = rowToObject($row);
                    $('#<%=hdnEntryID.ClientID %>').val(entity.PrescriptionOrderDetailID);
                    cbpProcess.PerformCallback('delete');
                }
            });
        });

        $('#lblAddCompound').live('click', function (evt) {
            if (IsValid(null, 'fsTrx', 'mpTrx')) {
                if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                    showLoadingPanel();
                    var url = ResolveUrl('~/Program/Prescription/UDD/MedicationOrder/UDDMedicationOrderCompoundEntryCtl1.ascx');
                    var transactionID = $('#<%=hdnPrescriptionOrderID.ClientID %>').val();
                    var locationID = cboLocation.GetValue();
                    var defaultGCMedicationRoute = '';
                    var paramedicID = $('#<%=hdnPhysicianID.ClientID %>').val();
                    var registration = $('#<%=hdnRegistrationID.ClientID %>').val();
                    var visitID = $('#<%=hdnVisitID.ClientID %>').val();
                    var chargeClassID = '';
                    var orderNotes = $('#<%=txtRemarks.ClientID %>').val();
                    var id = transactionID + '|' + locationID + '|' + defaultGCMedicationRoute + '|' + paramedicID + '|' + registration + '|' + visitID + '|' + chargeClassID + '|' + orderNotes;
                    openUserControlPopup(url, id, 'Compound Medication', 1250, 550);
                }
            }
        });

        $('#lblQuickPick').live('click', function (evt) {
            if (IsValid(null, 'fsTrx', 'mpTrx')) {
                if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                    showLoadingPanel();
                    var url = ResolveUrl('~/Program/Prescription/UDD/MedicationOrder/UDDMedicationOrderQuickPicksCtl.ascx');
                    var transactionID = $('#<%=hdnPrescriptionOrderID.ClientID %>').val();
                    var locationID = cboLocation.GetValue();
                    var defaultGCMedicationRoute = '';
                    var paramedicID = $('#<%=hdnPhysicianID.ClientID %>').val();
                    var registration = $('#<%=hdnRegistrationID.ClientID %>').val();
                    var visitID = $('#<%=hdnVisitID.ClientID %>').val();
                    var chargeClassID = '';
                    var orderNotes = $('#<%=txtRemarks.ClientID %>').val();
                    var presType = cboPrescriptionTypeMainPage.GetValue();

                    var id = transactionID + '|' + locationID + '|' + defaultGCMedicationRoute + '|' + paramedicID + '|' + registration + '|' + visitID + '|' + chargeClassID + '|' + orderNotes + '|' + presType;
                    openUserControlPopup(url, id, 'Quick Picks', 1300, 550);
                }
            }
        });

        $('#lblQuickPicksHistory').live('click', function (evt) {
            var transactionID = $('#<%=hdnPrescriptionOrderID.ClientID %>').val();
            if (transactionID != "0") {
                var messageBody = "Maaf, Fasilitas ini hanya untuk pembuatan Nota Transaksi Baru, Silahkan klik Tombol <b> New </b> terlebih dahulu.";
                displayMessageBox('ERROR : Quick Picks History', messageBody);
            } else {
                if (IsValid(null, 'fsTrx', 'mpTrx')) {
                    if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                        showLoadingPanel();
                        var url = ResolveUrl('~/Program/Prescription/UDD/MedicationOrder/UDDMedicationOrderQuickPicksHistoryCtl.ascx');
                        var transactionID = $('#<%=hdnPrescriptionOrderID.ClientID %>').val();
                        var locationID = cboLocation.GetValue();
                        var defaultGCMedicationRoute = '';
                        var paramedicID = $('#<%=hdnPhysicianID.ClientID %>').val();
                        var registration = $('#<%=hdnRegistrationID.ClientID %>').val();
                        var visitID = $('#<%=hdnVisitID.ClientID %>').val();
                        var chargeClassID = $('#<%=hdnClassID.ClientID %>').val();
                        var orderNotes = $('#<%=txtRemarks.ClientID %>').val();
                        var dispensaryUnitID = $('#<%=hdnDefaultDispensaryServiceUnitID.ClientID %>').val();
                        var prescriptionType = cboPrescriptionTypeMainPage.GetValue();
                        var date = Methods.dateToString(Methods.getDatePickerDate($('#<%=txtPrescriptionDate.ClientID %>').val()));
                        var time = $('#<%=txtPrescriptionTime.ClientID %>').val();

                        var id = transactionID + '|' + locationID + '|' + defaultGCMedicationRoute + '|' + paramedicID + '|' + registration + '|' + visitID + '|' + chargeClassID + '|' + orderNotes + '|' + dispensaryUnitID + '|' + date + '|' + time + '|' + prescriptionType;
                        openUserControlPopup(url, id, 'Quick Picks - Order History', 1200, 550);
                    }
                }
            }
        });

        function setCustomToolbarVisibility() {
            var orderStatus = $('#<%=hdnGCOrderStatus.ClientID %>').val();
            var isVoid = $('#<%:hdnIsAllowVoid.ClientID %>').val();
            if (isVoid == 1) {
                if (getIsAdd()) {
                    $('#<%=btnVoid.ClientID %>').hide();
                }
                else {
                    if (orderStatus == Constant.OrderStatus.OPEN || orderStatus == Constant.OrderStatus.RECEIVED) {
                        $('#<%=btnVoid.ClientID %>').show();
                    } else {
                        $('#<%=btnVoid.ClientID %>').hide();
                    }
                }
            } else {
                $('#<%=btnVoid.ClientID %>').hide();
            }
        }

        function onAfterSaveEditRecordEntryPopup(code, value, isAdd) {
            onLoadObject($('#<%=txtPrescriptionOrderNo.ClientID %>').val());
        }

        function onAfterProcessPopupEntry(PrescriptionOrderID) {
            onAfterSaveRecordDtSuccess(PrescriptionOrderID);
        }

        function onAfterSaveAddRecordEntryPopup(param) {
            onAfterSaveRecordDtSuccess(param);
        }

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
            }
            onLoadObject($('#<%=txtPrescriptionOrderNo.ClientID %>').val());
            setCustomToolbarVisibility();
        }

        function openCompoundEntry(value) {
            var prescriptionID = $('#<%=hdnPrescriptionOrderID.ClientID %>').val();
            var prescriptionDetailID = $('#<%=hdnEntryID.ClientID %>').val();
            var date = Methods.dateToString(Methods.getDatePickerDate($('#<%=txtPrescriptionDate.ClientID %>').val()));
            var time = $('#<%=txtPrescriptionTime.ClientID %>').val();
            var physician = $('#<%=hdnPhysicianID.ClientID %>').val();
            var visitID = $('#<%=hdnVisitID.ClientID %>').val();
            var classID = $('#<%=hdnClassID.ClientID %>').val();
            var prescriptionType = '';
            var refillInstruction = '';

            var queryString = "";
            queryString = value + '|' + prescriptionID + '|' + prescriptionDetailID + '|' + date + '|' + time + '|' + physician + '|' + refillInstruction + '|' + visitID + '|' + classID + '|' + cboDispensaryUnit.GetValue() + '|' + cboLocation.GetValue() + '|' + prescriptionType;

            var url = ResolveUrl("~/Program/Prescription/UDD/MedicationOrder/UDDMedicationOrderCompoundEntryCtl1.ascx");
            if (value == "edit") {
                url = ResolveUrl("~/Program/Prescription/UDD/MedicationOrder/UDDMedicationOrderCompoundEditCtl1.ascx");
            }
            openUserControlPopup(url, queryString, 'Compound Medication', 1100, 600);
        }

        function onCboDosingUnitEndCallback() {
            var gcDosingUnit = $('#<%=hdnGCDosingUnit.ClientID %>').val();
            cboDosingUnit.SetValue(gcDosingUnit);
        }

        $('#<%=txtDispenseQty.ClientID %>').live('change', function () {
            var dispQty = $('#<%=txtDispenseQty.ClientID %>').val();
            if (dispQty <= 0 || dispQty == "") {
                showToast('Error Message', 'Quantity should be greater than 0 !');
                $('#<%=txtDispenseQty.ClientID %>').val('');
            }
            $('#<%=txtTakenQty.ClientID %>').val($(this).val());
        });

        //#region calculate Dispense Qty
        $('#<%=txtFrequencyNumber.ClientID %>').live('change', function () {
            var value = parseInt($(this).val());
            if (value > 9) {
                $(this).val('1');
            }
            SetMedicationDefaultTime($('#<%=txtFrequencyNumber.ClientID %>').val());
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
            var frequency = $('#<%=txtFrequencyNumber.ClientID %>').val().replace(",", ".");
            var frequencyTimeLine = cboFrequencyTimeline.GetValue();
            var dose = $('#<%=txtDosingDose.ClientID %>').val().replace(",", ".");
            var dosingDuration = $('#<%=txtDosingDuration.ClientID %>').val().replace(",", ".");
            var strengthAmount = $('#<%=txtStrengthAmount.ClientID %>').val().replace(",", ".");
            var strengthUnit = $('#<%=txtStrengthUnit.ClientID %>').val().replace(",", ".");
            var dosingUnit = cboDosingUnit.GetText();
            var itemUnit = $('#<%=txtDispenseUnit.ClientID %>').val();

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

        function SetMedicationDefaultTime(frequency) {
            Methods.getMedicationSequenceTime(frequency, function (result) {
                if (result != null) {
                    var medicationTimeInfo = result.split('|');
                    $('#<%=txtStartTime.ClientID %>').val(medicationTimeInfo[0]);
                    $('#<%=txtStartTime1.ClientID %>').val(medicationTimeInfo[0]);
                    $('#<%=txtStartTime2.ClientID %>').val(medicationTimeInfo[1]);
                    $('#<%=txtStartTime3.ClientID %>').val(medicationTimeInfo[2]);
                    $('#<%=txtStartTime4.ClientID %>').val(medicationTimeInfo[3]);
                    $('#<%=txtStartTime5.ClientID %>').val(medicationTimeInfo[4]);
                    $('#<%=txtStartTime6.ClientID %>').val(medicationTimeInfo[5]);
                }
                else {
                    $('#<%=txtStartTime.ClientID %>').val();
                    $('#<%=txtStartTime1.ClientID %>').val('');
                    $('#<%=txtStartTime2.ClientID %>').val('');
                    $('#<%=txtStartTime3.ClientID %>').val('');
                    $('#<%=txtStartTime4.ClientID %>').val('');
                    $('#<%=txtStartTime5.ClientID %>').val('');
                    $('#<%=txtStartTime6.ClientID %>').val('');
                }
            });
        }

        function onCbpProcesEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'save') {
                if (param[1] == 'fail') {
                    if (param[2] == 'confirm') {
                        displayErrorMessageBox('SAVE', param[3]);
                        var PrescriptionOrderID = s.cpPrescriptionOrderID;
                        onAfterSaveRecordDtSuccess(PrescriptionOrderID);
                        $('#containerEntry').hide();
                    }
                    else displayErrorMessageBox('SAVE', param[3]);
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
                    displayErrorMessageBox('DELETE', param[2]);
                else
                    cbpView.PerformCallback('refresh');
            }
        }

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var prescriptionOrderID = $('#<%=hdnPrescriptionOrderID.ClientID %>').val();
            if (prescriptionOrderID == '' || prescriptionOrderID == '0') {
                errMessage.text = 'Transaksi Resep harap diselesaikan terlebih dahulu.';
                return false;
            }
            else {
                if (code == 'PH-00028' || code == 'PH-00012' || code == 'PH-00038' || code == 'PH-00044' || code == 'PH-00045' || code == 'PH-00048'
                 || code == 'PH-00053' || code == 'PH-00055' || code == 'PH-00060') {
                    filterExpression.text = prescriptionOrderID;
                    return true;
                }
            }
        }

        function setRightPanelButtonEnabled() {
            $('#btnScanPresciptionForm').attr('enabled', 'false');
            $('#btnUpdateUDDFlag').attr('enabled', 'false');
            $('#btnUpdateOrderStatus').attr('enabled', 'false');
            $('#btnReopenOrderStatus').attr('enabled', 'false');
            $('#btnDeleteMedicationSchedule').attr('enabled', 'false');

            var presID = $('#<%:hdnPrescriptionOrderID.ClientID %>').val();
            if (presID != '0') {
                $('#btnPrescriptionChecklist').removeAttr('enabled');
            }
            else {
                $('#btnPrescriptionChecklist').attr('enabled', 'false');
            }

            var presType = $('#<%=hdnGCPrescriptionType.ClientID %>').val();
            var transStatus = $('#<%=hdnGCTransactionStatus.ClientID %>').val();
            if (transStatus != Constant.TransactionStatus.VOID && transStatus != Constant.TransactionStatus.OPEN) {
                if (presType != Constant.PrescriptionType.DISCHARGE_PRESCRIPTION) {
                    $('#btnUpdateUDDFlag').removeAttr('enabled');
                }
                $('#btnUpdateOrderStatus').removeAttr('enabled');
                $('#btnReopenOrderStatus').removeAttr('enabled');
                $('#btnDeleteMedicationSchedule').removeAttr('enabled');
            }

            if ($('#<%=hdnIsHasPPRAItem.ClientID %>').val() == "1") {
                $('#btnViewPPRAForm').removeAttr('enabled');
            }
            else {
                $('#btnViewPPRAForm').attr('enabled', 'false');
            }
        }

        function onBeforeLoadRightPanelContent(code) {
            if (code == 'orderChangesLog' || code == 'notificationCPPTLog') {
                var param = $('#<%:hdnRegistrationID.ClientID %>').val() + '|' + $('#<%:hdnPrescriptionOrderID.ClientID %>').val() + '|' + cboDispensaryUnit.GetValue();
                return param;
            }
            else if (code == 'changeUDDStatus') {
                if ((cboPrescriptionTypeMainPage.GetValue() == Constant.PrescriptionType.MEDICATION_ORDER) || (cboPrescriptionTypeMainPage.GetValue() == Constant.PrescriptionType.TERAPI_BARU) || (cboPrescriptionTypeMainPage.GetValue() == Constant.PrescriptionType.PASIEN_BARU)) {
                    var param = $('#<%:hdnRegistrationID.ClientID %>').val() + '|' + $('#<%:hdnPrescriptionOrderID.ClientID %>').val() + '|' + $('#<%:txtPrescriptionOrderNo.ClientID %>').val();
                    return param;
                }
                else {
                    displayMessageBox('UBAH STATUS UDD', "Ubah status UDD hanya berlaku untuk <b>jenis resep Medication Order / Terapi Baru</b>");
                }
            }
            else if (code == 'changeOrderStatus') {
                var param = $('#<%:hdnRegistrationID.ClientID %>').val() + '|' + $('#<%:hdnPrescriptionOrderID.ClientID %>').val() + '|' + $('#<%:txtPrescriptionOrderNo.ClientID %>').val();
                return param;
            }
            else if (code == 'reopenOrderStatus') {
                var param = $('#<%:hdnRegistrationID.ClientID %>').val() + '|' + $('#<%:hdnPrescriptionOrderID.ClientID %>').val() + '|' + $('#<%:txtPrescriptionOrderNo.ClientID %>').val();
                return param;
            }
            else if (code == 'registrationNotes' || code == 'transactionNotes') {
                return $('#<%:hdnRegistrationID.ClientID %>').val();
            }
            else if (code == 'infoInstruction') {
                var param = $('#<%:hdnVisitID.ClientID %>').val();
                return param;
            }
            else if (code == 'deleteMedicationSchedule') {
                var param = $('#<%:hdnVisitID.ClientID %>').val() + '|' + "0" + '|' + "0";
                return param;
            }
            else if (code == 'prescriptionChecklist') {
                return "0" + '|' + $('#<%:hdnPrescriptionOrderID.ClientID %>').val() + '|' + $('#<%:hdnIsReviewPrescriptionMandatoryForProposedTransaction.ClientID %>').val();
            }
            else if (code == 'viewPPRAForm') {
                if (($('#<%:hdnPrescriptionOrderID.ClientID %>').val() != "0") && ($('#<%:hdnPrescriptionOrderID.ClientID %>').val() != "")) {
                    if ($('#<%=hdnIsHasPPRAItem.ClientID %>').val() != "1") {
                        displayMessageBox('Formulir PPRA', "Nomor resep ini tidak memiliki item yang termasuk dalam <b>Kelompok Antibiotik yang termasuk dalam kategori PPRA</b>");
                    }
                    var param = $('#<%:hdnPrescriptionOrderID.ClientID %>').val();
                    return param;
                }
                else {
                    displayMessageBox('Formulir PPRA', "Nomor resep tidak valid.");
                }
            }
        }

        function onAfterPopupControlClosing() {
        }

        function onAfterUpdateUDDStatus(param) {
            $('#<%=hdnIsRefreshPageWithID.ClientID %>').val("1");
            $('#<%=hdnPrescriptionOrderID.ClientID %>').val(param);
            cbpView.PerformCallback('refresh');
        }

        $('#imgDrugAlertMain').live('click', function () {
            var url = ResolveUrl("~/Libs/Program/Information/DrugAlertInformationCtl.ascx");
            var id = $('#<%:hdnPrescriptionOrderID.ClientID %>').val() + "|0";
            openUserControlPopup(url, id, 'Drug Alert Information', 700, 600);
        });
    </script>
    <input type="hidden" value="" id="hdnPrescriptionOrderID" runat="server" />
    <input type="hidden" value="" id="hdnDefaultVisitParamedicID" runat="server" />
    <input type="hidden" value="" id="hdnDefaultVisitParamedicCode" runat="server" />
    <input type="hidden" value="" id="hdnDefaultVisitParamedicName" runat="server" />
    <input type="hidden" value="" id="hdnPhysicianID" runat="server" />
    <input type="hidden" value="" id="hdnGCDoseUnit" runat="server" />
    <input type="hidden" value="" id="hdnGCItemUnit" runat="server" />
    <input type="hidden" value="" id="hdnGCDosingUnit" runat="server" />
    <input type="hidden" value="" id="hdnTakenQty" runat="server" />
    <input type="hidden" value="" id="hdnTakenUnit" runat="server" />
    <input type="hidden" value="" id="hdnPrescriptionFeeAmount" runat="server" />
    <input type="hidden" value="" id="hdnIsAllowVoid" runat="server" />
    <input type="hidden" value="" id="hdnGCOrderStatus" runat="server" />
    <input type="hidden" value="" id="hdnGCTransactionStatus" runat="server" />
    <input type="hidden" value="" id="hdnGCPrescriptionType" runat="server" />
    <input type="hidden" value="" id="hdnIsRefreshPageWithID" runat="server" />
    <input type="hidden" value="0" id="hdnIsEndingAmountRoundingTo100" runat="server" />
    <input type="hidden" value="0" id="hdnIsEndingAmountRoundingTo1" runat="server" />
    <input type="hidden" id="hdnIsReviewPrescriptionMandatoryForProposedTransaction"
        runat="server" value="0" />
    <input type="hidden" value="1" id="hdnIsUsingDrugAlertMain" runat="server" />
    <input type="hidden" value="" id="hdnIsHasPPRAItem" runat="server" />
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
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="padding-right: 1px; width: 145px">
                                            <asp:TextBox ID="txtPrescriptionOrderNo" Width="233px" ReadOnly="true" runat="server" />
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
                                <dxe:ASPxComboBox ID="cboPrescriptionTypeMainPage" ClientInstanceName="cboPrescriptionTypeMainPage"
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
                                <label class="lblLink" title="View Physician Prescription (Scanned)" id="lblPrescriptionNote">
                                    <%=GetLabel("Catatan Order") %></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRemarks" Width="100%" runat="server" TextMode="MultiLine" Height="90px" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table border="0" cellpadding="1" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30px" />
                                        <col style="width: 40px" />
                                        <col style="width: 30px" />
                                        <col style="width: 40px" />
                                        <col style="width: 30px" />
                                        <col style="width: 40px" />
                                        <col style="width: 30px" />
                                        <col style="width: 40px" />
                                        <col style="width: 30px" />
                                        <col style="width: 40px" />
                                        <col style="width: 30px" />
                                        <col style="width: 40px" />
                                        <col style="width: 30px" />
                                        <col style="width: 40px" />
                                        <col style="width: 30px" />
                                        <col style="width: 40px" />
                                        <col style="width: 30px" />
                                        <col style="width: 40px" />
                                        <col style="width: 30px" />
                                        <col style="width: 40px" />
                                        <col style="width: 30px" />
                                        <col style="width: 40px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIsCorrectPatient" runat="server" ToolTip="Benar Pasien" />
                                        </td>
                                        <td style="font-weight: bold;">
                                            <asp:Label ID="Label1" runat="server" ToolTip="Benar Pasien" Text="Px">Px</asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkIsCorrectMedication" runat="server" ToolTip="Benar Obat" />
                                        </td>
                                        <td style="font-weight: bold;">
                                            <asp:Label ID="Label2" runat="server" ToolTip="Benar Obat" Text="OB">OB</asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkIsCorrectStrength" runat="server" ToolTip="Benar Kekuatan Obat" />
                                        </td>
                                        <td style="font-weight: bold;">
                                            <asp:Label ID="Label3" runat="server" ToolTip="Benar Kekuatan Obat" Text="KE">KE</asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkIsCorrectFrequency" runat="server" ToolTip="Benar Frekuensi Pemberian" />
                                        </td>
                                        <td style="font-weight: bold;">
                                            <asp:Label ID="Label4" runat="server" ToolTip="Benar Frekuensi Pemberian" Text="FRE">FRE</asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkIsCorrectDosage" runat="server" ToolTip="Benar Dosis" />
                                        </td>
                                        <td style="font-weight: bold;">
                                            <asp:Label ID="Label5" runat="server" ToolTip="Benar Dosis" Text="DO">DO</asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkIsCorrectRoute" runat="server" ToolTip="Benar Rute Pemberian" />
                                        </td>
                                        <td style="font-weight: bold;">
                                            <asp:Label ID="Label6" runat="server" ToolTip="Benar Rute Pemberian" Text="RP">RP</asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkIsHasDrugInteraction" runat="server" ToolTip="ada tidaknya interaksi obat" />
                                        </td>
                                        <td style="font-weight: bold;">
                                            <asp:Label ID="Label7" runat="server" ToolTip="ada tidaknya interaksi obat" Text="IO">IO</asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkIsHasDuplication" runat="server" ToolTip="ada tidaknya duplikasi obat" />
                                        </td>
                                        <td style="font-weight: bold;">
                                            <asp:Label ID="Label8" runat="server" ToolTip="ada tidaknya duplikasi obat" Text="IO">DUP</asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkIsADChecked" runat="server" ToolTip="(AD)" />
                                        </td>
                                        <td style="font-weight: bold;">
                                            <asp:Label ID="Label9" runat="server" ToolTip="(AD)" Text="AD">AD</asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkIsFARChecked" runat="server" ToolTip="(FAR)" />
                                        </td>
                                        <td style="font-weight: bold;">
                                            <asp:Label ID="Label10" runat="server" ToolTip="(FAR)" Text="FAR">FAR</asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkIsKLNChecked" runat="server" ToolTip="(KLN)" />
                                        </td>
                                        <td style="font-weight: bold;">
                                            <asp:Label ID="Label11" runat="server" ToolTip="(KLN)" Text="FAR">KLN</asp:Label>
                                        </td>
                                        <td />
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div id="containerEntry" style="margin-top: 4px; display: none;">
                        <div class="pageTitle">
                            <%=GetLabel("Data Obat yang diresepkan")%></div>
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
                                                    <asp:TextBox ID="txtDrugName" ReadOnly="true" CssClass="required" Width="100%" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Nama Generik")%></label>
                                                </td>
                                                <td colspan="6">
                                                    <asp:TextBox runat="server" ID="txtGenericName" Width="100%" />
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
                                                    <label class="lblNormal">
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
                                                        <%=GetLabel("Aturan Pakai")%></label>
                                                </td>
                                                <td colspan="2">
                                                    <input type="hidden" value="" id="hdnSignaID" runat="server" />
                                                    <asp:TextBox runat="server" ID="txtSignaLabel" Width="100%" />
                                                </td>
                                                <td colspan="4">
                                                    <asp:TextBox runat="server" ID="txtSignaName1" Width="100%" ReadOnly="true" />
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
                                                        <ClientSideEvents EndCallback="onCboDosingUnitEndCallback" ValueChanged="function(s,e){ cboDosingUnitChanged(); }" />
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
                                                <td style="display: none">
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
                                                        TabIndex="999" Text="hari" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("Jumlah Resep")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtDispenseQty" Width="100%" CssClass="number" max="10000" />
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtDispenseUnit" Width="100%" ReadOnly="true" />
                                                </td>
                                                <td class="tdLabel" style="padding-left: 10px" colspan="2">
                                                    <asp:CheckBox runat="server" ID="chkIsUsingUDD" Text=' UDD' />
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkIsAsRequired" runat="server" Text=" PRN" Checked="false" />
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkIsIMM" runat="server" Text=" IMM" Checked="false" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("Jumlah Diambil")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtTakenQty" Width="100%" CssClass="number" max="10000"
                                                        Enabled="false" />
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtTakenUnit" Width="100%" ReadOnly="true" />
                                                </td>
                                                <td class="tdLabel" style="padding-left: 10px" colspan="2">
                                                </td>
                                                <td>
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
                                                            <td style="width: 15%" align="center">
                                                                <label class="lblNormal">
                                                                    1</label>
                                                            </td>
                                                            <td style="width: 15%" align="center">
                                                                <label class="lblNormal">
                                                                    2</label>
                                                            </td>
                                                            <td style="width: 15%" align="center">
                                                                <label class="lblNormal">
                                                                    3</label>
                                                            </td>
                                                            <td style="width: 15%" align="center">
                                                                <label class="lblNormal">
                                                                    4</label>
                                                            </td>
                                                            <td style="width: 15%" align="center">
                                                                <label class="lblNormal">
                                                                    5</label>
                                                            </td>
                                                            <td style="width: 15%" align="center">
                                                                <label class="lblNormal">
                                                                    6</label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 15%">
                                                                <asp:TextBox runat="server" ID="txtStartTime1" Style="text-align: center" CssClass="time"
                                                                    Width="100%" Text="00:00" />
                                                            </td>
                                                            <td style="width: 15%">
                                                                <asp:TextBox runat="server" ID="txtStartTime2" Style="text-align: center" CssClass="time"
                                                                    Width="100%" Text="00:00" />
                                                            </td>
                                                            <td style="width: 15%">
                                                                <asp:TextBox runat="server" ID="txtStartTime3" Style="text-align: center" CssClass="time"
                                                                    Width="100%" Text="00:00" />
                                                            </td>
                                                            <td style="width: 15%">
                                                                <asp:TextBox runat="server" ID="txtStartTime4" Style="text-align: center" CssClass="time"
                                                                    Width="100%" Text="00:00" />
                                                            </td>
                                                            <td style="width: 15%">
                                                                <asp:TextBox runat="server" ID="txtStartTime5" Style="text-align: center" CssClass="time"
                                                                    Width="100%" Text="00:00" />
                                                            </td>
                                                            <td style="width: 15%">
                                                                <asp:TextBox runat="server" ID="txtStartTime6" Style="text-align: center" CssClass="time"
                                                                    Width="100%" Text="00:00" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel(" AC/DC/PC")%></label>
                                                </td>
                                                <td colspan="2">
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
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Instruksi Khusus")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtMedicationAdministration" Width="100%" Height="100px" runat="server"
                                                        TextMode="MultiLine" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Alasan Pengobatan")%>
                                                        <br />
                                                        <%=GetLabel("(Fungsi Obat)")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPurposeOfMedication" Width="100%" Height="100px" runat="server"
                                                        TextMode="MultiLine" />
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
                                                                <input type="button" id="btnSave" class="w3-btn w3-hover-blue" value='<%= GetLabel("Save")%>' />
                                                            </td>
                                                            <td>
                                                                <input type="button" id="btnCancel" class="w3-btn w3-hover-blue" value='<%= GetLabel("Cancel")%>' />
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
                    <input type="hidden" runat="server" id="hdnSelectedID" value="" />
                    <input type="hidden" runat="server" id="hdnSelectedTime1" value="" />
                    <input type="hidden" runat="server" id="hdnSelectedTime2" value="" />
                    <input type="hidden" runat="server" id="hdnSelectedTime3" value="" />
                    <input type="hidden" runat="server" id="hdnSelectedTime4" value="" />
                    <input type="hidden" runat="server" id="hdnSelectedTime5" value="" />
                    <input type="hidden" runat="server" id="hdnSelectedTime6" value="" />
                    <input type="hidden" runat="server" id="hdnSelectedStartDate" value="" />
                    <input type="hidden" runat="server" id="hdnSelectedDuration" value="" />
                    <input type="hidden" runat="server" id="hdnSelectedFrequency" value="" />
                    <input type="hidden" runat="server" id="hdnSelectedQuantity" value="" />
                    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
                    <input type="hidden" id="hdnPrescriptionFlag" runat="server" value="" />
                    <div style="position: relative;">
                        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                            <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { hideLoadingPanel(); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height: 300px">
                                        <asp:ListView ID="lvwView" runat="server">
                                            <EmptyDataTemplate>
                                                <table id="tblView" runat="server" class="grdNormal notAllowSelect" cellspacing="0"
                                                    rules="all">
                                                    <tr>
                                                        <th rowspan="2" style="width: 80px;">
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
                                                        <th rowspan="2" style="width: 30px;">
                                                            <div>
                                                                <%=GetLabel("Rak")%></div>
                                                        </th>
                                                        <th rowspan="2" style="width: 30px;">
                                                            <div>
                                                                <%=GetLabel("UDD")%></div>
                                                        </th>
                                                        <th colspan="7" align="center">
                                                            <div>
                                                                <%=GetLabel("Signa")%></div>
                                                        </th>
                                                        <th rowspan="2" align="center" style="padding: 3px; width: 90px;">
                                                            <div>
                                                                <%=GetLabel("Start Date")%></div>
                                                        </th>
                                                        <th rowspan="2" align="right" style="padding: 3px; width: 60px;">
                                                            <div>
                                                                <%=GetLabel("Duration")%></div>
                                                        </th>
                                                        <th rowspan="2" align="right" style="padding: 3px; width: 60px;">
                                                            <div>
                                                                <%=GetLabel("Quantity")%></div>
                                                        </th>
                                                        <th colspan="6">
                                                            <div>
                                                                <%=GetLabel("Medication Time") %></div>
                                                        </th>
                                                        <th rowspan="2" align="center" style="padding: 3px; width: 90px;">
                                                            <div>
                                                                <%=GetLabel("End Date")%></div>
                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        <th style="width: 40px;">
                                                            <div style="text-align: right">
                                                                <%=GetLabel("Frequency") %></div>
                                                        </th>
                                                        <th style="width: 40px; text-align: left">
                                                            <div>
                                                                <%=GetLabel("Timeline") %></div>
                                                        </th>
                                                        <th style="width: 40px;">
                                                            <div style="text-align: right">
                                                                <%=GetLabel("Dose") %></div>
                                                        </th>
                                                        <th style="width: 50px;">
                                                            <div style="text-align: left">
                                                                <%=GetLabel("Unit") %></div>
                                                        </th>
                                                        <th align="center" style="width: 30px;">
                                                            <div>
                                                                <%=GetLabel("PRN")%></div>
                                                        </th>
                                                        <th align="center" style="width: 30px;">
                                                            <div>
                                                                <%=GetLabel("IMM")%></div>
                                                        </th>
                                                        <th style="width: 80px;">
                                                            <div style="text-align: left">
                                                                <%=GetLabel("Route") %></div>
                                                        </th>
                                                        <th style="width: 30px;">
                                                            <div>
                                                                <%=GetLabel("1") %></div>
                                                        </th>
                                                        <th style="width: 30px;">
                                                            <div>
                                                                <%=GetLabel("2") %></div>
                                                        </th>
                                                        <th style="width: 30px;">
                                                            <div>
                                                                <%=GetLabel("3") %></div>
                                                        </th>
                                                        <th style="width: 30px;">
                                                            <div>
                                                                <%=GetLabel("4") %></div>
                                                        </th>
                                                        <th style="width: 30px;">
                                                            <div>
                                                                <%=GetLabel("5") %></div>
                                                        </th>
                                                        <th style="width: 30px;">
                                                            <div>
                                                                <%=GetLabel("6") %></div>
                                                        </th>
                                                    </tr>
                                                    <tr runat="server" id="itemPlaceholder">
                                                    </tr>
                                                    <tr class="trFooter">
                                                        <td colspan="30">
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
                                                        <th rowspan="2" style="width: 80px;">
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
                                                        <th rowspan="2" style="width: 30px;">
                                                            <div>
                                                                <%=GetLabel("Rak")%></div>
                                                        </th>
                                                        <th rowspan="2" style="width: 30px;">
                                                            <div>
                                                                <%=GetLabel("UDD")%></div>
                                                        </th>
                                                        <th colspan="7" align="center">
                                                            <div>
                                                                <%=GetLabel("Signa")%></div>
                                                        </th>
                                                        <th rowspan="2" align="center" style="padding: 3px; width: 90px;">
                                                            <div>
                                                                <%=GetLabel("Start Date")%></div>
                                                        </th>
                                                        <th rowspan="2" align="right" style="padding: 3px; width: 60px;">
                                                            <div>
                                                                <%=GetLabel("Duration")%></div>
                                                        </th>
                                                        <th rowspan="2" align="right" style="padding: 3px; width: 60px;">
                                                            <div>
                                                                <%=GetLabel("Order Quantity")%></div>
                                                        </th>
                                                        <th rowspan="2" align="right" style="padding: 3px; width: 60px;">
                                                            <div>
                                                                <%=GetLabel("Taken Quantity")%></div>
                                                        </th>
                                                        <th colspan="6">
                                                            <div>
                                                                <%=GetLabel("Medication Time") %></div>
                                                        </th>
                                                        <th rowspan="2" align="center" style="padding: 3px; width: 90px;">
                                                            <div>
                                                                <%=GetLabel("End Date")%></div>
                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        <th style="width: 40px;">
                                                            <div style="text-align: right">
                                                                <%=GetLabel("Frequency") %></div>
                                                        </th>
                                                        <th style="width: 40px; text-align: left">
                                                            <div>
                                                                <%=GetLabel("Timeline") %></div>
                                                        </th>
                                                        <th style="width: 40px;">
                                                            <div style="text-align: right">
                                                                <%=GetLabel("Dose") %></div>
                                                        </th>
                                                        <th style="width: 50px;">
                                                            <div style="text-align: left">
                                                                <%=GetLabel("Unit") %></div>
                                                        </th>
                                                        <th align="center" style="width: 30px;">
                                                            <div>
                                                                <%=GetLabel("PRN")%></div>
                                                        </th>
                                                        <th align="center" style="width: 30px;">
                                                            <div>
                                                                <%=GetLabel("IMM")%></div>
                                                        </th>
                                                        <th style="width: 80px;">
                                                            <div style="text-align: left">
                                                                <%=GetLabel("Route") %></div>
                                                        </th>
                                                        <th style="width: 30px;">
                                                            <div>
                                                                <%=GetLabel("1") %></div>
                                                        </th>
                                                        <th style="width: 30px;">
                                                            <div>
                                                                <%=GetLabel("2") %></div>
                                                        </th>
                                                        <th style="width: 30px;">
                                                            <div>
                                                                <%=GetLabel("3") %></div>
                                                        </th>
                                                        <th style="width: 30px;">
                                                            <div>
                                                                <%=GetLabel("4") %></div>
                                                        </th>
                                                        <th style="width: 30px;">
                                                            <div>
                                                                <%=GetLabel("5") %></div>
                                                        </th>
                                                        <th style="width: 30px;">
                                                            <div>
                                                                <%=GetLabel("6") %></div>
                                                        </th>
                                                    </tr>
                                                    <tr runat="server" id="itemPlaceholder">
                                                    </tr>
                                                    <tr class="trFooter">
                                                        <td colspan="30">
                                                            <div style="text-align: right; padding: 0px 3px">
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td>
                                                        <img class="imgEdit <%# IsEditable().ToString() == "False" || Eval("cfIsEditable").ToString() == "False" ? "imgDisabled" : "imgLink"%>"
                                                            src='<%# IsEditable().ToString() == "False" || Eval("cfIsEditable").ToString() == "False" ? ResolveUrl("~/Libs/Images/Button/edit_disabled.png") : ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                            alt="" style="float: left; margin-left: 7px" />
                                                        &nbsp;
                                                        <img class="imgDelete <%# IsEditable().ToString() == "False" || Eval("cfIsEditable").ToString() == "False" ? "imgDisabled" : "imgLink"%>"
                                                            src='<%# IsEditable().ToString() == "False" || Eval("cfIsEditable").ToString() == "False" ? ResolveUrl("~/Libs/Images/Button/delete_disabled.png") : ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                            alt="" />
                                                        <input type="hidden" value="" bindingfield="ID" />
                                                        <input type="hidden" value="<%#:Eval("SignaID") %>" bindingfield="SignaID" />
                                                        <input type="hidden" value="<%#:Eval("SignaLabel") %>" bindingfield="SignaLabel" />
                                                        <input type="hidden" value="<%#:Eval("SignaName1") %>" bindingfield="SignaName1" />
                                                        <input type="hidden" value="<%#:Eval("PrescriptionOrderDetailID") %>" bindingfield="PrescriptionOrderDetailID" />
                                                        <input type="hidden" value="<%#:Eval("GenericName") %>" bindingfield="GenericName" />
                                                        <input type="hidden" value="<%#:Eval("IsRFlag") %>" bindingfield="IsRFlag" />
                                                        <input type="hidden" value="<%#:Eval("IsCompound") %>" bindingfield="IsCompound" />
                                                        <input type="hidden" value="<%#:Eval("GCItemType") %>" bindingfield="GCItemType" />
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
                                                        <input type="hidden" value="<%#:Eval("GCItemUnit") %>" bindingfield="GCItemUnit" />
                                                        <input type="hidden" value="<%#:Eval("ItemUnit") %>" bindingfield="ItemUnit" />
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
                                                        <input type="hidden" value="<%#:Eval("IsIMM") %>" bindingfield="IsIMM" />
                                                        <input type="hidden" value="<%#:Eval("IsUsingUDD") %>" bindingfield="IsUsingUDD" />
                                                        <input type="hidden" value="<%#:Eval("cfIsAsRequired") %>" bindingfield="cfIsAsRequired" />
                                                        <input type="hidden" value="<%#:Eval("cfIsIMM") %>" bindingfield="cfIsIMM" />
                                                        <input type="hidden" value="<%#:Eval("cfIsUsingUDD") %>" bindingfield="cfIsUsingUDD" />
                                                        <input type="hidden" value="<%#:Eval("IsAllowUDD") %>" bindingfield="IsAllowUDD" />
                                                        <input type="hidden" value="<%#:Eval("Sequence1Time") %>" bindingfield="Sequence1Time" />
                                                        <input type="hidden" value="<%#:Eval("Sequence2Time") %>" bindingfield="Sequence2Time" />
                                                        <input type="hidden" value="<%#:Eval("Sequence3Time") %>" bindingfield="Sequence3Time" />
                                                        <input type="hidden" value="<%#:Eval("Sequence4Time") %>" bindingfield="Sequence4Time" />
                                                        <input type="hidden" value="<%#:Eval("Sequence5Time") %>" bindingfield="Sequence5Time" />
                                                        <input type="hidden" value="<%#:Eval("Sequence6Time") %>" bindingfield="Sequence6Time" />
                                                    </td>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <div>
                                                                        <b>
                                                                            <%#: Eval("cfMedicationName")%></b></div>
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
                                                                        <div style="color: Blue; float: left;">
                                                                            <%#: Eval("GenericName")%></div>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td align="center">
                                                        <div style="text-align: center;">
                                                            <%#: Eval("BinLocationCode")%></div>
                                                        </div>
                                                    </td>
                                                    <td align="center">
                                                        <div style="text-align: center;">
                                                            <asp:CheckBox ID="chkIsUsingUDD" runat="server" Enabled="false" Checked='<%# Eval("IsUsingUDD")%>' />
                                                        </div>
                                                    </td>
                                                    <td align="right">
                                                        <div>
                                                            <%#: Eval("Frequency")%></div>
                                                    </td>
                                                    <td align="left">
                                                        <div>
                                                            <%#: Eval("DosingFrequency")%></div>
                                                    </td>
                                                    <td align="right">
                                                        <div>
                                                            <%#: Eval("cfNumberOfDosage")%></div>
                                                    </td>
                                                    <td align="left">
                                                        <div>
                                                            <%#: Eval("DosingUnit")%></div>
                                                    </td>
                                                    <td align="right">
                                                        <div style="text-align: center;">
                                                            <asp:CheckBox ID="chkIsRequired" runat="server" Enabled="false" Checked='<%# Eval("IsAsRequired")%>' />
                                                        </div>
                                                    </td>
                                                    <td align="right">
                                                        <div style="text-align: center;">
                                                            <asp:CheckBox ID="chkIMM" runat="server" Enabled="false" Checked='<%# Eval("IsIMM")%>' />
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div>
                                                            <%#: Eval("Route")%></div>
                                                    </td>
                                                    <td align="center">
                                                        <div>
                                                            <%#: Eval("StartDateInDatePickerFormat")%></div>
                                                    </td>
                                                    <td align="right">
                                                        <div>
                                                            <%#: Eval("DosingDuration")%></div>
                                                    </td>
                                                    <td align="right">
                                                        <div>
                                                            <%#: Eval("DispenseQtyInString")%></div>
                                                    </td>
                                                    <td align="right">
                                                        <div>
                                                            <%#: Eval("cfTakenQty")%></div>
                                                    </td>
                                                    <td align="center">
                                                        <div style="text-align: center;">
                                                            <%#: Eval("Sequence1Time")%>
                                                        </div>
                                                    </td>
                                                    <td align="center">
                                                        <div style="text-align: center;">
                                                            <%#: Eval("Sequence2Time")%>
                                                        </div>
                                                    </td>
                                                    <td align="center">
                                                        <div style="text-align: center;">
                                                            <%#: Eval("Sequence3Time")%>
                                                        </div>
                                                    </td>
                                                    <td align="center">
                                                        <div style="text-align: center;">
                                                            <%#: Eval("Sequence4Time")%>
                                                        </div>
                                                    </td>
                                                    <td align="center">
                                                        <div style="text-align: center;">
                                                            <%#: Eval("Sequence5Time")%>
                                                        </div>
                                                    </td>
                                                    <td align="center">
                                                        <div style="text-align: center;">
                                                            <%#: Eval("Sequence6Time")%>
                                                        </div>
                                                    </td>
                                                    <td align="center">
                                                        <div>
                                                            <%#: Eval("cfEndDate")%></div>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:ListView>
                                        <div class="imgLoadingGrdView" id="imgLoadingGrdViewList">
                                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                        </div>
                                        <div class="imgLoadingGrdView" id="containerImgLoadingView">
                                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                        </div>
                                        <div style='width: 100%; text-align: center;'>
                                            <span class="lblLink" style="margin-right: 100px; <%=IsEditable().ToString() == "False" ? "display:none": "" %>"
                                                id="lblAddData">
                                                <%= GetLabel("Tambah Data")%></span> <span class="lblLink" style="<%=IsEditable().ToString() == "False" ? "display:none": "" %>"
                                                    id="lblAddCompound">
                                                    <%= GetLabel("Tambah Racikan")%></span> <span class="lblLink" style="margin-left: 100px;
                                                        <%=IsEditable().ToString() == "False" ? "display:none": "" %>" id="lblQuickPick">
                                                        <%= GetLabel("Quick Picks")%></span> <span class="lblLink" style="margin-left: 100px;
                                                            <%=IsEditable().ToString() == "False" ? "display:none": "" %>" id="lblQuickPicksHistory">
                                                            <%= GetLabel("Quick Picks History")%></span>
                                        </div>
                                    </asp:Panel>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>
                    </div>
                </td>
            </tr>
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
    <dxcp:ASPxCallbackPanel ID="cbpProcess" runat="server" Width="100%" ClientInstanceName="cbpProcess"
        ShowLoadingPanel="false" OnCallback="cbpProcess_Callback">
        <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpProcesEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
    <div id="divContainerShortcutInformation" style="display: none">
        <div>
            <div id="divCloseShortcutInformation">
                X</div>
            <div id="divOpenShortcutInformation">
                <span style="color: Blue;"><b>Informasi Instruksi Dokter :</span></b></div>
            <div style="padding: 2px; width: 100%; height: 100%">
                <div class="accordion" id="listInstruction" style="display: none; text-align: left;">
                    <a style="font-size: 12px;">
                        <%= GetLabel("INSTRUKSI DOKTER")%></a>
                    <div class="containerInformationUl">
                        <asp:Repeater ID="rptPhysicianInstruction" runat="server">
                            <HeaderTemplate>
                                <ul>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <li><span>
                                    <%#: Eval("cfInstructionDatePickerFormat")%>
                                    <%#: Eval("InstructionTime")%>
                                    <%#: Eval("PhysicianName")%></span>
                                    <div style="white-space: normal; overflow: auto">
                                        <%#: Eval("Description")%>
                                    </div>
                                </li>
                            </ItemTemplate>
                            <FooterTemplate>
                                </ul>
                            </FooterTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
