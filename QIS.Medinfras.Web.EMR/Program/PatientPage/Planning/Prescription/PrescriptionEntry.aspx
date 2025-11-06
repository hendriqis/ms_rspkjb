<%@ Page Language="C#" MasterPageFile="~/MasterPage/MPPatientPageListEntry.master"
    AutoEventWireup="true" CodeBehind="PrescriptionEntry.aspx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.PrescriptionEntry" %>

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
    <li id="btnPrescriptionCompoundEntry" runat="server" crudmode="C">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbcompound.png")%>' alt="" /><div>
            <%=GetLabel("Compound")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhRightButtonToolbar" runat="server">
    <li id="btnSendOrder" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbProcess.png")%>' alt="" /><div>
            <%=GetLabel("Send Order")%></div>
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
                    $('#<%=hdnStrengthUnit.ClientID %>').val(result.GCDoseUnit.substring(5));
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
                $('#<%=hdnStrengthUnit.ClientID %>').val(entity.DoseUnit);
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
                var prescriptionID = $('#<%=hdnPrescriptionOrderID.ClientID %>').val();
                var date = Methods.dateToString(Methods.getDatePickerDate($('#<%=txtPrescriptionDate.ClientID %>').val()));
                var time = $('#<%=txtPrescriptionTime.ClientID %>').val();
                var physician = cboParamedicID.GetValue();
                var prescriptionType = cboPrescriptionType.GetValue();
                var dispensaryServiceUnitID = cboDispensaryUnit.GetValue();
                var refillInstruction = cboRefillInstruction.GetValue();
                var location = cboLocation.GetValue();

                var queryString = prescriptionID + '|' + date + '|' + time + '|' + physician + '|' + prescriptionType + '|' + dispensaryServiceUnitID + '|' + location + '|' + refillInstruction;

                var url = ResolveUrl("~/Program/PatientPage/Planning/Prescription/PrescriptionCompoundEntryCtl.ascx");
                openUserControlPopup(url, queryString, 'Compound Prescription', 900, 650);
            });

            $('#<%=btnSendOrder.ClientID %>').click(function () {
                if ($('#<%:hdnPrescriptionOrderID.ClientID %>').val() == "") {
                    showToast("ERROR", 'Error Message : ' + "There is no order to be sent !");
                }
                else {
                    var message = "Send your prescription to Pharmacy ?";
                    showToastConfirmation(message, function (result) {
                        if (result) cbpSendOrder.PerformCallback('sendOrder');
                    });
                }
            });

            setDatePicker('<%=txtStartDate.ClientID %>');
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
            var message = "Send your prescription to Pharmacy ?";
            showToastConfirmation(message, function (result) {
                if (result) cbpSendOrder.PerformCallback('sendOrder');
                else {
                    showLoadingPanel();
                    document.location = document.referrer;
                }
            });
        }

        function onBeforeEditRecordIsUsingCustomEdit(entity) {
            return (entity.IsCompound == 'True')
        }

        function onCustomEditRecord(entity) {
            var prescriptionID = $('#<%=hdnPrescriptionOrderID.ClientID %>').val();
            var queryString = prescriptionID + '|' + entity.PrescriptionOrderDetailID;

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
            if ($('#<%=hdnPrescriptionOrderID.ClientID %>').val() == '') {
                $('#<%=hdnPrescriptionOrderID.ClientID %>').val(param);
                var filterExpression = $('#<%=hdnFilterExpressionItem.ClientID %>').val();
                txtQuickEntry.SetFilterExpression(0, filterExpression);
                ledDrugName.SetFilterExpression(filterExpression);
                var filterExpressionPrescription = 'PrescriptionOrderID = ' + param;
                Methods.getObject('GetPrescriptionOrderHdList', filterExpressionPrescription, function (result) {
                    $('#<%=txtPrescriptionNo.ClientID %>').val(result.PrescriptionOrderNo);
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

        function onBeforeSaveRecord() {
            if ($('#<%=txtGenericName.ClientID %>').val() != '')
                return true;
            return ledDrugName.Validate();
        }

        function onTxtQuickEntrySearchClick(s) {
            onPatientPageListEntryQuickEntrySave(s.GetValue());
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

        //#region calculate Dispense Qty
        $('#<%=txtFrequencyNumber.ClientID %>').live('change', function () {
            var dispQty = $('#<%=txtDispenseQty.ClientID %>').val();
            calculateDispenseQty();
        });

        $('#<%=txtDosingDose.ClientID %>').live('change', function () {
            var dispQty = $('#<%=txtDispenseQty.ClientID %>').val();
            calculateDispenseQty();
        });

        $('#<%=txtDosingDuration.ClientID %>').live('change', function () {
            var dispQty = $('#<%=txtDispenseQty.ClientID %>').val();
            calculateDispenseQty();
        });

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
                if (strengthAmount != 0)
                    dispenseQty = Math.ceil((dosingDuration * frequency * dose) / strengthAmount);
                else
                    dispenseQty = 1;
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
                    showToast('Send Success', 'The prescription order was successfully sent to Pharmacy.');
                    document.location = document.referrer;
                }
                else {
                    showToast('Save Failed', 'Error Message : ' + param[2]);
                }
            }
        }
    </script>
    <input type="hidden" id="hdnPrescriptionOrderID" runat="server" value="" />
    <input type="hidden" id="hdnFilterExpressionItem" runat="server" value="" />
    <input type="hidden" id="hdnIsProposed" runat="server" value="0" />
    <input type="hidden" value="" id="hdnDefaultDispensaryServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnLocationID" runat="server" />
    <input type="hidden" value="" id="hdnGCBaseUnit" runat="server" />
    <input type="hidden" value="" id="hdnStrengthUnit" runat="server" />
    <input type="hidden" value="0" id="hdnStrengthAmount" runat="server" />
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
                                <ClientSideEvents ValueChanged="function() { cboLocation.PerformCallback(); }" />
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
                                <ClientSideEvents BeginCallback="function() { showLoadingPanel(); }" EndCallback="function() { hideLoadingPanel(); }" />
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
                    <tr>
                        <td class="tdLabel">
                            <%=GetLabel("Refill Instruction") %>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboRefillInstruction" ClientInstanceName="cboRefillInstruction"
                                Width="130px" runat="server" />
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
                            Description="Item Name" FilterExpression="IsDeleted = 0" MethodName="GetvDrugInfoList">
                            <Columns>
                                <qis:QISQuickEntryHintColumn Caption="Item Code" FieldName="ItemCode" Width="80px" />
                                <qis:QISQuickEntryHintColumn Caption="Item Name" FieldName="ItemName1" Width="600px" />
                                <qis:QISQuickEntryHintColumn Caption="Employee Formularium" FieldName="IsEmployeeFormularium" Width="70px" />
                                <qis:QISQuickEntryHintColumn Caption="Qty On Hand (All)" FieldName="QtyOnHandAll" Width="80px" />
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
                                        <asp:CheckBox runat="server" ID="chkIsRx" Text="Rx" />
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
                                Width="100%" ValueText="ItemID" FilterExpression="IsDeleted = 0" DisplayText="ItemName1"
                                MethodName="GetvDrugInfoList">
                                <ClientSideEvents ValueChanged="function(s){ onLedDrugNameLostFocus(s); }" />
                                <Columns>
                                    <qis:QISSearchTextBoxColumn Caption="Drug Name" FieldName="ItemName" Description="i.e. Panadol"
                                        Width="400px" />
                                    <qis:QISSearchTextBoxColumn Caption="Formularium" FieldName="IsFormularium" Description="Formularium"
                                        Width="80px" />
                                    <qis:QISSearchTextBoxColumn Caption="Employee" FieldName="IsEmployeeFormularium" Description="Employee Formularium"
                                        Width="80px" />
                                    <qis:QISSearchTextBoxColumn Caption="Qty On Hand (All)" FieldName="QtyOnHandAll" Description="Qty On Hand (Hospital)"
                                        Width="80px" />
                                </Columns>
                            </qis:QISSearchTextBox>
                        </td>
                    </tr>
                    <tr style="display:none">
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
                        <td class="tdLabel" style="display:none">
                            <label class="lblNormal">
                                <%=GetLabel("Form")%></label>
                        </td>
                        <td style="display:none">
                            <dxe:ASPxComboBox runat="server" ID="cboDrugForm" ClientInstanceName="cboDrugForm"
                                Width="100%" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Signa")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFrequencyNumber" runat="server" Width="100%" CssClass="number" />
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboFrequencyTimeline" ClientInstanceName="cboFrequencyTimeline"
                                runat="server" Width="100%" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtDosingDose" runat="server" Width="100%" CssClass="number" />
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboDosingUnit" ClientInstanceName="cboDosingUnit" runat="server"
                                Width="100%" />
                        </td>
                        <td class="tdLabel">
                            <label class="lblMandatory">
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
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
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
                                        <asp:TextBox runat="server" ID="txtStartTime" CssClass="time" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
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
                        <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                            <label class="lblNormal">
                                <%=GetLabel("Administration Instruction")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMedicationAdministration" Width="400px" runat="server" TextMode="MultiLine" />
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                            <label class="lblNormal">
                                <%=GetLabel("Purpose Of Medication")%></label>
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
                            ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                            <Columns>
                                <asp:BoundField DataField="PrescriptionOrderDetailID" HeaderStyle-CssClass="keyField"
                                    ItemStyle-CssClass="keyField" />
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
                                        <input type="hidden" value="<%#:Eval("GCCoenamRule") %>" bindingfield="GCCoenamRule" />
                                        <input type="hidden" value="<%#:Eval("MedicationAdministration") %>" bindingfield="MedicationAdministration" />
                                        <input type="hidden" value="<%#:Eval("IsMorning") %>" bindingfield="IsMorning" />
                                        <input type="hidden" value="<%#:Eval("IsNoon") %>" bindingfield="IsNoon" />
                                        <input type="hidden" value="<%#:Eval("IsEvening") %>" bindingfield="IsEvening" />
                                        <input type="hidden" value="<%#:Eval("IsNight") %>" bindingfield="IsNight" />
                                        <input type="hidden" value="<%#:Eval("IsAsRequired") %>" bindingfield="IsAsRequired" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                    <HeaderTemplate>
                                        <div>
                                            <%=GetLabel("Drug Name")%>
                                            (
                                            <%=GetLabel("Generic Name")%>
                                            ) -
                                            <%=GetLabel("Form")%>
                                            -
                                            <%=GetLabel("Dispense Qty")%>
                                        </div>
                                        <div>
                                            <div style="color: Blue; width: 35px; float: left;">
                                                <%=GetLabel("DOSE")%></div>
                                            <%=GetLabel("Dose")%>
                                            -
                                            <%=GetLabel("Route")%>
                                            -
                                            <%=GetLabel("Frequency")%></div>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div>
                                            <%#: Eval("InformationLine1")%></div>
                                        <div>
                                            <div style="color: Blue; width: 35px; float: left;">
                                                <%=GetLabel("DOSE")%></div>
                                            <%#: Eval("NumberOfDosageInString")%>
                                            <%#: Eval("DosingUnit")%>
                                            -
                                            <%#: Eval("Route")%>
                                            -
                                            <%#: Eval("cfDoseFrequency")%>
                                            - <span style="font-style: italic">
                                                <%#: Eval("MedicationAdministration")%>
                                            </span>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                    <HeaderTemplate>
                                        <div>
                                            <%=GetLabel("Compound Contents")%></div>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div>
                                            <%#: Eval("MedicationLine")%></div>
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
