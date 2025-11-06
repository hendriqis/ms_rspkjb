<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPEntry.master"
    AutoEventWireup="true" CodeBehind="TemplatePrescriptionEntry.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.TemplatePrescriptionEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnID" runat="server" value="" />
    <input type="hidden" id="hdnTemplateType" runat="server" value="" />
    <input type="hidden" id="hdnCurrentSessionID" runat="server" value="" />
    <%--<div class="pageTitle"><%=GetPageTitle()%></div>--%>
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table style="width: 100%">
                    <colgroup>
                        <col style="width: 10%" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Kode Template")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTemplateCode" Width="100px" CssClass="required" runat="server"
                                MaxLength="8" /><label class="lblNormal">
                                    <i>
                                        <%=GetLabel("maksimal 8 karakter")%></i></label>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="vertical-align: top">
                            <label class="lblMandatory">
                                <%=GetLabel("Nama Template")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTemplateName" Width="300px" CssClass="required" runat="server"
                                TextMode="Multiline" Rows="2" MaxLength="100" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
