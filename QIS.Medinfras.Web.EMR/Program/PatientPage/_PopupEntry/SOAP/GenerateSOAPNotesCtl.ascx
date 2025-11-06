<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GenerateSOAPNotesCtl.ascx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.GenerateSOAPNotesCtl" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" 
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>

<script type="text/javascript" id="dxss_diagnosisentryctl">
    setDatePicker('<%=txtNoteDate.ClientID %>');
    $('#<%=txtNoteDate.ClientID %>').datepicker('option', 'maxDate', '0');
    $('#<%=txtSubjectiveText.ClientID %>').focus();  
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
            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Tanggal ")%> - <%=GetLabel("Jam ")%></label></td>
            <td><asp:TextBox ID="txtNoteDate" Width="120px" CssClass="datepicker" runat="server" /></td>
            <td><asp:TextBox ID="txtNoteTime" Width="80px" CssClass="time" runat="server"/></td>
        </tr>
        <tr>
            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Physician")%></label></td>
            <td colspan="2"><dxe:ASPxComboBox runat="server" ID="cboPhysician" ClientInstanceName="cboPhysician" Width="300px" /></td>
        </tr>
        <tr>
            <td class="tdLabel" valign="top" style="font-weight: bold;padding-top: 5px;vertical-align: top;">
                <label class="lblNormal lblMandatory">
                    <%=GetLabel("Subjective") %></label>
            </td>
            <td colspan="2">
                <asp:TextBox ID="txtSubjectiveText" runat="server" Width="648px" TextMode="MultiLine"
                    Rows="2" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel" style="font-weight: bold; margin-bottom: 10px; vertical-align: top">
                <label class="lblNormal lblMandatory">
                    <%=GetLabel("Objective")%></label>
            </td>
            <td colspan="2">
                <asp:TextBox ID="txtObjectiveText" Width="648px" runat="server" TextMode="MultiLine"
                    Rows="2" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel" style="font-weight: bold; margin-bottom: 10px; vertical-align: top">
                <label class="lblNormal lblMandatory">
                    <%=GetLabel("Assessment")%></label>
            </td>
            <td colspan="2">
                <asp:TextBox ID="txtAssessmentText" Width="648px" runat="server" TextMode="MultiLine"
                    Rows="2" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel" style="font-weight: bold; margin-bottom: 10px; vertical-align: top">
                <label class="lblNormal lblMandatory">
                    <%=GetLabel("Planning")%></label>
            </td>
            <td colspan="2">
                <asp:TextBox ID="txtPlanningText" Width="648px" runat="server" TextMode="MultiLine"
                    Rows="3" />
            </td>
        </tr>    
        <tr>
            <td class="tdLabel" style="font-weight: bold; margin-bottom: 10px; vertical-align: top">
                <label class="lblNormal">
                    <%=GetLabel("Instruksi")%></label>
            </td>
            <td colspan="2">
                <asp:TextBox ID="txtInstructionText" Width="648px" runat="server" TextMode="MultiLine"
                    Rows="3" />
            </td>
        </tr>      
    </table>
</div>
