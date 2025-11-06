<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TreatmentProcedureCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.TreatmentProcedureCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<script type="text/javascript" id="dxss_TreatmentProcedureCtl">
    setDatePicker('<%=txtObservationDate.ClientID %>');
    $('#<%=txtObservationDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

    function onLedProcedureLostFocus(value) {
        $('#<%=hdnProcedureID.ClientID %>').val(value);

        var filterExpression = "ProcedureID = '" + value + "' AND IsDeleted = 0";
        Methods.getObject('GetProceduresList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=txtProcedureText.ClientID %>').val(result.ProcedureName);
            }
        });
    }
</script>
<div style="height: 300px; overflow-y: scroll;">
    <input type="hidden" runat="server" id="hdnID" value="" />
    <input type="hidden" runat="server" id="hdnDepartmentID" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top;">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 200px" />
                        <col style="width: 150px" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Date")%>
                                -
                                <%=GetLabel("Time")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtObservationDate" Width="120px" CssClass="datepicker" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtObservationTime" Width="80px" CssClass="time" runat="server"
                                Style="text-align: center" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Dokter / Tenaga Medis")%></label>
                        </td>
                        <td colspan="2">
                            <dxe:ASPxComboBox ID="cboParamedicID" ClientInstanceName="cboParamedicID" Width="300px"
                                runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Treatment Procedure")%></label>
                        </td>
                        <td colspan="2">
                            <input type="hidden" value="" id="hdnProcedureID" runat="server" />
                            <qis:QISSearchTextBox ID="ledProcedure" ClientInstanceName="ledProcedure" runat="server"
                                Width="400px" ValueText="ProcedureID" FilterExpression="IsDeleted = 0" DisplayText="ProcedureName"
                                MethodName="GetProceduresList">
                                <ClientSideEvents ValueChanged="function(s){ onLedProcedureLostFocus(s.GetValueText()); }" />
                                <Columns>
                                    <qis:QISSearchTextBoxColumn Caption="Procedure Code" FieldName="ProcedureID" Description="i.e. 1-16"
                                        Width="100px" />
                                    <qis:QISSearchTextBoxColumn Caption="Procedure Name" FieldName="ProcedureName" Description="i.e. Consultation"
                                        Width="300px" />
                                </Columns>
                            </qis:QISSearchTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                            <label class="lblMandatory">
                                <%=GetLabel("Treatment Procedure (Text)")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtProcedureText" Width="400px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                            <label class="lblNormal">
                                <%=GetLabel("Remarks")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtRemarks" Width="400px" runat="server" TextMode="MultiLine" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
