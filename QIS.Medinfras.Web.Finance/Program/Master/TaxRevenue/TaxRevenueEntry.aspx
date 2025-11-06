<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPEntry.master" AutoEventWireup="true" 
CodeBehind="TaxRevenueEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.TaxRevenueEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle"><%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnID" runat="server" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width:50%"/>
        </colgroup>
        <tr>
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:50%">
                    <colgroup>
                        <col style="width:25%"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Kode Pajak")%></label></td>
                        <td><asp:TextBox ID="txtTaxRevenueCode" Width="100px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Nama Pajak")%></label></td>
                        <td><asp:TextBox ID="txtTaxRevenueName" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td><asp:CheckBox ID="chkIsTaxRevenueInPercentage" Width="300px" runat="server" Text="dalam Persentase" /></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
