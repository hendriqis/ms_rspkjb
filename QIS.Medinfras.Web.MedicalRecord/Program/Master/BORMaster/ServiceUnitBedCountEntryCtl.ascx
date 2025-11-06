<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ServiceUnitBedCountEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.MedicalRecord.Program.ServiceUnitBedCountEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_ServiceUnitBedCountEntryCtl">
    $(function () {
        setDatePicker('<%:txtStartingDate.ClientID %>');
        $('#<%:txtStartingDate.ClientID %>').datepicker('option', 'minDate', 0);
    });

    $('#<%=txtBedCount.ClientID %>').keydown(function (e) {
        var key = e.charCode || e.keyCode || 0;
        return (
                    key == 8 ||
                    key == 9 ||
                    key == 13 ||
                    key == 46 ||
                    (key >= 35 && key <= 40) ||
                    (key >= 48 && key <= 57) ||
                    (key >= 96 && key <= 105));
    });

</script>
<div style="height: 250px; overflow-y: auto;">
    <input type="hidden" runat="server" id="hdnServiceUnitID" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 150px" />
            <col />
        </colgroup>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Unit Pelayanan")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtServiceUnitName" ReadOnly="true" Width="300px" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblMandatory">
                    <%=GetLabel("Tanggal Mulai Belaku")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtStartingDate" Width="120px" runat="server" CssClass="datepicker" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblMandatory">
                    <%=GetLabel("Jumlah Bed")%></label>
            </td>
            <td colspan="3">
                <asp:TextBox ID="txtBedCount" runat="server" Width="100px" CssClass="numeric">
                </asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Catatan")%></label>
            </td>
            <td colspan="3">
                <asp:TextBox ID="txtRemarks" runat="server" Width="300px">
                </asp:TextBox>
            </td>
        </tr>
    </table>
</div>
