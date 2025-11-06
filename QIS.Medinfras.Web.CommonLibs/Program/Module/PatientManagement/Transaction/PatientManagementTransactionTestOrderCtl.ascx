<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PatientManagementTransactionTestOrderCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientManagementTransactionTestOrderCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_patientmanagementtestorderctl">
    setDatePicker('<%=txtTestOrderDate.ClientID %>');
    $('#<%=txtTestOrderDate.ClientID %>').datepicker('option', 'maxDate', '0');
    setDatePicker('<%=txtScheduledDate.ClientID %>');
    $('#<%=txtScheduledDate.ClientID %>').datepicker('option', 'minDate', '0');

    $('#<%=txtScheduledDate.ClientID %>').attr('readonly', 'readonly');
    $('#<%=txtScheduledDate.ClientID %>').datepicker('disable');
    $('#<%=txtScheduledTime.ClientID %>').attr('readonly', 'readonly');

    $('#<%=txtScheduledDate.ClientID %>').keyup(function () {
        var orderdate = $(this).val();
        $this.val(orderdate);
    });
    $('#<%=txtScheduledTime.ClientID %>').keyup(function () {
        var orderdate = $(this).val();
        $this.val(orderdate);
    });

    $('#<%=txtTestOrderDate.ClientID %>').change(function () {
        onChangedDate($(this).val());
    });
    $('#<%=txtTestOrderTime.ClientID %>').change(function () {
        onChangedDate($(this).val());
    });

    var filterExpression = "ParamedicID = " + $('#<%=hdnDefaultVisitParamedicID.ClientID %>').val() + " AND IsDeleted = 0";
    Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
        if (result != null) {
            $('#<%=txtPhysicianCode.ClientID %>').val(result.ParamedicCode);
            $('#<%=txtPhysicianName.ClientID %>').val(result.ParamedicName);
        }
        else {
            $('#<%=hdnPhysicianID.ClientID %>').val('');
            $('#<%=txtPhysicianCode.ClientID %>').val('');
            $('#<%=txtPhysicianName.ClientID %>').val('');
        }
    });

    function onChangedDate(value) {
        var date = $('#<%=txtTestOrderDate.ClientID %>').val();
        var time = $('#<%=txtTestOrderTime.ClientID %>').val();
        var orderdate = "";

        if (orderdate != "" && orderdate != null) {
        } else {
            orderdate = date;
        }
        $('#<%=txtScheduledDate.ClientID %>').val(orderdate);

        if (time != "" && time != null) {
        } else {
            time = time;
        }
        $('#<%=txtScheduledTime.ClientID %>').val(time);
    }

    window.onCbpViewCtlEndCallback = function (s, e) {
        hideLoadingPanel();
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail') {
                showToast('Save Failed', 'Error Message : ' + param[2]);
            }
            else {
                $('#containerEntry').hide();
                $('#<%=hdnTestOrderID.ClientID %>').val(s.cpTestOrderID);
                cbpMainPopup.PerformCallback('loadaftersave');
            }
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail') {
                showToast('Delete Failed', 'Error Message : ' + param[2]);
            }
            else {
                $('#containerEntry').hide();
                $('#<%=hdnTestOrderID.ClientID %>').val(s.cpTestOrderID);
                cbpMainPopup.PerformCallback('loadaftersave');
            }
        }
    }

    $('#btnSaveHeader').click(function (evt) {
        cbpMainPopup.PerformCallback('save');
    });

    $('#btnProposedHeader').live('click', function () {
        if ($('#<%=hdnTestOrderID.ClientID %>').val() != "" && $('#<%=hdnTestOrderID.ClientID %>').val() != "0")
            cbpMainPopup.PerformCallback('proposed');
    });

    $('#btnVoidHeader').live('click', function () {
        if ($('#<%=hdnTestOrderID.ClientID %>').val() != "" && $('#<%=hdnTestOrderID.ClientID %>').val() != "0")
            cbpMainPopup.PerformCallback('void');
    });

    $('#btnNewHeader').live('click', function () {
        cbpMainPopup.PerformCallback('new');
    });

    //#region Transaction No
    $('#lblTestOrderNo.lblLink').live('click', function () {
        var filterExpression = "VisitID = '" + $('#<%=hdnVisitID.ClientID %>').val() + "' AND HealthcareServiceUnitID <> '" + $('#<%=hdnCurrentHealthcareServiceUnitID.ClientID %>').val() + "'";
        openSearchDialog('testorderhd', filterExpression, function (value) {
            $('#<%=txtTestOrderNo.ClientID %>').val(value);
            onTxtTestOrderNoChanged(value);
        });
    });

    $('#<%=txtTestOrderNo.ClientID %>').change(function () {
        onTxtTestOrderNoChanged($(this).val());
    });

    function onTxtTestOrderNoChanged(value) {
        cbpMainPopup.PerformCallback('load');
    }
    //#endregion

    $('#lblAddData').live('click', function (evt) {
        if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup')) {
            $('#<%=lblServiceUnit.ClientID %>').attr('class', 'lblDisabled');
            $('#<%=lblPhysician.ClientID %>').attr('class', 'lblDisabled');

            $('#<%=txtPhysicianCode.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtServiceUnitCode.ClientID %>').attr('readonly', 'readonly');

            $('#<%=hdnEntryID.ClientID %>').val('');
            $('#<%=hdnItemID.ClientID %>').val('');
            $('#<%=txtItemCode.ClientID %>').val('');
            $('#<%=txtItemName.ClientID %>').val('');

            $('#<%=txtRemarks.ClientID %>').val('');
            $('#<%=txtDiagnoseID.ClientID %>').val('');
            $('#<%=txtDiagnoseName.ClientID %>').val('');

            $('#<%=chkIsCITO_Detail.ClientID %>').prop("checked", false);

            $('#containerEntry').show();
        }
    });

    //#region Physician
    $('#<%=lblPhysician.ClientID %>.lblLink').live('click', function () {
        var filterExpression = 'IsDeleted = 0';
        openSearchDialog('paramedic', filterExpression, function (value) {
            $('#<%=txtPhysicianCode.ClientID %>').val(value);
            onTxtPhysicianCodeChanged(value);
        });
    });

    $('#<%=txtPhysicianCode.ClientID %>').live('change', function () {
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
        var filterExpression = "<%:GetServiceUnitFilterFilterExpression() %>";
        return filterExpression;
    }
    $('#<%=lblServiceUnit.ClientID %>.lblLink').live('click', function () {
        if ($('#<%=txtTestOrderNo.ClientID %>').val() != '')
            return;
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

    //#region Item
    function getItemMasterFilterExpression() {
        var testOrderID = $('#<%=hdnTestOrderID.ClientID %>').val();
        var filterExpression = '';
        if (testOrderID != '') {
            if ($('#<%=hdnHealthcareServiceUnitID.ClientID %>').val() == $('#<%=hdnLaboratoryServiceUnitID.ClientID %>').val() || $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val() == $('#<%=hdnImagingServiceUnitID.ClientID %>').val())
                filterExpression = 'ItemID IN (SELECT ItemID FROM vServiceUnitItem WHERE HealthcareServiceUnitID = ' + $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val() + ' AND IsTestItem = 1) AND ItemID NOT IN (SELECT ItemID FROM TestOrderDt WHERE TestOrderID = ' + $('#<%=hdnTestOrderID.ClientID %>').val() + ' AND IsDeleted = 0) AND IsDeleted = 0';
            else filterExpression = 'ItemID IN (SELECT ItemID FROM vServiceUnitItem WHERE HealthcareServiceUnitID = ' + $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val() + ') AND ItemID NOT IN (SELECT ItemID FROM TestOrderDt WHERE TestOrderID = ' + $('#<%=hdnTestOrderID.ClientID %>').val() + ' AND IsDeleted = 0) AND IsDeleted = 0';
        }
        else {
            if ($('#<%=hdnHealthcareServiceUnitID.ClientID %>').val() == $('#<%=hdnLaboratoryServiceUnitID.ClientID %>').val() || $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val() == $('#<%=hdnImagingServiceUnitID.ClientID %>').val())
                filterExpression = 'ItemID IN (SELECT ItemID FROM vServiceUnitItem WHERE HealthcareServiceUnitID = ' + $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val() + ' AND IsTestItem = 1) AND IsDeleted = 0';
            else filterExpression = 'ItemID IN (SELECT ItemID FROM vServiceUnitItem WHERE HealthcareServiceUnitID = ' + $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val() + ') AND IsDeleted = 0';
        }
        filterExpression += " AND GCItemStatus != 'X181^999'";
        return filterExpression;
    }

    $('#lblItem.lblLink').live('click', function () {
        openSearchDialog('item', getItemMasterFilterExpression(), function (value) {
            $('#<%=txtItemCode.ClientID %>').val(value);
            onTxtItemCodeChanged(value);
        });
    });

    $('#<%=txtItemCode.ClientID %>').live('change', function () {
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

    $('#btnCancel').live('click', function () {
        $('#containerEntry').hide();
    });

    $('#btnSave').live('click', function (evt) {
        if (IsValid(evt, 'fsTrxPopup', 'mpTrxPopup')) {
            cbpViewCtl.PerformCallback('save');
        }
    });

    $('#<%=grdView.ClientID %> .imgDelete.imgLink').live('click', function () {
        $row = $(this).closest('tr').parent().closest('tr');
        showToastConfirmation('Are You Sure Want To Delete?', function (result) {
            if (result) {
                var entity = rowToObject($row);
                $('#<%=hdnEntryID.ClientID %>').val(entity.ID);
                cbpViewCtl.PerformCallback('delete');
            }
        });
    });

    function onCboToBePerformedChanged() {
        if (cboToBePerformed.GetValue() != null && (cboToBePerformed.GetValue() == Constant.ToBePerformed.SCHEDULLED || cboToBePerformed.GetValue() == Constant.ToBePerformed.PRIOR_TO_NEXT_VISIT)) {
            if (cboToBePerformed.GetValue() == Constant.ToBePerformed.SCHEDULLED) {
                $('#<%=txtScheduledDate.ClientID %>').removeAttr('readonly');
                $('#<%=txtScheduledDate.ClientID %>').datepicker('enable');
                $('#<%=txtScheduledTime.ClientID %>').removeAttr('readonly');
            }

            $('#<%=chkIsCITO.ClientID %>').attr('disabled', true);
            $('#<%=chkIsCITO.ClientID %>').prop("checked", false);
        }
        else {
            $('#<%=txtScheduledDate.ClientID %>').val($('#<%=hdnDatePickerToday.ClientID %>').val());
            $('#<%=txtScheduledTime.ClientID %>').val($('#<%=hdnTimeToday.ClientID %>').val());

            $('#<%=txtScheduledDate.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtScheduledDate.ClientID %>').datepicker('disable');
            $('#<%=txtScheduledTime.ClientID %>').attr('readonly', 'readonly');

            $('#<%=chkIsCITO.ClientID %>').removeAttr('disabled');
        }
    }

    $('#<%=grdView.ClientID %> .imgEdit.imgLink').live('click', function () {
        $row = $(this).closest('tr').parent().closest('tr');
        var entity = rowToObject($row);

        $('#<%=hdnEntryID.ClientID %>').val(entity.ID);
        $('#<%=hdnItemID.ClientID %>').val(entity.ItemID);
        $('#<%=txtItemCode.ClientID %>').val(entity.ItemCode);
        $('#<%=txtItemName.ClientID %>').val(entity.ItemName1);

        $('#<%=txtRemarks.ClientID %>').val(entity.Remarks);
        $('#<%=txtDiagnoseID.ClientID %>').val(entity.DiagnoseID);
        $('#<%=txtDiagnoseName.ClientID %>').val(entity.DiagnoseName);
        if (entity.IsCito == "True") {
            $('#<%=chkIsCITO_Detail.ClientID %>').prop("checked", true);
        } else {
            $('#<%=chkIsCITO_Detail.ClientID %>').prop("checked", false);
        }
        $('#containerEntry').show();
    });

    function onCbpMainPopupEndCallback(s, e) {
        var result = s.cpResult.split('|');
        if (result[0] == 'save') {
            if (result[1] != 'success') {
                if (result[2] != '')
                    showToast('Save Failed', 'Error Message : ' + result[2]);
                else
                    showToast('Save Failed', '');
            } else {
                $('#containerEntry').hide();
                $('#<%=hdnTestOrderID.ClientID %>').val(s.cpTestOrderID);
                cbpMainPopup.PerformCallback('loadaftersave');
            }
        }
        if (result[0] == 'proposed') {
            if (result[1] != 'success') {
                if (result[2] != '')
                    showToast('Proposed Failed', 'Error Message : ' + result[2]);
                else
                    showToast('Proposed Failed', '');
            } else {
                $('#containerEntry').hide();
                $('#<%=hdnTestOrderID.ClientID %>').val(s.cpTestOrderID);
                cbpMainPopup.PerformCallback('loadaftersave');
            }
        }
        if (result[0] == 'void') {
            if (result[1] != 'success') {
                if (result[2] != '')
                    showToast('Void Failed', 'Error Message : ' + result[2]);
                else
                    showToast('Void Failed', '');
            } else {
                $('#containerEntry').hide();
                $('#<%=hdnTestOrderID.ClientID %>').val(s.cpTestOrderID);
                cbpMainPopup.PerformCallback('loadaftersave');
            }
        }
        hideLoadingPanel();
    }

</script>
<input type="hidden" value="" id="hdnDatePickerToday" runat="server" />
<input type="hidden" value="" id="hdnTimeToday" runat="server" />
<input type="hidden" value="" id="hdnDefaultVisitParamedicID" runat="server" />
<div style="height: 400px; overflow-y: scroll;">
    <dxcp:ASPxCallbackPanel ID="cbpMainPopup" runat="server" Width="100%" ClientInstanceName="cbpMainPopup"
        ShowLoadingPanel="false" OnCallback="cbpMainPopup_Callback">
        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpMainPopupEndCallback(s,e); }" />
        <PanelCollection>
            <dx:PanelContent ID="PanelContent2">
                <fieldset id="fsEntryPopup" style="margin: 0">
                    <asp:Panel runat="server" ID="Panel1" Style="width: 100%; margin-left: auto; margin-right: auto;
                        position: relative; font-size: 0.95em;">
                        <input type="button" id="btnNewHeader" class="w3-button w3-khaki w3-round-large" value='<%= GetLabel("New")%>' />
                        <input type="button" id="btnSaveHeader" class="w3-button w3-blue w3-round-large" value='<%= GetLabel("Save")%>' <%= IsEditable == true ? "" : "style='display:none'" %> />
                        <input type="button" id="btnProposedHeader" class="w3-button w3-green w3-round-large" value='<%= GetLabel("Proposed")%>' <%= IsEditable == true ? "" : "style='display:none'" %> />
                        <input type="button" id="btnVoidHeader" class="w3-button w3-red w3-round-large" value='<%= GetLabel("Void")%>' <%= IsEditable == true ? "" : "style='display:none'" %> />
                        
                        <input type="hidden" value="" id="hdnTestOrderID" runat="server" />
                        <input type="hidden" value="" id="hdnImagingServiceUnitID" runat="server" />
                        <input type="hidden" value="" id="hdnLaboratoryServiceUnitID" runat="server" />
                        <input type="hidden" value="" id="hdnVisitID" runat="server" />
                        <input type="hidden" value="" id="hdnCurrentHealthcareServiceUnitID" runat="server" />
                        <table class="tblContentArea">
                            <colgroup>
                                <col style="width: 50%" />
                                <col style="width: 50%" />
                            </colgroup>
                            <tr>
                                <td style="padding: 5px; vertical-align: top">
                                    <table class="tblEntryContent" style="width: 100%">
                                        <colgroup>
                                            <col style="width: 30%" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblLink" id="lblTestOrderNo">
                                                    <%=GetLabel("No. Order")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTestOrderNo" Width="232px" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <%=GetLabel("Tanggal") %>
                                                -
                                                <%=GetLabel("Jam Order") %>
                                            </td>
                                            <td>
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td style="padding-right: 1px; width: 145px">
                                                            <asp:TextBox ID="txtTestOrderDate" Width="120px" CssClass="datepicker" runat="server" />
                                                        </td>
                                                        <td style="width: 5px">
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtTestOrderTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                                <label>
                                                    <%=GetLabel("Catatan")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtNotes" Width="100%" TextMode="MultiLine" Height="90px" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="padding: 5px; vertical-align: top">
                                    <table class="tblEntryContent" style="width: 100%">
                                        <colgroup>
                                            <col style="width: 30%" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblLink lblMandatory" runat="server" id="lblPhysician">
                                                    <%=GetLabel("Dokter")%></label>
                                            </td>
                                            <td>
                                                <input type="hidden" id="hdnPhysicianID" value="" runat="server" />
                                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                    <colgroup>
                                                        <col style="width: 120px" />
                                                        <col style="width: 3px" />
                                                        <col />
                                                    </colgroup>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtPhysicianCode" Width="120px" runat="server" />
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtPhysicianName" Width="100%" runat="server" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblLink lblMandatory" runat="server" id="lblServiceUnit">
                                                    <%=GetLabel("Penunjang Medis")%></label>
                                            </td>
                                            <td>
                                                <input type="hidden" id="hdnHealthcareServiceUnitID" value="" runat="server" />
                                                <input type="hidden" id="hdnServiceUnitID" value="" runat="server" />
                                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                    <colgroup>
                                                        <col style="width: 120px" />
                                                        <col style="width: 3px" />
                                                        <col />
                                                    </colgroup>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtServiceUnitCode" Width="120px" runat="server" />
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtServiceUnitName" Width="100%" runat="server" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblMandatory">
                                                    <%=GetLabel("Waktu Pengerjaan")%></label>
                                            </td>
                                            <td>
                                                <dxe:ASPxComboBox runat="server" ID="cboToBePerformed" ClientInstanceName="cboToBePerformed"
                                                    Width="100%">
                                                    <ClientSideEvents ValueChanged="function(s,e){ onCboToBePerformedChanged(); }" />
                                                </dxe:ASPxComboBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                Dilakukan Tanggal
                                            </td>
                                            <td>
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td style="padding-right: 1px; width: 145px">
                                                            <asp:TextBox ID="txtScheduledDate" Width="120px" CssClass="datepicker" runat="server" />
                                                        </td>
                                                        <td style="width: 5px">
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtScheduledTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td />
                                            <td>
                                                <asp:CheckBox ID="chkIsCITO" Width="100px" runat="server" Text="CITO" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label>
                                                    <%=GetLabel("Status")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtStatus" Width="120px" runat="server" Style="text-align: center;
                                                    color: Red" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div id="containerEntry" style="margin-top: 4px; display: none;">
                                        <div class="pageTitle">
                                            <%=GetLabel("Entry")%></div>
                                        <fieldset id="fsTrxPopup" style="margin: 0">
                                            <input type="hidden" value="" id="hdnEntryID" runat="server" />
                                            <table style="width: 100%" class="tblEntryDetail">
                                                <colgroup>
                                                    <col style="width: 50%" />
                                                </colgroup>
                                                <tr>
                                                    <td valign="top">
                                                        <table style="width: 100%">
                                                            <colgroup>
                                                                <col style="width: 100px" />
                                                            </colgroup>
                                                            <tr>
                                                                <td class="tdLabel">
                                                                    <label class="lblLink lblMandatory" id="lblItem">
                                                                        <%=GetLabel("Pelayanan")%></label>
                                                                </td>
                                                                <td colspan="2">
                                                                    <input type="hidden" value="" id="hdnItemID" runat="server" />
                                                                    <input type="hidden" value="" id="hdnGCItemUnit" runat="server" />
                                                                    <table cellpadding="0" cellspacing="0">
                                                                        <colgroup>
                                                                            <col style="width: 120px" />
                                                                            <col style="width: 3px" />
                                                                            <col style="width: 250px" />
                                                                        </colgroup>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:TextBox ID="txtItemCode" Width="100%" runat="server" />
                                                                            </td>
                                                                            <td>
                                                                                &nbsp;
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtItemName" ReadOnly="true" Width="100%" runat="server" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="tdLabel">
                                                                    <label class="lblLink" id="lblDiagnose">
                                                                        <%=GetLabel("Diagnosa")%></label>
                                                                </td>
                                                                <td>
                                                                    <table cellpadding="0" cellspacing="0">
                                                                        <colgroup>
                                                                            <col style="width: 120px" />
                                                                            <col style="width: 3px" />
                                                                            <col style="width: 250px" />
                                                                        </colgroup>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:TextBox ID="txtDiagnoseID" Width="100%" runat="server" />
                                                                            </td>
                                                                            <td>
                                                                                &nbsp;
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtDiagnoseName" ReadOnly="true" Width="100%" runat="server" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td />
                                                                <td>
                                                                    <asp:CheckBox ID="chkIsCITO_Detail" Width="100px" runat="server" Text="CITO" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                                                    <label class="lblNormal">
                                                                        <%=GetLabel("Keterangan")%></label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtRemarks" Width="375px" runat="server" TextMode="MultiLine" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td valign="top">
                                                        <table style="width: 100%; display: none">
                                                            <tr>
                                                                <td class="tdLabel">
                                                                    <label class="lblMandatory">
                                                                        <%=GetLabel("Perform Date")%></label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtPerformDate" Width="120px" CssClass="datepicker" runat="server" />
                                                                </td>
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
                                    <dxcp:ASPxCallbackPanel ID="cbpViewCtl" runat="server" Width="100%" ClientInstanceName="cbpViewCtl"
                                        ShowLoadingPanel="false" OnCallback="cbpViewCtl_Callback">
                                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewCtlEndCallback(s,e); }" />
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent1" runat="server">
                                                <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                                    position: relative; font-size: 0.95em;">
                                                    <asp:GridView ID="grdView" runat="server" CssClass="grdService grdNormal notAllowSelect"
                                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                        <Columns>
                                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                            <asp:TemplateField ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <table cellpadding="0" cellspacing="0">
                                                                        <tr>
                                                                            <td>
                                                                                <img class="imgEdit <%# IsEditable.ToString() == "False" ? "imgDisabled" : "imgLink"%>"
                                                                                    title='<%=GetLabel("Edit")%>' src='<%# IsEditable.ToString() == "False" ? ResolveUrl("~/Libs/Images/Button/edit_disabled.png") : ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                    alt="" />
                                                                            </td>
                                                                            <td style="width: 1px">
                                                                                &nbsp;
                                                                            </td>
                                                                            <td>
                                                                                <img class="imgDelete <%# IsEditable.ToString() == "False" ? "imgDisabled" : "imgLink"%>"
                                                                                    title='<%=GetLabel("Delete")%>' src='<%# IsEditable.ToString() == "False" ? ResolveUrl("~/Libs/Images/Button/delete_disabled.png") : ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                    alt="" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                                    <input type="hidden" value="<%#:Eval("ItemID") %>" bindingfield="ItemID" />
                                                                    <input type="hidden" value="<%#:Eval("ItemCode") %>" bindingfield="ItemCode" />
                                                                    <input type="hidden" value="<%#:Eval("ItemName1") %>" bindingfield="ItemName1" />
                                                                    <input type="hidden" value="<%#:Eval("DiagnoseID") %>" bindingfield="DiagnoseID" />
                                                                    <input type="hidden" value="<%#:Eval("DiagnoseName") %>" bindingfield="DiagnoseName" />
                                                                    <input type="hidden" value="<%#:Eval("GCToBePerformed") %>" bindingfield="GCToBePerformed" />
                                                                    <input type="hidden" value="<%#:Eval("IsCito") %>" bindingfield="IsCito" />
                                                                    <input type="hidden" value="<%#:Eval("PerformedDateInDatePickerFormat") %>" bindingfield="PerformedDate" />
                                                                    <input type="hidden" value="<%#:Eval("Remarks") %>" bindingfield="Remarks" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="ItemName1" HeaderText="Pelayanan" HeaderStyle-Width="300px"
                                                                HeaderStyle-HorizontalAlign="Left" />
                                                            <asp:BoundField DataField="DiagnoseName" HeaderText="Diagnosa" HeaderStyle-Width="400px"
                                                                HeaderStyle-HorizontalAlign="Left" />
                                                            <asp:BoundField DataField="Remarks" HeaderText="Remarks" HeaderStyle-HorizontalAlign="Left" />
                                                        </Columns>
                                                        <EmptyDataTemplate>
                                                            <%=GetLabel("No Data To Display")%>
                                                        </EmptyDataTemplate>
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </dx:PanelContent>
                                        </PanelCollection>
                                    </dxcp:ASPxCallbackPanel>
                                    <div class="imgLoadingGrdView" id="containerImgLoadingView">
                                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                    </div>
                                    <div style="width: 100%; text-align: center">
                                        <span class="lblLink" id="lblAddData" <%=IsEditable.ToString() == "False" ? "style='display:none'" : "" %>>
                                            <%= GetLabel("Add Data")%></span>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </fieldset>
            </dx:PanelContent>
        </PanelCollection>
    </dxcp:ASPxCallbackPanel>
</div>
