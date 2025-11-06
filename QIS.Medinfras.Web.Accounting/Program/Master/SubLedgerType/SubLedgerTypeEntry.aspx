<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPEntry.master" AutoEventWireup="true" 
CodeBehind="SubLedgerTypeEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Accounting.Program.SubLedgerTypeEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        
    </script>
    <input type="hidden" id="hdnID" runat="server" value="" />
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width:50%"/>
        </colgroup>
        <tr>
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:50%">
                    <colgroup>
                        <col style="width:180px"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("SubLedgerType Code")%></label></td>
                        <td><asp:TextBox ID="txtSubLedgerTypeCode" Width="120px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("SubLedgerType Name")%></label></td>
                        <td><asp:TextBox ID="txtSubLedgerTypeName" Width="220px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Method Name")%></label></td>
                        <td><asp:TextBox ID="txtMethodName" Width="220px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("FilterExpression")%></label></td>
                        <td><asp:TextBox ID="txtFilterExpression" Width="220px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("ID FieldName")%></label></td>
                        <td><asp:TextBox ID="txtIDFieldName" Width="220px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Code FieldName")%></label></td>
                        <td><asp:TextBox ID="txtCodeFieldName" Width="220px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Display FieldName")%></label></td>
                        <td><asp:TextBox ID="txtDisplayFieldName" Width="220px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Search Dialog Type Name")%></label></td>
                        <td><asp:TextBox ID="txtSearchDialogTypeName" Width="220px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Table Name")%></label></td>
                        <td><asp:TextBox ID="txtTableName" Width="220px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top"><label class="lblNormal"><%=GetLabel("Remarks")%></label></td>
                        <td><asp:TextBox ID="txtRemarks" Width="220px" runat="server" TextMode="MultiLine" /></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
