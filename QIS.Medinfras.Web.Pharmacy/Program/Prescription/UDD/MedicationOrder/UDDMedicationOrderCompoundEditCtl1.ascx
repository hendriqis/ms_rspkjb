<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UDDMedicationOrderCompoundEditCtl1.ascx.cs"
    Inherits="QIS.Medinfras.Web.Pharmacy.Program.UDDMedicationOrderCompoundEditCtl1" %>
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
            cboCompoundUnit.SetValue('');
            $('#<%=txtConversionFactor.ClientID %>').val('');
            $('#divEntryProduct').show();
            $('#<%=hdnIsAdd.ClientID %>').val(1);
        }
    });

    function onCboCompoundUnitValueChanged() {
        setConversionText();
    }

    function onCboCompoundUnitEndCallback() {
        var gcItemUnit = $('#<%=hdnGCItemUnit.ClientID %>').val();
        cboCompoundUnit.SetValue(gcItemUnit);
        setConversionText();
    }

    $('.imgPopupDelete.imgLink').die('click');
    $('.imgPopupDelete.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        showToastConfirmation('Are You Sure Want To Delete?', function (result) {
            if (result) {
                var entity = rowToObject($row);
                $('#<%=hdnPrescriptionOrderDetailID.ClientID %>').val(entity.PrescriptionOrderDetailID);
                cbpPopupProcess.PerformCallback("delete");
            }
        });
    });

    $('.imgPopupEdit.imgLink').die('click');
    $('.imgPopupEdit.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        var entity = rowToObject($row);

        $('#<%=hdnPrescriptionOrderDetailID.ClientID %>').val(entity.PrescriptionOrderDetailID);
        $('#<%=txtGenericName.ClientID %>').val(entity.GenericName);
        $('#<%=hdnItemID.ClientID %>').val(entity.ItemID);
        $('#<%=txtItemCode.ClientID %>').val(entity.ItemCode);
        $('#<%=txtItemName1.ClientID %>').val(entity.DrugName);
        $('#<%=txtDose.ClientID %>').val(entity.Dose);
        $('#<%=txtDoseUnit.ClientID %>').val(entity.DoseUnit)
        $('#<%=hdnGCDoseUnit.ClientID %>').val(entity.GCDoseUnit);
        $('#<%=txtCompoundQty.ClientID %>').val(entity.CompoundQtyInString);
        $('#<%=hdnGCItemUnit.ClientID %>').val(entity.GCCompoundUnit);

        $('#<%=hdnTakenQty.ClientID %>').val(entity.TakenQty);

        $('#<%=hdnIsAdd.ClientID %>').val(0);
        $('#<%=hdnConversionFactor.ClientID %>').val(entity.ConversionFactor);
        cboCompoundUnit.PerformCallback();
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
                var parentID = s.cpParentID;
                var prescriptionOrderID = s.cpPrescriptionOrderID
                $('#<%=hdnParentID.ClientID %>').val(parentID);
                $('#<%=hdnPrescriptionOrderID.ClientID %>').val(prescriptionOrderID);
                $('#divEntryProduct').hide();
                onAfterSaveRecordDtSuccess(prescriptionOrderID);
                cbpPopupView.PerformCallback('refresh');
            }
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail')
                showToast('Delete Failed', 'Error Message : ' + param[2]);
            else {
                var parentID = s.cpParentID;
                var prescriptionOrderID = s.cpPrescriptionOrderID
                $('#<%=hdnParentID.ClientID %>').val(parentID);
                $('#<%=hdnPrescriptionOrderID.ClientID %>').val(prescriptionOrderID);
                cbpPopupView.PerformCallback('refresh');
            }
        }
    }

    //#region Item Product
    $('#lblProduct.lblLink').click(function () {
        var filterExpression = onGetDrugFilterExpression();
        openSearchDialog('druginfowithbalance1', filterExpression, function (value) {
            $('#<%=txtItemCode.ClientID %>').val(value);
            ontxtItemCodeChanged(value);
        });
    });

    $('#<%=txtItemCode.ClientID %>').change(function () {
        ontxtItemCodeChanged($(this).val());
    });

    function ontxtItemCodeChanged(value) {
        var filterExpression = " ItemCode = '" + value + "'";
        Methods.getObject('GetvDrugInfo1List', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnItemID.ClientID %>').val(result.ItemID);
                $('#<%=txtItemName1.ClientID %>').val(result.ItemName1);
                $('#<%=txtGenericName.ClientID %>').val(result.GenericName);
                $('#<%=txtDose.ClientID %>').val(result.Dose);
                $('#<%=txtDoseUnit.ClientID %>').val(result.DoseUnit);
                $('#<%=hdnGCDoseUnit.ClientID %>').val(result.GCDoseUnit);
                $('#<%=hdnGCItemUnit.ClientID %>').val(result.GCItemUnit);
                $('#<%=hdnItemUnit.ClientID %>').val(result.ItemUnit);
                cboCompoundUnit.PerformCallback();
            }
            else {
                $('#<%=hdnItemID.ClientID %>').val('');
                $('#<%=txtItemCode.ClientID %>').val('');
                $('#<%=txtItemName1.ClientID %>').val('');
                $('#<%=txtGenericName.ClientID %>').val('');
                $('#<%=hdnGCItemUnit.ClientID %>').val('');
                $('#<%=txtDose.ClientID %>').val('');
                $('#<%=txtDoseUnit.ClientID %>').val('');
                $('#<%=hdnGCDoseUnit.ClientID %>').val('');
            }
        });
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
                $('#<%=txtStartTime.ClientID %>').val('-');
                $('#<%=txtStartTime1.ClientID %>').val('-');
                $('#<%=txtStartTime2.ClientID %>').val('-');
                $('#<%=txtStartTime3.ClientID %>').val('-');
                $('#<%=txtStartTime4.ClientID %>').val('-');
                $('#<%=txtStartTime5.ClientID %>').val('-');
                $('#<%=txtStartTime6.ClientID %>').val('-');
            }
        });
    }

    $('#<%=txtFrequencyNumber.ClientID %>').change(function () {
        SetMedicationDefaultTime($('#<%=txtFrequencyNumber.ClientID %>').val());
        getTakenQty();
    });

    $('#<%=txtDosingDuration.ClientID %>').change(function () {
        getTakenQty();
    });

    $('#<%=txtCompoundQty.ClientID %>').change(function () {
        var compoundQty = $('#<%=txtCompoundQty.ClientID %>').val();
        if (compoundQty <= 0 || compoundQty == "") {
            showToast('Error Message', 'Dose Quantity should be greater than 0 !');
            $('#<%=txtCompoundQty.ClientID %>').val('');
        }
        //getTakenQty();
    });

    $('#<%=txtDispenseQty.ClientID %>').live('change', function () {
        var dispQty = $('#<%=txtDispenseQty.ClientID %>').val();
        if (dispQty <= 0 || dispQty == "") {
            showToast('Error Message', 'Compound Quantity should be greater than 0 !');
            $('#<%=txtDispenseQty.ClientID %>').val('');
            $('#<%=txtTakenQty.ClientID %>').val('');
        }
    });

    $('#<%=txtTakenQty.ClientID %>').change(function () {
        var takenQty = $('#<%=txtTakenQty.ClientID %>').val();
        if (takenQty <= 0 || takenQty == "") {
            showToast('Error Message', 'Taken Quantity should be greater than 0 !');
            $('#<%=txtTakenQty.ClientID %>').val('');
        }
        //getTakenQty();
    });

    function getTakenQty() {
        var duration = $('#<%=txtDosingDuration.ClientID %>').val();
        var frequency = $('#<%=txtFrequencyNumber.ClientID %>').val();
        var dose = $('#<%=txtDosingDose.ClientID %>').val();
        var dispense = duration * frequency * dose;
        $('#<%=txtDispenseQty.ClientID %>').val(dispense);
        if (dispense <= 0 || dispense == "") {
            $('#<%=txtDispenseQty.ClientID %>').val('');
        }
        $('#<%=txtTakenQty.ClientID %>').val(dispense);
    }
    
</script>
<input type="hidden" value="" id="hdnInlineEditingData" runat="server" />
<input type="hidden" value="" id="hdnQueryString" runat="server" />
<input type="hidden" value="" id="hdnFilterExpression" runat="server" />
<input type="hidden" value="" id="hdnIsAdd" runat="server" />
<input type="hidden" value="" id="hdnGCItemUnit" runat="server" />
<input type="hidden" value="" id="hdnItemUnit" runat="server" />
<input type="hidden" value="" id="hdnPrescriptionOrderDetailID" runat="server" />
<input type="hidden" value="" id="hdnGCDoseUnit" runat="server" />
<input type="hidden" value="" id="hdnChargeClassID" runat="server" />
<input type="hidden" value="" id="hdnRegistrationID" runat="server" />
<input type="hidden" value="" id="hdnVisitID" runat="server" />
<input type="hidden" value="" id="hdnCoverageAmount" runat="server" />
<input type="hidden" value="" id="hdnDiscountInPercentage" runat="server" />
<input type="hidden" value="" id="hdnCoverageInPercentage" runat="server" />
<input type="hidden" value="" id="hdnBaseTariff" runat="server" />
<input type="hidden" value="" id="hdnBaseQuantity" runat="server" />
<input type="hidden" value="" id="hdnTakenQty" runat="server" />
<input type="hidden" value="" id="hdnPrescriptionOrderID" runat="server" />
<input type="hidden" value="" id="hdnDepartmentID" runat="server" />
<input type="hidden" value="" id="hdnLocationID" runat="server" />
<input type="hidden" value="" id="hdnDispensaryServiceUnitID" runat="server" />
<input type="hidden" value="" id="hdnIsEditable" runat="server" />
<div style="height: 525px; overflow-y: auto">
    <table class="tblContentArea">
        <tr>
            <td valign="top">
                <table style="width: 100%" cellpadding="1" cellspacing="1">
                    <colgroup>
                        <col width="180px" />
                        <col width="40px" />
                        <col width="60px" />
                        <col width="40px" />
                        <col width="50px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Nama Racikan")%></label>
                        </td>
                        <td colspan="4">
                            <asp:TextBox runat="server" ID="txtCompoundMedicationName" Width="300px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Frekuensi / Dosis")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFrequencyNumber" runat="server" Width="100%" CssClass="number" />
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboFrequencyTimeline" runat="server" Width="100%" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtDosingDose" runat="server" Width="100%" CssClass="number" />
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboDosingUnitCompoundCtl" ClientInstanceName="cboDosingUnitCompoundCtl"
                                runat="server" Width="100%" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Durasi (hari)")%></label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtDosingDuration" Width="100%" CssClass="number" />
                        </td>
                        <td class="tdLabel" style="text-align: center">
                            <label class="lblMandatory">
                                <%=GetLabel("Quantity")%></label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtDispenseQty" Width="100%" CssClass="number" />
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="chkIsUsingSweetener" /><%=GetLabel("slqs")%>
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
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Embalase - Jumlah")%></label>
                        </td>
                        <td colspan="2">
                            <dxe:ASPxComboBox runat="server" ID="cboEmbalace" ClientInstanceName="cboEmbalace"
                                Width="100%" />
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtEmbalaceQty" Width="80px" CssClass="number" />
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="chkIsAsRequired" /><%=GetLabel("PRN")%>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Rute Pemberian")%></label>
                        </td>
                        <td colspan="2">
                            <dxe:ASPxComboBox runat="server" ID="cboMedicationRouteCompoundCtl" ClientInstanceName="cboMedicationRouteCompoundCtl"
                                Width="100%" />
                        </td>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("AC/DC/PC")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox runat="server" ID="cboCoenamRuleCompoundCtl" ClientInstanceName="cboCoenamRuleCompoundCtl"
                                Width="100%" />
                        </td>
                    </tr>
                </table>
            </td>
            <td valign="top">
                <table style="width: 100%">
                    <colgroup>
                        <col style="width: 160px" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Mulai Diberikan")%></label>
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
                                    <td style="display: none">
                                        <asp:TextBox runat="server" ID="txtStartTime" CssClass="time" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Taken Time")%></label>
                        </td>
                        <td>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td style="width: 26%">
                                        <asp:CheckBox ID="chkIsMorning" runat="server" Text="Morning" Checked="false" />
                                    </td>
                                    <td style="width: 24%">
                                        <asp:CheckBox ID="chkIsNoon" runat="server" Text="Noon" Checked="false" />
                                    </td>
                                    <td style="width: 26%">
                                        <asp:CheckBox ID="chkIsEvening" runat="server" Text="Evening" Checked="false" />
                                    </td>
                                    <td style="width: 24%">
                                        <asp:CheckBox ID="chkIsNight" runat="server" Text="Night" Checked="false" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="vertical-align: top; padding-top: 2px;">
                            <label class="lblNormal">
                                <%=GetLabel("Instruksi Khusus Pemberian")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMedicationAdministration" Width="100%" runat="server" TextMode="MultiLine"
                                Height="50px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="vertical-align: top; padding-top: 2px;">
                            <label class="lblNormal">
                                <%=GetLabel("Alasan Pengobatan")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMedicationPurpose" Width="100%" runat="server" TextMode="MultiLine"
                                Height="50px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Waktu Pemberian")%></label>
                        </td>
                        <td>
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="width: 50px" align="center">
                                        <label class="lblNormal">
                                            1</label>
                                    </td>
                                    <td style="width: 50px" align="center">
                                        <label class="lblNormal">
                                            2</label>
                                    </td>
                                    <td style="width: 50px" align="center">
                                        <label class="lblNormal">
                                            3</label>
                                    </td>
                                    <td style="width: 50px" align="center">
                                        <label class="lblNormal">
                                            4</label>
                                    </td>
                                    <td style="width: 50px" align="center">
                                        <label class="lblNormal">
                                            5</label>
                                        <td style="width: 50px" align="center">
                                            <label class="lblNormal">
                                                6</label>
                                        </td>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 50px">
                                        <asp:TextBox runat="server" ID="txtStartTime1" CssClass="time" Width="100%" Text="00:00" />
                                    </td>
                                    <td style="width: 50px">
                                        <asp:TextBox runat="server" ID="txtStartTime2" CssClass="time" Width="100%" Text="00:00" />
                                    </td>
                                    <td style="width: 50px">
                                        <asp:TextBox runat="server" ID="txtStartTime3" CssClass="time" Width="100%" Text="00:00" />
                                    </td>
                                    <td style="width: 50px">
                                        <asp:TextBox runat="server" ID="txtStartTime4" CssClass="time" Width="100%" Text="00:00" />
                                    </td>
                                    <td style="width: 50px">
                                        <asp:TextBox runat="server" ID="txtStartTime5" CssClass="time" Width="100%" Text="00:00" />
                                    </td>
                                    <td style="width: 50px">
                                        <asp:TextBox runat="server" ID="txtStartTime6" CssClass="time" Width="100%" Text="00:00" />
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
                <div style="display: none;" id="divEntryProduct">
                    <div class="pageTitle">
                        <%=GetLabel("Compound Medication : Detail")%></div>
                    <fieldset id="fsTrxPopup" style="margin: 0">
                        <table class="tblEntryDetail">
                            <colgroup>
                                <col style="width: 50%" />
                                <col />
                            </colgroup>
                            <tr>
                                <td valign="top">
                                    <table class="tblEntryDetail">
                                        <colgroup>
                                            <col width="30%" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <label class="lblMandatory lblLink" id="lblProduct">
                                                    <%=GetLabel("Item")%></label>
                                            </td>
                                            <td>
                                                <table cellpadding="0px" cellspacing="0px" width="100%">
                                                    <tr>
                                                        <td style="width: 100px">
                                                            <input type="hidden" id="hdnItemID" runat="server" value="" />
                                                            <asp:TextBox runat="server" ID="txtItemCode" Width="100%" />
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtItemName1" ReadOnly="true" Width="100%" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label class="lblNormal">
                                                    <%=GetLabel("Nama Generik")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtGenericName" Width="100%" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label class="lblNormal">
                                                    <%=GetLabel("Kadar")%></label>
                                            </td>
                                            <td>
                                                <table cellpadding="0px" cellspacing="0px" width="100%">
                                                    <tr>
                                                        <td style="width: 100px">
                                                            <asp:TextBox runat="server" ID="txtDose" CssClass="number" Width="100%" ReadOnly="true" />
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtDoseUnit" Width="100%" ReadOnly="true" />
                                                        </td>
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
                                    <table class="tblEntryDetail">
                                        <colgroup>
                                            <col width="30%" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td colspan="2">
                                                <div class="lblComponent">
                                                    <%=GetLabel("COMPOUND FORMULA")%></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label class="lblMandatory">
                                                    <%=GetLabel("Dose Quantity")%></label>
                                            </td>
                                            <td>
                                                <table cellpadding="0px" cellspacing="0px" width="100%">
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtCompoundQty" Width="100%" Style="text-align: right" />
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <dxe:ASPxComboBox runat="server" ID="cboCompoundUnit" ClientInstanceName="cboCompoundUnit"
                                                                OnCallback="cboCompoundUnit_Callback" Width="100%">
                                                                <ClientSideEvents ValueChanged="function(s,e){ onCboCompoundUnitValueChanged(); }"
                                                                    EndCallback="function(s,e){ onCboCompoundUnitEndCallback(); }" />
                                                            </dxe:ASPxComboBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label class="lblNormal">
                                                    <%=GetLabel("Conversion Factor") %></label>
                                            </td>
                                            <td>
                                                <input type="hidden" id="hdnConversionFactor" runat="server" />
                                                <asp:TextBox runat="server" ID="txtConversionFactor" ReadOnly="true" Width="100%" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                <dxcp:ASPxCallbackPanel ID="cbpPopupView" runat="server" Width="100%" ClientInstanceName="cbpPopupView"
                    ShowLoadingPanel="false" OnCallback="cbpPopupView_Callback">
                    <ClientSideEvents BeginCallback="function() { showLoadingPanel(); }" EndCallback="function(s,e){ onCbpPopupViewEndCallback(s) }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="max-height: 320px">
                                <input type="hidden" value="" id="hdnParentID" runat="server" />
                                <asp:ListView ID="lvwView" runat="server">
                                    <EmptyDataTemplate>
                                        <table class="grdNormal" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 80px" />
                                                <col style="width: 100px" />
                                                <col />
                                                <col style="width: 70px" />
                                                <col style="width: 100px" />
                                                <col style="width: 70px" />
                                                <col style="width: 100px" />
                                                <col style="width: 170px" />
                                            </colgroup>
                                            <tr>
                                                <th align="center" rowspan="2">
                                                    #
                                                </th>
                                                <th align="center" rowspan="2">
                                                    <%=GetLabel("Generic Name")%>
                                                </th>
                                                <th align="center" rowspan="2">
                                                    <%=GetLabel("Product")%>
                                                </th>
                                                <th align="center" colspan="2">
                                                    <%=GetLabel("Strength")%>
                                                </th>
                                                <th align="center" colspan="3">
                                                    <%=GetLabel("Compound")%>
                                                </th>
                                            </tr>
                                            <tr>
                                                <th align="center">
                                                    <%=GetLabel("Amount")%>
                                                </th>
                                                <th align="center">
                                                    <%=GetLabel("Unit")%>
                                                </th>
                                                <th align="center">
                                                    <%=GetLabel("Quantity")%>
                                                </th>
                                                <th align="center">
                                                    <%=GetLabel("Unit")%>
                                                </th>
                                                <th align="center">
                                                    <%=GetLabel("Conversion")%>
                                                </th>
                                            </tr>
                                            <tr>
                                                <td colspan="8" align="center" style="height: 45px; vertical-align: middle">
                                                    <%=GetLabel("No Data To Display")%>
                                                </td>
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                    <LayoutTemplate>
                                        <table class="grdNormal" runat="server" id="tblView" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 80px" />
                                                <col />
                                                <col style="width: 70px" />
                                                <col style="width: 100px" />
                                                <col style="width: 70px" />
                                                <col style="width: 100px" />
                                            </colgroup>
                                            <tr>
                                                <th align="center" rowspan="2">
                                                    #
                                                </th>
                                                <th align="left" rowspan="2">
                                                    <%=GetLabel("Item Name")%>
                                                </th>
                                                <th align="center" colspan="2">
                                                    <%=GetLabel("Strength")%>
                                                </th>
                                                <th align="center" colspan="3">
                                                    <%=GetLabel("Compound Formula")%>
                                                </th>
                                            </tr>
                                            <tr>
                                                <th align="center">
                                                    <%=GetLabel("Amount")%>
                                                </th>
                                                <th align="center">
                                                    <%=GetLabel("Unit")%>
                                                </th>
                                                <th align="center">
                                                    <%=GetLabel("Quantity")%>
                                                </th>
                                                <th align="center">
                                                    <%=GetLabel("Unit")%>
                                                </th>
                                            </tr>
                                            <tr runat="server" id="itemPlaceholder">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td align="center">
                                                <img class="imgPopupEdit imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                    alt="" />
                                                <img class="imgPopupDelete <%# IsEditable() == "0" ? "imgDisabled" : "imgLink"%>"
                                                    src='<%# IsEditable() == "0" ? ResolveUrl("~/Libs/Images/Button/delete_disabled.png") : ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                    alt="" />
                                                <input type="hidden" value="<%#:Eval("PrescriptionOrderDetailID") %>" bindingfield="PrescriptionOrderDetailID" />
                                                <input type="hidden" value="<%#:Eval("GenericName") %>" bindingfield="GenericName" />
                                                <input type="hidden" value="<%#:Eval("ItemID") %>" bindingfield="ItemID" />
                                                <input type="hidden" value="<%#:Eval("ItemCode") %>" bindingfield="ItemCode" />
                                                <input type="hidden" value="<%#:Eval("DrugName") %>" bindingfield="DrugName" />
                                                <input type="hidden" value="<%#:Eval("Dose") %>" bindingfield="Dose" />
                                                <input type="hidden" value="<%#:Eval("GCDoseUnit") %>" bindingfield="GCDoseUnit" />
                                                <input type="hidden" value="<%#:Eval("DoseUnit") %>" bindingfield="DoseUnit" />
                                                <input type="hidden" value="<%#:Eval("CompoundQty") %>" bindingfield="CompoundQty" />
                                                <input type="hidden" value="<%#:Eval("CompoundQtyInString") %>" bindingfield="CompoundQtyInString" />
                                                <input type="hidden" value="<%#:Eval("TakenQty") %>" bindingfield="TakenQty" />
                                                <input type="hidden" value="<%#:Eval("GCCompoundUnit") %>" bindingfield="GCCompoundUnit" />
                                                <input type="hidden" value="<%#:Eval("ParentID") %>" bindingfield="ParentID" />
                                                <input type="hidden" value="<%#:Eval("ConversionFactor") %>" bindingfield="ConversionFactor">
                                            </td>
                                            <td>
                                                <div>
                                                    <%#: Eval("DrugName") %></div>
                                                <div style="color: Blue; float: left;">
                                                    <%#: Eval("GenericName")%></div>
                                            </td>
                                            <td align="right">
                                                <%#: Eval("cfDose") %>
                                            </td>
                                            <td align="center">
                                                <%#: Eval("DoseUnit") %>
                                            </td>
                                            <td align="right">
                                                <%#: Eval("CompoundQtyInString")%>
                                            </td>
                                            <td align="center">
                                                <%#: Eval("CompoundUnit")%>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                                <div style="width: 100%; text-align: center" id="divContainerAddData" runat="server">
                                    <span class="lblLink" id="lblPopupAddData">
                                        <%= GetLabel("Add Data")%></span>
                                </div>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div class="imgLoadingGrdView" id="containerImgLoadingView">
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
            </td>
        </tr>
    </table>
    <dxcp:ASPxCallbackPanel ID="cbpPopupProcess" runat="server" Width="100%" ClientInstanceName="cbpPopupProcess"
        ShowLoadingPanel="false" OnCallback="cbpPopupProcess_Callback">
        <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpPopupProcesEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</div>
