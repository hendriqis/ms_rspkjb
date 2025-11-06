<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CompoundTemplateFormulaCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.CompoundTemplateFormulaCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_compoundtemplateformulactl">
    $('#lblEntryPopupAddData').live('click', function () {
        $('#<%=hdnGCCompoundUnit.ClientID %>').val('');
        $('#<%=hdnIsFlagAdd.ClientID %>').val('1');
        $('#lblItem').attr('class', 'lblLink lblMandatory');
        $('#<%=hdnItemID.ClientID %>').val('');
        $('#<%=txtItemCode.ClientID %>').removeAttr('readonly');
        $('#<%=txtItemCode.ClientID %>').val('');
        $('#<%=txtItemName.ClientID %>').val('');
        $('#<%=txtConversionStrength.ClientID %>').val('');
        $('#<%=txtCompoundQuantity.ClientID %>').val('');
        cboCompoundUnit.SetValue('');
        $('#<%=txtConversionFactor.ClientID %>').val('');
        $('#<%=txtConversionFactorUnit.ClientID %>').val('');
        $('#<%=txtResultQty.ClientID %>').val('');
        $('#<%=txtResultUnit.ClientID %>').val('');
        $('#<%=txtDisplayOrder.ClientID %>').val('');

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
        if (confirm("Are You Sure Want To Delete This Data?")) {
            $row = $(this).closest('tr');
            var itemID = $row.find('.hdnItemID').val();
            $('#<%=hdnItemID.ClientID %>').val(itemID);

            cbpEntryPopupView.PerformCallback('delete');
        }
    });

    $('.imgEdit.imgLink').die('click');
    $('.imgEdit.imgLink').live('click', function () {
        $('#containerPopupEntryData').show();
        $row = $(this).closest('tr');
        var itemID = $row.find('.hdnItemID').val();
        var itemCode = $row.find('.hdnItemCode').val();
        var strengthAmount = $row.find('.hdnStrengthAmount').val();
        var strengthUnit = $row.find('.hdnStrengthUnit').val();
        var compoundQtyInString = $row.find('.hdnCompoundQuantityString').val();
        var gccompoundUnit = $row.find('.hdnGCCompoundUnit').val();
        var displayOrder = $row.find('.hdnDisplayOrder').val();

        $('#<%=hdnIsFlagAdd.ClientID %>').val('0');
        $('#<%=hdnGCCompoundUnit.ClientID %>').val(gccompoundUnit);
        $('#lblItem').attr('class', 'lblDisabled');
        $('#<%=hdnItemID.ClientID %>').val(itemID);
        $('#<%=txtItemCode.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtItemCode.ClientID %>').val(itemCode);
        $('#<%=txtCompoundQuantity.ClientID %>').val(compoundQtyInString);
        $('#<%=txtDisplayOrder.ClientID %>').val(displayOrder);
        cboCompoundUnit.SetValue(gccompoundUnit);

        $('#<%=txtItemCode.ClientID %>').trigger('change');
    });

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

                var itemUnitInfo = result.ItemUnit;
                var doseUnitInfo = result.DoseUnit;
                var doseInfo = result.Dose;

                var conversion = '';

                if (itemUnitInfo == doseUnitInfo)
                    conversion = '1 ' + itemUnitInfo + ' = 1 ' + doseUnitInfo;
                else
                    conversion = '1 ' + itemUnitInfo + ' = ' + doseInfo + ' ' + doseUnitInfo;

                $('#<%=txtConversionStrength.ClientID %>').val(conversion);

                cboCompoundUnit.PerformCallback();
            }
            else {
                $('#<%=hdnItemID.ClientID %>').val('');
                $('#<%=txtItemCode.ClientID %>').val('');
                $('#<%=txtItemName.ClientID %>').val('');
                $('#<%=txtConversionStrength.ClientID %>').val('');
                $('#<%=txtCompoundQuantity.ClientID %>').val('');
                cboCompoundUnit.SetValue('');
                $('#<%=txtConversionFactor.ClientID %>').val('');
                $('#<%=txtConversionFactorUnit.ClientID %>').val('');
                $('#<%=txtResultQty.ClientID %>').val('');
                $('#<%=txtResultUnit.ClientID %>').val('');
                $('#<%=txtDisplayOrder.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region isi Conversion Factor & Result
    $('#<%=txtCompoundQuantity.ClientID %>').die('change');
    $('#<%=txtCompoundQuantity.ClientID %>').live('change', function () {
        onTxtCompoundQuantityChanged($(this).val());
    });

    function onTxtCompoundQuantityChanged(value) {
        cboCompoundUnitChanged();
    }

    function getCompoundQtyFromString() {
        var compoundQty = $('#<%=txtCompoundQuantity.ClientID %>').val();
        var qty = "";

        if (compoundQty != "") {
            if (compoundQty.includes("/")) {
                var compoundInfo = new Array();
                compoundInfo = compoundQty.split("/");
                for (a in compoundInfo) {
                    qty = (compoundInfo[0] / compoundInfo[1]).toFixed(2);
                }
            } else {
                qty = compoundQty;
            }
        } else {
            qty = 0;
        }
        $('#<%=hdnGetCompoundQty.ClientID %>').val(qty);
        return qty;
    }

    function cboCompoundUnitChanged() {
        var compoundUnit = cboCompoundUnit.GetValue();
        var compoundQty = $('#<%=txtCompoundQuantity.ClientID %>').val();
        var qty = getCompoundQtyFromString();

        var itemCode = $('#<%=txtItemCode.ClientID %>').val();
        var filterExpression = "ItemCode = '" + itemCode + "'";
        Methods.getObject('GetvDrugInfoList', filterExpression, function (result) {
            if (result != null) {
                var itemUnitInfoGC = result.GCItemUnit;
                var itemUnitInfo = result.ItemUnit;
                var doseUnitInfo = result.DoseUnit;
                var doseInfo = result.Dose;

                var duration = $('#<%=txtDosingDuration.ClientID %>').val();
                var frequency = $('#<%=txtFrequencyNumber.ClientID %>').val();
                var doseTemplate = $('#<%=txtDosingDose.ClientID %>').val();

                var qtyTotal = $('#<%=txtDispenseQty.ClientID %>').val();

                if (compoundUnit == itemUnitInfoGC) {
                    $('#<%=txtConversionFactor.ClientID %>').val('1');
                    $('#<%=txtConversionFactorUnit.ClientID %>').val(itemUnitInfo);

                    var resultQty = (duration * frequency * doseTemplate) * qty;
                    $('#<%=txtResultQty.ClientID %>').val(resultQty);
                    $('#<%=txtResultUnit.ClientID %>').val(itemUnitInfo);

                } else {
                    $('#<%=txtConversionFactor.ClientID %>').val(doseInfo);
                    $('#<%=txtConversionFactorUnit.ClientID %>').val(doseUnitInfo);

                    var conf = qty / doseInfo * 1;
                    //                    var resultQty = (duration * frequency * doseTemplate) * conf;
                    var resultQty = (qtyTotal) * conf;
                    $('#<%=txtResultQty.ClientID %>').val(resultQty.toFixed(2));
                    $('#<%=txtResultUnit.ClientID %>').val(itemUnitInfo);
                }
            }
        });
    }
    //#endregion

    function onCboCompoundUnitEndCallback() {
        if($('#<%=hdnGCCompoundUnit.ClientID %>').val() != "")
        {
            cboCompoundUnit.SetValue($('#<%=hdnGCCompoundUnit.ClientID %>').val());
        }
        $('#<%=txtCompoundQuantity.ClientID %>').trigger('change');
    }
</script>
<input type="hidden" value="0" id="hdnCompoundTemplateID" runat="server" />
<input type="hidden" value="0" id="hdnParamedicID" runat="server" />
<input type="hidden" value="0" id="hdnEmbalanceID" runat="server" />
<input type="hidden" value="0" id="hdnIsFlagAdd" runat="server" />
<input type="hidden" value="0" id="hdnGCCompoundUnit" runat="server" />
<input type="hidden" value="0" id="hdnGetCompoundQty" runat="server" />
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
                <tr>
                    <td class="tdLabel">
                        <label class="lblNormal">
                            <%=GetLabel("Signa")%></label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtFrequencyNumber" runat="server" Width="100%" CssClass="number"
                            ReadOnly="true" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtFrequencyTimeline" runat="server" Width="100%" ReadOnly="true" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtDosingDose" runat="server" Width="100%" CssClass="number" ReadOnly="true" />
                    </td>
                    <td colspan="2">
                        <asp:TextBox ID="txtDosingUnit" runat="server" Width="100%" ReadOnly="true" />
                    </td>
                </tr>
                <tr>
                    <td class="tdLabel">
                        <label class="lblNormal">
                            <%=GetLabel("Duration (day)")%></label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtDosingDuration" Width="100%" CssClass="number"
                            ReadOnly="true" />
                    </td>
                    <td class="tdLabel" style="padding-right: 10px" align="right">
                        <label class="lblNormal">
                            <%=GetLabel("Quantity Total")%></label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtDispenseQty" Width="100%" CssClass="number" ReadOnly="true" />
                    </td>
                    <td colspan="2">
                        <asp:CheckBox runat="server" ID="chkIsUsingSweetener" Enabled="false" /><%=GetLabel("Sweetener")%>
                    </td>
                </tr>
                <tr>
                    <td class="tdLabel">
                        <label class="lblNormal">
                            <%=GetLabel("Embalace")%></label>
                    </td>
                    <td colspan="2">
                        <asp:TextBox runat="server" ID="txtEmbalace" Width="100%" ReadOnly="true" />
                    </td>
                    <td class="tdLabel" style="padding-left: 10px" colspan="2">
                        <label class="lblNormal">
                            <%=GetLabel("Quantity Embalance")%></label>
                    </td>
                    <td colspan="2">
                        <asp:TextBox runat="server" ID="txtEmbalaceQty" Width="100%" CssClass="number" ReadOnly="true" />
                    </td>
                </tr>
                <tr>
                    <td class="tdLabel">
                        <label class="lblNormal">
                            <%=GetLabel("Medication Route")%></label>
                    </td>
                    <td colspan="2">
                        <asp:TextBox runat="server" ID="txtMedicationRoute" Width="100%" ReadOnly="true" />
                    </td>
                    <td class="tdLabel" style="padding-left: 10px" colspan="2">
                        <label class="lblNormal">
                            <%=GetLabel("AC/DC/PC")%></label>
                    </td>
                    <td colspan="2">
                        <asp:TextBox runat="server" ID="txtCoenamRule" Width="100%" ReadOnly="true" />
                    </td>
                </tr>
            </table>
        </td>
        <td valign="top">
            <table style="width: 100%">
                <colgroup>
                    <col style="width: 30%" />
                </colgroup>
                <tr>
                    <td class="tdLabel" style="vertical-align: top; padding-top: 2px; padding-left: 10px">
                        <label class="lblNormal">
                            <%=GetLabel("Administration Instruction")%></label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtMedicationAdministration" Width="350px" runat="server" TextMode="MultiLine"
                            Height="50px" ReadOnly="true" />
                    </td>
                </tr>
                <tr>
                    <td class="tdLabel" style="vertical-align: top; padding-top: 2px; padding-left: 10px">
                        <label class="lblNormal">
                            <%=GetLabel("Medication Purpose")%></label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtMedicationPurpose" Width="350px" runat="server" TextMode="MultiLine"
                            Height="50px" ReadOnly="true" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<div style="height: 440px; overflow-y: auto">
    <input type="hidden" id="hdnItemID" value="" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <div id="containerPopupEntryData" style="margin-top: 10px; display: none;">
                    <div class="pageTitle">
                        <%=GetLabel("Entry")%></div>
                    <fieldset id="fsEntryPopup" style="margin: 0">
                        <table class="tblEntryDetail" style="width: 100%">
                            <colgroup>
                                <col style="width: 15%" />
                                <col style="width: 10px" />
                                <col style="width: 10px" />
                                <col style="width: 10px" />
                                <col style="width: 35%" />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory lblLink" id="lblItem">
                                        <%=GetLabel("Item")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtItemCode" CssClass="required" Width="100%" runat="server" />
                                </td>
                                <td style="padding-left: 5px" colspan="2">
                                    <asp:TextBox ID="txtItemName" ReadOnly="true" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label id="lblConversion">
                                        <%=GetLabel("Strength Conversion")%></label>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtConversionStrength" ReadOnly="true" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label id="lblCompoundFormula">
                                        <%=GetLabel("Compound Qty")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCompoundQuantity" Width="100%" runat="server" CssClass="number" />
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboCompoundUnit" ClientInstanceName="cboCompoundUnit" runat="server"
                                        Width="100%" OnCallback="cboCompoundUnit_Callback">
                                        <ClientSideEvents EndCallback="onCboCompoundUnitEndCallback" SelectedIndexChanged="function(s,e){cboCompoundUnitChanged()}" />
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label id="lblConversionFactor">
                                        <%=GetLabel("Conversion Factor")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtConversionFactor" ReadOnly="true" Width="100%" runat="server"
                                        CssClass="number" />
                                </td>
                                <td style="padding-left: 5px">
                                    <asp:TextBox ID="txtConversionFactorUnit" ReadOnly="true" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label id="Label1">
                                        <%=GetLabel("Result")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtResultQty" ReadOnly="true" Width="100%" runat="server" CssClass="number" />
                                </td>
                                <td style="padding-left: 5px">
                                    <asp:TextBox ID="txtResultUnit" ReadOnly="true" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label id="lblDisplayOrder" class="lblMandatory">
                                        <%=GetLabel("Display Order")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDisplayOrder" Width="100%" runat="server" CssClass="number" />
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
                                                <th style="width: 70px" rowspan="2">
                                                    &nbsp;
                                                </th>
                                                <th style="width: 100px" rowspan="2" align="left">
                                                    <%=GetLabel("ITEM CODE")%>
                                                </th>
                                                <th rowspan="2" align="left">
                                                    <%=GetLabel("ITEM NAME")%>
                                                </th>
                                                <th style="width: 150px" rowspan="2" align="center">
                                                    <%=GetLabel("CONVERSION")%>
                                                </th>
                                                <th colspan="2" align="center">
                                                    <%=GetLabel("STRENGTH")%>
                                                </th>
                                                <th colspan="2" align="center">
                                                    <%=GetLabel("COMPOUND FORMULA")%>
                                                </th>
                                            </tr>
                                            <tr>
                                                <th style="width: 90px" align="center">
                                                    <%=GetLabel("AMOUNT")%>
                                                </th>
                                                <th style="width: 90px" align="center">
                                                    <%=GetLabel("UNIT")%>
                                                </th>
                                                <th style="width: 90px" align="center">
                                                    <%=GetLabel("QUANTITY")%>
                                                </th>
                                                <th style="width: 90px" align="center">
                                                    <%=GetLabel("UNIT")%>
                                                </th>
                                            </tr>
                                            <tr class="trEmpty">
                                                <td colspan="8">
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
                                                <th colspan="2" align="center">
                                                    <%=GetLabel("STRENGTH")%>
                                                </th>
                                                <th colspan="2" style="width: 150px" align="center">
                                                    <%=GetLabel("CONVERSION FACTOR")%>
                                                </th>
                                                <th colspan="2" align="center">
                                                    <%=GetLabel("COMPOUND FORMULA")%>
                                                </th>
                                            </tr>
                                            <tr>
                                                <th style="width: 90px" align="center">
                                                    <%=GetLabel("AMOUNT")%>
                                                </th>
                                                <th style="width: 90px" align="center">
                                                    <%=GetLabel("UNIT")%>
                                                </th>
                                                <th style="width: 90px" align="center">
                                                    <%=GetLabel("QUANTITY")%>
                                                </th>
                                                <th style="width: 90px" align="center">
                                                    <%=GetLabel("UNIT")%>
                                                </th>
                                                <th style="width: 90px" align="center">
                                                    <%=GetLabel("QUANTITY")%>
                                                </th>
                                                <th style="width: 90px" align="center">
                                                    <%=GetLabel("UNIT")%>
                                                </th>
                                            </tr>
                                            <tr runat="server" id="itemPlaceholder">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
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
                                                <input type="hidden" class="hdnStrengthUnit" value="<%#: Eval("ItemDoseUnit")%>" />
                                                <input type="hidden" class="hdnCompoundQuantity" value="<%#: Eval("CompoundQty")%>" />
                                                <input type="hidden" class="hdnCompoundQuantityString" value="<%#: Eval("CompoundQtyInString")%>" />
                                                <input type="hidden" class="hdnGCCompoundUnit" value="<%#: Eval("GCCompoundUnit")%>" />
                                                <input type="hidden" class="hdnCompoundUnit" value="<%#: Eval("CompoundUnit")%>" />
                                                <input type="hidden" class="hdnDisplayOrder" value="<%#: Eval("DisplayOrder")%>" />
                                            </td>
                                            <td class="tdItemCode">
                                                <%#: Eval("ItemCode")%>
                                            </td>
                                            <td class="tdItemName">
                                                <%#: Eval("DrugName")%>
                                            </td>
                                            <td class="tdDose" align="right">
                                                <%#: Eval("Dose")%>
                                            </td>
                                            <td class="tdItemDoseUnit" align="center">
                                                <%#: Eval("ItemDoseUnit")%>
                                            </td>
                                            <td class="tdConversionFactor" align="right">
                                                <%#: Eval("ConversionFactor","{0:n}")%>
                                            </td>
                                            <td class="tdConversionFactorUnit" align="center">
                                                <%#: Eval("CompoundUnit")%>
                                            </td>
                                            <td class="tdCompoundQtyInString" align="right">
                                                <%#: Eval("CompoundQtyInString")%>
                                            </td>
                                            <td class="tdCompoundUnit" align="center">
                                                <%#: Eval("CompoundUnit")%>
                                            </td>
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
