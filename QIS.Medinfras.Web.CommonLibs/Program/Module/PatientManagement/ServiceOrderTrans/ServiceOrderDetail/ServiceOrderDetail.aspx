<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx.master" AutoEventWireup="true" 
    CodeBehind="ServiceOrderDetail.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.ServiceOrderDetail" %>

<%@ Register Src="~/Libs/Controls/PatientBannerCtl.ascx" TagName="PatientBannerCtl" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<asp:Content ID="Content4" ContentPlaceHolderID="plhCustomButtonToolbarLeft" runat="server">
    <li id="btnPrescriptionBack" runat="server" CRUDMode="R"><img src='<%=ResolveUrl("~/Libs/Images/Icon/back.png")%>' alt="" /><div><%=GetLabel("Back")%></div></li>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnClinicTransactionTestOrder" runat="server" CRUDMode="R"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbset.png")%>' alt="" /><div><%=GetLabel("Prescription Detail")%></div></li>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">   
    <input type="hidden" value="" id="hdnGCRegistrationStatus" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnVisitID" runat="server" />
    <input type="hidden" value="" id="hdnClassID" runat="server" />        
    <input type="hidden" value="" id="hdnImagingServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnLaboratoryServiceUnitID" runat="server" />
    <uc1:PatientBannerCtl ID="ctlPatientBanner" runat="server" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">   
    <script type="text/javascript">
        $(function () {
            $('#<%=btnClinicTransactionTestOrder.ClientID %>').click(function () {
                var prescriptionOrderID = $('#<%=hdnPrescriptionOrderID.ClientID %>').val();
                var url = ResolveUrl("~/Program/Prescription/PrescriptionOrder/PrescriptionOrderNotesCtl.ascx");
                openUserControlPopup(url, prescriptionOrderID, 'Prescription Order Notes', 800, 600);
            });

            if ($('#<%=btnClinicTransactionTestOrder.ClientID %>').is(':visible'))
                $('#<%=btnClinicTransactionTestOrder.ClientID %>').click();
        });
    
        function onAfterSaveRecordDtSuccess(PrescriptionID) {
            if ($('#<%=hdnPrescriptionID.ClientID %>').val() == '0') {
                $('#<%=hdnPrescriptionID.ClientID %>').val(PrescriptionID);
                var filterExpression = 'PrescriptionID = ' + PrescriptionID;
                Methods.getObject('GetPrescriptionHdList', filterExpression, function (result) {
                    $('#<%=txtPrescriptionNo.ClientID %>').val(result.PrescriptionNo);
                });
                onAfterCustomSaveSuccess();
            }
        }

        function onLoad() {
            setDatePicker('<%=txtPrescriptionDate.ClientID %>');
            $('#<%=txtPhysicianName.ClientID %>').attr('readonly', 'readonly');

            function onGetDrugFilterExpression() {
                var LocationID = cboLocation.GetValue();
                var filterExpression = "ItemID IN (SELECT ItemID FROM ItemBalance WHERE LocationID = " + LocationID + " AND IsDeleted = 0)";
                return filterExpression;
            }

            $('#lblDrug.lblLink').click(function () {
                openSearchDialog('itembalance', onGetDrugFilterExpression(), function (value) {
                    $('#<%=txtDrugCode.ClientID %>').val(value);
                    ontxtDrugCodeChanged(value);
                });
            });

            $('#<%=txtDrugCode.ClientID %>').change(function () {
                ontxtDrugCodeChanged($(this).val());
            });

            function ontxtDrugCodeChanged(value) {
                var filterExpression = onGetDrugFilterExpression() + " AND ItemCode = '" + value + "'";
                Methods.getObject('GetvItemBalanceList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnDrugID.ClientID %>').val(result.ItemID);
                        $('#<%=hdnDrugName.ClientID %>').val(result.ItemName1);
                        $('#<%=txtDrugCode.ClientID %>').val(result.ItemCode);
                        $('#<%=txtDrugName.ClientID %>').val(result.ItemName1);
                    }
                    else {
                        $('#<%=hdnDrugID.ClientID %>').val('');
                        $('#<%=txtDrugCode.ClientID %>').val('');
                        $('#<%=txtDrugName.ClientID %>').val('');
                    }
                });
            }

            $('#<%=btnPrescriptionBack.ClientID %>').click(function () {
                showLoadingPanel();
                document.location = ResolveUrl('~/Program/Prescription/PrescriptionEntry/PrescriptionEntryList.aspx');
            });

            setDatePicker('<%=txtStartDate.ClientID %>');
            
            
            //#region Transaction No
            $('#lblPrescriptionNo.lblLink').click(function () {
                var filterExpression = 'VisitID = ' + $('#<%=hdnVisitID.ClientID %>').val();
                openSearchDialog('prescriptionorderhd', filterExpression, function (value) {
                    $('#<%=txtPrescriptionNo.ClientID %>').val(value);
                    onTxtPrescriptionNoChanged(value);
                });
            });

            $('#<%=txtPrescriptionNo.ClientID %>').change(function () {
                onTxtPrescriptionNoChanged($(this).val());
            });

            function onTxtPrescriptionNoChanged(value) {
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
             
             //#region Operasi
            $('#lblAddData').click(function (evt) {
                if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                    $('#<%=lblPhysician.ClientID %>').attr('class', 'lblDisabled');
                    $('#containerEntry').show();
                    $('#<%=hdnEntryID.ClientID %>').val('');
                    $('#<%=chkIsRx.ClientID %>').prop('checked', true);
                    $('#<%=txtGenericName.ClientID %>').val('');
                    $('#<%=hdnDrugID.ClientID %>').val('');
                    $('#<%=hdnDrugName.ClientID %>').val('');
                    cboForm.SetValue('');
                    $('#<%=txtPurposeOfMedication.ClientID %>').val('');
                    $('#<%=txtStrengthAmount.ClientID %>').val('');
                    cboStrengthUnit.SetValue('');
                    cboFrequencyTimeline.SetValue(Constant.DosingFrequency.DAY);
                    $('#<%=txtFrequencyNumber.ClientID %>').val('');
                    $('#<%=txtDosingDose.ClientID %>').val('');
                    cboDosingUnit.SetValue('');
                    cboMedicationRoute.SetValue('');
                    $('#<%=txtMedicationAdministration.ClientID %>').val('');
                    $('#<%=txtDispenseQty.ClientID %>').val('');
                }
            });

            $('.imgEdit.imgLink').die('click');
            $('.imgEdit.imgLink').live('click', function () {
                $row = $(this).closest('tr').parent().closest('tr');
                var entity = rowToObject($row);
                $('#<%=hdnEntryID.ClientID %>').val(entity.PrescriptionDetailID);
                $('#<%=grdView.ClientID %> tr.focus').addClass('selected');
                $('#<%=chkIsRx.ClientID %>').prop('checked', (entity.IsRFlag == 'True'));
                $('#<%=txtDrugCode.ClientID %>').val(entity.ItemCode);
                $('#<%=txtDrugName.ClientID %>').val(entity.DrugName);
                $('#<%=txtGenericName.ClientID %>').val(entity.GenericName);
                $('#<%=hdnDrugID.ClientID %>').val(entity.ItemID);
                $('#<%=hdnDrugName.ClientID %>').val(entity.DrugName);
                cboForm.SetValue(entity.GCDrugForm);
                $('#<%=txtPurposeOfMedication.ClientID %>').val(entity.MedicationPurpose);
                $('#<%=txtStrengthAmount.ClientID %>').val(entity.Dose);
                cboStrengthUnit.SetValue(entity.GCDoseUnit);
                cboFrequencyTimeline.SetValue(entity.GCDosingFrequency);
                $('#<%=txtFrequencyNumber.ClientID %>').val(entity.Frequency);
                $('#<%=txtDosingDose.ClientID %>').val(entity.NumberOfDosage);
                cboDosingUnit.SetValue(entity.GCDosingUnit);
                cboMedicationRoute.SetValue(entity.GCRoute);
                $('#<%=txtStartDate.ClientID %>').val(entity.StartDateInString);
                $('#<%=txtStartTime.ClientID %>').val(entity.StartTime);
                $('#<%=txtMedicationAdministration.ClientID %>').val(entity.MedicationAdministration);
                $('#<%=txtDispenseQty.ClientID %>').val(entity.DispenseQty);
                $('#containerEntry').show();
            });

            $('.imgDelete.imgLink').die('click');
            $('.imgDelete.imgLink').live('click', function () {
                $row = $(this).closest('tr').parent().closest('tr');
                showToastConfirmation('Are You Sure Want To Delete?', function (result) {
                    if (result) {
                        var entity = rowToObject($row);
                        $('#<%=hdnEntryID.ClientID %>').val(entity.PrescriptionDetailID);
                        cbpProcess.PerformCallback('delete');
                    }
                });
            });

            $('#btnCancel').click(function () {
                $('#containerEntry').hide();
            });

            $('#btnSave').click(function (evt) {
                if (IsValid(evt, 'fsTrxPopup', 'mpTrxPopup'))
                    cbpProcess.PerformCallback('save');
            });
            //#endregion
        }

        function onCbpProcesEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'save') {
                if (param[1] == 'fail')
                    showToast('Save Failed', 'Error Message : ' + param[2]);
                else {
                    var PrescriptionID = s.cpPrescriptionID;
                    onAfterSaveRecordDtSuccess(PrescriptionID);
                    $('#containerEntry').hide();
                    cbpView.PerformCallback('refresh');
                }
            }
            else if (param[0] == 'delete') {
                if (param[1] == 'fail')
                    showToast('Delete Failed', 'Error Message : ' + param[2]);
                else
                    cbpView.PerformCallback('refresh');
            }
        }

        function onAfterSaveAddRecordEntryPopup(param) {
            if ($('#<%=txtPrescriptionNo.ClientID %>').val() == '')
                onAfterAddRecordAddRowCount();
            onLoadObject(param);
        }

    </script> 
    <input type="hidden" value="" id="hdnPrescriptionID" runat="server" />  
    <input type="hidden" value="" id="hdnPrescriptionOrderID" runat="server" /> 
    <div style="height:435px;overflow-y:auto;overflow-x:hidden;">
        <div class="pageTitle">
            <div style="font-size: 1.1em"><%=GetLabel("Pelayanan Resep")%></div>
        </div>
        <table class="tblContentArea">
            <colgroup>
                <col style="width:50%"/>
                <col style="width:50%"/>
            </colgroup>
            <tr>
                <td style="padding:5px;vertical-align:top">
                    <table class="tblEntryContent" style="width:100%">
                        <colgroup>
                            <col style="width:30%"/>
                            <col/>
                        </colgroup>
                        <tr>
                            <td class="tdLabel"><label class="lblLink" id="lblPrescriptionNo"><%=GetLabel("Prescription No")%></label></td>
                            <td><asp:TextBox ID="txtPrescriptionNo" Width="150px" ReadOnly="true" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal lblMandatory" /><%=GetLabel("Tanggal") %> - <%=GetLabel("Jam") %></td>
                            <td>
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="padding-right: 1px;width:145px"><asp:TextBox ID="txtPrescriptionDate" Width="120px" CssClass="datepicker" runat="server" /></td>
                                    <td style="width:5px">&nbsp;</td>
                                    <td><asp:TextBox ID="txtPrescriptionTime" Width="80px" CssClass="time" runat="server" Style="text-align:center" /></td>
                                </tr>
                            </table>
                        </td>
                        </tr>
                    </table>
                </td>
                <td style="padding:5px;vertical-align:top">
                    <table class="tblEntryContent" style="width:100%">
                        <colgroup>
                            <col style="width:30%"/>
                            <col/>
                        </colgroup>
                        <tr>
                            <td class="tdLabel"><label class="lblLink lblMandatory" runat="server" id="lblPhysician"><%=GetLabel("Dokter / Paramedis")%></label></td>
                            <td>
                                <input type="hidden" id="hdnPhysicianID" value="" runat="server" />
                                <table style="width:100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width:30%"/>
                                        <col style="width:3px"/>
                                        <col/>
                                    </colgroup>
                                    <tr>
                                        <td><asp:TextBox ID="txtPhysicianCode" Width="100%" runat="server" /></td>
                                        <td>&nbsp;</td>
                                        <td><asp:TextBox ID="txtPhysicianName" Width="100%" runat="server" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label><%=GetLabel("Refill Instruction")%></label></td>
                            <td><dxe:ASPxComboBox ID="cboRefillInstruction" ClientInstanceName="cboRefillInstruction" Width="300px" runat="server" /></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2"> 
                    <div id="containerEntry" style="margin-top:4px;display:none;">
                        <div class="pageTitle"><%=GetLabel("Entry")%></div>
                        <fieldset id="fsTrxPopup" style="margin:0"> 
                            <input type="hidden" value="" id="hdnEntryID" runat="server" />
                            <table style="width:100%" class="tblEntryDetail">
                                <colgroup>
                                    <col style="width:50%"/>
                                </colgroup>
                                <tr>
                                <td valign="top">
                                    <table style="width:100%">
                                        <colgroup>
                                            <col style="width:150px"/>
                                        </colgroup>
                                        <tr>
                                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Rx")%></label></td>
                                            <td><asp:CheckBox runat="server" ID="chkIsRx" /></td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Generic Name")%></label></td>
                                            <td><asp:TextBox runat="server" ID="txtGenericName" Width="300px" /></td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Location")%></label></td>
                                            <td><dxe:ASPxComboBox runat="server" ID="cboLocation" ClientInstanceName="cboLocation" Width="300px" /></td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel"><label class="lblLink lblMandatory" id="lblDrug"><%=GetLabel("Drug")%></label></td> 
                                            <td>
                                                <input type="hidden" value="" id="hdnDrugID" runat="server" />
                                                <input type="hidden" value="" id="hdnDrugName" runat="server" />
                                                <table style="width:100%" cellpadding="0" cellspacing="0">
                                                    <colgroup>
                                                        <col style="width:30%"/>
                                                        <col style="width:3px"/>
                                                        <col/>
                                                    </colgroup>
                                                    <tr>
                                                        <td><asp:TextBox ID="txtDrugCode" CssClass="required" Width="100%" runat="server" /></td> <!-- Ini customer item code yg waktu mau insert -->
                                                        <td>&nbsp;</td>
                                                        <td><asp:TextBox ID="txtDrugName" ReadOnly="true" CssClass="required" Width="100%" runat="server" /></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Strength")%></label></td>
                                            <td>
                                                <table cellpadding="0" cellspacing="0">
                                                    <colgroup>
                                                        <col style="width:100px"/>
                                                        <col style="width:3px"/>
                                                    </colgroup>
                                                    <tr>
                                                        <td><asp:TextBox ID="txtStrengthAmount" runat="server" Width="100%" CssClass="number" /></td>
                                                        <td>&nbsp;</td>
                                                        <td><dxe:ASPxComboBox ID="cboStrengthUnit" ClientInstanceName="cboStrengthUnit" runat="server" Width="100%" /></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Form")%></label></td>
                                            <td><dxe:ASPxComboBox runat="server" ID="cboForm" ClientInstanceName="cboForm" Width="300px" /></td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel" style="vertical-align: top; padding-top: 5px;"><label class="lblNormal"><%=GetLabel("Purpose Of Medication")%></label></td>
                                            <td><asp:TextBox ID="txtPurposeOfMedication" Width="300px" runat="server" TextMode="MultiLine" /></td>
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
                                                        <td><dxe:ASPxComboBox ID="cboFrequencyTimeline" ClientInstanceName="cboFrequencyTimeline" runat="server" Width="100%" /></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Dosing")%></label></td>
                                            <td>
                                                <table cellpadding="0" cellspacing="0">
                                                    <colgroup>
                                                        <col style="width:100px"/>
                                                        <col style="width:3px"/>
                                                        <col style="width:100px"/>
                                                    </colgroup>
                                                    <tr>
                                                        <td><asp:TextBox ID="txtDosingDose" runat="server" Width="100%" CssClass="number" /></td>
                                                        <td>&nbsp;</td>
                                                        <td><dxe:ASPxComboBox ID="cboDosingUnit" ClientInstanceName="cboDosingUnit" runat="server" Width="100%" /></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Medication Route")%></label></td>
                                            <td><dxe:ASPxComboBox runat="server" ID="cboMedicationRoute" ClientInstanceName="cboMedicationRoute" Width="300px" /></td>
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
                                            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Dispense Quantity")%></label></td>
                                            <td><asp:TextBox runat="server" ID="txtDispenseQty" Width="100px" CssClass="number" /></td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel" style="vertical-align: top; padding-top: 5px;"><label class="lblNormal"><%=GetLabel("Administration Instruction")%></label></td>
                                            <td><asp:TextBox ID="txtMedicationAdministration" Width="300px" runat="server" TextMode="MultiLine" /></td>
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
                                                                <input type="button" id="btnSave" value='<%= GetLabel("Save")%>' />
                                                            </td>
                                                            <td>
                                                                <input type="button" id="btnCancel" value='<%= GetLabel("Cancel")%>' />
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
                    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
                    <input type="hidden" id="hdnPrescriptionFlag" runat="server" value="" />
                    <div style="position: relative;">
                        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height:300px">
                                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                            <Columns>
                                                <asp:BoundField DataField="PrescriptionDetailID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                <asp:TemplateField  HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20px">
                                                    <ItemTemplate>
                                                        <table cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td><img class="imgEdit imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>' alt="" style="float:left; margin-left:7px" /></td>
                                                                <td style="width:3px">&nbsp;</td>
                                                                <td><img class="imgDelete imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>' alt="" /></td>
                                                            </tr>
                                                        </table>
                                                        <input type="hidden" value="<%#:Eval("PrescriptionDetailID") %>" bindingfield="PrescriptionDetailID" />
                                                        <input type="hidden" value="<%#:Eval("GenericName") %>" bindingfield="GenericName" />
                                                        <input type="hidden" value="<%#:Eval("IsRFlag") %>" bindingfield="IsRFlag" />
                                                        <input type="hidden" value="<%#:Eval("IsCompound") %>" bindingfield="IsCompound" />
                                                        <input type="hidden" value="<%#:Eval("ItemCode") %>" bindingfield="ItemCode" />
                                                        <input type="hidden" value="<%#:Eval("ItemID") %>" bindingfield="ItemID" />
                                                        <input type="hidden" value="<%#:Eval("DrugName") %>" bindingfield="DrugName" />
                                                        <input type="hidden" value="<%#:Eval("GCDrugForm") %>" bindingfield="GCDrugForm" />
                                                        <input type="hidden" value="<%#:Eval("Dose") %>" bindingfield="Dose" />
                                                        <input type="hidden" value="<%#:Eval("GCDoseUnit") %>" bindingfield="GCDoseUnit" />
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
                                                        <input type="hidden" value="<%#:Eval("StartDateInString") %>" bindingfield="StartDateInString" />                                        
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                    <HeaderTemplate>
                                                        <div><%=GetLabel("Generic")%> - <%=GetLabel("Product")%> - <%=GetLabel("Strength")%> - <%=GetLabel("Form")%></div>
                                                        <div><div style="color:Blue;width:35px;float:left;"><%=GetLabel("DOSE")%></div> <%=GetLabel("Dose")%> - <%=GetLabel("Route")%> - <%=GetLabel("Frequency")%></div>
                                                    </HeaderTemplate>
                                                    <ItemTemplate><div><%#: Eval("InformationLine1")%></div>
                                                        <div><div style="color:Blue;width:35px;float:left;"><%=GetLabel("DOSE")%></div> <%#: Eval("NumberOfDosage")%> <%#: Eval("DosingUnit")%> - <%#: Eval("Route")%> - <%#: Eval("cfDoseFrequency")%></div>
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
                        <div class="imgLoadingGrdView" id="Div1" >
                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                        </div>
                        <div class="containerPaging">
                            <div class="wrapperPaging">
                                <div id="paging"></div>
                            </div>
                        </div> 
                    </div>
                    <div class="imgLoadingGrdView" id="containerImgLoadingView" >
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>     
                    <div style="width:100%;text-align:center">
                        <span class="lblLink" id="lblAddData" style=" text-align:center"><%= GetLabel("Add Data")%></span>
                    </div>
                </td>
            </tr>
        </table>  
    </div>

    <dxcp:ASPxCallbackPanel ID="cbpProcess" runat="server" Width="100%" ClientInstanceName="cbpProcess"
        ShowLoadingPanel="false" OnCallback="cbpProcess_Callback">
        <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }"
            EndCallback="function(s,e) { onCbpProcesEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</asp:Content>