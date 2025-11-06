<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EpisodeSummaryVitalSignIndicatorCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.EMR.Program.EpisodeSummaryVitalSignIndicatorCtl" %>

<h3 class="headerContent">Vital Signs and Indicators</h3>
<style type="text/css">
.warnaHeader
{
    color:#016482;
}
</style>
<div style="max-height:380px;overflow-y:auto">
    <table style="width:100%" cellpadding="0" cellspacing="0">
        <colgroup style="width:100px"/>
        <colgroup style="width:20px"/>
        <colgroup style="width:25px"/>
        <colgroup style="width:100px"/>
        <asp:Repeater ID="rptVitalSign" runat="server" OnItemDataBound="rptVitalSign_ItemDataBound">
            <ItemTemplate>
                <tr>
                    <td class="warnaHeader">Observation Date/Time </td>
                    <td style="text-align:center;">:</td>
                    <td colspan="2"><%# Eval("ObservationDateInString")%> / <%# Eval("ObservationTime") %></td>
                </tr>
                <asp:Repeater ID="rptVitalSignDt" runat="server" OnItemDataBound="rptVitalSignDt_ItemDataBound">
                    <HeaderTemplate>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr id="trVitalSignDt" runat="server">
                            <td class="labelColumn" ><%# Eval("VitalSignLabel")%></td>
                            <td style="text-align:center;">:</td>
                            <td id="tdTxt" runat="server" style="display:none">
                                <div style="text-align:right;color:#AD3400;float:left;width:50px"><%# Eval("VitalSignValue") %> </div>
                                <div style="width:150px;">&nbsp;&nbsp; <%# Eval("ValueUnit") %></div>
                            </td>
                            <td id="tdCbo" runat="server" style="display:none">
                                <div style="color:#AD3400;"><%# Eval("VitalSignValue") %> </div
                            </td>
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
