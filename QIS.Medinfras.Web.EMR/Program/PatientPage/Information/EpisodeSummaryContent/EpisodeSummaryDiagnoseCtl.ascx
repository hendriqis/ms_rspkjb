<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EpisodeSummaryDiagnoseCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.EMR.Program.EpisodeSummaryDiagnoseCtl" %>

<h3 class="headerContent">Different Diagnosis</h3>
<div style="max-height:380px;overflow-y:auto">
    <table style="width:100%" cellpadding="0" cellspacing="0">
        <colgroup style="width:100px"/>
        <colgroup style="width:20px"/>
        <colgroup style="width:200px"/>
        <asp:Repeater ID="rptDifferentDiagnosis" runat="server">
            <ItemTemplate>
                <tr>
                    <td class="warnaHeader">Observation Date/Time </td>
                    <td style="text-align:center;">:</td>
                    <td colspan="2"><%# Eval("DifferentialDateInString")%> / <%# Eval("DifferentialTime")%></td>
                </tr>
                <tr>
                    <td class="labelColumn" >Diagnose Type </td>
                    <td style="text-align:center;">:</td>
                    <td style=" color:#AD3400;"><%# Eval("DiagnoseType")%></td>
                </tr>
                <tr>
                    <td class="labelColumn" >Diagnose </td>
                    <td style="text-align:center;">:</td>
                    <td style=" color:#AD3400;"><%# Eval("DiagnosisText")%> (<%# Eval("DiagnoseID")%>) </td>
                </tr>
                <tr>
                    <td class="labelColumn" >ICD Chapter </td>
                    <td style="text-align:center;">:</td>
                    <td style=" color:#AD3400;"><%# Eval("ICDBlockName")%></td>
                </tr>
            
                <tr>
                    <td class="labelColumn" >Differential Status </td>
                    <td style="text-align:center;">:</td>
                    <td style=" color:#AD3400;"><%# Eval("DifferentialStatus")%></td>
                </tr>
                <tr><td>&nbsp;</td></tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</div>