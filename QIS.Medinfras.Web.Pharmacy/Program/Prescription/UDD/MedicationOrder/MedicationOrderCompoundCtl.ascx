<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MedicationOrderCompoundCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.Pharmacy.Program.MedicationOrderCompoundCtl" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" 
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
    
<script type="text/javascript" id="dxis_prescriptioncompoundentryctl" src='<%= ResolveUrl("~/Libs/Scripts/inlineEditing-1.0.js")%>'></script>
<script type="text/javascript" id="dxss_prescriptioncompoundentryctl">
    $(function () {
        $('#<%=txtCompoundMedicationName.ClientID %>').focus();
    });

    setDatePicker('<%=txtStartDate.ClientID %>');

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
            if (result[0].IsUseSweetener)
                $('#<%:chkIsUsingSweetener.ClientID %>').prop("checked", true);
            else
                $('#<%:chkIsUsingSweetener.ClientID %>').prop("checked", false);

            cboMedicationRouteCompoundCtl.SetValue(result[0].GCMedicationRoute);
            cboCoenamRuleCompoundCtl.SetValue(result[0].GCCoenamRule);
            cboEmbalace.SetValue(result[0].EmbalaceID);
            $('#<%=txtMedicationAdministration.ClientID %>').val(result[0].MedicationAdministration);
            $('#<%=txtMedicationPurpose.ClientID %>').val(result[0].MedicationPurpose);
            for (var i = 0; i < result.length; ++i) {
                var entity = result[i];
                grdCompound.addRow();
                $row = grdCompound.getRow(i);
                fillTableData($row, entity);
            }
            $('#<%=txtDispenseQty.ClientID %>').focus();
        });
    }

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
                Methods.getObject('GetvDrugInfoList', filterExpression, function (result) {
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
                    grdCompound.setComboBoxProperties($row, 'cboCompoundUnit', { "value": result.GCItemUnit });

                    grdCompound.setCellHiddenValue($row, 'hdnGenericName', result.GenericName);
                    grdCompound.setCellHiddenValue($row, 'hdnGCItemUnit', result.GCItemUnit);
                    grdCompound.setCellHiddenValue($row, 'hdnItemUnit', result.ItemUnit);
                    grdCompound.setCellHiddenValue($row, 'hdnDose', result.Dose);

                    setConversionText($row);
                    $row.find('.txtCompoundQty').focus();
                });
            }
        });
        grdCompound.addRow(true);

        var param = $('#<%=hdnQueryString.ClientID %>').val().split('|');
        var prescriptionOrderID = parseInt(param[1]);
        var prescriptionDetailID = parseInt(param[5]);

        alert(prescriptionDetailID);
        if (prescriptionDetailID > -1) {
            var filterExpression = "PrescriptionOrderID = " + prescriptionOrderID + " AND (PrescriptionOrderDetailID = " + prescriptionDetailID + " OR ParentID = " + prescriptionDetailID + ") AND IsDeleted = 0";
            Methods.getListObject("GetvPrescriptionOrderDtList", filterExpression, function (result) {
                for (var i = 0; i < result.length; ++i) {
                    var entity = result[i];
                    grdCompound.addRow();
                    $row = grdCompound.getRow(i);
                    fillTableData($row, entity);
                }
            });
        }
    }
    function fillTableData($row, entity) {
        grdCompound.setKeyValue($row, entity.PrescriptionOrderDetailID);

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
        setConversionText($row);
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

    $('#<%=txtDispenseQty.ClientID %>').live('change', function () {
        var dispQty = $('#<%=txtDispenseQty.ClientID %>').val();
        $('#<%=txtEmbalaceQty.ClientID %>').val(dispQty);
    });

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
<input type="hidden" value="" id="hdnEntryOrderID" />
<input type="hidden" value="" id="hdnTransactionID" runat="server" />
<input type="hidden" value="" id="hdnLocationID" runat="server" />
<input type="hidden" value="" id="hdnImagingServiceUnitID" runat="server" />
<input type="hidden" value="" id="hdnLaboratoryServiceUnitID" runat="server" />

<table id="tblTemplate" style="width:100%" runat="server" cellpadding="0" cellspacing="0">
    <tr>
        <td style="width:154px;" class="tdLabel"><label id="lblTemplateCode" class="lblNormal lblLink" runat="server" style="padding-left:5px"><%=GetLabel("Template")%></label></td>
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
                    <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Frekuensi / Dosis")%></label></td>
                    <td><asp:TextBox ID="txtFrequencyNumber" runat="server" Width="100%" CssClass="number" /></td>
                    <td><dxe:ASPxComboBox ID="cboFrequencyTimelineCompoundCtl" runat="server" Width="100%" /></td>
                    <td><asp:TextBox ID="txtDosingDose" runat="server" Width="100%" CssClass="number" /></td>
                    <td><dxe:ASPxComboBox ID="cboDosingUnitCompoundCtl" ClientInstanceName="cboDosingUnitCompoundCtl" runat="server" Width="100%" /></td>
                </tr>
                <tr>
                    <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Duration (day)")%></label></td>
                    <td><asp:TextBox runat="server" ID="txtDosingDuration" Width="100%" CssClass="number" /></td>
                    <td class="tdLabel" style="text-align:center"><label class="lblMandatory"><%=GetLabel("Quantity")%></label></td>
                    <td><asp:TextBox runat="server" ID="txtDispenseQty" Width="100%" CssClass="number" /></td>
                    <td><asp:CheckBox runat="server" ID="chkIsUsingSweetener" /><%=GetLabel("slqs")%></td>
                </tr>
                <tr>
                    <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Embalace - Qty")%></label></td>
                    <td colspan="2"><dxe:ASPxComboBox runat="server" ID="cboEmbalace" ClientInstanceName="cboEmbalace" Width="100%" /></td>
                    <td><asp:TextBox runat="server" ID="txtEmbalaceQty" Width="80px" CssClass="number" /></td>
                    <td />
                </tr>
                <tr>
                    <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Medication Route")%></label></td>
                    <td colspan="2"><dxe:ASPxComboBox runat="server" ID="cboMedicationRouteCompoundCtl" ClientInstanceName="cboMedicationRouteCompoundCtl" Width="100%" /></td>
                    <td class="tdLabel"><label class="lblNormal"><%=GetLabel("AC/DC/PC")%></label></td>
                    <td><dxe:ASPxComboBox runat="server" ID="cboCoenamRuleCompoundCtl" ClientInstanceName="cboCoenamRuleCompoundCtl" Width="100%" /></td>
                </tr>
            </table>
        </td>
        <td valign="top">
            <table style="width:100%">
                <colgroup>
                    <col style="width:160px"/>
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
                                <td><asp:TextBox runat="server" ID="txtStartTime" CssClass="time" Width="100%" /></td>
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
                                <td style="width:26%"><asp:CheckBox ID="chkIsMorning" runat="server" Text = "Morning" Checked="false" /></td>
                                <td style="width:24%"><asp:CheckBox ID="chkIsNoon" runat="server" Text = "Noon" Checked="false" /></td>
                                <td style="width:26%"><asp:CheckBox ID="chkIsEvening" runat="server" Text = "Evening" Checked="false" /></td>
                                <td style="width:24%"><asp:CheckBox ID="chkIsNight" runat="server" Text = "Night" Checked="false" /></td>
                            </tr>
                        </table>                                    
                    </td>
                </tr>
                <tr>
                    <td class="tdLabel" style="vertical-align: top; padding-top: 2px;"><label class="lblNormal"><%=GetLabel("Administration Instruction")%></label></td>
                    <td><asp:TextBox ID="txtMedicationAdministration" Width="250px" runat="server" TextMode="MultiLine" Height="50px" /></td>
                </tr>
                <tr>
                    <td class="tdLabel" style="vertical-align: top; padding-top: 2px;"><label class="lblNormal"><%=GetLabel("Medication Purpose")%></label></td>
                    <td><asp:TextBox ID="txtMedicationPurpose" Width="250px" runat="server" TextMode="MultiLine" Height="50px" /></td>
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
            <th align="Left" rowspan="2"><%=GetLabel("ITEM")%></th>
            <th align="center" colspan="2"><%=GetLabel("STRENGTH")%></th>
            <th align="center" colspan="3"><%=GetLabel("COMPOUND FORMULA")%></th>
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
<qis:QISSearchTextBox ID="ledProduct" ClientInstanceName="ledProduct" runat="server" FilterExpression="IsDeleted = 0"
    ValueText="ItemID" DisplayText="ItemName1" MethodName="GetvItemBalanceList" ClientVisible="false" Width="99.9%" >
    <ClientSideEvents Init="function(s,e){ numFinishLoad++; if(numFinishLoad == 3) init(); }" LostFocus="function(s){onLedProductLostFocus(s); }" />
    <Columns>
        <qis:QISSearchTextBoxColumn Caption="Item Name" FieldName="ItemName1" Description="i.e. Panadol" Width="300px" />
        <qis:QISSearchTextBoxColumn Caption="Balance" FieldName="QuantityEND" Description="Quantity on Hand per location"
            Width="100px" />
        <qis:QISSearchTextBoxColumn Caption="Unit" FieldName="ItemUnit" Description="Item Unit"
            Width="60px" />
    </Columns>
</qis:QISSearchTextBox>
<div id="containerCbo" style="display:none">

</div>

   