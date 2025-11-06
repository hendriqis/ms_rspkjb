<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPEntry.master" AutoEventWireup="true" 
    CodeBehind="FractionGroupEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Laboratory.Program.FractionGroupEntry" %>
    
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle"><%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnID" runat="server" value="" />
    <%--<div class="pageTitle"><%=GetLabel("Kelompok Artikel")%></div>--%>
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
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Kode Kelompok Artikel")%></label></td>
                        <td><asp:TextBox ID="txtFractionGroupCode" Width="100px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Nama Kelompok Artikel")%></label></td>
                        <td><asp:TextBox ID="txtFractionGroupName" Width="100%" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Urutan Tampil")%></label></td>
                        <td><asp:TextBox ID="txtDisplayOrder" Width="10%" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label><%=GetLabel("Catatan")%></label></td>
                        <td><asp:TextBox ID="txtRemarks" Width="100%" runat="server" TextMode="MultiLine" Rows="2" /></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
