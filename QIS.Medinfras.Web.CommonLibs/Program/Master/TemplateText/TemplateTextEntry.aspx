<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPEntry.master" AutoEventWireup="true" 
CodeBehind="TemplateTextEntry.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.TemplateTextEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle"><%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnID" runat="server" value="" />
    <input type="hidden" id="hdnTemplateType" runat="server" value="" />
    <%--<div class="pageTitle"><%=GetPageTitle()%></div>--%>
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
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Kode Template")%></label></td>
                        <td><asp:TextBox ID="txtTemplateCode" Width="100px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Nama Template")%></label></td>
                        <td><asp:TextBox ID="txtTemplateName" Width="300px" runat="server" /></td>
                    </tr>
                    <tr id="trTemplateGroup" runat="server">
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Kelompok Template")%></label></td>
                        <td><dxe:ASPxComboBox ID="cboTemplateGroup" Width="300px" runat="server"  /></td>
                    </tr>
                </table>
<%--                <%=GetLabel("Template Teks") %><br /> <br />--%>
                <asp:TextBox TextMode="MultiLine" Width="100%" Height="300px" ID="txtTemplateContent" runat="server" CssClass="htmlEditor" />
            </td>
        </tr>
    </table>
</asp:Content>
