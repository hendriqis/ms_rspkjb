<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EpisodeSummaryReviewOfSystemCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.EMR.Program.EpisodeSummaryReviewOfSystemCtl" %>

<h3 class="headerContent">Review of System</h3>
<style type="text/css">
.warnaHeader
{
    color:#016482;
}
</style>
<div style="max-height:380px;overflow-y:auto">
    <table style="width:100%" cellpadding="0" cellspacing="0">
        <colgroup style="width:200px"/>
        <colgroup style="width:20px"/>
        <colgroup style="width:300px"/>
        <asp:Repeater ID="rptReviewOfSystem" runat="server" OnItemDataBound="rptReviewOfSystem_ItemDataBound">
            <ItemTemplate> 
                <tr>
                    <td class="warnaHeader" style="width:200px;">Observation Date/Time </td>
                    <td style="text-align:center; width:20px;">:</td>
                    <td colspan="2"><%# Eval("ObservationDateInString")%> / <%# Eval("ObservationTime") %></td>
                </tr>
                <asp:Repeater ID="rptReviewOfSystemDt" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td class="labelColumn" ><%# Eval("ROSystem")%></td>
                            <td style="text-align:center;">:</td>
                            <td style="text-align:left;color:#AD3400;"><%# Eval("cfRemarks")%> </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
                <tr><td>&nbsp;</td></tr>
            </ItemTemplate>
            <FooterTemplate>
                <div id="divRptEmpty" class="divRptEmpty" runat="server" style="display:none">
                    No data to display 
                </div>
            </FooterTemplate>
        </asp:Repeater>
    </table>
</div>