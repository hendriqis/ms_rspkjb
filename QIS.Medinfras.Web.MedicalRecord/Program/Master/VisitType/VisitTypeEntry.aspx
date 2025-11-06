<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPEntry.master" AutoEventWireup="true" 
CodeBehind="VisitTypeEntry.aspx.cs" Inherits="QIS.Medinfras.Web.MedicalRecord.Program.VisitTypeEntry" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle"><%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnID" runat="server" value="" />
    <input type="hidden" id="hdnIsBridgingToMedinfrasMobileApps" runat="server" value="" />
    <%--<div class="pageTitle"><%=GetLabel("Jenis Kunjungan")%></div>--%>
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
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Kode Jenis Kunjungan")%></label></td>
                        <td><asp:TextBox ID="txtVisitTypeCode" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Nama Jenis Kunjungan")%></label></td>
                        <td><asp:TextBox ID="txtVisitTypeName" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Pendaftaran")%></label></td>
                        <td><dxe:ASPxComboBox ID="cboAdmission" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel">&nbsp;</td>
                        <td><asp:CheckBox ID="chkIsSick" runat="server" /><%=GetLabel("Diagnosa Pasien Harus Diisi")%></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>