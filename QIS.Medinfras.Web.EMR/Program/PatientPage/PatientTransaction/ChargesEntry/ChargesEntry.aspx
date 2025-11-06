<%@ Page Language="C#" MasterPageFile="~/MasterPage/MPPatientPageListEntry.master"
    AutoEventWireup="true" CodeBehind="ChargesEntry.aspx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.ChargesEntry" %>

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
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbitems.png")%>' alt="" /><div>
            <%=GetLabel("Quick Picks")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhScript" runat="server">
    <script type="text/javascript">
        $(function () {
        });

        function onRefreshControl(filterExpression) {
            if ($('#<%=hdnTransactionID.ClientID %>').val() == '') {
                $('#<%=hdnTransactionID.ClientID %>').val($('#hdnEntryOrderID').val());
                cboParamedicID.SetEnabled(false);
                cboRefillInstruction.SetEnabled(false);
                cboServiceUnit.SetEnabled(false);
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
                $('#<%=txtTransactionNo.ClientID %>').val(param[2]);
            }
        }
        //#endregion

        function onledItemNameLostFocus(led) {
            var drugID = led.GetValueText();
            if (drugID != '') {
                $('#<%=hdnItemID.ClientID %>').val(drugID);
                $('#<%=hdnItemName.ClientID %>').val(led.GetDisplayText());

                var filterExpression = "ItemID = " + drugID;
                Methods.getObject('GetvDrugInfoList', filterExpression, function (result) {
                    cboDrugForm.SetValue(result.GCDrugForm);
                    $('#<%=txtDosingDose.ClientID %>').val('1');
                    cboMedicationRoute.SetValue(result.GCMedicationRoute);
                    cboDosingUnit.SetValue(result.GCItemUnit);
                    $('#<%=hdnGCBaseUnit.ClientID %>').val(result.GCBaseUnit);
                    $('#<%=txtDispenseUnit.ClientID %>').val(result.ItemUnit);
                    $('#<%=txtFrequencyNumber.ClientID %>').focus();
                });
            }
        }

        function onButtonCancelClick() {
            alert('Click');
            $('#ctnEntry').hide();
        }

        //#region Entity To Control
        function entityToControl(entity) {
            }
        }
        //#endregion

        $(function () {
            $('#<%=btnBackPrescriptionList.ClientID %>').click(function () {
                showLoadingPanel();
                document.location = document.referrer;
            });

            $('#<%=btnPrescriptionCompoundEntry.ClientID %>').click(function () {
                var prescriptionID = $('#<%=hdnTransactionID.ClientID %>').val();
                var date = Methods.dateToString(Methods.getDatePickerDate($('#<%=txtStartDate.ClientID %>').val()));
                var time = $('#<%=txtStartTime.ClientID %>').val();
                var physician = cboParamedicID.GetValue();
                var prescriptionType = cboPrescriptionType.GetValue();
                var dispensaryServiceUnitID = cboServiceUnit.GetValue();
                var refillInstruction = cboRefillInstruction.GetValue();
                var location = cboLocation.GetValue();

                var queryString = prescriptionID + '|' + date + '|' + time + '|' + physician + '|' + prescriptionType + '|' + dispensaryServiceUnitID + '|' + location + '|' + refillInstruction;

                var url = ResolveUrl("~/Program/PatientPage/Planning/Prescription/PrescriptionCompoundEntryCtl.ascx");
                openUserControlPopup(url, queryString, 'Compound Prescription', 900, 650);
            });

            setDatePicker('<%=txtStartDate.ClientID %>');
        });

        function onBeforeEditRecordIsUsingCustomEdit(entity) {
            return (entity.IsCompound == 'True')
        }

        function onCustomEditRecord(entity) {
            var prescriptionID = $('#<%=hdnTransactionID.ClientID %>').val();
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
            if ($('#<%=hdnTransactionID.ClientID %>').val() == '') {
                $('#<%=hdnTransactionID.ClientID %>').val(param);
                var filterExpression = $('#<%=hdnFilterExpressionItem.ClientID %>').val();
                txtQuickEntry.SetFilterExpression(0, filterExpression);
                ledItemName.SetFilterExpression(filterExpression);
                var filterExpressionPrescription = 'PrescriptionOrderID = ' + param;
                Methods.getObject('GetPrescriptionOrderHdList', filterExpressionPrescription, function (result) {
                    $('#<%=txtTransactionNo.ClientID %>').val(result.PrescriptionOrderNo);
                });
                cboParamedicID.SetEnabled(false);
                cboRefillInstruction.SetEnabled(false);
                cboServiceUnit.SetEnabled(false);
                cboPrescriptionType.SetEnabled(false);
                cboLocation.SetEnabled(false);
                hideLoadingPanel();
            }
            cbpView.PerformCallback('refresh');
        }

        function onBeforeSaveRecord() {
            return true;
        }

        function onTxtQuickEntrySearchClick(s) {
            onPatientPageListEntryQuickEntrySave(s.GetValue());
        }

        function onAfterSaveQuickEntryRecord(val) {
            txtQuickEntry.SetText('');
            if ($('#<%=hdnTransactionID.ClientID %>').val() == '') {
                $('#<%=hdnTransactionID.ClientID %>').val(val);
                var filterExpression = $('#<%=hdnFilterExpressionItem.ClientID %>').val().replace("{PrescriptionID}", val);
                txtQuickEntry.SetFilterExpression(0, filterExpression);
                ledItemName.SetFilterExpression(filterExpression);
                cboParamedicID.SetEnabled(false);
                cboRefillInstruction.SetEnabled(false);
                cboServiceUnit.SetEnabled(false);
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
                dispenseQty = Math.ceil((dosingDuration * frequency * dose) / strengthAmount);
            }
            $('#<%=txtDispenseQty.ClientID %>').val(dispenseQty);
            $('#<%=txtDispenseQty.ClientID %>').change();
        }
        //#endregion
    </script>
    <input type="hidden" id="hdnTransactionID" runat="server" value="" />
    <input type="hidden" id="hdnFilterExpressionItem" runat="server" value="" />
    <input type="hidden" value="" id="hdnDefaultDispensaryServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnLocationID" runat="server" />
    <input type="hidden" value="" id="hdnGCBaseUnit" runat="server" />
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
                            <%=GetLabel("Transaction No.") %>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtTransactionNo" Width="99%" ReadOnly="true" runat="server" />
                        </td>
                        <td class="tdLabel" style="padding-left: 10px">
                            <label class="lblMandatory">
                                <%=GetLabel("Service Unit") %></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboServiceUnit" ClientInstanceName="cboServiceUnit" runat="server"
                                Width="100%">
                                <ClientSideEvents ValueChanged="function() { cboLocation.PerformCallback(); }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <%=GetLabel("Date") %>
                            -
                            <%=GetLabel("Time") %>
                        </td>
                        <td style="padding-right: 1px; width: 120px">
                            <asp:TextBox ID="txtTransactionDate" ReadOnly="true" Width="100%" CssClass="datepicker"
                                runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtTransactionTime" Width="98%" CssClass="time" ReadOnly="true"
                                runat="server" Style="text-align: center" />
                        </td>
                        <td class="tdLabel" style="padding-left: 10px">
                            <label class="lblNormal">
                                <%=GetLabel("Item Location")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox runat="server" ID="cboLocation" ClientInstanceName="cboLocation"
                                Width="100%" OnCallback="cboLocation_Callback">
                                <ClientSideEvents BeginCallback="function() { showLoadingPanel(); }" EndCallback="function() { hideLoadingPanel(); }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <%=GetLabel("Physician") %>
                        </td>
                        <td colspan="2">
                            <dxe:ASPxComboBox ID="cboParamedicID" ClientInstanceName="cboParamedicID" Width="100%"
                                runat="server" />
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
                                <%=GetLabel("Remarks") %></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRemarks" Width="100%" runat="server" TextMode="MultiLine" Height="70px" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="ctnQuickEntry" ContentPlaceHolderID="plhQuickEntry" runat="server">
    <table style="display:none">
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
                            </Columns>
                        </qis:QISQuickEntryHint>
                        <qis:QISQuickEntryHint Text="Frequency" ValueField="StandardCodeID" TextField="StandardCodeName"
                            Description="i.e. QD / BID / TID / QID / QH# / #dd / prn" MethodName="GetStandardCodeList" FilterExpression="ParentID = 'X233'">
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
    <input type="hidden" id="hdnLabHealthcareServiceUnitID" runat="server" value="" />
    <input type="hidden" id="hdnServiceDtTotal" runat="server" value="" />
    <input type="hidden" id="hdnRegistrationID" runat="server" value="" />
    <input type="hidden" id="hdnVisitID" runat="server" value="" />
    <input type="hidden" id="hdnTariffComp1Text" runat="server" value="" />
    <input type="hidden" id="hdnTariffComp2Text" runat="server" value="" />
    <input type="hidden" id="hdnTariffComp3Text" runat="server" value="" />
    <input type="hidden" id="hdnPrescriptionReturnItem" runat="server" value="" />
    <input type="hidden" id="hdnIsEditable" runat="server" value="" />

    <input type="hidden" id="hdnTempTotalPatient" runat="server" value="" />
    <input type="hidden" id="hdnTempTotalPayer" runat="server" value="" />
    <div id="containerEntryService" style="margin-top:4px;display:none;">
    <fieldset id="fsTrxService" style="margin:0"> 
        <table class="tblEntryDetail" style="width:100%">
            <colgroup>
                <col style="width:40%"/>
                <col style="width:33%"/>
                <col />
            </colgroup>
            <tr>
                <td valign="top">
                    <table>
                        <colgroup>
                            <col style="width:130px"/>
                        </colgroup>
                        <tr>
                            <td class="tdLabel"><label class="lblLink lblMandatory" id="lblServiceItem"><%=GetLabel("Pelayanan")%></label></td>
                            <td>
                                <input type="hidden" value="" id="hdnServiceItemID" runat="server" />
                                <input type="hidden" value="" id="hdnServiceRevenueSharingID" runat="server" />
                                <input type="hidden" value="" id="hdnServiceItemUnit" runat="server" />
                                <input type="hidden" value="" id="hdnServiceBaseTariff" runat="server" />
                                <input type="hidden" value="" id="hdnServiceDiscountAmount" runat="server" />
                                <input type="hidden" value="" id="hdnServiceCoverageAmount" runat="server" />
                                <input type="hidden" value="" id="hdnServiceIsDicountInPercentage" runat="server" />
                                <input type="hidden" value="" id="hdnServiceIsCoverageInPercentage" runat="server" />
                                <input type="hidden" value="" id="hdnServiceBaseCITOAmount" runat="server" />
                                <input type="hidden" value="" id="hdnServiceIsCITOInPercentage" runat="server" />
                                <input type="hidden" value="" id="hdnServiceBaseComplicationAmount" runat="server" />
                                <input type="hidden" value="" id="hdnServiceIsComplicationInPercentage" runat="server" />
                                <input type="hidden" value="" id="hdnServiceItemFilterExpression" runat="server" />
                                <input type="hidden" value="" id="hdnServicePrice" runat="server" />
                                <input type="hidden" value="" id="hdnServicePriceComp1" runat="server" />
                                <input type="hidden" value="" id="hdnServicePriceComp2" runat="server" />
                                <input type="hidden" value="" id="hdnServicePriceComp3" runat="server" />
                                <input type="hidden" value="" id="hdnServiceBasePriceComp1" runat="server" />
                                <input type="hidden" value="" id="hdnServiceBasePriceComp2" runat="server" />
                                <input type="hidden" value="" id="hdnServiceBasePriceComp3" runat="server" />
                                <input type="hidden" value="" id="hdnServiceTransactionID" runat="server" />
                                <input type="hidden" value="" id="hdnServiceTransactionDate" runat="server" />
                                <input type="hidden" value="" id="hdnServiceTransactionTime" runat="server" />
                                <input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />
                                <input type="hidden" value="" id="hdnServiceVisitID" runat="server" />
                                <input type="hidden" value="" id="hdnServiceUnitName" runat="server" />

                                <table style="width:100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width:30%"/>
                                        <col style="width:3px"/>
                                        <col/>
                                    </colgroup>
                                    <tr>
                                        <td><asp:TextBox ID="txtServiceItemCode" Width="100%" runat="server" /></td>
                                        <td>&nbsp;</td>
                                        <td><asp:TextBox ID="txtServiceItemName" ReadOnly="true" Width="100%" runat="server" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblLink lblMandatory" id="lblPhysician"><%=GetLabel("Dokter/Paramedis")%></label></td>
                            <td>
                                <input type="hidden" value="" id="hdnServicePhysicianID" runat="server" />
                                <table style="width:100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width:30%"/>
                                        <col style="width:3px"/>
                                        <col/>
                                    </colgroup>
                                    <tr>
                                        <td><asp:TextBox ID="txtServicePhysicianCode" Width="100%" runat="server" /></td>
                                        <td>&nbsp;</td>
                                        <td><asp:TextBox ID="txtServicePhysicianName" ReadOnly="true" Width="100%" runat="server" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblDisabled" id="lblTestPartner"><%=GetLabel("Test Partner")%></label></td>
                            <td>
                                <input type="hidden" value="" id="hdnTestPartnerID" runat="server" />
                                <table style="width:100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width:30%"/>
                                        <col style="width:3px"/>
                                        <col/>
                                    </colgroup>
                                    <tr>
                                        <td><asp:TextBox ID="txtTestPartnerCode" Width="100%" runat="server" ReadOnly="true" /></td>
                                        <td>&nbsp;</td>
                                        <td><asp:TextBox ID="txtTestPartnerName" ReadOnly="true" Width="100%" runat="server" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label></label></td>
                            <td>
                                <input type="hidden" id="hdnDefaultTariffComp" runat="server" value="1" />
                                <table style="width:100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width:20px"/>
                                        <col style="width:100px"/>
                                        <col style="width:20px"/>
                                    </colgroup>
                                    <tr>
                                        <td><asp:CheckBox ID="chkServiceIsVariable" runat="server" /></td>
                                        <td class="tdLabel"><label><%=GetLabel("Variable")%></label></td>
                                        <td><asp:CheckBox ID="chkServiceIsUnbilledItem" runat="server" /></td>
                                        <td class="tdLabel"><label><%=GetLabel("Tidak Ditagihkan")%></label></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label><%=GetLabel("Kelas Tagihan")%></label></td>
                            <td>
                                <dxe:ASPxComboBox ID="cboServiceChargeClassID" ClientInstanceName="cboServiceChargeClassID" Width="100%" runat="server">
                                    <ClientSideEvents ValueChanged="function(s,e){ onCboServiceChargeClassIDValueChanged(); }" />
                                </dxe:ASPxComboBox>
                            </td> 
                        </tr> 
                        <tr>
                            <td class="tdLabel"><label><%=GetLabel("Harga Satuan")%></label></td>
                            <td><asp:TextBox ID="txtServiceUnitTariff" Width="100px" CssClass="txtCurrency" runat="server" />
                            <input type="button" id="btnEditUnitTariff" title='<%=GetLabel("Unit Tariff Component") %>' value="..." style="width:10%"  /></td>
                        </tr>                                        
                        <tr>
                            <td class="tdLabel"><label><%=GetLabel("Jumlah")%></label></td>
                            <td><asp:TextBox ID="txtServiceQty" Width="100px" CssClass="number" runat="server" /></td>
                        </tr>
                    </table>
                </td>
                <td valign="top">
                    <table>    
                        <colgroup>
                            <col style="width:100px"/>
                            <col style="width:20px"/>
                            <col />
                        </colgroup>   
                        
                        <tr>
                            <td class="tdLabel"><label><%=GetLabel("Harga")%></label></td>
                            <td>&nbsp;</td>
                            <td><asp:TextBox ReadOnly="true" ID="txtServiceTariff" Width="200px" CssClass="txtCurrency" runat="server" /></td>
                        </tr>   
                        <tr>
                            <td class="tdLabel"><label><%=GetLabel("CITO")%></label></td>
                            <td><asp:CheckBox ID="chkServiceIsCITO" runat="server" /></td>
                            <td><asp:TextBox ID="txtServiceCITO" ReadOnly="true" Width="200px" CssClass="txtCurrency" runat="server" /></td>
                        </tr>
                        <tr style="display:none">
                            <td class="tdLabel"><label><%=GetLabel("Penyulit")%></label></td>
                            <td><asp:CheckBox ID="chkServiceIsComplication" runat="server" /></td>
                            <td><asp:TextBox ID="txtServiceComplication" ReadOnly="true" Width="200px" CssClass="txtCurrency" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label><%=GetLabel("Diskon")%></label></td>
                            <td><asp:CheckBox ID="chkServiceIsDiscount" runat="server" /></td>
                            <td><asp:TextBox ID="txtServiceDiscount" Width="200px" CssClass="txtCurrency" runat="server" />
                            <input type="button" class="btnEditDiscount" title='<%=GetLabel("Discount Component") %>' value="..." style="width:10%"  /></td>
                        </tr>
                    </table>
                </td>
                <td valign="top">
                    <table>    
                        <colgroup>
                            <col style="width:100px"/>
                        </colgroup>    
                        <tr>
                            <td class="tdLabel"><label><%=GetLabel("Pasien")%></label></td>
                            <td><asp:TextBox ID="txtServicePatient" ReadOnly="true" Width="200px" CssClass="txtCurrency" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label><%=GetLabel("Instansi")%></label></td>
                            <td><asp:TextBox ID="txtServicePayer" ReadOnly="true" Width="200px" CssClass="txtCurrency" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label><%=GetLabel("Total")%></label></td>
                            <td><asp:TextBox ID="txtServiceTotal" ReadOnly="true" Width="200px" CssClass="txtCurrency" runat="server" /></td>
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
                                            <input type="button" id="btnServiceSave" value='<%= GetLabel("Save")%>' />
                                        </td>
                                        <td>
                                            <input type="button" id="btnServiceCancel" value='<%= GetLabel("Cancel")%>' />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <colgroup>
                <col style="width:100px"/>
                <col />
            </colgroup>
        </table>
    </fieldset>
</div>
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
                        <input type="hidden" id="hdnServiceAllTotalPatient" runat="server" value="" />
                        <input type="hidden" id="hdnServiceAllTotalPayer" runat="server" value="" />
                        <input type="hidden" value='<%#: Eval("ID") %>' bindingfield="ID" runat="server" id="hdnChargesDt" />
                        <asp:ListView ID="lvwService" runat="server">
                            <LayoutTemplate>                                
                                <table id="tblView" runat="server" class="grdService grdNormal notAllowSelect" cellspacing="0" rules="all" >
                                    <tr>  
                                        <th style="width:80px" rowspan="2"></th>
        <%--                                <th style="width:80px" rowspan="2">
                                            <div style="text-align:left;padding-left:3px">
                                                <%=GetLabel("Kode")%>
                                            </div>
                                        </th>
        --%>                                <th rowspan="2">
                                            <div style="text-align:left;padding-left:3px">
                                                <%=GetLabel("Item")%>
                                            </div>
                                        </th>
                                        <th rowspan="2" style="width:70px">
                                            <div style="text-align:left;padding-left:3px">
                                                <%=GetLabel("Kelas Tagihan")%>
                                            </div>
                                        </th>
                                        <th rowspan="2" style="width:80px">
                                            <div style="text-align:right;padding-right:3px">
                                                <%=GetLabel("Harga Satuan")%>
                                            </div>
                                        </th>
                                        <th rowspan="2" style="width:50px">
                                            <div style="text-align:right;padding-right:3px">
                                                <%=GetLabel("Jumlah")%>
                                            </div>
                                        </th>
                                        <th colspan="3"><%=GetLabel("HARGA")%></th>
                                        <th colspan="3"><%=GetLabel("TOTAL")%></th>
                                        <th rowspan="2" style="width:90px">
                                            <div style="text-align:right;padding-right:3px">
                                                <%=GetLabel("Petugas")%>
                                            </div>
                                        </th>    
                                        <th rowspan="2">&nbsp;</th>                            
                                    </tr>
                                    <tr>
                                        <th style="width:80px">
                                            <div style="text-align:right;padding-right:3px">
                                                <%=GetLabel("Harga")%>
                                            </div>
                                        </th>
                                        <th style="width:90px">
                                            <div style="text-align:right;padding-right:3px">
                                                <%=GetLabel("CITO")%>
                                            </div>
                                        </th>
                                        <th style="width:80px;display:none">
                                            <div style="text-align:right;padding-right:3px">
                                                <%=GetLabel("Penyulit")%>
                                            </div>
                                        </th>
                                        <th style="width:80px">
                                            <div style="text-align:right;padding-right:3px">
                                                <%=GetLabel("Diskon")%>
                                            </div>
                                        </th>
                                        <th style="width:80px">
                                            <div style="text-align:right;padding-right:3px">
                                                <%=GetLabel("Instansi")%>
                                            </div>
                                        </th>
                                        <th style="width:80px">
                                            <div style="text-align:right;padding-right:3px">
                                                <%=GetLabel("Pasien")%>
                                            </div>
                                        </th>
                                        <th style="width:80px">
                                            <div style="text-align:right;padding-right:3px">
                                                <%=GetLabel("Total")%>
                                            </div>
                                        </th>
                                    </tr>
                                    <tr runat="server" id="itemPlaceholder" ></tr>
                                    <tr id="Tr1" class="trFooter" runat="server">
                                        <td colspan="8" align="right" style="padding-right:3px"><%=GetLabel("TOTAL") %></td>
                                        <td align="right" style="padding-right:9px" id="tdServiceTotalPayer" class="tdServiceTotalPayer" runat="server"></td>
                                        <td align="right" style="padding-right:9px" id="tdServiceTotalPatient" class="tdServiceTotalPatient" runat="server"></td>
                                        <td align="right" style="padding-right:9px" id="tdServiceTotal" class="tdServiceTotal" runat="server"></td>
                                        <td>&nbsp;</td>
                                        <td>&nbsp;</td>
                                    </tr>
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td align="center">
                                        <div>
<%--                                            <table cellpadding="0" cellspacing="0">
                                                <tr>                                               
                                                </tr>
                                            </table>--%>
                                            <input type="hidden" value='<%#: Eval("ID") %>' bindingfield="ID" />
                                            <input type="hidden" value='<%#: Eval("ItemID") %>' bindingfield="ItemID" />
                                            <input type="hidden" value='<%#: Eval("ItemCode") %>' bindingfield="ItemCode" />
                                            <input type="hidden" value='<%#: Eval("ItemName1") %>' bindingfield="ItemName1" />
                                            <input type="hidden" value='<%#: Eval("RevenueSharingID") %>' bindingfield="RevenueSharingID" />
                                            <input type="hidden" value='<%#: Eval("ParamedicID") %>' bindingfield="ParamedicID" />
                                            <input type="hidden" value='<%#: Eval("ParamedicCode") %>' bindingfield="ParamedicCode" />
                                            <input type="hidden" value='<%#: Eval("ParamedicName") %>' bindingfield="ParamedicName" />
                                            <input type="hidden" value='<%#: Eval("BusinessPartnerID") %>' bindingfield="TestPartnerID" />
                                            <input type="hidden" value='<%#: Eval("BusinessPartnerCode") %>' bindingfield="TestPartnerCode" />
                                            <input type="hidden" value='<%#: Eval("BusinessPartnerName") %>' bindingfield="TestPartnerName" />
                                            <input type="hidden" value='<%#: Eval("ChargeClassID") %>' bindingfield="ChargeClassID" />
                                            <input type="hidden" value='<%#: Eval("ChargedQuantity") %>' bindingfield="ChargedQuantity" />
                                            <input type="hidden" value='<%#: Eval("BaseTariff") %>' bindingfield="BaseTariff" />
                                            <input type="hidden" value='<%#: Eval("Tariff") %>' bindingfield="Tariff" />
                                            <input type="hidden" value='<%#: Eval("TariffComp1") %>' bindingfield="TariffComp1" />
                                            <input type="hidden" value='<%#: Eval("TariffComp2") %>' bindingfield="TariffComp2" />
                                            <input type="hidden" value='<%#: Eval("TariffComp3") %>' bindingfield="TariffComp3" />
                                            <input type="hidden" value='<%#: Eval("GrossLineAmount") %>' bindingfield="GrossLineAmount" />
                                            <input type="hidden" value='<%#: Eval("IsAllowCITO") %>' bindingfield="IsAllowCITO" />
                                            <input type="hidden" value='<%#: Eval("IsAllowComplication") %>' bindingfield="IsAllowComplication" />
                                            <input type="hidden" value='<%#: Eval("IsAllowDiscount") %>' bindingfield="IsAllowDiscount" />
                                            <input type="hidden" value='<%#: Eval("IsAllowVariable") %>' bindingfield="IsAllowVariable" />
                                            <input type="hidden" value='<%#: Eval("IsAllowUnbilledItem") %>' bindingfield="IsAllowUnbilledItem" />
                                            <input type="hidden" value='<%#: Eval("IsCITO") %>' bindingfield="IsCITO" />
                                            <input type="hidden" value='<%#: Eval("IsCITOInPercentage") %>' bindingfield="IsCITOInPercentage" />
                                            <input type="hidden" value='<%#: Eval("IsComplication") %>' bindingfield="IsComplication" />
                                            <input type="hidden" value='<%#: Eval("IsComplicationInPercentage") %>' bindingfield="IsComplicationInPercentage" />
                                            <input type="hidden" value='<%#: Eval("IsDiscount") %>' bindingfield="IsDiscount" />
                                            <input type="hidden" value='<%#: Eval("IsVariable") %>' bindingfield="IsVariable" />
                                            <input type="hidden" value='<%#: Eval("DefaultTariffComp") %>' bindingfield="DefaultTariffComp" />
                                            <input type="hidden" value='<%#: Eval("IsUnbilledItem") %>' bindingfield="IsUnbilledItem" />
                                            <input type="hidden" value='<%#: Eval("BaseCITOAmount") %>' bindingfield="BaseCITOAmount" />
                                            <input type="hidden" value='<%#: Eval("CITOAmount") %>' bindingfield="CITOAmount" />
                                            <input type="hidden" value='<%#: Eval("DiscountAmount") %>' bindingfield="DiscountAmount" />
                                            <input type="hidden" value='<%#: Eval("BaseComplicationAmount") %>' bindingfield="BaseComplicationAmount" />
                                            <input type="hidden" value='<%#: Eval("ComplicationAmount") %>' bindingfield="ComplicationAmount" />
                                            <input type="hidden" value='<%#: Eval("PatientAmount") %>' bindingfield="PatientAmount" />
                                            <input type="hidden" value='<%#: Eval("PayerAmount") %>' bindingfield="PayerAmount" />
                                            <input type="hidden" value='<%#: Eval("LineAmount") %>' bindingfield="LineAmount" />
                                        </div>
                                    </td>
                                    <td>
                                        <div style="padding:3px">
                                            <div><%#: Eval("ItemName1")%></div>
                                            <div><span style="font-style:italic"><%#: Eval("ItemCode") %></span>, <span style="color:Blue"> <%#: Eval("ParamedicName")%></span>
                                            </div> 
                                            <div <%# Eval("BusinessPartnerName").ToString() != "" ?  "" : "style='display:none'" %>><%#: Eval("BusinessPartnerName")%></div>
                                        </div>
                                    </td>
                                    <td>
                                        <div style="padding:3px;">
                                            <div><%#: Eval("ChargeClassName")%></div>                                                   
                                        </div>
                                    </td>
                                    <td>
                                        <div style="padding:3px;text-align:right;">
                                            <div><%#: Eval("Tariff", "{0:N}")%></div>                                                   
                                        </div>
                                    </td>
                                    <td>
                                        <div style="padding:3px;text-align:right;">
                                            <div><%#: Eval("ChargedQuantity")%></div>                                                   
                                        </div>
                                    </td>
                                    <td>
                                        <div style="padding:3px;text-align:right;">
                                            <div><%#: Eval("GrossLineAmount", "{0:N}")%></div>                                                   
                                        </div>
                                    </td>
                                    <td>
                                        <div style="padding:3px;text-align:right;">
                                            <div><%#: Eval("CITOAmount", "{0:N}")%></div>                                                   
                                        </div>
                                    </td>
                                    <td style="display:none">
                                        <div style="padding:3px;text-align:right;">
                                            <div><%#: Eval("ComplicationAmount", "{0:N}")%></div>                                                   
                                        </div>
                                    </td>
                                    <td>
                                        <div style="padding:3px;text-align:right;">
                                            <div><%#: Eval("DiscountAmount", "{0:N}")%></div>                                                   
                                        </div>
                                    </td>
                                    <td>
                                        <div style="padding:3px;text-align:right;">
                                            <div><%#: Eval("PayerAmount", "{0:N}")%></div>                                                   
                                        </div>
                                    </td>
                                    <td>
                                        <div style="padding:3px;text-align:right;">
                                            <div><%#: Eval("PatientAmount", "{0:N}")%></div>                                                   
                                        </div>
                                    </td>
                                    <td>
                                        <div style="padding:3px;text-align:right;">
                                            <div><%#: Eval("LineAmount", "{0:N}")%></div>                                                   
                                        </div>
                                    </td>
                                    <td>
                                        <div style="padding-right:3px;text-align:right;">
                                            <div><%#: Eval("CreatedByFullName")%></div>
                                            <div><%#: Eval("CreatedDateInString")%></div>  
                                        </div>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:ListView>
                    </asp:Panel>
                </dx:PanelContent>
            </PanelCollection>
        </dxcp:ASPxCallbackPanel>
        <div class="imgLoadingGrdView" id="containerImgLoadingView">
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
    </div>
</asp:Content>
