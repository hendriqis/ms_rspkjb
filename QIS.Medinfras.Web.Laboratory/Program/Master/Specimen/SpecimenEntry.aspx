<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPEntry.master" AutoEventWireup="true" 
    CodeBehind="SpecimenEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Laboratory.Program.SpecimenEntry" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
    
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle"><%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnID" runat="server" value="" />
    <%--<div class="pageTitle"><%=GetLabel("Specimen")%></div>--%>
    <table class="tblContentArea">
        <colgroup>
            <col style="width:50%"/>
        </colgroup>
        <tr>
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:50%">
                    <colgroup>
                        <col style="width:20%"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Kode Spesimen")%></label></td>
                        <td><asp:TextBox ID="txtSpecimenCode" Width="100px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Nama Spesimen")%></label></td>
                        <td><asp:TextBox ID="txtSpecimenName" Width="100%" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Jenis Tabung/Container")%></label></td>
                        <td>
                            <dxe:ASPxComboBox runat="server" ID="cboContainerType" ClientInstanceName="cboContainerType"
                                Width="200px">
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
