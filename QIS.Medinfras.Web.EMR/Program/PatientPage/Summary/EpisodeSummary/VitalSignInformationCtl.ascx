<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VitalSignInformationCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.EMR.Program.VitalSignInformationCtl" %>

<asp:Repeater ID="rptView" runat="server">
    <HeaderTemplate>
        <table class="grdNormal" style="font-size:0.9em" cellpadding="0" cellspacing="0" width="350px">
            <tr>
                <th><div> </div></th>
                <th align="center" style="width:50px" ><%=GetLabel("MIN")%></th>
                <th align="center" style="width:50px"><%=GetLabel("MAX")%></th>
                <th align="center" style="width:50px"><%=GetLabel("AVERAGE")%></th>
                <th align="center" style="width:50px"><%=GetLabel("LAST")%></th>
            </tr>      
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td align="left" style="width:150px"><%#:Eval("VitalSignLabel") %></td>
            <td align="center" style="width:50px"><input type="text" class="number" readonly="readonly" value='<%#:Eval("MinValueText") %>' /></td>
            <td align="center" style="width:50px"><input type="text" class="number" readonly="readonly" value='<%#:Eval("MaxValueText") %>' /></td>
            <td align="center" style="width:50px"><input type="text" class="number" readonly="readonly" value='<%#:Eval("AvgValueText") %>' /></td>
            <td align="center" style="width:50px"><input type="text" class="number" readonly="readonly" value='<%#:Eval("LatestValueText") %>' /></td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </table>
    </FooterTemplate>
</asp:Repeater>