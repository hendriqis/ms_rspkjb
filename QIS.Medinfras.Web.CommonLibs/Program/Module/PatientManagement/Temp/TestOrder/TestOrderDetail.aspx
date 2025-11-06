<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx2.master" AutoEventWireup="true" 
    CodeBehind="TestOrderDetail.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.TestOrderDetail" %>

<%@ Register Src="~/Libs/Controls/PatientBannerCtl.ascx" TagName="PatientBannerCtl" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnTestOrderBack" runat="server" CRUDMode="R"><img src='<%=ResolveUrl("~/Libs/Images/Icon/back.png")%>' alt="" /><div><%=GetLabel("Back")%></div></li>
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
        function onAfterSaveRecordDtSuccess(testOrderID) {
            if ($('#<%=hdnTestOrderID.ClientID %>').val() == '0') {
                $('#<%=hdnTestOrderID.ClientID %>').val(testOrderID);
                var filterExpression = 'TestOrderID = ' + testOrderID;
                Methods.getObject('GetTestOrderHdList', filterExpression, function (result) {
                    $('#<%=txtTestOderNo.ClientID %>').val(result.TestOrderNo);
                });

                onAfterCustomSaveSuccess();
            }
        }

        function onLoad() {
            setDatePicker('<%=txtTestOrderDate.ClientID %>');

            $('#<%=btnTestOrderBack.ClientID %>').click(function () {
                showLoadingPanel();
                document.location = ResolveUrl('~/Program/PatientList/VisitList.aspx?id=to');
            });

            $('#lblTestOrderQuickPick').click(function (evt) {
                if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                    showLoadingPanel();
                    var url = ResolveUrl('~/Libs/Program/Module/PatientManagement/TestOrder/TestOrderQuickPicksCtl.ascx');
                    var testOrderID = $('#<%=hdnTestOrderID.ClientID %>').val();
                    var paramedicID = $('#<%=hdnPhysicianID.ClientID %>').val();
                    var testOrderDate = $('#<%=txtTestOrderDate.ClientID %>').val();
                    var testOrderTime = $('#<%=txtTestOrderTime.ClientID %>').val();
                    var serviceUnitID = $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val();
                    var visitID = $('#<%=hdnVisitID.ClientID %>').val();
                    var svcUnitID = $('#<%=hdnServiceUnitID.ClientID %>').val();

                    $('#<%=lblServiceUnit.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%=lblPhysician.ClientID %>').attr('class', 'lblDisabled');

                    $('#<%=txtPhysicianCode.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtServiceUnitCode.ClientID %>').attr('readonly', 'readonly');

                    var id = testOrderID + '|' + paramedicID + '|' + testOrderDate + '|' + testOrderTime + '|' + serviceUnitID + '|' + visitID + '|' + svcUnitID;
                    openUserControlPopup(url, id, 'Quick Picks', 1000, 600);
                }
            });

            //#region Transaction No
            $('#lblTestOrderNo.lblLink').click(function () {
                var filterExpression = 'VisitID = ' + $('#<%=hdnVisitID.ClientID %>').val();
                openSearchDialog('testorderhd', filterExpression, function (value) {
                    $('#<%=txtTestOderNo.ClientID %>').val(value);
                    onTxtTestOrderNoChanged(value);
                });
            });

            $('#<%=txtTestOderNo.ClientID %>').change(function () {
                onTxtTestOrderNoChanged($(this).val());
            });

            function onTxtTestOrderNoChanged(value) {
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

            //#region Service Unit
            function getServiceUnitFilterFilterExpression() {
                var filterExpression = "HealthcareID = '" + AppSession.healthcareID + "' AND DepartmentID = 'DIAGNOSTIC'";
                return filterExpression;
            }
            $('#<%=lblServiceUnit.ClientID %>.lblLink').live('click', function () {
                openSearchDialog('serviceunitperhealthcare', getServiceUnitFilterFilterExpression(), function (value) {
                    $('#<%=txtServiceUnitCode.ClientID %>').val(value);
                    onTxtServiceUnitCodeChanged(value);
                });
            });

            $('#<%=txtServiceUnitCode.ClientID %>').live('change', function () {
                onTxtServiceUnitCodeChanged($(this).val());
            });

            function onTxtServiceUnitCodeChanged(value) {
                var filterExpression = getServiceUnitFilterFilterExpression() + " AND ServiceUnitCode = '" + value + "'";
                Methods.getObject('GetvHealthcareServiceUnitList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val(result.HealthcareServiceUnitID);
                        $('#<%=hdnServiceUnitID.ClientID %>').val(result.ServiceUnitID); 
                        $('#<%=txtServiceUnitName.ClientID %>').val(result.ServiceUnitName);
                    }
                    else {
                        $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val('');
                        $('#<%=hdnServiceUnitID.ClientID %>').val('');
                        $('#<%=txtServiceUnitCode.ClientID %>').val('');
                        $('#<%=txtServiceUnitName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Diagnose
            $('#lblDiagnose.lblLink').live('click', function () {
                openSearchDialog('diagnose', "IsDeleted = 0", function (value) {
                    $('#<%=txtDiagnoseID.ClientID %>').val(value);
                    onTxtDiagnoseIDChanged(value);
                });
            });

            $('#<%=txtDiagnoseID.ClientID %>').live('change', function () {
                onTxtDiagnoseIDChanged($(this).val());
            });

            function onTxtDiagnoseIDChanged(value) {
                var filterExpression = "DiagnoseID = '" + value + "' AND IsDeleted = 0";
                Methods.getObject('GetDiagnoseList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=txtDiagnoseName.ClientID %>').val(result.DiagnoseName);
                    }
                    else {
                        $('#<%=txtDiagnoseID.ClientID %>').val('');
                        $('#<%=txtDiagnoseName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Item
            function getItemMasterFilterExpression() {
                var testOrderID = $('#<%=hdnTestOrderID.ClientID %>').val();
                var filterExpression = '';
                if (testOrderID != '')
                    filterExpression = 'ItemID IN (SELECT ItemID FROM ServiceUnitItem WHERE HealthcareServiceUnitID = ' + $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val() + ') AND ItemID NOT IN (SELECT ItemID FROM TestOrderDt WHERE TestOrderID = ' + $('#<%=hdnTestOrderID.ClientID %>').val() + ' AND IsDeleted = 0) AND IsDeleted = 0';
                else
                    filterExpression = 'ItemID IN (SELECT ItemID FROM ServiceUnitItem WHERE HealthcareServiceUnitID = ' + $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val() + ') AND IsDeleted = 0';
                return filterExpression;
            }

            $('#lblItem.lblLink').click(function () {
                openSearchDialog('item', getItemMasterFilterExpression(), function (value) {
                    $('#<%=txtItemCode.ClientID %>').val(value);
                    onTxtItemCodeChanged(value);
                });
            });

            $('#<%=txtItemCode.ClientID %>').change(function () {
                onTxtItemCodeChanged($(this).val());
            });

            function onTxtItemCodeChanged(value) {
                var filterExpression = getItemMasterFilterExpression() + " AND ItemCode = '" + value + "'";
                Methods.getObject('GetItemMasterList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnItemID.ClientID %>').val(result.ItemID);
                        $('#<%=txtItemName.ClientID %>').val(result.ItemName1);
                        $('#<%=hdnGCItemUnit.ClientID %>').val(result.GCItemUnit);
                    }
                    else {
                        $('#<%=hdnItemID.ClientID %>').val('');
                        $('#<%=txtItemCode.ClientID %>').val('');
                        $('#<%=txtItemName.ClientID %>').val('');
                        $('#<%=hdnGCItemUnit.ClientID %>').val('');                        
                    }
                });
            }
            //#endregion

            $('#lblAddData').click(function (evt) {
                if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                    $('#<%=lblServiceUnit.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%=lblPhysician.ClientID %>').attr('class', 'lblDisabled');

                    $('#<%=txtPhysicianCode.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtServiceUnitCode.ClientID %>').attr('readonly', 'readonly');

                    $('#<%=hdnEntryID.ClientID %>').val('');
                    $('#<%=hdnItemID.ClientID %>').val('');
                    $('#<%=txtItemCode.ClientID %>').val('');
                    $('#<%=txtItemName.ClientID %>').val('');

                    $('#<%=txtRemarks.ClientID %>').val('');
                    cboToBePerformed.SetValue(Constant.ToBePerformed.CURRENT_EPISODE);
                    $('#<%=txtPerformDate.ClientID %>').val('');

                    $('#<%=txtDiagnoseID.ClientID %>').val('');
                    $('#<%=txtDiagnoseName.ClientID %>').val('');

                    $('#containerEntry').show();
                }
            });

            $('#btnCancel').click(function () {
                $('#containerEntry').hide();
            });

            $('#btnSave').click(function (evt) {
                if (IsValid(evt, 'fsTrxPopup', 'mpTrxPopup'))
                    cbpProcess.PerformCallback('save');
            });

            setDatePicker('<%=txtPerformDate.ClientID %>');
        }

        $('#<%=grdView.ClientID %> .imgDelete.imgLink').live('click', function () {
            $row = $(this).closest('tr').parent().closest('tr');
            showToastConfirmation('Are You Sure Want To Delete?', function (result) {
                if (result) {                    
                    var entity = rowToObject($row);
                    $('#<%=hdnEntryID.ClientID %>').val(entity.ID);
                    cbpProcess.PerformCallback('delete');
                }
            });
        });

        $('#<%=grdView.ClientID %> .imgEdit.imgLink').live('click', function () {
            $row = $(this).closest('tr').parent().closest('tr');
            var entity = rowToObject($row);

            $('#<%=hdnEntryID.ClientID %>').val(entity.ID);
            $('#<%=hdnItemID.ClientID %>').val(entity.ItemID);
            $('#<%=txtItemCode.ClientID %>').val(entity.ItemCode);
            $('#<%=txtItemName.ClientID %>').val(entity.ItemName1);

            $('#<%=txtRemarks.ClientID %>').val(entity.Remarks);
            cboToBePerformed.SetValue(entity.GCToBePerformed);
            if (entity.GCToBePerformed == Constant.ToBePerformed.SCHEDULLED)
                $('#<%=txtPerformDate.ClientID %>').val(entity.PerformedDate);
            else
                $('#<%=txtPerformDate.ClientID %>').val('');

            $('#<%=txtDiagnoseID.ClientID %>').val(entity.DiagnoseID);
            $('#<%=txtDiagnoseName.ClientID %>').val(entity.DiagnoseName);

            onCboToBePerformedChanged();
            $('#containerEntry').show();
        });

        function onCboToBePerformedChanged() {
            if (cboToBePerformed.GetValue() != null && cboToBePerformed.GetValue() == Constant.ToBePerformed.SCHEDULLED) {
                $('#<%=txtPerformDate.ClientID %>').removeAttr('readonly');
                $('#<%=txtPerformDate.ClientID %>').datepicker('enable');
            }
            else {
                $('#<%=txtPerformDate.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtPerformDate.ClientID %>').datepicker('disable');
            }
        }

        function getTrxDate() {
            var date = Methods.getDatePickerDate($('#<%=txtTestOrderDate.ClientID %>').val());
            var dateInYMD = Methods.dateToYMD(date);
            return dateInYMD;
        }

        function onCbpProcesEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'save') {
                if (param[1] == 'fail')
                    showToast('Save Failed', 'Error Message : ' + param[2]);
                else {
                    var testOrderID = s.cpTestOrderID;
                    onAfterSaveRecordDtSuccess(testOrderID);
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
            if ($('#<%=txtTestOderNo.ClientID %>').val() == '')
                onAfterAddRecordAddRowCount();
            onLoadObject(param);
        }
    </script> 
    <input type="hidden" value="" id="hdnTestOrderID" runat="server" />  
    <div style="height:435px;overflow-y:auto;overflow-x:hidden;">
        <div class="pageTitle">
            <div style="font-size: 1.1em"><%=GetLabel("Order Pemeriksaan Penunjang Medis")%></div>
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
                            <td class="tdLabel"><label class="lblLink" id="lblTestOrderNo"><%=GetLabel("No Order")%></label></td>
                            <td><asp:TextBox ID="txtTestOderNo" Width="150px" ReadOnly="true" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><%=GetLabel("Tanggal") %> - <%=GetLabel("Jam") %></td>
                            <td>
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="padding-right: 1px;width:145px"><asp:TextBox ID="txtTestOrderDate" Width="120px" CssClass="datepicker" runat="server" /></td>
                                    <td style="width:5px">&nbsp;</td>
                                    <td><asp:TextBox ID="txtTestOrderTime" Width="80px" CssClass="time" runat="server" Style="text-align:center" /></td>
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
                            <td class="tdLabel"><label class="lblLink lblMandatory" runat="server" id="lblServiceUnit"><%=GetLabel("Penunjang Medis")%></label></td>
                            <td>
                                <input type="hidden" id="hdnHealthcareServiceUnitID" value="" runat="server" />
                                <input type="hidden" id="hdnServiceUnitID" value="" runat="server" />
                                <table style="width:100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width:30%"/>
                                        <col style="width:3px"/>
                                        <col/>
                                    </colgroup>
                                    <tr>
                                        <td><asp:TextBox ID="txtServiceUnitCode" Width="100%" runat="server" /></td>
                                        <td>&nbsp;</td>
                                        <td><asp:TextBox ID="txtServiceUnitName" Width="100%" runat="server" /></td>
                                    </tr>
                                </table>
                            </td>                        
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
                                                <col style="width:100px"/>
                                            </colgroup>
                                            <tr>
                                                <td class="tdLabel"><label class="lblLink lblMandatory" id="lblItem"><%=GetLabel("Pelayanan")%></label></td>
                                                <td colspan="2">
                                                    <input type="hidden" value="" id="hdnItemID" runat="server" />
                                                    <input type="hidden" value="" id="hdnGCItemUnit" runat="server" />
                                                    <table cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width:120px"/>
                                                            <col style="width:3px"/>
                                                            <col style="width:250px"/>
                                                        </colgroup>
                                                        <tr>
                                                            <td><asp:TextBox ID="txtItemCode" Width="100%" runat="server" /></td>
                                                            <td>&nbsp;</td>
                                                            <td><asp:TextBox ID="txtItemName" ReadOnly="true" Width="100%" runat="server" /></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel"><label class="lblLink" id="lblDiagnose"><%=GetLabel("Diagnosa")%></label></td>
                                                <td>
                                                    <table cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width:120px"/>
                                                            <col style="width:3px"/>
                                                            <col style="width:250px"/>
                                                        </colgroup>
                                                        <tr>
                                                            <td><asp:TextBox ID="txtDiagnoseID" Width="100%" runat="server" /></td>
                                                            <td>&nbsp;</td>
                                                            <td><asp:TextBox ID="txtDiagnoseName" ReadOnly="true" Width="100%" runat="server" /></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel" style="vertical-align: top; padding-top: 5px;"><label class="lblNormal"><%=GetLabel("Keterangan")%></label></td>
                                                <td><asp:TextBox ID="txtRemarks" Width="500px" runat="server" TextMode="MultiLine" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td valign="top">
                                        <table style="width:100%; display:none">
                                            <tr>
                                                <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("To Be Performed")%></label></td>
                                                <td>
                                                    <dxe:ASPxComboBox runat="server" ID="cboToBePerformed" ClientInstanceName="cboToBePerformed" Width="300px">
                                                        <ClientSideEvents ValueChanged="function(s,e){ onCboToBePerformedChanged(); }" />
                                                    </dxe:ASPxComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Perform Date")%></label></td>
                                                <td><asp:TextBox ID="txtPerformDate" Width="120px" CssClass="datepicker" runat="server" /></td>
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
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                            EndCallback="function(s,e){ hideLoadingPanel(); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto; position: relative;font-size:0.95em;">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdService grdNormal notAllowSelect" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td><img class="imgEdit imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>' alt="" /></td>
                                                            <td style="width:1px">&nbsp;</td>
                                                            <td><img class="imgDelete imgLink" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>' alt="" /></td>
                                                        </tr>
                                                    </table>
                                                    <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                    <input type="hidden" value="<%#:Eval("ItemID") %>" bindingfield="ItemID" />
                                                    <input type="hidden" value="<%#:Eval("ItemCode") %>" bindingfield="ItemCode" />
                                                    <input type="hidden" value="<%#:Eval("ItemName1") %>" bindingfield="ItemName1" />
                                                    <input type="hidden" value="<%#:Eval("DiagnoseID") %>" bindingfield="DiagnoseID" />
                                                    <input type="hidden" value="<%#:Eval("DiagnoseName") %>" bindingfield="DiagnoseName" />
                                                    <input type="hidden" value="<%#:Eval("GCToBePerformed") %>" bindingfield="GCToBePerformed" />
                                                    <input type="hidden" value="<%#:Eval("PerformedDateInDatePickerFormat") %>" bindingfield="PerformedDate" />
                                                    <input type="hidden" value="<%#:Eval("Remarks") %>" bindingfield="Remarks" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ItemName1" HeaderText="Pelayanan" HeaderStyle-Width="300px" />
                                            <asp:BoundField DataField="DiagnoseName" HeaderText="Diagnosa" HeaderStyle-Width="300px" />
                                            <asp:BoundField DataField="Remarks" HeaderText="Remarks" />
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("No Data To Display")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>    
                    <div class="imgLoadingGrdView" id="containerImgLoadingView" >
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>     
                    <div style="width:100%;text-align:center">
                        <span class="lblLink" id="lblAddData" style="margin-right: 300px;"><%= GetLabel("Add Data")%></span>
                        <span class="lblLink" id="lblTestOrderQuickPick"><%= GetLabel("Quick Picks")%></span>
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