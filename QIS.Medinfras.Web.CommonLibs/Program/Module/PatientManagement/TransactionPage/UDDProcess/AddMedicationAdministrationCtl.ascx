<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddMedicationAdministrationCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.AddMedicationAdministrationCtl" %>
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
        setDatePicker('<%=txtMedicationDate.ClientID %>');
        $('#<%=txtMedicationDate.ClientID %>').datepicker('option', 'maxDate', '0');

        $('#<%=txtMedicationTime.ClientID %>').focus();
    });


    $('#btnMPEntryProcess').click(function () {
        var message = "Save the changes for <b>" + $('#<%:txtItemName.ClientID %>').val() + "</b> ?";
        showToastConfirmation(message, function (result) {
            if (result) cbpPopupProcess.PerformCallback('process');
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
                if (typeof onRefreshControl == 'function')
                    onRefreshControl();
            }
        }
    }

    function onCbpViewEndCallback(s) {
        hideLoadingPanel();
    }
</script>
<input type="hidden" runat="server" id="hdnPrescriptionOrderID" value="" />
<input type="hidden" runat="server" id="hdnPrescriptionOrderDetailID" value="" />
<input type="hidden" runat="server" id="hdnPastMedicationID" value="" />
<input type="hidden" runat="server" id="hdnItemID" value="" />
<div>
    <div>
        <table width="100%">
            <colgroup>
                <col width="115px" />
                <col width="145px" />
                <col width="115px" />
                <col />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Tanggal")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtMedicationDate" runat="server" Width="120px" CssClass="datepicker" />
                </td>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Medication Time")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtMedicationTime" runat="server" Width="60px" Text="00:00" CssClass="time"/>
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Item Name")%></label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtItemName" runat="server" Width="100%" ReadOnly="true"/>
                </td>
            </tr>
            <tr>
                <td class="tdLabel" style="vertical-align:top">
                    <label class="lblNormal">
                        <%=GetLabel("Special Instruction")%></label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtMedicationAdministration" runat="server" Width="100%" ReadOnly="true" TextMode="MultiLine" Height="50px"/>
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Dosage")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtNumberOfDosage" runat="server" Width="40px" CssClass="number" />
                </td>
                <td>
                    <dxe:ASPxComboBox ID="cboDosingUnit" ClientInstanceName="cboDosingUnit" runat="server"
                        Width="100%" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Medication Status")%></label>
                </td>
                <td colspan="3">
                    <dxe:ASPxComboBox ID="cboMedicationStatus" ClientInstanceName="cboMedicationStatus"
                    runat="server" Width="100%" >
                    </dxe:ASPxComboBox>
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Status Remarks")%></label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtOtherMedicationStatus" runat="server" Width="100%"/>
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Perawat")%></label>
                </td>
                <td colspan="3">
                    <dxe:ASPxComboBox ID="cboParamedicID" ClientInstanceName="cboParamedicID"
                    runat="server" Width="100%" >
                    </dxe:ASPxComboBox>
                </td>
            </tr>
        </table>
    </div>
    <dxcp:ASPxCallbackPanel ID="cbpPopupProcess" runat="server" Width="100%" ClientInstanceName="cbpPopupProcess"
        ShowLoadingPanel="false" OnCallback="cbpPopupProcess_Callback">
        <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpPopupProcesEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</div>
