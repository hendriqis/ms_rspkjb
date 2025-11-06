<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PrescriptionCompoundEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PrescriptionCompoundEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<script type="text/javascript" id="dxis_prescriptioncompoundentryctl" src='<%= ResolveUrl("~/Libs/Scripts/inlineEditing-1.0.js")%>'></script>
<script type="text/javascript" id="dxss_prescriptioncompoundentryctl">
    setDatePicker('<%=txtStartDate.ClientID %>');

    var grdCompound = new InlineEditing();
    var listParam = new Array();
    var cboCompoundStrengthUnitID = '<%=cboCompoundStrengthUnit.ClientID%>';
    var cboCompoundUnitID = '<%=cboCompoundUnit.ClientID%>';
    var ledProductID = '<%=ledProduct.ClientID%>';
    listParam[0] = { "type": "txt", "className": "txtGenericName", "isEnabled": true };
    listParam[1] = { "type": "led", "className": "ledProduct", "ledID": ledProductID, "isEnabled": true };
    listParam[2] = { "type": "txt", "dataType": "float", "className": "txtStrengthAmount", "isEnabled": true };
    listParam[3] = { "type": "cbo", "className": "cboCompoundStrengthUnit", "cboID": cboCompoundStrengthUnitID, "isUnique": false, "isEnabled": true };
    listParam[4] = { "type": "txt", "dataType": "float", "className": "txtCompoundQty", "isEnabled": true };
    listParam[5] = { "type": "cbo", "className": "cboCompoundUnit", "cboID": cboCompoundUnitID, "isUnique": false, "isEnabled": true };
    listParam[6] = { "type": "txt", "className": "txtConversionFactor", "isEnabled": false };
    listParam[7] = { "type": "hdn", "className": "hdnDrugName" };
    listParam[8] = { "type": "hdn", "className": "hdnDose" }; 30777              

    listParam[9] = { "type": "hdn", "className": "hdnDoseUnit" };
    listParam[10] = { "type": "hdn", "className": "hdnGCDoseUnit" };
    listParam[11] = { "type": "hdn", "className": "hdnItemUnit" };
    listParam[12] = { "type": "hdn", "className": "hdnGCItemUnit" };

    window.onLedProductLostFocus = function (s) {
        grdCompound.hideQisSearchTextBox(s);
    }

    var listItemID = [];

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

            if (newValue != "")
                var filterExpression = "ItemID = " + newValue;
            else
                var filterExpression = "";

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

                grdCompound.setCellHiddenValue($row, 'hdnDrugName', result.DrugName);
                grdCompound.setCellHiddenValue($row, 'hdnGCItemUnit', result.GCItemUnit);
                grdCompound.setCellHiddenValue($row, 'hdnItemUnit', result.ItemUnit);
                grdCompound.setCellHiddenValue($row, 'hdnDose', result.Dose);

                setConversionText($row);
                $row.find('.txtStrengthAmount').focus();
            });

        });
        grdCompound.addRow(true);

        var param = $('#<%=hdnQueryString.ClientID %>').val().split('|');
        var prescriptionDetailID = -1;
        var prescriptionIDEntryCompound = parseInt(param[0]);
        if (param.length <= 4) {
            prescriptionDetailID = parseInt(param[1]);
        }

        if (prescriptionDetailID > -1) {
            var filterExpression = "PrescriptionOrderDetailID = " + prescriptionDetailID + " OR ParentID = " + prescriptionDetailID + " AND IsDeleted = 0";
            Methods.getListObject("GetvPrescriptionOrderDtList", filterExpression, function (result) {
                for (var i = 0; i < result.length; ++i) {
                    var entity = result[i];
                    grdCompound.addRow();
                    $row = grdCompound.getRow(i);
                    fillTableData($row, entity);
                }
                if (entity.IsUseSweetener == true)
                    $('#<%=chkIsUsingSweetener.ClientID %>').attr('checked', 'checked')
            });
        }
        if (prescriptionIDEntryCompound > 0) {
            var filterExpression = '';
            if (prescriptionDetailID > 0)
                filterExpression = 'PrescriptionOrderID = ' + prescriptionIDEntryCompound + ' AND PrescriptionOrderDetailID != ' + prescriptionDetailID + ' AND (ParentID != ' + prescriptionDetailID + ' OR ParentID IS NULL) AND IsDeleted = 0';
            else
                filterExpression = 'PrescriptionOrderID = ' + prescriptionIDEntryCompound + ' AND IsDeleted = 0';
            Methods.getListObject('GetPrescriptionOrderDtList', filterExpression, function (result) {
                listItemID = result;
            });
        }

        $('#<%=txtDosingDurationTimeline.ClientID %>').val(cboFrequencyTimelineCtl.GetText());
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

        grdCompound.setTextBoxProperties($row, 'txtGenericName', { "value": entity.GenericName });
        grdCompound.setSearchTextBoxProperties($row, 'ledProduct', { "value": entity.ItemID });
        grdCompound.setTextBoxProperties($row, 'txtStrengthAmount', { "value": entity.Dose });
        grdCompound.setComboBoxProperties($row, 'cboCompoundStrengthUnit', { "value": entity.GCDoseUnit });
        grdCompound.setTextBoxProperties($row, 'txtCompoundQty', { "value": entity.CompoundQtyInString.split('/')[0] });
        grdCompound.setComboBoxProperties($row, 'cboCompoundUnit', { "value": entity.GCCompoundUnit });
        grdCompound.setCellHiddenValue($row, 'hdnDrugName', entity.DrugName);
        setConversionText($row);
    }

    function onBeforeSaveRecord() {
        $('#<%=hdnInlineEditingData.ClientID %>').val(grdCompound.getTableData());
        return true;
    }

    function OncboFrequencyTimelineCtlChanged() {
        var frequencyTimeLine = cboFrequencyTimelineCtl.GetText();
        $('#<%=txtDosingDurationTimeline.ClientID %>').val(frequencyTimeLine);
    }

    function OncboDosingUnitCtlValueChanged() {
        var dosingUnit = cboDosingUnitCtl.GetText();
        $('#<%=txtDispenseUnit.ClientID %>').val(dosingUnit);
    }

    $('#<%=txtFrequencyNumber.ClientID %>').live('change', function () {
        calculateDispenseQty();
    });

    $('#<%=txtDosingDose.ClientID %>').live('change', function () {
        calculateDispenseQty();
    });

    $('#<%=txtDosingDuration.ClientID %>').live('change', function () {
        calculateDispenseQty();
    });

    function calculateDispenseQty() {
        var frequency = $('#<%=txtFrequencyNumber.ClientID %>').val();
        var frequencyTimeLine = cboFrequencyTimelineCtl.GetValue();
        var dose = $('#<%=txtDosingDose.ClientID %>').val();
        var dosingDuration = $('#<%=txtDosingDuration.ClientID %>').val();

        var dispenseQty = dosingDuration * frequency * dose;
        $('#<%=txtDispenseQty.ClientID %>').val(dispenseQty);
        if (dispenseQty <= 0 || dispenseQty == "") {
            $('#<%=txtDispenseQty.ClientID %>').val('');
        }
    }

    $('#<%=txtDispenseQty.ClientID %>').live('change', function () {
        var dispenseqty = $('#<%=txtDispenseQty.ClientID %>').val();
        if (dispenseqty <= 0 || dispenseqty == "") {
            showToast('Error Message', 'Quantity Resep tidak boleh kurang dari atau sama dengan 0 !');
            $('#<%=txtDispenseQty.ClientID %>').val('');
        }
    });
</script>
<input type="hidden" value="" id="hdnInlineEditingData" runat="server" />
<input type="hidden" value="" id="hdnQueryString" runat="server" />
<input type="hidden" value="0" id="hdnIsDrugChargesJustDistributionCP" runat="server" />
<table style="width: 100%">
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
                        <label class="lblNormal">
                            <%=GetLabel("Nama Racikan")%></label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtCompoundMedicationName" Width="300px" />
                    </td>
                </tr>
                <tr>
                    <td class="tdLabel">
                        <label class="lblMandatory">
                            <%=GetLabel("Rute Obat")%></label>
                    </td>
                    <td>
                        <dxe:ASPxComboBox runat="server" ID="cboMedicationRouteCtl" Width="300px" />
                    </td>
                </tr>
                <tr>
                    <td class="tdLabel">
                        <label class="lblMandatory">
                            <%=GetLabel("Dosis")%></label>
                    </td>
                    <td>
                        <table cellpadding="0" cellspacing="0">
                            <colgroup>
                                <col style="width: 100px" />
                                <col style="width: 3px" />
                                <col style="width: 100px" />
                            </colgroup>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtDosingDose" runat="server" Width="100%" CssClass="number" />
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboDosingUnitCtl" runat="server" Width="100%" ClientInstanceName="cboDosingUnitCtl">
                                        <ClientSideEvents SelectedIndexChanged="function (s,e) { OncboDosingUnitCtlValueChanged();}" />
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                        <label class="lblNormal">
                            <%=GetLabel("Catatan Khusus")%></label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtMedicationAdministration" Width="300px" runat="server" TextMode="MultiLine" />
                    </td>
                </tr>
            </table>
        </td>
        <td valign="top">
            <table style="width: 100%">
                <colgroup>
                    <col style="width: 150px" />
                    <col style="width: 140px" />
                    <col style="width: 80px" />
                    <col />
                </colgroup>
                <tr>
                    <td class="tdLabel">
                        <label class="lblMandatory">
                            <%=GetLabel("Mulai diberikan")%></label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtStartDate" CssClass="datepicker" Width="110px" />
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtStartTime" CssClass="time" Width="60px" />
                    </td>
                    <td />
                </tr>
                <tr>
                    <td class="tdLabel">
                        <label class="lblMandatory">
                            <%=GetLabel("Frekuensi")%></label>
                    </td>
                    <td colspan="3">
                        <table border="0" cellpadding="0" cellspacing="1">
                            <colgroup>
                                <col style="width: 80px" />
                                <col style="width: 125px" />
                            </colgroup>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtFrequencyNumber" runat="server" Width="100%" CssClass="number" />
                                </td>
                                <td style="padding-left: 3px">
                                    <dxe:ASPxComboBox ID="cboFrequencyTimelineCtl" ClientInstanceName="cboFrequencyTimelineCtl"
                                        runat="server" Width="100%">
                                        <ClientSideEvents SelectedIndexChanged="function (s,e) { OncboFrequencyTimelineCtlChanged(); }" />
                                    </dxe:ASPxComboBox>
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
                    <td colspan="3">
                        <table border="0" cellpadding="0" cellspacing="1">
                            <colgroup>
                                <col style="width: 80px" />
                                <col style="width: 125px" />
                            </colgroup>
                            <tr>
                                <td>
                                    <asp:TextBox runat="server" ID="txtDosingDuration" Width="100%" CssClass="number" />
                                </td>
                                <td style="padding-left: 3px">
                                    <asp:TextBox runat="server" ID="txtDosingDurationTimeline" Width="100%" ReadOnly="true" />
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
                    <td colspan="3">
                        <table border="0" cellpadding="0" cellspacing="1">
                            <colgroup>
                                <col style="width: 80px" />
                                <col style="width: 125px" />
                            </colgroup>
                            <tr>
                                <td>
                                    <asp:TextBox runat="server" ID="txtDispenseQty" Width="100%" CssClass="number" />
                                </td>
                                <td style="padding-left: 3px">
                                    <asp:TextBox runat="server" ID="txtDispenseUnit" Width="100%" ReadOnly="true" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="tdLabel">
                        <label class="lblNormal">
                            <%=GetLabel("Tambahkan Pemanis")%></label>
                    </td>
                    <td>
                        <asp:CheckBox runat="server" ID="chkIsUsingSweetener" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<div style="width: 100%; height: 300px; overflow-y: scroll;">
    <table class="grdNormal" id="tblPrescriptionCompound" style="width: 100%; font-size: 0.9em"
        cellpadding="0" cellspacing="0">
        <colgroup>
            <col style="width: 30px" />
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
            <th align="left" rowspan="2" style="width: 120px">
                <%=GetLabel("GENERIK")%>
            </th>
            <th align="left" rowspan="2">
                <%=GetLabel("DAFTAR OBAT")%>
            </th>
            <th align="center" colspan="2">
                <%=GetLabel("KADAR")%>
            </th>
            <th align="center" colspan="3">
                <%=GetLabel("FORMULA RACIKAN")%>
            </th>
        </tr>
        <tr>
            <th align="center">
                <%=GetLabel("Jumlah")%>
            </th>
            <th align="center">
                <%=GetLabel("Satuan")%>
            </th>
            <th align="center">
                <%=GetLabel("Jumlah")%>
            </th>
            <th align="center">
                <%=GetLabel("Satuan")%>
            </th>
            <th align="center">
                <%=GetLabel("Faktor Konversi")%>
            </th>
        </tr>
    </table>
</div>
<dxe:ASPxComboBox ID="cboCompoundStrengthUnit" ClientInstanceName="cboCompoundStrengthUnit"
    runat="server" Width="100%" EnableSynchronization="False" ClientVisible="false"
    IncrementalFilteringMode="Contains">
    <ClientSideEvents LostFocus="function(s,e){grdCompound.hideAspxComboBox(s);}" KeyDown="grdCompound.onCboKeyDown"
        Init="function(s,e){ numFinishLoad++; if(numFinishLoad == 3) init(); }" />
</dxe:ASPxComboBox>
<dxe:ASPxComboBox ID="cboCompoundUnit" ClientInstanceName="cboCompoundUnit" runat="server"
    Width="100%" EnableSynchronization="False" ClientVisible="false" IncrementalFilteringMode="Contains">
    <ClientSideEvents LostFocus="function(s,e){grdCompound.hideAspxComboBox(s);}" KeyDown="grdCompound.onCboKeyDown"
        Init="function(s,e){ numFinishLoad++; if(numFinishLoad == 3) init(); }" />
</dxe:ASPxComboBox>
<qis:QISSearchTextBox ID="ledProduct" ClientInstanceName="ledProduct" runat="server"
    FilterExpression="IsDeleted = 0" ValueText="ItemID" DisplayText="ItemName1" MethodName="GetvDrugInfoList"
    ClientVisible="false" Width="99.9%">
    <ClientSideEvents Init="function(s,e){ numFinishLoad++; if(numFinishLoad == 3) init(); }"
        LostFocus="function(s){onLedProductLostFocus(s); }" />
    <Columns>
        <qis:QISSearchTextBoxColumn Caption="Nama Generik" FieldName="GenericName" Description="i.e. paracetamol"
            Width="100px" />
        <qis:QISSearchTextBoxColumn Caption="Nama Obat" FieldName="ItemName1" Description="i.e. Panadol"
            Width="300px" />
    </Columns>
</qis:QISSearchTextBox>
<div id="containerCbo" style="display: none">
</div>
