<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GenerateSOAPNotesCtl.ascx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.PatientPage.GenerateSOAPNotesCtl" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" 
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>

<script type="text/javascript" id="dxss_diagnosisentryctl">
    setDatePicker('<%=txtNoteDate.ClientID %>');
    $('#<%=txtNoteDate.ClientID %>').datepicker('option', 'maxDate', '0');
    $('#<%=txtNoteText.ClientID %>').focus();  
</script>

<div style="height: 450px; overflow-y: scroll; border: 0px">
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnDefaultParamedicID" runat="server" />
    <table style="width:100%">
        <colgroup>
            <col style="width:150px"/>
            <col style="width:150px"/>
            <col />
        </colgroup>
        <tr>
            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Date")%> - <%=GetLabel("Time")%></label></td>
            <td><asp:TextBox ID="txtNoteDate" Width="120px" CssClass="datepicker" runat="server" /></td>
            <td><asp:TextBox ID="txtNoteTime" Width="80px" CssClass="time" runat="server"/></td>
        </tr>
        <tr>
            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Physician")%></label></td>
            <td colspan="2"><dxe:ASPxComboBox runat="server" ID="cboPhysician" ClientInstanceName="cboPhysician" Width="300px" /></td>
        </tr>
        <tr>
            <td class="tdLabel" style="vertical-align:top"><label class="lblNormal"><%=GetLabel("Generated Note")%></label></td>
            <td colspan="2"><asp:TextBox ID="txtNoteText" Width="95%" runat="server" TextMode="MultiLine" Height="350px" /></td>
        </tr>
    </table>
</div>
