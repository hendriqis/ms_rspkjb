<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChangeScheduleSequenceCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.ChangeScheduleSequenceCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
    <%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<div class="toolbarArea">
    <ul>
        <li id="btnMPEntryProcess">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbProcess.png")%>' alt="" /><div>
                <%=GetLabel("Process")%></div>
        </li>
    </ul>
</div>
<script type="text/javascript" id="dxss_patiententryctl">
    $(function () {
    });

    $('#btnMPEntryProcess').click(function () {
        var message = "Are you sure to change the sequence ?";
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
                if (typeof onRefreshList == 'function') {
                    onRefreshList();
                    showToast('Medication Schedule', param[2]);
                }
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
<input type="hidden" runat="server" id="hdnPrescriptionOrderDtID" value="" />
<input type="hidden" runat="server" id="hdnPastMedicationID" value="" />
<div>
    <div>
        <table style="width:100%">
            <colgroup>
                <col width="115px" />
                <col width="60px" />
                <col width="115px" />
                <col />
            </colgroup>
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
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Tanggal")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtMedicationDate" runat="server" Width="120px" ReadOnly="true" CssClass="datepicker" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Current Sequence")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtCurrentSequence" runat="server" Width="60px" ReadOnly="true" CssClass="number" />
                </td>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("New Sequence")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtSequenceNo" runat="server" Width="60px" CssClass="number" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel" style="vertical-align:top">
                    <label class="lblNormal">
                        <%=GetLabel("Remarks")%></label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtRemarks" runat="server" Width="100%" TextMode=MultiLine Height="50px"/>
                </td>
            </tr>
            <tr>
                <td />
                <td colspan="3">
                    <asp:CheckBox ID="chkIsApplyToAllSchedule" runat="server" Text="Apply to All schedule for this item" Enabled="True" /> 
                </td>
            </tr>
        </table>
        <dxcp:ASPxCallbackPanel ID="cbpPopupProcess" runat="server" Width="100%" ClientInstanceName="cbpPopupProcess"
            ShowLoadingPanel="false" OnCallback="cbpPopupProcess_Callback">
            <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpPopupProcesEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
    </div>
</div>
