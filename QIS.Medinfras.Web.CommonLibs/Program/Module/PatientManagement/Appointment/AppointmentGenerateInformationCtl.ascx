<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AppointmentGenerateInformationCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.AppointmentGenerateInformationCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<div class="toolbarArea">
    <ul>
        <li id="btnMPEntryPopupVoid">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><div>
                <%=GetLabel("Void")%></div>
        </li>
    </ul>
</div>
<script type="text/javascript" id="dxss_generatesepctl">
    $('#btnMPEntryPopupVoid').click(function () {
        cbpPopupProcess.PerformCallback('void');
    });

    function onCbpPopupProcesEndCallback(s) {
        hideLoadingPanel();
        var param = s.cpResult.split('|');
        if (param[0] == 'sucess') {
            pcRightPanelContent.Hide();
            onRefreshPage();
        }
        else {
            showToast(' Failed', 'Error Message : ' + param[1]);
        }
    }
</script>
<div style="padding: 5px 0;">
    <input type="hidden" runat="server" id="hdnAppointmentID" value="" />
    <input type="hidden" runat="server" id="hdnAppointmentRequestID" value="" />
    <input type="hidden" runat="server" id="hdnRegistrationID" value="" />
    <table class="tblContentArea" width="100%">
        <col width="15%" />
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <b>
                        <%=GetLabel("No. Perjanjian")%></b></label>
            </td>
            <td>
                <asp:TextBox ID="txtAppointmentNo" Width="30%" ReadOnly="true" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <b>
                        <%=GetLabel("Pasien")%></b></label>
            </td>
            <td>
                <asp:TextBox ID="txtPatientName" Width="100%" ReadOnly="true" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <b>
                        <%=GetLabel("Phone No")%></b></label>
            </td>
            <td>
                <asp:TextBox ID="txtPhoneNo" Width="100%" ReadOnly="true" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <b>
                        <%=GetLabel("Mobile Phone")%></b></label>
            </td>
            <td>
                <asp:TextBox ID="txtMobilePhoneNo" Width="100%" ReadOnly="true" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <b>
                        <%=GetLabel("Pembayar")%></b></label>
            </td>
            <td>
                <asp:TextBox ID="txtBusinessPartnerName" Width="100%" ReadOnly="true" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <b>
                        <%=GetLabel("Tanggal ")%></b></label>
            </td>
            <td>
                <asp:TextBox ID="txtAppointmentDate" Width="100%" ReadOnly="true" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <b>
                        <%=GetLabel("Dokter")%></b></label>
            </td>
            <td>
                <asp:TextBox ID="txtParamedicName" Width="100%" ReadOnly="true" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <b>
                        <%=GetLabel("Klinik")%></b></label>
            </td>
            <td>
                <asp:TextBox ID="txtServiceUnitName" Width="100%" ReadOnly="true" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <b>
                        <%=GetLabel("Jenis Kunjungan")%></b></label>
            </td>
            <td>
                <asp:TextBox ID="txtVisitType" Width="100%" ReadOnly="true" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <b>
                        <%=GetLabel("No. Antrian")%></b></label>
            </td>
            <td>
                <asp:TextBox ID="txtQueueNo" Width="20%" ReadOnly="true" runat="server" />
            </td>
        </tr>
<%--        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <b>
                        <%=GetLabel("Sesi")%></b></label>
            </td>
            <td>
                <asp:TextBox ID="txtSession" Width="20%" ReadOnly="true" runat="server" />
            </td>
        </tr>--%>
    </table>
    <dxcp:ASPxCallbackPanel ID="cbpPopupProcess" runat="server" Width="100%" ClientInstanceName="cbpPopupProcess"
        ShowLoadingPanel="false" OnCallback="cbpPopupProcess_Callback">
        <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpPopupProcesEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</div>
