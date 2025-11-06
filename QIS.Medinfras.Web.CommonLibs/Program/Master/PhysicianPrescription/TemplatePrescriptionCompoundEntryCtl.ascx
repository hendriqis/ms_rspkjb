<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TemplatePrescriptionCompoundEntryCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.TemplatePrescriptionCompoundEntryCtl" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" 
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
    
<script type="text/javascript" id="dxis_prescriptioncompoundentryctl" src='<%= ResolveUrl("~/Libs/Scripts/inlineEditing-1.0.js")%>'></script>
<script type="text/javascript" id="dxss_prescriptioncompoundentryctl">
    $(function () {
        $('#<%=txtCompoundMedicationName.ClientID %>').focus();

        $("input[type='text']").click(function () {
            $(this).select();
        });
    });

    //#region Signa
    $('#lblSigna.lblLink').live('click', function () {
        var filterExpression = "IsDeleted = 0";
        openSearchDialog('signa', filterExpression, function (value) {
            $('#<%=txtSignaLabelCompound.ClientID %>').val(value);
            txtSignaLabelCompoundChanged(value);
        });
    });

    $('#<%=txtSignaLabelCompound.ClientID %>').live('change', function () {
        txtSignaLabelCompoundChanged($(this).val());
    });

    function txtSignaLabelCompoundChanged(value) {
        var filterExpression = "IsDeleted = 0 AND SignaLabel = '" + value + "'";
        Methods.getObject('GetvSignaList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnSignaIDCompound.ClientID %>').val(result.SignaID);
                $('#<%=txtSignaName1Compound.ClientID %>').val(result.SignaName1);
                cboCoenamRule.SetValue(result.GCCoenamRule);
                $('#<%=txtFrequencyNumber.ClientID %>').val(result.Frequency);
                $('#<%=txtFrequencyNumber.ClientID %>').change();
                cboDosingUnit.SetValue(result.GCDoseUnit);
                $('#<%=txtDosingDose.ClientID %>').val(result.Dose);
                $('#<%=txtDosingDose.ClientID %>').change();
            } else {
                $('#<%=hdnSignaIDCompound.ClientID %>').val('');
                $('#<%=txtSignaLabelCompound.ClientID %>').val('');
                $('#<%=txtSignaName1Compound.ClientID %>').val('');
            }
        });
    }
    //#endregion

    setDatePicker('<%=txtStartDate.ClientID %>');

    var grdCompound = new InlineEditing();
    var listParam = new Array();
    var cboCompoundStrengthUnitID = '<%=cboCompoundStrengthUnit.ClientID%>';
    var cboCompoundUnitID = '<%=cboCompoundUnit.ClientID%>';
    var ledProductID = '<%=ledProduct.ClientID%>';
    listParam[0] = { "type": "hdn", "className": "hdnGenericName" };
    listParam[1] = { "type": "led", "className": "ledProduct", "ledID": ledProductID, "isEnabled": true };
    listParam[2] = { "type": "txt", "dataType": "float", "className": "txtStrengthAmount", "isEnabled": false };
    listParam[3] = { "type": "cbo", "className": "cboCompoundStrengthUnit", "cboID": cboCompoundStrengthUnitID, "isUnique": false, "isEnabled": false };
    listParam[4] = { "type": "txt", "className": "txtCompoundQty", "isEnabled": true };
    listParam[5] = { "type": "cbo", "className": "cboCompoundUnit", "cboID": cboCompoundUnitID, "isUnique": false, "isEnabled": true };
    listParam[6] = { "type": "txt", "className": "txtConversionFactor", "isEnabled": false };
    listParam[7] = { "type": "hdn", "className": "hdnDrugName" };
    listParam[8] = { "type": "hdn", "className": "hdnDose" };
    listParam[9] = { "type": "hdn", "className": "hdnDoseUnit" };
    listParam[10] = { "type": "hdn", "className": "hdnGCDoseUnit" };
    listParam[11] = { "type": "hdn", "className": "hdnItemUnit" };
    listParam[12] = { "type": "hdn", "className": "hdnGCItemUnit" };
    listParam[13] = { "type": "hdn", "className": "hdnGCStockDeductionType" };
    listParam[14] = { "type": "hdn", "className": "hdnGCConsumptionDeductionType" };
    listParam[15] = { "type": "hdn", "className": "hdnPrescriptionOrderDtID", "value": "0" };
    listParam[16] = { "type": "hdn", "className": "hdnSignaID" };

    window.onLedProductLostFocus = function (s) {
        grdCompound.hideQisSearchTextBox(s);
    }

    var listItemID = [];

    function calculateResultQty($row) {
        var itemUnit = grdCompound.getCellHiddenValue($row, 'hdnItemUnit');
        var doseUnit = grdCompound.getCellHiddenValue($row, 'hdnDoseUnit');
        var dose = grdCompound.getCellHiddenValue($row, 'hdnDose');

        var conversion = '';
        if (itemUnit == doseUnit)
            conversion = '1 ' + itemUnit + ' = 1 ' + doseUnit;
        else
            conversion = '1 ' + itemUnit + ' = ' + dose + ' ' + doseUnit;
        grdCompound.setTextBoxProperties($row, 'txtConversionFactor', { "value": conversion });
    }

    function setConversionText($row) {
        var itemUnit = grdCompound.getCellHiddenValue($row, 'hdnItemUnit');
        var doseUnit = grdCompound.getCellHiddenValue($row, 'hdnDoseUnit');
        var dose = grdCompound.getCellHiddenValue($row, 'hdnDose');

        var conversion = '';
        if (itemUnit == doseUnit)
            conversion = '1 ' + itemUnit + ' = 1 ' + doseUnit;
        else
            conversion = '1 ' + itemUnit + ' = ' + dose + ' ' + doseUnit;
        grdCompound.setTextBoxProperties($row, 'txtConversionFactor', { "value": conversion });
    }

    window.numFinishLoad = 0;
    window.init = function () {
        cboCompoundUnit.ClearItems();
        grdCompound.init('tblPrescriptionCompound', listParam);
        grdCompound.setValidationGroup('mpEntryPopup');
        grdCompound.setOnCboFocusHandler(function ($row, cboClass) {
            if (cboClass == 'cboCompoundUnit') {
                cboCompoundUnit.ClearItems();

                var GCItemUnit = grdCompound.getCellHiddenValue($row, 'hdnGCItemUnit');
                var ItemUnit = grdCompound.getCellHiddenValue($row, 'hdnItemUnit');
                var GCDoseUnit = grdCompound.getCellHiddenValue($row, 'hdnGCDoseUnit');
                var DoseUnit = grdCompound.getCellHiddenValue($row, 'hdnDoseUnit');

                if (GCDoseUnit != "") {
                    cboCompoundUnit.AddItem(DoseUnit, GCDoseUnit);
                }
                cboCompoundUnit.AddItem(ItemUnit, GCItemUnit);

                var value = grdCompound.getComboBoxValue($row, cboClass);
                cboCompoundUnit.SetValue(value);
            }
        });
        grdCompound.setOnLedValueChangedHandler(function ($row, ledClass, oldValue, newValue) {
            var productName = grdCompound.getSearchTextBoxText($row, 'ledProduct');
            grdCompound.setCellHiddenValue($row, 'hdnDrugName', productName);
            if (newValue != '') {
                var filterExpression = "ItemID = " + newValue;
                Methods.getObject('GetvDrugInfo1List', filterExpression, function (result) {
                    cboCompoundUnit.ClearItems();
                    if (result.GCDoseUnit != "") {
                        cboCompoundStrengthUnit.SetValue(result.GCDoseUnit);
                        var doseUnit = cboCompoundStrengthUnit.GetText();
                        cboCompoundUnit.AddItem(doseUnit, result.GCDoseUnit);

                        grdCompound.setCellHiddenValue($row, 'hdnGCDoseUnit', result.GCDoseUnit);
                        grdCompound.setCellHiddenValue($row, 'hdnDoseUnit', doseUnit);
                    }
                    cboCompoundUnit.AddItem(result.ItemUnit, result.GCItemUnit);

                    grdCompound.setTextBoxProperties($row, 'txtStrengthAmount', { "value": result.Dose });
                    grdCompound.setComboBoxProperties($row, 'cboCompoundStrengthUnit', { "value": result.GCDoseUnit });
                    grdCompound.setTextBoxProperties($row, 'txtCompoundQty', { "value": "1" });

                    if (result.GCDoseUnit != "" && $('#<%=hdnIsDefaultUsingStrengthUnit.ClientID %>').val() == "1") {
                        grdCompound.setComboBoxProperties($row, 'cboCompoundUnit', { "value": result.GCDoseUnit });
                    }
                    else {
                        grdCompound.setComboBoxProperties($row, 'cboCompoundUnit', { "value": result.GCItemUnit });
                    }

                    grdCompound.setCellHiddenValue($row, 'hdnGenericName', result.GenericName);
                    grdCompound.setCellHiddenValue($row, 'hdnGCItemUnit', result.GCItemUnit);
                    grdCompound.setCellHiddenValue($row, 'hdnItemUnit', result.ItemUnit);
                    grdCompound.setCellHiddenValue($row, 'hdnDose', result.Dose);
                    grdCompound.setCellHiddenValue($row, 'hdnGCStockDeductionType', result.GCStockDeductionType);
                    grdCompound.setCellHiddenValue($row, 'hdnGCConsumptionDeductionType', result.GCConsumptionDeductionType);
                    grdCompound.setCellHiddenValue($row, 'hdnSignaID', result.SignaID);

                    setConversionText($row);
                    $row.find('.txtCompoundQty').focus();
                });
            }
        });
        grdCompound.addRow(true);

        var param = $('#<%=hdnQueryString.ClientID %>').val().split('|');
        var prescriptionDetailID = -1;
        var prescriptionIDEntryCompound = parseInt(param[0]);
        if (param.length > 2) {
            prescriptionDetailID = parseInt(param[2]);
        }

        if (prescriptionDetailID > -1) {
            var filterExpression = "PrescriptionTemplateDetailID = " + prescriptionDetailID + " OR ParentID = " + prescriptionDetailID + " AND OrderIsDeleted = 0 ORDER BY PrescriptionTemplateDetailID";
            Methods.getListObject("GetvPrescriptionTemplateDtList", filterExpression, function (result) {
                for (var i = 0; i < result.length; ++i) {
                    var entity = result[i];
                    grdCompound.addRow();
                    $row = grdCompound.getRow(i);
                    fillTableData($row, entity);
                }
            });
        }
    }

    //#region TemplateCompound

    $('#<%:lblTemplateCode.ClientID %>.lblLink').live('click', function () {
        var paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
        openSearchDialog('physiciancompoundtemplate', "ParamedicID = " + paramedicID, function (value) {
            onTemplateCodeChanged(value);
        });
    });

    $('#<%:txtTemplateCode.ClientID %>').live('change', function () {
        onTemplateCodeChanged($(this).val());
    });

    function onTemplateCodeChanged(value) {
        $('#<%:txtTemplateCode.ClientID %>').val(value);
        var templateCode = value;
        var filterExpression = "CompoundTemplateCode = '" + templateCode + "' ORDER BY DisplayOrder";
        Methods.getListObject("GetvCompoundTemplateDtList", filterExpression, function (result) {
            //Header Information
            $('#<%=txtCompoundMedicationName.ClientID %>').val(result[0].CompoundTemplateName);
            $('#<%=txtTemplateName.ClientID %>').val(result[0].CompoundTemplateName);
            $('#<%=txtFrequencyNumber.ClientID %>').val(result[0].Frequency);
            $('#<%=txtDosingDose.ClientID %>').val(result[0].NumberOfDosage);
            cboDosingUnitCompoundCtl.SetValue(result[0].GCDosingUnit);
            $('#<%=txtDosingDuration.ClientID %>').val(result[0].DosingDuration);
            $('#<%=txtDispenseQty.ClientID %>').val(result[0].DispenseQuantity);
            if (result[0].IsUseSweetener) {
                $('#<%:chkIsUsingSweetener.ClientID %>').prop("checked", true);
            }
            else {
                $('#<%:chkIsUsingSweetener.ClientID %>').prop("checked", false);
            }
            if (result[0].IsAsRequired) {
                $('#<%:chkIsPRN.ClientID %>').prop("checked", true);
            }
            else {
                $('#<%:chkIsPRN.ClientID %>').prop("checked", false);
            }

            cboMedicationRouteCompoundCtl.SetValue(result[0].GCMedicationRoute);
            cboCoenamRuleCompoundCtl.SetValue(result[0].GCCoenamRule);
            cboEmbalaceCompoundCtl.SetValue(result[0].EmbalaceID);
            $('#<%=txtMedicationAdministration.ClientID %>').val(result[0].MedicationAdministration);

            for (var j = 0; j < rowCountCompoundBefore; j++) {
                $row = grdCompound.getRow(j);
                grdCompound.removeRow($row);
            }

            for (var i = 0; i < result.length; i++) {
                var entity = result[i];
                grdCompound.addRow();
                $row = grdCompound.getRow(i);
                fillTableData($row, entity);
            }

            $('#<%=txtDispenseQty.ClientID %>').focus();

            rowCountCompoundBefore = result.length;
        });
    }

    //#endregion

    function fillTableData($row, entity) {
        grdCompound.setKeyValue($row, entity.PrescriptionTemplateDetailID);
        cboCompoundUnit.ClearItems();
        if (entity.ItemGCDoseUnit != "") {
            cboCompoundUnit.AddItem(entity.ItemDoseUnit, entity.ItemGCDoseUnit);
            grdCompound.setCellHiddenValue($row, 'hdnGCDoseUnit', entity.ItemGCDoseUnit);
            grdCompound.setCellHiddenValue($row, 'hdnDoseUnit', entity.ItemDoseUnit);
        }
            cboCompoundUnit.AddItem(entity.ItemUnit, entity.GCItemUnit);
            grdCompound.setCellHiddenValue($row, 'hdnGCItemUnit', entity.GCItemUnit);
            grdCompound.setCellHiddenValue($row, 'hdnItemUnit', entity.ItemUnit);
            grdCompound.setCellHiddenValue($row, 'hdnDose', entity.ItemDose);
            grdCompound.setSearchTextBoxProperties($row, 'ledProduct', { "value": entity.ItemID });
            grdCompound.setTextBoxProperties($row, 'txtStrengthAmount', { "value": entity.Dose });
            grdCompound.setComboBoxProperties($row, 'cboCompoundStrengthUnit', { "value": entity.GCDoseUnit });
            grdCompound.setTextBoxProperties($row, 'txtCompoundQty', { "value": entity.CompoundQtyInString });
            grdCompound.setComboBoxProperties($row, 'cboCompoundUnit', { "value": entity.GCCompoundUnit });
            grdCompound.setCellHiddenValue($row, 'hdnDrugName', entity.DrugName);
            grdCompound.setCellHiddenValue($row, 'hdnGCStockDeductionType', entity.GCStockDeductionType);
            grdCompound.setCellHiddenValue($row, 'hdnGCConsumptionDeductionType', entity.GCConsumptionDeductionType);
            grdCompound.setCellHiddenValue($row, 'hdnPrescriptionOrderDtID', entity.PrescriptionTemplateDetailID);
            grdCompound.setCellHiddenValue($row, 'hdnSignaID', entity.SignaID);
            setConversionText($row);
    }

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

    //#region calculate Dispense Qty
    $('#<%=txtFrequencyNumber.ClientID %>').live('change', function () {
        SetMedicationDefaultTime($('#<%=txtFrequencyNumber.ClientID %>').val());
        var dispQty = $('#<%=txtDispenseQty.ClientID %>').val();
        calculateDispenseQty();
    });

    $('#<%=txtFrequencyNumber.ClientID %>').live('input', function () {
        $('#<%=txtSignaName1Compound.ClientID %>').val('');
        $('#<%=txtSignaLabelCompound.ClientID %>').val('');
        $('#<%=hdnSignaIDCompound.ClientID %>').val('');
    });

    $('#<%=txtDosingDose.ClientID %>').live('change', function () {
        var dispQty = $('#<%=txtDispenseQty.ClientID %>').val();
        calculateDispenseQty();
    });

    $('#<%=txtDosingDose.ClientID %>').live('input', function () {
        $('#<%=txtSignaName1Compound.ClientID %>').val('');
        $('#<%=txtSignaLabelCompound.ClientID %>').val('');
        $('#<%=hdnSignaIDCompound.ClientID %>').val('');
    });

    $('#<%=txtDosingDuration.ClientID %>').live('change', function () {
        var dispQty = $('#<%=txtDispenseQty.ClientID %>').val();
        calculateDispenseQty();
    });

    function cboFrequencyTimelineCompoundCtlChanged() {
        $('#<%=txtSignaName1Compound.ClientID %>').val('');
        $('#<%=txtSignaLabelCompound.ClientID %>').val('');
        $('#<%=hdnSignaIDCompound.ClientID %>').val('');
    }

    function cboDosingUnitCompoundCtlChanged() {
        $('#<%=txtSignaName1Compound.ClientID %>').val('');
        $('#<%=txtSignaLabelCompound.ClientID %>').val('');
        $('#<%=hdnSignaIDCompound.ClientID %>').val('');
    }

    function calculateDispenseQty() {
        var frequency = $('#<%=txtFrequencyNumber.ClientID %>').val();
        var dose = $('#<%=txtDosingDose.ClientID %>').val();
        var dosingDuration = $('#<%=txtDosingDuration.ClientID %>').val();

        var dispenseQty = dosingDuration * frequency * dose;
        $('#<%=txtDispenseQty.ClientID %>').val(dispenseQty);
        $('#<%=txtDispenseQty.ClientID %>').change();
    }
    //#endregion

    function onBeforeSaveRecordEntryPopup() {
        $('#<%=hdnInlineEditingData.ClientID %>').val(grdCompound.getTableData());
        return true;
    }

</script>

<input type="hidden" value="" id="hdnInlineEditingData" runat="server" />
<input type="hidden" value="" id="hdnQueryString" runat="server" />
<input type="hidden" value="0" id="hdnParamedicID" runat="server" />
<input type="hidden" value="" id="hdnSignaIDCompound" runat="server" />
<input type="hidden" value="" id="hdnIsAddNew" runat="server" />
<input type="hidden" value="0" id="hdnIsDefaultUsingStrengthUnit" runat="server" />
<input type="hidden" value="" id="hdnPrescriptionValidateStockAllRS" runat="server" />
<input type="hidden" value="0" id="hdnIsEmbalaceQtyTerisiOtomatis" runat="server" />
<table id="tblTemplate" style="width:100%" runat="server" cellpadding="0" cellspacing="0">
    <tr>
        <td style="width:130px;" class="tdLabel"><label id="lblTemplateCode" class="lblNormal lblLink" runat="server" style="padding-left:5px"><%=GetLabel("Template")%></label></td>
        <td style="width:100px"><asp:TextBox runat="server" ID="txtTemplateCode" Width="100%" /></td>
        <td><asp:TextBox runat="server" ID="txtTemplateName" Width="100%" ReadOnly="true" /></td>
    </tr>
</table>
<table style="width:100%">
    <colgroup>
        <col style="width:50%"/>
    </colgroup>
    <tr>
        <td valign="top">
            <table style="width:100%" cellpadding="1" cellspacing="1">
                <colgroup>
                    <col width="180px"/>
                    <col width="40px" />
                    <col width="60px" />
                    <col width="40px" />
                    <col width="50px" />
                    <col  />
                </colgroup>
                <tr>
                    <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Compound Name")%></label></td>
                    <td colspan="4"><asp:TextBox runat="server" ID="txtCompoundMedicationName" Width="300px" /></td>
                </tr>
                <tr>
                    <td class="tdLabel">
                        <label class="lblNormal lblLink" id="lblSigna">
                            <%=GetLabel("Signa Template")%></label>
                    </td>
                    <td colspan="2">
                        <asp:TextBox runat="server" ID="txtSignaLabelCompound" Width="100%" />
                    </td>
                    <td colspan="4">
                        <asp:TextBox runat="server" ID="txtSignaName1Compound" Width="100%" ReadOnly="true" TabIndex="999" />
                    </td>
                </tr>
                <tr>
                    <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Frekuensi dan Dosis")%></label></td>
                    <td><asp:TextBox ID="txtFrequencyNumber" runat="server" Width="100%" CssClass="number" /></td>
                    <td><dxe:ASPxComboBox ID="cboFrequencyTimelineCompoundCtl" ClientInstanceName="cboFrequencyTimelineCompoundCtl"
                            runat="server" Width="100%" >
                            <ClientSideEvents SelectedIndexChanged="function(s,e){ cboFrequencyTimelineCompoundCtlChanged(); }"/>
                        </dxe:ASPxComboBox>
                    </td>
                    <td><asp:TextBox ID="txtDosingDose" runat="server" Width="100%" CssClass="number" /></td>
                    <td>
                        <dxe:ASPxComboBox ID="cboDosingUnitCompoundCtl" ClientInstanceName="cboDosingUnitCompoundCtl"
                            runat="server" Width="100%" >
                            <ClientSideEvents ValueChanged="function(s,e){ cboDosingUnitCompoundCtlChanged(); }" />
                        </dxe:ASPxComboBox>
                    </td>
                </tr>
                <tr>
                    <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Duration (day)")%></label></td>
                    <td><asp:TextBox runat="server" ID="txtDosingDuration" Width="100%" CssClass="number" /></td>
                    <td class="tdLabel" style="text-align:center"><label class="lblMandatory"><%=GetLabel("Quantity")%></label></td>
                    <td><asp:TextBox runat="server" ID="txtDispenseQty" Width="100%" CssClass="number" /></td>
                </tr>
                <tr>
                    <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Medication Route")%></label></td>
                    <td colspan="2"><dxe:ASPxComboBox runat="server" ID="cboMedicationRouteCompoundCtl" ClientInstanceName="cboMedicationRouteCompoundCtl" Width="100%" /></td>
                    <td class="tdLabel" style="text-align:right; padding-right:3px"><label class="lblNormal"><%=GetLabel("AC/DC/PC")%></label></td>
                    <td><dxe:ASPxComboBox runat="server" ID="cboCoenamRuleCompoundCtl" ClientInstanceName="cboCoenamRuleCompoundCtl" Width="100%" /></td>
                </tr>
                <tr>
                    <td></td>
                    <td colspan="2"><asp:CheckBox runat="server" ID="chkIsUsingSweetener" /><%=GetLabel("Sweetener")%></td>
                    <td colspan="2"><asp:CheckBox runat="server" ID="chkIsPRN" /><%=GetLabel(" PRN")%></td>
                </tr>
            </table>
        </td>
        <td valign="top">
            <table style="width:100%">
                <colgroup>
                    <col style="width:135px"/>
                </colgroup>
                <tr>
                    <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Start Date / Time")%></label></td>
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
                                <td style="display:none"><asp:TextBox runat="server" ID="txtStartTime" CssClass="time" Width="100%" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr style="display:none">
                    <td class="tdLabel">
                        <label class="lblNormal">
                            <%=GetLabel("Taken Time")%></label>
                    </td>
                    <td>
                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td style="width:26%"><asp:CheckBox ID="chkIsMorning" runat="server" Text = "Morning" Checked="false" /></td>
                                <td style="width:24%"><asp:CheckBox ID="chkIsNoon" runat="server" Text = "Noon" Checked="false" /></td>
                                <td style="width:26%"><asp:CheckBox ID="chkIsEvening" runat="server" Text = "Evening" Checked="false" /></td>
                                <td style="width:24%"><asp:CheckBox ID="chkIsNight" runat="server" Text = "Night" Checked="false" /></td>
                            </tr>
                        </table>                                    
                    </td>
                </tr>
                <tr>
                    <td class="tdLabel" style="vertical-align: top; padding-top: 2px;"><label class="lblNormal"><%=GetLabel("Special Instruction")%></label></td>
                    <td><asp:TextBox ID="txtMedicationAdministration" Width="250px" runat="server" TextMode="MultiLine" Height="50px" /></td>
                </tr>
                <tr>
                    <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Embalace")%></label></td>
                    <td>
                        <table border="0" cellpadding="0" cellspacing="0">
                            <colgroup>
                                <col width="100px" />
                                <col width="80px" />
                                <col width="50px" />
                            </colgroup>
                            <tr>
                                <td><dxe:ASPxComboBox runat="server" ID="cboEmbalaceCompoundCtl" ClientInstanceName="cboEmbalaceCompoundCtl" Width="100%" /></td>
                                <td class="tdLabel" style="text-align:right; padding-left:10px; padding-right:10px"><label class="lblNormal"><%=GetLabel("Quantity")%></label></td>
                                <td><asp:TextBox runat="server" ID="txtEmbalaceQty" Width="100%" CssClass="number" /></td>
                            </tr>
                        </table>                        
                    </td>
                </tr>
                <tr>
                    <td class="tdLabel">
                        <label class="lblNormal">
                            <%=GetLabel("Waktu Pemberian")%></label>
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
                                <td style="width: 15%" align="center"">
                                    <label class="lblNormal">6</label>
                                </td>
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
            </table>
        </td>
    </tr>
</table>

<div style="width:100%; height:300px; overflow-y:scroll;">
    <table class="grdNormal" id="tblPrescriptionCompound" style="width:100%;font-size:0.95em" cellpadding="0" cellspacing="0">
        <colgroup>
            <col style="width:30px"/>
            <col/>
            <col style="width:70px"/>
            <col style="width:100px"/>
            <col style="width:70px"/>
            <col style="width:100px"/>
            <col style="width:170px"/>
        </colgroup>
        <tr>
            <th align="center" rowspan="2">#</th>
            <th align="left" rowspan="2"><%=GetLabel("ITEM")%></th>
            <th align="center" colspan="2"><%=GetLabel("STRENGTH")%></th>
            <th align="center" colspan="3"><%=GetLabel("COMPOUND FORMULA")%><span style="color:Red; font-weight:bold"><%=GetLabel(" (metode d.t.d)")%></span></th>
        </tr>
        <tr>
            <th align="center"><%=GetLabel("Amount")%></th>
            <th align="center"><%=GetLabel("Unit")%></th>
            <th align="center"><%=GetLabel("Quantity")%></th>
            <th align="center"><%=GetLabel("Unit")%></th>
            <th align="center"><%=GetLabel("Conversion")%></th>
        </tr>
    </table>
</div>
<dxe:ASPxComboBox ID="cboCompoundStrengthUnit" ClientInstanceName="cboCompoundStrengthUnit" runat="server" Width="100%" 
    EnableSynchronization="False" ClientVisible="false" IncrementalFilteringMode="Contains" >
    <ClientSideEvents LostFocus="function(s,e){grdCompound.hideAspxComboBox(s);}" 
        KeyDown="grdCompound.onCboKeyDown"
        Init="function(s,e){ numFinishLoad++; if(numFinishLoad == 3) init(); }" />
</dxe:ASPxComboBox>
<dxe:ASPxComboBox ID="cboCompoundUnit" ClientInstanceName="cboCompoundUnit" runat="server" Width="100%" 
    EnableSynchronization="False" ClientVisible="false" IncrementalFilteringMode="Contains" >
    <ClientSideEvents LostFocus="function(s,e){grdCompound.hideAspxComboBox(s);}" 
        KeyDown="grdCompound.onCboKeyDown"
        Init="function(s,e){ numFinishLoad++; if(numFinishLoad == 3) init(); }" />
</dxe:ASPxComboBox>
<qis:QISSearchTextBox ID="ledProduct" ClientInstanceName="ledProduct" runat="server" FilterExpression="IsDeleted = 0 AND ISNULL(GCItemStatus,'') != 'X181^999'"
    ValueText="ItemID" DisplayText="ItemName1" MethodName="GetvDrugInfoList" ClientVisible="false" Width="99.9%" >
    <ClientSideEvents Init="function(s,e){ numFinishLoad++; if(numFinishLoad == 3) init(); }" LostFocus="function(s){onLedProductLostFocus(s); }" />
    <Columns>
        <qis:QISSearchTextBoxColumn Caption="Item Name" FieldName="ItemName1" Description="i.e. Panadol" Width="300px" />
        <qis:QISSearchTextBoxColumn Caption="Generic Name" FieldName="GenericName" Description="i.e. paracetamol"
            Width="100px" />
    </Columns>
</qis:QISSearchTextBox>
<div id="containerCbo" style="display:none">

</div>

   