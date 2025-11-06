<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChangePaymentDateCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.ChangePaymentDateCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_ChangePaymentDateCtl">
    $(function () {
        setDatePicker('<%:txtPaymentDate.ClientID %>');
        $('#<%:txtPaymentDate.ClientID %>').datepicker('option', 'maxDate', 0);
    });
</script>
<div style="height: 250px; overflow-y: auto;">
    <input type="hidden" runat="server" id="hdnPaymentIDCtl" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 150px" />
            <col />
        </colgroup>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("No Registrasi")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtRegistrationNo" ReadOnly="true" Width="200px" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Informasi Pasien")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtPatientInfomation" ReadOnly="true" Width="500px" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("No Pembayaran")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtPaymentNo" ReadOnly="true" Width="200px" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblMandatory">
                    <%=GetLabel("Tanggal Pembayaran")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtPaymentDate" Width="120px" runat="server" CssClass="datepicker" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblMandatory">
                    <%=GetLabel("Jam Pembayaran")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtPaymentTime" Width="120px" runat="server" CssClass="time" />
            </td>
        </tr>
    </table>
</div>
