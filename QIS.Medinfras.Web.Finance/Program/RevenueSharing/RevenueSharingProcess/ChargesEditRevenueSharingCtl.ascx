<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChargesEditRevenueSharingCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.ChargesEditRevenueSharingCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_ChargesEditRevenueSharingCtl">

    //#region Revenue Sharing
    function onGetRevenueSharingFilterExpression() {
        var filterExpression = "IsDeleted = 0";
        return filterExpression;
    }

    $('#lblRevenueSharingCtl.lblLink').live('click', function () {
        openSearchDialog('revenuesharing', onGetRevenueSharingFilterExpression(), function (value) {
            $('#<%=txtRevenueSharingCodeCtl.ClientID %>').val(value);
            ontxtRevenueSharingCodeCtlChanged(value);
        });
    });

    $('#<%=txtRevenueSharingCodeCtl.ClientID %>').live('change', function () {
        ontxtRevenueSharingCodeCtlChanged($(this).val());
    });

    function ontxtRevenueSharingCodeCtlChanged(value) {
        var filterExpression = onGetRevenueSharingFilterExpression() + " AND RevenueSharingCode = '" + value + "'";
        Methods.getObject('GetRevenueSharingHdList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnRevenueSharingIDCtl.ClientID %>').val(result.RevenueSharingID);
                $('#<%=txtRevenueSharingNameCtl.ClientID %>').val(result.RevenueSharingName);
            }
            else {
                $('#<%=hdnRevenueSharingIDCtl.ClientID %>').val('');
                $('#<%=txtRevenueSharingCodeCtl.ClientID %>').val('');
                $('#<%=txtRevenueSharingNameCtl.ClientID %>').val('');
            }
        });
    }
    //#endregion

</script>
<div style="height: 250px; overflow-y: auto;">
    <input type="hidden" runat="server" id="hdnParamedicIDCtlRS" value="" />
    <input type="hidden" runat="server" id="hdnPatientChargesDtIDCtlRS" value="" />
    <input type="hidden" runat="server" id="hdnChargesDtIDDtIDCtlRS" value="" />
    <input type="hidden" runat="server" id="hdnChargesDtIDSourceCtlRS" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 150px" />
            <col style="width: 150px" />
            <col />
        </colgroup>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("No Transaksi")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtTransactionNo" ReadOnly="true" Width="150px" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Tgl Transaksi")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtTransactionDate" ReadOnly="true" Width="150px" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Item Pelayanan")%></label>
            </td>
            <td colspan="2">
                <asp:TextBox ID="txtItemNameCode" ReadOnly="true" Width="600px" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Dokter/Paramedis")%></label>
            </td>
            <td colspan="2">
                <asp:TextBox ID="txtParamedicNameCode" ReadOnly="true" Width="600px" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <label class="lblMandatory lblLink" id="lblRevenueSharingCtl">
                    <%=GetLabel("Jasa Medis")%></label>
            </td>
            <td>
                <input type="hidden" id="hdnRevenueSharingIDCtl" runat="server" value="" />
                <asp:TextBox ID="txtRevenueSharingCodeCtl" Width="100%" runat="server" />
            </td>
            <td>
                <asp:TextBox ID="txtRevenueSharingNameCtl" ReadOnly="true" Width="100%" runat="server" />
            </td>
        </tr>
    </table>
</div>
