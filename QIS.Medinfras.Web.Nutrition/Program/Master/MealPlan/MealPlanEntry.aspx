<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPEntry.master"
    AutoEventWireup="true" CodeBehind="MealPlanEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Nutrition.Program.MealPlanEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnID" runat="server" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 50%" />
            <col style="width: auto" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: auto" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Kode Panel Menu Makanan")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMealPlanCode" Width="100px" runat="server" ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Nama Panel Menu Makanan")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMealPlanName" Width="300px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top" style="padding-top: 5px;">
                            <label>
                                <%=GetLabel("Jenis Diet") %></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboDietType" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top" style="padding-top: 5px;">
                            <label>
                                <%=GetLabel("Bentuk Makanan") %></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboMealForm" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top" style="padding-top: 5px;">
                            <label>
                                <%=GetLabel("Kategori Panel Menu") %></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboMealPlanCategory" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="vertical-align: top">
                            <label class="lblNormal">
                                <%=GetLabel("Catatan")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRemark" Width="100%" runat="server" Height="100px" TextMode="MultiLine" />
                        </td>
                    </tr>
                </table>
            </td>
            <td style="padding: 5px; vertical-align: top">
                <div>
                    <asp:CheckBox ID="chkIsStapleFood" runat="server" />
                    <%=GetLabel("Makanan Pokok")%></div>
            </td>
        </tr>
    </table>
</asp:Content>
