<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PatientManagementTransactionMedicationOrderCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientManagementTransactionMedicationOrderCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_patientmanagementmedicationorderctl">
    setDatePicker('<%=txtOrderDateCtl.ClientID %>');
    $('#<%=txtOrderDateCtl.ClientID %>').datepicker('option', 'maxDate', '0');

    window.onCbpViewCtlEndCallback = function (s, e) {
        hideLoadingPanel();
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else {
                $('#containerEntry').hide();
                $('#<%=hdnOrderIDCtl.ClientID %>').val(s.cpOrderID);
                cbpMainPopup.PerformCallback('loadaftersave');
            }
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail')
                showToast('Delete Failed', 'Error Message : ' + param[2]);
        }
    }

    $('#btnSaveHeader').click(function (evt) {
        if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup'))
            cbpMainPopup.PerformCallback('save');
    });

    $('#btnProposedHeader').live('click', function () {
        if ($('#<%=hdnOrderIDCtl.ClientID %>').val() != "" && $('#<%=hdnOrderIDCtl.ClientID %>').val() != "0")
            cbpMainPopup.PerformCallback('proposed');
    });

    $('#btnNewHeader').live('click', function () {
        cbpMainPopup.PerformCallback('new');
    });

    //#region Transaction No
    $('#lblOrderNoCtl.lblLink').live('click', function () {
        var filterExpression = "VisitID = '" + $('#<%=hdnVisitID.ClientID %>').val() + "'";
        openSearchDialog('prescriptionorderhd', filterExpression, function (value) {
            $('#<%=txtOrderNoCtl.ClientID %>').val(value);
            onTxtOrderNoCtlChanged(value);
        });
    });

    $('#<%=txtOrderNoCtl.ClientID %>').change(function () {
        onTxtOrderNoCtlChanged($(this).val());
    });

    function onTxtOrderNoCtlChanged(value) {
        cbpMainPopup.PerformCallback('load');
    }
    //#endregion

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

    $('#btnSave').live('click', function (evt) {
        if (IsValid(evt, 'fsTrxPopup', 'mpTrxPopup')) {
            cbpViewCtl.PerformCallback('save');
        }
    });

    window.onCbpMainPopupEndCallback = function (s, e) {
        var result = s.cpResult.split('|');
        if (result[0] == 'save') {
            if (result[1] != 'success') {
                if (result[2] != '')
                    showToast('Save Failed', 'Error Message : ' + result[2]);
                else
                    showToast('Save Failed', '');
            }
        }
        hideLoadingPanel();
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
                        <input type="button" id="btnNewHeader" value='<%= GetLabel("New")%>' />
                        <input type="button" id="btnSaveHeader" value='<%= GetLabel("Save")%>' <%= IsEditable == true ? "" : "style='display:none'" %> />
                        <input type="button" id="btnProposedHeader" value='<%= GetLabel("Proposed")%>' <%= IsEditable == true ? "" : "style='display:none'" %> />
                        <input type="hidden" value="" id="hdnOrderIDCtl" runat="server" />
                        <input type="hidden" value="" id="hdnImagingServiceUnitID" runat="server" />
                        <input type="hidden" value="" id="hdnLaboratoryServiceUnitID" runat="server" />
                        <input type="hidden" value="" id="hdnVisitID" runat="server" />
                        <input type="hidden" value="" id="hdnCurrentHealthcareServiceUnitID" runat="server" />
                        <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
                        <input type="hidden" value="" id="hdnClassID" runat="server" />
                        <input type="hidden" value="" id="hdnDefaultDispensaryServiceUnitID" runat="server" />
                        <input type="hidden" value="" id="hdnIPAddress" runat="server" />
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
                                                <label class="lblLink" id="lblOrderNoCtl">
                                                    <%=GetLabel("No. Order")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtOrderNoCtl" Width="232px" runat="server" />
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
                                                            <asp:TextBox ID="txtOrderDateCtl" Width="120px" CssClass="datepicker" runat="server" />
                                                        </td>
                                                        <td style="width: 5px">
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtOrderTimeCtl" Width="80px" CssClass="time" runat="server" Style="text-align: center" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr style="display: none">
                                            <td class="tdLabel">
                                                <label class="lblNormal">
                                                    <%=GetLabel("Location")%></label>
                                            </td>
                                            <td>
                                                <dxe:ASPxComboBox runat="server" ID="cboLocationCtl" ClientInstanceName="cboLocationCtl"
                                                    Width="300px" OnCallback="cboLocationCtl_Callback">
                                                    <ClientSideEvents BeginCallback="function() { showLoadingPanel(); }" EndCallback="function() { hideLoadingPanel(); }" />
                                                </dxe:ASPxComboBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                                <label>
                                                    <%=GetLabel("Catatan Resep")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtNotesCtl" Width="100%" TextMode="MultiLine" Height="250px" runat="server" />
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
                                                <label class="lblMandatory">
                                                    <%=GetLabel("Instalasi Farmasi") %></label>
                                            </td>
                                            <td>
                                                <dxe:ASPxComboBox ID="cboDispensaryUnitCtl" ClientInstanceName="cboDispensaryUnitCtl"
                                                    runat="server" Width="235px">
                                                    <ClientSideEvents ValueChanged="function() { cboLocationCtl.PerformCallback(); }" />
                                                </dxe:ASPxComboBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label>
                                                    <%=GetLabel("Status")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtStatus" Width="120px" runat="server" style="text-align: center" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </fieldset>
            </dx:PanelContent>
        </PanelCollection>
    </dxcp:ASPxCallbackPanel>
</div>
