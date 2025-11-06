<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPFrame.Master" AutoEventWireup="true" 
    CodeBehind="LaboratoryHistory.aspx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.LaboratoryHistory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhMPFrame" runat="server">
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
                    <th align="center"><%=GetLabel("Ref Range")%></th>
                    <th align="center"><%=GetLabel("1")%></th>
                    <th align="center"><%=GetLabel("2")%></th>
                    <th align="center"><%=GetLabel("3")%></th>
                    <th align="center"><%=GetLabel("4")%></th>
                    <th align="center"><%=GetLabel("5")%></th>
                </tr>      
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td align="left"><%#:Eval("FractionName") %></td>
                <td align="left"><%#:Eval("Ref_Range") %></td>
                <td align="left"><%#:Eval("Unit") %></td>
                <td align="center"></td>
                <td align="center"></td>
                <td align="center"></td>
                <td align="center"></td>
                <%--<td align="center"><input type="text" class="number" readonly="readonly" value='<%#:Eval("LatestValue") %>' /></td>--%>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>
</asp:Content>
