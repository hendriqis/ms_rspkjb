<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SetBPJSClaimTypeCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.SetBPJSClaimTypeCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_SetBPJSClaimTypeCtl">
    $(function () {
    });
</script>
<div style="height: 250px; overflow-y: auto;">
    <input type="hidden" runat="server" id="hdnPatientPaymentIDCtl" value="" />
    <input type="hidden" runat="server" id="hdnPatientPaymentDtIDCtl" value="" />
    <input type="hidden" runat="server" id="hdnSetvarCustomerTypeBPJSCtl" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 150px" />
            <col />
        </colgroup>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("No Piutang")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtPaymentNo" ReadOnly="true" Width="200px" runat="server" style="text-align:center" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Tgl/Jam Piutang")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtPaymentDateTime" ReadOnly="true" Width="200px" runat="server" style="text-align:center" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Instansi")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtBusinessPartnerName" ReadOnly="true" Width="100%" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%:GetLabel("No. Urut")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtSequenceNo" ReadOnly="true" Width="100px" runat="server" CssClass="number" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%:GetLabel("Jenis Klaim BPJS")%></label>
            </td>
            <td>
                <dxe:ASPxComboBox ID="cboBPJSClaimType" ClientInstanceName="cboBPJSClaimType" Width="200px" runat="server">
                </dxe:ASPxComboBox>
            </td>
        </tr>
    </table>
</div>
