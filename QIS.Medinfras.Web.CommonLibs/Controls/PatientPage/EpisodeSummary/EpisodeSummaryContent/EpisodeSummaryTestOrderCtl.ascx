<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EpisodeSummaryTestOrderCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.EpisodeSummaryTestOrderCtl" %>

<h3 class="headerContent"><%=GetLabel("Test Order")%></h3>
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
        <asp:Repeater ID="rptTestOrder" runat="server" OnItemDataBound="rptTestOrder_ItemDataBound">
            <ItemTemplate>
                <tr>
                    <td class="warnaHeader"><%=GetLabel("Order Date/Time")%></td>
                    <td style="text-align:center;">:</td>
                    <td><%#: Eval("TestOrderDateInString")%> / <%#: Eval("TestOrderTime") %></td>
                </tr>
                <asp:Repeater ID="rptTestOrderDt" runat="server">
                    <HeaderTemplate>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td class="labelColumn" >Item</td>
                            <td style="text-align:center;">:</td>
                            <td style=" color:#AD3400;"><%#: Eval("ItemName1")%></td>
                        </tr>
<%--                        <tr>
                            <td class="labelColumn" >Diagnose</td>
                            <td style="text-align:center;">:</td>
                            <td style=" color:#AD3400;"><%#: Eval("DiagnoseName")%></td>
                        </tr>
                        <tr>
                            <td class="labelColumn" >To Be Performed</td>
                            <td style="text-align:center;">:</td>
                            <td style=" color:#AD3400;"><%#: Eval("ToBePerformed")%></td>
                        </tr>--%>
                    </ItemTemplate>
                </asp:Repeater>
                <tr><td>&nbsp;</td></tr>
            </ItemTemplate>
            <FooterTemplate>
                <div id="divRptEmpty" class="divRptEmpty" runat="server" style="display:none">                    
                </div>
            </FooterTemplate>
        </asp:Repeater>
    </table>
</div>
