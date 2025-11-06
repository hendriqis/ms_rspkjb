<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPEntry.master"
    AutoEventWireup="true" CodeBehind="TemplateInstructionEntry.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.TemplateInstructionEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <input type="hidden" id="hdnID" runat="server" value="" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <div class="pageTitle">
        <%=GetLabel("Template Instruksi dan Rencana Tindak Lanjut")%></div>
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 50%">
                    <colgroup>
                        <col style="width: 30%" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Kode Instruksi")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtInstructionCode" Width="100px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Kelompok Instruksi")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboInstructionGroup" Width="300px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Deskripsi 1")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtInstructionDesc1" Width="300px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Deskripsi 2")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtInstructionDesc2" Width="450px" TextMode="MultiLine" Rows="15" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Urutan Tampilan")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDisplayOrder" Width="100px" runat="server" CssClass="number" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
