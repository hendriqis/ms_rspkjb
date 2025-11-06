<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPEntry.master" AutoEventWireup="true" 
    CodeBehind="InstructionEntry.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.InstructionEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <input type="hidden" id="hdnID" runat="server" value="" />
    <div class="pageTitle"><%=GetLabel("Instruction")%></div>
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
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Instruction Code")%></label></td>
                        <td><asp:TextBox ID="txtInstructionCode" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Instructio nGroup")%></label></td>
                        <td><dxe:ASPxComboBox runat="server" ID="cboGCInstructionGroup" Width="300px" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Instructio nDescription 1")%></label></td>
                        <td><asp:TextBox ID="txtInstructionDescription1" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label><%=GetLabel("Instruction Description 2")%></label></td>
                        <td><asp:TextBox ID="txtInstructionDescription2" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Display Order")%></label></td>
                        <td><asp:TextBox ID="txtDisplayOrder" CssClass="number required" Width="50px" runat="server" /></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
