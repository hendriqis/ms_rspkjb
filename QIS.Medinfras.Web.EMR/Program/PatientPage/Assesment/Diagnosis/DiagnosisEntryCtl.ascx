<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DiagnosisEntryCtl.ascx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.PatientPage.DiagnosisEntryCtl" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" 
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>

<script type="text/javascript" id="dxss_diagnosisentryctl">
    setDatePicker('<%=txtDiagnosisDate.ClientID %>');
    $('#<%=txtDiagnosisDate.ClientID %>').datepicker('option', 'maxDate', '0');
    
    function onLedDiagnoseLostFocus(led) {
        var diagnoseID = led.GetValueText();
        $('#<%=hdnDiagnoseID.ClientID %>').val(diagnoseID);
        $('#<%=hdnDiagnoseText.ClientID %>').val(led.GetDisplayText());
        ledMorphology.SetFilterExpression("DiagnoseID = '" + diagnoseID + "' AND IsDeleted = 0");
    }

    function onLedMorphologyLostFocus(value) {
        $('#<%=hdnMorphologyID.ClientID %>').val(value);
    }
</script>

<div style="height: 410px; overflow-y: scroll; border: 0px">
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnDefaultParamedicID" runat="server" />
    <table style="width: 100%">
        <colgroup>
            <col style="width: 150px" />
            <col style="width: 150px" />
            <col />
        </colgroup>
        <tr>
            <td class="tdLabel">
                <label class="lblMandatory">
                    <%=GetLabel("Physician")%></label>
            </td>
            <td colspan="2">
                <dxe:ASPxComboBox runat="server" ID="cboPhysician" ClientInstanceName="cboPhysician"
                    Width="305px" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblMandatory">
                    <%=GetLabel("Date")%>
                    -
                    <%=GetLabel("Time")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtDiagnosisDate" Width="120px" CssClass="datepicker" runat="server" />
            </td>
            <td>
                <asp:TextBox ID="txtDiagnosisTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblMandatory">
                    <%=GetLabel("Diagnosis Type")%></label>
            </td>
            <td colspan="2">
                <dxe:ASPxComboBox runat="server" ID="cboDiagnoseType" ClientInstanceName="cboDiagnoseType"
                    Width="305px" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblMandatory">
                    <%=GetLabel("Diagnosis")%></label>
            </td>
            <td colspan="2">
                <input type="hidden" value="" id="hdnDiagnoseID" runat="server" />
                <input type="hidden" value="" id="hdnDiagnoseText" runat="server" />
                <qis:QISSearchTextBox ID="ledDiagnose" ClientInstanceName="ledDiagnose" runat="server"
                    Width="300px" ValueText="DiagnoseID" FilterExpression="IsDeleted = 0" DisplayText="DiagnoseName"
                    MethodName="GetDiagnoseList">
                    <ClientSideEvents ValueChanged="function(s){ onLedDiagnoseLostFocus(s); }" />
                    <Columns>
                        <qis:QISSearchTextBoxColumn Caption="Diagnose Name" FieldName="DiagnoseName" Description="i.e. Cholera"
                            Width="300px" />
                        <qis:QISSearchTextBoxColumn Caption="Diagnose Code" FieldName="DiagnoseID" Description="i.e. A09"
                            Width="100px" />
                    </Columns>
                </qis:QISSearchTextBox>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Morphology")%></label>
            </td>
            <td colspan="2">
                <input type="hidden" value="" id="hdnMorphologyID" runat="server" />
                <qis:QISSearchTextBox ID="ledMorphology" ClientInstanceName="ledMorphology" runat="server"
                    Width="300px" ValueText="MorphologyID" FilterExpression="1 = 0" DisplayText="MorphologyName"
                    MethodName="GetMorphologyList">
                    <ClientSideEvents ValueChanged="function(s){ onLedMorphologyLostFocus(s.GetValueText()); }" />
                    <Columns>
                        <qis:QISSearchTextBoxColumn Caption="Morphology Code" FieldName="MorphologyID" Description="i.e. M8000/0"
                            Width="100px" />
                        <qis:QISSearchTextBoxColumn Caption="Morphology Name" FieldName="MorphologyName"
                            Description="i.e. Neoplasm" Width="300px" />
                    </Columns>
                </qis:QISSearchTextBox>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Status")%></label>
            </td>
            <td colspan="2">
                <dxe:ASPxComboBox runat="server" ID="cboStatus" ClientInstanceName="cboStatus" Width="305px" />
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td colspan="2">
                <asp:CheckBox runat="server" ID="chkIsFollowUp" /><%=GetLabel("Follow Up")%>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td colspan="2">
                <asp:CheckBox runat="server" ID="chkIsChronic" /><%=GetLabel("Chronic")%>
            </td>
        </tr>
    </table>
</div>
