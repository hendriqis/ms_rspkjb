<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChargesEditParamedicIDCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.ChargesEditParamedicIDCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_ChargesEditParamedicIDCtl">

    //#region Paramedic
    function onGetParamedicFilterExpression() {
        var filterExpression = "IsDeleted = 0 AND IsHasRevenueSharing = 1 AND IsAvailable = 1 AND IsDummy = 0";
        return filterExpression;
    }

    $('#lblParamedicCtl.lblLink').live('click', function () {
        openSearchDialog('paramedic', onGetParamedicFilterExpression(), function (value) {
            $('#<%=txtParamedicCodeCtl.ClientID %>').val(value);
            ontxtParamedicCodeCtlChanged(value);
        });
    });

    $('#<%=txtParamedicCodeCtl.ClientID %>').live('change', function () {
        ontxtParamedicCodeCtlChanged($(this).val());
    });

    function ontxtParamedicCodeCtlChanged(value) {
        var filterExpression = onGetParamedicFilterExpression() + " AND ParamedicCode = '" + value + "'";
        Methods.getObject('GetParamedicMasterList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnParamedicIDCtl.ClientID %>').val(result.ParamedicID);
                $('#<%=txtParamedicNameCtl.ClientID %>').val(result.FullName);
            }
            else {
                $('#<%=hdnParamedicIDCtl.ClientID %>').val('');
                $('#<%=txtParamedicCodeCtl.ClientID %>').val('');
                $('#<%=txtParamedicNameCtl.ClientID %>').val('');
            }
        });
    }
    //#endregion

</script>
<div style="height: 250px; overflow-y: auto;">
    <input type="hidden" runat="server" id="hdnChargesDtIDCtl" value="" />
    <input type="hidden" runat="server" id="hdnChargesDtIDDtIDCtl" value="" />
    <input type="hidden" runat="server" id="hdnChargesDtIDSourceCtl" value="" />
    <input type="hidden" runat="server" id="hdnIsUpdate" value="" />
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
            <td>
                <label class="lblMandatory lblLink" id="lblParamedicCtl">
                    <%=GetLabel("Dokter/Paramedis")%></label>
            </td>
            <td>
                <input type="hidden" id="hdnParamedicIDCtl" runat="server" value="" />
                <asp:TextBox ID="txtParamedicCodeCtl" Width="150px" runat="server" />
            </td>
            <td>
                <asp:TextBox ID="txtParamedicNameCtl" ReadOnly="true" Width="100%" runat="server" />
            </td>
        </tr>
    </table>
</div>
