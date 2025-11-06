<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AppointmentMultiVisitScheduleOrderInfoCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.AppointmentMultiVisitScheduleOrderInfoCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<div style="padding: 5px 0;">
    <input type="hidden" runat="server" id="hdnAppointmentID" value="" />
    <input type="hidden" runat="server" id="hdnAppointmentRequestID" value="" />
    <input type="hidden" runat="server" id="hdnRegistrationID" value="" />
    <table class="tblContentArea" width="100%">
        <col width="30%" />
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <b>
                        <%=GetLabel("No. Perjanjian")%></b></label>
            </td>
            <td>
                <asp:TextBox ID="txtAppointmentNo" Width="100%" ReadOnly="true" runat="server" />
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
                        <%=GetLabel("Tindakan")%></b></label>
            </td>
            <td>
                <asp:TextBox ID="txtItemName" Width="100%" ReadOnly="true" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <b>
                        <%=GetLabel("Tindakan ke - ")%></b></label>
            </td>
            <td>
                <asp:TextBox ID="txtSequenceNo" Width="20%" ReadOnly="true" runat="server" />
            </td>
        </tr>
    </table>
</div>
