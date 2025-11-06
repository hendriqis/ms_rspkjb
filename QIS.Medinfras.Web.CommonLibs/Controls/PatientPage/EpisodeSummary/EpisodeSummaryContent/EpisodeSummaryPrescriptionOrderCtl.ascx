<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EpisodeSummaryPrescriptionOrderCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.EpisodeSummaryPrescriptionOrderCtl" %>

<h3 class="headerContent"><%=GetLabel("Prescription Order")%></h3>
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
        <asp:Repeater ID="rptPrescriptionOrder" runat="server" OnItemDataBound="rptPrescriptionOrder_ItemDataBound">
            <ItemTemplate>
                <tr>
                    <td class="warnaHeader"><%=GetLabel("Order Date/Time")%></td>
                    <td style="text-align:center;">:</td>
                    <td><%#: Eval("PrescriptionDateInString")%> / <%#: Eval("PrescriptionTime") %></td>
                </tr>
                <asp:Repeater ID="rptPrescriptionOrderDtCustom" runat="server">
                    <HeaderTemplate>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <div><%=GetLabel("Generic")%> - <%=GetLabel("Product")%> - <%=GetLabel("Strength")%> - <%=GetLabel("Form")%></div>
                                <div><div style="color:Blue;width:35px;float:left;"><%=GetLabel("DOSE")%></div> <%=GetLabel("Dose")%> - <%=GetLabel("Route")%> - <%=GetLabel("Frequency")%></div>
                            </td>
                            <td align="center">:</td>
                            <td>
                                <div><%#: Eval("InformationLine1")%></div>
                                <div><div style="color:Blue;width:35px;float:left;"><%=GetLabel("DOSE")%></div> <%#: Eval("NumberOfDosage")%> <%#: Eval("DosingUnit")%> - <%#: Eval("Route")%> - <%#: Eval("cfDoseFrequency")%></div>
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
