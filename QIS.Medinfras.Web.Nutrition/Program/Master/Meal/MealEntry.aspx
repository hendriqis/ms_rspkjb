<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPEntry.master"
    AutoEventWireup="true" CodeBehind="MealEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Nutrition.Program.MealEntry" %>

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
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 50%">
                    <colgroup>
                        <col style="width: 20%" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Kode Menu")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMealCode" Width="100px" runat="server" ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Nama Menu")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMealName" Width="300px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Jumlah Porsi")%></label>
                        </td>
                        <td colspan="5">
                            <asp:TextBox ID="txtStandardPortion" CssClass="number required" runat="server" Width="60px" />
                            <%=GetLabel("porsi")%>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="vertical-align: top;">
                            <label class="lblNormal">
                                <%=GetLabel("Catatan Penyajian")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtServingLabel" Width="600px" runat="server" TextMode="MultiLine"
                                Rows="3" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                            <label class="lblNormal">
                                <%=GetLabel("Catatan Pembuatan")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRemark" Width="600px" TextMode="MultiLine" Rows="10" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
