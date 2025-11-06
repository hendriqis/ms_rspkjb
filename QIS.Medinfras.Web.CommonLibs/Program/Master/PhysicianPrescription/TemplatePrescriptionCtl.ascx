<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TemplatePrescriptionCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.TemplatePrescriptionCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_compoundtemplateformulactl">
    $('#lblEntryPopupAddData').live('click', function () {
        $('#<%=hdnIsFlagAdd.ClientID %>').val('1');
        $('#lblItem').attr('class', 'lblLink lblMandatory');
        $('#<%=hdnItemID.ClientID %>').val('');
        $('#<%=txtItemCode.ClientID %>').removeAttr('readonly');
        $('#<%=txtItemCode.ClientID %>').val('');
        $('#<%=txtItemName.ClientID %>').val('');
        $('#<%=txtFrequencyNumber.ClientID %>').val('1');
        $('#<%=txtDosingDose.ClientID %>').val('1');
        $('#<%=txtDosingDuration.ClientID %>').val('1');

        $('#containerPopupEntryData').show();
    });

    $('#btnEntryPopupCancel').live('click', function () {
        $('#containerPopupEntryData').hide();
    });

    $('#btnEntryPopupSave').click(function (evt) {
        if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup'))
            cbpEntryPopupView.PerformCallback('save');
    });

    $('.imgDelete.imgLink').die('click');
    $('.imgDelete.imgLink').live('click', function () {
        if (confirm) {
            $row = $(this).closest('tr');
            var prescriptiontemplateDetailID = $row.find('.hdnPrescriptionTemplateDetailID').val();
            $('#<%=hdnPrescriptionTemplateDetailID.ClientID %>').val(prescriptiontemplateDetailID);
            cbpEntryPopupView.PerformCallback('delete');
        }
    });

    $('.imgEdit.imgLink').die('click');
    $('.imgEdit.imgLink').live('click', function () {
        $('#containerPopupEntryData').show();
        $row = $(this).closest('tr');
        var entity = rowToObject($row);
        var itemID = $row.find('.hdnItemID').val();
        var itemCode = $row.find('.hdnItemCode').val();
        var itemName = $row.find('.hdnItemName').val();
        var strengthAmount = $row.find('.hdnStrengthAmount').val();
        var strengthUnit = $row.find('.hdnStrengthUnit').val();
        var doseUnit = $row.find('.hdnGCDosingUnit').val();
        var frequency = $row.find('.hdnFrequency').val();
        var dose = $row.find('.hdnDose').val();
        var isMorning = $row.find('.hdnIsMorning').val();
        var isNoon = $row.find('.hdnIsNoon').val();
        var isEvening = $row.find('.hdnIsEvening').val();
        var isNight = $row.find('.hdnIsNight').val();
        var isAsRequired = $row.find('.hdnIsAsRequired').val();
        var coenamRule = $row.find('.hdnGCCoenamRule').val();

        $('#<%=hdnIsFlagAdd.ClientID %>').val('0');
        $('#lblItem').attr('class', 'lblDisabled');
        $('#<%=hdnEntryID.ClientID %>').val(entity.PrescriptionTemplateDetailID);
        $('#<%=lvwView.ClientID %> tr.focus').addClass('selected');
        $('#<%=hdnItemID.ClientID %>').val(itemID);
        $('#<%=txtItemCode.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtItemCode.ClientID %>').val(itemCode);
        $('#<%=txtItemCode.ClientID %>').trigger('change');
        $('#<%=txtItemName.ClientID %>').val(itemName);
        $('#<%=hdnGCDoseUnit.ClientID %>').val(doseUnit);
        $('#<%=txtFrequencyNumber.ClientID %>').val(frequency);
        $('#<%=txtDosingDose.ClientID %>').val(dose);
        $('#<%=txtDosingDose.ClientID %>').val(dose);
        $('#<%=hdnGCCoenamRule.ClientID %>').val(coenamRule);
        if (isMorning == "1")
            $('#<%=chkIsMorning.ClientID %>').prop('checked', true);
        else
            $('#<%=chkIsMorning.ClientID %>').prop('checked', false);
        if (isNoon == "1")
            $('#<%=chkIsNoon.ClientID %>').prop('checked', true);
        else
            $('#<%=chkIsNoon.ClientID %>').prop('checked', false);
        if (isEvening == "1")
            $('#<%=chkIsEvening.ClientID %>').prop('checked', true);
        else
            $('#<%=chkIsEvening.ClientID %>').prop('checked', false);
        if (isNight == "1")
            $('#<%=chkIsNight.ClientID %>').prop('checked', true);
        else
            $('#<%=chkIsNight.ClientID %>').prop('checked', false);
        if (isAsRequired == "1")
            $('#<%=chkIsAsRequired.ClientID %>').prop('checked', true);
        else
            $('#<%=chkIsAsRequired.ClientID %>').prop('checked', false);

        cboDosingUnit.SetValue(doseUnit);

        calculateDispenseQty();
    });

    function onAfterSaveAddRecordEntryPopup(param) {
        return $('#<%=hdnPrescriptionTemplateID.ClientID %>').val(param);
    }

    function onGetEntryPopupReturnValue() {
        var result = $('#<%=hdnPrescriptionTemplateID.ClientID %>').val();
        return result;
    }

    function onCbpEntryPopupViewEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else
                $('#containerPopupEntryData').hide();
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail')
                showToast('Delete Failed', 'Error Message : ' + param[2]);
        }
        $('#<%=hdnPrescriptionTemplateDetailID.ClientID %>').val();
        $('#containerImgLoadingViewPopup').hide();
    }

    //#region Item
    function onGetItemMasterFilterExpression() {
        var filterExpression = "<%:OnGetItemMasterFilterExpression() %>";
        return filterExpression;
    }

    $('#lblItem.lblLink').die('click');
    $('#lblItem.lblLink').live('click', function () {
        openSearchDialog('item', onGetItemMasterFilterExpression(), function (value) {
            $('#<%=txtItemCode.ClientID %>').val(value);
            onTxtItemCodeChanged(value);
        });
    });

    $('#<%=txtItemCode.ClientID %>').die('change');
    $('#<%=txtItemCode.ClientID %>').live('change', function () {
        onTxtItemCodeChanged($(this).val());
    });

    function onTxtItemCodeChanged(value) {
        var filterExpression = onGetItemMasterFilterExpression() + " AND ItemCode = '" + value + "'";
        Methods.getObject('GetvDrugInfoList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnItemID.ClientID %>').val(result.ItemID);
                $('#<%=txtItemCode.ClientID %>').val(result.ItemCode);
                $('#<%=txtItemName.ClientID %>').val(result.ItemName1);
                $('#<%=txtDispenseUnit.ClientID %>').val(result.ItemUnit);
                $('#<%=txtStrengthUnit.ClientID %>').val(result.DoseUnit);
                $('#<%=txtStrengthAmount.ClientID %>').val(result.Dose);
                $('#<%=hdnGCItemUnit.ClientID %>').val(result.GCItemUnit);

                cboDosingUnit.SetValue(result.GCItemUnit);
            }
            else {
                $('#<%=hdnItemID.ClientID %>').val('');
                $('#<%=txtItemCode.ClientID %>').val('');
                $('#<%=txtItemName.ClientID %>').val('');
                $('#<%=txtDispenseUnit.ClientID %>').val('');
                $('#<%=txtStrengthUnit.ClientID %>').val('');
                $('#<%=txtStrengthAmount.ClientID %>').val('');
                $('#<%=hdnGCItemUnit.ClientID %>').val('');

                cboDosingUnit.SetValue('');
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
        Methods.getObject('GetvSignaList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnSignaID.ClientID %>').val(result.SignaID);
                $('#<%=txtSignaName1.ClientID %>').val(result.SignaName1);
                cboCoenamRule.SetValue(result.GCCoenamRule);
                $('#<%=txtFrequencyNumber.ClientID %>').val(result.Frequency);
                $('#<%=txtFrequencyNumber.ClientID %>').change();
                cboDosingUnit.SetValue(result.GCDoseUnit);
                $('#<%=hdnGCDosingUnit.ClientID %>').val(result.GCDosingUnit);
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
        calculateDispenseQty();
    });

    $('#<%=txtDosingDose.ClientID %>').live('change', function () {
        calculateDispenseQty();
    });

    $('#<%=txtDosingDuration.ClientID %>').live('change', function () {
        var dosingUnit = cboDosingUnit.GetText();
        var itemUnit = $('#<%=txtDispenseUnit.ClientID %>').val();
        if (dosingUnit == itemUnit) {
            calculateDispenseQty();
        }
    });

    function calculateDispenseQty() {
        var frequency = $('#<%=txtFrequencyNumber.ClientID %>').val();
        var frequencyTimeLine = cboFrequencyTimeline.GetValue();
        var dose = $('#<%=txtDosingDose.ClientID %>').val();
        var dosingDuration = $('#<%=txtDosingDuration.ClientID %>').val();
        var strengthAmount = $('#<%=txtStrengthAmount.ClientID %>').val();
        var dosingUnit = cboDosingUnit.GetText();
        var itemUnit = $('#<%=txtDispenseUnit.ClientID %>').val();

        var frequencyInt = parseInt(frequency);
        var doseInt = parseInt(dose);
        var dosingDurationInt = parseInt(dosingDuration);

        if (frequencyInt < 0) {
            $('#<%=txtFrequencyNumber.ClientID %>').val('0');
        }

        if (doseInt < 0) {
            $('#<%=txtDosingDose.ClientID %>').val('0');
        }

        if (dosingDurationInt < 0) {
            $('#<%=txtDosingDuration.ClientID %>').val('0');
        }

        var dispenseQty = 0;
        if (frequency != '' && dose != '' && dosingDuration != '' && frequencyInt > 0 && doseInt > 0 && dosingDurationInt > 0) {
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
        else {
            dispenseQty = 1;
            $('#<%=txtDispenseQty.ClientID %>').val(dispenseQty);
            $('#<%=txtDispenseQty.ClientID %>').change();
        }

    }
    //#endregion

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

    function onRefreshGridView() {
        cbpEntryPopupView.PerformCallback('refresh');
    }

    function onRefreshControl() {
        cbpEntryPopupView.PerformCallback('refresh');
    }
</script>
<input type="hidden" value="0" id="hdnPrescriptionTemplateID" runat="server" />
<input type="hidden" value="0" id="hdnParamedicID" runat="server" />
<input type="hidden" value="0" id="hdnEmbalanceID" runat="server" />
<input type="hidden" value="" id="hdnSignaID" runat="server" />
<input type="hidden" value="0" id="hdnIsFlagAdd" runat="server" />
<input type="hidden" value="0" id="hdnGCCompoundUnit" runat="server" />
<input type="hidden" value="0" id="hdnGetCompoundQty" runat="server" />
<input type="hidden" value="" id="hdnStrengthUnit" runat="server" />
<input type="hidden" value="0" id="hdnStrengthAmount" runat="server" />
<input type="hidden" value="" id="hdnDrugID" runat="server" />
<input type="hidden" value="" id="hdnDrugName" runat="server" />
<input type="hidden" value="" id="hdnGCDosingUnit" runat="server" />
<input type="hidden" value="" id="hdnGCItemUnit" runat="server" />
<input type="hidden" value="" id="hdnGCBaseUnit" runat="server" />
<input type="hidden" value="" id="hdnGCDoseUnit" runat="server" />
<input type="hidden" value="" id="hdnGCCoenamRule" runat="server" />

<table style="width: 100%">
    <colgroup>
        <col style="width: 50%" />
    </colgroup>
    <tr>
        <td valign="top">
            <table style="width: 100%" cellpadding="1" cellspacing="1">
                <colgroup>
                    <col width="60px" />
                    <col width="40px" />
                    <col width="20px" />
                    <col width="20px" />
                    <col width="20px" />
                    <col width="20px" />
                    <col width="30px" />
                    <col />
                </colgroup>
                <tr>
                    <td class="tdLabel">
                        <label id="lblTemplateCode" class="lblNormal" runat="server">
                            <%=GetLabel("Template")%></label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtTemplateCode" Width="100%" ReadOnly="true" />
                    </td>
                    <td colspan="5">
                        <asp:TextBox runat="server" ID="txtTemplateName" Width="100%" ReadOnly="true" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<div style="height: 440px; overflow-y: auto">
    <input type="hidden" id="hdnItemID" value="" runat="server" />
    <input type="hidden" id="hdnPrescriptionTemplateDetailID" value="" runat="server" />
    <input type="hidden" value="" id="hdnEntryID" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <div id="containerPopupEntryData" style="margin-top: 10px; display: none;">
                    <div class="pageTitle">
                        <%=GetLabel("Entry")%></div>
                    <fieldset id="fsEntryPopup" style="margin: 0">
                        <table class="tblEntryDetail" style="width: 100%">
                            <colgroup>
                                <col style="width: 15%"/>
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory lblLink" id="lblItem">
                                        <%=GetLabel("Item")%></label>
                                </td>
                                <td>
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 80px" />
                                            <col style="width: 3px" />
                                            <col style="width: 460px"/>
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                               <asp:TextBox ID="txtItemCode" CssClass="required" Width="100%" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtItemName" ReadOnly="true" Width="100%" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label id="lblConversion">
                                        <%=GetLabel("Strength Conversion")%></label>
                                </td>
                                <td>
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 80px" />
                                            <col style="width: 3px" />
                                            <col style="width: 100px"/>
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                               <asp:TextBox ID="txtStrengthAmount" ReadOnly="true" Width="104%" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtStrengthUnit" Width="100%" ReadOnly="true" TabIndex="999" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal lblLink" id="lblSigna">
                                        <%=GetLabel("Signa Template")%></label>
                                </td>
                                 <td>
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 80px" />
                                            <col style="width: 3px" />
                                            <col style="width: 460px"/>
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
                                                <asp:TextBox runat="server" ID="txtSignaName1" Width="100%" ReadOnly="true" TabIndex="999" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Frekuensi dan Dosis")%></label>
                                </td>
                                <td>
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="width: 80px">
                                                <asp:TextBox ID="txtFrequencyNumber" runat="server" Width="100%" CssClass="number" />
                                            </td>
                                            <td style="width: 3px">
                                                &nbsp;
                                            </td>
                                            <td style="width: 20%">
                                                <dxe:ASPxComboBox ID="cboFrequencyTimeline" ClientInstanceName="cboFrequencyTimeline"
                                                    runat="server" Width="100%" />
                                            </td>
                                            <td style="width: 80px">
                                                <asp:TextBox ID="txtDosingDose" runat="server" Width="100%" CssClass="number" />
                                            </td>
                                            <td style="width: 3px">
                                                &nbsp;
                                            </td>
                                            <td style="width: 20%">
                                                <dxe:ASPxComboBox ID="cboDosingUnit" ClientInstanceName="cboDosingUnit" runat="server"
                                                Width="100%" OnCallback="cboDosingUnit_Callback">
                                                <ClientSideEvents EndCallback="onCboDosingUnitEndCallback" SelectedIndexChanged="function(s,e){cboDosingUnitChanged()}" />
                                                </dxe:ASPxComboBox>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Duration (Day)")%></label>
                                </td>
                                <td>
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 9px" />
                                            <col style="width: 100px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtDosingDuration" Width="100%" CssClass="number" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Quantity Total")%></label>
                                </td>
                                <td>
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 80px" />
                                            <col style="width: 3px" />
                                            <col style="width: 100px" />
                                            <col style="width: 20px" />
                                            <col style="width: 20%" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtDispenseQty" Width="100%" CssClass="number" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtDispenseUnit" Width="100%" ReadOnly="true" TabIndex="999" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td style="padding-left: 5px">
                                                <asp:CheckBox runat="server" ID="chkIsUsingSweetener" /><%=GetLabel("Sweetener")%>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkIsAsRequired" runat="server" Text="As Required" Checked="false" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Medication Route")%></label>
                                </td>
                                <td>
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 10%" />
                                            <col style="width: 3%" />
                                            <col style="width: 1%" />
                                            <col style="width: 5%" />
                                            <col style="width: 20%" />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <dxe:ASPxComboBox runat="server" ID="cboMedicationRoute" ClientInstanceName="cboMedicationRoute"
                                                    Width="100%" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td style="padding-left: 5px" align="right">
                                                <label class="lblNormal">
                                                 <%=GetLabel("AC/DC/PC")%></label>
                                            </td>
                                            <td style="padding-left: 5px" align="center">
                                                <dxe:ASPxComboBox runat="server" ID="cboCoenamRule" ClientInstanceName="cboCoenamRule"
                                                    Width="100%" />
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
                                            <td style="width: 15%">
                                                <asp:CheckBox ID="chkIsMorning" runat="server" Text="Morning" Checked="false" />
                                            </td>
                                            <td style="width: 15%">
                                                <asp:CheckBox ID="chkIsNoon" runat="server" Text="Noon" Checked="false" />
                                            </td>
                                            <td style="width: 15%">
                                                <asp:CheckBox ID="chkIsEvening" runat="server" Text="Evening" Checked="false" />
                                            </td>
                                            <td>
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
                                    <asp:TextBox ID="txtMedicationAdministration" Width="540px" runat="server" TextMode="MultiLine" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <input style="width: 70px; text-align: center" type="button" id="btnEntryPopupSave"
                                        value='<%= GetLabel("Simpan")%>' />
                                    <input style="width: 70px; text-align: center" type="button" id="btnEntryPopupCancel"
                                        value='<%= GetLabel("Batal")%>' />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                <dxcp:ASPxCallbackPanel ID="cbpEntryPopupView" runat="server" Width="100%" ClientInstanceName="cbpEntryPopupView"
                    ShowLoadingPanel="false" OnCallback="cbpEntryPopupView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewPopup').show(); }"
                        EndCallback="function(s,e){ onCbpEntryPopupViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlTemplateDt" Style="width: 100%; margin-left: auto;
                                margin-right: auto; position: relative; font-size: 0.95em;">
                                <asp:ListView runat="server" ID="lvwView" OnItemDataBound="lvwView_ItemDataBound">
                                    <EmptyDataTemplate>
                                        <table id="tblView" runat="server" class="grdView notAllowSelect grdTemplateDt" cellspacing="0"
                                            rules="all">
                                            <tr>
                                                <th style="width: 70px">
                                                    &nbsp;
                                                </th>
                                                <th style="width: 100px" align="left">
                                                    <%=GetLabel("ITEM CODE")%>
                                                </th>
                                                <th align="left">
                                                    <%=GetLabel("ITEM NAME")%>
                                                </th>
                                                <th style="width: 60px" align="center">
                                                    <%=GetLabel("FRECUENCY")%>
                                                </th>
                                                <th style="width: 60px" align="left">
                                                    <%=GetLabel("TIMELINE")%>
                                                </th>
                                                <th style="width: 60px" align="right">
                                                    <%=GetLabel("DOSE")%>
                                                </th>
                                                <th style="width: 60px" align="left">
                                                    <%=GetLabel("UNIT")%>
                                                </th>
                                                <th style="width: 60px" align="left">
                                                    <%=GetLabel("ROUTE")%>
                                                </th>
                                                <th align="left">
                                                    <%=GetLabel("SIGNA")%>
                                                </th>
                                                <th style="width: 60px" align="right">
                                                    <%=GetLabel("QUANTITY")%>
                                                </th>
                                            </tr>
                                            <tr class="trEmpty">
                                                <td colspan="10">
                                                    <%=GetLabel("No Data")%>
                                                </td>
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                    <LayoutTemplate>
                                        <table id="tblView" runat="server" class="grdView notAllowSelect grdTemplateDt" cellspacing="0"
                                            rules="all">
                                            <tr>
                                                <th style="width: 70px" rowspan="2">
                                                    &nbsp;
                                                </th>
                                                <th style="width: 100px" rowspan="2" align="left">
                                                    <%=GetLabel("ITEM CODE")%>
                                                </th>
                                                <th rowspan="2" align="left">
                                                    <%=GetLabel("ITEM NAME")%>
                                                </th>
                                                <th style="width: 60px" rowspan="2" align="center">
                                                    <%=GetLabel("FRECUENCY")%>
                                                </th>
                                                <th style="width: 60px" rowspan="2" align="left">
                                                    <%=GetLabel("TIMELINE")%>
                                                </th>
                                                <th style="width: 60px" rowspan="2" align="right">
                                                    <%=GetLabel("DOSE")%>
                                                </th>
                                                <th style="width: 60px" rowspan="2" align="left">
                                                    <%=GetLabel("UNIT")%>
                                                </th>
                                                <th style="width: 60px" rowspan="2" align="left">
                                                    <%=GetLabel("ROUTE")%>
                                                </th>
                                                <th rowspan="2" align="left">
                                                    <%=GetLabel("SIGNA")%>
                                                </th>
                                                <th style="width: 60px" rowspan="2" align="right">
                                                    <%=GetLabel("QUANTITY")%>
                                                </th>
                                            </tr>
                                            <tr runat="server" id="itemPlaceholder">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                    <tr>
                                        <tr>
                                            <td align="center">
                                                <img class="imgEdit imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                    alt="" style="float: left; margin-left: 7px" />
                                                <img class="imgDelete imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                    alt="" style="margin-left: 2px" />
                                                <input type="hidden" class="hdnItemID" value="<%#: Eval("ItemID")%>" />
                                                <input type="hidden" class="hdnItemCode" value="<%#: Eval("ItemCode")%>" />
                                                <input type="hidden" class="hdnItemName" value="<%#: Eval("DrugName")%>" />
                                                <input type="hidden" class="hdnStrengthAmount" value="<%#: Eval("Dose")%>" />
                                                <input type="hidden" class="hdnStrengthUnit" value="<%#: Eval("DoseUnit")%>" />
                                                <input type="hidden" class="hdnGCDosingUnit" value="<%#: Eval("GCDosingUnit")%>" />
                                                <input type="hidden" class="hdnFrequency" value="<%#: Eval("Frequency")%>" />
                                                <input type="hidden" class="hdnDose" value="<%#: Eval("cfNumberOfDosage")%>" />
                                                <input type="hidden" class="hdnPrescriptionTemplateDetailID" value="<%#: Eval("PrescriptionTemplateDetailID")%>" />
                                                <input type="hidden" class="hdnIsMorning" value="<%#: Eval("IsMorning")%>" />
                                                <input type="hidden" class="hdnIsNoon" value="<%#: Eval("IsNoon")%>" />
                                                <input type="hidden" class="hdnIsEvening" value="<%#: Eval("IsEvening")%>" />
                                                <input type="hidden" class="hdnIsNight" value="<%#: Eval("IsNight")%>" />
                                                <input type="hidden" class="hdnIsAsRequired" value="<%#: Eval("IsAsRequired")%>" />
                                                <input type="hidden" class="hdnGCCoenamRule" value="<%#: Eval("GCCoenamRule")%>" />
                                            </td>
                                            <td class="tdItemCode">
                                                <%#: Eval("ItemCode")%>
                                            </td>
                                            <td class="tdItemName">
                                                <%#: Eval("DrugName")%>
                                            </td>
                                            <td class="tdFrequency" align="right">
                                                <%#: Eval("Frequency")%>
                                            </td>
                                            <td class="tdTimeline">
                                                <%#: Eval("DosingFrequency")%>
                                            </td>
                                            <td class="tdDose" align="right">
                                                <%#: Eval("cfNumberOfDosage")%>
                                            </td>
                                            <td class="tdUnit" >
                                                <%#: Eval("DosingUnit")%>
                                            </td>
                                            <td class="tdRoute" >
                                                <%#: Eval("Route")%>
                                            </td>
                                            <td class="tdSigna" >
                                                <%#: Eval("cfConsumeMethod")%>
                                            </td>
                                            <td class="tdQuantity" align="right">
                                                <%#: Eval("DispenseQtyInString")%>
                                            </td>
                                        </tr>
                                      </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                                <div class="imgLoadingGrdView" id="containerImgLoadingViewPopup">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div style="width: 100%; text-align: center" id="divContainerAddData" runat="server">
                    <span class="lblLink" id="lblEntryPopupAddData">
                        <%= GetLabel("Tambah Data")%></span>
                </div>
            </td>
        </tr>
    </table>
    <div style="width: 100%; text-align: right">
        <input type="button" value='<%= GetLabel("Tutup")%>' onclick="onRefreshControl();pcRightPanelContent.Hide();" />
    </div>
</div>
