<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VitalSignInformationCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.EMR.Program.VitalSignInformationCtl" %>

<asp:Repeater ID="rptView" runat="server">
    <HeaderTemplate>
        <table class="grdNormal" style="width:850px;font-size:1em" cellpadding="0" cellspacing="0">
            <colgroup>
                <col style="width:250px"/>
                <col style="width:100px"/>
                <col style="width:100px"/>
                <col style="width:100px"/>
                <col style="width:100px"/>
            </colgroup>
            <tr>
                <th><div> </div></th>
                <th align="center"><%=GetLabel("Min")%></th>
                <th align="center"><%=GetLabel("Avg")%></th>
                <th align="center"><%=GetLabel("Max")%></th>
                <th align="center"><%=GetLabel("Last")%></th>
            </tr>      
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td align="left"><%#:Eval("VitalSignLabel") %></td>
            <td align="center"><input type="text" class="number" readonly="readonly" value='<%#:Eval("MinValue") %>' /></td>
            <td align="center"><input type="text" class="number" readonly="readonly" value='<%#:Eval("AvgValue") %>' /></td>
            <td align="center"><input type="text" class="number" readonly="readonly" value='<%#:Eval("MaxValue") %>' /></td>
            <td align="center"><input type="text" class="number" readonly="readonly" value='<%#:Eval("LatestValue") %>' /></td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </table>
    </FooterTemplate>
</asp:Repeater>