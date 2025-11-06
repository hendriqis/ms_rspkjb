<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MedicationOrderCompoundEntryCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.MedicationOrderCompoundEntryCtl" %>

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

    function setConversionText() {
        var baseItemUnit = $('#<%=txtDoseUnit.ClientID %>').val();
        var dose = $('#<%=txtDose.ClientID %>').val();
        var compound = $('#<%=txtCompoundQty.ClientID %>').val();
        var compoundUnit = cboCompoundUnit.GetText();

        var conversion = '';
        var conversionFactor = 0;
        if (baseItemUnit == '' || dose == '0') {
            $('#<%=hdnGCDoseUnit.ClientID %>').val(cboCompoundUnit.GetValue());
            baseItemUnit = compoundUnit;
            dose = 1;
            $('#<%=txtDose.ClientID %>').val('1');
            $('#<%=txtDoseUnit.ClientID %>').val(compoundUnit);
        }
        if (compoundUnit == baseItemUnit) {
            conversionFactor = 1;
            conversion = '1 ' + baseItemUnit + " = 1 " + baseItemUnit;
        }
        else {
            conversionFactor = 1 / dose;
            conversion = '1 ' + compoundUnit + " = " + dose + " " + baseItemUnit;
        }

        $('#<%=hdnConversionFactor.ClientID %>').val(conversionFactor);
        $('#<%=txtConversionFactor.ClientID %>').val(conversion);
        
        getTakenQty();
    };

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
        $('#<%=txtCompoundQty.ClientID %>').val(entity.CompoundQty);
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
        openSearchDialog('druginfoperlocation', filterExpression, function (value) {
            $('#<%=txtItemCode.ClientID %>').val(value);
            ontxtItemCodeChanged(value);
        });
    });

    $('#<%=txtItemCode.ClientID %>').change(function () {
        ontxtItemCodeChanged($(this).val());
    });

    function ontxtItemCodeChanged(value) {
        var filterExpression = onGetDrugFilterExpression() + " AND ItemCode = '" + value + "'";
        Methods.getObject('GetvDrugInfoPerLocationList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnItemID.ClientID %>').val(result.ItemID);
                $('#<%=txtItemName1.ClientID %>').val(result.ItemName1);
                $('#<%=txtDose.ClientID %>').val(result.Dose);
                $('#<%=txtDoseUnit.ClientID %>').val(result.DoseUnit);
                $('#<%=hdnGCDoseUnit.ClientID %>').val(result.GCDoseUnit);
                $('#<%=hdnGCItemUnit.ClientID %>').val(result.GCItemUnit);
                cboCompoundUnit.PerformCallback();
            }
            else {
                $('#<%=hdnItemID.ClientID %>').val('');
                $('#<%=txtItemCode.ClientID %>').val('');
                $('#<%=txtItemName1.ClientID %>').val('');
                $('#<%=hdnGCItemUnit.ClientID %>').val('');
                $('#<%=txtDose.ClientID %>').val('');
                $('#<%=txtDoseUnit.ClientID %>').val('');
                $('#<%=hdnGCDoseUnit.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Signa
    $('#lblPopupSigna.lblLink').click(function () {
        var filterExpression = "IsDeleted = 0";
        openSearchDialog('signa', filterExpression, function (value) {
            $('#<%=txtPopupSignaLabel.ClientID %>').val(value);
            txtSignaLabelChanged(value);
        });
    });

    $('#<%=txtPopupSignaLabel.ClientID %>').change(function () {
        txtSignaLabelChanged($(this).val());
    });

    function txtSignaLabelChanged(value) {
        var filterExpression = "IsDeleted = 0 AND SignaLabel = '" + value + "'";
        Methods.getObject('GetvSignaList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnPopupSignaID.ClientID %>').val(result.SignaID);
                $('#<%=txtPopupSignaName1.ClientID %>').val(result.SignaName1);
                $('#<%=txtFrequencyNumber.ClientID %>').val(result.Frequency);
                cboPopupDrugForm.SetValue(result.GCDrugForm);
                cboFrequencyTimeline.SetValue(result.GCDosingFrequency);
                cboPopupCoenamRule.SetValue(result.GCCoenamRule);
                $('#<%=txtDosingDurationTimeline.ClientID %>').val(cboFrequencyTimeline.GetText());
                $('#<%=txtDispenseUnit.ClientID %>').val(result.DrugForm);
                cboPopupCoenamRule.SetValue(result.GCCoenamRule);
            } else {
                $('#<%=hdnPopupSignaID.ClientID %>').val('');
                $('#<%=txtPopupSignaName1.ClientID %>').val('');
                $('#<%=txtFrequencyNumber.ClientID %>').val('');
                cboPopupDrugForm.SetValue('');
                cboFrequencyTimeline.SetValue('');
                cboPopupCoenamRule.SetValue('');
                $('#<%=txtDosingDurationTimeline.ClientID %>').val('');
                $('#<%=txtDispenseUnit.ClientID %>').val('');
                cboPopupCoenamRule.SetValue('');
            }
        });
        getTakenQty();
    }
    //#endregion

    $('#<%=txtFrequencyNumber.ClientID %>').change(function () {
        getTakenQty();
    });

    $('#<%=txtDosingDuration.ClientID %>').change(function () {
        getTakenQty();
    });

    $('#<%=txtCompoundQty.ClientID %>').change(function () {
        getTakenQty();
    });

    function getTakenQty() {
        var duration = $('#<%=txtDosingDuration.ClientID %>').val();
        var frequency = $('#<%=txtFrequencyNumber.ClientID %>').val();
        var dispense = duration * frequency;

        $('#<%=txtDispenseQty.ClientID %>').val(dispense);
        var compoundQty = $('#<%=txtCompoundQty.ClientID %>').val();
        var takenQty = 0;

        var conversionFactor = parseFloat($('#<%=hdnConversionFactor.ClientID %>').val());
        takenQty = conversionFactor * dispense * compoundQty;
        $('#<%=hdnTakenQty.ClientID %>').val(takenQty);
    }
    
</script>
<input type="hidden" value="" id="hdnInlineEditingData" runat="server" />
<input type="hidden" value="" id="hdnQueryString" runat="server" />
<input type="hidden" value="" id="hdnFilterExpression" runat="server" />
<input type="hidden" value="" id="hdnIsAdd" runat="server" />
<input type="hidden" value="" id="hdnGCItemUnit" runat="server" />
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

<div style="height: 450px; overflow-y: auto">
    <table class="tblContentArea">
        <tr>
            <td>
                <table style="width:100%">
                    <colgroup>
                        <col style="width:50%"/>
                    </colgroup>
                    <tr>
                        <td valign="top">
                            <table style="width:100%">
                                <colgroup>
                                    <col style="width:150px"/>
                                    <col style="width:100px"/>
                                    <col /> 
                                </colgroup>
                                <tr>
                                    <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Obat Racikan")%></label></td>
                                    <td colspan="2"><asp:TextBox runat="server" ID="txtCompoundDrugName" Width="100%" /></td>
                                </tr>
                                <tr>
                                    <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Rute Obat")%></label></td>
                                    <td colspan="2"><dxe:ASPxComboBox runat="server" ID="cboMedicationRoute" Width="100%" /></td>
                                </tr>
                                <tr>
                                    <td class="tdLabel"><label class="lblNormal lblLink" id="lblPopupSigna"><%=GetLabel("Aturan Makan")%></label></td>
                                    <td>                                        
                                        <input type="hidden" value="" id="hdnPopupSignaID" runat="server" />
                                        <asp:TextBox runat="server" ID="txtPopupSignaLabel" Width="100%" />
                                    </td>
                                    <td><asp:TextBox runat="server" ID="txtPopupSignaName1" Width="100%" ReadOnly="true" /></td>
                                </tr>
                                <tr>
                                    <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Bentuk")%></label></td>
                                    <td colspan="2"><dxe:ASPxComboBox ID="cboPopupDrugForm" ClientInstanceName="cboPopupDrugForm" runat="server" Width="100px" /></td>
                                </tr>
                                <tr>
                                    <td><label class="lblNormal"><%=GetLabel("Coenam Rule")%></label></td>
                                    <td colspan="2"><dxe:ASPxComboBox runat="server" ID="cboPopupCoenamRule" ClientInstanceName="cboPopupCoenamRule" Width="100%" /></td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top">
                            <table style="width:100%">
                                <colgroup>
                                    <col style="width:170px"/>
                                </colgroup>
                                <tr>
                                    <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Frequency")%></label></td>
                                    <td>
                                        <table cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width:100px"/>
                                                <col style="width:3px"/>
                                                <col style="width:100px"/>
                                            </colgroup>
                                            <tr>
                                                <td><asp:TextBox ID="txtFrequencyNumber" runat="server" Width="100%" CssClass="number" /></td>
                                                <td>&nbsp;</td>
                                                <td><dxe:ASPxComboBox ID="cboFrequencyTimeline" runat="server" Width="100%" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
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
                                    <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Duration")%></label></td>
                                    <td>
                                        <table cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width:100px"/>
                                                <col style="width:3px"/>
                                                <col style="width:100px"/>
                                            </colgroup>
                                            <tr>
                                                <td><asp:TextBox runat="server" ID="txtDosingDuration" Width="100px" CssClass="number" /></td>
                                                <td>&nbsp;</td>
                                                <td><asp:TextBox runat="server" ID="txtDosingDurationTimeline" Width="100px" ReadOnly="true"/></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Dispense Quantity")%></label></td>
                                    <td>
                                        <table cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width:100px"/>
                                                <col style="width:3px"/>
                                                <col style="width:100px"/>
                                            </colgroup>
                                            <tr>
                                                <td><asp:TextBox runat="server" ID="txtDispenseQty" Width="100px" CssClass="number" /></td>
                                                <td>&nbsp;</td>
                                                <td><asp:TextBox runat="server" ID="txtDispenseUnit" Width="100px" ReadOnly="true" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel" style="vertical-align: top; padding-top: 5px;"><label class="lblNormal"><%=GetLabel("Administration Instruction")%></label></td>
                                    <td><asp:TextBox ID="txtMedicationAdministration" Width="300px" runat="server" TextMode="MultiLine" /></td>
                                </tr>
                                <tr>
                                    <td class="tdLabel"></td>
                                    <td><asp:CheckBox runat="server" ID="chkIsUsingSweetener" Text="Tambahkan Pemanis" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <div style="display:none;" id="divEntryProduct">
                    <div class="pageTitle"><%=GetLabel("Entry Product")%></div>
                    <fieldset id="fsTrxPopup" style="margin:0"> 
                        <table class="tblEntryDetail">
                            <colgroup>
                                <col style="width:50%" />
                                <col />
                            </colgroup>
                            <tr>
                                <td valign="top">
                                    <table class="tblEntryDetail" >
                                        <colgroup>
                                            <col width="30%" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td><label class="lblNormal"><%=GetLabel("Generic Name")%></label></td>
                                            <td><asp:TextBox runat="server" ID="txtGenericName" Width="100%" /></td>
                                        </tr>
                                        <tr>
                                            <td><label class="lblMandatory lblLink" id="lblProduct"><%=GetLabel("Product")%></label></td>
                                            <td>
                                                <table cellpadding="0px" cellspacing="0px" width="100%" >
                                                    <tr>
                                                        <td style="width:100px">
                                                            <input type="hidden" id="hdnItemID" runat="server" value="" />
                                                            <asp:TextBox runat="server" ID="txtItemCode" Width="100%" />
                                                        </td>
                                                        <td>&nbsp;</td>
                                                        <td><asp:TextBox runat="server" ID="txtItemName1" ReadOnly="true" Width="100%" /></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2"><div class="lblComponent" ><%=GetLabel("Strength")%></div></td>
                                        </tr>
                                        <tr>
                                            <td><label class="lblNormal"><%=GetLabel("Amount")%></label></td>
                                            <td>
                                                <table cellpadding="0px" cellspacing="0px" width="100%" >
                                                    <tr>
                                                        <td><asp:TextBox runat="server" ID="txtDose" CssClass="number" Width="100%" ReadOnly="true" /></td>
                                                        <td>&nbsp;</td>
                                                        <td><asp:TextBox runat="server" ID="txtDoseUnit" Width="100%" ReadOnly="true" /></td>
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
                                    <table class="tblEntryDetail" >
                                        <colgroup>
                                            <col width="30%" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td colspan="2"><div class="lblComponent" ><%=GetLabel("Compound")%></div></td>
                                        </tr>
                                        <tr>
                                            <td><label class="lblMandatory"><%=GetLabel("Quantity")%></label></td>
                                            <td>
                                                <table cellpadding="0px" cellspacing="0px" width="100%">
                                                    <tr>
                                                        <td><asp:TextBox runat="server" ID="txtCompoundQty" CssClass="number" Width="100%" /></td>
                                                        <td>&nbsp;</td>
                                                        <td>
                                                            <dxe:ASPxComboBox runat="server" ID="cboCompoundUnit" ClientInstanceName="cboCompoundUnit"
                                                                OnCallback="cboCompoundUnit_Callback"  Width="100%" >
                                                                <ClientSideEvents ValueChanged="function(s,e){ onCboCompoundUnitValueChanged(); }" 
                                                                    EndCallback="function(s,e){ onCboCompoundUnitEndCallback(); }" />
                                                            </dxe:ASPxComboBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td><label class="lblNormal"><%=GetLabel("Conversion Factor") %></label></td>
                                            <td>
                                                <input type="hidden" id="hdnConversionFactor" runat="server" />
                                                <asp:TextBox runat="server" ID="txtConversionFactor" ReadOnly="true" Width="100%"  />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                <dxcp:ASPxCallbackPanel ID="cbpPopupView" runat="server" Width="100%" ClientInstanceName="cbpPopupView"
                    ShowLoadingPanel="false" OnCallback="cbpPopupView_Callback" >
                    <ClientSideEvents BeginCallback="function() { showLoadingPanel(); }"
                        EndCallback="function(s,e){ onCbpPopupViewEndCallback(s) }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="max-height: 300px">
                                <input type="hidden" value="" id="hdnParentID" runat="server" />
                                <asp:ListView ID="lvwView" runat="server">
                                    <EmptyDataTemplate>
                                        <table class="grdNormal" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width:90px"/>
                                                <col style="width:100px"/>
                                                <col/>
                                                <col style="width:70px"/>
                                                <col style="width:100px"/>
                                                <col style="width:70px"/>
                                                <col style="width:100px"/>
                                                <col style="width:170px"/>
                                            </colgroup>
                                            <tr>
                                                <th align="center" rowspan="2">#</th>
                                                <th align="center" rowspan="2"><%=GetLabel("Generic Name")%></th>
                                                <th align="center" rowspan="2"><%=GetLabel("Product")%></th>
                                                <th align="center" colspan="2"><%=GetLabel("Strength")%></th>
                                                <th align="center" colspan="3"><%=GetLabel("Compound")%></th>
                                            </tr>
                                            <tr>
                                                <th align="center"><%=GetLabel("Amount")%></th>
                                                <th align="center"><%=GetLabel("Unit")%></th>
                                                <th align="center"><%=GetLabel("Quantity")%></th>
                                                <th align="center"><%=GetLabel("Unit")%></th>
                                                <th align="center"><%=GetLabel("Conversion")%></th>
                                            </tr>
                                            <tr>
                                                <td colspan="8" align="center" style="height:45px; vertical-align:middle">
                                                    <%=GetLabel("No Data To Display")%>
                                                </td>
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                    <LayoutTemplate>
                                        <table class="grdNormal" runat="server" id="tblView" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width:90px"/>
                                                <col style="width:100px"/>
                                                <col/>
                                                <col style="width:70px"/>
                                                <col style="width:100px"/>
                                                <col style="width:70px"/>
                                                <col style="width:100px"/>
                                                <col style="width:100px"/>
                                            </colgroup>
                                            <tr>
                                                <th align="center" rowspan="2">#</th>
                                                <th align="center" rowspan="2"><%=GetLabel("Generic Name")%></th>
                                                <th align="center" rowspan="2"><%=GetLabel("Product")%></th>
                                                <th align="center" colspan="2"><%=GetLabel("Strength")%></th>
                                                <th align="center" colspan="3"><%=GetLabel("Compound")%></th>
                                            </tr>
                                            <tr>
                                                <th align="center"><%=GetLabel("Amount")%></th>
                                                <th align="center"><%=GetLabel("Unit")%></th>
                                                <th align="center"><%=GetLabel("Quantity")%></th>
                                                <th align="center"><%=GetLabel("Unit")%></th>
                                                <th align="center"><%=GetLabel("Conversion")%></th>
                                            </tr>
                                            <tr runat="server" id="itemPlaceholder" ></tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td align="center">
                                                <img class="imgPopupEdit imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>' alt="" />
                                                <img class="imgPopupDelete <%# IsEditable() == "0" ? "imgDisabled" : "imgLink"%>" src='<%# IsEditable() == "0" ? ResolveUrl("~/Libs/Images/Button/delete_disabled.png") : ResolveUrl("~/Libs/Images/Button/delete.png")%>' alt="" />
                                                <input type="hidden" value="<%#:Eval("PrescriptionOrderDetailID") %>" bindingfield="PrescriptionOrderDetailID" />
                                                <input type="hidden" value="<%#:Eval("GenericName") %>" bindingfield="GenericName" />
                                                <input type="hidden" value="<%#:Eval("ItemID") %>" bindingfield="ItemID" />
                                                <input type="hidden" value="<%#:Eval("ItemCode") %>" bindingfield="ItemCode" />
                                                <input type="hidden" value="<%#:Eval("DrugName") %>" bindingfield="DrugName" />
                                                <input type="hidden" value="<%#:Eval("GCItemUnit") %>" bindingfield="GCItemUnit" />
                                                <input type="hidden" value="<%#:Eval("ItemUnit") %>" bindingfield="ItemUnit" />
                                                <input type="hidden" value="<%#:Eval("Dose") %>" bindingfield="Dose" />
                                                <input type="hidden" value="<%#:Eval("GCDoseUnit") %>" bindingfield="GCDoseUnit" />
                                                <input type="hidden" value="<%#:Eval("DoseUnit") %>" bindingfield="DoseUnit" />
                                                
                                                <input type="hidden" value="<%#:Eval("CompoundQty") %>" bindingfield="CompoundQty" />
                                                <input type="hidden" value="<%#:Eval("TakenQty") %>" bindingfield="TakenQty" />
                                                <input type="hidden" value="<%#:Eval("GCCompoundUnit") %>" bindingfield="GCCompoundUnit" />
                                                <input type="hidden" value="<%#:Eval("ParentID") %>" bindingfield="ParentID" />
                                                <input type="hidden" value="<%#:Eval("ConversionFactor") %>" bindingfield="ConversionFactor">
                                            </td>
                                            <td><%#: Eval("GenericName") %></td>
                                            <td><%#: Eval("DrugName") %></td>
                                            <td align="right"><%#: Eval("Dose") %></td>
                                            <td align="center"><%#: Eval("DoseUnit") %></td>
                                            <td align="right"><%#: Eval("CompoundQty")%></td>
                                            <td align="center"><%#: Eval("CompoundUnit")%></td>
                                            <td align="center"><%#: Eval("CustomConversion")%></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                                <div style="width:100%;text-align:center" id="divContainerAddData" runat="server">
                                    <span class="lblLink" id="lblPopupAddData"><%= GetLabel("Add Data")%></span>
                                </div>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div class="imgLoadingGrdView" id="containerImgLoadingView" >
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
            </td>
        </tr>
    </table>

    <dxcp:ASPxCallbackPanel ID="cbpPopupProcess" runat="server" Width="100%" ClientInstanceName="cbpPopupProcess"
        ShowLoadingPanel="false" OnCallback="cbpPopupProcess_Callback">
        <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }"
            EndCallback="function(s,e) { onCbpPopupProcesEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</div>
   