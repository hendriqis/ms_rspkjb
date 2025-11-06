<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddMedicationReconciliationCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.AddMedicationReconciliationCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
    <%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" 
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>

<script type="text/javascript" id="dxss_patiententryctl">
    $(function () {
        setDatePicker('<%=txtLogDate.ClientID %>');
        $('#<%=txtLogDate.ClientID %>').datepicker('option', 'maxDate', '0');

        setDatePicker('<%=txtStartDate.ClientID %>');
        $('#<%=txtStartDate.ClientID %>').datepicker('option', 'maxDate', '0');

        setDatePicker('<%=txtLastTakenDate.ClientID %>');
        $('#<%=txtLastTakenDate.ClientID %>').datepicker('option', 'maxDate', '0');

        calculateDuration();

        $('#<%=txtLogDate.ClientID %>').focus();
    });

    $('#<%=txtStartDate.ClientID %>').die('change');
    $('#<%=txtStartDate.ClientID %>').live('change', function (evt) {
        calculateDuration();
    });

    $('#<%=txtLastTakenDate.ClientID %>').die('change');
    $('#<%=txtLastTakenDate.ClientID %>').live('change', function (evt) {
        calculateDuration();
    });

    function calculateDuration() {
        var startDate = $('#<%=txtStartDate.ClientID %>').val();
        var lastDate = $('#<%=txtLastTakenDate.ClientID %>').val();

        var from = startDate.split("-");
        var f = new Date(from[2], from[1] - 1, from[0]);

        var to = lastDate.split("-");
        var t = new Date(to[2], to[1] - 1, to[0]);

        var duration = (t.getTime() - f.getTime()) / (1000 * 3600 * 24);
    }

    function onLedDrugNameLostFocus(led) {
        var drugID = led.GetValueText();
        if (drugID != '' || drugID != null) {
            $('#<%=hdnDrugID.ClientID %>').val(drugID);
            var itemName = led.GetDisplayText();
            $('#<%=hdnDrugName.ClientID %>').val(itemName);
            $('#<%=txtItemName.ClientID %>').val(itemName);
            if (drugID == '' || drugID == null) {
                $('#<%=txtDosingDose.ClientID %>').val('1');
            }
            else {
                var filterExpression = "ItemID = " + drugID;
                Methods.getObject('GetvDrugInfoList', filterExpression, function (result) {
                    $('#<%=hdnGenericName.ClientID %>').val(result.GenericName);
                    $('#<%=txtDosingDose.ClientID %>').val('1');
                    cboDosingUnit.SetValue(result.GCItemUnit);
                    cboDrugForm.SetValue(result.GCDrugForm);
                    $('#<%=txtDosingDose.ClientID %>').focus();
                });
            }
        }
    }

    $('#<%=chkIsMasterItem.ClientID %>').die('change');
    $('#<%=chkIsMasterItem.ClientID %>').live('change', function () {
        var isChecked = $(this).is(":checked");

        if ($(this).is(':checked')) {
            var drugID = $('#<%=hdnDrugID.ClientID %>').val();
            $('#<%=trExternalItemInfo.ClientID %>').attr("style", "display:none");
            $('#<%=trItemInfo.ClientID %>').attr("style", "display:table-row");
            if (drugID != '' || drugID != null) {
                var filterExpression = "ItemID = " + drugID;
                Methods.getObject('GetvDrugInfoList', filterExpression, function (result) {
                    cboDrugForm.SetValue(result.GCDrugForm);
                    cboDrugForm.SetEnabled(false);
                });
            }
            else {
                cboDrugForm.SetValue('');
                cboDrugForm.SetEnabled(false);
            }
        }
        else {
            $('#<%=trExternalItemInfo.ClientID %>').attr("style", "display:table-row");
            $('#<%=trItemInfo.ClientID %>').attr("style", "display:none");
            cboDrugForm.SetValue('X122^999');
            cboDrugForm.SetEnabled(true);
        }
    });

    $('#<%=chkIsContinueInpatientMedication.ClientID %>').die('change');
    $('#<%=chkIsContinueInpatientMedication.ClientID %>').live('change', function () {
        var isChecked = $(this).is(":checked");

        if ($(this).is(':checked')) {
            $('#<%=txtDosingDuration.ClientID %>').removeAttr("readonly");
            $('#<%=trMedicationTime.ClientID %>').attr("style", "display:table-row");
        }
        else {
            $('#<%=txtDosingDuration.ClientID %>').attr("readonly", "readonly");
            $('#<%=trMedicationTime.ClientID %>').attr("style", "display:none");
        }
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
                displayErrorMessageBox("Rekonsiliasi Obat", "Error Message : <br/><span style='color:red'>" + param[2] + "</span>");
            }
            else {
                pcRightPanelContent.Hide();
                if (typeof onRefreshList == 'function')
                    onRefreshList();
            }
        }
    }

    function SetMedicationDefaultTime(frequency) {
        if (frequency == '' || frequency == null) {
        }
        else {
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

    function onAfterSaveAddRecordEntryPopup(result) {
        if (typeof onRefreshControl == 'function')
            onRefreshControl();
    }

    function onAfterSaveEditRecordEntryPopup(result) {
        if (typeof onRefreshControl == 'function')
            onRefreshControl();
    }
</script>
<input type="hidden" runat="server" id="hdnPopupID" value="" />
<input type="hidden" runat="server" id="hdnSelectedItem" value="" />
<input type="hidden" runat="server" id="hdnSelectedScheduleID" value="" />
<input type="hidden" runat="server" id="hdnPrescriptionOrderDetailID" value="" />
<input type="hidden" runat="server" id="hdnOldFrequency" value="" />
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
                        <%=GetLabel("Tanggal Rekonsiliasi")%></label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtLogDate" runat="server" Width="120px" CssClass="datepicker" />
                </td>
                <td colspan="3">
                    <asp:CheckBox ID="chkIsMasterItem" runat="server" Text=" Obat internal Rumah Sakit" />                        
                </td>
            </tr>
            <tr id="trItemInfo" runat="server">
                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Nama Obat")%></label></td>
                <td colspan="6">
                    <input type="hidden" value="" id="hdnDrugID" runat="server" />
                    <input type="hidden" value="" id="hdnDrugName" runat="server" />
                    <input type="hidden" value="" id="hdnGenericName" runat="server" />
                    <qis:QISSearchTextBox ID="ledItem" ClientInstanceName="ledItem" runat="server" Width="100%"
                        ValueText="ItemID" FilterExpression="GCItemType = 'X001^002' AND IsDeleted = 0 AND ISNULL(GCItemStatus,'') != 'X181^999'" DisplayText="ItemName1"
                MethodName="GetvDrugInfoList" >
                        <ClientSideEvents ValueChanged="function(s){ onLedDrugNameLostFocus(s); }" />
                        <Columns>
                            <qis:QISSearchTextBoxColumn Caption="Nama item" FieldName="ItemName" Description="i.e. Panadol"
                                Width="400px" />
                        </Columns>
                    </qis:QISSearchTextBox>
                </td>
            </tr>
            <tr id="trExternalItemInfo" runat="server" >
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Nama Obat")%></label>
                </td>
                <td colspan="6">
                    <asp:TextBox ID="txtItemName" runat="server" Width="100%"  />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory"><%=GetLabel("Bentuk Obat")%></label>
                </td>
                <td colspan="2">
                    <dxe:ASPxComboBox runat="server" ID="cboDrugForm" ClientInstanceName="cboDrugForm" Width="100%" ClientEnabled="false"  />
                </td>
                <td colspan="4">                    
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Mulai diberikan")%></label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtStartDate" runat="server" Width="120px" CssClass="datepicker" />
                </td>
                <td colspan="2">
                    <asp:CheckBox ID="chkIsAsRequired" runat="server" Text=" Jika diperlukan" Checked="false" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Aturan Pakai")%></label>
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
                <td class="tdLabel">
                    <label class="lblNormal"><%=GetLabel("AC/DC/PC")%></label>
                </td>
                <td>
                    <dxe:ASPxComboBox runat="server" ID="cboCoenamRule" ClientInstanceName="cboCoenamRule"
                        Width="100%" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory"><%=GetLabel("Rute")%></label>
                </td>
                <td colspan="2">
                    <dxe:ASPxComboBox runat="server" ID="cboMedicationRoute" ClientInstanceName="cboMedicationRoute" Width="100%"  />
                </td>
                <td colspan="4">
                    <asp:CheckBox ID="chkIsExternalMedication" runat="server" Text=" Obat Luar" Checked="false" />
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
                <td class="tdLabel" style="vertical-align:top">
                    <label class="lblNormal"><%=GetLabel("Instruksi Khusus Pemberian")%></label>
                </td>
                <td colspan="6">
                    <asp:TextBox ID="txtMedicationAdministration" Width="100%" runat="server" TextMode="MultiLine" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel" style="vertical-align:top">
                    <label class="lblNormal"><%=GetLabel("Tujuan pengobatan")%></label>
                </td>
                <td colspan="6">
                    <asp:TextBox ID="txtMedicationPurpose" Width="100%" runat="server" TextMode="MultiLine" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Pemberian Terakhir")%></label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtLastTakenDate" runat="server" Width="120px" CssClass="datepicker" />
                </td>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Jam")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtLastTakenTime" runat="server" Width="60px" CssClass="time" />
                </td>
            </tr>
            <tr>
                <td />
                <td colspan="4">
                    <asp:CheckBox ID="chkIsContinueInpatientMedication" runat="server" Text=" Dilanjutkan di Rawat Inap"/>
                </td>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Durasi")%></label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtDosingDuration" Width="100%" ReadOnly="true" />
                </td>
            </tr>
            <tr id="trMedicationTime" runat="server" style="display:none">
                <td class="tdLabel">
                    <label class="lblNormal"><%=GetLabel("Waktu Pemberian / Sequence")%></label>
                </td>
                <td colspan="5">
                <fieldset id="fsAEM" style="margin: 0">
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
                    </fieldset>
                </td>
            </tr>
            <tr>
                <td class="tdLabel" style="vertical-align:top">
                    <label class="lblNormal"><%=GetLabel("Catatan Tambahan")%></label>
                </td>
                <td colspan="6">
                    <asp:TextBox ID="txtRemarks" Width="100%" runat="server" TextMode="MultiLine" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal"><%=GetLabel("Penyimpanan Obat")%></label>
                </td>
                <td colspan="6">
                    <dxe:ASPxComboBox runat="server" ID="cboMedicationStorage" ClientInstanceName="cboMedicationStorage" Width="100%"  />
                </td>
            </tr>
        </table>
    </div>
</div>
