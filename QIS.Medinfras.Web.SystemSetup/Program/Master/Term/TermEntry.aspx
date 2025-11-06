<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPEntry.master" AutoEventWireup="true" 
    CodeBehind="TermEntry.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.TermEntry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <input type="hidden" id="hdnID" runat="server" value="" />
    <div class="pageTitle"><%=GetLabel("Term")%></div>
    <table class="tblContentArea">
        <colgroup>
            <col style="width:50%"/>
        </colgroup>
        <tr>
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:50%">
                    <colgroup>
                        <col style="width:30%"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Term Code")%></label></td>
                        <td><asp:TextBox ID="txtTermCode" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Term Name")%></label></td>
                        <td><asp:TextBox ID="txtTermName" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Term Day")%></label></td>
                        <td><asp:TextBox ID="txtTermDay" Width="100px" runat="server" CssClass="number required" /></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
