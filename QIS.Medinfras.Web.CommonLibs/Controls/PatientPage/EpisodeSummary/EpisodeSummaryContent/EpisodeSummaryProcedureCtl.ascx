<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EpisodeSummaryProcedureCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.EpisodeSummaryProcedureCtl" %>

<h3 class="headerContent">Treatment Procedure</h3>
<div style="max-height:380px;overflow-y:auto">
    <table style="width:100%" cellpadding="0" cellspacing="0">
        <colgroup style="width:100px"/>
        <colgroup style="width:20px"/>
        <colgroup style="width:200px"/>
        <asp:Repeater ID="rptDifferentDiagnosis" runat="server">
            <ItemTemplate>
                <tr>
                    <td class="warnaHeader">Date/Time </td>
                    <td style="text-align:center;">:</td>
                    <td colspan="2"><%#: Eval("ProcedureDateInString")%> / <%#: Eval("ProcedureTime")%></td>
                </tr>
                <tr>
                    <td class="labelColumn" >Procedure </td>
                    <td style="text-align:center;">:</td>
                    <td style=" color:#AD3400;"><%#:Eval("ProcedureName")%> (<%#: Eval("ProcedureID")%>) </td>
                </tr>
                <tr><td>&nbsp;</td></tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</div>