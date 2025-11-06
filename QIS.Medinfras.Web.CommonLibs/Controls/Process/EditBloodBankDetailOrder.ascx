<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditBloodBankDetailOrder.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.EditBloodBankDetailOrder" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_EditBloodBankDetailOrder">
    $(function () {
        $('#<%=rblGCSourceType.ClientID %> input').change(function () {
            if ($(this).val() == "X533^001") {
                $('#<%=trPaymentTypeInfo.ClientID %>').removeAttr("style");
            }
            else {
                $('#<%=trPaymentTypeInfo.ClientID %>').attr("style", "display:none");
            }
        });
    });

</script>
<div style="height: 200px; overflow-y: scroll;">
    <input type="hidden" id="hdnVisitIDBloodBankCtl" value="0" runat="server" />
    <input type="hidden" id="hdnTestOrderIDBloodBankCtl" value="0" runat="server" />
    <table class="tblEntryContent">
        <colgroup>
            <col style="width: 150px" />
            <col style="width: 250px" />
            <col style="width: 200px" />
            <col />
        </colgroup>
        <tr>
            <td class="tdLabel">
                <label class="lblMandatory">
                    <%=GetLabel("Sumber/Asal Darah") %></label>
            </td>
            <td>
                <asp:RadioButtonList ID="rblGCSourceType" runat="server" RepeatDirection="Horizontal"
                    RepeatLayout="Table">
                    <asp:ListItem Text="PMI" Value="X533^001" />
                    <asp:ListItem Text="Persediaan BDRS" Value="X533^002" />
                    <asp:ListItem Text="Pendonor" Value="X533^003" />
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblMandatory">
                    <%=GetLabel("Cara Penyimpanan") %></label>
            </td>
            <td>
                <asp:RadioButtonList ID="rblGCUsageType" runat="server" RepeatDirection="Horizontal"
                    RepeatLayout="Table">
                    <asp:ListItem Text="Langsung digunakan" Value="X534^001" />
                    <asp:ListItem Text="Dititipkan di BDRS" Value="X534^002" />
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr id="trPaymentTypeInfo" runat="server" style="display: none">
            <td class="tdLabel">
                <label class="lblMandatory">
                    <%=GetLabel("Cara Pembayaran (Jika PMI)") %></label>
            </td>
            <td>
                <asp:RadioButtonList ID="rblGCPaymentType" runat="server" RepeatDirection="Horizontal"
                    RepeatLayout="Table">
                    <asp:ListItem Text="Dibayar langsung di PMI" Value="X535^001" />
                    <asp:ListItem Text="Tagihan Pasien di Rumah Sakit" Value="X535^002" />
                </asp:RadioButtonList>
            </td>
        </tr>
    </table>
</div>
