<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPEntry.master"
    AutoEventWireup="true" CodeBehind="TaxRevenueSettingEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.TaxRevenueSettingEntry" %>

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
                        <col style="width: 25%" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Kode Pengaturan")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTaxRevenueSettingCode" Width="100px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Nama Pengaturan")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTaxRevenueSettingName" Width="300px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Formula Berdasarkan")%></label>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rblFormulaType" runat="server" RepeatDirection="Horizontal"
                                Width="200px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Catatan")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtNotes" Width="300px" runat="server" TextMode="MultiLine" Rows="2" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
