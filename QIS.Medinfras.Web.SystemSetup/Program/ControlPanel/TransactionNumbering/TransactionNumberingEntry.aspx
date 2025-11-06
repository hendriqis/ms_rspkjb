<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPEntry.master" AutoEventWireup="true" 
CodeBehind="TransactionNumberingEntry.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.TransactionNumberingEntry" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
    
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle"><%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnID" runat="server" value="" />
    <%--<div class="pageTitle"><%=GetLabel("Transaction Numbering")%></div>--%>
    <table class="tblContentArea">
        <colgroup>
            <col style="width:50%"/>
        </colgroup>
        <tr>
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent">
                    <colgroup>
                        <col style="width:20%"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Transaction Code")%></label></td>
                        <td><asp:TextBox ID="txtTransactionCode" Width="100px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel" class="lblMandatory"><label><%=GetLabel("Transaction Name")%></label></td>
                        <td><asp:TextBox ID="txtTransactionName" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Transaction Initial")%></label></td>
                        <td><asp:TextBox ID="txtTransactionInitial" Width="80px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label><%=GetLabel("Numbering Method")%></label></td>
                        <td><dxe:ASPxComboBox ID="cboGCNumberingMethod" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Counter Digit")%></label></td>
                        <td><asp:TextBox ID="txtCounterDigit" Width="50px" runat="server" CssClass="number"/></td>
                    </tr>
                    
                </table>
            </td>
            <td valign="top">
                <table class="tblEntryContent">
                    <colgroup>
                        <col style="width:0%"/>
                    </colgroup>
                    <tr>
                        <td></td>
                        <td><asp:CheckBox ID="chkByDepartment" runat="server" Text="By Facility" /></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td><asp:CheckBox ID="chkByNeedApproval" runat="server" Text="Need Approval" /></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td><asp:CheckBox ID="chkInventoryTransaction" runat="server" Text="Inventory Transaction" /></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>