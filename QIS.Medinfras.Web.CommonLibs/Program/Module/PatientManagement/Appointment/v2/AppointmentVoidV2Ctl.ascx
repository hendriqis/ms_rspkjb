<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AppointmentVoidV2Ctl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.AppointmentVoidV2Ctl" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<script type="text/javascript" id="dxss_appointmentvoidctl">
    function onCboDeleteValueChanged() {
        if (cboDeleteReason.GetValue() == Constant.AppointmentDeleteReason.OTHER)
            $('#<%=txtOtherDeleteReason.ClientID %>').show();
        else
            $('#<%=txtOtherDeleteReason.ClientID %>').hide();
    }
</script>
<input type="hidden" runat="server" id="hdnID" value="" />
<input type="hidden" runat="server" id="hdnIsBridgingToGateway" value="" />
<input type="hidden" runat="server" id="hdnIsBridgingToMedinfrasMobileApps" value="" />
<input type="hidden" runat="server" id="hdnProviderGatewayService" value="" />
<table style="margin-top: 10px; text-align: left; width: 100%;">
    <colgroup style="width: 150px" />
    <colgroup style="width: 200px" />
    <tr>
        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Appointment No")%></label></td>
        <td><asp:TextBox ID="txtAppointmentNo" ReadOnly="true" Width="160px" runat="server" /></td>
    </tr>  
    <tr>
        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Patient Name")%></label></td>
        <td colspan="2"><asp:TextBox ID="txtPatientName" ReadOnly="true" Width="350px" runat="server" /></td>
    </tr>  
    <tr>
        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Service Unit")%></label></td>
        <td colspan="2"><asp:TextBox ID="txtServiceUnit" ReadOnly="true" Width="350px" runat="server" /></td>
    </tr>  
    <tr>
        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Physician")%></label></td>
        <td colspan="2"><asp:TextBox ID="txtPhysician" ReadOnly="true" Width="350px" runat="server" /></td>
    </tr>  
    <tr>
        <td class="tdLabel"><%=GetLabel("Reason For Delete")%></td>
        <td>
            <dxe:ASPxComboBox ID="cboDeleteReason" ClientInstanceName="cboDeleteReason" runat="server" Width="100%">
                <ClientSideEvents ValueChanged="function(s,e){ onCboDeleteValueChanged(); }"
                    Init="function(s,e){ onCboDeleteValueChanged(); }" />
            </dxe:ASPxComboBox>
        </td>
        <td><asp:TextBox ID="txtOtherDeleteReason" CssClass="required" runat="server" Width="100%" /></td>
    </tr>
</table>
