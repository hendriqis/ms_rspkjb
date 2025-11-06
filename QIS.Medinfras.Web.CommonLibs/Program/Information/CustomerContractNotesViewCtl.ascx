<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomerContractNotesViewCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.CustomerContractNotesViewCtl" %>
<asp:Repeater ID="rptView" runat="server">
    <HeaderTemplate>
        <table class="grdNormal" style="font-size: 0.9em" cellpadding="0" cellspacing="0"
            width="350px">
            <tr>
                <th align="center" style="width: 50px">
                    <%=GetLabel("Tanggal")%>
                </th>
                <th align="center" style="width: 50px">
                    <%=GetLabel("Catatan")%>
                </th>
            </tr>
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td align="left" style="width: 25%">
                <%#: Eval("LogDateInString")%>
            </td>
            <td align="left" style="max-width: 100px; word-wrap: break-word">
                <%#: Eval("LogRemarks")%>
            </td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </table>
    </FooterTemplate>
</asp:Repeater>
