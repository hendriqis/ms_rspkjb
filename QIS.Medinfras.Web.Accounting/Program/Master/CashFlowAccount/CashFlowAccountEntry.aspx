<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/libs/MasterPage/MPEntry.master"
    CodeBehind="CashFlowAccountEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Accounting.Program.CashFlowAccountEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
    </script>
    <input type="hidden" id="hdnGLCashFlowAccountID" runat="server" value="" />
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 50%">
                    <colgroup>
                        <col style="width: 150px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Content Code")%></label></td>
                        <td><asp:TextBox ID="txtContentCode" Width="150px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Content Name")%></label></td>
                        <td><asp:TextBox ID="txtContentName" Width="400px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Content Name 2")%></label></td>
                        <td><asp:TextBox ID="txtContentName2" Width="400px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Content Group")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboGCContentGroup" ClientInstanceName="cboGCContentGroup" Width="150px"
                                runat="server">
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Content Operator")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboContentOperator" ClientInstanceName="cboContentOperator" Width="150px"
                                runat="server">
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
