<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPEntry.master" AutoEventWireup="true" 
CodeBehind="MarginMarkupEntry.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.MarginMarkupEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <input type="hidden" id="hdnID" runat="server" value="" />
    <div class="pageTitle"><%=GetLabel("Margin Markup")%></div>
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
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Markup Code")%></label></td>
                        <td><asp:TextBox ID="txtMarkupCode" Width="100px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Markup Name")%></label></td>
                        <td><asp:TextBox ID="txtMarkupName" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td><asp:CheckBox ID="chkIsMarkupInPercentage" Width="300px" runat="server" Text="Is Markup In Percentage" /></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
