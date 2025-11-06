<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChangeTransactionPhysicianCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.ChangeTransactionPhysicianCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_changetransactionphysicianctl">
    //#region Physician
    function onGetPhysicianFilterExpression() {
        var filterExpression = "<%:OnGetParamedicFilterExpression() %>";
        return filterExpression;
    }

    $('#lblPhysician.lblLink').live('click', function () {
        openSearchDialog('paramedic', onGetPhysicianFilterExpression(), function (value) {
            $('#<%=txtPhysicianCode.ClientID %>').val(value);
            onTxtPhysicianCodeChanged(value);
        });
    });

    $('#<%=txtPhysicianCode.ClientID %>').live('change', function () {
        onTxtPhysicianCodeChanged($(this).val());
    });

    function onTxtPhysicianCodeChanged(value) {
        var filterExpression = "ParamedicCode = '" + value + "'";
        Methods.getObject('GetParamedicMasterList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnPhysicianID.ClientID %>').val(result.ParamedicID);
                $('#<%=txtPhysicianName.ClientID %>').val(result.FullName);
            }
            else {
                $('#<%=hdnPhysicianID.ClientID %>').val('');
                $('#<%=txtPhysicianCode.ClientID %>').val('');
                $('#<%=txtPhysicianName.ClientID %>').val('');
            }
        });
    }
    //#endregion
</script>
<div style="height: 150px; overflow-y: auto;">
    <input type="hidden" runat="server" id="hdnRegistrationIDCtlCP" value="" />
    <input type="hidden" runat="server" id="hdnVisitIDCtlCP" value="" />
    <input type="hidden" runat="server" id="hdnTransactionIDCtlCP" value="" />
    <input type="hidden" runat="server" id="hdnTransactionHealthcareServiceUnitIDCtlCP" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 150px" />
            <col style="width: 500px" />
            <col />
        </colgroup>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("No Transaksi")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtTransactionNo" ReadOnly="true" Width="160px" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Tanggal - Jam Transaksi")%></label>
            </td>
            <td>
                <table>
                    <colgroup>
                        <col style="width: 100px" />
                        <col style="width: 5px" />
                        <col style="width: 50px" />
                    </colgroup>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtTransactionDate" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txtTransactionTime" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblLink lblMandatory" id="lblPhysician">
                    <%=GetLabel("Dokter / Paramedis")%></label>
            </td>
            <td>
                <input type="hidden" id="hdnPhysicianID" value="" runat="server" />
                <table>
                    <colgroup>
                        <col style="width: 160px" />
                        <col style="width: 5px" />
                        <col style="width: 300px" />
                    </colgroup>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtPhysicianCode" CssClass="required" Width="100%" runat="server" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txtPhysicianName" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
