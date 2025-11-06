<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditMedicationOrderStatusCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Pharmacy.Program.EditMedicationOrderStatusCtl" %>
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
        setDatePicker('<%=txtCompleteDate.ClientID %>');
        $('#<%=txtCompleteDate.ClientID %>').datepicker('option', 'maxDate', '0');
    });


    $('#btnMPEntryProcess').click(function () {
        var message = "Save the changes for <b>" + $('#<%:txtPrescriptionOrderNo.ClientID %>').val() + "</b> ?";
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
                showToast("Print Failed", "Error Message : <br/><span style='color:red'>" + param[2] + "</span>");
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

    function onRefreshGrid() {
        cbpView.PerformCallback('refresh');
    }
</script>
<input type="hidden" runat="server" id="hdnSelectedID" value="" />
<input type="hidden" runat="server" id="hdnSelectedScheduleID" value="" />
<input type="hidden" runat="server" id="hdnDispensaryServiceUnitID" value="" />
<div>
    <div>
        <table>
            <colgroup>
                <col width="115px" />
                <col width="145px" />
                <col width="115px" />
                <col />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Order Date")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtMedicationDate" runat="server" Width="120px" ReadOnly="true" CssClass="datepicker" />
                </td>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Order Time")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtMedicationTime" runat="server" Width="60px" ReadOnly="true" Text="00:00" CssClass="time" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Order No.")%></label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtPrescriptionOrderNo" runat="server" Width="100%" ReadOnly="true"/>
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Order By")%></label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtParamedicName" runat="server" Width="100%" ReadOnly="true"/>
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Complete Date")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtCompleteDate" runat="server" Width="120px" CssClass="datepicker" />
                </td>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Complete Time")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtCompleteTime" runat="server" Width="60px" Text="00:00" CssClass="time" />
                </td>
            </tr>
        </table>
    </div>
    <dxcp:ASPxCallbackPanel ID="cbpPopupProcess" runat="server" Width="100%" ClientInstanceName="cbpPopupProcess"
        ShowLoadingPanel="false" OnCallback="cbpPopupProcess_Callback">
        <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpPopupProcesEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</div>
