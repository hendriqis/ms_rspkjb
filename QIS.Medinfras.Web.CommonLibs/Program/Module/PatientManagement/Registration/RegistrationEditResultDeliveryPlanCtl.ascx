<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RegistrationEditResultDeliveryPlanCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.RegistrationEditResultDeliveryPlanCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_registrationeditresultdeliveryplanctl">
    $(function () {

    });
    function onCboResultDeliveryPlanValueChangedEdit(s) {
        var oValue = cboResultDeliveryPlanEdit.GetValue();
        if (oValue == "X546^999") {
            $('#<%=txtResultDeliveryPlanOthersEdit.ClientID %>').attr("");
            $('#<%=txtResultDeliveryPlanOthersEdit.ClientID %>').removeAttr('readonly');
        } else {
            $('#<%=txtResultDeliveryPlanOthersEdit.ClientID %>').val("");
            $('#<%=txtResultDeliveryPlanOthersEdit.ClientID %>').attr('readonly', 'true');
        }
    }
    
</script>
<div style="height: 130px;">
    <input type="hidden" runat="server" id="hdnRegistrationIDCtl" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 200px" />
            <col />
        </colgroup>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("No Registrasi")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtRegistrationNo" ReadOnly="true" Width="150px" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("No. RM")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtNoRM" ReadOnly="true" Width="90%" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Pasien")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtPatient" ReadOnly="true" Width="90%" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%:GetLabel("Rencana Pengambilan Hasil")%></label>
            </td>
            <td>
                <dxe:ASPxComboBox ID="cboResultDeliveryPlanEdit" ClientInstanceName="cboResultDeliveryPlanEdit"
                    Width="100%" runat="server">
                    <ClientSideEvents ValueChanged="function(s){ onCboResultDeliveryPlanValueChangedEdit(s); }" />
                </dxe:ASPxComboBox>
            </td>
            <td>
                <asp:TextBox ID="txtResultDeliveryPlanOthersEdit" Width="100%" runat="server" />
            </td>
        </tr>
    </table>
</div>
