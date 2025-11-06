<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DraftServiceOrderEmergencyEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.DraftServiceOrderEmergencyEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_draftserviceorderemergencyctl">
    setDatePicker('<%=txtDraftServiceOrderEmergencyDate.ClientID %>');

    window.onCbpViewCtlEndCallback = function (s, e) {
        hideLoadingPanel();
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail') {
                showToast('Save Failed', 'Error Message : ' + param[2]);
            }
            else {
                $('#containerEntry').hide();
                $('#<%=hdnDraftServiceOrderEmergencyID.ClientID %>').val(s.cpServiceOrderID);
                cbpMainPopup.PerformCallback('loadaftersave');
            }
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail') {
                showToast('Delete Failed', 'Error Message : ' + param[2]);
            }
            else {
                $('#containerEntry').hide();
                $('#<%=hdnDraftServiceOrderEmergencyID.ClientID %>').val(s.cpTestOrderID);
                cbpMainPopup.PerformCallback('loadaftersave');
            }
        }
    }

    //#region Transaction No
    $('#lblDraftServiceOrderEmergencyNo.lblLink').live('click', function () {
        var filterExpression = "AppointmentID = '" + $('#<%=hdnAppointmentID.ClientID %>').val() + "'";
        openSearchDialog('draftserviceorderhd', filterExpression, function (value) {
            $('#<%=txtDraftServiceOrderEmergencyNo.ClientID %>').val(value);
            ontxtDraftServiceOrderEmergencyNoChanged(value);
        });
    });

    $('#<%=txtDraftServiceOrderEmergencyNo.ClientID %>').change(function () {
        ontxtDraftServiceOrderEmergencyNoChanged($(this).val());
    });

    function ontxtDraftServiceOrderEmergencyNoChanged(value) {
        var filterExpression = "DraftServiceOrderNo = '" + value + "'";
        Methods.getObject('GetvDraftServiceOrderHdList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnDraftServiceOrderEmergencyID.ClientID %>').val(result.DraftServiceOrderID);
                cbpMainPopup.PerformCallback('load');
            }
            else {
                $('#<%=hdnDraftServiceOrderEmergencyID.ClientID %>').val('');
            }
        });
    }
    //#endregion

    $('#lblAddData').live('click', function (evt) {
        if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup')) {
            $('#<%=lblPhysician.ClientID %>').attr('class', 'lblDisabled');

            $('#<%=txtPhysicianCode.ClientID %>').attr('readonly', 'readonly');

            $('#<%=hdnEntryID.ClientID %>').val('');
            $('#<%=hdnItemID.ClientID %>').val('');
            $('#<%=txtItemCode.ClientID %>').val('');
            $('#<%=txtItemName.ClientID %>').val('');
            $('#containerEntry').show();
        }
    });

    //#region Physician
    $('#<%=lblPhysician.ClientID %>.lblLink').live('click', function () {
        var serviceUnitID = $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val();
        var filterExpression = "ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = " + serviceUnitID + ") AND IsDeleted = 0";
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

    //#region Item
    function getItemMasterFilterExpression() {
        var testOrderID = $('#<%=hdnDraftServiceOrderEmergencyID.ClientID %>').val();
        var filterExpression = '';
        if (testOrderID != '') {
            if ($('#<%=hdnHealthcareServiceUnitID.ClientID %>').val() == $('#<%=hdnLaboratoryServiceUnitID.ClientID %>').val() || $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val() == $('#<%=hdnImagingServiceUnitID.ClientID %>').val())
                filterExpression = 'ItemID IN (SELECT ItemID FROM vServiceUnitItem WHERE HealthcareServiceUnitID = ' + $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val() + ' AND IsTestItem = 1) AND ItemID NOT IN (SELECT ItemID FROM TestOrderDt WHERE TestOrderID = ' + $('#<%=hdnDraftServiceOrderEmergencyID.ClientID %>').val() + ' AND IsDeleted = 0) AND IsDeleted = 0';
            else filterExpression = 'ItemID IN (SELECT ItemID FROM vServiceUnitItem WHERE HealthcareServiceUnitID = ' + $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val() + ') AND ItemID NOT IN (SELECT ItemID FROM TestOrderDt WHERE TestOrderID = ' + $('#<%=hdnDraftServiceOrderEmergencyID.ClientID %>').val() + ' AND IsDeleted = 0) AND IsDeleted = 0';
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

    $('#<%=grdView.ClientID %> .imgEdit.imgLink').live('click', function () {
        $row = $(this).closest('tr').parent().closest('tr');
        var entity = rowToObject($row);

        $('#<%=hdnEntryID.ClientID %>').val(entity.ID);
        $('#<%=hdnItemID.ClientID %>').val(entity.ItemID);
        $('#<%=txtItemCode.ClientID %>').val(entity.ItemCode);
        $('#<%=txtItemName.ClientID %>').val(entity.ItemName1);

        $('#<%=txtRemarks.ClientID %>').val(entity.Remarks);
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
                $('#<%=hdnDraftServiceOrderEmergencyID.ClientID %>').val(s.cpServiceOrderID);
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
                $('#<%=hdnDraftServiceOrderEmergencyID.ClientID %>').val(s.cpTestOrderID);
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
                $('#<%=hdnDraftServiceOrderEmergencyID.ClientID %>').val(s.cpTestOrderID);
                cbpMainPopup.PerformCallback('loadaftersave');
            }
        }

        hideLoadingPanel();
        var isAdd = $('#<%=hdnIsAdd.ClientID %>').val();
        var isEditable = $('#<%=hdnIsEditable.ClientID %>').val();
        SetCustomVisibilityControl(isAdd, isEditable);
    }


    function onAfterSaveAddRecordEntryPopup(param) {
        $('#containerEntry').hide();
        $('#<%=hdnDraftServiceOrderEmergencyID.ClientID %>').val(param);
        cbpMainPopup.PerformCallback('loadaftersave');
    }

    function onAfterProposedRecordEntryPopup(param) {
        $('#containerEntry').hide();
        $('#<%=hdnDraftServiceOrderEmergencyID.ClientID %>').val(param);
        cbpMainPopup.PerformCallback('loadaftersave');
    }

    function onAfterVoidRecordEntryPopup(param) {
        $('#containerEntry').hide();
        $('#<%=hdnDraftServiceOrderEmergencyID.ClientID %>').val(param);
        cbpMainPopup.PerformCallback('loadaftersave');
    }    
</script>
<div style="height: 400px; overflow-y: scroll;">
    <dxcp:ASPxCallbackPanel ID="cbpMainPopup" runat="server" Width="100%" ClientInstanceName="cbpMainPopup"
        ShowLoadingPanel="false" OnCallback="cbpMainPopup_Callback">
        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpMainPopupEndCallback(s,e); }" />
        <PanelCollection>
            <dx:PanelContent ID="PanelContent2">
                <fieldset id="fsEntryPopup" style="margin: 0">
                    <asp:Panel runat="server" ID="Panel1" Style="width: 100%; margin-left: auto; margin-right: auto;
                        position: relative; font-size: 0.95em;">
                        <input type="hidden" value="" id="hdnAppointmentID" runat="server" />
                        <input type="hidden" value="" id="hdnDraftServiceOrderEmergencyID" runat="server" />
                        <input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />
                        <input type="hidden" value="" id="hdnImagingServiceUnitID" runat="server" />
                        <input type="hidden" value="" id="hdnLaboratoryServiceUnitID" runat="server" />
                        <input type="hidden" value="" id="hdnParamedicID" runat="server" />
                        <input type="hidden" value="" id="hdnIsAdd" runat="server" />
                        <input type="hidden" value="" id="hdnIsEditable" runat="server" />
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
                                                <label class="lblLink" id="lblDraftServiceOrderEmergencyNo">
                                                    <%=GetLabel("No. Draft Order")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDraftServiceOrderEmergencyNo" Width="232px" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblNormal" id="lblReferenceNo">
                                                    <%=GetLabel("No Referensi")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtReferenceNo" Width="232px" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <%=GetLabel("Tanggal") %>
                                                -
                                                <%=GetLabel("Jam") %>
                                            </td>
                                            <td>
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td style="padding-right: 1px; width: 145px">
                                                            <asp:TextBox ID="txtDraftServiceOrderEmergencyDate" Width="120px" CssClass="datepicker" runat="server" />
                                                        </td>
                                                        <td style="width: 5px">
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtDraftServiceOrderEmergencyTime" Width="80px" CssClass="time" runat="server"
                                                                Style="text-align: center" />
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
                                                <label class="lblMandatory">
                                                    <%=GetLabel("Dari Dokter")%></label>
                                            </td>
                                            <td>
                                                <dxe:ASPxComboBox runat="server" ID="cboFromParamedic" ClientInstanceName="cboFromParamedic"
                                                    Width="100%" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblLink lblMandatory" runat="server" id="lblPhysician">
                                                    <%=GetLabel("Dokter / Paramedis")%></label>
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
                                                                    <input type="hidden" value="<%#:Eval("Remarks") %>" bindingfield="Remarks" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="ItemName1" HeaderText="Pelayanan" HeaderStyle-Width="300px"
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
                                    <div id="divAddDataEmergencyDraftOrder" style="display: none; width: 100%; text-align: center;" runat="server">
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
