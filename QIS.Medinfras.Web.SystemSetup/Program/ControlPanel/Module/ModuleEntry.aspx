<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPEntry.master" AutoEventWireup="true" 
    CodeBehind="ModuleEntry.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.ModuleEntry" %>
    
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle"><%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnID" runat="server" value="" />
    <%--<div class="pageTitle"><%=GetLabel("Module")%></div>--%>
    <table class="tblContentArea">
        <colgroup>
            <col style="width:50%"/>
            <col style="width:50%"/>
        </colgroup>
        <tr>
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:100%">
                    <colgroup>
                        <col style="width:20%"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Module Code")%></label></td>
                        <td><asp:TextBox ID="txtModuleCode" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Module Name")%></label></td>
                        <td><asp:TextBox ID="txtModuleName" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Short Name")%></label></td>
                        <td><asp:TextBox ID="txtShortName" Width="150px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Module Index")%></label></td>
                        <td><asp:TextBox ID="txtModuleIndex" CssClass="required number" Width="150px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="vertical-align: top; padding-top: 5px;"><label class="lblNormal"><%=GetLabel("Description")%></label></td>
                        <td><asp:TextBox ID="txtDescription" Width="100%" runat="server" TextMode="MultiLine" Rows="2" /></td>
                    </tr>
                </table>
            </td>
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:100%">
                    <colgroup>
                        <col style="width:25%"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Image Url")%></label></td>
                        <td><asp:TextBox ID="txtImageUrl" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Disabled Image Url")%></label></td>
                        <td><asp:TextBox ID="txtDisabledImageUrl" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Default Url")%></label></td>
                        <td><asp:TextBox ID="txtDefaultUrl" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td><asp:CheckBox ID="chkIsVisible" runat="server" /><%=GetLabel("Visible")%></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
