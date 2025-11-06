<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NurseEditMedicationSignaCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.NurseEditMedicationSignaCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
    <%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<div class="toolbarArea">
    <ul>
        <li id="btnMPEntryProcess">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbSave.png")%>' alt="" /><div>
                <%=GetLabel("Save")%></div>
        </li>
    </ul>
</div>
<script type="text/javascript" id="dxss_patiententryctl">
    $(function () {
        setDatePicker('<%=txtStartDate.ClientID %>');
        $('#<%=txtStartDate.ClientID %>').datepicker('option', 'minDate', '0');

        $('#lblPhysicianNoteID').removeClass('lblLink');
    });

    $('#lblPhysicianNoteID.lblLink').live('click', function () {
        var filterExpression = "VisitID = " + $('#<%=hdnVisitID.ClientID %>').val() + " AND GCPatientNoteType IN ('X011^010','X011^011','X011^012')  AND IsNeedNotification = 1";
        openSearchDialog('planningNote', filterExpression, function (value) {
            onTxtPlanningNoteChanged(value);
        });
    });

    function onTxtPlanningNoteChanged(value) {
        var filterExpression = "ID = " + value;
        Methods.getObject('GetPatientVisitNoteList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnPlanningNoteID.ClientID %>').val(result.ID);
                $('#<%=txtPatientVisitNoteText.ClientID %>').val(result.NoteText);
            }
            else {
                $('#<%=hdnPlanningNoteID.ClientID %>').val('');
                $('#<%=txtPatientVisitNoteText.ClientID %>').val('');
            }
        });
    }

    $('#btnMPEntryProcess').click(function () {
        var message = "Save the changes for <b>" + $('#<%:txtItemName.ClientID %>').val() + "</b> ?";
        displayConfirmationMessageBox("Change Signa", message, function (result) {
            if (result) cbpPopupProcess.PerformCallback('process');
        });
    });

    function onCbpPopupProcesEndCallback(s) {
        hideLoadingPanel();
        var param = s.cpResult.split('|');
        var retval = s.cpRetval.split('|');
        if (param[0] == 'process') {
            if (param[1] == '0') {
                displayErrorMessageBox("Change Signa", "Error Message : <br/><span style='color:red'>" + param[2] + "</span>");
            }
            else {
                pcRightPanelContent.Hide();
                if (typeof onRefreshList == 'function')
                    onRefreshList();
            }
        }
    }

    function onCbpViewEndCallback(s) {
        hideLoadingPanel();
    }

    function onRefreshGrid() {
        cbpView.PerformCallback('refresh');
    }

    function onCboPhysicianInstructionSourceChanged(s) {
        if (s.GetValue() != null && s.GetValue().indexOf('^01') > -1) {
            $('#lblPhysicianNoteID').addClass('lblLink');
        }
        else {
            $('#lblPhysicianNoteID').removeClass('lblLink');
            $('#<%=hdnPlanningNoteID.ClientID %>').val('');
            $('#<%=txtPatientVisitNoteText.ClientID %>').val('');

        }
    }

    function onCboPhysicianChanged(s) {
        if (s.GetValue() != null) {
            $('#<%=hdnParamedicID.ClientID %>').val(s.GetValue());
        }
        else
            $('#<%=hdnParamedicID.ClientID %>').val('');
    }

    function SetMedicationDefaultTime(frequency) {
        Methods.getMedicationSequenceTime(frequency, function (result) {
            if (result != null) {
                var medicationTimeInfo = result.split('|');
                $('#<%=txtStartTime1.ClientID %>').val(medicationTimeInfo[0]);
                $('#<%=txtStartTime2.ClientID %>').val(medicationTimeInfo[1]);
                $('#<%=txtStartTime3.ClientID %>').val(medicationTimeInfo[2]);
                $('#<%=txtStartTime4.ClientID %>').val(medicationTimeInfo[3]);
                $('#<%=txtStartTime5.ClientID %>').val(medicationTimeInfo[4]);
                $('#<%=txtStartTime6.ClientID %>').val(medicationTimeInfo[5]);
            }
            else {
                $('#<%=txtStartTime1.ClientID %>').val('-');
                $('#<%=txtStartTime2.ClientID %>').val('-');
                $('#<%=txtStartTime3.ClientID %>').val('-');
                $('#<%=txtStartTime4.ClientID %>').val('-');
                $('#<%=txtStartTime5.ClientID %>').val('-');
                $('#<%=txtStartTime6.ClientID %>').val('-');
            }
        });
    }

    $('#<%=txtFrequencyNumber.ClientID %>').live('change', function () {
        SetMedicationDefaultTime($('#<%=txtFrequencyNumber.ClientID %>').val());
    });
</script>
<input type="hidden" runat="server" id="hdnVisitID" value="" />
<input type="hidden" runat="server" id="hdnSelectedID" value="" />
<input type="hidden" runat="server" id="hdnSelectedItem" value="" />
<input type="hidden" runat="server" id="hdnSelectedScheduleID" value="" />
<input type="hidden" runat="server" id="hdnPrescriptionOrderDetailID" value="" />
<input type="hidden" runat="server" id="hdnHealthcareServiceUnitID" value="" />
<input type="hidden" runat="server" id="hdnOldFrequency" value="" />
<input type="hidden" runat="server" id="hdnPlanningNoteID" value="" />
<input type="hidden" runat="server" id="hdnParamedicID" value="" />
<div>
    <div>
        <table style="width: 100%">
            <colgroup>
                <col style="width: 200px" />
                <col width="40px" />
                <col width="60px" />
                <col width="40px" />
                <col width="60px" />
                <col width="100px" />
                <col width="60px" />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Nama Obat")%></label>
                </td>
                <td colspan="6">
                    <asp:TextBox ID="txtItemName" runat="server" Width="100%" ReadOnly="true" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Mulai Perubahan")%></label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtStartDate" runat="server" Width="120px" CssClass="datepicker" />
                </td>
                <td class="tdLabel" style="display:none">
                    <label class="lblMandatory">
                        <%=GetLabel("Durasi")%></label>
                </td>
                <td style="display:none">
                    <asp:TextBox runat="server" ID="txtDosingDuration" Width="100%" CssClass="number" Text="1" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Signa")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtFrequencyNumber" runat="server" Width="100%" CssClass="number" />
                </td>
                <td>
                    <dxe:ASPxComboBox ID="cboFrequencyTimeline" ClientInstanceName="cboFrequencyTimeline"
                        runat="server" Width="100%" Enabled="false" />
                </td>
                <td>
                    <asp:TextBox ID="txtDosingDose" runat="server" Width="100%" CssClass="number" />
                </td>
                <td>
                    <asp:TextBox ID="txtDosingUnit" runat="server" Width="100%" ReadOnly="true" />
                </td>
                <td colspan="2">
                    <asp:CheckBox ID="chkIsAsRequired" runat="server" Text=" PRN" Checked="false" Enabled="false" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory"><%=GetLabel("Rute Pemberian")%></label>
                </td>
                <td colspan="2">
                    <dxe:ASPxComboBox runat="server" ID="cboMedicationRoute" ClientInstanceName="cboMedicationRoute" Width="100%" Enabled="false" />
                </td>
                <td colspan="2" class="tdLabel">
                    <label class="lblNormal"><%=GetLabel("AC/DC/PC")%></label>
                </td>
                <td>
                    <dxe:ASPxComboBox runat="server" ID="cboCoenamRule" ClientInstanceName="cboCoenamRule"
                        Width="100%" />
                </td>
            </tr>
            <tr style="display:none">
                <td class="tdLabel">
                    <label class="lblNormal"><%=GetLabel("Waktu Pemberian")%></label>
                </td>
                <td colspan="5">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td style="width: 25%">
                                <asp:CheckBox ID="chkIsMorning" runat="server" Text="Morning" Checked="false" />
                            </td>
                            <td style="width: 25%">
                                <asp:CheckBox ID="chkIsNoon" runat="server" Text="Noon" Checked="false" />
                            </td>
                            <td style="width: 25%">
                                <asp:CheckBox ID="chkIsEvening" runat="server" Text="Evening" Checked="false" />
                            </td>
                            <td style="width: 25%">
                                <asp:CheckBox ID="chkIsNight" runat="server" Text="Night" Checked="false" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal"><%=GetLabel("Waktu Pemberian / Sequence")%></label>
                </td>
                <td colspan="5">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td style="width: 15%">
                                <asp:TextBox runat="server" ID="txtStartTime1" CssClass="time" Width="100%" Text="00:00" />
                            </td>
                            <td style="width: 15%">
                                <asp:TextBox runat="server" ID="txtStartTime2" CssClass="time" Width="100%" Text="00:00" />
                            </td>
                            <td style="width: 15%">
                                <asp:TextBox runat="server" ID="txtStartTime3" CssClass="time" Width="100%" Text="00:00" />
                            </td>
                            <td style="width: 15%">
                                <asp:TextBox runat="server" ID="txtStartTime4" CssClass="time" Width="100%" Text="00:00"/>
                            </td>
                            <td style="width: 15%">
                                <asp:TextBox runat="server" ID="txtStartTime5" CssClass="time" Width="100%" Text="00:00"/>
                            </td>
                            <td style="width: 15%">
                                <asp:TextBox runat="server" ID="txtStartTime6" CssClass="time" Width="100%" Text="00:00"/>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="tdLabel" style="vertical-align:top">
                    <label class="lblNormal"><%=GetLabel("Instruksi Khusus")%></label>
                </td>
                <td colspan="6">
                    <asp:TextBox ID="txtMedicationAdministration" Width="100%" runat="server" TextMode="MultiLine" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Cara Pemberian Instruksi")%></label>
                </td>
                <td colspan="6">
                    <dxe:ASPxComboBox ID="cboPhysicianInstructionSource" ClientInstanceName="cboPhysicianInstructionSource"
                    runat="server" Width="250px" >
                        <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboPhysicianInstructionSourceChanged(s); }" Init="function(s,e){ onCboPhysicianInstructionSourceChanged(s); }" />
                    </dxe:ASPxComboBox>
                </td>
            </tr>
            <tr>
                <td class="tdLabel" style="vertical-align:top">
                    <label class="lblLink" id="lblPhysicianNoteID">
                        <%=GetLabel("Catatan Dokter")%></label>
                </td>
                <td colspan="6">
                    <asp:TextBox ID="txtPatientVisitNoteText" Width="100%" runat="server" TextMode="MultiLine" ReadOnly="true" />
                </td>
            </tr>     
            <tr>
                <td class="tdLabel" style="vertical-align:top">
                    <label class="lblNormal"><%=GetLabel("Catatan Perawat")%></label>
                </td>
                <td colspan="6">
                    <asp:TextBox ID="txtNurseNoteText" Width="100%" runat="server" TextMode="MultiLine" />
                </td>
            </tr>
            <tr>
                <td />
                <td colspan="6">
                   <asp:CheckBox ID="chkIsNeedConfirmation" runat="server" Checked = "false" Text=" Perlu konfirmasi" /> 
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="">
                        <%=GetLabel("Dokter")%></label>
                </td>
                <td colspan="6">
                    <dxe:ASPxComboBox ID="cboPhysician" ClientInstanceName="cboPhysician" runat="server" Width="250px" >
                        <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboPhysicianChanged(s); }" Init="function(s,e){ onCboPhysicianChanged(s); }" />
                    </dxe:ASPxComboBox>
                </td>
            </tr>
            <tr>
                <td class="tdLabel" style="vertical-align:top">
                    <label class="lblNormal">
                        <%=GetLabel("Catatan Integrasi")%></label>
                </td>
                <td style="vertical-align:top" colspan="6">
                    <table border="0" cellpadding="0" cellspacing="0" style="width:100%">
                        <tr>
                            <td><asp:CheckBox ID="chkIsGenerateCPPT" runat="server" Text=" Generate CPPT" ToolTip="Generate CPPT" Checked="false" /></td>
                            <td><asp:CheckBox ID="chkIsGenerateJournal" runat="server" Text=" Generate Catatan Perawat" ToolTip="Generate Jurnal" Checked="true" /></td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <dxcp:ASPxCallbackPanel ID="cbpPopupProcess" runat="server" Width="100%" ClientInstanceName="cbpPopupProcess"
        ShowLoadingPanel="false" OnCallback="cbpPopupProcess_Callback">
        <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpPopupProcesEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</div>
