<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPEntry.master" AutoEventWireup="true" 
    CodeBehind="DTDEntry.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.DTDEntry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <input type="hidden" id="hdnID" runat="server" value="" />
    <div class="pageTitle"><%=GetLabel("DTD")%></div>
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
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("DTD Code")%></label></td>
                        <td><asp:TextBox ID="txtDTDCode" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("DTD Name")%></label></td>
                        <td><asp:TextBox ID="txtDTDName" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("DTD Label")%></label></td>
                        <td><asp:TextBox ID="txtDTDLabel" Width="300px" runat="server" /></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
