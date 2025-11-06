<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EpisodeSummaryFollowUpVisitCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.EpisodeSummaryFollowUpVisitCtl" %>

<h3 class="headerContent"><%=GetLabel("Follow Up Visit")%></h3>
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
        <asp:Repeater ID="rptReviewOfSystem" runat="server">
            <ItemTemplate> 
                <tr>
                    <td class="warnaHeader" style="width:200px;">
                        <div><%=GetLabel("Follow Up Date Time")%>, <%=GetLabel("Physician")%>, <%=GetLabel("Location")%></div>
                        <div><%=GetLabel("Visit Type")%>; <%=GetLabel("Notes")%></div>
                    </td>
                    <td style="text-align:center; width:20px;">:</td>
                    <td colspan="2">
                        <div><%#: Eval("StartDateTimeInString")%>, <%#: Eval("ParamedicName")%></div>
                        <div><%#: Eval("VisitTypeName")%>; <%#: Eval("Notes")%> </div>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                <div id="divRptEmpty" class="divRptEmpty" runat="server" style="display:none">
                    No data to display 
                </div>
            </FooterTemplate>
        </asp:Repeater>
    </table>
</div>