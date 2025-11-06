<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddExternalMedicationCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.AddExternalMedicationCtl" %>
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

        $('#<%=txtItemName.ClientID %>').focus();
    });


    $('#btnMPEntryProcess').click(function () {
        var message = "Add external medication <b>" + $('#<%:txtItemName.ClientID %>').val() + "</b> to medication schedule ?";
        showToastConfirmation(message, function (result) {
            if (IsValid(result, 'fsAEM', 'mpAEM'))
                cbpPopupProcess.PerformCallback('process');
            else {
                return false;
            }
        });
    });

    function onCbpPopupProcesEndCallback(s) {
        hideLoadingPanel();
        var param = s.cpResult.split('|');
        var retval = s.cpRetval.split('|');
        if (param[0] == 'process') {
            if (param[1] == '0') {
                showToast("Process Failed", "Error Message : <br/><span style='color:red'>" + param[2] + "</span>");
            }
            else {
                pcRightPanelContent.Hide();
                if (typeof onRefreshList == 'function')
                    onRefreshList();
            }
        }
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
                $('#<%=txtStartTime1.ClientID %>').val('');
                $('#<%=txtStartTime2.ClientID %>').val('');
                $('#<%=txtStartTime3.ClientID %>').val('');
                $('#<%=txtStartTime4.ClientID %>').val('');
                $('#<%=txtStartTime5.ClientID %>').val('');
                $('#<%=txtStartTime6.ClientID %>').val('');
            }
        });
    }

    $('#<%=txtFrequencyNumber.ClientID %>').live('change', function () {
        SetMedicationDefaultTime($('#<%=txtFrequencyNumber.ClientID %>').val());
    });

    function onCbpViewEndCallback(s) {
        hideLoadingPanel();
    }

    function onRefreshGrid() {
        cbpView.PerformCallback('refresh');
    }
</script>
<input type="hidden" runat="server" id="hdnSelectedID" value="" />
<input type="hidden" runat="server" id="hdnSelectedItem" value="" />
<input type="hidden" runat="server" id="hdnSelectedScheduleID" value="" />
<input type="hidden" runat="server" id="hdnPrescriptionOrderDetailID" value="" />
<input type="hidden" runat="server" id="hdnOldFrequency" value="" />
<div>
    <div>
        <table style="width: 100%">
            <colgroup>
                <col style="width: 150px" />
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
                        <%=GetLabel("Drug Name")%></label>
                </td>
                <td colspan="6">
                    <asp:TextBox ID="txtItemName" runat="server" Width="100%"  />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Start Date")%></label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtStartDate" runat="server" Width="120px" CssClass="datepicker" />
                </td>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Duration")%></label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtDosingDuration" Width="100%" CssClass="number" />
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
                        runat="server" Width="100%"  />
                </td>
                <td>
                    <asp:TextBox ID="txtDosingDose" runat="server" Width="100%" CssClass="number"  />
                </td>
                <td>
                    <dxe:ASPxComboBox ID="cboDosingUnit" ClientInstanceName="cboDosingUnit" runat="server"
                        Width="100%" />
                </td>
                <td colspan="2">
                    <asp:CheckBox ID="chkIsAsRequired" runat="server" Text="As Required" Checked="false" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory"><%=GetLabel("Medication Route")%></label>
                </td>
                <td colspan="2">
                    <dxe:ASPxComboBox runat="server" ID="cboMedicationRoute" ClientInstanceName="cboMedicationRoute" Width="100%"  />
                </td>
                <td colspan="2" class="tdLabel">
                    <label class="lblNormal"><%=GetLabel("AC/DC/PC")%></label>
                </td>
                <td>
                    <dxe:ASPxComboBox runat="server" ID="cboCoenamRule" ClientInstanceName="cboCoenamRule"
                        Width="100%" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal"><%=GetLabel("Taken Time")%></label>
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
                    <label class="lblNormal"><%=GetLabel("Start Time / Sequence")%></label>
                </td>
                <td colspan="5">
                <fieldset id="fsAEM" style="margin: 0">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td style="width: 15%">
                                <asp:TextBox runat="server" ID="txtStartTime1" Width="100%" Text="00:00" />
                            </td>
                            <td style="width: 15%">
                                <asp:TextBox runat="server" ID="txtStartTime2" Width="100%" Text="00:00" />
                            </td>
                            <td style="width: 15%">
                                <asp:TextBox runat="server" ID="txtStartTime3" Width="100%" Text="00:00" />
                            </td>
                            <td style="width: 15%">
                                <asp:TextBox runat="server" ID="txtStartTime4" Width="100%" Text="00:00"/>
                            </td>
                            <td style="width: 15%">
                                <asp:TextBox runat="server" ID="txtStartTime5" Width="100%" Text="00:00"/>
                            </td>
                            <td style="width: 15%">
                                <asp:TextBox runat="server" ID="txtStartTime6" Width="100%" Text="00:00"/>
                            </td>
                        </tr>
                    </table>
                    </fieldset>
                </td>
            </tr>
            <tr>
                <td class="tdLabel" style="vertical-align:top">
                    <label class="lblNormal"><%=GetLabel("Medication Administration")%></label>
                </td>
                <td colspan="6">
                    <asp:TextBox ID="txtMedicationAdministration" Width="100%" runat="server" TextMode="MultiLine" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel" style="vertical-align:top">
                    <label class="lblNormal"><%=GetLabel("Medication Purpose")%></label>
                </td>
                <td colspan="6">
                    <asp:TextBox ID="txtMedicationPurpose" Width="100%" runat="server" TextMode="MultiLine" />
                </td>
            </tr>
            <tr>
                <td />
                <td colspan="6">
                    <asp:CheckBox ID="chkIsExternalMedication" runat="server" Text="Obat Luar" Checked="false" />
                </td>
            </tr>
        </table>
    </div>
    <dxcp:ASPxCallbackPanel ID="cbpPopupProcess" runat="server" Width="100%" ClientInstanceName="cbpPopupProcess"
        ShowLoadingPanel="false" OnCallback="cbpPopupProcess_Callback">
        <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpPopupProcesEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</div>
