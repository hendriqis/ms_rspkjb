<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditMedicationScheduleCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.EditMedicationScheduleCtl" %>
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
    });


    $('#btnMPEntryProcess').click(function () {
        var message = "Save the changes for <b>" + $('#<%:txtItemName.ClientID %>').val() + "</b> ?";
        displayConfirmationMessageBox("Edit Waktu Pemberian",message, function (result) {
            if (result) {
                if (validateTime($('#<%=txtMedicationTime.ClientID %>').val())) {
                    cbpPopupProcess.PerformCallback('process');
                }
                else {
                    showToast('Warning', 'Format Waktu yang diinput salah');
                }
            }
        });
    });

    function validateTime(timeValue) {
        var result = true;
        if (timeValue == "" || timeValue.indexOf(":") < 0 || timeValue.length != 5) {
            result = false;
        }
        else {
            var sHours = timeValue.split(':')[0];
            var sMinutes = timeValue.split(':')[1];

            if (sHours == "" || isNaN(sHours) || parseInt(sHours) > 23) {
                result = false;
            }
            else if (parseInt(sHours) == 0)
                sHours = "00";
            else if (sHours < 10)
                sHours = "0" + sHours;

            if (sMinutes == "" || isNaN(sMinutes) || parseInt(sMinutes) > 59) {
                result = false;
            }
            else if (parseInt(sMinutes) == 0)
                sMinutes = "00";
            else if (sMinutes < 10)
                sMinutes = "0" + sMinutes;
        }
        return result;
    }

    function onCbpPopupProcesEndCallback(s) {
        hideLoadingPanel();
        var param = s.cpResult.split('|');
        var retval = s.cpRetval.split('|');
        if (param[0] == 'process') {
            if (param[1] == '0') {
                displayErrorMessageBox("Edit Waktu Pemberian","<span style='color:red'>" + param[2] + "</span>");
            }
            else {
                pcRightPanelContent.Hide();
                if (typeof onRefreshDetailList == 'function')
                    onRefreshDetailList();
                else
                    if (typeof onRefreshControl == 'function')
                        onRefreshControl();                    
            }
        }
    }

    function onCbpViewEndCallback(s) {
        hideLoadingPanel();
    }

    function onRefreshGrid() {
        cbpView.PerformCallback('refresh');
    }
</script>
<input type="hidden" runat="server" id="hdnSelectedID" value="" />
<input type="hidden" runat="server" id="hdnSelectedScheduleID" value="" />
<input type="hidden" runat="server" id="hdnSelectedItem" value="" />
<input type="hidden" runat="server" id="hdnMedicationDate" value="" />
<input type="hidden" runat="server" id="hdnDispensaryServiceUnitID" value="" />
<input type="hidden" runat="server" id="hdnPrescriptionOrderDtID" value="" />
<input type="hidden" runat="server" id="hdnPastMedicationID" value="" />
<div>
    <div>
        <table width="100%">
            <colgroup>
                <col width="115px" />
                <col width="120px" />
                <col width="115px" />
                <col />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Tanggal")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtMedicationDate" runat="server" Width="120px" ReadOnly="true" CssClass="datepicker" />
                </td>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Sequence No.")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtSequenceNo" runat="server" Width="40px" ReadOnly="true" CssClass="number" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Item")%></label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtItemName" runat="server" Width="100%" ReadOnly="true"/>
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Waktu Pemberian")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtMedicationTime" runat="server" Width="60px" Text="00:00" CssClass="time" />
                </td>
                <td class="tdLabel">
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td class="tdLabel" style="vertical-align:top">
                    <label class="lblNormal">
                        <%=GetLabel("Alasan Perubahan")%></label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtRemarks" runat="server" Width="100%" TextMode=MultiLine Height="50px"/>
                </td>
            </tr>
            <tr>
                <td />
                <td colspan="3">
                    <asp:CheckBox ID="chkIsApplyToAllSchedule" runat="server" Text=" Berlaku untuk hari-hari berikutnya" Enabled="True" /> 
                </td>
            </tr>
        </table>
    </div>
    <dxcp:ASPxCallbackPanel ID="cbpPopupProcess" runat="server" Width="100%" ClientInstanceName="cbpPopupProcess"
        ShowLoadingPanel="false" OnCallback="cbpPopupProcess_Callback">
        <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpPopupProcesEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</div>
